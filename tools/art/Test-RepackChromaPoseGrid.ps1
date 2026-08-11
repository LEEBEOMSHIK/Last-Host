[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$ToolPath,

    [Parameter(Mandatory = $true)]
    [string]$RealSourcePath,

    [Parameter(Mandatory = $true)]
    [string]$LayoutPath,

    [Parameter(Mandatory = $true)]
    [ValidatePattern('^[0-9A-F]{64}$')]
    [string]$ExpectedRealSourceSha256,

    [ValidateSet('Full', 'ManifestContract', 'MappingOracle', 'MatteDespill', 'NonlinearFringe', 'ResidualPink', 'OverwriteAtomic', 'SyntheticFull', 'Real')]
    [string]$TargetedCase = 'Full',

    [switch]$InjectMappingFault
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# This must remain the first filesystem precondition so the TDD RED result is
# independent of the fixture, manifest, and real-source state.
if (-not (Test-Path -LiteralPath $ToolPath -PathType Leaf)) {
    [Console]::Error.WriteLine('Tool not found')
    exit 1
}

Add-Type -AssemblyName System.Drawing

if (-not ('A01.RepackTests.ImageAssertions' -as [type])) {
    Add-Type -TypeDefinition @'
using System;
using System.Drawing;

namespace A01.RepackTests
{
    public static class ImageAssertions
    {
        public static void AssertOutputContract(
            string path, int width, int height, int columns, int rows,
            int cellWidth, int cellHeight, int band, double minimumCoverage,
            double maximumCoverage)
        {
            using (var bitmap = new Bitmap(path))
            {
                if (bitmap.Width != width || bitmap.Height != height)
                    throw new InvalidOperationException("Unexpected output dimensions.");

                var counts = new int[columns * rows];
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var color = bitmap.GetPixel(x, y);
                        if (color.A != 0 && color.A != 255)
                            throw new InvalidOperationException("Output alpha is not hard alpha.");
                        if (color.A == 0)
                        {
                            if (color.R != 0 || color.G != 0 || color.B != 0)
                                throw new InvalidOperationException("Transparent output RGB is not black.");
                            continue;
                        }

                        var column = x / cellWidth;
                        var row = y / cellHeight;
                        var localX = x % cellWidth;
                        var localY = y % cellHeight;
                        if (localX < band || localX >= cellWidth - band ||
                            localY < band || localY >= cellHeight - band)
                            throw new InvalidOperationException("Opaque pixel enters the boundary band.");
                        counts[row * columns + column]++;
                    }
                }

                var cellArea = cellWidth * cellHeight;
                for (var index = 0; index < counts.Length; index++)
                {
                    var coverage = (double)counts[index] / cellArea;
                    if (coverage < minimumCoverage || coverage > maximumCoverage)
                        throw new InvalidOperationException("Output coverage is outside the inclusive range.");
                }
            }
        }

        public static void AssertSyntheticExactMapping(
            string sourcePath, string outputPath, int sourceCellWidth,
            int sourceCellHeight, int[] sourceAxes, int targetX, int targetY)
        {
            using (var source = new Bitmap(sourcePath))
            using (var output = new Bitmap(outputPath))
            {
                var expected = new int[output.Width * output.Height];
                for (var row = 0; row < 5; row++)
                {
                    for (var column = 0; column < 4; column++)
                    {
                        var left = column * sourceCellWidth;
                        var top = row * sourceCellHeight;
                        var maxY = -1;
                        for (var y = top; y < top + sourceCellHeight; y++)
                        for (var x = left; x < left + sourceCellWidth; x++)
                        {
                            var c = source.GetPixel(x, y);
                            if (!(c.R == 255 && c.G == 0 && c.B == 255))
                                maxY = Math.Max(maxY, y);
                        }
                        if (maxY < 0)
                            throw new InvalidOperationException("Synthetic pose unexpectedly empty.");

                        var dx = column * 320 + targetX - sourceAxes[column];
                        var dy = row * 320 + targetY - maxY;
                        for (var y = top; y < top + sourceCellHeight; y++)
                        for (var x = left; x < left + sourceCellWidth; x++)
                        {
                            var c = source.GetPixel(x, y);
                            if (c.R == 255 && c.G == 0 && c.B == 255)
                                continue;
                            var destinationIndex = (y + dy) * output.Width + x + dx;
                            if (expected[destinationIndex] != 0)
                                throw new InvalidOperationException("Synthetic destination collision.");
                            expected[destinationIndex] = c.ToArgb();
                        }
                    }
                }

                for (var y = 0; y < output.Height; y++)
                for (var x = 0; x < output.Width; x++)
                {
                    if (output.GetPixel(x, y).ToArgb() != expected[y * output.Width + x])
                        throw new InvalidOperationException("Synthetic RGBA mapping mismatch.");
                }
            }
        }

        public static string AssertReferenceExactMapping(
            string sourcePath, string outputPath, int[] cutsX, int[] cutsY,
            int[] sourceAxes, int targetX, int targetY,
            int[] mustRemove, int[] mustRetain, int minimumDespillCount,
            int expectedNonlinearFringeCount = -1, int expectedResidualPinkCount = -1)
        {
            using (var source = new Bitmap(sourcePath))
            using (var output = new Bitmap(outputPath))
            {
                var width = source.Width;
                var height = source.Height;
                var pixels = new int[width * height];
                for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    pixels[y * width + x] = source.GetPixel(x, y).ToArgb();

                var removed = new bool[pixels.Length];
                var queue = new int[pixels.Length];
                var head = 0;
                var tail = 0;
                Func<int, int> keyDistance = argb =>
                {
                    var r = (argb >> 16) & 255;
                    var g = (argb >> 8) & 255;
                    var b = argb & 255;
                    return Math.Max(Math.Max(Math.Abs(r - 255), Math.Abs(g)), Math.Abs(b - 255));
                };
                for (var index = 0; index < pixels.Length; index++)
                {
                    if (keyDistance(pixels[index]) <= 24)
                    {
                        removed[index] = true;
                        queue[tail++] = index;
                    }
                }
                Action<int> enqueueFlood = index =>
                {
                    if (!removed[index] && keyDistance(pixels[index]) <= 48)
                    {
                        removed[index] = true;
                        queue[tail++] = index;
                    }
                };
                while (head < tail)
                {
                    var index = queue[head++];
                    var x = index % width;
                    var y = index / width;
                    if (x > 0) enqueueFlood(index - 1);
                    if (x + 1 < width) enqueueFlood(index + 1);
                    if (y > 0) enqueueFlood(index - width);
                    if (y + 1 < height) enqueueFlood(index + width);
                }
                foreach (var index in mustRemove)
                    if (!removed[index]) throw new InvalidOperationException("Reference flood failed a required removal marker.");
                foreach (var index in mustRetain)
                    if (removed[index]) throw new InvalidOperationException("Reference flood removed a required retained marker.");

                var nearMask = new bool[pixels.Length];
                for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    var index = y * width + x;
                    if (!removed[index]) continue;
                    for (var oy = -2; oy <= 2; oy++)
                    for (var ox = -2; ox <= 2; ox++)
                    {
                        var nx = x + ox;
                        var ny = y + oy;
                        if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                            nearMask[ny * width + nx] = true;
                    }
                }

                var processed = (int[])pixels.Clone();
                var despilled = new bool[pixels.Length];
                var despilledCount = 0;
                for (var index = 0; index < pixels.Length; index++)
                {
                    if (removed[index] || !nearMask[index]) continue;
                    var x = index % width;
                    var y = index / width;
                    var donorIndex = -1;
                    var bestSquared = int.MaxValue;
                    var bestY = int.MaxValue;
                    var bestX = int.MaxValue;
                    for (var oy = -8; oy <= 8; oy++)
                    for (var ox = -8; ox <= 8; ox++)
                    {
                        var squared = ox * ox + oy * oy;
                        if (squared == 0 || squared > 64) continue;
                        var nx = x + ox;
                        var ny = y + oy;
                        if (nx < 0 || nx >= width || ny < 0 || ny >= height) continue;
                        var next = ny * width + nx;
                        if (removed[next] || nearMask[next] || keyDistance(pixels[next]) <= 96) continue;
                        if (squared < bestSquared ||
                            (squared == bestSquared && (ny < bestY || (ny == bestY && nx < bestX))))
                        {
                            donorIndex = next;
                            bestSquared = squared;
                            bestY = ny;
                            bestX = nx;
                        }
                    }
                    if (donorIndex < 0) continue;

                    var candidate = pixels[index];
                    var donor = pixels[donorIndex];
                    var cr = (candidate >> 16) & 255;
                    var cg = (candidate >> 8) & 255;
                    var cb = candidate & 255;
                    var dr = (donor >> 16) & 255;
                    var dg = (donor >> 8) & 255;
                    var db = donor & 255;
                    var vr = 255 - dr;
                    var vg = -dg;
                    var vb = 255 - db;
                    var denominator = vr * vr + vg * vg + vb * vb;
                    if (denominator == 0) continue;
                    var projection = ((cr - dr) * vr + (cg - dg) * vg + (cb - db) * vb) / (double)denominator;
                    if (projection < 0.08 || projection > 0.92) continue;
                    var residual = Math.Max(Math.Max(
                        Math.Abs(cr - (dr + projection * vr)),
                        Math.Abs(cg - (dg + projection * vg))),
                        Math.Abs(cb - (db + projection * vb)));
                    if (residual > 24.0) continue;
                    processed[index] = (candidate & unchecked((int)0xFF000000)) | (dr << 16) | (dg << 8) | db;
                    despilled[index] = true;
                    despilledCount++;
                }
                if (despilledCount < minimumDespillCount)
                    throw new InvalidOperationException("Independent oracle found too few qualifying despilled pixels.");

                var expected = new int[output.Width * output.Height];
                var occupied = new bool[expected.Length];
                var unresolvedDespillCount = 0;
                for (var row = 0; row < 5; row++)
                for (var column = 0; column < 4; column++)
                {
                    var maxY = -1;
                    for (var y = cutsY[row]; y < cutsY[row + 1]; y++)
                    for (var x = cutsX[column]; x < cutsX[column + 1]; x++)
                        if (!removed[y * width + x]) maxY = Math.Max(maxY, y);
                    if (maxY < 0) throw new InvalidOperationException("Reference pose unexpectedly empty.");
                    var dx = column * 320 + targetX - sourceAxes[column];
                    var dy = row * 320 + targetY - maxY;
                    for (var y = cutsY[row]; y < cutsY[row + 1]; y++)
                    for (var x = cutsX[column]; x < cutsX[column + 1]; x++)
                    {
                        var sourceIndex = y * width + x;
                        if (removed[sourceIndex]) continue;
                        var destinationIndex = (y + dy) * output.Width + x + dx;
                        if (occupied[destinationIndex]) throw new InvalidOperationException("Reference destination collision.");
                        occupied[destinationIndex] = true;
                        expected[destinationIndex] = processed[sourceIndex];
                        if (expectedNonlinearFringeCount < 0 && despilled[sourceIndex] &&
                            output.GetPixel(x + dx, y + dy).ToArgb() != processed[sourceIndex])
                            unresolvedDespillCount++;
                    }
                }
                if (unresolvedDespillCount != 0)
                    throw new InvalidOperationException("Output contains unresolved qualifying edge blends.");
                Func<int, bool> hasNonlinearFringeRgb = argb =>
                {
                    var r = (argb >> 16) & 255;
                    var g = (argb >> 8) & 255;
                    var b = argb & 255;
                    return Math.Max(r, b) >= 128 && r - g >= 40 && b - g >= 40 && Math.Abs(r - b) <= 96;
                };
                Func<int[], int, bool> isNonlinearFringe = (values, index) =>
                {
                    var argb = values[index];
                    if (((argb >> 24) & 255) != 255) return false;
                    var x = index % output.Width;
                    var y = index / output.Width;
                    var touchesTransparent = false;
                    for (var oy = -1; oy <= 1; oy++)
                    for (var ox = -1; ox <= 1; ox++)
                    {
                        if (ox == 0 && oy == 0) continue;
                        var nx = x + ox;
                        var ny = y + oy;
                        if (nx >= 0 && nx < output.Width && ny >= 0 && ny < output.Height &&
                            ((values[ny * output.Width + nx] >> 24) & 255) == 0)
                            touchesTransparent = true;
                    }
                    if (!touchesTransparent) return false;
                    return hasNonlinearFringeRgb(argb);
                };
                Func<int, bool> hasResidualPinkRgb = argb =>
                {
                    var r = (argb >> 16) & 255;
                    var g = (argb >> 8) & 255;
                    var b = argb & 255;
                    return r >= 172 && r - g >= 120 && b - g >= 40 && r - b >= 96 && b >= 64;
                };
                Func<int[], int, bool> isResidualPink = (values, index) =>
                {
                    var argb = values[index];
                    if (((argb >> 24) & 255) != 255) return false;
                    var x = index % output.Width;
                    var y = index / output.Width;
                    for (var oy = -1; oy <= 1; oy++)
                    for (var ox = -1; ox <= 1; ox++)
                    {
                        if (ox == 0 && oy == 0) continue;
                        var nx = x + ox;
                        var ny = y + oy;
                        if (nx >= 0 && nx < output.Width && ny >= 0 && ny < output.Height &&
                            ((values[ny * output.Width + nx] >> 24) & 255) == 0)
                            return hasResidualPinkRgb(argb);
                    }
                    return false;
                };
                var expectedNonlinearFringes = new bool[expected.Length];
                var expectedResidualPinks = new bool[expected.Length];
                var expectedCleanupCandidates = new bool[expected.Length];
                var expectedNonlinearCount = 0;
                var expectedResidualCount = 0;
                if (expectedNonlinearFringeCount >= 0)
                {
                    for (var index = 0; index < expected.Length; index++)
                    {
                        expectedNonlinearFringes[index] = isNonlinearFringe(expected, index);
                        if (expectedNonlinearFringes[index]) expectedNonlinearCount++;
                    }
                    if (expectedNonlinearCount != expectedNonlinearFringeCount)
                        throw new InvalidOperationException("Unexpected pre-cleanup nonlinear fringe count: " + expectedNonlinearCount);
                }
                if (expectedResidualPinkCount >= 0)
                {
                    for (var index = 0; index < expected.Length; index++)
                    {
                        expectedResidualPinks[index] = isResidualPink(expected, index);
                        if (expectedResidualPinks[index]) expectedResidualCount++;
                    }
                    if (expectedResidualCount != expectedResidualPinkCount)
                        throw new InvalidOperationException("Unexpected pre-cleanup residual pink count: " + expectedResidualCount);
                }
                for (var index = 0; index < expected.Length; index++)
                    expectedCleanupCandidates[index] = expectedNonlinearFringes[index] || expectedResidualPinks[index];

                var strongKeyOpaqueCount = 0;
                var changedNonlinearRgbCount = 0;
                var changedResidualPinkRgbCount = 0;
                var changedNonCandidateRgbCount = 0;
                var changedAlphaCount = 0;
                var remainingNonlinearCount = 0;
                var remainingResidualPinkCount = 0;
                var actualPixels = new int[expected.Length];
                for (var y = 0; y < output.Height; y++)
                for (var x = 0; x < output.Width; x++)
                    actualPixels[y * output.Width + x] = output.GetPixel(x, y).ToArgb();
                for (var index = 0; index < actualPixels.Length; index++)
                {
                    if (isNonlinearFringe(actualPixels, index)) remainingNonlinearCount++;
                    if (isResidualPink(actualPixels, index)) remainingResidualPinkCount++;
                }
                if (expectedNonlinearFringeCount >= 0 && remainingNonlinearCount != 0)
                    throw new InvalidOperationException("Output retains nonlinear fringe candidates: " + remainingNonlinearCount);
                if (expectedResidualPinkCount >= 0 && remainingResidualPinkCount != 0)
                    throw new InvalidOperationException("Output retains residual pink candidates: " + remainingResidualPinkCount);
                for (var y = 0; y < output.Height; y++)
                for (var x = 0; x < output.Width; x++)
                {
                    var index = y * output.Width + x;
                    var outputArgb = actualPixels[index];
                    if (((outputArgb >> 24) & 255) == 255 && keyDistance(outputArgb) <= 24)
                        strongKeyOpaqueCount++;
                    if (expectedNonlinearFringeCount < 0 && expectedResidualPinkCount < 0)
                    {
                        if (outputArgb != expected[index])
                            throw new InvalidOperationException("Independent RGBA mapping mismatch or extra output pixel.");
                        continue;
                    }

                    if (((outputArgb >> 24) & 255) != ((expected[index] >> 24) & 255)) changedAlphaCount++;
                    if (!expectedCleanupCandidates[index])
                    {
                        if ((outputArgb & 0x00FFFFFF) != (expected[index] & 0x00FFFFFF)) changedNonCandidateRgbCount++;
                        if (outputArgb != expected[index])
                            throw new InvalidOperationException("Non-candidate RGB changed during nonlinear fringe cleanup.");
                        continue;
                    }

                    var donorIndex = -1;
                    var bestDistanceSquared = int.MaxValue;
                    var bestY = int.MaxValue;
                    var bestX = int.MaxValue;
                    for (var oy = -8; oy <= 8; oy++)
                    for (var ox = -8; ox <= 8; ox++)
                    {
                        var distanceSquared = ox * ox + oy * oy;
                        if (distanceSquared == 0 || distanceSquared > 64) continue;
                        var nx = x + ox;
                        var ny = y + oy;
                        if (nx < 0 || nx >= output.Width || ny < 0 || ny >= output.Height) continue;
                        var next = ny * output.Width + nx;
                        if (((expected[next] >> 24) & 255) != 255 || expectedCleanupCandidates[next] ||
                            hasNonlinearFringeRgb(expected[next]) || hasResidualPinkRgb(expected[next])) continue;
                        if (distanceSquared < bestDistanceSquared ||
                            (distanceSquared == bestDistanceSquared && (ny < bestY || (ny == bestY && nx < bestX))))
                        {
                            donorIndex = next;
                            bestDistanceSquared = distanceSquared;
                            bestY = ny;
                            bestX = nx;
                        }
                    }
                    if (donorIndex < 0) throw new InvalidOperationException("Cleanup fringe candidate has no donor.");
                    if ((outputArgb & 0x00FFFFFF) != (expected[donorIndex] & 0x00FFFFFF))
                        throw new InvalidOperationException(expectedResidualPinks[index]
                            ? "Residual pink candidate was not replaced by its deterministic donor RGB."
                            : "Nonlinear fringe candidate was not replaced by its deterministic donor RGB.");
                    if ((outputArgb & 0x00FFFFFF) != (expected[index] & 0x00FFFFFF))
                    {
                        if (expectedNonlinearFringes[index]) changedNonlinearRgbCount++;
                        if (expectedResidualPinks[index]) changedResidualPinkRgbCount++;
                    }
                }
                if (strongKeyOpaqueCount != 0)
                    throw new InvalidOperationException("Output retains opaque strong-key pixels.");
                if (expectedNonlinearFringeCount >= 0)
                {
                    if (changedAlphaCount != 0) throw new InvalidOperationException("Nonlinear fringe cleanup changed alpha values: " + changedAlphaCount);
                    if (changedNonCandidateRgbCount != 0) throw new InvalidOperationException("Nonlinear fringe cleanup changed non-candidate RGB values: " + changedNonCandidateRgbCount);
                    if (changedNonlinearRgbCount != expectedNonlinearFringeCount)
                        throw new InvalidOperationException("Nonlinear fringe cleanup changed candidate RGB count: " + changedNonlinearRgbCount);
                }
                if (expectedResidualPinkCount >= 0)
                {
                    if (changedResidualPinkRgbCount != expectedResidualPinkCount)
                        throw new InvalidOperationException("Residual pink cleanup changed candidate RGB count: " + changedResidualPinkRgbCount);
                }
                return String.Format("MAPPING_STATS strong-key-opaque={0}; unresolved-qualifying-blends={1}; despilled={2}",
                    strongKeyOpaqueCount, unresolvedDespillCount, despilledCount);
            }
        }
    }
}
'@ -ReferencedAssemblies @('System.Drawing.Common', 'System.Drawing.Primitives', 'System.Private.Windows.GdiPlus', 'System.Private.Windows.Core')
}

