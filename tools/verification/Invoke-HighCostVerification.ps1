#Requires -Version 7.0

[CmdletBinding(DefaultParameterSetName = 'Verify')]
param(
    [Parameter(Mandatory = $true)][ValidatePattern('^[A-Za-z0-9][A-Za-z0-9._-]{0,79}$')][string]$WorkId,
    [Parameter(Mandatory = $true)][ValidatePattern('^[A-Za-z0-9][A-Za-z0-9._-]{0,79}$')][string]$CriterionId,
    [Parameter(Mandatory = $true)][string]$LedgerPath,

    [Parameter(Mandatory = $true, ParameterSetName = 'Verify')][string]$Route,
    [Parameter(Mandatory = $true, ParameterSetName = 'Verify')][string]$RunId,
    [Parameter(Mandatory = $true, ParameterSetName = 'Verify')][string]$CandidateFingerprint,
    [Parameter(Mandatory = $true, ParameterSetName = 'Verify')][string]$AgentBriefPath,
    [Parameter(Mandatory = $true, ParameterSetName = 'Verify')][string]$CurrentStatePath,
    [Parameter(Mandatory = $true, ParameterSetName = 'Verify')][string[]]$QaHarnessPath,
    [Parameter(Mandatory = $true, ParameterSetName = 'Verify')][string[]]$ContractBaselinePath,
    [Parameter(Mandatory = $true, ParameterSetName = 'Verify')][string[]]$ProductionPath,
    [Parameter(Mandatory = $true, ParameterSetName = 'Verify')][string[]]$TestPath,
    [Parameter(Mandatory = $true, ParameterSetName = 'Verify')][string]$SourceProjectPath,
    [Parameter(Mandatory = $true, ParameterSetName = 'Verify')][string]$CacheRoot,
    [Parameter(ParameterSetName = 'Verify')][string]$UnityPath,
    [Parameter(ParameterSetName = 'Verify')][string]$ResultsPath,
    [Parameter(ParameterSetName = 'Verify')][string]$LogPath,
    [Parameter(ParameterSetName = 'Verify')][string]$TestFilter,
    [Parameter(ParameterSetName = 'Verify')][ValidateRange(1, 86400)][int]$TimeoutSeconds = 1800,
    [Parameter(ParameterSetName = 'Verify')][switch]$PreflightOnly,

    [Parameter(Mandatory = $true, ParameterSetName = 'Reclassify')][switch]$RegisterReclassification,
    [Parameter(Mandatory = $true, ParameterSetName = 'Reclassify')][string]$ReclassificationId,
    [Parameter(Mandatory = $true, ParameterSetName = 'Reclassify')][ValidateSet('R1', 'R2', 'R3')][string]$NewRiskClass,
    [Parameter(Mandatory = $true, ParameterSetName = 'Reclassify')][string]$RootCause,
    [Parameter(Mandatory = $true, ParameterSetName = 'Reclassify')][string]$ChangePlan,

    [string]$CapabilityProfilePath = (Join-Path $PSScriptRoot 'verification-capabilities.json')
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'VerificationGuard.Common.ps1')

function New-Ledger {
    return [pscustomobject][ordered]@{
        schema_version = 1
        work_id = $WorkId
        entries = @()
    }
}

function Read-Ledger {
    $ledger = if (Test-Path -LiteralPath $LedgerPath -PathType Leaf) {
        Read-GuardJson -Path $LedgerPath
    } else { New-Ledger }
    if ([int]$ledger.schema_version -ne 1 -or [string]$ledger.work_id -cne $WorkId) {
        throw 'Attempt ledger schema or work_id mismatch.'
    }
    return $ledger
}

