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
- `project-handoff/`: 사용자와 Codex가 함께 보는 현재 작업 후보와 핸드오프 상태판
- `unity/`: Unity 베이스라인, Unity MCP, 에디터/프로젝트 설정 관련 문서

## 현재 주요 문서

- `agents/agent-reference-map.md`: 작업 유형별 필수/선택 참조 색인
- `agents/agent-skill-plan.md`: 에이전트 역할, 루프 엔지니어링, 승인 흐름
- `agents/loop-engineering-gates.md`: 위험 등급·실행 순서·검증·완료 차단의 유일 실행 기준
- `agents/loop-engineering-user-guide.md`: 에이전트 배정, 검증 책임, 비용·중복 검증을 사용자가 한 파일로 확인하는 비실행 요약
- `design/game-design-summary.md`: 게임 기획 요약
- `design/README.md`: 상세 게임 기획 폴더 구조와 작성 기준
- `design/visual/README.md`: 2D 아이소메트릭 도트 비주얼 제작 문서 색인
- `design/visual/references/README.md`: 목표 목업의 출처·용도·한계와 reference 사용 규칙
- `design/visual/graphics-direction-management.md`: 현재 2D 그래픽 방향, 공통 규격과 시험안 수용 경계
- `design/visual/pixel-isometric-2d-production-guide.md`: 2D 타일·캐릭터 스프라이트·깊이 정렬·픽셀 출력·QA 제작 가이드
- `design/visual/pixel-lowpoly-3d-production-guide.md`: 2026-07-27 이전 2.5D/Blender 제작 이력과 기존 산출물 해석을 위한 레거시 가이드
- `prototype/README.md`: 프로토타입 문서 성격별 하위 폴더 색인
- `prototype/official/rat-host-prototype.md`: 쥐 숙주 프로토타입 범위
- `prototype/approvals/rat-host-approval-packet.md`: 프로토타입 승인 이력과 승인 항목
- `prototype/plans/rat-host-implementation-plan.md`: 승인된 쥐 숙주 프로토타입 구현 계획
- `prototype/plans/rat-host-ai-assisted-art-workflow.md`: ChatGPT 내장 이미지 생성의 승인·프롬프트·입력 출처·선별·2D 게임 에셋 재제작·Unity QA 작업 순서
- `project/project-prep.md`: 현재 프로젝트 준비 상태
- `project-handoff/current-task-board.md`: 현재 작업 후보, 미결 검증, 최근 작업 요약
- `project-handoff/task-cost-dashboard.md`: 작업별 계획·실제 비용 proxy, 중복·폐기, 필요한 비용·회피 가능 비용과 판정 중앙 현황판
- `unity/unity-baseline-report.md`: Unity 프로젝트 읽기 전용 기준 상태
- `unity/unity-mcp-setup.md`: Unity MCP 설정과 운영 기준
