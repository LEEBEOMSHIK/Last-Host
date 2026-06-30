# 쥐 숙주 구현 계획 보조 검토 통합 메모

작성일: 2026-06-29

## 입력 산출물

- `_workspace/active/2026-06-29-rat-host-architecture-review/artifacts/architecture-review.md`
- `_workspace/active/2026-06-29-rat-host-qa-review/artifacts/qa-review.md`
- `_workspace/active/2026-06-29-rat-host-plan-agent/artifacts/rat-host-implementation-plan-draft.md`

## 공통 결론

- `RatHostPrototype` 단일 씬으로 1차 프로토타입을 시작하는 방향은 적합하다.
- `Assets/_Project/` 아래에 게임 전용 폴더를 두는 방향은 적합하다.
- 기존 `Assets/Scenes/SampleScene.unity`는 당장 삭제하지 않는다.
- 쥐 숙주 핵심 루프 계획은 전체 흐름을 대체로 덮고 있다.
- 실제 Unity 씬, C# 코드, ProjectSettings, Build Settings 변경은 사용자 승인 후 진행해야 한다.

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
- `포유류 적응` 임시 효과를 구현 전 확정해야 한다.
- 실패 후 재시도 시 초기화할 값과 유지할 값을 구현 전 명시해야 한다.
- 입력 방식이 Input System인지 `UnityEngine.Input`인지 구현 전 확정해야 한다.
- UI 확인 기준 해상도는 16:9 PC 기준을 사용한다.
  - `1920x1080`
  - `1600x900`
  - `1366x768`
  - `1280x720`
- 카메라 프레이밍은 쥐 모드와 바이러스 모드를 별도로 확인한다.

## 구현 전 사용자 승인 필요

1. `docs/prototype/rat-host-approval-packet.md` 추천안 전체 승인 또는 수정 승인
2. Input System 사용 여부
3. `RatHostPrototype.unity` 생성 여부
4. `RatHostPrototype.unity`를 Build Settings 시작 씬으로 둘지 여부
5. `SampleScene.unity`를 빌드 대상에서 제외하거나 뒤로 미룰지 여부
6. `포유류 적응`의 1차 프로토타입 임시 효과
   - 특정 통로 접근
   - 보상 지점 접근
   - 다른 임시 효과
7. 실패 후 재시도 초기화 기준
8. 구현 계획 초안을 공식 `docs/rat-host-implementation-plan.md`로 승격할지 여부
9. 구현 후 테스트/빌드 실행 범위

## 다음 작업 제안

다음 작업은 게임플레이 루프 에이전트가 보조 검토 결과를 반영해 `rat-host-implementation-plan-draft.md`를 갱신하는 것이다.

반영할 내용:

- `Settings/Input/` 구조 추가
- Input System 권장안 반영
- Build Settings와 `SampleScene.unity` 처리 방침 반영
- QA 완료 조건 추가
- 재시도 초기화 기준 초안 추가
- 사용자 승인 필요 항목 갱신

그 다음 조정자가 사용자에게 승인 질문을 압축해서 제시한다.
