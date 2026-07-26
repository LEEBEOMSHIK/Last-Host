# 핸드오프 기록

## 작업 ID

`2026-07-24-rat-final-appearance-sample`

## 최신 사용자 요청

A2 기반 Blender r6와 neutral idle 수정본의 QA·총괄 통과 결과를 확인하고 최종 외형 수용 여부를 결정한다.

## 현재 상태

- 상태: r6 QA·총괄 통과 — 사용자 최종 외형 수용 대기, active 유지
- 여기서 멈춤: r6 neutral idle 수정 후 QA를 통과했고 총괄이 사용자 제시 가능한 최종 외형 승인 후보로 판정했다.
- 다음 세션의 첫 목표: 세 개의 사용자 확인 자료를 제시하고 r6 수용·수정·반려 결정을 받는다.

## 먼저 읽을 파일

1. A2 참고안 `artifacts/ai-concepts/rat-concept-a2-refined.png`
2. r6 비교표 `artifacts/a2-blender-revision-6/rat-final-appearance-a2-r6-contact-sheet-2048.png`
3. r6 턴어라운드 `artifacts/a2-blender-revision-6/rat-final-appearance-a2-r6-turnaround-preview-2048.png`

## 건드리면 안 되는 기존 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`
- `_workspace/previews/`
- `Builds/`
- 기존 completed v1~v5b 원본과 출력

## 사용자 승인 경계

- 검토 완료: A2 기반 r6 제작과 neutral idle 수정, 독립 QA 통과, 총괄 `사용자 제시 가능 / 최종 외형 승인 후보 / 사용자 결정 필요`.
- 사용자 확인 위험: 몸통의 캡슐/패널형 띠, 큰 귀와 어두운 얼굴 대비.
- 정리 이력: r1~r5 중간 바이너리는 삭제·커밋 제외했고 최종 후보가 아니다. 반려 사유만 문서로 보존한다.
- A2 생성 기록: 2026-07-26, built-in ChatGPT/OpenAI 이미지 편집, 입력 `artifacts/ai-concepts/rat-concept-a-natural.png`, 출력 `artifacts/ai-concepts/rat-concept-a2-refined.png`.
- 정제 목적: 측면 체형 기준, 정면·사선의 낮고 긴 몸통, 주둥이 연장, 눈 축소, 털 명암 단순화를 Blender 원본에 일관되게 옮기기 위한 참고안.
- 승인 필요: 이번 Blender 샘플의 최종 외형 채택, 최종 제품용 8방향 시트, 전체 64프레임, 런타임 atlas/스프라이트 시트 구성, Unity 반입.

## 프레임·비교표 경계

- 개별 `128×128` PNG는 방향·포즈별 프레임이다.
- contact sheet는 프레임을 한눈에 보는 비교표이며 런타임 스프라이트 시트나 atlas가 아니다.
- v1 원본·PNG·contact sheet·turnaround는 덮어쓰거나 삭제하지 않는다.
- 기존 v2·v5b 및 A/B/C도 덮어쓰거나 삭제하지 않는다.

## Blender 담당 산출물

- 원본: `artifacts/source/rat-final-appearance-sample-v1.blend`
- 재현 스크립트: `artifacts/source/create_rat_final_appearance_sample.py`
- 정지 샘플: `artifacts/renders/idle/` 8개
- 대표 보행키: `artifacts/renders/walk-key/` 8개
- 비교표: `artifacts/rat-final-appearance-contact-sheet-2048.png`
- 모델 턴어라운드: `artifacts/rat-final-appearance-turnaround-preview-2048.png`
- 프레임·렌더·팔레트: `artifacts/frame-map.csv`, `artifacts/render-settings.json`, `artifacts/palette-statistics.json`

## v2-natural 산출물

- 원본: `artifacts/v2-natural/source/rat-final-appearance-sample-v2-natural.blend`
- 재현 스크립트: `artifacts/v2-natural/source/create_rat_final_appearance_sample_v2_natural.py`
- 정지 샘플: `artifacts/v2-natural/renders/idle/` 8개
- 대표 보행키: `artifacts/v2-natural/renders/walk-key/` 8개
- contact sheet: `artifacts/v2-natural/rat-final-appearance-v2-natural-contact-sheet-2048.png`
- 턴어라운드: `artifacts/v2-natural/rat-final-appearance-v2-natural-turnaround-preview-2048.png`
- v1/v2 비교: `artifacts/v2-natural/rat-final-appearance-v1-v2-natural-comparison-2048.png`
- 기술 자료: `artifacts/v2-natural/frame-map.csv`, `artifacts/v2-natural/render-settings.json`, `artifacts/v2-natural/palette-statistics.json`

