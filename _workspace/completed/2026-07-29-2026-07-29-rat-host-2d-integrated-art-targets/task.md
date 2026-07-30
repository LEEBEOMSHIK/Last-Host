# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-29-rat-host-2d-integrated-art-targets`
- 작업명: 쥐 숙주 2D 통합 제작 기준 이미지와 시험 재제작 브리프
- 상태: 완료 — 환경·쥐 V2·HUD 사용자 수용
- 생성일: 2026-07-29
- 담당 에이전트: ChatGPT 이미지 아트 에이전트
- 보조 에이전트: 비주얼/테크아트 에이전트, QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `$imagegen`, `$pixel-lowpoly-style-keeper`

## 사용자 승인 근거

- 사용자는 첫 아트 후보 검토 뒤 추천 조합에 대해
  `내가 생각한 내용도 너가 추천한 조합이 나쁘지 않은거 같아`라고
  답했다.
- 직전 확인 요청은 추천 조합 그대로 재제작 단계로 진행할지에 대한
  것이었으므로, 이번 작업은 세 조합을 통합한 제작 기준 이미지와
  시험 재제작 규격 작성까지 승인된 것으로 해석한다.
- 쥐 전체 8방향·보행 완성, 최종 게임 에셋 선언, Unity 반입은
  승인 범위에 포함하지 않는다.
- 2026-07-29 총괄 `수정 필요` 판정 뒤 사용자는 `좋아 진행해`로
  쥐 통합본 교정 1회를 추가 승인했다.

## 확정 조합

1. 환경: `V2 구조·경로 + V1 팔레트·따뜻한 황동 조명`
2. 쥐: `V1 자연형 체형·실루엣 + 털 픽셀 군집 단순화`
3. HUD: `V2 구조·대비·모듈 + V1 따뜻한 재질`

## 목적

첫 후보의 장점을 한 장씩 통합해 실제 반복 타일, 방향별 쥐
스프라이트, 투명 HUD 모듈을 재제작할 때 흔들리지 않는 시각 기준을
만든다. 동시에 첫 시험 제작의 범위와 비최종 규격을 문서로 고정한다.

## 생성 범위

- 환경 통합 제작 기준 이미지 `1개`
- 쥐 대표 3시점 통합 제작 기준 이미지 `1개`
- 최소 HUD 통합 제작 기준 이미지 `1개`
- 최초 생성 수량: `3개`
- 추가 승인 생성: 쥐 교정본 `1개`
- 최종 사용자 확인 묶음: 환경·교정된 쥐·HUD `3개`
- 최초 쥐 통합본은 폐기 사유와 함께 별도 경로에 보존한다.

## 저장 위치

- 통합 이미지: `artifacts/integrated-targets/`
- 수정 전 쥐 확인본: `artifacts/rejected/rat-integrated-target-v1.png`
- 사용자 선별 기록: `artifacts/selection-record.md`
- 시험 재제작 규격: `artifacts/recreation-brief.md`
- 생성 프롬프트·출처: `artifacts/generation-log.md`
- 비주얼 검토: `artifacts/visual-review.md`

## 입력 자료

- 이전 후보 6개:
  `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-first-art-sample/artifacts/ai-candidates/`
- 목표 화면:
  `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
- 제작 가이드:
  `docs/design/visual/pixel-isometric-2d-production-guide.md`
- AI 연계 절차:
  `docs/prototype/plans/rat-host-ai-assisted-art-workflow.md`

## 역할과 산출물

| 에이전트 | 책임 | 산출물 |
| --- | --- | --- |
| ChatGPT 이미지 아트 에이전트 | 통합 이미지 3개 생성, 프롬프트·출처·1차 점검 | PNG 3개, 생성 로그 |
| 비주얼/테크아트 에이전트 | 조합 반영, 게임 규격 재제작 가능성 검토 | `artifacts/visual-review.md` |
| QA/검증 에이전트 | 파일·수량·출처·범위·문서 정합 독립 대조 | `verification.md` |
| 프로젝트 총괄 관리자 에이전트 | 승인·범위·검증 기록 내부 판정 | `director-review.md` |
| 메인 조정자 | 작업 패킷·현황판·인계 통합 | 상태판과 사용자 보고 |

## 금지 범위

- 추가 승인된 쥐 교정본 1개를 넘는 생성
- 통합 이미지를 반복 타일, 투명 스프라이트, 애니메이션 시트 또는
  최종 HUD로 선언
- 쥐 전체 8방향·완성 보행 제작
- UnityProject 코드·씬·Import·ProjectSettings·패키지 변경
- 기존 Stage2·Stage3·사용자 변경 수정
- 범위 밖 콘텐츠, 글자, 상표, 로고, 워터마크 추가

## 완료 기준

- 통합 기준 PNG 3개와 전체 생성 기록이 프로젝트 작업영역에 있다.
- 최종 통합 폴더에는 환경·교정된 쥐·HUD 3개만 있고 수정 전 쥐는
  반려 경로와 사유가 추적된다.
- 추천 조합이 각 이미지에 반영됐는지 비주얼 검토가 있다.
- 시험값과 최종 규격의 경계가 재제작 브리프에 명확하다.
- QA와 총괄 내부 판정 뒤 사용자에게 확인할 이미지 3개만 제시한다.
