# 루프 엔지니어링 게이트

최종 수정일: 2026-08-05

## 목적과 규칙 소유권

이 문서는 프로젝트 변경 작업의 위험 분류, 실행 순서, 검증 증거, 완료·커밋 차단 조건을 정하는 **유일한 실행 기준**이다. `AGENTS.md`, 역할 문서, 스킬, `_workspace` 템플릿은 이 문서를 요약하거나 입력 필드를 제공할 뿐 다른 실행 순서를 만들지 않는다.

사용자·신규 참여자용 설명과 확인 체크리스트는 `docs/agents/loop-engineering-user-guide.md`를 참고한다. 이 가이드는 비실행 요약이며 이 문서의 규칙을 대체하지 않는다.

독립 QA와 프로젝트 총괄 관리자의 최종 게이트는 비용 절감을 이유로 약화하거나 생략하지 않는다. 대신 위험에 필요한 역할만 호출하고, 싸고 직접적인 검증부터 실패 즉시 중단한다.

## 적용 대상과 상태 용어

코드, 테스트, 씬, 프리팹, 에셋, ProjectSettings, 승인 문서, 운영 문서 변경과 커밋·완료 주장은 이 문서를 따른다. 무변경 조회·설명은 R0로 분류한다.

- `구현 중`: production 후보가 아직 변경될 수 있다.
- `기술 검증 통과`: 고정된 후보가 자동·Play 검증을 통과했다.
- `기술 검증 통과 — 사용자 수용 대기`: 자동화할 수 없는 핵심 입력·화면·감각을 사용자가 아직 확인하지 않았다.
- `내부 승인 가능`: 총괄 관리자가 현재 증거로 사용자에게 올릴 수 있다고 판정했다.
- `완료`: 요구된 기술 검증과 필요한 사용자 수용·기록이 모두 끝났다.

`기술 검증 통과`와 `완료`를 같은 뜻으로 쓰지 않는다.

## R0~R3 위험 등급과 최소 역할

| 등급 | 기준 | 최소 역할과 루프 |
| --- | --- | --- |
| R0 조회 | 파일·상태 조회, 설명, 무변경 | 메인 응답. 작업 패킷·QA·총괄 없음 |
| R1 국소 수정 | 기존 계약 안의 문구·값·국소 코드, 새 상태·공개 API·에셋·설정 없음, 보통 1~3개 파일 | 구현 소유자 1명 → 표적 검증 → 독립 QA 표적 재검증 → 총괄 최종 판정. 패킷은 요약형 |
| R2 상태/통합 수정 | 새 상태 전이·컴포넌트, 씬+코드 결합, 물리·정렬·직렬화, 여러 오브젝트, 사용자 가시 결함 | QA S0 사전 charter → production 불변식별 구현 소유자 → 단계 검증 → 독립 QA → 최종 회귀 → 총괄 판정 |
| R3 구조/릴리즈 | 패키지, ProjectSettings, 렌더 파이프라인, 저장 형식, 대규모 씬·아트, 릴리즈 빌드 | 구조·통합 등 위험에 필요한 전문 역할 + 구현 소유자 + 독립 QA + 총괄. 승인·빌드·회귀 증거 유지 |

- 조정자는 파일 수보다 새 상태, 직렬화, 사용자 가시성, 복구 난이도로 등급을 정한다.
- 모든 역할을 관성적으로 호출하지 않는다. 분석·아키텍처·비주얼 등 전문 역할은 위험 근거가 있을 때만 추가한다.
- R1~R3 변경의 독립 QA와 최종 총괄 판정은 유지한다.
- 총괄 사전 검토는 R3, 승인 범위 충돌, 등급 불명확 때만 필수다. R1/R2 정형 분류는 조정자가 수행한다.
- 구현 중 새 상태·직렬화·담당 밖 production 파일이 생기면 즉시 중지하고 등급과 소유권을 다시 판정한다.

## 작업 배정과 S0 검증 charter

### R1 요약형

R1은 `_workspace/templates/task-r1-summary.md`를 사용한다. 다음 최소 필드만 요구한다.

1. 사용자 원증상과 완료 주장 한 문장
2. 변경 파일과 production owner
3. 표적 테스트
4. 금지 범위
5. correction cycle `0/2`
6. 독립 QA와 총괄 최종 판정

