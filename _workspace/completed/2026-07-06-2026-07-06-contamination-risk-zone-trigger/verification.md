# QA/검증 기록

## 검증 대상

`ToxicWaterRiskZone` 진입 시 HUD에 `오염 노출` 수치가 표시되지 않던 증상에 대해, 실제 씬과 씬 빌더의 위험 구역 트리거 구성이 `CharacterController` 기반 쥐 숙주와 Unity 트리거 이벤트를 전달할 수 있는 형태로 보정되었는지 확인한다.

## 작업영역

`_workspace/active/2026-07-06-contamination-risk-zone-trigger`

## 검토한 변경

- `UnityProject/Assets/_Project/Scripts/Immune/ImmuneRiskZone.cs`
  - `OnTriggerStay`만 사용하던 오염 노출 감지에 `Update` 기반 bounds overlap 감지 경로가 추가되었다.
  - 같은 프레임에 trigger callback과 bounds polling이 모두 감지되어도 노출이 중복 적용되지 않도록 frame guard가 추가되었다.
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
  - `RatHostPrototypeScene_ToxicWaterRiskZoneSupportsCharacterControllerTriggerDelivery` 테스트가 추가되었다.
  - 테스트는 현재 씬의 쥐에 `CharacterController`가 있고, `ToxicWaterRiskZone`에 `ImmuneRiskZone`, trigger `BoxCollider`, `Rigidbody`가 있으며, Rigidbody가 `isKinematic = true`, `useGravity = false`인지 확인한다.
  - `ImmuneRiskZone_OverlappingRatBoundsStoresContaminationFeedbackWithoutTriggerCallback` 테스트가 추가되었다.
  - 테스트는 trigger callback 없이도 쥐 `CharacterController` bounds와 오염 구역 bounds가 겹치면 `오염 노출 +6` 피드백과 숙주 피해가 적용되는지 확인한다.
  - `RatHostPrototypeScene_ToxicWaterRiskZoneOverlapRaisesHudFeedback` 테스트가 추가되었다.
  - 테스트는 실제 `RatHostPrototype` 씬에서 쥐를 `ToxicWaterRiskZone` 중심으로 배치하고, 오염 노출 적용 후 HUD objective가 `오염 노출 +6`으로 갱신되는지 확인한다.
- `UnityProject/Assets/_Project/Editor/RatHostPrototypeSceneBuilder.cs`
  - 씬 빌더가 `ToxicWaterRiskZone` 생성 직후 `Rigidbody`를 추가하고 `isKinematic = true`, `useGravity = false`로 설정한다.
- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
  - 현재 씬의 `ToxicWaterRiskZone` GameObject에 `Rigidbody` 컴포넌트가 추가되었고, `m_IsKinematic: 1`, `m_UseGravity: 0`으로 직렬화되어 있다.
- `UnityProject/ProjectSettings/ProjectSettings.asset`
  - 본 작업 범위가 아니므로 검증/판정 대상에서 제외했다.

## 실행한 검증

- 변경 diff 확인
  - 명령: `git diff -- UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
  - 명령: `git diff -- UnityProject/Assets/_Project/Editor/RatHostPrototypeSceneBuilder.cs`
  - 명령: `git diff -- UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
  - 결과: 테스트, 씬 빌더, 현재 씬 모두 `ToxicWaterRiskZone`의 Rigidbody 기반 trigger 전달 조건을 고정하는 변경으로 확인했다.
  - 해석: 변경 범위는 사용자 보고 증상과 연결된 기존 위험 구역 구성 보정에 한정된다.

