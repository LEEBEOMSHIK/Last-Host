# 루프 엔지니어링 운영 문서 보완 보고

## 담당과 범위

- 담당: 문서/릴리즈 에이전트
- 작업일: 2026-08-02 KST
- 위험 등급: R2 — 여러 운영 규칙·역할·검증 템플릿의 정합을 바꾸지만 Unity production 파일은 바꾸지 않음
- 입력 감사:
  - `overlap-incident-cost-analysis.md`
  - `current-loop-gap-audit.md`
  - `harness-improvement-design.md`
- 금지 범위 준수: Unity 코드·씬·ProjectSettings, 새 역할, 새 스킬을 생성·수정하지 않음

## 핵심 반영 결과

### 1. 유일 실행 기준과 위험 기반 최소 역할

- `docs/agents/loop-engineering-gates.md`를 루프의 유일 실행 기준으로 지정했다.
- R0~R3를 도입하고 모든 전문 역할을 자동 호출하지 않도록 했다.
- R1~R3 production·운영 변경의 독립 QA와 총괄 최종 판정은 유지했다.
- 총괄 사전 호출은 R3·승인 충돌·등급 불명확으로 제한하고, 정형 R1/R2 분류는 조정자가 수행하도록 했다.
- `AGENTS.md`의 기존 경량 루프가 독립 QA·총괄 문서를 생략할 수 있던 충돌을 제거했다.

### 2. 원증상·합성 oracle과 QA S0

- 사용자 원문, 재현 입력·좌표·상태, 실패 증거와 최종 합성 화면의 금지/허용 결과를 구현 전에 잠그도록 했다.
- QA를 구현 후 검사자뿐 아니라 구현 전 S0 charter 검토자로 이동했다.
- 성공·실패·경계·negative control·수명주기 criterion과 검증을 연결하는 필드를 `task.md`와 `verification.md`에 추가했다.
- 원증상 재현 또는 합성 oracle 잠금이 실패하면 추정 구현을 시작하지 않도록 했다.

### 3. production 단일 소유권과 correction cycle

- production 파일과 불변식별 단일 구현 소유자를 두고 QA가 production을 직접 고치지 않도록 했다.
- 상태 머신·게임플레이 수명주기와 씬·직렬화·wiring의 책임을 나눴다.
- QA 반환 뒤 수정자가 최소 관련 회귀를 통과해야 QA 재접수할 수 있게 했다.
- 같은 계약의 correction cycle 2회 실패 또는 QA 중 계약 2차 확장 시 `수정 필요 — 재분류`로 중단하도록 했다.

### 4. S1~S7 fail-fast

- 정적·컴파일 → 상태 단위 → 수명주기·관련 테스트 → scene smoke → 축소 경계 → 전체 회귀 → 최종 증거 순서를 고정했다.
- 첫 blocker에서 full suite, 전체 matrix, 다량 캡처를 중지하도록 했다.
- 전체 suite와 대형 matrix는 S1~S5가 같은 revision에서 통과한 freeze된 최종 후보에서 필요한 경우 각각 한 번만 실행하도록 했다.

### 5. 검증 revision과 증거 무효화

- `candidate_fingerprint`, `run_id`, `verification_revision`, canonical run을 도입했다.
- production·테스트·하네스·acceptance 변경별 PASS 무효화 범위를 명문화했다.
- 무효 증거는 `SUPERSEDED`로 기록하되 현재 통과 수에서 제외하도록 했다.
- Git HEAD나 `final-v2` 파일명만으로 dirty 후보·유효 증거를 판정하지 않도록 했다.

### 6. Unity single-owner lease와 원자 캡처

- Unity Editor, MCP, 같은 Library의 batch Unity를 한 시점에 한 소유자만 조작하도록 했다.
- lease에 work/agent/run/editor/scene/획득·만료/baseline 상태 필드를 두고 handoff 템플릿에 Play·Pause·scene·dirty·임시 객체·release 상태를 추가했다.
- 캡처 전 실제 root 단일성, `QA_Temp*`와 중복 player/controller/camera 부재를 확인하는 stale-object guard를 추가했다.
- 상태 설정·시스템 갱신·PNG·sidecar·checksum을 같은 frame transaction으로 묶는 원자 증거 계약을 추가했다.

### 7. artifact budget와 상태 용어

- criterion별 canonical 증거 1개를 기본으로 하고 중간 실패는 최소 반례와 핵심 로그 위치만 보존하도록 했다.
- `기술 검증 통과`, `기술 검증 통과 — 사용자 수용 대기`, `내부 승인 가능`, `완료`를 분리했다.
- 자동화할 수 없는 핵심 입력·화면·감각이 남으면 `완료`라고 표현하지 않도록 했다.

## 수정 파일

