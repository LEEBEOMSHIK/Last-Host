# 검증 기록

## 작업 ID

`2026-08-02-production2d-natural-occlusion-root-fix`

## 검증 대상

자연 부분 가림·실제 충돌 final test-corrected candidate의 C1~C7과 S6 canonical evidence.

## 검증 담당

독립 QA/검증 에이전트 `process_harness_qa`

## 검증 에이전트 수행 이력

- 검증 에이전트: `process_harness_qa`
- 검증 요청자: 프로젝트 조정 에이전트
- 검증한 산출물: `task.md`, `artifacts/evidence-plan.md`, `artifacts/visual-footprint-measurement.md`, 정본 PNG·meta, 현재 resolver/view/builder와 운영 게이트
- `agent-activity.md` 반영 여부: 예

## 입력 자료

- `task.md`
- 이전 사용자 acceptance FAIL
- `docs/agents/loop-engineering-gates.md`

## 원래 증상 또는 완료 주장

- 원증상: 오브젝트 접촉 시 쥐 renderer 전체를 꺼 캐릭터가 사라진다.
- 완료 주장: renderer/rat를 항상 유지하면서 실제 collision·footpoint sorting·foreground alpha로 자연 부분 가림을 만든다.

## 현재 검증 revision

- 위험 등급: R2
- verification revision: `natural-occlusion-final-evidence-r1`
- candidate fingerprint: `sha256:5cd81d7c836fb2561f9f416c20adeeec00f6ef960153b8380b32c7fafbef5db6`
- S0 scope fingerprint: `sha256:b09aa2ece964e9764af6bda98f56cbb6f7a3158887b03f06b6872f36d802a0f8` (repo-relative `/` 경로 + 소문자 file SHA256, 15개 항목 사전식 정렬·LF 결합)
- canonical run_id: `natural-occlusion-final-evidence-r1-20260802`
- candidate frozen 여부: 예 — production/scene runtime freeze, 마지막 변경은 test-only fixture correction
- 마지막 production 변경 식별값: scene r2 `cd6946de...`; final test-corrected `5cd81d7c...`
- 이 검증이 마지막 변경 이후 실행됐는지: 예 — final manifest/current file mismatch 0, 기존 증거 연속성 정적 감사

## Unity single-owner lease

- S6 audit lease: 미획득 — read-only audit라 Unity/MCP 실행 0
- 마지막 유효 Play lease: `process_harness_qa`, PID `54432`, run `natural-occlusion-qa-r3-20260802`
- baseline / final Play·Pause·scene·dirty: false / false / `RatHost2DTechnicalSample` / false
- 임시 객체: QA Play r3에서 `QA_Temp*=0`, RatHost2D/Main Camera 각 1

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 계획상 예
- 구현 주체가 실행한 검증과 별도로 확인한 항목: 미실행

## 실행한 검증

| criterion ID | 유형 | 검증 방법 | run_id | 결과 | canonical 증거 | 유효/SUPERSEDED |
| --- | --- | --- | --- | --- | --- | --- |
| S0-OWNER-r1 | production 소유권 | runtime과 scene owner 분리 여부 | 미생성 | FAIL | `task.md` historical revision | SUPERSEDED by r4 |
| S0-OWNER-r2 | production 소유권 정합 | 역할·파일 표와 커밋 전 확인 항목 대조 | 미생성 | FAIL | `task.md` historical revision | SUPERSEDED by r4 |
| S0-COLLISION-ORACLE-r3 | 충돌 oracle 정합 | 금지된 과대 invisible collider가 C2를 통과하는지 최소 반례 대조 | 미생성 | FAIL | `task.md`, `artifacts/evidence-plan.md` historical revision | SUPERSEDED by r4 |
| S0-COLLISION-ORACLE-r4 | 수치 계약 정합 | exact polygons·8 normals·support delta·gap·intersection·stop stability 전수 대조 | 미생성 | PASS | `task.md`, `artifacts/evidence-plan.md`, `artifacts/visual-footprint-measurement.md`, 정본 PNG/meta SHA256 | current S0 PASS |

```text
명령 또는 확인 방법: S0 계약 전수검색, normative measurement 원문, 정본 PNG/meta와 현재 builder/view 정적 읽기 대조, SHA256 scope 고정
결과: r4 S0 PASS. Unity·테스트·MCP 실행 0회
해석: exact polygon과 8 normals, support delta -2..+1px, gap -1..+2px, core intersection 0/overlap false, 3-frame·mirror <=1px가 r3 최소 반례를 거부한다. 구현 시작만 허용한다.
```

## 검증하지 못한 항목

