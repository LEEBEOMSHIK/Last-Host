# Unity 씬/통합 구현 자체 검증

## 검증 대상

- 씬: `Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- 에셋: `Assets/_Project/Art/Production2D/V1/`
- 구현: Production2D V1 Import, 환경·쥐·HUD 한 방 통합
- 검증 주체: Unity 씬/통합 구현 에이전트
- 독립 QA 여부: 아니오. 이 문서는 QA/검증 에이전트의 `verification.md`를 대체하지 않는다.

## 구현 기준

- PPU: `128` 후보
- 화면 표시: 원본 제작 크기의 `0.5` 배율 후보
- 카메라: 기존 `960×540` 후보와 같은 직교 크기 `4.21875`
- 쥐: 제공된 `side_right` 3프레임만 사용
- 좌우 전환: 제공된 측면 스프라이트의 `flipX`
- 미제작 방향: 생성·추정하지 않음
- 물리 루트와 시각 자식: `RatHost2D/Visual`로 분리

## 자동 검증

- Unity Refresh·컴파일: 통과
- TechnicalSample2D EditMode:
  - `PASS=42`
  - `FAIL=0`
  - `SKIP=0`
  - 결과 파일: `editmode-test-result.txt`
- Import:
  - PNG 18개가 `Sprite`
  - `Point`
  - mipmap off
  - alpha transparency on
  - uncompressed
  - PPU 128 후보
- 소스/Unity 반입 파일 SHA-256 대조: `20/20 일치`
- `git diff --check`: 통과

## Unity MCP Play 대조

- 씬 Play 진입·종료: 통과
- 런타임 필수 컴포넌트:
  - `Rigidbody2D`
  - `CapsuleCollider2D`
  - `RatHost2DController`
  - `RatSide3FrameView`
  - `PixelFollowCamera2D`
  - `YSortSprite2D`
- MCP 직접 상태 전환 대체 검증:
  - 시작 `(-1.00, -0.25)`
  - 12 fixed step 후 `(-0.28, -0.25)`
  - 이동량 `(0.72, 0.00)`
  - 카메라 논리 픽셀 오차 `(0.16, 0.00)`
  - 측면 프레임 `1`
  - 우측 보기 `true`
- 실제 네이티브 WASD 입력 검증: 구현 에이전트 단계에서는 미수행
- Console Error/Warning: `0`
- Play 종료 후 씬 dirty: `false`

## 화면 캡처

- `game-view-production2d-v1.png`
- 크기: `1920×1080`
- 포함:
  - clean/worn 바닥 반복
  - straight/corner 벽
  - water center/edge
  - barrel/crate/drain
  - 측면 쥐
  - 실제 portrait/health/immune HUD

## 보호 대상 대조

아래 파일은 구현 전후 SHA-256이 같다.

- `RatHost2DPrototype.unity`
  - `8B758BD5E7B47B46E13E7EA7EFD669DAF7332626AB19074818F8073222093ED6`
- `RatHost2DPrototypeSceneBuilder.cs`
  - `9C1D45D0B6CC4353ADCDBFA25E316B07DAC98E0456F8A2AB7D352C649C319135`
- `RatHost2DSessionController.cs`
  - `6462EE1B107052B494566DD69D6DA90D4E30AEA55E211874437930BE676AC081`
- `ProjectSettings.asset`
  - `008078ADBB3A01264F4C097558F5983453A93F6254E600AB2776D269DD8201D9`

## 남은 검증

- QA/검증 에이전트의 독립 EditMode·MCP Play·Console·diff 대조
- Game View 포커스가 확보된 실제 WASD 입력 또는 사용자 수동 플레이
- 쥐 접지, 벽·통·상자 앞뒤 가림, 물·벽·소품 충돌의 체감 확인
- PPU 128과 0.5 표시 후보의 사용자 수용
- 전체 8방향, 전체 타일셋, 정식 Windows 빌드는 이번 범위 밖

## 비주얼 반려 수정 V2

- 반려:
  - frame에 가려 red/teal fill이 보이지 않음
  - 방이 작아 하단 검은 여백이 과도함
- 수정:
  - HUD draw order를 `frame → fill → label`로 변경
  - PPU 128, 표시 0.5, ortho 4.21875 유지
  - 바닥 `23×17` 셀, 경계 X `±10.15`·Y `±5.15`, 수로 10셀로 확장
  - 확장된 방에 맞춰 벽 배치 재프레이밍
- 재검증:
  - EditMode `42 PASS / 0 FAIL / 0 SKIP`
  - MCP Play 진입·종료 통과
  - 직접 상태 전환 대체 검증 X `+0.72`, Y 편차 `0`, 카메라 오차 `0.16px`
  - Console Error/Warning `0`
  - scene dirty `false`
  - 보호 대상 SHA-256 불변
- 현재 확인본: `game-view-production2d-v2.png`
- 이전 확인본: `game-view-production2d-v1.png` 이력 보존
