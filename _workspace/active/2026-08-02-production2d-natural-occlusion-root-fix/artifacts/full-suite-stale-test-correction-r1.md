# Full EditMode stale scene-test contract correction r1

## 후보 식별

- correction: `2/2`
- historical full suite: `full-editmode-r1.xml`
- historical result: `203 total / 200 passed / 3 failed`
- run_id: `natural-occlusion-stale-test-correction-r1-20260802`
- candidate fingerprint: `5cd81d7c836fb2561f9f416c20adeeec00f6ef960153b8380b32c7fafbef5db6`
- manifest: `full-suite-stale-test-candidate-r1.json`

## r1 실패 분류

세 실패는 모두 `RatHost2DTechnicalSampleSceneTests`가 과거 `BoxCollider2D` 형상을 고정한 stale test contract였다.

1. `E01_SampleSceneContainsRequiredIsolatedTwoDimensionalContract`: props 아래 `BoxCollider2D` exactly 2 기대, 실제 0.
2. `YSortPropFootprintBlocksRigidbodyMovementWithoutPenetration("Barrel_A")`: BoxCollider2D 조회 null.
3. 같은 테스트의 `Crate_A`: BoxCollider2D 조회 null.

production·scene 결함으로 오분류하지 않고 r1 full FAIL은 historical evidence로 보존한다.

## 변경

소유 파일 하나만 수정했다.

- `UnityProject/Assets/_Project/Tests/EditMode/TechnicalSample2D/RatHost2DTechnicalSampleSceneTests.cs`

E01 계약을 다음과 같이 더 강하게 이관했다.

- props root 하위 `BoxCollider2D` 0
- props root 하위 `PolygonCollider2D` exactly 2
- Barrel_A/Crate_A 각각 non-trigger PolygonCollider2D exactly 1, path exactly 1
- point count Barrel `16`, Crate `4`
- Rigidbody2D 없음, SpriteRenderer와 YSortSprite2D 존재
- renderer enabled true, alpha 1
- prop/scene `VisualOcclusionResolver2D` 0

실제 충돌 테스트는 obstacle 조회와 helper 파라미터만 `PolygonCollider2D`로 바꿨다. 아래 기존 판정은 삭제·완화하지 않았다.

- 실제 `120` physics steps
- 60-step 이후 정지 변화 `<=1/64`
- signed distance `>=-0.001`
- obstacle 중심보다 왼쪽에 정지

hidden output 기대, collider tolerance 완화, production/scene/build 수정은 없다.

## 검증과 비용

- `git diff --check`: PASS
- 대상 test `ValidateScript standard`: 오류 0, 일반 GetComponent null-check warning 1
- Unity TestRunner/MCP scene/Play/build/full suite: 0
- 새 XML: 0. 조정자가 격리 복제본에서 실패 fixture targeted를 1회 실행한다.
- 역할 1, test file 1, fingerprint manifest 1, correction `2/2`, exact token/$ 미집계
- Unity lease: 미획득. Editor/scene을 조작하지 않았다.
- test file 소유권 release: `2026-08-02T05:22:55.3384346Z`

## 판정

- stale contract correction 후보 생성은 완료했다.
- targeted PASS 전 기술 검증 통과·전체 suite PASS·완료를 주장하지 않는다.
- r1 full FAIL은 현재 통과 수에 합산하지 않는다.
