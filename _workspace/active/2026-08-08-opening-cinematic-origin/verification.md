# 검증 기록

## 작업 ID

`2026-08-08-opening-cinematic-origin`

## 검증 대상

- 오프닝 시네마틱·주인공 기원 설계
- 승인 게이트별 제작 태스크 계획
- 오프닝 편집 비트 후보·기원 공개 장부
- 오프닝 하위 문서 색인

## 검증 담당

- 작성자 검증: v12 Gate S 메인 시나리오 디렉터 `author-opening-gate-s-editorial-contract-032` PASS — C1~C15, 33 ID·15필드, 승인 그룹·내부 경계, master/axis/scale/layer 매핑, 시간·placeholder·링크·SHA256·diff 대조
- 독립 QA: `qa-opening-gate-s-editorial-contract-033` PASS — C1~C15, blocker 0. 이전 QA-030은 사용자 Gate S 결정 뒤 SUPERSEDED
- 프로젝트 총괄 판정: `director-opening-gate-s-final-audit-034` 내부 승인 가능·최소 수정 0

## 실제 수행·검증 이력

| 역할/에이전트 | 실제 수행 | 산출물·판정 |
| --- | --- | --- |
| 프로젝트 조정 | 사용자 승인 범위와 R2 S0 C1~C6 고정 | 작업 배정서 작성 |
| 픽셀아트 시네마틱 연출 | 파일을 수정하지 않고 오프닝·기원·제작 계획 초안 제출 | 조정자가 사용자 원순서와 문구를 반영해 통합 |
| 프로젝트 조정 | production 3개 작성·통합, C1~C6·자리표시자·diff·해시 검사 | `author-static-001` PASS |
| QA/검증 | C1~C6, 사용자 원장면, 무기화 정보 안전 경계, 자리표시자·모순·diff 독립 대조 | `qa-opening-static-001` PASS, blocker 없음 |
| QA/검증 | 최종 fingerprint의 C1~C6·원장면·안전 경계·자리표시자·모순·diff 재대조 | `qa-opening-static-002` PASS, blocker 없음 |
| 프로젝트 총괄 | 프로젝트 메시지·인간/노동자 묘사·박테리오파지·무기화 안전·승인 경계 감사 | 내부 승인 가능, 최소 수정 없음 |
| 메인 시나리오 디렉터 | 사용자 승인 `복합 환승 노드형`의 다중 접점·세 생활권·차등 결과·파지 모티프 분리와 다음 결정 순서를 두 production 문서에 반영 | 자체 요구 대조·`git diff --check` PASS |
| 프로젝트 조정 | v2 C1~C7, 자리표시자, diff와 production SHA256 재검증 | `author-opening-composite-node-003` PASS |
| 프로젝트 총괄 | v2 production fingerprint·QA canonical·오해 방지·안전·프로토타입/후속 승인 경계를 read-only 감사 | 내부 승인 가능, 최소 수정사항 없음 |
| 메인 시나리오 디렉터 | 사용자 결정에 따라 현재 시작·반전 문구를 제작 기준으로 가확정하고 조건부 재검토 원칙과 다음 내레이션 결정을 반영 | `author-opening-provisional-copy-005` PASS |
| QA/검증 | v3 가확정 문구·조건부 재검토·다음 단계, production fingerprint, correction 2/2와 superseded 이력을 독립 재대조 | `qa-opening-provisional-copy-007` PASS, blocker 없음 |
| 메인 시나리오 디렉터 | 사용자 승인 최소 내레이션의 운용 원칙과 Task 2 Step 5 완료·Step 6 다음 결정을 production과 R2 패킷에 반영 | `author-opening-minimal-narration-008` PASS |
| QA/검증 | v4 C1~C8 production과 task의 현재 QA 범위·suite·완료 기준을 독립 대조 | `qa-opening-minimal-narration-009` FAIL — task의 현재 범위 3곳이 C1~C7로 잔존 |
| QA/검증 | 동기화된 v4 C1~C8 범위, production fingerprint, 최소 내레이션 원칙, correction 2/2와 superseded 이력을 독립 재대조 | `qa-opening-minimal-narration-010` PASS, blocker 없음 |
| 메인 시나리오 디렉터 | 사용자 승인 중앙 문구 한국어 최대 2줄·완전 가독 최소 5초와 페이드·접근성·다국어 경계, Task 2 Step 6 완료·다음 결정을 반영 | `author-opening-copy-readability-011` PASS |
| QA/검증 | v5 C1~C9 가독성 기준, production fingerprint, Task 2 Step 6·다음 결정, correction 2/2와 superseded 이력을 독립 재대조 | `qa-opening-copy-readability-012` PASS, blocker 없음 |
| 메인 시나리오 디렉터 | 사용자 승인 심야 복합 환승시설 청소 구역의 동선·인물·비원인성·환경 노출·파지 이동·인적 정보·후속 승인 경계와 Task 3 Step 1·다음 결정을 반영 | `author-opening-night-transit-cleaning-013` PASS |
| QA/검증 | v6 C1~C10 청소 구역 계약, production fingerprint, Task 3 Step 1·다음 결정, correction 2/2와 superseded 이력을 독립 재대조 | `qa-opening-night-transit-cleaning-014` PASS, blocker 없음 |
| 프로젝트 총괄 | v6 Task 3 Step 2와 노동자·기관 비난 방지 계약을 재감사 | `director-opening-step2-reclassification-015` FAIL — `보호 부족`이 보호 결핍·기관 과실을 선확정, 수정 필요·재분류 |
| 메인 시나리오 디렉터 | 승인된 중립 Step 2 문구와 결과물 후 사용자 수정 게이트를 origin·plan에 반영하고 v7 재분류 주기 시작 | `author-opening-step2-neutralization-016` PASS |
| QA/검증 | v7 중립 Step 2·미완료 체크·가안 수정 게이트, production fingerprint, 재분류 0/2와 superseded 이력을 독립 재대조 | `qa-opening-step2-neutralization-017` PASS, blocker 없음 |
| 메인 시나리오 디렉터 | 사용자 승인 `마감 동선 추적형 + 성실한 행동 한 비트`의 순서·소품 제한·금지 표현·주인공 식별 시점과 Task 3 Step 2 완료·다음 결정을 반영 | `author-opening-step2-closing-route-018` PASS |
| QA/검증 | v8 C1~C11 production fingerprint, 승인 순서·소품 제한·금지 표현·주인공 식별 시점, task·verification 기록 정합, diff·placeholder·Unity 비변경을 독립 재대조 | `qa-opening-step2-closing-route-019` PASS, blocker 없음 |
| 메인 시나리오 디렉터 | 사용자 승인 레이어형 혼합 커스터마이징의 초기·해금 항목, 수치 분리, 고정 실루엣·성장 언어, 기본형→선택 외형 시간 연속성, 저장·재시청·회상·성장 레이어와 제작/후속 승인 경계를 반영 | `author-opening-customization-continuity-020` PASS |
| QA/검증 | v9 C1~C12 production과 확정·미결정 목록, Task 3 Step 3 완료·Step 4 다음 결정, task·verification 기록 정합을 독립 대조 | `qa-opening-customization-continuity-021` FAIL — origin 미결정 목록에 확정된 커스터마이징 범위·이름 입력 여부 잔존 |
| 메인 시나리오 디렉터 | 확정된 커스터마이징 항목을 origin 미결정 목록에서 제거하고 첫 튜토리얼 선택을 다음 항목으로 정렬, fingerprint와 기록 보정 1/2 동기화 | `author-opening-customization-decision-sync-022` PASS |
| QA/검증 | v9 sync1 C1~C12 확정·미결정 목록, Step 4 다음 결정, production fingerprint, task·verification 기록 정합, correction 1/2, diff·placeholder·Unity 비변경을 독립 재대조 | `qa-opening-customization-decision-sync-023` PASS, blocker 없음 |
| 메인 시나리오 디렉터 | 사용자 승인 세균 감염 접속형의 비트·용어·무손실 실패 보조와 신규 콘텐츠 안전 시점 강제 큐·항목별 팝업·실습·도감·저장·입력/접근성/다국어·범위 경계를 반영 | `author-opening-bacterial-tutorial-notice-024` PASS |
| QA/검증 | v10 C1~C13 튜토리얼·생물학/용어·실패·알림/저장/접근성·범위 경계, production fingerprint, task·verification 기록 정합, correction 1/2, diff·placeholder·Unity 비변경을 독립 재대조 | `qa-opening-bacterial-tutorial-notice-025` PASS, blocker 없음 |
| 프로젝트 총괄 | v10 canonical QA-025, 승인 범위·생물학/안전·Task 4 후속 승인 경계를 read-only 최종 감사 | 내부 승인 가능, 최소 수정 없음 |
| 메인 시나리오 디렉터 | 사용자 승인 Task 4를 최종 숏 수가 아닌 33개 편집 비트 후보·15필드·병합/분리 경계·실제 측정 시간 산식·기원 공개 장부로 상세화하고 README·plan·R2 packet을 동기화 | `author-opening-edit-beats-reveal-ledger-026` PASS |
| QA/검증 | v11 C1~C14, 33개 편집 비트·15필드·병합/분리 경계·시간 비확정/산식·기원 공개 장부, production fingerprint, task·verification 기록 정합, correction 1/2, diff·placeholder·Unity 비변경을 독립 재대조 | `qa-opening-edit-beats-reveal-ledger-027` PASS, blocker 없음 |
| 프로젝트 총괄 | v11 canonical QA-027 뒤 Task 5 파일 수명주기와 총 러닝타임 산식을 read-only 감사 | `director-opening-edit-beats-contract-audit-028` 수정 필요 — 기존 shot spec overwrite 위험, hold·전환·입력 경계 이중 계산 위험 |
| 메인 시나리오 디렉터 | Task 5를 기존 shot spec Modify로 교정하고, 첫 프레임~T01 입력 해제 실제 경과에서 커스터마이징 체류만 빼는 단일 산식·breakdown 비재가산·건너뛰기 별도 보고·입력 이벤트를 동기화 | `author-opening-edit-beats-contract-correction-029` PASS |
| QA/검증 | correction 2/2 C1~C14, 기존 shot spec Modify 수명주기, 단일 비상호작용 러닝타임 산식·breakdown 비재가산·건너뛰기 분리·입력 이벤트, production fingerprint, task·verification 기록 정합, diff·placeholder·Unity 비변경을 독립 재대조 | `qa-opening-edit-beats-contract-correction-030` PASS, blocker 없음 |
| 프로젝트 총괄 | QA-030 canonical 증거와 v11 production fingerprint를 read-only 최종 감사 | `director-opening-edit-beats-final-audit-031` 내부 승인 가능, 최소 수정 0 |
| 메인 시나리오 디렉터 | 사용자 Gate S 보호형 압축 편집 그룹, 내부 beat boundary, master composition·camera axis·scale band·layer delta와 접근성·입력 경계를 기존 shot spec Modify 방식으로 반영 | `author-opening-gate-s-editorial-contract-032` PASS |
| QA/검증 | v12 C1~C15, 33 ID·15필드, 승인 그룹·내부 경계와 master/axis/scale/layer 전수 매핑, 시간·placeholder·링크·SHA256·diff·Unity 비변경을 독립 재대조 | `qa-opening-gate-s-editorial-contract-033` PASS, blocker 0 |
| 프로젝트 총괄 | QA-033 canonical 증거와 v12 Gate S fingerprint·승인 경계·후속 Step 6 상태를 read-only 최종 감사 | `director-opening-gate-s-final-audit-034` 내부 승인 가능, 최소 수정 0 |

