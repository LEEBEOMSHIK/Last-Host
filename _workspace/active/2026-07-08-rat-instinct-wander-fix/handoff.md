# 핸드오프 기록

## 작업 ID

`2026-07-08-rat-instinct-wander-fix`

## 최신 사용자 요청

쥐가 랜덤으로 움직이는지, 위아래 왕복과 좌우 입력 대각 이동이 의도인지 확인하고 버그면 수정한다.

## 현재 상태

- 상태: 수정/검증 완료, 사용자 플레이 체감 확인 대기
- 여기서 멈춤: 본능 배회 모델과 컨트롤러 연결, 테스트, MCP Play 콘솔 확인 완료
- 다음 세션의 첫 목표: 사용자 플레이 결과를 듣고 수치 조정 또는 커밋을 진행한다.

## 먼저 읽을 파일

1. `UnityProject/Assets/_Project/Scripts/Host/RatHostControlModel.cs`
2. `UnityProject/Assets/_Project/Scripts/Host/RatHostController.cs`
3. `_workspace/active/2026-07-08-rat-instinct-wander-fix/verification.md`

## 변경한 파일

- `UnityProject/Assets/_Project/Scripts/Host/RatHostControlModel.cs`
- `UnityProject/Assets/_Project/Scripts/Host/RatHostController.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
- `_workspace/active/CURRENT.md`
- `_workspace/active/2026-07-08-rat-instinct-wander-fix/`

## 건드리면 안 되는 기존 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 기존 `APP_UI_EDITOR_ONLY` define 변경

## 마지막 성공 검증

- 런타임/테스트 어셈블리 수동 컴파일 통과
- `git diff --check` 통과
- Unity MCP Play 2초 후 Console Error/Warning 0건

## 실패 또는 차단된 검증

- 실제 조작 체감은 사용자 확인 필요
- 커밋 직전 Unity MCP가 다시 `Connection revoked` 상태가 되어 추가 Play 재확인은 불가

## 이어서 해야 할 일

1. 사용자가 플레이로 배회/조종감을 확인한다.
2. 필요하면 `instinctTurnIntervalRange`, `instinctTurnAngleDegrees`, 조종력 수치를 조정한다.
3. 확정되면 직전 비활성 Move 수정과 함께 커밋/푸시한다.

## 사용자 승인 필요

- 조작감 수치 조정이 필요하면 사용자 체감 피드백이 필요하다.
