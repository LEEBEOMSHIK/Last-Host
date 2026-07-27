# 작업 로그

## 작업 ID

`2026-07-27-2d-playable-technical-sample`

## 2026-07-27 — 작업 시작

- 사용자가 현황판의 다음 후보인 실제 2D 플레이어블 기술 샘플 진행을 요청했다.
- Unity 수정·코드 작성·기술 플레이스홀더 생성 승인을 이번 요청으로 접수했다.
- Unity `6000.4.6f1`, URP, Input System, Tilemap/Physics2D 내장 모듈, Test Framework를 확인했다.
- 신규 패키지 없이 별도 씬으로 진행하고 기존 3D 씬을 보존한다.
- `960×540`, `64×32`, PPU `64`는 최종 확정이 아닌 시험값으로 둔다.

## 2026-07-27 — 설계·수용 기준 통합

- Unity 아키텍처: 신규 패키지와 ProjectSettings 변경 없이 새 runtime/editor/test 어셈블리와 별도 씬으로 격리한다.
- 게임플레이 수용 기준: 화면축 WASD, 대각선 정규화, idle 무이동, 무순간이동, Collider2D, 카메라 중심, Y 정렬, HUD를 수치화했다.
- 비주얼 규격: `960×540`, `64×32`, PPU `64`, 자연형 기술 쥐 `64×64`, 8방향×2프레임, Point/no mipmap을 시험값으로 정의했다.
- 통합 결정: Y 정렬은 `BaseOrder - RoundToInt(footWorldY * 100) + explicitTieBreak`로 통일한다.
- 통합 결정: Windows 빌드는 저장소 `Builds/`가 아니라 `C:/tmp/LastHost2DTechnicalSample/<run-id>/` 또는 OS temp에 출력한다.
- 다음 단계: 게임플레이 구현 에이전트가 새 전용 코드·테스트만 작성한다.

## 2026-07-27 — 게임플레이 구현·1차 회귀

- 전용 runtime/test asmdef와 입력, 이동, 8방향, 절대 픽셀 스냅, 즉시 카메라 추적, Y 정렬, HUD·텔레메트리 코드를 추가했다.
- 첫 코드 단위 EditMode 실행은 `32/34`였고 E08 벽·수로 충돌 두 케이스에서 Unity 기본 접촉 허용 폭만큼 `-0.005` 겹침을 확인했다.
- 수용 기준을 완화하지 않고 `Rigidbody2D.Cast`로 이동 전 최근접 충돌 거리를 검사해 `1/64` world unit 안전 폭을 남기도록 보정했다.
- 동일 코드 단위 EditMode 재실행은 `34 passed / 0 failed / 0 skipped / 0 inconclusive`였다.
- Unity Console Error/Warning `0/0`, `git diff --check` 통과를 확인했다.
- 씬 계약 E01/E11은 별도 통합 씬 생성 후 전체 테스트에서 검증한다.

## 2026-07-27 — Unity 씬 통합

- 전용 Editor asmdef와 `Last Host/Technical Sample 2D/Rebuild Scene` 결정적 빌더를 추가했다.
- `RatHost2DTechnicalSample.unity`에 Grid 1개, Floor/Water/Blocking Tilemap 3개, TilemapCollider2D 2개를 생성했다.
- 64×64/PPU64/Point/no mipmap/pivot Y `0.1875` 기술 쥐 8방향×2프레임, Y 정렬 소품 2개, 직교 카메라, HUD를 연결했다.
- E01 첫 실행에서 EditMode 재로드 직후 `PixelFollowCamera2D.TargetCamera` 캐시가 null인 문제를 확인했다.
- gameplay 구현 담당이 getter를 지연 조회로 1줄 보정했고 E01/E11 포함 전체 EditMode `36/36`을 통과했다.
- Windows 임시 빌드는 `C:/tmp/LastHost2DTechnicalSample/<yyyyMMdd-HHmmss>/`의 명시 씬만 대상으로 하며 EditorBuildSettings를 변경하지 않는다.
- Console 초기화 후 Error/Warning `0/0`을 확인했다.

## 2026-07-27 — 독립 QA·보호 설정 복구

- 독립 QA가 전체 EditMode `137/137`과 E01~E11 통과를 확인했다.
- 같은 `Host/Move` InputAction 경로의 MCP Play에서 W/A/S/D/대각선 이동량 `0.6000`, 최대 step `0.0600`, 반전 무순간이동, 카메라 축별 오차 `0.5px` 이내를 확인했다.
- 300 fixed-step idle drift는 `0.000000`이었다.
- 벽·수로 접촉은 signed distance `-0.000177`로 E08 허용치 `-0.001` 안에서 정지했으나 미세 음수 접촉은 잔여 위험으로 남겼다.
- Windows 임시 빌드는 `C:/tmp/LastHost2DTechnicalSample/20260727-085924/`에 성공했다.
- 빌드가 자동으로 남긴 Physics2D·UnityConnect·URP·preloaded/batching 설정 변경을 작업 시작 상태로 복구했다.
- 최종 설정 diff는 사용자의 기존 `APP_UI_EDITOR_ONLY` 한 줄만 남고 기존 3D 씬·Packages·EditorBuildSettings·저장소 Builds는 변경되지 않았다.
- QA 최종 판정은 `완료 가능`; Game View HUD·실제 물리 키보드 체감·placeholder 크기/가독성은 사용자 수동 수용으로 분리했다.

