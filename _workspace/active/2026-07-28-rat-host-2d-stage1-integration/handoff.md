# 핸드오프

## 현재 상태

`진행 중 — 게임플레이 구현·씬 통합 분리 배정`

## 구현 순서

1. 게임플레이 구현 에이전트가 2D 세션·숙주 본능 어댑터·오염 구역·테스트를 구현한다.
2. Unity 씬/통합 구현 에이전트가 별도 씬·씬 빌더·HUD·카메라·전환 셸을 연결한다.
3. QA/검증 에이전트가 전체 회귀, MCP Play, Console, Windows 임시 빌드를 독립 검증한다.
4. 프로젝트 총괄 관리자 에이전트가 QA 기록과 사용자 확인 경계를 판정한다.

## 보호 대상

- `RatHostPrototype.unity`
- `RatHost2DTechnicalSample.unity`
- `Scripts/TechnicalSample2D/**`
- 사용자 `APP_UI_EDITOR_ONLY`
- `_workspace/previews/`
- 저장소 `Builds/`
