# 작업 로그

## 작업 ID

`2026-07-21-character-sprite-resolution-standard`

## 2026-07-21 — 작업 시작

- 사용자 요청: 현재 쥐 스프라이트 해상도를 상향하고, 이후 오브젝트에도 동일한 해상도 기준을 적용할 수 있게 한다.
- 판단: 64×64/PPU32와 동일한 월드 표시 크기를 보존하려면 128×128/PPU64가 최소 상향 시험 기준이다.
- 게이트: 새 v4 출력, Unity Import·씬 연결, 문서 기준 추가, 독립 QA, 총괄 판정이 필요하다.

## 2026-07-21 — 비주얼/테크아트 기준 검토

- 승인 기준: 프레임당 `128×128` RGBA PNG와 Unity PPU `64`의 조합은 `128 / 64 = 2` Unity unit으로, 기존 `64 / 32 = 2` Unity unit 캔버스 폭을 보존한다. 따라서 기존 쥐 v3의 월드 표시 크기를 바꾸지 않는 올바른 상향 기준이다.
- 권장 범위: 이 값은 프리렌더 캐릭터와 전경 게임플레이 오브젝트의 공통 출발점으로 사용한다. Point, 밉맵 끔, 무압축, Clamp, Sprite Single, Custom Pivot `(0.5, 0.25)`은 같은 캔버스·접지 구성을 가진 대상에 적용한다.
- 예외: 비행체·부유물·긴 소품은 실제 접촉점에 맞춘 피벗을 별도 기록한다. 더 큰 캔버스가 필요한 대상은 PPU만 임의 변경하지 않고 정수쌍과 월드 폭 보존을 비교·승인한다. UI·원거리 환경 텍스처에는 강제하지 않는다.
- QA 수용 기준: 실제 128px 렌더 여부, 전체 Import 규격, 2 unit 월드 폭, 정지·이동·8방향 전환의 동일 세트 유지, 목표 화면에서 흐림·떨림·콘솔 오류·경고 없음, 사용자 시각 확인을 모두 기록해야 한다.
- 문서 반영: `docs/design/visual/pixel-lowpoly-3d-production-guide.md`에 공통 기준·예외·QA 수용 기준을 추가했다. 에셋·씬·코드는 변경하지 않았다.

## 2026-07-21 — Blender v4 128px 출력 완료

- 읽기 전용 입력: `2026-07-20-rat-walk-animation-blender/artifacts/source/rat-walk-trial-v3-exaggerated-motion.blend`.
- 신규 산출물: `source/rat-walk-trial-v4-128px.blend`, `source/create_rat_walk_asset_v4.py`, `renders-v4/` 64장, `walk-frame-map-v4.csv`, `render-settings-walk-v4.json`.
- v3에서 바꾼 것은 출력 캔버스뿐이다. 카메라(ORTHO, scale `7.111111`), 키 라이트(SUN, energy `2.0`), 8방향 yaw, 8프레임 위상, 메시 보행 키, 루트 피벗은 보존했다.
- 실제 Blender 5.1.2 렌더로 `128×128` RGBA PNG 64장을 생성했다. 규격·프레임 맵(8방향×8프레임)·접지(각 키 최소 Z `0`)·루트 위치/스케일·root fcurve `0`·`f08 → f01` 루프 계약을 확인했다.
- Unity 인계: `PPU 64`, Custom Pivot `(0.5, 0.25)`(128px top-left 좌표 `(64, 96)`)를 사용한다. 따라서 128px/PPU64는 기존 64px/PPU32와 같은 2 Unity-unit 캔버스 폭을 유지한다.
- Alpha 경계 상자도 v3의 약 2배 픽셀 범위로 대조되어 동일 카메라 화면 점유율을 유지한다. 픽셀 래스터화에 따른 1px 차이는 있다.

## 2026-07-21 — Unity v4 반입·활성 씬 연결

- Blender v4 출력 `renders-v4/`의 128×128 PNG 64장을 `UnityProject/Assets/_Project/Sprites/Characters/Rat/WalkTrialV4/` 별도 경로로 반입했다. TrialV1, WalkTrialV2, WalkTrialV3와 기존 Blender 원본은 수정·삭제하지 않았다.
- 원본·반입본은 64/64 파일 수와 SHA-256이 일치했고, 반입본 64/64이 128×128임을 확인했다.
- Unity MCP에서 64/64을 Sprite Single, PPU 64, Custom Pivot `(0.5, 0.25)`, Point, 밉맵 없음, Uncompressed, Clamp, NPOT None으로 재임포트했다. `RatVisual/RatDirectionalSpriteView`의 8개 방향 배열에는 각 방향의 v4 `f01→f08` 8장을 연결하고 8fps를 유지했다.
- 대표 v4 Sprite의 Unity 캔버스 폭은 `128px / PPU64 = 2.000 Unity units`로 확인되어 기존 `64px / PPU32 = 2` units와 일치한다.
- 활성 씬은 dirty 상태이므로 씬 Save·재로드·Play는 수행하지 않았다. 연결은 현재 활성 `RatVisual` 인스턴스에만 적용됐으며 실제 화면·8방향·접지·콘솔 검증은 QA 담당에 인계한다.
