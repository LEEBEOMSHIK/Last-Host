# 수정 후 MCP Play 점검

## 시작 상태

- 활성 씬: `RatHostPrototype`, clean
- Editor: Edit, 비컴파일, 비업데이트
- Console Error/Warning: 0

## Play 런타임

- `SessionMode`: `RatHost`
- `CameraMode` / `StartingMode`: `QuarterView` / `QuarterView`
- MainCamera: `IsometricCamera`, active, enabled, orthographic, `MainCamera` tag
- `PrototypeCameraController` Camera와 `Camera.main`: 동일
- `GameViewFrameCamera`: active, enabled, untagged, MainCamera와 별도, culling mask 0
- RatHost: active
- RatVisual: active, `West`, `rat-walk-v5b-f01-02-w`
- RatVisual ground clearance: `0.005000`
- HUD: active, enabled
- WorldPixelOutput RawImage: active, enabled
- MainCamera target texture와 RawImage texture: `RatPixelTrial960x540`, 동일
- RenderTexture: `960×540`
- 카메라 추적 표본 오차: `0.006300`
- RatVisual viewport 표본: `x=0.500005`, `y=0.437360`
- Play 중 Console Error/Warning: 0

## 종료 상태

- Stop 수행 완료
- Editor: Edit, 비컴파일, 비업데이트
- 활성 씬: `RatHostPrototype`, clean
- Console Error/Warning: 0
- 씬·ProjectSettings·테스트 파일 SHA-256: Play 전후 동일
- `Builds/` Git 변경: 0
