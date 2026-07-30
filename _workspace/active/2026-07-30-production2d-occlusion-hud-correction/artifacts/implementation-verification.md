# Unity 씬/통합 구현 자체 검증

## 구현 결과

- 정적 wall·barrel·crate `explicitTieBreak`: 모두 `1`
- 쥐 `CapsuleCollider2D`: `(0.62, 0.26) → (0.92, 0.26)`
- `Barrel_A` collider: `(0.48, 0.22) → (0.60, 0.22)`
- `Crate_A` collider: `(0.55, 0.24) → (0.70, 0.24)`
- wall collider와 collider offset: 변경 없음
- 별도 hysteresis 및 Y-sort stride 재설계: 적용하지 않음

수정 전 정지 sorting jitter는 재현되지 않았고, 서로 다른 tieBreak만으로 전이선이 최대 `0.12 world unit` 이동하는 원인이 확정됐다. 따라서 정렬 런타임 시스템을 확장하지 않고 scene builder의 정적 오클루더 계약만 정규화했다.

## HUD 동기화

다음 세 파일의 SHA-256이 같다.

- 보정 원본: `artifacts/hud_rat_portrait_184-corrected.png`
- 제작 게임 에셋: `2026-07-30-rat-host-2d-production-assets-v1/artifacts/game-assets/hud/hud_rat_portrait_184.png`
- Unity 반입본: `Assets/_Project/Art/Production2D/V1/HUD/hud_rat_portrait_184.png`
- SHA-256: `76BC4A430FC170C24C704CF54B2FAFC57EAED0CD2FE5DA5A0F52521F28371908`

기존 `hud-correction-verification.json` 기준:

- 변경 픽셀: `2,173`
- 최대 변경 Y: `26`
- `y >= 27` 행: byte-identical
- 다른 게임 에셋: `19/19` 동일
- 전체 판정: `passed=true`

## 수정 전·후 위치 스윕

- 수정 전: `implementation-sweep-before/occlusion-sweep-before.csv`, PNG 9장
- 수정 후: `implementation-sweep-after/occlusion-sweep-after.csv`, PNG 9장

수정 후 결과:

| 대상 | object order | 동일 pivot rat order | 동일 pivot 관계 | 앞/뒤 분리 |
| --- | ---: | ---: | --- | --- |
| Barrel | 76 | 75 | object가 `+1` 앞 | 양쪽 `0.015`, overlap false |
| Crate | -24 | -25 | object가 `+1` 앞 | 양쪽 `0.015`, overlap false |
| Wall | -74 | -75 | object가 `+1` 앞 | 양쪽 `0.015`, overlap false |

- 앞 분리 위치에서는 모두 rat이 object보다 앞이다.
- 뒤 분리 위치에서는 모두 rat이 object보다 뒤다.
- 동일 pivot에서는 모두 object가 rat보다 정확히 `+1`이다.
- 300회 정지 재계산에서 sorting order 변화는 세 대상 모두 `0`이다.

## Unity MCP Play

`RatHost2DController.SimulateFixedStep(0.02)`와 `Physics2D.Script` stepping으로 각 대상에 수평 접근했다.

| 대상 | 최종 collider distance | overlap | clamp step | 정지 jitter 변화 |
| --- | ---: | --- | ---: | ---: |
| Barrel | 0.0006 | false | 77 | 0 |
| Crate | 0.0006 | false | 77 | 0 |
| Wall | 0.0006 | false | 77 | 0 |

- 쥐 collider: `(0.920, 0.260)`
- HUD portrait 런타임 경로: `Assets/_Project/Art/Production2D/V1/HUD/hud_rat_portrait_184.png`
- HUD portrait 크기: `184×184`
- 결과 원문: `mcp-play-verification.txt`
- 최종 화면: `game-view-occlusion-hud-corrected.png`

## EditMode

- 관련 어셈블리 `LastHost.Prototype.TechnicalSample2D.Tests`
  - `44 passed / 0 failed / 0 skipped / 0 inconclusive`
  - `editmode-relevant-test-result.txt`
- 전체 EditMode
  - `198 passed / 0 failed / 0 skipped / 0 inconclusive`
  - `editmode-all-test-result.txt`

추가된 회귀 계약:

- 쥐·barrel·crate·wall collider 크기
- occluder tieBreak `1` 기준 sorting order
- ground pivot 전후 `0.02`에서 rat front/behind 전환
- 300회 정지 sorting 안정성

## Unity 최종 상태

- Play 진입·종료: 통과
- Console Error/Warning: `0`
- active scene: `RatHost2DTechnicalSample`
- scene dirty: `false`

## 보호 대상 SHA-256

다음 값은 구현 전 기준과 같다.

- `RatHost2DPrototype.unity`
  - `8B758BD5E7B47B46E13E7EA7EFD669DAF7332626AB19074818F8073222093ED6`
- `RatHost2DPrototypeSceneBuilder.cs`
  - `9C1D45D0B6CC4353ADCDBFA25E316B07DAC98E0456F8A2AB7D352C649C319135`
- `RatHost2DSessionController.cs`
  - `6462EE1B107052B494566DD69D6DA90D4E30AEA55E211874437930BE676AC081`
- `ProjectSettings.asset`
  - `008078ADBB3A01264F4C097558F5983453A93F6254E600AB2776D269DD8201D9`
- `Physics2DSettings.asset`
  - `E3CBDBF0BE15244B7B67D2F07BDB8E8981911C11A8C7FE8035F5261272FEC658`

## 남은 위험과 QA 인계

- 쥐는 긴 꼬리를 포함한 단일 전체 SpriteRenderer이므로 오브젝트 경계에서 몸 일부만 단계적으로 가리는 정밀 마스킹은 제공하지 않는다. 이번 수정은 ground pivot 전이와 물리 접지부를 바로잡는 범위다.
- MCP 스크립트식 이동·충돌 검증은 통과했지만 실제 Game View 포커스의 네이티브 WASD 체감 확인은 독립 QA 또는 사용자 확인이 필요하다.
- Production2D PPU 128과 표시 배율, 쥐 외형 자체의 사용자 수용은 이번 수정 범위 밖이다.
- 이 문서는 구현 에이전트 자체 검증이며 QA/검증 에이전트의 독립 판정을 대체하지 않는다.
