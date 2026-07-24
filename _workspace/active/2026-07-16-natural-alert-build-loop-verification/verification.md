# 검증 기록

## 작업 ID

`2026-07-16-natural-alert-build-loop-verification`

## 검증 대상

기존 Windows 빌드 `Builds/RatHostPrototype/LastHostPrototype.exe`에서 디버그 진입이나 런타임 상태 주입 없이 실제 Windows 입력만으로 같은 실행 세션의 `RatHost 시작 → 자연 면역 경계도 100% → WhiteBloodCellEvasion → 변이 조각 3개 → MutationSelection → 숫자키 변이 선택 → RatHost 복귀`를 완료할 수 있는지 확인한다.

## 작업 영역

- 읽기·실행 대상: 기존 Windows 빌드, 기존 `Player.log`, Unity 씬·소스의 이동 경로 정보
- 기록 대상: 이 작업 패킷
- 변경 금지: `UnityProject/`, `Builds/`, `.codex/config.toml`, 기존 완료 패킷

## 완료 주장

같은 실행 세션의 시작·자연 전환·조각 수집·변이 선택·RatHost 복귀·로그 안전성 증거가 모두 있을 때만 `완료 가능`으로 판정한다.

## 실행한 검증

```text
명령:
PowerShell 읽기 전용 파일 메타데이터·Git 상태 확인.

결과:
- 실행본: C:\project\Last-Host\Builds\RatHostPrototype\LastHostPrototype.exe
- 크기: 667648 bytes
- 생성 시각: 2026-06-30 11:20:21.293 +09:00
- 수정 시각: 2026-06-30 11:20:21.299 +09:00
- SHA-256: 65D646C9285BF2CBBAB784992E3AD5AE9012BEF5E8A6B4FFA46209592AF9DDA2
- 기존 Player.log: C:\Users\User\AppData\LocalLow\DefaultCompany\Last Host\Player.log
- 기존 Player.log 크기/수정 시각: 1267 bytes / 2026-07-01 10:26:11.559 +09:00
- 시작 전 `git status --short -- UnityProject Builds`: 출력 없음
- 시작 전 `git diff -- UnityProject Builds`: 출력 없음
- 시작 전 `git diff --cached -- UnityProject Builds`: 출력 없음
- `git diff --check`, `git diff --cached --check`: 오류 없음. 사용자 환경의 global ignore 접근 경고만 출력됨.

해석:
검증 대상 빌드는 존재하고 식별 가능했다. 검증 시작 시 UnityProject와 Builds에는 worktree·staged 변경이 없었다. 기존 Player.log는 이번 시도보다 오래된 로그이며, 빌드를 실행하지 못했으므로 이번 검증 증거로 사용하지 않았다.
```

```text
명령:
씬·소스 읽기 전용 경로 확인.

결과:
- RatHost 시작 좌표와 ToxicWaterRiskZone 중심이 모두 대략 (-0.7, 1.35)이다.
- 위험 구역의 오염 노출은 면역 경계도 +12/초, 숙주 피해 4/초다.
- 오염 노출 원인은 기본 WhiteBloodCellEvasion을 선택한다.
- 바이러스 시작: (-3.8, 0)
- 조각 A/B/C: (-1.4, 2.1), (1.2, -1.7), (3.8, 1.6)
- 백혈구 시작: (3.2, 0), 바이러스/백혈구 속도: 4.2/2.2

해석:
상태 주입 없이 약 8.3초 자연 오염 노출로 100%와 WhiteBloodCellEvasion 진입이 가능한 구성임을 확인했고, 실제 입력 시도의 이동 경로를 준비했다. 이 정적 확인은 Windows 실제 입력 성공을 대체하지 않는다.
```

```text
명령:
`computer-use` 공식 bundled client bootstrap 후 `list_apps`로 Windows 연결 확인.

결과:
첫 시도 실패:
Computer Use native pipe is unavailable: Error: failed to connect native pipe: 지정된 파일을 찾을 수 없습니다. (os error 2)

복구:
- JavaScript 세션 초기화 1회
- bundled client 재-bootstrap
- `list_apps` 1회 재시도

재시도 결과:
동일한 native pipe unavailable / os error 2.

해석:
`computer-use` 스킬이 허용하는 복구 횟수 안에서 Windows helper 연결에 실패했다. 규정상 PowerShell, SendKeys, Start-Process 또는 별도 네이티브 입력 우회로 전환하지 않고 UI 검증을 중단했다.
```

## Windows 실제 입력 플레이 체크

- `list_apps`: 연결 실패
- `launch_app`: 실행하지 못함
- 대상 창 polling/get_window/activate/snapshot: 실행하지 못함
- 빌드 프로세스: 시작되지 않음
- 실제 입력: 없음
- F6: 사용하지 않음
- 런타임 상태 주입·코드 수정: 없음

## 결과

