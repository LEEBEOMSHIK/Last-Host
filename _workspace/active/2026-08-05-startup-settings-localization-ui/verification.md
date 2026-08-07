# 검증 기록

## 2026-08-07 Play 진입·언어별 폰트 correction 독립 QA PASS

- revision: `startup-play-entry-font-profiles-r2-20260807`
- canonical run_id: `startup-play-font-qa-20260807-001`
- candidate fingerprint: `22eef3ed82cb72848ad3d7d78eefd5bf16b9d73815a7a03d8a5d7ac48798531a` / 34 inputs
- manifest 기록 correction: 보호 회귀 확인용 세 scene의 stale category `protected`를 canonical 생성값 `scene`으로 교정했다. manifest 알고리즘 재계산 결과 fingerprint `Match=true`, 입력 bytes/hash `34/34` 일치하며 production/test 변경은 없다.
- correction count 구분: 이번 dynamic QA wrapper의 preflight correction은 `0/2`; 앞선 S0 계약 문구 correction은 별도 `1/2`이며 두 값을 합산하지 않는다.
- 이전 `startup-background-qa-20260807-002`는 사용자 실제 Play 실패와 이후 production 변경으로 현재 완료 판정에서 `SUPERSEDED`.

### S1 정적·import·compile gate

- `StartupPlayModeBootstrap`가 `EditorSceneManager.playModeStartScene`을 저장된 `Assets/_Project/Scenes/Startup.unity`로 고정하고 누락 시 `[StartupPlay:PFC1_MISSING_START_SCENE]`을 기록하는 계약을 확인했다.
- Startup scene은 선택 배경 GUID와 Korean `Galmuri11`, English `Silkscreen-Regular` GUID를 각각 1회 직렬화한다. Assets meta GUID 510개 중 중복은 0이다.
- 두 폰트의 TTF import는 embedded data를 유지한다. family별 `SOURCE.md`의 고정 commit/raw URL/bytes/SHA-256과 `OFL.txt`가 task 계약과 일치한다: Galmuri TTF `E24256...DEF1`, OFL `9A9E5A...69C9`; Silkscreen TTF `C845B4...EBD8`, OFL `86C5DD...FBD3`.
- 같은 `Render()` cycle에서 언어 profile을 먼저 선택하고 active/inactive 전체 `Text`에 적용한 뒤 문자열을 갱신하는 source 계약과, 설정 취소 시 model snapshot 복원을 확인했다. localized catalog의 모든 비공백 문자와 `0-9`, `×`는 Unity font glyph 테스트 대상으로 고정됐다.
- 배경 `raycastTarget=false`, 누락 시 dark-plum 비검정 fallback과 `[StartupUI:PFC6_MISSING_BACKGROUND]`, 언어별 `[StartupUI:PFC6_MISSING_FONT_KO]`/`_EN`, Malgun Gothic→built-in 진단 fallback을 확인했다.
- 정적 대비 계산은 main white `8.675:1`, settings white `17.141:1`, button white `11.265:1`, settings error `6.749:1`로 PFC4 하한을 통과했다. 실제 960×540 preferred bounds/가독성은 Play 수용 항목으로 남는다.
- 이전 manifest의 보호 경로에서 허용 밖 drift 0, gameplay scene/package hash 보존, `git diff --check` 오류 0. QA production 수정 없음.

### Canonical UnityEditMode 결과

- 공용 wrapper `Invoke-HighCostVerification.ps1`, route `UnityEditMode`, criterion `PFC1-PFC8-C1-C9-qa-target`; `QaHarnessPath`는 `UnityProject/Assets/_Project/Tests/EditMode/Startup` 단일 디렉터리만 사용했다. low-level runner 직접 실행 없음.
- XML `artifacts/qa-target-results.xml`, SHA-256 `96E0180878E10A40F7B6B0BF4982ECE514B01677C71CC22C786789770284A352`: **Passed `38/38`**, failed/skipped/inconclusive `0`, Unity exit `0`, `valid_pass=true`.
- PFC1 saved Startup Play entry, 두 font의 scene mapping/import/glyph/source/hash/license, PFC6 diagnostic/fallback, 선택 배경, Build Settings 및 C1~C9 설정·로컬라이제이션 회귀가 같은 후보에서 통과했다.
- cache warmup 중 package test assembly의 일시 `DirectoryNotFoundException`/invalid 메시지가 있었으나 package refresh와 domain reload 뒤 script graph compile, 대상 assembly 실행, XML PASS, `Test run completed. Exiting with code 0 (Ok).`로 복구를 확인했다. 구현자가 겪은 외부 Roslyn `CodePages` blocker는 재사용하지 않았고 Unity import/compile을 새로 통과했다.

### 판정·비용·잔여 수용

- 독립 QA 판정: **표적 자동 기술 검증 PASS — 총괄 판정 및 post-correction 실제 Unity Play/사용자 화면 수용 대기**.
- task 누적 비용: Unity starts `3`, MCP `0`, build `0`, recorded high-cost attempts `3`. 이번 revision 실제 Unity 시작은 1회다.
- capability profile 확인 결과 `McpPlay`는 unavailable(`MCP Play requires a task-specific lease and evidence harness`)이다. lease 없는 우회·직접 Play/capture를 실행하지 않았다.
- 남은 실제 수용: Startup가 backup/blank가 아니라 첫 프레임에 열리는지, 960×540 배경·한국어/영어 폰트·텍스트 bounds/가독성, 설정 preview/취소 복원, 버튼 raycast, Console, `RatHost2DPrototype` 전이를 사용자/등록된 Play harness로 확인해야 한다.

## 2026-08-07 사용자 실제 Play 실패로 이전 판정 SUPERSEDED

