# 검증 기록

## 검증 대상

Unity EditMode 전체 테스트와 쥐 걷기(v3), 스프라이트 해상도(v4), 픽셀 처리(v5b), 카메라·RatVisual·WASD 회귀 기술 게이트.

## 완료 주장

초기 전체 EditMode 101개 중 2개가 실패했으나 담당 구현 에이전트의 테스트 계약 최소 수정 후 독립 전체 재실행에서 101/101이 통과했다. MCP Play와 전후 불변성까지 확인했으므로 이번 자동 기술 게이트는 완료 가능하다.

## 사전 기준

- Git HEAD: `53037318294471598d14c7d8d7dadd683fed1fbc`
- 기존 범위 밖 변경: `UnityProject/ProjectSettings/ProjectSettings.asset`, `_workspace/previews/`
- 작업 시작 문서 변경: `_workspace/active/CURRENT.md`, 공유 상태판, 본 active 작업 패킷
- staged 변경: 0
- `Builds/` 변경: 0
- Unity: PID `42724`, `6000.4.6f1`, Edit 상태, 비컴파일, 비업데이트
- 자식 Unity PID `88840`, `25940`은 PID `42724`의 AssetImportWorker이며 별도 Editor가 아니다.
- 활성 씬: `RatHostPrototype`, clean, build index 0
- 사전 Console Error/Warning: 0
- 씬 SHA-256: `68C222F449C530B54E5319BD11D94C7E3851161906ED9C19CD6F2FC073C88F02`
- ProjectSettings SHA-256: `008078ADBB3A01264F4C097558F5983453A93F6254E600AB2776D269DD8201D9`
- 테스트 파일 SHA-256: `E800B04D963BE78D1E99C600FCBC6D8C5AAAEA4FE0B1DB9F85345BCD935BD986`

### 실행 방식 결정

권장 batchmode 명령은 실행하지 않았다. 같은 `projectPath`를 기존 Editor가 열고 있어 별도 Unity batch 프로세스를 시작하면 프로젝트 잠금 충돌이 발생할 수 있기 때문이다. 기존 프로세스를 임의 종료하지 않고, 현재 Editor의 공식 `UnityEditor.TestTools.TestRunner.Api.TestRunnerApi`에서 전체 EditMode 필터를 동기 실행했다. 결과는 `TestRunnerApi.SaveResultToFile`로 NUnit XML에 저장했다.

첫 MCP 동적 명령은 중첩 callback 클래스가 동적 컴파일 래퍼에서 중복 배치되어 `CS1527`로 테스트 시작 전에 실패했다. 프로젝트 컴파일 오류가 아니라 QA 명령 컴파일 오류이며 프로젝트 파일을 만들거나 바꾸지 않았다. callback을 최상위 `internal` 클래스로 바꾼 다음 실행은 컴파일·실행에 성공했다.

## 실행 결과

| 검증 | 결과 | 증거 |
| --- | --- | --- |
| 전체 EditMode TestRunner | 실패 — 101개 중 99 통과, 2 실패, skip 0, inconclusive 0, 9.3474759초 | `artifacts/editmode-results.xml`, `artifacts/unity-editmode.log` |
| v3 걷기·8방향 | 관련 방향·idle·walk·asset 테스트 통과 | XML과 `artifacts/editmode-summary.md` |
| v4 해상도·접지 | asset canvas/pivot, 접지·ground resolver 통과 | XML과 summary |
| v5b 픽셀 | 카메라 output snap·invalid PPU 통과, RatVisual pixel snap 계약 1개 실패 | XML과 summary |
| 카메라·씬 | 카메라 모드·축·즉시 추적·출력 snap 통과, 씬 기본 카메라 1개 실패 | XML과 summary |
| WASD·숙주 본능 | RatHostControlModel·HostInstinct·wander 관련 식별 테스트 통과 | XML과 summary |
| 컴파일·Console | TestRunner 실행 명령 컴파일 성공, 종료 후 Editor 비컴파일, Console Error/Warning 0 | Unity MCP 상태·Console |
| MCP Play | 미실행 — 전체 테스트 실패 시 수정 없이 중지 조건 적용 | 본 기록 |
| 씬·ProjectSettings·Builds 비변경 | 통과 | 전후 SHA-256·Git 대조 |

