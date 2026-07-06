# 복귀 보호 시간 QA 검토

## 판정

PASS

변이 선택 후 `RatHost`로 복귀한 직후 짧은 오염/위험 구역 보호 시간이 적용되고, 보호 시간이 지난 뒤 같은 위험 구역 효과가 다시 면역 경계도와 숙주 피해를 적용한다는 기준은 완료 판정 가능하다.

## 검증 대상 주장

- 내부 미니게임/변이 선택 후 `RatHost` 복귀 시 `AlertAfterMutationReturn = 25` 상태에서 즉시 오염 노출/숙주 피해가 추가 적용되지 않는다.
- 복귀 보호 시간이 지난 뒤에는 같은 `ToxicWaterRiskZone` 효과가 다시 정상 적용된다.
- 내부 바이러스 모드 오염 누수 방지와 기존 오염 HUD 피드백 회귀가 깨지지 않는다.

## 확인한 변경

- `UnityProject/Assets/_Project/Scripts/Core/PrototypeConfig.cs`
  - `RiskZoneGraceAfterMutationReturnSeconds = 1.5f` 기본값 추가 확인.
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionState.cs`
  - 변이 선택 후 `RatHost` 복귀 시 위험 구역 보호 시간이 시작된다.
  - `TickRatMode`에서 보호 시간이 감소한다.
  - `EnterVirusMinigame` 시 보호 시간이 정리된다.
  - `IsRatHostRiskZoneGraceActive`로 현재 보호 상태를 노출한다.
- `UnityProject/Assets/_Project/Scripts/Immune/ImmuneRiskZone.cs`
  - 세션/상태가 없거나, 현재 모드가 `RatHost`가 아니거나, 복귀 보호 시간이 활성화되어 있으면 오염 노출을 적용하지 않는다.
  - 보호가 끝난 뒤에는 기존 `AddImmuneAlertAmount`와 `DamageHost` 경로를 그대로 사용한다.
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
  - `ImmuneRiskZone_MutationReturnGraceSuppressesImmediateContaminationOnly`가 추가되어 즉시 차단과 보호 종료 후 재적용을 함께 검증한다.
  - 기존 내부 바이러스 모드 오염 누수 방지, 소량 오염 HUD 피드백, 실제 씬-HUD 오염 피드백 테스트가 전체 EditMode에 포함되어 유지된다.

## 실행/확인한 검증

- 대상 4개 파일의 `git diff` 확인.
  - 복귀 보호 시간 추가, 위험 구역 적용 가드, 신규 회귀 테스트가 수용 기준과 직접 연결됨을 확인했다.
- RED 로그 확인: `editmode-return-grace-red4.xml`
  - `ImmuneRiskZone_MutationReturnGraceSuppressesImmediateContaminationOnly` 1개 실패.
  - 기대값 `25.0f`, 실제값 `37.0f`로, 복귀 직후 오염 1초치가 즉시 더해지던 기존 문제를 재현했다.
- GREEN targeted 로그 확인: `editmode-return-grace-green.xml`
  - 신규 targeted 테스트 1/1 통과.
- GREEN full 로그 확인: `editmode-all-green6.xml`
  - 전체 EditMode 37/37 통과, 실패 0.
  - 전체 목록에 복귀 보호 테스트, 내부 바이러스 모드 오염 누수 방지 테스트, 실제 씬-HUD 오염 피드백 테스트, 소량 오염 피드백 누적 테스트가 포함되어 통과했음을 확인했다.
- Unity MCP 직접 확인.
  - `Unity_ManageEditor GetState`: Play 아님, Pause 아님, Compile/Update 아님.
  - `Unity_ManageEditor GetProjectRoot`: `C:/project/Last-Host/UnityProject`.
  - `Unity_ManageScene GetActive`: `Assets/_Project/Scenes/RatHostPrototype.unity`.
  - `Unity_ReadConsole Clear` 후 `Unity_ManageEditor Play`, 4초 대기, `Unity_ManageEditor Stop`.
  - `Unity_ReadConsole Error`: Console Error 0건.
  - Play 중 `Unity_RunCommand`는 사용하지 않았다.
- 기존 작업 로그 확인.
  - Edit 상태 RunCommand 기록 `returnGraceCheck mode=RatHost alert=25 graceAfterReturn=True graceAfterTick=False`가 남아 있음을 확인했다.

## 남은 위험

- 이번 QA 패스에서 실제 키보드 입력으로 쥐를 오염 구역 위에 둔 채 미니게임 완료 후 복귀하는 전체 수동 플레이 장면을 눈으로 확인하지는 않았다.
- Windows 빌드와 빌드 실행본 검증은 수행하지 않았다.
- `RiskZoneGraceAfterMutationReturnSeconds = 1.5f`의 체감 길이는 기능 완료 기준에는 충분하지만, 최종 UX 밸런스는 별도 플레이 검증에서 조정 여지가 있다.
- 작업트리에는 이번 요청의 4개 파일 외에도 이전 단계의 씬/빌더/ProjectSettings 변경이 존재한다. 본 QA는 지정된 복귀 보호 시간 변경과 관련 검증 로그 기준으로 판정했다.

## 완료 판단

완료 가능.

근거는 RED가 기존 즉시 재오염 문제를 재현했고, GREEN targeted가 동일 수용 기준을 통과했으며, 전체 EditMode 37/37과 Unity MCP Play 스모크 Console Error 0으로 관련 회귀가 확인되었기 때문이다.
