# 인수인계: 쥐 본능 정지 리듬과 면역 신호 억제 테스트 진입

## 최신 사용자 요청

쥐 자동 배회가 계속 걷기만 하는 문제를 줄이고, 신경 조종 보상 이후 강제 조종 경계도 증가가 사라지는 것이 정상인지 확인하며, 면역 신호 억제 미니게임을 테스트할 수 있게 준비한다.

## 현재 상태

- 구현은 적용됨.
- Unity MCP 코드 컴파일/관련 테스트 13개는 통과.
- Unity MCP Play 진입은 성공했으나 Stop/Console 단계에서 연결 승인이 끊김.
- 사용자 최종 플레이 확인 전 상태.

## 변경한 파일

- `UnityProject/Assets/_Project/Scripts/Host/RatHostControlModel.cs`
- `UnityProject/Assets/_Project/Scripts/Host/RatHostController.cs`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeKeyboardInput.cs`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionController.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- `docs/design/encounters/immune-signal-suppression-rhythm-minigame.md`
- `_workspace/active/CURRENT.md`
- `_workspace/active/2026-07-08-rat-instinct-pause-and-signal-test/*`

## 건드리면 안 되는 기존 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` define 변경은 기존 범위 밖 변경이다.

## 마지막 성공 검증

- Unity MCP `Unity_RunCommand`: 관련 테스트 13/13 통과.
- `git diff --check`: 종료 코드 0, CRLF 변환 경고만 표시.

## 실패하거나 차단된 검증

- Unity MCP Play 중 Stop/Console 확인에서 `Connection revoked`.
- 실제 키보드로 `F6` 진입과 쥐 움직임 체감은 미확인.

## 바로 이어서 할 작업

1. Unity Editor의 MCP 승인을 복구하고 Play 상태라면 정지한다.
2. Console Error/Warning을 확인한다.
3. 사용자가 쥐 배회 멈춤, 신경 조종 후 강제 조종 경계도, `F6` 면역 신호 억제 진입을 플레이로 확인한다.

## 사용자 결정 필요 항목

- 신경 조종 이후 강제 조종 경계도가 오르지 않는 현재 규칙을 유지할지.
- 쥐 자동 배회의 이동/정지 시간 기본값을 더 짧거나 길게 조정할지.
