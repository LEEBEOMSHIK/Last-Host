# 작업 배정서

## 기본 정보

- 작업 ID: `2026-08-10-a01-office-animatic`
- 작업명: A01 회사 일상 혼합형 모션 독립 무음 애니매틱
- 상태: **기술 검증 통과 — 사용자 시각 수용 대기**. mask-only occlusion 기반 독립 Scene·Timeline·Animation·전용 Preview 구현과 canonical `a01-target-green-006` `18/18` PASS, 독립 QA CLEAN까지 완료했다.
- 생성일: 2026-08-10
- 담당 에이전트: Unity 아키텍처 에이전트
- 보조 에이전트: ChatGPT 이미지 아트, 비주얼/테크아트, Unity 씬/통합 구현, QA/검증, 프로젝트 총괄 관리자, 프로젝트 조정
- 사용 스킬: `superpowers:writing-plans`, `superpowers:test-driven-development`, `unity-prototype-planner`, `pixel-lowpoly-style-keeper`, `imagegen`, `unity-verification-runner`

## 2026-08-11 scene integration 최종 경계

- 최종 후보 fingerprint: `de4f3d9b147b1a44722bea6021389569b46694214f8c6034cd47491bf8fbaa50`
- 구현 범위: A01 독립 Scene 1개, Timeline 1개, 영속 AnimationClip 7개, 결정론적 Scene builder, 상태 복원 Preview launcher, 승인 BG·Cast·mask-only 가림 에셋
- 금지 범위 유지: rejected color foreground, Startup/Build Settings 연결, 오디오·자막, A02, ProjectSettings·package 변경
- 자동·독립 기술 검증은 통과했으며, 실제 모션·구도·호흡은 사용자가 기존 main Unity에서 확인하기 전까지 완료로 승격하지 않는다.

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| Unity 아키텍처 | 구현 계획 소유 | 독립 씬·Timeline·Editor 프리뷰 경계와 파일 구조 설계 | 실행 가능한 구현 계획 |
| ChatGPT 이미지 아트 | 프리비즈 레이어 후보 제작 | 승인 reference를 사용한 깨끗한 배경·인물 포즈 후보와 생성 로그 | 추적 가능한 비최종 PNG 후보 |
| 비주얼/테크아트 | 시각 계약 검토 | 픽셀 밀도·실루엣·피벗·가림·팔레트·후보 상태 판정 | Unity 반입 전 검토 기록 |
| Unity 씬/통합 구현 | production 구현 | Editor 빌더·프리뷰 실행기, 독립 씬·Timeline·Animation·Import 연결 | 재생 가능한 A01 무음 애니매틱 |
| QA/검증 | 독립 검증 | S0 잠금, EditMode·Play·Console·씬/BuildSettings 보호 대조 | canonical 검증 기록 |
| 프로젝트 총괄 | 내부 감사 | 범위·승인·QA·사용자 수용 대기 판정 | 내부 승인 가능 여부 |
| 프로젝트 조정 | 통합 | 패킷·계획·담당 결과와 사용자 보고 통합 | 최소 변경 통합본 |

## 구현 담당 확인

- 코드/테스트 변경 담당: Unity 씬/통합 구현 에이전트 — 시네마틱 전용 Editor 코드와 EditMode 테스트만 담당
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: Unity 씬/통합 구현 에이전트 — 새 독립 A01 씬·Timeline·Animation만 담당하며 입력·UI·ProjectSettings는 변경하지 않음
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 해당 없음

## 실행 승인·작업공간

