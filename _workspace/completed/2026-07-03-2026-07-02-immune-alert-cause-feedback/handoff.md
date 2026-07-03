# 인계 기록: 면역 경계도 원인 피드백

## 게임플레이 구현 에이전트 산출물

- `PrototypeSessionState`가 마지막 면역 경계도 피드백 라벨, 실제 변화량, 표시 문자열을 보관한다.
- `RatRiskInteractable` 기본 피드백 라벨은 `소음/조직 자극`이다.
- 소음 배관 기본 위험도 `0.75`와 기본 위험 보너스 `20` 조합으로 `+15` 피드백이 기록된다.
- `PrototypeHud`는 쥐 모드에서 피드백이 있으면 기존 `objectiveText`에 `소음/조직 자극 +15`를 우선 표시한다.
- 피드백은 `PrototypeConfig.ImmuneAlertFeedbackSeconds` 기본값 `2f` 동안 표시되고, `TickRatMode`에서 시간이 지나면 자동으로 clear된다.
- 피드백 만료 후 `objectiveText`는 현재 가능한 상호작용 프롬프트 또는 일반 목표 문구로 돌아간다.
- 면역 경계도 100% 전환 시 `EnterVirusMinigame()` 경로에서 상호작용 프롬프트가 비워지는 회귀 테스트를 추가했다.

## Unity 씬/통합 인계

- 현재 구현은 기존 `objectiveText`를 사용하므로 씬 YAML 직접 수정이나 새 HUD 필드 연결은 필요 없다.
- 별도 피드백 전용 Text로 분리하려면 `PrototypeHud`에 새 직렬화 필드를 추가하고, `RatHostPrototypeSceneBuilder.cs`와 씬 연결 작업을 통합 담당 범위에서 처리해야 한다.
- 이번 작업에서는 씬 YAML과 씬 빌더를 수정하지 않았다.

## QA/검증 인계

- 구현 에이전트가 Unity MCP에서 `RatHostPrototypeCoreTests` 27개 메서드 수동 호출 통과를 확인했다.
- 공식 Unity batch Test Runner는 테스트 결과 XML을 만들지 못하고 `return code 1`로 종료됐다.
- QA는 가능하면 Unity Test Runner 창 또는 안정화된 batch 경로로 EditMode 테스트를 다시 실행해 독립 판정을 남겨야 한다.
- Play 확인 시 중점:
  - 배관 근처 프롬프트가 뜬다.
  - Space 상호작용 직후 HUD objectiveText가 `소음/조직 자극 +15`를 표시한다.
  - 약 2초 후 HUD objectiveText가 다시 `소음 배관 조사 가능` 또는 일반 목표 문구로 돌아간다.
  - 경계도 100% 전환 시 `소음 배관 조사 가능` 프롬프트가 내부 바이러스 모드에 남지 않는다.

## 남은 위험

- `objectiveText`를 피드백 표시에도 재사용하므로, 2초 표시 중에는 일반 목표 문구나 프롬프트보다 우선 표시된다.
- 페이드/애니메이션/별도 피드백 슬롯은 이번 구현 범위 밖이다.
