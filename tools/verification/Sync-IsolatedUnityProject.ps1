#Requires -Version 7.0

[CmdletBinding(DefaultParameterSetName = 'Sync')]
param(
    [Parameter(Mandatory = $true)][ValidatePattern('^[A-Za-z0-9][A-Za-z0-9._-]{0,79}$')][string]$WorkId,
    [Parameter(Mandatory = $true)][string]$CacheRoot,
    [Parameter(Mandatory = $true, ParameterSetName = 'Sync')][string]$SourceProjectPath,
    [Parameter(ParameterSetName = 'Sync')][switch]$Sync,
    [Parameter(Mandatory = $true, ParameterSetName = 'Cleanup')][switch]$Cleanup
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'VerificationGuard.Common.ps1')

$cacheRootFull = Get-CanonicalPath -Path $CacheRoot
if ([System.IO.Path]::GetPathRoot($cacheRootFull).TrimEnd('\', '/') -ieq $cacheRootFull) {
    throw "CacheRoot cannot be a filesystem root: $cacheRootFull"
}
[System.IO.Directory]::CreateDirectory($cacheRootFull) | Out-Null

$instanceRoot = Assert-StrictChildPath -Parent $cacheRootFull -Child (Join-Path $cacheRootFull $WorkId)
$projectRoot = Assert-StrictChildPath -Parent $instanceRoot -Child (Join-Path $instanceRoot 'project')
$markerPath = Join-Path $instanceRoot '.last-host-isolated-unity-cache.json'

function Assert-Marker([string]$ExpectedSource = '') {
    $marker = Read-GuardJson -Path $markerPath
    if ([int]$marker.schema_version -ne 1 -or [string]$marker.kind -cne 'last-host-isolated-unity-cache') {
        throw "Invalid isolated cache marker: $markerPath"
    }
    if ([string]$marker.work_id -cne $WorkId) { throw 'Cache marker work_id mismatch.' }
    if ((Get-CanonicalPath -Path ([string]$marker.instance_root)) -cne $instanceRoot) {
        throw 'Cache marker instance_root mismatch.'
    }
    if ($ExpectedSource -and (Get-CanonicalPath -Path ([string]$marker.source_project)) -cne $ExpectedSource) {
        throw 'Cache marker source_project mismatch.'
    }
    return $marker
}

function Sync-Tree([string]$Source, [string]$Destination) {
    [System.IO.Directory]::CreateDirectory($Destination) | Out-Null
    $sourceFiles = @{}
    foreach ($file in Get-ChildItem -LiteralPath $Source -Recurse -File) {
        $relative = [System.IO.Path]::GetRelativePath($Source, $file.FullName)
        $sourceFiles[$relative] = $true
        $destinationFile = Assert-StrictChildPath -Parent $Destination -Child (Join-Path $Destination $relative)
        $destinationDirectory = Split-Path -Parent $destinationFile
        [System.IO.Directory]::CreateDirectory($destinationDirectory) | Out-Null
        $copyNeeded = -not (Test-Path -LiteralPath $destinationFile -PathType Leaf)
        if (-not $copyNeeded) {
            $destinationItem = Get-Item -LiteralPath $destinationFile
            $copyNeeded = $destinationItem.Length -ne $file.Length
            if (-not $copyNeeded) {
                $sourceHash = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash
                $destinationHash = (Get-FileHash -LiteralPath $destinationFile -Algorithm SHA256).Hash
                $copyNeeded = $sourceHash -cne $destinationHash
            }
        }
        if ($copyNeeded) { Copy-Item -LiteralPath $file.FullName -Destination $destinationFile -Force }
    }
    foreach ($destinationFile in Get-ChildItem -LiteralPath $Destination -Recurse -File) {
        $relative = [System.IO.Path]::GetRelativePath($Destination, $destinationFile.FullName)
        if (-not $sourceFiles.ContainsKey($relative)) {
            [void](Assert-StrictChildPath -Parent $Destination -Child $destinationFile.FullName)
            Remove-Item -LiteralPath $destinationFile.FullName -Force
        }
    }
    Get-ChildItem -LiteralPath $Destination -Recurse -Directory |
        Sort-Object -Property FullName -Descending |
        Where-Object { @(Get-ChildItem -LiteralPath $_.FullName -Force).Count -eq 0 } |
        ForEach-Object {
            [void](Assert-StrictChildPath -Parent $Destination -Child $_.FullName)
            Remove-Item -LiteralPath $_.FullName -Force
        }
}

if ($Cleanup) {
    [void](Assert-Marker)
    [void](Assert-StrictChildPath -Parent $cacheRootFull -Child $instanceRoot)
    Remove-Item -LiteralPath $instanceRoot -Recurse -Force
    [pscustomobject]@{ action = 'cleanup'; work_id = $WorkId; removed = $instanceRoot; marker_validated = $true } |
        ConvertTo-Json -Depth 5
    exit 0
}

if (-not (Test-Path -LiteralPath $SourceProjectPath -PathType Container)) {
    throw "Source Unity project does not exist: $SourceProjectPath"
}
$sourceFull = Get-CanonicalPath -Path (Resolve-Path -LiteralPath $SourceProjectPath).Path
foreach ($folder in @('Assets', 'Packages', 'ProjectSettings')) {
    if (-not (Test-Path -LiteralPath (Join-Path $sourceFull $folder) -PathType Container)) {
        throw "Source Unity project is missing required folder: $folder"
    }
}

if (Test-Path -LiteralPath $instanceRoot) {
    [void](Assert-Marker -ExpectedSource $sourceFull)
}
else {
    [System.IO.Directory]::CreateDirectory($projectRoot) | Out-Null
    Write-GuardJsonAtomic -Path $markerPath -Value ([ordered]@{
        schema_version = 1
        kind = 'last-host-isolated-unity-cache'
        work_id = $WorkId
        source_project = $sourceFull
        instance_root = $instanceRoot
        created_utc = [DateTimeOffset]::UtcNow.ToString('o')
    })
}

foreach ($folder in @('Assets', 'Packages', 'ProjectSettings')) {
    Sync-Tree -Source (Join-Path $sourceFull $folder) -Destination (Join-Path $projectRoot $folder)
}
[System.IO.Directory]::CreateDirectory((Join-Path $projectRoot 'Library')) | Out-Null

[pscustomobject]@{
    action = 'sync'
    work_id = $WorkId
    source_project = $sourceFull
    isolated_project = $projectRoot
    library_preserved = $true
    synced_folders = @('Assets', 'Packages', 'ProjectSettings')
    marker_path = $markerPath
} | ConvertTo-Json -Depth 8