R1은 R2/R3의 정식 상태 전이표, 수명주기 matrix, 전체 S0 criterion 표, 전체 suite·대형 matrix 계획을 작성하지 않는다. 이 항목이 필요해지거나 새 상태·직렬화·씬 통합이 드러나면 구현을 중지하고 R2로 재분류한다. QA는 원증상·완료 주장·표적 테스트가 직접 연결되는지만 구현 전에 짧게 확인한다.

### R2/R3 정식 S0

R2/R3는 `_workspace/templates/task.md`를 사용하고 구현 전 다음을 잠근다.

1. 사용자 원증상: 원문, 입력, 씬, 좌표·상태, 실패 캡처 또는 재현 절차
2. 합성 oracle: 사용자가 실제로 보는 최종 화면·동작의 금지/허용 결과
3. 완료 주장 한 문장과 criterion ID
4. 성공, 실패, 경계, negative control, 상태 전이, 수명주기 불변식
5. production 파일/불변식별 단일 구현 소유자
6. 위험 등급, 관련 suite, 전체 suite 허용 조건, correction cycle `0/2`
7. Unity 도구가 필요하면 session lease 예정 소유자

QA는 R2/R3의 정식 S0 charter에서 위 항목을 검토한다. 원증상을 재현하지 못했거나 합성 oracle이 잠기지 않으면 추정 구현을 시작하지 않고 `재현 불가` 또는 사용자 질문으로 중단한다. 구현자가 만든 테스트 이름이나 내부 수치로 사용자 원증상을 대체하지 않는다.

## 원인 교정과 증상 은폐 금지

사용자 화면에서 증상이 사라졌다는 사실만으로 원인이 교정됐다고 판정하지 않는다. 다음처럼 관찰 대상을 없애거나 실패 경로를 우회하는 변경은 원인 레이어가 따로 증명되지 않는 한 `증상 은폐`다.

- renderer·대상 object 비활성화, alpha `0`
- 정상 좌표를 바꾸는 teleport·clamp, 입력 잠금
- 오류를 삼키거나 결과를 성공처럼 반환하는 error swallow
- 가시 footprint보다 큰 invisible collider로 접근 자체를 막는 방식
- hidden output이나 우회된 상태를 기대하도록 테스트를 바꾸는 방식

workaround는 사용자가 명시적으로 승인하고, 플레이 화면·로그에 임시임을 표시하며, 제거 조건·기한 또는 후속 작업을 기록했을 때만 허용한다. 승인된 workaround도 근본 수정의 `완료`가 아니며 `temporary` 또는 `blocked` 상태로 관리한다.

QA는 원인 레이어의 변경과 함께 다음 negative control을 직접 증명한다.

1. 플레이어 root·renderer의 active/enabled/alpha, transform, 정상 입력 경로가 보존된다.
2. collision이 가시 footprint와 S0에 고정한 tolerance 안에서 일치하며, 보이지 않는 과대 collider가 없다.
3. 사용자가 실제로 보는 화면·동작 oracle이 통과하고, 실패를 숨긴 출력이 canonical 증거에 포함되지 않는다.

원인 레이어를 증명하지 못하고 증상만 바꾼 후보는 기술 검증 통과나 완료가 아니라 `temporary` 또는 `blocked`다. 2026-08-02 `7ba12df`의 whole-character hide가 자동 검증 뒤 사용자 수용에서 실패한 사례는 이 규칙을 도입한 교훈으로만 남긴다. 특정 커밋이나 구현 방식 자체를 영구 정책의 전제로 삼지 않는다.

## production 소유권

- production 파일 하나와 그 불변식에는 한 correction cycle 동안 구현 소유자 한 명만 둔다.
- 상태 머신·게임플레이 수명주기는 게임플레이 구현 담당, 씬 배치·직렬화·wiring은 Unity 씬/통합 담당이 소유한다.
- 공동 파일이 불가피하면 어떤 불변식을 누가 바꾸는지 먼저 나누고 동시 편집하지 않는다.
- QA는 production 파일을 수정하지 않고 첫 blocker의 최소 반례를 소유자에게 반환한다.
- 수정 소유자는 QA 재접수 전에 무효화된 최소 관련 검증을 직접 통과시킨다.

## 중복 검증 방지와 canonical 실행 소유권

