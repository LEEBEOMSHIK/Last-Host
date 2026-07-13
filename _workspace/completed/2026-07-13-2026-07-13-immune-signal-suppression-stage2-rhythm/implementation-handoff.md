# 게임플레이 구현 인계: 면역 신호 억제 2단계 리듬 확장

## 구현 결과

- 첫 내부 대응(`ImmuneResponseExperience == 1`)은 기존 1단계 고정 간격과 일반 신호를 유지한다.
- 두 번째 내부 대응부터 2단계를 선택한다.
- 2단계는 고정 신호열 `1.0, 0.7, 1.2, 0.65, 1.0, 0.8` 배율을 반복하며, 0.7·0.65·0.8 배율 신호를 `빠름`으로 표시한다. 난수를 사용하지 않는다.
- 런타임 신호 패널에 `리듬 N단계 · 다음 신호 일반/빠름` 상태 줄을 추가했다.
- 신호 마커 이동과 정확 판정창 폭은 현재 신호 간격을 사용하도록 갱신했다.

## 변경 파일

- `UnityProject/Assets/_Project/Scripts/VirusMinigame/ImmuneSignalSuppressionModel.cs`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionState.cs`
- `UnityProject/Assets/_Project/Scripts/UI/ImmuneSignalSuppressionHud.cs`
- `UnityProject/Assets/_Project/Scripts/UI/PrototypeHud.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`

## 추가 EditMode 검증

- 1단계 고정 간격·일반 신호 회귀
- 2단계 결정형 빠른 신호와 리셋 보존
- 성공 후 두 번째 내부 대응의 2단계 전환 및 성공 흐름
- 실패 후 다음 내부 대응의 2단계 전환
- 런타임 HUD의 2단계·다음 빠른 신호 문구

## 실행한 검증

- `git diff --check`: 통과.
- Unity Editor 로그: 변경 스크립트와 테스트 스크립트 import 확인, 확인 시점에 C# 컴파일 오류 문자열 없음.
- Unity batch EditMode 실행: 기존 Unity Editor가 같은 프로젝트를 열고 있어 별도 요청이 결과 XML/로그를 생성하지 못했다. 테스트 통과로 주장하지 않음.

## QA 인계

- Unity Editor에서 `LastHost.Prototype.Tests.EditMode.RatHostPrototypeCoreTests` EditMode 테스트를 실행한다.
- 신호 억제 첫 진입은 `리듬 1단계 · 다음 신호 일반`, 성공 또는 실패 후 다음 진입은 `리듬 2단계 · 다음 신호 일반`을 확인한다.
- 2단계 첫 신호를 정확히 누르면 다음 상태가 `리듬 2단계 · 다음 신호 빠름`으로 바뀌고, 다음 신호가 0.7초 뒤 판정선에 도달하는지 확인한다.

## 미해결 위험

- 2단계 신호열의 실제 체감 난이도와 HUD 배치가 Unity Play에서 아직 독립 검증되지 않았다.
- `UnityProject/ProjectSettings/ProjectSettings.asset`의 기존 사용자 변경은 건드리지 않았다.

## QA 후속 테스트 수정 (2026-07-13)

- 전체 EditMode 실행에서 확인된 테스트 준비/기대값 불일치 2건을 테스트 파일에서만 수정했다.
- `Session_SecondInternalResponseUsesSignalSuppressionStageTwoAndPreservesOutcomeFlow`는 성공 조건이 2회 입력인데 1회만 입력하던 문제를 바로잡아, 첫 대응을 실제 성공 처리한 뒤 2단계를 검증한다.
- `Session_ClearsRiskPromptWhenFeedbackAlertEntersVirusMode`는 `소음/조직 자극`이 기존 원인별 규칙상 `ImmuneSignalSuppression`으로 매핑됨을 명시적으로 검증하고 HUD 기대값을 `면역 신호 억제`로 맞췄다.
- 구현 코드와 `ProjectSettings`는 변경하지 않았다. Unity Test Runner 재실행은 프로젝트 조정 에이전트가 수행한다.