## 원래 증상 또는 완료 주장

- 완료 주장: 승인된 오프닝과 돌연변이 기원을 검토 가능한 설계 문서와 승인 게이트별 제작 태스크로 확인할 수 있다.

## 현재 검증 revision

- 위험 등급: R2
- verification revision: `opening-origin-r2-v12-gate-s-editorial-contract`
- candidate fingerprint: README `61DC1AFE3E5CF856FB053356514D46A90785A25A9E5318A73BFDB74C9759FF3B`; origin `1A998992C1881C008A14775282B02EFADC8E029CC82D0FC168A12203C2629110`; plan `A50BB54AA049510D687EFF45509230424A2744541E73C455F349A37C4C625B08`; shot spec `2A1854340F09975C62F8683F0F3A9E42991D15494BD5B628DC25908FD0DFD9B6`
- author run_id: `author-opening-gate-s-editorial-contract-032`
- canonical run_id: `qa-opening-gate-s-editorial-contract-033`
- candidate frozen 여부: 예 — v12 canonical QA PASS와 총괄 034 내부 승인 가능 상태로 precommit 동결하며 production 의미 변경 금지
- capability route / wrapper preflight: Markdown 정적 검증 / Unity preflight 불필요

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 예 — v12 작성자 032와 독립 QA 033 분리
- 이전 독립 검증 항목: C1~C13, 회사원 점심·학생 등교·어머니 점심 준비, 무기 제작 구체 정보 금지, 자리표시자·모순·diff
- correction 전 v11 독립 검증 항목: C1~C14, 33개 ID·15필드·병합/분리 경계·시간 비확정·실제 측정 산식·공개 장부·링크·SHA256·Unity 비변경 — QA-027 PASS 뒤 총괄 028로 SUPERSEDED
- correction 2/2 독립 재검증 항목: 기존 shot spec Modify 수명주기, 단일 비상호작용 러닝타임 산식과 breakdown 비재가산·건너뛰기/입력 경계, C1~C14·링크·SHA256·Unity 비변경 — PASS, blocker 없음
- v12 독립 검증 예정 항목: C1~C15, 33 ID·15필드, 승인 그룹·내부 경계와 master/axis/scale/layer 전수 매핑, scale band bitmap zoom 금지, 접근성·재시청·건너뛰기·입력, 시간·placeholder·링크·SHA256·diff·Unity 비변경
- v12 독립 검증 결과: `qa-opening-gate-s-editorial-contract-033` C1~C15 PASS, blocker 0

