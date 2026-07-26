# 작업 로그

## 작업 ID

`2026-07-24-rat-final-appearance-sample`

## 로그

### 2026-07-24

- 수행 내용: 사용자 요청에 따라 외형 방향 결정과 근접 샘플 제작 범위를 승인된 작업으로 시작했다.
- 판단: 단일 권장 방향의 저폴리 3D 원본, 정지 8방향과 대표 보행 키 8방향을 제작하고 전체 제품용 64프레임·Unity 반입은 후속 승인으로 분리한다.
- 루프 게이트 상태: Blender 담당 배정 전.

## 결정 기록

- 귀여움 60 / 기묘함 40의 도시 하수도 갈색쥐 방향을 권장안으로 채택했다.

## 열린 질문

- 사용자 시각 확인 뒤 체형·얼굴·색·보행 중 수정할 항목과 최종 채택 여부를 결정한다.

## 위험과 주의점

- 작은 화면에서 귀·발·꼬리가 사라지거나 기존처럼 회색 덩어리로 읽힐 수 있다.

## 게이트 진행 상태

- 작업 배정 게이트: 통과
- 담당 산출물 게이트: 대기
- 에이전트 수행 이력 게이트: 진행 중
- QA/검증 게이트: 대기
- 총괄 관리자 게이트: 대기
- 커밋 전 차단 조건: 커밋 요청 없음

## 2026-07-24 — 문서 담당 브리프 구조화

- task의 승인된 단일 외형 방향을 제작자용 `artifacts/rat-final-appearance-brief.md`로 정리했다.
- 체형·귀·눈·주둥이·발·꼬리, 팔레트, 잰걸음 보행, 금지 요소, `128×128` 판정 질문을 분리했다.
- v5b 제작·표시 기술 기준과 기존 원본 보존, 샘플 채택·전체 프레임·Unity 반입의 후속 승인 경계를 유지했다.
- 상태판과 `CURRENT.md`를 본 작업 진행 중으로 동기화했다.
- 아트 생성, Unity·씬·ProjectSettings·Builds·기존 completed 변경, 커밋은 수행하지 않았다.

## 2026-07-24 — Blender 근접 샘플 제작

- 담당: Blender 애니메이션 테크아트 에이전트 `rat_final_art_builder`
- 승인 브리프의 도시 하수도 갈색쥐, 귀여움 60 / 기묘함 40 방향으로 새 저폴리 원본을 제작했다.
- 형태: 낮은 몸통, 분리된 머리와 짧은 삼각 주둥이, 큰 귀 2개, 작은 반사 눈, 분리된 네 발, 몸통 길이에 가까운 가는 꼬리.
- 보행: 1~8프레임 대각선 다리쌍 교대, 작은 몸통 펄스, 절제된 꼬리 반대 흔들림을 키로 넣었다. 루트 이동·회전·스케일 애니메이션은 없다.
- 실제 Blender MCP로 최종 재현 스크립트를 실행해 별도 `.blend`, 정지 8방향, 대표 보행키 `f04` 8방향을 생성했다.
- 렌더 규격: `128×128 RGBA`, 직교 카메라, 동일 조명·피벗, 공용 27색 정의/25색 실사용, 이진 알파 `[0,255]`, 무디더.
- 비교 자료: `2048×512` 2배 nearest contact sheet와 `2048×640` 4배 nearest 4방향 턴어라운드를 생성했다.
- 재현 검증: 최종 스크립트에서 `.blend`와 16개 PNG를 재생성했다. Pillow 독립 대조에서도 16개 모두 `128×128 RGBA`, 알파 `[0,255]`, 실사용 불투명색 25개로 확인됐다.
- 화면 범위: 16개 모두 캔버스 안에 있으며 E 정지/보행키의 꼬리 끝이 우측 마지막 열에 닿는다. QA에서 꼬리 끝 여백을 별도 시각 판정할 필요가 있다.
- 금지 준수: 전체 64프레임 확장, Unity 반입, UnityProject·Builds·기존 completed 수정, 커밋을 수행하지 않았다.
- 현재 판정: 담당 제작 완료 / 독립 QA 대기. 이 샘플은 최종 승인 후보이며 최종 제품 아트로 확정하지 않았다.

