# 에이전트 수행 이력

## 작업 ID

`2026-07-20-rat-walk-unity-visual-trial`

## 참여 에이전트 요약

| 에이전트 | 역할 | 담당 업무 | 산출물 | 판정 |
| --- | --- | --- | --- | --- |
| 프로젝트 조정 | 배정·통합 | 작업 패킷·게이트 관리 | 본 작업 패킷 | 검증 진행 중 |
| 게임플레이 구현 | 코드 | 방향별 걷기 프레임 표시 | `RatDirectionalSpriteView.cs`, EditMode 계약 테스트, `handoff.md` | 구현 완료 / Unity 실행 검증 대기 |
| Unity 씬/통합 | 에셋·씬 | v2 스프라이트 시험 반입·연결 | `WalkTrialV2` PNG 64장, Unity Importer·씬 연결 기록 | 통합 완료 / 독립 QA 대기 |
| QA/검증 | 검증 | Play·콘솔·시각 증적, Unity MCP 차단 독립 확인 | `verification.md` | 핵심 Play 통과 / QA 미승인 |
| 프로젝트 총괄 | 승인 | 범위·QA 기록 판정 | 대기 | 대기 |

## 위임 기록

| 시각 | 위임한 에이전트 | 받은 에이전트 | 요청한 일 | 결과 | 산출물 |
| --- | --- | --- | --- | --- | --- |
| 2026-07-20 | 프로젝트 조정 | 게임플레이 구현·Unity 씬/통합 | 실제 게임 화면 v2 걷기 시험 구현 | 진행 중 | 작업 패킷 |
| 2026-07-20 | 프로젝트 조정 | 게임플레이 구현 | 기존 이동·충돌을 보존한 8방향×8프레임 런타임 표시 계약 | 완료 | 코드·테스트·통합 인계 |
| 2026-07-20 | 프로젝트 조정 | Unity 씬/통합 | v2 PNG를 격리 시험 경로로 반입하고 씬 연결 | PNG 64장 반입 완료, MCP 철회로 Unity 연결 보류 | `WalkTrialV2/`, `work-log.md`, `handoff.md` |
| 2026-07-20 | 프로젝트 조정 | QA/검증 | Unity MCP 차단 독립 확인 및 실제 화면 시험 가능 여부 판정 | `GetUserGuidelines`·`GetState`·콘솔 조회 모두 `Connection revoked`; PNG 64장 SHA-256·규격 보존만 통과 | `verification.md` |
| 2026-07-20 | 프로젝트 조정 | Codex 메인 에이전트(예외 수행) | 사용자 `진행해` 지시에 따라 MCP 복구 뒤 승인된 시험 범위의 Importer·씬 연결과 핵심 Play 검증 재개 | 64 Sprite Importer·8방향 배열·8fps 저장, MCP Play·콘솔·카메라 렌더 통과. 다중 에이전트 배정 제한으로 예외 수행, 독립 QA·총괄 판정은 미완료 | `work-log.md`, `verification.md` |
| 2026-07-20 | 프로젝트 조정 | Codex 메인 에이전트(예외 수행) | 사용자 승인에 따른 쥐 보행 화면 점유율·가독성 보정 | `RatVisual`을 1.35배 확대하고 8방향 접지 오프셋을 비례 보정했다. Play의 8방향·8fps·접지·콘솔·카메라 렌더를 재통과했으며, 다중 에이전트 배정 제한으로 예외 수행 | `work-log.md`, `verification.md` |
| 2026-07-20 | 프로젝트 조정 | Blender 애니메이션 테크아트 에이전트 | 사용자 정정에 따른 v3 보행 동작 폭 확대 | v2 보존, 별도 v3 원본·64 PNG 출력·루프·접지 검증을 배정. Unity 반입 전 산출물 검토 대기 | Blender 작업 패킷, v3 핸드오프 예정 |
| 2026-07-20 | Blender 애니메이션 테크아트 | Blender 애니메이션 테크아트 에이전트 | v3 과장 보행 원본·64 PNG 제작과 Blender 검증 | 스윙 발 전진·들림, 대각 접지쌍, 몸통·꼬리 리듬을 키운 별도 v3 원본·64 PNG를 제작. min Z=0, root fcurve 0, 카메라·조명 v2 동일 확인 | `_workspace/active/2026-07-20-rat-walk-animation-blender/artifacts/blender-animation-v2-handoff.md` |
| 2026-07-20 | 프로젝트 조정 | Codex 메인 에이전트(예외 수행) | 사용자 승인에 따른 v3 Unity 시험 반입·연결·Play 확인 | 원본/반입본 SHA-256 64/64, Importer 64/64, v3 배열·8fps, Play 8방향·접지·콘솔·카메라 렌더 통과. 다중 에이전트 배정 제한으로 예외 수행 | `work-log.md`, `verification.md` |
| 2026-07-21 | 프로젝트 조정 | Unity 씬/통합 구현 | 사용자 피드백(뒤로 걷는 인상·프레임별 화질 흔들림)에 따른 기본 시점 조사·정정 | 씬과 재생성기의 시작 시점을 `QuarterView`로 일치시켰다. V 키 순환은 코드상 활성이나 이번 범위에서는 비활성화하지 않았다. 활성 씬이 dirty여서 Save·재로드·Play는 미실행이며, 깨끗한 재로드 후 QA가 필요하다. | `RatHostPrototype.unity`, `RatHostPrototypeSceneBuilder.cs`, `work-log.md`, `handoff.md` |
| 2026-07-21 | 프로젝트 조정 | Unity 씬/통합 구현 | QA Play 종료 후, dirty 활성 씬 카메라의 즉시 쿼터뷰 적용 | 저장·재로드 없이 `IsometricCamera`의 `PrototypeCameraController` 인스턴스만 `QuarterView`로 설정하고 `ApplyCameraNow(RatHost)`를 실행했다. 결과 `starting=QuarterView`, `current=QuarterView`, `orthographic=True`; QA 재검증 가능. | `work-log.md`, `handoff.md` |
| 2026-07-21 | 게임플레이 구현 | 사용자 피드백(뒤로 걷는 인상·화질 변동) 코드 범위 조사·정정 | Unity 실제 오른쪽 이동이 `E` 프레임을 선택했지만 Blender 프리렌더 수평 축과 Unity `camera.right`의 좌우 기준이 반대여서 화면상 반대 방향을 향함을 재현했다. 시각용 right 축만 반전했고 이동·충돌·루트·카메라는 미변경이다. | `RatDirectionalSpriteView.cs`, EditMode 계약 갱신, `work-log.md`, `handoff.md`, `verification.md` |
| 2026-07-21 | 게임플레이 구현 | 정지 TrialV1 ↔ 이동 WalkTrialV3 전환 최소 정정 | 8방향 v3 배열이 완전하면 정지에도 현재 방향 v3 f01을 표시하고, 하나라도 불완전하면 기존 TrialV1 정지 Sprite fallback을 유지하도록 코드·계약을 갱신했다. | `RatDirectionalSpriteView.cs`, `RatHostPrototypeCoreTests.cs`, 작업 기록 |

