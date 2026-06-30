---
name: unity-verification-runner
description: 마지막 숙주 Unity 프로젝트의 변경사항을 완료로 주장하기 전에 검증 절차, 테스트 명령, 빌드 확인, 플레이 수용 기준, 남은 위험을 정리한다. Unity 구현 검증이나 완료 판단 요청에 사용한다.
---

# Unity Verification Runner

## 기본 역할

Unity 변경사항을 완료로 보고하기 전에 필요한 검증을 정의하고 실행 결과를 정리한다. 테스트나 빌드가 아직 불가능하면 그 이유와 대체 검증을 명확히 적는다.

## 필수 참조 순서

1. `AGENTS.md`
2. 관련 구현 계획 또는 변경 diff
3. `docs/prototype/official/rat-host-prototype.md`
4. `_workspace/README.md`
5. 필요 시 `docs/agents/agent-skill-plan.md`

## 검증 절차

1. 무엇을 완료했다고 주장하려는지 한 문장으로 쓴다.
2. 그 주장을 증명하는 검증 방법을 고른다.
3. 가능한 경우 테스트, 빌드, 플레이 확인을 실행한다.
4. 실행한 명령, 결과, 실패 여부를 기록한다.
5. 검증 결과를 작업 폴더의 `verification.md`에 남긴다.
6. 검증하지 못한 항목은 완료로 주장하지 않는다.

## Unity 검증 후보

- Unity 에디터 컴파일 오류 확인
- EditMode 테스트
- PlayMode 테스트
- PC 빌드
- 핵심 루프 수동 플레이 체크
- 화면/카메라/렌더링 확인

## 산출물 형식

```text
검증 대상:

실행한 검증:
- 

결과:
- 

검증하지 못한 항목:
- 

남은 위험:
- 

완료 판단:
```

## 추가 기준

검증 체크리스트가 필요하면 `references/verification-rules.md`를 읽는다.
