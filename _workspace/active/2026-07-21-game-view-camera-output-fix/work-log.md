# 작업 로그

## 2026-07-21 KST — 증상 확인

- 사용자는 Unity Game 뷰 `Display 1`에서 `No cameras rendering`과 카메라 추적 미표시를 확인했다.
- MCP 읽기 전용 점검에서 `IsometricCamera`는 활성 MainCamera·직교 카메라이고 카메라 캡처도 정상이다.
- 이 카메라는 `RatPixelTrial960x540.renderTexture`에 출력되며, `WorldPixelOutput` RawImage가 같은 텍스처를 참조한다. Display 1에 직접 렌더하는 카메라가 없어 Game 뷰 메시지가 나타나는 구조다.
- 수정 원칙: 저해상도 Point 월드 출력과 HUD 분리를 유지하면서 Display 1에 직접 프레임을 제공한다.

## 2026-07-21 KST — 통합·독립 QA·총괄 검토

- Unity 씬/통합 담당은 `GameViewFrameCamera`를 Display 1용 프레임 카메라로 추가했다. IsometricCamera는 기존대로 960×540 Point RenderTexture를 렌더하고, RawImage와 HUD가 화면을 구성한다.
- 독립 QA는 활성 Display 1 프레임 카메라, IsometricCamera RT, RawImage/HUD 계층, 1920×1080 Play, 쿼터뷰 추적, Console Error/Warning 0을 대체 MCP 근거로 통과시켰다. Game 뷰 UI 문구의 직접 캡처는 MCP 제한으로 남았다.
- 총괄 검토는 출력 구조 적합을 확인했으나, 작업 시작 후 `ProjectSettings.asset`에 `APP_UI_EDITOR_ONLY` define 변경이 나타난 것을 범위 충돌로 판정했다. 사용자 변경 가능성이 있어 원인이 확인되기 전에는 되돌리거나 커밋하지 않는다.

## 2026-07-21 KST — Unity 씬/통합 적용 및 자체 Play 점검

- 실제 씬에는 `IsometricCamera`만 있었고, 이 카메라는 MainCamera 태그를 유지한 채 `RatPixelTrial960x540`에 렌더한다. Display 1에 직접 쓰는 카메라가 없었던 원인을 재확인했다.
- 최소 변경으로 `GameViewFrameCamera`를 추가했다. 이 카메라는 Display 1(`targetDisplay=0`)에만 프레임을 제공하고, `targetTexture=null`, `cullingMask=0`, 검은 Solid Color clear, depth `-100`, Untagged다. 월드 렌더는 하지 않으며 IsometricCamera의 RT 출력·MainCamera 태그·추적을 바꾸지 않는다.
- 기존 `WorldPixelOutputPreview` ScreenSpaceOverlay RawImage는 IsometricCamera의 `RatPixelTrial960x540` Point RT를 계속 표시하고, HUD `Canvas` Overlay 정렬 0을 유지한다. Display 1 프레임 카메라는 Overlay UI가 그려질 기반만 제공한다.
- Unity MCP Play에서 카메라 2개(월드 RT 카메라 + Display 1 프레임 카메라), Display 1 프레임 카메라 조건, RawImage/RT·HUD 연결을 확인했다. 런타임 쥐를 `(0.5, 0, 0.25)` 이동한 뒤 명시적 RatHost 카메라 적용 시 쿼터뷰 카메라가 `(0.47, 0.03, 0.26)` 이동해 추적을 확인했다.
- Play 중 화면은 `1920×1080`이었고, Console Error/Warning 0건이다. 기존 dirty 씬을 저장·재로드하지 않았고, QA의 독립 Game 뷰 시각 확인을 남긴다.

## 2026-07-21 KST — 씬 저장 범위 확정

- 초기 진단 기준 `RatHostPrototype` 씬은 dirty가 아니었고, 현재 Scene diff는 `GameViewFrameCamera` 통합에 해당함을 확인했다.
- Unity MCP `EditorSceneManager.SaveScene`으로 `Assets/_Project/Scenes/RatHostPrototype.unity`만 저장했다. 결과 `dirty=False`.
- 저장 후 읽기 전용 재확인: IsometricCamera는 `RatPixelTrial960x540`을 계속 targetTexture로 사용하고, `GameViewFrameCamera`는 Display 1(`targetDisplay=0`), targetTexture 없음, culling mask 0, enabled 상태다. WorldPixelOutput RawImage의 RT 연결도 유지됐다.
- `ProjectSettings.asset`은 저장·수정·되돌리지 않았다. `APP_UI_EDITOR_ONLY` 자동 define 변경은 작업 범위 밖으로 보존한다.
