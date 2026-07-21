# Unity 씬/통합·QA 검증

## 검증 대상

`WalkTrialV2` PNG 64장의 Unity Importer·씬 연결과 `RatHostPrototype` 실제 Play에서의 8방향 걷기 표시.

## 실행한 검증

- 원본 `renders-v2`와 Unity 반입본의 이름·64×64 규격·SHA-256 64/64 일치(기존 기록)를 유지 확인.
- Unity MCP `Unity_GetUserGuidelines`, `Unity_ManageEditor(GetState)` 성공 응답 확인.
- Unity Editor에서 PNG 64/64를 Sprite(Single), PPU 32, Pivot `(0.5, 0.25)`, Point, 무압축, mipmap 없음으로 재임포트하고 각 Sprite가 로드되는지 검증.
- `RatVisual/RatDirectionalSpriteView`의 8개 배열이 방향별 `f01→f08`을 모두 참조하고 `HasCompleteWalkCycles=true`, `walkFramesPerSecond=8`인지 확인.
- Unity Play 진입·종료, Play 모드에서 8방향·frame index 3·정지 복귀·접지 clearance를 직접 검증.
- Play 시작 직후 Unity Console Error/Warning 0건 확인. 이후 MCP Camera Capture의 인스턴스 ID 오류 1건은 도구 자체 오류이며, MainCamera RenderTexture 캡처는 성공.
- 실제 MainCamera 렌더에서 동쪽 방향 8프레임을 순서대로 캡처해 스프라이트가 카메라에 보이고 프레임별 다리/꼬리 실루엣이 변하는지 확인.
- 화면 점유율 확대안으로 `RatVisual` 1.35배와 비례 접지 오프셋을 한 차례 Play 검증했으나, 사용자가 실제 보행 동작 폭 확대를 요구해 해당 Unity 확대안은 되돌렸다.
- Blender v3 64 PNG를 별도 `WalkTrialV3` 경로로 반입하고 원본/반입본 SHA-256 64/64 일치 확인.
- `WalkTrialV3` 64/64의 Sprite(Single), PPU 32, Pivot `(0.5, 0.25)`, Point, 무압축, mipmap 없음과 Sprite 로드를 재검증.
- Play에서 v3 배열의 8방향·8fps·frame index 3·접지 clearance·MainCamera 렌더·콘솔을 재검증.

## 결과

- `WalkTrialV2`는 기존 `TrialV1`과 분리된 시험 경로를 유지한다.
- 64/64 파일이 요구 Importer 설정과 Sprite 로드를 통과했다.
- 씬의 8개 배열은 각각 정확히 8장씩 해당 방향의 `f01→f08`을 참조하며, 8fps로 저장됐다.
- Play 모드에서 South, SW, W, NW, N, NE, E, SE 모두 해당 방향 배열의 frame index 3을 표시했다. 정지 시 마지막 SouthEast 방향의 TrialV1 정지 Sprite로 정상 복귀했다.
- 접지 surface는 검출됐고 visible foot clearance는 설정값과 같은 `0.005`였다. 기존 이동·충돌·루트 Transform 변경은 하지 않았다.
- MainCamera Play 렌더에서 쥐가 맵 안에 보이며, 8프레임 시퀀스의 다리·꼬리 실루엣 변화가 확인됐다.
- 확대안에서도 8방향 배열·frame index·8fps 재생과 ground clearance `0.005`가 유지됐으나, 현재 씬은 원래 크기와 접지 오프셋으로 되돌린 상태다. v3 Blender 프레임으로 실제 동작 폭을 키운 뒤 다시 판정한다.
- `RatVisual`의 8개 걷기 배열은 현재 `WalkTrialV3/rat-walk-v3-f01…f08`을 정확히 참조하며, Play에서 South, SW, W, NW, N, NE, E, SE 모두 해당 v3 frame index 3을 표시했다.
- v3는 화면 표시 크기를 바꾸지 않고 스윙 발의 들림·전진, 대각 접지쌍 교대, 절제된 몸통·꼬리 리듬을 키웠다. Play 접지는 기존과 같은 clearance `0.005`이며 Console Error/Warning은 0건이다.

