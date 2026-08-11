# A01 회사 일상 혼합형 모션 독립 애니매틱 Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 승인된 `A01-C03 공간 + A01-C02 연기`를 실제로 판단할 수 있는 독립 무음 Unity 애니매틱으로 만들고, 최종 아트·오디오·러닝타임·전체 오프닝 연결은 확정하지 않는다.

**Architecture:** imagegen으로 만든 비최종 배경·cast pose grid·전경 마스크를 시네마틱 전용 Editor assembly가 import하고, 결정적 scene builder가 `SpriteRenderer + AnimationClip + Timeline` 자산을 생성한다. 전용 preview launcher가 현재 편집 씬과 `playModeStartScene`을 `SessionState`에 보관한 뒤 A01만 재생하고, Play 종료나 중단 뒤 원상 복구한다. 런타임 게임플레이 assembly와 Startup 흐름은 의존하거나 수정하지 않는다.

**Tech Stack:** Unity `6000.4.6f1`, URP `17.4.0`, Timeline `1.8.12`, 기본 Animation, SpriteRenderer, Unity Test Framework `1.6.0`, OpenAI 내장 imagegen, PowerShell 7 + .NET `System.Drawing` 작업 전용 chroma-key 도구, PowerShell 7 검증 wrapper.

## Global Constraints

- 구현은 사용자가 옵션 1 isolated worktree 실행 방식·작업 위치와 함께 `내장 imagegen 초기 3회(반려 layer만 correction, 누적 최대 6회)` 및 이 계획에 열거된 A01 전용 Unity 파일 수정을 명시 승인한 뒤 시작한다. 작업 위치는 `C:/projects/Last-Host/.worktrees/a01-office-animatic`, branch는 `feat/a01-office-animatic`이다.
- `_workspace`는 기존 `task.md`와 `verification.md` 두 파일을 기본으로 유지한다. `artifacts/`는 이미지 원본 또는 실제 고비용 검증 증거가 생길 때만 만든다.
- A01은 독립 무음 프리비즈다. `StartupController`, `StartupPlayModeBootstrap`, `Startup.unity`, `EditorBuildSettings.asset`, `ProjectSettings`, `manifest.json`, `packages-lock.json`을 수정하지 않는다.
- 보호 파일의 canonical 계약은 byte stream에서 CRLF(`0D0A`)만 LF(`0A`)로 치환한 normalized bytes/SHA-256이다. lone CR(`0D` 뒤에 `0A` 없음)는 즉시 FAIL이다. raw checkout bytes/SHA-256은 환경 관찰값만 기록하며 pass/fail 기준이 아니고, Git blob OID는 provenance 보조값일 뿐 canonical gate가 아니다. `.gitattributes`는 수정하지 않는다.
- Cinemachine, 2D Animation, 오디오, ParticleSystem, 자막·내레이션, A02, B08, 학교·가정·확산·기침·주인공 장면을 추가하지 않는다.
- 생성 이미지는 모두 `preview-only` 후보다. 사용자 수용과 게임 규격 재제작 전에는 최종 에셋 또는 완성 컷신으로 표현하지 않는다.
- imagegen ledger는 attempt 07까지 실제 `7/7`로 종료됐다. attempt 08, 재시도, 외부/API/CLI 생성, 신규 패키지·네트워크 의존성은 새 사용자 승인 전 금지한다.
- Cast는 `tools/art/Repack-ChromaPoseGrid.ps1`와 `tools/art/Test-RepackChromaPoseGrid.ps1`의 manifest 기반 정수 translation 경로만 사용한다. 전경은 재생성하지 않고 별도 `Remove-ConnectedChromaMatte` TDD 경로로 preview 파생본만 만든다. Cast 독립 QA PASS 전에는 foreground·Unity import를 진행하지 않는다.
- Timeline은 `24 fps`, 제한 포즈 교체와 큰 변환은 `12 fps` 간격의 stepped key로 만든다. Transform 값은 `PPU 100` 기준 `0.01 world unit` 격자에 양자화한다.
- 초기 측정 스케줄은 `[36, 36, 36, 30, 42, 24]` frames, 합계 `204 frames = 8.5 seconds at 24 fps`다. 코드·manifest·테스트에 `preview-measurement-only`와 `IsFinalTiming == false`를 함께 기록한다.
- 고비용 Unity 테스트는 `tools/verification/Invoke-HighCostVerification.ps1 -Route UnityEditMode`만 사용한다. low-level runner의 Run parameter set을 직접 호출하지 않는다.
- 모든 Unity Editor·wrapper 작업은 한 run ID의 lease를 작업 직전에 획득하고 `try/finally` 의미로 해제한다. 실행이 60초를 넘으면 tool yield 지점마다, 늦어도 60초마다 `Renew`하고 단일 blocking call로 heartbeat를 막지 않는다.
- 현재 capability profile의 `McpPlay.available`은 `false`다. 전용 Editor Play는 개발 확인과 사용자 수용 보조이며 canonical 자동 PASS로 기록하지 않는다.
- technical PASS 뒤에도 사용자 실제 재생 수용 전 상태는 `기술 검증 통과 — 사용자 수용 대기`다.
- **Historical through pre-run004:** 계획 감사 R2 correction 2/2 뒤 신규 folder `.meta` candidate identity 누락을 발견해 `a01-plan-reclass-001`로 R3 재분류했다. 이후 R3 plan correction 2/2가 API compatibility/stale-state review failure까지 채워져 `a01-plan-reclass-002`로 R3을 유지했다. 사용자 승인 옵션 1 통합은 처음 current plan correction `1/2`였으나 독립 리뷰 `a01-option1-plan-review-001`이 Important 4건으로 FAIL해 current plan `2/2`를 채웠다. root cause는 `옵션1 개정에 stale counter·과거 실행 체크박스·P5 prop ambiguity·tool commit identity 누락`, change plan은 `위 4건만 수정`이며 `a01-plan-reclass-003`의 새 cycle을 시작했다. reclass-006은 Task 2B non-atomic bundle publish와 Task 2A external `pwsh`/nonterminating Copy rollback fail-fast 누락으로 2/2 종료됐다. `a01-plan-reclass-007` current plan correction `0/2`는 **CLEAN**이다. Task 2A production/test `a01-repack-implementation-reclass-003`은 atomic overwrite null-backup fix로 correction `1/2`다. QA 재진입 2차 candidate `30D41D844B7585513140BB38F0588FCF5689321538C332EB1F61ED248ABCBCA3` (`1280×1600`, `1,141,236` bytes)는 auto metrics strongkey `0`/unresolved-qualified `0`/despilled `14273` PASS지만 P4/P5 투명 경계의 non-linear bright magenta 1px 선·점으로 independent visual QA `2/2` REJECT다. root cause는 donor-line conservative classifier 밖 nonlinear key contamination이며 현 계약은 silhouette/edge RGB policy 확장 없이 제거할 수 없다. 새 `a01-repack-visual-fringe-reclass-001`은 **사용자 결정 대기**였고 새 QA current `0/2`는 시작 금지였다. 당시 canonical/Unity/foreground, imagegen `7/7`, execution S0 `0/2`, high-cost `0`, 비용 `주의`였다. R3도 기본 `task.md`+`verification.md` 두 파일을 유지하며 별도 이력 파일은 만들지 않는다.

- **Current status supersession — A01 mask-only occlusion reclassification:** 실행 가능한 color foreground recovery는 automatic PASS 뒤에도 독립 visual QA에서 `24A03C…D526`, `3B9426…A4AF4B` 두 후보 모두 같은 visible pink/magenta fringe로 REJECT되어 visual correction `2/2`를 채웠다. root cause는 반려된 foreground RGB를 visible production layer로 사용하는 구조다. `a01-foreground-occlusion-mask-reclass-001` current `0/2`는 preserved source에서 alpha silhouette만 만들고, 승인 BG를 같은 transform으로 복제해 SpriteMask 안에서만 그리는 mask-only 구조로 전환한다. color foreground RGB는 production·scene에서 사용 금지다. Scene production/test `0/2`, high-cost `3`, imagegen `7/7`, 비용 `주의`는 유지한다.

### Unity folder `.meta` contract

새 Unity folder의 sibling `.meta`는 자식 directory를 fingerprint하거나 `git add`해도 자동 포함되지 않는다. 다음 목록을 production dependency로 직접 다룬다.

```powershell
$a01RedFolderMetaPaths = @(
  'UnityProject/Assets/_Project/Art/Cinematics.meta',
  'UnityProject/Assets/_Project/Art/Cinematics/Opening.meta',
  'UnityProject/Assets/_Project/Art/Cinematics/Opening/A01.meta',
  'UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview.meta',
  'UnityProject/Assets/_Project/Editor/Cinematics.meta',
  'UnityProject/Assets/_Project/Editor/Cinematics/Opening.meta',
  'UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01.meta',
  'UnityProject/Assets/_Project/Tests/EditMode/Cinematics.meta',
  'UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01.meta'
)
$a01GeneratedFolderMetaPaths = @(
  'UnityProject/Assets/_Project/Scenes/Cinematics.meta',
  'UnityProject/Assets/_Project/Scenes/Cinematics/Opening.meta',
  'UnityProject/Assets/_Project/Timelines.meta',
  'UnityProject/Assets/_Project/Timelines/Cinematics.meta',
  'UnityProject/Assets/_Project/Timelines/Cinematics/Opening.meta',
  'UnityProject/Assets/_Project/Timelines/Cinematics/Opening/A01.meta',
  'UnityProject/Assets/_Project/Animations.meta',
  'UnityProject/Assets/_Project/Animations/Cinematics.meta',
  'UnityProject/Assets/_Project/Animations/Cinematics/Opening.meta',
  'UnityProject/Assets/_Project/Animations/Cinematics/Opening/A01.meta',
  'UnityProject/Assets/_Project/Animations/Cinematics/Opening/A01/Preview.meta'
)
$a01AllFolderMetaPaths = $a01RedFolderMetaPaths + $a01GeneratedFolderMetaPaths
$a01RedProductionPaths = @(
  'UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01',
  'UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview',
  'UnityProject/Assets/_Project/Editor/Startup/StartupPlayModeBootstrap.cs',
  'UnityProject/Assets/_Project/Scripts/UI/Startup/StartupController.cs'
) + $a01RedFolderMetaPaths
$a01GreenProductionPaths = @(
  'tools/art/Repack-ChromaPoseGrid.ps1',
  'tools/art/Test-RepackChromaPoseGrid.ps1',
  'tools/art/Remove-ConnectedChromaMatte.ps1',
  'tools/art/Test-RemoveConnectedChromaMatte.ps1',
  'UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01',
  'UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview',
  'UnityProject/Assets/_Project/Timelines/Cinematics/Opening/A01',
  'UnityProject/Assets/_Project/Animations/Cinematics/Opening/A01/Preview',
  'UnityProject/Assets/_Project/Scenes/Cinematics/Opening/A01OfficeAnimatic.unity.meta',
  'UnityProject/Assets/_Project/Editor/Startup/StartupPlayModeBootstrap.cs',
  'UnityProject/Assets/_Project/Scripts/UI/Startup/StartupController.cs'
) + $a01AllFolderMetaPaths
```

- RED 전에 `$a01RedFolderMetaPaths` 9개가 모두 존재해야 한다.
- GREEN/full 전에 `$a01AllFolderMetaPaths` 20개와 scene file `.meta`가 모두 존재해야 한다.
- file `.meta`는 각 production directory 안에서 fingerprint된다. root folder의 sibling `.meta`만 위 배열로 추가한다.

---

## Task 1: 실행 작업공간과 R3 S0 계약 고정

**Owner:** 프로젝트 조정 에이전트 → QA/검증 에이전트

**Files:**

