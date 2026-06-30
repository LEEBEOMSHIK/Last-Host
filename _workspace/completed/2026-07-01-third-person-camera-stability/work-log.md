# 작업 기록

## 2026-07-01

- 사용자 보고를 기준으로 3인칭 카메라 회전 문제를 조사했다.
- 원인: `PrototypeCameraController.ApplyThirdPerson`이 `hostTarget.eulerAngles.y`로 카메라 오프셋을 회전시키고 있었다.
- 추가 원인 연결: `RatHostController`는 이동 방향으로 쥐를 회전시키고, 이동 방향은 카메라 기준으로 계산된다. 따라서 입력 중 쥐 회전과 카메라 기준축이 서로 영향을 주는 구조였다.
- RED 확인: 쥐를 90도 회전시키면 3인칭 카메라 오프셋이 `(0, 3.6, -5.2)`에서 `(-5.2, 3.6, 0)`으로 바뀌는 것을 Unity MCP에서 확인했다.
- 수정: 3인칭 카메라 오프셋을 쥐 회전으로 회전시키지 않고 월드 기준 고정 오프셋으로 적용했다.
- 회귀 테스트: `PrototypeCameraController_ThirdPersonDoesNotOrbitWhenRatTurns`를 추가했다.
- 검증: 직접 테스트 실행 15/15 통과, Play 상태 카메라 안정성 확인, 콘솔 오류 0건 확인.
