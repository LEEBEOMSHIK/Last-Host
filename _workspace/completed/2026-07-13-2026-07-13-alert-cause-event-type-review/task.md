# 작업 패킷: 원인 라벨 enum/event type 전환 구현

- 작업 ID: `2026-07-13-alert-cause-event-type-review`
- 상태: 완료 (사용자 구현 승인: 2026-07-13)
- 담당 에이전트: 게임플레이 구현 에이전트
- 보조 에이전트: 게임플레이 루프 에이전트, QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `last-host-design-keeper`, `rat-host-loop-builder`

## 목적

면역 경계도 상승의 표시 문구(`feedbackLabel`)와 내부 미니게임 선택용 원인 분류를 분리한다. 사용자 승인에 따라 기존 `면역 신호`·`경보` → 면역 신호 억제 매핑을 명시적으로 보존한다.

## 입력 자료

- `AGENTS.md`
- `docs/project-handoff/current-task-board.md`
- `docs/design/encounters/internal-immune-response-minigame-types.md`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionState.cs`
- 원인별 미니게임 선택 완료 패킷

## 산출물

- `ImmuneAlertCauseType`과 최소 이벤트 데이터 모델
- 세션·컨트롤러·현재 발생원의 타입 기반 호출 전환
- 기존 매핑 보존 EditMode 테스트와 구현 인계
- QA 검증 기록과 총괄 관리자 판정
- 완료 패킷 보관과 사용자용 현재 상태판 동기화

## 금지 범위

- 새 미니게임, 새 원인 콘텐츠, 프로토타입 범위 확장
- 문서상 확정 결정을 사용자 승인 없이 변경
- 씬·프리팹·Inspector 직렬화·ProjectSettings·패키지 변경
- 기존 원인→미니게임 매핑, 보상·실패·복귀 흐름 변경

## 완료 기준

- [x] HUD 표시 문구가 바뀌어도 명시된 원인 타입의 내부 미니게임 선택은 바뀌지 않는다.
- [x] 무라벨·미지정 타입은 기존 기본 미니게임으로 폴백한다.
- [x] `오염·면역 포착·바이러스 흔적`은 백혈구 회피, `강제 조종·소음/조직 자극·면역 신호·경보`는 신호 억제를 유지한다.
- [x] 새 콘텐츠·씬·설정 변경 없이 EditMode 회귀와 Unity MCP Play 검증을 남긴다.