- 사용자 선택: 옵션 1 — 에이전트 분리 실행을 위한 isolated worktree.
- 작업 위치: `C:/projects/Last-Host/.worktrees/a01-office-animatic`
- Git branch / 시작 base: `feat/a01-office-animatic` / `a5a4cf8121a52f4d2a1c3ceb537db181bd141f4e` (`a5a4cf8`). 연결 worktree(`git-dir`과 common dir 상이, superproject 없음)임을 `superpowers:using-git-worktrees` 절차로 확인했다.
- 사용자 실행 승인: 옵션 1의 A01 전용 `Art/Cinematics`, `Editor/Cinematics`, `Tests/EditMode/Cinematics`, 독립 Scene/Timeline/Animation 수정과 built-in Cast attempt 07 **단 1회**다. historical 6회 뒤 attempt 07을 실행해 actual `7/7`, remaining 0이다. raw `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png`는 `1122×1402`, `1,648,495` bytes, SHA `24A143D7344DAC8358CD496C6AD03718AADB492D67B96E7CCCF0E46DA08A090D`; all y-band horizontal equal-grid boundary collision으로 independent REJECT다. semantic identity/standing/bags는 대체로 PASS지만 global nearest-neighbor로는 해결할 수 없다.
- 승인 제외 유지: attempt 08 또는 재시도, 외부/API/CLI 생성 경로, 신규 패키지·네트워크 의존성, A02/B08, 오디오, Startup 연결, 최종 아트 선언. 사용자 승인 implementation은 active이며, `a01-plan-reclass-004`의 두 plan blocker는 `f7fcf5a`로 보정됐다: root cause `sole active override omitted complete downstream foreground paths and protected background identity`; change plan `add exact paths/RED-GREEN-QA/copy SHA commands and full DA5F22DE7D1C9BDBABE2A8887640085142D23E02CF3BF94B21E217A7EC98AA0C preflight`. BG/foreground/old canonical/Unity는 불변이다.
- source `main` checkout의 관련 없는 `UnityProject/ProjectSettings/ProjectSettings.asset` 수정은 이 worktree에 없으며 복사·수정·커밋 범위에 포함하지 않는다.
- Task 1 시작 `git status --short`: `M docs/project-handoff/current-task-board.md`, `M docs/project-handoff/task-cost-dashboard.md`, `?? _workspace/active/2026-08-10-a01-office-animatic/`, `?? docs/superpowers/`. 모두 기존 A01 계획 준비 기록이며, Unity production 파일은 status에 없다.

## QA S0 실행 전 검토

- 검토자: QA/검증 에이전트. production 작성·imagegen·Unity·Editor Play·build 시작 전 정적 계약만 검토했다.
- A01-C01~C10은 `task.md`의 순서, 개별 포즈/반응, 공간 앵커, 픽셀·가림, negative control, 복구 수명주기, 출처 상태, 시간 경계, 사용자 수용 대기 조건으로 잠겼다.
- A01-C06 검증 방식: byte stream에서 CRLF만 LF로 치환하고 lone CR를 FAIL로 하는 LF-normalized canonical bytes/SHA-256을 비교한다. raw checkout bytes/SHA-256은 환경 관찰값, Git blob OID는 provenance 보조값이며 canonical gate가 아니다.
- `execution-s0-001`의 raw checkout 불일치 blocker는 독립 감사의 canonical PASS로 `SUPERSEDED`됐다. `.gitattributes`는 변경하지 않는다.
- QA S0 판정 (historical): `착수 가능 — A01-RP-01~06 잠금 및 Task 2A scoped review CLEAN`; Task 1의 C06 독립 리뷰 CLEAN 이력도 유지한다. 검토 기준은 계획 커밋 `c551078`(Task 2A 독립화·외부 `pwsh`/Copy fail-fast 보정)과 HEAD `3bf4369`(Task 2A Steps 2.1~2.8 active 범위)다. `a01-plan-reclass-007` current plan correction `0/2`는 **CLEAN**, execution S0 correction `0/2`; Task 2A는 reclass-002 production/test 2/2 뒤 `a01-repack-implementation-reclass-003` current `0/2`이며 visual QA correction `1/2`였다. 당시 canonical·Unity·foreground는 `0`, Task 2B blocked, high-cost 변경 `0`, actual imagegen `7/7`, 비용 `주의`였다.

- 현재 구현/QA 상태: color derivative REJECT 이력은 mask-only 구조로 대체됐다. 승인 BG duplicate와 exact mask만 사용한 Scene·Timeline·Animation·Preview가 canonical run006 `18/18`과 독립 QA CLEAN을 통과했으며 rejected RGB는 production·scene에서 계속 금지다.

## 루프 게이트

