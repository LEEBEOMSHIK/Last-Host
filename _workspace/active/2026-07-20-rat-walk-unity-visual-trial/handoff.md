# 핸드오프

## 현재 상태

- 상태: v3 동작 폭 확대 Unity 반입 후, 사용자 피드백에 따라 기본 카메라를 고정 쿼터뷰로 직렬화했고, 프리렌더 수평 축의 좌우 반전을 시각 계층에서 보정했다. 활성 씬이 dirty여서 Save·Reload·추가 Play는 하지 않았으며, 깨끗한 재로드 후 실제 Play·사용자 체감·EditMode 실행 결과·독립 QA·총괄 판정 대기
- 사용자 승인: v2 보행을 Unity 실제 게임 화면에서 시험 반입·검증
- 입력: Blender v2 64 PNG, 프레임 맵, 저해상도·공통 피벗 기준

## 범위

- 별도 `WalkTrialV2` Unity 시험 자산만 추가한다.
- 기존 정지 자산, Blender 원본, 게임플레이 판정, Unity 설정은 보존한다.

## 다음 작업

1. 미저장 변경이 없는 Unity 씬을 다시 열어 기본값이 `QuarterView`·orthographic인지 실제 Play에서 확인한다. V 키를 누르지 않은 상태에서만 쥐 이동 방향과 프레임 안정성을 판단한다.
2. V 토글은 코드상 여전히 활성이다. 고정 카메라를 제품/시험에서 잠글지 여부는 게임플레이 코드 담당에 별도 배정한다.
3. QA/검증 에이전트가 대상 EditMode 테스트와 작업 기록·상태판을 독립 대조하고, 총괄 관리자가 판정한다.

## 카메라 시점 정정 (2026-07-21)

- `RatHostPrototype.unity`: `PrototypeCameraController.startingHostMode: 1` (`QuarterView`).
- `RatHostPrototypeSceneBuilder.cs`: 재생성 시에도 `PrototypeCameraMode.QuarterView`를 설정해 씬과 생성 경로의 불일치를 막았다.
- 변경 당시 활성 Unity 씬이 dirty여서, 미저장 변경 보호를 위해 Save·재로드·Play는 의도적으로 생략했다. MCP 읽기 점검에서 메모리상 기존 `ThirdPerson` 상태를 확인했으므로, 다음 검증은 반드시 깨끗한 재로드 뒤 수행해야 한다.
- V 키는 UI에는 노출되지 않지만 코드상 카메라 3모드 순환 기능으로 남아 있다. 이 작업의 코드 변경 금지 범위에서는 비활성화하지 않았다.
- QA Play 종료 후에는 저장·재로드 없이 활성 `IsometricCamera` 인스턴스에도 `QuarterView`와 orthographic을 즉시 적용했다. MCP 실행 결과: `starting=QuarterView`, `current=QuarterView`, `orthographic=True`. 이 메모리 상태에서 QA Play 재검증이 가능하다.

## 적용된 씬/통합 결과 (2026-07-20)

- `WalkTrialV2` 64장은 Sprite(Single), PPU 32, Pivot `(0.5, 0.25)`, Point, 무압축, mipmap 없음으로 적용됐다.
- `RatVisual`의 1.35배 확대와 비례 접지 오프셋은 화면 점유율 비교안으로 Play 검증했으나, 사용자 지시에 따라 원래 값으로 되돌렸다. 다음 적용은 v3 Blender 프레임의 동작 폭 확대다.
- `WalkTrialV3` 64장이 별도 경로로 반입됐고, 현재 `RatVisual`의 8방향 걷기 배열은 v3 `f01→f08`을 참조한다. v2·TrialV1·Blender v2는 보존됐다.
- `RatVisual`의 `RatDirectionalSpriteView`는 아래 배열을 f01→f08 순서로 정확히 8장씩 참조하며 `walkFramesPerSecond=8`이다.
   - `southWalkFrames`: `00-s`
   - `southWestWalkFrames`: `01-sw`
   - `westWalkFrames`: `02-w`
   - `northWestWalkFrames`: `03-nw`
   - `northWalkFrames`: `04-n`
   - `northEastWalkFrames`: `05-ne`
   - `eastWalkFrames`: `06-e`
   - `southEastWalkFrames`: `07-se`

