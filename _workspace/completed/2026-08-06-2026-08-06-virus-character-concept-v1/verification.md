# 검증 기록

## 작업 ID

`2026-08-06-virus-character-concept-v1`

## 검증 대상

- 바이러스 의인화 콘셉트 A/B/C와 생성 추적 기록

## 검증 담당

- 비주얼/테크아트 검토: 완료 — A/B/C 원본 직접 확인, 비교용 제시 가능
- 독립 QA: correction 1 완료 — 사용자 원본·canonical reference·기준 문서·색인·금지 범위·비반입을 독립 정적 대조
- 프로젝트 총괄: correction 1 `내부 승인 가능`

## 실제 수행·검증 이력

| 역할/에이전트 | 실제 수행 | 산출물·판정 |
| --- | --- | --- |
| 조정자 | R2 S0·범위·비용 예산 작성 | 생성 시작 가능 |
| ChatGPT 이미지 아트 | 승인된 공통 브리프로 A/B/C 각 1회 생성, 원본·프롬프트·해시 기록 | PNG 3개와 `generation-log.md`, 추가 생성 없음 |
| 비주얼/테크아트 | A/B/C PNG를 원본 해상도로 직접 대조 | 세 후보 모두 비교용 제시 가능, 추천 `B > A > C` |
| QA/검증 | A/B/C를 `view_image(original)`로 전수 확인하고 bytes·1254×1254·SHA-256, 기본 원본 3개, 금지 요소와 Unity 비반입을 독립 대조 | 비교용 콘셉트 묶음 기술 검증 통과 — 사용자 수용 대기 |
| 문서/릴리즈 | 사용자 원본을 canonical reference로 비파괴 복사하고 캐릭터 기준·색인·상위 방향·상태 문서를 갱신 | correction 1 문서 후보 제출, 새 이미지 생성·Unity 변경 없음 |
| QA/검증 | correction 1 원본·복사본 무결성, 링크, 디자인 계약, 제3자 명칭, A/B/C 무효화, 비반입과 dirty 경계를 독립 대조 | `qa-selected-bacteriophage-static-20260806T124010Z` PASS, 총괄 감사 가능 |

## 입력 자료

- `task.md`
- 프로젝트 2D 도트 reference와 제작 가이드

## 원래 증상 또는 완료 주장

- 사용자 제공 박테리오파지 이미지와 프롬프트를 기본 바이러스 reference·디자인 계약으로 반영하고, 원본 무결성·외형 불변식·3D reference/2D production·후속 승인 경계를 검증해야 한다.

## 현재 검증 revision

- 위험 등급: R2
- verification revision: `selected-bacteriophage-reference-v1-correction-1`
- candidate fingerprint: `4EE57D1B2600440528E704A91A8A4CF187B13375F0613C399F971B08D06AE5D1`
  - 산출 범위: 사용자 원본·canonical PNG·`user-selected-reference.md`와 production/design 문서 7개. 상태-only board/dashboard는 후속 동기화가 이 QA 증거를 무효화하지 않도록 별도 대조한다.
  - 이전 revision fingerprint `9646485ADB7B2F77185A628303CD533AE2AB1976577CB6E804B1372C994ECADF`는 A/B/C 탐색 이력으로 `SUPERSEDED`
- canonical run_id: `qa-selected-bacteriophage-static-20260806T124010Z`
  - 이전 run `qa-virus-concept-static-20260806T080758Z`는 A/B/C 비교 revision의 유효한 과거 QA 이력
- candidate frozen 여부: 예 — correction 1 production/design/reference 후보를 위 fingerprint로 고정
- Unity/MCP/build route: 사용하지 않음
- attempt ledger 연속 실패 / reclassification ID: 0 / 해당 없음

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 예 — 이미지 생성 담당과 비주얼/테크아트 검토 담당 분리
- 구현 주체가 실행한 검증과 별도로 확인한 항목: correction 1 QA가 사용자 원본과 canonical 복사본, 링크, 외형·성격·업그레이드 계약, 3D reference/2D production 경계, 금지 명칭, A/B/C `SUPERSEDED`, imagegen·UnityProject·MCP·build 0과 unrelated dirty 비침범을 독립 대조했다.

