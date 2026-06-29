# 완료 보고

## 요약

Unity MCP를 프로젝트에 종속되게 사용할 수 있도록 Codex 프로젝트 로컬 설정과 활성화 절차를 준비했다.

## 변경 내용

- `.codex/config.toml`에 `mcp-unity` 서버 항목을 추가했다.
- `.codex/mcp/start-mcp-unity.ps1`로 저장소 루트와 하위 Unity 프로젝트의 MCP 서버 파일을 탐색하도록 했다.
- `docs/unity-mcp-setup.md`에 설치, 활성화, 승인 게이트, 문제 해결 절차를 정리했다.
- `docs/agent-reference-map.md`에 Unity MCP 작업 유형을 추가했다.
- `.agents/unity-architecture-agent.md`와 `.codex/skills/unity-prototype-planner/SKILL.md`에 Unity MCP 참조를 추가했다.
- `docs/agent-skill-plan.md`에 Unity MCP 설정 상태와 승인 항목을 반영했다.
- 현재 로컬 Unity 프로젝트 경로가 `My project/`임을 문서에 반영했다.

## 완료 상태

설정 준비는 완료했다. 실제 MCP 활성화는 Unity 프로젝트 생성과 패키지 설치 승인 이후 진행한다.

## 남은 승인 필요 항목

- Unity 프로젝트 생성
- `com.gamelovers.mcp-unity` 패키지 설치
- Unity Editor에서 MCP 서버 설치 및 시작
- `.codex/config.toml`의 `enabled = true` 변경
