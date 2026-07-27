# 2D 플레이어블 기술 샘플 Unity 아키텍처

## 판정 요약

- Unity `6000.4.6f1`, 현재 URP, PC 우선 기준을 유지한다.
- `manifest.json`에 `com.unity.modules.tilemap`, `com.unity.modules.physics2d`, `com.unity.inputsystem 1.19.0`, `com.unity.test-framework 1.6.0`이 이미 있으므로 신규 패키지는 필요하지 않다.
- 기존 `RatHostPrototype.unity`, 3D 런타임 코드, `RatHostPrototypeSceneBuilder.cs`, 기존 스프라이트와 테스트는 수정하지 않는다.
- 새 샘플은 `RatHost2DTechnicalSample.unity`와 전용 런타임·Editor 빌더·테스트 어셈블리로 격리한다.
- 기존 `RatHostPrototypeControls.inputactions`의 `Host/Move`를 이름으로 조회해 읽기 전용 재사용한다. 자산을 다시 쓰거나 액션 GUID를 복제하지 않는다.
- `960×540`, `64×32 px`, `PPU 64`는 이 샘플의 시험값이며 사용자 수용 전에는 프로젝트 공통 규격으로 승격하지 않는다.

## 기존 구조 조사 결과

### 재사용 가능한 기반

- `Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`
  - `Host/Move`가 Input System `2DVector` WASD 바인딩을 이미 가진다.
  - 새 컨트롤러는 `InputActionAsset.FindAction("Host/Move")`로 조회한다.
  - 기존 씬 빌더의 `WriteInputActionsAsset()`는 호출하지 않는다. 이 메서드는 액션 GUID를 새로 생성하므로 샘플 빌더에서 재사용하면 안 된다.
- `LastHost.Prototype.asmdef`
  - Input System과 uGUI 참조가 이미 설정되어 있다.
  - 다만 새 샘플은 기존 런타임 어셈블리에 파일을 섞지 않고 별도 어셈블리로 둔다.
- 기존 테스트 구성
  - Editor 전용 EditMode 테스트 어셈블리와 NUnit 사용 방식은 참고할 수 있다.
- 기존 Windows 빌드 방식
  - `BuildPlayerOptions.scenes`에 씬 경로를 직접 전달하는 방식은 재사용할 수 있다.
  - 기존 빌더의 `UpdateBuildSettings()`는 전역 `EditorBuildSettings.scenes`를 덮어쓰므로 새 샘플에서는 호출하지 않는다.

### 격리해야 하는 기존 구현

- `RatHostController`
  - `CharacterController`, XZ 평면, 3D 카메라 상대 입력, 숙주 본능·면역 상태에 결합되어 있다.
  - 2D 기술 샘플의 `Rigidbody2D` 이동에 직접 재사용하지 않는다.
- `PrototypeCameraController`
  - 3D 타깃, 회전된 카메라 평면, 숙주·바이러스 모드 전환에 결합되어 있다.
  - 수학적 의도만 참고하고 2D 전용 고정 직교 카메라를 만든다.
- `RatDirectionQuantizer`
  - XZ 평면과 3D 카메라 right/forward를 사용하므로 2D XY 스프라이트 방향 판정에 직접 사용하지 않는다.
- `RatVisualPixelSnapper`
  - XZ 스냅용이다. 2D 샘플에서는 XY 기준 절대 위치 스냅 함수를 별도로 둔다.
- `PrototypeSessionController`, `PrototypeHud`, 면역·변이·바이러스 미니게임
  - 이번 샘플 범위가 이동·충돌·정렬·카메라 기술 검증이므로 연결하지 않는다.
  - 핵심 루프 이관 승인을 받기 전에는 새 샘플 어셈블리가 이 타입들에 의존하지 않게 한다.

## 파일 및 어셈블리 구조안

