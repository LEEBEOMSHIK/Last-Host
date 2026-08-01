# 에이전트 작업영역

`_workspace`는 에이전트 간 작업을 배정하고, 진행 기록을 남기고, 완료 후 추적 가능한 결과 폴더를 보관하는 프로젝트 로컬 작업영역이다.

## 목적

- 에이전트별 작업 배정 내용을 한곳에 모은다.
- 어떤 에이전트가 어떤 일을 맡고 어떤 산출물을 냈는지 작업 단위로 남긴다.
- 작업 중 의사결정, 조사 내용, 산출물을 잃지 않도록 기록한다.
- 세션이나 토큰이 끊겨도 다른 AI 또는 다른 세션이 현재 상태를 이어받을 수 있도록 최소 상태 포인터를 유지한다.
- 작업 완료 후 별도 완료 폴더를 만들어 무엇을 했고 어떻게 완료했는지 추적한다.
- 사용자 승인 여부와 검증 결과를 작업 단위로 남긴다.

## 사용자 보고 원칙

`_workspace` 전체는 이력 확인과 에이전트 간 작업 전달을 위한 영역이다. 사용자에게는 이 폴더 전체를 확인 대상으로 제시하지 않고, 직접 판단해야 하는 산출물만 골라 `확인할 파일`과 `핵심 확인 사항`으로 요약해 전달한다.

## 루프 엔지니어링에서의 역할

`_workspace`는 루프 엔지니어링의 상태 저장소다. 진행 중 작업은 `active/`에 두고, 루프가 검증과 보고까지 끝나면 `completed/`로 옮긴다. 루프 진행 중 승인 게이트, 범위 충돌, 검증 불가, 에이전트 산출물 충돌, 에이전트 수행 이력 누락, 작업 기록 누락이 생기면 완료 처리하지 않고 사용자에게 문제 사안으로 보고한다.

위험 등급, 최소 역할, S0~S7 fail-fast, 검증 무효화, Unity lease, canonical evidence와 완료·커밋 차단 조건은 `docs/agents/loop-engineering-gates.md`가 유일 실행 기준이다. `_workspace` 문서는 그 실행 사실만 저장하며 별도 게이트를 만들지 않는다.

- R0은 작업 패킷이 필요 없다. R1은 `_workspace/templates/task-r1-summary.md`를 사용하고, R2/R3는 `_workspace/templates/task.md`의 정식 S0 charter를 사용한다.
- R1 요약형은 원증상·완료 주장·변경 파일과 owner·표적 테스트·금지 범위·correction cycle·QA·총괄만 기록한다. R2/R3의 상태 전이표·수명주기 matrix·전체 S0 표를 요구하지 않는다.
- `task.md` 또는 `task-r1-summary.md`는 계획·oracle·production 소유권, `agent-activity.md`는 실제 수행·인계, `verification.md`는 현재 revision의 증거, `completion-report.md`는 최종 판정만 소유한다.
- 중간 실패 산출물은 최소 반례와 핵심 로그 위치만 남기고, 최종 판정에는 canonical run_id 하나를 둔다.

## 작업 비용 기록

- 사용자 중앙 현황은 `docs/project-handoff/task-cost-dashboard.md`가 소유한다. `_workspace` 작업 문서는 각 행의 계획·실제 근거를 소유한다.
- R1은 `task-r1-summary.md`의 5줄 이하 비용 기록을 사용한다. R2/R3는 `task.md`의 계획/실제 표를 사용하며 새 per-task 비용 파일을 만들지 않는다.
- 작업 시작에는 계획 역할·검증 예산을 기록하고, blocker·correction에는 `agent-activity.md`·`verification.md`의 실제 실행 수와 무효·폐기를 갱신한다.
- 사용자 보고·완료·커밋 전에는 `completion-report.md`와 중앙 현황판에 필요한 비용·회피 가능 비용, `정상 / 주의 / 과다 / 미집계` 판정을 동기화한다.
- 정확한 토큰·금액은 플랫폼 계측값이 있을 때만 기록하며 추정하지 않는다. 근거가 없으면 `미집계`로 둔다.

## 폴더 구조

```text
_workspace/
  active/
    CURRENT.md
    <작업ID>/
      task.md 또는 task-r1-summary.md
      work-log.md
      agent-activity.md
      handoff.md
      artifacts/
  completed/
    <완료일>-<작업ID>/
      task.md 또는 task-r1-summary.md
      work-log.md
      agent-activity.md
      completion-report.md
      verification.md
      artifacts/
  templates/
    task-r1-summary.md
    task.md
    work-log.md
    agent-activity.md
    current.md
    handoff.md
    completion-report.md
    verification.md
```

## 세션 연속성 기준

토큰 압박, 세션 종료, 외부 도구 차단, 긴 작업 전환이 예상되면 먼저 `_workspace/active/CURRENT.md`와 현재 작업 폴더의 `handoff.md`를 갱신한다. 이 두 파일은 다음 세션의 첫 진입점이다.

`CURRENT.md`는 긴 기록 파일이 아니라 포인터다. 다음 항목만 짧게 둔다.

