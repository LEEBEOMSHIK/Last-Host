#Requires -Version 7.0

[CmdletBinding(DefaultParameterSetName = 'Run')]
param(
    [Parameter(Mandatory = $true)]
    [string]$ResultsPath,

    [Parameter(Mandatory = $true, ParameterSetName = 'Run')]
    [string]$ProjectPath,

    [Parameter(Mandatory = $true, ParameterSetName = 'Run')]
    [string]$UnityPath,

    [Parameter(Mandatory = $true, ParameterSetName = 'Run')]
    [string]$LogPath,

    [Parameter(ParameterSetName = 'Run')]
    [string]$TestFilter,

    [Parameter(ParameterSetName = 'Run')]
    [ValidateRange(1, 86400)]
    [int]$TimeoutSeconds = 1800,

    [Parameter(Mandatory = $true, ParameterSetName = 'Run')]
    [string]$GuardTokenPath,

    [Parameter(Mandatory = $true, ParameterSetName = 'Validate')]
    [switch]$ValidateResultsOnly
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'VerificationGuard.Common.ps1')

function Assert-AndConsumeGuardToken {
    param(
        [Parameter(Mandatory = $true)][string]$TokenPath,
        [Parameter(Mandatory = $true)][string]$RequestedProjectPath
    )

    $projectFull = Get-CanonicalPath -Path $RequestedProjectPath
    $instanceRoot = Split-Path -Parent $projectFull
    $tokenDirectory = Join-Path $instanceRoot 'tokens'
    $tokenFull = Assert-StrictChildPath -Parent $tokenDirectory -Child $TokenPath
    $markerPath = Join-Path $instanceRoot '.last-host-isolated-unity-cache.json'
    $marker = Read-GuardJson -Path $markerPath
    $token = Read-GuardJson -Path $tokenFull

    if ([int]$marker.schema_version -ne 1 -or [string]$marker.kind -cne 'last-host-isolated-unity-cache') {
        throw 'Guard token parent does not have a valid isolated-cache marker.'
    }
    if ([int]$token.schema_version -ne 1 -or
        [string]$token.issued_by -cne 'Invoke-HighCostVerification.ps1' -or
        [string]$token.target -cne 'Invoke-UnityEditModeTests.ps1') {
        throw 'Run mode requires a token issued by Invoke-HighCostVerification.ps1.'
    }
    if ((Get-CanonicalPath -Path ([string]$token.project_path)) -cne $projectFull) {
        throw 'Guard token project path mismatch.'
    }
    if ([DateTimeOffset]::Parse([string]$token.expires_utc) -le [DateTimeOffset]::UtcNow) {
        throw 'Guard token has expired.'
    }
    $expectedSignature = Get-GuardTokenSignature -WorkId ([string]$token.work_id) -RunId ([string]$token.run_id) `
        -CandidateFingerprint ([string]$token.candidate_fingerprint) -ProjectPath $projectFull `
        -Nonce ([string]$token.nonce) -MarkerCreatedUtc ([string]$marker.created_utc)
    if ([string]$token.signature -cne $expectedSignature) { throw 'Guard token signature mismatch.' }

    # One-shot token: consume before any Unity process can start.
    Remove-Item -LiteralPath $tokenFull -Force
}

function Get-TestResultSummary {
    param([string]$Path)

    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "Unity test result XML was not created: $Path"
    }

    $file = Get-Item -LiteralPath $Path
    if ($file.Length -le 0) {
        throw "Unity test result XML is empty: $Path"
    }

    try {
        [xml]$document = Get-Content -LiteralPath $Path -Raw
    }
    catch {
        throw "Unity test result XML cannot be parsed: $Path. $($_.Exception.Message)"
    }

    $root = $document.DocumentElement
    if ($null -eq $root -or $root.Name -ne 'test-run') {
        throw "Expected NUnit3 <test-run> root in Unity result XML: $Path"
    }

    $counts = [ordered]@{}
    foreach ($name in @('total', 'passed', 'failed', 'skipped', 'inconclusive')) {
        $raw = $root.GetAttribute($name)
        $value = 0
        if ([string]::IsNullOrWhiteSpace($raw) -or -not [int]::TryParse($raw, [ref]$value)) {
            throw "Missing or invalid '$name' count in Unity result XML: $Path"
        }
        $counts[$name] = $value
    }

    $valid = $counts.total -gt 0 -and
        $counts.passed -eq $counts.total -and
        $counts.failed -eq 0 -and
        $counts.skipped -eq 0 -and
        $counts.inconclusive -eq 0 -and
        $root.GetAttribute('result') -eq 'Passed'

    return [pscustomobject]@{
        results_path = [System.IO.Path]::GetFullPath($file.FullName)
        result = $root.GetAttribute('result')
        total = $counts.total
        passed = $counts.passed
        failed = $counts.failed
        skipped = $counts.skipped
        inconclusive = $counts.inconclusive
        valid_pass = $valid
    }
}