## 2026-07-27 — 총괄 검토·Windows 실행 시도

- 프로젝트 총괄 관리자는 기술 산출물과 QA 근거를 사용자 수용 단계로 올릴 수 있다고 보되 Windows 실행본과 사용자 Game View·키보드 수용이 남아 `조건부`로 판정했다.
- Computer Use 스킬로 임시 EXE 실행을 시도했으나 앱 실행 승인 대기가 만료되어 창·실제 키 입력·Player.log 확인은 수행하지 못했다.
- Windows `빌드 생성 성공`과 `실행 플레이 미검증` 경계를 유지한다.
- 상태 문서의 QA 진행·대기 표기를 최종 `완료 가능`과 총괄 `조건부`로 동기화했다.

## 2026-07-27 — 사용자 수용 중 소품 관통 발견

- 사용자가 벽과 초록 수로 충돌은 정상이나 단일 타일 통 형태 소품을 쥐가 관통한다고 확인했다.
- 코드 대조 결과 통·파이프는 `SpriteRenderer + YSortSprite2D`만 있고 `Collider2D`가 없어 실제 물리 장애물이 아니었다.
- 수정 기준: 소품 전체 스프라이트를 막지 않고 바닥에 닿는 하단 발자국만 정적 Collider2D로 막아 앞뒤 Y 정렬 경로를 유지한다.
- 두 소품 모두 관통 `0건`, signed distance `≥ -0.001`, 경계 정지, Y 정렬 유지, 전체 회귀 통과를 수용 기준으로 추가한다.
- 기존 총괄 `조건부` 판정은 버그 수정·재검증 전까지 다시 열린 상태로 본다.

## 2026-07-27 — 소품 충돌 수정·재검증

- `Pipe_A`에는 `0.27×0.16`, `Barrel_A`에는 `0.31×0.14`, offset `(0,0.02)`의 정적 non-trigger `BoxCollider2D` 발자국을 추가했다.
- 관련 씬 계약 4/4, TechnicalSample2D EditMode 38/38, 전체 EditMode 139/139를 통과했다.
- 독립 MCP Play의 실제 `Host/Move` 경로에서 두 소품 모두 signed distance `+0.000625`, 차단 후 추가 60-step 법선 진행 `0`, 중심 통과 `false`였다.
- 두 소품 모두 직교 방향으로 우회 가능했고 우회 중 Y 정렬 앞/뒤 전환은 정확히 1회, max jump `0.06`, camera max axis `0.48px`였다.
- 최신 Windows 임시 빌드 `C:/tmp/LastHost2DTechnicalSample/20260727-153843/` 생성에 성공했다.
- 빌드가 재생성한 Physics2D·UnityConnect·URP·preloaded/batching 자동 설정 diff를 정리해 사용자 `APP_UI_EDITOR_ONLY` 한 줄만 보존했다.
- 최종 QA는 자동 설정 정리 상태를 read-only로 재대조해 addendum `완료 가능`으로 승격했다.
- 프로젝트 총괄은 전체 기술 샘플은 Windows 실행본·사용자 수용이 남아 `조건부`를 유지하되, 이번 Pipe/Barrel 관통 수정분은 `내부 승인 가능`으로 판정했다.
- 다음 단계는 사용자가 수정된 씬에서 두 소품 차단·우회·가림을 다시 확인하는 것이다.

## 2026-07-27 — 사용자 최종 수용·커밋 승인

- 사용자가 수정된 실제 플레이에서 소품도 더 이상 통과되지 않는 것을 확인했다.
- 사용자는 현재 기술 샘플의 커밋·푸시를 명시적으로 요청했다.
- 총괄 관리자는 이동·벽·수로·소품 충돌의 사용자 실제 키보드 확인과 QA 근거를 합쳐 최종 `내부 승인 가능`으로 갱신했다.
- Windows 실행본은 `빌드 성공 / 실행 플레이 미검증` 잔여 위험으로 유지하되 현재 기술 샘플의 완료·보관·선별 커밋을 차단하지 않는다.
- `960×540`, `64×32`, PPU `64`, 기술 플레이스홀더는 최종 규격·최종 아트 승인으로 승격하지 않는다.

## 게이트 진행 상태

- 작업 배정 게이트: 충족
- 담당 산출물 게이트: 게임플레이·씬 통합 완료
- 에이전트 수행 이력 게이트: 구현·독립 QA·총괄 기록 완료
- QA/검증 게이트: 완료 가능 — 소품 충돌 addendum 포함
- 총괄 관리자 게이트: 내부 승인 가능
- 커밋 전 차단 조건: 검증·총괄 판정 전 커밋 금지
