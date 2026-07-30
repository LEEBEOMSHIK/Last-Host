# 작업 인수인계

## 최신 사용자 요청

벽·통 등의 오브젝트 통과가 부자연스럽고 HUD 쥐 초상 내부 상단에 불필요한 그래픽이 보이는 문제를 수정한다.

## 현재 상태

- HUD 제작 소스·Production2D 실제 에셋·Unity 반입본 동기화 완료. 세 파일 SHA-256은 `76BC4A430FC170C24C704CF54B2FAFC57EAED0CD2FE5DA5A0F52521F28371908`이다.
- 정적 occluder tieBreak를 `1`로 통일해 가림 전이점을 오브젝트 ground pivot으로 정규화했다.
- 쥐 collider 폭을 `0.92`, barrel을 `0.60`, crate를 `0.70`으로 조정했다. wall 폭과 높이·offset은 유지했다.
- 수정 전·후 각각 세 오브젝트×앞/동일/뒤의 CSV와 9장 캡처가 있다.
- 관련 EditMode `44/44`, 전체 EditMode `198/198`, MCP Play 충돌·정렬·HUD, Console `0`, scene dirty `false`까지 구현 자체 검증을 통과했다.
- 독립 QA clean clone에서 관련 EditMode `44/44`, 전체 EditMode `198/198`을 다시 통과했고 비주얼 postfix 검토도 PASS다.
- Stage2·Stage3, `RatHost2DPrototype`, ProjectSettings 보호 해시는 구현 전 기준과 동일하다.
- 고품질 2D 아트 체인과 Unity 반입·가림/HUD 수정은 `e7220a7 feat: integrate production 2d art sample`, 상태판은 `7adef75 docs: sync production 2d push state`로 선별 커밋해 `origin/main`에 푸시했다. Stage2·Stage3·ProjectSettings·반려된 규격 시험 산출물·캐시는 제외했다.

## 보호 대상

- `RatHost2DPrototype.unity`
- Stage2·Stage3 미커밋 변경
- `UnityProject/ProjectSettings/**`
- `_workspace/previews/`
- 저장소 `Builds/`

## 다음 작업

1. 사용자가 실제 Game View 포커스의 WASD로 wall·barrel·crate의 앞/옆/뒤 연속 통과와 충돌 체감을 확인한다.
2. 사용자가 HUD 초상 상단 잔여 조각 제거와 쥐 본체 보존을 최종 화면에서 확인한다.
3. 사용자 수용 뒤 PPU 128·전체 8방향·전체 아트 적용 범위를 결정한다.
