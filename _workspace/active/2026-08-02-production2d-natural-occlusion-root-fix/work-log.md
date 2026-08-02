# 작업 로그

## 작업 ID

`2026-08-02-production2d-natural-occlusion-root-fix`

## 로그

### 2026-08-02 S0 작업 접수

- 수행 내용: 사용자 acceptance FAIL을 새 R2 작업으로 분리하고 원증상·금지/허용 oracle·C1~C7·production owner·비용 예산을 고정했다.
- 확인한 자료: 이전 overlap correction QA/비용 기록, 루프 엔지니어링 게이트, 사용자 피드백
- 판단: `7ba12df`는 기술 검증을 통과했으나 whole-character hide가 사용자 원증상을 숨기는 방식이므로 현재 acceptance에는 사용할 수 없다.
- 루프 게이트 상태: S0 charter 작성 완료, 독립 QA 사전 검토 대기
- `agent-activity.md` 갱신 여부: 예
- 다음 작업: QA가 구현 전에 S0 criterion과 합성 oracle을 검토한다.

## 결정 기록

- 쥐를 비활성·투명·teleport·입력 잠금·과도한 collider 확장으로 숨기는 해결은 금지한다.
- runtime production은 게임플레이 구현 owner, builder·scene wiring은 Unity 씬/통합 owner가 분리 소유하며 공동 파일 편집을 금지한다.
- gameplay 후보·수치 계약·targeted PASS 후 명시 인계하고 scene owner가 적용·rebuild하는 순서를 지킨다.
- full suite, matrix, capture는 freeze 후보 전 실행하지 않는다.

### 2026-08-02 독립 QA S0 1차 FAIL

- 첫 blocker: 상태형 runtime 가림·collider/footpoint 불변식과 scene/builder wiring을 동일한 Unity 씬/통합 owner에 배정했다.
- 영향: 유일 실행 기준과 직전 overlap 비용 감사의 핵심 보완인 runtime 상태 소유권 분리를 다시 위반하므로 구현을 시작할 수 없다.
- 최소 보정: `VisualOcclusionResolver2D`와 runtime physics/footpoint는 게임플레이 구현 owner, builder·scene serialization/wiring은 Unity 씬/통합 owner로 나누고 공동 파일 인계 조건을 기록한다.
- fail-fast: C1~C7 후속 판정, production/test 변경, Unity·컴파일·EditMode·MCP·full suite·matrix·capture 모두 0회다.
- production 변경: 없음. correction cycle은 구현 전 charter 보정이므로 `0/2`를 유지한다.

## 열린 질문

- scene owner가 exact object polygon·serialized renderer 초기값·occluder wiring을 gameplay 계약과 함께 통합하는지는 후속 scene 검증 대기.

## 위험과 주의점

- 시각 가림만 고치고 실제 solid footprint collision을 놓치면 같은 사용자 실패가 반복된다.
- renderer enabled만 검사하고 alpha·root active·teleport를 놓치면 합성 oracle 우회가 가능하다.
- 3D legacy와 Stage2/3 dirty 변경을 잘못 포함하지 않는다.

## 게이트 진행 상태

- 작업 배정 게이트: owner·C2 수치 계약 r4 S0 PASS, scene owner 인계 가능
- 담당 산출물 게이트: gameplay 후보·targeted 3/3 PASS 완료, scene 미착수
- 에이전트 수행 이력 게이트: gameplay release·scene handoff 기록 완료
- QA/검증 게이트: r4 S0 PASS, gameplay S1~S3 targeted PASS; scene·독립 QA 대기
- 총괄 관리자 게이트: 대기
- 커밋 전 차단 조건: 구현·QA·총괄·사용자 수용 전 차단

### 2026-08-02 QA S0 r1 blocker

- 수행 내용: 독립 QA가 runtime과 scene production을 같은 씬/통합 owner에게 몰아준 ownership inversion을 first blocker로 반환했다.
- 당시 실제 비용: QA S0 1회, historical r1 cycle `1/2`. Unity/MCP/test/full suite/matrix/capture 0.
- 보정: gameplay owner가 runtime·수치 계약·순수/단위 테스트를 먼저 소유하고 targeted PASS 뒤 명시 인계한다. scene owner는 이후 builder·scene wiring·씬 계약 테스트만 적용/rebuild한다.
- 판단: r1 FAIL은 historical 기록으로 유지한다. 새 revision이 실제 통과하기 전에는 대체 또는 PASS를 기록하지 않는다.
- 다음 작업: QA S0 재접수.

