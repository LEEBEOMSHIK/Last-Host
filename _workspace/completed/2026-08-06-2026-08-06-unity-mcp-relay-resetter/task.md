# 작업 배정서

## 기본 정보

- 작업 ID: `2026-08-06-unity-mcp-relay-resetter`
- 작업명: Unity MCP client relay 일괄 정리 전역 스킬
- 상태: 완료
- 생성일: 2026-08-06
- 담당 에이전트: 메인 조정자
- 보조 에이전트: 독립 QA, 프로젝트 총괄 관리자
- 사용 스킬: `skill-creator`, `last-host-design-keeper`

## 위험 분류와 재분류

- 최초 분류: R1 — 전역 스킬 3파일, Unity/프로젝트 production 비변경
- 최종 분류: R2 — 독립 QA 안전 blocker가 2회 발생해 correction 2/2에서 재분류
- Unity/MCP/빌드 고비용 실행: 0
- 실제 프로세스 종료: 0

## S0 사용자 원증상·검증 charter

- 사용자 원증상: Unity `Remove All Connections` 뒤에도 살아 있는 Codex relay가 재접속해 승인 팝업이 반복된다.
- 완료 주장: Codex-owned `relay_win.exe --mcp`만 식별·정리하고 Unity/Codex 본체와 Unity Editor relay를 보존하는 전역 스킬을 설치한다.

| criterion ID | 유형 | 입력·상태 | 기대값 | 최소 검증 |
| --- | --- | --- | --- | --- |
| C1 | 성공 | Inspect | Codex client relay만 target | 실제 프로세스 조회 |
| C2 | negative control | Unity Editor `--relay` | skipped/preserved | Inspect/WhatIf |
| C3 | 실패 | `codex.exe`/`Unity.exe` | 종료 코드 경로 없음 | 정적 QA |
| C4 | 경계 | relay/parent PID 재사용 | 시작 시각 인스턴스 검증 | 정적 QA |
| C5 | 경계 | 같은 PID relay respawn | InstanceKey로 새 인스턴스 판정 | 정적 QA |
| C6 | 안전 | 생성 작업 | 실제 종료 0 | `-Apply -WhatIf` |

## 산출물

- 전역 설치: `C:\Users\bumci\.codex\skills\unity-mcp-relay-resetter`
- 후보 보관: `artifacts/unity-mcp-relay-resetter/`
- 파일: `SKILL.md`, `agents/openai.yaml`, `scripts/Stop-UnityMcpRelays.ps1`

## 금지 범위

- `codex.exe`, `Unity.exe`, Unity Hub, Unity Editor `relay_win.exe --relay` 종료
- 자동 승인, Unity/Codex 설정 변경, 연결 이력 삭제
- 생성·검증 단계의 실제 relay 종료

## 비용과 게이트

- correction: 2/2 뒤 R2 재분류
- 독립 QA: 최종 PASS
- 프로젝트 총괄: 내부 승인 가능
- 공식 `quick_validate.py`: 로컬 Python `PyYAML` 부재로 실행 불가
- 대체 검증: UTF-8/frontmatter/필수 파일 수동 정적 검사, parser, Inspect, Apply-WhatIf, SHA-256 설치본 대조
- 비용 판정: 과다 — 안전 blocker 2회로 재분류·재QA가 필요했으나 Unity/MCP/빌드·실제 종료는 0
