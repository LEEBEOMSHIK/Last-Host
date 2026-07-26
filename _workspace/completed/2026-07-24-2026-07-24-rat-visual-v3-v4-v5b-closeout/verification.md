# 검증 기록

## 작업 ID

`2026-07-24-rat-visual-v3-v4-v5b-closeout`

## 검증 대상

v5b 방식 수용과 현재 쥐 외형 미승인 경계, v3/v4/v5b 연계 작업의 통합 종결 가능 여부.

## 검증 담당

QA/검증 에이전트

## 검증 주장

사용자의 2026-07-24 결정에 따라 v5b까지 확립한 제작·표시 방식은 쥐 프로토타입 공통 기준으로 수용할 수 있다. 다만 현재 쥐의 체형·색감·얼굴·실루엣·보행 동작은 최종 제품 외형으로 승인하지 않으며 후속 재작업 대상으로 분리한다. 이 경계를 유지하면 v3 Blender/Unity, v4, v5b 시험 4건은 통합 종결 가능하다.

## 실행한 검증

### 문구·승인 경계

- umbrella task·handoff·completion-report·CURRENT·공유 상태판과 네 연계 작업의 최신 task·handoff·verification·completion-report를 대조했다.
- 수용 범위는 저폴리 3D 원본 기반 8방향 프리렌더, 128×128/PPU64/2-unit, 제한 팔레트·이진 알파·무디더, 960×540 Point 출력, RatVisual·카메라 픽셀 스냅으로 일치한다.
- 미승인 범위는 현재 쥐의 체형·색감·얼굴·실루엣·보행 동작과 최종 제품 외형으로 일치한다.
- 네 연계 작업과 umbrella는 모두 active에 있으며 QA·총괄 판정 전 보관 금지 상태다.
- 연계 handoff 상단의 사용자 수용 전 과거 상태와 상태판의 Blender 중복 행은 현재 결정과 불일치해 사실 관계만 최소 정정했다. 과거 실행·실패 이력은 삭제하지 않았다.

### 기술 증거

- 완료된 기술 게이트 원본 XML을 재대조했다: `101 total / 101 passed / 0 failed / 0 skipped / 0 inconclusive`.
- 완료 경로: `_workspace/completed/2026-07-24-2026-07-24-rat-visual-camera-editmode-regression/`.
- 해당 완료 기록의 MCP Play는 RatHost, QuarterView MainCamera, 별도 GameViewFrameCamera, RatVisual, HUD, 960×540 RT, Console 0, Stop/Edit clean과 씬·ProjectSettings·Builds 비변경을 확인한다.
- v3 Blender 출력, Unity WalkTrialV3, v4 출력·WalkTrialV4, v5b 출력·WalkTrialV5B는 각각 PNG 64장으로 실제 존재한다.
- v3/v4/v5b Blender 원본·생성 스크립트·frame map이 실제 존재한다.
- v5b contact sheet SHA-256: `AD7844E3E0A8494DAE6271A6FB588E7C0FBC185215A081495BAD15AC1DAEC799`.
- v5b 960×540 런타임 월드 캡처 SHA-256: `0A353FFDB9375C9082DD96A1983F11E1F4BF127ECF069A763AEEE51F3CC22F45`.
- 최신 v5b 캡처 종료 직후의 RenderTexture Warning 1건은 MCP CameraCapture 종료 경로의 도구 유발 기록으로 분리돼 있다. 완료된 독립 기술 게이트의 Play 전·중·후 Console 0 근거를 대체하거나 숨기지 않는다.

### 문서·Git 경계

- umbrella와 네 연계 후보는 필수 후보 문서 `task.md`, `handoff.md`, `verification.md`, `work-log.md`, `agent-activity.md`, `completion-report.md`를 각각 6/6 보유하며 모두 비어 있지 않다.
- staged 변경 0, `Builds/` 변경 0.
- Unity tracked 변경은 완료 기술 게이트의 테스트 계약 파일과 기존 범위 밖 `ProjectSettings.asset`뿐이다.
- `ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY`와 `_workspace/previews/`를 보존했다.
- `git diff --check` 통과.

