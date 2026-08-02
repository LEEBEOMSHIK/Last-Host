# 검증 기록

## 검증 대상

`2단계 2D 백혈구 회피 미니게임과 성공·실패 인계`

완료 주장 범위는 다음으로 제한한다.

- 내부 바이러스 모드 진입과 입력 배타
- 바이러스 이동·백혈구 접촉·조각 3개 집계
- 같은 프레임 성공 우선
- 실패 확인 전 잠금과 `RatHost` 60% 무보상 복귀
- 재진입 초기화
- Stage2 씬 생성·논리 연결·Windows 임시 빌드

실제 변이 선택·효과와 성공 후 쥐 복귀는 Stage3 범위다.

## QA 판정

`통과 — 원본 씬 표시·Stage2 런타임 기술 게이트 통과, 사용자 부분 수용`

2026-07-29 사용자 수동 확인:

- 검은 화면 해소 확인
- 실제 이동 확인
- Space 실패 확인과 Internal 화면 체감은 아직 별도 확인이 필요함

2026-07-29 원본 Unity 차단 해제 뒤 Stage2 씬을 다시 생성해 Floor `117`,
Water `5`, Blocking `40` 셀의 영속화와 `dirty=false`를 확인했다.
Host 화면은 `13×9` 맵·외곽 경계·수로가 식별되어 black-only 증상이
해소됐다. 원본 MCP Play에서 실패·복귀·재진입·성공, 카메라·입력 활성
배타와 최종 Console Error/Warning `0`을 독립 확인했다.

실제 OS 키보드 주입은 MCP에 없으므로 공개 런타임 API와 Physics2D
질의를 대체 경로로 사용했다. 상세 증거는
`artifacts/original-scene-qa.md`에 기록한다.

Windows 빌드는 사용자 요청에 따라 다시 만들지 않았다.

## 단일 임시 복제본

- 경로: `C:\tmp\LastHostQAStage2-20260728-1`
- 생성 시 포함: `Assets`, `Packages`, `ProjectSettings`
- 원본에서 복사하지 않은 경로:
  - `Library`
  - `Temp`
  - `Logs`
  - `UserSettings`
  - `Builds`
- 복사 직후 크기: `3,443,440 bytes` (`3.284 MiB`)
- QA 중 최대 확인 크기: `3,174,995,870 bytes` (`2.957 GiB`)
- QA 시작 전 C: 여유 확인값: `15,862,669,312 bytes` (`14.77 GiB`)
- 제거 직전 C: 여유: `10,589,245,440 bytes` (`9.832 GiB`)
- 제거 후 C: 여유: `13,738,450,944 bytes` (`12.79 GiB`)
- 최종 확인: `CloneExists=False`
- QA 종료 시점에는 Windows 빌드를 복제본 밖 별도 경로에 보존: `BuildPreserved=True`
- 2026-07-28 사용자 정리 요청 후 해당 임시 빌드와 정적 컴파일 DLL/PDB 삭제: `BuildExists=False`

최초 제거는 120초 시간 제한 동안 대부분을 제거하고 `216.82 MiB`가 남았다. 점유 Unity/bee 프로세스가 없음을 확인한 뒤 같은 정확한 경로만 두 번째로 제거해 최종 `Exists=False`를 확인했다.

## 신규 Stage2 EditMode

명령:

```text
Unity.exe -batchmode -nographics
  -projectPath C:\tmp\LastHostQAStage2-20260728-1
  -runTests -testPlatform EditMode
  -testFilter RatHost2DStage2SessionTests;RatHost2DStage2RuntimeTests
```

결과:

- total `10`
- passed `10`
- failed `0`
- skipped `0`
- inconclusive `0`
- duration `0.2242627s`
- XML SHA-256:
  - `6BF98E25FF69F040A0727CCE0CA8A29AE348893805B084AB7F4DE2FA7E16ECB9`

통과 계약:

- Host/Virus 입력 상호 배타
- 서로 다른 고유 조각의 같은 프레임 집계와 같은 index 중복 거부
- 세 번째 조각과 치명 접촉이 같은 프레임이면 성공 우선
- 조각+접촉 큐 단일 판정
- 실패 확인 전 잠금, 확인 뒤 `RatHost` 60%, 새 변이 없음
- 접촉 `0.5초` 쿨다운
- 조각 run당 단일 수집
- 실패 복귀 후 재진입 런타임 초기화
- 백혈구 접촉 창당 안정도 `34` 감소와 면역 포착 `+8`
- 바이러스 단일 논리 root·FollowTarget·collision motor

