[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$InputPath,

    [Parameter(Mandatory = $true)]
    [string]$LayoutPath,

    [Parameter(Mandatory = $true)]
    [string]$OutputPath,

    [switch]$Force
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$temporaryOutputPath = $null
$replacementBackupPath = $null

try {
    if (-not (Test-Path -LiteralPath $InputPath -PathType Leaf)) { throw "Input not found: $InputPath" }
    if (-not (Test-Path -LiteralPath $LayoutPath -PathType Leaf)) { throw "Layout not found: $LayoutPath" }

    $resolvedInputPath = (Resolve-Path -LiteralPath $InputPath).Path
    $resolvedLayoutPath = (Resolve-Path -LiteralPath $LayoutPath).Path
    $resolvedOutputPath = [IO.Path]::GetFullPath($OutputPath)
    if ($resolvedOutputPath -ieq $resolvedInputPath -or $resolvedOutputPath -ieq $resolvedLayoutPath) {
        throw 'Output path conflicts with an input path.'
    }

    $outputDirectory = [IO.Path]::GetDirectoryName($resolvedOutputPath)
    if ([string]::IsNullOrWhiteSpace($outputDirectory) -or -not (Test-Path -LiteralPath $outputDirectory -PathType Container)) {
        throw "Output directory not found: $outputDirectory"
    }
    if ((Test-Path -LiteralPath $resolvedOutputPath) -and -not $Force) {
        throw 'Output already exists; use -Force to approve replacement.'
    }

    $layout = Get-Content -Raw -LiteralPath $resolvedLayoutPath | ConvertFrom-Json -Depth 20
    if ($layout.PSObject.Properties.Name -cnotcontains 'schemaVersion' -or [int]$layout.schemaVersion -ne 1) {
        throw 'schemaVersion must be 1.'
    }
    foreach ($section in @('source', 'sourceCuts', 'output', 'matte', 'coverage', 'targetAnchor', 'poses')) {
        if ($layout.PSObject.Properties.Name -cnotcontains $section -or $null -eq $layout.$section) {
            throw "Layout is missing required section: $section."
        }
    }

    $sourceShaBefore = (Get-FileHash -Algorithm SHA256 -LiteralPath $resolvedInputPath).Hash.ToUpperInvariant()
    $manifestSha = [string]$layout.source.sha256
    if ($manifestSha -cnotmatch '^[0-9A-F]{64}$' -or $sourceShaBefore -cne $manifestSha) {
        throw 'Source SHA-256 mismatch.'
    }
    $sourceItem = Get-Item -LiteralPath $resolvedInputPath
    if ($layout.source.PSObject.Properties.Name -cnotcontains 'byteLength') {
        throw 'Source byteLength is required.'
    }
    if ([long]$layout.source.byteLength -ne $sourceItem.Length) {
        throw 'Source byte length mismatch.'
    }

    $sourceWidth = [int]$layout.source.width
    $sourceHeight = [int]$layout.source.height
    $outputWidth = [int]$layout.output.width
    $outputHeight = [int]$layout.output.height
    $columns = [int]$layout.output.grid.columns
    $rows = [int]$layout.output.grid.rows
    $cellWidth = [int]$layout.output.grid.cellWidth
    $cellHeight = [int]$layout.output.grid.cellHeight
    $boundaryBand = [int]$layout.boundaryBand
    $minimumCoverage = [double]$layout.coverage.minimum
    $maximumCoverage = [double]$layout.coverage.maximum
    $targetAnchorX = [int]$layout.targetAnchor.x
    $targetAnchorY = [int]$layout.targetAnchor.y

    if ($layout.sourceCuts.PSObject.Properties.Name -cnotcontains 'x' -or
        $layout.sourceCuts.PSObject.Properties.Name -cnotcontains 'y') {
        throw 'sourceCuts.x and sourceCuts.y are required.'
    }
    $sourceCutsX = @($layout.sourceCuts.x | ForEach-Object { [int]$_ })
    $sourceCutsY = @($layout.sourceCuts.y | ForEach-Object { [int]$_ })
    if ($sourceCutsX.Count -ne 5 -or $sourceCutsX[0] -ne 0 -or $sourceCutsX[4] -ne $sourceWidth) {
        throw 'sourceCuts.x must contain 5 cuts from 0 through source width.'
    }
    if ($sourceCutsY.Count -ne 6 -or $sourceCutsY[0] -ne 0 -or $sourceCutsY[5] -ne $sourceHeight) {
        throw 'sourceCuts.y must contain 6 cuts from 0 through source height.'
    }
    for ($index = 1; $index -lt $sourceCutsX.Count; $index++) {
        if ($sourceCutsX[$index] -le $sourceCutsX[$index - 1]) { throw 'sourceCuts.x must be strictly increasing.' }
    }
    for ($index = 1; $index -lt $sourceCutsY.Count; $index++) {
        if ($sourceCutsY[$index] -le $sourceCutsY[$index - 1]) { throw 'sourceCuts.y must be strictly increasing.' }
    }

    if ($outputWidth -ne 1280 -or $outputHeight -ne 1600 -or $columns -ne 4 -or $rows -ne 5 -or
        $cellWidth -ne 320 -or $cellHeight -ne 320 -or $outputWidth -ne $columns * $cellWidth -or
        $outputHeight -ne $rows * $cellHeight) {
        throw 'Output/grid contract mismatch.'
    }
    if ($boundaryBand -ne 6) { throw 'Boundary band must be 6.' }
    if ($minimumCoverage -ne 0.05 -or $maximumCoverage -ne 0.60) { throw 'Coverage contract must be 0.05..0.60.' }
    if ($targetAnchorX -ne 160 -or $targetAnchorY -ne 306) { throw 'Target anchor must be [160,306].' }
    if ([string]$layout.matte.distanceMetric -cne 'chebyshev' -or [int]$layout.matte.tolerance -ne 96 -or
        [int]$layout.matte.connectivity -ne 8 -or $layout.matte.keyColor.Count -ne 3 -or
        [int]$layout.matte.keyColor[0] -ne 255 -or [int]$layout.matte.keyColor[1] -ne 0 -or
        [int]$layout.matte.keyColor[2] -ne 255) {
        throw 'Matte contract mismatch.'
    }

    $expectedPoseIds = @(
        'p1_idle', 'p1_speak', 'p1_laugh', 'p1_rise',
        'p2_idle', 'p2_nod', 'p2_laugh', 'p2_hold',
        'p3_work', 'p3_shoulder_laugh', 'p3_head_turn', 'p3_hold',
        'p4_idle', 'p4_gesture', 'p4_exit_turn', 'p4_hold',
        'p5_idle', 'p5_laugh', 'p5_step_ready', 'p5_hold'
    )
    if ($layout.poses.Count -ne 20) { throw 'Layout must contain exactly 20 poses.' }

    Add-Type -AssemblyName System.Drawing
    if (-not ('A01.Repack.PoseSpec' -as [type])) {
        Add-Type -TypeDefinition @'
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace A01.Repack
{
    public sealed class PoseSpec
    {
        public string Id;
        public int X, Y, Width, Height, SourceAxisX, TargetRow, TargetColumn;
        public PoseSpec(string id, int x, int y, int width, int height, int sourceAxisX, int targetRow, int targetColumn)
        {
            Id = id; X = x; Y = y; Width = width; Height = height;
            SourceAxisX = sourceAxisX; TargetRow = targetRow; TargetColumn = targetColumn;
        }
    }

    public static class Engine
    {
        private static int KeyDistance(int argb, int keyR, int keyG, int keyB)
        {
            var r = (argb >> 16) & 255;
            var g = (argb >> 8) & 255;
            var b = argb & 255;
            return Math.Max(Math.Max(Math.Abs(r - keyR), Math.Abs(g - keyG)), Math.Abs(b - keyB));
        }

        private static int[] ReadPixels(Bitmap bitmap)
        {
            var pixels = new int[bitmap.Width * bitmap.Height];
            for (var y = 0; y < bitmap.Height; y++)
            for (var x = 0; x < bitmap.Width; x++)
                pixels[y * bitmap.Width + x] = bitmap.GetPixel(x, y).ToArgb();
            return pixels;
        }

        private static void WritePixels(string path, int width, int height, int[] pixels)
        {
            using (var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                var rectangle = new Rectangle(0, 0, width, height);
                var data = bitmap.LockBits(rectangle, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                try
                {
                    for (var y = 0; y < height; y++)
                        Marshal.Copy(pixels, y * width, IntPtr.Add(data.Scan0, y * data.Stride), width);
                }
                finally { bitmap.UnlockBits(data); }
                bitmap.Save(path, ImageFormat.Png);
            }
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

        private static bool HasCleanupFringeRgb(int argb)
        {
            return HasNonlinearFringeRgb(argb) || HasResidualPinkFringeRgb(argb);
        }

        private static bool IsNonlinearFringeCandidate(int[] pixels, int width, int height, int index)
        {
            var argb = pixels[index];
            if (((argb >> 24) & 255) != 255) return false;
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
            if (!touchesTransparent) return false;
            return HasCleanupFringeRgb(argb);
        }

        private static void CleanNonlinearFringes(int[] pixels, int width, int height)
        {
            var candidates = new bool[pixels.Length];
            for (var index = 0; index < pixels.Length; index++)
                candidates[index] = IsNonlinearFringeCandidate(pixels, width, height, index);

            var replacementRgb = new int[pixels.Length];
            for (var index = 0; index < pixels.Length; index++)
            {
                if (!candidates[index]) continue;
                var x = index % width;
                var y = index / width;
                var donorIndex = -1;
                var bestDistanceSquared = int.MaxValue;
                var bestY = int.MaxValue;
                var bestX = int.MaxValue;
                for (var offsetY = -8; offsetY <= 8; offsetY++)
                for (var offsetX = -8; offsetX <= 8; offsetX++)
                {
                    var distanceSquared = offsetX * offsetX + offsetY * offsetY;
                    if (distanceSquared == 0 || distanceSquared > 64) continue;
                    var nextX = x + offsetX;
                    var nextY = y + offsetY;
                    if (nextX < 0 || nextX >= width || nextY < 0 || nextY >= height) continue;
                    var nextIndex = nextY * width + nextX;
                    if (((pixels[nextIndex] >> 24) & 255) != 255 || candidates[nextIndex] ||
                        HasCleanupFringeRgb(pixels[nextIndex])) continue;
                    if (distanceSquared < bestDistanceSquared ||
                        (distanceSquared == bestDistanceSquared && (nextY < bestY || (nextY == bestY && nextX < bestX))))
                    {
                        donorIndex = nextIndex;
                        bestDistanceSquared = distanceSquared;
                        bestY = nextY;
                        bestX = nextX;
                    }
                }
                if (donorIndex < 0) throw new InvalidOperationException("Nonlinear fringe candidate has no donor.");
                replacementRgb[index] = pixels[donorIndex] & 0x00FFFFFF;
            }

            for (var index = 0; index < pixels.Length; index++)
                if (candidates[index]) pixels[index] = (pixels[index] & unchecked((int)0xFF000000)) | replacementRgb[index];
        }

        public static void Process(
            string inputPath, string temporaryOutputPath,
            int sourceWidth, int sourceHeight, int outputWidth, int outputHeight,
            int columns, int rows, int cellWidth, int cellHeight,
            int keyR, int keyG, int keyB, int tolerance, int boundaryBand,
            double minimumCoverage, double maximumCoverage,
            int targetAnchorX, int targetAnchorY, PoseSpec[] poses)
        {
            int[] sourcePixels;
            using (var source = new Bitmap(inputPath))
            {
                if (source.Width != sourceWidth || source.Height != sourceHeight)
                    throw new InvalidOperationException("Source canvas mismatch.");
                sourcePixels = ReadPixels(source);
            }

            var pixelCount = checked(sourceWidth * sourceHeight);
            var removed = new bool[pixelCount];
            var queue = new int[pixelCount];
            var head = 0;
            var tail = 0;
            for (var index = 0; index < pixelCount; index++)
            {
                if (KeyDistance(sourcePixels[index], keyR, keyG, keyB) <= 24)
                {
                    removed[index] = true;
                    queue[tail++] = index;
                }
            }
            Action<int> enqueueFlood = index =>
            {
                if (!removed[index] && KeyDistance(sourcePixels[index], keyR, keyG, keyB) <= 48)
                {
                    removed[index] = true;
                    queue[tail++] = index;
                }
            };
            while (head < tail)
            {
                var index = queue[head++];
                var x = index % sourceWidth;
                var y = index / sourceWidth;
                if (x > 0) enqueueFlood(index - 1);
                if (x + 1 < sourceWidth) enqueueFlood(index + 1);
                if (y > 0) enqueueFlood(index - sourceWidth);
                if (y + 1 < sourceHeight) enqueueFlood(index + sourceWidth);
            }

            var nearMask = new bool[pixelCount];
            for (var y = 0; y < sourceHeight; y++)
            for (var x = 0; x < sourceWidth; x++)
            {
                var index = y * sourceWidth + x;
                if (!removed[index]) continue;
                for (var offsetY = -2; offsetY <= 2; offsetY++)
                for (var offsetX = -2; offsetX <= 2; offsetX++)
                {
                    var nextX = x + offsetX;
                    var nextY = y + offsetY;
                    if (nextX >= 0 && nextX < sourceWidth && nextY >= 0 && nextY < sourceHeight)
                        nearMask[nextY * sourceWidth + nextX] = true;
                }
            }

            var processedPixels = (int[])sourcePixels.Clone();
            for (var index = 0; index < pixelCount; index++)
            {
                if (removed[index] || !nearMask[index]) continue;
                var x = index % sourceWidth;
                var y = index / sourceWidth;
                var donorIndex = -1;
                var donorDistanceSquared = int.MaxValue;
                var donorY = int.MaxValue;
                var donorX = int.MaxValue;
                for (var offsetY = -8; offsetY <= 8; offsetY++)
                for (var offsetX = -8; offsetX <= 8; offsetX++)
                {
                    var distanceSquared = offsetX * offsetX + offsetY * offsetY;
                    if (distanceSquared == 0 || distanceSquared > 64) continue;
                    var nextX = x + offsetX;
                    var nextY = y + offsetY;
                    if (nextX < 0 || nextX >= sourceWidth || nextY < 0 || nextY >= sourceHeight) continue;
                    var nextIndex = nextY * sourceWidth + nextX;
                    if (removed[nextIndex] || nearMask[nextIndex] ||
                        KeyDistance(sourcePixels[nextIndex], keyR, keyG, keyB) <= tolerance) continue;
                    if (distanceSquared < donorDistanceSquared ||
                        (distanceSquared == donorDistanceSquared && (nextY < donorY ||
                        (nextY == donorY && nextX < donorX))))
                    {
                        donorIndex = nextIndex;
                        donorDistanceSquared = distanceSquared;
                        donorY = nextY;
                        donorX = nextX;
                    }
                }
                if (donorIndex < 0) continue;

                var candidate = sourcePixels[index];
                var donor = sourcePixels[donorIndex];
                var candidateR = (candidate >> 16) & 255;
                var candidateG = (candidate >> 8) & 255;
                var candidateB = candidate & 255;
                var donorR = (donor >> 16) & 255;
                var donorG = (donor >> 8) & 255;
                var donorB = donor & 255;
                var vectorR = keyR - donorR;
                var vectorG = keyG - donorG;
                var vectorB = keyB - donorB;
                var denominator = vectorR * vectorR + vectorG * vectorG + vectorB * vectorB;
                if (denominator == 0) continue;
                var projection = ((candidateR - donorR) * vectorR +
                    (candidateG - donorG) * vectorG +
                    (candidateB - donorB) * vectorB) / (double)denominator;
                if (projection < 0.08 || projection > 0.92) continue;
                var residual = Math.Max(
                    Math.Max(Math.Abs(candidateR - (donorR + projection * vectorR)),
                             Math.Abs(candidateG - (donorG + projection * vectorG))),
                    Math.Abs(candidateB - (donorB + projection * vectorB)));
                if (residual > 24.0) continue;
                processedPixels[index] = (candidate & unchecked((int)0xFF000000)) |
                    (donorR << 16) | (donorG << 8) | donorB;
            }

            var ownership = new byte[pixelCount];
            foreach (var pose in poses)
            {
                if (pose.Width <= 0 || pose.Height <= 0 || pose.X < 0 || pose.Y < 0 ||
                    pose.X > sourceWidth - pose.Width || pose.Y > sourceHeight - pose.Height)
                    throw new InvalidOperationException("Source rect is out of range.");
                for (var y = pose.Y; y < pose.Y + pose.Height; y++)
                for (var x = pose.X; x < pose.X + pose.Width; x++)
                {
                    var index = y * sourceWidth + x;
                    ownership[index]++;
                    if (ownership[index] > 1)
                        throw new InvalidOperationException("Source rect ownership violation: overlap.");
                }
            }
            for (var index = 0; index < ownership.Length; index++)
                if (ownership[index] != 1)
                    throw new InvalidOperationException("Source rect ownership violation: gap.");

            var outputPixels = new int[checked(outputWidth * outputHeight)];
            var cellArea = cellWidth * cellHeight;
            foreach (var pose in poses)
            {
                var retainedCount = 0;
                var minX = int.MaxValue;
                var minY = int.MaxValue;
                var maxX = int.MinValue;
                var maxY = int.MinValue;
                for (var y = pose.Y; y < pose.Y + pose.Height; y++)
                for (var x = pose.X; x < pose.X + pose.Width; x++)
                {
                    var sourceIndex = y * sourceWidth + x;
                    if (removed[sourceIndex]) continue;
                    var alpha = (sourcePixels[sourceIndex] >> 24) & 255;
                    if (alpha != 255)
                        throw new InvalidOperationException("Hard alpha violation in retained source pixels.");
                    retainedCount++;
                    minX = Math.Min(minX, x); minY = Math.Min(minY, y);
                    maxX = Math.Max(maxX, x); maxY = Math.Max(maxY, y);
                }
                if (retainedCount == 0)
                    throw new InvalidOperationException("Empty pose union.");
                if (maxX - minX + 1 > 308 || maxY - minY + 1 > 308)
                    throw new InvalidOperationException("Pose union exceeds 308x308.");

                var coverage = (double)retainedCount / cellArea;
                if (coverage < minimumCoverage || coverage > maximumCoverage)
                    throw new InvalidOperationException("Pose coverage is outside 0.05..0.60.");

                var dx = pose.TargetColumn * cellWidth + targetAnchorX - pose.SourceAxisX;
                var dy = pose.TargetRow * cellHeight + targetAnchorY - maxY;
                for (var y = pose.Y; y < pose.Y + pose.Height; y++)
                for (var x = pose.X; x < pose.X + pose.Width; x++)
                {
                    var sourceIndex = y * sourceWidth + x;
                    if (removed[sourceIndex]) continue;
                    var destinationX = x + dx;
                    var destinationY = y + dy;
                    var cellLeft = pose.TargetColumn * cellWidth;
                    var cellTop = pose.TargetRow * cellHeight;
                    if (destinationX < cellLeft || destinationX >= cellLeft + cellWidth ||
                        destinationY < cellTop || destinationY >= cellTop + cellHeight)
                        throw new InvalidOperationException("Translated pose leaves its target cell.");
                    var localX = destinationX - cellLeft;
                    var localY = destinationY - cellTop;
                    if (localX < boundaryBand || localX >= cellWidth - boundaryBand ||
                        localY < boundaryBand || localY >= cellHeight - boundaryBand)
                        throw new InvalidOperationException("Translated pose enters the 6px boundary band.");
                    var destinationIndex = destinationY * outputWidth + destinationX;
                    if (outputPixels[destinationIndex] != 0)
                        throw new InvalidOperationException("Unexpected translated-pixel collision.");
                    outputPixels[destinationIndex] = processedPixels[sourceIndex];
                }
            }

            CleanNonlinearFringes(outputPixels, outputWidth, outputHeight);
            WritePixels(temporaryOutputPath, outputWidth, outputHeight, outputPixels);
            using (var verification = new Bitmap(temporaryOutputPath))
            {
                if (verification.Width != outputWidth || verification.Height != outputHeight)
                    throw new InvalidOperationException("Temporary output canvas mismatch.");
                var encodedPixels = ReadPixels(verification);
                for (var index = 0; index < encodedPixels.Length; index++)
                {
                    var argb = encodedPixels[index];
                    var alpha = (argb >> 24) & 255;
                    if (alpha != 0 && alpha != 255)
                        throw new InvalidOperationException("Temporary output alpha is not hard alpha.");
                    if (alpha == 0 && (argb & 0x00FFFFFF) != 0)
                        throw new InvalidOperationException("Temporary output transparent RGB is not black.");
                    if (argb != outputPixels[index])
                        throw new InvalidOperationException("Temporary output pixel-preservation mismatch.");
                }
            }
        }
    }
}
'@ -ReferencedAssemblies @('System.Drawing.Common', 'System.Drawing.Primitives', 'System.Private.Windows.GdiPlus', 'System.Private.Windows.Core')
    }

    $poseSpecs = [A01.Repack.PoseSpec[]]::new(20)
    $targetOwnership = [bool[]]::new(20)
    $seenPoseIds = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    for ($index = 0; $index -lt 20; $index++) {
        $pose = $layout.poses[$index]
        if ($null -eq $pose.target) { throw "Pose target is missing at index $index." }
        $id = [string]$pose.id
        if ($id -cne $expectedPoseIds[$index] -or -not $seenPoseIds.Add($id)) { throw "Pose ID mismatch or duplicate at index $index." }
        if ($null -eq $pose.sourceRect) { throw "Pose sourceRect is missing at index $index." }
        if ([string]$pose.sourceGroundY -cne 'maxRetainedY') { throw "Pose sourceGroundY must be maxRetainedY at index $index." }
        $targetRow = [int]$pose.target.row
        $targetColumn = [int]$pose.target.column
        $expectedRow = [int][Math]::Floor($index / 4)
        $expectedColumn = $index % 4
        if ($targetRow -ne $expectedRow -or $targetColumn -ne $expectedColumn) {
            throw "Pose target must match its fixed row-major slot at index $index."
        }
        if ($targetRow -lt 0 -or $targetRow -ge 5 -or $targetColumn -lt 0 -or $targetColumn -ge 4) {
            throw "Pose target is out of range at index $index."
        }
        $targetIndex = $targetRow * 4 + $targetColumn
        if ($targetOwnership[$targetIndex]) { throw "Target cell duplicate at index $index." }
        $targetOwnership[$targetIndex] = $true
        if ($null -eq $layout.sourceAxesX -or $layout.sourceAxesX.Count -ne 4 -or
            [int]$pose.sourceAxisX -ne [int]$layout.sourceAxesX[$targetColumn]) {
            throw "Pose source axis mismatch at index $index."
        }
        $rect = $pose.sourceRect
        if ([int]$rect.x -ne $sourceCutsX[$expectedColumn] -or
            [int]$rect.y -ne $sourceCutsY[$expectedRow] -or
            [int]$rect.width -ne ($sourceCutsX[$expectedColumn + 1] - $sourceCutsX[$expectedColumn]) -or
            [int]$rect.height -ne ($sourceCutsY[$expectedRow + 1] - $sourceCutsY[$expectedRow])) {
            throw "Pose sourceRect must equal adjacent sourceCuts at index $index."
        }
        $poseSpecs[$index] = [A01.Repack.PoseSpec]::new(
            $id, [int]$rect.x, [int]$rect.y, [int]$rect.width, [int]$rect.height,
            [int]$pose.sourceAxisX, $targetRow, $targetColumn)
    }
    if ($targetOwnership -contains $false) { throw 'Target cell ownership is incomplete.' }

    $temporaryOutputPath = Join-Path $outputDirectory ('.' + [IO.Path]::GetFileName($resolvedOutputPath) + '.' + [Guid]::NewGuid().ToString('N') + '.tmp.png')
    [A01.Repack.Engine]::Process(
        $resolvedInputPath, $temporaryOutputPath,
        $sourceWidth, $sourceHeight, $outputWidth, $outputHeight,
        $columns, $rows, $cellWidth, $cellHeight,
        255, 0, 255, 96, $boundaryBand,
        $minimumCoverage, $maximumCoverage,
        $targetAnchorX, $targetAnchorY, $poseSpecs)

    $sourceShaAfter = (Get-FileHash -Algorithm SHA256 -LiteralPath $resolvedInputPath).Hash.ToUpperInvariant()
    $sourceItemAfter = Get-Item -LiteralPath $resolvedInputPath
    if ($sourceShaAfter -cne $sourceShaBefore -or $sourceItemAfter.Length -ne $sourceItem.Length) {
        throw 'Source immutability violation.'
    }

    if (Test-Path -LiteralPath $resolvedOutputPath) {
        $replacementBackupPath = Join-Path $outputDirectory ('.' + [IO.Path]::GetFileName($resolvedOutputPath) + '.' + [Guid]::NewGuid().ToString('N') + '.bak')
        [IO.File]::Replace($temporaryOutputPath, $resolvedOutputPath, $replacementBackupPath)
        $temporaryOutputPath = $null
        try {
            [IO.File]::Delete($replacementBackupPath)
            $replacementBackupPath = $null
        }
        catch {
            $backupDeleteError = $_
            $failedCandidatePath = Join-Path $outputDirectory ('.' + [IO.Path]::GetFileName($resolvedOutputPath) + '.' + [Guid]::NewGuid().ToString('N') + '.rollback.png')
            [IO.File]::Replace($replacementBackupPath, $resolvedOutputPath, $failedCandidatePath)
            $replacementBackupPath = $null
            if (Test-Path -LiteralPath $failedCandidatePath) { [IO.File]::Delete($failedCandidatePath) }
            throw $backupDeleteError
        }
    }
    else {
        [IO.File]::Move($temporaryOutputPath, $resolvedOutputPath)
        $temporaryOutputPath = $null
    }
    $outputSha = (Get-FileHash -Algorithm SHA256 -LiteralPath $resolvedOutputPath).Hash.ToUpperInvariant()
    Write-Output "PASS: Repack-ChromaPoseGrid; candidate-sha256: $outputSha"
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
