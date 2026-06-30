# 작업 배정서

## 기본 정보

- 작업 ID: 2026-07-01-implementation-agent-role-split
- 작업명: 구현 전담 에이전트 추가 및 메인 에이전트 역할 보정
- 상태: 진행 중
- 생성일: 2026-07-01
- 담당 에이전트: Codex 메인 에이전트
- 보조 에이전트: QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: last-host-design-keeper

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| Codex 메인 에이전트 | 조정자/문서 작업자 | 작업 패킷 생성, 역할 문서 보정, 완료 기록 정리 | 에이전트 파일, 운영 문서 변경, 완료 기록 |
| QA/검증 에이전트 | 검증자 | 새 역할과 게이트가 사용자 요구를 충족하는지 검토 | `verification.md` 판정 |
| 프로젝트 총괄 관리자 에이전트 | 내부 승인자 | 역할 추가와 책임 변경 승인 여부 판정 | `completion-report.md` 판정 |

## 구현 담당 확인

- 코드/테스트 변경 담당: 해당 없음
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: 해당 없음
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 해당 없음. 이번 작업은 운영 문서와 에이전트 역할 파일 변경이다.

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: 에이전트 역할 추가와 책임 범위 변경
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예

## 목적

메인 에이전트가 코드/씬/테스트 구현을 직접 처리하지 않도록 하고, 실제 구현은 전담 에이전트에게 배정되는 구조로 보정한다.

## 입력 자료

- 사용자 승인: 구현 전담 에이전트를 세분화해 추가하고 보정 작업 진행
- `.agents/agent-roster.md`
- `.agents/project-coordinator-agent.md`
- `docs/agents/agent-skill-plan.md`
- `docs/agents/loop-engineering-gates.md`
- `AGENTS.md`
- `_workspace/templates/*`
- `_workspace/completed/2026-07-01-2026-07-01-camera-view-cycle/`

## 해야 할 일

1. `게임플레이 구현 에이전트` 파일을 추가한다.
2. `Unity 씬/통합 구현 에이전트` 파일을 추가한다.
3. 에이전트 목록과 운영 문서에 두 역할을 반영한다.
4. 코드/씬/테스트 변경 시 메인 에이전트 직접 구현을 예외로 제한한다.
5. 작업 배정 템플릿에 구현 담당 에이전트 확인 항목을 추가한다.
6. 직전 카메라 작업 이력에 역할 분리 미흡 보정 메모를 남긴다.
7. QA/검증과 총괄 관리자 판정을 받는다.

## 산출물

- `.agents/gameplay-implementation-agent.md`
- `.agents/unity-scene-integration-agent.md`
- `.agents/agent-roster.md` 갱신
- `.agents/project-coordinator-agent.md` 갱신
- `docs/agents/agent-skill-plan.md` 갱신
- `docs/agents/agent-reference-map.md` 갱신
- `docs/agents/loop-engineering-gates.md` 갱신
- `AGENTS.md` 갱신
- `_workspace/templates/task.md` 갱신
- 직전 카메라 작업 이력 보정

## 에이전트 수행 이력 기록

- `agent-activity.md` 생성 여부: 예
- 담당 에이전트별 수행 내용 기록 여부: 진행 중
- 위임/검토/승인 판정 기록 여부: 진행 중

## 금지 범위

- Unity 코드, 씬, ProjectSettings 수정
- 새 Codex 스킬 생성
- 게임플레이 범위 변경
- 기존 사용자 변경인 `.codex/config.toml` 수정

## 승인 필요 항목

- 에이전트 역할 추가와 책임 변경: 사용자 승인 완료

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 필요
- 담당 에이전트 산출물 확인: 필요
- 에이전트 수행 이력 확인: 필요
- 구현 담당 에이전트 확인: 필요
- 메인 에이전트 직접 구현 예외 사유 확인: 해당 없음
- QA/검증 에이전트 기록 확인: 필요
- 총괄 관리자 판정 확인: 필요
- 승인 게이트 확인: 사용자 승인 확인 필요
- 완료 판단에 영향을 주는 미검증 항목: 없어야 함

## 완료 기준

- 두 구현 전담 에이전트가 `.agents/`에 추가된다.
- 코드/씬/테스트 변경은 구현 담당 에이전트가 맡아야 한다는 규칙이 문서화된다.
- 메인 에이전트 직접 구현 예외 조건이 명확해진다.
- 직전 카메라 작업의 절차 미흡이 이력에 보정 기록으로 남는다.
- QA/검증과 총괄 관리자 판정이 완료된다.
