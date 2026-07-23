# 프로젝트 총괄 검토 — Game 뷰 카메라 출력 복구

검토일: 2026-07-21 KST
작업 ID: `2026-07-21-game-view-camera-output-fix`

## 검토 대상

`task.md`, `handoff.md`, `work-log.md`, `agent-activity.md`, `verification.md`, 현재 Unity `RatHostPrototype` 씬의 읽기 전용 상태, 작업 상태판과 Git 변경 상태.

## 판정

**수정 필요**

출력 구조와 QA 대체 근거는 목적에 부합한다. 그러나 현재 작업 트리에 `ProjectSettings.asset` 변경이 존재하는데 작업 기록은 ProjectSettings 순변경이 없다고 적고, 상태판도 구현 대기라고 남아 있어 완료·커밋 게이트의 범위/상태 정합성이 충족되지 않는다. 이 불일치를 정리하기 전에는 내부 승인 가능 또는 완료로 판정할 수 없다.

## 현재 Unity 상태 대조

- `GameViewFrameCamera`가 존재하며 `targetTexture=null`, `targetDisplay=0`(Display 1), `cullingMask=0`, depth `-100`이다. Display 1에 직접 렌더하는 카메라가 없던 원인은 구조적으로 해소됐다.
- `IsometricCamera`는 계속 `MainCamera`이고 직교 쿼터뷰이며 `RatPixelTrial960x540`을 targetTexture로 사용한다.
- `WorldPixelOutput` RawImage는 같은 RenderTexture를 참조하고 부모 Canvas order는 `-100`, HUD Overlay Canvas order `0`은 1개다. 따라서 960×540 Point 월드 출력과 HUD 전면 분리 계약은 보존된다.
- 현재 활성 씬은 `RatHostPrototype`, `dirty=True`다. 본 검토는 읽기 전용 상태 조회만 수행했으며 Play 검증을 재실행하지 않았다.

## 범위·금지 항목 확인

- 씬 변경은 Display 1 프레임 제공용 카메라 1개로 한정돼 있으며, 기록상 이동·충돌·면역·변이 스크립트는 변경하지 않았다. 3D 쿼터뷰·RT 월드 출력·v5b 시험안 경계도 유지된다.
- 다만 Git 작업 트리에서 `UnityProject/ProjectSettings/ProjectSettings.asset`에 Standalone define `APP_UI_EDITOR_ONLY` 추가 변경이 확인된다. 이 작업의 금지 범위·work-log·현황판과 충돌한다. 본 변경이 이전 작업의 잔여인지 이번 Unity 실행으로 생긴 것인지 기록상 식별되지 않으므로, 소유·필요성 확인 후 되돌리거나 명시적 승인/기록으로 분리해야 한다.
- 상태판의 해당 작업 상태가 `구현 대기`인데, 패킷에는 구현 완료와 독립 QA 대체 근거가 기록돼 있다. QA 대조 전 상태판을 실제 상태로 동기화해야 한다.

## QA/검증 기록 확인

- QA는 dirty 씬에서 Play→Stop, 활성 Display 1 프레임 카메라, IsometricCamera RT 유지, RawImage/HUD 계층, `1920×1080` Play, 쿼터뷰 추적, Console Error/Warning 0을 독립 기록했다.
- QA는 Camera Capture가 Game 뷰 UI·ScreenSpaceOverlay RawImage를 직접 캡처하지 못한다는 한계를 명시했다. 따라서 `No cameras rendering` 문구가 사라진 **직접 이미지 증적은 없으며**, 활성 Display 1 카메라·UI 연결이라는 대체 MCP 근거와 분리해 해석해야 한다.

## 수정 필요

