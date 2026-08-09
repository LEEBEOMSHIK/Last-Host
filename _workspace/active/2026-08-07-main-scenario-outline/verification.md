# 검증 기록

## 작업 ID

`2026-08-07-main-scenario-outline`

## 검증 대상

- 전체 게임 시나리오·화면 흐름·튜토리얼 초안
- 내러티브 색인
- Task 4 Step 6 승인 오프닝·기원 공개·세균/운반 숙주 계약 전파 revision

## 검증 담당

- 작성자 정적 대조: 메인 시나리오 디렉터 에이전트
- 기존 revision 독립 QA: `main_scenario_qa`
- 내부 승인: 프로젝트 총괄 관리자 에이전트

## 실제 수행·검증 이력

| 역할/에이전트 | 실제 수행 | 산출물·판정 |
| --- | --- | --- |
| 프로젝트 조정 | R2 범위·S0 작성, 색인·상태판 통합, 정적 구조 검사 | PASS |
| 기획 정리 에이전트 | 전체 시나리오 production 문서 작성 | 500줄 초안 제출 |
| 독립 정적 QA | C1~C5 최초 검토, correction 1 재검토 | 최초 FAIL → correction 1 PASS |
| 메인 시나리오 디렉터 | Task 4 Step 6 production revision 작성, C6~C15 작성자 정적 대조 | `author-main-scenario-opening-sync-20260810-r2` PASS |
| 독립 정적 QA | Task 4 Step 6 current candidate 첫 검토 | `qa-main-scenario-opening-sync-20260810-001` FAIL — HEAD 상태 증거 불일치 first blocker |
| 메인 시나리오 디렉터 | QA-001 first blocker 상태-only 교정 | `author-main-scenario-opening-sync-state-correction-20260810-r2` PASS — QA 재진입 대기 |
| 독립 정적 QA | correction 2 candidate C1~C15 재진입 검증 | `qa-main-scenario-opening-sync-20260810-002` PASS·blocker 0·수정 0 |
| 프로젝트 총괄 관리자 | Task 4 Step 6 최종 read-only 감사 | `director-main-scenario-opening-sync-final-audit-20260810-003` 내부 승인 가능·blocker 0·최소 수정 0 |
| 프로젝트 조정 | 승인 candidate 선별 커밋·push | content `6867b98` (`docs: sync opening into main scenario`) origin/main 반영, 신규 QA·총괄 0 |

- 검증 에이전트: 이전 revision `main_scenario_qa`; 현재 canonical `qa-main-scenario-opening-sync-20260810-002` PASS
- 검증 요청자: 프로젝트 조정 에이전트
- 검증한 산출물: 이전 revision `docs/design/narrative/main-scenario-outline.md`, `docs/design/narrative/README.md`; 현재 작성자 run `docs/design/narrative/main-scenario-outline.md`
- 조건부 R3 분리 이력 생성 사유·반영 여부 / 없으면 `미생성`: 미생성

## 입력 자료

- `docs/design/game-design-summary.md`
- `docs/prototype/official/rat-host-prototype.md`
- `AGENTS.md`
- `docs/design/narrative/opening/opening-cinematic-origin.md`
- `docs/design/narrative/opening/opening-shot-spec.md` v12
- `docs/design/narrative/opening/opening-cinematic-production-plan.md`
- `docs/design/hosts/host-map-transfer-route.md`

## 원래 증상 또는 완료 주장

- 전체 게임의 처음부터 엔딩까지 이어지는 시나리오·화면·튜토리얼 기준 문서가 없다.

## 현재 검증 revision

