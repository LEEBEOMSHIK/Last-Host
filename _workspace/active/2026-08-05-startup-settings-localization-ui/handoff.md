# 핸드오프 기록

## 작업 ID

`2026-08-05-startup-settings-localization-ui`

## 최신 사용자 요청

시작 화면과 설정 UI를 진행하되 계속 보정할 예정이므로 다국어 확장 가능성을 처음부터 고려한다.

## 현재 상태

- 상태: 내부 승인 가능 — Play 진입·언어별 폰트 UnityEditMode `38/38` PASS, 사용자 실제 화면 수용 대기
- 여기서 멈춤: 기능 blocker 없음. 실제 Play/MCP 화면·입력·씬 전이와 사용자 첫인상 수용은 아직 대기
- 다음 세션의 첫 목표: 재실행 없이 사용자가 Startup 화면·한영 전환·설정 가독성·2D 진입을 확인

## 먼저 읽을 파일

1. `task.md`
2. `artifacts/architecture-review.md`
3. `director-review.md`

## 건드리면 안 되는 기존 변경

- 사용자 소유 `docs/references/images/image.png`
- 기존 3D/2D 게임플레이 씬·코드·ProjectSettings의 관련 없는 값
- `_workspace/previews/`, `Builds/`

## 변경한 파일

- `UnityProject/Assets/_Project/Scripts/UI/Startup/StartupLocalization.cs`
- `UnityProject/Assets/_Project/Scripts/UI/Startup/StartupSettings.cs`
- `UnityProject/Assets/_Project/Scripts/UI/Startup/StartupController.cs`
- `UnityProject/Assets/_Project/Scripts/UI/Startup/StartupMenuView.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/Startup/StartupSettingsTests.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/Startup/StartupSceneContractTests.cs`
- `UnityProject/Assets/_Project/Scenes/Startup.unity`
- `UnityProject/ProjectSettings/EditorBuildSettings.asset`
- 위 신규 폴더·소스·씬의 Unity `.meta`

## 씬/통합 공개 API

- 생성: `StartupController.CreateDefault()` 또는 dependency 주입 생성자
- 렌더 상태: `Panel`, `SavedSettings`, `Draft`, `DefaultSettings`, `AvailableResolutions`, `Localizer`, `LastApplyResult`
- 이벤트: `StateChanged`, `Localizer.LanguageChanged`
- 설정 명령: `OpenSettings`, `SetDraftLanguage`, `SetDraftDisplayMode`, `SetDraftResolution`, `SetDraftVSyncCount`, `UseDefaults`, `ApplySettings`, `CancelSettings`, `HandleEscape`
- 앱 명령: `StartPrototype`, `RequestQuit`
- 문자열: `Localizer.GetText(StartupTextKey)`만 사용하며 Inspector에 한·영 원문을 저장하지 않는다.
- 씬 경로: `StartupSceneContract.StartupScenePath`, `StartupSceneContract.PrototypeScenePath`
- 구현 소유권: 코드·테스트 owner release 완료. 씬/빌더/Build Settings owner가 공개 API wiring을 이어받을 수 있다.

## 마지막 성공 검증

- Unity 제공 Roslyn 정적 컴파일: `UNITY_EDITOR` 런타임 분기 PASS.
- Unity 제공 Roslyn 정적 컴파일: standalone 런타임 분기 PASS.
- 키 완전성·해상도 fallback·Draft 무부작용·Apply 순서·저장 손상 복구·정확한 2D 씬 경로·Quit/Esc의 표적 테스트 소스 정적 대조 완료.
- Unity 6000.4.6f1 Roslyn reference 정적 컴파일: 통합된 Startup 런타임 소스 PASS.
- Unity 6000.4.6f1 Roslyn reference 정적 컴파일: `StartupSceneContractTests.cs` PASS.
- `git diff --check`, UI 직접 번역 문자열 부재, 내장 폰트·960x540·Input System EventSystem·Build Settings 순서·신규 meta GUID 중복 정적 대조 PASS.
- canonical run `startup-settings-qa-20260805-001`, fingerprint `d10c8cae0d0908828c038c5f2e689e32c765bf09659360cbe5a3915f33b8eb57`.
- 공용 wrapper 표적 UnityEditMode `32/32` PASS, failed/skipped/inconclusive `0`, Unity exit `0`.
- 결과: `artifacts/qa-target-results.xml`; 독립 QA 판정: `independent-qa-pass-awaiting-director`.

## 실패했거나 차단된 검증

- 초기 `pwsh` alias 실행 차단은 설치된 PowerShell 7 직접 경로의 승인된 실행으로 해소했다. Windows PowerShell·low-level runner 우회는 0회다.
- 첫 QA wrapper 호출은 current-state evidence의 stale run_id에서 차단됐으며 Unity 시작은 0회였다. 동일 후보의 메타데이터만 동기화한 correction `1/2` 뒤 실제 Unity 1회가 통과했다.
- isolated cache 초기 ShaderGraph/PackageCache 오류는 후속 package refresh·LastHost 재컴파일·32/32 PASS 순서로 해소됐다고 독립 QA가 판정했다. cold-cache 지연 위험은 기록으로 남긴다.
- MCP Play·build·full suite는 실행하지 않았다.

