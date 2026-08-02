#Requires -Version 7.0

[CmdletBinding()]
param([switch]$IntegrationOnly)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'VerificationGuard.Common.ps1')

$pwsh = (Get-Process -Id $PID).Path
$temporaryRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("last-host-guard-selftest-" + [guid]::NewGuid().ToString('N'))
$results = [System.Collections.Generic.List[object]]::new()
$unityStartCount = 0

function Write-Utf8([string]$Path, [string]$Text) {
    $directory = Split-Path -Parent $Path
    [System.IO.Directory]::CreateDirectory($directory) | Out-Null
    [System.IO.File]::WriteAllText($Path, $Text, [System.Text.UTF8Encoding]::new($false))
}

function Write-Json([string]$Path, $Value) {
    Write-Utf8 -Path $Path -Text (($Value | ConvertTo-Json -Depth 30) + [Environment]::NewLine)
}

function Invoke-Case([string]$Id, [bool]$ExpectPass, [string]$Script, [string[]]$Arguments) {
    $output = & $pwsh -NoProfile -File $Script @Arguments 2>&1
    $exitCode = $LASTEXITCODE
    $passed = if ($ExpectPass) { $exitCode -eq 0 } else { $exitCode -ne 0 }
    $results.Add([pscustomobject]@{
        id = $Id
        expected = if ($ExpectPass) { 'pass' } else { 'blocked' }
        observed_exit_code = $exitCode
        passed = $passed
        output = (($output | ForEach-Object { [string]$_ }) -join "`n")
    })
    if (-not $passed) {
        throw "Self-test case $Id produced unexpected exit code $exitCode. Child output: $($output -join [Environment]::NewLine)"
    }
}

function Get-LedgerEntries([string]$Path, [string]$Criterion) {
    $value = Get-Content -Raw -LiteralPath $Path | ConvertFrom-Json -Depth 30
    return @($value.entries | Where-Object { [string]$_.criterion_id -ceq $Criterion })
}

