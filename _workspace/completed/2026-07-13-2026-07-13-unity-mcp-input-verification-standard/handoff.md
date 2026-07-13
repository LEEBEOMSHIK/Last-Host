# 인계 메모

## 최신 사용자 요청

Unity MCP 입력 검증 방식 표준화 작업 진행.

## 현재 상태와 멈춘 지점

- 상태: 문서 반영 전
- 범위: Unity MCP 운영 문서와 현재 작업 상태판만 변경한다.
- 코드, 씬, 테스트, ProjectSettings, MCP 설정은 변경하지 않는다.

## 반드시 반영할 기준

1. Game View 포커스 후 네이티브 키 입력과 관측 가능한 상태/HUD 변화가 함께 확인돼야 실제 입력 검증이다.
2. `Unity_RunCommand` 등 직접 상태 전환은 기능 상태 대체 검증일 뿐, 실제 키 입력 수신 증명은 아니다.
3. `SendKeys` 실패, 포커스 미확보, MCP 승인 대기, relay 무응답은 입력 검증 미완료 또는 차단으로 기록한다.
4. Play 종료, 콘솔 Error/Warning, 씬 dirty 상태를 검증 기록에 남긴다.

## 근거 기록

- `_workspace/completed/2026-07-10-2026-07-10-mcp-recovered-signal-cue-verification/verification.md`
- `_workspace/completed/2026-07-10-2026-07-10-mcp-recovered-signal-cue-verification/work-log.md`
- `_workspace/completed/2026-07-13-2026-07-13-alert-cause-event-type-review/verification.md`

## 다음 작업

1. 문서/릴리즈 에이전트가 표준 절차와 상태판 초안을 반영한다.
2. QA/검증 에이전트가 증적·문서·상태판을 독립 대조한다.
3. completed 이동 후 QA가 상태판 경로 존재 여부를 재확인하고 총괄 관리자가 최종 판정한다.
