# 검증 기록: 비활성 CharacterController Move 오류 수정

## 검증 대상

`RatHostController.Update()`가 모드 전환 또는 컨트롤러 비활성화 이후 비활성 `CharacterController.Move()`를 호출하지 않는지 확인한다.

## 실행한 검증

- 런타임 스크립트 수동 컴파일
- 테스트 어셈블리 수동 컴파일
- `git diff --check`
- Unity MCP 콘솔 접근 확인

## 결과

- 런타임 컴파일: 통과
- 테스트 어셈블리 컴파일: 통과
- 공백 검사: 통과
- 줄바꿈 CRLF 변환 경고만 출력됨

## MCP / 에디터 제한

- 이전 Unity MCP 결과: `Connection revoked. Go to Unity Editor > Project Settings > AI > Unity MCP to change approval.`
- 2026-07-09 01:19 KST MCP 복구 후 Unity Editor Console 로그 확인과 Test Runner 실행을 완료했다.

## MCP 복구 후 추가 검증

- 실행: `Unity_RunCommand: Codex EditMode Test Runner Retry`
- 결과: `LastHost.Prototype.Tests` EditMode 64개 통과, 실패 0, 스킵 0
- 포함 확인: `RatHostController_UpdateDoesNotMoveInactiveCharacterController`
- 실행: `Unity_ManageEditor Play` + `Unity_RunCommand: Codex MCP Play Loop Check` + `Unity_ManageEditor Stop`
- 결과:
  - 내부 미니게임 진입 중 `RatHostController` 비활성화 확인
  - 변이 선택 후 쥐 숙주 복귀 시 `RatHostController` 재활성화 확인
  - Play 종료 후 Unity Console Error/Warning 0건

## 추가된 회귀 테스트

- `RatHostController_UpdateDoesNotMoveInactiveCharacterController`
- 의도: 비활성 `CharacterController`가 있을 때 `RatHostController.Update()`가 예기치 않은 Unity 로그를 만들지 않아야 한다.

## 남은 위험

- 실제 사용자 조작감 확인은 별도 수동 플레이가 필요하다.

## 완료 판단

- 코드 수준 수정, Test Runner, MCP Play/Console 검증 기준 통과.