## 2026-07-24 — QA 꼬리 절단 프레이밍 수정

- QA가 E 정지 우측 경계 불투명 3픽셀, E 보행키 2픽셀을 실제 꼬리 절단으로 판정하고 W 좌측 1픽셀 여백도 함께 수정 요청했다.
- 외형·꼬리 길이·팔레트·포즈·조명을 바꾸지 않고 직교 카메라만 수정했다.
- 최종 카메라: `orthographic_scale 6.8`, 평행 이동 대상점 `(-0.143, 0.208, 0.70)`.
- 동일 경로의 `.blend`, 정지 8, 보행키 8, contact sheet, turnaround, frame map, render settings, palette statistics를 재생성했다.
- `render-settings.json`에 파일별 bbox 여백과 전체 최소 여백을 기록했다.
- 자체 검증: 16개 전체 최소 여백 `4px`, E 정지/보행키 `4px`, W 정지/보행키 `4px`, 경계 접촉 파일 `0`.
- 독립 Pillow 대조: 16개 `128×128 RGBA`, 알파 `[0,255]`, 공용 불투명 팔레트 25색, 최소 bbox 여백 `4px`.
- 기존 외형·포즈·팔레트 규격과 전체 16장 샘플 범위는 유지했다.
- 금지 준수: UnityProject·Builds·기존 completed·커밋 변경 없음.
- 현재 판정: QA 수정 반영 완료 / 독립 재검증 대기.

## 2026-07-24 — QA Blender FPS 설정 동기화

- QA가 저장 `.blend` Scene FPS `24`와 `render-settings.json`의 `8` 불일치를 확인했다.
- 재현 스크립트 `configure_scene()`에 `scene.render.fps = 8`을 명시했다.
- 저장 `.blend`를 열어 Scene Render FPS를 `8`로 설정하고 같은 원본 경로에 저장했다.
- 재확인: `.blend` FPS `8`, 프레임 범위 `1~8`, settings timeline FPS `8`, 스크립트 FPS `8`.
- PNG는 재렌더하지 않았으며 모델·카메라·팔레트·포즈·프레임 맵과 시각 출력은 변경하지 않았다.
- Blender 자동 백업 `.blend1`은 남기지 않고 정식 `.blend` 1개만 유지했다.
- 금지 준수: 그 외 파일, UnityProject·completed·Builds·커밋 변경 없음.

## 2026-07-24 — 사용자 자연화 수정 요구 문서화

- 사용자 피드백: 도트/저폴리 방식은 유지하되 v1의 폴리곤 삼각 면과 각진 명암이 과도하므로 더 자연스럽게 수정한다.
- v1 원본·개별 PNG·contact sheet·turnaround는 읽기 전용 비교 기준으로 보존하고, 별도 `v2-natural` 샘플로 수정한다.
- 개별 PNG는 방향·포즈별 프레임이며 contact sheet는 시각 비교표다. contact sheet를 런타임 스프라이트 시트나 atlas로 해석하지 않는다.
- 전체 64프레임, 런타임 atlas/스프라이트 시트 구성, Unity 반입은 v2-natural 사용자 채택 뒤 각각 별도 승인한다.
- 상태판과 `CURRENT.md`는 `v2 자연화 수정 중`으로 전환했다.
- 이번 기록에서는 아트·기존 산출물·Unity·커밋을 변경하지 않았다.

## 2026-07-24 — v2-natural 별도 샘플 제작

- 담당: Blender 애니메이션 테크아트 에이전트 `rat_final_art_builder`
- v1 원본·스크립트·16개 PNG·contact sheet·turnaround 20개 핵심 파일의 SHA-256을 제작 전후 대조해 모두 동일함을 확인했다.
- v2 산출물을 `artifacts/v2-natural/` 아래에만 생성했다.
- 자연화 변경:
  - 몸통·머리·배·목의 세그먼트를 적당히 늘리고 큰 형태에는 smooth normal을 사용했다.
  - 갈색 등·중간 갈색 몸통·탁한 베이지 배·부드러운 주둥이의 큰 유기적 명암 군집으로 정리했다.
  - 머리와 목의 과도한 크림색 쐐기 경계를 갈색 계열로 완화했다.
  - 주둥이를 12면의 완만한 쐐기형으로, 발을 낮고 납작한 타원형으로 수정했다.
  - 귀·작은 반사 눈·코·가는 꼬리·8방향·보행 포즈는 유지했다.