- 위험 등급: R2
- verification revision: Task 4 Step 6 state correction revision 2
- candidate fingerprint: scenario `8316E5BA83A5D8FB7C880E895FAD895AF3E5C9EB706123534B0D8CC430BF81B0`; narrative index 변경 없음
- 입력 SHA-256: origin `1A998992C1881C008A14775282B02EFADC8E029CC82D0FC168A12203C2629110`; shot spec v12 `2A1854340F09975C62F8683F0F3A9E42991D15494BD5B628DC25908FD0DFD9B6`; production plan `A50BB54AA049510D687EFF45509230424A2744541E73C455F349A37C4C625B08`; host route `36C87D4489C16B0E2291AB0364E8F59011C13DC461CA494D6D8D9EC3224E67A1`
- production author run_id: `author-main-scenario-opening-sync-20260810-r2`; QA first run: `qa-main-scenario-opening-sync-20260810-001` FAIL·SUPERSEDED; state correction author run_id: `author-main-scenario-opening-sync-state-correction-20260810-r2`; canonical independent QA run_id: `qa-main-scenario-opening-sync-20260810-002` PASS
- candidate frozen 여부: 예 — 작성자 PASS candidate를 독립 QA 전 동결
- 마지막 production 변경 시각/식별값: Task 4 Step 6 오프닝 계약 최소 전파 revision 2
- 이 검증이 마지막 production 변경 이후 실행됐는지: 예 — QA-001 뒤 production 변경 0, 상태 문서 correction 2 뒤 QA-002 C1~C15 PASS와 총괄 003 최종 감사 완료
- current-state JSON 대조: 해당 없음
- capability route / wrapper preflight: 문서 정적 검토 / 해당 없음
- attempt ledger 연속 실패 / reclassification ID: 0 / 해당 없음

## Unity single-owner lease

- lease owner: 해당 없음
- editor PID / scene: 해당 없음
- 획득·해제 시각: 해당 없음
- baseline / final Play·Pause·scene·dirty: 해당 없음
- 임시 객체 유무: 없음

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 예 — QA-001이 작성자와 분리되어 상태 증거 first blocker를 발견했고 QA-002가 correction candidate를 재검증했다.
- 구현 주체가 실행한 검증과 별도로 확인한 항목: QA-002는 C1~C15, scenario full SHA, 17구간·필수 필드 23×5, 6장 baseline 동일, baseline `6347445`, self-reference 0, 링크 5/5, placeholder 0, diff 0, existing md 5, untracked·Unity·code·asset 0을 확인했다.

## 실행한 검증

