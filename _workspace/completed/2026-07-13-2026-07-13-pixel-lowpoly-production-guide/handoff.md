# 핸드오프 기록

## 작업 ID

`2026-07-13-pixel-lowpoly-production-guide`

## 최신 사용자 요청

3D 도트게임을 만드는 법과 도트 적용 방법을 정리한 제작 가이드를 만든다.

## 현재 상태

- 상태: 문서/릴리즈 산출물 완료, QA 대조 대기
- 여기서 멈춤: Unity·에셋·설정은 변경하지 않았으며, 문서의 범위와 링크를 독립 검토해야 한다.
- 다음 세션의 첫 목표: QA가 승인된 비주얼 방향, 쥐 숙주 범위, 문서 색인을 대조하고 판정을 기록한다.

## 넘기는 에이전트

문서/릴리즈 에이전트

## 받는 에이전트

QA/검증 에이전트

## 먼저 읽을 파일

1. `docs/design/visual/pixel-lowpoly-3d-production-guide.md`
2. `docs/design/visual/README.md`
3. `docs/design/game-design-summary.md`
4. `docs/prototype/official/rat-host-prototype.md`
5. `docs/agents/agent-reference-map.md`

## 변경한 파일

- `docs/design/visual/README.md`
- `docs/design/visual/pixel-lowpoly-3d-production-guide.md`
- `docs/design/README.md`
- `docs/README.md`
- `docs/agents/agent-reference-map.md`
- 작업 패킷의 `agent-activity.md`, `work-log.md`, 이 `handoff.md`

## 건드리면 안 되는 기존 변경

- `.codex/config.toml`의 범위 밖 로컬 변경

## 마지막 성공 검증

- 문서 담당의 범위 검토: 실제 Unity 코드, 씬, ProjectSettings, URP Renderer, 에셋, 패키지, MCP 설정을 변경하지 않았다.

## 실패 또는 차단된 검증

- Unity Play·빌드 검증은 문서 전용 작업이라 실행하지 않았다.

## 루프 게이트 상태

- 작업 배정 게이트: 충족
- 담당 산출물 게이트: 문서/릴리즈 산출물 완료, QA 검토 대기
- QA/검증 게이트: 대기
- 총괄 관리자 게이트: 대기
- 커밋 전 차단 조건: QA와 총괄 관리자 판정, 상태판 동기화 대조가 필요

## 넘기는 이유

문서 산출물이 승인된 비주얼 방향과 범위를 벗어나지 않는지 독립 검토가 필요하다.

## 넘기는 에이전트가 완료한 일

- `docs/design/visual/`에 비주얼 문서 구조와 제작 가이드를 만들었다.
- 모델, 텍스처, URP 렌더링, 카메라, 조명, UI, 플레이스홀더, 씬 체크리스트를 문서 기준으로 정리했다.
- 디자인·루트·에이전트 참조 색인에서 새 가이드를 찾을 수 있게 했다.

## 받는 에이전트에게 기대하는 산출물

- `verification.md`에 문서 구조, 링크, 승인 범위, 실제 Unity 변경 부재의 독립 판정을 기록한다.

## 이어서 해야 할 일

1. 링크와 색인 경로를 확인한다.
2. 쥐 숙주 프로토타입 범위 확대나 실제 설정 확정 표현이 없는지 대조한다.
3. QA 판정을 `verification.md`와 `agent-activity.md`에 기록한다.

## 참고 자료

- `.codex/skills/pixel-lowpoly-style-keeper/references/pixel-style-rules.md`
- `docs/project/project-prep.md`

## 에이전트 수행 이력 갱신

- `agent-activity.md`에 인계 기록 추가 여부: 문서 담당 완료 기록 추가
- 인계 결과 기록 책임자: QA/검증 에이전트

## 주의할 점

- 권장 시작값은 확정된 Unity 설정이 아니다.
- 실제 모델·텍스처·URP·카메라·후처리 변경은 별도 사용자 승인과 Unity 검증이 필요하다.

## 사용자 승인 필요

- 가이드 작성 자체는 사용자 요청으로 승인되었다.
- 가이드에 따른 실제 에셋·Unity·URP 변경은 아직 별도 승인 대상이다.

## 토큰 경계 메모

- 인수인계가 필요한 단계: QA 판정 후 총괄 관리자 검토
- 토큰 압박 체감: 낮음
- 새 구현 금지 여부: Unity·에셋·설정 구현 금지 유지
