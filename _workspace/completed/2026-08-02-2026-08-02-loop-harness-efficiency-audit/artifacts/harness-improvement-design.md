# Unity 검증 하네스 독립 감사와 개선 설계

## 감사 범위

- `VisualOcclusionResolver2D.cs`, `RatSide3FrameView.cs`
- `Production2DV1AssetAndSceneTests.cs`
- 구현 보고, 런타임 리뷰, 독립 QA 보고, `verification.md`, 총괄 판정
- `qa-*-pre-fix/post-fix/final/final-v2` CSV·XML·로그·캡처
- `$unity-verification-runner`, QA 역할 문서, 루프 게이트, `_workspace/templates/verification.md`

## 결론

현재 루프는 **독립 역할·기록·최종 차단 게이트는 작동했지만, 빠른 결함 차단 하네스로서는 미흡했다.** 독립 QA가 실제 결함을 찾아냈고 잘못된 캡처까지 폐기했으므로 형식만 있는 루프는 아니다. 반면 QA가 구현 후반에 처음 테스트 설계자로 투입됐고, 변경 후보를 고정하거나 증거와 소스 상태를 결합하는 장치가 없었다. 그 결과 싸게 잡을 수 있었던 상태 전이 결함을 전체 EditMode와 MCP 매트릭스 뒤에서 순차 발견하고 같은 검증을 여러 번 반복했다.

이번 작업의 난도는 단순 collider 수치 조정 이상이었다. 단일 긴 스프라이트, 방향별 core bounds, Y sorting, 물리 footprint, whole-character hide, renderer 소유권, 씬 직렬화, 다중 오클루더, hysteresis가 결합됐다. 따라서 런타임 코드와 테스트가 필요한 작업 자체는 타당했다. 그러나 `200/200` 구현자 PASS 이후 독립 QA에서 직렬화 복원, X축 hysteresis, 수정에 따른 wall 회귀가 연속 발견되고 캡처도 v2로 다시 만들어진 비용은 **결함 난도보다 하네스와 순서의 부족 때문에 커졌다.**

에이전트 수가 적어서 놓친 문제가 아니다. 실제로 런타임 리뷰는 첫 QA 전에 tail-only, 다중 오클루더 상태 누수, 외부 disabled renderer 침범을 잡았다. 문제는 역할이 많아도 각 역할의 검증 차원이 명시적으로 분할되지 않았고, QA가 구현 전에 실패 테스트 목록을 잠그지 않았다는 것이다. `독립 QA`라는 이름은 독립성을 보장하지만 자동으로 완전한 범위와 조기 발견을 보장하지 않는다.

## 실제 실패 연쇄와 조기 차단 가능성

| 순서 | 발견된 문제 | 왜 늦게 발견됐는가 | 첫 QA 전에 필요한 최소 차단 |
| --- | --- | --- | --- |
| 런타임 리뷰 | 양쪽 fragment만 검사해 한쪽 꼬리 조각 누락 | 최초 알고리즘 테스트가 대표 중앙 분할 한 가지에 치우침 | 좌·우 flip × 양쪽/한쪽 fragment 표 기반 순수 함수 테스트 |
| 런타임 리뷰 | 활성 hysteresis가 다른 오클루더로 누수 | 단일 오클루더 예제만 검증 | A에서 hide → B로 이동 → B는 4px entry부터 시작하는 상태 전이 테스트 |
| 런타임 리뷰 | 외부 disabled renderer를 강제로 복원 | `enabled`를 출력 상태로만 보고 소유권 상태로 보지 않음 | 외부 disabled 전·중·후, resolver disable/enable까지 포함한 소유권 테스트 |
| QA 1차 | 씬 저장 시 `enabled=false`는 직렬화되지만 resolver의 비직렬화 소유 상태는 소실 | in-memory `Configure` 테스트는 있었으나 save→unload→reload 경계 없음 | 실제 임시 씬 직렬화 round-trip 테스트 |
| QA 2차 | Barrel의 해제가 X축 core 교차로 결정돼 Y축 hysteresis가 무효 | wall 대표 경로만 수치 테스트 | X-/X+/Y-/Y+ core 경계와 좌우 fragment 경계를 분리한 축별 매개변수 테스트 |
| QA 3차 | occluder X 확장 보정이 wall fragment 폭을 줄여 기존 `+0.37` hold 회귀 | 수정된 조건만 양성 검증하고 기존 경계 기준 전체 재실행 전 QA에 인계 | 변경 즉시 관련 경계 테이블 전체를 먼저 실행하고 이전 PASS를 폐기 |
| 최종 캡처 | CSV 상태와 PNG에 남은 `QA_Temp*`/실제 Rat 상태 불일치 | 상태 설정, HUD 갱신, 화면 캡처가 서로 다른 호출·시점 | 단일 호출 원자 캡처와 stale-object guard, PNG sidecar manifest |

