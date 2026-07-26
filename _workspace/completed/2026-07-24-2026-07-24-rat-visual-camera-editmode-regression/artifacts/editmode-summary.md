# EditMode 전체 회귀 실행 요약

## 실행

- 실행 시각: 2026-07-24 14:11 KST
- Unity: `6000.4.6f1`
- 실행 경로: 기존 Unity Editor PID `42724`의 `TestRunnerApi`
- 실행 방식: `TestMode.EditMode`, 전체 필터 1개, 동기 실행 1회
- 별도 batchmode 미실행 사유: 같은 `projectPath`를 기존 Editor가 열고 있어 프로젝트 잠금 충돌을 피했다.
- 공식 결과: `editmode-results.xml`
- 콜백 로그: `unity-editmode.log`

## 총괄 결과

| 총수 | 통과 | 실패 | 건너뜀 | 미결 | 시간 |
| ---: | ---: | ---: | ---: | ---: | ---: |
| 101 | 99 | 2 | 0 | 0 | 9.3474759초 |

- 전체 판정: `Failed(Child)`
- XML SHA-256: `90DD2993F52BD3BA6AB9A2D3627ABFD86363C88FAE1444144892C7A41D0FFDA4`
- 로그 SHA-256: `59E1D6FDF8086AF21CA457C24DBC6976EE3C65ECD0BFCA24757399F057817D63`

## 실패

### RatDirectionalSpriteView_PixelSnapKeepsHostAndGroundClearanceWhileSnappingVisualHorizontally

- 위치: `RatHostPrototypeCoreTests.cs:1794`
- 메시지: 기대 `(0.12, 0.17, -0.10)`, 실제 `(0.12, 0.17, -0.10)`이지만 정확 `Vector3` 비교에서 실패.
- 경계: 240회 누적 이탈·스냅 좌표·접지 tolerance 검사는 먼저 통과했다. 마지막 `enablePixelSnap=false` 후 부모 Transform의 world position을 exact equality로 비교하는 테스트 계약/부동소수점 정밀도 경계가 유력하다.
- 담당 후보: 게임플레이 구현 에이전트가 테스트 계약과 실제 좌표 차이를 정밀 재현해 최소 수정 판단.

### RatHostPrototypeScene_DefaultsToThirdPersonCameraController

- 위치: `RatHostPrototypeCoreTests.cs:2064`
- 메시지: 기대 `GameViewFrameCamera`, 실제 컨트롤러 부착 카메라 `IsometricCamera`.
- 경계: 씬에는 `GameViewFrameCamera`와 `IsometricCamera`가 함께 있고, 컨트롤러는 `IsometricCamera`에 부착되어 있으며 `startingHostMode: 1`은 `QuarterView`다. `FindAnyObjectByType<Camera>()`와 ThirdPerson 기본을 전제로 한 기존 씬 테스트가 현재 승인된 QuarterView/출력 카메라 구성과 불일치한다.
- 담당 후보: Unity 씬/통합 구현 에이전트가 현재 씬 의도를 보존하는 테스트 계약으로 정리할지 판정.

## 관련 축

- WASD·숙주 본능: `RatHostControlModel` 7개, `HostInstinctControlSpike` 2개, `RatHostInstinctWander` 2개 통과.
- v3 걷기·8방향: 방향 quantizer, idle 유지, 8fps walk/idle 복귀, 방향 에셋 canvas/pivot 통과.
- v4 해상도·접지: 방향 에셋 공통 canvas/pivot, 위험 trigger 접지, ground resolver 통과.
- v5b 픽셀: 카메라 output pixel snap 2개와 invalid PPU fallback 통과. RatVisual pixel snap 계약 1개 실패.
- 카메라·씬: 카메라 모드·축·즉시 추적·출력 snap·top/third-person 고정 테스트 통과. 씬 기본 카메라 테스트 1개 실패.

## 전후 경계

- 씬 SHA-256 전후 동일: `68C222F449C530B54E5319BD11D94C7E3851161906ED9C19CD6F2FC073C88F02`
- ProjectSettings SHA-256 전후 동일: `008078ADBB3A01264F4C097558F5983453A93F6254E600AB2776D269DD8201D9`
- 테스트 파일 SHA-256 전후 동일: `E800B04D963BE78D1E99C600FCBC6D8C5AAAEA4FE0B1DB9F85345BCD935BD986`
- `Builds/` 변경: 0
- 추가 Unity tracked 변경: 0. 기존 사용자 `ProjectSettings.asset` 변경만 유지.
- Editor: Edit 상태, 비컴파일, `RatHostPrototype` active·clean.
- Console Error/Warning: 0.
- 기존 Unity PID와 AssetImportWorker를 종료하거나 교체하지 않았다.

## QA 판정

`수정 필요`

전체 실패가 2개이므로 기술 게이트를 완료로 닫지 않는다. 코드·테스트·씬 수정과 MCP Play는 수행하지 않았다.