- Blender MCP로 별도 `.blend`, 정지 8방향, 보행키 `f04` 8방향, contact sheet, turnaround를 실제 생성했다.
- v1/v2 비교 자료 `rat-final-appearance-v1-v2-natural-comparison-2048.png`를 추가했다. 각 방향은 v1 다음 v2 순서다.
- 자체 검증:
  - 16개 모두 `128×128 RGBA`, 알파 `[0,255]`, 무디더.
  - 공용 팔레트 27색 정의 / 25색 실사용.
  - 전체 bbox 최소 여백 `4px`, 경계 접촉 0개.
  - `.blend` FPS `8`, 프레임 `1~8`, root transform identity, root action 없음.
  - contact `2048×512`, turnaround `2048×640`, v1/v2 비교표 `2048×512`.
- 전체 64프레임·atlas·Unity 반입은 수행하지 않았다.
- 금지 준수: v1, UnityProject, completed, Builds, 커밋 변경 없음.
- 현재 판정: v2-natural 담당 제작·자체검증 완료 / 독립 QA 대기.

## 2026-07-25 — ChatGPT 이미지 모델 외형 콘셉트 3안

- built-in ChatGPT/OpenAI image generation으로 Blender 재작업용 외형 콘셉트 3안이 추가됐다.
- A 자연형: `artifacts/ai-concepts/rat-concept-a-natural.png`
- B 귀여움·불안형: `artifacts/ai-concepts/rat-concept-b-cute-anxious.png`
- C 절제된 기묘함형: `artifacts/ai-concepts/rat-concept-c-subtle-uncanny.png`
- 목적은 사용자가 Blender 재작업의 체형·얼굴·실루엣·색감 기준을 선택하도록 돕는 것이다.
- 세 이미지는 최종 런타임 스프라이트, 방향별 프레임, 런타임 atlas 또는 Unity 반입 자산이 아니다.
- 기존 v1/v2 원본과 출력은 보존하며 최종 외형·전체 64프레임·runtime atlas·Unity 반입은 여전히 미승인이다.
- 상태판과 `CURRENT.md`를 `AI 콘셉트 3안 생성 완료, 사용자 외형 선택 대기`로 동기화했다.

## 2026-07-26 — 사용자 A-natural 선택

- 사용자 결정: `rat-concept-a-natural.png`가 세 안 중 가장 적합하다고 선택했다.
- A-natural은 다음 외형 정제본과 Blender 재작업의 기본 체형·얼굴·실루엣·색감 기준으로 사용한다.
- B `rat-concept-b-cute-anxious.png`와 C `rat-concept-c-subtle-uncanny.png`는 비교 이력으로 보존한다.
- 이 선택은 현재 AI 이미지를 최종 런타임 스프라이트나 최종 8방향 시트로 확정한 것이 아니다.
- 새 이미지 생성, Blender 수정, 전체 64프레임, runtime atlas/스프라이트 시트, Unity 반입은 시작하거나 승인 처리하지 않았다.
- 상태판과 `CURRENT.md`를 A-natural 선택 완료·후속 제작 미착수 상태로 동기화했다.

## 2026-07-26 — A2 이미지 정제와 Blender 샘플 제작 승인

