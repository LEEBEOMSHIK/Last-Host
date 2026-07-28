# 현재 세션 포인터

## 현재 이어받을 작업

- 작업 ID: `2026-07-28-rat-host-2d-stage2-minigame`
- 상태: 구현 커밋 완료·푸시 준비 — `d12146f`, 원본 Reload·MCP Play·Console 사용자 결정 대기
- 작업 경로: `_workspace/active/2026-07-28-rat-host-2d-stage2-minigame/`
- 최신 사용자 요청: 2단계 2D 백혈구 회피 미니게임과 성공·실패 인계 작업을 진행한다.

## 먼저 읽을 파일

1. `_workspace/active/2026-07-28-rat-host-2d-stage2-minigame/task.md`
2. `_workspace/active/2026-07-28-rat-host-2d-stage1-integration/handoff.md`
3. `docs/prototype/approvals/rat-host-2d-core-loop-migration-brief.md`

## 바로 이어서 할 작업

1. 현황판과 CURRENT를 실제 구현 커밋 `d12146f`에 맞춰 동기화하고 푸시한다.
2. 사용자가 원본 Unity 외부 씬 변경 모달의 `Reload` 실행 여부를 결정한다.
3. 승인 후 원본 씬을 Stage2로 Rebuild·Save하고 MCP Play·Console·보호 diff를 검증한다.

## 병행 차단 작업

- `2026-07-16-natural-alert-build-loop-verification`: Computer Use 게임 창 캡처 오류로 QA `차단`·총괄 `보류`.
- 재개 조건: Windows 게임 창 캡처 지원 복구 또는 사용자의 같은 연속 루프 단계별 화면과 해당 세션 `Player.log`.
- `2026-07-28-rat-host-2d-stage1-integration`: 구현과 전체 EditMode는 통과했으나 Unity 외부 씬 변경 모달 때문에 원본 MCP Play·최신 QA 빌드가 차단됐다.

## 제외하거나 건드리면 안 되는 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 로컬 변경을 보존한다.
- `_workspace/previews/`를 보존한다.
- 저장소 `Builds/`는 건드리지 않고 Windows 검증 빌드는 임시 경로에 출력한다.

## Git 상태

- 현재 HEAD: `d12146f feat: add staged 2d rat host core loop`
- 원격 기준: `origin/main = 0dea64e docs: sync 2d sample commit state`
- 구현·테스트·승인/QA 기록은 커밋됐고 현황판 동기화 커밋과 푸시가 남았다.

## 갱신 정보

- 마지막 갱신: 2026-07-28 KST
- 갱신자: 메인 조정자
