# 루프 엔지니어링 사용자 가이드

> 이 문서는 사용자·신규 참여자용 요약이다. 프로젝트 작업의 위험 등급, 실행 순서, 검증 증거, 완료·커밋 차단 조건을 정하는 **유일한 실행 기준은 [`loop-engineering-gates.md`](loop-engineering-gates.md)**다. 두 문서가 충돌하면 실행 기준을 따르고 이 가이드를 갱신한다.

- 최종 수정일: 2026-08-02
- 문서 관리 책임: 프로젝트 조정 에이전트가 변경 필요를 접수하고, 문서/릴리즈 에이전트가 갱신하며, 독립 QA가 실행 기준과의 정합을 검증하고, 프로젝트 총괄 관리자가 최종 판정한다.
- 적용 대상: 코드, 테스트, Unity 씬·에셋·설정, 승인·운영 문서와 그 완료·커밋 주장

## 이 체계가 보장하는 것과 보장하지 못하는 것

이 체계는 **0결함·0재작업·0비용을 보장하지 못한다.** 소프트웨어 결함, 사람의 해석 차이, Unity·MCP·운영체제 같은 외부 상태, 자동화할 수 없는 화면·조작감 때문에 검증을 통과한 뒤에도 문제가 발견될 수 있다. 비용 절감률도 실제 계측 없이 약속하지 않는다.

대신 다음 통제를 보장한다.

- 사용자 원증상과 완료 기준을 구현 전에 고정한다.
- 위험도에 필요한 최소 역할만 투입하되, R1~R3의 독립 QA와 총괄 최종 판정은 유지한다.
- 가장 싸고 직접적인 검증부터 실행하고 첫 실패에서 고비용 검증을 중단한다.
- 구현자·독립 QA·총괄의 책임과 canonical 실행 소유자를 분리한다.
- 후보 변경 뒤 이전 PASS를 현재 증거로 재사용하지 않는다.
- 전체 suite, 대형 matrix, 캡처 산출물의 반복 생성을 제한한다.
- 자동 검증과 사용자의 실제 조작·화면 수용을 다른 상태로 보고한다.

즉, 결함을 없다고 약속하는 체계가 아니라 결함을 더 이르고 싸게 찾고, 무엇을 검증했는지 추적하며, 미검증을 완료로 포장하지 못하게 하는 체계다.

## R0~R3: 작업 크기별 필요한 범위

| 등급 | 대표 작업 | 필요한 역할 | 기본 문서 | 예상 검증·실행 범위 |
| --- | --- | --- | --- | --- |
| R0 조회 | 파일·상태 조회, 설명, 변경 없음 | 메인 응답 | 작업 패킷 없음 | 읽기 대조만. QA·총괄·Unity 실행 없음 |
| R1 국소 수정 | 기존 계약 안의 문구·값·1~3개 파일 국소 코드 | 구현 소유자 1명, 독립 QA, 총괄 | `_workspace/templates/task-r1-summary.md`, 수행 기록, QA·총괄 판정 | 구현자 표적 검증 1회, QA 원증상·수용 핵심 표적 검증 1회. 전체 suite·대형 matrix는 원칙적으로 생략 |
| R2 상태/통합 수정 | 새 상태 전이, 씬+코드 결합, 물리·가림·정렬·직렬화, 사용자 가시 결함 | 조정자, QA S0, 불변식별 구현 소유자, 필요 전문 역할, 독립 QA, 총괄 | 정식 `task.md`, `agent-activity.md`, `verification.md`, 판정 기록 | S0~S5 단계 검증 후 후보 freeze. QA가 필요한 전체 suite를 canonical 1회, 필요한 수용 증거 1회 실행 |
| R3 구조/릴리즈 | 패키지, ProjectSettings, 렌더 파이프라인, 저장 형식, 대규모 씬·아트, 릴리즈 빌드 | R2 역할 + 구조·통합·릴리즈 위험에 필요한 전문 역할, 사전 총괄 검토 | R2 문서 + 승인·빌드·릴리즈 근거 | S0~S7 전체. 빌드·전체 회귀는 freeze된 후보에서 QA canonical 1회씩, 승인 범위까지 대조 |

