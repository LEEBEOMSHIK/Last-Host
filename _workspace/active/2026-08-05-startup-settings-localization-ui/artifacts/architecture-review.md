# Startup·설정·다국어 준비 R3 사전 아키텍처 검토

- 작업 ID: `2026-08-05-startup-settings-localization-ui`
- 검토 범위: PC Startup 씬, 한국어/영어 키 기반 UI, PlayerPrefs 설정, `RatHost2DPrototype` 진입, Build Settings 연결
- 검토 기준: 작업 배정서 C1~C9, Unity 2D 구조 기준, 쥐 숙주 프로토타입 구현 계획
- 검토 방식: 문서 기반 사전 구조 검토만 수행했다. Unity·MCP 실행, production 코드/씬/ProjectSettings/테스트 변경은 수행하지 않았다.

## 결론

**판정: PASS.** 제안한 구조는 C1~C9와 production owner 분리를 만족하며, 기존 `RatHostPrototype` 및 2D 게임플레이 씬/코드를 수정하지 않고 구현할 수 있다. Production2D 화면 캡처에서 `숙주 생명력`, `면역 경계도` 한글 글리프가 정상 표시되고, 기존 2D 씬 빌더도 승인된 내장 `LegacyRuntime.ttf`를 사용한다는 정적 증거가 확인됐다. 신규 Startup UI가 같은 내장 폰트를 재사용하는 조건이면 C1·C5의 폰트 차단 요건은 해소된다.

`LegacyRuntime.ttf` 이외의 신규·외부 폰트 추가는 여전히 별도 사용자 승인 대상이다.

## 추천 파일·컴포넌트 경계

신규 런타임 코드는 task의 소유권을 따라 `Assets/_Project/Scripts/UI/Startup/` 아래에 한정한다. 기존 `Core`, `Host`, `Immune`, `VirusMinigame`, `Mutations` 및 기존 HUD에는 의존하거나 수정하지 않는다.

| 구분 | 추천 파일/컴포넌트 | 책임 | 금지 경계 |
| --- | --- | --- | --- |
| 문자열 키 | `StartupTextKey` | Startup과 설정의 모든 표시 문구를 enum 또는 상수 키로 정의 | UI·씬에 한/영 원문을 직접 쓰지 않음 |
| 카탈로그 | `StartupLocalizationCatalog` | `Language.Korean`/`Language.English`의 키→문자열 표, 기본 언어와 deterministic fallback 제공 | 외부 Localization 패키지, 런타임 다운로드, 신규 문자열 에셋 없음 |
| 문자열 제공자 | `IStartupLocalizer`, `StartupLocalizer` | 현재 미리보기 언어로 키를 해석하고, 언어 변경 시 UI 재렌더 요청 | 게임플레이/HUD 전역 로컬라이저로 확대하지 않음 |
| 설정 모델 | `StartupSettings`, `StartupSettingsDefaults`, `StartupSettingsDraft` | 저장값과 편집 중 임시값을 값 객체로 분리하고 언어·화면 모드·해상도·VSync만 보유 | 음량, 이어하기, 키 재지정 같은 비작동 설정을 추가하지 않음 |
| 저장소 | `IStartupSettingsRepository`, `PlayerPrefsStartupSettingsRepository` | 버전 있는 PlayerPrefs 키 읽기/검증/저장, 손상값 안전 기본값 복귀 | PlayerPrefs를 UI 버튼에서 직접 읽거나 쓰지 않음 |
| 플랫폼 어댑터 | `IStartupPlatform`, `StartupScreenPlatform` | 지원 해상도 검증, `Screen`/VSync 적용, standalone 종료와 Editor 안전 no-op 분리 | 순수 상태·저장소 테스트에 Unity 화면 API를 섞지 않음 |
| 흐름 제어 | `StartupController` | 첫 화면/설정 패널 상태, Apply·Cancel·Defaults·Start·Quit 명령 조율 | `RatHost2DPrototype` 내부 상태나 레거시 씬을 초기화하지 않음 |
| 씬 바인딩 | `StartupView` 또는 `StartupUiBinder` | 버튼·드롭다운·텍스트 참조를 받고 controller 결과를 렌더링 | 키가 아닌 표시 문자열을 Inspector에 저장하지 않음 |
| 씬 명명 | `StartupSceneContract` | 시작 대상 `RatHost2DPrototype`을 한 곳에서 선언하고 Build Settings 포함 여부를 검사할 수 있게 함 | `RatHostPrototype` 또는 기술 샘플을 시작 버튼 대상으로 사용하지 않음 |

