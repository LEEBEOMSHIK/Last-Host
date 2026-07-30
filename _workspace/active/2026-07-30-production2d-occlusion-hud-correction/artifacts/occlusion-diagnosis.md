# Production2D V1 오브젝트 가림 진단

## 판정 범위

- Unity 파일은 수정하지 않았다.
- 기존 코드·씬 계약과 이미 확보한 런타임 위치/정렬 값을 대조했다.
- 캡처 3종과 런타임 CSV는 생성됐으나, 이번 마감에서는 추가 MCP 재현이나 사용자 입력 이동 검증을 하지 않았다.
- 따라서 아래의 수치·정렬 계약은 `확정`, 실제 이동 중 체감 원인과 수정 효과는 `추정`으로 구분한다.

## 정렬 계약 — 확정

`YSortSprite2D`의 현재 계산식은 다음과 같다.

```text
sortingOrder = baseOrder - RoundToInt(footY * 100) + explicitTieBreak
```

- 쥐는 루트 Transform의 Y를 발 접지점(`footY`)으로 사용하고, 시각 자식 전체를 한 개의 `SpriteRenderer` 순서로 전환한다.
- 오브젝트는 바닥 중앙 pivot을 가진 SpriteRenderer의 루트 Y를 발 접지점으로 사용한다.
- 쥐 스프라이트 pivot은 `(0.5, 0.208333)`, 통·상자·벽 스프라이트 pivot은 `(0.5, 0)`이다.
- PPU는 모두 `128`이다.

## 오브젝트별 계약과 확보 값

| 대상 | 루트/foot Y | sprite pivot | collider size | collider offset | tieBreak | 확보된 sortingOrder 및 전환 표본 |
| --- | ---: | --- | --- | --- | ---: | --- |
| `RatHost` | 이동 루트 Y | `(0.5, 0.208333)` | `(0.62, 0.26)` Capsule, horizontal | `(0.08, 0.13)` | `0` | Y=-0.87에서 `87`, Y=0.12에서 `-12`, Y=0.71에서 `-71` |
| `Barrel_A` | `-0.75` | `(0.5, 0)` | `(0.48, 0.22)` | `(0, 0.11)` | `11` | 고정 `86`; 쥐의 순서 전환 경계는 약 Y=`-0.86` |
| `Crate_A` | `0.25` | `(0.5, 0)` | `(0.55, 0.24)` | `(0, 0.12)` | `12` | 고정 `-13`; 쥐의 순서 전환 경계는 약 Y=`0.13` |
| `WallStraight_Occlusion` | `0.75` | `(0.5, 0)` | `(1.05, 0.18)` | `(0, 0.08)` | `3` | 고정 `-72`; 쥐의 순서 전환 경계는 약 Y=`0.72` |

런타임 표본에서 앞 접촉/뒤 접촉 순서는 세 대상 모두 수식과 일치했다.

- 통: 앞 `rat 103 > barrel 86`, 뒤 `rat 51 < barrel 86`
- 상자: 앞 `rat 3 > crate -13`, 뒤 `rat -51 < crate -13`
- 벽: 앞 `rat -46 > wall -72`, 뒤 `rat -94 < wall -72`

원본 수치는 `occlusion-captures/occlusion-runtime-samples.csv`에 보존했다.

## 사용자 체감 원인 판정

### 확정

1. `explicitTieBreak`가 단순 동률 해소값이 아니라 공간상의 전환 경계를 이동시킨다.
   - 현재 식에서 통 `11`은 전환선을 `0.11 world unit`, 상자 `12`는 `0.12 world unit` 이동시킨다.
   - 따라서 오브젝트 루트의 지면 접점과 실제 앞뒤 전환선이 일치하지 않는다.
2. 쥐와 소품의 물리 collider는 보이는 전체 실루엣보다 좁다.
   - 쥐 SpriteRenderer bounds 폭은 약 `2.0`, collider 폭은 `0.62`다.
   - 통 bounds 폭은 약 `0.76`, collider 폭은 `0.48`이다.
   - 상자 bounds 폭은 약 `0.88`, collider 폭은 `0.55`이다.
   - 벽 bounds 폭은 약 `1.26`, collider 폭은 `1.05`이다.
3. 쥐는 단일 SpriteRenderer 단위로 앞/뒤가 한 순간에 전환된다. 부분 가림은 현재 계약에 없다.

### 추정 — 가능성 높은 순서

1. **가장 가능성이 높음:** 옆으로 돌아갈 때 collider끼리는 이미 분리됐지만 시각 실루엣은 계속 겹친 상태에서 쥐 전체 SpriteRenderer가 한 번에 순서를 바꿔, 쥐가 통·상자·벽 표면 위로 올라타거나 순간적으로 뒤집히는 것처럼 보인다.
2. `tieBreak 11/12`가 전환선을 지면 접점에서 각각 0.11/0.12만큼 밀어 위 현상을 더 눈에 띄게 만든다.
3. 실제 키 입력 중 프레임 떨림이 동반된다면 Y 전환선 근처에서 이전 순서를 유지하는 hysteresis가 없는 점도 보조 원인일 수 있다. 이번 마감에서는 실제 연속 입력을 다시 재현하지 않았으므로 확정하지 않는다.

## 최소 Unity 수정안

Unity 씬/통합 구현 에이전트가 아래 순서로 수정·검증한다.

1. **tie-break를 깊이에서 분리한다.**
   - 권장식: `sortingOrder = baseOrder + (-RoundToInt(footY * 100) * stride) + tieBreak`
   - `stride`는 모든 tieBreak 절댓값보다 큰 값(예: `16`)으로 둔다.
   - 이렇게 하면 tieBreak는 같은 Y 양자화 칸 안에서만 동률을 해소하고 전환선을 0.11~0.12 world unit 이동시키지 않는다.
2. **collider는 전체 그림 폭이 아니라 실제 지면 점유부에 맞춰 재조정한다.**
   - 우선 검증 후보: 쥐 폭 `0.62 → 0.90~1.00`, 통 `0.48 → 약 0.58`, 상자 `0.55 → 약 0.68`; 벽 `1.05`는 유지 후보.
   - 꼬리·수염·상단 장식까지 collider로 덮지는 않는다. 이동성이 과도하게 줄면 물리 collider와 가림 판단 footprint를 분리한다.
3. **수정 전 재현 테스트를 먼저 고정한다.**
   - 각 오브젝트에 대해 앞 접촉, 옆 collider 분리 직후, 뒤 접촉의 쥐 위치·sortingOrder를 테스트 데이터로 만든다.
   - 옆 표본에서 보이는 실루엣이 겹치는 동안 전체 순서가 부자연스럽게 바뀌는지 기록한다.
4. 위 두 수정 뒤에도 경계 떨림이 남을 때만 1~2 픽셀 상당의 Y hysteresis를 추가한다. 스프라이트 분할·마스크 도입은 최소 수정 범위를 넘으므로 이번 단계에서는 보류한다.

## 캡처 및 미완료 항목

- 생성 완료:
  - `occlusion-captures/Barrel_A-front-side-behind.png`
  - `occlusion-captures/Crate_A-front-side-behind.png`
  - `occlusion-captures/WallStraight_Occlusion-front-side-behind.png`
  - `occlusion-captures/occlusion-runtime-samples.csv`
- 미완료:
  - 사용자 실제 WASD 연속 이동 재현
  - 수정 적용 후 전후 비교
  - 관련/전체 EditMode, Console, sceneDirty 독립 QA

이 미완료 항목은 Unity 씬/통합 구현 에이전트와 QA/검증 에이전트에게 인계한다.
