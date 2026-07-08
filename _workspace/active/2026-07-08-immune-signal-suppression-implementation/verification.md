# 검증 기록: 면역 신호 억제 미니게임 구현

## 요약

- 결과: 제한적 통과
- 통과: C# 런타임 컴파일, 테스트 어셈블리 컴파일, 핵심 수용 기준 스모크 실행, 공백 검사
- 미완료: Unity MCP Play 체크, Unity Test Runner 실행

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

- Unity MCP: `Connection revoked. Go to Unity Editor > Project Settings > AI > Unity MCP to change approval.`
- 열린 Unity 에디터의 `Library/ScriptAssemblies`는 아직 새 스크립트를 반영하지 않았다.
- 같은 프로젝트가 Unity 에디터에서 열려 있어 별도 배치모드 Test Runner는 프로젝트 잠금과 충돌할 수 있어 실행하지 않았다.

## 제외 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` define 변경은 작업 시작 전부터 있던 별도 변경으로 보고 이번 구현 산출물에서 제외한다.

## 남은 확인

- Unity Editor에서 MCP 승인을 다시 허용한 뒤 Play 체크를 수행해야 한다.
- Unity Test Runner EditMode 전체 실행은 에디터가 새 스크립트를 컴파일한 뒤 확인해야 한다.
