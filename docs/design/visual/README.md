# 비주얼 제작 문서

`docs/design/visual/`은 `마지막 숙주`의 도트풍 저폴리 3D 비주얼을 일관되게 만들기 위한 제작 기준을 둔다. 이 폴더는 실제 Unity 설정값이나 에셋 파일이 아니라, 아트·테크 아트·씬 작업 전에 합의할 판단 기준을 보관한다.

## 확인 순서

1. `pixel-lowpoly-3d-production-guide.md`에서 3D 형태, 픽셀 텍스처, 렌더링, 카메라, 조명 기준과 씬 검토 순서를 확인한다.
2. AI 보조 래스터 초안을 실제로 생성하려면 `../../prototype/plans/rat-host-ai-assisted-art-workflow.md`에서 자산 묶음 승인, 기록, 선별, 후속 3D/Unity 분리 절차를 확인한다.
3. 실제 모델, 텍스처, URP Renderer, 카메라, 포스트 프로세싱을 변경하기 전에는 해당 작업 패킷에서 이 가이드의 권장값을 프로젝트 조건에 맞게 검증한다.
4. 쥐 숙주 프로토타입의 플레이 가능 범위는 `../../prototype/official/rat-host-prototype.md`를 우선한다.

## 현재 문서

- `pixel-lowpoly-3d-production-guide.md`: 도트풍 저폴리 3D의 모델·텍스처·URP 렌더링·카메라·UI·검토 기준.
- `../../prototype/plans/rat-host-ai-assisted-art-workflow.md`: AI 보조 래스터 초안의 자산 묶음 승인·기록·선별과 저폴리 3D·Unity 적용 분리 계획.

## 범위

- 이 폴더의 수치와 예시는 결정된 실제 설정이 아니라 검증을 시작하기 위한 문서 기준이다.
- 실제 Unity 설정, URP Renderer, 에셋 Import, 모델·텍스처 제작과 적용은 별도 사용자 승인과 검증을 거친다.
- 이 문서는 순수 2D 스프라이트 게임 전환이나 쥐 숙주 프로토타입 범위 확장을 제안하지 않는다.
