# 작업 로그

## 2026-07-21 KST — 증상 확인

- 사용자는 Unity Game 뷰 `Display 1`에서 `No cameras rendering`과 카메라 추적 미표시를 확인했다.
- MCP 읽기 전용 점검에서 `IsometricCamera`는 활성 MainCamera·직교 카메라이고 카메라 캡처도 정상이다.
- 이 카메라는 `RatPixelTrial960x540.renderTexture`에 출력되며, `WorldPixelOutput` RawImage가 같은 텍스처를 참조한다. Display 1에 직접 렌더하는 카메라가 없어 Game 뷰 메시지가 나타나는 구조다.
- 수정 원칙: 저해상도 Point 월드 출력과 HUD 분리를 유지하면서 Display 1에 직접 프레임을 제공한다.

## 2026-07-21 KST — 통합·독립 QA·총괄 검토

- Unity 씬/통합 담당은 `GameViewFrameCamera`를 Display 1용 프레임 카메라로 추가했다. IsometricCamera는 기존대로 960×540 Point RenderTexture를 렌더하고, RawImage와 HUD가 화면을 구성한다.
- 독립 QA는 활성 Display 1 프레임 카메라, IsometricCamera RT, RawImage/HUD 계층, 1920×1080 Play, 쿼터뷰 추적, Console Error/Warning 0을 대체 MCP 근거로 통과시켰다. Game 뷰 UI 문구의 직접 캡처는 MCP 제한으로 남았다.
- 총괄 검토는 출력 구조 적합을 확인했으나, 작업 시작 후 `ProjectSettings.asset`에 `APP_UI_EDITOR_ONLY` define 변경이 나타난 것을 범위 충돌로 판정했다. 사용자 변경 가능성이 있어 원인이 확인되기 전에는 되돌리거나 커밋하지 않는다.

## 2026-07-21 KST — Unity 씬/통합 적용 및 자체 Play 점검

- 실제 씬에는 `IsometricCamera`만 있었고, 이 카메라는 MainCamera 태그를 유지한 채 `RatPixelTrial960x540`에 렌더한다. Display 1에 직접 쓰는 카메라가 없었던 원인을 재확인했다.
- 최소 변경으로 `GameViewFrameCamera`를 추가했다. 이 카메라는 Display 1(`targetDisplay=0`)에만 프레임을 제공하고, `targetTexture=null`, `cullingMask=0`, 검은 Solid Color clear, depth `-100`, Untagged다. 월드 렌더는 하지 않으며 IsometricCamera의 RT 출력·MainCamera 태그·추적을 바꾸지 않는다.
- 기존 `WorldPixelOutputPreview` ScreenSpaceOverlay RawImage는 IsometricCamera의 `RatPixelTrial960x540` Point RT를 계속 표시하고, HUD `Canvas` Overlay 정렬 0을 유지한다. Display 1 프레임 카메라는 Overlay UI가 그려질 기반만 제공한다.
- Unity MCP Play에서 카메라 2개(월드 RT 카메라 + Display 1 프레임 카메라), Display 1 프레임 카메라 조건, RawImage/RT·HUD 연결을 확인했다. 런타임 쥐를 `(0.5, 0, 0.25)` 이동한 뒤 명시적 RatHost 카메라 적용 시 쿼터뷰 카메라가 `(0.47, 0.03, 0.26)` 이동해 추적을 확인했다.
- Play 중 화면은 `1920×1080`이었고, Console Error/Warning 0건이다. 기존 dirty 씬을 저장·재로드하지 않았고, QA의 독립 Game 뷰 시각 확인을 남긴다.

## 2026-07-21 KST — 씬 저장 범위 확정

