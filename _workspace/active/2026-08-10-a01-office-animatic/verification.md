# 검증 기록

## 작업 ID

`2026-08-10-a01-office-animatic`

## 검증 대상

- 승인된 A01 회사 일상 혼합형 모션의 독립 무음 Unity 애니매틱
- 현재 상태: **기술 검증 통과 — 사용자 시각 수용 대기**. current canonical run은 `a01-target-green-006`, fingerprint는 `de4f3d9b147b1a44722bea6021389569b46694214f8c6034cd47491bf8fbaa50`다.

## 검증 담당

- 구현 전 S0: QA/검증 에이전트
- 구현 후 canonical 검증: 구현 주체와 분리한 QA/검증 에이전트
- 내부 승인 판정: 프로젝트 총괄 관리자 에이전트

## 실제 수행·검증 이력

| 역할/에이전트 | 실제 수행 | 산출물·판정 |
| --- | --- | --- |
| 프로젝트 조정 | R2 최소 패킷 생성, 승인 명세와 구현·검증 경계 정리 | 구현 계획 작성 단계 |
| Unity 아키텍처 | 필수 기준·asmdef·Timeline·Startup 분리 구조 검토 | production 미변경, 공유 계획 파일 작성 전 지연으로 조정자가 통합 |
| 프로젝트 총괄 | 구현 계획 v1 범위·lease·state·fingerprint·imagegen·승인 게이트 감사 | `director-a01-office-animatic-plan-final-audit-001`: 수정 필요, blocker 5건 |
| 프로젝트 조정 | 총괄 blocker 5건과 비차단 위험 보정 | 계획 v2 |
| 프로젝트 총괄 | 계획 v2에서 감사 001 blocker 해소 여부 재대조 | `director-a01-office-animatic-plan-final-audit-002`: 수정 필요, stale evidence·lease identity 2건 |
| 프로젝트 조정 | 감사 002 blocker 보정 | 계획 v3, 최종 재감사 대기 |
| 프로젝트 총괄 | 계획 v3에서 감사 002와 기존 blocker 회귀 대조 | `director-a01-office-animatic-plan-final-audit-003`: 보류/재분류 필요, 신규 folder `.meta` candidate identity 1건 |
| 프로젝트 조정 | R2 2/2 뒤 R3 재분류와 folder `.meta` 한정 보정 | `a01-plan-reclass-001`, 계획 v4 |
| 프로젝트 총괄 | 계획 v4에서 folder `.meta`, 경로·state·lease·승인 게이트 재대조 | `director-a01-office-animatic-plan-final-audit-004`: 수정 필요, GREEN·frozen wrapper의 실제 `ProductionPath` 명시 1건 |
| 프로젝트 조정 | GREEN·frozen full wrapper를 완전한 명령으로 확장하고 R3 표기 보정 | 계획 v5, R3 correction 1/2 |
| 프로젝트 총괄 | 계획 v5 최종 회귀 감사 | `director-a01-office-animatic-plan-final-audit-005`: PASS, blocker 0건·비차단 위험 2건 |
| 독립 QA/검증 | raw checkout 관찰값, source/base/HEAD blob provenance, lone-CR 검사와 LF-normalized SHA-256 대조 | `qa-a01-execution-s0-line-ending-audit-001`: C06 canonical PASS, `execution-s0-001` raw 오탐 SUPERSEDED |
| 프로젝트 조정 | HIGH API compatibility/stale-state review failure 보정과 자체검증 | R3 plan correction 2/2 도달 후 `a01-plan-reclass-002`; 당시 독립 재리뷰 대기, 후속 Task 1 review CLEAN |
| Task 1 독립 리뷰 | `a5a4cf8..dc497b3`의 S0·보호 기준·승인 상태 재대조 | CLEAN; canonical protected baseline `5/5` PASS, imagegen/Unity/high-cost 당시 0 |
| ChatGPT 이미지 아트 | initial background/cast/foreground 3회와 Correction A 1회 호출 | initial usable 3개; Correction A는 `unsupported image image/png` no-result, 전체 invocation `4/6` |
| 비주얼/테크아트 | initial source 3개 독립 시각 검토와 foreground threshold 진단 | 3개 모두 REJECT; hard threshold `<=96`에 안전값 없음, `96`에서 mug handle 침식 시작 |
| 프로젝트 조정 | 사용자 승인 옵션 1과 로컬 실행 가능성 진단을 current plan에 통합 | current plan correction `1/2`; Python 3.11 Pillow 없음·worktree `.venv` 없음·helper exit 1, PowerShell 7 `System.Drawing` PNG smoke PASS |
| 독립 계획 리뷰 | 옵션 1 개정의 counter·과거 단계·P5 prop·tool identity 대조 | `a01-option1-plan-review-001`: FAIL — Important 4건; implementation/imagegen/Unity 실행 없음 |
| 프로젝트 조정 | reviewer 4건만 보정하고 R3 plan cycle 재분류 | `a01-plan-reclass-002` current plan `2/2` → `a01-plan-reclass-003` new current plan `0/2`; 독립 재검토 대기 |
| ChatGPT 이미지 아트 | invocation 5 background, invocation 6 cast spatial-only correction 실행 | background `1672×941`, SHA `DA5F…AA0C`; cast `1122×1402`, SHA `C3BD…E44AD`; historical imagegen `6/6` |
| 비주얼/테크아트 | 두 correction 결과 독립 시각·규격 검토 | `a01-imagegen-correction-visual-review-002`: background PASS, cast REJECT; 첫 결정적 blocker에서 중단 |
| 독립 계획 리뷰 | attempt 07 승인 절차의 spec compliance/plan quality 대조 | initial review FAIL `1/2`: old active rejected-canonical/foreground 경로 및 versioned 경로·prompt 자급성 finding |
| 독립 계획 재리뷰 | scoped attempt 07 plan 대조 | scoped re-review FAIL `2/2`: old block의 PowerShell-invalid `REM`, raw 자체 계산 expected SHA로 provenance 독립성 상실 |
| 프로젝트 총괄 | attempt 07 current plan correction 2/2 재분류 판정 | `a01-plan-reclass-003` → `a01-plan-reclass-004`; R3 유지, new current plan `0/2`, 구현 보정 금지 |
| 독립 계획 리뷰 | reclass-004 initial plan 대조 | FAIL `1/2`: historical old Copy-Item recipe runnable, Stage B RED/GREEN PowerShell variable scope 비자급 |
| 비주얼/테크아트 독립 리뷰 | attempt 07 raw 실제 크기·grid boundary 대조 | REJECT: 모든 y band의 horizontal equal-grid boundary collision; identity·standing·bags 의미는 대체로 PASS, global nearest-neighbor로 해결 불가 |

