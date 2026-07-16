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
