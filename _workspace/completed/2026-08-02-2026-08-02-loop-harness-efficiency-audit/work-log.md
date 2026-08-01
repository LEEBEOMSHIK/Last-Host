# 작업 로그

## 2026-08-02 접수

- 사용자가 쥐·오브젝트 겹침 수정에 사용된 토큰 비용이 실제 작업 규모에 비해 과도했는지 문제를 제기했다.
- 루프 엔지니어링과 하네스가 제대로 적용됐는지, 검증 역할이 여러 번의 수정 요구 끝에 결함을 찾은 원인을 정확히 분석하고 보완하도록 요청했다.
- 1차 확인에서 현행 규칙은 역할·완료 게이트는 강하지만 fail-fast 실행 순서, 변경 후 검증 무효화, 경계 계약의 측정 정의, Unity 세션 단독 소유, 원자적 캡처 규칙이 약한 것으로 보인다.

## 2026-08-02 운영 문서 반영

- 감사 보고 3건을 대조해 R0~R3 위험 등급, 필요한 최소 역할, 사용자 원증상·합성 oracle, QA S0 charter를 운영 기준에 반영했다.
- S1~S7 fail-fast, 첫 blocker stop, correction cycle 2회 재분류, 변경 후 PASS 무효화와 freeze 후보의 전체 suite·대형 matrix 1회 규칙을 추가했다.
- Unity single-owner lease 필드, candidate fingerprint·run_id, 원자 캡처와 stale-object guard, canonical evidence 예산을 역할·스킬·템플릿에 연결했다.
- `AGENTS.md` 경량 루프와 기존 필수 게이트의 충돌을 제거하되 독립 QA와 총괄 최종 판정은 유지했다.
- Unity 파일, 새 역할·스킬, 실제 lease/capture 도구는 이번 범위에서 변경하지 않았다.
- 자체 검사: `AGENTS.md` 139줄, `git diff --check` 오류 없음. 다음 단계는 독립 QA의 규칙 시뮬레이션이다.

## 2026-08-02 범용 검증 도구 구현

- 감사 설계 중 문서만으로 막을 수 없는 Unity 세션 경합, stale EditMode XML, dirty candidate 식별 문제를 최소 범용 도구로 구현했다.
- `tools/verification/UnityMcpLease.ps1`: project-scoped CreateNew lease와 명시적 owner/work/run 반납을 구현했다. TTL 만료 자동 강탈은 금지했다.
- `tools/verification/Invoke-UnityEditModeTests.ps1`: `-quit` 없이 Unity EditMode batch를 실행하고 NUnit3 XML 카운트를 strict 판정한다. 기존 XML read-only 검증도 지원한다.
- `tools/verification/Get-VerificationFingerprint.ps1`: 지정 production/test/scene/package/version 입력을 파일별 SHA-256과 run manifest로 묶는다.
- Unity 코드·씬·ProjectSettings는 수정하지 않았다. 실제 Unity를 시작하지 않고 임시 디렉터리 lease, 기존 final-v2 `202/202` XML, 누락 XML 실패, 동일 7파일 fingerprint 결정성을 검증했다.
- 원자 GameView 캡처는 작업별 저장소 소유 Editor harness가 필요하므로 README에 명시하고 이번 범위에서 제외했다.
- 메인 리뷰에서 batch Unity hang 시 무기한 대기 위험을 발견해 실행기에 기본 1800초 timeout을 추가했다. 초과 시 호출이 시작한 PID 하나만 종료하고 명확한 오류와 nonzero를 반환한다. Unity 인수를 무시하고 대기하는 hidden `cmd.exe` fake process로 1초 timeout과 잔존 process 0을 검증했다.
- 독립 QA가 운영 문서의 lease 필수 필드와 최초 JSON schema 불일치를 blocker로 반환했다. schema를 2로 올리고 `agent`, `editor_pid`, `scene`, 획득 당시 Play/Pause/scene/dirty baseline을 Acquire 필수 입력·JSON 출력으로 정합화했다. Renew/Release identity는 agent/work_id/run_id만 엄격히 대조하며, 기존 `Owner`/`ProcessId`는 입력 alias로만 남겼다.

## 2026-08-02 사용자용 통합 보고

