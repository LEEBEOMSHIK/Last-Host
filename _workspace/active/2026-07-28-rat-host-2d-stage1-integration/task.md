# 작업 배정

## 작업 ID

`2026-07-28-rat-host-2d-stage1-integration`

## 작업명

1단계 2D 쥐 숙주·면역 경계도·자연 100% 전환 통합

## 작업영역

Unity C# 게임플레이 코드, EditMode 테스트, 별도 2D 씬·씬 빌더·HUD·Windows 임시 빌드 도구

## 담당 에이전트

- 게임플레이 구현: 게임플레이 구현 에이전트
- 씬·통합 구현: Unity 씬/통합 구현 에이전트
- 검증: QA/검증 에이전트
- 내부 승인: 프로젝트 총괄 관리자 에이전트
- 조정·통합: Codex 메인 에이전트

## 목적

사용자 수용을 받은 2D 기술 샘플 기반을 보존하면서, 별도 `RatHost2DPrototype` 씬에서 기존 숙주 본능/WASD 인계와 공용 상태·면역 경계도를 연결하고 오염 노출로 자연 100%에 도달해 내부 모드 전환 셸로 정확히 한 번 인계한다.

## 입력 자료

- `docs/prototype/approvals/rat-host-2d-core-loop-migration-brief.md`
- `docs/prototype/approvals/rat-host-approval-packet.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `docs/design/systems/immune-alert.md`
- `docs/design/hosts/host-instinct-control.md`
- `_workspace/completed/2026-07-27-2026-07-27-2d-playable-technical-sample/`
- 기존 `Core`, `Host`, `Immune`, `TechnicalSample2D` 코드와 테스트

## 구현 범위

1. 새 `Scripts/RatHost2D/` 어셈블리가 기존 `LastHost.Prototype`과 `LastHost.Prototype.TechnicalSample2D`를 참조한다.
2. `PrototypeSessionState` 인스턴스 하나를 소유하는 2D 세션 컨트롤러를 만든다.
3. 기존 숙주 본능 모델의 XZ 규칙을 XY에 연결하고, 무입력 본능 이동과 실제 WASD 인계를 2D Rigidbody2D 이동에 적용한다.
4. 2D 오염 구역이 `ContaminationExposure`, 경계도 `+12/초`, 숙주 생명력 `-4/초`를 적용한다.
5. 무위험 대기·일반 이동의 경계도 상승은 `0`을 유지한다.
6. 자연 100%에서 `InternalVirus / WhiteBloodCellEvasion` 전환이 한 번 발생하고 Host 입력·충돌·위험 판정·Host HUD를 비활성화한다.
7. 실제 내부 미니게임 대신 1단계 전환 검증 셸과 목적 안내를 표시한다.
8. 별도 `RatHost2DPrototype.unity`, 결정적 씬 빌더, 단계 전용 Windows 임시 빌드 명령을 만든다.
9. 기존 Core·3D·TechnicalSample2D와 신규 1단계 계약을 자동 테스트로 보호한다.

## 변경 허용 경로

- `UnityProject/Assets/_Project/Scripts/RatHost2D/**`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHost2D/**`
- `UnityProject/Assets/_Project/Editor/RatHost2D/**`
- `UnityProject/Assets/_Project/Scenes/RatHost2DPrototype.unity(.meta)`
- 승인·구현 계획·상태판과 현재 작업 패킷

기존 기술 샘플 아트·입력 에셋은 읽기·참조 재사용할 수 있지만 수정하지 않는다.

## 금지 범위

- 기존 `RatHostPrototype.unity`와 `RatHost2DTechnicalSample.unity` 수정·삭제
- `Scripts/TechnicalSample2D/**`와 전용 테스트 수정
- 실제 바이러스·백혈구·변이 조각 플레이 구현
- 성공·실패·변이 선택·복귀 구현
- 소음·강제 조종 면역 트리거와 면역 신호 억제 2D 경로
- 시간 자동 면역 상승 재활성화
- 최종 아트·최종 PPU·타일·화면 규격 확정
- 패키지, Unity 버전, URP, ProjectSettings, Build Settings 변경
- 사용자 `APP_UI_EDITOR_ONLY`, `_workspace/previews/`, 저장소 `Builds/` 변경

## 수용 기준

- 새 씬은 기존 두 씬과 별개이며 런타임 상태 인스턴스가 하나다.
- 기존 `Host/Move` InputAction을 사용한다.
- 무입력 본능 이동과 WASD 이동에서 root·Visual·카메라 분리, 순간이동, 소품 관통이 없다.
- 활성 WASD 방향은 화면 입력 방향과 일치하고 대각선 속도가 정규화된다.
- 오염 구역 바깥에서는 경계도·생명력이 변하지 않고 안에서는 승인 시험값을 적용한다.
- `ContaminationExposure`가 `WhiteBloodCellEvasion`을 선택한다.
- 100% 전환은 한 번이며 전환 뒤 Host 상태 추가 변경이 없다.
- Host HUD에 생명력, 경계도, 모드, 오염 원인 피드백이 보인다.
- 내부 셸에는 1단계 기술 통합이며 실제 미니게임이 아님이 표시된다.
- 전체 EditMode, 신규 테스트, Unity MCP Play, Console, Windows 임시 빌드가 검증된다.
- 기존 3D 씬·기술 샘플·사용자 로컬 변경이 보존된다.

## 완료 경계

1단계는 전체 핵심 루프 완료가 아니다. 사용자 수동 플레이에서 이동·본능 인계·오염 노출·자연 100% 전환을 확인한 뒤에만 완료·보관 후보가 된다.