## 실행한 검증

| criterion ID | 유형 | 검증 방법 | run_id | 결과 | canonical 증거 | 유효/SUPERSEDED |
| --- | --- | --- | --- | --- | --- | --- |
| C1~C6 | 작성자 정적 검사 | 문구·범위 정규식 대조, 파일 존재, `git diff --check`, 자리표시자, SHA256 | `author-static-001` | PASS | production 문서 3개와 위 fingerprint | 유효 |
| C1~C6 | 독립 정적 QA | 사용자 원요청·S0·production·계획 대조와 `git diff --check` | `qa-opening-static-001` | PASS, blocker 없음 | 최초 QA 회신과 이전 fingerprint | SUPERSEDED — 형식 정리 전 fingerprint |
| C1~C6 | 독립 정적 QA | 최종 fingerprint·사용자 원장면·생물학/무기화 안전·자리표시자·모순·`git diff --check` 재대조 | `qa-opening-static-002` | PASS, blocker 없음 | v1 QA 회신과 이전 fingerprint | SUPERSEDED — 사용자 승인 의미 변경 |
| C1~C7 | v2 작성자 정적 검증 | 독립 3씬·복합 환승 노드·세 생활권·차등 결과·단일 발원/확정 사망/파지 원인 오해 방지·후산정·다음 결정 순서, 자리표시자·diff·SHA256 | `author-opening-composite-node-003` | PASS | v2 production 3개와 위 fingerprint | 유효 — QA 대기 |
| C1~C7·기록 정합 | v2 production과 task·verification의 suite·correction 상태 독립 대조 | `qa-opening-composite-node-003` | FAIL — production PASS, task 기록 불일치 | SUPERSEDED — 기록 동기화 전 |
| C1~C7·기록 정합 | 최종 fingerprint, task·verification C1~C7·1/2, 자리표시자·diff·Unity 비변경·안전/승인 경계 재대조 | `qa-opening-composite-node-004` | PASS, blocker 없음 | SUPERSEDED — 사용자 가확정 결정으로 production 의미 변경 |
| C1~C7·문구 상태 | 시작·반전 원문 유지, 제작 기준 가확정, 조건부 사용자 재검토, 다음 내레이션 단계, 범위·diff·SHA256 대조 | `author-opening-provisional-copy-005` | PASS | v3 production 문서와 위 fingerprint | 유효 — canonical QA 대조 완료 |
| C1~C7·기록 정합 | v3 작성자·독립 QA 대기 상태와 correction·superseded 기록 대조 | `qa-opening-provisional-copy-006` | FAIL — 작성자 PASS와 상충하는 대기 문구 2곳 | SUPERSEDED — 기록 동기화 전 |
| C1~C7·기록 정합 | v3 production fingerprint, 사용자 결정, 작성자 PASS, correction 2/2, superseded QA, diff·Unity 비변경·안전/승인 경계 재대조 | `qa-opening-provisional-copy-007` | PASS, blocker 없음 | v3 fingerprint와 QA 회신 | SUPERSEDED — 사용자 최소 내레이션 승인으로 production 의미 변경 |
| C1~C8·내레이션 상태 | 최소 내레이션 5개 원칙, Task 2 Step 5·6, 다음 승인 순서, 범위·diff·SHA256 대조 | `author-opening-minimal-narration-008` | PASS | v4 production 문서와 위 fingerprint | 유효 — canonical QA 대조 완료 |
| C1~C8·기록 정합 | v4 production과 task의 현재 QA 범위·suite·완료 기준 대조 | `qa-opening-minimal-narration-009` | FAIL — task의 현재 범위 3곳이 C1~C7로 잔존 | task와 QA 회신 | SUPERSEDED — 기록 동기화 전 |
| C1~C8·기록 정합 | 동기화된 task 범위·suite·완료 기준, v4 fingerprint, 최소 내레이션 원칙, correction 2/2, diff·Unity 비변경·안전/승인 경계 재대조 | `qa-opening-minimal-narration-010` | PASS, blocker 없음 | v4 fingerprint와 QA 회신 | SUPERSEDED — 사용자 중앙 문구 가독성 승인으로 production 의미 변경 |
| C1~C9·가독성 상태 | 한국어 블록 최대 2줄·완전 가독 최소 5초, 페이드 제외/연장, 접근성 연장·축소 재검토, 다국어 재조정, Task 2 Step 6·다음 승인 순서, 범위·diff·SHA256 대조 | `author-opening-copy-readability-011` | PASS | v5 production 문서와 위 fingerprint | 유효 — canonical QA 대조 완료 |
| C1~C9·기록 정합 | v5 production fingerprint, 가독성 기준, task 범위·suite·완료 기준, correction 2/2, diff·Unity 비변경·안전/승인 경계 재대조 | `qa-opening-copy-readability-012` | PASS, blocker 없음 | v5 fingerprint와 QA 회신 | SUPERSEDED — 사용자 심야 환승시설 청소 구역 승인으로 production 의미 변경 |
| C1~C10·청소 구역 상태 | 심야 세 공간 동선, 성실한 생활인, 비원인성 기침, 환경 노출 비난 방지, 세균 군집·환경 운반체 이동, 최소 인적 정보·사망/중증화 금지, 후속 제작 분리, Task 3 Step 1·다음 승인 순서, 범위·diff·SHA256 대조 | `author-opening-night-transit-cleaning-013` | PASS | v6 production 문서와 fingerprint | SUPERSEDED — 총괄 Step 2 의미 blocker |
| C1~C10·기록 정합 | v6 production fingerprint, 청소 구역 계약, task 범위·suite·완료 기준, correction 2/2, diff·Unity 비변경·안전/승인 경계 재대조 | `qa-opening-night-transit-cleaning-014` | PASS, blocker 없음 | v6 fingerprint와 QA 회신 | SUPERSEDED — 총괄 FAIL로 재분류 |
| C10·총괄 의미 감사 | Task 3 Step 2의 노동자·기관 비난 방지와 승인 게이트 대조 | `director-opening-step2-reclassification-015` | FAIL — `보호 부족`이 보호 결핍·기관 과실을 선확정 | v6 plan과 총괄 회신 | SUPERSEDED — v7 중립화 전 |
| C1~C10·재분류 상태 | 승인된 Step 2 중립 문구, 미완료 체크, 제작 기준 가안·결과물 후 사용자 수정 게이트, 범위·diff·SHA256·placeholder 대조 | `author-opening-step2-neutralization-016` | PASS | v7 production 문서와 위 fingerprint | 유효 — canonical QA 대조 완료 |
| C1~C10·기록 정합 | v7 production fingerprint, 중립 Step 2·가안 수정 게이트, task 상태·범위·suite·완료 기준, 재분류 0/2, diff·placeholder·Unity 비변경·안전/승인 경계 재대조 | `qa-opening-step2-neutralization-017` | PASS, blocker 없음 | v7 fingerprint와 QA 회신 | SUPERSEDED — 사용자 Step 2 제작 기준 가안 승인으로 production 의미 변경 |
| C1~C11·Step 2 제작 가안 | 승인 순서·핵심 소품 제한·금지 표현·비말 내부 진입 뒤 주인공 고유 색/음향, Task 3 Step 2 완료·Step 3 다음 결정, 재분류 0/2, diff·placeholder·SHA256·Unity 비변경 대조 | `author-opening-step2-closing-route-018` | PASS | 위 v8 candidate fingerprint와 작성자 정적 검사 | 유효 — canonical QA 대조 완료 |
| C1~C11·기록 정합 | v8 production fingerprint, Step 2 제작 가안·완료 체크·Step 3 다음 결정, task 상태·범위·suite·완료 기준, 재분류 0/2, diff·placeholder·Unity 비변경·안전/승인 경계 재대조 | `qa-opening-step2-closing-route-019` | PASS, blocker 없음 | v8 fingerprint와 QA 회신 | SUPERSEDED — 사용자 Task 3 Step 3 승인으로 production 의미 변경 |
| C1~C12·커스터마이징 연속성 | 초기 6/4/4/2·이름·성격과 해금, 게임 수치 분리, 고정 실루엣·성장 언어 예약, 기본형→실제 선택 외형, 저장·재시청·회상·성장 레이어, 합성·spike 2종·포즈 재사용, UI·에셋·코드·Unity 후속 승인, 비식별 실루엣안 비적용, Task 3 Step 3 완료·Step 4 다음 결정, 재분류 0/2, diff·placeholder·SHA256 대조 | `author-opening-customization-continuity-020` | PASS | 이전 v9 fingerprint와 작성자 정적 검사 | SUPERSEDED — QA-021 잔여 미결정 목록 blocker 발견 전 |
| C12·확정/미결정 기록 정합 | origin의 확정된 커스터마이징 범위·이름과 미결정 목록, plan·task·verification의 Step 4 다음 결정 대조 | `qa-opening-customization-continuity-021` | FAIL — 확정 항목이 미결정 목록에 잔존 | QA 회신과 이전 v9 fingerprint | SUPERSEDED — 잔여 미결정 목록 동기화 전 |
| C1~C12·기록 보정 | 확정 커스터마이징 항목 제거, 첫 튜토리얼을 origin 미결정 목록 첫 항목으로 정렬, plan·task·verification 다음 결정, correction 1/2, diff·placeholder·SHA256·Unity 비변경 대조 | `author-opening-customization-decision-sync-022` | PASS | 위 v9 sync1 candidate fingerprint와 작성자 정적 검사 | 유효 — canonical QA 대조 완료 |
| C1~C12·기록 정합 | v9 sync1 production fingerprint, 확정·미결정 목록, Task 3 Step 3 완료·Step 4 다음 결정, task 상태·범위·suite·완료 기준, correction 1/2, diff·placeholder·Unity 비변경·안전/승인 경계 재대조 | `qa-opening-customization-decision-sync-023` | PASS, blocker 없음 | v9 sync1 fingerprint와 QA 회신 | SUPERSEDED — 사용자 Task 3 Step 4 승인으로 production 의미 변경 |
| C1~C13·세균 튜토리얼·신규 알림 | 승인 튜토리얼 비트, 캡시드 외부·동물 세포 직접 감염 금지, 5개 용어, 무손실 재시도·3회 보조, 안전 시점 강제 큐·항목별 팝업·정지·명시적 확인·1/N·무손실 실습, 알림 대상/제외·필드·도감·4단계 저장 상태·중복/버전·입력/접근성/다국어, Task 3 Step 4·5 완료·Task 4 숏/비트 다음 결정, 쥐 프로토타입과 실제 제작 승인 분리, correction 1/2, diff·placeholder·SHA256 대조 | `author-opening-bacterial-tutorial-notice-024` | PASS | 위 v10 candidate fingerprint와 작성자 정적 검사 | 유효 — canonical QA 대조 완료 |
| C1~C13·기록 정합 | v10 production fingerprint, 세균 감염 접속형·신규 콘텐츠 알림 계약, Task 3 Step 4·5 완료·Task 4 다음 결정, task 상태·범위·suite·완료 기준, correction 1/2, diff·placeholder·Unity 비변경·안전/승인 경계 재대조 | `qa-opening-bacterial-tutorial-notice-025` | PASS, blocker 없음 | 위 v10 fingerprint와 QA 회신 | SUPERSEDED — v11 새 production 문서와 사용자 Task 4 승인 반영 전 |
| C1~C14·편집 비트·공개 장부 | `A01~T01` 33개 고유 ID와 15필드, 필수·병합 후보와 의미·입력 분리 경계, 중앙 문구 완전 가독 최소 5초 외 초 단위·총 러닝타임 선확정 금지, 실제 측정 산식·커스터마이징 체류 분리, 단계별 공개·비공개·반대 인물 기록 위치·단순 귀속/무기 제작 구체 정보 금지, README·plan·task·verification 상태, 링크·diff·placeholder·SHA256 대조 | `author-opening-edit-beats-reveal-ledger-026` | PASS | 이전 v11 fingerprint와 작성자 정적 검사 | SUPERSEDED — 총괄 028 계약 blocker 교정 전 |
| C1~C14·기록 정합 | v11 production fingerprint, 33개 ID·15필드·병합/분리·시간·공개 장부 계약, README·plan·task·verification 상태, correction 1/2, 링크·diff·placeholder·SHA256·Unity 비변경·안전/승인 경계 독립 재대조 | `qa-opening-edit-beats-reveal-ledger-027` | PASS, blocker 없음 | 이전 v11 fingerprint와 QA 회신 | SUPERSEDED — 총괄 028 계약 blocker 발견 |
| C14·총괄 계약 감사 | Task 5의 canonical shot spec 수명주기와 총 비상호작용 러닝타임 산식의 중복·이중 계산 가능성 대조 | `director-opening-edit-beats-contract-audit-028` | FAIL — 기존 파일을 다시 Create, 실제 타임라인 구성요소를 재가산 | 총괄 회신과 correction 전 plan·shot spec | SUPERSEDED — author 029 correction 적용 전 blocker |
| C1~C14·계약 correction 2/2 | Task 5 기존 shot spec Modify, 첫 프레임~T01 입력 해제 실제 경과-커스터마이징 체류 단일 산식, hold·전환·입력 잠금 breakdown 비재가산, 건너뛰기 별도 보고·입력 경계 이벤트, 33개 ID·15필드·시간·공개 장부·링크·diff·placeholder·SHA256 대조 | `author-opening-edit-beats-contract-correction-029` | PASS | 위 correction 2/2 candidate fingerprint와 작성자 정적 검사 | 유효 — canonical QA 대조 완료 |
| C1~C14·correction 기록 정합 | correction 2/2 production fingerprint, 파일 수명주기·단일 산식·비재가산·건너뛰기/입력 이벤트, 33개 ID·15필드·시간·공개 장부, task·verification 상태, 링크·diff·placeholder·SHA256·Unity 비변경·안전/승인 경계 독립 재대조 | `qa-opening-edit-beats-contract-correction-030` | PASS, blocker 없음 | 이전 correction 2/2 fingerprint와 QA 회신 | SUPERSEDED — 사용자 Gate S 승인으로 production 의미 변경 |
| C1~C15·Gate S 작성자 검증 | 33개 추적 ID·15필드, 승인 편집 그룹과 내부 beat boundary, 모든 그룹의 master composition·camera axis·scale band·layer delta, band 간 bitmap zoom 금지와 scale-correct redraw·mask-match cut, 접근성·자막·건너뛰기·재시청·입력 경계, Task 4 Step 6·Task 5 Step 1~6, 시간·placeholder·링크·SHA256·diff 대조 | `author-opening-gate-s-editorial-contract-032` | PASS | 위 v12 fingerprint와 작성자 정적 검사 | 유효 — canonical QA 대조 완료 |
| C1~C15·Gate S 기록 정합 | v12 production fingerprint, 33 ID·15필드, 승인 그룹·내부 경계와 master/axis/scale/layer 전수 매핑, 시간·placeholder·링크·SHA256·diff·Unity 비변경·안전/승인 경계 독립 재대조 | `qa-opening-gate-s-editorial-contract-033` | PASS, blocker 0 | 위 v12 fingerprint와 QA 회신 | 유효·canonical |