현재 `qa-contact-matrix-*.csv`와 `qa-stability-*.csv`는 결과 수치는 풍부하지만 `run_id`, 입력 fingerprint, 에디터 PID, 씬 dependency hash, 명령, 생성 시각, 파일 checksum이 없다. XML은 시간과 PID를 포함하지만 작업 트리의 후보 fingerprint는 포함하지 않는다. `final.csv`에는 PASS가 기록됐는데 연결된 PNG는 실제 화면 상태와 불일치했고, 사후에 `final-v2`로 정정됐다. 즉 **수치 증거의 양은 많았지만 어떤 소스 후보와 같은 순간의 화면인지 기계적으로 보증하지 못했다.**

또한 `72` contact와 `432` phase의 전체 Cartesian 매트릭스는 최종 후보 확인에는 유용하지만 내부 상태 결함을 빨리 찾는 테스트는 아니다. 방향·프레임·단계의 많은 행은 동일한 분기 결과를 반복했다. 직렬화와 축별 hysteresis 같은 위험 차원을 먼저 10~20개의 작은 테스트로 실행했다면 전체 매트릭스 반복을 줄일 수 있었다.

## 첫 QA 전 필수 최소 테스트 세트

이 테스트들은 구현 담당이 작성하되, QA가 구현 시작 전에 이름·입력·기대값을 승인하고 실패 상태(red)를 확인한다. 독립 QA는 같은 테스트를 재실행하는 것에 더해 원래 증상을 별도 Play로 확인한다.

### 1. 직렬화 round-trip

`VisualOcclusionResolver_SceneRoundTrip_RestoresOwnedVisibilityOnly`

1. 임시 씬에 character renderer, resolver, occluder를 만든다.
2. 가림 상태를 만든 뒤 씬 저장 → 씬 닫기 → 다시 연다.
3. resolver의 비직렬화 필드가 초기화된 실제 경계에서 첫 판정을 수행한다.
4. behind에서는 hidden, release 위치에서는 renderer enabled가 복구되는지 확인한다.
5. 명시적 runtime `Configure`로 외부에서 disabled한 renderer는 복구하지 않는 별도 케이스를 실행한다.
6. 임시 씬을 정리하고 원래 씬과 dirty 상태가 변하지 않았는지 확인한다.

가능하면 builder가 transient hide 결과를 씬에 저장하지 않도록 바꾸고, 저장 씬의 전용 character renderer는 기본 `enabled=true`를 계약으로 두는 편이 더 단순하다. 현재 테스트는 저장 씬의 `enabled=false`를 예상값으로 고정한 뒤 런타임 첫 판정에서 정상화한다. 이 설계는 작동하더라도 `직렬화된 외부 disabled`와 `stale resolver hide`를 값 하나로 구분할 수 없어 소유권이 모호하다. 장기적으로는 **transient runtime hide 비직렬화**가 우선 권고다.

### 2. 모든 경계 축 hysteresis

`WouldRemainOccludedDuringRelease_AllBoundaryAxes`

