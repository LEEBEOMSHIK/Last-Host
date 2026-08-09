[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = 'High')]
param(
    [switch]$Apply,

    [ValidateRange(0, 30)]
    [int]$VerifyDelaySeconds = 2
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-CodexUnityMcpRelaySnapshot {
    $userProfilePath = [Environment]::GetFolderPath([Environment+SpecialFolder]::UserProfile)
    $expectedRelayPath = [System.IO.Path]::GetFullPath(
        [System.IO.Path]::Combine($userProfilePath, '.unity', 'relay', 'relay_win.exe')
    )

    $allProcesses = @(Get-CimInstance Win32_Process)
    $processesById = @{}
    foreach ($process in $allProcesses) {
        $processesById[[int]$process.ProcessId] = $process
    }

    $targets = [System.Collections.Generic.List[object]]::new()
    $skipped = [System.Collections.Generic.List[object]]::new()

    foreach ($process in $allProcesses) {
        if ($process.Name -ne 'relay_win.exe') {
            continue
        }

        $commandLine = [string]$process.CommandLine
        $executablePath = [string]$process.ExecutablePath
        $isMcpClient = $commandLine -match '(?i)(?:^|\s)--mcp(?:\s|$)'
        $isEditorRelay = $commandLine -match '(?i)(?:^|\s)--relay(?:\s|$)'
        $pathMatches = -not [string]::IsNullOrWhiteSpace($executablePath) -and
            [System.IO.Path]::GetFullPath($executablePath).Equals(
                $expectedRelayPath,
                [System.StringComparison]::OrdinalIgnoreCase
            )

        $parent = $null
        $parentPid = [int]$process.ParentProcessId
        if ($processesById.ContainsKey($parentPid)) {
            $parent = $processesById[$parentPid]
        }

        $parentIsCodex = $null -ne $parent -and $parent.Name -eq 'codex.exe'
        $relayStartTimeUtcTicks = 0L
        $parentStartTimeUtcTicks = 0L
        try {
            $relayStartTimeUtcTicks = (Get-Process -Id ([int]$process.ProcessId) -ErrorAction Stop).StartTime.ToUniversalTime().Ticks
            if ($null -ne $parent) {
                $parentStartTimeUtcTicks = (Get-Process -Id $parentPid -ErrorAction Stop).StartTime.ToUniversalTime().Ticks
            }
        }
        catch {
            # A process that changes during discovery is not a safe termination target.
        }

        $parentPredatesRelay = $parentStartTimeUtcTicks -gt 0 -and
            $parentStartTimeUtcTicks -le $relayStartTimeUtcTicks

        $instanceKey = '{0}:{1}:{2}:{3}' -f @(
            [int]$process.ProcessId,
            $relayStartTimeUtcTicks,
            $parentPid,
            $parentStartTimeUtcTicks
        )
        $record = [pscustomobject]@{
            RelayPid                = [int]$process.ProcessId
            RelayStartTimeUtcTicks  = $relayStartTimeUtcTicks
            ParentPid               = $parentPid
            ParentStartTimeUtcTicks = $parentStartTimeUtcTicks
            ParentName              = if ($null -ne $parent) { [string]$parent.Name } else { $null }
            ExecutablePath          = $executablePath
            CommandLine             = $commandLine
            InstanceKey             = $instanceKey
            ParentPredatesRelay     = $parentPredatesRelay
        }

        if ($isMcpClient -and -not $isEditorRelay -and $pathMatches -and $parentIsCodex -and
            $relayStartTimeUtcTicks -gt 0 -and $parentPredatesRelay) {
            $targets.Add($record)
        }
        elseif ($isMcpClient -or $isEditorRelay) {
            $skipped.Add($record)
        }
    }

    [pscustomobject]@{
        ExpectedRelayPath = $expectedRelayPath
        Targets           = @($targets)
        Skipped           = @($skipped)
    }
}

function Get-ValidatedRelayProcess {
    param(
        [Parameter(Mandatory)]
        [psobject]$Target
    )

    $snapshot = Get-CodexUnityMcpRelaySnapshot
    $matching = @($snapshot.Targets | Where-Object { $_.InstanceKey -eq $Target.InstanceKey })
    if ($matching.Count -ne 1) {
        return $null
    }

    try {
        $nativeProcess = Get-Process -Id $Target.RelayPid -ErrorAction Stop
        if ($nativeProcess.StartTime.ToUniversalTime().Ticks -ne $Target.RelayStartTimeUtcTicks) {
            return $null
        }
        return $nativeProcess
    }
    catch {
        return $null
    }
}

$before = Get-CodexUnityMcpRelaySnapshot
$targetPids = @($before.Targets | ForEach-Object { $_.RelayPid })
$targetInstanceKeys = @($before.Targets | ForEach-Object { $_.InstanceKey })
$stoppedPids = [System.Collections.Generic.List[int]]::new()
$failed = [System.Collections.Generic.List[object]]::new()
$isWhatIf = [bool]$WhatIfPreference

if ($Apply) {
    foreach ($target in $before.Targets) {
        $validatedProcess = Get-ValidatedRelayProcess -Target $target
        if ($null -eq $validatedProcess) {
            $failed.Add([pscustomobject]@{
                RelayPid = $target.RelayPid
                Error    = 'Process instance disappeared or no longer matched the strict target predicate.'
            })
            continue
        }

        if ($PSCmdlet.ShouldProcess(
            "relay PID $($target.RelayPid), parent Codex PID $($target.ParentPid)",
            'Stop Codex Unity MCP client relay'
        )) {
            try {
                Stop-Process -InputObject $validatedProcess -ErrorAction Stop
                $stoppedPids.Add($target.RelayPid)
            }
            catch {
                $failed.Add([pscustomobject]@{
                    RelayPid = $target.RelayPid
                    Error    = $_.Exception.Message
                })
            }
        }
    }

    if (-not $isWhatIf -and $VerifyDelaySeconds -gt 0) {
        Start-Sleep -Seconds $VerifyDelaySeconds
    }
}

$after = Get-CodexUnityMcpRelaySnapshot
$remainingPids = @($after.Targets | ForEach-Object { $_.RelayPid })
$respawned = @($after.Targets | Where-Object { $_.InstanceKey -notin $targetInstanceKeys })

[pscustomobject]@{
    Mode                 = if ($isWhatIf) { 'WhatIf' } elseif ($Apply) { 'Apply' } else { 'Inspect' }
    Success              = if ($isWhatIf) { $failed.Count -eq 0 } elseif ($Apply) { $failed.Count -eq 0 -and $remainingPids.Count -eq 0 } else { $true }
    TargetCount          = $targetPids.Count
    Targets              = @($before.Targets)
    StoppedCount         = $stoppedPids.Count
    StoppedPids          = @($stoppedPids)
    FailedCount          = $failed.Count
    Failures             = @($failed)
    RemainingCount       = $remainingPids.Count
    Remaining            = @($after.Targets)
    RespawnedCount       = $respawned.Count
    Respawned            = $respawned
    SkippedCount         = @($before.Skipped).Count
    Skipped              = @($before.Skipped)
    EditorRelayPreserved = @($before.Skipped | Where-Object {
        $_.CommandLine -match '(?i)(?:^|\s)--relay(?:\s|$)'
    }).Count -gt 0
}
