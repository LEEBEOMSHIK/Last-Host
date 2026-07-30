# 실제 게임 에셋 1차 매니페스트

## 상태

- 제작 단계: 실제 RGBA 에셋 1차 제작 완료, 독립 비주얼·QA 대기
- 품질 기준: 승인된 2026-07-30 고품질 제작 마스터
- Unity 반입: 범위 밖
- 규격: 1차 후보이며 사용자 수용과 Unity 샘플 전까지 최종 확정하지 않음

## 공통 규격

| 항목 | 후보 값 |
| --- | --- |
| 래스터 | PNG RGBA |
| 픽셀 필터 전제 | Point |
| 밉맵 전제 | Off |
| 환경 셀 | 128×64 |
| 쥐 공통 캔버스 | 256×192 |
| 쥐 방향 | side_right |
| 쥐 프레임 | neutral, contact, passing |
| 피벗 표기 | bottom-left 픽셀 좌표와 normalized 좌표 |

## 환경 파일

| ID | 예정 경로 | 크기 | 알파 |
| --- | --- | --- | --- |
| floor_clean | `game-assets/environment/floor_clean_128x64.png` | 128×64 | RGBA |
| floor_worn | `game-assets/environment/floor_worn_128x64.png` | 128×64 | RGBA |
| wall_straight | `game-assets/environment/wall_straight_160x160.png` | 160×160 | RGBA |
| wall_corner | `game-assets/environment/wall_corner_192x160.png` | 192×160 | RGBA |
| water_center | `game-assets/environment/water_center_128x64.png` | 128×64 | RGBA |
| water_edge | `game-assets/environment/water_edge_128x96.png` | 128×96 | 128×64 footprint + raised curb |
| prop_barrel | `game-assets/environment/prop_barrel_96x112.png` | 96×112 | RGBA |
| prop_crate | `game-assets/environment/prop_crate_112x112.png` | 112×112 | RGBA |
| prop_drain | `game-assets/environment/prop_drain_128x80.png` | 128×80 | RGBA |

## 쥐 파일

| ID | 예정 경로 | 크기 | 접지·피벗 |
| --- | --- | --- | --- |
| side_neutral | `game-assets/rat/rat_side_neutral_256x192.png` | 256×192 | groundline top y=152, pivot BL (128,40) |
| side_contact | `game-assets/rat/rat_side_contact_256x192.png` | 256×192 | groundline top y=152, pivot BL (128,40) |
| side_passing | `game-assets/rat/rat_side_passing_256x192.png` | 256×192 | groundline top y=152, pivot BL (128,40) |
| sheet | `game-assets/rat/rat_side_walk_3f_sheet.png` | 768×192 | 좌→우 neutral/contact/passing |
| frame map | `game-assets/rat/rat_side_walk_3f_frame-map.json` | JSON | 프레임별 동일 pivot |

## HUD 파일

| ID | 예정 경로 | 크기 | 알파 |
| --- | --- | --- | --- |
| rat_portrait | `game-assets/hud/hud_rat_portrait_184.png` | 184×184 | RGBA |
| portrait_frame | `game-assets/hud/hud_portrait_frame_256.png` | 256×256 | RGBA |
| bar_frame | `game-assets/hud/hud_bar_frame_512x80.png` | 512×80 | RGBA |
| health_fill | `game-assets/hud/hud_health_fill_400x52.png` | 400×52 | RGBA |
| immune_fill | `game-assets/hud/hud_immune_fill_400x52.png` | 400×52 | RGBA |

HUD 조립 좌표는 `game-assets/hud/hud_module-layout.json`에 기록했다.

## 프리뷰

- `previews/environment_repeat_checker.png`
- `previews/environment_room_preview.png`
- `previews/rat_actual_size.png`
- `previews/rat_50_percent.png`
- `previews/rat_2x.png`
- `previews/hud_states.png`
- `previews/master_asset_comparison.png`

## 제작·검증 결과

- source board: 환경, 소품, 쥐, HUD 4개
- 실제 게임 에셋 파일: 20개
  - PNG 18개
  - JSON 2개
- 자동 검사: `128/128 PASS`
- 재생성: `20/20 SHA-256 일치`
- 환경 반복: clean/worn/water 각각 visible component 1, hole 0
- 쥐 알파 bbox:
  - neutral `(9,78)-(247,152)`
  - contact `(9,76)-(247,152)`
  - passing `(9,79)-(247,152)`
- 쥐 체형 편차:
  - 폭 `238/238/238`
  - 높이 `74/76/73`, 최대/최소 비율 1.041
- 크로마 잔류: 실제 PNG 18개 모두 magenta 0, green 0
- Unity Import·Play: 현재 작업 범위 밖

## 품질 경계

- source master는 이미지 생성 분리 소스이며 최종 에셋과 다른 경로에 둔다.
- 실제 에셋은 크로마 제거 뒤 공통 캔버스·접지·피벗·반복 경계·불필요 픽셀을 다시 정리한다.
- 단순 도형 또는 저밀도 재드로잉으로 대체하지 않는다.
- 승인 마스터와 직접 비교해 품질 등급이 명백히 낮아지면 기술 검증 결과와 무관하게 반려한다.
