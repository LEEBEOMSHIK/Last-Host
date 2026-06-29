# 쥐 숙주 핵심 루프 구현 계획 초안

작성일: 2026-06-29
작성 주체: 게임플레이 루프 에이전트 산출물 초안
작업 ID: `2026-06-29-rat-host-plan-agent`

## 문서 상태

이 문서는 공식 `docs/` 문서가 아니라 active 작업 산출물 초안이다. 조정자 검토와 사용자 승인 전에는 Unity 씬, C# 코드, 에셋, ProjectSettings 변경을 시작하지 않는다.
공식 `docs/` 승격 전 상태를 유지하며, 이 문서의 내용은 승인 질문과 후속 구현 작업 패킷을 만들기 위한 근거로만 사용한다.

## 목표

`쥐 숙주 프로토타입`의 최소 완성 루프를 Unity 3D에서 구현할 수 있도록 기능 단위, 파일 책임, 수용 기준, 검증 시나리오를 정의한다.

최소 완성 루프:

1. 작은 하수도 맵에서 쥐 숙주를 조종한다.
2. 시간 경과 또는 위험 행동으로 면역 경계도가 상승한다.
3. 면역 경계도 100%에서 내부 바이러스 미니게임으로 전환한다.
4. 바이러스를 조종해 백혈구를 피하고 변이 조각을 수집한다.
5. 변이 보상 1개를 선택한다.
6. 선택한 변이가 적용된 상태로 쥐 숙주 플레이에 복귀한다.

## 전제와 차단 조건

### 계획 전제

이 초안은 `docs/rat-host-approval-packet.md`의 추천안을 기준으로 한다.

- Unity 버전: `6000.4.6f1`
- 렌더 파이프라인: URP
- 플랫폼: PC 우선
- 시작 범위: 벌레 튜토리얼 없이 쥐 숙주부터 시작
- 기획 위치: 쥐는 벌레보다 상위 숙주지만, 첫 프로토타입에서는 정식 진행 순서가 아니라 핵심 루프 검증용 수직 슬라이스로 사용
- 조작: `WASD` 이동, `Space` 상호작용
- 카메라: 고정 쿼터뷰
- 미니게임 성공 조건: 바이러스 안정도 0 이전에 변이 조각 3개 수집
- 미니게임 실패 조건: 실패 화면과 재시도
- 에셋: 기본 도형, 단순 저폴리 모델, 단색/픽셀풍 임시 머티리얼
- 입력 권장안: Unity Input System 사용
- 새 입력 액션 자산 후보: `Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`
- 기존 템플릿 입력 자산 `Assets/InputSystem_Actions.inputactions`는 바로 수정하지 않음
- `RatHostPrototype.unity` 생성 후 사용자 승인에 따라 Build Settings 시작 씬으로 등록
- `Assets/Scenes/SampleScene.unity`는 삭제하지 않고, 프로토타입 빌드 대상에서는 제외하거나 뒤로 미룸

### 구현 전 차단 조건

- `docs/rat-host-approval-packet.md` 전체 승인 또는 수정 승인이 필요하다.
- Unity MCP를 통한 씬, 에셋, 패키지, 코드, ProjectSettings 변경 승인이 필요하다.
- Input System 사용 여부와 새 입력 액션 자산 생성 위치 승인이 필요하다.
- `RatHostPrototype.unity` 생성, `Assets/_Project/` 하위 폴더 생성, Build Settings 변경 승인이 필요하다.
- `SampleScene.unity`를 빌드 대상에서 제외하거나 뒤로 미룰지 승인이 필요하다.
- 실패 후 재시도 시 초기화할 값과 유지할 값 승인이 필요하다.
- 구현 후 테스트와 빌드 실행 범위 승인이 필요하다.
- 이 산출물을 공식 구현 계획으로 승격할지 사용자 확인이 필요하다.

## 범위

### 포함 범위

- 하수도 쥐 숙주 모드
- 쥐 이동과 방향 전환
- 관심 지점 상호작용 1종
- 면역 경계도 시간 상승
- 위험 요소 접촉 시 면역 경계도 추가 상승
- 면역 경계도 100% 모드 전환
- 내부 바이러스 아레나
- 바이러스 아바타 이동
- 백혈구 1종
- 변이 조각 3개 수집
- 바이러스 안정도
- 변이 3종 선택
- 선택 변이 효과 적용 후 쥐 모드 복귀
- 실패 화면과 재시도
- 최소 HUD

