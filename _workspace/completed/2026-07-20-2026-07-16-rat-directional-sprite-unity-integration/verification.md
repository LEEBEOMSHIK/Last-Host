# 구현 담당 자체 검증

## 결과

- 구현 상태: 완료
- 독립 QA: `완료 가능` — 별도 동적 재실행은 MCP 점유 지연으로 미확보, 정적·캡처·구현 증거 대조 완료
- 활성 씬: `Assets/_Project/Scenes/RatHostPrototype.unity`
- Unity 상태: Play 종료, 비컴파일, 씬 clean

## 정적·컴파일 검증

- `RatDirectionQuantizer.cs`: Unity 표준 검증 진단 0건
- `RatDirectionalSpriteView.cs`: Unity 표준 검증 진단 0건
- `RatHostPrototypeSceneBuilder.cs`: Unity 표준 검증 진단 0건
- `RatHostPrototypeCoreTests.cs`: Unity 표준 검증 진단 0건
- 원본/Unity 복사본 8개 PNG SHA-256 일치
- Import 메타 확인: Sprite Single, PPU 32, pivot `(0.5, 0.25)`, Point, mipmap off, alpha transparency

## EditMode

- 실행 경로: Unity `TestRunnerApi`, assembly `LastHost.Prototype.Tests`, EditMode 전체
- 최신 결과: 접지 회귀 포함 `94 passed / 0 failed / 0 skipped / 0 inconclusive`
- 최신 실행 시간: 4.621초

## Play Mode

- Play 진입/종료: 성공
- 자동 숙주 이동 관측: `NorthWest → rat-03-nw`
- `RatVisual`: SpriteRenderer 활성, `RatDirectionalSpriteView` 활성
- 캡슐 잔존 여부: MeshRenderer 없음, MeshFilter 없음
- 8방향 직접 바인딩 대조: 전부 일치
- 정지 유지: `True`
- 카메라 facing 오차: `0.000°`
- 대체 캡처: `UnityProject/Temp/rat-directional-sprite-play.png`에서 화면 표시와 바닥 접지 확인
- 종료 후 씬 dirty: `false`
- 최종 Console Error/Warning: 0건

## 도구 실패와 검증 경계

- 첫 씬 반입 동적 명령은 Unity 6 `TextureImporter` API 호환 오류로 컴파일 1회 실패했다. 씬 변경 전 실패했고 수정 후 성공했다.
- `Unity_Camera_Capture`는 Play Mode 카메라 인스턴스 ID를 찾지 못해 1회 실패했으며, 이 도구 오류가 콘솔에 기록되었다. Main Camera 직접 RenderTexture 렌더로 대체 캡처한 뒤 콘솔을 정리했고 최종 Error/Warning은 0건이다.
- 8방향 전체 대조는 `RefreshDirection` 직접 호출을 사용한 기능 상태 검증이다. 실제 WASD 입력 수신 증명이 아니며, 사용자가 Play Mode에서 이동 체감을 최종 확인해야 한다.

## QA 발견 금지 범위 변경과 복구

- 발견: `ProjectSettings.asset`의 `Standalone` scripting define이 `SENTIS_ANALYTICS_ENABLED;APP_UI_EDITOR_ONLY`로 바뀐 1줄 diff.
- 판정: 작업 금지 범위이며 구현 담당 초기 변경 파일 검토 누락.
- 복구: HEAD 기준 `SENTIS_ANALYTICS_ENABLED`로 해당 1줄만 최소 복구.
- 다른 ProjectSettings/사용자 변경: 없음, 미수정.
- 복구 검증: `git diff --exit-code -- UnityProject/ProjectSettings/ProjectSettings.asset` 통과, Git 상태 변경 0.
- 재발 확인: Unity Editor가 열린 상태에서 3초 후 재검사 통과.
- 경계: 정확한 자동 추가 주체는 확정하지 않았고, 씬·Play·Test Runner를 재실행하지 않았다.

## 사용자 접지 피드백 수정 검증 — 2026-07-16

