# A01 회사 기준 숏 이미지 후보 생성 로그

- 생성 작업 ID: `OPEN-T6-A01-20260810`
- 자산 묶음: 오프닝 시퀀스 A, 회사 독립 씬의 기준 숏 `A01`
- 사용 도구: OpenAI 내장 `imagegen`
- 생성일: 2026-08-10 KST
- 상태: 사용자 선별 대기

## 승인 범위

- 후보 수: 3
- 화면: 16:9 가로형 2D 픽셀아트 모션 코믹 시네마틱 키프레임
- 목적: 실제 결과에서 회사 씬의 카메라·인물 밀도·공간 기준물을 비교
- 후속 경계: 사용자 선별 전 `A02` 및 다른 장면을 생성하지 않음

## 입력 reference

- `docs/design/narrative/opening/opening-shot-spec.md`: `A01`, `A02`, `B08` 사건·재사용 계약
- `docs/design/narrative/pixel-art-motion-comic-cinematic-guide.md`: 숏·레이어·제한 모션·텍스트 분리 기준
- `docs/design/visual/graphics-direction-management.md`: 프로젝트 2D 도트 방향과 후보/최종 에셋 경계
- 외부 작품 이미지는 입력하지 않음

## 공통 불변식

- 평온한 현대 사무실의 점심 직전 일상
- 동료들의 관계와 활기가 감염 징후 없이 읽힘
- 두 명 이상이 함께 일어나 점심 이동을 시작할 준비가 보임
- 이후 `B08`에서 같은 공간을 재방문할 수 있는 책상·창·출입부 기준물 확보
- 화면에 텍스트·자막·UI·상표·로고를 굽지 않음
- 보라색 파지 모티프, 기침, 마스크, 환자, 공포·재난 암시 금지
- 특정 작품·회사·캐릭터 스타일을 복제하지 않는 독창적 2D 픽셀 시네마틱

## 후보 기록

| 후보 ID | 출력 파일 | SHA-256 | 상태 | 1차 판단 |
| --- | --- | --- | --- | --- |
| `A01-C01` | `a01-office-base-candidate-01-observational.png` | `FDE2AA1515173D56BC05852A415145E8FD2D3E94A3798D8892E73D4D4E7ECEF3` | 미선별 | 사무실·창·출입부와 일상 온기가 잘 읽힌다. 노트북의 작은 표식이 로고처럼 보일 여지가 있어 선택 시 제거·재제작 대상이다. |
| `A01-C02` | `a01-office-base-candidate-02-character-motion.png` | `C164015C61E31052D27D0F602BAFC31C54C65976319F17A86CC48C083A15096D` | 미선별 | 표정과 점심 이동 동작이 가장 즉각적으로 읽힌다. `B08` 재방문용 공간 기준은 후보 3보다 약하다. |
| `A01-C03` | `a01-office-base-candidate-03-spatial-anchor.png` | `11277953EF27E434FC90A5553535DFA9D222FADFD6BCFE375BD77433F5BFB447` | 미선별 | 책상·창·시계·출입부 기준이 가장 안정적이며 이후 같은 구도의 공백 회수에 유리하다. 인물 감정 거리는 후보 2보다 멀다. |

세 파일은 모두 `1672×941` PNG다.

## 실제 생성 프롬프트

### `A01-C01` 관찰형

```text
Use case: stylized-concept
Asset type: opening cutscene keyframe candidate for a PC game
Primary request: Create an original high-quality 2D pixel-art cinematic keyframe for scene A01. It is lunchtime in a calm contemporary office. A small group of friendly coworkers are smiling and chatting naturally around a shared work area; two coworkers have just begun to rise from their chairs to go to lunch together, while another closes a laptop and turns toward them. The scene must communicate ordinary human warmth and a lively work relationship before any crisis.
Scene/backdrop: believable contemporary open office with desks, monitors, chairs, a sunlit window, a clear doorway or corridor leading out, a small plant, and restrained everyday clutter. Keep memorable spatial anchors—the shared desk, window, and exit—so the same framing can later be revisited with one familiar seat absent.
Style/medium: handcrafted 2D pixel art for a motion-comic cinematic; crisp intentional pixel clusters, expressive readable faces and body language, polished game-pixel illustration, layered background/midground/foreground suitable for subtle parallax. Original visual language, not an imitation of any existing game or studio.
Composition/framing: widescreen 16:9, observational wide establishing shot, cinematic frontal three-quarter perspective at human eye level with only slight elevation; not top-down, not isometric gameplay, not quarter-view gameplay. All important coworkers and the exit direction are readable. Keep safe margins for later camera movement.
Lighting/mood: warm clean midday sunlight, comfortable and optimistic, natural office ambience, no dramatic shadows.
Color palette: warm neutral office colors with restrained teal, amber, cream, and muted blue accents; balanced saturation; no dominant purple.
Constraints: no text, no captions, no UI, no logos, no trademarks, no watermark; every depicted person is an adult office worker; natural workplace clothing; no embedded panel borders.
Avoid: cough, mask, illness, infection particles, virus imagery, purple virus motifs, empty-seat emphasis, horror, dread, disaster lighting, photorealism, 3D render, voxel art, painterly blur, excessive detail noise, malformed hands or faces.
```