## 검증하지 못한 항목

- 이미지·애니매틱·오디오·Unity 재생은 이번 작업 범위 밖이다.
- 가확정 문구의 실제 화면 흐름·톤·가독성 적합성과 승인 편집 그룹을 실제 배치한 최종 스토리보드·패널·숏 수·카메라 수치·타이밍은 아직 검증하지 않았다.
- 최소 내레이션의 정확한 화자·최종 대사량·언어별 녹음은 후속 결정 대상이다.
- 실제 폰트 크기·해상도·접근성 환경과 언어별 fallback의 화면 가독성은 후속 숏·구현 단계 검증 대상이다.
- 심야 환승시설 장면의 정확한 숏·러닝타임·에셋과 Unity 적용은 후속 승인·검증 대상이다.
- Step 2 제작 기준 가안의 실제 스토리보드·결과물 적합성은 후속 사용자 검토 대상이다.
- 커스터마이징 UI, 저장 형식, 방향별 스프라이트·초상·모션 코믹 에셋과 Unity 적용은 후속 승인·검증 대상이다.
- 세균 감염 접속형의 실제 숏·에셋·팝업 UI·도감·저장·실습·코드와 Unity 적용은 후속 승인·검증 대상이다.
- Task 5 Step 1~5의 문서 계약은 반영했지만 실제 스토리보드·이미지·에셋·애니매틱·오디오·코드·Unity는 만들지 않았고, 실제 연결 측정 Step 6은 미완료다.
- 총 러닝타임은 승인된 패널·비최종 애니매틱을 실제 연결하기 전에는 산정하지 않으며, 커스터마이징 체류 시간은 별도 측정 대상이다.
- `main-scenario-outline.md`의 기존 숲 바닥 도입과 새 오프닝 요약·공개 장부 링크 동기화는 별도 작업으로 남아 있다.