### 2026-08-02 독립 QA S0 r2 FAIL

- r1 blocker의 핵심인 gameplay runtime owner와 scene integration owner 분리, 순차 인계, 공동 파일 편집 금지는 역할·소유권 표에서 반영됐다.
- 첫 blocker: 같은 `task.md`의 커밋 전 차단 조건은 당시 scene owner만 지정해 gameplay/scene 두 owner 계약과 충돌했다.
- 영향: 실행자가 역할 표와 커밋 전 확인 중 어느 계약을 따라야 하는지 고정되지 않아 ownership inversion 재발을 차단하지 못한다.
- correction cycle: `2/2`. 패치 누적·구현 시작을 중지하고 소유권 계약과 작업 상태를 재분류한다.
- 미실행: production·정책 수정, Unity·컴파일·테스트·MCP·full suite·matrix·capture 모두 0회

### 2026-08-02 correction 2/2 계약 재분류

- 상태: `수정 필요 — 재분류 완료·natural-occlusion-s0-r3-reclassified 사전 검토 대기`, 구현 금지.
- root cause: task template의 one-owner 잔존 문구를 첫 보정에서 전수 검색하지 않고 부분 갱신해 역할표·production 표·커밋 게이트가 서로 다른 실행 계약을 유지했다.
- R2 유지 근거: 사용자에게 직접 보이는 가림 상태이며 runtime visibility·collider direction·footpoint와 serialized footprint·builder·scene wiring이 결합된다.
- 계약 정규화: gameplay owner 1 + scene owner 1 + docs owner 1, shared file 0. gameplay release → 명시 handoff → scene acquire/apply 순서를 고정했다.
- revision: r1/r2는 historical FAIL로 보존한다. 새 revision `natural-occlusion-s0-r3-reclassified`는 correction cycle `0/2`에서 시작하며 실제 통과 뒤에만 이전 실패를 superseded로 표시한다.
- 실제 비용: QA S0 2회 FAIL, correction 2회 뒤 재분류 1회. Unity/test/MCP/full suite/matrix/capture/build 0.
- 회피 가능 비용: 첫 보정 전 stale 역할·소유권 문구 전수 검색 누락으로 r2 검토가 추가됐다.
- 비용 판정: `주의 — 저비용 S0에서 재분류 완료 대기`. correction 2회 뒤 재분류하지 않았다면 과다 조건에 해당한다.
- 다음 작업: 독립 QA의 r3 문서 사전 검토. PASS 선기록과 production·정책 변경은 금지한다.

### 2026-08-02 독립 QA S0 r3 FAIL

- 소유권·shared file 0·release→handoff→acquire·무효화·커밋 게이트는 같은 gameplay/scene/docs 계약으로 정합했다. stale single-owner 표현은 현재 계약에서 발견되지 않았다.
- 첫 blocker: C2는 physical overlap과 visible solid core intersection이 0인지만 보므로, 과대 invisible collider로 쥐를 오브젝트보다 멀리서 막아도 PASS할 수 있다.
- 최소 반례: collider를 visible solid footprint보다 크게 확장한다. 쥐는 일찍 멈추고 실제 overlap/intersection은 0이므로 현재 C2 기대값을 만족하지만 금지 oracle과 충돌한다.
- 최소 보정: 오브젝트 3종별 visible/collider footprint 경계, 허용 inset/outset·접촉 간격 tolerance, 대표 접근 방향별 수치 판정과 C2 evidence 연결을 추가한다.
- correction cycle: r3 `1/2`. r1/r2 historical FAIL은 유지하며 r3 PASS 전 superseded 처리하지 않는다.
- fail-fast/비용: QA S0 누적 3회. production·정책 변경, Unity/test/MCP/full suite/matrix/capture/build는 모두 0회다.

### 2026-08-02 r3 수치 계약 보정·r4 접수 준비