- 감사 산출물 5건, 운영 문서 diff와 `tools/verification/` 실제 스크립트를 대조해 `artifacts/loop-harness-audit-summary.md`를 작성했다.
- 작업 자체는 중간 난도였지만 비용 일부는 과도하고 회피 가능했다는 결론과, 독립 QA는 정확히 한 명이었다는 역할 구분을 명시했다.
- 정당한 비용과 과도한 비용, 반복 원인, 실제 반영된 R0~R3·S0~S7·단일 owner·2회 재분류·lease·fingerprint·runner·증거 예산을 사용자 관점에서 통합했다.
- 토큰 수와 절감률은 근거가 없어 추정하지 않았다.
- 실제 Unity live run과 범용 atomic GameView capture는 미실행·미구현 한계로 남겼다. 시각 증거는 작업별 repo-owned Editor harness가 필요하다고 명시했다.
- 당시 상태는 QA 실행 전이었으며 판정을 선기록하지 않았다. 이후 독립 QA r2 PASS로 갱신됐다.

## 2026-08-02 독립 QA 1차 blocker

- 독립 QA가 운영 문서의 Unity lease 필수 필드와 `UnityMcpLease.ps1`이 실제 기록하는 JSON 필드가 일치하지 않는 문제를 발견했다.
- 누락 확인 대상은 `agent`, `editor_pid`, `scene`, `baseline_play`, `baseline_pause`, `baseline_scene`, `baseline_dirty`다.
- 도구 구현자에게 수정이 반환됐으며, 수정과 재QA 전까지 범용 도구 묶음을 PASS 또는 채택 완료로 표현하지 않는다.
- 통합 보고서·task·agent-activity·handoff를 r1 blocker 상태로 갱신했다. 이후 schema 2와 독립 QA r2 PASS로 해소됐다.

## 2026-08-02 lease schema 2 보완

- 도구 구현자가 QA blocker를 반영해 실제 lease JSON에 필수 11개 계약 필드를 기록하도록 schema 2로 보완했다.
- 필드는 `work_id`, `agent`, `run_id`, `editor_pid`, `scene`, `acquired_utc`, `expires_utc`, `baseline_play`, `baseline_pause`, `baseline_scene`, `baseline_dirty`다.
- `Owner`와 `ProcessId`는 이전 입력 호환 alias로만 유지하고 JSON에는 `agent`, `editor_pid`만 기록한다.
- 구현자는 임시 프로젝트에서 필수 필드, identity mismatch, concurrent acquire, renew/release, legacy alias를 자체 재검증했다.
- 통합 보고서는 schema 2 보완 사실을 반영했고, 후속 독립 QA r2가 도구 상태를 PASS로 갱신했다.

## 2026-08-02 독립 QA r2와 총괄 blocker 보완

- 독립 QA `process-harness-qa-r2`가 운영 문서·도구 18개 파일을 새 fingerprint와 run_id로 검증해 PASS했다. r1 실패는 `SUPERSEDED` 처리됐다.
- 총괄 1차 검토는 핵심 분석과 QA r2를 인정했으나 세 문서 blocker 때문에 `수정 필요`를 판정했다.
- 사용자용 통합 보고, task, work-log, agent-activity, handoff를 QA r2 PASS와 총괄 재검토 대기로 동기화했다.
- `agent-skill-plan.md`의 모든 목표 총괄 사전 호출을 제거하고, 사전 검토는 R3·승인 범위 충돌·등급 불명확에서만 필수로 맞췄다. R1/R2 정형 분류는 조정자가 수행하고 R1~R3 최종 총괄은 유지한다.
- `_workspace/templates/task-r1-summary.md`를 추가해 R1은 원증상·완료 주장·변경 파일/owner·표적 테스트·금지 범위·correction cycle·QA·총괄만 기록하도록 했다. R2/R3 정식 S0 필드는 요구하지 않는다.
- `_workspace/README.md`, `loop-engineering-gates.md`, `agent-reference-map.md`에 R1 템플릿 사용 경로와 R2 재분류 조건을 연결했다.
- QA r2의 도구 negative control은 통과 사실로 유지한다. 총괄 blocker로 바꾼 운영 문서는 새 fingerprint/run_id의 표적 QA를 거쳐야 하며, 그 뒤 총괄 재검토로 넘긴다. 최종 내부 승인 가능을 선기록하지 않았다.

## 2026-08-02 QA r3와 총괄 2차 상태 동기화

