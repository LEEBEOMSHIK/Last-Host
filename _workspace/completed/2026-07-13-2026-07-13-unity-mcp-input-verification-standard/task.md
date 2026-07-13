# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-13-unity-mcp-input-verification-standard`
- 작업명: Unity MCP 입력 검증 방식 표준화
- 상태: 진행 중
- 생성일: 2026-07-13
- 담당 에이전트: 문서/릴리즈 에이전트
- 보조 에이전트: QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `unity-verification-runner`

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| 문서/릴리즈 에이전트 | 운영 문서 담당 | 기존 MCP 입력 검증 증적을 바탕으로 표준 절차와 상태판을 갱신 | `docs/unity/unity-mcp-setup.md`, `docs/project-handoff/current-task-board.md`, 문서 인계 |
| QA/검증 에이전트 | 독립 검증 | 표준 절차가 과거 증적과 일치하고 실제 입력/대체 검증을 혼동하지 않는지 확인 | `verification.md`, 상태판 동기화 확인 |
| 프로젝트 총괄 관리자 에이전트 | 내부 승인 | 범위·승인 게이트·QA 기록·상태판 완료 반영을 검토 | `director-review.md`, 완료 판정 |

## 구현 담당 확인

- 코드/테스트 변경 담당: 해당 없음
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: 해당 없음
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 해당 없음

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: Unity MCP 운영 문서와 사용자용 현재 상태판을 변경한다.
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예

## 목적

Unity MCP로 입력을 검증할 때 실제 Game View 키 입력 검증, MCP 직접 상태 전환에 의한 대체 검증, relay 또는 승인 문제로 인한 미검증을 명확히 구분하는 재현 가능한 표준 절차를 운영 문서에 반영한다.

## 입력 자료

- `AGENTS.md`
- `docs/unity/unity-mcp-setup.md`
- `docs/project-handoff/current-task-board.md`
- `_workspace/completed/2026-07-10-2026-07-10-mcp-recovered-signal-cue-verification/verification.md`
- `_workspace/completed/2026-07-10-2026-07-10-mcp-recovered-signal-cue-verification/work-log.md`
- `_workspace/completed/2026-07-13-2026-07-13-alert-cause-event-type-review/verification.md`
- `.agents/documentation-release-agent.md`
- `.agents/qa-verification-agent.md`
- `.agents/project-director-agent.md`

## 해야 할 일

1. 실제 키 입력 검증의 사전 조건, 실행 순서, 판정 근거와 종료 정리 절차를 문서화한다.
2. MCP 직접 상태 전환과 런타임 조회를 입력 검증의 대체 검증으로 분리하고, 통과로 오인하지 않도록 기록 규칙을 명시한다.
3. `SendKeys` 실패, Game View 포커스 미확보, MCP 승인 대기·relay 무응답을 미검증 또는 차단으로 기록하는 기준을 명시한다.
4. 입력 방식·포커스·기대/실제 상태·콘솔·대체 검증·남은 위험을 남기는 최소 기록 양식을 추가한다.
5. 상태판에서 이 후보를 완료 요약으로 옮기고, 다음 후보에서 제거한다. 완료 경로는 `_workspace/completed/2026-07-13-2026-07-13-unity-mcp-input-verification-standard/`로 정확히 기록한다.

## 산출물

- `docs/unity/unity-mcp-setup.md`의 Unity MCP 입력 검증 표준 절차
- `docs/project-handoff/current-task-board.md`의 완료 상태 동기화
- 작업 폴더의 문서 인계, QA 검증, 총괄 검토, 완료 보고

## 에이전트 수행 이력 기록

- `agent-activity.md` 생성 여부: 생성 완료
- 담당 에이전트별 수행 내용 기록 여부: 진행 중
- 위임/검토/승인 판정 기록 여부: 진행 중

## 금지 범위

- Unity 코드, 테스트, 씬, 프리팹, 에셋, 패키지, `ProjectSettings`, `.codex/config.toml`을 변경하지 않는다.
- Unity Editor 프로세스 시작·종료, Game View 입력, relay 설정 변경을 이번 문서 작업의 수행 수단으로 사용하지 않는다.
- 사용자 수동 플레이 체감 확인을 자동 검증 통과로 대체하지 않는다.
- 기존 완료 작업 또는 다른 active 작업의 상태를 임의로 변경하지 않는다.

## 승인 필요 항목

- 사용자 요청으로 운영 문서 표준화 작업은 승인됐다.
- 추가 Unity 프로젝트 변경, MCP 설정 변경, 새 도구·패키지 설치는 해당 없음이며 수행하지 않는다.

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 생성 완료
- 담당 에이전트 산출물 확인: 대기
- 에이전트 수행 이력 확인: 대기
- 구현 담당 에이전트 확인: 해당 없음, 문서 담당 위임
- 메인 에이전트 직접 구현 예외 사유 확인: 해당 없음
- QA/검증 에이전트 기록 확인: 대기
- 총괄 관리자 판정 확인: 대기
- 승인 게이트 확인: 통과
- 완료 판단에 영향을 주는 미검증 항목: 문서 내용의 증적 대조와 상태판 완료 경로 존재 여부

## 완료 기준

- 실제 키 입력, 대체 상태 전환, 미검증/차단 상태의 구분과 기록 양식이 운영 문서에 반영된다.
- 문서 내용이 2026-07-10의 네이티브 F6 성공·`SendKeys` 실패·MCP 승인/relay 제한 증적과 모순되지 않는다.
- 상태판에서 후보가 제거되고 완료 경로가 실제 completed 폴더와 일치한다.
- QA/검증 에이전트의 완료 가능 판단과 프로젝트 총괄 관리자 에이전트의 `내부 승인 가능` 판정이 남는다.
