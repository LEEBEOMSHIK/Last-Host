# 작업 배정서 — 쥐 시각·카메라 EditMode 회귀 기술 게이트

## 기본 정보

- 작업 ID: `2026-07-24-rat-visual-camera-editmode-regression`
- 작업명: 쥐 걷기·스프라이트·픽셀·카메라 EditMode 회귀 테스트 일괄 실행 및 기술 게이트 종결
- 상태: 완료 보관 — 전체 EditMode 101/101·MCP Play 통과, 프로젝트 총괄 `내부 승인 가능`
- 생성일: 2026-07-24
- 주담당: QA/검증 에이전트
- 후속 담당: 프로젝트 총괄 관리자 에이전트
- 실패 시 최소 수정 담당: 게임플레이 구현 에이전트 또는 Unity 씬/통합 구현 에이전트에 원인 경계에 따라 별도 배정
- 사용 스킬: `$unity-verification-runner`

## 검증 주장

현재 저장소의 Unity EditMode 전체 테스트가 실패 0으로 통과하며, 쥐 걷기(v3), 스프라이트 해상도(v4), 픽셀 처리(v5b), 카메라·RatVisual·WASD 관련 회귀 테스트가 식별 가능한 결과로 확인되고, 컴파일·Console·가능 범위의 MCP Play와 씬 비변경 기준을 충족한다.

## 목적

여러 active 시각 작업과 완료된 카메라·이동 작업에 흩어진 EditMode TestRunner 잔여를 한 번에 실행한다. 기술 회귀 게이트를 사용자 시각 수용과 분리해 판정하고, 통과 시 v3·v4·v5b 작업의 기술 검증 공백을 닫을 근거를 만든다.

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 산출물 |
| --- | --- | --- | --- |
| QA/검증 에이전트 | 주담당 독립 검증자 | 전체 EditMode 실행, 관련 테스트 식별, 컴파일·Console·MCP Play·씬 비변경 확인, 완료 가능 여부 판정 | `verification.md`, `artifacts/`, `agent-activity.md` |
| 게임플레이 구현 에이전트 | 조건부 수정 담당 | 테스트 실패가 C# 로직·모델·테스트 원인일 때 별도 배정 후 최소 수정 | 별도 구현 기록과 변경 diff |
| Unity 씬/통합 구현 에이전트 | 조건부 수정 담당 | 테스트 실패가 씬 연결·직렬화·카메라 통합 원인일 때 별도 배정 후 최소 수정 | 별도 통합 기록과 변경 diff |
| 프로젝트 총괄 관리자 에이전트 | 내부 승인자 | QA 결과, 범위, 상태판, 미검증 항목을 검토하고 최종 기술 게이트 판정 | 필요 시 `director-review.md`, `completion-report.md` |
| 문서/릴리즈 에이전트 | 시작 상태 동기화 | 작업 패킷, `CURRENT.md`, 공유 상태판 시작 상태 정리 | 본 작업 시작 문서 |

## 입력 자료

- `AGENTS.md`
- `docs/agents/loop-engineering-gates.md`
- `_workspace/active/2026-07-20-rat-walk-unity-visual-trial/`
- `_workspace/active/2026-07-21-character-sprite-resolution-standard/`
- `_workspace/active/2026-07-21-rat-pixel-treatment-v5/`
- `_workspace/completed/2026-07-24-2026-07-21-game-view-camera-output-fix/`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
- `docs/project-handoff/current-task-board.md`

## 해야 할 일

