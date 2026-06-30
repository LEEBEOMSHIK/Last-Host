# 승인 완료 기록

작업 ID: `2026-06-29-rat-host-plan-agent`

## 사용자 승인

- 승인일: 2026-06-30
- 사용자 응답: `일단 프로토 타입이므로 다 승인할게`
- 처리: 아래 항목을 전체 승인으로 기록한다.

## 승인된 결정

1. `docs/prototype/rat-host-approval-packet.md` 추천안 전체 승인
2. 1차 프로토타입 입력 구현에 Unity Input System 사용
3. 새 입력 액션 자산 후보 위치 승인
   - `Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`
4. `RatHostPrototype.unity` 생성 승인
5. `Assets/_Project/` 하위 폴더 생성 승인
6. `RatHostPrototype.unity`를 Build Settings 시작 씬으로 등록
7. 기존 `Assets/Scenes/SampleScene.unity`는 삭제하지 않고 빌드 대상에서 제외하거나 뒤로 미룸
8. `포유류 적응`의 1차 프로토타입 임시 효과는 하수도 맵의 특정 통로 접근
9. 실패 후 재시도 초기화 기준은 초안대로 승인
10. 구현 후 테스트와 빌드 실행 범위는 공식 구현 계획의 검증 기준대로 승인
11. Unity MCP를 통한 승인 범위 내 씬, 에셋, 코드, ProjectSettings, Build Settings 변경 승인
12. 구현 계획 초안을 `docs/prototype/rat-host-implementation-plan.md` 공식 문서로 승격

## 확정된 결정

- 쥐는 벌레보다 상위 숙주에 가깝지만, 1차 프로토타입은 쥐 숙주 수직 슬라이스로 진행한다.
- 벌레 튜토리얼은 1차 프로토타입에 포함하지 않는다.
- 구현 계획 초안은 공식 요약 문서 `docs/prototype/rat-host-implementation-plan.md`로 승격했다.

## Unity 아키텍처 반영 결과

1. Input System 사용 승인
2. `RatHostPrototype.unity`를 Build Settings 시작 씬으로 등록
3. `SampleScene.unity`는 삭제하지 않고 빌드 대상에서 제외하거나 뒤로 미룸
4. 테스트 폴더 구조는 구현 중 검증 필요에 따라 추가

## QA 반영 결과

1. EditMode 테스트는 핵심 상태 로직 중심으로 포함
2. PlayMode 테스트는 최소 모드 전환과 실패 재시도 흐름 포함
3. 플레이어블 프로토타입 완료 조건에 PC 빌드 검증 포함
4. 실패 후 재시도 초기화 기준은 초안대로 승인
5. UI와 카메라 확인은 `1920x1080`, `1600x900`, `1366x768`, `1280x720` 네 해상도로 고정
