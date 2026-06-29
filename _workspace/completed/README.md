# 완료된 작업

이 폴더는 완료된 에이전트 작업의 추적 기록을 보관한다.

## 완료 폴더 규칙

완료 폴더는 다음 형식을 사용한다.

```text
_workspace/completed/YYYY-MM-DD-<작업ID>/
```

예시:

```text
_workspace/completed/2026-06-29-2026-06-29-agent-workspace/
```

## 완료 폴더 필수 파일

- `task.md`: 최초 작업 배정 내용
- `work-log.md`: 진행 중 기록
- `completion-report.md`: 무엇을 어떻게 완료했는지 정리
- `verification.md`: 실행한 검증과 미검증 항목
- `artifacts/`: 작업 산출물 보관 폴더

## 완료 처리 절차

1. 완료 폴더를 만든다.
2. 진행 중 작업 폴더의 기록 파일을 완료 폴더로 복사하거나 이동한다.
3. `completion-report.md`를 작성한다.
4. `verification.md`를 작성한다.
5. 사용자 승인 필요 항목이 남아 있으면 완료가 아니라 `승인 대기`로 표시한다.
6. 최종 보고에 완료 폴더 경로를 포함한다.
