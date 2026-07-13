# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-13-ai-assisted-pixel-art-workflow`
- 작업명: AI 보조 도트풍 3D 아트 제작 규칙과 작업 순서
- 상태: 완료
- 생성일: 2026-07-13
- 담당 에이전트: 문서/릴리즈 에이전트
- 보조 에이전트: QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `pixel-lowpoly-style-keeper`

## 목적

ChatGPT 이미지 생성을 도트풍 텍스처·콘셉트·UI 보조로 안전하게 사용하고, 저폴리 3D 모델·UV·리깅·Unity 적용은 별도 3D/Unity 단계로 분리하는 규칙과 쥐 숙주 프로토타입용 작업 순서를 문서화한다.

## 산출물

- `docs/design/visual/pixel-lowpoly-3d-production-guide.md`의 AI 보조 제작 규칙
- `docs/prototype/plans/rat-host-ai-assisted-art-workflow.md`의 승인 게이트·작업 순서·구성·검증 계획
- 관련 README·참조 색인·상태판·작업 기록

## 금지 범위

- 실제 이미지, 텍스처, 모델, UI 에셋을 생성하지 않는다.
- Unity 씬, URP, Import, ProjectSettings, 코드, 패키지를 변경하지 않는다.
- AI 생성 이미지를 최종 3D 메시·UV·리깅의 자동 대체물로 규정하지 않는다.

## 완료 기준

- AI 이미지 생성의 허용 용도, 금지/제한, 승인·기록·선별 규칙이 명시된다.
- 승인 → 이미지 생성 → 선별/정리 → 저폴리 모델·UV → Unity 적용 → 시각/플레이 검증 순서와 역할이 계획에 있다.
- 프로토타입 범위와 현재 승인 게이트를 넘지 않는다고 QA·총괄 검토가 확인한다.
