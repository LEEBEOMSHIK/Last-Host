# 작업 배정서

## 기본 정보

- 작업 ID: `2026-06-30-rat-host-prototype-implementation`
- 작업명: 쥐 숙주 1차 프로토타입 Unity 구현
- 상태: 완료
- 생성일: 2026-06-30
- 담당 에이전트: 게임플레이 루프 에이전트
- 보조 에이전트: Unity 아키텍처 에이전트, QA/검증 에이전트, 문서/릴리즈 에이전트
- 사용 스킬: `$rat-host-loop-builder`, `$unity-verification-runner`

## 목적

승인된 공식 구현 계획을 기준으로 Unity 프로젝트 안에 쥐 숙주 1차 프로토타입의 최소 플레이어블 루프를 구현한다.

## 입력 자료

- `AGENTS.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `docs/prototype/approvals/rat-host-approval-packet.md`
- `docs/unity/unity-mcp-setup.md`
- `.agents/gameplay-loop-agent.md`
- `.agents/unity-architecture-agent.md`
- `.agents/qa-verification-agent.md`
- `.codex/skills/rat-host-loop-builder/SKILL.md`
- `.codex/skills/unity-verification-runner/SKILL.md`

## 구현 범위

1. `UnityProject/Assets/_Project/` 구조 생성
2. `RatHostPrototype.unity` 씬 생성
3. 쥐 모드 이동, 면역 경계도, 모드 전환 구현
4. 내부 바이러스 미니게임, 백혈구, 변이 조각 수집 구현
5. 실패/재시도, 변이 선택, 쥐 모드 복귀 구현
6. 최소 HUD와 프로토타입 UI 구현
7. 핵심 상태 로직 EditMode 테스트 작성
8. 가능한 범위에서 Unity 컴파일/테스트/빌드 검증 실행

## 금지 범위

- 벌레 튜토리얼
- 다중 숙주 전이 체인
- 인간 단계
- 병원, 연구소, 백신 시스템
- 엔딩
- 최종 아트 에셋
- 모바일 입력과 모바일 UI

## 완료 기준

- 승인된 범위 안에서만 Unity 프로젝트 파일이 변경된다.
- `RatHostPrototype.unity`와 `_Project` 구조가 존재한다.
- 핵심 루프를 실행할 수 있는 스크립트와 씬 구성이 있다.
- 핵심 상태 로직 테스트를 작성하고 검증 결과를 기록한다.
- 완료 전 검증 결과와 미검증 항목을 `verification.md`에 남긴다.

## 완료 결과

- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity` 생성
- 쥐 숙주 모드, 면역 경계도, 내부 바이러스 모드, 변이 선택, 실패/재시도 루프 구현
- 플레이스홀더 재질과 씬 오브젝트 생성
- 핵심 상태 로직 EditMode 테스트 작성
- Unity MCP 내부 검증과 Windows 빌드 산출물 생성
