# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-30-rat-host-2d-production-assets-unity-sample`
- 작업명: 고품질 실제 에셋 Unity 한 방 반입 기술 샘플
- 상태: 내부 승인 완료 — 사용자 실제 WASD·PPU 수용 대기
- 생성일: 2026-07-30
- 담당 에이전트: Unity 씬/통합 구현 에이전트
- 보조 에이전트: 비주얼/테크아트 에이전트, QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `$pixel-lowpoly-style-keeper`, `$unity-prototype-planner`, `$unity-verification-runner`

## 사용자 승인 근거

- 사용자는 실제 RGBA 1차 에셋 품질을 수용하고 Unity 반입과 다음 작업 진행을 승인했다.
- 2026-07-27 승인된 현재 방향은 2D 아이소메트릭/쿼터뷰 도트다.

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| Unity 씬/통합 구현 에이전트 | 실제 구현 | PNG·JSON 반입, Import 설정, 독립 기술 샘플 씬 구성, 카메라·충돌·Y정렬·HUD 연결 | Unity 에셋, 메타, 씬/빌더 변경, 구현 기록 |
| 비주얼/테크아트 에이전트 | 시각 검토 | PPU 후보, 화면 점유율, 접지, 반복, 가림, HUD 배율 검토 | 비주얼 검토 기록 |
| QA/검증 에이전트 | 독립 검증 | Import 규격, EditMode, MCP Play, Console, 보호 diff 대조 | `verification.md` |
| 프로젝트 총괄 관리자 에이전트 | 내부 승인 | 범위·승인·QA 기록 대조 | `director-review.md` |

## 구현 담당 확인

- 코드/테스트 변경 담당: Unity 씬/통합 구현 에이전트
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: Unity 씬/통합 구현 에이전트
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 해당 없음

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: Unity 에셋 Import, 씬, UI, 테스트 변경
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예

## 목적

승인된 실제 RGBA 환경·쥐·HUD 1차 묶음을 기존 독립 2D 기술 샘플 한 방에 반입해 실제 플레이 화면에서 품질, 반복, 접지, 이동, 충돌, Y정렬, 가림, 카메라, HUD 배율을 검증한다.

## 입력 자료

- `_workspace/active/2026-07-30-rat-host-2d-production-assets-v1/artifacts/game-assets/`
- `_workspace/active/2026-07-30-rat-host-2d-production-assets-v1/artifacts/asset-manifest.md`
- `UnityProject/Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- `docs/design/visual/pixel-isometric-2d-production-guide.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`

## 해야 할 일

1. 실제 PNG·JSON을 `UnityProject/Assets/_Project/Art/Production2D/V1/` 아래 역할별 경로에 반입한다.
2. Sprite, Point, mipmap off, 무손실/무압축, 알파와 피벗을 적용하고 후보 PPU를 기록한다.
3. 기존 `RatHost2DTechnicalSample`을 기준으로 실제 바닥·벽·물·소품·쥐·HUD를 사용하는 독립 한 방 샘플을 구성한다.
4. 논리 루트와 시각 자식을 분리하고 기존 이동·카메라·충돌을 보존한다.
5. 쥐 측면 3프레임은 현재 제공 방향에 한해 보행 샘플로 사용하고 미제작 방향은 임의 생성하지 않는다.
6. 반복 타일, 벽/수로 충돌, 통·상자 앞뒤 Y정렬과 가림, 쥐 접지, 카메라 중심, HUD 상태를 검증한다.

## 산출물

- Unity 실제 에셋 반입 경로와 `.meta`
- 독립 기술 샘플 씬 또는 해당 씬 재현 빌더
- 필요한 최소 통합 코드와 EditMode 테스트
- `work-log.md`, `agent-activity.md`, `handoff.md`
- `verification.md`, `director-review.md`
- 사용자 확인용 Unity Game View 캡처

## 금지 범위

- `RatHost2DPrototype.unity`와 Stage2·Stage3 미커밋 변경 수정
- `ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 변경 수정
- 기존 3D/2.5D 씬·에셋 삭제
- 패키지 추가와 렌더 파이프라인 변경
- 전체 8방향·전체 하수도 타일셋·전체 UI 제작
- 후보 PPU·셀·내부 해상도의 최종 승격
- Windows 정식 배포 빌드와 전체 핵심 루프 아트 교체

## 승인 필요 항목

- 이번 독립 한 방 Unity 반입은 사용자 승인 완료.
- 전체 8방향과 전체 프로토타입 적용은 이번 샘플 사용자 수용 뒤 별도 진행한다.

## 완료 기준

- 실제 RGBA 에셋이 Unity Import 규격에 맞고 손상 없이 로드된다.
- 독립 한 방에서 환경 반복, 쥐 이동·접지, 벽/수로/소품 충돌, Y정렬·가림, 카메라 중심, HUD가 함께 보인다.
- 기존 기술 샘플 이동·카메라 회귀 테스트와 신규 Import/씬 테스트가 통과한다.
- QA가 Unity MCP Play 진입·종료, 주요 오브젝트, HUD, Console Error/Warning을 독립 확인한다.
- 보호 대상 변경을 보존하고 총괄 관리자가 `내부 승인 가능`을 판정한다.
