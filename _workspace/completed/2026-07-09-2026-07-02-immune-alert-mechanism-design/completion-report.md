# 완료 보고

## 작업 요약

면역 경계도 상승 기작을 실제 바이러스/면역 반응에서 빌린 설계 축과 현재 쥐 숙주 프로토타입 규칙으로 나눠 문서화했다.

사용자 피드백을 반영해 다음 내용을 보강했다.

- 바이러스 패턴 노출을 실제 게임 트리거 후보로 분해했다.
- 오염/더러운 구역은 접촉, 체류, 상처 상태 같은 명확한 조건에서만 경계도를 올리도록 정리했다.
- 숙주 본능과 플레이어 강제 조작 충돌을 별도 문서로 분리했다.
- 내부 미니게임을 백혈구 전투 하나로 고정하지 않고 여러 대응 유형 후보로 정리했다.
- 미니게임 난이도 강화는 경계도 상승 중 즉시 적용이 아니라 내부 대응 반복 이후 다음 대응 강화로 정리했다.

## 변경 문서

- `docs/design/systems/immune-alert.md`
- `docs/design/hosts/host-instinct-control.md`
- `docs/design/encounters/internal-immune-response-minigame-types.md`
- `docs/design/interactions/noisy-pipe-risk-interaction.md`
- `docs/design/ui-feedback/immune-alert-feedback.md`
- `docs/design/README.md`
- `docs/design/systems/README.md`
- `docs/design/hosts/README.md`
- `docs/design/encounters/README.md`
- `docs/design/interactions/README.md`
- `docs/design/ui-feedback/README.md`
- `docs/agents/agent-reference-map.md`

## 프로젝트 총괄 관리자 판정

내부 승인 가능.

근거:

- 1차 쥐 숙주 프로토타입 범위 안의 문서화 작업이다.
- 새 숙주, 백신 시스템, 인간 단계, 캠페인, 엔딩을 구현 요구로 추가하지 않았다.
- 실제 면역학 설명은 게임 추상화로만 사용했고, 의학 조언이나 실험 절차로 보이는 서술은 없다.
- 현재 구현 수치인 `BaseAlertPerSecond = 0`, `RiskAlertBonus = 20`, 소음 배관 위험도 `0.75`, 결과 `+15`와 문서 설명이 충돌하지 않는다.
- 사용자 피드백 반영 후 재검토에서도 `내부 승인 가능` 판정을 받았다.

## 사용자 확인 필요

- 면역 경계도를 올리는 원인 범주가 게임 의도와 맞는지
- 소음 배관을 `위험한 숙주 행동`으로 해석하는 방식이 적절한지
- HUD에 상승 원인과 변화량을 보여주는 방향이 맞는지
