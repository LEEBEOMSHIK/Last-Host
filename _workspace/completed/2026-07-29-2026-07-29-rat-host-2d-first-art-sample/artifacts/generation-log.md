# 이미지 생성 로그

## 공통 기록

- 생성 작업 ID: `2026-07-29-rat-host-2d-first-art-sample`
- 담당: ChatGPT 이미지 아트 에이전트
- 사용 도구: OpenAI 내장 `image_gen` 기본 모드
- 생성일: 2026-07-29
- 생성 수량: 승인 수량과 같은 `6개`
- CLI/API fallback: 사용하지 않음
- 후처리: 없음
- 공통 상태: `미선별`
- 경계: 모든 출력은 콘셉트 후보이며 반복 타일, 투명 스프라이트,
  공통 피벗, 애니메이션 프레임 또는 최종 Unity 에셋이 아니다.

## ENV-V1

- 자산 묶음: 환경·소품
- 상태: `미선별`
- 입력 reference:
  - `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
  - 역할: 분위기·카메라·환경 밀도·가독성 생성 reference. 편집 대상
    또는 타일 원본이 아님.
- 프로젝트 출력:
  `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-first-art-sample/artifacts/ai-candidates/environment/environment-v1.png`
- 도구 반환 원본/힌트:
  `C:\Users\User\.codex\generated_images\019fab41-5411-7df0-8a92-1937a168567e\call_GezMh24swPh75Fco48mQIcK1.png`
- 파일 검증: PNG 개방 성공, `1536×1024`, `Format24bppRgb`,
  `2,542,200 bytes`
- 전체 프롬프트:

```text
Use case: stylized-concept
Asset type: concept candidate for a 2D isometric pixel-art game exploration environment
Primary request: Create an original environment-and-props concept board for a small sewer room, guided by Image 1 only as a mood, camera, density, and readability reference; do not edit or reproduce Image 1.
Input images: Image 1 is a generation reference for the target atmosphere, fixed isometric viewpoint, deliberate pixel density, gameplay readability, and restrained HUD-world harmony; it is not an edit target and not a source tile sheet.
Scene/backdrop: warm dark-brown brick sewer, wet gray-brown floor, murky teal water channels, warm brass lamp light, small amounts of restrained green moss.
Subject: show clearly separated modular visual concepts for floor, wall, water edge, pipe, barrel, crate, and drain, plus one small assembled isometric room example using the same visual language.
Style/medium: true 2D pixel art with visible deliberate pixel clusters, limited palette, crisp hard pixel edges, fixed isometric or quarter-view game perspective; not 3D, not low-poly, not painterly, not smooth vector art.
Composition/framing: an organized visual concept board with the modular component ideas readable around or beside a small assembled room example; the room should feel dense but preserve a clearly readable walkable route and gameplay silhouettes.
Lighting/mood: damp, enclosed, worn, atmospheric, with warm brass illumination against cool murky water; readable value separation.
Color palette: warm dark browns, wet gray-browns, murky teal, aged brass highlights, restrained moss green.
Constraints: concept candidate only; preserve common isometric perspective across components; no claims or visual labels implying seamless or tileable output; no text, labels, numbers, logos, signatures, or watermark; no characters; no people, hospital, laboratory, vaccine, ending scene, or out-of-scope content; no named third-party style.
Avoid: fake readable writing, UI overlay, photorealism, 3D render, excessive black void, clutter that obscures routes, neon colors, duplicated malformed props, and presentation as a final production tile set.
```

- 1차 일관성 점검:
  - 요청한 바닥·벽·수로 가장자리·배관·통·상자·배수구와 작은 방
    조립 예시가 모두 보인다.
  - 따뜻한 갈색 벽돌, 청록 수로, 황동 조명이 목표 목업과 조화된다.
  - 조립 예시의 주 이동면과 수로가 구분되고 문자·로고는 보이지 않는다.
  - 일부 화분·덩굴·잔해가 승인 핵심 세트 밖의 보조 탐색 요소로
    추가됐으므로 실제 재제작 범위에는 자동 포함하지 않는다.
- 남은 위험:
  - 개별 조각은 반복·연결 타일로 검증되지 않았고 원근과 가장자리
    연결을 수작업으로 재설계해야 한다.
  - 작은 실제 화면에서는 표면 노이즈가 이동 경로를 압도할 수 있다.

## ENV-V2

- 자산 묶음: 환경·소품
- 상태: `미선별`
- 입력 reference:
  - `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
  - 역할: 카메라·픽셀 밀도·하수도 분위기·게임플레이 가독성 생성
    reference. 편집 대상 또는 타일 원본이 아님.
