# R1 국소 수정 요약 배정서

## 기본 정보

- 작업 ID: `2026-08-05-verification-loop-noise-reduction`
- 작업명: 검증 반복 체감과 사용자 보고 소음 축소
- 상태: 완료
- 생성일: 2026-08-05
- 위험 등급: R1 — 기존 fail-fast·2회 제한·상태-only 증거 계약을 3개 운영 문서에서 명확히 하는 국소 문서 변경

## 원증상과 완료 주장

- 사용자 원증상·재현: preflight 차단, 구현 후보 실패, 독립 QA, closeout 검토가 모두 `run` 또는 `QA`로 보이면서 `QA → 실패 → QA → 실패`가 과도하게 반복되는 느낌이 든다.
- 완료 주장 한 문장: 고비용 실행과 준비 차단을 구분하고, 구현 후보·독립 QA·상태-only 동기화의 최대 반복과 사용자 보고 시점을 한 번의 공통 규칙으로 고정한다.

## 변경 파일과 단일 owner

| 변경 파일 | production owner |
| --- | --- |
| `docs/agents/loop-engineering-gates.md` | 문서/릴리즈 에이전트 |
| `docs/agents/loop-engineering-user-guide.md` | 문서/릴리즈 에이전트 |
| `.agents/project-coordinator-agent.md` | 문서/릴리즈 에이전트 |

## 표적 테스트

- 구현자 표적 테스트: 세 문서의 용어·횟수·상태 전이·링크 정합, `git diff --check`, Unity/MCP/build 시작 0
- 독립 QA 표적 재검증: 실행 기준↔사용자 가이드↔조정자 절차의 동일 계약, 기존 fail-fast·독립 QA·총괄 게이트 비약화, 상태-only 예외 범위 정적 대조 1회

## 금지 범위

- 독립 QA와 총괄 최종 게이트 삭제, 실제 결함 실패 통과, wrapper/runner 코드 변경, Unity/MCP/build 실행, 게임 코드·씬·테스트·ProjectSettings 변경
- preflight failure 내부 원장 삭제. 사용자에게는 실행 여부를 정확히 구분하되 추적성은 유지한다.

## correction cycle

- 현재: 1/2 — r0 literal 정적 대조에서 coordinator의 `상태-only` 문자열 누락 FAIL, correction r1에서 exact 용어 보정
- 첫 정적 blocker에서 문서 owner에게 1회 반환한다. 두 번째 실패 또는 도구·상태 계약 변경 필요 발견 시 R2/R3로 재분류하고 자동 수정 반복을 중지한다.

## 비용 기록 (5줄 이하)

- planned roles/checks: 조정1 → 문서/릴리즈1 → 독립 정적 QA1 → 총괄 read-only1
- actual roles/checks: 조정1, 문서/릴리즈1, 독립 정적 QA1, 총괄 read-only1
- expensive runs (Unity/MCP/build/full suite/matrix/capture): 전부 0
- corrections/waste (SUPERSEDED/no-result/discard): owner r0 literal FAIL→correction r1 PASS, QA metadata blocker는 상태-only 자체 동기화 후 재QA 0
- cost verdict: 주의 — correction 1회, 중앙 dashboard 완료 동기화

## 최종 게이트

- QA 판정: 본문 criterion PASS, metadata blocker 1건 기록·상태-only 해소
- 총괄 최종 판정: `내부 승인 가능 — 상태-only 해소 후 완료 가능`
