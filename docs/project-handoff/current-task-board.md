# 현재 작업 후보와 핸드오프 현황

최종 갱신: 2026-08-09 KST

## 목적

이 문서는 최근 완료한 작업, 아직 닫히지 않은 검증 공백, 다음 작업 후보를 사용자가 한곳에서 확인하기 위한 현재 상태판이다.

세부 실행 로그는 `_workspace/active/`와 `_workspace/completed/`에 남긴다. 이 문서는 누적 이력 파일이 아니며, 다음 작업 발굴 시 현재 후보와 우선순위를 갱신하고 오래된 항목은 정리한다.

R2/R3 또는 실제 고비용 실행 작업의 비용은 `docs/project-handoff/task-cost-dashboard.md`에서 확인한다. 이 상태판은 active와 next 후보를 중심으로 갱신하며 완료 상세는 `_workspace/completed/`에 둔다.

## 운영 기준

- 현재 후보와 우선순위는 최신 상태로 갱신한다.
- 최근 작업 요약은 3~5개 정도만 유지한다.
- 완료된 상세 이력은 `_workspace/completed/`를 참조한다.
- 진행 중 상세 이력은 `_workspace/active/`를 참조한다.
- 판단이 끝난 미결 항목은 결과만 짧게 남기고 제거한다.
- 2026-08-06 이후 R0과 고비용 실행 없는 R1은 상태판 항목을 만들지 않는다. 기존 항목은 이력으로 소급 정리하지 않는다.

## 현재 저장소 상태

- 현재 로컬 Git HEAD와 `origin/main`: `c50f71a docs: define opening edit beats` 반영 완료.
- 현재 Git 작업 상태: Task 4 v11 상세 숏 명세와 R2·공유 상태 문서는 원격 반영 완료다. 최종 감사 fingerprint는 동결하며 production 의미는 변경하지 않는다.
- 현재 커밋 경계: Task 4 v11 production 문서와 직접 상태 기록은 `c50f71a`에 반영됐다. ProjectSettings·preview·`Builds/` 보호 원칙은 유지한다.
- 최근 완료: 자연 부분 가림 R2 최종 후보 `5cd81d7c…`는 gameplay `3/3`, scene `8/8`, stale fixture `4/4`, 전체 EditMode `203/203`, QA Play r3 PASS, Console Error 0·scene dirty false와 총괄 내부 승인을 통과했다. 사용자가 최종 가림 화면과 쥐 본체 보존을 수용한 내용임을 재확인해 완료 보관했다.
- 이전 쥐 외형 반영 기준: `ba883a2 art: integrate rat appearance candidate and visual gates`, 후속 현황판 동기화 `350d520 docs: sync rat appearance push state`
- 기존 반영 범위: A2/r6 최종 외형 후보, neutral idle 수정, QA staged 감사 통과, 총괄 내부 승인, 작업 패킷과 시각 게이트
- 정리 반영: 사용자 요청에 따라 r1~r5·r6 임시 preview 중간 바이너리 `1.38 MiB`를 삭제·커밋 제외하고 반려 사유만 문서로 보존
- 기술 샘플 경계: 신규 패키지·최종 아트·기존 3D 교체 없이 별도 씬에서 시험 규격을 검증한다.
- 보호 유지: ProjectSettings·`_workspace/previews/`·`Builds/`는 관련 변경이 다시 나타나도 별도 승인·검증 없이 후속 커밋에 섞지 않는다.
- 선별 커밋 결과: 오프닝 시네마틱·세균 감염 접속형·신규 콘텐츠 알림 승인 문서, canonical QA·총괄 감사 R2 기록과 현황판·비용 행을 `592dd4b`로 `origin/main`에 반영 완료했다.
- 후속 변경 제외 원칙: 위 반영 범위와 관련성 또는 소유권이 확정되지 않은 변경은 자동 포함하지 않는다.
- Stage2 임시 정리: QA 빌드 성공 기록은 문서로 남기고 `C:\tmp\LastHostRatHost2DStage2` 약 `205 MB`와 정적 컴파일 DLL/PDB는 사용자 요청으로 삭제

## 현재 진행 중

