# 현재 작업 후보와 핸드오프 현황

최종 갱신: 2026-07-27 KST

## 목적

이 문서는 최근 완료한 작업, 아직 닫히지 않은 검증 공백, 다음 작업 후보를 사용자가 한곳에서 확인하기 위한 현재 상태판이다.

세부 실행 로그는 `_workspace/active/`와 `_workspace/completed/`에 남긴다. 이 문서는 누적 이력 파일이 아니며, 다음 작업 발굴 시 현재 후보와 우선순위를 갱신하고 오래된 항목은 정리한다.

## 운영 기준

- 현재 후보와 우선순위는 최신 상태로 갱신한다.
- 최근 작업 요약은 3~5개 정도만 유지한다.
- 완료된 상세 이력은 `_workspace/completed/`를 참조한다.
- 진행 중 상세 이력은 `_workspace/active/`를 참조한다.
- 판단이 끝난 미결 항목은 결과만 짧게 남기고 제거한다.

## 현재 저장소 상태

- 작업 반영 기준 커밋: `ba883a2 art: integrate rat appearance candidate and visual gates`
- 후속 현황판 동기화: `350d520 docs: sync rat appearance push state`도 `origin/main` 반영 완료
- 반영 범위: A2/r6 최종 외형 후보, neutral idle 수정, QA staged 감사 통과, 총괄 내부 승인, 작업 패킷과 시각 게이트
- 정리 반영: 사용자 요청에 따라 r1~r5·r6 임시 preview 중간 바이너리 `1.38 MiB`를 삭제·커밋 제외하고 반려 사유만 문서로 보존
- post-push 상태: r6는 active 사용자 최종 외형 수용 대기이며 최종 채택·완료·보관 상태가 아니다.
- 로컬 제외 유지: `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` unstaged 변경, `_workspace/previews/` untracked
- `Builds/` 제외: 이번 커밋·푸시와 현재 post-push 변경에 포함하지 않음

## 현재 진행 중

| 작업 | 상태 | 목적 | 상세 기록 |
| --- | --- | --- | --- |
| 쥐 최종 외형 방향과 근접 샘플 | r6 QA·총괄 통과 — 사용자 최종 외형 수용 대기 | A2 기반 r6와 neutral idle 수정본이 QA를 통과했고 총괄이 사용자 제시 가능한 최종 외형 승인 후보로 판정했다. 사용자는 A2·r6 비교표·턴어라운드에서 몸통 캡슐/패널형 띠와 큰 귀·어두운 얼굴 대비를 확인한다. r1~r5 중간 바이너리는 삭제·커밋 제외하고 반려 사유만 문서에 보존했으며 전체 64프레임·atlas·Unity 반입은 미승인이다. | `_workspace/active/2026-07-24-rat-final-appearance-sample/` |
| 자연 경계도 100% Windows 빌드 성공 루프 엄격 검증 | 차단 — Computer Use 게임 창 캡처 오류 | `list_apps`와 새 빌드 실행·단일 창 식별은 복구됐지만 `get_window_state`가 `SetIsBorderRequired 0x80004002`로 최초·복구 1회 모두 실패했다. 화면 미확인 입력을 보내지 않아 자연 100% 이후 루프는 미검증이며, 실패 시도 로그와 정상 종료만 보존했다. | `_workspace/active/2026-07-16-natural-alert-build-loop-verification/` |

## 최근 작업 요약