- 프로젝트 출력:
  `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-first-art-sample/artifacts/ai-candidates/environment/environment-v2.png`
- 도구 반환 원본/힌트:
  `C:\Users\User\.codex\generated_images\019fab41-5411-7df0-8a92-1937a168567e\call_nUhIoGlnCGTEioNbjJDy7X4H.png`
- 파일 검증: PNG 개방 성공, `1536×1024`, `Format24bppRgb`,
  `2,168,276 bytes`
- 전체 프롬프트:

```text
Use case: stylized-concept
Asset type: alternate concept candidate for a 2D isometric pixel-art game exploration environment
Primary request: Create an original cooler, cleaner environment-and-props concept board for a small sewer room, guided by Image 1 only as a mood, camera, density, and readability reference; do not edit or reproduce Image 1.
Input images: Image 1 is a generation reference for the target fixed isometric viewpoint, deliberate pixel density, gameplay readability, and sewer-world coherence; it is not an edit target and not a source tile sheet.
Scene/backdrop: cooler charcoal-and-deep-green sewer, damp stone and dark iron, wet gray-green floor, murky teal water channels, restrained moss, sparse cool highlights with limited warm utility light.
Subject: show clearly separated modular visual concepts for floor, wall, water edge, pipe, barrel, crate, and drain, plus one small assembled isometric room example using the same visual language.
Style/medium: true 2D pixel art with visible deliberate pixel clusters, limited palette, crisp hard pixel edges, fixed isometric or quarter-view game perspective; not 3D, not low-poly, not painterly, not smooth vector art.
Composition/framing: an organized visual concept board with clean modular silhouettes around or beside a small assembled room example; emphasize stronger gameplay contrast, clearer walkable routes, and less decorative noise than the reference while retaining atmospheric depth.
Lighting/mood: cold, damp, tense, readable; high silhouette separation between walkable floor, blocking walls, water, and props; restrained warm accents only where useful.
Color palette: charcoal, oxidized dark iron, deep muted green, gray-green wet stone, murky teal water, small aged-brass accents.
Constraints: original variation; concept candidate only; preserve common isometric perspective across components; no claims or visual labels implying seamless or tileable output; no text, labels, numbers, logos, signatures, or watermark; no characters; no people, hospital, laboratory, vaccine, ending scene, or out-of-scope content; no named third-party style.
Avoid: fake readable writing, UI overlay, photorealism, 3D render, excessive ornament, clutter that obscures routes, bright neon green, duplicated malformed props, and presentation as a final production tile set.
```

- 1차 일관성 점검:
  - 같은 핵심 구성 세트와 조립 예시가 있으며 v1보다 바닥·벽·수로
    명도 단계와 실루엣이 단순하다.
  - 차콜·청록 중심의 차가운 변형과 제한된 램프가 일관되고 문자·로고는
    보이지 않는다.
  - 사다리·난간 등 보조 소품이 추가됐으므로 승인된 첫 묶음의 필수
    제작 대상으로 자동 승격하지 않는다.
- 남은 위험:
  - 반복성·충돌·가림 데이터가 없으며 벽과 수로 조각의 실제 연결 규칙을
    별도로 만들어야 한다.
  - 전반적으로 어두워 작은 실제 화면에서 쥐·수집물 대비를 별도
    시험해야 한다.

