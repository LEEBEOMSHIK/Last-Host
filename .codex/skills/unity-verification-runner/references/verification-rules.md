# Unity 검증 규칙

## 완료 주장 전 확인

- 실제로 실행한 검증만 통과했다고 말한다.
- 빌드하지 않았으면 빌드 성공이라고 말하지 않는다.
- 플레이하지 않았으면 조작감이 좋다고 단정하지 않는다.
- 실패나 미검증 항목은 최종 보고에 남긴다.
- 실행 순서와 무효화 기준은 `docs/agents/loop-engineering-gates.md`를 유일 기준으로 따른다.
- 구현 전 사용자 원증상·합성 oracle·경계·negative control을 S0 charter에 잠근다.
- 원인 레이어를 고치지 않고 renderer/object disable, alpha 0, teleport·clamp, 입력 잠금, error swallow, 과대 invisible collider 또는 hidden-output 기대 테스트로 증상만 가린 후보는 FAIL한다.
- negative control은 플레이어 active/enabled/alpha, root·visual transform과 정상 입력 경로가 보존되는지 확인한다.
- collision은 S0의 가시 footprint tolerance를 확인하고, 사용자 가시 oracle과 함께 통과해야 한다.
- workaround는 사용자 명시 승인·임시 표시·제거 조건이 있을 때만 허용하며 `temporary` 또는 `blocked`로 판정한다.
- 첫 blocker에서 full suite·전체 matrix·다량 캡처를 중지한다.
- production·테스트·하네스 변경 뒤 영향받는 이전 PASS는 `SUPERSEDED`이며 현재 판정에 합산하지 않는다.
- 전체 suite와 대형 matrix는 freeze된 최종 후보에서 필요한 경우 각각 한 번만 실행한다.
- 증거는 같은 `candidate_fingerprint`, `run_id`, Unity lease owner를 가져야 한다.
- 최종 캡처는 대상 root 단일성, 임시 QA 객체 0, scene dirty 전후를 확인한 원자적 증거만 사용한다.
- criterion마다 canonical evidence 1개를 기본으로 하고 중복 산출물을 만들지 않는다.
- Unity/MCP/build 같은 고비용 실행은 `tools/verification/Invoke-HighCostVerification.ps1` preflight를 통과해야 한다.
- capability unavailable, 연속 실패 2회, forbidden QA harness, stale component contract, packet-only/current-state 불일치면 실제 실행 전에 FAIL한다. current-state는 profile 허용 status와 route 기대 status를 모두 만족해야 하며 실행 상태 전이도 profile 계약을 따른다.
- `Invoke-UnityEditModeTests.ps1` Run을 직접 호출하지 않는다. wrapper one-shot token 없는 호환 경로는 `-ValidateResultsOnly`뿐이다.

## 쥐 숙주 프로토타입 수용 기준

- 쥐가 의도대로 이동한다.
- 면역 경계도가 오르는 이유가 보인다.
- 면역 경계도 100%에서 내부 미니게임으로 전환된다.
- 미니게임 목표가 이해된다.
- 변이 선택 후 쥐 모드에 변화가 적용된다.

## 검증 결과 형식

검증 결과는 명령, 결과, 해석을 분리해서 쓴다.

```text
명령:
결과:
해석:
```

완료 판단은 `기술 검증 통과`, `기술 검증 통과 — 사용자 수용 대기`, `완료 불가`로 구분한다. 자동화할 수 없는 핵심 입력·화면 수용이 남았으면 `완료`라고 쓰지 않는다.