## 실행한 검증

| criterion ID | 유형 | 검증 방법 | run_id | 결과 | canonical 증거 | 유효/SUPERSEDED |
| --- | --- | --- | --- | --- | --- | --- |
| C1 | 성공 | A/B/C 원본 `view_image(original)` 전수 비교 | `qa-virus-concept-static-20260806T080758Z` | PASS | A/B/C PNG 3개 | `SUPERSEDED` |
| C2 | 성공 | 원본 확대 시 픽셀 군집·팔레트와 실제 게임 규격 경계 대조 | `qa-virus-concept-static-20260806T080758Z` | PARTIAL | A/B/C PNG 3개 | `SUPERSEDED` |
| C3 | 경계 | 공통 몸체·돌기·핵·팔레트와 후보 간 변형 폭 비교 | `qa-virus-concept-static-20260806T080758Z` | PARTIAL | A/B/C PNG 3개 | `SUPERSEDED` |
| C4 | 성공 | 약한 시작형·기묘함·이동·생존·변이 가능성 비교 | `qa-virus-concept-static-20260806T080758Z` | PARTIAL | A/B/C PNG 3개 | `SUPERSEDED` |
| C5 | negative control | 글자·로고·HUD·워터마크·숙주·후반 콘텐츠 전수 검사 | `qa-virus-concept-static-20260806T080758Z` | PASS | A/B/C PNG 3개 | `SUPERSEDED` |
| C6 | 수명주기 | 로그의 도구·날짜·reference·프롬프트·경로·bytes·해상도·SHA-256을 실제 파일과 재대조 | `qa-virus-concept-static-20260806T080758Z` | PASS | `artifacts/generation-log.md`와 A/B/C PNG | `SUPERSEDED` |
| C7 | 실패 경계 | 후보·최종 게임 에셋·Unity 적용 승인 경계 대조 | `qa-virus-concept-static-20260806T080758Z` | PASS | `task.md`, `artifacts/generation-log.md` | `SUPERSEDED` |
| COR1 | 성공 | 사용자 원본·canonical 복사본 bytes·해상도·SHA-256 대조 | `qa-selected-bacteriophage-static-20260806T124010Z` | PASS | 두 PNG | 유효 |
| COR2 | 성공 | canonical reference·기준 문서·색인 경로 존재 대조 | `qa-selected-bacteriophage-static-20260806T124010Z` | PASS | 디자인·reference 색인 | 유효 |
| COR3 | 성공 | 외형·성격·업그레이드 실루엣·허용 추가요소 계약 대조 | `qa-selected-bacteriophage-static-20260806T124010Z` | PASS | `base-bacteriophage-character.md` | 유효 |
| COR4 | 경계 | 3D voxel reference/2D production, `150nm`·이미지 텍스트·실제 시트·Unity 승인 경계 대조 | `qa-selected-bacteriophage-static-20260806T124010Z` | PASS | 기준·reference·상위 방향 문서 | 유효 |
| COR5 | negative control | raw 사용자 artifact 밖 production/design 문서의 제3자 스타일 명칭 검색 | `qa-selected-bacteriophage-static-20260806T124010Z` | PASS — 0건 | `docs/` 정적 검색 | 유효 |
| COR6 | 수명주기 | A/B/C `SUPERSEDED`와 현재 canonical 우선순위 대조 | `qa-selected-bacteriophage-static-20260806T124010Z` | PASS | task·verification·기준·색인·상태판 | 유효 |
| COR7 | negative control | 새 imagegen·UnityProject 반입·MCP·build 실행 대조 | `qa-selected-bacteriophage-static-20260806T124010Z` | PASS — `0/0/0/0` | 파일·작업 기록·상태판 | 유효 |
| COR8 | 경계 | correction 대상 diff와 기존 unrelated Unity dirty 경계 대조 | `qa-selected-bacteriophage-static-20260806T124010Z` | PASS | Git 상태·mtime·대상 diff | 유효 |

