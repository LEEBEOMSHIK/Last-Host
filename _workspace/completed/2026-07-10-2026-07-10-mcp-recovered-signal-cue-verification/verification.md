# 검증 기록

## 작업 ID

`2026-07-10-mcp-recovered-signal-cue-verification`

## 검증 대상

- `2026-07-10-signal-suppression-approach-cue` 작업의 남은 Unity MCP Play 검증
- 면역 신호 억제 F6 진입
- HUD 접근 cue/펄스 상태
- Unity Console Error/Warning

## 검증 담당

QA/검증 에이전트 역할

## 검증 에이전트 수행 이력

- 검증 에이전트: QA/검증 에이전트 역할
- 검증 요청자: 사용자
- 검증한 산출물: Unity MCP 연결 상태, 프로세스/relay 상태, Unity 로그
- `agent-activity.md` 반영 여부: 반영

## 입력 자료

- `_workspace/active/2026-07-10-signal-suppression-approach-cue/verification.md`
- `UnityProject/Logs/traces.jsonl`
- `UnityProject/Logs/relay.txt`
- Unity/Codex/relay 프로세스 목록

## 원래 증상 또는 완료 주장

이전 작업에서 Unity MCP가 `Stop` 호출 시 승인 해제되어 F6 진입/HUD 자동 검증이 닫히지 않았다. 이번 작업은 MCP 승인 복구 후 그 누락 검증을 완료하는 것이 목표다.

## 실행한 검증

```text
명령 또는 확인 방법:
Get-CimInstance Win32_Process -Filter "name = 'Unity.exe' or name = 'codex.exe' or name = 'Codex.exe' or name = 'node.exe' or name = 'relay_win.exe'"

결과:
Unity Editor PID 78948 실행 중.
현재 재개 세션: node.exe PID 68788, codex.exe resume PID 64420, relay_win.exe PID 91604.
Unity relay server: PID 61492, port 9001/9002.
이전 문제 후보였던 relay PID 77484는 현재 목록에 없음.

해석:
이전 stale CLI relay는 사라졌으나, 현재 재개 세션 자체가 새 MCP 클라이언트로 Unity 승인을 요구하고 있다.
```

```text
명령 또는 확인 방법:
Unity_GetUserGuidelines

결과:
Connection revoked. Go to Unity Editor > Project Settings > AI > Unity MCP to change approval.

해석:
현재 Codex 세션의 Unity MCP 호출은 Unity Editor에서 승인되지 않았다.
```

```text
명령 또는 확인 방법:
UnityProject/Logs/traces.jsonl 확인

결과:
현재 MCP 클라이언트가 codex.exe PID 64420, relay PID 91604로 기록됨.
Unity 로그의 connection decision은 Reason: Awaiting user approval.

해석:
현재 재개 세션은 Unity Editor의 MCP 승인 UI에서 승인해야 한다.
```

## 이전 차단 시 검증하지 못했던 항목

- Play 상태 조회
- F6 면역 신호 억제 진입
- HUD `신호 접근`/`지금 차단` 상태 확인
- 마커 색상/펄스 상태 확인
- Play 중 콘솔 Error/Warning 확인

## 이전 차단 시 실패 또는 경고

- 현재 Unity MCP 호출은 `Connection revoked` 상태다.
- Unity Editor의 `Project Settings > AI > Unity MCP`에서 현재 세션 승인이 필요하다.

## 재개 후 최종 검증

```text
명령 또는 확인 방법:
Unity_ManageEditor GetState
Unity_ReadConsole Clear
Unity_ManageEditor Play
Unity_RunCommand 초기 세션 상태 조회

결과:
Play 진입 성공.
초기 상태는 Mode=RatHost, InternalType=WhiteBloodCellEvasion, DebugHotkeys=True.
면역 신호 억제 HUD는 F6 전 비활성.

해석:
F6 전환 검증을 진행할 수 있는 정상 시작 상태다.
```

```text
명령 또는 확인 방법:
Unity Game View focus 후 Windows 네이티브 F6 keybd_event 입력
Unity_RunCommand 세션 상태 조회

결과:
네이티브 F6 입력 후 Mode=InternalVirus, InternalType=ImmuneSignalSuppression 진입 확인.
시간이 흐른 상태에서는 입력 없이 VirusFailed로 넘어가는 것도 확인.

해석:
실제 F6 디버그 핫키 경로가 면역 신호 억제 미니게임 진입으로 연결된다.
단, 일반 SendKeys 방식은 Unity에 잡히지 않아 검증 입력 방식으로는 부적합했다.
```

