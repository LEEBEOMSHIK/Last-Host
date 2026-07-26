---
name: unity-prototype-planner
description: 마지막 숙주 프로젝트의 Unity 2D 아이소메트릭 프로토타입 구조, Unity 버전, 폴더·씬 구성, Tilemap·2D 충돌·정렬·카메라 후보와 승인 체크리스트를 계획한다. Unity 프로젝트 준비나 2D 전환 구조 설계 요청에 사용한다.
---

# Unity Prototype Planner

## 기본 역할

Unity 프로젝트 생성이나 2D 구조 전환 전에 구조와 승인 항목을 정리한다. 실제 Unity 프로젝트 생성, 패키지 설치, 코드·씬 작성은 수행하지 않고 계획과 체크리스트를 만든다.

## 필수 참조 순서

1. `AGENTS.md`
2. `docs/project/project-prep.md`
3. `docs/unity/unity-mcp-setup.md`
4. `docs/prototype/official/rat-host-prototype.md`
5. `docs/agents/agent-skill-plan.md`

## 작업 절차

1. 요청이 프로젝트 생성·전환 계획인지, 실제 구현인지 구분한다.
2. Unity 버전, 플랫폼, 렌더러, 폴더 구조, 씬 구조의 승인 여부를 확인한다.
3. Unity MCP 작업이면 `.codex/config.toml`의 `unity_mcp` 활성화 상태와 `docs/unity/unity-mcp-setup.md`의 승인 게이트를 확인한다.
4. 승인되지 않은 항목은 결정 후보와 장단점만 제시한다.
5. 시스템 경계를 `숙주`, `면역`, `모드 전환`, `바이러스 미니게임`, `변이`, `공용 상태`로 나눈다.
6. 2D 아이소메트릭 Tilemap 또는 동등 레이어, 2D Collider, Y 정렬, 고정 직교 카메라와 도트 스프라이트의 경계를 정리한다.
7. 기존 3D 씬은 레거시 회귀 기준으로 보존하고, 별도 2D 기술 샘플과 사용자 플레이 승인 전에는 교체·삭제를 제안하지 않는다.
8. `960x540` 내부 기준 화면은 후보로 관리하며 PPU·타일 격자·최종 기준 해상도와 패키지는 기술 샘플 검증 전에 확정하지 않는다.
9. 구현 계획으로 넘어가기 전 승인 질문 목록을 만든다.

## 기본 구조 후보

```text
Assets/
  _Project/
    Art/
      Tiles/
      Sprites/
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

씬 후보:

```text
Assets/_Project/Scenes/
  RatHostPrototype.unity          # 기존 3D 레거시·회귀 기준
  RatHost2DTechnicalSample.unity  # 별도 승인 후 생성할 2D 기술 샘플 후보
```

## 금지 범위

- 사용자 승인 없는 Unity 프로젝트 생성
- 사용자 승인 없는 패키지 설치
- 사용자 승인 없는 렌더 파이프라인 확정
- 코드 작성 또는 씬 파일 생성
- 기존 3D 씬·검증 자료의 즉시 삭제나 파괴적 교체
- 목업을 실제 타일셋·스프라이트 시트 또는 구현 완료 증거로 선언

## 산출물 형식

```text
Unity 준비 요약:

추천 구조:
- 

2D 공간·충돌·정렬·카메라 기준:
-

기존 3D 보존 경계:
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
