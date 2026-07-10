# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-10-project-handoff-board`
- 작업명: 프로젝트 핸드오프 상태판 정리
- 상태: 완료
- 생성일: 2026-07-10
- 담당 에이전트: 문서/릴리즈 에이전트
- 보조 에이전트: QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: last-host-design-keeper

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| 문서/릴리즈 에이전트 | 문서 구조 정리 | 사용자 확인용 handoff 폴더와 상태판 작성, 문서 색인 갱신 | `docs/project-handoff/`, `docs/README.md`, `docs/agents/agent-reference-map.md` |
| QA/검증 에이전트 | 문서 검증 | ProjectSettings diff 제거, 문서 색인 연결, handoff 운영 기준 확인 | `verification.md` |
| 프로젝트 총괄 관리자 에이전트 | 내부 승인 | 승인 범위, 문서 위치, 검증 기록 확인 | `completion-report.md` |

## 구현 담당 확인

- 코드/테스트 변경 담당: 해당 없음
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: 해당 없음
- 메인 에이전트 직접 구현 여부: 예
- 메인 에이전트 직접 구현 예외 사유: 사용자 요청은 운영 문서 정리와 기존 ProjectSettings 변경 되돌림이며, 코드/씬/ProjectSettings 신규 구현이 없다. 문서/릴리즈 에이전트 역할을 메인 에이전트가 직접 수행하고 이 작업 기록에 예외 사유를 남긴다.

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: 운영 문서 변경 및 커밋 요청
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예

## 목적

사용자와 Codex가 함께 확인할 수 있는 현재 작업 후보와 handoff 상태판을 `docs/agents/`가 아닌 별도 폴더에 정리한다. 다음 작업 발굴 결과는 누적 로그가 아니라 현재 상태판으로 갱신되도록 운영 기준을 명확히 한다. `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 변경은 사용자 요청에 따라 되돌린다.

## 입력 자료

- 사용자 요청: handoff 파일을 별도 폴더로 정리하고, 누적 로그가 아닌 현재 상태판으로 운영
- 사용자 요청: ProjectSettings `APP_UI_EDITOR_ONLY` 변경 되돌림
- `AGENTS.md`
- `docs/README.md`
- `docs/agents/agent-reference-map.md`
- `docs/agents/loop-engineering-gates.md`
- `.agents/documentation-release-agent.md`
- `.agents/project-director-agent.md`

## 해야 할 일

1. `docs/project-handoff/` 폴더를 생성한다.
2. `README.md`와 `current-task-board.md`를 작성한다.
3. `docs/README.md`와 `docs/agents/agent-reference-map.md`에 새 위치를 연결한다.
4. 기존 `docs/agents/next-task-candidates.md`를 제거한다.
5. ProjectSettings 변경을 되돌리고 diff가 없는지 확인한다.
6. 작업 기록, 검증 기록, 총괄 판정을 남긴다.

## 산출물

- `docs/project-handoff/README.md`
- `docs/project-handoff/current-task-board.md`
- `docs/README.md`
- `docs/agents/agent-reference-map.md`
- `_workspace/completed/2026-07-10-2026-07-10-project-handoff-board/`

## 에이전트 수행 이력 기록

- `agent-activity.md` 생성 여부: 생성
- 담당 에이전트별 수행 내용 기록 여부: 기록
- 위임/검토/승인 판정 기록 여부: 기록

## 금지 범위

- 게임플레이 코드, 씬, 프리팹, 패키지, ProjectSettings 신규 변경
- 새 Codex 스킬 또는 에이전트 역할 추가
- 완료된 작업 상세 이력을 사용자 handoff 문서에 누적 로그처럼 복사

## 승인 필요 항목

- 없음. 사용자가 문서 재배치와 ProjectSettings 되돌림을 직접 요청했다.

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 통과
- 담당 에이전트 산출물 확인: 통과
- 에이전트 수행 이력 확인: 통과
- 구현 담당 에이전트 확인: 해당 없음
- 메인 에이전트 직접 구현 예외 사유 확인: 통과
- QA/검증 에이전트 기록 확인: 통과
- 총괄 관리자 판정 확인: 통과
- 승인 게이트 확인: 통과
- 완료 판단에 영향을 주는 미검증 항목: 없음

## 완료 기준

- 사용자 확인용 handoff 폴더와 현재 상태판이 존재한다.
- 문서 색인에서 새 폴더를 찾을 수 있다.
- ProjectSettings `APP_UI_EDITOR_ONLY` diff가 없다.
- 커밋 전 작업 기록, QA 기록, 총괄 판정이 남아 있다.
