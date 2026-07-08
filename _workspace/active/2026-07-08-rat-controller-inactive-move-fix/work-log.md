# 작업 로그: 비활성 CharacterController Move 오류 수정

## 2026-07-08

- 사용자 제보: 면역 신호 억제 확인 중 백혈구 회피로 진입했고, 이후 `CharacterController.Move called on inactive controller` 오류가 발생했다.
- 원인 조사: `RatHostController.Update()`에서 강제 조종 디메리트가 모드 전환을 유발하면 같은 프레임 뒤쪽의 `CharacterController.Move()`가 비활성 컨트롤러에 호출될 수 있다.
- 주의: `UnityProject/ProjectSettings/ProjectSettings.asset`의 기존 `APP_UI_EDITOR_ONLY` 변경은 이번 작업 범위가 아니다.
- RED 테스트 추가: 비활성 `CharacterController`가 있을 때 `RatHostController.Update()`가 예기치 않은 Unity 로그를 만들지 않아야 한다.
- 수정: 디메리트 적용 뒤 RatHost 모드가 아니거나 오브젝트/컨트롤러가 비활성화되면 같은 프레임 이동 처리를 중단한다.
- 검증: 런타임/테스트 어셈블리 수동 컴파일 통과, `git diff --check` 통과.
- 제한: Unity MCP가 `Connection revoked` 상태라 에디터 콘솔과 Test Runner 실행은 미완료다.
