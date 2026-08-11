# A01 캐스트 재생성·결정론적 셀 재배치 설계

## 1. 목적과 승인 경계

이 문서는 A01 회사 장면의 Cast correction 실패를 반복하지 않기 위한 경량 설계다. attempt 07 실행 뒤 사용자가 승인한 현재 방향은 `추가 imagegen 없이 attempt 07 raw의 20개 포즈를 결정론적으로 분리·재배치한 project-ready derivative`다. 실제 도구 구현·Unity 반입은 이 문서 작성 범위가 아니다.

- 통과한 background correction은 byte-for-byte 불변으로 보존한다. canonical 기록의 SHA-256 `DA5F…AA0C`를 검증 기준으로 사용하며 재생성·재처리하지 않는다.
- built-in imagegen attempt 07은 누적 `7/7`로 종료됐다. versioned raw는 `1122×1402`, `1,648,495 bytes`, SHA-256 `24A143D7344DAC8358CD496C6AD03718AADB492D67B96E7CCCF0E46DA08A090D`다.
- attempt 08, retry, CLI/API 이미지 생성은 허용하지 않는다.
- 결과는 `preview-only candidate`이며 사용자 수용과 후속 게임 규격 QA 전에는 final art 또는 완성 스프라이트 시트가 아니다.
- Cast repack derivative가 모든 자동 계약과 독립 visual QA를 통과하기 전에는 foreground 처리와 Unity 작업을 시작하지 않는다.

## 2. 실패 원인

1. correction prompt가 `P4는 서 있는 인물이며 기존 가방을 같은 쪽에 유지한다`는 invariant를 축약해 P4의 자세·소품 정체성이 흔들렸다.
2. built-in imagegen 출력 크기를 exact canvas처럼 가정했지만 실제 크기는 보장되지 않는다.
3. literal `#ff00ff` 배경을 생성 모델에만 맡겨 near-magenta와 색 변이가 대량으로 남았다.
4. raw 생성 후보를 검증·정규화되지 않은 상태에서 final grid처럼 취급했다.
5. attempt 07은 인물·포즈 의미는 개선됐지만 raw의 네 수평 등분 경계를 캐릭터·의자·가방이 침범했다. global nearest-neighbor resize는 상대 배치를 보존하므로 이 결함을 해결할 수 없다.

따라서 attempt 07 raw의 시각 내용은 보존하되, exact canvas·hard alpha·cell boundary는 명시적 20-cell layout manifest와 정수 translation만 사용하는 결정론적 derivative가 보장한다.

## 3. 접근안 비교

| 접근 | 장점 | 단점 | 판정 |
| --- | --- | --- | --- |
| **attempt 07 manifest 기반 셀 재배치** | 추가 생성 0회, 승인 후보의 픽셀·정체성·포즈를 보존하고 grid·alpha·피벗을 결정론적으로 고정 | 20개 source 영역과 anchor를 한 번 명시·검증해야 함 | **채택** |
| Unity variable SpriteRect 직접 사용 | 원본 재배치 파일이 불필요 | 프레임마다 rect·pivot·간격이 달라 제한 애니메이션 교체와 QA가 복잡해짐 | 미채택 |
| 수동 재작화 | 각 포즈를 미술적으로 완전히 정리 가능 | 20 pose 재작화 비용이 크고 별도 아트 범위·승인이 필요 | 미채택 |

채택안은 생성 모델이 만든 `정체성·pose·원본 색상`을 다시 그리지 않고, 도구가 승인된 `matte/despill·20개 영역 소유권·정수 이동·alpha·cell boundary·결정성`만 담당한다.

## 4. 산출물 계층과 승격 순서

| 계층 | 경로 | 규칙 |
| --- | --- | --- |
| attempt 07 raw | `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png` | built-in 반환물을 변경 없이 저장하고 SHA-256을 기록한다. 덮어쓰지 않는다. |
| layout manifest | `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-layout.json` | raw SHA, 20개 non-overlapping source rect, target row/column, source axis와 target anchor를 고정한다. |
| QA용 derivative | `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-repacked-alpha.png` | raw와 manifest에서만 파생한다. exact grid·hard alpha 자동 검증과 독립 visual QA 대상이다. |
| 기존 canonical raw | `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-source.png` | attempt 07 QA PASS 전에는 덮어쓰지 않는다. |
| Unity preview | `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-cast-pose-grid-preview.png` | QA PASS와 별도 실행 게이트 뒤에만 생성한다. |

