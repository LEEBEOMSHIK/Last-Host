# 작업 배정

## 작업 ID

`2026-07-27-rat-core-loop-2d-migration-brief`

## 작업명

쥐 숙주 핵심 루프의 단계적 2D 이관 범위·승인 브리프

## 작업영역

기획 정리, Unity 구조 계획, 핵심 루프 작업 분해, 승인 질문 작성

## 담당 에이전트

- 주 담당: 게임플레이 루프 에이전트
- 보조: 기획 정리 에이전트
- 보조: Unity 아키텍처 에이전트
- 검증: QA/검증 에이전트
- 내부 승인: 프로젝트 총괄 관리자 에이전트
- 조정·통합: Codex 메인 에이전트

## 목적

사용자가 수용한 별도 2D 플레이어블 기술 샘플을 기반으로, 기존 쥐 숙주 핵심 루프를 어떤 순서와 경계로 2D 구조에 이관할지 결정할 수 있는 승인 브리프를 만든다.

## 입력 자료

- `AGENTS.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `docs/prototype/approvals/rat-host-approval-packet.md`
- `docs/design/game-design-summary.md`
- `docs/project/project-prep.md`
- `docs/agents/loop-engineering-gates.md`
- `_workspace/completed/2026-07-27-2026-07-27-2d-playable-technical-sample/`

## 해야 할 일

1. 기존 3D 핵심 상태 로직과 신규 2D 표현·물리 계층의 재사용/교체 경계를 정리한다.
2. 숙주·면역·모드 전환, 내부 바이러스 미니게임, 변이 선택·복귀를 독립 검증 가능한 단계로 나눈다.
3. 각 단계의 포함·제외 범위, 수용 기준, 테스트 시나리오와 회귀 보호 대상을 정의한다.
4. 씬 전략, 기술 샘플 처리, 기존 3D 보존 시점과 사용자 승인 질문을 제시한다.
5. QA 독립 대조와 프로젝트 총괄 내부 판정을 거친다.

## 산출물

- `docs/prototype/approvals/rat-host-2d-core-loop-migration-brief.md`
- `artifacts/` 아래 역할별 검토 기록
- `verification.md`
- `director-review.md`

## 금지 범위

- Unity 코드·씬·테스트·ProjectSettings·패키지 변경
- 새 아트 에셋 생성
- 기존 3D 씬·검증 자료 삭제
- 벌레 튜토리얼, 다중 숙주, 인간 단계, 백신, 엔딩, 영구 성장 추가
- 기술 샘플의 시험 PPU·해상도·플레이스홀더를 최종 규격이나 최종 아트로 확정

## 승인 필요 항목

- 실제 2D 이관의 1차 구현 단계
- 기술 샘플을 확장할지, 별도 2D 프로토타입 씬으로 승격할지
- 기존 상태 로직 재사용 원칙과 기존 3D 씬 보존 시점
- 단계별 구현을 시작할지 여부

## 완료 기준

- 세 단계 이관안과 추천안이 비교 가능하게 정리되어 있다.
- 각 단계에 기능 경계, 수용 기준, 테스트 시나리오, 제외 범위가 있다.
- 사용자 결정이 필요한 항목이 짧고 명확하다.
- QA/검증 에이전트가 문서 정합과 상태판을 독립 대조한다.
- 프로젝트 총괄 관리자 에이전트가 `내부 승인 가능` 또는 `사용자 결정 필요` 판정을 남긴다.
