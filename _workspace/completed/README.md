# 완료된 작업

이 폴더는 완료된 에이전트 작업의 추적 기록을 보관한다.

## 최근 완료 보관

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

- `task.md` 또는 `task-r1-summary.md`: 위험 등급에 맞는 최초 작업 배정 내용
- `work-log.md`: 진행 중 기록
- `completion-report.md`: 무엇을 어떻게 완료했는지 정리
- `verification.md`: 실행한 검증과 미검증 항목
- `artifacts/`: 작업 산출물 보관 폴더

## 완료 처리 절차

1. 완료 폴더를 만든다.
2. 진행 중 작업 폴더의 기록 파일을 완료 폴더로 복사하거나 이동한다.
3. `completion-report.md`를 작성한다.
4. `verification.md`를 작성한다.
5. 사용자 승인 필요 항목이 남아 있으면 완료가 아니라 `승인 대기`로 표시한다.
6. 최종 보고에 완료 폴더 경로를 포함한다.