- 새 revision: `startup-play-entry-font-profiles-r2-20260807`
- 실제 사용자 oracle: Play 시 배경이 검게 보임.
- Editor 로그: `Loaded scene 'Temp/__Backupscenes/0.backup'`; Startup 씬 진입 증거 없음.
- 이전 canonical `startup-background-qa-20260807-002` / `33/33 PASS`는 Sprite 직렬화·EditMode 계약에는 유효하지만 실제 Editor Play 진입과 화면 수용을 보장하지 못해 현재 완료 판정에서는 `SUPERSEDED`.
- S0 상태: **r2 PASS — 구현 시작 허용**. R3의 구현 후 독립 QA·실제 Play 화면 수용·총괄 판정은 별도 유지.
- 폰트 후보: 공식 `quiple/galmuri`의 `Galmuri11.ttf`, SIL OFL 1.1. TTF·license·출처·해시를 함께 검증한다.
- S0 r0: `BLOCKER` — PFC3 exact glyph/fallback, PFC4 bounds/contrast, PFC6 오류 식별자·진단 fallback, PFC7 pinned source/hash 누락. Unity/MCP/build 0.
- S0 r1: task의 정량 보완으로 glyph set, bounds/contrast, 오류 코드/fallback, commit·URL·TTF/OFL bytes/hash를 고정했다. QA 재검토 대기.
- 사용자 확장: 언어별 폰트 고려 요구로 S0 r1 재검토를 중지하고 r2로 확장했다.
- S0 r2: Korean→Galmuri11, English→Silkscreen Regular 매핑, 언어별 exact glyph·동일 render cycle 전환/취소 복원, 언어별 오류 ID, family별 pinned source/hash/license를 고정했다. QA 재검토 결과 아래 blocker가 남았다.
- S0 r2 correction: r2가 r1 단일 font/generic ID를 명시적으로 SUPERSEDED하고, main/settings 전체 텍스트 대비 하한을 정렬했으며 revision identity를 `startup-play-entry-font-profiles-r2-20260807`로 갱신했다. QA 재검토 PASS.

### S0 r2 correction 재검토

- 판정: **PASS — S0 기준 구현 시작 허용**.
- 이전 blocker 해소: r2 revision ID, r1 단일 font/generic 오류 ID의 명시적 `SUPERSEDED`, main/settings 모든 일반·오류·버튼 텍스트 `4.5:1`과 32px 이상 제목 `3:1`을 확인했다.
- PFC1~PFC8은 Editor Play start scene, first-frame 배경, Korean/English font profile과 같은 Render cycle preview/취소 복원, 언어별 exact glyph, 960×540 bounds/contrast, 입력·전이, 언어별 missing-reference 진단, 두 family의 pinned source/hash/OFL, 보호 diff에 각각 판정 가능한 최소 증거가 연결됐다.
- 실행 비용: Unity/MCP/build/TestRunner `0`; production/test 변경 `0`.

### S0 r2 최초 재검토 — historical BLOCKER, correction으로 SUPERSEDED

- 판정: **BLOCKER — 구현 시작 불가**.
- 해소 확인: Korean/English별 exact glyph 집합, 같은 `Render()` cycle의 문자열+font preview, 취소 시 원자 복원, 언어별 missing-font ID, Galmuri/Silkscreen 각각의 upstream commit·bytes·SHA-256·OFL·family별 분리는 판정 가능하게 고정됐다. PFC1·PFC2·PFC5·PFC7·PFC8도 충분하다.
- blocker 1 — 계약 충돌/precedence: 기존 PFC3과 r1은 정상 경로의 모든 한국어·영어 `Text.font`를 `Galmuri11`로 요구하지만 r2는 English를 `Silkscreen-Regular`로 요구한다. 기존 PFC6의 `[StartupUI:PFC6_MISSING_FONT]`와 r2의 언어별 `_KO`/`_EN` ID도 동시에 남아 있다. r2가 앞 두 단일-font/generic-ID 문구를 명시적으로 `SUPERSEDED`한다고 고정해야 하나의 구현을 PASS/FAIL로 판정할 수 있다.
- blocker 2 — PFC4 설정 대비: r1은 main panel만 최악의 흰 배경에서 `4.5:1`/큰 제목 `3:1`을 고정했다. 원래 PFC4가 포함한 settings panel 한국어·영어의 대비 하한은 여전히 없다. 동일 임계값을 settings의 title/body/label/button에도 적용하거나 별도의 결정론적 임계값을 고정해야 한다.
- blocker 3 — revision identity: task의 verification revision이 여전히 `startup-play-entry-font-correction-r1-20260807`이다. r2 acceptance 계약과 결합된 유일 revision ID로 갱신해야 이후 fingerprint/run이 어느 계약을 검증했는지 구분된다.
- 최소 반례: English 모든 `Text`가 Silkscreen을 사용해 r2를 만족해도 기존 PFC3/r1의 Galmuri 동일성에는 실패하며, settings panel을 흰색으로 바꿔도 현재 r1 정량 contrast는 main panel만 검사하므로 통과할 수 있다.
- 실행 비용: Unity/MCP/build/TestRunner `0`; production/test 변경 `0`.

### S0 r0 계약 검토

- 판정: **BLOCKER — 구현 시작 불가**.
- 원증상 대조: Editor 로그 `23307`, `24281`, `24726`, `24740`에서 Play 진입 대상이 저장된 `Assets/_Project/Scenes/Startup.unity`가 아니라 `Temp/__Backupscenes/0.backup`임을 확인했다. backup에는 `StartupMenuView.startupBackground` 직렬화 신호가 있으므로 이 로그는 잘못된 Play 진입점은 증명하지만 검은 렌더의 단일 원인까지 증명하지는 않는다. PFC1과 PFC2를 분리한 구조는 적절하다.

