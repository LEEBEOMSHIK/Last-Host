# 쥐 숙주 구현 계획 Unity 아키텍처 검토

작성일: 2026-06-29
담당: Unity 아키텍처 에이전트
작업 ID: `2026-06-29-rat-host-architecture-review`

## 1. Unity 구조안 검토 결과

검토 결론: 구현 계획 초안의 Unity 구조안은 1차 쥐 숙주 프로토타입에 적합하다. `Core`, `Host`, `Immune`, `VirusMinigame`, `Mutations`, `UI`로 시스템 경계를 나눈 점은 현재 프로젝트 기준과 맞고, `AGENTS.md`와 Unity 구조 기준 문서가 요구하는 모듈 경계와도 충돌하지 않는다.

다만 이 구조는 아직 승인 전 계획이다. 실제 `UnityProject/Assets` 아래 폴더, 씬, C# 파일, 프리팹, 머티리얼, ProjectSettings를 만들거나 바꾸려면 사용자 승인이 먼저 필요하다.

구조상 유지할 기준:

- `Core`는 현재 모드, 공용 상태, 모드 전환만 담당한다.
- `Host`는 쥐 조작, 숙주 생명력, 상호작용만 담당한다.
- `Immune`은 면역 경계도 수치와 경계도 상승 원인만 담당한다.
- `VirusMinigame`은 내부 아레나, 바이러스 조작, 백혈구, 변이 조각 수집만 담당한다.
- `Mutations`는 변이 선택 데이터와 적용 로직만 담당한다.
- `UI`는 표시와 선택 입력 전달에 집중하고 게임 규칙을 소유하지 않는다.

초안에서 주의할 점:

- `PrototypeGameState`를 전역 정적 상태처럼 키우지 말고, 1차 프로토타입에 필요한 값만 직렬화 가능한 형태로 둔다.
- `ModeTransitionController`가 모든 시스템의 세부 규칙까지 갖지 않게 한다. 모드 전환 트리거와 루트 활성화 제어 정도로 제한한다.
- 실패/재시도, 변이 선택 후 복귀 시 초기화 책임을 명확히 해야 한다. 특히 미니게임 오브젝트, 입력, UI 패널이 이전 상태를 들고 있으면 루프 검증이 흐려진다.

## 2. `RatHostPrototype` 단일 씬 적합성

`RatHostPrototype` 단일 씬으로 시작하는 제안은 적합하다.

이유:

- 1차 목표는 씬 로딩 구조가 아니라 숙주 조종, 면역 경계도, 내부 미니게임, 변이 선택 후 복귀의 최소 루프 검증이다.
- 쥐 모드와 바이러스 모드를 한 씬 안의 루트 오브젝트 활성/비활성으로 전환하면 초기 구현과 디버깅 비용이 낮다.
- `Bootstrap`, 별도 `VirusMinigamePrototype` 씬을 지금 분리하면 씬 간 상태 전달, 로딩 순서, 테스트 경로가 먼저 복잡해진다.

권장 씬 루트:

```text
RatHostPrototype
  Core
  RatHostMode
  VirusMinigameMode
  UI
  Cameras
  Lighting
```

조건:

- `RatHostMode`와 `VirusMinigameMode`는 루트 단위로 켜고 끌 수 있어야 한다.
- 두 모드가 같은 입력을 동시에 소비하지 않도록 입력 활성화 범위를 나눠야 한다.
- 카메라는 처음부터 두 대를 둘 수 있지만, 활성 카메라는 현재 모드 기준으로 하나만 유지한다.
- 단일 씬이 커지거나 두 모드의 리셋 규칙이 충돌하기 시작하면 그때 씬 분리를 재검토한다.

## 3. `Assets/_Project/` 구조 적합성

`UnityProject/Assets/_Project/` 아래에 게임 전용 구조를 두는 제안은 적합하다.

현재 베이스라인상 `Assets/Scenes/SampleScene.unity`, `Assets/Settings/*`, `Assets/InputSystem_Actions.inputactions`, `Assets/TutorialInfo/*` 등 Unity 템플릿 자산이 남아 있다. `_Project`를 사용하면 템플릿 자산과 실제 게임 자산을 분리할 수 있어 이후 정리가 쉬워진다.