- 상태: `수정 필요 — natural-occlusion-s0-r4-footprint-contract 재검토 대기`, 구현 금지.
- visual footprint analyst가 원본 PNG를 `alpha > 64`, PPU 128, asset pivot 기준으로 읽기 전용 정적 계측했다.
- 원문 보존: `artifacts/visual-footprint-measurement.md`에 wall 4-point, barrel 16-point, crate diamond의 exact px/world polygon과 normal set, rat capsule `(157,32)px`·offset X `±36.5px`, Y `16px`를 기록했다.
- C2 계약: support delta `-2..+1px`, visible gap `-1..+2px`, opaque-core intersection 0, ColliderDistance overlap false, 3-frame stop spread와 mirrored stop error 각각 `<=1px`.
- 구현 baseline: wall/barrel/crate `PolygonCollider2D`, rat capsule world size `(1.2265625,0.25)`, offset X `±0.28515625`, Y `0.125`, frame switch resize 금지.
- 변경 통제: baseline 수치를 바꾸려면 QA contract revision과 독립 사전 검토가 필요하다.
- 비용: 계획 외 visual footprint analyst 1회, static measurement 1회. QA S0 r3 FAIL 1회·새 cycle `1/2`; Unity/test/MCP/full/matrix/capture/build 0, exact token/$ 미집계.
- 판정: `주의 — 저비용 S0에서 수치 계약 재검토 대기`. r1/r2/r3 historical FAIL은 유지하며 r4 통과 전 PASS·superseded를 기록하지 않는다.

### 2026-08-02 독립 QA S0 r4 PASS

- 정적 대조: exact wall 4점·barrel 16점·crate 4점 polygon, rat capsule `(157,32)px`/offset `±36.5,+16px`, alpha `>64`, pivot 좌표계와 PPU 128 방법이 normative 원문에 고정됐다. 정본 PNG 캔버스와 meta pivot/PPU도 일치했다.
- C2: normal 8개 전체 support delta `-2..+1px`, gap `-1..+2px`, core intersection 0/overlap false, frame·mirror stop error `<=1px`로 r3의 과대 invisible collider 최소 반례와 visible penetration을 모두 거부한다.
- 계약 보존: C1~C7, gameplay/scene/docs 전용 owner, shared file 0, release→handoff→acquire와 하류 근거 무효화가 유지됐다. 수치 변경은 새 contract revision·독립 QA가 필요하다.
- 판정: `natural-occlusion-s0-r4-footprint-contract` S0 PASS. r1/r2/r3 historical FAIL은 r4로 superseded했고 r3 correction `1/2`를 해소했다.
- scope fingerprint: `sha256:b09aa2ece964e9764af6bda98f56cbb6f7a3158887b03f06b6872f36d802a0f8`
- 비용: QA S0 누적 4회(3 FAIL·1 PASS), static measurement 1회. Unity/test/MCP/full suite/matrix/capture/build 0, exact token/$ 미집계. 판정 `주의` 유지.
- 다음 작업: gameplay owner가 stable capsule·whole-hide 제거/대체·footpoint 후보와 targeted PASS를 만들고 release한다.

### 2026-08-02 freeze candidate 독립 QA r1

- candidate manifest와 현재 파일을 재해시해 fingerprint `cd6946de...`, mismatch 0을 확인했다.
- 공식 XML을 새로 실행하지 않고 strict 재판정했다: gameplay 3/3 PASS, scene r1 7/8 historical FAIL, scene r2 8/8 PASS.
- single-owner lease(PID 54432)에서 Play 1회, reduced matrix 1회 실행했다. wall/barrel/crate 대표 접근은 overlap 없이 각각 0.080/0.003/0.080px gap으로 정지했고 renderer enabled/alpha 1/root/capsule 불변을 유지했다.
- Y-sort 확인은 QA 위치 설정이 Rigidbody와 footpoint Transform을 동기화하지 않아 first blocker가 됐다. 같은 세션에서 수정·재시도하지 않았고 capture 0, Console Error 0 후 Stop·scene clean·lease release를 확인했다.
- 판정: C1/C2/C5/C7 PASS, C3/C6 BLOCKED, C4 NOT RUN. 제품 collision 회귀는 검출되지 않았으나 기술 게이트 종결은 불가하다.
- 비용 상한 중단: 조정자 지시 뒤 추가 검증·재시도 0. 상태 복원과 packet 기록만 수행했다.

### 2026-08-02 독립 QA r2 correction 1/2

