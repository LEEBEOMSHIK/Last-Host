# 핸드오프 기록

## 작업 ID

`2026-07-08-session-continuity-rules`

## 최신 사용자 요청

토큰/세션이 끊기기 전에 다른 AI 또는 다른 세션이 이어받을 수 있도록 기준을 잡아서 추가한다.

## 현재 상태

- 상태: 검증 완료, 사용자 확인 대기
- 여기서 멈춤: `_workspace` 운영 기준, `handoff.md` 템플릿, `CURRENT.md` 포인터를 추가하고 검증했다.
- 다음 세션의 첫 목표: 사용자가 기준을 확인한 뒤 필요하면 조정하거나, 커밋 요청 시 관련 파일만 선별해 커밋한다.

## 넘기는 에이전트

프로젝트 조정 에이전트

## 받는 에이전트

QA/검증 에이전트 또는 다음 세션의 프로젝트 조정 에이전트

## 먼저 읽을 파일

1. `_workspace/README.md`
2. `_workspace/templates/handoff.md`
3. `_workspace/active/CURRENT.md`

## 변경한 파일

- `_workspace/README.md`
- `_workspace/templates/handoff.md`
- `_workspace/templates/current.md`
- `_workspace/active/CURRENT.md`
- `_workspace/active/2026-07-08-session-continuity-rules/*`

## 건드리면 안 되는 기존 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 기존 `APP_UI_EDITOR_ONLY` define 변경
- 기존 활성 작업 폴더의 완료/이동 처리

## 마지막 성공 검증

- `git diff --check -- _workspace/README.md _workspace/templates/handoff.md _workspace/templates/current.md _workspace/active/CURRENT.md _workspace/active/2026-07-08-session-continuity-rules`
- `rg -n "세션 연속성 기준|토큰 경계|CURRENT.md|handoff.md|약 80%|구현 완료 후 검증 전" ...`

## 실패 또는 차단된 검증

- 없음

## 루프 게이트 상태

- 작업 배정 게이트: 완료
- 담당 산출물 게이트: 문서 반영 완료
- QA/검증 게이트: 완료
- 총괄 관리자 게이트: 내부 승인 가능
- 커밋 전 차단 조건: 커밋 요청 시 관련 파일만 선별하고 기존 `ProjectSettings.asset` 변경 제외

## 넘기는 이유

세션 연속성 기준 자체를 추가하는 작업이므로, 새 기준에 맞춘 실제 핸드오프 예시를 함께 남긴다.

## 넘기는 에이전트가 완료한 일

- 작업 패킷 생성
- 전역 현재 세션 포인터 추가
- 핸드오프 템플릿 보강
- `_workspace` 운영 문서에 토큰 경계 기준 추가

## 받는 에이전트에게 기대하는 산출물

- `verification.md`
- `agent-activity.md` QA/총괄 판정 갱신
- 사용자 확인 보고

## 이어서 해야 할 일

1. 사용자 확인 결과를 반영한다.
2. 커밋 요청 시 관련 파일만 스테이징한다.
3. 다음 구현 작업 전 `CURRENT.md`를 새 작업으로 갱신한다.

## 참고 자료

- `docs/agents/agent-skill-plan.md`
- `_workspace/README.md`

## 에이전트 수행 이력 갱신

- `agent-activity.md`에 인계 기록 추가 여부: 필요
- 인계 결과 기록 책임자: QA/검증 에이전트 또는 다음 세션 조정자

## 주의할 점

- 이 작업은 운영 문서 변경이며 새 에이전트/스킬 생성이 아니다.
- AGENTS.md에는 추가하지 않고 `_workspace` 문서에만 둔다.

## 사용자 승인 필요

- 없음. 사용자가 기준 추가를 승인했다.

## 토큰 경계 메모

- 인수인계가 필요한 단계: 사용자 확인 대기
- 토큰 압박 체감: 낮음
- 새 구현 금지 여부: 해당 없음
