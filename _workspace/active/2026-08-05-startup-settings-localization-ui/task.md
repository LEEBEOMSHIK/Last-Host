# 작업 배정서

## 기본 정보

- 작업 ID: `2026-08-05-startup-settings-localization-ui`
- 작업명: PC 시작 화면·설정 UI와 다국어 준비 구조
- 상태: 내부 승인 가능 — Play 진입·언어별 폰트 후보 UnityEditMode 38/38 PASS, 사용자 실제 화면 수용 대기
- 생성일: 2026-08-05 KST
- 담당 에이전트: 프로젝트 조정 에이전트
- 보조 에이전트: Unity 아키텍처, 게임플레이 구현, Unity 씬/통합 구현, QA/검증, 프로젝트 총괄 관리자
- 사용 스킬: `unity-prototype-planner`, `unity-verification-runner`

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| 프로젝트 조정 | 범위·소유권·상태·비용 | 작업 계약, 인계, 현황판 | 본 패킷·보드 |
| Unity 아키텍처 | R3 사전 구조 검토 | Startup 씬, 설정·로컬라이제이션 경계, Build Settings 보존안 | `artifacts/architecture-review.md` |
| QA/검증 | 구현 전 S0·구현 후 독립 검증 | C1~C9, fail-fast, Unity Play 가능 범위 | `verification.md` |
| 게임플레이 구현 | 런타임 코드·순수 상태·테스트 | 설정 모델, 저장소, 한·영 문자열 키, 씬 전환 명령 | 코드·EditMode 테스트·handoff |
| Unity 씬/통합 구현 | 씬·UI·직렬화·Build Settings | Startup 씬, 메뉴/설정 패널, 2D 씬 연결 | 씬·빌더·설정·handoff |
| 프로젝트 총괄 관리자 | R3 사전/최종 감사 | 범위·증거·비용·사용자 수용 대기 판정 | `director-review.md` |

## 구현 담당 확인

- 코드/테스트 변경 담당: 게임플레이 구현 에이전트
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: Unity 씬/통합 구현 에이전트
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 해당 없음

## 루프 게이트

- 게이트 적용 대상: 예
- 위험 등급: **R3**
- 위험 등급 근거: 새 시작 씬·화면 상태 전이·PlayerPrefs 저장 형식·Build Settings(ProjectSettings)·사용자 가시 UI가 결합된다.
- 적용 사유: 게임 실행 진입점을 3D 레거시 직접 시작에서 다국어 Startup UI로 변경한다.
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예 — R3 사전·최종
- 커밋 전 차단 조건 확인 필요: 예
- correction cycle: `0/2` — S0 r0/r1 연속 blocker 2회 뒤 `startup-settings-s0-default-profile-r2-20260805`로 재분류
- 고비용 preflight correction: `1/2` — stale evidence run_id를 동일 후보·동일 QA run으로 동기화한 뒤 실제 Unity 1회 PASS
- 재분류 root cause: Draft 전이만 먼저 잠그고 최초 실행·기본값·손상값 복구가 공유할 결정론적 기본 프로필과 해상도 선택 우선순위를 누락했다.
- 재분류 change plan: 언어·화면모드·VSync 기본값과 지원 해상도 선택/fallback 알고리즘을 C4/C6에 단일 계약으로 고정한다. 위험 등급과 production owner는 R3 그대로 유지한다.
- capability profile / 요청 route: 최신 QA 표적 `UnityEditMode` `38/38` PASS; `McpPlay`·build는 capability unavailable
- attempt ledger 경로 / 같은 criterion 연속 실패 수: `artifacts/verification-attempt-ledger.json` / 0

## S0 사용자 원증상·검증 charter

