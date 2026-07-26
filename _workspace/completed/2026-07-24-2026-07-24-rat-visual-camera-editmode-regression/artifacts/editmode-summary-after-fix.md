# 수정 후 전체 EditMode 요약

- 실행 시각: 2026-07-24 14:20 KST
- 실행 방식: 기존 Unity Editor의 `TestRunnerApi`에서 전체 EditMode 동기 실행
- Run ID: `c7ba0367-3339-4c63-aec5-b1deb3a949e8`
- 결과: `101 total / 101 passed / 0 failed / 0 skipped / 0 inconclusive`
- 실행 시간: `6.3731956s`
- XML: `editmode-results-after-fix.xml`
- XML SHA-256: `FB20ABC4DE772EBD5605691A87E6E9C53DC4F1A2460D7B5C2983F146D336F105`
- 로그: `unity-editmode-after-fix.log`
- 로그 SHA-256: `7C87D72222FC29DBC34348C8CEA9D819F5EBF8824432EE930827544010635933`

## 관련 회귀 축

- WASD·숙주 본능: `RatHostControlModel_*`, `HostInstinctControlSpike_*`, `RatHostInstinctWander_*`, `PrototypeKeyboardInput_Composes*` 통과.
- v3 방향·걷기: 8방향 quantizer, idle 방향 유지, 8fps walk cycle 통과.
- v4 관련 현재 자동화: `RatDirectionalSpriteAssets_ShareCanvasAndPivotForDirectionalVisibleFootOffsets`, 씬 쥐 가시성, 접지·ground resolver 통과.
- v5b 픽셀 처리: RatVisual horizontal pixel snap, invalid PPU 원위치, 카메라 screen-plane snap·invalid output 조건 통과.
- 카메라·씬: 모드 순환, QuarterView 즉시 추적·좌후방 축, ThirdPerson 비회전, TopView, QuarterView MainCamera 기본 계약 통과.

## 범위 메모

현재 전체 테스트의 스프라이트 importer 자동화는 TrialV1의 64×64, PPU 32, custom pivot 계약을 검사한다. v4의 128×128, PPU 64, world width 2를 직접 명명해 검사하는 EditMode 테스트는 없다. v4 선행 MCP 검증 증거는 유지되며, 이번 101/101 결과가 그 직접 자동화 공백까지 대체하지는 않는다.