`walkFramesPerSecond`는 시험 기준 8이며 최종 제품 사양이 아니다. `RatHostController`, CharacterController, 기존 정지 Sprite 슬롯, 충돌체와 루트 Transform은 보존됐다.

## 게임플레이 구현 산출물

- `UnityProject/Assets/_Project/Scripts/Host/RatDirectionalSpriteView.cs`
  - 완전한 8방향×8프레임 설정일 때만 이동 중 순환 표시한다.
  - 전역 시간 기반 프레임 계산으로 방향 전환 시 위상을 맞춘다.
  - 정지 또는 불완전 설정 시 기존 정지 Sprite와 마지막 방향을 유지한다.
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
  - 8fps, 공통 위상, 정지 복귀 단위 테스트를 추가했다.

## 2026-07-21 — 방향·선명도 피드백 코드 조사

- 수정 전 실제 `CurrentResolvedMoveDirection=East`, 루트 forward도 동쪽인데 `rat-walk-v3-...-06-e`가 화면 좌향으로 표시되는 것을 재현했다. Blender 출력의 수평 화면축이 Unity `camera.right`와 반대라는 프레임 맵·렌더 방향 대조 결과에 맞춰, `RefreshDirection`에서 시각 전용 `-cameraTransform.right`를 사용하도록 수정했다.
- Unity 컴파일/실행 검증에서 카메라 오른쪽 이동 벡터가 `RatSpriteDirection.West`와 `rat-walk-v3-f01-02-w`를 선택하는 것을 확인했다. `RatHostController.CurrentResolvedMoveDirection`은 기존 값 그대로여서 게임플레이 이동·충돌은 변경되지 않았다.
- PNG 64장은 모두 64×64이며 Point·mipmap 없음·무압축 Importer가 유지된다. 코드에는 프레임마다 필터·해상도·스케일을 바꾸는 경로가 없다. 방향별 불투명 실루엣 bbox는 W/E 폭 약 48px, N 폭 약 19px·높이 약 31px로 차이가 있어, 화면상 선명도/점유율 변화는 렌더 자산·픽셀 그리드 또는 카메라 통합 측 추가 QA가 필요하다. 이번 코드 담당 범위에서는 Importer·카메라·원본 PNG를 변경하지 않았다.

## 2026-07-21 — 정지·이동 Sprite 세트 통일

- 원인: 기존 구현은 정지 시 TrialV1, 이동 시에만 WalkTrialV3를 선택해 정상 구성에서도 프리렌더 세트·실루엣·선명도가 전환됐다.
- `ApplyCurrentSprite`는 8방향 v3 배열이 모두 완전할 때 현재 방향의 `f01`을 정지 Sprite로 선택하도록 수정했다. 하나라도 불완전하면 기존 TrialV1 정지 Sprite fallback을 유지한다. 이동·충돌·입력·카메라·Importer·씬·ProjectSettings는 변경하지 않았다.
- EditMode 계약은 완전 배열 정지 시 `northFrames[0]`, `northWalkFrames=null`으로 불완전하게 만든 뒤에는 기존 `staticNorth` fallback을 선택하도록 갱신했다.
- 활성 씬이 dirty여서 Save·Reload·추가 Play/TestRunner는 실행하지 않았다. Unity 스크립트와 테스트 파일의 정적 검증은 통과했으며, 실제 WASD 체감은 깨끗한 재로드 뒤 QA가 확인한다.

## Unity 씬/통합 이전 상태 (2026-07-20, 해소됨)

- v2 PNG 반입 완료: `UnityProject/Assets/_Project/Sprites/Characters/Rat/WalkTrialV2/`에 64장.
- 반입 검증: 예상 파일명·64×64 규격·Blender 원본과 SHA-256 64/64 일치.
- 당시 연결 철회로 보류했던 Sprite Importer, 8방향 배열, 8fps, 씬 저장과 Play 화면 확인은 위 적용 결과로 해소됐다.
