# 검증 기록

## 작업 ID

2026-07-01-rat-host-full-play-verification

## 검증 대상

쥐 숙주 프로토타입 전체 플레이 가능성.

## 검증 담당

Codex 메인 에이전트, QA/검증 에이전트

## 완료 주장

이번 작업에서 주장할 수 있는 것은 실제로 실행한 검증 결과에 한정한다.

## 실행한 검증

```text
명령:
Unity MCP `Unity_ManageEditor.GetState`

결과:
- `IsPlaying=false`
- `IsCompiling=false`
- `IsUpdating=false`
- Unity 버전 경로: `6000.4.6f1`

해석:
검증 시작 시 Unity Editor는 실행 가능한 상태였다.
```

```text
명령:
Unity MCP `Unity_ReadConsole Clear`

결과:
콘솔 초기화 성공.

해석:
이번 검증 중 발생한 콘솔 오류와 경고를 분리하기 위해 초기화했다.
```

```text
명령:
Unity MCP `Unity_RunCommand`로 `RatHostPrototypeCoreTests` 명시 호출

결과:
18/18 통과.

해석:
면역 경계도, 변이 효과, 바이러스 성공/실패, 키 매핑, 카메라 전환, 씬 기본 카메라 연결 테스트가 통과했다.

참고:
NUnit Attribute 검색 방식은 동적 명령 어셈블리의 NUnit 참조 제한으로 컴파일 실패했고, Reflection 방식은 MCP 보안 제한으로 실패했다. 두 실패는 테스트 실패가 아니라 실행 방식 제한이다.
```

```text
명령:
Unity MCP `Unity_ManageEditor.Play`

결과:
Play 모드 진입 성공.

해석:
`RatHostPrototype.unity`가 Play 모드에서 실행 가능했다.
```

```text
명령:
Unity MCP `Unity_RunCommand` Play 모드 전체 런타임 검증

결과:
통과.
- 시작 모드: `RatHost`
- `W` 입력 주입 후 쥐 이동: delta `(0.03, 0.06, 0.06)`
- 위험 구역 트리거: 면역 경계도 상승, 숙주 생명력 감소
- `Space` 입력 주입 + 위험 상호작용: 면역 경계도 상승
- 면역 경계도 임계치 주입 후 `InternalVirus` 전환
- `D` 입력 주입 후 바이러스 이동: delta `(0.08, 0.00, -0.03)`
- 변이 조각 트리거 3회 후 `MutationSelection` 진입, 조각 `3/3`
- `Digit3` 입력 주입 후 `MammalAdaptation` 적용, `RatHost` 복귀
- 백혈구 트리거 반복 후 `VirusFailed` 진입
- `Space` 입력 주입 후 `InternalVirus` 재시도, 안정도와 조각 수 초기화, 조각 3개 재활성화

해석:
실제 키보드 장치를 사람이 누른 것은 아니지만, Unity Input System 키 이벤트 주입과 기존 컨트롤러/트리거 핸들러를 통해 핵심 성공 루프와 실패 재시도 루프가 검증됐다.
```

```text
명령:
Unity MCP `Unity_GetConsoleLogs` Error 조회

결과:
Error 0건.

해석:
Play 런타임 검증 후 Unity 콘솔 게임 코드 오류는 관찰되지 않았다.
```

```text
명령:
Unity MCP `Unity_RunCommand` UI 레이아웃 검증

결과:
명령 실행 성공.
- 요청 해상도: `1920x1080`, 실제 `Screen.width/height`: `1920x1080`
- 요청 해상도: `1600x900`, 실제 `Screen.width/height`: `1920x1080`
- 요청 해상도: `1366x768`, 실제 `Screen.width/height`: `1920x1080`
- 요청 해상도: `1280x720`, 실제 `Screen.width/height`: `1920x1080`
- 쥐 프레이밍: viewport `(0.50, 0.47, 6.13)`
- 변이 선택 패널/버튼 화면 내 배치 확인
- 실패 패널/재시도 버튼 화면 내 배치 확인
- 텍스트 높이 경고: 0건

해석:
`1920x1080` 기준 UI 요소의 화면 밖 이탈, 변이 버튼 겹침, 실패 패널 배치 문제는 관찰되지 않았다. 다만 Unity Editor Game View가 낮은 요청 해상도로 실제 변경되지 않아 `1600x900`, `1366x768`, `1280x720`의 정확한 화면 캡처 검증으로는 인정하지 않는다.
```

