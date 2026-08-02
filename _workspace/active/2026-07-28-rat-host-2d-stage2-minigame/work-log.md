# 작업 로그

## 2026-07-28

- 사용자가 2단계 2D 백혈구 회피 미니게임과 성공·실패 인계 작업 착수를 요청했다.
- 기존 브리프 기준으로 성공은 `MutationSelection` 인계 셸, 실패는 무보상 `RatHost` 60% 복귀로 범위를 고정했다.
- 1단계는 전체 EditMode `176/176`, 신규 `37/37`을 통과했지만 외부 씬 변경 모달로 원본 MCP Play·최신 QA 빌드가 차단된 상태를 상속 위험으로 기록했다.
- 작업 패킷을 만들고 게임플레이 구현과 Unity 씬/통합 구현을 분리 배정할 준비를 했다.
- Unity 씬/통합 구현 에이전트가 `artifacts/scene-integration-plan.md`로 Stage2 목표 계층과 필요한 공개 API를 정리하고 게임플레이 구현 에이전트와 대조했다.
- 확정된 Session/Virus/WBC/Fragment/HUD API를 기존 `RatHost2DPrototypeSceneBuilder.cs`에 연결했다.
- 빌더에 내부 아레나 벽 4개, 바이러스·백혈구·조각 3개 플레이스홀더, 독립 내부 카메라, 안정도·조각·목표·면역포착 HUD, 실패 패널과 MutationSelection 인계 셸을 추가했다.
- 기존 기술 샘플 스프라이트는 읽기 전용 플레이스홀더로만 재사용하고 최종 아트·PPU·타일 규격은 확정하지 않았다.
- C:/tmp Stage2 임시 빌드 경로, 명시적 단일 씬, 보호 파일 snapshot/restore와 scene dirty 실패 계약을 유지했다.
- 외부 씬 변경 Reload 모달을 우회하지 않았으므로 실제 씬 Rebuild/Save, Unity 컴파일·테스트·MCP Play·Console·Windows 빌드는 실행하지 않았다.
- 별도 Unity 임시 복제본은 만들지 않았으며, QA가 단일 임시 복제본으로 컴파일·전체 테스트를 수행한 뒤 모달 해제된 원본 Unity 검증을 이어가도록 인계한다.
- 게임플레이 구현 에이전트가 `RatHost2DSessionController`를 Stage2 coordinator로 확장하고 바이러스 이동, 백혈구 추적·접촉 쿨다운, 고유 index 조각, 내부 HUD runtime을 추가했다.
- Host/Virus 입력 상호 배타, 동일 프레임 복수 조각, 세 번째 조각+치명 접촉 성공 우선, 실패 확인 잠금·60% 무보상, 재진입 초기화를 신규 EditMode 테스트 10개로 고정했다.
- 실패 확인은 Input 자산을 수정하지 않고 실패 대기 중 기존 `PrototypeKeyboardInput.WasInteractPressed()`의 Space 경로만 사용한다.
- Unity Bee rsp와 Unity/NUnit 참조를 재사용한 신규 runtime+test 정적 컴파일은 C# 오류 없이 exit code 0이었다. 직접 Mono Roslyn 실행의 source generator 버전 경고 3개는 정식 Unity 컴파일에서 재확인할 항목이다.
- Reload 모달 때문에 실제 EditMode 실행, MCP Play, Console, Windows 빌드는 수행하지 않았고 QA 단일 임시 복제본에 인계한다.
- QA 전체 EditMode `185/186`에서 Stage1 옛 안내 문구 `실제 미니게임`을 요구하던 `TransitionDisablesHostRootHudAndCollidersAndShowsShell` 1개가 실패했다.
- 승인된 Stage2 런타임 문구는 변경하지 않고 해당 테스트만 `변이 조각`과 `백혈구 회피` 포함 계약으로 최소 수정했다. QA 단일 복제본에서 전체 186개를 재실행한다.
- QA가 `Assets`, `Packages`, `ProjectSettings`만 포함한 단일 최소 복제본 `C:\tmp\LastHostQAStage2-20260728-1`을 만들었다. 복사 직후 크기는 `3.284 MiB`였다.
- 동일 Unity `6000.4.6f1`의 신규 Stage2 EditMode `10/10`, failed/skipped/inconclusive `0`을 확인했다.
- 전체 최초 실행은 옛 Stage1 안내 문구 테스트 1건으로 `185/186`이었고, 게임플레이 구현 에이전트의 해당 테스트 한 파일 최소 수정만 동기화한 뒤 최종 `186/186`을 통과했다.
- 최종 전체 XML SHA-256은 `7B7C487724068376D9C95ED106A12D05566F2B94604D3567F88272890C1668EA`다.
- 임시 복제본에서 Stage2 Rebuild를 실행해 return code 0과 씬 저장을 확인했다.
- Unity API 씬 검사에서 4벽, Virus/WBC, 조각 3개, HUD, FailurePanel, MutationSelection 셸, Host/Virus 카메라 target, Session 직렬화 참조와 scene dirty false를 확인했다.
- 반복 Rebuild의 byte hash는 local fileID 재할당으로 달랐지만 빌드 내부 Rebuild 뒤 논리 계약 검사를 다시 통과했다.
- `C:\tmp\LastHostRatHost2DStage2\20260728-065520\LastHostRatHost2DStage2.exe` Windows Development 빌드가 성공했다.
- BuildReport total size는 `204,848,539 bytes`, 실행 파일 SHA-256은 `098A43C3B20762E4BDF938771C36F0FB116126AEC8932B2A77EB403F0CB77938`이다.
- 빌드 전후 보호 파일·EditorBuildSettings·기존 씬·입력·Packages 해시가 동일했고 repo Builds는 생성되지 않았다.
- QA 복제본 최대 확인 크기는 `2.957 GiB`였다. 정확한 복제본만 제거해 `Exists=False`, 제거 후 C: 여유 `12.79 GiB`, QA 종료 시점의 Windows 빌드 보존을 확인했다.
- 원본 Unity Reload 모달에는 손대지 않았고 원본 `RatHost2DPrototype.unity`는 `InternalVirusShell2D`가 남은 Stage1 씬이다.
- 원본 MCP Play·Console Error/Warning과 실행본 수동 플레이는 미검증이므로 QA 판정은 완료가 아닌 `차단`이다.
- 프로젝트 총괄 관리자가 승인 범위, 담당 산출물, QA 기록, 원본 씬과 Git 보호 경계를 독립 대조했다.
- 자동 기술 게이트는 통과했지만 원본 씬은 Stage1이고 MCP Play·Console이 차단되어 있어 총괄 판정을 `사용자 결정 필요`로 남겼다.
- 원본 Unity Reload 실행 여부를 사용자 결정 사안으로 분리했다. Reload 승인 후 원본 Stage2 Rebuild·QA·문서 동기화·총괄 재검토가 필요하다.
- 공유 현황판·세션 포인터·일부 승인/계획 문서를 QA 최종 상태로 동기화하고, 기존 테스트 최소 수정 수행 주체를 게임플레이 구현 에이전트로 정합화했다.
- 사용자 요청에 따라 `C:\tmp\LastHostRatHost2DStage2`의 임시 Windows 빌드 `205,441,545 bytes`와 `C:\tmp\LastHost.Prototype.RatHost2D.Stage2*` 정적 컴파일 DLL/PDB를 삭제했다. 저장소 파일, `ProjectSettings.asset` 사용자 변경과 `_workspace/previews/`는 보존했다.
- 커밋 요청에 따라 Stage1/Stage2 구현·테스트·승인/QA 기록만 선별 스테이징하려 했으나 `.git/index.lock` 쓰기 권한이 필요했다. 권한 승인 사용량 한도 도달로 `git add`가 거부되어 staged 파일 없이 커밋·푸시를 중단했다. 사용자 `ProjectSettings.asset` 변경과 `_workspace/previews/`는 계속 제외한다.
- 2026-07-29 사용자가 Git 쓰기 실행을 다시 명시 승인했다. 같은 선별 범위로 스테이징하고 Markdown 공백 형식 경고를 정리한 뒤 `d12146f feat: add staged 2d rat host core loop` 커밋을 생성했다. `ProjectSettings.asset`, `_workspace/previews/`, 현황판·CURRENT는 첫 커밋에서 제외했다.
- 2026-07-29 사용자가 현황판의 최우선 작업부터 진행하도록 지시했다. 이를 원본 `RatHost2DPrototype`의 빈 Tilemap·검은 배경 복구, Stage2 Rebuild·Save와 MCP Play·Console 검증 승인으로 기록했다.
- Unity MCP가 다시 응답했고 활성 씬은 `RatHost2DPrototype`, `isDirty=false`, Play 정지 상태임을 확인했다. Computer Use 네이티브 pipe는 사용할 수 없었지만 Reload 차단은 이미 해제된 상태다.
- Unity 씬/통합 구현 에이전트가 원본 Stage1 계층과 Tilemap `0/0/0`, Host 카메라의 검은 바탕 증상을 Unity API와 카메라 캡처로 재현했다.
- Stage2 Rebuild 자체는 계층을 만들었지만 저장 전부터 Tilemap 셀이 `0`임을 새 후조건으로 확인했다. 기존 Tile·Input asset을 `NewScene(Single)` 전에 로드해 씬 교체 과정에서 native asset 참조가 무효화되는 순서가 원인이었다.
- 새 씬 생성 뒤 dependency를 로드하도록 빌더 순서를 최소 수정하고, 현재 씬 asset 강제 재임포트를 제거했으며, Tilemap dirty 처리와 저장 전·후 `117/5/40` 셀 검증을 추가했다.
- 원본 Stage2 Rebuild·Save 후 Unity API에서 Floor `117` `(-6,-4)..(6,4)`, Water `5`, Blocking wall `40`과 `sceneDirty=false`를 확인했다. 디스크에서 씬을 다시 Load한 뒤에도 같은 셀 수·범위가 유지됐다.
- Host 카메라 캡처에서 `13×9` 바닥·외곽 벽·수로·오염 구역·쥐·소품이 함께 보여 맵 범위를 식별할 수 있음을 확인했다.
- 기본 MCP Play에서 `RatHost`의 Host/HostCamera 활성과 Internal 비활성을 확인하고, 직접 면역 경계도 전환 후 `InternalVirus`의 Host 비활성, Internal/InternalCamera·Virus·WBC·4벽 활성화를 확인했다.
- 카메라 캡처 도구의 일시적 RenderTexture 경고를 분리한 뒤 캡처 없이 Play 전환·Stop을 반복했고 최종 Console Error/Warning `0`을 확인했다. 실제 WASD/Space와 성공·실패·재진입 전체 흐름은 독립 QA에 인계한다.
- 게임플레이 코드, Packages, ProjectSettings, 입력 asset, 기존 3D/TechnicalSample 씬은 변경하지 않았고 Windows 빌드는 생성하지 않았다.
- QA/검증 에이전트가 원본 활성 씬 `RatHost2DPrototype`, `dirty=false`를
  독립 대조하고 Tilemap Floor `117`, Water `5`, Blocking `40`,
  Floor/Blocking 범위 `(-6,-4)..(6,4)`를 재확인했다.