- 게이트 적용 대상: 예
- 위험 등급: R3 scene integration. mask-only architecture와 scene production/test는 canonical run006 기술 PASS이며 사용자 시각 수용만 남았다.
- 위험 등급 근거: 승인된 한 장면의 독립 프리비즈지만 새 Unity 씬·Timeline·Editor 코드·테스트·래스터 후보와 다섯 production 계층의 folder `.meta` 재현 계약을 함께 다루며, R2 계획 감사 correction 2/2 뒤 누락된 candidate identity 범위를 보정해야 한다.
- 적용 사유: 시작 화면과 빌드 흐름을 건드리지 않은 상태에서 A01 모션 방식의 실제 가독성과 기술 경계를 검증해야 한다.
- QA/검증 필요: 예 — 구현 주체와 분리한 EditMode·Play 사전 검증
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예
- correction cycle (pre-run004 historical): R2 plan 2/2 종료 → `a01-plan-reclass-001` → R3 plan correction 2/2(감사 004 wrapper 실인자 보정 + NET Standard 2.0 API compatibility/stale-state review failure) → `a01-plan-reclass-002` → 옵션 1 current R3 plan correction 2/2(초기 통합 1/2 + `a01-option1-plan-review-001` FAIL 2/2) → `a01-plan-reclass-003` current plan correction 2/2 → `a01-plan-reclass-004` current plan correction 2/2 → `a01-plan-reclass-005` current plan correction 2/2 → `a01-plan-reclass-006` current plan correction 2/2(Task2A/2B isolation) → `a01-plan-reclass-007` current plan correction 0/2·**CLEAN** → `a01-repack-implementation-reclass-001` production/test 2/2(sourceCuts 고정 preflight는 올바르게 조기 REJECT, `SyntheticFull` old `Target cell`·`ownership` 기대가 wrong reason 실패) → `a01-repack-implementation-reclass-002` current production/test 0/2 → independent visual QA correction 1/2(derivative automatic PASS, bright magenta fringe REJECT). change plan은 all strong seed `d∞<=24`·4-neighbor `d∞<=48` matte와 authorized edge-only despill이며 A01-RP-03는 all unmasked non-despilled core exact + authorized matte/despill only다. 당시 execution S0 0/2, actual imagegen 7/7 REJECT, high-cost 0, 비용 주의였다.
- capability profile / 요청 route: canonical 자동 검증은 `UnityEditMode`; 현재 `McpPlay.available=false`이므로 전용 Editor Play는 비canonical 개발 확인·사용자 수용 보조로만 기록
- attempt ledger 경로 / 같은 criterion 연속 실패 수: 실제 고비용 검증 시작 시 `artifacts/verification-attempt-ledger.json` 생성 / 0; 계획 재분류는 고비용 wrapper 실행 전 canonical 문서 판정 `a01-plan-reclass-001`~`a01-plan-reclass-006`(각 종료 2/2) → `a01-plan-reclass-007`(current plan 0/2·CLEAN) → `a01-repack-implementation-reclass-001`(production/test 2/2; real candidate 전) → `a01-repack-implementation-reclass-002`(production/test current 0/2) / independent visual QA correction 1/2

## S0 사용자 원증상·검증 charter

- 사용자 원문 또는 원증상: 회사원들이 웃고 떠드는 A01을 정지 슬라이드가 아니라 움직이는 2D 시네마틱으로 보고 싶으며, 추천한 제한 프레임·분리 레이어·카메라 혼합형으로 진행한다.
- 재현 씬·입력·좌표·상태: 새 독립 `A01OfficeAnimatic` 씬을 전용 Editor 메뉴로 열어 무음 재생한다. 시작 화면과 게임 시작 버튼에서는 진입하지 않는다.
- 원증상 증거: 현재 저장소에는 A01 공간·연기 후보 PNG와 제작 명세만 있고 재생 가능한 씬·Timeline·Animation은 없다.
- 합성 oracle의 금지 결과: 단일 평면 PNG 흔들기, 전 인물 동기 루프, 고무 같은 관절 변형, 픽셀 흔들림·가림 틈, 감염·기침·공포 암시, 자막·내레이션·오디오, Startup/Build Settings 연결, 새 패키지, 후보를 최종 에셋으로 선언
- 합성 oracle의 허용 결과: 깨끗한 배경과 다섯 인물·전경 레이어를 분리하고 제한 포즈·작은 변환·절제된 카메라로 대화→반응 확산→웃음→이동 준비를 무음 상태에서 읽게 하는 독립 프리비즈
- 완료 주장 한 문장: A01 독립 무음 애니매틱이 승인된 여섯 비트를 재생하고 기술 검증을 통과했으며, 최종 시간·아트·오디오·Startup 연결은 확정하지 않은 채 사용자 시각 수용을 기다린다.

