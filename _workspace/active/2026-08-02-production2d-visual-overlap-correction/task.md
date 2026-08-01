# 작업 배정

## 작업명

Production2D 쥐·오브젝트 가시 실루엣 겹침 완전 교정

## 작업 ID

`2026-08-02-production2d-visual-overlap-correction`

## 상태

진행 중 — 사용자 실제 화면에서 이전 교정 불충분 확인

## 담당 에이전트

- 실제 코드·씬·테스트 변경: Unity 씬/통합 구현 에이전트
- 시각 기준 보조: 비주얼/테크아트 에이전트
- 조건부 런타임 정렬 로직 변경: 게임플레이 구현 에이전트
- 독립 검증: QA/검증 에이전트
- 내부 승인: 프로젝트 총괄 관리자 에이전트

## 목적

사용자 제공 화면에서 쥐의 가시 몸통·꼬리가 벽이나 소품 내부에 들어가 보이는 문제를 재현하고, 실제로 도달 가능한 모든 접촉·모서리·앞뒤 이동 상태에서 부자연스러운 관통이나 정렬 팝이 보이지 않도록 수정한다.

## 입력 자료

- 사용자 확인 화면: `docs/references/images/image.png`
- `UnityProject/Assets/_Project/Scenes/RatHost2DTechnicalSample.unity`
- `UnityProject/Assets/_Project/Editor/TechnicalSample2D/RatHost2DProductionSampleSceneBuilder.cs`
- `UnityProject/Assets/_Project/Scripts/TechnicalSample2D/YSortSprite2D.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/TechnicalSample2D/Production2DV1AssetAndSceneTests.cs`
- 이전 교정 기록: `_workspace/active/2026-07-30-production2d-occlusion-hud-correction/`

## 해야 할 일

1. 사용자 화면과 같은 중앙 벽 접촉 위치를 재현하고 쥐 루트, collider, sprite alpha bounds, 벽 visual/footprint bounds를 수치화한다.
2. 이전 `tieBreak=1`·좁은 발밑 footprint만으로 가시 실루엣 관통이 남은 원인을 기록한다.
3. 단순 정지 좌표뿐 아니라 실제 이동으로 도달 가능한 벽·통·상자 앞/뒤/좌우 모서리 전체 경로에서 가시 몸통이 오브젝트 내부를 가로지르지 않게 최소 구조 수정한다.
4. 투명 캔버스 전체가 아니라 실제 가시 몸통과 접지 범위를 기준으로 충돌 여유를 정한다. 꼬리 처리 규칙도 명시한다.
5. 쥐가 앞일 때 전체 쥐가 자연스럽게 앞에, 뒤일 때 오브젝트가 자연스럽게 가리도록 하고 경계에서 반복 점멸하지 않게 한다.
6. 회귀 테스트와 씬 계약 테스트를 추가하고 씬을 rebuild/save한다.
7. 실제 Play 이동 경로 캡처, 충돌 거리, sorting order, jitter, Console, scene dirty, 보호 diff를 기록한다.

## 시각 관통과 자연 가림의 구분

- 불투과 지면 영역: 벽·통·상자의 실제 solid footprint를 화면 투영한 영역이다. 실제로 도달 가능한 자세에서 쥐의 머리·몸통·발 가시 alpha는 이 영역과 교차하지 않아야 한다.
- 최소 시각 여유: 접촉 상태에서 불투과 지면 영역과 쥐 머리·몸통·발 가시 alpha 사이에 최소 `1 logical pixel = 1/128 world unit`의 화면 투영 여유를 유지하거나, 오브젝트의 수직 전면에 의해 완전히 가려져야 한다. 0↔1픽셀 반복 점멸은 실패다.
- 자연 가림: 쥐 발 접지점이 오브젝트 뒤에 있을 때 오브젝트의 수직 전면이 쥐를 가리는 것은 허용한다. 단, 최종 합성 화면에서 머리·몸통·발이 오브젝트 양쪽의 분리된 조각으로 동시에 보이면 실패다.
- 꼬리: 수직 전면 뒤로 자연스럽게 가려질 수 있지만 solid footprint 내부를 통과하거나, 같은 프레임에서 벽의 반대쪽까지 이어져 오브젝트를 관통하는 것처럼 보이면 실패다. 허용 가림에서는 보이는 꼬리 조각이 쥐 본체 쪽과 공간적으로 일관되거나 완전히 가려져야 한다.
- 앞/뒤 전환: 단일 스프라이트 전체 order를 유지하는 경우 한 프레임에서 전체 쥐가 앞 또는 뒤 중 하나여야 한다. 별도 마스크·분리 렌더를 도입하려면 위 연속 실루엣 규칙을 만족하고 새 아트 생성 없이 기술 샘플 범위 안에서만 적용한다.