| 작업 | 상태 | 핵심 결과 | 확인 위치 |
| --- | --- | --- | --- |
| 쥐 v3/v4/v5b 제작·표시 방식 통합 종결 | 완료 보관 | QA `완료 가능 — 총괄 수정 조건 해소`, 총괄 `내부 승인 가능`으로 Blender v3·Unity v3·v4 해상도·v5b 픽셀 처리와 umbrella를 보관했다. v5b 제작·표시 방식은 수용됐지만 현재 쥐의 체형·색감·얼굴·실루엣·보행은 최종 미승인이며 후속 재작업 대상이다. | `_workspace/completed/2026-07-24-2026-07-24-rat-visual-v3-v4-v5b-closeout/` |
| 쥐 시각·카메라 EditMode 회귀 기술 게이트 | 완료 보관 | 단일 테스트 계약 최소 수정 후 전체 EditMode `101/101`, 실패·skip·inconclusive 0, MCP Play의 RatHost·두 카메라·RatVisual·HUD·960×540 RT, Console 0, Stop/Edit clean과 씬·ProjectSettings·Builds 비변경을 확인했다. 총괄 `내부 승인 가능`; v4 직접 규격 자동화 공백과 사용자 시각 수용은 별도 유지한다. | `_workspace/completed/2026-07-24-2026-07-24-rat-visual-camera-editmode-regression/` |
| Game 뷰 카메라 출력 복구와 이동 정합 | 완료 보관 | Display 1 출력 복구, RatVisual 누적 픽셀 이탈 수정, WASD 입력 우선과 숙주 본능 복구를 완료했다. 독립 QA에서 무입력 360스텝 이탈 0, D/A/W/S 방향 내적 1, Console 0을 확인했고 총괄 `내부 승인 가능`, 사용자 종료·보관 승인을 받았다. | `_workspace/completed/2026-07-24-2026-07-21-game-view-camera-output-fix/` |
| Blender 애니메이션 테크아트 에이전트 역할 통합 | 완료 보관 | 사용자 승인으로 Blender 원본·리깅·보행·8방향 시험 렌더의 실제 제작 역할과 위임 절차를 추가했다. QA `완료 가능`, 총괄 `내부 승인 가능`; v2 시각 품질·Unity 통합은 별도 작업으로 유지한다. | `_workspace/completed/2026-07-20-2026-07-20-blender-animation-agent-role-integration/` |
| 쥐 정지 8방향 스프라이트 Unity 시험 반입 | 완료 보관 | 사용자 2차 접지 피드백을 수용했다. 위험 trigger를 유지한 채 시각 표면을 분리했고, QA MCP Play에서 8방향 실제 발 y `-0.015`, clearance `0.005`, 구역 안·밖 차이 `0.000`을 확인했다. QA `완료 가능`, 총괄 `내부 승인 가능`; EditMode 전체 재실행과 연속 WASD·경계 체감은 남은 확인이다. | `_workspace/completed/2026-07-20-2026-07-16-rat-directional-sprite-unity-integration/` |
| 8방향 쥐 시험 에셋 | 완료 보관 | 단일 저폴리 원본과 정지 8방향 PNG·시트·프리뷰를 만들고 QA·총괄 판정을 거쳤으며, 사용자가 `index.html`로 결과를 확인했다. | `_workspace/completed/2026-07-16-2026-07-16-rat-8-direction-trial-asset/` |
| 3D 원본 기반 8방향 캐릭터 스프라이트 방향 정리 | 완료 보관 | 쥐 프로토타입은 3D 환경·게임플레이 루트를 유지하고 쥐·바이러스·백혈구를 3D 원본 기반 8방향 스프라이트로 표시하도록 문서화했다. 실제 에셋·Unity 적용과 정확 사양은 후속 승인으로 분리했고 QA `완료 가능`, 총괄 `내부 승인 가능` 판정을 받았다. | `_workspace/completed/2026-07-16-2026-07-16-prerendered-character-sprite-direction/` |
| 면역 신호 억제 접근 예고 검증 종결 | 완료 보관 | EditMode 90/90과 직접 상태 전환 Play에서 대기·접근·정확 HUD, cue, 색·scale, 콘솔 0건, 종료·씬 무변경을 확인했고 총괄 `내부 승인 가능` 판정을 받았다. | `_workspace/completed/2026-07-16-2026-07-10-signal-suppression-approach-cue/` |
| 현황판·완료 작업 보관 정합성 복구 | 완료 보관 | 게이트를 충족한 두 작업을 보관하고 상태판·`CURRENT.md`를 실제 Git·작업 경로와 맞췄다. QA `완료 가능`과 총괄 `내부 승인 가능` 판정을 받은 뒤 이번 작업도 완료 보관했다. | `_workspace/completed/2026-07-16-2026-07-16-current-task-board-consistency/` |
| 쥐 숙주 전체 플레이 검증 | 완료 보관 | 자동/대리 입력 기준 성공 루프와 완료 게이트를 충족한 기록을 보관했다. 사용자 조작감·난이도·무설명 이해 여부는 별도 보류다. | `_workspace/completed/2026-07-10-2026-07-01-rat-host-full-play-verification/` |
| AI 보조 도트풍 3D 아트 제작 규칙과 작업 순서 | 완료 | 래스터 초안 범위, 자산 묶음별 승인, 출처 기록, 선별, 후속 저폴리 3D·Unity·QA 순서를 문서화했다. | `_workspace/completed/2026-07-13-2026-07-13-ai-assisted-pixel-art-workflow/` |