- 초기 진단 기준 `RatHostPrototype` 씬은 dirty가 아니었고, 현재 Scene diff는 `GameViewFrameCamera` 통합에 해당함을 확인했다.
- Unity MCP `EditorSceneManager.SaveScene`으로 `Assets/_Project/Scenes/RatHostPrototype.unity`만 저장했다. 결과 `dirty=False`.
- 저장 후 읽기 전용 재확인: IsometricCamera는 `RatPixelTrial960x540`을 계속 targetTexture로 사용하고, `GameViewFrameCamera`는 Display 1(`targetDisplay=0`), targetTexture 없음, culling mask 0, enabled 상태다. WorldPixelOutput RawImage의 RT 연결도 유지됐다.
- `ProjectSettings.asset`은 저장·수정·되돌리지 않았다. `APP_UI_EDITOR_ONLY` 자동 define 변경은 작업 범위 밖으로 보존한다.

## 2026-07-23 KST — 실제 D·W+D 재검증과 MCP transport 차단

- Codex Unity MCP `GetState`는 `Transport closed`로 반복 실패했다. Unity Editor는 실행 중이며 Project Settings의 Unity MCP Server는 Running/direct client 1개로 보였지만, 현재 Codex 세션의 tool transport는 복구되지 않았다.
- 실제 Unity Play 상태를 상단 Play 버튼 활성 색으로 확인한 뒤 D·W+D를 각각 0.7초 입력했다. Game 뷰의 쥐·발판·주변 월드 변화가 캡처돼 실제 시각 이동은 확인됐다.
- 키 해제 후 0.25초와 1.25초의 화면·HUD도 달랐다. 카메라 보간, 키 해제 미수신, 자동 상태 전환의 원인은 현 증적만으로 분리하지 않았다.
- 증적은 `evidence/actual-direction-recheck-2026-07-23/`에 보관했다. 독립 QA와 총괄은 부분 통과/전체 보류를 기록했다.
- 다음 세션에서는 Unity를 재시작하기 전에 새 Codex MCP transport를 먼저 만들고, `GetState`·Console 성공 후 입력 상태·RatHost/카메라 좌표·HUD를 같은 Play 세션에서 대조한다.

## 2026-07-23 KST — 새 MCP 세션 무입력 원인 분리 (조정자 읽기 전용 확인)

- 새 Unity MCP 세션에서 `GetState`와 Console 조회가 정상 복구됐다. 활성 씬은 `RatHostPrototype`, Play 진입 전 `isDirty=False`였다.
- 같은 Play 세션의 두 표본(약 1.25초 간격)에서 `RatHostController.CurrentMoveWorldDirection`은 모두 `(0,0,0)`이었다. 즉 사용자 입력 잔류는 관찰되지 않았다.
- 그러나 `CurrentHostInstinctMoveDirection`과 `CurrentResolvedMoveDirection`은 비영(非零)이고 `IsHostInstinctPaused=False`였다. 쥐는 `(-3.15, 0.06, 3.09)`에서 `(-3.38, 0.06, 3.40)`으로 이동했고, IsometricCamera도 `(-5.92, 7.44, -3.40)`에서 `(-6.15, 7.47, -3.00)`으로 추적했다.
- HUD는 같은 구간에 `오염 노출 +13.24`에서 `하수도 탐색 중`으로 바뀌었다. 첫 표본의 카메라 목표 오차는 `(0.03,-0.02,-0.09)`, 두 번째는 `(0.03,0.01,0.00)`이었다.
- 따라서 이전 release 캡처의 화면·HUD 변화에는 **자동 숙주 본능 이동과 그에 따른 탐색 상태 갱신이 직접 기여**한다. 이 증거는 카메라 보간 또는 픽셀 격자 영향이 전혀 없다는 뜻은 아니며, 실제 WASD 이동 중 상대 떨림은 별도 확인이 필요하다.
- Unity Console Error는 0건이었다. 경고는 `ManageGameObject` 조회가 Unity AI MCP의 `TransformHandle` 직렬화에서 낸 도구 경고뿐이며 게임 코드 경고로 분류하지 않았다. Windows Computer Use 앱 승인은 시간 초과되어 실제 OS 키를 이번 세션에서 추가 전송하지 못했다.

## 2026-07-23 KST — 쿼터뷰 쥐 중심 즉시 추적 수정 (사용자 명시 승인)

