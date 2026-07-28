# Stage2 Unity 씬·통합 계획

## 통합 원칙

- 기존 `RatHost2DPrototype.unity`를 유지하면서 Stage1의 내부 안내 셸을 실제 2D 미니게임 표현 계층으로 확장한다.
- 런타임 판정·이동·접촉·수집·성공·실패는 `Scripts/RatHost2D`의 공개 API만 사용하고 Editor 빌더에는 게임 규칙을 중복 구현하지 않는다.
- 외부 씬 변경 Reload 모달이 해제되기 전에는 `.unity` 파일을 외부에서 직접 저장하거나 Unity를 강제 종료·우회하지 않는다.
- 기존 3D 씬, `RatHost2DTechnicalSample`, `TechnicalSample2D` 코드·아트·입력, Packages, ProjectSettings는 보호한다.

## 목표 계층

```text
RatHost2DPrototype
├─ Core2D
│  ├─ RatHost2DSessionController
│  └─ RatHost2DMinigameCoordinator
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

## 2D 공간·표시 계약

- 아레나는 독립 XY 공간이며 4개의 비트리거 `BoxCollider2D` 벽으로 경계를 만든다.
- 바이러스 논리 루트에 `Rigidbody2D`, 충돌 Collider, 입력·이동 런타임 컴포넌트를 두고 `Visual`은 표시만 담당한다.
- 백혈구 논리 루트에는 추적 런타임 컴포넌트와 물리 접촉 Collider를 연결한다.
- 조각 3개는 각각 고유 런타임 수집 컴포넌트와 트리거 Collider를 가진다.
- 신규 최종 아트는 만들지 않는다. 기존 기술 샘플 스프라이트를 읽기 전용으로 재사용하고 색·크기만 달리한 명시적 플레이스홀더로 바이러스·백혈구·조각·벽을 구분한다.
- 내부 HUD는 안정도, `조각 0/3`, 목표, `면역 포착 +8` 피드백을 표시한다.
- 실패 패널은 `면역 반응 돌파 실패`와 확인 입력 전 Host 잠금 안내를 표시한다.
- 성공 시 실제 변이 선택 UI 대신 `MutationSelection` 인계 셸과 Stage3 경계 안내를 표시한다.

## 카메라 계약

- `HostCamera2D`와 `InternalCamera2D`는 별도 루트·별도 target을 사용한다.
- Host 카메라는 쥐 논리 루트를, 내부 카메라는 바이러스 논리 루트를 추적한다.
- 모드 전환 런타임이 두 카메라의 활성 상태를 상호 배타적으로 관리해야 한다.
- `MutationSelection`과 실패 대기 중에는 Host·Virus 이동 및 내부 충돌 판정이 모두 정지한다.

## 필요한 런타임 공개 API

게임플레이 구현 에이전트가 확정한 이름을 우선하며, 씬 빌더에는 다음 기능이 필요하다.

1. 미니게임 총괄 컴포넌트
   - Session, 내부 루트, Host/내부 카메라 루트, 실패 패널, MutationSelection 셸 연결
   - 바이러스, 백혈구, 조각 3개, 내부 Collider 목록 연결
   - 내부 진입·실패 대기·실패 확인 복귀·성공 인계 시 루트 활성과 충돌 잠금 관리
2. 바이러스 이동 컴포넌트
   - Session/총괄, 기존 `TechnicalSample2DInput`, 이동 속도 구성
   - `FollowTarget`, 논리 위치, 활성 여부 공개
3. 백혈구 추적·접촉 컴포넌트
   - 총괄, 바이러스 target, 이동 속도, 접촉 쿨다운 구성
4. 변이 조각 컴포넌트
   - 총괄과 고유 조각 index 구성
5. 내부 HUD 컴포넌트
   - 안정도 Text/Slider, 조각 Text, 목표 Text, 포착 피드백 Text 연결
6. 공개 상태/이벤트
   - 안정도 현재/최대, 조각 현재/목표, 포착 피드백, 실패 대기, 성공 인계, 현재 내부 활성 상태

## 빌드·보호 계약

- 기존 `C:/tmp/LastHostRatHost2DStage1/<시각>/` 출력과 명시적 단일 씬 `BuildPlayerOptions.scenes` 방식을 유지한다.
- `EditorBuildSettings`를 변경하지 않는다.
- 렌더·ProjectSettings 보호 파일의 빌드 전 바이트 snapshot과 `finally` 복구를 유지한다.
- 모달 해제 후 Unity에서 빌더를 실행해 씬을 생성·저장하며, `scene.isDirty=false`를 확인한다.

## 검증 인계

- 현재는 빌더 코드의 정적 구조와 런타임 API 연결만 준비한다.
- 모달 해제 뒤 QA가 Rebuild, 컴파일, 전체 EditMode, MCP Play, Console, Windows 임시 빌드와 보호 diff를 실행한다.
- MCP Play 핵심은 Host/Virus 입력 상호 배타, 벽 충돌, 접촉 1회, 조각 3개 성공 우선, 실패 확인 복귀 60%, 재진입 초기화다.