## 담당 검증과 남은 위험

- Blender 5.1.2에서 최종 스크립트로 실제 재생성.
- 16개 모두 `128×128 RGBA`, 알파 `[0,255]`, 공용 팔레트 실사용 25색, 무디더.
- 루트 변환은 위치 `(0,0,0)`, 회전 `(0,0,0)`, 스케일 `(1,1,1)`이고 root action이 없다.
- QA 지적 후 프레이밍을 수정했으며 16개 전체 최소 bbox 여백은 `4px`, 경계 접촉 파일은 0개다.
- `render-settings.json`에 `minimum_bbox_margin_px`, `per_file_bbox_margin_px`, `edge_contact_files`를 기록했다.
- E와 W의 정지·보행키는 모두 최소 `4px` 여백을 확보했다.
- 저장 `.blend`, 재현 스크립트, `render-settings.json`의 FPS는 모두 `8`로 동기화됐다.
- FPS 수정에서는 PNG를 재렌더하지 않았고 시각 출력은 변경되지 않았다.
- v2-natural 16개는 `128×128 RGBA`, 알파 `[0,255]`, 공용 25색, 무디더, 최소 bbox 여백 `4px`, 경계 접촉 0개다.
- v2 `.blend`는 FPS `8`, 프레임 `1~8`, root transform identity와 root action 없음이다.
- v1 핵심 20개 파일은 제작 전후 SHA-256이 동일하다.
- 현재 v2 결과는 자연화한 최종형 후보이며 사용자 최종 채택이나 Unity 반입 승인이 아니다.

## A2 Blender revision-6 인계

- 사용자 우선 확인: `artifacts/a2-blender-revision-6/rat-final-appearance-a2-r6-contact-sheet-2048.png`
- 4방향 확대 확인: `artifacts/a2-blender-revision-6/rat-final-appearance-a2-r6-turnaround-preview-2048.png`
- 단일 Blender 원본: `artifacts/a2-blender-revision-6/source/rat-final-appearance-a2-r6.blend`
- 독립 재현 스크립트: `artifacts/a2-blender-revision-6/source/create_rat_final_appearance_a2_r6.py`
- 정지 8방향: `artifacts/a2-blender-revision-6/renders/idle/`
- 대표 보행키 8방향: `artifacts/a2-blender-revision-6/renders/walk-key/`
- 기술 자료: `artifacts/a2-blender-revision-6/frame-map.csv`, `render-settings.json`, `palette-statistics.json`
- 기술 결과: 16개 `128×128 RGBA`, 이진 알파, 실사용 25색, 무디더, 전체 최소 여백4, edge-touch0.
- 제작 해석: A2의 자연스러운 갈색쥐 방향을 단일 3D 원본에 옮긴 최종 후보 샘플이며, 방향별 AI 프레임은 사용하지 않았다.
- 승인 경계: 이 결과는 사용자 최종 외형 승인, 전체64프레임, runtime atlas/스프라이트 시트, Unity Import·통합을 뜻하지 않는다.
- 다음 단계: 사용자에게 A2·r6 비교표·턴어라운드를 제시하고 최종 외형 수용 여부를 확인한다.

### r6 idle 계약 수정 인계

- idle은 더 이상 walk frame1을 재사용하지 않는다. 별도 neutral frame0에서 네 발을 공통 지면에 접지한 결과다.
- `frame-map.csv`: idle frame `0`, phase `neutral_four_paw_ground_contact`; walk-key frame `4`, phase `diagonal_FL_RR_lift`.
- `render-settings.json > pose_contract`:
  - idle 네 발 world bbox minZ 모두 `0.01`.
  - walk-key 접지 `Paw_FR`, `Paw_RL`; 들림 `Paw_FL`, `Paw_RR`.
  - idle-vs-walk 8방향 픽셀 차이 모두 0 초과.
- 접촉 시트의 위 행은 수정된 neutral idle, 아래 행은 기존 대표 보행키다.
- 수정 후에도 16개 기술 규격, 공용 25색, 이진 알파, 무디더, 최소 여백4, edge-touch0을 유지한다.
