# 작업 로그

## 2026-07-30 — 사용자 피드백 접수

- Unity V2 샘플 사용자 확인에서 오브젝트 앞뒤 통과의 부자연스러움과 HUD 초상 내부 상단 잔여 그래픽이 지적됐다.
- HUD는 `hud_rat_portrait_184.png` 상단에 잘못 포함된 황동 프레임 조각으로 원인을 확인했다.
- 쥐 얼굴·털·실루엣은 그대로 두고 제작 소스의 분리 오류만 수정한다.
- 오브젝트는 정지 캡처 한 장으로 원인을 단정하지 않고 벽·통·상자 전후 위치를 재현해 collider와 Y-sort를 분리 대조한다.

## 2026-07-30 — Unity 구현 전 런타임 위치 스윕 재현

- Unity 씬/통합 구현 에이전트가 수정 전 `RatHost2DTechnicalSample` Play 상태에서 독립 위치 스윕을 실행했다.
- 대상:
  - `Barrel_A`
  - `Crate_A`
  - `WallStraight_Occlusion`
- 각 대상마다 쥐 발 Y를 다음 세 위치에 배치했다.
  - 오브젝트 앞 collider 분리 위치
  - 오브젝트 pivot과 동일 Y
  - 오브젝트 뒤 collider 분리 위치
- 각 위치의 쥐/오브젝트 sorting order, collider signed distance·overlap, visual bounds overlap을 CSV와 9장 캡처로 남겼다.
- 산출물: `artifacts/implementation-sweep-before/`

### 재현 결과

| 대상 | 앞 분리 | 동일 pivot | 뒤 분리 |
| --- | --- | --- | --- |
| Barrel | rat `103` > object `86`, collider `+0.01`, visual overlap | rat `75` < object `86`, collider `-0.23` overlap | rat `51` < object `86`, collider `+0.01`, visual overlap |
| Crate | rat `3` > object `-13`, collider `+0.01`, visual overlap | rat `-25` < object `-13`, collider `-0.25` overlap | rat `-51` < object `-13`, collider `+0.01`, visual overlap |
| Wall | rat `-46` > object `-72`, collider `+0.01`, visual overlap | rat `-75` < object `-72`, collider `-0.18` overlap | rat `-94` < object `-72`, collider `+0.01`, visual overlap |

### 원인 판정

- `explicitTieBreak`가 wall `3`, barrel `11`, crate `12`로 달라 동일 지면 pivot에서 쥐가 각각 0.03·0.11·0.12 world unit 먼저 뒤로 전환된다.
- 앞/뒤 collider가 `0.01`만큼 분리된 상태에서도 세 대상 모두 visual bounds는 겹친다.
- 이는 소품 footprint와 쥐 footprint가 실제 보이는 지면 점유부보다 좁어 옆으로 돌아갈 때 물리는 분리됐지만 시각은 겹치는 구간이 넓다는 뜻이다.
- 정지 상태에서 sorting order가 반복 변하는 현상은 재현되지 않았다. 따라서 이번 최소 수정에서는 별도 hysteresis 시스템을 먼저 추가하지 않는다.
- 최소 수정 방향:
  - 모든 정적 occluder tieBreak를 동일한 작은 값으로 정규화
  - 쥐·통·상자 footprint 폭을 실제 지면 점유부에 가깝게 확대
  - wall footprint는 현재 폭을 유지

## 2026-07-30 — HUD 제작 소스 수정

- Production2D 제작 스크립트에 초상 상단 분리 컴포넌트만 제거하는 보정 단계를 추가했다.
- `hud_rat_portrait_184.png`의 Y=0~26에 있던 황동 조각 2,173픽셀만 제거했다.
- Y=27 이하의 쥐 얼굴·털·수염·알파 픽셀은 byte-identical로 보존했다.
- 게임 에셋 20개 중 대상 1개만 SHA가 변경됐고 나머지 19개는 `19/19` 동일했다.
- 검증 결과: `hud-correction-verification.json`의 전체 check `passed=true`.

## 2026-07-30 — 오브젝트 가림 정적 진단 마감

- 추가 MCP 실행 없이 이미 확보된 세 오브젝트의 앞/옆/뒤 런타임 위치와 sortingOrder를 정리했다.
- 현 수식에서 tieBreak 11/12가 전환선을 각각 0.11/0.12 world unit 이동시키는 계약 문제를 확정했다.
- 좁은 물리 footprint와 단일 SpriteRenderer 전체 전환이 결합되어 옆 통과 시 표면을 타는 듯 보일 가능성이 가장 높다고 판정했다.
- `artifacts/occlusion-diagnosis.md`에 확정/추정, 최소 수정안, 수정 전 재현 테스트 인계를 기록했다.
- Unity 코드·씬·반입본은 수정하지 않았다. 실제 WASD 재현, 수정 후 캡처와 QA는 미완료로 인계한다.

