# 1단계 2D 씬·통합 구현 기록

## 구현 결과

- 별도 씬 `Assets/_Project/Scenes/RatHost2DPrototype.unity`를 생성했다.
- 결정적 씬 빌더와 전용 Editor 어셈블리를 `Assets/_Project/Editor/RatHost2D/`에 추가했다.
- 기존 `TechnicalSample2D` 타일·방향별 쥐 프레임·입력 에셋은 읽기 전용 참조로 재사용했다.
- 기존 `RatHostPrototype.unity`, `RatHost2DTechnicalSample.unity`, `TechnicalSample2D` 코드·테스트·아트는 수정하지 않았다.

## 씬 구조와 연결

```text
RatHost2DPrototype
├─ Core2D
├─ HostMode2D
│  ├─ World2D
│  │  ├─ Grid
│  │  │  ├─ FloorTilemap
│  │  │  ├─ WaterTilemap
│  │  │  └─ BlockingTilemap
│  │  ├─ YSortProps
│  │  │  ├─ Pipe_A
│  │  │  └─ Barrel_A
│  │  └─ ContaminationZone2D
│  └─ RatHost2D
│     ├─ Visual
│     └─ FootPoint
├─ HostCamera2D
│  └─ Main Camera
├─ UI2D
│  └─ HostHud2D
└─ InternalVirusShell2D
```

- `Core2D`의 `RatHost2DSessionController`가 런타임 상태 인스턴스를 하나만 소유한다.
- `RatHost2DMovementController`가 기존 `Host/Move` 입력과 본능 이동을 하나의 2D 충돌 모터로 합성한다.
- 기존 `RatHost2DController`는 충돌 이동 함수만 호출하고 자체 `Update/FixedUpdate`는 비활성화해 이중 이동을 막는다.
- `Visual`만 픽셀 스냅과 방향별 프레임 표시를 담당하고, 논리 루트와 카메라는 `RatHost2D`를 직접 따른다.
- 카메라는 `HostMode2D` 밖에 있어 내부 셸 전환 때 Host 루트가 비활성화돼도 유지된다.
- 파이프와 통에는 기존 기술 샘플 수용값의 비트리거 `BoxCollider2D`와 Y 정렬을 적용했다.
- 초록 오염 구역은 트리거이며 `ContaminationExposure`, 경계도 `+12/초`, 생명력 `-4/초`를 연결했다.
- Host HUD에는 생명력·면역 경계도·현재 모드·원인 피드백과 시험 에셋 안내를 표시한다.
- 내부 셸은 `WhiteBloodCellEvasion` 인계 확인용이며 실제 미니게임·복귀·변이 구현이 아님을 명시한다.

## 재생성과 빌드

- 씬 재생성: `Last Host/Rat Host 2D/Stage 1/Rebuild Scene`
- 임시 Windows 빌드: `Last Host/Rat Host 2D/Stage 1/Build Windows Temporary`
- 빌드는 명시적인 단일 씬 배열을 사용하며 `EditorBuildSettings`를 수정하지 않는다.
- 출력은 저장소 밖 `C:/tmp/LastHostRatHost2DStage1/<시각>/LastHostRatHost2DStage1.exe`에만 생성한다.
- 씬 빌더는 씬 import 뒤 필요 시 한 번 더 저장하고, 그래도 `scene.isDirty`이면 성공 로그 대신 예외로 중단한다.
- Windows 빌드 전에는 렌더 설정과 ProjectSettings 보호 파일 5개의 현재 바이트를 보관하고 `finally`에서 원상복구한다. 따라서 사용자 로컬 변경을 빌드 이전 상태 그대로 유지하는 계약이다.

## 구현자 자체 확인

- Unity 컴파일: 신규 Runtime, Editor, Tests 어셈블리 DLL 생성 확인
- Unity Console: 빌드 전 Error/Warning 0
- 메인 에이전트 MCP 재생성: 메뉴 실행 성공, 계층과 필수 루트 생성 확인
- 정적 연결 검사:
  - 활성 씬 경로 일치
  - Tilemap 3개
  - 충돌 소품 2개
  - 직교 Main Camera와 쥐 논리 루트 추적
  - Host 초기 활성, 내부 셸 초기 비활성
  - 기술 샘플 모터 자체 틱 비활성
  - HUD Slider 2개
  - BuildSettings 미등록
- Windows 임시 빌드 성공:
  - `C:/tmp/LastHostRatHost2DStage1/20260728-024656/LastHostRatHost2DStage1.exe`
  - `204,796,383 bytes`
  - 빌드 중 Unity가 자동 변경한 범위 밖 렌더·연결 설정 4개는 빌드 직전 상태로 복구했다.
- 초기 빌드 뒤 자동 변경 4개를 복구하고 보호 경로의 tracked diff가 없음을 확인했다.
- 메인 에이전트가 재생성 직후 `isDirty=true`를 한 차례 확인해, 이후 빌더에 import 후 최종 저장과 dirty 실패 검증을 추가했다.

## 남은 검증

- 이 기록은 구현자 자체 확인이며 독립 QA 판정이 아니다.
- 보완된 빌더 실행 뒤 MCP `GetActive.isDirty=false` 최종 대조는 동시 호출 충돌 방지를 위해 메인/QA 에이전트에 남긴다.
- 보호 파일 스냅샷 보완 뒤 Windows 빌드 재실행과 최종 Git diff 대조도 QA 담당이다.
- 전체 EditMode와 신규 테스트, MCP Play에서 본능/WASD·충돌·카메라·오염·자연 100% 단일 전환을 독립 검증해야 한다.
- 빌드 셰이더 경고는 Unity AI Inference/Sentis 패키지의 기존 셰이더 경고이며 빌드 오류는 없었다.
- 사용자 수동 플레이 수용 전에는 작업 완료·보관으로 판정하지 않는다.
