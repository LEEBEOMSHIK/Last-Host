# 프로젝트 총괄 관리자 최종 검토

## 검토 대상

- `docs/prototype/approvals/rat-host-2d-core-loop-migration-brief.md`
- `_workspace/active/2026-07-27-rat-core-loop-2d-migration-brief/` 작업 패킷 전체
- 역할별 검토안 3개
- `verification.md`
- `docs/project-handoff/current-task-board.md`
- `_workspace/active/CURRENT.md`
- `AGENTS.md`
- `.agents/project-director-agent.md`
- `docs/agents/loop-engineering-gates.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/approvals/rat-host-approval-packet.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `docs/design/systems/immune-alert.md`
- `docs/design/hosts/host-instinct-control.md`

## 판정

**내부 승인 가능 — 사용자 승인 대기**

승인 브리프의 방향·범위·기술 경계·사용자 결정 항목은 현재 프로젝트 기준과 맞는다. 실제 Unity 이관은 아직 승인되지 않았으므로 브리프를 사용자에게 제시해 추천안 수용 여부를 받아야 한다.

단, 사용자 보고 전에 메인 조정자가 `agent-activity.md`, `handoff.md`, `_workspace/active/CURRENT.md`, `current-task-board.md`의 QA 이전 상태 문구를 현재 `QA 완료·총괄 내부 승인·사용자 승인 대기` 상태로 동기화해야 한다. 이는 브리프 내용 수정 사유는 아니지만, 동기화하지 않은 채 완료·보관·커밋으로 진행하면 상태판 게이트 차단 사유다.

## 근거

### 1. 프로젝트 방향

- 브리프는 2026-07-27 사용자 승인으로 확정된 `2D 아이소메트릭/쿼터뷰 도트 게임`을 현재 기준으로 사용한다.
- 환경·캐릭터·효과를 2D 타일·방향별 스프라이트·2D 충돌·발 기준 Y 정렬로 다루며 과거 2.5D/Blender 방향으로 되돌리지 않는다.
- `960×540`, `64×32`, PPU `64`, 기술 플레이스홀더를 최종 규격이나 최종 아트로 승격하지 않는다.

### 2. 세 단계 이관 범위

- 1단계: `2D 쥐 숙주 탐험·숙주 본능/WASD 인계·면역 경계도·100% 전환 셸`
- 2단계: `2D 백혈구 회피·변이 조각·성공/실패`
- 3단계: `변이 선택·효과 적용·2D 쥐 숙주 복귀`
- 각 단계에 포함·제외·수용 기준과 전체 루프 완료 주장 경계가 있다.
- 벌레 튜토리얼, 다중 숙주, 인간 단계, 병원·연구소·백신, 엔딩, 영구 성장, 최종 아트는 계속 제외한다.

### 3. 면역 경계도와 내부 미니게임 라우팅

- 실제 기본값 `PrototypeConfig.BaseAlertPerSecond=0`을 반영해 무위험 대기·일반 이동의 자동 경계도 상승을 사용하지 않는다.
- 1단계 자연 100% 경로는 `ContaminationExposure` 2D 오염 구역으로 제한한다.
- 실제 상태 로직의 `ContaminationExposure → WhiteBloodCellEvasion` 라우팅과 맞는다.
- `NoiseOrTissueIrritation`, `ForcedHostControl`은 현재 `ImmuneSignalSuppression`으로 라우팅되므로, 신호 억제 이관을 보류하는 1~2단계에서는 해당 면역 트리거를 2D 씬에 노출하지 않는다.
- 숙주 본능 이동과 WASD 인계 자체는 1단계에 남기되, 보류된 강제 조종 면역 트리거와 구분한다.
- 따라서 자연 100% 이후 미구현 신호 억제형으로 들어가는 경로가 기본안에 남아 있지 않다.

### 4. 잠복 강화 `0.55`

- 현재 `MutationLoadout.ImmuneAlertRateMultiplier=0.55`는 시간 기반 `ImmuneAlert.Tick`에만 적용되고 위험 이벤트·원시 증가량에는 자동 적용되지 않는다.
- 기본 시간 상승이 0인 현재 설정에서는 기존 코드 그대로일 때 잠복 강화 체감이 거의 없다.
- 브리프는 시간 자동 상승을 다시 켜지 않고 위험 행동 경계도 상승량에도 `0.55`를 적용하는 변경을 3단계의 명시적 사용자 승인 항목으로 분리했다.
- 이는 승인된 “면역 경계도 상승 억제” 의도와 맞는 추천이지만 실제 동작 변경이므로 사용자 승인 전 확정하지 않은 처리가 적절하다.
- 구현 시 `AddRiskAlert`와 `AddImmuneAlertAmount` 양쪽 적용 범위를 테스트해야 한다는 QA 지적도 타당하다.

### 5. 씬·어셈블리 전략

- 검증용 `RatHost2DTechnicalSample.unity`를 계속 증축하지 않고 별도 `RatHost2DPrototype.unity`를 만드는 추천이 적절하다.
- 첫 전체 루프까지 단일 씬 안에서 Host/Virus 모드 root를 전환하고, 초기부터 Bootstrap·additive 씬 수명 문제를 추가하지 않는다.
- 신규 `LastHost.Prototype.RatHost2D` 어셈블리가 기존 `LastHost.Prototype`과 `LastHost.Prototype.TechnicalSample2D`를 참조하는 구조는 현재 참조 방향에서 순환을 만들지 않는다.
- `PrototypeSessionState`, 면역·미니게임·변이 상태 모델은 참조 재사용하고, 3D `MonoBehaviour`, 물리·카메라·UI 결합부는 2D로 교체한다.
- `PrototypeSessionState`가 완전한 Unity 비의존 Domain은 아니라는 제한을 QA가 기록했으며, 대규모 Domain 추출을 전체 2D 루프 수용 이후로 미룬 판단도 회귀 위험을 줄인다.

### 6. 기존 3D와 기술 샘플 보존

- 기존 `RatHostPrototype.unity`, 3D 코드·테스트·검증 자료를 1~3단계의 비교·회귀 기준으로 유지한다.
- `RatHost2DTechnicalSample.unity`와 전용 테스트를 2D 이동·카메라·충돌·정렬 회귀 기준으로 유지한다.
- 전체 2D 루프 수용 뒤에도 기존 3D/2.5D/Blender 산출물을 자동 삭제하지 않고, 정리는 별도 승인으로 남긴다.
- 기술 샘플의 Windows 실행본 플레이 미검증을 숨기거나 통과로 확대하지 않았다.

## QA/검증 기록 확인

**충분함**

- QA/검증 에이전트 기록이 `verification.md`에 존재한다.
- 기획·승인 문서, 실제 상태·면역·변이 코드, asmdef 참조, 기술 샘플 완료 기록, 작업 패킷, 색인, Git 상태를 독립 대조했다.
- 초기 `소음 배관 → ImmuneSignalSuppression` 충돌을 발견하고 최종 브리프에서 `ContaminationExposure → WhiteBloodCellEvasion`으로 해소했음을 확인했다.
- 잠복 강화 `0.55`의 현재 적용 경계와 제안 변경을 구분했다.
- 사용자 로컬 `ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY`와 `_workspace/previews/`가 이번 작업 대상이 아님을 확인했다.
- 판정은 `승인 대기 전환 가능`이며, 남은 사용자 결정과 상태 문서 동기화 조건을 기록했다.

## MCP 플레이 체크 확인

**미실행이 타당함**

- 이번 작업은 승인 브리프·색인·작업 패킷·상태판을 다루는 문서 작업이다.
- Unity 코드·씬·테스트·ProjectSettings·패키지·아트를 변경하지 않았다.
- 따라서 Unity Play 동작이 달라졌다는 주장이 없고, Unity MCP Play·EditMode·빌드 실행은 이번 문서 정합 주장을 추가로 증명하지 않는다.
- 실제 1단계 구현 작업에서는 새 작업 패킷을 만들고 기존 Core/TechnicalSample2D 회귀, 신규 2D 테스트, Unity MCP Play, Console, Windows 빌드와 실행본 플레이를 단계 경계에 맞춰 수행해야 한다.

## 수정 필요

브리프 본문 수정은 필요하지 않다.

사용자 보고 전에 다음 상태 문서만 현재 사실로 동기화해야 한다.

1. `agent-activity.md`
   - QA/검증 에이전트 산출물과 `승인 대기 전환 가능` 판정 기록
   - 프로젝트 총괄 관리자 산출물과 본 판정 기록
2. `handoff.md`
   - `QA·총괄 검토 대기`를 `내부 승인 가능·사용자 승인 대기`로 변경
3. `_workspace/active/CURRENT.md`
   - `역할별 초안 작성 전`을 `승인 브리프·QA·총괄 검토 완료, 사용자 승인 대기`로 변경
4. `docs/project-handoff/current-task-board.md`
   - 현재 진행 상태와 로컬 작업 요약을 사용자 승인 대기 상태로 변경

## 문제 사안

- 브리프 내용의 차단 문제: 없음
- Unity 구현·검증 차단 문제: 이번 문서 작업에는 해당 없음
- 운영상 남은 조건: 위 네 상태 문서 동기화 전에는 완료·보관·커밋 보고를 하지 않는다.

## 사용자 결정 필요

브리프의 다음 추천안을 사용자가 승인하거나 수정해야 한다.

1. 첫 구현은 1단계까지만 진행한다.
2. 기술 샘플을 보존하고 별도 `RatHost2DPrototype.unity`를 만든다.
3. 기존 상태·테스트를 재사용하고 3D 결합부만 2D로 교체한다.
4. `BaseAlertPerSecond=0`을 유지하고 오염 위험 행동으로 자연 100%에 도달한다.
5. 2단계는 백혈구 회피형만 우선 이관하고 신호 억제형과 소음·강제 조종 면역 트리거를 보류한다.
6. 3단계에서 잠복 강화가 위험 행동 경계도 상승량에도 `0.55` 배율을 적용한다.
7. 기존 3D 씬과 2D 기술 샘플을 별도 정리 승인 전까지 보존한다.
8. 승인 뒤 실제 Unity 구현은 별도 작업 패킷으로 시작한다.

질문은 하나의 전체 추천안 승인 문구와 번호별 수정 승인 예시를 제공하므로 사용자가 승인 범위를 명확히 답할 수 있다.

## 사용자에게 올릴 확인 파일

- `docs/prototype/approvals/rat-host-2d-core-loop-migration-brief.md`
  - 첫 구현을 1단계로 제한할지
  - 별도 2D 프로토타입 씬과 상태 재사용 구조를 승인할지
  - 백혈구 회피 우선·신호 억제 보류를 승인할지
  - 잠복 강화의 위험 행동 상승량 `0.55` 적용을 승인할지

작업 이력과 역할별 검토안은 사용자 요청이 없는 한 별도 확인 목록으로 올릴 필요가 없다.

## 다음 단계

1. 메인 조정자가 QA와 본 총괄 판정을 활동 기록·핸드오프·CURRENT·현황판에 동기화한다.
2. 사용자에게 승인 브리프 한 파일과 핵심 결정 사항만 제시한다.
3. 사용자 승인 전에는 Unity 코드·씬·테스트·ProjectSettings·패키지·아트를 변경하지 않는다.
4. 승인 후 `1단계 2D 숙주·면역·100% 전환` 전용 구현 작업 패킷을 새로 만들고 구현·독립 QA·총괄 게이트를 다시 적용한다.
