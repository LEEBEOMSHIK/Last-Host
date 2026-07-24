# 현재 세션 포인터

## 현재 이어받을 작업

- 작업 ID: `2026-07-16-natural-alert-build-loop-verification`
- 상태: 차단 — Computer Use 연결과 새 빌드 실행은 복구됐지만 게임 창 캡처가 `SetIsBorderRequired 0x80004002`로 최초·복구 1회 모두 실패했다. 화면·포커스 증거 없이 입력하지 않았으며 자연 성공 루프는 미검증.
- 작업 경로: `_workspace/active/2026-07-16-natural-alert-build-loop-verification/`

## 먼저 읽을 파일

1. `_workspace/active/2026-07-16-natural-alert-build-loop-verification/handoff.md`
2. `_workspace/active/2026-07-16-natural-alert-build-loop-verification/verification.md`
3. `_workspace/active/2026-07-16-natural-alert-build-loop-verification/task.md`

## 바로 이어서 할 작업

1. 쥐 걷기·스프라이트·픽셀·카메라 관련 EditMode 회귀 테스트 일괄 실행 및 기술 게이트 종결을 우선한다.
2. Windows 게임 창 캡처가 지원되는 환경으로 바뀌거나 사용자가 같은 연속 루프의 단계별 화면·해당 세션 `Player.log`를 제공하면 현재 엄격 검증을 재개한다.

## 제외하거나 건드리면 안 되는 변경

- `UnityProject/`, `Builds/`, 패키지·에셋은 이 검증에서 수정하거나 재생성하지 않는다.
- `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 변경과 `_workspace/previews/`는 이전 작업의 범위 밖 변경으로 유지한다.
- `F6`, MCP/Inspector/메모리 상태 주입, 서로 다른 실행 세션의 증거 조합은 엄격 성공 근거로 사용하지 않는다.

## 갱신 정보

- 마지막 갱신: 2026-07-24 KST
- 갱신자: 문서/릴리즈 에이전트
