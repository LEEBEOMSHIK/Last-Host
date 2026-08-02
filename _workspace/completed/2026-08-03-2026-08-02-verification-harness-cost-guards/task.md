# 검증 하네스 비용·재시도 차단 보완

## 기본 정보

- 작업 ID: `2026-08-02-verification-harness-cost-guards`
- 상태: 후속 R3 `2026-08-02-verification-current-state-contract`로 SUPERSEDED
- 위험 등급: R2 — 공용 검증 실행 경로·작업 템플릿·운영 게이트 변경
- 사용자 요청: 회피 가능했던 MCP 무결과, 허용되지 않은 Reflection, stale 테스트 계약, 전체 이력 위임, 하네스 반복 실패와 stale 상태 재교정을 실제 제한과 preflight로 차단한다.
- correction cycle: `2/2`

## 최소 역할

| 역할 | 수 | 책임 |
| --- | ---: | --- |
| 조정자 | 1 | 범위·negative control·인계·최종 통합 |
| 하네스 구현자 | 1 | 스크립트·설정·템플릿·운영 문서 구현 |
| 독립 QA | 1 | 정적·negative control·상태 lint 검증 |
| 총괄 | 1 | read-only 최종 감사 |

- 서브에이전트 전달 방식: `fork_turns: none`, 이 작업 패킷과 3개 이하 필수 파일만 전달한다.
- 전체 대화 이력 전달: 금지. 예외는 사용자 승인과 비용 사유 기록이 있을 때만 허용한다.

## 완료 주장

지원되지 않거나 이미 실패한 검증 경로, 위험한 임시 QA 코드, stale 테스트·상태 문서, correction 상한 초과, 반복 콜드 임포트와 과도한 에이전트 컨텍스트가 고비용 실행 전에 자동 차단된다.

## criterion

| ID | 유형 | 입력 | 기대값 | 최소 검증 |
| --- | --- | --- | --- | --- |
| G1 | capability | `McpTestRunner` 또는 알려진 실패 route 요청 | Unity/MCP 실행 전 nonzero 차단, fallback 제시 | negative control |
| G2 | harness safety | `System.Reflection`, private reflection, sync 없는 Rigidbody→Y-sort | 실행 전 nonzero 차단 | negative control |
| G3 | impact scan | collider/resolver 계약 변경 + 테스트의 과거 타입 기대 | 전체 회귀 전 stale contract 목록과 nonzero | fixture 샘플 |
| G4 | retry budget | 같은 criterion의 실패가 2회 누적 | 3번째 실행 전 재분류 요구·nonzero | attempt ledger 샘플 |
| G5 | cache reuse | 같은 work ID로 격리 프로젝트 준비 2회 | Library 보존·소스 3폴더 증분 동기화·cleanup 안전성 | 임시 dummy project |
| G6 | agent context | full-history/필수 파일 3개 초과/brief 과대 | 위임 전 nonzero, packet-only 통과 | brief lint |
| G7 | state consistency | current block의 fingerprint/run/status/cost 불일치 | 총괄 전 nonzero | stale/current 샘플 |
| G8 | low-level bypass | 공용 wrapper 없이 고비용 Unity 실행 | 운영 규칙·문서·wrapper token으로 차단 | 정적/negative control |

## 구현 범위

- `tools/verification/` capability profile, preflight/wrapper, attempt ledger, isolated cache reuse, state/brief lint, negative-control self-test
- `tools/verification/README.md`
- `docs/agents/loop-engineering-gates.md`, `docs/agents/loop-engineering-user-guide.md`
- `AGENTS.md`, `docs/agents/agent-reference-map.md`
- `_workspace/templates/task.md`, `handoff.md`, `agent-activity.md`, `verification.md`
- `.codex/skills/unity-verification-runner/references/verification-rules.md`

## 금지 범위

- Unity production 코드·씬·테스트·ProjectSettings·패키지 변경
- 실제 Unity, MCP Play, 빌드 실행
- 새 Codex skill/agent role 생성
- 정확 토큰·금액 추정
- full-history 서브에이전트 fork
- 커밋·푸시

## 실행 예산

| 항목 | 계획 | 실제 |
| --- | --- | --- |
| 역할 | 구현자 1 → QA 1 → 총괄 1 | 조정자 1 시작 |
| 동적 검증 | PowerShell dummy negative control 1묶음 | correction 2/2 bundle 1회 PASS |
| Unity/MCP/빌드 | 0 | 0 |
| correction | 최대 2, 실패마다 원인·변경계획 기록 전 재시도 금지 | 2/2 — QA actual-ledger blocker 교정 bundle 1회 PASS, 추가 correction 0 |

## 완료 기준

- G1~G8 PASS
- 알려진 실패 route와 위험 QA harness가 실제 실행 전에 차단됨
- 동일 work ID cache 재사용과 안전 cleanup negative control PASS
- current-state stale와 과도한 agent brief가 자동 차단됨
- 독립 QA·총괄 내부 승인 가능