| criterion ID | 유형 | 입력·상태 | 기대값 | 최소 검증 |
| --- | --- | --- | --- | --- |
| A01-C01 | 원증상·성공 | 무음 전체 재생 | `P1` 발화, 지연 반응, 함께 웃음, 점심 이동 준비가 순서대로 구분됨 | Timeline Play 관찰 + 비트/트랙 EditMode 계약 |
| A01-C02 | 성공·경계 | 다섯 인물 | `P1`~`P5`가 서로 다른 역할·포즈 수·반응 시점을 가지며 동기 반복하지 않음 | Sprite/Animation/Timeline 트랙 대조 |
| A01-C03 | 연속성 | 첫 프레임과 마지막 프레임 | 왼쪽 창·중앙 책상 섬·`P1` 자리·오른쪽 출입문 축이 유지되고 마지막은 A02 보행 직전임 | 씬 앵커 계약 + 화면 확인 |
| A01-C04 | 시각 품질 | 960×540 Game View | Point 필터·정수 픽셀 위치·명시적 책상 전면 마스크를 유지하고 흔들림·틈·잘못된 가림이 없음 | Import/Transform 정적 검사 + Play 관찰 |
| A01-C05 | negative control | 화면·오브젝트·트랙 | 감염·기침·보라색 파지·먼지 입자·텍스트·로고·AudioTrack·ParticleSystem이 없음 | 씬/Timeline 계층 검사 |
| A01-C06 | 범위 보호 | Startup·Build Settings·패키지 | 기존 Startup 시작 씬, 프로토타입 버튼 목적지, Build Settings, manifest/lock이 바뀌지 않음 | LF-normalized canonical baseline diff + EditMode 계약 (lone CR FAIL; raw checkout hash는 관찰값) |
| A01-C07 | 수명주기 | 전용 프리뷰 진입·종료 | A01만 재생하고 종료 뒤 Startup play-mode 시작 씬 설정과 Edit 상태가 복구됨 | 전용 launcher Play smoke + scene/dirty 기록 |
| A01-C08 | 출처·상태 | 생성 PNG와 로그 | reference·전체 프롬프트·도구·날짜·출력 경로가 기록되고 모두 `비최종 프리비즈 후보`임 | manifest·생성 로그 대조 |
| A01-C09 | 시간 경계 | 여섯 비트 스케줄 | 양수인 초기 측정값으로 순차 재생되며 수정 가능하고 최종 러닝타임으로 선언되지 않음 | schedule 단위 테스트 + 문서 문구 대조 |
| A01-C10 | 사용자 수용 | 기술 검증 후 실제 재생 | 사용자 확인 전 상태가 `기술 검증 통과 — 사용자 수용 대기`를 넘지 않음 | verification·총괄 판정 대조 |

- QA S0 사전 검토: 구현 실행 선택 뒤, production 작성 전 QA/검증 에이전트가 A01-C01~C10과 증거 예산을 잠근다.

### Task 2A Cast repack S0 charter

- 완료 주장 한 문장: attempt 07 raw와 고정 manifest만 사용한 20-pose derivative가 A01-RP-01~06의 자동·독립 시각 oracle을 모두 통과하고, 보호 BG·FG·old canonical·Unity preview는 그 QA PASS 전까지 불변이다.
- 합성 oracle: raw의 정체성·포즈를 재작화 없이 20개 고정 cell로 정수 이동해 exact `1280×1600` hard-alpha sheet를 만든다. all unmasked non-despilled core는 exact RGBA로 보존하고, enclosed key hole matte와 blend-edge despill만 승인 조건에서 허용한다. scale·rotation·interpolation·repaint·largest-component 선택, 설명되지 않은 추가 불투명 픽셀, pose/소품 누락, 셀 경계 침범·unresolved magenta fringe는 모두 금지한다.

