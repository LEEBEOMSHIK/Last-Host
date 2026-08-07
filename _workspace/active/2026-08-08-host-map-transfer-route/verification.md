# 검증 기록

## 작업 ID

`2026-08-08-host-map-transfer-route`

## 검증 대상

- `docs/design/hosts/host-map-transfer-route.md`
- `docs/design/hosts/README.md`

## 검증 담당

- 작성자 표적 검증: 메인 조정자
- 독립 QA: `qa-static-002` PASS
- 총괄 감사: 프로젝트 총괄 관리자

## 실제 수행·검증 이력

| 역할/에이전트 | 실제 수행 | 산출물·판정 |
| --- | --- | --- |
| 메인 조정자 | 문서 작성, 자리표시자·승인 경계 검색, `git diff --check`, 변경 범위 확인 | 작성자 표적 검증 PASS |
| 프로젝트 총괄 관리자 | C1~C5 내용과 승인 범위 1차 감사 | 본문 통과, R2 재분류·독립 QA 기록 요청 |
| QA/검증 에이전트 | 동결 후보 C1~C5, 자리표시자, 모순, 대표 루트 표현, diff 범위 독립 확인 | C1~C5 PASS, 명료성 개선 2건 비차단 권고 |
| QA/검증 에이전트 | 권고 반영 후보에서 C1~C5 재확인 | `qa-static-002` 최종 PASS, blocker 없음 |

- 검증 에이전트: QA/검증 에이전트
- 검증 요청자: 메인 조정자
- 검증한 산출물: 위 production 문서 2개
- 조건부 R3 분리 이력 생성 사유·반영 여부: 미생성

## 입력 자료

- `task.md` S0 C1~C5
- 위 검증 대상의 현재 Git diff

## 원래 증상 또는 완료 주장

- 완료 주장: 숙주 범위, 생활권 지역군, 전이 유형, 대표 분기, 조건부 이전 맵 복귀와 승인 경계를 한 문서에서 확인할 수 있다.

## 현재 검증 revision

- 위험 등급: R2
- verification revision: `host-map-route-r2-v1`
- candidate fingerprint: `host-map-transfer-route.md` SHA256 `36C87D4489C16B0E2291AB0364E8F59011C13DC461CA494D6D8D9EC3224E67A1`; `README.md` SHA256 `E5849D294C613FA51BB05B79893839754217998B7E21028CCD034B6840C28348`
- canonical run_id: `qa-static-002`
- candidate frozen 여부: 예
- 마지막 production 변경 시각/식별값: 2026-08-08 현재 working tree
- 이 검증이 마지막 production 변경 이후 실행됐는지: 예 — 권고 2건 반영 뒤 `qa-static-002` 재진입 PASS
- current-state JSON 대조: 해당 없음
- capability route / wrapper preflight: 정적 문서 검증 / Unity preflight 불필요
- attempt ledger 연속 실패 / reclassification ID: 0 / `R1-to-R2-director-audit-20260808`

## Unity single-owner lease

- lease owner: 해당 없음
- editor PID / scene: 해당 없음
- 획득·해제 시각: 해당 없음
- baseline / final Play·Pause·scene·dirty: 해당 없음
- 임시 객체 유무: 없음

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 예
- 구현 주체가 실행한 검증과 별도로 확인한 항목: C1~C5, 자리표시자, 내부 모순, 대표 루트 오해 가능성, 변경 범위

## 실행한 검증

| criterion ID | 유형 | 검증 방법 | run_id | 결과 | canonical 증거 | 유효/SUPERSEDED |
| --- | --- | --- | --- | --- | --- | --- |
| 작성자 정적 검사 | 성공 | `rg`, `git diff --check`, scoped `git status` | `author-static-001` | PASS | 현재 문서와 Git diff | 유효 |
| C1~C5 | 독립 QA | 본문·색인·S0 대조와 정적 검사 | `qa-static-001` | PASS, 비차단 명료성 개선 2건 | QA 회신 | SUPERSEDED — 권고 반영으로 production 변경 |
| C1~C5 | 독립 QA 재진입 | 권고 2건 반영 후보의 전체 기준 회귀와 `git diff --check` | `qa-static-002` | PASS, blocker 없음 | 최종 후보 문서와 QA 회신 | 유효·canonical |

```text
명령 또는 확인 방법: 자리표시자·승인 경계·포식성 조류·이전 맵 문구 검색, git diff --check, 대상 경로 status
결과: 자리표시자 없음, 공백 오류 없음, 대상은 새 문서·색인·작업 패킷으로 제한됨
해석: 독립 QA로 넘길 수 있는 동결 후보
```

## 검증하지 못한 항목

