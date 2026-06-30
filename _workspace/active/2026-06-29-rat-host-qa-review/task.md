# 작업 배정서

## 기본 정보

- 작업 ID: `2026-06-29-rat-host-qa-review`
- 작업명: 쥐 숙주 구현 계획 QA/검증 검토
- 상태: 진행 중
- 생성일: 2026-06-29
- 담당 에이전트: QA/검증 에이전트
- 보조 에이전트: 게임플레이 루프 에이전트
- 사용 스킬: `$unity-verification-runner`

## 목적

쥐 숙주 구현 계획 초안의 수용 기준과 검증 시나리오가 핵심 루프 완료 판단에 충분한지 검토하고, 구현 후 필요한 검증 체크리스트를 정의한다.

## 입력 자료

- `AGENTS.md`
- `docs/prototype/rat-host-prototype.md`
- `docs/prototype/rat-host-approval-packet.md`
- `docs/unity/unity-baseline-report.md`
- `.agents/qa-verification-agent.md`
- `.codex/skills/unity-verification-runner/SKILL.md`
- `.codex/skills/unity-verification-runner/references/verification-rules.md`
- `_workspace/active/2026-06-29-rat-host-plan-agent/artifacts/rat-host-implementation-plan-draft.md`

## 해야 할 일

1. 구현 계획 초안의 수용 기준이 쥐 숙주 핵심 루프 성공 기준을 충분히 덮는지 검토한다.
2. 수동 플레이 체크리스트를 성공 루프, 실패 루프, 변이별 확인으로 정리한다.
3. EditMode/PlayMode 테스트 권장 범위를 제안한다.
4. 구현 완료 조건에 컴파일, 플레이, 빌드 중 무엇을 포함할지 제안한다.
5. UI 겹침과 카메라 프레이밍 확인 기준을 제안한다.
6. 미검증 항목과 남은 위험을 분리한다.

## 산출물

- `_workspace/active/2026-06-29-rat-host-qa-review/artifacts/qa-review.md`
- 필요 시 `_workspace/active/2026-06-29-rat-host-qa-review/handoff.md` 갱신

## 금지 범위

- Unity 테스트 실행
- Unity 씬 또는 코드 수정
- 빌드 실행
- 구현 범위 확대

## 승인 필요 항목

- 구현 후 테스트/빌드 실행 범위 승인
- 수동 플레이 체크 기준 승인

## 완료 기준

- `qa-review.md`에 검증 대상, 실행 가능 검증, 미검증 항목, 남은 위험, 완료 판단 기준이 정리되어 있다.
- 계획 초안의 검증 질문에 직접 답한다.
- Unity 프로젝트 파일은 변경되지 않았다.
