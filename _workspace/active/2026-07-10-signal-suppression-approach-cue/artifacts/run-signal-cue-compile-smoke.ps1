$ErrorActionPreference = 'Stop'

$repoRoot = 'C:\project\Last-Host'
$projectRoot = Join-Path $repoRoot 'UnityProject'
$workDir = Join-Path $repoRoot '_workspace\active\2026-07-10-signal-suppression-approach-cue\artifacts'
$csc = 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Data\DotNetSdkRoslyn\csc.dll'
$runtimeDll = Join-Path $workDir 'LastHost.Prototype.CompileCheck.dll'
$testsDll = Join-Path $workDir 'LastHost.Prototype.Tests.CompileCheck.dll'

function Resolve-UnityPath($path) {
    if ([System.IO.Path]::IsPathRooted($path)) {
        return $path
    }

    return Join-Path $projectRoot $path
}

function Get-CsprojData($relativePath) {
    [xml]$csproj = Get-Content -LiteralPath (Join-Path $projectRoot $relativePath)
    $defines = $csproj.Project.PropertyGroup.DefineConstants |
        Where-Object { -not [string]::IsNullOrWhiteSpace($_) } |
        Select-Object -First 1
    if ($null -ne $defines) {
        $defines = ([string]$defines).Trim()
    }

    $references = @()
    foreach ($reference in $csproj.Project.ItemGroup.Reference) {
        if ($null -eq $reference.HintPath -or [string]::IsNullOrWhiteSpace($reference.HintPath)) {
            continue
        }

        $resolved = Resolve-UnityPath ([string]$reference.HintPath)
        if (Test-Path -LiteralPath $resolved) {
            $references += $resolved
        }
    }

    $sources = @()
    foreach ($compile in $csproj.Project.ItemGroup.Compile) {
        if ($null -eq $compile.Include -or [string]::IsNullOrWhiteSpace($compile.Include)) {
            continue
        }

        $sources += (Resolve-UnityPath ([string]$compile.Include))
    }

    [pscustomobject]@{
        Defines = $defines
        References = $references
        Sources = $sources
    }
}

function Invoke-Csc($outputPath, $csprojData, $extraReferences) {
    $rspArgs = @(
        '-nologo',
        '-noconfig',
        '-nostdlib+',
        '-target:library',
        '-langversion:9.0',
        "-out:$outputPath"
    )

    if (-not [string]::IsNullOrWhiteSpace($csprojData.Defines)) {
        $rspArgs += "-define:$($csprojData.Defines)"
    }

    foreach ($reference in ($csprojData.References + $extraReferences | Sort-Object -Unique)) {
        $rspArgs += "-reference:`"$reference`""
    }

    foreach ($source in $csprojData.Sources) {
        $rspArgs += "`"$source`""
    }

    $logPath = [System.IO.Path]::ChangeExtension($outputPath, '.log')
    $rspPath = [System.IO.Path]::ChangeExtension($outputPath, '.rsp')
    Set-Content -LiteralPath $rspPath -Value $rspArgs -Encoding UTF8
    & dotnet $csc "@$rspPath" 2>&1 | Tee-Object -FilePath $logPath
    if ($LASTEXITCODE -ne 0) {
        throw "csc failed for $outputPath"
    }
}

New-Item -ItemType Directory -Force -Path $workDir | Out-Null

$runtimeData = Get-CsprojData 'LastHost.Prototype.csproj'
Invoke-Csc $runtimeDll $runtimeData @()

$testsData = Get-CsprojData 'LastHost.Prototype.Tests.csproj'
Invoke-Csc $testsDll $testsData @($runtimeDll)

foreach ($hint in $runtimeData.References + $testsData.References + @($runtimeDll, $testsDll)) {
    if (Test-Path -LiteralPath $hint) {
        try {
            [System.Reflection.Assembly]::LoadFrom($hint) | Out-Null
        }
        catch {
        }
    }
}

$assembly = [System.Reflection.Assembly]::LoadFrom($runtimeDll)

function Get-PrototypeType($name) {
    $assembly.GetType($name, $true)
}

function New-PrototypeEnum($typeName, $name) {
    [System.Enum]::Parse((Get-PrototypeType $typeName), $name)
}

