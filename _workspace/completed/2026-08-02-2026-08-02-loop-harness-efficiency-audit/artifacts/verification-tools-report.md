# 범용 Unity 검증 도구 구현 보고

## 담당 범위

- Unity 게임플레이 코드·씬·ProjectSettings를 수정하지 않고 `tools/verification/`에 감사 설계의 P0/P1 최소 도구를 구현했다.
- 새 에이전트 역할이나 새 Codex 스킬은 만들지 않았다.
- 작업별 원자 GameView 캡처 Editor harness는 범용 PowerShell로 안전하게 대체할 수 없어 구현 범위에서 제외하고 README에 한계를 고정했다.

## 추가 파일

| 파일 | 역할 | 핵심 차단 |
| --- | --- | --- |
| `tools/verification/UnityMcpLease.ps1` | 프로젝트 단위 Unity/MCP lease | `FileMode.CreateNew` 원자 획득, agent/work_id/run_id 일치 갱신·반납, editor PID·scene·baseline·TTL 기록, 만료 자동 강탈 금지 |
| `tools/verification/Invoke-UnityEditModeTests.ps1` | EditMode batch 실행과 NUnit XML 판정 | 기존 결과 삭제, `-quit` 없이 Test Framework 종료 대기, XML 존재·비어 있지 않음·카운트 판정, 실패 nonzero, 기존 XML 전용 검증 |
| `tools/verification/Get-VerificationFingerprint.ps1` | 변경 후보 fingerprint와 run manifest | 지정 production/test/scene/package/version 파일의 정렬된 SHA-256 집계, run_id·파일별 hash JSON 기록 |
| `tools/verification/README.md` | 안전 사용 계약 | lease 순서, S0~S7 연결, PASS 무효화, batch 동시 실행 금지, 원자 캡처 한계 |

## 구현 판단

### Unity MCP lease

- lock 위치는 `<ProjectPath>/Temp/last-host-unity-mcp-lease.json`이다.
- `Acquire`는 파일이 하나라도 존재하면 TTL/PID 상태와 관계없이 실패한다. 따라서 만료를 자동 탈취 권한으로 사용하지 않는다.
- JSON schema 2는 최소 `work_id`, `agent`, `run_id`, `editor_pid`, `scene`, `acquired_utc`, `expires_utc`, `baseline_play`, `baseline_pause`, `baseline_scene`, `baseline_dirty`를 기록한다.
- Acquire는 실제 `EditorProcessId`, 대상 `Scene`, 획득 당시 Play/Pause/scene/dirty 기준 상태를 필수 입력으로 받는다.
- `Renew`와 `Release`는 agent/work_id/run_id를 모두 case-sensitive로 대조한다.
- 이전 호출 호환용 `Owner`→`Agent`, `ProcessId`→`EditorProcessId` alias는 입력에서만 제공하며 JSON은 모호하지 않은 `agent`, `editor_pid`만 쓴다.
- `Status`는 `expired`, `process_alive`, `automatic_takeover_allowed=false`를 반환한다.
- lease는 협력적 잠금이므로 스크립트를 무시한 MCP 호출까지 차단하지는 않는다. 운영 게이트와 manifest 대조가 함께 필요하다.

### EditMode 실행기

- Unity 실행 인수는 `-batchmode -nographics -projectPath -runTests -testPlatform EditMode -testResults -logFile`이며 `-quit`가 없다.
- 기본 1800초 timeout을 `Process.WaitForExit`으로 적용하며 초과 시 이 호출이 시작한 PID 하나만 종료한다. 기존 Unity 프로세스와 자식 process tree를 포괄 종료하지 않는다.
- 실행 전 정확히 지정한 기존 result XML만 제거해 stale PASS를 막는다.
- NUnit3 `<test-run>`에서 `total`, `passed`, `failed`, `skipped`, `inconclusive`, `result`를 파싱한다.
- `total > 0`, `passed == total`, 나머지 세 카운트 0, `result=Passed`, Unity exit code 0일 때만 성공한다. skipped/inconclusive도 최종 미검증이므로 nonzero다.
- `-ValidateResultsOnly`는 Unity를 시작하거나 결과 파일을 수정하지 않는다.

### candidate fingerprint