- 검증 에이전트: 실행 선택 뒤 배정; 현재는 총괄 계획 감사만 수행
- 검증 요청자: 프로젝트 조정 에이전트
- 검증한 산출물: 구현 계획 v1~v4 총괄 감사와 계획 v5 보정; Unity production 결과는 없음
- 조건부 R3 분리 이력 생성 사유·반영 여부 / 없으면 `미생성`: 미생성

## 입력 자료

- `docs/design/narrative/opening/a01-office-hybrid-motion-design.md`
- `_workspace/active/2026-08-10-a01-office-animatic/task.md`

## 원래 증상 또는 완료 주장

- 정지 후보 이미지 단계인 A01을 제한 프레임·분리 레이어·절제된 카메라가 결합된 재생 가능한 독립 무음 애니매틱으로 검증한다.
- 사용자 실제 재생 확인 전에는 완료를 주장하지 않는다.

## Task 1 실행 시작 기준

- worktree / branch / base: `C:/projects/Last-Host/.worktrees/a01-office-animatic` / `feat/a01-office-animatic` / `a5a4cf8121a52f4d2a1c3ceb537db181bd141f4e` (`a5a4cf8`)
- 시작 Git 상태: `M docs/project-handoff/current-task-board.md`, `M docs/project-handoff/task-cost-dashboard.md`, `?? _workspace/active/2026-08-10-a01-office-animatic/`, `?? docs/superpowers/`; 기존 A01 계획 준비 범위만 존재하며 Unity production 파일 변경은 없다.
- 비용 시작값은 imagegen `0/6`이었다. initial `3` + no-result `1` + background/cast correction `2`의 historical ledger `6/6`은 보존하며, Cast attempt 07 실행 뒤 actual imagegen은 `7/7`, remaining 0이다. Unity starts `0`, Editor Play `0`, build `0`. R2 plan `2/2` → `a01-plan-reclass-001` → R3 plan `2/2` → `a01-plan-reclass-002` → option1 current plan `2/2` → `a01-plan-reclass-003` current plan `2/2` → `a01-plan-reclass-004` current plan correction `2/2` → `a01-plan-reclass-005` current plan correction `2/2` → `a01-plan-reclass-006` current plan correction `2/2` → `a01-plan-reclass-007` current plan correction `0/2` **CLEAN**, execution S0 correction `0/2`, production/test correction `2/2` → `a01-repack-implementation-reclass-001` `2/2` → `a01-repack-implementation-reclass-002` current `0/2`, independent visual QA correction `1/2`.
- artifacts: 실제 고비용 실행 전이므로 `artifacts/agent-brief.json`, `artifacts/verification-current-state.json`, `artifacts/verification-attempt-ledger.json`을 생성하지 않았다.

### 보호 파일 raw checkout 관찰값과 canonical 기준

raw checkout bytes/SHA-256은 Windows line-ending 환경 관찰값으로 보존한다. canonical pass/fail은 CRLF만 LF로 치환하고 lone CR를 FAIL로 하는 normalized bytes/SHA-256이며, blob OID는 source/base/HEAD provenance 보조값이다.

| 보호 파일 | raw checkout bytes | raw checkout SHA-256 | LF-normalized bytes | LF-normalized SHA-256 | source/base/HEAD blob OID | canonical |
| --- | ---: | --- | ---: | --- | --- | --- |
| `UnityProject/Packages/manifest.json` | 2069 | `B07DD4E37BA1336B93D763B23E3480BE7943EF4C56DBFDA7EE191FF87B0AF298` | 2069 | `B07DD4E37BA1336B93D763B23E3480BE7943EF4C56DBFDA7EE191FF87B0AF298` | `984c4b254df4b24250d43b41c2d2b258507a8fdf` | PASS |
| `UnityProject/Packages/packages-lock.json` | 13840 | `943F92F1229C2A366FD42AA7180B73BDB8B6019AE21C1A6CE38C80A15D8C262E` | 13840 | `943F92F1229C2A366FD42AA7180B73BDB8B6019AE21C1A6CE38C80A15D8C262E` | `466e4738b58517431fa8939ac222f4eacdb28277` | PASS |
| `UnityProject/ProjectSettings/EditorBuildSettings.asset` | 799 | `67B153F8C73C6C9E7F8C60D47D03A837DFEC207E757AC65FEB6619F58BE28755` | 799 | `67B153F8C73C6C9E7F8C60D47D03A837DFEC207E757AC65FEB6619F58BE28755` | `b346dc3a522aa57c945e71a18a8caf6ab16a94a0` | PASS |
| `UnityProject/Assets/_Project/Editor/Startup/StartupPlayModeBootstrap.cs` | 1390 | `B2158D49FE9EC1E8069BD3C7924299164869D3F94D4DB6766DD3D155CC581EF9` | 1346 | `634BD355DF765B7283774D3B20983299F2637C8F0503B831057535F58133E5C2` | `bf6a9f7c04a40f587c14da2c2a4a7f9423eb1577` | PASS |
| `UnityProject/Assets/_Project/Scripts/UI/Startup/StartupController.cs` | 15511 | `58FAEF9D089A7DBDF3B0FF60EC31B1CC6F5F5729E737E55FD4B3977F7881A6B3` | 15040 | `042B816E531448ABD5DC265C183D309AE1E084E25581E8DF9D4E48FE73931730` | `6e615f7f570727a2793d1d686dc80aa7c67da0f8` | PASS |

### S0 QA 정적 검토

- `execution-s0-001`: raw checkout SHA-256 직접 대조로 Startup 두 파일을 오탐했다. 비독립 self-check이므로 `SUPERSEDED`다.
- 독립 감사: source/base/HEAD blob OID가 모두 같고, 5개 파일은 lone CR 없이 LF-normalized canonical bytes/SHA-256가 계획값과 일치한다.
- 판정 (historical S0): Task 1 `착수 가능`·독립 리뷰 CLEAN은 역사로 유지한다. `a01-imagegen-correction-visual-review-002` Cast REJECT와 attempt 07 raw의 equal-grid REJECT도 provenance로 유지한다. `c551078`/`3bf4369`의 Task 2A Steps 2.1~2.8 scoped review와 `a01-plan-reclass-007` current plan `0/2`는 **CLEAN**, A01-RP-01~06 S0는 **착수 가능**이다. execution S0 `0/2`; Task 2A production/test는 `a01-repack-implementation-reclass-001` `2/2` 뒤 `a01-repack-implementation-reclass-002` current `0/2`, versioned derivative automatic PASS·independent visual QA correction `1/2`였다. 당시 Task 2B blocked, canonical/Unity production·high-cost `0`, 비용 `주의`였다.

### Task 2A scoped S0 계약 검토

