# 현재 작업 후보와 핸드오프 현황

최종 갱신: 2026-07-30 KST

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

- 현재 Git 기준: `main` 푸시 완료. 구현 `e7220a7 feat: integrate production 2d art sample`, 1차 상태 동기화 `7adef75 docs: sync production 2d push state`
- 현재 로컬 작업: 고품질 실제 RGBA 환경·쥐·HUD 제작, 분리된 `RatHost2DTechnicalSample` 반입, 오브젝트 가림·HUD 초상 잔여 조각 수정까지 기술·비주얼·독립 QA와 총괄 검토를 통과해 `origin/main`에 푸시했다. 첫 아트 후보→통합 기준→품질 마스터→실제 에셋→Unity 반입→가림/HUD 수정 체인과 현재 상태판만 포함했으며, 사용자 실제 WASD·PPU 128·상대 크기 수용은 커밋과 별도로 남는다.
- 이전 쥐 외형 반영 기준: `ba883a2 art: integrate rat appearance candidate and visual gates`, 후속 현황판 동기화 `350d520 docs: sync rat appearance push state`
- 기존 반영 범위: A2/r6 최종 외형 후보, neutral idle 수정, QA staged 감사 통과, 총괄 내부 승인, 작업 패킷과 시각 게이트
- 정리 반영: 사용자 요청에 따라 r1~r5·r6 임시 preview 중간 바이너리 `1.38 MiB`를 삭제·커밋 제외하고 반려 사유만 문서로 보존
- 기술 샘플 경계: 신규 패키지·최종 아트·기존 3D 교체 없이 별도 씬에서 시험 규격을 검증한다.
- 로컬 제외 유지: `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` unstaged 변경, `_workspace/previews/` untracked
- `Builds/` 제외: 현재 완료 보관 동기화에 포함하지 않음
- 이번 선별 커밋 제외: Stage2·Stage3 소유 변경, 사용자에게 반려된 저품질 규격 시험 산출물, Python `__pycache__`
- Stage2 임시 정리: QA 빌드 성공 기록은 문서로 남기고 `C:\tmp\LastHostRatHost2DStage2` 약 `205 MB`와 정적 컴파일 DLL/PDB는 사용자 요청으로 삭제

## 현재 진행 중

