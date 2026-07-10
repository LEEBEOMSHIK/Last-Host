# 검증 기록

## 작업 ID

2026-07-10-internal-response-cause-minigame-selection

## 검증 대상

면역 경계도 상승 원인에 따라 내부 바이러스 미니게임 타입이 선택되는지.

## 완료 주장

원인 라벨 기반 미니게임 선택 로직은 코드 단위 대체 실행 검증까지 통과했다.

Unity Editor 내부 MCP `RunCommand` 직접 검증은 relay 응답 문제로 완료 결과를 확보하지 못했으므로 완료로 주장하지 않는다.

## 실행한 검증

### 코드/공백 검증

- 명령: `git diff --check`
- 결과: 통과. LF/CRLF 경고만 확인.

### Unity 내장 Mono/Roslyn 임시 컴파일

- 대상: `LastHost.Prototype`
- 방식: `Library/Bee/.../LastHost.Prototype.rsp`를 `%TEMP%` 출력으로 치환해 Unity 내장 `mono.exe` + Roslyn `csc.exe` 실행
- 결과: 통과. Unity SourceGenerator analyzer 로딩 경고 `CS8032`가 있었으나 컴파일 종료 코드는 0.

- 대상: `LastHost.Prototype.Tests`
- 방식: 테스트 rsp의 제품 참조를 임시 제품 ref dll로 치환해 컴파일
- 결과: 통과. 동일한 analyzer 로딩 경고 `CS8032`가 있었으나 컴파일 종료 코드는 0.

### 임시 실행 하니스

- 대상: `%TEMP%\CauseSelectionHarness.exe`
- 검증 케이스:
  - 직접 `EnterVirusMinigame()` 기본값은 `WhiteBloodCellEvasion`
  - `강제 조종` 임계치 도달은 `ImmuneSignalSuppression`
  - `소음/조직 자극` 위험 행동 임계치 도달은 `ImmuneSignalSuppression`
  - `오염 노출`은 기본 설정이 신호 억제여도 `WhiteBloodCellEvasion`
  - `면역 포착`은 기본 설정이 신호 억제여도 `WhiteBloodCellEvasion`
  - 미분류 원인은 설정된 기본 내부 미니게임 사용
- 결과: `CAUSE_SELECTION_HARNESS_OK 6/6`

## 검증하지 못한 항목

- Unity MCP `Unity_RunCommand` 직접 실행 검증:
  - relay trace에는 `Validate cause based internal minigame selection` 실행 시작 로그만 있고 완료 로그가 남지 않았다.
  - 이후 `Unity_ReadConsole Clear`, `Unity_ManageEditor GetState` 호출이 각각 300초 타임아웃됐다.
  - 로컬 포트 확인 결과 9001/9002 연결 실패.
- Unity Test Runner UI/Editor 내부 테스트 실행:
  - MCP 응답 제한으로 실행하지 못했다.
- 실제 플레이 체감:
  - 이 작업 범위 밖이며 `docs/project-handoff/manual-play-checklist.md` 보류 항목으로 유지한다.

## 완료 판단

QA/검증 에이전트 판정: 대체 실행 검증 기준 통과. Unity MCP 직접 검증은 제한 사항으로 남김.

프로젝트 총괄 관리자 판정: 승인된 쥐 숙주 프로토타입 범위 안의 작은 게임플레이 분기 변경이다. 새 미니게임, 새 UI, 새 입력, ProjectSettings 변경 없음. Unity MCP 직접 검증 미확보를 보고 조건으로 작업 완료 처리 가능.
