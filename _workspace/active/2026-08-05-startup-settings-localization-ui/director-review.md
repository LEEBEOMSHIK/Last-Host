# 프로젝트 총괄 관리자 사전·최종 판정

## 기본 정보

- 작업 ID: `2026-08-05-startup-settings-localization-ui`
- 판정 단계: R3 구현 전 총괄 사전 게이트
- 검토 자료: `task.md`, `artifacts/architecture-review.md`, `verification.md`
- 검토 방식: 지정 문서 3개의 범위·아키텍처·S0 계약 대조
- 동적 검증: Unity/MCP/build/TestRunner 실행 없음

## 판정

**내부 승인 가능(구현 시작 가능)**

이 판정은 구현 착수만 허용한다. 기술 검증 통과, 사용자 화면 수용, 최종 완료 또는 커밋 가능 판정이 아니다.

## 판정 근거

1. 사용자 승인 범위가 PC Startup 첫 화면, 한국어/영어 준비형 UI, 화면 모드·해상도·VSync 설정, `RatHost2DPrototype` 시작 전이로 한정되어 있다. 신규 패키지·외부 폰트·아트·오디오, 추가 언어, 기존 HUD 다국어화, 음량·이어하기·키 재지정은 금지 범위로 분리되어 있어 범위 확장이 없다.
2. 새 시작 씬과 화면 상태 전이, PlayerPrefs 저장 형식, `EditorBuildSettings.asset` 변경이 결합되므로 R3 분류가 타당하다. production owner와 검증 주체가 분리되어 있고 R3 사전·최종 총괄 게이트도 유지된다.
3. S0 r0의 C3/C5, r1의 C4/C6 blocker는 historical 기록으로 보존되어 있으며 `startup-settings-s0-default-profile-r2-20260805` 재분류 뒤 r2가 PASS다. 현재 correction cycle은 `0/2`, 동적 실행은 0회다.
4. C1~C9는 Startup 초기 상태, 정확한 2D 씬 전이, Draft preview/apply/cancel, 결정론적 기본값, 키 완전성, 저장 수명주기, 960×540 가독성, 보호 diff, 종료·Esc까지 구현 후 판정 가능한 최소 증거와 연결되어 있다.
5. 기본 프로필은 `Korean`, `FullScreenWindow`, VSync `1`, `1920×1080` 우선으로 고정됐다. 미지원 시 지원 16:9 최고→지원 최고→현재 화면값 순의 해상도 fallback을 사용하고, 최초·부분 키·손상·미지원 저장값은 전체 기본 프로필로 원자 복귀한다. `기본값`은 Draft만 바꾸며 Apply는 검증→화면 적용→전체 저장 순서다.
6. 한국어 표시는 기존 Production2D 증거와 승인 내장 `LegacyRuntime.ttf` 재사용 조건으로 아키텍처 PASS다. Startup은 같은 폰트를 명시적으로 재사용해야 하며 신규·외부 폰트 추가는 이 승인에 포함되지 않는다.
7. Build Settings 변경은 `Startup`을 index 0에 두고 `RatHost2DPrototype`을 시작 대상으로 포함하는 최소 diff로 제한된다. `RatHostPrototype`, 기존 2D 핵심 루프, 기술 샘플, 패키지, reference는 수정·삭제하지 않고 보호 diff 대상으로 유지된다.

## 구현 착수 조건

1. **게임플레이 구현 owner**가 순수 설정 모델, Draft, 결정론적 기본값, 저장소, 한·영 키 카탈로그, 씬·종료 명령과 C2~C6·C9 EditMode 테스트를 먼저 구현한다. 이 단계에서는 씬과 Build Settings를 변경하지 않는다.
2. 코드와 최소 API 인계가 확인된 뒤 **Unity 씬/통합 구현 owner**가 `Startup.unity`, UI 바인딩, 960×540 레이아웃, 승인 내장 폰트 연결, Build Settings 최소 diff를 순차 적용한다.
3. production 소유권을 교차하지 않는다. 기존 3D/2D 씬·게임플레이 코드·패키지·아트는 변경하지 않는다.
4. 추가 패키지·외부 폰트·아트·오디오·지원 언어 확대가 필요해지면 구현을 확대하지 말고 별도 사용자 승인을 받는다.

## 후속 게이트

- 독립 QA는 같은 freeze candidate에서 C1~C9, PlayerPrefs 수명주기, 한·영 전체 키 표시, 대표 Play smoke, Build Settings와 레거시 보호 diff를 확인해야 한다.
- 고비용 검증은 승인된 wrapper preflight와 capability route를 따라야 하며 low-level runner 직접 실행은 금지한다.
- QA 기록과 작업 패킷·현황판·비용 대조가 끝난 뒤 프로젝트 총괄 관리자가 R3 최종 판정을 별도로 수행한다.
- 총괄 최종 내부 승인 뒤에도 시작 화면 첫인상, 한·영 전환, 설정 UI 가독성, 2D 프로토타입 시작 흐름은 사용자 수용 대기 상태로 남는다.

