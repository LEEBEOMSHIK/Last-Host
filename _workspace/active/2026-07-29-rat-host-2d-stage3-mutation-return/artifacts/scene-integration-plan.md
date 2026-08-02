# Stage3 씬·UI 통합 계획

## 담당과 변경 경계

- 담당: Unity 씬/통합 구현 에이전트
- 변경 허용: `RatHost2DPrototypeSceneBuilder.cs`, `RatHost2DPrototype.unity`, 이 작업 패킷 기록
- 변경 금지: 게임플레이 `Scripts/**`, `Tests/**`, `ProjectSettings/**`, `Packages/**`, 입력 액션 에셋, 기존 3D·2D 기술 샘플
- 산출물 성격: 최종 아트가 아닌 2D 기술 플레이스홀더

## 씬 구조안

```text
RatHost2DPrototype
  HostMode2D
    World2D
      MammalAdaptationPassage2D
        PassageOpeningMarker
        PassageGate
  UI2D
    HostHud2D
      AppliedMutationText
  MutationSelectionShell2D
    MutationSelectionBackdrop
      MutationSelectionTitle
      MutationSelectionInstruction
      MutationOption1_Dormancy
      MutationOption2_NeuralControl
      MutationOption3_MammalAdaptation
```

## 연결 계약

게임플레이 구현부에 다음 2D 공개 계약이 필요하다.

1. `RatHost2DSessionController.ConfigureStage3(...)`
   - 변이 선택 루트
   - 적용 변이 표시 `Text`
   - 포유류 적응 전용 `Collider2D`
   - 포유류 적응 통로의 차단/개방 표시용 `SpriteRenderer`
2. `RatHost2DSessionController.SelectMutation(MutationType)` 또는 같은 의미의 공개 메서드
   - 현재 모드가 `MutationSelection`일 때만 한 번 성공
   - 성공 시 선택 변이 적용, 내부 런타임 초기화, Host 복귀 및 HUD·카메라·입력·Collider 상태 갱신
3. 버튼 연결용 2D 컴포넌트
   - `RatHost2DSessionController`와 `MutationType`을 직렬화하여 `Button.onClick`에서 선택 메서드를 호출
   - 숫자키 `1/2/3` 입력은 세션 컨트롤러의 런타임 입력 처리에서 동일 선택 경로를 사용
4. 적용 상태 표시
   - 미적용 시 `적용 변이: 없음`
   - 적용 후 선택한 한국어 변이명과 핵심 효과를 표시
5. 포유류 적응 통로
   - 전용 `BoxCollider2D`만 변이 적용 시 비활성
   - 다른 TilemapCollider2D, 수로, 소품 Collider2D는 변경하지 않음

## UI·비주얼 기준

- `960x540` 후보 기준 `CanvasScaler`를 재사용한다.
- 세 선택지는 숫자키와 버튼 이름이 한눈에 대응되게 한다.
- 버튼에는 변이명과 프로토타입 효과를 함께 표시한다.
- 선택 패널은 기존 성공 인계 셸을 교체하고 다른 HUD보다 높은 정렬 순서를 유지한다.
- 적용 변이 표시는 Host HUD 우측 상단의 모드 표시 아래에 배치한다.
- 통로는 월드 플레이스홀더 색으로 닫힘/개방 상태를 구분하되, 최종 타일·스프라이트로 선언하지 않는다.

## 보존 계약

- 기존 Floor `117`, Water `5`, Walls `40` Tilemap 셀을 그대로 보존한다.
- Stage2 내부 아레나·백혈구·변이 조각·실패 패널·카메라 연결을 보존한다.
- 기존 Host/내부 카메라와 Y정렬 설정을 변경하지 않는다.
- 포유류 적응 전용 통로 외 Collider 활성 상태를 변이로 바꾸지 않는다.
- Windows 빌드는 생성하지 않는다.

## 통합 후 확인

- 원본 씬 Rebuild·Save 후 `sceneDirty=false`
- 선택 UI 세 버튼과 숫자키 안내 존재
- 적용 변이 표시 존재
- 지정 통로 전용 Collider만 세션에 연결
- Floor/Water/Walls 셀 수 보존
- Stage2 루트와 내부 런타임 보존
- Unity Console Error/Warning 확인
- 실제 효과와 입력은 독립 QA 에이전트가 MCP Play로 검증
