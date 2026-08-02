# 검증 기록

## 작업 ID

`2026-07-29-rat-host-2d-stage3-mutation-return`

## 검증 대상

Stage3 세 변이 선택·효과·2D 쥐 숙주 성공 복귀

## QA 판정

`조건부 통과 — 원본 씬 Stage3 런타임 기술 게이트 통과, 독립 전체 EditMode 재실행과 실제 물리 입력·화면 수용 확인 대기`

2026-07-29 원본 `RatHost2DPrototype` MCP Play에서 세 변이를 각각 새
Play 세션으로 검증했다.

- 잠복 강화: 버튼 선택, `25 + 8 = 33%` 성공 복귀, 오염 면역 증가
  `20 × 0.55 = 11`, 체력 피해 `10` 유지
- 신경 조종: 숫자 2 입력 처리 공개 경로, 조종력 `1.1`, 속도 배율
  `1.35`, 실제 Physics2D 1 step 우측 `0.081` 이동
- 포유류 적응: 지정 gate collider만 해제, Blocking/Water Tilemap과
  Barrel/Pipe 등 다른 충돌 유지
- 실패 회귀: 3회 피격 후 `VirusFailed`, 변이 없이 `60%` 복귀,
  재진입 안정성 `100`, 조각 `0/3` 초기화

선택 뒤 Host root·HUD·카메라·collider는 활성, 내부 root·카메라·
collider와 선택 UI는 비활성으로 복귀했다. 최종 Console은 Error `0`,
Warning `0`이며 Play 종료 뒤 원본 씬은 `dirty=false`다.

구현 담당 실행 증거는 신규 Stage3 EditMode `6/6`, RatHost2D 전체
`53/53` PASS다. 독립 QA의 전체 EditMode 재실행은 Unity MCP 테스트
콜백 실행 도구가 실패해 결과를 확정하지 못했다. 첫 요청은 MCP 자동
코드 보정 중 QA 콜백 클래스 중복으로 `CS1527`, 재요청은
`UNEXPECTED_ERROR: No logs available`였으며 제품 코드 컴파일 오류는
아니다. 추가 반복은 중단했다.

MCP에서 실제 OS 키다운을 주입하지 않았으므로 숫자키 처리는
`PrototypeInputState`, 버튼은 실제 UI 어댑터의 `SelectMutation()` 공개
경로로 대체했다. 실제 `1/2/3` 키·마우스 클릭과 화면 가독성은 사용자가
Game View에서 최종 확인해야 한다.

Windows 빌드는 실행하지 않았다. 상세 증거는
`artifacts/qa-verification.md`에 기록한다.

## 보호 경계

- `ProjectSettings.asset`: 기존 사용자 변경
  `SENTIS_ANALYTICS_ENABLED;APP_UI_EDITOR_ONLY` 한 줄만 유지
- Packages tracked diff 없음
- `_workspace/previews/` untracked 상태 보존
- 기존 3D 씬, `RatHost2DTechnicalSample`, 입력 asset 미수정

## 남은 위험

- 독립 전체 EditMode 재실행 결과는 MCP 도구 실패로 미확인이다.
- 실제 물리 키보드·마우스 입력과 UI 가독성은 사용자 수동 확인이 필요하다.
- 현재 UI, gate, 캐릭터와 맵은 기술 플레이스홀더이며 최종 아트 수용을
  의미하지 않는다.

## 후속 운영 게이트 대조

2026-07-29 상태판·세션 포인터·실제 경로·Git을 읽기 전용으로 대조했다.
최초 확인에서 완료 보관된 정합성 작업과 같은 이름의 빈 active 폴더
`_workspace/active/2026-07-16-current-task-board-consistency/`가 발견됐다.
메인 조정자가 이 빈 폴더를 제거한 뒤 재검증했다.

최종 결과:

- 상태판 active 참조 4개와 실제 active 디렉터리 4개 일치
- 상태판의 모든 active/completed 참조 경로 존재
- active/completed 동일 작업 중복 없음
- Stage3는 구현·원본 Play QA 완료, 독립 전체 EditMode TestRunner
  미확인, 사용자 실제 `1/2/3`·버튼·가독성 대기로 정확히 표시
- Stage2 실제 Space 키 수신과 내부 화면 체감 미확인 유지
- 사용자 수동 플레이 보류 항목은 한 곳에만 있고 다음 후보와 중복 없음
- 자연 경계도 엄격 검증은 active 차단 상태로 유지
- `CURRENT.md`는 Stage3를 현재 포인터로 가리킴
- 로컬 `HEAD`, 추적 `origin/main`, 실제 원격 `refs/heads/main` 모두
  `73c575058ee73a9c4ae926d42ae77480a82e5604`

후속 운영 상태판 게이트 판정은 `통과`다. 독립 전체 EditMode와 실제
물리 입력·가독성 미확인은 그대로 유지한다.
