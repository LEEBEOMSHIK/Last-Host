# 진행 중 작업

이 폴더는 현재 진행 중인 에이전트 작업을 보관한다.

## 현재 포인터

- 현재 이어받을 작업: `2026-08-02-production2d-visual-overlap-correction`
- 상태: 내부 승인 가능, 사용자 실제 WASD·작은 소품 완전 가림 확인 대기, 선별 커밋 준비 중
- 루프·하네스 비용 감사는 `_workspace/completed/2026-08-02-2026-08-02-loop-harness-efficiency-audit/`로 완료 보관했다.

## 새 작업 생성 절차

1. R0은 작업 폴더를 만들지 않는다.
2. R1은 `record.md`, R2/R3는 `task.md`+`verification.md`를 기본 배치한다.
3. R3 분리 기록은 기본 파일에 안전하게 통합할 수 없는 실제 추적 필요가 있을 때만, `handoff.md`는 세션 중단·외부 차단·실제 인계 때만, `artifacts/`는 원래 위치 참조로 부족한 indispensable canonical 증거가 있을 때만 만든다. 빈 폴더·빈 템플릿은 만들지 않는다.

## 진행 중 기록 원칙

- 중요한 판단은 등급별 canonical 기록에 통합한다. R3 `work-log.md`도 장기 순서 이력을 기본 파일에 안전하게 합칠 수 없을 때만 쓴다.
- 다른 에이전트가 실제로 이어받아야 하는 내용만 `handoff.md`에 남긴다.
- 승인 필요 항목은 작업 중에도 계속 갱신한다.
- 완료된 작업은 새 완료 패킷을 만들지 않고 같은 최소 작업 폴더를 `completed/` 아래로 이동한다.