- 사용자 원문 또는 원증상: “게임이 켜졌을 때의 초기 화면과 설정 등에 대한 UI, 내용을 진행”하고 “계속 보정할 것이므로 다국어까지 고려”한다.
- 재현 씬·입력·좌표·상태: 현재 Build Settings는 3D 레거시 `RatHostPrototype.unity`를 직접 시작하며 별도 시작 화면·설정·언어 선택이 없다.
- 원증상 증거: `UnityProject/ProjectSettings/EditorBuildSettings.asset`, 메뉴/설정/로컬라이제이션 런타임 부재.
- 합성 oracle의 금지 결과: 레거시 3D 삭제·수정, 기술 샘플을 본 게임으로 오인, UI 문자열 산재·누락 키, 언어 변경 후 일부 텍스트 잔존, 취소가 설정을 저장, 시작 버튼이 잘못된 씬 로드, 작동하지 않는 음량/이어하기/키 재지정 UI, 신규 패키지·아트 추가.
- 합성 oracle의 허용 결과: Startup 첫 화면에서 제목·문구·시작·설정·종료가 보이고, 설정은 한국어/영어·화면 모드·해상도·VSync·조작 안내·적용/취소/기본값을 제공하며, 적용 설정은 저장되고 시작은 `RatHost2DPrototype`을 로드한다.
- 완료 주장 한 문장: PC 실행 진입점이 다국어 준비형 Startup UI가 되고 한국어·영어 설정과 2D 프로토타입 시작 흐름이 보존 가능한 구조로 동작한다.

| criterion ID | 유형 | 입력·상태 | 기대값 | 최소 검증 |
| --- | --- | --- | --- | --- |
| C1 | 원증상/성공 | 앱/Startup 씬 진입 | 제목·태그라인·시작·설정·종료 표시, 설정 패널 닫힘 | 씬 계약+Play smoke |
| C2 | 상태 전이 | `프로토타입 시작` | `RatHost2DPrototype` 로드, 3D 레거시 시작 금지 | 순수 명령 테스트+씬 smoke |
| C3 | 성공/실패 | 설정 열기→언어/화면 초안 변경→취소/Esc | 언어 초안은 전체 Startup 문자열에만 즉시 preview되고 PlayerPrefs write·플랫폼 화면 적용은 0회다. 취소/Esc는 저장 언어와 전체 문자열을 원자 복원하고 모든 초안을 폐기한다. | 상태 trace+저장소/platform spy |
| C4 | 성공/경계 | 설정 변경→적용/기본값 | 기본 프로필은 `Korean`, `FullScreenWindow`, VSync `1`, 해상도 `1920×1080` 우선이다. 미지원이면 지원 목록 중 16:9 최고 해상도, 그것도 없으면 지원 목록 최고값, 목록이 비면 현재 화면값 순으로 고른다. 기본값 버튼은 이 결과를 Draft에만 적용하고 저장·플랫폼 적용 0회다. 적용은 Draft 검증→화면 적용→전체 저장 순서다. | 상태·저장소·platform spy+해상도 후보 표 테스트 |
| C5 | 다국어 | 한국어↔영어 Draft 전환 | 적용 전에도 모든 시작/설정/조작 문자열이 같은 render cycle에 preview된다. 적용 시 저장되고, 취소 시 저장 언어로 복원되며 누락 키 0·fallback은 결정론이다. | 키 완전성+preview/apply/cancel 씬 UI 테스트 |
| C6 | 수명주기 | 최초 실행/저장 후 새 세션/손상값 | 최초 실행과 schema·언어·화면모드·해상도·VSync 중 하나라도 손상/미지원이면 C4의 전체 기본 프로필로 원자 복귀한다. 정상 묶음만 그대로 복원한다. | 저장소 수명주기+부분 키/손상값 표 테스트 |
| C7 | 경계/가시성 | 960×540 및 16:9 PC 후보 | 버튼·제목·설정·긴 영문이 잘리지 않고 게임 화면을 가리지 않음 | 레이아웃 계약+Play 확인 |
| C8 | negative control | 변경 전후 기존 자산 | `RatHostPrototype`·2D 핵심 루프·패키지·사용자 reference 변경 없음 | 보호 diff |
| C9 | 종료/입력 | 종료·Esc·설정 복귀 | Editor에서는 안전, standalone 종료 경로 존재, Esc는 예측 가능하게 뒤로 이동 | 명령/씬 smoke |

