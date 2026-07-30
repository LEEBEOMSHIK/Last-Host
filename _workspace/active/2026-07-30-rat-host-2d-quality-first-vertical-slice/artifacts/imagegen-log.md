# 고품질 제작 마스터 이미지 생성 로그

## 공통 정보

- 생성 작업 ID: `2026-07-30-rat-host-2d-quality-first-vertical-slice`
- 담당: ChatGPT 이미지 아트 에이전트
- 사용 도구: OpenAI 내장 `imagegen`
- 생성일: 2026-07-30
- 결과 성격: 고품질 제작 마스터 후보
- 최종 게임 에셋 여부: 아님
- 기존 저품질 Q1 PNG 입력 사용: 없음
- 공통 후속 경계: 실제 게임 에셋 승격 전 반복 타일 분해, 공통
  캔버스·피벗, 투명 배경, 수작업 도트 페인트오버와 Unity QA 필요

## ENV-QM-01 환경 품질 마스터

### 입력 reference

- `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
  - 역할: 승인된 게임플레이 분위기·환경 밀도·구도·광원 reference
- `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-integrated-art-targets/artifacts/integrated-targets/environment-integrated-target.png`
  - 역할: 승인된 모듈형 하수도 재질·벽·수면·소품 품질 reference

### 전체 프롬프트

```text
Use case: stylized-concept
Asset type: high-quality 2D isometric pixel-art game environment production master candidate
Input images: Image 1 is the approved gameplay mood, density, composition, and lighting reference; Image 2 is the approved modular sewer material, wall, water, and prop quality reference. Use them as visual references only, not as edit targets and not as layouts to copy literally.
Primary request: create one polished 16:9 fixed isometric/quarter-view sewer gameplay environment for The Last Host, suitable as a quality-first vertical-slice art master. It must visibly match the approved references in material richness and intentional pixel craftsmanship.
Scene/backdrop: a compact playable sewer chamber with a clear broad stone walkway, deep brick retaining walls, dark teal water channels, one iron grate, one drain outlet with a small water flow, a wooden barrel, a wooden crate, restrained pipes, moss, mineral staining, damp seams, scattered rubble, and wet reflective patches. The route must remain immediately readable and unobstructed.
Subject: the environment only; no character and no HUD.
Style/medium: premium hand-crafted 2D isometric pixel art, deliberate clustered pixels, limited earthy palette, crisp hard-edged sprite-like details, dense but controlled surface variation. Do not look like simple geometric vector art, a low-detail placeholder, a 3D render, painted concept art, or blurred pixel-filtered illustration.
Composition/framing: fixed isometric game camera, landscape 16:9, playable stone path dominates the middle, walls and water create clear depth tiers, props placed sparingly at path edges, no cutaway labels or asset-sheet layout.
Lighting/mood: strong deep upper-left key light from a warm sewer lamp, cool blue-green reflected light from water, very dark recesses, rich occlusion, dramatic but gameplay-readable contrast. The path and any future player position must read before decoration.
Color palette: umber and charcoal masonry, oxidized iron, muted moss green, dark teal water, selective warm amber highlights.
Materials/textures: individually readable worn stone blocks, aged brick mortar, damp gloss, slimy moss, rusty metal, wet wood, animated-looking water ripples implied by pixel clusters; avoid mechanically repeated marks.
Constraints: original design; no UI, no text, no numbers, no letters, no logo, no watermark, no humans, no hospital or laboratory elements, no modern signage. Keep a consistent isometric grid and coherent upper-left lighting across every surface. Quality is the highest priority.
Avoid: empty black background, sparse room, flat colors, giant simple tiles, repeated V marks, excessive noise, photorealism, smooth vector gradients, fake pixelation, shallow shadows, miniature diorama presentation, asset sheet, labels.
```

### 출력과 1차 판정

- 내장 도구 원본:
  `C:\Users\User\.codex\generated_images\019fb05c-dbbe-7213-adac-2e704271299b\call_hd6BRXLqHGH0ZRFtTqLGoDP2.png`
- 프로젝트 저장:
  `artifacts/quality-masters/environment-quality-master.png`
- 크기: `1672×941`, RGB PNG
- 상태: `비주얼/테크아트 대조 대기 — 1차 채택 후보`
- 1차 판정:
  - 석재·벽돌·물·이끼·습윤 반사·나무·금속 재질이 분리되어 읽힌다.
  - 상단 좌측의 따뜻한 광원과 청록 수로 반사가 깊은 명암을 만든다.
  - 조밀함을 유지하면서 중앙 이동 경로가 장식보다 먼저 읽힌다.
  - 텍스트·HUD·인물·범위 밖 요소가 없다.
  - 목표 목업 및 환경 통합 기준과 같은 제품군의 품질로 판단한다.
- targeted regeneration: 불필요. 명백한 자동 반려 조건을 발견하지 못했다.

## RAT-QM-01 쥐 측면 3프레임 1차 생성

### 입력 reference

- `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-integrated-art-targets/artifacts/integrated-targets/rat-integrated-target-v2.png`
  - 역할: 승인된 자연형 갈색 쥐 해부·털·실루엣·팔레트·광원 reference
- `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
  - 역할: 실제 월드에서의 자연형 쥐 가독성과 분위기 reference

