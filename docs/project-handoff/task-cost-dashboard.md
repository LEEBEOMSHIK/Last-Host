# 작업 비용 중앙 현황판

최종 갱신: 2026-08-02 KST

> 정확한 토큰 수와 금액은 플랫폼이 작업별 계측값을 제공할 때만 기록한다. 계측값이 없으면 절대 추정하거나 절감률을 만들어내지 않는다. 이 저장소에서는 역할·인계, Unity/MCP/빌드 시작, 테스트·matrix·capture 실행, correction, 무효·폐기 증거와 산출물 크기처럼 작업 기록에서 직접 관찰 가능한 비용 proxy만 기록한다.

## 목적과 관리 책임

이 문서는 사용자가 작업마다 필요했던 비용과 회피 가능했던 비용을 한곳에서 비교하는 공식 중앙 현황판이다. 상세 근거는 각 작업의 `task.md` 또는 `task-r1-summary.md`, `agent-activity.md`, `verification.md`, `completion-report.md`와 artifact에 남긴다. 현황판은 그 근거를 요약하며 새로운 실행 기록을 만들어내지 않는다.

- 조정자: 작업 시작 시 행을 만들고 계획 예산을 기록하며 blocker·correction·보고·커밋 전에 실제값과 상태를 갱신한다.
- 구현자와 QA: 실제 역할·인계, 표적 검증, Unity/MCP/빌드, full suite, matrix/capture 실행 수와 `run_id` 근거를 제공한다.
- 독립 QA: 중복 실행, no-result 실행, 무효·폐기 증거와 필요한 비용/회피 가능 비용 분류를 대조한다.
- 프로젝트 총괄 관리자: 비용 판정의 근거 충분성, 미집계 공개와 기존 완료 게이트 비약화 여부를 감사한다.

## 비용 판정 규칙

| 판정 | 기준 |
| --- | --- |
| 정상 | 계획된 역할·검증·산출물 예산 안에서 끝났고 이유 없는 중복·폐기가 없다. |
| 주의 | 정당화된 계획 초과 또는 correction 1회가 있으며 사유·재실행 근거가 기록돼 있다. |
| 과다 | 같은 fingerprint의 full suite 중복, first blocker 뒤 고비용 계속, 결과 없는 Unity 실행, correction 2회 뒤 미재분류, 이유 없는 추가 역할·인계, 비원자 증거 폐기 중 하나 이상이 확인된다. |
| 미집계 | 계획 또는 실제 실행 근거가 부족해 정상·주의·과다를 판정할 수 없다. 누락값을 0으로 간주하지 않는다. |

`과다`는 기능 실패를 자동 통과시키거나 QA·총괄을 생략하는 판정이 아니다. 원인과 회피 가능 비용을 공개하고, correction 2회 조건이면 재분류가 끝날 때까지 다음 고비용 실행·완료·커밋을 차단한다.

## 중앙 현황

