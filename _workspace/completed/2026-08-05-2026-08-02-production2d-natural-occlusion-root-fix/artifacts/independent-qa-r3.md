# 독립 QA r3 — correction 2/2

## 실행 전 고정

- r1/r2: historical FAIL, 제품 결함 미확정으로 보존.
- candidate fingerprint: `sha256:cd6946deff7ecf1e1f4e4aed6c2fd532f1a97c5e895bb79de6fe00b4bee49385`.
- run_id: `natural-occlusion-qa-r3-20260802`.
- 범위: C3 공개 Y-sort 앞/뒤 관계, C4 10회 왕복 중 renderer 전체 hide 0·root/visual transform 안정, C6 duplicate·Console·dirty·복원.
- 재실행 금지: collision, XML, policy, C1/C2/C5/C7, build, full suite, 전체 matrix.
- actual WASD: MCP 키 이벤트 주입 부재로 사용자 확인 대기.

## 사전 정적 self-check

- 저장 scene 직접 대조:
  - `RatHost2D`: `Rigidbody2D`, `RatHost2DController`, active.
  - child `Visual`: `YSortSprite2D`, `SpriteRenderer`, `RatSide3FrameView`, local position `(0,0,0)`.
  - `Barrel_A`: `YSortSprite2D`.
  - `Main Camera`: 존재.
- RunCommand는 공개 API만 사용한다: `GameObject.Find`, `Transform.Find`, `GetComponent<T>`, `FindObjectsByType<T>`, `Rigidbody2D.position`, `Transform.position`, `Physics2D.SyncTransforms`, `YSortSprite2D.ApplySorting`, `SpriteRenderer.sortingOrder/enabled/color`.
- 금지 항목 0: `System.Reflection`, private API/field, invoke, dynamic member access.
- 하네스 순서: baseline active/enabled/alpha/root → Rigidbody/root 위치 설정 → `Physics2D.SyncTransforms()` → Rigidbody/root/visual world·local self-check → 공개 `ApplySorting()` → return/renderer sortingOrder self-check → 앞/뒤 관계 확인.
- 단일 representative는 `Barrel_A`; 초기 behind 뒤 `front↔behind` 10회로 관계 변화 20회와 stationary ApplySorting 반복 안정성을 확인한다.
- first blocker면 즉시 중지하며 correction 2/2 이후 추가 시도는 없다.

## 결과

`PASS — 기술 검증 통과, 사용자 실제 WASD·최종 화면 수용 대기`

- 공개 API RunCommand 컴파일·실행 PASS.
- C3 정렬: rat가 `Barrel_A`보다 앞일 때 rat sorting order가 더 크고, 뒤일 때 더 작은 관계를 확인했다. `ApplySorting()` 반환값과 `SpriteRenderer.sortingOrder`가 매번 일치했다.
- C4: 초기 behind 뒤 front↔behind 10회 왕복, sorting relation 변화 정확히 20회. 같은 위치에서 `ApplySorting()`을 반복해 order 변화 0, renderer enabled `true`, alpha `1.000`, rat/visual active `true`를 전 구간 유지했다.
- transform self-check: 각 위치에서 `Rigidbody2D.position == root Transform.position == target`, visual world가 root offset을 따라가고 visual local position/scale 및 root scale이 변하지 않았다.
- C6 상태: `RatHost2D=1`, `Main Camera=1`, `QA_Temp*=0`, scene rootCount `1`, Console Error `0`.
- 캡처: `0/1`. 시험 배치는 실제 연속 이동 장면을 대표하지 않으므로 stale/misleading 시각 증거를 만들지 않았다.
- 종료: Play false, Pause false, `RatHost2DTechnicalSample`, scene dirty false, rootCount 1, lease Released.
- actual WASD: MCP 키 이벤트 주입 부재로 사용자 확인 대기. 기술 PASS가 사용자 실제 입력·자연스러운 화면 수용을 대체하지 않는다.

## correction·비용

- correction `2/2` 종료. r1은 Rigidbody/Transform self-check 누락, r2는 unauthorized `System.Reflection`으로 historical FAIL을 유지한다. r3가 제품 변경 없이 검증 하네스만 공개 API로 보정해 통과했다.
- 실제 비용: 정적 scene self-check 1묶음, lease 1, Play 1, RunCommand 1 PASS, Console 1, Stop 1, final state/scene 1묶음, capture 0.
- 재실행 0: collision, XML, policy, C1/C2/C5/C7, build, full suite, 전체 matrix.
- candidate fingerprint는 r1과 동일한 `cd6946de...`; production/scene/test/policy 변경 없음.
