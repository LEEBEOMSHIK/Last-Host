# 작업 배정서

## 기본 정보

- 작업 ID: `2026-08-05-rat-collision-surface-slide`
- 작업명: 쥐 대각선 충돌 표면 미끄러짐 교정
- 상태: 완료 보관 — 사용자 수용 반영
- 생성일: 2026-08-05
- 담당 에이전트: 프로젝트 조정 에이전트
- 보조 에이전트: 게임플레이 구현 에이전트, QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `rat-host-loop-builder`, `unity-verification-runner`

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| 프로젝트 조정 에이전트 | 조정·통합 | 범위, S0, 비용, 상태 동기화 | 작업 패킷과 통합 보고 |
| QA/검증 에이전트 | 독립 검증 | 구현 전 S0 검토, 구현 후 원증상·표적 회귀·MCP 가능 범위 검증 | `verification.md` |
| 게임플레이 구현 에이전트 | production owner | `RatHost2DController` 충돌 이동과 직접 관련 테스트 | 최소 코드·테스트 변경 |
| 프로젝트 총괄 관리자 에이전트 | 최종 내부 판정 | QA 충분성, 범위, 사용자 수용 대기 감사 | `director-review.md` |

## 구현 담당 확인

- 코드/테스트 변경 담당: 게임플레이 구현 에이전트
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: 없음. 필요해지면 구현 중지 후 재분류
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 해당 없음

## 루프 게이트

- 게이트 적용 대상: 예
- 위험 등급: R2
- 위험 등급 근거: 사용자 가시 조작 결함이며 Rigidbody2D 충돌 응답과 접선 이동 불변식을 바꾼다.
- 적용 사유: 자연 부분 가림 후보의 실제 WASD 수용 중 발견된 이동·물리 결함
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예
- correction cycle: 최신 성공 재분류 후 run005/run006 연속 실패 2회, 다음 실행 전 재분류 필수
- capability profile / 요청 route: 정적·표적 EditMode 우선, MCP Play가 필요하면 `unity_mcp`
- attempt ledger 경로 / 같은 criterion 연속 실패 수: `artifacts/verification-attempt-ledger.json` / 최신 재분류 후 2(run005, run006 product)

## S0 사용자 원증상·검증 charter

- 사용자 원문 또는 원증상: 오브젝트 뒤쪽 아래 경계와 충돌한 상태에서 대각선 이동을 입력하면 보행 모션만 재생되고 쥐가 움직이지 않는다. 자연스럽게 경계를 따라 미끄러져 지나가야 한다.
- 재현 씬·입력·좌표·상태: `RatHost2DTechnicalSample`에서 쥐를 벽·통·상자 아래/뒤 경계에 접촉시킨 뒤 충돌면 안쪽 성분과 좌우 접선 성분을 함께 가진 대각선 WASD를 유지한다.
- 원증상 증거: 사용자 실제 플레이 acceptance 피드백. 현재 `RatHost2DController.ClampStepToCollision`은 요청 대각선 전체를 단일 cast 거리로 축소한다.
- 합성 oracle의 금지 결과: 입력·보행만 유지되고 위치가 고정됨, collider 관통, 과대 collider로 접근 차단, teleport/clamp, 접촉 시 renderer/입력 비활성화, 축 순서에 따른 반대 방향 비대칭.
- 합성 oracle의 허용 결과: 공용 충돌 모터가 쥐와 바이러스 모두에서 충돌 법선 방향 성분만 차단하고 남은 접선 성분으로 경계를 따라 연속 이동한다. 양쪽 축이 모두 막힌 실제 코너에서는 정지한다. 바이러스의 상태 전이·입력 배타·무충돌 속도는 그대로 유지한다.
- 완료 주장 한 문장: 쥐가 벽·통·상자에 대각선으로 밀어붙일 때 관통 없이 충돌면을 따라 미끄러지고, 정면 충돌과 실제 코너 정지는 유지된다.

| criterion ID | 유형 | 입력·상태 | 기대값 | 최소 검증 |
| --- | --- | --- | --- | --- |
| C1 | 원증상/성공 | 평평한 아래 경계 접촉 후 대각선 입력 | 법선 이동은 skin 이내, 접선 위치는 프레임마다 진행 | Rigidbody2D 합성 EditMode |
| C2 | negative control | 경계에 정면 입력 | 관통 없이 기존처럼 정지 | 기존 E08 + 신규 표적 |
| C3 | 경계 | 실제 90도 코너로 대각선 입력 | 두 축이 막히면 정지, 떨림·순간이동 없음 | 합성 코너 EditMode |
| C4 | 대칭 | 좌·우 대각선 각각 동일 접촉 조건 | 접선 진행량 절댓값 대칭, 방향만 반대 | pairwise 표적 테스트 |
| C5 | 수명주기 | 무충돌 이동과 0 입력 | 기존 속도·대각선 정규화·정지 유지 | 기존 movement 관련 테스트 |
| C6 | 사용자 가시 | 실제 씬 벽·통·상자 연속 WASD | 제자리걸음 없이 자연 slide, 관통·pop·jitter 없음 | QA MCP 가능 범위 + 사용자 수용 |
| C7 | shared consumer 회귀 | 내부 바이러스 모드 무충돌 이동·벽 접촉·모드 비활성 | 기존 속도·방향·입력 배타 유지, 벽 관통 없음, 공통 slide 외 상태 변화 없음 | `RatHost2DStage2RuntimeTests` + 관련 표적 |

