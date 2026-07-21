# 작업 로그

## 2026-07-16 15:04 KST

- 사용자가 쥐 시험 `index.html` 확인을 완료했다.
- 기존 시험 에셋 작업을 완료 보관으로 전환했다.
- 정지 8방향 Unity 시험 반입 작업을 새로 열었다.
- 현재 Unity MCP 활성 씬이 `RatHostPrototype`임을 확인했다.
- 현재 캡슐은 씬 빌더가 생성하는 `RatVisual` 플레이스홀더이며 시험 스프라이트는 아직 Unity에 반입되지 않은 상태다.

## 2026-07-16 15:14 KST

- 완료 시험 에셋의 8개 PNG를 `UnityProject/Assets/_Project/Sprites/Characters/Rat/TrialV1/`에 원본 SHA-256을 유지한 채 복사했다.
- Unity Import를 `Sprite / Single / PPU 32 / Point / mipmap off / alpha transparency / Clamp / uncompressed / custom pivot (0.5, 0.25)`로 설정했다.
- 표시 전용 `RatDirectionQuantizer`, `RatDirectionalSpriteView`를 추가했다. `RatHostController.CurrentResolvedMoveDirection`만 읽으며 기존 컨트롤러는 수정하지 않았다.
- 카메라의 지면 투영 right/forward를 기준으로 `S, SW, W, NW, N, NE, E, SE`를 선택하고, 정지 시 마지막 방향을 유지하며, `LateUpdate`에서 카메라 월드 회전을 적용한다.
- 씬 빌더가 같은 8개 Sprite와 Import 설정을 재현하도록 수정했다.
- 현재 `RatHostPrototype` 씬의 캡슐 `RatVisual`을 `SpriteRenderer + RatDirectionalSpriteView`로 교체하고 저장했다. `RatHost`의 `CharacterController`와 게임플레이 컴포넌트는 그대로 유지했다.
- Unity 6 `TextureImporter` API 호환 문제로 첫 동적 반입 명령이 컴파일 1회 실패했다. 씬 변경 전 실패했으며 `TextureImporterSettings` 사용으로 수정한 뒤 재컴파일과 반입이 성공했다.
- Unity 스크립트 표준 검증 4개 파일은 진단 0건, 프로젝트 강제 Refresh 후 콘솔 Error/Warning 0건이었다.
- EditMode 전체 결과: `92 passed / 0 failed / 0 skipped / 0 inconclusive`, 4.586초.
- Play Mode에서 자동 숙주 이동이 `NorthWest → rat-03-nw`로 표시되는 것을 확인했고, SpriteRenderer 활성, MeshRenderer/MeshFilter 부재를 확인했다.
- MCP 직접 상태 확인으로 8방향 파일 바인딩 전체 일치, 정지 방향 유지 `True`, 카메라 회전 오차 `0.000°`를 확인했다. 실제 WASD 입력 증명으로 승격하지 않는다.
- `Unity_Camera_Capture`는 Play Mode 인스턴스 ID를 찾지 못해 1회 실패했다. 대체로 Main Camera를 RenderTexture에 직접 렌더해 `UnityProject/Temp/rat-directional-sprite-play.png`를 생성했고 쥐 스프라이트의 화면 표시와 바닥 접지를 확인했다.
- Play 종료 후 씬은 clean, 최종 콘솔 Error/Warning 0건이다.

## 2026-07-16 15:20 KST — QA 금지 범위 변경 복구

- 독립 QA가 `UnityProject/ProjectSettings/ProjectSettings.asset`의 Standalone define에 `APP_UI_EDITOR_ONLY`가 추가된 1줄 diff를 발견했다.
- 이 변경은 작업 금지 범위이며 구현 담당의 초기 변경 파일 검토에서 누락됐다.
- HEAD의 `Standalone: SENTIS_ANALYTICS_ENABLED`와 정확히 대조한 뒤, 추가된 `;APP_UI_EDITOR_ONLY`만 `apply_patch`로 제거했다. 다른 ProjectSettings 및 사용자 변경은 건드리지 않았다.
- 복구 직후 `git diff --exit-code -- UnityProject/ProjectSettings/ProjectSettings.asset` 통과 및 해당 파일 Git 상태 변경 0을 확인했다.
- Unity Editor가 열린 상태에서 3초 후 같은 검사를 다시 실행했고 재발하지 않았다.
- 발생 시점상 Unity/MCP 동적 명령 실행 과정의 Editor 전용 define 부수 효과일 가능성이 있으나, 정확한 자동 추가 주체는 이 검증에서 확정하지 않았다.

## 2026-07-16 16:29 KST — 사용자 접지 피드백 수정