| 작업 | 상태 | 목적 | 상세 기록 |
| --- | --- | --- | --- |
| 오프닝 시네마틱·세균 감염 접속형 첫 튜토리얼 설계 | Task 4 상세 숏 명세 내부 승인 통과·`c50f71a` 원격 반영 완료 — Task 5 편집 비트 유지·병합·분할 결정 대기 | Task 4를 최종 숏 수가 아닌 `A01~T01` 33개 편집 비트 후보와 15필드·병합/분리 경계·기원 공개 장부로 상세화했다. canonical QA `qa-opening-edit-beats-contract-correction-030` C1~C14 PASS, 총괄 `director-opening-edit-beats-final-audit-031` 내부 승인 가능·최소 수정 0이다. 실제 스토리보드·카메라·레이어·이미지·에셋·오디오·UI·저장·코드·Unity는 후속 승인이다. | `_workspace/active/2026-08-08-opening-cinematic-origin/` |
| 메인 시나리오 디렉터 역할과 오프닝 구조 동기화 | 내부 승인 가능 — 사용자 문서 확인 대기 | 전체 서사·숙주/맵·성장·기원 미스터리의 연속성을 맡는 역할을 추가했다. 독립 QA C1~C6 PASS와 총괄 내부 승인을 통과했으며, 평온한 독립 3씬 뒤 혼합형 감염 확산을 두고 러닝타임은 숏 구성 뒤 산정한다. | `_workspace/active/2026-08-08-main-scenario-director-agent/` |
| 픽셀아트 모션 코믹형 시네마틱 기준·전담 역할 | 내부 승인 가능 — 사용자 가이드 수용 대기 | 컷신 기본 형식과 숏·스토리보드·레이어 명세·비최종 애니매틱 계획·Unity 인계 전담 역할을 추가했다. 독립 QA C1~C7 PASS와 총괄 내부 승인을 통과했으며 이미지·실제 애니매틱·Unity 구현은 후속 승인으로 분리한다. | `_workspace/active/2026-08-08-pixel-motion-comic-cinematics/` |
| 전체 게임 시나리오·화면 흐름·튜토리얼 초안 | 내부 승인 가능 — 사용자 내용 보완·수용 대기 | 시작 화면부터 튜토리얼, 숙주 탐험·내부 바이러스·변이·실패·숙주 전이·5엔딩까지 17구간으로 연결했다. 독립 QA correction 1에서 C1~C5 PASS, 총괄 내부 승인 가능이며 쥐 프로토타입만 현재 확정 범위다. | `_workspace/active/2026-08-07-main-scenario-outline/` |
| 바이러스 주인공 기본 외형 기준 반영 | 완료 보관 — correction 1 QA PASS·총괄 내부 승인 가능 | 사용자 제공 박테리오파지 원본과 canonical reference의 `1036×1248`·bytes·SHA-256, 기준 문서·색인·2D production 경계·금지 문구·비반입을 검증했다. 기존 A/B/C는 `SUPERSEDED` 이력이며 실제 턴어라운드·시트·Unity 적용은 별도 승인이다. | `_workspace/completed/2026-08-06-2026-08-06-virus-character-concept-v1/` |
| 시작 화면 V2 비주얼 후보 제작 | 방향 재검토 — 바이러스 콘셉트 선택 전 보류 | 기존 후보는 먹이사슬과 숙주 사이 바이러스 이동감이 부족해 선별하지 않는다. 바이러스 캐릭터 기준을 먼저 선택한 뒤 새 시작 화면 브리프로 재개한다. | `_workspace/active/2026-08-06-startup-ui-visual-v2/` |
| 작업 기록·검증 운영 경량 구조 개편 | 완료 보관 — 독립 QA PASS·총괄 내부 승인 가능 | R0 무폴더, R1 단일 `record.md`, R2/R3 기본 두 파일, 조건부 분리 기록·artifact, completed 동일 폴더 이동과 기존 안전 게이트를 27파일 revision에서 검증했다. | `_workspace/completed/2026-08-06-2026-08-06-workspace-recording-lightweight/` |
| Unity MCP client relay 일괄 정리 전역 스킬 | 완료 보관 — 전역 설치·QA PASS·총괄 승인 | Codex-owned `relay_win.exe --mcp`만 정리하는 전역 스킬을 설치했다. 실제 relay 종료는 별도 요청 전까지 수행하지 않았다. | `_workspace/completed/2026-08-06-2026-08-06-unity-mcp-relay-resetter/` |
| PC 시작 화면·설정 UI와 다국어 준비 구조 | 내부 승인 가능 — Play 진입·언어별 폰트 `38/38` PASS, 사용자 실제 화면 수용 대기 | Editor Play를 저장된 Startup 씬으로 고정하고 선택 배경을 연결했다. 한국어 Galmuri11·영어 Silkscreen profile과 라이선스·누락 진단을 추가했다. canonical UnityEditMode `38/38` PASS; McpPlay unavailable로 실제 960×540 화면·입력·가독성은 사용자 확인 대상으로 남긴다. | `_workspace/active/2026-08-05-startup-settings-localization-ui/` |
| Unity MCP 프로젝트 로컬 relay 경로 교정 | 완료 보관 — 실제 연결 재검증 대기 | `.codex/config.toml`의 `unity_mcp.command`를 `C:\Users\bumci\.unity\relay\relay_win.exe`로 교정했다. QA 기술 검증 통과·총괄 내부 승인 가능; 실제 MCP 연결은 Codex 재시작 후 확인한다. | `_workspace/completed/2026-08-04-2026-08-04-unity-mcp-local-path-fix/` |
| 검증 current-state 상태 계약 교정 | 완료 보관·운영 `a33164b` push | 후보 `71e4dcdd…`: unknown/status-only stale 차단, route expected status와 `ready-for-verification`→`verification-running` 전이, 기존 G1~G8 회귀를 구현자·QA 각 1회 `24/24`로 확인했다. Unity/MCP/빌드 0. | `_workspace/completed/2026-08-03-2026-08-02-verification-current-state-contract/` |
| 검증 하네스 비용·재시도 차단 보완 | 완료 보관·운영 `a33164b` push — 후속 R3로 SUPERSEDED | 미지원 route·Reflection·stale contract·실패 2회·cache·full-history·low-level 우회는 유지되고, status 값·전이 계약은 후속 최종 후보 `71e4dcdd…`로 대체됐다. | `_workspace/completed/2026-08-03-2026-08-02-verification-harness-cost-guards/` |
| Production2D 자연 부분 가림·실제 충돌 루트 교정 | 완료 보관 — 사용자 수용 | 최종 `5cd81d7c…`: gameplay `3/3`, scene `8/8`, stale fixture `4/4`, 전체 EditMode `203/203`, QA Play r3 PASS, Console Error 0·scene dirty false, 3D legacy 보존. 총괄 `내부 승인 가능`과 사용자 자연 부분 가림 화면·쥐 본체 보존 수용을 충족했다. 상태-only 종결에서 Unity/QA 재실행은 0이며 비용은 `과다 — 부분 회피 가능`이다. | `_workspace/completed/2026-08-05-2026-08-02-production2d-natural-occlusion-root-fix/` |
| Production2D 쥐·오브젝트 가시 실루엣 겹침 완전 교정 | 기술 검증 PASS·사용자 acceptance FAIL — 새 작업으로 SUPERSEDED/수정 필요 | `7ba12df`는 자동 검증을 통과했지만 wall/barrel/crate 접촉 시 쥐 전체 renderer를 꺼 증상을 숨긴다는 사용자 판정을 받았다. 해당 방식은 재사용하지 않고 새 R2 자연 가림 루트 교정으로 대체한다. | `_workspace/active/2026-08-02-production2d-visual-overlap-correction/` |
| Production2D V1 오브젝트 가림·HUD 초상 잔여 조각 수정 | 부분 수용 — HUD 통과, 가림 후속 교정으로 재개 | HUD 잔여 조각 제거는 유지한다. tieBreak 1 통일과 footprint 확대는 자동 검증을 통과했지만 사용자 실제 화면에서 가시 실루엣 관통이 남아, 가림 부분은 2026-08-02 후속 교정으로 넘겼다. | `_workspace/active/2026-07-30-production2d-occlusion-hud-correction/` |
| 고품질 실제 에셋 Unity 한 방 반입 기술 샘플 | 내부 승인 완료 — 사용자 실제 WASD·PPU 수용 대기 | 실제 RGBA 환경·쥐·HUD를 독립 2D 기술 샘플 한 방에 반입했다. V1 HUD·프레이밍 문제를 V2에서 수정했고 비주얼 `PASS`, SHA `20/20`, Import `18/18`, 관련 `42/42`·전체 EditMode `196/196`, MCP Play·충돌·Y정렬·카메라·HUD·Console 0을 통과했다. 자동 직렬화 변경도 원복·QA 재대조했고 총괄 `내부 승인 가능`이다. 실제 Game View 포커스 네이티브 WASD와 PPU 128·상대 크기 수용은 사용자 확인 항목이다. | `_workspace/active/2026-07-30-rat-host-2d-production-assets-unity-sample/` |
| 고품질 마스터 기준 실제 게임 에셋 1차 재제작 | 사용자 품질 수용 — Unity 반입으로 인계 | 반복 환경·투명 소품, 공통 캔버스·피벗의 쥐 측면 3프레임, 분리 HUD 모듈 20파일을 제작했다. HUD fill 문제를 수정했고 비주얼 PASS, QA `128/128`, 재생성 `20/20`, 총괄 내부 승인을 통과한 뒤 사용자가 Unity 반입을 승인했다. | `_workspace/active/2026-07-30-rat-host-2d-production-assets-v1/` |
| 쥐 숙주 2D 품질 우선 고품질 수직 샘플 | 사용자 품질 수용 — 실제 에셋 재제작으로 인계 | 환경·자연형 쥐 측면 보행·HUD 고품질 제작 마스터 3종을 만들었다. 쥐 프레임 체형 불일치를 한 차례 반려·재생성했고 최종 비주얼 PASS, 원본 추적성 QA와 총괄 내부 승인 뒤 사용자가 품질을 수용했다. | `_workspace/active/2026-07-30-rat-host-2d-quality-first-vertical-slice/` |
| 쥐 숙주 2D 실제 아트 제작 순서·기간 로드맵 | 고품질 대표 샘플 재제작 기준 보완 필요 | 기존 빠른 트랙과 통합 기준은 유지하되, 실제 아트 단계에서는 목표 목업에 가까운 묘사 밀도와 수작업 도트 페인트오버·정리를 필수 게이트로 추가해야 한다. | `_workspace/active/2026-07-29-rat-host-2d-art-production-roadmap/` |
| 3단계 2D 변이 선택·효과·쥐 숙주 복귀 | `8285bb0` 원격 보존·내부 승인 가능 — 사용자 실제 입력·화면 수용 대기 | 세 변이 단일 선택·효과·Host 복귀, 전용 통로·HUD를 구현했다. 구현 측 `6/6`·`53/53`, 독립 원본 Play·Console·보호 diff와 상태판 운영 게이트를 통과했다. 독립 전체 EditMode는 MCP TestRunner 오류로 미확인이다. | `_workspace/active/2026-07-29-rat-host-2d-stage3-mutation-return/` |
| 2단계 2D 백혈구 회피 미니게임과 성공·실패 인계 | `8285bb0` 원격 보존·사용자 부분 수용 — 검은 화면 해소·실제 이동 확인 | 원본 씬의 Floor `117`, Water `5`, Blocking `40` 셀과 13×9 맵 표시를 복구했다. 독립 QA 기술 게이트와 총괄 검토를 통과했고, 사용자가 화면 표시와 이동을 확인했다. Space 실패 확인과 내부 화면 체감은 남아 있다. | `_workspace/active/2026-07-28-rat-host-2d-stage2-minigame/` |
| 1단계 2D 쥐 숙주·면역 경계도·자연 100% 전환 통합 | 원본 Reload·MCP 차단 해소 — Stage2 사용자 확인에 통합 | Stage2 원본 씬 복구와 QA가 과거 Reload 차단을 해소했다. 별도 완료 보관은 Stage2 사용자 실제 WASD/Space 확인 뒤 함께 정리한다. | `_workspace/active/2026-07-28-rat-host-2d-stage1-integration/` |
| 자연 경계도 100% Windows 빌드 성공 루프 엄격 검증 | 차단 — Computer Use 게임 창 캡처 오류 | `list_apps`와 새 빌드 실행·단일 창 식별은 복구됐지만 `get_window_state`가 `SetIsBorderRequired 0x80004002`로 최초·복구 1회 모두 실패했다. 화면 미확인 입력을 보내지 않아 자연 100% 이후 루프는 미검증이며, 실패 시도 로그와 정상 종료만 보존했다. | `_workspace/active/2026-07-16-natural-alert-build-loop-verification/` |