```text
명령:
UI 레이아웃 검증 후 Unity MCP `Unity_GetConsoleLogs` Error 조회

결과:
Error 0건.

해석:
UI 레이아웃 검증 후에도 Unity 콘솔 게임 코드 오류는 관찰되지 않았다.
```

```text
명령:
Unity MCP `BuildWindowsPrototype`

결과:
빌더 메서드 완료 로그 확인. 단, Unity Sentis/Inference shader warning 때문에 MCP wrapper는 `success=false`로 반환.

해석:
`BuildWindowsPrototype`는 실패 예외 없이 끝났고, 빌드 산출물은 갱신됐다. 경고는 `Packages/com.unity.ai.inference` shader 경고이며 게임 코드 오류로 보이지 않는다.
```

```text
명령:
빌드 산출물 확인

결과:
- `Builds/RatHostPrototype/LastHostPrototype_Data/Managed/LastHost.Prototype.dll`
  - LastWriteTime: 2026-07-01 10:24:38
- `Builds/RatHostPrototype/LastHostPrototype_Data/Managed/Assembly-CSharp.dll`
  - LastWriteTime: 2026-07-01 10:24:38

해석:
Windows 빌드의 Managed 산출물이 이번 검증 시점에 갱신됐다.
```

```text
명령:
`Builds/RatHostPrototype/LastHostPrototype.exe` 실행본 5초 실행

결과:
`BUILD_RUN_OK pid=63772 aliveForSeconds=5 exitedAfterStop=True`

해석:
빌드 실행본은 즉시 크래시하지 않고 5초 이상 프로세스로 유지됐다.
```

```text
명령:
`C:\Users\User\AppData\LocalLow\DefaultCompany\Last Host\Player.log` 확인

결과:
로그 갱신 시각: 2026-07-01 10:26:11. 게임 코드 예외나 크래시 로그는 관찰되지 않음.

해석:
빌드 실행본 초기 구동 중 명확한 게임 코드 예외는 보이지 않았다.
```

```text
명령:
사용자 수동 플레이 피드백

결과:
- `WASD` 이동은 사용자가 직접 확인했다.
- `Space` 상호작용 등 이동 외 키는 현재 씬에서 상호작용 가능한 대상을 식별할 수 없어 사용자가 확인하지 못했다.

해석:
쥐 이동 수동 체감은 검증 근거로 추가한다. 반면 상호작용 키는 입력 로직 자체가 자동 검증에서 통과했더라도, 현재 씬이 플레이어에게 상호작용 대상을 충분히 드러내지 못하므로 수동 검증 차단 항목이자 씬 UX 결함으로 본다.
```

```text
명령:
Unity MCP `Unity_Camera_Capture` 및 씬 오브젝트 확인

결과:
- `NoisyPipeRiskInteractable` 존재 확인: 위치 `(2.2, 0.35, -2.2)`, 크기 `(0.9, 0.7, 0.9)`
- 쥐 시작 위치: `(-4.4, 0, -1)`
- 기본 카메라 캡처에서 쥐와 독성 물웅덩이는 보이지만 `NoisyPipeRiskInteractable`은 초기 화면에서 뚜렷하게 드러나지 않음
- HUD 목표 문구는 `하수도 탐색 중`으로, 상호작용 대상이나 접근 가능 상태를 알려주지 않음

해석:
자동 검증은 `RatRiskInteractable`을 직접 트리거했기 때문에 통과했지만, 실제 플레이어가 씬에서 발견하고 검증할 수 있는 상호작용 affordance가 부족하다.
```

