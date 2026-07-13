# 문서 담당 핸드오프

## 작업 ID

`2026-07-13-pixel-lowpoly-production-guide`

## 문서 반영

- 새 비주얼 문서 폴더: `docs/design/visual/`
- 새 가이드: `docs/design/visual/pixel-lowpoly-3d-production-guide.md`
- 새 폴더 색인: `docs/design/visual/README.md`
- 갱신 색인: `docs/design/README.md`, `docs/README.md`, `docs/agents/agent-reference-map.md`

## 작성 판단

- 게임은 순수 2D가 아닌 도트풍 저폴리 3D라는 확정 방향을 첫 항목으로 고정했다.
- 모델·텍스처·URP 저해상도 렌더링·카메라·조명·UI의 수치는 실제 설정이 아닌 권장 시작 기준으로 표시했다.
- 낮은 내부 해상도와 정수 배율 확대를 우선 비교하고, 픽셀화 후처리는 별도 승인·검증이 필요한 비교안으로 구분했다.
- 쥐 숙주 프로토타입에 없는 콘텐츠·시스템·최종 아트를 확정하지 않았고, 플레이스홀더와 최종 아트의 역할을 분리했다.

## 검증

- `git diff --check` 통과.
- 디자인·문서 루트·에이전트 참조 색인에서 새 가이드 경로를 확인.
- 실제 Unity 코드, 씬, ProjectSettings, URP Renderer, 에셋, 패키지, MCP 설정 변경 없음.

## 상태판 동기화와 완료 보관

- 총괄 검토의 수정 필요 판정에 따라 상태판을 갱신했고, QA 강화 게이트와 총괄 재검토를 통과했다.
- 완료 패킷은 `_workspace/completed/2026-07-13-2026-07-13-pixel-lowpoly-production-guide/`로 이동했다.
- 상태판에서 진행 중 작업 행을 제거하고, 최근 작업 요약의 최신 5개 중 맨 위에 이 작업의 정확한 completed 경로를 추가했다.
- 다음 작업 후보 없음과 사용자 수동 플레이 체감 확인 보류 항목 1개를 유지했다.
- 미커밋 문서·completed 작업 패킷·상태판 동기화와 범위 밖 `.codex/config.toml` 변경을 분리해 기록했다.

## QA 요청

1. `game-design-summary.md`, `rat-host-prototype.md`와 비교해 순수 2D 전환이나 범위 확정 표현이 없는지 확인한다.
2. 가이드의 권장 시작값이 실제 설정값처럼 서술되지 않았는지 확인한다.
3. 새 폴더 README와 상위 색인·참조 색인의 경로를 확인하고 `verification.md`에 기록한다.
4. completed 경로, 상태판의 제작 가이드 경로 단일 참조, 후보 없음, 사용자 수동 플레이 보류, 실제 Git 상태를 독립 대조한다.

## 최종 상태와 남은 게이트

- 문서/릴리즈 산출물과 완료 보관·상태판 정리는 완료했다.
- QA/검증 에이전트는 completed 경로와 상태판 최종 상태를 재대조해야 한다.
- 프로젝트 총괄 관리자 에이전트는 QA 재대조 기록을 확인해야 한다.
- 실제 Unity·에셋·URP 적용은 이 문서 작업의 범위 밖이며, 별도 승인·검증 게이트를 유지한다.