function Assert-True {
    param([bool]$Condition, [string]$Message)
    if (-not $Condition) { throw $Message }
}

function Get-UpperSha256 {
    param([string]$Path)
    return (Get-FileHash -Algorithm SHA256 -LiteralPath $Path).Hash.ToUpperInvariant()
}

function Write-JsonFile {
    param([object]$Value, [string]$Path)
    $Value | ConvertTo-Json -Depth 12 | Set-Content -LiteralPath $Path -Encoding utf8NoBOM
}

$PoseIds = @(
    'p1_idle', 'p1_speak', 'p1_laugh', 'p1_rise',
    'p2_idle', 'p2_nod', 'p2_laugh', 'p2_hold',
    'p3_work', 'p3_shoulder_laugh', 'p3_head_turn', 'p3_hold',
    'p4_idle', 'p4_gesture', 'p4_exit_turn', 'p4_hold',
    'p5_idle', 'p5_laugh', 'p5_step_ready', 'p5_hold'
)

function New-SyntheticSource {
    param(
        [string]$Path,
        [ValidateSet('Valid', 'Empty', 'Oversize', 'Boundary', 'LowCoverage', 'HighCoverage', 'SoftAlpha')]
        [string]$Mode = 'Valid'
    )

    $sourceCellWidth = if ($Mode -in @('Oversize', 'Boundary', 'HighCoverage')) { 320 } else { 80 }
    $sourceCellHeight = if ($Mode -eq 'HighCoverage') { 320 } else { 80 }
    $bitmap = [Drawing.Bitmap]::new($sourceCellWidth * 4, $sourceCellHeight * 5, [Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.Clear([Drawing.Color]::FromArgb(255, 255, 0, 255))
        for ($row = 0; $row -lt 5; $row++) {
            for ($column = 0; $column -lt 4; $column++) {
                if ($Mode -eq 'Empty' -and $row -eq 0 -and $column -eq 0) { continue }

                $localLeft = 4
                $localTop = 6
                $shapeWidth = 72
                $shapeHeight = 72
                if ($sourceCellWidth -eq 320) {
                    $localLeft = 80
                    $localTop = 46
                    $shapeWidth = 160
                    $shapeHeight = 32
                }
                if ($Mode -eq 'Oversize' -and $row -eq 0 -and $column -eq 0) {
                    $localLeft = 6; $localTop = 60; $shapeWidth = 309; $shapeHeight = 18
                }
                elseif ($Mode -eq 'Boundary' -and $row -eq 0 -and $column -eq 0) {
                    $localLeft = 5
                }
                elseif ($Mode -eq 'LowCoverage' -and $row -eq 0 -and $column -eq 0) {
                    $localLeft = 5; $localTop = 8; $shapeWidth = 70; $shapeHeight = 70
                }
                elseif ($Mode -eq 'HighCoverage' -and $row -eq 0 -and $column -eq 0) {
                    $localLeft = 35; $localTop = 57; $shapeWidth = 250; $shapeHeight = 250
                }

                $color = [Drawing.Color]::FromArgb(255, 30 + ($row * 30), 180 + ($column * 3), 60 + (($row * 4 + $column) * 5))
                for ($y = $localTop; $y -lt $localTop + $shapeHeight; $y++) {
                    for ($x = $localLeft; $x -lt $localLeft + $shapeWidth; $x++) {
                        $bitmap.SetPixel(($column * $sourceCellWidth) + $x, ($row * $sourceCellHeight) + $y, $color)
                    }
                }

                if ($Mode -eq 'Valid') {
                    $propColor = [Drawing.Color]::FromArgb(255, 210, 120 + $column, 20 + $row)
                    $bitmap.SetPixel(($column * $sourceCellWidth) + 78, ($row * $sourceCellHeight) + 74, $propColor)
                    $bitmap.SetPixel(($column * $sourceCellWidth) + 79, ($row * $sourceCellHeight) + 74, $propColor)
                    $bitmap.SetPixel(($column * $sourceCellWidth) + 78, ($row * $sourceCellHeight) + 75, $propColor)
                    $bitmap.SetPixel(($column * $sourceCellWidth) + 79, ($row * $sourceCellHeight) + 75, $propColor)
                }
            }
        }
        if ($Mode -eq 'SoftAlpha') {
            $bitmap.SetPixel(4, 6, [Drawing.Color]::FromArgb(128, 30, 40, 60))
        }
        $bitmap.Save($Path, [Drawing.Imaging.ImageFormat]::Png)
    }
    finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }

    return [pscustomobject]@{ CellWidth = $sourceCellWidth; CellHeight = $sourceCellHeight }
}

