# 작업: 쥐 정지 8방향 스프라이트 Unity 시험 반입

## 상태

- 작업 ID: `2026-07-16-rat-directional-sprite-unity-integration`
- 시작: 2026-07-16 15:04 KST
- 상태: 사용자 2차 접지 피드백 구현·QA·총괄 확인 완료 — 사용자 Game View 확인 대기
- 사용자 근거: 사용자가 `preview/index.html`로 시험 에셋을 확인한 뒤 승인했으며, 앞서 제안한 Unity 시험 반입을 진행한다.

## 목적

현재 Play Mode의 캡슐 `RatVisual`을 승인된 정지 8방향 쥐 스프라이트 시험 표시로 교체한다. 기존 3D 이동·충돌·게임플레이 루트는 유지하고, 카메라 기준 이동 방향에 따라 8방향 프레임을 선택한다.

## 입력 자료

- 시험 에셋: `_workspace/completed/2026-07-16-2026-07-16-rat-8-direction-trial-asset/artifacts/renders/rat-00-s.png`부터 `rat-07-se.png`
- 방향표: `_workspace/completed/2026-07-16-2026-07-16-rat-8-direction-trial-asset/artifacts/direction-map.csv`
- 렌더 설정: `_workspace/completed/2026-07-16-2026-07-16-rat-8-direction-trial-asset/artifacts/render-settings.json`
- 현재 씬: `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- 씬 빌더: `UnityProject/Assets/_Project/Editor/RatHostPrototypeSceneBuilder.cs`
- 이동 방향 입력: `RatHostController.CurrentResolvedMoveDirection`

## 담당

- 조정·통합: 메인 에이전트
- 실제 Unity 변경: Unity 씬/통합 구현 에이전트
- 독립 검증: QA/검증 에이전트
- 완료 판정: 프로젝트 총괄 관리자 에이전트

## 산출물

- Unity 프로젝트 내부의 8개 개별 Sprite 자산
- Point 필터, mipmap 비활성, 투명 배경, 공통 pivot을 보존한 Import 설정
- 카메라 기준 이동 벡터를 8방향으로 양자화하고 정지 시 마지막 방향을 유지하는 표시 전용 컴포넌트
- `RatVisual` 캡슐 렌더 제거 또는 비활성화 및 SpriteRenderer 표시
- 씬 빌더 재실행 후에도 같은 구성이 재현되는 편집기 통합
- 방향 선택 로직 EditMode 테스트와 Unity MCP Play 확인 기록

## 수용 기준

1. `RatHost`의 CharacterController, 이동, 상호작용, 면역 경계도 루프를 바꾸지 않는다.
2. Play Mode에서 기존 캡슐 메시가 보이지 않고 쥐 스프라이트가 보인다.
3. 카메라 기준 이동 방향 8개가 `S, SW, W, NW, N, NE, E, SE`에 일관되게 대응한다.
4. 입력이 멈추면 마지막 유효 방향을 유지한다.
5. 스프라이트가 고정 카메라를 향하고 바닥 접지 위치가 급격히 흔들리지 않는다.
6. Import는 Sprite/Point/no mipmap/alpha 조건을 만족한다.
7. EditMode 테스트와 컴파일이 통과하고, Unity MCP Play 진입·이동·종료 후 Error/Exception이 없다.
8. 씬 저장 여부와 변경 파일이 작업 기록에 남는다.

## 금지 범위

- 걷기·공격·피격 애니메이션 추가
- 새 3D 모델 또는 Blender 수정
- 게임플레이 로직, 충돌체, 속도, 카메라, URP, ProjectSettings, Build Settings 변경
- 패키지 추가
- 바이러스·백혈구 비주얼 교체
- `.codex/config.toml`, `_workspace/previews/` 수정
- 자연 경계도 엄격 검증의 차단 판정 변경

## 중단 조건

- `RatHostController`의 게임플레이 동작 변경이 필요하면 Unity 씬/통합 담당은 중단하고 게임플레이 구현 담당으로 재배정한다.
- 방향표 또는 시각 방향을 바꾸어야 하면 사용자 승인을 다시 받는다.

## 사용자 Play 피드백 — 2026-07-16

- 문제: 쥐가 오염 구역을 지날 때 스프라이트 하단과 오염 표면이 겹쳐 보인다.
- 확인된 구조 원인: 일반 바닥 top은 약 `-0.02`, 오염 구역 top은 약 `0.07`인데 오염 구역은 trigger라 쥐 이동 기준점 y는 올라가지 않는다. `RatVisual` pivot이 y `0`에 남아 오염 표면이 쥐 하단을 관통한다.
- 수정 원칙: CharacterController, trigger 크기와 위험 판정은 유지하고 표시 전용 접지 높이만 현재 시각 표면에 맞춘다.
- 추가 수용 기준: 일반 바닥과 오염 구역 양쪽에서 쥐 하단이 표면과 교차하거나 눈에 띄게 뜨지 않는다.

## 사용자 Play 피드백 2차 — 2026-07-20

- 문제: 오염 구역의 표면 관통을 피하려고 `RatVisual`을 오염 trigger 상단으로 올린 뒤, 구역 밖으로 나올 때 다시 내려와 일반 지면과 맞지 않는 인상을 준다.
- 수정 원칙: 쥐 표시의 접지 높이는 오염 trigger에 따라 전환하지 않는다. 오염 위험 판정 trigger는 유지하되, 오염 시각 표면을 일반 하수도 바닥 높이로 맞추거나 그 아래로 배치해 쥐 스프라이트와 교차하지 않게 한다.
- 추가 수용 기준:
  1. 오염 구역 진입·이탈 시 `RatVisual`의 y 높이가 변하지 않는다.
  2. 일반 바닥과 오염 시각 표면 모두에서 **스프라이트의 불투명 하단**이 눈에 띄게 뜨거나 관통하지 않는다. Transform pivot만으로 접지 판정을 대체하지 않는다.
  3. `ImmuneRiskZone` trigger 위치·크기·위험 판정, `RatHostController`, CharacterController 및 카메라를 변경하지 않는다.
