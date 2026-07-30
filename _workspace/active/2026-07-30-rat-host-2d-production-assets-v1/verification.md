# QA/검증

상태: **PASS — 실제 RGBA 게임 에셋 1차 묶음 기술 검증 완료**

검증일: 2026-07-30
담당: QA/검증 에이전트

## 검증 대상과 완료 주장

승인된 고품질 마스터를 기준으로 제작한 환경·쥐·HUD 1차 게임
에셋이 지정 파일·알파·반복·접지·피벗·모듈 규격을 충족하고,
고정된 제작 입력에서 같은 공식 20파일을 재생성한다는 주장을
독립 검증했다.

`visual-review.md`의 마스터 대비 시각 PASS는 기술 검사로 대체하지
않았다. QA는 해당 리뷰가 현재 source board, 실제 RGBA와 프리뷰
7종을 검토한 기록인지 대조했다.

## 자동 검사

명령:

`python source\validate_production_assets.py`

결과:

- `128/128 PASS`
- 실패 `0`
- 재빌드 종료 코드 `0`
- validator 내부 공식 산출물 재현성 `20/20`

해석:

파일 존재·크기·RGBA·투명 모서리·가시 픽셀·크로마 잔류, 환경
반복, 쥐 bbox·접지·체형·피벗·프레임 순서, HUD 배치, 프리뷰
존재와 재빌드 조건이 모두 통과했다.

## 공식 20파일 결정성

validator와 별개로 `asset-hashes.json`의 20경로를 기준으로 재빌드
전 SHA-256을 계산하고 `build_production_assets.py`를 공식 경로에
다시 실행한 뒤 같은 경로를 재계산했다.

- 공식 파일: `20개`
- 재빌드 전·후 일치: `20/20`
- mismatch: `0`
- 재빌드 뒤 `asset-hashes.json`과 불일치: `0`

공식 20파일은 환경 PNG 9개, 쥐 PNG 4개와 frame map JSON 1개,
HUD PNG 5개와 layout JSON 1개다.

## PNG·알파·크로마

- 실제 PNG: `18개`
- JSON: `2개`
- PNG signature·디코딩 실패: `0`
- 실제 PNG 헤더 color type `6`(8-bit RGBA) 실패: `0`
- 매니페스트 예상 크기 실패: `0`
- 네 모서리 alpha 0 실패: `0`
- validator의 가시 영역 크로마 검사:
  - 자홍 잔류 실패 `0/18`
  - 녹색 키 잔류 실패 `0/18`

validator는 로드 이미지를 RGBA로 변환해 검사하므로, 파일 자체
색상 모드도 PNG 헤더에서 별도 대조했다.

## 환경

실제 파일과 매니페스트 크기가 모두 일치한다.

- 바닥 clean/worn: 각각 `128×64`
- water center: `128×64`
- straight wall: `160×160`
- corner wall: `192×160`
- water edge: `128×96`
- 통: `96×112`
- 상자: `112×112`
- 배수구: `128×80`

4×4 조립 자동 검사:

- clean: visible component `1`, hole `0`
- worn: visible component `1`, hole `0`
- water center: visible component `1`, hole `0`

`environment_repeat_checker.png`에서 clean, clean/worn, water 반복을
직접 확인했고 눈에 띄는 알파 구멍이나 셀 외곽 단절이 없다.
`environment_room_preview.png`에는 straight/corner 벽, water edge,
통·상자·배수구가 실제 모듈로 조립돼 있으며 파일·투시·구성은
비주얼 리뷰 기록에 대응한다.

## 쥐

- 개별 프레임: neutral/contact/passing 정확히 `3개`
- 공통 캔버스: 전부 `256×192` RGBA
- alpha bbox:
  - neutral `(9,78)-(247,152)`
  - contact `(9,76)-(247,152)`
  - passing `(9,79)-(247,152)`
- visible 폭: `238/238/238`
- visible 높이: `74/76/73`, 최대/최소 비율 `1.041`
- 접지선: 세 프레임 모두 top-origin `y=152`
- pivot bottom-left: `(128,40)`
- normalized pivot: `(0.5,0.208333)`
- frame map 순서: neutral/contact/passing
- 시트: `768×192`
- 시트 각 256×192 셀과 개별 PNG 픽셀 mismatch:
  `0/0/0`

`rat_actual_size.png`, `rat_50_percent.png`, `rat_2x.png`를 직접
확인했다. 공통 접지와 세 보행 위상이 대응하고 50%에서도 진행
방향·귀·코·꼬리가 남는다. 이는 프리뷰 대응 확인이며 실제 이동
속도의 애니메이션 품질 판정은 아니다.

## HUD

독립 RGBA 모듈 5개의 크기가 매니페스트와 일치한다.

- 쥐 초상 `184×184`
- 초상 프레임 `256×256`
- 공용 bar frame `512×80`
- red/teal fill 각각 `400×52`
- fill offset top-left `(56,14)`
- 후보 표시 배율 `0.5`

`hud_states.png`에서 red full, teal half, empty의 100% 조립과 같은
상태의 nearest 50% 표시를 직접 확인했다. 프레임과 채움의 배치가
layout JSON에 대응하며, 비주얼 리뷰가 기록한 최초 fill 미표시
blocker의 수정본이다.

