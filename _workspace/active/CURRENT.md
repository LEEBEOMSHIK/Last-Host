# 현재 세션 포인터

## 현재 이어받을 작업

- 작업 ID: `2026-08-02-production2d-visual-overlap-correction`
- 상태: 구현 커밋 `7ba12df` 완료 — 사용자 실제 WASD·작은 소품 완전 가림 확인 대기, 운영 커밋 준비 중
- 작업 경로: `_workspace/active/2026-08-02-production2d-visual-overlap-correction/`
- 최신 사용자 요청: 첫 기능 커밋 `7ba12df` 반영 상태를 동기화하고 완료 보관한 루프 감사를 별도 운영 커밋으로 준비한다.

## 먼저 읽을 파일

1. `_workspace/active/2026-08-02-production2d-visual-overlap-correction/handoff.md`
2. `_workspace/active/2026-08-02-production2d-visual-overlap-correction/verification.md`
3. `docs/project-handoff/current-task-board.md`

## 바로 이어서 할 작업

1. 완료 보관한 루프 감사·비용 현황판·운영 도구의 운영 커밋 범위를 대조한다.
2. 구현 커밋 `7ba12df`와 운영 커밋 대상을 섞지 않는다.
3. 사용자 실제 WASD와 작은 소품 뒤 완전 가림 수용은 별도 확인 항목으로 유지한다.

## 병행 차단 작업

- `2026-07-16-natural-alert-build-loop-verification`: Computer Use 게임 창 캡처 오류로 QA `차단`·총괄 `보류`.
- 재개 조건: Windows 게임 창 캡처 지원 복구 또는 사용자의 같은 연속 루프 단계별 화면과 해당 세션 `Player.log`.
- `2026-07-28-rat-host-2d-stage1-integration`: 과거 Unity Reload·원본 MCP 차단은 Stage2 원본 씬 복구와 QA로 해소됐다. Stage2 사용자 확인 뒤 함께 완료 상태를 정리한다.
- `2026-07-29-rat-host-2d-stage3-mutation-return`: 기술·운영 게이트와 총괄 검토를 통과했고, 사용자 실제 `1/2/3`·버튼·HUD·이동·전용 통로 수용을 기다린다.
- `2026-08-02-production2d-visual-overlap-correction`: 내부 승인 가능, 사용자 실제 WASD와 작은 소품 뒤 완전 가림 수용 대기.

## 제외하거나 건드리면 안 되는 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 로컬 변경을 보존한다.
- `UnityProject/ProjectSettings/Physics2DSettings.asset`의 기존 로컬 변경을 보존한다.
- `_workspace/previews/`를 보존한다.
- 사용자 소유 `docs/references/images/image.png`를 입력 reference로만 사용하고 수정·이동하지 않는다.
- 사용자에게 반려된 `_workspace/active/2026-07-29-rat-host-2d-game-spec-trial-assets/`는 이번 커밋에서 제외한다.
- Stage2·Stage3 소유 코드·씬·문서는 이번 커밋에서 제외한다.
- 저장소 `Builds/`는 건드리지 않고 Windows 검증 빌드는 임시 경로에 출력한다.

## Git 상태

- `main` 푸시 완료: 구현 `e7220a7 feat: integrate production 2d art sample`, 1차 상태 동기화 `7adef75 docs: sync production 2d push state`
- `e7220a7` 포함: 첫 아트 후보, 통합 기준, 아트 로드맵, 품질 마스터, 실제 에셋, Unity 기술 샘플 반입, 오브젝트 가림·HUD 수정과 관련 검증 기록.
- 현재 HEAD: `7ba12df fix: correct production 2d visual occlusion` — overlap 구현 커밋 완료.
- 현재: 루프 감사는 `_workspace/completed/2026-08-02-2026-08-02-loop-harness-efficiency-audit/`로 완료 보관했고 별도 운영 커밋 준비 중이다.
- 제외 유지: Stage2 원본 씬 복구, Stage3 구현·검증, `APP_UI_EDITOR_ONLY`, 반려된 저품질 규격 시험 산출물, `_workspace/previews/`, `Builds/`.

## 갱신 정보

- 마지막 갱신: 2026-08-02 KST
- 갱신자: 메인 조정자
