# 작업 배정서

## 기본 정보

- 작업 ID: `2026-08-07-main-scenario-outline`
- 작업명: 전체 게임 시나리오·화면 흐름·튜토리얼 초안
- 상태: Task 4 Step 6 content `6867b98` origin/main 반영 완료
- 생성일: 2026-08-07
- 담당 에이전트: 메인 시나리오 디렉터 에이전트
- 보조 에이전트: 프로젝트 조정 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `last-host-design-keeper`

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| 기획 정리 | production 문서 작성 | 기존 기획과 승인 범위를 보존한 전체 시나리오 초안 | `docs/design/narrative/main-scenario-outline.md` |
| 메인 시나리오 디렉터 | Task 4 Step 6 production 소유 | 승인된 오프닝·기원 공개·세균/운반 숙주 계약을 상위 시나리오에 최소 전파 | `docs/design/narrative/main-scenario-outline.md` revision 2 |
| 프로젝트 조정 | 통합·상태 기록 | 문서 구조, 색인, R2 기록 통합 | 작업 패킷과 색인 갱신 |
| 프로젝트 총괄 | 내부 감사 | 범위·승인 게이트·미확정 표시 검토 | 내부 승인 또는 수정 판정 |

## 구현 담당 확인

- 코드/테스트 변경 담당: 해당 없음
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: 해당 없음
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 해당 없음

## 루프 게이트

- 게이트 적용 대상: 예
- 위험 등급: R2
- 위험 등급 근거: 전체 캠페인·튜토리얼·숙주 체인·엔딩 제안을 포함하는 신규 상위 설계 문서
- 적용 사유: 현재 승인된 쥐 숙주 프로토타입과 장기 제안을 분리해야 함
- QA/검증 필요: 예 — 정적 문서 대조
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예
- correction cycle: 2/2
- Task 4 Step 6 correction 의미: 승인된 새 오프닝 계약의 production revision 자체는 기존 오류 correction이 아니다. 다만 QA `qa-main-scenario-opening-sync-20260810-001`이 실제 기준선 `6347445`와 상태 문서의 `f9e8bd0` 현재값 불일치를 first blocker로 판정했으므로 상태 교정을 correction 2로 기록한다.
- capability profile / 요청 route: 문서 정적 검토
- attempt ledger 경로 / 같은 criterion 연속 실패 수: 해당 없음 / 0

## S0 사용자 원증상·검증 charter

- 사용자 원문 또는 원증상: 전체 시나리오 안에 게임 내 씬 연출, 게임 화면과 흐름, 튜토리얼 등 처음부터의 전반적인 내용을 담은 초안이 필요함
- 재현 씬·입력·좌표·상태: 해당 없음
- 원증상 증거: `docs/design/narrative/`에 상세 전체 시나리오가 없음
- 합성 oracle의 금지 결과: 장기 제안을 현재 구현 승인으로 오인하거나 쥐 숙주 프로토타입 범위를 임의 확대함
- 합성 oracle의 허용 결과: 처음부터 엔딩까지의 흐름을 읽을 수 있고, 화면·튜토리얼·플레이·연출·미확정 항목이 구분된 초안
- 완료 주장 한 문장: 전체 게임의 첫 실행부터 엔딩까지 이어지는 검토용 시나리오 초안과 색인이 생성되었다.

| criterion ID | 유형 | 입력·상태 | 기대값 | 최소 검증 |
| --- | --- | --- | --- | --- |
| C1 | 성공 | 신규 시나리오 문서 | 시작 화면, 도입, 튜토리얼, 핵심 루프, 숙주·스테이지 진행, 엔딩 포함 | 목차·본문 정적 확인 |
| C2 | 경계 | 승인 범위 대조 | 쥐 프로토타입 확정과 전체 캠페인 제안이 명확히 구분됨 | 기준 문서와 문구 대조 |
| C3 | 성공 | 플레이 흐름 | 화면, 조작 학습, 실패·재시도, 보상·복귀가 연결됨 | 단계별 흐름 대조 |
| C4 | negative control | 구현 범위 | Unity·코드·에셋 변경이나 구현 승인으로 선언하지 않음 | Git diff·문서 문구 확인 |
| C5 | 수명주기 | 문서 탐색 | narrative README에서 신규 문서를 찾을 수 있음 | 링크 확인 |