## 비주얼/테크아트 검토

- 검토 대상: A/B/C PNG 원본과 `artifacts/generation-log.md`
- 검토 방식: 세 PNG를 원본 해상도로 직접 확인하고 C1~C7, 공통 바이러스 정체성, 의인화 강도, 약한 시작형, 이동·생존·변이 확장성, 실제 플레이 축소 실루엣과 일반 슬라임·몬스터·상업 마스코트 위험을 대조했다.
- 비교 제시 판정: A/B/C 모두 의인화 수준 선택용으로 사용자에게 제시할 수 있으며 즉시 제외할 후보는 없다. 다만 세 후보는 동일 계열로는 읽히지만 동일 개체의 엄밀한 통제 변형은 아니다.
- 추천 순위: `B > A > C`
- C 후보 경고: C는 최대 의인화의 상한선 비교안으로 제시한다. 큰 유광 눈, 굵은 팔다리, 신발처럼 읽히는 발과 결연한 자세 때문에 상업 마스코트 또는 일반 귀여운 몬스터로 기울 위험이 가장 높아 무수정 채택은 권장하지 않는다.

| criterion ID | 판정 | 비주얼/테크아트 근거 |
| --- | --- | --- |
| C1 | PASS | A 최소 의인화, B 반의인화, C 마스코트형의 얼굴·팔다리·행동성 단계가 구분된다. |
| C2 | PARTIAL | 세 후보 모두 제한 팔레트와 픽셀 군집의 2D 도트풍으로 읽히지만, 고해상도 콘셉트이므로 실제 논리 픽셀 격자와 플레이 크기 축소 가독성은 검증되지 않았다. |
| C3 | PARTIAL | 구형 외피·둥근 돌기·청록 핵·팔레트는 공통이나 핵 위치·비율, 돌기 수·배치, 체형과 팔다리 구분이 달라 같은 바이러스의 엄밀한 일관성은 부족하다. |
| C4 | PARTIAL | A는 약함과 기묘함, B는 이동·생존 행동성과 변이 확장성의 균형이 좋다. C는 행동성은 선명하지만 약한 시작형보다 결연한 마스코트로 기운다. |
| C5 | PASS | 글자·로고·HUD·워터마크·숙주·후반 콘텐츠 등 금지 요소가 보이지 않는다. |
| C6 | PASS | 생성 로그에 도구·날짜·reference·전체 프롬프트·출력 경로·크기·해시가 기록되어 있다. |
| C7 | PASS | 세 PNG는 사용자 비교용 콘셉트이며 최종 스프라이트·애니메이션·Unity 적용본이 아니다. |

### 후보별 유지·수정 기준

- A 유지: 작은 눈, 무표정에 가까운 얼굴, 비인간형 구체 몸체, 취약하고 기묘한 인상, 발광 핵과 돌기 중심 실루엣.
- A 수정: 팔다리 대신 막 수축·기울기 같은 이동 단서를 보강하고, 축소 시 눈·핵·돌기가 뭉개지지 않도록 정보량과 명도 단계를 단순화한다.
- B 유지: 바이러스 몸체가 지배적인 반의인화 수준, 조심스러운 생존 행동, 짧은 팔다리와 절제된 표정, 향후 변이를 붙일 수 있는 여백.
- B 수정: 팔·다리를 더 짧고 막성으로 만들고 손가락·발가락 인상을 제거한다. 공통 8개 돌기, 핵 위치·비율과 기준 체형을 일관성 시트에서 고정한다.
- C 유지: 최대 의인화 상한선을 보여 주는 선명한 표정·보행 가독성만 비교 기준으로 유지한다.
- C 수정: 눈 크기와 광택, 입과 결연한 표정을 낮추고 미튼형 손·신발형 발을 제거한다. 몸체와 8개 돌기가 먼저 읽히도록 바이러스 실루엣을 복원한다.
- 공통 후속 기준: 사용자 선택 뒤 동일 체형·핵·8개 돌기·팔레트의 일관성 시트를 만들고, 실제 게임 크기 실루엣과 방향·피벗을 게임 규격으로 다시 검증한다. 현재 후보는 최종 게임 에셋이 아니다.

