# 루프 엔지니어링·검증 하네스 감사 통합 보고

## 직접 결론

쥐와 오브젝트의 겹침 교정은 단순 collider 수치 변경으로 끝나는 작업은 아니었다. 최종 해법에는 긴 단일 스프라이트의 합성 가림, 방향별 collider, 상태형 occlusion resolver, renderer 소유권, 다중 오클루더, 씬 직렬화 복원, X/Y 경계 hysteresis와 관련 테스트가 결합됐다. 따라서 **작업 자체는 중간 난도의 런타임·씬 통합 수정**으로 분류하는 것이 맞다.

그러나 발생한 비용 전부가 난도 때문에 불가피했던 것은 아니다. 이전 작업에서 사용자 화면의 원증상을 완료 기준으로 잠그지 않았고, 최초 구현 소유권이 뒤집혔으며, 변경 후 기존 PASS를 무효화하는 규칙과 fail-fast 순서가 없었다. 전체 EditMode와 대형 matrix를 후보가 안정되기 전에 반복했고 캡처도 한 번 폐기했다. 따라서 **비용의 일부는 과도했고 회피 가능했다.** 저장소에 토큰 계측값은 없으므로 토큰 수나 절감률은 추정하지 않는다.

현재의 독립 QA·총괄 차단 기능 자체는 유효했다. 독립 QA가 저장 씬 복원, 수평 경계 hysteresis, 수정에 따른 벽 회귀와 잘못된 캡처를 실제로 차단했다. 문제는 QA가 무능했거나 인원이 부족했던 것이 아니라, 이 결함들이 독립 QA 단계에 도달하기 전에 싸게 실패하도록 만드는 앞단 계약과 하네스가 부족했다는 점이다.

## 독립 QA는 정확히 한 명이었다

가림 교정 기록에는 여러 역할이 등장하지만 독립 QA는 `QA/검증 에이전트` 한 명/한 역할뿐이다.

| 역할 | 독립 QA인가 | 실제 책임 |
| --- | --- | --- |
| 메인 조정자 | 아니오 | 작업 패킷, 배정, 상태 통합 |
| 비주얼/테크아트 분석자 | 아니오 | alpha 실루엣과 가림 계약 분석 |
| Unity 씬/통합 구현자 | 아니오 | 최초 코드·씬·테스트 구현과 구현자 자체 검증 |
| 게임플레이 런타임 리뷰어 | 아니오 | 상태형 resolver 코드 리뷰와 후속 수정 |
| QA/검증 에이전트 | **예** | 독립 테스트·MCP·경계 matrix·증거 감사 |
| 프로젝트 총괄 관리자 | 아니오 | QA 기록과 승인 범위의 최종 내부 판정 |

구현자 테스트, 코드 리뷰, 총괄 검토를 모두 “검증 에이전트 수”로 합산하면 실제 구조를 잘못 이해하게 된다. 반복 수정의 원인은 인원수가 아니라 서로 다른 단계가 어떤 oracle과 불변식을 소유하는지 구현 전에 고정하지 않은 데 있었다.

## 정당한 비용과 과도했던 비용

### 정당하거나 필요한 비용

- 물리 collider 겹침과 최종 합성 화면의 조각난 실루엣이 다른 문제라는 진단
- collider 확대나 sorting order만으로는 해결할 수 없다는 분석
- 상태형 whole-character occlusion 구조와 방향별 collider 동기화
- tail-only, 다중 오클루더, renderer 외부 소유권, 저장·재로드, hysteresis 테스트
- 구현자 자체 검증 한 번, 독립 QA의 최종 관련·전체 검증 한 번, 총괄 최종 판정
- 벽·통·상자의 앞/뒤 화면 증거와 사용자의 실제 WASD 수용 분리

### 과도했거나 회피 가능했던 비용

- 직전 교정에서 `visual_overlap=true`를 실패로 보지 않아 같은 사용자 결함을 두 번째 작업으로 다시 연 비용
- 씬/통합 담당이 런타임 상태 머신을 먼저 만들고 런타임 담당이 사후 보정한 이중 소유 비용
- 계약에 있던 한쪽 꼬리 조각을 최초 관련 테스트로 옮기지 못한 비용
- save→reload, X/Y entry·release, 외부 renderer 상태를 독립 QA 전에 검증하지 않은 비용
- 구현 변경자가 최소 관련 회귀를 실행하지 못한 채 QA에 재인계해 `QA → 수정 → QA`가 세 번 반복된 비용
- 안정화 전 전체 EditMode cold run과 72/432 matrix를 반복한 비용
- 임시 QA 객체와 실제 RatHost 상태가 섞인 PNG·CSV 세트를 폐기하고 다시 만든 비용
- 최종 판정에 쓰이지 않는 여러 세대의 전체 로그·CSV·PNG를 장기 증거로 남긴 비용

## 반복 수정의 구조 원인