- 사용자 확인 증상: 쿼터뷰에서 쥐가 화면 중심에 남지 않고 카메라와 따로 움직인다.
- 원인: `ApplyQuarterView`가 다른 카메라 모드와 동일하게 `followSharpness=18` 보간을 적용해, 이동한 쥐보다 카메라가 뒤따랐다.
- 변경: `PrototypeCameraController.ApplyQuarterView`만 목표 위치·회전을 즉시 적용한 뒤, 기존 `RefreshQuarterViewOutputPixelSnap`을 실행하도록 바꿨다. ThirdPerson·TopView·VirusView의 보간, 960×540 Point RT, 쥐 이동·충돌·면역·변이 로직은 변경하지 않았다.
- 회귀 조건: 쿼터뷰에서 카메라를 의도적으로 목표에서 이탈시킨 뒤 쥐를 이동하고 `ApplyQuarterView(..., snap:false)`를 호출해도 카메라가 새 쥐 목표 위치에 즉시 일치하는 EditMode 테스트를 추가했다.
- 실행 확인: Unity 스크립트 표준 검증에서 새 테스트는 진단 0건, 카메라 스크립트는 기존 일반 경고 1건뿐이었다. Play 런타임에서 공개 즉시 적용 경로는 출력 픽셀 스냅 기대 위치와 `delta=(0,0,0)`으로 일치했다. Unity MCP의 프레임 진행 제약으로 `LateUpdate`를 실제 OS WASD 중 독립 검증하지 못했으므로 QA·사용자 Play 확인은 계속 필요하다.

## 2026-07-23 KST — WASD 입력 방향과 숙주 본능 분리 수정 (사용자 재현 증상)

- 사용자 증상: 키를 누르면 쥐가 의도하지 않은 곳으로 이동한다.
- 원인: 기본 조종력 `0.35`에서 `RatHostControlModel`이 입력 35%와 랜덤 숙주 본능 65%를 합성했다. 따라서 WASD 입력 자체가 카메라 상대 의도 방향과 달라졌다.
- 변경: 입력이 존재할 때는 입력 방향을 그대로 이동 방향으로 사용한다. 입력이 없을 때의 본능 배회, 반대 방향 입력 시 강제 조종 감속·면역 패널티는 유지한다.
- 검증: `RatHostControlModel`과 EditMode 테스트 파일의 Unity 표준 검증은 모두 진단 0건이었다. 런타임 모델 호출에서 본능 전진 + 우측 입력은 우측, 본능 전진 + 후진 입력은 후진(강제 조종), 무입력은 본능 전진으로 확인했다. 실제 OS WASD Game 뷰 확인은 사용자/독립 QA 단계로 남는다.

## 2026-07-23 KST — 무입력 쥐·카메라 자동 배회 제거 (사용자 재현 증상)

- 사용자 재현: 방향키를 누르지 않아도 쥐와 카메라가 따로 움직이며 돌아다닌다.
- 원인: 무입력 시 `RatHostControlModel`이 숙주 본능 방향과 passive speed를 반환했다. 쥐가 이동하면 쿼터뷰 카메라가 이를 따라가므로 두 오브젝트가 함께 자동 배회했다.
- 변경: 무입력 제어 프레임을 이동 방향 `(0,0,0)`, 속도 0으로 고정했다. 입력 중 방향 우선과 반대 입력 강제 조종 패널티는 유지한다.
- 검증: 모델·테스트 스크립트 Unity 표준 검증 진단 0건. 런타임 모델 호출에서 `idleDirection=(0,0,0)`, `idleSpeed=0`, 우측 입력은 `(1,0,0)`으로 확인했다. 실제 Game 뷰 Play 확인은 사용자/독립 QA 단계로 남는다.

## 2026-07-23 KST — MCP 좌→우 시각 정합 재현

