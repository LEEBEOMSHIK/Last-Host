# 작업 인계

## 최신 사용자 확인

원본 Game View가 검은 화면이 아니며 실제 이동하는 것을 확인했다.

## 현재 상태

- 구현·통합 완료
- 신규 Stage2 EditMode `10/10`
- 전체 EditMode 최종 `186/186`
- 임시 복제본 Stage2 씬 논리 계약 2회 PASS
- Windows Development 빌드 성공 기록 완료, 사용자 정리 요청으로 임시 실행본 삭제
- 원본 Unity Reload 차단 해제
- 원본 Stage2 씬 Rebuild·Save·디스크 재Load 완료
- Host Tilemap Floor `117`, Water `5`, Blocking wall `40`, 범위 `(-6,-4)..(6,4)`
- Host 카메라에서 바닥·외곽 경계·수로가 표시되어 맵 범위 식별 가능
- 독립 QA 원본 MCP Play에서 실패·복귀·재진입·성공 전체 대체 경로 통과
- Host 외곽·수로와 Internal 4벽 Physics2D 질의 통과
- 최종 Console Error/Warning `0`
- 프로젝트 총괄 관리자 `내부 승인 가능`
- 사용자 수동 확인: 검은 화면 해소·실제 이동 확인 완료
- 남은 사용자 확인: Space 실패 확인·Internal 화면 전환과 가독성

## 구현 경계

- 성공: 조각 3개 → `MutationSelection` 인계 셸
- 실패: 안정도 0 → 실패 UI → 확인 → 무보상 `RatHost`, 면역 60%
- 3단계로 유지: 실제 변이 선택·효과·성공 후 쥐 복귀

## 건드리면 안 되는 변경

- `UnityProject/ProjectSettings/ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY`
- `_workspace/previews/`
- 기존 3D 씬과 `RatHost2DTechnicalSample`
- Packages와 렌더 파이프라인

## 다음 작업

1. 사용자가 Game View에서 Space 실패 확인과 Internal 화면 가독성을
   확인한다. Windows 빌드는 사용자가 필요하다고 요청할 때만 만든다.
2. 사용자 수용 뒤 Stage1·Stage2를 닫고 Stage3 후보를 별도 승인에 따라 진행한다.
