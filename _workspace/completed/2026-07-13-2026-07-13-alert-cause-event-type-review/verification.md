# QA 검증: 원인 라벨 enum/event type 전환

## 검증 대상

면역 경계도 원인의 표시 문구와 내부 미니게임 선택을 분리하고, 타입 기반 매핑·`Unspecified` 기본 폴백·`면역 신호`/`경보`의 신호 억제 매핑을 보존하는 변경이다.

## 검증 담당

- QA/검증 에이전트
- 작업영역: `_workspace/active/2026-07-13-alert-cause-event-type-review/`

## 독립 검증 여부

- 구현 담당인 게임플레이 구현 에이전트와 검증 담당을 분리했다.
- 구현 인계의 정적 설명에만 의존하지 않고, Unity Play 중 별도 `Unity_RunCommand`로 새 `PrototypeSessionState`를 생성해 타입별 임계치 전환을 재현했다. 이 명령은 씬·프리팹·설정·에셋을 수정하지 않는다.

## 실행한 검증

- `git diff --check`: 통과. 공백 오류 없음. CRLF 변환 경고만 출력됐다.
- 정적 코드 대조:
  - `ImmuneAlertEvent`는 `CauseType`과 HUD용 `FeedbackLabel`을 분리한다.
  - `PrototypeSessionState`는 미니게임 선택에 `alertEvent.CauseType`만 전달하고, 문자열 오버로드는 `Unspecified` 이벤트로 폴백한다.
  - resolver는 `ContaminationExposure`·`ImmuneDetection`·`VirusPatternExposure`을 백혈구 회피로, `ForcedHostControl`·`NoiseOrTissueIrritation`·`ImmuneSignalOrAlarm`을 면역 신호 억제로 연결한다.
  - `default`는 `Config.DefaultInternalMinigameType`을 반환한다.
- Unity batch EditMode 전체 스위트 결과 대조:
  - 결과 파일: `C:\tmp\last-host-alert-cause-editmode-results.xml`
  - `test-run`: total 90 / passed 90 / failed 0 / inconclusive 0 / skipped 0 / result Passed.
  - 실행 시각: 2026-07-13 02:20:24Z~02:20:27Z.
- Unity MCP Play 사전 상태:
  - `RatHostPrototype` 씬이 로드됐고 Play 중이며, 컴파일·업데이트 상태가 아님을 확인했다.
  - 실행 전 및 재현 후 Unity Console Error/Warning 조회 결과는 각각 0건이다.
- Unity MCP `Unity_RunCommand` 런타임 재현 (컴파일·실행 모두 성공):
  - `ImmuneSignalOrAlarm` + `면역 신호` → `ImmuneSignalSuppression`.
  - `ImmuneSignalOrAlarm` + `경보` → `ImmuneSignalSuppression`.
  - 동일한 `ContaminationExposure`에 `표시 A`, `라벨이 달라도 선택은 같음`을 각각 부여해도 둘 다 `WhiteBloodCellEvasion`으로 진입했다. 즉 표시 라벨은 매핑을 바꾸지 않는다.
  - `Unspecified` + 임의 표시 문구 → 현재 `PrototypeConfig.DefaultInternalMinigameType`인 `WhiteBloodCellEvasion`으로 폴백했다.
  - 각 사례에서 임계치 도달 후 `PrototypeGameMode.InternalVirus` 진입과 전달한 HUD 피드백 라벨 보존도 함께 확인했다.
- MCP 씬 상태 대조:
  - Play 중 `RatHostMode`, `PrototypeHud`, `SignalSuppressionPanel`을 포함한 `RatHostPrototype` 계층을 조회했다. 시작 상태에서는 쥐 숙주 모드와 HUD가 활성이고 내부 바이러스 모드 및 신호 억제 패널은 비활성으로, 시작 모드 구성에 부합한다.

## 검증하지 못한 항목

- 사용자 실제 입력으로 `면역 신호` 또는 `경보` 발생원을 임계치까지 올려 완전한 시각 전환을 수행하는 수동 플레이는 실행하지 않았다. 타입 매핑·모드 진입·HUD 피드백은 런타임 모델 재현과 전체 EditMode 스위트로 확인했다.
- PC 빌드는 이번 변경 범위의 완료 기준에 포함되지 않아 실행하지 않았다.

## 실패 또는 경고

- Unity MCP Play 중 Console Error/Warning 0건.
- Unity batch EditMode 실패 0건.

## 게이트 판정

- QA/검증 게이트: 통과.
- `agent-activity.md`에 QA 판정 반영: 완료.
- 총괄 관리자 검토로 넘길 수 있음: 예.

## 남은 위험

- 사용자의 실제 플레이 기준으로 원인별 HUD 문구와 전환 체감은 별도 확인이 남아 있다. 이는 자동 회귀·런타임 매핑 검증을 대체하지 않는 사용성 확인 항목이다.

## 완료 판단

**완료 가능.**

## 완료 판단 근거

전체 EditMode 90/90 통과, Play 중 런타임 타입 매핑 재현, `Unspecified` 기본 폴백, 라벨 독립성, 씬 시작 모드와 Console Error/Warning 0건을 모두 확인했다. 새 콘텐츠·씬·설정 변경은 없으며, 작업 패킷의 완료 기준을 충족한다.