- r1의 Rigidbody/Transform self-check 누락을 보정하는 실행 순서를 새 artifact에 사전 고정했다.
- 동일 candidate, 새 lease/Play에서 축소 하네스를 한 번 요청했으나 RunCommand가 `System.Reflection`을 unauthorized namespace로 거부했다. 하네스 본문 실행 0이다.
- first blocker에서 즉시 중지해 재작성·재시도·capture 및 기존 PASS 항목 재검증을 하지 않았다.
- 실제 비용: lease 성공 1, Play 1, RunCommand 요청 1/실행 0, Console 1(Error 0), Stop 1, final state/scene 1묶음, capture/build/full/XML/matrix 0.
- 최종 Play/Pause false, dirty false, rootCount 1, lease Released. C3/C4와 nested duplicate는 미검증, actual WASD 사용자 대기다.

### 2026-08-02 독립 QA r3 correction 2/2 PASS

- 저장 scene에서 `RatHost2D/Visual`, `Rigidbody2D`, `SpriteRenderer`, 양쪽 `YSortSprite2D`, `Main Camera` 공개 경로를 정적으로 확인했다.
- 공개 API만 쓴 최소 Play 하네스가 1회에 PASS했다: 앞/뒤 sorting 관계, 10회 왕복, relation change 20, stationary jitter 0, renderer enabled/alpha1/root·visual transform 안정.
- RatHost2D/Main Camera 각 1, QA_Temp 0, rootCount 1, Console Error 0. Stop 뒤 Play/Pause false, dirty false, lease Released.
- 비용: lease1, Play1, RunCommand1 PASS, Console1, Stop1, final state/scene1묶음, capture/build/full/XML/전체 matrix0. 기존 PASS 항목 재실행0.
- correction 2/2 종료. 기술 검증은 통과했으며 actual WASD·최종 화면은 사용자 수용 대기다.

### 2026-08-02 S6 canonical evidence audit

- 새 실행 없이 final manifest, current file SHA, scene/gameplay 이전 manifest, XML 결과와 test-only diff를 읽기 대조했다.
- final `5cd81d7c...`는 scene r2의 production/scene/package/version 6개와 gameplay runtime 2개가 동일하고, stale scene fixture test 하나만 보정된 후보다.
- `stale-fixture-targeted-r1.xml` 4/4 PASS와 `full-editmode-r2.xml` 203/203 PASS(valid_pass true, exit0)를 canonical evidence로 확정했다.
- `full-editmode-r1.xml` 200/203 FAIL은 stale BoxCollider2D 기대 3건으로 historical/SUPERSEDED다.
- canonical run은 `natural-occlusion-final-evidence-r1-20260802`, manifest는 `artifacts/canonical-evidence-r1.json` 하나다.
- 감사 비용: 정적 audit 1묶음, Unity/MCP/TestRunner/build/test/capture 0. 판정은 기술 검증 통과·사용자 수용 대기다.

### 2026-08-02 gameplay runtime 후보와 검증 차단

- `VisualOcclusionResolver2D`가 더 이상 renderer enabled/color, GameObject active, transform, 입력 상태를 변경하지 않도록 whole-character hide 상태 전이를 제거했다. 기존 serialized scene이 컴파일되는 동안 sorting refresh와 공개/정적 API는 호환용으로 보존했다.
- `RatSide3FrameView`는 caller가 이전 크기를 넘겨도 승인된 캡슐 size `(1.2265625,0.25)`, offset X `±0.28515625`, Y `0.125`를 적용하며 세 프레임과 좌우 mirror에서 resize하지 않는다.
- gameplay-owned EditMode 계약 테스트 3개를 새로 추가했다. 외부 disabled renderer를 강제로 enable하지 않는 negative control과 root/visual local transform 불변도 포함했다.
- 정적/스크립트 validation과 Console Error 0까지 확인한 후보 fingerprint는 `7cefea1d56632fd633a15d2574a0f56167607411a89ceb329f1c447bd037ca25`, run_id는 `natural-occlusion-gameplay-r1-20260802`다.
- first blocker: lease를 확보한 현재 Editor에서 단 한 번의 targeted TestRunner bundle을 요청했으나 MCP가 사용자 상호작용 요구 작업을 거부해 XML이 생성되지 않았다. fail-fast로 다른 Unity 실행과 후속 고비용 검증을 중단했다.
- Unity lease는 PID `54432`, 원래 `RatHost2DTechnicalSample`, Play/Pause false, dirty false 상태로 반납했다. scene owner handoff는 targeted PASS 전까지 차단한다.
- 상세 구현·명령·비용·인계 수치는 `artifacts/gameplay-implementation-r1.md`를 따른다.

### 2026-08-02 gameplay targeted PASS·scene owner 해제

