# 작업 패킷: 오염/조직 자극 위험 구역

## 작업 ID

`2026-07-03-contamination-risk-zone`

## 목적

쥐 숙주 프로토타입에서 `더러운 곳에 가면 면역 경계도가 오른다`는 규칙을 배경 자동 상승이 아니라 명확한 접촉/체류 위험으로 검증한다.

현재 씬에는 `ToxicWaterRiskZone`과 `ImmuneRiskZone`이 이미 존재한다. 이번 작업은 새 시스템을 크게 추가하지 않고, 기존 오염 위험 구역이 면역 경계도 원인 피드백 규칙을 따르도록 보정한다.

## 루프 단계

- 쥐 숙주 모드 탐험
- 위험 환경 접촉/체류
- 면역 경계도 상승 원인 피드백

## 입력 자료

- `docs/prototype/official/rat-host-prototype.md`
- `docs/design/systems/immune-alert.md`
- `docs/design/ui-feedback/immune-alert-feedback.md`
- `docs/prototype/plans/rat-host-immune-alert-work-direction.md`
- `UnityProject/Assets/_Project/Scripts/Immune/ImmuneRiskZone.cs`
- `UnityProject/Assets/_Project/Editor/RatHostPrototypeSceneBuilder.cs`
- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`

## 구현 범위

- `ImmuneRiskZone`이 면역 경계도 상승 시 원인 라벨을 전달하게 한다.
- 기본 피드백 라벨은 `오염 노출`로 둔다.
- 기존 수치 `alertPerSecond = 12`, `hostDamagePerSecond = 4`는 유지한다.
- Scene Builder로 생성되는 `ToxicWaterRiskZone`에도 같은 기본 라벨이 들어가야 한다.
- 현재 씬의 `ToxicWaterRiskZone` 직렬화 값도 같은 라벨을 가져야 한다.
- EditMode 테스트로 접촉/체류 위험이 피드백을 기록하고, 비쥐 콜라이더는 무시함을 확인한다.

## 금지 범위

- 시간 경과 기본 면역 경계도 상승 재활성화 금지.
- 새 내부 미니게임 유형 추가 금지.
- 독성 물웅덩이 외 다수 위험 구역 추가 금지.
- 숙주 본능/강제 조종 입력 방해 구현 금지.
- 보상/실패 규칙 코드 구현 금지.

## 수용 기준

- 오염 구역 밖에서는 기존처럼 경계도가 오르지 않는다.
- 쥐가 `ImmuneRiskZone` 안에 머물면 면역 경계도와 숙주 피해가 오른다.
- 쥐가 오염 구역에 머무른 직후 HUD 피드백 구조가 `오염 노출 +<수치>`를 표시할 수 있다.
- `ImmuneRiskZone`은 비쥐 콜라이더에는 반응하지 않는다.
- 기존 소음 배관 피드백 테스트는 계속 통과한다.
- QA/검증 에이전트와 프로젝트 총괄 관리자 판정을 남긴다.

## 담당 에이전트

- 게임플레이 구현 에이전트: `ImmuneRiskZone` 동작과 EditMode 테스트 보정.
- Unity 씬 구현 에이전트: Scene Builder와 현재 씬 직렬화 값 확인/보정.
- QA/검증 에이전트: 테스트, diff, 씬 반영 상태 검증.
- 프로젝트 총괄 관리자: 산출물 승인 여부 최종 판정.

## 사용자 확인 필요 항목

이번 작업은 이미 승인된 쥐 숙주 프로토타입 범위의 작은 보정으로 진행한다. 아래 항목은 이번 작업에서 다루지 않고, 별도 승인 전까지 보류한다.

- 오염 구역 수 추가
- 경계도/피해 수치 재밸런싱
- 새로운 시각 에셋 제작
- 내부 미니게임 난이도 연동 구현
