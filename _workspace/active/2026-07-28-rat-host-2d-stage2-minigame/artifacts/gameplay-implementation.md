# Stage2 게임플레이 구현 기록

## 작업명

2단계 2D 백혈구 회피 미니게임과 성공·실패 인계

## 작업 ID

`2026-07-28-rat-host-2d-stage2-minigame`

## 변경한 코드

- `RatHost2DSessionController`
  - 기존 Stage1 `Configure`를 유지하고 Stage2 연결용 `ConfigureStage2`를 추가했다.
  - Host/Virus 입력과 Collider를 모드별로 상호 배타 활성화한다.
  - 내부 진입, 실패 확인 대기, `MutationSelection` 인계의 root와 카메라 상태를 관리한다.
  - 고유 fragment index를 `HashSet<int>`로 큐잉해 서로 다른 조각의 동일 프레임 수집은 모두 집계하고 같은 index 중복은 거부한다.
  - 조각과 백혈구 접촉을 한 판정 프레임으로 합치며, 치명 접촉과 세 번째 조각이 겹치면 세 번째 조각과 접촉을 같은 `ResolveVirusFrame` 호출에 넣어 기존 성공 우선 규칙을 재사용한다.
  - 실패 대기 중에만 기존 `PrototypeKeyboardInput.WasInteractPressed()`의 Space 입력을 읽고, 확인 뒤 `ReturnToRatHostAfterVirusFailure()`를 호출한다.
  - 진입 때 바이러스·백혈구 위치, 접촉 쿨다운, 조각 활성 상태와 내부 큐를 초기화한다.
- `RatHost2DVirusMovementController`
  - 기존 `TechnicalSample2D.RatHost2DController`를 비활성 단일 collision motor로 합성했다.
  - 바이러스 논리 root 자체를 이동시키며 같은 transform을 `FollowTarget`으로 공개한다.
  - Session의 Virus 활성 상태에서만 입력과 물리 이동을 처리한다.
- `RatHost2DWhiteBloodCellChaser`
  - 내부 활성 중 바이러스 논리 위치를 추적한다.
  - `RatHost2DContactCooldownGate`로 지속 접촉의 중복 피해를 제한한다.
- `RatHost2DMutationFragment`
  - 고유 index와 run별 수집 상태를 가지며 한 번만 Session 큐에 들어간다.
- `RatHost2DVirusHudSnapshot`, `RatHost2DStage2Hud`
  - 안정도, `조각 n/3`, 목표, `면역 포착 +8`, 실패 대기와 성공 인계 상태를 공개·표시한다.

## 변경한 테스트

- `RatHost2DStage2SessionTests` 5개
  - Host/Virus 입력 상호 배타와 성공 인계 1회
  - 세 번째 조각+치명 접촉 성공 우선
  - 동일 프레임 서로 다른 index 2개 집계와 중복 index 거부
  - 실패 확인 전 잠금, 확인 후 RatHost 60%, 무보상
  - 조각+접촉 큐의 1회 합산 판정
- `RatHost2DStage2RuntimeTests` 5개
  - 바이러스 단일 논리 root·FollowTarget·collision motor
  - 접촉 쿨다운 게이트
  - 조각의 run당 단일 수집
  - 실패 복귀 후 재진입 런타임 초기화
  - 백혈구 접촉 창당 안정도 34·포착 8 적용

## 씬/통합 공개 API

- `RatHost2DSessionController.ConfigureStage2(...)`
- `CanProcessVirusGameplay`
- `IsInternalArenaVisible`
- `IsVirusFailureAwaitingConfirmation`
- `IsMutationSelectionHandoff`
- `QueueVirusFragmentCollected(int fragmentIndex)`
- `QueueWhiteBloodCellHit()`
- `FlushQueuedVirusFrame()`
- `ConfirmVirusFailureReturn()`
- `ProcessFailureConfirmationInput(bool confirmPressed)`
- `ReadVirusHud()`, `VirusHudChanged`
- `RatHost2DVirusMovementController.Configure(...)`, `FollowTarget`
- `RatHost2DWhiteBloodCellChaser.Configure(...)`
- `RatHost2DMutationFragment.Configure(...)`
- `RatHost2DStage2Hud.Configure(...)`, `ConfigureStabilitySlider(...)`

## 실행한 검증

- Unity가 외부 씬 변경 Reload 모달에 막힌 상태라 원본 Editor의 EditMode와 PlayMode는 실행하지 않았다.
- 별도 Unity 복제본은 만들지 않았다.
- 기존 Unity Bee `RatHost2D.rsp`와 Unity/NUnit 참조를 재사용해 신규 런타임 및 신규 테스트 소스를 Roslyn으로 정적 컴파일했다.
  - 결과: exit code `0`, C# 문법·타입 오류 `0`
  - 직접 Mono Roslyn 실행과 Unity source generator 버전 차이로 `CS8032` analyzer 경고 3개가 발생했다. 게임 코드 오류는 아니며 정식 Unity 컴파일에서 다시 확인해야 한다.
- 실제 테스트 실행, MCP Play, Console, Windows 빌드는 QA의 단일 임시 복제본 검증으로 인계한다.

## 수용 기준 자체 대조

- Host/Virus 동시 입력 금지: Session 모드 게이트와 테스트로 고정
- 바이러스 단일 root·벽 collision motor: 기존 검증된 2D motor 재사용, 정식 물리 실행은 QA 인계
- 접촉 중복: 0.5초 기본 쿨다운 게이트와 테스트로 고정
- 조각 단일·동일 프레임 복수 집계: 고유 index 집합과 테스트로 고정
- 성공 우선: `PrototypeSessionState.ResolveVirusFrame` 재사용과 동일 프레임 테스트로 고정
- 실패 60%·무보상·확인 잠금: 상태 API 재사용과 테스트로 고정
- 재진입 초기화: 런타임 reset과 테스트로 고정

## 남은 위험

- 실제 2D 물리 Trigger/Collision, 아레나 벽 통과 방지, 카메라 추적, Space 입력, HUD/패널 활성은 씬 Rebuild 뒤 MCP Play로 확인해야 한다.
- 전체 EditMode 회귀와 Windows 임시 빌드는 아직 통과 판정이 없다.
- 사용자 수동 플레이 전에는 조작감과 Stage2 완료를 주장하지 않는다.

## QA 회귀 수정

- QA 전체 EditMode 186개 중 `RatHost2DSessionTests.TransitionDisablesHostRootHudAndCollidersAndShowsShell` 1개가 Stage1 안내 셸 문구 `실제 미니게임`을 계속 요구해 실패했다.
- 런타임의 승인된 Stage2 목표 `변이 조각 3개 수집 / 백혈구 회피`는 유지하고, 해당 테스트만 `변이 조각`과 `백혈구 회피`를 모두 포함하는지 검증하도록 수정했다.
- 런타임, 씬, 다른 테스트는 변경하지 않았다. QA 단일 복제본에서 전체 186개를 다시 실행한다.
