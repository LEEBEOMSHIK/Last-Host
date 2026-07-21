# 작업 로그

## 2026-07-20 — 작업 시작

- 사용자가 Blender v2 쥐 보행을 실제 게임 화면에서 검증하도록 Unity 시험 반입을 승인했다.
- 기존 v2 PNG·프레임 맵을 입력으로, Unity 코드 담당과 씬/통합 담당을 분리해 배정한다.
- 최종 아트·최종 재생 속도·기존 정지 자산 교체는 이번 작업에서 제외한다.

## 2026-07-20 — 게임플레이 표시 계약 구현

- `RatDirectionalSpriteView`에 8방향별 `Sprite[8]` 시험 프레임 계약과 `walkFramesPerSecond`(기본 8fps)를 추가했다.
- 컨트롤러의 기존 `CurrentResolvedMoveDirection`만 읽어 이동 여부를 판단한다. 방향 정량화, 이동, 충돌, CharacterController, 루트 Transform에는 변경을 가하지 않았다.
- 모든 방향 배열이 8개의 non-null Sprite를 가질 때만 걷기 표시를 켠다. 누락·불완전 설정 또는 정지 시에는 기존 방향별 TrialV1 정지 Sprite로 안전하게 되돌린다.
- 프레임 인덱스는 공통 `Time.time` 기준이므로, 이동 중 방향 전환해도 같은 보행 위상(f01~f08)을 유지한다.
- EditMode 테스트 `RatDirectionalSpriteView_WalkCyclesAtEightFpsAndReturnsToLastIdleDirection`을 추가해 8fps 순환, 방향 전환 시 공통 위상, 정지 시 마지막 방향 정지 Sprite 복귀를 검증하도록 했다.
- Unity MCP `Unity_GetUserGuidelines` 및 `Unity_ValidateScript`는 Editor가 `Connection revoked`를 반환해 실행 불가했다. 씬/통합 및 QA 담당이 MCP 승인 복구 뒤 Unity 컴파일·Play를 확인해야 한다.

## 2026-07-20 — Unity 시험 자산 반입 (씬/통합)

- Blender v2의 `renders-v2` PNG 64장을 `UnityProject/Assets/_Project/Sprites/Characters/Rat/WalkTrialV2/`로만 복사했다. 기존 `TrialV1`과 Blender 출력은 수정·삭제하지 않았다.
- 파일명은 `rat-walk-f01-00-s.png`부터 `rat-walk-f08-07-se.png`까지, 방향 순서 `S, SW, W, NW, N, NE, E, SE`, 프레임 순서 `f01 → f08`을 확인했다.
- 원본/반입본 SHA-256 대조 결과 64/64 일치, 모든 이미지 64×64 px를 확인했다.
- Unity MCP 상태 확인 결과는 `Connection revoked. Go to Unity Editor > Project Settings > AI > Unity MCP to change approval.`이다. 따라서 Unity가 생성할 `.meta`와 Importer 설정(스프라이트, PPU 32, Pivot 0.5/0.25, Point, 압축 없음), `RatVisual` 배열 연결, 씬 저장, 컴파일/Play는 수행하지 않았다.
- MCP 승인 복구 후 Unity 씬/통합 담당은 AssetDatabase 재임포트로 새 `.meta` 64개를 생성·설정한 다음, `RatHostPrototype`의 `RatVisual/RatDirectionalSpriteView`에 방향별 `f01→f08` 8개 배열과 `walkFramesPerSecond=8`을 연결해야 한다. 기존 정지 슬롯·root·콜라이더는 변경하지 않는다.

## 2026-07-20 — MCP 복구 후 통합·Play 재개

- Unity MCP `GetUserGuidelines`와 `ManageEditor(GetState)`가 정상 응답하는 것을 확인한 뒤 재개했다.
- 사용자 요청 `진행해`에 따라, 기존 작업 패킷의 통합 담당 범위만 Codex 메인 에이전트가 예외 수행했다. 별도 에이전트 배정은 현재 세션의 다중 에이전트 제한으로 불가했으며, 변경·검증 결과는 본 작업 패킷에 기록한다.
- `WalkTrialV2` 64장을 Unity Sprite(Single), PPU 32, Pivot `(0.5, 0.25)`, Point 필터, 무압축, mipmap 없음, Clamp로 재임포트했다. 64/64 Importer 설정과 Sprite 로드를 Unity Editor에서 재대조했다.
- `RatVisual/RatDirectionalSpriteView`의 8개 걷기 배열에 방향별 `f01→f08`을 연결하고 `walkFramesPerSecond=8`을 저장했다. 기존 TrialV1 정지 슬롯·RatHost 루트·CharacterController·게임플레이 판정은 변경하지 않았다.
- Play 진입/종료가 정상 동작했다. Play 모드에서 8방향 모두 frame 4(0-based 3)를 올바른 배열에서 표시하고, 정지 시 마지막 방향 TrialV1 정지 Sprite로 복귀하며, 접지 clearance가 `0.005`인 것을 확인했다.
- 실제 카메라 렌더로 동쪽 방향의 8프레임을 캡처해 다리/꼬리 실루엣 변화와 카메라 내 표시를 확인했다. Unity MCP 기본 Camera Capture는 Play 인스턴스 ID를 찾지 못해 실패했지만, 동일 MainCamera의 RenderTexture 렌더는 성공했다.
- 대상 EditMode 테스트는 동적 TestRunner 래퍼 제약과 별도 배치 Unity 프로세스의 즉시 종료(결과 XML·log 미생성)로 아직 실행 결과를 확보하지 못했다. 통과로 주장하지 않는다.