- 빌드 존재·해시·시각 식별: 통과
- 시작 전 UnityProject/Builds 변경 0: 통과
- 자연 경계도 100% 도달: 미검증
- 기본 WhiteBloodCellEvasion 화면: 미검증
- 변이 조각 0/3 → 중간 → 3/3: 미검증
- MutationSelection과 숫자키 선택: 미검증
- 변이 적용 RatHost 복귀: 미검증
- 같은 성공 세션 Player.log 오류 검색: 미검증
- 종료 후 프로세스 정상 종료: 실행하지 않았으므로 해당 없음
- EditMode 90/90: 실행하지 않음. 핵심 Windows 입력 검증의 helper 연결 단계에서 작업이 차단되었다.

## MCP 플레이 체크

이번 작업의 핵심 완료 조건은 기존 Windows 빌드의 실제 입력 연속 루프이며, Unity Editor 직접 상태 전환은 성공 증거로 금지되어 있다. 따라서 대체 상태 주입 MCP 플레이 체크는 실행하지 않았다.

## 미검증 항목

- 같은 프로세스에서의 모든 플레이 단계와 단계별 화면
- 창 포커스와 입력 전달 증적
- 동일 성공 세션 `Player.log`
- 실제 사용자 조작감·난이도·무설명 이해 여부는 원래 별도 보류이며 계속 보류한다.

## 남은 위험

- 빌드 실행본의 자연 경계도 100% 성공 루프 가능 여부가 여전히 증명되지 않았다.
- Computer Use native helper가 복구되지 않으면 금지된 우회 없이 독립 실제 입력 검증을 수행할 수 없다.

## 재개 조건

1. Computer Use native helper가 정상 연결되어 `list_apps`가 응답해야 한다.
2. 새 검증 시도에서 `list_apps → launch_app → poll → get_window → activate → snapshot` 순서를 다시 지킨다.
3. 같은 새 실행 세션의 시작·100%·WhiteBloodCellEvasion·조각·선택·복귀·Player.log 증거를 전부 다시 수집한다.

## 완료 판단

`차단`

## 완료 판단 근거

엄격 성공 조건의 핵심인 Windows 빌드 실행, 창 포커스, 실제 입력, 단계별 화면, 같은 세션 로그를 하나도 확보하지 못했다. helper 연결 실패는 기능 실패로 단정할 근거는 아니지만, 성공을 주장할 검증 근거도 없으므로 `완료 가능` 또는 `부분 통과`로 승격하지 않는다.

## 재개 조건 재점검 — 2026-07-20

명령/절차:

1. Computer Use 표준 bundled client bootstrap 후 `list_apps`.
2. 2초 후 `list_apps` 재시도.
3. 세션 초기화 후 재-bootstrap하고 `list_apps` 재시도.

결과:

- 세 시도 모두 Computer Use native pipe unavailable, `os error 2`로 실패했다.
- Windows 빌드 실행, 대상 창 탐색·포커스·스냅샷, 실제 입력, 자연 경계도 100%, 내부 미니게임·조각·변이·복귀, 동일 세션 `Player.log` 검사는 실행하지 않았다.
- 코드·Unity·Builds 변경과 금지된 UI 입력 우회는 수행하지 않았다.

해석:

- 동일 차단 조건이 반복됐으므로 QA 판정은 **차단**으로 유지한다. 기능 실패나 완료 가능으로 해석하지 않는다.

재개 조건:

1. Computer Use helper가 정상 연결되어 `list_apps`가 응답할 것.
2. 또는 사용자가 같은 연속 루프의 단계별 화면과 해당 세션 `Player.log`를 제공할 것.

## 2026-07-16 차단 기록 커밋 범위 QA 판정

### 판정 대상

사용자의 `일단 커밋 푸쉬해` 지시에 따라 기능 완료가 아닌 현재 차단 상태, 누적 완료 보관 이동, 상태판 기록만 커밋하는 예외 범위를 검토했다. 자연 성공 루프 QA 판정은 `차단`, 작업 위치는 `active`로 유지한다.

### 실행한 대조

```text
명령:
Git HEAD의 기존 active 파일 목록·blob과 현재 completed 대상 파일을 경로별로 대조.

결과:
- `2026-07-01-rat-host-full-play-verification`:
  - tracked 원본 49개
  - completed 대상 누락 0개
  - blob 불일치 0개
- `2026-07-09-white-blood-cell-response-scaling`:
  - tracked 원본 5개
  - completed 대상 누락 0개
  - blob 불일치 0개
- `2026-07-10-signal-suppression-approach-cue`:
  - tracked 원본 6개
  - completed 대상 누락 0개
  - artifact 스크립트는 원본 blob과 일치
  - 문서 5개는 2026-07-16 QA 재검증·총괄 판정·완료 보관 기록이 추가되어 최종 완료본으로 갱신됨
  - completed 대상에는 게이트 산출물 `director-review.md`, `handoff.md` 2개가 추가됨

해석:
삭제되는 세 active 원본의 모든 파일은 대응 completed 경로에 보존되어 있다. 두 이동은 byte 단위로 동일하고, 접근 예고 패킷의 문서 차이는 기록된 검증 종결·완료 보관 추가 내용과 일치한다.
```