### 전체 프롬프트

```text
Use case: stylized-concept
Asset type: high-quality 2D pixel-art game character animation master candidate
Input images: Image 1 is the approved natural brown rat anatomy, fur treatment, silhouette, palette, and upper-left lighting reference; Image 2 is the approved gameplay mood and actual in-world rat readability reference. Use them as visual references only, not edit targets.
Primary request: create a single clean character animation reference sheet containing exactly three separate side-view poses of the same natural brown sewer rat: idle, walk contact, and walk passing. This is a quality-first production master for later hand-built game sprites.
Scene/backdrop: one perfectly uniform dark neutral charcoal-gray background across the entire image, with no floor plane, no cast shadows, no texture, no vignette, no scene, no grid, and no dividers.
Subject: exactly three full-body rat poses, all facing right, evenly spaced left to right. Pose 1 idle: compact alert crouch with all feet grounded. Pose 2 walk contact: body stretched slightly, front and rear legs reaching opposite directions, paws contacting the same implied groundline. Pose 3 walk passing: body subtly compressed with passing legs beneath the torso. All three must unmistakably be the same animal.
Style/medium: premium hand-crafted 2D pixel art with crisp intentional pixel clusters and controlled anti-aliasing; natural zoological anatomy translated into readable game-pixel form. Not a photograph, not a 3D render, not smooth digital painting, not low-detail placeholder pixel art.
Composition/framing: horizontal landscape sheet; three isolated poses at identical scale and baseline; generous clean spacing; every nose, ear, paw, and entire tail fully inside the canvas; no overlap. Rat body is low and elongated rather than round; arched haunch flows continuously through shoulder, neck, wedge-shaped head, and tapered muzzle.
Lighting/mood: coherent upper-left warm-neutral key light in all three poses, subdued lower-right shadow, enough facial contrast to read at reduced size.
Color palette: natural warm dark brown and taupe fur, lighter gray-beige muzzle and belly, muted pink ears/nose/paws/tail, very dark glossy eye with one restrained highlight.
Anatomy and identity invariants: identical torso length, head size, ear shape, eye position, muzzle length, fur markings, tail length, body volume, palette, lighting, and pixel density in all three poses. Fine whiskers, small natural ears, slim segmented tail longer than the body, believable joint placement, four grounded paws as appropriate to each gait. Preserve a smooth back-neck-head silhouette; do not split the body into geometric blobs.
Constraints: exactly three rats and no other animals; exactly three poses in the requested order; all facing right; same baseline and scale; original design; no text, no letters, no numbers, no labels, no arrows, no grid, no borders, no logo, no watermark, no props, no UI.
Avoid: cartoon mascot, oversized head or eyes, round hamster body, mouse-like giant ears, smiling expression, rectangular head, oval blob torso, black crescent shoulder band, chunky tail, floating feet, inconsistent identity, extra limbs, duplicate tails, motion blur, fake low-resolution filter, painterly fur, realistic photographic fur, sparse flat shading. Quality and natural anatomy are the highest priority.
```

### 1차 출력 판정

- 내장 도구 원본:
  `C:\Users\User\.codex\generated_images\019fb05c-dbbe-7213-adac-2e704271299b\call_jimJlABDHHA5nyDhbxLtLl9P.png`
- 상태: `재생성`
- 사유:
  - 자연형 실루엣과 묘사 밀도는 충분했으나, 정지 프레임이 보행
    프레임보다 짧고 높게 보여 엄격한 동일 개체·체형 게이트의 경계였다.
  - 단순 규격 통과로 넘기지 않고 몸통 비율 일관성만 겨냥한 재생성을
    1회 수행했다.

## RAT-QM-02 쥐 측면 3프레임 targeted regeneration