- 같은 후보를 격리 복제본에서 공식 EditMode로 한 번 실행해 `gameplay-targeted-r1.xml`의 `3/3 PASS`, 실패·skip·inconclusive 0, Unity exit 0을 확보했다.
- 저장소 결과 판정 도구도 XML을 `valid_pass=true`로 확인했다. 앞선 MCP no-result는 PASS에서 제외했다.
- gameplay production·test 편집과 live Unity lease를 release했다. scene owner는 캡슐 `(1.2265625,0.25)`, offset X `±0.28515625`, Y `0.125`, renderer 외부 소유·root 불변 계약을 변경하지 않고 builder·scene integration을 시작한다.
- full suite, MCP Play, matrix, capture, build는 여전히 0이며 scene 통합·독립 QA·사용자 수용은 후속 단계다.

### 2026-08-02 증상 은폐 방지 정책 r1

- gameplay report와 S0 r4의 negative control을 일반 정책으로 승격했다. renderer/object disable, alpha 0, 이동·입력 우회, error swallow, 과대 invisible collider, hidden-output 기대 테스트는 원인 레이어 증명 없이는 완료 수정이 아니다.
- 유일 실행 기준은 `docs/agents/loop-engineering-gates.md`에 두고 사용자 가이드, 2D production guide, pixel style·Unity verification reference와 `AGENTS.md`에는 역할별 요약과 링크만 추가했다.
- workaround는 사용자 명시 승인·임시 표시·제거 조건을 요구하고, 증상만 바뀐 후보는 `temporary` 또는 `blocked`로 판정한다.
- `7ba12df`는 사용자 수용 FAIL의 역사적 교훈으로만 기록했으며 특정 커밋을 전역 정책의 영구 전제로 만들지 않았다.
- production·씬·테스트·수치 변경 및 Unity/MCP/test/build 실행 없음. 정책 r1 독립 QA 대기.

### 2026-08-02 measured-footprint scene integration r1

- 실제 production builder 경로를 조정자 승인으로 정정하고 scene owner lease를 획득했다.
- 직선 벽 3개·통·상자의 box를 S0 exact polygon으로 교체했다. 쥐 캡슐은 `(1.2265625,0.25)`, right offset `(0.28515625,0.125)`로 저장했으며 renderer enabled/alpha 1, resolver count 0이다.
- 저장 씬을 builder로 재생성했고 active scene·Play/Pause·dirty 상태를 baseline과 동일하게 유지했다. scene smoke `4/16/4` points, Console Error 0.
- hidden-output 기대 scene test를 제거하고 polygon/support·capsule·visibility lifecycle·legacy 보호 계약으로 교체했다.
- candidate `ed8e9caf5f0e0f38fd05fbd11ff3151e54caf1e5fe128232357d8759b1836f8d`, run `natural-occlusion-scene-r1-20260802`.
- MCP TestRunner가 노출되지 않아 targeted 실제 시작/XML 0에서 fail-fast 중지했다. 조정자 격리 targeted 실행 전 기술 검증 통과·완료 주장은 금지한다.

### 2026-08-02 scene targeted r1 FAIL → correction r2

- 격리 targeted r1은 8개 중 7 PASS, 1 FAIL이었다. initial rat capsule과 Barrel_A exact polygon이 `isOverlapped=true`인 first blocker를 검출했다.
- 충돌체 축소·test 삭제·renderer hide 없이 correction `1/2`를 시작했다.
- 침투량 약 `7.28px`를 계측한 뒤 initial rat spawn만 `+8px Y` 이동했다. 수정 후 barrel gap은 약 `0.72px`, overlap false로 S0 허용 범위다.
- r2 candidate `cd6946deff7ecf1e1f4e4aed6c2fd532f1a97c5e895bb79de6fe00b4bee49385`, run `natural-occlusion-scene-r2-20260802`.
- scene 저장·dirty false·Console Error 0을 확인했다. 조정자 격리 targeted r2 외 고비용 검증은 실행하지 않는다.

### 2026-08-02 full EditMode r1 stale contract → correction 2/2

