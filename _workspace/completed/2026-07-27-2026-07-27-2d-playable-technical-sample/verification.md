# 2D 플레이어블 기술 샘플 독립 QA

## 검증 대상

- 주장: 격리된 `RatHost2DTechnicalSample`이 기존 `Host/Move` 입력으로 2D 이동하며, 타일 충돌·8방향 표시·즉시 카메라 추적·Y 정렬·HUD·Windows 단일 씬 빌드가 기술 샘플 수용 기준을 만족한다.
- 대상 씬: `Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- 검증 역할: QA/검증 에이전트
- 검증일: 2026-07-27

## 실행한 검증

### 1. 전체 EditMode 회귀

명령:

- Unity Test Runner API로 전체 EditMode 테스트를 독립 재실행

결과:

- `137 passed / 0 failed / 0 skipped / 0 inconclusive`
- 소요 시간: `8.652s`
- `E01`부터 `E11`까지 모두 `Passed`
- 기존 테스트를 포함한 전체 EditMode 회귀도 실패 없음

해석:

- 씬 계약, 이동 정규화, 반대 입력·정지, 반전 최대 스텝, 벽·수로 충돌 모델, 카메라 절대 스냅, Y 정렬, HUD·입력·레거시 보호 테스트가 자동 테스트 범위에서는 통과했다.

### 2. 씬·컴포넌트 계약

명령:

- Unity MCP로 샘플 씬 로드 후 전체 hierarchy와 런타임 컴포넌트 조회

결과:

- `TechnicalSample2D` 단일 루트, `Grid`, `FloorTilemap`, `WaterTilemap`, `BlockingTilemap` 존재
- `RatHost2D/Visual/FootPoint`, `Main Camera`, `Canvas`와 필수 HUD 텍스트 존재
- Dynamic `Rigidbody2D`, `CapsuleCollider2D`, `TechnicalSample2DInput`, `RatHost2DController` 존재
- 직교 카메라, `PixelFollowCamera2D`, Tilemap 3개, `YSortSprite2D` 3개, rat sprite 연결 확인
- 초기 상태:
  - root `(-1.00, -0.25)`
  - `LastFixedStepDelta=(0,0)`
  - facing `South`
  - camera error `(0,0) px`
  - rat sorting order `37`

해석:

- 기존 3D 씬과 분리된 2D 기술 샘플의 최소 직렬화 계약은 충족한다.
- 아키텍처 문서의 `WallVisualTilemap`과 `CompositeCollider2D`는 실제 씬에 없고 `BlockingTilemap + TilemapCollider2D`로 축소되어 있다. 사용자 요청의 “TilemapCollider2D 또는 CompositeCollider2D” 범위에는 들어가지만 아키텍처 명칭과는 차이가 있다.

### 3. MCP Play와 실제 InputAction 경로

명령:

- 씬 Play 진입
- `InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(...))`
- `Host/Move` 액션을 `InputSystem.Update()`로 처리
- 액션 값을 `RatHost2DController`의 FixedStep 경로에 전달하고 2D 물리를 `0.02s` 단위로 실행
- Transform 직접 변경은 사용하지 않음

결과:

| 입력 | 10 fixed-step 이동량 | 최대 단일 step | facing | camera error |
| --- | ---: | ---: | --- | --- |
| W | `0.6000` | `0.0600` | North | `(0.00, 0.40) px` |
| S | `0.6000` | `0.0600` | South | `(0.00, 0.00) px` |
| A | `0.6000` | `0.0600` | West | `(-0.40, 0.00) px` |
| D | `0.6000` | `0.0600` | East | `(0.00, 0.00) px` |
| W+D | `0.6000` | `0.0600` | NorthEast | `(0.15, 0.15) px` |

- 좌→우 반전:
  - 각 구간 이동량 `0.6000`
  - `West → East`
  - 반전 후 camera error `(0.15, 0.15) px`
- 상→하 반전:
  - 각 구간 이동량 `0.6000`
  - `North → South`
  - 반전 후 camera error `(0.15, 0.15) px`
- 300 fixed-step 정지:
  - drift `0.000000`
  - 마지막 step `0.000000`
  - camera error `(0.12, 0.12) px`
- 전 구간 camera error는 축별 `0.5 px` 이내였다.
- 최초의 “QueueStateEvent만 유지” 방식은 에디터의 실제 키보드 상태 갱신에 덮여 이동하지 않았다. 액션이 즉시 `(0,1)`을 읽는 것을 확인한 뒤 명시적 `InputSystem.Update`와 결정적 FixedStep 방식으로 대조했다.

해석:

- 같은 `Host/Move` 액션 경로에서 WASD, 대각선 정규화, 좌우·상하 반전, 방향 표시와 카메라 중심 오차가 수치 기준을 통과했다.
- MCP가 실제 물리 키보드를 지속 홀드하지 못해 일반 Game View 포커스 상태의 사람이 누르는 키 홀드와 완전히 동일한 방식은 아니다. 사용자 수동 조작 수용을 별도로 남긴다.

### 4. 벽·수로 충돌

명령:

- 같은 InputAction 경로로 수로 방향 220 fixed-step, 외벽 방향 360 fixed-step 이동
- `Physics2D.Distance`로 rat collider와 대상 TilemapCollider2D의 signed distance 확인

결과:

- 수로: `signedDistance=-0.000177`, `isOverlapped=true`
- 외벽: `signedDistance=-0.000177`, `isOverlapped=true`
- 두 경우 모두 이동은 경계에서 멈췄고 자동 E08 기준 `-0.001` 이상을 만족했다.

해석:

- 눈에 띄는 관통 진행은 없고 자동 테스트 허용오차 안이다.
- 다만 Physics2D API는 미세 접촉을 `overlapped`로 보고했다. “어떠한 음수 겹침도 불허”로 해석하면 조정이 필요한 잔여 위험이다.

### 5. 화면·HUD

명령:

- Unity MCP `Camera Capture`로 Play 중 Main Camera 화면 확인
- hierarchy와 `Text` 컴포넌트로 HUD 연결 및 문자열 확인

결과:

- `1920×1080` 카메라 캡처에서 아이소메트릭 바닥, 외벽, 청록 수로, rat placeholder, pipe/barrel 소품이 모두 가시적이었다.
- `SampleTitle`, `SpecText`, `ControlsText`, `RuntimeStatusText`, `PlaceholderNotice`가 활성 상태이며 텍스트가 비어 있지 않았다.
- MCP 카메라 캡처는 Screen Space Overlay Canvas를 포함하지 않는다.
- `ScreenCapture.CaptureScreenshot` 보조 시도는 에디터 Game View 파일을 생성하지 못했다.

해석:

- 월드 오브젝트 가시성은 확인했다.
- HUD의 실제 화면 배치·가독성, rat placeholder의 체감 크기와 픽셀 흔들림은 사용자 Game View 수동 확인이 필요하다.

### 6. Console

명령:

- Play 입력 검증 후 및 Windows 빌드 후 Unity Console Error/Warning 조회

결과:

- `Error 0`
- `Warning 0`

### 7. Windows 임시 빌드

명령:

- 메뉴 `Last Host/Technical Sample 2D/Build Windows Temporary`

결과:

- 성공 경로: `C:/tmp/LastHost2DTechnicalSample/20260727-085924/LastHost2DTechnicalSample.exe`
- Unity BuildReport total size: `204,767,109 bytes`
- EXE: `667,648 bytes`
- 전체 파일: `318개`, `205,360,078 bytes`
- Data 폴더: `287개`, `80,488,903 bytes`
- 저장소 안 `Builds` 경로 생성·변경 없음
- `EditorBuildSettings`에는 기존 `RatHostPrototype.unity`만 활성 상태이고 샘플 씬은 추가되지 않았다.

해석:

- 명시적 단일 씬 Windows 빌드는 성공했고 저장소 외 임시 경로에 생성되었다.
- 이 QA에서는 EXE를 별도 프로세스로 실행해 사람이 조작하는 단계까지는 수행하지 않았다.

### 8. 보호 경로·Git 대조

명령:

- `git hash-object UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- Packages, ProjectSettings, BuildSettings, 저장소 Builds 경로 diff 확인

