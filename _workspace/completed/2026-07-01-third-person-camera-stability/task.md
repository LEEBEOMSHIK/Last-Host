# 작업 배정서

## 기본 정보

- 작업 ID: 2026-07-01-third-person-camera-stability
- 작업명: 3인칭 카메라 회전 안정화
- 상태: 완료
- 생성일: 2026-07-01
- 담당 에이전트: Codex
- 보조 에이전트: 없음
- 사용 스킬: systematic-debugging, test-driven-development, unity-verification-runner, verification-before-completion

## 목적

쥐 숙주 프로토타입의 3인칭 카메라가 쥐 회전을 따라 돌아가면서 조작 방향이 혼란스러워지는 문제를 수정한다.

## 입력 자료

- 사용자 보고: 3인칭일 때 카메라가 계속 돌아간다.
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeCameraController.cs`
- `UnityProject/Assets/_Project/Scripts/Host/RatHostController.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
- `docs/prototype/official/rat-host-prototype.md`

## 해야 할 일

1. 3인칭 카메라 회전 문제의 원인을 확인한다.
2. 회귀 테스트를 추가한다.
3. 3인칭 카메라가 쥐 위치만 따라가고 쥐 회전은 따라가지 않도록 수정한다.
4. EditMode 테스트와 Play 상태 검증을 실행한다.

## 산출물

- 3인칭 카메라 안정화 코드 수정
- 카메라 회전 회귀 테스트
- 검증 기록

## 금지 범위

- 쥐 숙주 프로토타입 범위 외 게임플레이 추가
- 전체 카메라 시스템 구조 변경
- 신규 패키지 추가

## 승인 필요 항목

- 없음. 사용자가 프로토타입 구현과 카메라 방향 전환 작업을 승인한 범위 안의 버그 수정이다.

## 완료 기준

- 쥐가 회전해도 3인칭 카메라 오프셋과 회전이 유지된다.
- 쿼터뷰 전환 기능이 유지된다.
- 관련 테스트와 Play 검증에서 오류가 없다.
