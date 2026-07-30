# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-30-production2d-occlusion-hud-correction`
- 작업명: Production2D V1 오브젝트 가림·HUD 초상 잔여 조각 수정
- 상태: 내부 승인 완료 — 사용자 실제 WASD 재확인 대기
- 생성일: 2026-07-30
- 담당 에이전트: 비주얼/테크아트 에이전트, 2D 에셋 제작 담당, Unity 씬/통합 구현 에이전트
- 보조 에이전트: QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `$pixel-lowpoly-style-keeper`, `$unity-verification-runner`

## 사용자 피드백

- 쥐가 벽·통 등의 오브젝트를 넘어다닐 때 앞뒤 표현이 부자연스럽다.
- 쥐 초상 HUD 내부 상단에 불필요한 그래픽이 보인다.
- 불명확한 부분은 사용자에게 다시 물어본다.

## 확인된 HUD 원인

- `hud_portrait_frame_256.png` 자체에는 내부 상단 장식이 없다.
- `hud_rat_portrait_184.png` 상단에 황동 프레임 조각이 잘못 포함되어 외곽 프레임과 이중 표시된다.
- 새 이미지 생성이 아니라 기존 제작 소스의 분리·크롭 오류 수정으로 처리한다. 쥐 얼굴·털·실루엣은 변경하지 않는다.

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| 비주얼/테크아트 에이전트 | 원인 재현·수정 기준 | 벽·통 전후 위치 캡처, 정렬·가림·충돌 원인 분리 | 시각 진단 기록 |
| 2D 에셋 제작 담당 | HUD 실제 에셋 수정 | 제작 소스에서 상단 잔여 조각만 제거, 원본/Unity 후보 동기화 자료 | 수정 HUD PNG와 검증 |
| Unity 씬/통합 구현 에이전트 | 실제 구현 | collider footprint·Y-sort/가림 전환과 수정 HUD 반입, 씬 rebuild | Unity 코드·씬·테스트 |
| QA/검증 에이전트 | 독립 검증 | 전후 이동·충돌·정렬·HUD·회귀·상태판 감사 | `verification.md` |
| 프로젝트 총괄 관리자 에이전트 | 내부 승인 | 범위·QA·사용자 확인본 판정 | `director-review.md` |

## 구현 담당 확인

- 코드/테스트 변경 담당: Unity 씬/통합 구현 에이전트
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: Unity 씬/통합 구현 에이전트
- 실제 HUD PNG 제작 수정 담당: 2D 에셋 제작 담당
- 메인 에이전트 직접 구현 여부: 아니오

## 목적

쥐가 벽·통·상자 앞뒤를 지날 때 오브젝트 위로 올라타거나 전체 이미지가 잘못 뒤집히는 듯한 가림을 재현·수정하고, HUD 초상 상단의 잘못 포함된 황동 조각을 제거한다.

## 입력 자료

- `UnityProject/Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- `UnityProject/Assets/_Project/Editor/TechnicalSample2D/RatHost2DProductionSampleSceneBuilder.cs`
- `UnityProject/Assets/_Project/Art/Production2D/V1/HUD/hud_rat_portrait_184.png`
- `_workspace/active/2026-07-30-rat-host-2d-production-assets-v1/source/`
- `_workspace/active/2026-07-30-rat-host-2d-production-assets-v1/artifacts/game-assets/hud/hud_rat_portrait_184.png`
- 이전 V2 Game View와 사용자 피드백

## 해야 할 일

1. 벽·통·상자의 앞·옆·뒤에 쥐 발 접지점을 배치해 프레임별 가림과 충돌 경계를 재현한다.
2. 임의 tie-break, footprint collider, 쥐 발 접지 정렬 중 실제 원인을 기록한다.
3. 정렬 전환이 오브젝트의 지면 접점에서 일어나고, 쥐가 시각 표면 위로 올라탄 것처럼 보이지 않게 최소 수정한다.
4. HUD portrait 상단 황동 잔여 조각만 제거하고 얼굴·털·알파 경계는 보존한다.
5. Production source 실제 에셋과 Unity 반입본의 SHA를 다시 맞춘다.
6. 씬 rebuild·save 후 전후 위치 캡처, 실제 이동 대체 검증, 관련/전체 EditMode, Console, sceneDirty를 확인한다.

## 금지 범위

- 쥐 초상·외형 재생성 또는 얼굴 변경
- 전체 HUD 재디자인
- 전체 8방향 제작
- 전체 타일셋 확장
- Stage2·Stage3·`RatHost2DPrototype`·ProjectSettings 수정
- 패키지 추가와 Windows 빌드
- 사용자가 지적하지 않은 아트 리스타일

## 완료 기준

- HUD 상단 잔여 황동 조각이 사라지고 외곽 프레임만 남는다.
- 벽·통·상자 앞뒤 통과 위치에서 쥐의 접지점과 정렬 순서가 일치한다.
- 충돌 때문에 오브젝트 시각 표면 위로 쥐가 올라탄 것처럼 보이지 않는다.
- 이동 중 정렬 팝·경계 떨림이 기존보다 악화되지 않는다.
- 관련·전체 EditMode, Unity MCP Play, Console, sceneDirty와 보호 diff를 QA가 독립 확인한다.