## 독립 QA 판정

- 원본·무결성: A/B/C 모두 `1254×1254`이며 로그의 bytes와 SHA-256이 실제 파일과 일치한다.
  - A: `1,049,196 bytes`, `749D0A237968E0254B2BF8F4065C5D5AFD13E62DC706C106C9AD8F94FCE0DBF0`
  - B: `1,065,899 bytes`, `EDBDA8DB3E0335D2AC3EB85B3051FF1885C882C0DED934341AC00EA5063B14CE`
  - C: `1,087,163 bytes`, `15D1B4187F12C64D582264742E79FA540E57E199BC183CB3EEAB8C729503A2AE`
- 생성 수: 작업 패킷 PNG는 정확히 3개이며, 로그가 가리키는 기본 생성 원본 폴더도 해시가 같은 PNG 정확히 3개다. 기록된 생성 수와 일치한다.
- C1·C5·C6·C7: PASS. 세 의인화 단계가 구분되고, 금지 요소는 없으며, 추적성과 최종 에셋 경계가 유지된다.
- C2 PARTIAL 비차단 근거: 세 후보는 제한 팔레트의 2D 도트풍 방향 비교에는 충분하다. 실제 논리 픽셀 격자와 게임 크기 축소 가독성은 선택 후 게임 규격 재제작 단계의 위험이므로 현재 사용자 방향 선별을 차단하지 않는다.
- C3 PARTIAL 비차단 근거: 공통 구형 외피·둥근 돌기·청록 핵·팔레트로 동일 계열은 유지된다. 핵 위치·비율, 돌기 수·배치와 체형이 달라 동일 개체의 엄밀한 통제 변형은 아니지만 의인화 강도 비교는 가능하다. 이 한계를 사용자에게 공개해야 한다.
- C4 PARTIAL 비차단 근거: A는 약함·기묘함, B는 이동·생존 행동성과 변이 여백의 균형이 읽힌다. C는 약한 시작형보다 결연한 마스코트로 기울지만 최대 의인화 상한선으로 명시하면 비교 목적을 충족한다.
- 추천: `B > A > C`. B를 기준 방향 우선안으로 추천하고, C는 상업 마스코트·일반 귀여운 몬스터 위험을 가진 의인화 상한선으로만 제시한다.
- 금지 범위: 글자·로고·HUD·워터마크·숙주·쥐·인간·백혈구·병원·연구소·백신·엔딩·제3자 캐릭터는 0건이다.
- UnityProject 반입: 후보 이름, 동일 bytes·SHA-256, 생성 시각 이후 이미지 파일을 대조한 결과 0건이다.
- Unity/MCP/build: 이 정적 이미지 route에서 실행하지 않았으며 실제값은 `0/0/0`이다.
- 사용자 선별 가능 경계: 현재 A/B/C 중 의인화 강도와 기준 외형 방향만 선택할 수 있다. 턴어라운드·표정·변이 시트, 실제 게임 규격 스프라이트 재제작, Unity Import·적용은 별도 승인과 후속 QA 전에는 선택·완료 범위가 아니다.
- QA 완료 판단: `기술 검증 통과 — 사용자 수용 대기`. C2~C4 PARTIAL은 비교 제시를 차단하지 않지만 `동일 개체의 엄밀한 통제 변형`, `최종 게임 에셋 준비 완료`, `C1~C7 무조건 완전 충족` 주장은 차단한다.

## correction 1 독립 QA 판정