## 현재 검증 후보

- candidate fingerprint: `d10c8cae0d0908828c038c5f2e689e32c765bf09659360cbe5a3915f33b8eb57`
- canonical run_id/current-state run_id: `startup-settings-qa-20260805-001`
- 이전 코드-only run_id: `startup-settings-impl-20260805-001` — 씬 통합으로 superseded
- verification revision: S0 r2 + current candidate
- candidate frozen 여부: 씬/통합 owner 기준 예, 독립 QA 인계 가능
- superseded run: 코드-only candidate `2cc42f372ccfbba0643fbc43b3207cd097523f7640975c8a85e80902134cc552`
- verification current-state JSON: `artifacts/verification-current-state.json` — `technical-pass`
- fingerprint manifest: `artifacts/candidate-fingerprint.json` — 19개 production/test/scene/package/version 입력
- attempt ledger: 이전 code-only PowerShell 경로 blocker, current candidate stale evidence preflight correction, canonical success를 모두 보존

## Unity single-owner lease 인계

- lease 상태: MCP lease 미획득; 공용 wrapper의 격리 Unity 실행만 1회
- Play / Pause / scene / dirty: 미조작
- 임시 객체 유무: 없음

## 루프 게이트 상태

- 위험 등급 / correction cycle: R3 / S0 r2 0/2, preflight correction 1/2
- S0 charter: r2 PASS
- 마지막 통과 단계: 동일 후보 독립 QA·표적 UnityEditMode `32/32` PASS
- first blocker: 기능 blocker 없음; 총괄 1차 최종 감사의 상태 문서 불일치는 동기화·read-only 재대조로 해소
- 커밋 전 차단 조건: 사용자 화면 수용 대기

## 이어서 해야 할 일

1. 사용자가 Unity에서 Startup 첫 화면, 한영 preview/취소, 설정 가독성, 2D 시작 흐름을 확인한다.
2. 확인 결과에 따라 UI 배치·문구를 후속 보정한다.
3. 추가 언어·외부 폰트·아트·오디오는 별도 승인 뒤 진행한다.

## production 소유권 release

- 게임플레이 코드·테스트 owner: release 완료.
- Unity 씬/통합 owner: `StartupMenuView.cs`, `Startup.unity`, `EditorBuildSettings.asset`, `StartupSceneContractTests.cs`와 관련 meta release 완료.
- 다음 owner: 사용자 수용. production owner는 없음.

## 사용자 승인 필요

- 현재 V1은 승인 완료. 외부 폰트·신규 패키지·오디오·추가 언어는 별도 승인.

## 2026-08-07 선택 배경 통합 인계

- 상태: 선택 배경 통합 기술 검증 통과 — 총괄 상태-only 재대조와 사용자 화면 수용 대기.
- canonical run/fingerprint: `startup-background-qa-20260807-002` / `be3e9ce5a76ff6951272a6a191a89018a7f28eeef182b30df878495c750d3649`.
- 선택 PNG는 `startup-bacteriophage-food-chain-background-v1.png`로 rename했고 Unity Sprite/UI 사본과 Startup 씬 직렬화 참조를 연결했다.
- UnityEditMode `33/33 PASS`, failed/skipped/inconclusive `0`, Unity exit `0`.
- 기존 `startup-settings-qa-20260805-001` / `32/32`는 배경 production·test 변경으로 현재 revision에서 `SUPERSEDED`다.
- 배경 revision correction `1/2`: run001은 wrapper 다중 경로 binding preflight 차단(Unity 0), run002는 같은 두 파일을 포함한 단일 테스트 디렉터리로 safety lint 후 PASS.
- 작업 누적 Unity/MCP/build `2/0/0`; 이번 revision `1/0/0`. full/matrix/capture `0`.
- McpPlay unavailable로 실제 960×540 화면·입력은 사용자 수용 대기다.

## 2026-08-07 실제 Play·언어별 폰트 correction 인계

- canonical run: `startup-play-font-qa-20260807-001`; fingerprint는 34-input `artifacts/candidate-fingerprint.json`의 `22eef3ed...531a`이며 재계산 `Match=true`.
- Editor Play는 저장된 `Startup.unity`를 `playModeStartScene`으로 사용한다.
- 한국어는 bundled Galmuri11, 영어는 bundled Silkscreen으로 같은 Render cycle에 전환하며 family별 TTF/OFL/SOURCE를 보존한다.
- UnityEditMode `38/38 PASS`, failed/skipped/inconclusive `0`, Unity exit `0`.
- 이전 배경 revision `33/33`은 현재 revision에서 `SUPERSEDED`.
- 작업 누적 Unity/MCP/build `3/0/0`; 현재 revision `1/0/0`; full/matrix/capture `0`.
- S0 계약 correction `1/2`; 동적 QA/preflight correction `0/2`.
- McpPlay unavailable. 실제 Startup 첫 프레임, 960×540 배경·한영 폰트·bounds/가독성, 설정 preview/cancel, raycast·2D 전이·Console은 사용자 수용 대기.
