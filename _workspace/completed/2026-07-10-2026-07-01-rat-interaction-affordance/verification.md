# 검증 기록

## 작업 ID

2026-07-01-rat-interaction-affordance

## 검증 대상

쥐 숙주 씬에서 상호작용 대상을 플레이어가 식별하고 `Space` 상호작용을 검증할 수 있는지.

## 검증 담당

Codex 메인 에이전트, QA/검증 에이전트

## 검증 에이전트 수행 이력

- 검증 에이전트: QA/검증 에이전트 `Leibniz`
- 검증 요청자: Codex 메인 에이전트
- 검증한 산출물: 코드, 테스트, 씬 빌더, 씬, placeholder 머티리얼, Unity Play 검증 결과
- `agent-activity.md` 반영 여부: 진행 중

## 입력 자료

- 사용자 피드백: `WASD` 이동은 확인했지만 상호작용 대상이 없어 이동 외 키를 확인하지 못함
- 이전 전체 플레이 검증 기록
- 변경 diff
- Unity Play 검증 결과

## 원래 증상 또는 완료 주장

- 원래 증상: 현재 씬에서 상호작용 가능한 대상이 드러나지 않아 `Space` 등 이동 외 키를 확인할 수 없음
- 완료 주장 예정: 상호작용 대상이 식별 가능하고, 근접 HUD와 `Space` 입력 효과가 검증됨

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 부분 분리. 구현은 구현 에이전트가 수행했고, Codex 메인 에이전트가 별도 Unity MCP 검증을 실행했다. QA/검증 에이전트 판정은 대기 중이다.
- 구현 주체가 실행한 검증과 별도로 확인한 항목: 최종 affordance assertion 5/5, Play 상호작용 검증, Unity Console Error 0건, 씬뷰 다각도 시각 확인

## 실행한 검증

```text
명령 또는 확인 방법:
구현 에이전트 TDD/검증 보고

결과:
- RED: `PrototypeSessionState.IsRatRiskInteractionAvailable` 부재로 신규 회귀 테스트 실패 확인
- GREEN: 신규 회귀 테스트 3개 통과
- `RatHostPrototypeCoreTests` 21/21 통과 보고
- Play 검증: 근접 시 HUD `소음 배관 조사 가능`, `Space` 입력 후 면역 경계도 상승, 이탈 후 `하수도 탐색 중`

해석:
구현 주체의 1차 검증은 요구한 동작을 충족한다. 단, 최종 보고 전 별도 검증으로 재확인했다.
```

```text
명령 또는 확인 방법:
Unity MCP `Unity_RunCommand` 동등 조건 21개 명시 검증

결과:
`Equivalent RatHost prototype assertions passed 21/21`

해석:
Unity Test Runner API는 MCP 동적 명령에서 테스트 asmdef/NUnit 참조 및 사용자 상호작용 제한으로 직접 실행이 어려웠다. 대신 동일 제품 코드와 씬을 직접 호출해 면역 경계도, 변이, 바이러스 미니게임, 입력, 카메라, 씬 프레이밍, 상호작용 표식 조건을 검증했다.
```

```text
명령 또는 확인 방법:
Unity MCP Play 모드 상호작용 검증

결과:
통과.
- 시작 HUD: `하수도 탐색 중`
- 쥐를 `NoisyPipeRiskInteractable` 범위로 이동 후 HUD: `소음 배관 조사 가능`
- `Space` 입력 주입 후 면역 경계도 상승 확인
- Trigger Exit 후 HUD: `하수도 탐색 중`

해석:
원래 증상인 `Space` 상호작용 수동 검증 차단 사유가 해소됐다. 대상 근접 상태와 입력 효과가 Play 모드에서 연결되어 있다.
```

```text
명령 또는 확인 방법:
Unity MCP `Unity_SceneView_CaptureMultiAngleSceneView`

결과:
`NoisyPipeRiskInteractable` 포커스 다각도 캡처에서 파란 배관, 빨간 밸브, 노란 표시 링이 보임.

해석:
기존 갈색 큐브보다 상호작용 대상이 시각적으로 분리된다.
```

