# 비주얼 제작 문서

`docs/design/visual/`은 `마지막 숙주`의 도트풍 하이브리드 2.5D 비주얼을 일관되게 만들기 위한 제작 기준을 둔다. 3D 환경과 게임플레이 루트, 저폴리 3D 캐릭터 원본, 8방향 프리렌더드 스프라이트의 경계를 다루며 실제 Unity 설정값이나 에셋 파일은 보관하지 않는다.

## 확인 순서

1. `graphics-direction-management.md`에서 그래픽 방향, 참고 게임의 허용된 판단 범위, 현재 기준과 시험안의 경계를 확인한다.
2. `pixel-lowpoly-3d-production-guide.md`에서 3D 환경, 캐릭터 원본·8방향 스프라이트, 픽셀 표면, 렌더링, 카메라, 조명 기준과 씬 검토 순서를 확인한다.
3. AI 보조 래스터 초안을 실제로 생성하려면 `../../prototype/plans/rat-host-ai-assisted-art-workflow.md`에서 자산 묶음 승인, 기록, 선별, 후속 3D/Unity 분리 절차를 확인한다.
4. 실제 모델, 텍스처, URP Renderer, 카메라, 포스트 프로세싱을 변경하기 전에는 해당 작업 패킷에서 이 가이드의 권장값을 프로젝트 조건에 맞게 검증한다.
5. 쥐 숙주 프로토타입의 플레이 가능 범위는 `../../prototype/official/rat-host-prototype.md`를 우선한다.

## 현재 문서

- `graphics-direction-management.md`: 그래픽 방향 관리 기준, Dave the Diver·The Binding of Isaac의 그래픽 판단 원칙, 공통 기준과 시험안의 수용 경계.
- `pixel-lowpoly-3d-production-guide.md`: 도트풍 하이브리드 2.5D의 3D 환경·캐릭터 원본·8방향 스프라이트·URP 렌더링·카메라·UI·검토 기준.
- `../../prototype/plans/rat-host-ai-assisted-art-workflow.md`: AI 보조 래스터 초안의 승인·선별과 캐릭터 3D 원본·방향 렌더·Unity 적용 분리 계획.

## 범위

- 이 폴더의 수치와 예시는 결정된 실제 설정이 아니라 검증을 시작하기 위한 문서 기준이다.
- 실제 Unity 설정, URP Renderer, 에셋 Import, 모델·텍스처 제작과 적용은 별도 사용자 승인과 검증을 거친다.
- 이 문서는 3D 환경·게임플레이 루트를 폐기하는 순수 2D 전환이나 쥐 숙주 프로토타입 범위 확장을 제안하지 않는다.