| criterion ID | 고정 입력·상태 | PASS oracle | 최소 증거 |
| --- | --- | --- | --- |
| A01-RP-01 source provenance | `a01-office-cast-pose-grid-attempt-07-raw.png` | 처리 전후 exact `1122×1402`, `1,648,495 bytes`, SHA-256 `24A143D7344DAC8358CD496C6AD03718AADB492D67B96E7CCCF0E46DA08A090D`; raw byte 변경 `0` | 실행 전후 dimensions·bytes·uppercase SHA 대조 |
| A01-RP-02 20-cell ownership + authorized matte | half-open fixed cuts `x=[0,281,561,842,1122]`, `y=[0,318,591,847,1107,1402]`; row-major pose ID 20개와 target `(row,column)` 20개 | 20 rect가 source canvas 모든 좌표를 overlap·gap 없이 정확히 1회 소유한다. rect 적용 전 all strong seed `d∞<=24`(enclosed hole 포함)에서 4-neighbor `d∞<=48` flood-fill로 hard matte mask를 1회 계산하며 retained pixel도 정확히 pose 하나에만 속한다. 누락·중복 `0` | manifest 정적 검사 + synthetic overlap/gap/out-of-range/ID·target 누락·중복 + closed key hole negative control |
| A01-RP-03 all unmasked non-despilled core exact + authorized matte/despill only | column source axes `x=[140,421,701,982]`; cell-local target anchor `(160,306)`; pose별 `sourceGroundY=maxRetainedY` | all unmasked non-despilled core source `(x,y,RGBA)`는 정확히 한 번 `(x+dx,y+dy)`에 동일 RGBA로 존재한다. authorized matte는 transparent black만, authorized despill은 mask Chebyshev distance `<=2`, donor radius `8`·mask distance `>2`·key distance `>96`, squared distance/y/x tie, donor→key projection `t=0.08..0.92`, residual `<=24`일 때만 RGB를 바꾸며 alpha·silhouette은 유지한다. target cell에 manifest가 설명하지 않는 추가 opaque pixel `0`; scale·rotation·interpolation 금지 | 20 pose별 core exact RGBA 1:1 offset + closed-hole/blend-edge/legitimate-purple synthetic fixture + deterministic assertion |
| A01-RP-04 output contract | output `1280×1600`, grid `4×5`, cell `320×320`, boundary band `6` | 각 cell local `x/y=0..5,314..319`의 opaque pixel `0`; 각 cell opaque coverage `0.05..0.60` inclusive; 모든 alpha는 `0` 또는 `255`; alpha `0` RGB는 `(0,0,0)`; 같은 raw·manifest·인자 2회 output SHA 동일 | real candidate 자동 test + repeated SHA 대조 |
| A01-RP-05 identity/pose/bag visual invariant | P1 blue shirt·brown curls·glasses seated 4; P2 bun·olive top seated 4; P3 dark hair·dark-green·mostly back-facing seated 4; P4 beige overshirt standing 4/no chair; P5 rust blouse·cream pants standing 4/no chair | 20 pose 의미가 ID와 일치하고 P4 black/P5 brown personal commuter bag의 종류·색·같은 body side가 각 행에서 유지된다. chair wheels, P3 laptop, bag straps, shoes 등 disconnected retained prop의 누락·추가·잘림이 없고 magenta fringe·셀 간 jitter가 없다 | 독립 비주얼 QA가 raw와 derivative를 원본 상세도로 20 cell 대조 |
| A01-RP-06 protected immutability | BG source `DA5F22DE7D1C9BDBABE2A8887640085142D23E02CF3BF94B21E217A7EC98AA0C` (`1,648,998 bytes`); FG source `D782D38E4D510E1D13680C21D6642F86647DF53662B8D94150376EC73770F1E1` (`1,097,398 bytes`); old canonical Cast `C3BD3E5F15CDA75F74AE13433D6C7C03E6D3BCC122E8A9A48B8AF1986B8E44AD` (`1,738,437 bytes`); Unity Cast preview baseline `MISSING` | A01-RP-01~05 automatic+independent visual QA PASS 전 세 기존 파일의 bytes/SHA는 불변이고 Unity preview는 계속 존재하지 않는다. QA 실패 시 canonical copy·preview 생성·foreground·Unity 후속 `0` | QA 직전 protected SHA/bytes 및 preview absence 대조; 승격은 Step 2.8 exact dual-PASS preflight 뒤에만 허용 |
| A01-RP-07 nonlinear bright-key RGB cleanup | 입력은 fixed candidate SHA `30D41D844B7585513140BB38F0588FCF5689321538C332EB1F61ED248ABCBCA3`. 후보는 alpha `255`, 8-neighbor transparent 인접, `max(R,B)>=128`, `R-G>=40`, `B-G>=40`, `abs(R-B)<=96`을 모두 만족하는 픽셀이다 | fixed candidate의 alpha mask·opaque 좌표·셀 피벗은 byte-for-byte 유지한다. 후보 `4,554`개만 radius `8` 안의 가장 가까운 opaque donor RGB로 교체한다. safe donor는 위치와 무관하게 RGB가 위 네 chroma 수치 조건을 모두 만족하지 않아야 하며, tie는 squared distance→y→x다. donor가 없으면 fail-fast한다. 교정 후 같은 후보 `0`, 변경 alpha `0`, 비후보 RGB 변경 `0`; 어두운 보라 외곽선과 내부 색은 보존한다 | nonlinear synthetic RED/GREEN + fixed real candidate before/after 독립 pixel oracle + light/dark background visual QA |
| A01-RP-08 reusable Unity cinematic asset bundle | `Assets/_Project/Art/Cinematics/Opening/A01/Office/` 아래 approved BG 1장, cleaned Cast 1장, manifest 1개와 필요한 `.meta`를 둔다. foreground는 S0 QA PASS 전 미포함이며 PASS 뒤 byte-identical `a01-office-foreground-v1.png`와 `.meta`를 조건부 포함한다 | Cast는 Sprite Multiple `4×5`, 각 `320×320`, 20개 고정 이름, PPU `100`, Point, mipmap off, uncompressed, alpha transparency, 공통 ground pivot `(0.5,0.04375)`다. BG는 Sprite Single, Point, mipmap off, uncompressed다. foreground는 source SHA 불변·hard alpha·transparent black·determinism·fringe no-regression과 독립 visual QA PASS 후에만 manifest/static/import contract에 포함한다. AI raw·반려본은 Unity production 경로에 복사하지 않는다 | 파일 SHA/PNG contract + `.meta`/manifest 정적 검사 + Unity EditMode Import 계약 1회 + 독립 visual QA |

