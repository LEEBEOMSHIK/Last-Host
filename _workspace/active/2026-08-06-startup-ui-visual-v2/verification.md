# 검증 기록

## 2026-08-07 사용자 최종 선택

- 사용자 선택 파일: `docs/design/visual/references/image.png`
- 후속 이름: `docs/design/visual/references/startup-bacteriophage-food-chain-background-v1.png`
- 선택본 무결성: `1672×941`, SHA-256 `5ED62B0BE9E0FC68FED15135C8BEDB3F08639CD020E914EF420FE73831B17C8D`.
- 판정: L/M/N 자동 후보 비교는 사용자 직접 제작·선택본으로 대체되어 `SUPERSEDED`다.
- 승인 범위: 선택본을 Startup 배경으로 import·적용하는 별도 R3 revision 승인. 게임플레이 타일·스프라이트·최종 전체 아트 승격은 아님.
- 실제 Unity 통합과 검증 기록은 `_workspace/active/2026-08-05-startup-settings-localization-ui/`에 이어서 기록한다.

## 현재 상태

- 작업 ID: `2026-08-06-startup-ui-visual-v2`
- 위험 등급: R2
- 상태: revision 3 I~K와 revision 4 L~N 모두 `SUPERSEDED` — 사용자 직접 제작·선택본으로 대체
- candidate revision: `brief-v4-phage-background-integration-correction-1`
- current candidate: 없음. 실제 선택본은 `docs/design/visual/references/startup-bacteriophage-food-chain-background-v1.png`이며 별도 Startup 통합 작업에서 관리한다.
- candidate fingerprint: `A8EC0F3FAD91E8FA2B54D734CD759FD59AF1D7D1EDA07FAC0095A2DFDF357444` (4파일 `relative/path<TAB>SHA-256` 정렬·LF 결합 후 SHA-256)
- canonical QA run_id: `qa-startup-v4-visual-20260807T012331Z`
- Git 보존 경계: A~N PNG 14개·총 `25,960,954 bytes`는 `SUPERSEDED` 로컬 이력으로 유지하고 커밋에서 제외한다. `artifacts/generation-log.md`의 프롬프트·bytes·SHA-256·판정은 추적 기록으로 커밋한다.

## SUPERSEDED A~D 후보 검증표

| 후보 | 파일 | C1 | C2 | C3 | C4 | C5 | 판정 |
| --- | --- | --- | --- | --- | --- | --- | --- |
| A | `artifacts/candidates/startup-bg-candidate-a-crossroads.png` | PASS | PASS | PASS | PASS | PASS | 채택 가능·추천 1위 |
| B | `artifacts/candidates/startup-bg-candidate-b-tunnel.png` | PASS | FAIL | PASS | PASS | PASS | 선별 제외 — 정면 원근 |
| C | `artifacts/candidates/startup-bg-candidate-c-chamber.png` | PASS | PASS | PASS | PASS | PASS | 채택 가능·추천 2위 |
| D | `artifacts/candidates/startup-bg-candidate-d-gate.png` | PASS | FAIL | PASS | FAIL | PASS | 선별 제외 — 정면 원근·`+`형 기호 |

## 1차 비주얼 검토

- 사용자 선별 대상: `A`, `C`.
- A: 프로젝트 대표성과 좌측 세로 UI 여백이 가장 안정적이다.
- C: 중앙 타이틀과 수직 공간·생존 규모감이 강하다.
- B: 정면 터널 원근이 강해 아이소메트릭 계약을 충족하지 않으므로 선별에서 제외한다.
- D: 정면 원근과 `+`형 발광 기호 때문에 C2·C4를 충족하지 않으므로 선별에서 제외한다.
- 명시적 글자·제목·버튼·HUD·로고·워터마크·인간·범위 밖 콘텐츠: A~C에서는 발견되지 않음. D는 `+`형 발광 기호로 제외.
- 해상도: 네 후보 모두 `1672×941` PNG.
- 생성 프롬프트·도구·날짜·reference·SHA-256: `artifacts/generation-log.md`에 기록.

## 독립 QA correction 1

- 최초 QA: FAIL — B/D C2, D C4, 생성 로그 바이트 크기 누락.
- 처리: 이미지 재생성 없이 B/D를 사용자 선별에서 제외하고 네 파일의 실측 바이트 크기를 생성 로그에 추가했다.
- correction cycle: `1/2`.
- correction 1 독립 QA: PASS — C1~C6, 파일 크기·해상도·SHA-256 일치.

