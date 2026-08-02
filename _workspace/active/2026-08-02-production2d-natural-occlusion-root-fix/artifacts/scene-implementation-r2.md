# Production2D initial spawn non-overlap correction r2

## 후보 식별

- correction: `1/2`
- supersedes candidate: `ed8e9caf5f0e0f38fd05fbd11ff3151e54caf1e5fe128232357d8759b1836f8d`의 r1 FAIL 후보
- run_id: `natural-occlusion-scene-r2-20260802`
- candidate fingerprint: `cd6946deff7ecf1e1f4e4aed6c2fd532f1a97c5e895bb79de6fe00b4bee49385`
- manifest: `scene-candidate-manifest-r2.json`
- r1 failure evidence: `scene-targeted-r1.xml`, `8 total / 7 passed / 1 failed`

## root cause와 계측

- 초기 rat root: `(-1,-0.25)`
- Barrel_A root: `(-1,-0.75)`
- exact collider 상태에서 rat↔barrel distance: `-0.05687499 world`, 약 `-7.28 logical px`
- `ColliderDistance2D.isOverlapped=true`, 분리축은 rat을 `+Y`로 이동하는 방향
- collider 축소·test 완화·renderer hide는 사용하지 않았다.

## 최소 수정

- builder의 초기 rat spawn에 `+8/128 world Y`를 더했다.
- 새 rat root: `(-1,-0.1875)`
- Barrel_A와 map 오브젝트 배치는 변경하지 않았다.
- exact rat capsule, wall/barrel/crate reference polygon, scene test는 변경하지 않았다.
- camera가 rat을 따라가므로 캐릭터 화면 중심은 유지되고, 월드 초기 framing 변화는 8 logical px로 제한된다.

## 구현자 재검증

- builder `ValidateScript standard`: 오류 0, 기존 일반 GetComponent null-check warning 1.
- builder 재실행·scene 저장: 성공, Play/Pause false, dirty false.
- 초기 collider distance:
  - Barrel_A: overlap false, distance `0.00562499464 world` = 약 `0.72 logical px`
  - Crate_A: overlap false, distance `1.80094`
  - WallStraight_Occlusion: overlap false, distance `0.917257547`
  - 다른 직선 벽 2개도 overlap false
- visible gap `0.72px`는 S0 허용 `-1..+2px` 안이다.
- Unity Console Error: 0.
- `git diff --check`: PASS.
- MCP TestRunner 재시도: 0. 조정자 격리 scene targeted r2 1회 실행 대기.
- full suite/MCP Play/matrix/capture/build: 0.

## Unity lease

- owner/run: `unity_scene_integration` / `natural-occlusion-scene-r2-20260802`
- editor PID: `54432`
- acquire/renew: `1/1`
- baseline/final: scene `RatHost2DTechnicalSample`, Play/Pause false, dirty false, 임시 객체 0
- release: `2026-08-02T04:53:14.9733500Z` 명시 반납

## 비용 proxy

- correction `1/2`
- Unity lease acquire/renew/release `1/1/1`
- measurement RunCommand 2, builder 1, post-fix scene smoke 1, validation 1, Console read 1
- targeted TestRunner 실제 시작/XML 0/0
- full suite/MCP Play/matrix/capture/build 0
- exact token/$ 미집계

## 인계

- r2는 구현자 static/scene smoke 후보이며 targeted PASS가 아니다.
- 조정자가 같은 fingerprint의 격리 복제본에서 `Production2DV1AssetAndSceneTests`를 1회 실행해야 한다.
