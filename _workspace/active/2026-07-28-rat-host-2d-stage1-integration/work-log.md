# 작업 로그

## 2026-07-28

- 사용자가 단계적 2D 이관 브리프의 전체 추천안을 승인하고 계속 진행하도록 요청했다.
- 1단계 구현 작업 패킷을 생성했다.
- 구현은 게임플레이 코드·테스트와 Unity 씬·통합으로 분리 배정한다.
- 사용자 로컬 `ProjectSettings.asset`과 `_workspace/previews/`는 제외한다.
- Unity 씬/통합 구현 에이전트가 별도 `RatHost2DPrototype.unity`, 결정적 씬 빌더, Host HUD, 오염 구역, 내부 전환 셸을 구현했다.
- Unity 정적 연결 검사에서 Tilemap 3개, 충돌 소품 2개, 카메라 추적 대상, 초기 모드·셸 상태, HUD·BuildSettings 보호를 확인했다.
- `C:/tmp/LastHostRatHost2DStage1/20260728-024656/` Windows Development 빌드가 성공했다.
- 빌드 전처리가 자동 변경한 범위 밖 렌더·연결 설정 4개는 빌드 직전 상태로 복구했고 사용자 기존 `ProjectSettings.asset` 변경은 보존했다.
- 씬·통합 구현 기록은 `artifacts/scene-integration.md`에 남겼으며 독립 QA와 사용자 수동 플레이는 대기한다.
- 메인 에이전트 MCP 재생성 메뉴 실행과 필수 계층 생성은 성공했다.
- 재생성 직후 dirty 관측에 따라 씬 import 후 최종 저장·dirty 예외 검증을 추가했다.
- Windows 빌드가 사용자 로컬 상태를 남기지 않도록 렌더·ProjectSettings 보호 파일 5개의 빌드 전 바이트를 `finally`에서 복구하도록 보완했다.
- 동시 Unity 호출 충돌 방지를 위해 구현 에이전트의 도구 사용을 중단했다. 보완 뒤 `GetActive.isDirty=false`와 빌드 후 보호 경로 diff는 메인/QA 대조 항목으로 남긴다.
- 게임플레이 구현 에이전트가 `Scripts/RatHost2D/**`에 새 런타임 어셈블리, 단일 세션, XY↔XZ 본능 어댑터, Rigidbody2D 이동, 오염 구역, HUD snapshot/presenter를 구현했다.
- `Tests/EditMode/RatHost2D/**`에 본능/WASD, 무위험 변화 0, 오염 시험값, 자연 100%, 단일 전환, 전환 후 중지, HUD 계약 테스트를 추가했다.
- Unity가 신규 Runtime/Editor/Tests DLL을 생성해 컴파일 가능한 상태임을 확인했다.
- 신규 EditMode 최초 실행은 leaf 기준 `36 passed / 1 failed`였다. 실패는 Kinematic MovePosition 직후 실제 위치를 검사한 테스트 문제였다.
- 전역 `Physics2D.simulationMode` 변경과 `Physics2D.Simulate`를 사용하는 1차 보완은 `Physics2DSettings.asset` 재직렬화 위험 때문에 제거했다.
- 최종 보완은 `Motor.LastFixedStepDelta`의 Y 양수·X 0과 이동 방향·FollowTarget·LogicalPosition-root 동일성을 검증하는 결정적 EditMode 계약으로 교체했다.
- Unity MCP 동시 호출 충돌 방지 지시에 따라 수정 후 재실행은 중단하고 QA/검증 에이전트에 인계한다. 작업 전체 완료는 아직 주장하지 않는다.
- 게임플레이 구현 파일·API·남은 위험은 `artifacts/gameplay-implementation.md`에 기록했다.
- QA가 동일 Unity `6000.4.6f1`의 임시 소스 복제본에서 전체 EditMode `176/176`, failed/skipped/inconclusive `0`을 확인했다.
- 메인 조정자가 보존된 신규 테스트 결과 `37/37`을 읽기 전용 대조했고, 전체 회귀에도 신규 테스트가 포함됨을 확인했다.
- QA 임시 복제본은 원본 Library·Temp·Logs·Builds·UserSettings·obj를 복사하지 않았고 Unity 자체 생성 Library 포함 최대 확인 `2.703 GiB`였다.
- 테스트 종료 후 정확한 `C:\tmp\LastHostQAProject-20260728-1`만 제거했고 C: 여유 공간은 확인값 `11.78 GiB`에서 `14.56 GiB`로 회복됐다.
- 원본 활성 씬은 Rebuild 전 `RatHost2DPrototype`, `isDirty=false`였고 Stage 1 Rebuild 메뉴 실행은 성공했다.
- Rebuild 직후 Unity 외부 씬 변경 모달이 발생했다. `Reload` 접근성 요소 클릭, 키보드 입력, 정상 창 닫기가 모두 실패했다.
- Unity 강제 종료·재시작은 미저장 데이터 손실 위험 때문에 사용자 명시 승인 없는 파괴적 조치로 거부됐다.
- 원본 MCP Play·Console 최종값·QA 최신 Windows 빌드·빌드 후 보호 diff는 미완료이며 QA는 작업을 `차단`으로 판정했다.