- Modify: `_workspace/active/2026-08-10-a01-office-animatic/task.md`
- Modify: `_workspace/active/2026-08-10-a01-office-animatic/verification.md`
- Modify: `docs/project-handoff/current-task-board.md`
- Modify: `docs/project-handoff/task-cost-dashboard.md`
- Create on first high-cost run only: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/agent-brief.json`
- Create on first high-cost run only: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/verification-current-state.json`
- Created by wrapper on first high-cost run: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/verification-attempt-ledger.json`

- [ ] 사용자가 선택한 실행 방식과 작업 위치를 `task.md`에 기록한다. 같은 응답에서 내장 imagegen 초기 3회·누적 최대 6회와 이 계획의 A01 전용 Art/Editor/Test/Scene/Timeline/Animation 수정 승인도 명시한다. isolated worktree 선택 시 `superpowers:using-git-worktrees`를 사용하고, 현재 checkout 선택 시 `main` in-place 변경 승인을 명시한다.
- [ ] `git status --short`로 작업 시작 baseline을 기록하고, 예상 밖 변경이 있으면 해당 파일을 계획 범위에 섞지 않는다.
- [ ] QA/검증 에이전트가 `task.md`의 A01-C01~C10을 production 작성 전에 검토한다. 누락·충돌이 있으면 첫 blocker만 반환하고 구현을 시작하지 않는다.
- [ ] 아래 보호 파일의 시작 **LF-normalized canonical** SHA-256과 bytes를 `verification.md`에 기록한다. raw checkout bytes/SHA-256은 관찰값으로 별도 기록하고 canonical 판정에는 쓰지 않는다.

| 보호 파일 | LF-normalized canonical bytes | LF-normalized canonical SHA-256 |
| --- | ---: | --- |
| `UnityProject/Packages/manifest.json` | 2069 | `B07DD4E37BA1336B93D763B23E3480BE7943EF4C56DBFDA7EE191FF87B0AF298` |
| `UnityProject/Packages/packages-lock.json` | 13840 | `943F92F1229C2A366FD42AA7180B73BDB8B6019AE21C1A6CE38C80A15D8C262E` |
| `UnityProject/ProjectSettings/EditorBuildSettings.asset` | 799 | `67B153F8C73C6C9E7F8C60D47D03A837DFEC207E757AC65FEB6619F58BE28755` |
| `UnityProject/Assets/_Project/Editor/Startup/StartupPlayModeBootstrap.cs` | 1346 | `634BD355DF765B7283774D3B20983299F2637C8F0503B831057535F58133E5C2` |
| `UnityProject/Assets/_Project/Scripts/UI/Startup/StartupController.cs` | 15040 | `042B816E531448ABD5DC265C183D309AE1E084E25581E8DF9D4E48FE73931730` |

```powershell
function Get-A01CanonicalLfHash {
  param([Parameter(Mandatory)][string]$Path)
  $raw = [System.IO.File]::ReadAllBytes($Path)
  $normalized = New-Object System.Collections.Generic.List[byte]
  for ($i = 0; $i -lt $raw.Length; $i++) {
    if ($raw[$i] -eq 13) {
      if ($i + 1 -ge $raw.Length -or $raw[$i + 1] -ne 10) { throw "Lone CR detected: $Path at byte $i" }
      [void]$normalized.Add(10)
      $i++
    } else {
      [void]$normalized.Add($raw[$i])
    }
  }
  $bytes = $normalized.ToArray()
  [pscustomobject]@{
    Bytes = $bytes.Length
    Sha256 = $(
      $sha = [System.Security.Cryptography.SHA256]::Create()
      try { ([System.BitConverter]::ToString($sha.ComputeHash($bytes))).Replace('-','') }
      finally { $sha.Dispose() }
    )
  }
}
```

- [ ] `A01OfficeProtectedBaselineTests.cs`의 두 protected baseline test와 Task 5 정적 검사는 위와 동등한 byte-level LF-normalized helper를 사용한다. C# test는 `File.ReadAllBytes`에서 CRLF만 LF로 치환하고 lone CR를 실패시킨 뒤 `using (var sha = SHA256.Create()) { sha.ComputeHash(normalizedBytes); }`로 비교한다. 이는 Unity NET Standard 2.0 reference 호환 계약이다. raw `Get-FileHash` 또는 raw checkout SHA-256을 pass/fail 직접 판정에 사용하지 않는다.

- [ ] 첫 Unity 작업 전에 실제 Editor PID, Play/Pause/scene/dirty를 확인한다. Play·Pause가 켜져 있으면 정상 종료하고, dirty scene은 사용자에게 저장 또는 취소를 선택받은 뒤 다시 확인한다. 정리 뒤 확인값만 lease baseline으로 사용하며, 열린 scene이 없을 때만 `(none)`을 기록한다.

```powershell
$a01EditorPid = [int](Read-Host '현재 Last-Host Unity Editor PID')
$a01BaselineScene = Read-Host '확인한 baseline scene path 또는 (none)'
$a01BaselinePlay = [bool]::Parse((Read-Host '확인한 baseline Play: True 또는 False'))
$a01BaselinePause = [bool]::Parse((Read-Host '확인한 baseline Pause: True 또는 False'))
$a01BaselineDirty = [bool]::Parse((Read-Host '확인한 baseline dirty: True 또는 False'))
$a01LeaseAgent = Read-Host '현재 단계 lease owner: unity-scene-integration 또는 qa-verification'
$a01LeaseRunId = Read-Host '현재 단계 exact run ID'
if ($a01EditorPid -le 0) { throw 'Unity Editor PID must be positive.' }
if ([string]::IsNullOrWhiteSpace($a01BaselineScene)) { throw 'Baseline scene must be explicit.' }
if ($a01BaselinePlay -or $a01BaselinePause -or $a01BaselineDirty) { throw 'Unity baseline must be clean and stopped before lease acquire.' }
if ($a01LeaseAgent -notin @('unity-scene-integration','qa-verification')) { throw 'Unexpected lease owner.' }
if ([string]::IsNullOrWhiteSpace($a01LeaseRunId)) { throw 'Lease run ID must be explicit.' }
pwsh tools/verification/UnityMcpLease.ps1 Acquire `
  -ProjectPath UnityProject `
  -Agent $a01LeaseAgent `
  -WorkId 2026-08-10-a01-office-animatic `
  -RunId $a01LeaseRunId `
  -EditorProcessId $a01EditorPid `
  -Scene 'Assets/_Project/Scenes/Cinematics/Opening/A01OfficeAnimatic.unity' `
  -BaselinePlay $a01BaselinePlay `
  -BaselinePause $a01BaselinePause `
  -BaselineScene $a01BaselineScene `
  -BaselineDirty $a01BaselineDirty `
  -TtlSeconds 300
```

- [ ] 위 Acquire block은 실제 Unity 작업이 있는 Task 3~5에서 해당 run ID로 다시 실행한다. 실행 중 60초마다 `UnityMcpLease.ps1 Renew`를 호출하고 성공·실패·중단 모두 `finally` 단계에서 같은 agent/work/run으로 `Release`한다. 만료는 자동 강탈 근거가 아니며 `Status`와 기존 process 생존을 먼저 확인한다.
- [ ] 첫 고비용 실행 직전에 `agent-brief.json`을 다음 exact schema로 `apply_patch`한다.

```json
{
  "work_id": "2026-08-10-a01-office-animatic",
  "context_mode": "packet-only",
  "fork_turns": "none",
  "required_files": [
    "_workspace/active/2026-08-10-a01-office-animatic/task.md",
    "docs/superpowers/plans/2026-08-10-a01-office-animatic.md",
    "UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01/LastHost.Prototype.Cinematics.A01.Tests.asmdef"
  ],
  "message": "Run only the requested A01 UnityEditMode criterion against the current run ID and fingerprint. Stop at the first blocker. Do not use McpPlay, build, a low-level runner directly, or modify production from QA.",
  "include_conversation_history": false
}
```

- [ ] RED 전 아래 object가 console에 출력한 exact JSON을 `apply_patch`로 `verification-current-state.json`에 넣는다. shell redirection이나 `Set-Content`로 파일을 만들지 않는다.

```powershell
$a01RunId = 'a01-contract-red-001'
$a01Fingerprint = (Get-Content -Raw -LiteralPath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/fingerprint-a01-contract-red-001.json' | ConvertFrom-Json).candidate_fingerprint
$a01State = [ordered]@{
  schema_version = 1
  work_id = '2026-08-10-a01-office-animatic'
  status = 'ready-for-verification'
  run_id = $a01RunId
  candidate_fingerprint = $a01Fingerprint
  cost = [ordered]@{
    unity_starts = 0
    mcp_starts = 0
    build_starts = 0
    recorded_high_cost_attempts = 0
  }
  evidence = @()
}
$a01State | ConvertTo-Json -Depth 6
```

- [ ] 각 후속 run 전에는 직전 current-state의 네 cost 정수를 그대로 복사하고 새 run/fingerprint/status를 설정하되 `evidence`는 반드시 빈 배열로 초기화한다. 과거 run의 RED/GREEN 증거는 `verification.md`에만 남긴다.
- [ ] 비용 현황판에 `Unity starts 0 / Editor Play 0 / build 0`의 시작 행과 `R2 plan 2/2 → a01-plan-reclass-001 → R3 plan 2/2 → a01-plan-reclass-002 → option1 current plan 2/2(독립 리뷰 FAIL) → a01-plan-reclass-003 current plan 2/2(initial/scoped review FAIL) → a01-plan-reclass-004 current plan 2/2 → a01-plan-reclass-005 current plan 2/2 → a01-plan-reclass-006 current plan 2/2(Task2A/2B isolation) → a01-plan-reclass-007 current plan 0/2(CLEAN) / execution S0 0/2 / production-test 2/2 → a01-repack-implementation-reclass-001 2/2 → a01-repack-implementation-reclass-002 2/2 → a01-repack-implementation-reclass-003 current 0/2 / independent visual QA correction 1/2`를 분리해 기록한다. historical imagegen `6/6`은 보존하고, attempt 07 actual `7/7`을 유지한다.

**Current gate:** `a01-foreground-occlusion-mask-reclass-001` current `0/2`에서 mask-only RED/GREEN·mask visual QA·static contract를 통과한 뒤에만 scene target RED·owner GREEN·independent frozen Play/scene verification을 시작한다.

**Commit:**

```powershell
git add _workspace/active/2026-08-10-a01-office-animatic docs/project-handoff/current-task-board.md docs/project-handoff/task-cost-dashboard.md docs/superpowers/plans/2026-08-10-a01-office-animatic.md
git commit -m "docs: start A01 office animatic execution"
```

---

## Task 2: 비최종 프리비즈 레이어 후보 3종 제작과 시각 게이트

### Active Task 2 override — deterministic Cast repack

이 절이 현재 Task 2의 **유일한 실행 절차**다. 아래 `Historical attempt 07 기록`부터 Task 3 전까지 남은 과거 prompt·normalization·`Remove-ConnectedChromaMatte` Cast 명령과 unchecked checkbox는 provenance일 뿐 실행하지 않는다. 새 계획을 만들지 않고 승인 설계 `docs/superpowers/specs/2026-08-10-a01-cast-regeneration-normalization-design.md`를 그대로 구현한다.

**Owners:** 비주얼/테크아트 구현 에이전트(layout manifest) → Unity 씬/통합 구현 에이전트(tool·test·derivative) → 독립 비주얼 QA(raw/derivative 판정)

**Files:**

- Create: `tools/art/Repack-ChromaPoseGrid.ps1`
- Create: `tools/art/Test-RepackChromaPoseGrid.ps1`
- Create: `tools/art/Remove-ConnectedChromaMatte.ps1`
- Create: `tools/art/Test-RemoveConnectedChromaMatte.ps1`
- Create: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-layout.json`
- Create: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-repacked-alpha.png`
- Create: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-foreground-mask-alpha.png`
- Modify only after automatic + independent visual QA PASS: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-source.png`
- Create after relevant QA PASS: `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-bg-room-preview.png`
- Create only after the same PASS: `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-cast-pose-grid-preview.png`
- Create after foreground QA PASS: `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-foreground-mask-preview.png`
- Create after all three preview layers PASS: `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-preview-contract.json`
- Create after all three preview layers PASS: `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/SOURCE.md`

**Interfaces:**

- Tool: `Repack-ChromaPoseGrid.ps1 -InputPath <png> -LayoutPath <json> -OutputPath <png> [-Force]`
- Test: `Test-RepackChromaPoseGrid.ps1 -ToolPath <ps1> -RealSourcePath <png> -LayoutPath <json> -ExpectedRealSourceSha256 <64 uppercase hex>`
- Foreground tool: `Remove-ConnectedChromaMatte.ps1 -InputPath <png> -OutputPath <png> [-KeyColor '#ff00ff'] [-SeedTolerance 24] [-FloodTolerance 48] [-EdgeRadius 2] [-DonorRadius 8] [-BlendResidual 24] [-MinKeyMix 0.08] [-Force]`
- Foreground test: `Test-RemoveConnectedChromaMatte.ps1 -ToolPath <ps1> -RealSourcePath <png> -ExpectedRealSourceSha256 <64 uppercase hex>`
- Raw invariant: `1122×1402`, `1,648,495 bytes`, SHA-256 `24A143D7344DAC8358CD496C6AD03718AADB492D67B96E7CCCF0E46DA08A090D`
- Manifest fixed cuts: `x=[0,281,561,842,1122]`, `y=[0,318,591,847,1107,1402]`; column source axes `x=[140,421,701,982]`; each pose uses `sourceGroundY=maxRetainedY`, target anchor cell-local `(160,306)`.
- Pose IDs row-major: `p1_idle,p1_speak,p1_laugh,p1_rise`; `p2_idle,p2_nod,p2_laugh,p2_hold`; `p3_work,p3_shoulder_laugh,p3_head_turn,p3_hold`; `p4_idle,p4_gesture,p4_exit_turn,p4_hold`; `p5_idle,p5_laugh,p5_step_ready,p5_hold`.

#### Task 2A — Cast repack (현재 실행 가능)

- [ ] **Step 2.1 — S0 갱신·검토.** `task.md`의 합성 oracle과 criterion을 `A01-RP-01 source provenance`, `A01-RP-02 20-cell ownership + authorized matte`, `A01-RP-03 all unmasked non-despilled core exact + authorized matte/despill only`, `A01-RP-04 canvas/alpha/boundary/coverage/determinism`, `A01-RP-05 identity/pose/bag visual invariant`, `A01-RP-06 protected BG/FG/old canonical/Unity immutability`로 고정하고 QA가 구현 착수 가능 여부를 기록한다.

- [ ] **Step 2.2 — RED test 작성.** `Test-RepackChromaPoseGrid.ps1`만 먼저 만든다. TEMP synthetic 4×5 fixture와 manifest로 disconnected chair wheel·bag·shoe 보존, closed key hole 제거, blend edge despill, legitimate nonblend purple byte-exact 보존, wrong SHA, 20 ID/target 누락·중복, rect overlap/gap/out-of-range, empty pose, `308×308` 초과, 6px band 침범, coverage 범위 밖, hard alpha·transparent black, source 불변, all unmasked non-despilled core exact + authorized matte/despill only, repeated SHA와 real unresolved blend fringe `0`을 자체 assert한다. Pester·신규 패키지는 사용하지 않는다.

- [ ] **Step 2.3 — RED 실행 확인.** 다음 명령은 production tool 부재 때문에 nonzero이고 `Tool not found`를 출력해야 한다.

```powershell
pwsh tools/art/Test-RepackChromaPoseGrid.ps1 `
  -ToolPath tools/art/Repack-ChromaPoseGrid.ps1 `
  -RealSourcePath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png `
  -LayoutPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-layout.json `
  -ExpectedRealSourceSha256 24A143D7344DAC8358CD496C6AD03718AADB492D67B96E7CCCF0E46DA08A090D
```