권장 구조:

```text
UnityProject/Assets/_Project/
  Scenes/
    RatHostPrototype.unity
  Scripts/
    Core/
    Host/
    Immune/
    VirusMinigame/
    Mutations/
    UI/
  Prefabs/
    Host/
    VirusMinigame/
    UI/
  Materials/
    Placeholder/
  Settings/
    Input/
```

초안의 구조에 `Settings/Input/`을 추가하는 것을 권장한다. Input System을 사용하기로 승인되면 게임 전용 입력 액션 자산을 템플릿 루트의 `Assets/InputSystem_Actions.inputactions`와 섞지 않고 이 위치에 둘 수 있다.

아직 필요하지 않은 폴더:

- `Art/`, `Audio/`는 1차 구현에서 실제 자산을 만들 때 추가해도 된다.
- `Tests/`는 QA/검증 에이전트가 테스트 범위를 정한 뒤 추가하는 편이 낫다.

## 4. Input System 사용 여부 제안

제안: 1차 프로토타입도 `Input System` 사용을 권장한다.

이유:

- 현재 패키지 목록에 `com.unity.inputsystem`이 이미 포함되어 있다.
- 쥐 모드, 바이러스 모드, UI 선택/재시도처럼 입력 컨텍스트가 바뀌는 구조와 잘 맞는다.
- PC 키보드 우선으로 시작하더라도 이후 게임패드나 모바일 대응을 고려할 때 확장 비용이 낮다.

권장 범위:

- 새 입력 액션 자산 후보: `Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`
- 액션 맵 후보: `Host`, `VirusMinigame`, `UI`
- 공통 액션: `Move`, `Interact`, `Confirm`, `Retry`
- 1차 범위에서는 리바인딩 UI, 복수 컨트롤러 프로필, 모바일 터치 입력은 제외한다.

주의:

- 기존 템플릿 자산인 `Assets/InputSystem_Actions.inputactions`를 바로 수정하지 않는다.
- Player Settings의 Active Input Handling 변경이 필요하면 ProjectSettings 변경이므로 별도 승인을 받아야 한다.
- 구현 승인 전에는 Input System 액션 자산도 생성하지 않는다.

대안:

- 아주 빠른 임시 실험이라면 `UnityEngine.Input`으로 시작할 수 있지만, 현재 프로젝트에는 이미 Input System 패키지가 있으므로 장기적으로 다시 갈아엎을 가능성이 크다. 따라서 구조 검토 기준에서는 Input System 사용을 권장한다.

## 5. Build Settings에 `RatHostPrototype.unity`를 추가할지 여부

제안: 구현 승인 후 `RatHostPrototype.unity`가 생성되고 최소 루트 구성이 확인되면 Build Settings에 추가한다.

권장 정책:

- `Assets/_Project/Scenes/RatHostPrototype.unity`를 enabled scene으로 등록한다.
- PC 프로토타입 빌드의 시작 씬이 되도록 Build Index `0` 위치를 권장한다.
- `SampleScene.unity`가 계속 Build Index `0`이면 빌드 실행 시 프로토타입이 아니라 샘플 씬이 열릴 수 있으므로 피한다.

단계:

1. 승인 후 `RatHostPrototype.unity`를 만든다.
2. 씬이 열리고 기본 루트가 있는지 확인한다.
3. Build Settings에 `RatHostPrototype.unity`를 등록한다.
4. `SampleScene.unity`는 삭제하지 않고 빌드 대상에서는 제외하거나 뒤로 보낸다.

주의:

- Build Settings 변경은 Unity 프로젝트 설정 변경에 해당하므로 사용자 승인 전에는 실행하지 않는다.

## 6. `SampleScene.unity` 처리 방안

제안: 당장은 삭제하지 않는다. 보관하되, 프로토타입 빌드 대상에서는 제외하는 방향이 적절하다.

권장 처리:

- 파일 유지: `Assets/Scenes/SampleScene.unity`
- 역할: Unity 템플릿 기본 씬 또는 비교용 잔여 자산
- Build Settings: `RatHostPrototype.unity`를 시작 씬으로 삼는 시점에는 비활성화하거나 빌드 목록에서 제외
- 후속 정리: 쥐 숙주 프로토타입이 안정화된 뒤 템플릿 자산 정리 작업으로 별도 처리