- 상위·색인: `AGENTS.md`, `docs/agents/agent-reference-map.md`, `docs/agents/agent-skill-plan.md`
- 유일 실행 기준: `docs/agents/loop-engineering-gates.md`
- 역할: `.agents/project-coordinator-agent.md`, `.agents/qa-verification-agent.md`, `.agents/project-director-agent.md`
- 검증 스킬: `.codex/skills/unity-verification-runner/SKILL.md`, `references/verification-rules.md`
- 작업영역·템플릿: `_workspace/README.md`, `templates/task.md`, `verification.md`, `handoff.md`, `agent-activity.md`

## 자체 검토

- `AGENTS.md`: 139줄로 200줄 미만 유지
- `git diff --check`: 오류 없음(저장소 기존 CRLF 변환 경고만 출력)
- 독립 QA·총괄 최종 게이트 삭제: 없음
- 새 에이전트·스킬 생성: 없음
- Unity 파일 변경: 없음
- 중복 실행 규칙: 세부 절차를 `loop-engineering-gates.md`로 모으고 다른 파일은 역할별 요약·입력 필드만 유지

## 남은 보완과 QA 인계

- 이 문서의 최초 반영 범위는 운영 문서였다. 후속 하네스 담당이 lease·EditMode runner·fingerprint 도구를 `tools/verification/`에 추가했고 독립 QA r2를 통과했다. 범용 atomic GameView capture는 포함하지 않으며 시각 작업별 repo-owned Editor harness로 다룬다.
- 독립 QA는 R0/R1/R2/R3 예시를 템플릿에 대입해 최소 역할, 첫 blocker stop, 변경 후 무효화, correction cycle 2회, 사용자 수용 대기 표현이 모순 없이 동작하는지 확인해야 한다.
- 총괄 관리자는 독립 QA와 최종 판정이 약화되지 않았는지, 문서 규칙이 승인 범위를 바꾸지 않는지 확인해야 한다.

## 후속 총괄 blocker 보완

- 독립 QA `process-harness-qa-r2`는 lease schema 2, XML, fingerprint, timeout과 위험 등급 문서 정합을 PASS했다.
- `agent-skill-plan.md`의 전체 운영 구조에서 모든 목표에 총괄 사전 확인을 요구하던 중복을 제거했다. 총괄 사전 검토는 R3·승인 범위 충돌·등급 불명확에서만 필수이고, R1/R2 정형 분류는 조정자가 수행한다. R1~R3 최종 총괄 판정은 유지한다.
- `_workspace/templates/task-r1-summary.md`를 추가했다. R1은 원증상·완료 주장·변경 파일/owner·표적 테스트·금지 범위·correction cycle·QA·총괄만 기록하며 R2/R3 정식 S0 표를 요구하지 않는다.
- `_workspace/README.md`, `loop-engineering-gates.md`, `agent-reference-map.md`에서 R1 요약형과 R2/R3 정식 템플릿 경로를 연결했다.
- 사용자용 통합 보고와 작업 패킷은 QA r2 PASS·총괄 1차 수정 필요로 동기화했다. 총괄 blocker로 변경된 문서는 새 fingerprint/run_id의 표적 QA 뒤 총괄 재검토로 넘긴다.

## 사용자용 지속관리 가이드 후속 보완

- 후속 요청에 따라 `docs/agents/loop-engineering-user-guide.md`를 새 공식 사용자·온보딩 참고 문서로 작성했다.
- 문서 첫머리에 비실행 요약임을 명시하고 `loop-engineering-gates.md`만 유일 실행 기준으로 유지했다.
- 0결함·0비용 비보장 경계와 대신 제공하는 통제, R0~R3 역할·문서·예상 실행 범위, 역할별 검증 책임, 허용 재실행 3조건, S0~S7 first blocker stop, correction cycle 2회 재분류를 통합했다.
- full suite 1회, 대형 matrix 1회, criterion canonical evidence 1개, 대량 캡처 금지와 verification revision·fingerprint·`SUPERSEDED`·상태-only `unaffected` 규칙을 사용자 관점에서 설명했다.
- Unity lease의 협력적 잠금 성격, 실제 Unity live/Editor PID 결합 미실행, 범용 atomic capture 부재, caller-managed fingerprint dependency 한계를 숨기지 않았다.
- `docs/README.md`와 `agent-reference-map.md`에서 에이전트 배정·검증·비용·중복 검증 문의 시 필수 사용자 참고로 연결했다.
- 실행 기준에는 `중복 검증 방지와 canonical 실행 소유권` 절만 추가했다. 구현자 표적 검증 1회, 독립 QA 핵심·freeze 후 전체 suite canonical 1회, 총괄 테스트 직접 실행 금지, 후보 변경·실패 재현·독립성 확보 때만 재실행 허용을 고정했다.
- Unity, 스크립트, 템플릿, AGENTS.md, 새 에이전트·스킬은 변경하지 않았다.
- 현재 상태는 `사용자용 운영 가이드 보완 진행 중, active·미커밋`이다. 이 후속 변경에 대한 독립 QA·총괄 판정은 아직 기록하지 않았다.
- 자체 위생 검사: `git diff --check` 통과, 새 가이드 후행 공백 없음, 필수 문서 경로 존재 확인. 독립 QA 인계 전 기능·Unity 실행은 하지 않았다.
