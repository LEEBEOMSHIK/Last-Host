# 작업 배정서

## 기본 정보

- 작업 ID: `2026-07-13-pixel-lowpoly-production-guide`
- 작업명: 도트풍 저폴리 3D 제작 가이드
- 상태: 진행 중
- 생성일: 2026-07-13
- 담당 에이전트: 문서/릴리즈 에이전트
- 보조 에이전트: QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `pixel-lowpoly-style-keeper`

## 목적

승인된 도트풍 저폴리 3D 방향을 실제 제작자가 일관되게 적용할 수 있도록 모델, 픽셀 텍스처, URP 저해상도 렌더링, 카메라, 조명, UI, 플레이스홀더와 검토 순서를 담은 사용자 확인용 제작 가이드를 만든다.

## 입력 자료

- `AGENTS.md`
- `docs/design/README.md`
- `docs/design/game-design-summary.md`
- `docs/project/project-prep.md`
- `docs/prototype/official/rat-host-prototype.md`
- `.codex/skills/pixel-lowpoly-style-keeper/SKILL.md`
- `.codex/skills/pixel-lowpoly-style-keeper/references/pixel-style-rules.md`

## 담당과 산출물

| 에이전트 | 담당 | 산출물 |
| --- | --- | --- |
| 문서/릴리즈 에이전트 | 제작 가이드·색인 반영 | `docs/design/visual/README.md`, `pixel-lowpoly-3d-production-guide.md`, 관련 색인 |
| QA/검증 에이전트 | 승인 방향·문서 구조·범위 대조 | `verification.md` |
| 프로젝트 총괄 관리자 에이전트 | 범위·승인 게이트·QA 기록 검토 | `director-review.md` |

## 해야 할 일

1. `docs/design/visual/`의 문서 성격과 읽는 순서를 정의하는 README를 만든다.
2. 제작 가이드에 다음을 정리한다: 3D/저폴리 형태 기준, 픽셀 텍스처와 팔레트, Unity Import 기준, URP 저해상도·픽셀화 적용 순서와 권장 시작값, 쿼터뷰/탑다운 카메라, 조명·포그, UI, 플레이스홀더 구분, 씬 검토 체크리스트.
3. 각 권장값은 문서 기준일 뿐 실제 Unity 설정 변경은 아니며, 구현·에셋 생성 전에 별도 승인과 검증이 필요함을 명시한다.
4. `docs/design/README.md`와 필요한 참조 색인에서 새 문서를 찾을 수 있게 한다.

## 금지 범위

- Unity 코드, 씬, ProjectSettings, URP Renderer, 머티리얼, 텍스처, 모델, 패키지, MCP 설정을 변경하지 않는다.
- 실제 아트 에셋을 생성하지 않는다.
- 순수 2D 스프라이트 게임으로 방향을 바꾸지 않는다.
- 승인된 쥐 숙주 프로토타입 범위를 넓히지 않는다.

## 완료 기준

- 새 visual 문서 구조와 가이드가 docs 색인에 반영된다.
- 가이드는 도트풍 3D를 구현 가능한 순서·기준·검토 항목으로 설명하고, 플레이스홀더와 최종 아트를 구분한다.
- 실제 설정·에셋 변경이 문서 기준과 별도 승인 대상임을 명시한다.
- QA 완료 가능과 총괄 관리자 내부 승인 가능 판정이 기록된다.
