# 검증 기록

## 검증 대상

사용자가 승인한 `3D 환경·게임플레이 루트 + 저폴리 3D 캐릭터 제작 원본 + 쥐 프로토타입의 8방향 프리렌더드 캐릭터 스프라이트` 방향이 살아 있는 프로젝트 문서에 일관되게 반영되었고, 실제 아트 에셋 제작과 Unity 적용은 후속 작업으로 분리되었다는 주장.

## 실행한 검증

### 1. 변경 범위와 Git 상태 대조

명령:

```powershell
git status --short
git diff --stat
git diff --name-status
git status --short -- UnityProject Builds
git diff --name-only -- UnityProject Builds
git status --short -- .codex/config.toml _workspace/previews
```

결과:

- 방향 문서와 작업 패킷 변경을 확인했다.
- `UnityProject/`, `Builds/`의 추적·미추적 변경은 0건이다.
- `.codex/config.toml`은 별도 수정 상태, `_workspace/previews/`는 별도 미추적 상태로 남아 있다.

해석:

- 이번 변경은 Unity 코드·씬·프리팹·ProjectSettings·빌드·실제 아트 에셋을 건드리지 않았다.
- 두 기존 변경은 현황판에서 작업 범위 밖 로컬 변경으로 분리되어 있으며 현재 작업 커밋 대상이 아니다.

### 2. 용어·범위·승인 경계 대조

명령:

```powershell
rg -n --glob '*.md' --glob '!_workspace/completed/**' --glob '!_workspace/previews/**' --glob '!UnityProject/**' '(실시간|런타임).{0,20}3D|3D 캐릭터|3D 모델.*사용|스프라이트.*(전환|대체).*않|도트풍 저폴리 3D' AGENTS.md docs .agents .codex/skills _workspace/active/2026-07-16-prerendered-character-sprite-direction
rg -n --glob '*.md' --glob '!_workspace/completed/**' --glob '!_workspace/previews/**' --glob '!UnityProject/**' '캐릭터.*(64×64|96×96)|PPU|프레임 수|재생 속도|기존 게임플레이 검증|새 비주얼|플레이스홀더 화면' AGENTS.md docs .agents .codex/skills _workspace/active/2026-07-16-prerendered-character-sprite-direction
```

결과:

- 상위 규칙, 공식 프로토타입, 구현 계획, 비주얼 가이드, AI 아트 작업 순서, 에이전트·스킬 기준은 `도트풍 하이브리드 2.5D`, `3D 환경·게임플레이 루트`, `저폴리 3D 원본`, `8방향 프리렌더드 스프라이트` 경계를 공유한다.
- 순수 2D 전환을 금지하고, 콜라이더·이동·위험 판정·상호작용은 3D 게임플레이 루트에 유지한다고 명시한다.
- 정확한 셀 해상도, PPU, 동작별 프레임 수, 재생 속도, 그림자·깊이 정렬은 후속 시험 에셋과 시각·플레이 검증 뒤 결정한다고 명시한다.
- `agent-activity.md`와 `work-log.md`는 기존 게임플레이 검증은 유효하지만 현재 3D 플레이스홀더 화면은 새 비주얼 품질 증거가 아니며 실제 통합 뒤 별도 검증이 필요하다고 구분한다.

해석:

- 핵심 방향과 미확정 사양 분리는 통과했다.
- 최초 지적 4건과 추가 승인 연혁 1건의 수정 사항을 재대조했고 모두 해소되었다.

### 3. Markdown 형식·링크 대조

명령:

```powershell
git diff --check
# 변경 Markdown과 작업 패킷의 인라인 상대 링크를 각 문서 기준 절대 경로로 해석해 Test-Path 대조
```

결과:

- `git diff --check`: 오류 0건.
- 변경 Markdown 인라인 링크: 누락 대상 0건.
- 문서에서 직접 참조한 핵심 경로와 작업 패킷 경로가 존재한다.

해석:

- 공백·충돌 표식·깨진 인라인 링크 문제는 발견하지 못했다.

## 재검증 결과

1. `docs/design/game-design-summary.md`는 8방향 표시 방식을 현재 쥐 프로토타입으로 한정하고, 이후 숙주는 각 마일스톤 승인에 따른다고 명시한다.
2. `docs/design/visual/pixel-lowpoly-3d-production-guide.md`의 대표 제목은 `도트풍 하이브리드 2.5D 제작 가이드`로 표준 용어와 일치한다.
3. `docs/prototype/approvals/rat-host-approval-packet.md`는 `2026-06-30` 최초 프로토타입 승인과 `2026-07-16` 비주얼 방향 추가 승인을 분리하고 적용 범위를 각각 1~10번과 11번으로 기록한다.
4. `docs/prototype/official/rat-host-prototype.md`도 두 승인 날짜를 분리하고 8방향 비주얼 항목에 `2026-07-16 추가 승인`을 표시한다.
5. `docs/project-handoff/current-task-board.md`는 현재 미커밋 문서·작업 패킷과 작업 범위 밖 `.codex/config.toml`, `_workspace/previews/`를 분리한다.

