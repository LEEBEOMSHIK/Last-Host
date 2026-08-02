# 원본 Stage2 씬 복구

## 담당

- Unity 씬/통합 구현 에이전트
- 작업일: 2026-07-29

## 복구 대상

- `Assets/_Project/Scenes/RatHost2DPrototype.unity`
- 증상:
  - `FloorTilemap`, `WaterTilemap`, `BlockingTilemap`의 실제 셀이 모두 `0`
  - Host 카메라에서 소품과 쥐만 보이고 맵 바닥·외곽 경계가 보이지 않음
  - 원본 씬은 `InternalVirusShell2D`가 남은 Stage1 구조

## 원인

Stage2 씬 빌더가 기존 `TileBase`와 `InputActionAsset`을
`EditorSceneManager.NewScene(..., NewSceneMode.Single)`보다 먼저 로드했다.
열려 있던 씬을 교체하는 과정에서 기존 asset의 native 참조가 무효화되어
이후 `Tilemap.SetTile`이 null tile을 기록했다.

저장 직전 검사를 추가해 다음 오류로 원인을 확인했다.

```text
2D host tilemap population is invalid before save:
floor=0, water=0, walls=0
```

## 최소 수정

- 새 씬을 만든 뒤 Tile·Input asset을 다시 로드하도록 순서를 변경했다.
- 현재 열린 씬 asset의 불필요한 강제 재임포트와 2차 저장을 제거했다.
- 타일을 배치한 세 Tilemap을 dirty 처리했다.
- 저장 전·후 다음 셀 수를 만족하지 않으면 Rebuild가 실패하도록 후조건을 추가했다.
  - Floor `117`
  - Water `5`
  - Blocking wall `40`

게임플레이 코드, Packages, ProjectSettings, 입력 asset, 최종 아트는 변경하지 않았다.
Windows 빌드는 만들지 않았다.

## Unity MCP 확인

Rebuild와 Save 후:

```text
SCENE dirty=false
FloorTilemap: 117, min=(-6,-4,0), max=(6,4,0)
WaterTilemap: 5, min=(3,-2,0), max=(3,2,0)
BlockingTilemap: 40, min=(-6,-4,0), max=(6,4,0)
Host Camera: orthographic, target=RatHost2D
Internal Camera: orthographic, target=Virus2D
Arena walls: 4/4 non-trigger
Virus=1, WhiteBloodCell=1, Fragment=3
FailurePanel=inactive, MutationSelectionShell=inactive
```

씬을 디스크에서 다시 Load한 뒤에도 같은 셀 수와 범위를 확인했다.
저장된 scene YAML의 세 `m_Tiles` 블록도 `5, 117, 40`개 셀을 직렬화했다.
Host 카메라 캡처에서는 `13×9` 바닥, 외곽 벽, 수로, 오염 구역,
쥐와 소품이 함께 표시되어 맵 범위가 읽혔다.

## 기본 Play 확인

MCP 직접 상태 전환 대체 검증:

```text
PLAY_INITIAL
mode=RatHost
host=true
internal=false
hostCamera=true
internalCamera=false

PLAY_INTERNAL
mode=InternalVirus
host=false
internal=true
hostCamera=false
internalCamera=true
virus=true
wbc=true
wallsEnabled=4/4
```

카메라 캡처 도구 사용 직후 `RenderTexture.active` 해제 경고 1건이 발생했다.
이는 캡처 도구 경고로 분리했다. 콘솔을 비우고 캡처 없이 동일한
Play 전환·Stop을 반복한 최종 Console은 Error/Warning `0`이었다.

## 남은 검증

- 실제 Game View WASD/Space 입력은 이번 구현자 확인에 포함하지 않았다.
- 성공·실패·재진입 전체 흐름은 독립 QA가 원본 Unity에서 재검증해야 한다.
- 현재 타일·캐릭터·내부 아레나는 기술 플레이스홀더이며 최종 아트가 아니다.
- 카메라 바깥 영역은 어두운 단색이지만, 맵 바닥과 외곽 경계는 현재 식별된다.
