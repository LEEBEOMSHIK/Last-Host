# 현재 세션 포인터

## 현재 이어받을 작업

- 작업 ID: `2026-07-24-rat-final-appearance-sample`
- 상태: r6 QA·총괄 통과 — 사용자 최종 외형 수용 대기
- 작업 경로: `_workspace/active/2026-07-24-rat-final-appearance-sample/`
- 최신 사용자 요청: neutral idle 수정과 QA·총괄 검토를 통과한 A2 Blender r6를 최종 외형 후보로 확인한다.

## 먼저 읽을 파일

1. `_workspace/active/2026-07-24-rat-final-appearance-sample/artifacts/ai-concepts/rat-concept-a2-refined.png`
2. `_workspace/active/2026-07-24-rat-final-appearance-sample/artifacts/a2-blender-revision-6/rat-final-appearance-a2-r6-contact-sheet-2048.png`
3. `_workspace/active/2026-07-24-rat-final-appearance-sample/artifacts/a2-blender-revision-6/rat-final-appearance-a2-r6-turnaround-preview-2048.png`

## 바로 이어서 할 작업

1. A2 참고안과 r6 비교표·턴어라운드를 대조해 최종 외형 수용 여부를 결정한다.
2. 몸통의 캡슐/패널형 띠와 큰 귀·어두운 얼굴 대비를 수용할지 확인한다.
3. 사용자 결정 전 active를 유지하고 전체 64프레임·atlas·Unity 반입을 진행하지 않는다.

## 승인 경계

- 완료: A2 기반 Blender r6 제작, neutral idle 수정, 독립 QA 통과.
- 총괄 판정: `사용자 제시 가능 / 최종 외형 승인 후보 / 사용자 결정 필요`.
- 정리 이력: r1~r5 중간 바이너리는 삭제·커밋 제외했고 반려 사유만 문서로 보존한다. A/B/C와 기존 v1·v2는 읽기 전용 기준으로 유지한다.
- 미승인: A2 또는 이번 샘플의 최종 런타임 스프라이트·최종 제품용 8방향 시트 확정, 전체 64프레임, runtime atlas/스프라이트 시트 구성, Unity 반입.

## 병행 차단 작업

- `2026-07-16-natural-alert-build-loop-verification`: Computer Use 게임 창 캡처 오류로 QA `차단`·총괄 `보류`.
- 재개 조건: Windows 게임 창 캡처 지원 복구 또는 사용자의 같은 연속 루프 단계별 화면과 해당 세션 `Player.log`.

## 제외하거나 건드리면 안 되는 변경

- Unity 코드·씬·Sprite Import·ProjectSettings·패키지와 `Builds/`를 변경하지 않는다.
- 기존 completed 작업과 v1~v5b 원본·PNG를 수정하지 않는다.
- `UnityProject/ProjectSettings/ProjectSettings.asset`의 기존 범위 밖 변경과 `_workspace/previews/`를 보존한다.

## 갱신 정보

- 마지막 갱신: 2026-07-27 KST
- 갱신자: 문서/릴리즈 에이전트