- RED 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-toxic-red.xml`
  - 결과: 신규 테스트 1개 중 0개 통과, 1개 실패. 실패 위치는 `RatHostPrototypeCoreTests.cs:642`이며 `ToxicWaterRiskZone`의 `Rigidbody`가 null인 상태를 재현했다.
  - 해석: 기존 씬 구성에서 `CharacterController` 기반 트리거 전달 조건이 누락되어 있음을 회귀 테스트로 재현했다.

- GREEN targeted 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-toxic-green.xml`
  - 결과: `RatHostPrototypeScene_ToxicWaterRiskZoneSupportsCharacterControllerTriggerDelivery` 1개 중 1개 통과.
  - 해석: 현재 씬의 `ToxicWaterRiskZone` Rigidbody 구성 보정이 신규 회귀 테스트를 만족한다.

- GREEN full 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-all-green.xml`
  - 결과: EditMode 전체 32개 중 32개 통과, 실패 0개.
  - 해석: 신규 보정으로 기존 EditMode 회귀 테스트가 깨지지 않았다.

- 사용자 재보고 후 RED 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-toxic-overlap-red.log`
  - 결과: `ImmuneRiskZone_OverlappingRatBoundsStoresContaminationFeedbackWithoutTriggerCallback` 컴파일 실패.
  - 실패 사유: `ImmuneRiskZone`에 `ApplyOverlappingRatExposure` 정의가 없음.
  - 해석: trigger callback 없이 bounds 기반 감지 경로가 필요하다는 요구를 RED로 고정했다.

- 사용자 재보고 후 GREEN targeted 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-toxic-overlap-green.xml`
  - 결과: `ImmuneRiskZone_OverlappingRatBoundsStoresContaminationFeedbackWithoutTriggerCallback` 1개 중 1개 통과.
  - 해석: bounds overlap 감지 경로가 쥐 `CharacterController` 기반 오염 노출 적용을 수행한다.

- 사용자 재보고 후 GREEN full 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-all-green2.xml`
  - 결과: EditMode 전체 33개 중 33개 통과, 실패 0개.
  - 해석: bounds overlap 보강 후에도 기존 프로토타입 회귀 테스트가 깨지지 않았다.

- 실제 씬-HUD targeted 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-toxic-scene-hud-green.xml`
  - 결과: `RatHostPrototypeScene_ToxicWaterRiskZoneOverlapRaisesHudFeedback` 1개 중 1개 통과.
  - 해석: 실제 씬 배선 기준으로 오염 구역 overlap이 세션 값과 HUD objective `오염 노출 +6` 갱신까지 이어진다.

- 최종 GREEN full 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-all-green3.xml`
  - 결과: EditMode 전체 34개 중 34개 통과, 실패 0개.
  - 해석: 실제 씬-HUD 통합 테스트 추가 후에도 전체 EditMode 회귀 테스트가 깨지지 않았다.

- Unity MCP Play 스모크 확인
  - 자료: Unity MCP `Unity_ManageEditor`, `Unity_ReadConsole`
  - 결과: Play 진입, 4초 대기, Stop 완료, 콘솔 Error 0.
  - 해석: 씬이 Play 진입 시 즉시 오류를 내지는 않는 것으로 확인했다. 단, 이 검증은 실제 쥐 이동과 HUD 수치 증가 확인을 포함하지 않는다.

- 최종 Unity MCP Play 스모크 확인
  - 자료: Unity MCP `Unity_ManageEditor`, `Unity_ReadConsole`
  - 결과: 최종 테스트/문서 갱신 후 원본 에디터에서 Play 진입, 4초 대기, Stop 완료, 콘솔 Error 0.
  - 해석: 현재 작업트리 상태에서 에디터 Play 진입 오류는 확인되지 않았다.

