# 작업 로그

## 2026-07-24 KST — 작업 시작

- 사용자가 다음 우선 작업으로 쥐 시각·카메라 관련 EditMode 회귀 기술 게이트 진행을 승인했다.
- 문서/릴리즈 에이전트가 작업 패킷, 공유 상태판, `CURRENT.md`를 시작 상태로 동기화했다.
- 주담당은 QA/검증 에이전트다.
- 전체 EditMode 실행 전에는 코드·테스트·씬을 수정하지 않는다.
- 실패가 나오면 원인 경계에 따라 게임플레이 구현 또는 Unity 씬/통합 구현 에이전트에 최소 수정을 별도 배정한다.
- 사용자 시각 수용과 자연 경계도 엄격 검증 차단 해제는 본 작업 범위에 포함하지 않는다.
- Git 스테이징·커밋·푸시는 수행하지 않았다.

## 2026-07-24 14:09 KST — QA 사전 기준선

- 기존 Unity Editor PID `42724`가 같은 프로젝트를 열고 있었고 PID `88840`, `25940`은 자식 AssetImportWorker로 확인했다.
- 기존 프로세스는 이번 QA가 만든 것이 아니므로 종료하지 않았다.
- Unity MCP 상태는 Edit·비컴파일·비업데이트, 활성 `RatHostPrototype` 씬 clean, Console Error/Warning 0이었다.
- Git은 기존 ProjectSettings 사용자 변경, previews, 시작 문서·본 패킷만 변경 상태였고 staged 0, Builds 변경 0이었다.
- 씬·ProjectSettings·테스트 파일 SHA-256을 사전 기록했다.
- 별도 batchmode는 같은 projectPath 잠금 충돌 위험 때문에 실행하지 않고 기존 Editor TestRunner API로 전환했다.

## 2026-07-24 14:11 KST — 전체 EditMode 실행

- 첫 QA 동적 명령은 중첩 callback 클래스의 MCP 래퍼 재배치로 `CS1527`이 발생해 테스트 전에 중단됐다. 프로젝트 파일·컴파일에는 영향이 없다.
- callback을 최상위 internal 클래스로 바꿔 TestRunner API 전체 EditMode 동기 실행 1회를 완료했다.
- 공식 NUnit XML과 테스트별 callback 로그를 artifacts에 저장했다.
- 결과: 101 total, 99 passed, 2 failed, 0 skipped, 0 inconclusive, 9.3474759초.
- 실패: RatVisual pixel snap 비활성 exact position 비교 1개, 씬 기본 ThirdPerson 카메라 계약 1개.
- 지침에 따라 코드·테스트·씬 수정과 MCP Play를 수행하지 않았다.

## 2026-07-24 14:12 KST — 전후 경계와 판정

- 씬·ProjectSettings·테스트 파일 SHA-256은 전후 동일했다.
- Builds 변경 0, 추가 Unity tracked 변경 0을 확인했다.
- 테스트 후 Editor는 Edit·비컴파일, 활성 씬 clean, Console Error/Warning 0이었다.
- 기존 Unity 프로세스와 workers가 보존됐고 새 batch Editor는 없다.
- QA 판정은 `수정 필요`다. 실패별 담당 경계를 게임플레이 구현과 Unity 씬/통합 구현으로 나눠 인계한다.

## 2026-07-24 14:18 KST — 게임플레이 구현 에이전트 최소 수정

