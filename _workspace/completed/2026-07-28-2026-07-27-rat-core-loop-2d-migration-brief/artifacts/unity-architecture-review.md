# 쥐 숙주 핵심 루프 단계적 2D 이관 Unity 아키텍처 검토

## 검토 판정

- 추천 씬 전략은 기존 기술 샘플을 계속 확장하는 방식이 아니라 별도 `RatHost2DPrototype.unity`를 만드는 것이다.
- `RatHost2DTechnicalSample.unity`는 이동·충돌·카메라·Y 정렬의 독립 회귀 기준으로 동결하고, 새 프로토타입 씬이 검증된 기술 요소를 가져다 쓰게 한다.
- 기존 `RatHostPrototype.unity`와 3D/2.5D 구현은 2D 핵심 루프 전체가 사용자 수용을 받을 때까지 수정·삭제하지 않는다.
- 첫 이관에서는 기존 차원 독립 상태 모델을 옮기거나 복제하지 않고 재사용한다. 대신 3D 결합 `MonoBehaviour`는 새 2D 어댑터·컨트롤러로 교체한다.
- `PrototypeSessionState`를 별도 Domain 어셈블리로 즉시 분리하는 리팩터링은 1차 이관 범위에서 제외한다. 먼저 새 2D 통합 어셈블리에서 참조하고, 전체 루프 수용 후 양쪽 구현이 공유하는 Domain 어셈블리 추출을 별도 작업으로 판단한다.
- 추천 1차 구현은 `쥐 이동·충돌 + 숙주 본능 인계 + 면역 경계도 + 100% 내부 모드 전환 셸`이다. 내부 미니게임 실제 플레이와 변이 선택은 다음 단계로 분리한다.

## 저장소 조사 근거

### 이미 검증된 2D 기술 기반

- `Scripts/TechnicalSample2D/`는 별도 `LastHost.Prototype.TechnicalSample2D` 어셈블리이며 기존 `LastHost.Prototype`을 참조하지 않는다.
- 다음 요소는 2D 기술 샘플에서 사용자 수용과 자동 회귀 검증을 받은 기반이다.
  - `Movement2DModel`: 화면 기준 XY 입력 정규화와 고정 스텝 이동량
  - `RatHost2DController`: `Rigidbody2D.Cast` 기반 통행 차단과 `MovePosition`
  - `Direction8Model`, `RatHost2DView`: 8방향 표시와 정지 방향 유지
  - `PixelFollowCamera2D`, `VisualPixelSnap2D`, `PixelGrid2D`: 동일 논리 root 기준 카메라·표시 스냅
  - `YSortOrder2D`, `YSortSprite2D`: 발 접지점 기준 결정론적 깊이 정렬
- 이 컴포넌트의 구현과 시험값은 기술 기준이지 최종 공통 규격이나 최종 아트가 아니다.

### 차원과 무관하게 재사용 가능한 기존 상태 로직

- `PrototypeSessionState`
  - 모드 상태, 면역 경계도, 내부 미니게임 결과, 변이 선택과 복귀를 소유한다.
  - `Transform`, `Collider`, 카메라를 직접 참조하지 않으므로 2D 씬에서도 같은 상태 전이를 사용할 수 있다.
  - 다만 UI 문구와 진행 문자열까지 포함하고 있어 완전한 순수 Domain은 아니다. 1차 이관에서는 동작 회귀를 우선해 유지하고, 후속 정리 때 표시 문자열을 UI Presenter로 분리한다.
- `PrototypeConfig`, `PrototypeGameMode`, `InternalVirusMinigameType`, `ImmuneAlertEvent`
  - 공간 차원에 의존하지 않으므로 그대로 재사용한다.
- `ImmuneAlertModel`
  - 시간 기반 증가 계산을 지원하지만 현재 `PrototypeConfig.BaseAlertPerSecond=0`이므로 기본 자동 상승은 비활성이다.
  - 위험량·임계치·복귀값 계약을 그대로 재사용하고, 1차 이관도 위험 행동으로 경계도가 오르는 현재 설정을 유지한다.
- `VirusMinigameModel`, `ImmuneSignalSuppressionModel`, `VirusMinigameOutcome`
  - 수집 수량·안정도·성공/실패와 신호 억제 판정 모델은 2D 물리와 무관하므로 그대로 재사용한다.
- `MutationDefinition`, `MutationLoadout`, `MutationType`
  - 잠복 강화·신경 조종·포유류 적응의 상태와 수치 효과는 그대로 재사용한다.
