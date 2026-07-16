# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-16-current-task-board-consistency`
- 작업명: 현재 현황판·CURRENT·완료 작업 보관 정합성 복구
- 상태: 진행 중
- 생성일: 2026-07-16
- 담당 에이전트: 문서/릴리즈 에이전트
- 보조 에이전트: 프로젝트 조정 에이전트, QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `last-host-design-keeper`

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| 프로젝트 조정 에이전트 | 작업 조정 | 사용자 요청, 범위, 금지 사항, 완료 기준을 작업 패킷으로 고정 | `task.md`, `work-log.md`, `agent-activity.md` |
| 문서/릴리즈 에이전트 | 운영 문서 및 기록 정리 | 현재 상태판과 `CURRENT.md`를 실제 상태와 맞추고, 보관 가능한 완료 작업만 `completed/`로 이동 | 상태판, `CURRENT.md`, 완료 작업 경로, 작업 로그 |
| QA/검증 에이전트 | 독립 정합성 검증 | 완료 경로, 후보·보류 중복, Git 상태, 필수 완료 기록을 실제 파일과 대조 | `verification.md`와 QA 완료 판단 |
| 프로젝트 총괄 관리자 에이전트 | 내부 승인 | 범위, 승인 게이트, QA 기록을 검토하고 최종 판정 | `completion-report.md`의 총괄 판정 |

## 구현 담당 확인

- 코드/테스트 변경 담당: 해당 없음
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: 해당 없음
- 운영 문서 및 작업 기록 변경 담당: 문서/릴리즈 에이전트
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 해당 없음

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: 사용자 공유 운영 문서와 `_workspace` 완료 보관 경로를 변경한다.
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예

## 목적

사용자 수동 플레이 체감 확인은 보류로 유지하면서, `docs/project-handoff/current-task-board.md`, `_workspace/active/CURRENT.md`, 완료되었으나 `active/`에 남은 작업 기록의 상태를 현재 사실과 일치시킨다.

## 입력 자료

- 사용자 요청: 수동 플레이 체감 확인은 보류하고 현황판·작업 기록 정합성 복구 진행
- `AGENTS.md`
- `docs/agents/agent-reference-map.md`
- `docs/agents/loop-engineering-gates.md`
- `docs/project-handoff/README.md`
- `docs/project-handoff/current-task-board.md`
- `_workspace/README.md`
- `_workspace/active/CURRENT.md`
- `_workspace/active/2026-07-01-rat-host-full-play-verification/`
- `_workspace/active/2026-07-09-white-blood-cell-response-scaling/`
- `_workspace/active/2026-07-10-signal-suppression-approach-cue/`

## 해야 할 일

1. 현재 Git HEAD, 원격 반영 상태, 미커밋 변경을 읽기 전용으로 확인하고 상태판의 저장소 상태를 실제 사실과 맞춘다.
2. 사용자 수동 플레이 체감 확인을 `보류 항목`에만 유지하고 다음 작업 후보와 중복하지 않는다.
3. 완료 작업 3건의 `completion-report.md`, `verification.md`, `agent-activity.md` 내 QA 및 총괄 관리자 판정을 확인한다.
4. 필수 게이트가 모두 충족된 작업만 `_workspace/completed/2026-07-16-<기존 작업 ID>/`로 보관하고, 미충족 작업은 `active/`에 유지한 채 필요한 후속 게이트를 기록한다.
5. 실제 진행 중인 작업을 기준으로 `_workspace/active/CURRENT.md`를 짧은 포인터 형식으로 갱신한다.
6. 상태판의 최근 작업 요약, 완료 경로, 후보, 보류, Git 상태를 갱신한다.
7. QA/검증 에이전트가 실제 경로와 상태판을 독립 대조한 뒤 프로젝트 총괄 관리자 판정을 받는다.

## 산출물

- `docs/project-handoff/current-task-board.md`
- `_workspace/active/CURRENT.md`
- 게이트를 충족한 기존 완료 작업의 `_workspace/completed/2026-07-16-<기존 작업 ID>/` 보관 경로
- `_workspace/active/2026-07-16-current-task-board-consistency/work-log.md`
- `_workspace/active/2026-07-16-current-task-board-consistency/agent-activity.md`
- 완료 단계의 `verification.md`, `completion-report.md`

## 에이전트 수행 이력 기록

- `agent-activity.md` 생성 여부: 예
- 담당 에이전트별 수행 내용 기록 여부: 진행 중
- 위임/검토/승인 판정 기록 여부: 진행 중

## 금지 범위

- Unity 코드, 테스트, 씬, 프리팹, 패키지, ProjectSettings 변경
- `.codex/config.toml` 수정 또는 이동
- 기존 사용자 변경 되돌림 또는 관련 없는 파일 정리
- 필수 QA/총괄 판정이 없는 작업의 완료 보관
- 사용자 수동 플레이 체감 확인을 완료로 처리하거나 다음 작업 후보에 중복 기재
- 커밋 생성

## 승인 필요 항목

- 없음. 사용자가 이번 정합성 복구를 승인했다.
- 단, 기존 기록만으로 보관 게이트가 충족되지 않는 작업은 임의 완료하지 않고 QA/총괄 판정을 추가로 받아야 한다.

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 생성 완료
- 담당 에이전트 산출물 확인: 대기
- 에이전트 수행 이력 확인: 진행 중
- 구현 담당 에이전트 확인: 코드/씬 구현 없음
- 메인 에이전트 직접 구현 예외 사유 확인: 해당 없음
- QA/검증 에이전트 기록 확인: 대기
- 총괄 관리자 판정 확인: 대기
- 승인 게이트 확인: 사용자 승인 확인
- 완료 판단에 영향을 주는 미검증 항목: `2026-07-10-signal-suppression-approach-cue`의 QA 부분 통과와 총괄 판정 누락

## 완료 기준

- 수동 플레이 체감 확인이 상태판의 보류 항목으로만 유지된다.
- 상태판과 `CURRENT.md`가 실제 활성 작업, 완료 경로, Git 상태를 가리킨다.
- 완료 작업 3건 각각의 필수 기록 존재 여부와 보관 가능 여부가 근거와 함께 기록된다.
- 게이트 충족 작업만 `completed/`에 보관되고, 미충족 작업은 `active/`에 남아 후속 조치가 명시된다.
- 상태판의 후보·보류·최근 요약에 중복이 없고 모든 완료 경로가 실제로 존재한다.
- QA/검증 에이전트가 독립 대조 후 완료 가능으로 판단한다.
- 프로젝트 총괄 관리자 에이전트가 `내부 승인 가능`으로 판정한다.
- Unity 프로젝트, `.codex/config.toml`, 범위 밖 기존 변경은 수정되지 않는다.
- 커밋은 생성하지 않는다.
