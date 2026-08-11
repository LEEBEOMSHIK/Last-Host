[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$ToolPath,

    [Parameter(Mandatory = $true)]
    [string]$RealSourcePath,

    [Parameter(Mandatory = $true)]
    [ValidatePattern('^[0-9A-F]{64}$')]
    [string]$ExpectedSourceSha,

    [ValidatePattern('^[0-9A-F]{64}$')]
    [string]$ExpectedOutputSha,

    [ValidateSet('Real', 'MaskOnly')]
    [string]$TargetedCase = 'Real'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# This must remain the first filesystem precondition so RED is stable and does
# not depend on image fixtures or the local System.Drawing runtime.
if (-not (Test-Path -LiteralPath $ToolPath -PathType Leaf)) {
    [Console]::Error.WriteLine('Tool not found')
    exit 1
}

Add-Type -AssemblyName System.Drawing

if (-not ('A01.ForegroundRecoveryTests.ImageOracle' -as [type])) {
    Add-Type -TypeDefinition @'
using System;
using System.Drawing;

namespace A01.ForegroundRecoveryTests
{
    public static class ImageOracle
    {
        private static int DistanceToMagenta(Color color)
        {
            return Math.Max(Math.Max(Math.Abs(color.R - 255), Math.Abs(color.G)), Math.Abs(color.B - 255));
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

        private static bool IsVisualFringeCandidate(int[] pixels, byte[] alpha, int width, int height, int index)
        {
            if (alpha[index] != 255) return false;
            var x = index % width;
            var y = index / width;
            var touchesTransparent = false;
            for (var oy = -1; oy <= 1; oy++)
            for (var ox = -1; ox <= 1; ox++)
            {
                if (ox == 0 && oy == 0) continue;
                var nx = x + ox;
                var ny = y + oy;
                if (nx >= 0 && nx < width && ny >= 0 && ny < height && alpha[ny * width + nx] == 0)
                    touchesTransparent = true;
            }
            return touchesTransparent && (HasNonlinearFringeRgb(pixels[index]) || HasResidualPinkFringeRgb(pixels[index]));
        }

        private static void AssertVisualFringePredicateFixtures()
        {
            var alpha = new byte[9];
            var pixels = new int[9];
            alpha[4] = 255;
            pixels[4] = Color.FromArgb(255, 190, 60, 160).ToArgb();
            if (!IsVisualFringeCandidate(pixels, alpha, 3, 3, 4))
                throw new InvalidOperationException("Synthetic nonlinear fringe predicate fixture was not detected.");
            pixels[4] = Color.FromArgb(255, 234, 23, 137).ToArgb();
            if (!IsVisualFringeCandidate(pixels, alpha, 3, 3, 4))
                throw new InvalidOperationException("Synthetic residual-pink predicate fixture was not detected.");
            pixels[4] = Color.FromArgb(255, 40, 110, 65).ToArgb();
            if (IsVisualFringeCandidate(pixels, alpha, 3, 3, 4))
                throw new InvalidOperationException("Synthetic safe edge fixture was classified as visual fringe.");
        }

        public static string AssertRecoveryContract(string path)
        {
            using (var bitmap = new Bitmap(path))
            {
                if (bitmap.Width != 1672 || bitmap.Height != 941)
                    throw new InvalidOperationException("Unexpected foreground dimensions.");

                var width = bitmap.Width;
                var height = bitmap.Height;
                var alpha = new byte[width * height];
                var pixels = new int[width * height];
                var strongKeyOpaque = 0;
                AssertVisualFringePredicateFixtures();

                for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    var index = y * width + x;
                    alpha[index] = color.A;
                    pixels[index] = color.ToArgb();
                    if (color.A != 0 && color.A != 255)
                        throw new InvalidOperationException("Output alpha is not hard alpha.");
                    if (color.A == 0)
                    {
                        if (color.R != 0 || color.G != 0 || color.B != 0)
                            throw new InvalidOperationException("Transparent output RGB is not black.");
                        continue;
                    }
                    if (DistanceToMagenta(color) <= 24) strongKeyOpaque++;
                }

                var unresolvedBlends = 0;
                var visualFringeCandidates = 0;
                var knownMonitorRegionCandidates = 0;
                for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    var index = y * width + x;
                    if (alpha[index] != 255) continue;
                    var touchesTransparent = false;
                    for (var oy = -1; oy <= 1; oy++)
                    for (var ox = -1; ox <= 1; ox++)
                    {
                        if (ox == 0 && oy == 0) continue;
                        var nx = x + ox;
                        var ny = y + oy;
                        if (nx >= 0 && nx < width && ny >= 0 && ny < height && alpha[ny * width + nx] == 0)
                            touchesTransparent = true;
                    }
                    if (!touchesTransparent) continue;
                    var argb = pixels[index];
                    var r = (argb >> 16) & 255;
                    var g = (argb >> 8) & 255;
                    var b = argb & 255;
                    if (Math.Max(Math.Max(Math.Abs(r - 255), Math.Abs(g)), Math.Abs(b - 255)) <= 96)
                        unresolvedBlends++;
                    if (IsVisualFringeCandidate(pixels, alpha, width, height, index))
                    {
                        visualFringeCandidates++;
                        if (x >= 102 && x <= 366 && y >= 456 && y <= 626)
                            knownMonitorRegionCandidates++;
                    }
                }

                if (strongKeyOpaque != 0)
                    throw new InvalidOperationException("Output retains opaque strong-key pixels: " + strongKeyOpaque);
                if (unresolvedBlends != 0)
                    throw new InvalidOperationException("Output contains unresolved qualifying blends: " + unresolvedBlends);
                if (visualFringeCandidates != 0)
                {
                    if (knownMonitorRegionCandidates == 0)
                        throw new InvalidOperationException("Visual fringe did not cover the known monitor QA region.");
                    throw new InvalidOperationException("Output retains visual fringe candidates: " + visualFringeCandidates + "; known-monitor-region=" + knownMonitorRegionCandidates);
                }
                return String.Format("RECOVERY_STATS strong-key-opaque={0}; unresolved-qualifying-blends={1}; visual-fringe-candidates={2}", strongKeyOpaque, unresolvedBlends, visualFringeCandidates);
            }
        }

        public static string AssertMaskOnlyContract(string path)
        {
            using (var bitmap = new Bitmap(path))
            {
                if (bitmap.Width != 1672 || bitmap.Height != 941)
                    throw new InvalidOperationException("Unexpected mask dimensions.");

                var width = bitmap.Width;
                var height = bitmap.Height;
                var opaque = new bool[width * height];
                var opaqueCount = 0;
                for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    var index = y * width + x;
                    if (color.A != 0 && color.A != 255)
                        throw new InvalidOperationException("Mask alpha is not hard alpha.");
                    if (color.A == 0)
                    {
                        if (color.R != 0 || color.G != 0 || color.B != 0)
                            throw new InvalidOperationException("Transparent mask RGB is not black.");
                        continue;
                    }
                    if (color.R != 255 || color.G != 255 || color.B != 255)
                        throw new InvalidOperationException("Opaque mask RGB is not white.");
                    if (y < 390)
                        throw new InvalidOperationException("Known empty top mask area contains an opaque pixel.");
                    opaque[index] = true;
                    opaqueCount++;
                }

                var coverage = (double)opaqueCount / opaque.Length;
                if (coverage < 0.01 || coverage > 0.80)
                    throw new InvalidOperationException("Mask opaque coverage is outside the sanity range: " + coverage);

                var visited = new bool[opaque.Length];
                var queue = new int[opaque.Length];
                var components = 0;
                var largestComponent = 0;
                for (var index = 0; index < opaque.Length; index++)
                {
                    if (!opaque[index] || visited[index]) continue;
                    components++;
                    var head = 0;
                    var tail = 0;
                    queue[tail++] = index;
                    visited[index] = true;
                    while (head < tail)
                    {
                        var current = queue[head++];
                        var x = current % width;
                        var y = current / width;
                        if (x > 0)
                        {
                            var left = current - 1;
                            if (opaque[left] && !visited[left]) { visited[left] = true; queue[tail++] = left; }
                        }
                        if (x + 1 < width)
                        {
                            var right = current + 1;
                            if (opaque[right] && !visited[right]) { visited[right] = true; queue[tail++] = right; }
                        }
                        if (y > 0)
                        {
                            var up = current - width;
                            if (opaque[up] && !visited[up]) { visited[up] = true; queue[tail++] = up; }
                        }
                        if (y + 1 < height)
                        {
                            var down = current + width;
                            if (opaque[down] && !visited[down]) { visited[down] = true; queue[tail++] = down; }
                        }
                    }
                    if (tail > largestComponent) largestComponent = tail;
                }
                if (components == 0 || components > 512 || largestComponent < opaqueCount / 4)
                    throw new InvalidOperationException("Mask component sanity check failed: components=" + components + "; largest=" + largestComponent + "; opaque=" + opaqueCount);
                return String.Format("MASK_STATS opaque={0}; coverage={1:F6}; components={2}; largest-component={3}", opaqueCount, coverage, components, largestComponent);
            }
        }
    }
}
'@ -ReferencedAssemblies @('System.Drawing.Common', 'System.Drawing.Primitives', 'System.Private.Windows.GdiPlus', 'System.Private.Windows.Core')
}