- 검토 범위: `c551078`(Task 2A 독립화·external `pwsh`/Copy fail-fast 보정)부터 `3bf4369`(Active Task 2A Steps 2.1~2.8)까지의 계획 diff와 최종 설계.
- 합성 oracle: attempt 07 raw의 20 pose·동일 RGBA를 fixed-cut 소유권과 pose별 정수 `(dx,dy)` translation만으로 exact 4×5 hard-alpha derivative에 옮기며, 자동 계약과 독립 시각 QA 전에는 BG·FG·old canonical·Unity preview를 바꾸지 않는다.

| criterion ID | 잠근 exact oracle | S0 판정 |
| --- | --- | --- |
| A01-RP-01 | raw `1122×1402`, `1,648,495 bytes`, SHA `24A143D7344DAC8358CD496C6AD03718AADB492D67B96E7CCCF0E46DA08A090D`; 처리 전후 bytes/SHA 동일 | CLEAN |
| A01-RP-02 | half-open cuts `x=[0,281,561,842,1122]`, `y=[0,318,591,847,1107,1402]`의 20 rect가 full canvas와 all strong seed `d∞<=24`·4-neighbor `d∞<=48` enclosed-hole matte mask를 누락·중복 없이 정확히 1회 소유 | correction 계약 반영 |
| A01-RP-03 | source axes `x=[140,421,701,982]`, target cell-local `(160,306)`; all unmasked non-despilled core exact 1:1 mapping, authorized matte/despill only, 설명되지 않은 opaque pixel `0` | correction 계약 반영 |
| A01-RP-04 | output `1280×1600`, `4×5`, cell `320`, band `6`, coverage inclusive `0.05..0.60`, alpha `{0,255}`, transparent RGB `(0,0,0)`, repeated derivative SHA 동일 | CLEAN |
| A01-RP-05 | P1~P3 seated, P4/P5 standing/no chair, P4 black/P5 brown same-body-side commuter bag, 20 pose 의미, chair wheels·P3 laptop·bag straps·shoes 보존, 잘림·추가 prop·magenta fringe·cell jitter 없음 | final `6FA3F088…61EED1` independent Visual QA 1차 REJECT, correction `1/2`; RGB-only correction owner 재배정 가능 |
| A01-RP-07 | fixed candidate high-confidence boundary 후보 `4,554` → `0`; alpha/opaque 좌표 불변; non-candidate RGB 불변; safe donor는 위치와 무관하게 chroma 수치 비후보; deterministic SHA | targeted RED→GREEN PASS; fixture 충돌 `2/2` 뒤 reclass-001 current `0/2` |
| A01-RP-08 | BG+Cast+manifest Unity bundle, Cast 20 slices/공통 pivot/import settings, foreground blocked·raw 미복사 | pre-final static bundle PASS 뒤 final run004 Unity import QA PASS: BG Single/zero physics, Cast 20 slices·rect/pivot/import/zero physics shapes PASS |
| A01-RP-06 | BG `DA5F22DE7D1C9BDBABE2A8887640085142D23E02CF3BF94B21E217A7EC98AA0C`; FG `D782D38E4D510E1D13680C21D6642F86647DF53662B8D94150376EC73770F1E1`; old canonical `C3BD3E5F15CDA75F74AE13433D6C7C03E6D3BCC122E8A9A48B8AF1986B8E44AD`; Unity Cast preview baseline `MISSING`이며 dual QA PASS 전 모두 불변 | CLEAN |

- QA S0 verdict (asset-bundle historical): **착수 가능**. 이 asset 단계 판정은 후속 mask-only Scene 통합과 canonical run006으로 SUPERSEDED됐다.

## 현재 검증 revision

- 위험 등급: R3 scene integration — canonical run006 기술 PASS, 사용자 시각 수용 대기
- verification revision: mask-only Scene·Timeline·Animation·Preview와 path-based Startup 복원 oracle을 포함한 fingerprint `de4f3d9b…baa50`
- `a01-scene-integration-s0-002`: **PASS** — `a01-scene-integration-s0-001`의 두 blocker는 `SUPERSEDED`로 보존되고, reclass current `0/2`의 신규 test→RED→tool GREEN→actual derivative→독립 visual QA→조건부 Office foreground/meta/manifest/static/import 전이가 실행 가능하게 잠겼다. foreground recovery는 착수 가능하나 scene/Unity는 visual QA PASS와 static 전이 뒤에만 조건부로 시작한다.
- `a01-foreground-color-visual-qa-001/002`: **REJECT 2/2** — 두 automatic-PASS color derivatives 모두 monitor·plant·mug·pen·desk 외곽의 visible pink fringe가 남았다. same RGB cleanup 반복을 중단하고 `a01-foreground-occlusion-mask-reclass-001` current `0/2`의 mask-only architecture로 전환한다.
- current candidate fingerprint/run_id: `de4f3d9b147b1a44722bea6021389569b46694214f8c6034cd47491bf8fbaa50` / `a01-target-green-006`
- canonical base asset run004는 provenance로 유지하며 current Scene candidate는 run006 XML `18/18` PASS가 소유한다.
- candidate frozen 여부: 예 — run006 이후 production/test/scene 변경 없음
- 마지막 production 변경 식별값: fingerprint manifest `63` files; Scene·Timeline·Animation·Editor·test와 보호 dependency 포함
- 이 검증이 마지막 production 변경 이후 실행됐는지: 예
- current-state JSON 대조: `technical-pass`, current evidence 1개
- capability route / wrapper preflight: `UnityEditMode`, fresh short cache, wrapper exit0
- attempt ledger: run005 test-oracle failure 뒤 correction run006 success; 추가 retry 없음
- supplemental evidence/staging hygiene: `artifacts/qa/fingerprint-a01-office-assets-final-qa-004-supplemental.json`와 `artifacts/qa/evidence-a01-office-assets-final-qa-004-supplemental.json`는 parent metas byte-identical/run004 isolated cache + strengthened static test independent R1 PASS를 연결한다. supplemental Unity/high-cost `0`; status-only commit은 production/tools/assets/artifacts를 stage하지 않는다.
- static command 기록: `pwsh tools/art/Test-A01OfficeAssetBundle.ps1 -ProjectRoot .`는 존재하지 않는 `ProjectRoot` parameter 때문에 exit `1`; corrected no-arg `pwsh tools/art/Test-A01OfficeAssetBundle.ps1`는 PASS였으며 Unity/high-cost/correction은 소비하지 않았다.

## Unity single-owner lease

- lease owner: 없음
- editor PID / scene: 미획득 / 미실행
- 획득·해제 시각: 해당 없음
- baseline / final Play·Pause·scene·dirty: 구현 전 baseline에서 기록 예정
- 임시 객체 유무: 미확인

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 독립 QA가 production 작성 전 S0를 검토했으며 구현·후속 QA 주체는 계속 분리
- 구현 주체가 실행한 검증과 별도로 확인한 항목: `c551078`/`3bf4369` plan diff, final spec, Task 2A Steps 2.1~2.8, A01-RP-01~06 exact oracle