## RAT-V1

- 자산 묶음: 쥐 대표 3방향
- 상태: `미선별`
- 입력 reference:
  - `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
    - 역할: 픽셀 세계·게임플레이 크기·팔레트 생성 reference.
  - `_workspace/completed/2026-07-27-2026-07-24-rat-final-appearance-sample/artifacts/ai-concepts/rat-concept-a-natural.png`
    - 역할: 자연형 쥐 체형·색·귀·코·발·꼬리 생성 reference.
  - 두 파일 모두 편집 대상이 아님.
- 프로젝트 출력:
  `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-first-art-sample/artifacts/ai-candidates/rat/rat-3dir-v1.png`
- 도구 반환 원본/힌트:
  `C:\Users\User\.codex\generated_images\019fab41-5411-7df0-8a92-1937a168567e\call_ATfqqhhBEwidmkdTtLhl6RTi.png`
- 파일 검증: PNG 개방 성공, `1792×878`, `Format24bppRgb`,
  `1,335,864 bytes`
- 전체 프롬프트:

```text
Use case: stylized-concept
Asset type: three-view character concept candidate for a 2D isometric pixel-art game
Primary request: Create one original pixel-art concept sheet of a single natural lean brown sewer rat, using Image 1 for the target gameplay pixel world and Image 2 for body proportions, natural silhouette, fur color, ears, nose, feet, and tail; both images are generation references only, not edit targets.
Input images: Image 1 is a generation reference for deliberate pixel clusters, gameplay scale, palette harmony, and isometric-world readability. Image 2 is a generation reference for the rat's natural lean body, proportions, markings, brown fur, pink-brown ears, nose, feet, and long tail. Do not reproduce or edit either image.
Subject: exactly three views of the same one rat with identical body proportions, fur markings, ear size, muzzle length, tail thickness and length, and screen occupancy.
Composition/framing: arrange exactly three full-body views left-to-right in this order: side view, front three-quarter view, rear three-quarter view. Keep generous separation. Every rat must be fully visible with grounded feet and an uncut tail. No fourth view and no extra rat.
Style/medium: true 2D pixel art concept sheet with crisp visible deliberate pixel clusters, limited palette, simplified but natural fur grouping, hard pixel edges, readable at small gameplay scale; not 3D, not low-poly, not painterly, not smooth vector art.
Scene/backdrop: flat dark neutral presentation background, without floor texture or scenery.
Lighting/mood: restrained neutral studio-like value separation translated into pixel clusters; consistent light direction across all three views.
Color palette: natural dark-to-medium brown fur, subtly lighter underside, muted pink-brown ears/nose/feet/tail, dark eyes; harmonious with the sewer gameplay reference.
Constraints: natural lean sewer rat, not chibi, not anthropomorphic, not cute caricature; preserve the same identity in all three views; clear ears, nose, grounded feet, and tail; concept candidate only; no text, labels, arrows, numbers, logos, signature, or watermark; no people, hospital, laboratory, vaccine, ending content, or named third-party style.
Avoid: extra rats, extra views, merged bodies, duplicated limbs, floating feet, cropped tails, oversized head or eyes, hamster-like face, aggressive monster features, photorealism, 3D render, smooth airbrush, and presentation as a final sprite sheet or animation.
```

- 1차 일관성 점검:
  - 정확히 3개 전신이 측면·앞쪽 쿼터·뒤쪽 쿼터 순서로 배치됐고
    문자·로고·추가 쥐는 없다.
  - 갈색 털, 귀·코·발·긴 꼬리와 자연형 실루엣이 reference와
    유사한 방향을 유지한다.
  - 앞쪽 쿼터는 정면에 가깝고 뒤쪽 쿼터의 몸통이 다른 두 방향보다
    다소 크게 읽힌다.
- 남은 위험:
  - 털 픽셀 군집이 실제 작은 플레이 크기에는 세밀해 축소 시 노이즈와
    형태 붕괴가 생길 수 있다.
  - 동일 피벗·접지점·셀 크기·실제 8방향 일관성을 보장하지 않는다.

## RAT-V2

- 자산 묶음: 쥐 대표 3방향
- 상태: `미선별`
- 입력 reference:
  - `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
    - 역할: 픽셀 세계·게임플레이 크기·팔레트 생성 reference.
  - `_workspace/completed/2026-07-27-2026-07-24-rat-final-appearance-sample/artifacts/ai-concepts/rat-concept-a-natural.png`
    - 역할: 같은 자연형 쥐의 체형·표식·색 생성 reference.
  - 두 파일 모두 편집 대상이 아님.
