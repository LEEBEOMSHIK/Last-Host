# 검증 기록

## 검증 대상

- 작업 ID: `2026-08-05-rat-collision-surface-slide`
- 대상: S0 사용자 원증상·합성 oracle, 동결 후보 C1~C5·C7·E08, C6 사용자 수용과 production 소유권·금지 범위
- 위험 등급: R2
- QA 검증 revision: `S0-r1 + 2286f04110addaa6d5fa9d67e0b269a8c6d800094e40a118339c1ae327e67414 + user-c6-accepted-20260805`

## 실행한 검증

- 정적 문서 대조: `task.md`, 루프 엔지니어링 게이트, QA 역할·Unity 검증 규칙
- 정적 코드 대조: `RatHost2DController.cs`, `RatHost2DMovementController.cs`, `RatHost2DVirusMovementController.cs`
- 정적 테스트 대조: `PhysicsCameraAndSort2DTests`, `MovementAndDirection2DTests`, `RatHost2DTechnicalSampleSceneTests`, `RatHost2DStage2RuntimeTests`의 현재 계약과 추가 가능 지점
- Unity/MCP/TestRunner/build: 실행하지 않음(요청 및 S0 fail-fast 준수)

## 결과

- 사용자 원증상과 C1~C6은 평면 slide·정면 정지·코너 정지·좌우 대칭·무충돌/idle·실제 씬 수용을 직접 연결한다.
- C7은 공용 motor의 두 번째 consumer인 바이러스에 대해 공통 slide만 허용하고 상태 전이·입력 배타·무충돌 속도·벽 관통 금지를 보존한다.
- `RatHost2DController.cs`와 두 표적 테스트 파일의 단일 owner가 지정됐고, 씬·collider·공개 API·새 컴포넌트가 필요하면 중지하도록 경계가 잠겼다.
- C1~C7, production 소유권과 금지 범위는 현재 승인 범위 안의 구현 시작에 충분하다.

## r0 first blocker와 correction

- `RatHost2DController.ClampStepToCollision`은 기술 샘플 쥐뿐 아니라 `RatHost2DVirusMovementController`가 호출하는 공용 충돌 모터다.
- 해당 메서드의 접선 이동 응답을 바꾸면 바이러스의 대각선 충돌 동작도 바뀔 수 있다.
- r0에서는 금지 범위가 `바이러스 이동 변경`을 금지하면서 C1~C6와 관련 suite에 바이러스 영향 계약이 없어 FAIL했다.
- correction 1/2에서 공통 surface slide 허용, 바이러스 상태·입력·속도 보존 C7, `RatHost2DStage2RuntimeTests`와 단일 owner를 추가해 해당 blocker를 해소했다.

## S0 r1 추적성

- C1: 평면 접촉의 법선 차단·접선 진행 합성 검증
- C2: 정면 충돌 정지·관통 금지 negative control
- C3: 실제 코너에서 두 축 정지·pop/jitter 금지 경계
- C4: 좌우 접선 진행 대칭
- C5: 무충돌 속도·대각선 정규화·idle 보존
- C6: 실제 벽·통·상자의 WASD slide 및 사용자 감각 수용
- C7: 바이러스 공통 slide 외 상태 전이·입력 배타·무충돌 속도·벽 관통 금지 회귀

## fail-fast·판정

- r0 first blocker 뒤 중지한 단계: production 구현, 테스트 작성, S1~S7, Unity/MCP/TestRunner/build
- candidate fingerprint / run_id: 구현 전 S0이고 실행 후보가 없어 발급하지 않음
- Unity lease: 획득하지 않음
- correction cycle: `1/2`
- superseded S0 판정: `S0-r0-shared-motor-contract-review` FAIL — correction 1/2로 대체
- QA S0 판정: PASS — 지정된 범위에서 게임플레이 구현 시작 허용
- 완료 판단: 구현 전 계약 통과. 기능 완료·기술 검증 통과 주장은 아직 불가
- 다음 단계: 게임플레이 구현 owner가 C1~C5·C7 표적 구현/검증 후 고정 후보를 독립 QA에 제출

## 독립 QA 실행 시도 — `surface-slide-qa-001`