### 입력 reference

- RAT-QM-01 1차 생성물
  - 역할: 유지할 동일 개체·렌더링 품질·팔레트·얼굴·털·꼬리·광원 기준
- `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-integrated-art-targets/artifacts/integrated-targets/rat-integrated-target-v2.png`
  - 역할: 승인된 자연형 해부와 목표 픽셀 품질 기준

### 전체 프롬프트

```text
Use case: identity-preserve
Asset type: high-quality 2D pixel-art game character animation master candidate
Input images: Image 1 is the first generated three-pose rat sheet and defines the exact approved individual, rendering quality, palette, fur treatment, head, ears, face, tail, and upper-left lighting; Image 2 is the approved natural-rat anatomy and pixel-quality reference. Create a corrected replacement sheet, not a different character.
Primary request: regenerate exactly the same natural brown rat in exactly three right-facing side-view poses—idle, walk contact, walk passing—while correcting only frame-to-frame body proportion and volume consistency.
Scene/backdrop: perfectly uniform dark neutral charcoal-gray background, no floor, no shadow, no texture, no vignette, no grid, no dividers.
Subject and pose order: left idle alert crouch, center walk contact, right walk passing. Preserve exactly the same head size, ear size, eye and muzzle placement, shoulder and haunch mass, body length, belly depth, markings, tail length and thickness, palette, lighting, and pixel density in all three poses. Torso length and torso height must remain within approximately 5% across frames. The idle pose must remain low and elongated rather than becoming shorter, taller, rounder, heavier, or hamster-like. Walking is shown by limbs, modest shoulder/hip motion, and a slight body-height shift—not by stretching or shrinking the whole animal.
Style/medium: premium hand-crafted 2D pixel art, crisp intentional clusters, natural zoological anatomy, same level of detail as the input. Preserve the smooth back–shoulder–neck–head curve, tapered muzzle, fine whiskers, slim tail, and natural fur shading.
Composition/framing: horizontal landscape sheet; three full-body poses at identical scale and a common paw groundline; even spacing; all tails and whiskers fully inside canvas.
Constraints: change only proportion consistency and gait clarity; keep identity and visual finish unchanged. Exactly three rats, all facing right, no extra animals or limbs, no overlapping poses, no text, no numbers, no labels, no borders, no grid, no UI, no props, no logo, no watermark.
Avoid: body stretch between frames, short round idle body, different weight or age, oversized head or ears, cartoon mascot, rectangular head, oval blob torso, black crescent shoulder band, chunky tail, floating paws, anatomy errors, motion blur, smooth painting, low-detail placeholder.
```

### 출력과 1차 판정

- 내장 도구 원본:
  `C:\Users\User\.codex\generated_images\019fb05c-dbbe-7213-adac-2e704271299b\call_LEYk5i5niIz9nCRQ4evI1flZ.png`
- 프로젝트 저장:
  `artifacts/quality-masters/rat-side-walk-quality-master.png`
- 크기: `1881×836`, RGB PNG
- 상태: `비주얼/테크아트 대조 대기 — 재생성 채택 후보`
- 1차 판정:
  - 자연형 등–목–머리 곡선, 주둥이, 귀, 눈, 수염, 발, 긴 가는
    꼬리와 털 명암이 목표 V2 수준으로 읽힌다.
  - 세 포즈에서 머리·귀·얼굴·팔레트·광원이 동일 개체로 유지된다.
  - contact와 passing의 발 위상이 1차본보다 명확해졌다.
  - 생성형 후보 특성상 실제 애니메이션용 체형·접지·피벗 일치는
    후속 공통 리그와 수작업 도트 재제작이 필요하다.
  - 문자·격자·프레임 테두리·추가 동물은 없다.
- 후속 상태: `총괄 blocker로 교체`
  - 좌측 idle이 중앙 contact보다 짧고 높아 `몸통 길이·높이·부피
    약 ±5%` 게이트를 충족하지 못한다는 총괄 판정을 받았다.

## HUD-QM-01 HUD 품질 마스터

### 입력 reference

- `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-integrated-art-targets/artifacts/integrated-targets/hud-integrated-target.png`
  - 역할: 승인된 황동 초상 프레임과 분절된 적색·청록 게이지 품질 reference
