# 검증 기록

## 작업 ID

`2026-06-30-doc-purpose-folder-rule`

## 실행일

2026-06-30

## 실행한 검증

- `git diff --check`
- `(Get-Content -LiteralPath 'AGENTS.md').Count`
- `rg -n "성격별 폴더|문서 배치 규칙|docs/README.md" AGENTS.md docs\README.md docs\agents\agent-reference-map.md .agents\documentation-release-agent.md .codex\skills\last-host-design-keeper\SKILL.md`

## 결과

- `git diff --check`: exit 0. 줄바꿈 변환 경고만 있고 공백 오류 없음.
- `AGENTS.md` 줄 수: 113줄. 200줄 미만 유지.
- 성격별 폴더 규칙 참조 검색: `AGENTS.md`, `docs/README.md`, 문서/릴리즈 에이전트, 참조 색인, 디자인 키퍼 스킬에서 확인됨.

## 미실행 검증

- Unity 컴파일, 테스트, 플레이, 빌드는 실행하지 않았다. 이번 작업은 문서 운영 규칙 정리다.