```text
명령:
빌드 후 작업트리 확인과 자동 변경 복구

결과:
빌드 후 Unity가 다음 파일에 자동 직렬화/설정 변경을 남겼다.
- `UnityProject/Assets/Settings/DefaultVolumeProfile.asset`
- `UnityProject/Assets/Settings/PC_RPAsset.asset`
- `UnityProject/Assets/Settings/UniversalRenderPipelineGlobalSettings.asset`
- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- `UnityProject/Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`
- `UnityProject/ProjectSettings/ProjectSettings.asset`
- `UnityProject/ProjectSettings/UnityConnectSettings.asset`

검증 작업 범위 밖 변경이므로 `git restore`로 복구했다.

해석:
이번 작업은 검증 기록만 남기고 Unity 프로젝트 산출물을 변경하지 않는다.
```

## 검증하지 못한 항목

- `Space` 상호작용 수동 확인: 현재 씬에서 플레이어가 식별 가능한 상호작용 대상이나 피드백이 부족해 차단됨
- 빌드 실행본 내부에서 성공 루프와 실패 루프를 사람이 직접 플레이한 검증
- 1600x900, 1366x768, 1280x720 각각의 실제 화면 캡처 기반 UI 겹침/가독성 검증
- 저해상도 렌더링 또는 후처리 적용 후 UI 가독성 검증

## 실패 또는 경고

- 사용자 수동 확인 결과, `WASD` 이동은 확인됐지만 이동 외 상호작용 키는 씬에서 대상이 드러나지 않아 검증 불가
- Unity MCP 런타임 검증 스크립트 작성 중 2회 실패:
  - NUnit 참조 누락으로 컴파일 실패
  - Reflection 네임스페이스 사용 제한으로 거부
  - 두 항목은 검증 스크립트 실행 방식 문제이며, 이후 명시 호출 방식으로 핵심 테스트는 통과
- Windows 빌드 중 `Packages/com.unity.ai.inference` / Sentis shader warning 다수 발생
- 빌드 명령은 warning 때문에 MCP wrapper상 `success=false`로 보고됐으나, 빌더 메서드 완료 로그와 산출물 갱신, 실행본 5초 실행은 확인됨
- Unity Editor Game View는 `Screen.SetResolution` 요청 후에도 실제 `Screen.width/height`가 `1920x1080`으로 유지되어 낮은 해상도 3종의 자동 화면 검증은 미검증으로 남음

## QA/검증 에이전트 판정

```text
검증 대상:
쥐 숙주 프로토타입 전체 플레이 가능성 검증 기록.

결과:
자동화 가능한 핵심 루프 검증은 통과로 판단한다. 쥐 숙주 시작부터 내부 바이러스 미니게임, 변이 선택, 쥐 모드 복귀, 실패 후 재시도까지 승인된 1차 프로토타입 루프의 주요 상태 전환은 기록상 확인되었다.

미검증 항목:
- 사람이 실제 키보드로 Game View에서 조작한 수동 체감
- 빌드 실행본 안에서 사람이 성공/실패 루프를 직접 플레이한 검증
- 기준 해상도별 화면 캡처 기반 UI 겹침/가독성 검증
- 저해상도 렌더링 또는 후처리 적용 후 UI 가독성 검증

남은 위험:
- 입력 주입 기반 검증은 실제 조작감, 반응성, 카메라 체감 문제를 완전히 대체하지 못한다.
- 빌드 wrapper가 warning 때문에 `success=false`를 반환했으므로, 빌드 성공은 산출물 갱신과 5초 실행 확인 수준의 조건부 판단이다.
- UI/카메라가 16:9 PC 해상도에서 핵심 플레이를 가리지 않는지는 아직 완료 기준을 충족하지 못했다.
- 총괄 관리자 판정과 `completion-report.md`가 아직 없어 최종 완료 보고 게이트는 남아 있다.

완료 판단:
부분 통과

완료 판단 근거:
공식 구현 계획의 핵심 자동 검증, Play 런타임 루프, 콘솔 오류 확인, 빌드 산출물 및 초기 실행 확인은 통과했다. 다만 플레이어블 프로토타입 완료 조건에 포함된 빌드 실행본 직접 플레이, 수동 조작 체감, 기준 해상도 UI/카메라 확인이 미검증이므로 전체 완료 통과로 판정할 수 없다.

다음 권장 작업:
빌드 실행본에서 사람이 성공 루프와 실패 재시도 루프를 직접 플레이하고, 기준 해상도 캡처로 UI 겹침/가독성 및 카메라 가림 여부를 확인한 뒤 총괄 관리자 판정을 진행한다.
```

