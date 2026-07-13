# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-13-manual-play-candidate-cleanup`
- 작업명: 수동 플레이 보류 항목과 다음 작업 후보 중복 정리
- 상태: 진행 중
- 생성일: 2026-07-13
- 담당 에이전트: 문서/릴리즈 에이전트
- 보조 에이전트: QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트

## 목적

사용자 수동 플레이 체감 확인은 `보류 항목`으로만 유지하고, `다음 작업 후보`에는 사용자 수동 플레이 결과 반영을 포함해 어떠한 후보도 남지 않도록 상태판을 동기화한다.

## 입력 자료

- `AGENTS.md`
- `docs/agents/loop-engineering-gates.md`
- `docs/project-handoff/current-task-board.md`
- `_workspace/completed/2026-07-13-2026-07-13-unity-mcp-input-verification-standard/completion-report.md`

## 담당과 산출물

| 에이전트 | 담당 | 산출물 |
| --- | --- | --- |
| 문서/릴리즈 에이전트 | 상태판 중복 제거와 최신 상태 정정 | `docs/project-handoff/current-task-board.md`, `documentation-handoff.md` |
| QA/검증 에이전트 | 보류/후보 분리와 후보 0개 독립 확인 | `verification.md` |
| 프로젝트 총괄 관리자 에이전트 | 범위·QA·상태판 정합성 검토 | `director-review.md` |

## 금지 범위

- Unity 코드, 씬, 테스트, 설정, MCP 설정, 다른 작업 상태를 변경하지 않는다.
- 사용자 수동 플레이 확인 자체를 완료 처리하지 않는다.

## 완료 기준

- `사용자 수동 플레이 체감 확인`은 보류 항목에만 남는다.
- `다음 작업 후보` 절에는 후보 항목이 0개다.
- QA와 총괄 관리자 판정이 작업 기록에 남는다.