추가 수정이 필요한 직접 충돌 문구, 깨진 링크 또는 범위 확대 문장은 발견하지 못했다.

## 완료 보관·현황판 최종 대조

명령:

```powershell
$done = '_workspace/completed/2026-07-16-2026-07-16-prerendered-character-sprite-direction'
Test-Path $done
Test-Path '_workspace/active/2026-07-16-prerendered-character-sprite-direction'
@('task.md','work-log.md','agent-activity.md','handoff.md','verification.md','completion-report.md') | ForEach-Object { Test-Path (Join-Path $done $_) }
@('_workspace/active/2026-07-16-natural-alert-build-loop-verification','docs/project-handoff/manual-play-checklist.md','.codex/config.toml','_workspace/previews') | ForEach-Object { Test-Path $_ }
$hold = @('사용자 수동 플레이 체감 확인')
$candidate = @('자연 경계도 엄격 검증 재개','8방향 쥐 시험 에셋')
@($hold | Where-Object { $candidate -contains $_ }).Count
git rev-parse --short HEAD
git rev-parse --short origin/main
git status --short --branch
git status --short -- UnityProject Builds
git diff --name-only -- UnityProject Builds
git diff --cached --name-only
```

결과:

- 완료 보관 경로는 존재하고 이전 active 경로는 존재하지 않는다.
- 완료 폴더 필수 파일 `task.md`, `work-log.md`, `agent-activity.md`, `handoff.md`, `verification.md`, `completion-report.md`가 모두 존재한다.
- 현황판 최근 완료 5건의 경로, 차단 중인 자연 경계도 엄격 검증 active 경로, 수동 플레이 체크리스트 경로가 모두 존재한다.
- 보류 항목은 `사용자 수동 플레이 체감 확인` 1건, 다음 작업 후보는 `자연 경계도 엄격 검증 재개`, `8방향 쥐 시험 에셋` 2건이며 두 집합의 중복은 0건이다.
- `8방향 쥐 시험 에셋`은 후속 승인 전 후보로만 기록되고, 실제 생성 범위·도구·후보 수·Unity 반입과 정확 사양은 미승인 상태로 분리된다.
- HEAD와 로컬 `origin/main` 참조는 모두 `d5d1ade`이며 현황판의 최신 커밋·원격 반영 기록과 일치한다.
- 스테이징된 파일은 0건이다. `.codex/config.toml`은 별도 수정, `_workspace/previews/`는 별도 미추적 상태이며 현황판의 작업 범위 밖 기록과 일치한다.
- `UnityProject/`, `Builds/`의 추적·미추적 변경은 0건이다.

해석:

- 완료 경로, 진행·완료·보류·후보 상태, Git 상태, 제외 파일이 실제 워크트리와 일치한다.
- 완료 보관 후에도 자연 경계도 엄격 검증의 active·QA `차단`·총괄 `보류` 상태는 변경되지 않았다.

## MCP 플레이 체크

- 대상 아님.
- 이번 작업은 문서·승인·운영 기준만 변경했고 Unity 플레이어블 변경이 없다.

## 검증하지 못한 항목

- 실제 쥐·바이러스·백혈구 3D 원본 제작 품질.
- 8방향 스프라이트 출력, Import, SpriteRenderer·Animator·방향 매핑 구현.
- 방향 전환 팝핑, 피벗 흔들림, 발 미끄러짐, 그림자·가림·깊이 정렬, 환경 조명 일치.
- 새 비주얼이 적용된 Unity 실행·빌드 화면.

이 항목들은 이번 완료 주장에 포함하지 않으며, 별도 사용자 승인과 Unity 통합·시각 QA가 필요하다.

## 남은 위험

- 실제 통합 시 방향 경계 팝핑, 피벗·접지, 구운 명암과 3D 환경 조명, 가림·깊이 정렬 문제가 남는다.
- 정확한 셀 해상도·PPU·프레임 수를 시험 에셋 전에 확정하면 현재 문서의 승인 경계를 벗어난다.
- 커밋 시 작업 범위 밖 `.codex/config.toml`, `_workspace/previews/`를 포함하지 않도록 경로를 대조해야 한다.

## 완료 판단

**완료 가능**

핵심 비주얼 방향, 쥐 프로토타입 한정 범위, 순수 2D 오해 방지, 미확정 사양 분리, 기존 게임플레이 검증과 새 비주얼 증거의 경계, 승인 연혁, 완료 보관 경로, 현황판의 진행·완료·보류·후보, Git 상태·제외 파일, UnityProject·Builds 무변경을 확인했다. 실제 에셋 제작과 Unity 적용을 이번 완료 주장에 포함하지 않는 조건으로 문서 정리와 완료 보관은 완료 가능하다.