function New-PrototypeObject($typeName, [object[]]$arguments) {
    if ($arguments.Count -eq 1 -and $arguments[0] -is [object[]]) {
        $arguments = $arguments[0]
    }

    $type = Get-PrototypeType $typeName
    if ($arguments.Count -eq 0) {
        return [System.Activator]::CreateInstance($type)
    }

    $argTypes = [Type[]]($arguments | ForEach-Object { $_.GetType() })
    $constructor = $type.GetConstructor($argTypes)
    if ($null -eq $constructor) {
        throw "Constructor not found on $typeName with args: $($argTypes -join ', ')"
    }

    $constructor.Invoke($arguments)
}

function Get-PrototypeProperty($object, $name) {
    $object.GetType().GetProperty($name).GetValue($object)
}

function Set-PrototypeProperty($object, $name, $value) {
    $object.GetType().GetProperty($name).SetValue($object, $value)
}

function Invoke-PrototypeMethod($object, $name, [object[]]$arguments) {
    if ($arguments.Count -eq 1 -and $arguments[0] -is [object[]]) {
        $arguments = $arguments[0]
    }

    if ($arguments.Count -eq 0) {
        $argTypes = [Type[]]@()
    }
    else {
        $argTypes = [Type[]]($arguments | ForEach-Object { $_.GetType() })
    }

    $method = $object.GetType().GetMethod($name, $argTypes)
    if ($null -eq $method) {
        throw "Method not found on $($object.GetType().FullName).$name with args: $($argTypes -join ', ')"
    }

    $method.Invoke($object, $arguments)
}

function Assert-PrototypeEqual($expected, $actual, $label) {
    if (-not [object]::Equals($expected, $actual)) {
        throw "$label expected <$expected> but was <$actual>"
    }
}

function Assert-PrototypeGreater($actual, $minimum, $label) {
    if ($actual -le $minimum) {
        throw "$label expected > $minimum but was $actual"
    }
}

function Assert-PrototypeLess($actual, $maximum, $label) {
    if ($actual -ge $maximum) {
        throw "$label expected < $maximum but was $actual"
    }
}

$configType = 'LastHost.Prototype.Core.PrototypeConfig'
$sessionType = 'LastHost.Prototype.Core.PrototypeSessionState'
$internalType = 'LastHost.Prototype.Core.InternalVirusMinigameType'

$config = New-PrototypeObject $configType ([object[]]@())
Set-PrototypeProperty $config 'SignalSuppressionSignalIntervalSeconds' ([float]1.0)
Set-PrototypeProperty $config 'SignalSuppressionAccurateWindowSeconds' ([float]0.1)
Set-PrototypeProperty $config 'SignalSuppressionCueLeadSeconds' ([float]0.35)

$session = New-PrototypeObject $sessionType ([object[]]@($config))
Invoke-PrototypeMethod $session 'EnterVirusMinigame' ([object[]]@((New-PrototypeEnum $internalType 'ImmuneSignalSuppression'))) | Out-Null

Assert-PrototypeEqual $false (Get-PrototypeProperty $session 'IsSignalSuppressionCueActive') 'cue inactive at start'
Assert-PrototypeEqual ([float]0.0) (Get-PrototypeProperty $session 'SignalSuppressionCueIntensity') 'cue intensity at start'
Assert-PrototypeEqual '다음 신호 1.0초' (Get-PrototypeProperty $session 'SignalSuppressionTimingText') 'timing at start'

Invoke-PrototypeMethod $session 'TickSignalSuppression' ([object[]]@([float]0.7)) | Out-Null
Assert-PrototypeEqual $true (Get-PrototypeProperty $session 'IsSignalSuppressionCueActive') 'cue active before accurate window'
Assert-PrototypeGreater (Get-PrototypeProperty $session 'SignalSuppressionCueIntensity') 0.0 'cue intensity ramps up'
Assert-PrototypeLess (Get-PrototypeProperty $session 'SignalSuppressionCueIntensity') 1.0 'cue intensity remains below ready'
Assert-PrototypeEqual '신호 접근 0.3초' (Get-PrototypeProperty $session 'SignalSuppressionTimingText') 'timing during cue'

Invoke-PrototypeMethod $session 'TickSignalSuppression' ([object[]]@([float]0.21)) | Out-Null
Assert-PrototypeEqual ([float]1.0) (Get-PrototypeProperty $session 'SignalSuppressionCueIntensity') 'cue intensity at ready'
Assert-PrototypeEqual '지금 차단' (Get-PrototypeProperty $session 'SignalSuppressionTimingText') 'timing at ready'

'SIGNAL_SUPPRESSION_CUE_COMPILE_SMOKE_PASS'