- 생성 방식: built-in ChatGPT/OpenAI 이미지 편집.
- 입력 A: `artifacts/ai-concepts/rat-concept-a-natural.png`.
- 출력 A2: `artifacts/ai-concepts/rat-concept-a2-refined.png`.
- 정제 목적: 측면 체형을 기준으로 정면·사선에서도 낮고 긴 몸통을 유지하고, 주둥이 연장, 눈 축소, 털 명암 단순화를 반영한 Blender 제작 참고안을 만든다.
- A 원본과 B/C 비교안은 그대로 보존했으며 A2는 최종 런타임 스프라이트나 최종 8방향 시트가 아니다.
- 사용자 승인: A2를 기준으로 기존 원본과 분리된 Blender 원본 1개, 정지 8방향, 대표 보행 키 8방향, 비교표, 턴어라운드, 제작 설정·프레임 맵 제작을 계속 진행한다.
- 기존 v1·v2·v5b 및 A/B/C 덮어쓰기·삭제, UnityProject 변경, 전체 64프레임, runtime atlas/스프라이트 시트 구성, Unity 반입은 금지한다.
- 상태판, `CURRENT.md`, 작업 배정서와 작업 기록을 `A2 정제 완료 — Blender 8방향 정지·대표 보행 샘플 제작 승인/착수` 상태로 동기화했다.

## 2026-07-26 — A2 Blender revision-6 최종 후보 제작

- 담당: Blender 애니메이션 테크아트 에이전트 `rat_a2_blender_sample`.
- A2 이미지 `artifacts/ai-concepts/rat-concept-a2-refined.png`를 외형 기준으로 사용하되 모든 방향·포즈는 단일 Blender 원본에서 렌더했다.
- r1~r5는 시각 게이트에서 몸통 캡슐화, 쉘형 색면, 얼굴 가독성, 꼬리 고리·클리핑 문제를 확인해 완료 후보에서 제외했다. 이후 중간 바이너리는 정리하고 반려 사유만 문서 이력으로 보존한다.
- 최종 후보 경로: `artifacts/a2-blender-revision-6/`.
- r6 변경:
  - 단일 변형 UV 몸통 메시와 자연스러운 등·옆구리·배 큰 명암 군집.
  - 큰 외이·내이, 돌출된 작은 어두운 눈과 반사점, 어두운 코 끝, 밝은 턱, 방향별 수염.
  - 분리된 앞·뒷다리, 발과 발가락, 낮은 대각선 보행 키.
  - 꼬리는 몸통 외곽과 닫힌 고리를 만들지 않는 열린 후방→측면 S곡선으로 수정.
- 렌더 방식: Blender `512×512` RGBA 원본을 프레임마다 렌더한 뒤 4배 Point 샘플링으로 `128×128` 출력하고 공용 27색 팔레트에 무디더 양자화.
- 생성: 정지 8방향, 대표 보행키 `f04` 8방향, contact sheet, turnaround, frame-map, render settings, palette statistics, `.blend`, 독립 재생성 스크립트.
- 자체 기술 검증:
  - PNG `16개`, 전부 `128×128 RGBA`.
  - 알파 `[0,255]`, 공용 팔레트 실사용 `25색`, 무디더.
  - 전체 bbox 최소 여백 `4px`, 캔버스 경계 접촉 `0개`; 측면 프리뷰는 좌/우 최소 `12px/6px`.
  - contact `2048×512`, turnaround `2048×640`.
  - 전체 64프레임, runtime atlas, Unity Import·통합은 수행하지 않았다.
- 시각 자체 검토: r5의 얼굴 가독성을 유지하면서 r6에서 꼬리 고리·클리핑이 제거됐고 8방향에서 눈·귀·코·주둥이·발·꼬리 방향이 판독된다.
- 현재 판정: **Blender 담당 제작·자체 검증 완료 / 독립 QA와 총괄 검토 대기 / 사용자 최종 외형 승인 아님**.

## 2026-07-26 — r6 중립 idle 포즈 계약 수정

- 독립 QA가 기존 r6 `idle`이 보행 frame1을 사용해 FR/RL 발이 들린 상태인데 frame-map에는 `neutral_idle`로 기록된 계약 불일치를 발견했다.
- r6 외형과 walk-key frame4는 유지하고, Blender 원본에 별도 frame0 중립 포즈를 추가했다.
- frame0에서 네 발의 stride x 오프셋과 lift z 오프셋을 모두 제거하고, 앞발/뒷발 발바닥 world bbox minZ를 공통 `0.01`로 맞췄다.
- 정지 8방향은 frame0, 대표 보행키 8방향은 기존 frame4에서 다시 렌더했다.
- frame-map idle phase를 `neutral_four_paw_ground_contact`, walk phase를 `diagonal_FL_RR_lift`로 수정했다.
- `render-settings.json`에 frame0/4 포즈별 발 위치·world minZ, 접지/들림 발 목록, 방향별 idle-vs-walk 픽셀 차이를 기록했다.
- 재검증:
  - idle 4발 minZ: FL/FR/RL/RR 모두 `0.01`, 공통 접지 true.
  - walk-key frame4: FR/RL 접지, FL/RR 들림.
  - idle-vs-walk 방향별 픽셀 차이 `274~610px`, 8방향 모두 차이 있음.
  - 16개 `128×128 RGBA`, 알파 `[0,255]`, 공용 25색, 무디더, 최소 여백4, edge-touch0 유지.
