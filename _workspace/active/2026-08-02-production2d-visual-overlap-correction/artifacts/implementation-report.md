# Unity 씬/통합 구현 보고

## 구현 상태

- 구현 주체: Unity 씬/통합 구현 에이전트
- 상태: 구현 및 구현자 검증 완료, 독립 QA 대기
- 대상 씬: `Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- 새 아트·PPU·패키지·ProjectSettings 변경: 없음

## 재현과 원인

- 쥐 3프레임의 `alpha >= 128` 가시 폭은 약 `237~238px = 1.852~1.859 world`다.
- 두께가 12px 이상인 머리·몸통 core의 보수 X 범위는 root 기준 약 `-0.328..+0.922 world`다.
- 기존 capsule X 범위는 size `0.92`, offset `+0.08`이므로 `-0.38..+0.54 world`뿐이었다. 오른쪽 머리·몸통이 최대 약 `0.382 world` 밖에 있었고 좌우 flip에도 offset이 고정됐다.
- 물리 footprint가 서로 겹치지 않더라도, 쥐가 오브젝트 뒤에 있을 때 좁은 벽·통·상자가 긴 단일 쥐 스프라이트의 중앙만 가려 양쪽에 4px 이상의 조각을 남길 수 있었다.
- 따라서 collider 확대만으로는 꼬리까지 포함한 과도한 간격이 생기며, 뒤쪽의 분리 실루엣도 완전히 해결하지 못한다.

## 구현

1. 머리·몸통 core 물리 정합
   - 쥐 capsule을 `1.28 x 0.26`, 오른쪽 기준 offset `(0.30, 0.13)`으로 조정했다.
   - `RatSide3FrameView`가 좌우 방향에 따라 X offset을 `-0.30 / +0.30`으로 동기화한다.
   - 전체 꼬리 폭은 collider에 넣지 않아 측면 접근의 과도한 빈 간격을 피했다.
2. 물리와 시각 가림 분리
   - `VisualOcclusionResolver2D`를 새로 추가했다.
   - 쥐 3프레임과 중앙 벽·통·상자의 투명 캔버스가 아닌 실제 alpha bounds 계약을 builder에서 직렬화한다.
   - 쥐가 오브젝트 뒤이고, 오클루더가 core와 교차하면서 좌우에 각각 4 logical px 이상의 가시 조각을 남길 때만 쥐 전체를 숨긴다.
   - 앞쪽 상태는 쥐 전체를 표시한다. `YSortSprite2D` 계산은 변경하지 않았다.
   - 해제 임계에는 `2/128 = 0.015625 world` hysteresis를 적용했다.
3. 씬·테스트
   - 최신 builder로 기술 샘플 씬을 rebuild/save했다.
   - 방향별 collider offset, 4px 진입/2px 해제, 300회 정지 유지, core 교차 조건과 씬 계약 테스트를 추가했다.

## 변경 파일

- `UnityProject/Assets/_Project/Editor/TechnicalSample2D/RatHost2DProductionSampleSceneBuilder.cs`
- `UnityProject/Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- `UnityProject/Assets/_Project/Scripts/TechnicalSample2D/RatSide3FrameView.cs`
- `UnityProject/Assets/_Project/Scripts/TechnicalSample2D/VisualOcclusionResolver2D.cs`
- `UnityProject/Assets/_Project/Scripts/TechnicalSample2D/VisualOcclusionResolver2D.cs.meta`
- `UnityProject/Assets/_Project/Tests/EditMode/TechnicalSample2D/Production2DV1AssetAndSceneTests.cs`

## 구현자 검증

- 관련 클래스 EditMode: `8/8 PASS`
- TechnicalSample2D 전체 EditMode: `46/46 PASS` (최종 코드 재컴파일 후 재검증)
- 전체 EditMode: `200/200 PASS` (임시 할당 제거 직전 기능 동일 상태)
- MCP Play 직접 상태 검증:
  - 중앙 벽 뒤 분리 위험: hidden `true`, rat order `-90`, wall order `-74`
  - 정지 300회: visibility transition `1 -> 1`
  - X `+0.37`: hysteresis 유지, X `+0.38`: 해제
  - 벽 앞: 전체 쥐 visible
  - 통·상자 뒤 분리 위험: 모두 hidden `true`
  - 좌/우 collider offset: `-0.30 / +0.30`
- Unity Console Error/Warning: `0`
- 활성 씬 dirty: `false`
- Editor: Play/Compile/Update 모두 종료 상태
- 보호 diff: Stage2·Stage3·`RatHost2DPrototype`·ProjectSettings의 기존 변경은 건드리지 않았다.

## 캡처

- `play-wall-behind-whole-occlusion.png`: 뒤쪽 분리 위험에서 전체 가림
- `play-wall-front-whole-character.png`: 앞쪽에서 전체 캐릭터 표시

## 남은 위험

- MCP 직접 위치 전환은 실제 WASD 입력과 사용자의 체감 확인을 대체하지 않는다.
- 벽·통·상자 8경로 × 3프레임 전체 매트릭스와 실제 접촉 경로는 독립 QA가 다시 확인해야 한다.
- 작은 소품 뒤에서 whole-character 숨김이 사용자에게 과도하게 느껴지는지는 최종 사용자 플레이 확인 대상이다.
- 향후 8방향 스프라이트와 전·후면 분리 아트가 도입되면 이 보조 가림 계약을 재평가해야 한다.
