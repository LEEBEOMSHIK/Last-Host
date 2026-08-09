# 검증 기록

## 작업 ID

`2026-08-08-opening-cinematic-origin`

## 검증 대상

- 오프닝 시네마틱·주인공 기원 설계
- 승인 게이트별 제작 태스크 계획
- 오프닝 하위 문서 색인

## 검증 담당

- 작성자 검증: v10 메인 시나리오 디렉터 `author-opening-bacterial-tutorial-notice-024` PASS — C1~C13, 튜토리얼·생물학/용어·실패·알림/저장/접근성·범위 경계, fingerprint, diff, placeholder 대조
- 독립 QA: `qa-opening-bacterial-tutorial-notice-025` PASS — C1~C13, 튜토리얼·생물학/용어·실패·알림/저장/접근성·범위 경계, production fingerprint, task·verification 기록 정합, diff·placeholder·Unity 비변경, blocker 없음
- 프로젝트 총괄 판정: v10 최종 감사 `내부 승인 가능`, 최소 수정 없음 — canonical QA `qa-opening-bacterial-tutorial-notice-025`

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

## 원래 증상 또는 완료 주장

- 완료 주장: 승인된 오프닝과 돌연변이 기원을 검토 가능한 설계 문서와 승인 게이트별 제작 태스크로 확인할 수 있다.

## 현재 검증 revision

- 위험 등급: R2
- verification revision: `opening-origin-r2-v10-bacterial-tutorial-notice`
- candidate fingerprint: README `D0BC3E78EBDE3316F80FD0711197541ED416F51EFD5CCE9CD72FB66EEE3CC99B`; origin `57C2592D4F4E74E768B78AECA844D518007C982DCB1188A88560C42B3ACAB397`; plan `73EA1FF51DFAB374F77462295657B32FF195698942DDAAF7ADDB1EBF72C4162B`
- canonical run_id: `qa-opening-bacterial-tutorial-notice-025`
- candidate frozen 여부: 예 — canonical QA 뒤 프로젝트 총괄 재감사 전 production 의미 변경 금지
- capability route / wrapper preflight: Markdown 정적 검증 / Unity preflight 불필요

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 예
- 구현 주체가 실행한 검증과 별도로 확인한 항목: C1~C6, 회사원 점심·학생 등교·어머니 점심 준비, 무기 제작 구체 정보 금지, 자리표시자·모순·diff

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
| C1~C13·기록 정합 | v10 production fingerprint, 세균 감염 접속형·신규 콘텐츠 알림 계약, Task 3 Step 4·5 완료·Task 4 다음 결정, task 상태·범위·suite·완료 기준, correction 1/2, diff·placeholder·Unity 비변경·안전/승인 경계 재대조 | `qa-opening-bacterial-tutorial-notice-025` | PASS, blocker 없음 | 위 v10 fingerprint와 QA 회신 | 유효·canonical |

## 검증하지 못한 항목

- 이미지·애니매틱·오디오·Unity 재생은 이번 작업 범위 밖이다.
- 가확정 문구의 실제 화면 흐름·톤·가독성 적합성과 숏별 상세는 아직 검증하지 않았다.
- 최소 내레이션의 정확한 화자·최종 대사량·언어별 녹음은 후속 결정 대상이다.
- 실제 폰트 크기·해상도·접근성 환경과 언어별 fallback의 화면 가독성은 후속 숏·구현 단계 검증 대상이다.
- 심야 환승시설 장면의 정확한 숏·러닝타임·에셋과 Unity 적용은 후속 승인·검증 대상이다.
- Step 2 제작 기준 가안의 실제 스토리보드·결과물 적합성은 후속 사용자 검토 대상이다.
- 커스터마이징 UI, 저장 형식, 방향별 스프라이트·초상·모션 코믹 에셋과 Unity 적용은 후속 승인·검증 대상이다.
- 세균 감염 접속형의 실제 숏·에셋·팝업 UI·도감·저장·실습·코드와 Unity 적용은 후속 승인·검증 대상이다.

## fail-fast·무효화

- first blocker: 최초 작성자 검사에서 C4~C6 검사식이 문서의 실제 표현과 달라 false negative가 발생했다. 문서 결함이 아님을 해당 구간 직접 대조로 확인했다.
- blocker 발견 뒤 중지한 고비용 단계: Unity/MCP/build 계획·실행 0
- correction cycle: 1/2 — v7 재분류 검증 주기. QA-021의 잔여 미결정 목록 blocker를 기록 동기화로 1회 보정했고, 기존 v1~v6 제작·기록 수정 주기 2/2는 소진 이력으로 보존한다.
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
- 비적용 설계 이력: 사용자 피드백 전 `커스터마이징 전 비식별 미시 실루엣·빛·시점만 사용` 제안은 최종 승인과 달라 폐기했으며 production에 적용하지 않았다.
- S6 전체 suite 실행 허용/실행 횟수: 해당 없음 / 0
- S7 대형 matrix 실행 허용/실행 횟수: 해당 없음 / 0