- 사용자 실제 연속 WASD 입력 수용
- 최종 Game View에서 자연 부분 가림·전체 캐릭터 소실 0의 사용자 육안 수용
- Windows build는 이 correction 범위에서 실행하지 않았으며 빌드 성공을 주장하지 않는다.

## 실패 또는 경고

- 이전 `7ba12df`는 기술 검증을 통과했지만 사용자가 whole-character hide를 증상 은폐로 판정해 현재 acceptance에서는 `SUPERSEDED/수정 필요`다.
- 첫 blocker: `VisualOcclusionResolver2D`의 삭제·대체를 포함한 상태형 런타임 가림 불변식과 `RatSide3FrameView`의 collider/footpoint 동기화를 Unity 씬/통합 구현 에이전트가 소유한다. 이는 `loop-engineering-gates.md`의 "상태 머신·게임플레이 수명주기는 게임플레이 구현 담당, 씬 wiring은 Unity 씬/통합 담당" 규칙과 충돌하며, 직전 사고 감사에서 확인한 런타임 소유권 역전을 반복한다.
- 필요한 최소 보정: runtime state/physics invariant는 게임플레이 구현 owner, builder·scene serialization/wiring은 씬/통합 owner로 분리하고 공동 파일의 인계 조건을 명시한다.
- r2 결과: 역할·파일 표와 순차 인계는 보정됐지만 당시 `task.md` 커밋 전 차단 조건에 scene owner만 지정한 잔존 문구가 남아 두 owner 계약과 충돌했다.
- 재분류 root cause: task template의 one-owner 잔존 문구를 부분 갱신해 역할표·production 표·커밋 게이트의 실행 계약이 불일치했다.
- r3 첫 blocker: C2는 `physical overlap 0`, `solid footprint core intersection 0`만 요구한다. 오브젝트의 visible solid footprint보다 collider를 크게 잡아 쥐를 멀리서 막는 최소 반례도 두 값이 모두 0이어서 PASS할 수 있다. 이는 금지 oracle의 `과도한 collider 확장`과 완료 주장의 `visible solid footprint와 collision 정합`을 검출하지 못한다.
- r3 필요한 최소 보정: 벽·통·상자 각각에 대해 visible solid footprint 경계와 collider footprint 경계, 허용 inset/outset 또는 접촉 간격 tolerance를 logical pixel/world unit으로 고정하고, 모든 대표 접근 방향에서 과대 돌출·과소 침투·접촉 간격 초과를 C2 FAIL로 만드는 수치 판정을 evidence matrix에 연결한다.
- r4 결과: alpha `>64`, PPU 128, asset pivot 기준 exact wall 4점·barrel 16점·crate 4점 polygon과 rat capsule `(157,32)px`, offset X `±36.5px`, Y `16px`가 원문에 고정됐다. 정본 PNG 캔버스와 meta PPU/pivot도 일치한다.
- r4 C2 판정: 대표 normal 8개 전체 support delta `-2..+1px`, visible gap `-1..+2px`, opaque-core intersection `0`, `isOverlapped=false`, 3-frame stop spread와 mirrored stop error `<=1px`를 함께 요구하므로 과대 invisible collider와 visible penetration을 모두 FAIL시킨다.
- 변경 통제: exact polygon·normal·rat capsule·tolerance 변경은 구현 전에 새 contract revision과 독립 QA를 요구한다. C1·C3~C7, ownership·shared file 0·release→handoff→acquire·무효화 계약도 유지됐다.

## fail-fast·무효화

- historical first blocker: `S0-COLLISION-ORACLE-r3` — r4 수치 계약으로 해소
- r4에서 계속 중지한 고비용 단계: production/test 수정 전이므로 Unity/컴파일/EditMode/MCP/full suite/matrix/capture 전체
- correction cycle: r1/r2 `2/2` 종료 → 재분류 1회 완료 → r3 FAIL·새 cycle `1/2` → r4 PASS로 해당 correction 해소
- 변경 뒤 무효화한 run/증거와 사유: 이전 whole-character hide acceptance는 새 S0가 대체
- superseded_by: r1/r2/r3 historical FAIL은 `natural-occlusion-s0-r4-footprint-contract` S0 PASS로 대체됐다. 실패 이력 자체는 보존한다.
- S1~S5: gameplay targeted, scene r2 targeted, QA Play r3와 candidate continuity로 PASS
- S6 전체 suite 실행 횟수: 2 — r1 `200/203` historical FAIL, test-only correction 뒤 r2 `203/203` PASS
- S7 대형 matrix: 0 — 승인된 reduced Play만 사용, 사용자 실제 WASD 수용 대기