## QA 재개 판정

- Unity MCP 연결은 정상화됐고, 통합과 핵심 Play 검증이 재개됐다.
- 대상 EditMode 테스트 실행 결과와 독립 QA·총괄 판정이 남아 있으므로 현재 판정은 `QA 미승인`이다.

## 2026-07-21 — 독립 QA 회귀 검증 기록

| 에이전트 | 역할 | 검증 범위 | 실행 결과 | 판정 |
| --- | --- | --- | --- | --- |
| 독립 QA/검증 | Unity Play 회귀 검증 | 수평 방향, v3 정지·이동 세트, 기본 카메라, 접지, 콘솔 | dirty 씬을 저장·재로드 없이 Play/Pause/Stop. 8방향 v3 `f04` 이동과 같은 방향 `f01` 정지, 우측 이동의 `West` 보정, 접지 `0.0050`, Console Error/Warning 0 통과. 실제 시작 카메라는 `ThirdPerson`/원근으로 확인되어 QuarterView 요구 실패. | **미승인** |

| 독립 QA/검증 | QuarterView 재검증 | 활성 메모리 카메라 보정 뒤 Play 회귀 | 저장·재로드 없이 재검증. `startingHostMode`·`CurrentHostMode=QuarterView`, `orthographic=True`, 우측 이동 `West`의 v3 `f01→f04→f01`, 접지 `0.0050`, Console Error/Warning 0, MainCamera Capture 통과. | **이번 회귀 범위 통과** |
