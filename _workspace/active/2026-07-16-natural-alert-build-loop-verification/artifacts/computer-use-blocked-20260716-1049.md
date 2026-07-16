# Computer Use 연결 차단 요약

- 시각: 2026-07-16 10:49 KST
- 대상: `Builds/RatHostPrototype/LastHostPrototype.exe`
- 첫 연결: bundled client bootstrap 후 `list_apps` 실패
- 오류: `Computer Use native pipe is unavailable: Error: failed to connect native pipe: 지정된 파일을 찾을 수 없습니다. (os error 2)`
- 허용 복구: JavaScript 세션 초기화, bundled client 재-bootstrap, `list_apps` 1회 재시도
- 재시도 결과: 동일 오류
- 빌드 실행: 안 함
- 실제 키 입력: 안 함
- F6/상태 주입/PowerShell UI 우회: 사용하지 않음
- 증거 화면: helper가 snapshot 단계에 도달하지 못해 없음
- QA 결과: `차단`