- 기존 `RatHostPrototypeCoreTests.cs`의 상태·면역·미니게임·변이 테스트는 2D 이관 중에도 모두 유지해야 한다.

### 교체하거나 어댑터가 필요한 기존 구현

| 영역 | 기존 구현 | 판정 | 2D 경계 |
| --- | --- | --- | --- |
| Core | `PrototypeSessionController` | 교체 | `RatHostController`, `VirusMinigameController`, `PrototypeHud`를 직접 참조하고 모드 root를 3D 구성에 맞게 제어한다. 새 `RatHost2DSessionController`가 같은 `PrototypeSessionState`를 소유한다. |
| Host | `RatHostController` | 교체 | `CharacterController`, XZ 평면, 3D 카메라 상대 입력, Transform 회전에 결합되어 있다. `Rigidbody2D` 기반 새 Host controller가 필요하다. |
| Host | `RatHostControlModel`, `RatHostInstinctWanderModel` | 어댑터 재사용 | 논리는 재사용 가능하지만 XZ `Vector3` 계약이다. XY↔XZ 변환 어댑터와 정합 테스트를 두고, 활성 WASD 방향을 휘게 하지 않는 기존 계약을 유지한다. |
| Host | `RatDirectionalSpriteView`, `RatDirectionQuantizer`, `RatVisualPixelSnapper` | 교체 | 3D 카메라 축, XZ, Raycast 기반 접지에 결합되어 있다. 기술 샘플의 2D 표시·스냅·정렬 기반을 사용한다. |
| Host | `RatRiskInteractable`, `MammalAdaptationGate` | 교체 | `Collider`와 기존 세션 타입을 참조한다. `Collider2D` 기반 상호작용·게이트 컴포넌트를 만든다. |
| Immune | `ImmuneRiskZone` | 교체 | `Collider`, `RatHostController`, 3D bounds와 XZ 위험 방향에 결합되어 있다. 상태 호출은 유지하고 감지는 `Collider2D`/명시적 2D overlap으로 교체한다. |
| Virus | `VirusMinigameController`, `VirusPlayerController` | 교체 | 3D Transform, XZ bounds, 기존 세션 컨트롤러에 결합되어 있다. 모델은 재사용하고 이동·아레나·충돌은 2D로 새로 연결한다. |
| Virus | `MutationFragmentPickup`, `WhiteBloodCellChaser` | 교체 | 3D `Collider` 이벤트와 3D 위치 이동을 사용한다. `Collider2D` 이벤트와 `Rigidbody2D` 또는 결정론적 2D 이동으로 교체한다. |
| Mutations | `MutationLoadout` | 재사용 | 차원 독립 상태와 수치 효과다. |
| UI | `PrototypeHud`, `MutationOptionButton` | 교체 | 기존 `PrototypeSessionController`에 직접 결합되어 있다. 상태 읽기와 사용자 명령을 새 2D 세션 컨트롤러 인터페이스로 연결한다. |
| UI | `ImmuneSignalSuppressionHud` | 조건부 재사용 | RectTransform 표시 로직은 공간 차원과 무관하지만 상태·시험 레이아웃 결합을 다시 확인해야 한다. 2D HUD의 하위 View로만 조합하고 세션 소유권은 주지 않는다. |
| Camera | `PrototypeCameraController` | 교체 | 3D 모드별 카메라 위치·회전과 3D 타깃에 결합되어 있다. 숙주 모드는 검증된 `PixelFollowCamera2D` 계약을 유지하고 내부 모드는 별도 2D 카메라 상태로 전환한다. |

## 추천 씬 전략

### 선택안 비교

| 선택안 | 장점 | 위험 | 판정 |
| --- | --- | --- | --- |
| `RatHost2DTechnicalSample`를 계속 확장 | 초기 씬 복제 비용이 작다. | 기술 회귀 기준과 게임 상태·UI·미니게임이 섞여 이동·카메라 문제를 독립 재현하기 어려워진다. 시험 HUD와 시험 규격이 사실상 제품 규격으로 굳을 수 있다. | 비추천 |
| 별도 `RatHost2DPrototype` 씬 생성 | 기술 샘플을 고정 회귀 기준으로 보존하고 핵심 루프만 독립 통합할 수 있다. 3D와 2D를 나란히 비교할 수 있다. | 초기 빌더·씬 계약과 중복 연결 비용이 든다. | 추천 |
| 곧바로 Bootstrap + Additive 다중 씬 | 모드별 제작 경계와 로딩 확장성이 좋다. | 첫 수직 슬라이스에 씬 수명·상태 영속·중복 EventSystem/Camera 문제를 추가한다. | 현재 보류 |

