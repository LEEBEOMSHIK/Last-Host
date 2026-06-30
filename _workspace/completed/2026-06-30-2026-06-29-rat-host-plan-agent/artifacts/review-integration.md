# 쥐 숙주 구현 계획 보조 검토 통합 메모

작성일: 2026-06-29
승인 반영일: 2026-06-30

## 입력 산출물

- `_workspace/completed/2026-06-30-2026-06-29-rat-host-architecture-review/artifacts/architecture-review.md`
- `_workspace/completed/2026-06-30-2026-06-29-rat-host-qa-review/artifacts/qa-review.md`
- `_workspace/completed/2026-06-30-2026-06-29-rat-host-plan-agent/artifacts/rat-host-implementation-plan-draft.md`

## 공통 결론

- `RatHostPrototype` 단일 씬으로 1차 프로토타입을 시작하는 방향은 적합하다.
- `Assets/_Project/` 아래에 게임 전용 폴더를 두는 방향은 적합하다.
- 기존 `Assets/Scenes/SampleScene.unity`는 당장 삭제하지 않는다.
- 쥐 숙주 핵심 루프 계획은 전체 흐름을 대체로 덮고 있다.
- 실제 Unity 씬, C# 코드, ProjectSettings, Build Settings 변경은 2026-06-30 사용자 전체 승인 범위 안에서 진행할 수 있다.

## 아키텍처 반영 권장

- `Assets/_Project/Settings/Input/`을 구조 후보에 추가한다.
- 1차 프로토타입도 Input System 사용을 권장한다.
- 기존 템플릿 입력 자산 `Assets/InputSystem_Actions.inputactions`는 바로 수정하지 않는다.
- 새 입력 액션 자산을 만들 경우 후보 위치는 `Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions`다.
- `RatHostPrototype.unity` 생성 후에는 Build Settings에 시작 씬으로 등록하는 방향을 권장한다.
- `SampleScene.unity`는 파일은 유지하되, 프로토타입 빌드 대상에서는 제외하거나 뒤로 미룬다.

## QA 반영 권장

- 구현 완료 조건을 단계별로 나눈다.
  - 내부 구현 완료: 컴파일 오류 없음, 핵심 로직 테스트 또는 대체 검증, 성공/실패 수동 루프 확인
  - 플레이어블 프로토타입 완료: Windows PC 빌드 성공과 빌드 실행본 루프 확인
- `포유류 적응` 임시 효과는 `특정 통로 접근`으로 확정했다.
- 실패 후 재시도 시 초기화할 값과 유지할 값은 구현 계획 초안대로 승인되었다.
- 입력 방식은 Input System 사용으로 확정했다.
- UI 확인 기준 해상도는 16:9 PC 기준을 사용한다.
  - `1920x1080`
  - `1600x900`
  - `1366x768`
  - `1280x720`
- 카메라 프레이밍은 쥐 모드와 바이러스 모드를 별도로 확인한다.

## 사용자 승인 완료

1. `docs/prototype/approvals/rat-host-approval-packet.md` 추천안 전체 승인
2. Input System 사용 승인
3. `RatHostPrototype.unity` 생성 승인
4. `RatHostPrototype.unity`를 Build Settings 시작 씬으로 등록 승인
5. `SampleScene.unity`는 빌드 대상에서 제외하거나 뒤로 미룸
6. `포유류 적응`의 1차 프로토타입 임시 효과는 특정 통로 접근
7. 실패 후 재시도 초기화 기준은 초안대로 승인
8. 구현 계획 초안은 공식 `docs/prototype/plans/rat-host-implementation-plan.md`로 승격
9. 구현 후 테스트/빌드 실행 범위는 공식 구현 계획 기준대로 승인

## 다음 작업 제안

다음 작업은 승인된 공식 구현 계획을 기준으로 Unity 구현 작업 패킷을 만드는 것이다.
