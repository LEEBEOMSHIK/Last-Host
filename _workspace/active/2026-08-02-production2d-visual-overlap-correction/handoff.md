# 작업 인수인계

## 최신 사용자 요청

사용자 제공 화면에서 쥐와 물체가 겹치는 문제를 완벽하게 수정한다.

## 현재 상태

- 구현, 런타임 공동 리뷰, 독립 QA와 총괄 최종 검토가 끝났다.
- 독립 QA `PASS — 총괄 내부 승인 검토 가능`, 총괄 `내부 승인 가능`이다.
- 작업 완료 선언은 사용자 실제 WASD와 통·상자 뒤 완전 가림 수용 확인 뒤로 보류한다.
- 머리·몸통 core collider는 방향에 따라 `offset X -0.30 / +0.30`으로 동기화된다.
- 뒤쪽 접촉에서 쥐가 4px 이상 두 조각으로 갈라질 때만 `VisualOcclusionResolver2D`가 쥐 전체를 숨긴다.
- 한쪽에 core와 분리된 4px 이상 꼬리 끝만 남는 좌우 flip 상태도 전체 가림한다.
- 2px hysteresis는 기존 활성 오클루더에만 적용하며, resolver가 외부 renderer 비활성 상태를 강제 해제하지 않는다.
- 독립 QA `200 pass / 1 fail`에서 발견된 씬 직렬화 renderer 복원 결함을 추가 보정했다. 역직렬화 첫 판정만 stale disabled를 정상화하고 명시적 Configure의 외부 disabled는 보존한다.
- 독립 QA 2차 통 수평 해제 누락 뒤 적용한 오클루더 X/Y 확장은 3차 QA에서 wall `+0.37` hold를 깨서 철회했다. 현재는 visible/occluder bounds를 고정하고 활성 character core만 X/Y 2px 확장하며 fragment release는 기존 2px를 유지한다.
- 최종 전체 EditMode `202/202`, TechnicalSample2D `48/48`, 관련 `10/10`을 통과했다.
- 접촉 `72/72`, 단계 `432/432`, 물리 overlap·표시 불일치 `0`, hysteresis `0.015625 world`, 정지·subpixel 불필요 전환 `0`을 확인했다.
- final-v2 동기 캡처 4장, Console Error/Warning `0`, Play 종료, scene dirty `false`를 확인했다.
- 기준 화면은 `docs/references/images/image.png`이며 사용자 소유 파일은 변경하지 않았다.

## 보호 대상

- Stage2·Stage3·`RatHost2DPrototype` 변경
- `UnityProject/ProjectSettings/ProjectSettings.asset`
- `UnityProject/ProjectSettings/Physics2DSettings.asset`
- `_workspace/previews/`, `Builds/`, 반려된 시험 산출물, Python 캐시
- 사용자 소유 `docs/references/images/image.png`

## 구현 변경

- `RatHost2DProductionSampleSceneBuilder.cs`
- `RatHost2DTechnicalSample.unity`
- `RatSide3FrameView.cs`
- `VisualOcclusionResolver2D.cs(.meta)`
- `Production2DV1AssetAndSceneTests.cs`
- 세부 내용: `artifacts/implementation-report.md`
- 런타임 공동 리뷰·추가 보정: `artifacts/runtime-code-review.md`

## 마지막 성공 검증

- 관련 클래스 EditMode `10/10`
- TechnicalSample2D EditMode `48/48`
- 전체 EditMode `202/202`
- MCP Play: 접촉 `72/72`, 단계 `432/432`, 벽·통·상자 뒤 hidden, 벽 앞 visible, 300회 정지 및 subpixel transition 증가 없음
- Console Error/Warning `0`, Play 종료, scene dirty `false`, QA 임시 객체 없음

## 다음 작업

1. 사용자가 실제 WASD로 벽·통·상자 경계와 짧은 방향 반전을 확인한다.
2. 통·상자 뒤 완전 가림의 위치 추적성을 수용할지 결정한다.
3. 사용자 수용 뒤 완료·보관 또는 선별 커밋 대상으로 전환한다.

## 미검증·사용자 확인

- 구현자 검증은 MCP 직접 상태 전환이며 실제 키 입력 검증이 아니다.
- 작은 통·상자 뒤에서 전체 숨김이 자연스러운지는 사용자 플레이 확인이 필요하다.
- 통·상자 높이 약 `0.836 world`가 쥐 높이 최대 약 `0.586 world`보다 커 뒤쪽 완전 가림은 작업 계약상 허용된다. 다만 작은 소품 뒤 정지 때의 위치 추적성은 실제 사용자 WASD로 확인한다.
- 최종 QA에서 Play 종료·Console 초기화·scene clean을 확인했다.
- 기존 `qa-*-final.png` 4장은 임시 QA 객체가 섞인 불일치 증거라 제외했고, `final-v2` 4장과 동기 CSV만 유효 근거로 사용한다.