- QA/검증 에이전트 Unity MCP 재검증
  - 수행 에이전트: Volta (`019f35a5-5a4b-7ac2-8698-f2f15422318e`)
  - 배경: 이전 QA 에이전트 Wegener는 read-only 검토였고 Unity MCP를 직접 사용하지 않았으므로, 사용자 지적에 따라 재검증했다.
  - 실행한 MCP 호출: `Unity_ManageEditor GetState`, `Unity_ManageEditor GetProjectRoot`, `Unity_ReadConsole Clear`, `Unity_ManageEditor Play`, 4초 대기, `Unity_ManageEditor Stop`, `Unity_ReadConsole Error`, Edit 상태 `Unity_RunCommand`.
  - 결과: PASS.
  - 확인 내용: Unity 프로젝트 루트가 `C:/project/Last-Host/UnityProject`이며, Play 4초 진입/정지 후 Console Error 0. 현재 씬 `Assets/_Project/Scenes/RatHostPrototype.unity`에서 `ToxicWaterRiskZone`, `ImmuneRiskZone`, session 연결, trigger collider, `RatHostController`, `CharacterController`, `PrototypeHud`, `PrototypeSessionController` 존재 확인.
  - 제한: QA 에이전트도 키보드 직접 조작으로 HUD 숫자가 오르는 장면을 눈으로 확인하지는 않았다.

- 소량 연속 피드백 RED 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-feedback-small-delta-red.xml`
  - 결과: `Session_ContinuousImmuneAlertFeedbackAccumulatesSmallSameCauseDeltas` 1개 중 0개 통과, 1개 실패.
  - 실패 사유: `0.04 + 0.04` 연속 오염 노출이 `0.08`로 누적되어야 하지만 기존 구현은 마지막 값 `0.04`만 저장했다.
  - 해석: `오염 노출 +0`으로 보일 수 있는 표시 원인을 RED로 재현했다.

- 소량 연속 피드백 GREEN targeted 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-feedback-small-delta-green.xml`
  - 결과: `Session_ContinuousImmuneAlertFeedbackAccumulatesSmallSameCauseDeltas` 1개 중 1개 통과.
  - 해석: 같은 원인 라벨의 소량 연속 증가가 누적되고, `오염 노출 +0.08`처럼 표시된다.

- 최종 GREEN full 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-all-green4.xml`
  - 결과: EditMode 전체 35개 중 35개 통과, 실패 0개.
  - 해석: `+0` 표시 보정 후에도 전체 EditMode 회귀 테스트가 깨지지 않았다.

- QA/검증 에이전트 소량 피드백 Unity MCP 검증
  - 수행 에이전트: Fermat (`019f35f9-b89c-7250-b4bb-3f87f98dc235`)
  - 실행한 MCP 호출: `Unity_ManageEditor GetState`, `Unity_ReadConsole Clear`, `Unity_ManageEditor Play`, 4초 대기, `Unity_ManageEditor Stop`, `Unity_ReadConsole Error`, Edit 상태 `Unity_RunCommand`, 재확인 `Unity_ReadConsole Error`.
  - 결과: PASS.
  - 확인 내용: Play 4초 진입/정지 후 Console Error 0, `PrototypeSessionState`에 `오염 노출` 0.04 + 0.04를 연속 적용했을 때 결과 텍스트가 `오염 노출 +0.08`임을 확인했다. 현재 씬의 `ToxicWaterRiskZone`, `ImmuneRiskZone`, session 연결, trigger collider, `RatHostController`, `CharacterController`, `PrototypeHud`, `PrototypeSessionController` 존재도 확인했다.
  - 제한: QA 에이전트도 실제 키 입력으로 쥐를 움직여 화면 HUD가 바뀌는 장면을 시각적으로 확인하지는 않았다.

- 내부 바이러스 모드 오염 효과 RED 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-internal-exposure-red.xml`
  - 결과: `ImmuneRiskZone_InternalVirusModeExposureDoesNotChangeImmuneAlertOrHostHealth` 1개 중 0개 통과, 1개 실패.
  - 실패 사유: `InternalVirus` 모드에서 오염 노출 호출 시 면역 경계도는 0으로 유지되었지만 숙주 생명력이 100에서 96으로 감소했다.
  - 해석: 내부 바이러스 모드에서 오염 구역 효과 일부가 새는 문제를 RED로 재현했다.