## fail-fast·무효화

- first blocker: 최초 작성자 검사에서 C4~C6 검사식이 문서의 실제 표현과 달라 false negative가 발생했다. 문서 결함이 아님을 해당 구간 직접 대조로 확인했다.
- blocker 발견 뒤 중지한 고비용 단계: Unity/MCP/build 계획·실행 0
- correction cycle: 2/2 — 기존 QA-021 기록 보정 1회에 더해 총괄 028의 파일 수명주기·러닝타임 이중 계산 blocker 2건을 단일 correction으로 보정했다. 기존 v1~v6 제작·기록 수정 주기 2/2는 별도 소진 이력으로 보존한다.
- 변경 뒤 무효화한 run/증거와 사유: `qa-opening-night-transit-cleaning-014`는 v6 총괄 blocker로, `qa-opening-step2-neutralization-017`, `qa-opening-step2-closing-route-019`, `qa-opening-customization-decision-sync-023`은 각각 후속 사용자 승인에 따른 production 의미 변경으로 현재 완료 증거에서 SUPERSEDED 처리한다.
- 총괄 blocker·재분류 사유: 기존 Step 2가 보호 결핍과 특정 기관 과실을 제작 전제로 선확정해 origin의 중립 환경 노출 계약과 충돌했다. 사용자 승인에 따라 `수정 필요 — 재분류`로 전환하고 v7 주기를 0/2에서 시작한다.
- v4는 새 사용자 결정 반영 revision이며 기존 제작/기록 correction cycle `2/2`를 추가 증가시키지 않는다.
- `qa-opening-minimal-narration-009`의 blocker는 production 의미 변경이 아닌 현재 QA 범위의 clerical evidence sync이므로 correction cycle은 `2/2`를 유지한다.
- v5도 새 사용자 결정 반영 revision이므로 correction cycle `2/2`를 추가 증가시키지 않는다.
- v6도 새 사용자 결정 반영 revision이므로 correction cycle `2/2`를 추가 증가시키지 않는다.
- v8도 새 사용자 결정 반영 revision이며 v7 재분류 correction cycle `0/2`를 계승하고 추가 증가시키지 않는다.
- v9도 새 사용자 결정 반영 revision이며 v7 재분류 correction cycle `0/2`를 계승하고 추가 증가시키지 않는다.
- `qa-opening-customization-continuity-021` blocker는 production 의미 변경이 아니라 확정·미결정 목록의 clerical sync 결함이므로 v7 재분류 correction cycle을 `1/2`로 증가시킨다.
- v10은 새 사용자 결정 반영 revision이며 v7 재분류 correction cycle `1/2`를 계승하고 추가 증가시키지 않는다.
- v11은 새 사용자 Task 4 승인 반영 revision으로 v7 재분류 correction cycle `1/2`를 계승했다. 총괄 028이 Task 5의 기존 파일 재생성 선언과 러닝타임 구성요소 재가산을 blocker로 판정해 단일 correction으로 `2/2`가 됐다. QA-027은 correction 전 fingerprint이므로 SUPERSEDED다.
- v12는 새 사용자 Gate S 승인 반영 revision이므로 correction cycle `2/2`를 추가 증가시키지 않는다. production 의미가 변경돼 QA-030과 총괄 031은 이전 fingerprint 이력으로 SUPERSEDED이며 새 독립 QA가 필요하다.
- 비적용 설계 이력: 사용자 피드백 전 `커스터마이징 전 비식별 미시 실루엣·빛·시점만 사용` 제안은 최종 승인과 달라 폐기했으며 production에 적용하지 않았다.
- S6 전체 suite 실행 허용/실행 횟수: 해당 없음 / 0
- S7 대형 matrix 실행 허용/실행 횟수: 해당 없음 / 0

