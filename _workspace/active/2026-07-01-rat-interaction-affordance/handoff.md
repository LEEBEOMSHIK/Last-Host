# 핸드오프 기록

## 작업 ID

2026-07-01-rat-interaction-affordance

## 넘기는 에이전트

Codex 메인 에이전트

## 받는 에이전트

Unity 씬/통합 구현 에이전트, 게임플레이 구현 에이전트

## 현재 상태

사용자가 상호작용 대상 식별성 보정을 승인했다. 이전 검증에서 `WASD` 이동은 수동 확인됐지만, `Space` 상호작용은 현재 씬에서 대상이 드러나지 않아 수동 검증이 차단됐다.

## 루프 게이트 상태

- 작업 배정 게이트: 생성됨
- 담당 산출물 게이트: 진행 중
- QA/검증 게이트: 대기
- 총괄 관리자 게이트: 대기
- 커밋 전 차단 조건: 대기

## 넘기는 이유

코드, 테스트, 씬 빌더, 씬 직렬화 변경이 필요할 수 있으므로 프로젝트 규칙상 구현 전담 에이전트가 담당해야 한다.

## 넘기는 에이전트가 완료한 일

- 원인 조사: 상호작용 로직과 오브젝트는 존재하지만 초기 화면/동선/HUD에서 식별성이 부족함
- 사용자 승인 수신: 식별 가능한 저폴리 배관/밸브 표시와 근접 HUD 피드백 추가
- 작업 패킷 생성

## 받는 에이전트에게 기대하는 산출물

- 회귀 테스트
- 필요한 C# 런타임 상태 변경
- 씬 빌더와 플레이스홀더 오브젝트 변경
- 씬 재생성 결과
- 실행한 검증 요약

## 이어서 해야 할 일

1. 상호작용 대상 식별성 테스트를 먼저 추가한다.
2. 근접 상태와 HUD 문구를 최소 구현한다.
3. `NoisyPipeRiskInteractable`을 더 알아보기 쉬운 배관/밸브 플레이스홀더로 만든다.
4. 씬 빌더 실행 후 Unity 검증을 진행한다.

## 참고 자료

- `AGENTS.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/agents/loop-engineering-gates.md`
- `.agents/unity-scene-integration-agent.md`
- `.agents/gameplay-implementation-agent.md`
- `UnityProject/Assets/_Project/Editor/RatHostPrototypeSceneBuilder.cs`
- `UnityProject/Assets/_Project/Scripts/Host/RatRiskInteractable.cs`
- `UnityProject/Assets/_Project/Scripts/UI/PrototypeHud.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`

## 에이전트 수행 이력 갱신

- `agent-activity.md`에 인계 기록 추가 여부: 예
- 인계 결과 기록 책임자: Codex 메인 에이전트

## 주의할 점

- 입력 키는 바꾸지 않는다.
- 최종 아트나 외부 에셋을 만들지 않는다.
- 새 튜토리얼 시스템으로 확장하지 않는다.
- `.codex/config.toml`은 건드리지 않는다.

## 사용자 승인 필요

- 현재 없음. 범위가 커지면 사용자에게 보고한다.