- 지정 경로가 파일이면 그 파일, 디렉터리면 모든 하위 파일을 포함한다.
- 각 행은 `category<TAB>relative-path<TAB>file-sha256<TAB>length`로 만든 뒤 정렬·결합해 최종 SHA-256을 계산한다.
- manifest에는 schema, run_id, UTC, project root, candidate fingerprint, 입력 경로, 파일별 길이·hash가 들어간다.
- dependency 자동 추론 도구가 아니므로 호출자가 증거 의존 경로를 빠짐없이 지정해야 한다.

## 실행한 검증

### 1. PowerShell 구문

```text
명령: System.Management.Automation.Language.Parser로 tools/verification/*.ps1 3개 ParseFile
결과: parser error 0
판정: PASS
```

### 2. lease 정상 흐름

```text
명령: 임시 프로젝트 경로에서 schema 2 Acquire → Status → identity mismatch Release → concurrent Acquire → Renew → Release → Status
결과: 필수 11개 계약 필드·값 일치, 다른 agent 반납 차단 뒤 lease 보존, concurrent 획득 차단, 정상 반납 후 Available
판정: PASS
```

### 3. lease 경합

```text
명령: agent-a lease 보유 중 agent-b가 같은 프로젝트에 Acquire
결과: 두 번째 CreateNew가 차단되고 기존 lease 유지
판정: PASS
```

### 3-1. lease 이전 입력 alias

```text
명령: Owner/ProcessId alias로 Acquire 후 Status와 Agent identity로 Release
결과: 입력 호환 성공, JSON에는 owner/pid 없이 agent/editor_pid만 기록
판정: PASS
```

### 4. EditMode XML 판정

```text
명령: -ValidateResultsOnly로 가림 교정 final-v2 XML 검증
입력: _workspace/active/2026-08-02-production2d-visual-overlap-correction/artifacts/qa-editmode-results-final-v2.xml
결과: total=202, passed=202, failed=0, skipped=0, inconclusive=0, valid_pass=true
판정: PASS
```

```text
명령: 존재하지 않는 XML 경로를 -ValidateResultsOnly로 검증
결과: nonzero
판정: PASS
```

### 5. fingerprint 결정성

```text
명령: 동일한 production 3개, test 1개, scene 1개, package lock 1개, Unity version 1개로 서로 다른 run_id의 manifest 2회 생성
결과: 7개 파일, 양쪽 candidate_fingerprint=11cf6bca454a773e9d03d7e462446759073245c107407b5d88aeb5f24be8e4f4
판정: PASS
```

### 6. diff 위생

```text
명령: git diff --check -- tools/verification
결과: whitespace error 없음
판정: PASS
```

### 7. batch hang 차단

```text
명령: Unity 인수를 무시하고 대기하는 임시 fake executable에 TimeoutSeconds=1 적용
기대: 1초 뒤 해당 fake PID만 종료, timeout 메시지와 nonzero, Unity 미실행
결과: `cmd.exe`를 hidden fake process로 사용해 1초 뒤 시작 PID 종료, timeout 메시지, nonzero, 잔존 대상 process 0 확인
판정: PASS
```

## 실행하지 않은 검증

- 실제 Unity batch EditMode 재실행: 이번 도구 구현은 Unity 상태를 바꾸지 않는 조건이므로 실행하지 않았다. 대신 이미 독립 QA가 만든 final-v2 XML의 read-only 판정 경로를 검증했다.
- 실제 MCP lease와 Unity Editor PID 결합 운영: 협력적 운영 절차의 다음 Unity 검증 작업에서 확인해야 한다.
- 원자 GameView 캡처: task-specific repo-owned Editor harness가 필요하며 이번 범용 도구에 포함하지 않았다.

## 완료 판단

- 요청된 범용 도구 3종과 안전 사용 문서는 구현 및 비파괴 자체 검증을 통과했다.
- 이 도구는 QA 역할을 늘리지 않고 `Unity 단독 소유`, `stale XML 차단`, `candidate revision 결합`을 기계화한다.
- 최종 채택 전 독립 QA는 실패 XML 카운트, lease identity 불일치, fingerprint 입력 누락을 포함한 negative control과 문서 규칙 정합을 재검증해야 한다.
