# 1단계 2D 쥐 숙주 씬·통합 계획

## 조사 대상

- 기존 2D 기술 샘플 씬:
  - `Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- 기존 기술 샘플 빌더:
  - `Assets/_Project/Editor/TechnicalSample2D/RatHost2DTechnicalSampleSceneBuilder.cs`
- 읽기·참조 재사용 에셋:
  - `Assets/_Project/Art/TechnicalSample2D/Tiles/*.asset`
  - `Assets/_Project/Art/TechnicalSample2D/Textures/rat-*.png`
  - `Assets/_Project/Art/TechnicalSample2D/Textures/prop-pipe.png`
  - `Assets/_Project/Art/TechnicalSample2D/Textures/prop-barrel.png`
  - `Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`
- 읽기·참조 재사용 런타임:
  - `LastHost.Prototype.TechnicalSample2D`
  - `PixelFollowCamera2D`, `VisualPixelSnap2D`, `RatHost2DView`
  - `YSortSprite2D`, `YSortOrder2D`

## 기존 기술 샘플에서 유지할 계약

- Grid는 XY 평면의 `Isometric`, 시험 셀 크기 `(1, 0.5, 1)`을 사용한다.
- 바닥은 비충돌, 물·벽은 `TilemapCollider2D`를 가진다.
- 쥐 root는 `Rigidbody2D Dynamic`, 중력 0, 회전 고정, Continuous collision과 수평 `CapsuleCollider2D`를 사용한다.
- 쥐 Visual과 카메라는 같은 쥐 물리 root를 기준으로 절대 위치 스냅한다.
- 8방향 표시와 발 접지점 Y 정렬은 방향 전환 시 물리 root를 바꾸지 않는다.
- Pipe와 Barrel은 하단 footprint `BoxCollider2D`를 유지하며 같은 Y 정렬 계산을 사용한다.
- 기존 `Host/Move` InputAction은 이름 조회로 참조만 하고 입력 에셋을 재작성하지 않는다.
- 기존 기술 샘플의 `960×540`, PPU 64, 타일 64×32, 카메라 크기는 1단계 시험값이다. 최종 규격으로 선언하지 않는다.

## 새 씬 경로와 루트

- 새 씬: `Assets/_Project/Scenes/RatHost2DPrototype.unity`
- 새 빌더: `Assets/_Project/Editor/RatHost2D/RatHost2DPrototypeSceneBuilder.cs`
- 새 Editor 어셈블리: `Assets/_Project/Editor/RatHost2D/LastHost.Prototype.RatHost2D.Editor.asmdef`

추천 계층:

```text
RatHost2DPrototype
  Core2D
    RatHost2DSessionController
  HostMode2D
    World2D
      Grid
        FloorTilemap
        WaterTilemap
        BlockingTilemap
      YSortProps
        Pipe_A
        Barrel_A
      ContaminationZone2D
    RatHost2D
      Visual
      FootPoint
    HostCamera2D
  InternalVirusShell2D
    ShellBackdrop 또는 전환 셸 표시 root
  UI2D
    HostHud2D
      HostHealth
      ImmuneAlert
      CurrentMode
      CauseFeedback
      Controls/시험값 안내
    InternalVirusShellHud2D
      1단계 기술 통합 안내
      WhiteBloodCellEvasion 인계 표시
      실제 미니게임 미구현 안내
```

## 필수 컴포넌트 연결

### Core2D

- 게임플레이 담당의 `RatHost2DSessionController`
  - `PrototypeSessionState` 단일 인스턴스 소유
  - Host controller, contamination zone, Host HUD, internal shell root 참조
  - 시작 모드 `RatHost`
  - 기본 내부 타입 `WhiteBloodCellEvasion`
  - `BaseAlertPerSecond=0`
- 새 씬에 기존 `PrototypeSessionController`를 추가하지 않는다.

### HostMode2D / RatHost2D

- `Rigidbody2D`
- `CapsuleCollider2D`
- 게임플레이 담당의 2D 숙주 본능/WASD controller
- 기존 `Host/Move` InputAction 참조
- `RatHost2DView`
- `VisualPixelSnap2D`
- `YSortSprite2D`
- `FootPoint`

연결 조건:

- controller와 Visual·카메라의 target은 모두 동일한 `RatHost2D` root다.
- 기존 숙주 본능 XZ 규칙의 XY 어댑터는 게임플레이 담당 API 안에서 처리한다.
- 씬 빌더는 임의 Transform 이동이나 별도 카메라 target을 만들지 않는다.

### World2D

- 기술 샘플과 같은 타일 에셋을 참조해 작은 하수도 방을 구성한다.
- 물·벽은 통행 차단 Collider2D를 유지한다.
- Pipe:
  - 위치 시험값 `(-1.35, 0.45)`
  - footprint size `(0.27, 0.16)`
  - offset `(0, 0.02)`
- Barrel:
  - 위치 시험값 `(1.1, -0.65)`
  - footprint size `(0.31, 0.14)`
  - offset `(0, 0.02)`
- 오염 구역은 플레이어 시작점과 겹치지 않고 실제 이동으로 진입·이탈할 수 있는 위치에 둔다.
- 오염 시각 표시와 Trigger Collider2D는 분리하거나, 시각 SpriteRenderer가 게임플레이 판정 위치를 바꾸지 않게 한다.
- 오염 zone 시험값:
  - 면역 경계도 `+12/초`
  - 숙주 생명력 `-4/초`
  - 원인 `ContaminationExposure`
  - 피드백 `오염 노출`

### HostCamera2D

- `Camera` Orthographic
- `PixelFollowCamera2D`
- target은 `RatHost2D` root
- 시험 PPU·orthographic size는 기술 샘플 상수를 참조한다.
- 카메라는 새 씬에서 하나만 MainCamera 태그를 가진다.
- 내부 전환 셸에서 Host 월드를 계속 보일지 전체 화면 셸을 보일지는 게임플레이 세션 API의 root 활성 정책에 맞춘다. 카메라가 비활성화되어 렌더 카메라가 0개가 되는 구성은 금지한다.

### UI2D

- Screen Space Overlay Canvas와 `960×540` 시험 reference resolution
- 게임플레이 담당 HUD API가 요구하는 Text/Slider 참조를 빌더에서 명시적으로 연결한다.
- Host HUD 필수 출력:
  - 숙주 생명력 현재값/최대값
  - 면역 경계도 현재값/100
  - 현재 모드
  - 마지막 면역 원인 피드백
- 내부 셸 필수 출력:
  - `1단계 기술 통합`
  - `WhiteBloodCellEvasion 인계 확인`
  - `실제 내부 미니게임은 2단계에서 구현`
- HUD가 화면 중앙의 쥐와 오염 구역 판독을 가리지 않게 상단/하단 가장자리에 배치한다.
- 기술 샘플 HUD와 Telemetry를 새 제품 상태 HUD로 재사용하지 않는다.

## 모드 전환 연결

- 100% 이전:
  - Host controller 활성
  - RatHost Collider2D와 오염 Trigger 판정 활성
  - Host HUD 활성
  - Internal shell 비활성
- `ContaminationExposure`로 `99.x → 100` 도달:
  - 상태가 `InternalVirus`
  - 내부 타입이 `WhiteBloodCellEvasion`
  - 전환은 한 번만 처리
- 전환 이후:
  - Host 입력과 물리 이동 중지
  - RatHost gameplay Collider2D 비활성
  - 오염 zone 판정 비활성
  - Host HUD 비활성
  - Internal shell과 셸 HUD 활성
  - 경계도·생명력 추가 변화 없음

## 결정적 씬 빌더 원칙

- 메뉴:
  - `Last Host/Rat Host 2D/Stage 1/Rebuild Scene`
  - `Last Host/Rat Host 2D/Stage 1/Build Windows Temporary`
- `EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single)`에서 시작한다.
- 기존 기술 샘플 빌더 메서드를 호출하지 않는다.
  - 기존 빌더는 기술 샘플 아트 생성·import까지 수행하므로 호출하면 보호 경계를 침범할 수 있다.
- 기존 타일·Sprite·InputActionAsset을 `AssetDatabase.LoadAssetAtPath`로 읽기만 한다.
- 필수 참조가 없으면 새 에셋을 생성하거나 기존 에셋을 고치지 않고 명확한 예외로 중단한다.
- 동일 빌더 재실행 시 같은 이름·계층·위치·직렬화 값이 생성되어야 한다.
- 저장 대상은 새 `RatHost2DPrototype.unity` 한 개다.
- `EditorBuildSettings.scenes`, `PlayerSettings`, `ProjectSettings`, Tag/Layer/Sorting Layer를 수정하지 않는다.

## 임시 Windows 빌드

- `BuildPlayerOptions.scenes = new[] { RatHost2DPrototypeScenePath }`로 씬을 직접 지정한다.
- `EditorBuildSettings.scenes`를 쓰지 않는다.
- 출력:

```text
C:/tmp/LastHostRatHost2DStage1/<yyyyMMdd-HHmmss>/LastHostRatHost2DStage1.exe
```

- 저장소 `Builds/`와 기존 임시 빌드를 덮어쓰지 않는다.
- `StandaloneWindows64`, Development 빌드를 사용한다.
- 빌드 성공과 Windows 실행본 플레이 통과는 분리해 기록한다.

## 보호 경계

수정 금지:

- `Assets/_Project/Scenes/RatHostPrototype.unity`
- `Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- `Assets/_Project/Editor/TechnicalSample2D/**`
- `Assets/_Project/Scripts/TechnicalSample2D/**`
- `Assets/_Project/Tests/EditMode/TechnicalSample2D/**`
- `Assets/_Project/Art/TechnicalSample2D/**`
- `Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`
- `ProjectSettings/**`
- `Packages/**`
- 저장소 `Builds/**`
- 사용자 `_workspace/previews/**`

## 런타임 API 준비 전 차단 조건

다음 API가 준비되기 전에는 씬 빌더와 씬을 구현하지 않는다.

- 2D 세션 컨트롤러와 초기화/참조 연결 방식
- 2D 숙주 본능/WASD controller와 InputActionAsset 연결 방식
- 2D 오염 구역과 세션 연결 방식
- Host HUD와 내부 전환 셸 HUD의 참조/Configure 방식

현재 위 API가 존재하지 않으면 본 계획까지만 완료하고 메인 조정자에게 대기 상태를 알린다.