## 비용 실행 대조 — S0 당시 historical 계획표

| 비용 항목 | 계획 예산 | 실제 수·run_id/근거 | 정상/초과/미집계 | 필요한 비용/회피 가능 비용 |
| --- | --- | --- | --- | --- |
| 실제 역할·인계 | 조정1·gameplay1·scene1·docs1·QA1·총괄1 | 조정1·QA S0 4회(3 FAIL·1 PASS), 계획 외 visual footprint analyst 1(read-only static measurement); gameplay·scene·docs·총괄 미착수 | 주의 | analyst 추가와 r4 재검토는 r3 blocker 해소에 필요 |
| 표적 검증 | 구현자 1묶음·QA 1묶음 | 0 | 계획 | freeze 전 중복 금지 |
| Unity/MCP/빌드 시작 | MCP 1 session·build 0 | 0/0 | 계획 | build 불필요 |
| full suite | freeze 후 QA 1회 | 0 | 계획 | freeze 전 실행 회피 |
| matrix/capture·artifact | reduced pairwise 1·capture 최대4 | 0 | 계획 | 전체 Cartesian·raw log commit 회피 |
| correction·무효/폐기 | r1/r2 2/2 뒤 재분류, 새 cycle 최대 2회 | S0 QA 4회(3 FAIL·r4 PASS), 계약 재분류 1회, r3 correction 1/2 해소, static measurement 1회, 고비용 실행·증거 폐기 0 | 주의 | exact polygon 수치 계약으로 r3 blocker 해소 |

- 위 표는 구현 전 S0 당시 기록으로 보존한다.
- 최종 실제 비용 판정: **과다 — 부분 회피 가능**. no-result Unity/MCP와 QA Play 하네스 correction 2회가 과다 기준에 해당한다. 정확 token/$는 미집계다.
- 최신 실제 수와 필요한/회피 가능 비용은 `docs/project-handoff/task-cost-dashboard.md`와 `artifacts/independent-qa-r4-s6-audit.md`가 소유한다.

## 최종 증거 원자성

- 대상 instance count: QA Play r3에서 RatHost2D 1, Main Camera 1, scene root 1, `QA_Temp*=0`
- stale·중복 player/controller/camera guard: PASS
- capture: 0 — 시험 배치를 사용자 실제 이동 증거로 오인하지 않도록 생략
- Console error count: 0
- scene dirty before/after: false / false
- evidence manifest: `artifacts/canonical-evidence-r1.json`
- canonical evidence와 artifact budget: 준수, raw logs 신규 생성·커밋 없음

## 게이트 판정

- QA/검증 게이트 통과 여부: **기술 검증 PASS — 사용자 실제 WASD·최종 화면 수용 대기**
- `agent-activity.md`에 QA 판정 반영 여부: 예
- 총괄 관리자 검토로 넘길 수 있는지: 예

## 완료 판단

- 기술 검증 통과 — 사용자 실제 WASD와 최종 Game View 수용 전에는 작업 완료로 선언하지 않는다.

## 사용자 수용 상태

- 사용자 직접 확인 필요: 실제 연속 WASD, 자연 부분 가림, 위치 추적성
- 확인 전 `완료` 표현 금지 여부: 예

## 완료 판단 근거

- final candidate `5cd81d7c...`에서 targeted·full EditMode·QA Play와 test-only correction 연속성을 canonical manifest로 감사했다.

## 2026-08-02 독립 QA r1 — freeze candidate `cd6946de...`

- 판정: `FAIL/완료 불가`. 제품 collision은 대표 wall/barrel/crate에서 각각 `0.080px`, `0.003px`, `0.080px` non-overlap gap으로 정지했고 renderer/root/alpha/capsule 불변도 통과했다.
- 공식 결과 재판정: gameplay `3/3 PASS`, scene r1 `7/8 historical FAIL`, scene r2 `8/8 PASS`; 현재 manifest fingerprint 일치·mismatch 0.
- first blocker: Play Y-sort 앞/뒤 검증에서 Rigidbody 위치만 바꾸고 정렬 기준 Transform을 갱신하지 않은 QA 하네스 오류로 `frontRat=0`, `object=76`이 발생했다. 제품 결함으로 확정하지 않았고 matrix·capture를 재시도하지 않았다.
- criterion: C1 PASS, C2 PASS, C3 BLOCKED, C4 NOT RUN, C5 PASS, C6 BLOCKED, C7 PASS.
- 실제 WASD: MCP 키 이벤트 주입 부재. `Host/Move` 활성과 동일 controller 경로 simulation만 PASS이며 사용자 실제 WASD는 pending이다.
- Unity: lease 1, Play 1, matrix 1, Console 1(Error 0), capture 0, build/full suite 0. 최종 Play/Pause false, scene dirty false, root 1, lease release 완료.
- 상세: `artifacts/independent-qa-r1.md`.
- 비용 상한 중단: first blocker 이후 조정자 지시에 따라 새 검증 0, 재시도 0. 상태 복원·기록만 수행했다.