- 문제 구조를 일반 바닥 top `-0.02`, 오염 trigger top `0.07`, 쥐 게임플레이 루트 y `0`으로 재확인했다.
- `RatVisualGroundingResolver`를 추가해 후보 중 자기 계층이 아니고, 활성 Renderer가 있으며, 상향 표면이고, 숙주 기준 허용 높이 안에 있는 가장 높은 표면을 선택하도록 했다.
- `RatDirectionalSpriteView`가 `LateUpdate`에서 할당 없는 수직 `RaycastNonAlloc`을 실행하도록 수정했다. `QueryTriggerInteraction.Collide`로 오염 trigger를 포함하고 `RatHost` 자기 `CharacterController`는 계층 비교로 제외한다.
- 접지 보정은 `RatVisual`의 월드 y만 `표면 top + 0.005`로 조정한다. `RatHost`, CharacterController, `ImmuneRiskZone`, trigger 위치·크기·판정, 카메라는 변경하지 않았다.
- 씬 빌더와 현재 씬에 probe 높이 `1.0`, 거리 `1.6`, 최대 상승 `0.2`, 최대 하강 `0.5`, 최소 normal y `0.5`, clearance `0.005`를 동일 반영했다.
- 순수 후보 선택 테스트와 실제 Physics trigger/자기 CharacterController 제외 EditMode 테스트를 추가했다.
- EditMode 전체 결과: `94 passed / 0 failed / 0 skipped / 0 inconclusive`, 4.621초.
- Play 일반 바닥: surface y `-0.0200`, RatVisual pivot y `-0.0150`, clearance `0.0050`.
- Play 오염 구역: surface y `0.0700`, RatVisual pivot y `0.0750`, clearance `0.0050`.
- 캡처: `UnityProject/Temp/rat-grounding-floor.png`, `UnityProject/Temp/rat-grounding-toxic.png`. 두 위치 모두 시각 교차와 과도한 부유가 보이지 않음을 확인했다.
- 방향 회귀: 8개 바인딩 전체 일치 `all8=True`, 정지 유지 `idleHeld=True`.
- Play 종료 후 씬 clean, Console Error/Warning 0건이다.
- Unity/MCP 동적 명령이 `APP_UI_EDITOR_ONLY` define을 다시 추가했고 씬 저장이 무관한 `signalSuppressionHud: null` 1줄을 직렬화했다. 검증 종료 후 두 부수 변경만 최소 복구했으며 ProjectSettings diff 0, Unity 열린 상태 3초 재발 없음까지 확인했다.

## 2026-07-16 — 사용자 접지 피드백

- 사용자가 Play Mode에서 쥐가 오염 구역 표면과 겹쳐 보인다고 보고했다.
- 씬 빌더 기준 일반 바닥 top 약 `-0.02`, 오염 구역 top 약 `0.07`, `RatVisual` pivot y `0`을 대조해 trigger 표면과 표시 기준점의 높이 불일치를 원인으로 좁혔다.
- 기존 내부 승인 상태를 해제하고 시각 접지 수정·재검증 단계로 전환했다.

## 2026-07-20 — 사용자 2차 접지 피드백 구현

- 사용자 피드백의 약 `0.09` y 전환 원인을 접지 후보 선택이 `ToxicWaterRiskZone` trigger의 top `0.07`도 표시용 바닥으로 선택한 데서 확인했다.
- `RatGroundSurfaceCandidate`에 trigger 여부를 보존하고 `RatVisualGroundingResolver`가 trigger를 접지 후보에서 제외하도록 변경했다. 따라서 구역 안팎 모두 일반 바닥 top `-0.02`와 `RatVisual` pivot y `-0.015`를 사용한다.
- `ImmuneRiskZone` GameObject의 위치 `(-0.7, 0.03, 1.35)`, 크기 `(2.6, 0.08, 1.5)`, trigger·Rigidbody·위험 로직은 변경하지 않았다. 해당 Renderer만 비활성화하고, 같은 가로·세로 범위의 collider 없는 `ToxicWaterVisual`을 top `-0.018`(일반 바닥보다 `0.002` 위, center y `-0.023`, thickness `0.01`)에 별도 배치했다. RatVisual pivot `-0.015`보다 `0.003` 낮아 깊이 충돌과 하단 관통을 함께 피한다.
- 씬 빌더와 저장 씬에 같은 구성을 반영했다. EditMode는 trigger 내부/외부에서 같은 RatVisual y, 위험 trigger 구조 및 기존 8방향·idle 회귀를 대조하도록 갱신했다.
- 정적 검증: `git diff --check` 통과. Unity 컴파일·EditMode·Play는 사용자의 열린 Unity Editor와 AssetImportWorker가 `UnityProject`를 점유 중이라 실행하지 않았다. 동적 검증 결과를 주장하지 않는다.
- 인계 직전 Unity 프로세스 재확인 명령은 사용자 중단으로 완료 결과를 수집하지 않았다. 추가 Unity 실행은 하지 않으며, 동적 검증은 QA 인계 항목으로 남긴다.

## 2026-07-20 — QA 발견 ProjectSettings define 최소 복구