1. **사용자 원증상 oracle 미고정**
   - 사용자 문제는 최종 화면에서 쥐가 오브젝트 양쪽에 끊긴 조각으로 보이는 것이었다.
   - 이전 완료 계약은 collider 비중첩과 sorting order를 중심으로 삼아 다른 문제를 정확히 검증했다.
   - 많은 PASS 수치가 있어도 사용자 합성 화면 oracle이 없으면 원래 문제의 해결을 증명하지 못한다.

2. **production 소유권 역전**
   - 상태 머신·renderer 소유권·lifecycle을 포함한 새 resolver를 씬/통합 담당이 먼저 만들었다.
   - 런타임 담당은 사후 리뷰에서 tail-only, 다중 오클루더 hysteresis 누수, 외부 disabled renderer 침범을 발견했다.
   - 상태형 production 코드의 단일 소유자가 처음부터 정해졌어야 했다.

3. **변경 후 PASS 무효화 부재**
   - Barrel 경계 수정 뒤 기존 wall 계약의 PASS가 유효한 것처럼 남았고, 다음 전체 QA가 회귀를 처음 발견했다.
   - production·테스트·하네스 변경마다 어떤 XML·CSV·PNG가 무효가 되는지 규칙이 없었다.

4. **fail-fast 부재**
   - 값싼 상태·축 경계·직렬화 검증보다 전체 EditMode와 대형 matrix가 먼저 또는 반복 실행됐다.
   - 첫 blocker에서 고비용 검증을 멈추고 최소 반례만 반환하는 순서가 없었다.

5. **캡처 비원자성**
   - 상태 설정, resolver·camera·HUD 갱신, PNG 캡처와 CSV 기록이 서로 다른 호출·시점에 수행됐다.
   - 임시 객체와 실제 객체가 함께 남은 첫 최종 캡처는 수치 증거와 화면이 일치하지 않아 폐기됐다.

## 실제 반영한 운영 보완

### 위험 기반 최소 루프: R0~R3

- `R0`: 무변경 조회·설명. 작업 패킷·QA·총괄 없음
- `R1`: 기존 계약 안의 국소 수정. 구현 소유자 1명, 표적 독립 QA, 총괄 최종 판정
- `R2`: 새 상태·씬/코드 통합·직렬화·사용자 가시 결함. QA S0, 단일 production owner, 단계 검증, 독립 QA, 최종 회귀
- `R3`: 패키지·ProjectSettings·렌더링·저장 형식·릴리즈. 위험에 필요한 전문 역할과 전체 승인·빌드 게이트

모든 전문 역할을 관성적으로 호출하지 않되 R1~R3의 독립 QA와 총괄 최종 판정은 유지한다. 세부 실행 기준은 `docs/agents/loop-engineering-gates.md` 한 파일이 소유한다.

R1은 실제 `_workspace/templates/task-r1-summary.md`를 사용한다. 원증상·완료 주장·변경 파일/owner·표적 테스트·금지 범위·correction cycle·QA·총괄만 기록하며, R2/R3의 정식 상태 전이표·수명주기 matrix·전체 S0 표는 작성하지 않는다.

### 구현 전 S0와 S1~S7 fail-fast

- `S0`: 사용자 원문, 입력·좌표·상태, 합성 oracle, 성공/실패/경계/negative control을 QA가 구현 전에 잠금
- `S1`: diff·정적 검증·컴파일
- `S2`: 순수 함수·상태·경계·소유권 단위 테스트
- `S3`: 직렬화·reload·관련 EditMode
- `S4`: 실제 대상 scene과 원증상 smoke
- `S5`: 위험 기반 축소 pairwise·경계 왕복
- `S6`: freeze된 후보의 전체 회귀 1회
- `S7`: 필요한 대형 matrix와 최종 증거 1회

첫 blocker가 나오면 뒤의 full suite, 전체 matrix, 대량 캡처를 중지한다.

### 단일 owner와 2회 재분류

- production 파일과 불변식은 한 correction cycle 동안 소유자 한 명만 수정한다.
- QA는 production을 고치지 않고 최소 반례를 소유자에게 돌려준다.
- 수정자는 QA 재접수 전에 무효화된 최소 관련 회귀를 직접 통과한다.
- 같은 S0 계약에서 correction cycle 2회 실패하거나 QA 중 계약이 두 번째 확장되면 `수정 필요 — 재분류`로 중지하고 root cause·상태 전이·소유권·구조를 다시 검토한다.

### 검증 revision, lease, fingerprint와 증거 예산

- production·테스트·scene·package/version 입력을 `candidate_fingerprint`로 묶고 실행마다 `run_id`를 남긴다.
- 변경으로 영향받은 이전 PASS는 `SUPERSEDED`이며 최종 통과 수에 포함하지 않는다.
- Unity Editor, MCP, TestRunner와 같은 Library의 batch Unity는 project 단위 single-owner lease를 사용한다.
- criterion마다 canonical 증거 1개를 기본으로 하고, 중간 실패는 최소 반례와 핵심 로그 위치만 보존한다.
- `기술 검증 통과`, `기술 검증 통과 — 사용자 수용 대기`, `내부 승인 가능`, `완료`를 분리한다.