- 경계 종류를 `core overlap X-/X+/Y-/Y+`, `fragment left/right`, `tail-only left/right`로 분리한다.
- 각 행에서 `entry 직전`, `4px entry`, `entry 후`, `2px release band 내부`, `release 경계`, `release 이후`를 검사한다.
- 좌우 flip을 모두 적용한다.
- entry 계산과 release 계산에 서로 다른 geometry를 쓰는 경우, 기존 wall 기준과 barrel 수평 기준을 같은 `[TestCaseSource]`에서 함께 실행한다.
- 부동소수점 임의 탐색 대신 PPU에서 `pixel / 128f`로 정확한 경계값을 생성하고 경계 양옆 epsilon만 추가한다.

필수 불변식은 다음과 같다.

- entry 영역은 release 영역의 부분집합이다.
- 동일 활성 오클루더에서 2px 이내 왕복은 상태 전환 0이다.
- 2px 밖으로 나가면 정확히 1회 release한다.
- hysteresis 보정이 visible fragment 폭이나 다른 오클루더의 entry 폭을 바꾸지 않는다.

### 3. 다중 오클루더 상태 전이

`Resolver_HysteresisBelongsOnlyToActiveOccluder`

- A 진입 → A hold → A 이탈과 동시에 B의 3px fragment: B는 hide하면 안 된다.
- A 진입 → A 이탈과 동시에 B의 4px fragment: B는 새 active가 돼야 한다.
- A가 disable/destroy/reorder된 경우 stale index를 사용하지 않는다.
- 배열 순서를 바꾼 동일 geometry에서도 결과가 같다.
- frame 또는 flip 변경으로 active 조건이 사라진 경우 정확히 한 번 복구한다.

### 4. renderer 외부 소유권

현재 `WholeCharacterOcclusionPreservesAnExternallyDisabledRenderer`는 빈 occluder 상태의 한 번 호출만 확인한다. 아래 전이까지 포함해야 한다.

- 외부 disabled → resolver visible 판정: 계속 disabled.
- 외부 enabled → resolver hide → release: enabled로 복구.
- resolver hide 중 외부 disabled 요청 → release: 외부 정책을 덮어쓰지 않음. 이를 지원하지 않는다면 API 계약상 금지하고 assertion/log로 드러낸다.
- resolver가 숨긴 상태에서 component disable/destroy: resolver가 소유한 상태만 복구.
- save/reload된 disabled가 resolver stale인지 외부 권한인지 구분되는지 확인.

이 부분은 boolean 하나를 직접 공유하는 현재 방식의 구조적 위험이다. 후속 구현에서는 `SpriteRenderer.enabled`를 여러 시스템이 직접 쓰지 않도록 visibility reason/owner를 모으는 작은 정책 계층 또는 전용 visual child 소유권을 명시해야 한다.

### 5. 원자 캡처 계약

캡처 한 건은 다음 작업을 **하나의 Editor 명령과 하나의 frame barrier**에서 수행해야 한다.

1. lease와 candidate fingerprint 확인
2. 대상 씬·대상 `GlobalObjectId` 및 기대 인스턴스 수 확인
3. 위치/프레임/flip 설정
4. physics sync, sorting, resolver, camera, HUD 갱신
5. 같은 Camera를 직접 render하여 PNG 생성
6. 같은 메모리 상태에서 sidecar JSON 생성
7. PNG·JSON checksum 계산 후 임시 파일을 최종 이름으로 atomic rename
8. 임시 오브젝트 정리, scene dirty/Play 상태 확인

sidecar 필수 필드:

```text
run_id, candidate_fingerprint, scene_path, scene_dependency_hash,
editor_pid, lease_owner, utc, camera_global_id,
subject_global_id, subject_instance_id, subject_count,
root_position, frame, flip, sorting_orders,
resolver_hidden, renderer_enabled, collider_distance, overlap,
hud_root, console_error_count, scene_dirty_before/after,
png_sha256, harness_version
```

`QA_Temp*`, 예상 밖의 두 번째 player/controller, 비활성 duplicate camera, DontDestroyOnLoad의 동일 타입이 있으면 캡처를 만들지 말고 실패한다. CSV는 사람이 따로 쓰지 않고 sidecar JSON 묶음에서만 생성한다.