## 2026-07-20 — 보행 가독성 보정

- 사용자 시각 확인에서 실제 보행 변화가 너무 미세하다는 피드백을 받았다.
- 사용자 승인에 따라, 충돌·이동·카메라·원본 PNG를 바꾸지 않는 시각 자식 보정으로 `RatVisual`을 1.35배 확대한다.
- 확대에 맞춰 8방향 visible-foot-bottom offset도 같은 비율로 보정해 접지 위치를 유지한다. 수정 뒤 Play에서 화면 점유율·보행 가독성·접지·콘솔을 재검증한다.
- `RatVisual.localScale=(1.35, 1.35, 1.35)`와 방향별 접지 오프셋을 적용·저장했다.
- Play에서 8방향 frame index 3, 8fps, 접지 clearance `0.005`, 콘솔 Error/Warning 0건을 재확인했다. 실제 MainCamera 렌더에서 쥐 실루엣·다리 분리가 이전보다 커진 것을 확인했다.
- 원본 v2 프레임의 다리 동작 폭 자체는 변경하지 않았다. 실제 키보드 연속 이동에서 보행이 충분히 읽히는지는 사용자 체감 확인으로 남긴다.

## 2026-07-20 — 방향 정정: Blender v3 동작 폭 확대

- 사용자가 화면 크기 확대가 아니라 실제 다리·몸통 보행 동작을 크게 해야 한다고 방향을 정정했다.
- `RatVisual` 1.35배 확대와 비례 접지 오프셋은 v3 Unity 적용 전에 되돌린다.
- Blender 애니메이션 테크아트 담당에 v2를 보존한 별도 v3 원본·8방향×8프레임 64 PNG 제작을 배정했다. v3는 접지 발 고정, 스윙 발의 명확한 들림·전진·착지, 대각 접지쌍 교대를 64px에서 읽히도록 한다.

## 2026-07-20 — v3 Unity 시험 반입·Play 재검증

- Blender 담당의 `renders-v3` 64 PNG를 `WalkTrialV3` 별도 Unity 경로로 반입했다. Blender 원본·v2·TrialV1은 보존했다.
- 원본/반입본 SHA-256은 64/64 일치했다. Unity에서 Sprite(Single), PPU 32, Pivot `(0.5, 0.25)`, Point, 무압축, mipmap 없음으로 64/64 재검증했다.
- 사용자 승인에 따라 Codex 메인 에이전트가 예외적으로 `RatVisual`의 8방향 걷기 배열을 v3 `f01→f08`으로 교체하고 8fps를 유지했다. 기존 정지 슬롯·이동·충돌·카메라·루트 Transform은 변경하지 않았다.
- Play에서 v3의 8방향 frame index 3, 8fps, 접지 clearance `0.005`, MainCamera 렌더와 Unity Console Error/Warning 0건을 확인했다.

## 2026-07-21 — 사용자 피드백 대응: 고정 쿼터뷰 시작점

- 사용자 확인에서 쥐가 뒤로 움직여 보이고 화질이 프레임마다 흔들려 보인다는 피드백이 들어왔다. 원인 후보로, 기존 씬과 재생성기 모두 `PrototypeCameraController.startingHostMode=ThirdPerson`(원근·추적)을 기본값으로 둔 점을 확인했다.
- 쥐 스프라이트 시험의 판정 기준을 고정 쿼터뷰로 통일하기 위해, `RatHostPrototype.unity`의 직렬화 값(열거형 `QuarterView=1`)과 `RatHostPrototypeSceneBuilder.cs`의 재생성 기본값을 `QuarterView`로 함께 변경했다. 이동, 충돌, 스프라이트 배열·Importer, ProjectSettings는 변경하지 않았다.
- V 키는 HUD에 안내되지는 않지만 `PrototypeKeyboardInput`과 `PrototypeCameraController`에서 여전히 ThirdPerson → QuarterView → TopView 순환을 실행한다. 이번 씬 범위에서는 코드 변경이 금지되어 토글 비활성화는 하지 않았다. 사용자가 V를 누르면 고정 시점 판정이 다시 혼동될 위험이 남는다.
- Unity MCP의 활성 씬은 이미 dirty였고, 메모리상 카메라는 기존 ThirdPerson 상태였다. 다른 미저장 씬 변경을 함께 저장하거나 덮어쓰지 않기 위해 이 세션에서는 강제 Save·재로드·Play를 실행하지 않았다. `Unity_RunCommand`의 컴파일/읽기 전용 점검은 성공했으며, 다음 깨끗한 씬 재로드에서 직렬화된 QuarterView를 실제 Play로 재확인해야 한다.