| criterion | S0 판정 | 근거 |
| --- | --- | --- |
| PFC1 | 충분 | 현재 열린 씬과 무관한 저장 Startup 고정, backup 금지, 실제 로그/Play 확인이 원증상을 직접 판정한다. |
| PFC2 | 충분 | 첫 프레임의 실제 Canvas/Background 상태와 캡처를 함께 요구해 직렬화만 통과하는 false positive를 막는다. |
| PFC3 | **BLOCKER** | `지원 글리프`의 유한 집합과 정상/실패 fallback 경계가 없다. 한국어·영어 카탈로그, 드롭다운, 숫자·해상도·문장부호에서 실제 표시되는 고유 문자를 모두 검사 대상으로 고정해야 한다. |
| PFC4 | **BLOCKER** | `배경 대비가 충분하다`의 PASS/FAIL 기준이 없다. text preferred bounds≤RectTransform과 제목/본문·버튼의 최소 대비 또는 사용자 수용 기준을 수치/절차로 고정해야 한다. |
| PFC5 | 충분 | background raycast와 설정 열기·취소, 실제 active scene이 `RatHost2DPrototype`으로 바뀌는 smoke를 묶으면 입력/전이를 판정할 수 있다. |
| PFC6 | **BLOCKER** | missing Sprite/Font에서 요구할 정확한 오류 식별자와 화면 fallback이 없다. Sprite 누락 시 비검정 진단 배경/오류 배너, Font 누락 시 오류 로그와 진단용 fallback 폰트 허용 여부를 명시해야 한다. 정상 경로의 `LegacyRuntime.ttf` 금지와도 분리해야 한다. |
| PFC7 | **BLOCKER** | 공식 저장소만 있고 tag/commit·다운로드 경로·기대 TTF/OFL SHA-256이 사전 고정되지 않아 다른 버전도 PASS가 된다. 정확한 source identity와 기대 해시를 S0에 잠가야 한다. |
| PFC8 | 충분 | Startup 허용 목록과 core/Build Settings/gameplay/package/reference 금지 목록이 보호 diff 범위를 판정 가능하게 고정한다. |

- 첫 blocker 최소 반례: `Galmuri11.ttf`가 일부 한글만 포함하거나 다른 공식 commit의 파일이어도 현재 PFC3/PFC7 문구만으로는 PASS와 FAIL을 구분할 수 없다.
- correction 요구: PFC3/PFC7의 exact font identity·glyph 집합을 먼저 고정하고, 같은 revision에서 PFC4 대비/bounds와 PFC6 missing-reference 화면·로그 oracle을 함께 결정론화한 뒤 S0 r1 재검토한다.
- 실행 비용: Unity/MCP/build/TestRunner `0`; production/test 변경 `0`.

## 2026-08-07 선택 배경 통합 독립 QA correction PASS

- canonical run_id: `startup-background-qa-20260807-002`
- candidate fingerprint: `be3e9ce5a76ff6951272a6a191a89018a7f28eeef182b30df878495c750d3649` — correction 전후 production 불변
- 대체 run: `startup-background-qa-20260807-001` preflight BLOCKER는 `SUPERSEDED`. 첫 run은 Unity를 시작하지 않았고 결과 XML을 만들지 않았다.
- correction 사유: wrapper와 guard의 다중 `string[]` 외부 script 전달이 `-Path first second`가 되어 두 번째 파일이 positional argument로 차단됐다. 두 C#을 모두 포함하는 공통 디렉터리 `UnityProject/Assets/_Project/Tests/EditMode/Startup` 1개를 전달해 safety lint가 두 파일을 재귀 검사하도록 했다. 검사 범위 축소·우회는 없다.
- fingerprint 17 inputs: reference PNG 1; Unity PNG/meta 2; `StartupMenuView.cs/meta`와 core 3개 5; Startup scene/meta와 Build Settings 3; scene test/meta와 settings test 3; package manifest/lock와 ProjectVersion 3.

### Canonical UnityEditMode 결과

- 공용 wrapper: `Invoke-HighCostVerification.ps1`, route `UnityEditMode`, criterion `BG1-BG6-qa-target`.
- XML: `artifacts/qa-background-results.xml` — SHA-256 `148B49ACD9B683B188E078509E561642A2F3137F00DC7C5A3DD97C6B8F644A0E`, **Passed `33/33`**, failed/skipped/inconclusive `0`.
- Unity exit: `0`, 최종 로그 `Test run completed. Exiting with code 0 (Ok).`
- 새 배경 계약 테스트는 Sprite import, mipmap off, point filter, scene의 정확한 Sprite 직렬화 참조를 통과했다.
- 기존 28개 설정/로컬라이제이션 테스트와 4개 기존 scene/Build Settings 계약을 포함해 C1~C9 관련 회귀가 같은 후보에서 통과했다.
- 격리 cache warmup 중 `Unity.Collections.Tests` 의존 DLL 경로에 일시 `DirectoryNotFoundException`과 invalid package test assembly 메시지가 있었으나, package refresh·domain reload·최종 script graph compile 뒤 대상 assembly와 33개 테스트가 실행되어 XML PASS와 exit 0을 남겼다. Startup source 최종 compile error로 판정하지 않는다.
- 라이선스는 구버전 채널 handshake 실패 뒤 Unity `6000.4.6` 채널 연결·entitlement 획득·license update에 성공했다.

### 현재 criterion 판정

| criterion | 현재 판정 |
| --- | --- |
| BG1 | PASS — rename, 크기, SHA-256 |
| BG2 | PASS — Sprite/UI import와 유일 GUID를 정적+Unity에서 확인 |
| BG3 | 부분 PASS — scene 직렬화·fallback·raycast source 통과, 실제 첫 프레임 렌더 미확인 |
| BG4 | 부분 PASS — 960×540 정적 배치 계약 통과, 실제 clipping/핵심 이미지 가림 미확인 |
| BG5 | 부분 PASS — 설정 UI 계약·회귀 통과, 실제 배경 위 가독성 미확인 |
| BG6 | PASS — 관련 EditMode `33/33`, 기존 `32/32`는 `SUPERSEDED` |
| BG7 | PASS — 보호 scene/package 변경 없음 |
| BG8 | PASS — 시작화면 전용 승인 경계 유지 |
| C1~C9 | 관련 EditMode 회귀 PASS. 실제 Play 입력·렌더·scene transition 체감 항목은 미확인 |