```text
명령:
`git status --porcelain=v1 --untracked-files=all`, 허용 경로 필터, staged·Unity/Build·diff-check 확인.

결과:
- 전체 status 항목: 140개
- 허용 범위 밖 예상 외 변경: 0개
- 허용 포함 범위: `_workspace/**`, `docs/project-handoff/current-task-board.md`
- 명시 제외: `.codex/config.toml`
- staged 변경: 0개
- `git status/diff/diff --cached -- UnityProject Builds`: 모두 출력 없음
- `git diff --check`: 공백 오류 없음
- `git diff --cached --check`: 공백 오류 없음
- HEAD: `866fd8ac2c0d55712b839ce7a536de4b6b56c6f3`
- 로컬 `refs/remotes/origin/main`: 동일

해석:
현재 변경은 사용자 예외 승인 범위와 제외 경계 안에 있다. 전역 ignore 접근 및 LF/CRLF 안내 경고는 있었지만 diff-check 실패는 아니다.
```

### 상태·게이트 대조

- 현재 active 작업 디렉터리: `2026-07-16-natural-alert-build-loop-verification` 1개.
- 현재 패킷: 필수 문서 7개와 차단 artifact 1개 존재.
- 상태판과 `CURRENT.md`: 작업을 완료가 아닌 `차단`, active 유지, Computer Use 복구 또는 사용자 수동 증거라는 재개 조건으로 기록한다.
- `.codex/config.toml`, `UnityProject/`, `Builds/`는 커밋에서 제외한다.
- 이번 판정은 기능 완료·QA `완료 가능`·총괄 `내부 승인 가능`으로 승격하지 않는다.

### QA 커밋 범위 판정

`커밋 범위 적합`

근거: 이동 원본의 대응 파일 보존, 허용 경로 밖 변경 0, staged 0, Unity/Build diff 0, diff-check 통과, active 차단 상태와 재개 조건 유지가 확인됐다. 지정 범위만 정확히 스테이징하고 `.codex/config.toml`을 제외한다는 조건으로 차단 기록 커밋을 진행할 수 있다.

## 2026-07-16 staged 범위 최종 QA

### staged 스냅샷

- 총 84개: `A 22`, `M 2`, `D 5`, `R 55`.
- staged 허용 경로: `_workspace/**`, `docs/project-handoff/current-task-board.md`만 존재.
- 허용 경로 밖 staged 변경: 0개.
- `.codex/config.toml`: worktree 수정만 있고 staged 변경 0개.
- `UnityProject/`, `Builds/`: staged 변경 0개.
- `git diff --cached --check`: 통과.

### 이동·보존 대조

- `R55`: 이전 full-play 49개, 백혈구 스케일링 5개, 접근 예고 artifact 스크립트 1개가 모두 `R100`으로 검출됐다.
- 접근 예고 문서 `D5` 각각에 completed 경로의 동일 상대 이름 `A5`가 대응한다.
- 접근 예고 completed 문서는 QA 재검증·총괄 판정·완료 보관을 포함한 최종본이며, 이전 대조에서 원본 파일 누락 0을 확인했다.
- 현재 active 차단 패킷의 문서 7개와 artifact 1개가 모두 staged에 포함됐다.

### 상태 표현 대조

- staged `CURRENT.md`: 작업 `차단`, active 유지, 기능 완료·보관 아님, helper 복구 또는 사용자 수동 증거라는 재개 조건을 유지한다.
- staged 상태판: 자연 성공 루프를 `차단`, QA `차단`, 총괄 `보류`, 사용자 지시에 따른 차단 기록 커밋 예외로 표시한다.
- staged `task.md`·`verification.md`: 기능 완료가 아닌 차단 기록 커밋이며 작업 재개 필요를 유지한다.

### 최종 판정

`staged 범위 최종 적합 — 커밋 실행 가능(기능 완료 아님)`

판정 조건: 이 QA 기록을 같은 active 패킷 범위에 다시 포함한 뒤 `.codex/config.toml`, `UnityProject/`, `Builds/` 제외와 cached diff-check 통과를 유지한다. 자연 성공 루프 QA 판정은 계속 `차단`이다.

## 2026-07-24 Computer Use 재개 QA

### 검증 주장

같은 새 Windows 빌드 실행 세션에서 실제 플레이 화면과 허용 입력만으로 `RatHost 시작 → 자연 면역 경계도 100% → 기본 WhiteBloodCellEvasion → 조각 3개 → 변이 선택 → 변이 적용 RatHost 복귀`를 완료하고 같은 세션 로그를 연결할 수 있는지 재검증한다.

### 연결·실행

```text
명령:
- computer-use bundled client 초기화.
- sky.documentation("guidance"), sky.documentation("confirmations"), sky.documentation("api") 확인.
- sky.list_apps().
- 명시적 실행본 경로로 sky.launch_app().
- sky.list_windows()로 새 창을 다시 조회하고 정확한 대상 창을 필터링.

결과:
- 이전 native pipe `os error 2` 없이 list_apps/list_windows 정상 응답.
- 시도 ID: NATURAL-20260724-2026-07-24T02-05-04-587Z
- 프로세스: PID 73708, 2026-07-24 11:05:11 KST 시작.
- 실행 전 대상 Windows 빌드 창: 0개.
- 실행 후 반환된 정확한 대상 창: 1개.
  - app: process:C:\project\Last-Host\Builds\RatHostPrototype\LastHostPrototype.exe
  - id: 15532732
  - title: Last Host

해석:
Computer Use 연결 단계와 기존 빌드의 새 단일 실행 세션 생성은 통과했다. Unity Editor 창은 실행본 경로와 다른 app 객체이므로 대상에서 제외했다.
```

### 시작 화면 캡처와 복구

```text
명령:
1. 반환된 유일한 게임 창을 get_window로 재수화.
2. activate_window 후 get_window_state(include_screenshot:true, include_text:true).
3. 실패 후 list_windows를 새로 호출해 정확히 하나인 게임 창을 다시 선택.
4. 새 반환 객체로 get_window 후 get_window_state(include_screenshot:true, include_text:false) 1회 복구.

결과:
- 최초: SetIsBorderRequired failed: 해당 인터페이스를 지원하지 않습니다. (0x80004002)
- 복구 1회: 같은 오류.
- 스크린샷 ID, 접근성 상태, 시작 HUD 시각 증거를 받지 못함.

해석:
native pipe 연결 문제는 해소됐지만 게임 창의 Windows Graphics Capture 단계가 차단됐다. 최신 관찰 없이 키나 좌표를 보내면 엄격 검증의 포커스·화면·입력 전달 증거가 성립하지 않으므로 플레이 입력을 보내지 않았다.
```

### 종료와 동일 시도 로그

```text
명령:
- 정확한 대상 창에 Computer Use Alt+F4.
- 1.5초 후 list_windows와 Get-Process로 종료 확인.
- 동일 시도 Player.log 보존 및 Exception|Error|Crash|failed|abort|fatal 검색.

결과:
- 종료 후 Last Host 게임 창: 0개.
- 종료 후 LastHostPrototype 프로세스: 0개.
- 보존 로그: artifacts/Player-NATURAL-20260724-attempt-1.log
- 보존 로그 SHA-256: D6634589E1E1B4EE5763598937099ED474857F0909CF18F851843A6726D5B2C9
- Exception/Error/Crash/fatal/abort: 0건.
- failed: 1건, `d3d12: failed to query info queue interface (0x80004002).`
- Physics/Input/CodeReloadManager 종료 기록 존재.

해석:
이번 짧은 시도는 정상 종료됐고 예외·크래시는 기록되지 않았다. D3D12 정보 큐 조회 실패는 실행을 중단시키지 않았지만, 핵심 플레이를 실행하지 못했으므로 전체 루프의 로그 안전성 통과로 확대하지 않는다.
```

### 변경 경계

- 빌드 재생성·수정 없음.
- `Builds/` Git 변경 없음.
- `UnityProject/`에는 검증 시작 전부터 존재한 카메라·RatVisual·WASD·숙주 본능·ProjectSettings 관련 6개 미커밋 경로가 종료 후에도 같은 경로 목록으로 유지됐다.
- QA는 코드·씬·ProjectSettings·공유 상태판·`CURRENT.md`를 수정하지 않았다.
- F6, Unity Editor Play, MCP/Inspector/reflection/메모리/세션 상태 주입, PowerShell/SendKeys UI 우회 없음.

### 단계별 판정

| 필수 단계 | 판정 |
| --- | --- |
| Computer Use 연결 | 통과 |
| 새 단일 Windows 빌드 실행·정확한 창 식별 | 통과 |
| 시작 RatHost HUD·경계도·변이 없음 화면 | 차단 |
| 자연 경계도 상승과 100% | 미검증 |
| 기본 WhiteBloodCellEvasion | 미검증 |
| 조각 0/3 → 중간 → 3/3 | 미검증 |
| MutationSelection과 공개 입력 선택 | 미검증 |
| 변이 적용 RatHost 복귀 | 미검증 |
| 같은 성공 세션 Player.log | 미검증 |
| 이번 실패 시도 종료·로그 보존 | 통과 |

### 최종 QA 판정

`차단`

이전 `list_apps` native pipe 차단은 해소됐지만, 게임 창 캡처가 `SetIsBorderRequired 0x80004002`로 최초와 규정된 복구 1회 모두 실패했다. 필수 화면과 실제 입력 증거가 없으므로 `부분 통과`나 `완료 가능`으로 승격하지 않는다. 상세 시도 기록은 `artifacts/attempt-NATURAL-20260724-1.md`에 보존했다.

## 2026-07-24 선별 커밋 전 QA

### 검증 대상

완료된 카메라·이동 변경과 보관 패킷, 자연 경계도 엄격 검증의 최신 차단 기록, `CURRENT.md`, 공유 현황판으로 구성된 staged 38개 경로가 사용자 지시와 루프 엔지니어링 게이트에 맞는지 독립 대조한다.

### 작업영역

- staged Unity 변경 5개: `RatHostPrototype.unity`, 카메라·방향 스프라이트·숙주 제어 코드, EditMode 회귀 테스트.
- `_workspace/completed/2026-07-24-2026-07-21-game-view-camera-output-fix/` 보관 패킷과 대응 active 제거.
- 본 active 자연 경계도 작업의 2026-07-24 차단 기록과 두 artifact.
- `_workspace/active/CURRENT.md`, `docs/project-handoff/current-task-board.md`.
- 명시적 제외: `UnityProject/ProjectSettings/ProjectSettings.asset`, `_workspace/previews/`, `Builds/`.

### 실행한 검증

명령:
`git status --short`

결과:
- staged 38개 경로를 확인했다.
- `UnityProject/ProjectSettings/ProjectSettings.asset`은 unstaged, `_workspace/previews/`는 untracked로 남아 있다.

해석:
- 사용자 범위 밖 변경은 index에 포함되지 않았다.

명령:
`git diff --cached --name-status -M`

결과:
- 카메라·이동 Unity 변경 5개, 완료 카메라 작업의 active 제거·completed 추가/이동, 본 active 차단 기록, `CURRENT.md`, 상태판만 확인했다.
- 허용한 정확한 경로 집합과 비교한 예상 밖 staged 경로는 0개다.

해석:
- staged 범위는 문서/릴리즈 에이전트가 고정한 포함 범위와 일치한다.

명령:
staged 경로를 대상으로 `ProjectSettings.asset`, `_workspace/previews/`, `Builds/` 포함 수를 집계.

결과:
- `ProjectSettings.asset`: 0개.
- `_workspace/previews/`: 0개.
- `Builds/`: 0개.

해석:
- 세 필수 제외 범위가 모두 지켜졌다.

명령:
완료 카메라 보관 경로의 `task.md`, `work-log.md`, `agent-activity.md`, `verification.md`, `completion-report.md` worktree/index 존재 확인과 QA·총괄 판정 검색.

결과:
- 필수 문서 5개는 worktree와 index에 모두 존재한다.
- 독립 QA는 기능 통과 근거와 실제 OS 키·자연 시간 리듬·전체 Test Runner 미검증을 분리했다.
- 프로젝트 총괄 관리자는 `내부 승인 가능`을 기록했고, 사용자 종료·보관 지시와 메인 에이전트 직접 구현 예외 사유가 `task.md`·`agent-activity.md`에 남아 있다.

해석:
- 카메라·이동 작업은 완료 보관 및 커밋 게이트 기록을 갖췄다. 미검증 항목은 사용자 체감·후속 회귀 보강으로 남아 있으며 통과 범위를 과장하지 않는다.

명령:
본 active 패킷, `CURRENT.md`, 상태판에서 `차단`, `보류`, 재개 조건, 완료 금지 경계를 대조하고 실제 active/completed 경로를 조회.

결과:
- 자연 경계도 작업은 실제 active 경로에 있고 QA `차단`, 총괄 `보류`, 완료 보관 금지로 일치한다.
- Computer Use 연결·빌드 실행·단일 창 식별만 통과했고, 게임 창 캡처 실패 뒤 실제 플레이 입력과 자연 성공 루프는 미검증으로 유지된다.
- 상태판의 다음 후보 3개는 현재 active 작업을 닫기 위한 후속 회귀·수용·시각 검토이며, 자연 경계도 엄격 검증을 후보에 중복하지 않는다.
- 카메라·이동 완료 경로와 상태판 최근 완료 경로가 실제 completed 경로와 일치한다.
- `_workspace/active/2026-07-16-current-task-board-consistency/`라는 추적 파일 없는 빈 로컬 디렉터리가 남아 있으나 Git 작업 패킷이나 staged 변경은 아니다. 완료 패킷은 실제 completed 경로에 존재한다.

해석:
- Git에 기록될 상태판·포인터·실제 작업 패킷의 기능 상태와 후보/보류 경계는 정합하다. 빈 비추적 디렉터리는 저장소 결과에 포함되지 않으며 기능 작업 중복으로 보지 않는다.

명령:
`git diff --cached --check`

결과:
- 종료 코드 2.
- 유일한 보고 경로는 `artifacts/Player-NATURAL-20260724-attempt-1.log`이며, 원본 Windows 로그 43줄의 CRLF가 각 줄의 trailing whitespace로 보고됐다.
- 그 외 경로의 whitespace 오류는 0개다.

해석:
- 전체 검사의 실패 원인은 원본 증적 로그의 줄끝 형식뿐이다. 로그를 수정하면 기록된 증적 해시가 달라지므로 정리하지 않는다.

명령:
`git diff --cached --check -- . ':(exclude)_workspace/active/2026-07-16-natural-alert-build-loop-verification/artifacts/Player-NATURAL-20260724-attempt-1.log'`

결과:
- 종료 코드 0.

해석:
- 원본 로그를 제외한 staged 변경의 diff-check는 통과했다.

명령:
`Get-FileHash -Algorithm SHA256`, `git hash-object`, `git rev-parse :<Player.log 경로>`, `git ls-files --stage`.

결과:
- SHA-256: `D6634589E1E1B4EE5763598937099ED474857F0909CF18F851843A6726D5B2C9`.
- worktree blob과 index blob: 모두 `82f14f1d15c92effc812af9e8074e8419cdd8608`.

해석:
- 시도 요약과 검증 기록의 SHA-256이 실제 파일과 일치하고, index에 원본과 동일한 내용이 staged됐다.

### 결과

- 포함 범위, 완료 보관 필수 기록, active 차단 상태, 상태판·포인터, 후보·보류 경계가 현재 사실과 일치한다.
- 예상 밖 staged 경로는 0개이며 세 필수 제외 범위는 모두 index에서 0개다.
- 원본 `Player.log`의 줄끝 예외를 제외한 cached diff-check가 통과했고 증적 무결성이 확인됐다.

### MCP 플레이 체크

이번 검증은 이미 수행된 카메라·이동 QA와 자연 경계도 차단 기록을 커밋 직전 대조하는 릴리즈 QA다. Unity나 Windows 빌드를 새로 실행하지 않았다. 카메라·이동 플레이 근거와 미검증 경계는 completed 패킷에 있고, 자연 경계도 기능은 이번 기록으로 통과시키지 않는다.

### 미검증 항목

- 아직 커밋과 push가 실행되지 않아 새 HEAD·`origin/main` 반영은 미검증이다.
- 자연 경계도 100%부터 변이 적용 RatHost 복귀까지의 Windows 연속 성공 루프는 계속 미검증이다.
- 실제 물리 OS 키 체감, 자연 시간 숙주 본능 리듬, 전체 EditMode Test Runner는 카메라·이동 completed 패킷의 남은 확인으로 유지한다.

### 남은 위험

- 원본 로그를 포함한 전체 `git diff --cached --check`는 CRLF 줄끝 때문에 종료 코드 2를 반환한다. 이는 내용 오류가 아니라 해시로 고정한 외부 증적 형식 예외이며, 커밋 전에 로그를 자동 정리하면 안 된다.
- push 후 원격 반영과 필수 제외 파일 유지 여부를 별도 QA로 확인해야 한다.

### 완료 판단

`선별 커밋 범위 적합 — 커밋 실행 가능(자연 경계도 기능 완료 아님)`

### 완료 판단 근거

staged 38개가 승인·검증된 카메라·이동 변경/보관과 최신 자연 경계도 차단 기록으로 한정됐고, 예상 밖 경로와 필수 제외 경로가 0개다. 완료 카메라 패킷은 QA·총괄·사용자 보관 근거를 갖췄으며 본 작업은 active·QA `차단`·총괄 `보류`로 유지된다. 원본 로그는 기록된 SHA-256과 index/worktree blob이 일치하고, 이를 제외한 cached diff-check가 통과했다. 커밋·push 후 post-push QA를 조건으로 커밋 범위만 적합하다고 판정한다.

## 2026-07-24 post-push QA

### 검증 대상

사용자가 지시한 선별 커밋 `3beb976`이 `origin/main`에 정확히 반영됐는지, 제외 범위와 로컬 범위 밖 변경이 보존됐는지, 자연 경계도 엄격 검증의 active·차단·재개 조건이 유지됐는지 독립 대조한다.

### 실행한 검증

명령:
`git rev-parse HEAD`

결과:
`3beb97635a067403ccc6486dd4a7e71be6d4d8fa`

명령:
`git rev-parse refs/remotes/origin/main`

결과:
`3beb97635a067403ccc6486dd4a7e71be6d4d8fa`

명령:
`git ls-remote origin refs/heads/main`

결과:
샌드박스 내부 첫 조회는 GitHub 443 연결 제한으로 실패했다. 같은 읽기 전용 명령을 승인된 외부 네트워크 경계에서 재실행해 `3beb97635a067403ccc6486dd4a7e71be6d4d8fa refs/heads/main`을 확인했다.

해석:
로컬 HEAD, 원격 추적 참조, GitHub의 실제 `origin/main`이 모두 같은 커밋이다.

명령:
`git show --stat --oneline 3beb976`

결과:
- 커밋 제목: `fix: stabilize rat movement and sync verification state`.
- rename 감지 기준 변경 엔트리 38개, 1,713 insertions, 370 deletions.

명령:
`git diff-tree --no-commit-id --name-status -r -M 3beb976`

결과:
- rename 감지 기준 엔트리 수: 38개.
- rename을 삭제·추가 양끝 경로로 풀어 센 원시 endpoint는 45개다. 차이 7개는 `R050` 1건, `R100` 5건, `R073` 1건의 이동이며 예상 밖 파일 추가가 아니다.

해석:
pre-commit QA가 확인한 “38개”와 실제 커밋이 rename 감지 기준으로 일치한다.

명령:
커밋의 원시 endpoint 경로에서 제외 범위 포함 수 집계.

결과:
- `UnityProject/ProjectSettings/ProjectSettings.asset`: 0개.
- `_workspace/previews/`: 0개.
- `Builds/`: 0개.

해석:
필수 제외 범위가 실제 커밋에도 포함되지 않았다.

명령:
`git status --short`, `git diff --cached --name-only`, `git diff --name-status`

결과:
- post-push QA 기록을 추가하기 전 index 변경: 0개.
- post-push QA 기록을 추가하기 전 tracked worktree 변경: `UnityProject/ProjectSettings/ProjectSettings.asset` 1개.
- post-push QA 기록을 추가하기 전 untracked: `_workspace/previews/`만 존재.

해석:
커밋 범위 밖의 사용자 로컬 변경 두 종류가 그대로 보존됐고, 커밋 누락으로 의심되는 staged 변경은 없다.

명령:
`CURRENT.md`, 공유 상태판, 본 active 패킷의 기능 상태·재개 조건 검색 및 active 경로 존재 확인.

결과:
- `_workspace/active/2026-07-16-natural-alert-build-loop-verification/`가 존재한다.
- `CURRENT.md`와 상태판은 게임 창 캡처 `SetIsBorderRequired 0x80004002` 차단, 화면 미확인 입력 중단, 자연 성공 루프 미검증을 유지한다.
- 상태판은 QA `차단`, 총괄 `보류`, 기능 완료가 아님을 유지하고 자연 경계도 작업을 다음 후보에 중복하지 않는다.
- 재개 조건은 Computer Use 게임 창 캡처 복구 또는 사용자의 같은 연속 세션 단계별 화면·해당 `Player.log` 제공으로 유지된다.

해석:
자연 경계도 기능 상태와 재개 경계는 push 후에도 변하지 않았다.

### 상태판·포인터 post-push 유의

- `CURRENT.md`의 바로 이어서 할 작업 1~2와 상태판의 `현재 로컬 변경`·`현재 릴리즈 작업`은 아직 “선별 스테이징·커밋·푸시 예정/미커밋” 단계로 적혀 있다.
- 실제 Git은 이미 `3beb976` push 완료 상태이므로 위 릴리즈 단계 문구는 post-push 현재 사실보다 한 단계 뒤다.
- 이번 QA는 두 QA 기록 외 다른 파일 수정이 금지되어 `CURRENT.md`와 상태판을 직접 고치지 않았다.
- 이 문구 불일치는 push 내용·원격 반영을 무효로 만들지는 않지만, 다음 작업 시작 전 문서/릴리즈 담당자가 완료 hash와 post-push 결과로 동기화해야 한다.

### 미검증 항목

- 자연 경계도 100%부터 변이 적용 RatHost 복귀까지의 기능 성공 루프는 여전히 미검증이다.
- 이번 QA는 Unity·Windows 빌드를 재실행하지 않았다.

### 완료 판단

`push 반영 적합 — 자연 경계도 기능 완료 아님, 상태판·포인터 post-push 문구 동기화 필요`

### 완료 판단 근거

로컬 HEAD, 원격 추적 참조, GitHub 실제 `origin/main`이 `3beb97635a067403ccc6486dd4a7e71be6d4d8fa`로 일치한다. 커밋은 rename 감지 기준 38개 엔트리이며 세 제외 범위를 포함하지 않았다. 기록 추가 전 worktree에는 의도대로 범위 밖 `ProjectSettings.asset`과 `_workspace/previews/`만 남았다. 자연 경계도 작업은 active·QA `차단`·총괄 `보류`와 재개 조건을 유지한다. 따라서 push 반영은 적합하지만, 이미 끝난 릴리즈 단계를 예정으로 적은 `CURRENT.md`·상태판 문구는 후속 동기화가 필요하다.

## 2026-07-24 post-push 문서 동기화 QA

### 확인 결과

- HEAD, 원격 추적 참조, GitHub `origin/main`은 모두 `3beb97635a067403ccc6486dd4a7e71be6d4d8fa`로 일치한다.
- 상태판 상단의 최신 커밋, 원격 일치, rename 감지 기준 38개, `ProjectSettings.asset`·`_workspace/previews/`·`Builds/` 포함 0은 실제 Git과 일치한다.
- 문서 동기화 대상은 `verification.md`, `agent-activity.md`, `CURRENT.md`, `current-task-board.md` 정확히 4개이며 예상 밖 문서 변경과 누락은 0개다.
- `CURRENT.md`는 완료된 스테이징·커밋·푸시 단계를 제거했고 EditMode 회귀 테스트 일괄 실행을 우선 작업으로 둔다. Computer Use 게임 창 캡처 복구 또는 사용자의 같은 세션 화면·`Player.log` 제공이라는 자연 경계도 재개 조건도 유지한다.
- 자연 경계도 작업은 active·QA `차단`·총괄 `보류`이며 다음 후보에 중복되지 않는다.
- `ProjectSettings.asset`은 기존 `APP_UI_EDITOR_ONLY` 1줄 변경 그대로이고 `_workspace/previews/`는 기존 untracked 파일 1개로 남아 있다. 두 범위는 문서 동기화 대상에 포함되지 않았다.
- 네 문서의 `git diff --check`는 통과했다.

### 불일치

- `current-task-board.md` 하단의 `자연 성공 루프 엄격 검증 경계`에는 아직 “이번 선별 커밋에 포함한다”가 남아 있다.
- 같은 문서의 `엄격 검증 차단 판정`에는 “선별 커밋을 진행한다”가 남아 있다.
- 상단은 `3beb976` push 완료로 갱신됐으므로 위 두 완료 전 시제와 현재 사실이 충돌한다. 각각 반영 완료/반영됨으로 동기화해야 한다.
- 상태판의 `현재 로컬 변경`은 네 문서 동기화 커밋 이후 남을 범위 밖 잔여 상태로는 맞지만, 현재 worktree에는 이 네 문서도 미커밋이다. 문구를 post-sync 잔여 기준으로 명시하면 커밋 전후 해석 충돌을 줄일 수 있다.

### 완료 판단

`커밋 범위 수정 필요 — 경로 범위와 diff-check는 적합하나 상태판 완료 전 시제 2곳 동기화 필요`

자연 경계도 기능 판정은 변경하지 않는다.

## 2026-07-24 post-push 문서 동기화 QA 재대조

- 예정 변경은 `verification.md`, `agent-activity.md`, `CURRENT.md`, `current-task-board.md` 정확히 4개이며 누락·예상 밖 경로는 0개다.
- 상태판의 완료 전 시제 2곳은 `포함해 반영했다`, `선별 커밋·푸시를 완료했다`로 수정됐다. 로컬 상태도 post-push 문서 4개 동기화 예정과 범위 밖 ProjectSettings/previews로 분리됐다.
- `CURRENT.md`는 EditMode 회귀 테스트 우선과 자연 경계도 재개 조건을 유지한다.
- 자연 경계도 active·QA `차단`·총괄 `보류` 및 후보 비중복은 유지된다.
- `ProjectSettings.asset`과 `_workspace/previews/`는 예정 4개 경로에서 제외됐고, 네 문서 `git diff --check`는 종료 코드 0이다.

### 재판정

`문서 동기화 커밋 범위 적합`

이 판정은 이전 상태판 시제 불일치에 대한 `수정 필요`를 해제한다. 자연 경계도 기능 판정은 변경하지 않는다.

## 2026-07-24 상태판 자기참조 보정 QA

- `최신 커밋`을 `기능 변경 기준 커밋 3beb976`으로 바꿔 이후 문서 커밋이 추가돼도 기능 변경 기준이 stale하지 않는다.
- 기능 변경과 후속 검증 문서 동기화가 `origin/main`에 반영 완료됐다는 문구는 현재 `23a9770` 상태와 일치하며, 이번 QA 기록이 후속 커밋돼도 기존 완료 사실을 계속 정확히 표현한다.
- 후속 검증 기록 5개를 별도 문서 동기화 커밋으로 반영 완료했다는 문구는 `23a9770`의 실제 5개 문서와 일치한다.
- `범위 밖 로컬 변경`은 ProjectSettings와 previews를 별도 범위로 서술해 현재 상태와 맞고, 다른 문서 변경이 생겨도 “전체 로컬 변경은 이것뿐”이라고 오해할 자기참조 표현을 제거했다.
- 자연 경계도 active·QA `차단`·총괄 `보류`, 후보 비중복, EditMode 회귀 우선, 재개 조건은 유지된다.
- 상태판 diff-check는 종료 코드 0이다.

### 판정

`상태판 자기참조 보정 커밋 범위 적합`

자연 경계도 기능 판정은 변경하지 않는다.