## 프로젝트 총괄 관리자 판정

```text
판정:
사용자 결정 필요

근거:
자동화 가능한 핵심 루프 검증, 테스트 18/18, Play 런타임 성공/실패 루프, 콘솔 Error 0건, 빌드 산출물 갱신과 5초 실행 확인은 보고 근거로 충분하다. 다만 공식 완료 기준인 빌드 실행본 직접 플레이, 수동 조작 체감, 1600x900/1366x768/1280x720 실제 캡처 기반 UI/카메라 검증이 남아 있어 전체 완료 통과로는 보고할 수 없다.

QA/검증 기록 확인:
확인됨. QA/검증 에이전트 `Singer`의 부분 통과 판정과 미검증 항목이 기록되어 있다.

수정 필요:
완료 통과 표현 금지. 전체 완료 처리 전 `completion-report.md` 작성 또는 동등한 완료 보고 기록이 필요하다.

문제 사안:
공식 플레이어블 완료 조건 일부 미충족. 빌드 wrapper `success=false`가 Sentis/Inference shader warning 때문이라는 조건부 해석도 함께 보고해야 한다.

사용자 결정 필요:
현재 결과를 부분 통과로 인정하고 후속 수동/해상도 검증을 별도 작업으로 넘길지, 지금 추가 검증 후 완료 판단을 다시 받을지 결정이 필요하다.
```

## 완료 판단

부분 통과.

## 완료 판단 근거

자동화 가능한 핵심 루프와 빌드 초기 실행은 통과했고, 사용자가 `WASD` 이동을 직접 확인했다. 다만 `Space` 상호작용은 현재 씬에서 식별 가능한 상호작용 대상이 부족해 수동 검증이 차단됐고, 빌드 실행본 직접 플레이와 낮은 해상도 캡처 기반 UI/카메라 확인도 아직 미검증이다.

## 사용자 결정 필요

- 현재 결과를 `부분 통과`로 인정하고 후속 수동/해상도 검증을 별도 작업으로 넘길지
- 지금 추가 검증을 계속 진행해 전체 완료 판단을 다시 받을지

## 2026-07-10 재정리 검증

사용자 요청 `전체 플레이어블 검증 재정리 작업 진행해`에 따라 기존 부분 통과 항목을 최신 구현 기준으로 다시 확인했다.

### 실행한 검증

```text
명령:
Unity MCP `Unity_ManageEditor.GetState`
Unity MCP `Unity_ManageScene.GetActive`

결과:
- Unity Editor 실행 중
- `IsPlaying=false`
- `IsPaused=false`
- `IsUpdating=false`
- 활성 씬: `RatHostPrototype`
- 씬 경로: `Assets/_Project/Scenes/RatHostPrototype.unity`
- 씬 dirty=false

해석:
재정리 검증 시작 시 Unity Editor와 씬은 검증 가능한 상태였다.
```