| 작업 | 상태 | 목적 | 상세 기록 |
| --- | --- | --- | --- |
| Production2D V1 오브젝트 가림·HUD 초상 잔여 조각 수정 | 내부 승인 완료 — 사용자 실제 WASD 재확인 대기 | 정렬 전환선이 tieBreak 때문에 지면 접점에서 0.03~0.12만큼 밀리고 collider footprint가 좁은 원인을 재현했다. 정적 tieBreak 1 통일, 쥐·통·상자 지면 점유 폭 조정, HUD portrait 상단 황동 잔여 2,173픽셀 제거를 적용했다. 비주얼 PASS, 독립 관련 EditMode `44/44`·전체 `198/198`, Play 충돌 겹침·정렬 jitter 0, Console 0·scene clean·보호 diff와 총괄 `내부 승인 가능`을 통과했다. 실제 네이티브 WASD 모서리 통과 체감은 사용자 확인 항목이다. | `_workspace/active/2026-07-30-production2d-occlusion-hud-correction/` |
| 고품질 실제 에셋 Unity 한 방 반입 기술 샘플 | 내부 승인 완료 — 사용자 실제 WASD·PPU 수용 대기 | 실제 RGBA 환경·쥐·HUD를 독립 2D 기술 샘플 한 방에 반입했다. V1 HUD·프레이밍 문제를 V2에서 수정했고 비주얼 `PASS`, SHA `20/20`, Import `18/18`, 관련 `42/42`·전체 EditMode `196/196`, MCP Play·충돌·Y정렬·카메라·HUD·Console 0을 통과했다. 자동 직렬화 변경도 원복·QA 재대조했고 총괄 `내부 승인 가능`이다. 실제 Game View 포커스 네이티브 WASD와 PPU 128·상대 크기 수용은 사용자 확인 항목이다. | `_workspace/active/2026-07-30-rat-host-2d-production-assets-unity-sample/` |
| 고품질 마스터 기준 실제 게임 에셋 1차 재제작 | 사용자 품질 수용 — Unity 반입으로 인계 | 반복 환경·투명 소품, 공통 캔버스·피벗의 쥐 측면 3프레임, 분리 HUD 모듈 20파일을 제작했다. HUD fill 문제를 수정했고 비주얼 PASS, QA `128/128`, 재생성 `20/20`, 총괄 내부 승인을 통과한 뒤 사용자가 Unity 반입을 승인했다. | `_workspace/active/2026-07-30-rat-host-2d-production-assets-v1/` |
| 쥐 숙주 2D 품질 우선 고품질 수직 샘플 | 사용자 품질 수용 — 실제 에셋 재제작으로 인계 | 환경·자연형 쥐 측면 보행·HUD 고품질 제작 마스터 3종을 만들었다. 쥐 프레임 체형 불일치를 한 차례 반려·재생성했고 최종 비주얼 PASS, 원본 추적성 QA와 총괄 내부 승인 뒤 사용자가 품질을 수용했다. | `_workspace/active/2026-07-30-rat-host-2d-quality-first-vertical-slice/` |
| 쥐 숙주 2D 실제 아트 제작 순서·기간 로드맵 | 고품질 대표 샘플 재제작 기준 보완 필요 | 기존 빠른 트랙과 통합 기준은 유지하되, 실제 아트 단계에서는 목표 목업에 가까운 묘사 밀도와 수작업 도트 페인트오버·정리를 필수 게이트로 추가해야 한다. | `_workspace/active/2026-07-29-rat-host-2d-art-production-roadmap/` |
| 3단계 2D 변이 선택·효과·쥐 숙주 복귀 | 내부 승인 가능 — 사용자 실제 입력·화면 수용 대기 | 세 변이 단일 선택·효과·Host 복귀, 전용 통로·HUD를 구현했다. 구현 측 `6/6`·`53/53`, 독립 원본 Play·Console·보호 diff와 상태판 운영 게이트를 통과했다. 독립 전체 EditMode는 MCP TestRunner 오류로 미확인이다. | `_workspace/active/2026-07-29-rat-host-2d-stage3-mutation-return/` |
| 2단계 2D 백혈구 회피 미니게임과 성공·실패 인계 | 사용자 부분 수용 — 검은 화면 해소·실제 이동 확인 | 원본 씬의 Floor `117`, Water `5`, Blocking `40` 셀과 13×9 맵 표시를 복구했다. 독립 QA 기술 게이트와 총괄 검토를 통과했고, 사용자가 화면 표시와 이동을 확인했다. Space 실패 확인과 내부 화면 체감은 남아 있다. | `_workspace/active/2026-07-28-rat-host-2d-stage2-minigame/` |
| 1단계 2D 쥐 숙주·면역 경계도·자연 100% 전환 통합 | 원본 Reload·MCP 차단 해소 — Stage2 사용자 확인에 통합 | Stage2 원본 씬 복구와 QA가 과거 Reload 차단을 해소했다. 별도 완료 보관은 Stage2 사용자 실제 WASD/Space 확인 뒤 함께 정리한다. | `_workspace/active/2026-07-28-rat-host-2d-stage1-integration/` |
| 자연 경계도 100% Windows 빌드 성공 루프 엄격 검증 | 차단 — Computer Use 게임 창 캡처 오류 | `list_apps`와 새 빌드 실행·단일 창 식별은 복구됐지만 `get_window_state`가 `SetIsBorderRequired 0x80004002`로 최초·복구 1회 모두 실패했다. 화면 미확인 입력을 보내지 않아 자연 100% 이후 루프는 미검증이며, 실패 시도 로그와 정상 종료만 보존했다. | `_workspace/active/2026-07-16-natural-alert-build-loop-verification/` |

## 최근 작업 요약

