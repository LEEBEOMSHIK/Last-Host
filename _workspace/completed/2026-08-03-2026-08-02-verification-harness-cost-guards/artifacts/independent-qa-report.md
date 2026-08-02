# 독립 QA 보고 — 검증 하네스 비용·재시도 차단

## 판정

- 결과: **FAIL — 수정 필요**
- 첫 blocker: 실제 preflight 실패가 attempt ledger에 자동 기록되지 않는다.
- candidate fingerprint: `9af899fe1ac8f06c7e4c4ea07dd25f04bae92c01eb809ef157f114a863af8759`
- QA run: `independent-qa-ledger-negative-001`
- Unity/MCP/TestRunner/build 시작: `0`
- production/하네스 코드 수정: 없음

## 정적 대조

- manifest에 포함된 22개 후보 파일의 SHA-256과 길이를 현재 파일과 대조했다.
- mismatch는 0이며 QA가 검사한 후보는 implementation report의 fingerprint와 일치한다.
- `Invoke-HighCostVerification.ps1`에서 `Add-LedgerEntry`는 reclassification 등록과 low-level runner 종료 뒤에만 호출된다.
- unavailable/unknown route, retry budget, agent brief, current state, QA harness, component contract, cache sync preflight의 예외를 ledger `failure`로 기록하는 공통 catch 경로가 없다.

## 최소 재현

fresh system temp 아래 존재하지 않는 ledger 경로를 지정하고 다음과 동등한 호출을 1회 실행했다.

```powershell
pwsh -NoProfile -File tools/verification/Invoke-HighCostVerification.ps1 `
  -WorkId independent-qa-ledger -CriterionId G1 `
  -LedgerPath <fresh-temp>/attempt-ledger.json `
  -Route McpTestRunner -RunId qa-ledger-run-001 `
  -CandidateFingerprint qa-fingerprint `
  <필수 preflight 인자> -PreflightOnly
```

결과:

- wrapper exit code: `1` (예상한 route 차단)
- fallback 메시지: 출력됨
- ledger 파일: 생성되지 않음
- ledger entry 수: `0`
- Unity/MCP/TestRunner/build 시작: `0`
- fresh temp: 검증 후 안전 경로 확인 뒤 정리됨

## 원인과 영향

route availability 검사는 ledger를 읽은 뒤 즉시 `throw`한다. 이 예외는 failure entry를 추가하는 경로를 거치지 않는다. 다른 preflight guard도 child nonzero를 `throw`로 변환하지만 이를 기록하는 상위 catch가 없다.

따라서 실제 preflight 실패를 반복해도 criterion별 consecutive failure가 누적되지 않는다. 구현자 self-test는 failure entry 두 개를 수동으로 넣은 `ledger-blocked.json`에서 세 번째 호출만 검사하므로, 자동 기록 누락을 검출하지 못했다. G4와 완료 주장은 충족되지 않는다.

## 수정 조건

1. unavailable/unknown route와 모든 실제 preflight guard 실패가 high-cost 시작 전에 atomic ledger `failure` entry 하나로 기록되어야 한다.
2. entry에는 최소 criterion, run ID, candidate fingerprint, route, 실패 원인이 남아야 한다.
3. 같은 실행이 중복 기록되지 않아야 한다.
4. fresh ledger에서 같은 criterion의 실제 preflight 실패를 두 번 유발한 뒤, 세 번째 호출이 guard/Unity 시작 전에 차단되어야 한다.
5. self-test는 수동 seed ledger만 사용하지 말고 위 자연 경로를 negative control로 검증해야 한다.

## fail-fast 범위

첫 blocker에서 중단했다. 다음 항목은 독립 QA 판정하지 않았다.

- reclassification이 root cause와 change plan을 각각 필수화하는지
- low-level token 누락·만료·재사용이 모두 Unity 시작 전에 차단되는지
- cache가 같은 크기·같은 timestamp의 내용 변경을 놓치지 않는지

완료 판단: **완료 불가**
