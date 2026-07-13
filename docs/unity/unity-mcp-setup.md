# Unity MCP 프로젝트 로컬 설정

최종 수정일: 2026-07-13

## 목적

이 문서는 `마지막 숙주 / The Last Host`를 Codex와 Unity MCP로 작업하기 위한 프로젝트 종속 설정과 운영 절차를 정리한다. 현재 `UnityProject/`는 루트 저장소에 편입되어 있고, Codex에서 Unity MCP 읽기 호출이 가능한 상태다.

## 현재 상태

- 프로젝트 로컬 Codex 설정: `.codex/config.toml`
- Unity MCP 실행 래퍼: `.codex/mcp/start-mcp-unity.ps1`
- Codex MCP 서버 이름: `unity_mcp`
- 현재 활성화 상태: `enabled = true`
- 현재 감지된 Unity 프로젝트 경로: `UnityProject/`
- 현재 감지된 Unity 버전: `6000.4.6f1`
- 현재 Unity MCP 연결: `Unity_ManageScene(GetActive)` 읽기 호출 성공
- Unity 프로젝트 패키지: `com.gamelovers.mcp-unity`가 `UnityProject/Packages/manifest.json`에 포함됨

현재 Codex 설정은 Unity의 로컬 relay 실행 파일을 사용한다.

```toml
[mcp_servers.unity_mcp]
command = "C:\\Users\\User\\.unity/relay\\relay_win.exe"
args = ["--mcp"]
enabled = true
```

## 선택한 Unity MCP

프로젝트에 설치된 Unity MCP 패키지는 `CoderGamester/mcp-unity`다.

- 패키지명: `com.gamelovers.mcp-unity`
- Unity Package Manager Git URL: `https://github.com/CoderGamester/mcp-unity.git`
- 예상 서버 파일: `Library/PackageCache/com.gamelovers.mcp-unity@*/Server~/build/index.js`

`.codex/mcp/start-mcp-unity.ps1`는 패키지 캐시 해시가 바뀌어도 서버 파일을 찾기 위한 보조 래퍼로 유지한다. 현재 `.codex/config.toml`의 활성 MCP 항목은 Unity 로컬 relay 기반 `unity_mcp`다.

## 운영 절차

MCP가 이미 연결된 상태라도, Unity 프로젝트를 변경하는 작업은 사용자 승인을 받은 뒤 진행한다.

1. 작업 목적과 변경 가능 파일을 먼저 정리한다.
2. 읽기 전용 확인이면 `Unity_ManageScene`, `Unity_ListResources` 같은 조회 도구만 사용한다.
3. 씬, 에셋, 패키지, 코드, ProjectSettings를 바꾸는 작업이면 사용자 승인을 먼저 받는다.
4. MCP 작업 결과는 `_workspace/active/<작업ID>/` 또는 `_workspace/completed/<완료일>-<작업ID>/`에 기록한다.
5. 변경 후에는 Unity MCP 조회, Git diff, 필요한 경우 Unity 테스트/빌드로 검증한다.

## 승인 게이트

다음 항목은 사용자 승인 없이 실행하지 않는다.

- Unity 프로젝트 생성
- Unity MCP 패키지 설치 또는 변경
- `.codex/config.toml`의 MCP 서버 설정 변경
- Unity Editor에서 MCP 서버 설치 또는 재시작 작업
- MCP를 통한 씬, 에셋, 패키지, 코드 변경

## 운영 규칙

- Unity MCP는 이 프로젝트의 Unity Editor 자동화에만 사용한다.
- MCP 작업 전에는 어떤 씬, 에셋, 패키지, 코드가 변경될 수 있는지 먼저 정리한다.
- MCP 작업 결과는 `_workspace/active/<작업ID>/` 또는 `_workspace/completed/<완료일>-<작업ID>/`에 기록한다.
- 패키지 설치, 씬 생성, 코드 작성은 `AGENTS.md` 승인 게이트를 따른다.
- 토큰, API 키, 개인 경로 등 비밀값은 `.codex/config.toml`에 넣지 않는다.

## Unity MCP 입력 검증 표준

### 검증 결과 분류

입력과 기능 상태를 같은 결과로 기록하지 않는다.

| 분류 | 통과 조건 | 기록 원칙 |
| --- | --- | --- |
| 실제 키 입력 검증 | Game View 포커스 확인 뒤 네이티브 키 이벤트를 보내고, 기대한 런타임 상태와 관측 가능한 HUD·오브젝트 변화가 함께 확인됨 | `실제 키 입력 검증 통과`로 기록한다. |
| MCP 직접 상태 전환 대체 검증 | `Unity_RunCommand` 등으로 상태를 직접 전환하고 런타임 상태·HUD·콘솔을 확인함 | 기능 상태 대체 검증으로만 기록한다. 키 입력 수신 증명으로 쓰지 않는다. |
| 미검증 또는 차단 | 입력 전달 또는 MCP 응답 자체를 신뢰할 수 없거나 관측 결과가 없음 | 통과로 주장하지 않고 원인·재개 조건·확인하지 못한 항목을 남긴다. |

2026-07-10 검증에서는 일반 `SendKeys` F6 입력이 Unity Game View에 잡히지 않았고, Game View 포커스 후 Windows 네이티브 `keybd_event` F6 입력에서만 `InternalVirus / ImmuneSignalSuppression` 전환과 HUD 상태가 확인됐다. 따라서 `SendKeys` 성공 여부나 상태 전환 명령의 반환만으로 실제 키 입력 통과를 선언하지 않는다.

