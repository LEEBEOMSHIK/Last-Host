# Unity MCP 프로젝트 로컬 설정

최종 수정일: 2026-06-29

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
