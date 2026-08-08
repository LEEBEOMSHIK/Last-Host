# 작업 배정서

## 기본 정보

- 작업 ID: `2026-08-08-opening-cinematic-origin`
- 작업명: 오프닝 시네마틱·주인공 돌연변이 기원 설계와 제작 태스크 분해
- 상태: 승인 대기
- 생성일: 2026-08-08
- 담당 에이전트: 픽셀아트 시네마틱 연출 에이전트
- 보조 에이전트: 프로젝트 조정 에이전트, QA/검증 에이전트, 프로젝트 총괄 관리자
- 사용 스킬: `superpowers:brainstorming`, `superpowers:writing-plans`, `last-host-design-keeper`, `pixel-lowpoly-style-keeper`

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| 픽셀아트 시네마틱 연출 | 기획·연출 설계 | 승인된 오프닝 사건을 시퀀스·전환·후속 제작 태스크로 구조화 | 오프닝 설계 초안과 제작 계획 |
| 프로젝트 조정 | 통합 | 기존 설정·승인 경계 대조, 문서 배치, 기록 통합 | production 문서와 R2 기록 |
| QA/검증 | 독립 정적 QA | 아래 C1~C6 및 변경 범위 확인 | `verification.md` QA 판정 |
| 프로젝트 총괄 | 내부 감사 | 인간·파지 설정, 장기 캠페인 승인 경계 판정 | 내부 승인 가능 여부 |

## 구현 담당 확인

- 코드/테스트 변경 담당: 해당 없음
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: 해당 없음
- 메인 에이전트 직접 구현 여부: 아니오 — 조정·통합만 수행
- 메인 에이전트 직접 구현 예외 사유: 해당 없음

## 루프 게이트

- 게이트 적용 대상: 예
- 위험 등급: R2
- 위험 등급 근거: 전체 캠페인의 오프닝과 주인공 기원이라는 새 서사 계약을 고정하되 실행 파일은 변경하지 않는다.
- 적용 사유: 기존 숲 바닥 도입 초안, 박테리오파지 생물학 경계, 쥐 프로토타입 승인 범위와 분리 필요
- QA/검증 필요: 예 — 독립 정적 QA 1회
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예
- correction cycle: 0/2
- capability profile / 요청 route: Markdown 정적 문서 검증, Unity route 없음
- attempt ledger 경로 / 같은 criterion 연속 실패 수: 해당 없음 / 0

## S0 사용자 원증상·검증 charter

- 사용자 원문 또는 원증상: 게임 시작 뒤 시민들의 평범한 일상과 감염병 확산을 시네마틱으로 보여주고, 청소 노동자의 기침에서 나온 주인공 파지가 커스터마이징과 튜토리얼로 이어지며, 주인공의 기원은 방어·치료 연구가 전쟁 중 생화학 무기 연구로 변질되어 탄생한 미스터리로 정리한다.
- 재현 씬·입력·좌표·상태: 해당 없음 — 내러티브 문서 계약
- 원증상 증거: 기존 `main-scenario-outline.md`의 숲 바닥 도입은 이번 사용자 승인 방향과 다름
- 합성 oracle의 금지 결과: 청소 노동자를 최초 원인으로 비난, 파지가 인간 세포를 직접 감염한다고 표현, 오프닝에서 무기 기원을 전부 폭로, 인간 전체를 단순 악역으로 규정, 실제 생화학 무기 제작 절차 포함, 전체 캠페인·Unity 구현 승인으로 확대
- 합성 oracle의 허용 결과: 평범한 일상→감염 확산→기침·비말→배수 환경→주인공 각성→커스터마이징→튜토리얼 전환이 연결되고, 방어 연구의 군사화 기원은 후반 미스터리로 남음
- 완료 주장 한 문장: 승인된 오프닝과 돌연변이 기원을 검토 가능한 설계 문서와 승인 게이트별 제작 태스크로 확인할 수 있다.

| criterion ID | 유형 | 입력·상태 | 기대값 | 최소 검증 |
| --- | --- | --- | --- | --- |
| C1 | 성공 | 오프닝 사건 | 일상·확산·기침·각성·커스터마이징·튜토리얼 순서 연결 | 설계 문서 대조 |
| C2 | negative control | 청소 노동자 역할 | 원인·환자 0이 아닌 구조적 피해자 | 금지·허용 문구 확인 |
| C3 | 경계 | 박테리오파지 설정 | 인간 세포 직접 감염이 아닌 감염 세균·비말 운반 | 생물학 경계 확인 |
| C4 | 성공 | 돌연변이 기원 | 방어·치료 연구가 전쟁 중 무기화되고 기원은 점진 공개 | 기원·공개 단계 확인 |
| C5 | 수명주기 | 제작 계획 | 지금 가능한 문서와 별도 승인 대상 이미지·애니매틱·Unity 분리 | 태스크 게이트 확인 |
| C6 | 실패 | 범위 | 장기 캠페인과 쥐 프로토타입 구현 승인을 확대하지 않음 | 승인 경계 확인 |

