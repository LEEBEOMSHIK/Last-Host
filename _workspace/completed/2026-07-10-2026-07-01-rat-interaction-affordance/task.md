# 작업 배정서

## 기본 정보

- 작업 ID: 2026-07-01-rat-interaction-affordance
- 작업명: 쥐 숙주 상호작용 대상 식별성 보정
- 상태: 완료 처리 / 사용자 수동 체감 확인은 별도 체크리스트로 보류
- 생성일: 2026-07-01
- 담당 에이전트: Unity 씬/통합 구현 에이전트
- 보조 에이전트: 게임플레이 구현 에이전트, QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: rat-host-loop-builder, pixel-lowpoly-style-keeper, unity-verification-runner, systematic-debugging, test-driven-development

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| Unity 씬/통합 구현 에이전트 | 구현 담당 | 씬 빌더, 플레이스홀더 오브젝트, 머티리얼, UI 연결 | 상호작용 대상 표시와 씬 연결 변경 |
| 게임플레이 구현 에이전트 | 코드 보조 | 근접 상태, HUD 문구, 회귀 테스트 | C# 코드와 EditMode 테스트 |
| QA/검증 에이전트 | 검증 담당 | 원래 증상 재현, 테스트, Play 검증, 콘솔 확인 | `verification.md` 판정 |
| 프로젝트 총괄 관리자 에이전트 | 내부 승인자 | 범위, 게이트, QA 기록 확인 | 완료 가능 여부 판정 |

## 구현 담당 확인

- 코드/테스트 변경 담당: 게임플레이 구현 에이전트
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: Unity 씬/통합 구현 에이전트
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 없음

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: Unity 코드, 테스트, 씬 빌더, 씬 직렬화 변경 가능성이 있음
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예

## 목적

현재 씬에서 `Space` 상호작용 로직은 존재하지만 플레이어가 상호작용 대상을 알아보고 접근하기 어렵다. 쥐 숙주 프로토타입 범위 안에서 상호작용 대상을 시각적으로 식별 가능하게 만들고, 근접 시 HUD가 상호작용 가능 상태를 알려주도록 보정한다.

## 입력 자료

- 사용자 피드백: `WASD` 이동은 확인했지만 상호작용 대상이 없어 이동 외 키를 확인하지 못함
- `_workspace/active/2026-07-01-rat-host-full-play-verification/verification.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/design/game-design-summary.md`
- `docs/agents/loop-engineering-gates.md`
- `.agents/unity-scene-integration-agent.md`
- `.agents/gameplay-implementation-agent.md`
- `UnityProject/Assets/_Project/Editor/RatHostPrototypeSceneBuilder.cs`
- `UnityProject/Assets/_Project/Scripts/Host/RatRiskInteractable.cs`
- `UnityProject/Assets/_Project/Scripts/UI/PrototypeHud.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`

## 해야 할 일

1. 상호작용 대상 식별 실패를 회귀 테스트로 고정한다.
2. `RatRiskInteractable`이 쥐 근접 상태를 세션에 전달하도록 최소한의 런타임 상태를 추가한다.
3. HUD 목표 문구가 쥐 근접 시 상호작용 가능 상태를 표시하도록 연결한다.
4. `NoisyPipeRiskInteractable`을 기존 갈색 큐브보다 식별 가능한 배관/밸브형 플레이스홀더로 보정한다.
5. 씬 빌더를 실행해 `RatHostPrototype.unity`에 변경을 반영한다.
6. 테스트, Play 검증, 콘솔 오류 확인을 실행한다.
7. QA/검증 에이전트와 프로젝트 총괄 관리자 에이전트 판정을 기록한다.

## 산출물

- 변경된 C# 코드와 EditMode 테스트
- 변경된 씬 빌더와 `RatHostPrototype.unity`
- `work-log.md`
- `agent-activity.md`
- `handoff.md`
- `verification.md`
- 필요 시 `completion-report.md`

## 에이전트 수행 이력 기록

- `agent-activity.md` 생성 여부: 예
- 담당 에이전트별 수행 내용 기록 여부: 진행 중
- 위임/검토/승인 판정 기록 여부: 진행 중

## 금지 범위

- 새 숙주, 새 스테이지, 새 시스템 추가
- 최종 아트 에셋 생성 또는 외부 에셋 다운로드
- 패키지 추가
- 입력 키 변경
- 상호작용을 전체 튜토리얼 시스템으로 확장
- `.codex/config.toml` 변경 반영

## 승인 필요 항목

- 보정 방향은 사용자 승인됨: 기존 `NoisyPipeRiskInteractable` 유지, 식별 가능한 저폴리 배관/밸브 표시, 근접 시 HUD 피드백 추가
- 추가 승인 필요 항목: 현재 없음. 범위가 커지면 사용자에게 보고

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 진행 중
- 담당 에이전트 산출물 확인: 진행 중
- 에이전트 수행 이력 확인: 진행 중
- 구현 담당 에이전트 확인: 완료
- 메인 에이전트 직접 구현 예외 사유 확인: 해당 없음
- QA/검증 에이전트 기록 확인: 대기
- 총괄 관리자 판정 확인: 대기
- 승인 게이트 확인: 사용자 승인 범위 안
- 완료 판단에 영향을 주는 미검증 항목: 대기

## 완료 기준

- 현재 씬에서 상호작용 대상이 초기 플레이 동선에서 시각적으로 구분된다.
- 쥐가 상호작용 대상에 근접하면 HUD 목표 문구가 상호작용 가능 상태를 표시한다.
- `Space` 입력 시 면역 경계도가 상승하는 기존 동작이 유지된다.
- 회귀 테스트가 추가되고 통과한다.
- Play 검증에서 상호작용 대상 발견성, 근접 HUD, 상호작용 입력 효과가 확인된다.
- Unity 콘솔 Error 0건 또는 남은 Error의 비관련성이 기록된다.
- QA/검증 에이전트와 프로젝트 총괄 관리자 판정이 기록된다.