function Get-UpperSha256 {
    param([string]$Path)
    return (Get-FileHash -Algorithm SHA256 -LiteralPath $Path).Hash.ToUpperInvariant()
}

function Assert-True {
    param([bool]$Condition, [string]$Message)
    if (-not $Condition) { throw $Message }
}

$resolvedToolPath = (Resolve-Path -LiteralPath $ToolPath).Path
$pwshPath = (Get-Command pwsh -ErrorAction Stop).Source
$canonicalMaskOutputSha = 'F59EBC810A943DB76C17691AD364237F473BAB6A97EF3A8966321BAEF8400D95'

function Invoke-RecoveryTool {
    param([string]$OutputPath, [switch]$MaskOnly)
    $arguments = @(
        '-NoLogo', '-NoProfile', '-File', $resolvedToolPath,
        '-InputPath', $RealSourcePath,
        '-OutputPath', $OutputPath,
        '-KeyColor', '#FF00FF',
        '-StrongTolerance', '24',
        '-FloodTolerance', '48',
        '-DespillTolerance', '96'
    )
    if ($MaskOnly) { $arguments += '-MaskOnly' }
    $captured = @(& $pwshPath @arguments 2>&1)
    return [pscustomobject]@{
        ExitCode = $LASTEXITCODE
        Text = (($captured | ForEach-Object { $_.ToString() }) -join "`n")
    }
}