- 독립 QA `process-harness-qa-r3`가 fingerprint `11edc0c864b179cd1dd2468764b74aa2dda94c20376a19815b422f0a334a8aa6`, run `loop-harness-qa-r3-20260802`에서 총괄 1차 blocker 보정을 PASS했다.
- 총괄 2차는 총괄 사전 호출 충돌과 R1 요약 경로 부재가 해소됐음을 인정했으나, task·handoff·사용자용 통합 보고가 이미 끝난 r3를 아직 대기 상태로 기록한 한 건 때문에 다시 `수정 필요`로 판정했다.
- 운영 계약·템플릿·스크립트는 변경하지 않고 task, work-log, agent-activity, handoff, 사용자용 통합 보고와 필요 상태판만 r3 PASS·총괄 최종 재대조 대기로 동기화했다.
- `loop-engineering-gates.md`의 “QA 보고·상태판 문구만 변경하면 기능 증거 유지” 규칙에 따라 r3 기능 증거는 유지하며 새 QA revision을 만들지 않는다.
- 총괄 최종 판정은 아직 없으며 `내부 승인 가능`이나 `완료`를 선기록하지 않았다.

## 2026-08-02 총괄 3차 최종 판정 동기화

- 프로젝트 총괄 관리자 3차가 상태-only 보정을 재대조해 `내부 승인 가능`을 판정했다.
- 현재 상태를 `QA r3 PASS, 총괄 내부 승인 가능, 사용자 보고 가능, active·미커밋`으로 동기화했다.
- 1·2차 `수정 필요` 이력과 실제 Unity live run·실제 Editor PID/MCP lease 결합·범용 atomic GameView capture 미실행/미구현 한계는 유지한다.
- 이번 반영은 총괄의 명시적 다음 단계에 따른 판정 상태 반영이므로 새 QA·총괄 루프를 열지 않는다.
- 사용자가 완료·보관 또는 커밋을 요청하지 않았으므로 작업 폴더는 `active/`에 유지하고 커밋하지 않는다.

## 2026-08-02 사용자용 지속관리 가이드 보완

- 사용자 후속 요청에 따라 기존 감사 작업을 재개하고 상태를 `사용자용 운영 가이드 보완 진행 중, active·미커밋`으로 변경했다.
- `docs/agents/loop-engineering-user-guide.md`를 새 공식 사용자·온보딩 참고 문서로 작성했다. 0결함·0비용 비보장 경계, R0~R3, 역할별 검증 책임, 허용 재실행, S0~S7, 2회 재분류, 증거 예산·revision, Unity 한계, 사용자 체크리스트와 문서 관리 책임을 한 파일에 통합했다.
- `loop-engineering-gates.md`에는 구현자 표적 1회, QA 핵심·freeze 후 canonical 실행, 총괄 테스트 실행 금지, 허용 재실행 조건과 run 기록만 최소 실행 규칙으로 추가했다.
- `docs/README.md`와 `agent-reference-map.md`에 에이전트 배정·검증·비용·중복 검증 문의의 필수 사용자 참고로 연결했다.
- Unity, 스크립트, 템플릿, 새 역할·스킬은 변경하지 않았다. 새 변경의 독립 QA·총괄 판정은 선기록하지 않는다.
- 자체 문서 위생 검사에서 `git diff --check`가 오류 없이 통과했고 새 가이드의 후행 공백과 필수 경로도 이상이 없었다. 저장소의 기존 Unity·AGENTS.md·템플릿 dirty 변경은 사용자/다른 작업 소유로 보존했으며 이번 패치에서 수정하지 않았다.

## 2026-08-02 사용자 가이드 최종 상태 동기화

- 독립 QA r4가 사용자 가이드와 canonical 실행 소유권·중복 검증 제한을 PASS했고, 프로젝트 총괄 관리자가 `내부 승인 가능`을 판정했다.
- 현재 상태를 `QA r4 PASS, 총괄 내부 승인 가능, 사용자 확인 가능, active·미커밋`으로 동기화했다.
- 이번 반영은 완료된 판정의 상태-only 동기화다. 운영 계약·가이드 본문·색인·`verification.md`·`director-review.md`·Unity·스크립트·템플릿을 변경하지 않았고 새 QA·총괄 루프를 열지 않는다.
- 사용자가 완료·보관 또는 커밋을 요청하지 않았으므로 작업은 `active/`와 미커밋 상태를 유지한다.

## 2026-08-02 작업 비용 중앙 현황판 보완

