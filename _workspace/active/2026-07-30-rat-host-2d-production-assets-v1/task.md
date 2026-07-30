# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-30-rat-host-2d-production-assets-v1`
- 작업명: 고품질 마스터 기준 실제 게임 에셋 1차 재제작
- 상태: 내부 승인 완료 — 사용자 실제 에셋 확인 대기
- 생성일: 2026-07-30
- 담당 에이전트: 2D 에셋 제작 담당
- 보조 에이전트: ChatGPT 이미지 아트 에이전트,
  비주얼/테크아트 에이전트, QA/검증 에이전트,
  프로젝트 총괄 관리자 에이전트
- 사용 스킬: `$imagegen`, `$pixel-lowpoly-style-keeper`,
  `$last-host-design-keeper`

## 사용자 승인 근거

- 사용자는 환경·쥐 보행·HUD 고품질 제작 마스터의 품질을 수용했다.
- 사용자는 프로젝트에서 가장 중요한 기준이 퀄리티라고 명시했다.
- 사용자는 현황판을 갱신하고 고품질 실제 에셋 1차 재제작을
  시작하도록 승인했다.

## 목적

승인된 고품질 마스터의 재질·명암·자연형 체형을 낮추지 않고
실제 게임에서 사용할 수 있는 반복 타일, 투명 캐릭터 스프라이트,
투명 HUD 모듈의 첫 묶음으로 재제작한다.

## 품질 우선 원칙

- 시각 품질이 마스터보다 명백히 낮아지면 반복·알파·피벗 검사가
  통과해도 반려한다.
- 단순 도형·저밀도 Pillow 플레이스홀더 방식은 금지한다.
- imagegen 결과는 분리 소스·가이드로 사용할 수 있으나 그대로
  잘라 최종 에셋으로 자동 선언하지 않는다.
- 실제 에셋에는 크로마 제거, 공통 캔버스·접지·피벗 정리,
  타일 이음 보정, 픽셀 페인트오버·불필요 픽셀 정리를 수행한다.

## 1차 제작 범위

### 환경

- 반복 바닥 타일: clean 1종, worn 1종
- 벽: straight 1종, corner 1종
- 수로: center 1종, edge 1종
- 소품: 통, 상자, 배수구
- 반복 검사판과 작은 방 조립 프리뷰

### 쥐

- 방향: 측면 1방향
- 프레임: neutral, contact, passing
- 동일한 투명 공통 캔버스·접지선·피벗
- 개별 PNG, 시트, frame map
- 실제 크기와 확대 비교 프리뷰

### HUD

- 쥐 초상
- 초상 황동 프레임
- 공용 게이지 프레임
- 붉은 생명력 채움
- 청록 면역 경계도 채움
- 투명 모듈과 조립 상태 프리뷰

## 제작 규격 후보

- 품질 보존용 2배 소스 격자:
  - 환경 셀 `128×64`
  - 쥐 공통 캔버스 `256×192`
- 실제 화면 비교:
  - 100% 소스와 nearest 50% 표시를 함께 확인
  - 최종 셀·PPU 승격은 Unity 샘플 뒤 결정
- 래스터: PNG RGBA
- 필터 전제: Point, mipmap off
- 쥐 피벗: 발 접지 중앙 bottom-left 기준으로 frame map에 명시
- 모든 프레임에서 논리 루트와 접지선 고정

## 입력

- 환경 품질 마스터:
  `_workspace/active/2026-07-30-rat-host-2d-quality-first-vertical-slice/artifacts/quality-masters/environment-quality-master.png`
- 쥐 품질 마스터:
  `_workspace/active/2026-07-30-rat-host-2d-quality-first-vertical-slice/artifacts/quality-masters/rat-side-walk-quality-master.png`
- HUD 품질 마스터:
  `_workspace/active/2026-07-30-rat-host-2d-quality-first-vertical-slice/artifacts/quality-masters/hud-quality-master.png`
- 품질 게이트:
  `_workspace/active/2026-07-30-rat-host-2d-quality-first-vertical-slice/artifacts/quality-rubric.md`
- 목표 목업:
  `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`

## 산출물

- 실제 에셋: `artifacts/game-assets/`
- imagegen 분리 소스: `artifacts/source-masters/`
- 제작·정리 스크립트: `source/`
- 매니페스트·피벗·frame map: `artifacts/asset-manifest.md`
- 프리뷰: `artifacts/previews/`
- 출처·제작 기록: `artifacts/production-log.md`
- 비주얼 검토: `artifacts/visual-review.md`

## 검증

- 환경 타일 4×4 반복, 물·벽 straight/corner 조립
- RGBA와 투명 모서리, 녹색·자홍 크로마 잔류 검사
- 쥐 3프레임 동일 캔버스·접지·피벗과 체형 일관성
- HUD 모듈 독립 알파와 empty/half/full 조립
- 마스터와 실제 에셋을 100%·50%에서 나란히 시각 대조
- 생성·후처리·수정 이력과 최종 파일 해시 기록

## 금지 범위

- 기존 저품질 Q1 PNG를 품질 기준이나 제작 입력으로 사용
- 전체 8방향·전체 하수도 타일셋·최종 UI 전체 제작
- UnityProject Import·씬·코드·ProjectSettings·패키지 변경
- 현재 후보 규격을 최종 PPU·타일·셀 규격으로 승격
- 기존 Stage2·Stage3·ProjectSettings 변경 수정

## 완료 기준

- 환경·쥐·HUD 실제 RGBA 파일이 지정 경로에 존재한다.
- 반복·알파·피벗·모듈 조립이 기술적으로 유효하다.
- 엄격 비주얼 검토에서 마스터 대비 명백한 품질 저하가 없다.
- QA와 총괄 내부 판정 뒤 사용자에게 실제 에셋 프리뷰를 제시한다.
- 사용자 수용 전 Unity 반입이나 전체 확장을 진행하지 않는다.