### 비용·capability·판정

- correction run 실제 시작: Unity `1`, MCP `0`, build `0`. task 누적 current-state는 과거 Startup QA를 포함해 Unity `2`, MCP `0`, build `0`.
- full suite·대형 matrix·build는 실행하지 않았다. low-level runner 직접 실행 없음.
- McpPlay는 capability profile상 unavailable(`MCP Play requires a task-specific lease and evidence harness`)이므로 우회하지 않았다.
- 보호 diff와 `git diff --check` 재확인 PASS. 원본 production은 격리 cache 실행으로 변경되지 않았다.
- 독립 QA 판정: **표적 자동 기술 검증 PASS — 총괄 판정 및 Unity Play/사용자 화면 수용 대기**.
- 남은 사용자 확인: 960×540 실제 첫 화면에서 배경 표시, 왼쪽 메뉴·설정 가독성, 버튼 입력/raycast, Console, `RatHost2DPrototype` 시작 전이.

## 2026-08-07 선택 배경 통합 독립 QA — preflight BLOCKER

- verification revision: `startup-selected-background-integration-r1-20260807`
- 독립 QA run_id: `startup-background-qa-20260807-001`
- QA canonical candidate fingerprint: `be3e9ce5a76ff6951272a6a191a89018a7f28eeef182b30df878495c750d3649` / 변경 production·회귀·package/version 17 inputs
- 구현 보고 fingerprint `6b8c3b18fd161ff218bfa78a73c9777ade2231ae4590d5d7b45e97b4869d0f2b`는 13개 입력 경로 목록이 없어 재현 가능한 canonical 값으로 채택하지 않고 비교 참고로만 보존한다.
- 후보 freeze: 정적 검사 시점의 위 17개 입력으로 고정. production 변경 시 이 판정 전체를 `SUPERSEDED` 처리한다.

### S1 정적·무결성 결과

- `git diff --check`: exit `0`. 줄바꿈 변환 warning 외 whitespace 오류 없음.
- BG1: 기존 `docs/design/visual/references/image.png`는 없고 설명적 이름의 reference가 존재한다. reference는 `1672×941`, SHA-256 `5ED62B0BE9E0FC68FED15135C8BEDB3F08639CD020E914EF420FE73831B17C8D`로 계약과 일치한다.
- BG2: Unity import PNG도 같은 크기·SHA-256이다. meta의 Sprite/UI 계약은 `textureType: 8`, `spriteMode: 1`, mipmap off, point filter이며 GUID `9e248f35b7804a7c8e61d9f2a4b5c6d7`은 `Assets` 전체에서 1회만 정의된다.
- BG3: `Startup.unity`가 해당 GUID를 1회 직렬화 참조한다. `StartupMenuView`는 전체 stretch 배경, `raycastTarget=false`, sprite null 시 기존 어두운 색 fallback을 유지한다.
- BG4: 960×540 CanvasScaler에서 main panel 중심 `x=-285`, 크기 `360×420`, 제목/태그라인 폭 `332/326`, 버튼 폭 `300`으로 왼쪽 여백 안의 정적 경계에 들어간다.
- 이전 후보 manifest 19개 입력 대조 결과 변경은 허용된 `StartupMenuView.cs`, `Startup.unity`, `StartupSceneContractTests.cs` 3개뿐이다. `StartupLocalization.cs`, `StartupSettings.cs`, `StartupController.cs`, `StartupSettingsTests.cs`, Build Settings, package manifest, Unity version과 관련 meta는 이전 해시를 유지한다.
- 보호 diff: `RatHost2DPrototype.unity`, `RatHostPrototype.unity`, `Packages/manifest.json`, `Packages/packages-lock.json`에 이번 후보의 Git 변경 없음. Startup 코어 3개는 기존 untracked dirty baseline이지만 이전 manifest 해시와 일치한다.

### criterion → evidence 판정

| criterion | 현재 증거 | 판정 |
| --- | --- | --- |
| BG1 | rename 존재/원명 부재, PNG header, SHA-256 | PASS |
| BG2 | Unity copy hash, TextureImporter meta, GUID 전체 단일성 | 정적 PASS / 실제 import 미검증 |
| BG3 | scene GUID, Image sprite/fallback/raycast source | 정적 PASS / 첫 프레임 미검증 |
| BG4 | 960×540 좌표·크기 source 계약 | 정적 PASS / 실제 clipping 미검증 |
| BG5 | 기존 880×510 설정 패널·텍스트 계약과 core hash 보존 | 회귀 정적 PASS / 실제 배경 위 가독성 미검증 |
| BG6 | core·settings test·Build Settings 이전 hash 보존 | 정적 PASS / 관련 EditMode `SUPERSEDED` 상태 |
| BG7 | 보호 scene/package Git status와 이전 manifest 대조 | PASS |
| BG8 | task의 시작화면 전용 승인 경계와 reference README 대조 | PASS |
| C1~C9 | 기존 core·Build Settings 계약 해시 보존 | 정적 회귀만 PASS / 이전 `32/32`는 `SUPERSEDED` |

### 첫 blocker와 고비용 실행