### 제외 범위

- 벌레 튜토리얼
- 다중 숙주 전이 체인
- 인간 숙주 플레이
- 병원, 연구소, 백신 시스템
- 엔딩
- 완성형 로그라이크 런 구조
- 영구 성장 시스템
- 최종 아트 에셋

## Unity 구조 제안

초기에는 씬을 과하게 나누지 않는다. `RatHostPrototype` 하나의 씬 안에서 쥐 숙주 루트, 바이러스 미니게임 루트, UI 루트를 전환한다.

```text
UnityProject/Assets/_Project/
  Scenes/
    RatHostPrototype.unity
  Scripts/
    Core/
      PrototypeMode.cs
      PrototypeGameState.cs
      ModeTransitionController.cs
    Host/
      RatHostController.cs
      HostHealth.cs
      HostInteractionProbe.cs
      HostInterestPoint.cs
    Immune/
      ImmuneAlertMeter.cs
      ImmuneAlertSource.cs
    VirusMinigame/
      VirusAvatarController.cs
      WhiteBloodCellController.cs
      MutationShard.cs
      VirusMinigameController.cs
    Mutations/
      MutationType.cs
      MutationChoice.cs
      MutationSelectionController.cs
      MutationEffectApplier.cs
    UI/
      PrototypeHudController.cs
      MutationChoiceView.cs
      RetryPanelView.cs
  Prefabs/
    Host/
    VirusMinigame/
    UI/
  Materials/
    Placeholder/
  Settings/
    Input/
      RatHostPrototypeControls.inputactions
```

### 입력 구조 제안

- 1차 프로토타입도 Unity Input System 사용을 권장한다.
- 새 입력 액션 자산은 `Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`에 둔다.
- 액션 맵 후보는 `Host`, `VirusMinigame`, `UI`로 나눈다.
- 액션 후보는 `Move`, `Interact`, `Confirm`, `Retry`로 시작한다.
- 기존 템플릿 입력 자산 `Assets/InputSystem_Actions.inputactions`는 수정하지 않는다.
- Player Settings의 Active Input Handling 변경이 필요하면 ProjectSettings 변경이므로 별도 승인 후 처리한다.
- 1차 범위에서는 리바인딩 UI, 게임패드 프로필, 모바일 터치 입력은 제외한다.

## 씬 구성 제안

`RatHostPrototype.unity` 루트:

```text
RatHostPrototype
  Core
    PrototypeGameState
    ModeTransitionController
  RatHostMode
    Rat
    SewerBlockout
    ImmuneAlertSources
    HostInterestPoints
  VirusMinigameMode
    VirusArena
    VirusAvatar
    WhiteBloodCells
    MutationShards
  UI
    PrototypeHUD
    MutationChoicePanel
    RetryPanel
  Cameras
    RatHostCamera
    VirusMinigameCamera
  Lighting
```

초기 전환 방식:

- `RatHostMode` 활성, `VirusMinigameMode` 비활성으로 시작한다.
- 면역 경계도 100% 도달 시 `RatHostMode`를 비활성화하고 `VirusMinigameMode`를 활성화한다.
- 미니게임 성공 시 `MutationChoicePanel`을 표시한다.
- 변이 선택 후 `VirusMinigameMode`를 비활성화하고 `RatHostMode`를 활성화한다.
- 미니게임 실패 시 `RetryPanel`을 표시한다.

## 기능 단위 계획

### 1. 승인 상태 고정

목적:

- 구현 전에 결정되지 않은 항목을 차단 조건으로 분리한다.

입력:

- `docs/rat-host-approval-packet.md`
- `docs/project-prep.md`
- `docs/unity-baseline-report.md`

처리:

- 승인 패킷 항목 1~10의 승인 여부를 확인한다.
- 수정 승인 항목이 있으면 구현 계획 초안에 반영한다.
- 미승인 항목은 구현 차단 조건으로 남긴다.

출력:

- 승인된 전제 목록
- 열린 질문 목록

UI 반응:

- 없음

수용 기준:

- Unity 버전, URP, 플랫폼, 시작 범위, 조작, 성공/실패 조건, UI 게이지, 변이 효과, 플레이스홀더 범위가 명확하다.
- 미승인 상태면 Unity 변경 작업을 시작하지 않는다.

