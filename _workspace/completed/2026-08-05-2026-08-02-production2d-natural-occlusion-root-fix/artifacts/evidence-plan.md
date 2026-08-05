# 검증 증거 계획

## 목적

쥐 전체를 숨기는 증거가 아니라, renderer/rat active 유지·실제 collision·자연 부분 가림을 같은 freeze 후보와 Unity lease에서 증명한다.

## criterion→evidence 계획

| criterion | canonical evidence | 예산 |
| --- | --- | --- |
| C1 | active/enabled/alpha/hide-transition sidecar | 1 manifest 묶음 |
| C2 | exact object polygon × normal 8개 × 3 frames × 좌우 mirror support/gap matrix | CSV 1개 + `visual-footprint-measurement.md` |
| C3 | 앞·경계·뒤 상태 원자 캡처 | PNG 최대 4 + sidecar |
| C4 | 경계 왕복 10회 결과 | matrix/sidecar에 통합 |
| C5 | lifecycle 관련 EditMode XML | XML 1개에 통합 |
| C6 | MCP Play 상태·Console·dirty sidecar | 1 session 기록 |
| C7 | 보호 diff 목록 | verification 보고에 통합 |

## 원자성·stale guard

- single-owner lease와 candidate fingerprint/run_id를 먼저 고정한다.
- 실제 rat root·camera는 각각 기대 인스턴스 1개, `QA_Temp*` 0을 확인한다.
- 캡처와 sidecar는 같은 root·frame·camera 상태에서 생성한다.
- raw Unity logs는 로컬 감사용이며 Git commit 금지다.

## fail-fast와 비용 제한

- S1~S5 첫 blocker에서 full suite·matrix·capture를 중단한다.
- 구현자 targeted 1묶음, QA targeted+관련 1묶음만 계획한다.
- full suite는 freeze 후보에서 QA 1회만 허용한다.
- 3-object reduced pairwise matrix 1회, canonical capture 최대 4장이다.
- build는 실행하지 않는다.
- r1/r2 correction `2/2`는 종료했고 계약 재분류 1회를 수행했다. r3 collision oracle FAIL 뒤 새 cycle `1/2`는 `natural-occlusion-s0-r4-footprint-contract` PASS로 해소됐다. 승인된 gameplay owner 범위의 구현 시작을 허용한다.

## production owner와 순차 인계

1. 게임플레이 구현 owner가 `VisualOcclusionResolver2D.cs`의 whole-hide 제거/대체, `RatSide3FrameView.cs`의 collider·방향·footpoint runtime 불변식, 필요 helper와 순수/단위 테스트를 소유한다.
2. gameplay 후보·수치 계약과 targeted PASS를 기록하고 Unity lease를 release한 뒤 scene owner에게 명시 인계한다.
3. Unity 씬/통합 owner는 `RatHost2DProductionSampleSceneBuilder.cs`, `RatHost2DTechnicalSample.unity`, serialized collider/occluder wiring과 씬 계약 테스트만 소유한다.
4. scene owner는 gameplay 수치 계약을 임의 변경하지 않고 apply/rebuild한다. 공동 파일 편집은 금지한다.

- QA S0 r1은 ownership inversion, r2는 task 내부 owner 계약 불일치, r3는 과대 invisible collider 반례 허용으로 FAIL했다. 세 historical FAIL은 r4 S0 PASS로 superseded했으며 실패 이력은 보존한다.
- S0 r1/r2/r3와 수치 계약 보정 단계의 Unity/MCP/test/full suite/matrix/capture/build 실행은 모두 0이다. visual footprint analyst의 읽기 전용 static measurement만 1회다.

## C2 normative numeric contract

- normative source: `visual-footprint-measurement.md`의 wall 4-point polygon, barrel 16-point polygon, crate diamond와 px/world 좌표
- normal set: wall/crate는 face normal 4개 + normalized adjacent-normal sums 4개, barrel은 cardinal/diagonal 8개
- `support(P,n)=max(dot(p,n))`
- `Δ=support(collider,n)-support(reference,n)`
- support delta: `-2px <= Δ <= +1px` (`-0.015625 <= Δ <= +0.0078125 world`)
- visible gap: `-1px..+2px` (`-0.0078125..+0.015625 world`)
- opaque-core intersection: `0`
- `ColliderDistance2D.isOverlapped`: `false`
- 3-frame stop spread: `<=1px` (`<=0.0078125 world`)
- mirrored stop error: `<=1px` (`<=0.0078125 world`)
- `gap > +2px`는 invisible collider FAIL, `gap < -1px` 또는 opaque-core intersection은 penetration FAIL이다.
- implementation baseline: wall/barrel/crate `PolygonCollider2D`; rat CapsuleCollider2D size `(1.2265625,0.25)`, offset X `±0.28515625`, Y `0.125`, frame switch resize 금지
- 위 후보 수치는 S0 baseline이다. owner가 변경하려면 QA contract revision과 독립 사전 검토가 필요하다.

## state/invariant 증거 owner

| state/invariant | evidence owner | 증거 범위 | 무효화 규칙 |
| --- | --- | --- | --- |
| runtime visibility·collider direction·footpoint | gameplay owner 1 | runtime·순수/단위 표적 근거 | 변경 시 gameplay와 모든 후속 scene·QA 근거 무효 |
| serialized object footprint·builder·scene·scene tests | scene owner 1 | serialization·wiring·scene 표적 근거 | gameplay handoff 뒤 생성하며 변경 시 scene와 후속 QA 근거 무효 |
| policy docs | docs owner 1 | 구현 확정 뒤 상태·운영 정합 | acceptance 계약 변경 시 S0부터 무효 |

- shared file: 0. 서로 다른 owner가 같은 production 또는 test 파일을 편집하지 않는다.

## 금지 증거

- renderer disabled·alpha 0·rat inactive 상태를 PASS 화면으로 사용하는 것
- teleport·입력 잠금·과도한 collider 확장 상태
- stale/중복 rat·camera·QA 임시 객체가 포함된 캡처
- final/final-v2 파일명만으로 정본을 판정하는 것