- QA S0 사전 검토: r0 C3/C5 BLOCKER, r1 C4/C6 기본 프로필 BLOCKER 후 S0 r2 재분류 PASS. 구현 시작 허용.

## 고비용 preflight 입력

- agent brief JSON: `artifacts/agent-brief-implementation.json`, `artifacts/agent-brief-qa.json` — packet-only, fork_turns none, 필수 파일 3개 이하
- verification current-state JSON: `artifacts/verification-current-state.json`
- QA C# harness lint 경로: 신규 Startup 관련 EditMode 테스트 1개
- component contract baseline / candidate / test 경로: 기존 `EditorBuildSettings.asset` 및 Startup code/builder/test
- isolated Unity cache root / work ID marker: wrapper가 work ID 기준으로 발급
- low-level runner 직접 Run 금지 확인: 예

## 목적

게임 실행 직후의 첫인상과 설정 접근성을 만들고, 후속 보정에서 언어를 추가해도 UI 구현을 다시 뜯지 않도록 키 기반 로컬라이제이션 경계를 확보한다.

## 입력 자료

- 사용자 승인 및 다국어 고려 요청
- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `docs/agents/loop-engineering-gates.md`

## 해야 할 일

1. 새 `Startup` 씬과 UI 상태 경계를 설계한다.
2. 한국어·영어 키 카탈로그, 언어/화면 설정 모델과 저장소를 구현한다.
3. 제목/설정 UI를 씬에 연결하고 Build Settings 0번을 Startup으로 둔다.
4. `프로토타입 시작`은 `RatHost2DPrototype`으로 연결한다.
5. 표적 검증과 가능한 범위의 Unity Play 확인 뒤 사용자 화면 수용으로 넘긴다.

## 산출물

- Startup 런타임 코드와 EditMode 테스트
- `Assets/_Project/Scenes/Startup.unity` 및 생성 빌더
- `EditorBuildSettings.asset`의 Startup→RatHost2DPrototype 구성
- 작업 패킷·QA·총괄 기록

## production 소유권과 검증 예산

| production 파일/불변식 | 단일 구현 소유자 | 변경 금지/인계 조건 |
| --- | --- | --- |
| `Assets/_Project/Scripts/UI/Startup/StartupLocalization.cs`, `StartupSettings.cs`, `StartupController.cs`, 관련 설정 EditMode 테스트 | 게임플레이 구현 | 코드 API 인계와 소유권 해제 완료 |
| `Assets/_Project/Scripts/UI/Startup/StartupMenuView.cs`, 씬 계약 EditMode 테스트 | Unity 씬/통합 구현 | 공개 API만 사용해 UI wiring; 코어 3개 파일 수정 금지 |
| `Assets/_Project/Editor/Startup/**`, `Scenes/Startup.unity`, `EditorBuildSettings.asset` | Unity 씬/통합 구현 | 코드 API 인계 뒤 단일 owner 적용 |
| 3D/2D 기존 씬·패키지·아트 | 변경 금지 | 신규 Startup 연결만 허용 |

- Unity session lease 예정 소유자: 씬/통합 구현 1회, 독립 QA 1회 순차
- 관련 suite: 신규 Startup 설정/로컬라이제이션/씬 계약 EditMode
- 전체 suite 실행 조건: S1~S5 동일 fingerprint green이고 공유 Build Settings 회귀 위험이 남을 때 QA 1회만
- 대형 matrix 실행 필요·근거: 없음. 4개 해상도는 정적 레이아웃+대표 Play 1회로 축소
- artifact budget: S0 1, 구현 표적 XML 1, QA canonical manifest 1, 사용자 확인 화면 최대 2장

## 비용 계획·실제

