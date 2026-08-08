# 검증 기록

## 작업 ID

`2026-08-08-opening-cinematic-origin`

## 검증 대상

- 오프닝 시네마틱·주인공 기원 설계
- 승인 게이트별 제작 태스크 계획
- 오프닝 하위 문서 색인

## 검증 담당

- 작성자 검증: 프로젝트 조정 에이전트 `author-static-001` PASS
- 독립 QA: `qa-opening-static-002` PASS
- 프로젝트 총괄 판정: 내부 승인 가능

## 실제 수행·검증 이력

| 역할/에이전트 | 실제 수행 | 산출물·판정 |
| --- | --- | --- |
| 프로젝트 조정 | 사용자 승인 범위와 R2 S0 C1~C6 고정 | 작업 배정서 작성 |
| 픽셀아트 시네마틱 연출 | 파일을 수정하지 않고 오프닝·기원·제작 계획 초안 제출 | 조정자가 사용자 원순서와 문구를 반영해 통합 |
| 프로젝트 조정 | production 3개 작성·통합, C1~C6·자리표시자·diff·해시 검사 | `author-static-001` PASS |
| QA/검증 | C1~C6, 사용자 원장면, 무기화 정보 안전 경계, 자리표시자·모순·diff 독립 대조 | `qa-opening-static-001` PASS, blocker 없음 |
| QA/검증 | 최종 fingerprint의 C1~C6·원장면·안전 경계·자리표시자·모순·diff 재대조 | `qa-opening-static-002` PASS, blocker 없음 |
| 프로젝트 총괄 | 프로젝트 메시지·인간/노동자 묘사·박테리오파지·무기화 안전·승인 경계 감사 | 내부 승인 가능, 최소 수정 없음 |

## 원래 증상 또는 완료 주장

- 완료 주장: 승인된 오프닝과 돌연변이 기원을 검토 가능한 설계 문서와 승인 게이트별 제작 태스크로 확인할 수 있다.

## 현재 검증 revision

- 위험 등급: R2
- verification revision: `opening-origin-r2-v1`
- candidate fingerprint: README `D0BC3E78EBDE3316F80FD0711197541ED416F51EFD5CCE9CD72FB66EEE3CC99B`; origin `3578BE7185B0635B8847D98A29AEAA5D988AE17E163CF0EFD3F39116C6D25DBC`; plan `516D4AD53E73EF7DCEF28E07FF3DC82AC67FB848D956600611D2A7BDD9E052AE`
- canonical run_id: `qa-opening-static-002`
- candidate frozen 여부: 예 — QA 뒤 의미 변경은 없고, QA 완료 체크박스와 Markdown 공백 형식만 동기화했다. 최종 fingerprint 기준으로 QA 재확인한다.
- capability route / wrapper preflight: Markdown 정적 검증 / Unity preflight 불필요

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 예
- 구현 주체가 실행한 검증과 별도로 확인한 항목: C1~C6, 회사원 점심·학생 등교·어머니 점심 준비, 무기 제작 구체 정보 금지, 자리표시자·모순·diff

## 실행한 검증

| criterion ID | 유형 | 검증 방법 | run_id | 결과 | canonical 증거 | 유효/SUPERSEDED |
| --- | --- | --- | --- | --- | --- | --- |
| C1~C6 | 작성자 정적 검사 | 문구·범위 정규식 대조, 파일 존재, `git diff --check`, 자리표시자, SHA256 | `author-static-001` | PASS | production 문서 3개와 위 fingerprint | 유효 |
| C1~C6 | 독립 정적 QA | 사용자 원요청·S0·production·계획 대조와 `git diff --check` | `qa-opening-static-001` | PASS, blocker 없음 | 최초 QA 회신과 이전 fingerprint | SUPERSEDED — 형식 정리 전 fingerprint |
| C1~C6 | 독립 정적 QA | 최종 fingerprint·사용자 원장면·생물학/무기화 안전·자리표시자·모순·`git diff --check` 재대조 | `qa-opening-static-002` | PASS, blocker 없음 | 최종 QA 회신과 위 fingerprint | 유효·canonical |

