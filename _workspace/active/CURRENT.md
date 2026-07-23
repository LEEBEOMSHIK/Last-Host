# 현재 세션 포인터

## 현재 이어받을 작업

- 작업 ID: `2026-07-21-game-view-camera-output-fix`
- 상태: 보류 — 실제 W 이동·idle 안정화는 통과했으나 D·W+D 시각 결과와 이번 세션 Console 재검증이 남음.
- 작업 경로: `_workspace/active/2026-07-21-game-view-camera-output-fix/`

## 먼저 읽을 파일

1. `_workspace/active/2026-07-21-game-view-camera-output-fix/task.md`
2. `_workspace/active/2026-07-21-game-view-camera-output-fix/handoff.md`
3. `_workspace/active/2026-07-21-rat-pixel-treatment-v5/verification.md`

## 바로 이어서 할 작업

1. native MCP transport를 복구한 뒤 QA가 이번 세션 Console 상세과 D·W+D 이동 중 Game 뷰를 재검증한다.
2. 사용자가 Display 1에서 직선·대각 이동 중 쥐-카메라 정합을 확인한다.
3. QA·총괄 보류가 해소된 뒤에만 커밋을 판단한다.

## 제외하거나 건드리면 안 되는 변경

- `.codex/config.toml`, `_workspace/previews/`는 범위 밖이다.
- 자연 경계도 엄격 검증의 active·QA `차단`·총괄 `보류` 판정은 사용자 또는 검증 근거 없이 바꾸지 않는다.

## 갱신 정보

- 마지막 갱신: 2026-07-23 KST
- 갱신자: Codex 메인 에이전트