- 20 pose row-major 순서: `p1_idle,p1_speak,p1_laugh,p1_rise`; `p2_idle,p2_nod,p2_laugh,p2_hold`; `p3_work,p3_shoulder_laugh,p3_head_turn,p3_hold`; `p4_idle,p4_gesture,p4_exit_turn,p4_hold`; `p5_idle,p5_laugh,p5_step_ready,p5_hold`.
- QA S0 scoped verdict: **착수 가능**. 계획 `c551078`부터 `3bf4369`까지의 Task 2A Steps 2.1~2.8은 위 oracle과 일치하며 blocker `0`; Task 2B는 계속 blocked다.

## 고비용 preflight 입력

- agent brief JSON (`packet-only`, `fork_turns:none`, 필수 파일 3개 이하): 실제 Unity 검증 시작 시 `artifacts/agent-brief.json` 생성
- verification current-state JSON: RED 전 `artifacts/verification-current-state.json` 생성, 매 run 새 run/fingerprint와 빈 evidence로 갱신하고 과거 증거는 `verification.md`에만 보존
- QA C# harness lint 경로: `UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01`
- component contract baseline / candidate / test 경로: Startup/Build Settings baseline, 새 Cinematics Editor 경로, A01 EditMode 테스트 경로
- isolated Unity cache root / work ID marker: `$env:TEMP/last-host-unity-cache` / `2026-08-10-a01-office-animatic`
- low-level runner 직접 Run 금지 확인: `Invoke-HighCostVerification.ps1`만 사용하고 `Invoke-UnityEditModeTests.ps1` Run 직접 호출 금지

## 목적

승인된 `03 공간 + 02 연기` 혼합 명세를 실제로 판단할 수 있는 독립 A01 무음 애니매틱으로 옮기되, 최종 아트·오디오·러닝타임·전체 오프닝 연결을 확정하지 않는다.

## 입력 자료

- `docs/design/narrative/opening/a01-office-hybrid-motion-design.md`
- `_workspace/active/2026-08-08-opening-cinematic-origin/artifacts/task6/a01-office-base/a01-office-base-candidate-02-character-motion.png`
- `_workspace/active/2026-08-08-opening-cinematic-origin/artifacts/task6/a01-office-base/a01-office-base-candidate-03-spatial-anchor.png`
- `docs/design/visual/pixel-isometric-2d-production-guide.md`
- Unity `6000.4.6f1`, URP `17.4.0`, Timeline `1.8.12`

## 해야 할 일

