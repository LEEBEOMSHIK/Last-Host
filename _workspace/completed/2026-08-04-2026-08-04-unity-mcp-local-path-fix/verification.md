# 검증 기록

## 작업 ID

`2026-08-04-unity-mcp-local-path-fix`

## 검증 대상

프로젝트 로컬 `.codex/config.toml`의 Unity MCP relay 실행 경로

## 검증 담당

QA/검증 에이전트

## 원래 증상 또는 완료 주장

존재하지 않는 `C:\Users\User\...` 경로를 현재 컴퓨터의 `C:\Users\bumci\.unity\relay\relay_win.exe`로 교정한다.

## 현재 검증 revision

- 위험 등급: R1
- verification revision: R1-v1
- candidate fingerprint: `b28584a8b2cb9f4e757001452987ddec7e955668aa27cfe78548701f2d26409c`
- canonical run_id: `QA-20260804T142903+0900-b28584a8`
- candidate frozen 여부: 예
- 마지막 production 변경 시각/식별값: 구현 owner의 단일 줄 수정
- 이 검증이 마지막 production 변경 이후 실행됐는지: 예
- current-state JSON 대조: 해당 없음
- capability route / wrapper preflight: 정적 R1, 고비용 경로 미사용
- attempt ledger 연속 실패 / reclassification ID: 0 / 해당 없음

## Unity single-owner lease

- lease owner: 불필요
- editor PID / scene: 조작 없음
- 획득·해제 시각: 해당 없음
- baseline / final Play·Pause·scene·dirty: 변경 없음
- 임시 객체 유무: 없음

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 예
- 구현 주체가 실행한 검증과 별도로 확인한 항목: 후보 SHA, diff, TOML, relay 파일, 다른 설정·Unity 파일 비변경

## 실행한 검증

| criterion ID | 유형 | 검증 방법 | run_id | 결과 | canonical 증거 | 유효/SUPERSEDED |
| --- | --- | --- | --- | --- | --- | --- |
| C1 | 변경 범위 | Git diff | `QA-20260804T142903+0900-b28584a8` | command 한 줄만 변경 | QA 반환 기록 | 유효 |
| C2 | 문법·값 | TOML 독립 파싱 | `QA-20260804T142903+0900-b28584a8` | parse 성공, 기대 경로 일치 | QA 반환 기록 | 유효 |
| C3 | 환경 | relay 파일 존재 확인 | `QA-20260804T142903+0900-b28584a8` | 존재 | QA 반환 기록 | 유효 |
| C4 | negative control | 다른 `.codex` 설정·Unity 파일 diff 확인 | `QA-20260804T142903+0900-b28584a8` | 이번 owner 변경 없음 | QA 반환 기록 | 유효 |

## 검증하지 못한 항목

- 변경 설정을 읽은 새 Codex 세션의 실제 Unity MCP 연결

## 실패 또는 경고

- Git의 향후 LF→CRLF 변환 경고는 있으나 `git diff --check` 오류는 없음

## fail-fast·무효화

- first blocker: 없음
- blocker 발견 뒤 중지한 고비용 단계: 해당 없음
- correction cycle: 0/2
- 변경 뒤 무효화한 run/증거와 사유: 없음
- superseded_by: 없음
- S1~S5 한 revision 통과 여부: R1 정적 표적 검증 범위 통과
- S6 전체 suite 실행 허용/실행 횟수: 불필요 / 0
- S7 대형 matrix 실행 허용/실행 횟수: 불필요 / 0
- low-level runner token / 직접 Run 차단 확인: 직접 Run 없음
- isolated cache marker / Library reuse / cleanup 확인: 해당 없음

## 비용 실행 대조

| 비용 항목 | 계획 예산 | 실제 수·run_id/근거 | 정상/초과/미집계 | 필요한 비용/회피 가능 비용 |
| --- | --- | --- | --- | --- |
| 실제 역할·인계 | 구현1·QA1 | 구현1·QA1 | 정상 | R1 최소 역할 / 추가 역할 없음 |
| 표적 검증 | 구현1·QA1 | 각 1회 | 정상 | 필요한 정적 검증 / 중복 없음 |
| Unity/MCP/빌드 시작 | 0 | 0 | 정상 | 없음 / 고비용 회피 |
| full suite | 0 | 0 | 정상 | 없음 |
| matrix/capture·artifact | 0 | 0 | 정상 | 없음 |
| correction·무효/폐기 | 0 | 0 | 정상 | 없음 |

- 비용 판정: 정상
- 같은 fingerprint 중복·first blocker 뒤 고비용·no-result Unity·2회 미재분류·추가 역할·비원자 폐기 확인: 없음
- `docs/project-handoff/task-cost-dashboard.md` 갱신·독립 대조 여부: 예

## 게이트 판정

- QA/검증 게이트 통과 여부: 예
- `agent-activity.md`에 QA 판정 반영 여부: 예
- 총괄 관리자 검토로 넘길 수 있는지: 예

## 총괄 관리자 판정

- 내부 승인 가능
- 설정 경로 교정 완료로만 범위를 제한하며 실제 MCP 연결 완료는 주장하지 않는다.

## 완료 판단

- 기술 검증 통과

## 사용자 수용 상태

- 사용자 직접 확인 필요: Codex 재시작 후 실제 Unity MCP 도구 노출·읽기 호출
- 확인 전 `완료` 표현 금지 여부: 설정 파일 교정은 완료 가능, 실제 연결 완료는 별도 확인 전 주장 금지

## 완료 판단 근거

- 현재 후보의 경로, TOML 문법, 실제 파일, 변경 범위가 독립 검증을 통과했다.