## 실행한 검증

| criterion ID | 유형 | 검증 방법 | run_id | 결과 | canonical 증거 | 유효/SUPERSEDED |
| --- | --- | --- | --- | --- | --- | --- |
| A01-C01~C10 | 계획 | S0 charter와 증거 예산 정리 | plan-005 | 실행 대기 | `task.md`, 구현 계획 v5 | 계획 기준 |
| PLAN-G1~G5 | 계획 감사 | lease/state/fingerprint/imagegen copy/명시 승인 대조 | director-plan-audit-001 | 수정 필요 → v2 보정 | 총괄 감사 001, 구현 계획 v2 | v1 판정 이력 |
| PLAN-G6~G7 | 계획 감사 | current-state evidence freshness·단계별 lease identity 대조 | director-plan-audit-002 | 수정 필요 → v3 보정 | 총괄 감사 002, 구현 계획 v3 | v2 판정 이력 |
| PLAN-G8 | 계획 감사·candidate identity | 신규 Unity folder `.meta` fingerprint·staging 대조 | director-plan-audit-003 | 재분류 필요 → R3 v4 보정 | 총괄 감사 003, 구현 계획 v4 | v3 판정 이력 |
| PLAN-G9 | 계획 감사·wrapper 실행성 | GREEN·frozen wrapper의 `ProductionPath` 실인자 대조 | director-plan-audit-004 | 수정 필요 → R3 v5 보정 | 총괄 감사 004, 구현 계획 v5 | v4 판정 이력 |
| PLAN-G10 | 계획 최종 감사 | 감사 001~004 blocker 해소·회귀와 실행 승인 경계 대조 | director-plan-audit-005 | PASS, blocker 0 | 총괄 감사 005, 구현 계획 v5 | 현재 계획 기준 |
| A01-C01~C10 | 실행 S0 정적 검토 | 계약·승인·worktree·raw 보호 파일 직접 대조 | execution-s0-001 | raw line-ending 오탐 | raw checkout 관찰값 | **SUPERSEDED — 비독립 self-check·raw 측정 오탐** |
| A01-C01~C10 | 독립 S0 감사 | source/base/HEAD blob provenance + lone-CR 검사 + LF-normalized canonical hash 대조 | qa-a01-execution-s0-line-ending-audit-001 | **착수 가능 — C06 PASS** | 이 문서의 canonical 표 | 현재 S0 기준 |
| A01-RP-01~06 | Task 2A 독립 S0 scoped review | final spec·`c551078`/`3bf4369` diff·Steps 2.1~2.8과 exact source/ownership/mapping/output/visual/protection oracle 대조 | qa-a01-task2a-s0-scoped-review-001 | **CLEAN — 착수 가능, blocker 0** | `task.md` Task 2A charter와 이 문서 S0 추적표 | 현재 S0 기준 |
| A01-C08 | imagegen initial + correction ledger | source dimensions/SHA와 tool invocation 대조 | a01-imagegen-task2-001 | initial 3, no-result 1, background/cast correction 2의 historical `6/6` 뒤 attempt 07 raw actual `7/7` REJECT — all y-band horizontal equal-grid boundary collision; derivative automatic PASS·visual fringe REJECT | `artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png`, Task 2 report | **Task 2A active — reclass-007 0/2 CLEAN; reclass-002 production/test 0/2; visual QA correction 1/2; Task2B blocked** |
| A01-C04/C08 | initial source 독립 시각 검토 | identity·chair anchor·grid·fringe·handle 대조 | a01-imagegen-visual-review-001 | **REJECT 3/3**, Unity import 차단 | 독립 시각 리뷰 판정 | correction 전 기준 |
| A01-C03/C04/C08 | corrected background/cast 독립 시각 검토 | exact canvas/cells·boundary·P4·literal chroma·anchor 대조 | a01-imagegen-correction-visual-review-002 | background **PASS**; cast **REJECT** — `1122×1402`, mod4=2/mod5=2, row collision, P4/bag drift, literal `#ff00ff` 13px/near-magenta 1,121,291px | 독립 시각 리뷰 판정 | **canonical first blocker** |
| A01-C04/C08 | 로컬 경로 feasibility | Python/Pillow/helper와 PowerShell/.NET PNG smoke | a01-alpha-path-diagnostic-001 | Python helper exit 1; `System.Drawing` PASS | TEMP 진단 출력, 계획의 tool contract | current plan 근거 |

```text
명령 또는 확인 방법: git worktree 상태/branch/base/status 확인, raw 관찰값 기록, lone-CR 검사와 LF-normalized canonical byte·SHA-256 및 source/base/HEAD blob provenance 대조, A01-C01~C10 S0 계약 대조
결과: raw 두 Startup 값의 Windows CRLF 관찰 차이는 canonical 계약에서 PASS이며 C06 오탐은 SUPERSEDED
해석: historical imagegen tool invocation `6/6` 뒤 attempt 07 raw로 actual은 `7/7`, remaining 0이다. raw는 모든 y band의 horizontal equal-grid boundary collision으로 independent REJECT됐으며 identity·standing·bags 의미는 대체로 PASS지만 global nearest-neighbor로 해결할 수 없다. fail-fast로 normalization tool/TDD·derivative·canonical·foreground·Unity는 0회이고 attempt 08은 금지한다. foreground source SHA `D782…F1E1`는 unchanged다.
```

## 검증하지 못한 항목

- attempt 07 raw의 exact grid·boundary·coverage·P4/P5 standing/same-side commuter bag·literal chroma 품질 — raw REJECT 이력은 보존하되 final repack candidate/visual QA는 PASS했다.
- 로컬 normalized-alpha cast의 품질 — PowerShell tool TDD·derivative·rect-meta final PASS까지 완료했으며 foreground는 미시작이다.
- A01 scene/Timeline 재생과 Startup 회귀는 별도 승인된 scene integration 전 미시작이다.
- Timeline 재생, 무음 가독성, 픽셀 안정성, Console 오류와 scene dirty
- 전용 프리뷰 종료 뒤 Startup play-mode 시작 씬 복구
- 사용자 실제 모션·구도·호흡 수용

## 실패 또는 경고