- 내부 바이러스 모드 오염 효과 GREEN targeted 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-internal-exposure-green.xml`
  - 결과: `ImmuneRiskZone_InternalVirusModeExposureDoesNotChangeImmuneAlertOrHostHealth` 1개 중 1개 통과.
  - 해석: 내부 바이러스 모드에서는 오염 구역 호출이 면역 경계도, 숙주 생명력, HUD 피드백을 변경하지 않는다.

- 최종 GREEN full 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-all-green5.xml`
  - 결과: EditMode 전체 36개 중 36개 통과, 실패 0개.
  - 해석: 내부 바이러스 모드 가드 보정 후에도 전체 EditMode 회귀 테스트가 깨지지 않았다.

- QA/검증 에이전트 내부 바이러스 모드 Unity MCP 검증
  - 수행 에이전트: Faraday (`019f360d-5274-7c40-b34c-cd86ada1d954`)
  - 실행한 MCP 호출: `Unity_ManageEditor GetState`, `Unity_ReadConsole Clear`, `Unity_ManageEditor Play`, 4초 대기, `Unity_ManageEditor Stop`, `Unity_ReadConsole Error`, `Unity_ManageEditor GetState`, Edit 상태 `Unity_RunCommand`, 최종 `Unity_ReadConsole Error`.
  - 결과: PASS.
  - 확인 내용: Play 스모크 Console Error 0. `InternalVirus` 모드에서 `ImmuneRiskZone.ApplyExposure` 직접 호출 후 `mode=InternalVirus`, `alert=0`, `health=100`, `feedback=''` 유지 확인.
  - 제한: 미니게임 종료 후 `RatHost`로 돌아왔을 때 쥐가 여전히 오염 구역 위에 있으면 그때부터 다시 오염 노출이 적용되는 것은 별도 현상이다.

- 복귀 후 위험 구역 보호 RED 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-return-grace-red4.xml`
  - 결과: `ImmuneRiskZone_MutationReturnGraceSuppressesImmediateContaminationOnly` 1개 중 0개 통과, 1개 실패.
  - 실패 사유: 변이 선택 후 복귀 기준 면역 경계도 25를 기대했지만 기존 구현은 오염 1초치가 즉시 더해져 37이 되었다.
  - 해석: 복귀 직후 같은 오염 구역 위에 남아 있을 때 즉시 재노출되는 문제를 RED로 재현했다.
  - 환경 참고: `editmode-return-grace-red.log`, `editmode-return-grace-red2.log`는 임시 Unity 복사본의 `com.unity.ai.assistant` PackageCache 권한 오류로 테스트 전 종료되었고, `editmode-return-grace-red3.log`는 최초 에셋 임포트 타임아웃으로 테스트 전 종료되어 완료 근거로 사용하지 않는다.

- 복귀 후 위험 구역 보호 GREEN targeted 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-return-grace-green.xml`
  - 결과: `ImmuneRiskZone_MutationReturnGraceSuppressesImmediateContaminationOnly` 1개 중 1개 통과.
  - 해석: 변이 선택 후 쥐 모드 복귀 직후에는 오염 노출/숙주 피해/피드백이 적용되지 않고, 보호 시간이 지난 뒤에는 다시 정상 적용된다.

- 최종 GREEN full 확인
  - 자료: `_workspace/active/2026-07-06-contamination-risk-zone-trigger/editmode-all-green6.xml`
  - 결과: EditMode 전체 37개 중 37개 통과, 실패 0개.
  - 해석: 복귀 보호 보정 후에도 기존 오염 HUD, 내부 바이러스 모드 가드, 씬-HUD 테스트, 카메라/입력 테스트가 깨지지 않았다.

