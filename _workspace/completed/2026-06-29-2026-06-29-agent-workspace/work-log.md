# 작업 로그

## 작업 ID

`2026-06-29-agent-workspace`

## 로그

### 2026-06-29

- 수행 내용: `_workspace` 구조와 템플릿을 추가했다.
- 확인한 자료: `AGENTS.md`, `.agents/agent-roster.md`, `docs/agent-skill-plan.md`, `skill-creator` 지침.
- 판단: 작업 배정과 진행 기록은 `active`, 완료 기록은 `completed`로 분리하는 구조가 적절하다.
- 다음 작업: 구조와 문서 참조 검증 후 커밋한다.

## 결정 기록

- 작업 ID 형식은 `YYYY-MM-DD-short-topic`으로 한다.
- 완료 폴더는 `_workspace/completed/<완료일>-<작업ID>/` 형식으로 둔다.
- 완료 폴더에는 `task.md`, `work-log.md`, `completion-report.md`, `verification.md`, `artifacts/`를 둔다.
- 빌드 산출물, 대용량 에셋, 임시 캐시는 `_workspace`에 보관하지 않는다.

## 열린 질문

- 없음.

## 위험과 주의점

- 완료 기록이 형식만 채우는 문서가 되지 않도록 실제 검증 결과와 미검증 항목을 반드시 기록해야 한다.
- 사용자 승인 대기 항목이 있으면 완료가 아니라 `승인 대기`로 표시해야 한다.
