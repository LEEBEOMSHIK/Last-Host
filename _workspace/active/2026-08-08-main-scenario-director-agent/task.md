# 메인 시나리오 디렉터 에이전트 추가 계획

> **For agentic workers:** REQUIRED SUB-SKILL: Use `superpowers:subagent-driven-development` or `superpowers:executing-plans` only when this plan is handed off. This session executes the approved documentation change inline.

**Goal:** 게임 전체 서사의 연속성을 소유하는 메인 시나리오 디렉터 역할을 추가하고, 승인된 오프닝 구조 결정을 관련 문서에 반영한다.

**Architecture:** 새 역할은 이야기 구조·숙주/맵/게임플레이 연결·미스터리 공개 장부를 소유한다. 프로젝트 총괄의 승인 판정, 시네마틱 연출의 숏 설계, 구현·에셋·QA 책임은 침범하지 않는다.

**Tech Stack:** 한국어 Markdown 운영 문서. 새 Codex 스킬·스크립트·에셋·Unity 변경 없음.

## 기본 정보

- 작업 ID: `2026-08-08-main-scenario-director-agent`
- 작업명: 메인 시나리오 디렉터 에이전트 추가와 오프닝 구조 동기화
- 상태: 승인 대기
- 생성일: 2026-08-08
- 담당 에이전트: 프로젝트 조정 에이전트
- 보조 에이전트: QA/검증, 프로젝트 총괄 관리자
- 사용 스킬: `superpowers:brainstorming`, `superpowers:writing-plans`, `last-host-design-keeper`, `skill-creator`

## 루프 게이트

- 위험 등급: R2
- 근거: 새 프로젝트 역할과 운영 참조 계약을 추가하며 여러 문서의 책임 경계를 동기화한다.
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- correction cycle: 1/2 — 시퀀스 A~E 확장 뒤 제작 계획의 기존 `C·D` 참조를 `D·E`로 교정하고 기록을 동기화함
- Unity/MCP/build preflight: 해당 없음

## S0 사용자 원요청·검증 charter

- 사용자 원문: "게임의 전체적인 내용(시나리오)를 이끌어 갈 에이전트를 만드는 게 좋을거 같아." 이후 제안된 메인 시나리오 디렉터 구성을 승인함.
- 추가 확정: 회사원·학생·가족은 각각 독립된 3개 씬이며, 그 뒤 별도 감염 확산 구간에서 확산 경로와 피해 결과를 함께 보여주는 혼합형을 사용한다.
- 금지 결과: 새 역할이 사용자 최종 결정, 총괄 승인, 세부 숏 연출, Unity·에셋 제작 또는 QA를 대신함.
- 허용 결과: 새 역할이 전체 서사 구조와 연속성을 관리하고 기존 전문 역할로 명시적으로 인계함.
- 완료 주장: 역할 파일과 세 색인이 일관되며, 오프닝 문서가 독립 3씬·혼합형·내용 우선 러닝타임 산정을 명시한다.

| criterion ID | 유형 | 기대값 | 최소 검증 |
| --- | --- | --- | --- |
| C1 | 성공 | `.agents/main-scenario-director-agent.md`가 역할·입력·산출물·금지·협업·절차를 정의 | 정적 대조 |
| C2 | 일관성 | roster·skill plan·reference map에서 같은 이름·경계를 사용 | 경로·문구 대조 |
| C3 | 경계 | 총괄·기획 정리·시네마틱·구현·QA와 책임이 중복되지 않음 | 금지 범위 대조 |
| C4 | 비용 | 새 스킬·스크립트·에셋·Unity 변경이 없음 | 변경 파일 목록 확인 |
| C5 | 시나리오 | 평온한 독립 3씬 뒤 별도 혼합형 확산 구간 | 오프닝 문서 대조 |
| C6 | 순서 | 러닝타임은 사건·씬·숏·호흡 뒤 산정 | 계획 순서 대조 |

## 변경 파일과 소유권

| 파일 | 단일 소유자 | 책임 |
| --- | --- | --- |
| `.agents/main-scenario-director-agent.md` | 프로젝트 조정 | 새 역할 계약 |
| `.agents/agent-roster.md` | 프로젝트 조정 | 역할 요약 색인 |
| `docs/agents/agent-skill-plan.md` | 프로젝트 조정 | 상세 운영 역할·핸드오프 |
| `docs/agents/agent-reference-map.md` | 프로젝트 조정 | 시나리오 작업 참조 경로 |
| `docs/design/narrative/opening/opening-cinematic-origin.md` | 프로젝트 조정 | 독립 3씬·혼합형 확산·길이 산정 원칙 |
| `docs/design/narrative/opening/opening-cinematic-production-plan.md` | 프로젝트 조정 | 제작 결정 순서 |
| `docs/project-handoff/current-task-board.md` | 프로젝트 조정 | active 상태-only 동기화 |
| `docs/project-handoff/task-cost-dashboard.md` | 프로젝트 조정 | R2 비용 상태-only 동기화 |

## 실행 계획

- [x] **Task 1:** 메인 시나리오 디렉터 역할 파일을 작성한다.
- [x] **Task 2:** roster·agent skill plan·reference map에 역할과 핸드오프를 연결한다.
- [x] **Task 3:** 오프닝 문서에 독립 3씬·혼합형 확산·내용 우선 러닝타임 산정을 반영한다.
- [x] **Task 4:** 작성자 정적 검사와 `git diff --check`를 수행한다.
- [x] **Task 5:** 독립 QA와 프로젝트 총괄 판정을 받는다.

## 비용 계획

| 비용 항목 | 계획 |
| --- | --- |
| 역할·인계 | 조정1·QA1·총괄1 |
| 표적 검증 | 작성자 정적1·독립 QA1 |
| Unity/MCP/빌드·full suite | 0 |
| matrix/capture·artifact | 0 |

## 금지 범위

- 새 Codex 스킬·에이전트 UI 패키지·스크립트를 생성하지 않는다.
- 전체 캠페인 내용을 새로 확정하거나 승인된 쥐 숙주 구현 범위를 확대하지 않는다.
- 이미지·애니매틱·오디오·Unity 파일을 수정하지 않는다.
- 기존 미커밋 변경을 되돌리거나 이 작업의 커밋에 자동 포함하지 않는다.

## 승인 상태

- 새 에이전트 역할 추가: 2026-08-08 사용자 승인됨
- 관련 역할 목록·참조 문서 개정: 위 승인에 포함됨
- 새 스킬 생성: 승인되지 않았으며 생성하지 않음

## 완료 기준

- C1~C6가 최종 후보에서 PASS한다.
- 독립 QA와 프로젝트 총괄이 역할 충돌·승인 경계를 확인한다.
- 사용자가 다음 시나리오 논의를 이어갈 수 있는 참조 경로가 명확하다.