- Unity MCP 복귀 보호 상태 검증
  - 실행한 MCP 호출: `Unity_ManageEditor GetState`, `Unity_ReadConsole Get`, `Unity_ReadConsole Clear`, `Unity_ManageEditor Play`, 4초 대기, `Unity_ManageEditor Stop`, `Unity_ReadConsole Error`, Edit 상태 `Unity_RunCommand`, 최종 `Unity_ReadConsole Error`.
  - 결과: PASS.
  - 확인 내용: Play 4초 진입/정지 후 Console Error 0. Edit 상태 `Unity_RunCommand`에서 `returnGraceCheck mode=RatHost alert=25 graceAfterReturn=True graceAfterTick=False` 확인.
  - 제한: 이 MCP command는 순수 세션 상태 검증이며, 실제 키 입력으로 쥐를 움직여 미니게임을 끝낸 장면을 시각 확인한 것은 아니다.

- 커밋 직전 전체 EditMode 재검증
  - 자료: `_workspace/completed/2026-07-06-2026-07-06-contamination-risk-zone-trigger/editmode-all-precommit.xml`
  - 결과: EditMode 전체 37개 중 37개 통과, 실패 0개.
  - 해석: 완료 폴더 이동과 최종 기록 정리 후에도 Unity EditMode 회귀 테스트가 통과했다.

- 커밋 직전 공백 검증
  - 명령: `git diff --check`
  - 결과: 종료 코드 0. CRLF 변환 경고만 확인.
  - 해석: 공백 오류는 확인되지 않았다.

- Unity MCP 실제 이동 검증 시도
  - 방법: Play 중 `Unity_RunCommand`로 쥐를 `ToxicWaterRiskZone` 위치로 이동시키고 세션/HUD 상태를 읽으려 했다.
  - 결과: 동적 command 실행 후 Play가 일시적으로 pause 상태가 되었고 `PrototypeSessionController.State`가 null인 오류가 발생했다.
  - 처리: 해당 시도는 검증 근거로 채택하지 않았고, Play를 중지한 뒤 콘솔을 비우고 기본 Play 스모크를 다시 수행했다.
  - 재확인 결과: 기본 Play 진입, 4초 대기, 콘솔 Error 0, Stop 완료.

- `git diff --check`
  - 명령: `git diff --check`
  - 결과: 종료 코드 0. `RatHostPrototypeSceneBuilder.cs`, `RatHostPrototypeCoreTests.cs`의 CRLF 변환 경고만 표시됨.
  - 해석: 공백 오류는 확인되지 않았다.

## 결과

- RED 자료가 `Rigidbody` null로 기존 누락 상태를 재현한다.
- GREEN targeted와 GREEN full 자료가 현재 변경 후 씬 구성과 전체 EditMode 회귀 테스트 통과를 확인한다.
- 사용자 재보고 후 RED/GREEN 자료가 `OnTriggerStay` 누락을 보완하는 bounds overlap 감지 경로를 확인한다.
- 실제 씬-HUD targeted 자료가 `ToxicWaterRiskZone` overlap 후 HUD objective가 `오염 노출 +6`으로 갱신되는 것을 확인한다.
- 소량 연속 피드백 자료가 `오염 노출 +0` 표시 문제를 재현하고, 보정 후 `오염 노출 +0.08`로 표시됨을 확인한다.
- 내부 바이러스 모드 자료가 미니게임 중 오염 구역 효과가 면역 경계도, 숙주 생명력, HUD 피드백에 새지 않음을 확인한다.
- 복귀 후 위험 구역 보호 자료가 변이 선택 직후 오염 재노출을 막고, 보호 시간 이후에는 오염 위험이 다시 정상 적용됨을 확인한다.
- 최신 전체 EditMode 기준은 37개 중 37개 통과다.
- 커밋 직전 재검증 기준도 37개 중 37개 통과다.
- 씬 빌더와 현재 씬 양쪽에 동일한 Rigidbody 설정이 반영되어, 씬 재생성 시에도 `ToxicWaterRiskZone` trigger 전달 구성이 유지될 것으로 판단한다.
- `오염 노출` 피드백 라벨과 `ImmuneRiskZone` 수치 설정은 변경 diff에서 바뀌지 않았다.

## 미검증 항목

