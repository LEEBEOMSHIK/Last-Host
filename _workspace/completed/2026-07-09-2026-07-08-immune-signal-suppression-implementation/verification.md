# 검증 기록: 면역 신호 억제 미니게임 구현

## 요약

- 결과: 통과
- 통과: C# 런타임 컴파일, 테스트 어셈블리 컴파일, 핵심 수용 기준 스모크 실행, 공백 검사, Unity Test Runner EditMode, Unity MCP Play/Console 체크
- 미완료: Windows 빌드

## RED

- 방법: 기존 `UnityProject/Library/ScriptAssemblies/LastHost.Prototype.dll`에 새 API가 없는지 확인했다.
- 결과: 예상 실패
- 확인 내용:
  - `ImmuneSignalSuppressionModel` 없음
  - `InternalVirusMinigameType` 없음
  - `EnterVirusMinigame(InternalVirusMinigameType)` 없음

## GREEN / 회귀

### 런타임 컴파일

- 명령: Unity Roslyn `csc.dll`로 `Assets/_Project/Scripts/**/*.cs` 수동 컴파일
- 산출물:
  - `artifacts/LastHost.Prototype.CompileCheck.dll`
  - `artifacts/LastHost.Prototype.CompileCheck.log`
- 결과: 통과

### 테스트 어셈블리 컴파일

- 명령: Unity Roslyn `csc.dll`로 `Assets/_Project/Tests/**/*.cs` 수동 컴파일
- 산출물:
  - `artifacts/LastHost.Prototype.Tests.CompileCheck.dll`
  - `artifacts/LastHost.Prototype.Tests.CompileCheck.log`
- 결과: 통과

### 핵심 수용 기준 스모크

- 명령: `artifacts/run-immune-signal-smoke.ps1`
- 결과: `IMMUNE_SIGNAL_SUPPRESSION_REFLECTION_SMOKE_PASS`
- 확인 내용:
  - 정확 입력 누적 시 성공
  - 빠름/늦음 입력은 안정도를 감소시키고 실패 가능
  - 누락으로 목표 달성이 불가능하면 실패
  - 기본 내부 미니게임은 백혈구 회피 유지
  - HUD 진행 텍스트에서 `다음 신호`, `지금 차단` 타이밍 단서 노출
  - 신호 억제 성공 시 변이 선택으로 전환
  - 신호 억제 실패 후 복귀 시 변이 보상 없음

### 공백 검사

- 명령: `git diff --check`
- 결과: 통과
- 비고: 줄바꿈 CRLF 변환 경고만 출력됨

## MCP / 에디터 제한

- 이전 상태: Unity MCP가 `Connection revoked. Go to Unity Editor > Project Settings > AI > Unity MCP to change approval.` 오류로 차단됐다.
- 2026-07-09 01:19 KST 복구 후 `Unity_ManageEditor GetState`가 정상 응답했고, 열린 Unity 에디터에서 검증을 재개했다.

## MCP 복구 후 추가 검증

### Unity Test Runner EditMode

- 실행: `Unity_RunCommand: Codex EditMode Test Runner Retry`
- 대상: `LastHost.Prototype.Tests`
- 결과: 64개 통과, 실패 0, 스킵 0, 소요 3.6566505초
- 결과 파일: `UnityProject/Temp/CodexMcpValidation/editmode-summary.txt`

### Unity MCP Play 체크

- 실행:
  - `Unity_ManageEditor Play`
  - `Unity_RunCommand: Codex MCP Play Loop Check`
  - `Unity_ManageEditor Stop`
- 결과:
  - `RatHostPrototype` 씬 Play 진입/종료 통과
  - 면역 신호 억제 미니게임 진입 시 HUD 루트 활성화 확인
  - 신호 마커가 Tick 후 이동하는지 확인
  - 정확 입력 8회 누적 후 변이 선택 전환 확인
  - 변이 선택 후 쥐 숙주 복귀 및 면역 경계도 `25` 확인
  - Play 종료 후 씬 `isDirty=false`
  - Unity Console Error/Warning 0건

## 제외 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` define 변경은 작업 시작 전부터 있던 별도 변경으로 보고 이번 구현 산출물에서 제외한다.

## 남은 확인

- Windows 빌드는 아직 실행하지 않았다.
- 사운드 싱크, 노트 프리팹, 이펙트는 후속 확장 후보이며 이번 검증 범위 밖이다.