- 공용 wrapper 진입: `tools/verification/Invoke-HighCostVerification.ps1`, route `UnityEditMode`, criterion `BG1-BG6-qa-target`.
- preflight blocker: `Test-QaHarnessSafety.ps1` 호출에서 두 QA harness 경로 중 두 번째 `StartupSceneContractTests.cs`를 받을 positional parameter가 없어 exit `1`.
- 최소 반례: wrapper에 `QaHarnessPath` 두 개를 주면 내부 `-Path first second` 전달에서 두 번째 경로가 positional argument로 해석되어 Unity 시작 전에 차단된다.
- fail-fast: 첫 blocker에서 중단했으며 재시도, full suite, matrix, build, MCP, low-level runner 직접 실행은 하지 않았다.
- 결과 XML/log: 생성되지 않음. Unity process 시작 `0`, MCP `0`, build `0`.
- task 누적 비용 state는 과거 Startup QA Unity `1`을 유지하며 이번 revision이 추가한 실제 high-cost 시작은 `0`이다. preflight 차단은 사용자-facing 고비용 실행 횟수로 세지 않는다.
- McpPlay capability: profile상 unavailable(`MCP Play requires a task-specific lease and evidence harness`). 우회하지 않았다.

### dirty baseline과 완료 판단

- 시작 시 기존 dirty: `EditorBuildSettings.asset`, Startup scene/scripts/tests 전체 untracked, 관련 active 기록과 현황판 변경. 사용자·기존 작업 변경을 되돌리지 않았다.
- QA production 수정: 없음. QA는 current-state/brief와 본 canonical 검증 기록만 갱신했다.
- 현재 상태: **BLOCKED — 정적 QA PASS, 관련 UnityEditMode·Play 화면 확인 미완료**.
- 사용자 수용 잔여: 실제 Startup 960×540에서 배경 표시, 왼쪽 메뉴와 설정 가독성, 버튼 raycast, Console, 시작 전이 확인.
- 완료 판단: **완료 불가**. wrapper harness 인수 전달을 구현/검증 도구 소유자가 교정하고 같은 production fingerprint로 구현자 최소 확인 후 독립 QA 재접수 1회가 필요하다.

## 2026-08-07 선택 배경 통합 검증 대기

- verification revision: `startup-selected-background-integration-r1-20260807`
- 상태: S0 계약 고정, 구현 배정 대기
- 이전 `32/32` UnityEditMode PASS는 배경 production·관련 테스트 변경 시 `SUPERSEDED` 처리한다.
- 새 canonical candidate fingerprint/run_id: 구현 freeze 후 독립 QA가 기록한다.
- 필수 순서: 정적/import 계약 → 관련 EditMode → Startup Play smoke/960×540 화면 확인 → 보호 diff → 총괄 판정.
- 고비용 검증: `tools/verification/Invoke-HighCostVerification.ps1` 외 직접 실행 금지.
- 사용자 수용: 기술 검증 뒤 실제 시작 화면의 구도·가독성 확인 필요.

## 작업 ID

`2026-08-05-startup-settings-localization-ui`

## 검증 대상

Startup 씬, 설정 저장, 한국어·영어 키 기반 UI, `RatHost2DPrototype` 시작 전이와 보호 경계.

## 검증 담당

독립 QA/검증 에이전트 — S0 및 post-implementation 독립 정적 QA

## 원래 증상 또는 완료 주장

현재 게임은 시작 화면·설정·언어 전환 없이 레거시 씬을 직접 시작한다. 완료 시 다국어 준비형 Startup UI와 설정 적용/취소/복원, 2D 프로토타입 시작이 동작해야 한다.

## 현재 검증 revision

- 위험 등급: R3
- verification revision: S0 r2 + candidate `d10c8cae0d0908828c038c5f2e689e32c765bf09659360cbe5a3915f33b8eb57`
- candidate fingerprint: `d10c8cae0d0908828c038c5f2e689e32c765bf09659360cbe5a3915f33b8eb57`
- 독립 정적 QA run_id: `startup-settings-independent-static-qa-20260805-001`
- canonical 동적 run_id: `startup-settings-qa-20260805-001`
- candidate frozen 여부: 예 — production owner release 및 19개 입력 해시 일치
- current-state JSON 대조: `independent-qa-pass-awaiting-director`, 동일 run/fingerprint, Unity 1/MCP 0/build 0
- capability route / wrapper preflight: UnityEditMode available, build/McpPlay 일반 route unavailable
- attempt ledger 연속 실패: 0

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 예
- 구현 주체가 실행한 검증과 별도로 확인한 항목: fingerprint 19개 입력, 정적 compile/source/API, C1~C9 정적 계약, 보호 diff

## 실행한 검증

post-implementation 독립 정적 QA bundle 1회를 수행했다. 기존 PowerShell 7 prerequisite blocker는 재시도하지 않았고 wrapper·lease·Unity/MCP/build/TestRunner/low-level runner는 모두 0회다.

## Post-implementation 독립 정적 QA

- run_id: `startup-settings-independent-static-qa-20260805-001`
- candidate fingerprint: `d10c8cae0d0908828c038c5f2e689e32c765bf09659360cbe5a3915f33b8eb57`
- manifest 실제 이름: `artifacts/candidate-fingerprint.json` (`integration-fingerprint-manifest*.txt`는 존재하지 않음)
- fingerprint 대조: current-state·manifest·요청 fingerprint 일치, manifest의 production/test/scene/package/version 19개 현재 길이·SHA-256 모두 일치. stale 없음.
- 정적 컴파일: 1회 호출했으나 QA 참조 집합에 `netstandard 2.1`이 빠져 CS0012만 발생했다. source 오류로 판정하지 않고 재실행 없이 source/API compile audit로 제한했다.
- source/API compile audit: 구현자와 분리해 runtime·editor API 경계, namespace/type 연결, 테스트의 공개 API 사용을 대조했고 정적 blocker 없음.
- `git diff --check`: exit 0.
- 로컬라이제이션: `StartupTextKey`/한국어/영어 `28/28/28`, 빈 키 없음 계약, `StartupMenuView` 직접 표시 문자열 0, `localizer.GetText` 경유 24곳. 해상도 숫자와 object name은 허용 예외로만 남는다.
- 설정·수명주기: Draft preview/cancel, defaults Draft-only, Apply 검증→platform apply→전체 저장 순서, PlayerPrefs 전체 묶음/부분·손상·미지원 fallback의 source와 표적 테스트 계약 일치.
- 씬·Build Settings: 정확한 2D 경로, Startup enabled index 0, 2D enabled index 1, legacy/sample disabled 보존 정적 PASS.
- asset identity: Startup scene의 `StartupMenuView` GUID가 meta와 일치하고 Assets meta 496개 GUID 중 중복 0.
- UI·입력·오류 경계: 960×540 CanvasScaler, `InputSystemUIInputModule`, Esc 설정 취소, Editor quit no-op/standalone quit, 적용 오류 key 경계 정적 PASS.
- 보호 경계: tracked `RatHost2DPrototype`, `RatHostPrototype`, package manifest는 변경 없음. `docs/references/`는 handoff에 기록된 기존 사용자 소유 untracked 항목 1건으로 candidate 변경에서 제외했다.

