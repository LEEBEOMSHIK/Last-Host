# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-29-rat-host-2d-stage3-mutation-return`
- 작업명: 3단계 2D 변이 선택·효과·쥐 숙주 복귀
- 상태: 승인됨 / 진행 중
- 생성일: 2026-07-29
- 담당 에이전트: 게임플레이 구현 에이전트, Unity 씬/통합 구현 에이전트
- 보조 에이전트: QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `rat-host-loop-builder`, `unity-prototype-planner`, `unity-verification-runner`

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| 게임플레이 구현 에이전트 | 코드·테스트 구현 | 선택 단일 적용, 복귀 상태, 세 변이 효과와 회귀 테스트 | `artifacts/gameplay-implementation.md` |
| Unity 씬/통합 구현 에이전트 | 씬·UI·2D 통합 | 세 선택지 UI, 입력/버튼 연결, 지정 통로와 Host 복귀 표시 | `artifacts/scene-integration.md` |
| QA/검증 에이전트 | 독립 검증 | 테스트, MCP Play, Console, 보호 diff, 필요 빌드 경계 | `verification.md`, `artifacts/qa-verification.md` |
| 프로젝트 총괄 관리자 에이전트 | 내부 승인 | 범위·승인·QA·보호 경계 판정 | `director-review.md` |

## 구현 담당 확인

- 코드/테스트 변경 담당: 게임플레이 구현 에이전트
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: Unity 씬/통합 구현 에이전트
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 해당 없음

## 루프 게이트

- 게이트 적용 대상: 예
- 적용 사유: Unity 게임플레이 코드·테스트·씬·UI 변경
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예

## 목적

Stage2 성공 인계 셸을 실제 세 가지 변이 선택 화면으로 바꾸고,
선택한 변이 하나를 적용한 상태로 2D 쥐 숙주 모드에 복귀시킨다.

## 입력 자료

- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/approvals/rat-host-2d-core-loop-migration-brief.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `_workspace/active/2026-07-28-rat-host-2d-stage2-minigame/handoff.md`
- 현재 `RatHost2DSessionController`와 차원 독립 `PrototypeSessionState`

## 해야 할 일

1. `MutationSelection`에서 숫자키 `1/2/3` 또는 UI 버튼으로 한 변이만 선택한다.
2. 성공 복귀 면역값 `25% + 내부 포착 흔적`을 적용하고 내부 런타임을 초기화한다.
3. `잠복 강화`가 오염 노출 면역 상승량에도 `0.55` 배율을 적용하게 한다.
4. `신경 조종`이 2D 쥐 이동 속도·조종력을 기존 수치에 맞게 향상한다.
5. `포유류 적응`이 지정 통로만 열고 다른 벽·수로 충돌은 유지하게 한다.
6. 선택 UI, 적용 변이 표시, Host 복귀 시 root·카메라·입력·충돌 상태를 연결한다.

## 산출물

- Unity 런타임 코드와 EditMode 회귀 테스트
- `RatHost2DPrototype` 씬 빌더·원본 씬의 Stage3 통합
- 세 변이 선택 UI와 지정 통로 플레이스홀더
- 구현·통합·QA·총괄 기록

## 에이전트 수행 이력 기록

- `agent-activity.md` 생성 여부: 예
- 담당 에이전트별 수행 내용 기록 여부: 진행 중
- 위임/검토/승인 판정 기록 여부: 진행 중

## 금지 범위

- 시간 경과 기본 면역 상승 재활성화
- 면역 신호 억제형 2D 이관
- 복수 백혈구·항체·보스·절차 생성
- 새 숙주·정식 다중 숙주 전이
- 최종 아트·PPU·타일·화면 규격 확정
- 새 패키지·ProjectSettings·렌더 파이프라인 변경
- 기존 3D 씬·2D 기술 샘플·레거시 산출물 삭제
- 사용자 `ProjectSettings.asset` 변경과 `_workspace/previews/` 수정

## 승인 필요 항목

- 2026-07-29 사용자 `다음 작업 진행해`를 현황판의 다음 후보인 Stage3 착수 승인으로 기록한다.
- 승인된 추천 규칙: 잠복 강화는 위험 행동/오염 노출 면역 상승량에 `0.55` 배율 적용.
- 범위 확장, 새 패키지, 최종 아트는 별도 승인 필요.

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 충족
- 담당 에이전트 산출물 확인: 대기
- 에이전트 수행 이력 확인: 대기
- 구현 담당 에이전트 확인: 충족
- 메인 에이전트 직접 구현 예외 사유 확인: 해당 없음
- QA/검증 에이전트 기록 확인: 대기
- 총괄 관리자 판정 확인: 대기
- 승인 게이트 확인: Stage3 착수 승인 기록
- 완료 판단에 영향을 주는 미검증 항목: 실제 Space 실패 확인은 Stage2 수동 미검증으로 별도 기록

## 완료 기준

- 성공한 경우에만 선택 화면이 표시된다.
- 중복 입력에도 한 보상에서 한 변이만 적용된다.
- 선택 뒤 변이가 적용된 RatHost로 복귀하고 이전 내부 UI·입력·충돌·카메라가 비활성이다.
- 세 변이 효과가 각각 테스트와 원본 MCP Play에서 구분된다.
- 포유류 적응은 지정 통로만 열고 다른 충돌을 무효화하지 않는다.
- 전체 EditMode, 원본 MCP Play·Console·보호 diff와 총괄 판정을 통과한다.
- Windows 빌드와 실행본 전체 루프는 필요 시 별도 생성·검증 사실을 구분해 기록한다.