1. 독립 씬·Timeline·레이어·전용 프리뷰 실행기의 구체 구조와 TDD 순서를 구현 계획으로 고정한다.
2. 승인 reference로 깨끗한 배경과 인물별 제한 포즈 레이어 후보를 만들고 출처·프롬프트를 기록한다.
3. imagegen historical `6/6` 이력을 보존한다. invocation 5 background `1672×941`, SHA `DA5F…AA0C`는 PASS이고 invocation 6 cast `1122×1402`, SHA `C3BD…E44AD`는 exact grid·boundary·P4·chroma 계약 REJECT다. foreground source SHA와 기존 canonical cast/Unity preview는 불변이다.
4. 승인된 built-in Cast attempt 07을 **한 번만** 실행해 versioned raw와 normalized-alpha derivative를 분리한다. raw prompt·sole spatial reference·dimensions·bytes·SHA를 log에 기록하고, failure면 attempt 08을 금지한다.
5. 기존 두 PowerShell 파일만 2단계 TDD로 만든다. 먼저 chroma-only `tool not found` RED→GREEN을 완료하고, 그 GREEN tool에 normalization parameter/behavior RED를 추가해 `tool not found`가 아닌 실제 미지원으로 nonzero를 확인한 뒤 normalize/alpha/grid/boundary/coverage/source-SHA/determinism GREEN을 구현한다. 자동 계약과 독립 visual QA가 모두 PASS한 경우에만 canonical cast·Unity preview를 승격하고, 그 뒤 foreground 및 Unity 단계를 재개한다.
6. 실패하는 계약 테스트를 먼저 확인한 뒤 Editor 빌더·전용 launcher와 씬·Timeline·Animation을 구현한다.
7. 표적 EditMode, frozen candidate 전체 EditMode, 전용 Play smoke와 Console/dirty/Startup 복구를 독립 검증한다.
8. 총괄 감사 뒤 사용자에게 실제 재생 결과와 조정할 비트만 제시한다.

## 산출물

- `docs/superpowers/plans/2026-08-10-a01-office-animatic.md`
- 비최종 레이어 후보와 생성 로그
- A01 전용 Editor 빌더·프리뷰 실행기·EditMode 테스트
- `Assets/_Project/Scenes/Cinematics/Opening/A01OfficeAnimatic.unity`
- A01 Timeline·Animation preview assets
- `_workspace/active/2026-08-10-a01-office-animatic/verification.md`

## production 소유권과 검증 예산

| production 파일/불변식 | 단일 구현 소유자 | 변경 금지/인계 조건 |
| --- | --- | --- |
| imagegen 원본 후보·생성 로그 | ChatGPT 이미지 아트 에이전트 | 최종 에셋 선언·Unity 직접 반입 금지, 비주얼 검토로 인계 |
| `tools/art` 알파 도구·자체 테스트와 preview 파생 계약 | Unity 씬/통합 구현 에이전트 | source overwrite·신규 패키지·threshold 96 이상·외부/API 금지, 독립 코드·시각 QA로 인계 |
| 픽셀·레이어·피벗·가림 계약 | 비주얼/테크아트 에이전트 | 사용자 선택 대체 금지, 구현 전 PASS/반려 사유 기록 |
| Cinematics Editor 코드·테스트·씬·Timeline·Animation | Unity 씬/통합 구현 에이전트 | Startup·게임플레이·ProjectSettings·패키지 변경 금지, QA freeze 전 인계 금지 |
| canonical 검증 증거와 완료 상태 | QA/검증 에이전트 | production 수정 금지, 첫 blocker에서 고비용 후속 단계 중지 |
| 범위·내부 승인 판정 | 프로젝트 총괄 관리자 | 직접 구현·MCP 실행 금지, 사용자 수용 대기를 완료로 승격 금지 |

- Unity session lease 예정 소유자: `a01-implementation-001`·RED·GREEN은 `unity-scene-integration`; frozen full `a01-frozen-full-001`과 Play smoke `a01-qa-play-smoke-001`은 `qa-verification`로 명시 인계
- 관련 suite: A01 Cinematics EditMode + Startup 계약 회귀
- 전체 suite 실행 조건: candidate freeze와 표적 GREEN 뒤 QA가 전체 EditMode 1회 실행
- 대형 matrix 실행 필요·근거: 없음 — 단일 960×540 프리비즈와 한 재생 경로만 검증
- artifact budget / criterion별 canonical 증거: imagegen 원본·로그 1묶음, targeted XML 1개, full XML 1개, Play 캡처 최대 3장과 동일 run sidecar, Console/scene 상태 1기록

## 비용 계획

