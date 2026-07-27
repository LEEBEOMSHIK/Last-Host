# 프로젝트 총괄 관리자 내부 검토

## 검토 대상

- 작업 ID: `2026-07-27-2d-playable-technical-sample`
- 작업명: 실제 2D 플레이어블 기술 샘플
- 검토 역할: 프로젝트 총괄 관리자 에이전트
- 검토일: 2026-07-27 KST
- 검토 범위:
  - 작업 배정·수행 이력·설계·수용 기준·비주얼 규격·QA 기록
  - 실제 Git 상태와 신규 runtime/editor/tests/scene/art 산출물
  - 기존 3D 씬, 사용자 `APP_UI_EDITOR_ONLY`, Packages, ProjectSettings, BuildSettings, 저장소 `Builds/` 보호
  - EditMode, Unity MCP Play, Console, Windows 임시 빌드 근거
  - 기술 플레이스홀더와 최종 2D 규격·최종 아트의 경계

## 판정

**내부 승인 가능**

별도 2D 기술 샘플의 코드·씬·에셋, 독립 QA 근거와 사용자 실제 플레이 확인을 종합해 현재 선별 범위의 완료·보관·커밋·푸시를 내부 승인한다.

- 사용자가 수정된 실제 플레이에서 키보드 이동과 벽·수로·소품 비통과를 확인했다.
- 사용자가 확인 직후 `커밋 푸쉬`를 요청했으므로, 이는 현 기술 샘플의 사용자 수용과 보관 승인으로 해석한다.
- Windows 임시 빌드는 생성 성공했지만 EXE 별도 실행 플레이는 미검증이다. 이 사실을 잔여 위험으로 명시하면 이번 커밋을 차단하지 않는다.
- 이번 승인은 기술 샘플 구현과 시험 플레이스홀더의 현재 상태에 한정된다. 시험 규격의 프로젝트 공통 승격, 최종 쥐 외형·최종 아트 승인, 전체 핵심 루프 2D 이관 승인이 아니다.

사용자 제보 소품 통과 버그의 수정분도 최신 독립 QA와 사용자 실제 플레이 재확인을 모두 충족했다.

## 근거

### 범위·승인

- 사용자가 기존 3D 프로토타입과 분리된 2D 플레이어블 기술 샘플의 코드·씬·기술 플레이스홀더 생성과 검증을 승인했다.
- 구현은 작은 하수도 방, 쥐 이동, 충돌, 8방향 표시, 카메라, Y 정렬, HUD에 한정됐다.
- 면역 경계도, 내부 바이러스 미니게임, 변이, 숙주 체인, 인간·병원·백신·엔딩은 이관하거나 추가하지 않았다.
- 신규 패키지, Unity 버전, URP 변경 근거는 없으며 승인 범위 확대도 확인되지 않았다.

### 산출물·메타데이터