## 최종 메모

현재 사전 계약에는 사용자 결정을 다시 요구할 충돌이나 구현 착수 blocker가 없다. 위 소유권 순서와 보호 경계를 지키는 조건으로 구현을 시작할 수 있다.

---

## R3 최종 read-only 감사

- 감사 대상 후보: `d10c8cae0d0908828c038c5f2e689e32c765bf09659360cbe5a3915f33b8eb57`
- canonical run: `startup-settings-qa-20260805-001`
- QA 상태: `independent-qa-pass-awaiting-director`
- 동적 증거: UnityEditMode `32/32 PASS`, Unity `1` / MCP `0` / build `0`
- 감사 중 재실행: 없음

### 최종 판정

**수정 필요 — 기술 증거는 통과 수준이나 최종 상태·비용 기록이 canonical 증거와 불일치한다.**

이 판정은 production 결함이나 QA 결과 무효 판정이 아니다. 아래 상태-only 기록을 동기화하기 전에는 `내부 승인 가능 — 기술 검증 통과, 사용자 화면 수용 대기`로 올리거나 완료·커밋을 보고할 수 없다.

### 기술·범위 감사 결과

1. `verification.md`, `verification-current-state.json`, `verification-attempt-ledger.json`, `qa-target-results.xml`은 같은 fingerprint와 canonical run을 가리킨다. XML은 EditMode `32/32` PASS, failed/skipped/inconclusive `0`, Unity exit 성공 근거를 남긴다.
2. stale evidence run_id preflight 차단은 Unity 시작 `0`회에서 중지됐고 같은 후보의 메타데이터만 동기화한 뒤 Unity `1`회로 통과했다. correction `1/2`이며 반복 full suite, matrix, MCP, build는 없다.
3. isolated cache 최초 ShaderGraph `GUID` CS0246는 package refresh와 LastHost runtime/test 재컴파일 뒤 표적 테스트가 통과한 순서로 QA가 분리했다. 같은 최종 어셈블리의 32/32 PASS를 무효화할 근거는 없고, cold-cache 복구 지연 위험만 남긴다.
4. 구현 범위는 Startup UI, 한국어/영어 키, 화면 모드·해상도·VSync, PlayerPrefs 묶음 저장, `RatHost2DPrototype` 진입과 Build Settings 최소 연결 안에 있다. 신규 패키지·외부 폰트·아트·오디오·추가 언어 확대 증거는 없다.
5. 보호 diff와 씬 계약 테스트는 `RatHostPrototype`, 2D 핵심 씬, package, reference 보존을 지지한다. renderer/object disable, alpha 0, 입력·이동 우회, error swallow, 과대 invisible collider, hidden-output 기대 테스트 같은 증상 은폐 증거도 없다.
6. 실제 Play/MCP 화면·입력·한영 preview·960×540 clipping·scene transition·standalone 종료와 사용자 화면 수용은 아직 미검증이다. 따라서 기술 자동 검증 범위를 넘어선 `완료` 표현은 금지한다.

### 최종 게이트 blocker

- `task.md` 상태와 비용 실제값이 여전히 게임플레이 코드 구현·동적 검증 `0` 단계에 머문다.
- `handoff.md`가 canonical run 없음, PowerShell 7 blocker, UnityEditMode 미시작 상태를 가리킨다.
- `current-task-board.md`가 Startup 씬·UI 통합 단계만 표시하고 현재 QA PASS·사용자 수용 대기를 반영하지 않는다.
- `task-cost-dashboard.md`가 구현·QA·총괄을 대기, Unity/MCP/build `0/0/0`, 비용 `미집계`로 기록한다. 실제값 Unity `1` / MCP `0` / build `0`, preflight correction `1/2`와 그에 따른 비용 판정을 반영해야 한다.

### 필요한 수정

조정자는 production·테스트·씬을 바꾸거나 Unity를 재실행하지 말고 위 네 상태 문서와 작업 이력 요약을 동일 fingerprint/run 기준으로 동기화해야 한다. 비용 판정에는 실제 Unity 1회와 stale evidence 메타데이터 correction 1회를 구분해 필요한 비용·회피 가능 비용을 기록한다. 동기화 뒤 read-only 재대조에서만 최종 `내부 승인 가능 — 기술 검증 통과, 사용자 화면 수용 대기` 여부를 확정한다.

---

## R3 상태-only 최종 재대조

- 재대조 자료: 최신 `task.md`, `handoff.md`, `_workspace/active/CURRENT.md`, `current-task-board.md`, `task-cost-dashboard.md`, `agent-activity.md`
- production/test/scene/fingerprint 변경: 없음
- 새 QA·Unity/MCP/build 실행: 없음

