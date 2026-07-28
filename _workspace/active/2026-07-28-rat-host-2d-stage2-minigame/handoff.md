# 작업 인계

## 최신 사용자 요청

2단계 2D 백혈구 회피 미니게임과 성공·실패 복귀 작업을 진행한다.

## 현재 상태

- 구현·통합 완료
- 신규 Stage2 EditMode `10/10`
- 전체 EditMode 최종 `186/186`
- 임시 복제본 Stage2 씬 논리 계약 2회 PASS
- Windows Development 빌드 성공 기록 완료, 사용자 정리 요청으로 임시 실행본 삭제
- 원본 Unity 외부 씬 변경 모달과 Stage1 씬 유지로 MCP Play·Console 차단

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

1. 사용자가 원본 Unity 외부 변경 모달을 안전하게 해제한다.
2. 원본 Unity에서 Stage2 Rebuild를 실행하고 `sceneDirty=false`를 확인한다.
3. MCP Play로 Host/Virus 입력 배타, 벽·Trigger, 카메라, 성공·실패·재진입과 Console을 확인한다.
4. 원본 검증 통과 후 필요할 때만 새 임시 빌드를 생성해 사용자 수동 플레이에 인계한다.
