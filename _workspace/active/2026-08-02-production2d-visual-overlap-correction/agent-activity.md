# 에이전트 활동

| 에이전트 | 역할 | 담당 업무 | 산출물 | 판정 |
| --- | --- | --- | --- | --- |
| 메인 조정자 | 작업 접수·배정 | 사용자 화면 확인, 작업 패킷과 보호 범위 작성 | `task.md`, 상태판 | 진행 중 |
| 비주얼/테크아트 에이전트 | 시각 기준 | alpha body·접지·꼬리·solid footprint와 최소 clearance 분석 | `artifacts/visual-occlusion-contract.md` | 정량 기준 완료 |
| Unity 씬/통합 구현 에이전트 | 실제 구현 | alpha bounds 재현, 방향-aware core collider, whole-character occlusion, 씬 rebuild, 테스트·Play 사전 검증 | 구현 diff, `artifacts/implementation-report.md`, Play 캡처 2장 | 구현자 검증 완료, 독립 QA 대기 |
| 게임플레이 구현 에이전트 | 런타임 공동 구현·리뷰 | `RatSide3FrameView`와 신규 resolver의 한쪽 꼬리·다중 occluder·renderer 상태·할당 검토 및 최소 보정 | `VisualOcclusionResolver2D.cs`, 관련 EditMode 테스트, `artifacts/runtime-code-review.md` | 코드 결함 보정, 작은 소품 whole-hide 추적성 UX 위험과 함께 QA 인계 |
| QA/검증 에이전트 | 독립 검증 | 최종 EditMode, 72 contact·432 phase, hysteresis·jitter, 동기화된 Play 화면, Console·scene clean·보호 diff 감사 | `verification.md`, `artifacts/qa-final-report.md`, `artifacts/qa-visual-captures-final-v2.csv`, final-v2 캡처 4장 | `PASS — 총괄 내부 승인 검토 가능` |
| 프로젝트 총괄 관리자 에이전트 | 내부 승인 | 범위·보호 변경·QA 증거·캡처 정정·사용자 재확인 경계 판정 | `director-review.md` | `내부 승인 가능` — 사용자 실제 WASD·작은 소품 완전 가림 수용 전 완료 미선언 |

## 총괄 1차 범위 게이트

- 최초 판정: `수정 필요` — 머리·몸통·발·꼬리 규칙, 역할 경계, QA 매트릭스, Git 기준 보강 필요.
- 메인 조정자가 `task.md`, 실행 순서, 현황판 Git 기준을 보강했다.

## Unity 씬/통합 구현 기록

- 변경 주체: Unity 씬/통합 구현 에이전트
- 신규 기술 샘플 보조 컴포넌트: `VisualOcclusionResolver2D`
- 변경하지 않은 런타임 정렬: `YSortSprite2D`
- 구현자 검증: 관련 `8/8`, TechnicalSample2D `46/46`, 전체 EditMode `200/200`, MCP Play 계약 PASS, Console 0, scene clean
- 완료 주장은 하지 않으며 독립 QA와 총괄 판정을 요청한다.

## 게임플레이 구현 에이전트 런타임 리뷰

- 양쪽 fragment만 검사해 한쪽 독립 꼬리 끝을 놓치는 결함을 확인하고 좌우 flip tail-only 계약을 추가했다.
- 2px hysteresis를 실제 활성 오클루더에만 적용해 다중 오클루더 상태 누수를 막았다.
- renderer를 resolver가 숨기기 전 상태로만 복원해 외부 비활성 상태를 보존했다.
- `RatSide3FrameView`의 프레임·flip·좌우 collider offset은 논리 루트를 이동시키지 않고 매 프레임 동기화되므로 추가 변경하지 않았다.
- Unity ValidateScript 오류 0, `git diff --check` 통과. EditMode·Play는 독립 QA 재실행 대기다.
- 통·상자 높이 `0.836 world`가 쥐 높이 `0.586 world`보다 커 뒤쪽 whole-character hide는 완전 가림 계약에 부합한다. 작은 소품 뒤 정지 시 위치 추적성은 blocker가 아닌 사용자 UX 확인 위험으로 인계한다.

## 독립 QA 실패 후 런타임 재보정

