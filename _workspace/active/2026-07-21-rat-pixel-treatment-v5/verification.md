# 검증 기록

## 작업 ID

`2026-07-21-rat-pixel-treatment-v5`

## 검증 대상

v5의 16색 이하·무안티앨리어싱·이진 알파 PNG, PPU64 Unity Import, 480×270 Point 정수 배율 표시, 시각 픽셀 스냅, 정지·이동·8방향·쿼터뷰 Play.

## 현재 상태

v5b 고밀도 픽셀 처리 출력의 Unity 반입·시험 화면 연결을 마쳤다. 독립 QA Play와 사용자 시각 확인 대기다.

## 실행한 사전 점검

- Unity MCP `RunCommand`로 현재 씬을 저장·재로드 없이 검사했다.
- 결과: `RatHostPrototype.unity`, `dirty=True`; `IsometricCamera`는 직교 및 화면 직접 출력; HUD `Canvas`는 `ScreenSpaceOverlay` 1개.
- v5b PNG 64장을 별도 `WalkTrialV5B`로 SHA-256 대조 반입했다.
- Unity MCP Import·연결 검증: 64/64가 `128×128`, Sprite Single, PPU64, Custom Pivot `(0.5,0.25)` (실제 sprite pivot `64,32`), Point, Mipmap Off, Uncompressed, Clamp을 충족했다.
- `RatDirectionalSpriteView`: complete cycle `True`; south 배열의 `f01=rat-walk-v5b-f01-00-s`, `f04=rat-walk-v5b-f04-00-s`; 시각 자식 pixel snap `True/64`.
- 출력 계약: `RatPixelTrial960x540`은 960×540, Point, AA 1, no mipmaps; MainCamera targetTexture 및 `WorldPixelOutputPreview` RawImage 연결 확인; 출력 Canvas 정렬 -100, HUD Overlay 정렬 0 유지.
- 쿼터뷰 카메라 출력 스냅: `IsometricCamera`의 `enableQuarterViewOutputPixelSnap=True`, `quarterViewOutputPixelHeight=540`을 활성화했다. 기본 시작 모드는 `QuarterView`이며, 스냅은 직교 쿼터뷰 출력 위치만 대상으로 한다.
- MCP 명령 컴파일 성공. Unity Console Error/Warning 0건.

## 검증하지 못한 항목

- 독립 QA Play에서 v5b 팔레트·이진 알파·중간 명도 픽셀 군집의 실제 화면 품질 확인
- 1920×1080 정확한 2배 출력 및 비정수 화면의 레터박스/필러박스 처리 확인
- 정지·이동·대각선·카메라 추적 중 픽셀 안정성
- 독립 QA Play에서 카메라 출력 픽셀 스냅이 실제 추적 중 서브픽셀 떨림을 줄이고 추적/회전/게임플레이를 훼손하지 않는지

## 완료 판단

통합 사전 검증 통과. 독립 QA Play와 사용자 화면 확인 전에는 완료로 판단하지 않는다.

## 게임플레이 구현 자체 검증 — RatVisual 수평 픽셀 스냅

검증 대상:

- PPU64에서 `RatVisual` 표시 자식만 수평(X/Z) `1/64` 월드 격자로 스냅하고, 숙주 루트와 접지 여유를 보존하는지.

실행한 검증:

- Unity MCP `RunCommand` 컴파일·스모크: `(0.1234, 0.1725, -0.0987)`을 PPU64로 처리해 `(0.125, 0.1725, -0.09375)`를 확인했고, PPU 0에서는 원위치 fallback을 확인했다.
- 새 EditMode 계약 테스트 직접 실행: `RatDirectionalSpriteView_PixelSnapKeepsHostAndGroundClearanceWhileSnappingVisualHorizontally`, `RatVisualPixelSnapper_InvalidPixelsPerUnitUsesOriginalPosition` 2/2 통과.
- Unity Console Error/Warning 조회: 0건.

결과:

- 스냅 활성화 시 RatHost 루트 위치는 유지되고, RatVisual X/Z만 스냅된다.
- RatVisual Y와 `CurrentGroundClearance = 0.005`는 접지값 그대로 유지된다.
- 비활성화 또는 0/NaN/Infinity PPU는 시각 위치를 변경하지 않는다.
- 팔레트·알파 처리와 독립적인 중립 옵션이므로 v5b 비교안에서도 재사용할 수 있다.

검증하지 못한 항목:

- 씬 반입·480×270 정수 배율 출력·실제 v5/v5b 스프라이트를 사용한 Play 검증은 통합/QA 범위이며 새 아트 방향 확정 전 중지 상태다.

완료 판단:

- 게임플레이 구현 담당 범위 완료. 독립 QA·통합·총괄 판정 대기.