$unityExitCode = $null
if (-not $ValidateResultsOnly) {
    Assert-AndConsumeGuardToken -TokenPath $GuardTokenPath -RequestedProjectPath $ProjectPath
    if (-not (Test-Path -LiteralPath $ProjectPath -PathType Container)) {
        throw "Unity project directory does not exist: $ProjectPath"
    }
    if (-not (Test-Path -LiteralPath $UnityPath -PathType Leaf)) {
        throw "Unity executable does not exist: $UnityPath"
    }

    $resultDirectory = Split-Path -Parent ([System.IO.Path]::GetFullPath($ResultsPath))
    $logDirectory = Split-Path -Parent ([System.IO.Path]::GetFullPath($LogPath))
    [System.IO.Directory]::CreateDirectory($resultDirectory) | Out-Null
    [System.IO.Directory]::CreateDirectory($logDirectory) | Out-Null

    if (Test-Path -LiteralPath $ResultsPath -PathType Leaf) {
        Remove-Item -LiteralPath $ResultsPath
    }

    $arguments = @(
        '-batchmode',
        '-nographics',
        '-projectPath', [System.IO.Path]::GetFullPath($ProjectPath),
        '-runTests',
        '-testPlatform', 'EditMode',
        '-testResults', [System.IO.Path]::GetFullPath($ResultsPath),
        '-logFile', [System.IO.Path]::GetFullPath($LogPath)
    )
    if (-not [string]::IsNullOrWhiteSpace($TestFilter)) {
        $arguments += @('-testFilter', $TestFilter)
    }

    # Deliberately no -quit: Unity Test Framework owns the run completion and shutdown.
    $startInfo = [System.Diagnostics.ProcessStartInfo]::new()
    $startInfo.FileName = [System.IO.Path]::GetFullPath($UnityPath)
    $startInfo.UseShellExecute = $false
    $startInfo.CreateNoWindow = $true
    foreach ($argument in $arguments) {
        $startInfo.ArgumentList.Add([string]$argument)
    }

    $process = [System.Diagnostics.Process]::new()
    $process.StartInfo = $startInfo
    try {
        if (-not $process.Start()) {
            throw "Unity process could not be started: $UnityPath"
        }

        $unityProcessId = $process.Id
        $finished = $process.WaitForExit($TimeoutSeconds * 1000)
        if (-not $finished) {
            try {
                # Kill only the process started by this invocation. Do not kill a process tree.
                $process.Kill()
                [void]$process.WaitForExit(10000)
            }
            catch {
                throw "Unity EditMode test run timed out after $TimeoutSeconds seconds and PID $unityProcessId could not be terminated. Inspect log: $([System.IO.Path]::GetFullPath($LogPath)). $($_.Exception.Message)"
            }

            throw "Unity EditMode test run timed out after $TimeoutSeconds seconds. PID $unityProcessId was terminated. Inspect log: $([System.IO.Path]::GetFullPath($LogPath))"
        }

        $unityExitCode = $process.ExitCode
    }
    finally {
        $process.Dispose()
    }
}

$summary = Get-TestResultSummary -Path $ResultsPath
$summary | Add-Member -NotePropertyName mode -NotePropertyValue $(if ($ValidateResultsOnly) { 'ValidateResultsOnly' } else { 'Run' })
$summary | Add-Member -NotePropertyName unity_exit_code -NotePropertyValue $unityExitCode
$summary | ConvertTo-Json -Depth 5

if (($null -ne $unityExitCode -and $unityExitCode -ne 0) -or -not $summary.valid_pass) {
    exit 1
}