### C1~C9 정적 판정

| criterion | 독립 정적 판정 | 동적 제한 |
| --- | --- | --- |
| C1 | Startup index 0, 단일 scene View GUID와 초기 main/settings panel 코드 계약 PASS | 실제 첫 프레임·표시 미검증 |
| C2 | 정확한 `RatHost2DPrototype` 경로와 Build Settings index 1 PASS | 실제 scene transition 미검증 |
| C3 | Draft preview, write/apply 0, cancel/Esc 복원 source·spy test 계약 PASS | 실제 UI 상호작용 미검증 |
| C4 | 기본 프로필·해상도 후보·Draft-only defaults·Apply 순서 PASS | 실제 Screen 적용 미검증 |
| C5 | 28/28/28 키와 visible text localizer 경유 PASS | 동일 render cycle 화면 미검증 |
| C6 | 전체 bundle validation과 원자 default fallback 계약 PASS | 실제 PlayerPrefs 새 세션 persistence 미검증 |
| C7 | 960×540 기준·880×510 설정 패널 정적 경계 PASS | clipping/긴 영문 Play 캡처 미검증 |
| C8 | tracked 보호 대상·package 변경 없음, 신규 GUID 중복 0 PASS | 런타임 회귀 미검증 |
| C9 | Input System Esc, Editor/standalone quit, 오류 key 경계 PASS | 실제 입력·종료 동작 미검증 |

독립 정적 QA 판정: **PASS**. Unity 동적 검증은 미완료이며 사용자 화면 수용은 대기다.

## Canonical UnityEditMode 결과 감사

- 감사 성격: 상태-only evidence audit. Unity/wrapper를 재실행하지 않았고 별도 QA run으로 세지 않는다.
- canonical run/fingerprint: `startup-settings-qa-20260805-001` / `d10c8cae0d0908828c038c5f2e689e32c765bf09659360cbe5a3915f33b8eb57`.
- canonical XML: `artifacts/qa-target-results.xml` — Passed `32/32`, failed/skipped/inconclusive `0`, EditMode, Unity process PID `35824`.
- canonical log: `artifacts/qa-target-unity.log` — 최종 `Test run completed. Exiting with code 0`, Unity exit `0`.
- preflight 이력: 첫 wrapper 호출은 current-state evidence run_id stale로 preflight 차단되어 Unity 시작 `0`; evidence 메타데이터를 같은 run/fingerprint로 동기화한 뒤 실제 Unity 표적 실행 `1`회 성공. preflight correction `1/2`.
- 비용: Unity `1`, MCP `0`, build `0`, 실제 high-cost 실행 `1`.

로그 sequence 판정:

1. isolated cache 최초 warmup에서 PackageCache가 재구축되는 동안 ShaderGraph package 파일 두 곳이 `GUID` CS0246를 남겼다.
2. package refresh 뒤 `LastHost.Prototype.dll`과 `LastHost.Prototype.Tests.dll`이 다시 Csc/IL post-process/copy 되었고, 이 후 Startup source compile error는 없다.
3. 최종 어셈블리로 표적 EditMode 32개가 실행되어 XML 전부 PASS와 exit 0을 남겼다.

따라서 초기 CS0246는 Startup source 결함이나 최종 컴파일 실패가 아니라 격리 cache/package warmup 중 일시 로그로 판정한다. 결과 무효화 근거는 아니다. 다만 격리 cache cold-start에서 같은 복구 지연이 다시 나타날 수 있는 운영 리스크로 보존한다. Licensing은 초기 구버전 채널 handshake 실패 뒤 Unity 6000.4.6 채널의 entitlement를 정상 획득했다. Relay warning은 테스트 종료 선언 뒤 AI Assistant relay에서 발생했으며 표적 테스트 결과와 분리한다.

동적 증거 범위:

- C3~C6 설정 상태·PlayerPrefs·fallback, C2/C9 명령 경계, C1/C8 Startup scene/Build Settings 계약은 UnityEditMode에서 통과했다.
- 실제 Play 렌더, 한·영 화면 preview, 960×540 clipping, 버튼 입력, 실제 scene transition, standalone 종료, MCP 캡처는 수행하지 않았다.
- PC build도 수행하지 않았다.

canonical 판정: **independent-qa-pass-awaiting-director**. 표적 UnityEditMode 기술 증거는 유효하며, Unity Play/MCP와 사용자 화면 수용은 대기다.

## S0 계약 검토 이력

### r0 historical BLOCKER

- 판정: `BLOCKER` — r1 계약으로 대체됨
- blocker: C3과 C5가 `적용` 전 언어 초안의 화면 반영과 `취소` 후 복귀 동작을 하나의 oracle로 잠그지 못했다.
- correction: r1에서 언어 Draft 즉시 preview, 저장·플랫폼 적용 0회, 취소/Esc 원자 복원으로 해소했다.
- 동적 실행: 0

