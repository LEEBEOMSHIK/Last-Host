# QA/검증 기록

## 검증 대상

- 작업 ID: `2026-07-27-rat-core-loop-2d-migration-brief`
- 완료 주장: 사용자 수용을 받은 2D 기술 샘플 이후, 쥐 숙주 핵심 루프를 세 단계로 이관하기 위한 범위·구조·회귀 게이트와 사용자 승인 질문이 현재 기획·코드·저장소 사실에 맞게 정리됐다.
- 주 산출물: `docs/prototype/approvals/rat-host-2d-core-loop-migration-brief.md`
- 검증 역할: QA/검증 에이전트
- 검증일: 2026-07-27 KST

## 대조 기준

- `AGENTS.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `docs/prototype/approvals/rat-host-approval-packet.md`
- `docs/design/systems/immune-alert.md`
- `docs/design/hosts/host-instinct-control.md`
- `docs/agents/loop-engineering-gates.md`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeConfig.cs`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionState.cs`
- `UnityProject/Assets/_Project/Scripts/Immune/ImmuneAlertModel.cs`
- `UnityProject/Assets/_Project/Scripts/Immune/ImmuneRiskZone.cs`
- `UnityProject/Assets/_Project/Scripts/Mutations/MutationLoadout.cs`
- `UnityProject/Assets/_Project/Scripts/Host/RatHostControlModel.cs`
- `UnityProject/Assets/_Project/Scripts/TechnicalSample2D/`
- 기존 Core·TechnicalSample2D runtime/tests `.asmdef`
- `_workspace/completed/2026-07-27-2026-07-27-2d-playable-technical-sample/`
- 현재 작업 패킷 전체
- `docs/prototype/README.md`
- `_workspace/active/CURRENT.md`
- `docs/project-handoff/current-task-board.md`
- 실제 Git 상태

## 실행한 검증

### 1. 문서 범위·방향 대조

명령:

```text
Get-Content AGENTS.md
Get-Content docs/prototype/official/rat-host-prototype.md
Get-Content docs/prototype/plans/rat-host-implementation-plan.md
Get-Content docs/prototype/approvals/rat-host-approval-packet.md
Get-Content docs/prototype/approvals/rat-host-2d-core-loop-migration-brief.md
Get-Content docs/design/systems/immune-alert.md
Get-Content docs/design/hosts/host-instinct-control.md
```

결과:

- 현재 기준은 2026-07-27 승인된 `2D 아이소메트릭/쿼터뷰 도트 게임`이며 브리프도 같은 방향을 사용한다.
- 과거 3D 환경·저폴리 원본 기반 2.5D 방향은 신규 제작 기준에서 제외하고 레거시로 보존한다.
- 1단계 `숙주·면역·100% 전환`, 2단계 `백혈구 회피 미니게임·성공/실패`, 3단계 `변이 선택·효과·쥐 복귀`가 독립 포함·제외·수용 기준으로 구분돼 있다.
- 벌레 튜토리얼, 다중 숙주, 인간 숙주, 병원·연구소·백신, 엔딩, 영구 성장, 모바일, 최종 아트는 계속 제외돼 있다.
- 기술 샘플 시험값과 플레이스홀더를 최종 PPU·타일·해상도·프레임·아트 규격으로 승격하지 않는다.

해석:

- 현재 프로젝트 방향, 공식 쥐 숙주 수직 슬라이스와 승인 범위를 벗어나지 않는다.

### 2. 면역 경계도·자연 100% 사실 대조

명령:

```text
Get-Content UnityProject/Assets/_Project/Scripts/Core/PrototypeConfig.cs
Get-Content UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionState.cs
Get-Content UnityProject/Assets/_Project/Scripts/Immune/ImmuneAlertModel.cs
Get-Content UnityProject/Assets/_Project/Scripts/Immune/ImmuneRiskZone.cs
rg -n "BaseAlertPerSecond|ContaminationExposure|NoiseOrTissueIrritation|ForcedHostControl|AddRiskAlert|AddImmuneAlertAmount"
```

결과:

- 실제 기본값은 `PrototypeConfig.BaseAlertPerSecond = 0f`다.
- `TickRatMode`가 시간 틱을 호출해도 기본값 0에서는 무위험 대기·일반 이동으로 경계도가 증가하지 않는다.
- `ImmuneRiskZone`의 현재 시험값은 경계도 `12/초`, 숙주 생명력 피해 `4/초`이고 `ContaminationExposure` 원인으로 `AddImmuneAlertAmount`를 호출한다.
- `ContaminationExposure`는 `WhiteBloodCellEvasion`으로 라우팅된다.
- `NoiseOrTissueIrritation`과 `ForcedHostControl`은 현재 `ImmuneSignalSuppression`으로 라우팅된다.
- 브리프의 1단계 기본 위험은 수정 후 `ContaminationExposure` 2D 오염 구역 1종이며, 오염 구역에서 위험 행동 중심 자연 100%를 만든다.
- 브리프는 소음 배관·강제 조종 면역 트리거를 신호 억제형 이관 또는 라우팅 변경의 별도 승인 전까지 2D 최소 이관에서 노출하지 않는다.

해석:

- `BaseAlertPerSecond=0`, 위험 행동 중심 자연 100%, 백혈구 회피형 1종 우선 이관이 실제 라우팅과 충돌하지 않게 정리됐다.
- 검토 중 발견됐던 `소음 배관 → ImmuneSignalSuppression`과 `2단계 신호 억제 보류`의 충돌은 최종 브리프에서 해소됐다.

### 3. 잠복 강화 정합 대조

명령:

```text
Get-Content UnityProject/Assets/_Project/Scripts/Mutations/MutationLoadout.cs
Get-Content UnityProject/Assets/_Project/Scripts/Immune/ImmuneAlertModel.cs
Select-String PrototypeSessionState.cs -Pattern "ImmuneAlertRateMultiplier|AddRiskEvent|AddImmuneAlertAmount"
```

결과:

- `MutationLoadout.ImmuneAlertRateMultiplier`는 잠복 강화 보유 시 `0.55f`다.
- 현재 배율은 `PrototypeSessionState.TickRatMode → ImmuneAlert.Tick`의 시간 상승에만 전달된다.
- `ImmuneAlert.AddRiskEvent`와 `AddRawAmount`에는 잠복 강화 배율이 자동 적용되지 않는다.
- 기본 시간 상승이 0이므로 현 상태 그대로라면 잠복 강화의 면역 상승 억제 효과를 체감하기 어렵다.
- 브리프는 시간 자동 상승을 다시 켜지 않고 3단계에서 위험 행동 경계도 상승량에도 `0.55`를 적용하는 변경을 별도 사용자 승인 항목으로 제시한다.

해석:

- 현재 구현과 승인된 “면역 경계도 상승 억제” 의도의 차이가 숨겨지지 않았고, 동작 변경을 승인 없이 확정하지 않았다.
- 구현 시 `AddRiskAlert`뿐 아니라 1단계 기본 오염 구역이 사용하는 `AddImmuneAlertAmount` 경로에도 승인된 배율 규칙이 적용되는지 테스트해야 한다.

### 4. 재사용·교체 클래스와 어셈블리 사실 대조

명령:

```text
Get-Content PrototypeSessionState.cs
Get-Content RatHostControlModel.cs
Get-Content Movement2DModel.cs
Get-Content RatHost2DController.cs
Get-Content PixelFollowCamera2D.cs
Get-Content YSortSprite2D.cs
Get-Content LastHost.Prototype.asmdef
Get-Content LastHost.Prototype.TechnicalSample2D.asmdef
Get-Content 기존 두 EditMode 테스트 asmdef
```

결과:

- `PrototypeSessionState`, `PrototypeConfig`, `PrototypeGameMode`, `ImmuneAlertModel`, `VirusMinigameModel`, `MutationLoadout`은 기존 `LastHost.Prototype` 어셈블리에 있고 공개 타입으로 참조 가능하다.
- `PrototypeSessionState`는 UnityEngine 수학 타입을 사용하므로 완전한 엔진 독립 Domain은 아니지만 3D 씬·Collider에 직접 묶인 MonoBehaviour는 아니며 현재 단계의 참조 재사용 대상이라는 표현은 사실에 맞다.
- `RatHostControlModel`은 `Vector3` XZ 평면 규칙이므로 브리프의 XY↔XZ 어댑터 전제가 필요하다.
- `Movement2DModel`, `RatHost2DController`, `PixelFollowCamera2D`, `YSortSprite2D`는 `LastHost.Prototype.TechnicalSample2D` 어셈블리의 공개 타입이다.
- 기존 `LastHost.Prototype`과 `LastHost.Prototype.TechnicalSample2D`는 서로 참조하지 않는다. 신규 2D 조립 어셈블리가 두 어셈블리를 참조하는 추천은 현재 구조에서 순환 참조를 만들지 않는다.
- 브리프는 3D `CharacterController`·3D 충돌·카메라·표시 결합부를 2D 물리·카메라·SpriteRenderer/Y 정렬로 교체하고, 대규모 Domain 추출은 후속으로 미룬다.

해석:

- 재사용·교체 후보와 어셈블리 추천은 실제 클래스·참조 구조와 일치한다.
- 구현 단계에서 `차원 독립`을 `UnityEngine 비의존` 또는 “코드 수정 없이 무조건 재사용”으로 확대 해석하면 안 된다.

### 5. 기술 샘플·기존 3D 보존 대조

명령:

```text
Get-Content _workspace/completed/2026-07-27-2026-07-27-2d-playable-technical-sample/completion-report.md
Get-Content _workspace/completed/2026-07-27-2026-07-27-2d-playable-technical-sample/verification.md
Test-Path UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity
Test-Path UnityProject/Assets/_Project/Scenes/RatHost2DTechnicalSample.unity
```

결과:

- 완료 기록은 2D 기술 샘플의 이동·충돌·카메라·Y 정렬·HUD와 Windows 임시 빌드 생성을 검증했다.
- 기술 샘플의 Windows 실행본 플레이는 미검증으로 남았으며, 브리프도 이를 Windows 플레이 통과로 확대하지 않는다.
- 기술 샘플은 면역·모드 전환·내부 미니게임·변이 전체 루프 완료가 아니다.
- 브리프는 `RatHost2DTechnicalSample.unity`를 고정 회귀 기준으로 보존하고, 별도 `RatHost2DPrototype.unity`를 추천한다.
- 기존 `RatHostPrototype.unity`와 3D/2.5D/Blender 산출물은 전체 2D 루프 수용 뒤에도 별도 정리 승인 전 자동 삭제하지 않는다.

해석:

- 기술 샘플의 승인 범위를 과장하지 않고 기존 3D 회귀 기준을 보호한다.

### 6. 작업 패킷·색인·상태판·Git 대조

명령:

```text
rg --files _workspace/active/2026-07-27-rat-core-loop-2d-migration-brief
Get-Content docs/prototype/README.md
Get-Content _workspace/active/CURRENT.md
Get-Content docs/project-handoff/current-task-board.md
git status --short
git log -3 --oneline --decorate
git rev-parse HEAD
git rev-parse origin/main
git diff --name-only
git diff -- UnityProject/ProjectSettings/ProjectSettings.asset
```

결과:

- 작업 패킷의 `task.md`, `work-log.md`, `agent-activity.md`, `handoff.md`와 역할별 검토안 3개가 존재한다.
- `docs/prototype/README.md`에 승인 브리프 경로와 확인 순서가 추가돼 있다.
- 활성 작업 경로는 `_workspace/active/2026-07-27-rat-core-loop-2d-migration-brief/`로 실제 존재하며 상태판 경로와 일치한다.
- 완료된 기술 샘플 경로 `_workspace/completed/2026-07-27-2026-07-27-2d-playable-technical-sample/`가 실제 존재한다.
- 현재 작업과 차단 중인 자연 경계도 엄격 검증은 서로 다른 항목이며, 다음 후보·보류에 동일 작업이 중복 기재되지 않았다.
- `HEAD = origin/main = 0dea64e6d1e77288c88533f9f58d19fdd04fed1d`이고 상태판의 `0dea64e docs: sync 2d sample commit state`와 일치한다.
- 이번 작업의 추적·미추적 변경은 승인 브리프, 프로토타입 색인, 현재 상태 문서와 작업 패킷이다.
- `UnityProject/ProjectSettings/ProjectSettings.asset`의 유일한 diff는 사용자 로컬 `Standalone: SENTIS_ANALYTICS_ENABLED;APP_UI_EDITOR_ONLY`다.
- `_workspace/previews/`는 사용자 로컬 untracked 상태다.
- 이번 문서 작업에서 Unity 코드·씬·테스트·패키지·아트 변경은 확인되지 않았다.

해석:

- 경로, Git 기준, 사용자 로컬 변경 제외는 사실과 일치한다.
- `_workspace/active/CURRENT.md`, `current-task-board.md`, `agent-activity.md`, `handoff.md`의 상태 문구는 아직 `역할별 초안 작성/QA 전` 단계다. 이는 본 QA 직전 기록으로 이해 가능하지만, 사용자 승인 대기로 전환하기 전에 QA 결과와 현재 상태로 동기화해야 한다.

## 테스트·Play·빌드 판단

### 실행하지 않은 검증

- Unity EditMode/PlayMode 테스트
- Unity MCP Play
- Unity Console 확인
- Windows 빌드
- Windows 실행본 플레이

### 실행하지 않은 이유

- 이번 작업은 승인 브리프·색인·작업 패킷·상태판만 변경한 문서 작업이다.
- `git status`, `git diff --name-only`와 실제 diff 대조에서 Unity 코드·씬·테스트·ProjectSettings의 작업 변경, 패키지·아트 변경이 없다.
- 기존 사용자 `ProjectSettings.asset` 로컬 변경은 이번 작업 범위가 아니며 수정·스테이징 대상에서 제외돼 있다.
- 따라서 Unity 동작 결과가 달라질 변경이 없고, EditMode/PlayMode/MCP Play/빌드를 다시 실행해도 이번 문서 주장을 추가로 증명하지 않는다.
- 실제 1단계 구현 작업에서는 브리프의 단계별 공통 게이트에 따라 기존 Core/TechnicalSample2D 회귀 테스트, 신규 2D 테스트, MCP Play, 단계별 Windows 빌드를 수행해야 한다.

## 발견 사항

### 해소된 중요 불일치

문제:

- 초기 통합안은 1단계 기본 위험으로 소음 배관을 사용했지만 `NoiseOrTissueIrritation`은 실제 코드에서 `ImmuneSignalSuppression`으로 라우팅된다.
- 동시에 2단계 추천은 신호 억제형을 보류해 백혈구 회피형 1종만 이관하는 것이어서, 수정 전에는 자연 100% 이후 미구현 타입으로 진입할 수 있었다.

수정 확인:

- 최종 브리프의 1단계 기본 위험이 `ContaminationExposure` 2D 오염 구역으로 변경됐다.
- 실제 현재 시험값 `경계도 +12/초`, `숙주 생명력 -4/초`가 명시됐다.
- `ContaminationExposure → WhiteBloodCellEvasion` 라우팅을 수용 기준으로 고정했다.
- 소음 배관·강제 조종 면역 트리거는 신호 억제 이관 또는 별도 라우팅 승인 전 2D 최소 이관에서 노출하지 않는다.

판정:

- 해소됨. 사용자 승인 대기 전환을 막지 않는다.

### 상태 문서 동기화 필요

문제:

- `CURRENT.md`, 현황판, `agent-activity.md`, `handoff.md`가 본 QA 완료 전 상태 문구를 유지한다.

영향:

- 그대로 사용자 승인 대기 또는 커밋 단계로 넘어가면 루프 엔지니어링 상태판 동기화 게이트와 맞지 않는다.

추천:

- 메인 조정자가 본 `verification.md`의 판정을 기록하고, QA 완료·총괄 검토 단계 또는 승인 대기 상태로 네 문서를 동기화한다.

판정:

- 현 시점에서는 정상적인 다음 조정 절차이며 브리프 내용 수정 사유는 아니다.
- 동기화하지 않은 채 완료·보관·커밋하면 차단 조건이 된다.

## 남은 위험과 사용자 결정

- 1단계 구현 범위와 별도 `RatHost2DPrototype` 씬 생성은 아직 사용자 승인 전이다.
- `BaseAlertPerSecond=0` 유지와 위험 행동 중심 자연 100%를 사용자가 승인해야 한다.
- 백혈구 회피형만 우선 이관하고 신호 억제형을 보류하는 범위가 사용자 승인 전이다.
- 잠복 강화 `0.55`를 위험 행동 경계도 상승에도 적용하는 동작 변경은 사용자 승인 전이다.
- 실제 구현에서는 `AddRiskAlert`와 `AddImmuneAlertAmount` 두 경로의 잠복 강화 적용 여부를 명시적으로 테스트해야 한다.
- 기술 샘플의 Windows 실행 플레이 미검증은 해당 샘플 기록의 잔여 위험이다. 1단계 구현 검증에서 단계별 Windows 빌드와 실행 플레이를 분리 기록해야 한다.
- 최종 PPU·타일·내부 해상도·프레임·아트 규격은 승인되지 않았다.
- 초기 QA 시점에는 총괄 관리자 판정이 남아 있었으나, 이후 `director-review.md`에 `내부 승인 가능 — 사용자 승인 대기`가 기록됐다.

## 완료 판단

**승인 대기 전환 가능**

- 브리프 내용은 현재 2D 방향, 쥐 숙주 프로토타입 범위, 실제 상태·라우팅·클래스·어셈블리 구조, 기술 샘플과 3D 레거시 보존 원칙에 정합한다.
- 중요 라우팅 충돌과 잠복 강화 체감 문제를 숨기지 않고 각각 수정·사용자 승인 항목으로 분리했다.
- Unity 구현 변경이 없는 문서 작업이므로 테스트·MCP Play·빌드 미실행은 타당하다.
- 초기 QA 판정 시 후속 조건은 본 결과를 `agent-activity.md`, `handoff.md`, `_workspace/active/CURRENT.md`, `current-task-board.md`에 반영하고 총괄 관리자 판정을 받는 것이었다. 아래 최종 대조에서 해당 조건의 충족을 확인했다.

## 상태판 동기화 최종 대조

### 대조 대상

- `_workspace/active/2026-07-27-rat-core-loop-2d-migration-brief/agent-activity.md`
- `_workspace/active/2026-07-27-rat-core-loop-2d-migration-brief/handoff.md`
- `_workspace/active/CURRENT.md`
- `docs/project-handoff/current-task-board.md`
- `_workspace/active/2026-07-27-rat-core-loop-2d-migration-brief/verification.md`
- `_workspace/active/2026-07-27-rat-core-loop-2d-migration-brief/director-review.md`
- `docs/prototype/approvals/rat-host-2d-core-loop-migration-brief.md`
- 실제 Git 상태

### 실행한 확인

명령:

```text
Get-Content agent-activity.md
Get-Content handoff.md
Get-Content _workspace/active/CURRENT.md
Get-Content docs/project-handoff/current-task-board.md
Get-Content director-review.md
Test-Path docs/prototype/approvals/rat-host-2d-core-loop-migration-brief.md
git status --short
git rev-parse HEAD
git rev-parse origin/main
```

결과:

- `agent-activity.md`는 QA 산출물 `verification.md`와 판정 `승인 대기 전환 가능`을 기록한다.
- `agent-activity.md`는 총괄 산출물 `director-review.md`와 판정 `내부 승인 가능 — 사용자 승인 대기`를 기록한다.
- `handoff.md`는 현재 상태를 `승인 대기 — QA 승인 대기 전환 가능, 총괄 내부 승인 가능`으로 기록하고, 사용자 승인 후에만 구현 계획 반영과 1단계 구현 패킷 생성을 진행하도록 제한한다.
- `_workspace/active/CURRENT.md`는 동일 작업 ID, 실제 active 경로, QA·총괄 완료와 사용자 승인 대기 상태를 가리킨다.
- `current-task-board.md`의 현재 로컬 작업과 진행 중 표는 QA `승인 대기 전환 가능`, 총괄 `내부 승인 가능 — 사용자 승인 대기`와 일치한다.
- 승인 브리프는 `docs/prototype/approvals/rat-host-2d-core-loop-migration-brief.md`에 실제 존재한다.
- 다음 작업 후보는 현재 브리프 자체가 아니라, 사용자 승인 뒤 별도 패킷으로 시작할 `1단계 2D 쥐 숙주·면역 경계도·100% 모드 전환` 구현이다.
- 현재 작업인 승인 브리프, 다음 후보인 1단계 구현, 차단 중인 자연 경계도 Windows 엄격 검증, 보류 중인 사용자 수동 플레이 체감 확인은 서로 다른 작업이다. current/next/hold 중 동일 작업 중복은 없다.
- `HEAD`와 `origin/main`은 모두 `0dea64e6d1e77288c88533f9f58d19fdd04fed1d`이며 상태 문서의 `0dea64e docs: sync 2d sample commit state`와 일치한다.
- `git status --short`에서 `UnityProject/ProjectSettings/ProjectSettings.asset`의 사용자 로컬 변경은 unstaged로 유지된다.
- `_workspace/previews/`는 사용자 로컬 untracked 상태로 유지된다.
- 상태 문서는 두 경로를 이번 승인 브리프·후속 구현의 변경 대상과 커밋 대상에서 제외하도록 명시한다.
- Unity 코드·씬·테스트·패키지·아트의 이번 작업 변경은 없다.

### 최종 상태판 판정

**통과 — 승인 대기 상태 동기화 완료**

- 앞선 QA에서 후속 게이트로 남겼던 `agent-activity.md`, `handoff.md`, `_workspace/active/CURRENT.md`, `current-task-board.md`의 상태 동기화가 완료됐다.
- QA 판정, 총괄 판정, active 경로, Git 기준, 다음 후보, 사용자 로컬 제외 상태가 실제 파일·저장소 상태와 일치한다.
- 브리프는 사용자에게 승인 요청할 수 있다.
- 사용자 승인 전에는 작업을 완료 보관하거나 실제 Unity 1단계 구현을 시작하지 않는다.

## 최종 완료 판단

**승인 대기 전환 가능**

- 문서 내용과 상태판 동기화의 추가 차단 문제는 없다.
- 사용자 승인 결과를 받은 뒤에만 승인 패킷·구현 계획 반영과 별도 1단계 구현 작업으로 전환한다.