검증 시나리오:

- `docs/rat-host-approval-packet.md`의 승인 응답을 확인한다.
- `git status --short`로 작업 전 상태를 확인한다.

### 2. 프로토타입 씬과 폴더 구조

목적:

- 게임 전용 폴더와 단일 프로토타입 씬 기준을 만든다.

파일 후보:

- 생성: `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- 생성: `UnityProject/Assets/_Project/Scripts/Core/`
- 생성: `UnityProject/Assets/_Project/Scripts/Host/`
- 생성: `UnityProject/Assets/_Project/Scripts/Immune/`
- 생성: `UnityProject/Assets/_Project/Scripts/VirusMinigame/`
- 생성: `UnityProject/Assets/_Project/Scripts/Mutations/`
- 생성: `UnityProject/Assets/_Project/Scripts/UI/`
- 생성: `UnityProject/Assets/_Project/Prefabs/`
- 생성: `UnityProject/Assets/_Project/Materials/Placeholder/`
- 생성: `UnityProject/Assets/_Project/Settings/Input/`
- 생성 후보: `UnityProject/Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`

처리:

- 기존 `Assets/Scenes/SampleScene.unity`는 삭제하지 않는다.
- `SampleScene.unity`는 템플릿 기본 씬 또는 비교용 잔여 자산으로 보관한다.
- 새 씬 `RatHostPrototype`에 루트 오브젝트 그룹만 배치한다.
- Input System 승인이 있으면 기존 템플릿 입력 자산 대신 게임 전용 입력 액션 자산을 새 위치에 만든다.
- `RatHostPrototype.unity` 생성과 기본 루트 확인 후, 사용자 승인이 있으면 Build Settings에 시작 씬으로 등록한다.
- 프로토타입 빌드에서는 `SampleScene.unity`를 빌드 대상에서 제외하거나 `RatHostPrototype.unity` 뒤로 미룬다.

출력:

- 게임 전용 폴더 구조
- 단일 프로토타입 씬
- 게임 전용 입력 액션 자산 후보
- 승인된 경우 프로토타입 시작 씬 Build Settings 정책

UI 반응:

- 없음

수용 기준:

- `Assets/_Project/` 아래에 프로젝트 전용 구조가 있다.
- `Assets/_Project/Settings/Input/` 구조가 있다.
- `RatHostPrototype.unity`가 생성되어 있다.
- 씬에는 Core, RatHostMode, VirusMinigameMode, UI, Cameras, Lighting 루트가 있다.
- Input System 사용이 승인된 경우 `RatHostPrototypeControls.inputactions`가 게임 전용 입력 자산으로 존재한다.
- Build Settings 변경이 승인된 경우 `RatHostPrototype.unity`가 프로토타입 시작 씬이고 `SampleScene.unity`가 시작 씬이 아니다.

검증 시나리오:

- Unity MCP `Unity_ListResources(Under=Assets/_Project)`로 리소스 존재를 확인한다.
- Unity MCP `Unity_ManageScene(Action=GetActive)`로 활성 씬 정보를 확인한다.
- 승인 후 Build Settings에서 시작 씬이 `RatHostPrototype.unity`인지 확인한다.
- `git diff --check`를 실행한다.

보조 검토 반영:

- `RatHostPrototype` 단일 씬 시작은 적합하다.
- `Assets/_Project/Settings/Input/` 구조를 추가한다.
- Input System 사용을 권장하되 사용자 승인 전 입력 액션 자산은 만들지 않는다.
- `RatHostPrototype.unity`는 생성 후 Build Settings 시작 씬 등록을 권장한다.
- `SampleScene.unity`는 삭제하지 않고 빌드 대상에서 제외하거나 뒤로 미룬다.

### 3. 공용 상태와 모드 전환

목적:

- 쥐 모드, 바이러스 미니게임, 변이 선택, 재시도 흐름을 중앙에서 관리한다.

파일 후보:

- 생성: `UnityProject/Assets/_Project/Scripts/Core/PrototypeMode.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/Core/PrototypeGameState.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/Core/ModeTransitionController.cs`

입력:

- 면역 경계도 100% 이벤트
- 미니게임 성공 이벤트
- 미니게임 실패 이벤트
- 변이 선택 이벤트
- 재시도 입력

처리:

- 현재 모드를 `RatHost`, `VirusMinigame`, `MutationSelection`, `Retry` 중 하나로 보관한다.
- 모드 변경 시 관련 루트 오브젝트와 UI 패널의 활성 상태를 바꾼다.
- 쥐 모드 복귀 시 미니게임 상태를 초기화한다.
- 실패 후 재시도 시에는 쥐 모드 전체를 되돌리지 않고 미니게임 상태만 초기화한다.

실패 후 재시도 초기화 기준 초안:

| 구분 | 값 |
| --- | --- |
| 초기화 | 바이러스 위치, 바이러스 안정도, 변이 조각 수, 변이 조각 활성 상태, 백혈구 위치와 내부 상태, 미니게임 타이머 또는 진행 상태, 실패 패널 표시 상태 |
| 유지 | 이미 선택된 변이, 쥐 숙주 위치, 숙주 생명력, 쥐 모드 맵 상호작용 상태, 이번 미니게임 진입 전의 공용 게임 상태 |
| 입력 | 재시도 직후 `VirusMinigame` 입력만 활성화하고 `Host`, `MutationSelection`, `Retry` 입력은 비활성화 |
| 면역 경계도 | 실패 재시도 중에는 100% 도달 상태로 잠근다. 미니게임 성공 후 변이 선택을 마치고 쥐 모드로 복귀할 때 0 또는 승인된 복귀값으로 낮춘다. |
| 동시 성공/실패 | 같은 프레임에 조각 3개 수집과 안정도 0이 동시에 발생하면 성공 판정을 우선하는 초안을 둔다. 최종 확정은 사용자 승인 대상이다. |

출력:

- 현재 모드
- 활성 루트 오브젝트
- UI 표시 상태

UI 반응:

- 현재 모드 텍스트 갱신
- 변이 선택 패널 표시/숨김
- 재시도 패널 표시/숨김

수용 기준:

- 면역 100%에서 쥐 모드가 멈추고 바이러스 미니게임이 시작된다.
- 미니게임 성공 후 변이 선택 화면이 표시된다.
- 변이 선택 후 쥐 모드로 복귀한다.
- 미니게임 실패 후 재시도 패널이 표시된다.
- 재시도 시 미니게임 값은 초기화되고, 이전에 선택된 변이나 쥐 모드 진행 상태는 의도 없이 사라지지 않는다.

검증 시나리오:

- 디버그 트리거 또는 테스트용 버튼으로 각 모드 전환을 순서대로 실행한다.
- 전환 후 잘못된 모드 루트가 동시에 활성화되지 않는지 확인한다.
- 전환 중 HUD가 이전 모드 값을 계속 표시하지 않는지 확인한다.
- 실패 후 재시도 입력을 실행하고 바이러스 안정도, 조각 수, 백혈구 위치, 조각 활성 상태가 초기값으로 돌아왔는지 확인한다.

QA 반영 기준:

- 모드 전환 중 입력이 중복 처리되는지
- 실패 후 재시도 시 면역 경계도와 미니게임 상태 초기화 기준
- 성공과 실패가 같은 프레임에 발생할 때 우선순위

### 4. 쥐 숙주 이동과 상호작용

목적:

- 쥐 조종이 숙주 모드의 기본 플레이로 작동하는지 확인한다.

파일 후보:

- 생성: `UnityProject/Assets/_Project/Scripts/Host/RatHostController.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/Host/HostHealth.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/Host/HostInteractionProbe.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/Host/HostInterestPoint.cs`

입력:

- Input System `Host/Move`: `WASD` 이동
- Input System `Host/Interact`: `Space` 상호작용
- 변이 효과: 신경 조종

처리:

- 카메라 기준 또는 월드 기준으로 쥐를 이동시킨다.
- 이동 방향에 맞춰 쥐의 바라보는 방향을 바꾼다.
- 상호작용 범위 안의 관심 지점을 찾는다.
- 신경 조종 변이가 있으면 이동 속도 또는 상호작용 속도를 높인다.

출력:

- 쥐 위치와 회전
- 상호작용 성공 이벤트
- 숙주 생명력 값

UI 반응:

- 숙주 생명력 표시
- 상호작용 가능한 지점에 간단한 표시
- 신경 조종 적용 시 현재 변이 표시

수용 기준:

- `WASD`로 쥐가 이동한다.
- 쥐가 이동 방향을 향한다.
- `Space`로 관심 지점 1종과 상호작용한다.
- 신경 조종 선택 후 이동 또는 상호작용 변화가 체감된다.

검증 시나리오:

- 30초 이상 이동하면서 벽이나 장애물과의 기본 충돌을 확인한다.
- 관심 지점 앞에서 `Space` 입력 전후 반응을 확인한다.
- 신경 조종 적용 전후 동일 구간 이동 시간을 비교한다.

범위 주의:

- 정교한 AI, 은신, 소리 시스템은 1차 범위가 아니다.

### 5. 면역 경계도

목적:

- 플레이어가 면역 압박 상승과 모드 전환 이유를 이해하게 만든다.

파일 후보:

- 생성: `UnityProject/Assets/_Project/Scripts/Immune/ImmuneAlertMeter.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/Immune/ImmuneAlertSource.cs`
- 수정 후보: `ModeTransitionController.cs`
- 수정 후보: `PrototypeHudController.cs`

입력:

- 시간 경과
- 위험 요소 접촉
- 변이 효과: 잠복 강화

처리:

- 쥐 모드에서 초당 일정량 면역 경계도를 올린다.
- 위험 요소 접촉 중에는 추가 상승량을 더한다.
- 잠복 강화 변이가 있으면 상승량을 낮춘다.
- 100% 도달 시 모드 전환 이벤트를 발행한다.

출력:

- 현재 면역 경계도 0~100
- 100% 도달 이벤트

UI 반응:

- 면역 경계도 게이지 증가
- 위험 요소 접촉 시 경고 색상 또는 짧은 피드백
- 100% 직전 경고 표시

수용 기준:

- 시간 경과만으로도 면역 경계도가 오른다.
- 위험 요소 접촉 시 상승 속도가 증가한다.
- 100% 도달 시 바이러스 미니게임으로 전환된다.
- 잠복 강화 선택 후 상승 속도가 감소한다.

검증 시나리오:

- 위험 요소 접촉 없이 30초 동안 상승량을 기록한다.
- 위험 요소 접촉 상태에서 10초 동안 상승량을 기록한다.
- 잠복 강화 적용 후 같은 조건에서 상승량이 줄었는지 확인한다.

QA 검토 필요:

- 상승 속도 수치가 너무 빨라 루프를 방해하지 않는지
- 100% 도달 직후 중복 전환 이벤트가 발생하지 않는지

### 6. 내부 바이러스 미니게임

목적:

- 숙주 외부 탐험에서 내부 면역 대응으로 전환되는 긴장감을 검증한다.

파일 후보:

- 생성: `UnityProject/Assets/_Project/Scripts/VirusMinigame/VirusAvatarController.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/VirusMinigame/WhiteBloodCellController.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/VirusMinigame/MutationShard.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/VirusMinigame/VirusMinigameController.cs`

입력:

- Input System `VirusMinigame/Move`: `WASD` 바이러스 이동
- 백혈구 접촉
- 변이 조각 접촉

처리:

- 바이러스 아바타를 작은 아레나 안에서 이동시킨다.
- 백혈구 1종은 단순 추적 또는 순찰을 수행한다.
- 백혈구 접촉 시 바이러스 안정도를 낮춘다.
- 변이 조각 수집 시 수집 수를 증가시키고 조각을 숨긴다.
- 조각 3개 수집 시 성공 이벤트를 발행한다.
- 안정도 0 도달 시 실패 이벤트를 발행한다.

출력:

- 바이러스 안정도
- 변이 조각 수
- 성공 또는 실패 이벤트

UI 반응:

- 바이러스 안정도 표시
- 변이 조각 수 `0/3` 표시
- 성공 시 변이 선택 화면 표시
- 실패 시 재시도 패널 표시

수용 기준:

- 바이러스가 아레나 안에서 움직인다.
- 백혈구와 닿으면 안정도가 감소한다.
- 변이 조각 3개를 모으면 성공한다.
- 안정도 0이면 실패한다.

검증 시나리오:

- 백혈구를 피해서 조각 3개를 수집한다.
- 백혈구에 일부러 접촉해 안정도 감소를 확인한다.
- 안정도 0 실패 흐름을 확인한다.

범위 주의:

- 여러 백혈구 타입, 항체 패턴, 복잡한 탄막은 제외한다.

### 7. 변이 선택과 적용

목적:

- 미니게임 성공 보상이 쥐 모드 플레이 변화로 이어지는지 검증한다.

파일 후보:

- 생성: `UnityProject/Assets/_Project/Scripts/Mutations/MutationType.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/Mutations/MutationChoice.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/Mutations/MutationSelectionController.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/Mutations/MutationEffectApplier.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/UI/MutationChoiceView.cs`

입력:

- 미니게임 성공 이벤트
- 플레이어 선택 입력

처리:

- 세 가지 변이 선택지를 표시한다.
- 선택된 변이를 공용 상태에 저장한다.
- 선택 변이에 맞는 효과를 쥐 모드 시스템에 반영한다.

출력:

- 선택된 변이
- 쥐 모드에 적용된 효과

UI 반응:

- 변이 이름과 효과 설명 표시
- 선택 후 현재 변이 표시

변이별 1차 효과:

| 변이 | 효과 |
| --- | --- |
| 잠복 강화 | 면역 경계도 상승 속도 감소 |
| 신경 조종 | 쥐 이동 속도 또는 상호작용 속도 증가 |
| 포유류 적응 | 하수도 맵의 특정 통로 접근 또는 보상 지점 접근 허용 |

수용 기준:

- 미니게임 성공 후 세 가지 선택지가 보인다.
- 하나를 선택하면 쥐 모드로 복귀한다.
- 선택한 변이 효과가 쥐 모드에서 확인된다.
- 같은 성공 이벤트에서 중복 선택이 발생하지 않는다.

검증 시나리오:

- 잠복 강화를 선택하고 면역 상승 속도 감소를 확인한다.
- 신경 조종을 선택하고 이동 또는 상호작용 변화 확인한다.
- 포유류 적응을 선택하고 접근 제한 통로 또는 보상 지점 접근을 확인한다.

사용자 승인 필요:

- 포유류 적응의 임시 효과가 “특정 통로 접근”인지 “보상 지점 접근”인지 최종 선택이 필요하다.

### 8. HUD와 실패/재시도 UI

목적:

- 플레이어가 현재 상태와 목표를 별도 설명 없이 이해하게 한다.

파일 후보:

- 생성: `UnityProject/Assets/_Project/Scripts/UI/PrototypeHudController.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/UI/RetryPanelView.cs`
- 수정 후보: `MutationChoiceView.cs`

입력:

- 숙주 생명력
- 면역 경계도
- 현재 모드
- 바이러스 안정도
- 변이 조각 수
- 선택된 변이
- 실패 이벤트
- Input System `UI/Retry`: 재시도 입력
- Input System `UI/Confirm`: 변이 선택 또는 확인 입력

처리:

- 현재 모드에 필요한 HUD만 표시한다.
- 쥐 모드에서는 숙주 생명력, 면역 경계도, 현재 변이를 표시한다.
- 바이러스 모드에서는 바이러스 안정도와 변이 조각 수를 표시한다.
- 실패 시 재시도 패널을 표시한다.
- 실패 패널이 열린 동안 쥐 모드와 변이 선택 입력은 받지 않는다.

출력:

- 모드별 HUD 상태
- 실패/재시도 UI 상태

수용 기준:

- 쥐 모드와 바이러스 모드의 HUD가 구분된다.
- 변이 조각 목표가 `0/3`, `1/3`, `2/3`, `3/3`처럼 읽힌다.
- 실패 시 재시도 버튼 또는 입력 안내가 보인다.
- UI 요소가 16:9 데스크톱 화면에서 서로 겹치지 않는다.
- 실패 패널이 열린 동안 뒤쪽 플레이 오브젝트가 입력을 받지 않는다.

검증 시나리오:

- 쥐 모드, 바이러스 모드, 변이 선택, 실패 화면을 각각 확인한다.
- 긴 텍스트가 버튼이나 패널 밖으로 넘치지 않는지 확인한다.
- 실패 패널에서 재시도를 실행한 뒤 패널이 닫히고 미니게임 HUD가 초기값으로 갱신되는지 확인한다.

### 9. 플레이스홀더 하수도 맵과 위험 요소

목적:

- 최소 공간 안에서 이동, 위험, 변이 효과를 검증한다.

파일 후보:

- 수정: `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- 생성 후보: `UnityProject/Assets/_Project/Materials/Placeholder/SewerFloor.mat`
- 생성 후보: `UnityProject/Assets/_Project/Materials/Placeholder/ToxicWater.mat`
- 생성 후보: `UnityProject/Assets/_Project/Materials/Placeholder/BlockedPath.mat`