- 구현자는 변경 불변식의 표적 검증을 후보 제출 전 한 번 실행한다. QA는 현재 freeze 후보의 사용자 원증상·수용 핵심을 독립적으로 한 번 검증한다.
- 전체 suite, 대형 matrix와 최종 수용 증거의 canonical 실행 소유자는 독립 QA다. S1~S5가 같은 후보에서 green인 뒤 필요한 항목을 각각 한 번만 실행한다.
- 프로젝트 총괄 관리자는 TestRunner, MCP Play, 빌드, matrix, 캡처를 직접 실행하지 않는다. 현재 QA 증거·fingerprint·승인 범위·사용자 수용 대기를 감사한다.
- 동일 criterion이나 suite 재실행은 후보 변경으로 기존 PASS가 무효화됐을 때, 첫 실패의 최소 반례를 재현·해소할 때, 구현자 자체 검증 뒤 독립성을 확보할 때만 허용한다.
- 재실행에는 새 `run_id`, `candidate_fingerprint`, 사유와 대체되는 run을 기록한다. 같은 후보·목적의 반복 실행, 인원수만 늘린 복제 검증, 불안정 후보의 전체 suite·대형 matrix 반복은 금지한다.

### 공통 실행·보고 계약

1. **preflight 차단**: 내부 attempt ledger와 진단용 `run_id`는 보존한다. 다만 실제 Unity/MCP/build가 시작되지 않은 차단은 고비용 실행이나 사용자-facing run 횟수·번호로 표현하지 않는다.
2. **구현 고비용 표적 상한**: 같은 원인 분류에서는 최초 1회와 correction 1회까지만 허용한다. 두 번째 실패 뒤에는 `수정 필요 — 재분류`로 중지하고, 원인·위험 등급을 재분류해 사용자에게 `문제 / 선택지 / 추천`을 보고하기 전 새 고비용 후보를 시작하지 않는다.
3. **독립 QA 상한**: 구현자의 current fingerprint가 green이 된 뒤 독립 QA가 1회 진입한다. QA 실패를 소유자가 보정한 뒤 QA 재진입은 1회까지만 허용하며, 두 번째 QA 실패에서는 중지·재분류·사용자 보고한다. 이 상한은 독립 QA 생략을 허용하지 않는다.
4. **S0 표현**: 내부 검토 담당이 QA여도 사용자에게는 `S0 계약 검토`로 표현하고 `QA run`이나 고비용 실행 횟수로 세지 않는다.
5. **상태-only 최종 동기화**: 독립 QA와 총괄 판정 뒤 board·cost·CURRENT·completed 경로·상태만 바꾸는 최종 동기화는 새 QA·총괄 라운드 없이 조정자가 source/target path·status·diff를 자체 대조하고 끝낸다. 운영 규칙, acceptance contract, production, 테스트·하네스 변경은 상태-only가 아니며 기존 QA·총괄·증거 무효화 게이트를 그대로 적용한다.
6. **사용자 진행 보고**: 최초 blocker, 재분류·사용자 결정 필요, 기술 PASS·최종 결과를 중심으로 압축한다. 내부 run label과 30초 단위 세부 상태는 사용자가 요청하지 않으면 노출하지 않는다.

이 계약은 내부 추적성을 줄이지 않는다. attempt ledger, candidate fingerprint, canonical run, `SUPERSEDED`, lease와 비용 기록은 기존 규칙대로 유지한다.

## S1~S7 fail-fast 실행 순서

한 단계가 실패하면 즉시 중단하고 뒤의 고비용 단계로 가지 않는다.

| 단계 | 목적 | 필수 실행 원칙 |
| --- | --- | --- |
| S0 계약 고정 | 원증상·oracle·불변식 누락 차단 | QA 사전 charter, criterion→검증 추적표 |
| S1 정적·컴파일 | 가장 싼 오류 차단 | diff check, 대상 validation, 컴파일·Console |
| S2 순수 함수·상태 단위 | 상태·경계·소유권 검증 | 변경 불변식의 성공/실패/경계/negative control |
| S3 수명주기·관련 테스트 | 직렬화·reload·관련 fixture | save/reopen, enable/disable, 관련 EditMode |
| S4 대상 scene smoke | wiring과 원증상 확인 | 대상 scene, 실제 root 1개, 원증상 1개, Console |
| S5 축소 경계 검증 | 대표 geometry와 전이 회귀 | 위험 기반 pairwise·경계 왕복. 전체 Cartesian 금지 |
| S6 전체 회귀 | 프로젝트 회귀 | S1~S5가 같은 고정 후보에서 green인 뒤 1회 |
| S7 최종 증거 | 사용자 보고용 수용 증거 | 필요한 대형 matrix 1회, 원자 캡처, 보호 diff·dirty 확인 |

