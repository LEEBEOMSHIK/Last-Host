# 검증 기록

## 검증 대상

- 실행 기준, 사용자 가이드, 조정자 절차의 반복 제한·용어·상태-only 계약

## 계획

- 문서 owner 자체 정적 대조 1회
- 독립 QA 정적 대조 1회
- 총괄 read-only 감사 1회
- Unity/MCP/TestRunner/build/full suite/matrix/capture 0회

## 판정

- 문서 owner 자체 정적 대조 r0: `git diff --check` PASS, exact 3문서 matrix는 coordinator의 `상태-only` 문자열 누락으로 FAIL.
- correction r1: coordinator의 bullet을 `상태-only 최종 동기화`로 보정했다.
- r1 결과: `git diff --check` PASS, 세 production 문서의 preflight/S0 표현·구현/QA 상한·상태-only 예외·사용자 보고 전이가 일치한다.
- coordinator의 실행 기준 참조와 user-guide의 `loop-engineering-gates.md` 링크가 유효하다.
- 기존 fail-fast·독립 QA·총괄·추적성 게이트 삭제·약화 없음.
- Unity/MCP/TestRunner/build/full suite/matrix/capture: 0.
- 독립 QA 정적 대조와 총괄 read-only 판정: 대기.

## 독립 QA 1회 판정과 상태-only 해소

- 독립 QA 1회: `FAIL — 수정 요청`.
- 운영 규칙 본문 criterion 1~7, 변경 파일 3개 한정, 링크, `git diff --check`, Unity/MCP/build 0은 PASS했다.
- first blocker: `agent-activity.md` 상단 correction cycle `0/2`가 실제 이력 `1/2`와 불일치했다.
- 조정자 해소: 해당 상태-only 값을 `1/2`로 동기화했다. 운영 규칙·acceptance contract·production·테스트 변경은 없다.
- 사용자 승인에 따라 같은 정적 QA를 반복하지 않고, 총괄이 본문 QA 충분성과 상태-only 해소를 read-only로 감사한다.

## 2026-08-05 독립 정적 QA — 1회 판정

- 판정: **FAIL — 수정 요청**
- first blocker: `agent-activity.md`의 실행 기준이 correction cycle을 `0/2`로 기록하지만, 같은 파일의 상세 기록과 `task-r1-summary.md`·`verification.md`·`work-log.md`는 r0 literal FAIL 뒤 correction r1이 수행되어 현재 `1/2`임을 기록한다. 구현자 r0 FAIL→correction r1 PASS 이력과 현재 cycle 표기가 서로 불일치한다.
- 수정 요청: `agent-activity.md`의 correction cycle 현재값을 실제 이력과 일치시키고, 문서 owner 자체 정적 기록만 갱신한다. 이 FAIL 판정에 따른 QA 재실행은 하지 않는다.
- 나머지 정적 대조: 공통 실행·보고 계약의 preflight/run 분리, 구현·QA 상한, S0 표현, 상태-only 예외, 사용자 보고 압축, 기존 fail-fast·독립 QA·총괄·ledger·fingerprint·canonical·`SUPERSEDED`·lease·cost 보존은 일치했다.
- 범위·형식: 운영 문서 diff는 허용된 3개 파일로 한정되고 링크는 유효하며 `git diff --check`는 PASS했다.
- 실행 횟수: 독립 정적 QA 1회. Unity/MCP/TestRunner/build/full suite/matrix/capture 0회, production·테스트 수정 0회.
