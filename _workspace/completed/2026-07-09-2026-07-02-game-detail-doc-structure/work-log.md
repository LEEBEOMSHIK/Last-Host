# 작업 로그

## 작업 ID

2026-07-02-game-detail-doc-structure

## 로그

### 2026-07-02

- 수행 내용: 기존 `docs/`와 `docs/prototype/` 구조를 확인하고, 상세 기획은 `docs/design/` 아래에 두는 방향으로 판단했다.
- 확인한 자료: `docs/README.md`, `docs/prototype/README.md`, `docs/agents/agent-reference-map.md`, `AGENTS.md`
- 판단: 배관 상호작용처럼 게임 규칙의 이유와 플레이어 의미를 설명하는 문서는 구현 계획이 아니라 상세 기획 문서에 속한다.
- 다음 작업: 첫 상세 기획 문서로 긴 소음 배관 상호작용과 면역 경계도 상승 의도를 작성할 수 있다.

## 결정 기록

- `docs/design/interactions/`는 오브젝트와 행동 결과를 다룬다.
- `docs/design/systems/`는 면역 경계도 같은 수치/상태 시스템을 다룬다.
- `docs/design/ui-feedback/`는 플레이어가 원인을 이해하는 HUD/프롬프트/경고를 다룬다.
- 구현 계획, 테스트 로그, 에이전트 이력은 `docs/design/`에 두지 않는다.

## 위험과 주의점

- 이번 작업은 폴더 구조와 색인만 정리했다. 실제 상세 기획 본문은 아직 작성하지 않았다.
