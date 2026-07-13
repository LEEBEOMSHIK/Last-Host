# 게임플레이 루프 검토: 원인 라벨 enum/event type 전환

## 루프 단계

쥐 숙주 조종 중 위험 행동·위험 구역·강제 조종으로 면역 경계도가 상승하고, 100% 도달 시 원인에 맞는 내부 대응 미니게임을 선택하는 단계다. 이번 검토는 이 전환의 **원인 판정 데이터**와 **플레이어 표시 문구**를 분리하는 최소 구조만 다룬다.

## 현재 문자열 결합 지점과 회귀 위험

| 지점 | 현재 역할 | 위험 |
| --- | --- | --- |
| `RatHostController.forcedControlFeedbackLabel` | `"강제 조종"`을 표시하고 `AddImmuneAlertAmount`에 전달 | Inspector에서 문구를 바꾸면 신호 억제 선택도 바뀌거나 기본형으로 후퇴한다. |
| `RatRiskInteractable.immuneAlertFeedbackLabel` | `"소음/조직 자극"`을 표시하고 `AddRiskAlert`에 전달 | 복수 원인을 한 라벨에 합쳐도 `Contains` 우선순위에 의존한다. 번역·표현 변경이 분기에 영향을 준다. |
| `ImmuneRiskZone.immuneAlertFeedbackLabel` / `forcedControlFeedbackLabel` | `"오염 노출"`과 `"강제 조종"`을 표시하고 각각 경계도 API에 전달 | 위험 구역 지속 피해와 강제 조종이 동일한 문자열 API에 묶여, 신규 호출부에서 원인 누락 가능성이 있다. |
| `PrototypeSessionState.AddRiskAlert` / `AddImmuneAlertAmount` | 라벨로 HUD 피드백을 기록하고, 임계치 도달 때 동일 라벨로 미니게임을 선택 | 표시 수명·누적 규칙과 게임플레이 선택 책임이 한 인자에 결합되어 있다. |
| `SelectInternalMinigameTypeForAlertCause` | trim 후 키워드 `Contains`로 분기 | 오탈자, 조사·공백·로컬라이즈, 부분 일치(예: 미래 문구의 `경보`)가 무음으로 기본 미니게임 또는 잘못된 미니게임을 선택한다. |
| `LastImmuneAlertFeedbackLabel` / HUD | UI 텍스트와 동일 문자열 비교로 연속 경계도 증가를 합산 | 같은 원인이라도 표시 문구가 달라지면 누적이 분리된다. 반대로 같은 문구를 다른 원인에 재사용하면 잘못 합산된다. |

현재 테스트는 `강제 조종`, `소음/조직 자극`, `오염 노출`, `면역 포착` 등 대표 라벨만 고정한다. `바이러스 흔적`, `면역 신호`, `경보`, 공백·번역·알 수 없는 문자열의 선택 회귀를 완전히 막지 못한다.

## 최소 enum/event type 모델

### 책임 분리

| 요소 | 제안 책임 | UI 문구·미니게임과의 관계 |
| --- | --- | --- |
| `ImmuneAlertCauseType` enum | 게임플레이 원인을 식별한다. 최소값은 `Unspecified`, `ContaminationExposure`, `ForcedHostControl`, `NoiseOrTissueIrritation`, `ImmuneDetection`, `VirusPatternExposure`다. | 표시 문구를 갖지 않으며, 원인 판정의 유일한 키다. |
| `ImmuneAlertEvent` 값 객체/구조체 | 한 번의 경계도 상승에 필요한 `CauseType`, `FeedbackLabel`, `Amount 또는 Severity`를 함께 전달한다. | `FeedbackLabel`은 빈 문자열을 허용한다. UI는 이 값만 표시한다. |
| `ImmuneAlertPresentation` 또는 event의 `FeedbackLabel` | HUD 표시·동일 표시 문구의 일시 누적만 담당한다. | 미니게임 선택에 사용하지 않는다. 초기 이전에서는 기존 한국어 문구를 그대로 보존한다. |
| `InternalMinigameTypeResolver` (순수 매핑) | `CauseType -> InternalVirusMinigameType`을 결정하고 `Unspecified`는 `Config.DefaultInternalMinigameType`으로 보낸다. | UI 라벨을 읽지 않는다. 현재는 `PrototypeSessionState`의 private 순수 메서드로 두어도 충분하며, 유형이 더 늘 때만 별도 클래스로 추출한다. |

최소 호출 형태는 `AddRiskAlert(severity, new ImmuneAlertEvent(ImmuneAlertCauseType.NoiseOrTissueIrritation, "소음/조직 자극"))` 또는 양별 API의 동등한 형태다. 기존 무라벨 오버로드는 `Unspecified`와 빈 라벨을 만들도록 유지해 기존 호출의 기본 미니게임 동작을 보존한다.

### 매핑 규칙