- frozen candidate fingerprint: `2286f04110addaa6d5fa9d67e0b269a8c6d800094e40a118339c1ae327e67414`
- S1 정적 대조: manifest 5개 파일 hash·length와 aggregate fingerprint 일치, 허용된 production/test 3파일 diff 및 `git diff --check` PASS, 공개 API·직렬화·씬·collider·가림 변경 없음.
- oracle 대조: C1~C5·C7·E08과 negative control이 사용자 원증상 및 shared motor 회귀에 연결됨.
- first blocker: PowerShell 7 native 경계에서 lease Acquire의 `BaselinePlay` 값이 문자열로 전달되어 `Nullable[bool]` 바인딩에 실패했다.
- lease: Acquire 파일 생성 전 실패. QA lease 미획득, release 대상 없음.
- wrapper preflight / Unity / 표적 bundle / XML: `0 / 0 / 0 / 0`.
- 변경한 operational artifact: `agent-brief-qa.json` 추가, `verification-current-state.json`을 `surface-slide-qa-001` / `ready-for-verification` / 동일 fingerprint로 전환.
- 구현자 canonical `surface-slide-impl-007` 16/16 PASS는 현재 fingerprint와 일치하지만 독립 QA PASS로 대체하지 않는다.
- fail-fast 판정: no-result lease CLI blocker에서 중지, 재시도하지 않음.
- 완료 판단: 완료 불가 — 독립 QA 표적 bundle 미실행, C6 사용자 실제 WASD 수용 대기.
- 후속 상태: 사용자가 같은 `surface-slide-qa-001` identity의 저비용 boolean 인자 correction과 resume을 명시 승인해 아래 독립 QA canonical 실행으로 대체했다. 이 no-result 시도는 비용 이력으로만 유지한다.

## 독립 QA canonical 실행 — `surface-slide-qa-001`

- verification revision: `S0-r1 + 2286f04110addaa6d5fa9d67e0b269a8c6d800094e40a118339c1ae327e67414`
- candidate frozen: 예. 실행 전 manifest 5파일과 aggregate, 실행 후 같은 5파일 hash·length를 대조해 drift 0.
- wrapper route: `UnityEditMode`; preflight PASS 뒤 표적 bundle 정확히 1회 실행.
- QA XML: `artifacts/qa-target-results-r1.xml` — `Passed`, total 16 / passed 16 / failed 0 / skipped 0 / inconclusive 0, `valid_pass=true`, Unity exit code 0.
- criterion 연결: C1 평면 slide, C2 정면 정지, C3 실제 코너 정지, C4 좌우 대칭, C5 무충돌·idle, C7 바이러스 상태/입력/속도 회귀, E08 Wall/WaterChannel 비관통을 PASS했다.
- negative control: 공개 API·직렬화·씬·collider·renderer·alpha·입력 우회 변경 0, 바이러스 motor disabled/input 배타와 무충돌 속도 보존 PASS.
- canonical QA run_id: `surface-slide-qa-001`.
- 구현자 증거: `surface-slide-impl-007` 16/16 PASS는 구현자 자체 검증으로 유지하며 독립 QA canonical run에 합산하지 않는다.
- superseded product runs: `surface-slide-impl-004`, `surface-slide-impl-005`, `surface-slide-impl-006`.
- lease: owner `qa-verification-agent`, PID `23672`, baseline Play/Pause/dirty `false/false/false`, scene `RatHost2DTechnicalSample`; `2026-08-05T06:43:48.3082581Z` 획득, `2026-08-05T06:46:40.2742238Z` release. 임시 객체·MCP·scene 조작 없음.
- 비용: QA Unity start 1, 표적 bundle 1, wrapper high-cost attempt 1, full suite 0, MCP 0, build 0, capture 0. lease CLI no-result 1은 회피 가능 비용으로 기록.
- first blocker: canonical 실행에는 없음.
- 실행하지 않은 항목: full suite, MCP Play, build는 명시적으로 금지되어 미실행. C6 native WASD 조작감은 자동 검증할 수 없어 사용자 수용 대기.
- QA 게이트 판정: PASS.
- 완료 판단: 기술 검증 통과 — 사용자 수용 대기.
- 총괄 전달: 가능. 다만 C6 사용자 수용 전 `완료` 표현 금지.

