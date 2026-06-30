# 검증 기록

## 작업 ID

`2026-06-30-rat-host-prototype-implementation`

## 검증 1: Unity MCP 씬 생성

명령:
`Unity_RunCommand`로 `RatHostPrototypeSceneBuilder.RebuildScene()` 실행

결과:
성공. `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity` 생성, 활성 씬과 Build Settings에서 0번 씬으로 확인.

해석:
승인된 씬과 `_Project` 구조가 Unity 직렬화로 생성됐다.

## 검증 2: 핵심 상태 로직

명령:
`Unity_RunCommand`로 면역 경계도, 바이러스 성공/실패, 변이 선택, 재시도 조건 직접 검증

결과:
7개 핵심 로직 검증 통과. 상세 결과는 `artifacts/core-logic-verification.txt`에 기록.

해석:
씬과 무관한 핵심 루프 상태 전환은 기대대로 동작한다.

## 검증 3: 씬 배선

명령:
`Unity_RunCommand`로 `RatHostPrototype.unity` 열기, 주요 참조, HUD, 변이 조각 3개, Build Settings, 누락 스크립트 확인

결과:
7개 씬 배선 검증 통과. 상세 결과는 `artifacts/scene-wiring-verification.txt`에 기록.

해석:
씬 오브젝트와 컴포넌트 참조는 기본 실행 가능한 상태로 연결되어 있다.

## 검증 4: Windows 빌드

명령:
`Unity_RunCommand`로 `RatHostPrototypeSceneBuilder.BuildWindowsPrototype()` 실행

결과:
`Builds/RatHostPrototype/LastHostPrototype.exe` 생성 확인. MCP 결과는 Unity AI Inference/Sentis 패키지 shader 경고 때문에 실패로 표시됐지만, 실행 로그에 빌드 완료 메시지가 남았다.

해석:
Windows 실행본 산출물은 생성됐다. 남은 경고는 이번 프로토타입 코드가 아니라 Unity 패키지 shader 경고다.

## 제한 사항

- 열린 Unity Editor 때문에 별도 Unity batchmode Test Runner는 프로젝트 잠금 상태에서 실행되지 않았다.
- MCP의 공식 Test Runner API 호출은 사용자 상호작용 도구로 분류되어 차단됐다.
- 따라서 `RatHostPrototypeCoreTests.cs`는 작성했지만, 공식 NUnit EditMode Test Runner 통과는 아직 별도 확인이 필요하다.
- 실제 키보드 조작감은 자동 검증하지 않았고, 사람이 에디터 또는 빌드에서 한 번 더 플레이 확인해야 한다.