| criterion ID | 유형 | 검증 방법 | run_id | 결과 | canonical 증거 | 유효/SUPERSEDED |
| --- | --- | --- | --- | --- | --- | --- |
| C1 | 성공 | 첫 실행부터 엔딩 후까지 목차·본문 정적 대조 | `qa-main-scenario-static-20260807-c1` | PASS | 시나리오 3장·5장·9장 | 현재 production 변경으로 SUPERSEDED |
| C2 | 경계 | 쥐 프로토타입과 승인 표기 대조 | `qa-main-scenario-static-20260807-c1` | PASS | 시나리오 2장·4.3절·5.11절·13장 | 현재 production 변경으로 SUPERSEDED |
| C3 | 성공 | 5.1~5.17의 화면·행동·튜토리얼·연출·전환 항목 대조 | `qa-main-scenario-static-20260807-c1` | PASS | 17구간, 필수 표기 각 23회 | 현재 production 변경으로 SUPERSEDED |
| C4 | negative control | 구현 미승인·금지 해석 문구와 Git 변경 범위 확인 | `qa-main-scenario-static-20260807-c1` | PASS | 시나리오 1장·13장, Unity 변경 0 | 현재 production 변경으로 SUPERSEDED |
| C5 | 수명주기 | narrative README 상대 링크와 대상 파일 존재 확인 | `qa-main-scenario-static-20260807-c1` | PASS | `docs/design/narrative/README.md` | 현재 production 변경으로 SUPERSEDED |
| C6 | 성공 | 승인 사건 토큰 순서·17구간 수 대조 | `author-main-scenario-opening-sync-20260810-r2` | PASS | 독립 3씬→복합 환승·차등 공백→C01→심야 노동자→비말·배수→각성→커스터마이징→E03/T01, 17구간 | 작성자 증거 — QA 대기 |
| C7 | 생물학 경계 | 직접 감염 금지와 숙주 용어 대조 | `author-main-scenario-opening-sync-20260810-r2` | PASS | 동물세포 직접 감염 금지, 세균 숙주·운반 숙주 구분, 비말·세척수/배수 미세환경 | 작성자 증거 — QA 대기 |
| C8 | 공개 경계 | 기원 공개 canonical 링크·오프닝 spoiler 대조 | `author-main-scenario-opening-sync-20260810-r2` | PASS | shot spec 링크, 연구소·전쟁·군사화·제작 주체 오프닝 폭로 금지 | 작성자 증거 — QA 대기 |
| C9 | 편집 계약 | OPEN 비트 복제·33 ID 요약 수 대조 | `author-main-scenario-opening-sync-20260810-r2` | PASS | `OPEN-B` 복제 0, 33 ID·Gate S 요약 1곳 | 작성자 증거 — QA 대기 |
| C10 | 시간 경계 | 최초 도입 시간·C01 가독 대조 | `author-main-scenario-opening-sync-20260810-r2` | PASS | 10~20초 값 0, 숏·패널·총시간 미확정, 최대 2줄·최소 5초 | 작성자 증거 — QA 대기 |
| C11 | downstream 정합 | 구간 3~8·프롤로그·Q01/Q02/Q12·길이표·승인 경계 대조 | `author-main-scenario-opening-sync-20260810-r2` | PASS | 대표 `곤충→쥐→포식성 조류`, 두 조류 역할과 host route canonical 링크 보존 | 작성자 증거 — QA 대기 |
| C12 | 범위 | HEAD와 현재 6장 byte-content 대조 | `author-main-scenario-opening-sync-20260810-r2` | PASS | 쥐 프로토타입 단독 흐름 불변 | 작성자 증거 — QA 대기 |
| C13 | negative control | Git diff·untracked·확장자 대조 | `author-main-scenario-opening-sync-20260810-r2` | PASS | Unity diff 0, 코드·에셋 diff 0, 새 파일 0 | production 범위 의미 유효, HEAD 상태 증거 SUPERSEDED |
| C14 | 수명주기 | 상대 링크 대상·placeholder 대조 | `author-main-scenario-opening-sync-20260810-r2` | PASS | opening 2종·host route 대상 존재, TODO/TBD/FIXME 0 | 작성자 증거 — QA 대기 |
| C15 | 원자성 | SHA-256·`git diff --check` | `author-main-scenario-opening-sync-20260810-r2` | 부분 유효 | scenario `8316E5BA...81B0` production 의미 유효, 당시 HEAD 상태 증거 stale | 상태 증거 SUPERSEDED |
| STATE-HEAD | 상태 증거 | 실제 HEAD/origin-main과 상태 문서 대조 | `qa-main-scenario-opening-sync-20260810-001` | FAIL | 실제 `6347445`; board·verification·cost가 `f9e8bd0`을 현재값으로 표기 | SUPERSEDED by QA-002 |
| STATE-HEAD | 상태 증거 | baseline 문구·production SHA·5파일 scope·diff·Unity0 대조 | `author-main-scenario-opening-sync-state-correction-20260810-r2` | PASS | Task 4 Step 6 작업 시작 baseline `6347445` origin/main의 precommit candidate; production SHA 불변 | QA-002에서 확인 |
| C1~C15 | canonical 독립 QA | production 의미·구조·상태·수명주기 전체 대조 | `qa-main-scenario-opening-sync-20260810-002` | PASS | full SHA `8316E5BA83A5D8FB7C880E895FAD895AF3E5C9EB706123534B0D8CC430BF81B0`; 17구간·23×5·6장 baseline 동일·self-reference 0·links 5/5·placeholder/diff 0·existing md 5·untracked/Unity/code/asset 0 | 유효 canonical, blocker 0 |
| FINAL-AUDIT | 총괄 최종 감사 | QA-002 canonical·범위·미완료 후속 작업 대조 | `director-main-scenario-opening-sync-final-audit-20260810-003` | 내부 승인 가능 | 17구간·쥐 범위·생물학·조류·공개·시간·범위 적합, blocker 0·최소 수정 0 | 유효, Task 4 Step 6 한정 |