- `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
  - 역할: 게임 화면의 HUD 배치·크기·대비·월드 조화 reference

### 전체 프롬프트

```text
Use case: ui-mockup
Asset type: high-quality 2D pixel-art game HUD production master candidate
Input images: Image 1 is the approved aged-brass portrait frame and segmented red/teal bar quality reference; Image 2 is the approved gameplay HUD placement, scale, contrast, and world-cohesion reference. Use them only as visual references, not edit targets.
Primary request: create one polished pixel-art HUD component presentation for The Last Host containing exactly one natural brown rat portrait medallion and exactly two long status bars: a red host-health bar and a teal immune-alert bar. This is a quality-first visual master for later modular UI recreation.
Scene/backdrop: clean dark neutral charcoal background with a subtle even vignette only; no environment scene.
Subject: on the left, one octagonal/circular aged-brass portrait frame containing a close natural brown rat head-and-shoulders portrait; on the right, two parallel horizontal segmented bars of equal length, red above and teal below. Both bars are visibly filled to roughly four-fifths so material, segment divisions, and state contrast can be judged.
Style/medium: premium hand-crafted 2D pixel-art game UI; crisp intentional pixel clusters; thin readable metal edges; restrained ornamental detail; coherent with a dark isometric sewer game. Not vector UI, not glossy mobile UI, not 3D render, not a low-detail placeholder.
Composition/framing: landscape presentation with generous margins; portrait and bars fully visible and separated; consistent alignment; no extra buttons or panels.
Lighting/mood: upper-left warm highlights on aged brass, deep charcoal inner shadows, restrained metallic glints. Red and teal fills glow subtly but remain opaque pixel materials, not neon gradients.
Color palette: tarnished brass, dark iron, soot-black recesses, deep crimson-to-warm-red health fill, dark turquoise-to-pale-teal immune fill; portrait uses natural warm brown/taupe fur and muted pink ear/nose.
Materials/textures: chipped brass bevels, small rivets, oxidized spots, dark stone/iron backing, segmented inset glass or enamel fill, controlled wear without noisy clutter. The portrait must retain a natural rat silhouette and not become a mascot.
Constraints: exactly one portrait medallion and two bars; top bar red, bottom bar teal; thin strong silhouettes readable at game size; original design; no text, no letters, no numbers, no labels, no icons inside bars, no logo, no watermark, no mouse cursor, no environment, no extra UI modules.
Avoid: thick oversized frames, gold luxury ornament, ornate fantasy filigree, rounded modern mobile UI, flat vector shapes, sparse placeholder bars, smooth blurry gradients, excessive glow, cartoon rat, giant eyes, text-like accidental marks. Quality and material finish are the highest priority.
```

### 출력과 1차 판정

- 내장 도구 원본:
  `C:\Users\User\.codex\generated_images\019fb05c-dbbe-7213-adac-2e704271299b\call_LMelvu94qZfBWE4DsJxqxvfC.png`
- 프로젝트 저장:
  `artifacts/quality-masters/hud-quality-master.png`
- 크기: `1672×941`, RGB PNG
- 상태: `비주얼/테크아트 대조 대기 — 1차 채택 후보`
- 1차 판정:
  - 얇은 낡은 황동 프레임, 어두운 내부 재질, 분절된 적색·청록
    상태가 목표 HUD와 같은 제품군으로 읽힌다.
  - 자연형 쥐 초상이 마스코트형으로 변형되지 않았다.
  - 모듈 경계가 명확해 후속 초상 프레임·공용 바·채움 분해 기준으로
    사용할 수 있다.
  - 텍스트·수치·로고·추가 UI 요소가 없다.
- targeted regeneration: 불필요. 명백한 자동 반려 조건을 발견하지 못했다.

## RAT-QM-03 총괄 blocker 대응 재생성

### 입력 reference

- 당시 프로젝트 저장본 `rat-side-walk-quality-master.png`
  - 역할: 유지할 자연형 갈색 쥐의 정체성·픽셀 품질·얼굴·털·팔레트·광원
- `rat-integrated-target-v2.png`
  - 역할: 승인된 자연형 쥐 품질 기준

### 전체 프롬프트

```text
Use case: identity-preserve
Asset type: strict high-quality 2D pixel-art side-walk animation consistency master
Input images: Image 1 is the current rat master whose exact natural brown rat identity, pixel rendering quality, face, fur, palette, and lighting must be preserved; Image 2 is the approved natural-rat visual-quality reference. Do not redesign the rat.
Primary request: create a corrected three-frame side-view animation sheet for the exact same natural brown rat. The three frames are idle, walk contact, and walk passing, but FRAME STABILITY IS MORE IMPORTANT THAN POSE DRAMA.
Scene/backdrop: one completely uniform neutral dark charcoal background, identical in every area; no floor plane, shadow, gradient, texture, vignette, grid, dividers, boxes, or borders.
Subject: exactly three isolated full-body rats, left to right: idle, contact, passing. All face right. Treat the body-and-head artwork as a locked master cell duplicated three times. Keep the same low, long natural torso outer silhouette and nearly identical body bounding envelope in all three frames. Lock the spine curve, back height, chest, belly line, shoulder mass, hip mass, neck, head, muzzle, ear, eye, nose, whisker origin, tail root, total tail length and thickness. Torso width and height, head size, body volume, and total animal scale must remain within 5 percent across all three frames.
Animation changes allowed: change only the visible forepaw and hind-paw positions for idle/contact/passing; allow only a tiny one-to-two logical-pixel shoulder and pelvis rhythm and a tiny tail-height shift. Do not crouch, stretch, squash, enlarge, shrink, raise, lower, or change the outer body silhouette between frames. The same paws share one invisible groundline.
Style/medium: premium hand-crafted 2D pixel art with crisp intentional pixel clusters; natural zoological brown rat; same detailed fur, natural anatomy, smooth back-neck-head curve, tapered muzzle, small ear, glossy eye, fine whiskers, slim segmented tail, muted pink paws/nose/ear/tail, coherent upper-left light as the inputs.
Composition/framing: wide horizontal sheet; three identical-size invisible cells with equal spacing; same common groundline; full nose, feet, whiskers, and tail visible with generous margins. No overlap.
Lighting and palette invariants: identical upper-left warm-neutral lighting, shadow placement, fur markings, brown/taupe values, pink accents, eye highlight and pixel density across all three.
Constraints: exactly three rats, same individual, same scale, same body envelope, same groundline; original design; no extra limbs or tails, no motion blur, no text, labels, letters, numbers, arrows, grid, borders, UI, props, logo, or watermark.
Avoid: short high idle versus long low contact; any whole-body stretch or crouch; changing torso length or height; different weight, age, identity, head size or ear size; hamster body, mouse ears, mascot cartoon, rectangular head, blob torso, black crescent shoulder band, chunky tail, floating feet, anatomy error, smooth painterly rendering, low-detail placeholder. The three body silhouettes should look almost perfectly registered when overlaid, with leg-phase changes providing the animation.
```

### 출력 판정

- 내장 도구 원본:
  `C:\Users\User\.codex\generated_images\019fb05c-dbbe-7213-adac-2e704271299b\call_v2hIey4fjmIJEnSILORFa98h.png`
- 상태: `targeted edit`
- 사유: 중앙과 우측은 가까워졌으나 좌측이 여전히 짧고 높아 저장하지 않았다.

## RAT-QM-04 좌측 neutral targeted edit

### 전체 프롬프트

```text
Use case: precise-object-edit
Asset type: strict high-quality 2D pixel-art side-walk animation consistency master
Input images: Image 1 is the edit target, a three-frame natural brown rat animation sheet. Preserve the center contact frame and right passing frame exactly. Use the center frame as the locked body master for correcting the left idle frame.
Primary request: change only the LEFT idle rat's torso, spine, shoulder, hip, neck, head placement, belly line, and total body envelope so they match the CENTER contact rat's exact low, long body proportions and scale within 5 percent. The left rat must no longer be shorter, taller, rounder, more crouched, or heavier. Copy the center rat's body-and-head outer silhouette, body length, height, volume, head size, ear size, eye/nose placement, tail root, tail length/thickness, lighting, fur markings, palette, and pixel density into the left frame. For the left idle pose, change only the paws to a neutral grounded stance and let the tail rest with a tiny height difference. Do not change the body's outer envelope to express idle.
Invariants: keep the center and right rats unchanged; keep exactly three right-facing rats; keep the same uniform neutral dark charcoal background; keep common groundline, equal scale and spacing; preserve premium crisp hand-crafted pixel-art quality and the exact natural brown rat identity. All three bodies should register almost perfectly when overlaid, with only leg phase and tiny one-to-two-pixel shoulder/hip rhythm differing.
Constraints: no whole-body stretch, squash, crouch, size change, posture change, or weight change between frames; no extra limbs or tails; no text, labels, numbers, grid, dividers, borders, props, UI, logo, or watermark. Do not alter canvas composition or background.
Avoid: short high idle, round hamster body, different animal identity, head-size drift, ear drift, tail-length drift, floating paws, anatomy errors, smooth painting, blurred pixels, low-detail placeholder.
```

### 출력 판정

- 내장 도구 원본:
  `C:\Users\User\.codex\generated_images\019fb05c-dbbe-7213-adac-2e704271299b\call_b0GbZnFgDoGDgsbzVvNI1XfZ.png`
- 상태: `추가 targeted edit`
- 사유:
  - 좌측과 중앙의 낮고 긴 envelope가 안정됐다.
  - 정량 대조에서 우측 passing의 몸통 폭·등 최고점이 약간 크게 남아
    총괄 요청에 따라 우측만 한 번 더 맞췄다.

## RAT-QM-05 우측 passing targeted edit — 현재 최종 후보

### 전체 프롬프트

```text
Use case: precise-object-edit
Asset type: strict high-quality 2D pixel-art side-walk animation consistency master
Input images: Image 1 is the edit target, a three-frame natural brown rat walk-cycle sheet. The LEFT neutral frame and CENTER contact frame are now approved and must remain visually unchanged. Correct only the RIGHT passing frame's body envelope.
Primary request: change only the RIGHT rat from tail root through rump, spine, hip, belly, shoulder, neck and head placement so its nose-to-rump distance, back peak, ground clearance, torso height, head size, ear size, and total body volume exactly match the CENTER rat within 5 percent. The right rat currently reads slightly larger and higher: reduce and lower only its body envelope to the center frame's exact low, long silhouette while preserving the right frame's passing-leg phase. Keep its tail length/thickness and palette consistent. Do not alter the left or center rats.
Strict registration: imagine overlaying the three frames. The spine curve, rump edge, shoulder mass, belly baseline, neck, head, muzzle, ear, eye, nose and tail root must register almost perfectly. Pose differences may exist only in four paw positions, one-to-two logical-pixel shoulder/pelvis rhythm, and tiny tail height. All paws use one invisible groundline.
Style/medium: preserve the exact premium hand-crafted 2D pixel-art rendering, intentional pixel clusters, natural brown/taupe fur, muted pink details, upper-left lighting, markings, fine whiskers and slim segmented tail from the input.
Scene/background invariants: keep the same neutral dark charcoal background and entire canvas composition. Exactly three isolated right-facing rats, identical scale and equal spacing.
Constraints: change only the right body size/placement to match center; no crouch, stretch, squash, whole-animal scale drift, identity drift, extra limbs or tails; no text, labels, numbers, grid, dividers, borders, props, UI, logo, or watermark. Preserve crisp pixels; no blur or smooth painting.
```

### 출력과 자체 일관성 판정

- 내장 도구 원본:
  `C:\Users\User\.codex\generated_images\019fb05c-dbbe-7213-adac-2e704271299b\call_nramYlACylZYQ6JLO0pF8PBg.png`
- 프로젝트 저장:
  `artifacts/quality-masters/rat-side-walk-quality-master.png`
- 크기: `1881×836`, RGB PNG
- SHA-256:
  `6BB8AC96832D74988093BABEB798E2F4ADE5C96D62E07DA625C49D24B7C6CCFC`
- 상태: `비주얼/테크아트·총괄 재검토 대기 — 현재 최종 후보`
- 정량 보조 대조:
  - 동일한 색·명도 임계와 몸통 높이 band로 산출한 폭:
    `neutral 415`, `contact 404`, `passing 420 px`
  - 최대·최소 차이: `3.96%`
  - 동일 band 높이:
    `neutral 109`, `contact 114`, `passing 114 px`
  - 최대·최소 차이: `4.59%`
  - 자동 측정은 최종 QA가 아니라 직접 육안 대조를 돕는 보조값이다.
- 육안 판정:
  - 세 프레임의 nose-to-rump 거리, 등 최고점, 지면 간격, 머리·귀·눈·코,
    꼬리 비율과 조명·팔레트가 같은 낮고 긴 자연형 개체로 읽힌다.
  - neutral/contact/passing 차이는 주로 발 위상으로 제한됐다.
  - 문자·격자·테두리·추가 신체 결함이 없다.

## ChatGPT 이미지 사용 경계

- 세 출력은 사용자와 비주얼 담당자가 품질 방향을 판단할
  `고품질 제작 마스터 후보`다.
- 반복 타일, 투명 배경, 공통 캔버스·피벗, 실제 프레임 시트 또는 Unity
  Import 결과가 아니다.
- 사람의 선별, 실제 게임 규격 재제작, 독립 QA와 Unity 플레이 검증 전에는
  최종 게임 에셋으로 선언하지 않는다.
