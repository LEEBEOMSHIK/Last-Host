# 작업 인계

## 최종 상태

- 완료 보관 가능. 운영 문서 3개 계약 반영, 독립 정적 QA 1회, 총괄 read-only 1회 완료.
- QA metadata blocker는 correction cycle `1/2`로 상태-only 동기화했고 재QA하지 않았다.
- Unity/MCP/TestRunner/build/full suite/matrix/capture는 전부 0이다.

## 목표

안전 게이트는 유지하면서 준비 차단·구현 실패·독립 QA·상태-only 동기화가 사용자에게 반복 QA/run으로 보이지 않게 운영 규칙을 명확히 한다.

## 현재 상태

- 문서/릴리즈 owner가 허용된 production 3개에 공통 실행·보고 계약 반영을 완료했다.
- 첫 patch의 gates/user-guide 부분 적용은 조정자 감사로 동일 owner의 의도한 변경임을 확인했고, 잔여 coordinator 절차만 이어서 적용했다.
- 자체 `git diff --check`·3문서 계약·링크 정적 대조 PASS, Unity/MCP/TestRunner/build 0이다.
- current 문서 후보의 독립 정적 QA 1회와 총괄 read-only 1회만 남았다.

## 변경 결과

- preflight 차단과 S0 계약 검토는 실제 고비용 run·사용자-facing run 번호에서 분리하고 내부 추적은 보존한다.
- 구현과 독립 QA의 고비용 진입은 각각 최초1+보정1로 제한하며 두 번째 실패 뒤 재분류·사용자 보고 전 새 후보를 금지한다.
- QA·총괄 PASS 뒤 순수 board/cost/CURRENT/completed path·status sync는 추가 QA·총괄 없이 조정자 자체 대조로 닫는다.
- 운영 규칙·acceptance contract·production·테스트/하네스 변경은 상태-only 예외가 아니다.
- 사용자 보고는 최초 blocker, 재분류·결정 필요, 기술 PASS·최종 결과 중심으로 압축한다.

## 변경 허용

1. `docs/agents/loop-engineering-gates.md`
2. `docs/agents/loop-engineering-user-guide.md`
3. `.agents/project-coordinator-agent.md`

## 반드시 포함할 계약

- preflight 차단은 내부 원장에 보존하지만 실제 Unity/MCP/build run으로 표현하거나 사용자-facing run 번호를 증가시키지 않는다.
- 같은 원인 분류의 구현 고비용 표적 실행은 최초 1회+correction 1회까지만 자동 허용한다.
- 두 번째 실패 뒤에는 재분류·사용자 보고 전 새 고비용 후보를 시작하지 않는다.
- 독립 QA는 구현자 green인 동결 후보에서 1회 진입한다. QA 재진입도 correction 1회 뒤 1회가 상한이며 두 번째 QA 실패 시 중지한다.
- QA/총괄 판정 뒤 board·cost·CURRENT·completed 경로만 바꾸는 상태-only 동기화는 새 QA/총괄 라운드를 만들지 않고 조정자 정적 대조로 끝낸다.
- 실행 기준·acceptance contract·production·테스트를 바꾸는 변경은 상태-only가 아니며 기존 QA/총괄 게이트를 유지한다.
- 사용자 진행 보고는 최초 blocker, 재분류/결정 필요, 기술 통과/최종 결과 중심으로 압축한다.

## 금지

- wrapper/runner와 게임 production 변경
- 독립 QA·총괄 게이트 약화
- 실패 증거 삭제 또는 실패 통과
- Unity/MCP/build 실행