## 최근 작업 요약

| 작업 | 상태 | 핵심 결과 | 확인 위치 |
| --- | --- | --- | --- |
| Production2D 자연 부분 가림·실제 충돌 루트 교정 | 완료 보관 — 사용자 수용 | final `5cd81d7c…`, 전체 EditMode `203/203`, QA Play r3 PASS, 총괄 내부 승인 뒤 사용자가 자연 부분 가림 화면과 쥐 본체 보존을 수용한 내용임을 재확인했다. | `_workspace/completed/2026-08-05-2026-08-02-production2d-natural-occlusion-root-fix/` |
| 검증 반복 체감과 사용자 보고 소음 축소 | 완료 보관 — 운영 적용 | preflight·S0를 실제 run/QA 실행과 구분하고, 구현 최초1+correction1·QA green 뒤1+재진입1 상한, 두 번째 실패 중지·보고, 최종 상태-only sync 재QA 금지, key transition 보고 계약을 반영했다. 독립 정적 QA 1회·총괄 1회, Unity/MCP/build 0이다. | `_workspace/completed/2026-08-05-2026-08-05-verification-loop-noise-reduction/` |
| 쥐 대각선 충돌 표면 미끄러짐 교정 | 완료 보관 — 사용자 수용 | frozen fingerprint `2286f...67414`에서 구현자 run007과 독립 QA qa-001이 각각 16/16 PASS했다. 사용자가 실제 WASD 표면 slide를 수용했고, 재발 방지 계약·closeout QA·총괄 `완료 보관 가능` 판정을 반영했다. 비용은 `주의`다. | `_workspace/completed/2026-08-05-2026-08-05-rat-collision-surface-slide/` |
| 루프 엔지니어링·검증 하네스 비용 효율 감사 | 완료 보관·운영 `533152e` push 완료 | QA r6 PASS와 총괄 내부 승인을 통과했다. 비용은 `과다 — 부분 회피 가능`, 정확 token/금액은 `미집계`; Unity/MCP/빌드는 0회다. 운영 문서·도구 커밋 `533152e`는 origin/main에 반영됐고 중앙 현황판 지속 관리를 시작한다. | `_workspace/completed/2026-08-02-2026-08-02-loop-harness-efficiency-audit/` |
| 쥐 숙주 2D 통합 제작 기준 이미지 | 완료 보관·사용자 수용 | 환경·쥐 V2·HUD 통합 기준 3개를 제작했다. 쥐 V2는 뒤쪽 체형을 교정하고 비주얼·QA·총괄 재검토를 통과했으며, 사용자가 세 이미지를 모두 수용했다. 실제 에셋·8방향·Unity 반입은 후속 승인이다. | `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-integrated-art-targets/` |
| 쥐 숙주 2D 첫 아트 샘플 후보 | 완료 보관·조합 선별 | 환경·소품 2안, 쥐 대표 3방향 2안, HUD 2안 총 6개를 생성·검토했고, 사용자 선별 조합을 후속 통합 기준에 반영했다. | `_workspace/completed/2026-07-29-2026-07-29-rat-host-2d-first-art-sample/` |
| 쥐 숙주 핵심 루프 단계적 2D 이관 승인 브리프 | 완료 보관 | 사용자 전체 추천안 승인으로 3단계 이관, 별도 2D 프로토타입 씬, 상태 재사용·2D 어댑터, 오염 노출 자연 100%, 신호 억제 보류, 레거시 보존과 1단계 착수를 확정했다. | `_workspace/completed/2026-07-28-2026-07-27-rat-core-loop-2d-migration-brief/` |
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