### 실제 키 입력 검증 절차

1. 작업 기록에 대상 씬, 대상 키, 기대 상태/HUD, 허용된 변경 범위를 적는다.
2. MCP 승인, Unity Editor 준비 상태, 대상 씬, Play 진입 가능 여부를 읽기 호출로 확인한다. 승인 대기나 relay 응답 이상이면 이 단계에서 차단으로 기록한다.
3. Play 진입 후 Game View 포커스를 명시적으로 확보한다. 포커스 확보 증적이 없으면 키 입력을 보내지 않거나 미검증으로 처리한다.
4. 검증 대상으로 승인된 네이티브 키 이벤트를 한 번 보낸다. 일반 `SendKeys`는 보조 시도일 뿐 표준 입력 경로가 아니다.
5. 입력 직후 런타임 상태를 읽고, 해당 기능의 HUD 텍스트·활성 상태·관측 가능한 오브젝트 변화를 함께 확인한다. 상태 전환 하나만 확인된 경우에는 실제 입력 검증이 아니라 불충분으로 기록한다.
6. Unity Console의 Error/Warning, Play 종료 성공 여부, 활성 씬의 dirty 상태를 확인한다. 시간 배율 등 런타임 값을 바꿨다면 원복 여부도 남긴다.
7. 결과를 작업 폴더의 `verification.md`와 `agent-activity.md`에 남기고, 사용자 수동 플레이 확인이 필요한 항목은 별도로 표시한다.

### 직접 상태 전환 대체 검증

네이티브 입력을 사용할 수 없더라도 `Unity_RunCommand` 등의 직접 상태 전환으로 기능 상태, HUD 갱신, 콘솔 오류를 확인할 수 있다. 이 경우에는 다음처럼 범위를 제한한다.

- 결과 분류를 `MCP 직접 상태 전환 대체 검증`으로 쓴다.
- 확인된 기능 상태와 확인하지 못한 실제 입력 수신을 각각 적는다.
- 대체 검증 통과를 실제 Game View 키 입력 통과나 사용자 체감 확인으로 승격하지 않는다.
- 실제 키 입력 재검증이 필요한 조건과 다음 실행 주체를 남긴다.

### 미검증·차단 기준

다음 조건은 재시도 횟수와 관계없이 실제 키 입력 검증 통과가 아니다.

- 일반 `SendKeys` 입력이 Unity에 잡히지 않음
- Game View 포커스를 확보하거나 증명하지 못함
- Unity MCP가 `Awaiting user approval`, `Connection revoked` 등 승인 대기 상태임
- relay 또는 MCP 호출이 무응답·시간 초과로 완료 결과를 주지 않음

각 경우에 입력 시도 방식, 마지막 정상 응답, 차단 메시지 또는 시간 초과, 미확인 상태, 재개 조건을 기록한다. 승인 대기·relay 무응답을 우회하려고 Unity 설정이나 프로세스를 변경하지 않으며, 그 변경에는 별도 승인 게이트가 필요하다.

### 최소 기록 양식

```text
검증 대상: <씬 / 기능 / 키>
결과 분류: 실제 키 입력 검증 | MCP 직접 상태 전환 대체 검증 | 미검증 또는 차단

사전 조건:
- MCP 승인/relay 상태:
- Play 상태와 Game View 포커스 증적:

입력 또는 대체 경로:
- 방식: <네이티브 키 이벤트 | SendKeys 보조 시도 | Unity_RunCommand 직접 상태 전환>
- 기대 상태/HUD:
- 실제 상태/HUD:

종료 확인:
- Console Error/Warning:
- Play 종료 / 씬 dirty / 런타임 원복:

미검증·대체 검증 범위:
-
남은 위험과 재개 조건:
-
```

## 문제 해결

### 서버 파일을 찾지 못하는 경우

- 현재 활성 설정이 Unity relay인지, 보조 래퍼인지 먼저 확인한다.
- `com.gamelovers.mcp-unity` 패키지가 설치되었는지 확인한다.
- Unity 메뉴에서 `Force Install Server`를 실행했는지 확인한다.
- Unity 프로젝트가 저장소 루트 또는 1단계 하위 폴더에 있는지 확인한다.

### Node.js를 찾지 못하는 경우

- `node`가 PATH에 있는지 확인한다.
- 현재 래퍼는 Codex 번들 Node를 fallback으로 탐색한다.
- Unity MCP의 공식 요구사항은 Node.js 18 이상이므로, 문제가 있으면 Node.js LTS를 설치한 뒤 Codex를 다시 시작한다.

### Codex에서 MCP가 보이지 않는 경우

- `.codex/config.toml`의 `enabled` 값이 `true`인지 확인한다.
- MCP 서버 이름이 `unity_mcp`인지 확인한다.
- Codex가 이 프로젝트를 신뢰한 상태로 열렸는지 확인한다.
- Codex를 다시 시작한 뒤 MCP 도구 목록과 `Unity_ManageScene(GetActive)` 호출로 상태를 확인한다.

## 근거

- Codex는 신뢰된 프로젝트의 `.codex/config.toml`로 프로젝트별 MCP 서버를 설정할 수 있다.
- `CoderGamester/mcp-unity`는 Codex CLI용 프로젝트 로컬 `.codex/config.toml` 예시와 Unity 패키지 캐시 기반 서버 경로를 안내한다.
