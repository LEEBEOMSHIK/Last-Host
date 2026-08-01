#Requires -Version 7.0

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$ProjectRoot,

    [string[]]$ProductionPath = @(),
    [string[]]$TestPath = @(),
    [string[]]$ScenePath = @(),
    [string[]]$PackagePath = @(),
    [string[]]$VersionPath = @(),

    [Parameter(Mandatory = $true)]
    [string]$ManifestPath,

    [string]$RunId = ([guid]::NewGuid().ToString('N'))
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Resolve-InputPath {
    param(
        [string]$Root,
        [string]$InputPath
    )

    if ([System.IO.Path]::IsPathRooted($InputPath)) {
        return [System.IO.Path]::GetFullPath($InputPath)
    }
    return [System.IO.Path]::GetFullPath((Join-Path $Root $InputPath))
}

function Get-NormalizedRelativePath {
    param(
        [string]$Root,
        [string]$Path
    )

    return [System.IO.Path]::GetRelativePath($Root, $Path).Replace('\', '/')
}

if (-not (Test-Path -LiteralPath $ProjectRoot -PathType Container)) {
    throw "ProjectRoot directory does not exist: $ProjectRoot"
}
if ([string]::IsNullOrWhiteSpace($RunId)) {
    throw 'RunId cannot be empty.'
}

$canonicalRoot = [System.IO.Path]::GetFullPath((Resolve-Path -LiteralPath $ProjectRoot).Path).TrimEnd('\', '/')
$groups = [ordered]@{
    production = $ProductionPath
    test = $TestPath
    scene = $ScenePath
    package = $PackagePath
    version = $VersionPath
}

if (($groups.Values | ForEach-Object { $_.Count } | Measure-Object -Sum).Sum -le 0) {
    throw 'At least one production/test/scene/package/version path is required.'
}

$records = [System.Collections.Generic.List[object]]::new()
$seen = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
foreach ($category in $groups.Keys) {
    foreach ($inputPath in $groups[$category]) {
        if ([string]::IsNullOrWhiteSpace($inputPath)) {
            throw "Empty path supplied for category '$category'."
        }

        $resolved = Resolve-InputPath -Root $canonicalRoot -InputPath $inputPath
        if (-not (Test-Path -LiteralPath $resolved)) {
            throw "Fingerprint input does not exist ($category): $resolved"
        }

        $item = Get-Item -LiteralPath $resolved
        $files = if ($item.PSIsContainer) {
            Get-ChildItem -LiteralPath $resolved -File -Recurse
        }
        else {
            @($item)
        }

        foreach ($file in $files) {
            $relativePath = Get-NormalizedRelativePath -Root $canonicalRoot -Path $file.FullName
            $key = "$category`0$relativePath"
            if (-not $seen.Add($key)) {
                continue
            }

            $records.Add([pscustomobject]@{
                category = $category
                path = $relativePath
                length = [long]$file.Length
                sha256 = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash.ToLowerInvariant()
            })
        }
    }
}

if ($records.Count -le 0) {
    throw 'Fingerprint inputs resolved to zero files.'
}

$sortedRecords = @($records | Sort-Object -Property category, path)
$candidateLines = foreach ($record in $sortedRecords) {
    "$($record.category)`t$($record.path)`t$($record.sha256)`t$($record.length)"
}
$candidateText = ($candidateLines -join "`n") + "`n"
$sha = [System.Security.Cryptography.SHA256]::Create()
try {
    $candidateHash = [System.BitConverter]::ToString(
        $sha.ComputeHash([System.Text.UTF8Encoding]::new($false).GetBytes($candidateText))
    ).Replace('-', '').ToLowerInvariant()
}
finally {
    $sha.Dispose()
}

$manifest = [ordered]@{
    schema_version = 1
    run_id = $RunId
    generated_utc = [DateTimeOffset]::UtcNow.ToString('o')
    project_root = $canonicalRoot
    candidate_fingerprint = $candidateHash
    algorithm = 'sha256(category<TAB>relative-path<TAB>file-sha256<TAB>length; ordinal path order)'
    file_count = $sortedRecords.Count
    inputs = $groups
    files = $sortedRecords
}

$manifestFullPath = [System.IO.Path]::GetFullPath($ManifestPath)
$manifestDirectory = Split-Path -Parent $manifestFullPath
[System.IO.Directory]::CreateDirectory($manifestDirectory) | Out-Null
$temporaryPath = "$manifestFullPath.$([guid]::NewGuid().ToString('N')).tmp"
try {
    [System.IO.File]::WriteAllText(
        $temporaryPath,
        (($manifest | ConvertTo-Json -Depth 8) + [Environment]::NewLine),
        [System.Text.UTF8Encoding]::new($false)
    )
    Move-Item -LiteralPath $temporaryPath -Destination $manifestFullPath -Force
}
finally {
    if (Test-Path -LiteralPath $temporaryPath -PathType Leaf) {
        Remove-Item -LiteralPath $temporaryPath
    }
}

[pscustomobject]@{
    run_id = $RunId
    candidate_fingerprint = $candidateHash
    file_count = $sortedRecords.Count
    manifest_path = $manifestFullPath
} | ConvertTo-Json -Depth 5
