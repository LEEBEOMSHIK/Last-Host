# 생성 로그

## 콘셉트 참고안

- 상태: 생성 완료, 참고안으로 선별
- 도구: OpenAI built-in image generation (`imagegen`, 기본 built-in 모드)
- 생성일: 2026-07-16
- 참조 출처: 외부 이미지 참조 없음. 프로젝트 스타일 브리프만 사용.
- 용도: 저폴리 3D 원본의 큰 실루엣·비율·제한 팔레트 참고
- 생성 원본: `C:\Users\User\.codex\generated_images\019f695f-4904-71e0-b812-faea83ace380\exec-58c4d48f-59b6-46fb-879b-f361bfba3165.png`
- 프로젝트 보관본: `concept/rat-concept-reference-v1.png`
- 선택 이유: 큰 몸통, 긴 주둥이, 둥근 귀, 회갈색·탁한 분홍색의 구분이 브리프와 맞았다.
- 실제 3D 반영: 큰 덩어리와 팔레트만 참고했다. 사실적인 발가락·수염·고밀도 면 분할은 64×64 가독성을 위해 반영하지 않았다.
- 금지 준수: 이 이미지를 방향별 최종 프레임, 3D 메시 자동 변환, Unity 에셋으로 사용하지 않았다.

### 최종 프롬프트

```text
Use case: stylized-concept
Asset type: concept reference for a low-poly 3D game character source model, not a final directional sprite
Primary request: one original sewer rat host character concept, shown as a single three-quarter standing pose for silhouette, proportions, and palette reference
Scene/backdrop: plain warm gray studio background, no environment, no floor props
Subject: a compact rat with a large rounded low-poly body, forward tapered muzzle, two round ears, four short low legs, a long thin tail curving clearly behind, alert but slightly tense posture
Style/medium: clean stylized low-poly 3D maquette render with deliberately faceted large planes and a subtle pixel-art sensibility
Composition/framing: centered full body with generous padding; all ears, feet, muzzle, and tail fully visible; one character and one pose only
Lighting/mood: simple fixed key light from upper left, soft fill, readable three-value form separation
Color palette: limited 6–8 colors; warm taupe-gray body, lighter gray-brown belly, muted dusty pink ears nose and tail, near-black eye
Materials/textures: matte opaque surfaces, no realistic fur
Constraints: original character; useful only as a concept reference for one reproducible 3D source; no turnaround sheet, no multiple directions, no animation frames, no text, no watermark, no logo
Avoid: realistic fur, photorealism, clothing, accessories, wounds, gore, oversized teeth, complex PBR, glossy materials, detailed sewer environment, references to existing characters or artists
```

## 단일 3D 원본과 방향 렌더

- 상태: 제작 완료
- 제작 도구: Python 3.11.2 표준 라이브러리만 사용하는 결정론적 소프트웨어 렌더러
- Blender 상태: PATH에서 확인되지 않아 사용하지 않았다.
- 원본 정의: `source/render_rat_8dir.py`의 `build_rat()`
- 교환 원본: `source/rat-trial-v1.obj`, `source/rat-trial-v1.mtl`
- 원본 밀도: 정점 528개, 삼각형 924개
- 렌더 방식: 고정 직교 카메라, 고정 키 라이트, 삼각형 z-buffer, 안티앨리어싱 없음
- 출력: 투명 RGBA PNG 64×64 8장, 512×64 스프라이트 시트
- 방향 순서: `S, SW, W, NW, N, NE, E, SE`
- 공통 피벗: 좌상단 기준 `(32, 48)`
- 팔레트: 투명 제외 최대 7색
- 설정·해시: `render-settings.json`, `direction-map.csv`
- 재현 명령: artifacts 폴더에서 `python source/render_rat_8dir.py`
- 검증 명령: artifacts 폴더에서 `python source/validate_outputs.py`
- 최종 시트 SHA-256: `961ace066b18501973acde238b2521a75254a040fc2271994edfff4058db4db5`

## 생성·권리 메모

- 특정 제3자 캐릭터·상표·로고·작가 화풍을 입력하거나 모사하지 않았다.
- 프로젝트 비공개 원문이나 개인정보를 이미지 생성 입력에 넣지 않았다.
- 콘셉트 참고안은 시험 작업 기록으로만 보관하며, 최종 상업 에셋 적합성이나 독점성을 보증하지 않는다.
- 8방향 PNG는 AI가 방향별로 생성한 결과가 아니라 저장소에 포함된 단일 메시 정의와 렌더 코드의 출력이다.

## 실행 중 이슈

- 기본 샌드박스 Python 프로세스가 OBJ 쓰기에서 `PermissionError: [Errno 13]`를 반환했다.
- 사용자가 승인한 동일 렌더 명령을 격리 해제 실행해 작업 패킷 내부에만 출력했고 정상 완료했다.
- 진단 과정의 probe·중복 OBJ 파일은 artifacts 루트 내부임을 확인한 뒤 모두 제거했다.
