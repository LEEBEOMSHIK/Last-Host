# 실제 에셋 재제작용 source master 생성 로그

## 공통 정보

- 작업 ID: `2026-07-30-rat-host-2d-production-assets-v1`
- 담당: ChatGPT 이미지 아트 에이전트
- 도구: OpenAI 내장 `imagegen`
- 생성일: 2026-07-30
- 용도: 2D 에셋 제작 담당의 크로마 제거·분리·공통 캔버스·피벗·타일
  이음·도트 페인트오버용 원본
- 최종 게임 에셋 여부: 아님
- 크로마 제거 여부: 제거하지 않음
- 주요 시각 reference:
  - `environment-quality-master.png`
  - `rat-side-walk-quality-master.png`
  - `hud-quality-master.png`
- 제외 입력: 기존 저품질 Q1 PNG, 목표 목업, 통합 시안

## SRC-ENV-01 환경 타일 source board

### 입력 reference

- `_workspace/active/2026-07-30-rat-host-2d-quality-first-vertical-slice/artifacts/quality-masters/environment-quality-master.png`
  - 역할: 하수도 석재·벽돌·수면 재질, 픽셀 밀도, 팔레트와 상단 좌측
    광원의 유일한 품질 기준

### 전체 프롬프트

```text
Use case: stylized-concept
Asset type: high-quality separated 2D isometric pixel-art environment source board for manual game-asset reconstruction
Input images: Image 1 is the only visual-quality reference. Preserve its premium sewer masonry, dark teal water, deliberate pixel clusters, material density, upper-left warm lighting, and restrained earthy palette. Do not copy the scene composition.
Primary request: create exactly six isolated, non-touching isometric environment source pieces for later manual reconstruction: (1) clean worn-stone floor diamond, (2) more weathered mossy stone floor diamond, (3) straight brick wall module with visible cap and vertical face, (4) inside corner brick wall module with coherent 90-degree isometric turn, (5) dark teal water-center diamond tile, (6) dark teal water-edge tile with a stone embankment along one side.
Scene/backdrop: perfectly flat solid #ff00ff chroma-key background filling the entire canvas. The background must be one exact uniform color with no shadows, gradients, texture, noise, vignette, reflections, floor plane, or lighting variation. Do not use #ff00ff anywhere in the assets.
Style/medium: premium hand-crafted 2D isometric pixel art, crisp intentional pixel clusters, same quality and pixel density as the approved master; no simple geometry placeholder, no 3D render, no painted concept art, no fake pixel-filter effect.
Composition/framing: clean landscape source board with two loose rows of three assets, very generous empty chroma gaps around every silhouette, no overlap, no touching edges, no grid lines, no dividers, no labels. Keep the floor and water modules on one consistent 2:1 isometric diamond axis and equal footprint. Keep straight and corner wall bricks on the same axes, brick scale, wall height, cap thickness, and base footprint.
Lighting/mood: identical upper-left warm-neutral key light baked into every piece, cool lower-right occlusion, no cast shadow or contact shadow on the chroma background.
Materials/textures: individually readable chipped brown-gray stone, dark mortar, subtle moss and mineral staining, damp highlights; water has dark teal depth and restrained ripple clusters. Variants differ in wear but remain compatible.
Constraints: exactly six pieces and no additional assets; consistent scale, axes, material, palette, outline, lighting, and logical pixel density; every asset complete and uncropped with crisp isolated edge; no scene assembly, no props, no rat, no HUD, no text, letters, numbers, arrows, symbols, watermark, logo, border, cell box, grid, or cast shadow.
Avoid: fused pieces, touching sprites, perspective mismatch, inconsistent tile footprints, uneven scale, ornate decoration, black background, magenta spill or reflected magenta on edges, soft blurred edges, alpha/transparency, empty low-detail surfaces. Quality preservation and clean separability are the highest priorities.
```

### 출력과 1차 판정

- imagegen 원본:
  `C:\Users\User\.codex\generated_images\019fb05c-dbbe-7213-adac-2e704271299b\call_zP6Jazq5P5qpMiM5lOnH6rlA.png`
- 안정 저장명:
  `artifacts/source-masters/environment-tile-source-board.png`
- 크기: `1536×1024`, RGB PNG
- SHA-256:
  `588EE90358E7B9435EAAF0A4CD1F0AF7A40952ED592B0D65FA8EAFB4700BDC01`