### `A01-C02` 인물·이동형

```text
Use case: stylized-concept
Asset type: opening cutscene keyframe candidate for a PC game
Primary request: Create an original high-quality 2D pixel-art cinematic keyframe for scene A01. Show a warm, ordinary lunchtime moment in a contemporary office: four adult coworkers share a quick laugh, one coworker pushes back a chair and stands, another lifts a small lunch bag, and the group is clearly about to leave together. The emotional focus is friendship and spontaneous human interaction, with no hint of future trouble.
Scene/backdrop: contemporary office work area beside a corridor exit, with desks, monitors, office chairs, daylight, a coat rack or shelf, and restrained everyday objects. The environment must remain spatially believable and contain a recognizable shared desk and exit that can reappear later.
Style/medium: original handcrafted 2D pixel art for a polished motion-comic cinematic; deliberate crisp pixel clusters, expressive faces, strong silhouettes and gesture acting, layered background/midground/foreground for subtle parallax. Do not imitate any existing game, studio, artist, or character design.
Composition/framing: widescreen 16:9, character-centered medium-wide cinematic shot, near eye level with a subtle three-quarter angle. Frame the laughing group prominently while still showing the shared desk and exit. Use a gentle horizontal line of action toward the doorway. Not top-down, not an isometric gameplay view, not a fixed quarter-view camera.
Lighting/mood: clean warm midday office light, friendly, energetic, comfortable, lightly comedic without exaggerating into slapstick.
Color palette: warm creams, muted greens, soft denim blue, wood and daylight tones; controlled saturation; no dominant purple.
Constraints: no text, no subtitles, no UI, no logos, no trademarks, no watermark; adults only; practical modern workplace clothing; one coherent single frame with no comic panel borders.
Avoid: cough, mask, illness, infection, particles, virus imagery, purple motifs, ominous foreshadowing, empty-seat emphasis, horror, disaster lighting, photorealistic style, 3D rendering, voxel art, blurry painted edges, malformed hands, duplicate people.
```

### `A01-C03` 공간 앵커형

```text
Use case: stylized-concept
Asset type: opening cutscene keyframe candidate for a PC game
Primary request: Create an original high-quality 2D pixel-art cinematic keyframe for scene A01. Show an entirely normal, cheerful lunchtime transition in a contemporary office. Five adult coworkers occupy a clearly organized shared desk zone; a friendly conversation has just made them laugh, two begin standing to leave for lunch, and the others turn toward them. The scene should feel lived-in, warm and human, never staged or ominous.
Scene/backdrop: a distinctive but believable office composition built around one shared desk island, a large window, a wall clock without readable numerals, a plant, and a visible doorway. Arrange the coworkers so every familiar working place currently feels occupied and connected. The desk island, one recognizable chair position, window and doorway must form strong spatial anchors for a later return to the exact room after circumstances change.
Style/medium: original premium 2D pixel-art motion-comic keyframe; crisp hand-placed pixel clusters, readable facial expressions and silhouettes, restrained texture, layered depth suitable for later parallax and limited animation. No imitation of any existing game, studio, artist, franchise or character.
Composition/framing: widescreen 16:9, spatial-anchor-first wide cinematic shot, camera near eye level from a clean side three-quarter angle. Use foreground desk details sparingly, coworkers in midground, window and exit in background. Maintain a clear stable composition that can be reused later, while still feeling like a film shot rather than gameplay. Not top-down, not isometric, not fixed quarter-view.
Lighting/mood: soft midday sunlight with gentle interior bounce light, peaceful, sociable, optimistic.
Color palette: warm cream, light wood, muted sage, soft sky blue and restrained coral accents; moderate contrast; no dominant purple.
Constraints: no text, no subtitles, no UI, no logos, no trademarks, no watermark; adults only; modern ordinary work clothing; one single cinematic frame, no panels.
Avoid: empty or abandoned-looking desk, cough, masks, sickness, infection particles, virus motifs, purple foreshadowing, danger, horror, melancholy, hard noir shadows, 3D render, voxel art, photorealism, painterly blur, overly smooth vector art, malformed anatomy, duplicate limbs.
```

## 1차 일관성 점검

- 세 후보 모두 16:9에 근접한 동일 해상도이며 텍스트·자막·UI와 감염·파지 모티프는 없다.
- 모두 게임플레이 쿼터뷰가 아니라 사람 눈높이의 2D 시네마틱 구도를 사용한다.
- 후보 사이 인물 외형은 아직 잠기지 않았으므로, 사용자 선택 뒤 선택 후보를 캐릭터·공간 앵커로 고정해야 한다.
- AI 생성 PNG는 레이어 분리본이나 최종 픽셀 에셋이 아니다. 선택 뒤 재생성·페인트오버·레이어 분해와 픽셀 QA가 필요하다.

## 남은 위험

- `A01-C01`의 노트북 표식은 로고처럼 읽힐 수 있다.
- 손가락·작은 사무용품·모니터 내부 픽셀은 최종 사용 전 수작업 검수가 필요하다.
- 선택 후보 없이 `A02`를 생성하면 인물·공간 일관성이 흔들리므로 후속 생성을 잠근다.
