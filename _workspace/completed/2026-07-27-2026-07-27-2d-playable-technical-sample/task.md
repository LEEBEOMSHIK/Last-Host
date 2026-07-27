# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-27-2d-playable-technical-sample`
- 작업명: 실제 2D 플레이어블 기술 샘플
- 상태: 완료 — 사용자 플레이 수용·보관·커밋 승인
- 생성일: 2026-07-27 KST
- 담당 에이전트: Unity 아키텍처, 게임플레이 루프, 게임플레이 구현, Unity 씬/통합 구현
- 보조 에이전트: 비주얼/테크아트, QA/검증, 문서/릴리즈, 프로젝트 총괄 관리자
- 사용 스킬: `unity-prototype-planner`, `pixel-lowpoly-style-keeper`, `rat-host-loop-builder`, `unity-verification-runner`

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| Unity 아키텍처 | 구조·패키지·씬 경계 | 별도 2D 씬, 기존 3D 보존, 패키지 비추가 구조 | `artifacts/architecture.md` |
| 게임플레이 루프 | 입력·수용 기준 | WASD·충돌·카메라·정렬 테스트 시나리오 | `artifacts/acceptance.md` |
| 비주얼/테크아트 | 시험 규격 | 목업 경계, 타일·PPU·픽셀·플레이스홀더 기준 | `artifacts/visual-spec.md` |
| 게임플레이 구현 | C#·테스트 | 2D 이동·카메라·정렬·EditMode 테스트 | 코드·테스트·핸드오프 |
| Unity 씬/통합 구현 | 씬·타일·HUD | 별도 기술 샘플 씬과 재현 가능한 씬 빌더 | 씬·에디터 통합·핸드오프 |
| QA/검증 | 독립 검증 | 컴파일·테스트·MCP Play·빌드·상태판 대조 | `verification.md` |
| 프로젝트 총괄 | 내부 승인 | 범위·QA 기록·사용자 확인 경계 판정 | `completion-report.md` 판정 |

## 구현 담당 확인

- 코드/테스트 변경 담당: 게임플레이 구현 에이전트
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: Unity 씬/통합 구현 에이전트
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 해당 없음

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: Unity 코드·테스트·씬·에셋을 변경하는 플레이어블 구현
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예

## 목적

기존 3D `RatHostPrototype`을 보존한 채 별도 `RatHost2DTechnicalSample` 씬에서 실제 2D 아이소메트릭 플레이 규격과 이동·충돌·정렬·카메라의 기술 성립 여부를 검증한다.

## 입력 자료

- `AGENTS.md`
- `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
- `docs/design/visual/pixel-isometric-2d-production-guide.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `.codex/skills/unity-prototype-planner/references/unity-architecture.md`
- 기존 `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`

## 해야 할 일

1. 새 패키지 없이 Unity 내장 Tilemap·Physics2D와 기존 Input System을 사용한다.
2. 별도 `RatHost2DTechnicalSample` 씬과 재현 가능한 에디터 씬 빌더를 만든다.
3. 작은 하수도 방에 반복 바닥·벽·수로, 2D 충돌, 소품 앞뒤 정렬을 구성한다.
4. 기술 플레이스홀더 쥐로 WASD 이동, 8방향 표시 후보, 정수 픽셀 스냅 카메라 추적을 구현한다.
5. 최소 HUD에 샘플 상태·시험 규격·조작 안내를 표시한다.
6. EditMode 테스트, Unity MCP Play, 콘솔, Windows 임시 빌드를 검증한다.

## 시험 규격

- 기준 화면 후보: `960×540`, 16:9
- 아이소메트릭 셀 후보: `64×32 px`
- PPU 후보: `64`
- 카메라: 고정 직교, 플레이어 추적, `1/64` 월드 단위 스냅
- 입력: 화면 기준 WASD, 정규화된 대각선 속도
- 정렬: 발 접지점 Y 기반 결정론적 Sorting Order
- 상태: 사용자 수용 전까지 모두 시험값이며 최종 규격이 아니다.

## 산출물

- `UnityProject/Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- 2D 기술 샘플 런타임 코드·Editor 씬 빌더·EditMode 테스트
- 기술 플레이스홀더 타일·쥐·소품
- 작업 패킷 설계·수용 기준·검증·총괄 판정

## 금지 범위

- 기존 `RatHostPrototype` 씬·코드·스프라이트·검증 기록 삭제 또는 파괴적 교체
- 신규 Unity 패키지 설치, URP/Unity 버전 변경
- 목업이나 기술 플레이스홀더를 최종 게임 에셋으로 선언
- 면역 미니게임·변이·전체 핵심 루프의 2D 이관
- 인간·병원·연구소·백신·엔딩 등 범위 밖 콘텐츠
- 사용자 로컬 `ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY`, `_workspace/previews/` 변경

## 승인 필요 항목

- 사용자의 이번 요청으로 별도 2D 기술 샘플의 Unity 코드·씬·플레이스홀더 에셋 생성과 검증을 승인받았다.
- 신규 패키지 설치 또는 기존 3D 프로토타입 교체는 승인되지 않았으며 필요 시 별도 질문한다.
- 시험 규격의 최종 승격은 구현·검증 결과와 사용자 화면 확인 후 별도 승인한다.

## 완료 기준

- 별도 씬에서 반복 타일, 2D 충돌, WASD 이동, 카메라 중심 유지, 순간이동 없음, Y 정렬과 최소 HUD가 동작한다.
- 기존 3D 씬이 변경되지 않는다.
- EditMode 테스트·MCP Play·콘솔·Windows 임시 빌드 결과가 기록된다.
- QA/검증 `완료 가능`과 프로젝트 총괄 `내부 승인 가능` 판정이 있다.
- 사용자 수동 화면·조작 수용이 필요한 항목은 자동 완료와 분리한다.
