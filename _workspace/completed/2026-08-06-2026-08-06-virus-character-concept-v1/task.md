# 작업 배정서

## 기본 정보

- 작업 ID: `2026-08-06-virus-character-concept-v1`
- 작업명: 바이러스 주인공 의인화 수준 콘셉트 3종 비교
- 상태: 완료 보관 준비 — 사용자 제공 박테리오파지 기준 반영·QA·총괄 완료
- 생성일: 2026-08-06 KST
- 담당 에이전트: ChatGPT 이미지 아트 에이전트
- 보조 에이전트: 비주얼/테크아트, QA/검증, 프로젝트 총괄 관리자
- 사용 스킬: `imagegen`, `last-host-design-keeper`, `pixel-lowpoly-style-keeper`

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| 조정자 | 범위·S0·사용자 보고 | 작업 패킷, 후보 통합, 선택 요청 | `task.md`, 통합 보고 |
| ChatGPT 이미지 아트 | 생성 owner | 공통 브리프와 A/B/C 생성·로그·1차 점검 | PNG 3개, `generation-log.md` |
| 비주얼/테크아트 | 시각 적합성 검토 | 실루엣·픽셀·변이 확장성 비교 | 후보별 판정·추천 |
| QA/검증 | 독립 검증 | C1~C7, 파일·출처·금지 범위 대조 | 독립 QA 판정 |
| 프로젝트 총괄 | 내부 승인 | 기획·범위·QA·사용자 선택 경계 감사 | 내부 승인 판정 |

## 구현 담당 확인

- 코드/테스트 변경 담당: 해당 없음
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: 해당 없음
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 해당 없음

## 루프 게이트

- 게이트 적용 대상: 예
- 위험 등급: R2
- 위험 등급 근거: 신규 래스터 콘셉트 3종 생성과 사용자 가시 비주얼 방향 결정에 영향을 줌
- 적용 사유: 생성 owner, 독립 QA, 총괄 판정과 사용자 선별이 필요함
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예
- correction cycle: 1/2
- capability profile / 요청 route: 사용자 제공 reference 비파괴 복사·정적 문서 검토, 새 이미지 생성·Unity route 없음
- attempt ledger 경로 / 같은 criterion 연속 실패 수: `verification.md` / 0

## S0 사용자 원증상·검증 charter

- 사용자 원문 또는 원증상: 사용자 제공 `docs/references/images/image.png`와 후속 프롬프트를 가장 기본 바이러스(박테리오파지) 기준으로 사용하고 프로젝트 내용을 갱신한다.
- 재현 씬·입력·좌표·상태: 사용자 원본 이미지·프롬프트를 canonical reference와 캐릭터 기준 문서로 반영하고 프로젝트의 2D 제작 계약과 대조한다.
- 원증상 증거: 사용자가 이전 A/B/C 대신 별도 박테리오파지 reference와 프롬프트를 기본 바이러스 기준으로 지정했다.
- 합성 oracle의 금지 결과: 원본 변형·해시 불일치, 3D voxel을 production 경로로 전환, 제3자 스타일 모사 승계, 이미지의 150nm·행동·업그레이드 텍스트 자동 기획 확정, A/B/C를 현재 후보로 유지, 실제 시트·Unity 적용 완료 주장.
- 합성 oracle의 허용 결과: 원본과 동일한 canonical reference, 박테리오파지 외형·성격·업그레이드 불변식, 3D reference/2D production 경계, 후속 별도 승인 조건이 관련 문서에 일관되게 기록됨.
- 완료 주장 한 문장: 사용자 제공 박테리오파지 이미지와 프롬프트를 기준 캐릭터 reference·디자인 계약으로 기록하고, 확정된 2D 아이소메트릭 도트 제작 경계에 맞게 프로젝트 문서를 갱신한다.

### 현재 correction 1 criterion

