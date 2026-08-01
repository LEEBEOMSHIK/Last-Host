# Production2D 가림 런타임 코드 리뷰

## 역할과 범위

- 리뷰·최소 수정 주체: 게임플레이 구현 에이전트
- 대상: `RatSide3FrameView.cs`, `VisualOcclusionResolver2D.cs`, 관련 EditMode 테스트
- 제외 준수: builder·씬·ProjectSettings·Stage2·Stage3는 수정하지 않음
- 판단 기준: 2D 아이소메트릭 도트의 발 접지 정렬, 머리·몸통·발 solid 교차 0 또는 완전 가림, 독립 꼬리 조각 0, 4px fragment, 2px hysteresis, 좌우 collider offset, 정지 jitter·프레임당 할당 0

## 발견한 결함

1. 기존 `WouldSplitIntoTwoVisibleFragments`는 좌우 조각이 모두 4px 이상일 때만 숨겼다. 오클루더가 본체와 한쪽 전체를 덮고 반대쪽에 독립 꼬리 끝만 남기는 경우는 `false`가 되어 계약을 위반했다.
2. 가림 해제 hysteresis가 현재 가림을 만든 오클루더가 아니라 모든 오클루더에 적용됐다. 여러 물체의 가림 영역이 인접할 때 새 물체가 4px 진입 조건을 충족하지 않아도 이전 물체의 2px 해제 조건으로 숨김이 이어질 수 있었다.
3. resolver는 `Configure`, 해제, `OnDisable`에서 renderer를 무조건 활성화했다. 다른 시스템이 먼저 비활성화한 renderer의 상태를 침범할 수 있었다.
4. frame sprite와 `flipX`는 매 `LateUpdate`에 다시 읽고 world bounds를 계산하므로 프레임·좌우 변경 반영은 안전하다. `Rect`, `Vector3`는 값 형식이고 배열·목록 생성이나 LINQ가 없어 해당 경로의 관리 힙 할당은 추가되지 않는다.

## 최소 수정

- core와 오클루더가 교차하고, 한쪽에 4px 이상 조각이 남되 그 조각이 core 바깥에만 존재하는 `tail-only` 상태도 전체 가림으로 판정했다. 좌우 flip core 계약을 각각 검사한다.
- `_activeOccluderIndex`를 기록해 2px 해제 hysteresis를 기존 활성 오클루더에만 적용한다. 다른 오클루더는 항상 4px 진입 조건부터 평가한다.
- resolver가 직접 숨기기 직전의 renderer 활성 상태를 저장하고, 해제·컴포넌트 비활성 시 그 상태만 복원한다. 외부 비활성 상태를 강제로 켜지 않는다.
- `RatSide3FrameView`의 `-0.30/+0.30` collider offset 갱신은 방향 입력 후 같은 `ApplyView`에서 수행되고 논리 루트를 이동시키지 않아 변경하지 않았다.

## 검증

- Unity `ValidateScript` standard:
  - `VisualOcclusionResolver2D.cs`: 진단 0
  - `Production2DV1AssetAndSceneTests.cs`: 진단 0
  - `RatSide3FrameView.cs`: 오류 0, 도구의 기존 `Update 문자열 연결` 성능 경고 1개. 실제 코드에는 문자열 연결이 없어 오탐으로 분류
- `git diff --check`: 통과
- Unity `RunCommand` 정적 계약 smoke:
  - 최초 실행은 tail-only 경계식이 core 경계를 `0.03 world` 포함한 표본을 놓쳐 실패했다.
  - 남은 core 폭이 활성 fragment 임계보다 작은지를 판정하도록 보정 후 좌/우 flip 모두 `True/True`, 컴파일·실행 PASS.
  - 외부에서 먼저 disabled된 renderer를 configure·resolve 후에도 disabled로 보존: 컴파일·실행 PASS.
