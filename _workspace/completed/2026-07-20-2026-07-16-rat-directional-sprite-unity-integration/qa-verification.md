# 독립 QA 검증

## 검증 대상

- 작업: `2026-07-16-rat-directional-sprite-unity-integration`
- 완료 주장: 기존 3D 게임플레이 루트를 유지하면서 캡슐 `RatVisual`을 카메라 기준 정지 8방향 Sprite 표시로 교체했다.
- 검증일: 2026-07-16 KST

## 작업영역 대조

- 구현 파일:
  - `UnityProject/Assets/_Project/Scripts/Host/RatDirectionQuantizer.cs`
  - `UnityProject/Assets/_Project/Scripts/Host/RatDirectionalSpriteView.cs`
  - `UnityProject/Assets/_Project/Editor/RatHostPrototypeSceneBuilder.cs`
  - `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
  - `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
  - `UnityProject/Assets/_Project/Sprites/Characters/Rat/TrialV1/`
- 금지 범위 대조:
  - `RatHostController`, 이동·충돌·속도·카메라·URP·Build Settings·패키지는 이번 구현 diff에 포함되지 않았다.
  - QA 중 발견한 `ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 1줄 자동 변경은 구현 담당이 최소 복구했다.
  - 최종 `git diff -- UnityProject/ProjectSettings/ProjectSettings.asset`와 해당 경로 `git status --short`는 모두 비어 있으며, 2초 후 재대조에서도 diff가 재발하지 않았다.
  - 자연 경계도 엄격 검증 차단 판정과 `.codex/config.toml`, `_workspace/previews/`는 이 작업에서 수정하지 않았다.

## 독립 정적 검증

### 씬과 코드

- 씬 YAML의 `RatVisual`은 `Transform`, `SpriteRenderer`, `RatDirectionalSpriteView` 3개 컴포넌트로 구성된다.
- `RatVisual`의 기존 `MeshRenderer`와 `MeshFilter` 블록이 제거된 diff를 확인했다. 씬의 다른 오브젝트가 사용하는 메시 컴포넌트는 대상 밖이다.
- 8개 직렬화 참조는 `S, SW, W, NW, N, NE, E, SE` 순서와 각 파일 GUID에 모두 연결돼 있다.
- `RatDirectionalSpriteView`는 `RatHostController.CurrentResolvedMoveDirection`을 읽기만 하며, 유효 입력일 때만 방향을 갱신하므로 정지 시 마지막 방향을 유지한다.
- 뷰는 `LateUpdate`에서 카메라 회전을 적용하며, 씬 빌더도 동일한 SpriteRenderer·8방향 바인딩을 재현한다.
- 추가 EditMode 테스트는 8방향 양자화 전부와 정지 방향 유지 케이스를 직접 검증한다.
- 대상 구현 diff의 `git diff --check`는 오류가 없다.

### Sprite 원본과 Import

- 완료 시험 에셋 원본과 Unity 복사본 8개 PNG의 SHA-256을 파일별로 대조했고 모두 일치했다.
- 8개 `.png.meta` 공통값을 대조했다.
  - Sprite Single: `textureType: 8`, `spriteMode: 1`
  - PPU: `32`
  - Pivot: `(0.5, 0.25)`
  - Point: `filterMode: 0`
  - Mipmap off: `enableMipMap: 0`
  - Alpha transparency: `alphaIsTransparency: 1`
  - Clamp: `wrapU: 1`, `wrapV: 1`
  - 기본 Import 압축: `textureCompression: 0`; 플랫폼 항목은 `overridden: 0`

## Unity 상태와 동적 증거

### 독립 QA가 확보한 초기 상태

- `Unity_ManageEditor(GetState)`: Unity `6000.4.6f1`, Play 아님, 컴파일 아님, 업데이트 아님.
- `Unity_ManageScene(GetActive)`: `Assets/_Project/Scenes/RatHostPrototype.unity`, `isDirty=false`.
- `Unity_ReadConsole(Error, Warning)`: 0건.

### 구현 담당 증거 대조

- `C:/Users/User/AppData/LocalLow/DefaultCompany/Last Host/TestResults.xml`을 독립 열람했다.
- 결과 파일 시각: 2026-07-16 15:12:30 KST.
- 결과: EditMode `92 passed / 0 failed / 0 skipped / 0 inconclusive`, duration `4.5857212`초.
- 구현 담당 Play 기록은 다음을 포함한다.
  - `RatVisual` SpriteRenderer와 `RatDirectionalSpriteView` 활성
  - MeshRenderer/MeshFilter 부재
  - 8방향 직접 바인딩 일치
  - `RefreshDirection` 대체 경로로 방향 변경과 정지 유지 확인
  - 카메라 facing 오차 `0.000°`
  - Play 종료 후 씬 clean, Console Error/Warning 0건
- 위 Play 기록은 실제 WASD 수신 증명이 아니라 MCP 직접 상태 전환 대체 검증이다.

### 독립 동적 재실행 제한

- 독립 QA가 Test Runner API 전체 재실행을 요청했으나 완료 응답과 QA 완료 마커를 확보하지 못했다.
- 이후 `Unity_ManageEditor(GetState)` 재확인도 장시간 응답하지 않아 호출을 중단했다. 같은 점유 지연이 반복되어 추가 MCP 호출은 하지 않았다.
- 따라서 독립 QA가 별도로 실행한 92개 테스트 완료 결과와 별도 Play 방향 변경은 주장하지 않는다.
- 이 제한은 제품 실패 증거가 아니라 Unity MCP 검증 인프라의 점유·응답 지연이다. 구현 담당 결과 파일, Play 기록, 독립 정적 대조, QA 시작 시 정상 Unity 상태를 구분해 근거로 사용했다.

## 수용 기준 판정

| 항목 | 판정 | 근거 |
| --- | --- | --- |
| 3D 게임플레이 루트 유지 | 통과 | 컨트롤러·충돌·속도 변경 없음, 표시 자식만 교체 |
| 캡슐 메시 제거와 Sprite 표시 | 통과 | 씬 YAML과 구현 담당 Play 상태 일치 |
| 카메라 기준 8방향 대응 | 통과 | 코드·직렬화 바인딩·8방향 테스트 결과 대조 |
| 정지 시 마지막 방향 유지 | 통과 | 코드 경계와 전용 테스트, 구현 담당 대체 Play 기록 |
| 카메라 facing과 접지 | 통과 | 회전 코드, pivot, 구현 담당 Play `0.000°` 및 캡처 기록 |
| Import 조건 | 통과 | 8개 meta와 원본 SHA 독립 대조 |
| 컴파일·EditMode | 통과 | 저장된 TestResults.xml 92/92, 실패 0 |
| Play 종료·콘솔·씬 clean | 통과 | QA 시작 시 정상 상태와 구현 담당 종료 기록 대조 |
| 금지 범위 | 통과 | ProjectSettings 자동 diff 복구 후 최종 diff/status 0 |

## 미검증 항목과 남은 위험

- 독립 QA 세션의 Test Runner 완료 콜백과 별도 Play 재실행은 MCP 점유 지연으로 확보하지 못했다.
- 실제 WASD 입력 수신과 방향 전환 체감, 방향 경계 프레임 튐, 카메라 모드 전환 체감은 사용자 Play 확인 대상이다.
- 현재 에셋은 정지 1프레임 시험본이므로 걷기·공격·피격 애니메이션 품질은 이 작업 범위가 아니다.

## 완료 판단

- 판정: **완료 가능**
- 수정 필요 사항: 없음.
- 근거: 독립 정적 대조에서 구현·씬·Import·원본 일치·금지 범위 위반이 없고, QA가 발견한 ProjectSettings 자동 변경은 최종 diff/status 0으로 복구됐다. 저장된 92/92 결과와 구현 담당 Play 증거가 정적 결과와 일치한다. 독립 동적 재실행 미확보는 제품 결함이 아닌 MCP 응답 지연으로 분리 기록했으며, 사용자 최종 Play 확인을 대체하지 않는다.

## 상태판 동기화 게이트 재대조

### 대조 대상

- `docs/project-handoff/current-task-board.md`
- `_workspace/active/CURRENT.md`
- 이 작업의 `task.md`, `handoff.md`
- 총괄 1차 `completion-report.md`의 상태판 수정 요구
- 실제 active/completed 경로와 관련 Git 상태

### 명령·결과·해석

명령:

- 네 문서에서 상태, 보류, 금지 범위, WASD 검증 경계를 `rg`로 줄 단위 대조.
- 자연 경계도 엄격 검증 active 패킷의 QA·총괄 판정과 실제 경로 대조.
- 관련 Unity 구현 파일, `ProjectSettings`, `Packages`, `.codex/config.toml`, `_workspace/previews/`의 `git status --short` 대조.
- `git diff -- UnityProject/ProjectSettings/ProjectSettings.asset` 확인.
- 현재 작업의 active 경로와 동일 작업 completed 경로 존재 여부 확인.

결과:

1. 자연 경계도 엄격 검증은 실제 active 경로에 남아 있고, 상태판과 `CURRENT.md`가 QA `차단`, 총괄 `보류`, Computer Use 복구 또는 사용자 수동 증거라는 재개 조건을 유지한다.
2. 사용자 수동 플레이 체감은 상태판의 별도 보류 항목으로 유지되며 `manual-play-checklist.md`도 존재한다.
3. 쥐 Sprite 시험 반입은 네 문서 모두 `구현·QA 완료 → 상태판 재대조·총괄 재검토 → 사용자 WASD 확인` 순서로 기록한다.
4. 구현 담당의 방향 변경은 직접 상태 전환 대체 검증이고 실제 WASD 수신 증명이 아니라는 경계, 방향 전환·프레임 경계·정지 유지 체감은 사용자가 확인한다는 경계가 일치한다.
5. `.codex/config.toml`과 `_workspace/previews/`는 실제로 각각 수정·미추적 상태지만 작업 범위 밖 로컬 변경으로 분리돼 있다.
6. `ProjectSettings.asset`의 diff와 경로 status는 0이다. `ProjectSettings`, `Packages`, Build Settings의 순변경은 확인되지 않았다.
7. 구현 파일 Git 상태는 상태판 설명과 일치한다.
   - 수정: 씬 빌더, `RatHostPrototype.unity`, EditMode 테스트.
   - 신규: 방향 양자화·Sprite View 스크립트와 meta, Sprite 폴더와 meta.
8. 현재 작업은 `_workspace/active/2026-07-16-rat-directional-sprite-unity-integration/`에 남아 있고 동일 작업의 completed 경로는 0개다. 사용자 수용 전 완료 보관 금지가 지켜졌다.
9. `task.md`와 `handoff.md`는 독립 QA 대기 표현을 제거하고 QA `완료 가능`, 총괄 재검토 대기, 사용자 확인 전 보관 금지로 동기화했다.

해석:

- 총괄 1차 `수정 필요`의 원인이었던 현황판·세션 포인터 불일치는 해소됐다.
- 기능 완료·실제 WASD 통과·자연 경계도 차단 해제처럼 증거보다 앞선 표현은 없다.
- 상태판 동기화 게이트에 추가 수정이 필요하지 않다.

### 재대조 판정

- 상태판 동기화 게이트: **통과**
- 수정 필요 사항: 없음.
- 다음 게이트: 프로젝트 총괄 관리자 재검토. 총괄 재판정 뒤에도 사용자 실제 Game View WASD 확인 전에는 완료 보관하지 않는다.
- 2026-07-16 15:41 KST 최종 문구 재대조: **수정 필요** — board·`CURRENT.md`·`task.md`는 `구현·QA·총괄 확인 완료 — 사용자 WASD 확인 대기`로 일치하지만 `handoff.md`에는 같은 의미의 QA·총괄 판정과 사용자 확인 대기만 있고 지정된 통합 상태 문구가 없다. 작업은 active, 동일 completed 경로 0, 사용자 수용 전 완료 보관·커밋 완료 보고 금지, `git diff --check` 통과, ProjectSettings diff/status 0은 모두 정상이다.
- 2026-07-16 15:42 KST 최종 재대조: **통과** — `handoff.md` 보강 후 board·`CURRENT.md`·`task.md`·`handoff.md` 네 문서가 모두 `구현·QA·총괄 확인 완료 — 사용자 WASD 확인 대기`로 일치하며, `git diff --check` 통과와 ProjectSettings diff/status 0을 재확인했다.

## 사용자 접지 피드백 수정 독립 QA — 2026-07-16

### 검증 대상

- 사용자 증상: 오염 구역 통과 시 쥐 스프라이트 하단과 오염 표면이 겹쳐 보임.
- 완료 주장: CharacterController·위험 trigger·게임플레이 위치를 바꾸지 않고 `RatVisual` pivot y만 현재 보이는 표면 top + `0.005`에 맞춘다.
- 기대 수치:
  - 일반 바닥: surface `-0.0200` → pivot `-0.0150`, clearance `0.0050`
  - 오염 trigger: surface `0.0700` → pivot `0.0750`, clearance `0.0050`

### 독립 QA가 직접 확보한 근거

- 코드·씬·테스트 diff를 독립 대조했다.
- Unity MCP 초기 상태는 Play 아님, 컴파일 아님, 업데이트 아님이었다.
- 초기 활성 씬은 `RatHostPrototype`, `isDirty=false`였다.
- 초기 Unity Console Error/Warning은 0건이었다.
- 두 캡처 `rat-grounding-floor.png`, `rat-grounding-toxic.png`를 원본 해상도로 직접 열어 확인했다.
  - 일반 바닥: 쥐 하단이 바닥을 뚫지 않고 눈에 띄게 뜨지 않는다.
  - 오염 표면: 쥐 하단이 녹색 표면 위에 놓이며 기존 관통이 보이지 않고 과도한 부유도 없다.
- `git diff --check`는 통과했다.
- QA 대조 시점에 게임플레이·위험 trigger·CharacterController·카메라·ProjectSettings·Build Settings·Packages 대상 diff/status는 0이었다.

### 정적 구현 대조

- `LateUpdate` 순서는 방향 갱신 → 접지 갱신 → 카메라 facing이며, 접지 수정은 `RatVisual` transform의 월드 y만 바꾼다.
- `Physics.RaycastNonAlloc`과 `QueryTriggerInteraction.Collide`를 사용해 오염 trigger를 포함한다.
- hit collider가 `RatHost` 자신이거나 자식이면 후보에서 제외하므로 자기 CharacterController가 접지면으로 선택되지 않는다.
- collider GameObject에 활성 Renderer가 없는 trigger는 `HasVisibleSurface=false`로 제외한다.
- 상향 표면은 `normalY >= 0.5`만 허용하고, 숙주 y 기준 `maxSurfaceRise=0.2`, `maxSurfaceDrop=0.5` 안에서 가장 높은 표면을 선택한다.
- 경계 조건은 `< minHeight`, `> maxHeight`를 제외하므로 정확히 `-0.5`, `+0.2`인 표면은 포함된다. 전용 경계값 단독 assertion은 없지만 선택기 조건을 정적으로 대조했다.
- 씬 빌더와 현재 씬의 probe/clearance 값이 `1.0 / 1.6 / 0.2 / 0.5 / 0.5 / 0.005`로 일치한다.
- EditMode 테스트에는 다음 회귀가 존재한다.
  - 8방향 양자화 전체
  - idle 마지막 방향 유지
  - 보이는 비자기 최상단 표면 선택
  - trigger 포함, 자기 CharacterController 제외, 오염 `0.07 → 0.075`와 일반 바닥 `-0.02 → -0.015`

### 구현 담당 동적 증거 대조

아래는 독립 QA 재실행 결과가 아니라 구현 담당이 남긴 저장 결과·로그·Play 기록이다.

- 저장된 EditMode 결과와 Unity 완료 마커: `94 passed / 0 failed / 0 skipped / 0 inconclusive`, duration `4.6208452`초.
- 결과 XML에서 접지 통합 테스트, 접지 선택기 테스트, 8방향 테스트, idle 유지 테스트가 각각 `Passed`임을 대조했다.
- Play 일반 바닥: `HasGroundSurface=true`, surface `-0.0200`, pivot `-0.0150`, clearance `0.0050`.
- Play 오염 trigger: `HasGroundSurface=true`, surface `0.0700`, pivot `0.0750`, clearance `0.0050`.
- 방향 회귀: 8개 바인딩 일치, idle 유지 `true`.
- 구현 담당 종료 기록: Play 종료 성공, 씬 clean, Console Error/Warning 0건.

### 독립 동적 재실행 제한

- 독립 QA의 전체 EditMode 재실행 요청은 완료 마커 없이 장시간 점유됐고, 후속 `GetState`도 응답하지 않았다.
- 새 검증 턴의 상태 조회도 즉시 응답하지 않아 추가 Unity MCP 호출을 중단했다.
- 따라서 독립 QA가 94개 테스트, 일반 바닥·오염 trigger Play 수치, Play 종료 clean·최종 콘솔을 별도로 재실행했다고 주장하지 않는다.
- 이 제한은 제품 실패 증거가 아니라 Unity MCP 점유·응답 지연이다. 독립 정적·초기 상태·캡처 관찰과 구현 담당 동적 증거를 구분해 판정했다.

### 접지 수정 QA 판정

- 판정: **완료 가능**
- 수정 필요 사항: 없음.
- 근거: 사용자 증상의 구조 원인과 표시 전용 보정이 일치하고, trigger 포함·자기 CharacterController 제외·Renderer 없는 trigger 제외·rise/drop/normal 제한이 코드와 씬에 반영됐다. 저장된 94/94와 구현 Play 수치가 기대값과 정확히 일치하며, 독립 캡처 관찰에서도 표면 교차나 과도한 부유가 보이지 않는다. 금지 범위 순변경도 발견되지 않았다.
- 남은 위험: 독립 MCP Play 수치 재현과 종료 콘솔은 점유 지연으로 미확보다. 표면 경계에서 약 `0.09` 높이 전환의 실제 이동 체감과 WASD 방향 체감은 사용자 Game View 재확인 대상이다.

## 접지 수정 상태판 재대조 — 2026-07-16

- 대조 대상: `current-task-board.md`, `CURRENT.md`, `task.md`, `handoff.md`, `verification.md` 상단, 실제 Git·active/completed 경로.
- 공통 상태: board·`CURRENT.md`·`task.md`·`handoff.md`는 모두 `접지 수정 구현·QA 완료 — 총괄 상태판 재검토 후 사용자 확인 대기`와 동등하게 일치한다.
- board: 일반 `-0.02→-0.015`, 오염 `0.07→0.075`, 94/94, QA `완료 가능`, 독립 MCP 점유 제한, 사용자 경계 이동·WASD 대기를 모두 기록했다.
- handoff: 접지 테스트 포함 94/94, QA `완료 가능`, 독립 MCP 제한, 사용자 경계 이동·WASD·정지 유지 확인 대기, 사용자 수용 전 완료 보관 금지를 기록했다.
- task: 접지 수정의 사용자 증상·구조 원인·표시 전용 수정 원칙과 추가 수용 기준을 유지한다.
- verification: 상단 독립 QA `완료 가능`과 MCP 점유 제한은 최신 상태지만, 상단 EditMode 결과가 접지 수정 전 `92 passed`로 남아 있다. 같은 문서의 접지 수정 절에는 최신 `94 passed`와 두 표면 수치가 있다.
- `CURRENT.md`: 상태·MCP 제한·사용자 경계 이동/WASD 대기·active 유지·보관 금지는 맞지만, `마지막 정상 근거`가 접지 수정 전 `92 passed`로 남아 최신 94/94 및 접지 수치를 반영하지 못했다.
- 실제 경로: 작업은 active에 있고 동일 completed 경로는 0개다.
- 범위 밖 로컬 변경: `.codex/config.toml` 수정과 `_workspace/previews/` 미추적은 작업 범위 밖으로 분리돼 있다.
- Git: `git diff --check` 통과, ProjectSettings diff/status 0.
- 판정: **수정 필요**.
- 수정 항목: `CURRENT.md`의 마지막 정상 근거를 접지 포함 94/94와 일반/오염 수치·QA 완료 가능으로 갱신하고, `verification.md` 상단 EditMode 결과를 최신 94/94로 바꾸거나 기존 92개 결과가 접지 수정 전 실행임을 명시한 뒤 최신 접지 결과를 상단에 함께 표시한다.

### 상태판 동기화 해소 재검증 — 2026-07-16 16:46 KST

- 판정: **통과**.
- 해소 확인: `CURRENT.md`의 마지막 정상 근거와 `verification.md` 상단이 접지 포함 `94 passed / 0 failed / 0 skipped / 0 inconclusive`로 갱신됐고, 일반 바닥 `-0.0200→-0.0150`, 오염 표면 `0.0700→0.0750`, 공통 clearance `0.0050` 수치도 최신 근거와 일치한다.
- 다섯 문서 대조: `current-task-board.md`, `CURRENT.md`, `task.md`, `handoff.md`, `verification.md`는 접지 수정 구현·QA 완료, 독립 Unity MCP 재실행 제한, 사용자 경계 이동·WASD 체감 확인 대기, 사용자 수용 전 active 유지·완료 보관 금지 경계를 일관되게 기록한다.
- 저장소 경계: `git diff --check` 통과, `ProjectSettings.asset` diff/status 0, 작업 active 경로 존재, 동일 completed 경로 0개를 확인했다.
- 남은 경계: 이번 재검증은 문서·Git 상태 대조이며 Unity MCP는 호출하지 않았다. 사용자 실제 Game View WASD 방향 전환·표면 경계·정지 유지 체감 확인 전에는 완료 보관과 최종 수용 완료를 주장하지 않는다.

### 총괄 최종 판정 문구 반영 대조 — 2026-07-16 16:49 KST

- 판정: **통과**.
- 상태 문구: `current-task-board.md`, `CURRENT.md`, `task.md`, `handoff.md`가 모두 `접지 수정 구현·QA·총괄 확인 완료 — 사용자 Game View 확인 대기`로 일치한다.
- 검증 근거: 접지 회귀 포함 EditMode `94 passed / 0 failed`, 일반 바닥 `-0.02→-0.015`, 오염 구역 `0.07→0.075`, 공통 clearance `0.005`가 최신 board·세션 포인터·작업 검증 근거와 일치한다.
- 저장소 경계: `git diff --check` 통과, `ProjectSettings.asset` diff/status 0, 작업 active 경로 존재, 동일 completed 경로 0개를 확인했다.
- 검증 제한: 요청에 따라 Unity MCP는 호출하지 않았다. 사용자 Game View의 실제 WASD 방향 전환·경계 이동·정지 유지 체감은 계속 사용자 확인 대기다.

## 사용자 2차 접지 피드백 독립 QA — 2026-07-20

- 정적 대조: resolver는 `candidate.IsTrigger`를 제외한다. 위험 trigger는 위치 `(-0.7, 0.03, 1.35)`, 크기 `(2.6, 0.08, 1.5)`, trigger/Rigidbody 구성과 `ImmuneRiskZone`을 유지했으며 `RatHostController` 순변경은 없다.
- QA 발견/복구: 최초 저장 씬에는 `ToxicWaterVisual`의 부모-자식 YAML 링크가 빠져 Unity Hierarchy와 Play에서 누락됐다. `RatHostMode.m_Children`에 `{fileID: 1999987811}` 최소 복구 후 씬 재로드·런타임 Hierarchy에서 존재를 확인했다.
- MCP Play: 오염 구역 안/밖 모두 pivot y `-0.0150`, 선택 표면 y `-0.0200`, y 차이 `0.0000`이었다. 위험 trigger overlap·trigger·기존 위치/크기, 별도 visual top `-0.0180` 및 collider 부재도 확인했다. Play 종료 후 scene clean, Console Error/Warning 0건이다.
- 금지 범위: ProjectSettings의 `APP_UI_EDITOR_ONLY` 재발을 발견해 HEAD 기준 최소 복구 후 경로 한정 diff/status clean을 재대조했다. `git diff --check`도 통과했다.
- 수용 기준 실패: `rat-00-s.png`의 alpha 경계는 64×64/PPU32/pivot y 16px/opaque max y 53px으로, 불투명 하단이 pivot보다 `6px = 0.1875` units 아래다. 현재 pivot·오염 visual top 여유 `0.003`은 실제 쥐 하단 비관통을 증명하지 못하며, Play SpriteRenderer world bounds min도 `-0.4969`다.

### 완료 판단

- 판정: **수정 필요**
- 필요 조치: 모든 방향 Sprite의 실제 불투명 하단을 반영한 공통 표시 접지 오프셋 또는 pivot 보정 후, 일반/오염 양쪽 실제 하단 clearance와 방향 회귀를 재검증한다.
- 미검증: 추가 보정 전후 EditMode 전체 Test Runner는 실행하지 않았다. 위험 노출 로직은 소스 diff·trigger overlap 구조로만 대조했으며, Play의 위험 grace 상태 때문에 노출량 양성값을 확보하지 못했다.

### 공통 visibleFootBottomOffset 재검토

- 구현값 `visibleFootBottomOffset = 0.1875`를 포함해 8개 PNG의 alpha 최하단을 pivot `(0.5, 0.25)`·PPU `32` 기준으로 독립 계산했다.

| 방향 | 필요한 pivot→하단 offset |
| --- | ---: |
| S | `0.1875` |
| SW | `0.15625` |
| W | `0.09375` |
| NW | `0.28125` |
| N | `0.375` |
| NE | `0.40625` |
| E | `0.15625` |
| SE | `0.15625` |

- 최댓값은 `NE = 0.40625`이므로 현 `0.1875` 공통값은 `NW/N/NE`에서 부족하다. 특히 NE 방향은 추가 `0.21875`만큼 표면 아래로 내려간다.
- 판정: **수정 필요**. 공통 보정을 유지하려면 `visibleFootBottomOffset`을 최소 `0.40625`로 올리고 기존 `groundClearance=0.005`를 별도로 더하거나, 방향별 offset으로 실제 하단을 맞춰야 한다. 이 조건이 충족되기 전에는 완료 가능으로 바꾸지 않는다.

### 방향별 보정 재검증

- 구현은 공통값 대신 `S/SW/W/NW/N/NE/E/SE = 0.1875/0.15625/0.09375/0.28125/0.375/0.40625/0.15625/0.15625`의 방향별 alpha 하단 offset을 적용했다. scene builder와 저장 씬 값도 동일하다.
- Unity 표준 스크립트 검증: `RatDirectionalSpriteView.cs`, `RatHostPrototypeCoreTests.cs` 진단 0건.
- Unity MCP Play: 8방향 각각에서 위험 trigger 내부/외부를 직접 대조했다. 모든 방향의 실제 alpha 발 y는 `-0.0150`, clearance는 `0.0050`, 내부·외부 차이는 `0.0000`이었다.
- Play 종료 후 Unity는 비재생·비컴파일, 활성 씬 `RatHostPrototype`은 clean, Console Error/Warning은 0건이었다.
- EditMode 전체 Test Runner는 이번 보정 후 실행하지 않았다. 추가된 8방향 회귀 테스트는 코드·표준 스크립트 검증으로만 대조했다.

### 최종 완료 판단

- 판정: **완료 가능**
- 근거: trigger 접지 제외, 위험 trigger 구조/크기/위치와 `ImmuneRiskZone`·`RatHostController` 비변경, 별도 `ToxicWaterVisual`의 존재·Renderer/collider 경계, 8방향 실제 alpha 발의 공통 y·clearance·구역 내외 동일성을 독립 정적/Play 대조로 확인했다.
- 남은 위험: EditMode 전체 재실행과 실제 사용자의 연속 WASD 조작·Game View 시각 체감은 미검증이다. 위험 노출량의 양성 Play 수치는 시작 grace 상태 때문에 별도 확보하지 못했으며, 구조·소스 비변경·overlap으로만 확인했다.

## 상태판 게이트 재대조 — 2026-07-20

- `task.md`, `handoff.md`, `_workspace/active/CURRENT.md`, `docs/project-handoff/current-task-board.md`는 모두 상태 문구 `사용자 2차 접지 피드백 구현·QA·총괄 확인 완료 — 사용자 Game View 확인 대기`를 기록한다.
- active 작업 패킷은 존재하고 동일 작업 ID의 completed 폴더는 없다. `git diff --check`는 통과했으며 ProjectSettings 순변경도 없다.
- 그러나 `handoff.md`는 같은 문서의 최신 8방향 보정 근거와 충돌하게, 구형 공통 `visibleFootBottomOffset 0.1875`, pivot `0.1725`를 현재값처럼 서술한다. 실제 최종 구현은 방향별 offset이며 QA MCP Play의 공통 결과는 **발 y** `-0.015`, clearance `0.005`이지 pivot y가 동일한 것이 아니다.

### 상태판 게이트 판정

- 판정: **수정 필요**
- 필요 조치: `handoff.md`의 구형 공통 offset/pivot 문장을 방향별 alpha foot offset과 현재 8방향 검증 수치로 교체한 뒤 네 문서를 다시 대조한다. 완료 보관·커밋 완료 보고 금지는 유지한다.

### 상태판 게이트 2차 재대조

- 구형 공통 offset/pivot 문장은 방향별 alpha foot offset과 8방향 MCP Play 수치로 갱신됐다.
- 하지만 `handoff.md`에는 여전히 "Unity 재로드·컴파일·EditMode·Play는 미실행"이라는 구형 문장이 남아, 같은 문서의 실제 QA MCP Play 기록 및 이번 QA 실행 사실과 충돌한다.
- 판정: **수정 필요**. 이번 보정 후 EditMode 전체 Test Runner는 미실행이지만, Unity 재로드·컴파일·MCP Play는 실행됐다는 정확한 검증 경계로 문장을 정정한 뒤 재대조한다.

### 상태판 게이트 최종 재대조

- `handoff.md`의 실행 경계가 "EditMode 전체 재실행은 미실행, Unity 재로드·컴파일·MCP Play는 QA가 실행"으로 정정됐고 최신 QA 근거와 일치한다.
- 네 문서 상태 문구, active 작업 패킷 존재·동일 completed 부재, `git diff --check`, ProjectSettings 순변경 0을 재확인했다.
- 판정: **통과**. 사용자 Game View 확인 전 active 유지·완료 보관 금지 경계도 일관된다.

## 사용자 수용 후 완료 보관 상태판 대조 — 2026-07-20

- 완료 경로 `_workspace/completed/2026-07-20-2026-07-16-rat-directional-sprite-unity-integration/`가 존재하며 `task.md`, `work-log.md`, `handoff.md`, `verification.md`, `qa-verification.md`, `completion-report.md`, `agent-activity.md`와 `artifacts/`를 확인했다.
- 이전 active 경로 `_workspace/active/2026-07-16-rat-directional-sprite-unity-integration/`는 존재하지 않는다. `CURRENT.md`는 현재 별도 차단 작업을 가리키며, 상태판의 진행 중 행에도 보관 작업은 남아 있지 않다.
- `current-task-board.md` 최근 작업 요약의 완료 보관 경로는 실재하며, 이 작업은 다음 후보·보류 항목에 중복되지 않는다. 사용자 수동 체감은 별도 보류 항목으로 남아 있다.
- `git diff --check` 통과, ProjectSettings 순변경 0을 재확인했다. 미커밋 Unity/문서/작업 패킷과 범위 밖 로컬 변경은 상태판 기록과 일치한다.

### 완료 보관 상태판 판정

- 판정: **통과**
- 남은 경계: 완료 이력의 EditMode 전체 재실행·연속 WASD/Game View 체감 미확인은 보존되며, 새 active 작업이나 완료 보관 게이트 위반으로 처리하지 않는다.
