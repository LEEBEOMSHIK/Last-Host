# 오프닝 시네마틱 제작 계획

> **For agentic workers:** REQUIRED SUB-SKILL: Use `superpowers:subagent-driven-development` (recommended) or `superpowers:executing-plans` to execute this plan task-by-task. Steps use checkbox syntax for tracking.

**Goal:** 승인된 오프닝·기원 설계를 픽셀아트 모션 코믹형 시네마틱, 커스터마이징과 첫 튜토리얼로 안전하게 발전시킨다.

**Architecture:** 사건·기원 계약, 문구·길이·튜토리얼 선택, 숏·스토리보드 명세, 이미지 후보, 애니매틱·오디오, Unity 통합을 서로 독립된 승인 게이트로 나눈다. 앞 단계의 승인은 다음 단계의 제작 승인을 자동으로 포함하지 않는다.

**Tech Stack:** Markdown 기획 문서, 2D 아이소메트릭 도트, 픽셀아트 모션 코믹형 시네마틱, 향후 별도 승인 시 Unity `6000.4.6f1`·URP.

## Global Constraints

- 현재 실행 승인 범위는 문서 설계와 태스크 정리까지다.
- 주인공은 박테리오파지이며 인간 세포를 직접 감염하지 않는다.
- 청소 노동자는 감염의 원인이나 환자 0이 아니다.
- 생화학 무기의 구체적인 물질·유전자·배양·제조·전달 절차를 작성하거나 시각화하지 않는다.
- 생성 이미지는 후보이며 사용자 선택과 픽셀 적합성 검토 전에는 최종 에셋이 아니다.
- 쥐 숙주 프로토타입과 전체 캠페인 구현 범위를 자동 확대하지 않는다.
- 기존 미커밋 메인 시나리오·시네마틱 가이드 파일은 별도 동기화 승인 전 수정하지 않는다.

---

## 파일 구조

| 파일 | 책임 |
| --- | --- |
| `docs/design/narrative/opening/opening-cinematic-origin.md` | 오프닝 사건, 시작 문구 후보, 기원 미스터리와 생물학·표현 경계 |
| `docs/design/narrative/opening/opening-cinematic-production-plan.md` | 승인 게이트별 제작 태스크와 역할 인계 |
| `docs/design/narrative/opening/README.md` | 오프닝 하위 문서 색인과 효력 경계 |
| `docs/design/narrative/main-scenario-outline.md` | 후속 동기화 시 오프닝 요약과 전체 캠페인 위치 반영 |
| 향후 `docs/design/narrative/opening/opening-shot-spec.md` | 사용자 승인 뒤 최종 숏 후보·레이어·타이밍 명세 |
| 향후 `docs/design/narrative/opening/opening-reveal-ledger.md` | 구간별 기원 공개·비공개 정보와 단서 추적 |

## Task 1: 오프닝 사건·기원 계약 고정

**Files:**

- Create: `docs/design/narrative/opening/README.md`
- Create: `docs/design/narrative/opening/opening-cinematic-origin.md`
- Create: `docs/design/narrative/opening/opening-cinematic-production-plan.md`
- Test: `_workspace/active/2026-08-08-opening-cinematic-origin/verification.md`

**Interfaces:**

- Consumes: 사용자 승인 대화, 기존 메인 시나리오, 시네마틱 가이드, 숙주·맵 전이 설계, 기본 박테리오파지 외형
- Produces: 후속 문구·숏·스토리보드 작업의 단일 서사 입력

- [x] **Step 1:** 일상→확산→중앙 문구→기침→비말·배수→각성→커스터마이징→튜토리얼 순서를 문서화한다.
- [x] **Step 2:** 청소 노동자 비난 금지와 박테리오파지의 세균 숙주 경계를 명시한다.
- [x] **Step 3:** 방어·치료 연구의 군사화, 주인공 돌연변이와 점진 공개 단계를 기록한다.
- [x] **Step 4:** 현실 기반·게임적 확대·초고도 변이 능력을 분리한다.
- [x] **Step 5:** 독립 QA C1~C6와 프로젝트 총괄 판정을 기록한다.
- [ ] **Step 6:** 사용자에게 세 문서를 제시하고 내용 수용 또는 수정 요청을 받는다.

## Task 2: 시작 문구·길이·내레이션 확정

**Files:**

- Modify: `docs/design/narrative/opening/opening-cinematic-origin.md`
- Create after approval: `docs/design/narrative/opening/opening-shot-spec.md`

**Interfaces:**

- Consumes: Task 1의 시작·반전·마지막 질문 후보와 길이 A/B/C
- Produces: 숏 상세화에 사용할 확정 총길이, 문자열과 화자