```text
명령 또는 확인 방법:
Unity MCP `Unity_RunCommand` 최종 affordance assertion 5개

결과:
`FINAL_AFFORDANCE_ASSERTIONS_OK passed 5/5`
- 세션 affordance 상태와 prompt clear
- HUD prompt 표시
- 씬 카메라 `MainCamera` 태그와 `Camera.main`
- `NoisyPipeRiskInteractable` 배관/밸브/표식 자식
- `Camera.main` 기반 카메라 상대 이동

해석:
상호작용 보정 후 발견된 `MainCamera` 태그 회귀 가능성도 함께 검증했다.
```

```text
명령 또는 확인 방법:
코드 리뷰 수정 후 Unity MCP `Unity_RunCommand` 최종 리뷰 수정 assertion 3개

결과:
`FINAL_REVIEW_FIX_ASSERTIONS_OK passed 3/3`
- 공백/null prompt는 상호작용 가능 상태를 켜지 않음
- HUD prompt 표시 유지
- 씬 카메라 `MainCamera` 태그, `Camera.main`, 상호작용 표식 유지

해석:
코드 리뷰에서 지적된 공백 prompt 상태 버그와 테스트 reflection 취약성 수정 후 핵심 조건이 유지된다.
```

```text
명령 또는 확인 방법:
Unity MCP `Unity_GetConsoleLogs` Error

결과:
Error 0건.

해석:
최종 Play/검증 후 Unity 콘솔 게임 코드 오류는 관찰되지 않았다.
```

```text
명령 또는 확인 방법:
`git diff --check` 대상 코드 파일

결과:
Exit 0. CRLF 변환 경고만 출력됨.

해석:
공백 오류는 관찰되지 않았다.
```

## 검증하지 못한 항목

- Unity Test Runner 정식 배치 실행: 현재 같은 프로젝트가 Unity Editor에 열려 있어 별도 Unity 배치 프로세스가 `Multiple Unity instances cannot open the same project`로 차단됨
- 사용자가 직접 Game View에서 보정 후 상호작용 대상을 보고 `Space`를 누른 수동 확인

## 실패 또는 경고

- Unity MCP 동적 명령에서 `RatHostPrototypeCoreTests` 테스트 클래스 직접 참조는 테스트 asmdef 참조 제한으로 실패했다.
- Unity Test Runner API는 MCP에서 사용자 상호작용 필요 도구로 차단됐다.
- 별도 Unity 배치 테스트는 현재 같은 프로젝트를 연 Unity 인스턴스 때문에 차단됐다.
- `.codex/config.toml`은 작업 시작 전부터 modified 상태이며 이번 작업 커밋 대상에서 제외해야 한다.

## 게이트 판정

- QA/검증 게이트 통과 여부: 부분 통과. 사용자 수동 확인은 별도 체크리스트로 분리
- `agent-activity.md`에 QA 판정 반영 여부: 완료
- 총괄 관리자 검토로 넘길 수 있는지: 가능

## 완료 판단

- 완료 처리 가능. 사용자 수동 플레이 체감 확인은 별도 보류 항목

## 완료 판단 근거

구현 에이전트 검증과 별도 Unity MCP 검증에서 핵심 조건은 통과했다. QA/검증 에이전트는 자동/Play 검증 기준으로 총괄 관리자에게 넘길 수 있다고 판정했고, 프로젝트 총괄 관리자는 내부 승인 가능으로 판정했다. 2026-07-10 사용자 지시에 따라 사용자 직접 수동 확인은 `docs/project-handoff/manual-play-checklist.md`로 분리해 보류하고, 이 작업은 자동/내부 검증 기준으로 완료 처리한다.

## QA/검증 에이전트 판정

```text
판정:
QA_PARTIAL

근거:
- 보정 범위는 승인된 쥐 숙주 프로토타입 안의 상호작용 대상 식별성 개선이다.
- 회귀 테스트 21/21 보고, Unity MCP 동등 조건 21/21, 근접 HUD `소음 배관 조사 가능`, `Space` 입력 후 면역 경계도 상승, 이탈 후 `하수도 탐색 중`, 시각 표식 확인, 최종 affordance assertion 5/5, Console Error 0건이 기록되어 있다.
- `git diff --check`는 공백 오류 없이 통과했고 CRLF 경고만 있었다.

미검증:
- 사용자가 보정 후 Game View에서 직접 대상 식별과 `Space` 입력을 확인하지는 않았다.
- 열린 Unity Editor 때문에 Unity Test Runner 정식 배치 실행은 차단됐다.

남은 위험:
- 씬 파일 diff가 재직렬화로 크므로 커밋 전 관련 변경만 포함해야 한다.
- `.codex/config.toml`은 이번 작업 대상에서 제외해야 한다.

총괄 관리자에게 넘길 수 있는지:
넘길 수 있다. 단, 최종 완료 보고에는 위 미검증 2건을 반드시 남기는 조건이다.
```

