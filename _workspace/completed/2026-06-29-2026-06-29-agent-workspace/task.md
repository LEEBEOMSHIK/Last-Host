# 작업 배정서

## 기본 정보

- 작업 ID: `2026-06-29-agent-workspace`
- 작업명: 에이전트 간 공유 작업영역과 완료 추적 구조 생성
- 상태: 완료
- 생성일: 2026-06-29
- 담당 에이전트: 프로젝트 조정 에이전트
- 보조 에이전트: 문서/릴리즈 에이전트, QA/검증 에이전트
- 사용 스킬: `skill-creator`, `unity-verification-runner`

## 목적

에이전트 간 작업 배정, 진행 기록, 핸드오프, 완료 추적을 위한 프로젝트 로컬 `_workspace` 폴더를 만들고 운영 절차를 문서화한다.

## 입력 자료

- 사용자 요청
- `AGENTS.md`
- `.agents/agent-roster.md`
- `docs/agent-skill-plan.md`
- `.codex/skills/`

## 해야 할 일

1. `_workspace` 기본 폴더 구조를 만든다.
2. 진행 중 작업과 완료 작업 추적 규칙을 작성한다.
3. 작업 배정, 로그, 핸드오프, 완료 보고, 검증 템플릿을 만든다.
4. 에이전트 운영 문서와 관련 에이전트/스킬에 `_workspace` 참조를 추가한다.
5. 이번 작업의 완료 기록을 `_workspace/completed/`에 남긴다.

## 산출물

- `_workspace/README.md`
- `_workspace/active/README.md`
- `_workspace/completed/README.md`
- `_workspace/templates/*.md`
- `_workspace/completed/2026-06-29-2026-06-29-agent-workspace/`
- 관련 운영 문서 갱신

## 금지 범위

- Unity 프로젝트 생성
- 게임플레이 코드 작성
- 전역 Codex 스킬 설치

## 승인 필요 항목

- 없음. 사용자가 `_workspace` 생성과 설정을 명시 요청했다.

## 완료 기준

- `_workspace` 구조가 생성되어 있다.
- 템플릿이 생성되어 있다.
- 운영 문서가 `_workspace`를 참조한다.
- 완료 추적 폴더에 작업 경위와 완료 근거가 남아 있다.
