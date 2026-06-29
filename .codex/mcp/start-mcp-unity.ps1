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
$serverCandidates = @()

$packageCacheRoot = Join-Path $projectRoot "Library\PackageCache"
if (Test-Path -LiteralPath $packageCacheRoot) {
    $serverCandidates += Get-ChildItem -LiteralPath $packageCacheRoot -Directory -Filter "com.gamelovers.mcp-unity@*" -ErrorAction SilentlyContinue |
        ForEach-Object { Join-Path $_.FullName "Server~\build\index.js" }
}

$embeddedPackageServer = Join-Path $projectRoot "Packages\com.gamelovers.mcp-unity\Server~\build\index.js"
$serverCandidates += $embeddedPackageServer

$server = $serverCandidates |
    Where-Object { Test-Path -LiteralPath $_ } |
    Sort-Object |
    Select-Object -First 1

if (-not $server) {
    Stop-WithMessage "Unity MCP server was not found. Install com.gamelovers.mcp-unity in Unity, then run Tools > MCP Unity > Server Window > Force Install Server."
}

$node = Get-Command node -ErrorAction SilentlyContinue | Select-Object -First 1
if (-not $node) {
    Stop-WithMessage "Node.js was not found in PATH. Install Node.js or expose node before enabling Unity MCP."
}

& $node.Source $server
exit $LASTEXITCODE