- [ ] **Step 1:** `25~35초`, `40~55초`, `60~75초`의 정보량과 첫 조작 도달 시간을 비교한다.
- [ ] **Step 2:** 시작 문구, 후반 반전 문구와 마지막 질문의 한국어 최종안을 사용자가 선택한다.
- [ ] **Step 3:** 무내레이션, 최소 내레이션, 부분 내레이션 중 한 방식을 선택한다.
- [ ] **Step 4:** 한국어 기준 줄 수와 최소 읽기 시간을 고정하고 다른 언어는 의미 우선 재번역 대상으로 둔다.
- [ ] **Step 5:** 확정 결과를 `opening-shot-spec.md`의 입력 계약으로 기록한다.

## Task 3: 노동자 공간·커스터마이징·튜토리얼 인계 확정

**Files:**

- Modify: `docs/design/narrative/opening/opening-cinematic-origin.md`
- Create after approval: `docs/design/narrative/opening/opening-reveal-ledger.md`

**Interfaces:**

- Consumes: Task 1의 시퀀스 C·D와 튜토리얼 안 A/B
- Produces: 숏 배경, 커스터마이징 범위, 첫 플레이 시작 상태

- [ ] **Step 1:** 청소 노동자의 작업 공간을 공공시설·의료시설 외곽·교통시설 중 하나로 선택한다.
- [ ] **Step 2:** 노동자가 원인으로 읽히지 않는 선행 확산 장면과 보호 부족 소품을 고정한다.
- [ ] **Step 3:** 색·표정·spike·캡시드 무늬·이름 중 실제 커스터마이징 항목을 선택한다.
- [ ] **Step 4:** 커스터마이징이 초기 능력치와 엔딩 성향을 바꾸지 않는지 확인한다.
- [ ] **Step 5:** 미세환경 짧은 조작과 곤충 운반체 즉시 전환 중 첫 튜토리얼 인계를 선택한다.
- [ ] **Step 6:** 새 미세 이동 모드나 벌레 튜토리얼이 선택되면 구현 전 별도 범위 승인을 받는다.

## Task 4: 기원 공개 장부 작성

**Files:**

- Create after approval: `docs/design/narrative/opening/opening-reveal-ledger.md`
- Modify after concurrent task resolution: `docs/design/narrative/main-scenario-outline.md`

**Interfaces:**

- Consumes: 오프닝·초반·중반·병원·연구소·마지막 선택의 공개 단계
- Produces: 각 캠페인 구간이 사용할 새 사실, 단서와 금지 스포일러

- [ ] **Step 1:** 각 구간의 `새로 아는 사실`, `공개 수단`, `아직 감출 정보`를 한 행씩 기록한다.
- [ ] **Step 2:** 군사화 책임을 한 개인·직업·국가에 단순 귀속하지 않는지 확인한다.
- [ ] **Step 3:** 치료 목적을 지키거나 군사화에 반대한 인물의 기록 위치를 지정한다.
- [ ] **Step 4:** 무기 제작에 활용 가능한 구체 정보가 포함되지 않았는지 검사한다.
- [ ] **Step 5:** 기존 메인 시나리오 작업이 커밋·동결된 뒤 오프닝 요약과 공개 장부 링크를 동기화한다.

## Task 5: 최종 숏·스토리보드 명세

**Files:**

- Create after Gate S approval: `docs/design/narrative/opening/opening-shot-spec.md`

**Interfaces:**

- Consumes: Task 2~4의 사용자 선택 결과
- Produces: 이미지 후보와 애니매틱 작업이 사용할 숏 ID, 구도, 레이어, 타이밍, 자막·오디오 큐

- [ ] **Step 1:** 사건 비트를 실제 숏 후보로 나누고 각 숏의 시작·종료 상태와 서사 목적 하나를 기록한다.
- [ ] **Step 2:** 배경·중경·전경·인물·파지·효과·마스크·자막 안전 영역을 숏별로 나눈다.
- [ ] **Step 3:** 카메라 패닝·줌·패럴랙스·제한 프레임과 전환 시간을 지정한다.
- [ ] **Step 4:** 노동자 원인 오인, 파지의 인간 세포 감염 오인과 기원 조기 폭로를 negative control로 점검한다.
- [ ] **Step 5:** 사용자에게 숏 후보 승인을 받고 이미지 생성 여부를 별도로 묻는다.

## Task 6: 스토리보드·이미지 후보

**Files:**

- Create after Gate A approval: 작업별 이미지 브리프와 생성 기록
- Output after Gate A approval: 사용자 지정 후보 경로

**Interfaces:**

- Consumes: 사용자 승인 `opening-shot-spec.md`, 기본 박테리오파지 reference, 2D 아이소메트릭 도트 기준
- Produces: 사용자 선택용 구도·레이어 후보와 생성 이력