- 현재 이어받을 작업 ID
- 현재 상태: 설계 중, 구현 중, 검증 중, 커밋 전, 승인 대기, 차단
- 최신 사용자 요청 1문장
- 먼저 읽을 파일 3개 이하
- 바로 이어서 할 작업 3개 이하
- 건드리면 안 되는 변경 또는 제외 파일
- 마지막 갱신 시각과 갱신자

각 작업의 `handoff.md`는 다음 세션이 세부 맥락을 복원하는 문서다. 100~150줄 이하를 목표로 하고, 전체 작업 로그를 반복하지 않는다.

필수 내용:

- 최신 사용자 요청
- 현재 상태와 멈춘 지점
- 변경한 파일 목록
- 건드리면 안 되는 기존 변경
- 마지막으로 성공한 검증
- 실패했거나 차단된 검증
- 다음 세션이 바로 실행할 작업 3개 이하
- 사용자 결정이 필요한 항목
- candidate fingerprint, canonical run_id와 superseded run
- Unity lease owner, Play/Pause/scene/dirty, 임시 객체와 release 상태

## 토큰 경계와 인수인계 우선순위

토큰 경계는 숫자보다 작업 단계 기준을 우선한다. 다음 지점에서는 큰 새 작업으로 넘어가기 전에 인수인계를 갱신한다.

- 설계와 수용 기준이 확정된 직후
- 첫 코드 또는 운영 문서 수정 직전
- 코드나 문서 수정 완료 직후, 검증 전
- 검증 실패 후 원인 분석에 들어가기 전
- 검증 통과 후, 커밋 전
- 커밋 또는 푸시 직전
- Unity MCP, Unity Editor, 네트워크, 권한 같은 외부 상태에 막혔을 때

토큰 사용량을 체감 기준으로 병행한다.

- 약 60~70%: 다음 큰 단계로 넘어가기 전에 `handoff.md` 갱신
- 약 80% 이상: 새 구현을 시작하지 않고 검증, 정리, 인수인계 우선
- 약 90% 근처: 최종 답변보다 먼저 `CURRENT.md`와 `handoff.md` 갱신

가장 위험한 끊김 지점은 `구현 완료 후 검증 전`, `검증 실패 원인 분석 중`, `커밋 직전 staged/unstaged가 섞인 상태`다. 이 상태에서는 다음 세션이 다시 조사하지 않도록 반드시 현재 상태와 제외 파일을 적는다.

## 작업 ID 규칙

작업 ID는 다음 형식을 사용한다.

```text
YYYY-MM-DD-short-topic
```

예시:

```text
2026-06-29-agent-workspace
2026-06-29-unity-project-planning
2026-06-29-rat-host-loop-spec
```

## 기본 흐름

1. 작업을 시작할 때 `_workspace/active/<작업ID>/`를 만든다.
2. R1은 `templates/task-r1-summary.md`, R2/R3는 `templates/task.md`를 복사해 작업 배정 내용을 기록한다.
3. `templates/agent-activity.md`를 복사해 참여 에이전트, 역할, 담당 업무, 산출물, 판정을 기록한다.
4. 진행 중 `work-log.md`에 조사, 판단, 변경 내용을 누적한다.
5. 다른 에이전트로 넘길 내용은 `handoff.md`에 정리한다.
6. blocker·correction 때 실제 비용 proxy와 중앙 현황판 행을 갱신한다.
7. 작업 완료 시 `_workspace/completed/<완료일>-<작업ID>/`를 만든다.
8. 완료 폴더에 작업 기록, 에이전트 수행 이력, 검증 기록, 비용 요약과 완료 보고서를 남긴다.
9. 사용자 보고·커밋 전 중앙 비용 판정과 완료 폴더 경로를 최종 보고에 포함한다.

## 금지 사항

- `_workspace`에 빌드 산출물, 대용량 에셋, 임시 캐시를 보관하지 않는다.
- 승인되지 않은 구현 변경을 완료 폴더에 완료 처리하지 않는다.
- 검증하지 않은 내용을 완료로 기록하지 않는다.
- 어떤 에이전트가 어떤 일을 처리했는지 누락한 채 완료 처리하지 않는다.
- 사용자 승인 대기 항목을 누락한 채 완료 처리하지 않는다.
- 같은 criterion의 중복 전체 로그·PNG·CSV 세대를 canonical 증거처럼 보관하지 않는다.
- production 변경 뒤 이전 PASS를 현재 검증으로 재사용하지 않는다.

## 완료 조건

작업은 다음 문서가 완료 폴더에 있을 때 완료로 본다.

- `task.md` 또는 R1의 `task-r1-summary.md`
- `work-log.md`
- `agent-activity.md`
- `completion-report.md`
- `verification.md`

작업 중 산출물이 있으면 `artifacts/` 아래에 보관한다.

코드, 씬, ProjectSettings, 승인 문서, 운영 문서 변경은 `verification.md`에 QA/검증 에이전트 완료 판단이 있고, `completion-report.md`에 프로젝트 총괄 관리자 판정이 있어야 완료로 본다.

또한 `docs/project-handoff/task-cost-dashboard.md`의 해당 행이 실제 실행 수·무효/폐기·필요한 비용/회피 가능 비용·최종 판정과 마지막 갱신을 현재 근거로 반영해야 한다.
