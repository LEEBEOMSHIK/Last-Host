# QA/검증 에이전트 기록

## 요약

- 전체 EditMode: `176 / 176 Passed`
- 신규 RatHost2D 단독 대조: `37 / 37 Passed`
- 테스트 실패·스킵·불확정: 모두 `0`
- 원본 활성 씬 사전 상태: `RatHost2DPrototype`, `isDirty=false`
- Stage 1 Rebuild 메뉴: 실행 성공
- QA 판정: `차단`

## 테스트 실행 방법

활성 Unity 프로젝트는 편집기 잠금으로 두 번째 배치 Unity를 열 수 없고, Test Runner API는 MCP 사용자 상호작용 제한으로 실행되지 않았다. 원본에 캐시나 설정을 남기지 않기 위해 `Assets`, `Packages`, `ProjectSettings` 중심의 임시 소스 복제본을 `C:\tmp`에 만들고 동일 Unity `6000.4.6f1`에서 전체 EditMode를 실행했다.

결과:

```text
total=176
passed=176
failed=0
skipped=0
inconclusive=0
duration=24.8450647
```

임시 복제본은 Unity 자체 생성 캐시 포함 최대 `2.703 GiB`였고 테스트 종료 뒤 정확한 경로 검증 후 제거했다. 기존 `C:\tmp` 빌드는 제거하지 않았다.

## 원본 Play·빌드 차단

원본 Stage 1 Rebuild 메뉴는 성공했지만, 직후 외부 씬 변경 감지 모달이 편집기를 막았다. 접근성에는 `Reload` 버튼이 보였으나 Computer Use 스크린샷/요소 캐시 오류 때문에 안전하게 클릭할 수 없었다. 정상 창 닫기 요청도 실패했고, Unity 강제 종료는 사용자 명시 승인이 없는 파괴적 조치로 거부됐다.

따라서 실제 MCP Play·Console·QA 최신 Windows 임시 빌드는 아직 검증하지 않았다. 구현자 빌드 성공 기록을 독립 QA 빌드 통과로 대체하지 않는다.

## 다음 QA 실행 순서

1. 모달을 `Reload`로 닫는다.
2. 활성 씬 `isDirty=false`, 전체 계층과 연결을 확인한다.
3. Play에서 본능 이동과 실제 WASD 인계·카메라·충돌을 확인한다.
4. 오염 밖/안 변화와 자연 100% 단일 전환·전환 후 동결을 확인한다.
5. Console Error/Warning `0`을 확인한다.
6. Stage 1 전용 Windows 임시 빌드를 새로 실행한다.
7. 기존 씬·입력·Packages·ProjectSettings 사용자 변경·Build Settings·저장소 Builds 보호를 빌드 전후 대조한다.