- [ ] **Step 2.4 — manifest 작성.** fixed cuts로 source canvas 전체를 공백·중복 없이 20개 rect로 분할하고 위 pose ID·source axis·target anchor를 JSON에 기록한다. raw SHA·dimensions, output `1280×1600`, grid `4×5`, cell `320×320`, key `(255,0,255)`, Chebyshev tolerance `96`, connectivity `8`, boundary band `6`, coverage `0.05..0.60`을 함께 기록한다.

- [ ] **Step 2.5 — GREEN 최소 구현.** `System.Drawing.Bitmap` 32bpp ARGB buffer에서 full-canvas의 all strong seed key-distance `d∞<=24`(enclosed hole 포함)를 4-neighbor `d∞<=48`로 flood-fill해 투명 black으로 만든다. mask Chebyshev distance `<=2` retained edge만 radius `8` donor(mask distance `>2`, key distance `>96`, squared distance/y/x tie)로 검토하고 donor→key projection `t=0.08..0.92`, residual `<=24`일 때만 donor RGB로 despill한다. alpha·silhouette과 all unmasked non-despilled core는 보존하며 largest-component 선택·scale·rotation·interpolation은 금지한다. manifest rect와 reference retained mask의 교집합 전체를 pose union으로 정수 translation하고 모든 검사를 TEMP output에서 통과한 경우에만 `-OutputPath`를 원자 교체한다.

- [ ] **Step 2.6 — GREEN·real candidate 검증.** 다음 test와 tool 실행이 모두 exit `0`이어야 하며 raw SHA는 전후 동일해야 한다. 구현 소유자는 derivative SHA를 uppercase로 계산해 verification에 literal line `- a01-repack-automatic-qa: PASS; candidate-sha256: <64 uppercase hex>`를 한 번 기록한다.

```powershell
pwsh tools/art/Test-RepackChromaPoseGrid.ps1 `
  -ToolPath tools/art/Repack-ChromaPoseGrid.ps1 `
  -RealSourcePath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png `
  -LayoutPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-layout.json `
  -ExpectedRealSourceSha256 24A143D7344DAC8358CD496C6AD03718AADB492D67B96E7CCCF0E46DA08A090D

pwsh tools/art/Repack-ChromaPoseGrid.ps1 `
  -InputPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png `
  -LayoutPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-layout.json `
  -OutputPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-repacked-alpha.png `
  -Force
```

- [ ] **Step 2.7 — 독립 비주얼 QA.** raw와 repacked-alpha를 원본 상세도로 비교해 P1~P3 seated, P4/P5 standing, P4 black/P5 brown same-body-side bag, 20 pose 의미, chair wheels·P3 laptop·bag straps·shoes 보존, magenta fringe·잘림·셀 간 jitter 부재를 판정한다. 첫 blocker에서 중단하고 production은 수정하지 않는다. PASS면 QA가 직접 확인한 derivative SHA와 함께 verification에 literal line `- a01-repack-independent-visual-qa: PASS; candidate-sha256: <64 uppercase hex>`를 한 번 기록한다.

- [ ] **Step 2.8 — 조건부 승격.** 아래 preflight가 자동·비주얼 QA literal PASS를 각각 정확히 한 번 확인한 경우에만 repacked-alpha bytes를 canonical Cast 경로로 승격한다. 같은 raw·manifest·tool로 Unity preview를 다시 생성하고 derivative/canonical/preview 세 SHA가 같은지 확인한다. preflight 또는 SHA 비교가 실패하면 복사·foreground·후속 Unity를 시작하지 않는다.

```powershell
$a01VerificationPath = '_workspace/active/2026-08-10-a01-office-animatic/verification.md'
$a01VerificationText = Get-Content -Raw -LiteralPath $a01VerificationPath -ErrorAction Stop
$a01AutomaticPass = [regex]::Matches($a01VerificationText, '(?m)^- a01-repack-automatic-qa: PASS; candidate-sha256: ([0-9A-F]{64})\r?$')
$a01VisualPass = [regex]::Matches($a01VerificationText, '(?m)^- a01-repack-independent-visual-qa: PASS; candidate-sha256: ([0-9A-F]{64})\r?$')
if ($a01AutomaticPass.Count -ne 1 -or $a01VisualPass.Count -ne 1) { throw 'Cast promotion requires exactly one automatic PASS and one independent visual PASS.' }

$a01RawPath = '_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png'
$a01LayoutPath = '_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-layout.json'
$a01DerivativePath = '_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-repacked-alpha.png'
$a01CanonicalPath = '_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-source.png'
$a01PreviewPath = 'UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-cast-pose-grid-preview.png'
$a01DerivativeSha = (Get-FileHash -Algorithm SHA256 -LiteralPath $a01DerivativePath -ErrorAction Stop).Hash.ToUpperInvariant()
if ($a01AutomaticPass[0].Groups[1].Value -cne $a01DerivativeSha -or $a01VisualPass[0].Groups[1].Value -cne $a01DerivativeSha) { throw 'Automatic and visual PASS must reference the current derivative SHA.' }

$a01PromotionRoot = Join-Path ([IO.Path]::GetTempPath()) ('last-host-a01-cast-promotion-' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $a01PromotionRoot -ErrorAction Stop | Out-Null
$a01PreviewCandidatePath = Join-Path $a01PromotionRoot 'preview-candidate.png'
$a01CanonicalBackupPath = Join-Path $a01PromotionRoot 'canonical-backup.png'
$a01PreviewBackupPath = Join-Path $a01PromotionRoot 'preview-backup.png'
$a01CanonicalExisted = Test-Path -LiteralPath $a01CanonicalPath
$a01PreviewExisted = Test-Path -LiteralPath $a01PreviewPath

try {
  pwsh tools/art/Repack-ChromaPoseGrid.ps1 -InputPath $a01RawPath -LayoutPath $a01LayoutPath -OutputPath $a01PreviewCandidatePath -Force
  if ($LASTEXITCODE -ne 0) { throw "Repack preview candidate failed with exit code $LASTEXITCODE." }
  $a01PreviewCandidateSha = (Get-FileHash -Algorithm SHA256 -LiteralPath $a01PreviewCandidatePath -ErrorAction Stop).Hash.ToUpperInvariant()
  if ($a01DerivativeSha -ne $a01PreviewCandidateSha) { throw 'Cast derivative and regenerated preview candidate SHA must match before promotion.' }

  if ($a01CanonicalExisted) { Copy-Item -LiteralPath $a01CanonicalPath -Destination $a01CanonicalBackupPath -ErrorAction Stop }
  if ($a01PreviewExisted) { Copy-Item -LiteralPath $a01PreviewPath -Destination $a01PreviewBackupPath -ErrorAction Stop }

  try {
    Copy-Item -LiteralPath $a01DerivativePath -Destination $a01CanonicalPath -Force -ErrorAction Stop
    Copy-Item -LiteralPath $a01PreviewCandidatePath -Destination $a01PreviewPath -Force -ErrorAction Stop
    $a01CanonicalSha = (Get-FileHash -Algorithm SHA256 -LiteralPath $a01CanonicalPath -ErrorAction Stop).Hash.ToUpperInvariant()
    $a01PreviewSha = (Get-FileHash -Algorithm SHA256 -LiteralPath $a01PreviewPath -ErrorAction Stop).Hash.ToUpperInvariant()
    if ($a01DerivativeSha -ne $a01CanonicalSha -or $a01DerivativeSha -ne $a01PreviewSha) { throw 'Cast derivative, canonical, and Unity preview SHA must match.' }
  }
  catch {
    if ($a01CanonicalExisted) { Copy-Item -LiteralPath $a01CanonicalBackupPath -Destination $a01CanonicalPath -Force -ErrorAction Stop }
    elseif (Test-Path -LiteralPath $a01CanonicalPath) { Remove-Item -LiteralPath $a01CanonicalPath -Force -ErrorAction Stop }
    if ($a01PreviewExisted) { Copy-Item -LiteralPath $a01PreviewBackupPath -Destination $a01PreviewPath -Force -ErrorAction Stop }
    elseif (Test-Path -LiteralPath $a01PreviewPath) { Remove-Item -LiteralPath $a01PreviewPath -Force -ErrorAction Stop }
    throw
  }
}
finally {
  if (Test-Path -LiteralPath $a01PromotionRoot) { Remove-Item -LiteralPath $a01PromotionRoot -Recurse -Force -ErrorAction Stop }
}
```

#### Task 2B — foreground와 세 레이어 bundle publish (Task 2A 뒤, 원자 publish 계약 보정 전 실행 금지)

- [ ] **Step 2.9 — foreground-only RED/GREEN.** Cast 승격 뒤 `Test-RemoveConnectedChromaMatte.ps1`을 먼저 작성하고 아래 명령이 tool 부재 `Tool not found`로 nonzero인 RED를 확인한다. 그 뒤 `Remove-ConnectedChromaMatte.ps1`을 최소 구현한다. `System.Drawing` 32bpp buffer에서 seed/flood mask만 transparent black으로 만들고, mask 2px 안의 retained edge만 donor-line 조건을 통과할 때 RGB despill하며 alpha와 silhouette은 바꾸지 않는다.

```powershell
pwsh tools/art/Test-RemoveConnectedChromaMatte.ps1 `
  -ToolPath tools/art/Remove-ConnectedChromaMatte.ps1 `
  -RealSourcePath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-foreground-mask-source.png `
  -ExpectedRealSourceSha256 D782D38E4D510E1D13680C21D6642F86647DF53662B8D94150376EC73770F1E1
```

- [ ] **Step 2.10 — foreground GREEN·자동·시각 QA.** 아래 두 명령이 exit `0`이어야 한다. source SHA `D782…F1E1`, canvas `1672×941`, hard alpha, transparent black, deterministic SHA를 확인한다. `(0,0)`, `(800,300)`, `(800,750)`, mug-hole `(1208,835)`는 transparent이고 monitor `(230,520)`, lower desk `(800,900)`, mug `(1160,830)`, `(1200,820)`, `(1214,842)`, `(1204,850)`은 opaque여야 한다. 독립 비주얼 QA는 desk/monitor/plant/mug/pen 윤곽의 magenta fringe와 geometry 침식을 실제 크기로 판정한다.

```powershell
pwsh tools/art/Test-RemoveConnectedChromaMatte.ps1 `
  -ToolPath tools/art/Remove-ConnectedChromaMatte.ps1 `
  -RealSourcePath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-foreground-mask-source.png `
  -ExpectedRealSourceSha256 D782D38E4D510E1D13680C21D6642F86647DF53662B8D94150376EC73770F1E1

pwsh tools/art/Remove-ConnectedChromaMatte.ps1 `
  -InputPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-foreground-mask-source.png `
  -OutputPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-foreground-mask-alpha.png `
  -KeyColor '#ff00ff' -SeedTolerance 24 -FloodTolerance 48 `
  -EdgeRadius 2 -DonorRadius 8 -BlendResidual 24 -MinKeyMix 0.08 -Force
```

- [ ] **Step 2.11 — 세 레이어 preview 계약.** 아래 명령으로 PASS한 background를 복사하고 foreground를 같은 인자로 다시 생성한다. background source/preview SHA와 foreground derivative/preview SHA가 각각 같아야 한다. `a01-office-preview-contract.json`에는 실제로 계산한 세 PNG SHA, `status: preview-only`, canvas, `PPU:100`, `4×5`, `320×320`, 20 pose IDs와 integer anchor를 기록한다. `SOURCE.md`에는 `A01 independent silent previs only`, `not final art`, `A02/B08/Startup not applied`를 기록한다.

```powershell
$a01ExpectedBgSha = 'DA5F22DE7D1C9BDBABE2A8887640085142D23E02CF3BF94B21E217A7EC98AA0C'
$a01CurrentBgSha = (Get-FileHash -Algorithm SHA256 _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-bg-room-source.png).Hash.ToUpperInvariant()
if ($a01CurrentBgSha -cne $a01ExpectedBgSha) { throw 'Canonical background source SHA mismatch before preview copy.' }

Copy-Item -LiteralPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-bg-room-source.png `
  -Destination UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-bg-room-preview.png -Force

pwsh tools/art/Remove-ConnectedChromaMatte.ps1 `
  -InputPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-foreground-mask-source.png `
  -OutputPath UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-foreground-mask-preview.png `
  -KeyColor '#ff00ff' -SeedTolerance 24 -FloodTolerance 48 `
  -EdgeRadius 2 -DonorRadius 8 -BlendResidual 24 -MinKeyMix 0.08 -Force