구성:

- 작은 직사각형 하수도 공간
- 쥐 시작 위치
- 독성 물웅덩이 1개
- 관심 지점 1개
- 포유류 적응으로 열리는 통로 또는 보상 지점 1개

수용 기준:

- 쥐가 이동할 수 있는 좁은 하수도 맵이 있다.
- 위험 요소가 면역 경계도 상승 이유로 보인다.
- 포유류 적응 효과를 확인할 지점이 있다.
- 최종 아트처럼 보이게 꾸미지 않고 플레이스홀더임이 명확하다.

검증 시나리오:

- 맵을 한 바퀴 이동해 막히는 지점과 접근 가능한 지점을 확인한다.
- 위험 요소 접촉 전후 면역 경계도 상승을 확인한다.
- 포유류 적응 선택 전후 접근 가능 여부를 확인한다.

## 통합 검증 계획

### 최소 성공 루프

1. `RatHostPrototype` 씬을 연다.
2. 쥐를 `WASD`로 이동한다.
3. 위험 요소와 접촉해 면역 경계도 상승을 확인한다.
4. 면역 경계도 100%에서 바이러스 미니게임으로 전환되는지 확인한다.
5. 바이러스로 백혈구를 피하며 변이 조각 3개를 수집한다.
6. 변이 선택 화면에서 변이 1개를 선택한다.
7. 쥐 모드로 복귀한다.
8. 선택한 변이가 쥐 모드에 반영되었는지 확인한다.

