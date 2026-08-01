# 작업 로그

## 2026-08-02 접수

- 사용자가 `docs/references/images/image.png`에서 쥐와 중앙 벽이 겹쳐 보이는 문제를 지적했고 완전 교정을 요청했다.
- 이전 작업은 tieBreak 통일과 바닥 footprint 확대를 검증했지만, 실제 가시 실루엣 기준 관통을 제거하지 못했으므로 사용자 수용 기준에서 실패로 재판정했다.
- 새 작업 패킷을 만들고 Unity 씬/통합 구현 → 독립 QA → 총괄 검토 순서로 진행한다.
- 기존 로컬 Stage2·Stage3·ProjectSettings·Physics2DSettings·사용자 reference 변경은 보호한다.

## 2026-08-02 총괄 1차 범위 보완

- 작업 자체는 승인 범위지만 자연 가림과 불가능한 관통의 정량 구분이 부족해 총괄이 `수정 필요`로 판정했다.
- 머리·몸통·발 solid 교차 0, 꼬리 허용/금지 규칙, 최소 1 logical pixel 여유, 벽·통·상자 8경로×3프레임×접촉 단계 검증 매트릭스를 추가했다.
- 런타임 정렬 코드 변경은 게임플레이 구현 에이전트 조건부 담당으로 분리했다.

## 2026-08-02 Unity 씬/통합 구현

- PNG alpha를 직접 측정해 쥐 3프레임, 중앙 벽, 통, 상자의 visible/core bounds를 world 단위로 환산했다.
- 기존 capsule이 머리·몸통 core 오른쪽 약 `0.382 world`를 보호하지 못하고 flip에도 offset이 고정되는 문제를 확인했다.
- collider-only 확대를 피하고 다음 두 계약으로 분리했다.
  - 방향-aware core capsule: `1.28 x 0.26`, offset X `-0.30 / +0.30`
  - 뒤쪽에서 4px 이상 두 조각이 되는 경우에만 whole-character occlusion, 해제 hysteresis `2/128 world`
- `YSortSprite2D`는 수정하지 않았고 새 `VisualOcclusionResolver2D`가 시각 가림만 담당한다.
- 최신 builder로 `RatHost2DTechnicalSample.unity`를 rebuild/save했다.
- 최초 관련 테스트는 컴파일 중 이전 builder assembly로 씬이 저장되어 `5/8`이었다. 컴파일 종료 후 최신 builder로 재생성해 `8/8`로 통과했다.
- 이후 TechnicalSample2D `46/46`, 전체 EditMode `200/200`을 통과했다.
- MCP Play 직접 상태 검증에서 벽·통·상자 뒤 분리 위험 전체 숨김, 벽 앞 전체 표시, 300회 정지 전환 0회, 2px hysteresis와 좌우 collider offset을 확인했다.
- Play 캡처 2장을 `artifacts/`에 저장하고 육안 확인했다.
- 마지막 임시 배열 할당 제거 후 TechnicalSample2D `46/46`, Console 0, scene dirty false를 다시 확인했다.

## 2026-08-02 독립 QA·총괄 종결 게이트

- 독립 QA가 씬 직렬화 renderer 복원, 통 수평 hysteresis, 벽 기존 해제 계약의 세 차단 결함을 순차 발견했고 런타임 담당이 각각 최소 보정했다.
- 최종 전체 EditMode `202/202`, TechnicalSample2D `48/48`, 관련 `10/10`을 통과했다.
- 접촉 `72/72`, 6단계 `432/432`, 물리 overlap·hidden/renderer 불일치 `0`을 확인했다.
- 벽·통·상자 hysteresis `0.015625 world`, 정지 300회와 ±0.001 subpixel 10주기 불필요 전환 `0`을 확인했다.
- 기존 final 캡처 4장은 임시 QA 객체가 섞인 불일치 증거로 제외하고, 실제 RatHost2D 한 개와 상태가 동기화된 final-v2 4장을 재생성·육안 대조했다.
- 최종 Console Error/Warning `0`, Play 종료, scene dirty `false`, QA 임시 객체 없음, `git diff --check` PASS다.
- 독립 QA `PASS — 총괄 내부 승인 검토 가능`, 총괄 `내부 승인 가능` 판정을 받았다.
- 실제 키보드 WASD와 통·상자 뒤 완전 가림의 위치 추적성은 사용자 확인 항목으로 남긴다.

## 2026-08-02 검증 증거 정본화·생성물 정리

- 사용자 커밋 요청에 따라 저장소 QA 정본을 `final-v2` XML·동기 캡처 4장과 `*-final.csv` 매트릭스로 고정했다.
- 임시 QA 객체가 섞여 PASS 증거에서 제외됐던 non-v2 PNG 4장, 결과 없는 최초 batch 로그, run2/post-fix/final 중간 로그·XML·CSV를 삭제했다.
- 구현자 사전 캡처 2장은 구현 보고 근거로 보존했다. `qa-editmode-unity-final-v2.log`는 원본 whitespace를 가공하지 않은 로컬 감사 증거로 유지하고 artifact budget에 따라 Git에서는 제외한다.
- 삭제는 검증 결과를 바꾸지 않는 artifact 위생 정리다. 최종 `202/202`, `72/72`, `432/432`, final-v2 화면 판정과 사용자 실제 WASD 확인 대기 상태는 유지한다.
