#Requires -Version 7.0

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)][string]$BriefPath,
    [string]$CapabilityProfilePath = (Join-Path $PSScriptRoot 'verification-capabilities.json')
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'VerificationGuard.Common.ps1')

$profile = Read-GuardJson -Path $CapabilityProfilePath
$brief = Read-GuardJson -Path $BriefPath
$errors = [System.Collections.Generic.List[string]]::new()

foreach ($property in @('work_id', 'context_mode', 'fork_turns', 'required_files', 'message')) {
    if ($null -eq $brief.PSObject.Properties[$property]) {
        $errors.Add("missing required property: $property")
    }
}
if ($errors.Count -eq 0) {
    if ([string]$brief.context_mode -cne [string]$profile.agent_brief.required_context_mode) {
        $errors.Add("context_mode must be '$($profile.agent_brief.required_context_mode)'")
    }
    if ([string]$brief.fork_turns -cne [string]$profile.agent_brief.required_fork_turns) {
        $errors.Add("fork_turns must be '$($profile.agent_brief.required_fork_turns)'")
    }
    if (@($brief.required_files).Count -gt [int]$profile.agent_brief.max_required_files) {
        $errors.Add("required_files exceeds limit $($profile.agent_brief.max_required_files)")
    }
    if (@($brief.required_files).Count -le 0) {
        $errors.Add('required_files must contain the packet entry point')
    }
    if ([string]$brief.message -match '(?i)full[- ]history|entire conversation|전체\s*(대화|이력)') {
        $errors.Add('message requests forbidden full-history context')
    }
    if (([string]$brief.message).Length -gt [int]$profile.agent_brief.max_message_characters) {
        $errors.Add("message exceeds character limit $($profile.agent_brief.max_message_characters)")
    }
    if ($null -ne $brief.PSObject.Properties['include_conversation_history'] -and [bool]$brief.include_conversation_history) {
        $errors.Add('include_conversation_history must be false')
    }
}

Write-GuardResult -Check 'agent-brief' -Passed ($errors.Count -eq 0) -Details $errors
if ($errors.Count -gt 0) { exit 1 }
