# 검증 하네스 비용 차단 correction 2/2 구현 보고

## 판정

- 결과: 구현자 correction bundle PASS, 독립 QA 재검증 가능
- candidate fingerprint: `2711e9f5cb5e10de24c9f563bbac4e264f752db69e7afc3df8fded592276a663`
- run ID: `cost-guards-selftest-correction-002`
- 이전 fingerprint `9af899fe1ac8f06c7e4c4ea07dd25f04bae92c01eb809ef157f114a863af8759`: `SUPERSEDED`
- Unity/MCP/TestRunner/build 시작: `0`
- 커밋: 하지 않음

## QA blocker 교정

route/capability와 child preflight 예외를 공통 catch에서 처리한다. failure entry는 ledger lock 아래 최신 원장을 다시 읽고 temporary JSON을 원자 교체하며 criterion, outcome, run ID, fingerprint, route와 실패 원인을 기록한다.

같은 criterion/outcome/run/fingerprint/route는 중복 기록하지 않는다. retry-budget 검사는 catch 밖에서 수행하므로 실제 failure 두 건 뒤 세 번째 호출 차단은 새 failure를 만들지 않는다.

재분류는 같은 criterion의 연속 failure가 2회 이상일 때만 허용한다. `RootCause`와 `ChangePlan`은 각각 필수 parameter이며 원장의 `root_cause`, `change_plan` 독립 필드로 남는다.

## 추가 반례 교정

- cache: 길이 또는 timestamp가 아니라 SHA-256 내용 비교로 세 소스 폴더를 동기화한다.
- token: 누락과 만료를 실행 전 차단한다. 유효 token은 Unity executable 확인 전에 소비해 재사용을 차단한다.
- self-test: 수동 seed ledger를 제거하고 실제 unsupported route failure 2회로 원장을 만든다.

## 정적 대조

- 모든 `tools/verification/*.ps1` PowerShell AST parse PASS
- capability profile JSON parse PASS
- 옛 `-Reason`, 수동 `ledger-blocked/old-1/old-2` 잔존 0
- scoped `git diff --check` PASS

## canonical dummy bundle

```powershell
pwsh -NoProfile -File tools/verification/Invoke-VerificationGuardSelfTest.ps1
```

correction 2/2에서 전체 bundle은 위 명령 1회만 실행했고 PASS했다.

- 실제 failure 2회 원자 기록, 동일 run dedup, 3회차 guard와 entry 수 2 유지: PASS
- failure 2회 전 재분류 차단, 이후 root cause/change plan 별도 기록: PASS
- 같은 크기·같은 UTC timestamp의 `v1`→`v2` cache 변경 반영과 Library 보존: PASS
- token 누락·만료·소비 후 재사용 차단: PASS
- 유효 token 소비 뒤 존재하지 않는 Unity executable에서 중지: PASS, process 시작 0
- G1~G3, G6~G8 기존 negative control과 정상 wrapper preflight: PASS
- marker-safe cleanup과 self-test temp cleanup: PASS

## 비용과 경계

- correction 2/2 dummy PowerShell bundle: 1회 PASS
- correction 2/2 정적 대조: 1묶음 PASS
- fingerprint: 1회 PASS
- Unity/MCP/TestRunner/build: 0회
- 추가 correction 여유: `0`

실제 Unity 성공 경로는 금지 범위라 실행하지 않았다. 현재 fingerprint에서 독립 QA와 총괄 판정 전에는 완료를 주장하지 않는다.