- Windows UI 자동화는 Unity 창 상태 캡처에서 `SetIsBorderRequired failed (0x80004002)`가 복구 1회에도 반복돼 실제 OS 키 주입에 사용하지 못했다. Unity InputSystem 동적 키보드 주입도 동적 명령 환경에서 `NullReference`로 중단돼 Play 변경을 저장하지 않고 종료했다.
- 대체 MCP Play 검증으로 RatHost 루트를 좌측 1.5 unit, 이어서 우측 3 unit 이동시키고 매 단계 `ApplyCameraNow`·RatVisual 방향/접지/픽셀 스냅을 실행했다.
- 좌측: root viewport `(0.50,0.48)`, visual viewport `(0.50,0.49)`; 우측 전환 후: root `(0.50,0.48)`, visual `(0.50,0.50)`였다. 누적 화면 이탈은 재현되지 않았고 Console Error는 0건이었다.
- 이 대체 검증은 현재 저장된 루트·시각·카메라 코드의 좌→우 정합을 확인한 것이며, 실제 OS 키 이벤트의 Unity 입력 루프는 Windows 자동화 복구 뒤 독립 확인이 필요하다.

## 2026-07-23 KST — RatHost 시각 중심 앵커 보정

- Edit 상태에서 `RatHost` 루트는 `y=0`, CharacterController 중심은 `y=0.35`, `RatVisual` 스프라이트 bounds 중심은 루트보다 약 `y=0.5` 위에 있음을 확인했다. 이는 충돌 루트가 발 기준인 정상 구조다.
- 다만 `RatVisual`은 8방향별 가시 발 바닥 보정값(`0.09375~0.40625`)으로 접지되고, 기존 쿼터뷰 초점 높이 `0.3`은 화면상 스프라이트 중심을 y `0.54~0.56`에 두었다.
- Play 중 4방향을 전환해 초점 후보를 대조한 결과, `quarterViewFocusHeight=0.9`에서 스프라이트 중심 y 범위가 `0.4942706~0.5118691`로 중앙에 수렴했다.
- `PrototypeCameraController` 기본값과 `RatHostPrototype`의 `IsometricCamera` 직렬화 값을 `0.9`로 저장했다. 이동 루트와 충돌체는 옮기지 않았다.
- 표준 스크립트 검증은 기존 Update 문자열 연결 권고 1건 외 오류가 없고, MCP Play 후 Console Error/Warning은 0건이었다. 실제 OS WASD 입력 경로는 Windows UI 자동화 문제로 계속 별도 확인 대상이다.

## 2026-07-24 KST — 픽셀 처리 이후 누적 이탈 원인 확정·수정

- 픽셀 시험 커밋 `52bf071`에서 추가된 `RatVisual` 64 PPU 월드 스냅과 쿼터뷰 540px 출력 스냅을 4조합으로 MCP Play 측정했다.
- 0.003유닛씩 총 0.72유닛 이동한 수정 전 결과:
  - 둘 다 끔: 화면 상대 이동 오차 사실상 0px.
  - 카메라 출력 스냅만 켬: 약 1px 범위.
  - RatVisual 스냅 켬: `RatVisual.localPosition.x=-0.72`, 화면 x 약 34px·y 약 10px 이탈.
  - 둘 다 켠 기존 씬: 같은 약 34px 이탈.
- 원인은 자식 `RatVisual`의 이전 월드 위치를 반올림한 뒤 그 결과를 다시 자식 Transform에 저장해, 다음 부모 이동 때 로컬 역방향 오프셋으로 누적한 것이다. 이는 “키를 누르는 시간에 따라 쥐와 카메라 간 거리가 멀어진다”는 사용자 증상과 일치한다.
- `RefreshPixelSnap()`이 항상 `RatHost` 루트를 원본 앵커로 사용하도록 수정했다. 씬에서는 중복 `RatVisual.enablePixelSnap`을 비활성화하고, 960×540 Point RenderTexture·카메라 출력 스냅은 유지했다.
- 수정 후 같은 측정에서 강제로 RatVisual 스냅을 다시 켜도 최종 로컬 오프셋은 약 `-0.01`로 반 격자 이내였고, 현재 씬 설정(시각 스냅 끔·카메라 스냅 켬)은 약 1px 범위였다.
- 240fps 상당의 좌 1초→우 2초 전환 720스텝에서 수평 루트-시각 최대 거리는 `0`, 화면 x 범위는 약 `0.924px`였다. Console Error/Warning은 0건이었다.
- Windows Computer Use는 Unity 창 상태 캡처에서 `SetIsBorderRequired failed (0x80004002)`가 선택 갱신 후 복구 1회에도 반복돼 실제 OS WASD 자동 입력은 수행하지 않았다.

