# Unity 베이스라인 검증 보고

최종 수정일: 2026-07-27

## 목적

쥐 숙주 프로토타입 구현 전에 2026-06-29에 확인한 Unity 프로젝트의 읽기 전용 기준 상태를 기록한다. 이 보고서의 조회 결과는 당시의 역사적 베이스라인이며 현재 Unity 프로젝트 전체 구현 상태를 뜻하지 않는다.

## 2026-07-27 방향 전환 해석

- Unity `6000.4.6f1`, PC 우선, 기존 핵심 상태 시스템은 유지한다.
- 신규 비주얼·공간 제작 기본 방향은 2D 아이소메트릭/쿼터뷰 도트 타일과 스프라이트다.
- 현재 구현된 3D 씬과 검증 자료는 레거시 회귀 기준으로 보존하며 즉시 파괴하거나 대체하지 않는다.
- 후속 전환은 별도 2D 플레이어블 기술 샘플에서 Tilemap 또는 동등한 2D 레이어, 2D Collider, Y 정렬, 고정 직교 카메라와 도트 스프라이트를 먼저 검증한 뒤 진행한다.
- 이 문서 갱신은 Unity 씬, 코드, ProjectSettings, 패키지 또는 빌드 설정을 변경하지 않는다.

## 검증 대상

- Unity 프로젝트 루트: `UnityProject/`
- Unity 버전
- 활성 씬과 빌드 설정 씬
- Assets 리소스 구성
- 주요 패키지
- URP 설정 존재 여부
- Unity MCP 읽기 연결

아래 결과의 `현재` 표현은 모두 원래 조회일인 2026-06-29 시점을 가리킨다.

## 실행한 검증

### Unity MCP 활성 씬 조회

명령:

```text
Unity_ManageScene(Action=GetActive)
```

결과:

- 씬 이름: 없음
- 씬 경로: 없음
- Build Index: `-1`
- Dirty 상태: `false`
- Loaded: `true`
- Root Count: `2`

해석:

- 현재 에디터 활성 씬은 저장되지 않은 기본 씬 상태다.
- 구현 작업 전에 실제 작업 씬을 `Assets/Scenes/SampleScene.unity`로 열지, 별도 프로토타입 씬을 만들지 결정해야 한다.

### Unity MCP 계층 조회

명령:

```text
Unity_ManageScene(Action=GetHierarchy, Depth=-1)
```

결과:

- `Main Camera`
- `Directional Light`

해석:

- 현재 활성 씬에는 기본 카메라와 조명만 있다.
- 쥐 숙주 프로토타입용 맵, 컨트롤러, UI, 미니게임 오브젝트는 아직 없다.

### Unity MCP 빌드 설정 조회

명령:

```text
Unity_ManageScene(Action=GetBuildSettings)
```

결과:

- `Assets/Scenes/SampleScene.unity`
  - Enabled: `true`
  - Build Index: `0`
  - GUID: `99c9720ab356a0642a771bea13969a05`

해석:

- 빌드 설정에는 샘플 씬 1개만 등록되어 있다.
- 현재 활성 씬과 빌드 설정 씬이 다르므로, 구현 전 작업 씬 정책이 필요하다.

### Unity MCP Assets 목록 조회

명령:

```text
Unity_ListResources(Under=Assets, Pattern=*, Limit=120)
```

결과:

- 총 리소스 수: `35`
- 주요 리소스:
  - `Assets/InputSystem_Actions.inputactions`
  - `Assets/Scenes/SampleScene.unity`
  - `Assets/Settings/PC_RPAsset.asset`
  - `Assets/Settings/PC_Renderer.asset`
  - `Assets/Settings/Mobile_RPAsset.asset`
  - `Assets/Settings/UniversalRenderPipelineGlobalSettings.asset`
  - `Assets/TutorialInfo/...`

해석:

- 프로젝트는 URP 템플릿 기반 초기 자산과 튜토리얼/리드미 자산 중심이다.
- `Assets/_Project/` 같은 게임 전용 폴더 구조는 아직 없다.

### Unity 버전 파일 확인

명령:

