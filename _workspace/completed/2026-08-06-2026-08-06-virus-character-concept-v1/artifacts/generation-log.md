# 바이러스 캐릭터 콘셉트 생성 로그

- 생성 작업 ID: `2026-08-06-virus-character-concept-v1`
- 자산 묶음: 바이러스 주인공 외형 콘셉트
- 사용 도구: OpenAI 내장 `imagegen` (`image_gen` built-in mode)
- 생성일: 2026-08-06 KST
- 생성 횟수: 정확히 3회(A/B/C 각 1회), 추가 생성 없음
- 상태: 생성·검토 완료 후 사용자 제공 외부 reference 선택으로 `SUPERSEDED`

## 입력 reference

- `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
  - 역할: 제한 팔레트·논리 픽셀 밀도·픽셀 군집 명암·어두운 분위기 reference
  - 입력 방식: 세 생성 호출 모두 `referenced_image_paths`로 전달
  - 경계: 스타일·분위기 참고 전용이며 편집·확장·장면 복제 대상이 아님
- `docs/design/game-design-summary.md`
  - 역할: 귀엽지만 기묘한 의인화 바이러스와 생존·적응·변이 정체성의 문서 reference
  - 입력 방식: 프롬프트 브리프에 반영한 문서 입력이며 이미지 입력은 아님

## 공통 생성 계약

- 동일한 비대칭 구형 바이러스 몸체, 8개의 불규칙한 짧은 둥근 돌기, 어두운 청록·보라 외피, 청록 발광 내부 핵을 요구했다.
- 같은 3/4 쿼터뷰, 아래-오른쪽 방향, 약 58% 화면 점유율, 정사각 중립 배경과 동일한 광원을 요구했다.
- 제한 팔레트와 단단한 픽셀 군집을 가진 정교한 2D 도트 캐릭터 콘셉트로 요구했다.
- 약한 시작형이지만 이동·생존·변이 가능성이 읽히도록 요구했다.
- A/B/C는 얼굴·행동성·팔다리 수준만 단계적으로 다르게 지정했다.
- 글자·라벨·로고·HUD·워터마크·숙주·인간·백혈구·병원·연구소·백신·엔딩·제3자 캐릭터 모사를 금지했다.
- 생성 결과는 비교용 콘셉트 후보이며 최종 스프라이트·애니메이션·Unity 적용본이 아니다.

## 출력 후보

### A — 최소 의인화 (`SUPERSEDED`)

- 후보 ID: A
- 파일: `artifacts/candidates/virus-concept-a-minimal.png`
- 기본 생성 원본: `C:\Users\bumci\.codex\generated_images\019fd611-526c-7312-9dbb-b52ced84998e\exec-8769bc42-9a8f-4d67-b7dc-1717d7b28716.png`
- 상태: 의인화 강도 탐색 이력 / `SUPERSEDED`
- 해상도: 1254×1254 px
- 파일 크기: 1,049,196 bytes
- SHA-256: `749D0A237968E0254B2BF8F4065C5D5AFD13E62DC706C106C9AD8F94FCE0DBF0`
- 도구 반환 힌트: 기본 생성 원본을 프로젝트 경로로 복사하고 기본 원본은 보존할 것
- 전체 실제 프롬프트:

```text
Use case: stylized-concept
Asset type: The Last Host virus protagonist character concept candidate A, comparison-only raster concept, not a final sprite or Unity asset
Input image: the supplied rat-host gameplay mockup is STYLE AND MOOD REFERENCE ONLY for restricted palette, logical pixel density, clustered pixel shading, and moody light; do not edit, reproduce, extend, or copy its scene, rat, HUD, composition, or objects
Scene/backdrop: one isolated character centered on a simple neutral very dark desaturated slate-teal square backdrop, subtle flat oval grounding patch only, no environment, no props
Subject identity shared with the A/B/C set: a weak early-stage virus protagonist; asymmetrical near-spherical body; exactly 8 irregular short rounded protrusions distributed around the body; dark teal and muted purple outer membrane; one clearly visible cyan-teal glowing inner core through a small translucent membrane area; cute but uncanny, biologically strange, capable of survival and future mutation
Candidate A — minimal anthropomorphism: mostly organism-like, no arms, no legs, no clothing; locomotion suggested only by a slight directional lean and two tiny membrane nubs touching the ground; facial cue limited to two extremely small dim eye-like specks embedded in the membrane, no mouth, no eyebrows, no human gesture
Style/medium: polished 2D pixel-art character concept with deliberate hard-edged pixel clusters, limited color ramps, crisp readable silhouette, no smooth vector curves, no painterly brushwork, no 3D render, no anti-aliased illustration look
Composition/framing: fixed 3/4 quarter view from slightly above, facing down-right; full body visible; character occupies about 58% of canvas height; generous equal padding; single character only
Lighting/mood: soft cyan core glow from within, restrained upper-left cool rim light, compact shadows; vulnerable and curious rather than heroic or aggressive
Color palette: near-black slate, dark teal, muted violet, desaturated cyan-teal glow, at most a few pale cyan highlight pixels; keep saturation restrained
Constraints: preserve all shared identity details; readable at small gameplay scale; no anatomy other than specified; no extra objects or particles
Avoid: text, letters, numbers, labels, logo, HUD, icon frame, watermark, border, host animal, rat, human, white blood cell, hospital, laboratory, syringe, vaccine, ending imagery, slime monster, generic blob, skull, teeth, weapon, gore, third-party character, franchise motif, or imitation of any living artist
```

- 1차 판정: 큰 비대칭 구형 몸체, 어두운 외피, 발광 핵, 미세한 눈만으로 최소 의인화가 가장 분명하다. 외곽 돌기는 7개의 뚜렷한 자루형 돌기와 하단 접지 돌기/막 돌기의 구분이 모호해 정확한 8개 판독은 불안정하다.

### B — 반의인화 (`SUPERSEDED`)

- 후보 ID: B
- 파일: `artifacts/candidates/virus-concept-b-semi-anthropomorphic.png`
- 기본 생성 원본: `C:\Users\bumci\.codex\generated_images\019fd611-526c-7312-9dbb-b52ced84998e\exec-b5d07739-6350-4612-92aa-1e019d31e711.png`
- 상태: 의인화 강도 탐색 이력 / `SUPERSEDED`
- 해상도: 1254×1254 px
- 파일 크기: 1,065,899 bytes
- SHA-256: `EDBDA8DB3E0335D2AC3EB85B3051FF1885C882C0DED934341AC00EA5063B14CE`
- 도구 반환 힌트: 기본 생성 원본을 프로젝트 경로로 복사하고 기본 원본은 보존할 것
- 전체 실제 프롬프트:

```text
Use case: stylized-concept
Asset type: The Last Host virus protagonist character concept candidate B, comparison-only raster concept, not a final sprite or Unity asset
Input image: the supplied rat-host gameplay mockup is STYLE AND MOOD REFERENCE ONLY for restricted palette, logical pixel density, clustered pixel shading, and moody light; do not edit, reproduce, extend, or copy its scene, rat, HUD, composition, or objects
Scene/backdrop: one isolated character centered on a simple neutral very dark desaturated slate-teal square backdrop, subtle flat oval grounding patch only, no environment, no props
Subject identity shared with the A/B/C set: a weak early-stage virus protagonist; asymmetrical near-spherical body; exactly 8 irregular short rounded protrusions distributed around the body; dark teal and muted purple outer membrane; one clearly visible cyan-teal glowing inner core through a small translucent membrane area; cute but uncanny, biologically strange, capable of survival and future mutation
Candidate B — semi-anthropomorphic: retain the organism body as dominant; two small readable cyan eyes embedded in the membrane and one short subtle mouth notch; two very short flexible membrane arms without fingers and two tiny grounded feet; cautious forward-leaning survival pose, one arm slightly raised as if sensing a path; no clothing, no human proportions, no heroic stance
Style/medium: polished 2D pixel-art character concept with deliberate hard-edged pixel clusters, limited color ramps, crisp readable silhouette, no smooth vector curves, no painterly brushwork, no 3D render, no anti-aliased illustration look
Composition/framing: fixed 3/4 quarter view from slightly above, facing down-right; full body visible; character occupies about 58% of canvas height; generous equal padding; single character only
Lighting/mood: soft cyan core glow from within, restrained upper-left cool rim light, compact shadows; vulnerable, alert, and endearing but strange
Color palette: near-black slate, dark teal, muted violet, desaturated cyan-teal glow, at most a few pale cyan highlight pixels; keep saturation restrained
Constraints: preserve all shared identity details; readable at small gameplay scale; appendages remain secondary to virus silhouette; no extra objects or particles
Avoid: text, letters, numbers, labels, logo, HUD, icon frame, watermark, border, host animal, rat, human, white blood cell, hospital, laboratory, syringe, vaccine, ending imagery, slime monster, generic blob, skull, teeth, weapon, gore, third-party character, franchise motif, or imitation of any living artist
```

- 1차 판정: 눈·입·양팔·양발과 감지 동작이 추가되어 A보다 행동성이 명확하고 C보다 표정이 절제되어 반의인화 단계로 읽힌다. 외곽 돌기가 요구한 8개보다 적게 보이며 팔·다리가 돌기 체계를 대체해 공통 실루엣 일관성은 부분 충족이다.

### C — 마스코트형 (`SUPERSEDED`)

- 후보 ID: C
- 파일: `artifacts/candidates/virus-concept-c-mascot.png`
- 기본 생성 원본: `C:\Users\bumci\.codex\generated_images\019fd611-526c-7312-9dbb-b52ced84998e\exec-a8cada8f-7266-447a-94d2-e23f73549ee5.png`
- 상태: 의인화 강도 탐색 이력 / `SUPERSEDED`
- 해상도: 1254×1254 px
- 파일 크기: 1,087,163 bytes
- SHA-256: `15D1B4187F12C64D582264742E79FA540E57E199BC183CB3EEAB8C729503A2AE`
- 도구 반환 힌트: 기본 생성 원본을 프로젝트 경로로 복사하고 기본 원본은 보존할 것
- 전체 실제 프롬프트:

```text
Use case: stylized-concept
Asset type: The Last Host virus protagonist character concept candidate C, comparison-only raster concept, not a final sprite or Unity asset
Input image: the supplied rat-host gameplay mockup is STYLE AND MOOD REFERENCE ONLY for restricted palette, logical pixel density, clustered pixel shading, and moody light; do not edit, reproduce, extend, or copy its scene, rat, HUD, composition, or objects
Scene/backdrop: one isolated character centered on a simple neutral very dark desaturated slate-teal square backdrop, subtle flat oval grounding patch only, no environment, no props
Subject identity shared with the A/B/C set: a weak early-stage virus protagonist; asymmetrical near-spherical body; exactly 8 irregular short rounded protrusions distributed around the body; dark teal and muted purple outer membrane; one clearly visible cyan-teal glowing inner core through a small translucent membrane area; cute but uncanny, biologically strange, capable of survival and future mutation
Candidate C — mascot-like anthropomorphism: retain the virus sphere and all eight protrusions as the dominant silhouette; larger expressive cyan eyes embedded in the membrane, a tiny determined closed mouth, two short rounded membrane arms with simple mitten-like tips, two short grounded legs; lively cautious stepping pose that suggests movement and agency; charming and readable but still strange and visibly weak, not triumphant; no clothing and no human body proportions
Style/medium: polished 2D pixel-art character concept with deliberate hard-edged pixel clusters, limited color ramps, crisp readable silhouette, no smooth vector curves, no painterly brushwork, no 3D render, no anti-aliased illustration look
Composition/framing: fixed 3/4 quarter view from slightly above, facing down-right; full body visible; character occupies about 58% of canvas height; generous equal padding; single character only
Lighting/mood: soft cyan core glow from within, restrained upper-left cool rim light, compact shadows; endearing, curious, and determined with a lingering uncanny biological quality
Color palette: near-black slate, dark teal, muted violet, desaturated cyan-teal glow, at most a few pale cyan highlight pixels; keep saturation restrained
Constraints: preserve all shared identity details; readable at small gameplay scale; mascot traits must not erase the virus silhouette; no extra objects or particles
Avoid: text, letters, numbers, labels, logo, HUD, icon frame, watermark, border, host animal, rat, human, white blood cell, hospital, laboratory, syringe, vaccine, ending imagery, slime monster, generic blob, skull, teeth, weapon, gore, superhero pose, third-party character, franchise motif, or imitation of any living artist
```

- 1차 판정: 큰 눈·표정·양팔·큰 보행 발로 마스코트 단계가 가장 명확하다. 바이러스 구형 외곽과 발광 핵은 남지만 돌기 수가 요구보다 적게 읽히고, 큰 눈과 굵은 발 때문에 “약한 시작형”보다 결연한 상업 마스코트 인상이 강해질 위험이 있다.

## 원본 직접 확인과 일관성 점검

- 확인 방식: 프로젝트에 복사한 세 PNG를 `view_image` 원본 해상도 모드로 전수 확인했다.
- C1 / 의인화 단계: PASS. A 최소 의인화, B 반의인화, C 마스코트형이 얼굴·팔다리·행동성 증가로 구분된다.
- C2 / 2D 도트·제한 팔레트: PARTIAL PASS. 세 후보 모두 어두운 청록·보라·청록 핵과 픽셀 군집으로 읽힌다. 다만 1254×1254 콘셉트 일러스트이므로 실제 논리 픽셀 격자·스프라이트 축소 가독성은 아직 검증되지 않았다.
- C3 / 공통 정체성: PARTIAL PASS. 구형 외피·둥근 돌기·청록 핵·팔레트·배경·광원은 유지된다. 정확한 8개 돌기, 체형 비대칭, 핵의 위치·비율, 팔다리와 돌기의 구분은 B/C에서 흔들린다.
- C4 / 귀엽지만 기묘한 약한 시작형·이동/생존/변이: PARTIAL PASS. A는 약하고 기묘하며, B는 이동·생존 행동성이 가장 균형 있게 읽힌다. C는 이동성과 캐릭터성은 강하지만 약한 시작형보다 결연한 마스코트로 기울 수 있다. 세 후보 모두 발광 핵과 유기 표면으로 변이 가능성은 읽힌다.
- C5 / 금지 요소: PASS. 원본 전수 검사에서 글자·로고·HUD·워터마크·숙주·쥐·인간·백혈구·병원·연구소·백신·엔딩·제3자 캐릭터를 발견하지 못했다.
- C6 / 추적성: PASS(본 로그 기준). 도구·날짜·reference 역할·전체 프롬프트·기본 원본·프로젝트 출력 경로·해상도·bytes·SHA-256을 기록했다.
- C7 / 최종 에셋 경계: PASS. 세 파일은 사용자 비교용 콘셉트 후보이며 최종 스프라이트·애니메이션·Unity 적용본으로 선언하지 않는다.

## 비교 가능성

- 같은 정사각 배경, 유사한 화면 점유율, 동일한 3/4 방향, 팔레트, 내부 발광 핵을 유지해 의인화 강도 비교는 가능하다.
- A→B→C 순으로 눈 크기, 입, 팔다리, 포즈의 행동성이 단계적으로 증가해 사용자 선별 질문에 사용할 수 있다.
- 단, B/C의 몸체 비율과 돌기 수가 A와 완전히 같지 않으므로 “의인화 요소만 바뀐 엄밀한 통제 비교”는 아니다. 선택 후 기준 체형을 별도 일관성 시트로 재설계해야 한다.

## 남은 위험

- 세 후보 모두 게임 규격 도트 스프라이트가 아니라 고해상도 도트풍 콘셉트이므로 실제 플레이 크기 축소 시 눈·핵·표면 정보가 뭉개질 수 있다.
- 요구한 8개 돌기와 동일 체형이 생성 결과에서 정확히 고정되지 않았다.
- B/C의 팔다리는 방향별 프레임에서 돌기와 겹치거나 체형·피벗을 흔들 수 있다.
- C는 일반 귀여운 몬스터 또는 상업 마스코트처럼 보일 위험이 가장 높다.
- A/B/C는 비주얼/테크아트·독립 QA·총괄 검토를 거쳤으나, 사용자가 별도 외부 reference를 선택했으므로 탐색 이력으로만 보존하며 최종 에셋으로 사용할 수 없다.

## 사용자 선택 correction 1

- 사용자는 A/B/C 중 하나를 그대로 채택하지 않고 `docs/references/images/image.png`와 후속 영문 프롬프트를 기본 박테리오파지 기준으로 선택했다.
- 선택 원문과 프로젝트 적용 해석: `artifacts/user-selected-reference.md`
- A/B/C PNG와 이 로그의 생성 정보는 삭제하지 않고 의인화 강도 탐색 이력으로 보존한다.
- correction 1에서는 새 이미지를 생성하지 않는다.
