# 검증 기록: 백혈구 회피 대응 경험 스케일링

## 검증 대상

반복 내부 대응 경험이 기존 백혈구 회피 미니게임의 백혈구 속도 배율에만 소폭 반영되는지 확인한다.

## 실행한 검증

```text
명령:
git diff --check

결과:
종료 코드 0. Unity C# 파일, 문서, `_workspace` 파일의 LF/CRLF 변환 경고만 출력됨.

해석:
공백 오류는 관찰되지 않았다.
```

```text
명령:
Unity MCP `Unity_ManageEditor GetState`

결과:
IsPlaying=false, IsCompiling=false, IsUpdating=false.

해석:
에디터는 테스트 실행 가능한 상태였다.
```

```text
명령:
Unity MCP `Unity_RunCommand: Codex EditMode Test Runner WhiteBloodCellScaling`

결과:
`LastHost.Prototype.Tests` EditMode 69개 통과.
FailCount=0
SkipCount=0

결과 파일:
UnityProject/Temp/CodexMcpValidation/white-blood-cell-scaling-editmode-summary.txt

해석:
신규 백혈구 회피 경험 스케일링 테스트 5개와 기존 회귀 테스트가 통과했다.
```

```text
명령:
Unity MCP Play 체크

실행:
- Unity_ManageEditor Play
- Unity_RunCommand: Codex MCP White Blood Cell Scaling Play Check Retry
- Unity_ManageEditor Stop

결과:
- 첫 백혈구 회피 진입: 경험 1, 속도 배율 1.0, 기존 속도 유지
- 두 번째 백혈구 회피 진입: 경험 2, 속도 배율 1.1, 백혈구 `CurrentSpeed`에 반영
- 실패 후 다음 진입: 경험 4, 속도 배율 1.3, 이전보다 강화
- 속도 배율은 최대값 이하 유지
- 성공 루프, 실패 루프, 실패 후 쥐 숙주 복귀 유지

해석:
실제 `RatHostPrototype` 씬의 세션과 백혈구 오브젝트에 배율 적용이 연결되어 있다.
```

```text
명령:
Unity MCP `Unity_ReadConsole` Error/Warning

결과:
0건.

해석:
Play 체크 후 Unity Console Error/Warning은 관찰되지 않았다.
```

```text
명령:
Unity MCP `Unity_ManageScene GetActive`

결과:
`RatHostPrototype` 씬 `isDirty=false`.

해석:
검증 과정에서 씬 저장 변경은 발생하지 않았다.
```

## 결과

- 통과.

## 검증하지 못한 항목

- Windows 빌드는 실행하지 않았다.
- 사용자가 직접 조작하며 체감한 난이도 변화는 아직 확인하지 않았다.

## 남은 위험

- 현재 배율은 작은 수치 조정이며 실제 체감이 약하거나 강할 수 있다. 필요하면 `WhiteBloodCellSpeedMultiplierPerExperience`와 `MaxWhiteBloodCellSpeedMultiplier`를 조정한다.

## 완료 판단

- 코드, 테스트, MCP Play/Console 기준 통과.