## 사용자 수용 피드백과 correction 2

- 사용자 판정: A~D 모두 쥐 중심이어서 게임 전체 시작 화면 정체성과 불일치.
- 처리: A~D는 파일을 삭제하지 않고 전부 `SUPERSEDED` 생성 이력으로 보존한다.
- 새 브리프: 특정 숙주 중심 금지, 바이러스의 생존·변이·숙주 이동을 중심으로 E~H 4안을 생성한다.
- correction cycle: `2/2` — 이후 자동 추가 생성 금지.

## SUPERSEDED correction 2 이미지 전담 검토

- 검토 대상: 후보 `E`~`H` 원본 PNG와 `artifacts/generation-log.md`
- 검토 기준: C2~C6, 특정 숙주 중심 여부, 쥐의 상대 비중, 바이러스의 생존·변이·숙주 이동 가독성, UI negative space, 글자·유사 기호·UI·후반 콘텐츠
- 추천 순위: `G > E`

| 후보 | C2 | C3 | C4 | C5 | C6 | 판정 | 상세 사유 |
| --- | --- | --- | --- | --- | --- | --- | --- |
| E | PASS | PASS | PASS | PASS | PASS | 채택 가능 | 딱정벌레·모기·쥐·새가 우측 환경의 작은 세부로 분산되고 쥐가 대표 주인공으로 보이지 않는다. 분기·변형되는 청록 입자 경로가 숙주 이동과 변이를 전달하며, 하수도는 우측 끝의 작은 흔적으로 제한된다. 왼쪽 약 38%의 어두운 여백이 제목·세로 메뉴에 충분하고 글자·유사 로고·HUD·후반 콘텐츠가 보이지 않는다. |
| F | PASS | PASS | PASS | PASS | FAIL | 선별 제외 | 개별 숙주 비중은 동등하고 순환은 읽히지만, 중앙의 밝은 원형·사방 돌기 기호와 점선 원이 HUD 아이콘·로고·인포그래픽처럼 보인다. 배경 자체에 UI·로고 같은 요소를 두지 않는 C6과 충돌한다. |
| G | PASS | PASS | PASS | PASS | PASS | 채택 가능 | 딱정벌레·모기·쥐·새가 하단 생태 전이에 분산되어 특정 숙주가 중심이 아니다. 숲·습지·배수로·도시 가장자리와 변화하는 청록 입자 경로가 생존·변이·숙주 이동을 가장 직관적으로 전달한다. 중앙 상단 약 35%의 여백이 실제 제목·메뉴에 충분하다. 청록 픽셀 군집은 유기적 경로로 읽히며 독립된 글자·HUD·로고는 아니다. |
| H | PASS | PASS | FAIL | PASS | FAIL | 선별 제외 | 특정 숙주 중심 문제는 없고 미시적 적응·변이 흐름은 보이지만, 사전 설명 없이 여러 숙주 사이의 이동을 읽기 어렵다. 우측의 밝은 원과 꼬리 형태가 `Q` 또는 돋보기형 UI 기호처럼 보여 C6에도 충돌한다. |

- 사용자 선별 대상: `G`, `E`만 제시한다.
- correction `2/2`이므로 추가 생성이나 자동 재생성은 수행하지 않는다.
- E~H는 생성 후보이며 최종 게임 에셋 또는 Unity 적용본이 아니다.

## correction 2 독립 QA

- 검증 revision: `brief-v2-host-journey-correction-2`
- 결과: `PASS`
- C1~C6: 모두 PASS
- 사용자 선별 가능: `E`, `G`
- 선별 제외 타당: `F` — 원형·점선 구성이 HUD·로고·인포그래픽처럼 보임, `H` — 숙주 이동 가독성 부족과 `Q`형 유사 기호
- 파일 무결성: E~H 모두 생성 로그의 파일 크기·`1672×941` 해상도·SHA-256과 일치
- UnityProject 내 E~H 후보 파일: `0개`
- Unity/MCP/build 실행: `0회`
- correction `2/2` 경계에 따라 추가 생성·재생성: `0회`

## correction 2 프로젝트 총괄 판정

- 판정: `내부 승인 가능` — E/G만 사용자 선별에 제시 가능.
- 추천 순위: `G > E`.
  - G: 자연에서 도시 생활권으로 이어지는 환경 변화, 여러 숙주와 변화하는 바이러스 경로가 한 화면에서 읽혀 게임 전체 정체성을 가장 잘 전달한다.
  - E: 숙주 이동 경로와 아이소메트릭 공간은 더 직접적이지만 우측 입자 경로와 환경 밀도가 G보다 복잡하다.
