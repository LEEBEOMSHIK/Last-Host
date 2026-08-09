# 검증 기록

검증 대상: `unity-mcp-relay-resetter` 전역 스킬

실행한 검증:
- PowerShell parser
- 필수 파일·UTF-8·frontmatter·TODO 잔존 여부 수동 정적 검사
- 실제 프로세스 Inspect
- `-Apply -WhatIf`
- 후보와 전역 설치본 SHA-256 대조
- 독립 QA correction 2 최종 재검토

결과:
- parser 오류 0
- Inspect: Codex client relay target 9, Unity Editor relay skipped/preserved 1
- WhatIf: 실제 종료 0, 실패 0, 모든 target에서 parent start ≤ relay start
- QA 최종 PASS
- 설치본 해시 일치 3/3
- `SKILL.md`: `43C09E9FD475A3CF75FF9BFB5A490D2C3303BEB351FA36BD69CBDA43FF13F60D`
- `agents/openai.yaml`: `3408DEE7CE90E4E70866ECC5D65BBAC23BE0EC7F53AF62DF187F24659AE75726`
- `scripts/Stop-UnityMcpRelays.ps1`: `A403D1595813D9C182D49CC742153D5C4429EFF60E350E93B85844AF24D8AD52`

검증 제한:
- 공식 `quick_validate.py`는 `ModuleNotFoundError: yaml`로 실행 불가. 새 패키지를 설치하지 않고 수동 정적 검사로 대체.
- 실제 relay 종료는 스킬 생성 승인과 별도인 파괴적 실행이므로 수행하지 않음.

완료 판단: 기술 검증 통과 — 실제 relay 종료는 별도 사용자 요청 필요
