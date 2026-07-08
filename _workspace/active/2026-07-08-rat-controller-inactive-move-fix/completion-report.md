# 완료 보고: 비활성 CharacterController Move 오류 수정

## 요약

쥐 숙주 업데이트 중 모드 전환 또는 컨트롤러 비활성화가 발생해도 비활성 `CharacterController.Move()`를 호출하지 않도록 수정했다.

## 변경 파일

- `UnityProject/Assets/_Project/Scripts/Host/RatHostController.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
- `_workspace/active/2026-07-08-rat-controller-inactive-move-fix/`

## 검증

- 런타임/테스트 어셈블리 수동 컴파일 통과
- `git diff --check` 통과

## 총괄 판정

- 내부 승인 가능
- 사용자 제보 오류에 대한 국소 수정이며 프로토타입 범위 변경이 없다.
