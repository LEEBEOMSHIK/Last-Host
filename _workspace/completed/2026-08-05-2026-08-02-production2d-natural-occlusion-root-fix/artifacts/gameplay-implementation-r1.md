# Gameplay runtime 후보 구현 보고 r1

## 후보 식별

- S0 revision: `natural-occlusion-s0-r4-footprint-contract`
- S0 scope fingerprint: `b09aa2ece964e9764af6bda98f56cbb6f7a3158887b03f06b6872f36d802a0f8`
- run_id: `natural-occlusion-gameplay-r1-20260802`
- candidate fingerprint: `7cefea1d56632fd633a15d2574a0f56167607411a89ceb329f1c447bd037ca25`
- manifest: `gameplay-candidate-manifest-r1.json`
- 후보 상태: 구현·S1 정적 검사와 격리 복제본 공식 EditMode targeted `3/3 PASS`; scene owner 인계 가능

## 변경 파일과 불변식

| 파일 | 변경 | 고정 계약 |
| --- | --- | --- |
| `VisualOcclusionResolver2D.cs` | whole-character hide와 stale deserialize force-enable 제거, passive compatibility 동작 | resolver는 renderer enabled/color, root active, transform, 입력을 변경하지 않음; `ResolveNow=false`, hide state false, transition 0 |
| `RatSide3FrameView.cs` | 캡슐 수치를 runtime에서 고정 | horizontal, size `(1.2265625,0.25)`, right `(0.28515625,0.125)`, left `(-0.28515625,0.125)`, 프레임 resize 없음 |
| `NaturalOcclusionGameplayContractTests.cs`·meta | gameplay 전용 표적 테스트 3개 | enabled/alpha/active/root/lifecycle, external disabled, 3-frame/mirror capsule |

기존 `ConfigureBodyClearance` 시그니처와 resolver 공개/정적 API는 scene owner의 순차 이관 전 컴파일 호환을 위해 유지했다. caller가 과거 수치 `(1.28,0.26)` 등을 넘겨도 runtime 수치는 위 측정 계약으로 정규화된다.

## 실행한 검증

1. `git diff --check`와 resolver 금지 write 정적 검색: PASS. production resolver의 `characterRenderer.enabled/color`, `SetActive`, transform/input write 0.
2. Unity `ValidateScript standard` 3개 파일: compile/syntax error 0. `RatSide3FrameView`에 실제 문자열 연결이 없지만 일반 `Update()` 휴리스틱 warning 1건.
3. Unity Console Error 조회: 0.
4. targeted TestRunner 요청 1회: FAIL/BLOCKED — `User interactions are not supported for MCP tool calls. Tools requiring user interaction cannot be called via MCP.`
5. 위 MCP no-result 뒤 같은 후보를 별도 project key의 격리 복제본에서 공식 EditMode로 1회 실행: `gameplay-targeted-r1.xml`, `Passed`, total/pass `3/3`, failed/skipped/inconclusive `0`, Unity exit `0`.
6. 저장소 결과 판정 도구의 `ValidateResultsOnly`: `valid_pass=true`.

첫 MCP 요청은 결과 없는 실행으로 보존하고 공식 격리 실행이 gameplay targeted 정본을 대체한다. 기존 scene test, full suite, MCP Play, matrix, capture, build는 실행하지 않았다.

## Unity lease와 복원

- agent/work/run: `gameplay_implementation` / `2026-08-02-production2d-natural-occlusion-root-fix` / `natural-occlusion-gameplay-r1-20260802`
- editor PID: `54432`
- baseline/final: Play `false`, Pause `false`, scene `Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`, dirty `false`
- 임시 객체·씬 변경: 없음
- release: `2026-08-02T04:15:23.1899848Z` 완료

## 비용 proxy

| 항목 | 실제 |
| --- | ---: |
| 구현 역할 | 1 |
| fingerprint manifest | 1 |
| lease acquire/release | 1/1 |
| Unity script validation | 3 |
| Console read | 1 |
| targeted TestRunner 요청 | 1 no-result |
| TestRunner 실제 시작/XML | 1/1 PASS (`gameplay-targeted-r1.xml`) |
| correction | 0 |
| full suite/MCP Play/matrix/capture/build | 0 |
| exact token/$ | 미집계 |

## Scene owner 인계값과 차단 상태

- rat collider: `CapsuleCollider2D`, horizontal, size `(1.2265625,0.25)`
- right offset: `(0.28515625,0.125)`
- left offset: `(-0.28515625,0.125)`; X 부호만 mirror
- frame behavior: neutral/contact/passing에서 size·Y offset 변경 금지
- root/footpoint: root position과 visual local position 변경 금지
- visibility: resolver가 renderer enabled/alpha, root active를 변경하지 않음; 외부 disabled는 disabled 유지
- resolver scene wiring: passive 호환 상태이며 scene owner가 자연 부분 가림 구조로 이관하면서 제거 가능
- 기존 저장 씬에는 과거 workaround가 남긴 `SpriteRenderer.enabled=false`가 있을 수 있다. runtime이 이를 강제 복원하면 외부 소유권을 다시 침범하므로, scene owner가 builder와 serialized scene에서 쥐 renderer 초기값을 명시적으로 `true`로 교정하고 씬 계약 테스트로 확인해야 한다.
- **handoff 판정:** Unity lease와 gameplay 편집 소유권은 release했고 공식 targeted `3/3 PASS`를 확보했다. scene owner는 위 수치·visibility 계약을 변경하지 않는 조건으로 builder/scene acquire·적용을 시작할 수 있다.