- 상태: `SOURCE PASS — 2D 에셋 제작 담당 후처리 입력 가능`
- 자체 검토:
  - clean/worn 바닥, straight/corner 벽, water center/edge 총 6개가
    비접촉 상태로 완전히 보인다.
  - 공통 아이소메트릭 축, 벽돌 크기, 광원과 재질 품질이 유지된다.
  - 자홍 배경 표본값은 대략 `RGB 250~253 / 1~5 / 249~252`다.
    생성 원본 특성상 정확한 단일 `255,0,255`은 아니므로 제작 담당의
    색 허용오차 기반 크로마 제거가 필요하다.
  - 반복 가능한 최종 타일이 아니며 분리·축척·이음 재제작이 필요하다.
- targeted regeneration: 없음. 융합·잘림·명백한 품질 저하가 없다.

## SRC-PROP-01 소품 source board

### 입력 reference

- `environment-quality-master.png`
  - 역할: 통·상자·배수구 재질, 픽셀 밀도, 상대 크기와 광원 기준

### 전체 프롬프트

```text
Use case: stylized-concept
Asset type: high-quality separated 2D isometric pixel-art sewer prop source board for manual game-asset reconstruction
Input images: Image 1 is the only visual-quality reference. Preserve its premium barrel, crate, drain, sewer material richness, intentional pixel clusters, earthy palette, scale relationship, and upper-left warm lighting. Do not copy the full scene.
Primary request: create exactly three isolated, non-touching isometric sewer props: (1) one upright closed wooden barrel with metal hoops, (2) one closed reinforced wooden crate, (3) one circular iron floor drain/grate viewed in the same isometric plane. Each must be a complete game-sprite source, consistent in logical pixel density and believable relative scale.
Scene/backdrop: perfectly flat solid #ff00ff chroma-key background across the whole canvas. One exact uniform color, no shadows, gradients, texture, noise, vignette, reflections, floor plane, or lighting variation. Do not use #ff00ff anywhere in the props.
Style/medium: premium hand-crafted 2D isometric pixel art, crisp deliberate pixel clusters, chipped and damp sewer wear, same quality and pixel density as the approved master; not 3D render, not smooth painting, not low-detail placeholder.
Composition/framing: one loose horizontal row with very generous chroma gaps and padding around each prop; all three fully visible and uncropped; no overlap, no touching sprites, no cell boxes, no grid, no dividers, no labels.
Lighting/mood: coherent upper-left warm-neutral highlight and lower-right form shadow baked into each prop, but absolutely no cast shadow or contact shadow on the chroma background.
Materials/textures: barrel has wet dark wood staves and oxidized iron hoops; crate has worn wood grain and dark metal corner reinforcement; drain has aged iron rim, readable grate openings, rust and damp highlights. Preserve clean silhouettes over micro-noise.
Constraints: exactly three props and nothing else; same isometric axes, scale family, outline, palette, lighting and logical pixel size; each edge crisp and separated from background; no floor tile attached to any prop, no environment assembly, no rat, no HUD, no text, letters, numbers, logo, watermark, border, arrows or cast shadow.
Avoid: fused props, open barrel, broken crate, rectangular drain, perspective mismatch, inconsistent size, black background, magenta edge spill, soft blurry edge, alpha/transparency, empty flat surfaces. Quality and separability are highest priority.
```

### 출력과 1차 판정

- imagegen 원본:
  `C:\Users\User\.codex\generated_images\019fb05c-dbbe-7213-adac-2e704271299b\call_phphet69CWo8dZgeFGxDLWlZ.png`
- 안정 저장명: `artifacts/source-masters/props-source-board.png`
- 크기: `1821×864`, RGB PNG
- SHA-256:
  `425730C4C7A3B3261AF7118E346F3344FC5E99DB8F9EABC69E86FB56EB07F0B3`
- 상태: `SOURCE PASS — 2D 에셋 제작 담당 후처리 입력 가능`
- 자체 검토:
  - 닫힌 통, 보강 상자, 원형 철제 배수구가 완전한 비접촉 실루엣으로
    분리돼 있다.
  - 목재·철재 재질과 상단 좌측 광원이 동일 제품군으로 읽힌다.
  - 자홍 배경 표본값은 환경 보드와 같은 허용오차 범위다.
- targeted regeneration: 없음. 융합·잘림·명백한 품질 저하가 없다.

## SRC-RAT-01 쥐 측면 보행 source board

### 입력 reference

- `_workspace/active/2026-07-30-rat-host-2d-quality-first-vertical-slice/artifacts/quality-masters/rat-side-walk-quality-master.png`
  - 역할: 승인된 동일 자연형 갈색 개체, 체형 envelope, 세 보행 위상,
    픽셀 품질과 광원의 유일한 기준