```text
UnityProject/Assets/_Project/
  Scenes/
    RatHostPrototype.unity                         # 기존 3D, 변경 금지
    RatHost2DTechnicalSample.unity                 # 신규 샘플

  Scripts/TechnicalSample2D/
    LastHost.Prototype.TechnicalSample2D.asmdef
    TechnicalSample2DConstants.cs
    TechnicalSample2DInput.cs
    Movement2DModel.cs
    RatHost2DController.cs
    Direction8Model.cs
    RatHost2DView.cs
    PixelGrid2D.cs
    PixelFollowCamera2D.cs
    YSortOrder2D.cs
    YSortSprite2D.cs
    TechnicalSample2DHud.cs

  Editor/TechnicalSample2D/
    LastHost.Prototype.TechnicalSample2D.Editor.asmdef
    RatHost2DTechnicalSampleSceneBuilder.cs
    RatHost2DTechnicalSampleWindowsBuilder.cs

  Tests/EditMode/TechnicalSample2D/
    LastHost.Prototype.TechnicalSample2D.Tests.asmdef
    Movement2DModelTests.cs
    Direction8ModelTests.cs
    PixelGrid2DTests.cs
    YSortOrder2DTests.cs
    RatHost2DTechnicalSampleSceneTests.cs

  Art/TechnicalSample2D/
    Tiles/
      Floor/
      Wall/
      Water/
    Sprites/
      RatPlaceholder/
      Props/
    TileAssets/
```

### 어셈블리 경계

- 런타임 어셈블리
  - 이름: `LastHost.Prototype.TechnicalSample2D`
  - 참조: `Unity.InputSystem`, `UnityEngine.UI`
  - `LastHost.Prototype` 참조는 두지 않는다. 기존 핵심 상태나 3D Host 코드의 우발적 결합을 막는다.
- Editor 어셈블리
  - Editor 플랫폼 전용
  - 새 런타임 어셈블리만 참조한다.
  - 기존 `RatHostPrototypeSceneBuilder`를 호출하거나 수정하지 않는다.
- 테스트 어셈블리
  - Editor 플랫폼과 `UNITY_INCLUDE_TESTS` 전용
  - 새 런타임 어셈블리, `UnityEngine.TestRunner`, `UnityEditor.TestRunner`만 참조한다.
  - 기존 `RatHostPrototypeCoreTests.cs`에 새 테스트를 추가하지 않는다.

## 씬 구조안

```text
RatHost2DTechnicalSample
  TechnicalSample2D
    World2D
      Grid                         [Grid: Isometric]
        FloorTilemap              [Tilemap, TilemapRenderer / 고정 배경]
        WaterVisualTilemap        [Tilemap, TilemapRenderer / 고정 배경]
        WallVisualTilemap         [Tilemap, TilemapRenderer / Individual]
        BlockingTilemap           [TilemapRenderer off]
                                   [TilemapCollider2D, CompositeCollider2D,
                                    Rigidbody2D Static]
      Props
        Crate_A                   [SpriteRenderer, Collider2D, YSortSprite2D]
        Pipe_A                    [SpriteRenderer, Collider2D, YSortSprite2D]
      SpawnPoints
        RatSpawn
    Actors
      RatHost2D                   [Rigidbody2D Dynamic, CapsuleCollider2D,
                                   RatHost2DController]
        Visual                    [SpriteRenderer, RatHost2DView]
        FootPoint                 [Y 정렬 기준]
    Cameras
      Main Camera                 [Orthographic, PixelFollowCamera2D]
    UI
      Canvas                      [Screen Space Overlay]
        SampleTitle
        SpecText
        ControlsText
        RuntimeStatusText
      EventSystem                 [InputSystemUIInputModule]
```

### Tilemap 구성

- Grid는 XY 평면, `Cell Layout = Isometric`을 사용한다.
- `64×32 px` 타일을 `PPU 64`로 가져오면 시험 셀은 `1×0.5` 월드 단위가 된다.
- 바닥과 물 시각 타일은 충돌을 소유하지 않는다.
- 벽·수로의 통행 금지는 별도 `BlockingTilemap`에 기록한다.
- `BlockingTilemap`은 렌더러를 끄고 `TilemapCollider2D + CompositeCollider2D + Static Rigidbody2D`로 합친다.
- 가림 검증이 필요한 벽과 큰 소품은 개별 SpriteRenderer 또는 Individual 모드 타일로 두어 발 접지 Y와 비교할 수 있게 한다.
- 최종 타일 팔레트나 최종 아트가 아니라 빌더가 만드는 단색·제한 팔레트 기술 플레이스홀더만 사용한다.

