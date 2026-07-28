# Stage2 QA/검증 에이전트 기록

## 요약

- 신규 Stage2 EditMode: `10/10`
- 전체 EditMode 최종: `186/186`
- Stage2 batch Rebuild: 성공
- 씬 논리 계약: Rebuild 후·빌드 후 `2회 PASS`
- Windows Development 빌드: 성공
- 보호 파일·기존 씬·입력·Packages: 보존
- QA 단일 복제본과 후속 임시 Windows 빌드·정적 컴파일 산출물: 제거 완료
- QA 판정: `차단 — 원본 Stage2 씬·MCP Play·Console 미검증`

## 핵심 증거

| 항목 | 결과 |
| --- | --- |
| 신규 테스트 XML SHA-256 | `6BF98E25FF69F040A0727CCE0CA8A29AE348893805B084AB7F4DE2FA7E16ECB9` |
| 전체 최종 XML SHA-256 | `7B7C487724068376D9C95ED106A12D05566F2B94604D3567F88272890C1668EA` |
| QA Windows 실행 파일 | `C:\tmp\LastHostRatHost2DStage2\20260728-065520\LastHostRatHost2DStage2.exe` — 빌드 성공 기록 후 사용자 정리 요청으로 삭제 |
| 실행 파일 SHA-256 | `098A43C3B20762E4BDF938771C36F0FB116126AEC8932B2A77EB403F0CB77938` |
| 복제본 최대 확인 | `2.957 GiB` |
| 복제본 최종 상태 | `Exists=False` |
| 제거 후 C: 여유 | `12.79 GiB` |

## 최초 전체 회귀 실패와 해소

최초 전체 실행은 Stage1 안내 문구를 요구하는 기존 테스트 1개 때문에 `185/186`이었다. 게임플레이 구현 에이전트가 런타임을 바꾸지 않고 해당 테스트만 Stage2 목표 문구인 `변이 조각`, `백혈구 회피` 계약으로 최소 수정했다. QA가 그 한 파일만 기존 단일 복제본에 동기화해 재실행했고 `186/186`을 확인했다.

## 씬 계약 판정

Stage2 계층·물리 컴포넌트·Session 직렬화 참조·HUD·FailurePanel·MutationSelection 셸·두 카메라 target은 Unity API 검사에서 통과했다. 반복 Rebuild의 YAML byte hash는 Unity local fileID 재할당으로 달랐지만, 빌드 내부 Rebuild 뒤에도 논리 계약은 다시 통과했다.

## 차단 경계

원본 Unity의 외부 씬 변경 모달에는 손대지 않았다. 원본 씬은 여전히 Stage1이므로 임시 복제본의 성공 결과가 현재 repo 플레이어블에 자동 적용된 것은 아니다. 원본 MCP Play·Console과 Windows 실행본 수동 플레이는 남아 있다.
