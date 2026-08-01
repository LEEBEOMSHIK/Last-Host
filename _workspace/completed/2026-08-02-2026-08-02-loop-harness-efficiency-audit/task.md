# 작업 배정서

## 기본 정보

- 작업 ID: `2026-08-02-loop-harness-efficiency-audit`
- 작업명: 루프 엔지니어링·검증 하네스 비용 효율 감사와 보완
- 상태: 완료 보관·운영 커밋 대기 — QA r6 PASS, 총괄 내부 승인 가능, 비용 `과다 — 부분 회피 가능`
- 생성일: 2026-08-02 KST
- 담당 에이전트: 프로젝트 조정 에이전트
- 보조 에이전트: QA/검증 에이전트, Unity 하네스 감사 에이전트, 문서/릴리즈 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `last-host-design-keeper`, `unity-verification-runner`

## 목적

Production2D 쥐·오브젝트 가림 교정에서 실제 변경량보다 토큰·검증 반복 비용이 커진 원인을 사실 기반으로 분석하고, 동일 유형 결함을 더 이른 단계에서 한 번에 찾도록 루프 순서·검증 계약·하네스·증거 생성 규칙을 보완한다.

## 입력 자료

- `AGENTS.md`
- `docs/agents/agent-skill-plan.md`
- `docs/agents/loop-engineering-gates.md`
- `.agents/qa-verification-agent.md`
- `.agents/project-coordinator-agent.md`
- `.agents/project-director-agent.md`
- `.codex/skills/unity-verification-runner/`
- `_workspace/templates/`
- `_workspace/active/2026-08-02-production2d-visual-overlap-correction/`

## 해야 할 일

1. 가림 교정 작업의 에이전트·테스트·실패·재실행 타임라인을 재구성하고 비용 원인을 분류한다.
2. 현행 루프 문서와 실제 실행의 불일치, 중복 게이트, 늦은 결함 발견 원인을 감사한다.
3. 결함 수정용 위험 기반 루프, fail-fast 검증 순서, 변경 후 검증 무효화 규칙, Unity 세션 단독 소유, 원자적 증거 캡처 규칙을 문서·템플릿에 반영한다.
4. 새 규칙이 기존 승인 게이트와 독립 QA를 약화하지 않으면서 불필요한 반복을 줄이는지 독립 QA와 총괄 검토를 받는다.
5. 사용자가 에이전트 배정·검증 책임·비용·중복 실행·Unity 한계를 한 파일에서 지속 확인할 수 있는 비실행 요약 가이드를 제공한다.
6. 작업별 계획·실제 비용 proxy, 중복·폐기, 필요한 비용·회피 가능 비용을 사용자가 직접 확인할 중앙 현황판과 최소 기록 필드를 제공한다.

## 산출물

- `artifacts/overlap-incident-cost-analysis.md`
- `artifacts/current-loop-gap-audit.md`
- `artifacts/harness-improvement-design.md`
- `artifacts/operations-doc-change-report.md`
- `artifacts/verification-tools-report.md`
- `artifacts/loop-harness-audit-summary.md`
- `tools/verification/` 범용 lease·EditMode runner·fingerprint 도구
- 보완된 운영 문서·QA 역할·검증 스킬 참조·작업 템플릿
- `verification.md`, `director-review.md`
- `docs/agents/loop-engineering-user-guide.md`
- `docs/project-handoff/task-cost-dashboard.md`
- `artifacts/task-cost-dashboard-change-report.md`

## 금지 범위

- Unity 게임플레이 코드·씬·ProjectSettings 수정
- 새 에이전트 역할 또는 새 Codex 스킬 생성
- 독립 QA·총괄 게이트 삭제
- 기존 사용자 변경, Stage2/Stage3, 가림 교정 구현 변경
- 토큰 절감만을 이유로 실패·미검증을 통과 처리

## 승인 필요 항목

- 사용자의 이번 요청을 기존 운영 문서·역할 책임·템플릿 보완 승인으로 본다.
- 새 에이전트/스킬 생성, 승인 게이트 삭제, 프로젝트 범위 변경은 별도 승인 없이는 하지 않는다.

## 완료 기준

- 반복 비용의 직접 원인과 구조 원인을 구분한 타임라인이 있다.
- “왜 QA가 한 번에 못 잡았는가”에 에이전트 수가 아닌 실행 순서·계약·하네스 근거로 답한다.
- 위험 등급별 최소 에이전트 구성과 fail-fast 검증 단계가 명문화된다.
- 코드 변경 뒤 이전 검증을 무효화하고 변경 주체가 최소 회귀를 실행하는 규칙이 생긴다.
- Unity MCP 동시 조작과 비동기 캡처 불일치를 막는 규칙이 생긴다.
- QA가 문서 정합과 예시 시뮬레이션을 검증하고 총괄 `내부 승인 가능`을 판정한다.