파일 수가 적어도 새 상태, 직렬화, 사용자 화면, 복구 난이도가 커지면 R2/R3가 될 수 있다. 구현 중 예상 밖의 상태·설정·담당 밖 production 파일이 생기면 즉시 멈추고 재분류한다.

## 역할별 검증 책임과 canonical 실행 소유권

| 역할 | 해야 하는 검증 | 기본 실행 횟수 | 하면 안 되는 일 |
| --- | --- | --- | --- |
| 구현자 | 자신이 바꾼 불변식의 가장 작은 표적 검증 | 후보 제출 전 1회 | QA 대신 전체 수용 판정, 불안정 후보에서 전체 suite·대형 matrix 반복 |
| 독립 QA | 사용자 원증상 oracle과 수용 핵심을 독립 재현·검증 | 현재 freeze 후보에서 핵심 검증 1회 | production 수정, 첫 blocker 뒤 고비용 단계 계속 실행 |
| 독립 QA | 프로젝트 전체 suite와 필요한 대형 matrix·최종 증거 | S1~S5 green 뒤 canonical 1회 | 구현자와 같은 실행을 이유 없이 복제, 여러 run을 동시에 canonical 지정 |
| 프로젝트 총괄 관리자 | QA 증거, fingerprint, 승인 범위, 상태판, 사용자 수용 대기 감사 | 최종 판정 1회 | TestRunner·MCP Play·빌드·캡처 같은 테스트 직접 실행, QA 역할 대행 |

구현자 검증은 빠른 결함 차단이고, QA 검증은 독립성 확보이며, 총괄은 증거 충분성 감사다. 이름이 비슷한 테스트라도 목적이 다르면 정당할 수 있지만, 같은 목적·후보·criterion의 반복은 중복이다.

## 동일 검증을 다시 실행해도 되는 경우

동일 criterion이나 suite의 재실행은 다음 세 경우에만 허용한다.

1. **후보 변경**: production·관련 테스트·하네스·계약 변경으로 이전 PASS가 무효화됐다.
2. **실패 재현**: 첫 blocker의 최소 반례를 확정하거나 수정 후 해당 blocker가 해소됐는지 확인한다.
3. **독립성 확보**: 구현자 자체 검증 뒤 독립 QA가 현재 freeze 후보를 별도 환경·책임으로 한 번 확인한다.

재실행할 때는 `run_id`, 대상 `candidate_fingerprint`, 재실행 사유, 대체되는 run을 기록한다. 단순 불안감, 담당자 수 늘리기, 로그를 더 많이 남기기, `final-v2` 파일을 만들기 위한 반복은 금지한다. 후보가 바뀌지 않은 같은 전체 suite·대형 matrix·캡처의 반복은 새 증거가 아니다.

## S0~S7 fail-fast 흐름

| 단계 | 먼저 확인하는 것 | 실패 시 행동 |
| --- | --- | --- |
| S0 계약 고정 | 사용자 원증상, 입력·상태, 금지/허용 화면, criterion, owner | 재현 불가 또는 oracle 불명확이면 구현하지 않고 질문·차단 |
| S1 정적·컴파일 | diff, 정적 validation, 컴파일, Console | 첫 오류만 최소 반례로 반환하고 중지 |
| S2 상태 단위 | 성공·실패·경계·negative control, 상태·소유권 | 관련 단위 결함 수정 전 뒤 단계 금지 |
| S3 수명주기·관련 테스트 | save/reopen, reload, enable/disable, 관련 EditMode | 직렬화·수명주기 결함 수정 전 scene smoke 금지 |
| S4 대상 scene smoke | 실제 root 1개, 원증상, wiring, Console | 실제 씬 실패 시 matrix·전체 suite 금지 |
| S5 축소 경계 | 위험 기반 pairwise, 경계 왕복, 대표 geometry | 첫 경계 blocker에서 전체 조합·전체 suite 금지 |
| S6 전체 회귀 | freeze된 같은 후보의 프로젝트 회귀 | 실패 run을 보존하고 수정·무효화 뒤 필요한 최소 단계부터 재시작 |
| S7 최종 증거 | 사용자 판단에 필요한 canonical matrix·캡처·dirty 보호 | 원자성·대상 identity가 틀리면 증거 폐기 후 원인부터 수정 |

핵심은 **first blocker stop**이다. 첫 실패를 찾은 순간 뒤의 full suite, 전체 matrix, 대량 캡처를 실행하지 않는다.

