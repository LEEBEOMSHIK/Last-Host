# 검증 기록

## 판정

`차단 — 자동 테스트 통과, 원본 Unity MCP Play·QA Windows 빌드 미완료`

사용자 수동 플레이 전 완료 경계와 별개로, 현재 Unity 편집기를 막고 있는 외부 씬 변경 모달 때문에 독립 QA의 MCP Play와 최신 Windows 임시 빌드 검증을 끝내지 못했다. 따라서 이 작업을 완료·보관 가능으로 판정하지 않는다.

## 통과한 항목

- Unity 버전: `6000.4.6f1`
- 전체 EditMode 독립 실행:
  - `176 / 176 Passed`
  - failed `0`
  - skipped `0`
  - inconclusive `0`
  - 실제 테스트 duration `24.8450647s`
- 전체 결과 XML SHA-256:
  - `AD57FE73EB941DD3817DDB4A5633DA80239C2EE1BAFFD852D49561B1D4735D73`
- 신규 `RatHost2D` 단독 결과:
  - 메인 조정자가 보존된 `TestResults.xml`을 읽기 전용 대조해 `37 / 37 Passed` 확인
  - 전체 `176 / 176`에도 해당 테스트가 포함됨
- 테스트 실행 전 원본 활성 씬:
  - `RatHost2DPrototype`
  - `Assets/_Project/Scenes/RatHost2DPrototype.unity`
  - `isDirty=false`
- 원본 Unity Console을 비운 뒤 Stage 1 `Rebuild Scene` 메뉴 실행 성공
- 테스트용 임시 복제본:
  - 시작 시 원본 `Library`, `Temp`, `Logs`, `Builds`, `UserSettings`, `obj`를 복사하지 않음
  - 정확한 경로: `C:\tmp\LastHostQAProject-20260728-1`
  - Unity가 자체 생성한 `Library` 포함 최대 확인 점유: `2.703 GiB`
  - 테스트 종료 후 해당 경로만 제거, `Exists=False`
  - C: 여유 공간 확인값: `11.78 GiB` → 제거 후 `14.56 GiB`

## 차단된 항목

- Stage 1 씬 재생성 직후 Unity가 `The open scene(s) have been modified externally` 모달을 표시했다.
- 모달의 `Reload`와 `Ignore`는 Computer Use 접근성 트리에 표시됐으나, 창 캡처 계층 오류로 요소 클릭 캐시가 만들어지지 않았다.
- 키보드 `Return`, `Alt+R`, `Tab` 후 `Space`, `Escape`도 모달에 전달되지 않았다.
- `CloseMainWindow()` 정상 종료 요청은 `False`를 반환했다.
- 강제 종료·재시작은 미저장 데이터 손실 위험 때문에 사용자 명시 승인 없이 실행할 수 없다는 승인 판정을 받았다.

이 차단 때문에 다음 독립 QA는 미완료다.

- 원본 씬 `GetActive.isDirty=false` 재확인
- 전체 계층과 단일 세션·입력·카메라·HUD·Collider 실제 연결 확인
- MCP Play의 무입력 본능 이동
- 실제 `Host/Move` WASD 인계, 현재 위치 기준 이동, 순간이동·누적 분리 없음
- root·Visual·카메라 중심 정합
- 벽·Pipe·Barrel 비관통
- 오염 구역 밖 변화 `0`, 안 `+12/초`, `-4/초`, 원인 피드백
- 자연 100%에서 `InternalVirus / WhiteBloodCellEvasion` 단일 전환
- 전환 후 Host 입력·이동·Collider·HUD 비활성, 내부 셸 활성, 추가 상태 동결
- Play 종료 후 Console Error/Warning `0`
- 보완된 보호 스냅샷을 사용한 QA 최신 Windows 임시 빌드
- 빌드 전후 보호 파일·Build Settings·저장소 `Builds/` 최종 대조

## 보호 범위 중간 대조

- `UnityProject/Packages/manifest.json`, `packages-lock.json`: tracked diff 없음
- 기존 `RatHostPrototype.unity`, `RatHost2DTechnicalSample.unity`: tracked diff 없음
- `InputSystem_Actions.inputactions`, `RatHostPrototypeControls.inputactions`: tracked diff 없음
- 저장소 `UnityProject/Builds`: 존재하지 않음
- `UnityProject/ProjectSettings/ProjectSettings.asset`의 유일 tracked diff는 기존 사용자 로컬 변경:
  - `SENTIS_ANALYTICS_ENABLED;APP_UI_EDITOR_ONLY`
- `_workspace/previews/`: untracked 사용자 경계로 유지

단, Stage 1 QA 빌드를 실행하지 못했으므로 위 보호 범위의 빌드 후 재대조는 아직 완료 근거가 아니다.

## 재개 조건

다음 중 하나가 필요하다.

1. 사용자가 Unity 모달에서 `Reload`를 직접 클릭한다.
2. 미저장 데이터 손실 위험을 인지한 사용자가 PID `42724` Unity 강제 종료·재시작을 명시 승인한다.

모달이 닫히면 같은 작업 패킷에서 MCP Play, Console, 최신 `C:\tmp` Windows 빌드와 보호 diff를 이어서 검증한다.
