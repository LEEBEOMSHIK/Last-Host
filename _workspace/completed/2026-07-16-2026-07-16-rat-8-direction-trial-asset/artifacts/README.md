# 쥐 8방향 시험 에셋

이 묶음은 최종 아트가 아니라 `단일 저폴리 3D 원본 → 8방향 정지 PNG` 파이프라인을 확인하는 시험 플레이스홀더다. Unity에는 반입하지 않았다.

## 빠른 확인

- 브라우저 비교: `preview/index.html`
- 스프라이트 시트: `renders/rat-8dir-sheet.png`
- 원본 메시: `source/rat-trial-v1.obj`
- 전체 설정과 해시: `render-settings.json`

## 방향과 출력

- 셀: 투명 RGBA `64×64`
- 시트: `512×64`
- 순서: `S, SW, W, NW, N, NE, E, SE`
- 공통 루트 피벗: 좌상단 기준 `(32, 48)`
- 확대 필터: Point / nearest-neighbor

## 재현과 검증

artifacts 폴더에서 실행한다.

```powershell
python source\render_rat_8dir.py
python source\validate_outputs.py
```

렌더 스크립트는 외부 패키지 없이 같은 메시, 카메라, 조명, 팔레트와 방향 매핑을 다시 출력한다. 검증 스크립트는 PNG 형식·크기·알파·팔레트·방향 순서·시트 결합·파일 해시·OBJ 정점과 면 수를 대조한다.

## 파일 역할

| 위치 | 역할 |
| --- | --- |
| `concept/rat-concept-reference-v1.png` | imagegen 실루엣·팔레트 참고 1안. 최종 방향 프레임 아님 |
| `source/render_rat_8dir.py` | 단일 원본 정의와 결정론적 렌더러 |
| `source/rat-trial-v1.obj`, `.mtl` | 교환·검토용 저폴리 3D 원본 |
| `renders/rat-00-s.png` ~ `rat-07-se.png` | 방향별 투명 정지 PNG |
| `renders/rat-8dir-sheet.png` | 방향 순서대로 이어 붙인 시트 |
| `direction-map.csv` | 인덱스·방향·원본 회전·파일 해시 |
| `render-settings.json` | 카메라·조명·피벗·팔레트·출력 해시 |
| `preview/index.html` | 콘셉트, 8방향 4배 Point 확대, 시트 비교 |

## 미확정

실루엣·팔레트 수용, 최종 셀 크기·PPU, 걷기 프레임, 바닥 그림자·깊이 정렬, Unity Import와 런타임 통합은 후속 승인 대상이다.
