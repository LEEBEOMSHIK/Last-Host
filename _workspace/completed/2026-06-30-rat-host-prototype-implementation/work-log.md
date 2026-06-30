# 작업 로그

## 작업 ID

`2026-06-30-rat-host-prototype-implementation`

## 로그

### 2026-06-30

- 수행 내용: 사용자 요청을 승인된 쥐 숙주 프로토타입 실제 구현 작업으로 분류했다.
- 판단: 공식 구현 계획이 이미 승인되어 있으므로 새 기획 승인은 요구하지 않는다.
- 판단: 테스트 가능한 핵심 상태 로직은 TDD로 진행하고, Unity 씬/배치 검증은 Unity 테스트와 수동 검증 기록으로 보완한다.
- 수행 내용: `UnityProject/Assets/_Project/` 구조를 만들고 런타임 asmdef와 테스트 asmdef를 추가했다.
- 수행 내용: 면역 경계도, 바이러스 미니게임, 변이 적용, 세션 전환 상태 모델과 NUnit EditMode 테스트를 작성했다.
- 수행 내용: 쥐 이동, 위험 구역, Space 상호작용 위험 증가, 바이러스 이동, 백혈구 추적, 변이 조각 수집, 재시도, 변이 선택 HUD를 구현했다.
- 수행 내용: Unity Editor 자동화 스크립트로 `RatHostPrototype.unity`, 플레이스홀더 재질, 입력 액션 자산, Build Settings를 생성했다.
- 수행 내용: Unity MCP에서 핵심 상태 로직 검증, 씬 배선 검증, Windows 빌드를 실행했다.
- 주의: 열린 Unity Editor 때문에 batchmode Test Runner는 실행하지 못했고, MCP의 공식 Test Runner API 호출은 사용자 상호작용 도구로 분류되어 차단됐다.
- 결과: `Builds/RatHostPrototype/LastHostPrototype.exe`가 생성됐으며, 빌드 경고는 Unity AI Inference/Sentis 패키지 shader 경고만 남았다.

## 열린 질문

- 없음.

## 위험과 주의점

- 공식 Unity Test Runner 실행은 아직 별도 확인이 필요하다.
- 실제 플레이 조작감은 에디터/빌드에서 사람이 한 번 더 확인하는 것이 좋다.
