# 독립 QA 검증 기록

## 작업 ID

`2026-07-21-game-view-camera-output-fix`

## 검증 대상

`Display 1` Game 뷰의 직접 프레임 카메라와 기존 `IsometricCamera → 960×540 Point RenderTexture → RawImage → HUD` 출력 구조의 공존.

## 실행한 검증

- 기존 dirty 활성 씬을 저장·재로드하지 않고 Unity MCP Play → Stop을 수행했다.
- `GameViewFrameCamera`를 런타임에서 독립 조회했다. 활성 상태, `targetTexture=null`, `targetDisplay=0`(Display 1), `cullingMask=0`, 검은 `SolidColor` clear를 모두 확인했다.
- `IsometricCamera`는 활성 `MainCamera`이며 `RatPixelTrial960x540` targetTexture를 계속 참조했다. 런타임 카메라는 총 2개였다.
- `WorldPixelOutput` RawImage는 활성 상태로 `RatPixelTrial960x540`를 참조하고, 부모 `WorldPixelOutputPreview` Canvas의 order는 -100이었다. 별도 HUD ScreenSpaceOverlay Canvas 1개는 order 0이므로 HUD가 월드 출력 앞에 유지된다.
- Play 해상도는 `1920×1080`이었다. 런타임 RatHost를 QA 전용으로 `(0.137, 0, -0.089)` 이동한 뒤 `ApplyCameraNow(PrototypeGameMode.RatHost)`를 호출해 IsometricCamera가 `(0.13, 0.04, -0.10)` 이동하고 QuarterView 직교를 유지하는 것을 확인했다.
- IsometricCamera Capture에서 실제 쿼터뷰 월드/쥐 화면이 렌더되는 것을 확인했다. Unity MCP의 Camera Capture는 Game 뷰 UI·Overlay RawImage 자체를 캡처하지 않으므로, `No cameras rendering` 문구의 직접 이미지 증적은 제공하지 못했다.
- Play 중 Unity Console Error/Warning은 0건이었다.

## 결과

- **통과:** Display 1에 직접 쓰는 활성 `GameViewFrameCamera`가 존재하므로, 기존처럼 Display 1에 카메라가 전혀 없는 구조는 해소됐다.
- **통과:** IsometricCamera의 960×540 RT 출력, 활성 RawImage 표시, HUD 전면 계층, 1920×1080 Play 해상도, 쿼터뷰 카메라 추적, 콘솔 상태를 유지했다.

## 검증하지 못한 항목

- Unity Game 뷰 창 자체의 `No cameras rendering` 문구가 사라진 화면을 MCP로 직접 캡처. 대체로 Display 1 활성 프레임 카메라와 RawImage/HUD 런타임 연결을 확인했다.
- 실제 키보드 WASD 장시간 이동에서의 사용자 체감과 최종 Game 뷰 시각 확인.

## 완료 판단

**출력 경로 독립 QA는 대체 MCP 근거로 통과.** Game 뷰 UI 직접 캡처와 사용자 시각 확인은 잔여이며, 이 기록은 작업 완료·커밋 판단이 아니다.

## 재검증 — 연속 카메라 추적·픽셀 격자 불일치 분석 (2026-07-23)

### 재검증 경계

이전의 `RatHost` 위치 1회 변경 뒤 `ApplyCameraNow` 호출은 연속 추적 검증 근거에서 제외했다. 코드·씬·ProjectSettings를 저장하거나 변경하지 않고 Play 런타임과 현재 수치만 사용했다.

### 확인한 구현 경로

- `LateUpdate`는 `ApplyCamera(ResolveMode(), Time.deltaTime, snap:false)`를 호출한다.
- QuarterView에서 `MoveCamera`는 `t = 1 - exp(-followSharpness × deltaTime)`의 `Vector3.Lerp`/`Quaternion.Slerp`를 적용한 뒤 `RefreshQuarterViewOutputPixelSnap`을 적용한다.
- 현재 값은 `followSharpness=18`, visual PPU64 격자 `1/64 = 0.015625` world unit, camera 출력 격자 `2 × 5.2 / 540 = 0.019259` world unit이다. 두 격자의 비율은 `0.811298`로 정수배가 아니다.

### 실행한 재검증