- 구현 계획 v1은 blocker 5건, v2는 2건, v3는 신규 folder `.meta` candidate identity 1건을 받았다. R2 plan correction 2/2 뒤 `a01-plan-reclass-001`로 R3 재분류했고, v4 감사의 GREEN·frozen wrapper 실인자 1건으로 R3 plan correction 1/2를 기록했다. HIGH review의 Unity NET Standard 2.0 API compatibility 정적 확인 누락과 승인 상태 동기화 불완전이 R3 plan correction 2/2가 되어 `a01-plan-reclass-002`의 새 plan cycle을 시작했다. change plan은 `SHA256.Create().ComputeHash`, 상태 단일화, 동일 protected contract 독립 재리뷰였고 후속 Task 1 review는 CLEAN이다. `execution-s0-001` raw line-ending 오탐은 execution S0 correction을 소모하지 않으며 독립 감사 PASS로 SUPERSEDED됐다.
- Task 1 review CLEAN과 `a01-option1-plan-review-001` 보정은 역사로 유지한다. `a01-plan-reclass-004` initial review FAIL `1/2`의 root cause는 `historical old Copy-Item recipe가 runnable로 남았고 Stage B RED/GREEN이 별도 shell call 사이의 PowerShell variable scope를 가정함`; change plan은 `historical recipe executable block 삭제 + RED/GREEN 각 block의 generation-log SHA provenance preflight 자급화`다. invocation 5 background correction은 `1672×941`, SHA `DA5F…AA0C`, independent visual PASS다. invocation 6 cast correction은 `1122×1402`, SHA `C3BD…E44AD`이며 exact canvas/cell 실패(mod4=2/mod5=2), row boundary collision, P4 standing/commuter bag drift, literal `#ff00ff` 13px·near-magenta 1,121,291px로 `a01-imagegen-correction-visual-review-002` REJECT다. historical imagegen `6/6`은 재실행하지 않으며 attempt 07 raw의 actual은 `7/7` REJECT다.
- 승인 범위는 built-in Cast attempt 07 단 **1회**, 누적 cap `7/7`이다. raw는 versioned path에 SHA와 함께 보존했고 derivative `31D43DF6CEE7B5B68400B4373760D825838BA7F756B2EAFFC10612E2051FBF3B`는 automatic PASS 뒤 independent visual QA REJECT로 보존한다. authorized matte/despill 보정과 독립 visual QA가 모두 PASS한 뒤에만 canonical/Unity를 승격한다. attempt 08, resize/crop/padding-only, 외부/API는 새 승인 없이 사용하지 않는다.
- 제공 Python helper는 현재 Python 3.11에서 Pillow 미설치로 exit `1`이고, helper 구현상 alpha `>=252`에는 despill을 적용하지 않는다. 신규 패키지를 설치하지 않고 `System.Drawing` 경로를 사용한다. hard threshold를 `96` 이상으로 높여 mug handle을 지우는 방식은 금지한다.

## fail-fast·무효화

- first blocker: 총괄 계획 감사 001의 실제 baseline·heartbeat·release 계약 누락
- blocker 발견 뒤 중지한 고비용 단계: 해당 없음
- correction cycle (pre-run004 historical): R2 plan 2/2 종료 → `a01-plan-reclass-001` → R3 plan 2/2 → `a01-plan-reclass-002` → option1 current R3 plan 2/2 → `a01-plan-reclass-003` current plan 2/2 → `a01-plan-reclass-004` current plan 2/2 → `a01-plan-reclass-005` current plan 2/2 → `a01-plan-reclass-006` current plan 2/2(Task2A/2B isolation) → `a01-plan-reclass-007` current plan 0/2(**CLEAN**) → `a01-repack-implementation-reclass-001` production/test 2/2(sourceCuts fixed preflight early REJECT + `SyntheticFull` old-error-string wrong reason) → `a01-repack-implementation-reclass-002` current 0/2 → independent visual QA correction 1/2(automatic PASS derivative bright-magenta-fringe REJECT); execution S0 0/2; 당시 high-cost 0
- 변경 뒤 무효화한 run/증거와 사유: 없음
- superseded_by: 없음
- S1~S5 한 revision 통과 여부: 미실행
- S6 전체 suite 실행 허용/실행 횟수: candidate freeze 뒤 1회 예정 / 0
- S7 대형 matrix 실행 허용/실행 횟수: 불필요 / 0
- low-level runner token / 직접 Run 차단 확인: wrapper만 사용하도록 계획에 고정 / 직접 Run 0
- isolated cache marker / Library reuse / cleanup 확인: 미실행

## 비용 실행 대조

| 비용 항목 | 계획 예산 | 실제 수·run_id/근거 | 정상/초과/미집계 | 필요한 비용/회피 가능 비용 |
| --- | --- | --- | --- | --- |
| 실제 역할·인계 | 아키텍처1→이미지1→비주얼1→Unity1→QA1→총괄1 | 계획·생성·가공·asset QA·scene owner·test owner·독립 QA 인계가 다수 발생 | 과다 | 다음 장면은 playbook의 단일 경로만 사용 |
| 표적 검증 | TDD RED1·GREEN1 | asset static PASS; canonical scene run006 `18/18` PASS; independent QA CLEAN | PASS | 사용자 화면 수용은 별도 |
| Unity/MCP/빌드 시작 | isolated Unity 최대3·Play1·빌드0 | wrapper recorded Unity `6` / MCP `0` / build `0`; 별도 builder/visible Editor 시작은 미집계 | 과다 | visible duplicate와 동일 후보 cache 재실행이 회피 대상 |
| full suite | frozen 후보1 | project full suite 0; A01 target namespace run006 `18/18` PASS | PASS | 현재 장면 범위에 full project suite는 실행하지 않음 |
| matrix/capture·artifact | historical imagegen 최대6 + 승인된 attempt 07 1회·캡처 최대3·대형 matrix0 | actual imagegen `7/7`, final asset·Scene·canonical XML 1개, capture 0 | 과다 | 사용자 Preview 전 자동 capture 추가 금지 |
| correction·무효/폐기 | R2 최대2 뒤 재분류 | 다중 plan/art/tool/wrapper/cache/scene/test correction과 run001~005 SUPERSEDED, run006 canonical | **과다** | 원인·회피 규칙을 공용 playbook에 통합 |

- 비용 판정: **과다 — 후속 회피 가능**
- 같은 fingerprint·목적의 환경성 Unity 재실행, visible duplicate Editor, 계획·이미지·foreground 반복이 확인됐다. run006 이후 추가 자동 재실행은 없으며 다음 시네마틱은 공용 playbook의 사전 체크를 통과한 단일 후보·단일 hidden Unity 경로만 허용한다.
- `docs/project-handoff/task-cost-dashboard.md` 갱신·독립 대조 여부: R3 행을 imagegen `7/7`, canonical run006 `18/18`, wrapper recorded high-cost `6`, 별도 Editor 시작 `미집계`, 비용 `과다 — 후속 회피 가능`, 사용자 시각 수용 대기로 동기화

## 최종 증거 원자성

