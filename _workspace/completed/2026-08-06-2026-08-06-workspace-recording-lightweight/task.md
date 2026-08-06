# 작업 배정서

## 기본 정보

- 작업 ID: `2026-08-06-workspace-recording-lightweight`
- 작업명: 작업 기록·검증 운영 경량 구조 개편
- 상태: 재분류 구현 완료 — 사용자 승인 재개 후 독립 QA 대기
- 생성일: 2026-08-06
- 담당 에이전트: 문서/릴리즈 에이전트
- 보조 에이전트: 독립 QA/검증 에이전트, 프로젝트 총괄 관리자
- 사용 스킬: `last-host-design-keeper`

## 위험 등급과 역할

- 위험 등급: R3 — `AGENTS.md`, 유일 실행 기준, 역할·작업영역·템플릿 계약을 함께 변경
- 구현 소유자: 문서/릴리즈 에이전트 1명
- 독립 QA: 운영 계약 정합성·기존 안전 게이트 보존 검증
- 총괄: 사용자 승인 범위·QA 근거·완료 판정 감사
- Unity lease: 해당 없음

## S0 사용자 원증상·검증 charter

- 사용자 원증상: 모든 작업에서 다수 이력 파일과 `_workspace` 기록·현황판 동기화가 반복돼 실제 작업보다 컨텍스트·에이전트 호출·문서 관리 비용이 커진다.
- 원증상 근거: `_workspace` 1,538파일, Markdown 656개·56,149줄, active 11·completed 82, 반복 packet 파일 500개 이상.
- 금지 결과: QA·원증상 검증·승인 게이트를 비용 절감 명목으로 없애거나, 기존 이력을 대량 삭제하거나, R2/R3 고위험 변경을 무기록 처리한다.
- 허용 결과: 위험에 비례해 기록 표면만 줄이고, 필요한 독립 QA·총괄·canonical evidence는 유지한다.
- 완료 주장: 2026-08-06 이후 작업부터 위험 등급별 최소 기록 구조를 적용하고 기존 이력은 그대로 보존한다.

| criterion ID | 유형 | 입력·상태 | 기대값 | 최소 검증 |
| --- | --- | --- | --- | --- |
| C1 | 성공 | R0 조회·설명 | packet·QA·총괄·board 비용 0 | 규칙·색인·예시 대조 |
| C2 | 성공 | R1 국소 수정 | `record.md` 1개와 표적 검증 1회. 실행 코드·자동화 스크립트, 사용자 동작/데이터, 공개 계약, 검증 신뢰성, 파괴 가능 작업 영향은 독립 QA 필수. 실행 결과에 영향 없는 오탈자·링크·색인·표현 정리만 QA 생략. 불명확하면 R2 승급. 범위·승인 충돌은 총괄 감사와 사용자 승인 대기 | AGENTS·gate·template 대조 |
| C3 | 성공 | R2 일반 구현 | `task.md`+`verification.md`; 독립 QA·총괄 유지. `verification.md`가 실행 이력·QA 판정·총괄 판정·canonical evidence·최종 상태를 통합 소유 | 필수 파일/역할 대조 |
| C4 | 성공 | R3 구조·릴리즈 | 기본 canonical 파일은 `task.md`+`verification.md`. `work-log.md`, `agent-activity.md`, `completion-report.md`는 정보가 두 기본 파일에 안전하게 통합될 수 없을 때만 조건부 생성 | S0~S7·lease·preflight와 정보 소유권 대조 |
| C5 | 경계 | 세션 중단·외부 차단·실제 인계 | 이 조건 중 하나가 있을 때만 `handoff.md` 추가 | workspace 규칙 대조 |
| C6 | negative control | 기존 active/completed 이력 | 삭제·대량 재작성 없음 | Git diff 경로 대조 |
| C7 | 비용 | 2026-08-06 이후 신규 board/dashboard/artifacts | board는 active/next 중심, dashboard는 `R2/R3 또는 실제 고비용 실행`의 합집합만, canonical evidence만 보존 | 문서·템플릿 대조 |
| C8 | 수명주기 | S0 계약·candidate 불변이고 path/status/diff만 변경 | 새 QA·총괄 라운드 없음 | gate·역할 문서 대조 |
| C9 | 생성 최소화 | 신규 active 작업 | 위험 등급별 필수 canonical 파일 외 빈 템플릿·빈 폴더·관성적 역할별 보고서·중복 증거를 만들지 않음 | workspace·template·역할 규칙 대조 |
| C10 | 완료 최소화 | active에서 completed로 종결 | 같은 최소 작업 폴더를 이동하며 완료용 파일·패킷을 새로 복제하지 않음. 조건부 파일과 indispensable artifact만 함께 보존 | active/completed 수명주기 대조 |

## 공통 안전 불변식

