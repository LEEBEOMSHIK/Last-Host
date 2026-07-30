# 독립 QA/검증

## 검증 대상

- 완료 주장: 승인된 실제 RGBA 환경·쥐·HUD 1차 묶음이 독립
  `RatHost2DTechnicalSample` 한 방에 손상 없이 반입되어 PPU 128 후보에서
  환경 반복, 2D 이동, 벽·수로·소품 충돌, Y 정렬·가림, 픽셀 카메라,
  HUD가 함께 동작한다.
- 대상 씬:
  `UnityProject/Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- 에셋:
  `UnityProject/Assets/_Project/Art/Production2D/V1/`
- 최종 확인본:
  `artifacts/game-view-production2d-v2.png`
- 검증 주체: QA/검증 에이전트

## 작업영역

- 이번 작업 소유:
  - Production2D V1 실제 에셋과 `.meta`
  - Production2D 기술 샘플 빌더·런타임 뷰·HUD
  - 독립 기술 샘플 씬
  - 관련 EditMode 테스트와 작업 패킷
- 보호:
  - `RatHost2DPrototype.unity`
  - Stage2·Stage3 기존 미커밋 코드·테스트·문서
  - `ProjectSettings.asset`의 기존 `APP_UI_EDITOR_ONLY`
  - `_workspace/previews/`
- Windows 정식 빌드: 이번 범위가 아니므로 실행하지 않음(`N/A`)

## 실행한 검증

### 1. 에셋 원본·Import·씬 계약

명령:

- PowerShell `Get-FileHash -Algorithm SHA256`로 제작 원본과 Unity 반입본 대조
- Unity MCP `Unity_ManageScene Load/GetActive/GetHierarchy`
- Unity MCP 읽기 전용 `Unity_RunCommand`로 TextureImporter, Sprite, 씬 구성,
  HUD 순서·상태, 바닥 범위, Collider를 대조

결과:

- 제작 원본 PNG 18개·JSON 2개와 Unity 반입본: `20/20 SHA-256 일치`
- TextureImporter: `18/18`
  - `Sprite`
  - `Single`
  - `Point`
  - mipmap off
  - alpha transparency on
  - uncompressed
  - PPU `128`
- 쥐 개별 3프레임:
  - 캔버스 `256×192`
  - pivot `(128, 40)`
  - 공통 PPU `128`
- 씬:
  - 경로 일치
  - `sceneDirty=false`
  - 단일 `TechnicalSample2D` 루트
  - Environment, Actors, Cameras, UI 활성
  - `RatHost2D/Visual`과 `FootPoint`가 논리 루트와 분리
- V2 계약:
  - FloorTilemap bounds `23×17`
  - health fill `0.90`, immune fill `0.55`
  - HUD sibling `frame → fill → label`
  - Camera orthographic size `4.21875`
- `Rigidbody2D`, `CapsuleCollider2D`, `RatHost2DController`,
  `RatSide3FrameView`, `PixelFollowCamera2D`, `YSortSprite2D`,
  `Production2DSampleHud` 존재
- RoomBoundary, BlockingWaterTilemap, wall, barrel, crate Collider가 모두
  non-trigger로 존재

해석:

- 실제 파일과 Unity Import 사이의 바이너리 손상이나 저품질 재압축은 없다.
- PPU 128은 검증 후보이며 사용자 수용 전 최종 규격으로 승격하지 않는다.

### 2. EditMode

명령:

- Unity MCP TestRunner API
  - assembly filter:
    `LastHost.Prototype.TechnicalSample2D.Tests`
  - 전체 EditMode 영향 범위

결과:

- TechnicalSample2D: `42 PASS / 0 FAIL / 0 SKIP`, `5.201s`
- 전체 EditMode: `196 PASS / 0 FAIL / 0 SKIP`, `11.424s`

해석:

- 신규 Import·씬 계약과 기존 이동·카메라·정렬 회귀를 포함한 관련 범위가
  통과했다.
- 현재 저장소의 다른 EditMode 어셈블리까지 포함한 전체 영향 범위도
  통과했다.

### 3. Unity MCP Play

명령:

- 대상 씬 Load
- Console Clear
- Play
- 런타임 hierarchy·component·HUD·camera 상태 조회
- 직접 상태와 수동 Physics2D step을 이용한 이동·충돌·Y-sort 대체 검증
- Main Camera `1920×1080` 캡처
- Error/Warning 조회
- Stop
- GetActive로 종료 후 scene dirty 확인

결과:

- Play 진입·종료: PASS
- 시작 런타임:
  - 쥐 root `(-1.00, -0.25)`
  - camera error `(0.00, 0.00) px`
  - HUD 활성
- 직접 상태 이동 대체 검증:
  - 12 fixed step
  - X `+0.72`
  - Y 편차 `0`
  - camera error `(0.16, 0.00) px`
  - 측면 3프레임 범위와 right facing 유지
- 충돌 대체 검증:
  - barrel: 요청 `1.80`, 실제 `0.16`
  - crate: 요청 `1.80`, 실제 `0.12`
  - wall: 요청 `1.80`, 실제 `0.67`
  - occupied water: 요청 `3.00`, 실제 X `0.83`
  - water 종단 penetration `-0.005`는 controller collision skin
    `1/64 = 0.015625` 안이며 수로를 관통하지 않음
- Y-sort:
  - 쥐 뒤쪽 order `25`
  - barrel order `86`
  - 쥐 앞쪽 order `125`
  - 앞뒤 관계 `25 < 86 < 125`
- 접지:
  - sprite pivot `(128,40)`
  - FootPoint local `(0,0,0)`
  - Visual local `(0,0,0)`
- V2 Main Camera 재캡처:
  - `1920×1080`
  - red health와 teal immune fill 가시
  - 23×17 월드가 화면 대부분을 채움
  - 쥐·통·상자·벽·수로가 함께 보임
- 최종 fresh Play Console Error/Warning: `0`
- Stop 후 `sceneDirty=false`

해석:

- 이동 루트, 카메라 중심, 충돌, 접지, Y-sort 계약은 런타임 상태와
  대체 step으로 확인했다.
- MCP가 Game View에 실제 물리 키를 누르는 방식은 증명하지 못했다.
  따라서 위 결과는 `MCP 직접 상태/수동 Physics2D 대체 검증`이며
  실제 네이티브 WASD 키 수신 통과라고 선언하지 않는다.

### 4. 비주얼 blocker 재대조

명령:

- `game-view-production2d-v1.png`와 `game-view-production2d-v2.png` 직접 비교
- 비주얼/테크아트 V2 재검토 기록 대조

결과:

- V1 HUD fill 미표시와 과도한 단색 배경 문제를 확인했다.
- V2:
  - strong-red `10,398px`
  - strong-teal `5,497px`
  - exact background 비율 `70.4% → 26.6%`
  - PPU 선명도, 타일 seam, 쥐 접지 유지
- 비주얼/테크아트 최종 판정:
  `v2 PASS — 사용자 확인 가능`

해석:

- V1은 반려 이력으로만 보존한다.
- 사용자에게는 V2만 확인본으로 제시해야 한다.

### 5. 보호 diff·Git·현황판

명령:

- 보호 파일 SHA-256 재계산
- `git rev-parse HEAD`
- `git rev-parse origin/main`
- `git diff --check`
- `CURRENT.md`, `current-task-board.md` 직접 대조

결과:

- 보호 파일 현재 SHA-256이 구현 전 기록과 일치:
  - `RatHost2DPrototype.unity`
    `8B758BD5E7B47B46E13E7EA7EFD669DAF7332626AB19074818F8073222093ED6`
  - `RatHost2DPrototypeSceneBuilder.cs`
    `9C1D45D0B6CC4353ADCDBFA25E316B07DAC98E0456F8A2AB7D352C649C319135`
  - `RatHost2DSessionController.cs`
    `6462EE1B107052B494566DD69D6DA90D4E30AEA55E211874437930BE676AC081`
  - `ProjectSettings.asset`
    `008078ADBB3A01264F4C097558F5983453A93F6254E600AB2776D269DD8201D9`
- `ProjectSettings.asset` diff는 기존
  `APP_UI_EDITOR_ONLY` 추가 1건 그대로다.
- 초기 독립 QA의 보호 집합에서 작업 시작 시 clean이던
  `Physics2DSettings.asset`을 누락했다.
  - 메인 조정자의 최종 Git 대조에서 Unity 자동 직렬화
    `v4 → v11` 변경이 발견됐다.
  - Unity 씬/통합 구현 에이전트가
    `UnityProject/ProjectSettings/Physics2DSettings.asset` 한 파일만
    HEAD 상태로 원복했다.
  - 원복 후 독립 재확인:
    `git diff --exit-code -- UnityProject/ProjectSettings/Physics2DSettings.asset`
    종료 코드 `0`
  - `git status --short -- UnityProject/ProjectSettings/Physics2DSettings.asset`
    출력 없음
  - 현재 소유 diff에도 `Physics2DSettings.asset`이 없다.
- HEAD와 origin/main:
  `73c575058ee73a9c4ae926d42ae77480a82e5604`
- `git diff --check`: PASS
- 현황판:
  - 현재 작업 ID·경로·최우선 후보·Git SHA는 실제 상태와 일치
  - `CURRENT.md`와 공유 현황판 모두
    `QA 완료 — 총괄 검토 대기`로 전환됨

해석:

- 이번 반입이 보호 대상 파일을 덮어쓴 흔적은 없다.
- `Physics2DSettings.asset` 보호 누락은 숨기지 않고 최종 게이트 전
  메인 발견·단독 원복·QA 독립 재확인으로 해소했다.
- 저장소는 Stage2·Stage3·아트 작업이 함께 미커밋인 dirty worktree이므로
  커밋 시 이번 작업 소유 경로를 선별해야 한다.

## MCP 플레이 체크

- 대상 씬 Load: PASS
- Play 진입: PASS
- 주요 root/HUD/camera/rat 상태: PASS
- 이동·카메라·정렬·충돌 대체 상태: PASS
- `1920×1080` Main Camera V2 캡처: PASS
- Console Error/Warning: `0`
- Stop: PASS
- Stop 후 scene dirty: `false`

## 미검증 항목

- Game View 포커스가 확보된 실제 네이티브 WASD 키 수신과 사용자 조작감
- 상자·벽 앞뒤를 사람이 연속 이동하며 보는 체감
- PPU 128, 쥐 크기, HUD 상대 크기의 사용자 최종 수용
- 전체 8방향 쥐, 전체 하수도 타일셋
- Windows 빌드(`N/A`, 이번 범위 아님)

## 남은 위험

- 쥐는 제공된 측면 3프레임과 `flipX`만 사용하므로 상하 이동 시 시각 방향은
  임시 측면으로 유지된다.
- 긴 수로에는 embankment·corner 변형이 부족해 완성 목업보다 깊이감이 낮다.
- worn floor의 대각선 반복 주기가 넓은 방에서 읽힌다.
- PPU와 화면 점유율은 사용자 수용 결과에 따라 재조정될 수 있다.
- 이번 작업 외 Stage2·Stage3 미커밋 변경이 같은 worktree에 있으므로
  커밋 선별이 필요하다.

## 완료 판단

`완료 가능 — 실제 네이티브 WASD 및 PPU 사용자 수용 대기`

## 완료 판단 근거

- 실제 에셋 SHA `20/20`, Import `18/18`, V2 씬/HUD/프레이밍 계약 통과
- TechnicalSample2D `42/42`, 전체 EditMode `196/196`
- MCP Play·카메라·충돌·Y-sort·접지·Console·Stop·sceneDirty 통과
- 비주얼 V2 PASS와 보호 SHA 불변
- 누락했던 `Physics2DSettings.asset` 자동 직렬화 변경도 최종 게이트 전
  단독 원복하고 HEAD 대비 clean을 독립 확인
- 네이티브 WASD·사용자 화면 수용·PPU 최종 승격은 완료 주장과 분리해
  남은 승인 항목으로 명시

최종 상태판 감사: `PASS` — task/CURRENT/공유 현황판 상태, active 경로,
다음 후보·보류 비중복, HEAD=origin/main `73c5750`,
`Physics2DSettings.asset` clean, completed 오표시 없음.
