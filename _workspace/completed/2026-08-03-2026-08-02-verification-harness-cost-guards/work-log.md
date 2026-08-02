# 작업 로그

## 2026-08-02

- 사용자 비용 감사 결과를 운영 하네스 R2로 재분류했다.
- 회피 가능 항목을 G1~G8 negative control로 고정했다.
- 전체 대화 fork를 금지하고 구현자 1명에게 packet-only로 인계하기로 했다.

### correction 1/2 — dummy bundle first blocker

- 명령: `pwsh -NoProfile -File tools/verification/Invoke-VerificationGuardSelfTest.ps1`
- 결과: G1~G8 negative/positive 개별 case 뒤 마지막 `integration-valid-preflight`에서 exit 1로 중지했다.
- 비용: PowerShell dummy bundle 1회, Unity/MCP/build 0회.
- 직접 원인: `Invoke-Case`가 unexpected exit에서 child output을 예외에 포함하지 않아 통합 preflight의 실제 blocker가 가려졌다. 같은 bundle을 진단 없이 반복할 수 없다.
- 수정 계획: self-test 실패 예외에 child output을 포함한다. 그 뒤 전체 bundle이 아니라 valid integration preflight만 임시 fixture로 최소 재현해 실제 blocker를 식별하고, 해당 원인을 수정한 후 correction 검증 bundle을 1회만 실행한다.
- 무효 evidence: 첫 dummy bundle은 최종 통합 PASS가 없어 canonical evidence로 쓰지 않는다.
- 최소 재현 결과: `Test-QaHarnessSafety.ps1`의 문자열 보간 `$relative:` 두 곳이 PowerShell parser에서 잘못된 scope-qualified 변수로 해석됐다. `${relative}:`로 고치는 국소 parser 수정이 실제 원인 교정이다.
- fingerprint 첫 호출 실패: 외부 `pwsh -File` 인자에서 `-ProductionPath @('...')`를 넘겨 배열이 단일 parameter 값으로 binding되지 않았고 두 번째 경로부터 positional argument로 해석됐다. candidate나 self-test 실패가 아니다. 수정 계획은 현재 PowerShell 프로세스에서 경로 배열 변수를 만들고 스크립트를 직접 호출하는 것이며, 같은 잘못된 명령은 반복하지 않는다.
- correction bundle: G1~G8 negative/positive case와 정상 wrapper preflight PASS. dummy 임시 파일·cache cleanup PASS. Unity/MCP/build 0회.
- candidate fingerprint: `9af899fe1ac8f06c7e4c4ea07dd25f04bae92c01eb809ef157f114a863af8759`, run `cost-guards-selftest-correction-001`, 22 files.
- 실제 packet brief/current-state lint PASS, scoped `git diff --check` PASS, `AGENTS.md` 141줄.

### 독립 QA first blocker — 실제 preflight 실패 ledger 미기록

- QA run: `independent-qa-ledger-negative-001`.
- 정적 대조: candidate fingerprint manifest의 22개 파일 SHA-256·길이가 모두 일치했다.
- 최소 재현: fresh temp ledger 경로로 `McpTestRunner` preflight를 1회 실행했다.
- 결과: wrapper exit 1, ledger 미생성(entry 0), Unity/MCP/TestRunner/build 0회.
- 직접 원인: route availability와 개별 preflight guard의 `throw`를 ledger failure로 변환하는 catch/record 경로가 없다. `Add-LedgerEntry -Outcome failure`는 low-level runner 종료 뒤에만 호출된다.
- 영향: 실제 실패 2회가 ledger에 쌓이지 않아 세 번째 호출 차단 조건이 자연 경로에서 성립하지 않는다. 수동으로 failure 2개를 미리 넣은 self-test는 자동 기록 누락을 가린다.
- 수정 계획 조건: 실제 preflight 실패마다 원자적으로 failure 1개를 기록하고, fresh ledger에서 동일 criterion 실패 2회를 유발한 뒤 세 번째 호출이 모든 guard·고비용 실행 전에 차단됨을 독립 검증한다. 같은 run의 중복 기록 방지도 함께 고정한다.
- fail-fast: 추가 반례와 (b)~(d) 동적 검증을 중단했다. 독립 QA 판정은 `완료 불가`다.

