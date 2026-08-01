# Unity 검증 범용 도구

이 폴더의 스크립트는 검증 결과를 늘리는 도구가 아니라, **한 Unity 세션·한 변경 후보·한 테스트 결과**를 서로 혼동하지 않게 만드는 최소 하네스다. 모든 예시는 저장소 루트에서 PowerShell 7로 실행한다.

## 1. Unity MCP lease

동일 프로젝트의 Unity Editor, MCP, TestRunner, 배치 Unity를 조작하기 전에 한 작업만 lease를 획득한다.

```powershell
pwsh tools/verification/UnityMcpLease.ps1 Acquire `
  -ProjectPath UnityProject `
  -Agent qa-verification-agent `
  -WorkId 2026-08-02-example `
  -RunId example-run-001 `
  -EditorProcessId 12345 `
  -Scene 'Assets/_Project/Scenes/RatHost2DTechnicalSample.unity' `
  -BaselinePlay $false `
  -BaselinePause $false `
  -BaselineScene 'Assets/_Project/Scenes/RatHost2DTechnicalSample.unity' `
  -BaselineDirty $false `
  -TtlSeconds 300

pwsh tools/verification/UnityMcpLease.ps1 Status -ProjectPath UnityProject

pwsh tools/verification/UnityMcpLease.ps1 Renew `
  -ProjectPath UnityProject `
  -Agent qa-verification-agent `
  -WorkId 2026-08-02-example `
  -RunId example-run-001

pwsh tools/verification/UnityMcpLease.ps1 Release `
  -ProjectPath UnityProject `
  -Agent qa-verification-agent `
  -WorkId 2026-08-02-example `
  -RunId example-run-001
```

- 획득은 `FileMode.CreateNew`라서 원자적이다.
- lease JSON은 `work_id`, `agent`, `run_id`, `editor_pid`, `scene`, `acquired_utc`, `expires_utc`, `baseline_play`, `baseline_pause`, `baseline_scene`, `baseline_dirty`를 반드시 기록한다.
- `agent/work_id/run_id`가 case-sensitive로 모두 일치할 때만 갱신·반납할 수 있다.
- Acquire의 `EditorProcessId`는 현재 Unity Editor 또는 batch Unity 소유 프로세스의 실제 PID여야 한다. `Scene`/`BaselineScene`에 대상이 없으면 빈 값 대신 `(none)`처럼 명시적인 값을 쓴다.
- 이전 호출 호환성을 위해 `Owner`는 `Agent`, `ProcessId`는 `EditorProcessId`의 alias로만 허용한다. JSON 필드명은 `agent`, `editor_pid`로 고정한다.
- TTL이 지났어도 자동 강탈하지 않는다. `Status`의 `expired`, `process_alive`를 확인하고 기존 소유자 인계 또는 명시적 운영 판단을 거친다.
- 기본 파일은 `<ProjectPath>/Temp/last-host-unity-mcp-lease.json`이며 추적 대상이 아니다.
- lease 획득 뒤 60초 간격 heartbeat를 권장하고, Play 종료·임시 객체 제거·scene dirty 확인 뒤 반납한다.

## 2. EditMode 테스트 실행과 XML 판정

```powershell
pwsh tools/verification/Invoke-UnityEditModeTests.ps1 `
  -ProjectPath UnityProject `
  -UnityPath 'C:/Program Files/Unity/Hub/Editor/6000.4.6f1/Editor/Unity.exe' `
  -ResultsPath (Join-Path $env:TEMP 'last-host-editmode/results.xml') `
  -LogPath (Join-Path $env:TEMP 'last-host-editmode/unity.log') `
  -TimeoutSeconds 1800
```

기존 XML만 검증할 수도 있다.

```powershell
pwsh tools/verification/Invoke-UnityEditModeTests.ps1 `
  -ValidateResultsOnly `
  -ResultsPath '_workspace/active/<작업ID>/artifacts/results.xml'
```