## 비용 실행 대조

| 비용 항목 | 계획 예산 | 실제 수·run_id/근거 | 정상/초과/미집계 | 필요한 비용/회피 가능 비용 |
| --- | --- | --- | --- | --- |
| 실제 역할·인계 | 연출1·조정1·QA1·총괄1 | v1~v11 이력 유지, v12 작성자 032 PASS→독립 QA 033 PASS→최종 총괄 034 내부 승인 가능·최소 수정 0 | 주의 | 사용자 승인 의미 변경의 작성자·독립 QA·총괄 감사 완료 |
| 표적 검증 | 작성자1·QA1 | v12 작성자 032 C1~C15 PASS, 독립 QA 033 C1~C15 PASS·blocker 0, 총괄 034 내부 승인 가능 | 주의 | 정적 검증만 수행, 동적 실행 0 |
| Unity/MCP/빌드 시작 | 0 | 0 | 정상 | 전부 회피 |
| full suite | 0 | 0 | 정상 | 전부 회피 |
| matrix/capture·artifact | 0 | 0 | 정상 | 전부 회피 |

- 비용 판정: 주의 — correction 2/2 유지, v12 작성자 032·독립 QA 033 각 1회, Unity/MCP/build 실행은 0
- 중앙 비용 현황판 갱신: 기존 다른 작업의 미커밋 변경과 충돌 방지를 위해 사용자 검토 전 보류, 본 파일에 단일 기록

