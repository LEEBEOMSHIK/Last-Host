# 복귀 후 위험 구역 보호 총괄 관리자 검토

## 검토 대상

- 작업 ID: `2026-07-06-contamination-risk-zone-trigger`
- 사용자 질문: `복귀 후 짧은 오염 무시 시간 등의 처리는 한거야? 해야된다는거야?`
- 조정자 답변/구현: 이전까지는 미구현이었고, 이번에 변이 선택 후 `RatHost` 복귀 직후 위험 구역 보호 시간을 구현했다.
- 변경 파일:
  - `UnityProject/Assets/_Project/Scripts/Core/PrototypeConfig.cs`
  - `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionState.cs`
  - `UnityProject/Assets/_Project/Scripts/Immune/ImmuneRiskZone.cs`
  - `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
- 같은 작업 범위의 기존 변경:
  - `UnityProject/Assets/_Project/Editor/RatHostPrototypeSceneBuilder.cs`
  - `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- 판정 제외:
  - `UnityProject/ProjectSettings/ProjectSettings.asset`
  - 작업 시작 전부터 존재한 별도 dirty 변경으로 기록되어 있으므로 이번 복귀 보호 판정 대상에 포함하지 않는다.

## 판정

`내부 승인 가능`

변이 선택 후 쥐 숙주 모드로 복귀했을 때 같은 오염 구역 위에 남아 있어 즉시 면역 경계도와 숙주 피해가 다시 적용되는 문제를 막는 보정은 승인된 쥐 숙주 프로토타입 범위 안의 버그 보정으로 판단한다.

## 근거

- 승인된 1차 프로토타입 범위에는 `변이 선택 후 쥐 숙주 모드 복귀`와 `위험 행동/위험 구역으로 면역 경계도 상승`이 포함되어 있다.
- 이번 변경은 복귀 직후 위험 구역 효과를 짧게 차단하고, 보호 시간이 끝난 뒤 기존 위험 구역 효과를 다시 적용하는 처리에 한정되어 있다.
- 새 숙주, 새 스테이지, 백신/인간 단계, 캠페인, 엔딩, 신규 패키지, 신규 에셋은 추가하지 않았다.
- `RiskZoneGraceAfterMutationReturnSeconds = 1.5f`는 독립 시스템 추가가 아니라 복귀 직후 재오염을 막기 위한 프로토타입 기본값이다. 체감 길이는 후속 플레이 검증에서 조정 가능하다.
- 기존 dirty인 `ProjectSettings.asset`는 본 작업 범위 밖으로 분리되어 있고, 이번 판정 대상에 포함하지 않는다는 기록이 있다.

## 루프 게이트 확인

- 작업 배정 게이트: `task.md`에 작업 목적, 담당 에이전트, 입력 자료, 금지 범위, 수용 기준이 기록되어 있다.
- 담당 산출물 게이트: `work-log.md`와 `agent-activity.md`에 조정자 답변, 직접 보정 예외 사유, 변경 대상, 제한 범위가 기록되어 있다.
- QA/검증 게이트: QA/검증 에이전트 Euler의 별도 산출물 `artifacts/qa-return-grace-review.md`가 있고 판정은 `PASS`다.
- 총괄 관리자 게이트: 본 문서가 복귀 보호 변경에 대한 총괄 관리자 판정 산출물이다.

절차상 조정자 직접 보정은 원칙적으로 예외 처리 대상이다. 다만 같은 작업에서 구현 에이전트 지연/종료 이력과 사용자 재보고가 있었고, `agent-activity.md`에 예외 사유와 제한 범위가 기록되어 있으며, 별도 QA/검증 에이전트가 diff와 테스트 로그를 재확인했으므로 이번 내부 승인을 차단하지 않는다.

## QA/검증 기록 확인

- RED: `editmode-return-grace-red4.xml`
  - `ImmuneRiskZone_MutationReturnGraceSuppressesImmediateContaminationOnly` 실패.
  - 기대값 `25.0f`, 실제값 `37.0f`로 변이 선택 후 복귀 직후 오염 1초치가 즉시 더해지던 기존 문제를 재현했다.
- GREEN targeted: `editmode-return-grace-green.xml`
  - 신규 targeted 테스트 1개 중 1개 통과.
- GREEN full: `editmode-all-green6.xml`
  - 전체 EditMode 37개 중 37개 통과, 실패 0.
- Unity MCP:
  - Play 4초 진입/정지 후 Console Error 0 기록이 있다.
  - Edit 상태 RunCommand 기록은 `returnGraceCheck mode=RatHost alert=25 graceAfterReturn=True graceAfterTick=False`다.
- QA/검증 에이전트 Euler:
  - 대상 4개 파일 diff, RED/GREEN 로그, 전체 EditMode 로그, Unity MCP Play 스모크를 확인했고 `PASS`로 판정했다.

## 수정 필요

없음.

완료 보고에서는 다음을 주장하지 않는다.

- 실제 키보드 조작으로 오염 구역 위에서 미니게임을 끝내고 복귀하는 전체 장면을 눈으로 확인했다.
- Windows 빌드 또는 빌드 실행본에서 해당 경로를 검증했다.
- `1.5초`가 최종 밸런스 값으로 확정되었다.

## 문제 사안

차단 문제는 없다.

남은 위험은 다음과 같이 사용자 보고에 제한 사항으로 포함한다.

- 실제 키보드 조작 기반 전체 수동 플레이 장면은 미검증이다.
- Windows 빌드와 빌드 실행본 검증은 수행하지 않았다.
- 보호 시간 1.5초는 기능 완료 기준에는 충분하지만 UX 체감은 후속 플레이테스트에서 조정될 수 있다.

## 사용자 결정 필요

없음.

이번 변경은 승인된 쥐 숙주 프로토타입의 복귀 루프와 위험 구역 처리 안에 있으므로, 별도 범위 변경 승인이나 새 사용자 결정은 필요하지 않다.

## 사용자에게 올릴 확인 파일

- `UnityProject/Assets/_Project/Scripts/Core/PrototypeConfig.cs`
  - 복귀 후 위험 구역 보호 기본값이 `1.5초`로 들어갔는지 확인한다.
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionState.cs`
  - 변이 선택 후 `RatHost` 복귀 시 보호 시간이 시작되고 `TickRatMode`에서 소진되는지 확인한다.
- `UnityProject/Assets/_Project/Scripts/Immune/ImmuneRiskZone.cs`
  - `RatHost` 모드가 아니거나 복귀 보호 시간이 활성일 때 오염 노출, 숙주 피해, HUD 피드백을 적용하지 않는지 확인한다.
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
  - 복귀 직후 즉시 오염 차단과 보호 시간 이후 재적용이 회귀 테스트로 고정되었는지 확인한다.
- `_workspace/active/2026-07-06-contamination-risk-zone-trigger/artifacts/qa-return-grace-review.md`
  - RED/GREEN/전체 EditMode/Unity MCP 검증 근거와 남은 위험을 확인한다.

## 다음 단계

사용자에게는 `내부 승인 가능` 판정과 함께 미검증 항목을 제한 사항으로 보고한다. 완료 또는 커밋 단계에서는 `ProjectSettings.asset` 변경을 본 작업 범위 밖 dirty 변경으로 분리해 다룬다.
