# 검증 기록

## 작업 ID

`2026-08-07-main-scenario-outline`

## 검증 대상

- 전체 게임 시나리오·화면 흐름·튜토리얼 초안
- 내러티브 색인

## 검증 담당

- 정적 대조: 프로젝트 조정 에이전트, 독립 QA `main_scenario_qa`
- 내부 승인: 프로젝트 총괄 관리자 에이전트

## 실제 수행·검증 이력

| 역할/에이전트 | 실제 수행 | 산출물·판정 |
| --- | --- | --- |
| 프로젝트 조정 | R2 범위·S0 작성, 색인·상태판 통합, 정적 구조 검사 | PASS |
| 기획 정리 에이전트 | 전체 시나리오 production 문서 작성 | 500줄 초안 제출 |
| 독립 정적 QA | C1~C5 최초 검토, correction 1 재검토 | 최초 FAIL → correction 1 PASS |

- 검증 에이전트: `main_scenario_qa`
- 검증 요청자: 프로젝트 조정 에이전트
- 검증한 산출물: `docs/design/narrative/main-scenario-outline.md`, `docs/design/narrative/README.md`
- 조건부 R3 분리 이력 생성 사유·반영 여부 / 없으면 `미생성`: 미생성

## 입력 자료

- `docs/design/game-design-summary.md`
- `docs/prototype/official/rat-host-prototype.md`
- `AGENTS.md`

## 원래 증상 또는 완료 주장

- 전체 게임의 처음부터 엔딩까지 이어지는 시나리오·화면·튜토리얼 기준 문서가 없다.

## 현재 검증 revision

- 위험 등급: R2
- verification revision: correction 1
- candidate fingerprint: scenario `4A8CF27A4EF01A535B5C364738EA82A6856123DF84A184AE3A690D0542230065`; narrative index `794A62BC563BFA91BB58355B764C3FBCBA400F1E7D302261D480170602E7BC4B`
- canonical run_id: `qa-main-scenario-static-20260807-c1`
- candidate frozen 여부: 예 — 총괄 검토 전 production 문서 동결
- 마지막 production 변경 시각/식별값: correction 1 HUD 승인 상태 정정
- 이 검증이 마지막 production 변경 이후 실행됐는지: 예
- current-state JSON 대조: 해당 없음
- capability route / wrapper preflight: 문서 정적 검토 / 해당 없음
- attempt ledger 연속 실패 / reclassification ID: 0 / 해당 없음

## Unity single-owner lease

- lease owner: 해당 없음
- editor PID / scene: 해당 없음
- 획득·해제 시각: 해당 없음
- baseline / final Play·Pause·scene·dirty: 해당 없음
- 임시 객체 유무: 없음

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 예 — 기획 작성과 독립 정적 QA 분리
- 구현 주체가 실행한 검증과 별도로 확인한 항목: 전체 흐름, 구간별 필수 항목, 쥐 승인 범위, 장기 구현 미승인 경계, 색인 링크

## 실행한 검증

| criterion ID | 유형 | 검증 방법 | run_id | 결과 | canonical 증거 | 유효/SUPERSEDED |
| --- | --- | --- | --- | --- | --- | --- |
| C1 | 성공 | 첫 실행부터 엔딩 후까지 목차·본문 정적 대조 | `qa-main-scenario-static-20260807-c1` | PASS | 시나리오 3장·5장·9장 | 유효 |
| C2 | 경계 | 쥐 프로토타입과 승인 표기 대조 | `qa-main-scenario-static-20260807-c1` | PASS | 시나리오 2장·4.3절·5.11절·13장 | 유효 |
| C3 | 성공 | 5.1~5.17의 화면·행동·튜토리얼·연출·전환 항목 대조 | `qa-main-scenario-static-20260807-c1` | PASS | 17구간, 필수 표기 각 23회 | 유효 |
| C4 | negative control | 구현 미승인·금지 해석 문구와 Git 변경 범위 확인 | `qa-main-scenario-static-20260807-c1` | PASS | 시나리오 1장·13장, Unity 변경 0 | 유효 |
| C5 | 수명주기 | narrative README 상대 링크와 대상 파일 존재 확인 | `qa-main-scenario-static-20260807-c1` | PASS | `docs/design/narrative/README.md` | 유효 |

```text
명령 또는 확인 방법: Select-String 구조 계수, Test-Path, Git diff --check, 독립 QA 정적 대조
결과: Segments 17, Screens/Actions/Tutorials/Purposes/Transitions 각 23, 링크 대상 존재, diff --check 오류 0
해석: 사용자 보완용 전체 시나리오 초안의 구조와 승인 경계가 현재 기준에 맞는다.
```