- contact sheet, turnaround, frame-map, settings, palette stats, `.blend`를 수정된 idle 기준으로 재생성했다.

## 2026-07-26 — r6 최종 QA·총괄 판정

- 독립 QA 재검증: neutral idle의 네 발 공통 접지와 대표 보행 키의 대각선 보행 위상, 16개 기술 규격과 자료 동기화를 확인해 `통과`했다.
- 총괄 판정: `사용자 제시 가능 / 최종 외형 승인 후보 / 사용자 결정 필요`.
- 사용자 확인 자료:
  - A2 참고안 `artifacts/ai-concepts/rat-concept-a2-refined.png`
  - r6 비교표 `artifacts/a2-blender-revision-6/rat-final-appearance-a2-r6-contact-sheet-2048.png`
  - r6 턴어라운드 `artifacts/a2-blender-revision-6/rat-final-appearance-a2-r6-turnaround-preview-2048.png`
- 남은 시각 위험: 몸통의 캡슐/패널형 띠, 큰 귀와 어두운 얼굴 대비.
- r1~r5 중간 바이너리는 삭제·커밋 제외했고 반려 사유만 문서에 보존한다. r6와 함께 최종 후보로 제시하지 않는다.
- 사용자 최종 외형 수용 전에는 active 상태를 유지하고 완료 처리·보관하지 않는다.
- 전체 64프레임, runtime atlas/스프라이트 시트 구성, Unity 반입은 계속 미승인이다.

## 2026-07-27 — 반려 중간 바이너리 정리

- 실제 삭제: `artifacts/a2-blender/`(r1), `artifacts/a2-blender-revision-2/`~`revision-5/` 전체, `artifacts/a2-blender-revision-6/previews/`, r6 `source/*-preview.blend`.
- 정리 용량: 총 `1.38 MiB`.
- 유지: AI concepts, v1·v2-natural, A2 참고안, r6 최종 재현 스크립트와 `.blend`, idle 8, walk-key 8, contact, turnaround, settings, stats, frame-map.
- 이력 경계: r1~r5의 반려 사유는 `work-log.md`, `verification.md`, `agent-activity.md`에 문서로 남기고 삭제한 중간 바이너리는 커밋하지 않는다.
- 상태 경계: r6는 사용자 최종 외형 수용 대기이며 작업은 active 상태다. 완료 처리·보관하지 않는다.
- 미승인 유지: 전체 64프레임, runtime atlas/스프라이트 시트, Unity 반입.

## 2026-07-27 — 선별 커밋·푸시와 post-push 동기화

- 사용자 요청 cleanup: r1~r5·r6 임시 preview 중간 바이너리 `1.38 MiB` 삭제, 최종 r6와 반려 사유 문서 유지.
- 커밋 전 QA staged 감사: 통과.
- 프로젝트 총괄 내부 승인: 커밋·푸시 가능.
- 커밋: `ba883a2 art: integrate rat appearance candidate and visual gates`.
- 푸시: `origin/main`에 `5303731..ba883a2` 반영 완료. 로컬 `HEAD`와 `origin/main`은 `ba883a2679209d243d7b5d998c33ec1635883101`로 일치한다.
- post-push 상태: r6는 사용자 최종 외형 수용 대기인 active 후보이며 최종 채택·완료·보관 상태가 아니다.
- 로컬 제외 유지: `ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY`, `_workspace/previews/`; `Builds/`는 커밋·푸시에서 제외.
