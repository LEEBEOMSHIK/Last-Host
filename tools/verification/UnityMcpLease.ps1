#Requires -Version 7.0

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateSet('Acquire', 'Renew', 'Release', 'Status')]
    [string]$Action,

    [Parameter(Mandatory = $true)]
    [string]$ProjectPath,

    [Alias('Owner')]
    [string]$Agent,
    [string]$WorkId,
    [string]$RunId,
    [Alias('ProcessId')]
    [Nullable[int]]$EditorProcessId,
    [string]$Scene,
    [Nullable[bool]]$BaselinePlay,
    [Nullable[bool]]$BaselinePause,
    [string]$BaselineScene,
    [Nullable[bool]]$BaselineDirty,
    [ValidateRange(30, 86400)]
    [int]$TtlSeconds = 300
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-CanonicalProjectPath {
    param([string]$Path)

    if (-not (Test-Path -LiteralPath $Path -PathType Container)) {
        throw "ProjectPath directory does not exist: $Path"
    }

    return [System.IO.Path]::GetFullPath((Resolve-Path -LiteralPath $Path).Path).TrimEnd('\', '/')
}

function Assert-IdentityArguments {
    foreach ($entry in @(
        @{ Name = 'Agent'; Value = $Agent },
        @{ Name = 'WorkId'; Value = $WorkId },
        @{ Name = 'RunId'; Value = $RunId }
    )) {
        if ([string]::IsNullOrWhiteSpace([string]$entry.Value)) {
            throw "$($entry.Name) is required for $Action."
        }
    }
}

function Read-LeaseFromStream {
    param([System.IO.FileStream]$Stream)

    $Stream.Position = 0
    $reader = [System.IO.StreamReader]::new($Stream, [System.Text.UTF8Encoding]::new($false), $true, 1024, $true)
    try {
        $json = $reader.ReadToEnd()
    }
    finally {
        $reader.Dispose()
    }

    if ([string]::IsNullOrWhiteSpace($json)) {
        throw 'Lease file is empty.'
    }

    return $json | ConvertFrom-Json
}

function Assert-LeaseIdentity {
    param($Lease)

    if ($Lease.agent -cne $Agent -or $Lease.work_id -cne $WorkId -or $Lease.run_id -cne $RunId) {
        throw "Lease identity mismatch. Active agent/work_id/run_id is '$($Lease.agent)'/'$($Lease.work_id)'/'$($Lease.run_id)'."
    }
}

function Assert-AcquireArguments {
    foreach ($entry in @(
        @{ Name = 'Scene'; Value = $Scene },
        @{ Name = 'BaselineScene'; Value = $BaselineScene }
    )) {
        if ([string]::IsNullOrWhiteSpace([string]$entry.Value)) {
            throw "$($entry.Name) is required for Acquire. Use an explicit sentinel such as '(none)' when no scene is loaded."
        }
    }

    if ($null -eq $EditorProcessId -or [int]$EditorProcessId -le 0) {
        throw 'EditorProcessId is required for Acquire and must be the positive PID of the Unity Editor or batch Unity process that owns the session.'
    }

    foreach ($entry in @(
        @{ Name = 'BaselinePlay'; Value = $BaselinePlay },
        @{ Name = 'BaselinePause'; Value = $BaselinePause },
        @{ Name = 'BaselineDirty'; Value = $BaselineDirty }
    )) {
        if ($null -eq $entry.Value) {
            throw "$($entry.Name) is required for Acquire and must be an explicit boolean."
        }
    }
}

function Write-LeaseToStream {
    param(
        [System.IO.FileStream]$Stream,
        $Lease
    )

    $json = $Lease | ConvertTo-Json -Depth 5
    $bytes = [System.Text.UTF8Encoding]::new($false).GetBytes($json + [Environment]::NewLine)
    $Stream.Position = 0
    $Stream.SetLength(0)
    $Stream.Write($bytes, 0, $bytes.Length)
    $Stream.Flush($true)
}

function Add-LeaseStatus {
    param($Lease)

    $now = [DateTimeOffset]::UtcNow
    $expires = [DateTimeOffset]::Parse([string]$Lease.expires_utc)
    $processAlive = $false
    try {
        $processAlive = $null -ne (Get-Process -Id ([int]$Lease.editor_pid) -ErrorAction Stop)
    }
    catch {
        $processAlive = $false
    }

    $Lease | Add-Member -NotePropertyName expired -NotePropertyValue ($expires -le $now) -Force
    $Lease | Add-Member -NotePropertyName process_alive -NotePropertyValue $processAlive -Force
    $Lease | Add-Member -NotePropertyName automatic_takeover_allowed -NotePropertyValue $false -Force
    return $Lease
}

$canonicalProjectPath = Get-CanonicalProjectPath -Path $ProjectPath
$leaseDirectory = Join-Path $canonicalProjectPath 'Temp'
$leasePath = Join-Path $leaseDirectory 'last-host-unity-mcp-lease.json'

if ($Action -eq 'Status') {
    if (-not (Test-Path -LiteralPath $leasePath -PathType Leaf)) {
        [pscustomobject]@{
            state = 'Available'
            project_path = $canonicalProjectPath
            lease_path = $leasePath
            automatic_takeover_allowed = $false
        } | ConvertTo-Json -Depth 5
        return
    }

    $stream = [System.IO.File]::Open($leasePath, [System.IO.FileMode]::Open, [System.IO.FileAccess]::Read, [System.IO.FileShare]::ReadWrite)
    try {
        $lease = Read-LeaseFromStream -Stream $stream
    }
    finally {
        $stream.Dispose()
    }

    Add-LeaseStatus -Lease $lease | ConvertTo-Json -Depth 5
    return
}

Assert-IdentityArguments

if ($Action -eq 'Acquire') {
    Assert-AcquireArguments
    [System.IO.Directory]::CreateDirectory($leaseDirectory) | Out-Null
    $now = [DateTimeOffset]::UtcNow
    $lease = [ordered]@{
        schema_version = 2
        project_path = $canonicalProjectPath
        work_id = $WorkId
        agent = $Agent
        run_id = $RunId
        editor_pid = [int]$EditorProcessId
        scene = $Scene
        ttl_seconds = $TtlSeconds
        acquired_utc = $now.ToString('o')
        heartbeat_utc = $now.ToString('o')
        expires_utc = $now.AddSeconds($TtlSeconds).ToString('o')
        baseline_play = [bool]$BaselinePlay
        baseline_pause = [bool]$BaselinePause
        baseline_scene = $BaselineScene
        baseline_dirty = [bool]$BaselineDirty
    }

    try {
        $stream = [System.IO.File]::Open($leasePath, [System.IO.FileMode]::CreateNew, [System.IO.FileAccess]::Write, [System.IO.FileShare]::None)
        try {
            Write-LeaseToStream -Stream $stream -Lease $lease
        }
        finally {
            $stream.Dispose()
        }
    }
    catch [System.IO.IOException] {
        if (Test-Path -LiteralPath $leasePath -PathType Leaf) {
            throw "Unity MCP lease already exists. Inspect it with Status; expiry never authorizes automatic takeover: $leasePath"
        }
        throw
    }

    [pscustomobject]$lease | ConvertTo-Json -Depth 5
    return
}

if (-not (Test-Path -LiteralPath $leasePath -PathType Leaf)) {
    throw "No Unity MCP lease exists for project: $canonicalProjectPath"
}

if ($Action -eq 'Renew') {
    $stream = [System.IO.File]::Open($leasePath, [System.IO.FileMode]::Open, [System.IO.FileAccess]::ReadWrite, [System.IO.FileShare]::None)
    try {
        $lease = Read-LeaseFromStream -Stream $stream
        Assert-LeaseIdentity -Lease $lease

        $now = [DateTimeOffset]::UtcNow
        $lease.ttl_seconds = $TtlSeconds
        $lease.heartbeat_utc = $now.ToString('o')
        $lease.expires_utc = $now.AddSeconds($TtlSeconds).ToString('o')
        Write-LeaseToStream -Stream $stream -Lease $lease
    }
    finally {
        $stream.Dispose()
    }

    $lease | ConvertTo-Json -Depth 5
    return
}

$stream = [System.IO.File]::Open($leasePath, [System.IO.FileMode]::Open, [System.IO.FileAccess]::ReadWrite, [System.IO.FileShare]::None)
try {
    $lease = Read-LeaseFromStream -Stream $stream
    Assert-LeaseIdentity -Lease $lease
}
finally {
    $stream.Dispose()
}

Remove-Item -LiteralPath $leasePath
[pscustomobject]@{
    state = 'Released'
    project_path = $canonicalProjectPath
    work_id = $WorkId
    agent = $Agent
    run_id = $RunId
    released_utc = [DateTimeOffset]::UtcNow.ToString('o')
} | ConvertTo-Json -Depth 5
