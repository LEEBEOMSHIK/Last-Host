# Unity 베이스라인 검증 보고

최종 수정일: 2026-06-29

## 목적

쥐 숙주 프로토타입 구현 전에 현재 Unity 프로젝트의 읽기 전용 기준 상태를 기록한다. 이 보고서는 Unity 프로젝트를 변경하지 않고 MCP 조회와 로컬 파일 확인만으로 작성했다.

## 검증 대상

- Unity 프로젝트 루트: `UnityProject/`
- Unity 버전
- 활성 씬과 빌드 설정 씬
- Assets 리소스 구성
- 주요 패키지
- URP 설정 존재 여부
- Unity MCP 읽기 연결

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
- 도트풍 저폴리 3D 렌더링을 위한 세부 조정은 아직 수행하지 않았다.

## 미검증 항목

- Unity 에디터 컴파일 오류 여부
- EditMode 테스트
- PlayMode 테스트
- PC 빌드
- 실제 플레이 조작감
- 카메라, 저해상도 렌더링, UI 가독성
- MCP를 통한 씬/에셋 변경

## 남은 위험

- 현재 활성 씬은 저장되지 않은 기본 씬이고, 빌드 설정에는 `SampleScene.unity`가 등록되어 있어 작업 씬 기준이 아직 불명확하다.
- 게임 전용 폴더 구조가 없어 구현을 시작하면 에셋과 스크립트 위치를 먼저 정해야 한다.
- Unity 버전, URP 사용, Input System 사용, 플레이스홀더 범위는 승인 패킷에서 최종 확인해야 한다.

## 완료 판단

읽기 전용 베이스라인 기록은 완료했다. 컴파일, 테스트, 빌드, 플레이 검증은 아직 실행하지 않았으므로 통과로 주장하지 않는다.