- 원본·canonical 무결성: 두 PNG 모두 `1036×1248`, `1,882,663 bytes`, SHA-256 `0C1D22C07C0CAC8B2F70D7BEFCFB5FA5E6ECB66D0F183125DFF359D33CEA039F`로 일치한다.
- 링크: canonical reference, 사용자 원본, 캐릭터 기준, visual/reference/design 색인과 작업 artifact의 예상 경로가 모두 존재하며 누락은 0건이다.
- 디자인 계약: 큰 정이십면체 캡시드, `60~70%` 머리 비율, `2~3마디` 짧은 몸통, 팔 2개·다리 2개·작은 꼬리, 짧고 둥근 spike, 보라·분홍·흰색 팔레트, 호기심·용감함·친근함·영리함을 일관되게 기록한다.
- 업그레이드 경계: 기본 실루엣을 유지하며 작은 방어구, 발광 코어, 제한적인 결정 성장, 에너지 효과, 캡시드 장식, 진화 spike만 허용 후보로 둔다. 실제 능력·수치·해금 규칙은 확정하지 않는다.
- reference/production 경계: `3D voxel`·재질·조명은 외형 reference이며 production은 방향별 `2D 아이소메트릭/쿼터뷰 도트 스프라이트`다. 이미지의 `150nm`, 행동·표정·업그레이드 텍스트는 별도 기획 승인 없이 자동 확정하지 않는다.
- 후속 승인: 실제 턴어라운드·표정·행동·변이 시트, 애니메이션, 게임 규격 스프라이트와 Unity Import·적용은 별도 사용자 승인과 QA 대상이다.
- 제3자 모사 금지: 사용자 원문 보존 artifact를 제외한 `docs/`에서 `Nintendo`, `Pokémon`/`Pokemon`, `Kirby`는 0건이다. 저장소 전체의 기존 `UnityProject/ProjectSettings/QualitySettings.asset`에 있는 `Nintendo Switch`는 플랫폼 키이며 스타일 모사 지시가 아니다.
- 이력: 기존 A/B/C와 이전 fingerprint/run은 탐색 이력으로 보존하되 현재 기준에서 모두 `SUPERSEDED`다.
- 비반입·비실행: correction 1 새 imagegen 0, UnityProject 반입 0, Unity/MCP/build `0/0/0`이다.
- unrelated dirty: correction 이전인 2026-08-05 Unity dirty 파일을 포함한 기존 변경을 보존했고, correction-specific 변경은 지정 reference·design·상태 문서에 한정된다.
- 정적 형식: 대상 tracked 문서의 `git diff --check`는 exit 0이며 CRLF 변환 안내만 있었다.
- 사용자 보고 경계: correction 1 QA는 PASS이며 총괄 감사로 넘길 수 있다. 총괄 PASS와 상태-only 동기화 뒤 사용자에게 reference·문서 반영 결과를 보고할 수 있으나 실제 시트·애니메이션·Unity 적용 완료로 표현할 수 없다.

## 검증하지 못한 항목

- 실제 논리 픽셀 격자·게임 플레이 크기 축소 가독성, 방향별 프레임·공통 피벗과 Unity 출력은 현재 reference·문서 반영 범위 밖이므로 미검증이다.

## 실패 또는 경고

- C3은 부분 충족이다. 세 후보가 동일 계열로는 읽히지만 동일 개체의 엄밀한 통제 변형은 아니다.
- C는 상업 마스코트·일반 귀여운 몬스터 위험이 높으므로 최대 의인화 상한선 경고 없이 기준형으로 채택하지 않는다.

## fail-fast·무효화

- first blocker: 없음 — correction 1 원본 무결성·문서 계약·금지 범위·비반입 정적 QA PASS
- correction cycle: 1/2 — 사용자가 외부 reference와 프롬프트를 기본 박테리오파지 기준으로 선택
- 변경 뒤 무효화한 run/증거와 사유: A/B/C 비교 결과와 QA run은 탐색 이력으로 보존하되 현재 선택 기준에서는 `SUPERSEDED`
- correction 1 기록 정합 blocker: 총괄이 task 정식 S0 미동기화를 지적해 COR1~COR8을 추가했고, QA가 최초 재대조에서 COR3~COR7 ID 의미 불일치를 발견했다. verification의 이미 검증된 의미와 task criterion을 1:1로 최소 동기화했으며 production/design/reference 변경은 없다.

