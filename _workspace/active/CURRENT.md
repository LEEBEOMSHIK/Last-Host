# 현재 세션 포인터

## 현재 이어받을 작업

- 작업 ID: `2026-07-16-natural-alert-build-loop-verification`
- 상태: 차단 — Windows Computer Use 연결 복구 또는 사용자 수동 검증 필요.
- 작업 경로: `_workspace/active/2026-07-16-natural-alert-build-loop-verification/`
- 최신 사용자 요청: “일단 커밋 푸쉬해” — 기능 완료가 아닌 현재 차단 상태와 누적 완료 보관·현황판 기록의 커밋·푸시 예외 지시.

## 먼저 읽을 파일

1. `_workspace/active/2026-07-16-natural-alert-build-loop-verification/task.md`
2. `_workspace/active/2026-07-16-natural-alert-build-loop-verification/handoff.md`
3. `docs/project-handoff/current-task-board.md`

## 바로 이어서 할 작업

1. QA/총괄이 사용자 커밋 예외 범위, `.codex/config.toml` 제외, Unity/Builds 무변경을 재대조한다.
2. 재검토 통과 뒤 현재 차단 기록과 누적 완료 보관·현황판 기록만 커밋·푸시한다. 기능 완료·보관은 하지 않는다.
3. 커밋 후에도 Computer Use helper 복구 또는 사용자 수동 증거로 엄격 검증을 재개한다.

## 제외하거나 건드리면 안 되는 변경

- `.codex/config.toml`은 작업 범위 밖 로컬 변경으로 유지하며 수정·복원·스테이징·커밋하지 않는다.
- 사용자 수동 플레이 체감 확인은 상태판의 보류 항목 한 곳에만 유지한다.
- Unity 코드·씬·테스트·ProjectSettings·패키지·에셋, `Builds/`, `docs/project-handoff/manual-play-checklist.md`는 수정하지 않는다.
- 이전 완료 작업 패킷을 수정하거나 이동하지 않는다.
- `F6`, 직접 상태 주입, Unity Editor 대체 검증을 이번 Windows 빌드 자연 성공 루프의 통과 근거로 사용하지 않는다.

## 마지막 정상·실패·재개 조건

- 마지막 정상: 기존 빌드 존재·SHA-256 식별, 시작 전 `UnityProject/`·`Builds/` 변경 0, 정적 구성상 자연 경계도 상승 경로 확인.
- 실패: Computer Use native pipe가 초기 연결과 세션 초기화·재-bootstrap 후 허용 재시도 1회 모두 `os error 2`로 실패.
- 미검증: 빌드 실행, 창 포커스, 실제 입력, 자연 경계도 100%, 단계별 화면, 조각 3개, 변이 선택·복귀, 동일 세션 `Player.log`.
- 재개 조건: Computer Use helper의 `list_apps` 정상 응답 또는 사용자 수동 플레이의 연속 단계·해당 세션 로그 증거.
- QA/검증 에이전트 판정: `차단`.
- 프로젝트 총괄 관리자 판정: `보류` — 핵심 실제 플레이 증거 미검증으로 완료·보관·커밋·푸시 금지.
- 커밋·푸시: 사용자 명시 지시로 차단 기록 커밋 예외 허용. 기능 완료가 아니며 QA/총괄 범위 재검토 전에는 스테이징하지 않는다.

## 보류와 검증 경계

- 이전 F6 `면역 신호 억제` 빌드 성공 루프는 자연 경계도 100%와 기본 백혈구 회피 성공의 증거가 아니다.
- 성공 증거는 같은 실행 세션의 창 포커스, 자연 전환, 조각 3개, 변이 선택, RatHost 복귀 화면과 `Player.log`로 연결해야 한다.
- 사용자 수동 플레이 체감·난이도·무설명 이해 여부는 계속 보류한다.
- 누적 완료 작업 보관과 상태판 변경은 최종 커밋에 포함하되 `.codex/config.toml`은 제외한다.
- PowerShell, SendKeys, Start-Process, 별도 네이티브 입력으로 Computer Use 연결 실패를 우회하지 않는다.
- 차단 기록 커밋 후에도 현재 작업은 active, QA `차단`, 총괄 `보류`, 재개 조건 A·B를 유지한다.

## 갱신 정보

- 마지막 갱신: 2026-07-16 10:52 KST
- 갱신자: 프로젝트 조정/문서 릴리즈 에이전트