`오프닝 시네마틱·세균 감염 접속형 첫 튜토리얼 설계`의 Task 4 상세 숏 명세(33개 편집 비트 후보)는 canonical QA C1~C14와 총괄 최종 감사를 통과해 `c50f71a`로 origin/main에 반영 완료됐다. 다음 사용자 결정은 Task 5에서 각 편집 비트를 유지·병합·분할할지 정하는 것이다. 총 러닝타임은 실제 연결 타임라인 뒤 산정하며, 실제 스토리보드·카메라·레이어·이미지·에셋·오디오·UI·저장·코드·Unity는 사용자 승인 전 시작하지 않는다. `PC 시작 화면·설정 UI와 다국어 준비 구조`는 Play 진입·언어별 폰트 후보의 표적 UnityEditMode `38/38`와 독립 QA를 통과했으며, 다음은 재실행이 아니라 사용자 Startup 첫 프레임·한영 폰트 전환·설정 가독성·2D 진입 수용 확인이다. 대각선 충돌 표면 slide와 자연 부분 가림·쥐 본체 보존은 완료 상태를 유지하고 재검증하지 않는다.

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

1. 완료된 대각선 충돌 표면 slide와 자연 부분 가림·쥐 본체 보존은 재검증하지 않는다.
2. 새 작업 후보는 아직 확정하지 않는다.
3. 남아 있는 active 항목의 실제 미충족 게이트를 대조한 뒤 다음 작업을 사용자와 선택한다.
