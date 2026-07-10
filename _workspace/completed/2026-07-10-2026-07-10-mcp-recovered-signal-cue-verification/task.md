# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-10-mcp-recovered-signal-cue-verification`
- 작업명: MCP 승인 복구 후 면역 신호 억제 접근 예고 누락 검증
- 상태: 완료
- 생성일: 2026-07-10
- 담당 에이전트: QA/검증 에이전트
- 보조 에이전트: 프로젝트 총괄 관리자 에이전트
- 사용 스킬: unity-verification-runner

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| QA/검증 에이전트 | Unity MCP 검증 | Play 진입, 면역 신호 억제 진입, HUD 상태, 콘솔 Error/Warning 확인 | `verification.md` |
| 프로젝트 총괄 관리자 에이전트 | 내부 승인 | QA 기록 충분성, 범위 충돌, 미검증 항목 확인 | `completion-report.md` |

## 구현 담당 확인

- 코드/테스트 변경 담당: 해당 없음
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: 해당 없음
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 해당 없음

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: Unity 플레이어블 검증 완료 주장
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 아니오

## 목적

`2026-07-10-signal-suppression-approach-cue` 작업에서 Unity MCP 승인 해제로 남았던 F6 진입/HUD 자동 플레이 검증과 콘솔 확인을 마무리한다.

## 입력 자료

- `_workspace/active/2026-07-10-signal-suppression-approach-cue/task.md`
- `_workspace/active/2026-07-10-signal-suppression-approach-cue/verification.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `.agents/qa-verification-agent.md`
- `.codex/skills/unity-verification-runner/SKILL.md`

## 해야 할 일

1. Unity MCP 연결과 프로젝트 가이드라인 조회가 가능한지 확인한다.
2. `RatHostPrototype` 씬에서 Play 상태에 진입한다.
3. 면역 신호 억제 미니게임에 진입한다.
4. 접근 예고 상태, cue intensity, `신호 접근`/`지금 차단` HUD 텍스트, 마커 펄스 관련 상태를 확인한다.
5. Unity Console Error/Warning을 확인한다.
6. Play 상태를 종료하고 검증 기록을 남긴다.

## 산출물

- `verification.md`
- `agent-activity.md`
- `completion-report.md`

## 에이전트 수행 이력 기록

- `agent-activity.md` 생성 여부: 예정
- 담당 에이전트별 수행 내용 기록 여부: 예정
- 위임/검토/승인 판정 기록 여부: 예정

## 금지 범위

- 게임플레이 코드 수정
- 씬 저장 또는 프리팹/에셋 변경
- ProjectSettings 변경
- 새 패키지 설치
- 면역 신호 억제 기능 범위 확장

## 승인 필요 항목

- 없음. 사용자가 MCP 복구 후 누락 검증 마무리를 요청했다.

## 완료 기준

- Unity MCP 호출이 성공한다.
- Play 진입과 종료가 가능하다.
- 면역 신호 억제 상태에서 접근 cue와 정확 판정 상태가 확인된다.
- 콘솔 Error/Warning 결과가 기록된다.
- QA/검증 에이전트 기록과 프로젝트 총괄 관리자 판정이 남는다.
