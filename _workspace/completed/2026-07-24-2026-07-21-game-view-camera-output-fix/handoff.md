# Game 뷰 카메라 출력 복구 인계

## 최신 사용자 요청

픽셀 스냅 누적 이탈 수정 뒤 이동 문제가 해결된 것으로 확인했으므로, 진단 중 제거했던 무입력 숙주 본능 랜덤 이동을 안전 조건을 유지해 다시 반영하고 회귀 여부를 확인한다.

## 현재 상태와 다음 세션의 시작점

- 픽셀 시험 커밋의 `RatVisual` 월드 스냅이 자식 Transform의 로컬 오프셋을 매 프레임 누적시키는 원인을 확정했다. 0.72유닛 부모 이동에서 로컬 오프셋 `-0.72`, 화면 약 34px 이탈이 재현됐다.
- `RefreshPixelSnap()`의 기준을 항상 RatHost 루트로 변경했고, 저장된 씬에서는 중복 `RatVisual.enablePixelSnap`을 껐다. 960×540 Point RT와 카메라 출력 스냅은 유지했다.
- 수정 후 240fps 상당 좌→우 전환에서 수평 RatHost-RatVisual 최대 거리 `0`, 화면 x 범위 약 `0.924px`, Console Error/Warning 0건이었다.
- 무입력 숙주 본능 이동을 0.45배 속도로 복구했다. 본능 휴지 중에는 정지하고, WASD 입력 시에는 랜덤 본능을 섞지 않고 입력 방향을 그대로 사용한다.
- MCP 실제 컨트롤러 300회 호출에서 자동 이동 중 RatHost-RatVisual 수평 거리 `0`, 카메라 목표 오차 `0`, 루트·시각·bounds 화면 x 범위 각각 약 `0.992px`, Console Error/Warning 0건이었다.
- 독립 QA가 무입력 360스텝과 Input System 합성 D/A/W/S를 실제 입력 읽기·컨트롤러·CharacterController 경로로 재검증했다. 네 방향의 raw/해결/실제 이동 방향 내적은 모두 `1`, 첫 스텝은 정상 이동량 `0.0416~0.064`, RatHost-RatVisual XZ 최대 거리는 `0`, 화면 초점 오차는 최대 `0.6748px`였다.
- 독립 QA 기능 판정은 **통과 — MCP 대체 입력 범위**다. 현재 재현에서는 추가 카메라 추적·픽셀 격자 보정을 열 근거가 없다.
- 프로젝트 총괄 재판정은 **내부 승인 가능**이다. 물리 키보드·자연 시간 랜덤 리듬·전체 EditMode Test Runner는 사용자 체감 또는 후속 보강 항목이며 이번 기능 QA 보고를 막지 않는다고 판단했다.
- Windows Computer Use는 Unity 창 캡처 오류 `SetIsBorderRequired 0x80004002`가 복구 1회에도 반복돼 실제 OS WASD 자동 입력은 미검증이다.
- 물리 OS 키 입력, 자연 시간 랜덤 휴지 주기, EditMode 전체 Test Runner는 미검증이다. 작업은 사용자 최종 종료·보관 결정 대기다.

## 최신 증적

- `_workspace/active/2026-07-21-game-view-camera-output-fix/evidence/actual-direction-recheck-2026-07-23/play-confirmed.png`: Play 활성 상태.
- 같은 폴더의 `before.png`, `d-070s.png`, `wd-070s.png`: D·W+D 실제 화면 이동.
- 같은 폴더의 `release-025s.png`, `release-125s.png`: 키 해제 뒤에도 화면·HUD가 달라지는 재현 증적.

## 다음 세션 실행 순서

1. 사용자에게 QA 통과와 총괄 내부 승인 가능 판정을 보고한다.
2. 사용자 결정에 따라 현재 작업을 종료·보관한다.
3. 사용자가 원하면 물리 키보드에서 무입력 랜덤 리듬과 WASD 인계 체감을 추가 확인한다.

## 확인된 원인

- `IsometricCamera`는 활성화되어 있으며 정상 캡처된다.
- 카메라 출력 대상이 `RatPixelTrial960x540.renderTexture`뿐이라 Display 1에 직접 쓰는 카메라가 없다.
- `WorldPixelOutputPreview`의 RawImage는 RT를 참조하지만, 직접 Game 뷰 출력 카메라가 없어 Unity가 `No cameras rendering`을 표시한다.

## 변경 경계

- 유지: 3D 쿼터뷰, 쥐 8방향 스프라이트, 960×540 Point 월드 출력, HUD 별도 Overlay.
- 금지: 승인된 WASD 방향 분리·무입력 숙주 본능 복구 외 이동·충돌·면역·변이 변경, ProjectSettings·패키지 변경, v5b 공통 기준 승격.

## 구현 후 확인

1. Unity 씬/통합 완료: `GameViewFrameCamera`가 Display 1에 렌더 대상 없이 프레임만 제공한다. `IsometricCamera`는 960×540 Point RT를 계속 렌더하며 MainCamera 태그를 유지한다.
2. Unity MCP Play 자체 점검: Display 프레임 카메라·RT RawImage·HUD 연결 정상, 런타임 쥐 위치 이동에 대해 쿼터뷰 카메라 추적 확인, Console Error/Warning 0건.
3. QA가 Play Game 뷰에서 `No cameras rendering`이 사라지고 실제 월드·HUD가 보이는지, WASD 연속 이동·픽셀 보간/떨림을 독립 확인한다.

## 변경 경계 확인

- RenderTexture는 제거하지 않았고, ProjectSettings·패키지·이동·충돌·면역·변이와 v5b 공통 기준은 변경하지 않았다.
- 기존 dirty 씬을 저장·재로드하지 않았다. 커밋 전 QA와 총괄 판정이 필요하다.

## 저장 상태

- `RatHostPrototype.unity`에 `IsometricCamera.quarterViewFocusHeight=0.9`를 저장했고 active scene `isDirty=False`다. 이 값은 지면 기준 RatHost 루트가 아닌 화면상 RatVisual 중심을 쿼터뷰 중심에 맞춘다.
- ProjectSettings의 `APP_UI_EDITOR_ONLY` 자동 변경은 이 작업 범위 밖이다. 저장·수정·되돌림을 하지 않았으며 별도 정리 판정이 필요하다.
- `_workspace/previews/`도 범위 밖이다. 이 두 경로와 카메라 수치·픽셀 규격·게임플레이 코드는 원인 판정 전 건드리지 않는다.
