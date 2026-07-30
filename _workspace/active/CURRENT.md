# 현재 세션 포인터

## 현재 이어받을 작업

- 작업 ID: `2026-07-30-production2d-occlusion-hud-correction`
- 상태: 커밋 준비 — 내부 승인 완료, 사용자 실제 WASD 재확인 대기
- 작업 경로: `_workspace/active/2026-07-30-production2d-occlusion-hud-correction/`
- 최신 사용자 요청: 검증을 통과한 고품질 2D 에셋·Unity 반입·가림/HUD 수정 체인을 선별 커밋하고 푸시한다.

## 먼저 읽을 파일

1. `_workspace/active/2026-07-30-production2d-occlusion-hud-correction/task.md`
2. `_workspace/active/2026-07-30-rat-host-2d-production-assets-unity-sample/verification.md`
3. `UnityProject/Assets/_Project/Editor/TechnicalSample2D/RatHost2DProductionSampleSceneBuilder.cs`

## 바로 이어서 할 작업

1. 현재 작업 체인만 선별 스테이징하고 Stage2·Stage3·보호 변경 제외를 감사한다.
2. 현황판과 작업 포인터를 포함해 커밋·푸시한다.
3. 푸시 뒤 사용자가 실제 WASD로 통·상자·벽 모서리 왕복과 짧은 방향 반전을 확인한다.

## 병행 차단 작업

- `2026-07-16-natural-alert-build-loop-verification`: Computer Use 게임 창 캡처 오류로 QA `차단`·총괄 `보류`.
- 재개 조건: Windows 게임 창 캡처 지원 복구 또는 사용자의 같은 연속 루프 단계별 화면과 해당 세션 `Player.log`.
- `2026-07-28-rat-host-2d-stage1-integration`: 과거 Unity Reload·원본 MCP 차단은 Stage2 원본 씬 복구와 QA로 해소됐다. Stage2 사용자 확인 뒤 함께 완료 상태를 정리한다.
- `2026-07-29-rat-host-2d-stage3-mutation-return`: 기술·운영 게이트와 총괄 검토를 통과했고, 사용자 실제 `1/2/3`·버튼·HUD·이동·전용 통로 수용을 기다린다.

## 제외하거나 건드리면 안 되는 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 로컬 변경을 보존한다.
- `_workspace/previews/`를 보존한다.
- 사용자에게 반려된 `_workspace/active/2026-07-29-rat-host-2d-game-spec-trial-assets/`와 Python `__pycache__`는 이번 커밋에서 제외한다.
- Stage2·Stage3 소유 코드·씬·문서는 이번 커밋에서 제외한다.
- 저장소 `Builds/`는 건드리지 않고 Windows 검증 빌드는 임시 경로에 출력한다.

## Git 상태

- 현재 HEAD = origin/main: `73c5750 docs: sync stage2 commit and handoff state`
- 이번 커밋 예정: 첫 아트 후보, 통합 기준, 아트 로드맵, 품질 마스터, 실제 에셋, Unity 기술 샘플 반입, 오브젝트 가림·HUD 수정과 관련 검증 기록.
- 제외 유지: Stage2 원본 씬 복구, Stage3 구현·검증, `APP_UI_EDITOR_ONLY`, 반려된 저품질 규격 시험 산출물, `_workspace/previews/`, `Builds/`.

## 갱신 정보

- 마지막 갱신: 2026-07-30 KST
- 갱신자: 메인 조정자