## 실패 상세

### RatDirectionalSpriteView pixel snap

- 실패 테스트: `RatDirectionalSpriteView_PixelSnapKeepsHostAndGroundClearanceWhileSnappingVisualHorizontally`
- 스택: `RatHostPrototypeCoreTests.cs:1794`
- 결과: 화면 표시 메시지의 기대·실제 Vector3는 동일하게 반올림되지만 exact equality가 실패했다.
- 경계: 앞선 240회 host anchor 스냅·local 누적 이탈·접지 tolerance 검사는 통과했다. 마지막 pixel snap 비활성 후 부모 Transform world position exact 비교의 정밀도/테스트 계약 경계가 유력하다.
- 인계: 게임플레이 구현 에이전트가 실제 성분 차이를 정밀 재현한 뒤 테스트 tolerance 또는 구현 중 최소 수정 대상을 판단한다.

### RatHostPrototypeScene camera

- 실패 테스트: `RatHostPrototypeScene_DefaultsToThirdPersonCameraController`
- 스택: `RatHostPrototypeCoreTests.cs:2064`
- 결과: 테스트가 `FindAnyObjectByType<Camera>()`로 얻은 `GameViewFrameCamera`와 컨트롤러가 부착된 `IsometricCamera`가 다르다.
- 경계: 씬에는 출력용 `GameViewFrameCamera`가 추가돼 있고 실제 카메라 컨트롤러는 `IsometricCamera`에 있다. 씬 `startingHostMode: 1`은 `QuarterView`인데 테스트는 ThirdPerson 기본도 요구한다. 현재 승인된 고정 쿼터뷰·출력 카메라 구성과 기존 씬 테스트 계약의 불일치다.
- 인계: Unity 씬/통합 구현 에이전트가 씬 의도를 보존하는 테스트 계약과 연결 검증 기준을 정한다.

## 원본 증거

- XML SHA-256: `90DD2993F52BD3BA6AB9A2D3627ABFD86363C88FAE1444144892C7A41D0FFDA4`
- 로그 SHA-256: `59E1D6FDF8086AF21CA457C24DBC6976EE3C65ECD0BFCA24757399F057817D63`
- XML 총괄: `total=101`, `passed=99`, `failed=2`, `skipped=0`, `inconclusive=0`, `duration=9.3474759`

## 전후 경계

- 씬, ProjectSettings, 테스트 파일 SHA-256이 모두 사전 기준과 동일하다.
- `Builds/` Git 변경은 전후 0이다.
- Unity tracked diff는 기존 사용자 `ProjectSettings.asset` 1개뿐이다.
- 테스트 후 Editor는 Edit 상태, 비컴파일이며 활성 씬은 `RatHostPrototype`, clean이다.
- 테스트 후 Console Error/Warning은 0이다.
- 기존 Unity PID `42724`와 자식 AssetImportWorker를 종료하지 않았고 새 QA batch Editor를 만들지 않았다.

## 미검증 항목

- 두 실패 수정 이후 전체 EditMode 재실행.
- 실패 테스트의 수정안과 담당 구현 에이전트 산출물.
- MCP Play의 RatHost/RatVisual/camera/HUD/Console/Stop 확인. 전체 테스트 실패로 이번 시도에서는 실행하지 않았다.
- 사용자 시각 수용은 본 작업의 검증 대상이 아니다.

## 초기 QA 완료 판단

`수정 필요`

전체 TestRunner와 전후 비변경 검증은 실행됐지만 필수 조건인 실패 0을 충족하지 못했다. 실패 2개를 코드·테스트·씬 수정 없이 원인 경계로 분리했으며, 담당 구현 에이전트의 최소 수정과 전체 재검증이 필요하다.

## 수정 후 독립 전체 재검증

### TestRunner