### r1 재검토

- 판정: `BLOCKER` — r2 재분류 계약으로 대체됨
- correction cycle: `1/2`, 같은 S0 계약의 연속 두 번째 blocker
- r0 blocker 해소 확인: C3·C4·C5의 preview/apply/cancel 시점과 부작용 경계는 서로 일치했다.
- blocker: C4의 `기본값` Draft와 C6의 손상값 복구가 사용할 정확한 기본 설정 프로필 또는 결정 규칙이 없었다.
- 동적 실행: 0

최소 반례:

1. 저장값이 없거나 언어·해상도 저장값이 손상된 상태에서 설정을 열거나 `기본값`을 누른다.
2. 구현 A는 한국어·창 모드·현재 데스크톱 해상도·VSync 켜짐을, 구현 B는 시스템 언어·전체 화면·960×540·VSync 꺼짐을 기본값으로 선택할 수 있다.
3. 두 구현 모두 현재 C4의 “기본값은 Draft만 변경”과 C6의 “안전 기본값” 문구를 만족하지만 사용자에게 보이는 결과가 다르므로 하나를 PASS/FAIL로 판정할 oracle이 없다.

### 재분류 기록

- reclassification ID: `startup-settings-s0-default-profile-r2-20260805`
- root cause: Draft 전이만 먼저 잠그고 최초 실행·기본값·손상값 복구가 공유할 결정론적 기본 프로필과 해상도 선택 우선순위를 누락했다.
- change plan: `Korean`, `FullScreenWindow`, VSync `1`과 `1920×1080` 우선→지원 16:9 최고→지원 최고→현재 화면값을 C4에 고정하고, C6의 최초·부분 키·손상·미지원 상태는 이 전체 프로필로 원자 복귀시킨다.
- 위험 등급 / production owner: R3 유지 / 기존 단일 소유권 유지
- correction cycle: r2 경계에서 `0/2`로 재시작

### r2 최종 재검토

- 판정: **PASS**
- 구현 시작 허용: **예 — S0 계약 기준. R3 총괄 사전 게이트는 별도 유지**
- 검토 방식: 지정된 3개 기준 파일의 문서 계약만 대조했다.
- 동적 실행: Unity/MCP/build/TestRunner `0`회
- 해석: C1~C9가 원증상, 성공/실패, 경계, 상태 전이, 수명주기, negative control, 설정·다국어·씬 전이를 구현 전에 판정 가능한 oracle과 최소 증거에 연결한다.

### criterion → 최소 증거 추적

| criterion | 최소 canonical 증거 | S0 검토 상태 |
| --- | --- | --- |
| C1 | Build Settings 0번·Startup 초기 UI/패널 상태 씬 계약 manifest + Startup Play smoke | 잠김 |
| C2 | 시작 명령 순수 테스트 + 실제 `RatHost2DPrototype` 로드 scene smoke | 잠김 |
| C3 | 적용값·저장값 baseline, Draft preview, 취소/Esc 뒤 전체 문자열 원자 복원과 저장소 write/platform apply 0회를 묶은 상태 trace | 잠김 — r0 blocker 해소 |
| C4 | 기본 프로필과 1920×1080 지원/16:9만 지원/비16:9만 지원/빈 목록 후보표, Draft-only 기본값, Apply 순서를 묶은 상태·저장소·platform spy 테스트 | 잠김 — r1 blocker 해소 |
| C5 | 한·영 키 집합 완전성 + 실제 Startup 전체 텍스트 preview/apply/cancel scene trace | 잠김 — r0 blocker 해소 |
| C6 | 최초·부분 키·손상·미지원 각 입력의 전체 기본 프로필 원자 복귀와 정상 묶음 복원을 확인하는 저장소 수명주기 테스트 | 잠김 — r1 blocker 해소 |
| C7 | 960×540 포함 4개 16:9 후보 정적 레이아웃 계약 + 대표 Play 캡처 1개 | 잠김 |
| C8 | 보호 대상 3D/2D 씬·패키지·reference의 변경 없음 diff manifest | 잠김 |
| C9 | 종료/Esc 명령 테스트 + Editor 안전·설정 복귀·standalone 종료 경로 scene smoke | 잠김 |

## fail-fast·무효화

- first blocker: 현재 canonical 후보 blocker 없음. stale evidence preflight 차단은 correction `1/2`로 해소
- correction cycle: r0/r1 연속 blocker 2회 뒤 재분류, r2 `0/2`
- reclassification ID: `startup-settings-s0-default-profile-r2-20260805`
- S0 historical blocker / 현재 판정: r0 C3/C5 BLOCKER(해소) / r1 C4/C6 BLOCKER(해소) / r2 PASS
- 동적 실행 횟수: wrapper 실제 실행 1 / Unity 1 / MCP 0 / build 0. 첫 preflight 차단은 Unity 시작 0
- S6 전체 suite 실행 허용/실행 횟수: 미허용 / 0
- S7 대형 matrix 실행 허용/실행 횟수: 불필요 / 0
- low-level runner 직접 Run 차단 확인: 예

## 게이트 판정

- QA/검증 게이트 통과 여부: 표적 UnityEditMode 독립 QA PASS
- 총괄 관리자 검토로 넘길 수 있는지: 예 — `independent-qa-pass-awaiting-director`

## 완료 판단

`표적 UnityEditMode 독립 QA PASS — 총괄 판정 대기 — Unity Play/MCP 및 사용자 화면 수용 대기.`

## 사용자 수용 상태

- 사용자 직접 확인 필요: 시작 화면 첫인상, 한·영 전환, 설정 UI 가독성, 시작 흐름
- 확인 전 `완료` 표현 금지 여부: 예