- C6 사용자 수용: 2026-08-05 사용자가 실제 플레이 후 `좋아. 잘 수정됐고`라고 확인했다. 평면 경계의 자연 slide와 관통·pop·jitter 부재에 대한 사용자 가시 criterion을 PASS로 닫는다.

- QA S0 사전 검토: r1 PASS — 공통 slide 허용, 바이러스 상태/입력/속도 보존 C7, 단일 owner와 금지 범위가 연결됐다. 승인된 production·테스트 범위에서 게임플레이 구현 시작 허용.

### QA S0 first blocker와 최소 보정

- first blocker: `RatHost2DVirusMovementController`도 `RatHost2DController.SimulateFixedStep`을 호출하므로 `ClampStepToCollision`의 접선 응답 변경은 바이러스 이동에도 적용된다. 현재 production 소유권·금지 범위·criterion이 이 공유 component contract를 서로 다르게 규정한다.
- 구현 금지 해제: correction 1/2의 공통 계약 보정과 독립 QA r1 PASS로 기존 차단을 해제한다. 지정된 production·테스트 owner와 금지 범위 밖 변경이 필요하면 다시 중지한다.
- 적용한 최소 보정: 금지 범위를 `바이러스 상태 전이·입력·속도 계약 변경 금지`로 좁히고, shared motor의 관통 없는 표면 slide를 쥐·바이러스 공통 계약으로 명시했다. 바이러스 무충돌 속도·입력 배타·아레나 벽 관통 금지를 C7과 관련 suite(`RatHost2DStage2RuntimeTests`)에 추가했다.
- 대안: 쥐에만 slide를 적용하려면 caller별 정책/API가 필요하므로 공개 API·직렬화·추가 production 파일 여부를 먼저 판정하고 작업 범위와 소유권을 다시 잠근다.

## 고비용 preflight 입력

- agent brief JSON: 고비용 검증 전 packet-only로 생성
- verification current-state JSON: 고비용 검증 전 생성
- QA C# harness lint 경로: 신규 reflection 없는 공개 API 표적 테스트
- component contract baseline / candidate / test 경로: `RatHost2DController.cs` / 동일 / `PhysicsCameraAndSort2DTests.cs`, `RatHost2DStage2RuntimeTests.cs`
- isolated Unity cache root / work ID marker: wrapper가 작업 ID 기준 발급
- low-level runner 직접 Run 금지 확인: 확인

## 목적

쥐가 오브젝트와 대각선으로 충돌할 때 전체 이동 벡터가 취소되는 문제를 근본 이동 로직에서 교정한다.

## 입력 자료

- 사용자 실제 플레이 피드백
- `RatHost2DController.cs`
- 기존 자연 부분 가림 후보와 collider 계약

## 해야 할 일

1. QA가 C1~C7과 금지 결과를 구현 전에 검토한다.
2. 구현자는 충돌 법선 성분과 접선 성분을 분리해 최소 변경하고 표적 테스트를 추가한다.
3. 구현자 표적 PASS 뒤 독립 QA가 같은 고정 후보에서 원증상·negative control·관련 회귀를 검증한다.
4. 총괄 판정 뒤 사용자가 실제 WASD 조작감을 확인한다.

## 산출물

- 이동 충돌 응답 코드 최소 수정
- 평면 slide·정면 정지·코너 정지·좌우 대칭 표적 테스트
- 독립 QA와 총괄 판정 기록

## production 소유권과 검증 예산

| production 파일/불변식 | 단일 구현 소유자 | 변경 금지/인계 조건 |
| --- | --- | --- |
| `UnityProject/Assets/_Project/Scripts/TechnicalSample2D/RatHost2DController.cs` 접선 slide·관통 금지 | 게임플레이 구현 에이전트 | QA S0 PASS 전 수정 금지 |
| `UnityProject/Assets/_Project/Tests/EditMode/TechnicalSample2D/PhysicsCameraAndSort2DTests.cs` 표적 회귀 | 게임플레이 구현 에이전트 | 사용자 oracle을 hidden-output 기대값으로 대체 금지 |
| `UnityProject/Assets/_Project/Tests/EditMode/RatHost2D/RatHost2DStage2RuntimeTests.cs` shared consumer 회귀 | 게임플레이 구현 에이전트 | 바이러스 상태·입력·속도 계약 변경 금지 |