### 추천 씬 구성

첫 전체 루프 수용까지는 한 씬 안에서 모드 root를 전환한다.

```text
RatHost2DPrototype
  Core2D
    RatHost2DSessionController
  RatHostMode2D
    World2D
    RatHost2D
    HostCamera2D
  VirusMode2D
    VirusArena2D
    VirusPlayer2D
    WhiteBloodCells2D
    Fragments2D
    VirusCamera2D
  UI2D
    HostHud2D
    VirusHud2D
    MutationSelection2D
    FailurePanel2D
  EventSystem
```

- 상태는 `Core2D` 하나만 소유한다.
- Host/Virus root는 상태에 따라 활성화하되 동일 프레임에 두 입력 컨트롤러가 함께 동작하지 않게 한다.
- UI는 상태를 읽고 세션 컨트롤러에 명령만 전달한다. UI가 상태를 직접 변경하지 않는다.
- 내부 모드 전환 시 Rigidbody2D 위치·속도·입력 캐시를 명시적으로 초기화한다.
- `Bootstrap`, additive 로딩, `DontDestroyOnLoad`는 한 씬 전체 루프에서 수명 문제가 실제로 확인된 뒤 별도 승인한다.

## 어셈블리·의존성 전략

### 1차 이관

```text
LastHost.Prototype.RatHost2D
  ├─ 참조: LastHost.Prototype
  │          └─ 기존 상태/면역/미니게임/변이 모델
  └─ 참조: LastHost.Prototype.TechnicalSample2D
             └─ 검증된 2D 순수 계산·카메라·정렬 기반
```

- 새 2D 씬 통합 코드는 별도 `Scripts/RatHost2D/` 어셈블리에 둔다.
- 새 어셈블리가 기존 상태 모델을 참조하되 기존 `LastHost.Prototype` 파일과 3D 씬을 수정하지 않는다.
- 기술 샘플 어셈블리도 수정하지 않는다. 새 2D 프로토타입에서 필요한 순수 계산 또는 안정된 표시 컴포넌트만 조합한다.
- `TechnicalSample2D`의 시험 HUD·Telemetry·씬 전용 상수에 제품 로직이 의존하지 않게 한다.
- 기존 입력 에셋의 `Host/Move`는 이름 조회로 재사용하고 에셋 자체를 재작성하지 않는다.

### 전체 루프 수용 이후 후보

- 기존 `LastHost.Prototype` 전체를 참조하는 결합이 장기화되기 전에 `Core/Immune/Mutations/VirusMinigame`의 순수 모델을 `LastHost.Prototype.Domain` 같은 공용 어셈블리로 추출할 수 있다.
- 이 작업은 파일 이동, asmdef 참조 변경, 기존 3D 회귀 위험이 있으므로 1차 이관과 동시에 하지 않는다.
- 추출 시 의존 방향은 다음으로 제한한다.

```text
Domain(Core state + models)
  ↑                 ↑
Legacy3D          RatHost2D
                      ↑
                     UI2D
```

- `Domain`은 Host/Virus의 `MonoBehaviour`, UI, 씬, 카메라를 참조하지 않는다.
- Host와 Virus gameplay는 Core 상태에 명령·결과만 전달한다.
- `Mutations`는 Host 구현을 직접 참조하지 않고 상태 수치/권한만 제공한다.
- UI는 Host/Immune/Virus 구현을 직접 탐색하지 않고 세션 상태 snapshot 또는 명시적 Presenter를 통해 읽는다.

## 단계별 이관안

### 1단계 — 숙주 탐험·면역 경계도·100% 전환 셸

포함:

- 별도 `RatHost2DPrototype` 씬
- 검증된 WASD·Rigidbody2D 충돌·카메라·Y 정렬
- 2D 쥐 controller와 기존 숙주 본능 모델의 XY↔XZ 어댑터
- `PrototypeSessionState`, `ImmuneAlertModel`, `MutationLoadout` 재사용
- 2D 위험 구역·위험 행동 1종 이상에 따른 면역 경계도 상승과 자연스러운 100% 도달
- 면역 경계도 100%에서 `InternalVirus` 상태와 비플레이 진단 셸로 전환
- 숙주 생명력·면역 경계도·현재 모드의 2D HUD

제외:

- 실제 바이러스 이동, 백혈구, 변이 조각
- 성공/실패 플레이 결과
- 변이 선택·효과 적용·복귀
- 신호 억제 미니게임 실제 플레이
- `BaseAlertPerSecond`를 0보다 크게 바꾸는 시간 경과 기본 상승 재활성화
- 최종 아트·최종 PPU·최종 HUD