## 2D 이동·입력 경계

- `TechnicalSample2DInput`
  - 기존 InputActionAsset을 참조하되 `Host/Move`를 경로·이름으로 조회한다.
  - `OnEnable/OnDisable`에서 액션을 활성화·비활성화한다.
  - 액션 자산 자체는 수정하지 않는다.
- `Movement2DModel`
  - 입력 dead zone, 대각선 정규화, 속도 적용을 순수 함수로 제공한다.
  - `(1,1)` 입력의 속도가 축 입력보다 빠르지 않아야 한다.
- `RatHost2DController`
  - Update에서 입력을 캐시하고 FixedUpdate에서 `Rigidbody2D.MovePosition`으로 이동한다.
  - `gravityScale = 0`, Z 회전 고정, 동적 Rigidbody2D와 CapsuleCollider2D를 사용한다.
  - 화면 기준 WASD는 `W=(0,+1)`, `S=(0,-1)`, `A=(-1,0)`, `D=(+1,0)`이다.
  - 이번 샘플에는 숙주 본능·면역 경계도·맵 경계 Clamp를 넣지 않는다. 실제 Collider2D가 이동 경계를 결정한다.

## 8방향 표시와 픽셀 스냅

- `Direction8Model`
  - XY 이동 벡터를 45도 단위 8방향으로 양자화한다.
  - 마지막 유효 방향을 유지해 정지 시 임의로 남쪽으로 튀지 않게 한다.
- `RatHost2DView`
  - 기술 플레이스홀더 8방향 Sprite 배열과 SpriteRenderer만 소유한다.
  - 방향 전환은 Rigidbody2D, Collider2D, root 위치를 바꾸지 않는다.
- `PixelGrid2D`
  - `1 / PPU = 1/64` 월드 단위로 XY 절대 좌표를 반올림한다.
- 시각 자식 스냅
  - 매 프레임 누적 offset을 더하지 않는다.
  - 논리 root의 현재 월드 위치에서 직접 계산한 스냅 위치를 Visual에 대입한다.
- 카메라 스냅
  - RatHost2D root를 기준으로 목표 위치를 매 LateUpdate 새로 계산한 뒤 `1/64` 단위로 스냅한다.
  - 카메라와 Visual이 각각 이전 프레임 위치를 누적하지 않게 해 기존의 캐릭터-카메라 분리 회귀를 막는다.

## 카메라와 화면 시험값

- Camera projection: Orthographic
- Camera transform: `(0, 0, -10)`, 회전 없음
- 기준 내부 화면: `960×540`
- PPU: `64`
- 세로 월드 크기: `540 / 64 = 8.4375`
- 시험 orthographic size: `8.4375 / 2 = 4.21875`
- 가로 월드 크기: `960 / 64 = 15`
- follow: 기술 검증에서는 보간 없이 즉시 추적 후 스냅한다.
- 확대 출력: 우선 `960×540` 창과 `1920×1080` 2배 정수 확대를 비교한다.
- Pixel Perfect Camera 패키지는 설치하지 않는다. 이번 샘플은 전용 계산 코드와 Point 필터로 성립 여부를 검증한다.

## Y 정렬 규칙

- ProjectSettings의 Sorting Layer를 추가하지 않고 기본 레이어와 숫자 `sortingOrder`만 사용한다.
- 발 접지점의 논리 Y가 낮을수록 앞에 표시한다.
- 순수 계산 후보:

```text
sortingOrder = BaseOrder - RoundToInt(footWorldY * 100) + explicitTieBreak
```

- actor와 앞뒤가 바뀌어야 하는 props는 같은 `BaseOrder`를 사용한다.
- `explicitTieBreak`는 같은 Y에 놓인 오브젝트의 순서를 이름이나 인스턴스 생성 순서에 맡기지 않고, 빌더가 명시적으로 부여하는 작은 정수다.
- PPU 64 기준 `1/64` 월드 단위의 Y 차이는 `RoundToInt(footWorldY * 100)`에서 최소 1 order 차이를 만들어야 한다.
- 바닥·물 같은 절대 배경은 별도의 낮은 고정 order를 사용한다.
- 벽 상단 장식처럼 항상 앞에 있어야 하는 부분은 별도 foreground renderer로 분리한다.
- Y order는 논리 root/FootPoint에서 계산하고, 스냅된 Visual 위치의 미세 변화로 매 프레임 흔들리지 않게 한다.
- 카메라의 `transparencySortMode`와 축은 씬 카메라에만 설정할 수 있으나, 샘플의 합격 근거는 `YSortOrder2D`의 결정론적 결과로 둔다.

