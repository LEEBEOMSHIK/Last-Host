# Unity MCP 프로젝트 로컬 설정

최종 수정일: 2026-06-29

## 목적

이 문서는 `마지막 숙주 / The Last Host`를 Codex와 Unity MCP로 작업하기 위한 프로젝트 종속 설정과 활성화 절차를 정리한다. 현재는 Unity 프로젝트 생성과 패키지 설치 전 단계이므로, MCP 설정 파일은 준비하되 기본값은 비활성화한다.

## 현재 상태

- 프로젝트 로컬 Codex 설정: `.codex/config.toml`
- Unity MCP 실행 래퍼: `.codex/mcp/start-mcp-unity.ps1`
- MCP 서버 이름: `mcp-unity`
- 현재 활성화 상태: `enabled = false`
- 현재 감지된 Unity 프로젝트 경로: `UnityProject/`
- 현재 감지된 Unity 버전: `6000.4.6f1`

비활성 상태로 둔 이유는 아직 `com.gamelovers.mcp-unity` 패키지가 설치되지 않았기 때문이다. 지금 활성화하면 Codex 시작 시 서버 파일을 찾지 못해 MCP 연결이 실패할 수 있다.

## 선택한 Unity MCP

프로젝트 기준 MCP 후보는 `CoderGamester/mcp-unity`다.

- 패키지명: `com.gamelovers.mcp-unity`
- Unity Package Manager Git URL: `https://github.com/CoderGamester/mcp-unity.git`
- 예상 서버 파일: `Library/PackageCache/com.gamelovers.mcp-unity@*/Server~/build/index.js`

프로젝트 로컬 Codex 설정은 패키지 캐시 해시가 바뀌어도 동작하도록 PowerShell 래퍼가 서버 파일을 탐색하게 구성했다. 래퍼는 저장소 루트와 1단계 하위 폴더 중 Unity 프로젝트 루트를 자동으로 찾는다.

## 활성화 절차

다음 절차는 사용자 승인을 받은 뒤 진행한다.

1. `UnityProject/`를 현재 Unity 프로젝트 루트로 사용할지 승인받는다.
2. Unity Package Manager에서 `https://github.com/CoderGamester/mcp-unity.git` 패키지 추가 승인을 받는다.
3. 패키지를 설치한다.
4. Unity 메뉴에서 `Tools > MCP Unity > Server Window`를 연다.
5. `Force Install Server`를 실행한다.
6. `Start Server`로 Unity MCP 서버를 시작한다.
7. `UnityProject/Library/PackageCache/com.gamelovers.mcp-unity@*/Server~/build/index.js` 파일이 있는지 확인한다.
8. `.codex/config.toml`에서 `enabled = true`로 바꾼다.
9. Codex를 프로젝트 신뢰 상태로 다시 열고 `/mcp`로 `mcp-unity` 연결을 확인한다.

## 승인 게이트

다음 항목은 사용자 승인 없이 실행하지 않는다.

- Unity 프로젝트 생성
- Unity MCP 패키지 설치
- `.codex/config.toml`의 `mcp-unity` 활성화
- Unity Editor에서 MCP 서버 설치 또는 시작 작업
- MCP를 통한 씬, 에셋, 패키지, 코드 변경

## 운영 규칙

- Unity MCP는 이 프로젝트의 Unity Editor 자동화에만 사용한다.
- MCP 작업 전에는 어떤 씬, 에셋, 패키지, 코드가 변경될 수 있는지 먼저 정리한다.
- MCP 작업 결과는 `_workspace/active/<작업ID>/` 또는 `_workspace/completed/<완료일>-<작업ID>/`에 기록한다.
- 패키지 설치, 씬 생성, 코드 작성은 `AGENTS.md` 승인 게이트를 따른다.
- 토큰, API 키, 개인 경로 등 비밀값은 `.codex/config.toml`에 넣지 않는다.

## 문제 해결

### 서버 파일을 찾지 못하는 경우

- Unity 프로젝트가 아직 생성되지 않았는지 확인한다.
- `com.gamelovers.mcp-unity` 패키지가 설치되었는지 확인한다.
- Unity 메뉴에서 `Force Install Server`를 실행했는지 확인한다.
- Unity 프로젝트가 저장소 루트 또는 1단계 하위 폴더에 있는지 확인한다.

### Node.js를 찾지 못하는 경우

- `node`가 PATH에 있는지 확인한다.
- 필요하면 Node.js 설치 후 Codex를 다시 시작한다.

### Codex에서 MCP가 보이지 않는 경우

- `.codex/config.toml`의 `enabled` 값이 `true`인지 확인한다.
- Codex가 이 프로젝트를 신뢰한 상태로 열렸는지 확인한다.
- Codex를 다시 시작한 뒤 `/mcp`로 상태를 확인한다.

## 근거

- Codex는 신뢰된 프로젝트의 `.codex/config.toml`로 프로젝트별 MCP 서버를 설정할 수 있다.
- `CoderGamester/mcp-unity`는 Codex CLI용 프로젝트 로컬 `.codex/config.toml` 예시와 Unity 패키지 캐시 기반 서버 경로를 안내한다.