- smoke 후 Console을 비웠고 Error/Warning 0, Editor Play/Compile/Update 모두 종료 상태를 확인했다.
- 추가 테스트 계약:
  - 오른쪽 방향의 왼쪽 tail-only fragment 숨김
  - flip 방향의 오른쪽 tail-only fragment 숨김
  - 외부 disabled renderer 보존
- EditMode TestRunner와 실제 Play 매트릭스는 독립 QA가 재실행해야 하며 이 리뷰에서는 통과로 주장하지 않는다.

## 사용자 UX 확인 위험

현재 구조는 관통 조각을 없애기 위해 쥐 SpriteRenderer 전체를 비활성화한다. 통·상자 alpha 높이는 약 `0.836 world`, 쥐 높이는 최대 약 `0.586 world`라 뒤쪽 완전 가림 자체는 물리·작업 계약상 타당하고 완료 blocker는 아니다. 다만 폭이 작은 소품 뒤에 정지하면 위치 추적성이 잠시 끊기는 사용자 UX 위험은 남는다.

향후 사용자 수용에서 문제가 되면 오클루더별 전면 마스크/전후 레이어 또는 가림 중 위치를 보여주는 승인된 silhouette/indicator 표현을 별도 구조로 검토해야 한다. 이는 builder·씬 또는 아트 계약 변경이므로 이번 런타임 최소 수정 범위를 넘는다. 본 결과는 **코드 결함 보정 완료, 독립 QA 인계**이며 최종 완료는 QA와 사용자 실제 WASD 확인 전 주장하지 않는다.

## 독립 QA 1차 실패 후 직렬화 복원 보정

- QA 결과: 전체 EditMode `200 pass / 1 fail`. `ProductionV1_WholeCharacterOcclusionUsesFourPixelEntryAndTwoPixelRelease`에서 `+0.38 world` 해제 판정은 `false`로 맞았지만 renderer가 계속 disabled였다.
- 원인: builder가 편집 시 초기 가림을 평가해 `SpriteRenderer.enabled=false`를 씬에 저장했다. 반면 `_rendererHiddenByResolver`와 `_rendererEnabledBeforeHide`는 런타임 비직렬화 필드라 씬 재로드 때 사라졌다. 첫 숨김 판정은 이미 false인 renderer를 이전 상태로 기록했고 해제 시 false를 복원했다.
- 수정: `_visibilityStateInitialized` 런타임 세션 표식을 추가했다.
  - 씬에서 역직렬화된 resolver의 첫 `ResolveNow`는 전용 character renderer에 남은 stale disabled를 먼저 true로 정상화한 뒤 현재 위치를 다시 판정한다.
  - 명시적 `Configure`는 세션 초기화 완료로 표시하므로 외부에서 먼저 disabled한 renderer를 그대로 보존한다.
  - 이후 외부 비활성은 기존 소유 상태 저장·복원 계약을 유지한다.
- 테스트 보강: 실제 저장 씬이 builder-time disabled 상태를 포함함을 명시하고 기존 `hide → 0.37 유지 → 0.38 visible 복원` 회귀 테스트가 직렬화 경계를 직접 검증하도록 했다. 외부 disabled 보존 테스트도 유지한다.
- 컴파일·Play 상태 smoke: `entered=True`, `held=True`, `released=True`, `external=True`. 실행 당시 QA가 Unity를 이미 Play+Paused로 유지하고 있어 저장 씬의 초기 false 상태는 Play 초기화 후 true로 바뀐 상태였다. Play 중 `EditorSceneManager.OpenScene` 재로드 시도는 도구 사용 오류를 남겼으며 기능 실패와 분리한다.
- 남은 검증: QA가 Play 종료 후 관련 EditMode와 전체 EditMode를 재실행하고 Console을 새 기준으로 확인해야 한다. 본 에이전트는 QA의 Play 상태를 임의 종료하지 않았다.

## 독립 QA 2차 실패 후 수평 히스테리시스 보정