결과:

- 기존 씬 Git blob SHA1:
  - 기대값 `3bc15837d24a32da2660576b10ec0baa4a20447a`
  - 실제값 `3bc15837d24a32da2660576b10ec0baa4a20447a`
- `manifest.json`, `packages-lock.json`: diff 없음
- `EditorBuildSettings.asset`: diff 없음
- 저장소 Builds 경로: diff 없음
- 구현 담당의 자동 설정 변경 정리 후 다음 5개 파일은 HEAD diff `0`이다.
  - `ProjectSettings/Physics2DSettings.asset`
  - `ProjectSettings/UnityConnectSettings.asset`
  - `Assets/Settings/DefaultVolumeProfile.asset`
  - `Assets/Settings/PC_RPAsset.asset`
  - `Assets/Settings/UniversalRenderPipelineGlobalSettings.asset`
- `ProjectSettings/ProjectSettings.asset`에는 기존 사용자 변경인
  `Standalone: SENTIS_ANALYTICS_ENABLED;APP_UI_EDITOR_ONLY` 한 줄만 남아 있다.

해석:

- 기존 3D 씬, Packages, BuildSettings, 저장소 Builds 비변경 조건을 통과했다.
- 보호 설정의 자동 변경은 정리되었고 기존 사용자 define 변경만 보존되었다.