- Host 카메라 캡처에서 13×9 바닥·외곽 경계·수로·오염 구역·쥐·소품이
  보여 black-only 증상 해소를 독립 판정했다. 내부 진입 캡처에서는
  아레나 경계·Virus·WBC·조각 3개가 식별됐다.
- 원본 MCP Play에서 실제 키보드 대신 저장 비변경 공개 런타임 API를
  사용해 WBC 접촉 3회 실패, 확인 복귀, 재진입 count 2, 조각 3개 성공과
  MutationSelection 인계를 확인했다.
- Physics2D 네 방향 질의에서 Host 외곽 Blocking, 수로 Water와
  Internal 4벽을 각각 검출했다.
- Main Camera target=RatHost, Internal Camera target=Virus와 각 모드의
  root·HUD·카메라·입력 활성 배타를 확인했다.
- 카메라 캡처 도구 기인 RenderTexture 경고를 분리하고 콘솔을 비운 뒤
  캡처 없이 Play·전환·Stop을 재실행해 최종 Error/Warning `0`을 확인했다.
- 기존 EditMode `10/10`, 전체 `186/186` 기록과 이번 변경 범위를
  대조해 테스트를 중복 재실행하지 않았으며 Windows 빌드도 만들지 않았다.
- 보호 diff는 ProjectSettings 사용자 한 줄과 `_workspace/previews/`를
  그대로 유지했고 Packages·입력·레거시 씬 tracked diff `0`이다.
- 프로젝트 총괄 관리자가 원본 복구·독립 QA·Git 보호 경계를 재검토했다.
  Reload/Stage1 차단은 해소됐고 원본 씬 표시·Stage2 런타임 기술 게이트를
  `내부 승인 가능`으로 판정했다. 실제 OS WASD/Space 손 감각과 화면
  가독성은 사용자 확인으로 남겼으며, 이번 복구에서 Windows 빌드를
  재실행하지 않은 것은 변경 위험과 사용자 요청에 비례해 타당하다고
  기록했다.
- 메인 조정자가 `current-task-board.md`와 `CURRENT.md`를 원본 기술 게이트
  통과·사용자 실제 키보드 확인 대기 상태로 동기화했다. Stage3는 사용자
  수용과 별도 승인 뒤 진행하도록 유지했다.
- 2026-07-29 사용자가 원본 Game View에서 검은 화면이 해소됐고 실제
  이동하는 것을 확인했다. 이를 사용자 수동 QA 부분 수용으로 기록하고,
  남은 확인을 Space 실패 확인과 Internal 화면 전환·가독성으로 좁혔다.