- QA 실측: `WallStraight_Occlusion` release-entry `0.017 world`, 0.002 폭 10회 왕복 전환 0; `Crate_A` `0.016`, 전환 0; `Barrel_A` `0.001`, 전환 20.
- 당시 중간 증거는 정본 통합 과정에서 폐기했다. 최종 근거: `artifacts/qa-subpixel-jitter-final.csv`.
- 원인: 활성 오클루더의 release hysteresis가 bounds의 Y축만 확장했다. 통의 해제는 X축 core 교차가 먼저 사라지는 경계라 `2/128 world` 보호를 받지 못했다.
- 수정:
  - 활성 오클루더에만 적용되는 기존 `_activeOccluderIndex` 계약을 유지했다.
  - release 시 오클루더 bounds를 X/Y 양축으로 각각 `2/128 world` 확장한다.
  - fragment 임계도 기존처럼 `4px - 2px`로 낮춰, bounds X 확장이 좌우 fragment 판정 자체를 임의 변경하지 않고 core 교차 해제 대역만 2px 보호하게 했다.
  - entry는 확장하지 않으므로 4px 진입, 다른 오클루더, tail-only, renderer 소유·직렬화 복원 계약은 그대로다.
- 테스트 추가: `WholeCharacterOcclusionReleaseHysteresisProtectsHorizontalCoreBoundary`.
  - 확장 전 X core 교차가 사라진 subpixel 표본은 entry 판정 false.
  - 같은 표본은 2px release 판정 true.
  - 2px 수평 대역을 지난 표본은 release false.
- 정적 검증: 변경 파일 `git diff --check` PASS. QA Play와 충돌하지 않도록 Unity 컴파일·TestRunner·Console 조작은 수행하지 않았다.
- 상태: 이 1차 양축 오클루더 확장은 아래 3차 QA에서 wall fragment release 계약을 깨는 것으로 확인되어 character core 확장 방식으로 대체했다.

## 독립 QA 3차 실패 후 단일 release 계약 교정

- QA 결과: 전체 `201 pass / 1 fail`, 관련 `9/10`. 저장 씬 wall 테스트에서 중앙 hidden 후 X `+0.37`이 해제되어 기존 기대 `true`를 깨뜨렸다. 새 수평 core 단위 테스트 자체는 통과했다.
- 원인: 오클루더 bounds X 확장은 왼쪽·오른쪽 visible fragment 폭도 각각 2px 줄인다. 동시에 fragment 임계를 `4px → 2px`로 낮추면 기존 wall release 판정과 동일하지 않고 실질적으로 fragment 여유가 상쇄돼 `+0.37` hold가 사라진다.
- 최종 단일 방식:
  - visible bounds와 occluder bounds는 entry·release 모두 원본을 유지한다.
  - release 중인 활성 오클루더에 한해 `characterCoreBounds`만 X/Y 양축 `2/128 world` 확장한다.
  - fragment 임계는 기존대로 `4px - 2px = 2px`를 사용한다.
- 수학적 영향:
  - core/occluder 교차는 어느 축이 먼저 끊겨도 2px 추가 유지되어 통의 수평 경계를 보호한다.
  - visible/occluder X는 움직이지 않아 wall의 기존 2px fragment release band를 그대로 보존한다.
  - entry에는 확장도 임계 완화도 적용되지 않아 4px 계약이 바뀌지 않는다.
- 테스트 보강:
  - 통형 수평 core 경계: entry false, release true, 2px 이후 release false.
  - 3px fragment 표본: 4px entry false, 2px release true. 오클루더 X 확장으로 다시 깨지는 회귀를 정적으로 차단한다.
  - 실제 저장 씬 wall `+0.37 true / +0.38 false`, crate, multi/tail/disabled/serialization은 기존 테스트와 QA 재실행 대상으로 유지한다.
- `git diff --check` PASS. Unity 조작은 금지 지시에 따라 수행하지 않았다.
- 상태: 런타임·테스트 최소 교정 완료, 독립 QA 재검증 대기. 완료 주장은 하지 않는다.