- 프로젝트 출력:
  `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-first-art-sample/artifacts/ai-candidates/rat/rat-3dir-v2.png`
- 도구 반환 원본/힌트:
  `C:\Users\User\.codex\generated_images\019fab41-5411-7df0-8a92-1937a168567e\call_pQX88YRVRP5TYHHHuDGsXF2r.png`
- 파일 검증: PNG 개방 성공, `1755×896`, `Format24bppRgb`,
  `1,130,045 bytes`
- 전체 프롬프트:

```text
Use case: stylized-concept
Asset type: alternate three-view character concept candidate for a 2D isometric pixel-art game
Primary request: Create one original pixel-art concept sheet of a single natural brown sewer rat with a slightly softer, rounder, more readable silhouette and simplified fur clusters for small gameplay scale, using Image 1 for the target gameplay pixel world and Image 2 for identity, anatomy, proportions, markings, and color; both images are generation references only, not edit targets.
Input images: Image 1 is a generation reference for deliberate pixel clusters, gameplay scale, palette harmony, and isometric-world readability. Image 2 is a generation reference for the same rat's natural body proportions, markings, brown fur, ears, nose, feet, and long tail. Do not reproduce or edit either image.
Subject: exactly three views of the same one rat with identical body proportions, fur markings, ear size, muzzle length, tail thickness and length, and screen occupancy. The silhouette may be slightly softer and rounder than a lean field-rat profile, but must remain a plausible natural rat rather than a cute caricature.
Composition/framing: arrange exactly three full-body views left-to-right in this order: side view, front three-quarter view, rear three-quarter view. Keep generous separation. Every rat must be fully visible with grounded feet and an uncut tail. No fourth view and no extra rat.
Style/medium: true 2D pixel art concept sheet with crisp visible deliberate pixel clusters, limited palette, fewer and larger simplified fur clusters for readability at small gameplay scale, hard pixel edges; not 3D, not low-poly, not painterly, not smooth vector art.
Scene/backdrop: flat dark neutral presentation background, without floor texture or scenery.
Lighting/mood: restrained neutral value separation translated into simplified pixel clusters; consistent light direction across all three views.
Color palette: natural dark-to-medium brown fur, subtly lighter underside, muted pink-brown ears/nose/feet/tail, dark eyes; harmonious with the sewer gameplay reference.
Constraints: slightly softer and rounder readable silhouette, but natural and not cute caricature; not chibi and not anthropomorphic; preserve the same identity in all three views; clear ears, nose, grounded feet, and tail; concept candidate only; no text, labels, arrows, numbers, logos, signature, or watermark; no people, hospital, laboratory, vaccine, ending content, or named third-party style.
Avoid: extra rats, extra views, merged bodies, duplicated limbs, floating feet, cropped tails, oversized head or eyes, hamster-like face, plush-toy look, smile, aggressive monster features, photorealism, 3D render, smooth airbrush, overly fine fur noise, and presentation as a final sprite sheet or animation.
```

- 1차 일관성 점검:
  - 정확히 3개 전신과 요청 순서를 지키고 문자·로고·추가 쥐가 없다.
  - v1보다 등과 몸통이 둥글고 털 군집이 비교적 단순해 작은 크기용
    실루엣 탐색에는 유리하다.
  - 뒤쪽 쿼터의 머리·앞발이 옆으로 많이 돌아가 있고 몸통이 크게
    보여 다른 방향과 점유율 차이가 남는다.