- [ ] **Step 1:** 생성할 숏, 후보 수, 비율, 출력 경로와 금지 요소를 사용자에게 승인받는다.
- [ ] **Step 2:** ChatGPT 이미지 아트 담당이 숏 또는 레이어별 후보 2~3개를 생성한다.
- [ ] **Step 3:** 프롬프트·날짜·입력 reference·출력 경로를 기록한다.
- [ ] **Step 4:** 사용자가 후보를 선택하고 비주얼/테크아트가 픽셀·팔레트·실루엣 적합성을 검토한다.
- [ ] **Step 5:** 최종 게임 에셋이 아닌 후보 상태로 유지한다.

## Task 7: 비최종 애니매틱과 오디오 큐

**Files:**

- Create after Gate M approval: 비최종 애니매틱 계획과 결과 경로
- Create after Gate AU approval: 임시 오디오 큐시트

**Interfaces:**

- Consumes: 선택 스토리보드, 레이어 명세, 숏 길이와 문자열
- Produces: Unity 구현 전 리듬·가독성·전환 검토 결과

- [ ] **Step 1:** 타이밍·카메라·자막만 포함한 비최종 애니매틱 제작 승인을 받는다.
- [ ] **Step 2:** 도시 일상, 확산, 암전 문구, 기침, 미시 세계와 각성의 리듬을 연결한다.
- [ ] **Step 3:** 환경음·기침 뒤 무음·각성 모티프의 임시 큐를 별도 승인 범위에서 배치한다.
- [ ] **Step 4:** 첫 조작까지 대기 시간, 자막 읽기, 노동자 원인 오인과 비말 운반 가독성을 사용자와 검토한다.
- [ ] **Step 5:** 실제 영상·음원·성우 결과로 승인하지 않고 프리비즈 상태를 유지한다.

## Task 8: Unity 재생·커스터마이징·튜토리얼 연결

**Files:**

- Modify only after Gate U approval: Unity 씬·Timeline·UI·저장·로컬라이제이션 관련 파일
- Test after Gate U approval: EditMode·PlayMode·MCP 플레이 수용 시나리오

**Interfaces:**

- Consumes: 승인 애니매틱·오디오·문자열, 커스터마이징과 첫 튜토리얼 시작 상태
- Produces: `새 게임`부터 첫 조작까지 재생 가능한 Unity 흐름

- [ ] **Step 1:** Unity 변경 범위, production owner, 저장 형식과 테스트 계약을 별도 R2/R3 작업으로 승인받는다.
- [ ] **Step 2:** 첫 재생, 한 번 본 컷신 건너뛰기·재시청, 언어별 자막·폰트 fallback을 테스트 우선으로 설계한다.
- [ ] **Step 3:** 커스터마이징 확정·취소·저장 실패와 시네마틱 중단·복귀 상태를 구현한다.
- [ ] **Step 4:** 승인된 미세환경 또는 곤충 튜토리얼 시작 상태로 연결한다.
- [ ] **Step 5:** 접근성의 점멸·흔들림·음량 옵션과 Console·dirty 상태를 검증한다.
- [ ] **Step 6:** 독립 QA, 총괄 판정과 사용자 실제 화면 수용 전 완료로 선언하지 않는다.

## 단계별 중단 조건

다음 중 하나가 생기면 다음 제작 단계로 넘어가지 않는다.

- 청소 노동자가 발병 원인 또는 환자 0처럼 읽힘
- 파지가 인간 세포를 직접 감염한다고 표현됨
- 기원이 오프닝에서 전부 폭로되어 후반 미스터리가 사라짐
- 인간 전체나 연구자 전체가 단순 악역으로 묘사됨
- 무기 제작에 활용 가능한 구체적인 절차·물질·유전자·배양 조건이 포함됨
- 생성 후보가 기본 박테리오파지 실루엣을 잃음
- 문서 승인을 이미지·애니매틱·Unity 또는 전체 캠페인 구현 승인으로 확대함
- 쥐 숙주 프로토타입의 단독 시작 흐름을 승인 없이 변경함

## 다음 사용자 승인 순서

1. 균형형·압축형·드라마형 중 오프닝 길이 방향
2. 시작·반전 문구
3. 청소 노동자의 작업 공간과 묘사
4. 미세 이동 또는 곤충 숙주 튜토리얼
5. 커스터마이징 항목
6. 숏·스토리보드 상세 제작
7. 이미지 후보 생성 수량
8. 비최종 애니매틱 제작
9. 오디오 제작
10. Unity 재생과 튜토리얼 연결

각 항목은 이전 항목의 승인이 다음 항목 전체를 자동 승인하지 않는다.
