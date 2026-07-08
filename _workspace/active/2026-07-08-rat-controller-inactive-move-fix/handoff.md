# 핸드오프 기록

## 작업 ID

`2026-07-08-rat-controller-inactive-move-fix`

## 최신 사용자 요청

`CharacterController.Move called on inactive controller` 오류가 발생했다.

## 현재 상태

- 상태: 수정/컴파일 검증 완료, 에디터 Play 확인 대기
- 여기서 멈춤: RatHostController 이동 중단 조건과 회귀 테스트를 추가했다.
- 다음 세션의 첫 목표: Unity Editor에서 Console Error가 사라졌는지 Play로 확인한다.

## 먼저 읽을 파일

1. `UnityProject/Assets/_Project/Scripts/Host/RatHostController.cs`
2. `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
3. `_workspace/active/2026-07-08-rat-controller-inactive-move-fix/verification.md`

## 변경한 파일

- `UnityProject/Assets/_Project/Scripts/Host/RatHostController.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
- `_workspace/active/2026-07-08-rat-controller-inactive-move-fix/`
- `_workspace/active/CURRENT.md`

## 건드리면 안 되는 기존 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 기존 `APP_UI_EDITOR_ONLY` define 변경

## 마지막 성공 검증

- 런타임/테스트 어셈블리 수동 컴파일 통과
- `git diff --check` 통과

## 실패 또는 차단된 검증

- Unity MCP: `Connection revoked`
- Unity Test Runner / Console 확인 미완료

## 이어서 해야 할 일

1. Unity Editor에서 Play 재현 확인
2. Console에 같은 `CharacterController.Move called on inactive controller` 오류가 없는지 확인
3. 필요하면 수정 커밋/푸시

## 사용자 승인 필요

- 없음. 사용자 제보 오류에 대한 국소 수정이다.