### 실패 루프

1. 내부 바이러스 미니게임에 진입한다.
2. 백혈구와 반복 접촉해 바이러스 안정도를 0으로 만든다.
3. 실패 패널이 표시되는지 확인한다.
4. 재시도 입력으로 미니게임을 다시 시작할 수 있는지 확인한다.

### 변이별 확인

- 잠복 강화: 같은 조건에서 면역 경계도 상승 속도가 느려진다.
- 신경 조종: 이동 또는 상호작용 속도가 증가한다.
- 포유류 적응: 특정 통로 또는 보상 지점 접근이 가능해진다.

### UI와 카메라 확인 기준

확인 해상도:

- `1920x1080`: 기본 검증 해상도
- `1600x900`: 중간 창 크기
- `1366x768`: 낮은 노트북 해상도
- `1280x720`: 최소 16:9 기준

UI 통과 기준:

- 숙주 생명력, 면역 경계도, 현재 변이 표시가 서로 겹치지 않는다.
- 바이러스 안정도와 변이 조각 수가 서로 겹치지 않는다.
- 변이 선택 화면의 세 선택지가 모두 선택 가능하다.
- 한국어 변이 이름과 효과 설명이 버튼 또는 패널 밖으로 넘치지 않는다.
- 실패/재시도 패널이 HUD와 의미 없이 겹치지 않는다.
- 모드 전환 시 이전 모드 HUD가 남아 새 모드 HUD와 겹치지 않는다.

