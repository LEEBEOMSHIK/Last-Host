# Game 뷰 카메라 출력 복구 인계

## 최신 사용자 요청

Game 뷰에서도 카메라가 제대로 움직이도록 수정한다.

## 확인된 원인

- `IsometricCamera`는 활성화되어 있으며 정상 캡처된다.
- 카메라 출력 대상이 `RatPixelTrial960x540.renderTexture`뿐이라 Display 1에 직접 쓰는 카메라가 없다.
- `WorldPixelOutputPreview`의 RawImage는 RT를 참조하지만, 직접 Game 뷰 출력 카메라가 없어 Unity가 `No cameras rendering`을 표시한다.

## 변경 경계

- 유지: 3D 쿼터뷰, 쥐 8방향 스프라이트, 960×540 Point 월드 출력, HUD 별도 Overlay.
- 금지: 이동·충돌·면역·변이 변경, ProjectSettings·패키지 변경, v5b 공통 기준 승격.

## 구현 후 확인

1. Unity 씬/통합 완료: `GameViewFrameCamera`가 Display 1에 렌더 대상 없이 프레임만 제공한다. `IsometricCamera`는 960×540 Point RT를 계속 렌더하며 MainCamera 태그를 유지한다.
2. Unity MCP Play 자체 점검: Display 프레임 카메라·RT RawImage·HUD 연결 정상, 런타임 쥐 위치 이동에 대해 쿼터뷰 카메라 추적 확인, Console Error/Warning 0건.
3. QA가 Play Game 뷰에서 `No cameras rendering`이 사라지고 실제 월드·HUD가 보이는지, WASD 연속 이동·픽셀 보간/떨림을 독립 확인한다.

## 변경 경계 확인

- RenderTexture는 제거하지 않았고, ProjectSettings·패키지·이동·충돌·면역·변이와 v5b 공통 기준은 변경하지 않았다.
- 기존 dirty 씬을 저장·재로드하지 않았다. 커밋 전 QA와 총괄 판정이 필요하다.

## 저장 상태

- `RatHostPrototype.unity`만 저장 완료, active scene `isDirty=False`.
- ProjectSettings의 `APP_UI_EDITOR_ONLY` 자동 변경은 이 작업 범위 밖이다. 저장·수정·되돌림을 하지 않았으며 별도 정리 판정이 필요하다.
