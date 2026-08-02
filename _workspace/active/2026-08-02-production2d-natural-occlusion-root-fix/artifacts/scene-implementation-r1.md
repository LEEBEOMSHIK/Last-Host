# Production2D natural occlusion scene 통합 보고 r1

## 후보 식별

- S0 revision: `natural-occlusion-s0-r4-footprint-contract`
- S0 scope fingerprint: `b09aa2ece964e9764af6bda98f56cbb6f7a3158887b03f06b6872f36d802a0f8`
- gameplay handoff: `gameplay-targeted-r1.xml` `3/3 PASS`
- run_id: `natural-occlusion-scene-r1-20260802`
- candidate fingerprint: `ed8e9caf5f0e0f38fd05fbd11ff3151e54caf1e5fe128232357d8759b1836f8d`
- manifest: `scene-candidate-manifest-r1.json`
- 경로 정정: 배정서의 `Editor/RatHost2D/RatHost2DProductionSampleSceneBuilder.cs`는 존재하지 않았다. 조정자 승인에 따라 실제 파일 `Editor/TechnicalSample2D/RatHost2DProductionSampleSceneBuilder.cs`를 소유 builder로 사용했다.

## 변경 파일과 불변식

| 파일 | 변경 | 고정 계약 |
| --- | --- | --- |
| `RatHost2DProductionSampleSceneBuilder.cs` | 직선 벽·통·상자를 exact `PolygonCollider2D`로 생성, rat capsule exact, RatVisual enabled, resolver wiring 제거 | wall `4`, barrel `16`, crate `4` points; capsule `(1.2265625,0.25)`, right offset `(0.28515625,0.125)` |
| `RatHost2DTechnicalSample.unity` | builder로 재생성·저장 | RatVisual enabled/alpha 1, resolver 0, measured polygon 직렬화, scene dirty false |
| `Production2DV1AssetAndSceneTests.cs` | whole-hide 기대 제거, exact polygon/support·capsule·visibility lifecycle·legacy 보호 계약 추가 | hidden output를 PASS로 인정하지 않음 |

계측 계약이 없는 `WallCorner_BackLeft`는 임의 폴리곤으로 바꾸지 않고 기존 box를 유지했다. 계측된 직선 벽 3개는 모두 같은 exact 4-point polygon을 사용한다.

## 구현자 검증

1. `git diff --check`: owned builder/test PASS.
2. Unity `ValidateScript standard`: builder 오류 0·일반 GetComponent null-check warning 1, scene test 오류·warning 0.
3. Unity builder 실행: 성공. active scene `Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`, Play/Pause false, dirty false.
4. Unity scene smoke: Rat active true, renderer enabled true, alpha 1, resolver count 0, polygon point count wall/barrel/crate `4/16/4`, exact capsule가 저장 씬 YAML에 직렬화됨.
5. Unity Console Error: 0.
6. targeted TestRunner: 이 MCP 세션에는 TestRunner 도구가 노출되지 않았다. UI/비동기 우회 재시도 없이 fail-fast 중지했다. 결과 XML·실제 test start는 0이며 PASS로 계산하지 않는다. 조정자가 격리 실행한다.

full suite, MCP Play, matrix, capture, build는 실행하지 않았다.

## Unity lease와 복원

- agent/work/run: `unity_scene_integration` / `2026-08-02-production2d-natural-occlusion-root-fix` / `natural-occlusion-scene-r1-20260802`
- editor PID: `54432`
- baseline/final: Play `false`, Pause `false`, scene `Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`, dirty `false`
- 임시 객체: 없음
- release: `2026-08-02T04:43:01.1447846Z` 명시 반납

## 비용 proxy

| 항목 | 실제 |
| --- | ---: |
| scene 구현 역할 | 1 |
| path correction | 1 |
| fingerprint manifest | 1 |
| Unity lease acquire/renew/release | 1/1/1 |
| script validation | 2 |
| builder 실행 | 1 |
| scene smoke | 1 |
| Console Error read | 2 |
| targeted TestRunner 실제 시작/XML | 0/0 |
| correction | 0 |
| full suite/MCP Play/matrix/capture/build | 0 |
| exact token/$ | 미집계 |

## 인계 판정

- scene production·test 후보는 구현됐고 Unity lease를 release할 수 있다.
- scene targeted XML이 없으므로 기술 검증 통과 또는 완료를 주장하지 않는다.
- 조정자/독립 QA는 현재 fingerprint에서 scene targeted를 격리 1회 실행한 뒤 다음 게이트를 결정해야 한다.
- 사용자 실제 연속 WASD와 자연 부분 가림 수용은 후속 대기다.

## 격리 targeted 결과 — FAIL 보존

- evidence: `scene-targeted-r1.xml`
- 결과: `8 total / 7 passed / 1 failed`
- first blocker: `ProductionV1_UsesMeasuredPolygonFootprintsAndStableRatCapsule`에서 초기 RatHost2D와 `Barrel_A`의 `ColliderDistance2D.isOverlapped=true`
- 이 r1 후보와 XML은 PASS로 재사용하지 않는다. correction `1/2`에서 형상·테스트 완화 없이 시작 배치만 보정한 r2 후보로 이관했다.
