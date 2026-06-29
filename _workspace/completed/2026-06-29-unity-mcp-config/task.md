# 작업 기록: Unity MCP 프로젝트 로컬 설정

## 작업 ID

2026-06-29-unity-mcp-config

## 목적

`마지막 숙주 / The Last Host` 프로젝트에서 Codex가 Unity MCP를 프로젝트 종속 설정으로 사용할 수 있도록 준비한다.

## 입력 자료

- 사용자 요청: Unity MCP로 작업할 것이므로 프로젝트 종속 설정 필요
- `AGENTS.md`
- `docs/agent-reference-map.md`
- `docs/agent-skill-plan.md`
- Codex MCP 설정 문서
- `CoderGamester/mcp-unity` README

## 산출물

- `.codex/config.toml`
- `.codex/mcp/start-mcp-unity.ps1`
- `docs/unity-mcp-setup.md`
- 관련 에이전트/스킬 참조 갱신

## 금지 범위

- Unity 프로젝트 생성
- Unity 패키지 설치
- Unity Editor 실행
- MCP 활성화
- 게임플레이 코드 작성

## 승인 필요 항목

- Unity 프로젝트 생성
- `com.gamelovers.mcp-unity` 패키지 설치
- `.codex/config.toml`에서 `mcp-unity` 활성화
- MCP를 통한 Unity Editor 작업
