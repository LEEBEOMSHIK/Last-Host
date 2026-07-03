# 작업 패킷: 면역 경계도 원인 피드백 구현

## 작업 ID

`2026-07-02-immune-alert-cause-feedback`

## 목적

소음 배관 위험 상호작용 후 플레이어가 면역 경계도가 왜 얼마나 올랐는지 알 수 있도록 HUD에 짧은 원인 피드백을 표시한다.

## 입력 자료

- `AGENTS.md`
- `docs/agents/loop-engineering-gates.md`
- `docs/prototype/plans/rat-host-immune-alert-work-direction.md`
- `docs/design/systems/immune-alert.md`
- `docs/design/interactions/noisy-pipe-risk-interaction.md`
- `docs/design/ui-feedback/immune-alert-feedback.md`
- `.agents/gameplay-implementation-agent.md`
- `.agents/unity-scene-integration-agent.md`
- `.agents/qa-verification-agent.md`

## 담당

- 게임플레이 구현 에이전트: 면역 경계도 원인 피드백 상태/이벤트 구조와 EditMode 테스트
- Unity 씬/통합 구현 에이전트: HUD 표시 연결, 씬 빌더/현재 씬 연결
- QA/검증 에이전트: 테스트/Unity 검증과 남은 위험 기록
- 프로젝트 총괄 관리자 에이전트: 범위, 승인 게이트, 검증 기록 최종 판정

## 구현 범위

- 소음 배관 상호작용에서 `소음/조직 자극 +15` 피드백을 남긴다.
- HUD에 마지막 면역 경계도 상승 원인과 변화량을 짧게 표시한다.
- 같은 구조를 이후 오염 구역, 강제 조종, 바이러스 흔적 피드백에도 재사용할 수 있게 한다.
- EditMode 테스트로 상태 모델과 HUD 표시를 고정한다.

## 금지 범위

- 새 미니게임 유형 구현
- 강제 조종 입력 지연/방해 구현
- 시간 경과 면역 경계도 상승 재활성화
- 조종 부하 별도 게이지 추가
- 새 패키지 설치

## 완료 기준

- 소음 배관 위험 상호작용 후 상태에 `소음/조직 자극 +15` 피드백이 남는다.
- HUD가 면역 경계도 원인 피드백을 표시한다.
- 면역 경계도 100%로 내부 바이러스 모드 전환 시 상호작용 프롬프트와 원인 피드백이 충돌하지 않는다.
- 관련 EditMode 테스트가 실패 후 구현으로 통과한다.
- 가능한 Unity 검증을 실행하고 `_workspace`에 기록한다.