승격 순서는 다음으로 고정한다.

1. attempt 07 raw를 versioned 경로에 저장하고 전체 prompt·입력 reference·raw dimensions·bytes·SHA-256을 generation log에 기록한다.
2. full-canvas reference retained mask를 먼저 계산한 뒤 20개 source rect와 anchor를 manifest에 고정한다. 20개 rect는 source canvas 전체를 겹침 없이 정확히 한 번 분할하고, 따라서 reference retained pixel도 모두 정확히 한 pose에 속해야 한다.
3. versioned raw와 manifest에서 QA용 derivative를 만든다. 기존 canonical raw와 Unity preview는 건드리지 않는다.
4. 자동 계약과 독립 visual QA가 모두 PASS하면 repacked-alpha derivative bytes를 기존 canonical raw 경로 `a01-office-cast-pose-grid-source.png`에 승격한다. versioned raw와 manifest는 provenance로 그대로 보존한다.
5. 같은 raw·manifest·명령으로 Unity preview를 다시 만들고, QA용 derivative와 SHA-256이 같은지 확인한다.
6. 그 뒤에만 foreground 도구와 Task 2 후속 단계를 재개한다.

## 5. attempt 07 생성 프롬프트 계약

아래 내용은 실제 attempt 07의 재현·출처 기록이다. 호출은 이미 `7/7`로 종료됐으며 다시 실행하지 않는다. spatial reference는 identity·의상·색·시점의 유일한 입력이었다.

```text
Use case: stylized-concept
Asset type: preview-only A01 pixel-art limited-animation cast pose sheet
Input images: Image 1 is the sole spatial reference for all five worker identities, outfits, proportions, pixel density, camera-facing angles, and bag sides. Do not import identity, clothing, pose, or props from any other source.

Primary request: Create one cast sheet arranged as four columns by five rows. Treat the canvas as 20 invisible equal cells with no visible grid lines. Keep at least 12% of every cell width and height as perfectly flat #ff00ff gutter on all four sides. No person, hair, chair, hand, foot, or bag may touch or cross a cell boundary.

Canvas intent: exactly 1280×1600 pixels, four columns by five rows, each cell exactly 320×320 pixels. Keep all figures centered and consistently scaled inside their own cells.

Row 1 — P1: the blue-shirt man with brown curly hair and glasses, seated in all four cells. Poses: seated idle; small speaking hand gesture; warm laugh; chair-push/rise start.
Row 2 — P2: the woman with a hair bun and olive top, seated in all four cells. Poses: seated idle; delayed nod and smile; short laugh; neutral hold.
Row 3 — P3: dark hair and dark-green clothing, mostly back-facing and seated in all four cells. Poses: seated work; small shoulder laugh; clearly different short head turn; neutral hold.
Row 4 — P4: beige overshirt, standing in all four cells, never seated and never paired with a chair. Preserve the same black personal commuter bag on the same body side shown in Image 1 in every cell. Poses: standing idle; conversational hand gesture; standing turn toward the right exit; standing neutral hold.
Row 5 — P5: rust blouse and cream pants, standing in all four cells, never seated and never paired with a chair. Preserve the same brown personal commuter bag on the same body side shown in Image 1 in every cell. Poses: standing idle; warm laugh; right-exit step-ready pose; standing neutral hold.

Background: every empty pixel must be literal #ff00ff only. No gradient, near-magenta variation, noise, texture, shadow, floor, reflection, halo, or fringe.
Constraints: preserve identity, outfit, scale, pixel density, camera angle, bag identity, and bag side across each row; hard pixel edges; generous separation between cells.
Avoid: visible grid lines, labels, letters, numbers, extra people, extra props, food or lunch bags, text, logo, infection cue, horror cue, boundary crossing, cropped silhouettes, chairs in rows 4 or 5.
```

프롬프트의 `12% gutter`는 생성 안전 여백 목표다. 자동 수용의 hard minimum은 repack derivative에서 각 cell 경계의 6px band이며, 두 기준을 같은 값으로 해석하지 않는다.

## 6. 결정론적 derivative 계약

역할을 혼합하지 않도록 다음 focused tool과 test를 사용한다.

- `tools/art/Repack-ChromaPoseGrid.ps1`
- `tools/art/Test-RepackChromaPoseGrid.ps1`

입력은 versioned raw와 versioned layout manifest다. manifest는 최소 다음 값을 소유한다.