```text
명령:
Unity MCP `Unity_RunCommand`로 `RatHostPrototypeCoreTests` 전체 TestAttribute 메서드 리플렉션 호출

결과:
- `System.Reflection` import와 `BindingFlags` 사용은 MCP 보안 규칙으로 거부됨.
- 허용된 `GetMethods()` 경로로 호출 시 71개 중 70개 통과.
- `RatHostController_UpdateDoesNotMoveInactiveCharacterController` 1개는 `LogAssert.NoUnexpectedReceived()`가 실제 Unity Test Runner log scope 밖에서 실행되어 `No log scope is available`로 실패.
- 별도 Unity batch Test Runner 명령은 종료 코드 0이었으나 `editmode-results.xml`과 로그 파일을 생성하지 않아 유효 결과로 인정하지 않음.

해석:
실제 Test Runner 결과로는 인정하지 않는다. 다만 70/71 수동 호출과 실패 원인상 게임 코드 회귀로 보이는 항목은 관찰되지 않았다.
```

```text
명령:
Unity MCP Play 진입 후 런타임 루프 검증

결과:
- 초기 `RatHost` 모드 확인
- RatHost 루트 활성, VirusMinigame 루트 비활성 확인
- HUD `쥐 숙주` 표시 확인
- `NoisyPipeRiskInteractable`와 `InteractionMarkerRing` 존재 확인
- 면역 경계도 100 주입 후 `InternalVirus / WhiteBloodCellEvasion` 진입 확인
- 변이 조각 3개 처리 후 `MutationSelection` 진입 확인
- `MammalAdaptation` 선택 후 `RatHost` 복귀, HUD `포유류 적응` 표시 확인
- 백혈구 피격 3회 처리 후 `VirusFailed` 진입 확인
- 실패 패널 활성과 `보상 없이 쥐 숙주로 복귀` 문구 확인
- 실패 복귀 후 `RatHost` 복귀, 실패 패널 비활성, 기존 변이 유지 확인
- 검증 로그: `EDITOR_FULL_PLAYABLE_LOOP_PASS safetyHits=3`

해석:
에디터 Play 상태에서 성공 루프와 실패 후 보상 없는 복귀 루프는 최신 구현 기준으로 다시 통과했다.
```

```text
명령:
Unity MCP `Unity_ReadConsole` Error/Warning
Unity MCP `Unity_ManageEditor.Stop`
Unity MCP `Unity_ManageScene.GetActive`

결과:
- 에디터 Play 검증 중 Console Error/Warning 0건
- Play 종료 성공
- 종료 후 활성 씬 `RatHostPrototype` dirty=false

해석:
에디터 Play 검증은 콘솔 오류/경고와 씬 dirty 변경 없이 종료됐다.
```

```text
명령:
Unity MCP `RatHostPrototypeSceneBuilder.BuildWindowsPrototype()`

결과:
- `BUILD_WINDOWS_PROTOTYPE_DONE` 로그 확인
- MCP wrapper는 Sentis/Inference shader warning 때문에 `success=false` 반환
- `Builds/RatHostPrototype/LastHostPrototype_Data/Managed/LastHost.Prototype.dll`
  - LastWriteTime: 2026-07-10 22:12 KST
- `Builds/RatHostPrototype/LastHostPrototype_Data/Managed/Assembly-CSharp.dll`
  - LastWriteTime: 2026-07-10 22:12 KST
- `Builds/RatHostPrototype/LastHostPrototype.exe`
  - LastWriteTime: 2026-06-30 11:20 KST

해석:
빌드 프로세스는 완료 로그와 최신 Managed 산출물을 남겼다. player launcher exe 타임스탬프는 갱신되지 않았지만 데이터 어셈블리는 최신 구현으로 갱신됐다. Sentis/Inference shader warning은 이전 검증과 동일한 패키지 경고로, 게임 코드 오류로 보지는 않는다.
```