### 9. 설정 정리 후 read-only 재대조

명령:

- 지정 5개 설정 파일, ProjectSettings, Packages, EditorBuildSettings, 저장소 Builds를 Git HEAD와 read-only 대조
- 기존 임시 빌드 산출물 존재·크기 재확인
- Unity Editor 상태, 활성 씬 dirty 상태, Console Error/Warning 재확인
- 자동 설정 diff 재발 방지를 위해 Windows 빌드는 다시 실행하지 않음

결과:

- 지정 5개 파일: HEAD diff `0`
- `ProjectSettings.asset`: 기존 `APP_UI_EDITOR_ONLY` define 한 줄만 diff
- Packages, EditorBuildSettings, 저장소 Builds: diff 없음
- 기존 3D 씬 Git blob SHA:
  `3bc15837d24a32da2660576b10ec0baa4a20447a`
- 기존 임시 빌드 유지:
  - EXE 존재, `667,648 bytes`
  - Data 폴더 존재, `287개 / 80,488,903 bytes`
  - 전체 `318개 / 205,360,078 bytes`
- Unity:
  - 비재생
  - 비컴파일
  - 비업데이트
  - 활성 씬 `RatHost2DTechnicalSample`
  - scene dirty `false`
- Console:
  - Error `0`
  - Warning `0`

해석:

- 이전 차단 사유였던 보호 설정 diff가 해소됐다.
- 이전 Windows 빌드 성공 근거와 산출물은 유지되며, 재빌드 없이 설정 정리 상태를 보존했다.

## 미검증 항목

- 임시 Windows EXE를 실제 실행한 키보드 조작·화면 확인
- Game View의 Screen Space HUD 실제 배치와 가독성
- 사람 조작에서의 픽셀 흔들림·체감 속도·rat 최종 수용
- 최종 아트 품질. 현재 것은 명시적 기술 플레이스홀더다.

## 남은 위험

- 벽·수로 접촉에서 `-0.000177`의 미세 음수 signed distance가 관찰됐다.
- 카메라 캡처 기준 rat placeholder가 매우 작고 단순해 최종 비주얼 수용 근거가 될 수 없다.
- MCP 결정적 입력 검증은 동일 InputAction과 컨트롤러 경로를 사용했지만 물리 키보드 장시간 홀드의 완전한 대체는 아니다.

## 사용자 확인 필요

