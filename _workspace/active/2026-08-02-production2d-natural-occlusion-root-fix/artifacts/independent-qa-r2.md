# 독립 QA r2 — correction 1/2

## 실행 전 고정

- r1 상태: `historical FAIL`, 제품 결함 미확정. 삭제하거나 PASS로 덮지 않는다.
- candidate fingerprint: `sha256:cd6946deff7ecf1e1f4e4aed6c2fd532f1a97c5e895bb79de6fe00b4bee49385` — r1과 동일한 freeze candidate.
- run_id: `natural-occlusion-qa-r2-20260802`
- 범위: C3 정렬 관련 부분, C4 자연 부분 가림/전체 hide 금지, C6 Console·dirty·root duplicate·복원.
- 재실행 금지: wall/barrel/crate collision, XML, policy, C1/C2/C5/C7, build, full suite, 전체 matrix.
- actual WASD: MCP 키 이벤트 주입 부재로 사용자 확인 대기 유지.
- Play/capture 예산: Unity lease 1, Play session 1, 축소 하네스 1, Console 조회 1, capture 최대 1.

## r2 하네스 순서

1. Play 진입 뒤 root/player/camera 단일성과 `QA_Temp* == 0`, renderer active/enabled/alpha를 먼저 확인한다.
2. C3 앞/뒤 위치마다 `Rigidbody2D.position`과 논리 root `Transform.position`을 같은 값으로 지정한다.
3. `Physics2D.SyncTransforms()`를 호출하고, Rigidbody position·root Transform·Y-sort footpoint world Y가 목표 위치와 일치하는지 self-check한다. 하나라도 불일치하면 즉시 FAIL한다.
4. self-check 통과 뒤에만 `YSortSprite2D.ApplySorting()`을 호출한다. 이는 LateUpdate의 동등 경로다.
5. `SpriteRenderer.sortingOrder == ApplySorting 반환값`을 확인하고, rat가 오브젝트보다 앞일 때 `ratOrder > objectOrder`, 뒤일 때 `ratOrder < objectOrder`를 확인한다.
6. 각 상태에서 rat/root/visual active, renderer enabled, alpha 1, visual local transform 불변을 확인한다. 전체 hide·alpha 0·teleport/clamp·오브젝트 disable은 사용하지 않는다.
7. C4는 같은 오브젝트 경계의 앞↔뒤 왕복 10회를 위치/동기화/정렬 경로로 반복하고 sorting 관계가 매회 정확히 한 번 뒤집히며 renderer/alpha/root가 유지되는지 확인한다. collision은 재검증하지 않는다.
8. 성공 시 대표 경계 상태를 최대 1장 캡처한다. 캡처 전 root/player/camera 단일성, `QA_Temp* == 0`, 같은 run/fingerprint를 재확인한다.
9. Console Error를 1회 조회하고 Stop한다. 최종 Play/Pause false, scene dirty false, root duplicate 0을 확인한 뒤 lease를 해제한다.
10. 첫 blocker가 발생하면 즉시 중지하며 같은 session에서 수정·재시도·추가 capture를 하지 않는다.

## 결과

`FAIL — correction 1/2 first blocker, 제품 결함 미확정`

- first blocker: Unity RunCommand 보안 검사가 `System.Reflection` namespace를 허용하지 않아 하네스가 컴파일·실행되기 전에 거부됐다.
- 영향: private serialized footpoint/target renderer를 읽어 self-check하려던 방법이 실행되지 않았다. C3 정렬, C4 10회 왕복·전체 hide 금지, C6 player/camera duplicate는 새 증거가 없다.
- fail-fast: 공개 API 또는 다른 접근으로 같은 세션에서 하네스를 다시 작성·실행하지 않았다. capture 0, build/full suite/XML/전체 matrix 0.
- 보존된 r1 결과: C1/C2/C5/C7 PASS, wall/barrel/crate collision PASS, 정책/XML PASS는 재실행하지 않았다.
- C6 종료 상태: Console Error 0, Play false, Pause false, `RatHost2DTechnicalSample`, scene dirty false, scene rootCount 1. nested player/camera duplicate는 하네스 미실행으로 미검증.
- actual WASD: MCP 키 이벤트 주입 부재로 사용자 확인 대기.
- Unity lease: PID `54432`, 획득 `2026-08-02T05:08:40.6252076+00:00`, 해제 `2026-08-02T05:10:15.6968398+00:00`.
- 실제 비용: lease acquire 실제 성공 1회(잘못된 bool/parameter 바인딩 명령 2회는 상태 변경 없음), Play 1회, RunCommand 요청 1회·실행 0, Console 조회 1회, Stop 1회, final state/scene 확인 1묶음, capture 0, 재시도 0.
- correction 상태: `1/2`. 다음 correction이 허용되면 reflection 없이 공개 component/transform 경로로 footpoint를 특정하거나, 검증 전용 접근을 production/test 변경 없이 구성할 수 있는지 먼저 정적 확인해야 한다.