핵심 회귀 보호:

- 활성 WASD는 기존 `RatHostControlModel`을 거친 뒤에도 입력 방향과 일치해야 한다.
- 무입력 숙주 본능 이동 중 물리 root·Visual·카메라가 같은 쥐를 기준으로 움직여야 한다.
- 방향 반전 시 순간이동, 누적 오프셋, 카메라 분리, 통·파이프 관통이 없어야 한다.
- 면역 경계도 임계 이벤트는 한 번만 발생하고 내부 root 활성화 후 Host 입력은 중지되어야 한다.

### 2단계 — 내부 바이러스 미니게임과 성공·실패

포함:

- 2D 바이러스 이동과 아레나 충돌
- `VirusMinigameModel` 재사용
- 2D 백혈구 1종, 변이 조각 수집, 안정도
- 성공 시 `MutationSelection`, 실패 시 `VirusFailed`
- 실패 UI 확인 후 보상 없이 쥐 모드 복귀
- 현재 상태 로직의 면역 포착 흔적과 복귀 경계도

조건부 포함:

- `ImmuneSignalSuppressionModel`과 신호 억제 UI는 기존 확장 기능의 2D 동등성 항목이다.
- 핵심 수직 슬라이스의 필수 조건은 백혈구 회피·조각 수집 1종이다. 신호 억제를 2단계에 함께 넣을지는 사용자 승인 항목으로 둔다.
- 신호 억제를 미룰 경우 2D 위험 원인은 `WhiteBloodCellEvasion`으로만 라우팅하고, 선택 불가능한 내부 타입으로 진입하는 경로가 없어야 한다.

핵심 회귀 보호:

- 모드 전환 직후 Host와 Virus 입력이 동시에 처리되지 않는다.
- 조각 수집과 백혈구 타격이 같은 프레임일 때 기존 모델의 성공 우선 규칙이 유지된다.
- 실패·재진입 때 조각, 안정도, 백혈구 위치와 속도 배수가 정확히 초기화된다.
- Collider2D 접촉이 한 번의 물리 스텝에서 중복 피해·중복 수집을 발생시키지 않는다.

### 3단계 — 변이 선택·효과·쥐 숙주 복귀

포함:

- 세 가지 변이 선택 UI
- `MutationLoadout` 재사용
- 잠복 강화의 면역 상승률 감소
- 신경 조종의 조종력·이동 속도 반영
- 포유류 적응의 2D 특정 통로 개방
- 성공 복귀와 실패 복귀의 면역 경계도 차이
- 변이 적용 상태로 쥐 숙주 모드 재개

제외:

- 두 번째 숙주와 정식 전이
- 영구 성장, 다중 숙주 계승
- 벌레 튜토리얼, 인간 단계, 백신, 엔딩

핵심 회귀 보호:

- 변이 선택 명령은 `MutationSelection` 모드에서 한 번만 적용된다.
- 복귀 시 쥐가 유효한 2D 스폰 위치에 있고 기존 Collider와 겹치지 않는다.
- 위험 구역 복귀 유예가 2D overlap에서도 적용되어 즉시 재전환 루프가 생기지 않는다.
- 포유류 적응 게이트의 시각 상태와 `Collider2D.enabled`가 같은 프레임에 일치한다.
- 잠복 강화와 신경 조종의 수치가 기존 Core 테스트와 같은 결과를 낸다.

## 단계별 승인·회귀 위험

| 위험 | 영향 | 차단 기준 |
| --- | --- | --- |
| 기술 샘플을 제품 씬으로 계속 증축 | 이동·카메라 회귀를 독립 재현할 기준 상실 | 기술 샘플 씬·전용 테스트 변경 0건을 기본으로 한다. |
| 기존 상태 모델과 새 2D 세션이 각각 상태를 소유 | HUD와 모드 root가 다른 상태를 표시 | 런타임 세션 상태 인스턴스는 정확히 1개여야 한다. |
| 기존 3D 세션 컨트롤러까지 활성 | Tick·입력·전환이 이중 실행 | 새 씬에는 `PrototypeSessionController`와 3D controller가 없어야 한다. |
| XY↔XZ 숙주 본능 변환 오류 | WASD 방향 굴절, 순간이동 재발 | 8방향·반전·idle 본능·충돌 중 root/카메라 정합 테스트를 둔다. |
| 시험값의 최종 규격 승격 | PPU·HUD·타일 제작이 조기에 고정 | `960×540`, `64×32`, PPU 64, 플레이스홀더는 계속 시험값으로 표기한다. |
| 한 번에 Domain 추출과 2D 구현 수행 | 실패 원인이 구조 변경인지 2D 통합인지 구분 불가 | 1차는 참조·어댑터, Domain 추출은 전체 루프 수용 후 별도 커밋으로 분리한다. |
| 내부 미니게임 타입 미구현 경로 진입 | 전환 후 플레이 불가 상태 | 단계별 허용 타입을 명시하고 미구현 타입 선택 경로를 차단한다. |
| 기존 3D 회귀 기준 조기 삭제 | 수치·루프 동등성 비교 불가 | 전체 2D 루프 사용자 수용 전 기존 씬·코드·테스트 삭제 0건. |

