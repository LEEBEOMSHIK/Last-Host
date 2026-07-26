# 검증 기록

## 2026-07-27 — 독립 QA/검증

- 담당: `2d_unity_planning_sync` 재배정 QA
- 1차 판정: `FAIL — 수정 후 재대조 필요`
- 발견:
  - `docs/prototype/README.md`의 AI 아트 색인이 이전 3D 원본·8방향 렌더 절차로 남아 있었다.
  - `docs/agents/agent-skill-plan.md`의 `unity-prototype-planner`가 이전 Unity 3D 설명과 존재하지 않는 reference 경로를 사용했다.
- 수정:
  - AI 아트 색인을 ChatGPT 이미지 생성·출처 기록·사람 선별·2D 게임 규격 재제작·Unity 적용·QA 흐름으로 변경했다.
  - Unity 계획 스킬 설명을 2D Tilemap·Collider·Y 정렬·고정 직교 카메라 기준과 실제 `unity-architecture.md` reference로 변경했다.

## 2026-07-27 — 최종 QA 재대조

- 판정: `PASS — 완료 가능`
- 현재형 문서의 기본 방향은 2D 아이소메트릭·쿼터뷰 도트로 일치한다.
- 3D·2.5D·Blender 문구는 승인 이력, 레거시 가이드, 과거 완료 작업 또는 기존 씬 회귀 기준으로만 남아 있다.
- 목업 PNG `1672×941`, `2,465,975 bytes` 존재와 시각 내용을 확인했다.
- 목업은 target-screen reference이며 타일셋·스프라이트 시트·애니메이션·충돌 데이터·최종 에셋이 아님을 확인했다.
- imagegen 후보 생성→도구·프롬프트·입력·출력 기록→비주얼 검토·사용자 선별→실제 게임 에셋 재제작→QA 흐름을 확인했다.
- ChatGPT 이미지 아트 에이전트는 생성·추적, 비주얼/테크아트 에이전트는 게임 규격 적합성 검토로 역할이 분리된다.
- 에이전트 12개와 필수 파일 존재, `AGENTS.md` 137줄, `git diff --check` exit 0을 확인했다.
- 이번 작업의 UnityProject 변경은 없다. 기존 `ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` 변경과 `_workspace/previews/`는 사용자 로컬 제외이며 `Builds/` 변경도 없다.
- Unity 테스트·빌드·MCP Play는 Unity 구현을 바꾸지 않은 문서·reference·역할 전환이므로 이번 완료 주장 대상이 아니다.

## 남은 게이트

- 프로젝트 총괄 관리자 에이전트 1차 판정: `수정 필요`
  - 현황판 Git 기준이 실제 `HEAD = origin/main = c2298db`와 달랐다.
  - `CURRENT.md`와 `handoff.md`가 이미 끝난 단계를 남은 작업으로 표시했다.
  - `agent-skill-plan.md`의 최종 수정일이 이전 날짜로 남아 있었다.
- 상태 문서 3건을 교정한 뒤 독립 QA가 다시 대조했다.
- 최종 QA 재대조: `PASS — 완료 가능`
  - 실제 `HEAD`와 `origin/main` 전체 SHA가 같고 현황판의 `c2298db` 기준과 일치한다.
  - CURRENT와 handoff는 재검토→보관→별도 2D 기술 샘플 승인 대기 순서로 정합한다.
  - `agent-skill-plan.md` 최종 수정일은 `2026-07-27`이다.
  - 이전 2D 방향·레거시·목업·imagegen·Unity 비변경 PASS 범위를 유지한다.
  - `git diff --check` exit 0이다.
- 프로젝트 총괄 관리자 재판정: `내부 승인 가능`
- 근거: Git·상태 문서·수정일 교정, QA 재대조 기록, 2D 방향·역할·목업·레거시·Unity 미착수 경계가 모두 정합하다.
- 수정 필요·문제 사안·사용자 결정 필요: 없음.
- 최종 판정: 이번 문서·reference·에이전트 체계 전환 작업은 완료 보관 가능하다. 2D 플레이어블 구현·Unity 테스트·빌드 통과를 뜻하지 않는다.

## 2026-07-27 — 완료 경로 보관 QA

- 1차 보관 대조에서 이전 r6 기록 4곳의 삭제된 2D 전환 active 경로를 발견해 현재 completed 경로로 교정했다.
- 최종 판정: `PASS — 완료 주장 가능`
- 두 작업의 active 경로가 없고 completed 경로와 핵심 기록 파일이 존재한다.
- 목업 입력 reference의 completed PNG 경로가 존재하며 stale active 경로는 0건이다.
- 현황판과 CURRENT의 완료·차단·다음 후보 상태가 실제 경로와 일치한다.
- `git diff --check` exit 0, Unity/Builds 변경 없음, 사용자 로컬 제외 유지다.
