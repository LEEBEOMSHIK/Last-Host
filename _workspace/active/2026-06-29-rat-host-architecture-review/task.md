# 작업 배정서

## 기본 정보

- 작업 ID: `2026-06-29-rat-host-architecture-review`
- 작업명: 쥐 숙주 구현 계획 Unity 아키텍처 검토
- 상태: 진행 중
- 생성일: 2026-06-29
- 담당 에이전트: Unity 아키텍처 에이전트
- 보조 에이전트: 게임플레이 루프 에이전트
- 사용 스킬: `$unity-prototype-planner`

## 목적

게임플레이 루프 에이전트가 작성한 쥐 숙주 구현 계획 초안을 Unity 프로젝트 구조, 씬 구성, 폴더 구조, 입력 방식, 빌드 설정 관점에서 검토한다.

## 입력 자료

- `AGENTS.md`
- `docs/project-prep.md`
- `docs/unity-mcp-setup.md`
- `docs/rat-host-prototype.md`
- `docs/rat-host-approval-packet.md`
- `docs/unity-baseline-report.md`
- `.agents/unity-architecture-agent.md`
- `.codex/skills/unity-prototype-planner/SKILL.md`
- `.codex/skills/unity-prototype-planner/references/unity-architecture.md`
- `_workspace/active/2026-06-29-rat-host-plan-agent/artifacts/rat-host-implementation-plan-draft.md`

## 해야 할 일

1. 구현 계획 초안의 Unity 폴더 구조가 프로젝트 기준과 맞는지 검토한다.
2. `RatHostPrototype` 단일 씬 제안이 1차 프로토타입에 적절한지 판단한다.
3. `Assets/_Project/` 구조가 현재 Unity 템플릿 자산과 충돌하지 않는지 확인한다.
4. 초기 입력 구현에서 `Input System` 직접 사용 여부를 제안한다.
5. `RatHostPrototype.unity`를 Build Settings에 바로 추가할지 판단한다.
6. `SampleScene.unity` 처리 방안을 제안한다.
7. 구현 전 승인 필요 항목과 위험을 정리한다.

## 산출물

- `_workspace/active/2026-06-29-rat-host-architecture-review/artifacts/architecture-review.md`
- 필요 시 `_workspace/active/2026-06-29-rat-host-architecture-review/handoff.md` 갱신

## 금지 범위

- Unity 씬 생성 또는 수정
- C# 코드 작성
- ProjectSettings 변경
- 패키지 추가 또는 제거
- 게임플레이 범위 변경

## 승인 필요 항목

- `docs/rat-host-approval-packet.md` 승인
- Unity MCP를 통한 프로젝트 변경 승인
- `RatHostPrototype.unity` 생성과 Build Settings 반영 승인

## 완료 기준

- `architecture-review.md`에 추천 구조, 승인 필요 항목, 구현 전 위험, 다음 담당이 정리되어 있다.
- 검토 결과가 구현 계획 초안의 질문에 직접 답한다.
- Unity 프로젝트 파일은 변경되지 않았다.