- Unity session lease 예정 소유자: 구현자 표적 테스트 시 gameplay owner, 명시 release 뒤 QA
- 관련 suite: `PhysicsCameraAndSort2DTests`, `MovementAndDirection2DTests`, `RatHost2DStage2RuntimeTests`, 필요 시 `RatHost2DTechnicalSampleSceneTests`
- 전체 suite 실행 조건: 동일 freeze 후보에서 S1~S5 green 뒤 QA가 필요성을 판단해 최대 1회
- 대형 matrix 실행 필요·근거: 불필요. 평면 좌우·정면·코너의 축소 pairwise만 사용
- artifact budget / criterion별 canonical 증거: 표적 XML 1개, MCP 사용 시 manifest 1개·캡처 최대 2개

## 비용 계획·실제

| 비용 항목 | 계획 | 실제·근거 |
| --- | --- | --- |
| 역할·인계 | 조정1 → QA S0 1 → gameplay1 → QA1 → 총괄1 | 조정1, QA S0 2(r0+r1), gameplay1, 독립 QA1, 총괄1, 문서/릴리즈1; 상태-only closeout 독립 QA·총괄 대기 |
| 표적 검증 | 구현자 관련 묶음 1회, QA 독립 묶음 1회 | preflight failure 3, run004 16/13/3, run005 16/10/6, run006 16/10/6, run007 16/16 PASS, qa-001 독립 16/16 PASS |
| Unity/MCP/빌드·full suite | preflight 뒤 필요한 Unity test/MCP만, build 0, full 최대 1 | Unity 5 / MCP 0 / build 0 / full 0 |
| matrix/capture·artifact | 축소 pairwise 1, capture 최대 2 | 0 |
| correction·무효/폐기·비용 판정 | 최대 2/2, token/$ 미집계 | R2 재분류 뒤 run007·qa-001 PASS, run004~run006 SUPERSEDED, QA lease CLI no-result 1; token/$ 미집계, `주의 — C6 수용·상태-only closeout 검토 대기` |

- 중앙 현황판 행: `docs/project-handoff/task-cost-dashboard.md`

## 에이전트 수행 이력 기록

- `agent-activity.md` 생성 여부: 예
- 담당 에이전트별 수행 내용 기록 여부: 구현·QA·총괄·문서/릴리즈 기록 완료
- 위임/검토/승인 판정 기록 여부: production 독립 QA·총괄 판정과 C6 사용자 수용 기록 완료. 상태-only 문서 closeout 독립 QA·총괄 판정 대기

## 금지 범위

- collider polygon·크기·offset, 씬 직렬화, ProjectSettings, 패키지 변경
- 자연 부분 가림·Y 정렬·renderer·alpha 로직 변경
- 3D legacy, Stage2/Stage3 상태 전이, 바이러스 입력·속도·모드 활성/비활성 계약 변경. 공용 motor의 관통 없는 slide만 허용
- 입력을 무시하거나 이동 애니메이션을 끄는 증상 은폐

## 승인 필요 항목

- 사용자가 본 수정 작업을 명시 승인했다.
- 씬·collider 수치·공개 API·새 컴포넌트가 필요하면 별도 보고 후 재분류한다.

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 생성
- 담당 에이전트 산출물 확인: 구현·QA·총괄·문서 산출물 확인 완료
- 에이전트 수행 이력 확인: 기록 완료
- 구현 담당 에이전트 확인: 게임플레이 구현 에이전트
- 메인 에이전트 직접 구현 예외 사유 확인: 해당 없음
- QA/검증 에이전트 기록 확인: canonical `surface-slide-qa-001` 16/16 PASS, 상태-only closeout 독립 QA PASS
- 총괄 관리자 판정 확인: `완료 보관 가능 — 사용자 수용 반영`
- 승인 게이트 확인: 기존 승인 범위 및 이번 사용자 수정 지시
- 완료 판단에 영향을 주는 미검증 항목: 없음

## 완료 기준

- C1~C5·C7 자동 검증과 C6 QA 가능 범위를 통과하고, 총괄이 내부 승인 가능으로 판정한다.
- 2026-08-05 사용자 실제 WASD C6 수용을 기록했다.
- 상태-only 문서 closeout 독립 QA와 총괄 최종 판정을 통과했고 현황판 동기화와 완료 폴더 보관을 수행했다.