| 작업 ID / 작업명 | R등급 | 계획 역할·검증 예산 | 실제 역할·인계 | 표적 검증 | Unity/MCP/빌드 시작 | full suite | matrix/capture | correction cycle | 무효·폐기 실행/증거 | 비용 판정 | 필요한 비용 | 회피 가능 비용 | 근거 작업 경로 | 마지막 갱신 |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| `2026-08-02-production2d-visual-overlap-correction`<br>Production2D 쥐·오브젝트 가시 실루엣 겹침 완전 교정 | R2 | 조정·비주얼·씬/통합·조건부 게임플레이·독립 QA·총괄.<br>관련/전체 EditMode, MCP Play, 3오브젝트×경계 matrix, 최종 capture; legacy 계획은 실행 횟수 미지정 | 역할 6종, 독립 QA 1종.<br>씬/통합→게임플레이 런타임 소유권 추가 인계 1회.<br>QA→수정→QA 3회 | 구현자 EditMode 최소 5회(`5/8`,`8/8`,`46/46`,`200/200`,`46/46`), 런타임 static smoke 최소 3회 | 독립 QA batch Unity **5 starts**.<br>첫 1회 XML 없음.<br>MCP Play 시작 횟수 미집계, 빌드 실행 기록 없음 | 결과 있는 QA 전체 EditMode **4회**(`200/201`,`201/201`,`201/202`,`202/202`).<br>구현자 `200/200` 1회는 별도 | contact CSV 3세대, phase/stability/subpixel 각 2세대.<br>최종 PNG 4장 1세트 폐기 후 final-v2 4장.<br>전체 artifacts **34개 / 약 18.5MB** | QA correction **3회**.<br>당시 2회 재분류 규칙 부재 | no-result batch Unity 1회(약 3.99MB 로그), invalid capture set 1, pre/post/final 중간 증거 다수 | **과다 — 부분 회피 가능** | 합성 원인 진단, 상태형 resolver, 방향 collider, save/reload·ownership·hysteresis 관련 검증, 독립 QA 최종 full suite 1회, 최종 matrix/capture 1회, 총괄 판정 | 이전 합성 oracle 누락, 런타임 소유권 역전, 구현자 최소 회귀 누락, no-result cold run, QA full suite 4회 중 안정 후보 이전 반복, 경계검사 전 중간 matrix, 비원자 캡처 폐기 | 기능 `7ba12df fix: correct production 2d visual occlusion` origin/main 반영 완료<br>`_workspace/active/2026-08-02-production2d-visual-overlap-correction/`<br>`_workspace/completed/2026-08-02-2026-08-02-loop-harness-efficiency-audit/artifacts/overlap-incident-cost-analysis.md` | 2026-08-02 KST |
| `2026-08-02-loop-harness-efficiency-audit`<br>루프 엔지니어링·검증 하네스 비용 효율 감사와 보완 | R2 | 최초 계획: 조정자, QA, Unity 하네스 감사, 문서/릴리즈, 총괄.<br>문서 정합·예시 시뮬레이션, 필요한 범용 도구 negative control, `git diff --check`; Unity 플레이·빌드 없음 | 실제 역할 기록 9행(문서/릴리즈 2기능 포함): 조정, 프로세스 감사, 하네스 감사, 도구 구현, 사고 감사, 문서 구현, 사용자 보고, QA, 총괄.<br>QA revision **r1~r6**, 총괄 판정 **6회**(수정 필요 3, 내부 승인 3).<br>비용 dashboard: r5 QA 1회 FAIL, r6 QA 1회 PASS, 총괄 1차 수정 필요 1회, 최종 read-only 내부 승인 1회 | r1 lease blocker, r2 동적 negative-control 1묶음, r3/r4 표적 문서 QA.<br>r5 비용 현황판 QA 1회 FAIL(`정상` 정의), r6 표적 재QA 1회 PASS.<br>r5 manifest 1 + r6 manifest 1 | 전체 감사 실제 Unity **0**, MCP Play **0**, 빌드 **0**.<br>비용 dashboard r5/r6·총괄에서 동적 도구도 **0**.<br>r2 historical PowerShell negative-control만 1묶음 | Unity/프로젝트 full suite **0**.<br>비용 dashboard r5/r6도 **0** | 대형 matrix **0**, GameView capture **0**.<br>비용 dashboard r5/r6도 matrix/capture **0**.<br>전체 artifact 수·크기 미집계 | 공식 전체 correction cycle 수는 미집계.<br>관찰됨: QA r1→r2 보정 1회, 기존 총괄 문서·상태 보정 요구 2회.<br>비용 dashboard sub-correction **1/2** | r1 FAIL·r3 문서 판정·r5 FAIL은 `SUPERSEDED`.<br>r5/r6 manifest 각 1개.<br>r2 동적 증거는 `unaffected`.<br>상태-only 동기화는 새 기능 run 없음 | **완료 보관·운영 `533152e` origin/main 반영 — 과다 — 부분 회피 가능**<br>QA r6 PASS, 비용 현황판 최종 read-only 재대조 `내부 승인 가능`.<br>정확 token/금액 **미집계** | 사고·프로세스·하네스 분석, r2 negative-control, 변경 후보별 r3/r4 QA, r5 blocker 발견과 r6 재QA, 총괄의 구조·근거 감사와 최종 read-only 재대조, 완료 판정 반영용 상태-only 동기화 | r1 lease 불일치, 기존 stale 상태 재대조, r5 정의 축약 불일치 correction 1회, r6 PASS 뒤 dashboard·상태가 r5에 머물러 발생한 총괄 `수정 필요`와 추가 read-only 재대조. 변경 후보별 QA 자체는 필요한 비용이나 stale 상태 blocker는 회피 가능 | 운영 커밋 `533152e chore: improve loop verification efficiency` origin/main 반영 완료<br>`_workspace/completed/2026-08-02-2026-08-02-loop-harness-efficiency-audit/` | 2026-08-02 KST |

## 읽는 방법

1. `비용 판정`이 `주의` 또는 `과다`인 행에서 `필요한 비용`과 `회피 가능 비용`을 먼저 비교한다.
2. 실행 수가 `0`인지 `미집계`인지 구분한다. `0`은 기록으로 미실행이 확인된 값이고, `미집계`는 근거가 부족한 값이다.
3. correction, no-result Unity, 같은 fingerprint full suite, 폐기 capture가 있으면 근거 작업 경로에서 `run_id`·fingerprint·실패 원인을 확인한다.
4. 진행 중 행은 blocker·correction 때 바뀔 수 있다. 사용자 보고·완료·커밋 전 마지막 갱신과 QA·총괄 판정을 다시 확인한다.

## 변경 이력

| 날짜 | 변경 | 상태 |
| --- | --- | --- |
| 2026-08-02 | 중앙 현황판 생성, 비용 proxy·판정 규칙·관리 책임 정의, 겹침 교정과 루프/하네스 감사 초기 행 등록 | 작업 비용 중앙 현황판 보완 진행 중, active·미커밋 |
| 2026-08-02 | QA r6 PASS와 총괄 최종 read-only 재대조 반영, 중앙 비용 현황판 확정·지속 관리 시작 | 내부 승인 가능, 사용자 확인 가능, active·미커밋 |
| 2026-08-02 | 사용자 커밋 요청, 감사 완료 보관과 superseded raw evidence·Python cache 정리 반영 | 완료 보관·선별 커밋 대기 |
| 2026-08-02 | overlap 기능 구현 커밋 `7ba12df` 반영, 감사·운영 변경과 커밋 경계 분리 | overlap 구현 커밋 완료·감사 운영 커밋 대기 |
| 2026-08-02 | 기능 `7ba12df`와 운영 `533152e` origin/main 반영, 중앙 현황판 지속 관리 시작 | 원격 반영 완료·지속 관리 |
