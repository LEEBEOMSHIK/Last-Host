#Requires -Version 7.0

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)][string]$StatePath,
    [string]$ExpectedWorkId,
    [string]$ExpectedRunId,
    [string]$ExpectedCandidateFingerprint,
    [string]$ExpectedStatus,
    [string]$CapabilityProfilePath = (Join-Path $PSScriptRoot 'verification-capabilities.json')
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'VerificationGuard.Common.ps1')

$state = Read-GuardJson -Path $StatePath
$profile = Read-GuardJson -Path $CapabilityProfilePath
$errors = [System.Collections.Generic.List[string]]::new()
$contractProperty = $profile.PSObject.Properties['current_state_contract']
if ($null -eq $contractProperty) {
    $errors.Add('capability profile missing current_state_contract')
    $allowedStatuses = @()
}
else {
    $allowedStatuses = @($contractProperty.Value.allowed_statuses | ForEach-Object { [string]$_ })
    if ($allowedStatuses.Count -eq 0 -or @($allowedStatuses | Where-Object { [string]::IsNullOrWhiteSpace($_) }).Count -gt 0) {
        $errors.Add('capability profile current_state_contract.allowed_statuses must contain non-empty values')
    }
}
$required = @('schema_version', 'work_id', 'status', 'run_id', 'candidate_fingerprint', 'cost', 'evidence')
foreach ($property in $required) {
    if ($null -eq $state.PSObject.Properties[$property]) { $errors.Add("missing required property: $property") }
}
if ($errors.Count -eq 0) {
    $actualStatus = [string]$state.status
    if (-not ($allowedStatuses -ccontains $actualStatus)) { $errors.Add("unknown status: $actualStatus") }
    if ($ExpectedStatus) {
        if (-not ($allowedStatuses -ccontains $ExpectedStatus)) { $errors.Add("expected status is not allowed by profile: $ExpectedStatus") }
        elseif ($actualStatus -cne $ExpectedStatus) { $errors.Add("status does not match request: expected '$ExpectedStatus', actual '$actualStatus'") }
    }
    if ($ExpectedWorkId -and [string]$state.work_id -cne $ExpectedWorkId) { $errors.Add('work_id does not match request') }
    if ($ExpectedRunId -and [string]$state.run_id -cne $ExpectedRunId) { $errors.Add('run_id does not match request') }
    if ($ExpectedCandidateFingerprint -and [string]$state.candidate_fingerprint -cne $ExpectedCandidateFingerprint) { $errors.Add('candidate_fingerprint does not match request') }
    if ([string]::IsNullOrWhiteSpace([string]$state.run_id)) { $errors.Add('run_id cannot be empty') }
    if ([string]::IsNullOrWhiteSpace([string]$state.candidate_fingerprint)) { $errors.Add('candidate_fingerprint cannot be empty') }

    $costRequired = @('unity_starts', 'mcp_starts', 'build_starts', 'recorded_high_cost_attempts')
    foreach ($property in $costRequired) {
        if ($null -eq $state.cost.PSObject.Properties[$property]) { $errors.Add("cost missing property: $property") }
        elseif ([int]$state.cost.$property -lt 0) { $errors.Add("cost.$property cannot be negative") }
    }
    if ($errors.Count -eq 0) {
        $sum = [int]$state.cost.unity_starts + [int]$state.cost.mcp_starts + [int]$state.cost.build_starts
        if ($sum -ne [int]$state.cost.recorded_high_cost_attempts) {
            $errors.Add("cost mismatch: starts sum $sum != recorded_high_cost_attempts $($state.cost.recorded_high_cost_attempts)")
        }
    }
    foreach ($evidence in @($state.evidence)) {
        if ([string]$evidence.run_id -cne [string]$state.run_id) { $errors.Add('evidence run_id is stale') }
        if ([string]$evidence.candidate_fingerprint -cne [string]$state.candidate_fingerprint) { $errors.Add('evidence candidate_fingerprint is stale') }
    }
    if ([string]$state.status -eq 'technical-pass' -and @($state.evidence).Count -eq 0) {
        $errors.Add('technical-pass requires current evidence')
    }
}

Write-GuardResult -Check 'verification-current-state' -Passed ($errors.Count -eq 0) -Details $errors
if ($errors.Count -gt 0) { exit 1 }
