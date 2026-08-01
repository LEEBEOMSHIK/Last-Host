# Production2D 시각 겹침 교정 독립 QA 최종 보고

## 판정

`PASS — 총괄 내부 승인 검토 가능`

판정 범위는 `RatHost2DTechnicalSample`의 Production2D V1 쥐 측면 3프레임과 `WallStraight_Occlusion`, `Barrel_A`, `Crate_A` 사이의 물리 접촉·앞뒤 정렬·whole-character occlusion 교정이다. 실제 사용자 WASD 조작감과 작은 소품 뒤 완전 가림의 위치 추적성은 사용자 수용 항목으로 남긴다.

## 자동 테스트 독립 감사

- 최종 XML: `qa-editmode-results-final-v2.xml`
- 전체 EditMode: `202/202` Passed, 실패·skip·inconclusive `0`
- `TechnicalSample2D`: `48/48` Passed
- 관련 `Production2DV1AssetAndSceneTests`: `10/10` Passed
- Unity 명령줄 종료 코드: `0`
- `qa-editmode-unity-final-v2.log`는 배치 렌더 환경의 `RenderTexture.Create failed` 계열 메시지를 포함한 로컬 감사 증거로 보존한다. artifact budget과 원본 whitespace 보존을 위해 Git에서는 제외하며, 저장소의 판정 근거는 `qa-editmode-results-final-v2.xml`, 아래 라이브 에디터 최종 기준, final-v2 PNG와 CSV다.

## 접촉·가림 매트릭스

- `qa-contact-matrix-final.csv`: 벽·통·상자 × 8방향 × 3프레임 = `72/72` 표본
  - 물리 overlap `0`
  - `hidden`과 `renderer_enabled` 불일치 `0`
- `qa-phase-matrix-final.csv`: 위 72표본 × 접촉 6단계 = `432/432` 표본
  - 물리 overlap `0`
  - `hidden`과 `renderer_enabled` 불일치 `0`
- `qa-stability-final.csv`
  - 대상별 정지 `300`회 동안 불필요 전환 `0`
  - release hysteresis: 벽·통·상자 모두 `0.015625 world = 2/128`
  - 10회 경계 왕복은 진입·해제 각 1회로 총 20회 전환
  - 다중 오클루더 누수 없음, 외부 renderer 비활성 보존, 좌·우 tail-only 계약 모두 PASS
- `qa-subpixel-jitter-final.csv`
  - 경계 안쪽 ±`0.001 world` 10주기에서 hidden/visible 어느 상태도 추가 전환 `0`
  - renderer 상태 불일치 `0`

## 동기화된 최종 화면 감사

기존 `qa-*-final.png` 4장은 `QA_Temp*` 런타임 객체와 실제 `RatHost2D`가 함께 남은 상태에서 예약 캡처돼, CSV 좌표와 화면 HUD `Root (-1.00,-0.25)`가 일치하지 않았다. 이 4장은 PASS 증거에서 제외하고 아래 `final-v2`만 유효 증거로 사용한다.

| 캡처 | 동기 상태 | 육안 판정 |
| --- | --- | --- |
| `qa-wall-behind-final-v2.png` | Root `(0.000000, 0.937813)`, hidden `true`, renderer `false`, Sort `-94/-74` | 쥐가 완전히 가려지고 분리된 몸·꼬리 조각 없음 |
| `qa-wall-front-final-v2.png` | Root `(0.000000, 0.462188)`, hidden `false`, renderer `true`, Sort `-46/-74` | 쥐 한 마리가 완전한 실루엣으로 벽 앞에 표시됨 |
| `qa-barrel-behind-final-v2.png` | Root `(-1.000000, -0.512188)`, hidden `true`, renderer `false`, Sort `51/76` | 쥐가 완전히 가려지고 분리 조각 없음 |
| `qa-crate-behind-final-v2.png` | Root `(2.000000, 0.507813)`, hidden `true`, renderer `false`, Sort `-51/-24` | 쥐가 완전히 가려지고 분리 조각 없음 |

네 캡처는 실제 `RatHost2D` 하나만 각 좌표에 배치하고 같은 호출에서 `ResolveNow`, 카메라 follow, HUD refresh를 적용한 뒤 즉시 1920×1080 렌더했다. 화면 HUD Root, resolver 상태, renderer 상태, sorting order와 `qa-visual-captures-final-v2.csv`가 일치한다. 모든 표본의 collider distance는 `0.007812..0.007813 world`, 물리 overlap은 `false`다.

## Unity MCP 최종 상태

- 대상 씬: `Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- Play 종료: `IsPlaying=false`, `IsPaused=false`
- 컴파일·업데이트: `false/false`
- scene dirty: `false`
- 씬 루트: 편집 모드 기준 `1`; `QA_Temp*` 런타임 객체 없음
- 최종 Console Error/Warning: `0`

## 보호 diff·운영 상태 감사

- 이번 QA는 코드·씬·ProjectSettings를 수정하지 않았다.
- `git diff --check` 통과. 출력은 줄바꿈 변환 경고뿐이며 whitespace 오류는 없다.
- Stage2·Stage3, `RatHost2DPrototype`, `ProjectSettings.asset`, `Physics2DSettings.asset`, `_workspace/previews/`, 반려 시험 산출물, Python 캐시, 사용자 `docs/references/images/image.png`는 기존 로컬 변경으로 식별했고 건드리지 않았다.
- `_workspace/active/CURRENT.md`와 `docs/project-handoff/current-task-board.md`는 아직 `구현 배정 중`으로 표시된다. QA 판정 뒤 메인 조정자가 `QA 완료 — 총괄 검토 대기`로 동기화해야 하며, 이 동기화 전에는 완료·커밋 보고를 하면 안 된다.

## 남은 위험과 사용자 확인

- 자동·MCP 검증은 실제 키보드 WASD 입력과 사용자의 조작 체감을 대체하지 않는다.
- 통·상자 뒤에서 쥐가 완전히 사라지는 것은 현재 오클루더 높이 계약에는 부합하지만, 위치 추적성이 불편한지는 사용자가 실제 WASD로 확인해야 한다.
- 현재 V1은 측면 3프레임 기술 샘플이다. 전체 8방향과 최종 오브젝트별 전후 레이어는 이번 범위가 아니다.

## 완료 판단

코드·씬 교정의 기술 게이트는 통과했다. 상태판 동기화와 프로젝트 총괄 관리자 `내부 승인 가능` 판정 후 사용자 실제 WASD 재확인 단계로 넘길 수 있다.