- 제외 확인:
  - F: 원형·점선 구성이 HUD·로고·인포그래픽처럼 보여 C6과 충돌한다.
  - H: 여러 숙주 사이의 이동이 직관적으로 읽히지 않고 `Q`형 UI 유사 기호가 보여 C4·C6과 충돌한다.
- 독립 QA 확인: correction 2 `PASS`, C1~C6 충족, 파일 무결성·추적성 일치.
- 후보 경계: E/G는 사용자 선별용 시작 화면 reference 후보이며 최종 게임 에셋이 아니다.
- 적용 경계: 리샘플링·UI 오버레이·Import·Unity 적용은 사용자 선택 후 별도 승인과 검증이 필요하다.
- Unity/MCP/build 실행: `0회`.
- 사용자 결정 필요: G 또는 E 중 후속 제작 기준 선택.

## 실행 경계

- Unity/MCP/build 실행: 0
- Unity 프로젝트 변경: 없음
- 사용자 선별: E/G 제시 대기
- 최종 에셋·Unity 적용 승인: 대기
- 이전 독립 QA·총괄 판정: A/C 이미지에 한정된 판정으로 사용자 피드백 뒤 `SUPERSEDED`.
- correction 2 독립 QA: PASS.
- correction 2 총괄 판정: `내부 승인 가능` — E/G 사용자 선별 대기, 추천 `G > E`.

## 사용자 승인 revision 3

- 사용자 판정: E/G는 특정 숙주 편중은 해소했지만 먹이사슬과 바이러스가 여기저기 이동하는 감각이 부족하다.
- 사용자 승인: 확정된 기본 박테리오파지를 사용해 시작 화면 이미지 생성을 재개한다.
- 새 완료 기준: I~K 3안에서 승인된 박테리오파지 실루엣, 자연스러운 포식 관계, 숙주 사이를 횡단하는 동작·afterimage, 16:9 UI 여백이 함께 읽힌다.
- 입력 reference: 기본 박테리오파지 canonical reference는 캐릭터 외형, gameplay mockup은 2D 아이소메트릭 도트 밀도·팔레트 참고용이다.
- 경계: 생성 후보만 제작하며 Unity Import·씬/코드 적용과 최종 게임 에셋 승격은 하지 않는다.
- revision correction cycle: `0/2`

## revision 3 구현자 정적·육안 점검

- 파일: I~K 3개 모두 존재, `1672×941` PNG, 생성 로그 bytes·SHA-256 기록 완료.
- I: 왼쪽 UI 여백이 가장 넓고, 과일을 먹는 딱정벌레·모기를 노리는 개구리·쥐를 향하는 올빼미가 단계적으로 보여 먹이사슬 가독성이 가장 직접적이다. 박테리오파지는 중앙 도약과 2개 afterimage로 이동이 명확하다.
- J: 오른쪽 UI 여백이 넓고, 박테리오파지의 긴 S자 횡단과 속도감이 가장 강하다. 다만 afterimage가 의도한 2~3개보다 많아 독립 검토에서 복수 캐릭터처럼 보이는지 확인이 필요하다.
- K: 중앙 상단 UI 여백, 전경 박테리오파지, 애벌레·딱정벌레·개구리/모기·쥐·새의 연쇄와 S자 이동 경로가 균형적이다. 캐릭터성과 전체 게임 여정 전달이 가장 안정적이다.
- 금지 요소 1차 확인: 명시적 글자·버튼·HUD·화살표·원형 도표·로고·인간·병원·연구소·백신·고어 없음.
- 구현자 추천: `K > I > J`. 사용자 선별 전 독립 시각·범위 검토 필요.

## revision 3 독립 시각 QA

- 실행: 읽기 전용 시각·파일 대조. Unity/MCP/build 0.
- 결과: I~K 모두 사용자 선별에 제시 가능. 추천 순위 `I > K > J`.

| 후보 | phage 불변식 | 먹이사슬 | 숙주 전이 이동감 | UI 여백 | 금지 요소 | pixel/style | 추적성 | 판정 |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| I | PASS | PASS | PASS | PASS | PASS | PASS | PASS | 추천 1위 |
| J | PASS | PASS | PARTIAL | PASS | PASS | PASS | PASS | 추천 3위 — afterimage 약 4개로 복수 캐릭터·잔상 과밀 위험 |
| K | PASS | PARTIAL | PASS | PASS | PASS | PASS | PASS | 추천 2위 — 개구리/모기 외 포식 연결이 느슨해 생태 행동 순회로 읽힐 수 있음 |