- 대상 Root/instance count: EditMode 계약에서 exact root 1·camera 1·director 1·cast root 5 PASS
- stale·중복 player/controller/camera guard: exact hierarchy와 component count PASS
- 캡처와 sidecar의 run/fingerprint 일치: 미실행
- Console error count: fresh cache API Updater 전 transient package CS0246 이력 후 최종 compile·NUnit exit0; 사용자 main Editor Console은 Preview 수용 때 확인
- scene dirty before/after: 미실행
- evidence manifest: `artifacts/verification-current-state.json` current evidence 1개
- canonical evidence와 artifact budget 준수: final NUnit XML 1개만 보존, 대형 log는 bytes·SHA만 기록

## 게이트 판정

- QA/검증 게이트 통과 여부: canonical run006 `18/18` PASS와 독립 read-only QA CLEAN. 사용자 실제 Preview 수용은 별도다.
- 조건부 R3 분리 이력에 QA 판정 반영 여부 / 없으면 `미생성`: 미생성
- 총괄 관리자 검토로 넘길 수 있는지: 예 — current fingerprint·XML·QA CLEAN 준비 완료

## 프로젝트 총괄 관리자 판정

- 역사 판정: R3 구현 계획 v5는 `director-a01-office-animatic-plan-final-audit-005` PASS, blocker 0건이었다.
- 현재 판정: **내부 승인 가능 — 기술 검증 통과, 사용자 시각 수용 대기**. 총괄 최종 감사에서 current 63-file fingerprint 불일치 `0`, canonical XML `18/18`, 독립 QA CLEAN, 범위 보호와 비용 공개를 확인했고 blocker `0`이다.
- 승인 범위·사용자 수용 대기: 옵션 1 isolated worktree, actual imagegen `7/7`, attempt 08 금지다. 사용자 최종 시각 수용, 외부/API 생성 경로, A02, 오디오, Startup 연결은 별도 승인이다.

## 완료 판단

- Task 1·asset run004 이력과 후속 Scene run006을 연결했다. 기술 검증은 통과했지만 사용자 실제 재생 수용 전이므로 완료가 아니다.

## 사용자 수용 상태

- 사용자 직접 확인 필요: 실제 A01 무음 재생에서 대화·웃음·점심 이동 준비의 가독성, P1 자리·출입문 기억, 모션 호흡
- 확인 전 `완료` 표현 금지 여부: 예

## 완료 판단 근거

- attempt 07 actual `7/7`과 earlier fringe REJECT는 이력으로 보존한다. Mask-only final asset과 Scene candidate는 run006 `18/18`·독립 QA CLEAN을 통과했으며, 남은 완료 게이트는 총괄 감사와 사용자 실제 Preview 수용이다.

## 최종 상태

- **기술 검증 통과 — 사용자 시각 수용 대기.** canonical run006 `18/18`, independent QA CLEAN, imagegen `7/7`; 비용은 `과다 — 후속 회피 가능`이다.
- 완료 경로와 Git 상태: feature worktree 커밋과 main Unity 통합 전

## Task 2A 구현 소유자 TDD 증거

- RED command: `pwsh tools/art/Test-RepackChromaPoseGrid.ps1 -ToolPath tools/art/Repack-ChromaPoseGrid.ps1 -RealSourcePath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png -LayoutPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-layout.json -ExpectedRealSourceSha256 24A143D7344DAC8358CD496C6AD03718AADB492D67B96E7CCCF0E46DA08A090D`
- RED expected output: `Tool not found`
- RED observed output: `Tool not found`
- RED exit code: `1`
- GREEN blocker 1: test helper의 `Add-Type`가 PowerShell `7.6.4`의 `System.Drawing.Common` 전이 참조를 완전하게 전달하지 않아 `CS0012`로 중단됐다. 최초 최소 반례는 `IImage` / `System.Private.Windows.GdiPlus`, 이어진 같은 원인 조사에서 `IRawData` / `System.Private.Windows.Core`와 `Color` / `System.Drawing.Primitives` 누락을 확인했다. 실제 repack 처리와 real candidate에는 진입하지 않았다.
- GREEN blocker 1 root cause/change: 명시적 C# compile reference가 `System.Drawing.Common`만 포함해 런타임 의존 폐쇄와 달랐다. 최소 compile probe에서 `System.Drawing.Common`, `System.Drawing.Primitives`, `System.Private.Windows.GdiPlus`, `System.Private.Windows.Core` 조합을 확인해 test/tool의 같은 참조 목록에 반영했다.
- GREEN blocker 2: 참조 교정 뒤 synthetic manifest helper가 ordered hashtable 안의 괄호 없는 산술식을 `System.Object[]`로 평가해 `System.UInt32` 변환에서 중단됐다 (`New-SyntheticManifest`, 당시 line 247/249). 실제 repack 처리와 real candidate에는 진입하지 않았다.
- GREEN blocker 2 root cause/change: `width`, `height`, source cut, rect 좌표의 hashtable 산술식에 명시적 괄호가 없었다. 최소 shell 반례로 같은 변환 오류를 재현한 뒤 해당 test helper 식에만 괄호를 추가했다. correction 상한 도달로 교정 후 재실행하지 않았다.
- production/test correction cycle: `a01-repack-implementation-reclass-001` `2/2` — **`a01-repack-implementation-reclass-002` current `0/2`로 재분류**. 두 실패는 production 결함이 아니라 sourceCuts fixed preflight가 더 일찍 올바르게 거부했는데 `SyntheticFull` negative case가 옛 `Target cell`·`ownership` 오류 문자열을 고정해 wrong reason으로 실패한 것이다. change plan은 negative 기대를 preflight `target`·`sourceRect`로 맞추고, unreachable ownership overlap/gap defense-in-depth는 별도 정적/fixture 계약으로 분리한 뒤 `SyntheticFull` 1회부터 재개한다. 실제 구현/테스트·이미지·Unity, derivative 생성, automatic PASS, canonical/Unity 승격과 커밋은 그 전까지 중단한다.
- current source provenance: attempt 07 raw expected SHA-256 `24A143D7344DAC8358CD496C6AD03718AADB492D67B96E7CCCF0E46DA08A090D`; production/test 두 blocker는 real candidate 진입 전 발생했다.

## Task 2A 독립 visual QA 1차

- versioned derivative: `a01-office-cast-pose-grid-attempt-07-repacked-alpha.png`, `1280×1600`, SHA-256 `31D43DF6CEE7B5B68400B4373760D825838BA7F756B2EAFFC10612E2051FBF3B`; automatic contract PASS.
- 독립 visual QA: **REJECT** — P1~P5, 의자, 가방, 신발 외곽의 bright magenta fringe.
- 픽셀 진단: opaque `415081`; retained `d∞<=96` `5308`은 모두 edge-adjacent `0`으로 enclosed hole이고, edge opaque `17442` 중 magenta-dominant `12483`이다.
- root cause: perimeter-only `<=96` mask와 RGB 수정 `0`이 enclosed key hole과 edge blend를 보존했다.
- 승인된 최소 보정: all strong seed `d∞<=24`(enclosed 포함)에서 4-neighbor `d∞<=48` flood, transparent black matte; mask Chebyshev distance `<=2` edge만 donor radius `8`/mask distance `>2`/key distance `>96`/squared distance-y-x tie/`t=0.08..0.92`/residual `<=24` despill, alpha·silhouette 유지, 그 외 core byte-exact.
- 검증 재개 계약: closed key hole 제거, blend edge despill, legitimate nonblend purple unchanged, determinism, real unresolved blend fringe `0`. QA correction `1/2`; production/test current `a01-repack-implementation-reclass-002` `0/2`는 소비하지 않았고 canonical/Unity/foreground는 불변이다.