- 사용자 후속 요청에 따라 상태를 `작업 비용 중앙 현황판 보완 진행 중, active·미커밋`으로 전환했다.
- `docs/project-handoff/task-cost-dashboard.md`를 만들고 정확한 토큰·금액 비추정, 관찰 가능한 비용 proxy, 관리 owner와 `정상 / 주의 / 과다 / 미집계` 판정 기준을 정의했다.
- 겹침 교정 행에는 사고 보고의 독립 QA 1종, QA batch Unity 5 starts, 결과 있는 full suite 4회, QA correction 3회, invalid capture 1세트, artifacts 34개·약 18.5MB를 그대로 기록하고 `과다 — 부분 회피 가능`으로 분류했다.
- 현재 감사 행에는 QA r1~r4, 총괄 판정 4회, Unity/MCP Play/빌드 0회, r2 negative-control 1묶음, r3/r4 표적 문서 QA와 상태-only 반복의 필요·회피 가능 구분만 기록했다. 공식 correction 수·artifact 총량처럼 근거가 불명확한 값은 `미집계`로 남겼다.
- R1·R2/R3 작업 배정, QA 검증, 완료 보고 템플릿과 실행 게이트에 최소 계획/실제 비용 필드·동기화 차단 조건을 추가하고 사용자 가이드·문서 색인·작업영역에 중앙 경로를 연결했다.
- Unity, 검증 스크립트, 에이전트, 스킬, AGENTS.md는 변경하지 않았다. 현재 비용 현황판 보완의 QA·총괄 판정은 선기록하지 않는다.

## 2026-08-02 QA r5 blocker 보정

- 독립 QA r5가 실행 기준의 `정상` 축약 정의와 중앙 비용 현황판의 전체 정의가 정확히 일치하지 않는 blocker 1건을 반환했다.
- `loop-engineering-gates.md`의 정의를 `계획된 역할·검증·산출물 예산 이내이며 이유 없는 중복·폐기가 없음`으로 맞췄다. 그 외 운영 계약·표·템플릿·색인·Unity·스크립트는 변경하지 않았다.
- 현재 비용 현황판 보완의 실제값에 r5 표적 QA 1회, blocker correction 1/2, Unity/MCP/빌드 0회를 반영했다.
- 상태는 `r5 blocker 보정 후 표적 재QA 대기, active·미커밋`이다. 재QA PASS와 총괄 판정은 선기록하지 않는다.

## 2026-08-02 QA r6·총괄 비용 현황판 상태 blocker 동기화

- 독립 QA r5 1회 FAIL은 r6 후보로 `SUPERSEDED`됐고, 독립 QA r6 1회가 PASS했다. r5·r6 manifest는 각각 1개다.
- 비용 dashboard sub-correction은 1/2이며 r5/r6의 Unity, MCP, 빌드, 동적 도구, full suite, matrix, capture는 모두 0회다. 정확한 token·금액은 계측 근거가 없어 `미집계`다.
- 감사 전체 누적은 QA revision r1~r6, 총괄 판정 5회(수정 필요 3, 내부 승인 2)다.
- 총괄 비용 dashboard 1차 판정은 구조·계약을 승인했지만 중앙 행과 상태 문서가 r5에 머문 stale 1건 때문에 `수정 필요`다.
- 현재 상태를 `QA r6 PASS, 총괄 1차 수정 필요 — 상태-only 보정 후 read-only 재대조 대기, active·미커밋`으로 맞췄다. 새 QA는 열지 않으며 총괄의 read-only 재대조만 남는다.

## 2026-08-02 비용 현황판 총괄 최종 승인 동기화

- 프로젝트 총괄 관리자가 상태-only 보정 뒤 중앙 비용 행과 상태 문구를 read-only로 재대조해 `내부 승인 가능`을 판정했다.
- 감사 전체 총괄 판정은 6회(수정 필요 3, 내부 승인 3)이며 QA r6 PASS와 r5 FAIL `SUPERSEDED`는 유지한다.
- 비용 판정을 `과다 — 부분 회피 가능`으로 확정하고 사용자 확인 가능, active·미커밋 상태로 동기화했다.
- 새 QA·총괄·Unity·MCP·build·동적 도구·full suite·matrix·capture 실행은 없었다.

## 2026-08-02 사용자 커밋 요청·완료 보관

- 사용자 커밋 요청을 완료·보관 승인으로 반영해 감사 작업을 `active/`에서 `_workspace/completed/2026-08-02-2026-08-02-loop-harness-efficiency-audit/`로 이동했다.
- `completion-report.md`를 추가하고 QA r6 PASS, 총괄 내부 승인, 비용 `과다 — 부분 회피 가능`, 정확 token/금액 `미집계`를 최종 기록했다.
- 이번 상태 정리는 기존 canonical evidence를 재사용했으며 새 QA·Unity·MCP·빌드는 실행하지 않았다.
- Git 상태는 기능 구현 `7ba12df` 선행 커밋 완료, 감사 완료 보관·운영 변경 별도 커밋 대기다.
