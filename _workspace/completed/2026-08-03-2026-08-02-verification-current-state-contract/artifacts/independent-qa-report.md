# 독립 QA 보고

- 검증 대상: current-state 허용값·route 기대값·실행 전이 계약과 기존 G1~G8 회귀
- canonical run: `state-contract-independent-qa-001`
- candidate fingerprint: `71e4dcdd173ebe2211ec57856dc35d1069a7170a343d33b2d2a2c395e6ebbdda`
- 정적 대조: manifest 15파일의 길이·SHA-256·합성 fingerprint 일치, PowerShell AST 10파일 오류 0, allowed status 8개와 route 5개 계약 일치
- S1: PASS — 전역 허용 status 8개와 각 route의 `expected_status`·`allowed_transitions` 확인
- S2: PASS — unknown status와 route 기대값 불일치가 각각 nonzero로 차단됨
- S3: PASS — preflight가 `ready-for-verification`을 요구하고 실제 실행 분기에서 같은 상태를 재확인한 뒤 허용된 `verification-running` 전이를 기록하고 low-level runner로 진행하는 순서 확인
- S4: PASS — `G7-valid-but-stale-status-only-blocked`, `G7-current-state-pass` 기대 결과 확인
- S5: PASS — 전체 dummy self-test 정확히 1회, 24/24 PASS. G1~G8, failure ledger/dedup/reclassification, one-shot token, same-size/same-timestamp SHA-256 cache, 정상 통합 preflight 회귀 없음
- 비용: QA PowerShell dummy bundle 1회, Unity/MCP/TestRunner/build 0회, correction·재실행·폐기 증거 0
- 첫 blocker: 없음
- 미검증: 실제 Unity/MCP/TestRunner/build는 작업 계약에 따라 실행하지 않음
- 완료 판단: **S1~S5 독립 QA PASS — 총괄 read-only 판정 대기**