### 최종 판정

**내부 승인 가능 — 기술 검증 통과, 사용자 화면 수용 대기**

1. 상태 문서는 canonical run `startup-settings-qa-20260805-001`과 fingerprint `d10c8cae0d0908828c038c5f2e689e32c765bf09659360cbe5a3915f33b8eb57`를 동일 후보로 유지한다.
2. 표적 UnityEditMode `32/32 PASS`, Unity/MCP/build `1/0/0`, preflight correction `1/2`가 작업 패킷·handoff·공유 현황판·비용 현황판·작업 이력에서 일치한다.
3. stale evidence run_id preflight 차단은 Unity 시작 `0`의 회피 가능 비용으로, canonical Unity 1회는 필요한 비용으로 분리됐다. full suite·matrix·capture·MCP·build·중복 Unity 실행은 추가되지 않았다.
4. 이전 총괄 `수정 필요`의 원인이었던 구현 전 상태, canonical run 없음, 동적 검증 0, 비용 미집계 표시는 현재 기술 검증 통과·사용자 화면 수용 대기 상태로 동기화됐다.
5. 실제 Play/MCP 화면·입력·한영 전환·960×540 가독성·scene transition·standalone 종료는 사용자 확인 대상으로 남는다. 이 판정은 해당 수용을 대신하지 않으며 사용자 확인 전 `완료`를 허용하지 않는다.

이 재대조로 총괄 최종 게이트는 통과한다. 다음 단계는 재검증이 아니라 사용자의 Startup 첫 화면, 한영 전환·취소, 설정 UI 가독성, `프로토타입 시작` 2D 진입 수용 확인이다.

---

## 2026-08-07 선택 배경 통합 감사와 상태 교정

- 최초 판정: 기능·QA blocker 없음. 다만 task/handoff/CURRENT/board/cost/manifest가 이전 `32/32` 후보를 가리켜 상태-only `수정 필요`.
- 최신 canonical 증거: `startup-background-qa-20260807-002`, fingerprint `be3e9ce5a76ff6951272a6a191a89018a7f28eeef182b30df878495c750d3649`, UnityEditMode `33/33 PASS`, Unity exit `0`.
- run001은 다중 QA harness 경로 binding preflight 차단으로 Unity 시작 `0`이며 run002가 대체한다.
- 배경 revision correction `1/2`; 작업 누적 preflight incident 2건은 서로 다른 revision으로 분리한다.
- 작업 누적 Unity/MCP/build `2/0/0`, 이번 revision `1/0/0`, full/matrix/capture `0`.
- McpPlay unavailable. 실제 960×540 첫 프레임·메뉴/설정 가독성·버튼 입력·2D 전이는 사용자 수용 대기다.
- 본 절은 총괄 최종 판정이 아니라 상태-only 교정 기록이며, 교정 후 read-only 재대조가 필요하다.

### 선택 배경 통합 최종 재대조

**내부 승인 가능 — 기술 검증 통과, 실제 960×540 사용자 화면 수용 대기**

- 17-input manifest 재계산 `Match=True`, path/bytes/hash 불일치 `0`.
- canonical UnityEditMode `33/33 PASS`, Unity exit `0`.
- 작업 누적 Unity/MCP/build `2/0/0`, 배경 revision `1/0/0`, correction `1/2` 정렬.
- production/test 추가 변경·Unity 재실행 없이 상태-only 동기화와 `git diff --check`를 통과했다.

---

## 2026-08-07 실제 Play·언어별 폰트 correction 감사

- 기술 후보: `startup-play-font-qa-20260807-001`, 34-input fingerprint `22eef3ed...531a`, UnityEditMode `38/38 PASS`, Unity exit `0`.
- 최초 상태-only 판정: production blocker 없음. manifest scene category와 공유 상태·비용이 이전 33/33 후보를 가리켜 `수정 필요`.
- 교정: 보호 scene 3개 category를 canonical 계산의 `scene`으로 정렬해 manifest 재계산 `Match=true`, bytes/hash `34/34`.
- 비용: 작업 누적 Unity/MCP/build `3/0/0`, 현 revision `1/0/0`, full/matrix/capture `0`; S0 계약 correction `1/2`, 동적 QA/preflight correction `0/2`.
- 현재는 상태-only 재대조 대기이며, 실제 960×540 first frame·배경·언어별 폰트·입력/전이는 사용자 수용 대기다.

### 실제 Play·언어별 폰트 최종 재대조

**내부 승인 가능 — 표적 자동 기술 검증 통과, post-correction 실제 Play/사용자 화면 수용 대기**

- manifest 34/34 hash·bytes 일치, computed=declared fingerprint `Match=true`.
- canonical UnityEditMode `38/38 PASS`, Unity exit `0`.
- 최신 상태·비용·correction·SUPERSEDED 경계가 task/handoff/CURRENT/board/cost와 일치한다.