## 2026-08-07 선택 배경 통합 구현자 점검

- verification revision: `startup-selected-background-integration-r1-20260807`
- 구현 owner: Unity 씬/통합 구현 에이전트
- candidate fingerprint: `6b8c3b18fd161ff218bfa78a73c9777ade2231ae4590d5d7b45e97b4869d0f2b` / production·test·package/version 입력 13개
- BG1: 원본을 `startup-bacteriophage-food-chain-background-v1.png`로 rename했고 reference와 Unity import copy가 모두 `1672×941`, SHA-256 `5ED62B0BE9E0FC68FED15135C8BEDB3F08639CD020E914EF420FE73831B17C8D`로 일치한다.
- BG2~BG4 정적 점검: Sprite/UI meta, 유일 GUID, 씬 직렬화 참조, background raycast 비활성 코드, `960×540` 왼쪽 메뉴 좌표·크기 계약을 대조해 `STATIC_CHECK_PASS`.
- BG3 fallback: 씬 참조가 누락되면 기존 어두운 단색 배경을 사용하도록 유지했다.
- 보호 경계: `StartupLocalization.cs`, `StartupSettings.cs`, `StartupController.cs`, Build Settings, 기존 2D/3D 씬, package는 이번 구현에서 수정하지 않았다.
- 고비용 실행: Unity/MCP/build/TestRunner `0`회. low-level runner 직접 실행 없음.
- 무효화: 선택 PNG, `StartupMenuView.cs`, Startup scene과 관련 테스트가 바뀌었으므로 기존 canonical UnityEditMode `startup-settings-qa-20260805-001`의 PASS는 현재 revision에 대해 `SUPERSEDED`다.
- 현재 상태: 구현자 정적 점검 통과. 독립 QA의 wrapper 기반 관련 EditMode 및 가능한 Play 화면 확인 전 기술 완료를 주장하지 않는다.

## 2026-08-07 Play 진입·언어별 폰트 correction 구현자 점검

- verification revision: `startup-play-entry-font-profiles-r2-20260807`
- 구현 owner: Unity 씬/통합 구현 에이전트
- PFC1: `Assets/_Project/Editor/Startup/StartupPlayModeBootstrap.cs`가 domain/script reload마다 `EditorSceneManager.playModeStartScene`을 저장된 `Assets/_Project/Scenes/Startup.unity`로 설정한다. 씬 누락 시 start scene을 비우고 `[StartupPlay:PFC1_MISSING_START_SCENE]` 오류를 남긴다.
- PFC2/PFC5/PFC6: 기존 선택 Sprite 직렬화와 full-stretch·`raycastTarget=false`를 보존했다. Sprite 누락은 `[StartupUI:PFC6_MISSING_BACKGROUND]`와 불투명 dark-plum 진단 배경으로 드러내며 순검정 성공처럼 숨기지 않는다.
- PFC3: Startup scene에 `koreanFont`=`Galmuri11`, `englishFont`=`Silkscreen-Regular`를 각각 직렬화했다. `Render()`의 언어 preview cycle에서 활성·비활성 자식 `Text` 전체에 현재 언어 Font profile을 함께 적용하며 cancel 복원도 같은 localizer render 경로를 사용한다. 정상 serialized 경로에서 `LegacyRuntime.ttf`를 사용하지 않고, 누락 진단 fallback에서만 `Malgun Gothic`→built-in 순서를 사용한다.
- PFC6: 누락 ID는 Korean `[StartupUI:PFC6_MISSING_FONT_KO]`, English `[StartupUI:PFC6_MISSING_FONT_EN]`으로 분리했다. 다른 지원 언어 폰트를 암묵 fallback으로 사용하지 않는다.
- PFC7: family별 `Galmuri11/`, `Silkscreen/` 폴더에 TTF·별도 `OFL.txt`·`SOURCE.md`를 반입했다. pinned raw URL 다운로드 직후와 반입 후 바이트·SHA-256을 대조해 계약값 4개 모두 PASS했다.
- 관련 테스트 보강: public property/API와 scene serialization으로 background/font mapping을 확인하고, playModeStartScene, 언어별 localizer exact glyph+`0~9`·`×`, pinned binary/license/source, stable missing-reference diagnostic를 검증하도록 `StartupSceneContractTests.cs`를 확장했다. private reflection은 추가하지 않았다.
- GUID 정적 검사: 신규 meta를 포함한 `Assets` 전체 GUID 중복 `0`.
- 보호 경계: 이전 `candidate-fingerprint.json`과 대조해 `StartupLocalization.cs`, `StartupSettings.cs`, `StartupController.cs`, `EditorBuildSettings.asset`, package manifest/lock, ProjectVersion의 SHA-256이 모두 동일하다. gameplay scene·다른 visual asset은 수정하지 않았다.
- `git diff --check`: exit `0`(기존 line-ending warning만 존재).
- 정적 컴파일 blocker: 설치 Unity Roslyn 두 경로가 모두 compiler 진입 전에 `System.Text.Encoding.CodePages, Version=4.1.1.0` 로드 실패로 중단됐다. 코드 diagnostic가 아니며 같은 원인의 추가 재시도는 중단했다. QA의 공용 wrapper 기반 Unity compile/import에서 확인해야 한다.
- 고비용 실행: Unity/MCP/build/TestRunner `0`회. low-level runner 직접 실행 없음.
- 남은 실제 위험: Unity import가 font meta/fileID와 glyph를 실제로 수용하는지, Editor Play가 Startup으로 진입하는지, 첫 frame background, 한·영 draft/cancel font 전환, 960×540 bounds/contrast, 버튼·씬 전이는 미검증이다.
- 현재 상태: **구현 후보 제출 — 정적 무결성 PASS, Roslyn harness prerequisite BLOCKED, 독립 QA Unity compile·Play 확인 대기**. 완료 선언 금지.
