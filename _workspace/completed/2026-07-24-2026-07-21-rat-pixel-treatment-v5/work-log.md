# 작업 로그

## 2026-07-21 — 작업 시작

- 사용자 판단: v4 해상도 상향만으로는 의도한 도트 게임 품질에 도달하지 않았다.
- 사용자 승인: 무안티앨리어싱, 제한 팔레트, 알파 픽셀 정리, Unity 고정 픽셀 격자와 정수 배율을 결합한 v5 시험.
- 경계: v5는 v4를 대체하지 않는 별도 시험이며 사용자 화면 확인 뒤에만 공통 기준을 교체한다.

## 2026-07-21 — Blender v5 출력 완료

- v4를 읽기 전용으로 열어 동일 카메라·조명·방향·보행·피벗·접지·root no-motion 조건의 128px 64장을 별도 `raw-renders-v5/`에 실제 렌더했다.
- Blender 단일 샘플 출력 뒤, 전체 프레임 공통 적응형 16색·이진 알파·무디더 후처리로 `renders-v5/`를 제작했다. 통계는 `pixel-statistics-v5.json`에 남겼다.
- 사용자 그래픽 참고 기준이 이후 Dave the Diver/Binding of Isaac으로 구체화되어, 16색·전면 이진 알파는 최종안이 아닌 진단 비교안으로 재분류했다. v5b는 더 풍부한 공통 팔레트·선택적 알파 규칙·픽셀 군집 정리의 별도 비교안으로 제안만 기록했고, Unity 반입은 중지한다.

## 2026-07-21 — Blender v5b 고밀도 픽셀 출력 완료

- v4 `.blend`를 다시 읽기 전용 입력으로 사용해 v5b 원본과 raw 64장을 별도 생성했다. v5a 산출물을 재가공하지 않았다.
- 공통 32-entry 팔레트에서 실제 27색을 사용하고, 알파는 0/255만 사용했다. 디더링 없이 불투명 중간 명도색으로 윤곽 계단과 표면 명도 단계를 표현했다.
- 보수적 저대비 1px 컴포넌트 정리 규칙으로 851px을 병합했으며, 강한 대비의 포인트와 노출된 얇은 선은 규칙 대상에서 제외했다.
- `renders-v5b/` 64장과 반입 계약을 핸드오프·검증 기록에 넘겼다. UnityProject는 변경하지 않았다.

## 2026-07-21 — Unity 통합 사전 점검 및 보류

- 현재 `RatHostPrototype` 씬은 기존 미저장 변경이 있는 상태(`dirty=True`)라 저장·재로드 없이 읽기 전용 점검만 했다.
- `IsometricCamera`는 직교 카메라로 화면에 직접 렌더 중이며, HUD `Canvas`는 `ScreenSpaceOverlay`다.
- 제안했던 `480×270` Point `RenderTexture` → 화면 정수 4배 `RawImage` 구성은 월드의 보간 흐림과 서브픽셀 흔들림을 줄일 수 있다. 단, 실제 화면 해상도가 1920×1080/정수 4배가 아니면 레터박스 또는 잘림 처리 규칙이 추가로 필요하고, 저해상도 월드 렌더 때문에 3D 환경의 세부 표현도 함께 거칠어진다.
- HUD는 저해상도 출력에 넣지 않고 별도 `ScreenSpaceOverlay`로 유지해야 텍스트·게이지 가독성을 보존한다. 월드 출력용 `RawImage`는 HUD보다 낮은 정렬 순서의 별도 캔버스에 둬야 한다.
- 사용자 참고 기준이 Dave the Diver/Binding of Isaac 그래픽 언어로 변경되어, 기존 v5의 `16색 이하 + 이진 알파` 방향을 최종 기준으로 고정하지 않았다. `WalkTrialV5` 반입, 카메라 targetTexture 변경, RawImage 연결은 중지하며 v1~v4 경로와 현재 씬 상태를 보존한다.

## 2026-07-21 — v5b Unity 통합