- I 근거: 2개 afterimage와 중앙 도약이 한 주체의 이동으로 명확하고, 딱정벌레/과일·개구리/모기·올빼미/쥐 관계가 즉시 읽힌다. 왼쪽 여백도 안정적이다.
- J 근거: 다층 포식과 횡단 속도감은 강하지만 잔상 수가 브리프의 2~3개를 초과한다. 투명도와 연속 경로 덕분에 motion echo로는 읽혀 제외 수준은 아니다.
- K 근거: 전경 박테리오파지와 S자 숙주 횡단감, 중앙 상단 여백이 강하다. 다만 단일 연결 먹이사슬보다 여러 생태 행동을 방문하는 여정으로 읽힐 가능성이 있다.
- 공통 금지 요소: 글자·버튼·HUD·화살표·도표·로고·워터마크·인간·병원·연구소·백신·엔딩·고어 없음.
- 파일 무결성: I~K 모두 `1672×941`, 생성 로그 bytes·SHA-256 실측 일치. `UnityProject/` 내 후보 파일 0.
- 공통 C2 주의: 큰 캡시드·짧은 몸통·둥근 spike·팔 2개·다리 2개·흰 장갑은 읽히지만 작은 박테리오파지 꼬리는 실제 화면에서 명확히 판독되지 않는다. 후보 선별을 막지는 않되 최종 선택 후 2D 재제작에서 보정·검증한다.
- 경계: 세 후보는 시작 화면 reference 후보이며 UI overlay·리샘플링·Import·Unity 적용은 별도 승인 대상이다.

## revision 3 정적 재대조

- 결과: PASS
- canonical run_id: `qa-startup-v3-static-20260806T133304Z`
- candidate fingerprint: `C8D8092DBF364C3B7564838E769F9D4779A7BF2CC98A3E87102C1923D471FA87` 재현 일치.
- C1 PASS, C2 PASS(작은 꼬리 가독성 미확인 공개·후속 보정), C3 PARTIAL(K만 연결 먹이사슬 느슨), C4 PARTIAL(J만 afterimage 과밀), C5~C7 PASS.
- C3/C4 PARTIAL은 사용자 선별용 reference 단계에서는 비차단이며 실제 재제작 위험으로 공개한다.
- A~H는 현재 후보·판정 근거에서 제외된 `SUPERSEDED` 이력이고 I~K만 current candidate다.
- UnityProject 내 I/J/K 0, Unity/MCP/build 0, 재대조 중 새 이미지·파일 수정·동적 실행 0.

## revision 3 프로젝트 총괄 최종 판정

- 판정: `내부 승인 가능`
- 근거: revision 3 C1~C7, I~K 생성 3회·재생성 0, A~H `SUPERSEDED`, 4파일 fingerprint와 canonical QA run이 현재 후보에서 일치한다.
- 사용자 제시: I/J/K 3개 모두 가능, 추천 `I > K > J`.
- 공개 위험: K는 연결 먹이사슬 PARTIAL, J는 잔상 과밀·복수 캐릭터 오독 위험 PARTIAL, 세 후보 공통 작은 phage tail 가독성은 후속 2D 재제작에서 보정·검증.
- 경계: 선택 후 UI 합성·리샘플링·Unity Import·씬 적용은 별도 승인 대상이다.

## 사용자 색·질감 통합 피드백

- 사용자 판정: 박테리오파지가 배경과 따로 노는 느낌이며 캐릭터 색이 지나치게 튄다.
- 처리: I의 구도·먹이사슬·UI 여백은 유지하고, 캐릭터 채도·명도·광원·그림자·픽셀 군집과 이동 잔광만 장면에 통합하는 L~N 보정안을 만든다.
- revision 3 I~K 선별 판정: 현재 사용자 피드백으로 `SUPERSEDED`; 콘텐츠 이력은 보존한다.
- 새 revision: `brief-v4-phage-background-integration-correction-1`, correction `1/2`.
- Unity/MCP/build: 0. 실제 UI·Unity 적용은 별도 승인 대상.

## revision 4 구현자 육안·정적 점검