### 구현 경계

- 변경: `RatVisual` 표시 y 보정, 접지 후보 선택 로직, 씬 빌더 설정, EditMode 테스트
- 미변경: `RatHostController`, CharacterController, `ImmuneRiskZone`, trigger 판정·크기·위치, 카메라, ProjectSettings
- 표면 감지: `RaycastNonAlloc`, trigger 포함, 자기 숙주 계층 제외, 활성 Renderer가 있는 상향 표면만 허용

### EditMode

- 순수 후보 선택: 일반 바닥 `-0.02`와 오염 표면 `0.07` 중 유효한 최상단 선택
- Physics 통합: trigger 표면 포함, 자기 CharacterController 제외, 일반 바닥 복귀 확인
- 전체 결과: `94 passed / 0 failed / 0 skipped / 0 inconclusive`
- 실행 시간: 4.621초

### Play 수치

| 위치 | 표면 top y | RatVisual pivot y | clearance |
| --- | ---: | ---: | ---: |
| 일반 바닥 | `-0.0200` | `-0.0150` | `0.0050` |
| 오염 trigger | `0.0700` | `0.0750` | `0.0050` |

- 일반 바닥 캡처: `UnityProject/Temp/rat-grounding-floor.png`
- 오염 구역 캡처: `UnityProject/Temp/rat-grounding-toxic.png`
- 시각 판정: 두 위치 모두 표면 교차 없음, 과도한 부유 없음
- 방향 회귀: 8방향 전체 일치
- 정지 유지 회귀: `True`
- Play 종료: 성공, 씬 dirty `false`
- Console Error/Warning: 0건

### 부수 변경 복구

- 동적 Unity 명령으로 재발한 `APP_UI_EDITOR_ONLY` define 1줄을 HEAD 상태로 복구했다.
- 씬 저장으로 추가된 무관한 `signalSuppressionHud: null` 1줄을 제거했다.
- 최종 `ProjectSettings.asset` diff/status: 0
- Unity 열린 상태 3초 후 ProjectSettings 재발: 없음

## 사용자 2차 접지 피드백 수정 검증 — 2026-07-20

검증 대상:

- 오염 위험 trigger를 지나도 `RatVisual` y가 일반 바닥 기준 `-0.015`로 유지되는지, 위험 노출 trigger는 유지되는지, 오염 시각 표면이 쥐 하단을 관통하지 않는지.

실행한 검증:

- `git diff --check` 통과.
- 코드·저장 씬 정적 대조: trigger 후보 제외, `ImmuneRiskZone` 위치 `(-0.7, 0.03, 1.35)`·크기 `(2.6, 0.08, 1.5)` 보존, 별도 `ToxicWaterVisual` top `-0.018`, RatVisual pivot 기준 여유 `0.003`을 확인.
- EditMode 갱신: trigger 내부/외부 RatVisual y 동일, 위험 trigger 구조·위험 노출 회귀, 기존 8방향/idle 회귀를 포함.

결과:

- 정적 검증 통과. Unity 동적 검증은 아직 실행하지 않아 통과로 주장하지 않는다.

MCP 플레이 체크:

- 미실행. 사용자 Unity Editor와 AssetImportWorker가 `UnityProject`를 점유 중이어서 별도 batch Unity 및 Play 진입을 실행하지 않았다.
- 인계 직전 프로세스 재확인 명령도 사용자 중단으로 결과를 수집하지 않았으며, 구현 담당은 추가 Unity 실행을 하지 않는다.

검증하지 못한 항목:

- Unity 컴파일, EditMode 전체 실행, Play에서 일반/오염 구역 pivot y 수치, 위험 노출, Console Error/Warning.

남은 위험:

- 실제 Game View에서 얇은 오염 표면의 가시성 및 쥐 하단 간격, 경계 통과 체감은 동적 검증과 사용자 확인이 필요하다.

완료 판단:

- 구현 담당 기준 동적 검증 대기. QA 완료 또는 사용자 수용을 주장하지 않는다.