- v5b Blender 출력 `renders-v5b/` 64장을 원본과 SHA-256 대조 후 `UnityProject/Assets/_Project/Sprites/Characters/Rat/WalkTrialV5B/`에 별도 반입했다. v1~v4 및 v5a 파일은 변경·삭제하지 않았다.
- 64/64 Import를 `Sprite Single`, `128×128`, `PPU64`, Custom Pivot `(0.5,0.25)`, Point, Mipmap Off, Uncompressed, Clamp으로 적용·검사했다.
- `RatVisual`의 8방향×8프레임 배열을 `rat-walk-v5b-f01`~`f08`로 연결하고, 시각 자식 수평 픽셀 스냅을 `enablePixelSnap=true`, `PPU64`로 명시 활성화했다.
- `Assets/_Project/Settings/Rendering/RatPixelTrial960x540.renderTexture`를 `960×540`, Point, AA 1, no mipmaps으로 생성했다. `MainCamera`의 targetTexture와 별도 `WorldPixelOutputPreview` ScreenSpaceOverlay Canvas(정렬 -100)의 RawImage에 연결했다. 기존 HUD `Canvas`는 Overlay 정렬 0으로 그대로 남는다.
- 현재 씬의 기존 dirty 변경을 보호하기 위해 씬 저장·재로드는 하지 않았다.

## 2026-07-21 — 쿼터뷰 카메라 출력 픽셀 스냅 활성화

- 게임플레이 구현 담당의 `PrototypeCameraController` opt-in 구현을 확인한 뒤, 현재 dirty 씬을 저장·재로드하지 않고 `IsometricCamera`에 `enableQuarterViewOutputPixelSnap=true`, `quarterViewOutputPixelHeight=540`을 적용했다.
- 사전 점검 결과: 기본 시작 모드는 `QuarterView`; v5b `WalkTrialV5B` Import 64/64·8방향 완전 루프·RatVisual PPU64 snap·960×540 Point RT·RawImage·HUD Overlay 분리가 모두 연결된 상태다.
- Unity MCP 컴파일·연결 성공 및 Console Error/Warning 0건을 확인했다. 실제 Play 중 카메라 추적 안정성은 독립 QA 범위로 남긴다.

## 2026-07-21 — 게임플레이 구현: RatVisual 수평 픽셀 스냅

- `RatDirectionalSpriteView`에 opt-in `enablePixelSnap`과 `pixelSnapPixelsPerUnit`(기본 64)을 추가했다.
- 표시 자식의 월드 X/Z만 `1 / PPU` 격자로 반올림한다. 숙주 루트 Transform, CharacterController, 입력, 이동·충돌·면역·카메라는 읽거나 쓰지 않는다.
- Y는 접지와 `groundClearance = 0.005`가 격자 배수가 아니므로 스냅하지 않는다. 접지 높이와 발-바닥 여유를 정확히 보존한다.
- PPU가 0, NaN, Infinity이면 원위치를 유지하는 안전 fallback을 둔다. 스냅 비활성화 상태도 원위치를 유지한다.
- Dave the Diver/Binding of Isaac 참고 방향 반영: 이 기능은 팔레트 수·알파 경계·디더링을 강제하지 않는 중립 표시 옵션이다. 향후 v5b 비교안에도 같은 방식으로 재사용할 수 있다.

## 2026-07-21 — 게임플레이 구현: 쿼터뷰 카메라 출력 픽셀 스냅

- `PrototypeCameraController`에 opt-in `enableQuarterViewOutputPixelSnap`과 내부 출력 높이 `quarterViewOutputPixelHeight = 540`을 추가했다.
- 쿼터뷰 직교 카메라의 최종 출력 위치만 카메라 right/up 축에서 `2 × orthographicSize / 540` 단위로 스냅한다. 카메라 forward 깊이·회전·추적 목표는 보존한다.
- 비활성화, 비직교 카메라, 0 이하 높이, 유효하지 않은 orthographicSize, 유효하지 않은 화면 축은 원 위치 fallback이다.
- 기존 카메라 모드, RatHost 루트, 입력·이동·충돌·면역에는 변경이 없다. 씬·ProjectSettings도 변경하지 않았다.

## 2026-07-21 — 영구 그래픽 관리 기준 연결

- 사용자가 지정한 `Dave the Diver`, `The Binding of Isaac`을 그래픽 판단용 참고로만 영구 문서화했다. 픽셀 밀도·명도 깊이·실루엣·가독성의 원칙만 사용하며, 고유 자산·UI·구성은 복제하지 않는다.
- `docs/design/visual/graphics-direction-management.md`에 3D 하이브리드 2.5D 유지, 현재 `128×128 / PPU64` 비교 기준, v5b 시험안의 사용자 수용 전 확장 금지, 시각 수용 검토표를 기록했다.
- 문서 색인과 에이전트 참조 맵에 연결했다. 이 기록은 참고 게임 추가가 v5b 공통 규격 확정을 의미하지 않음을 명확히 한다.