function New-SyntheticManifest {
    param([string]$SourcePath, [int]$SourceCellWidth, [int]$SourceCellHeight)
    $sourceItem = Get-Item -LiteralPath $SourcePath
    $sourceAxes = @(for ($column = 0; $column -lt 4; $column++) { ($column * $SourceCellWidth) + [int]($SourceCellWidth / 2) })
    $poses = @()
    for ($row = 0; $row -lt 5; $row++) {
        for ($column = 0; $column -lt 4; $column++) {
            $index = ($row * 4) + $column
            $poses += [ordered]@{
                id = $PoseIds[$index]
                sourceRect = [ordered]@{ x = ($column * $SourceCellWidth); y = ($row * $SourceCellHeight); width = $SourceCellWidth; height = $SourceCellHeight }
                sourceAxisX = $sourceAxes[$column]
                sourceGroundY = 'maxRetainedY'
                target = [ordered]@{ row = $row; column = $column }
            }
        }
    }
    return [ordered]@{
        schemaVersion = 1
        source = [ordered]@{ sha256 = (Get-UpperSha256 $SourcePath); width = ($SourceCellWidth * 4); height = ($SourceCellHeight * 5); byteLength = $sourceItem.Length }
        sourceCuts = [ordered]@{ x = @(0, $SourceCellWidth, ($SourceCellWidth * 2), ($SourceCellWidth * 3), ($SourceCellWidth * 4)); y = @(0, $SourceCellHeight, ($SourceCellHeight * 2), ($SourceCellHeight * 3), ($SourceCellHeight * 4), ($SourceCellHeight * 5)) }
        sourceAxesX = $sourceAxes
        output = [ordered]@{ width = 1280; height = 1600; grid = [ordered]@{ columns = 4; rows = 5; cellWidth = 320; cellHeight = 320 } }
        matte = [ordered]@{ keyColor = @(255, 0, 255); distanceMetric = 'chebyshev'; tolerance = 96; connectivity = 8 }
        boundaryBand = 6
        coverage = [ordered]@{ minimum = 0.05; maximum = 0.60 }
        targetAnchor = [ordered]@{ x = 160; y = 306 }
        poses = $poses
    }
}

