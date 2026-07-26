# 비주얼 제작 문서

`docs/design/visual/`은 `마지막 숙주`의 현재 승인된 2D 아이소메트릭 도트 방향과, 이전 하이브리드 2.5D 방향의 이력을 구분해 관리한다.

## 확인 순서

1. `references/README.md`와 `references/rat-host-2d-isometric-gameplay-mockup-v1.png`에서 목표 화면의 용도와 한계를 확인한다.
2. `graphics-direction-management.md`에서 현재 그래픽 방향과 수용 경계를 확인한다.
3. `pixel-isometric-2d-production-guide.md`에서 타일, 스프라이트, 픽셀 격자, 정렬, 카메라와 기술 샘플 기준을 확인한다.
4. ChatGPT 이미지 생성이 필요하면 `../../prototype/plans/rat-host-ai-assisted-art-workflow.md`와 `.agents/chatgpt-image-art-agent.md`를 따른다.
5. 기존 구현이나 과거 산출물을 해석할 때만 `pixel-lowpoly-3d-production-guide.md`를 레거시 문서로 확인한다.

## 현재 문서

- `graphics-direction-management.md`: 현재 2D 아이소메트릭 도트 그래픽 방향과 reference·수용 관리 기준.
- `pixel-isometric-2d-production-guide.md`: 2D 타일·캐릭터 스프라이트·깊이 정렬·픽셀 출력·QA 제작 기준.
- `references/`: 승인된 목표 화면 reference와 출처·용도·한계.
- `../../prototype/plans/rat-host-ai-assisted-art-workflow.md`: OpenAI 내장 `imagegen`을 이용한 후보 생성, 기록, 선별, 후속 제작·Unity 적용 분리 절차.
- `pixel-lowpoly-3d-production-guide.md`: 2026-07-27 이전 하이브리드 2.5D/Blender 프리렌더 제작 이력. 신규 제작 기본 경로가 아니다.

## 범위

- Unity 엔진, PC 우선, 쥐 숙주 핵심 루프는 유지한다.
- 환경·캐릭터·효과의 신규 제작 기본 경로는 2D 아이소메트릭 타일과 스프라이트다.
- 목업과 AI 생성 결과는 목표·후보 자료이며 실제 타일셋이나 최종 스프라이트로 자동 승인하지 않는다.
- 실제 Unity 설정, 에셋 Import, 타일맵·씬·코드 적용은 별도 작업 패킷과 사용자 승인·검증을 거친다.