- QA가 분리한 실패 2개를 테스트 계약 원인으로 확인하고 프로덕션 코드·씬·ProjectSettings를 변경하지 않았다.
- pixel snap 비활성 검증은 같은 테스트의 기존 위치 검증과 동일한 `0.0001f` 성분별 허용오차로 바꿨다. X/Y/Z 원위치 보존을 모두 검증하므로 기능 계약은 유지된다.
- 씬 카메라 검증은 임의 `Camera` 검색을 제거하고 `PrototypeCameraController`에 부착된 카메라를 기준으로 `MainCamera`, `Camera.main`, `startingHostMode=QuarterView`를 확인하도록 갱신했다.
- `GameViewFrameCamera`는 별도 untagged 카메라이며 MainCamera와 다름을 고정했다. 가시성 테스트도 임의 카메라 대신 `Camera.main`을 사용하도록 보강했다.
- 첫 집중 실행에서 EditMode의 `CurrentHostMode`가 런타임 초기화 전 기본값 `ThirdPerson`인 점을 확인해 해당 런타임 상태 단언은 제거하고 직렬화된 시작 계약만 검증했다.
- Unity 자산 새로고침·컴파일 후 수정 대상 2개를 집중 재실행해 모두 통과했다.
- 실행 후 Editor는 Edit·비컴파일, 활성 `RatHostPrototype` 씬 clean, Console Error/Warning 0이다.
- `git diff --check` 통과. 전체 101개 재실행과 최종 판정은 독립 QA에 인계한다.

## 2026-07-24 14:20 KST — 수정 후 독립 전체 EditMode 재실행

- 기존 Unity Editor의 공식 TestRunner API에서 전체 EditMode를 다시 실행했다.
- 결과는 101 total, 101 passed, 0 failed, 0 skipped, 0 inconclusive, 6.3731956초다.
- 수정 후 XML과 callback 로그를 별도 artifacts로 보존하고 SHA-256을 기록했다.
- WASD·숙주 본능, v3 방향·걷기, v5b 픽셀 처리, 카메라·씬 관련 테스트를 결과 XML에서 식별해 모두 통과한 것을 확인했다.
- v4 관련 현재 importer 테스트는 TrialV1 64×64·PPU 32 계약이며 v4 128×128·PPU 64·world width 2 직접 자동화는 없음을 남은 위험으로 기록했다.

## 2026-07-24 14:22 KST — MCP Play와 최종 경계

- Play 전 활성 씬 clean, Console Error/Warning 0을 확인했다.
- Play에서 RatHost 모드, QuarterView MainCamera, 별도 GameViewFrameCamera, RatHost·RatVisual·HUD·WorldPixelOutput, 960×540 RT 연결을 확인했다.
- RatVisual ground clearance 0.005, 카메라 추적 표본 오차 0.006300, viewport x 0.500005를 확인했다.
- Play 중 Console Error/Warning은 0이었다.
- Stop 후 Editor가 Edit·비컴파일·비업데이트, 활성 씬 clean, Console Error/Warning 0으로 복귀했다.
- 씬·ProjectSettings·수정 후 테스트 파일 hash가 유지되고 Builds 변경 0, staged 변경 0임을 확인했다.
- QA 최종 판정은 `완료 가능 — 자동 기술 게이트`다. 사용자 시각 수용과 자연 경계도 차단 상태는 별도다.

## 2026-07-24 KST — QA 통과 상태 문서 동기화

- 문서/릴리즈 에이전트가 QA 원문을 기준으로 작업 상태를 `QA 완료 가능 — 총괄 판정 대기`로 동기화했다.
- 공유 상태판의 본 작업과 v3·v4·v5b 관련 행에 전체 EditMode `101/101` 통과를 반영했다.
- v3 사용자 WASD 체감, v4·v5b 사용자 화면 수용, v4 직접 EditMode 자동화 공백은 그대로 유지했다.
- 자연 경계도 엄격 검증은 active·QA `차단`·총괄 `보류`로 유지했다.
- Git 스테이징·커밋·푸시는 수행하지 않았다.

## 2026-07-24 KST — 총괄 승인 후 완료 보관

- 프로젝트 총괄 관리자 판정 `내부 승인 가능`을 확인했다.
- active 작업을 `_workspace/completed/2026-07-24-2026-07-24-rat-visual-camera-editmode-regression/`로 이동했다.
- 이동 전후 절대 경로와 task·work-log·agent-activity·verification·handoff·completion-report·director-review·artifacts 존재를 대조했다.
- v3·v4·v5b 관련 active 작업은 사용자 체감·시각 수용 경계가 남아 완료 처리하지 않았다.
- Git 스테이징·커밋·푸시는 수행하지 않았다.