function Add-OracleFixtureSentinels {
    param([string]$Path)
    $replacementPath = $Path + '.fixture.png'
    $bitmap = [Drawing.Bitmap]::new($Path)
    try {
        $bitmap.SetPixel(5, 7, [Drawing.Color]::FromArgb(255, 158, 0, 255))
        $bitmap.SetPixel(4, 6, [Drawing.Color]::FromArgb(255, 158, 0, 255))
        $bitmap.SetPixel(10, 10, [Drawing.Color]::FromArgb(255, 157, 0, 255))
        $bitmap.SetPixel(20, 20, [Drawing.Color]::FromArgb(255, 255, 0, 255))
        for ($x = 76; $x -le 83; $x++) {
            $bitmap.SetPixel($x, 20, [Drawing.Color]::FromArgb(255, 17, 34, 201))
            $bitmap.SetPixel($x, 27, [Drawing.Color]::FromArgb(255, 203, 71, 9))
        }
        for ($y = 21; $y -le 26; $y++) {
            $bitmap.SetPixel(76, $y, [Drawing.Color]::FromArgb(255, 17, 34, 201))
            $bitmap.SetPixel(83, $y, [Drawing.Color]::FromArgb(255, 203, 71, 9))
        }
        $bitmap.SetPixel(30, 30, [Drawing.Color]::FromArgb(255, 17, 34, 201))
        $bitmap.SetPixel(31, 30, [Drawing.Color]::FromArgb(255, 203, 71, 9))
        $bitmap.Save($replacementPath, [Drawing.Imaging.ImageFormat]::Png)
    }
    finally { $bitmap.Dispose() }
    [IO.File]::Delete($Path)
    [IO.File]::Move($replacementPath, $Path)
}

