# 작업별 참조 문서 색인

최종 수정일: 2026-08-08

## 목적

이 문서는 `AGENTS.md`를 200줄 미만의 상위 규칙 파일로 유지하기 위한 참조 색인이다. 작업 요청이 들어오면 이 문서에서 해당 작업 유형을 찾고, 필요한 세부 문서만 읽는다.

## 운영 원칙

- `AGENTS.md`에는 전역 원칙, 승인 게이트, 참조 위치만 둔다.
- 세부 절차가 길어지면 이 문서 또는 작업별 참조 문서로 분리한다.
- 새 작업 유형이 생기면 이 문서에 참조 위치를 추가한다.
- 참조 문서를 추가할 때는 어떤 요청에서 읽어야 하는지 함께 적는다.
- 프로젝트 변경의 위험 등급, 최소 역할, 검증 순서와 완료 차단 조건은 `docs/agents/loop-engineering-gates.md`만 실행 기준으로 사용한다.
- 에이전트 배정, 검증 책임, 비용·중복 검증을 사용자 관점에서 설명하거나 문의받으면 `docs/agents/loop-engineering-user-guide.md`도 필수로 읽는다. 이 문서는 비실행 요약이다.
- 작업별 비용 과다·불필요 비용, 실행 수, 폐기 증거와 판정 문의에는 `docs/project-handoff/task-cost-dashboard.md`를 공식 중앙 근거로 함께 읽는다.

## 공통 참조 순서

모든 작업은 기본적으로 다음 순서를 따른다.

1. `AGENTS.md`
2. 이 문서
3. 작업 유형별 필수 참조 문서
4. 필요한 경우 작업 유형별 선택 참조 문서

## 작업 유형별 참조

### 기획 정리 / 원본 기획 확인

사용 요청 예시:

- 기획서 내용을 다시 정리해 달라는 요청
- 원본 `.docx`와 현재 문서의 차이 확인
- 게임 방향, 메시지, 프로토타입 범위 확인

필수 참조:

- `docs/design/game-design-summary.md`
- `docs/prototype/official/rat-host-prototype.md`
- `.codex/skills/last-host-design-keeper/SKILL.md`

선택 참조:

- `C:\project\game\last_host\last_host_game_plan.docx`
- `.codex/skills/last-host-design-keeper/references/design-guardrails.md`

### 게임 상세 기획 / 시스템과 상호작용 정리

사용 요청 예시:

- 특정 오브젝트가 왜 특정 게임 수치를 바꾸는지 정리
- 면역 경계도, 변이, 숙주, 위험 행동의 상세 기획 작성
- 긴 배관, 독성 물웅덩이, 전이 대상 같은 상호작용 의도 정리

필수 참조:

- `docs/design/README.md`
- `docs/design/game-design-summary.md`
- `.codex/skills/last-host-design-keeper/SKILL.md`

선택 참조:

- `docs/design/interactions/README.md`
- `docs/design/interactions/noisy-pipe-risk-interaction.md`
- `docs/design/hosts/host-instinct-control.md`
- `docs/design/progression/host-experience-traits.md`
- `docs/design/encounters/internal-immune-response-minigame-types.md`
- `docs/design/systems/README.md`
- `docs/design/systems/immune-alert.md`
- `docs/design/ui-feedback/README.md`
- `docs/design/ui-feedback/immune-alert-feedback.md`
- `docs/prototype/official/rat-host-prototype.md`

### 메인 시나리오 / 캠페인 흐름 설계

사용 요청 예시:

- 게임 시작부터 엔딩 후보까지 전체 시나리오를 차근차근 설계
- 숙주·먹이사슬·맵 이동과 사건·성장을 연결
- 돌연변이 기원 미스터리, 단서와 반전 공개 순서를 관리
- 시네마틱·튜토리얼·탐험·미니게임이 이야기에서 맡는 기능을 정리

필수 참조:

- `.agents/main-scenario-director-agent.md`
- `docs/design/game-design-summary.md`
- `docs/design/narrative/README.md`
- `docs/design/narrative/main-scenario-outline.md`
- `docs/design/hosts/host-map-transfer-route.md`
- `.codex/skills/last-host-design-keeper/SKILL.md`
- 해당 `_workspace/active/<작업ID>/task.md`

