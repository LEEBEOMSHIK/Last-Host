# 증상 은폐 방지 정책 업데이트 r1

## 목적

사용자 화면의 증상을 숨긴 변경이 자동 테스트 통과만으로 근본 수정·완료로 판정되지 않도록 전역 루프, 사용자 안내, 2D 아이소메트릭 시각 규칙과 Unity 검증 규칙을 같은 계약으로 연결한다.

## 실행 기준과 요약 경계

- 유일 실행 기준: `docs/agents/loop-engineering-gates.md`의 `원인 교정과 증상 은폐 금지`
- 사용자 설명: `docs/agents/loop-engineering-user-guide.md`
- 2D 시각 적용: `docs/design/visual/pixel-isometric-2d-production-guide.md`
- pixel style keeper 요약: `.codex/skills/pixel-lowpoly-style-keeper/references/pixel-style-rules.md`
- Unity QA 요약: `.codex/skills/unity-verification-runner/references/verification-rules.md`
- `AGENTS.md`는 전역 한 줄과 실행 기준 링크만 소유한다.

세부 금지 목록과 상태 판정은 실행 기준 한 곳에 두고 나머지 문서는 독자·역할별 요약과 링크만 둔다.

## 고정한 정책

다음은 원인 레이어가 별도로 증명되지 않으면 `증상 은폐`다.

- renderer/object disable, alpha `0`
- teleport·clamp, input lock
- error swallow 또는 실패 결과의 성공 위장
- visible footprint보다 큰 invisible collider
- hidden output이나 우회 상태를 기대하도록 테스트 변경

증상 은폐 후보는 완료가 아니라 `temporary` 또는 `blocked`다. workaround는 다음 세 조건이 모두 있을 때만 허용한다.

1. 사용자 명시 승인
2. 플레이 화면·로그·작업 상태의 임시 표시
3. 제거 조건·기한 또는 후속 작업

QA는 원인 레이어와 함께 플레이어 active/enabled/alpha, root·visual transform, 정상 input 보존 negative control, 가시 footprint 대비 collision tolerance, 사용자 가시 oracle을 증명한다.

## 역사적 교훈의 사용 범위

이전 `7ba12df`는 whole-character hide가 자동 검증을 통과했으나 사용자 수용에서 증상 은폐로 실패했다. 이 사실은 정책 도입 교훈으로 기록하지만, 특정 커밋·구현·수치를 전역 정책의 영구 전제로 사용하지 않는다.

## reference map 정적 연결

- `agent-reference-map.md`의 2D 비주얼 항목은 production guide와 pixel style keeper를 필수/선택 참조로 연결한다.
- Unity 구현·검증 항목은 `loop-engineering-gates.md`, 사용자 가이드, unity verification runner와 verification rules 경로를 연결한다.
- 새 참조 문서를 만들지 않았으므로 reference map의 새 항목은 필요하지 않다.

## 변경 경계

- production 코드·씬·테스트·ProjectSettings·에셋·수치 변경 없음
- 현재 S0 r4 footprint contract와 gameplay candidate 수치 변경 없음
- Unity·MCP·테스트·빌드 실행 없음
- 정책 후보는 독립 QA와 총괄 판정 전 완료로 선언하지 않는다.

## 정적 검증 계획

- 금지 유형, workaround 세 조건, QA negative control, `temporary/blocked`, historical lesson과 비영구 전제 문구 전수 검색
- 모든 참조 경로 존재 확인
- `AGENTS.md` 200줄 미만 확인
- `git diff --check`
