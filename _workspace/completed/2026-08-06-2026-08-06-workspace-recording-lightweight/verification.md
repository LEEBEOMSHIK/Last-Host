# 검증 기록

## 작업 ID와 후보

- 작업 ID: `2026-08-06-workspace-recording-lightweight`
- 위험 등급: R3
- candidate: 현재 작업 트리의 경량 기록 운영 문서 27파일(26 tracked + 신규 `_workspace/templates/record.md`)
- revision: `workspace-minimal-generation-reclassified-r1-role-contract`
- 상태: 독립 QA PASS·총괄 내부 승인 가능 — 완료 보관 준비

## criterion 판정

| criterion | 판정 | 근거 |
| --- | --- | --- |
| C1 | PASS | R0 무기록·QA/총괄/board 없음 |
| C2 | PASS | R1 `record.md` 1개, 표적 검증과 조건부 QA·총괄 |
| C3 | PASS | R2 `task.md`+`verification.md`, 독립 QA·총괄 유지 |
| C4 | SUPERSEDED | 이전 revision의 R3 필수 5파일 계약. 사용자 요청으로 기본 2파일+조건부 분리 파일 계약으로 변경 |
| C5 | PASS | 세션 중단·외부 차단·실제 인계 때만 handoff |
| C6 | PASS | 기존 active/completed 이력 비소급 |
| C7 | PASS | board active/next, dashboard 대상 합집합, canonical evidence |
| C8 | PASS | 계약·candidate 불변 상태-only 동기화 재검토 없음 |
| C9 | PASS | 신규 active 작업은 필수 canonical 파일만 기본 생성하고 조건부 파일·artifact·빈 폴더·중복 증거 생성을 금지하도록 전역 규칙·workspace·역할·템플릿 갱신 |
| C10 | PASS | completed는 같은 최소 active 폴더를 이동하고 새 완료 패킷·보고서·중복 artifact를 생성하지 않도록 수명주기 갱신 |

## 정적 검사

- `AGENTS.md`: 141줄, 200줄 미만.
- 변경 문서 Markdown 링크·필수 경로: 존재 확인, 누락 0.
- 옛 R1 일괄 QA·총괄·다중 기록 계약 검색: 잔여 0.
- 기존 이력: 삭제·일괄 이동·소급 재작성 없음.
- `git diff --check`: PASS.
- Unity/MCP/build/full suite/matrix/capture: 0.

## QA와 잔여 위험

- 독립 QA 최종 판정: PASS. C1~C10 전부 현재 revision에서 통과.
- QA run_id: `qa-workspace-minimal-reclassified-20260806T215802+09:00`
- 27파일 content-manifest SHA-256: `0ED73144523EF6D3E850DA0BE4B5AF15D9424F00E2DA316A490785264C8228E3` (`relative/path<TAB>file_sha256` 정렬, LF 결합; 신규 `_workspace/templates/record.md` 포함).
- canonical evidence: 전역 규칙·gate·workspace lifecycle·역할·templates의 현재 27파일. `AGENTS.md` 141줄, stale 전체 패킷 강제 문구 0, Markdown 링크 누락 0, active/completed tracked 삭제·이동 0, `git diff --check` staged/unstaged PASS(CRLF 안내만), Unity/MCP/build/full/matrix/capture 0.
- 구현 owner: 문서/릴리즈 에이전트 시도 후 안전 검토 blocker, 사용자 직접 승인 근거에 따라 조정자가 같은 task에서 남은 패치 완료.
- correction 1 first blocker: `agent-reference-map.md`의 오래된 상태판·비용판 전 작업 강제 동기화 문구가 C7/C8과 충돌. active·next/대상 비용 작업만 동기화하고 상태-only 최종 동기화 재QA 없음으로 최소 교정.
- correction 2 first blocker: `docs/unity/unity-mcp-setup.md`가 일반 R2에도 `agent-activity.md`를 무조건 요구. 결과는 `verification.md`에 통합하고 조건부 R3 파일이 실제 존재할 때만 동기화하도록 교정했으며, 조건부 templates의 상호 참조도 같은 의미로 정리했다.
- correction 2 QA FAIL: `docs/agents/agent-skill-plan.md`의 오래된 완료 차단 문구가 실행 결과에 영향 없는 R1에도 QA와 총괄을 무조건 요구해 C2와 충돌한다. 2/2 한도에 따라 추가 자동 패치를 중단하고 사용자 보고·재분류를 기다린다.
- reclassification: 사용자 `진행하고, 완료되면 커밋 푸쉬` 승인으로 `workspace-minimal-generation-reclassified-r1-role-contract` revision을 시작했다. 잔여 두 문구를 해당 등급·변경에 필수인 QA·총괄로 한정했고 reclassification correction은 0/2다.
- 총괄 판정: `내부 승인 가능`. QA 이후 `verification.md`·board·dashboard의 판정 문구만 C8 상태-only로 동기화되어 QA 시점 fingerprint와 현재 전체 manifest가 다른 provenance를 확인했으며, 정책 계약 파일에는 후속 변경이 없다고 판정했다.
- 총괄 잔여 위험: shared board/dashboard와 dirty Unity·다른 작업 변경을 커밋에서 분리하고, QA 시점 fingerprint를 현재 정책 fingerprint로 오인하지 않는다.
- 잔여 위험: 작업 트리의 관련 없는 Unity 변경은 이 작업 커밋에서 반드시 제외한다.
