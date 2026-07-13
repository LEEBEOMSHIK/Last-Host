# 인계 메모

- 규칙: ChatGPT 이미지 생성은 콘셉트 시트, 픽셀 텍스처 초안, 타일 패턴, UI 아이콘 초안에 한정한다. 출력은 선별·정리 후 저폴리 모델의 텍스처 소스로만 사용한다.
- 별도 단계: 메시·UV·리깅·콜라이더·머티리얼·Import·URP 적용은 3D/Unity 작업이며 자동 생성 이미지가 이를 대체하지 않는다.
- 승인: 실제 이미지 생성, 신규 아트 에셋 추가, Unity 적용은 각각 사용자 승인과 작업 패킷·QA가 필요하다.
- 계획: 자산 목록/스타일 브리프 → 승인 → 생성·프롬프트 기록 → 선별·정리 → 저폴리 모델/UV → Unity 적용 → 화면/플레이 검증 → 수용 기록.

## 문서 반영 완료 후 인계

- 변경: `docs/design/visual/pixel-lowpoly-3d-production-guide.md`에 AI 보조 래스터 제작 규칙을 추가했고, `docs/prototype/plans/rat-host-ai-assisted-art-workflow.md`에 7단계 실행 계획을 만들었다.
- 범위 확인: 실제 이미지·텍스처·모델·UI 에셋 생성, Unity 씬·URP·Import·ProjectSettings·코드 변경은 없다.
- QA 확인: 허용 용도와 비대체 작업의 구분, 묶음별 사용자 승인, 프롬프트·변형·출처·파일 위치 기록, 제3자 권리·개인정보 금지, 색인·상태판의 진행 상태를 대조한다.
- QA 판정: 문서 규칙·범위·상태판·Git 변경 범위와 `CURRENT.md` 세션 포인터를 재대조해 완료 가능이다. 실제 이미지·Unity 변경이 없으므로 MCP 플레이 체크는 적용 대상이 아니다.
- 총괄 관리자 판정: `director-review.md`에 `내부 승인 가능`이 기록됐다.
- 완료 보관: 작업 패킷은 `_workspace/completed/2026-07-13-2026-07-13-ai-assisted-pixel-art-workflow/`에 보관됐다. `completion-report.md`는 QA `완료 가능`과 총괄 관리자 `내부 승인 가능`을 반영한다.
- 상태 동기화: 상태판의 진행 중 행을 제거하고 최근 작업 요약을 실제 완료 경로로 바꿨으며, `CURRENT.md`도 작업 없음으로 갱신했다.
- 다음 조치: 사용자 요청에 따라 관련 문서·완료 패킷·상태판 동기화를 범위 밖 `.codex/config.toml`과 분리해 커밋·푸시한다.
