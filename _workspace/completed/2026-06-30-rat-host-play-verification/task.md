# 작업 배정서

## 기본 정보

- 작업 ID: `2026-06-30-rat-host-play-verification`
- 작업명: 쥐 숙주 프로토타입 Play 검증
- 상태: 완료
- 생성일: 2026-06-30
- 담당 에이전트: QA/검증 에이전트
- 완료일: 2026-06-30
- 사용 스킬: `$unity-verification-runner`

## 목적

`RatHostPrototype.unity`가 Unity MCP Play 제어를 통해 실제로 실행 가능한지 확인하고, 조작/모드 전환/변이 선택/UI 입력의 검증 결과를 기록한다.

## 입력 자료

- `AGENTS.md`
- `.agents/qa-verification-agent.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionController.cs`
- `UnityProject/Assets/_Project/Scripts/UI/PrototypeHud.cs`
- 최근 커밋:
  - `a119b29 feat: implement rat host prototype`
  - `9fd3264 fix: add event system fallback for prototype UI`

## 검증 범위

1. Unity MCP 연결 상태 확인
2. 에디터 컴파일/콘솔 오류 확인
3. `RatHostPrototype.unity` 활성 씬 확인
4. `Unity_ManageEditor.Play`로 Play 모드 진입
5. 쥐 모드 시작 상태 확인
6. 쥐 이동 입력 또는 입력 경로 확인
7. 면역 경계도 100% 도달 후 내부 바이러스 모드 전환 확인
8. 변이 조각 수집/성공 또는 상태 전환 로직 확인
9. 변이 선택 UI/EventSystem 및 `1/2/3` 키 선택 경로 확인
10. `Unity_ManageEditor.Stop`으로 Play 모드 종료

## 금지 범위

- 사용자 승인 없는 프로토타입 범위 확장
- 새 게임플레이 기능 추가
- 아트 에셋 생성
- 기존 구현 임의 수정

## 산출물

- 검증 실행 결과 요약
- 발견된 오류와 재현 조건
- 미검증 항목
- 수정 필요 여부

## 완료 기준

- 실제 실행한 검증만 통과로 기록한다.
- Play 모드 진입/종료 결과를 기록한다.
- 검증하지 못한 항목은 명확히 남긴다.