## 빠른 → 느린 staged pipeline

한 단계가 실패하면 다음 단계로 가지 않는다. 구현 수정이 생기면 아래 무효화 규칙에 따라 필요한 앞 단계로 돌아간다.

| 단계 | 목적 | 권장 검증 | 실행 시점 |
| --- | --- | --- | --- |
| S0 계약 고정 | 원래 증상과 위험 차원 누락 방지 | QA가 재현 좌표, 상태 전이 표, 금지·허용 결과를 테스트 이름으로 고정 | 구현 전 1회 |
| S1 정적·컴파일 | 문법·참조·diff 오류 조기 차단 | `git diff --check`, 대상 script validation, 컴파일/Console | 매 수정 후 |
| S2 순수 함수·상태 단위 | 가장 싼 논리 결함 차단 | axis table, tail-only, multi-occluder, renderer ownership | 매 수정 후, 수초 |
| S3 직렬화·관련 EditMode | Unity 생명주기와 저장 경계 | 임시 씬 round-trip, 관련 테스트 fixture만 | S2 PASS 후 |
| S4 대상 scene smoke | 연결·초기 상태 확인 | 씬 open/Play, 대상 수, HUD/camera, 원래 증상 1개, Console | 후보별 1회 |
| S5 관련 경계 축소 매트릭스 | 대표 geometry 회귀 | 3 오클루더 × flip × frame의 pairwise 표본, 경계 왕복 | S4 PASS 후 |
| S6 전체 회귀 | 프로젝트 회귀 | 전체 EditMode/필요 PlayMode | 후보 freeze 후 1회 |
| S7 최종 넓은 매트릭스·증거 | 수용 범위와 화면 증명 | 72/432 전체 매트릭스가 정말 필요할 때 1회, atomic capture, Console·dirty·보호 diff | 사용자 보고 직전 |

이번 사례에서는 QA 1차 전에 S2의 ownership/multi와 S3의 round-trip이 있었어야 한다. Barrel jitter 수정 뒤에는 S2 축별 표만 다시 돌려 wall 회귀를 즉시 잡은 후에 S6 전체 스위트를 실행했어야 한다. 전체 `200+` 테스트, 72/432 표, 캡처를 각 시도마다 반복하는 순서는 비용 대비 효율이 낮다.

최종 전체 매트릭스도 위험 기반으로 제한할 수 있다. occlusion 로직의 독립 차원은 `오클루더 geometry`, `frame alpha`, `flip`, `앞/뒤 sorting`, `entry/release boundary`다. 물리 8접근 × 6단계는 controller/collider가 변경된 경우에만 최종 1회 필요하다. 시각 resolver만 변경됐다면 pairwise 표 + 사용자 대표 WASD 경로가 더 직접적인 증거다.

## Unity MCP single-owner lease

이번 이력에는 QA가 Play+Paused인 상태라 런타임 구현 담당이 TestRunner를 실행하지 못했고, 서로의 Unity 상태를 보존하려고 수동 조정한 흔적이 있다. 이는 올바른 조심성이지만, 사람/에이전트 메시지에만 의존해서는 경합과 재작업을 막지 못한다.

### 정책

- 동일 Unity 프로젝트의 MCP/Editor/배치 검증 소유자는 항상 한 명이다.
- lease 획득 전에는 Play, Pause, scene open/save, TestRunner, Console clear, Camera capture, Refresh를 호출하지 않는다.
- lease에는 `work_id`, `agent`, `run_id`, `editor_pid`, `scene`, `acquired_utc`, `expires_utc`, `baseline_play/pause/scene/dirty`를 기록한다.
- 60초 단위 heartbeat, 기본 만료 5분. PID가 살아 있고 만료만 지난 경우 자동 탈취하지 않는다.
- handoff는 현재 소유자가 Play 종료, 임시 오브젝트 제거, scene dirty 원복 여부를 기록하고 명시적으로 release한 뒤 이뤄진다.
- 배치 Unity도 같은 프로젝트의 Library를 쓰면 동일 lease 대상이다. 병렬 실행이 필요하면 검증용 격리 복사본마다 별도 project key를 사용한다.