선택 참조:

- `docs/design/narrative/opening/README.md`
- `docs/design/narrative/pixel-art-motion-comic-cinematic-guide.md`
- `docs/prototype/official/rat-host-prototype.md`
- `.agents/pixel-cinematic-director-agent.md`
- `.agents/gameplay-loop-agent.md`

운영 경계:

- 메인 시나리오 디렉터는 이야기의 의미·순서·공개 정보·게임플레이 연결을 소유한다.
- 세부 숏 연출은 픽셀아트 시네마틱 연출, 구현은 게임플레이 또는 Unity 통합, 검증과 내부 승인은 QA와 프로젝트 총괄로 분리한다.
- 장기 캠페인 문서화는 쥐 숙주 프로토타입 밖의 구현 승인이 아니다.

### Unity 프로젝트 준비 / 아키텍처

사용 요청 예시:

- Unity 프로젝트 생성 준비
- Unity 버전, URP, 폴더 구조, 씬 구조 결정
- 시스템 경계 설계

필수 참조:

- `docs/project/project-prep.md`
- `.codex/skills/unity-prototype-planner/SKILL.md`

선택 참조:

- `docs/unity/unity-baseline-report.md`
- `.codex/skills/unity-prototype-planner/references/unity-architecture.md`

### Unity MCP / Unity Editor 자동화

사용 요청 예시:

- Unity MCP 설정 요청
- Codex에서 Unity Editor를 MCP로 연결하려는 요청
- MCP를 통해 씬, 에셋, 패키지, 코드 작업을 준비하려는 요청

필수 참조:

- `docs/unity/unity-mcp-setup.md`
- `.codex/config.toml`
- `.agents/unity-architecture-agent.md`

선택 참조:

- `.codex/mcp/start-mcp-unity.ps1`
- `.codex/skills/unity-prototype-planner/SKILL.md`
- `_workspace/templates/verification.md`

### 쥐 숙주 프로토타입 / 핵심 루프

사용 요청 예시:

- 쥐 조종 설계
- 벽·통·상자에 대각선으로 충돌할 때 제자리 보행하거나 표면 slide가 되지 않는 문제 정리
- 면역 경계도 설계
- 내부 바이러스 미니게임 설계
- 변이 선택 루프 설계

필수 참조:

- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `.codex/skills/rat-host-loop-builder/SKILL.md`

선택 참조:

- `docs/prototype/approvals/rat-host-approval-packet.md`
- `docs/design/progression/host-experience-traits.md`
- `.codex/skills/rat-host-loop-builder/references/rat-loop-rules.md`

특수 적용:

- 대각선 충돌·제자리 보행·표면 slide 요청은 `docs/prototype/plans/rat-host-implementation-plan.md`의 `2D 이동·충돌 표면 슬라이드 계약`을 원증상·금지 방식·수용 기준·재발 처리 절차로 사용한다.

### 2D 아이소메트릭 도트 / 비주얼

사용 요청 예시:

- 비주얼 스타일 확인
- 2D 환경 타일, 방향별 캐릭터 스프라이트 기준 확인
- 방향·프레임·피벗·앞뒤 정렬·가림과 픽셀 출력 기준 확인
- 고정 아이소메트릭 카메라와 플레이스홀더 기준 정리

필수 참조:

- `docs/design/game-design-summary.md`
- `docs/design/visual/graphics-direction-management.md`
- `docs/design/visual/pixel-isometric-2d-production-guide.md`
- `docs/design/visual/references/README.md`
- `.codex/skills/pixel-lowpoly-style-keeper/SKILL.md`

선택 참조:

- `.codex/skills/pixel-lowpoly-style-keeper/references/pixel-style-rules.md`
- `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`
- `docs/prototype/plans/rat-host-ai-assisted-art-workflow.md`
- `docs/design/visual/pixel-lowpoly-3d-production-guide.md` (기존 2.5D 산출물과 테스트를 해석할 때만)

