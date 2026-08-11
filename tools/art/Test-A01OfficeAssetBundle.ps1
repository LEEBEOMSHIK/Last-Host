[CmdletBinding()]
param(
    [string]$BundleRoot = 'UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Office'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Assert-True {
    param([Parameter(Mandatory = $true)][bool]$Condition, [Parameter(Mandatory = $true)][string]$Message)
    if (-not $Condition) { throw $Message }
}

function Get-UpperSha256 {
    param([Parameter(Mandatory = $true)][string]$Path)
    return (Get-FileHash -LiteralPath $Path -Algorithm SHA256 -ErrorAction Stop).Hash.ToUpperInvariant()
}

function Assert-Png {
    param([Parameter(Mandatory = $true)][string]$Path, [int]$Width, [int]$Height)
    $bytes = [IO.File]::ReadAllBytes($Path)
    Assert-True ($bytes.Length -ge 24) "PNG is too short: $Path"
    $signature = [byte[]](137, 80, 78, 71, 13, 10, 26, 10)
    for ($index = 0; $index -lt $signature.Length; $index++) {
        Assert-True ($bytes[$index] -eq $signature[$index]) "PNG signature mismatch: $Path"
    }
    $actualWidth = ([uint32]$bytes[16] -shl 24) -bor ([uint32]$bytes[17] -shl 16) -bor ([uint32]$bytes[18] -shl 8) -bor [uint32]$bytes[19]
    $actualHeight = ([uint32]$bytes[20] -shl 24) -bor ([uint32]$bytes[21] -shl 16) -bor ([uint32]$bytes[22] -shl 8) -bor [uint32]$bytes[23]
    Assert-True ($actualWidth -eq $Width -and $actualHeight -eq $Height) "PNG dimensions mismatch: $Path"
}

function Assert-CSharpImportContract {
    param([Parameter(Mandatory = $true)][string]$Path)
    Assert-True (Test-Path -LiteralPath $Path -PathType Leaf) "A01 Office C# import test missing: $Path"
    $source = Get-Content -Raw -LiteralPath $Path
    Assert-True ($source -notmatch 'spriteGenerateFallbackPhysicsShape') 'C# import contract references unsupported spriteGenerateFallbackPhysicsShape.'
    Assert-True ($source -notmatch 'System\.Reflection|BindingFlags|FieldInfo|PropertyInfo|MethodInfo|MemberInfo|\.GetField\(|\.GetProperty\(|\.GetMethod\(|SerializedObject|SerializedProperty|GetType\(') 'C# import contract uses unsupported reflection or serialized internals.'
    Assert-True ($source -match 'AssetDatabase\.LoadAssetAtPath<Sprite>\(BackgroundPath\)') 'C# import contract must load the background Sprite directly.'
    Assert-True ($source -match 'backgroundSprite\.GetPhysicsShapeCount\(\)') 'C# import contract must check the background physics shape count directly.'
    Assert-True ($source -match 'AssetDatabase\.LoadAssetAtPath<Sprite>\(OcclusionMaskPath\)') 'C# import contract must load the occlusion mask Sprite directly.'
    Assert-True ($source -match 'occlusionMaskSprite\.GetPhysicsShapeCount\(\)') 'C# import contract must check the occlusion mask physics shape count directly.'
    Assert-True ($source -match 'sprites\.Length, Is\.EqualTo\(20\)') 'C# import contract must assert exactly 20 cast sprites.'
    Assert-True ($source -match 'FrameNames\.Length, Is\.EqualTo\(20\)') 'C# import contract must assert exactly 20 frame names.'
    Assert-True ($source -match 'sprite\.GetPhysicsShapeCount\(\)') 'C# import contract must check every cast sprite physics shape count directly.'
}

function Assert-Unity6000NestedSpriteRects {
    param(
        [Parameter(Mandatory = $true)][string]$Meta,
        [Parameter(Mandatory = $true)][object[]]$ExpectedFrames
    )

    $nestedRectPattern = '(?m)^      rect:\r?\n        serializedVersion: 2\r?\n        x: \d+\r?\n        y: \d+\r?\n        width: 320\r?\n        height: 320$'
    Assert-True (([regex]::Matches($Meta, $nestedRectPattern)).Count -eq 20) 'Cast .meta must contain exactly 20 Unity 6000 nested 320x320 rect nodes.'
    Assert-True (-not ($Meta -match '(?m)^      rect: \{x:')) 'Cast .meta must not contain inline rect nodes.'

    foreach ($expected in $ExpectedFrames) {
        $framePattern = '(?ms)^    - serializedVersion: 2\r?\n      name: ' + [regex]::Escape([string]$expected[0]) + '\r?\n      rect:\r?\n        serializedVersion: 2\r?\n        x: ' + $expected[1] + '\r?\n        y: ' + $expected[2] + '\r?\n        width: 320\r?\n        height: 320$'
        Assert-True ($Meta -match $framePattern) "Cast .meta Unity 6000 nested rect mismatch for $($expected[0])."
    }
}

function Assert-A01OfficeFolderMetaChain {
    param([Parameter(Mandatory = $true)][string]$BundleRoot)

    $officeDirectory = $BundleRoot.TrimEnd('\', '/')
    $a01Directory = Split-Path -Path $officeDirectory -Parent
    $openingDirectory = Split-Path -Path $a01Directory -Parent
    $cinematicsDirectory = Split-Path -Path $openingDirectory -Parent
    $folderMetaContracts = @(
        [pscustomobject]@{ Path = "$cinematicsDirectory.meta"; Guid = '035dd97e96dfe884eb47bd2d34285fd3' },
        [pscustomobject]@{ Path = "$openingDirectory.meta"; Guid = '61f28b1b310660e42948f91404d7c2d2' },
        [pscustomobject]@{ Path = "$a01Directory.meta"; Guid = '422a6e70bc3ec4e4cb4247f9c298c1b4' },
        [pscustomobject]@{ Path = "$officeDirectory.meta"; Guid = 'a010ff1ce0004000b000000000000001' }
    )
    $guids = @($folderMetaContracts | ForEach-Object { $_.Guid })
    Assert-True (($guids | Select-Object -Unique).Count -eq 4) 'A01 Office folder meta GUIDs must be unique.'

    foreach ($contract in $folderMetaContracts) {
        Assert-True (Test-Path -LiteralPath $contract.Path -PathType Leaf) "A01 Office folder meta missing: $($contract.Path)"
        $actual = (Get-Content -Raw -LiteralPath $contract.Path) -replace "`r`n", "`n"
        $expected = "fileFormatVersion: 2`nguid: $($contract.Guid)`nfolderAsset: yes`nDefaultImporter:`n  externalObjects: {}`n  userData: `n  assetBundleName: `n  assetBundleVariant: `n"
        Assert-True ($actual -ceq $expected) "A01 Office folder meta contract mismatch: $($contract.Path)"
    }
}

$expectedRawSha = '24A143D7344DAC8358CD496C6AD03718AADB492D67B96E7CCCF0E46DA08A090D'
$expectedDerivativeSha = '71F6542C8DD6229F40DB8E1CD1DF9A1C7B293FFDB28B172A3C87900465BD365D'
$expectedDerivativeBytes = 1131247
$expectedBackgroundSha = 'DA5F22DE7D1C9BDBABE2A8887640085142D23E02CF3BF94B21E217A7EC98AA0C'
$expectedForegroundSourceSha = 'D782D38E4D510E1D13680C21D6642F86647DF53662B8D94150376EC73770F1E1'
$expectedForegroundSourceBytes = 1097398
$expectedOcclusionMaskSha = 'F59EBC810A943DB76C17691AD364237F473BAB6A97EF3A8966321BAEF8400D95'
$expectedOcclusionMaskBytes = 15067
$expectedOcclusionMaskGuid = 'a010ff1ce0004000b000000000000004'
$rejectedColorDerivativeSha = '3B94269DE3D3CDD41BD534450EF0A6E5CB8E3A64C44316692E639B1A30A4AF4B'
$expectedFrames = @(
    @('p1_seated_idle', 0, 1280), @('p1_speaking', 320, 1280), @('p1_laugh', 640, 1280), @('p1_rise_start', 960, 1280),
    @('p2_seated_idle', 0, 960), @('p2_nod_smile', 320, 960), @('p2_laugh', 640, 960), @('p2_neutral', 960, 960),
    @('p3_seated_work', 0, 640), @('p3_shoulder_laugh', 320, 640), @('p3_head_turn', 640, 640), @('p3_neutral', 960, 640),
    @('p4_standing_idle', 0, 320), @('p4_conversation', 320, 320), @('p4_exit_turn', 640, 320), @('p4_neutral', 960, 320),
    @('p5_standing_idle', 0, 0), @('p5_laugh', 320, 0), @('p5_exit_step', 640, 0), @('p5_neutral', 960, 0)
)

try {
    $repositoryRoot = (Resolve-Path '.').Path
    Assert-True (Test-Path -LiteralPath (Join-Path $repositoryRoot 'UnityProject') -PathType Container) 'Canonical execution cwd must be the repository root.'
    Assert-True (Test-Path -LiteralPath $BundleRoot -PathType Container) "A01 Office asset bundle missing: $BundleRoot"
    Assert-A01OfficeFolderMetaChain $BundleRoot
    $backgroundPath = Join-Path $BundleRoot 'a01-office-background-v1.png'
    $castPath = Join-Path $BundleRoot 'a01-office-cast-poses-v1.png'
    $occlusionMaskPath = Join-Path $BundleRoot 'a01-office-occlusion-mask-v1.png'
    $manifestPath = Join-Path $BundleRoot 'a01-office-assets-v1.manifest.json'
    foreach ($path in @($backgroundPath, $castPath, $occlusionMaskPath, $manifestPath, "$backgroundPath.meta", "$castPath.meta", "$occlusionMaskPath.meta", "$BundleRoot.meta")) {
        Assert-True (Test-Path -LiteralPath $path -PathType Leaf) "A01 Office bundle file missing: $path"
    }

    Assert-Png $backgroundPath 1672 941
    Assert-Png $castPath 1280 1600
    Assert-Png $occlusionMaskPath 1672 941
    Assert-True ((Get-UpperSha256 $backgroundPath) -ceq $expectedBackgroundSha) 'Background is not byte-identical to the approved source.'
    Assert-True ((Get-UpperSha256 $castPath) -ceq $expectedDerivativeSha) 'Cast is not byte-identical to the cleaned derivative.'
    Assert-True ((Get-Item -LiteralPath $castPath).Length -eq $expectedDerivativeBytes) 'Cast byte length mismatch.'
    Assert-True ((Get-UpperSha256 $occlusionMaskPath) -ceq $expectedOcclusionMaskSha) 'Occlusion mask is not byte-identical to the mask derivative.'
    Assert-True ((Get-Item -LiteralPath $occlusionMaskPath).Length -eq $expectedOcclusionMaskBytes) 'Occlusion mask byte length mismatch.'
    $pngNames = @(Get-ChildItem -LiteralPath $BundleRoot -File -Filter '*.png' | Select-Object -ExpandProperty Name | Sort-Object)
    $expectedPngNames = @('a01-office-background-v1.png', 'a01-office-cast-poses-v1.png', 'a01-office-occlusion-mask-v1.png')
    Assert-True ($pngNames.Count -eq $expectedPngNames.Count -and -not (Compare-Object -ReferenceObject $expectedPngNames -DifferenceObject $pngNames)) 'Office production PNG names must contain exactly background, cast, and occlusion mask.'
    Assert-True ((Get-UpperSha256 $backgroundPath) -cne $rejectedColorDerivativeSha -and (Get-UpperSha256 $castPath) -cne $rejectedColorDerivativeSha -and (Get-UpperSha256 $occlusionMaskPath) -cne $rejectedColorDerivativeSha) 'Rejected color derivative SHA must not occur in Office production PNGs.'

    $manifestText = Get-Content -Raw -LiteralPath $manifestPath
    Assert-True ($manifestText -notmatch [regex]::Escape($rejectedColorDerivativeSha)) 'Manifest must not reference the rejected color derivative SHA.'
    Assert-True ($manifestText -notmatch 'a01-office-foreground-mask-alpha\.png') 'Manifest must not reference the rejected color derivative path.'
    $manifest = $manifestText | ConvertFrom-Json
    Assert-True ($manifest.schema_version -eq 1) 'Manifest schema_version must be 1.'
    Assert-True ($manifest.foreground_status -ceq 'mask-only-candidate') 'Foreground must be the approved mask-only candidate.'
    Assert-True ($manifest.sources.raw.sha256 -ceq $expectedRawSha) 'Manifest raw SHA mismatch.'
    Assert-True ($manifest.sources.derived.sha256 -ceq $expectedDerivativeSha) 'Manifest derived SHA mismatch.'
    Assert-True ($manifest.sources.derived.byte_length -eq $expectedDerivativeBytes) 'Manifest derived byte length mismatch.'
    Assert-True ($manifest.sources.background.sha256 -ceq $expectedBackgroundSha) 'Manifest background SHA mismatch.'
    Assert-True ($manifest.sources.foreground_source.path -ceq '_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-foreground-mask-source.png') 'Manifest foreground source path mismatch.'
    Assert-True ($manifest.sources.foreground_source.sha256 -ceq $expectedForegroundSourceSha) 'Manifest foreground source SHA mismatch.'
    Assert-True ($manifest.sources.foreground_source.byte_length -eq $expectedForegroundSourceBytes) 'Manifest foreground source byte length mismatch.'
    Assert-True ($manifest.sources.foreground_derived.path -ceq '_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-occlusion-mask-alpha.png') 'Manifest foreground derivative path mismatch.'
    Assert-True ($manifest.sources.foreground_derived.sha256 -ceq $expectedOcclusionMaskSha) 'Manifest foreground derivative SHA mismatch.'
    Assert-True ($manifest.sources.foreground_derived.byte_length -eq $expectedOcclusionMaskBytes) 'Manifest foreground derivative byte length mismatch.'
    $foregroundSourcePath = Join-Path $repositoryRoot $manifest.sources.foreground_source.path
    $foregroundDerivedPath = Join-Path $repositoryRoot $manifest.sources.foreground_derived.path
    Assert-True (Test-Path -LiteralPath $foregroundSourcePath -PathType Leaf) 'Manifest foreground source path does not resolve from repository root.'
    Assert-True (Test-Path -LiteralPath $foregroundDerivedPath -PathType Leaf) 'Manifest foreground derivative path does not resolve from repository root.'
    Assert-True ((Get-UpperSha256 $foregroundSourcePath) -ceq $expectedForegroundSourceSha) 'Manifest foreground source file SHA mismatch.'
    Assert-True ((Get-Item -LiteralPath $foregroundSourcePath).Length -eq $expectedForegroundSourceBytes) 'Manifest foreground source file byte length mismatch.'
    Assert-True ((Get-UpperSha256 $foregroundDerivedPath) -ceq $expectedOcclusionMaskSha) 'Manifest foreground derivative file SHA mismatch.'
    Assert-True ((Get-Item -LiteralPath $foregroundDerivedPath).Length -eq $expectedOcclusionMaskBytes) 'Manifest foreground derivative file byte length mismatch.'
    Assert-True ((Get-UpperSha256 $occlusionMaskPath) -ceq (Get-UpperSha256 $foregroundDerivedPath)) 'Office occlusion mask must be byte-identical to the actual manifest derivative.'
    Assert-True ($manifest.import.cast.sprite_mode -ceq 'Multiple') 'Cast import mode must be Multiple.'
    Assert-True ($manifest.import.cast.pixels_per_unit -eq 100) 'Cast PPU must be 100.'
    Assert-True ($manifest.import.cast.filter_mode -ceq 'Point') 'Cast filter must be Point.'
    Assert-True (-not $manifest.import.cast.mipmap_enabled -and $manifest.import.cast.compression -ceq 'Uncompressed') 'Cast mipmap/compression contract mismatch.'
    Assert-True ($manifest.import.cast.alpha_transparency -and -not $manifest.import.cast.fallback_physics_shape) 'Cast alpha/fallback physics contract mismatch.'
    Assert-True ($manifest.import.cast.pivot.x -eq 0.5 -and $manifest.import.cast.pivot.y -eq 0.04375) 'Cast pivot contract mismatch.'
    Assert-True ($manifest.import.background.sprite_mode -ceq 'Single' -and $manifest.import.background.pixels_per_unit -eq 100) 'Background sprite/PPU contract mismatch.'
    Assert-True ($manifest.import.occlusion_mask.sprite_mode -ceq 'Single' -and $manifest.import.occlusion_mask.pixels_per_unit -eq 100) 'Occlusion mask sprite/PPU contract mismatch.'
    Assert-True ($manifest.import.occlusion_mask.filter_mode -ceq 'Point' -and -not $manifest.import.occlusion_mask.mipmap_enabled -and $manifest.import.occlusion_mask.compression -ceq 'Uncompressed') 'Occlusion mask filter/mipmap/compression contract mismatch.'
    Assert-True ($manifest.import.occlusion_mask.alpha_transparency -and -not $manifest.import.occlusion_mask.fallback_physics_shape) 'Occlusion mask alpha/fallback physics contract mismatch.'
    Assert-True ($manifest.frames.Count -eq 20) 'Manifest must contain exactly 20 frames.'
    for ($index = 0; $index -lt $expectedFrames.Count; $index++) {
        $expected = $expectedFrames[$index]
        $frame = $manifest.frames[$index]
        Assert-True ($frame.name -ceq $expected[0] -and $frame.rect.x -eq $expected[1] -and $frame.rect.y -eq $expected[2]) "Frame $index mapping mismatch."
        Assert-True ($frame.rect.width -eq 320 -and $frame.rect.height -eq 320) "Frame $index dimensions mismatch."
    }

    $castMeta = Get-Content -Raw -LiteralPath "$castPath.meta"
    $backgroundMeta = Get-Content -Raw -LiteralPath "$backgroundPath.meta"
    $occlusionMaskMeta = Get-Content -Raw -LiteralPath "$occlusionMaskPath.meta"
    Assert-CSharpImportContract 'UnityProject/Assets/_Project/Tests/EditMode/TechnicalSample2D/A01OfficeAssetBundleTests.cs'
    Assert-Unity6000NestedSpriteRects $castMeta $expectedFrames
    Assert-True ($castMeta -match '(?m)^  spriteMode: 2$' -and $castMeta -match '(?m)^  spritePixelsToUnits: 100$') 'Cast .meta sprite mode/PPU mismatch.'
    Assert-True ($castMeta -match '(?m)^    enableMipMap: 0$' -and $castMeta -match '(?m)^    filterMode: 0$' -and $castMeta -match '(?m)^  spriteGenerateFallbackPhysicsShape: 0$') 'Cast .meta import flags mismatch.'
    Assert-True ($backgroundMeta -match '(?m)^  spriteMode: 1$' -and $backgroundMeta -match '(?m)^  spritePixelsToUnits: 100$') 'Background .meta sprite mode/PPU mismatch.'
    Assert-True ($occlusionMaskMeta -match ('(?m)^guid: ' + $expectedOcclusionMaskGuid + '$')) 'Occlusion mask .meta GUID mismatch.'
    Assert-True ($occlusionMaskMeta -match '(?m)^  spriteMode: 1$' -and $occlusionMaskMeta -match '(?m)^  spritePixelsToUnits: 100$') 'Occlusion mask .meta sprite mode/PPU mismatch.'
    Assert-True ($occlusionMaskMeta -match '(?m)^    enableMipMap: 0$' -and $occlusionMaskMeta -match '(?m)^    filterMode: 0$' -and $occlusionMaskMeta -match '(?m)^  spriteGenerateFallbackPhysicsShape: 0$' -and $occlusionMaskMeta -match '(?m)^  alphaIsTransparency: 1$' -and $occlusionMaskMeta -match '(?m)^    textureCompression: 0$') 'Occlusion mask .meta import flags mismatch.'
    Write-Output 'PASS: A01 Office asset bundle static contract'
}
catch {
    [Console]::Error.WriteLine($_.Exception.Message)
    exit 1
}
