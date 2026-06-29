---
name: unity-prototype-planner
description: 마지막 숙주 프로젝트의 Unity 3D 프로토타입 생성 전 구조, Unity 버전, URP, 폴더 구조, 씬 구성, 패키지 후보, 승인 체크리스트를 계획한다. Unity 프로젝트 준비나 구조 설계 요청에 사용한다.
---

# Unity Prototype Planner

## 기본 역할

Unity 프로젝트를 만들기 전에 구조와 승인 항목을 정리한다. 실제 Unity 프로젝트 생성, 패키지 설치, 코드 작성은 수행하지 않고 계획과 체크리스트를 만든다.

## 필수 참조 순서

1. `AGENTS.md`
2. `docs/project-prep.md`
3. `docs/unity-mcp-setup.md`
4. `docs/rat-host-prototype.md`
5. `docs/agent-skill-plan.md`

## 작업 절차

1. 요청이 프로젝트 생성 전 계획인지, 실제 구현인지 구분한다.
2. Unity 버전, URP, 플랫폼, 폴더 구조, 씬 구조의 승인 여부를 확인한다.
3. Unity MCP 작업이면 `.codex/config.toml`의 `unity_mcp` 활성화 상태와 `docs/unity-mcp-setup.md`의 승인 게이트를 확인한다.
4. 승인되지 않은 항목은 결정 후보와 장단점만 제시한다.
5. 시스템 경계를 `숙주`, `면역`, `모드 전환`, `바이러스 미니게임`, `변이`, `공용 상태`로 나눈다.
6. 구현 계획으로 넘어가기 전 승인 질문 목록을 만든다.

## 기본 구조 후보

```text
Assets/
  _Project/
    Art/
    Audio/
    Materials/
    Prefabs/
    Scenes/
    Scripts/
      Core/
      Host/
      Immune/
      Mutations/
      VirusMinigame/
      UI/
    Settings/
```

## 금지 범위

- 사용자 승인 없는 Unity 프로젝트 생성
- 사용자 승인 없는 패키지 설치
- 사용자 승인 없는 렌더 파이프라인 확정
- 코드 작성 또는 씬 파일 생성

## 산출물 형식

```text
Unity 준비 요약:

추천 구조:
- 

승인 필요:
- 

구현 전 위험:
- 

다음 단계:
- 
```

## 추가 기준

Unity 구조 기준이 필요하면 `references/unity-architecture.md`를 읽는다.