- Screen Space HUD의 실제 Game View 배치와 가독성
- 실제 키보드 홀드·빠른 반전 시 조작감, 순간이동·카메라 분리 없음
- rat placeholder의 화면 점유율과 기술 샘플 시각 수용
- 위 항목은 기술 게이트의 완료 가능 판정과 별개인 사용자 수동 화면·조작 수용이다.

## 완료 판단

**완료 가능**

완료 판단 근거:

- 기능·자동 테스트·MCP Play 수치·Console·Windows 빌드는 기술 게이트 대부분을 통과했다.
- 보호 설정 자동 변경을 정리한 뒤 지정 파일, Packages, EditorBuildSettings와 저장소 Builds 비변경을 read-only로 확인했다.
- 기존 3D 씬 SHA와 기존 사용자 `APP_UI_EDITOR_ONLY` define 변경도 그대로 유지됐다.
- 물·벽 접촉의 `-0.000177` 미세 접촉과 Screen Space HUD·실제 화면·조작은 남은 위험 및 사용자 수동 수용 항목으로 분리한다.

---

## Addendum — 소품 충돌·우회 독립 QA 재검증

검증일: 2026-07-27

검증 범위:

- 최신 `RatHost2DTechnicalSample` 씬의 Pipe/Barrel 실제 충돌
- 막힌 뒤 직교 입력 우회와 Y-sort 전환
- 최신 코드·씬을 포함한 새 Windows 임시 빌드
- 보호 경로 재대조

### A. EditMode 재실행

명령:

- 전체 프로젝트 EditMode
- `LastHost.Prototype.TechnicalSample2D.Tests` 어셈블리 필터

결과:

- 전체 프로젝트: `139 passed / 0 failed / 0 skipped / 0 inconclusive`
  - 소요 시간 `18.684s`
- TechnicalSample2D 필터: `38 passed / 0 failed / 0 skipped / 0 inconclusive`
  - 소요 시간 `6.517s`

해석:

- 새 Pipe/Barrel 직렬화·충돌 회귀를 포함한 전체 테스트와 전용 테스트가 모두 통과했다.

### B. 소품 Collider 직렬화

샘플 씬을 새로 로드한 뒤 read-only 조회했다.

- `BoxCollider2D`는 씬 전체에서 정확히 `2개`이며 둘 다 `YSortProps` 아래에 있다.
- 두 Collider 모두 non-trigger이고 Rigidbody2D가 없어 정적 Collider로 동작한다.
- `Pipe_A`
  - size `(0.27, 0.16)`
  - offset `(0.00, 0.02)`
  - SpriteRenderer, YSortSprite2D 연결
- `Barrel_A`
  - size `(0.31, 0.14)`
  - offset `(0.00, 0.02)`
  - SpriteRenderer, YSortSprite2D 연결

### C. Pipe_A 실제 입력 접근·충돌·우회

명령:

- Play를 새로 시작해 초기 root `(-1.00, -0.25)`에서 실행
- `Host/Move`에 `QueueStateEvent → InputSystem.Update`
- A로 X 정렬 후 W로 Pipe에 접근
- 막힌 상태에서 W를 60 fixed-step 더 입력
- A로 옆 이동한 뒤 W로 위쪽을 통과
- Transform 직접 변경 없음

결과:

- 접촉·차단 위치: `(-1.36, 0.22)`
- signed distance: `+0.000625`
- 차단 뒤 60-step 법선 진행: `0.000000`
- 접촉 상태에서 소품 중심 통과: `false`
- 우회 종료: `(-2.08, 0.74)`
- 우회 이동량: `0.8876`
- 정렬 관계: `RatFront → RatBehind`
- 정렬 관계 전환 횟수: 정확히 `1회`
- 최대 실제 root step: `0.060000`
- 순간이동 판정: `0회`
- camera error 최대 축: `0.480 px`
- 사용 액션: `Host/Move`

해석:

- Pipe는 물리 경계를 관통하지 않고, 계속 미는 입력에도 멈춘다.
- 직교 입력으로 빠져나온 뒤 우회할 수 있고 발 기준 앞/뒤 정렬이 한 번만 전환된다.

### D. Barrel_A 실제 입력 접근·충돌·우회

