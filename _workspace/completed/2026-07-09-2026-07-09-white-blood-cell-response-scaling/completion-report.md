# 완료 보고: 백혈구 회피 대응 경험 스케일링

## 요약

기존 백혈구 회피 미니게임에 면역 대응 경험을 연결했다. 첫 내부 대응은 기존 백혈구 속도를 유지하고, 두 번째 이후부터 경험에 따라 백혈구 속도 배율이 소폭 증가한다. 실패한 내부 대응은 추가 경험을 남겨 다음 백혈구 회피 압박을 더 키운다.

## 변경 파일

- `UnityProject/Assets/_Project/Scripts/Core/PrototypeConfig.cs`
- `UnityProject/Assets/_Project/Scripts/Core/PrototypeSessionState.cs`
- `UnityProject/Assets/_Project/Scripts/VirusMinigame/WhiteBloodCellChaser.cs`
- `UnityProject/Assets/_Project/Scripts/VirusMinigame/VirusMinigameController.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
- `docs/design/encounters/internal-immune-response-minigame-types.md`
- `_workspace/active/2026-07-09-white-blood-cell-response-scaling/`

## 검증

- `git diff --check`: 통과, LF/CRLF 변환 경고만 있음
- Unity Test Runner EditMode: `LastHost.Prototype.Tests` 69개 통과, 실패 0, 스킵 0
- Unity MCP Play 체크:
  - 첫 진입 속도 배율 `1.0`
  - 두 번째 진입 속도 배율 `1.1`
  - 실패 후 다음 진입 속도 배율 `1.3`
  - 백혈구 `CurrentSpeed`에 세션 배율 반영
  - 성공/실패/복귀 루프 유지
- Unity Console Error/Warning 0건
- Play 종료 후 씬 `isDirty=false`

## 제외 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 기존 `APP_UI_EDITOR_ONLY` define 변경은 이번 작업 범위 밖이며 건드리지 않았다.
- 백혈구 수, 새 스폰 패턴, 씬 배치, ProjectSettings는 변경하지 않았다.

## 남은 위험

- Windows 빌드는 실행하지 않았다.
- 난이도 체감은 사용자가 직접 플레이하며 수치 조정이 필요할 수 있다.

## 총괄 판정

- 판정: 내부 승인 가능
- 이유: 승인된 쥐 숙주 1차 프로토타입 안의 기존 백혈구 회피 일부 수정이며, 새 시스템/씬/에셋/패키지를 추가하지 않았다. QA 검증 기록과 MCP Play 체크가 통과했다.
