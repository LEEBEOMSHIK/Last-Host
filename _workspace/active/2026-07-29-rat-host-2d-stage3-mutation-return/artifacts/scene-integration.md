# Stage3 씬·UI 통합 결과

## 작업명

3단계 2D 변이 선택·효과·쥐 숙주 복귀 씬 통합

## 담당 수행 주체

Unity 씬/통합 구현 에이전트

## 변경한 씬·설정

- `UnityProject/Assets/_Project/Editor/RatHost2D/RatHost2DPrototypeSceneBuilder.cs`
- `UnityProject/Assets/_Project/Editor/RatHost2D/LastHost.Prototype.RatHost2D.Editor.asmdef`
- `UnityProject/Assets/_Project/Scenes/RatHost2DPrototype.unity`

Editor asmdef에는 빌더가 `MutationType`을 정식 참조할 수 있도록 기존 런타임 어셈블리 `LastHost.Prototype` 참조만 추가했다. 새 패키지나 ProjectSettings는 변경하지 않았다.

## 연결한 컴포넌트

- `MutationSelectionShell2D`
  - `MutationOption1_Dormancy`
  - `MutationOption2_NeuralControl`
  - `MutationOption3_MammalAdaptation`
- 각 선택지
  - Unity UI `Button`
  - 숫자키 `1/2/3` 안내
  - `RatHost2DMutationOptionButton`
  - 한국어 변이명·프로토타입 효과 문구
- `HostHud2D/AppliedMutationText`
  - `RatHost2DMutationStatusDisplay`
- `MammalAdaptationPassage2D/PassageGate`
  - `BoxCollider2D`
  - `SpriteRenderer`
  - `RatHost2DMammalPassageGate`
- `EventSystem2D`
  - `EventSystem`
  - `InputSystemUIInputModule`

## 2D 충돌·정렬·카메라 확인

- 포유류 적응 전용 `PassageGate`의 `BoxCollider2D`만 해당 변이로 개폐한다.
- 기존 `BlockingTilemap`·`WaterTilemap`의 `TilemapCollider2D` 두 개는 별도 컴포넌트로 유지했다.
- 저장 후 Tilemap 셀 수:
  - Floor `117`
  - Water `5`
  - Blocking wall `40`
- Host 카메라는 활성, 내부 루트·카메라는 초기 비활성 상태를 유지했다.
- 카메라 캡처에서 기존 하수도 방·외곽 경계·수로·쥐·소품과 우측 상단의 전용 적색 차단막이 함께 표시됐다.
- 기존 Y정렬 컴포넌트와 카메라 설정은 변경하지 않았다.

## Unity MCP 확인

- Assets Refresh 및 컴파일 완료
- Stage3 Rebuild 메뉴 실행 성공
- 활성 씬: `Assets/_Project/Scenes/RatHost2DPrototype.unity`
- 저장 상태: `sceneDirty=false`
- Console: Error `0`, Warning `0`
- 게임플레이 담당의 전체 RatHost2D EditMode 종료 후 원본 씬을 다시 로드해 `isPlaying=false`, `isPaused=false`, `sceneDirty=false`, Console Error/Warning `0`을 최종 재확인했다.
- 직렬화 계약 조회:
  - 선택 버튼 `3`, 타입 `Dormancy`, `NeuralControl`, `MammalAdaptation`
  - gate Collider/Renderer 참조 일치
  - 적용 변이 Text 참조 일치
  - EventSystem/InputSystem UI 모듈 존재
- 기본 Play 확인은 게임플레이 담당의 EditMode 테스트 전환과 동시에 실행되어 임시 테스트 씬으로 바뀌었으므로 무효 처리하고 즉시 Stop했다. 원본 씬은 다시 로드했으며 실제 Stage3 Play는 독립 QA에 넘긴다.

## 기존 구현 보존 상태

- Stage2 내부 아레나, 바이러스, 백혈구, 변이 조각 3개, 실패 패널 보존
- 기존 3D 씬·2D 기술 샘플·입력 asset·Packages 보존
- 사용자 `ProjectSettings.asset` 변경과 `_workspace/previews/` 미수정
- Windows 빌드 미생성

## 남은 위험

- 선택 화면의 실제 16:9 가독성과 마우스 클릭 수신은 QA Play 확인이 필요하다.
- 실제 숫자키 `1/2/3`, 선택 직후 Host 복귀, 적용 텍스트, gate 개방과 다른 충돌 보존은 독립 QA 대상이다.
- 통로 표현은 기술 플레이스홀더이며 최종 타일·스프라이트가 아니다.