명령:

- Play와 씬을 다시 시작해 같은 초기 root에서 독립 실행
- S로 Y 정렬 후 D로 Barrel에 접근
- 막힌 상태에서 D를 60 fixed-step 더 입력
- W로 위쪽 직교 이동 후 D로 소품 중심 X를 넘어 우회
- Transform 직접 변경 없음

결과:

- 접촉·차단 위치: `(0.72, -0.67)`
- signed distance: `+0.000625`
- 차단 뒤 60-step 법선 진행: `0.000000`
- 접촉 상태에서 소품 중심 통과: `false`
- 직교 이동 위치: `(0.72, -0.07)`
- 우회 종료: `(1.44, -0.07)`
- 소품 중심을 우회해 반대편 도달: `true`
- 우회 이동량: `0.9372`
- 우회 구간 정렬 관계: `RatFront → RatBehind`
- 우회 구간 정렬 관계 전환 횟수: 정확히 `1회`
- 최대 실제 root step: `0.060000`
- 순간이동 판정: `0회`
- camera error 최대 축: `0.480 px`
- 사용 액션: `Host/Move`

참고:

- 첫 하단 우회 시도는 외벽이 가까워 반대편까지 나가지 못했다.
- 씬을 초기화한 뒤 위쪽 우회 경로로 다시 검증해 충돌·우회·정렬 조건을 통과했다.
- 충돌 높이를 너무 얕게 맞춘 탐색 경로는 Collider 모서리를 돌아가므로 최종 충돌 증거에서 제외했다.

해석:

- Barrel도 정면 입력을 차단하고, 직교 입력으로 옆을 돌아 반대편에 도달할 수 있다.
- 우회 구간의 Y-sort 관계는 발 위치 경계를 지날 때 한 번만 바뀌었다.

### E. Play 종료·Console·씬 상태

- Play 종료 성공
- Console Error `0`
- Console Warning `0`
- 활성 씬 `RatHost2DTechnicalSample`
- scene dirty `false`
- Unity 비재생·비컴파일·비업데이트

### F. 최신 Windows 임시 빌드

명령:

- `Last Host/Technical Sample 2D/Build Windows Temporary`

결과:

- 새 run-id: `20260727-153843`
- 경로:
  `C:/tmp/LastHost2DTechnicalSample/20260727-153843/LastHost2DTechnicalSample.exe`
- BuildReport: `Succeeded`
- BuildReport total size: `204,767,541 bytes`
- EXE: `667,648 bytes`
- 전체 파일: `317개 / 204,767,541 bytes`
- Data 폴더: `287개 / 80,489,335 bytes`
- 빌드 후 Console Error/Warning: `0/0`

미검증:

- 새 EXE를 별도 프로세스로 실행한 실제 화면·키보드 플레이

### G. 보호 경로

통과:

- 기존 3D 씬 Git blob SHA:
  `3bc15837d24a32da2660576b10ec0baa4a20447a`
- `manifest.json`, `packages-lock.json`: diff 없음
- `EditorBuildSettings.asset`: diff 없음
- 저장소 Builds 경로: 생성·diff 없음

자동 발생 diff:

- `ProjectSettings/Physics2DSettings.asset`
  - QA 시작 시 이미 Unity 6 직렬화 마이그레이션 diff가 재발한 상태였다.
- `Assets/Settings/DefaultVolumeProfile.asset`
- `Assets/Settings/PC_RPAsset.asset`
- `Assets/Settings/UniversalRenderPipelineGlobalSettings.asset`
- `ProjectSettings/ProjectSettings.asset`
  - 기존 `APP_UI_EDITOR_ONLY` 외 preloaded asset과 Standalone batching 직렬화가 추가됐다.
- `ProjectSettings/UnityConnectSettings.asset`
  - `m_Enabled: 0 → 1`

QA 역할 경계:

- 위 자동 설정 diff는 수정하거나 되돌리지 않았다.
- 최신 빌드 성공 근거를 유지한 채 구현 담당이 자동 diff를 정리해야 한다.
- 정리 후 빌드를 다시 실행하면 같은 diff가 재발할 수 있으므로, 재빌드 없이 read-only 보호 경로 대조가 적절하다.