| 비용 항목 | 계획 | 실제·근거 |
| --- | --- | --- |
| 역할·인계 | 조정1→아키텍처1+S0 QA1→총괄 사전1→코드1→씬1→독립 QA1→총괄 최종1 | 모든 구현·QA 역할 완료; 총괄 1차 상태-only blocker 동기화 뒤 read-only 재대조 PASS |
| 표적 검증 | 구현자 1묶음, QA 1묶음 | 기존 `32/32`는 배경 변경으로 SUPERSEDED; 최신 독립 정적 QA와 canonical UnityEditMode `33/33` PASS |
| Unity/MCP/빌드·full suite | Unity EditMode 최대2, MCP Play 최대1, build 0, full 최대1 | 작업 누적 **2/0/0**(기존 1 + 배경 revision 1), full 0; 서로 다른 revision의 preflight 차단 2건은 Unity 시작 0 |
| matrix/capture·artifact | 대형 matrix 0, 화면 최대2 | 0 |
| correction·무효/폐기·비용 판정 | 0/2, 첫 blocker 중지 | S0 r0/r1 blocker 2회→r2 재분류 0/2, 동적 실행·폐기 0, 미집계 |

- 중앙 현황판 행: `docs/project-handoff/task-cost-dashboard.md`

## 금지 범위

- Unity Localization 등 신규 패키지 설치
- 신규 이미지·폰트·오디오 에셋 생성/다운로드
- 작동하지 않는 음량, 저장/이어하기, 키 재지정 UI
- 인게임 일시정지 메뉴와 기존 HUD 다국어화
- 기존 3D 레거시·2D 게임플레이 씬·코드 변경
- 모바일 UI, 전체 게임 로컬라이제이션 완료 주장

## 승인 필요 항목

- 사용자가 본 V1 구현과 다국어 고려를 승인했다.
- 추가 패키지·외부 폰트·오디오·새 아트·지원 언어 확대는 별도 승인 대상이다.

## 커밋 전 차단 조건

- 작업 패킷·담당 산출물·agent-activity 기록
- QA 현재 fingerprint 독립 검증과 상태판/비용 대조
- 프로젝트 총괄 최종 `내부 승인 가능`
- 사용자에게 Startup 화면과 한·영 전환 수용 대기 명시

## 완료 기준

- C1~C9 기술 근거가 같은 freeze candidate에서 통과한다.
- 기존 3D/2D 씬·패키지·아트 보호 diff가 통과한다.
- 총괄 내부 승인 뒤 사용자가 초기 화면·설정·언어 전환을 수용한다.

## 2026-08-07 선택 배경 통합 revision

- 사용자 승인 원문: `docs/design/visual/references/image.png`를 시작 화면으로 선택하고, 내용에 맞게 파일명을 바꿔 실제 시작화면에 적용한다.
- 위험 등급: 기존 R3 유지 — 신규 PNG import와 Startup UI 가시 레이아웃 변경.
- verification revision: `startup-selected-background-integration-r1-20260807`
- correction cycle: 현 배경 revision `1/2` — run001 다중 harness 경로 binding preflight 차단 뒤 같은 범위의 단일 테스트 디렉터리 입력으로 run002 PASS. 작업 누적 preflight incident는 서로 다른 revision에서 2건이다.
- production owner: Unity 씬/통합 구현 에이전트 1명.
- Unity session lease 예정 소유자: 구현자 표적 확인 1회 후 독립 QA 1회. 고비용 실행은 공용 wrapper만 사용한다.

### 합성 oracle과 수용 기준