- Play에서 임시 런타임 `MonoBehaviour` 16프레임 프로브와 `EditorApplication.update` 콜백 프로브를 각각 시도했다. 동적 `RunCommand` 어셈블리의 Unity 생명주기/Editor 콜백이 다음 프레임에서 실행되지 않아, 두 프로브 모두 실제 프레임 샘플을 남기지 못했다. 런타임 임시 오브젝트는 Play 종료로 소멸했고 저장된 변경은 없다.
- Unity MCP 정책은 비공개 `ApplyCamera` 반사 호출을 차단했다.
- 대체로, 현재 런타임의 쿼터뷰 카메라·RatHost·RatVisual 값에서 `LateUpdate` 안의 동일한 `Lerp + Slerp + PrototypeCameraOutputPixelSnapper.SnapPosition` 식을 60fps `Δt=1/60`, 16회, root step `(0.031, 0, -0.019)`으로 재현했다. 이는 실제 PlayerLoop 입력 재현은 아니지만, 스무딩과 출력 스냅 수식 자체의 정량 분석이다.
- 수식 재현 결과: 프레임 보간 계수 `t=0.259182`, 최대 world lag `0.097343`, 화면 평면 최대 lag `4.781` internal-output pixel, visual pivot의 nearest output-pixel 분수 잔차 `0.159..0.491`, camera output grid 잔차 최대 `0.000023`이었다.
- Play 중 Console Error/Warning은 0건이었다.

### 분석 결과

- **스무딩 지연 가능성:** `followSharpness=18`은 60fps에서 한 프레임마다 목표-현재 거리의 약 25.9%만 보정한다. 위 연속 step 시나리오에서 카메라는 최대 약 4.8 internal pixel 뒤처졌으므로, 지속 이동 시 쥐와 카메라가 서로 늦게 따라오는 인상은 현재 수식만으로도 발생할 수 있다. 이는 버그라기보다 현재 `Lerp` 설정의 예상 결과다.
- **격자 위상 불일치 가능성:** visual은 X/Z를 `1/64` 격자에 맞추지만 camera output은 `10.4/540` 격자에 맞춘다. 둘은 정수배가 아니므로 visual pivot가 output pixel의 정수 좌표에 계속 고정되지 않는다(재현 잔차 최대 0.491 pixel). Point RT라도 이동 중 단계적 흔들림/상대 위상 변화가 보일 수 있는 조건이다.
- **판정:** 사용자 보고와 양립하는 유력 원인은 (1) 의도된 카메라 스무딩의 누적 지연, (2) visual 64 PPU와 540-high output snap의 비정수 격자 조합이다. 이 기록은 어느 하나를 단정하거나 수정안을 승인하지 않는다.

### 미검증·남은 위험

- 실제 키보드 WASD가 `RatHostController.Update`에서 만드는 속도·방향 전환·충돌을 포함한 장시간 PlayerLoop 샘플은 MCP 입력 재현 제한으로 측정하지 못했다.
- 임시 프레임 콜백 프로브가 동적 MCP 환경에서 실행되지 않았으므로, 수치는 실제 프레임 로그가 아니라 현재 코드 수식의 동일 조건 재현값이다.
- Game 뷰 최종 합성 화면과 사용자 체감은 여전히 직접 확인이 필요하다. 따라서 이전의 출력 경로 QA 통과를 움직임 일치 통과로 확대 해석하지 않는다.

### 재검증 판단

**추적 일치 판정 보류.** 출력 경로 자체는 별개로 유지되지만, 연속 이동에서의 카메라 지연과 격자 위상 불일치가 사용자 증상에 기여할 수 있는 수치 근거가 확인됐다.

## 실제 OS W 입력 캡처 독립 검토 — 2026-07-23

### 입력 증적과 관찰

MCP 전송이 끊긴 상태에서 남긴 Unity Editor 전용 캡처 세 장만을 독립 검토했다. 추가 입력을 보내거나 런타임 값을 수정하지 않았다.

| 시점 | 캡처 | HUD/화면 관찰 |
| --- | --- | --- |
| 입력 전 (10:46:38) | `evidence/actual-os-input-2026-07-23/before-w.png` | Game 뷰가 1920x1080으로 정상 렌더된다. HUD는 `오염 노출 +21.28`, 숙주 생명력 `93/100`, 면역 경계도는 약 31%로 읽힌다. |
| W 1초 (10:46:39) | `evidence/actual-os-input-2026-07-23/w-1s.png` | HUD가 `강제 조종 +14`로 바뀌고 면역 경계도는 약 35%다. 쥐와 주변 바닥/초록 발판의 화면 위치가 입력 전과 달라진다. |
| W 2초 (10:46:40) | `evidence/actual-os-input-2026-07-23/w-2s.png` | `강제 조종 +20`, 면역 경계도 약 41%로 계속 증가한다. 쥐는 계속 렌더되고 배경/발판 구성이 다시 이동한다. 생명력은 세 장 모두 `93/100`이다. |

세 장 모두 `No cameras rendering` 없이 Game 뷰가 출력된다. W를 2초간 유지한 실제 OS 입력은 게임에 수신됐고, 강제 조종 HUD·면역 경계도·쥐/월드 및 카메라 출력이 연속으로 변한 증거가 있다.