- 실행 전 지정 결과 XML을 지워 stale PASS 재사용을 막는다.
- Unity Test Framework가 결과를 쓸 때까지 기다리며 `-quit`를 전달하지 않는다.
- 기본 1800초 안에 종료하지 않으면 이 호출이 시작한 Unity PID 하나만 종료하고 nonzero로 실패한다. 기존 에디터나 전체 process tree는 종료하지 않는다.
- XML 존재·크기·NUnit3 형식과 `total/passed/failed/skipped/inconclusive`를 검사한다.
- `total > 0`, `passed == total`, 나머지 결과 0, 루트 결과 `Passed`일 때만 종료 코드 0이다.
- skipped/inconclusive도 미검증이므로 최종 PASS에서는 실패로 취급한다.
- 같은 프로젝트를 연 Unity/MCP와 배치 Unity를 동시에 실행하지 않는다. 먼저 lease를 획득한다.

## 3. verification candidate fingerprint

```powershell
pwsh tools/verification/Get-VerificationFingerprint.ps1 `
  -ProjectRoot . `
  -ProductionPath 'UnityProject/Assets/_Project/Scripts/TechnicalSample2D' `
  -TestPath 'UnityProject/Assets/_Project/Tests/EditMode/TechnicalSample2D' `
  -ScenePath 'UnityProject/Assets/_Project/Scenes/RatHost2DTechnicalSample.unity' `
  -PackagePath 'UnityProject/Packages/packages-lock.json' `
  -VersionPath 'UnityProject/ProjectSettings/ProjectVersion.txt' `
  -RunId example-run-001 `
  -ManifestPath (Join-Path $env:TEMP 'last-host-verification/example-run-001.json')
```

입력 파일을 `category/상대경로` 순으로 정렬하고, 각 파일 SHA-256·길이를 다시 SHA-256으로 묶는다. manifest에는 `run_id`, 후보 fingerprint, 생성 시각, 입력과 파일별 hash가 기록된다. Production·test·scene·package/version 중 검증 증거가 의존하는 경로를 빠짐없이 지정해야 한다. 누락된 의존성은 스크립트가 추론하지 않는다.

## S0~S7 연계

| 단계 | 도구 사용 |
| --- | --- |
| S0 계약 고정 | 도구 없음. 원증상·경계·negative control을 먼저 고정한다. |
| S1 정적·컴파일 | 후보 경로를 정한 뒤 fingerprint manifest를 만든다. |
| S2~S3 단위·EditMode | lease 획득 후 관련 EditMode를 실행하고 XML을 판정한다. |
| S4~S5 scene smoke·축소 매트릭스 | 같은 lease와 `run_id`를 유지하고 후보 fingerprint가 바뀌지 않았는지 재확인한다. |
| S6 전체 회귀 | 후보 freeze 뒤 전체 EditMode를 한 번 실행한다. |
| S7 최종 증거 | 현재 manifest와 XML·Play 증거의 `run_id/fingerprint`를 대조하고 lease를 반납한다. |

Production, scene, test 또는 harness가 바뀌면 이전 PASS는 즉시 `SUPERSEDED`다. 새 fingerprint/run_id로 영향받는 빠른 단계부터 다시 실행한다.

## 안전 범위와 한계

- 이 도구들은 Unity 코드·씬·ProjectSettings를 수정하지 않는다. 테스트 실행 자체가 만드는 `Library`, `Logs`, `Temp` 캐시는 Unity의 통상 동작이다.
- fingerprint는 지정 경로만 보호한다. 누락된 dependency, 비결정적 런타임 상태, 사용자 입력을 증명하지 않는다.
- lease는 협력적 잠금이다. lease를 무시한 외부 Unity/MCP 호출을 기술적으로 차단하지 못한다.
- **원자적인 GameView 캡처는 이 PowerShell 도구에 포함하지 않는다.** 정확한 상태 설정, frame barrier, camera render, stale-object guard, PNG와 sidecar의 동일 메모리 상태 기록은 작업별로 저장소에 추적되는 Editor harness가 필요하다. 일회성 `RunCommand`나 별도 시점의 CSV+PNG를 최종 원자 증거로 간주하지 않는다.