- QA S0 사전 검토: production 작성 후 요청

## 고비용 preflight 입력

- agent brief JSON: 해당 없음 — Unity/MCP/build 미사용
- verification current-state JSON: 해당 없음
- QA C# harness lint 경로: 해당 없음
- component contract baseline / candidate / test 경로: 해당 없음
- isolated Unity cache root / work ID marker: 해당 없음
- low-level runner 직접 Run 금지 확인: 고비용 실행 0회 유지

## 목적

사용자와 차근차근 확장할 메인 시나리오의 첫 구간을 독립 설계하고, 실제 제작 전에 필요한 의사결정과 작업 순서를 명확히 한다.

## 입력 자료

- `docs/design/narrative/main-scenario-outline.md`
- `docs/design/narrative/pixel-art-motion-comic-cinematic-guide.md`
- `docs/design/hosts/host-map-transfer-route.md`
- `docs/design/visual/characters/base-bacteriophage-character.md`
- 사용자 승인 대화: 현실 기반 혼합형 돌연변이, 방어·치료 연구의 전쟁 중 무기화, 미스터리 기원

## 해야 할 일

1. 오프닝 시퀀스와 플레이 전환 설계를 문서화한다.
2. 돌연변이 능력의 현실 기반·게임적 확대·초고도 변이 층을 정리한다.
3. 기원 미스터리의 확정 사실과 공개 금지 정보를 나눈다.
4. 현재 제작 가능한 문서 작업과 별도 승인이 필요한 제작 작업을 계획으로 분해한다.
5. 독립 정적 QA와 총괄 판정을 기록한다.

## 산출물

- `docs/design/narrative/opening/README.md`
- `docs/design/narrative/opening/opening-cinematic-origin.md`
- `docs/design/narrative/opening/opening-cinematic-production-plan.md`
- `_workspace/active/2026-08-08-opening-cinematic-origin/verification.md`

## production 소유권과 검증 예산

| production 파일/불변식 | 단일 구현 소유자 | 변경 금지/인계 조건 |
| --- | --- | --- |
| 오프닝·기원 설계 | 픽셀아트 시네마틱 연출 에이전트 | 이미지·애니매틱·Unity 제작 금지, 사용자 승인 사실만 확정 |
| 제작 태스크 계획 | 픽셀아트 시네마틱 연출 에이전트 | 승인 게이트와 담당 역할을 생략하지 않음 |
| 문서 구조·기존 설정 정합성 | 프로젝트 조정 에이전트 | 기존 미커밋 메인 시나리오 파일 수정 금지 |

- Unity session lease 예정 소유자: 없음
- 관련 suite: C1~C6 Markdown 정적 대조
- 전체 suite 실행 조건: 해당 없음
- 대형 matrix 실행 필요·근거: 없음
- artifact budget / criterion별 canonical 증거: production Markdown 3개와 `verification.md`

## 비용 계획

| 비용 항목 | 계획 |
| --- | --- |
| 역할·인계 | 시네마틱 연출 1 → 조정 통합 1 → 독립 QA 1 → 총괄 1 |
| 표적 검증 | 정적 구조·범위·링크 대조 각 1회 |
| Unity/MCP/빌드·full suite | 0 |
| matrix/capture·artifact | 0 |

- 중앙 현황판 대상 여부·행: R2 대상이나 현황판에 기존 다른 작업의 미커밋 변경이 있어 본 `verification.md`에 비용을 단일 기록하고 사용자 검토 전 중복 편집하지 않는다.

## 금지 범위

- 이미지·영상·애니매틱·오디오 생성
- Unity 코드·씬·프리팹·Timeline·패키지·ProjectSettings 변경
- 생화학 무기의 구체적 제작 절차·물질·유전자·배양 조건 설계
- 기존 미커밋 `main-scenario-outline.md`, `pixel-art-motion-comic-cinematic-guide.md`, 상위 narrative README 수정
- 청소 노동자를 감염병 원인이나 도덕적 책임자로 묘사
- 쥐 숙주 프로토타입 또는 전체 캠페인 구현 승인 확대

## 승인 필요 항목

- 시작 문구 최종안
- 오프닝 숏·스토리보드 후보
- 생성 이미지 후보 제작
- 비최종 애니매틱 제작
- Unity 재생 구현과 튜토리얼 연결

## 완료 기준

- C1~C6 독립 정적 QA PASS
- 프로젝트 총괄 `내부 승인 가능`
- 사용자가 오프닝·기원 문서와 제작 태스크를 검토할 수 있음