$a01BgSourceSha = (Get-FileHash -Algorithm SHA256 _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-bg-room-source.png).Hash
$a01BgPreviewSha = (Get-FileHash -Algorithm SHA256 UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-bg-room-preview.png).Hash
$a01FgDerivativeSha = (Get-FileHash -Algorithm SHA256 _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-foreground-mask-alpha.png).Hash
$a01FgPreviewSha = (Get-FileHash -Algorithm SHA256 UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-foreground-mask-preview.png).Hash
if ($a01BgSourceSha -ne $a01BgPreviewSha) { throw 'Background preview SHA mismatch.' }
if ($a01FgDerivativeSha -ne $a01FgPreviewSha) { throw 'Foreground preview SHA mismatch.' }
```

**Active expected gate:** Cast와 foreground가 자동·독립 QA를 모두 PASS하고 세 preview 계약이 고정된 뒤에만 Unity 애니매틱 Task 3을 재개한다. imagegen은 `7/7`, attempt 08은 금지한다.

### Historical attempt 07 기록 — 아래 Task 2 내용은 실행 금지

아래 내용은 imagegen 1~7회와 superseded global normalization 설계의 provenance다. 체크되지 않은 항목과 명령도 active 작업이 아니며, 위 `Active Task 2 override`만 실행한다.

**Owner:** ChatGPT 이미지 아트 에이전트 → Unity 씬/통합 구현 에이전트(알파 도구·테스트 단일 owner) → 비주얼/테크아트 에이전트

**Inputs:**

- Attempt 07 sole spatial reference: `_workspace/active/2026-08-08-opening-cinematic-origin/artifacts/task6/a01-office-base/a01-office-base-candidate-03-spatial-anchor.png`
- Acting reference: `_workspace/active/2026-08-08-opening-cinematic-origin/artifacts/task6/a01-office-base/a01-office-base-candidate-02-character-motion.png` — initial Prompt B용 historical-only이며 attempt 07 입력으로 사용하지 않는다.
- Attempt 07 single source of truth: `docs/superpowers/specs/2026-08-10-a01-cast-regeneration-normalization-design.md` §5 full prompt 및 §6~8 계약
- Reference canvas: `1672×941`, 16:9

**Files:**

- Create: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-bg-room-source.png`
- Create: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png`
- Create: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-normalized-alpha.png`
- Modify conditionally after attempt 07 automatic + independent visual QA PASS: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-source.png` (canonical raw)
- Create: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-foreground-mask-source.png`
- Create: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/generation-log.md`
- Create during implementation: `tools/art/Remove-ConnectedChromaMatte.ps1`
- Create during implementation: `tools/art/Test-RemoveConnectedChromaMatte.ps1`
- Create: `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-bg-room-preview.png`
- Create conditionally after canonical promotion: `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-cast-pose-grid-preview.png`
- Create: `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-foreground-mask-preview.png`
- Create: `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-preview-contract.json`
- Create: `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/SOURCE.md`

**현재 실행 상태와 correction 배정:**

- 초기 usable source 3개 생성 뒤 background·cast·foreground가 독립 시각 리뷰에서 모두 반려됐다.
- 다중 reference Correction A는 spatial reference를 `unsupported image image/png`로 보고 usable 결과 없이 종료됐다. 이 tool invocation도 attempt `4/6`으로 센다.
- invocation 5 background correction은 exact `1672×941`, SHA-256 `DA5F…AA0C`이며 독립 시각 PASS다.
- invocation 6 cast correction은 `1122×1402`, SHA-256 `C3BD…E44AD`이며 `a01-imagegen-correction-visual-review-002`에서 REJECT됐다. width `mod 4 = 2`, height `mod 5 = 2`, row boundary collision, P4 standing/commuter bag drift, literal `#ff00ff` 13 pixels와 near-magenta 1,121,291 pixels로 exact `1280×1600`·`4×5`·`320×320` cell·chroma 계약을 충족하지 못한다.
- attempt 07 actual은 `7/7`, remaining `0`이다. raw `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png`는 `1122×1402`, `1,648,495` bytes, SHA-256 `24A143D7344DAC8358CD496C6AD03718AADB492D67B96E7CCCF0E46DA08A090D`다. independent review는 모든 y band의 horizontal equal-grid boundary collision으로 REJECT했으며, identity·standing·bag 의미는 대체로 PASS지만 global nearest-neighbor로는 고칠 수 없다. fail-fast로 normalization tool/TDD/derivative/canonical/foreground/Unity를 시작하지 않았고 attempt 08은 금지한다.
- foreground source SHA-256 `D782D38E4D510E1D13680C21D6642F86647DF53662B8D94150376EC73770F1E1`는 덮어쓰지 않고 로컬 알파 처리 입력으로 보존한다.
- 기존 background PASS bytes/SHA, foreground source, canonical cast와 Unity preview는 attempt 07 QA PASS 전 byte-for-byte 보존한다. raw versioned 파일과 normalized-alpha derivative를 분리하고, attempt 07 실패면 attempt 08을 호출하지 않는다.

### 승인된 Cast attempt 07 절차 — 이 절차가 아래의 과거 차단·복사 예시와 충돌할 경우 우선

**Attempt 07 full prompt — `2026-08-10-a01-cast-regeneration-normalization-design.md` §5와 동일, one sole spatial reference만 첨부:**

```text
Use case: stylized-concept
Asset type: preview-only A01 pixel-art limited-animation cast pose sheet
Input images: Image 1 is the sole spatial reference for all five worker identities, outfits, proportions, pixel density, camera-facing angles, and bag sides. Do not import identity, clothing, pose, or props from any other source.

Primary request: Create one cast sheet arranged as four columns by five rows. Treat the canvas as 20 invisible equal cells with no visible grid lines. Keep at least 12% of every cell width and height as perfectly flat #ff00ff gutter on all four sides. No person, hair, chair, hand, foot, or bag may touch or cross a cell boundary.

Canvas intent: exactly 1280×1600 pixels, four columns by five rows, each cell exactly 320×320 pixels. Keep all figures centered and consistently scaled inside their own cells.

Row 1 — P1: the blue-shirt man with brown curly hair and glasses, seated in all four cells. Poses: seated idle; small speaking hand gesture; warm laugh; chair-push/rise start.
Row 2 — P2: the woman with a hair bun and olive top, seated in all four cells. Poses: seated idle; delayed nod and smile; short laugh; neutral hold.
Row 3 — P3: dark hair and dark-green clothing, mostly back-facing and seated in all four cells. Poses: seated work; small shoulder laugh; clearly different short head turn; neutral hold.
Row 4 — P4: beige overshirt, standing in all four cells, never seated and never paired with a chair. Preserve the same black personal commuter bag on the same body side shown in Image 1 in every cell. Poses: standing idle; conversational hand gesture; standing turn toward the right exit; standing neutral hold.
Row 5 — P5: rust blouse and cream pants, standing in all four cells, never seated and never paired with a chair. Preserve the same brown personal commuter bag on the same body side shown in Image 1 in every cell. Poses: standing idle; warm laugh; right-exit step-ready pose; standing neutral hold.

Background: every empty pixel must be literal #ff00ff only. No gradient, near-magenta variation, noise, texture, shadow, floor, reflection, halo, or fringe.
Constraints: preserve identity, outfit, scale, pixel density, camera angle, bag identity, and bag side across each row; hard pixel edges; generous separation between cells.
Avoid: visible grid lines, labels, letters, numbers, extra people, extra props, food or lunch bags, text, logo, infection cue, horror cue, boundary crossing, cropped silhouettes, chairs in rows 4 or 5.
```

1. 명세 §5의 full prompt와 A01-C03 spatial reference **한 장만** 사용해 built-in imagegen Cast attempt 07을 한 번 호출한다. 결과를 변경 없이 `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png`에 저장하고 prompt, 입력 reference, dimensions, bytes, SHA-256을 generation log에 기록한다. generation log에는 이후 tool/test보다 먼저 다음 machine-readable line을 정확히 한 번 기록한다.

```text
- attempt-07-raw-sha256: <64 uppercase hex>
```

기존 canonical source 경로에 복사하거나 덮어쓰지 않는다.
2. **TDD 단계 A — 기존 chroma-only 경로.** 아직 두 파일이 없으므로 `Test-RemoveConnectedChromaMatte.ps1`만 먼저 작성하고 `Remove-ConnectedChromaMatte.ps1` 부재의 `tool not found` nonzero를 RED로 기록한다. 그 다음 같은 두 경로만 만들어 기존 foreground chroma/despill contract를 GREEN으로 만든다. 새 tool 쌍이나 새 파일은 만들지 않는다.
3. **TDD 단계 B — 정규화 경로.** chroma-only GREEN tool을 대상으로 test script에 normalization 계약을 추가하고 다시 nonzero RED를 확인한다. 이 RED는 `tool not found`가 아니라 existing tool의 normalization parameter/behavior 미지원이어야 한다. optional 인자는 `-NormalizeWidth 1280 -NormalizeHeight 1600 -GridColumns 4 -GridRows 5 -BoundaryBand 6`이며 하나라도 주면 다섯 개를 모두 요구한다. raw opaque PNG의 4:5 aspect 상대 오차가 `0.005` 이하면 nearest-neighbor global resize만 허용하고, crop·padding-only·cell별 재조립은 금지한다.
4. 단계 B RED 뒤 같은 `Remove-ConnectedChromaMatte.ps1`를 확장해 정상화 후 connected chroma/despill을 적용하고 `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-normalized-alpha.png`만 만든다. 임시 output에서 검사한 뒤 전부 PASS할 때만 output을 교체하고, raw bytes/SHA와 canonical source는 실패에도 불변이어야 한다.
5. 단계 B test는 exact `1280×1600`, `4×5`/`320×320`, alpha `{0,255}`, transparent RGB `(0,0,0)`, 6px cell boundary opaque `0`, cell coverage `5%..60%`, source-SHA 불변, 같은 raw·인자의 derivative SHA 결정성을 검증한다. 단계 A의 normalization 없는 chroma 동작과 real foreground 검사는 계속 PASS해야 한다.
6. 구현 주체와 분리된 비주얼/테크아트가 raw와 derivative의 20 cells를 실제 크기로 검토해 P1~P3 seated, P4/P5 standing, P4/P5 동일 쪽 commuter bag, identity·outfit·pixel quality를 PASS해야 한다. 자동 계약 PASS만으로는 승격하지 않는다.
7. 자동 계약과 독립 visual QA가 모두 PASS한 경우에만 versioned raw를 기존 canonical cast source로 승격하고, canonical raw에서 같은 명령으로 Unity preview를 재생성해 derivative SHA 일치를 확인한다. 그 뒤에만 foreground 도구와 이후 Task 2/Unity 단계를 재개한다. 어느 하나라도 FAIL이면 versioned raw와 판정만 보존하고 foreground·canonical·Unity를 정지한다.

- [x] **Historical completed — 재실행 금지.** 초기 생성 전 두 reference를 `view_image`로 확인하고 사람 수·P1 자리·창·책상 섬·출입문과 1672×941 canvas를 로그에 기록했다.
- [x] **Historical completed — 재실행 금지.** 아래 Prompt A~C로 OpenAI 내장 `imagegen` 초기 3회를 사용했다. 세 결과는 독립 시각 리뷰에서 반려됐으며, 이 체크박스를 다시 실행해 invocation을 늘리지 않는다.

**Prompt A — clean room plate**

```text
Edit the attached A01-C03 spatial reference into a clean background plate for a 2D pixel-art motion-comic game cinematic. Preserve the exact 16:9 framing, camera axis, 1672×941 composition, large left window and daytime city, central desk island and monitors, P1 chair and desk continuity anchor, wall clock and shelf, open right door and corridor, foreground desk edge, warm ordinary lunch-time office mood, palette, pixel density, and lighting direction. Remove all five people completely and reconstruct every occluded wall, desk, chair, floor, and corridor area. Keep P1's chair clearly readable for later B08 comparison. No food bags, no text, no logo, no UI, no infection clue, no cough, no mask, no purple virus motif, no particles, no ominous lighting. Output one clean full-frame pixel-art background, not a mockup and not a character sheet.
```

**Historical Prompt B — cast pose grid (historical-only, 재실행 금지; acting reference와 함께 attempt 07 입력으로 사용하지 않음)**

```text
Use the attached A01-C03 spatial reference for the exact five office-worker identities, clothing palette, proportions, pixel density, and camera-facing angle; use the attached A01-C02 reference only for natural conversational acting. Create one clean 5-row by 4-column pixel-art pose grid on a perfectly uniform #ff00ff background. No grid lines, labels, letters, numbers, props outside each character cell, shadows on the background, or extra people. Keep each identity, outfit, scale, bottom-center foot or seated pivot, and outline consistent across its row. Row 1 P1: seated idle, speaking with one small hand gesture, warm laugh, chair-push/rise start. Row 2 P2: seated idle, delayed nod/smile, short laugh, neutral hold. Row 3 P3: seated work seen mostly from behind, small shoulder laugh, short head turn, neutral hold. Row 4 P4: standing idle with bag, conversational hand gesture, body turned toward right exit, neutral hold. Row 5 P5: standing idle, warm laugh, right-exit step-ready pose, neutral hold. Friendly ordinary coworkers, limited-frame animation poses, no exaggerated anatomy, no rubber limbs, no food bag, no text, no logo, no infection or horror cue.
```

**Prompt C — occluding foreground plate**

```text
Edit the attached A01-C03 spatial reference into a full-canvas occlusion plate aligned to the exact original 1672×941 camera and coordinates. Keep only the opaque foreground surfaces that must naturally cover character lower bodies: central desk fronts and edges, monitor and stationery silhouettes that sit in front of the seated cast, and the bottom foreground desk and props. Reconstruct their hidden edges cleanly. Remove every person and every background element that is not an occluding foreground surface. Use a perfectly uniform #ff00ff background for all empty pixels. Preserve the reference pixel density, warm palette, and hard pixel edges. No shadows detached from the retained objects, no text, no logo, no food bag, no particles, no infection or horror cue.
```

**Historical invocation 5 — background correction, spatial reference only (PASS, 재실행 금지)**

```text
Use case: stylized-concept. Asset type: preview-only A01 motion-comic clean room plate. Input images: Image 1 is the sole A01-C03 spatial identity and coordinate reference. Create exactly 1672×941 pixels. Preserve the exact camera, large left window and daytime city, central desk island, wall clock, shelf, open right door and corridor, foreground desk edge, palette, pixel density, and ordinary warm lunch-time lighting. Remove all five people. P1 is the face-visible seated man with brown curly hair, glasses, and a blue shirt behind the central island. Reconstruct P1's own chair and readable backrest at approximately x=712..795, y=454..525, distinct from P3's large foreground chair and from every monitor; do not replace that chair anchor with a monitor. Reconstruct all newly exposed desk, wall, floor, chair, and corridor pixels. No people, text, logo, UI, infection clue, cough, mask, virus motif, particles, food bag, or ominous lighting. Output one full-frame clean pixel-art background plate only.
```

**Historical invocation 6 — cast correction, spatial reference only (REJECT, 재실행 금지)**

