# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-13-current-task-board-governance`
- 작업명: 현재 작업 상태판 지속 동기화 규칙 강화
- 상태: 진행 중
- 생성일: 2026-07-13
- 담당 에이전트: 문서/릴리즈 에이전트
- 보조 에이전트: QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트

## 목적

`docs/project-handoff/current-task-board.md`를 사용자와 Codex가 지속적으로 함께 확인하는 현황판으로 명시하고, 작업 시작·보류 전환·완료/보관·커밋 전 상태판 동기화와 독립 대조를 완료·커밋의 필수 조건으로 만든다.

## 입력 자료

- `AGENTS.md`
- `docs/agents/loop-engineering-gates.md`
- `docs/project-handoff/README.md`
- `docs/project-handoff/current-task-board.md`
- `docs/agents/agent-reference-map.md`
- `_workspace/completed/2026-07-13-2026-07-13-manual-play-candidate-cleanup/completion-report.md`

## 담당과 산출물

| 에이전트 | 담당 | 산출물 |
| --- | --- | --- |
| 문서/릴리즈 에이전트 | 전역 원칙과 세부 게이트 반영 | `AGENTS.md`, 게이트·상태판 운영 문서, 인계 기록 |
| QA/검증 에이전트 | 규칙의 누락 방지 조건·경로·Git 상태 대조 가능성 검증 | `verification.md` |
| 프로젝트 총괄 관리자 에이전트 | 전역 규칙·게이트·상태판 문서의 정합성 및 승인 검토 | `director-review.md` |

## 해야 할 일

1. `AGENTS.md`에 상태판의 공유 현황판 역할과 필수 동기화 시점을 짧은 전역 규칙으로 추가한다.
2. 완료·커밋 전 게이트에 상태판 대조를 차단 조건으로 추가한다.
3. 상태판 운영 README와 참조 색인에 시작·보류·완료·커밋 전 갱신 및 QA 대조 기준을 명시한다.

## 금지 범위

- Unity 코드, 씬, 테스트, 설정, 에셋, 패키지, MCP 설정을 변경하지 않는다.
- 상태판의 실제 후보·보류 내용 자체를 새로 결정하지 않는다.
- `AGENTS.md` 200줄 제한을 넘기지 않는다.

## 완료 기준

- `AGENTS.md`에 전역 동기화 원칙이 있고, 세부 절차는 게이트 문서에 연결된다.
- 완료·커밋 전 상태판 누락, 중복, 완료 경로, Git 상태 불일치가 차단 조건으로 명시된다.
- QA와 총괄 관리자 판정이 작업 기록에 남는다.
