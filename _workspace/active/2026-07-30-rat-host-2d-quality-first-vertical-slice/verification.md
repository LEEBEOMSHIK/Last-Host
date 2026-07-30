# QA/검증

상태: **PASS — 고품질 제작 마스터 후보 3종의 기술·추적성 확인 완료**

검증일: 2026-07-30
담당: QA/검증 에이전트

## 검증 대상과 완료 주장

환경·쥐 측면 보행·HUD 고품질 제작 마스터 후보 3종이 손상 없는
PNG로 저장됐고, `imagegen` 입력·프롬프트·원본 출력과 프로젝트
사본의 추적성 및 `후보/최종 게임 에셋` 경계가 기록됐다는 주장을
검증했다.

이번 QA는 `visual-review.md`의 미적 PASS를 기술 검사로 대체하거나
새로 판정하지 않는다. 실제 PNG가 비주얼 담당이 검토한 파일·크기·
구성과 대응하는지만 확인했다.

## 실행한 검증

### 파일·PNG 무결성

- `artifacts/quality-masters/`의 PNG 수: 정확히 `3개`
- 요구 파일명:
  - `environment-quality-master.png`
  - `rat-side-walk-quality-master.png`
  - `hud-quality-master.png`
- 세 파일 모두 PNG signature 정상, 8-bit PNG color type `2`(RGB),
  이미지 디코딩 성공

| 파일 | 실제 크기 | 바이트 | SHA-256 |
| --- | ---: | ---: | --- |
| `environment-quality-master.png` | 1672×941 RGB | 2,702,528 | `52ac27b6acdf642f567aac0d33a9a0fbaaed5aa6bd175417c0e5d52130611406` |
| `rat-side-walk-quality-master.png` | 1881×836 RGB | 1,689,293 | `6bb8ac96832d74988093babeb798e2f4ade5c96d62e07da625c49d24b7c6ccfc` |
| `hud-quality-master.png` | 1672×941 RGB | 1,552,670 | `24f906301f98aeb72369522c9ddcd7a4ce20db5bbd9f8775aaf37ca8204df0f9` |

실제 크기·모드는 `imagegen-log.md`와 `visual-review.md`의 기록에
모두 일치한다.

### 쥐 최종 교체본 delta 재검증

- 기준 SHA-256:
  `6BB8AC96832D74988093BABEB798E2F4ADE5C96D62E07DA625C49D24B7C6CCFC`
- 프로젝트 사본: 존재, PNG signature 정상, 디코딩 성공
- 실제 규격: `1881×836`, 8-bit PNG color type `2`(RGB)
- 로그의 현재 최종 원본:
  `call_nramYlACylZYQ6JLO0pF8PBg.png`
- 원본과 프로젝트 사본 SHA-256: 정확히 일치
- `visual-review.md`도 위 새 SHA를 명시하고 원본 크기 재검토한
  파일에만 쥐 PASS를 부여한다.
- 환경 SHA-256:
  `52AC27B6ACDF642F567AAC0D33A9A0FBAAED5AA6BD175417C0E5D52130611406`
  — 이전 QA와 동일
- HUD SHA-256:
  `24F906301F98AEB72369522C9DDCD7A4CE20DB5BBD9F8775AAF37CA8204DF0F9`
  — 이전 QA와 동일

따라서 쥐만 교체됐으며 환경·HUD 기존 판정 대상은 바뀌지 않았다.
QA는 새 쥐의 시각 품질을 기술 해시로 재판정하지 않고, 비주얼
담당이 새 SHA를 직접 재검토했다는 대응만 확인했다.

### imagegen 생성 추적성

- 작업 ID, 담당, OpenAI 내장 `imagegen`, 생성일 `2026-07-30` 기록:
  확인
- 입력 reference 절: `5개`
- 전체 프롬프트 절: `7개`
  - 환경 1회
  - 쥐 1차 1회
  - 쥐 동일 개체 targeted regeneration·targeted edit 4회
  - HUD 1회
- 일곱 생성 원본 경로가 모두 실제로 존재한다.
- 최종 선별된 환경·쥐 재생성본·HUD 프로젝트 사본은 각각 로그의
  원본과 SHA-256이 정확히 같다.
- 입력으로 기록된 승인 reference 4종은 모두 실제로 존재한다.
  - 목표 게임플레이 목업
  - 환경 통합 기준
  - 쥐 V2 통합 기준
  - HUD 통합 기준