```text
명령:
Windows 빌드 실행본 창 실행
`-screen-width`, `-screen-height`, `-screen-fullscreen 0`, `-logFile` 사용

결과:
- `1920x1080`, `1600x900`, `1366x768`, `1280x720` 실행본 모두 창 생성 후 중단 전까지 프로세스 유지
- Player.log 4종 및 재촬영 Player.log에서 `Exception`, `Error`, `Crash` 패턴 없음
- 각 로그에 Unity `6000.4.6f1` 초기화 확인

해석:
빌드 실행본은 네 기준 해상도에서 초기 구동과 종료가 가능했고, 명확한 게임 코드 예외나 크래시는 관찰되지 않았다.
```

```text
명령:
Windows 빌드 실행본 16:9 해상도별 스크린샷 확인

결과:
- `1920x1080`: `artifacts/build-rathost-after-w-1920x1080.png`
  - HUD와 RatHost 화면 표시. 쥐, 하수도, 위험 요소가 보이고 HUD가 핵심 플레이 영역을 완전히 가리지 않음.
- `1600x900`: `artifacts/build-topmost-rathost-after-w-1600x900.png`
  - HUD와 쥐/하수도 화면 표시. 텍스트 겹침이나 핵심 플레이 가림 없음.
- `1366x768`: `artifacts/build-topmost-rathost-after-w-1366x768.png`
  - HUD와 쥐/위험 요소 표시. `오염 노출` 피드백과 게이지 표시도 화면 안에 유지됨.
- `1280x720`: `artifacts/build-topmost-rathost-idle-1280x720.png`
  - HUD와 쥐/하수도 화면 표시. 화면 하단 여유는 줄지만 핵심 플레이 가림 없음.

해석:
기준 16:9 해상도 4종에서 RatHost 기본 화면의 HUD/카메라 가림 문제는 관찰되지 않았다.
```

```text
명령:
Windows 빌드 실행본 1280x720에서 실제 F6/Space 입력

결과:
- `artifacts/build-1280x720-after-f6.png`
  - `내부 바이러스 / 면역 신호 억제` HUD 표시 확인
- `artifacts/build-1280x720-after-missed-f6.png`
  - 입력 실패 후 `면역 반응 돌파 실패`, 실패 패널, `재시도` 표시 확인
- `artifacts/build-1280x720-after-failure-space.png`
  - Space 입력 후 RatHost 복귀, 면역 경계도 60% 유지 확인
- timed Space 입력으로 면역 신호 억제 성공과 변이 선택까지는 도달하지 못함

해석:
빌드 실행본에서 내부 미니게임 진입, 실패 패널, Space 복귀는 직접 확인했다. 다만 자동 타이밍 입력으로 성공 루프를 완료하지 못했으므로 빌드 실행본 직접 성공 루프는 여전히 미검증으로 남긴다.
```

```text
명령:
빌드 후 작업트리 확인과 Unity 자동 변경 되돌림

결과:
빌드 과정에서 다음 파일이 자동 직렬화/설정 변경됨.
- `UnityProject/Assets/Settings/DefaultVolumeProfile.asset`
- `UnityProject/Assets/Settings/PC_RPAsset.asset`
- `UnityProject/Assets/Settings/UniversalRenderPipelineGlobalSettings.asset`
- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- `UnityProject/Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`
- `UnityProject/ProjectSettings/ProjectSettings.asset`
- `UnityProject/ProjectSettings/UnityConnectSettings.asset`

검증 범위 밖 변경이므로 `git restore`로 되돌림.
되돌림 후 UnityProject diff는 없음.

해석:
이번 재정리 작업은 Unity 코드/씬/ProjectSettings 변경 없이 검증 기록과 artifacts만 남긴다.
```

### 2026-07-10 재정리 시점 검증하지 못한 항목

아래 항목은 빌드 실행본 성공 루프 최종 확인 전 기준이다. 이 중 `면역 신호 억제` 경로의 빌드 실행본 성공 루프는 `20260710-224037` 후속 시도에서 해소됐다.

