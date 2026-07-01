# 검증 기록

## 작업 ID

2026-07-01-rat-interaction-input-alert-fix

## 검증 대상

쥐 숙주 프로토타입의 `Space` 상호작용 입력 경로와 면역 경계도 기본 상승 조건.

## 검증 담당

검증 에이전트

## 검증 에이전트 수행 이력

- 검증 에이전트: Epicurus
- 검증 요청자: 메인 에이전트
- 검증한 산출물: 입력/면역 경계도 보정 코드와 테스트
- `agent-activity.md` 반영 여부: 예

## 입력 자료

- 사용자 보고 증상
- 구현 에이전트 산출물
- Unity EditMode 테스트
- Unity MCP 또는 동등 런타임 확인

## 원래 증상 또는 완료 주장

- `Space` 상호작용이 실제 Play 모드에서 동작하지 않음
- Play 모드 진입 직후 면역 경계도가 상승해 상호작용 상승 원인이 불명확함

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 예
- 구현 주체가 실행한 검증과 별도로 확인한 항목: 정적 검증, Unity MCP 상태 모델 검증, Play 모드 무행동 검증, Play 모드 Space 경로 검증

## 실행한 검증

```text
명령 또는 확인 방법:
- 검증 에이전트 정적 확인
- Unity MCP `Unity_RunCommand` 상태 모델 검증
- Unity MCP Play 모드 2초 무행동 세션 확인
- Unity MCP Play 모드 임시 검증 컴포넌트로 Space 입력 경로 확인

결과:
- `RatRiskInteractable`은 `Keyboard.current.spaceKey` 직접 사용 없음
- `Space` 입력 감지는 `Update()`에서 수행
- `PrototypeKeyboardInput.Interact`와 `WasInteractPressed` 존재
- `BaseAlertPerSecond` 기본값 0
- 상태 모델 검증: `PASS defaultAlert=[0]; configuredMode=[InternalVirus]; riskMode=[InternalVirus]; explicitInteract=[True]`
- Play 모드 무행동 검증: `PASS play idle alert: mode=[RatHost], alert=[0]`
- Play 모드 Space 경로 검증: `PLAY_SPACE_VERIFY_PASS startAlert=0; alert=15; mode=RatHost`

해석:
- Play 시작 직후 면역 경계도 자동 상승은 제거됐다.
- `Space` 상호작용은 공통 입력 경로와 런타임 상호작용 경로를 통해 면역 경계도를 상승시킨다.
```

## 검증하지 못한 항목

- 실제 물리 키보드로 사용자가 직접 누르는 수동 조작 확인은 남아 있다.
- 검증 에이전트의 batchmode EditMode 테스트는 Unity 라이선스 초기화 문제로 실행 전 종료됐다. 대신 Unity MCP 컴파일/런타임 검증은 통과했다.

## 실패 또는 경고

- Unity MCP 동적 입력 주입은 직접 `WasPressedThisFrame` 확인에서는 제한이 있었으나, Play 모드 `onBeforeUpdate` 경유 검증 컴포넌트에서는 상호작용 결과가 통과했다.

## 게이트 판정

- QA/검증 게이트 통과 여부: 통과
- `agent-activity.md`에 QA 판정 반영 여부: 예
- 총괄 관리자 검토로 넘길 수 있는지: 예

## 완료 판단

- 완료

## 완료 판단 근거

- 구현 에이전트 산출물, 독립 검증 에이전트 PASS, Unity MCP 상태 모델/Play 모드 검증 PASS를 근거로 완료 판단한다.