| ID | 기준 | 기대값 |
| --- | --- | --- |
| BG1 | 원본 명명·무결성 | 선택 PNG는 `startup-bacteriophage-food-chain-background-v1.png`로 이름이 바뀌며 `1672×941`, SHA-256 `5ED62B0BE9E0FC68FED15135C8BEDB3F08639CD020E914EF420FE73831B17C8D`를 유지한다. |
| BG2 | Unity import | Unity `Assets/_Project/` 아래에 같은 내용의 Sprite/UI용 PNG와 유일한 meta GUID가 존재한다. |
| BG3 | 실제 표시 | Startup 첫 프레임에서 선택 이미지가 전체 16:9 배경으로 표시되고 UI보다 뒤에 있으며 raycast를 가로채지 않는다. 누락 시 오류 없이 기존 어두운 배경으로 fallback한다. |
| BG4 | 메뉴 배치 | 제목·태그라인·시작·설정·종료는 이미지의 왼쪽 어두운 여백 안에서 읽히며 960×540 기준 잘림·우측 먹이사슬 핵심 가림이 없다. |
| BG5 | 설정 가독성 | 설정 패널을 열면 기존 필드·적용·취소·기본값이 배경 위에서도 읽히고 긴 영문과 해상도 표기가 잘리지 않는다. |
| BG6 | 회귀 | C1~C9의 로컬라이제이션, 설정 저장/취소, `RatHost2DPrototype` 시작 전이와 Build Settings 계약이 유지된다. |
| BG7 | 보호 경계 | 기존 2D/3D 플레이 씬, 패키지, 게임플레이 코드와 다른 비주얼 reference는 변경하지 않는다. |
| BG8 | 승인 경계 | 이번 PNG는 사용자 선택 시작화면 배경으로만 승격한다. 반복 타일·스프라이트·최종 게임플레이 아트로 확대 선언하지 않는다. |

### 변경 허용 범위

- 선택 원본의 설명적 rename과 Unity용 import copy.
- `StartupMenuView.cs`, Startup scene contract test, 필요한 `.meta` 및 Startup scene/UI wiring.
- 왼쪽 negative space에 맞추는 최소 UI 레이아웃 보정.
- `StartupLocalization.cs`, `StartupSettings.cs`, `StartupController.cs`, 기존 게임플레이 씬·패키지 변경은 금지한다.

### 최신 기술 증거

- canonical run: `startup-background-qa-20260807-002`
- canonical fingerprint: `be3e9ce5a76ff6951272a6a191a89018a7f28eeef182b30df878495c750d3649` / 17 inputs
- UnityEditMode: `33/33 PASS`, failed/skipped/inconclusive `0`, Unity exit `0`.
- 비용: 배경 revision Unity/MCP/build `1/0/0`; 작업 누적 `2/0/0`. full/matrix/capture `0`.
- run001: 두 QA harness 경로 전달 binding에서 preflight 차단, Unity 시작 `0`, `SUPERSEDED`.
- 남은 항목: McpPlay capability unavailable. 실제 960×540 첫 프레임·메뉴/설정 가독성·버튼 입력·화면 전이는 사용자 수용 대기.

## 2026-08-07 실제 Play 검은 배경·한글 도트 폰트 correction

- 사용자 원증상: “재생을 눌러도 검은 배경이 나오며 적용 여부를 확인할 수 없다. 폰트도 추가로 고려해 만들어야 한다.”
- 재현 증거: 실제 Editor 로그가 Play 진입 시 `Startup.unity`가 아니라 `Temp/__Backupscenes/0.backup`을 로드했다. 이전 EditMode 33/33은 Sprite 직렬화 계약만 확인해 Editor Play 진입점을 검증하지 못했다.
- 위험 등급: R3 유지 — Editor Play 진입점, Startup UI 런타임 폰트 asset·씬 직렬화·사용자 가시 화면 correction.
- verification revision: `startup-play-entry-font-profiles-r2-20260807`
- correction cycle: 새 사용자 acceptance 실패에 따른 correction `0/2`; 이전 기술 PASS는 사용자 원증상을 막지 못했으므로 `SUPERSEDED`.
- production owner: Unity 씬/통합 구현 에이전트 1명.
- 폰트 방향: `Galmuri11` Regular TTF. 한글 음절·라틴 지원 도트 폰트이며 공식 저장소의 SIL Open Font License 1.1 원문과 함께 반입한다.

### S0 합성 oracle

