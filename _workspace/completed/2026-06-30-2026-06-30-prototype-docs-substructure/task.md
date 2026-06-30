# 작업 배정서

## 기본 정보

- 작업 ID: `2026-06-30-prototype-docs-substructure`
- 작업명: prototype 문서 성격별 하위 구조 정리
- 상태: 완료
- 생성일: 2026-06-30
- 담당 에이전트: 문서/릴리즈 에이전트
- 보조 에이전트: 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `$last-host-design-keeper`

## 목적

`docs/prototype/` 내부의 공식 범위 문서, 승인 이력 문서, 구현 계획 문서를 성격별 하위 폴더로 나눠 사용자가 바로 구분할 수 있게 한다.

## 입력 자료

- 사용자 요청: `prototype 폴더 내에서도 계획, 공식 문서 성격이 다른 파일들이 있으니, 별도 폴더로 나눠 내가 알아볼 수 있도록 해야돼`
- `AGENTS.md`
- `docs/prototype/`
- `docs/README.md`
- `docs/agents/agent-reference-map.md`
- `.agents/`
- `.codex/skills/`

## 해야 할 일

1. `docs/prototype/` 하위 폴더 구조를 정한다.
2. 기존 프로토타입 문서를 성격별 폴더로 이동한다.
3. `docs/prototype/README.md`를 추가해 사용자가 확인할 문서 위치를 설명한다.
4. 현재 운영 문서, 에이전트, 스킬, 작업 이력의 참조 경로를 새 위치로 갱신한다.

## 금지 범위

- Unity 프로젝트 수정
- 게임플레이 코드 작성
- 패키지 추가
- 에셋 생성
- 쥐 숙주 프로토타입 범위 변경

## 완료 기준

- `docs/prototype/official/`, `docs/prototype/approvals/`, `docs/prototype/plans/` 구조가 생긴다.
- 기존 프로토타입 문서 3개가 새 위치로 이동한다.
- 옛 `docs/prototype/<파일명>.md` 참조가 남지 않는다.
- 검증 결과와 완료 보고를 남긴다.

## 완료 결과

- `official/`, `approvals/`, `plans/` 하위 폴더를 만들었다.
- `rat-host-prototype.md`, `rat-host-approval-packet.md`, `rat-host-implementation-plan.md`를 성격별 폴더로 이동했다.
- `docs/prototype/README.md`를 추가했다.
- 현재 참조 경로를 새 구조에 맞게 갱신했다.
