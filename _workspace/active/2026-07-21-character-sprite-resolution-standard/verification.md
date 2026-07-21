# 검증 기록

## 작업 ID

`2026-07-21-character-sprite-resolution-standard`

## 검증 대상

128×128 v4 프리렌더 출력, Unity Import·화면 크기·정지/이동·8방향·쿼터뷰 선명도.

## 현재 상태

v4 Blender 출력과 Unity 반입·활성 씬 연결까지 완료했다. 실제 Play 화면 QA는 대기다.

## 비주얼/테크아트 사전 판정 — 2026-07-21

- 판정: `128×128 / PPU64`는 기존 `64×64 / PPU32`와 같은 2 Unity unit 캔버스 폭을 유지하므로, 월드 표시 크기 보존 조건에 통과한다.
- 승인된 기본 Import: Sprite Single, Custom Pivot `(0.5, 0.25)`, Point, 밉맵 끔, 무압축, Clamp.
- 조건: 실제 실루엣 크기와 선명도는 Blender 카메라·알파 여백·지면 원점, Unity 목표 출력의 정수 배율까지 포함해 Play에서 확인해야 한다. 이 사전 판정은 v4 에셋·Unity 통합·최종 QA 통과를 의미하지 않는다.
- 예외: 피벗·캔버스 구성이 다른 오브젝트, UI, 원거리 환경은 공통 규격을 무조건 적용하지 않고 별도 기록·비교한다.

## Unity 씬/통합 검증 — 2026-07-21

- 원본 `renders-v4`와 `WalkTrialV4` 반입본: PNG 64/64, SHA-256 일치 64/64, 128×128 확인 64/64.
- Unity Importer: Sprite Single, PPU64, Custom Pivot `(0.5, 0.25)`, Point, 밉맵 없음, Uncompressed, Clamp, NPOT None을 64/64에 적용·재대조했다.
- 활성 `RatVisual/RatDirectionalSpriteView`: 남·남서·서·북서·북·북동·동·남동의 8개 걷기 배열 모두 v4 `f01→f08` 8장으로 연결됐고, `walkFramesPerSecond=8`이다.
- 월드 폭: 대표 Sprite MCP 확인 결과 `128px / PPU64 = 2.000 Unity units`; 기존 v3의 `64px / PPU32 = 2.000 Unity units`와 동일하다.
- 미실행: 활성 씬이 dirty여서 사용자 지시대로 씬 Save·재로드·Play·카메라/접지/콘솔 검증을 실행하지 않았다. QA가 현재 메모리 씬에서 독립 Play로 수행해야 한다.
- 완료 판단: `통합 완료 / QA 미승인`.

## 독립 QA Play 검증 — 2026-07-21

### 검증 대상

`WalkTrialV4` 128px/PPU64 반입본, `RatVisual` 8방향 연결, 정지·이동 전환, 월드 표시 폭, 쿼터뷰 화면.

### 실행한 검증

- 원본 `renders-v4`와 Unity `WalkTrialV4`를 독립 SHA-256로 대조했다. 각각 64장이고 64/64 일치했으며, 원본 64/64가 `128×128`이었다.
- dirty 활성 씬을 저장·재로드하지 않고 Unity MCP Play → Pause → Stop을 실행했다.
- Play 런타임에서 v4 64장을 전수 조회했다. `128×128`, Sprite Single, PPU64, Custom Pivot `(0.5, 0.25)`, Point, 밉맵 없음, Uncompressed, Clamp, NPOT None의 Importer 위반은 `0/64`였다.
- `RatVisual`의 8방향 배열을 독립 조회했다. 각 배열은 정확히 방향별 v4 `f01→f08` 8장을 참조했고 배열·이름 불일치는 `0`건이었다.
- 카메라 기준 8방향마다 정지 `f01` → 이동 frame index 3 `f04` → 같은 방향 정지 `f01`을 런타임 시각 레이어에서 재현했다. 8/8 모두 통과했으며, 화면 우측 이동은 보정 계약대로 `West`의 v4 `f01-02-w`를 표시했다.
- 대표 v4 Sprite와 기존 v3 Sprite의 `rect.width / PPU`를 독립 대조했다. v4 `128 / 64 = 2.000`, v3 `64 / 32 = 2.000` Unity unit이었다.
- Play의 `startingHostMode=QuarterView`, `CurrentHostMode=QuarterView`, `orthographic=True`, 접지 surface `True`, clearance `0.0050`을 확인했다.
- Pause 상태 MainCamera Capture에서 v4 Point 스프라이트가 쿼터뷰로 표시되고 정지 v4 세트만 보이는 것을 확인했다. 같은 월드 폭·직교 카메라 조건에서 기존 v3보다 원본 샘플 수가 2배여서 윤곽/다리·꼬리 계단이 더 조밀하게 표시되는 것을 확인했으나, 사용자 장시간 체감의 대체는 아니다.
- Play 중 Console Error/Warning은 0건이었다.

### 결과

- **통과:** v4 원본/반입본 64/64, 128px 캔버스, Importer 64/64, 8방향×8프레임 연결, 정지·이동·정지 같은 v4 세트, 수평 방향, 동일 2-unit 월드 폭, QuarterView 직교 카메라, 접지, 콘솔.
- **통과:** 저장·재로드 없이 현재 dirty 씬으로 Play 진입·종료 가능했다.

### 검증하지 못한 항목

- 실제 키보드 WASD 장시간 연속 조작에서의 사용자의 선명도·움직임 체감.
- 기존 v3과 같은 프레임의 동시 GameView A/B 캡처. 현재 활성 배열을 v4로 유지했으며, QA 중 v3로 되돌려 씬 상태를 바꾸지 않았다.
- 대상 EditMode TestRunner 단독 실행.

### 완료 판단

**이번 v4 해상도·Unity 통합 회귀 범위의 독립 QA 통과.** 사용자 시각 체감과 TestRunner 결과는 전체 작업의 별도 잔여 확인이다.

## 공유 상태 문서 독립 대조 — 2026-07-21

### 대조 대상

- `_workspace/active/CURRENT.md`
- `docs/project-handoff/current-task-board.md`
- 본 작업 `task.md`

### 대조 결과

- 세 문서는 모두 `2026-07-21-character-sprite-resolution-standard`의 최신 판정을 **회귀 QA 통과**로 표시한다.
- 세 문서가 공통으로 남긴 잔여 항목은 사용자 화면/연속 WASD 체감, 대상 EditMode TestRunner, 프로젝트 총괄 재검토다. 이는 본 문서의 독립 QA Play 통과와 "사용자 체감·TestRunner는 별도" 경계에 일치한다.
- 상태판의 v4 요약(128×128/PPU64, 2 Unity unit 폭, Importer 64/64, 8방향 `f01→f04→f01`, 접지·콘솔·캡처 통과)은 본 독립 QA 기록과 일치한다.
- 불일치 없음: 세 상태 문서에 QA 이전의 `QA 대기` 또는 전체 작업 `완료`로 잘못 표시된 항목은 없었다.

### 참고

`agent-activity.md`의 Unity 씬/통합 담당 행에는 해당 담당 산출물 시점의 `통합 완료 / QA 대기`가 남아 있다. 뒤의 독립 QA 행은 최신 `이번 v4 해상도·통합 회귀 범위 통과`를 명시하므로 이력 순서상 충돌은 아니지만, 요약표만 읽을 때 혼동될 수 있는 과거 시점 표현이다.

### 완료 판단

**공유 상태 문서 대조 통과.** 전체 작업 완료·커밋 완료로 승격하지 않으며, 기록된 사용자 체감·TestRunner·총괄 재검토를 유지한다.
