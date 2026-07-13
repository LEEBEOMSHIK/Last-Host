# 문서 반영 인계

## 반영 내용

- `AGENTS.md`에 `current-task-board.md`를 사용자와 Codex의 공유 현황판으로 규정하는 짧은 전역 원칙을 추가했다.
- `docs/agents/loop-engineering-gates.md`의 완료 전 필수 게이트를 다섯 개로 확장하고, 상태판 동기화·QA 독립 대조·실패 시 완료/커밋 중지 조건을 추가했다.
- `docs/project-handoff/README.md`와 `docs/agents/agent-reference-map.md`에 작업 시작, 보류 전환, 완료/보관, 커밋 전의 상태판 갱신 시점과 QA 대조를 명시했다.

## QA 대조 요청

- `AGENTS.md`가 200줄 미만인지 확인한다.
- 전역 원칙과 세부 게이트의 동기화 시점이 일치하는지 확인한다.
- 완료·보관 시 후보 제거, 실존 완료 경로 기록, 후보·보류 중복 방지, Git 상태 대조, 실패 시 완료·커밋 중지 조건이 모두 있는지 확인한다.
- `current-task-board.md`와 Unity 코드·씬·테스트·설정·MCP 설정이 변경되지 않았는지 확인한다.

## 문서/릴리즈 판정

QA와 프로젝트 총괄 관리자 검토 전까지 완료 보고 또는 커밋 보고를 하지 않는다.