## 2026-07-21 — dirty 활성 씬의 즉시 카메라 적용

- QA가 Play를 종료한 뒤, 사용자 요구에 따라 저장·재로드 없이 활성 `IsometricCamera`의 `PrototypeCameraController`와 `Camera` 인스턴스만 MCP `Unity_RunCommand`으로 수정했다.
- `startingHostMode`를 `QuarterView`로 설정하고 현재 모드를 순환 확인해 `QuarterView`로 맞춘 뒤 `ApplyCameraNow(RatHost)`를 호출했다. 실행 결과는 `starting=QuarterView`, `current=QuarterView`, `orthographic=True`다.
- 씬 Save, 재로드, 파일 수정, ProjectSettings 변경은 수행하지 않았다. 다른 dirty 씬 변경은 보존했으며, QA가 이 메모리 상태로 즉시 Play 재검증할 수 있다.

## 2026-07-21 — 사용자 피드백 대응: 프리렌더 수평 축 보정

- 사용자 피드백의 뒤로 걷는 인상을 코드 범위에서 재현했다. 실제 이동·루트가 동쪽(`CurrentResolvedMoveDirection`, root forward)인데 런타임 표시가 `rat-06-e` 프레임이어서 화면상 좌향으로 보였다.
- `RatDirectionalSpriteView.RefreshDirection`에만 Blender 프리렌더 수평축 보정(`-cameraTransform.right`)을 적용했다. `RatHostController`, 입력, CharacterController, 이동 벡터, 충돌, 루트 Transform, 카메라는 수정하지 않았다.
- Unity MCP 컴파일 검증은 경고 1건(기존 `Update()` 문자열 연결 성능 힌트)뿐이고 오류는 없었다. 오른쪽 이동 벡터를 주면 `West` 표시 프레임(`rat-walk-v3-f01-02-w`)이 선택되고, 호스트의 실제 이동 벡터는 보존됨을 로그로 확인했다. 콘솔 Error/Warning은 0건이었다.
- 프레임별 선명도 변동은 코드에서 필터나 텍스처 설정을 전환하는 경로가 없고, v3 PNG가 동일 64×64·Point·mipmap 없음·무압축임을 재확인했다. 방향별 렌더 실루엣의 pixel bbox 차이는 남아 있어, 카메라 픽셀 정렬 또는 Blender 출력 구도는 Unity 통합/테크아트 QA로 인계한다.
- 활성 씬이 dirty인 상태이므로, 다른 미저장 씬 변경을 보호하기 위해 Save·Reload·추가 Play는 실행하지 않았다. 다음 검증은 깨끗한 씬 재로드 뒤에 수행한다.

## 2026-07-21 — 사용자 확인 원인 반영: 정지·이동 v3 세트 통일

- 원인 확인: 정지 시 TrialV1 정지 Sprite, 이동 시 WalkTrialV3 Sprite를 교체하는 기존 경로가 형태·선명도 전환을 만들었다.
- `RatDirectionalSpriteView.ApplyCurrentSprite`를 최소 수정했다. 8방향 v3 배열이 모두 완전하면 현재 방향의 `f01`을 정지 Sprite로 사용하고, 불완전 배열이면 기존 `GetSprite(CurrentDirection)` TrialV1 fallback을 유지한다. 이동 프레임 순환·입력·이동·충돌·카메라·Importer·씬·ProjectSettings에는 변경이 없다.
- EditMode 계약은 완전 배열에서 정지 시 `f01` 유지, 배열을 의도적으로 불완전하게 만든 뒤 TrialV1 fallback 복귀를 확인하도록 갱신했다.
- Unity `ValidateScript`에서 런타임 코드·테스트 파일 모두 오류 0건(런타임 코드의 기존 Update 문자열 연결 성능 힌트 1건만)을 확인했다. 활성 씬이 dirty이므로 Save·Reload·추가 Play/TestRunner는 하지 않았고, 깨끗한 재로드 뒤 QA가 실제 WASD 체감을 확인해야 한다.