## MCP 플레이 체크

- 통과: 대상 씬 Play 진입·종료, RatHost/`RatVisual`/HUD/IsometricCamera 활성 상태, 8방향 걷기 프레임 선택, 정지 복귀, 접지, 콘솔을 확인했다.
- 제한: MCP의 기본 `Unity_Camera_Capture`는 Play 런타임 인스턴스 ID를 해석하지 못했다. 같은 MainCamera를 RenderTexture로 직접 렌더한 캡처는 성공했으므로 화면 검증은 수행했다.

## 검증하지 못한 항목

- `RatDirectionalSpriteView_WalkCyclesAtEightFpsAndReturnsToLastIdleDirection` EditMode 테스트의 TestRunner 실행 결과. 동적 TestRunner 래퍼와 별도 Unity 배치 실행이 결과를 남기지 못했다.
- 실제 키보드 연속 WASD 입력에 따른 장시간 보행의 체감·경계 전환. Play 중 API로 8방향 상태와 프레임을 재현한 검증이며 사용자 수동 조작을 대체하지 않는다.
- 독립 QA/검증 에이전트 재검증 및 프로젝트 총괄 관리자 판정.

## 남은 위험

- v3의 실제 키보드 연속 이동에서 보행 가독성이 충분한지와 대각 전환 체감은 사용자 확인이 필요하다. 독립 QA·총괄 판정 전에는 완료·커밋 완료로 주장하지 않는다.
- 대상 EditMode 테스트의 단독 실행 결과가 없으므로 테스트 통과로는 주장하지 않는다.

## 완료 판단

**검증 진행 중 / QA 미승인.** Unity 통합과 핵심 MCP Play 검증은 통과했다. EditMode 실행 결과, 독립 QA 기록, 총괄 관리자 판정 전에는 작업 완료·보관·커밋 완료로 보고하지 않는다.

## 2026-07-21 — 방향 축 보정 재현·검증

### 검증 대상

`RatDirectionalSpriteView`가 Unity 카메라 기준 실제 이동과 Blender v3 프리렌더의 수평 화면 방향을 일치시키는지.

### 실행한 검증

- 수정 전 실제 이동·루트 forward는 동쪽인데 `CurrentDirection=East`, 표시 Sprite=`rat-06-e`가 화면 좌향으로 보이는 것을 Unity Play에서 기록했다.
- `RefreshDirection`의 시각용 right 축만 `-cameraTransform.right`로 바꾸고, Unity 스크립트 검증과 Unity MCP 컴파일/실행을 수행했다. 코드 진단은 기존 `Update()` 문자열 연결 성능 힌트 1건뿐이며 오류는 없었다.
- 컴파일 후 카메라 오른쪽 이동 벡터 `(1,0,0)`에서 `CurrentDirection=West`, Sprite=`rat-walk-v3-f01-02-w`가 선택되고, `RatHostController.CurrentResolvedMoveDirection`이 보존되는 것을 로그로 확인했다. 콘솔 Error/Warning은 0건, `git diff --check`는 통과했다.

### 결과

- 좌우 반전은 프리렌더 축과 Unity 카메라 축의 통합 불일치였으며, 시각 자식에서만 보정됐다. 이동·충돌·입력·카메라·ProjectSettings는 변경하지 않았다.
- 화질의 프레임별 체감 변화는 코드가 아닌 64px 원본의 방향별 실루엣/화면 픽셀 정렬 쪽으로 분리됐다. 모든 v3 PNG는 64×64와 동일 Importer 기준을 유지하므로, 원본 재렌더 또는 카메라/픽셀 정렬 변경은 별도 승인·통합 작업이 필요하다.
- 활성 씬의 다른 미저장 변경을 보호하기 위해 Save·Reload·추가 Play는 실행하지 않았다. 따라서 수정 뒤의 실제 키보드 연속 WASD·대각선 체감은 깨끗한 씬 재로드 후 QA/사용자 확인이 필요하다.

