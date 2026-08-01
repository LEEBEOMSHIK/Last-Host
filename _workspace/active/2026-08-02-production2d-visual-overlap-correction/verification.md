# 독립 QA 검증

상태: QA 완료 — 총괄 내부 승인 검토 가능

## 검증 대상

Production2D 쥐·벽·통·상자 가시 실루엣 겹침 교정이 물리 overlap 없이 앞·뒤를 일관되게 표시하고, 뒤쪽에서는 분리된 몸·꼬리 조각을 남기지 않으며 경계에서 jitter를 만들지 않는지 검증했다.

## 실행한 검증

- 최종 EditMode XML 독립 감사: 전체 `202/202`, TechnicalSample2D `48/48`, 관련 `10/10`
- 접촉 매트릭스: 벽·통·상자 × 8방향 × 3프레임 `72/72`
- 접촉 단계 매트릭스: 72표본 × 6단계 `432/432`
- 대상별 정지 resolver `300`회, 경계 10회 왕복, ±`0.001 world` subpixel 10주기
- 다중 오클루더, 외부 disabled renderer, 좌·우 tail-only 계약 대조
- Unity MCP Play 상태에서 실제 Root 좌표별 resolver·renderer·sorting·collider distance 대조와 1920×1080 즉시 캡처
- 최종 Console Error/Warning, Play 종료, scene dirty, 보호 diff·현황판 상태 감사

## 결과

- XML: `202 passed / 0 failed / 0 skipped / 0 inconclusive`
- 접촉 `72/72`, 단계 `432/432`: 물리 overlap `0`, hidden/renderer 상태 불일치 `0`
- 벽·통·상자 hysteresis 모두 정확히 `0.015625 world`
- 정지 300회와 subpixel 10주기에서 불필요한 가림 전환 `0`
- 최종 동기 캡처 4장과 `qa-visual-captures-final-v2.csv`의 Root·hidden·enabled·sorting·distance가 일치
- `git diff --check` 통과; 이번 QA에서 코드·씬·ProjectSettings 변경 없음

## MCP 플레이 체크

- 씬: `Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- 벽 뒤: Root `(0.000000, 0.937813)`, hidden `true`, renderer `false`, 분리 조각 없음
- 벽 앞: Root `(0.000000, 0.462188)`, hidden `false`, renderer `true`, 쥐 전체 표시
- 통 뒤: Root `(-1.000000, -0.512188)`, hidden `true`, renderer `false`, 분리 조각 없음
- 상자 뒤: Root `(2.000000, 0.507813)`, hidden `true`, renderer `false`, 분리 조각 없음
- 최종 상태: Play/Paused/Compiling/Updating 모두 `false`, scene dirty `false`, Console Error/Warning `0`

## 증거 정정

- 기존 `qa-*-final.png` 4장은 임시 QA 객체가 남은 화면이라 CSV 상태와 불일치했다. PASS 증거에서 제외했다.
- 유효 화면은 `qa-*-final-v2.png` 4장과 `qa-visual-captures-final-v2.csv`다.
- 상세 감사: `artifacts/qa-final-report.md`

## 검증하지 못한 항목

- 사용자의 실제 키보드 WASD 입력과 조작 체감
- 통·상자 뒤 완전 가림의 위치 추적성에 대한 사용자 수용
- 승인 범위 밖 전체 8방향·최종 전후 레이어

## 남은 위험

- 작은 소품 뒤에서는 현재 계약상 쥐 전체가 가려진다. 기술 결함은 아니지만 사용자가 불편하다고 판단하면 별도 silhouette/indicator 또는 오브젝트 전후 레이어 구조가 필요하다.
- 현황판과 `CURRENT.md`는 아직 구현 배정 중으로 표시되어 메인 조정자의 QA 완료 상태 동기화가 필요하다.

## 완료 판단

`PASS — 총괄 내부 승인 검토 가능`