```text
Use case: stylized-concept. Asset type: preview-only A01 limited-animation cast pose sheet. Input images: Image 1 is the sole source of all five worker identities, outfits, proportions, pixel density, and camera-facing angles; do not import identities from any acting reference. Output exactly 1280×1600 pixels as four columns by five rows, exactly 320×320 pixels per cell, with generous padding and no pose crossing a cell boundary. Row 1 P1: brown curly hair, glasses, blue-shirt man — seated idle, small speaking gesture, warm laugh, chair-push/rise start. Row 2 P2: hair bun, olive-green top woman — seated idle, delayed nod/smile, short laugh, neutral hold. Row 3 P3: dark hair and dark-green clothing, mostly back-facing — seated work, small shoulder laugh, clearly different short head turn, neutral hold. Row 4 is spatial-reference P4 in four corresponding limited poses. Row 5 is spatial-reference P5 in four corresponding limited poses; preserve P5's existing personal commuter shoulder bag from the spatial reference on the same side in every cell. That personal commuter bag is required and is not the forbidden lunch/food bag. Do not add any lunch bag, takeout bag, grocery bag, food container, or other food-carrying prop. Every empty pixel must be literal #ff00ff with no gradient, noise, blend, shadow, floor, or fringe. Hard pixel edges only. No grid lines, labels, letters, numbers, extra people, text, logo, infection, or horror cue.
```

- [x] **Historical completed — 재실행 금지.** 초기 3회 + no-result 1회 + background/cast correction 2회의 invocation ledger는 `6/6`이며, 동일 prompt 재시도와 foreground 재생성은 금지한다.
- [x] **Historical completed — 재실행 금지.** 초기 background/cast/foreground 3개 반환 경로는 packet source에 복사하고 SHA·검토 이력을 기록했다. 이 단계에서 기존 foreground source를 다시 생성·복사·덮어쓰지 않는다.
- [x] **Historical completed — 재실행 금지.** 두 correction 결과의 local path·dimensions·SHA를 검사했다. background는 PASS, cast는 REJECT이며 rejected cast를 alpha/preview/Unity 단계로 넘기지 않는다. foreground source SHA-256은 불변이다.

Prompt A~C와 invocation 5/6의 과거 copy recipe는 historical record일 뿐이며, 실행 가능한 명령은 이 계획에 남기지 않는다. attempt 07에는 사용하지 않으며, 위 승인 절차의 versioned raw/canonical 분리 규칙을 대체하지 않는다.

- [x] **Historical completed — 재실행 금지.** 초기 source 3개의 해상도·구성·시각 검사를 수행했고 세 결과를 반려했다. 과거 검사 체크박스를 재실행하거나 과거 Prompt A~C를 다시 호출하지 않는다.
- [x] corrected background는 exact `1672×941` PASS, corrected cast는 `1122×1402`로 exact `1280×1600`·`4×5`·각 cell exact `320×320` FAIL, 보존 foreground SHA-256은 불변임을 `a01-imagegen-correction-visual-review-002`로 확인했다. 이는 attempt 07 이전의 historical 6회 기록이며 재실행하지 않는다.
- [x] **attempt 07 단발 실행 완료 — REJECT, 재실행 금지.** raw는 all y-band horizontal equal-grid boundary collision으로 independent REJECT됐고, 사용자 채팅에서 추가 imagegen 없는 deterministic per-cell repack 방향과 active override 구현을 승인했다. attempt 08은 금지한다.

- [ ] **TDD 단계 A RED.** `tools/art/Test-RemoveConnectedChromaMatte.ps1`만 먼저 작성하고 다음 foreground chroma-only 명령이 production tool 부재로 nonzero이며 `tool not found`를 출력하는지 확인한다. Pester에 의존하지 않고 자체 assert와 process exit code를 사용한다.

```powershell
pwsh tools/art/Test-RemoveConnectedChromaMatte.ps1 `
  -ToolPath tools/art/Remove-ConnectedChromaMatte.ps1 `
  -RealSourcePath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-foreground-mask-source.png `
  -ExpectedRealSourceSha256 D782D38E4D510E1D13680C21D6642F86647DF53662B8D94150376EC73770F1E1
```

- [ ] **TDD 단계 A GREEN.** `tools/art/Remove-ConnectedChromaMatte.ps1`를 chroma-only 최소 구현한다. CLI는 mandatory `-InputPath`, `-OutputPath`와 optional `-KeyColor '#ff00ff'`, `-SeedTolerance 24`, `-FloodTolerance 48`, `-EdgeRadius 2`, `-DonorRadius 8`, `-BlendResidual 24`, `-MinKeyMix 0.08`, `-Force`만 노출한다. input/output canonical path가 같거나 input이 불투명 PNG가 아니거나 범위가 잘못되거나 기존 output에 `-Force`가 없으면 nonzero로 실패한다.
- [ ] 구현 알고리즘은 `System.Drawing.Bitmap`을 `Format32bppArgb`로 복제하고 `LockBits`/`Marshal.Copy` byte buffer에서만 처리한다. RGB의 key 거리 `d∞ = max(|R-Kr|, |G-Kg|, |B-Kb|)`를 계산하고 `d∞ <= 24`인 모든 strong seed에서 4-neighbor로 `d∞ <= 48` 영역만 flood한다. 이 mask만 alpha `0`, RGB `(0,0,0)`으로 만들고 나머지는 alpha `255`를 유지한다. 단순 threshold를 `96` 이상으로 올리는 방식은 mug handle 침식 반례 때문에 금지한다.
- [ ] retained pixel 중 transparent mask의 Chebyshev 거리 `2` 이내인 edge만 despill 후보로 본다. 후보마다 radius `8` 안에서 transparent mask 거리 `>2`이고 key 거리 `>96`인 가장 가까운 retained donor를 squared distance, `y`, `x` 순으로 결정한다. 후보색이 donor→key 선분의 projection `t`에서 `0.08 <= t <= 0.92`이고 재구성 RGB의 채널별 최대 오차가 `<=24`일 때만 donor RGB로 치환한다. alpha와 silhouette은 바꾸지 않으며 조건을 만족하지 않는 RGB는 byte-for-byte 보존한다.
- [ ] test script는 TEMP 안에서 synthetic fixtures를 만들고 다음을 자체 assert한다: 배경과 닫힌 handle 내부 key component가 alpha `0`; teal handle과 brown object silhouette은 alpha `255`; key와 donor의 혼합 edge는 donor RGB로 decontaminate; blend가 아닌 purple edge는 RGB 불변; alpha 값은 `{0,255}`뿐; transparent RGB는 black; 같은 입력을 두 번 처리한 SHA-256이 동일; source SHA-256 불변; source=output·invalid PNG·overwrite without `-Force`는 nonzero.
- [ ] real foreground acceptance는 source SHA 고정, `1672×941`, `(0,0)`, `(800,300)`, `(800,750)`, mug handle hole `(1208,835)` alpha `0`; monitor `(230,520)`, lower desk `(800,900)`, mug body/handle `(1160,830)`, `(1200,820)`, `(1214,842)`, `(1204,850)` alpha `255`; hard alpha와 deterministic SHA 일치를 검사한다. 실제 source에서 decontaminated pixel이 `>0`이고 같은 blend-line 판정의 unresolved opaque fringe가 `0`이어야 한다.
- [ ] **TDD 단계 B RED.** 이 code block 자체에서 generation log의 literal attempt 07 SHA를 exactly-one uppercase-64 regex로 읽고 current raw와 fail-fast 대조한 뒤, 단계 A GREEN tool을 대상으로 normalization 계약 test를 nonzero로 실행한다. 실패 원인은 `tool not found`가 아니라 normalization parameter/behavior 미지원이어야 한다.

```powershell
$a01GenerationLogPath = '_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/generation-log.md'
$a01Attempt07RawPath = '_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png'
$a01GenerationLogText = Get-Content -Raw -LiteralPath $a01GenerationLogPath
$a01Attempt07ShaMatches = [regex]::Matches($a01GenerationLogText, '(?m)^- attempt-07-raw-sha256: ([0-9A-F]{64})\r?$')
if ($a01Attempt07ShaMatches.Count -ne 1) { throw 'generation-log.md must contain exactly one uppercase attempt-07 raw SHA line.' }
$a01Attempt07ExpectedSha = $a01Attempt07ShaMatches[0].Groups[1].Value
$a01Attempt07CurrentSha = (Get-FileHash -Algorithm SHA256 -LiteralPath $a01Attempt07RawPath).Hash.ToUpperInvariant()
if ($a01Attempt07CurrentSha -cne $a01Attempt07ExpectedSha) { throw 'Attempt 07 raw SHA does not match the generation-log provenance line.' }

pwsh tools/art/Test-RemoveConnectedChromaMatte.ps1 `
  -ToolPath tools/art/Remove-ConnectedChromaMatte.ps1 `
  -RealSourcePath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png `
  -ExpectedRealSourceSha256 $a01Attempt07ExpectedSha `
  -NormalizeWidth 1280 -NormalizeHeight 1600 -GridColumns 4 -GridRows 5 -BoundaryBand 6
```

- [ ] **TDD 단계 B GREEN 및 derivative 자동 QA.** 이 code block도 별도 shell call로 완결되도록 generation-log literal SHA를 다시 regex 검증하고 current raw와 대조한 뒤, 같은 tool로 versioned normalized-alpha derivative와 test를 실행한다. test는 동일 인자로 두 번 실행한 SHA-256, source SHA 불변, exact normalize/alpha/grid/boundary/coverage를 PASS해야 한다. 실패 시 output·canonical source·Unity preview를 변경하지 않는다.

```powershell
$a01GenerationLogPath = '_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/generation-log.md'
$a01Attempt07RawPath = '_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png'
$a01GenerationLogText = Get-Content -Raw -LiteralPath $a01GenerationLogPath
$a01Attempt07ShaMatches = [regex]::Matches($a01GenerationLogText, '(?m)^- attempt-07-raw-sha256: ([0-9A-F]{64})\r?$')
if ($a01Attempt07ShaMatches.Count -ne 1) { throw 'generation-log.md must contain exactly one uppercase attempt-07 raw SHA line.' }
$a01Attempt07ExpectedSha = $a01Attempt07ShaMatches[0].Groups[1].Value
$a01Attempt07CurrentSha = (Get-FileHash -Algorithm SHA256 -LiteralPath $a01Attempt07RawPath).Hash.ToUpperInvariant()
if ($a01Attempt07CurrentSha -cne $a01Attempt07ExpectedSha) { throw 'Attempt 07 raw SHA does not match the generation-log provenance line.' }

pwsh tools/art/Remove-ConnectedChromaMatte.ps1 `
  -InputPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png `
  -OutputPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-normalized-alpha.png `
  -KeyColor '#ff00ff' -SeedTolerance 24 -FloodTolerance 48 `
  -EdgeRadius 2 -DonorRadius 8 -BlendResidual 24 -MinKeyMix 0.08 `
  -NormalizeWidth 1280 -NormalizeHeight 1600 -GridColumns 4 -GridRows 5 -BoundaryBand 6 -Force

pwsh tools/art/Test-RemoveConnectedChromaMatte.ps1 `
  -ToolPath tools/art/Remove-ConnectedChromaMatte.ps1 `
  -RealSourcePath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png `
  -ExpectedRealSourceSha256 $a01Attempt07ExpectedSha `
  -NormalizeWidth 1280 -NormalizeHeight 1600 -GridColumns 4 -GridRows 5 -BoundaryBand 6
```

- [ ] 비주얼/테크아트 에이전트가 attempt 07 raw와 normalized-alpha derivative의 `5×4` 동일 셀 분리, identity drift, P1~P3 seated, P4/P5 standing·same-side commuter bag, alpha 모서리, magenta fringe를 실제 크기에서 확인한다. pose가 셀 경계를 건드리거나 자동 계약 또는 시각 QA 하나라도 FAIL이면 반려한다.
- [ ] **승격 — 자동 QA와 독립 visual QA 모두 PASS한 경우에만.** versioned raw를 canonical cast source로 promote하고, canonical raw에서 **동일 normalization/chroma 인자**로 Unity preview를 다시 만든 뒤 versioned derivative SHA와 Unity preview SHA가 일치하는지 확인한다. 하나라도 FAIL이면 promote/Unity/foreground를 수행하지 않는다.

```powershell
Copy-Item `
  -LiteralPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-raw.png `
  -Destination _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-source.png `
  -Force

pwsh tools/art/Remove-ConnectedChromaMatte.ps1 `
  -InputPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-source.png `
  -OutputPath UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-cast-pose-grid-preview.png `
  -KeyColor '#ff00ff' -SeedTolerance 24 -FloodTolerance 48 `
  -EdgeRadius 2 -DonorRadius 8 -BlendResidual 24 -MinKeyMix 0.08 `
  -NormalizeWidth 1280 -NormalizeHeight 1600 -GridColumns 4 -GridRows 5 -BoundaryBand 6 -Force

$a01DerivativeSha = (Get-FileHash -Algorithm SHA256 _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-cast-pose-grid-attempt-07-normalized-alpha.png).Hash
$a01PreviewSha = (Get-FileHash -Algorithm SHA256 UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview/a01-office-cast-pose-grid-preview.png).Hash
if ($a01DerivativeSha -ne $a01PreviewSha) { throw 'Canonical Unity preview SHA must equal attempt 07 derivative SHA.' }
```

- **Historical / execution prohibited:** 이전 foreground chroma/despill command, `a01-office-preview-contract.json`의 three-PNG/`deskMaskRect`/`propFrontRect`, `SOURCE.md` preview-copy 절차는 실행하지 않는다. 이는 아래 active override의 보존 source→alpha derivative 복구와 별개이며, Preview art 생성·복제와 old three-PNG contract는 계속 금지다.