- 빌드 실행본에서 쥐 모드 시작부터 백혈구 회피 성공, 변이 선택, 쥐 모드 복귀까지 사람이 직접 완료한 검증
- 빌드 실행본에서 자동 입력만으로 변이 선택 성공 루프를 완료하는 검증
- 실제 사용자 조작감, 난이도, 설명 없이 목표 이해 여부

### 2026-07-10 QA/검증 에이전트 판정

```text
후속 성공 루프 최종 확인 전 판정.

검증 대상:
쥐 숙주 프로토타입 전체 플레이어블 검증 재정리.

결과:
개선된 부분 통과. 에디터 Play 기준 성공 루프와 실패 복귀 루프는 최신 구현 기준으로 다시 통과했다. Windows 빌드 실행본은 네 기준 해상도에서 구동되고 Player.log에 예외/크래시가 없었으며, RatHost 화면의 HUD/카메라 가림 문제도 관찰되지 않았다. 빌드 실행본에서 F6 내부 미니게임 진입, 실패 패널, Space 복귀도 확인했다.

미검증 항목:
- 빌드 실행본에서 성공 루프를 직접 완료하는 검증
- 실제 사용자 조작감과 난이도 체감

남은 위험:
- 빌드 실행본 성공 루프는 자동 타이밍 입력으로 닫지 못했다.
- 빌드 wrapper는 여전히 Sentis/Inference shader warning 때문에 `success=false`를 반환한다. 빌드 완료 로그와 산출물 갱신은 있으나 wrapper 기준 완전 성공은 아니다.
- 직접 Test Runner 결과 파일은 생성되지 않았다. MCP 수동 호출은 70/71 통과이며, 실패 1건은 Test Runner log scope 외부 실행 방식 문제다.

완료 판단:
부분 통과 유지. 다만 이전보다 검증 공백은 줄었다.
```

### 2026-07-10 프로젝트 총괄 관리자 판정

```text
후속 성공 루프 최종 확인 전 판정.

판정:
사용자 결정 필요

근거:
공식 플레이어블 완료 조건 중 Windows 빌드 구동, 실패 후 보상 없는 복귀, 16:9 UI/카메라 확인은 이번 검증으로 상당 부분 보강됐다. 그러나 `빌드 실행본에서 쥐 모드 시작부터 변이 선택 후 쥐 모드 복귀까지 한 번 이상 완료` 항목은 아직 직접 완료되지 않았다. 따라서 전체 플레이어블 완료 통과로 닫을 수는 없다.

QA/검증 기록 확인:
확인됨. 에디터 Play 루프, 빌드 구동, 해상도 스크린샷, Player.log, 실패 복귀 기록이 있다.

MCP 플레이 체크 확인:
에디터 Play 체크 통과. 빌드 실행본 직접 성공 루프는 미검증.

사용자 결정 필요:
1. 사용자가 빌드 실행본에서 직접 성공 루프 1회를 플레이해 완료 확인할지
2. 빌드 실행본 자동 검증 훅 또는 테스트 전용 진입 방식을 별도 승인해 구현할지

완료 보고:
부분 통과와 남은 사용자 결정 항목으로 보고 가능. 전체 완료로는 보고하지 않는다.
```

## 2026-07-10 빌드 실행본 성공 루프 최종 확인

### 검증 대상

Windows 빌드 실행본에서 내부 미니게임 성공, 변이 선택, 쥐 숙주 모드 복귀가 실제 실행본 입력으로 한 번 이상 완료되는지 확인한다.

### 실행한 검증

```text
명령:
Windows 빌드 실행본 1280x720 창 실행 후 네이티브 키 입력 주입.

실행본:
`Builds/RatHostPrototype/LastHostPrototype.exe`

입력 경로:
1. RatHost 화면 진입 확인
2. `F6`으로 기존 디버그 경로의 `면역 신호 억제` 내부 미니게임 진입
3. 약 1초 간격 `Space` 입력 10회
4. 변이 선택 화면에서 `1` 입력
5. RatHost 화면 복귀 확인

성공 시도:
`artifacts/build-success-loop-attempt-20260710-224037.txt`
```

