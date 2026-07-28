# 게임플레이 구현 인계

## 작업명

1단계 2D 쥐 숙주·면역 경계도·자연 100% 전환 통합

## 작업 ID

`2026-07-28-rat-host-2d-stage1-integration`

## 담당 수행 주체

게임플레이 구현 에이전트

## 구현 범위

- 새 `LastHost.Prototype.RatHost2D` 런타임 어셈블리
- `PrototypeSessionState` 단일 인스턴스를 소유하는 2D 세션
- 기존 XZ 숙주 본능/WASD 모델을 XY Rigidbody2D에 연결하는 어댑터
- 기술 샘플 `RatHost2DController`를 단일 충돌 모터로 조합한 2D 이동
- 무입력 본능 이동과 활성 WASD 방향 우선
- 2D 오염 노출 `+12/초`, 숙주 생명력 `-4/초`
- 자연 100%에서 `WhiteBloodCellEvasion` 내부 전환 셸로 단일 인계
- 전환 후 Host 입력·이동·Collider·위험·Host HUD 중지
- Host HUD 읽기 snapshot과 동적 uGUI presenter
- 신규 EditMode 테스트

실제 내부 바이러스 플레이, 성공·실패, 변이, 신호 억제, 소음·강제 조종 면역 트리거는 구현하지 않았다.

## 변경한 코드

- `Scripts/RatHost2D/LastHost.Prototype.RatHost2D.asmdef`
- `Scripts/RatHost2D/RatHost2DControlAdapter.cs`
- `Scripts/RatHost2D/RatHost2DHudSnapshot.cs`
- `Scripts/RatHost2D/RatHost2DSessionController.cs`
- `Scripts/RatHost2D/RatHost2DMovementController.cs`
- `Scripts/RatHost2D/RatHost2DContaminationZone.cs`
- `Scripts/RatHost2D/RatHost2DStage1Hud.cs`
- 위 파일과 폴더의 Unity `.meta`

어셈블리는 `LastHost.Prototype`, `LastHost.Prototype.TechnicalSample2D`, `Unity.InputSystem`, `UnityEngine.UI`를 참조한다. 기존 `Core`, `Host`, `Immune`, `TechnicalSample2D` 코드는 수정하지 않았다.

## 변경한 테스트

- `Tests/EditMode/RatHost2D/LastHost.Prototype.RatHost2D.Tests.asmdef`
- `Tests/EditMode/RatHost2D/RatHost2DControlAdapterTests.cs`
- `Tests/EditMode/RatHost2D/RatHost2DSessionTests.cs`
- `Tests/EditMode/RatHost2D/RatHost2DMovementAndRiskTests.cs`
- 위 파일과 폴더의 Unity `.meta`

테스트는 XY↔XZ 방향 왕복, 무입력 본능 이동, 8방향 활성 WASD 우선, 대각선 정규화, 본능 반대 입력 속도 페널티, 무위험 변화 0, 오염 시험값, 자연 100%, 단일 전환, 전환 후 상태·이동 중지, HUD 갱신을 고정한다.

## 주요 런타임 API

### 세션

```csharp
session.Configure(
    GameObject hostRoot,
    GameObject shellRoot,
    GameObject hudRoot,
    RatHost2DMovementController movement,
    Collider2D[] colliders);
```

- `State`, `CurrentMode`, `CanProcessHostGameplay`, `InternalShellEntryCount`
- `ApplyContaminationExposure(float deltaTime)`
- `ReadHostHud()`
- `HostHudChanged`, `ModeChanged`
- `InternalShellTitle`, `InternalShellObjective`
- 기본값: `BaseAlertPerSecond = 0`, `WhiteBloodCellEvasion`

### 이동

```csharp
movement.Configure(session, technicalSample2DInput, speed);
movement.ConfigureInstinct(
    initialDirection,
    horizontalBounds,
    verticalBounds,
    turnInterval,
    turnAngle);
```

