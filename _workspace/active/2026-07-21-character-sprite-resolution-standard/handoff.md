# 핸드오프

## 최신 사용자 요청

쥐 프리렌더 해상도를 올리고 이후 오브젝트에 재사용할 동일 기준을 만든다.

## 현재 상태

- 상태: v4 Blender 출력·활성 `RatVisual` 연결·독립 QA Play 완료. 총괄 검토는 기록 불일치와 남은 사용자/목표 출력 검증 때문에 `수정 필요` 판정.
- 기본안: 128×128, PPU64, Point, 밉맵 끔, 무압축, Clamp, Custom Pivot `(0.5, 0.25)`
- 보존: 기존 64px/PPU32와 같은 월드 표시 크기, v3 카메라·조명·방향·루프·게임플레이
- 공통화: 향후 프리렌더 캐릭터·전경 게임플레이 오브젝트는 128px/PPU64를 기본으로 시작한다. 피벗 구성·캔버스가 다르면 예외 사유와 값, UI·원거리 환경이면 별도 Import 기준을 기록한다.
- Blender 산출물: `source/rat-walk-trial-v4-128px.blend`, `source/create_rat_walk_asset_v4.py`, `renders-v4/`(64 PNG), `walk-frame-map-v4.csv`, `render-settings-walk-v4.json`.
- Blender 검증: 64/64 `128×128 RGBA`, 8방향×8프레임, 접지 최소 Z `0`, root fcurve `0`, `f08 → f01`, v3와 같은 ORTHO 카메라·SUN 키 라이트. v3 입력은 변경하지 않았다.

## 다음 작업

1. `CURRENT.md`와 공유 상태판을 독립 QA 기록에 맞춰 동기화하고, QA가 상태판 대조 결과를 남긴다.
2. 목표 출력 해상도를 기록한 뒤 연속 WASD·정지·대각선 전환의 선명도와 서브픽셀 떨림을 확인한다.
3. 대상 EditMode TestRunner와 사용자 v4 시각 체감 확인 후 총괄 재검토를 받는다.

## 총괄 검토 — 2026-07-21

- 판정: **수정 필요**.
- 이유: 독립 QA Play는 통과했으나, `verification.md`의 QA 통과 상태와 `CURRENT.md`·공유 상태판·기존 핸드오프의 QA 대기 상태가 불일치한다. 또한 목표 출력 해상도/연속 WASD/TestRunner/사용자 체감 확인이 남았다.
- 상세: `director-review.md`.

## Unity v4 반입 결과 (2026-07-21)

- 반입 경로: `UnityProject/Assets/_Project/Sprites/Characters/Rat/WalkTrialV4/`.
- 64 PNG가 원본과 SHA-256 64/64 일치하고 128×128이다.
- Unity 설정 64/64: Sprite Single, PPU64, Custom Pivot `(0.5, 0.25)`, Point, 밉맵 없음, Uncompressed, Clamp, NPOT None.
- `RatVisual/RatDirectionalSpriteView`의 8방향 배열은 v4 `f01→f08`으로 연결됐으며 8fps 유지. `128 / 64 = 2.000` Unity unit으로 기존 64/32와 화면 월드 폭이 같다.
- 기존 static, TrialV1, WalkTrialV2, WalkTrialV3, 이동·충돌·카메라·ProjectSettings는 보존했다. 활성 씬은 dirty이므로 Save·재로드는 하지 않았고 Play는 QA 담당이다.
