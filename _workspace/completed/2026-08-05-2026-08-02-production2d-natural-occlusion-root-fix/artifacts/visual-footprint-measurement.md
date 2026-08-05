# Production2D visible footprint 정적 계측 원문

## 성격과 사용 경계

- 작성 역할: visual footprint analyst
- 작업 방식: 원본 PNG를 읽기 전용으로 정적 계측했다. Unity·테스트·MCP·빌드는 실행하지 않았다.
- alpha 기준: `alpha > 64`
- 좌표계: sprite pivot 기준 logical pixel, X는 오른쪽 양수, Y는 아래쪽 원본 이미지를 뒤집어 위쪽 양수
- 변환: `PPU 128`, 따라서 `world = logical px / 128`
- 이 문서의 exact polygon 좌표는 C2와 evidence matrix의 normative input이다. 구현 편의를 위한 AABB나 근삿값으로 대체할 수 없다.

## 캔버스·pivot·alpha 범위

| 대상 | 캔버스 | pivot(px) | `alpha > 64` root-relative 범위(px) |
| --- | --- | --- | --- |
| wall straight | `160×160` | `(80, 0)` | X `-69..67`, Y `4..153` |
| barrel | `96×112` | `(48, 0)` | X `-35..33`, Y `2..107` |
| crate | `112×112` | `(56, 0)` | X `-47..45`, Y `2..107` |
| rat neutral | `256×192` | `(128, 40)` | X `-119..118`, Y `1..73` |
| rat contact | `256×192` | `(128, 40)` | X `-119..118`, Y `1..74` |
| rat passing | `256×192` | `(128, 40)` | X `-119..118`, Y `0..72` |

## object-specific reference polygons

모든 꼭짓점은 CCW 순서다. px와 world 목록은 같은 꼭짓점을 가리킨다.

### Wall straight — 4 points

- px: `(-67,71), (54,4), (66,12), (-55,79)`
- world: `(-0.5234375,0.5546875), (0.421875,0.03125), (0.515625,0.09375), (-0.4296875,0.6171875)`
- outward face normals: `(-0.4844,-0.8748), (0.5547,-0.8321), (0.4844,0.8748), (-0.5547,0.8321)`
- QA normal set: 위 face normal 4개와 각 인접 face normal 합을 normalize한 axis-diagonal-sum 4개, 총 8개

### Barrel — 16 points

- px: `(-35,36), (-27,12), (-18,5), (-6,2), (5,2), (22,9), (27,14), (33,35), (33,36), (25,60), (16,67), (4,70), (-7,70), (-24,63), (-29,58), (-35,37)`
- world: `(-0.2734375,0.28125), (-0.2109375,0.09375), (-0.140625,0.0390625), (-0.046875,0.015625), (0.0390625,0.015625), (0.171875,0.0703125), (0.2109375,0.109375), (0.2578125,0.2734375), (0.2578125,0.28125), (0.1953125,0.46875), (0.125,0.5234375), (0.03125,0.546875), (-0.0546875,0.546875), (-0.1875,0.4921875), (-0.2265625,0.453125), (-0.2734375,0.2890625)`
- QA normal set: cardinal/diagonal 8개 `(1,0), (0.70710678,0.70710678), (0,1), (-0.70710678,0.70710678), (-1,0), (-0.70710678,-0.70710678), (0,-1), (0.70710678,-0.70710678)`

### Crate — diamond 4 points

- px: `(-47,29), (-1,2), (45,28), (-1,55)`
- world: `(-0.3671875,0.2265625), (-0.0078125,0.015625), (0.3515625,0.21875), (-0.0078125,0.4296875)`
- outward face normals: `(-0.5060,-0.8625), (0.4921,-0.8706), (0.5060,0.8625), (-0.4921,0.8706)`
- QA normal set: 위 face normal 4개와 각 인접 face normal 합을 normalize한 axis-diagonal-sum 4개, 총 8개

## rat capsule baseline

- logical px size: `(157, 32)`
- logical px offset: right-facing `(+36.5, +16)`, left-facing `(-36.5, +16)`
- world size: `(1.2265625, 0.25)`
- world offset: right-facing `(+0.28515625, +0.125)`, left-facing `(-0.28515625, +0.125)`
- neutral/contact/passing 프레임 전환 중 size는 바꾸지 않는다. 좌우 flip은 X offset의 부호만 바꾼다.

## 현재 BoxCollider2D 최대 오차

reference polygon과 현 BoxCollider2D의 normal support를 비교한 최대값이다.

| 대상 | 최대 outset error | 최대 inset error |
| --- | ---: | ---: |
| wall | `63.33px` | `57.24px` |
| barrel | `17.96px` | `41.84px` |
| crate | `24.15px` | `24.28px` |

이 오차는 현 box를 C2 정본 후보로 사용할 수 없음을 보여준다. wall/barrel/crate는 위 exact reference polygon을 기반으로 `PolygonCollider2D` 후보를 만들어야 한다.

## C2 support delta·접촉 계약

- support 정의: `support(P,n) = max(dot(p,n))`, `p ∈ P`.
- normal support delta: `Δ = support(collider,n) - support(reference,n)`.
- 허용 delta: `-2px <= Δ <= +1px`, world `-0.015625 <= Δ <= +0.0078125`.
- visible gap: `-1px..+2px`, world `-0.0078125..+0.015625`.
- gap 부호: 양수는 visible reference와 정지한 rat capsule 사이의 빈틈, 음수는 reference 안쪽 침투다.
- `gap > +2px`: invisible collider가 너무 커 일찍 정지하므로 FAIL.
- `gap < -1px`: 보이는 footprint 침투이므로 FAIL.
- opaque-core intersection: `0`; 한 픽셀이라도 있으면 FAIL.
- `ColliderDistance2D.isOverlapped`: `false`.
- neutral/contact/passing 3프레임 stop spread: `<= 1px` (`<= 0.0078125 world`).
- 좌우 mirrored stop error: `<= 1px` (`<= 0.0078125 world`).

## 변경 통제

위 exact polygon, normal set, rat capsule과 tolerance는 S0 baseline이다. gameplay 또는 scene owner가 후보 수치를 바꾸려면 먼저 QA contract revision을 만들고 독립 사전 검토를 받아야 한다. 구현 중 임의 완화는 금지한다.