## 비용 실행 대조

| 비용 항목 | 계획 예산 | 실제 수·run_id/근거 | 정상/초과/미집계 | 필요한 비용/회피 가능 비용 |
| --- | --- | --- | --- | --- |
| 실제 역할·인계 | 기존 5역할 + correction 문서/릴리즈1 → 독립 QA1 → 총괄1 | 기존 조정1 + 이미지 아트1 + 비주얼1 + QA1 + 총괄1, correction 문서/릴리즈1 + 독립 QA1, 총괄 대기 | 진행 중 | correction 필수 owner·QA 완료, 총괄1 대기 |
| 표적 검증 | 기존 2묶음 + correction 2묶음 | 기존 비주얼·QA 2묶음 + 원본/복사본 무결성1 + 문서 링크·계약·금지·2D 경계1 / `qa-selected-bacteriophage-static-20260806T124010Z` | 정상 | 계획된 correction 표적 검증 완료 |
| Unity/MCP/빌드 시작 | 0/0/0 | 0/0/0 | 정상 | 전부 회피 |
| full suite | 0 | 0 | 정상 | 해당 없음 |
| matrix/capture·artifact | matrix 0, capture 0, 기존 PNG 3 + log 1, canonical reference 1 + 기준·색인 | matrix 0, capture 0, 기존 PNG 3 + log 1, canonical reference 1 + 기준·색인 | 정상 | 별도 matrix·capture 없음 |
| correction·무효/폐기 | 최대 2 | 1 — 사용자 외부 reference 선택, A/B/C 탐색 이력 `SUPERSEDED`, correction QA PASS | 주의 | 새 이미지 생성 없이 문서 반영·정적 QA만 수행 |

- 비용 판정: 주의 — 사용자 선택에 따른 정당한 correction 1회. 문서 owner1·독립 QA1이 추가됐고 새 이미지·Unity/MCP/build/full/matrix/capture는 0
- `docs/project-handoff/task-cost-dashboard.md` 갱신·독립 대조 여부: correction 1 QA PASS·총괄 대기로 상태-only 동기화

## 게이트 판정

- QA/검증 게이트 통과 여부: correction 1 문서 revision 독립 정적 QA PASS
- 총괄 관리자 검토로 넘길 수 있는지: 예 — 현재 fingerprint와 canonical run을 총괄 감사 대상으로 전달 가능

## 프로젝트 총괄 관리자 판정

- 판정: 내부 승인 가능 — correction 1 COR1~COR8과 canonical QA가 현재 사용자 선택 기준을 지지함
- 근거: task·verification criterion→evidence가 1:1로 정렬됐고 revision `selected-bacteriophage-reference-v1-correction-1`, fingerprint `4EE57D1B2600440528E704A91A8A4CF187B13375F0613C399F971B08D06AE5D1`, run `qa-selected-bacteriophage-static-20260806T124010Z`를 QA와 총괄이 재현했다.
- 승인 범위·사용자 수용 대기: 기본 외형 선택은 완료. 실제 2D 턴어라운드·표정·변이 시트·게임 규격 재제작·Unity Import·적용은 별도 승인 대상이다.

## 완료 판단

- correction 1 기술 검증 통과 — 프로젝트 총괄 감사·사용자 보고 대기

## 사용자 수용 상태

- 사용자 직접 확인 필요: 문서 반영 결과와 2D 재해석 경계 확인
- 확인 전 `완료` 표현 금지 여부: 예

## 완료 판단 근거

- 사용자 제공 reference와 프롬프트의 기준 문서 반영, correction 1 독립 QA PASS와 프로젝트 총괄 `내부 승인 가능` 판정을 완료했다.

## 최종 상태

- 완료/보류/승인 대기: 기본 박테리오파지 기준 반영 완료 — 후속 2D 시트·Unity 적용은 별도 승인 대기
- 완료 경로와 Git 상태: completed 이동·커밋 준비