- 금지된 저품질 Q1 시험 에셋 작업 경로
  `rat-host-2d-game-spec-trial-assets`의 로그 언급: `0건`
- 로그는 `기존 저품질 Q1 PNG 입력 사용: 없음`을 명시한다.
  쥐 재생성·수정의 추가 입력은 저품질 Q1 PNG가 아니라 바로 앞
  고품질 쥐 후보와 승인된 `rat-integrated-target-v2.png`다.

따라서 작업 패킷 안에서 요구한 입력 reference·전체 프롬프트·도구·
날짜·원본 출력·선별 출력의 문서 추적성은 충족한다. 다만 PNG 자체에
원본 tool-call 요청과 첨부 입력을 암호학적으로 증명하는 메타데이터가
있는 것은 아니므로, 실제 호출 이력의 독립 감사 범위는 생성 로그와
보존된 원본 경로까지다.

### 비주얼 리뷰와 실제 파일 대응

세 PNG를 원본 크기로 직접 열고 다음 대응만 확인했다.

- 환경: 아이소메트릭 하수도 장면, 석재 경로·벽·청록 수로·통·상자·
  원형 배수구와 조명이 실제 파일에 존재한다.
- 쥐: 단색 중립 배경에 오른쪽을 보는 동일 갈색 쥐의 측면 포즈
  3개가 실제 파일에 존재하며 문자·격자·프레임 테두리가 없다.
- HUD: 자연형 쥐 초상 1개, 위쪽 적색 바 1개, 아래쪽 청록 바 1개가
  실제 파일에 존재하며 문자·수치가 없다.

이는 `visual-review.md`가 다른 파일이나 이전 Q1 파일을 검토한 것이
아님을 확인하는 대조다. 환경/쥐/HUD의 `PASS` 및 품질 기준 충족
판정 자체는 비주얼/테크아트 기록을 따른다.

### 후보와 최종 게임 에셋 경계

`task.md`, `imagegen-log.md`, `quality-rubric.md`,
`visual-review.md`가 다음 경계를 일관되게 명시한다.

- 현재 3종은 `고품질 제작 마스터 후보`다.
- 최종 타일셋·최종 스프라이트 시트·Unity 반입 완료본이 아니다.
- 환경 반복 타일 분해, 쥐 투명 배경·공통 셀·피벗·프레임
  페인트오버, HUD 투명 모듈·9-slice 재제작이 후속으로 필요하다.
- 사용자 선별과 게임 규격 재제작·독립 QA·Unity Play 검증 전에는
  최종 게임 에셋으로 승격하지 않는다.

## Unity·Git 경계

- 후보 3종 파일명으로 `UnityProject/`를 검색한 결과: `0개`
- 이번 QA는 UnityProject 파일을 수정하거나 되돌리지 않았다.
- UnityProject에는 기존 Stage2·Stage3 작업의 modified 5개,
  untracked 8개가 남아 있으며 그대로 보존했다.
- `git diff --check`: 종료 코드 `0`
- 검증 기준 HEAD:
  `73c575058ee73a9c4ae926d42ae77480a82e5604`
  (`main`, upstream `origin/main`)

## Play·Build 비적용

Unity EditMode·Play Mode·Windows Build는 실행하지 않았다. 현재
산출물은 배경을 포함한 RGB 제작 마스터 후보이고 UnityProject에
반입되지 않았다. 작업 배정서도 Unity Import를 금지하므로 현 단계의
Play·Build는 후보 PNG의 품질·추적성을 검증하지 못하며, 반입과
플레이 검증은 별도 승인 작업이다.

## 남은 위험

- 세 파일은 투명 게임 에셋, 반복 타일, 공통 캔버스·피벗 또는
  실제 애니메이션 시트가 아니다.
- 환경 타일 반복·가림·충돌, 쥐 프레임 접지·체적 안정,
  HUD 모듈 분해·9-slice가 검증되지 않았다.
- 실제 카메라 배율의 픽셀 가독성과 게임 배경 위 HUD 대비는
  Unity 반입 뒤 확인해야 한다.
- 생성 로그 외의 원격 tool-call 첨부 입력 이력은 PNG만으로
  독립 증명할 수 없다.
- 사용자 최종 품질 수용과 프로젝트 총괄 판정이 남아 있다.

## 완료 판단

**QA PASS.** 고품질 제작 마스터 후보 3종은 사용자 품질 확인 단계로
넘길 수 있다. 이 판정은 최종 게임 에셋·Unity 적용 완료 판정이 아니다.
