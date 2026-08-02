# 독립 QA 보고 — correction 2/2

## 판정

- 결과: **PASS — 기술 검증 통과, 총괄 최종 판정 대기**
- candidate fingerprint: `2711e9f5cb5e10de24c9f563bbac4e264f752db69e7afc3df8fded592276a663`
- canonical QA run: `cost-guards-independent-qa-correction-002`
- manifest 대조: 22/22 SHA-256·길이 일치, mismatch 0
- production/하네스 코드 수정: 없음

## 실행과 결과

명령: `pwsh -NoProfile -File tools/verification/Invoke-VerificationGuardSelfTest.ps1`

- 현재 fingerprint의 fresh-temp 전체 dummy bundle 정확히 1회: exit 0, suite PASS, temp cleanup PASS
- 실제 preflight failure: 첫 run 1건 기록, 동일 run dedup 후 1건 유지, 두 번째 run 뒤 2건, 세 번째 호출 차단 후에도 2건 유지
- 재분류: failure 2회 전 차단; 이후 mandatory `RootCause`·`ChangePlan` 통과 및 `root_cause`·`change_plan` 별도 보존
- token: parameter·파일 누락, 만료, 소비 후 재사용을 Unity 시작 전에 차단; 유효 token은 Unity path 확인 전에 소비
- cache: 같은 크기·같은 UTC timestamp의 `v1`→`v2` 변경을 SHA-256으로 반영하고 Library 보존; cleanup guard PASS
- G1~G3, G6~G8과 정상 wrapper preflight: 모두 PASS
- Unity/MCP/TestRunner/build 시작: 0회

| ID | 결과 | 핵심 evidence |
| --- | --- | --- |
| G1 | PASS | unsupported route + fallback, preflight nonzero |
| G2 | PASS | Reflection·sync 없는 Rigidbody→Y-sort 차단 |
| G3 | PASS | stale component contract 차단 |
| G4 | PASS | actual failure 2회·dedup·3회차 무기록 차단·재분류 필드 |
| G5 | PASS | hash sync·Library 보존·cleanup guard |
| G6 | PASS | full-history 차단, packet-only 통과 |
| G7 | PASS | stale state 차단, current state 통과 |
| G8 | PASS | token 누락·만료·재사용 차단 |

## 남은 경계

실제 Unity 성공 경로는 금지 범위라 실행하지 않았다. 총괄 최종 판정 전 `내부 승인 가능` 또는 `완료`는 주장하지 않는다.
