# 작업 로그

## 작업 ID

`2026-06-30-doc-purpose-folder-rule`

## 로그

### 2026-06-30

- 수행 내용: 사용자 요청을 문서 운영 규칙 보강 작업으로 분류했다.
- 판단: 문서 구조 운영 원칙만 보강하며 Unity, 코드, 에셋은 변경하지 않는다.

- 수행 내용: `AGENTS.md`, `docs/README.md`, `.agents/documentation-release-agent.md`, `docs/agents/agent-reference-map.md`, `.codex/skills/last-host-design-keeper/SKILL.md`를 갱신했다.
- 검증: `git diff --check`, `AGENTS.md` 줄 수, 성격별 폴더 규칙 참조 검색을 실행했다.

## 열린 질문

- 없음.

## 위험과 주의점

- AGENTS.md는 200줄 미만을 유지한다.
- 규칙이 너무 길어지면 `docs/README.md`와 작업별 참조 색인에 둔다.