## 2026-08-02 독립 QA r2 — correction 1/2

- r1은 historical FAIL/제품 결함 미확정으로 보존했다. 같은 freeze candidate `cd6946de...`, 새 run `natural-occlusion-qa-r2-20260802`를 사용했다.
- 실행 전 `Rigidbody position + root Transform → Physics2D.SyncTransforms → position/footpoint self-check → YSort.ApplySorting → renderer sorting self-check` 순서를 `artifacts/independent-qa-r2.md`에 고정했다.
- first blocker: RunCommand 보안 검사가 `System.Reflection` namespace를 거부해 하네스 실행 0. C3/C4와 nested player/camera duplicate는 미검증이다.
- fail-fast: 하네스 재작성·재시도, capture, collision/XML/policy/C1/C2/C5/C7 재실행, build/full suite/전체 matrix 모두 0.
- 종료: Console Error 0, Play/Pause false, scene dirty false, scene rootCount 1, lease release 완료. actual WASD는 사용자 대기 유지.
- 판정: `FAIL — correction 1/2`, 제품 결함 증거가 아니라 검증 하네스 권한 blocker다.

## 2026-08-02 독립 QA r3 — correction 2/2 PASS

- 같은 freeze candidate `cd6946de...`, run `natural-occlusion-qa-r3-20260802`에서 공개 API 전용 최소 하네스를 사전 정적 self-check 후 1회 실행했다.
- C3: rat/object 앞뒤 sorting 관계와 `ApplySorting()` 반환값↔renderer order 일치 PASS.
- C4: front↔behind 10회, relation 변화 20회, stationary order jitter 0, 전체 hide/alpha fade/root·visual transform drift 0.
- C6 축소 상태: RatHost2D/Main Camera 각 1, QA_Temp 0, rootCount 1, Console Error 0, final Play/Pause false, dirty false, lease release.
- collision/XML/policy/C1/C2/C5/C7, build/full suite/전체 matrix 재실행 0. capture 0.
- r1/r2는 historical FAIL로 보존하고 correction 2/2를 종료한다.
- 게이트 판정: `기술 검증 통과 — 사용자 실제 WASD·최종 화면 수용 대기`. MCP 키 이벤트 주입 부재 때문에 C6의 실제 입력 부분은 자동 PASS로 승격하지 않는다.
- 상세: `artifacts/independent-qa-r3.md`.

## 2026-08-02 독립 QA r4 — S6 evidence audit PASS

- final candidate `5cd81d7c...`의 9개 manifest 항목은 현재 파일과 mismatch 0.
- scene r2와 공유하는 non-test 6개 및 gameplay와 공유하는 runtime 2개 해시가 모두 동일해 gameplay 3/3, scene r2 8/8, QA Play r3 PASS의 유효성을 유지한다.
- stale fixture targeted 4/4와 full EditMode r2 203/203, strict valid_pass true, exit 0을 canonical evidence로 고정했다.
- full EditMode r1 200/203 FAIL은 stale Box contract historical/SUPERSEDED다.
- canonical run/manifest: `natural-occlusion-final-evidence-r1-20260802` / `artifacts/canonical-evidence-r1.json`.
- S6 audit 실행 비용: Unity/MCP/TestRunner/build/test/capture 0. 정적 manifest/diff/XML 감사만 수행했다.
- 최종 판정: `기술 검증 통과 — 사용자 실제 WASD·최종 화면 수용 대기`. 3D legacy 보존 유지.

## 2026-08-05 사용자 수용 종결

- 사용자가 자연 부분 가림 최종 화면과 쥐 본체 보존을 수용한 내용임을 재확인했다.
- 기존 canonical candidate `5cd81d7c...`, gameplay `3/3`, scene `8/8`, stale fixture `4/4`, 전체 EditMode `203/203`, QA Play r3 PASS, 총괄 2차 `내부 승인 가능`을 완료 근거로 유지한다.
- 이번 변경은 수용 상태·경로 동기화뿐이므로 새 Unity/MCP/TestRunner/build/QA 실행은 `0`이다.
- 완료 판단: **PASS — 사용자 수용 완료·보관 가능**.