function Inject-OutputChannelSwap {
    param([string]$Path, [int]$X, [int]$Y)
    $replacementPath = $Path + '.fault.png'
    $bitmap = [Drawing.Bitmap]::new($Path)
    try {
        $color = $bitmap.GetPixel($X, $Y)
        $bitmap.SetPixel($X, $Y, [Drawing.Color]::FromArgb($color.A, $color.B, $color.G, $color.R))
        $bitmap.Save($replacementPath, [Drawing.Imaging.ImageFormat]::Png)
    }
    finally { $bitmap.Dispose() }
    [IO.File]::Delete($Path)
    [IO.File]::Move($replacementPath, $Path)
}

function Add-MatteDespillFixture {
    param([string]$Path)
    $replacementPath = $Path + '.matte-despill.png'
    $bitmap = [Drawing.Bitmap]::new($Path)
    try {
        $key = [Drawing.Color]::FromArgb(255, 255, 0, 255)
        $flood = [Drawing.Color]::FromArgb(255, 207, 48, 207)
        $donor = [Drawing.Color]::FromArgb(255, 30, 40, 60)
        $blend = [Drawing.Color]::FromArgb(255, 143, 20, 158)
        $nonblendPurple = [Drawing.Color]::FromArgb(255, 100, 0, 120)
        $farCore = [Drawing.Color]::FromArgb(255, 5, 180, 90)

        $bitmap.SetPixel(20, 20, $key)
        $bitmap.SetPixel(21, 20, $flood)
        $bitmap.SetPixel(30, 30, $key)
        $bitmap.SetPixel(31, 31, $flood)
        $bitmap.SetPixel(50, 30, $key)
        $bitmap.SetPixel(51, 30, $blend)
        $bitmap.SetPixel(53, 30, $donor)
        $bitmap.SetPixel(60, 30, $key)
        $bitmap.SetPixel(61, 30, $nonblendPurple)
        $bitmap.SetPixel(70, 40, $farCore)
        $bitmap.Save($replacementPath, [Drawing.Imaging.ImageFormat]::Png)
    }
    finally { $bitmap.Dispose() }
    [IO.File]::Delete($Path)
    [IO.File]::Move($replacementPath, $Path)
}

function Assert-MatteDespillFixture {
    param([string]$SourcePath, [string]$OutputPath)
    $source = [Drawing.Bitmap]::new($SourcePath)
    $output = [Drawing.Bitmap]::new($OutputPath)
    try {
        $dx = 120
        $dy = 229
        $strong = $output.GetPixel(20 + $dx, 20 + $dy)
        Assert-True ($strong.A -eq 0 -and $strong.R -eq 0 -and $strong.G -eq 0 -and $strong.B -eq 0) 'Enclosed strong-key island was not transparent black.'
        $orthogonalFlood = $output.GetPixel(21 + $dx, 20 + $dy)
        Assert-True ($orthogonalFlood.A -eq 0 -and $orthogonalFlood.R -eq 0 -and $orthogonalFlood.G -eq 0 -and $orthogonalFlood.B -eq 0) '4-neighbor flood did not remove the <=48 orthogonal pixel.'
        Assert-True ($output.GetPixel(31 + $dx, 31 + $dy).A -eq 255) 'Diagonal-only <=48 pixel was incorrectly flooded.'

        $donor = $source.GetPixel(53, 30)
        $despilled = $output.GetPixel(51 + $dx, 30 + $dy)
        Assert-True ($despilled.A -eq 255 -and $despilled.R -eq $donor.R -and $despilled.G -eq $donor.G -and $despilled.B -eq $donor.B) 'Qualified donor-line edge was not despilled to donor RGB.'

        foreach ($point in @(@(61, 30), @(70, 40))) {
            $expected = $source.GetPixel($point[0], $point[1])
            $actual = $output.GetPixel($point[0] + $dx, $point[1] + $dy)
            Assert-True ($actual.ToArgb() -eq $expected.ToArgb()) "Retained non-despilled pixel changed at $($point[0]),$($point[1])."
        }
    }
    finally {
        $output.Dispose()
        $source.Dispose()
    }
}