카메라 통과 기준:

- 쥐 모드에서는 주요 경로, 위험 요소, 관심 지점, 포유류 적응 접근 지점이 식별 가능하다.
- 쥐가 기본 이동 중 화면 중심 또는 의도한 추적 위치에서 벗어나지 않는다.
- 바이러스 모드에서는 아레나 경계, 바이러스, 백혈구, 변이 조각이 동시에 식별 가능하다.
- UI 텍스트는 저해상도 렌더링 또는 후처리 때문에 흐려지지 않는다.

### QA 완료 조건

내부 구현 완료 조건:

- 사용자 승인된 범위 안에서만 Unity 씬, 코드, 에셋, ProjectSettings가 변경되어 있다.
- Unity 에디터 컴파일 오류가 없다.
- 신규 또는 수정된 EditMode 테스트가 통과한다.
- 최소 PlayMode 테스트가 통과하거나, PlayMode 자동화 제외 사유가 기록되어 있다.
- 수동 플레이 체크리스트의 성공 루프와 실패 루프가 통과한다.
- 세 가지 변이 효과가 각각 한 번 이상 확인되어 있다.
- 콘솔에 루프 진행을 막는 예외가 없다.
- 실행하지 않은 빌드나 테스트를 통과로 기록하지 않는다.

플레이어블 프로토타입 완료 조건:

