[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$InputPath,

    [Parameter(Mandatory = $true)]
    [string]$OutputPath,

    [string]$KeyColor = '#FF00FF',

    [ValidateRange(0, 255)]
    [int]$StrongTolerance = 24,

    [ValidateRange(0, 255)]
    [int]$FloodTolerance = 48,

    [ValidateRange(0, 255)]
    [int]$DespillTolerance = 96,

    [switch]$MaskOnly
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$temporaryOutputPath = $null

try {
    if (-not (Test-Path -LiteralPath $InputPath -PathType Leaf)) { throw "Input not found: $InputPath" }
    if ($KeyColor -cnotmatch '^#[0-9A-Fa-f]{6}$') { throw 'KeyColor must be #RRGGBB.' }

    $resolvedInputPath = (Resolve-Path -LiteralPath $InputPath).Path
    $resolvedOutputPath = [IO.Path]::GetFullPath($OutputPath)
    if ($resolvedInputPath -ieq $resolvedOutputPath) { throw 'Output path conflicts with the input path.' }
    $outputDirectory = [IO.Path]::GetDirectoryName($resolvedOutputPath)
    if ([string]::IsNullOrWhiteSpace($outputDirectory) -or -not (Test-Path -LiteralPath $outputDirectory -PathType Container)) {
        throw "Output directory not found: $outputDirectory"
    }
    if (Test-Path -LiteralPath $resolvedOutputPath) { throw 'Output already exists.' }

    $sourceShaBefore = (Get-FileHash -Algorithm SHA256 -LiteralPath $resolvedInputPath).Hash.ToUpperInvariant()
    $sourceLengthBefore = (Get-Item -LiteralPath $resolvedInputPath).Length
    $keyRed = [Convert]::ToInt32($KeyColor.Substring(1, 2), 16)
    $keyGreen = [Convert]::ToInt32($KeyColor.Substring(3, 2), 16)
    $keyBlue = [Convert]::ToInt32($KeyColor.Substring(5, 2), 16)

    Add-Type -AssemblyName System.Drawing
    if (-not ('A01.ForegroundRecovery.Engine' -as [type])) {
        Add-Type -TypeDefinition @'
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace A01.ForegroundRecovery
{
    public static class Engine
    {
        private static int KeyDistance(int argb, int keyRed, int keyGreen, int keyBlue)
        {
            var red = (argb >> 16) & 255;
            var green = (argb >> 8) & 255;
            var blue = argb & 255;
            return Math.Max(Math.Max(Math.Abs(red - keyRed), Math.Abs(green - keyGreen)), Math.Abs(blue - keyBlue));
        }

        private static bool TouchesMask(bool[] mask, int width, int height, int x, int y)
        {
            for (var offsetY = -1; offsetY <= 1; offsetY++)
            for (var offsetX = -1; offsetX <= 1; offsetX++)
            {
                if (offsetX == 0 && offsetY == 0) continue;
                var nextX = x + offsetX;
                var nextY = y + offsetY;
                if (nextX >= 0 && nextX < width && nextY >= 0 && nextY < height && mask[nextY * width + nextX])
                    return true;
            }
            return false;
        }

        private static bool HasNonlinearFringeRgb(int argb)
        {
            var r = (argb >> 16) & 255;
            var g = (argb >> 8) & 255;
            var b = argb & 255;
            return Math.Max(r, b) >= 128 && r - g >= 40 && b - g >= 40 && Math.Abs(r - b) <= 96;
        }

        private static bool HasResidualPinkFringeRgb(int argb)
        {
            var r = (argb >> 16) & 255;
            var g = (argb >> 8) & 255;
            var b = argb & 255;
            return r >= 172 && r - g >= 120 && b - g >= 40 && r - b >= 96 && b >= 64;
        }

        private static bool IsVisualFringeCandidate(int[] pixels, int width, int height, int index)
        {
            if (((pixels[index] >> 24) & 255) != 255) return false;
            var x = index % width;
            var y = index / width;
            var touchesTransparent = false;
            for (var offsetY = -1; offsetY <= 1; offsetY++)
            for (var offsetX = -1; offsetX <= 1; offsetX++)
            {
                if (offsetX == 0 && offsetY == 0) continue;
                var nextX = x + offsetX;
                var nextY = y + offsetY;
                if (nextX >= 0 && nextX < width && nextY >= 0 && nextY < height &&
                    ((pixels[nextY * width + nextX] >> 24) & 255) == 0)
                    touchesTransparent = true;
            }
            return touchesTransparent && (HasNonlinearFringeRgb(pixels[index]) || HasResidualPinkFringeRgb(pixels[index]));
        }

        private static int CleanVisualFringes(int[] pixels, int width, int height)
        {
            const int visualDonorDistanceSquared = 81;
            var candidates = new bool[pixels.Length];
            for (var index = 0; index < pixels.Length; index++)
                candidates[index] = IsVisualFringeCandidate(pixels, width, height, index);

            var replacementRgb = new int[pixels.Length];
            var replaced = 0;
            for (var index = 0; index < pixels.Length; index++)
            {
                if (!candidates[index]) continue;
                var x = index % width;
                var y = index / width;
                var donor = -1;
                var bestDistanceSquared = Int32.MaxValue;
                var bestY = Int32.MaxValue;
                var bestX = Int32.MaxValue;
                for (var offsetY = -8; offsetY <= 8; offsetY++)
                for (var offsetX = -8; offsetX <= 8; offsetX++)
                {
                    var distanceSquared = offsetX * offsetX + offsetY * offsetY;
                    if (distanceSquared == 0 || distanceSquared > visualDonorDistanceSquared) continue;
                    var nextX = x + offsetX;
                    var nextY = y + offsetY;
                    if (nextX < 0 || nextX >= width || nextY < 0 || nextY >= height) continue;
                    var next = nextY * width + nextX;
                    if (((pixels[next] >> 24) & 255) != 255 || candidates[next] ||
                        HasNonlinearFringeRgb(pixels[next]) || HasResidualPinkFringeRgb(pixels[next])) continue;
                    if (distanceSquared < bestDistanceSquared ||
                        (distanceSquared == bestDistanceSquared && (nextY < bestY || (nextY == bestY && nextX < bestX))))
                    {
                        donor = next;
                        bestDistanceSquared = distanceSquared;
                        bestY = nextY;
                        bestX = nextX;
                    }
                }
                if (donor < 0) throw new InvalidOperationException("Visual fringe candidate has no deterministic donor.");
                replacementRgb[index] = pixels[donor] & 0x00FFFFFF;
                replaced++;
            }
            for (var index = 0; index < pixels.Length; index++)
                if (candidates[index]) pixels[index] = (pixels[index] & unchecked((int)0xFF000000)) | replacementRgb[index];
            return replaced;
        }

        public static string Process(
            string inputPath, string outputPath, int keyRed, int keyGreen, int keyBlue,
            int strongTolerance, int floodTolerance, int despillTolerance, bool maskOnly)
        {
            using (var source = new Bitmap(inputPath))
            {
                if (source.Width != 1672 || source.Height != 941)
                    throw new InvalidOperationException("Unexpected foreground dimensions.");

                var width = source.Width;
                var height = source.Height;
                var count = width * height;
                var sourcePixels = new int[count];
                var mask = new bool[count];
                var queue = new int[count];
                var head = 0;
                var tail = 0;
                for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    var index = y * width + x;
                    sourcePixels[index] = source.GetPixel(x, y).ToArgb();
                    if (KeyDistance(sourcePixels[index], keyRed, keyGreen, keyBlue) <= strongTolerance)
                    {
                        mask[index] = true;
                        queue[tail++] = index;
                    }
                }

                var cardinalX = new[] { -1, 1, 0, 0 };
                var cardinalY = new[] { 0, 0, -1, 1 };
                while (head < tail)
                {
                    var index = queue[head++];
                    var x = index % width;
                    var y = index / width;
                    for (var direction = 0; direction < 4; direction++)
                    {
                        var nextX = x + cardinalX[direction];
                        var nextY = y + cardinalY[direction];
                        if (nextX < 0 || nextX >= width || nextY < 0 || nextY >= height) continue;
                        var next = nextY * width + nextX;
                        if (mask[next] || KeyDistance(sourcePixels[next], keyRed, keyGreen, keyBlue) > floodTolerance) continue;
                        mask[next] = true;
                        queue[tail++] = next;
                    }
                }

                var outputPixels = new int[count];
                var despilled = 0;
                var visualDespelled = 0;
                if (maskOnly)
                {
                    for (var index = 0; index < count; index++)
                        outputPixels[index] = mask[index] ? 0 : unchecked((int)0xFFFFFFFF);
                }
                else
                {
                    Array.Copy(sourcePixels, outputPixels, count);
                    for (var y = 0; y < height; y++)
                    for (var x = 0; x < width; x++)
                    {
                        var index = y * width + x;
                        if (mask[index])
                        {
                            outputPixels[index] = 0;
                            continue;
                        }
                        if (!TouchesMask(mask, width, height, x, y) ||
                            KeyDistance(sourcePixels[index], keyRed, keyGreen, keyBlue) > despillTolerance)
                        {
                            outputPixels[index] = unchecked((int)0xFF000000) | (sourcePixels[index] & 0x00FFFFFF);
                            continue;
                        }

                        var donor = -1;
                        var bestDistanceSquared = Int32.MaxValue;
                        var bestY = Int32.MaxValue;
                        var bestX = Int32.MaxValue;
                        for (var offsetY = -8; offsetY <= 8; offsetY++)
                        for (var offsetX = -8; offsetX <= 8; offsetX++)
                        {
                            var distanceSquared = offsetX * offsetX + offsetY * offsetY;
                            if (distanceSquared == 0 || distanceSquared > 64) continue;
                            var nextX = x + offsetX;
                            var nextY = y + offsetY;
                            if (nextX < 0 || nextX >= width || nextY < 0 || nextY >= height) continue;
                            var next = nextY * width + nextX;
                            if (mask[next] || KeyDistance(sourcePixels[next], keyRed, keyGreen, keyBlue) <= despillTolerance) continue;
                            if (distanceSquared < bestDistanceSquared ||
                                (distanceSquared == bestDistanceSquared && (nextY < bestY || (nextY == bestY && nextX < bestX))))
                            {
                                donor = next;
                                bestDistanceSquared = distanceSquared;
                                bestY = nextY;
                                bestX = nextX;
                            }
                        }
                        if (donor < 0) throw new InvalidOperationException("Qualifying edge blend has no deterministic donor.");
                        outputPixels[index] = unchecked((int)0xFF000000) | (sourcePixels[donor] & 0x00FFFFFF);
                        despilled++;
                    }
                    visualDespelled = CleanVisualFringes(outputPixels, width, height);
                }

                using (var output = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                {
                    for (var y = 0; y < height; y++)
                    for (var x = 0; x < width; x++)
                        output.SetPixel(x, y, Color.FromArgb(outputPixels[y * width + x]));
                    output.Save(outputPath, ImageFormat.Png);
                }

                var strongKeyOpaque = 0;
                var unresolvedBlends = 0;
                var visualFringeCandidates = 0;
                for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    var index = y * width + x;
                    if (mask[index]) continue;
                    if (KeyDistance(outputPixels[index], keyRed, keyGreen, keyBlue) <= strongTolerance) strongKeyOpaque++;
                    if (TouchesMask(mask, width, height, x, y) &&
                        KeyDistance(outputPixels[index], keyRed, keyGreen, keyBlue) <= despillTolerance) unresolvedBlends++;
                    if (IsVisualFringeCandidate(outputPixels, width, height, index)) visualFringeCandidates++;
                }
                if (strongKeyOpaque != 0) throw new InvalidOperationException("Output retains opaque strong-key pixels.");
                if (unresolvedBlends != 0) throw new InvalidOperationException("Output contains unresolved qualifying blends.");
                if (visualFringeCandidates != 0) throw new InvalidOperationException("Output retains visual fringe candidates.");
                return maskOnly
                    ? String.Format("MASK_STATS strong-key-opaque={0}; unresolved-qualifying-blends={1}; visual-fringe-candidates={2}", strongKeyOpaque, unresolvedBlends, visualFringeCandidates)
                    : String.Format("RECOVERY_STATS strong-key-opaque={0}; unresolved-qualifying-blends={1}; despilled={2}; visual-fringe-candidates={3}; visual-despilled={4}", strongKeyOpaque, unresolvedBlends, despilled, visualFringeCandidates, visualDespelled);
            }
        }
    }
}
'@ -ReferencedAssemblies @('System.Drawing.Common', 'System.Drawing.Primitives', 'System.Private.Windows.GdiPlus', 'System.Private.Windows.Core')
    }

    $temporaryOutputPath = Join-Path $outputDirectory ('.' + [IO.Path]::GetFileName($resolvedOutputPath) + '.' + [Guid]::NewGuid().ToString('N') + '.tmp.png')
    $metrics = [A01.ForegroundRecovery.Engine]::Process(
        $resolvedInputPath, $temporaryOutputPath,
        $keyRed, $keyGreen, $keyBlue,
        $StrongTolerance, $FloodTolerance, $DespillTolerance, $MaskOnly.IsPresent)

    $sourceShaAfter = (Get-FileHash -Algorithm SHA256 -LiteralPath $resolvedInputPath).Hash.ToUpperInvariant()
    $sourceLengthAfter = (Get-Item -LiteralPath $resolvedInputPath).Length
    if ($sourceShaAfter -cne $sourceShaBefore -or $sourceLengthAfter -ne $sourceLengthBefore) {
        throw 'Source immutability violation.'
    }

    [IO.File]::Move($temporaryOutputPath, $resolvedOutputPath)
    $temporaryOutputPath = $null
    $outputSha = (Get-FileHash -Algorithm SHA256 -LiteralPath $resolvedOutputPath).Hash.ToUpperInvariant()
    Write-Output "PASS: Remove-ConnectedChromaMatte; $metrics; candidate-sha256: $outputSha"
}
catch {
    [Console]::Error.WriteLine($_.Exception.Message)
    exit 1
}
finally {
    if ($null -ne $temporaryOutputPath -and (Test-Path -LiteralPath $temporaryOutputPath)) {
        Remove-Item -LiteralPath $temporaryOutputPath -Force
    }
}