QA는 항상 원증상 oracle부터 독립적으로 재확인한다. 첫 blocker를 찾으면 full suite, 전체 matrix, 다량 캡처를 중지하고 최소 반례 하나를 반환한다. 전체 suite와 대형 matrix는 `candidate frozen`으로 표시된 최종 후보에서 각각 한 번만 실행한다.

## candidate fingerprint, run_id와 verification revision

- `candidate_fingerprint`: 검증 대상 production 파일, 관련 테스트·하네스, Unity 버전·package lock과 필요한 설정의 내용을 정렬해 식별한 값이다. Git HEAD만으로 dirty 후보를 식별하지 않는다.
- `run_id`: 같은 후보에서 수행한 한 검증 실행과 증거 묶음의 ID다.
- `verification_revision`: S0 계약과 candidate fingerprint를 결합한 판정 단위다.
- XML, 로그, CSV, PNG sidecar는 사용한 `run_id`와 `candidate_fingerprint`를 기록한다. QA 판정에는 현재 유효 run_id 하나만 canonical로 지정한다.

### 변경 후 PASS 무효화

| 변경 | 무효화 범위 |
| --- | --- |
| production 코드·씬·프리팹·에셋·설정 | S1 이후 자동·Play·캡처 PASS 전체 |
| 관련 테스트 | 해당 테스트 PASS와 이를 인용한 판정 |
| 캡처·matrix 하네스 | 그 하네스가 만든 CSV·PNG·sidecar |
| acceptance contract | S0부터 전체 판정 |
| QA 문구·상태판만 변경 | 기능 증거 유지, 문서만 갱신 |

무효 증거는 삭제하지 않고 `SUPERSEDED`와 후속 run_id를 기록하되 통과 수에 합산하지 않는다. production 수정 뒤 관련 테스트만 통과했다면 전체 회귀 통과를 다시 주장할 수 없다.

## Unity MCP/Editor single-owner lease

같은 Unity 프로젝트의 Editor, MCP, 같은 Library를 쓰는 batch Unity는 한 시점에 한 소유자만 조작한다. lease 획득 전에는 Play/Pause, scene open/save, TestRunner, Console clear, capture, Refresh를 실행하지 않는다.

lease 필수 필드:

```text
work_id, agent, run_id, editor_pid, scene,
acquired_utc, expires_utc,
baseline_play, baseline_pause, baseline_scene, baseline_dirty
```

- 기본 만료 5분, 60초 heartbeat를 사용한다. PID가 살아 있으면 만료만으로 탈취하지 않는다.
- 인계 전 소유자는 Play 종료, 임시 객체 제거, scene dirty 복원 여부를 기록하고 명시적으로 release한다.
- QA가 blocker를 반환하면 QA가 증거를 저장하고 lease를 반납한 뒤 구현자가 최소 회귀를 실행한다. 동시 조작 금지가 구현자 검증 생략의 이유가 되어서는 안 된다.
- 격리 복사본은 project key를 분리한다. 최종 증거에 `lease_owner`가 없으면 총괄 통과를 차단한다.

## 원자적 증거와 stale-object guard

최종 화면 캡처는 한 Editor 명령과 한 frame barrier 안에서 다음을 묶는다.

1. lease, run_id, candidate fingerprint 확인
2. 대상 scene·GlobalObjectId·기대 인스턴스 수 확인
3. 위치·프레임·상태 설정
4. physics, sorting, resolver, camera, HUD 동기 갱신
5. 같은 Camera로 PNG 생성
6. 같은 메모리 상태에서 sidecar 생성·checksum 계산
7. 임시 파일을 최종 파일로 원자 이동
8. 임시 객체와 dirty 상태 확인

`QA_Temp*`, 예상 밖의 중복 player/controller/camera, 잘못된 root identity가 있으면 증거를 생성하지 않고 실패한다. sidecar에는 최소 `run_id`, `candidate_fingerprint`, scene, editor PID, lease owner, subject ID/count, 좌표·상태, Console 오류 수, dirty 전후, PNG checksum을 둔다. CSV는 가능한 한 sidecar에서 생성한다.

## correction cycle 중단 규칙