## 2026-07-30 — Unity 최소 수정과 수정 후 대조

- Unity 씬/통합 구현 에이전트가 수정 전 스윕 결과에 근거해 다음 값만 조정했다.
  - 정적 wall·barrel·crate의 `explicitTieBreak`: 모두 `1`
  - 쥐 `CapsuleCollider2D` 폭: `0.62 → 0.92`
  - `Barrel_A` footprint 폭: `0.48 → 0.60`
  - `Crate_A` footprint 폭: `0.55 → 0.70`
  - wall footprint 폭과 모든 collider 높이·offset: 유지
- sorting stride 재설계는 적용하지 않았다. 현재 floor의 고정 sorting order와 actor 범위를 함께 바꿔야 하는 범위 확장이며, 이번 원인은 tieBreak 정규화만으로 제거됐기 때문이다.
- 정지 상태 sorting 변화가 재현되지 않아 hysteresis도 추가하지 않았다.
- 보정 HUD PNG를 Production2D 제작 게임 에셋과 Unity 반입본에 동기화했다.
  - 세 파일 SHA-256: `76BC4A430FC170C24C704CF54B2FAFC57EAED0CD2FE5DA5A0F52521F28371908`
  - 기존 검증 JSON의 `y >= 27 byte-identical`, 변경 대상 외 `19/19` 동일 판정을 유지했다.
- Production2D 기술 샘플 씬을 builder로 재생성·저장했다.

### 수정 후 위치 스윕

- 수정 전과 같은 세 대상에 앞/동일 pivot/뒤 위치 스윕을 반복했다.
- 산출물: `artifacts/implementation-sweep-after/`

| 대상 | 앞 분리 | 동일 pivot | 뒤 분리 |
| --- | --- | --- | --- |
| Barrel | rat `104` > object `76`, collider `+0.015` | rat `75` < object `76`, collider `-0.23` overlap | rat `50` < object `76`, collider `+0.015` |
| Crate | rat `3` > object `-24`, collider `+0.015` | rat `-25` < object `-24`, collider `-0.25` overlap | rat `-52` < object `-24`, collider `+0.015` |
| Wall | rat `-46` > object `-74`, collider `+0.015` | rat `-75` < object `-74`, collider `-0.18` overlap | rat `-95` < object `-74`, collider `+0.015` |

- 세 대상 모두 동일 pivot에서 오브젝트가 쥐보다 정확히 `+1` sorting order로 앞에 놓인다.
- 쥐가 앞/뒤로 분리된 표본은 모두 물리 overlap이 없고 기대하는 앞/뒤 관계를 유지한다.
- 단일 전체 쥐 SpriteRenderer의 bounds에는 긴 꼬리와 투명 캔버스가 포함되므로 `visual_overlap=true`만으로 접지부 충돌 실패를 판정하지 않았다. 실제 물리 접근 검증을 별도로 수행했다.

### Unity MCP Play와 자동 테스트

- `RatHost2DController.SimulateFixedStep`와 `Physics2D.Script` stepping으로 통·상자·벽에 각각 수평 접근했다.
- 세 대상 모두 최종 collider distance 약 `0.0006`, overlap `false`, 충돌 이후 이동 clamp가 작동했다.
- 세 대상 각각 300회 정지 sorting 재계산에서 order 변화 `0`.
- HUD 런타임 참조가 `Assets/_Project/Art/Production2D/V1/HUD/hud_rat_portrait_184.png`, 크기 `184×184`임을 확인했다.
- 관련 EditMode: `44 passed / 0 failed`.
- 전체 EditMode: `198 passed / 0 failed`.
- Play 종료 후 Console Error/Warning `0`, active scene dirty `false`.
- 세부 결과: `artifacts/implementation-verification.md`

## 2026-07-30 선별 커밋

- 독립 QA와 프로젝트 총괄이 staged `222`파일, `47.23 MiB`, 금지 경로·민감정보·예상 밖 파일 `0`, `git diff --cached --check` 통과를 재확인했다.
- 첫 아트 후보→통합 기준→아트 로드맵→품질 마스터→실제 에셋→Unity 반입→가림/HUD 수정 체인을 `e7220a7 feat: integrate production 2d art sample`로 커밋했다.
- Stage2·Stage3·`RatHost2DPrototype`·ProjectSettings·반려된 규격 시험 산출물·루트 `_workspace/previews/`·`Builds/`·Python 캐시는 제외했다.
- 구현 `e7220a7`과 1차 상태 동기화 `7adef75`를 `origin/main`에 푸시했다.