### 전체 프롬프트

```text
Use case: identity-preserve
Asset type: high-quality separated 2D pixel-art rat walk-cycle source board for manual sprite reconstruction
Input images: Image 1 is the approved final rat quality master and the exact identity, anatomy, three pose phases, body envelope, rendering quality, palette, lighting, and spacing reference. Preserve this same natural brown individual; do not redesign it.
Primary request: recreate exactly three isolated right-facing side-view frames of the same rat, left to right: neutral walk-cycle stance, contact, passing. Preserve the approved frame stability: the same low long body envelope, nose-to-rump distance, back peak, ground clearance, spine, shoulder, hip, head, ear, eye, nose, tail proportions and lighting in all three; pose differences only in paw phase, one-to-two logical-pixel shoulder/hip rhythm, and tiny tail height.
Scene/backdrop: perfectly flat solid #00ff00 chroma-key background across the entire canvas. The background must be one exact uniform green with no shadows, gradients, texture, noise, vignette, reflections, floor plane, or lighting variation. Do not use #00ff00 anywhere in the rats.
Style/medium: preserve the approved premium hand-crafted 2D pixel art, crisp intentional clusters, natural zoological brown rat anatomy, detailed brown/taupe fur, muted pink ear/nose/paws/tail, fine whiskers, glossy dark eye and slim segmented tail. No quality reduction.
Composition/framing: clean wide horizontal source board, three equal invisible cells with very generous green gaps, same scale and common paw groundline; all noses, whiskers, feet and complete tails fully visible and uncropped; no overlap, grid, dividers, labels or cell borders.
Lighting/mood: identical upper-left warm-neutral key and lower-right form shadow baked into each rat, but no cast shadow or contact shadow on the green background.
Constraints: exactly three rats and no other subjects; same exact individual and body envelope within 5 percent; no crouch/stretch/squash/size drift; every silhouette clean and separated; no scene, props, floor, UI, text, letters, numbers, arrows, logo, watermark, border, cast shadow or reflected green fringe.
Avoid: changed identity, short high neutral, oversized passing frame, hamster body, giant ears, cartoon mascot, different tail length, extra limbs/tails, fused paws, motion blur, smooth painting, low-detail placeholder, black or textured background, green spill. Quality, identity, and frame consistency are the highest priorities.
```

### 출력과 1차 판정

- imagegen 원본:
  `C:\Users\User\.codex\generated_images\019fb05c-dbbe-7213-adac-2e704271299b\call_0RD4RKAAgCJMIOHZs8UbW5tu.png`
- 안정 저장명: `artifacts/source-masters/rat-side-walk-source-board.png`
- 크기: `1881×836`, RGB PNG
- SHA-256:
  `F17BF99CF22FDACF3288588CC43E2721595B4B7F4A664479377FEB8ACA86D451`
- 상태: `SOURCE PASS — 2D 에셋 제작 담당 후처리 입력 가능`
- 자체 검토:
  - neutral/contact/passing 3프레임이 같은 낮고 긴 자연형 갈색 개체로
    읽히고 차이는 주로 발 위상에 제한된다.
  - 세 프레임의 전체 꼬리·수염·발이 잘리지 않고 공통 지면선에 있다.
  - 녹색 배경 표본값은 대략 `RGB 9~12 / 248~250 / 25~29`다.
    생성 원본 특성상 정확한 단일 `0,255,0`은 아니므로 제작 담당의
    색 허용오차·despill 처리가 필요하다.
  - 실제 공통 캔버스·접지선·피벗과 프레임별 도트 일치는 제작 담당이
    재정렬한다.
- targeted regeneration: 없음. 개체 변형·융합·잘림·명백한 품질 저하가 없다.

## SRC-HUD-01 HUD 모듈 source board

### 입력 reference

- `_workspace/active/2026-07-30-rat-host-2d-quality-first-vertical-slice/artifacts/quality-masters/hud-quality-master.png`
  - 역할: 낡은 황동·어두운 내부·쥐 초상·적색/청록 게이지의 유일한
    품질·재질·광원 기준

### 전체 프롬프트