## C6 사용자 수용 — 2026-08-05

- 사용자 확인 문구: `좋아. 잘 수정됐고, 방금과 같은 이슈는 해당과 같이 처리될 수 있도록 정리해둬`.
- 확인 대상: 실제 `RatHost2DTechnicalSample`에서 벽·통·상자 아래/뒤 경계에 대각선 WASD를 유지했을 때의 자연스러운 표면 slide와 관통·pop·jitter 부재.
- 판정: C6 PASS. 자동 검증으로 대체하지 않고 사용자 실제 플레이 수용 증거로 기록한다.
- canonical 구현자 run/fingerprint: `surface-slide-impl-007` / `2286f04110addaa6d5fa9d67e0b269a8c6d800094e40a118339c1ae327e67414` 유지.
- canonical 독립 QA run: `surface-slide-qa-001` 16/16 PASS 유지. run004~run006 실패와 비용 기록은 삭제하지 않고 `SUPERSEDED` 상태로 보존한다.
- 추가 Unity/MCP/TestRunner/build 실행: 0. 사용자 수용 기록과 재발 방지 계약 문서화는 상태-only 변경이다.
- 현재 완료 차단: 없음. 상태-only 문서 독립 QA와 총괄 최종 판정을 통과해 현황판·완료 경로 동기화가 허용됐다.

## 상태-only closeout 독립 QA — 2026-08-05

- 검토 범위: `rat-host-implementation-plan.md`, `agent-reference-map.md`, 현재 active 패킷의 `task.md`, `verification.md`, `agent-activity.md`, `handoff.md`, `completion-report.md`.
- C6 기록: 사용자 원문 `좋아. 잘 수정됐고`를 실제 플레이 수용으로만 기록했고, 자동 검증·빌드·추가 화면 확인으로 확대하지 않아 과장 없음.
- 계약 대조: 새 `2D 이동·충돌 표면 슬라이드 계약`은 실제 `RatHost2DController.ResolveCollisionStep`의 반복 normal projection, `CollisionSkin`, `ClampUnsafeFinalSweep`와 안전한 단일 `MovePosition` 적용을 정확히 설명한다.
- criterion 대조: C1 평면 slide, C2 정면 정지, C3 실제 코너 정지, C4 좌우 대칭, C5 무충돌·idle, C7 shared consumer, E08 Wall/WaterChannel 비관통 경로가 문서 수용 기준과 연결된다.
- 금지 방식·재발 처리: 입력/renderer/collider/tolerance 우회 금지와 같은 criterion 연속 실패 2회 뒤 원인·변경 계획·R등급 재분류 규칙이 `loop-engineering-gates.md`와 일치한다.
- 증거 보존: canonical 구현자 `surface-slide-impl-007`, 독립 QA `surface-slide-qa-001`, fingerprint `2286f04110addaa6d5fa9d67e0b269a8c6d800094e40a118339c1ae327e67414`, run004~run006 `SUPERSEDED`, Unity 5/MCP 0/build 0/full 0과 회피 가능 비용 기록이 유지됐다.
- 링크·트리거: 구현 계획의 계약 heading과 `agent-reference-map.md`의 쥐 숙주/Unity 구현 특수 적용 링크 및 작업 패킷·canonical XML·manifest 경로가 모두 존재한다.
- 상태 보류: board/cost/CURRENT는 active 작업 경로를 유지하고 completed 대상 폴더는 아직 존재하지 않는다. closeout QA·총괄 뒤 최종 동기화·이동한다는 패킷 설명과 일치한다.
- `git diff --check`: PASS.
- fingerprint 대조: manifest 입력 production/test/package/version 5파일의 SHA-256·길이 모두 일치. 상태-only 문서 변경은 production/test 기능 증거를 무효화하지 않는다.
- first blocker: 없음.
- 실행하지 않은 항목: Unity/MCP/TestRunner/build 0. production/test 수정 0.
- 판정: **기능 증거 unaffected, closeout 문서 QA PASS**.
- 다음 단계: 프로젝트 총괄 관리자의 상태-only closeout 최종 판정 뒤 board/cost/CURRENT 최종 동기화와 completed 이동.
