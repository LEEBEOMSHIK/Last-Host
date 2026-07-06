# ToxicWaterRiskZone 오염 노출/복귀 보호 완료 보고

## 작업 ID

`2026-07-06-contamination-risk-zone-trigger`

## 완료 요약

`ToxicWaterRiskZone`의 오염 노출이 실제 씬/HUD에 반영되지 않던 문제를 보정했고, 내부 바이러스 모드와 변이 선택 후 쥐 모드 복귀 직후에 오염 효과가 새지 않도록 방어했다.

## 변경 파일

- `UnityProject/Assets/_Project/Editor/RatHostPrototypeSceneBuilder.cs`
- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeConfig.cs`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionState.cs`
- `UnityProject/Assets/_Project/Scripts/Immune/ImmuneRiskZone.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`

## 핵심 변경

- `ToxicWaterRiskZone`에 kinematic `Rigidbody` 구성을 추가했다.
- `ImmuneRiskZone`이 trigger callback 외에 bounds overlap으로도 쥐 오염 노출을 감지한다.
- 같은 프레임의 trigger/polling 중복 적용을 막았다.
- 같은 원인의 소량 연속 오염 피드백을 누적해 `오염 노출 +0` 고정을 피했다.
- `InternalVirus` 모드에서는 오염 노출, 숙주 피해, HUD 피드백이 적용되지 않도록 했다.
- 변이 선택 후 `RatHost` 복귀 직후 1.5초 동안 위험 구역 효과를 무시한다.

## 검증

- RED: `editmode-return-grace-red4.xml`
  - 변이 선택 후 즉시 오염이 더해져 `25 -> 37`이 되는 기존 문제 재현.
- GREEN targeted: `editmode-return-grace-green.xml`
  - 복귀 보호 테스트 1개 중 1개 통과.
- GREEN full: `editmode-all-green6.xml`
  - 전체 EditMode 37개 중 37개 통과.
- Unity MCP:
  - Play 4초 진입/정지 후 Console Error 0.
  - Edit 상태에서 `returnGraceCheck mode=RatHost alert=25 graceAfterReturn=True graceAfterTick=False` 확인.
- QA/검증 에이전트 Euler:
  - `artifacts/qa-return-grace-review.md`에서 PASS.
- 프로젝트 총괄 관리자 에이전트 Ampere:
  - `artifacts/director-return-grace-review.md`에서 `내부 승인 가능`.
- 커밋 직전 재검증:
  - `editmode-all-precommit.xml`에서 전체 EditMode 37개 중 37개 통과.
  - `git diff --check` 종료 코드 0. CRLF 변환 경고만 확인.

## 미검증/남은 위험

- 실제 키보드 조작으로 오염 구역 위에서 미니게임 완료 후 복귀하는 전체 장면을 눈으로 확인하지는 않았다.
- Windows 빌드와 빌드 실행본 검증은 수행하지 않았다.
- 보호 시간 `1.5초`는 후속 플레이 검증에서 체감 조정될 수 있다.

## 범위 밖 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`는 작업 시작 전부터 존재한 별도 dirty 변경으로, 이번 작업 완료/커밋 대상에서 제외한다.
