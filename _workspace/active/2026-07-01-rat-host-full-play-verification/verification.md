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