## 2026-07-21 — 정지·이동 v3 Sprite 세트 검증

### 검증 대상

8방향 v3 걷기 배열이 완전할 때 정지 표시가 같은 방향의 v3 `f01`을 유지하고, 배열이 불완전할 때만 TrialV1 정지 Sprite로 fallback하는지.

### 실행한 검증

- `ApplyCurrentSprite`를 검토해, 이전에는 항상 `GetSprite(CurrentDirection)`(TrialV1)을 선택해 정지↔이동 세트 전환을 일으킨 경로를 확인했다.
- 완전한 8방향 테스트 배열에서 이동을 멈춘 뒤 `northFrames[0]`을, `northWalkFrames=null`로 불완전하게 만든 뒤 `staticNorth`를 기대하도록 EditMode 계약을 갱신했다.
- Unity `ValidateScript`로 런타임 스크립트와 EditMode 테스트 파일을 확인했다. 오류는 없고 런타임 코드에 기존 Update 문자열 연결 성능 힌트 1건만 남았다. `git diff --check`도 통과했다.

### 검증하지 못한 항목

- 활성 씬이 dirty여서 Save·Reload·추가 Play 또는 TestRunner 실행은 하지 않았다. 다른 미저장 씬 변경을 덮어쓰지 않기 위한 제한이다.
- 수정 뒤 실제 연속 WASD·대각선 이동에서 정지↔이동 전환 체감은 깨끗한 씬 재로드 후 QA/사용자 확인이 필요하다.

### 결과

- 완전한 v3 구성에서는 정지·이동이 같은 v3 세트 안에서 전환되며, 불완전 구성의 기존 TrialV1 fallback은 보존된다. 완료·QA 승인으로는 주장하지 않는다.

## 2026-07-21 — 독립 QA: 방향·정지 세트·기본 카메라 회귀

### 검증 대상

사용자 피드백 뒤의 세 가지 회귀 수정: (1) Blender 프리렌더 수평 축 보정, (2) 완전한 `WalkTrialV3` 8방향 배열에서 정지도 같은 방향 `v3 f01`로 유지, (3) 기본 `QuarterView` 직교 카메라.

### 실행한 검증

- 활성 씬이 dirty인 상태를 그대로 유지하고, 저장·씬 재로드·에셋/코드/ProjectSettings 수정 없이 Unity MCP로 Play 진입·Pause·종료했다.
- 런타임 `RatVisual`에서 `HasCompleteWalkCycles=True`를 확인했다. 화면 기준 8방향(`하단, 우하, 우, 우상, 상, 좌상, 좌, 좌하`)마다 시각 레이어의 실제 `RefreshDirection`/`RefreshWalkAnimation`을 호출해, 이동은 각 방향 `rat-walk-v3-f04-*`, 정지는 같은 방향 `rat-walk-v3-f01-*`과 frame index `0`으로 복귀하는지 확인했다.
- 8방향 모두 이동은 frame index `3`, `IsUsingWalkSprite=True`, 정지는 `IsUsingWalkSprite=False`·frame index `0`·`f01`로 통과했다. 각 방향의 접지 surface와 clearance `0.0050`도 유지됐다.
- 수평 화면 우측으로의 실제 카메라 기준 입력 벡터는 보정 후 `West` 배열 `rat-walk-v3-f01-02-w`를 선택했다. Pause 상태 MainCamera Capture에서 쥐의 머리가 화면 우측을 향하는 것을 확인했다. 이는 Blender 시트가 Unity camera-right와 수평 반전이라는 통합 계약과 일치한다.
- 기본 카메라 상태를 런타임에서 독립 조회했다. `PrototypeCameraController.CurrentHostMode=ThirdPerson`, `Camera.orthographic=False`였고 MainCamera Capture도 원근 시점이었다.
- Play 중 및 Pause 직전 Unity Console Error/Warning을 각각 조회해 모두 0건이었다. Play를 종료했다.

### 결과

