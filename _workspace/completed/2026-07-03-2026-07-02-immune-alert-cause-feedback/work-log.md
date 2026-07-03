# 작업 로그

## 2026-07-02

- 사용자 요청에 따라 `면역 경계도 원인 피드백` 구현 작업을 시작했다.
- 기존 구조를 확인했다.
  - `PrototypeSessionState`가 면역 경계도 변경과 모드 전환을 관리한다.
  - `RatRiskInteractable`은 `session.AddRiskAlert(riskSeverity)`로 소음 배관 위험 행동을 전달한다.
  - `PrototypeHud`는 `objectiveText`와 각 게이지 텍스트를 갱신한다.
- 구현 방향은 상태 모델에 마지막 면역 경계도 피드백을 저장하고 HUD가 이를 표시하는 방식으로 정했다.
- TDD RED:
  - `RatHostPrototypeCoreTests.cs`에 면역 경계도 원인 피드백 테스트 3개를 먼저 추가했다.
  - Unity batch Test Runner는 `return code 1`로 테스트 결과 XML을 만들지 못했다. 대용량 실행 로그는 커밋에서 제외하고 실패 요약만 보존한다.
  - Unity MCP에서 동일 테스트 3개를 직접 호출해 예상 실패를 확인했다.
    - `RatRiskInteractable.immuneAlertFeedbackLabel` 필드 없음
    - `PrototypeSessionState.AddRiskAlert(float, string)` 오버로드 없음
- 구현:
  - `PrototypeSessionState`에 마지막 면역 경계도 피드백 라벨, 실제 변화량, 표시 문자열을 추가했다.
  - `AddRiskAlert(float, string)`와 `AddImmuneAlertAmount(float, string)` 오버로드를 추가하고 실제 변화량을 기록하게 했다.
  - `RatRiskInteractable`에 기본 피드백 라벨 `소음/조직 자극`을 추가하고 위험 이벤트 호출 시 전달하게 했다.
  - `PrototypeSessionController`가 경계도 값만 바뀐 경우에도 HUD를 갱신하게 했다.
  - `PrototypeHud`는 쥐 모드에서 피드백이 있으면 `objectiveText`에 `소음/조직 자극 +15` 같은 짧은 문구를 우선 표시한다.
- 구현 에이전트 사전 검증:
  - Unity MCP 에셋 Refresh 후 컴파일 오류 0건을 확인했다.
  - 신규 RED 테스트 3개가 GREEN으로 통과했다.
  - `RatHostPrototypeCoreTests` 전체 27개 테스트 메서드를 Unity MCP에서 직접 호출해 27/27 통과를 확인했다.
  - Unity 콘솔 Error 0건을 확인했다.
  - `git diff --check`는 종료 코드 0이며 CRLF 변환 경고만 출력했다.

## 2026-07-02 추가 보정

- 사용자 피드백:
  - `objectiveText` 재사용은 허용 가능하지만, 피드백이 계속 남아 일반 목표/프롬프트를 가리는 문제가 있다.
  - 짧은 피드백이어야 하므로 일정 시간 후 자동으로 일반 목표/상호작용 프롬프트로 복귀해야 한다.
- 원인 확인:
  - `PrototypeSessionState`가 마지막 피드백 라벨과 변화량은 저장하지만 피드백 표시 수명은 관리하지 않았다.
  - 따라서 `HasImmuneAlertFeedback`가 한 번 true가 되면 변이 선택 등 명시적 clear 전까지 계속 true로 남았다.
- TDD RED:
  - `Session_ImmuneAlertFeedbackExpiresAfterConfiguredRatModeTime`
  - `PrototypeHud_ReturnsToRiskPromptAfterImmuneAlertFeedbackExpires`
  - Unity MCP 직접 호출 결과 두 테스트 모두 실패.
  - 실패 원인: `PrototypeConfig.ImmuneAlertFeedbackSeconds` 프로퍼티 없음.
- 구현:
  - `PrototypeConfig.ImmuneAlertFeedbackSeconds` 기본값 `2f`를 추가했다.
  - `PrototypeSessionState`에 피드백 남은 시간을 추가했다.
  - 피드백 기록 시 config 표시 시간을 설정하고, `TickRatMode`에서 시간이 지나면 피드백을 clear한다.
  - 만료 후 HUD가 기존 `objectiveText` 우선순위에 따라 상호작용 프롬프트 또는 일반 목표로 돌아가게 했다.
- 구현 에이전트 사전 검증:
  - 신규 만료 테스트 2개 GREEN.
  - `RatHostPrototypeCoreTests` 전체 29개 테스트 메서드를 Unity MCP에서 직접 호출해 29/29 통과.
  - Unity 콘솔 Error 0건.
  - `git diff --check`는 종료 코드 0이며 CRLF 변환 경고만 출력했다.

## 2026-07-03 독립 검증

- 원본 Unity 프로젝트는 에디터가 열려 있어 batch EditMode 테스트가 프로젝트 잠금으로 실패했다.
  - 대용량 실행 로그는 커밋에서 제외하고 실패 요약만 보존한다.
- 원본 프로젝트를 임시 폴더로 복사해 `Library`, `Temp`, `Logs`, `obj`, `.vs`를 제외한 동일 소스 상태로 EditMode 테스트를 실행했다.
  - 임시 경로: `%TEMP%\LastHost_UnityProject_Test_20260702_immune_feedback`
  - 첫 실행은 초기 임포트/패키지 복원 단계에서 테스트 결과 XML을 만들지 못했다.
  - `-quit` 없이 재실행한 결과 Unity Test Runner가 정상 실행됐다.
- 공식 Unity Test Runner 결과:
  - 결과 파일: `artifacts/editmode-results-copy-noquit.xml`
  - 결과: 29/29 통과, 실패 0, 스킵 0.
  - 신규 테스트 5개 모두 통과했다.
- `git diff --check`를 재실행했다.
  - 종료 코드 0.
  - CRLF 변환 경고만 출력됐다.