| criterion ID | 유형 | 입력·상태 | 기대값 | 최소 검증 |
| --- | --- | --- | --- | --- |
| COR1 | 성공 | 원본/canonical PNG | bytes·1036×1248·SHA-256이 동일 | 파일·해시·원본 시각 대조 |
| COR2 | 성공 | canonical reference·기준·색인 | 모든 경로가 존재하고 source·용도·한계가 추적됨 | 링크·색인 검사 |
| COR3 | 성공 | 외형·성격·성장 계약 | 캡시드 60~70%, 2~3마디, 팔2·다리2·작은 꼬리·둥근 spike·팔레트, 성격, 기본 실루엣 유지·허용 추가요소 명시 | 캐릭터 기준 문서 계약 검색 |
| COR4 | 경계 | 제작·기획·후속 승인 | 3D voxel은 reference, production은 방향별 2D 아이소메트릭 도트이며 150nm·이미지 텍스트는 자동 확정하지 않고 실제 시트·Unity는 별도 승인 | 기준·reference·상위 방향 문서 대조 |
| COR5 | negative control | 제3자 스타일명 | raw 사용자 artifact 밖 production/design 문서의 제3자 스타일 모사 명칭 0 | `docs/` 정적 검색 |
| COR6 | 수명주기 | A/B/C와 canonical 우선순위 | A/B/C는 `SUPERSEDED`, 사용자 제공 박테리오파지 reference가 현재 기준 | task·verification·기준·색인·상태판 대조 |
| COR7 | negative control | 생성·반입·실행 | 새 imagegen·UnityProject 반입·MCP·build 0 | 파일·작업 기록·상태판 대조 |
| COR8 | 경계 | 작업 트리 | correction 대상 diff와 기존 unrelated Unity dirty 변경을 분리·보존 | Git 상태·mtime·대상 diff 대조 |

- 이전 A/B/C 비교 C1~C7은 `brief-v1-anthropomorphism-levels@qa-20260806`의 `SUPERSEDED` 이력이며 현재 완료 주장에 사용하지 않는다.
- QA S0 검토: correction 1 독립 정적 QA가 COR1~COR8 PASS를 기록했고 총괄은 task의 정식 criterion 동기화를 요구했다.

## 목적

시작 화면과 내부 바이러스 모드에 공통으로 이어질 기본 박테리오파지의 외형·성격·업그레이드 불변식 reference를 확정하고 2D 제작 경계를 기록한다.

## 사용자 선택 correction 1

- 2026-08-06 사용자는 기존 A/B/C 중 하나를 그대로 선택하지 않고 `docs/references/images/image.png`와 후속 영문 프롬프트를 기본 박테리오파지 캐릭터 기준으로 지정했다.
- 기존 A/B/C는 의인화 강도 탐색 이력으로 `SUPERSEDED` 처리한다.
- 선택 기준은 큰 정이십면체 캡시드 머리, 2~3마디의 매우 짧은 몸통, 짧은 팔 2개와 다리 2개, 작은 파지 꼬리, 짧고 둥근 표면 돌기, 친근한 표정, 보라·분홍·흰색 팔레트다.
- `3D voxel` 표현은 외형 탐색 reference다. 실제 게임 에셋은 현재 확정된 2D 아이소메트릭 도트 스프라이트로 재설계한다.
- 사용자 원문에 포함된 특정 회사·캐릭터명 스타일 문구는 모사 지시로 재사용하지 않고 `친근하고 장시간 보아도 피로하지 않은 고품질 게임 마스코트`라는 일반 기준으로 치환한다.
- 기준 reference 원본: `1036×1248`, `1,882,663 bytes`, SHA-256 `0C1D22C07C0CAC8B2F70D7BEFCFB5FA5E6ECB66D0F183125DFF359D33CEA039F`.

## 입력 자료

- `docs/design/game-design-summary.md`
- `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png` — 분위기·픽셀 밀도 reference, 편집 대상 아님
- `docs/design/visual/graphics-direction-management.md`
- `docs/design/visual/pixel-isometric-2d-production-guide.md`
- `docs/prototype/plans/rat-host-ai-assisted-art-workflow.md`

## 해야 할 일

1. 기존 A/B/C를 `SUPERSEDED` 탐색 이력으로 보존한다.
2. 사용자 원본을 canonical visual reference 위치에 비파괴 복사하고 출처·해시·용도·한계를 기록한다.
3. 박테리오파지 기준 캐릭터 문서를 새 visual/characters 하위 폴더와 README 색인으로 작성한다.
4. 게임 기획 요약·비주얼 방향·reference 색인을 선택된 기준과 2D 재해석 경계에 맞게 최소 갱신한다.
5. 독립 정적 QA와 총괄 감사를 거쳐 사용자에게 반영 결과와 남은 승인 항목을 보고한다.

## 산출물

