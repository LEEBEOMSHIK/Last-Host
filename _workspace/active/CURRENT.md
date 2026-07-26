# 현재 세션 포인터

## 활성 구현 작업

- 상태: 없음
- 최근 완료: `_workspace/completed/2026-07-27-2026-07-27-project-2d-direction-transition/`
- 레거시 보관: `_workspace/completed/2026-07-27-2026-07-24-rat-final-appearance-sample/`
- 현재 프로젝트 기준: 목업 기반 2D 아이소메트릭 도트 타일·방향별 스프라이트

## 다음 작업 후보

- 작업: 실제 2D 플레이어블 기술 샘플 계획·승인
- 범위 후보: 작은 하수도 방 1개, Tilemap, 2D Collider, SpriteRenderer/Animator, Y축 깊이 정렬, 고정 직교 픽셀 카메라, 실제 쥐 스프라이트와 플레이 해상도 규격
- 승인 경계: 별도 규격·구현 계획과 사용자 승인 전에는 Unity 씬·코드·Import·ProjectSettings·패키지를 변경하지 않는다.
- 목업 경계: `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`는 reference이며 실제 타일셋·스프라이트 시트·플레이어블 완료 증거가 아니다.

## 병행 차단 작업

- `2026-07-16-natural-alert-build-loop-verification`: Computer Use 게임 창 캡처 오류로 QA `차단`·총괄 `보류`.
- 재개 조건: Windows 게임 창 캡처 지원 복구 또는 사용자의 같은 연속 루프 단계별 화면과 해당 세션 `Player.log`.

## 제외하거나 건드리면 안 되는 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 로컬 변경을 보존한다.
- `_workspace/previews/`를 보존한다.
- `Builds/`를 현재 작업 범위에서 제외한다.

## Git 상태

- 2D 방향 전환 반영: `e654429 docs: adopt 2d isometric project direction`
- 푸시 상태: `e654429`까지 `origin/main` 반영 완료
- 후속 상태 동기화: 이 `CURRENT.md`와 현황판의 푸시 기록만 별도 docs-only 커밋으로 반영한다.

## 갱신 정보

- 마지막 갱신: 2026-07-27 KST
- 갱신자: 메인 조정자
