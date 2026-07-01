# 작업 배정서

## 기본 정보

- 작업 ID: 2026-07-01-rat-host-full-play-verification
- 작업명: 쥐 숙주 프로토타입 전체 플레이 검증
- 상태: 진행 중
- 생성일: 2026-07-01
- 담당 에이전트: QA/검증 에이전트
- 보조 에이전트: 프로젝트 총괄 관리자 에이전트
- 사용 스킬: unity-verification-runner, verification-before-completion

## 목적

쥐 숙주 프로토타입이 현재 구현 상태에서 플레이어블 검증 기준을 얼마나 충족하는지 확인한다. 이전 검증에서 미검증으로 남은 실제 입력, 충돌 기반 루프, 실패/재시도, 빌드 실행본 검증을 가능한 범위에서 실행하고, 미검증 항목은 명확히 분리한다.

## 입력 자료

- 사용자 요청: “1번부터 작업 진행”
- `AGENTS.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `_workspace/completed/2026-06-30-rat-host-play-verification/verification.md`
- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- `UnityProject/Assets/_Project/Scripts/`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`

## 해야 할 일

1. Unity Editor와 씬 상태를 확인한다.
2. EditMode 핵심 테스트 또는 동등한 Unity MCP 테스트를 실행한다.
3. Play 모드에서 쥐 이동, 카메라, 면역 경계도, 위험 요소, 모드 전환을 확인한다.
4. 내부 바이러스 미니게임에서 이동, 백혈구 충돌, 변이 조각 수집, 성공 루프를 확인한다.
5. 실패 후 재시도 루프를 확인한다.
6. 변이 선택 후 쥐 모드 복귀와 변이 효과를 확인한다.
7. Windows PC 빌드를 실행하고 빌드 산출물을 확인한다.
8. 실행한 검증과 미검증 항목을 `verification.md`에 기록한다.
9. QA/검증 에이전트 판정과 프로젝트 총괄 관리자 판정을 받는다.

## 산출물

- `verification.md`
- `agent-activity.md`
- `work-log.md`
- `completion-report.md`
- 필요한 경우 `artifacts/` 아래 검증 로그 요약

## 금지 범위

- 코드, 씬, ProjectSettings 수정
- 새 기능 구현
- 패키지 추가
- 범위 밖 콘텐츠 추가
- 검증하지 않은 항목을 통과로 기록

## 승인 필요 항목

- 없음. 승인된 쥐 숙주 1차 프로토타입 범위 안의 검증 작업이다.

## 완료 기준

- 실행 가능한 검증 항목이 실제로 실행되어 결과가 기록된다.
- 실패 또는 미검증 항목이 통과 항목과 분리된다.
- QA/검증 에이전트가 검증 기록을 판정한다.
- 프로젝트 총괄 관리자 에이전트가 완료 보고 가능 여부를 판정한다.