- 기존 탐색 이력: `artifacts/candidates/virus-concept-*.png`, `artifacts/generation-log.md`
- canonical reference: `docs/design/visual/references/bacteriophage-base-character-reference-v1.png`
- 기준 문서: `docs/design/visual/characters/base-bacteriophage-character.md`
- 색인: `docs/design/visual/characters/README.md`, `docs/design/visual/README.md`, `docs/design/visual/references/README.md`, `docs/design/README.md`
- 상위 방향 최소 갱신: `docs/design/game-design-summary.md`, `docs/design/visual/graphics-direction-management.md`
- 검증 기록: `verification.md`

## production 소유권과 검증 예산

| production 파일/불변식 | 단일 구현 소유자 | 변경 금지/인계 조건 |
| --- | --- | --- |
| 사용자 reference 원본과 canonical 복사본 | 문서/릴리즈 에이전트 | 원본 불변, 복사본 해시 동일, 새 생성·보정 금지 |
| 박테리오파지 기준·색인 문서 | 문서/릴리즈 에이전트 | 선택된 외형만 기록, 구현 규격·Unity 적용으로 승격 금지 |
| A/B/C PNG와 생성 로그 | 변경 없음 | `SUPERSEDED` 이력으로 보존 |
| UnityProject 전체 | 변경 없음 | 별도 사용자 승인 전 Import·씬 적용 금지 |

- Unity session lease 예정 소유자: 해당 없음
- 관련 suite: 정적 시각·파일 무결성 검토
- 전체 suite 실행 조건: 해당 없음
- 대형 matrix 실행 필요·근거: 없음
- artifact budget / criterion별 canonical 증거: PNG 3개 + generation log 1개

## 비용 계획

| 비용 항목 | 계획 |
| --- | --- |
| 역할·인계 | 기존 조정·이미지 아트·비주얼·QA·총괄 + correction 문서/릴리즈1 → 독립 QA1 → 총괄1 |
| 표적 검증 | reference 원본/복사본 해시·해상도 1묶음 + 문서 링크·금지 문구·2D 경계 정적 대조 1묶음 |
| Unity/MCP/빌드·full suite | 0/0/0, full suite 0 |
| matrix/capture·artifact | matrix 0, 별도 capture 0, canonical reference 1 + 기준 문서·색인 |

- 중앙 현황판 대상 여부·행: R2이므로 대상 / `docs/project-handoff/task-cost-dashboard.md`

## 금지 범위

- 새 이미지 자동 생성 또는 사용자 원본 수정
- 시작 화면 배경 재생성
- 최종 스프라이트 시트·애니메이션 제작 또는 Unity 적용
- 쥐나 다른 숙주를 후보 이미지에 포함
- 인간·병원·연구소·백신·엔딩 직접 묘사
- 글자·로고·HUD·워터마크·제3자 캐릭터 또는 특정 작가 스타일 모사

## 승인 필요 항목

- 기본 박테리오파지 기준 선택: 사용자 승인 완료
- 턴어라운드·표정·변이 시트 및 2D 실제 스프라이트 제작은 별도 승인 필요
- 시작 화면 반영과 Unity Import·적용은 별도 승인 필요

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 생성 완료
- 담당 에이전트 산출물 확인: correction 문서 반영 대기
- 에이전트 수행 이력 확인: 대기
- 구현 담당 에이전트 확인: 이미지 아트 owner 지정
- 메인 에이전트 직접 구현 예외 사유 확인: 해당 없음
- QA/검증 에이전트 기록 확인: 기존 후보 QA 완료, correction 문서 QA 대기
- 총괄 관리자 판정 확인: correction 1 `내부 승인 가능`
- 승인 게이트 확인: 사용자 제공 박테리오파지 기준 채택과 문서 갱신 승인됨
- 완료 판단에 영향을 주는 미검증 항목: 실제 2D 스프라이트 재제작·축소 가독성·Unity 적용

## 완료 기준

- 사용자 원본과 canonical reference 복사본의 해시가 일치한다.
- 기준 문서와 관련 색인이 선택된 외형·성격·업그레이드 불변식·2D 재해석 경계를 일관되게 기록한다.
- 특정 제3자 스타일 모사 문구를 제작 지시로 승계하지 않는다.
- 독립 QA와 총괄 판정이 correction 1 revision을 지지한다.
