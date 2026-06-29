# 작업 로그

## 2026-06-29

- 루트 저장소와 `My project/` 내부 저장소가 별도 Git 저장소임을 확인했다.
- 내부 저장소 원격이 `https://github.com/LEEBEOMSHIK/My-project.git`임을 확인했다.
- 내부 저장소 이력을 `artifacts/my-project-inner-repo-before-merge.bundle`로 보관했다.
- `My project/.git`을 제거해 중첩 Git 저장소 상태를 해제했다.
- Unity가 일부 캐시 파일을 잠그고 있어 전체 폴더 이동은 실패했다.
- Git에 필요한 Unity 소스 파일을 `UnityProject/`로 복사했다.
- 기존 `My project/` 잔여 폴더를 루트 `.gitignore`에서 무시하도록 했다.
- Unity 내부 프로젝트명을 `Last Host`로 수정했다.
- 프로젝트 문서와 MCP 문서를 `UnityProject/` 기준으로 갱신했다.
- Unity 종료 후 기존 `My project/` 잔여 폴더를 삭제했다.
- Unity용 Git 속성 규칙은 루트 `.gitattributes`로 옮겨 `UnityProject/` 경로에 적용되도록 정리했다.
