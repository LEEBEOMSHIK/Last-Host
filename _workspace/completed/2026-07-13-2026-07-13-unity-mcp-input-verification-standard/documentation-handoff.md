# 문서/릴리즈 인계

## 작업 ID

`2026-07-13-unity-mcp-input-verification-standard`

## 문서 반영

- `docs/unity/unity-mcp-setup.md`에 실제 키 입력 검증, MCP 직접 상태 전환 대체 검증, 미검증·차단을 구분하는 표준 절차와 최소 기록 양식을 추가했다.
- `docs/project-handoff/current-task-board.md`에서 후보를 제거하고 완료 요약으로 옮겼다. 완료 패킷 링크는 `_workspace/completed/2026-07-13-2026-07-13-unity-mcp-input-verification-standard/`로 고정했다.

## 증적 대조

- 2026-07-10 기록의 Game View 포커스 후 네이티브 `keybd_event` F6 입력 성공과 `InternalVirus / ImmuneSignalSuppression`·HUD 관측 결과를 실제 키 입력의 근거로 반영했다.
- 같은 기록의 일반 `SendKeys` F6 실패를 표준 입력 경로가 아닌 보조 시도·미검증 조건으로 반영했다.
- 승인 대기(`Awaiting user approval`, `Connection revoked`)와 relay 무응답·시간 초과는 실제 입력 검증의 차단 조건으로 반영했다.

## 작업영역

- 운영 문서와 상태판만 수정했다.
- Unity 코드, 테스트, 씬, 에셋, ProjectSettings, `.codex/config.toml`, Unity 실행 상태는 변경하지 않았다.

## QA 인계 요청

1. 세 결과 분류가 과거 증적과 모순되지 않는지 확인한다.
2. 실제 키 입력 통과 조건에 포커스·네이티브 이벤트·관측 상태/HUD가 모두 있는지 확인한다.
3. 상태판의 후보 제거와 완료 경로를 확인한다. completed 이동 후에는 해당 경로가 실제 존재하는지 재확인한다.

## 커밋 메시지 후보

`docs: standardize Unity MCP input verification`

## 남은 작업

- QA/검증 에이전트의 독립 대조
- completed 이동 후 상태판 경로 존재 여부 재확인
- 프로젝트 총괄 관리자 내부 승인
