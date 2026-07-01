# 문서 색인

`docs/`는 문서 성격별 하위 폴더로 나눈다. 새 문서를 추가할 때는 가장 가까운 성격의 폴더에 넣고, 작업별 참조가 필요하면 `docs/agents/agent-reference-map.md`도 함께 갱신한다.

## 문서 배치 규칙

- 새 문서는 먼저 성격을 정한 뒤 가장 가까운 폴더에 둔다.
- 한 폴더 안에 공식 범위, 승인 이력, 구현 계획, 검증 기록처럼 성격이 다른 문서가 섞이면 하위 폴더를 만든다.
- 하위 폴더를 만든 경우 해당 폴더에 `README.md`를 두어 각 폴더의 성격과 사용자가 확인할 순서를 적는다.
- 문서 위치를 바꾸면 `AGENTS.md`, `docs/agents/agent-reference-map.md`, 관련 에이전트, 스킬, 작업 이력의 참조 경로를 함께 갱신한다.
- `_workspace/`는 에이전트 작업 이력 영역이므로 사용자 확인 대상 문서와 섞지 않는다.

## 폴더 구조

- `agents/`: 에이전트, 스킬, 루프 엔지니어링, 작업 참조 색인
- `design/`: 원본 기획 해석, 게임 방향 요약, 상세 게임 기획
- `prototype/`: 쥐 숙주 프로토타입 공식 범위, 승인 이력, 구현 계획
- `project/`: 저장소와 프로젝트 준비 상태
- `unity/`: Unity 베이스라인, Unity MCP, 에디터/프로젝트 설정 관련 문서

## 현재 주요 문서

- `agents/agent-reference-map.md`: 작업 유형별 필수/선택 참조 색인
- `agents/agent-skill-plan.md`: 에이전트 역할, 루프 엔지니어링, 승인 흐름
- `design/game-design-summary.md`: 게임 기획 요약
- `design/README.md`: 상세 게임 기획 폴더 구조와 작성 기준
- `prototype/README.md`: 프로토타입 문서 성격별 하위 폴더 색인
- `prototype/official/rat-host-prototype.md`: 쥐 숙주 프로토타입 범위
- `prototype/approvals/rat-host-approval-packet.md`: 프로토타입 승인 이력과 승인 항목
- `prototype/plans/rat-host-implementation-plan.md`: 승인된 쥐 숙주 프로토타입 구현 계획
- `project/project-prep.md`: 현재 프로젝트 준비 상태
- `unity/unity-baseline-report.md`: Unity 프로젝트 읽기 전용 기준 상태
- `unity/unity-mcp-setup.md`: Unity MCP 설정과 운영 기준