function Add-LedgerEntryAtomic(
    [string]$Outcome,
    [string]$EntryRunId,
    [string]$Fingerprint,
    [string]$EntryRoute,
    [string]$Note,
    [string]$EntryRootCause = '',
    [string]$EntryChangePlan = '',
    [string]$EntryRiskClass = ''
) {
    $ledgerFullPath = [System.IO.Path]::GetFullPath($LedgerPath)
    [System.IO.Directory]::CreateDirectory((Split-Path -Parent $ledgerFullPath)) | Out-Null
    $lockPath = "$ledgerFullPath.lock"
    $lockStream = $null
    $ownsLock = $false
    try {
        foreach ($attempt in 1..20) {
            try {
                $lockStream = [System.IO.File]::Open($lockPath, [System.IO.FileMode]::CreateNew, [System.IO.FileAccess]::Write, [System.IO.FileShare]::None)
                $ownsLock = $true
                break
            }
            catch [System.IO.IOException] {
                if ($attempt -eq 20) { throw "Attempt ledger lock is busy: $lockPath" }
                Start-Sleep -Milliseconds 50
            }
        }

        $current = Read-Ledger
        $duplicate = @($current.entries | Where-Object {
            [string]$_.criterion_id -ceq $CriterionId -and
            [string]$_.outcome -ceq $Outcome -and
            [string]$_.run_id -ceq $EntryRunId -and
            [string]$_.candidate_fingerprint -ceq $Fingerprint -and
            [string]$_.route -ceq $EntryRoute
        }).Count -gt 0
        if ($duplicate) {
            return [pscustomobject]@{ added = $false; ledger = $current }
        }

        $entries = @($current.entries)
        $entries += [pscustomobject][ordered]@{
            criterion_id = $CriterionId
            outcome = $Outcome
            run_id = $EntryRunId
            candidate_fingerprint = $Fingerprint
            route = $EntryRoute
            note = $Note
            root_cause = $EntryRootCause
            change_plan = $EntryChangePlan
            risk_class = $EntryRiskClass
            recorded_utc = [DateTimeOffset]::UtcNow.ToString('o')
        }
        $current.entries = $entries
        Write-GuardJsonAtomic -Path $ledgerFullPath -Value $current
        return [pscustomobject]@{ added = $true; ledger = $current }
    }
    finally {
        if ($null -ne $lockStream) { $lockStream.Dispose() }
        if ($ownsLock -and (Test-Path -LiteralPath $lockPath -PathType Leaf)) { Remove-Item -LiteralPath $lockPath -Force }
    }
}

function Get-ConsecutiveFailures($Ledger) {
    $count = 0
    $criterionEntries = @($Ledger.entries | Where-Object { [string]$_.criterion_id -ceq $CriterionId })
    [array]::Reverse($criterionEntries)
    foreach ($entry in $criterionEntries) {
        if ([string]$entry.outcome -eq 'failure') { $count++; continue }
        break
    }
    return $count
}

function Invoke-Guard([string]$ScriptName, [System.Collections.IDictionary]$Parameters) {
    $scriptPath = Join-Path $PSScriptRoot $ScriptName
    $payload = [ordered]@{
        script_path = $scriptPath
        parameters = $Parameters
    }
    $payloadBase64 = [Convert]::ToBase64String([System.Text.UTF8Encoding]::new($false).GetBytes(($payload | ConvertTo-Json -Depth 20)))
    $command = @'
$payload = [System.Text.Encoding]::UTF8.GetString([Convert]::FromBase64String('__PAYLOAD__')) | ConvertFrom-Json -Depth 20
$invocation = @{}
foreach ($property in $payload.parameters.PSObject.Properties) {
    $entry = $property.Value
    $values = @($entry.values | ForEach-Object { $_ })
    switch ([string]$entry.kind) {
        'scalar' {
            if ($values.Count -ne 1) { throw "Scalar payload '$($property.Name)' must contain exactly one value." }
            $invocation[$property.Name] = [string]$values[0]
        }
        'array' {
            $invocation[$property.Name] = [string[]]$values
        }
        'switch' {
            if ($values.Count -ne 1 -or -not [bool]$values[0]) { throw "Switch payload '$($property.Name)' must contain true." }
            $invocation[$property.Name] = $true
        }
        default { throw "Unsupported payload kind '$([string]$entry.kind)' for '$($property.Name)'." }
    }
}
& ([string]$payload.script_path) @invocation
exit $LASTEXITCODE
'@.Replace('__PAYLOAD__', $payloadBase64)
    $encodedCommand = [Convert]::ToBase64String([System.Text.Encoding]::Unicode.GetBytes($command))
    $output = & (Get-Process -Id $PID).Path -NoProfile -EncodedCommand $encodedCommand 2>&1
    $exitCode = $LASTEXITCODE
    if ($exitCode -ne 0) {
        throw "$ScriptName blocked preflight (exit $exitCode): $($output -join [Environment]::NewLine)"
    }
    return ($output -join [Environment]::NewLine)
}