1. `ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 변경을 작업 범위에서 제거하거나, 이미 존재한 외부 변경이면 소유/발생 경위를 작업 기록과 상태판에 명확히 분리한다.
2. 상태판을 구현/QA 실제 상태로 동기화한다.
3. 위 두 항목 정리 후 사용자에게 Game 뷰에서 `No cameras rendering`이 사라지고 월드·HUD가 표시되는지, WASD 이동 중 쿼터뷰 추적·픽셀 안정성이 유지되는지 확인받는다.

## 사용자 확인 항목

- Display 1 Game 뷰에서 검은 빈 프레임이나 `No cameras rendering` 문구 없이 픽셀 월드와 HUD가 함께 보이는지.
- WASD 직선·대각 이동 중 카메라가 쥐를 따라가며, v5b 픽셀 출력의 흐림·떨림·정지/이동 외형 변화가 없는지.

## 다음 단계

- 수정 필요 항목을 정리한 뒤 총괄 재검토한다.
- 사용자 확인 전에는 v5b 공통 기준 승격, 이 작업의 완료/커밋 보고를 하지 않는다.

---

## 재검토 — ProjectSettings 원인 분리 및 저장 상태 확인

### 판정

**사용자 결정 필요**

이전 `수정 필요` 사유였던 ProjectSettings 소유 불명과 상태판 불일치는 해소됐다. 구현 담당 기록에 따르면 `APP_UI_EDITOR_ONLY`은 `com.unity.dt.app-ui`의 AppUIManager가 editorOnly 설정에서 Play/도메인 갱신 중 자동 추가한 범위 밖 로컬 변경이다. 이 작업은 해당 파일을 저장·수정·되돌리거나 커밋 대상으로 삼지 않았고, 현황판·CURRENT도 그 상태와 사용자 Game 뷰 확인 대기로 동기화됐다.

### 재확인 근거

- 현재 Unity 읽기 전용 조회에서 활성 씬 `RatHostPrototype`은 `dirty=False`다.
- `GameViewFrameCamera`는 enabled, Display 1(`targetDisplay=0`), targetTexture 없음, cullingMask 0이다.
- `IsometricCamera`는 MainCamera·직교 쿼터뷰·`RatPixelTrial960x540` targetTexture를 유지하고, `WorldPixelOutput`도 같은 RT를 참조한다.
- 따라서 저장된 씬 변경은 Display 1 프레임 카메라 추가에 한정되며, RT 픽셀 월드 출력·HUD 분리·3D 쿼터뷰와 이동/충돌/면역/변이 경계는 보존된다.

### QA 및 사용자 확인 경계

- 독립 QA의 Play·추적·RT/HUD 계층·콘솔 0 대체 MCP 근거는 유효하다.
- MCP는 Game 뷰 UI 자체를 직접 캡처하지 못하므로, `No cameras rendering` 문구 소거와 실제 WASD 체감은 여전히 사용자 Game 뷰 확인이 필요하다. 이는 QA 통과를 부정하는 오류가 아니라, 최종 시각 수용 전제의 잔여 확인이다.

### 다음 단계

- 사용자가 Display 1 Game 뷰에서 월드·HUD 표시와 WASD 추적을 확인한다.
- 확인 전에는 작업 완료·커밋 또는 v5b 공통 기준 승격을 주장하지 않는다.

---

## 재검토 — 실제 OS 입력·idle 안정화 증적 (2026-07-23)

### 판정

**보류**

Game 뷰 출력 복구의 핵심 경로는 실제 OS `W` 입력과 정지 캡처로 더 강하게 뒷받침됐다. 그러나 D 및 W+D의 게임 내 시각 결과와 이번 OS 입력 세션의 Console 상세은 확인되지 않았고, 현재 Codex의 native MCP transport도 끊겨 있다. 따라서 작업을 완료·커밋 가능으로 올리거나, 사용자가 지적한 이동 중 쥐-카메라 상대 흔들림/지연이 해소됐다고 판정할 수 없다.

### QA 증적 확인

- 실제 OS `W` 2초 입력에서 Game HUD가 `강제 조종 +14 → +20`, 면역 경계도가 약 31%→35%→41%로 변했고, 쥐·월드·카메라 출력도 계속 변했다. `No cameras rendering` 없이 출력된 실제 입력 수신·직선 이동은 **통과**다.
- D와 W+D는 OS 키 이벤트 전송·해제까지만 확인됐다. 이동 중 화면 증적이 없으므로 방향·대각 이동의 시각 결과는 **보류**다.
- 키 해제 후 0.25초와 1.25초 Unity 창 캡처는 243,067 bytes 및 SHA-256 `B0C4E5DB145309781799774A239F8462B050D45A136014E8AF30E6205C77D33B`가 완전히 일치한다. 이 1초 구간의 표시 정지·idle 안정화는 **통과**다. 해제 직후 0.25초 이내의 감속/보간은 별도 보류다.
- 이번 OS 입력 세션의 Console 상세은 MCP transport closed 때문에 미검증이다. 이전 세션의 Console 0건을 이번 세션의 통과 근거로 재사용하지 않는다.

### 환경·저장소 경계

- 외부 Unity relay 중복 3쌍 종료 뒤 Unity PID `42724`, Unity 내부 relay PID `25004`, 현재 Codex node PID `58760`만 남았다. 현 native MCP 도구는 종료된 이전 relay를 참조하므로 연결 단절은 검증 인프라 상태이며, 게임 기능 실패 증거로 해석하지 않는다.
- Git 상태상 작업 관련 저장 대상은 `RatHostPrototype.unity`, 작업 패킷·상태 문서다. `ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY`은 범위 밖 자동 변경으로 계속 제외해야 하며, `_workspace/previews/`도 제외 범위다.
- 현재 `CURRENT.md`와 상태판은 2026-07-23의 OS 입력·idle 증적과 ProjectSettings 원인 분리 결과를 아직 반영하지 않아, 완료·커밋 전 실제 상태로 동기화해야 한다.

### 커밋 판정 및 다음 단계

- **커밋·푸시 불가:** QA가 부분 통과/전체 완료 보류이고, 총괄 판정도 보류이며 상태판 동기화가 남았다.
- native MCP transport 복구 후 QA가 이번 세션의 Console 상세과 D/W+D 이동 중 Game 뷰 결과를 확인하거나, 사용자가 Display 1에서 직선·대각 이동의 쥐-카메라 정합과 Console 상태를 직접 확인해야 한다.
- 이 확인으로도 v5b 공통 기준 승격은 자동으로 허용되지 않는다. v5b의 사용자 시각 수용은 별도 결정으로 유지한다.

### 사용자 명시 커밋·푸시 예외 — 2026-07-23

사용자가 명시적으로 `커밋 푸쉬해`를 지시했다. 이에 따라 현재 GameView 출력 복구의 저장된 씬 변경, 작업 패킷, 실제 검증 증적, 현황판 문서만 커밋·푸시하는 예외를 허용한다.

- 이 예외는 QA의 **부분 통과/전체 완료 보류**와 총괄 **보류** 판정을 해제하지 않는다.
- `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 자동 변경과 `_workspace/previews/`는 범위 밖이므로 스테이징·커밋·푸시에서 제외한다.
- 이후 D/W+D 이동 중 시각 결과, 이번 OS 입력 세션 Console, 사용자 체감 확인은 계속 열린 검증 항목으로 남는다.
