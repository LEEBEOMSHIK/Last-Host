# 작업 로그: 면역 신호 억제 미니게임 구현

## 2026-07-08

- 사용자 요청: 면역 신호 억제 미니게임 구현을 먼저 진행하고, 이후 기존 백혈구 회피 미니게임을 수정한다.
- 범위 판단: 기존 백혈구 회피는 기본값으로 유지하고, 신호 억제는 새 내부 미니게임 타입으로 추가한다.
- TDD 방침: 판정 모델과 세션 연결 테스트를 먼저 추가하고 RED 확인 후 구현한다.
- 주의: `UnityProject/ProjectSettings/ProjectSettings.asset`의 기존 미커밋 변경은 이번 작업에서 제외한다.
- RED 확인: 기존 컴파일 산출물에는 `ImmuneSignalSuppressionModel`, `InternalVirusMinigameType`, 타입 지정 진입 API가 없어 실패하는 것을 확인했다.
- 구현: 신호 억제 판정 모델, 내부 미니게임 타입, 세션 전환, HUD 상태 표시, 개발용 진입 API, EditMode 테스트를 추가했다.
- 검증: 런타임/테스트 어셈블리 수동 컴파일 통과, 리플렉션 스모크 검증 통과, `git diff --check` 통과.
- 제한: Unity MCP는 `Connection revoked` 상태라 MCP Play 체크와 Unity Test Runner 실행은 완료하지 못했다.
