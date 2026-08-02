# 원본 Stage2 씬 독립 QA

## 검증 대상

- 원본 `Assets/_Project/Scenes/RatHost2DPrototype.unity`
- 빈 Tilemap 때문에 맵 범위가 보이지 않던 증상의 복구
- Stage2 백혈구 회피 미니게임의 원본 Unity Play 연결

검증일은 2026-07-29이며 Windows 빌드는 실행하지 않았다.

## 판정

`통과 — 원본 씬 표시·Stage2 런타임 기술 게이트 통과, 사용자 키보드 체감 확인 대기`

## 씬 저장과 Tilemap

Unity MCP 원본 프로젝트에서 다음을 확인했다.

```text
scene=RatHost2DPrototype
path=Assets/_Project/Scenes/RatHost2DPrototype.unity
loaded=true
dirty=false
rootCount=1

FloorTilemap    117 cells  occupied=(-6,-4)..(6,4)
WaterTilemap      5 cells  occupied=(3,-2)..(3,2)
BlockingTilemap  40 cells  occupied=(-6,-4)..(6,4)
```

Floor와 Blocking의 `cellBounds`는 모두
`Position (-6,-4,0), Size (13,9,1)`이었다.
Water와 Blocking에는 활성 비트리거 `TilemapCollider2D`가 있고,
편집 모드에서 물과 외곽 벽의 실제 collider bounds가 0이 아님을 확인했다.

Play 중 월드 중심에서 네 방향 `Physics2D.RaycastAll`을 실행했다.

```text
Host E/W/S -> BlockingTilemap
Host N     -> ContaminationZone2D, WaterTilemap, BlockingTilemap
```

따라서 단순 렌더 타일뿐 아니라 물과 외곽 경계의 2D 물리 질의도
원본 씬에서 성립한다.

## 화면·카메라

Host 카메라 캡처는 1920×1080으로 수행했다.

- `13×9` 바닥과 외곽 경계가 화면 안에서 한 방으로 읽힘
- 수로 5셀, 오염 구역, 쥐, Pipe와 Barrel 소품 식별 가능
- 검은 단색 화면이 아니라 플레이 공간의 범위가 명확히 표시됨
- 현재 표현은 기술 플레이스홀더이며 목표 목업이나 최종 아트로 판정하지 않음

카메라 계약:

```text
Main Camera     target=RatHost2D  orthographic=true  centerError=(0,0)
Internal Camera target=Virus2D    orthographic=true  centerError=(0,0)
```

내부 모드 진입 뒤 Internal 카메라 캡처에서 직사각형 아레나,
바이러스, 백혈구, 녹색 조각 3개를 확인했다. 어두운 팔레트지만
게임 오브젝트와 경계가 식별되어 black-only 상태는 아니다.

## Stage2 계층과 충돌 계약

```text
Arena walls=4
Virus=1
WhiteBloodCell=1
MutationFragments=3 (indices 0,1,2)
InternalHud=1
FailurePanel=1
MutationSelectionShell=1
missing scripts=0
```

네 아레나 벽은 모두 비트리거 `BoxCollider2D`다.
Play 중 중심에서 네 방향 물리 질의를 실행해 각각
`Wall_East`, `Wall_West`, `Wall_North`, `Wall_South`가 검출됐다.
Virus collider는 비트리거, WBC와 조각 collider는 트리거다.

## MCP Play 대체 입력 검증

Unity MCP에는 OS 키보드의 실제 WASD/Space 키다운을 주입하는 기능이
없어, 저장 상태를 바꾸지 않는 공개 런타임 API를 사용했다.

Host 이동 입력 대체:

- `CachePlayerInput(Vector2.right)` 뒤 `SimulateFixedStep(0.1)`
- 입력 캐시 `(1,0)`, 해석 이동 방향 `(1,0)` 확인
- Main Camera target은 RatHost이며 중심 오차 `(0,0)` 유지

모드 전환·실패·복귀·재진입·성공:

```text
RatHost
  Host root/camera/input ON
  Internal root/camera/input OFF

ApplyContaminationExposure
  -> InternalVirus, entryCount=1
  Host OFF / Internal ON / Internal Camera ON

WBC contact 3회
  -> VirusFailed, stability=0/100
  FailurePanel ON, Internal Camera 유지

ProcessFailureConfirmationInput(true)
  -> RatHost
  Host root/HUD/camera 복귀, FailurePanel OFF

재오염
  -> InternalVirus, entryCount=2

Fragment 0/1/2 TryCollect + FlushQueuedVirusFrame
  -> Running / Running / Success
  -> MutationSelection
  MutationSelectionShell ON
  Host/Internal root와 두 카메라 OFF
```

WBC는 `TryApplyContact`를 거쳐 접촉 큐를 사용했고,
조각은 각 실제 `RatHost2DMutationFragment.TryCollect`와 Session flush를
거쳤다. 이는 상태를 직접 대입한 검사가 아니라 Stage2 런타임 공개 경로다.
Play 종료 뒤 씬은 다시 `dirty=false`였다.

## 테스트 범위 판단

- 기존 신규 Stage2 EditMode `10/10`
- 기존 전체 EditMode `186/186`
- 이번 변경은 런타임 게임플레이 코드가 아니라 Editor 씬 빌더의
  `NewScene`/asset 로드 순서와 Tilemap 저장 후조건, 생성된 씬이다.
- 독립 QA는 원본 Rebuild 결과의 실제 셀·collider·카메라·Play 계약을
  Unity API로 직접 확인했다.
- 같은 전체 EditMode와 Windows 빌드는 재실행하지 않았다.
  현재 위험에 비해 중복 비용이 크고, 사용자가 불필요한 빌드를 원하지
  않는다고 명시했기 때문이다.

## Console과 보호 경계

카메라 캡처 직후 캡처 도구가
`Releasing render texture that is set to be RenderTexture.active!`
경고 1건을 남겼다. 제품 플레이 경고가 아니므로 캡처 종료 뒤 콘솔을
비우고, 캡처 없이 Play·상태 전환·Stop을 다시 실행했다.

최종 Unity Console:

```text
Error=0
Warning=0
```

보호 diff:

- `ProjectSettings.asset`에는 기존 사용자 한 줄
  `SENTIS_ANALYTICS_ENABLED;APP_UI_EDITOR_ONLY`만 유지
- `_workspace/previews/`는 untracked 상태 그대로 보존
- Packages, 입력 asset, 기존 3D 씬, 2D TechnicalSample 씬 tracked diff `0`
- Windows 빌드 미생성

## 남은 위험

- 실제 물리 키보드 WASD/Space의 손 감각과 UI 가독성은 사용자가
  Game View에서 최종 확인해야 한다.
- 카메라 바깥은 어두운 단색이고 내부 아레나도 의도적으로 어두운
  플레이스홀더 팔레트다. 최종 조명·아트 수용을 의미하지 않는다.
- 성공 뒤 실제 변이 선택·효과·쥐 복귀는 승인된 Stage3 범위다.
- 반복 Rebuild의 Unity local fileID/YAML byte 비결정성은 기존 위험으로
  남지만, 셀 수와 런타임 논리 계약에는 영향을 주지 않았다.
