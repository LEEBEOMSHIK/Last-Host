# 검증 기록

- 상태: 후속 R3 상태 계약 후보 `71e4dcdd…`·총괄 r2 승인으로 SUPERSEDED
- verification revision: `cost-guards-s0-r1`
- candidate fingerprint: `2711e9f5cb5e10de24c9f563bbac4e264f752db69e7afc3df8fded592276a663`
- canonical run: `cost-guards-independent-qa-correction-002`
- superseded: `9af899fe1ac8f06c7e4c4ea07dd25f04bae92c01eb809ef157f114a863af8759`, `cost-guards-selftest-correction-001`
- Unity/MCP/TestRunner/build: 0

## correction 2/2 독립 QA

- QA run: `cost-guards-independent-qa-correction-002`
- manifest 정적 대조: correction 2 manifest의 22개 파일 SHA-256·길이 전부 일치, mismatch 0.
- canonical 명령: `pwsh -NoProfile -File tools/verification/Invoke-VerificationGuardSelfTest.ps1`
- 실행 횟수: 현재 fingerprint에서 전체 fresh-temp dummy bundle 정확히 1회.
- 결과: exit 0, suite `passed: true`, 모든 case PASS, temp cleanup PASS.
- G4: 실제 unsupported-route preflight failure 첫 1건 기록, 동일 run dedup 후 1건 유지, 두 번째 실제 failure 뒤 2건, 세 번째 호출은 retry guard에서 차단되고 entry 2건 유지.
- 재분류: failure 2회 전 차단, 이후 mandatory `RootCause`·`ChangePlan`으로 통과하고 ledger의 `root_cause`·`change_plan` 별도 필드 보존.
- G5: 같은 크기·같은 UTC timestamp의 `v1`→`v2` 변경을 SHA-256 차이로 반영하고 Library 보존, unmarked cleanup 차단과 marker cleanup 통과.
- G8: token parameter 누락·token 파일 누락·만료·소비 후 재사용을 모두 Unity process 시작 전에 차단. 유효 token은 Unity path 확인 전에 소비됨.
- G1~G3, G6~G8 및 정상 integration preflight: 모두 PASS.
- Unity/MCP/TestRunner/build 시작: 0회.
- production/하네스 코드 수정: 없음.
- 완료 판단: 기술 검증 통과. 총괄 최종 판정 전 완료·내부 승인 가능은 주장하지 않는다.

## 독립 QA first blocker

- QA run: `independent-qa-ledger-negative-001`
- 정적 후보 대조: fingerprint manifest의 22개 파일 SHA-256·길이 전부 일치, mismatch 0.
- 최소 재현: fresh temp ledger를 지정하고 `McpTestRunner` route preflight를 1회 호출했다.
- 결과: wrapper exit 1, fallback 메시지 출력, ledger 파일 미생성, ledger entry 0, Unity/MCP/TestRunner/build 시작 0회.
- 원인: `Invoke-HighCostVerification.ps1`의 route/capability 및 guard preflight 예외가 `Add-LedgerEntry` 호출 전에 종료된다. 현재 `failure` 기록은 low-level runner가 실제 실행된 뒤에만 추가된다.
- 영향: 실제 preflight 실패가 criterion 실패 누적으로 남지 않으므로 G4의 "2회 실패 뒤 3회차 차단"을 실제 경로에서 보장할 수 없다. self-test의 수동 preseed ledger는 이 누락을 검증하지 못한다.
- fail-fast: 이 blocker에서 중단했다. G4 이후 동적 검증과 요청 항목 (b) reclassification 필수 필드, (c) token 누락·만료·재사용, (d) same-size/same-timestamp cache 변경은 독립 판정하지 않았다.
- 수정 조건: 모든 실제 preflight 실패를 high-cost 시작 전 atomic ledger `failure`로 1회 기록하고, 그 실제 실패 2회를 누적한 뒤 세 번째 호출이 guard/Unity 시작 전에 차단되는 fresh-ledger negative control을 추가해야 한다. 수동으로 실패 2개를 seed한 ledger만으로는 통과로 인정하지 않는다.
- 완료 판단: 완료 불가.

## criterion 상태

| ID | 결과 | 증거 |
| --- | --- | --- |
| G1 | 독립 QA PASS | unsupported `McpTestRunner`가 fallback과 함께 실행 전 nonzero |
| G2 | 독립 QA PASS | Reflection + sync 없는 Rigidbody→Y-sort fixture nonzero |
| G3 | 독립 QA PASS | BoxCollider2D→CapsuleCollider2D + stale test 목록 nonzero |
| G4 | 독립 QA PASS | 실제 실패 2회 원자 기록, 동일 run dedup, 3회차 guard 무기록 차단, 2회 뒤 재분류 필드 |
| G5 | 독립 QA PASS | same-size/same-timestamp 내용 변경 hash 반영, Library 보존, marker cleanup |
| G6 | 독립 QA PASS | full-history/4 files 차단, packet-only/3 files 통과 |
| G7 | 독립 QA PASS | stale run/fingerprint/cost 차단, current 상태 통과 |
| G8 | 독립 QA PASS | token parameter·파일 누락, 만료, 소비 후 재사용을 Unity 시작 전 차단 |

## fail-fast

- 첫 blocker에서 후속 검증을 중지한다.
- 같은 criterion 실패 1회마다 원인·수정계획·무효 evidence를 기록한다.
- 실패 2회 뒤 세 번째 실행은 재분류 전 하네스가 차단해야 한다.
- first blocker: `integration-valid-preflight` exit 1. child output 미노출로 상세 원인이 가려져, 오류 출력 보존 후 최소 재현 전 동일 bundle 재실행 금지.
- 실제 비용: PowerShell dummy bundle 1회, Unity/MCP/TestRunner/build 0회, 무효 bundle 1개.
- correction 결과: parser 원인 수정 뒤 dummy bundle 1회 PASS. 누적 비용은 dummy bundle 2회(무효 1/PASS 1), 최소 진단 1회, Unity/MCP/TestRunner/build 0회.
- 독립 QA 전 완료 판단: 완료 불가 — 구현 후보만 검증됨.
- 구현자 추가 정적 대조: actual packet brief lint PASS, actual current-state run/fingerprint/cost lint PASS, scoped diff whitespace PASS, `AGENTS.md` 141줄.
- correction 2/2: 정적 대조 뒤 전체 dummy bundle을 정확히 1회 실행해 PASS. Unity/MCP/TestRunner/build 0회. 새 fingerprint에서 독립 QA 재검증 전 완료 불가.
- correction 2/2 독립 QA: manifest 22/22 일치 후 전체 dummy bundle 정확히 1회 PASS. 이전 first blocker는 현재 fingerprint에서 해소됐으며 canonical QA run은 `cost-guards-independent-qa-correction-002`다.