```text
명령 또는 확인 방법:
Time.timeScale=0에서 F6 직후 상태 캡처

결과:
Mode=InternalVirus
InternalType=ImmuneSignalSuppression
TimeUntilSignal=1
Timing='다음 신호 1.0초'
Progress='신호 차단 0/8 / 다음 신호 1.0초'
CueActive=False
CueIntensity=0
HudActive=True
MarkerScale=(1.00, 1.00, 1.00)
MarkerColor=RGBA(0.380, 0.780, 0.960, 1.000)

해석:
F6 직후 면역 신호 억제 HUD가 정상 활성화되고 대기 상태를 표시한다.
```

```text
명령 또는 확인 방법:
SignalSuppressionRun 수동 tick 0.7초 후 HUD Refresh

결과:
TimeUntilSignal=0.3
Timing='신호 접근 0.3초'
Progress='신호 차단 0/8 / 신호 접근 0.3초'
CueActive=True
CueIntensity=0.2173913
HudActive=True
MarkerScale=(1.04, 1.04, 1.04)
MarkerColor=RGBA(0.506, 0.780, 0.812, 1.000)

해석:
접근 예고 cue가 활성화되고 HUD 문구, 진행 문구, 마커 펄스/색상 변화가 반영된다.
```

```text
명령 또는 확인 방법:
SignalSuppressionRun 추가 tick 0.21초 후 HUD Refresh

결과:
TimeUntilSignal=0.09000002
Timing='지금 차단'
Progress='신호 차단 0/8 / 지금 차단'
CueActive=True
CueIntensity=1
HudActive=True
MarkerScale=(1.22, 1.22, 1.22)
MarkerColor=RGBA(0.450, 0.960, 0.580, 1.000)

해석:
정확 판정 구간에서 `지금 차단` 문구, 최대 cue intensity, 녹색 ready 마커가 정상 표시된다.
```

```text
명령 또는 확인 방법:
Unity_ReadConsole Error/Warning
Time.timeScale=1 복구
Unity_ManageEditor Stop
Unity_ManageEditor GetState
Unity_ManageScene GetActive

결과:
Play 중 및 종료 후 Unity Console Error/Warning 0건.
Play 종료 성공.
종료 후 IsPlaying=false, IsCompiling=false, IsUpdating=false.
활성 씬 RatHostPrototype, isDirty=false.

해석:
검증 과정에서 Unity 콘솔 오류/경고나 씬 dirty 변경은 발생하지 않았다.
```

```text
명령 또는 확인 방법:
git diff -- UnityProject/ProjectSettings/ProjectSettings.asset
git restore -- UnityProject/ProjectSettings/ProjectSettings.asset
git status --short --branch

결과:
Unity가 Standalone define에 APP_UI_EDITOR_ONLY를 다시 기록한 한 줄 변경을 확인했다.
사용자 요청에 따라 ProjectSettings.asset을 되돌렸고, 되돌림 후 ProjectSettings diff는 없음.

해석:
이번 검증 작업의 최종 변경에 ProjectSettings 변경은 포함하지 않는다.
```

## 최종 미검증 항목

- 사용자 직접 플레이 체감 확인은 이번 MCP 누락 검증 범위가 아니다.
- Windows 빌드 실행본 직접 플레이와 해상도별 UI/카메라 검증은 별도 `전체 플레이어블 검증 재정리` 후보로 남긴다.

## 최종 실패 또는 경고

- `SendKeys` 방식의 F6 입력은 Unity Game View에 잡히지 않았다. 검증은 네이티브 `keybd_event` 방식으로 완료했다.
- ProjectSettings `APP_UI_EDITOR_ONLY`는 Unity가 다시 기록했으나 사용자 요청에 따라 되돌렸다.

## 게이트 판정

- QA/검증 게이트 통과 여부: 통과
- `agent-activity.md`에 QA 판정 반영 여부: 반영
- 총괄 관리자 검토로 넘길 수 있는지: 예

## 완료 판단

완료

## 완료 판단 근거

현재 세션에서 Unity MCP Play 진입/종료, 실제 F6 진입, HUD 접근 cue/정확 판정 cue, 콘솔 Error/Warning 0건, 씬 dirty=false를 확인했다. 이번 작업은 코드/씬/ProjectSettings 변경 없이 누락 검증 공백을 닫는 범위이며, ProjectSettings 재기록분은 사용자 요청에 따라 되돌렸다.