- 신규 전용 경로가 분리돼 있다.
  - `Assets/_Project/Scripts/TechnicalSample2D/**`
  - `Assets/_Project/Editor/TechnicalSample2D/**`
  - `Assets/_Project/Tests/EditMode/TechnicalSample2D/**`
  - `Assets/_Project/Art/TechnicalSample2D/**`
  - `Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- 신규 산출물 대조 결과 파일-메타 누락 `0`, 고아 메타 `0`이다.
- 신규 네 전용 디렉터리 안의 실파일 `45`, 메타 `47`이며 디렉터리 메타를 포함한다.
- 전체 `Assets` 메타 `426`개의 GUID 중 중복 `0`이다.
- 쥐 기술 플레이스홀더는 `64×64` 16장, 타일은 `64×32` 3장이고 PPU `64`, Point 필터, no mipmap, 쥐 pivot Y `0.1875`가 메타에 기록돼 있다.
- 씬에는 별도 `TechnicalSample2D` 루트, Grid, Floor/Water/Blocking Tilemap, TilemapCollider2D, Dynamic Rigidbody2D와 CapsuleCollider2D 쥐, Visual·FootPoint, 직교 `4.21875` 카메라, 필수 HUD가 존재한다.
- 아키텍처의 별도 `WallVisualTilemap + CompositeCollider2D` 대신 실제 구현은 `BlockingTilemap + TilemapCollider2D`로 단순화됐다. 사용자 요청의 `TilemapCollider2D 또는 CompositeCollider2D` 범위 안이며 별도 패키지나 전역 설정을 요구하지 않는다.

### 기존 3D·사용자 변경 보호

- 기존 `RatHostPrototype.unity`의 Git blob SHA1은 HEAD와 실제 파일 모두 `3bc15837d24a32da2660576b10ec0baa4a20447a`로 일치한다.
- `manifest.json`, `packages-lock.json`, `EditorBuildSettings.asset`, 저장소 `Builds/`에는 변경이 없다.
- QA가 정리 대상으로 기록한 Physics2D·UnityConnect·URP 관련 5개 설정 파일은 HEAD diff `0`이다.
- `ProjectSettings/ProjectSettings.asset`의 유일한 diff는 기존 사용자 변경인
  `Standalone: SENTIS_ANALYTICS_ENABLED;APP_UI_EDITOR_ONLY`이며 보존돼 있다.
- `_workspace/previews/`는 기존 사용자 로컬 untracked 상태로 유지되며 이번 기술 샘플 산출물에 포함되지 않았다.

### Git 상태

- `HEAD = origin/main = f34ca43b3ba3b84612a5df20d9317d7e1261ef81`이다.
- 기술 샘플 runtime/editor/tests/scene/art와 작업 패킷은 현재 untracked이고, 상태판·`CURRENT.md`는 작업 진행에 따라 수정돼 있다.
- staged 변경은 없다.
- 사용자가 현재 기술 샘플의 커밋·푸시를 명시적으로 요청했다.
- 이번 `내부 승인 가능` 판정은 기술 샘플 전용 산출물과 완료·보관·상태 동기화 문서의 선별 커밋을 허용한다.
- 사용자 로컬 `ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY`, `_workspace/previews/`, 저장소 `Builds/`와 예상 밖 경로는 커밋 대상에서 제외해야 한다.

## 상태 문서 재대조와 최종 사용자 수용

- 기존 상태 문서는 QA `완료 가능`, 총괄 `조건부`, 사용자 수용 대기를 기록하고 있었다.
- 이후 사용자가 수정된 실제 플레이에서 키보드 이동과 벽·수로·소품 충돌을 확인했다.
- 사용자의 즉시 커밋·푸시 요청으로 사용자 수동 수용 대기 조건이 해소됐다.
- 완료·보관·커밋 전에 현황판, task, work-log, agent-activity, handoff를 QA `완료 가능`, 총괄 `내부 승인 가능`, 사용자 수용·보관 승인으로 동기화해야 한다.
- 이 상태 동기화는 새 기능이나 범위 변경이 아니라 커밋 전 기록 정합 작업이다.

## QA/검증 기록 확인

- 최신 독립 전체 EditMode: `139 passed / 0 failed / 0 skipped / 0 inconclusive`
- 최신 `LastHost.Prototype.TechnicalSample2D.Tests`: `38 passed / 0 failed / 0 skipped / 0 inconclusive`
- E01~E11: 모두 통과 기록
- Console: Error `0`, Warning `0`
- 실제 `Host/Move` InputAction 경로를 사용한 결정적 MCP 입력 대조:
  - W/S/A/D 및 W+D 이동량과 단일 스텝 기준 통과
  - 좌우·상하 반전 기준 통과
  - 300 fixed-step idle drift `0`
  - 카메라 축별 오차 `0.5 logical px` 이내
- Windows 임시 빌드:
  - `C:/tmp/LastHost2DTechnicalSample/20260727-153843/LastHost2DTechnicalSample.exe`
  - EXE와 Data 폴더 존재
  - 명시적 샘플 씬만 빌드
  - 저장소 `Builds/`와 `EditorBuildSettings` 비변경
- QA의 `완료 가능` 판정은 위 자동·MCP 수치·빌드 생성 범위에서는 근거가 있다.
- 사용자의 실제 키보드 플레이 확인이 MCP 대리 입력의 남은 수동 수용 공백을 해소했다.
- 다만 Windows EXE 실행 플레이 완료로 확대할 수는 없다.

## 사용자 제보 소품 통과 수정 재대조

- 원인 확인:
  - 기존 `Pipe_A`, `Barrel_A`는 `SpriteRenderer + YSortSprite2D`만 있고 `Collider2D`가 없어 실제 물리 장애물이 아니었다.
- 수정 구조:
  - 두 소품에 스프라이트 전체가 아닌 하단 footprint만 막는 정적 non-trigger `BoxCollider2D`를 추가했다.
  - `Pipe_A`: size `(0.27, 0.16)`, offset `(0, 0.02)`
  - `Barrel_A`: size `(0.31, 0.14)`, offset `(0, 0.02)`
  - 씬에는 해당 `BoxCollider2D`가 정확히 2개이고 둘 다 `YSortProps` 아래에 있다.
  - 결정적 씬 빌더도 같은 size·offset으로 두 Collider를 다시 생성한다.
- 테스트 변경:
  - 씬 계약에서 두 소품의 non-trigger 정적 footprint, SpriteRenderer, YSortSprite2D 연결을 확인한다.
  - Pipe/Barrel 각각에 대해 Rigidbody2D 이동 차단과 비관통 회귀 테스트를 추가했다.
- 실제 `Host/Move` Play 검증:
  - 두 소품 signed distance: 각각 `+0.000625`
  - 차단 뒤 추가 60 fixed-step 법선 진행: 각각 `0.000000`
  - 접촉 상태에서 소품 중심 통과: 각각 `false`
  - 직교 방향 우회: 각각 성공
  - 우회 중 Y-sort 관계 전환: 각각 정확히 `1회`
  - 최대 실제 root step: `0.060000`
  - 순간이동: `0회`
  - camera error 최대 축: `0.480 px`
- 판정:
  - 사용자가 제보한 “통·파이프를 그대로 통과함”의 기술 원인은 제거됐다.
  - 하단 footprint 방식은 앞뒤 Y 정렬을 유지하면서 정면 물리 통과를 차단한다.
  - 테스트와 MCP Play 수치상 수정분은 완료 가능하다.
  - 사용자가 수정된 실제 플레이에서 소품이 통과되지 않는 것을 확인했다.
  - footprint 크기의 최종 제품 규격 승격은 별도 아트·조작 수용 단계에서 판단한다.

## MCP 플레이 체크 확인

- 대상 씬 Play 진입, 필수 hierarchy·컴포넌트·초기 텔레메트리, 실제 InputAction 경로 입력, 충돌, 카메라 오차, idle, Console 확인 기록이 있다.
- Transform 직접 이동은 사용하지 않았다는 기록이 있다.
- MCP는 물리 키보드 장시간 홀드를 완전히 재현하지 못했으며 결정적 Input System 갱신 방식으로 대조했다.
- 패킷의 `qa-gameview.png`는 HUD와 월드가 함께 보이는 `1920×1080` 정지 화면 근거지만, runtime status가 `pending`인 정적 캡처라 Play 중 동적 HUD·조작감·960×540 수용을 증명하지는 않는다.
- 사용자가 이후 실제 키보드 플레이에서 이동과 벽·수로·소품 충돌을 확인했으므로, 이번 커밋 범위의 물리 키보드 조작·비통과 수용은 충족됐다.

## Windows 임시 빌드 확인

- 소품 collider 수정이 포함된 최신 빌드 `20260727-153843`의 생성 성공과 산출물 존재는 확인됐다.
- 빌드 메뉴는 `BuildPlayerOptions.scenes`에 샘플 씬을 직접 지정하고 `EditorBuildSettings`를 변경하지 않는다.
- 최신 EXE는 `667,648 bytes`, Data 폴더는 `287개 / 80,489,335 bytes`이며 BuildReport는 `Succeeded`다.
- QA는 최신 EXE를 별도 프로세스로 실행하지 않았다.
- 후속 Computer Use 실행 시도는 앱 실행 승인이 만료돼 창 생성·실제 키 입력·`Player.log` 확인 전에 종료됐다. 이는 빌드 실패가 아니라 실행 플레이 미검증 사유다.
- 따라서 판정은 `Windows 빌드 성공 / Windows 실행 플레이 미검증`으로 제한한다.
- 별도 씬의 Unity Play와 사용자 실제 키보드 수용, 전체 회귀, 최신 Windows 빌드 성공이 이미 확보됐으므로 EXE 실행 미검증은 이번 선별 커밋의 차단 조건이 아니라 후속 실행 환경 위험이다.

## signedDistance 평가

- 수로와 외벽 접촉에서 각각 `signedDistance=-0.000177`, `isOverlapped=true`가 기록됐다.
- 승인된 허용오차는 `signed distance ≥ -0.001`이므로 수치 계약에는 통과한다.
- 경계에서 이동이 멈췄고 눈에 띄는 관통 진행이 없었다는 QA 기록도 있다.
- 다만 음수 접촉 자체는 남아 있으므로 최종 충돌 여유·시각 접지 규격으로 승격하지 않고 잔여 위험으로 유지한다. 사용자 플레이에서 벽·수로 안쪽으로 보이는 프레임이 있으면 후속 조정한다.
- 새 Pipe/Barrel footprint의 signed distance는 모두 `+0.000625`로 양수이며, 60 fixed-step 추가 입력에서도 법선 진행이 `0`이었다. 소품 통과 수정의 충돌 수치에는 음수 겹침 잔여 위험이 관찰되지 않았다.

## 기술 샘플·최종 아트 경계

- HUD에 `TECHNICAL PLACEHOLDER • NOT FINAL ART`가 명시돼 있다.
- 현재 타일·소품·16프레임 쥐는 충돌, 방향, 피벗, 픽셀 출력과 정렬을 검증하는 기술 플레이스홀더다.
- `960×540`, `64×32`, PPU `64`, 8방향×2프레임, `4.21875`는 사용자 수용 전 시험값이다.
- 목표 목업 또는 ChatGPT 이미지 후보를 실제 타일셋·최종 스프라이트로 사용하거나 승인한 근거는 없다.
- 이번 결과는 최종 2D 규격 확정, 최종 쥐 외형 승인, 전체 핵심 루프의 2D 이관을 의미하지 않는다.

## 커밋 전 확인

1. 현황판과 작업 패킷을 QA `완료 가능`, 총괄 `내부 승인 가능`, 사용자 수용·보관 승인으로 동기화한다.
2. 기술 샘플 전용 코드·씬·아트·테스트·작업 기록만 선별한다.
3. 기존 사용자 `APP_UI_EDITOR_ONLY`, `_workspace/previews/`, 저장소 `Builds/`와 예상 밖 파일은 커밋에서 제외한다.
4. 커밋·푸시 보고에는 `Windows 빌드 성공 / Windows 실행 플레이 미검증`을 잔여 위험으로 유지한다.
5. 시험 규격·기술 플레이스홀더를 최종 규격·최종 아트로 표현하지 않는다.

## 문제 사안

- 문제: Windows 빌드 생성은 성공했지만 실행본 플레이가 없다.
- 영향: Windows 플레이 통과는 주장할 수 없지만 현재 기술 샘플 커밋은 차단하지 않는다.
- 원인: Computer Use 앱 실행 승인 대기가 만료돼 실행 검증을 시작하지 못했다.
- 추천: 실행 플레이 미검증을 잔여 위험으로 유지하고 필요 시 후속 Windows 실행 검증으로 분리한다.

## 사용자 결정 필요

- 현재 기술 샘플의 완료·보관·커밋에는 추가 사용자 결정이 필요하지 않다.
- `960×540 / 64×32 / PPU64 / 8방향×2프레임`의 프로젝트 공통 규격 승격과 최종 쥐·환경·HUD 아트 승인은 별도 후속 결정으로 남는다.

## 사용자에게 올릴 확인 파일

- `artifacts/qa-gameview.png`
  - 기술 플레이스홀더임을 전제로 HUD 배치, 쥐 크기, 바닥·벽·수로·소품 가독성을 확인한다.

## 다음 단계

1. 커밋 전 상태 문서와 완료 보관 경로를 동기화한다.
2. 승인된 기술 샘플 파일만 선별 커밋하고 origin에 푸시한다.
3. 커밋 보고에 Windows 실행 미검증과 시험 규격·최종 아트 경계를 남긴다.
4. 향후 규격 승격 또는 최종 아트 제작은 별도 사용자 승인 작업으로 연다.