## 전체 EditMode

### 최초 실행

- total `186`
- passed `185`
- failed `1`
- skipped `0`
- inconclusive `0`
- XML SHA-256:
  - `F90F82E6F1463B7118E5469DFA82377453EAF165E9BA4FE7B0DC58AE2DC7B9CD`

실패:

```text
RatHost2DSessionTests.TransitionDisablesHostRootHudAndCollidersAndShowsShell
Expected: "실제 미니게임" 포함
Actual: "변이 조각 3개 수집 / 백혈구 회피"
```

게임플레이 구현 에이전트가 승인된 Stage2 문구에 맞게 해당 회귀 테스트 한 파일만 최소 수정했다. QA는 repo 구현 파일을 수정하지 않고 그 최신 테스트 파일만 같은 임시 복제본에 동기화했다.

### 최소 수정 동기화 후 재실행

- total `186`
- passed `186`
- failed `0`
- skipped `0`
- inconclusive `0`
- duration `13.8119799s`
- XML SHA-256:
  - `7B7C487724068376D9C95ED106A12D05566F2B94604D3567F88272890C1668EA`

최초 빈 Library 임포트 중 ShaderGraph PackageCache의 `GUID` CS0246 2건이 일시 발생했으나 Unity API Updater가 해당 캐시를 갱신하고 재컴파일했다. 최종 Unity 테스트 실행에는 제품 C# 컴파일 오류가 없었다.

## Stage2 씬 Rebuild와 논리 계약

실행 메서드:

```text
LastHost.Prototype.RatHost2D.Editor
  .RatHost2DPrototypeSceneBuilder.RebuildScene
```

결과:

- Unity batchmode return code `0`
- `RatHost2DPrototype.unity` 생성·저장 성공
- 로드 후 `sceneDirty=false`

임시 복제본 전용 Unity QA 검사기로 다음을 직접 확인했다.

```text
STAGE2_QA_CONTRACT PASS
root=1
session=1
walls=4
virus=1
wbc=1
fragments=3
hud=1
failurePanel=1
mutationShell=1
hostCameraTarget=rat
internalCameraTarget=virus
internalColliders=9
sceneDirty=false
```

추가 확인:

- 4벽 모두 비트리거 `BoxCollider2D`
- Virus: Dynamic Rigidbody2D, 비트리거 CircleCollider2D, 자체 틱이 꺼진 충돌 motor
- WBC: Dynamic Rigidbody2D, 트리거 CircleCollider2D, Virus logical root 추적
- 조각 index `0,1,2` 고유, 각 트리거 Collider
- Stage2 HUD의 Session/Text/Slider 직렬화 참조 모두 연결
- FailurePanel과 MutationSelection 셸 초기 비활성
- Host 카메라 target은 쥐 root, 내부 카메라 target은 Virus root
- Session의 Stage2 root·카메라·패널·Virus/WBC/Fragment/Collider 직렬화 배열 연결
- missing script `0`

빌드 메서드가 내부 Rebuild를 한 뒤 같은 Unity API 검사를 다시 실행했고 동일하게 통과했다.

### 씬 직렬화 결정성 경계

동일 빌더 반복 실행의 씬 byte SHA-256은 일치하지 않았다.

- 1차: `10FB159F6C32497F3840DFE3039561468A67BC9A010111656F0BCAEC670995F5`
- 2차: `7C1D3F3F949903D4AE1212CBF20AA03A642639E03F45639E13E0D9D58D190F4E`
- 3차: `57659BCEF251EE6EFA23CB94996C3BD5EDE977E9099BF910ECD5984EEC6B9C2B`

YAML diff는 Unity local fileID 재할당과 문서 배치 변경으로 전체가 재정렬되는 형태였다. Stage2 수용 기준은 byte-identical YAML이 아니라 논리 계층·참조·상태 계약이며, Unity API 계약 검사는 Rebuild 전후 두 번 동일하게 통과했다. byte-level 비결정성은 향후 diff 안정성 위험으로 남긴다.

## Windows 임시 빌드

실행 메서드:

```text
LastHost.Prototype.RatHost2D.Editor
  .RatHost2DPrototypeSceneBuilder.BuildWindowsTemporary
```

결과:

- Unity batchmode return code `0`
- BuildReport result: `Succeeded`
- BuildReport total size: `204,848,539 bytes`
- 실제 빌드 폴더: `205,441,545 bytes`, `320 files`
- 실행 파일:
  - `C:\tmp\LastHostRatHost2DStage2\20260728-065520\LastHostRatHost2DStage2.exe`
  - 크기 `667,648 bytes`
  - SHA-256 `098A43C3B20762E4BDF938771C36F0FB116126AEC8932B2A77EB403F0CB77938`