```text
결과:
- 시작 화면: `artifacts/build-success-loop-start-20260710-224037.png`
  - `쥐 숙주`, `하수도 탐색 중`, 숙주 생명력 100/100, 면역 경계도 0%, 획득 변이 없음 확인
- 내부 미니게임 진입: `artifacts/build-success-loop-signal-20260710-224037.png`
  - `내부 바이러스`, `면역 신호 억제`, `신호 차단 0/8` 확인
- 미니게임 성공 후 변이 선택: `artifacts/build-success-loop-mutation-20260710-224037.png`
  - `변이 선택` 화면, `잠복 강화`, `신경 조종`, `포유류 적응` 선택지 표시 확인
- 변이 선택 후 복귀: `artifacts/build-success-loop-return-20260710-224037.png`
  - `쥐 숙주`, `하수도 탐색 중`, 면역 경계도 25%, 획득 변이 `잠복 강화` 확인
- Player.log: `artifacts/player-success-loop-20260710-224037.log`
  - `Exception`, `Error`, `Crash` 패턴 0건
  - Unity `6000.4.6f1` 초기화 확인
  - `<b>[App UI]</b> Unable to find an AppUISettings instance, creating a default one...` 로그가 있으나 실행 중단, 예외, 크래시로 이어지지 않음
```

### 실패/보정 시도

```text
참고 기록:
- `20260710-223706`: Unity splash 중 입력이 들어가 무효
- `20260710-223826`: 미니게임 진입 후 반복 입력 간격이 포커스 보정 시간 때문에 밀려 실패 복귀
- `20260710-223931`: `신호 차단 7/8`까지 도달했으나 성공 조건 미달
- `20260710-224037`: 10회 timed Space 입력으로 변이 선택과 RatHost 복귀 성공
```

### QA/검증 에이전트 추가 판정

```text
판정:
빌드 실행본 성공 루프 최종 확인 완료.

근거:
Windows 빌드 실행본에서 실제 창 포커스를 잡고 키 입력을 주입해 `면역 신호 억제` 성공, 변이 선택 화면, `잠복 강화` 선택, RatHost 복귀를 확인했다. 성공 시도 Player.log에는 `Exception`, `Error`, `Crash` 패턴이 없다.

제한:
이번 빌드 실행본 성공 확인은 기존 F6 디버그 진입을 사용한 `면역 신호 억제` 경로다. 자연 경계도 100% 도달 후 기본 `백혈구 회피`에서 조각 3개를 직접 수집하는 빌드 실행본 루프는 별도로 주장하지 않는다. 다만 Editor Play에서는 백혈구 회피 성공 루프가 이미 통과했다.

남은 항목:
사용자 수동 조작감, 난이도, 설명 없이 목표 이해 여부는 별도 사용자/테스터 확인 항목으로 남긴다.
```

### 프로젝트 총괄 관리자 추가 판정

```text
판정:
자동/대리 입력 기준의 빌드 실행본 성공 루프 차단 해소.

근거:
이전 차단 항목이었던 `빌드 실행본에서 내부 미니게임 성공, 변이 선택, 쥐 모드 복귀 1회`가 20260710-224037 시도에서 스크린샷과 Player.log로 확인됐다.

완료 보고 가능 범위:
빌드 실행본 성공 루프 최종 확인은 완료로 보고 가능하다.

완료로 주장하지 않는 범위:
사람 플레이어가 설명 없이 목표를 이해하는지, 난이도가 적절한지, 자연 경계도 100%에서 백혈구 회피 조각 수집을 빌드 실행본에서 직접 완료했는지는 이번 추가 검증으로 주장하지 않는다.
```
