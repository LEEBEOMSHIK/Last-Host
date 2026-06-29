# 에이전트 작업영역

`_workspace`는 에이전트 간 작업을 배정하고, 진행 기록을 남기고, 완료 후 추적 가능한 결과 폴더를 보관하는 프로젝트 로컬 작업영역이다.

## 목적

- 에이전트별 작업 배정 내용을 한곳에 모은다.
- 작업 중 의사결정, 조사 내용, 산출물을 잃지 않도록 기록한다.
- 작업 완료 후 별도 완료 폴더를 만들어 무엇을 했고 어떻게 완료했는지 추적한다.
- 사용자 승인 여부와 검증 결과를 작업 단위로 남긴다.

## 폴더 구조

```text
_workspace/
  active/
    <작업ID>/
      task.md
      work-log.md
      handoff.md
      artifacts/
  completed/
    <완료일>-<작업ID>/
      task.md
      work-log.md
      completion-report.md
      verification.md
      artifacts/
  templates/
    task.md
    work-log.md
    handoff.md
    completion-report.md
    verification.md
```

## 작업 ID 규칙

작업 ID는 다음 형식을 사용한다.

```text
YYYY-MM-DD-short-topic
```

예시:

```text
2026-06-29-agent-workspace
2026-06-29-unity-project-planning
2026-06-29-rat-host-loop-spec
```

## 기본 흐름

1. 작업을 시작할 때 `_workspace/active/<작업ID>/`를 만든다.
2. `templates/task.md`를 복사해 작업 배정 내용을 기록한다.
3. 진행 중 `work-log.md`에 조사, 판단, 변경 내용을 누적한다.
4. 다른 에이전트로 넘길 내용은 `handoff.md`에 정리한다.
5. 작업 완료 시 `_workspace/completed/<완료일>-<작업ID>/`를 만든다.
6. 완료 폴더에 작업 기록, 검증 기록, 완료 보고서를 남긴다.
7. 완료 폴더 경로를 최종 보고와 필요 시 커밋 메시지에 포함한다.

## 금지 사항

- `_workspace`에 빌드 산출물, 대용량 에셋, 임시 캐시를 보관하지 않는다.
- 승인되지 않은 구현 변경을 완료 폴더에 완료 처리하지 않는다.
- 검증하지 않은 내용을 완료로 기록하지 않는다.
- 사용자 승인 대기 항목을 누락한 채 완료 처리하지 않는다.

## 완료 조건

작업은 다음 문서가 완료 폴더에 있을 때 완료로 본다.

- `task.md`
- `work-log.md`
- `completion-report.md`
- `verification.md`

작업 중 산출물이 있으면 `artifacts/` 아래에 보관한다.
