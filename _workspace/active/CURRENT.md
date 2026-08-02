# 현재 세션 포인터

## 현재 이어받을 작업

- 작업 ID: `2026-08-02-production2d-natural-occlusion-root-fix`
- 상태: 기술 검증 통과·내부 승인 가능 — 사용자 실제 WASD·최종 화면 수용 대기
- 작업 경로: `_workspace/active/2026-08-02-production2d-natural-occlusion-root-fix/`
- 최신 사용자 요청: 검증 하네스·상태 계약 변경 커밋·푸시와 불필요한 원시 로그 정리를 완료했다.

## 먼저 읽을 파일

1. `_workspace/active/2026-08-02-production2d-natural-occlusion-root-fix/task.md`
2. `_workspace/active/2026-08-02-production2d-natural-occlusion-root-fix/artifacts/canonical-evidence-r1.json`
3. `docs/project-handoff/current-task-board.md`

## 바로 이어서 할 작업

1. 운영 커밋 `a33164b`와 후속 상태 동기화 커밋을 기준으로 사용한다.
2. 자연 부분 가림 production·씬·테스트와 Stage2·Stage3 기존 dirty는 보존한다.
3. 후속으로 사용자의 실제 연속 WASD·최종 화면 수용을 확인한다.

## 병행 차단 작업

- `2026-07-16-natural-alert-build-loop-verification`: Computer Use 게임 창 캡처 오류로 QA `차단`·총괄 `보류`.
- 재개 조건: Windows 게임 창 캡처 지원 복구 또는 사용자의 같은 연속 루프 단계별 화면과 해당 세션 `Player.log`.
- `2026-07-28-rat-host-2d-stage1-integration`: 과거 Unity Reload·원본 MCP 차단은 Stage2 원본 씬 복구와 QA로 해소됐다. Stage2 사용자 확인 뒤 함께 완료 상태를 정리한다.
- `2026-07-29-rat-host-2d-stage3-mutation-return`: 기술·운영 게이트와 총괄 검토를 통과했고, 사용자 실제 `1/2/3`·버튼·HUD·이동·전용 통로 수용을 기다린다.
- `2026-08-02-production2d-visual-overlap-correction`: 내부 승인 가능, 사용자 실제 WASD와 작은 소품 뒤 완전 가림 수용 대기.
- `2026-08-02-production2d-natural-occlusion-root-fix`: 기술 검증 통과·내부 승인 가능, 사용자 실제 WASD와 최종 화면 수용 대기.
- `7ba12df` 기술 검증 PASS는 사용자 acceptance에서 FAIL. whole-character hide가 증상 은폐라 새 R2 작업으로 `SUPERSEDED/수정 필요`다.

## 제외하거나 건드리면 안 되는 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 로컬 변경을 보존한다.
- `UnityProject/ProjectSettings/Physics2DSettings.asset`의 기존 로컬 변경을 보존한다.
- `_workspace/previews/`를 보존한다.
- 사용자 소유 `docs/references/images/image.png`를 입력 reference로만 사용하고 수정·이동하지 않는다.
- 사용자에게 반려된 `_workspace/active/2026-07-29-rat-host-2d-game-spec-trial-assets/`는 이번 커밋에서 제외한다.
- Stage2·Stage3 소유 코드·씬·문서는 이번 커밋에서 제외한다.
- 저장소 `Builds/`는 건드리지 않고 Windows 검증 빌드는 임시 경로에 출력한다.

## Git 상태

- 원격 반영 기준: `a33164b chore: harden verification execution guards`.
- 검증 하네스·상태 계약 운영 변경과 완료 패킷은 `origin/main` 반영 완료다.
- Stage2·Stage3·ProjectSettings·preview·reference 등 현재 작업과 무관한 기존 dirty도 함께 존재한다.
- 자연 부분 가림 구현·씬·테스트는 사용자 실제 수용 대기라 운영 커밋에서 제외했고 로컬에 보존돼 있다.

## 갱신 정보

- 마지막 갱신: 2026-08-03 KST
- 갱신자: 메인 조정자