`StartupSettings`의 영속 필드는 `schemaVersion`, `language`, `fullScreenMode`, `width`, `height`, `vSync`로 제한한다. 읽기 시 지원하지 않는 언어, 음수/미지원 해상도, 알 수 없는 화면 모드, 파싱 실패, 버전 불일치는 각각 기본값으로 정규화한다. 기본값은 한국어, 명시한 PC 기본 화면 모드, 하나의 유효한 기본 해상도, 결정된 VSync 값으로 코드에서 한 번만 정의한다.

PlayerPrefs 키는 접두사를 포함해 충돌을 막는다. 예: `last_host.startup.settings.v1.*`. 저장은 Apply 성공 뒤에만 수행하며, 개별 키만 남은 중단 저장도 다음 세션에서 전체 기본값으로 안전 복귀할 수 있게 유효성 검사를 한 묶음으로 한다. 향후 형식 변경에는 `schemaVersion`을 올리고 마이그레이션 또는 기본값 복귀 규칙을 추가한다.

## 상태와 다국어 계약

```text
저장값(Settings) ── 설정 열기 ──> Draft 복제 ── UI 미리보기 언어 ──> 화면 렌더
       ^                                  │
       │ Apply: 검증 → 화면 적용 → 저장    │ Cancel/Esc: Draft 폐기 → 저장값 언어로 재렌더
       └──────────────────────────────────┘
```

- 첫 화면은 제목, 태그라인, `프로토타입 시작`, `설정`, `종료`만 보이며 설정 패널은 닫힌 상태다.
- 설정 열기는 저장값을 `Draft`로 복제한다. 드롭다운을 바꿔도 PlayerPrefs와 실제 화면 모드·해상도·VSync는 바뀌지 않는다.
- 언어 드롭다운은 Draft 언어만 즉시 바꾸어 모든 Startup/설정/조작 안내 텍스트를 같은 렌더 사이클에 갱신한다. 따라서 C5의 즉시 전환과 C3의 취소 무저장을 양립한다.
- `기본값`은 Draft를 결정론적 기본값으로 교체할 뿐 저장·화면 적용은 하지 않는다. `적용`이 그 Draft를 검증하고 화면 적용 후 저장한다. 화면 적용이 실패하면 저장하지 않고 오류/안전 기본값 처리 계약을 남긴다.
- `취소`와 설정 패널의 `Esc`는 Draft를 폐기하고 저장값 언어로 UI를 다시 렌더링한다. 첫 화면에서 Esc는 동작하지 않는다. `종료`는 standalone에서 종료 요청을 보내고 Editor에서는 로그 또는 안전 no-op만 수행한다.
- 시작 버튼은 `StartupSceneContract`의 정확한 `RatHost2DPrototype`만 비동기로 또는 일반 씬 로드한다. Startup UI는 씬 전환과 함께 언로드되어 게임 화면을 가리지 않는다.

키 카탈로그는 제목, 태그라인, 시작/설정/종료, 설정 제목, 언어/화면 모드/해상도/VSync, 적용/취소/기본값/뒤로, 조작 안내, 선택지와 오류/대체 문구를 모두 포함한다. `StartupTextKey` 전체 집합과 각 언어 표의 키 집합이 동일한지 EditMode 테스트로 검사한다. fallback은 `요청 언어 키 → 기본 언어 같은 키 → 개발 오류 표식` 순서로 고정하고, 배포 후보에서는 누락 키 0을 통과 조건으로 둔다.

## 코드 → 씬 순차 인계