| ID | 유형 | 기대값 | 최소 검증 |
| --- | --- | --- | --- |
| PFC1 | 원증상 | Unity Editor에서 현재 열려 있던 씬과 무관하게 Play하면 저장된 `Assets/_Project/Scenes/Startup.unity`부터 시작하고 backup/빈 씬 검은 화면으로 진입하지 않는다. | Editor Play start-scene 계약+실제 Editor 로그/Play 확인 |
| PFC2 | 성공 | 첫 프레임에 선택 배경 Sprite가 전체 16:9로 표시되고 단색 검정만 보이지 않는다. | 런타임 Canvas/Background object·sprite·enabled·color·크기 확인+화면 캡처 |
| PFC3 | 폰트 | 제목·메뉴·설정의 한국어와 영어가 bundled `Galmuri11`로 렌더되며 정상 경로에서 `LegacyRuntime.ttf`를 사용하지 않는다. | Font asset/씬 직렬화·지원 글리프·runtime Text.font 확인 |
| PFC4 | 가독성 | 960×540에서 왼쪽 제목·버튼, 설정 패널 한국어/영어가 잘리지 않고 배경 대비가 충분하다. | 실제 960×540 main/settings 캡처와 bounds 확인 |
| PFC5 | 입력/전이 | 배경은 raycast를 차단하지 않고 설정 열기·취소와 `RatHost2DPrototype` 시작 경로가 유지된다. | raycastTarget·버튼/전이 smoke |
| PFC6 | 실패 경계 | Sprite 또는 Font 직렬화가 누락되면 명시적 오류를 남기되 검은 화면/무문자 상태를 성공처럼 숨기지 않는다. | missing-reference negative control |
| PFC7 | 라이선스 | 폰트 TTF, OFL 원문, 출처·버전·SHA-256이 프로젝트 안에서 함께 추적된다. | asset/license/source manifest 대조 |
| PFC8 | 보호 | Startup 외 게임플레이 씬·코어 설정 로직·패키지·기존 비주얼 reference는 변경하지 않는다. | 보호 diff |

### 변경 허용·금지

- 허용: `Assets/_Project/Editor/Startup/**`, Startup font asset/license/meta, `StartupMenuView.cs`, `Startup.unity`, 관련 Startup scene contract/test와 본 작업 기록.
- 금지: `StartupLocalization.cs`, `StartupSettings.cs`, `StartupController.cs`, Build Settings 추가 변경, 게임플레이 씬·패키지·다른 비주얼 asset 변경.
- 고비용 검증은 공용 wrapper와 single-owner lease만 사용한다. 실제 화면 확인 없는 EditMode PASS만으로 완료하지 않는다.

### S0 r1 정량 보완

- PFC3 r1 단일 폰트 문장은 `SUPERSEDED`다. exact glyph의 공통 정의만 유지하며 정상 runtime `Text.font` 동일성은 아래 r2의 현재 언어 profile을 기준으로 판정한다.
- PFC4 bounds: 960×540 main/settings 각 상태와 한국어·영어에서 모든 활성 `Text`의 preferred width/height가 해당 RectTransform 내부 여유 `1px` 안에 들어오고 `HorizontalWrapMode.Overflow`·`VerticalWrapMode.Overflow`로 잘림을 숨기지 않는다. 메인과 Settings 패널 모두 최악의 흰 배경 합성 가정에서 일반·오류·버튼 텍스트 contrast ratio `4.5:1` 이상, 32px 이상 제목은 `3:1` 이상이어야 한다.
- PFC6 r1의 generic font 오류 ID는 `SUPERSEDED`다. Sprite 누락은 `[StartupUI:PFC6_MISSING_BACKGROUND]`; 폰트 누락은 아래 r2의 `_KO`/`_EN` ID를 사용한다. 배경 누락은 순검정이 아닌 불투명 dark-plum 진단색으로 렌더하고, 폰트 누락은 Windows `Malgun Gothic`→built-in 순서의 명시적 fallback을 쓰되 정상 PASS로 간주하지 않는다.
- PFC7 pinned source: upstream commit `71e1cacf1437a11220307120e63e30bc275312d4`.
  - `dist/Galmuri11.ttf`: `5,376,428 bytes`, SHA-256 `E24256F42E43713D2EA086A1E1669D78B968F5B3CC547E5C157F0606FFA5DEF1`
  - `ofl.md` 저장명 `OFL.txt`: `4,266 bytes`, SHA-256 `9A9E5A342C430C3FCF01A408B680F4405D5BF4AC659C931BE35F8A1B27EA69C9`
  - source URL은 위 commit의 `raw.githubusercontent.com/quiple/galmuri/...` 두 경로로 고정하고 `SOURCE.md`에 기록한다.
