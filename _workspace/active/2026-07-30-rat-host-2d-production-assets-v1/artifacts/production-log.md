# 제작 기록

## 2026-07-30

- 사용자 승인: 고품질 제작 마스터 기준 실제 게임 에셋 1차 재제작 시작.
- 입력 기준:
  - `environment-quality-master.png`
  - `rat-side-walk-quality-master.png`
  - `hud-quality-master.png`
  - `quality-rubric.md`
- 제작 경계:
  - source master와 실제 RGBA 에셋을 분리한다.
  - OpenAI 내장 imagegen source는 곧바로 최종 에셋으로 선언하지 않는다.
  - 설치된 `remove_chroma_key.py`로 크로마를 제거하고, 공통 게임 규격으로 정리한다.
  - 환경 반복·쥐 접지·HUD 모듈 분해를 별도 검증한다.
  - Unity 반입은 수행하지 않는다.
- source master board 수신:
  - `environment-tile-source-board.png`
  - `props-source-board.png`
  - `rat-side-walk-source-board.png`
  - `hud-module-source-board.png`
- source 생성·stable copy 추적은 `source-generation-log.md`에 기록되었고
  4/4 SHA-256 일치가 확인되었다.
- 크로마 제거:
  - 도구: 설치된
    `C:\Users\User\.codex\skills\.system\imagegen\scripts\remove_chroma_key.py`
  - 옵션: `--auto-key border --soft-matte
    --transparent-threshold 12 --opaque-threshold 220 --despill`
  - 환경 key: `#fa03fa`
  - 소품 key: `#fb03fa`
  - 쥐 key: `#09f91b`
  - HUD key: `#f204f5`
- 후속 게임 규격 정리:
  - source board의 주요 component를 모듈별로 다시 분리했다.
  - 바닥·낡은 바닥·수면은 외곽 slab 그림자를 제외한 상면을
    rectification하고, 반대 edge를 동일화한 뒤 128×64 diamond로
    재투영했다.
  - 벽·수로 경계·소품은 alpha bbox 기준으로 불필요 픽셀을 제거하고
    공통 논리 픽셀 밀도로 nearest 배치했다.
  - 쥐는 각 frame을 단순 crop하지 않고 component 단위로 분리한 뒤
    256×192 공통 캔버스, top y=152 접지선, bottom-left `(128,40)`
    pivot에 다시 배치했다.
  - 쥐의 green-key 잔류 후보 픽셀은 삭제하지 않고 warm-neutral
    edge cleanup으로 수염·코·털 외곽을 보존했다.
  - HUD는 초상/프레임/공용 bar/붉은 fill/청록 fill을 독립 RGBA로
    분리하고, full/half/empty 상태를 100%와 nearest 50%에서 조립했다.
- 산출 스크립트:
  - `source/build_production_assets.py`
  - `source/validate_production_assets.py`
- 자동 검증: `128/128 PASS`
- 재생성 해시: 실제 게임 에셋 `20/20` 일치
- 현재 상태: 제작 담당 자체 검증 완료, 독립 비주얼·QA 대기.
