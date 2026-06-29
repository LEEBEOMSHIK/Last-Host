# 진행 중 작업

이 폴더는 현재 진행 중인 에이전트 작업을 보관한다.

## 새 작업 생성 절차

1. `_workspace/active/<작업ID>/` 폴더를 만든다.
2. `_workspace/templates/task.md`를 복사해 `task.md`로 둔다.
3. `_workspace/templates/work-log.md`를 복사해 `work-log.md`로 둔다.
4. `_workspace/templates/handoff.md`를 복사해 `handoff.md`로 둔다.
5. 필요한 산출물을 보관할 `artifacts/` 폴더를 만든다.

## 진행 중 기록 원칙

- 중요한 판단은 `work-log.md`에 남긴다.
- 다른 에이전트가 이어받아야 하는 내용은 `handoff.md`에 남긴다.
- 승인 필요 항목은 작업 중에도 계속 갱신한다.
- 완료된 작업은 이 폴더에 방치하지 않고 `completed/` 아래로 별도 완료 폴더를 만든다.