## 독립 QA Play 검증 — v5b 고밀도 픽셀 처리

### 검증 대상

`renders-v5b`/`WalkTrialV5B` 64장, 32색·이진 알파·무디더 처리, v5b 8방향 전환, 시각/카메라 픽셀 스냅, `960×540` Point 출력과 HUD 계층.

### 실행한 검증

- 원본 `renders-v5b`와 Unity `WalkTrialV5B`를 독립 SHA-256로 대조했다. 각각 64장이고 64/64 일치했으며 원본 규격은 64/64 `128×128`이었다.
- 픽셀 통계 파일과 실제 후처리 스크립트를 대조했다. 세트 공통 팔레트 제한은 32색, 프레임 최대 실사용 색은 27색, 중간 알파 합계는 0, 디더링은 `Image.Dither.NONE`으로 명시돼 있다. 통계상 고립 저대비 픽셀 병합은 851px이다.
- dirty 활성 씬을 저장·재로드하지 않고 Unity MCP Play → Pause → Stop을 수행했다.
- Play 런타임에서 v5b 64장을 전수 검사했다. `128×128`, Sprite Single, PPU64, Custom Pivot `(0.5,0.25)`, Point, 밉맵 없음, Uncompressed, Clamp, NPOT None의 Importer 위반은 0/64였다.
- `RatVisual`의 8방향 배열은 모두 방향별 v5b `f01→f08`을 참조했다. 카메라 기준 8방향마다 정지 `f01` → 이동 frame index 3 `f04` → 같은 방향 `f01`을 재현했고 실패는 0/8이었다.
- `RatVisual.enablePixelSnap=True`, PPU64이며 수평 격자 오차는 `0.000000`이었다. 접지 surface는 True, clearance는 `0.0050`으로 유지됐다.
- `IsometricCamera`는 QuarterView 직교이며 `enableQuarterViewOutputPixelSnap=True`, 출력 높이 540이었다. `2 × orthographicSize / 540 = 0.019259` world unit/pixel 기준 화면 평면 스냅 잔차는 표본에서 `0.000004~0.000011`이었다.
- `RatPixelTrial960x540`는 `960×540`, Point, AA 1, no mipmaps이고 MainCamera targetTexture에 연결됐다. 같은 RT를 쓰는 RawImage의 Canvas order는 -100, HUD ScreenSpaceOverlay Canvas 1개의 order는 0으로 HUD가 출력 앞에 있음을 확인했다.
- Pause 상태 MainCamera Capture에서 정수 배율용 쿼터뷰 픽셀 화면과 v5b의 불투명 색상 군집·계단 윤곽을 확인했다. Point RT/스냅 설정과 캡처상 보간 흐림은 보이지 않았다.
- 런타임 RatHost 위치를 QA 전용으로 이동한 뒤 `ApplyCameraNow(PrototypeGameMode.RatHost)` 경로를 실행해 카메라가 `(0.13, 0.04, -0.09)`만큼 따라오고 스냅 잔차 `0.000011`을 유지하는 것을 확인했다. Play 중 Console Error/Warning은 0건이었다.

### 도구 유발 예외

- QA 중 인자 없는 `ApplyCameraNow()`를 한 번 강제 호출했을 때 `PrototypeCameraController.ResolveMode()`의 session 참조에서 NullReference가 발생했다. 이는 자동 LateUpdate/실제 사용자 입력 경로가 아닌 QA 도구의 강제 호출에서만 발생했고 Unity Console에는 남지 않았다. 인자를 명시한 `ApplyCameraNow(PrototypeGameMode.RatHost)` 추적 경로는 성공했다. 이 예외는 통과 근거에 포함하지 않고, 향후 수동 카메라 호출 API의 별도 위험으로 남긴다.

### 검증하지 못한 항목

- 실제 키보드 WASD 장시간 연속 조작 중의 사용자가 느끼는 흐림·떨림·가독성.
- 게임 창의 OS 최종 합성 단계에서 1920×1080 고정 2배 및 비정수 해상도 레터박스/필러박스 동작. MainCamera/RT/RawImage 계약과 Camera Capture까지만 확인했다.
- 대상 EditMode TestRunner 전체 실행.

### 완료 판단

**v5b Unity 통합·픽셀 처리 회귀 범위의 독립 QA 통과.** 사용자 시각 수용 전에는 v4 공통 기준을 v5b로 교체하거나 작업 전체 완료·커밋 완료로 주장하지 않는다.

## 게임플레이 구현 자체 검증 — 쿼터뷰 카메라 출력 픽셀 스냅

검증 대상:

- 쿼터뷰 직교 카메라의 출력 위치만 내부 RenderTexture 높이 `540` 기준의 화면 right/up 격자로 스냅하고, 추적·회전·forward 깊이·게임플레이 루트를 보존하는지.

실행한 검증:

- Unity MCP 컴파일과 새 EditMode 계약 테스트 직접 실행: `PrototypeCameraController_QuarterViewOutputPixelSnapOnlySnapsCameraScreenPlane`, `PrototypeCameraOutputPixelSnapper_UsesOriginalPositionForNonOrthographicOrInvalidHeight` 2/2 통과.
- 간격 `2 × 5.2 / 540`과 forward 깊이 보존, 쿼터뷰 이외/무효 높이 fallback, 옵션 비활성 시 원래 추적 위치를 테스트로 대조했다.
- Unity Console Error/Warning 조회: 0건.

결과:

- `enableQuarterViewOutputPixelSnap`은 기본 비활성이고, 쿼터뷰 직교 카메라에서만 final output position의 screen right/up 축을 반올림한다.
- 카메라 회전·forward 깊이·추적 대상과 RatHost root·입력·이동·충돌은 변경하지 않는다.
- 비활성, 비직교, 무효 출력 높이 또는 무효 orthographicSize는 원 위치 fallback이다.

검증하지 못한 항목:

- v5b `960×540` Point RenderTexture 실제 연결과 플레이 중 카메라 추적의 시각적 안정성은 Unity 통합·독립 QA 범위다.

완료 판단:

- 게임플레이 구현 담당 범위 완료. 독립 QA·통합·총괄 판정 대기.
# Blender v5 픽셀 처리 검증 — 2026-07-21

## 대상과 경계

- 읽기 전용 입력: `2026-07-21-character-sprite-resolution-standard/source/rat-walk-trial-v4-128px.blend`
- 별도 출력: `source/rat-walk-trial-v5-pixel-treatment.blend`, `renders-v5/`, `walk-frame-map-v5.csv`, `render-settings-walk-v5.json`, `pixel-statistics-v5.json`
- UnityProject 및 v1~v4 원본·PNG·경로는 변경하지 않았다. Unity 반입은 조정자 재지시 전 중지한다.

## 실제 Blender 렌더 및 규격

- Blender 5.1.2에서 8프레임×8방향의 64장을 실제 렌더했다. 원본과 최종 PNG 모두 64장, 모든 캔버스는 RGBA `128×128`이다.
- v4의 `RatWalkCamera`, `RatWalkKeyLight`, 8방향 yaw, shape key 보행, 8 fps 및 f08→f01 루프를 보존했다. 프레임 맵은 `f01-00-s`부터 `f08-07-se`까지 64행이다.
- root 위치는 `(0,0,0)`, scale은 `(1,1,1)`, root transform fcurve는 `0`이고, 8개 보행 키의 최소 발바닥 Z는 `0.0`이다.
- Eevee `taa_samples=1`, `taa_render_samples=1`, reconstruction filter `0.01`로 원본 렌더의 다중 샘플 완화를 낮췄다.

## v5 진단 처리 결과 (최종 비주얼 기준 아님)

- 최종 PNG는 전체 64장에 공통으로 계산한 적응형 16색 팔레트를 적용했다. 투명 제외 최대 색 수는 `16`, 전체 세트의 불투명 색 수도 `16`이다.
- 알파는 원본 coverage를 임계값 128에서 `0/255`로 이진화했다. 중간 알파 픽셀은 `0`이며, 디더링은 사용하지 않았다.
- 위 수치는 `pixel-statistics-v5.json`의 프레임별 실제 PNG 검사 결과다.
- **판정: 통과(요청된 16색·이진 알파 진단 기준).** 단, 이후 사용자가 Dave the Diver/Binding of Isaac의 그래픽 특성을 참고 기준으로 명시했으므로, 이 16색·전면 이진 알파 출력은 공통 제작 기준이나 최종 방향으로 확정하지 않는다.

## v5b 후속 비교안 제안

직접 모사가 아니라 다음의 고수준 특성을 비교한다: 정교한 픽셀 군집, 의도적으로 선택된 계단 윤곽·명도 단계, 강한 실루엣과 대비.