- 기록 파일 수와 검증 진실성을 분리한다. R1~R3 모두 증상 은폐 금지, production 단일 owner, 후보 변경 시 PASS 무효화, correction 2회 중단·재분류, 고비용 preflight, 미검증 완료 금지를 유지한다.
- R1 `record.md`는 원요청·완료 주장, 등급 근거, owner·변경 파일, 금지 범위, 표적 검증 결과, correction, QA·총괄 적용 여부와 판정을 반드시 포함한다.
- 총괄 판정은 범위·승인 충돌의 내부 감사이며 사용자 승인을 대신하지 않는다.
- 기존 active/completed 이력에는 새 구조를 소급 적용하지 않는다.
- 향후 R3도 기본 기록은 `task.md`+`verification.md`이며, 분리 파일은 실제 다중 production owner 인계, 규제·릴리즈 추적, 세션 중단, 외부 차단 또는 기본 파일에 안전하게 통합할 수 없는 증거가 있을 때만 만든다.
- `artifacts/`는 실제 canonical 증거가 있고 원래 production/test/docs 위치를 참조하는 것만으로 부족할 때만 생성한다. 빈 폴더, 중복 복사본, 에이전트별 원문 보고서, 상태만 다른 세대 파일은 만들지 않는다.
- completed 보관은 active 최소 폴더의 이동이며 완료 시점에 새 `completion-report.md`나 복제 packet을 생성하지 않는다. 최종 상태·QA·총괄·사용자 수용은 `verification.md` 또는 R1 `record.md`에 통합한다.

## 사용자 승인 재분류

- 2026-08-06 correction 2/2 뒤 사용자가 `진행하고, 완료되면 커밋 푸쉬`를 명시해 재분류와 후속 교정을 승인했다.
- 재분류 root cause: 파일 생성 최소화 규칙은 반영됐지만, 주변 참조 문서의 오래된 무조건 QA·총괄 요구가 R1 조건부 역할 계약과 충돌했다.
- 새 revision: `workspace-minimal-generation-reclassified-r1-role-contract`
- 위험 등급: R3 유지 — 전역 운영 계약·역할·템플릿을 함께 변경함.
- production owner: 문서/릴리즈 owner 경로가 안전 검토에 막힌 뒤 사용자 명시 승인과 기존 예외 기록에 따라 조정자가 잔여 두 조건 문구만 직접 교정.
- 변경 계획: `agent-skill-plan.md`의 완료 차단·문제 사안 문구를 `해당 등급·변경에 필수인 경우`로 한정하고 전체 C1~C10을 새 독립 QA와 총괄이 재검증한다.
- reclassification correction cycle: 0/2

## 변경 후보와 단일 owner

| 불변식/문서군 | owner | 경계 |
| --- | --- | --- |
| 전역 원칙·최소 역할 | 문서/릴리즈 에이전트 | `AGENTS.md` 200줄 미만 |
| 실행 기준 | 문서/릴리즈 에이전트 | `loop-engineering-gates.md`가 유일 기준 |
| 역할·참조 색인 | 문서/릴리즈 에이전트 | 새 역할·스킬 생성 없음 |
| workspace·templates | 문서/릴리즈 에이전트 | 기존 작업 이력 삭제 없음 |
| board·cost 정책 | 문서/릴리즈 에이전트 | 이번 작업 상태는 현행 규칙으로 기록 |

### 구현 owner 예외 기록

- 문서/릴리즈 에이전트가 사용자 추가 범위 구현을 시도했으나 도구 안전 검토가 현재 사용자 승인 문맥을 보지 못해 전역 정책 변경을 차단했다.
- 사용자가 이 대화에서 active/completed의 향후 파일·폴더 생성 최소화와 프로젝트 규칙 갱신을 직접 승인했으므로, 조정자가 같은 task 계약 안에서 남은 운영 문서·템플릿 패치를 직접 완료했다.
- 새 작업 폴더·새 역할·새 스킬은 만들지 않았고 기존 이력 삭제·이동·소급 재작성은 하지 않았다.

## 예상 변경 파일

- `AGENTS.md`
- `docs/agents/loop-engineering-gates.md`
- `docs/agents/loop-engineering-user-guide.md`
- `docs/agents/agent-reference-map.md`
- `docs/agents/agent-skill-plan.md`
- `.agents/agent-roster.md`
- `.agents/documentation-release-agent.md`
- `.agents/project-director-agent.md`
- `_workspace/README.md`, `_workspace/active/README.md`, `_workspace/completed/README.md`
- `_workspace/templates/record.md` 및 관련 기존 템플릿 안내
- `docs/project-handoff/current-task-board.md`
- `docs/project-handoff/task-cost-dashboard.md`

## 검증 예산

- 구현자: `rg` 계약 검색, Markdown/링크·중복 규칙 정적 검사, `git diff --check` 각 1회
- 독립 QA: 현재 후보 1회, 첫 blocker에서 중지
- Unity/MCP/build/full suite/matrix/capture: 0
- correction cycle: 기존 C1~C8 correction 2/2 이력 보존. 2026-08-06 사용자 추가 범위 C9~C10으로 acceptance revision 갱신, 이전 QA는 `SUPERSEDED`
- S0 QA 판정: C1~C8 이전 revision PASS 이력. C4·C9·C10 갱신 revision은 구현 후 독립 QA 필요
- artifact budget: 텍스트 검증 기록 1개, 대형 로그·이미지 0

## 금지 범위

- 기존 `_workspace/active`·`completed` 파일 삭제 또는 일괄 이동
- 게임 기획·Unity 코드·씬·ProjectSettings·패키지·에셋 변경
- 독립 QA가 필요한 R2/R3 production 변경의 검증 생략
- 새 에이전트 역할 또는 Codex 스킬 생성

## 완료 조건

- C1~C10이 문서와 템플릿에서 단일 의미로 일치한다.
- 독립 QA PASS와 총괄 `내부 승인 가능` 판정이 있다.
- 변경 파일 외 사용자 변경을 보존하고 `git diff --check`가 통과한다.
- 실제 적용일·기존 이력 보존·남은 제한을 사용자에게 보고한다.
