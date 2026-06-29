$ErrorActionPreference = "Stop"

function Stop-WithMessage {
    param(
        [Parameter(Mandatory = $true)]
        [string] $Message
    )

    [Console]::Error.WriteLine($Message)
    exit 1
}

$projectRoot = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot "..\..")).Path
$unityRoots = @()

function Test-UnityProjectRoot {
    param(
        [Parameter(Mandatory = $true)]
        [string] $Path
    )

    $projectVersion = Join-Path $Path "ProjectSettings\ProjectVersion.txt"
    $manifest = Join-Path $Path "Packages\manifest.json"
    return (Test-Path -LiteralPath $projectVersion) -and (Test-Path -LiteralPath $manifest)
}

if (Test-UnityProjectRoot -Path $projectRoot) {
    $unityRoots += $projectRoot
}

$unityRoots += Get-ChildItem -LiteralPath $projectRoot -Directory -ErrorAction SilentlyContinue |
    Where-Object { Test-UnityProjectRoot -Path $_.FullName } |
    ForEach-Object { $_.FullName }

$unityRoots = $unityRoots | Sort-Object -Unique

if (-not $unityRoots) {
    Stop-WithMessage "Unity project root was not found. Create a Unity project at the repository root or in a direct child folder before enabling Unity MCP."
}

$serverRecords = foreach ($unityRoot in $unityRoots) {
    $packageCacheRoot = Join-Path $unityRoot "Library\PackageCache"
    if (Test-Path -LiteralPath $packageCacheRoot) {
        Get-ChildItem -LiteralPath $packageCacheRoot -Directory -Filter "com.gamelovers.mcp-unity@*" -ErrorAction SilentlyContinue |
            ForEach-Object {
                [pscustomobject]@{
                    UnityRoot = $unityRoot
                    Server = Join-Path $_.FullName "Server~\build\index.js"
                }
            }
    }

    [pscustomobject]@{
        UnityRoot = $unityRoot
        Server = Join-Path $unityRoot "Packages\com.gamelovers.mcp-unity\Server~\build\index.js"
    }
}

$serverRecord = $serverRecords |
    Where-Object { Test-Path -LiteralPath $_.Server } |
    Sort-Object UnityRoot, Server |
    Select-Object -First 1

if (-not $serverRecord) {
    Stop-WithMessage "Unity MCP server was not found. Install com.gamelovers.mcp-unity in Unity, then run Tools > MCP Unity > Server Window > Force Install Server."
}

$nodePath = $null
$node = Get-Command node -ErrorAction SilentlyContinue | Select-Object -First 1
if ($node) {
    $nodePath = $node.Source
}

if (-not $nodePath -and $env:LOCALAPPDATA) {
    $codexNode = Get-ChildItem -LiteralPath (Join-Path $env:LOCALAPPDATA "OpenAI\Codex\bin") -Recurse -Filter "node.exe" -ErrorAction SilentlyContinue |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1
    if ($codexNode) {
        $nodePath = $codexNode.FullName
    }
}

if (-not $nodePath) {
    Stop-WithMessage "Node.js was not found. Install Node.js 18+ or make node available before enabling Unity MCP."
}

Set-Location -LiteralPath $serverRecord.UnityRoot
& $nodePath $serverRecord.Server
exit $LASTEXITCODE