### 판정

| 검증 항목 | 판정 | 근거/한계 |
| --- | --- | --- |
| 실제 W 입력 수신 및 강제 조종 반영 | **통과** | `강제 조종 +14 → +20`과 면역 경계도 증가가 1초 간격 캡처에 남았다. |
| 쥐와 월드/카메라 출력의 연속 이동 | **통과** | 쥐가 매 캡처에 유지되고 주변 지형 및 화면 구성이 W 입력 시간에 따라 변한다. |
| 쥐-카메라 상대 위치의 정밀 정합(지연/흔들림 없음) | **보류** | 1초 간격 정지 캡처뿐이고 Game 뷰 우측이 Project Settings 창에 가려져 있다. 원시 쥐/카메라 좌표나 프레임 연속 영상이 없어 이전의 보간 지연·격자 불일치 우려를 반증하거나 픽셀 단위로 측정할 수 없다. |
| W 해제 후 정지·idle 안정화 | **미검증** | 키 해제 뒤의 정지 캡처가 없다. |
| D 및 W+D 방향/대각 이동 | **미검증** | 해당 캡처는 생성되지 않았으며 존재한다고 가정하지 않았다. |
| 이번 실제 OS 입력 세션의 Console 오류/경고 | **미검증** | MCP가 끊겨 Console을 재확인할 수 없었다. 이전 세션의 Console 0건은 이번 입력 세션의 증거로 재사용하지 않는다. |

**작업 전체 결론: 보류.** 실제 W 입력과 Game 뷰 출력 경로는 통과했지만, 사용자가 보고한 쥐-카메라 상대 흔들림/지연과 정지 전환은 아직 독립적으로 통과시킬 근거가 없다.

## 실제 D/W+D 입력 뒤 정지 안정화 캡처 독립 검토 — 2026-07-23

### 입력 증적과 대조

Unity PID 42724 전경 상태에서 D 0.7초와 W+D 0.7초 OS 키 이벤트를 순서대로 전송하고 모두 해제한 뒤, 해제 후 0.25초와 1.25초에 만든 Unity 창 캡처를 검토했다. 방향 이동 중의 직접 캡처는 생성되지 않았다.

| 시점 | 캡처 | 독립 확인 |
| --- | --- | --- |
| 키 해제 후 0.25초 | `evidence/actual-os-input-2026-07-23/stop-025s.png` | HUD가 `하수도 탐색 중`이고 쥐·월드·HUD가 표시된다. |
| 키 해제 후 1.25초 | `evidence/actual-os-input-2026-07-23/stop-125s.png` | 첫 캡처와 화면 전체가 동일하다. |

- 두 파일의 길이는 모두 243,067 bytes이며 SHA-256은 모두 `B0C4E5DB145309781799774A239F8462B050D45A136014E8AF30E6205C77D33B`다.
- 따라서 최소 0.25초부터 1.25초까지의 1초 구간은 Unity 창의 Game 뷰 합성 결과와 HUD를 포함해 픽셀 단위로 변하지 않았다. 이 판정은 캡처 파일 자체와 해시를 재계산해 확인한 결과다.

### 판정 갱신

| 검증 항목 | 판정 | 근거/한계 |
| --- | --- | --- |
| 키 해제 후 표시 정지·idle 안정화 | **통과** | 해제 후 0.25초와 1.25초 캡처의 SHA-256 및 파일 크기가 완전히 동일하고, HUD가 `하수도 탐색 중`으로 안정 상태를 표시한다. |
| 정지까지 걸린 정확한 시간 | **보류** | 첫 증적이 해제 0.25초 뒤이므로, 해제 직후부터 0.25초 사이의 감속/보간 여부는 측정하지 못했다. |
| D 입력 전송 | **확인** | 실제 OS 키 이벤트 전송 및 해제는 수행됐으나 이동 중 시각 캡처가 없다. 게임 내 방향 이동 결과로 확대 해석하지 않는다. |
| W+D 입력 전송 | **확인** | 실제 OS 키 이벤트 전송 및 해제는 수행됐으나 이동 중 시각 캡처가 없다. 대각 이동의 시각 결과는 보류다. |
| D/W+D 방향·대각 이동의 시각 결과 | **보류** | 해당 구간 캡처가 생성되지 않았다. |
| 이번 세션 Console 오류/경고 | **미검증** | MCP 전송 단절로 Console 상세를 재조회할 수 없다. |

**최종 QA 결론: 부분 통과, 전체 완료 판정은 보류.** W 이동 수신/출력과 키 해제 뒤 1초 정지 안정화는 통과했다. 그러나 쥐-카메라 상대 정합(이동 중 흔들림·지연 없음) 및 D/W+D의 시각 방향 결과는 증적 부족으로 보류다.