**Historical Fix 2 재확인:** original finding 3건을 다시 대조한 이력이다. Task 2 Files에는 versioned raw와 normalized-alpha derivative가 있고 canonical/Unity preview는 QA PASS 뒤 conditional이었다. attempt 07은 sole spatial reference·§5 full prompt·12% gutter·P1~P5/P4·P5 standing/bag invariant만 사용했다. raw→generation-log literal SHA provenance preflight→stage B derivative 자동 QA→독립 visual QA→canonical promote→Unity preview SHA 대조→foreground 순서는 현재 실행 절차가 아니며, scene integration은 아래 active override만 사용한다.

**Historical expected gate:** 이 문단은 superseded global normalization 이력이며 현재 상태 판정에 사용하지 않는다. 현재 gate는 아래 `a01-scene-integration-001` active override의 S0→TDD→independent frozen Play/scene verification이다.

**Historical checkpoint:** imagegen artifacts, preview files와 foreground를 함께 commit하던 절차는 실행 금지다. `a01-scene-integration-001`은 새 Preview art를 만들지 않고 active override의 허용 source·staging 경계를 따른다.

---

## ACTIVE EXECUTION OVERRIDE — `a01-scene-integration-001`

> **상태:** **기술 재분류 — mask-only occlusion**. color foreground visual QA `2/2` 뒤 `a01-foreground-occlusion-mask-reclass-001` current `0/2`, scene production/test `0/2`다. 아래 mask-only 계약이 visible foreground 관련 유일한 active 기준이다.

**허용 source와 생성 경계**

- Unity source는 현재 `Assets/_Project/Art/Cinematics/Opening/A01/Office/a01-office-background-v1.png`, `Assets/_Project/Art/Cinematics/Opening/A01/Office/a01-office-cast-poses-v1.png`, `Assets/_Project/Art/Cinematics/Opening/A01/Office/a01-office-assets-v1.manifest.json`뿐이다. checkout에서는 같은 세 path의 `UnityProject/` 접두어만 사용한다.
- S0 recovery 입력은 보존된 `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-foreground-mask-source.png` 하나이며 SHA-256 `D782D38E4D510E1D13680C21D6642F86647DF53662B8D94150376EC73770F1E1`는 불변이다. 새 imagegen·retry·외부 생성, `Preview/` art copy, old three-PNG/preview contract와 `DESK_Mask`/`PROP_Front`는 계속 금지한다.
- 기존 color recovery RED/GREEN과 두 rejected derivative SHA는 verification 이력으로만 보존한다. 현재 test에 `MaskOnly` targeted case를 먼저 추가해 현재 tool의 mask-only 미지원 RED를 1회 확인하고, production tool에 `-MaskOnly`를 최소 구현해 GREEN 1회만 실행한다.

```powershell
pwsh tools/art/Test-RemoveConnectedChromaMatte.ps1 -ToolPath tools/art/Remove-ConnectedChromaMatte.ps1 -RealSourcePath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-foreground-mask-source.png -ExpectedSourceSha D782D38E4D510E1D13680C21D6642F86647DF53662B8D94150376EC73770F1E1 -ExpectedOutputSha F59EBC810A943DB76C17691AD364237F473BAB6A97EF3A8966321BAEF8400D95 -TargetedCase MaskOnly
```

- GREEN 뒤 아래 actual command로 mask derivative를 만든다. output은 `1672×941`, hard alpha `{0,255}`, transparent RGB black, opaque RGB white, repeated SHA determinism, source SHA 불변을 만족해야 한다. color foreground derivative는 production으로 승격하지 않는다.

```powershell
pwsh tools/art/Remove-ConnectedChromaMatte.ps1 -InputPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-foreground-mask-source.png -OutputPath _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/a01-office-occlusion-mask-alpha.png -KeyColor '#FF00FF' -StrongTolerance 24 -FloodTolerance 48 -DespillTolerance 96 -MaskOnly
```

- 독립 mask silhouette QA PASS 뒤에만 mask를 `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Office/a01-office-occlusion-mask-v1.png`로 byte-identical promote한다. manifest는 `foreground_status: mask-only-candidate`, mask source/derived path·SHA·bytes와 Single·PPU100·Point·mipmap false·Uncompressed·alpha true·fallback physics false를 기록한다. 그때만 static/import tests를 mask 포함 계약으로 바꾼다.

**장면·Timeline·launcher 계약**

- scene은 `VisualRoot/FG_BackgroundRepeat`(승인 BG와 동일 sprite/transform, order 200, `maskInteraction=VisibleInsideMask`)와 `VisualRoot/FG_OcclusionMask`(exact mask asset의 `SpriteMask`, custom range가 CHAR보다 위의 repeat만 제어)를 사용한다. base BG와 repeat의 sprite·transform equality, mask path/PPU, CHAR 가림과 scene prohibition을 tests가 고정한다. rejected foreground RGB는 어떤 renderer/material에도 연결하지 않는다.
- Timeline은 `24 fps`, `preview-measurement-only`, `IsFinalTiming == false`, `[36,36,36,30,42,24]` frames, total `204` frames만 사용한다. pose swap과 절제된 camera 이동은 P1 발화→지연 반응→함께 웃음→점심 이동 준비를 읽게 하되 최종 러닝타임·아트로 선언하지 않는다.
- menu `Last Host/Cinematics/A01/Rebuild Preview`와 `Last Host/Cinematics/A01/Play Preview`를 제공한다. Play launcher는 저장되지 않은 scene 취소 시 무변경으로 중지하고, 승인 시 SceneSetup·active scene·`playModeStartScene`을 SessionState에 snapshot한 뒤 A01만 연다. EditMode 복귀·중단은 snapshot과 start scene을 복원하고 SessionState를 clear한다.
- S0와 tests는 source SHA 불변, foreground derivative의 hard alpha·transparent black·determinism·fringe no-regression과 QA PASS 전 scene/Unity 금지, QA PASS 뒤 one full-frame `FG_Occlusion`/BG+P1~P5/one camera/one director, 24fps/204 schedule, no Audio/Text/Particle, scene Build Settings 미포함, Startup·package·ProjectSettings·protected LF-normalized hashes 불변, launcher restore를 고정한다.

**TDD·비용 계약**

- mask-only cycle은 targeted RED1→GREEN1→actual mask1→mask QA1 순서다. PASS 뒤 production/meta/manifest/static test를 전이하고, static PASS 뒤에만 scene target RED·owner GREEN Unity run·independent frozen Play/scene QA를 진행한다.
- actual Unity/high-cost는 현재 `3`에서 시작하고, 이 승인 문서 커밋은 Unity·code·asset·tool을 실행하지 않는다. run004 asset import evidence는 asset bytes 불변 조건에서 재사용하며 현재 비용 판정 `주의`를 유지한다.

**Historical execution prohibition**

- 아래 Task 3~6의 old 3-PNG import/foreground chroma/despill, `Preview/` art copy, `a01-office-preview-contract.json`, `DESK_Mask`/`PROP_Front`, preview-art staging/commit, old frozen-full command는 historical이며 실행 금지다. 보존 source→`a01-office-foreground-mask-alpha.png`→QA-PASS byte-identical Office foreground의 현재 recovery만 예외이며, 이 override와 충돌하는 Files, commands, checkboxes, expected gates는 사용하지 않는다.

---

## Task 3 (Historical reference — execution prohibited): 시네마틱 Editor 계약과 실패하는 EditMode 테스트 작성

**Owner:** Unity 씬/통합 구현 에이전트

**Files:**

- Create: `UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01/LastHost.Prototype.Cinematics.A01.Editor.asmdef`
- Create: `UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01/A01OfficeAnimaticContract.cs`
- Create: `UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01/A01OfficePreviewSession.cs`
- Create: `UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01/LastHost.Prototype.Cinematics.A01.Tests.asmdef`
- Create: `UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01/A01OfficeAnimaticContractTests.cs`
- Create: `UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01/A01OfficePreviewSessionTests.cs`
- Create: `UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01/A01OfficeAssetImportTests.cs`
- Create: `UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01/A01OfficeSceneContractTests.cs`
- Create: `UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01/A01OfficeProtectedBaselineTests.cs`

- [ ] Editor asmdef를 `includePlatforms: [Editor]`, `autoReferenced: true`, reference `Unity.Timeline`으로 만든다. Test asmdef는 Editor assembly, `LastHost.Prototype`, `Unity.Timeline`, `UnityEngine.TestRunner`, `UnityEditor.TestRunner`만 참조하고 `UNITY_INCLUDE_TESTS`를 요구한다.
- [ ] 테스트 파일을 먼저 작성한다. compile scaffold에는 type·member signature와 `throw new NotImplementedException()`만 두고 동작을 구현하지 않는다.
- [ ] `A01OfficeAnimaticContract`의 public API를 다음으로 고정한다.

```csharp
public enum A01OfficeBeatId
{
    SpaceRead,
    Speaker,
    ReactionSpread,
    SharedLaugh,
    LunchTransition,
    Handoff
}

public readonly struct A01OfficeBeatWindow
{
    public A01OfficeBeatId Id { get; }
    public int StartFrame { get; }
    public int DurationFrames { get; }
    public int EndFrame { get; }
}

public static class A01OfficeAnimaticContract
{
    public const int TimelineFps = 24;
    public const int PoseFps = 12;
    public const bool IsFinalTiming = false;
    public const string TimingStatus = "preview-measurement-only";
    public const string ScenePath = "Assets/_Project/Scenes/Cinematics/Opening/A01OfficeAnimatic.unity";
    public const string TimelinePath = "Assets/_Project/Timelines/Cinematics/Opening/A01/A01OfficeAnimatic.playable";
    public const string PreviewArtRoot = "Assets/_Project/Art/Cinematics/Opening/A01/Preview";
    public const string StartupScenePath = "Assets/_Project/Scenes/Startup.unity";
    public static IReadOnlyList<A01OfficeBeatWindow> CreatePreviewSchedule();
    public static IReadOnlyList<A01OfficeBeatWindow> BuildSchedule(IReadOnlyList<int> durationFrames);
}
```

- [ ] contract tests를 4개로 고정한다: six beat order와 `[36,36,36,30,42,24]`; cumulative frame windows와 total 204/8.5s; 0·음수·6개 이외 duration 거부; `IsFinalTiming == false`와 경로 계약.
- [ ] session tests를 4개로 고정한다: scene setup snapshot round-trip; previous start scene path round-trip; active session consume-and-clear; interrupted edit-mode recovery가 Startup start scene과 원래 scene setup을 복원.
- [ ] import tests를 3개로 고정한다: preview contract와 3 PNG 존재·SHA 일치; Point/no mipmaps/uncompressed/PPU 100; cast grid의 exact 20 sprite names와 bottom-center pivot.
- [ ] scene tests를 5개로 고정한다: exact hierarchy/one camera/one director/five cast roots; exact Timeline tracks와 204-frame duration; no AudioTrack/AudioSource/ParticleSystem/UI text; sorting/mask/anchor bands; A01 scene not in Build Settings and Startup first two entries unchanged.
- [ ] protected baseline tests를 2개로 고정한다: package manifest/lock의 **LF-normalized canonical** hash; EditorBuildSettings/Startup bootstrap/StartupController의 **LF-normalized canonical** hash와 현재 단계에 필요한 folder `.meta` 존재. `A01OfficeProtectedBaselineTests.cs`는 byte-level CRLF→LF helper와 lone-CR failure를 공유하고 raw `Get-FileHash` 직접 판정을 금지한다. 총 target count는 `18`이다.
- [ ] compile scaffold와 tests를 저장한 뒤 agent `unity-scene-integration`, run ID `a01-red-meta-refresh-001` lease 아래 source Unity의 `AssetDatabase.Refresh()`만 실행하고 즉시 Release한다. `$a01RedFolderMetaPaths` 9개가 모두 생기지 않으면 fingerprint 전에 중지한다.
- [ ] RED 후보 fingerprint를 만든다. 아래 명령의 ScenePath는 아직 없는 scene도 입력해야 하므로 RED 단계에서는 ScenePath를 생략하고 contract/editor/test/art만 fingerprint한다.

```powershell
$a01ArtifactRoot = '_workspace/active/2026-08-10-a01-office-animatic/artifacts'
$a01RunId = 'a01-contract-red-001'
pwsh tools/verification/Get-VerificationFingerprint.ps1 `
  -ProjectRoot . `
  -ProductionPath $a01RedProductionPaths `
  -TestPath 'UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01' `
  -ScenePath 'UnityProject/Assets/_Project/Scenes/Startup.unity','UnityProject/ProjectSettings/EditorBuildSettings.asset' `
  -PackagePath 'UnityProject/Packages/manifest.json','UnityProject/Packages/packages-lock.json' `
  -VersionPath 'UnityProject/ProjectSettings/ProjectVersion.txt' `
  -RunId $a01RunId `
  -ManifestPath "$a01ArtifactRoot/fingerprint-$a01RunId.json"
```

- [ ] manifest의 실제 fingerprint를 읽은 뒤 `verification-current-state.json`을 `status: ready-for-verification`, 동일 run ID/fingerprint, cost counters 0으로 `apply_patch`한다. 이전 결과나 임의 문자열을 재사용하지 않는다.
- [ ] RED 직전에 agent `unity-scene-integration`, run ID `a01-contract-red-001`로 lease를 획득한다. wrapper tool call은 60초 이내에 yield하고, 완료될 때까지 매 yield마다 lease를 Renew한다. 결과가 failure여도 `finally` 단계에서 같은 agent/run으로 Release한다.
- [ ] wrapper로 target namespace를 실행한다. `A01OfficeAnimaticContractTests` 또는 scene/asset 존재 계약이 실패하고 wrapper가 nonzero여야 RED가 성립한다.