- exact source SHA-256·width·height
- output `1280×1600`, grid `4×5`, cell `320×320`, boundary band `6`
- P1~P5 × 4 pose의 고유 ID, source canvas 전체를 겹침 없이 분할하는 source rect, 서로 중복되지 않는 target row/column
- 각 pose의 source axis와 cell-local target anchor
- seated P1~P3는 의자 중심축, standing P4~P5는 몸통 root 축을 수평 기준으로 사용하며 target x는 `160`
- retained union의 최저점을 공통 접지선 cell-local y `306`에 맞춘다.

처리 순서는 다음으로 고정한다.

1. raw bytes와 SHA-256, `1122×1402` canvas를 확인한다.
2. source rect를 적용하기 전에 full-canvas reference retained mask를 한 번 계산한다. key color는 RGB `(255,0,255)`이고, 픽셀 `(R,G,B)`의 key distance는 `max(abs(R-255), abs(G-0), abs(B-255))`다. **enclosed hole을 포함한 모든** strong seed `distance <= 24`에서 4-neighbor로 `distance <= 48`인 픽셀만 flood-fill한다. flood-fill된 mask는 RGBA `(0,0,0,0)` hard matte로 만든다. retained pixel 중 mask Chebyshev distance `<=2`인 edge만 despill 후보로 보고, radius `8` 안에서 mask distance `>2`·key distance `>96`인 donor를 squared distance, `y`, `x` 순으로 고른다. 후보색이 donor→key 선분 projection `t`에서 `0.08..0.92`이고 재구성 residual이 채널별 최대 `<=24`일 때만 donor RGB로 바꾸며 alpha·silhouette은 유지한다. 그 외 unmasked non-despilled core는 byte-exact다.
3. manifest의 20개 source rect가 source canvas의 모든 좌표를 중복·공백 없이 정확히 한 번 덮는지 먼저 검사한다. 각 rect와 full-canvas reference retained mask의 교집합에 있는 **모든 retained pixel을 하나의 pose union**으로 취급한다. largest-component만 고르는 방식은 의자 바퀴·노트북·가방·신발을 잃을 수 있으므로 금지한다.
4. pose union을 정수 좌표로만 translation한다. 확대·축소·회전·보간·재도색은 금지한다.
5. 각 pose는 target cell에서 x anchor `160`, 접지선 y `306`에 맞춘다. 6px boundary band를 침범하면 실패한다.
6. 모든 검사를 임시 output에 수행하고 전부 PASS한 경우에만 versioned derivative를 원자 교체한다. 실패 시 raw·manifest·기존 output·canonical source는 그대로 둔다.

비주얼 사전검토의 최대 실루엣은 약 `175×265px`, 4px 안전 halo 포함 약 `183×273px`로 `308×308` 수용 영역 안에 들어간다. 행 사이 clean gap 근사치는 `33/22/9/11px`이며, R3/R4의 9px gap은 exact retained-mask 검사로 재확인한다. 원래 생성 프롬프트의 12% gutter는 생성 목표이고 repack hard PASS 조건이 아니다.

### 자동 수용 계약

| 항목 | PASS 조건 |
| --- | --- |
| canvas | exact `1280×1600` |
| grid | exact `4×5`, 각 cell exact `320×320` |
| alpha | 모든 픽셀이 `0` 또는 `255` |
| transparent RGB | alpha `0`인 모든 픽셀이 RGB `(0,0,0)` |
| boundary | 각 cell의 local x/y `0..5`, `314..319` band에 alpha `255` 픽셀 `0` |
| coverage | 각 cell의 opaque coverage가 cell 면적의 `5%` 이상 `60%` 이하 |
| ownership | 20개 source rect가 source canvas 모든 좌표를 정확히 한 번 덮고, 각 reference retained pixel이 pose 하나에만 속해 중복·누락 `0` |
| pixel preservation | **all unmasked non-despilled core** source pixel마다 동일 RGBA가 정확히 `(dx,dy)` 정수 offset 위치에 1:1 존재한다. authorized matte는 transparent black만 만들고, authorized despill은 위 distance·donor·projection·residual 조건을 모두 만족한 edge RGB만 바꾸며 alpha·silhouette은 바꾸지 않는다. 해당 target cell에 manifest가 설명하지 않는 추가 불투명 픽셀 `0` |
| source 불변 | 처리 전후 raw bytes와 SHA-256 동일 |
| 결정성 | 같은 raw·manifest·인자로 두 번 만든 derivative SHA-256 동일 |

