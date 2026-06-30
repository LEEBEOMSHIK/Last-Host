# 핸드오프 기록

## 작업 ID

2026-07-01-quarter-left-rear-camera

## 넘기는 에이전트

Codex 메인 에이전트

## 받는 에이전트

게임플레이 구현 에이전트, Unity 씬/통합 구현 에이전트

## 현재 상태

작업 패킷 생성 완료. 쿼터뷰를 정면 후방에서 약간 왼쪽 후방으로 조정하는 구현 작업을 분리 배정한다.

## 루프 게이트 상태

- 작업 배정 게이트: 생성됨
- 담당 산출물 게이트: 대기
- 에이전트 수행 이력 게이트: 진행 중
- QA/검증 게이트: 대기
- 총괄 관리자 게이트: 대기
- 커밋 전 차단 조건: 대기

## 넘기는 이유

코드/테스트와 씬/통합 변경이 필요하므로 메인 에이전트가 직접 구현하지 않고 구현 전담 에이전트에 배정한다.

## 넘기는 에이전트가 완료한 일

- 현재 쿼터뷰가 바로 뒤편 `x=0`임을 확인
- 새 기준을 왼쪽 후방 `x<0`, 후방 우세 `abs(x)<abs(z)`로 정리
- 작업 패킷 생성

## 받는 에이전트에게 기대하는 산출물

- 게임플레이 구현 에이전트: 코드 기본값과 회귀 테스트 변경
- Unity 씬/통합 구현 에이전트: 씬 빌더와 현재 씬 값 동기화

## 이어서 해야 할 일

1. 테스트를 먼저 실패하도록 수정한다.
2. 카메라 기본값을 왼쪽 후방으로 변경한다.
3. 씬 빌더와 현재 씬 값을 같은 오프셋으로 저장한다.
4. 테스트와 Play 검증을 실행한다.

## 참고 자료

- `UnityProject/Assets/_Project/Scripts/Core/PrototypeCameraController.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
- `UnityProject/Assets/_Project/Editor/RatHostPrototypeSceneBuilder.cs`
- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`

## 주의할 점

- 특정 외부 게임의 카메라를 그대로 복제하지 않는다.
- `.codex/config.toml`은 기존 사용자 변경이므로 제외한다.

## 사용자 승인 필요

- 없음

## 에이전트 수행 이력 갱신

- `agent-activity.md`에 인계 기록 추가 여부: 진행 중
- 인계 결과 기록 책임자: Codex 메인 에이전트
