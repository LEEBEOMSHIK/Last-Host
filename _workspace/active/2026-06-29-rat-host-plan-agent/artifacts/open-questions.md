# 열린 질문

작업 ID: `2026-06-29-rat-host-plan-agent`

## 사용자 결정 필요

1. `docs/rat-host-approval-packet.md` 추천안을 전체 승인할지, 일부 수정할지
2. 포유류 적응의 1차 임시 효과를 다음 중 무엇으로 둘지
   - 하수도 맵의 특정 통로 접근
   - 보상 지점 접근
   - 다른 임시 효과
3. 구현 계획 초안을 `docs/rat-host-implementation-plan.md` 공식 문서로 승격할지

## Unity 아키텍처 확인 필요

1. `RatHostPrototype` 단일 씬으로 시작할지
2. `RatHostPrototype.unity`를 Build Settings에 추가할지
3. 초기 입력 구현에서 Unity Input System을 직접 사용할지
4. `Assets/Scenes/SampleScene.unity`를 유지할지

## QA 확인 필요

1. 구현 완료 조건에 PC 빌드 검증을 포함할지
2. EditMode/PlayMode 테스트를 어떤 범위까지 요구할지
3. 수동 플레이 체크 기준 화면 크기를 무엇으로 둘지
