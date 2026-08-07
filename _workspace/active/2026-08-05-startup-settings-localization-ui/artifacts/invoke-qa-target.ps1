#Requires -Version 7.0

[CmdletBinding()]
param()

$arguments = @{
    WorkId = '2026-08-05-startup-settings-localization-ui'
    CriterionId = 'C1-C9-qa-target'
    LedgerPath = '_workspace/active/2026-08-05-startup-settings-localization-ui/artifacts/verification-attempt-ledger.json'
    Route = 'UnityEditMode'
    RunId = 'startup-settings-qa-20260805-001'
    CandidateFingerprint = 'd10c8cae0d0908828c038c5f2e689e32c765bf09659360cbe5a3915f33b8eb57'
    AgentBriefPath = '_workspace/active/2026-08-05-startup-settings-localization-ui/artifacts/agent-brief-qa.json'
    CurrentStatePath = '_workspace/active/2026-08-05-startup-settings-localization-ui/artifacts/verification-current-state.json'
    QaHarnessPath = @(
        'UnityProject/Assets/_Project/Tests/EditMode/Startup/StartupSettingsTests.cs'
    )
    ContractBaselinePath = @(
        'UnityProject/Assets/_Project/Scripts/UI/Startup'
    )
    ProductionPath = @(
        'UnityProject/Assets/_Project/Scripts/UI/Startup'
    )
    TestPath = @(
        'UnityProject/Assets/_Project/Tests/EditMode/Startup'
    )
    SourceProjectPath = 'UnityProject'
    CacheRoot = (Join-Path $env:TEMP 'last-host-unity-cache')
    UnityPath = 'C:/Program Files/Unity/Hub/Editor/6000.4.6f1/Editor/Unity.exe'
    ResultsPath = '_workspace/active/2026-08-05-startup-settings-localization-ui/artifacts/qa-target-results.xml'
    LogPath = '_workspace/active/2026-08-05-startup-settings-localization-ui/artifacts/qa-target-unity.log'
    TestFilter = 'LastHost.Prototype.Tests.EditMode.Startup.StartupSettingsTests;LastHost.Prototype.Tests.Startup.StartupSceneContractTests'
    TimeoutSeconds = 1800
}

& 'tools/verification/Invoke-HighCostVerification.ps1' @arguments
exit $LASTEXITCODE
