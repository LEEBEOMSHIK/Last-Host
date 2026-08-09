# R1 국소 수정 요약 배정서

## 기본 정보

- 작업 ID: `2026-08-06-unity-mcp-relay-resetter`
- 작업명: Unity MCP client relay 일괄 정리 전역 스킬
- 상태: R2로 재분류 후 완료
- 생성일: 2026-08-06
- 위험 등급: R1

## 원증상과 완료 주장

- 사용자 원증상·재현: Unity의 `Remove All Connections` 뒤에도 살아 있는 Codex client relay가 재접속해 승인 팝업과 연결 한도 문제가 반복된다.
- 완료 주장 한 문장: Codex가 실행한 `relay_win.exe --mcp`만 안전하게 식별·정리하고 재생성 여부를 보고하는 전역 스킬을 제공한다.

## 변경 파일과 단일 owner

| 변경 파일 | production owner |
| --- | --- |
| 전역 `unity-mcp-relay-resetter/SKILL.md` | 메인 조정자 — 사용자 명시 승인 예외 |
| 전역 `unity-mcp-relay-resetter/agents/openai.yaml` | 메인 조정자 — 사용자 명시 승인 예외 |
| 전역 `unity-mcp-relay-resetter/scripts/Stop-UnityMcpRelays.ps1` | 메인 조정자 — 사용자 명시 승인 예외 |

## 표적 테스트

- 구현자 표적 테스트: skill quick validation, PowerShell parser, `-WhatIf` 대상·제외 대상 확인
- 독립 QA 표적 재검증: 실제 프로세스 목록을 대상으로 target predicate와 안전 불변식 검토

## 금지 범위

- `Unity.exe`, `codex.exe`, Unity Editor 소유 `relay_win.exe --relay`를 종료하지 않는다.
- 자동 승인, Unity 설정 변경, 연결 이력 파일 삭제를 하지 않는다.
- 실제 relay 종료 검증은 별도 명시 실행 요청 없이 수행하지 않는다.

## correction cycle

- 현재: 2/2 — correction 1의 인스턴스 키·Process 객체 종료에 이어 부모 PID 재사용 방지를 위한 `parent start <= relay start` 조건 추가
- 2회 실패 또는 Codex/Unity 본체 종료 필요 발견 시: R2/R3 재분류

## 비용 기록 (5줄 이하)

- planned roles/checks: 메인 구현 1, 독립 QA 1, 총괄 판정 1
- actual roles/checks: 메인 구현 1, 독립 QA 최초·correction 1·correction 2, 총괄 1; parser·Inspect·WhatIf·설치 해시 대조
- expensive runs (Unity/MCP/build/full suite/matrix/capture): 0 예정
- corrections/waste (SUPERSEDED/no-result/discard): QA 이전 후보 2개 superseded, official generator/validator PyYAML blocker
- cost verdict: 과다 — correction 2/2 뒤 R2 재분류; 안전 보강 필요 비용이며 Unity/MCP/build·실제 종료 0

## 최종 게이트

- QA 판정: PASS
- 총괄 최종 판정: 내부 승인 가능