이유:

- 지금 삭제하면 불필요한 Unity `.meta` 변경과 템플릿 자산 정리 범위가 섞인다.
- 현재 활성 씬이 저장되지 않은 기본 씬 상태이고 Build Settings에는 `SampleScene.unity`만 있으므로, 구현 전에는 작업 씬 정책을 먼저 확정해야 한다.

## 7. 승인 필요 항목

구현 전 사용자 승인이 필요한 항목:

- `docs/prototype/rat-host-approval-packet.md` 전체 승인 또는 수정 승인
- Unity 버전 `6000.4.6f1` 고정 여부
- URP 사용 확정
- PC 키보드 우선 프로토타입 확정
- 초기 입력 방식: Input System 사용 여부와 입력 액션 자산 생성 위치
- `RatHostPrototype.unity` 생성 승인
- `Assets/_Project/` 하위 폴더 생성 승인
- Build Settings에 `RatHostPrototype.unity`를 등록하고 시작 씬으로 둘지 여부
- `SampleScene.unity`를 빌드 대상에서 제외하거나 뒤로 미룰지 여부
- 플레이스홀더 에셋 사용 범위
- `포유류 적응`의 1차 임시 효과를 특정 통로 접근으로 할지, 보상 지점 접근으로 할지
- Unity MCP를 통한 씬, 에셋, 코드, ProjectSettings 변경 승인
- 구현 계획 초안을 공식 `docs/` 문서로 승격할지 여부

## 8. 구현 전 위험

- 승인 패킷이 아직 최종 승인되지 않았으면 구현 전제가 흔들릴 수 있다.
- 현재 Build Settings에는 `SampleScene.unity`만 등록되어 있어 빌드 시작 씬이 프로토타입과 어긋날 수 있다.
- 현재 활성 씬은 저장되지 않은 기본 씬 상태로 기록되어 있어, 구현 시작 시 명시적으로 작업 씬을 열어야 한다.
- Input System 사용을 권장하지만 Active Input Handling 상태가 맞지 않으면 ProjectSettings 변경 승인이 필요할 수 있다.
- 단일 씬 구조는 초기에는 단순하지만, 모드 전환과 리셋 책임이 불명확하면 두 모드의 상태가 서로 오염될 수 있다.
- UI, 입력, 모드 루트 비활성화가 맞물리면 변이 선택이나 재시도 중 이동 입력이 남는 문제가 생길 수 있다.
- `포유류 적응` 효과가 확정되지 않으면 맵 구조와 변이 검증 지점 설계가 늦어진다.
- 도트풍 저폴리 3D 렌더링 조정은 아직 수행되지 않았으므로, 초기 화면이 의도한 비주얼보다 단순한 플레이스홀더로 보일 수 있다.
- Test Framework는 포함되어 있지만 1차 구현에서 EditMode/PlayMode 테스트 범위가 아직 정해지지 않았다.

## 9. 다음 담당 추천

다음 담당 순서:

1. 프로젝트 조정 에이전트: 승인 패킷, Input System, 씬/Build Settings 처리 방침을 사용자 승인 항목으로 정리한다.
2. 게임플레이 루프 에이전트: 이 검토 결과를 반영해 구현 계획 초안의 구조 전제와 차단 조건을 갱신한다.
3. QA/검증 에이전트: 단일 씬 모드 전환, 입력 충돌, Build Settings, 수동 플레이 체크리스트, 테스트 범위를 검토한다.
4. 비주얼/테크아트 에이전트: 구현 승인 후 카메라, 조명, 저해상도 렌더링, 플레이스홀더 머티리얼 기준을 별도 검토한다.

최종 판단:

- 구조안은 1차 쥐 숙주 프로토타입에 사용할 수 있다.
- 단일 씬과 `Assets/_Project/` 구조는 조건부로 승인 가능하다.
- 실제 Unity 변경은 승인 패킷과 프로젝트 변경 승인이 끝난 뒤 진행해야 한다.