$tempRoot = Join-Path ([IO.Path]::GetTempPath()) ('last-host-a01-foreground-recovery-' + [Guid]::NewGuid().ToString('N'))
try {
    Assert-True (Test-Path -LiteralPath $RealSourcePath -PathType Leaf) 'Real source not found.'
    $sourceShaBefore = Get-UpperSha256 $RealSourcePath
    Assert-True ($sourceShaBefore -ceq $ExpectedSourceSha) 'Real source SHA-256 does not match the independent expected value.'
    New-Item -ItemType Directory -Path $tempRoot -Force | Out-Null

    if ($TargetedCase -eq 'MaskOnly') {
        Assert-True (-not [string]::IsNullOrWhiteSpace($ExpectedOutputSha)) 'MaskOnly requires ExpectedOutputSha.'
        Assert-True ($ExpectedOutputSha -ceq $canonicalMaskOutputSha) 'MaskOnly ExpectedOutputSha must equal the canonical mask SHA.'
        $maskOutputA = Join-Path $tempRoot 'mask-a.png'
        $maskOutputB = Join-Path $tempRoot 'mask-b.png'
        $maskRunA = Invoke-RecoveryTool $maskOutputA -MaskOnly
        Assert-True ($maskRunA.ExitCode -eq 0) "mask-only run A expected exit 0, got $($maskRunA.ExitCode): $($maskRunA.Text)"
        $maskRunB = Invoke-RecoveryTool $maskOutputB -MaskOnly
        Assert-True ($maskRunB.ExitCode -eq 0) "mask-only run B expected exit 0, got $($maskRunB.ExitCode): $($maskRunB.Text)"
        Assert-True ((Get-UpperSha256 $RealSourcePath) -ceq $sourceShaBefore) 'Real source bytes changed.'
        Assert-True ((Get-UpperSha256 $maskOutputA) -ceq (Get-UpperSha256 $maskOutputB)) 'Repeated mask output SHA differs.'
        Assert-True ((Get-UpperSha256 $maskOutputA) -ceq $canonicalMaskOutputSha) 'Mask output A SHA does not match the canonical mask SHA.'
        Assert-True ((Get-UpperSha256 $maskOutputB) -ceq $canonicalMaskOutputSha) 'Mask output B SHA does not match the canonical mask SHA.'
        $metrics = [A01.ForegroundRecoveryTests.ImageOracle]::AssertMaskOnlyContract($maskOutputA)
        Write-Output "PASS: Remove-ConnectedChromaMatte mask-only contract; $metrics; candidate-sha256: $(Get-UpperSha256 $maskOutputA)"
        return
    }

    $outputA = Join-Path $tempRoot 'foreground-a.png'
    $outputB = Join-Path $tempRoot 'foreground-b.png'
    $runA = Invoke-RecoveryTool $outputA
    Assert-True ($runA.ExitCode -eq 0) "real candidate run A expected exit 0, got $($runA.ExitCode): $($runA.Text)"
    $runB = Invoke-RecoveryTool $outputB
    Assert-True ($runB.ExitCode -eq 0) "real candidate run B expected exit 0, got $($runB.ExitCode): $($runB.Text)"

    Assert-True ((Get-UpperSha256 $RealSourcePath) -ceq $sourceShaBefore) 'Real source bytes changed.'
    Assert-True ((Get-UpperSha256 $outputA) -ceq (Get-UpperSha256 $outputB)) 'Repeated real output SHA differs.'
    $metrics = [A01.ForegroundRecoveryTests.ImageOracle]::AssertRecoveryContract($outputA)
    Write-Output "PASS: Remove-ConnectedChromaMatte real contract; $metrics; candidate-sha256: $(Get-UpperSha256 $outputA)"
}
catch {
    [Console]::Error.WriteLine($_.Exception.Message)
    exit 1
}
finally {
    if (Test-Path -LiteralPath $tempRoot) {
        Remove-Item -LiteralPath $tempRoot -Recurse -Force
    }
}
