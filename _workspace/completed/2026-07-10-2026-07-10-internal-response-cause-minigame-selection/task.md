# 작업 배정서

## 기본 정보

- 작업 ID: 2026-07-10-internal-response-cause-minigame-selection
- 작업명: 내부 대응 원인별 미니게임 선택
- 상태: 구현/대체 검증 완료, Unity MCP 직접 검증 제한
- 생성일: 2026-07-10
- 담당 에이전트: 게임플레이 구현 에이전트
- 보조 에이전트: QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: rat-host-loop-builder, unity-verification-runner

## 루프 단계

면역 경계도 100%에서 내부 바이러스 미니게임으로 전환되는 단계.

## 목적

면역 경계도 상승 원인에 따라 내부 대응 미니게임 타입을 선택한다. 기본 루프는 백혈구 회피를 유지하되, 강제 조종이나 신호성 자극 계열 원인은 이미 구현된 `면역 신호 억제`로 진입하게 한다.

## 입력 자료

- 사용자 요청: `내부 대응 원인별 미니게임 선택 작업 진행해`
- `AGENTS.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/plans/rat-host-immune-alert-work-direction.md`
- `docs/design/game-design-summary.md`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionState.cs`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionController.cs`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeConfig.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`

## 작업 단위

1. 면역 경계도 상승 원인 라벨을 내부 대응 선택에 사용한다.
2. 원인 라벨을 `백혈구 회피` 또는 `면역 신호 억제`로 매핑한다.
3. 기본/무라벨/오염 노출은 기존 백혈구 회피를 유지한다.
4. `강제 조종`, `소음/조직 자극`, `면역 신호` 계열은 면역 신호 억제로 진입한다.
5. 테스트와 Unity MCP 또는 동등 검증 기록을 남긴다.

## 수용 기준

- 기존 `EnterVirusMinigame()` 직접 호출과 기본 설정은 `WhiteBloodCellEvasion`을 유지한다.
- `AddImmuneAlertAmount(..., "강제 조종")`으로 경계도 100%에 도달하면 `ImmuneSignalSuppression`으로 진입한다.
- `AddRiskAlert(..., "소음/조직 자극")`으로 경계도 100%에 도달하면 `ImmuneSignalSuppression`으로 진입한다.
- `AddImmuneAlertAmount(..., "오염 노출")`은 기존처럼 `WhiteBloodCellEvasion`으로 진입한다.
- `F6` 디버그 진입은 계속 `ImmuneSignalSuppression`으로 동작한다.
- 기존 성공/실패/변이 선택 테스트가 회귀하지 않는다.

## 금지 범위

- 새 내부 미니게임 유형 추가
- 새 UI 패널 추가
- 새 입력 키 추가
- 전체 튜토리얼 시스템 추가
- 숙주 체인, 인간 단계, 백신, 엔딩 확장
- ProjectSettings 변경

## 완료 기준

- 코드와 테스트 변경이 적용됨.
- Unity 내장 Mono/Roslyn 기반 제품/테스트 어셈블리 컴파일과 임시 실행 하니스 검증 완료.
- Unity MCP 직접 `RunCommand` 검증은 relay 응답 문제로 완료 결과를 확보하지 못해 제한 사항으로 기록.
- `verification.md`, `work-log.md`, `agent-activity.md`, `completion-report.md`에 결과 기록.
- 검증하지 못한 항목은 완료로 주장하지 않는다.
