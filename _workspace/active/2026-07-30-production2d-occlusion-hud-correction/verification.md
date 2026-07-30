# 독립 QA 검증

## 판정

`완료 가능 — 사용자 실제 네이티브 WASD 체감 확인만 별도`

이번 수정 범위인 Production2D V1 오브젝트 가림 전환·충돌 footprint와 HUD 쥐 초상 상단 잔여 조각 제거는 독립 QA를 통과했다. Windows 빌드는 이번 기술 샘플 수정의 요구 범위가 아니므로 실행하지 않았다.

## HUD와 에셋 무결성

- 보정 원본·제작 게임 에셋·Unity 반입본:
  - 모두 `RGBA`, `184×184`
  - SHA-256 `76BC4A430FC170C24C704CF54B2FAFC57EAED0CD2FE5DA5A0F52521F28371908`
  - 세 픽셀 배열 동일
- 수정 전 이미지를 제작 소스에서 메모리 재구성해 독립 대조:
  - 변경 픽셀 `2,173`
  - 최대 변경 Y `26`
  - `y >= 27` byte-identical
  - 상단 `y=0..26` 완전 투명
  - 가시 컴포넌트 `2 → 1`
- 현재 제작 해시 맵 `20/20`, Unity PNG 반입본 `18/18`, 대상 외 제작 에셋 `19/19` 일치.
- `hud-before-after.png`와 수정 Game View를 확대 확인했으며, 분리된 황동 상단 조각만 제거되고 쥐 본체·HUD frame은 유지됐다.

## 씬 계약과 위치 스윕

원본 `RatHost2DTechnicalSample`을 읽기 전용으로 대조했다.

| 대상 | collider size | offset | tieBreak |
| --- | --- | --- | ---: |
| RatHost2D | `(0.92, 0.26)` | `(0.08, 0.13)` | 동적 쥐 기준 |
| Barrel_A | `(0.60, 0.22)` | `(0.00, 0.11)` | `1` |
| Crate_A | `(0.70, 0.24)` | `(0.00, 0.12)` | `1` |
| WallStraight_Occlusion | `(1.05, 0.18)` | `(0.00, 0.08)` | `1` |

수정 후 CSV와 캡처 9장을 대조한 결과:

- 앞 분리 `0.015`: 세 대상 모두 rat이 object보다 앞, collider overlap `false`.
- 동일 pivot: Barrel `75/76`, Crate `-25/-24`, Wall `-75/-74`로 object가 rat보다 정확히 `+1`.
- 뒤 분리 `0.015`: 세 대상 모두 rat이 object보다 뒤, collider overlap `false`.
- 통 동일 pivot, 벽 앞 분리와 수정 Game View를 시각 확인했으며 전후 관계가 수치 계약과 일치했다.

## Unity MCP Play

`Physics2D.Script`, `RatHost2DController.SimulateFixedStep(0.02)`로 원본 씬에서 독립 재검증했다.

| 대상 | 시작 distance | 최종 distance | overlap | clamp step | 정지 300회 jitter |
| --- | ---: | ---: | --- | ---: | ---: |
| Barrel_A | `0.2000` | `0.0006` | `false` | `3` | `0` |
| Crate_A | `0.2000` | `0.0006` | `false` | `3` | `0` |
| WallStraight_Occlusion | `0.2000` | `0.0006` | `false` | `3` | `0` |

- 접촉 상태에서도 동일 pivot order는 각각 `75/76`, `-25/-24`, `-75/-74`.
- 이동 회귀: 우측 12스텝 `delta=(0.72, 0.00)`, 방향 `East`.
- 카메라 중심 오차: `(0.16, 0.00) px`.
- HUD 런타임 경로: `Assets/_Project/Art/Production2D/V1/HUD/hud_rat_portrait_184.png`.
- 최종 Console Error/Warning `0`.
- Stop 후 active scene `RatHost2DTechnicalSample`, `sceneDirty=false`.
- Game Camera MCP 캡처는 런타임 instance ID 해석 실패로 1회 실패했으나 이는 MCP 도구 오류였다. 해당 로그를 제거한 뒤 Console 0을 재확인했고, 구현 캡처와 독립 파일 시각 대조로 화면 검증을 완료했다.

실제 Game View 포커스에서 키보드로 누른 네이티브 WASD 수신·체감은 위 스크립트식 이동 검증에 포함하지 않았으며 사용자 확인 항목으로 분리한다.

## EditMode

별도 QA 복제본 `C:\tmp\LastHostQaOcclusion-20260730-1`에서 원본과 독립 실행했다. 최초 Library·패키지 임포트가 장시간 진행돼 한동안 결과 XML이 없었으나, 최종적으로 두 실행 모두 XML을 생성하고 Unity 프로세스가 정상 종료됐다.

- 관련 `LastHost.Prototype.TechnicalSample2D.Tests`
  - `44 passed / 0 failed / 0 skipped / 0 inconclusive`
  - 결과: `artifacts/qa-editmode-related-results.xml`
  - SHA-256 `DB1E2E42AE1A271EFBA8881240C9257D778FD337FD87AC54C6A6DB7ECDDF388E`
- 전체 EditMode
  - `198 passed / 0 failed / 0 skipped / 0 inconclusive`
  - 결과: `artifacts/qa-editmode-all-results.xml`
  - SHA-256 `B053300F3C9272D79DCF95276C278124B6FFAE075373CCB54C3579BB1CAC9870`
- 구현 에이전트가 원본에서 남긴 `44/44`, `198/198` 결과와 독립 복제본 수가 일치한다.
- 복제본 Unity 잔류 프로세스 `0`.

## 보호 범위와 저장소

다음 보호 대상 SHA-256은 구현 전 기준과 같다.

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

`Physics2DSettings.asset` diff는 없고, `ProjectSettings.asset`에는 작업 전부터 관리되던 `APP_UI_EDITOR_ONLY` define 차이만 있다. Stage2/Stage3와 `RatHost2DPrototype` 보호 범위에서 이번 QA로 생긴 예상 밖 직렬화 변경은 없다. `git diff --check`도 오류 없이 통과했으며 줄바꿈 경고만 출력됐다.

## 남은 위험

- 쥐는 꼬리까지 포함한 단일 SpriteRenderer이므로 몸 일부만 오브젝트에 단계적으로 가리는 정밀 마스킹은 제공하지 않는다.
- 실제 네이티브 WASD 입력 체감과 PPU 128·상대 크기 사용자 수용은 별도 확인 사항이다.
- 위 두 항목은 이번 최소 수정의 기술 완료를 막지 않는다.

최종 상태판 감사: `PASS` — task·CURRENT·현황판 상태, active 경로, completed 중복 없음, 사용자 WASD→HUD→후속 결정 순서, `HEAD=origin/main`, Physics2D 무변경과 기존 `APP_UI_EDITOR_ONLY` 소유 diff가 서로 일치한다.
