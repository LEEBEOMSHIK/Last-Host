# 쥐 숙주 핵심 루프 구현 계획 초안

작성일: 2026-06-29
작성 주체: 게임플레이 루프 에이전트 산출물 초안
작업 ID: `2026-06-29-rat-host-plan-agent`

## 문서 상태

이 문서는 공식 `docs/` 문서가 아니라 active 작업 산출물 초안이다. 조정자 검토와 사용자 승인 전에는 Unity 씬, C# 코드, 에셋, ProjectSettings 변경을 시작하지 않는다.

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
- 조작: `WASD` 이동, `Space` 상호작용
- 카메라: 고정 쿼터뷰
- 미니게임 성공 조건: 바이러스 안정도 0 이전에 변이 조각 3개 수집
- 미니게임 실패 조건: 실패 화면과 재시도
- 에셋: 기본 도형, 단순 저폴리 모델, 단색/픽셀풍 임시 머티리얼

### 구현 전 차단 조건

- `docs/rat-host-approval-packet.md` 전체 승인 또는 수정 승인이 필요하다.
- Unity MCP를 통한 씬, 에셋, 패키지, 코드, ProjectSettings 변경 승인이 필요하다.
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
```

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

처리:

- 기존 `Assets/Scenes/SampleScene.unity`는 삭제하지 않는다.
- 새 씬 `RatHostPrototype`에 루트 오브젝트 그룹만 배치한다.
- 초기에는 빌드 설정에 추가할지 여부를 별도 승인 항목으로 둔다.

출력:

- 게임 전용 폴더 구조
- 단일 프로토타입 씬

UI 반응:

- 없음

수용 기준:

- `Assets/_Project/` 아래에 프로젝트 전용 구조가 있다.
- `RatHostPrototype.unity`가 생성되어 있다.
- 씬에는 Core, RatHostMode, VirusMinigameMode, UI, Cameras, Lighting 루트가 있다.

검증 시나리오:

- Unity MCP `Unity_ListResources(Under=Assets/_Project)`로 리소스 존재를 확인한다.
- Unity MCP `Unity_ManageScene(Action=GetActive)`로 활성 씬 정보를 확인한다.
- `git diff --check`를 실행한다.

Unity 아키텍처 검토 필요:

- `RatHostPrototype.unity`를 빌드 설정에 바로 추가할지
- `Assets/Scenes/SampleScene.unity`를 유지할지, 후속 정리 후보로 표시할지

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

검증 시나리오:

- 디버그 트리거 또는 테스트용 버튼으로 각 모드 전환을 순서대로 실행한다.
- 전환 후 잘못된 모드 루트가 동시에 활성화되지 않는지 확인한다.
- 전환 중 HUD가 이전 모드 값을 계속 표시하지 않는지 확인한다.

QA 검토 필요:

- 모드 전환 중 입력이 중복 처리되는지
- 실패 후 재시도 시 면역 경계도와 미니게임 상태 초기화 기준

### 4. 쥐 숙주 이동과 상호작용

목적:

- 쥐 조종이 숙주 모드의 기본 플레이로 작동하는지 확인한다.

파일 후보:

- 생성: `UnityProject/Assets/_Project/Scripts/Host/RatHostController.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/Host/HostHealth.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/Host/HostInteractionProbe.cs`
- 생성: `UnityProject/Assets/_Project/Scripts/Host/HostInterestPoint.cs`

입력:

- `WASD`: 이동
- `Space`: 상호작용
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

- `WASD`: 바이러스 이동
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

처리:

- 현재 모드에 필요한 HUD만 표시한다.
- 쥐 모드에서는 숙주 생명력, 면역 경계도, 현재 변이를 표시한다.
- 바이러스 모드에서는 바이러스 안정도와 변이 조각 수를 표시한다.
- 실패 시 재시도 패널을 표시한다.

출력:

- 모드별 HUD 상태
- 실패/재시도 UI 상태

수용 기준:

- 쥐 모드와 바이러스 모드의 HUD가 구분된다.
- 변이 조각 목표가 `0/3`, `1/3`, `2/3`, `3/3`처럼 읽힌다.
- 실패 시 재시도 버튼 또는 입력 안내가 보인다.
- UI 요소가 16:9 데스크톱 화면에서 서로 겹치지 않는다.

검증 시나리오:

- 쥐 모드, 바이러스 모드, 변이 선택, 실패 화면을 각각 확인한다.
- 긴 텍스트가 버튼이나 패널 밖으로 넘치지 않는지 확인한다.

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

## Unity 아키텍처 에이전트 검토 요청

- `RatHostPrototype` 단일 씬으로 시작하는 구성이 적절한지
- `Assets/_Project/` 폴더 구조가 현재 Unity 템플릿 자산과 충돌하지 않는지
- `Input System` 패키지를 직접 사용할지, 초기에는 `UnityEngine.Input` 수준으로 제한할지
- `RatHostPrototype.unity`를 Build Settings에 바로 추가할지
- `SampleScene.unity`를 유지할지

## QA/검증 에이전트 검토 요청

- 수동 플레이 체크리스트가 핵심 루프 성공 기준을 충분히 증명하는지
- EditMode 또는 PlayMode 테스트를 어느 범위까지 요구할지
- 컴파일, 플레이, 빌드 중 어떤 검증을 구현 완료 조건으로 둘지
- UI 겹침과 카메라 프레이밍을 어떤 화면 크기로 확인할지

## 범위 초과 감지 기준

다음 요청이 들어오면 구현 계획을 확장하지 말고 별도 승인 질문으로 분리한다.

- 벌레 튜토리얼 추가
- 두 번째 숙주 추가
- 인간 숙주, 병원, 연구소, 백신 시스템 추가
- 엔딩 또는 장기 성장 시스템 추가
- 최종 모델, 애니메이션, AI 생성 아트 에셋 제작
- 복잡한 백혈구 패턴이나 다중 적 타입 추가

## 사용자 승인 필요

구현을 시작하려면 아래 중 하나가 필요하다.

```text
승인: 쥐 숙주 프로토타입 승인 패킷 전체 승인
```

또는:

```text
수정 승인:
- 항목 번호와 변경 내용
```

추가로, 이 초안을 공식 문서로 승격하려면 다음 승인이 필요하다.

```text
승인: 계획 초안을 docs/rat-host-implementation-plan.md로 반영
```
