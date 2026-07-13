# 현재 작업 후보와 핸드오프 현황

최종 갱신: 2026-07-13

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

- 최신 커밋: `2b1fa26 feat: refine immune response verification flow`
- 원격 반영: `origin/main` 반영 완료
- Unity 코드·씬·테스트·설정 변경: 없음
- 미커밋 작업 기록: 현재 상태판 동기화와 `_workspace/active/2026-07-13-manual-play-candidate-cleanup/`
- 작업 범위 밖 로컬 변경: `.codex/config.toml`

## 최근 작업 요약

| 작업 | 상태 | 핵심 결과 | 확인 위치 |
| --- | --- | --- | --- |
| Unity MCP 입력 검증 방식 표준화 | 완료 | 실제 Game View 네이티브 키 입력, MCP 직접 상태 전환 대체 검증, `SendKeys`·포커스·승인·relay 차단을 분리한 표준 절차와 기록 양식을 운영 문서에 반영 | `_workspace/completed/2026-07-13-2026-07-13-unity-mcp-input-verification-standard/` |
| 원인 라벨 enum/event type 전환 | 완료, 사용자 체감 확인 별도 | `FeedbackLabel`과 원인 타입을 분리해 타입 기반 미니게임 선택으로 전환. `면역 신호`·`경보`의 면역 신호 억제 매핑을 보존했고, EditMode 90/90 및 Unity MCP Play·Console 검증을 통과 | `_workspace/completed/2026-07-13-2026-07-13-alert-cause-event-type-review/` |
| 면역 신호 억제 2단계 리듬 확장 | 완료, 사용자 체감 확인 별도 | 두 번째 내부 대응부터 변칙 간격·빠른 신호와 리듬 HUD를 적용. EditMode 81/81 통과, 체감 난이도·HUD 배치만 사용자 확인 필요 | `_workspace/completed/2026-07-13-2026-07-13-immune-signal-suppression-stage2-rhythm/` |
| 내부 대응 원인별 미니게임 선택 | 완료, Unity MCP 직접 검증 제한 | 경계도 원인 라벨에 따라 백혈구 회피 또는 면역 신호 억제를 선택. 임시 실행 하니스 `CAUSE_SELECTION_HARNESS_OK 6/6` 통과 | `_workspace/completed/2026-07-10-2026-07-10-internal-response-cause-minigame-selection/` |
| 전체 플레이어블 검증 재정리 / 빌드 성공 루프 최종 확인 | 자동/대리 입력 기준 통과, 사용자 체감 확인 별도 | Editor Play 성공/실패 루프 통과, 빌드 실행본 4개 해상도 구동/화면 확인, 1280x720 실패 복귀와 F6 `면역 신호 억제` 성공 루프 확인 | `_workspace/active/2026-07-01-rat-host-full-play-verification/` |
| 상호작용 대상 식별성 보정 | 완료 처리, 사용자 체감 확인 별도 | 배관/밸브/표식 링, 근접 HUD, `Space` 면역 경계도 상승 검증 기록 확인. active 작업을 completed로 이동 | `_workspace/completed/2026-07-10-2026-07-01-rat-interaction-affordance/` |
| MCP 승인 복구 후 누락 검증 | 완료 | 실제 F6 진입, HUD 접근 cue/정확 판정 cue, 콘솔 Error/Warning 0건, 씬 dirty=false 확인 | `_workspace/completed/2026-07-10-2026-07-10-mcp-recovered-signal-cue-verification/` |
| 백혈구 회피 대응 경험 스케일링 | 커밋/푸시 완료 | 내부 대응 경험에 따라 백혈구 속도 배율 증가, 실패 시 추가 압박 | `_workspace/active/2026-07-09-white-blood-cell-response-scaling/` |

## 보류 항목

### 사용자 수동 플레이 체감 확인

- 상태: 사용자 확인 전까지 보류
- 이유: 자동/대리 입력 기준의 핵심 루프는 닫혔지만, 공식 프로토타입 성공 기준의 조작감, 난이도, 설명 없이 목표 이해 여부는 사람이 직접 봐야 한다.
- 범위: Windows 빌드 실행본을 사용자가 직접 플레이하며 이동, 위험 노출, 내부 미니게임, 변이 선택, 복귀의 이해 가능성과 체감을 확인.
- 체크리스트: `docs/project-handoff/manual-play-checklist.md`

## 다음 작업 후보

현재 후보 없음. 사용자 수동 플레이 체감 확인은 `보류 항목`과 체크리스트에서만 관리한다.

## 최근 판단 항목

### 내부 대응 원인별 미니게임 선택

- 상태: 완료, Unity MCP 직접 검증 제한
- 변경 내용: `오염`, `면역 포착`, `바이러스 흔적` 계열은 백혈구 회피를 우선하고 `강제 조종`, `소음`, `조직 자극`, `면역 신호`, `경보` 계열은 면역 신호 억제로 연결한다.
- 검증: Unity 내장 Mono/Roslyn 제품/테스트 어셈블리 임시 컴파일 통과, 임시 실행 하니스 `CAUSE_SELECTION_HARNESS_OK 6/6`.
- 제한: Unity MCP `RunCommand` 직접 검증은 relay 응답 문제로 완료 결과 미확보.

### ProjectSettings `APP_UI_EDITOR_ONLY`

- 상태: 되돌림 완료
- 변경 내용: `Standalone` scripting define에 `APP_UI_EDITOR_ONLY`가 추가되어 있었다.
- 의미: Unity `com.unity.dt.app-ui` 패키지의 Editor Only 설정으로, App UI 런타임 어셈블리를 플레이어 빌드에서 제외하는 define이다.
- 판단: 현재 프로토타입은 `UnityEngine.UI` 기반이고 App UI 런타임을 직접 쓰지 않지만, 사용자가 되돌림을 요청했으므로 ProjectSettings 변경은 커밋하지 않고 되돌렸다.

## 추천 순서

현재 새 작업 추천 없음. 사용자 수동 플레이 체감 확인은 보류 상태와 체크리스트만 유지한다.