### correction 2/2 — QA actual-ledger blocker 교정

- 범위: 실제 preflight failure atomic ledger 기록·run identity 중복 방지, failure 2회 뒤에만 root cause/change plan 별도 재분류, token 누락·만료·재사용, same-size/same-timestamp cache hash 반례.
- 계획: wrapper/cache/self-test/README/실행 게이트만 수정하고 정적 parser·문구 대조 뒤 전체 dummy bundle을 정확히 1회 실행한다.
- 중단 조건: 최종 dummy bundle 첫 실패 시 재실행하지 않고 `blocked`로 기록한다.
- Unity/MCP/TestRunner/build: 0회 유지.
- 정적 대조: PowerShell AST, capability JSON, 옛 parameter/수동 seed 잔존, scoped diff whitespace 모두 PASS.
- correction 2/2 dummy bundle: 정확히 1회 실행, 전체 PASS. 실제 failure 2회→3회차 무기록 차단, 재분류 필드, token 누락·만료·재사용, same-size/same-timestamp hash sync 포함.
- 새 candidate fingerprint: `2711e9f5cb5e10de24c9f563bbac4e264f752db69e7afc3df8fded592276a663`, run `cost-guards-selftest-correction-002`, 22 files.
- 이전 fingerprint `9af899fe...8759`와 구현자 run은 `SUPERSEDED`.
- correction 2/2 비용: dummy bundle 1, 정적 대조 1묶음, fingerprint 1, Unity/MCP/TestRunner/build 0.

### correction 2/2 독립 QA PASS

- QA run: `cost-guards-independent-qa-correction-002`.
- 후보 대조: manifest 22개 파일 SHA-256·길이 전부 일치, mismatch 0.
- canonical 명령: `pwsh -NoProfile -File tools/verification/Invoke-VerificationGuardSelfTest.ps1`.
- 실행: 현재 fingerprint에서 전체 fresh-temp dummy bundle 정확히 1회, exit 0, suite PASS, temp cleanup PASS.
- G4: 첫 실제 failure entry 1, 동일 run dedup 뒤 1, 두 번째 실제 failure 뒤 2, 세 번째 guard 뒤에도 2로 유지.
- 재분류: failure 2회 전 차단, 이후 mandatory RootCause/ChangePlan 통과 및 별도 ledger 필드 보존.
- G5: 같은 크기·같은 UTC timestamp의 내용 변경 반영, Library 보존, cleanup guard PASS.
- G8: token parameter·파일 누락, 만료, 유효 token 선소비, 소비 후 재사용 차단 PASS. Unity process 시작 0.
- G1~G3, G6~G8과 정상 integration preflight 모두 PASS.
- 비용: 독립 QA dummy bundle 1회, Unity/MCP/TestRunner/build 0회, 추가 correction·재실행 0.
- 판정: 기술 검증 통과. 총괄 최종 판정 대기.

### 총괄 read-only 감사 — G7 first blocker

- manifest 22/22 SHA-256·길이와 candidate fingerprint `2711e9f5...a663`, QA run/current-state 일치를 재계산·대조했다.
- `Test-VerificationCurrentState.ps1`는 status 존재만 확인하고 기대값·허용값·전이를 검증하지 않는다. self-test의 stale case도 status-only 반례가 아니므로 G7의 status mismatch 차단을 증명하지 못한다.
- 판정: 내부 승인 불가. G7에서 fail-fast 중단. Unity/MCP/TestRunner/build/dummy self-test 재실행 0회.
- correction `2/2`, 추가 여유 0. 재분류 또는 승인된 후속 작업에서 보완 후 새 fingerprint 독립 QA가 필요하다.
