# 시작 화면 V2 이미지 생성 로그

## 공통 정보

- 생성 작업 ID: `2026-08-06-startup-ui-visual-v2`
- 자산 묶음: PC 시작 화면 16:9 배경 후보
- 사용 도구: OpenAI 내장 `imagegen`
- 생성일: 2026-08-06
- 입력 reference: `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
- reference 역할: 분위기·아이소메트릭 가독성·논리 픽셀 밀도·제한 팔레트 판단. 장면이나 UI 복제 대상이 아님.
- 실행 방식: 후보별 독립 프롬프트 1회, 총 4회. 자동 재생성 0회.
- 출력 규격: PNG, `1672×941`, 16:9 근사 비율.

## 공통 프롬프트

```text
Full-bleed 16:9 PC game title-screen background, carefully crafted 2D isometric pixel art with crisp intentional pixel clusters and a consistent logical pixel scale. Original urban sewer scene for a game about a natural brown rat host and a strange virus. Limited charcoal, deep umber, moss green, dark teal, muted turquoise, and sparse amber palette. The rat is small, anatomically natural, four feet grounded, with a readable ear, nose, back curve, and thin tail; no clothes or anthropomorphism. The intended UI will be rendered separately. Absolutely no text, letter-like markings, numbers, signage, title, menu, buttons, HUD, frame, logo, watermark, signature, or interface. No humans, hospitals, laboratories, vaccines, ending imagery, third-party characters or brands, artist imitation, photorealism, 3D rendering, painterly smoothing, gore, body horror, giant viruses, or monsters.
```

각 후보의 전체 프롬프트는 위 공통 프롬프트에 아래 후보별 블록을 이어 붙인 값이다.

## 후보 A — 탐험의 교차로

```text
Use case: stylized-concept
Asset type: game title-screen background candidate A, “exploration crossroads”
Scene/backdrop: a broad fixed-quarter-view sewer crossroads with wet brick walkways, two intersecting dark teal channels, low masonry walls, iron pipes, restrained crates and drain grates.
Composition/framing: reserve the LEFT 40% as deliberately simple, dark, low-contrast negative space made only of broad brick and shadow shapes, with no focal props, hard edges, lamps, or bright reflections. Concentrate detail in the center-right and right. Place the small rat on the right-side walkway, facing into the unexplored crossing.
Lighting/mood: one warm amber wall lamp in the upper-right against cool canal reflections; inviting threshold and beginning of an expedition, mysterious but not frightening.
Constraints: a new composition, not a copy of any existing gameplay mockup; controlled detail and readable silhouettes.
```

- 파일: `candidates/startup-bg-candidate-a-crossroads.png`
- 파일 크기: `1,798,646 bytes`
- SHA-256: `EAA04E0D8BBB0C38CD10B84EC12745CB5AAC14E9E7884F28B806E15BB3B18488`
- 상태: `SUPERSEDED` — 사용자 판정상 쥐 중심

## 후보 B — 깊은 터널의 문턱

```text
Use case: stylized-concept
Asset type: game title-screen background candidate B, “threshold of the deep tunnel”
Scene/backdrop: a large pitch-dark circular sewer tunnel entrance in the LEFT third, a narrow wet brick maintenance walkway leading diagonally from the lower-left toward it, old masonry, sparse pipes, low railing, thin trickle of water.
Composition/framing: reserve the RIGHT 38% as broad quiet dark negative space, mostly charcoal brick shadow and subtle moisture, with no lamp, pipe junction, grate, bright reflection, or silhouette crossing it. Place the rat left-of-center on the walkway, hesitating at the edge of the tunnel.
Lighting/mood: weak cold backlight deep inside the tunnel and faint warm rim light from lower-left, strong depth bands, curiosity and unknown territory rather than horror.
Constraints: right negative space must remain practical for a vertical menu; tunnel must not dominate like a horror portal.
```

- 파일: `candidates/startup-bg-candidate-b-tunnel.png`
- 파일 크기: `1,638,461 bytes`
- SHA-256: `9799394BC59D0A6E339BD745B6ECCFCA2C541686AD50DCE02AA20A469589B4A1`
- 상태: `SUPERSEDED` — 사용자 판정상 최초 A~D 전체 쥐 중심

## 후보 C — 빗물 수직 챔버

```text
Use case: stylized-concept
Asset type: game title-screen background candidate C, “storm-water vertical chamber”
Scene/backdrop: a tall urban storm-water chamber with layered vertical brick walls, high rainwater inlet, descending oxidized pipes, narrow lower catwalk, and water falling into a shallow dark teal basin.
Composition/framing: arrange pipes and masonry around the outer edges and lower third. Preserve the CENTRAL UPPER 35% as one coherent dark low-detail negative-space panel of broad damp brick shadow and faint mist; no pipe, waterfall, lamp, grate, rat, reflection, or hard silhouette crosses it. Put the very small rat on the lower-right catwalk looking upward.
Lighting/mood: cool green-blue moisture light from the upper inlet with one restrained amber maintenance light low on one side; tense, damp, resilient, not horrific.
Constraints: fixed isometric quarter-view must remain clear despite vertical scale; clean depth tiers and controlled detail.
```

- 파일: `candidates/startup-bg-candidate-c-chamber.png`
- 파일 크기: `1,931,242 bytes`
- SHA-256: `8CF9B0E6DE9A9CBFCAFD3D712E9BDDB92F64A71C592DA228C7F8853E07220205`
- 상태: `SUPERSEDED` — 사용자 판정상 쥐 중심

## 후보 D — 격자문 너머의 기묘한 빛

```text
Use case: stylized-concept
Asset type: game title-screen background candidate D, “strange light beyond the gate”
Scene/backdrop: a closed iron sewer gate in the RIGHT third, damp old brickwork, a faint organic-looking teal-green bioluminescent film and tiny drifting motes visible only beyond the bars, modest wet walkway and shallow channel in the lower-right, restrained pipes and moss at outer edges.
Composition/framing: reserve the LEFT-CENTER 38% as broad calm dark negative space with low-contrast charcoal-brown brick planes and soft shadow only; no glow, pipe, bars, rat, debris, or focal detail intrudes. Place the rat in the lower-right foreground, curious and turning toward the glow.
Lighting/mood: cool bioluminescent rim light balanced by one tiny subdued amber lamp at far right; cute-but-uncanny viral identity, mysterious and restrained, never gruesome.
Constraints: biological presence is only an abstract subtle hint, not literal cells, magic, radiation, tentacles, mutation, or a monster.
```

- 파일: `candidates/startup-bg-candidate-d-gate.png`
- 파일 크기: `1,529,295 bytes`
- SHA-256: `313DB7B7C9DF9CD942700EBDF53A4703D4B325A051949CD50AC49B245F027FD7`
- 상태: `SUPERSEDED` — 사용자 판정상 최초 A~D 전체 쥐 중심

## 경계

- 네 파일은 생성 원본 후보이며 최종 게임 에셋이 아니다.
- A~D는 사용자 수용 피드백 뒤 모두 선별 대상에서 제외하고 생성 이력으로만 보존한다.
- 실제 UI 오버레이, 필요 해상도 리샘플링, Import 설정과 Unity 적용은 사용자 선별 후 별도 작업으로 수행한다.

## correction 2 공통 프롬프트

```text
Use case: stylized-concept
Asset type: full-bleed 16:9 PC game title-screen background
Style: carefully crafted 2D pixel art, coherent logical pixel scale, limited charcoal, deep umber, muted moss, dark teal, oxidized green, pale turquoise, and sparse dim amber palette.
Core identity: a tiny virus survives, changes, and transfers among many hosts. No host species is the protagonist or franchise mascot. Any depicted host must occupy less than 2% of the canvas, use equal visual weight, and never appear in the foreground, a portrait, a spotlight, a hero pose, or eye contact.
UI layout: preserve the specified broad dark low-detail negative space for future title and menu rendered separately.
Forbidden: any text, letters, numbers, title, menu, buttons, HUD, logo, watermark, signature, signage, border, or interface; dominant or oversized animal; central rat; human, hospital, laboratory, vaccine, ending imagery; visible disease, gore, body horror, giant virus icon, literal coronavirus symbol, medical diagram, monsters; third-party characters, brands, artist imitation; photorealism, 3D render, vector art, painterly blur.
```

각 correction 2 후보의 전체 프롬프트는 위 공통 프롬프트에 아래 후보별 블록을 이어 붙인 값이다.

## 후보 E — 숙주 이동 경로

```text
Candidate E — isometric host-transfer journey.
Create one fixed quarter-view isometric world that connects compact ecological fragments across the CENTER-RIGHT and RIGHT: damp leaf litter and roots, a shallow marsh edge, an old brick rain gutter, and only a tiny sewer-drain hint at the far end. Connect them with a subtle branching trail of tiny teal organic particles whose shapes change at transfer points, suggesting survival, infection, mutation, and movement.
Distribute four equally small host silhouettes as background details: a beetle among leaves, a mosquito above the marsh, a natural rat near the gutter, and a small bird on a distant brick edge. Each is below 2% of canvas, same contrast and detail. The evolving particle path, never an animal, is the narrative focus.
Reserve the LEFT 38% as quiet dark negative space made only of broad shadowed soil, damp stone, and restrained mist; no host, bright path, branch, pipe, reflection, or focal prop crosses it. Original composition, controlled detail, readable isometric axes.
```

- 파일: `candidates/startup-bg-candidate-e-host-path.png`
- 파일 크기: `2,053,328 bytes`
- 해상도: `1672×941`
- SHA-256: `C1CA0980417FD286DE99CAF64B0C1D963AA5CBE05B2FF81051F117CD1B97F938`
- 상태: 채택 가능 — 사용자 선별 추천 2위

## 후보 F — 동등 숙주 순환

```text
Candidate F — equal host cycle around a tiny evolving viral presence.
On the LEFT half, create a restrained circular ecosystem composition around one SMALL abstract pale-teal organic particle knot, no larger than 4% of the canvas. It is a few evolving pixel clusters, not an orb, magic spell, emblem, or scientific icon.
Around it, place four evenly spaced, equally sized, equally contrasted host silhouettes integrated into tiny habitat fragments: beetle with leaf, mosquito with water ripple, natural rat with brick gutter, small bird with branch. Each below 2% of canvas. Link the circle with a faint broken trail whose pattern changes between segments to imply host transfer and adaptation. No host receives more detail or prominence.
Reserve the RIGHT 38% as clean dark negative space with faint pixel texture; no silhouette, ring segment, bright particle, branch, reflection, or hard edge enters it. The evolving cycle is the only focus.
```

- 파일: `candidates/startup-bg-candidate-f-host-cycle.png`
- 파일 크기: `1,297,978 bytes`
- 해상도: `1672×941`
- SHA-256: `F1520513C7D2A2F2D901DA973238241F0CAA0CC1C055DED01F0A46B8F1E2E8BB`
- 상태: 선별 제외 — 중앙 원형 기호와 점선 구성이 HUD·로고·인포그래픽처럼 보여 C6 불충족

## 후보 G — 생태계 전이 파노라마

```text
Candidate G — continuous ecosystem-to-city-edge panorama.
Create a wide 2D pixel-art panorama with gently isometric ground planes across the LOWER two-thirds: damp forest floor and roots, reeds and standing water, an overgrown drainage ditch, then old brick, rain gutter, rooftop edges, and a distant storm drain. These are one continuous ecological transition, not panels. The city remains quiet and unoccupied.
Trace a subtle sequence of tiny teal particle clusters across the lower journey, fading, reappearing, and changing pixel shape between habitats. Add four equal tiny host traces: beetle in forest, mosquito over wetland, natural rat at drainage edge, small bird on distant rooftop. Each below 2%, equal detail/contrast, never centered or spotlighted. The environment transition and evolving path are the story.
Reserve the CENTRAL UPPER 35% as broad dark negative space of night sky, canopy haze, and low-contrast distant masonry. No host, branch tip, skyline spike, particle trail, lamp, antenna, pipe, or bright cloud crosses it. Mood is resilient and mysterious, not apocalyptic.
```

- 파일: `candidates/startup-bg-candidate-g-ecosystem-panorama.png`
- 파일 크기: `1,495,270 bytes`
- 해상도: `1672×941`
- SHA-256: `CD126EB635C80A94AF6308C89AF48958EBAEBFB3A4290EAA3DBCF09C78CFE884`
- 상태: 채택 가능 — 사용자 선별 추천 1위

## 후보 H — 미시·거시 이중 세계

```text
Candidate H — dual-scale microscopic life and host traces.
On the RIGHT half, create an abstract microscopic cellular environment using layered membranes, rounded chambers, branching channels, and tiny organic particles, all in cohesive 2D pixel art. Biological and strange, but not anatomical or medical.
Interweave four faint equal-weight host traces rather than full animals: beetle-shell texture, mosquito-wing vein silhouette, a short natural-rat footprint trail, and one small feather contour. Each below 2% of canvas, same subdued contrast, at separate depths. A chain of tiny pale-teal particle clusters travels through membrane channels, changing shape and palette, then echoes into the four traces. This adaptive particle chain is the focus.
Behind the micro structures, show only fragmented macro-world reflections: leaf moisture, water ripple, old brick edge, distant drain. Sewer imagery is just one trace.
Reserve the LEFT-CENTER 38% as broad quiet dark negative space with low-contrast membrane haze fading into charcoal. No host trace, cell boundary, particle, channel, reflection, or hard silhouette crosses it. Cute-but-uncanny and adaptive, never grotesque.
```

- 파일: `candidates/startup-bg-candidate-h-dual-scale.png`
- 파일 크기: `1,410,647 bytes`
- 해상도: `1672×941`
- SHA-256: `ADC260DD6C582FC083B0A310B8013E315F199CF61DDAB63C378AC10949DAA5E1`
- 상태: 선별 제외 — 숙주 이동 가독성 부족과 `Q`형 유사 기호로 C4·C6 불충족

## 사용자 승인 revision 3 공통 정보

- revision: `brief-v3-bacteriophage-food-web`
- 생성일: 2026-08-06
- 사용 도구: OpenAI 내장 `imagegen`
- 실행 방식: 후보 I~K별 독립 프롬프트 1회, 총 3회, 자동 재생성 0회
- 입력 reference 1: `docs/design/visual/references/bacteriophage-base-character-reference-v1.png` — 캐릭터 외형 불변식만 참조
- 입력 reference 2: `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png` — 2D 아이소메트릭 도트 분위기·팔레트 밀도·카메라만 참조
- 출력 규격: PNG, `1672×941`, 16:9 근사 비율

### 공통 프롬프트

```text
Use case: stylized-concept
Asset type: full-bleed 16:9 PC game title-screen background candidate for "The Last Host"
Input images: Image 1 is the bacteriophage character identity reference only; Image 2 is the 2D isometric pixel-art atmosphere, palette density, and camera reference only. Do not copy their layouts or any text/UI.
Primary request: Show a cute anthropomorphic bacteriophage as the actual protagonist moving through a living food web and transferring among hosts. The food-chain feeling must come from natural feeding, stalking, chasing, or swooping actions inside one coherent ecosystem scene, never from a chart.
Bacteriophage invariants: reinterpret Image 1 as crisp 2D pixel art; large faceted icosahedral lavender-purple capsid taking 60-70% of height, tiny rounded pink spikes, very short 2-3 segment body, exactly two short arms with white gloves, exactly two short legs, tiny phage tail, friendly clever face. No spider legs. It must remain clearly recognizable and lovable.
Movement language: the same single protagonist appears once as the solid focal character, with only two or three faint translucent purple-teal pose afterimages along one continuous curved route to communicate fast travel; afterimages are clearly motion echoes, not extra characters. The route visibly passes close to several different host animals, suggesting host-to-host transfer.
Style/medium: carefully crafted 2D isometric or quarter-view pixel-art title illustration, crisp intentional pixel clusters, coherent logical pixel scale, no 3D voxel rendering, no painterly smoothing.
Palette: charcoal, deep umber, moss, dark teal, oxidized green, with the protagonist's lavender, soft violet, pink, white and restrained pale-teal motion glow.
Mood: adventurous, clever, ecologically mysterious, cute but not childish, never horrific.
Constraints: no text, letters, numbers, title, menu, buttons, HUD, arrows, diagram lines, circles, logo, watermark, signature, signage, frame or interface. No humans, hospital, laboratory, vaccine, ending imagery, gore, disease sores, body horror, medical diagram, giant monster, third-party character, brand, or artist imitation. Keep a broad dark low-detail negative-space region for UI rendered separately.
```

### 후보 I — 대각선 먹이사슬 상승

```text
Candidate I — diagonal food-chain ascent.
Scene/backdrop: one continuous fixed-quarter-view landscape rising diagonally from a damp forest floor at lower-right, through a reed-lined stream edge, toward an old brick drainage ledge at upper-right. Show a beetle feeding on fallen fruit, a small frog poised to catch a mosquito, a natural brown rat cautiously stalking the beetle near the drain, and a small owl or night bird swooping toward the rat from the far upper-right. These actions form a readable layered food web; no animal is posed as the player hero.
Composition/framing: the solid bacteriophage is mid-leap near center-right, following an energetic curved path that threads beetle -> frog/mosquito -> rat -> bird. Reserve the LEFT 34% as broad dark soil, shadow and mist with no subject, bright trail or hard silhouette. Keep animals secondary and naturally proportioned.
Lighting: moonlit cool teal with sparse warm reflections on wet stone; strong readable silhouettes.
```

- 파일: `candidates/startup-bg-candidate-i-phage-food-chain-ascent.png`
- 파일 크기: `2,170,240 bytes`
- SHA-256: `BCBF350E0A424BB7A0EAB123B4C9BDCDB0E4D8EDAD70D9B779688BCB7E26CAF2`
- 상태: 사용자 선별 후보 — 독립 QA 전 항목 PASS, 추천 1위

### 후보 J — 다층 생태 추격

```text
Candidate J — cross-depth ecosystem chase.
Scene/backdrop: a single wide isometric wetland-to-gutter clearing with three depth layers. In the lower layer, a beetle eats a seed while a frog lunges toward a mosquito. In the middle layer, a natural rat searches beside the reeds while a small snake watches from cover. In the distant layer, a bird dives toward the snake or rat. The simultaneous actions must instantly read as creatures feeding and being hunted, a living food web rather than animals standing still.
Composition/framing: place the solid bacteriophage left-of-center, running and then airborne, with a sweeping S-curve of faint pose afterimages crossing the three depth layers and skimming past multiple hosts. Reserve the RIGHT 34% as calm dark negative space of water haze and distant masonry, completely free of focal animals and bright motion trail.
Lighting: dramatic but friendly dusk, violet protagonist pops clearly against muted organic background.
```

- 파일: `candidates/startup-bg-candidate-j-phage-ecosystem-chase.png`
- 파일 크기: `2,052,292 bytes`
- SHA-256: `37B2D7548828E1E6FFC4AEB5DC5D7C5C99511A36D508F66A9329A7DB285CDD46`
- 상태: 사용자 선별 후보 — 독립 QA 이동 잔상 과밀 PARTIAL, 추천 3위

### 후보 K — 박테리오파지의 먹이 통로 여정

```text
Candidate K — mascot-led journey through an S-shaped food corridor.
Scene/backdrop: an isometric night ecosystem whose lower two-thirds form one S-shaped natural corridor: leaf litter with a caterpillar eating a leaf and a beetle nearby; a shallow puddle where a frog hunts a mosquito; a broken brick gutter where a natural rat scavenges; a high branch where a small bird prepares to swoop. Arrange real body orientation and gaze so the predation chain is obvious without arrows or symbols.
Composition/framing: show the solid bacteriophage in the lower-left foreground at modest mascot scale, leaning forward in a confident running start. Its continuous violet-teal afterimage route winds through every food-chain beat and exits toward the upper-right, creating a strong sense of travel across hosts and habitats. Reserve the CENTRAL UPPER 34% as uninterrupted dark canopy haze and distant night sky for title/menu, no branches, animals, lights, particles or skyline crossing it.
Lighting: restrained amber rim light near the lower path, cool teal moon haze, adventurous opening-screen energy.
```

- 파일: `candidates/startup-bg-candidate-k-phage-food-corridor.png`
- 파일 크기: `2,060,905 bytes`
- SHA-256: `6D7DF093488051F0930CF8BA265C4732C6BD8F4117C47D6773BA130110CDA466`
- 상태: 사용자 선별 후보 — 독립 QA 연결된 먹이사슬 가독성 PARTIAL, 추천 2위

## revision 3 생성 경계

- I~K는 사용자 선별용 시작 화면 reference 후보이며 최종 게임 에셋이 아니다.
- 실제 UI 오버레이·리샘플링·Unity Import·씬 적용은 사용자 선택과 별도 승인 뒤 진행한다.

## revision 4 — 캐릭터·배경 통합 보정

- revision: `brief-v4-phage-background-integration-correction-1`
- 생성일: 2026-08-07 KST
- 사용 도구: OpenAI 내장 `imagegen` edit
- edit target: `candidates/startup-bg-candidate-i-phage-food-chain-ascent.png`
- 실행 방식: L~N 각 독립 edit 1회, 총 3회, 자동 재생성 0회
- 보존 불변식: 캔버스·카메라·환경·먹이사슬 동물과 행동·왼쪽 UI 여백·박테리오파지 위치·도약 방향
- 변경 범위: 박테리오파지의 채도·명도·픽셀 군집·환경광·접촉 명암과 이동 잔상만

### 공통 edit 프롬프트

```text
Use case: precise-object-edit
Asset type: 16:9 pixel-art PC game title-screen background correction
Input image: edit target. Preserve the exact canvas, camera, environment geometry, left-side dark negative space, food-chain animals, animal poses, bacteriophage pose and placement, motion direction, and overall composition.
Primary request: integrate the bacteriophage into the environment so it no longer looks pasted on or stylistically separate.
Change only the bacteriophage rendering, its motion afterimages/trail, and the immediately adjacent lighting/contact interaction. Keep all animals and the wider background unchanged.
Integration requirements: substantially reduce purple saturation and peak brightness; match the environment's coarse pixel clusters, limited value steps, texture grain and edge treatment; apply cool teal moonlight and restrained amber wet-brick bounce to the capsid, body, gloves and feet; deepen ambient occlusion; make gloves muted bone-gray; make spikes dusty muted rose; replace the bright neon rail with dim broken violet-teal pixel motes and two faint afterimages; preserve the friendly face and canonical silhouette without a bright halo.
Constraints: no new animals, objects, text, title, menu, HUD, logo, symbols, arrows, watermark, interface, layout change, crop, scene repaint, 3D rendering, smooth vector edges, or glossy toy material.
```

### 후보 L — dusty plum·moss

- variant prompt: `dark dusty plum, muted aubergine, gray-lavender, moss-green and damp-teal reflected shadows; lowest saturation; bright lavender limited to tiny eye highlights and one cap plane`
- 파일: `candidates/startup-bg-candidate-l-integrated-dusty-plum.png`
- 파일 크기: `2,139,602 bytes`
- 해상도: `1672×941`
- SHA-256: `80C4308B5856D0626028FC3EF44D8A1DDA420651F57487281C497A3D5AE9D10B`
- 상태: 미선별

### 후보 M — cool indigo·teal

- variant prompt: `deep indigo-violet, desaturated slate-purple, narrow low-intensity teal rim light; thin mist and water bounce partially veil the lower body and motion echoes; cool damp integration`
- 파일: `candidates/startup-bg-candidate-m-integrated-cool-indigo.png`
- 파일 크기: `2,228,941 bytes`
- 해상도: `1672×941`
- SHA-256: `2367293917AF31B3C54635B76552A11066481E7B875F3F9CCE6FCA74EB146FE0`
- 상태: 미선별

### 후보 N — earthy mulberry·amber

- variant prompt: `muted mulberry, brown-violet, smoky mauve, sparse soft amber brick reflection and opposing cool teal shadows; dark umber-violet outline; subtle damp capsid texture`
- 파일: `candidates/startup-bg-candidate-n-integrated-earthy-mulberry.png`
- 파일 크기: `2,154,107 bytes`
- 해상도: `1672×941`
- SHA-256: `130D354C43A91645C07C36210B3BE37FCE21C2C99A4244797D70D62D5D3F74F5`
- 상태: 미선별

### revision 4 경계

- L~N은 사용자 선별용 통합 색·광원 reference 후보이며 최종 게임 에셋이 아니다.
- 실제 UI 합성·리샘플링·2D 수작업 재제작·Unity Import와 씬 적용은 사용자 선택 후 별도 승인 대상이다.
