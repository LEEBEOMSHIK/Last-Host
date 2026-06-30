# 완료 보고

## 작업 ID

`2026-06-30-rat-host-input-eventsystem-fix`

## 문제

- 사용자 확인 중 조작이 되지 않는다고 보고됐다.
- Unity 콘솔에는 MCP Unity WebSocket `8090` 포트 중복 사용 오류가 있었다.

## 확인 결과

- `8090` 포트는 현재 열린 Unity Editor 프로세스가 점유 중이었다.
- 이 오류는 MCP 서버 중복 시작 로그로 보이며, 게임 입력 미동작의 직접 원인으로 확인되지는 않았다.
- 현재 진단 시점의 Unity Editor는 Play 모드가 아니었다.
- `Keyboard.current`와 Input System 장치 상태는 Unity 내부 검증에서 정상으로 확인됐다.
- `RatHostPrototype.unity`에는 `EventSystem`이 없어 UI 버튼 입력은 실제로 동작하지 않는 상태였다.

## 수정

- 씬 생성기에 `EventSystem`과 `InputSystemUIInputModule` 생성을 추가했다.
- 기존 씬을 재저장하지 않아도 Play 모드에서 UI 입력이 살아나도록 `PrototypeHud`가 런타임에 EventSystem을 보강하게 했다.
- 변이 선택은 키보드 `1`, `2`, `3`으로도 가능하게 했다.

## 검증

- `git diff --check` 통과.
- MCP 승인이 중간에 끊겨 Unity 컴파일 검증과 씬 재생성은 실행하지 못했다.
- Unity Editor에서 `Project Settings > AI > Unity MCP` 승인 복구 후 씬 재생성/컴파일 검증 필요.