### H. 자동 diff 정리 후 최종 read-only 대조

빌드·테스트·Play를 다시 실행하지 않고 최신 증거와 정리 상태만 확인했다.

결과:

- 다음 지정 5개 파일은 HEAD diff `0`이다.
  - `ProjectSettings/Physics2DSettings.asset`
  - `ProjectSettings/UnityConnectSettings.asset`
  - `Assets/Settings/DefaultVolumeProfile.asset`
  - `Assets/Settings/PC_RPAsset.asset`
  - `Assets/Settings/UniversalRenderPipelineGlobalSettings.asset`
- `ProjectSettings/ProjectSettings.asset`에는 기존 사용자 변경인
  `Standalone: SENTIS_ANALYTICS_ENABLED;APP_UI_EDITOR_ONLY` 한 줄만 남아 있다.
- `manifest.json`, `packages-lock.json`: diff 없음
- `EditorBuildSettings.asset`: diff 없음
- 저장소 Builds 경로: 생성·diff 없음
- 기존 3D 씬 Git blob SHA:
  `3bc15837d24a32da2660576b10ec0baa4a20447a`
- 최신 build `20260727-153843` 유지:
  - EXE 존재, `667,648 bytes`
  - 전체 `317개 / 204,767,541 bytes`
  - Data 폴더 `287개 / 80,489,335 bytes`
- Unity 상태:
  - 비재생
  - 비컴파일
  - 비업데이트
  - 활성 씬 `RatHost2DTechnicalSample`
  - scene dirty `false`
- Console Error/Warning: `0/0`

해석:

- 최신 소품 충돌·우회 검증과 Windows 빌드 성공 증거를 유지하면서 보호 설정 자동 diff만 정리됐다.
- 빌드를 재실행하지 않아 같은 자동 diff를 다시 만들지 않았다.

### Addendum 판정

**완료 가능**

판정 근거:

- 최신 소품 충돌, 60-step 차단, 중심 미통과, 직교 우회, Y-sort 1회 전환, 카메라 오차, 순간이동 방지, 테스트, Console, Windows 빌드는 모두 통과했다.
- 기존 3D 씬, Packages, EditorBuildSettings와 저장소 Builds는 보호됐다.
- Windows 빌드가 자동 생성했던 Physics2D/URP/PlayerSettings diff는 정리됐고, 기존 사용자 `APP_UI_EDITOR_ONLY`만 남았다.
- 최신 build `20260727-153843`의 EXE/Data와 성공 근거도 유지된다.
- 실제 EXE 실행, Screen Space HUD와 실제 키보드 조작은 사용자 수동 수용 항목으로 계속 분리한다.
- 소품 signed distance `+0.000625`는 합격 기준 `-0.001` 이상이지만 실제 실행본에서의 장시간 접촉 체감은 사용자 확인 범위다.

### 커밋 전 상태판 대조

- 판정: **완료 가능**
- 완료 패킷의 `task.md`, `work-log.md`, `agent-activity.md`, `verification.md`, `completion-report.md`, `artifacts/`가 모두 존재하고 active 원본은 없다.
- 현황판은 기술 샘플을 진행 항목에서 제거하고 실존 완료 경로를 최근 요약에 한 번 연결했다. 다음 후보와 보류 항목은 서로 중복되지 않는다.
- `CURRENT.md`는 완료 경로, QA `완료 가능`, 총괄 `내부 승인 가능`, 사용자 커밋·푸시 승인과 선별 커밋 직전 상태를 가리킨다.
- 예상 커밋 범위는 TechnicalSample2D 전용 runtime/editor/test/art/scene, 완료 패킷, `current-task-board.md`, `CURRENT.md`다.
- `ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY`, `_workspace/previews/`, 저장소 Builds, Packages, 기존 3D 씬과 예상 밖 경로는 제외 가능하며 현재 Git 상태에 다른 필수 포함 변경은 없다.
