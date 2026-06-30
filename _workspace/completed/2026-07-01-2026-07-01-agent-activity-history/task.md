# 작업 배정서

## 기본 정보

- 작업 ID: 2026-07-01-agent-activity-history
- 작업명: 에이전트 수행 이력 기록 강화
- 상태: 진행 중
- 생성일: 2026-07-01
- 담당 에이전트: Codex 메인 에이전트
- 보조 에이전트: QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: last-host-design-keeper

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: `_workspace` 운영 문서와 템플릿 변경
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예

## 목적

완료 이력에서 어떤 에이전트가 어떤 작업을 맡았고, 어떤 산출물과 판정을 남겼는지 나중에 확인할 수 있도록 `_workspace` 기록 규칙과 템플릿을 보강한다.

## 입력 자료

- 사용자 피드백: 이력에 어떤 에이전트가 어떤 일을 처리했는지 남겨야 나중에 확인 가능함
- `AGENTS.md`
- `_workspace/README.md`
- `_workspace/templates/*`
- `docs/agents/loop-engineering-gates.md`
- 최근 완료 작업: `_workspace/completed/2026-07-01-2026-07-01-camera-view-cycle/`

## 해야 할 일

1. `_workspace` 완료 폴더 구조에 `agent-activity.md`를 추가한다.
2. 작업 템플릿에 에이전트별 역할, 수행 내용, 산출물, 판정 기록을 남기도록 보강한다.
3. 루프 게이트 문서에 에이전트 수행 이력 누락을 차단 조건으로 반영한다.
4. 최근 완료 작업에도 에이전트 수행 이력 파일을 소급 추가한다.
5. QA/검증 에이전트와 총괄 관리자 판정을 받아 완료 처리한다.

## 산출물

- `_workspace/templates/agent-activity.md`
- `_workspace/README.md` 갱신
- `_workspace/templates/*` 갱신
- `docs/agents/loop-engineering-gates.md` 갱신
- 최근 완료 작업의 `agent-activity.md`

## 금지 범위

- 게임플레이 코드 수정
- Unity 씬 또는 ProjectSettings 수정
- 에이전트 역할 자체의 추가/삭제

## 승인 필요 항목

- 없음. 사용자가 직접 요청한 이력 기록 보강이다.

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 필요
- 담당 에이전트 산출물 확인: 필요
- QA/검증 에이전트 기록 확인: 필요
- 총괄 관리자 판정 확인: 필요
- 승인 게이트 확인: 필요
- 완료 판단에 영향을 주는 미검증 항목: 없어야 함

## 완료 기준

- 새 작업부터 `agent-activity.md`로 에이전트별 수행 이력을 확인할 수 있다.
- 최근 완료된 카메라 작업에도 에이전트 수행 이력이 소급 기록되어 있다.
- QA/검증과 총괄 관리자 게이트가 완료되어 있다.