- PFC1 원인 경계: backup scene 진입은 잘못된 Editor Play 경로의 증거이나 검은 렌더의 단일 원인으로 단정하지 않는다. Play start scene 교정과 first-frame background/font 검증을 독립 criterion으로 유지한다.

### S0 r2 언어별 폰트 프로필

- 사용자 추가 요구: 폰트는 지원 언어 각각의 문자 집합·글자폭·가독성을 고려한다.
- 우선순위: 이 r2 절이 r1의 단일 Galmuri font 동일성·generic missing-font ID를 명시적으로 대체한다. r1의 source hash, 공통 bounds, background 오류·fallback 계약은 충돌하지 않는 범위에서 유지한다.
- 현재 언어 매핑:
  - `Korean` → `Galmuri11.ttf`: 한글 음절과 한국어 UI 밀도에 맞춘 11px 계열 도트 폰트.
  - `English` → `Silkscreen-Regular.ttf`: 라틴 글자폭과 영문 픽셀 UI 가독성을 위한 별도 폰트.
- 언어 Draft preview가 바뀌는 같은 `Render()` cycle에서 모든 활성·비활성 Startup `Text`의 문자열과 Font profile을 함께 교체한다. 취소 시 저장 언어의 문자열·폰트를 함께 원자 복원한다.
- 언어별 exact glyph 검증:
  - Korean font는 한국어 localizer 전 키의 모든 비공백 문자 + 공용 숫자 `0`~`9`, `×`를 지원한다.
  - English font는 영어 localizer 전 키의 모든 비공백 문자 + 공용 숫자 `0`~`9`, `×`를 지원한다.
- 누락 오류: Korean font는 `[StartupUI:PFC6_MISSING_FONT_KO]`, English font는 `[StartupUI:PFC6_MISSING_FONT_EN]`. 정상 경로에서 한 언어의 폰트를 다른 언어 fallback으로 암묵 사용하지 않는다.
- English pinned source:
  - upstream `google/fonts` commit `c28e08582e7bd36751febb3391142a5eb18bbb34`
  - `ofl/silkscreen/Silkscreen-Regular.ttf`: `32,220 bytes`, SHA-256 `C845473330B94C2079CE9AF01C51AC8BA2D99C24F4D14C039843BBB8E642EBD8`
  - `ofl/silkscreen/OFL.txt`: `4,394 bytes`, SHA-256 `86C5E9C9382CDCC5948704FDFE60F2AA164A719746931219A42736ECD9CEFBD3`
  - 위 commit의 raw URL과 출처를 English font `SOURCE.md`에 기록한다.
- 폰트 license와 source manifest는 font family별 폴더에 분리한다. 서로 다른 OFL 파일을 하나로 합치지 않는다.

### r2 최신 기술 증거·비용

- canonical run: `startup-play-font-qa-20260807-001`
- canonical fingerprint: `22eef3ed`로 시작하는 34-input manifest의 전체 값은 `artifacts/candidate-fingerprint.json`을 따른다.
- UnityEditMode: `38/38 PASS`, failed/skipped/inconclusive `0`, Unity exit `0`.
- 이전 `startup-background-qa-20260807-002` / `33/33`은 현재 revision에서 `SUPERSEDED`.
- 작업 누적 Unity/MCP/build `3/0/0`; 현재 r2 revision `1/0/0`; full/matrix/capture `0`.
- correction: S0 계약 correction `1/2`; 현재 동적 QA/preflight correction `0/2`.
- McpPlay unavailable. post-correction 실제 first frame, 960×540 배경·언어별 폰트·bounds/가독성, 설정 preview/cancel, raycast·2D 전이·Console은 사용자 수용 대기.
