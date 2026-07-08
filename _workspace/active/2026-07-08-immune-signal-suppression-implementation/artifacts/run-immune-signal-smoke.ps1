$ErrorActionPreference = 'Stop'

$projectRoot = 'C:\project\Last-Host\UnityProject'
$workDir = 'C:\project\Last-Host\_workspace\active\2026-07-08-immune-signal-suppression-implementation\artifacts'
$runtimeDll = Join-Path $workDir 'LastHost.Prototype.CompileCheck.dll'
$csprojPath = Join-Path $projectRoot 'LastHost.Prototype.csproj'

[xml]$csproj = Get-Content -LiteralPath $csprojPath
foreach ($hint in $csproj.Project.ItemGroup.Reference.HintPath) {
    if ([string]::IsNullOrWhiteSpace($hint)) {
        continue
    }

    $path = [string]$hint
    if (-not [System.IO.Path]::IsPathRooted($path)) {
        $path = Join-Path $projectRoot $path
    }

    if (Test-Path -LiteralPath $path) {
        try {
            [System.Reflection.Assembly]::LoadFrom($path) | Out-Null
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

$modelType = 'LastHost.Prototype.VirusMinigame.ImmuneSignalSuppressionModel'
$judgementType = 'LastHost.Prototype.VirusMinigame.ImmuneSignalSuppressionJudgement'
$outcomeType = 'LastHost.Prototype.VirusMinigame.VirusMinigameOutcome'
$modeType = 'LastHost.Prototype.Core.PrototypeGameMode'
$internalType = 'LastHost.Prototype.Core.InternalVirusMinigameType'
$configType = 'LastHost.Prototype.Core.PrototypeConfig'
$sessionType = 'LastHost.Prototype.Core.PrototypeSessionState'
$mutationType = 'LastHost.Prototype.Mutations.MutationType'

$minigame = New-PrototypeObject $modelType ([object[]]@(4, 3, [float]100.0, [float]25.0, [float]0.1, [float]1.0))
for ($i = 0; $i -lt 3; $i++) {
    Invoke-PrototypeMethod $minigame 'Tick' ([object[]]@([float]1.0)) | Out-Null
    Assert-PrototypeEqual (New-PrototypeEnum $judgementType 'Accurate') (Invoke-PrototypeMethod $minigame 'ResolveInput' @()) 'accurate judgement'
}

Assert-PrototypeEqual (New-PrototypeEnum $outcomeType 'Success') (Get-PrototypeProperty $minigame 'Outcome') 'accurate outcome'
Assert-PrototypeEqual 3 (Get-PrototypeProperty $minigame 'SuppressedSignals') 'accurate suppressed count'
Assert-PrototypeEqual 0 (Get-PrototypeProperty $minigame 'MissedSignals') 'accurate missed count'

$minigame = New-PrototypeObject $modelType ([object[]]@(4, 3, [float]40.0, [float]20.0, [float]0.1, [float]1.0))
Invoke-PrototypeMethod $minigame 'Tick' ([object[]]@([float]0.5)) | Out-Null
Assert-PrototypeEqual (New-PrototypeEnum $judgementType 'Early') (Invoke-PrototypeMethod $minigame 'ResolveInput' @()) 'early judgement'
Assert-PrototypeEqual ([float]20.0) (Get-PrototypeProperty $minigame 'Stability') 'stability after early'
Invoke-PrototypeMethod $minigame 'Tick' ([object[]]@([float]1.2)) | Out-Null
Assert-PrototypeEqual (New-PrototypeEnum $judgementType 'Late') (Invoke-PrototypeMethod $minigame 'ResolveInput' @()) 'late judgement'
Assert-PrototypeEqual (New-PrototypeEnum $outcomeType 'Failed') (Get-PrototypeProperty $minigame 'Outcome') 'mistimed outcome'
Assert-PrototypeEqual ([float]0.0) (Get-PrototypeProperty $minigame 'Stability') 'stability after late'
Assert-PrototypeEqual 2 (Get-PrototypeProperty $minigame 'MissedSignals') 'missed count after mistiming'

$minigame = New-PrototypeObject $modelType ([object[]]@(3, 3, [float]100.0, [float]10.0, [float]0.1, [float]1.0))
Assert-PrototypeEqual (New-PrototypeEnum $judgementType 'Missed') (Invoke-PrototypeMethod $minigame 'ResolveMissedSignal' @()) 'manual miss judgement'
Assert-PrototypeEqual (New-PrototypeEnum $outcomeType 'Failed') (Get-PrototypeProperty $minigame 'Outcome') 'manual miss outcome'

$session = New-PrototypeObject $sessionType ([object[]]@())
Invoke-PrototypeMethod $session 'EnterVirusMinigame' @() | Out-Null
Assert-PrototypeEqual (New-PrototypeEnum $internalType 'WhiteBloodCellEvasion') (Get-PrototypeProperty $session 'CurrentInternalMinigameType') 'default minigame type'
Assert-PrototypeEqual '변이 조각 수집 / 백혈구 회피' (Get-PrototypeProperty $session 'InternalMinigameObjectiveText') 'default objective'

$config = New-PrototypeObject $configType ([object[]]@())
Set-PrototypeProperty $config 'SignalSuppressionRequiredSuppressions' 2
Set-PrototypeProperty $config 'SignalSuppressionTotalSignals' 3
Set-PrototypeProperty $config 'SignalSuppressionSignalIntervalSeconds' ([float]1.0)
$session = New-PrototypeObject $sessionType ([object[]]@($config))
Invoke-PrototypeMethod $session 'EnterVirusMinigame' ([object[]]@((New-PrototypeEnum $internalType 'ImmuneSignalSuppression'))) | Out-Null
Assert-PrototypeEqual '면역 신호 억제' (Get-PrototypeProperty $session 'InternalMinigameObjectiveText') 'signal initial objective'
Assert-PrototypeEqual '신호 차단 0/2 / 다음 신호 1.0초' (Get-PrototypeProperty $session 'InternalMinigameProgressText') 'signal initial progress'
Invoke-PrototypeMethod $session 'TickSignalSuppression' ([object[]]@([float]1.0)) | Out-Null
Assert-PrototypeEqual '지금 차단' (Get-PrototypeProperty $session 'SignalSuppressionTimingText') 'signal timing cue'
Assert-PrototypeEqual (New-PrototypeEnum $judgementType 'Accurate') (Invoke-PrototypeMethod $session 'ResolveSignalSuppressionInput' @()) 'signal first input'
Assert-PrototypeEqual '신호 차단 1/2' (Get-PrototypeProperty $session 'InternalMinigameObjectiveText') 'signal progress objective'
Assert-PrototypeEqual '신호 차단 1/2 / 다음 신호 1.0초' (Get-PrototypeProperty $session 'InternalMinigameProgressText') 'signal progress text'
Invoke-PrototypeMethod $session 'TickSignalSuppression' ([object[]]@([float]1.0)) | Out-Null
Assert-PrototypeEqual (New-PrototypeEnum $judgementType 'Accurate') (Invoke-PrototypeMethod $session 'ResolveSignalSuppressionInput' @()) 'signal second input'
Assert-PrototypeEqual (New-PrototypeEnum $modeType 'MutationSelection') (Get-PrototypeProperty $session 'Mode') 'signal success mode'

$config = New-PrototypeObject $configType ([object[]]@())
Set-PrototypeProperty $config 'SignalSuppressionRequiredSuppressions' 3
Set-PrototypeProperty $config 'SignalSuppressionTotalSignals' 3
Set-PrototypeProperty $config 'SignalSuppressionStartingStability' ([float]20.0)
Set-PrototypeProperty $config 'SignalSuppressionMistakeDamage' ([float]20.0)
Set-PrototypeProperty $config 'SignalSuppressionSignalIntervalSeconds' ([float]1.0)
$session = New-PrototypeObject $sessionType ([object[]]@($config))
Invoke-PrototypeMethod $session 'EnterVirusMinigame' ([object[]]@((New-PrototypeEnum $internalType 'ImmuneSignalSuppression'))) | Out-Null
Invoke-PrototypeMethod $session 'TickSignalSuppression' ([object[]]@([float]0.5)) | Out-Null
Assert-PrototypeEqual (New-PrototypeEnum $judgementType 'Early') (Invoke-PrototypeMethod $session 'ResolveSignalSuppressionInput' @()) 'signal failure input'
Assert-PrototypeEqual (New-PrototypeEnum $modeType 'VirusFailed') (Get-PrototypeProperty $session 'Mode') 'signal failure mode'
Assert-PrototypeEqual '경보 증폭 1' (Get-PrototypeProperty $session 'InternalMinigameObjectiveText') 'signal failure objective'
Invoke-PrototypeMethod $session 'ReturnToRatHostAfterVirusFailure' @() | Out-Null
Assert-PrototypeEqual (New-PrototypeEnum $modeType 'RatHost') (Get-PrototypeProperty $session 'Mode') 'signal failure return mode'
$mutations = Get-PrototypeProperty $session 'Mutations'
Assert-PrototypeEqual $false (Invoke-PrototypeMethod $mutations 'Has' ([object[]]@((New-PrototypeEnum $mutationType 'Dormancy')))) 'signal failure no mutation reward'
$immuneAlert = Get-PrototypeProperty $session 'ImmuneAlert'
Assert-PrototypeEqual (Get-PrototypeProperty $config 'AlertAfterVirusFailureReturn') (Get-PrototypeProperty $immuneAlert 'Value') 'signal failure return alert'

'IMMUNE_SIGNAL_SUPPRESSION_REFLECTION_SMOKE_PASS'