1. 실행 전 Git 상태, Unity Play/Edit 상태, 대상 씬과 Console을 확인하고 범위 밖 변경을 기록한다.
2. Unity EditMode 전체 테스트를 한 번에 실행해 총수·성공·실패·건너뜀·소요 시간을 원본 결과와 함께 남긴다.
3. 전체 결과에서 v3 걷기 프레임·방향·정지 복귀, v4 해상도·Importer·월드 크기, v5b 픽셀 스냅·출력, 카메라 추적·RatVisual 누적 이탈·WASD 우선 관련 테스트를 식별한다.
4. Unity 컴파일 상태와 Console Error/Warning을 확인한다.
5. 가능한 범위에서 `RatHostPrototype` MCP Play 진입·종료, RatHost·RatVisual·카메라 핵심 상태와 씬 dirty 여부를 확인한다. 실행 불가하면 사유를 기록한다.
6. 테스트 전후 씬·ProjectSettings·Builds와 Git 상태를 대조해 검증이 프로젝트 산출물을 바꾸지 않았는지 확인한다.
7. QA가 `완료 가능`, `수정 필요`, `차단` 중 하나로 판정한다.
8. 실패 시 즉시 코드·씬을 수정하지 말고 원인을 게임플레이 로직 또는 씬/통합 경계로 분류해 해당 구현 에이전트에 별도 배정한다.
9. QA 통과 후 프로젝트 총괄 관리자에게 내부 승인 판정을 요청한다.

## 산출물

- `task.md`
- `work-log.md`
- `agent-activity.md`
- `verification.md`
- `handoff.md`
- `artifacts/` 아래 TestRunner 원본 결과·Console·MCP Play·전후 상태 증거
- 필요 시 후속 담당이 작성할 `director-review.md`, `completion-report.md`

## 금지 범위

- `UnityProject/ProjectSettings/ProjectSettings.asset`
- `_workspace/previews/`
- `Builds/`
- 패키지 추가·갱신
- 아트·스프라이트·Blender 원본 생성·수정
- 사용자의 화면 수용이 필요한 보행 과장도·선명도·픽셀 미감·최종 시각 판정
- 테스트 실패가 확인되고 담당 구현 에이전트가 별도 배정되기 전 코드·테스트·씬 수정
- 자연 경계도 엄격 검증의 차단 상태를 본 기술 테스트로 해제

## 완료 기준

1. Unity EditMode 전체 실행 결과의 총수와 실패 `0`이 원본 증거로 남는다.
2. v3·v4·v5b·카메라·RatVisual·WASD 관련 회귀 테스트가 결과에서 식별된다.
3. 컴파일 오류가 없고 Console Error/Warning 결과가 기록된다.
4. 가능한 범위의 MCP Play와 Play 종료 후 씬 비변경을 확인하거나 수행 불가 사유를 남긴다.
5. QA/검증 에이전트가 `완료 가능`으로 판정한다.
6. 프로젝트 총괄 관리자 에이전트가 `내부 승인 가능`으로 판정한다.
7. 사용자 시각 수용은 별도 후보로 유지하며 본 기술 게이트 통과로 대신하지 않는다.

## 2026-07-24 QA 결과와 남은 판정

- 수정 후 전체 EditMode: `101/101 PASS`, 실패·skip·inconclusive `0`, `6.3731956s`.
- MCP Play: RatHost, QuarterView MainCamera, 별도 GameViewFrameCamera, RatVisual, HUD, 960×540 RenderTexture 연결과 Console Error/Warning `0` 확인.
- 종료 상태: Stop 후 Edit·비컴파일·비업데이트, 활성 씬 clean.
- 비변경: 씬·ProjectSettings·Builds 변경 없음.
- QA 판정: `완료 가능 — 자동 기술 게이트`.
- 남은 위험: v4의 `128×128 / PPU64 / world width 2`를 직접 검사하는 EditMode 테스트는 없으며 선행 MCP 증거를 유지한다.
- 남은 게이트: 프로젝트 총괄 관리자 에이전트의 범위·위험·상태판 검토와 내부 승인 판정.

## 커밋 전 차단 조건

- 작업 패킷·담당 산출물·`agent-activity.md` 누락
- 전체 EditMode 결과 원본 또는 총수 누락
- 실패 또는 컴파일·Console 문제 미해결
- 코드·씬 변경이 생겼지만 구현 담당 에이전트 별도 배정과 재검증이 없음
- 상태판 독립 대조 또는 총괄 관리자 판정 누락
