# 현재 세션 포인터

## 현재 이어받을 작업

- 작업 ID: 신규 작업 후보 미선정
- 상태: 자연 부분 가림·쥐 본체 보존 사용자 수용 완료 및 보관
- 작업 경로: `_workspace/completed/2026-08-05-2026-08-02-production2d-natural-occlusion-root-fix/`
- 최신 사용자 요청: 다음 후보로 표시된 자연 부분 가림 화면과 쥐 본체 보존은 이미 수용한 내용이므로 완료 상태로 바로잡는다.

## 먼저 읽을 파일

1. `docs/project-handoff/current-task-board.md`
2. `_workspace/completed/2026-08-05-2026-08-02-production2d-natural-occlusion-root-fix/completion-report.md`
3. `_workspace/completed/2026-08-05-2026-08-02-production2d-natural-occlusion-root-fix/verification.md`

## 바로 이어서 할 작업

1. 완료된 대각선 충돌 표면 slide와 자연 부분 가림·쥐 본체 보존 QA는 반복하지 않는다.
2. 남아 있는 active 항목의 실제 미충족 게이트를 상태판과 대조한다.
3. 새 작업 후보는 사용자와 선택한 뒤 별도 작업 패킷으로 시작한다.

## 병행 차단 작업

- `2026-07-16-natural-alert-build-loop-verification`: Computer Use 게임 창 캡처 오류로 QA `차단`·총괄 `보류`.
- 재개 조건: Windows 게임 창 캡처 지원 복구 또는 사용자의 같은 연속 루프 단계별 화면과 해당 세션 `Player.log`.
- `2026-07-28-rat-host-2d-stage1-integration`: 과거 Unity Reload·원본 MCP 차단은 Stage2 원본 씬 복구와 QA로 해소됐다. Stage2 사용자 확인 뒤 함께 완료 상태를 정리한다.
- `2026-07-29-rat-host-2d-stage3-mutation-return`: 기술·운영 게이트와 총괄 검토를 통과했고, 사용자 실제 `1/2/3`·버튼·HUD·이동·전용 통로 수용을 기다린다.
- `2026-08-02-production2d-visual-overlap-correction`: 사용자 acceptance 실패로 `SUPERSEDED/수정 필요`; 별도 사용자 수용 대상이 아니다.
- `7ba12df` 기술 검증 PASS는 사용자 acceptance에서 FAIL. whole-character hide가 증상 은폐라 새 R2 작업으로 `SUPERSEDED/수정 필요`다.

## 제외하거나 건드리면 안 되는 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 로컬 변경을 보존한다.
- `UnityProject/ProjectSettings/Physics2DSettings.asset`의 기존 로컬 변경을 보존한다.
- `_workspace/previews/`를 보존한다.
- 사용자 소유 `docs/references/images/image.png`를 입력 reference로만 사용하고 수정·이동하지 않는다.
- 사용자에게 반려된 `_workspace/active/2026-07-29-rat-host-2d-game-spec-trial-assets/`는 이번 커밋에서 제외한다.
- 저장소 `Builds/`는 건드리지 않고 Windows 검증 빌드는 임시 경로에 출력한다.

## Git 상태

- 기능·완료 기록 원격 반영 기준: `4de3975 fix: complete surface slide and verification updates`.
- Stage2·Stage3 `8285bb0`, 자연 부분 가림 `4cb578b`, surface slide·검증 반복 축소·Unity MCP 경로 교정 `4de3975`는 `origin/main` 반영 완료다.
- 자연 부분 가림 기능 후보는 기존 QA·총괄 판정과 후속 사용자 수용을 근거로 완료 보관했다.
- 자연 부분 가림 실제 입력·화면 수용 대기는 2026-08-05 사용자 재확인으로 종료됐다.
- 대각선 충돌 표면 slide 작업은 사용자 수용·closeout QA·총괄을 통과해 완료 보관·원격 반영됐다.
- 사용자 소유 `docs/references/images/image.png`는 untracked 상태로 커밋에서 제외해 보존한다.

## 갱신 정보

- 마지막 갱신: 2026-08-05 KST
- 갱신자: 메인 조정자