- 남은 위험:
  - 둥근 몸통과 큰 눈이 축소 시 자연형보다 귀여운 인상으로 기울 수
    있어 수작업 재제작 때 머리·눈 비율을 제한해야 한다.
  - 동일 피벗·접지점·셀 크기·실제 8방향 일관성을 보장하지 않는다.

## HUD-V1

- 자산 묶음: 최소 HUD
- 상태: `미선별`
- 입력 reference:
  - `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
  - 역할: 하수도 팔레트·픽셀 밀도·월드/HUD 분리 생성 reference.
    편집 대상이 아님.
- 프로젝트 출력:
  `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-first-art-sample/artifacts/ai-candidates/hud/hud-minimal-v1.png`
- 도구 반환 원본/힌트:
  `C:\Users\User\.codex\generated_images\019fab41-5411-7df0-8a92-1937a168567e\call_btDhubrZZGMd8LQNpjTgxcep.png`
- 파일 검증: PNG 개방 성공, `1672×941`, `Format24bppRgb`,
  `1,135,076 bytes`
- 전체 프롬프트:

```text
Use case: ui-mockup
Asset type: minimal pixel HUD concept candidate for a 2D isometric sewer exploration game
Primary request: Create an original modular pixel-art HUD concept, guided by Image 1 only for world harmony, readable scale, and restrained framing; do not edit or reproduce Image 1.
Input images: Image 1 is a generation reference for the sewer palette, pixel clustering, rat-world mood, and separation of HUD from the world; it is not an edit target.
Scene/backdrop: flat dark neutral presentation background with no game scene.
Subject: exactly three modular HUD components: one rat portrait frame, one long red host-health bar, and one long teal immune-alert bar. Show the components cleanly and separately as a coherent set.
Style/medium: true 2D pixel-art UI with crisp visible deliberate pixel clusters, limited palette, hard edges, aged bronze and worn stone sewer framing; readable at small gameplay scale; not 3D, not smooth vector UI, not painterly.
Composition/framing: organized component presentation with the circular or compact rat portrait frame beside or above two distinct long horizontal bars; both bars must be long and clearly different by color; generous spacing; no extra panels or controls.
Lighting/mood: subdued aged-metal highlights, dark stone shadows, readable contrast, atmospheric but functional.
Color palette: aged bronze, dark warm stone, soot-black recesses, muted red health fill, muted teal immune-alert fill, small off-white edge highlights.
Constraints: same exact three components only; no labels, no words, no letters, no numbers, no ticks, no icons outside the rat portrait, no logos, signature, or watermark; modular readable shapes; concept candidate only; no people, hospital, laboratory, vaccine, ending content, or named third-party style.
Avoid: fake text, fantasy ornament overload, skulls, hearts, shields, ability slots, buttons, minimap, decorative background scene, bright neon, photorealism, 3D render, and presentation as final implemented UI.
```

- 1차 일관성 점검:
  - 쥐 초상 프레임, 긴 적색 게이지, 긴 청록 게이지의 정확한 3개
    구성이고 문자·숫자·추가 아이콘은 없다.
  - 황동·어두운 돌 프레임과 두 게이지의 명도 대비가 명확하다.
  - 초상 프레임의 장식 탭과 세부 묘사는 최소 HUD 기준보다 다소 크다.
- 남은 위험:
  - 실제 게임 화면 비율에 맞춘 배치, 빈/가득 참 상태, 마스크,
    9-slice, 해상도 축소를 별도로 설계해야 한다.
  - 초상 쥐가 실제 최종 방향 스프라이트와 자동으로 동일하지 않다.

## HUD-V2

- 자산 묶음: 최소 HUD
- 상태: `미선별`
- 입력 reference:
  - `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
  - 역할: 하수도 팔레트·픽셀 밀도·월드/HUD 분리 생성 reference.
    편집 대상이 아님.
