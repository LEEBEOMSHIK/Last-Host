# ToxicWaterRiskZone 오염 노출 트리거 보정

## 작업 ID

`2026-07-06-contamination-risk-zone-trigger`

## 사용자 보고

- `ToxicWaterRiskZone`에 가도 HUD에 `오염 노출` 수치가 전혀 표시되지 않는다.

## 루프 단계

- 쥐 숙주 탐험 중 위험 구역 진입
- 위험 행동/위험 구역으로 면역 경계도 상승

## 원인 조사 기록

- `ImmuneRiskZone.ApplyExposure` 직접 호출 테스트는 존재한다.
- 현재 씬의 `ToxicWaterRiskZone`은 `BoxCollider.isTrigger = true`와 `ImmuneRiskZone`을 가진다.
- 쥐 숙주는 `CharacterController` 기반이며, `ToxicWaterRiskZone`에는 Rigidbody가 없다.
- 현재 테스트는 실제 씬 트리거 전달 조건을 보장하지 않는다.

## 작업 가설

`CharacterController`가 이동하는 쥐와 `ToxicWaterRiskZone` 트리거 사이에서 Unity 물리 트리거 이벤트가 안정적으로 전달되도록 Rigidbody 기반 트리거 구성을 명시해야 한다.

## 재보고 후 추가 가설

Rigidbody 기반 trigger 구성만으로는 실제 플레이 이동 경로에서 `OnTriggerStay`가 누락될 수 있다. `ToxicWaterRiskZone`은 trigger callback 외에도 쥐 숙주 collider bounds가 위험 구역 collider bounds와 겹치는지 확인해 오염 노출을 적용해야 한다.

## 담당 에이전트

- 게임플레이 구현 에이전트
  - `ImmuneRiskZone` 관련 회귀 테스트/필요 최소 코드 검토
- Unity 씬/통합 구현 에이전트
  - `ToxicWaterRiskZone` 씬 구성과 씬 빌더 연결 보정
- QA/검증 에이전트
  - EditMode 테스트와 원 증상 기준 검증 기록
- 프로젝트 총괄 관리자 에이전트
  - 범위, 승인 게이트, 작업 기록, 검증 기록 확인

## 입력 자료

- `AGENTS.md`
- `.agents/agent-roster.md`
- `.agents/gameplay-implementation-agent.md`
- `.agents/unity-scene-integration-agent.md`
- `.agents/qa-verification-agent.md`
- `.agents/project-director-agent.md`
- `UnityProject/Assets/_Project/Scripts/Immune/ImmuneRiskZone.cs`
- `UnityProject/Assets/_Project/Editor/RatHostPrototypeSceneBuilder.cs`
- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`

## 산출물

- 실제 씬의 `ToxicWaterRiskZone` 트리거 전달 조건을 고정하는 테스트
- `ToxicWaterRiskZone` 씬/빌더 보정
- trigger callback 없이도 bounds 겹침으로 오염 노출이 적용되는 감지 경로와 회귀 테스트
- 실제 `RatHostPrototype` 씬 배선에서 오염 구역 overlap 후 HUD objective가 `오염 노출 +6`으로 갱신되는 통합 테스트
- 소량 연속 오염 노출이 `오염 노출 +0`으로 고정되어 보이지 않도록 누적 피드백 표시 보정
- 내부 바이러스 모드에서는 오염 구역 효과가 면역 경계도, 숙주 피해, HUD 피드백을 변경하지 않도록 모드 가드 보정
- 변이 선택 후 쥐 모드로 복귀한 직후에는 짧은 시간 동안 오염/위험 구역 노출과 숙주 피해가 즉시 재적용되지 않도록 복귀 보호 보정
- 검증 기록 `verification.md`
- 총괄 관리자 판정 `director-review.md`

## 금지 범위

- 새 위험 구역 추가
- 면역 경계도 밸런스 변경
- 미니게임 난이도 변경
- 프로토타입 루프 범위 확장
- 사용자 변경사항 되돌리기

## 수용 기준

- 씬의 `ToxicWaterRiskZone`이 `오염 노출` 피드백을 발생시킬 수 있는 Unity 트리거 구성으로 보정된다.
- 실제 이동 경로에서 trigger callback이 빠져도 쥐와 오염 구역 bounds가 겹치면 `오염 노출` 피드백과 숙주 피해가 적용된다.
- 실제 씬의 세션/HUD 배선에서도 오염 구역 overlap 후 objective 문구가 `오염 노출 +n`으로 갱신된다.
- 연속 오염 노출의 프레임당 증가량이 1 미만이어도 HUD가 `오염 노출 +0`으로 고정되지 않고 누적 증가량을 보여준다.
- 내부 바이러스 미니게임 모드에서는 오염 구역 노출 호출이 상태를 변경하지 않는다.
- 내부 미니게임 성공 후 변이 선택으로 쥐 모드에 복귀한 직후에는 같은 오염 구역 위에 남아 있어도 복귀 기준 면역 경계도 25%에서 즉시 추가 상승하지 않는다.
- 복귀 보호 시간이 지난 뒤에는 같은 오염 구역 노출이 다시 면역 경계도, 숙주 피해, HUD 피드백을 정상 적용한다.
- 씬 빌더로 재생성해도 동일한 구성이 유지된다.
- `오염 노출` 피드백 라벨과 수치 계산은 기존 의도와 충돌하지 않는다.
- EditMode 테스트가 통과한다.

## 현재 주의 사항

- 작업 시작 시점에 `UnityProject/ProjectSettings/ProjectSettings.asset` 변경이 이미 존재한다.
- 해당 ProjectSettings 변경은 본 작업 범위가 아니므로 되돌리거나 커밋 대상에 포함하지 않는다.