## 검증하지 못한 항목

- 이미지·애니매틱·오디오·Unity 재생은 이번 작업 범위 밖이다.
- 시작 문구 최종 선택과 숏별 상세는 사용자 후속 승인 대상이다.

## fail-fast·무효화

- first blocker: 최초 작성자 검사에서 C4~C6 검사식이 문서의 실제 표현과 달라 false negative가 발생했다. 문서 결함이 아님을 해당 구간 직접 대조로 확인했다.
- blocker 발견 뒤 중지한 고비용 단계: Unity/MCP/build 계획·실행 0
- correction cycle: 1/2 — 검사식을 실제 문구에 맞춘 뒤 C1~C6 전체 PASS, 자리표시자 0건과 production SHA256을 재확인했다.
- 변경 뒤 무효화한 run/증거와 사유: 최초 QA의 의미 검토는 유효하나 fingerprint 증거는 superseded — QA 뒤 계획 문서의 완료 체크박스와 README·인용문의 Markdown 공백만 정리했으며 요구사항·제작 내용은 변경하지 않았다. 최종 fingerprint로 정적 QA를 재확인한다.
- S6 전체 suite 실행 허용/실행 횟수: 해당 없음 / 0
- S7 대형 matrix 실행 허용/실행 횟수: 해당 없음 / 0

## 비용 실행 대조

| 비용 항목 | 계획 예산 | 실제 수·run_id/근거 | 정상/초과/미집계 | 필요한 비용/회피 가능 비용 |
| --- | --- | --- | --- | --- |
| 실제 역할·인계 | 연출1·조정1·QA1·총괄1 | 연출1, 조정1, QA1, 총괄1 | 정상 | 역할 경계 검토 필요 |
| 표적 검증 | 작성자1·QA1 | `author-static-001` 1, `qa-opening-static-001` 1, 형식 정리 뒤 `qa-opening-static-002` 1 | 정상 | 최종 fingerprint 일치를 위해 QA 재확인 |
| Unity/MCP/빌드 시작 | 0 | 0 | 정상 | 전부 회피 |
| full suite | 0 | 0 | 정상 | 전부 회피 |
| matrix/capture·artifact | 0 | 0 | 정상 | 전부 회피 |

- 비용 판정: 정상
- 중앙 비용 현황판 갱신: 기존 다른 작업의 미커밋 변경과 충돌 방지를 위해 사용자 검토 전 보류, 본 파일에 단일 기록

## 게이트 판정

- QA/검증 게이트 통과 여부: PASS — `qa-opening-static-002`, C1~C6, blocker 없음
- 총괄 관리자 검토로 넘길 수 있는지: 예

## 프로젝트 총괄 관리자 판정

- 판정: 내부 승인 가능
- 근거: 기존 메시지와 충돌하지 않고 인간 전체·청소 노동자를 단순 원인으로 만들지 않으며, 박테리오파지·돌연변이 설정과 서사 수준 무기화 경계, 후속 제작 승인 분리가 충분하다.
- 승인 범위·사용자 수용 대기: 전체 캠페인·실제 시네마틱·Unity 구현은 별도 승인

## 완료 판단

- 기술 검증 통과 — 사용자 수용 대기

## 사용자 수용 상태

- 사용자 직접 확인 필요: 오프닝 사건 순서, 시작 문구 후보, 기원 미스터리, 제작 태스크
- 확인 전 `완료` 표현 금지 여부: 예

## 최종 상태

- 완료/보류/승인 대기: 사용자 문서 검토 대기
- 완료 경로와 Git 상태: `_workspace/active/2026-08-08-opening-cinematic-origin/`, 사용자 검토용 선별 커밋 대상