| 작업 | 상태 | 핵심 결과 | 확인 위치 |
| --- | --- | --- | --- |
| 쥐 숙주 2D 통합 제작 기준 이미지 | 완료 보관·사용자 수용 | 환경·쥐 V2·HUD 통합 기준 3개를 제작했다. 쥐 V2는 뒤쪽 체형을 교정하고 비주얼·QA·총괄 재검토를 통과했으며, 사용자가 세 이미지를 모두 수용했다. 실제 에셋·8방향·Unity 반입은 후속 승인이다. | `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-integrated-art-targets/` |
| 쥐 숙주 2D 첫 아트 샘플 후보 | 완료 보관·조합 선별 | 환경·소품 2안, 쥐 대표 3방향 2안, HUD 2안 총 6개를 생성·검토했고, 사용자 선별 조합을 후속 통합 기준에 반영했다. | `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-first-art-sample/` |
| 쥐 숙주 핵심 루프 단계적 2D 이관 승인 브리프 | 완료 보관 | 사용자 전체 추천안 승인으로 3단계 이관, 별도 2D 프로토타입 씬, 상태 재사용·2D 어댑터, 오염 노출 자연 100%, 신호 억제 보류, 레거시 보존과 1단계 착수를 확정했다. | `_workspace/completed/2026-07-28-2026-07-27-rat-core-loop-2d-migration-brief/` |
| 실제 2D 플레이어블 기술 샘플 | 완료 보관·구현 커밋 `a2cfe20` | 별도 2D 씬에 Tilemap 3개, 2D 충돌, WASD·8방향 쥐, 카메라, Y 정렬, HUD를 구현했다. 사용자 피드백으로 Pipe/Barrel 하단 충돌도 보강했고 전체 EditMode 139/139, 실제 Host/Move 충돌·우회·Y-sort, 최신 Windows 임시 빌드, 보호 설정을 통과했다. 사용자 플레이 수용, QA `완료 가능`, 총괄 `내부 승인 가능`; Windows 실행본은 빌드 성공/실행 미검증 경계를 유지한다. | `_workspace/completed/2026-07-27-2026-07-27-2d-playable-technical-sample/` |
| 프로젝트 2D 아이소메트릭 방향 전환 | 완료 보관 | 목업 기반 2D 방향, reference, ChatGPT 이미지 연계 워크플로와 전담 에이전트를 동기화했다. 최종 QA `PASS — 완료 가능`, 총괄 `내부 승인 가능`; Unity 플레이어블은 변경하지 않았다. | `_workspace/completed/2026-07-27-2026-07-27-project-2d-direction-transition/` |
| 쥐 최종 외형 방향과 근접 샘플 | 레거시 완료 보관 | r6는 당시 QA·총괄을 통과한 후보였으나 사용자 최종 채택 전 2D 방향으로 전환됐다. 신규 제작 기준이 아닌 2.5D/Blender 레거시 이력으로 보관했다. | `_workspace/completed/2026-07-27-2026-07-24-rat-final-appearance-sample/` |
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

현재 최우선 작업은 QA를 통과한 벽·통·상자 가림·충돌과 HUD 초상 수정본을 사용자가 실제 WASD와 최종 화면으로 재확인하는 것이다. 사용자 재확인 전 PPU 128 최종 수용이나 전체 8방향·전체 타일셋 확장으로 넘어가지 않는다. Stage3 실제 `1/2/3`·버튼·HUD·이동·전용 통로 확인과 Stage2 실제 Space 키 수신은 병행 사용자 수용 항목으로 유지한다.

## 최근 판단 항목

### 2D 아이소메트릭 방향 전환 경계

- 사용자 승인으로 목업 기반 2D 아이소메트릭 도트가 현재 프로젝트 비주얼·공간 표현 기준이 됐다.
- `docs/design/visual/references/rat-host-2d-isometric-gameplay-mockup-v1.png`는 목표 분위기와 화면 구성 reference이며 반복 타일셋·스프라이트 시트·애니메이션 프레임이 아니다.
- 기존 3D 환경·저폴리 원본·Blender 프리렌더와 r6는 삭제하지 않고 레거시 이력으로 보관한다.
- ChatGPT 이미지 아트 에이전트와 AI 연계 워크플로는 후보 생성·기록·사람 선별을 담당하며, 게임 규격 재제작과 QA 전에는 최종 에셋으로 선언하지 않는다.
- 이번 전환 작업에는 Unity 씬·코드·ProjectSettings·패키지 변경과 실제 2D 플레이어블 기술 샘플 구현이 포함되지 않는다.

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

1. 사용자가 수정 Unity 샘플에서 실제 WASD로 통·상자·벽 모서리 왕복과 짧은 방향 반전을 확인한다.
2. 사용자가 HUD 초상 상단 잔여 조각 제거와 쥐 본체 보존을 확인한다.
3. 사용자 수용 뒤 PPU 128·전체 8방향·전체 프로토타입 아트 적용 범위를 결정한다.