```powershell
$a01TempRoot = Join-Path $env:TEMP 'last-host-a01-office-animatic'
$a01Fingerprint = (Get-Content -Raw -LiteralPath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/fingerprint-a01-contract-red-001.json' | ConvertFrom-Json).candidate_fingerprint
if ([string]::IsNullOrWhiteSpace($a01Fingerprint)) { throw 'Fingerprint manifest is missing candidate_fingerprint.' }
New-Item -ItemType Directory -Force -Path $a01TempRoot | Out-Null
pwsh tools/verification/Invoke-HighCostVerification.ps1 `
  -WorkId 2026-08-10-a01-office-animatic `
  -CriterionId A01-TDD-CONTRACT `
  -Route UnityEditMode `
  -RunId a01-contract-red-001 `
  -CandidateFingerprint $a01Fingerprint `
  -LedgerPath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/verification-attempt-ledger.json' `
  -AgentBriefPath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/agent-brief.json' `
  -CurrentStatePath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/verification-current-state.json' `
  -QaHarnessPath 'UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01' `
  -ContractBaselinePath 'UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01' `
  -ProductionPath $a01RedProductionPaths `
  -TestPath 'UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01' `
  -SourceProjectPath UnityProject `
  -CacheRoot (Join-Path $env:TEMP 'last-host-unity-cache') `
  -UnityPath 'C:/Program Files/Unity/Hub/Editor/6000.4.6f1/Editor/Unity.exe' `
  -ResultsPath (Join-Path $a01TempRoot 'contract-red-results.xml') `
  -LogPath (Join-Path $a01TempRoot 'contract-red-unity.log') `
  -TestFilter 'LastHost.Prototype.Cinematics.A01.Tests' `
  -TimeoutSeconds 1800
```

- [ ] RED XML과 예상 실패명을 `verification.md`에 기록하고 production 구현 전 실패임을 명시한다. 결과 XML 미생성·컴파일 중단은 좋은 RED가 아니므로 test/scaffold만 최소 수정하고 correction budget을 기록한다.
- [ ] RED wrapper 종료 직후 lease `Status`로 owner/run을 대조하고 같은 identity로 Release됐는지 확인한다.

**Expected gate:** target `18` 중 최소 1개 assertion failure, NUnit3 XML 존재, skipped/inconclusive 0. production 동작은 아직 없음.

---

## Task 4 (Historical reference — execution prohibited): 결정적 importer·scene builder·preview launcher 구현

**Owner:** Unity 씬/통합 구현 에이전트

**Files:**

- Modify: `UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01/A01OfficeAnimaticContract.cs`
- Modify: `UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01/A01OfficePreviewSession.cs`
- Create: `UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01/A01OfficePreviewAssetImporter.cs`
- Create: `UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01/A01OfficeAnimaticSceneBuilder.cs`
- Create: `UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01/A01OfficePreviewLauncher.cs`
- Generate through builder: `UnityProject/Assets/_Project/Scenes/Cinematics/Opening/A01OfficeAnimatic.unity`
- Generate through builder: `UnityProject/Assets/_Project/Timelines/Cinematics/Opening/A01/A01OfficeAnimatic.playable`
- Generate through builder: `UnityProject/Assets/_Project/Animations/Cinematics/Opening/A01/Preview/A01_Camera_Preview.anim`
- Generate through builder: `UnityProject/Assets/_Project/Animations/Cinematics/Opening/A01/Preview/A01_Background_Preview.anim`
- Generate through builder: `UnityProject/Assets/_Project/Animations/Cinematics/Opening/A01/Preview/A01_P1_Preview.anim`
- Generate through builder: `UnityProject/Assets/_Project/Animations/Cinematics/Opening/A01/Preview/A01_P2_Preview.anim`
- Generate through builder: `UnityProject/Assets/_Project/Animations/Cinematics/Opening/A01/Preview/A01_P3_Preview.anim`
- Generate through builder: `UnityProject/Assets/_Project/Animations/Cinematics/Opening/A01/Preview/A01_P4_Preview.anim`
- Generate through builder: `UnityProject/Assets/_Project/Animations/Cinematics/Opening/A01/Preview/A01_P5_Preview.anim`

- [ ] `A01OfficePreviewAssetImporter.Apply()`가 contract JSON의 실제 SHA와 grid를 검증한 뒤 background를 Single Sprite, cast와 foreground를 Multiple Sprite로 import하게 구현한다. foreground는 contract의 non-overlap rect 두 개를 `DESK_Mask`, `PROP_Front`로 자른다. 세 texture 모두 Point, mipmap off, uncompressed, PPU 100, FullRect이며 alpha asset은 alpha transparency를 사용한다.
- [ ] 5×4 cast grid를 동일 cell rect 20개로 자르고 Task 3의 exact pose names를 row-major로 부여한다. cell 안 opaque pixel이 없거나 opaque pixel이 cell border에 닿으면 stable diagnostic으로 실패한다.
- [ ] `A01OfficeAnimaticSceneBuilder.RebuildPreview()`를 menu `Last Host/Cinematics/A01/Rebuild Preview`에 등록한다. YAML을 직접 편집하지 않고 Unity API로 asset을 생성·갱신한다.
- [ ] `AssetDatabase.Refresh()` 뒤 새 PNG·JSON·SOURCE·Editor/Test/Scene/Timeline/Animation 각각의 file `.meta`, `$a01AllFolderMetaPaths` 20개와 scene file `.meta`가 생겼는지 확인한다. Unity가 생성한 `.meta`를 누락하거나 수동으로 임의 GUID를 작성하지 않는다.
- [ ] scene hierarchy를 다음으로 고정한다. sorting layer를 새로 만들지 않고 `Default`의 explicit order만 사용한다.

```text
A01_Office_Animatic
├─ CameraRig
│  └─ Main Camera                 Camera + Animator
├─ VisualRoot
│  ├─ BG_Room                    SpriteRenderer order 0 + Animator
│  ├─ CHAR_P1                    Animator + SpriteRenderer order 110
│  ├─ CHAR_P2                    Animator + SpriteRenderer order 100
│  ├─ CHAR_P3                    Animator + SpriteRenderer order 120
│  ├─ CHAR_P4                    Animator + SpriteRenderer order 130
│  ├─ CHAR_P5                    Animator + SpriteRenderer order 140
│  ├─ DESK_Mask                  foreground sheet deskMaskRect, order 200
│  ├─ PROP_Front                 foreground sheet propFrontRect, order 300
│  └─ FX_Ambient                 empty, inactive, no ParticleSystem
└─ Timeline                      PlayableDirector
```

- [ ] Timeline을 24fps/204 frames로 만들고 exact tracks `CameraTrack`, `BackgroundTrack`, `P1_SpeakerTrack`, `P2_ReactorTrack`, `P3_WorkerTrack`, `P4_ExitLeadTrack`, `P5_ExitFollowTrack`만 둔다. 모든 track은 AnimationTrack이며 AudioTrack과 SignalTrack은 만들지 않는다.
- [ ] 여섯 비트의 주요 동작을 다음 경계에 배치한다: frame 0 공간 인지, 36 P1 발화, 72 반응 확산, 108 함께 웃음, 138 점심 전환, 180 인계, 204 종료. Sprite swap과 큰 transform은 2-frame 간격 또는 그 배수에 두고 stepped tangent를 사용한다. 모든 Transform curve key는 `(value * 100)`이 정수이고 tangent가 Constant인지 scene contract test로 검사한다.
- [ ] Camera는 orthographic, clear color는 불투명한 따뜻한 중립색, `playOnAwake: true`, `DirectorWrapMode.None`으로 만든다. 줌·회전 없이 넓은 공간→대화 중심→오른쪽 출입문으로만 절제해 이동한다.
- [ ] builder를 같은 candidate에서 두 번 실행해 scene/timeline/clip dependency hash가 동일함을 test로 확인한다. 기존 `.meta` GUID는 유지하고 target 경로 밖 asset을 삭제하지 않는다.
- [ ] `A01OfficePreviewSession`은 `SceneSetup[]`, active scene path, 기존 `playModeStartScene` path를 serializable snapshot으로 SessionState에 저장하고 consume-and-clear한다.
- [ ] `A01OfficePreviewLauncher.PlayPreview()`를 menu `Last Host/Cinematics/A01/Play Preview`에 등록한다. modified scene 저장을 사용자가 취소하면 무변경으로 중지하고, 승인하면 현재 setup을 capture→A01 single open→`playModeStartScene = null`→EnterPlaymode 순서로 실행한다.
- [ ] `[InitializeOnLoad]` state handler가 `ExitingEditMode`에서 preview override를 재적용하고, `EnteredEditMode` 또는 중단 복구에서 기존 scene setup과 start scene을 복원한 뒤 SessionState를 지운다. existing `StartupPlayModeBootstrap`은 수정하지 않는다.
- [ ] 실제 연결된 Unity Editor에서 lease 아래 `Rebuild Preview`를 한 번 실행해 생성 asset을 source checkout에 저장한다. 임시 객체와 scene dirty를 확인하고 lease를 release한다.

```powershell
pwsh tools/verification/UnityMcpLease.ps1 Release `
  -ProjectPath UnityProject `
  -Agent unity-scene-integration `
  -WorkId 2026-08-10-a01-office-animatic `
  -RunId a01-implementation-001
```

- [ ] GREEN fingerprint는 아래 exact dependency set과 새 run ID `a01-target-green-001`로 만든다. `verification-current-state.json`은 RED 후 wrapper가 기록한 네 누적 cost 정수를 보존하고, status를 `ready-for-verification`, run/fingerprint를 GREEN 값으로 바꾸며 `evidence`는 빈 배열로 초기화한다. RED XML은 `verification.md`에만 보존한다.

```powershell
$a01RunId = 'a01-target-green-001'
pwsh tools/verification/Get-VerificationFingerprint.ps1 `
  -ProjectRoot . `
  -ProductionPath $a01GreenProductionPaths `
  -TestPath 'UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01' `
  -ScenePath 'UnityProject/Assets/_Project/Scenes/Cinematics/Opening/A01OfficeAnimatic.unity','UnityProject/Assets/_Project/Scenes/Startup.unity','UnityProject/ProjectSettings/EditorBuildSettings.asset' `
  -PackagePath 'UnityProject/Packages/manifest.json','UnityProject/Packages/packages-lock.json' `
  -VersionPath 'UnityProject/ProjectSettings/ProjectVersion.txt' `
  -RunId $a01RunId `
  -ManifestPath "_workspace/active/2026-08-10-a01-office-animatic/artifacts/fingerprint-$a01RunId.json"