| 원인 타입 | 기존 대표 라벨 | 현재와 동일한 미니게임 | 비고 |
| --- | --- | --- | --- |
| `Unspecified` | 빈 문자열·알 수 없는 기존 라벨 | `Config.DefaultInternalMinigameType` | 하위 호환 및 안전한 기본값 |
| `ContaminationExposure` | `오염 노출` | `WhiteBloodCellEvasion` | 현재 `오염` 키워드 규칙 보존 |
| `ImmuneDetection` | `면역 포착` | `WhiteBloodCellEvasion` | 현재 `면역 포착` 규칙 보존 |
| `VirusPatternExposure` | `바이러스 흔적` | `WhiteBloodCellEvasion` | 현재 `바이러스 흔적` 규칙 보존 |
| `ForcedHostControl` | `강제 조종` | `ImmuneSignalSuppression` | 현재 `강제 조종` 규칙 보존 |
| `NoiseOrTissueIrritation` | `소음/조직 자극` | `ImmuneSignalSuppression` | 기존 복합 라벨의 의미를 한 타입으로 명시 |

문서상 후보인 `면역 신호`, `경보`는 현재 실제 호출부가 확인되지 않았으므로, 타입을 지금 추가하거나 새 콘텐츠로 취급하지 않는다. 이후 실제 발생원이 추가될 때 해당 발생원과 매핑을 함께 승인·추가한다.

## 단계적 이전안

1. `ImmuneAlertCauseType`과 최소 `ImmuneAlertEvent`를 Core에 추가하고, 원인 타입 매핑의 단위 테스트를 먼저 만든다. 이 단계에서는 기존 `string feedbackLabel` 오버로드를 삭제하지 않는다.
2. `PrototypeSessionState`와 `PrototypeSessionController`에 event/enum 기반 오버로드를 추가한다. 임계치 선택은 오직 `CauseType`으로 처리하고, HUD 기록은 event의 `FeedbackLabel`로 처리한다.
3. 실제 발생원 4곳(`RatHostController`, `RatRiskInteractable`, `ImmuneRiskZone`의 노출·강제 조종)을 타입+기존 라벨 호출로 한 번에 이전한다. 씬 Inspector의 라벨 문자열은 UI 용도로 유지하되 원인 타입은 enum 직렬화 필드로 분리한다.
4. 모든 활성 호출부가 이전된 것을 검색과 EditMode 테스트로 확인한 뒤, 문자열 기반 미니게임 선택 함수와 키워드 `Contains`를 제거한다. 단, 무라벨 호환 오버로드는 `Unspecified` 기본 경로로 유지한다.

## 수용 기준

- 동일한 원인 타입은 UI 문구를 바꾸거나 빈 문구로 두어도 같은 미니게임을 선택한다.
- 기존 여섯 매핑(무라벨, 오염, 면역 포착, 바이러스 흔적, 강제 조종, 소음/조직 자극)이 표의 결과와 일치한다.
- `Unspecified`는 기존처럼 `Config.DefaultInternalMinigameType`을 사용한다.
- HUD는 기존 대표 라벨과 `+수치` 형식을 유지하며, 피드백 지속 시간·누적·정리 동작이 바뀌지 않는다.
- 임계치 미도달 이벤트는 HUD 피드백만 갱신하고 미니게임을 시작하지 않는다.
- 문자열 키워드 검사로 미니게임을 고르는 코드가 남지 않는다.
- 새 타입·미니게임·보상·난이도 규칙을 추가하지 않는다.

## 테스트 시나리오

1. 각 원인 타입이 임계치 도달 시 표의 미니게임으로 진입하는 EditMode 단위 테스트를 둔다.
2. `ForcedHostControl`의 표시 라벨을 임의 한국어 문구와 빈 문자열로 각각 바꿔도 신호 억제로 진입하는지 확인한다.
3. `ContaminationExposure`의 표시 라벨을 바꿔도 백혈구 회피로 진입하는지 확인한다.
4. `Unspecified`와 무라벨 기존 오버로드가 서로 같은 기본 미니게임을 선택하는지 확인한다.
5. `NoiseOrTissueIrritation` 반복 이벤트의 동일 라벨 수치 누적과, 서로 다른 라벨일 때 표시 교체가 현재 HUD 규칙과 같은지 확인한다.
6. `RatHostController`, `RatRiskInteractable`, `ImmuneRiskZone`의 실제 이벤트 발생에서 타입·표시 라벨이 함께 전달되는지 Play 검증한다.
7. 전체 EditMode 스위트와 기존 백혈구 회피/신호 억제 성공·실패 루프 회귀를 실행한다.

## 범위 초과 여부와 승인

- **이번 검토:** 설계 문서와 작업 기록만 변경하므로 승인된 쥐 숙주 루프 안의 저위험 분석이며, 새 콘텐츠·씬·패키지·ProjectSettings 변경이 없다.
- **실제 전환:** Unity 게임플레이 코드와 씬 직렬화 enum 필드를 바꾸므로 AGENTS.md의 `게임플레이 코드 작성`, `Unity 프로젝트 수정` 승인 게이트 대상이다. 현재 사용자 요청은 검토 진행으로 해석하며, 구현 전에는 사용자 승인을 다시 받아야 한다.
- **범위 밖:** 추가 원인 콘텐츠, 신규 내부 미니게임, 원인별 보상·난이도, 로컬라이제이션 체계 전면 도입은 이번 최소 리팩터링에 포함하지 않는다.