- blocker 수정과 QA 재접수를 한 correction cycle로 센다.
- 같은 S0 계약에서 2회 연속 실패하거나 QA 중 계약이 두 번째로 확장되면 패치 누적을 중지한다.
- 상태를 `수정 필요 — 재분류`로 바꾸고 root cause, 상태 전이표, 위험 등급, production 소유권, 대안 구조를 다시 검토한다.
- 재분류는 실패 통과, 독립 QA 또는 총괄 생략을 허용하지 않는다.

## 고비용 검증 실행 전 자동 차단

Unity TestRunner, MCP Play/TestRunner, build와 같은 고비용 경로는 `tools/verification/Invoke-HighCostVerification.ps1`를 유일 공용 진입점으로 사용한다. 실행 문서의 권고만으로 통과 처리하지 않으며 wrapper preflight가 nonzero이면 Unity/MCP/build를 시작하지 않는다.

preflight는 machine-readable capability profile, criterion별 attempt ledger, packet-only agent brief, current-state run/fingerprint/status/cost, QA C# 안전성, component contract 영향, task-scoped isolated cache marker를 대조한다. profile 허용 목록 밖의 status와 route 기대 상태가 아닌 status를 차단하고, 실제 실행 전 profile에 허용된 `ready-for-verification` → `verification-running` 전이만 적용한다. 알려진 실패·미지원 route, Reflection/private reflection QA 코드, Rigidbody 위치 변경과 Y-sort 사이 sync 누락, collider/resolver 과거 타입 기대, full-history 위임, 필수 파일 3개 초과, stale 상태는 실행 전에 차단한다.

route/capability와 각 preflight guard의 실제 실패는 high-cost 시작 전에 criterion, 내부 run ID, fingerprint, route, 원인과 함께 attempt ledger에 원자 기록한다. 같은 run identity의 중복 failure는 추가하지 않는다. preflight 차단은 실제 Unity/MCP/build 시작이나 사용자-facing run 번호가 아니다. 같은 criterion의 연속 실패는 최대 2회이며 세 번째 호출은 retry-budget guard에서 차단하되 이 차단 자체를 새 failure로 기록하지 않는다.

재분류는 실제 failure 2회 뒤에만 허용하고 `root_cause`, `change_plan`, 새 위험 등급, reclassification ID를 별도 원장 필드로 기록한다. 재분류 기록은 실패 이력을 삭제하지 않고 새 분류 경계를 남긴다.

격리 Unity cache는 work ID별 marker가 있는 canonical cache root의 엄격한 하위 경로만 사용한다. `Assets`, `Packages`, `ProjectSettings`는 SHA-256 내용 기준으로 증분 동기화해 같은 크기·timestamp의 변경도 반영하고 `Library`는 재사용한다. cleanup은 work ID, source, instance canonical path가 marker와 일치할 때만 허용한다.

`Invoke-UnityEditModeTests.ps1` Run은 low-level이며 wrapper가 격리 cache 아래 발급한 만료형 one-shot token 없이는 시작할 수 없다. token 누락·만료·소비 후 재사용은 Unity process 시작 전에 차단한다. `-ValidateResultsOnly`만 token 없는 저비용 호환 경로로 유지한다.

## artifact budget와 canonical evidence

- criterion 하나당 기본 canonical 증거 1개를 둔다. 여러 파일이 필요하면 manifest 하나가 묶음을 소유한다.
- 중간 실패는 결함명, 최소 반례, 핵심 로그 위치만 보존하고 같은 의미의 전체 로그·PNG·CSV 세대를 반복 보관하지 않는다.
- 최종 QA는 canonical run_id, evidence manifest, superseded run 목록을 명시한다. `final`, `final-v2` 같은 파일명만으로 유효성을 판정하지 않는다.
- 대형 로그·빌드·캐시는 `_workspace`에 두지 않는다. 사용자 보고에는 직접 판단할 canonical 파일만 선별한다.

## 비용 계측과 중앙 현황판 동기화

정확한 토큰 수와 금액은 플랫폼이 작업별 계측값을 제공할 때만 기록한다. 계측값이 없으면 추정값·가상 금액·절감률을 만들지 않고, 역할·인계·Unity/MCP/빌드 시작·테스트·matrix/capture·correction·무효/폐기 증거 같은 관찰 가능한 비용 proxy를 `docs/project-handoff/task-cost-dashboard.md`에 기록한다.