- production/test reclass-002: SyntheticFull stale enclosed-key marker expectation은 `1/2`에서 교정됐고, Real test는 actual tool entry 전 `Console.WriteLine` diagnostic의 missing `System.Console` explicit reference로 C# `CS0103` compile FAIL해 `2/2`다. production matte targeted+SyntheticFull GREEN이다.
- production/test reclass-003: current `0/2`. Console 의존을 제거하고 metric은 existing string/result channel로 반환하며, no-new-ref compile-only/targeted real harness preflight 뒤 Real exactly once를 실행한다. visual QA correction `1/2`, old derivative SHA, canonical/Unity/foreground는 불변이다.

## QA 재진입 2차 candidate

- candidate SHA-256 `30D41D844B7585513140BB38F0588FCF5689321538C332EB1F61ED248ABCBCA3`, `1280×1600`, `1,141,236` bytes; auto metrics strongkey `0`, unresolved-qualified `0`, despilled `14273` PASS이며 큰 enclosed patch는 제거됐다.
- independent visual QA `2/2`: **REJECT** — P4/P5 머리·손·가방·바지/신발 투명 경계에 non-linear bright magenta 1px 선/점.
- root cause: donor-line conservative classifier 밖 nonlinear key contamination; 현 contract은 silhouette/edge RGB policy 확장 없이는 제거 불가.
- `a01-repack-visual-fringe-reclass-001`은 2026-08-10 사용자 응답으로 종료한다. 선택은 **alpha·실루엣 무변경 + high-confidence bright nonlinear key RGB-only donor 교정 + 통과 에셋 Unity 재사용 bundle 구성**이다. 새 실행 경계는 `a01-game-asset-finalization-reclass-001` production/test `0/2`이며, 그 이후 independent Visual QA 1차 REJECT가 QA correction `1/2`를 기록했다.

## A01 게임 에셋 최종화 독립 Visual QA 1차

- automatic 결과: `SyntheticFull`/`Real`/atomic derivative/static bundle PASS. final derivative·Office Cast SHA-256 `6FA3F088638570451EE872372B3677F5D8B33930622C88F6009F7586C561EED1`, `1280×1600`; alpha·opaque 좌표는 보존됐다.
- 독립 Visual QA 1차: **REJECT, correction `1/2`** — dark transparent와 `#E8E8E8` light composite의 20 frame 전반에 bright magenta/pink fringe가 남았다. high-confidence alpha `255` + transparent 8-neighbor residual은 `24`px이며 예시는 P1 `(234,23,137)/(255,76,116)`, P4 `(203,57,106)/(172,20,72)/(179,41,82)`, P5 `(175,27,76)/(199,56,101)/(201,54,94)`다.
- fail-fast: QA static/code review·Unity EditMode·high-cost는 미실행이고 Unity run_id/fingerprint는 없다. protected raw/BG/FG/old cast, canonical/Unity preview/foreground는 unchanged이며 imagegen `7/7`, attempt 08 금지다.
- 다음: correction owner에게 RGB-only high-confidence pink edge predicate + deterministic donor correction을 재배정할 수 있다. alpha·silhouette은 불변이고 imagegen·새 asset/bundle structure 변경은 없다. 수정 후 affected low-cost test·derivative/package copy는 무효화하며 owner regression 뒤 QA final re-entry는 1회만 허용한다.

## A01 final QA Unity preflight blocker

- asset evidence: candidate SHA-256 `71F6542C8DD6229F40DB8E1CD1DF9A1C7B293FFDB28B172A3C87900465BD365D`; final independent Visual PASS, fresh static contract PASS, code/meta/manifest review PASS. production is frozen and protected hashes are unchanged.
- QA attempt: run `a01-office-assets-final-qa-001`, fingerprint `80202c9fc9bd6a7145686b692b314a23fe822c269029ace9c2fc11696090b38f`; `Invoke-HighCostVerification.ps1` exactly once, preflight exit `1` before Unity. `unity_starts=0`, recorded high-cost `0`, XML/log 없음, retry 없음이다.
- root cause: wrapper line 약 `223`이 multi-value `ProductionPath`를 child `pwsh -File Test-ComponentContractImpact.ps1`로 flatten해 두 번째 path가 unbound positional argument가 됐다.
- next: R1 `a01-high-cost-wrapper-multipath-fix-001` production/test current `0/2`에서 exact multi-value forwarding RED/self-test와 minimal wrapper GREEN만 owner가 수행하고 Unity는 실행하지 않는다. fix 뒤 새 run/fingerprint로 same final QA를 계속하되 asset/test/meta bytes가 불변이면 visual/static evidence는 유효하며 wrapper invocation은 1회만 허용한다. visual QA correction history `1/2`는 보존되고 Unity import는 pending이며 완료로 표기하지 않는다.

## A01 wrapper correction 2/2 stop

- `a01-high-cost-wrapper-multipath-fix-001` production/test는 `2/2`에서 중단됐다. failure1은 MultiPathOnly outer `pwsh -File`가 wrapper array를 flatten해 second baseline에서 실패했고, failure2는 hashtable+EncodedCommand outer harness가 singleton까지 string[]로 복원해 scalar `CurrentStatePath` binding에서 실패했다. 둘 다 intended child guard 전이다.
- production `Invoke-HighCostVerification.ps1`는 미변경이며 partial change는 test-only `tools/verification/Invoke-VerificationGuardSelfTest.ps1`다. Unity/high-cost actual `0`, retry 없음이다.
- root cause: process-boundary argv가 scalar/array/switch shape를 보존하지 못했고 test bootstrap cardinality model이 없었다.
- next R1 `a01-high-cost-wrapper-multipath-fix-002` current `0/2`: explicit typed/cardinality payload bootstrap을 먼저 고쳐 current production child multipath flattening exact RED를 확인하고, `Invoke-Guard`/callsites를 같은 typed payload로 minimal GREEN한 뒤 targeted GREEN과 guard full low-cost self-test 1회만 실행한다. child isolation/nonzero capture는 유지하고 Unity는 실행하지 않는다. 이후 final QA wrapper는 새 run identity로 1회만 재진입한다.

## A01 Unity run002 compile blocker