실행본 자체 플레이는 하지 않았다. 빌드 성공 증거를 기록한 뒤, 2026-07-28 사용자 요청에 따라 `C:\tmp\LastHostRatHost2DStage2` 전체를 삭제했다. 이후 사용자 수동 플레이가 필요하면 원본 검증 뒤 새 임시 빌드를 생성한다.

## 보호 diff

임시 복제본 빌드 전후 SHA-256 동일:

- `DefaultVolumeProfile.asset`
- `PC_RPAsset.asset`
- `UniversalRenderPipelineGlobalSettings.asset`
- `ProjectSettings.asset`
- `UnityConnectSettings.asset`
- `EditorBuildSettings.asset`

기준선과 빌드 후 SHA-256 동일:

- 기존 `RatHostPrototype.unity`
- 기존 `RatHost2DTechnicalSample.unity`
- `InputSystem_Actions.inputactions`
- `RatHostPrototypeControls.inputactions`
- `Packages/manifest.json`
- `Packages/packages-lock.json`

원본 repo 감사:

- 기존 3D 씬, TechnicalSample2D 씬·코드, 입력, Packages tracked diff `0`
- repo `UnityProject/Builds` 없음
- 원본 `ProjectSettings.asset`의 tracked diff는 기존 사용자 변경 한 줄만 유지:
  - `SENTIS_ANALYTICS_ENABLED;APP_UI_EDITOR_ONLY`
- `_workspace/previews/`는 사용자 untracked 경계로 유지

## 2026-07-28 원본 Unity/MCP 차단 이력

- 당시 원본 Unity PID `42724`에는 외부 씬 Reload 모달이 남아 있었다.
- 지시대로 Reload, Ignore, 강제 종료, 원본 씬 저장 우회를 실행하지 않았다.
- 당시 원본 `RatHost2DPrototype.unity`는:
  - `InternalVirusMode2D` 없음
  - `InternalVirusShell2D` 있음
  - 즉 Stage1 씬 유지
- 당시 원본 MCP Play와 Unity Console Error/Warning 확인은 수행하지 못했다.

## 2026-07-29 원본 씬 재검증

- 활성 씬: `RatHost2DPrototype`, loaded `true`, dirty `false`
- Tilemap:
  - Floor `117`, `(-6,-4)..(6,4)`
  - Water `5`, `(3,-2)..(3,2)`
  - Blocking `40`, `(-6,-4)..(6,4)`
- Host 캡처:
  - 13×9 바닥·외곽 벽·수로·오염 구역·쥐·소품 식별
  - black-only 증상 해소
- 카메라:
  - Main Camera → `RatHost2D`, 중심 오차 0
  - Internal Camera → `Virus2D`, 중심 오차 0
- Play:
  - WBC 접촉 3회 → `VirusFailed`
  - 확인 입력 대체 API → `RatHost`
  - 재진입 count `2`
  - 조각 3개 → `MutationSelection`
  - 각 단계 Host/Internal root·HUD·카메라 활성 배타 통과
- Physics2D 질의:
  - Host 네 방향 외곽 `BlockingTilemap` 검출
  - 수로 `WaterTilemap` 검출
  - Internal 네 방향에서 4벽 각각 검출
- missing script `0`
- 최종 Console Error `0`, Warning `0`
- 보호 diff 통과
- Windows 빌드 미실행

## 남은 위험

- MCP는 실제 OS 키보드 키다운을 주입하지 못한다. 사용자가 실제 이동과
  검은 화면 해소는 확인했으며, Space 실패 확인과 Internal 화면 체감이 남았다.
- 현재 Host와 내부 화면은 기술 플레이스홀더이며 최종 아트 수용이 아니다.
- 성공 뒤 실제 변이 선택·효과·쥐 복귀는 Stage3 범위다.
- 반복 Rebuild의 byte-level YAML/local fileID 안정성이 없다.

## 완료 판단 근거

기존 신규 `10/10`, 전체 `186/186`, Windows 빌드 성공 기록에 더해
원본 씬의 Tilemap 영속화, 카메라 캡처, Physics2D 충돌 질의,
실패·복귀·재진입·성공 Play 경로와 최종 Console을 독립 확인했다.
원본 씬 표시와 Stage2 런타임 기술 게이트는 통과다.
사용자에게는 Space 실패 확인과 Internal 화면 가독성 확인만 남긴다.
