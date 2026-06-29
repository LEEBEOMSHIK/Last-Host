# 핸드오프 기록

## 작업 ID

`2026-06-29-rat-host-plan-agent`

## 넘기는 에이전트

프로젝트 조정 에이전트

## 받는 에이전트

게임플레이 루프 에이전트

## 현재 상태

승인 대기. 구현 계획 초안은 폐기되었고, 담당 에이전트가 작업할 배정 패킷만 준비된 상태다.

## 넘기는 이유

쥐 숙주 핵심 루프 구현 계획은 게임플레이 루프 에이전트의 책임 영역이다. 조정자가 직접 최종 계획서를 작성하지 않고, 담당 에이전트 산출물로 받은 뒤 검토해야 한다.

## 이어서 해야 할 일

1. 입력 자료를 읽고 승인되지 않은 결정과 확정된 범위를 분리한다.
2. 핵심 루프 구현 계획 초안을 `artifacts/rat-host-implementation-plan-draft.md`에 작성한다.
3. Unity 구조 검토가 필요한 항목은 Unity 아키텍처 에이전트에게 넘길 질문으로 분리한다.
4. 검증 기준은 QA/검증 에이전트에게 넘길 체크리스트로 분리한다.

## 참고 자료

- `docs/rat-host-prototype.md`
- `docs/rat-host-approval-packet.md`
- `docs/unity-baseline-report.md`
- `.agents/gameplay-loop-agent.md`
- `.codex/skills/rat-host-loop-builder/references/rat-loop-rules.md`

## 주의할 점

- 구현 계획 산출물은 아직 `docs/`에 직접 반영하지 않는다.
- 벌레 튜토리얼, 다중 숙주, 인간 단계, 백신, 엔딩은 범위 밖이다.
- Unity MCP는 읽기 호출만 허용된 상태로 보고, 변경 작업은 별도 승인 후 진행한다.

## 사용자 승인 필요

- `docs/rat-host-approval-packet.md` 승인
- 구현 계획 산출물을 공식 문서로 승격할지 여부
