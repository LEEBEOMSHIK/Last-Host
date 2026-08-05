#Requires -Version 7.0

[CmdletBinding()]
param([switch]$Execute)

$arguments = @{
    WorkId = '2026-08-05-rat-collision-surface-slide'
    CriterionId = 'C1-C5-C7-implementer-target'
    LedgerPath = '_workspace/active/2026-08-05-rat-collision-surface-slide/artifacts/verification-attempt-ledger.json'
    Route = 'UnityEditMode'
    RunId = 'surface-slide-impl-007'
    CandidateFingerprint = '2286f04110addaa6d5fa9d67e0b269a8c6d800094e40a118339c1ae327e67414'
    AgentBriefPath = '_workspace/active/2026-08-05-rat-collision-surface-slide/artifacts/agent-brief.json'
    CurrentStatePath = '_workspace/active/2026-08-05-rat-collision-surface-slide/artifacts/verification-current-state.json'
    QaHarnessPath = @(
        'UnityProject/Assets/_Project/Tests/EditMode/TechnicalSample2D/PhysicsCameraAndSort2DTests.cs'
    )
    ContractBaselinePath = @(
        'UnityProject/Assets/_Project/Scripts/TechnicalSample2D/RatHost2DController.cs'
    )
    ProductionPath = @(
        'UnityProject/Assets/_Project/Scripts/TechnicalSample2D/RatHost2DController.cs'
    )
    TestPath = @(
        'UnityProject/Assets/_Project/Tests/EditMode'
    )
    SourceProjectPath = 'UnityProject'
    CacheRoot = (Join-Path $env:TEMP 'last-host-unity-cache')
}

if ($Execute) {
    $arguments.UnityPath = 'C:/Program Files/Unity/Hub/Editor/6000.4.6f1/Editor/Unity.exe'
    $arguments.ResultsPath = '_workspace/active/2026-08-05-rat-collision-surface-slide/artifacts/implementer-target-results-r7.xml'
    $arguments.LogPath = '_workspace/active/2026-08-05-rat-collision-surface-slide/artifacts/implementer-target-unity-r7.log'
    $arguments.TestFilter = 'LastHost.Prototype.TechnicalSample2D.Tests.PhysicsCameraAndSort2DTests;LastHost.Prototype.RatHost2D.Tests.RatHost2DStage2RuntimeTests'
    $arguments.TimeoutSeconds = 1800
}
else {
    $arguments.PreflightOnly = $true
}

& 'tools/verification/Invoke-HighCostVerification.ps1' @arguments
exit $LASTEXITCODE