- 실제 캠페인 플레이, 맵 이동, 숙주 전이는 구현되지 않았으므로 검증 대상이 아니다.
- 숙주 종과 정확한 캠페인 순서는 사용자 후속 결정 대상이다.

## 실패 또는 경고

- 최초 R1 분류는 총괄 감사에서 부적절 판정되어 R2로 교정했다. 본문 내용 수정 요구는 없었다.

## fail-fast·무효화

- first blocker: R1 기록 구조가 새 캠페인 설계 계약의 위험도에 비해 부족함
- blocker 발견 뒤 중지한 고비용 단계: 해당 없음
- correction cycle: 1/2 — QA 비차단 명료성 권고 2건 반영
- 변경 뒤 무효화한 run/증거와 사유: R1 `record.md`는 R2 패킷으로 대체. `qa-static-001`은 지역군 양방향 표시와 환경 전이의 세균 운반 표현을 반영해 무효화
- superseded_by: `task.md`, `verification.md`
- S1~S5 한 revision 통과 여부: 실행 변경이 없는 문서 route에서 최종 revision 정적 QA PASS
- S6 전체 suite 실행 허용/실행 횟수: 해당 없음 / 0
- S7 대형 matrix 실행 허용/실행 횟수: 해당 없음 / 0
- low-level runner token / 직접 Run 차단 확인: 실행 0
- isolated cache marker / Library reuse / cleanup 확인: Unity 미사용

## 비용 실행 대조

| 비용 항목 | 계획 예산 | 실제 수·run_id/근거 | 정상/초과/미집계 | 필요한 비용/회피 가능 비용 |
| --- | --- | --- | --- | --- |
| 실제 역할·인계 | 작성 1, QA 1, 총괄 1 | 작성 1, 총괄 1차 1, QA 1+재진입 1 | 정상 | 권고 반영으로 QA 재진입 필요 |
| 표적 검증 | 작성자 1, QA 1 | `author-static-001` 1, `qa-static-001` 1, `qa-static-002` 1 | 정상 | 후보 변경에 따른 필수 재확인 |
| Unity/MCP/빌드 시작 | 0 | 0 | 정상 | 불필요 비용 회피 |
| full suite | 0 | 0 | 정상 | 불필요 비용 회피 |
| matrix/capture·artifact | 0 | 0 | 정상 | 불필요 비용 회피 |
| correction·무효/폐기 | 최대 2 | 분류 교정 1, production 폐기 0 | 정상 | 기록 계약 교정 필요 |

- 비용 판정: 정상
- 같은 fingerprint 중복·first blocker 뒤 고비용·no-result Unity·2회 미재분류·추가 역할·비원자 폐기 확인: 없음
- `docs/project-handoff/task-cost-dashboard.md` 갱신·독립 대조 여부: 기존 다른 작업의 미커밋 변경과 충돌을 피하기 위해 사용자 검토 전 보류, 본 파일에 비용 단일 기록

## 최종 증거 원자성

- 대상 Root/GlobalObjectId/instance count: 해당 없음
- stale·중복 player/controller/camera guard: 해당 없음
- 캡처와 sidecar의 run/fingerprint 일치: 해당 없음
- Console error count: 해당 없음
- scene dirty before/after: Unity 미사용
- evidence manifest: production 문서 2개, R2 패킷 2개
- canonical evidence와 artifact budget 준수: 예

## 게이트 판정

- QA/검증 게이트 통과 여부: PASS — `qa-static-002`, C1~C5, blocker 없음
- 조건부 R3 분리 이력에 QA 판정 반영 여부: 미생성
- 총괄 관리자 검토로 넘길 수 있는지: 예

## 프로젝트 총괄 관리자 판정

- 판정: 내부 승인 가능
- 근거: R2 기록 동기화, 최종 후보 `qa-static-002` C1~C5 PASS, blocker 없음
- 승인 범위·사용자 수용 대기: 전체 캠페인 구현은 미승인, 사용자 문서 검토 대기

## 완료 판단

- 기술 검증 통과 — 사용자 수용 대기

## 사용자 수용 상태

- 사용자 직접 확인 필요: 숙주 범위, 지역군, 대표 루트와 복귀 규칙
- 확인 전 `완료` 표현 금지 여부: 예

## 완료 판단 근거

- 작성자 정적 검사 PASS, 독립 QA `qa-static-002` C1~C5 PASS, 프로젝트 총괄 `내부 승인 가능`

## 최종 상태

- 완료/보류/승인 대기: 사용자 문서 검토 대기
- 완료 경로와 Git 상태: `_workspace/active/2026-08-08-host-map-transfer-route/`, 사용자 검토용 선별 커밋 대상