## 보류 항목

### 사용자 수동 플레이 체감 확인

- 상태: 사용자 확인 전까지 보류
- 이유: 자동/대리 입력 기준의 핵심 루프는 닫혔지만, 공식 프로토타입 성공 기준의 조작감, 난이도, 설명 없이 목표 이해 여부는 사람이 직접 봐야 한다.
- 범위: Windows 빌드 실행본을 사용자가 직접 플레이하며 이동, 위험 노출, 내부 미니게임, 변이 선택, 복귀의 이해 가능성과 체감을 확인.
- 체크리스트: `docs/project-handoff/manual-play-checklist.md`

## 다음 작업 후보

쥐 최종 외형 작업은 A2 기반 Blender r6 제작, neutral idle 수정, QA·총괄 검토까지 통과했다. 현재 r6는 사용자에게 제시 가능한 최종 외형 승인 후보이며, 몸통 캡슐/패널형 띠와 큰 귀·어두운 얼굴 대비의 수용 여부를 사용자가 결정해야 한다. r1~r5 중간 바이너리는 삭제·커밋 제외하고 반려 사유만 문서에 보존하며 전체 64프레임·atlas·Unity 반입은 진행하지 않는다.

## 최근 판단 항목

### 완료 작업 보관 기준

- `2026-07-01-rat-host-full-play-verification`과 `2026-07-09-white-blood-cell-response-scaling`은 QA 완료 판단과 총괄 관리자 판정 기록이 있어 `completed/`로 이동했다.
- `2026-07-10-signal-suppression-approach-cue`는 QA `완료 가능`과 총괄 `내부 승인 가능` 판정을 받아 `_workspace/completed/2026-07-16-2026-07-10-signal-suppression-approach-cue/`로 보관했다.
- 보류 작업은 완료나 다음 작업 후보로 중복 기재하지 않는다.

### 접근 예고 검증 종결 경계

- computer-use 네이티브 연결 불가로 실제 F6 수신은 미검증이다.
- 완료 근거는 `MCP 직접 상태 전환 대체 검증`이며 F6 키 입력 통과를 의미하지 않는다.
- 사용자 수동 플레이 체감 확인은 별도 보류로 유지한다.

### 자연 성공 루프 엄격 검증 경계

- 이번 통과 주장은 같은 Windows 빌드 실행 세션의 `RatHost 시작 → 자연 경계도 100% → 기본 WhiteBloodCellEvasion → 조각 3개 → 변이 선택 → 변이 적용 RatHost 복귀`로 제한한다.
- `F6`, 직접 상태 전환, Unity Editor 대체 검증은 성공 근거로 인정하지 않는다.
- 사용자 수동 플레이 체감·난이도·무설명 이해 여부는 별도 보류로 유지한다.
- 완료된 카메라·이동 변경, 완료 보관, 최신 차단·현황판 기록은 이번 선별 커밋에 포함해 반영했다. `UnityProject/ProjectSettings/ProjectSettings.asset`, `_workspace/previews/`, `Builds/`, 그 외 예상 밖 경로는 제외했다.

### 엄격 검증 차단 판정

- QA 판정: `차단`.
- 프로젝트 총괄 관리자 판정: `보류`.
- 마지막 정상 확인: Computer Use 연결, 기존 Windows 빌드 실행, 단일 `Last Host` 창 식별, 실패 시도 정상 종료, 같은 시도 `Player.log` 보존.
- 실패: 게임 창 캡처가 최초와 새 창 객체 복구 1회 모두 `SetIsBorderRequired 0x80004002`로 실패했다.
- 미검증: 창 포커스와 실제 입력, 단계별 화면, 자연 성공 루프, 동일 성공 세션 `Player.log`.
- 커밋·푸시: 사용자 명시 지시에 따라 완료된 카메라·이동 변경과 현재 차단 기록의 선별 커밋·푸시를 완료했다. 이는 자연 경계도 기능 완료 승인이 아니며 작업은 active·QA `차단`·총괄 `보류`로 유지한다.

## 추천 순서

1. A2 참고안, r6 비교표, r6 턴어라운드를 사용자에게 제시해 최종 외형 수용 여부를 확인한다.
2. 수용·수정·반려 중 사용자 결정을 기록하고 그전까지 active 상태를 유지한다.
3. 최종 제품용 8방향 시트, 전체 64프레임, runtime atlas/스프라이트 시트, Unity 반입은 이후 각각 별도 승인한다.