```text
Use case: ui-mockup
Asset type: high-quality separated 2D pixel-art HUD module source board for manual game-asset reconstruction
Input images: Image 1 is the only visual-quality reference. Preserve its exact aged-brass material language, dark iron/stone recesses, natural brown rat portrait quality, segmented red and teal fill treatment, crisp pixel clusters, palette, and upper-left lighting. Do not copy the assembled layout.
Primary request: create exactly five isolated, non-touching HUD source modules for later manual reconstruction: (1) one EMPTY octagonal/circular aged-brass portrait frame with a fully open dark center and no rat inside, (2) one separate natural brown rat head-and-shoulders portrait interior subject with no surrounding frame, (3) one EMPTY long shared aged-brass segmented bar frame with no colored fill, (4) one separate red segmented host-health fill strip with no outer frame, (5) one separate teal segmented immune-alert fill strip with no outer frame. Each element must be visually separable and complete.
Scene/backdrop: perfectly flat solid #ff00ff chroma-key background filling the entire canvas. One exact uniform color with no shadows, gradients, texture, noise, vignette, reflections, floor plane, or lighting variation. Do not use #ff00ff anywhere in the modules.
Style/medium: premium hand-crafted 2D pixel-art game UI at the approved master quality; crisp intentional clusters, thin readable bevels, chipped tarnished brass, dark iron/stone, controlled glass/enamel highlights; no vector UI, no modern mobile UI, no 3D render, no low-detail placeholder.
Composition/framing: clean source board with generous magenta padding around each item; portrait frame and portrait subject separated on the left half, shared bar frame above the two colored fill strips on the right half; no overlap, touching silhouettes, grid, dividers, labels or cell boxes. All modules fully visible and uncropped.
Lighting/mood: identical upper-left warm highlights and lower-right form shadow baked only into each module; absolutely no cast shadow or contact shadow on the chroma background.
Materials and modularity: portrait frame has thin aged brass bevel and dark inset rim but empty center; portrait subject preserves a natural non-cartoon brown rat head/shoulders and clean cutout silhouette; shared bar frame has aged-brass ends, dark inner channel and readable segment separators but no colored cells; red and teal fills have equal dimensions and matching segment rhythm, isolated without brass frames.
Constraints: exactly five modules and nothing else; original design; consistent pixel density, scale family, brass tone, outline and lighting; no assembled HUD, no environment, no extra icons, no text, letters, numbers, labels, logo, watermark, arrows, borders, cast shadow or accidental magenta details.
Avoid: fused portrait/frame, colored fill inside shared frame, red/teal strips with different lengths, thick oversized gold frame, ornate fantasy filigree, cartoon rat, glossy modern gradient, soft blurry edge, black background, magenta spill, alpha/transparency, extra UI elements. Quality preservation and clean module separability are highest priority.
```

### 출력과 1차 판정

- imagegen 원본:
  `C:\Users\User\.codex\generated_images\019fb05c-dbbe-7213-adac-2e704271299b\call_rgkSQjXb1oscsXwNMSmmZeec.png`
- 안정 저장명: `artifacts/source-masters/hud-module-source-board.png`
- 크기: `1672×941`, RGB PNG
- SHA-256:
  `4A3CF50AB0E304BE180C8CDAFE05EDAFAACE756DF5E1494A1C1761A62B12A060`
- 상태: `SOURCE PASS — 2D 에셋 제작 담당 후처리 입력 가능`
- 자체 검토:
  - 빈 초상 프레임, 분리된 쥐 초상, 빈 공용 바 프레임, 동일 길이의
    적색·청록 채움 총 5개가 비접촉 상태로 완전히 보인다.
  - 황동·어두운 내부·쥐 초상·상태 채움 품질이 승인 마스터와 같은
    제품군으로 읽힌다.
  - 자홍 배경 표본값은 대략 `RGB 242~244 / 3~5 / 244~248`이며
    허용오차 기반 제거가 필요하다.
- targeted regeneration: 없음. 소스 융합·잘림·명백한 품질 저하가 없다.

## 공통 경계와 다음 담당

- 네 파일은 원본 imagegen RGB 크로마 source board이며 알파 파일이나
  최종 게임 에셋이 아니다.
- 2D 에셋 제작 담당이 다음을 수행한다.
  - 크로마 제거와 edge despill
  - 개별 소스 분리
  - 환경 `128×64` 후보 격자·반복 이음 재제작
  - 쥐 `256×192` 공통 캔버스·접지선·피벗·프레임 안정화
  - HUD 모듈 크기·segment rhythm·조립 상태 정리
  - 마스터와 100%·50% 직접 대조, 도트 페인트오버
- 사람의 비주얼 검토, QA와 총괄 판정 전에는 최종 에셋으로 선언하지 않는다.