```powershell
Get-Content UnityProject\ProjectSettings\ProjectVersion.txt
```

결과:

- `m_EditorVersion: 6000.4.6f1`
- `m_EditorVersionWithRevision: 6000.4.6f1 (0b051c2e5d54)`

해석:

- 현재 프로젝트 기준 Unity 버전은 `6000.4.6f1`이다.

### 패키지 구성 확인

명령:

```powershell
Get-Content UnityProject\Packages\manifest.json
```

결과:

- `com.gamelovers.mcp-unity`: `https://github.com/CoderGamester/mcp-unity.git`
- `com.unity.render-pipelines.universal`: `17.4.0`
- `com.unity.inputsystem`: `1.19.0`
- `com.unity.test-framework`: `1.6.0`
- `com.unity.ai.assistant`: `2.13.0-pre.2`
- `com.unity.ai.inference`: `2.6.1`
- `com.unity.ai.navigation`: `2.0.12`

해석:

- Unity MCP, URP, Input System, Test Framework가 패키지 목록에 있다.
- 프로토타입 구현 전에 실제로 사용할 입력 방식과 테스트 범위를 승인해야 한다.

### ProjectSettings 주요 값 확인

명령:

```powershell
Select-String UnityProject\ProjectSettings\ProjectSettings.asset -Pattern "projectName:|scriptingDefineSymbols|Standalone:"
```

결과:

- `projectName: My project`
- `scriptingDefineSymbols`
- `Standalone: SENTIS_ANALYTICS_ENABLED`

해석:

- Unity 프로젝트 표시 이름은 사용자가 유지하겠다고 정한 `My project`다.
- Standalone 빌드에 `SENTIS_ANALYTICS_ENABLED` define이 설정되어 있다.

### URP 설정 확인

명령:

```powershell
Select-String UnityProject\ProjectSettings\GraphicsSettings.asset,UnityProject\ProjectSettings\QualitySettings.asset -Pattern "RenderPipeline"
```

결과:

- `GraphicsSettings.asset`에 커스텀 렌더 파이프라인 참조가 있다.
- `QualitySettings.asset`에 품질 레벨별 커스텀 렌더 파이프라인 참조가 있다.

해석:

- URP 설정 에셋은 프로젝트에 연결되어 있다.
- 당시 도트풍 저폴리 3D 렌더링을 위한 세부 조정은 수행하지 않은 상태였다.
- 2026-07-27 이후 신규 기본 방향은 2D이므로, URP의 3D 세부 조정은 새 기본 과제가 아니다. URP 유지·축소 범위와 2D 렌더러 사용 여부는 기술 샘플 구조 제안에서 검토하되 패키지·ProjectSettings 변경은 별도 승인한다.

## 미검증 항목

- Unity 에디터 컴파일 오류 여부
- EditMode 테스트
- PlayMode 테스트
- PC 빌드
- 실제 플레이 조작감
- 카메라, 저해상도 렌더링, UI 가독성
- MCP를 통한 씬/에셋 변경

## 남은 위험

- 이 보고서의 씬·에셋 수치는 2026-06-29 베이스라인이므로 현재 상태 확인에는 최신 Unity 조회와 Git 기록이 필요하다.
- 기존 3D 플레이어블을 보존하면서 2D 기술 샘플을 병행해야 하므로 씬·프리팹·입력·공용 상태 의존성을 먼저 분리해야 한다.
- 2D 기준 PPU, 타일 격자, Sorting Layer, 2D Collider 형태와 `960x540` 내부 기준 화면은 아직 기술 샘플 검증 전 후보값이다.
- 2D Renderer나 Pixel Perfect Camera 등 추가 패키지·렌더러·ProjectSettings 변경이 필요하면 별도 승인과 회귀 검증이 필요하다.

## 완료 판단

2026-06-29 읽기 전용 베이스라인 기록은 완료했다. 이 보고서만으로 현재 컴파일, 테스트, 빌드, 플레이 상태 또는 2D 전환 가능성을 통과로 주장하지 않는다. 2D 전환 완료 판정은 별도 기술 샘플 구현과 사용자 플레이 승인 뒤에만 내린다.