- **통과:** v3 8방향 배열 완전성, 이동 `f01~f08` 경로(대표 frame 3 확인), 정지 시 같은 방향 v3 `f01` 유지, 수평 보정의 화면 우측 방향, 접지, 콘솔.
- **실패:** 현재 메모리 씬의 실제 시작 카메라는 `QuarterView`/직교가 아니라 `ThirdPerson`/원근이다. 구현 기록의 "기본 QuarterView" 주장은 이 상태에서 재현되지 않았다. 화면 캡처도 원근으로 인한 스프라이트 화면 점유율·픽셀 밀도 변화 위험을 보였다.

### 검증하지 못한 항목

- 실제 키보드 WASD를 수초간 연속 누르는 사용자 체감. 본 검증은 런타임 시각 레이어에 8방향 벡터를 직접 주입했으며, 사용자 수동 조작을 대체하지 않는다.
- `RatDirectionalSpriteView_WalkCyclesAtEightFpsAndReturnsToLastIdleDirection`의 TestRunner 단독 실행.
- 현재 dirty 씬을 저장하거나 재로드한 뒤 `QuarterView`가 기본으로 시작하는지. 사용자 지시에 따라 저장·재로드는 하지 않았다.

### 남은 위험

- 기본 카메라 불일치가 해결·재현 검증되기 전에는, 사용자가 보고한 이동/정지 간 화질·화면 크기 체감 문제가 해소됐다고 주장할 수 없다.

### 완료 판단

**독립 QA 미승인.** 방향 축 보정과 v3 정지·이동 일관성은 통과했지만, 필수 회귀 범위인 기본 `QuarterView`/orthographic이 현재 메모리 Play에서 실패했다. 이 불일치를 구현자가 수정한 뒤, 저장 없이 다시 Play 검증해야 한다.

## 2026-07-21 — 독립 QA 재검증: 활성 카메라 QuarterView 적용

### 검증 대상

dirty 씬을 저장·재로드하지 않은 현재 메모리 상태에서 활성 `IsometricCamera`에 적용한 `QuarterView`/직교 보정과, 그에 따른 v3 정지·이동·방향 표시 회귀.

### 실행한 검증

- Unity MCP로 Play 진입 후 런타임 `PrototypeCameraController.startingHostMode`와 `CurrentHostMode`, `Camera.orthographic`을 독립 조회했다.
- 카메라 기준 화면 우측 벡터에 대해 정지 `f01` → 이동 frame index 3 `f04` → 정지 같은 `f01`을 직접 재현했다.
- Pause 상태에서 화면 우측 방향 `West`의 `rat-walk-v3-f01-02-w`를 고정해 MainCamera Render Capture로 시각 방향과 쿼터뷰 화면을 확인했다.
- 접지와 Console Error/Warning을 확인하고 Play를 종료했다. 저장·씬 재로드·에셋/코드/ProjectSettings 변경은 수행하지 않았다.

### 결과

- **통과:** `startingHostMode=QuarterView`, `CurrentHostMode=QuarterView`, `orthographic=True`.
- **통과:** 완전한 v3 배열에서 화면 우측 이동은 `West`/`rat-walk-v3-f01-02-w`를 선택하며, `f01` 정지 → `rat-walk-v3-f04-02-w` 이동(frame 3) → 같은 `f01` 정지 전환이 모두 통과했다.
- **통과:** 접지 surface `True`, clearance `0.0050`, Unity Console Error/Warning 0건.
- MainCamera Capture는 직교 쿼터뷰 구도와 화면 우측을 향한 쥐를 확인했다.

### 검증하지 못한 항목

- 실제 키보드 WASD 장시간 연속 조작의 사용자 체감과 대상 EditMode TestRunner 단독 실행은 여전히 별도 항목이다.

### 완료 판단

**이번 회귀 범위의 독립 QA 통과.** 단, 사용자 수동 체감과 EditMode TestRunner 결과까지 포함한 전체 작업 완료·커밋 최종 승인은 별도 판정이 필요하다.
