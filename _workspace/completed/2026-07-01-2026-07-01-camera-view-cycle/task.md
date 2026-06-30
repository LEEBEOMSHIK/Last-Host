# 작업 배정서

## 기본 정보

- 작업 ID: 2026-07-01-camera-view-cycle
- 작업명: 쿼터뷰 후방 배치 및 탑뷰 전환 추가
- 상태: 진행 중
- 생성일: 2026-07-01
- 담당 에이전트: Codex 메인 에이전트
- 보조 에이전트: QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: receiving-code-review, systematic-debugging, test-driven-development, pixel-lowpoly-style-keeper, unity-verification-runner

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: Unity 코드와 테스트 변경
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예

## 목적

쥐 숙주 프로토타입의 카메라 전환에서 쿼터뷰가 플레이어 진행 방향 뒤쪽에서 보이도록 수정하고, 누락된 탑뷰 전환을 추가한다.

## 입력 자료

- 사용자 피드백: 쿼터뷰가 캐릭터 앞쪽으로 전환되어 조작이 어렵고, 탑뷰 전환이 없다.
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeCameraController.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
- `docs/agents/loop-engineering-gates.md`

## 해야 할 일

1. 현재 카메라 모드와 오프셋 동작을 확인한다.
2. 쿼터뷰가 후방 기준인지 테스트로 고정한다.
3. 탑뷰 모드와 전환 순서를 테스트로 고정한다.
4. 구현 후 EditMode 테스트와 Play 상태 검증을 실행한다.
5. QA/검증 에이전트와 총괄 관리자 판정을 받는다.

## 산출물

- 카메라 모드 코드 수정
- 카메라 회귀 테스트 추가/수정
- 검증 기록
- 완료 보고

## 금지 범위

- 새 아트 에셋 생성
- 프로토타입 범위 확대
- 쥐 숙주 외 신규 콘텐츠 추가

## 승인 필요 항목

- 없음. 승인된 쥐 숙주 프로토타입의 카메라 조작 보정 범위다.

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 필요
- 담당 에이전트 산출물 확인: 필요
- QA/검증 에이전트 기록 확인: 필요
- 총괄 관리자 판정 확인: 필요
- 승인 게이트 확인: 필요
- 완료 판단에 영향을 주는 미검증 항목: 없어야 함

## 완료 기준

- 쿼터뷰가 진행 방향 뒤쪽에서 하수도 맵과 쥐를 보도록 배치된다.
- 카메라 전환이 `3인칭 -> 쿼터뷰 -> 탑뷰 -> 3인칭`으로 동작한다.
- 탑뷰는 정사영 상단 시야로 쥐를 프레이밍한다.
- 관련 테스트와 Unity Play 검증이 통과한다.