## 실제 추가한 범용 검증 도구

| 도구 | 실제 기능 | 현재 검증 상태 |
| --- | --- | --- |
| `UnityMcpLease.ps1` | schema 2가 `FileMode.CreateNew`, `agent/work_id/run_id`, `editor_pid`, `scene`, `baseline_*`, TTL, 명시적 갱신·반납, 만료 자동 강탈 금지를 제공 | 독립 QA r2 PASS — 필수 11필드·필수 인자·identity·경합·만료·alias 검증 |
| `Invoke-UnityEditModeTests.ps1` | stale XML 삭제, Unity EditMode batch 대기, NUnit XML strict 판정, timeout 시 시작 PID만 종료 | 독립 QA r2 PASS — pass/fail/missing/skipped/inconclusive·timeout negative control |
| `Get-VerificationFingerprint.ps1` | 지정 production/test/scene/package/version 파일별 SHA-256과 정렬 집계, run manifest | 독립 QA r2 PASS — 결정성·변경 감지·누락 입력 차단 |
| `tools/verification/README.md` | lease→fingerprint→S0~S7 사용 순서와 한계 | 독립 QA r2 문서·도구 계약 정합 PASS |

이 도구는 QA 역할을 대체하거나 늘리지 않는다. Unity 단독 소유, stale XML 차단, dirty candidate 식별을 기계화해 QA가 올바른 후보를 검증하도록 돕는다.

## 남은 한계와 다음 검증

- **실제 Unity live run은 이번 감사·도구 구현에서 실행하지 않았다.** 기존 독립 QA의 final-v2 XML을 read-only로 판정했을 뿐, 새 runner로 Unity를 실제 기동하거나 실제 Editor PID와 lease를 결합하지 않았다.
- 독립 QA r1이 lease 문서와 스크립트 사이의 필드 불일치를 blocker로 확인했고, 구현자는 schema 2에서 필수 11개 필드를 실제 lease JSON에 반영했다. 독립 QA r2는 새 fingerprint `92938ce9f246d5d6d263faecfca8e2f5449f220af2c788ec80b4039967e169a0`에서 해당 필드와 negative control을 PASS했으며 r1 증거는 `SUPERSEDED` 처리됐다.
- lease는 협력적 잠금이다. 스크립트를 무시한 외부 MCP 호출을 기술적으로 막지는 못하므로 운영 게이트와 evidence manifest 대조가 함께 필요하다.
- fingerprint는 호출자가 지정한 경로만 보호한다. 누락 dependency를 자동 추론하지 않는다.
- **범용 atomic GameView capture 도구는 만들지 않았다.** 모든 시각 작업을 하나의 과도한 범용 도구로 추상화하면 오히려 상태 계약이 흐려질 수 있다. 시각 검증이 필요한 작업마다 해당 scene·root·상태·frame barrier를 아는 **repo-owned task-specific Editor harness**를 만들고 버전 관리해야 한다.
- 총괄 1차 blocker 보정 후보는 독립 QA `process-harness-qa-r3`에서 fingerprint `11edc0c864b179cd1dd2468764b74aa2dda94c20376a19815b422f0a334a8aa6`, run `loop-harness-qa-r3-20260802`로 PASS했다. 총괄 2차의 마지막 상태 문구 blocker도 동기화했고, 총괄 3차 최종 재대조는 `내부 승인 가능`을 판정했다. 이후 비용 현황판 revision은 QA r6 PASS와 총괄 최종 승인을 통과했고, 사용자 커밋 요청에 따라 `_workspace/completed/2026-08-02-2026-08-02-loop-harness-efficiency-audit/`로 완료 보관했다.

## 사용자 관점의 변화

앞으로 같은 유형의 시각 결함은 “많은 테스트를 일단 전부 실행”하는 방식이 아니라 다음 순서로 처리한다.

1. 사용자가 본 화면과 입력을 실패 oracle로 먼저 잠근다.
2. 상태형 코드와 씬 wiring의 단일 소유자를 처음부터 나눈다.
3. 값싼 경계·소유권·직렬화 테스트에서 먼저 실패시킨다.
4. 첫 blocker 하나만 수정하고 변경자가 최소 회귀를 통과시킨다.
5. 후보가 고정된 뒤 전체 suite와 최종 증거를 한 번 만든다.
6. 자동 검증과 사용자의 실제 WASD·화면 수용을 별도 상태로 보고한다.

이 구조는 독립 QA와 총괄 게이트를 없애 비용을 줄이는 방식이 아니다. 그 두 게이트에 불안정한 후보가 도달하지 않도록 앞단에서 결함을 더 싸고 명확하게 차단하는 방식이다.
