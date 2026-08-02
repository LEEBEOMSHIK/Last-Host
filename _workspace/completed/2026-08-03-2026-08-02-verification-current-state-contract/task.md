# 검증 current-state 상태 계약 교정

## 기본 정보

- 작업 ID: `2026-08-02-verification-current-state-contract`
- 상태: 기술 검증 통과 — 총괄 내부 승인 가능, 커밋 대기
- 위험 등급: R3 — 공용 검증 상태 값과 역할 간 상태 전이 계약 변경
- 재분류 ID: `reclass-g7-status-contract-001`
- 이전 작업: `2026-08-02-verification-harness-cost-guards` correction 2/2 뒤 총괄 G7 FAIL
- root cause: current-state lint가 run·fingerprint·cost만 비교하고 요청 역할이 기대하는 status 값은 비교하지 않았다.
- change plan: 허용 상태 목록과 호출자 기대 status를 machine-readable profile·lint·wrapper에 연결하고 status-only stale 반례로 검증한다.
- correction cycle: `0/2` (총괄 1차 판정의 문서·상태 동기화는 후보 코드 correction이 아님)

## S0 사용자 원증상·검증 charter

- 사용자 원증상: 오래된 작업 패킷 상태를 여러 에이전트가 다시 읽고 교정하면서 동일 검증·문서 갱신 비용이 반복된다.
- 합성 oracle: work/run/fingerprint/cost/evidence가 같은 JSON에서 `status`만 route 기대값과 다르게 바꾼 fixture.
- 성공 oracle: unknown status와 valid-but-stale status-only fixture가 nonzero이고 `ready-for-verification` fixture가 PASS한다.
- 실패 oracle: profile 밖 status 또는 route 기대와 다른 status가 wrapper preflight를 통과한다.
- 경계값: 허용 status 8개, route 5개, `ready-for-verification` → `verification-running` 단일 실행 전이.
- negative control: run/fingerprint/cost/evidence는 그대로 유지하고 status만 바꿔 다른 guard 영향 없이 차단을 증명한다.
- production owner: 상태 계약 구현자 1명만 `tools/verification` 관련 파일을 수정한다.
- QA owner: 독립 QA는 후보를 수정하지 않고 manifest와 dummy bundle을 1회만 검증한다.

## 상태 전이·수명주기

| 시점 | 기대 status | 허용 전이/처리 |
| --- | --- | --- |
| 구현 후보 고정 | `ready-for-verification` | wrapper preflight 진입 허용 |
| 실제 고비용 실행 직전 | `verification-running` | profile의 `ready-for-verification` → `verification-running`만 허용 |
| 독립 QA 통과 | `independent-qa-pass-awaiting-director` | 총괄 read-only 감사 입력 |
| 차단 | `blocked` | 원인·변경계획 기록 전 재실행 금지 |

- 초기화: 새 work ID current-state는 현재 후보 run/fingerprint와 함께 생성한다.
- reset: 후보 변경 시 이전 run/evidence는 superseded 처리하고 새 current-state를 만든다.
- 중복 방지: 같은 run/fingerprint/status를 다시 기록해 새 검증 근거로 세지 않는다.

## 최소 역할·비용 제한

- 구현자 1 → 독립 QA 1 → 총괄 1
- 모든 위임은 `fork_turns:none`, packet-only, 필수 진입 파일 3개 이하
- 실제 Unity/MCP/TestRunner/build 0회
- dummy negative control은 구현자 1회, QA 1회 이내
- 첫 실패에서 중지. correction은 최대 2회이며 원인·변경계획 기록 전 재실행 금지

## 비용 계획·실제

| 항목 | 계획 | 실제 |
| --- | --- | --- |
| 역할 | 구현자 1 → 독립 QA 1 → 총괄 1 | 구현자 1, QA 1, 총괄 read-only 1차 1 |
| dummy bundle | 구현자 1회, QA 1회 | 구현자 1회 `24/24`, QA 1회 `24/24` |
| Unity/MCP/TestRunner/build | 0 | 0 |
| correction | 0/2 시작 | 코드 correction 0/2, 상태 문서 동기화 1회 |

## criterion

| ID | 기대값 |
| --- | --- |
| S1 | profile에 허용 current-state status와 역할별 기대 status가 machine-readable로 정의된다. |
| S2 | 알 수 없는 status와 호출자가 기대한 status 불일치는 high-cost 실행 전에 nonzero다. |
| S3 | wrapper 실행 preflight는 `ready-for-verification`만 허용하고 실행 시작 시 `verification-running`으로 전이한다. |
| S4 | status-only stale fixture와 정상 fixture가 self-test에서 각각 차단/PASS한다. |
| S5 | 기존 G1~G8, failure ledger, token, cache 반례가 회귀하지 않는다. |

## 금지 범위

- Unity production·씬·테스트·ProjectSettings·패키지 변경
- 실제 Unity/MCP/TestRunner/build 실행
- 기존 두 correction 결과를 덮어쓰기
- 커밋·푸시

## 완료 기준

- S1~S5 독립 QA PASS (`24/24`)
- 총괄 read-only r2 내부 승인 가능
- 원 작업은 이 작업의 승인 전 완료로 승격하지 않는다.