function Get-RouteCurrentStateContract($RouteCapability, [string[]]$AllowedStatuses) {
    $contractProperty = $RouteCapability.PSObject.Properties['current_state']
    if ($null -eq $contractProperty) { throw "Route '$Route' is missing current_state contract." }
    $contract = $contractProperty.Value
    if ([string]::IsNullOrWhiteSpace([string]$contract.expected_status)) {
        throw "Route '$Route' current_state.expected_status cannot be empty."
    }
    if (@($contract.allowed_transitions).Count -eq 0) {
        throw "Route '$Route' current_state.allowed_transitions cannot be empty."
    }
    if (-not ($AllowedStatuses -ccontains [string]$contract.expected_status)) {
        throw "Route '$Route' expected status is not in current_state_contract.allowed_statuses."
    }
    foreach ($transition in @($contract.allowed_transitions)) {
        if (-not ($AllowedStatuses -ccontains [string]$transition.from) -or
            -not ($AllowedStatuses -ccontains [string]$transition.to)) {
            throw "Route '$Route' transition contains a status outside current_state_contract.allowed_statuses."
        }
    }
    return $contract
}

function Assert-RouteStatusTransition($RouteContract, [string]$From, [string]$To) {
    $allowed = @($RouteContract.allowed_transitions | Where-Object {
        [string]$_.from -ceq $From -and [string]$_.to -ceq $To
    }).Count -gt 0
    if (-not $allowed) { throw "Route '$Route' does not allow current-state transition '$From' -> '$To'." }
}

$profile = Read-GuardJson -Path $CapabilityProfilePath
$ledger = Read-Ledger