- QA `200 pass / 1 fail`의 renderer 해제 실패를 분석해, 씬에 저장된 `enabled=false`와 비직렬화 resolver 소유 상태가 분리되는 원인을 확인했다.
- `VisualOcclusionResolver2D`에 역직렬화 첫 판정 정상화를 추가하고, 명시적 `Configure` 경로의 외부 disabled 보존 계약은 유지했다.
- 실제 Play 상태 smoke에서 hide/hold/release와 external-disabled 보존은 모두 PASS였다.
- 실행 당시 Unity가 QA의 Play+Paused 상태였으므로 EditMode 재실행은 하지 않았고, QA 상태를 임의 종료하지 않은 채 재검증을 인계한다.

## 독립 QA 2차 jitter 실패 후 런타임 재보정

- 통 실측 release-entry `0.001 world`, 0.002 폭 10회 왕복 전환 20회를 수평 core 교차 히스테리시스 누락으로 판정했다.
- 최초에는 활성 오클루더 release bounds를 X/Y 양축 확장했으나, 3차 QA에서 wall `+0.37` fragment hold를 깨는 것으로 확인됐다.
- 수평 core 경계의 entry false / release 유지 true / 2px 이후 release false를 검사하는 EditMode 테스트를 추가했다.
- wall·crate·entry·외부 disabled·다중 occluder·tail-only 계약은 변경하지 않았다.
- `git diff --check` PASS. Unity 조작은 QA Play와 충돌 방지를 위해 수행하지 않았으며 독립 QA에 인계한다.

## 독립 QA 3차 wall 회귀 후 단일 release 방식 교정

- 전체 `201/202`, 관련 `9/10`에서 wall `+0.37` hold 실패를 확인했다.
- 오클루더 X 확장이 fragment 폭까지 줄이는 원인이므로 철회하고, visible/occluder bounds는 고정한 채 활성 `characterCoreBounds`만 X/Y 2px 확장하도록 교정했다.
- fragment 임계는 기존 `4px → 2px release`를 유지해 wall `+0.37/+0.38` 계약과 통 수평 core 보호를 분리했다.
- 통 수평 core 3표본과 3px fragment의 entry false/release true 정적 회귀를 테스트에 추가했다.
- 기존 entry·다중 occluder·tail-only·외부 disabled·직렬화 복원 경로는 변경하지 않았다. Unity 조작 없이 diff-check만 통과시키고 QA에 인계한다.

## 독립 QA 최종 판정

- 최종 XML을 다시 파싱해 전체 `202/202`, TechnicalSample2D `48/48`, 관련 `10/10`과 실패·skip·inconclusive 0을 확인했다.
- 접촉 `72/72`, 접촉 단계 `432/432`, 대상별 정지 `300`회, 2px hysteresis, subpixel ±0.001 왕복, 다중 오클루더·외부 disabled·좌우 tail-only 계약을 감사했다.
- 기존 final PNG 4장은 임시 QA 객체가 남아 CSV Root와 불일치하므로 증거에서 제외했다.
- 실제 RatHost2D 하나를 Root 좌표별로 배치하고 resolver·camera·HUD를 같은 호출에서 갱신한 final-v2 PNG 4장을 재생성했다. 벽·통·상자 뒤는 쥐가 완전히 가려지고 벽 앞은 전체 쥐가 표시됨을 직접 확인했다.
- 최종 Console Error/Warning 0, Play 종료, scene dirty false, 기준 씬 경로와 보호 diff를 확인했다.
- 기술 판정은 `PASS — 총괄 내부 승인 검토 가능`; 실제 WASD와 작은 소품 뒤 위치 추적성은 사용자 확인으로 남긴다.

## 검증 증거 정본화·정리 기록

- 문서/릴리즈 담당이 사용자 커밋 요청 범위에서 superseded raw evidence 17개를 정리했다. Python 생성 캐시 1개는 별도 저장소 위생 항목으로 삭제했다.
- 저장소 정본은 `qa-editmode-results-final-v2.xml`, `qa-*-final.csv`, `qa-visual-captures-final-v2.csv`, final-v2 PNG 4장이다. `qa-editmode-unity-final-v2.log`는 원본 상태의 로컬 감사 증거로만 보존한다.
- 임시 객체가 섞인 non-v2 PNG와 no-result·중간 세대 로그/XML/CSV는 기존 `verification.md`와 `qa-final-report.md`의 제외 판정에 따라 삭제했다.
- 구현 캡처·최종 QA 보고·활동/작업 이력은 보존했다. final-v2 raw log는 artifact budget과 whitespace 원본 보존 때문에 Git에서 제외했고, 새 QA·Unity·MCP·빌드는 실행하지 않았다.
