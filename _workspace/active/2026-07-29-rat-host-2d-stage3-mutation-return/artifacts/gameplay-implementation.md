# 게임플레이 구현 결과

## 작업명

3단계 2D 변이 선택·효과·쥐 숙주 복귀

## 작업 ID

`2026-07-29-rat-host-2d-stage3-mutation-return`

## 담당

게임플레이 구현 에이전트

## 변경한 코드

- `RatHost2DSessionController`
  - `TrySelectMutation(MutationType)` 공개 선택 API
  - `ProcessMutationSelectionInput(PrototypeInputState)` 숫자키 `1/2/3` 입력 API
  - 선택 직후 Host root·카메라·입력·Collider 복귀와 내부 런타임·큐 초기화
  - 중복 선택·선택 모드 밖 명령·정의되지 않은 enum 값 거부
  - 잠복 강화 보유 시 `ApplyContaminationExposure`의 면역 상승량에 `0.55` 배율 적용
  - `CanUseMammalPassage` 공개 상태 제공
- `RatHost2DMutationOptionButton`
  - UI Button을 `TrySelectMutation`에 연결하는 2D 전용 어댑터
- `RatHost2DMutationStatusDisplay`
  - 현재 적용된 변이를 Host HUD Text에 표시하는 어댑터
- `RatHost2DMammalPassageGate`
  - 지정된 단일 통로 Collider와 SpriteRenderer만 갱신하는 2D gate

기존 `PrototypeSessionState.SelectMutation`의 성공 복귀값
`25% + VirusPatternExposureTotal`과 `MutationLoadout`의 승인 수치를 재사용했다.
시간 자동 면역 상승은 `0`을 유지했다.

## 변경한 테스트

`RatHost2DStage3MutationTests` 6개:

1. 포착 `+8`을 가진 성공 선택이 면역 `33%`로 한 번만 복귀하고 Host 전용 상태를 복원
2. 동시 `1/2/3` 입력과 반복 입력에서도 첫 변이 하나만 적용
3. 잠복 강화가 오염 면역 상승량만 `0.55`배로 줄이고 체력 피해·무위험 대기는 유지
4. 신경 조종이 실제 2D control ratio, 강제 조종 해제, motor speed와 이동 step에 반영
5. 포유류 적응이 지정 통로 Collider만 열고 무관한 벽 Collider를 유지
6. 2D 선택 Button과 적용 변이 Text 어댑터 계약

## 수용 기준 대조

- 성공한 경우에만 선택 가능: 충족
- 한 보상에서 한 변이만 적용: 충족
- 성공 복귀 `25% + 포착값`: 충족
- 내부 런타임·큐 초기화: 충족
- Host root·카메라·입력·충돌 활성 복귀: 코드 계약 충족
- 잠복 강화 오염 상승 `0.55`: 충족
- 신경 조종 2D 실제 이동 반영: 충족
- 포유류 적응 공개 상태와 지정 gate 경계: 충족
- 실패 무보상: 기존 Stage2 경로를 변경하지 않음

## 실행한 검증

- Unity Assets Refresh 후 컴파일 완료
- Editor asmdef 통합 참조 보완 후 Console Error `0`, Warning `0`
- 신규 Stage3 EditMode: `6/6 PASS`, 실패·건너뜀·불확정 `0`
- `LastHost.Prototype.RatHost2D.Tests` 전체 EditMode:
  `53/53 PASS`, 실패·건너뜀·불확정 `0`
- `git diff --check`: 담당 Scripts/Tests 경로 이상 없음
- Windows 빌드: 생성하지 않음

프로젝트 전체 EditMode 및 원본 MCP Play는 독립 QA 게이트에서 다시 실행한다.

## 씬/통합 인계

- 버튼: `RatHost2DMutationOptionButton.Configure(session, type, label)`
- 적용 변이 Text:
  `RatHost2DMutationStatusDisplay.Configure(session, text)`
- 지정 통로:
  `RatHost2DMammalPassageGate.Configure(session, collider, renderer)`
- 통로 색:
  `RatHost2DMammalPassageGate.ConfigureColors(blocked, open)`
- 공개 상태: `RatHost2DSessionController.CanUseMammalPassage`

통로 어댑터는 전달받은 Collider만 변경하며 Tilemap 또는 다른 Collider를
검색하거나 비활성화하지 않는다.

## 남은 위험

- 씬의 세 버튼, 적용 변이 Text, 지정 통로 직렬화 연결은 Unity 씬/통합
  구현 에이전트가 검증해야 한다.
- 실제 키보드 `1/2/3`, Button 클릭, Host 카메라 복귀와 gate 통과는 원본
  MCP Play에서 독립 QA가 확인해야 한다.
- 전체 EditMode 회귀와 사용자 체감 확인 전 완료로 선언하지 않는다.