## 비용 실행 대조

| 비용 항목 | 계획 예산 | 실제 수·run_id/근거 | 정상/초과/미집계 | 필요한 비용/회피 가능 비용 |
| --- | --- | --- | --- | --- |
| 실제 역할·인계 | 연출1·조정1·QA1·총괄1 | v1 이력 유지, v2 메인 시나리오 디렉터1·조정1·QA 최초1 FAIL+재진입1 PASS·총괄1 | 주의 | 승인된 의미 변경과 기록 정합 확인에 필요한 역할 |
| 표적 검증 | 작성자1·QA1 | v1~v9 이력 유지, v10 작성자1 PASS·QA1 PASS | 주의 | C1~C13 canonical 재확인 완료, 총괄 재감사 필요 |
| Unity/MCP/빌드 시작 | 0 | 0 | 정상 | 전부 회피 |
| full suite | 0 | 0 | 정상 | 전부 회피 |
| matrix/capture·artifact | 0 | 0 | 정상 | 전부 회피 |

- 비용 판정: 정상 — Unity/MCP/build 없이 승인된 서사 문서만 갱신
- 중앙 비용 현황판 갱신: 기존 다른 작업의 미커밋 변경과 충돌 방지를 위해 사용자 검토 전 보류, 본 파일에 단일 기록

## 게이트 판정

- QA/검증 게이트 통과 여부: PASS — `qa-opening-bacterial-tutorial-notice-025`, C1~C13, blocker 없음
- 총괄 관리자 검토로 넘길 수 있는지: 예 — v10 canonical QA 전달 가능

## 프로젝트 총괄 관리자 판정

- 판정: 내부 승인 가능 — v10 최종 감사
- 근거: 기존 Task 3 Step 2 blocker는 중립화됐고, 승인된 Step 2~4 제작 기준과 C1~C13 기록 정합을 canonical QA `qa-opening-bacterial-tutorial-notice-025`가 PASS·blocker 없음으로 확인했다.
- 최소 수정사항: 없음
- 승인 범위·사용자 수용 대기: 전체 캠페인·실제 시네마틱·Unity 구현은 별도 승인

## 완료 판단

- Task 3 Step 4 세균 감염 접속형과 신규 콘텐츠 알림 계약은 v10 canonical QA와 프로젝트 총괄 최종 감사를 통과했다. 다음은 Task 4 숏·비트 분해의 사용자 결정 대기다.
- 이번 최종 감사 결과를 반영하는 상태-only 동기화는 production 의미와 canonical fingerprint를 바꾸지 않으므로 새 QA·총괄 라운드가 필요하지 않다.

## 사용자 수용 상태

- 사용자 결정 반영: 현재 시작·반전 문구를 제작 진행 기준으로 가확정
- 사용자 결정 반영: 오프닝은 최소 내레이션으로 진행
- 사용자 결정 반영: 한국어 중앙 문구는 블록당 최대 2줄·완전 가독 시간 최소 5초로 가확정
- 사용자 결정 반영: 청소 노동자의 작업 공간은 영업 종료 직전~심야의 복합 환승시설 청소 구역
- 사용자 결정 반영: Task 3 Step 2 제작 기준 가안은 `마감 동선 추적형 + 성실한 행동 한 비트`
- 사용자 결정 반영: Task 3 Step 3은 레이어형 혼합 커스터마이징과 기본형→실제 선택 외형 연속성
- 사용자 결정 반영: Task 3 Step 4는 세균 감염 접속형과 안전 시점 강제 큐·항목별 확인/실습 신규 콘텐츠 알림
- 다음 사용자 결정: Task 4 숏·비트 분해와 그 결과에 따른 총 러닝타임
- 프로젝트 총괄 재감사 전 작업 전체 `완료` 표현 금지 여부: 아니오 — v10 최종 감사 완료
- Task 4 사용자 승인 전 숏·에셋·UI·저장·코드·Unity 제작 완료 표현 금지 여부: 예

## 최종 상태

- 완료/보류/승인 대기: v10 canonical QA PASS·프로젝트 총괄 내부 승인 가능 — Task 3 완료, Task 4 숏·비트 분해 사용자 결정 및 승인 문서 선별 커밋 대기
- 완료 경로와 Git 상태: `_workspace/active/2026-08-08-opening-cinematic-origin/`, 사용자 검토용 선별 커밋 대상
