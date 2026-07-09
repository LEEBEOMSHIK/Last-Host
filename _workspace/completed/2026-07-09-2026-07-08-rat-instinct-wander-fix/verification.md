# 검증 기록: 쥐 숙주 본능 이동 왕복 문제 수정

## 검증 대상

쥐 숙주의 본능 이동이 고정 forward 왕복이 아니라 초기 방향 랜덤화, 주기적 방향 전환, 경계 회피를 통해 생물적 배회에 가까워졌는지 확인한다.

## 실행한 검증

- 런타임 스크립트 수동 컴파일
- 테스트 어셈블리 수동 컴파일
- `git diff --check`
- Unity MCP `GetState`
- Unity MCP Play 진입 후 2초 대기
- Unity Console Error/Warning 확인
- Unity MCP Stop

## 결과

- 런타임 컴파일: 통과
- 테스트 어셈블리 컴파일: 통과
- 공백 검사: 통과
- Unity Editor 상태: Play 전 `IsPlaying=false`, `IsCompiling=false`, `IsUpdating=false`
- Unity Play 체크: Play 진입/Stop 성공
- Unity Console: Error/Warning 0건

## 커밋 직전 재검증

- `git diff --check`: 통과
- 런타임/테스트 어셈블리 수동 컴파일: 통과
- Unity MCP 재확인: `Connection revoked`로 바뀌어 추가 Play 재검증은 불가

## 추가된 회귀 테스트

- `RatHostInstinctWander_PeriodicTurnChangesInstinctDirection`
- `RatHostInstinctWander_BoundarySteersAlongWallInsteadOfVerticalBounce`

## 남은 위험

- 랜덤 배회 체감은 수동 플레이로 조정해야 한다.
- 낮은 조종력에서 좌우 입력이 대각으로 섞이는 것은 의도된 모델이다. 이것이 불편하면 수치 조정 또는 조종력 UI 피드백 보강이 필요하다.
- 커밋 직전 Unity MCP 연결이 다시 해제되어 마지막 Play 재확인은 못 했다.

## 완료 판단

- 코드와 최소 Unity Play 검증은 완료.
- 사용자 플레이 체감 확인 후 커밋 여부를 결정할 수 있다.
