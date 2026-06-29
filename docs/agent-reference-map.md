# 작업별 참조 문서 색인

최종 수정일: 2026-06-29

## 목적

이 문서는 `AGENTS.md`를 200줄 미만의 상위 규칙 파일로 유지하기 위한 참조 색인이다. 작업 요청이 들어오면 이 문서에서 해당 작업 유형을 찾고, 필요한 세부 문서만 읽는다.

## 운영 원칙

- `AGENTS.md`에는 전역 원칙, 승인 게이트, 참조 위치만 둔다.
- 세부 절차가 길어지면 이 문서 또는 작업별 참조 문서로 분리한다.
- 새 작업 유형이 생기면 이 문서에 참조 위치를 추가한다.
- 참조 문서를 추가할 때는 어떤 요청에서 읽어야 하는지 함께 적는다.

## 공통 참조 순서

모든 작업은 기본적으로 다음 순서를 따른다.

1. `AGENTS.md`
2. 이 문서
3. 작업 유형별 필수 참조 문서
4. 필요한 경우 작업 유형별 선택 참조 문서

## 작업 유형별 참조

### 기획 정리 / 원본 기획 확인

사용 요청 예시:

- 기획서 내용을 다시 정리해 달라는 요청
- 원본 `.docx`와 현재 문서의 차이 확인
- 게임 방향, 메시지, 프로토타입 범위 확인

필수 참조:

- `docs/game-design-summary.md`
- `docs/rat-host-prototype.md`
- `.codex/skills/last-host-design-keeper/SKILL.md`

선택 참조:

- `C:\project\game\last_host\last_host_game_plan.docx`
- `.codex/skills/last-host-design-keeper/references/design-guardrails.md`

### Unity 프로젝트 준비 / 아키텍처

사용 요청 예시:

- Unity 프로젝트 생성 준비
- Unity 버전, URP, 폴더 구조, 씬 구조 결정
- 시스템 경계 설계

필수 참조:

- `docs/project-prep.md`
- `.codex/skills/unity-prototype-planner/SKILL.md`

선택 참조:

- `.codex/skills/unity-prototype-planner/references/unity-architecture.md`

### Unity MCP / Unity Editor 자동화

사용 요청 예시:

- Unity MCP 설정 요청
- Codex에서 Unity Editor를 MCP로 연결하려는 요청
- MCP를 통해 씬, 에셋, 패키지, 코드 작업을 준비하려는 요청

필수 참조:

- `docs/unity-mcp-setup.md`
- `.codex/config.toml`
- `.agents/unity-architecture-agent.md`

선택 참조:

- `.codex/mcp/start-mcp-unity.ps1`
- `.codex/skills/unity-prototype-planner/SKILL.md`
- `_workspace/templates/verification.md`

### 쥐 숙주 프로토타입 / 핵심 루프

사용 요청 예시:

- 쥐 조종 설계
- 면역 경계도 설계
- 내부 바이러스 미니게임 설계
- 변이 선택 루프 설계

필수 참조:

- `docs/rat-host-prototype.md`
- `.codex/skills/rat-host-loop-builder/SKILL.md`

선택 참조:

- `docs/rat-host-approval-packet.md`
- `.codex/skills/rat-host-loop-builder/references/rat-loop-rules.md`

### 도트풍 저폴리 3D / 비주얼

사용 요청 예시:

- 비주얼 스타일 확인
- 저폴리 모델, 픽셀 텍스처, 저해상도 렌더링 기준 확인
- 카메라와 플레이스홀더 에셋 기준 정리

필수 참조:

- `docs/game-design-summary.md`
- `.codex/skills/pixel-lowpoly-style-keeper/SKILL.md`

선택 참조:

- `.codex/skills/pixel-lowpoly-style-keeper/references/pixel-style-rules.md`

### 에이전트 배정 / 스킬 운영

사용 요청 예시:

- 어떤 에이전트에게 일을 맡길지 정리
- 에이전트 간 핸드오프 절차 확인
- 스킬 추가 또는 역할 변경 검토

필수 참조:

- `docs/agent-skill-plan.md`
- `.agents/agent-roster.md`

선택 참조:

- `.agents/project-coordinator-agent.md`
- `.agents/design-keeper-agent.md`
- `.agents/unity-architecture-agent.md`
- `.agents/gameplay-loop-agent.md`
- `.agents/visual-tech-art-agent.md`
- `.agents/qa-verification-agent.md`
- `.agents/documentation-release-agent.md`

### 에이전트 작업영역 / 완료 추적

사용 요청 예시:

- 에이전트 작업 폴더 생성
- 진행 중 작업 기록
- 완료 작업 보고서 작성
- 핸드오프 기록 작성

필수 참조:

- `_workspace/README.md`
- `_workspace/active/README.md`
- `_workspace/completed/README.md`

선택 참조:

- `_workspace/templates/task.md`
- `_workspace/templates/work-log.md`
- `_workspace/templates/handoff.md`
- `_workspace/templates/completion-report.md`
- `_workspace/templates/verification.md`

### 검증 / 완료 판단

사용 요청 예시:

- 완료 여부 판단
- 테스트, 빌드, 검증 체크리스트 작성
- 미검증 항목 정리

필수 참조:

- `.codex/skills/unity-verification-runner/SKILL.md`
- `_workspace/templates/verification.md`

선택 참조:

- `.codex/skills/unity-verification-runner/references/verification-rules.md`

### 문서 정리 / 커밋 요약 / 릴리즈 정리

사용 요청 예시:

- 변경사항 문서화
- 완료 보고 작성
- 커밋 메시지 후보 작성
- 남은 작업 정리

필수 참조:

- `.agents/documentation-release-agent.md`
- `_workspace/templates/completion-report.md`

선택 참조:

- `docs/agent-skill-plan.md`
- `_workspace/templates/verification.md`

## AGENTS.md가 200줄에 가까워질 때

다음 절차를 따른다.

1. 새 내용이 전역 원칙인지 작업별 세부 절차인지 구분한다.
2. 전역 원칙이면 `AGENTS.md`에 짧게 추가한다.
3. 세부 절차이면 작업별 문서나 스킬 reference에 추가한다.
4. 이 문서에 해당 참조 위치를 추가한다.
5. `AGENTS.md` 줄 수가 200줄 미만인지 확인한다.

## 새 참조 문서 추가 규칙

새 참조 문서를 만들 때는 다음 정보를 포함한다.

- 어떤 요청에서 읽어야 하는지
- 필수 참조인지 선택 참조인지
- 관련 에이전트 또는 스킬
- 승인 게이트가 있는지