- 입력은 `TechnicalSample2DInput.Configure(existingInputActionAsset)`로 기존 `Host/Move`를 연결한다.
- 카메라는 반드시 `movement.FollowTarget`을 사용한다.
- `FollowTarget`은 Rigidbody2D 논리 root의 `transform`이며 별도 누적 위치가 없다.
- 기술 샘플 `RatHost2DController`는 비활성 충돌 모터로 조합한다. 새 이동 컨트롤러만 `CacheMoveInput/SimulateFixedStep`을 호출해 두 번째 Update/FixedUpdate 입력 덮어쓰기를 막는다.

### 오염

```csharp
zone.Configure(session, movement, 12f, 4f, "오염 노출");
```

- `Collider2D.isTrigger = true`
- Host 모드가 아니면 추가 상태 변경 없음

### HUD

```csharp
hud.Configure(session, healthText, alertText, modeText, feedbackText);
hud.ConfigureSliders(healthSlider, alertSlider); // 선택
```

- `HostHudChanged`와 `ReadHostHud()`만 읽으며 상태를 직접 변경하지 않는다.
- 내부 셸 안내는 `InternalShellTitle`, `InternalShellObjective`를 사용한다.

## 실행한 검증

### 컴파일

Unity 임포트 후 다음 어셈블리 생성까지 확인했다.

- `LastHost.Prototype.RatHost2D.dll`
- `LastHost.Prototype.RatHost2D.Editor.dll`
- `LastHost.Prototype.RatHost2D.Tests.dll`

최초 신규 테스트 실행 전 Unity Console에 C# 컴파일 오류는 없었다.

### 신규 EditMode 최초 실행

- leaf 기준 `36 passed / 1 failed / 0 skipped / 0 inconclusive`
- 실패: `IdleInstinctMovesSingleLogicalRootAndExposesSameCameraTarget`
- 원인: EditMode에서 Kinematic `Rigidbody2D.MovePosition` 직후 물리 스텝을 시뮬레이션하지 않고 `body.position`을 검사했다.
- 1차 수정안의 `Physics2D.simulationMode` 변경과 `Physics2D.Simulate`는 전역 `Physics2DSettings.asset` 재직렬화 위험이 있어 제거했다.
- 최종 테스트 계약은 실제 물리 월드를 진행하지 않고 `movement.Motor.LastFixedStepDelta`의 Y 양수·X 0, 현재 이동 방향, `FollowTarget`, `LogicalPosition`과 물리 root의 동일성을 검증한다.

이 최종 테스트 보완 뒤에는 지시에 따라 Unity/MCP를 실행하지 않았다. 최종 통과 수치는 주장하지 않으며 QA/검증 에이전트가 재실행해야 한다.

## 씬/통합 인계

- 기존 InputActionAsset을 `TechnicalSample2DInput.Configure`로 연결
- 세션·이동·오염·HUD의 위 Configure API 연결
- 카메라 target은 `movement.FollowTarget`으로 고정
- Host Collider를 세션 colliders 배열에 전달
- 내부 셸에는 1단계 안내만 표시
- Y 정렬·시각 스냅은 기술 샘플 공개 컴포넌트를 읽기 전용 조합

## 남은 위험

- 전역 Physics2D 설정 비변경 상태에서 신규 EditMode 테스트 최종 재실행 필요
- 실제 InputAction, Trigger, 카메라, HUD root 전환은 신규 씬 QA 필요
- 본능 회전 주기와 속도는 시험값이며 최종 조작감이 아님
- 비활성 `RatHost2DController` 충돌 모터가 씬에서 이중 활성화되지 않는지 확인 필요
- Windows 실행본과 사용자 수동 플레이 전에는 1단계 완료 주장 불가

## 완료 판단

게임플레이 구현 에이전트 담당 범위의 코드·테스트 작성과 통합 API 인계는 끝났다. QA/검증 전이므로 작업 전체 완료 주장은 하지 않는다.