- 실행 시각: 2026-07-24 14:20 KST
- Run ID: `c7ba0367-3339-4c63-aec5-b1deb3a949e8`
- 결과: `101 total / 101 passed / 0 failed / 0 skipped / 0 inconclusive / 6.3731956s`
- XML: `artifacts/editmode-results-after-fix.xml`
- XML SHA-256: `FB20ABC4DE772EBD5605691A87E6E9C53DC4F1A2460D7B5C2983F146D336F105`
- 로그: `artifacts/unity-editmode-after-fix.log`
- 로그 SHA-256: `7C87D72222FC29DBC34348C8CEA9D819F5EBF8824432EE930827544010635933`
- 요약: `artifacts/editmode-summary-after-fix.md`

관련 축의 WASD·숙주 본능, v3 방향·걷기, v5b RatVisual·카메라 pixel snap, 카메라 모드·QuarterView 추적·씬 기본 계약은 모두 통과했다. v4 관련 현재 자동화는 asset canvas/pivot, 씬 가시성, 접지·ground resolver를 포함한다.

다만 현재 importer 테스트는 TrialV1의 64×64·PPU 32·custom pivot을 검사하며 v4의 128×128·PPU 64·world width 2를 직접 검사하는 테스트는 아니다. v4 선행 MCP 검증은 유지하되 이 직접 자동화 공백은 후속 위험으로 분리한다.

### MCP Play

- Play 전 `RatHostPrototype` active·clean, Console Error/Warning 0.
- `SessionMode=RatHost`, `CameraMode=QuarterView`, `Camera.main=IsometricCamera`와 컨트롤러 부착 Camera 동일.
- `GameViewFrameCamera`는 enabled·untagged·별도 카메라이며 culling mask 0.
- RatHost·RatVisual·HUD·WorldPixelOutput 활성, RatVisual ground clearance `0.005000`.
- MainCamera target texture와 RawImage texture가 `RatPixelTrial960x540`로 일치하고 RT는 960×540.
- 카메라 추적 표본 오차 `0.006300`, RatVisual viewport `x=0.500005`, `y=0.437360`.
- Play 중 Console Error/Warning 0.
- Stop 후 Edit·비컴파일·비업데이트, 활성 씬 clean, Console Error/Warning 0.
- 상세: `artifacts/mcp-play-after-fix.md`.

### 수정 후 전후 경계

