# 작업 패킷: 면역 경계도 기작 상세 기획

## 작업 ID

`2026-07-02-immune-alert-mechanism-design`

## 목적

바이러스가 숙주의 면역 경계도를 올리는 기작을 현재 게임 프로젝트 특성과 실제 바이러스/면역 반응의 특징을 접목해 상세 기획 문서로 정리한다.

## 입력 자료

- `AGENTS.md`
- `docs/design/game-design-summary.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/design/README.md`
- `docs/design/systems/README.md`
- `docs/design/interactions/README.md`
- `docs/design/ui-feedback/README.md`
- 현재 프로토타입 수치: `MaxImmuneAlert`, `BaseAlertPerSecond`, `RiskAlertBonus`, `RatRiskInteractable.riskSeverity`

## 산출물

- `docs/design/systems/immune-alert.md`
- `docs/design/interactions/noisy-pipe-risk-interaction.md`
- `docs/design/ui-feedback/immune-alert-feedback.md`
- 관련 README와 `docs/agents/agent-reference-map.md` 색인 갱신

## 금지 범위

- Unity 코드, 씬, ProjectSettings 변경
- 새 숙주, 백신 시스템, 인간 단계 구현 범위 추가
- 실제 면역학을 의학 조언이나 실험 절차처럼 서술

## 승인 필요 항목

이 작업은 승인된 쥐 숙주 프로토타입의 설명 문서화로 처리한다. 새 게임 시스템 구현, 수치 적용, 코드 변경은 별도 승인과 작업 패킷이 필요하다.
