# 독립 QA r1

## 판정

`FAIL — 제품 결함 확정이 아니라 독립 Play 하네스의 Y-sort 검증 오류와 C3·C4·실제 WASD 증거 미완료로 기술 게이트 종결 불가`

- 중단 사유: first blocker 발생 뒤 fail-fast했으며, 조정자가 장기 실행 비용 상한을 적용해 추가 검증·재시도 금지를 지시했다. 이후에는 상태 복원과 기록만 수행했다.

- freeze candidate: `sha256:cd6946deff7ecf1e1f4e4aed6c2fd532f1a97c5e895bb79de6fe00b4bee49385`
- QA run: `natural-occlusion-qa-r1-20260802`
- Unity lease: PID `54432`, 획득 `2026-08-02T04:58:45.4275339+00:00`, 해제 `2026-08-02T05:04:16.2872543+00:00`
- Play session: 1회 시작·1회 종료, 재시도 0
- capture: 0/4, Console 조회 1회(Error 0)
- final editor: Play false, Pause false, `RatHost2DTechnicalSample`, dirty false, root 1

## 정적·XML 재검증

| 항목 | 결과 | 근거 |
| --- | --- | --- |
| candidate manifest | PASS | 현재 파일 mismatch 0, fingerprint가 freeze candidate와 일치 |
| gameplay targeted | PASS | 공식 XML `3/3`, failed/skipped/inconclusive 0, strict `valid_pass=true` |
| scene targeted r1 | historical FAIL | 공식 XML `7/8`, initial rat↔barrel overlap. r2가 대체 |
| scene targeted r2 | PASS | 공식 XML `8/8`, failed/skipped/inconclusive 0, strict `valid_pass=true` |
| symptom-masking policy | PASS | 중앙 규칙 1개와 역할별 최소 요약, temporary/blocked, negative control, 사용자 oracle이 연결됨 |
| legacy 보호 | PASS | `RatHostPrototype.unity`, `PrototypeCameraController.cs`, `PrototypeKeyboardInput.cs` diff 0 |

## 단일 Play reduced matrix

MCP에 키보드 이벤트 주입 도구가 없어 `Host/Move` InputAction의 존재·활성화를 확인한 뒤 동일 `RatHost2DController.CacheMoveInput → SimulateFixedStep` 경로로 대표 연속 입력을 시뮬레이션했다. 이 결과는 실제 사용자 WASD를 대체하지 않는다.

- 방향·프레임 전환 16회: renderer enabled/alpha 1, root/visual active, visual local position·root scale 불변, capsule `(1.2265625, 0.25)`, mirrored X offset `±0.28515625`, Y `0.125` 유지.
- 대표 접근 3개 모두 non-overlap/허용 gap으로 정지:
  - wall: `0.080px`
  - barrel: `0.003px`
  - crate: `0.080px`
- 접근 전 과정에서 renderer enabled/alpha 1과 stable footprint 유지.
- 첫 blocker: Y-sort 앞/뒤 확인에서 Rigidbody 위치만 변경하고 정렬 기준 Transform을 갱신하지 않아 `frontRat=0`, `object=76`으로 하네스가 실패했다. 이는 제품 Y-sort 결함 증거가 아니며, fail-fast 계약에 따라 같은 Play session에서 수정·재시도·capture를 하지 않았다.

## C1~C7 판정

| criterion | 판정 | 설명 |
| --- | --- | --- |
| C1 | PASS | lifecycle XML과 3개 오브젝트 접근 중 active/enabled/alpha 불변, hide transition 0 |
| C2 | PASS | exact polygon/support scene r2 8/8와 대표 Play gap 3종 모두 허용 범위, overlap false |
| C3 | BLOCKED | 앞/뒤 Y-sort Play 판정 하네스 오류, 원자 capture 0. 자연 부분 가림 육안 증거 없음 |
| C4 | NOT RUN | direction/frame 전환 16회는 통과했지만 각 대표 경계 왕복 10회는 실행하지 않음 |
| C5 | PASS | external disabled renderer 보존, lifecycle·scene 계약 XML PASS |
| C6 | BLOCKED | controller-path 연속 입력·collision·Console 0·scene clean은 확인. MCP 키 주입 부재로 실제 WASD 및 Y-sort/가림 연속 확인은 사용자 대기 |
| C7 | PASS | 보호 대상 3D legacy scene/camera/V-toggle diff 0 |

## 정책·하네스 분석

- 정책 문서 자체는 이번 수정의 핵심 금지 패턴을 충분히 명시한다. renderer/object disable, alpha 0, 이동·입력 우회, 오류 숨김, 과대 invisible collider, hidden-output 기대 테스트는 완료 수정으로 인정되지 않는다.
- 실행 하네스는 아직 criterion 완결성을 자동 보장하지 않는다. 공식 11개 targeted 테스트는 C1·C2·C5·C7의 상당 부분을 다루지만 C3의 실제 앞/경계/뒤 시각 원자성, C4의 오브젝트별 10회 왕복, C6의 실제 InputAction 입력을 한 번에 막는 canonical gate가 없다.
- `scene r1 7/8 FAIL → r2 8/8 PASS`는 독립 테스트가 실제 초기 barrel 침투를 잡은 좋은 사례다. 반면 Play 하네스가 정렬 기준 Transform 대신 Rigidbody 위치만 바꾼 이번 실패는 검증 코드 자체의 사전 self-check가 부족하다는 사례다.
- 따라서 에이전트 수가 많아도 criterion→단일 canonical evidence 매핑과 사용자 oracle이 비어 있으면 여러 차례 수정이 발생할 수 있다. 역할 수보다 freeze candidate, 한 번의 reduced matrix, 실패 가능한 negative control, 증거 완결성 체크가 중요하다.

## 필요한 최소 보완

1. Play reduced matrix를 저장소의 재사용 가능한 읽기 전용 검증 명령/테스트로 승격하고, Rigidbody 위치 변경 뒤 Transform·footpoint 실제 좌표가 바뀌었는지 self-check한 다음 Y-sort를 판정한다.
2. wall/barrel/crate 각각 `앞→접촉→뒤→복귀` 10회에서 renderer/alpha/root, signed gap, sorting order crossing, pop/jitter를 한 행으로 기록한다.
3. 실제 키 이벤트를 주입할 수 없으면 C6를 자동 PASS하지 말고 `controller-path PASS + 사용자 WASD pending`으로 고정한다.
4. 커밋 게이트가 C1~C7 중 `BLOCKED/NOT RUN`을 자동 집계해 하나라도 있으면 완료·커밋 보고를 차단하도록 한다.
5. capture는 Y-sort self-check와 candidate fingerprint 확인 뒤에만 최대 4장 생성해 실패 실행의 stale 시각 증거를 남기지 않는다.

## 비용 대조

- Unity: lease 1회, Play 1회, reduced matrix 1회(첫 blocker에서 중지), Console 1회, capture 0, build/full suite 0.
- XML: 기존 공식 결과 3개 strict 재판정만 수행, 실제 테스트 재실행 0.
- 재시도·보정 실행: 0. lease acquire 명령은 bool 인자 바인딩 실패 2회 후 실제 획득 1회였으며 앞선 2회는 Unity 상태를 변경하지 않았다.
- 판정: 이번 독립 QA 자체는 비용 예산 안이다. 다만 C3·C4·C6를 메우는 canonical harness가 없어 후속 수동 요구가 다시 발생할 위험이 남는다.
- 비용 상한 중단 이후 추가 Unity/MCP 검증 0회. 최종 상태는 Play false, Pause false, scene dirty false, Console Error 0, capture 0, lease Released다.
