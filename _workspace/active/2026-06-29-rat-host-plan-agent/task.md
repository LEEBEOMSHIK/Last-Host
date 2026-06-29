# 작업 배정서

## 기본 정보

- 작업 ID: `2026-06-29-rat-host-plan-agent`
- 작업명: 쥐 숙주 핵심 루프 구현 계획 에이전트 산출
- 상태: 검토 중
- 생성일: 2026-06-29
- 담당 에이전트: 게임플레이 루프 에이전트
- 보조 에이전트: Unity 아키텍처 에이전트, QA/검증 에이전트
- 사용 스킬: `$rat-host-loop-builder`, 필요 시 `$unity-prototype-planner`, `$unity-verification-runner`

## 목적

쥐 숙주 프로토타입의 실제 Unity 구현 전에, 담당 에이전트가 핵심 루프 구현 계획과 수용 기준, 테스트 시나리오를 작성할 수 있도록 작업 입력과 금지 범위를 명확히 제공한다.

## 입력 자료

- `AGENTS.md`
- `docs/agent-reference-map.md`
- `docs/game-design-summary.md`
- `docs/rat-host-prototype.md`
- `docs/rat-host-approval-packet.md`
- `docs/project-prep.md`
- `docs/unity-baseline-report.md`
- `.agents/gameplay-loop-agent.md`
- `.agents/unity-architecture-agent.md`
- `.agents/qa-verification-agent.md`
- `.codex/skills/rat-host-loop-builder/SKILL.md`
- `.codex/skills/rat-host-loop-builder/references/rat-loop-rules.md`

## 해야 할 일

1. `docs/rat-host-approval-packet.md`의 승인 항목을 확인하고, 미승인 항목을 구현 전 차단 조건으로 분리한다.
2. 쥐 숙주 핵심 루프를 기능 단위로 나눈다: 쥐 조종, 면역 경계도, 모드 전환, 내부 바이러스 미니게임, 변이 선택, 쥐 모드 복귀.
3. 각 기능 단위별 입력, 처리, 출력, UI 피드백, 수용 기준을 작성한다.
4. Unity 아키텍처 에이전트가 검토할 폴더/씬/스크립트 책임 경계를 제안한다.
5. QA/검증 에이전트가 검토할 테스트 시나리오와 수동 플레이 체크리스트를 제안한다.
6. 범위 초과 항목과 사용자 승인 필요 항목을 별도로 정리한다.

## 산출물

- `_workspace/active/2026-06-29-rat-host-plan-agent/artifacts/rat-host-implementation-plan-draft.md`
- `_workspace/active/2026-06-29-rat-host-plan-agent/handoff.md` 갱신
- 필요 시 `_workspace/active/2026-06-29-rat-host-plan-agent/artifacts/open-questions.md`

## 금지 범위

- Unity 씬 생성 또는 수정
- C# 코드 작성
- 패키지 추가 또는 ProjectSettings 변경
- `docs/rat-host-implementation-plan.md` 직접 생성
- 승인 패킷의 범위를 넘어서는 벌레 튜토리얼, 다중 숙주, 인간 단계, 백신, 엔딩, 영구 성장 설계

## 승인 필요 항목

- `docs/rat-host-approval-packet.md` 전체 승인 또는 수정 승인
- Unity MCP를 통한 프로젝트 변경 승인
- 구현 계획 산출물을 `docs/`에 반영할지 여부

## 완료 기준

- 담당 에이전트 산출물이 `artifacts/rat-host-implementation-plan-draft.md`에 있다.
- 기능 단위, 수용 기준, 테스트 시나리오, 범위 초과 항목이 분리되어 있다.
- 구현 전 사용자 승인 필요 항목이 누락되지 않았다.
- 조정자가 산출물을 검토한 뒤에만 `docs/` 반영 여부를 결정할 수 있다.