## source 4종과 imagegen 추적성

source master 4개와 imagegen 원본을 대조했다.

| source board | 크기·모드 | SHA-256 | 원본과 일치 |
| --- | --- | --- | --- |
| `environment-tile-source-board.png` | 1536×1024 RGB | `588EE90358E7B9435EAAF0A4CD1F0AF7A40952ED592B0D65FA8EAFB4700BDC01` | 예 |
| `props-source-board.png` | 1821×864 RGB | `425730C4C7A3B3261AF7118E346F3344FC5E99DB8F9EABC69E86FB56EB07F0B3` | 예 |
| `rat-side-walk-source-board.png` | 1881×836 RGB | `F17BF99CF22FDACF3288588CC43E2721595B4B7F4A664479377FEB8ACA86D451` | 예 |
| `hud-module-source-board.png` | 1672×941 RGB | `4A3CF50AB0E304BE180C8CDAFE05EDAFAACE756DF5E1494A1C1761A62B12A060` | 예 |

- imagegen 원본 4개: 모두 존재
- 안정 저장 사본: 원본과 각각 SHA-256 일치
- 입력 reference 절: `4개`
- 전체 프롬프트 절: `4개`
- 도구: OpenAI 내장 `imagegen`
- 날짜: `2026-07-30`
- 승인된 환경·쥐·HUD 품질 마스터: 모두 존재
- 쥐 품질 마스터 SHA:
  `6BB8AC96832D74988093BABEB798E2F4ADE5C96D62E07DA625C49D24B7C6CCFC`
- 금지 Q1 trial 작업 경로 언급: `0건`
- 로그의 제외 입력: 기존 저품질 Q1 PNG, 목표 목업, 통합 시안

크로마 제거 후 보존된 cleaned board 4개도 원본 보드와 같은 크기의
8-bit RGBA로 디코딩된다.

## 프리뷰 7종

모두 존재하고 정상 디코딩된다.

- 환경 반복 `960×420`
- 환경 방 `960×540`
- 쥐 actual `768×224`
- 쥐 50% `384×112`
- 쥐 2× `1536×448`
- HUD 상태 `960×540`
- 마스터 비교 `1440×1250`

일곱 프리뷰를 원본 크기로 직접 열어 `visual-review.md`가 기록한
환경 반복·방 조립, 쥐 actual/50%/2×, HUD 상태와 마스터 비교의
현재 파일 대응을 확인했다. 시각적 PASS 자체는 비주얼 담당 판정을
따른다.

## 소스 구문 검사

명령:

`python -m py_compile source\build_production_assets.py
source\validate_production_assets.py source\inspect_source_boards.py`

결과:

- 스크립트 `3/3` 구문 검사 통과
- 첫 캐시 경로 `C:\tmp\last-host-production-qa-pyc`는 권한 거부로
  코드 검사 전에 종료됐다.
- 허용된 임시 캐시 경로로 동일 명령을 다시 실행해 종료 코드 `0`을
  확인했다. 프로젝트 안에 `__pycache__`를 만들지 않았다.

## Unity·Git 경계

- 공식 후보 20파일명을 `UnityProject/`에서 검색한 결과: `0개`
- UnityProject에는 기존 Stage2·Stage3 작업의 modified 5개,
  untracked 8개가 남아 있으며 수정하거나 되돌리지 않았다.
- `git diff --check`: 종료 코드 `0`
- 검증 기준 HEAD:
  `73c575058ee73a9c4ae926d42ae77480a82e5604`
  (`main`, upstream `origin/main`)

## Play·Build 비적용

Unity EditMode·Play Mode·Windows Build는 실행하지 않았다. 현재
에셋은 작업 패킷 아래에 있고 UnityProject로 Import되지 않았으며,
`task.md`가 UnityProject 변경을 금지한다. 따라서 현 단계 Play·Build는
이 PNG 묶음을 사용하지 않아 유효한 게임 검증이 아니고, 반입 자체가
별도 승인 작업이다.

## 남은 위험

- 환경은 clean/worn/water 변형 수가 적어 더 큰 맵에서 반복 주기가
  보일 수 있다.
- 쥐는 측면 1방향·3프레임이며 전체 방향과 실제 이동 속도에서의
  프레임 타이밍·픽셀 안정성은 미검증이다.
- 후보 `128×64`, `256×192`, 피벗과 50% 표시는 최종 PPU·셀 규격이
  아니다.
- 벽·소품 Y축 sorting, 가림, 충돌, Pixel Perfect 출력은 Unity
  반입 뒤 검증해야 한다.
- HUD safe area, UI scale, fill masking과 9-slice는 미검증이다.
- 공식 20파일 재빌드는 보존된 cleaned board부터 결정적이다.
  imagegen RGB 원본에서 cleaned board를 만드는 외부 크로마 제거
  단계는 옵션과 해시가 기록됐지만 이 build 명령에 포함되지 않는다.
- 사용자 최종 수용과 프로젝트 총괄 판정이 남아 있다.

## 완료 판단

**QA PASS.** 실제 RGBA 게임 에셋 1차 묶음은 총괄 검토와 사용자
확인 단계로 넘길 수 있다. 이 판정은 최종 PPU·전체 방향·Unity
적용·Play·Windows Build 완료를 의미하지 않는다.