1. **게임플레이 구현 owner**가 순수 모델·Draft·기본값·저장소·키 카탈로그·scene command를 작성하고, C2~C6·C9의 EditMode 테스트를 먼저 통과시킨다. 이 단계에서는 Unity 씬과 Build Settings를 바꾸지 않는다.
2. 같은 owner가 씬 담당자가 호출할 최소 API를 인계한다. 인계 내용에는 키 목록, 초기 저장값 로드, `Open/Change/Apply/Cancel/Defaults/Start/Quit/Back` 명령, 결과 상태, 화면 적용/종료 어댑터의 fake 가능 인터페이스를 포함한다.
3. **Unity 씬/통합 owner**가 새 `Assets/_Project/Scenes/Startup.unity` 하나에 Canvas, EventSystem, 첫 화면 패널, 설정 패널, `StartupUiBinder`를 연결한다. 신규 이미지·오디오·외부 폰트 없이 Unity 기본 UI 구성요소와 이미 승인된 프로젝트 리소스만 사용한다.
4. 씬 owner는 960×540 기준 `Canvas Scaler`와 16:9 앵커/레이아웃을 적용한다. 긴 영문이 잘리지 않도록 버튼과 라벨의 최소 폭·줄바꿈·텍스트 영역을 계약으로 잡고, Startup Canvas는 다른 게임 씬에 복제하지 않는다.
5. 코드 API와 Startup 씬 계약이 확인된 뒤에만 `EditorBuildSettings.asset`을 단일 owner가 변경한다. Build index 0은 `Startup`, 시작 대상 `RatHost2DPrototype`은 build 포함 상태로 둔다. `RatHostPrototype`은 삭제·수정·시작 대상화하지 않고 레거시 회귀 기준으로 보존한다.
6. QA는 동일 freeze candidate에서 키 완전성, PlayerPrefs 손상/복원, 취소 무저장, 시작 대상, 보호 diff, 대표 960×540·16:9 Play smoke를 독립 확인한다.

## 레거시·2D 보호 경계

- `RatHostPrototype.unity`는 3D 레거시 회귀 기준이다. Startup 도입으로 씬 본문, 프리팹, 코드, 입력, 패키지를 변경하지 않는다. 허용되는 연결은 Build Settings에서 기존 직접 시작을 없애는 것뿐이다.
- `RatHost2DPrototype`은 시작 버튼의 **대상**일 뿐 Startup이 해당 씬의 루트, 카메라, Tilemap, 2D Collider, Y 정렬, HUD, 면역/바이러스/변이 상태를 참조하거나 초기화하지 않는다.
- `RatHost2DTechnicalSample`은 구조 기준 문서의 기술 검증 후보이며 Start 대상의 대체물이 아니다. 현재 작업에서 새 기술 샘플을 만들거나 포함 목록을 확대하지 않는다.
- `SampleScene`과 기존 Build Settings의 다른 항목은 삭제하지 않는다. 순서 및 포함 여부 변경은 Startup과 `RatHost2DPrototype` 연결에 꼭 필요한 최소 diff로 제한하고, 변경 전후 목록을 QA 보호 diff에 남긴다.
- Bootstrap, 전역 `DontDestroyOnLoad`, 신규 Input System/URP/2D Renderer/Localization 패키지는 이 UI 작업의 필수 요소가 아니므로 도입하지 않는다.

## 외부 패키지 없는 확장 방식

V1은 코드 소유의 작은 2언어 카탈로그로 시작한다. 모든 소비 지점이 `StartupTextKey`만 요구하게 만들면, 추후 언어를 늘릴 때는 새 `Language` 값과 같은 키 집합의 표를 추가하는 작업으로 제한된다. UI hierarchy, 버튼 코드, PlayerPrefs 형식은 그대로 유지된다. 언어 코드는 플랫폼 locale 자동 감지 대신 저장된 명시 선택값만 사용하며, 최초 값은 결정된 기본값을 쓴다. 이는 지원 언어 확대를 자동 승인하지 않으며, V1 밖의 언어 추가·번역 품질·폰트는 별도 승인 대상이다.