function New-TestToken(
    [string]$Path,
    [string]$ProjectPath,
    [string]$MarkerCreatedUtc,
    [string]$ExpiresUtc,
    [string]$Nonce
) {
    $signature = Get-GuardTokenSignature -WorkId 'guard-selftest' -RunId 'token-run' `
        -CandidateFingerprint 'token-fingerprint' -ProjectPath $ProjectPath -Nonce $Nonce -MarkerCreatedUtc $MarkerCreatedUtc
    Write-Json $Path ([ordered]@{
        schema_version = 1; issued_by = 'Invoke-HighCostVerification.ps1'; target = 'Invoke-UnityEditModeTests.ps1'
        work_id = 'guard-selftest'; run_id = 'token-run'; candidate_fingerprint = 'token-fingerprint'
        project_path = $ProjectPath; nonce = $Nonce; issued_utc = [DateTimeOffset]::UtcNow.ToString('o')
        expires_utc = $ExpiresUtc; signature = $signature
    })
}

try {
    [System.IO.Directory]::CreateDirectory($temporaryRoot) | Out-Null
    $sourceProject = Join-Path $temporaryRoot 'source-project'
    foreach ($folder in @('Assets', 'Packages', 'ProjectSettings')) {
        [System.IO.Directory]::CreateDirectory((Join-Path $sourceProject $folder)) | Out-Null
    }
    Write-Utf8 (Join-Path $sourceProject 'Assets/Source.txt') 'v1'
    Write-Utf8 (Join-Path $sourceProject 'Packages/manifest.json') '{}'
    Write-Utf8 (Join-Path $sourceProject 'ProjectSettings/ProjectVersion.txt') 'm_EditorVersion: dummy'

    $baseline = Join-Path $temporaryRoot 'baseline'
    $candidateGood = Join-Path $temporaryRoot 'candidate-good'
    $candidateStale = Join-Path $temporaryRoot 'candidate-stale'
    $tests = Join-Path $temporaryRoot 'tests'
    $qaGood = Join-Path $temporaryRoot 'qa-good'
    $qaBad = Join-Path $temporaryRoot 'qa-bad'
    Write-Utf8 (Join-Path $baseline 'Player.cs') '[RequireComponent(typeof(BoxCollider2D))] public class Player {}'
    Write-Utf8 (Join-Path $candidateGood 'Player.cs') '[RequireComponent(typeof(BoxCollider2D))] public class Player {}'
    Write-Utf8 (Join-Path $candidateStale 'Player.cs') '[RequireComponent(typeof(CapsuleCollider2D))] public class Player {}'
    Write-Utf8 (Join-Path $tests 'PlayerTests.cs') 'public class PlayerTests { BoxCollider2D expected; }'
    Write-Utf8 (Join-Path $qaGood 'SafeHarness.cs') 'public class SafeHarness { public void Run() {} }'
    Write-Utf8 (Join-Path $qaBad 'UnsafeHarness.cs') 'using System.Reflection; public class UnsafeHarness { Rigidbody2D body; void Run(){ body.position = default; RefreshYSort(); } void RefreshYSort(){} }'

    $goodBrief = Join-Path $temporaryRoot 'brief-good.json'
    $badBrief = Join-Path $temporaryRoot 'brief-bad.json'
    Write-Json $goodBrief ([ordered]@{
        work_id = 'guard-selftest'; context_mode = 'packet-only'; fork_turns = 'none'
        required_files = @('task.md', 'tools/verification/README.md', 'docs/agents/loop-engineering-gates.md')
        message = 'Use only the listed packet entry points.'; include_conversation_history = $false
    })
    Write-Json $badBrief ([ordered]@{
        work_id = 'guard-selftest'; context_mode = 'full-history'; fork_turns = 'all'
        required_files = @('1', '2', '3', '4'); message = 'Send the entire conversation history.'
        include_conversation_history = $true
    })

    $goodState = Join-Path $temporaryRoot 'state-good.json'
    $unknownStatusState = Join-Path $temporaryRoot 'state-unknown-status.json'
    $statusOnlyStaleState = Join-Path $temporaryRoot 'state-status-only-stale.json'
    $baseState = [ordered]@{
        schema_version = 1; work_id = 'guard-selftest'; status = 'ready-for-verification'
        run_id = 'run-001'; candidate_fingerprint = 'fingerprint-001'
        cost = [ordered]@{ unity_starts = 0; mcp_starts = 0; build_starts = 0; recorded_high_cost_attempts = 0 }
        evidence = @()
    }
    Write-Json $goodState $baseState
    $unknownStatus = [ordered]@{
        schema_version = 1; work_id = 'guard-selftest'; status = 'invented-status'
        run_id = 'run-001'; candidate_fingerprint = 'fingerprint-001'
        cost = [ordered]@{ unity_starts = 0; mcp_starts = 0; build_starts = 0; recorded_high_cost_attempts = 0 }
        evidence = @()
    }
    Write-Json $unknownStatusState $unknownStatus
    $statusOnlyStale = [ordered]@{
        schema_version = 1; work_id = 'guard-selftest'; status = 'verification-running'
        run_id = 'run-001'; candidate_fingerprint = 'fingerprint-001'
        cost = [ordered]@{ unity_starts = 0; mcp_starts = 0; build_starts = 0; recorded_high_cost_attempts = 0 }
        evidence = @()
    }
    Write-Json $statusOnlyStaleState $statusOnlyStale

    $ledger = Join-Path $temporaryRoot 'ledger.json'
    $retryLedger = Join-Path $temporaryRoot 'ledger-natural-retry.json'
    $earlyReclassLedger = Join-Path $temporaryRoot 'ledger-early-reclass.json'
    Write-Json $ledger ([ordered]@{ schema_version = 1; work_id = 'guard-selftest'; entries = @() })
    Write-Json $retryLedger ([ordered]@{ schema_version = 1; work_id = 'guard-selftest'; entries = @() })
    Write-Json $earlyReclassLedger ([ordered]@{ schema_version = 1; work_id = 'guard-selftest'; entries = @() })

    $wrapper = Join-Path $PSScriptRoot 'Invoke-HighCostVerification.ps1'
    $common = @(
        '-WorkId', 'guard-selftest', '-RunId', 'run-001', '-CandidateFingerprint', 'fingerprint-001',
        '-AgentBriefPath', $goodBrief, '-CurrentStatePath', $goodState,
        '-QaHarnessPath', $qaGood, '-ContractBaselinePath', $baseline,
        '-ProductionPath', $candidateGood, '-TestPath', $tests,
        '-SourceProjectPath', $sourceProject, '-CacheRoot', (Join-Path $temporaryRoot 'wrapper-cache'), '-PreflightOnly'
    )

    if ($IntegrationOnly) {
        Invoke-Case 'integration-valid-preflight-diagnostic' $true $wrapper (@('-CriterionId', 'integration', '-Route', 'UnityEditMode', '-LedgerPath', $ledger) + $common)
        [pscustomobject]@{ passed = $true; diagnostic_only = $true; cases = $results; unity_starts = 0 } | ConvertTo-Json -Depth 20
        return
    }

    Invoke-Case 'G1-unsupported-route' $false $wrapper (@('-CriterionId', 'G1', '-Route', 'McpTestRunner', '-LedgerPath', $ledger) + $common)
    $g2Args = @('-WorkId', 'guard-selftest', '-CriterionId', 'G2', '-LedgerPath', $ledger, '-Route', 'UnityEditMode', '-RunId', 'run-001', '-CandidateFingerprint', 'fingerprint-001', '-AgentBriefPath', $goodBrief, '-CurrentStatePath', $goodState, '-QaHarnessPath', $qaBad, '-ContractBaselinePath', $baseline, '-ProductionPath', $candidateGood, '-TestPath', $tests, '-SourceProjectPath', $sourceProject, '-CacheRoot', (Join-Path $temporaryRoot 'wrapper-cache'), '-PreflightOnly')
    Invoke-Case 'G2-forbidden-qa-harness' $false $wrapper $g2Args
    $g3Args = @('-WorkId', 'guard-selftest', '-CriterionId', 'G3', '-LedgerPath', $ledger, '-Route', 'UnityEditMode', '-RunId', 'run-001', '-CandidateFingerprint', 'fingerprint-001', '-AgentBriefPath', $goodBrief, '-CurrentStatePath', $goodState, '-QaHarnessPath', $qaGood, '-ContractBaselinePath', $baseline, '-ProductionPath', $candidateStale, '-TestPath', $tests, '-SourceProjectPath', $sourceProject, '-CacheRoot', (Join-Path $temporaryRoot 'wrapper-cache'), '-PreflightOnly')
    Invoke-Case 'G3-stale-component-contract' $false $wrapper $g3Args
    Invoke-Case 'G4-early-reclassification-blocked' $false $wrapper @(
        '-WorkId', 'guard-selftest', '-CriterionId', 'G4-natural', '-LedgerPath', $earlyReclassLedger,
        '-RegisterReclassification', '-ReclassificationId', 'too-early', '-NewRiskClass', 'R2',
        '-RootCause', 'root cause is not yet evidenced', '-ChangePlan', 'change plan is not yet authorized'
    )
    $g4Run1 = @('-WorkId', 'guard-selftest', '-CriterionId', 'G4-natural', '-LedgerPath', $retryLedger, '-Route', 'McpTestRunner', '-RunId', 'retry-run-001', '-CandidateFingerprint', 'fingerprint-001', '-AgentBriefPath', $goodBrief, '-CurrentStatePath', $goodState, '-QaHarnessPath', $qaGood, '-ContractBaselinePath', $baseline, '-ProductionPath', $candidateGood, '-TestPath', $tests, '-SourceProjectPath', $sourceProject, '-CacheRoot', (Join-Path $temporaryRoot 'wrapper-cache'), '-PreflightOnly')
    Invoke-Case 'G4-first-actual-preflight-failure' $false $wrapper $g4Run1
    if ((Get-LedgerEntries -Path $retryLedger -Criterion 'G4-natural').Count -ne 1) {
        throw 'G4 first actual failure was not recorded exactly once.'
    }
    Invoke-Case 'G4-duplicate-run-not-recorded' $false $wrapper $g4Run1
    if ((Get-LedgerEntries -Path $retryLedger -Criterion 'G4-natural').Count -ne 1) {
        throw 'G4 duplicate run identity created a duplicate failure entry.'
    }
    $g4Run2 = @('-WorkId', 'guard-selftest', '-CriterionId', 'G4-natural', '-LedgerPath', $retryLedger, '-Route', 'McpTestRunner', '-RunId', 'retry-run-002', '-CandidateFingerprint', 'fingerprint-001', '-AgentBriefPath', $goodBrief, '-CurrentStatePath', $goodState, '-QaHarnessPath', $qaGood, '-ContractBaselinePath', $baseline, '-ProductionPath', $candidateGood, '-TestPath', $tests, '-SourceProjectPath', $sourceProject, '-CacheRoot', (Join-Path $temporaryRoot 'wrapper-cache'), '-PreflightOnly')
    Invoke-Case 'G4-second-actual-preflight-failure' $false $wrapper $g4Run2
    if ((Get-LedgerEntries -Path $retryLedger -Criterion 'G4-natural').Count -ne 2) {
        throw 'G4 second actual failure did not produce the second ledger entry.'
    }
    $g4Run3 = @('-WorkId', 'guard-selftest', '-CriterionId', 'G4-natural', '-LedgerPath', $retryLedger, '-Route', 'McpTestRunner', '-RunId', 'retry-run-003', '-CandidateFingerprint', 'fingerprint-001', '-AgentBriefPath', $goodBrief, '-CurrentStatePath', $goodState, '-QaHarnessPath', $qaGood, '-ContractBaselinePath', $baseline, '-ProductionPath', $candidateGood, '-TestPath', $tests, '-SourceProjectPath', $sourceProject, '-CacheRoot', (Join-Path $temporaryRoot 'wrapper-cache'), '-PreflightOnly')
    Invoke-Case 'G4-third-attempt-guard-blocked' $false $wrapper $g4Run3
    if ((Get-LedgerEntries -Path $retryLedger -Criterion 'G4-natural').Count -ne 2) {
        throw 'G4 retry-budget guard was incorrectly recorded as another failure.'
    }
    Invoke-Case 'G4-reclassification-after-two-failures' $true $wrapper @(
        '-WorkId', 'guard-selftest', '-CriterionId', 'G4-natural', '-LedgerPath', $retryLedger,
        '-RegisterReclassification', '-ReclassificationId', 'reclass-001', '-NewRiskClass', 'R2',
        '-RootCause', 'unsupported route was requested twice', '-ChangePlan', 'select the supported wrapper route before retry'
    )
    $reclassEntries = Get-LedgerEntries -Path $retryLedger -Criterion 'G4-natural'
    $reclass = @($reclassEntries | Where-Object { [string]$_.outcome -eq 'reclassified' })
    if ($reclass.Count -ne 1 -or [string]::IsNullOrWhiteSpace([string]$reclass[0].root_cause) -or [string]::IsNullOrWhiteSpace([string]$reclass[0].change_plan)) {
        throw 'G4 reclassification did not preserve separate root_cause and change_plan fields.'
    }

    $cacheTool = Join-Path $PSScriptRoot 'Sync-IsolatedUnityProject.ps1'
    $cacheRoot = Join-Path $temporaryRoot 'cache-reuse'
    Invoke-Case 'G5-first-cache-sync' $true $cacheTool @('-WorkId', 'guard-selftest', '-CacheRoot', $cacheRoot, '-SourceProjectPath', $sourceProject, '-Sync')
    $librarySentinel = Join-Path $cacheRoot 'guard-selftest/project/Library/preserved.txt'
    Write-Utf8 $librarySentinel 'keep'
    $sourceAsset = Join-Path $sourceProject 'Assets/Source.txt'
    $cachedAsset = Join-Path $cacheRoot 'guard-selftest/project/Assets/Source.txt'
    $fixedTimestamp = [DateTime]::SpecifyKind([DateTime]::new(2026, 1, 1, 0, 0, 0), [DateTimeKind]::Utc)
    [System.IO.File]::SetLastWriteTimeUtc($sourceAsset, $fixedTimestamp)
    [System.IO.File]::SetLastWriteTimeUtc($cachedAsset, $fixedTimestamp)
    Write-Utf8 $sourceAsset 'v2'
    [System.IO.File]::SetLastWriteTimeUtc($sourceAsset, $fixedTimestamp)
    Invoke-Case 'G5-second-cache-sync' $true $cacheTool @('-WorkId', 'guard-selftest', '-CacheRoot', $cacheRoot, '-SourceProjectPath', $sourceProject, '-Sync')
    if (-not (Test-Path -LiteralPath $librarySentinel) -or (Get-Content -Raw -LiteralPath $cachedAsset) -ne 'v2') {
        throw 'G5 hash sync missed same-size/same-timestamp content or did not preserve Library.'
    }

    $lowLevel = Join-Path $PSScriptRoot 'Invoke-UnityEditModeTests.ps1'
    $isolatedProject = Join-Path $cacheRoot 'guard-selftest/project'
    $instanceRoot = Split-Path -Parent $isolatedProject
    $marker = Get-Content -Raw -LiteralPath (Join-Path $instanceRoot '.last-host-isolated-unity-cache.json') | ConvertFrom-Json -Depth 10
    $tokenDirectory = Join-Path $instanceRoot 'tokens'
    [System.IO.Directory]::CreateDirectory($tokenDirectory) | Out-Null
    $runnerBase = @('-ProjectPath', $isolatedProject, '-UnityPath', (Join-Path $temporaryRoot 'never-unity.exe'), '-ResultsPath', (Join-Path $temporaryRoot 'never-results.xml'), '-LogPath', (Join-Path $temporaryRoot 'never-unity.log'))
    Invoke-Case 'G8-token-missing-blocked' $false $lowLevel ($runnerBase + @('-GuardTokenPath', (Join-Path $tokenDirectory 'missing.json')))
    $expiredToken = Join-Path $tokenDirectory 'expired.json'
    New-TestToken -Path $expiredToken -ProjectPath $isolatedProject -MarkerCreatedUtc ([string]$marker.created_utc) -ExpiresUtc ([DateTimeOffset]::UtcNow.AddMinutes(-1).ToString('o')) -Nonce 'expired-nonce'
    Invoke-Case 'G8-token-expired-blocked' $false $lowLevel ($runnerBase + @('-GuardTokenPath', $expiredToken))
    $oneShotToken = Join-Path $tokenDirectory 'one-shot.json'
    New-TestToken -Path $oneShotToken -ProjectPath $isolatedProject -MarkerCreatedUtc ([string]$marker.created_utc) -ExpiresUtc ([DateTimeOffset]::UtcNow.AddMinutes(5).ToString('o')) -Nonce 'one-shot-nonce'
    Invoke-Case 'G8-valid-token-consumed-before-unity-path-check' $false $lowLevel ($runnerBase + @('-GuardTokenPath', $oneShotToken))
    if (Test-Path -LiteralPath $oneShotToken -PathType Leaf) { throw 'G8 valid one-shot token was not consumed.' }
    Invoke-Case 'G8-consumed-token-reuse-blocked' $false $lowLevel ($runnerBase + @('-GuardTokenPath', $oneShotToken))

    [System.IO.Directory]::CreateDirectory((Join-Path $cacheRoot 'unmarked')) | Out-Null
    Invoke-Case 'G5-unmarked-cleanup-blocked' $false $cacheTool @('-WorkId', 'unmarked', '-CacheRoot', $cacheRoot, '-Cleanup')
    Invoke-Case 'G5-marker-cleanup' $true $cacheTool @('-WorkId', 'guard-selftest', '-CacheRoot', $cacheRoot, '-Cleanup')

    Invoke-Case 'G6-full-history-brief-blocked' $false (Join-Path $PSScriptRoot 'Test-AgentBrief.ps1') @('-BriefPath', $badBrief)
    Invoke-Case 'G6-packet-only-brief-pass' $true (Join-Path $PSScriptRoot 'Test-AgentBrief.ps1') @('-BriefPath', $goodBrief)
    Invoke-Case 'G7-unknown-status-blocked' $false (Join-Path $PSScriptRoot 'Test-VerificationCurrentState.ps1') @('-StatePath', $unknownStatusState)
    Invoke-Case 'G7-valid-but-stale-status-only-blocked' $false (Join-Path $PSScriptRoot 'Test-VerificationCurrentState.ps1') @('-StatePath', $statusOnlyStaleState, '-ExpectedStatus', 'ready-for-verification')
    Invoke-Case 'G7-current-state-pass' $true (Join-Path $PSScriptRoot 'Test-VerificationCurrentState.ps1') @('-StatePath', $goodState, '-ExpectedWorkId', 'guard-selftest', '-ExpectedRunId', 'run-001', '-ExpectedCandidateFingerprint', 'fingerprint-001', '-ExpectedStatus', 'ready-for-verification')

    Invoke-Case 'G8-token-parameter-omitted-blocked' $false (Join-Path $PSScriptRoot 'Invoke-UnityEditModeTests.ps1') @(
        '-ProjectPath', $sourceProject, '-UnityPath', (Join-Path $temporaryRoot 'never-unity.exe'),
        '-ResultsPath', (Join-Path $temporaryRoot 'never-results.xml'), '-LogPath', (Join-Path $temporaryRoot 'never-unity.log')
    )
    Invoke-Case 'integration-valid-preflight' $true $wrapper (@('-CriterionId', 'integration', '-Route', 'UnityEditMode', '-LedgerPath', $ledger) + $common)

    [pscustomobject]@{
        schema_version = 1
        suite = 'verification-cost-guards-negative-control'
        passed = (@($results | Where-Object { -not $_.passed }).Count -eq 0)
        cases = $results
        actual_cost = [ordered]@{
            powershell_dummy_bundle = 1
            unity_starts = $unityStartCount
            mcp_starts = 0
            build_starts = 0
        }
        temporary_files_removed_by_finally = $true
    } | ConvertTo-Json -Depth 30
}
finally {
    if (Test-Path -LiteralPath $temporaryRoot -PathType Container) {
        $resolvedTemp = [System.IO.Path]::GetFullPath($temporaryRoot)
        $systemTemp = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
        if (-not $resolvedTemp.StartsWith($systemTemp, [System.StringComparison]::OrdinalIgnoreCase) -or
            -not ([System.IO.Path]::GetFileName($resolvedTemp)).StartsWith('last-host-guard-selftest-', [System.StringComparison]::Ordinal)) {
            throw "Refusing unsafe self-test cleanup: $resolvedTemp"
        }
        Remove-Item -LiteralPath $resolvedTemp -Recurse -Force
    }
}
