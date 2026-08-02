# 상태 계약 최소 구현 보고

## 후보

- work ID: `2026-08-02-verification-current-state-contract`
- run ID: `state-contract-implementer-001`
- candidate fingerprint: `71e4dcdd173ebe2211ec57856dc35d1069a7170a343d33b2d2a2c395e6ebbdda`
- manifest: `artifacts/candidate-fingerprint.json` (15파일)

## 구현

- capability profile에 allowed status 8개와 5개 route별 `expected_status`, `allowed_transitions`를 정의했다.
- current-state lint가 profile 밖의 unknown status와 명시 `ExpectedStatus` 불일치를 차단한다.
- high-cost wrapper가 route 기대값 `ready-for-verification`을 lint에 전달한다.
- 실제 실행 분기는 current-state를 다시 확인하고 profile에 허용된 `ready-for-verification` → `verification-running` 전이만 적용한다.
- self-test에 unknown status, valid-but-stale status-only mismatch, 정상 status fixture를 분리했다.
- README, 중앙 gate, verification skill reference에는 같은 계약을 최소 문구로 연결했다.

## 검증

- 정적 대조 PASS: JSON parse, 전체 verification PowerShell AST, 5개 route 계약, allowed status endpoint, wrapper 인자·전이 연결, G7 fixture, scoped diff whitespace.
- 전체 dummy bundle: 정확히 1회, 24/24 PASS.
- G7: unknown status 차단, 유효하지만 route 기대와 다른 status-only stale 차단, 정상 status PASS.
- 회귀: 기존 G1~G8, 실제 failure 원장·dedup·재분류, one-shot token, same-size/same-timestamp SHA-256 cache, 정상 통합 preflight PASS.
- 비용: PowerShell dummy bundle 1회, Unity/MCP/TestRunner/build 0회.

## 판정

구현자 검증은 PASS다. 독립 QA와 총괄 read-only 승인이 남아 있으므로 작업 완료는 주장하지 않는다.
