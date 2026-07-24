# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-21-game-view-camera-output-fix`
- 작업명: Game 뷰 카메라 출력 복구
- 상태: 진행 중
- 생성일: 2026-07-21 KST
- 담당 에이전트: Unity 씬/통합 구현
- 보조 에이전트: QA/검증, 프로젝트 총괄
- 사용 스킬: `pixel-lowpoly-style-keeper`, `unity-verification-runner`

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| Unity 씬/통합 구현 | 출력 경로 수정 | Game 뷰 직접 표시와 RenderTexture 픽셀 월드 출력의 공존 | 씬/필요 최소 코드 수정, 구현 기록 |
| QA/검증 | 독립 플레이 확인 | Game 뷰 출력, 카메라 추적, 픽셀 처리, HUD, 콘솔 확인 | `verification.md` |
| 프로젝트 총괄 | 범위·게이트 검토 | 3D 하이브리드 방향, v5b 시험안 경계, 기록 정합성 판단 | `director-review.md` |

## 구현 담당 확인

- 코드/테스트 변경 담당: Unity 씬/통합 구현
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: Unity 씬/통합 구현
- 메인 에이전트 직접 구현 여부: 예 — 2026-07-23 사용자 명시 승인
- 메인 에이전트 직접 구현 예외 사유: 사용자가 쿼터뷰 카메라와 쥐가 따로 노는 실제 증상 및 WASD 입력 시 쥐가 의도하지 않은 곳으로 이동하는 증상을 확인하고 즉시 수정하도록 명시 승인했다. 2026-07-24에는 픽셀 작업 이후 같은 현상이 지속됨을 재확인해, 원인으로 측정된 `RatDirectionalSpriteView`의 누적 자식 오프셋과 씬의 중복 시각 스냅까지 같은 최소 수정 범위에 포함한다. 이후 이동 문제가 해결된 것으로 확인하고, 진단 중 임시 제거했던 무입력 숙주 본능 이동을 안전 조건(WASD 입력 우선·본능 휴지 시 정지)을 유지한 채 다시 반영하도록 명시 요청했다. 현재 협업 운영 제약으로 구현 에이전트를 별도 배정할 수 없어, 메인 에이전트가 관련 코드·씬 설정·회귀 EditMode 테스트만 변경한다.

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: Unity 씬·카메라·출력 경로 변경
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예

## 목적

Unity `Display 1` Game 뷰에서 `No cameras rendering`이 표시되지 않고, 실제 쿼터뷰 카메라 추적 화면이 이동 중에도 표시되게 한다. v5b의 960×540 Point RenderTexture 기반 픽셀 월드 출력과 HUD 분리 구조는 유지한다.

## 입력 자료

- `Assets/_Project/Scenes/RatHostPrototype.unity`
- `Assets/_Project/Settings/Rendering/RatPixelTrial960x540.renderTexture`
- `Assets/_Project/Scripts/Core/PrototypeCameraController.cs`
- `_workspace/active/2026-07-21-rat-pixel-treatment-v5/verification.md`
- `docs/design/visual/graphics-direction-management.md`

## 해야 할 일

1. `IsometricCamera`가 RenderTexture로 렌더하는 상태에서 Display 1에 직접 출력이 없는 원인을 씬 기준으로 확인한다.
2. 월드 저해상도 출력은 유지하면서 Game 뷰에 프레임을 제공하는 최소한의 출력 경로를 구현한다.
3. Play 중 쥐 이동·카메라 추적·HUD가 보이고, 콘솔 오류/경고가 없는지 독립 QA에 인계한다.

## 산출물

- 필요한 최소 Unity 씬/코드 변경
- `work-log.md`, `agent-activity.md`, `handoff.md`
- QA `verification.md`와 총괄 `director-review.md`

## 에이전트 수행 이력 기록

- `agent-activity.md` 생성 여부: 예
- 담당 에이전트별 수행 내용 기록 여부: 예
- 위임/검토/승인 판정 기록 여부: 예

## 금지 범위

- 승인된 WASD 방향 분리·무입력 숙주 본능 이동 복구 외의 쥐 이동·충돌·면역·변이 루프 변경
- v5b를 다른 오브젝트의 공통 그래픽 기준으로 자동 승격
- 새 패키지, ProjectSettings, Build Settings 변경
- 픽셀 월드 RenderTexture를 제거하고 고해상도 직접 렌더로 되돌리는 방향 전환

## 승인 필요 항목

- 목표 외 카메라 구도·시각 방향 변경
- v5b 공통 기준 승격 또는 팔레트·해상도 기준 변경

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 필요
- 담당 에이전트 산출물 확인: 필요
- 에이전트 수행 이력 확인: 필요
- 구현 담당 에이전트 확인: 필요
- 메인 에이전트 직접 구현 예외 사유 확인: 확인됨 — 2026-07-23 사용자 즉시 수정 승인과 2026-07-24 픽셀 회귀·숙주 본능 복구 요청을 `구현 담당 확인` 및 `agent-activity.md`에 기록
- QA/검증 에이전트 기록 확인: 필요
- 총괄 관리자 판정 확인: 필요
- 승인 게이트 확인: 사용자 요청으로 카메라 출력 수정 승인됨
- 완료 판단에 영향을 주는 미검증 항목: 실제 사용자 Game 뷰 체감

## 완료 기준

- Play 중 Display 1 Game 뷰에 `No cameras rendering`이 나타나지 않는다.
- 쥐 이동에 따라 쿼터뷰 화면이 추적되고, 960×540 Point 월드 출력과 HUD 분리가 유지된다.
- Unity MCP Play 확인·콘솔 점검을 QA가 기록하고 총괄 관리자가 판정한다.