## 코드 품질 리뷰 판정

```text
판정:
CODE_REVIEW_PASS

근거:
리뷰 대상 코드, 씬, 테스트에서 추가 수정이 필요한 품질 이슈는 발견되지 않았다. 이전 리뷰에서 지적된 공백 prompt 정규화와 테스트 reflection 제거도 반영됐다.

남은 위험:
- `.codex/config.toml`은 작업 범위에서 제외하고 커밋 대상에서도 제외해야 한다.
- 신규 머티리얼 3종과 `.meta`는 씬 참조 GUID 유지를 위해 작업 커밋에 포함해야 한다.
- 정식 Unity Test Runner 배치 로그는 없고, 현재 근거는 MCP assertions, Console Error 0건, `git diff --check` 통과다.
```

## 프로젝트 총괄 관리자 판정

```text
판정:
PROJECT_DIRECTOR_APPROVE

근거:
이번 변경은 승인된 쥐 숙주 프로토타입 범위 안의 상호작용 식별성 보정이며, 새 숙주/시스템/패키지/최종 아트 추가로 보이지 않는다. 플레이스홀더 머티리얼 3종과 `.meta`는 씬 참조 유지를 위해 작업 범위에 포함 가능하다.

게이트 확인:
작업 패킷, 담당 구현 에이전트 기록, QA/검증 기록, 코드 리뷰 판정이 존재한다. 메인 에이전트 직접 구현 예외는 없고, 코드/씬 변경은 구현 에이전트 산출물로 기록되어 있다.

QA 기록:
`QA_PARTIAL`이지만 핵심 조건은 통과했다. Unity MCP 동등 조건 21/21, Play 검증의 근접 HUD/`Space` 면역 경계도 상승/이탈 HUD clear, 씬뷰 배관·밸브·노란 링 확인, 최종 리뷰 수정 assertion 3/3, Console Error 0건, `git diff --check` 통과가 기록되어 있다. 코드 품질 리뷰는 `CODE_REVIEW_PASS`다.

남은 미검증:
- 사용자가 보정 후 Game View에서 직접 대상 식별과 `Space` 입력을 확인하지 않음
- 열린 Unity Editor 때문에 정식 Unity Test Runner 배치 실행 로그가 없음
- `.codex/config.toml`은 이번 작업 범위 밖 변경으로 커밋/완료 범위에서 제외해야 함

다음 단계:
사용자 Game View 수동 확인 후 최종 완료 처리. 커밋 시 작업 변경 파일과 신규 머티리얼 `.meta`는 포함하고, `.codex/config.toml` 및 무관 작업 폴더는 제외한다.
```

## 2026-07-10 완료 처리 판정

```text
판정:
COMPLETE_WITH_MANUAL_FEEL_CHECK_DEFERRED

근거:
사용자 요청에 따라 수동 플레이 체감 확인은 별도 체크리스트로 분리해 보류했다. 상호작용 식별성 작업 자체는 기존 QA/검증 기록, 코드 품질 리뷰, 프로젝트 총괄 관리자 내부 승인, 현재 UnityProject diff 없음 확인을 근거로 완료 처리한다.

완료로 주장하는 범위:
- 상호작용 대상 배관/밸브/표식 링이 구현되어 있다.
- 근접 HUD 문구와 `Space` 상호작용 면역 경계도 상승은 Play/MCP 검증으로 확인됐다.
- 코드 품질 리뷰 보정과 핵심 assertion 검증이 완료됐다.

완료로 주장하지 않는 범위:
- 사용자가 실제 빌드나 Game View에서 조작감과 이해도를 확인했다는 주장
- 정식 Unity Test Runner 배치 결과 파일 통과
```