## 구현 전 위험과 선행 확인

| 위험 | 영향 기준 | 구현 전 대응 |
| --- | --- | --- |
| Startup UI가 기존과 다른 폰트를 참조함 | C1, C5 화면에서 네모/누락 글리프 | Startup 씬/바인더가 기존 2D 화면과 같은 승인 내장 `LegacyRuntime.ttf`를 명시적으로 재사용한다. 신규/외부 폰트는 승인 없이 추가하지 않는다. |
| `RatHost2DPrototype`의 정확한 scene path 또는 Build 포함 상태 불일치 | C2 시작 실패 | 씬 owner가 변경 직전 실제 scene path와 Build Settings 항목을 확인하고 contract 상수·검증을 같은 값으로 맞춘다. |
| `Screen.SetResolution`이 PC/Editor에서 요청값을 그대로 수용하지 않음 | C4, C6 | 화면 모드/해상도 후보는 플랫폼 어댑터가 검증한 값만 노출하고, 지원하지 않는 저장값은 기본값으로 정규화한다. Editor smoke에서 실제 종료는 수행하지 않는다. |
| 적용 중 저장 선행 또는 언어 preview 누수 | C3~C6 | Apply 순서를 검증→화면 적용→저장으로 고정하고, Cancel은 Draft 폐기와 저장값 언어 재렌더를 테스트한다. |
| UI 표시 문자열의 직접 입력 또는 키 누락 | C5 | 키 enum 집합과 양 언어 표의 완전성 테스트, Inspector 문자열 금지, fallback 오류 표식을 적용한다. |
| 960×540/긴 영문 레이아웃 초과 | C7 | Canvas 기준 해상도·앵커·최소 폭을 명시하고 960×540과 대표 16:9 한 화면에서 C7을 Play 확인한다. |
| Build Settings의 넓은 재작성 | C2, C8 | 씬 owner만 최소 diff를 적용하고 QA가 `RatHostPrototype`·2D 핵심 루프·패키지·reference 보호 diff를 확인한다. |

## C1~C9 구조상 실행 가능성

| 기준 | 구조 근거 | 판정 |
| --- | --- | --- |
| C1 | Startup 단일 씬과 명시적 첫 화면/설정 패널 상태, 기존 검증된 내장 `LegacyRuntime.ttf` 재사용 | 가능 |
| C2 | 단일 scene contract와 `RatHost2DPrototype` 고정 시작 명령 | 가능 |
| C3 | 저장값/Draft 분리 및 Cancel/Esc 폐기 | 가능 |
| C4 | 결정론적 defaults, 검증 후 화면 적용·PlayerPrefs 저장 | 가능 |
| C5 | enum 키, 양 언어 전체 표, 즉시 재렌더, fallback, 기존 검증된 내장 `LegacyRuntime.ttf` 재사용 | 가능(키 완전성 확인 필요) |
| C6 | 버전 있는 PlayerPrefs, 묶음 검증, 안전 기본값 | 가능 |
| C7 | 960×540 기준 Canvas/앵커와 Startup 한정 overlay | 가능(실기 레이아웃 확인 필요) |
| C8 | Startup 신규 추가 및 Build Settings 최소 연결, 기존 scene/code 불변 | 가능 |
| C9 | 플랫폼 종료 어댑터와 설정 상태 Back/Esc 계약 | 가능 |

## 다음 인계 조건

1. 씬 owner는 기존 2D 화면에서 검증된 승인 내장 `LegacyRuntime.ttf`를 Startup UI에 재사용하고, QA는 한·영 전체 키 표시를 확인한다. 신규·외부 폰트는 추가하지 않는다.
2. 게임플레이 구현 owner가 위 API 경계와 순수 EditMode 테스트를 먼저 인계한다.
3. 씬/통합 owner가 API 인계 후 Startup 씬과 Build Settings 최소 변경을 수행한다.
4. QA와 총괄 관리자가 같은 freeze candidate의 C1~C9 및 보호 diff를 확인한 뒤에만 사용자 수용 단계로 넘긴다.
