# 작업 배정서

## 기본 정보

- 작업 ID: 2026-07-07-host-instinct-control-spike
- 작업명: 숙주 본능, 강제 조종 스파이크, 실패 복귀와 검증 운영 보강
- 상태: 완료 가능
- 생성일: 2026-07-07
- 담당 에이전트: 게임플레이 구현 에이전트
- 보조 에이전트: Unity 씬/통합 구현 에이전트, QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: superpowers:brainstorming, superpowers:test-driven-development, rat-host-loop-builder, last-host-design-keeper, unity-verification-runner

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| 프로젝트 조정 에이전트 | 조정/통합 | 범위 확인, 작업 패킷 생성, 결과 통합 | 작업 배정서, 작업 로그, 최종 보고 |
| 게임플레이 구현 에이전트 | 코드/테스트 구현 | 강제 조종 스파이크 상태 모델과 EditMode 테스트 | C# 코드 변경, RED/GREEN 테스트 결과 |
| Unity 씬/통합 구현 에이전트 | 씬 보정 | 위험 구역 트리거 검증을 위한 Rigidbody 복구 | `RatHostPrototype.unity` 보정 |
| QA/검증 에이전트 | 검증 | 테스트 실행 결과, 미검증 항목 정리 | verification.md |
| 프로젝트 총괄 관리자 에이전트 | 내부 승인 검토 | 범위/승인 게이트/검증 기록 확인 | completion-report.md 내 판정 |

## 구현 담당 확인

- 코드/테스트 변경 담당: 게임플레이 구현 에이전트
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: Unity 씬/통합 구현 에이전트
- 메인 에이전트 직접 구현 여부: 예
- 메인 에이전트 직접 구현 예외 사유: 현재 도구 실행 주체가 메인 Codex이므로 코드 편집 도구는 메인이 대행한다. 단, 담당 역할과 산출물은 게임플레이 구현 에이전트 기준으로 기록하고 QA/총괄 게이트를 별도로 남긴다.

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: Unity C# 코드, EditMode 테스트, 씬 변경
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예

## 목적

쥐 숙주가 소음/오염 위험을 피하려는 본능을 갖는다는 설계를 작은 코드 스파이크로 검증한다. 일반 이동에는 면역 경계도 상승을 주지 않고, 위험 방향으로 반복 입력할 때만 `강제 조종` 원인의 면역 경계도 상승을 발생시킨다.

## 입력 자료

- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `docs/prototype/plans/rat-host-immune-alert-work-direction.md`
- `docs/design/hosts/host-instinct-control.md`
- `docs/design/ui-feedback/immune-alert-feedback.md`
- `docs/agents/loop-engineering-gates.md`
- `.agents/gameplay-implementation-agent.md`
- `.agents/qa-verification-agent.md`

## 해야 할 일

1. 강제 조종 스파이크 수용 기준을 EditMode 테스트로 먼저 추가하고 RED를 확인한다.
2. 위험 방향 반복 입력을 감지하는 최소 상태 모델을 구현한다.
3. 기존 오염 위험 구역 흐름에 조작 방해 없이 `강제 조종` 피드백만 연결한다.
4. 기존 씬 검증을 깨는 `ToxicWaterRiskZone` Rigidbody 누락을 최소 보정한다.
5. EditMode 테스트를 실행하고 검증 기록을 남긴다.
6. 승인 범위와 미검증 항목을 총괄 관리자 기준으로 판정한다.

## 산출물

- `UnityProject/Assets/_Project/Scripts/**` 코드 변경
- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity` 씬 보정
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs` 테스트 변경
- `_workspace/completed/2026-07-07-2026-07-07-host-instinct-control-spike/work-log.md`
- `_workspace/completed/2026-07-07-2026-07-07-host-instinct-control-spike/agent-activity.md`
- `_workspace/completed/2026-07-07-2026-07-07-host-instinct-control-spike/verification.md`
- `_workspace/completed/2026-07-07-2026-07-07-host-instinct-control-spike/completion-report.md`

## 에이전트 수행 이력 기록

- `agent-activity.md` 생성 여부: 예
- 담당 에이전트별 수행 내용 기록 여부: 예
- 위임/검토/승인 판정 기록 여부: 예

## 금지 범위

- 실제 입력 지연, 반동, 조작 방해 추가
- 조종 부하 별도 게이지 추가
- 새 숙주, 새 스테이지, 내부 미니게임 유형 추가
- 씬의 신규 콘텐츠 추가 또는 배치 변경
- ProjectSettings 변경. 단, 작업 전부터 존재한 `APP_UI_EDITOR_ONLY` define 변경은 별도 변경으로 분리한다.
- 시간 경과 기본 면역 경계도 상승 재활성화

## 승인 필요 항목

- 이번 작업 안에서는 추가 승인 필요 항목 없음
- 금지 범위 항목을 구현하려면 별도 사용자 승인 필요

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 예
- 담당 에이전트 산출물 확인: 예
- 에이전트 수행 이력 확인: 예
- 구현 담당 에이전트 확인: 예
- 메인 에이전트 직접 구현 예외 사유 확인: 예
- QA/검증 에이전트 기록 확인: 예
- 총괄 관리자 판정 확인: 예
- 승인 게이트 확인: 예
- 완료 판단에 영향을 주는 미검증 항목: PC 빌드와 사용자 최종 수동 플레이는 남음

## 완료 기준

- 평범한 이동 또는 위험에서 멀어지는 입력은 면역 경계도를 올리지 않는다.
- 위험 중심을 향한 입력이 설정 시간 이상 반복될 때만 `강제 조종 +8` 피드백이 기록된다.
- 강제 조종은 이동 벡터를 바꾸거나 입력을 막지 않는다.
- 기존 면역 경계도 원인 피드백, 오염 구역, 내부 바이러스 전환 테스트가 통과한다.
- QA/검증 기록과 프로젝트 총괄 관리자 판정이 작업 폴더에 남는다.
- 내부 미니게임 실패 시 실패 UI를 보여준 뒤 보상 없이 쥐 숙주 모드로 복귀한다.
- 사용자 최종 확인 전 Unity MCP Play 체크를 수행하고 결과를 기록한다.
