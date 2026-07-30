# 작업 로그

## 2026-07-30 — 작업 시작

- 사용자가 실제 RGBA 1차 에셋을 Unity에 반입하고 다음 작업을 진행하도록 승인했다.
- 기존 `RatHost2DPrototype.unity`에는 Stage2·Stage3 미커밋 변경이 있으므로 수정 대상에서 제외했다.
- 독립 `RatHost2DTechnicalSample.unity` 한 방을 반입 대상으로 선택했다.
- 실제 에셋의 `128×64` 환경 셀, `256×192` 쥐 캔버스, Point/mipmap off 전제를 입력으로 사용한다.
- PPU·내부 해상도는 후보로만 적용하고 사용자 샘플 수용 전 최종 규격으로 승격하지 않는다.

## 2026-07-30 — Unity 씬/통합 구현

- 실제 에셋 20개를 `Assets/_Project/Art/Production2D/V1/` 아래 `Environment`, `Rat`, `HUD`로 반입했다.
- 소스와 Unity 반입본 SHA-256을 대조해 `20/20 일치`를 확인했다.
- PNG 18개에 Sprite, Point, mipmap off, alpha transparency, uncompressed, PPU 128 후보를 적용했다.
- 쥐 피벗은 frame map의 normalized `(0.5, 0.208333)`을 사용했다.
- PPU 128은 최종 규격이 아니라 원본 128×64 환경 셀과 256×192 쥐를 기존 960×540 후보 화면에서 0.5 배율로 비교하기 위한 후보다.
- `RatHost2DProductionSampleSceneBuilder`를 추가해 실제 clean/worn 바닥, straight/corner 벽, water center/edge, barrel/crate/drain, 측면 3프레임 쥐와 실제 HUD를 독립 씬에 연결했다.
- `RatHost2D/Visual`을 논리 루트와 분리하고 제공된 측면 3프레임만 재생하도록 했다. 좌우는 `flipX`만 사용하며 다른 방향은 생성하지 않았다.
- 기존 기술 샘플 메뉴도 새 Production2D 빌더로 연결해 플레이스홀더 씬으로 되돌아가지 않게 했다.
- Unity MCP로 씬 rebuild/save, hierarchy, Play 진입·종료, Console, 런타임 이동·카메라 대체 검증을 수행했다.
- EditMode 결과는 `42 PASS / 0 FAIL / 0 SKIP`이다.
- 1920×1080 HUD 포함 Game View 캡처를 `artifacts/game-view-production2d-v1.png`에 남겼다.
- `RatHost2DPrototype`, Stage2/Stage3 핵심 코드, `ProjectSettings.asset`의 구현 전후 SHA-256이 같음을 확인했다.
- 실제 네이티브 WASD와 사용자 체감은 독립 QA·사용자 수용 단계로 남겼다.

## 2026-07-30 — 비주얼/테크아트 반려 2건 수정

- 비주얼/테크아트 검토에서 다음 blocker를 받았다.
  1. 불투명 bar frame이 red/teal fill을 덮어 두 게이지가 빈 회색으로 보임
  2. 월드가 화면 중앙 상단에 작게 떠 하단 검은 여백이 과도함
- HUD 계층을 `frame → inset fill → label` sibling 순서로 바꿨다.
  - health red와 immune teal이 frame 내부에서 실제로 보인다.
  - fill은 frame 테두리 안쪽 `200×26` 영역을 유지해 테두리를 가리지 않는다.
  - label은 마지막 sibling으로 유지해 두 색 위에서 읽힌다.
- PPU `128`, 표시 배율 `0.5`, orthographic size `4.21875`는 변경하지 않았다.
- 방 바닥을 `13×9`에서 `23×17` 셀 범위로 확장했다.
- 방 경계를 X `±10.15`, Y `±5.15`로 확장하고 수로를 10셀로 늘렸다.
- back/occlusion 벽 배치를 확장된 방에 맞게 재프레이밍했다.
- 화면 확대·블러·PPU 변경 없이 월드가 1920×1080 카메라 뷰 대부분을 채우고, 이동 중 검은 바닥 노출 여유가 줄어들도록 했다.
- Unity 씬을 다시 rebuild/save했다.
- V2 검증:
  - Unity 컴파일 통과
  - EditMode `42 PASS / 0 FAIL / 0 SKIP`
  - MCP Play 진입·종료 통과
  - 직접 상태 전환 대체 검증: X `+0.72`, Y 편차 `0`, 카메라 오차 `0.16px`
  - Console Error/Warning `0`
  - scene dirty `false`
  - 보호 대상 SHA-256 불변
- V1 캡처는 이력으로 보존하고 새 사용자 확인본을 `artifacts/game-view-production2d-v2.png`로 남겼다.
- 구현 수정 완료, 비주얼/테크아트와 QA 재검증 대기 상태다.

## 2026-07-30 — 예상 밖 Physics2DSettings 자동 직렬화 발견

- 최종 diff 대조에서 작업 시작 시 clean이던 `UnityProject/ProjectSettings/Physics2DSettings.asset` 변경을 발견했다.
- 변경 내용은 실제 기획·물리 설정 의도 변경이 아니라 Unity가 구형 직렬화 구조를 현재 구조로 자동 마이그레이션한 diff다.
  - `serializedVersion: 4 → 11`
  - `m_VelocityThreshold → m_BounceThreshold`
  - `m_AutoSimulation → m_SimulationMode`
  - simulation layer/sub-step/contact/gizmo 직렬화 필드 추가·재구성
- 이번 Production2D 독립 샘플의 승인 범위와 무관하므로 해당 파일 한 개만 HEAD 내용으로 원복한다.
- 다른 Unity 파일과 기존 사용자 변경은 복구하지 않는다.
- `git restore --source=HEAD -- UnityProject/ProjectSettings/Physics2DSettings.asset`로 해당 파일 한 개만 원복했다.
- `git diff --exit-code -- UnityProject/ProjectSettings/Physics2DSettings.asset` 결과 `0`으로 HEAD 대비 clean을 확인했다.
- 다른 파일은 복구하거나 수정하지 않았다.