## correction cycle 2회 재분류

blocker 수정 후 QA에 다시 접수하는 것을 correction cycle 1회로 센다. 같은 S0 계약에서 두 번 연속 실패하거나 QA 중 완료 계약이 두 번째로 확장되면 패치 추가를 멈춘다.

상태를 `수정 필요 — 재분류`로 바꾸고 다음을 다시 정한다.

- 실제 root cause와 사용자 원증상
- 상태 전이와 수명주기 불변식
- R0~R3 위험 등급
- production 파일·불변식별 단일 owner
- 계속 패치할지, 구조를 바꿀지에 대한 대안

재분류는 실패를 통과로 바꾸거나 독립 QA·총괄을 생략하는 절차가 아니다.

## 비용·산출물 예산

- 전체 suite: freeze된 후보에서 QA canonical **최대 1회**
- 대형 matrix: criterion에 정말 필요할 때 **최대 1회**
- criterion별 canonical evidence: 기본 **1개**; 여러 파일이면 manifest 1개가 묶음을 소유
- 중간 실패: 결함명, 최소 반례, 핵심 로그 위치만 보존
- 금지: 같은 의미의 대량 PNG·CSV·전체 로그 세대, 불안정 후보의 반복 빌드·전체 suite, 증거용이 아닌 대량 캡처

`1회`는 영구적으로 다시 실행할 수 없다는 뜻이 아니다. 후보 변경·실패 재현·독립성 확보 사유가 생기면 새 `run_id`로 재실행하되 이전 증거를 `SUPERSEDED` 처리한다.

## verification revision과 증거 유효성

- `candidate_fingerprint`: 검증 대상 production·테스트·씬·패키지·버전·필요 설정의 내용 식별자
- `run_id`: 한 번의 검증 실행과 증거 묶음 식별자
- `verification_revision`: S0 계약과 candidate fingerprint를 합친 판정 단위
- `canonical`: 현재 판정에 쓰는 유효 run 하나
- `SUPERSEDED`: 변경으로 무효화됐거나 후속 canonical run으로 대체된 과거 증거

production, 관련 테스트, 캡처·matrix 하네스, acceptance contract가 바뀌면 영향받은 PASS는 무효다. 삭제할 필요는 없지만 현재 통과 수에 합산하지 않고 후속 run을 연결한다.

반대로 QA 설명 문구, 상태판, handoff처럼 기능 후보를 바꾸지 않는 **상태-only 변경**은 기능 증거에 `unaffected`다. 이 경우 새 기능 QA run을 만들지 않고 어떤 파일이 상태-only였는지 기록한다. 상태-only라는 이름으로 계약이나 production 변경을 숨기면 안 된다.

## Unity lease와 현재 자동화 한계

Unity Editor, MCP, TestRunner, 같은 `Library`를 쓰는 batch Unity는 프로젝트별 single-owner lease를 사용한다. lease owner만 Play/Pause, scene open/save, Console clear, TestRunner, capture, Refresh를 실행한다. 인계할 때 Play 종료, 임시 객체 제거, scene dirty 복원, release 상태를 기록한다.

현재 한계도 함께 봐야 한다.

- lease는 협력적 잠금이다. 규칙을 무시한 외부 MCP·Editor 조작을 운영체제 수준에서 완전히 차단하지 못한다.
- 이번 감사에서 새 EditMode runner로 실제 Unity live batch와 실제 Editor PID/MCP lease 결합을 실행하지 않았다.
- 범용 atomic GameView capture 도구는 없다. 시각 작업마다 scene·root·상태를 아는 저장소 소유의 task-specific Editor harness가 필요하다.
- fingerprint는 호출자가 지정한 dependency만 보호하며 누락 파일을 자동 추론하지 않는다.
- 자동 캡처가 성공해도 실제 키 입력 감각, 화면 품질, 무설명 이해도는 사용자 수용을 대체하지 못한다.

## 사용자가 확인할 체크리스트

작업 보고를 받을 때 다음을 확인하면 된다.