```text
명령 또는 확인 방법: PowerShell token-order·section equality·링크·placeholder·diff scope 검사, Get-FileHash SHA-256, git diff --check
결과: C6~C15 작성자 PASS, Segments 17, 쥐 프로토타입 6장 불변, UnityDiffCount 0, UntrackedCount 0, 링크 대상 존재, placeholder 0, diff check exit 0
해석: production 의미와 bytes를 유지한 correction 2 candidate가 canonical QA-002와 총괄 003 최종 감사를 통과한 뒤 content `6867b98`로 origin/main에 반영됐다. 후속 제작 완료를 뜻하지 않는다.
```

## 검증하지 못한 항목

- 사용자 시나리오 내용 수용과 구체 보완
- Task 5 Step 6 실제 연결 측정과 storyboard·image·asset·animatic·audio·code·Unity 제작·구현

## 실패 또는 경고

- 최초 QA에서 `현재 모드` HUD를 프로토타입 승인 범위로 과대 표기한 C2 오류가 발견됐다.
- 최초 QA 입력에 narrative README가 없어 C5가 미검증이었다.
- correction 1에서 승인 상태를 정정하고 색인 링크를 확인해 C1~C5가 PASS했다.
- Task 4 Step 6 production revision 자체는 계획된 변경으로 correction을 증가시키지 않았다. 이후 QA-001이 actual HEAD `6347445`와 stale `f9e8bd0` 현재값의 불일치를 발견해 상태-only correction 2를 수행했다.
- QA-002는 correction 2 candidate를 수정 없이 C1~C15 PASS·blocker 0으로 판정했고 QA-001을 SUPERSEDED했다.

## fail-fast·무효화

- Task 4 Step 6 first blocker: `qa-main-scenario-opening-sync-20260810-001` — actual HEAD/origin-main `6347445`와 board·verification·cost의 current HEAD `f9e8bd0` 표기 불일치
- blocker 발견 뒤 중지한 고비용 단계: Unity/MCP/build는 계획부터 0이며 실행하지 않음
- correction cycle: 2/2
- 변경 뒤 무효화한 run/증거와 사유: 최초 QA 판정은 C2 수정 전 revision이라 correction 1에서 SUPERSEDED됐고, `qa-main-scenario-static-20260807-c1`도 Task 4 Step 6 production 변경 전 fingerprint라 현재 candidate에는 SUPERSEDED
- superseded_by: `qa-main-scenario-opening-sync-20260810-002` canonical PASS
- S1~S5 한 revision 통과 여부: 예 — QA-002 C1~C15 PASS·blocker 0
- S6 전체 suite 실행 허용/실행 횟수: 해당 없음 / 0
- S7 대형 matrix 실행 허용/실행 횟수: 해당 없음 / 0
- low-level runner token / 직접 Run 차단 확인: 해당 없음
- isolated cache marker / Library reuse / cleanup 확인: 해당 없음

## 비용 실행 대조

