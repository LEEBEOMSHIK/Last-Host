# 작업 로그: 쥐 본능 정지 리듬과 면역 신호 억제 테스트 진입

## 2026-07-08

- 사용자 요청을 세 가지로 분리했다: 쥐 자동 이동 체감, 조종력 보상 후 강제 조종 경계도 기준, 면역 신호 억제 테스트 진입.
- 원인 판단: 현재 쥐 본능 배회는 방향 전환은 있지만 이동/정지 리듬이 없어 계속 걷는 느낌이 난다.
- 조종력 기준: 현재 모델은 `RatControlPower >= RatHostInstinctResistance`이면 자유 조작에 가까워지고 `IsForcedControl=false`가 된다. 이는 사용자가 앞서 말한 방향과 일치한다.
- 테스트 진입: 기본 내부 미니게임 선택은 바꾸지 않고 `F6` 개발 키를 추가한다.
- 구현: 본능 이동/정지 리듬을 추가하고, 정지 중 무입력일 때는 이동 벡터를 0으로 돌린다. 정지 중에도 플레이어 입력은 처리된다.
- 구현: `F6` 입력을 `PrototypeKeyboardInput`에 추가하고, 쥐 숙주 모드에서 `EnterImmuneSignalSuppressionMinigame()`으로 진입하게 했다.
- 씬: `RatHostPrototype.unity`에 본능 방향 랜덤화, 본능 정지, `F6` 디버그 진입 기본값을 명시했다.
- 검증: Unity MCP `Unity_RunCommand` 명시 호출로 관련 테스트 13개가 통과했다.
- 검증: `git diff --check` 종료 코드 0. CRLF 변환 경고만 확인했다.
- 제한: Unity MCP Play 진입은 성공했지만, Stop/상태/Console 확인 시점에 `Connection revoked`가 발생했다. Console Error/Warning 최종 확인은 MCP 승인 복구 후 필요하다.