## 결과

- 제작·표시 방식 수용과 현재 외형 최종 미승인 경계: 통과.
- 네 연계 시험의 기술 증거 존재·교차참조: 통과.
- active 유지·필수 완료 후보 문서·보관 금지 경계: 통과.
- 범위 밖 변경 제외와 Git 형식: 통과.

## 남은 위험

- v4 `128×128 / PPU64 / world width 2`를 직접 명명해 검사하는 EditMode 자동화는 없다. 선행 MCP 증거는 유지하되 직접 자동화 통과로 표현하지 않는다.
- 현재 쥐 외형과 보행 동작의 최종 품질은 미승인이다.
- 물리 키보드 장시간 WASD 체감과 OS Game 창 직접 캡처는 본 방식 종결 근거가 아니다.
- Computer Use Game 창 캡처의 `SetIsBorderRequired 0x80004002` 제한은 별도 유지한다.
- 후속 외형 재작업의 체형·색감·얼굴·실루엣·보행 방향은 별도 사용자 승인 전 시작할 수 없다.

## 완료 판단

`완료 가능 — 제작·표시 방식 통합 종결 범위`

현재 외형을 최종 승인하지 않는 경계에서 v3 Blender/Unity, v4, v5b 시각 시험 4건을 종결할 수 있다. 본 QA 판정만으로 폴더를 이동하거나 보관하지 않으며, 프로젝트 총괄 관리자 `내부 승인 가능` 판정 뒤 문서/릴리즈 에이전트가 완료 경로를 다시 대조해야 한다.

## 2026-07-24 — 총괄 수정 조건 재QA

### 공식 문서 대조

- `graphics-direction-management.md`는 v4의 `128×128 / PPU64 / Point / Mipmap Off / Uncompressed / Clamp`를 해상도·Import 기반으로 계승하고, v5b의 최대 32색·이진 알파·무디더·`960×540` Point 월드 출력·카메라 출력 스냅을 현재 쥐 숙주 프로토타입 공통 제작·표시 기준으로 기록한다.
- `pixel-lowpoly-3d-production-guide.md`도 같은 v4 기반 계승과 v5b 공통 방식을 명시하며, 일반 해상도 후보와 별도로 `960×540`을 현재 적용값으로 구분한다.
- 두 문서는 현재 쥐의 체형·색감·얼굴·실루엣·보행을 최종 미승인·후속 재작업 대상으로 유지한다.
- 두 문서는 바이러스·백혈구·다른 캐릭터·게임플레이 오브젝트·환경·HUD로 자동 확대하지 않으며 대상별 시각 검증과 사용자 승인을 별도로 요구한다.
- 따라서 총괄이 지적한 `v5b 승인 대기 / v4만 공통 기본`의 공식 문서 불일치는 해소됐다.

### 저장 설정·적용값 대조

- `RatHostPrototype.unity`: `RatVisual.enablePixelSnap: 0`.
- `RatHostPrototype.unity`: `enableQuarterViewOutputPixelSnap: 1`, 기준 높이 `540`.
- `RatPixelTrial960x540.renderTexture`: `m_Width: 960`, `m_Height: 540`, `m_AntiAliasing: 1`.
- 공식 문서는 RatVisual 루트 앵커 스냅을 검증된 선택 기능이지만 현재는 끈 값으로, 카메라 출력 스냅을 현재 켠 값으로 구분한다. 두 스냅의 중복 양자화를 필수화하지 않는다.

### umbrella 기록·기존 QA 유지

