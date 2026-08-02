#Requires -Version 7.0

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)][string[]]$BaselinePath,
    [Parameter(Mandatory = $true)][string[]]$CandidatePath,
    [Parameter(Mandatory = $true)][string[]]$TestPath
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'VerificationGuard.Common.ps1')

function Get-CsFiles([string[]]$Paths) {
    $result = [System.Collections.Generic.List[System.IO.FileInfo]]::new()
    foreach ($path in $Paths) {
        if (-not (Test-Path -LiteralPath $path)) { throw "Contract scan path does not exist: $path" }
        $item = Get-Item -LiteralPath $path
        if ($item.PSIsContainer) {
            Get-ChildItem -LiteralPath $item.FullName -Recurse -File -Filter '*.cs' | ForEach-Object { $result.Add($_) }
        }
        elseif ($item.Extension -ieq '.cs') { $result.Add($item) }
    }
    return @($result)
}

function Get-ContractTypes([System.IO.FileInfo[]]$Files) {
    $types = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
    foreach ($file in $Files) {
        $text = Get-Content -Raw -LiteralPath $file.FullName
        foreach ($match in [regex]::Matches($text, '\b[A-Za-z_][A-Za-z0-9_]*(?:Collider2D|Collider|Resolver)\b')) {
            [void]$types.Add($match.Value)
        }
    }
    return $types
}

$baselineTypes = Get-ContractTypes -Files (Get-CsFiles -Paths $BaselinePath)
$candidateTypes = Get-ContractTypes -Files (Get-CsFiles -Paths $CandidatePath)
$testFiles = Get-CsFiles -Paths $TestPath
$stale = [System.Collections.Generic.List[string]]::new()

foreach ($type in $baselineTypes) {
    if ($candidateTypes.Contains($type)) { continue }
    foreach ($testFile in $testFiles) {
        $testText = Get-Content -Raw -LiteralPath $testFile.FullName
        if ($testText -match "(?<![A-Za-z0-9_])$([regex]::Escape($type))(?![A-Za-z0-9_])") {
            $stale.Add("$($testFile.FullName): stale contract '$type' was removed from candidate")
        }
    }
}

Write-GuardResult -Check 'component-contract-impact' -Passed ($stale.Count -eq 0) -Details $stale
if ($stale.Count -gt 0) { exit 1 }
