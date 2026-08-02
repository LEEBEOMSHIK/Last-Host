#Requires -Version 7.0

[CmdletBinding()]
param([Parameter(Mandatory = $true)][string[]]$Path)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'VerificationGuard.Common.ps1')

$files = [System.Collections.Generic.List[System.IO.FileInfo]]::new()
foreach ($inputPath in $Path) {
    if (-not (Test-Path -LiteralPath $inputPath)) { throw "QA harness path does not exist: $inputPath" }
    $item = Get-Item -LiteralPath $inputPath
    if ($item.PSIsContainer) {
        Get-ChildItem -LiteralPath $item.FullName -Recurse -File -Filter '*.cs' | ForEach-Object { $files.Add($_) }
    }
    elseif ($item.Extension -ieq '.cs') { $files.Add($item) }
}
if ($files.Count -eq 0) { throw 'QA harness lint received zero C# files.' }

$errors = [System.Collections.Generic.List[string]]::new()
foreach ($file in $files) {
    $text = Get-Content -Raw -LiteralPath $file.FullName
    $relative = $file.FullName
    $forbidden = @(
        @{ name = 'System.Reflection'; pattern = '(?m)\bSystem\.Reflection\b|\busing\s+System\.Reflection\s*;' },
        @{ name = 'private reflection'; pattern = '(?m)\bBindingFlags\s*\.\s*(NonPublic|Private)|BindingFlags[^;\r\n]*(NonPublic|Private)' },
        @{ name = 'reflection member lookup'; pattern = '(?m)\.(GetField|GetProperty|GetMethod)\s*\([^;\r\n]*BindingFlags' }
    )
    foreach ($rule in $forbidden) {
        if ($text -match $rule.pattern) { $errors.Add("${relative}: forbidden $($rule.name)") }
    }

    $usesRigidBodyMotion = $text -match '\b(Rigidbody2D|Rigidbody)\b' -and
        $text -match '(\.position\s*=|\.MovePosition\s*\()'
    $usesYSort = $text -match '(?i)(sortingOrder|SortingGroup|YSort|RefreshSort|UpdateSort)'
    if ($usesRigidBodyMotion -and $usesYSort -and $text -notmatch '\bPhysics2D\s*\.\s*SyncTransforms\s*\(') {
        $errors.Add("${relative}: Rigidbody motion reaches Y-sort without Physics2D.SyncTransforms()")
    }
}

Write-GuardResult -Check 'qa-harness-safety' -Passed ($errors.Count -eq 0) -Details $errors
if ($errors.Count -gt 0) { exit 1 }