- L~N 모두 `1672×941` PNG이며 생성 로그의 bytes·SHA-256을 실측 기록했다.
- 세 후보 모두 I의 환경·동물·먹이사슬·왼쪽 UI 여백을 유지하면서 캐릭터 peak brightness, 순백 장갑, 네온 이동선이 감소했다.
- L: 자두·회보라와 이끼 반사광으로 가장 저채도이며 환경 통합이 강하다. 캐릭터 탐색성은 세 안 중 가장 낮을 수 있다.
- M: 남보라·청록 달빛이 수로와 가장 자연스럽게 이어져 차가운 야간 환경 통합이 안정적이다.
- N: 자주·갈색과 호박색 벽돌 반사광으로 캐릭터와 우측 벽돌 공간의 연결이 좋고 표정 가독성이 비교적 유지된다.
- 구현자 추천: `M > N > L`. 독립 검토에서 배경 일치, 실루엣 탐색성, 실제 색 차별성을 함께 확인한다.
- 명시적 글자·HUD·로고·새 오브젝트·범위 밖 콘텐츠 없음. Unity/MCP/build 0.

## revision 4 독립 시각 QA

- 결과: PASS — L/M/N 모두 I 대비 배경 통합과 과도한 보라·네온 문제가 명확히 개선됨.
- canonical run_id: `qa-startup-v4-visual-20260807T012331Z`
- candidate fingerprint: `A8EC0F3FAD91E8FA2B54D734CD759FD59AF1D7D1EDA07FAC0095A2DFDF357444`
- 추천 순위: `M > N > L`.

| 후보 | 팔레트 통합 | 광원·AO | 픽셀·외곽·질감 | trail 완화 | 실루엣·얼굴 | 환경·먹이사슬·여백 보존 | 금지·추적성 | 판정 |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| L | PASS | PASS | PASS | PASS | PARTIAL | PASS | PASS | 가장 저채도이나 본체·얼굴 탐색성 일부 희생 |
| M | PASS | PASS | PASS | PASS | PASS | PASS | PASS | 추천 1위 — 청록 수면·달빛과 가장 자연스럽게 결합 |
| N | PASS | PASS | PASS | PASS | PASS | PASS | PASS | 추천 2위 — 벽돌 호박색 반사와 얼굴 가독성 균형 |

### revision 4 criterion→evidence

| ID | 판정 | current evidence |
| --- | --- | --- |
| V4C1 | PASS | L/M/N `1672×941`, bytes·SHA-256 생성 로그와 실측 일치, 4파일 fingerprint 재현 |
| V4C2 | PASS | I 대비 환경·먹이사슬·동물 행동·왼쪽 여백·도약 구도 보존 |
| V4C3 | PASS | L/M/N 모두 I의 밝은 라벤더·순백 장갑 분리감을 낮추고 동일 달빛·벽돌 반사·AO·픽셀 질감으로 통합 |
| V4C4 | PASS | 연속 보라·시안 rail이 낮은 채도의 끊긴 입자와 희미한 잔상으로 완화 |
| V4C5 | PASS / L PARTIAL | M/N 실루엣·얼굴 PASS, L은 가장 저채도라 탐색성 일부 희생을 사용자에게 공개 |
| V4C6 | PASS | 새 글자·HUD·로고·오브젝트·범위 밖 콘텐츠 0, reference/후속 승인 경계 명시 |
| V4C7 | PASS | edit target·프롬프트·도구·날짜·경로·bytes·해시 기록, `UnityProject/` 복제 0 |

- 공통 개선: 순백 장갑·밝은 라벤더를 회·갈색 환경광으로 낮추고, 연속 광선형 trail을 약한 분절 입자로 바꾸며 접촉 명암과 AO를 강화했다.
- 꼬리 가독성은 I에서 이어진 후속 2D 재제작 과제이며 이번 통합 보정에서 악화되지 않았다.
- 무결성: L~N 모두 `1672×941`, 생성 로그 bytes·SHA-256 일치, `UnityProject/` 내 복제 0.
- 실행 경계: 추가 생성 0, Unity/MCP/build 0.

## revision 4 프로젝트 총괄 최종 판정

- 판정: `내부 승인 가능`
- V4C1~V4C7과 1:1 current evidence, fingerprint, canonical QA run이 현재 후보를 지지한다.
- 사용자 선별 후보: L/M/N 모두 가능, 추천 `M > N > L`.
- 공개 위험: L은 가장 저채도라 캐릭터·표정 탐색성이 일부 낮다. 작은 phage tail은 후속 2D 재제작에서 보정·검증한다.
- 후속 경계: UI 합성·리샘플링·수작업 2D 정리·Unity 적용은 선택 후 별도 승인 대상이다.