## 2026-07-24 KST — 숙주 본능 랜덤 이동 안전 복구

- 사용자 확인에 따라 픽셀 스냅 누적 이탈 수정은 유지하고, 원인 분리를 위해 임시 제거했던 무입력 숙주 본능 이동만 복구했다.
- 제어 우선순위는 다음과 같이 고정했다.
  - 무입력 + 본능 활성: 숙주 본능 방향으로 기본 속도의 0.45배 이동.
  - 무입력 + 본능 휴지: 정지.
  - WASD 입력: 현재 카메라 상대 입력 방향을 그대로 사용. 랜덤 본능 벡터를 합성하지 않음.
  - 본능 반대 방향 강제 조종: 기존 감속·면역 경계도 패널티 유지.
- Unity 스크립트 검증에서 모델과 테스트 파일 모두 진단 0건이었다.
- MCP Play에서 실제 `RatHostController.Update()`를 300회 호출해 약 6초 상당의 무입력 이동을 재현했다. RatHost 루트는 약 `(-3.31, 0.06, -4.55)` 이동했고, RatHost-RatVisual 수평 최대 거리는 `0`, 카메라 목표 오차는 `0`이었다.
- 재측정한 화면 x 범위는 RatHost 루트 `0.9916px`, RatVisual 피벗 `0.9916px`, 스프라이트 bounds 중심 `0.9916px`로 서로 일치했다. 이전 픽셀 누적 이탈처럼 이동 시간에 비례해 시각 자식이 벌어지지 않았다.
- 직접 제어 분기 대조는 무입력 본능 `(0,0,1) / 0.45`, 본능 휴지 `(0,0,0) / 0`, WASD 우측 `(1,0,0) / 1`로 통과했다. Play 세션 Console Error/Warning은 0건이었다.
- MCP 동적 명령에서는 Unity `Time.time`이 자연 진행되지 않아 시간 기반 랜덤 휴지·회전 주기는 연속 실시간으로 관찰하지 못했다. 휴지 분기는 회귀 테스트와 직접 모델 호출로 확인했고, 경계 방향 전환은 실제 컨트롤러 호출 중 확인했다.

## 2026-07-24 KST — 숙주 본능 이동·WASD 인계 독립 QA

- QA/검증 에이전트가 구현 주체의 사전검증과 분리해 `RatHostPrototype` Play를 다시 실행했다.
- 무입력 360스텝에서 raw input은 계속 0이었고, 360/360스텝 본능 이동이 발생했다. RatHost-RatVisual XZ 최대 거리는 `0`, 화면 x 차이는 `0px`, 화면 초점 오차는 최대 `0.6748px`였다.
- Unity Input System 합성 D/A/W/S를 `PrototypeKeyboardInput → RatHostController.Update → CharacterController.Move` 실제 게임 경로에 넣었다. 모든 방향에서 raw·해결·실제 이동 방향 내적이 `1`이었고, 첫 스텝은 설정과 일치하는 `0.0416~0.064`로 순간이동이 없었다.
- 키 해제 첫 스텝은 즉시 본능 0.45배 이동 거리 `0.0288`로 복귀했고, 본능 휴지 모델은 무입력 정지·입력 우선을 유지했다.
- 게임 코드 Console Error/Warning은 0건이며 Play 종료 뒤 씬은 dirty가 아니었다.
- QA 판정은 **기능 통과 — MCP 대체 입력 범위**다. 물리 OS 키, 자연 시간 랜덤 휴지 주기, EditMode 전체 Test Runner는 남은 사용자/인프라 확인 항목이다.
