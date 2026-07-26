# 프로젝트 총괄 관리자 검토

## 작업 ID

`2026-07-24-rat-visual-camera-editmode-regression`

## 검토 대상

- 전체 EditMode 초기 실행·실패 분류와 수정 후 독립 재실행 기록
- `RatHostPrototypeCoreTests.cs` 단일 파일의 테스트 계약 수정 diff
- 수정 후 NUnit XML·로그·MCP Play 요약과 SHA-256
- 작업 패킷, `CURRENT.md`, 공유 상태판, Git 변경 경계
- v3·v4·v5b·카메라 관련 기술 잔여와 사용자 수용 경계

## 판정

**내부 승인 가능**

이번 판정은 자동 기술 회귀 게이트를 완료로 보고할 수 있다는 뜻이다. 사용자 WASD 체감, v4/v5b 화면 수용, 자연 경계도 Windows 성공 루프를 완료했다는 뜻은 아니다.

## 근거

- 초기 전체 EditMode는 `101 total / 99 passed / 2 failed`였고, 실패를 숨기지 않고 부동소수점 exact 비교와 구형 ThirdPerson·임의 Camera 계약으로 분리했다.
- 담당 구현 에이전트는 프로덕션 코드·씬·ProjectSettings를 건드리지 않고 `RatHostPrototypeCoreTests.cs` 한 파일만 수정했다.
- pixel snap 비활성 검증은 X/Y/Z 원위치 보존을 계속 단언하면서 기존 테스트의 다른 좌표 검증과 동일한 `0.0001f` 허용오차를 적용했다. 기능 요구를 제거한 수정이 아니다.
- 카메라 테스트는 임의 `Camera` 검색과 구형 ThirdPerson 기본을 제거하고 현재 승인·저장된 `QuarterView MainCamera + 별도 untagged GameViewFrameCamera` 계약을 명시적으로 검증한다.
- 집중 재실행 `2/2 PASS` 뒤 독립 전체 EditMode 재실행이 `101/101 PASS`, 실패·skip·inconclusive `0`, `6.3731956s`로 통과했다.
- 수정 후 XML SHA-256 `FB20ABC4DE772EBD5605691A87E6E9C53DC4F1A2460D7B5C2983F146D336F105`와 로그 SHA-256 `7C87D72222FC29DBC34348C8CEA9D819F5EBF8824432EE930827544010635933`이 QA 기록과 실제 artifact에 일치한다.

## QA/검증 기록 확인

- QA 최종 판정: `완료 가능 — 자동 기술 게이트`
- 전체 TestRunner: `101/101`, 실패·skip·inconclusive 0
- 수정된 두 테스트가 최종 XML에서 각각 `Passed`로 확인된다.
- v3 방향·idle·walk, WASD·숙주 본능, v5b RatVisual·카메라 pixel snap, 카메라 모드·QuarterView·씬 기본 계약의 기존 자동 회귀가 통과했다.
- 씬·ProjectSettings·Builds는 이번 구현·검증에서 변경하지 않았고, 수정 대상 테스트 파일도 최종 QA 실행 전후 같은 SHA-256을 유지했다.
- `git diff --check`는 테스트 diff와 작업 패킷 범위에서 통과했다.

## MCP 플레이 체크 확인

- QA가 `RatHostPrototype` Play에서 RatHost 모드, QuarterView `IsometricCamera` MainCamera, 별도 `GameViewFrameCamera`, RatVisual, HUD, `WorldPixelOutput`, `RatPixelTrial960x540` 연결을 확인했다.
- RenderTexture는 `960×540`, RatVisual ground clearance는 `0.005000`, 카메라 추적 표본 오차는 `0.006300`, viewport x는 `0.500005`였다.
- Play 중 Console Error/Warning 0, Stop 후 Edit·비컴파일·비업데이트, 활성 씬 clean, Console Error/Warning 0으로 복귀했다.
- 총괄 관리자는 MCP Play를 재실행하지 않고 독립 QA 기록의 충분성만 확인했다.

## v3·v4·v5b 기술 게이트 경계

- v3: 기존 방향·idle·walk 자동 회귀의 전체 TestRunner 잔여는 해소됐다. 사용자 실제 WASD 체감은 별도 유지한다.
- v5b: RatVisual·카메라 pixel snap과 960×540 출력 연결의 자동·MCP 기술 잔여는 해소됐다. 사용자 화면 수용은 별도 유지한다.
- v4: 전체 TestRunner 실행 잔여는 해소됐고 공통 canvas/pivot·씬 가시성·접지·ground resolver는 통과했다. 그러나 `128×128 / PPU64 / world width 2` 자체를 직접 명명해 검사하는 EditMode 테스트는 없다.
- 따라서 v4의 정확 규격 자동화 공백은 남은 위험으로 유지하며, 선행 MCP 증거가 있다는 이유로 직접 EditMode 테스트 통과라고 표현하지 않는다.
- 관련 v3/v4/v5b active 작업은 이번 기술 게이트만으로 자동 완료·보관하지 않는다. 각 작업의 사용자 체감·시각 수용과 남은 총괄 판정을 별도로 처리한다.

## 범위·상태판 확인

- 수정 범위는 승인된 쥐 프로토타입의 테스트 계약 한 파일이며 새 패키지·아트·씬·ProjectSettings·Builds 변경은 없다.
- `UnityProject/ProjectSettings/ProjectSettings.asset`의 기존 `APP_UI_EDITOR_ONLY` 변경과 `_workspace/previews/`는 범위 밖으로 유지한다.
- 공유 상태판과 `CURRENT.md`는 본 작업을 QA 완료 가능·총괄 판정 대기로, v4 직접 자동화 공백과 사용자 수용을 별도로, 자연 경계도 작업을 active·QA `차단`·총괄 `보류`로 기록한다.
- 자연 경계도 재개 조건은 Computer Use 게임 창 캡처 복구 또는 사용자 동일 세션 화면·`Player.log` 제공으로 유지한다.

## 완료·보관 가능 여부

- 자동 기술 게이트 완료 보고: 가능
- 본 작업 완료 보관: 가능
- 조건: 조정자가 이 총괄 판정과 완료 보고를 `CURRENT.md`·공유 상태판에 동기화하고, active→completed 이동 뒤 실제 완료 경로와 후보 중복을 QA 대조한다.
- 관련 v3/v4/v5b 작업의 일괄 완료 보관: 불가. 사용자 WASD·시각 수용 및 각 작업 경계를 별도 유지한다.
- 자연 경계도 작업 완료 보관: 불가

## 남은 위험

- v4 `128×128 / PPU64 / world width 2` 직접 EditMode 자동화 공백
- 물리 키보드 기반 WASD 체감
- v4·v5b의 사용자 최종 시각 수용
- 자연 경계도 엄격 Windows 성공 루프 차단

## 다음 단계

1. 조정자가 총괄 `내부 승인 가능`과 완료 가능 경계를 상태판·`CURRENT.md`에 반영한다.
2. 본 작업을 completed 폴더로 보관하고 QA가 완료 경로·후보·Git 상태를 대조한다.
3. v3/v4/v5b는 사용자 시각 수용과 개별 남은 판정으로 진행한다.
4. 자연 경계도 엄격 검증은 기존 재개 조건 충족 전까지 차단 상태를 유지한다.
