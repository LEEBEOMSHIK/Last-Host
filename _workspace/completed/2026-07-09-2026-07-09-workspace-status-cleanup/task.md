# 작업 패킷: 작업 상태 정리

## 목표

MCP 복구 후 검증이 끝난 작업들의 `_workspace` 상태를 정리하고, 완료 조건을 갖춘 active 작업을 completed로 이동한다.

## 범위

- 완료 보고와 검증 기록이 갖춰진 작업 폴더 이동
- MCP 복구로 조건부 보류가 해소된 작업의 완료 보고 보강
- `_workspace/active/CURRENT.md` 갱신
- 보류 중인 active 작업은 이유를 남기고 유지

## 비범위

- Unity 코드, 씬, ProjectSettings 수정
- 기존 `ProjectSettings.asset` 변경 되돌리기
- 사용자 결정 필요 상태인 작업을 임의 완료 처리
- 새 기능 구현

## 완료 기준

- 완료 처리한 작업은 `_workspace/completed/2026-07-09-<작업ID>/` 아래에 있다.
- active에는 실제 보류 작업과 `CURRENT.md`, `README.md`만 남는다.
- `git diff --check`가 통과한다.
