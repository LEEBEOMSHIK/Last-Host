# 완료된 작업

이 폴더는 완료된 에이전트 작업의 추적 기록을 보관한다.

## 최근 완료 보관

- `2026-08-06-2026-08-06-workspace-recording-lightweight`: 신규 작업 R0 무폴더·R1 단일 기록·R2/R3 기본 두 파일과 조건부 기록 구조, 독립 QA PASS·총괄 내부 승인, Unity/MCP/build 0
- `2026-08-06-2026-08-06-virus-character-concept-v1`: 사용자 제공 박테리오파지 기본 외형 reference 반영, correction 1 QA PASS·총괄 내부 승인, 후속 2D 시트·Unity 적용 별도 승인
- `2026-08-05-2026-08-02-production2d-natural-occlusion-root-fix`: final `5cd81d7c…`, 전체 EditMode `203/203`, QA Play r3 PASS, 총괄 내부 승인, 사용자 자연 부분 가림 화면·쥐 본체 보존 수용, 상태-only 종결 Unity/QA 0
- `2026-08-05-2026-08-05-verification-loop-noise-reduction`: preflight/S0와 실제 run 구분, 구현·QA 반복 상한, 상태-only 최종 sync·보고 압축 운영 계약 반영, 정적 QA1·총괄1, Unity/MCP/build 0
- `2026-08-05-2026-08-05-rat-collision-surface-slide`: 구현자·독립 QA 각각 `16/16`, 사용자 C6 실제 WASD 수용, closeout QA·총괄 PASS, 비용 `주의`, Unity/MCP/build `5/0/0`
- `2026-08-03-2026-08-02-verification-current-state-contract`: 독립 QA `24/24`, 총괄 r2 내부 승인, 비용 `주의`, Unity/MCP/build 0, 운영 `a33164b` 원격 반영
- `2026-08-03-2026-08-02-verification-harness-cost-guards`: 후속 R3로 superseded, G1~G8 차단 유지, 비용 `과다 — 부분 회피 가능`
- `2026-08-02-2026-08-02-loop-harness-efficiency-audit`: QA r6 PASS, 총괄 내부 승인, 비용 `과다 — 부분 회피 가능`, 운영 `533152e`·상태 동기화 `2eff18d` 원격 반영

## 완료 폴더 규칙

완료 폴더는 다음 형식을 사용한다.

```text
_workspace/completed/YYYY-MM-DD-<작업ID>/
```

예시:

```text
_workspace/completed/2026-06-29-2026-06-29-agent-workspace/
```

## 완료 폴더 필수 파일

- R1: `record.md`
- R2: `task.md`, `verification.md`
- R3: 기본 `task.md`, `verification.md`
- 조건부: 기본 파일에 안전하게 통합할 수 없는 실제 책임·규제/릴리즈 추적이면 R3 분리 기록, 세션 중단·외부 차단·실제 인계가 있었으면 `handoff.md`, 원래 위치 참조로 부족한 indispensable canonical 증거가 있으면 `artifacts/`

이 구조는 2026-08-06 이후 신규 작업에만 적용한다. 기존 완료 이력은 삭제하거나 새 구조로 다시 쓰지 않는다.

## 완료 처리 절차

1. 진행 중 최소 작업 폴더를 완료 경로로 이동한다.
2. 완료 시 새 packet·`completion-report.md`·중복 artifact를 생성하거나 복제하지 않는다.
3. 필요한 판정과 최종 상태를 해당 canonical 기록에 반영한다.
4. 사용자 승인 필요 항목이 남아 있으면 완료가 아니라 `승인 대기`로 표시한다.