if ($RegisterReclassification) {
    $failureCount = Get-ConsecutiveFailures -Ledger $ledger
    if ($failureCount -lt [int]$profile.max_consecutive_failures) {
        throw "Reclassification requires $($profile.max_consecutive_failures) consecutive failures; current count is $failureCount."
    }
    if ([string]::IsNullOrWhiteSpace($RootCause) -or $RootCause.Length -lt 10) {
        throw 'RootCause must be a separate value of at least 10 characters.'
    }
    if ([string]::IsNullOrWhiteSpace($ChangePlan) -or $ChangePlan.Length -lt 10) {
        throw 'ChangePlan must be a separate value of at least 10 characters.'
    }
    [void](Add-LedgerEntryAtomic -Outcome 'reclassified' -EntryRunId $ReclassificationId -Fingerprint '' -EntryRoute '' `
        -Note 'risk/root-cause reclassification' -EntryRootCause $RootCause -EntryChangePlan $ChangePlan -EntryRiskClass $NewRiskClass)
    [pscustomobject]@{ status = 'reclassified'; work_id = $WorkId; criterion_id = $CriterionId; reclassification_id = $ReclassificationId; risk_class = $NewRiskClass } |
        ConvertTo-Json -Depth 5
    exit 0
}

$failureCount = Get-ConsecutiveFailures -Ledger $ledger
if ($failureCount -ge [int]$profile.max_consecutive_failures) {
    throw "Criterion '$CriterionId' has $failureCount consecutive failures. Register a risk/root-cause reclassification before another high-cost attempt."
}

try {
    $routeProperty = $profile.routes.PSObject.Properties[$Route]
    if ($null -eq $routeProperty) {
        throw "Unknown verification route '$Route'. Register a capability or choose a listed route."
    }
    $routeCapability = $routeProperty.Value
    if (-not [bool]$routeCapability.available) {
        throw "Verification route '$Route' is unavailable: $($routeCapability.failure_reason). Fallback: $($routeCapability.fallback)"
    }
    if ([string]$routeCapability.cost_class -ne 'high') {
        throw "Route '$Route' is not a high-cost route and must not use this wrapper."
    }
    $allowedStatuses = @($profile.current_state_contract.allowed_statuses | ForEach-Object { [string]$_ })
    $routeStateContract = Get-RouteCurrentStateContract -RouteCapability $routeCapability -AllowedStatuses $allowedStatuses

    [void](Invoke-Guard -ScriptName 'Test-AgentBrief.ps1' -Parameters ([ordered]@{
        BriefPath = [ordered]@{ kind = 'scalar'; values = @($AgentBriefPath) }
        CapabilityProfilePath = [ordered]@{ kind = 'scalar'; values = @($CapabilityProfilePath) }
    }))
    [void](Invoke-Guard -ScriptName 'Test-VerificationCurrentState.ps1' -Parameters ([ordered]@{
        StatePath = [ordered]@{ kind = 'scalar'; values = @($CurrentStatePath) }
        ExpectedWorkId = [ordered]@{ kind = 'scalar'; values = @($WorkId) }
        ExpectedRunId = [ordered]@{ kind = 'scalar'; values = @($RunId) }
        ExpectedCandidateFingerprint = [ordered]@{ kind = 'scalar'; values = @($CandidateFingerprint) }
        ExpectedStatus = [ordered]@{ kind = 'scalar'; values = @([string]$routeStateContract.expected_status) }
        CapabilityProfilePath = [ordered]@{ kind = 'scalar'; values = @($CapabilityProfilePath) }
    }))
    [void](Invoke-Guard -ScriptName 'Test-QaHarnessSafety.ps1' -Parameters ([ordered]@{
        Path = [ordered]@{ kind = 'array'; values = @($QaHarnessPath) }
    }))
    [void](Invoke-Guard -ScriptName 'Test-ComponentContractImpact.ps1' -Parameters ([ordered]@{
        BaselinePath = [ordered]@{ kind = 'array'; values = @($ContractBaselinePath) }
        CandidatePath = [ordered]@{ kind = 'array'; values = @($ProductionPath) }
        TestPath = [ordered]@{ kind = 'array'; values = @($TestPath) }
    }))

    $syncOutput = Invoke-Guard -ScriptName 'Sync-IsolatedUnityProject.ps1' -Parameters ([ordered]@{
        WorkId = [ordered]@{ kind = 'scalar'; values = @($WorkId) }
        CacheRoot = [ordered]@{ kind = 'scalar'; values = @($CacheRoot) }
        SourceProjectPath = [ordered]@{ kind = 'scalar'; values = @($SourceProjectPath) }
        Sync = [ordered]@{ kind = 'switch'; values = @($true) }
    })
    $syncResult = $syncOutput | ConvertFrom-Json -Depth 10
}
catch {
    $failureRecord = Add-LedgerEntryAtomic -Outcome 'failure' -EntryRunId $RunId -Fingerprint $CandidateFingerprint `
        -EntryRoute $Route -Note $_.Exception.Message
    if (-not $failureRecord.added) {
        Write-Warning 'Preflight failed; the same run identity was already recorded and was not duplicated.'
    }
    throw
}

if ($PreflightOnly) {
    [pscustomobject]@{
        status = 'preflight-pass'
        route = $Route
        work_id = $WorkId
        criterion_id = $CriterionId
        run_id = $RunId
        candidate_fingerprint = $CandidateFingerprint
        consecutive_failures = $failureCount
        isolated_project = $syncResult.isolated_project
        high_cost_started = $false
    } | ConvertTo-Json -Depth 8
    exit 0
}

try {
    if ($Route -cne 'UnityEditMode') { throw "No executor is implemented for available route '$Route'." }
    foreach ($requiredPath in @($UnityPath, $ResultsPath, $LogPath)) {
        if ([string]::IsNullOrWhiteSpace($requiredPath)) { throw 'UnityPath, ResultsPath, and LogPath are required for execution.' }
    }

    [void](Invoke-Guard -ScriptName 'Test-VerificationCurrentState.ps1' -Parameters ([ordered]@{
        StatePath = [ordered]@{ kind = 'scalar'; values = @($CurrentStatePath) }
        ExpectedWorkId = [ordered]@{ kind = 'scalar'; values = @($WorkId) }
        ExpectedRunId = [ordered]@{ kind = 'scalar'; values = @($RunId) }
        ExpectedCandidateFingerprint = [ordered]@{ kind = 'scalar'; values = @($CandidateFingerprint) }
        ExpectedStatus = [ordered]@{ kind = 'scalar'; values = @([string]$routeStateContract.expected_status) }
        CapabilityProfilePath = [ordered]@{ kind = 'scalar'; values = @($CapabilityProfilePath) }
    }))
    $state = Read-GuardJson -Path $CurrentStatePath
    $runningStatus = 'verification-running'
    Assert-RouteStatusTransition -RouteContract $routeStateContract -From ([string]$state.status) -To $runningStatus

    $instanceRoot = Split-Path -Parent ([string]$syncResult.isolated_project)
    $markerPath = Join-Path $instanceRoot '.last-host-isolated-unity-cache.json'
    $marker = Read-GuardJson -Path $markerPath
    $tokenDirectory = Join-Path $instanceRoot 'tokens'
    [System.IO.Directory]::CreateDirectory($tokenDirectory) | Out-Null
    $nonce = [guid]::NewGuid().ToString('N')
    $tokenPath = Join-Path $tokenDirectory "$RunId-$nonce.json"
    $signature = Get-GuardTokenSignature -WorkId $WorkId -RunId $RunId -CandidateFingerprint $CandidateFingerprint `
        -ProjectPath ([string]$syncResult.isolated_project) -Nonce $nonce -MarkerCreatedUtc ([string]$marker.created_utc)
    Write-GuardJsonAtomic -Path $tokenPath -Value ([ordered]@{
        schema_version = 1
        issued_by = 'Invoke-HighCostVerification.ps1'
        target = 'Invoke-UnityEditModeTests.ps1'
        work_id = $WorkId
        run_id = $RunId
        candidate_fingerprint = $CandidateFingerprint
        project_path = [string]$syncResult.isolated_project
        nonce = $nonce
        issued_utc = [DateTimeOffset]::UtcNow.ToString('o')
        expires_utc = [DateTimeOffset]::UtcNow.AddMinutes(5).ToString('o')
        signature = $signature
    })

    $state.cost.unity_starts = [int]$state.cost.unity_starts + 1
    $state.cost.recorded_high_cost_attempts = [int]$state.cost.recorded_high_cost_attempts + 1
    $state.status = $runningStatus
    Write-GuardJsonAtomic -Path $CurrentStatePath -Value $state
}
catch {
    [void](Add-LedgerEntryAtomic -Outcome 'failure' -EntryRunId $RunId -Fingerprint $CandidateFingerprint `
        -EntryRoute $Route -Note $_.Exception.Message)
    throw
}

$runnerArguments = @(
    '-NoProfile', '-File', (Join-Path $PSScriptRoot 'Invoke-UnityEditModeTests.ps1'),
    '-ProjectPath', [string]$syncResult.isolated_project,
    '-UnityPath', $UnityPath,
    '-ResultsPath', $ResultsPath,
    '-LogPath', $LogPath,
    '-TimeoutSeconds', [string]$TimeoutSeconds,
    '-GuardTokenPath', $tokenPath
)
if (-not [string]::IsNullOrWhiteSpace($TestFilter)) { $runnerArguments += @('-TestFilter', $TestFilter) }
$runnerOutput = & (Get-Process -Id $PID).Path @runnerArguments 2>&1
$runnerExitCode = $LASTEXITCODE
$outcome = if ($runnerExitCode -eq 0) { 'success' } else { 'failure' }
[void](Add-LedgerEntryAtomic -Outcome $outcome -EntryRunId $RunId -Fingerprint $CandidateFingerprint -EntryRoute $Route -Note "low-level exit $runnerExitCode")
$runnerOutput | ForEach-Object { Write-Output $_ }
if ($runnerExitCode -ne 0) { exit $runnerExitCode }
