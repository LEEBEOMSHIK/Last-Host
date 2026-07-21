# 핸드오프

## 현재 상태

- 상태: 차단 — Blender는 열려 있으나 Windows 앱 조작 연결이 native pipe `os error 2`로 실패한다.
- 사용자 승인: 쥐 걷기 애니메이션 Blender 시험 제작.
- 비주얼 기준: `visual-review.md`의 8프레임 공통 위상, 고정 8방향 출력, `Z=0` 발 접지 기준.

## 변경한 파일

- 작업 패킷과 비주얼 검토 기록만 생성했다.
- Blender·Unity·기존 정지 8방향 자산은 변경하지 않았다.

## 다음 작업

1. Windows 앱 조작 연결 복구 후 Blender 창을 읽기 전용으로 확인한다.
2. 저장 상태와 원본 경로가 안전하면 별도 경로에 시험 `.blend`와 걷기 프레임을 만든다.

## 사용자 결정 필요

- 연결 복구 전에는 열린 Blender의 원본·저장 상태를 안전하게 확인할 수 없다.

## 사용자 요청 재시도 — 2026-07-20

- Computer Use bootstrap→`list_apps`, 2초 후 재시도, 세션 초기화 후 재-bootstrap→`list_apps`가 다시 모두 native pipe `os error 2`로 실패했다.
- Blender·Unity·기존 정지 자산·걷기 출력은 열거나 수정하지 않았다. 상태는 QA `차단`·총괄 `보류`로 유지한다.
- 재개 조건은 helper `list_apps` 정상 응답 또는 사용자가 저장 Blender 원본 경로를 제공하는 것이다.

## 재개 후 인계

- 생성물은 `artifacts/`에 있으며 Unity 반입은 하지 않았다.
- 남은 수정: W(02) 정지 PNG 바닥 bbox `y=50`과 걷기 `y=49`의 1px 차이 처리 후 QA·총괄 재검토.
- 주의: S/SW/SE 일부 중간 프레임은 가림 때문에 동일 픽셀이 있어, 수용 전 사용자 시각 검토를 권장한다.

## v3 과장 보행 인계 — 2026-07-20

- 상태: **Blender v3 시험 산출 완료, 사용자 시각 검토 대기**. Unity 반입·정지 자산 교체·완료 판정은 이 작업에 포함하지 않는다.
- 입력: 읽기 전용 `artifacts/source/rat-walk-trial-v2.blend`와 `create_rat_walk_asset_v2.py`.
- 산출물:
  - `artifacts/source/rat-walk-trial-v3-exaggerated-motion.blend`
  - `artifacts/source/create_rat_walk_asset_v3.py`
  - `artifacts/renders-v3/` — 64 PNG (8방향 × 8프레임)
  - `artifacts/walk-frame-map-v3.csv`
  - `artifacts/render-settings-walk-v3.json`
- v3 변경: 쥐의 표시 크기, 카메라, 피벗, 조명은 바꾸지 않았다. 스윙 발만 더 크게 전진·들리고, 메시 단위의 짧은 몸통 리듬과 절제된 꼬리 반대 흔들림을 추가했다.
- 검증: 실제 Blender 렌더 64장 모두 64×64, 접지 최저 Z=0, root location/scale `(0,0,0)/(1,1,1)`, root fcurve 0, f08→f01 전환을 포함한 W 방향 프레임별 화면 변화 201~229픽셀을 확인했다. 카메라·조명 값은 v2와 일치한다.
- 남은 위험: 64px와 쿼터뷰 가림에서 모든 방향의 발이 동등하게 눈에 띄는지는 Unity 게임 카메라의 실제 재생으로 최종 판단해야 한다. 사용자의 v3 시각 확인 후에만 Unity 반입 작업을 별도 배정한다.
