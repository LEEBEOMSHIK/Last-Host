# 게임플레이 구현 인계: 원인 라벨 enum/event type 전환

## 구현 결과

- `ImmuneAlertCauseType`과 불변 `ImmuneAlertEvent`를 Core에 추가했다.
- 내부 미니게임 선택은 `CauseType`만 사용한다. `FeedbackLabel`은 기존 HUD 표시·누적 처리만 유지한다.
- `Unspecified` 및 기존 무라벨/문자열 오버로드는 `PrototypeConfig.DefaultInternalMinigameType`으로 폴백한다.
- 백혈구 회피는 `ContaminationExposure`, `ImmuneDetection`, `VirusPatternExposure`에, 면역 신호 억제는 `ForcedHostControl`, `NoiseOrTissueIrritation`, `ImmuneSignalOrAlarm`에 연결했다.
- `면역 신호`, `경보`는 `ImmuneSignalOrAlarm` 이벤트 타입으로 명시적으로 면역 신호 억제에 연결했다.

## 호출부 전환

- `RatHostController`: 강제 조종 → `ForcedHostControl`
- `RatRiskInteractable`: 소음/조직 자극 → `NoiseOrTissueIrritation`
- `ImmuneRiskZone`: 오염 노출 → `ContaminationExposure`, 강제 조종 → `ForcedHostControl`
- `PrototypeSessionController`와 `PrototypeSessionState`: event 오버로드 추가 및 기존 문자열 오버로드의 `Unspecified` 폴백 처리

## 변경 파일

- `UnityProject/Assets/_Project/Scripts/Core/ImmuneAlertCauseType.cs`
- `UnityProject/Assets/_Project/Scripts/Core/ImmuneAlertEvent.cs`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionState.cs`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionController.cs`
- `UnityProject/Assets/_Project/Scripts/Host/RatHostController.cs`
- `UnityProject/Assets/_Project/Scripts/Host/RatRiskInteractable.cs`
- `UnityProject/Assets/_Project/Scripts/Immune/ImmuneRiskZone.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`

## 검증과 QA 인계

- 추가 EditMode 테스트: 타입별 매핑, 임의 표시 문구 독립성, `면역 신호`·`경보` 보존, Unspecified/기존 문자열 폴백.
- `git diff --check` 통과.
- `PrototypeSessionState`에서 문자열 `Contains` 기반 원인 선택이 제거되고 타입 기반 resolver만 남았음을 검색으로 확인했다.
- Unity Test Runner와 Play 검증은 QA 담당 대기다.

## 남은 위험

- 새 스크립트 2개를 포함한 Unity 컴파일과 전체 EditMode 회귀는 QA가 확인해야 한다.
- 씬/프리팹/Inspector 직렬화/ProjectSettings/패키지는 변경하지 않았다.
