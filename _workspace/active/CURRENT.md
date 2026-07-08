# 현재 세션 포인터

## 현재 이어받을 작업

- 작업 ID: `2026-07-08-rat-instinct-wander-fix`
- 상태: 수정 완료, 커밋/푸시 진행 중
- 최신 사용자 요청: 쥐 숙주 본능 배회/비활성 컨트롤러 수정 변경을 커밋/푸시한다.

## 먼저 읽을 파일

1. `_workspace/active/2026-07-08-rat-instinct-wander-fix/task.md`
2. `UnityProject/Assets/_Project/Scripts/Host/RatHostController.cs`
3. `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`

## 바로 이어서 할 작업

1. 커밋/푸시 결과를 확인한다.
2. 사용자가 플레이로 쥐 배회와 조종감을 확인한다.
3. 필요하면 `instinctTurnIntervalRange`, `instinctTurnAngleDegrees`, 조종력 수치를 조정한다.

## 제외하거나 건드리면 안 되는 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 기존 `APP_UI_EDITOR_ONLY` define 변경은 이번 작업 범위가 아니다.
- 기존 활성 작업 폴더를 임의로 완료 처리하지 않는다.

## 마지막 성공 검증

- 런타임/테스트 어셈블리 수동 컴파일 통과
- `git diff --check` 통과
- Unity MCP Play 2초 후 Console Error/Warning 0건

## 차단 항목

- 실제 조작 체감은 사용자 확인 필요
- Unity MCP가 커밋 직전 다시 `Connection revoked` 상태가 되어 추가 Play 재확인은 불가
- `ProjectSettings.asset` 기존 define 변경은 제외

## 갱신 정보

- 마지막 갱신: 2026-07-08
- 갱신자: 프로젝트 조정 에이전트 / QA 검증 에이전트 / 문서 릴리즈 에이전트