### Task 4 Step 6 오프닝 계약 전파 criterion

| criterion ID | 유형 | 입력·상태 | 기대값 | 최소 검증 |
| --- | --- | --- | --- | --- |
| C6 | 성공 | 구간 2 오프닝 | 독립 3씬→복합 환승 확산·차등 공백→C01→심야 노동자→비말·배수→각성→커스터마이징→E03/T01 첫 입력 순서가 이어짐 | 본문 순서 대조 |
| C7 | 생물학 경계 | 파지·숙주 표현 | 동물세포 직접 감염 금지, 세균 숙주와 운반 숙주 구분 | 용어·금지 문구 대조 |
| C8 | 공개 경계 | 오프닝·후반 기원 | 기원 공개 장부는 shot spec 링크가 canonical이며 오프닝에 연구소·전쟁·군사화를 폭로하지 않음 | 링크·오프닝 문구 대조 |
| C9 | 편집 계약 | 33 ID·Gate S | 상위 문서에는 요약·링크만 있고 상세 계약을 복제하지 않음 | 중복·링크 대조 |
| C10 | 시간 경계 | 최초 도입 길이 | 10~20초 후보 제거, 최종 숏·패널·총시간 미확정, C01 한국어 최대 2줄·완전 가독 최소 5초만 유지 | 시간 문구 검색 |
| C11 | downstream 정합 | 구간 3~8·프롤로그·Q01/Q02/Q12·승인 경계 | 벌레 직접 감염 도입을 세균 감염 접속형과 첫 운반 숙주 후보로 최소 교정 | 표·질문·경계 대조 |
| C12 | 범위 | 쥐 프로토타입 | 승인 범위 불변, 장기 튜토리얼·팝업·전체 체인 구현 미승인 | 6장·13장 대조 |
| C13 | negative control | 저장소 변경 | Unity·코드·에셋 변경 0, 새 파일 0 | Git diff·status 확인 |
| C14 | 수명주기 | Markdown 링크·placeholder | 내부 링크 대상 존재, 새 TODO/TBD·깨진 placeholder 없음 | 링크·placeholder 검사 |
| C15 | 원자성 | production revision | author run 뒤 SHA-256와 diff check를 같은 candidate에 기록 | SHA256·diff check |

- QA 재진입 전 상태: `qa-main-scenario-opening-sync-20260810-001` FAIL — production 의미·SHA는 유효하나 HEAD 상태 증거는 SUPERSEDED.
- 상태 교정 작성자 run: `author-main-scenario-opening-sync-state-correction-20260810-r2` PASS — Task 4 Step 6 작업 시작 baseline `6347445`(origin/main)의 precommit candidate를 검증했다.
- canonical QA: `qa-main-scenario-opening-sync-20260810-002` C1~C15 PASS·blocker 0·수정 0. QA-001은 SUPERSEDED이며 프로젝트 총괄 감사 전 완료 표현을 금지한다.
- 최종 총괄: `director-main-scenario-opening-sync-final-audit-20260810-003` 내부 승인 가능·blocker 0·최소 수정 0. 판정 범위는 Task 4 Step 6 상위 시나리오 전파에 한정한다.
- push 결과: content commit `6867b98` (`docs: sync opening into main scenario`)을 origin/main에 반영했다. 새 QA·총괄 run은 0이다.
- 다음 승인 게이트: Gate S를 실제 배치한 스토리보드 후보와 결과 패널·숏 수, 이미지 후보 생성 여부. Task 5 Step 6 실제 연결 측정은 스토리보드·비최종 애니매틱 연결 뒤 수행한다.

- QA S0 사전 검토: 최초 C2 승인 표기와 C5 증거 부족으로 FAIL, correction 1에서 C1~C5 PASS

## 목적