- 내부 구현 완료 조건을 모두 만족한다.
- Windows PC 대상 빌드가 성공한다.
- 빌드 실행본에서 성공 루프 1회와 실패 루프 1회를 확인한다.
- 빌드 실행본에서 UI 겹침과 카메라 프레이밍 기준을 통과한다.

## 커밋 단위 제안

구현 승인 후 작업할 경우 커밋을 다음처럼 나눈다.

1. `chore: add rat host prototype scene structure`
2. `feat: add prototype mode flow`
3. `feat: add rat host movement`
4. `feat: add immune alert meter`
5. `feat: add virus minigame loop`
6. `feat: add mutation selection`
7. `feat: add prototype hud and retry flow`
8. `test: document rat host prototype verification`

## Unity 아키텍처 보조 검토 반영 결과

- `RatHostPrototype` 단일 씬으로 시작한다.
- `Assets/_Project/` 아래에 게임 전용 구조를 둔다.
- `Assets/_Project/Settings/Input/` 구조를 추가한다.
- 1차 프로토타입도 Input System 사용을 권장한다.
- 새 입력 액션 자산 후보는 `Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`다.
- 기존 `Assets/InputSystem_Actions.inputactions`는 바로 수정하지 않는다.
- `RatHostPrototype.unity` 생성 후 Build Settings 시작 씬 등록을 권장한다.
- `SampleScene.unity`는 삭제하지 않고 빌드 대상에서 제외하거나 뒤로 미룬다.
- 모든 Unity 씬, 입력 액션 자산, Build Settings, ProjectSettings 변경은 사용자 승인 전에는 수행하지 않는다.

