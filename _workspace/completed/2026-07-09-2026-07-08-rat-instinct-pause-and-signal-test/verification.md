# 검증 기록: 쥐 본능 정지 리듬과 면역 신호 억제 테스트 진입

## 검증 대상

- 쥐 자동 배회가 계속 걷기만 하지 않고 짧게 멈출 수 있는지
- 낮은 조종력의 강제 조종 디메리트와 높은 조종력의 자유 조종 기준이 유지되는지
- 면역 신호 억제 미니게임을 기본 진입과 분리해 `F6`으로 테스트할 수 있는지

## 실행한 검증

```text
검증 방법:
Unity MCP Unity_RunCommand에서 RatHostPrototypeCoreTests 관련 테스트 13개 명시 호출

결과:
targeted-tests passed=13 total=13
isCompilationSuccessful=true
isExecutionSuccessful=true

포함 테스트:
- MutationLoadout_AppliesPrototypeEffects
- Session_DefaultInternalMinigameTypeRemainsWhiteBloodCellEvasion
- Session_ImmuneSignalSuppressionSuccessMovesToMutationSelection
- Session_ImmuneSignalSuppressionFailureReturnsWithoutMutationReward
- RatHostControlModel_NoInputFollowsHostInstinctAtPassiveSpeed
- RatHostControlModel_NoInputStopsWhenHostInstinctIsPaused
- RatHostControlModel_PlayerInputStillWorksWhileHostInstinctIsPaused
- RatHostControlModel_LowControlBlendsPlayerInputIntoInstinct
- RatHostControlModel_HighControlOverridesHostInstinct
- RatHostControlModel_LowControlAgainstInstinctAppliesDemerit
- RatHostControlModel_HighControlAgainstInstinctDoesNotApplyDemerit
- RatHostInstinctWander_PeriodicTurnChangesInstinctDirection
- PrototypeKeyboardInput_MapsDebugSignalSuppressionKey
```

```text
검증 방법:
git diff --check

결과:
종료 코드 0
CRLF 변환 경고만 표시됨
```

```text
검증 방법:
2026-07-09 커밋 전 Unity Roslyn csc.dll 런타임/테스트 어셈블리 fresh 컴파일

결과:
런타임 어셈블리 종료 코드 0
테스트 어셈블리 종료 코드 0

해석:
쥐 본능 정지 리듬, F6 테스트 진입, 면역 신호 억제 HUD 추가분을 포함한 현재 코드가 컴파일된다.
```

```text
검증 방법:
사용자 수동 플레이 확인

결과:
쥐 자동 움직임, 조종력 보상 후 강제 조종 디메리트 변화, 면역 신호 억제 진입/시각화 확인이 진행됐다.
```

```text
검증 방법:
Unity MCP Play 스모크

결과:
- Unity_ManageEditor GetState 성공: Play 아님, Compile 아님
- Unity_ReadConsole Clear 성공
- Unity_ManageEditor Play 성공
- Unity_ManageEditor Stop 실패: Connection revoked
- 이후 GetState/Console 조회도 Connection revoked
```

## 검증하지 못한 항목

- Play 진입 후 Stop, Console Error/Warning 최종 조회는 MCP 연결 해제로 완료하지 못했다.
- 실제 키보드 입력으로 쥐가 멈춤/배회 리듬을 충분히 쥐처럼 느끼는지는 사용자 수동 확인이 필요하다.
- 실제 `F6` 키 입력으로 면역 신호 억제 미니게임 화면에 들어가는 장면은 MCP 연결 해제로 시각 확인하지 못했다. 입력 매핑과 세션 진입 코드는 테스트로 확인했다.
- Windows 빌드는 실행하지 않았다.

## 남은 위험

- Unity 에디터가 Play 상태로 남아 있을 수 있다. MCP 승인이 끊겨 현재 세션에서 Stop 확인이 불가능했다.
- 쥐 움직임의 동물다운 체감은 수치 문제일 수 있어, 사용자 확인 후 `instinctMoveDurationRange`, `instinctPauseDurationRange`, `instinctTurnIntervalRange` 조정이 필요할 수 있다.

## 완료 판단

사용자 수동 확인과 fresh 컴파일 검증 기준으로 커밋 가능. 다만 MCP 승인 복구 후 Stop/Console 확인을 다시 수행하는 편이 안전하다.