| 비용 항목 | 계획 |
| --- | --- |
| 역할·인계 | 아키텍처 1 → 이미지 후보 1 → 비주얼 검토 1 → Unity 구현 1 → 독립 QA 1 → 총괄 1 |
| 표적 검증 | TDD RED 1회, 구현자 표적 GREEN 1회, 중복 재실행 금지 |
| Unity/MCP/빌드·full suite | isolated Unity EditMode 최대 3회(RED·GREEN·frozen full), MCP/Editor Play smoke 1회, 빌드 0회 |
| matrix/capture·artifact | historical imagegen `6/6`(no-result 1·반려 결과 포함) + 승인된 Cast attempt 07 1회로 cap `7/7`. versioned derivative 1개는 automatic PASS·independent visual QA REJECT로 correction `1/2`이며, PowerShell/.NET repack TDD는 `a01-repack-implementation-reclass-002` current `0/2`, preview·capture 0, 대형 matrix 0; attempt 07 QA PASS 전 후속 실행 금지 |

- 현재 집계: wrapper current-state의 누적 Unity/high-cost start는 `6`, canonical scene NUnit은 `18/18` PASS다. 별도 builder/visible Editor 시작은 초기 기록에 완전히 정규화되지 않아 전체 시작 수는 `6 이상·미집계`이며 비용은 `과다 — 후속 회피 가능`이다.

- 중앙 현황판 대상 여부·행: R3 대상. canonical run006 기술 PASS·사용자 시각 수용 대기, wrapper high-cost `6`, 비용 `과다 — 후속 회피 가능`으로 동기화한다.

## 금지 범위

- A02 이후 회사 보행, B08 재방문, 학교·가정·확산·기침·주인공 장면 제작
- Startup 버튼·전체 오프닝·Build Settings 연결
- 내레이션·자막·대사 텍스트·오디오 제작 또는 임시 AudioTrack
- Cinemachine·2D Animation 등 패키지 추가와 manifest/lock 변경
- 생성 후보를 최종 픽셀 아트·최종 게임 에셋·완성 컷신으로 선언
- 기존 씬·프로토타입·ProjectSettings·게임플레이 코드 변경
- 최종 초 단위 러닝타임 확정

## 승인 필요 항목

- 옵션 1 isolated worktree 실행 방식과 작업 위치 승인 완료
- OpenAI 내장 imagegen 초기 3회, no-result 1회, background/cast correction 2회의 historical `6/6` 보존
- 사용자 승인: built-in Cast attempt 07 단 1회, 누적 cap `7/7`; `a01-imagegen-correction-visual-review-002`의 background PASS/cast REJECT 이력은 재실행하지 않음
- attempt 07 raw·normalized-alpha derivative의 자동 계약과 독립 visual QA가 모두 PASS하기 전 canonical/Unity 승격 금지; 실패 시 attempt 08, resize/crop/padding-only, 외부/API 경로는 새 승인 없이는 금지
- 이 계획에 열거된 A01 전용 `Art/Cinematics`, `Editor/Cinematics`, `Tests/EditMode/Cinematics`, 독립 Scene/Timeline/Animation 수정 승인 완료
- imagegen 내장 도구로 해결되지 않는 투명도·일관성 문제에 외부/API 생성 경로를 쓰는 경우 별도 승인
- 기술 검증 뒤 실제 모션·구도·호흡에 대한 사용자 최종 시각 수용
- 외부/API 생성 경로, A02, 오디오, Startup 연결은 각각 별도 승인

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 이 `task.md`와 `verification.md` 두 파일 유지, 실제 실행 전 불필요한 분리 이력 미생성
- 담당 에이전트 산출물 확인: 아키텍처·이미지·비주얼·Unity 구현 결과가 각 소유권과 일치
- 에이전트 수행 이력 확인: `verification.md`의 실제 수행 표에 통합
- 구현 담당 에이전트 확인: Unity 씬/통합 구현 에이전트
- 메인 에이전트 직접 구현 예외 사유 확인: 해당 없음
- QA/검증 에이전트 기록 확인: A01-C01~C10, current fingerprint/run, Play/Console/dirty/Startup 복구
- 총괄 관리자 판정 확인: 내부 승인 가능 또는 수정 필요 명시
- 승인 게이트 확인: 이미지 생성·Unity 수정 실행 승인과 범위 일치
- 완료 판단에 영향을 주는 미검증 항목: 사용자 실제 재생 수용은 항상 별도 유지

## 완료 기준

- 승인 명세의 여섯 비트가 독립 무음 Timeline에서 재생되고 A01-C01~C09의 현재 후보 검증이 모두 유효하다.
- Startup·Build Settings·패키지·기존 플레이어블이 변하지 않는다.
- QA와 총괄 판정이 기록되며, 사용자 실제 재생 확인 전에는 `기술 검증 통과 — 사용자 수용 대기`로 유지한다.
