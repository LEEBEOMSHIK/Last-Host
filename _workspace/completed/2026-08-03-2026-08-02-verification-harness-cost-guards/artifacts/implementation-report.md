# 검증 하네스 비용 차단 구현 보고

## 결과

- 구현 후보: 독립 QA 인계 가능
- candidate fingerprint: `9af899fe1ac8f06c7e4c4ea07dd25f04bae92c01eb809ef157f114a863af8759`
- run ID: `cost-guards-selftest-correction-001`
- Unity/MCP/TestRunner/build 실행: `0`
- 커밋: 하지 않음

## G1~G8 구현

| ID | 실행 전 차단 | 구현 |
| --- | --- | --- |
| G1 | 미지원·알려진 실패 route | `verification-capabilities.json` + wrapper route lookup, fallback 포함 nonzero |
| G2 | Reflection/private reflection, Rigidbody→Y-sort sync 누락 | `Test-QaHarnessSafety.ps1` C# lint |
| G3 | collider/resolver 계약 변경 뒤 stale test | baseline/candidate/test 타입 impact scan과 stale 목록 nonzero |
| G4 | 같은 criterion 연속 실패 2회 뒤 세 번째 실행 | machine-readable ledger + 명시적 reclassification parameter set |
| G5 | cold import 반복·위험 cleanup | work ID cache, 소스 3폴더 증분 sync, Library 보존, canonical marker cleanup |
| G6 | full-history·필수 파일 3개 초과·과대 brief | packet-only JSON brief lint |
| G7 | run/fingerprint/status/cost stale | machine-readable current-state/evidence consistency lint |
| G8 | low-level EditMode Run 우회 | wrapper-issued 5분 one-shot token을 Run 필수 parameter로 강제; `ValidateResultsOnly`만 호환 유지 |

## 주요 파일

- `tools/verification/Invoke-HighCostVerification.ps1`: 단일 고비용 preflight/wrapper, ledger, reclassification, token 발급
- `tools/verification/Invoke-UnityEditModeTests.ps1`: low-level Run token 검증·선소비
- `tools/verification/Sync-IsolatedUnityProject.ps1`: task-scoped cache sync/reuse/marker cleanup
- `tools/verification/Test-*.ps1`: agent brief, current-state, QA harness, component contract lint
- `tools/verification/Invoke-VerificationGuardSelfTest.ps1`: 실제 Unity 0회의 dummy negative-control 묶음
- `tools/verification/verification-capabilities.json`: route·retry·brief machine-readable profile

## 검증과 correction

최초 dummy bundle은 마지막 정상 통합 preflight에서 PowerShell parser 오류가 나 canonical evidence로 사용하지 않았다. self-test가 child output을 보존하도록 바꾼 뒤 최소 재현에서 `Test-QaHarnessSafety.ps1`의 `$relative:` 문자열 보간 두 곳을 원인으로 확정했다. `${relative}:`로 수정한 후에만 전체 묶음을 한 번 다시 실행했다.

canonical 명령:

```powershell
pwsh -NoProfile -File tools/verification/Invoke-VerificationGuardSelfTest.ps1
```

결과: G1~G8 negative/positive case와 정상 wrapper preflight 모두 PASS, 임시 cache cleanup PASS, Unity/MCP/build `0`.

추가 정적 대조:

- 실제 packet `agent-brief.json` lint PASS
- 실제 `verification-current-state.json`의 work/run/fingerprint/cost lint PASS
- scoped `git diff --check` PASS
- `AGENTS.md` 141줄로 200줄 미만 유지

비용 proxy:

- dummy PowerShell bundle: 2회(첫 bundle 무효 1, correction PASS 1)
- 최소 integration 진단: 1회
- fingerprint: 잘못된 배열 binding 호출 1회 실패 후 수정 호출 1회 성공
- Unity/MCP/TestRunner/build: 0회
- correction: `1/2`

## 남은 검증 경계

- 실제 Unity 실행이 금지 범위였으므로 wrapper token을 소비한 뒤의 실제 EditMode 성공 경로는 실행하지 않았다.
- 독립 QA가 현재 fingerprint에서 정적 대조와 dummy bundle을 독립 검증해야 한다.
- 총괄 판정 전까지 `기술 검증 통과`, `내부 승인 가능`, `완료`를 주장하지 않는다.
