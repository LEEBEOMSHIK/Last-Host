# 작업 배정서

2026-08-06 이후 신규 R2/R3 작업의 계획·S0·소유권 문서다. 실제 수행·QA·총괄·최종 상태는 `verification.md`가 소유한다. R3 분리 이력·활동·완료 보고는 기본 두 파일에 안전하게 통합할 수 없는 실제 추적 필요가 있을 때만 추가한다.

## 기본 정보

- 작업 ID:
- 작업명:
- 상태: 제안 / 승인 대기 / 승인됨 / 진행 중 / 검토 중 / 수정 필요 / 완료 / 보류
- 생성일:
- 담당 에이전트:
- 보조 에이전트:
- 사용 스킬:

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
|  |  |  |  |

## 구현 담당 확인

- 코드/테스트 변경 담당:
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당:
- 메인 에이전트 직접 구현 여부: 아니오 / 예
- 메인 에이전트 직접 구현 예외 사유:

## 루프 게이트

- 게이트 적용 대상: 예 / 아니오
- 위험 등급: R0 / R1 / R2 / R3
- 위험 등급 근거:
- 적용 사유:
- QA/검증 필요: 예 / 아니오
- 총괄 관리자 판정 필요: 예 / 아니오
- 커밋 전 차단 조건 확인 필요: 예 / 아니오
- correction cycle: 0/2
- capability profile / 요청 route:
- attempt ledger 경로 / 같은 criterion 연속 실패 수:

## S0 사용자 원증상·검증 charter

- 사용자 원문 또는 원증상:
- 재현 씬·입력·좌표·상태:
- 원증상 증거:
- 합성 oracle의 금지 결과:
- 합성 oracle의 허용 결과:
- 완료 주장 한 문장:

| criterion ID | 유형(원증상/성공/실패/경계/negative control/수명주기) | 입력·상태 | 기대값 | 최소 검증 |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

- QA S0 사전 검토:

## 고비용 preflight 입력

- agent brief JSON (`packet-only`, `fork_turns:none`, 필수 파일 3개 이하):
- verification current-state JSON:
- QA C# harness lint 경로:
- component contract baseline / candidate / test 경로:
- isolated Unity cache root / work ID marker:
- low-level runner 직접 Run 금지 확인:

## 목적


## 입력 자료

-

## 해야 할 일

1.
2.
3.

## 산출물

-

## production 소유권과 검증 예산

| production 파일/불변식 | 단일 구현 소유자 | 변경 금지/인계 조건 |
| --- | --- | --- |
|  |  |  |

- Unity session lease 예정 소유자:
- 관련 suite:
- 전체 suite 실행 조건:
- 대형 matrix 실행 필요·근거:
- artifact budget / criterion별 canonical 증거:

## 비용 계획

정확한 토큰·금액은 플랫폼 계측값이 있을 때만 기록한다. 실제 실행 근거와 비용 판정은 `verification.md`에 둔다.

| 비용 항목 | 계획 |
| --- | --- |
| 역할·인계 |  |
| 표적 검증 |  |
| Unity/MCP/빌드·full suite |  |
| matrix/capture·artifact |  |

- 중앙 현황판 대상 여부·행: R2/R3이므로 대상 / `docs/project-handoff/task-cost-dashboard.md`

## 금지 범위

-

## 승인 필요 항목

-

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인:
- 담당 에이전트 산출물 확인:
- 에이전트 수행 이력 확인:
- 구현 담당 에이전트 확인:
- 메인 에이전트 직접 구현 예외 사유 확인:
- QA/검증 에이전트 기록 확인:
- 총괄 관리자 판정 확인:
- 승인 게이트 확인:
- 완료 판단에 영향을 주는 미검증 항목:

## 완료 기준

-