## HUD 경계

- 기존 `PrototypeHud`는 세션·면역·변이 상태에 결합되어 있으므로 재사용하지 않는다.
- 전용 HUD는 다음만 표시한다.
  - `2D TECHNICAL SAMPLE`
  - `960×540 / Tile 64×32 / PPU 64`
  - `WASD 이동`
  - 현재 방향, root 위치, 카메라-쥐 화면 중심 오차
- 목업의 최종 HUD 디자인이나 실제 생명력·면역 게이지를 구현하지 않는다.

## Editor 씬 빌더 원칙

- 메뉴 후보: `Last Host/Technical Sample 2D/Rebuild Scene`
- 새 전용 폴더와 에셋만 생성·갱신한다.
- 기존 씬, 기존 inputactions, 기존 스프라이트 import, 기존 재질을 쓰지 않는다.
- `EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single)`로 생성한 뒤 새 경로에만 저장한다.
- 재실행해도 동일 경로·동일 이름·동일 값이 나오는 결정론적 빌더로 만든다.
- `AssetDatabase.SaveAssets/Refresh`는 전용 에셋 생성 후에만 호출한다.
- `EditorBuildSettings.scenes`, TagManager, Physics2DSettings, GraphicsSettings, QualitySettings, PlayerSettings를 수정하지 않는다.
- 빌드 전후 `RatHostPrototype.unity`의 Git diff가 없어야 한다.

## ProjectSettings 비변경 전략

- 신규 Tag, Layer, Sorting Layer를 만들지 않는다.
- 기본 2D 충돌 매트릭스를 사용하고 필요한 제외 관계는 `Physics2D.IgnoreCollision` 또는 명시적 Collider 참조로 샘플 내부에서 처리한다.
- 기존 URP Renderer를 유지하며 Renderer2D로 교체하지 않는다.
- 새 Pixel Perfect 패키지와 전역 픽셀 설정을 추가하지 않는다.
- 기본 해상도·fullscreen 값을 PlayerSettings에 쓰지 않는다.
- `APP_UI_EDITOR_ONLY`가 포함된 사용자 로컬 `ProjectSettings.asset` 변경을 스테이징하거나 덮어쓰지 않는다.
- 구현 전후 다음 경로의 diff를 별도 확인한다.

```text
UnityProject/ProjectSettings/**
UnityProject/Packages/manifest.json
UnityProject/Packages/packages-lock.json
UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity
UnityProject/Assets/_Project/Editor/RatHostPrototypeSceneBuilder.cs
```

## 임시 Windows 빌드 전략

- 메뉴 후보: `Last Host/Technical Sample 2D/Build Windows Temporary`
- `BuildPlayerOptions.scenes = new[] { "Assets/_Project/Scenes/RatHost2DTechnicalSample.unity" }`를 직접 사용한다.
- 전역 `EditorBuildSettings.scenes`를 변경하지 않는다.
- 출력 후보:

```text
C:/tmp/LastHost2DTechnicalSample/<run-id>/LastHost2DTechnicalSample.exe
```

- `C:/tmp`를 사용할 수 없으면 `Path.GetTempPath()/LastHost2DTechnicalSample/<run-id>/` 아래에 출력한다.
- 창 해상도는 PlayerSettings를 바꾸지 않고 실행 인자
  `-screen-width 960 -screen-height 540 -screen-fullscreen 0`으로 검증한다.
- 빌드 결과는 임시 검증 산출물이며 커밋 대상이 아니다.
- 빌드 성공만으로 화면·조작 수용을 주장하지 않고 같은 실행본의 화면, 입력, 충돌, Y 정렬과 Player.log를 별도 확인한다.

## 구현 담당별 파일 후보

