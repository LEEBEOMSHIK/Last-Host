# 열린 질문

작업 ID: `2026-06-29-rat-host-plan-agent`

## 사용자 결정 필요

1. `docs/prototype/rat-host-approval-packet.md` 추천안을 전체 승인할지, 일부 수정할지
2. 1차 프로토타입 입력 구현에 Unity Input System을 사용할지
3. 새 입력 액션 자산 후보 위치를 승인할지
   - `Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`
4. `RatHostPrototype.unity` 생성을 승인할지
5. `Assets/_Project/` 하위 폴더 생성을 승인할지
6. `RatHostPrototype.unity`를 Build Settings 시작 씬으로 등록할지
7. 기존 `Assets/Scenes/SampleScene.unity`를 삭제하지 않고 빌드 대상에서 제외하거나 뒤로 미룰지
8. 포유류 적응의 1차 임시 효과를 다음 중 무엇으로 둘지
   - 하수도 맵의 특정 통로 접근
   - 보상 지점 접근
   - 다른 임시 효과
9. 실패 후 재시도 초기화 기준을 초안대로 둘지, 수정할지
10. 구현 후 테스트와 빌드 실행 범위를 어디까지 요구할지
11. Unity MCP를 통한 씬, 에셋, 코드, ProjectSettings, Build Settings 변경 범위를 승인할지
12. 구현 계획 초안을 `docs/rat-host-implementation-plan.md` 공식 문서로 승격할지

## 확정된 결정

- 쥐는 벌레보다 상위 숙주에 가깝지만, 1차 프로토타입은 쥐 숙주 수직 슬라이스로 진행한다.
- 벌레 튜토리얼은 1차 프로토타입에 포함하지 않는다.
- 구현 계획 초안은 사용자 승인 전까지 공식 `docs/` 문서가 아니라 `_workspace/active/.../artifacts/` 산출물 상태로 유지한다.

## Unity 아키텍처 확인 필요

1. Input System 사용 시 Player Settings의 Active Input Handling 변경이 필요한지
2. `RatHostPrototype.unity` 생성 후 Build Settings에 어떤 순서로 등록할지
3. `SampleScene.unity`를 빌드 대상에서 제외할지, 뒤로 미룰지
4. 테스트 폴더 구조를 1차 구현에 포함할지, QA 작업에서 분리할지

## QA 확인 필요

1. 내부 구현 완료 조건에 포함할 EditMode 테스트 범위
2. 내부 구현 완료 조건에 포함할 최소 PlayMode 테스트 범위
3. 플레이어블 프로토타입 완료 조건에 PC 빌드 검증을 포함할지
4. 실패 후 재시도 초기화 기준 초안의 유지/초기화 값이 적절한지
5. UI와 카메라 확인을 `1920x1080`, `1600x900`, `1366x768`, `1280x720` 네 해상도로 고정할지