- 작업 시작: 조정자가 R등급, 계획 역할·인계, 표적 검증, Unity/MCP/빌드, full suite, matrix/capture와 artifact 예산으로 중앙 행을 만든다.
- 실행 중: 구현자와 QA가 실제 실행 수와 `run_id` 근거를 제공한다. first blocker, correction, no-result 실행, 무효·폐기 증거가 생길 때마다 조정자가 실제값을 갱신한다.
- 사용자 보고·완료·커밋 전: 독립 QA가 중복·폐기와 필요한 비용/회피 가능 비용을 분류하고, 조정자가 `정상 / 주의 / 과다 / 미집계` 판정을 동기화하며, 총괄이 근거 충분성을 감사한다.
- `정상`: 계획된 역할·검증·산출물 예산 이내이며 이유 없는 중복·폐기가 없음. `주의`: 정당화된 초과 또는 correction 1회. `과다`: 같은 fingerprint full suite 중복, first blocker 뒤 고비용 계속, no-result Unity, correction 2회 미재분류, 이유 없는 추가 역할·인계, 비원자 증거 폐기 중 하나 이상. `미집계`: 근거 부족이며 0으로 간주하지 않는다.
- R1은 `task-r1-summary.md`의 5줄 이하 비용 기록을 쓰고, R2/R3는 `task.md`의 계획/실제 표를 blocker·correction 때 같은 파일에서 갱신한다. 별도 per-task 비용 파일은 만들지 않는다.

비용 `과다`는 실패나 미검증을 자동 통과시키는 예외가 아니다. 원인·회피 가능 비용을 공개하고, correction 2회 조건이면 재분류가 끝날 때까지 다음 고비용 실행과 완료·커밋을 차단한다.

## 완료 전 필수 게이트

R1~R3 완료·커밋 전에는 다음이 모두 있어야 한다.

1. **작업 배정**: R1은 요약형 원증상·완료 주장·소유자·표적 테스트·금지 범위가 있고, R2/R3는 정식 S0 oracle·소유자·범위·금지 항목이 있다.
2. **담당 산출물**: 실제 수행·인계·판정이 `agent-activity.md`와 필요한 artifact에 기록됐다.
3. **독립 QA**: 현재 fingerprint에서 원증상과 필요한 S단계를 독립 검증했고, 무효 증거를 제외한 `verification.md`가 있다.
4. **상태판 동기화**: 현재 상태, 후보·보류 중복, 완료 경로, Git 상태를 QA가 독립 대조했다.
5. **비용 현황판 동기화**: 계획·실제 proxy, correction·무효/폐기, 필요한 비용/회피 가능 비용과 비용 판정이 중앙 행에 현재 근거로 갱신됐다.
6. **총괄 최종 판정**: QA 충분성, revision 일치, 승인 범위와 사용자 수용 대기를 확인해 `내부 승인 가능`을 판정했다.

Unity 플레이어블 변경은 QA가 가능한 범위의 MCP Play를 수행하거나 불가 사유를 남긴다. 총괄 관리자는 MCP 실행자가 아니라 현재 QA 증거와 게이트의 감사자다.

## 커밋·완료 차단 조건

다음 중 하나라도 해당하면 커밋·완료 보고를 중지한다.

- R1~R3 작업 패킷, production 소유자, 독립 QA 또는 총괄 최종 판정이 없다.
- 사용자 원증상·합성 oracle·criterion→evidence 연결이 없다.
- 원인 레이어와 negative control 증명 없이 renderer/object disable, alpha 0, 이동·입력 우회, error swallow, 과대 invisible collider 또는 hidden-output 기대 테스트로 증상만 가렸다.
- 첫 blocker 뒤 고비용 검증을 계속했거나, 수정 뒤 이전 PASS를 유효하게 재사용했다.
- 현재 후보와 증거 fingerprint가 다르거나 canonical run_id가 둘 이상이다.
- Unity 증거에 lease owner가 없거나 stale/중복 객체가 포함됐다.
- correction cycle 2회 뒤 재분류하지 않았다.
- 검증 실패·미검증 사용자 수용 항목을 `완료`로 표현해야 한다.
- 작업 비용 중앙 현황판 행이 없거나 blocker·correction·사용자 보고·커밋 전 실제값과 판정이 동기화되지 않았다.
- 상태판과 실제 작업·Git 상태가 다르다.

## 문제 사안과 사용자 보고

```text
문제:
영향:
선택지:
추천:
확인할 파일:
```

최종 보고에는 `기술 검증 통과`, `사용자 수용 대기`, `내부 승인 가능`, `완료`를 구분하고 사용자가 직접 확인할 canonical 파일만 제시한다.