### ChatGPT 이미지 생성 / 목업·콘셉트·래스터 후보

사용 요청 예시:

- ChatGPT 이미지 모델로 게임플레이 목업이나 캐릭터 콘셉트 생성
- 프로젝트 reference를 사용한 환경·타일·HUD 이미지 후보 생성
- 생성 프롬프트·입력 출처·선별 기록 정리

필수 참조:

- `.agents/chatgpt-image-art-agent.md`
- `docs/design/visual/references/README.md`
- `docs/design/visual/graphics-direction-management.md`
- `docs/prototype/plans/rat-host-ai-assisted-art-workflow.md`
- 해당 `_workspace/active/<작업ID>/task.md`

선택 참조:

- `docs/design/visual/pixel-isometric-2d-production-guide.md`
- `.agents/visual-tech-art-agent.md`
- `.agents/qa-verification-agent.md`

운영 경계:

- OpenAI 내장 `imagegen`을 우선 사용한다.
- 생성 결과는 후보이며 최종 타일·스프라이트로 자동 승인하지 않는다.
- 프롬프트, 도구·날짜, 입력 reference 출처, 출력 경로와 선별 결과를 기록한다.
- 방향·프레임 일관성, 게임 규격 재제작, Unity 적용과 QA는 후속 역할로 분리한다.

### 픽셀아트 모션 코믹형 시네마틱 / 컷신 연출 설계

사용 요청 예시:

- 도입, 중요 숙주 전이, 세계 변화와 엔딩의 컷신 구성
- 픽셀아트 모션 코믹형 시네마틱의 숏·스토리보드·레이어 설계
- 비최종 애니매틱 계획, 자막·오디오 큐와 Unity 인계 명세 작성

필수 참조:

- `docs/design/narrative/main-scenario-outline.md`
- `docs/design/narrative/pixel-art-motion-comic-cinematic-guide.md`
- `docs/design/visual/graphics-direction-management.md`
- `.agents/pixel-cinematic-director-agent.md`
- 해당 `_workspace/active/<작업ID>/task.md`

선택 참조:

- `docs/design/visual/pixel-isometric-2d-production-guide.md`
- `docs/design/visual/references/README.md`
- `.agents/chatgpt-image-art-agent.md`
- `.agents/visual-tech-art-agent.md`
- `.agents/unity-scene-integration-agent.md`
- `.agents/qa-verification-agent.md`

운영 경계:

- 시네마틱 연출 역할은 숏·스토리보드·레이어 명세·비최종 애니매틱 계획·Unity 인계 명세만 담당한다.
- 실제 이미지·영상·애니매틱·오디오 제작과 Unity 구현은 각각 별도 승인·담당 작업으로 분리한다.
- 생성 후보는 사용자 선택과 픽셀 검토·QA 없이 최종 에셋이나 완성 컷신으로 선언하지 않는다.
- 전체 캠페인 장면의 문서화는 벌레·인간·병원·연구소·백신·엔딩 구현 승인이 아니다.

### 레거시 Blender 원본 / 리깅 / 프리렌더 조사

사용 요청 예시:

- 기존 Blender 저폴리 캐릭터 원본이나 렌더 이력 확인
- 레거시 쥐 보행·대기 프리렌더의 재현 또는 보존
- 기존 8방향 프리렌더 카메라·피벗·루프·접지 문제 조사

필수 참조:

- `.agents/blender-animation-tech-artist-agent.md`
- `docs/design/visual/pixel-lowpoly-3d-production-guide.md`
- `.codex/skills/pixel-lowpoly-style-keeper/SKILL.md`
- 해당 `_workspace/active/<작업ID>/task.md`

선택 참조:

- `.agents/visual-tech-art-agent.md`
- `.agents/qa-verification-agent.md`
- 해당 작업의 기존 `.blend`, 프레임 맵, 렌더 설정, 시각 검토 기록

운영 경계:

- 신규 2D 제작에는 Blender 애니메이션 테크아트 에이전트를 기본 배정하지 않는다.
- 레거시 원본·애니메이션·시험 렌더의 조사·재현·보존만 Blender 애니메이션 테크아트 에이전트에 배정한다.
- Unity 반입은 사용자 별도 승인 뒤 Unity 씬/통합 구현 에이전트에 분리 배정한다.