## QA/검증 보조 검토 반영 결과

- 구현 완료 조건은 내부 구현 완료와 플레이어블 프로토타입 완료로 나눈다.
- 실패 후 재시도 초기화 기준 초안을 계획에 포함한다.
- `포유류 적응` 임시 효과는 구현 전 사용자 승인이 필요하다.
- Input System 사용 여부와 Build Settings 씬 정책은 구현 전 사용자 승인이 필요하다.
- UI 확인 기준은 16:9 PC 해상도 `1920x1080`, `1600x900`, `1366x768`, `1280x720`로 둔다.
- 카메라 프레이밍은 쥐 모드와 바이러스 모드를 별도로 확인한다.

## 범위 초과 감지 기준

다음 요청이 들어오면 구현 계획을 확장하지 말고 별도 승인 질문으로 분리한다.

- 벌레 튜토리얼 추가
- 두 번째 숙주 추가
- 인간 숙주, 병원, 연구소, 백신 시스템 추가
- 엔딩 또는 장기 성장 시스템 추가
- 최종 모델, 애니메이션, AI 생성 아트 에셋 제작
- 복잡한 백혈구 패턴이나 다중 적 타입 추가

## 사용자 승인 필요

구현을 시작하려면 다음 항목의 승인 또는 수정 승인이 필요하다.

1. `docs/rat-host-approval-packet.md` 추천안 전체 승인 또는 수정 승인
2. Input System 사용 여부
3. 새 입력 액션 자산 생성 위치: `Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`
4. `RatHostPrototype.unity` 생성 여부
5. `Assets/_Project/` 하위 폴더 생성 여부
6. `RatHostPrototype.unity`를 Build Settings 시작 씬으로 둘지 여부
7. `SampleScene.unity`를 빌드 대상에서 제외하거나 뒤로 미룰지 여부
8. `포유류 적응`의 1차 효과
   - 특정 통로 접근
   - 보상 지점 접근
   - 다른 임시 효과
9. 실패 후 재시도 초기화 기준
10. 구현 후 테스트와 빌드 실행 범위
11. Unity MCP를 통한 씬, 에셋, 코드, ProjectSettings, Build Settings 변경 범위
12. 이 초안을 공식 `docs/rat-host-implementation-plan.md`로 승격할지 여부

승인 예시:

```text
승인: 쥐 숙주 프로토타입 승인 패킷 전체 승인
```

수정 승인 예시:

```text
수정 승인:
- 2번 Input System 사용 승인
- 8번 포유류 적응은 특정 통로 접근으로 승인
- 9번 재시도 초기화 기준은 초안대로 승인
```

공식 문서 승격 전까지 이 산출물은 `_workspace/active/.../artifacts/`의 작업 초안 상태로 유지한다.