- 팔레트: 프레임 전체에 공유하는 `24~32색`으로 늘려 몸통의 명도 단계와 눈·발·꼬리 분리를 보존한다. 색은 프레임별 자동 팔레트가 아니라 세트 공통 고정 팔레트로 관리한다.
- 알파: 외곽은 여전히 binary alpha로 픽셀 경계를 잠그되, 얇은 꼬리·발가락·특수 효과처럼 1px가 소실되는 요소만 사전 승인된 선택적 coverage 규칙을 시험한다. 전면 반투명 안티앨리어싱은 사용하지 않는다.
- 픽셀 군집: 현재 128px 3D 렌더를 그대로 확대하지 않고, 후처리에서 고립 1px 색을 인접 군집의 명도색으로 합치고 실루엣을 작은 계단 단위로 정리할 수 있다. 이 단계는 자동 생성 결과를 프레임별 점검해야 하므로 별도 비교 산출물로 제작한다.
- 구현 가능성: 현재 v5의 Blender 단일 샘플 출력과 Pillow 후처리 파이프라인은 `PALETTE_LIMIT`, 팔레트 고정, 알파 마스크 규칙, 군집 정리 단계를 분리할 수 있어 v5b를 별도 경로로 안전하게 추가할 수 있다. 사용자 확인 전 v5/v4를 덮어쓰지 않는다.

## Blender v5b 고밀도 픽셀 처리 검증 — 2026-07-21

### 산출물

- 읽기 전용 입력: `2026-07-21-character-sprite-resolution-standard/source/rat-walk-trial-v4-128px.blend`
- 별도 v5b 원본: `source/rat-walk-trial-v5b-high-density-pixel.blend`
- 재현 스크립트: `source/create_rat_walk_asset_v5b.py`, `source/postprocess_rat_walk_v5b.py`
- 최종 반입 후보: `renders-v5b/`의 64 PNG
- 추적 자료: `raw-renders-v5b/`, `walk-frame-map-v5b.csv`, `render-settings-walk-v5b.json`, `pixel-statistics-v5b.json`

### 실제 출력·픽셀 통계

- Blender 5.1.2가 v4를 읽기 전용으로 열어 8프레임×8방향의 raw 64장을 실제 렌더했다. 최종 `renders-v5b/`도 64장이고 전부 RGBA `128×128`이다.
- Eevee TAA viewport/render sample은 모두 `1`, reconstruction filter는 `0.01`이다. 이후 픽셀 처리는 디더링을 사용하지 않았다.
- 하나의 세트 공통 32-entry 팔레트를 만든 뒤 출력했다. 최종 PNG의 프레임별 불투명 색은 21~27색, 전체 실사용 불투명 색은 27색으로, 최대 32색 조건에 통과한다. 27개의 서로 다른 명도 단계는 약 `7.8~150.7` 범위다.
- 투명 알파는 임계값 128에서 `0/255`로만 기록됐다. 64장 중간 알파 픽셀 합계는 `0`이다. 계단 윤곽의 중간 표현은 반투명이 아니라 이 공통 팔레트의 불투명 중간 명도색으로만 이뤄진다.
- 고립 픽셀 정리는 단일 색상 컴포넌트가 5개 이상 불투명 이웃 안에 있고, 그중 4개 이상이 같은 인접색이며 색 차가 작은 경우에만 적용했다. 어둡거나 밝은 대비 포인트 및 노출된 1px 선은 보존한다. 실제 병합 수는 64장 합계 `851` 픽셀이다.

### 3D 원본 계약 검증

- v4의 `RatWalkCamera`, `RatWalkKeyLight`, 8방향 yaw, shape-key 보행, 8 fps, f08→f01 및 128px 캔버스를 보존했다. 프레임 맵은 `f01-00-s`부터 `f08-07-se`까지 64행이다.
- root 위치 `(0,0,0)`, scale `(1,1,1)`, root transform fcurve `0`을 확인했다. 각 보행 키의 최소 발바닥 Z는 `0.0`이므로 접지 기준도 보존한다.
- **판정: Blender v5b 산출물 통과.** 이는 특정 참고작의 캐릭터·자산을 복제한 결과가 아니라, 일반적인 고밀도 픽셀 군집·명도 단계·강한 실루엣 대비만을 적용한 비교용 쥐 스프라이트 세트다.

### Unity 반입 인계

- 반입 원본은 `renders-v5b/rat-walk-v5b-f01-00-s.png` … `rat-walk-v5b-f08-07-se.png` 64장이다. v1~v4 및 v5a 경로를 덮어쓰지 말고 `WalkTrialV5B` 같은 별도 Unity 경로를 사용한다.
- 적용 계약: Sprite Single, PPU `64`, Custom Pivot `(0.5, 0.25)`, Point, Mipmap Off, Uncompressed, Clamp. `128 / 64 = 2.0` Unity unit 캔버스 폭을 유지한다.
- `walk-frame-map-v5b.csv`의 0~7 방향 배열마다 f01→f08 순서로 연결하고, 정지에는 같은 방향의 v5b f01을 사용한다. Unity 반입 후 PNG SHA 대조, Importer 64/64, 배열 8×8, 960×540 Point 출력·2배 표시 Play 검증은 통합/QA 범위다.