- 실제 사람이 키보드로 쥐를 `ToxicWaterRiskZone`까지 이동시키는 장면을 눈으로 확인하지는 못했다.
- Unity MCP 동적 command 방식의 강제 이동 검증은 Play 상태를 손상시켜 신뢰 가능한 결과로 채택하지 않았다.
- 미니게임 종료 후 `RatHost`로 복귀했을 때 쥐가 여전히 오염 구역 위에 있으면 1.5초 보호 시간이 지난 뒤 다시 면역 경계도가 오르는 동작은 의도적으로 유지했다.
- Windows 빌드와 빌드 실행본 검증은 수행하지 않았다.

## 남은 위험

- 자동화 테스트는 trigger callback 없이도 bounds 겹침으로 세션 값과 HUD objective가 갱신되는 경로를 실제 씬 기준으로 검증하지만, 실제 키보드 조작 장면을 눈으로 확인한 것은 아니다.
- Play 스모크는 오류 없는 진입 확인에 한정되며, 원 증상의 최종 체감 확인은 별도 수동 플레이 검증이 필요하다.
- 실제 이동 검증은 플레이어 입력 또는 별도 PlayMode 테스트로 다시 확인하는 편이 안전하다.
- 복귀 보호 시간 1.5초는 프로토타입 기본값이므로 플레이 감각에 따라 후속 밸런스 조정이 필요할 수 있다.

## 완료 판단

PASS

## 완료 판단 근거

- 본 작업의 보정 범위인 `ToxicWaterRiskZone`의 Rigidbody 기반 trigger 전달 구성은 RED에서 누락을 재현했고 GREEN targeted에서 통과했다.
- 사용자 재보고 후 bounds overlap 감지 경로는 RED에서 미구현을 확인했고 GREEN targeted에서 통과했다.
- 실제 씬-HUD targeted 테스트가 `오염 노출 +6` HUD objective 갱신을 확인했다.
- 소량 연속 피드백 targeted 테스트가 `오염 노출 +0.08` 표시를 확인했다.
- 내부 바이러스 모드 targeted 테스트가 미니게임 중 오염 구역 효과 누수를 차단했음을 확인했다.
- 복귀 후 위험 구역 보호 targeted 테스트가 변이 선택 직후 즉시 재노출 방지와 보호 시간 이후 정상 재노출을 확인했다.
- EditMode 전체 최신 기준 37/37이 통과했다.
- 현재 씬과 씬 빌더가 동일한 Rigidbody 설정을 가진다.
- Play 스모크에서 콘솔 Error 0으로 진입/정지가 완료되었다.
- 최종 Play 스모크에서도 콘솔 Error 0으로 진입/정지가 완료되었다.
- QA/검증 에이전트 Volta가 Unity MCP를 직접 사용해 프로젝트 루트, Play 스모크, 콘솔 Error 0, 현재 씬 배선을 확인했다.
- QA/검증 에이전트 Fermat가 Unity MCP를 직접 사용해 `오염 노출 +0.08` 상태 계산과 Play 스모크, 현재 씬 배선을 확인했다.
- QA/검증 에이전트 Faraday가 Unity MCP를 직접 사용해 `InternalVirus` 모드 오염 노출 호출 후 `alert=0`, `health=100`, `feedback=''` 유지와 Play 스모크를 확인했다.
- 조정자가 Unity MCP를 직접 사용해 복귀 보호 상태가 `alert=25`, `graceAfterReturn=True`, `graceAfterTick=False`로 전환되는 것을 확인했다. 별도 QA/검증 에이전트 Euler 재검증은 `artifacts/qa-return-grace-review.md`에 기록한다.
- 단, PASS는 트리거 구성 보정, bounds overlap 보강, 실제 씬-HUD 자동화 검증, 소량 피드백 표시 검증, 내부 바이러스 모드 누수 차단, 스모크 검증 기준의 판단이며, 키보드 직접 조작 장면을 눈으로 확인했다는 의미는 아니다.