```

- [ ] agent `unity-scene-integration`, run ID `a01-target-green-001` lease를 직전에 Acquire하고 60초 heartbeat를 Renew하며 성공·실패 모두 같은 identity로 Release한다.
- [ ] 아래 exact wrapper command로 target namespace를 실행한다. `CandidateFingerprint`는 GREEN manifest에서 다시 읽고, `ProductionPath`는 Editor/art/Timeline/Animation, 생성 scene `.meta`, 두 Startup 보호 C#과 모든 신규 folder `.meta`를 포함하는 `$a01GreenProductionPaths`를 직접 전달한다.

```powershell
$a01TempRoot = Join-Path $env:TEMP 'last-host-a01-office-animatic'
$a01Fingerprint = (Get-Content -Raw -LiteralPath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/fingerprint-a01-target-green-001.json' | ConvertFrom-Json).candidate_fingerprint
if ([string]::IsNullOrWhiteSpace($a01Fingerprint)) { throw 'Fingerprint manifest is missing candidate_fingerprint.' }
New-Item -ItemType Directory -Force -Path $a01TempRoot | Out-Null
pwsh tools/verification/Invoke-HighCostVerification.ps1 `
  -WorkId 2026-08-10-a01-office-animatic `
  -CriterionId A01-TARGET-GREEN `
  -Route UnityEditMode `
  -RunId a01-target-green-001 `
  -CandidateFingerprint $a01Fingerprint `
  -LedgerPath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/verification-attempt-ledger.json' `
  -AgentBriefPath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/agent-brief.json' `
  -CurrentStatePath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/verification-current-state.json' `
  -QaHarnessPath 'UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01' `
  -ContractBaselinePath 'UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01' `
  -ProductionPath $a01GreenProductionPaths `
  -TestPath 'UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01' `
  -SourceProjectPath UnityProject `
  -CacheRoot (Join-Path $env:TEMP 'last-host-unity-cache') `
  -UnityPath 'C:/Program Files/Unity/Hub/Editor/6000.4.6f1/Editor/Unity.exe' `
  -ResultsPath (Join-Path $a01TempRoot 'target-green-results.xml') `
  -LogPath (Join-Path $a01TempRoot 'target-green-unity.log') `
  -TestFilter 'LastHost.Prototype.Cinematics.A01.Tests' `
  -TimeoutSeconds 1800
```

**Expected gate:** target `18/18 Passed`, failed/skipped/inconclusive `0`, Editor compile error `0`, protected hashes unchanged.

**Commit:**

```powershell
if (@($a01AllFolderMetaPaths).Count -ne 20) { throw 'A01 folder meta contract must contain 20 paths.' }
$a01MissingFolderMeta = @($a01AllFolderMetaPaths | Where-Object { -not (Test-Path -LiteralPath $_ -PathType Leaf) })
if ($a01MissingFolderMeta.Count -gt 0) { throw "Missing A01 folder meta: $($a01MissingFolderMeta -join ', ')" }
git add -- $a01AllFolderMetaPaths
git add tools/art/Remove-ConnectedChromaMatte.ps1 tools/art/Test-RemoveConnectedChromaMatte.ps1 _workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Preview UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01 UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01 UnityProject/Assets/_Project/Scenes/Cinematics/Opening/A01OfficeAnimatic.unity UnityProject/Assets/_Project/Scenes/Cinematics/Opening/A01OfficeAnimatic.unity.meta UnityProject/Assets/_Project/Timelines/Cinematics/Opening/A01 UnityProject/Assets/_Project/Animations/Cinematics/Opening/A01/Preview
git commit -m "feat: build A01 office motion animatic preview"
```

---

## Task 5 (Historical reference — execution prohibited): frozen candidate 전체 회귀와 실제 재생 보조 확인

**Owner:** QA/검증 에이전트

**Files:**

- Modify: `_workspace/active/2026-08-10-a01-office-animatic/verification.md`
- Create on execution: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/fingerprint-a01-frozen-full-001.json`
- Create on execution: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/a01-frozen-full-results.xml`
- Create on execution: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/a01-frozen-full-unity.log`
- Optional review aid 1: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/review/a01-frame-space.png`
- Optional review aid 2: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/review/a01-frame-laugh.png`
- Optional review aid 3: `_workspace/active/2026-08-10-a01-office-animatic/artifacts/review/a01-frame-handoff.png`

- [ ] QA가 production owner의 GREEN, diff와 protected hashes를 확인하고 candidate를 freeze한다. production·test·harness가 바뀌면 GREEN과 이전 fingerprint를 `SUPERSEDED`로 표시한다.
- [ ] 새 fingerprint/run ID `a01-frozen-full-001`을 GREEN과 같은 dependency set으로 만들되 `TestPath`에는 `UnityProject/Assets/_Project/Tests` 전체를 넣는다. ScenePath에는 A01 scene, Startup scene, `EditorBuildSettings.asset`을 모두 포함한다. current-state는 GREEN 후 네 누적 cost 정수만 보존하고 동일 fingerprint/run ID, `ready-for-verification`, 빈 `evidence`로 갱신한다. RED/GREEN 결과는 `verification.md`에만 둔다.
- [ ] 아래 command로 frozen fingerprint를 만든다.

```powershell
$a01RunId = 'a01-frozen-full-001'
pwsh tools/verification/Get-VerificationFingerprint.ps1 `
  -ProjectRoot . `
  -ProductionPath $a01GreenProductionPaths `
  -TestPath 'UnityProject/Assets/_Project/Tests' `
  -ScenePath 'UnityProject/Assets/_Project/Scenes/Cinematics/Opening/A01OfficeAnimatic.unity','UnityProject/Assets/_Project/Scenes/Startup.unity','UnityProject/ProjectSettings/EditorBuildSettings.asset' `
  -PackagePath 'UnityProject/Packages/manifest.json','UnityProject/Packages/packages-lock.json' `
  -VersionPath 'UnityProject/ProjectSettings/ProjectVersion.txt' `
  -RunId $a01RunId `
  -ManifestPath "_workspace/active/2026-08-10-a01-office-animatic/artifacts/fingerprint-$a01RunId.json"
```

- [ ] agent `qa-verification`, run ID `a01-frozen-full-001` lease를 직전에 Acquire하고 60초 heartbeat를 Renew하며 성공·실패 모두 같은 identity로 Release한다.
- [ ] 아래 exact wrapper command를 `TestFilter` 없이 한 번만 실행한다. `CandidateFingerprint`는 frozen manifest에서 다시 읽고 `ProductionPath`에는 `$a01GreenProductionPaths`를 직접 전달한다. 이것이 유일한 frozen full EditMode run이다.

```powershell
$a01Fingerprint = (Get-Content -Raw -LiteralPath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/fingerprint-a01-frozen-full-001.json' | ConvertFrom-Json).candidate_fingerprint
if ([string]::IsNullOrWhiteSpace($a01Fingerprint)) { throw 'Fingerprint manifest is missing candidate_fingerprint.' }
pwsh tools/verification/Invoke-HighCostVerification.ps1 `
  -WorkId 2026-08-10-a01-office-animatic `
  -CriterionId A01-FROZEN-FULL `
  -Route UnityEditMode `
  -RunId a01-frozen-full-001 `
  -CandidateFingerprint $a01Fingerprint `
  -LedgerPath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/verification-attempt-ledger.json' `
  -AgentBriefPath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/agent-brief.json' `
  -CurrentStatePath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/verification-current-state.json' `
  -QaHarnessPath 'UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01' `
  -ContractBaselinePath 'UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01' `
  -ProductionPath $a01GreenProductionPaths `
  -TestPath 'UnityProject/Assets/_Project/Tests' `
  -SourceProjectPath UnityProject `
  -CacheRoot (Join-Path $env:TEMP 'last-host-unity-cache') `
  -UnityPath 'C:/Program Files/Unity/Hub/Editor/6000.4.6f1/Editor/Unity.exe' `
  -ResultsPath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/a01-frozen-full-results.xml' `
  -LogPath '_workspace/active/2026-08-10-a01-office-animatic/artifacts/a01-frozen-full-unity.log' `
  -TimeoutSeconds 1800
```
- [ ] 결과 XML에서 discovered test 전부 Passed, failed/skipped/inconclusive 0을 확인하고, A01 target 18개와 기존 `EditorPlayMode_AlwaysStartsFromSavedStartupScene` PASS를 별도로 적는다.
- [ ] full PASS 뒤 wrapper가 남긴 `verification-running` state를 QA가 `apply_patch`한다. status는 `independent-qa-pass-awaiting-director`, run/fingerprint는 frozen 값, cost는 wrapper의 실제 누적값을 그대로 보존한다. evidence에는 frozen run ID와 frozen fingerprint가 붙은 frozen full 결과만 넣는다. 과거 RED/GREEN은 넣지 않는다. full FAIL이면 status를 `blocked`로 두고 PASS evidence를 만들지 않는다.
- [ ] 현재 profile에서 `McpPlay`는 unavailable이므로 wrapper에 해당 route를 요청하지 않는다. 전용 menu의 Editor Play smoke는 연결된 Editor에서 agent `qa-verification`, run ID `a01-qa-play-smoke-001` lease를 새로 획득한 뒤 한 번만 수행하거나, 불가능하면 정확한 사유를 기록한다.
- [ ] Play smoke가 가능하면 다음 세 순간만 사용자 review aid로 확인한다: 공간 인지(첫 비트 중간), 함께 웃음(네 번째 비트 중간), 출입문 인계(마지막 비트 중간). 이 PNG는 task-specific atomic harness가 없으므로 canonical 자동 증거로 선언하지 않는다.
- [ ] 무음 전체 재생에서 P1 발화→P2/P4/P5 지연 반응→그룹 웃음→P1 rise와 출입문 방향 전환을 확인하고, P3는 약한 반응만 유지하는지 기록한다.
- [ ] Play 종료 뒤 original scene setup, `playModeStartScene == Startup.unity`, Play false, Pause false, dirty baseline, 임시 객체 0과 Console Error 0을 확인한다. 복구 실패는 A01-C07 blocker다.
- [ ] 다음 low-cost 정적 검사를 수행한다.

```powershell
git diff --check
git status --short
rg -n "AudioTrack|AudioSource|ParticleSystem|Cinemachine|UnityEngine\.U2D\.Animation" UnityProject/Assets/_Project/Editor/Cinematics/Opening/A01 UnityProject/Assets/_Project/Tests/EditMode/Cinematics/A01
function Get-A01CanonicalLfHash {
  param([Parameter(Mandatory)][string]$Path)
  $raw = [System.IO.File]::ReadAllBytes($Path)
  $normalized = New-Object System.Collections.Generic.List[byte]
  for ($i = 0; $i -lt $raw.Length; $i++) {
    if ($raw[$i] -eq 13) {
      if ($i + 1 -ge $raw.Length -or $raw[$i + 1] -ne 10) { throw "Lone CR detected: $Path at byte $i" }
      [void]$normalized.Add(10); $i++
    } else { [void]$normalized.Add($raw[$i]) }
  }
  $bytes = $normalized.ToArray()
  $sha = [System.Security.Cryptography.SHA256]::Create()
  try { $hash = ([System.BitConverter]::ToString($sha.ComputeHash($bytes))).Replace('-','') }
  finally { $sha.Dispose() }
  [pscustomobject]@{ Path = $Path; Bytes = $bytes.Length; Sha256 = $hash }
}
$a01CanonicalExpected = @{
  'UnityProject/Packages/manifest.json' = @{ Bytes = 2069; Sha256 = 'B07DD4E37BA1336B93D763B23E3480BE7943EF4C56DBFDA7EE191FF87B0AF298' }
  'UnityProject/Packages/packages-lock.json' = @{ Bytes = 13840; Sha256 = '943F92F1229C2A366FD42AA7180B73BDB8B6019AE21C1A6CE38C80A15D8C262E' }
  'UnityProject/ProjectSettings/EditorBuildSettings.asset' = @{ Bytes = 799; Sha256 = '67B153F8C73C6C9E7F8C60D47D03A837DFEC207E757AC65FEB6619F58BE28755' }
  'UnityProject/Assets/_Project/Editor/Startup/StartupPlayModeBootstrap.cs' = @{ Bytes = 1346; Sha256 = '634BD355DF765B7283774D3B20983299F2637C8F0503B831057535F58133E5C2' }
  'UnityProject/Assets/_Project/Scripts/UI/Startup/StartupController.cs' = @{ Bytes = 15040; Sha256 = '042B816E531448ABD5DC265C183D309AE1E084E25581E8DF9D4E48FE73931730' }
}
foreach ($entry in $a01CanonicalExpected.GetEnumerator()) {
  $actual = Get-A01CanonicalLfHash -Path $entry.Key
  if ($actual.Bytes -ne $entry.Value.Bytes -or $actual.Sha256 -ne $entry.Value.Sha256) { throw "A01 canonical protected baseline mismatch: $($entry.Key)" }
  $actual
}
```

- [ ] `verification.md`에 A01-C01~C10별 run ID, fingerprint, XML/log, review aid 상태, valid/SUPERSEDED와 실제 비용을 연결한다.

**Expected gate:** frozen full EditMode PASS, 보호 **LF-normalized canonical** hash 5개 일치, Console Error 0. Editor Play를 실행하지 못했다면 기술 자동 검증과 사용자 재생 수용의 공백을 명시한다.

**Commit:**

```powershell
git add _workspace/active/2026-08-10-a01-office-animatic/verification.md _workspace/active/2026-08-10-a01-office-animatic/artifacts docs/project-handoff/task-cost-dashboard.md
git commit -m "test: verify A01 office animatic candidate"
```

---

## Task 6 (Historical reference — execution prohibited): 총괄 감사와 사용자 재생 인계

**Owner:** 프로젝트 총괄 관리자 에이전트 → 프로젝트 조정 에이전트

**Files:**

- Modify: `_workspace/active/2026-08-10-a01-office-animatic/verification.md`
- Modify: `_workspace/active/2026-08-10-a01-office-animatic/task.md`
- Modify: `docs/project-handoff/current-task-board.md`
- Modify: `docs/project-handoff/task-cost-dashboard.md`

- [ ] 총괄 관리자가 승인 명세, A01-C01~C10, canonical fingerprint/run, QA 결과, protected hashes, artifact budget, correction count를 대조한다.
- [ ] 이미지 후보가 final로 오기되지 않았고 Startup·Build Settings·package·오디오·A02/B08로 범위가 확대되지 않았는지 감사한다.
- [ ] 총괄 판정은 `내부 승인 가능`, `수정 필요`, `사용자 결정 필요`, `보류` 중 하나로 `verification.md`에 기록한다.
- [ ] 기술 검증을 통과해도 상태를 `기술 검증 통과 — 사용자 수용 대기`로 두고, 사용자에게는 다음 세 가지만 확인 요청한다: 대화와 웃음이 자연스러운가, P1 자리와 출입문이 기억되는가, 204-frame preview 호흡에서 늘리거나 줄일 비트가 무엇인가.
- [ ] 사용자 수용 전에는 active 폴더를 completed로 이동하지 않고, A02·오디오·Startup 연결 작업도 만들지 않는다.
- [ ] 최종 diff에서 plan·packet·preview art·A01 Editor/test/generated assets·상태판만 포함되는지 확인한다. push는 사용자가 명시적으로 요청한 경우에만 수행한다.

**Final handoff:**

- User-facing file: `docs/design/narrative/opening/a01-office-hybrid-motion-design.md` — 승인 연출 기준
- User-facing playback: Unity menu `Last Host/Cinematics/A01/Play Preview` — 실제 무음 모션 확인
- Engineering reference: `docs/superpowers/plans/2026-08-10-a01-office-animatic.md` — 구현·검증 경계

**Commit:**

```powershell
git add _workspace/active/2026-08-10-a01-office-animatic docs/project-handoff/current-task-board.md docs/project-handoff/task-cost-dashboard.md
git commit -m "docs: hand off A01 animatic for visual acceptance"
```

---

## Plan Self-Review

- Spec coverage: A01 여섯 비트, P1~P5, P1/B08 자리 앵커, A02 직전 종료, 무음, 제한 프레임·레이어·카메라, 최종 시간 후측정을 모두 task와 tests에 연결했다.
- Scope protection: Startup·Build Settings·package·ProjectSettings·게임플레이·A02/B08·오디오 금지를 파일 해시와 EditMode 계약으로 이중 보호한다.
- Type consistency: Editor assembly가 유일한 production code owner이며 runtime gameplay assembly에는 새 타입을 추가하지 않는다. Test assembly가 Editor와 Timeline을 명시 참조한다.
- Evidence consistency: TDD RED 1, target GREEN 1, frozen full 1의 세 Unity start만 계획했고 current-state/run/fingerprint를 매번 새로 만든다.
- Dynamic-value scan: 구현자가 추측해 채울 값은 없다. 이미지 SHA·anchor는 실제 생성 결과를 측정해 기록하며 예시 상수로 위장하지 않는다.
- User gate: 실제 재생 수용 전 완료 금지, 외부/API transparency 경로·A02·오디오·Startup 연결은 별도 승인으로 남긴다.
