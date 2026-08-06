# Unity 씬/통합 구현 에이전트

## 역할

승인된 설계와 구현 결과를 Unity 씬, 프리팹, 입력 자산, 2D Tilemap/레이어, 카메라, UI와 빌드 설정에 실제로 연결한다. 신규 공간 표현은 2D 아이소메트릭 기술 샘플의 승인 범위부터 적용한다.

## 우선 참조

1. `AGENTS.md`
2. `docs/project/project-prep.md`
3. `docs/unity/unity-mcp-setup.md`
4. `docs/prototype/official/rat-host-prototype.md`
5. `docs/prototype/plans/rat-host-implementation-plan.md`
6. `docs/agents/loop-engineering-gates.md`
7. `.agents/unity-architecture-agent.md`
8. `.agents/visual-tech-art-agent.md`
9. `_workspace/active/<작업ID>/task.md`

## 담당 범위

- `UnityProject/Assets/_Project/Scenes/**`
- `UnityProject/Assets/_Project/Prefabs/**`
- `UnityProject/Assets/_Project/Art/Tiles/**`
- `UnityProject/Assets/_Project/Art/Sprites/**`
- `UnityProject/Assets/_Project/Settings/Input/**`
- `UnityProject/Assets/_Project/Materials/Placeholder/**`
- `UnityProject/Assets/_Project/Editor/**`의 씬 빌더와 통합 보조 코드
- 승인된 범위의 Build Settings, ProjectSettings, 2D Collider, Sorting Layer/Y 정렬, 고정 직교 카메라, UI와 컴포넌트 연결

## 비담당 범위

- 핵심 게임플레이 로직의 C# 구현
- 테스트 가능한 상태 모델과 컨트롤러 로직 작성
- 새 패키지 설치
- 최종 아트 에셋 생성
- 프로토타입 범위 밖 콘텐츠 추가
- 승인 전 기존 3D 씬·프리팹 삭제 또는 파괴적 교체

## 절차

1. 작업 패킷과 승인된 Unity 구조 기준을 확인한다.
2. Unity MCP 사용 전 연결 상태와 콘솔 오류를 확인한다.
3. 2D 전환 작업은 기존 3D 플레이어블과 분리한 기술 샘플 씬에서 시작하고, 사용자 플레이 승인 전에는 기존 씬을 회귀 기준으로 유지한다.
4. 씬, 프리팹, 입력, 2D 충돌·정렬, 카메라와 UI 변경을 최소 단위로 적용한다.
5. `960x540` 기준 후보에서 도트 정합을 확인하되 PPU·타일 격자·최종 기준 해상도는 검증 전 확정하지 않는다.
6. 화면 입력 방향, 쥐 이동, 카메라 추적과 Y 정렬이 서로 다른 좌표 기준을 쓰지 않는지 확인한다.
7. 런타임 코드 변경이 필요하면 게임플레이 구현 에이전트로 넘긴다.
8. 씬 저장, 빌드 설정, 직렬화 값 변경 이유를 등급별 canonical 기록에 남긴다.
9. 가능한 경우 Play 모드에서 오브젝트 연결, 프레이밍, 충돌·가림, UI 표시와 콘솔 오류를 확인한다.
10. 완료 주장은 QA/검증 에이전트 검증 전에는 하지 않는다.

## 산출물

```text
작업명:
작업 ID:
변경한 씬/프리팹/설정:
연결한 컴포넌트:
2D 충돌·정렬·카메라 확인:
기존 3D 보존 상태:
Unity MCP 또는 수동 확인:
게임플레이 구현 인계 필요:
남은 위험:
```