- `completion-report.md`와 `work-log.md`는 공식 문서 수정 대응 범위, v5b 방식 수용, 현재 외형 미승인, 다른 대상 별도 승인, 저장 씬 스냅 값과 `960×540` 적용값을 동일하게 기록한다.
- 기존 QA의 전체 EditMode `101/101`, MCP Play·960×540 RT·Console 0·Stop/Edit clean, v3/v4/v5b 산출물·contact sheet·런타임 캡처 증거는 변경되지 않았다.
- umbrella와 네 연계 작업은 active 상태이며 폴더 이동·보관을 수행하지 않았다.
- staged 변경 0, `Builds/` 변경 0, Unity tracked 변경은 기존 테스트 계약 파일과 범위 밖 `APP_UI_EDITOR_ONLY` ProjectSettings뿐이다. `_workspace/previews/`도 보존됐다.
- `git diff --check` 통과.

### 재QA 판정

`완료 가능 — 총괄 수정 조건 해소`

공식 그래픽 기준 두 문서가 사용자 최신 결정과 실제 저장 설정에 맞게 동기화됐다. 현재 외형을 최종 승인하지 않는 조건에서 제작·표시 방식 통합 종결 게이트는 다시 완료 가능하며, 실제 보관은 프로젝트 총괄 관리자 재판정 뒤에만 진행한다.

## 2026-07-24 — 완료 보관 최종 경로 대조

### 경로·필수 문서

- `_workspace/completed/2026-07-24-2026-07-20-rat-walk-animation-blender/`: 존재, active 원본 없음, 필수 문서 6/6.
- `_workspace/completed/2026-07-24-2026-07-20-rat-walk-unity-visual-trial/`: 존재, active 원본 없음, 필수 문서 6/6.
- `_workspace/completed/2026-07-24-2026-07-21-character-sprite-resolution-standard/`: 존재, active 원본 없음, 필수 문서 6/6.
- `_workspace/completed/2026-07-24-2026-07-21-rat-pixel-treatment-v5/`: 존재, active 원본 없음, 필수 문서 6/6.
- `_workspace/completed/2026-07-24-2026-07-24-rat-visual-v3-v4-v5b-closeout/`: 존재, active 원본 없음, 필수 문서 6/6.
- 필수 문서는 각 경로의 `task.md`, `handoff.md`, `verification.md`, `work-log.md`, `agent-activity.md`, `completion-report.md`이며 모두 비어 있지 않다.

### 상태판·세션 포인터

- 공유 상태판의 현재 진행 중 표에서는 위 다섯 작업이 제거됐고, umbrella 최근 완료 행은 실제 completed 경로를 가리킨다.
- `CURRENT.md`도 주 작업 없음, umbrella completed 실제 경로, 제작·표시 방식 수용과 현재 외형·보행 최종 미승인을 일치하게 기록한다.
- 다음 사용자 결정 후보는 `쥐 최종 외형 재작업 방향·승인 브리프`이며, 승인 전 새 쥐 아트 생성·Unity 구현 금지를 상태판과 `CURRENT.md`가 동일하게 유지한다.
- 다른 캐릭터·오브젝트·환경 적용은 별도 승인 대상이며 자연 경계도 active 차단 작업도 분리돼 있다.

### Git·보존 경계

- Git은 보관 이동을 unstaged active 삭제와 completed untracked 경로로 표시한다. 커밋 미요청 상태에서 예상되는 이동 전 표현이며 source 부재·destination 존재와 일치한다.
- staged 변경 0, `Builds/` 변경 0, `git diff --check` 통과.
- 기존 `RatHostPrototypeCoreTests.cs` 테스트 계약 변경이 유지된다.
- `ProjectSettings.asset`의 범위 밖 변경은 `APP_UI_EDITOR_ONLY` define 한 줄이며 보존됐다.
- `_workspace/previews/3d-vs-2_5d/index.html`과 completed EditMode 기술 게이트 경로를 보존했다.
- Unity·씬·ProjectSettings·아트·Builds를 변경하거나 폴더를 추가 이동하지 않았고 커밋도 수행하지 않았다.

### 최종 판정

`완료 경로 적합`

다섯 작업의 실제 completed 보관, active 원본 제거, 필수 문서, 상태판·CURRENT의 사용자 승인 경계와 후속 승인 브리프, 기존 기술·범위 밖 변경 보존이 서로 일치한다.
