# 검증 기록

## 작업 ID

`2026-08-08-pixel-motion-comic-cinematics`

## 검증 대상

- 픽셀아트 모션 코믹형 시네마틱 제작 기준
- 신규 픽셀아트 시네마틱 연출 에이전트와 운영 색인

## 실제 수행·검증 이력

| 역할/에이전트 | 실제 수행 | 산출물·판정 |
| --- | --- | --- |
| 프로젝트 조정 | R3 S0·소유권·비용 예산과 상태 통합 | 완료 |
| 프로젝트 총괄 사전 검토 | 최초 계약 감사와 correction 1 재검토 | 수정 필요 → 착수 가능 |
| 문서/릴리즈 owner | 가이드·역할 파일과 7개 관련 문서 동기화 | 자체 정적 검사 PASS |
| 독립 QA `cinematic_docs_qa` | C1~C7 역할·범위·링크·diff 대조 | PASS |

## 원래 증상 또는 완료 주장

- 컷신 기본 형식과 전담 설계 역할이 없어, 생성 이미지부터 Unity 재생까지의 책임 인계가 정의되지 않았다.
- 완료 주장: 기본 형식과 전담 역할이 문서·색인에 일관되게 반영됐으며 실제 제작은 후속 승인으로 분리됐다.

## 현재 검증 revision

- 위험 등급: R3
- verification revision: production v1 / S0 correction 1
- candidate fingerprint: `0D8C39EBD2BB8BB9DA6CCF3A8B33488B3E57AF30F6545F1193F13576832898B0`
- fingerprint 대상: `AGENTS.md`, 역할 2개, agent plan/reference map, narrative README/main scenario/guide, graphics direction 총 9개
- canonical run_id: `qa-pixel-motion-comic-static-20260808-001`
- candidate frozen 여부: 예 — 총괄 최종 검토 전 production 동결
- 마지막 production 변경: 문서/릴리즈 owner의 9개 문서 적용
- 이 검증이 마지막 production 변경 이후 실행됐는지: 예
- correction cycle: 1/2 — production 전 계약 보완

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 예
- 구현자 자체 검사와 별도로 확인한 항목: 가이드 완전성, 생성형 이미지 경계, 프로토타입 범위, 신규 역할, 여섯 권한 비중복, 링크, Unity 무변경

## 실행한 검증

| criterion ID | 검증 방법 | 결과 | canonical 근거 |
| --- | --- | --- | --- |
| C1 | 가이드 §1~§10의 정의·적용·숏·레이어·모션·자막/오디오·Unity 인계 대조 | PASS | 시네마틱 가이드 |
| C2 | 후보와 최종 픽셀·프레임·영상·Unity 에셋 경계 대조 | PASS | 가이드 §9 |
| C3 | 개별 제작 별도 승인·전체 캠페인 확대 금지 대조 | PASS | 가이드 §1·§5·§10, 시나리오 10장 |
| C4 | 신규 역할 담당·금지 범위 대조 | PASS | 신규 에이전트 파일 |
| C5 | 연출·이미지·픽셀 검토·Unity·QA·선택/승인 단일 소유자 대조 | PASS | 가이드 §11, 역할 협업 경계 |
| C6 | roster·agent plan·reference map·narrative README 링크 11개 존재 확인 | PASS | 관련 색인 |
| C7 | `git diff --check`, AGENTS 줄 수, UnityProject 변경 여부 확인 | PASS | diff 오류 0, AGENTS 143줄, Unity 변경 0 |

## 검증하지 못한 항목

- 실제 시네마틱의 화면 품질·타이밍·자막 가독성: 이번 작업에서 에셋·애니매틱·Unity를 만들지 않았으므로 후속 제작 범위

## 실패 또는 경고

- 총괄 사전 검토에서 비주얼 역할 입력과 여섯 권한·비최종 애니매틱 경계가 부족해 correction 1이 필요했다.
- correction 1에서 계약을 보완한 뒤 총괄 `착수 가능`, production 이후 독립 QA PASS를 받았다.

## 비용 실행 대조

| 비용 항목 | 계획 예산 | 실제 수·근거 | 판정 |
| --- | --- | --- | --- |
| 실제 역할·인계 | 총괄 사전1→문서1→QA1→총괄 최종1 | 총괄 사전 최초+correction1, 문서 owner 최초 차단+직접 승인 후 owner1, QA1, 총괄 최종1 | 주의 |
| 표적 검증 | C1~C7 정적 대조1 | owner 자체1, 독립 QA1 | 정상 |
| Unity/MCP/빌드·full suite | 0 | 0 | 정상 |
| matrix/capture·artifact | 0 | 0 | 정상 |
| correction·무효/폐기 | 0 | 계약 correction 1, 승인 전달이 인정되지 않은 owner 시도1(변경 0) | 주의 |

- 비용 판정: 주의 — 안전한 승인·권한 경계 correction 1과 변경 없는 owner 재배정 1
- 회피 가능 비용: 최초 위임에 실제 사용자 승인 턴이 포함되지 않아 발생한 변경 0 차단 시도
- 고비용 실행·대형 artifact: 없음

## 프로젝트 총괄 관리자 판정

- 사전 판정: correction 1 후 착수 가능
- 최종 판정: 내부 승인 가능
- 승인 범위: 시네마틱 형식과 에이전트 역할·문서 개정만 승인. 실제 제작·Unity 적용은 후속 승인

## 완료 판단

- 기술 검증 통과 — 사용자 수용 대기

## 사용자 수용 상태

- 사용자 직접 확인 필요: 가이드의 기본 제작 방향
- 확인 전 `완료` 표현 금지 여부: 예

## 최종 상태

- 내부 승인 가능 — 사용자 수용 대기, 미커밋