$ResolvedToolPath = (Resolve-Path -LiteralPath $ToolPath).Path
$PwshPath = (Get-Command pwsh -ErrorAction Stop).Source

function Invoke-RepackTool {
    param([string]$InputPath, [string]$ManifestPath, [string]$OutputPath, [switch]$Force)
    $arguments = @('-NoLogo', '-NoProfile', '-File', $ResolvedToolPath, '-InputPath', $InputPath, '-LayoutPath', $ManifestPath, '-OutputPath', $OutputPath)
    if ($Force) { $arguments += '-Force' }
    $captured = @(& $PwshPath @arguments 2>&1)
    $exitCode = $LASTEXITCODE
    return [pscustomobject]@{ ExitCode = $exitCode; Text = (($captured | ForEach-Object { $_.ToString() }) -join "`n") }
}

function Assert-Pass {
    param([object]$Run, [string]$Label)
    Assert-True ($Run.ExitCode -eq 0) "$Label expected exit 0, got $($Run.ExitCode): $($Run.Text)"
}

function Assert-Failure {
    param([object]$Run, [string]$Label, [string]$Pattern)
    Assert-True ($Run.ExitCode -ne 0) "$Label unexpectedly passed."
    Assert-True ($Run.Text -match $Pattern) "$Label failed for the wrong reason: $($Run.Text)"
}

function Copy-ManifestObject {
    param([object]$Manifest)
    return ($Manifest | ConvertTo-Json -Depth 12 | ConvertFrom-Json -Depth 12)
}

