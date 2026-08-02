#Requires -Version 7.0

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Read-GuardJson {
    param([Parameter(Mandatory = $true)][string]$Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "Required JSON file does not exist: $Path"
    }
    try {
        return Get-Content -Raw -LiteralPath $Path | ConvertFrom-Json -Depth 30
    }
    catch {
        throw "JSON file cannot be parsed: $Path. $($_.Exception.Message)"
    }
}

function Write-GuardJsonAtomic {
    param(
        [Parameter(Mandatory = $true)][string]$Path,
        [Parameter(Mandatory = $true)]$Value
    )
    $fullPath = [System.IO.Path]::GetFullPath($Path)
    $directory = Split-Path -Parent $fullPath
    [System.IO.Directory]::CreateDirectory($directory) | Out-Null
    $temporaryPath = "$fullPath.$([guid]::NewGuid().ToString('N')).tmp"
    try {
        [System.IO.File]::WriteAllText(
            $temporaryPath,
            (($Value | ConvertTo-Json -Depth 30) + [Environment]::NewLine),
            [System.Text.UTF8Encoding]::new($false)
        )
        Move-Item -LiteralPath $temporaryPath -Destination $fullPath -Force
    }
    finally {
        if (Test-Path -LiteralPath $temporaryPath -PathType Leaf) {
            Remove-Item -LiteralPath $temporaryPath -Force
        }
    }
}

function Get-CanonicalPath {
    param([Parameter(Mandatory = $true)][string]$Path)
    return [System.IO.Path]::GetFullPath($Path).TrimEnd(
        [System.IO.Path]::DirectorySeparatorChar,
        [System.IO.Path]::AltDirectorySeparatorChar
    )
}

function Assert-StrictChildPath {
    param(
        [Parameter(Mandatory = $true)][string]$Parent,
        [Parameter(Mandatory = $true)][string]$Child
    )
    $parentFull = Get-CanonicalPath -Path $Parent
    $childFull = Get-CanonicalPath -Path $Child
    $prefix = $parentFull + [System.IO.Path]::DirectorySeparatorChar
    if (-not $childFull.StartsWith($prefix, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "Path must be a strict child of '$parentFull': $childFull"
    }
    return $childFull
}

function Write-GuardResult {
    param(
        [Parameter(Mandatory = $true)][string]$Check,
        [Parameter(Mandatory = $true)][bool]$Passed,
        [Parameter(Mandatory = $true)]$Details
    )
    [pscustomobject]@{
        schema_version = 1
        check = $Check
        passed = $Passed
        details = @($Details)
    } | ConvertTo-Json -Depth 20
}

function Get-GuardTokenSignature {
    param(
        [Parameter(Mandatory = $true)][string]$WorkId,
        [Parameter(Mandatory = $true)][string]$RunId,
        [Parameter(Mandatory = $true)][string]$CandidateFingerprint,
        [Parameter(Mandatory = $true)][string]$ProjectPath,
        [Parameter(Mandatory = $true)][string]$Nonce,
        [Parameter(Mandatory = $true)][string]$MarkerCreatedUtc
    )
    $payload = "$WorkId`n$RunId`n$CandidateFingerprint`n$(Get-CanonicalPath -Path $ProjectPath)`n$Nonce`n$MarkerCreatedUtc`nInvoke-UnityEditModeTests.ps1"
    $sha = [System.Security.Cryptography.SHA256]::Create()
    try {
        return [System.BitConverter]::ToString(
            $sha.ComputeHash([System.Text.UTF8Encoding]::new($false).GetBytes($payload))
        ).Replace('-', '').ToLowerInvariant()
    }
    finally { $sha.Dispose() }
}