- 씬 SHA-256: `68C222F449C530B54E5319BD11D94C7E3851161906ED9C19CD6F2FC073C88F02` 유지.
- ProjectSettings SHA-256: `008078ADBB3A01264F4C097558F5983453A93F6254E600AB2776D269DD8201D9` 유지.
- 수정 후 테스트 파일 SHA-256: `3F2FDE756CA8ED64ED827EFDF5A159B42FCEE5A09F49329F2F6213A383173BC3` 유지.
- `Builds/` 변경 0, staged 변경 0.
- 기존 `ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 변경과 `_workspace/previews/`는 보존했다.

## 최종 QA 완료 판단

`완료 가능 — 자동 기술 게이트`

담당 구현 에이전트의 단일 테스트 파일 수정 뒤 독립 전체 EditMode 101/101, MCP Play 런타임 연결, Console 0, Stop/Edit 복귀, 씬·ProjectSettings·Builds 불변성을 확인했다. 사용자 시각 수용과 자연 경계도 엄격 검증 차단 상태는 별도이며 이번 판정으로 대체하지 않는다.

## 프로젝트 총괄 관리자 판정

`내부 승인 가능`

총괄은 전체 EditMode 101/101, MCP Play·Console 0, Stop/Edit clean, 씬·ProjectSettings·Builds 비변경과 단일 테스트 계약 최소 수정 범위를 확인했다. v4 직접 규격 자동화 공백과 사용자 시각 수용은 별도 위험으로 유지한다.

## 2026-07-24 — 완료 보관·상태판 QA

### 보관 구조

- active 원본 `_workspace/active/2026-07-24-rat-visual-camera-editmode-regression/`: 없음.
- completed 경로 `_workspace/completed/2026-07-24-2026-07-24-rat-visual-camera-editmode-regression/`: 존재.
- 필수 문서 `task.md`, `handoff.md`, `verification.md`, `work-log.md`, `agent-activity.md`, `director-review.md`, `completion-report.md`: 7/7 존재·비어 있지 않음.
- `artifacts/`: 파일 8개 존재.

### 증적 무결성

- 초기 XML: `90DD2993F52BD3BA6AB9A2D3627ABFD86363C88FAE1444144892C7A41D0FFDA4`
- 초기 로그: `59E1D6FDF8086AF21CA457C24DBC6976EE3C65ECD0BFCA24757399F057817D63`
- 초기 요약: `708478C63A6851CA25B9148C6010AFD016DF9839294061CE2011A5F3DFCA39BB`
- 수정 후 XML: `FB20ABC4DE772EBD5605691A87E6E9C53DC4F1A2460D7B5C2983F146D336F105`
- 수정 후 로그: `7C87D72222FC29DBC34348C8CEA9D819F5EBF8824432EE930827544010635933`
- 수정 후 요약: `B40EC12753D068CDE745A771C0E9D0F2EABE694098D58CA3218C20E9B5C1C758`
- MCP Play 요약: `D2D1703249092A00F851ED21DFF0048E6A919272D5D6CD1DB4355C6B44626DE2`
- artifacts README: `3BC2A778F1AD5176C30F5F1B9B4DDDAB393B1C03ACFEF30DE343C784E15A3A49`
- 초기 XML은 `101 total / 99 passed / 2 failed`, 수정 후 XML은 `101 total / 101 passed / 0 failed / 0 skipped / 0 inconclusive / 6.3731956s`로 기록과 일치한다.
- MCP Play·Stop/Edit clean과 총괄 `내부 승인 가능` 기록은 `mcp-play-after-fix.md`, `director-review.md`, `completion-report.md`와 일치한다.

### 포인터·상태판·관련 작업

- 공유 상태판의 현재 진행 중 표에는 본 완료 작업 행이 없고, 최근 작업 요약은 실제 completed 경로를 가리킨다.
- 자연 경계도 엄격 검증은 현재 진행 중의 active 차단 작업으로만 유지하며 다음 작업 후보에는 중복하지 않는다.
- `CURRENT.md`는 v5b 사용자 화면 수용을 현재 작업으로 가리키고 자연 경계도 차단과 재개 조건을 분리한다.
- v3·v4·v5b의 최신 task·handoff·verification 상태는 전체 TestRunner 잔여를 해소하고 사용자 WASD 체감·화면 수용을 유지한다. 과거 시점의 TestRunner 대기 문장은 이력으로 남아 있으나 각 문서의 최신 섹션에서 `101/101` 종결로 명시됐다.
- v4 `128×128 / PPU64 / world width 2` 직접 EditMode 자동화 공백은 completed 기록, 상태판, v4 active 기록에서 동일하게 유지된다.

### Git·범위

- staged 변경 0, `Builds/` 변경 0.
- Unity tracked 변경은 본 작업의 테스트 파일과 기존 범위 밖 `ProjectSettings.asset`뿐이다.
- `ProjectSettings.asset` diff는 `Standalone` define에 `APP_UI_EDITOR_ONLY`를 보존한 한 줄이며 수정하지 않았다.
- `_workspace/previews/3d-vs-2_5d/index.html`은 기존 untracked 범위 밖 파일로 보존했다.
- 그 밖의 tracked 변경은 관련 v3·v4·v5b 상태 문서 9개, `CURRENT.md`, 공유 상태판이고, untracked 변경은 completed 패킷 15개와 previews 1개로 예상 범위와 일치한다.
- 테스트 파일과 tracked 상태 문서의 `git diff --check`는 통과했다.
- untracked 보관본은 원본 `artifacts/unity-editmode.log`의 실패 메시지·스택 4행에만 trailing whitespace가 있다. 원본 증적을 수정하지 않고 예외로 분리했으며 나머지 보관 파일에는 trailing whitespace가 없다.

### 판정

`완료 경로 적합`

완료 보관 구조, 원본 증적, 총괄 판정, 상태판·세션 포인터, 관련 active 작업의 최신 경계와 Git 범위가 일치한다. Unity·TestRunner·Play·코드·테스트·씬·ProjectSettings·상태판·CURRENT·관련 작업 문서는 변경하지 않았으며 본 QA 기록 두 문서만 갱신했다.
