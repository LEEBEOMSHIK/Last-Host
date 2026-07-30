# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-30-rat-host-2d-quality-first-vertical-slice`
- 작업명: 쥐 숙주 2D 품질 우선 고품질 수직 샘플
- 상태: 사용자 품질 수용 — 실제 에셋 재제작으로 인계
- 생성일: 2026-07-30
- 담당 에이전트: ChatGPT 이미지 아트 에이전트
- 보조 에이전트: 비주얼/테크아트 에이전트, QA/검증 에이전트,
  프로젝트 총괄 관리자 에이전트
- 사용 스킬: `$imagegen`, `$pixel-lowpoly-style-keeper`,
  `$last-host-design-keeper`

## 사용자 승인과 최우선 원칙

- 사용자는 기존 Q1 시험 에셋을 목표 목업 대비 품질이 지나치게
  낮다고 반려했다.
- 사용자는 프로젝트에서 가장 중요하게 생각하는 부분이
  `퀄리티`라고 명시하고 이를 참고해 작업을 계속하도록 승인했다.
- 이번 작업에서는 파일 규격·반복·피벗보다 시각 품질을 먼저
  통과해야 한다. 기술적으로 유효해도 목표 품질에 못 미치면
  사용자 확인본으로 올리지 않는다.

## 목적

전체 타일셋·8방향을 양산하기 전에 목표 목업과 수용된 통합 기준에
가까운 묘사 밀도, 자연형 쥐, 명암 깊이, 재질 표현을 실제 제작
마스터 후보로 증명한다.

## 제작 범위

### 환경 품질 마스터

- `960×540` 또는 그 이상 비율의 하수도 고정 아이소메트릭 화면
- 조밀한 석재 바닥·벽·수로·이끼·습윤 재질
- 통·상자·배수구 등 대표 소품
- 이동 경로와 쥐가 배경에 묻히지 않는 명도 우선순위
- HUD와 문자는 제외

### 쥐 품질 마스터

- 동일 자연형 갈색 쥐의 측면 3프레임
- 정지, 보행 접촉, 보행 통과 자세
- 등–목–머리 곡선, 낮고 긴 몸, 접지된 발, 읽히는 귀·코·꼬리
- 세 프레임에서 체형·무늬·광원·화면 점유율 유지
- 단색 중립 배경, 격자·문자·프레임 테두리 제외

### HUD 품질 마스터

- 쥐 초상 프레임, 붉은 숙주 생명력 바, 청록 면역 경계도 바
- 낡은 황동·어두운 금속·석재 질감
- 얇지만 실제 화면에서 읽히는 외곽과 상태 대비
- 문자·수치·로고·워터마크 제외

## 입력 reference

- 목표 화면:
  `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
- 환경 통합 기준:
  `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-integrated-art-targets/artifacts/integrated-targets/environment-integrated-target.png`
- 쥐 통합 기준:
  `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-integrated-art-targets/artifacts/integrated-targets/rat-integrated-target-v2.png`
- HUD 통합 기준:
  `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-integrated-art-targets/artifacts/integrated-targets/hud-integrated-target.png`

## 품질 게이트

- 목업과 비슷한 수준의 자연형 쥐 실루엣이 보인다.
- 환경은 단순 도형 타일이 아니라 석재·벽돌·물·이끼·습윤면의
  재질 차이와 깊은 명암을 가진다.
- 픽셀 군집이 기계적으로 반복되지 않고, 확대·실제 화면 모두에서
  의도적인 고품질 도트 아트로 읽힌다.
- 쥐 3프레임이 같은 개체로 보이며 포즈만 달라진다.
- HUD가 목업의 얇은 황동 프레임과 상태 대비를 계승한다.
- 단순한 Pillow 도형·플레이스홀더 수준이면 규격과 무관하게 반려한다.

## 제작 방식과 경계

- OpenAI 내장 `imagegen`을 우선 사용한다.
- 세 묶음은 각각 별도 생성 호출과 전용 프롬프트를 사용한다.
- 입력 reference, 전체 프롬프트, 생성 도구·날짜, 출력 경로와
  검토 결과를 기록한다.
- 생성 결과는 `고품질 제작 마스터 후보`이며 최종 타일셋,
  최종 스프라이트 시트 또는 Unity 반입 완료본이 아니다.
- 실제 게임 에셋 승격 전에는 반복 타일 분해, 투명 배경, 공통
  캔버스·피벗, 수작업 도트 페인트오버와 Unity QA가 필요하다.

## 금지 범위

- 저품질 시험 PNG를 품질 reference로 사용하는 것
- 단순 도형 생성 스크립트로 이번 품질 샘플을 대체하는 것
- 전체 8방향·전체 타일셋·Unity Import를 동시에 진행하는 것
- 생성 결과를 자동으로 최종 게임 에셋으로 선언하는 것
- 인간·병원·연구소·백신·엔딩 등 범위 밖 콘텐츠
- 제3자 게임·작가의 고유 스타일·캐릭터·UI 복제

## 산출물

- `artifacts/quality-masters/environment-quality-master.png`
- `artifacts/quality-masters/rat-side-walk-quality-master.png`
- `artifacts/quality-masters/hud-quality-master.png`
- `artifacts/imagegen-log.md`
- `artifacts/visual-review.md`
- `verification.md`
- `director-review.md`

## 완료 기준

- 세 마스터 후보가 모두 저장되고 생성 기록이 있다.
- 비주얼 검토가 각 후보를 목표 목업·통합 기준과 직접 대조한다.
- 품질 미달 후보는 사용자 확인본에서 제외하거나 재생성한다.
- QA가 파일·출처·프롬프트·후보/최종 경계를 확인한다.
- 총괄 검토 뒤 사용자에게 품질 판단에 필요한 이미지만 제시한다.