- QA가 금지 범위인 `UnityProject/ProjectSettings/ProjectSettings.asset`의 Standalone define 재발(`SENTIS_ANALYTICS_ENABLED;APP_UI_EDITOR_ONLY`)을 발견했다.
- HEAD 기준으로 추가 토큰 `;APP_UI_EDITOR_ONLY`만 `apply_patch`로 제거해 `SENTIS_ANALYTICS_ENABLED`를 복구했다. 다른 ProjectSettings와 사용자 변경은 수정하지 않았다.
- `git diff --check` 통과, `git diff --exit-code -- UnityProject/ProjectSettings/ProjectSettings.asset` 통과, 해당 경로 Git status clean을 확인했다. Unity는 실행하지 않았다.

## 2026-07-20 — QA 발견 ToxicWaterVisual 저장 씬 계층 링크 복구

- QA의 Unity MCP 정적 확인에서 `ToxicWaterVisual` Transform `1999987811`은 `RatHostMode`(`582540621`)를 `m_Father`로 가리키지만, 부모 `m_Children` 목록에 없어 런타임/Hierarchy에 로드되지 않는 결함을 발견했다.
- 저장 `RatHostPrototype.unity`의 `RatHostMode` 자식 목록에 `{fileID: 1999987811}` 한 줄만 추가했다. 씬 빌더의 생성 경로와 Transform 위치·크기, 위험 trigger 로직은 변경하지 않았다.
- 정적 대조: `ToxicWaterVisual`은 Transform·MeshRenderer·MeshFilter만 보유해 Renderer 활성·Collider 부재를 확인했다. `ToxicWaterRiskZone`의 위치 `(-0.7, 0.03, 1.35)`, 크기 `(2.6, 0.08, 1.5)`, `ImmuneRiskZone`, `BoxCollider isTrigger`, `Rigidbody`도 보존됐다.
- `git diff --check` 통과, `ProjectSettings.asset` 경로 diff/status clean을 재확인했다. Unity는 실행하지 않았다.

## 2026-07-20 — QA 발견 방향별 알파 하단 접지 복구

- QA가 공통 `0.1875` offset이 8방향 전체의 실제 불투명 하단을 맞추지 못함을 확인했다. 방향별 pivot-to-foot alpha offset은 `S 0.1875`, `SW 0.15625`, `W 0.09375`, `NW 0.28125`, `N 0.375`, `NE 0.40625`, `E 0.15625`, `SE 0.15625`를 사용한다.
- `RatDirectionalSpriteView`는 현재 8방향에 해당하는 offset을 선택해 pivot y를 `surface y + current foot offset + 0.005`로 계산한다. `groundClearance`는 이제 현재 방향의 보이는 알파 발바닥과 접지 표면 사이의 거리다.
- 따라서 방향별 pivot y는 달라져도, 일반 바닥과 위험 trigger 내부 모두 현재 방향의 실제 foot y는 `-0.015`, clearance는 `0.005`로 같다. trigger는 계속 접지 후보에서 제외되므로 경계에서 foot y가 전환하지 않는다.
- 씬 빌더와 저장 씬의 8개 offset을 동기화했다. EditMode는 8개 방향별 offset, 일반/trigger 내부 foot y 동일, clearance, 8개 PNG의 공통 64x64/PPU32/custom pivot을 명시 대조하도록 갱신했다.
- Unity 컴파일·EditMode·Play는 지시에 따라 실행하지 않았다. 동적 검증은 QA 인계 항목이다.

## 2026-07-20 — QA 발견 알파 하단 기준 접지 복구

- QA가 pivot y만 `groundClearance`로 사용한 기존 방식에서 실제 불투명 쥐 하단이 오염 시각 표면 아래로 관통함을 확인했다.
- `RatDirectionalSpriteView.visibleFootBottomOffset = 0.1875`를 추가했다. 이는 64px 공통 캔버스, PPU 32 기준 6px world offset이며, `groundClearance = 0.005`는 이제 **보이는 발바닥 알파 하단과 선택 표면 사이의 간격**을 뜻한다.
- 접지 계산은 모든 방향에서 `surface y + visibleFootBottomOffset + groundClearance`를 사용한다. 일반 바닥 top `-0.02`와 오염 trigger 내부 모두 RatVisual pivot y `0.1725`, 보이는 하단 y `-0.015`, 일반 바닥 clearance `0.005`가 되어 경계 y 전환이 없다. 오염 시각 표면 top `-0.018`보다도 보이는 하단이 `0.003` 위다.
- 씬 빌더와 저장 `RatHostPrototype.unity`에 offset `0.1875`를 동기화했다. EditMode는 trigger 안/밖 pivot·보이는 하단·clearance 동일, 저장 씬 값, 8개 PNG의 공통 `64x64` 캔버스·PPU 32·custom pivot `(0.5, 0.25)`를 검증하도록 갱신했다.
- 정적 알파 스캔도 기록했다. 8개 PNG의 최하단 non-zero alpha 아래 투명 행은 순서대로 `10, 11, 13, 7, 4, 3, 11, 11`이며, QA가 지정한 6px(`0.1875`) 공통 발바닥 보정 기준을 모든 방향에 동일 적용한다.
- `git diff --check` 통과와 `ProjectSettings.asset` clean을 확인했다. 지시에 따라 Unity 컴파일·EditMode·Play는 실행하지 않았다.