## 게이트 판정

- QA/검증 게이트 통과 여부: PASS — `qa-opening-gate-s-editorial-contract-033` C1~C15, blocker 0
- 총괄 관리자 검토로 넘길 수 있는지: 예 — v12 최종 감사 완료

## 프로젝트 총괄 관리자 판정

- 판정: 내부 승인 가능 — `director-opening-gate-s-final-audit-034`
- 근거: `qa-opening-gate-s-editorial-contract-033` C1~C15 PASS·blocker 0, production fingerprint 일치, 후속 제작·측정 경계 유지
- 최소 수정사항: 없음(0)
- 이전 판정: v10 `내부 승인 가능`은 이전 fingerprint 이력이며 새 production 문서의 승인으로 확대하지 않는다.
- 현재 근거: QA-033 canonical PASS와 총괄 034 최종 감사
- 승인 범위·사용자 수용 대기: 최종 스토리보드·러닝타임·이미지·애니매틱·오디오·Unity 구현은 별도 승인

## 완료 판단

- Task 5 Gate S의 보호형 압축 편집 그룹과 구도·축·스케일·레이어·접근성·입력 계약은 작성자 032, canonical QA-033, 총괄 034 최종 감사를 통과했다. 33개 추적 ID는 유지되며 최종 숏·패널 수·시간은 확정하지 않았다.
- Task 5 Step 1~5 문서 계약은 완료, Step 6 실제 스토리보드·비최종 애니매틱 연결 측정은 미완료다. Task 4 Step 6도 `main-scenario-outline.md`의 숲 바닥 도입 충돌 때문에 미완료다.