### 구현 판단

문서만으로는 경쟁 조건을 막을 수 없으므로 **실제 범용 lease 스크립트가 필요하다.** 권장 파일은 `tools/verification/unity-mcp-lease.ps1`이며 `FileMode.CreateNew`를 사용해 원자적으로 획득하고 owner/run_id가 일치할 때만 renew/release해야 한다. 잠금 파일은 추적하지 않는 `UnityProject/Temp/last-host-unity-mcp-lease.json`에 둔다. QA 역할·스킬에는 모든 Unity 도구 호출 전 이 스크립트 사용을 강제한다.

MCP 서버 자체에 hook을 넣을 필요는 없다. 프로젝트 단위 lease와 정책 준수로 충분하다. 다만 lease를 거치지 않은 호출도 기술적으로 가능하므로 총괄 게이트는 evidence manifest의 lease_owner 누락을 PASS 차단 사유로 봐야 한다.

## 증거 원자성, stale-object guard, PASS 무효화

### candidate fingerprint

Git HEAD만으로는 dirty worktree 후보를 식별할 수 없다. `candidate_fingerprint`는 최소한 다음을 정렬해 SHA-256 해야 한다.

- 검증 대상 production code, scene, prefab, asset meta, ProjectSettings dependency
- 관련 test/harness source
- Unity version과 package lock
- `git diff --binary -- <관련 경로>` 또는 각 파일 내용 hash

모든 XML, CSV, log, PNG sidecar는 같은 fingerprint와 `run_id`를 가져야 한다. 파일명 `final`, `final-v2`는 사람이 이해하기 쉽지만 유효성 판정 기준으로 사용하면 안 된다.

### 변경 후 이전 PASS 무효화 규칙

| 변경 | 무효화 범위 |
| --- | --- |
| production code/scene/prefab/asset/settings | S1 이후의 모든 자동·Play·캡처 PASS |
| 관련 test 코드 | 해당 자동 테스트 PASS와 그 테스트 결과를 인용한 QA 판정 |
| 캡처/매트릭스 harness | 그 harness가 만든 CSV·PNG·sidecar |
| acceptance contract 변경 | S0부터 전체 판정 |
| QA 보고 문구만 변경 | 실행 증거는 유지, 보고서 checksum만 갱신 |
| 상태판/작업 기록만 변경 | 기능 증거 유지 |

새 수정 뒤 이전 XML이 계속 `Passed`여도 상태는 `SUPERSEDED`다. QA 보고에는 `현재 유효 run_id` 하나만 둔다. 실패와 과거 PASS는 삭제하지 않고 manifest에서 `superseded_by`를 기록한다. 코드 수정 후 QA가 관련 테스트만 실행해 PASS한 상태에서는 전체 회귀 PASS를 다시 주장할 수 없다.

### 실제 범용 도구 필요성

다음 두 도구는 문서만으로 충분하지 않다.

1. `UnityVerificationEvidenceCapture.cs` 같은 Editor 전용 원자 캡처/sidecar 생성기
   - Camera 직접 렌더, 상태 manifest, stale-object guard, checksum, atomic rename 담당
2. `verification-evidence-audit.ps1`
   - candidate fingerprint 계산, 산출물 manifest 대조, 변경 후 stale 판정, 유효 run_id 하나만 선택

접촉/phase CSV 생성도 반복 작업이 계속된다면 Editor harness로 승격해야 한다. 현재처럼 `Unity_RunCommand`에 긴 일회성 코드를 넣으면 실행 코드 자체가 저장소에 남지 않아 동일 조건 재실행을 증명하기 어렵다. 최소한 matrix sampler는 `UnityProject/Assets/_Project/Editor/Verification/` 아래 버전 관리 코드로 두고 RunCommand는 그 entry point만 호출해야 한다.