| 비용 항목 | 계획 예산 | 실제 수·run_id/근거 | 정상/초과/미집계 | 필요한 비용/회피 가능 비용 |
| --- | --- | --- | --- | --- |
| 실제 역할·인계 | 기획1→조정1→총괄1 | 기존 기획1·조정1·독립 QA 최초+correction1·총괄1; Task 4 Step 6 작성자1→QA-001 FAIL→상태 교정 작성자1→QA-002 PASS→총괄 003 내부 승인 | 주의 | QA first blocker 교정과 canonical 재진입·최종 감사 |
| 표적 검증 | 정적 대조 1묶음 | 기존 구조 검사1·독립 QA 최초1+correction1; Task 4 작성자 C6~C15 1회, QA-001 FAIL1, 상태 교정 작성자 PASS1, QA-002 PASS1 | 주의 | stale HEAD blocker를 바로잡고 동일 candidate를 확인한 필요한 재대조 |
| Unity/MCP/빌드 시작 | 0 | 0 | 정상 | 없음 |
| full suite | 0 | 0 | 정상 | 없음 |
| matrix/capture·artifact | 0 | 0 | 정상 | 없음 |
| correction·무효/폐기 | 0 | correction 2/2; 원 author production 의미 유효, self-referential HEAD 상태 증거만 SUPERSEDED | 주의 | QA-001 blocker 해소를 위한 상태-only correction |

- 비용 판정: 주의 — correction 2/2, 고비용 실행 없음
- 같은 fingerprint 중복·first blocker 뒤 고비용·no-result Unity·2회 미재분류·추가 역할·비원자 폐기 확인: 해당 없음
- `docs/project-handoff/task-cost-dashboard.md` 갱신·독립 대조 여부: 작성자 production1·QA-001 FAIL1·상태 교정 작성자1·QA-002 PASS1·총괄 003 내부 승인1로 동기화

## 최종 증거 원자성

- 대상 Root/GlobalObjectId/instance count: 해당 없음
- stale·중복 player/controller/camera guard: 해당 없음
- 캡처와 sidecar의 run/fingerprint 일치: 해당 없음
- Console error count: 해당 없음
- scene dirty before/after: 해당 없음
- evidence manifest: production 문서 1개, 기존 narrative 색인 변경 0, author run·SHA·diff 기록 1묶음
- canonical evidence와 artifact budget 준수: 예, 별도 artifact 미생성

## 게이트 판정

- QA/검증 게이트 통과 여부: PASS — canonical `qa-main-scenario-opening-sync-20260810-002` C1~C15, blocker 0
- 조건부 R3 분리 이력에 QA 판정 반영 여부 / 없으면 `미생성`: 미생성
- 총괄 관리자 검토로 넘길 수 있는지: 예 — 최종 감사 완료

## 프로젝트 총괄 관리자 판정

- 판정: 내부 승인 가능 — `director-main-scenario-opening-sync-final-audit-20260810-003`, blocker 0, 최소 수정 0
- 근거: canonical QA-002, production SHA, 17구간과 쥐 범위, 생물학·조류·기원 공개·시간·범위 경계, existing md 5와 Unity·code·asset·untracked 0을 대조했다.
- 승인 범위·사용자 수용 대기: Task 4 Step 6 상위 시나리오 전파에 한정한다. Task 5 Step 6 실제 측정과 storyboard·image·asset·animatic·audio·code·Unity는 미완료·별도 승인이다.

## 완료 판단

- Task 4 Step 6 content `6867b98` origin/main 반영 완료

## 사용자 수용 상태

- 사용자 직접 확인 필요: 예
- 확인 전 `완료` 표현 금지 여부: Task 4 Step 6 content 반영은 완료됐으나 Task 5 Step 6과 후속 제작 완료로 확대하지 않음

## 완료 판단 근거

- candidate `8316E5BA...81B0`은 canonical QA-002 C1~C15 PASS·blocker 0·수정 0과 총괄 003 내부 승인 가능·blocker 0·최소 수정 0을 충족했고 content `6867b98`로 origin/main에 반영됐다.

## 최종 상태

- 완료/보류/승인 대기: Task 4 Step 6 content `6867b98` origin/main 반영 완료 — 다음 승인 게이트 대기
- 완료 경로와 Git 상태: `_workspace/active/2026-08-07-main-scenario-outline/`; 작업 시작 baseline `6347445`, content `6867b98` origin/main 반영