### 에이전트 배정 / 스킬 운영

사용 요청 예시:

- 어떤 에이전트에게 일을 맡길지 정리
- 에이전트 간 핸드오프 절차 확인
- 스킬 추가 또는 역할 변경 검토

필수 참조:

- `docs/agents/agent-skill-plan.md`
- `docs/agents/loop-engineering-gates.md`
- `docs/agents/loop-engineering-user-guide.md` — 사용자·온보딩용 책임·비용·중복 검증 요약
- `docs/project-handoff/task-cost-dashboard.md` — 작업별 계획·실제 비용과 판정 중앙 현황
- `.agents/agent-roster.md`
- `.agents/project-director-agent.md`

선택 참조:

- `.agents/project-coordinator-agent.md`
- `.agents/design-keeper-agent.md`
- `.agents/unity-architecture-agent.md`
- `.agents/gameplay-loop-agent.md`
- `.agents/gameplay-implementation-agent.md`
- `.agents/unity-scene-integration-agent.md`
- `.agents/visual-tech-art-agent.md`
- `.agents/chatgpt-image-art-agent.md`
- `.agents/pixel-cinematic-director-agent.md`
- `.agents/main-scenario-director-agent.md`
- `.agents/qa-verification-agent.md`
- `.agents/documentation-release-agent.md`

### 에이전트 작업영역 / 완료 추적

사용 요청 예시:

- 에이전트 작업 폴더 생성
- 진행 중 작업 기록
- 완료 상태 기록과 최소 작업 폴더 보관
- 핸드오프 기록 작성

필수 참조:

- `_workspace/README.md`
- `_workspace/active/README.md`
- `_workspace/completed/README.md`

선택 참조:

- `_workspace/templates/record.md` — 2026-08-06 이후 신규 R1 통합 기록
- `_workspace/templates/task.md`, `_workspace/templates/verification.md` — 신규 R2/R3
- `_workspace/templates/work-log.md`, `_workspace/templates/agent-activity.md`, `_workspace/templates/completion-report.md` — 신규 R3 조건부. 기본 두 파일에 안전하게 통합할 수 없는 실제 추적 필요가 있을 때만 생성
- `_workspace/templates/handoff.md` — 세션 중단·외부 차단·실제 인계 때만
- `_workspace/templates/task-r1-summary.md` — 기존 이력 호환용

### 프로젝트 핸드오프 / 다음 작업 후보 현황

사용 요청 예시:

- 현재 작업 후보를 한곳에서 확인
- 다음 작업 발굴 결과의 사용자 확인용 상태판 갱신
- 현재 active·next 후보나 Git 상태가 실제로 바뀐 시점의 상태판 동기화
- 최근 작업 요약과 미결 검증 항목 정리
- Codex와 사용자가 함께 보는 handoff 문서 확인

필수 참조:

- `docs/project-handoff/README.md`
- `docs/project-handoff/current-task-board.md`
- `docs/project-handoff/task-cost-dashboard.md`
- `docs/agents/loop-engineering-gates.md`

필수 확인:

- 상태판은 active·next 후보 또는 실제 Git 상태가 바뀔 때만 현재 사실에 맞춘다. 비용 현황판은 R2/R3 또는 실제 Unity/MCP/build/full suite/matrix/capture가 있는 작업만 시작·blocker/correction·사용자 보고/커밋 전 실제값을 동기화한다. 위험 등급상 독립 QA가 필요한 후보는 QA가 기능·증거와 비용 분류를 대조하되, QA·총괄 판정 뒤 path/status/diff만 바꾸는 상태-only 최종 동기화는 새 QA·총괄 라운드를 만들지 않는다.

선택 참조:

- `_workspace/active/CURRENT.md`
- `_workspace/active/`
- `_workspace/completed/`

### Unity 구현 / 씬 통합

사용 요청 예시:

- C# 게임플레이 코드 구현
- EditMode 테스트 작성 또는 수정
- Unity 씬, 프리팹, 입력, 카메라, UI 연결
- 승인된 범위의 Build Settings 또는 ProjectSettings 변경
- 2D 이동 중 대각선 충돌·제자리 보행·표면 slide 회귀 수정

필수 참조:

- `docs/prototype/plans/rat-host-implementation-plan.md`
- `docs/agents/loop-engineering-gates.md`
- `.agents/gameplay-implementation-agent.md`
- `.agents/unity-scene-integration-agent.md`
- `.agents/project-coordinator-agent.md`

선택 참조:

- `.agents/gameplay-loop-agent.md`
- `.agents/unity-architecture-agent.md`
- `.agents/visual-tech-art-agent.md`
- `.agents/qa-verification-agent.md`
- `_workspace/templates/record.md` — 기존 계약 안의 1~3개 파일 신규 R1 국소 수정
- `_workspace/templates/task.md`
- `_workspace/templates/verification.md`

특수 적용:

- 2D 이동·충돌 surface slide 수정은 `docs/prototype/plans/rat-host-implementation-plan.md`의 동명 계약을 읽고, 평면 slide·정면 정지·실제 코너 정지·좌우 대칭·공용 motor consumer·사용자 실제 WASD 수용을 작업 charter에 연결한다.

### 검증 / 완료 판단

사용 요청 예시:

- 완료 여부 판단
- 테스트, 빌드, 검증 체크리스트 작성
- 미검증 항목 정리

필수 참조:

- `docs/agents/loop-engineering-gates.md`
- `docs/agents/loop-engineering-user-guide.md` — 사용자에게 검증 역할·비용·재실행 이유를 설명할 때 필수
- `docs/project-handoff/task-cost-dashboard.md` — 실제 실행 수·중복·폐기와 비용 판정 대조
- `.codex/skills/unity-verification-runner/SKILL.md`
- `.agents/qa-verification-agent.md`
- `.agents/project-director-agent.md`
- `_workspace/templates/verification.md`

필수 확인:

- 구현 전 S0 사용자 원증상·합성 oracle, candidate fingerprint·run_id, Unity single-owner lease, 변경 후 PASS 무효화, canonical evidence를 `loop-engineering-gates.md` 기준으로 대조한다.
- 기술 검증 통과와 사용자 수용 대기를 분리한다.
- 고비용 Unity/MCP/build 전 `tools/verification/verification-capabilities.json`과 `Invoke-HighCostVerification.ps1` preflight를 사용하고 low-level runner를 직접 Run하지 않는다.

선택 참조:

- `docs/unity/unity-baseline-report.md`
- `.codex/skills/unity-verification-runner/references/verification-rules.md`

### 문서 정리 / 커밋 요약 / 릴리즈 정리

사용 요청 예시:

- 변경사항 문서화
- 완료 보고 작성
- 커밋 메시지 후보 작성
- 남은 작업 정리

필수 참조:

- `docs/README.md`
- `.agents/documentation-release-agent.md`
- `_workspace/templates/record.md` — R1 최종 상태 포함
- `_workspace/templates/completion-report.md` — R3 조건부. 완료 상태를 `verification.md`에 통합할 수 없을 때만 사용

선택 참조:

- `docs/agents/agent-skill-plan.md`
- `_workspace/templates/verification.md`

## AGENTS.md가 200줄에 가까워질 때

다음 절차를 따른다.

1. 새 내용이 전역 원칙인지 작업별 세부 절차인지 구분한다.
2. 전역 원칙이면 `AGENTS.md`에 짧게 추가한다.
3. 세부 절차이면 작업별 문서나 스킬 reference에 추가한다.
4. 이 문서에 해당 참조 위치를 추가한다.
5. `AGENTS.md` 줄 수가 200줄 미만인지 확인한다.

## 새 참조 문서 추가 규칙

새 참조 문서를 만들 때는 다음 정보를 포함한다.

- 어떤 요청에서 읽어야 하는지
- 필수 참조인지 선택 참조인지
- 관련 에이전트 또는 스킬
- 승인 게이트가 있는지