## 사용자 수용 상태

- 사용자 결정 반영: 현재 시작·반전 문구를 제작 진행 기준으로 가확정
- 사용자 결정 반영: 오프닝은 최소 내레이션으로 진행
- 사용자 결정 반영: 한국어 중앙 문구는 블록당 최대 2줄·완전 가독 시간 최소 5초로 가확정
- 사용자 결정 반영: 청소 노동자의 작업 공간은 영업 종료 직전~심야의 복합 환승시설 청소 구역
- 사용자 결정 반영: Task 3 Step 2 제작 기준 가안은 `마감 동선 추적형 + 성실한 행동 한 비트`
- 사용자 결정 반영: Task 3 Step 3은 레이어형 혼합 커스터마이징과 기본형→실제 선택 외형 연속성
- 사용자 결정 반영: Task 3 Step 4는 세균 감염 접속형과 안전 시점 강제 큐·항목별 확인/실습 신규 콘텐츠 알림
- 사용자 결정 반영: Task 4는 최종 숏 수가 아닌 33개 편집 비트 후보와 기원 공개 장부로 상세화하고 시간은 실제 연결 뒤 산정
- 사용자 결정 반영: Task 5 Gate S 보호형 압축 편집 그룹, 내부 분할, 마스터 구도·카메라 축·스케일·레이어 delta와 접근성·입력 경계 승인
- 다음 작업: Task 4 Step 6에서 `main-scenario-outline.md`의 숲 바닥 도입을 승인된 오프닝으로 동기화
- 이후 사용자 결정: Gate S 계약을 적용한 실제 스토리보드 후보·결과 패널/숏 수와 이미지 후보 생성 여부
- v12 프로젝트 총괄 재감사 전 작업 전체 `완료` 표현 금지 여부: 아니오 — 최종 감사 완료. 단, Task 4 Step 6 전파와 Task 5 Step 6 실제 측정은 미완료
- 실제 이미지·에셋·스토리보드·애니매틱·오디오·UI·저장·코드·Unity 제작 완료 표현 금지 여부: 예

## 최종 상태

- 완료/보류/승인 대기: v12 QA-033 C1~C15 PASS·blocker 0, 총괄 034 내부 승인 가능·최소 수정 0 — Gate S 계약 커밋 대기, Task 4 Step 6 메인 시나리오 전파 다음, Task 5 Step 6 실제 연결 측정 대기
- 완료 경로와 Git 상태: `_workspace/active/2026-08-08-opening-cinematic-origin/`; 현재 HEAD `a20d4f7`, v12 production·R2 상태 변경 pending
