# 핸드오프 기록

## 작업 ID

`2026-07-27-2d-playable-technical-sample`

## 최신 사용자 요청

수정된 소품 충돌을 확인했으며 현재 기술 샘플을 커밋·푸시한다.

## 현재 상태

- 상태: 사용자 수용·QA 완료 가능·총괄 내부 승인 가능, 완료 보관·커밋 직전
- 여기서 멈춤: 사용자가 소품 비통과를 직접 확인하고 커밋·푸시를 요청했다.
- 다음 세션의 첫 목표: 완료 폴더·현황판·Git 상태를 대조해 선별 커밋·푸시한다.

## 먼저 읽을 파일

1. `task.md`
2. `docs/design/visual/pixel-isometric-2d-production-guide.md`
3. `.codex/skills/unity-prototype-planner/references/unity-architecture.md`

## 건드리면 안 되는 기존 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY`
- `_workspace/previews/`
- 기존 `RatHostPrototype` 씬과 레거시 2.5D 산출물

## 마지막 성공 검증

- Unity MCP `GetState`: 비재생·비컴파일
- 활성 씬 `RatHostPrototype`: dirty false
- Tilemap·Physics2D·Input System·Test Framework 존재
- 아키텍처·수용 기준·비주얼 규격 자체 검증 통과
- 게임플레이 코드 단위 EditMode `34/34`, Console Error/Warning `0/0`
- 씬 계약 포함 전체 EditMode `36/36`, Console 초기화 후 Error/Warning `0/0`
- 독립 전체 EditMode `137/137`, MCP Play 입력·카메라·idle 통과
- Windows 임시 빌드 성공, 기존 3D SHA·Packages·BuildSettings·저장소 Builds 보존

## 실패 또는 차단된 검증

- 첫 E08 충돌 검증은 `-0.005` 겹침으로 `32/34`였으나 이동 전 cast와 `1/64` world unit 안전 폭으로 보정해 재실행 `34/34`를 통과했다.
- E01 첫 실행은 EditMode 재로드 직후 카메라 캐시 null로 실패했으나 getter 지연 조회 보정 후 전체 `36/36`을 통과했다.
- 빌드 직후 Unity 자동 설정 diff로 QA가 한 차례 차단했으나 task-owned diff를 복구하고 사용자의 APP_UI 한 줄만 보존해 최종 `완료 가능` 판정을 받았다.
- signed distance `-0.000177` 미세 접촉과 실제 Game View HUD·키보드 체감은 남은 수동 수용 항목이다.
- Computer Use Windows EXE 실행은 앱 실행 승인 만료로 수행하지 못했다.
- 사용자 발견 소품 관통은 하단 footprint collider로 수정했으며 QA addendum `완료 가능`, 총괄 최종 `내부 승인 가능`, 사용자 실제 플레이 수용을 받았다.

## 이어서 해야 할 일

1. 사용자에게 수정된 통·파이프 충돌·우회·가림을 우선 재확인 요청한다.
2. 필요하면 Computer Use 앱 실행 승인을 받아 Windows 실행본을 재검증한다.
3. 사용자 확인 전 시험값과 기술 플레이스홀더를 최종 규격으로 승격하지 않는다.

## 사용자 승인 필요

- 현재 기술 샘플 완료·보관·커밋·푸시는 승인 완료
- 시험 규격과 기술 플레이스홀더의 최종 규격·최종 아트 승격은 별도 승인 필요