- full EditMode r1은 `203/200/3` FAIL이며 세 blocker 모두 기술 샘플 scene test의 BoxCollider2D stale 기대였다. historical FAIL로 보존한다.
- `RatHost2DTechnicalSampleSceneTests.cs`만 PolygonCollider2D 정본 계약으로 이관했다. props Box 0, polygon 2, Barrel/Crate points 16/4, visibility/resolver negative control을 추가했다.
- 기존 실제 120-step collision·penetration·stop·left-side 판정은 유지했다.
- candidate `5cd81d7c836fb2561f9f416c20adeeec00f6ef960153b8380b32c7fafbef5db6`, run `natural-occlusion-stale-test-correction-r1-20260802`.
- ValidateScript/diff check만 PASS. Unity/TestRunner/scene/build/full suite는 실행하지 않았고 조정자 격리 targeted를 기다린다.

### 2026-08-02 프로젝트 총괄 관리자 1차 read-only 감사 — 반려

- final candidate `5cd81d7c...` manifest 9개와 canonical evidence 6개의 현재 SHA mismatch 0, fingerprint 재계산 일치, full EditMode `203/203`, whole-hide 제거, exact polygon/capsule/spawn gap, legacy 3D diff 0을 정적으로 확인했다.
- 기술 후보와 QA 증거 자체는 내부 승인 가능한 수준이다. 실제 WASD와 최종 Game View 자연 부분 가림은 사용자 수용 대기로 올바르게 남아 있다.
- first blocker: 공유 `current-task-board.md`, `CURRENT.md`, `task.md`와 `handoff.md`·`agent-activity.md` 요약이 S0/gameplay 시작·scene 미착수·이전 candidate·비용 `주의` 상태를 혼재해 final evidence와 불일치한다.
- 판정: `반려 — 상태 문서 정합 수정 필요`. production/scene/test/policy 변경은 요구하지 않는다.
- 본 감사 비용: 정적 파일·diff·manifest/XML 읽기 1묶음, Unity/MCP/TestRunner/build/capture 0, 커밋 0.
- 다음 작업: 조정자가 상태-only 동기화 후 총괄 read-only 재감사를 요청한다. 상세는 `director-review.md`를 따른다.

### 2026-08-02 최종 기술 상태·사용자 수용 경계 동기화

- 정본 manifest `artifacts/canonical-evidence-r1.json`을 다시 읽어 final fingerprint `5cd81d7c836fb2561f9f416c20adeeec00f6ef960153b8380b32c7fafbef5db6`와 gameplay `3/3`, scene `8/8`, stale fixture `4/4`, full EditMode `203/203`, QA Play r3 PASS를 대조했다.
- Console Error 0·scene dirty false·3D legacy 보존을 유지하고, 실제 연속 WASD와 최종 화면 확인은 사용자 수용 대기로 분리했다.
- 공유 현황판과 CURRENT의 구현 대기 문구를 `기술 검증 통과 — 사용자 수용 대기`로 갱신했다. 다음 후보 목록은 바꾸지 않고 현 작업 사용자 확인을 최우선으로 고정했다.
- 중앙 비용 현황은 이미 `과다 — 부분 회피 가능`과 canonical run을 반영해 수정하지 않았다.
- 상태 전용 동기화만 수행했다. Unity·MCP·TestRunner·빌드·테스트·capture 실행 0, production·scene·test·policy 변경 0, staging·commit·push 0.

### 2026-08-02 프로젝트 총괄 관리자 2차 read-only 재감사 — 내부 승인 가능

- 1차 반려 뒤 교정된 task/handoff/activity/current board/CURRENT/cost의 현재 요약을 verification과 canonical manifest에 다시 대조했다.
- 이전 gameplay 후보·scene 미착수·full suite 0·비용 `주의`는 현재 요약에서 0건이며 historical FAIL/no-result는 역사 섹션과 superseded·비용 기록에만 보존됐다.
- final candidate `5cd81d7c...`, canonical run `natural-occlusion-final-evidence-r1-20260802`, gameplay `3/3`, scene `8/8`, stale fixture `4/4`, full EditMode `203/203`, QA Play r3/r4 evidence audit, 비용 `과다 — 부분 회피 가능`이 일치한다.
- manifest/current mismatch 0·fingerprint 일치, canonical evidence SHA mismatch 0, legacy scene/camera diff 0, HEAD `2eff18d`, staged 0을 확인했다.
- 판정: `내부 승인 가능 — 사용자 실제 WASD·최종 화면 수용 대기`. 사용자 수용 전 완료·보관·커밋 금지는 유지한다.
- 본 재감사 실행: Unity/MCP/TestRunner/build/test/capture 0, commit/push 0.