## 검증하지 못한 항목

- 사용자 시나리오 내용 수용과 구체 보완

## 실패 또는 경고

- 최초 QA에서 `현재 모드` HUD를 프로토타입 승인 범위로 과대 표기한 C2 오류가 발견됐다.
- 최초 QA 입력에 narrative README가 없어 C5가 미검증이었다.
- correction 1에서 승인 상태를 정정하고 색인 링크를 확인해 C1~C5가 PASS했다.

## fail-fast·무효화

- first blocker: C2 승인 표기 오류와 C5 증거 부족
- blocker 발견 뒤 중지한 고비용 단계: Unity/MCP/build는 계획부터 0이며 실행하지 않음
- correction cycle: 1/2
- 변경 뒤 무효화한 run/증거와 사유: 최초 QA 판정은 C2 수정 전 revision이므로 SUPERSEDED
- superseded_by: `qa-main-scenario-static-20260807-c1`
- S1~S5 한 revision 통과 여부: 문서 정적 범위 PASS
- S6 전체 suite 실행 허용/실행 횟수: 해당 없음 / 0
- S7 대형 matrix 실행 허용/실행 횟수: 해당 없음 / 0
- low-level runner token / 직접 Run 차단 확인: 해당 없음
- isolated cache marker / Library reuse / cleanup 확인: 해당 없음

## 비용 실행 대조

| 비용 항목 | 계획 예산 | 실제 수·run_id/근거 | 정상/초과/미집계 | 필요한 비용/회피 가능 비용 |
| --- | --- | --- | --- | --- |
| 실제 역할·인계 | 기획1→조정1→총괄1 | 기획1, 조정1, 독립 QA 최초+correction1, 총괄1 | 주의 | 작은 승인 표기 correction으로 QA 재대조 1회 필요 |
| 표적 검증 | 정적 대조 1묶음 | 구조 검사1, 독립 QA 최초1+correction1 | 주의 | 최초 QA가 실제 C2 오류를 발견해 correction 재검토가 필요했음 |
| Unity/MCP/빌드 시작 | 0 | 0 | 정상 | 없음 |
| full suite | 0 | 0 | 정상 | 없음 |
| matrix/capture·artifact | 0 | 0 | 정상 | 없음 |
| correction·무효/폐기 | 0 | correction 1, 최초 QA 판정 SUPERSEDED | 주의 | 승인 오인 방지를 위한 필요한 비용 |

- 비용 판정: 주의 — correction 1, 고비용 실행 없음
- 같은 fingerprint 중복·first blocker 뒤 고비용·no-result Unity·2회 미재분류·추가 역할·비원자 폐기 확인: 해당 없음
- `docs/project-handoff/task-cost-dashboard.md` 갱신·독립 대조 여부: correction·QA·총괄 실제값 갱신

## 최종 증거 원자성

- 대상 Root/GlobalObjectId/instance count: 해당 없음
- stale·중복 player/controller/camera guard: 해당 없음
- 캡처와 sidecar의 run/fingerprint 일치: 해당 없음
- Console error count: 해당 없음
- scene dirty before/after: 해당 없음
- evidence manifest: production 문서 1개와 narrative 색인 1개
- canonical evidence와 artifact budget 준수: 예, 별도 artifact 미생성

## 게이트 판정

- QA/검증 게이트 통과 여부: PASS
- 조건부 R3 분리 이력에 QA 판정 반영 여부 / 없으면 `미생성`: 미생성
- 총괄 관리자 검토로 넘길 수 있는지: 예

## 프로젝트 총괄 관리자 판정

- 판정: 내부 승인 가능
- 근거: 17개 전체 구간과 구간별 5개 필수 항목, 실패·보상·엔딩 후 흐름, 독립 QA correction 1 PASS 및 승인 경계가 충분함
- 승인 범위·사용자 수용 대기: 검토용 초안 공개만 내부 승인. 장기 캠페인은 제안 초안이며 사용자 Q01~Q21 보완·승인 대기

## 완료 판단

- 기술 검증 통과 — 사용자 수용 대기

## 사용자 수용 상태

- 사용자 직접 확인 필요: 예
- 확인 전 `완료` 표현 금지 여부: 예

## 완료 판단 근거

- C1~C5 정적 QA와 총괄 내부 승인을 통과했으며 사용자 내용 수용·보완이 남았다.

## 최종 상태

- 완료/보류/승인 대기: 기술 검증 통과 — 사용자 수용 대기
- 완료 경로와 Git 상태: `_workspace/active/2026-08-07-main-scenario-outline/`; 미커밋