- 프로젝트 출력:
  `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-first-art-sample/artifacts/ai-candidates/hud/hud-minimal-v2.png`
- 도구 반환 원본/힌트:
  `C:\Users\User\.codex\generated_images\019fab41-5411-7df0-8a92-1937a168567e\call_gbN2pyP6Rs4bLyiCeQZCTsr7.png`
- 파일 검증: PNG 개방 성공, `1672×941`, `Format24bppRgb`,
  `1,124,921 bytes`
- 전체 프롬프트:

```text
Use case: ui-mockup
Asset type: alternate minimal pixel HUD concept candidate for a 2D isometric sewer exploration game
Primary request: Create an original cleaner dark-iron-and-teal modular pixel-art HUD concept with stronger gameplay contrast and less ornament, guided by Image 1 only for world harmony, readable scale, and restrained framing; do not edit or reproduce Image 1.
Input images: Image 1 is a generation reference for the sewer palette, pixel clustering, rat-world mood, and separation of HUD from the world; it is not an edit target.
Scene/backdrop: flat dark neutral presentation background with no game scene.
Subject: exactly three modular HUD components: one rat portrait frame, one long red host-health bar, and one long teal immune-alert bar. Show the components cleanly and separately as a coherent set.
Style/medium: true 2D pixel-art UI with crisp visible deliberate pixel clusters, limited palette, hard edges, clean dark iron framing with restrained teal oxidation accents; readable at small gameplay scale; not 3D, not smooth vector UI, not painterly.
Composition/framing: organized component presentation with a compact rat portrait frame beside or above two distinct long horizontal bars; both bars must be long and clearly different by color; stronger contrast, thinner cleaner frames, generous spacing, and no extra panels or controls.
Lighting/mood: cool functional contrast, near-black iron recesses, restrained edge highlights, less ornament than an aged fantasy frame.
Color palette: dark charcoal iron, near-black recesses, restrained oxidized teal accents, strong muted red health fill, bright readable teal immune-alert fill, limited pale metal highlights.
Constraints: same exact three components only; no labels, no words, no letters, no numbers, no ticks, no icons outside the rat portrait, no logos, signature, or watermark; modular readable shapes; concept candidate only; no people, hospital, laboratory, vaccine, ending content, or named third-party style.
Avoid: fake text, bronze-heavy ornament, fantasy flourishes, skulls, hearts, shields, ability slots, buttons, minimap, decorative background scene, uncontrolled neon glow, photorealism, 3D render, and presentation as final implemented UI.
```

- 1차 일관성 점검:
  - 정확한 3개 구성과 문자·숫자·추가 아이콘 금지를 지켰다.
  - v1보다 프레임이 얇고 장식이 적으며 적색·청록 게이지 대비가
    강하다.
  - 어두운 철 프레임의 미세 청록 산화 포인트가 하수도 환경과
    연결되지만 검은 배경에서는 외곽이 일부 묻힌다.
- 남은 위험:
  - 실제 화면에서 월드 배경 위 외곽 대비, 비율, 빈 상태와
    9-slice/마스크 동작을 별도 설계·검증해야 한다.
  - 초상 쥐가 실제 최종 방향 스프라이트와 자동으로 동일하지 않다.

## 전체 1차 비교

- 환경: v1은 목표 목업의 따뜻하고 조밀한 인상에 가깝고, v2는
  경로·충돌 실루엣이 더 단순하다.
- 쥐: v1은 자연형 reference에 가깝고, v2는 작은 화면용 단순화
  후보지만 둥근 체형·큰 눈이 귀여운 방향으로 흐를 위험이 있다.
- HUD: v1은 목표 목업의 황동 장식과 가깝고, v2는 실제 플레이
  가독성과 모듈화에 유리한 출발점이다.
- 공통: 6개 모두 `미선별`이며 사용자 선별·비주얼 검토·독립 QA
  전에는 채택, 실제 에셋, 최종 타일셋/스프라이트/UI로 선언하지 않는다.
