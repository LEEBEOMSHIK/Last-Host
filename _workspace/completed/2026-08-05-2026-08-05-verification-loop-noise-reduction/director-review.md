# 프로젝트 총괄 관리자 검토

## 검토 대상

- 작업 ID: `2026-08-05-verification-loop-noise-reduction`
- 위험 등급: R1 — 운영 문서 3개의 국소 계약 명확화
- production 문서:
  - `docs/agents/loop-engineering-gates.md`
  - `docs/agents/loop-engineering-user-guide.md`
  - `.agents/project-coordinator-agent.md`
- 독립 QA: 정적 대조 1회, `FAIL — 수정 요청`

## 판정

**내부 승인 가능 — 상태-only 해소 후 완료 가능**

독립 QA의 유일한 blocker였던 `agent-activity.md` correction cycle `0/2` 표기는 실제 이력에 맞는 `1/2`로 동기화됐다. 운영 규칙, acceptance contract, production 문서 3개와 게임 production/test는 이 해소 과정에서 변경되지 않았다. 따라서 사용자 승인 방향에 따라 같은 정적 QA를 다시 만들지 않고 closeout할 수 있다.

## 공통 계약 감사

1. **preflight와 actual/user-facing run 구분**: preflight 차단은 내부 ledger·진단 `run_id`에 보존하되 실제 Unity/MCP/build 시작이나 사용자-facing run 횟수·번호로 세지 않도록 세 문서가 일치한다.
2. **구현 상한**: 같은 원인 분류의 고비용 표적은 최초 1회와 correction 1회가 상한이며, 두 번째 실패 뒤 `수정 필요 — 재분류`로 중지하고 사용자에게 `문제 / 선택지 / 추천`을 보고하기 전 새 후보를 금지한다.
3. **독립 QA 상한**: 구현자 current fingerprint green 뒤 QA 1회, 보정 뒤 재진입 1회까지만 허용하고 두 번째 QA 실패에서는 중지·재분류·사용자 보고한다. 독립 QA 자체는 생략할 수 없다.
4. **S0 용어**: 구현 전 단계는 사용자에게 `S0 계약 검토`로 표현하며 `QA run` 또는 고비용 실행으로 세지 않는다.
5. **순수 final status-only sync**: 독립 QA·총괄 판정 뒤 board·cost·CURRENT·completed 경로·상태만 바꾸는 최종 sync는 조정자 자체 대조로 닫는다. 운영 규칙·acceptance contract·production·테스트/하네스 변경은 예외가 아니며 기존 QA·총괄·증거 무효화 게이트를 적용한다.
6. **핵심 전이 보고**: 최초 blocker, 재분류·사용자 결정 필요, 기술 PASS·최종 결과를 중심으로 보고하며 내부 run label과 30초 단위 상태는 요청 시에만 제공한다.

## 기존 안전 게이트 보존

- S1~S7 fail-fast와 첫 blocker 중단 규칙을 유지한다.
- 독립 QA와 프로젝트 총괄 최종 판정을 삭제하거나 선택 사항으로 만들지 않았다.
- attempt ledger, candidate fingerprint, canonical run, `SUPERSEDED`, Unity lease와 비용 추적을 기존 규칙대로 유지한다고 명시했다.
- preflight 차단의 내부 기록을 삭제하지 않으며, 실제 failure 2회 뒤 재분류와 retry-budget guard 계약도 유지한다.
- 상태-only라는 이름으로 acceptance contract·production·테스트/하네스 변경을 숨길 수 없도록 예외 범위를 제한했다.

## QA FAIL metadata 해소 판정

- QA는 본문 criterion 1~7, 변경 파일 3개 범위, 링크, `git diff --check`, Unity/MCP/build 0을 모두 PASS했다.
- 유일한 FAIL은 QA가 기대값을 `1/2`로 명시한 패킷 activity 현재값이 `0/2`였다는 상태 기록 불일치였다.
- 조정자가 QA가 지정한 한 값을 `1/2`로 변경했고 현재 `task-r1-summary.md`, `agent-activity.md`, `verification.md`, `work-log.md`가 일치한다.
- 이 보정은 새 판단이나 운영 계약 변경이 필요 없는 기계적 상태-only 해소다. 독립 QA의 본문 검증 결과를 대체하지 않고, QA가 남긴 expected value를 총괄이 read-only로 대조했다.
- 새 계약의 `상태-only 최종 동기화` 예외를 QA 이전 변경에 소급 적용한 것이 아니다. 사용자가 이 metadata correction에 대해 QA 재실행을 만들지 않도록 승인한 범위에서 closeout한 것이다.

## 범위·비용 확인

- 변경은 허용된 운영 문서 3개와 작업 패킷 기록에 한정됐다.
- Unity/MCP/TestRunner/build/full suite/matrix/capture는 0회이며 Unity lease가 필요하지 않았다.
- correction cycle은 `1/2`다. 비용 판정은 correction 1회가 있어 `주의`가 적절하며, 초기 부분 patch와 stale activity 표기는 회피 가능 비용으로 남긴다.
- 게임 production, 테스트, 씬, ProjectSettings, package, wrapper/runner 변경은 없다.

## 수정 필요

- 공통 운영 계약 수정 필요 없음.
- board/cost/CURRENT와 completed 경로는 조정자가 이 판정 뒤 source/target path·status·diff를 자체 대조해 최종 동기화한다.

## 문제 사안

- 남은 기능·계약 blocker 없음.
- QA FAIL 원문과 해소 이력은 삭제하거나 PASS로 다시 쓰지 않고 보존해야 한다.

## 사용자 결정 필요

- 없음. 사용자 승인 방향 안의 운영 소음 축소 문서 변경이다.

## 다음 단계

1. 조정자가 task/verification/handoff의 최종 상태와 비용을 상태-only로 동기화한다.
2. board/cost/CURRENT를 완료 상태와 completed 경로에 맞춘다.
3. 작업 폴더를 completed로 이동하며 새 QA·총괄 라운드는 만들지 않는다.