$tempRoot = Join-Path ([IO.Path]::GetTempPath()) ('last-host-repack-tests-' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $tempRoot | Out-Null

try {
    $validSource = Join-Path $tempRoot 'valid-source.png'
    $validShape = New-SyntheticSource -Path $validSource -Mode Valid
    $validManifest = New-SyntheticManifest -SourcePath $validSource -SourceCellWidth $validShape.CellWidth -SourceCellHeight $validShape.CellHeight
    $validLayout = Join-Path $tempRoot 'valid-layout.json'
    Write-JsonFile $validManifest $validLayout
    $sourceShaBefore = Get-UpperSha256 $validSource

    if ($TargetedCase -eq 'OverwriteAtomic') {
        $overwriteOutput = Join-Path $tempRoot 'overwrite-output.png'
        [IO.File]::WriteAllBytes($overwriteOutput, [byte[]](1, 2, 3, 4, 5))
        $overwriteShaBefore = Get-UpperSha256 $overwriteOutput
        Assert-Pass (Invoke-RepackTool $validSource $validLayout $overwriteOutput -Force) 'existing output atomic replacement'
        Assert-True ((Get-UpperSha256 $overwriteOutput) -cne $overwriteShaBefore) 'Existing output bytes were not replaced.'
        [A01.RepackTests.ImageAssertions]::AssertOutputContract($overwriteOutput, 1280, 1600, 4, 5, 320, 320, 6, 0.05, 0.60)
        Assert-True (@(Get-ChildItem -LiteralPath $tempRoot -Filter '*.bak' -File).Count -eq 0) 'Atomic replacement left a backup file.'
        Write-Output 'PASS: targeted existing-output atomic replacement'
        return
    }

    if ($TargetedCase -in @('MatteDespill', 'SyntheticFull', 'Full')) {
        $matteSource = Join-Path $tempRoot 'matte-despill-source.png'
        $matteShape = New-SyntheticSource -Path $matteSource -Mode Valid
        Add-MatteDespillFixture -Path $matteSource
        $matteSourceSha = Get-UpperSha256 $matteSource
        $matteManifest = New-SyntheticManifest -SourcePath $matteSource -SourceCellWidth $matteShape.CellWidth -SourceCellHeight $matteShape.CellHeight
        $matteLayout = Join-Path $tempRoot 'matte-despill-layout.json'
        $matteOutput = Join-Path $tempRoot 'matte-despill-output.png'
        Write-JsonFile $matteManifest $matteLayout
        Assert-Pass (Invoke-RepackTool $matteSource $matteLayout $matteOutput) 'matte/despill baseline'
        Assert-True ((Get-UpperSha256 $matteSource) -ceq $matteSourceSha) 'Matte/despill source bytes changed.'
        Assert-MatteDespillFixture -SourcePath $matteSource -OutputPath $matteOutput
        $matteWidth = 320
        [A01.RepackTests.ImageAssertions]::AssertReferenceExactMapping(
            $matteSource, $matteOutput,
            @(0, 80, 160, 240, 320), @(0, 80, 160, 240, 320, 400),
            @(40, 120, 200, 280), 160, 306,
            @(((20 * $matteWidth) + 20), ((20 * $matteWidth) + 21), ((30 * $matteWidth) + 30), ((30 * $matteWidth) + 50), ((30 * $matteWidth) + 60)),
            @(((31 * $matteWidth) + 31), ((30 * $matteWidth) + 61), ((40 * $matteWidth) + 70)), 1)
        Write-Output 'PASS: targeted matte/despill contract'
        if ($TargetedCase -eq 'MatteDespill') { return }
    }

    if ($TargetedCase -in @('ManifestContract', 'SyntheticFull', 'Full')) {
        $baselineOutput = Join-Path $tempRoot 'manifest-baseline.png'
        Assert-Pass (Invoke-RepackTool $validSource $validLayout $baselineOutput) 'manifest contract baseline'

        $contractCases = @()
        $missingSchema = Copy-ManifestObject $validManifest; $missingSchema.PSObject.Properties.Remove('schemaVersion'); $contractCases += ,@('missing schemaVersion', $missingSchema, 'schemaVersion')
        $wrongSchema = Copy-ManifestObject $validManifest; $wrongSchema.schemaVersion = 2; $contractCases += ,@('wrong schemaVersion', $wrongSchema, 'schemaVersion')
        $missingByteLength = Copy-ManifestObject $validManifest; $missingByteLength.source.PSObject.Properties.Remove('byteLength'); $contractCases += ,@('missing byteLength', $missingByteLength, 'byteLength')
        $missingCuts = Copy-ManifestObject $validManifest; $missingCuts.PSObject.Properties.Remove('sourceCuts'); $contractCases += ,@('missing sourceCuts', $missingCuts, 'sourceCuts')
        $xCutCount = Copy-ManifestObject $validManifest; $xCutCount.sourceCuts.x = @(0, 80, 160, 320); $contractCases += ,@('sourceCuts.x count', $xCutCount, 'sourceCuts.x')
        $yCutCount = Copy-ManifestObject $validManifest; $yCutCount.sourceCuts.y = @(0, 80, 160, 240, 400); $contractCases += ,@('sourceCuts.y count', $yCutCount, 'sourceCuts.y')
        $xCutStart = Copy-ManifestObject $validManifest; $xCutStart.sourceCuts.x = @(1, 80, 160, 240, 320); $contractCases += ,@('sourceCuts.x start', $xCutStart, 'sourceCuts.x')
        $yCutStart = Copy-ManifestObject $validManifest; $yCutStart.sourceCuts.y = @(1, 80, 160, 240, 320, 400); $contractCases += ,@('sourceCuts.y start', $yCutStart, 'sourceCuts.y')
        $xCutOrder = Copy-ManifestObject $validManifest; $xCutOrder.sourceCuts.x = @(0, 80, 160, 159, 320); $contractCases += ,@('sourceCuts.x order', $xCutOrder, 'sourceCuts.x')
        $yCutOrder = Copy-ManifestObject $validManifest; $yCutOrder.sourceCuts.y = @(0, 80, 160, 159, 320, 400); $contractCases += ,@('sourceCuts.y order', $yCutOrder, 'sourceCuts.y')
        $xCutEnd = Copy-ManifestObject $validManifest; $xCutEnd.sourceCuts.x = @(0, 80, 160, 240, 319); $contractCases += ,@('sourceCuts.x end', $xCutEnd, 'sourceCuts.x')
        $yCutEnd = Copy-ManifestObject $validManifest; $yCutEnd.sourceCuts.y = @(0, 80, 160, 240, 320, 399); $contractCases += ,@('sourceCuts.y end', $yCutEnd, 'sourceCuts.y')
        $rectMismatch = Copy-ManifestObject $validManifest; $rectMismatch.poses[0].sourceRect.width = 79; $contractCases += ,@('sourceRect not adjacent cuts', $rectMismatch, 'sourceRect')
        $idSwap = Copy-ManifestObject $validManifest; $idSwap.poses[0].id = 'p1_speak'; $idSwap.poses[1].id = 'p1_idle'; $contractCases += ,@('pose ID swap', $idSwap, 'Pose ID')
        $targetSwap = Copy-ManifestObject $validManifest
        $targetSwap.poses[0].target = [pscustomobject]@{ row = 0; column = 1 }; $targetSwap.poses[0].sourceAxisX = 120
        $targetSwap.poses[1].target = [pscustomobject]@{ row = 0; column = 0 }; $targetSwap.poses[1].sourceAxisX = 40
        $contractCases += ,@('pose target/axis swap', $targetSwap, 'target')

        $caseIndex = 0
        foreach ($case in $contractCases) {
            $caseLayout = Join-Path $tempRoot ("contract-layout-$caseIndex.json")
            $caseOutput = Join-Path $tempRoot ("contract-output-$caseIndex.png")
            Write-JsonFile $case[1] $caseLayout
            Assert-Failure (Invoke-RepackTool $validSource $caseLayout $caseOutput) $case[0] $case[2]
            Assert-True (-not (Test-Path -LiteralPath $caseOutput)) "$($case[0]) created an output."
            $caseIndex++
        }
        Write-Output 'PASS: targeted manifest contract'
        if ($TargetedCase -eq 'ManifestContract') { return }
    }

    if ($TargetedCase -in @('MappingOracle', 'SyntheticFull', 'Full')) {
        $oracleSource = Join-Path $tempRoot 'oracle-source.png'
        $oracleShape = New-SyntheticSource -Path $oracleSource -Mode Valid
        Add-OracleFixtureSentinels -Path $oracleSource
        $oracleManifest = New-SyntheticManifest -SourcePath $oracleSource -SourceCellWidth $oracleShape.CellWidth -SourceCellHeight $oracleShape.CellHeight
        $oracleLayout = Join-Path $tempRoot 'oracle-layout.json'
        $oracleOutput = Join-Path $tempRoot 'oracle-output.png'
        Write-JsonFile $oracleManifest $oracleLayout
        Assert-Pass (Invoke-RepackTool $oracleSource $oracleLayout $oracleOutput) 'mapping oracle baseline'
        if ($InjectMappingFault) { Inject-OutputChannelSwap -Path $oracleOutput -X 150 -Y 261 }
        $sourceWidth = 320
        [A01.RepackTests.ImageAssertions]::AssertReferenceExactMapping(
            $oracleSource, $oracleOutput,
            @(0, 80, 160, 240, 320), @(0, 80, 160, 240, 320, 400),
            @(40, 120, 200, 280), 160, 306,
            @(((20 * $sourceWidth) + 20), ((23 * $sourceWidth) + 79), ((23 * $sourceWidth) + 80)),
            @(((7 * $sourceWidth) + 5), ((6 * $sourceWidth) + 4), ((10 * $sourceWidth) + 10)), 0)
        Write-Output 'PASS: targeted independent mapping oracle'
        if ($TargetedCase -eq 'MappingOracle') { return }
    }

    if ($TargetedCase -ne 'Real') {
    $syntheticOutputA = Join-Path $tempRoot 'synthetic-a.png'
    $syntheticOutputB = Join-Path $tempRoot 'synthetic-b.png'
    Assert-Pass (Invoke-RepackTool $validSource $validLayout $syntheticOutputA) 'valid synthetic run A'
    Assert-Pass (Invoke-RepackTool $validSource $validLayout $syntheticOutputB) 'valid synthetic run B'
    Assert-True ((Get-UpperSha256 $validSource) -ceq $sourceShaBefore) 'Synthetic source bytes changed.'
    Assert-True ((Get-UpperSha256 $syntheticOutputA) -ceq (Get-UpperSha256 $syntheticOutputB)) 'Repeated synthetic output SHA differs.'
    [A01.RepackTests.ImageAssertions]::AssertOutputContract($syntheticOutputA, 1280, 1600, 4, 5, 320, 320, 6, 0.05, 0.60)
    [A01.RepackTests.ImageAssertions]::AssertSyntheticExactMapping($validSource, $syntheticOutputA, $validShape.CellWidth, $validShape.CellHeight, @(40, 120, 200, 280), 160, 306)

    $manifestCases = @()
    $wrongSha = Copy-ManifestObject $validManifest; $wrongSha.source.sha256 = ('0' * 64); $manifestCases += ,@('wrong SHA', $wrongSha, 'Source SHA-256 mismatch')
    $missingId = Copy-ManifestObject $validManifest; $missingId.poses = @($missingId.poses | Select-Object -Skip 1); $manifestCases += ,@('missing pose ID', $missingId, 'exactly 20 poses')
    $duplicateId = Copy-ManifestObject $validManifest; $duplicateId.poses[1].id = $duplicateId.poses[0].id; $manifestCases += ,@('duplicate pose ID', $duplicateId, 'Pose ID')
    $missingTarget = Copy-ManifestObject $validManifest; $missingTarget.poses[0].target = $null; $manifestCases += ,@('missing target', $missingTarget, 'target')
    $duplicateTarget = Copy-ManifestObject $validManifest; $duplicateTarget.poses[1].target.row = 0; $duplicateTarget.poses[1].target.column = 0; $manifestCases += ,@('duplicate target', $duplicateTarget, 'fixed row-major slot')
    $overlapRect = Copy-ManifestObject $validManifest; $overlapRect.poses[0].sourceRect.width = 81; $manifestCases += ,@('rect overlap', $overlapRect, 'sourceRect')
    $gapRect = Copy-ManifestObject $validManifest; $gapRect.poses[0].sourceRect.width = 79; $manifestCases += ,@('rect gap', $gapRect, 'sourceRect')
    $outOfRangeRect = Copy-ManifestObject $validManifest; $outOfRangeRect.poses[19].sourceRect.width = 81; $manifestCases += ,@('rect out of range', $outOfRangeRect, 'sourceRect')

    $caseIndex = 0
    foreach ($case in $manifestCases) {
        $caseLayout = Join-Path $tempRoot ("invalid-layout-$caseIndex.json")
        $caseOutput = Join-Path $tempRoot ("invalid-output-$caseIndex.png")
        Write-JsonFile $case[1] $caseLayout
        Assert-Failure (Invoke-RepackTool $validSource $caseLayout $caseOutput) $case[0] $case[2]
        Assert-True (-not (Test-Path -LiteralPath $caseOutput)) "$($case[0]) created an output."
        $caseIndex++
    }

    $imageCases = @(
        @('Empty', 'Empty pose'),
        @('Oversize', '308x308'),
        @('Boundary', 'boundary band'),
        @('LowCoverage', 'coverage'),
        @('HighCoverage', 'coverage'),
        @('SoftAlpha', 'Hard alpha')
    )
    foreach ($case in $imageCases) {
        $caseSource = Join-Path $tempRoot ("$($case[0])-source.png")
        $caseShape = New-SyntheticSource -Path $caseSource -Mode $case[0]
        $caseManifest = New-SyntheticManifest -SourcePath $caseSource -SourceCellWidth $caseShape.CellWidth -SourceCellHeight $caseShape.CellHeight
        $caseLayout = Join-Path $tempRoot ("$($case[0])-layout.json")
        $caseOutput = Join-Path $tempRoot ("$($case[0])-output.png")
        Write-JsonFile $caseManifest $caseLayout
        Assert-Failure (Invoke-RepackTool $caseSource $caseLayout $caseOutput) $case[0] $case[1]
        Assert-True (-not (Test-Path -LiteralPath $caseOutput)) "$($case[0]) created an output."
    }

    $preservedOutput = Join-Path $tempRoot 'preserved-output.png'
    [IO.File]::WriteAllBytes($preservedOutput, [byte[]](1, 2, 3, 4, 5))
    $preservedSha = Get-UpperSha256 $preservedOutput
    $badForPreservation = Copy-ManifestObject $validManifest; $badForPreservation.source.sha256 = ('F' * 64)
    $badPreservationLayout = Join-Path $tempRoot 'bad-preservation-layout.json'
    Write-JsonFile $badForPreservation $badPreservationLayout
    Assert-Failure (Invoke-RepackTool $validSource $badPreservationLayout $preservedOutput -Force) 'failed run output preservation' 'Source SHA-256 mismatch'
    Assert-True ((Get-UpperSha256 $preservedOutput) -ceq $preservedSha) 'Failed run changed the existing output.'
    }

    if ($TargetedCase -eq 'SyntheticFull') {
        Write-Output 'PASS: full synthetic contracts'
        return
    }

    Assert-True (Test-Path -LiteralPath $RealSourcePath -PathType Leaf) 'Real source not found.'
    Assert-True (Test-Path -LiteralPath $LayoutPath -PathType Leaf) 'Real layout not found.'
    $realShaBefore = Get-UpperSha256 $RealSourcePath
    Assert-True ($realShaBefore -ceq $ExpectedRealSourceSha256) 'Real source SHA-256 does not match the independent expected value.'
    $realOutputA = Join-Path $tempRoot 'real-a.png'
    $realOutputB = Join-Path $tempRoot 'real-b.png'
    Assert-Pass (Invoke-RepackTool $RealSourcePath $LayoutPath $realOutputA) 'real candidate run A'
    Assert-Pass (Invoke-RepackTool $RealSourcePath $LayoutPath $realOutputB) 'real candidate run B'
    Assert-True ((Get-UpperSha256 $RealSourcePath) -ceq $realShaBefore) 'Real source bytes changed.'
    Assert-True ((Get-UpperSha256 $realOutputA) -ceq (Get-UpperSha256 $realOutputB)) 'Repeated real output SHA differs.'
    [A01.RepackTests.ImageAssertions]::AssertOutputContract($realOutputA, 1280, 1600, 4, 5, 320, 320, 6, 0.05, 0.60)
    [A01.RepackTests.ImageAssertions]::AssertReferenceExactMapping(
        $RealSourcePath, $realOutputA,
        @(0, 281, 561, 842, 1122), @(0, 318, 591, 847, 1107, 1402),
        @(140, 421, 701, 982), 160, 306, @(), @(), 1,
        $(if ($TargetedCase -in @('NonlinearFringe', 'ResidualPink', 'Real', 'Full')) { 4554 } else { -1 }),
        $(if ($TargetedCase -in @('NonlinearFringe', 'ResidualPink', 'Real', 'Full')) { 11 } else { -1 }))

    if ($TargetedCase -eq 'NonlinearFringe') {
        Write-Output 'PASS: targeted nonlinear fringe cleanup contract'
        return
    }

    if ($TargetedCase -eq 'ResidualPink') {
        Write-Output 'PASS: targeted residual pink boundary cleanup contract'
        return
    }

    Write-Output "PASS: Repack-ChromaPoseGrid synthetic and real contracts; candidate-sha256: $(Get-UpperSha256 $realOutputA)"
}
finally {
    if (Test-Path -LiteralPath $tempRoot) {
        Remove-Item -LiteralPath $tempRoot -Recurse -Force
    }
}
