# 작업 배정서 — 쥐 v2 걷기 Unity 실제 화면 시험

## 기본 정보

- 작업 ID: `2026-07-20-rat-walk-unity-visual-trial`
- 작업명: 쥐 v2 걷기 8방향 스프라이트를 실제 RatHost 게임 화면에서 검증
- 상태: 검증 진행 중 — v3 Blender 동작 폭 확대 반입·핵심 Play 확인 완료, 사용자 체감·독립 QA·총괄 판정 대기
- 생성일: 2026-07-20
- 담당 에이전트: 게임플레이 구현 에이전트, Unity 씬/통합 구현 에이전트
- 보조 에이전트: Blender 애니메이션 테크아트 에이전트, QA/검증 에이전트
- 사용 스킬: `$pixel-lowpoly-style-keeper`, `$unity-verification-runner`

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| Blender 애니메이션 테크아트 | 입력 인계 | v2 출력 경로·프레임 맵·피벗 기준 제공 | 기존 v2 핸드오프 |
| 게임플레이 구현 | 런타임 표시 | 8방향별 프레임 순환이 기존 방향 표시와 충돌하지 않도록 최소 C# 구현 | 코드·테스트·핸드오프 |
| Unity 씬/통합 구현 | 에셋·씬 연결 | 별도 시험 PNG 반입, Import·씬 연결 | Unity 에셋·씬 연결 기록 |
| QA/검증 | 실제 화면 확인 | 컴파일·Play·MCP 화면·콘솔·접지 검증 | `verification.md` |
| 프로젝트 총괄 관리자 | 내부 판정 | 범위·QA 기록·사용자 보고 가능 여부 확인 | `director-review.md` |

## 구현 담당 확인

- 코드/테스트 변경 담당: 게임플레이 구현 에이전트
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: Unity 씬/통합 구현 에이전트
- 메인 에이전트 직접 구현 여부: 아니오

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: Unity 코드·스프라이트 에셋·씬 연결 변경과 실제 Play 검증
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예

## 목적

Blender v2 보행이 실제 RatHost 쿼터뷰 게임 화면에서 다리 움직임으로 읽히는지 확인한다. 이 작업은 최종 아트·최종 재생 사양 확정이 아니라 별도 시험 자산을 통한 시각 검증이다.

## 입력 자료

- `_workspace/active/2026-07-20-rat-walk-animation-blender/artifacts/blender-animation-v2-handoff.md`
- `_workspace/active/2026-07-20-rat-walk-animation-blender/artifacts/renders-v2/`
- `_workspace/active/2026-07-20-rat-walk-animation-blender/artifacts/walk-frame-map-v2.csv`
- `UnityProject/Assets/_Project/Scripts/Host/RatDirectionalSpriteView.cs`
- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- 기존 정지 8방향 TrialV1 스프라이트와 해당 `.meta`

## 해야 할 일

1. v2 64 PNG를 기존 정지 자산과 별도 Unity 시험 경로로 반입하고, Point/Nearest·투명·공통 피벗 기준을 확인한다.
2. 기존 방향 정량화와 게임플레이 판정을 보존한 채, 이동 중 해당 방향의 8프레임 걷기 이미지를 순환 표시하는 최소 런타임 표시를 구현한다.
3. RatHostPrototype 씬에서 시험 표시를 연결하고, 정지·이동·방향 전환·카메라·접지·가림을 실제 Play에서 확인한다.
4. 화면 증적과 콘솔·컴파일·테스트 결과를 작업 폴더에 남긴다.

## 산출물

- `UnityProject/Assets/_Project/Sprites/Characters/Rat/WalkTrialV2/**`의 시험 스프라이트와 Import 설정
- 필요한 최소 런타임 표시 코드·테스트
- 씬 연결 결과와 실제 화면 증적
- `handoff.md`, `verification.md`, `completion-report.md`, `director-review.md`

## 금지 범위

- 기존 정지 TrialV1 자산·원본·v2 Blender 출력의 덮어쓰기·삭제
- 숙주 루프·면역·상호작용·콜라이더·이동 속도·변이 시스템의 변경
- 새 패키지, ProjectSettings, Build Settings, 최종 아트·최종 재생 속도 확정
- v3 리그/Blender 원본 수정

## 승인 필요 항목

- 사용자 승인: 2026-07-20, 실제 게임 화면에서의 v2 걷기 시험 반입·검증 승인
- 별도 승인 필요: 시험 결과의 최종 수용, 기존 정지 자산 교체, 제품용 재생 사양 확정, v3 Blender 수정

## 2026-07-20 보행 가독성 재작업 승인

- 사용자 피드백: Unity 화면에서 쥐를 키우는 것이 아니라 실제 보행 동작 폭을 더 크게 한다.
- 승인된 변경: v2를 읽기 전용 입력으로 한 별도 v3 Blender 원본·64 PNG 시험 출력, 그리고 기존 Unity 시험 연결을 v3로 교체한 뒤 Play 재검증.
- 유지 조건: 기존 v2·TrialV1·게임플레이 이동·충돌·카메라·ProjectSettings는 보존한다. Unity의 임시 `RatVisual` 1.35배 확대는 v3 적용 전에 되돌린다.

## 완료 기준

1. 실제 RatHost 게임 화면에서 이동 시 방향별 v2 프레임이 표시된다.
2. 정지·이동·방향 전환에서 캐릭터 피벗·접지·카메라 프레이밍·충돌 판정이 깨지지 않는다.
3. QA가 가능한 범위의 Unity MCP Play·콘솔 확인과 증적을 기록한다.
4. 총괄 관리자 판정은 시험 통합 범위에 한정하며, 최종 아트 수용과 Unity 제품 반입을 혼동하지 않는다.