| 담당 | 생성·변경 후보 | 변경 금지 |
| --- | --- | --- |
| 게임플레이 구현 | `Scripts/TechnicalSample2D/**`, `Tests/EditMode/TechnicalSample2D/**` | 기존 `Scripts/Core`, `Scripts/Host`, 기존 테스트 파일 |
| Unity 씬/통합 구현 | `Editor/TechnicalSample2D/**`, `Scenes/RatHost2DTechnicalSample.unity`, `Art/TechnicalSample2D/**` | 기존 씬 빌더, 기존 3D 씬, inputactions, ProjectSettings |
| 비주얼/테크아트 | 플레이스홀더 규격 검토와 별도 source PNG 후보 | 목업 직접 분할, 최종 에셋 선언 |
| QA/검증 | 새 테스트 실행, MCP Play, 콘솔, 임시 Windows 빌드, 보호 경로 diff | 기능 코드·씬 수정 |

## 최소 테스트 분리

- `Movement2DModelTests`
  - 축·대각선·반대 키 상쇄·0 입력과 정규화
- `Direction8ModelTests`
  - 8방향 경계와 정지 시 마지막 방향 유지
- `PixelGrid2DTests`
  - PPU 64 스냅, 음수 좌표, NaN/잘못된 PPU 방어
- `YSortOrder2DTests`
  - 낮은 Y가 더 큰 order, 같은 Y에서 `explicitTieBreak` 결정성, 경계 양자화
  - PPU 64 기준 정확히 `1/64` 차이 나는 두 foot Y가 최소 1 sorting order 차이를 만든다.
- `RatHost2DTechnicalSampleSceneTests`
  - 새 씬 경로와 root 존재
  - Main Camera 직교·`4.21875`
  - Grid Isometric, 필수 Tilemap과 Collider2D
  - RatHost2D의 Rigidbody2D·Collider2D·Visual·FootPoint
  - 기존 3D 씬 경로와 활성 Build Settings가 변하지 않음

## 구현 순서

1. 게임플레이 구현 담당이 순수 모델과 런타임 컴포넌트, EditMode 테스트를 만든다.
2. Unity 씬/통합 담당이 전용 빌더와 기술 플레이스홀더를 만들고 새 씬만 저장한다.
3. 새 테스트 전체와 기존 EditMode 회귀 테스트를 함께 실행한다.
4. Unity MCP Play에서 WASD, 벽·수로 충돌, 카메라 중심, 순간이동 없음, Y 정렬, Console 0을 확인한다.
5. 임시 Windows 빌드를 명시적 단일 씬으로 생성하고 `960×540` 창에서 검증한다.
6. QA 판정과 사용자 화면·조작 수용 후에만 시험 규격 승격 또는 핵심 루프 이관을 제안한다.

## 구현 전 위험과 대응

- **아이소메트릭 외형과 2D 물리 불일치**: 충돌은 장식 타일이 아니라 별도 BlockingTilemap으로 관리한다.
- **픽셀 스냅으로 물리 root가 흔들림**: Rigidbody2D는 연속 좌표를 유지하고 Visual과 Camera만 절대 위치에서 스냅한다.
- **카메라와 쥐가 다시 분리됨**: 둘 다 같은 논리 root를 입력으로 사용하고 누적 offset을 금지한다.
- **Y 정렬 팝**: foot Y를 PPU 단위로 양자화하고 actor·props가 같은 계산 함수를 사용한다.
- **기존 입력 자산 재생성**: 빌더는 inputactions를 쓰지 않고 이름 조회만 한다.
- **전역 설정 오염**: 전역 Build Settings와 ProjectSettings를 건드리지 않고 단일 씬 BuildPlayerOptions를 사용한다.
- **기술 샘플이 전체 이관으로 확대됨**: 면역·변이·미니게임·최종 HUD는 참조하지 않고 별도 승인까지 제외한다.

## 아키텍처 완료 판정

이 구조는 신규 패키지와 ProjectSettings 변경 없이 별도 2D 플레이어블 기술 샘플을 구현할 수 있다. 다음 단계는 게임플레이 구현 에이전트와 Unity 씬/통합 구현 에이전트가 각각 지정된 전용 경로만 변경하는 것이다. 실제 동작 완료와 규격 승격은 EditMode·MCP Play·Windows 빌드·사용자 수용 전에는 주장하지 않는다.