- wrapper multipath repair independent targeted PASS 뒤 QA run `a01-office-assets-final-qa-002`, fingerprint `5a3db5a8…f8805` UnityEditMode wrapper는 정확히 1회 실행됐다. actual Unity start `1`, batch exit `0`이나 NUnit XML이 없어 wrapper exit `1`; retry는 없다.
- log root cause: `A01OfficeAssetBundleTests.cs` line `40`/`63`이 Unity `6000.4.6f1`에 없는 `TextureImporter.spriteGenerateFallbackPhysicsShape`를 참조해 CS1061. tests never ran and import assertions remain unverified.
- same criterion consecutive failure `2`로 `a01-unity-import-test-compat-reclass-001` R1 production/test current `0/2`를 등록한다. C# test만 unsupported importer assertion을 제거하고 `.meta` flag static test는 유지하며 BG sprite와 all 20 cast sprites의 `Sprite.GetPhysicsShapeCount()==0` 및 explicit count를 direct observable로 검사한다. reflection/SerializedObject/private API/contract weakening은 금지한다.
- low-cost static source/bundle check 뒤 ledger reclass 및 new run003/fingerprint wrapper를 1회만 허용한다. asset SHA `71F6542C…BD365D` visual/static PASS frozen, imagegen `7/7`, protected hashes unchanged다. 비용은 distinct fingerprint·compiler log diagnosis·retry0·즉시 reclass로 `주의`이며 과다 조건은 없다.

## A01 run003 Cast import rect failure

- `a01-office-assets-import-compat-reclass-001` is registered PASS; run003 fingerprint `017051eef32948caa08d651bb24976be24f4bae72cc984bfd87d1fa115c8388c` wrapper exactly once started Unity and ended exit2/wrapper1, NUnit total2 pass1 fail1, retry0. cumulative Unity starts/recorded high-cost `2`다.
- BG test PASS. Cast test line74 first rect `p1_seated_idle` expected `(0,1280,320,320)`, actual `(0,0,0,0)`; names and all 20 sprites were reached, so image/name presence is verified but rect serialization is invalid.
- installed Unity 6000 built-in `com.unity.2d.sprite` meta evidence requires nested `rect:` + `serializedVersion: 2` + x/y/width/height. A01 inline rect mapping imports as zero.
- `a01-unity6000-sprite-rect-meta-fix-001` production/test current `0/2`: cast png.meta and static bundle test only; assert 20 nested exact rects/no inline rect, rewrite only 20 rect nodes, preserve PNG/meta flags/guid/spriteID/internal/name maps/frame names/pivots/BG/manifest. static GREEN, owner Unity 금지; run004 new fingerprint wrapper 1회만, failure면 중단·재분류한다. post-reclass failure `1/2`, 비용 `주의`다.

## A01 run004 technical PASS

- rect-meta owner RED→GREEN은 nested rect `20`/inline rect `0`과 meta SHA-256 `7251E14F2D2CDA869998F831FFAA9EDF4F2D643F466D6DABAFE6630BDEBB5ADD`를 확인했고 PNG SHA-256 `71F6542C8DD6229F40DB8E1CD1DF9A1C7B293FFDB28B172A3C87900465BD365D`는 불변이다.
- final independent QA `a01-office-assets-final-qa-004`, fingerprint `3c95323f3f9d18a8b2b8cc19cb8ea931135c79708084e31f8866fc5da1fa29ae`는 static PASS, wrapper/Unity exit0, NUnit total2 pass2 fail0, ledger success를 기록했다. token0·source lease 없음·isolated Unity/runner process0이며 raw/BG/FG/old cast는 불변이다.
- cumulative Unity starts/recorded high-cost는 `3`(run001 preflight 제외)이다. distinct fingerprint, retry0, blocker별 fail-fast/reclass·targeted correction 뒤 run004 단발 PASS라 `과다` 조건은 없고 비용 `주의`를 유지한다. 다음은 사용자 시각 수용이며, 그 뒤 scene integration은 별도 승인 단계다.

## A01 scene integration canonical PASS

- 구현 후보: mask-only occlusion Scene `A01OfficeAnimatic.unity`, 24fps·204 frames Timeline, 영속 AnimationClip 7개, P1~P5 개별 포즈·위치 곡선, 결정론적 in-place Scene builder, 원래 Scene setup과 nullable `playModeStartScene` path를 복원하는 전용 Preview launcher다.
- `a01-target-green-005`는 `15/18`로 SUPERSEDED다. production/session 결함이 아니라 같은 Startup asset의 managed wrapper identity를 `SameAs`로 비교한 2건과 isolated Editor 초기 `playModeStartScene == null`을 금지한 1건이 test oracle root cause였다. asset path round-trip 2곳으로 교체하고 초기 값은 null/non-null 그대로 보존하며 protected baseline 5개는 유지했다.
- canonical run: `a01-target-green-006`, fingerprint `de4f3d9b147b1a44722bea6021389569b46694214f8c6034cd47491bf8fbaa50`; Unity `6000.4.6f1` wrapper exit `0`, NUnit `18/18` PASS, failed/skipped/inconclusive `0`.
- canonical XML: `artifacts/a01-target-green-006-results.xml`, `19,229` bytes, SHA-256 `187EA51263638010ECB0E22B44C04CAE323A620EC0939D99B4674CAC8059610D`.
- fresh cache first import의 ShaderGraph `GUID` CS0246 두 줄은 API Updater 전 중간 상태였다. 같은 로그에서 후속 Tundra success 4회 뒤 `Test run completed ... code 0`으로 종료했으며 isolated Unity·`bee_backend` 잔존은 `0`이다. 대형 로그는 artifact budget에 따라 복사하지 않고 원본 `4,254,162` bytes·SHA-256 `E55402127FE65B2C974DD80B7E6B67CB36E907BBEC1899ECE5943F97ECE8A3D3`만 기록한다.
- low-cost bundle contract PASS, `git diff --check` PASS, 독립 scene/code/result QA 최종 판정 `CLEAN`이다. rejected color foreground는 Scene dependency가 아니며 Startup·Build Settings·package·보호 C# LF-normalized baseline은 current 18-test에서 PASS했다.
- 비용: wrapper current-state가 기록한 누적 Unity/high-cost start는 `6`이다. 별도 visible/hidden builder Editor 실행은 초기 기록에 완전히 정규화되지 않아 실제 전체 Unity 시작 수는 `6 이상·미집계`다. 같은 candidate의 환경성 재실행과 visible duplicate Editor가 발생했으므로 비용 판정은 **과다 — 후속 회피 가능**이며, 재발 방지 절차는 `docs/design/narrative/cinematic-production-failure-prevention-playbook.md`에 통합했다.
- 최종 상태: **기술 검증 통과 — 사용자 시각 수용 대기**. 사용자 실제 화면 확인 전 `완료`로 표현하지 않는다.
