# 쥐 숙주 프로토타입 Play 검증

작업 ID: `2026-06-30-rat-host-play-verification`
검증일: 2026-06-30
대상 커밋: `9fd3264 fix: add event system fallback for prototype UI`

## 검증 대상

- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- `PrototypeSessionController` 기반 쥐 숙주 핵심 루프
- `PrototypeHud`의 EventSystem 보강 및 변이 선택 UI 입력 경로
- Unity MCP Play/Stop 제어

## 실행한 검증

### 1. 지침 및 기준 문서 확인

명령:
- `AGENTS.md`
- `.agents/qa-verification-agent.md`
- `.codex/skills/unity-verification-runner/SKILL.md`
- `.codex/skills/unity-verification-runner/references/verification-rules.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `_workspace/active/2026-06-30-rat-host-play-verification/task.md`

결과:
- 검증 기록은 실제 실행 결과와 미검증 항목을 분리해 남기기로 확인했다.

해석:
- 이번 검증은 구현 변경 없이 Play 실행, 상태 조회, 런타임 명령 기반 핵심 루프 확인만 수행했다.

### 2. Unity MCP 상태 확인

명령:
- `Unity_ManageEditor.GetState`

결과:
- `success=true`
- `IsPlaying=false`
- `IsCompiling=false`
- `IsUpdating=false`
- Unity 실행 경로: `C:/Program Files/Unity/Hub/Editor/6000.4.6f1/Editor/Unity.exe`

해석:
- Unity MCP 연결은 정상이며, Play 진입 전 컴파일/업데이트 대기 상태는 아니었다.

### 3. Play 전 콘솔 Error/Warning 수집

명령:
- `Unity_ReadConsole` with `Types=["Error","Warning"]`

결과:
- Error 1건 수집:
  - `DirectoryNotFoundException: Could not find a part of the path 'C:\Program Files\JetBrains\JetBrains Rider 2026.1.1\plugins\rider-unity\EditorPlugin'.`
  - 파일: `./Library/PackageCache/com.unity.ide.rider@4504839d03f7/Rider/Editor/RiderScriptEditor.cs`

해석:
- 스택이 `com.unity.ide.rider` 패키지 내부에 한정되어 있어 게임 코드 오류로 보이지 않는다.
- 콘솔은 완전 무오류 상태가 아니므로 "콘솔 클린"으로 판단하지 않는다.

### 4. 활성 씬 및 계층 확인

명령:
- `Unity_ManageScene.GetActive`
- `Unity_ManageScene.GetHierarchy`

결과:
- 활성 씬: `RatHostPrototype`
- 경로: `Assets/_Project/Scenes/RatHostPrototype.unity`
- `buildIndex=0`
- `isDirty=false`
- 루트 `RatHostPrototype` 아래 주요 오브젝트 확인:
  - `Core`
  - `RatHostMode`
  - `VirusMinigameMode`
  - `UI`
  - `Cameras`
  - `Lighting`
- Play 전 상태:
  - `RatHostMode` active
  - `VirusMinigameMode` inactive
  - `MutationSelectionPanel` inactive
  - `VirusFailurePanel` inactive

해석:
- 요청 대상 씬이 이미 활성 상태였고, 쥐 모드 시작 구성이 맞다.

### 5. Play 모드 진입

명령:
- `Unity_ManageEditor.Play` with `WaitForCompletion=true`
- `Unity_ManageEditor.GetState`

결과:
- `Entered play mode.`
- `IsPlaying=true`
- `IsCompiling=false`
- `IsUpdating=false`

해석:
- `RatHostPrototype.unity`는 Unity MCP로 Play 모드에 진입했다.

### 6. Play 중 콘솔 Error/Warning 확인

명령:
- `Unity_ReadConsole` with `Types=["Error","Warning"]`

결과:
- 총 3건 수집:
  - Rider 경로 Error 1건
  - `[MCP Unity] Failed to start WebSocket server: Port 8090 is already in use.` Warning 1건
  - 같은 MCP 8090 포트 Error 1건

해석:
- Rider 오류는 `com.unity.ide.rider` 패키지 경로 문제다.
- MCP 8090 포트 오류는 `com.gamelovers.mcp-unity` 패키지 내부에서 발생했다. 현재 Codex relay가 이미 포트를 사용 중인 상태와 관련된 것으로 보이며, 이후 MCP 명령은 계속 성공했다.
- Play 중 게임 코드 네임스페이스 또는 `Assets/_Project` 스크립트에서 발생한 Error/Warning은 관찰되지 않았다.

### 7. Play 중 런타임 상태 조회

명령:
- `Unity_RunCommand`: `Inspect Rat Host Play State`

결과:
- `Application.isPlaying=True`
- `ActiveScene=RatHostPrototype|Assets/_Project/Scenes/RatHostPrototype.unity|isLoaded=True`
- `PrototypeSessionController.exists=True`
- `Session.CurrentMode=RatHost`
- `State.HostHealth=100`
- `State.ImmuneAlert=0.1`
- `State.VirusStability=100`
- `State.MutationFragments=0/3`
- `RatHostMode activeInHierarchy=True`
- `VirusMinigameMode activeInHierarchy=False`
- `RatHostController.exists=True`
- `RatHostController.enabled=True`
- `Keyboard.current=True`
- `PrototypeHud.exists=True`
- `PrototypeHud modeText=쥐 숙주`
- `PrototypeHud objectiveText=하수도 탐색 중`
- `EventSystem.exists=True`
- `EventSystem.name=EventSystem`
- `hasInputSystemUIInputModule=True`
- `MutationOptionButton.count=3`
- 세 변이 버튼 확인:
  - `Dormancy`: 잠복 강화 / 면역 경계도 상승 속도 감소
  - `NeuralControl`: 신경 조종 / 쥐 이동 속도 증가
  - `MammalAdaptation`: 포유류 적응 / 하수도 특정 통로 접근 허용

해석:
- `PrototypeSessionController`, HUD, 쥐 모드 루트, 키보드 입력 장치, EventSystem 보강 상태는 Play 중 확인됐다.
- `PrototypeHud.EnsureEventSystem()`의 fallback 결과로 볼 수 있는 `EventSystem + InputSystemUIInputModule`이 존재했다.

### 8. 명령 기반 핵심 루프 전환 확인

명령:
- `Unity_RunCommand`: 면역 경계도 직접 상승, 변이 조각 3회 처리, 변이 선택 상태 확인

결과:
- `AddImmuneAlertAmount(1000f)` 후:
  - `mode=InternalVirus`
  - `RatHostMode=False`
  - `VirusMinigameMode=True`
- `ResolveVirusFrame(true, false)` 3회 후:
  - `mode=MutationSelection`
  - `fragments=3/3`
  - `MutationSelectionPanel=True`

해석:
- 면역 경계도 100% 도달 후 내부 바이러스 모드로 전환되는 경로와, 변이 조각 3개 수집 후 변이 선택 모드로 전환되는 경로는 런타임 명령으로 확인됐다.

### 9. `1/2/3` 키 선택 경로 시도

명령:
- `Unity_RunCommand`: `KeyboardState(Key.Digit1/2/3)` 주입 후 `PrototypeSessionController.Update` 경로 호출 시도

결과:
- `Digit1`, `Digit2`, `Digit3` 모두 선택으로 이어지지 않았다.
- 예시 결과:
  - `AfterKey key=Digit1 expected=Dormancy, mode=MutationSelection, hasExpected=False`
  - `AfterKey key=Digit2 expected=NeuralControl, mode=MutationSelection, hasExpected=False`
  - `AfterKey key=Digit3 expected=MammalAdaptation, mode=MutationSelection, hasExpected=False`

해석:
- 현재 MCP 명령 기반 키 상태 주입 방식으로는 `1/2/3` 실제 키 경로를 검증하지 못했다.
- 소스상 `PrototypeSessionController.Update()`에는 `Keyboard.current.digit1Key/digit2Key/digit3Key.wasPressedThisFrame` 처리 경로가 존재하지만, 이번 검증에서는 실제 키 입력으로 동작했다고 판정하지 않는다.

### 10. EventSystem UI Submit 경로로 변이 선택 및 복귀 확인

명령:
- `Unity_RunCommand`: `EventSystem`으로 `포유류 적응Button`에 Submit 이벤트 실행

결과:
- Submit 전:
  - `mode=MutationSelection`
  - `targetButton=포유류 적응Button`
  - `panelActive=True`
- Submit 후:
  - `mode=RatHost`
  - `mammalMutation=True`
  - `mammalPassage=True`
  - `immuneAlert=25`
  - `RatHostMode=True`
  - `VirusMinigameMode=False`
  - `MutationPanel=False`
  - `MammalAdaptationGate colliderEnabled=False`
  - `rendererMaterial=GateOpen`

해석:
- EventSystem 기반 UI Submit 경로로 변이 선택이 동작했다.
- 변이 선택 후 쥐 숙주 모드로 복귀했고, 포유류 적응 효과가 게이트 개방으로 반영됐다.

### 11. Play 모드 종료 및 종료 후 상태 확인

명령:
- `Unity_ManageEditor.Stop` with `WaitForCompletion=true`
- `Unity_ManageEditor.GetState`
- `Unity_ManageScene.GetActive`
- `Unity_ReadConsole` with `Types=["Error","Warning"]`

결과:
- `Exited play mode.`
- `IsPlaying=false`
- `IsCompiling=false`
- `IsUpdating=false`
- 활성 씬:
  - `RatHostPrototype`
  - `Assets/_Project/Scenes/RatHostPrototype.unity`
  - `isDirty=false`
- 종료 후 콘솔 Error/Warning은 Play 중 확인한 3건과 동일했다.

해석:
- Play 모드는 정상 종료됐고, 씬 저장 변경은 발생하지 않았다.

## 결과

- Unity MCP 연결: 통과
- `RatHostPrototype.unity` 활성 씬 확인: 통과
- Play 모드 진입: 통과
- Play 모드 종료: 통과
- `PrototypeSessionController` 존재 및 기본 상태: 통과
- 쥐 모드 루트 활성: 통과
- `PrototypeHud` 존재 및 HUD 텍스트 갱신: 통과
- EventSystem fallback 확인: 통과
- 면역 경계도 100% 기반 내부 바이러스 모드 전환: 명령 기반 통과
- 변이 조각 3개 성공 후 변이 선택 패널 표시: 명령 기반 통과
- EventSystem UI Submit으로 변이 선택 후 쥐 모드 복귀: 통과
- 포유류 적응 효과로 게이트 개방: 통과
- 게임 코드 콘솔 오류: 관찰되지 않음
- 전체 콘솔 무오류: 실패. Rider 경로 Error와 MCP 8090 포트 Error/Warning이 남아 있음
- `1/2/3` 실제 키 선택 경로: 미검증. MCP 키 주입 시도는 선택을 발생시키지 못함

## 검증하지 못한 항목

- 실제 사용자가 Game View에서 `WASD`로 쥐를 움직이는 수동 조작감
- 실제 키보드 `1/2/3` 입력으로 변이가 선택되는지
- 실제 충돌 기반 변이 조각 픽업과 백혈구 회피 플레이
- 백혈구 충돌 실패 후 `Space` 재시도 루프
- Windows PC 빌드
- 해상도별 UI/카메라 가림 여부
- 자동 EditMode/PlayMode 테스트 실행

## 남은 위험

- 콘솔에 남은 Rider 경로 Error는 게임 코드와 무관해 보이나, 개발 환경 설정 문제로 계속 노출될 수 있다.
- MCP 8090 포트 Error/Warning은 현재 유지해야 하는 relay가 이미 포트를 점유한 상태에서 MCP Unity 서버가 재시작을 시도하며 발생한 것으로 보인다. 이번 검증 중 MCP 명령은 계속 성공했지만, 콘솔 무오류 기준에는 걸린다.
- 명령 기반 상태 전환은 실제 플레이 입력/물리 충돌을 완전히 대체하지 못한다.
- `1/2/3` 선택 경로는 소스상 존재만 확인됐고, 이번 MCP 키 주입 검증에서는 동작을 증명하지 못했다.

## 완료 판단

부분 통과.

`RatHostPrototype.unity`는 Unity MCP로 Play 모드에 들어가고 종료할 수 있었다. Play 중 `PrototypeSessionController`, `PrototypeHud`, 쥐 모드 루트, EventSystem 보강, 명령 기반 핵심 상태 전환, EventSystem UI Submit 기반 변이 선택 후 쥐 모드 복귀는 확인됐다.

다만 콘솔은 무오류가 아니며, 실제 `WASD` 이동과 실제 `1/2/3` 키 변이 선택은 이번 검증에서 통과로 판정하지 않는다. 플레이어블 완료 판단을 위해서는 Game View 수동 입력 또는 신뢰 가능한 입력 자동화가 추가로 필요하다.

## 변경/작성 파일

- `_workspace/active/2026-06-30-rat-host-play-verification/verification.md`