작업별 비용은 [`task-cost-dashboard.md`](../project-handoff/task-cost-dashboard.md)에서 한꺼번에 확인한다. 정확한 토큰·금액은 플랫폼 계측이 있을 때만 보며, 평소에는 계획 대비 실제 역할·인계, Unity/MCP/빌드·full suite, matrix/capture, correction, 무효·폐기 증거와 `정상 / 주의 / 과다 / 미집계` 판정을 확인한다. `주의`·`과다` 행은 필요한 비용과 회피 가능 비용이 분리됐는지, `미집계`를 0으로 오해하지 않았는지를 근거 작업 경로에서 대조한다.

- [ ] 작업이 R0~R3 중 무엇이며 그 이유가 한 문장으로 적혀 있는가?
- [ ] 내가 말한 원증상과 완료 화면·동작이 criterion에 직접 연결됐는가?
- [ ] 구현자, 독립 QA, 총괄이 서로 다른 책임으로 기록됐는가?
- [ ] 구현자는 표적 검증, QA는 수용 핵심과 freeze 후 canonical 검증, 총괄은 증거 감사만 했는가?
- [ ] 첫 실패 뒤 전체 suite·대형 matrix·캡처가 계속 실행되지 않았는가?
- [ ] 전체 suite와 대형 matrix가 필요했다면 canonical run이 각각 하나인가?
- [ ] 후보 변경 뒤 과거 PASS가 `SUPERSEDED` 처리됐는가?
- [ ] Unity 작업이면 lease owner, scene, Play·dirty·임시 객체 정리 상태가 있는가?
- [ ] 자동 검증 통과와 내가 직접 확인할 WASD·화면·품질 항목이 분리됐는가?
- [ ] `내부 승인 가능`, `사용자 수용 대기`, `완료`, `미커밋`이 실제 상태와 일치하는가?
- [ ] 작업 시작·blocker/correction·사용자 보고/커밋 전에 비용 중앙 현황판이 갱신됐는가?

## 이상 징후

다음 중 하나가 보이면 완료·커밋 전에 이유를 물어봐야 한다.

- 작업 등급과 owner 없이 여러 에이전트가 같은 production 파일을 번갈아 수정한다.
- 사용자 원증상 대신 테스트 개수나 내부 수치만 완료 근거로 제시한다.
- 후보가 고정되기 전에 전체 suite·72/432 같은 대형 matrix·대량 PNG를 반복한다.
- 첫 blocker가 있는데 뒤 단계 결과까지 함께 보고한다.
- 같은 후보·criterion에 canonical run이 둘 이상이다.
- production 변경 후 이전 PASS를 그대로 인용하거나 `final-v2` 파일명만으로 최신이라 주장한다.
- QA가 production을 직접 고치거나 총괄이 테스트 실행자로 등장한다.
- Unity 증거에 lease owner, 실제 scene/root, Console·dirty·임시 객체 상태가 없다.
- 자동화할 수 없는 사용자 체감을 확인하지 않고 `완료`라고 표현한다.
- correction cycle이 두 번을 넘었는데도 재분류 없이 작은 패치만 계속 붙인다.

## 문서 관리와 업데이트 조건

이 가이드는 다음 상황에서 갱신한다.

1. `loop-engineering-gates.md`의 R등급, S단계, 책임, 증거 규칙이 바뀐다.
2. 새 검증 도구가 실제 Unity live에서 채택되거나 기존 한계가 해소·추가된다.
3. 반복 비용 사고에서 새로운 구조 원인이나 이상 징후가 확인된다.
4. 사용자가 이해하기 어려운 상태 용어·보고 형식을 지적한다.

갱신 절차는 `조정자 접수 → 문서/릴리즈 수정 → 독립 QA 정합 검증 → 총괄 최종 판정 → 상태판 동기화`다. 실행 규칙이 바뀌면 이 가이드보다 `loop-engineering-gates.md`를 먼저 수정한다.

## 변경 이력

| 날짜 | 변경 | 상태 |
| --- | --- | --- |
| 2026-08-02 | 최초 작성. R0~R3, 역할별 검증 소유권, 중복 재실행 제한, S0~S7, correction cycle, 증거 예산·revision, Unity 한계, 사용자 체크리스트 통합 | QA r4 PASS, 총괄 내부 승인 가능, 사용자 확인 가능, active·미커밋 |
| 2026-08-02 | 작업 비용 중앙 현황판 확인 경로와 사용자 점검 항목 연결 | 작업 비용 중앙 현황판 보완 진행 중, active·미커밋 |