## 문서·테스트 템플릿만 보완하면 되는 부분

다음은 새 에이전트나 새 스킬, 대형 프레임워크 없이 문서 수정으로 충분하다.

- QA를 구현 후 검사자가 아니라 **S0 테스트 charter 작성자**로 앞당기기
- 작업 배정서에 위험 차원과 최소 테스트 이름을 적는 필드 추가
- `verification.md`에 candidate fingerprint, run_id, lease owner, 증거 manifest, superseded run, 마지막 production 변경 이후 실행 여부 필드 추가
- `handoff.md`에 Unity lease 상태, Play/Pause/scene/dirty, 임시 오브젝트 유무 추가
- QA 역할 문서에 실패 후 production 변경 시 이전 PASS 자동 무효화와 fast-stage 재시작 규칙 추가
- `$unity-verification-runner`에 S1~S7 stop-on-failure와 최종 증거 원자성 체크 추가
- 총괄 게이트에 `현재 후보 fingerprint와 증거 fingerprint 일치`를 차단 조건으로 추가
- 전체 Cartesian 매트릭스는 최종 후보에서 한 번만 실행하고, 개발 반복에는 위험 기반 pairwise 표를 쓰는 규칙 추가

새 검증 에이전트를 더 만드는 것은 권장하지 않는다. 현재 구현·독립 QA·총괄의 역할 수는 충분하다. 필요한 것은 역할 수 증가가 아니라 QA의 선행 참여, 책임 차원 명시, 기계적인 lease/fingerprint/evidence 결합이다.

## 권장 적용 순서와 예상 효과

### P0 — 즉시

1. QA 역할/검증 스킬/verification·handoff 템플릿에 S0~S7, 무효화, run_id/fingerprint/lease 필드를 추가한다.
2. 현재 occlusion 테스트에 serialize round-trip, 축별 hysteresis, 다중 오클루더, renderer 전체 소유권 전이를 추가한다.
3. transient hide를 씬에 저장하는 현재 builder 동작을 유지할지 재검토하고, 가능하면 저장 기본 상태를 enabled로 단순화한다.

효과: 이번 QA의 세 번 연속 코드 재수정은 관련 테스트 단계 한두 번으로 축소할 수 있다.

### P1 — 다음 Unity 플레이어블 검증 전

1. project 단위 MCP lease 스크립트를 추가한다.
2. evidence fingerprint auditor를 추가한다.
3. 원자 캡처/sidecar/stale-object guard Editor helper를 추가한다.

효과: 잘못된 `final` 캡처의 PASS 채택, Play 상태 충돌, 수정 뒤 구형 XML 재사용을 기계적으로 차단한다.

### P2 — 매트릭스 검증이 두 번째로 반복될 때

1. 일회성 RunCommand matrix 코드를 저장소의 재사용 가능한 Editor harness로 승격한다.
2. final full matrix와 개발 pairwise matrix를 분리한다.
3. 실패 산출물은 run manifest로 보존하되 중복 로그와 PNG는 압축·선별한다.

효과: 긴 RunCommand 작성·재설명·CSV 수동 해석 비용을 낮추고 동일 조건 재현성을 높인다.

## 최종 판정

- 루프 엔지니어링의 **독립 검증과 최종 차단 기능은 유효**하다. 독립 QA가 실제 결함과 잘못된 증거를 찾아냈다는 점이 이를 증명한다.
- 현재 하네스는 **조기 차단, Unity 단일 소유, 증거 원자성, 변경 후 PASS 무효화가 부족**하다.
- 이번 비용은 “이 정도 기능이면 당연히 필요한 비용”으로 전부 정당화할 수 없다. 시각 가림 자체는 중간 난도였지만, 반복 전체 검증과 캡처 재생성 상당 부분은 프로세스·하네스 부채다.
- 에이전트 수를 늘리는 해결책은 맞지 않는다. QA를 구현 전에 투입하고, 최소 상태 테스트를 고정하며, lease/fingerprint/atomic evidence 세 도구를 추가하는 것이 정확한 보완이다.