## 검증 기준

매 단계에서 다음 세 묶음을 모두 실행한다.

1. 기존 회귀
   - 기존 `RatHostPrototypeCoreTests`
   - 기존 `TechnicalSample2D` 전체 EditMode 테스트
   - 기존 `RatHostPrototype.unity`와 레거시 보호 경로의 해시·Git diff 대조
2. 새 2D 단계 테스트
   - 상태 전이와 모델 결과를 검증하는 EditMode 테스트
   - 새 씬 root·컴포넌트·2D 물리·HUD 계약 테스트
   - 동일 프레임 중복 명령, 재진입 초기화, 입력 root 단일 소유권 테스트
3. 플레이 검증
   - Unity MCP Play에서 대상 단계의 실제 Input Action 경로, 모드/HUD, Collider2D, Console 확인
   - Windows 빌드 성공과 Windows 실행본 플레이 검증을 분리 기록
   - 사용자 확인 전 기술 시험값·플레이스홀더를 최종 규격이나 최종 아트로 선언하지 않음

## 기존 3D·기술 샘플 보존 시점

- 1~3단계 동안 `RatHostPrototype.unity`, 기존 3D 코드·씬 빌더·테스트는 비교·회귀 기준으로 유지한다.
- `RatHost2DTechnicalSample.unity`와 전용 테스트는 2D 이동 기반 회귀 기준으로 유지한다.
- 3단계까지 자동 QA, Unity MCP Play, Windows 빌드/실행 검증, 사용자 핵심 루프 수용을 모두 받은 뒤에만 2D 씬을 기본 프로토타입 후보로 승격한다.
- 승격 이후에도 기존 3D/2.5D 산출물 삭제는 자동으로 허용되지 않는다. 레거시 폴더 이동, Build Settings 기본 씬 변경, Domain 어셈블리 추출은 각각 별도 승인·검증 작업으로 둔다.

## 사용자 승인 필요 항목

1. 씬 전략: 기술 샘플 확장이 아니라 별도 `RatHost2DPrototype` 씬을 생성한다.
2. 1차 구현 범위: 숙주 탐험·숙주 본능·면역 경계도·100% 내부 전환 셸까지만 먼저 구현한다.
3. 상태 재사용 원칙: 기존 `PrototypeSessionState`와 차원 독립 모델은 참조 재사용하고, 3D 결합 `MonoBehaviour`만 2D로 교체한다.
4. 리팩터링 시점: 공용 Domain 어셈블리 추출은 전체 2D 루프 수용 이후로 미룬다.
5. 2단계 범위: 신호 억제 미니게임까지 동시에 2D 이관할지, 필수 백혈구 회피·조각 수집만 먼저 이관할지 결정한다.
6. 보존 원칙: 기존 3D 씬과 2D 기술 샘플은 전체 2D 루프 수용 전까지 수정·삭제하지 않는다.
7. 시간 경과 기본 상승: 현재 `BaseAlertPerSecond=0`을 유지한다. 자동 상승을 다시 켜는 변경은 위험 행동 중심 100% 도달 QA와 별개인 사용자 승인 항목으로 둔다.

## 최종 추천

승인 브리프의 기본안은 `별도 RatHost2DPrototype 씬 + 기존 상태 모델 재사용 + 2D 어댑터 + 3단계 수직 이관`으로 잡는 것이 가장 안전하다. 첫 구현은 상태 모델 이동이나 대규모 어셈블리 정리보다 `숙주/면역/전환` 한 단계의 동등성 검증에 집중한다. 이 구조라면 이미 해결한 이동·카메라·충돌 회귀를 기술 샘플에서 계속 보호하면서, 기존 3D 핵심 루프와 새 2D 표현 계층의 차이를 단계별로 분리해서 확인할 수 있다.