자동 검사는 identity·의상·pose 의미를 판정하지 않는다. P4/P5 정체성, standing 여부, 가방 색·종류·같은 쪽 유지, P1~P3 seated 역할, 픽셀 품질은 구현 주체와 분리된 비주얼/테크아트 에이전트가 raw와 derivative를 함께 보고 판정한다.

### 역할 소유권

- 비주얼/테크아트 구현 소유자는 20개 source rect·axis·anchor가 든 manifest 한 파일과 그 불변식을 소유한다.
- Unity 씬/통합 구현 소유자는 repack tool·test·derivative 생성과 QA PASS 뒤 조건부 canonical 승격을 소유한다.
- 독립 비주얼 QA는 raw·manifest·derivative를 비교 판정하며 production 파일을 수정하지 않는다.

## 7. 오류 처리와 fail-fast

- raw·manifest가 없거나 PNG decode 실패, source SHA·canvas 불일치, 20개 ID·target cell의 누락·중복, source rect 범위 초과·중첩·canvas 공백, reference retained pixel 중복·누락, 빈 pose, pose union이 `308×308`을 초과, output 경로 충돌, overwrite 승인 부재 중 하나라도 있으면 nonzero로 끝낸다.
- exact canvas·hard alpha·transparent RGB·boundary·coverage·ownership·authorized matte/despill·pixel preservation·결정성 중 하나라도 실패하면 derivative를 승격하지 않는다.
- P4/P5 identity·pose·bag 또는 전체 시각 품질이 독립 visual QA에서 실패하면 자동 검사가 PASS여도 전체 결과는 REJECT다.
- 실패 결과는 versioned raw와 판정만 이력으로 보존한다. canonical raw, background, foreground, preview, contract, Unity는 변경하지 않는다.
- repack 실패 뒤 attempt 08 또는 다른 이미지 생성은 금지한다. 수동 재작화 또는 추가 생성은 새 사용자 승인 항목이다.

## 8. TDD와 검증 경계

### RED

먼저 `Test-RepackChromaPoseGrid.ps1`을 만들고 production tool 부재로 test command가 nonzero인 상태를 RED로 기록한다.

- synthetic 4×5 source rect의 disconnected components를 모두 보존하지 못함
- wrong SHA, source rect overlap/out-of-range, retained pixel ownership 중복·누락을 거부하지 않음
- `308×308` 초과 pose, 빈 pose, 6px boundary collision, coverage `5%..60%` 밖 cell을 거부하지 않음
- per-pose scaling·rotation·interpolation 없이 정수 translation만 수행하고 pose별 동일 RGBA가 exact `(dx,dy)` offset에 1:1 대응한다는 계약을 충족하지 않음
- closed key hole 제거, blend edge despill, legitimate nonblend purple byte-exact 보존, hard alpha·transparent RGB black·source 불변·repeated output SHA 동일, real candidate unresolved blend fringe `0` 계약 중 하나라도 충족하지 않음

### GREEN

`Repack-ChromaPoseGrid.ps1`에 위 authorized matte/despill과 최소 repack 경로를 구현한 뒤 같은 test가 exit `0`이어야 한다. focused tool은 foreground나 다른 아트 소스를 처리하지 않는다.

### attempt 07 수용

1. generation log에 먼저 기록한 attempt 07 raw SHA-256을 expected source SHA로 고정한다.
2. test script가 versioned raw의 source SHA 불변, manifest ownership, 자동 수용 계약, repeated derivative SHA를 확인한다.
3. 독립 visual QA가 20 cells를 실제 크기에서 비교한다.
4. 자동·시각 검토가 모두 PASS한 경우에만 canonical cast를 교체한다.

## 9. 완료 조건

- background는 기존 PASS bytes를 유지한다.
- attempt 07 raw·layout manifest·derivative가 서로 다른 versioned 파일로 추적된다.
- derivative가 exact canvas/grid, hard alpha, boundary, coverage, ownership, authorized matte/despill, all unmasked non-despilled core exactness, source 불변, 결정성 계약을 통과한다.
- P1~P5 identity·seated/standing 역할·P4/P5 commuter bag invariant가 독립 visual QA를 통과한다.
- canonical cast는 QA PASS 뒤에만 교체되고, 같은 raw·manifest 명령으로 만든 Unity preview가 QA derivative SHA와 일치한다.
- 모든 산출물은 `preview-only candidate`로 남으며 final art로 선언하지 않는다.
- 실패 시 foreground와 Unity는 정지하고 imagegen attempt 08은 실행하지 않는다.