## 검증 매트릭스

- 대상: `WallStraight_Occlusion`, `Barrel_A`, `Crate_A`
- 접근: 앞, 뒤, 좌, 우, 좌앞, 우앞, 좌뒤, 우뒤의 8경로
- 프레임: neutral, contact, passing의 사용 프레임 전부
- 단계: 접촉 직전, 최초 접촉, 접촉 유지, 후퇴, 같은 모서리 짧은 반전 2회
- 각 표본 기록: 쥐 위치, collider distance/overlap, sorting order, 가시 alpha와 solid footprint의 최소 투영 여유, 최종 화면 분리 실루엣 여부
- 실패 조건: 머리·몸통·발 solid 교차, 꼬리의 반대편 관통, 1픽셀 미만 여유 반복, order 2회 이상 점멸, 정지 jitter, 물리 overlap

## 금지 범위

- Stage2·Stage3·`RatHost2DPrototype` 코드·씬·테스트 변경
- `ProjectSettings.asset`, `Physics2DSettings.asset` 변경
- `_workspace/previews/`, `Builds/`, 반려된 규격 시험 산출물, Python 캐시 변경
- 쥐 또는 환경 아트 재생성, HUD 재디자인, 전체 8방향·전체 타일셋 확장
- 패키지 추가, 렌더 파이프라인·PPU·기준 해상도 변경
- 사용자 소유 `docs/references/images/image.png` 수정 또는 이동

## 완료 기준

- 사용자 화면 위치와 검증 매트릭스 전 표본에서 머리·몸통·발의 가시 alpha와 불투과 solid footprint 교차가 `0`이다.
- 꼬리는 허용된 수직 전면 가림 외에 solid footprint를 통과하거나 오브젝트 반대편으로 이어져 보이는 표본이 `0`이다.
- 의도된 앞/뒤 가림은 유지하되, 한 프레임 안에서 쥐의 머리·몸통·발이 오브젝트 양쪽의 분리된 조각으로 보이는 표본이 `0`이다.
- 접촉 시 최소 시각 여유가 `1 logical pixel` 이상이며 과도한 빈 간격 여부를 전후 캡처로 대조한다.
- 모서리 왕복과 짧은 방향 반전에서 sorting order 반복 점멸·jitter가 없다.
- 관련 및 전체 EditMode, Unity MCP Play, Console 0, scene clean과 보호 diff를 독립 QA가 확인한다.
- 프로젝트 총괄 관리자 판정이 `내부 승인 가능`이어야 사용자 재확인 단계로 넘긴다.

## 구현 역할 경계

- builder의 collider·sorting 직렬화 값, 씬 rebuild/save, 통합 테스트 변경은 Unity 씬/통합 구현 에이전트가 담당한다.
- `YSortSprite2D.cs` 또는 이동·정렬 런타임 동작 자체를 변경해야 할 경우 게임플레이 구현 에이전트로 조건부 인계하고 `agent-activity.md`에 변경 주체를 기록한다.
- 메인 조정자는 코드·씬·테스트를 직접 수정하지 않는다.

## 작업 순서

1. 비주얼/테크아트 에이전트가 alpha body·접지·꼬리와 최소 clearance 기준을 수치화한다.
2. Unity 씬/통합 구현 에이전트가 기준을 반영해 최소 수정한다.
3. 런타임 정렬 코드 변경이 필요할 때만 게임플레이 구현 에이전트가 담당한다.
4. QA/검증 에이전트가 검증 매트릭스와 Unity MCP Play를 독립 수행한다.
5. 프로젝트 총괄 관리자 에이전트가 최종 내부 승인 판정을 내린다.

## 승인 경계

- 현재 승인된 2D 기술 샘플의 사용자 지적 결함 수정이므로 구현 진행은 승인된 범위다.
- 새 아트, 전체 8방향, 전체 프로토타입 적용으로 확장하려면 별도 사용자 승인이 필요하다.
