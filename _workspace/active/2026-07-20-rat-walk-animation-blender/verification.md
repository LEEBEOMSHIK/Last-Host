# 검증 기록

## 검증 대상

Blender에서 쥐 걷기 애니메이션 시험 제작을 시작할 수 있는 연결·원본 상태.

## 실행한 검증

- Windows 앱 조작 연결 `list_apps` 초기 호출, 2초 후 재시도, 세션 초기화 후 재호출.

## 결과

- 세 호출 모두 native pipe `os error 2`로 실패했다.
- Blender 창·파일·저장 상태, 애니메이션 루프, 렌더 출력은 확인하지 못했다.

## 완료 판단

차단. 연결 복구 또는 사용자가 안전한 Blender 파일 경로를 제공하기 전에는 열린 원본을 변경하지 않는다.

## 독립 QA 차단 기록 대조 — 2026-07-20

### 대조 결과

- 작업 패킷의 `task.md`, `work-log.md`, `handoff.md`, `verification.md`, `agent-activity.md`, `visual-review.md`는 Blender 시험 자산·Unity 통합·기존 정지 자산 비변경과 3회 연결 실패를 일관되게 기록한다.
- Computer Use 표준 연결 시도는 bootstrap→`list_apps`, 2초 후 재시도, 세션 초기화 후 재-bootstrap→`list_apps`이며 세 시도 모두 native pipe `os error 2`로 실패했다.
- 이 작업의 `artifacts/` 출력 폴더는 생성되지 않았다. 기존 정지 PNG 8장은 `direction-map.csv`의 SHA-256과 8/8 일치했고, 기준 OBJ도 기존 완료 패킷에 존재한다.
- `UnityProject/`의 현재 미커밋 변경은 이전 쥐 방향 표시·접지 작업 범위의 변경으로 대조됐으며, 이번 Blender 차단 작업에서 새 Unity·Blender·기존 정지 자산 변경은 없다.
- `_workspace/active/CURRENT.md`와 `current-task-board.md`는 이 작업을 차단 상태의 현재 작업으로 가리키고, 재개 조건도 앱 조작 연결 복구 또는 사용자가 저장한 Blender 원본 경로 제공으로 일치한다.

### QA 판정

- 판정: **차단 유지**
- 미실행: Blender 창·원본·저장 상태 확인, 걷기 루프 제작, 렌더 출력, Unity 통합·테스트.
- 재개 조건: Computer Use `list_apps` 정상 응답 또는 사용자가 안전한 저장 Blender 원본 경로를 제공해야 한다.

## 사용자 요청 재시도 — 2026-07-20

- 절차: Computer Use bootstrap→`list_apps`, 2초 후 `list_apps` 재시도, 세션 초기화 후 재-bootstrap→`list_apps`.
- 결과: 세 시도 모두 native pipe `os error 2`로 실패했다.
- 무변경 확인: Blender·Unity·걷기 출력·기존 정지 자산은 이번 재시도에서 열거나 수정하지 않았다.
- QA 판정: **차단 유지**. helper `list_apps` 정상 응답 또는 사용자가 저장 Blender 원본 경로를 제공하기 전에는 걷기 제작을 시작하지 않는다.

## 산출물 재개 검증 — 2026-07-20

- Blender 5.1.2 백그라운드에서 완료 패킷의 읽기 전용 OBJ를 입력으로 별도 `.blend`와 64개 PNG를 생성했다.
- PNG: 8방향 × 8프레임 = 64장, 전부 64×64 RGBA, 투명 배경, PNG filter 0, alpha 0/255, 캔버스 경계 clipping 없음.
- 루프: 각 방향의 f01~f08 바닥선 bottom pixel이 동일하며, f01 좌전/우후 접지, f03 통과, f05 우전/좌후 접지, f07 통과가 CSV 64행에 일치한다.
- Blender: timeline 1~8, `RatWalkRoot` 위치 `[0,0,0]`·scale `[1,1,1]`, root animation/keyframe 0, 8개 걷기 shape key, 전 프레임 mesh min Z=0.0과 몸통/root 수직 이동 0.0을 확인했다.
- 보존: 기존 정지 시험 자산의 검증 스크립트가 통과했고 원본 OBJ SHA-256은 `8D486D8727964A34936B49051E6E689C98CC5EA63473DAC6C2FDC0DE75CF951F`로 확인됐다. UnityProject는 기존 사용자 미커밋 변경만 존재하며 이번 작업은 해당 경로에 쓰지 않았다.
- QA 판정: **조건부 통과**. 걷기 내부 프레임의 바닥선은 방향별로 고정됐지만, W(02)는 기존 정지 PNG 하단보다 1px 위다. 기존 정지 포즈와의 픽셀 수직 기준을 절대 조건으로 적용하면 ground lift/피벗 재조정 뒤 재검증해야 한다. Unity 반입·런타임 재생·사용자 체감 검증은 이번 범위 밖이다.
