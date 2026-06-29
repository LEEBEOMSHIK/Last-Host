# 작업 배정서

## 기본 정보

- 작업 ID: `2026-06-29-agents-line-policy`
- 작업명: AGENTS.md 200줄 제한과 작업별 참조 문서 색인 정리
- 상태: 완료
- 생성일: 2026-06-29
- 담당 에이전트: 문서/릴리즈 에이전트
- 보조 에이전트: 프로젝트 조정 에이전트
- 사용 스킬: `last-host-design-keeper`, `unity-verification-runner`

## 목적

`AGENTS.md`가 200줄 이상으로 커지지 않도록 관리 규칙을 추가하고, 세부 작업 규칙은 작업별 참조 문서로 분리해 필요한 요청에서만 읽을 수 있게 한다.

## 입력 자료

- 사용자 요청
- `AGENTS.md`
- `docs/agent-skill-plan.md`
- `.agents/`
- `.codex/skills/`
- `_workspace/`

## 해야 할 일

1. 현재 `AGENTS.md` 줄 수를 확인한다.
2. `AGENTS.md`에 200줄 제한과 참조 분리 원칙을 짧게 추가한다.
3. 작업별 참조 문서 색인을 만든다.
4. 운영 문서가 참조 색인을 사용하도록 연결한다.
5. 검증 후 커밋한다.

## 산출물

- `docs/agent-reference-map.md`
- 수정된 `AGENTS.md`
- 수정된 `docs/agent-skill-plan.md`
- 완료 추적 폴더

## 금지 범위

- Unity 프로젝트 생성
- 게임플레이 코드 작성
- 에이전트/스킬 역할 변경

## 승인 필요 항목

- 없음. 사용자가 명시적으로 요청한 정책 정리 작업이다.

## 완료 기준

- `AGENTS.md`가 200줄 미만이다.
- 작업별 참조 색인 문서가 있다.
- 운영 문서가 참조 색인을 가리킨다.
- 완료 기록이 `_workspace/completed/`에 남아 있다.
