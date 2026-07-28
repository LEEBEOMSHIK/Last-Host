# Stage2 Unity 씬·통합 구현 기록

## 작업명

2단계 2D 백혈구 회피 미니게임과 성공·실패 인계

## 변경한 빌더

- `UnityProject/Assets/_Project/Editor/RatHost2D/RatHost2DPrototypeSceneBuilder.cs`
- 메뉴를 Stage2 재생성·임시 Windows 빌드로 갱신했다.
- 실제 `RatHost2DPrototype.unity`는 외부 씬 변경 Reload 모달 때문에 이번 구현 단계에서 재생성·저장하지 않았다.

## 목표 씬 계층

```text
RatHost2DPrototype
├─ Core2D
├─ HostMode2D
├─ HostCamera2D
│  └─ Main Camera
├─ UI2D
│  └─ HostHud2D
├─ InternalVirusMode2D
│  ├─ Arena2D
│  │  ├─ ArenaBackdrop
│  │  ├─ ArenaWalls2D
│  │  │  ├─ Wall_North
│  │  │  ├─ Wall_South
│  │  │  ├─ Wall_West
│  │  │  └─ Wall_East
│  │  ├─ Virus2D
│  │  │  └─ Visual
│  │  ├─ WhiteBloodCell2D
│  │  │  └─ Visual
│  │  └─ MutationFragments2D
│  │     ├─ Fragment_01
│  │     ├─ Fragment_02
│  │     └─ Fragment_03
│  ├─ InternalCamera2D
│  │  └─ Internal Camera
│  ├─ InternalHud2D
│  └─ FailurePanel2D
└─ MutationSelectionShell2D
```

## 연결한 런타임 API

- `RatHost2DSessionController.ConfigureStage2(...)`
  - Host 카메라 루트
  - 내부 카메라 루트
  - 실패 패널
  - `MutationSelection` 인계 셸
  - 바이러스 이동 컴포넌트
  - 백혈구 배열
  - 고유 index를 가진 조각 3개
  - 벽·바이러스·백혈구·조각 Collider 배열
- `RatHost2DVirusMovementController.Configure(session, input, speed)`
- `RatHost2DWhiteBloodCellChaser.Configure(session, virus, 1.8, 0.5)`
- `RatHost2DMutationFragment.Configure(session, uniqueIndex)`
- `RatHost2DStage2Hud.Configure(...)`와 `ConfigureStabilitySlider(...)`

Fragment는 빌더에서 결과 판정을 중복 구현하지 않고 고유 index만 제공한다. 수집·접촉의 같은 프레임 성공 우선, 중복 접촉 쿨다운, 실패 복귀 60%와 재진입 초기화는 런타임 세션의 공개 계약을 사용한다.

## 공간·충돌·카메라

- 아레나에는 비트리거 `BoxCollider2D` 벽 4개를 둔다.
- 바이러스에는 Dynamic `Rigidbody2D`, 비트리거 `CircleCollider2D`, 기존 `Host/Move` 입력과 단일 충돌 모터를 연결한다.
- 백혈구에는 Dynamic `Rigidbody2D`, 트리거 `CircleCollider2D`, 바이러스 추적과 `0.5초` 접촉 쿨다운을 연결한다.
- 조각 3개에는 각각 트리거 `CircleCollider2D`와 고유 index를 연결한다.
- Host 카메라는 쥐 논리 루트, 내부 카메라는 바이러스 논리 루트를 각각 추적한다.
- Session이 두 카메라와 Host/Virus 입력·Collider를 모드별로 상호 배타 관리한다.

## HUD와 결과 셸

- 내부 HUD: 바이러스 안정도 Text/Slider, `조각 0/3`, 현재 목표, 면역 포착 피드백
- 실패 패널: `면역 반응 돌파 실패`, `SPACE` 확인, 무보상 쥐 숙주·면역 경계도 60% 복귀 안내
- 성공 셸: `MutationSelection 인계 성공`, 실제 변이 선택·효과·성공 후 복귀가 Stage3임을 안내

## 플레이스홀더 경계

- 신규 이미지나 최종 아트를 만들지 않았다.
- 기존 `TechnicalSample2D` 바닥·벽·물 스프라이트를 읽기 전용으로 재사용하고 색·크기로 바이러스, 백혈구, 조각과 아레나를 구분했다.
- 모든 내부 표시에 `TECHNICAL PLACEHOLDER · NOT FINAL ART/SPEC` 경계를 둔다.
- 최종 PPU, 타일, 캐릭터 프레임, 내부 아레나 아트 규격은 확정하지 않았다.

## 빌드·보호 계약

- 임시 출력: `C:/tmp/LastHostRatHost2DStage2/<시각>/LastHostRatHost2DStage2.exe`
- `BuildPlayerOptions.scenes`에 `RatHost2DPrototype.unity` 한 개를 명시한다.
- `EditorBuildSettings`, Packages, ProjectSettings를 수정하지 않는다.
- 기존 렌더·ProjectSettings 보호 파일 5개의 빌드 전 바이트 snapshot과 `finally` 복구를 유지한다.
- 씬 import 뒤 최종 저장하고 `scene.isDirty`가 남으면 예외로 중단하는 Stage1 보호 계약을 유지한다.

## 구현자 정적 대조

- 구 `BuildInternalShell`, Stage1 메뉴·출력 경로 참조가 남지 않음을 확인했다.
- 신규 빌더 헬퍼와 result struct 정의가 모두 존재함을 확인했다.
- 빌더의 중괄호 수가 일치함을 확인했다.
- `ConfigureStage2`, Virus/WBC/Fragment/HUD 공개 타입과 서명이 현재 런타임 소스에 존재함을 확인했다.
- Stage2 신규 런타임 `.meta` 파일이 존재함을 확인했다.
- 기존 3D 씬, `TechnicalSample2D`, 입력 자산, Packages, ProjectSettings는 수정하지 않았다.

## 미실행·QA 인계

- Unity 외부 씬 변경 Reload 모달을 강제 종료·Reload 우회하지 않았다.
- 따라서 이번 구현자 단계에서는 실제 씬 Rebuild/Save, Unity 컴파일, 테스트, MCP Play, Console, Windows 빌드를 실행하지 않았다.
- 별도 Unity 임시 복제본도 만들지 않았다.
- 모달 해제 뒤 QA가 단일 임시 복제본으로 컴파일·전체 테스트를 먼저 수행하고, 안전한 원본 Unity에서 Rebuild·Play·Console·Windows 빌드·보호 diff를 독립 검증해야 한다.
- 실제 생성 씬에서 벽 충돌, 카메라 활성 상호 배타, 실패 패널의 Space 확인, 세 번째 조각 성공 인계와 재진입 초기화를 확인하기 전에는 완료로 주장하지 않는다.