사용자가 구체 내용을 보완할 수 있도록 전체 캠페인의 이야기, 실제 게임 화면, 조작 학습, 시스템 개방, 모드 전환, 실패와 엔딩을 하나의 기준 초안으로 연결한다.

## 입력 자료

- `docs/design/game-design-summary.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/design/visual/characters/base-bacteriophage-character.md`
- `docs/design/narrative/opening/opening-cinematic-origin.md`
- `docs/design/narrative/opening/opening-shot-spec.md` v12
- `docs/design/narrative/opening/opening-cinematic-production-plan.md`
- `docs/design/hosts/host-map-transfer-route.md`

## 해야 할 일

1. 확정·제안·미정 상태를 먼저 정의한다.
2. 시작 화면부터 첫 플레이, 튜토리얼, 장기 숙주 체인, 결말까지 플레이 흐름을 작성한다.
3. 컷신과 인게임 연출, UI 안내, 실패·복귀 구조를 함께 명시한다.
4. 쥐 숙주 수직 슬라이스와 정식 캠페인 위치를 구분한다.
5. 내러티브 README 색인을 갱신한다.

## 산출물

- `docs/design/narrative/main-scenario-outline.md`
- `docs/design/narrative/README.md`
- R2 `task.md`, `verification.md`

## production 소유권과 검증 예산

| production 파일/불변식 | 단일 구현 소유자 | 변경 금지/인계 조건 |
| --- | --- | --- |
| 전체 시나리오 초안 | 기획 정리 에이전트 | 승인된 쥐 프로토타입을 장기 캠페인 확정으로 확대하지 않음 |
| 전체 시나리오 초안 Task 4 Step 6 revision | 메인 시나리오 디렉터 에이전트 | opening production 3종은 소비만 하며 33 ID·Gate S 상세를 복제하지 않음 |
| narrative 색인 | 프로젝트 조정 에이전트 | 기존 폴더 성격 유지 |

- Unity session lease 예정 소유자: 해당 없음
- 관련 suite: Markdown 정적 대조
- 전체 suite 실행 조건: 없음
- 대형 matrix 실행 필요·근거: 없음
- artifact budget / criterion별 canonical 증거: production 문서와 `verification.md`만 사용

## 비용 계획

| 비용 항목 | 계획 |
| --- | --- |
| 역할·인계 | 기획1 → 조정 통합1 → 총괄1 |
| 표적 검증 | 문서 구조·링크·범위 정적 대조 1묶음 |
| Unity/MCP/빌드·full suite | 0 |
| matrix/capture·artifact | 0 |

- 중앙 현황판 대상 여부·행: R2이므로 대상 / `docs/project-handoff/task-cost-dashboard.md`

## 금지 범위

- Unity 프로젝트, 코드, 씬, 패키지, ProjectSettings, 아트 에셋 변경
- 장기 캠페인 제안을 구현 승인 또는 확정 설정으로 선언
- 기존 쥐 숙주 프로토타입 범위의 임의 변경

## 승인 필요 항목

- 사용자가 보완한 뒤 전체 숙주 순서, 주요 사건, 엔딩 조건의 최종 확정
- 벌레 튜토리얼과 전체 캠페인의 실제 구현 착수

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 필요
- 담당 에이전트 산출물 확인: 필요
- 에이전트 수행 이력 확인: 필요
- 구현 담당 에이전트 확인: 해당 없음
- 메인 에이전트 직접 구현 예외 사유 확인: 해당 없음
- QA/검증 에이전트 기록 확인: 정적 대조 필요
- 총괄 관리자 판정 확인: 필요
- 승인 게이트 확인: 장기 제안과 구현 승인 분리
- 완료 판단에 영향을 주는 미검증 항목: 사용자 내용 보완과 최종 수용

## 완료 기준

- 사용자가 한 파일에서 게임의 처음부터 엔딩까지 전반 흐름을 검토할 수 있다.
- 씬 연출, 인게임 화면, 튜토리얼, 핵심 루프, 실패·보상·분기가 포함된다.
- 확정 사항과 미확정 제안이 혼동되지 않는다.
