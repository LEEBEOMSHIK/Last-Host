# 검증 기록

## 검증 항목

- `UnityProject/ProjectSettings/ProjectVersion.txt` 존재 확인
- `UnityProject/Packages/manifest.json` 존재 확인
- `UnityProject/.git` 부재 확인
- 루트 Git에서 `UnityProject/` 파일 추적 가능 여부 확인
- `My project/`가 루트 `.gitignore`로 무시되는지 확인
- `AGENTS.md` 200줄 미만 유지 확인
- `git diff --cached --check`

## 결과

- `UnityProject/ProjectSettings/ProjectVersion.txt`: 존재 확인
- `UnityProject/Packages/manifest.json`: 존재 확인
- `UnityProject/.git`: 없음
- `My project/`: 없음
- `AGENTS.md`: 103줄로 200줄 미만 유지
- `git check-ignore -v "My project\Library" ".idea"`: 루트 `.gitignore` 규칙 적용 확인
- `git status --short`: `UnityProject/` 파일이 루트 저장소의 스테이징 항목으로 표시됨
- `git diff --cached --name-only | Select-String -Pattern '^My project/'`: 결과 없음
- `git diff --cached --check`: 종료 코드 0

## 남은 항목

- Unity MCP 패키지는 아직 설치하지 않았다.
