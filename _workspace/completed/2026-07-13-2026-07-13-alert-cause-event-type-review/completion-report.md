# 완료 보고: 원인 라벨 enum/event type 전환

## 결과

면역 경계도 원인 분기를 UI 표시 문구에서 분리했다.

- `ImmuneAlertCauseType`과 불변 `ImmuneAlertEvent`로 내부 미니게임 선택의 입력을 명시했다.
- `FeedbackLabel`은 HUD 표시·누적만 맡고, 미니게임 선택은 `CauseType`만 사용한다.
- 기존 `면역 신호`·`경보`는 `ImmuneSignalOrAlarm`으로 명시 보존해 면역 신호 억제로 진입한다.
- 기존 오염·면역 포착·바이러스 흔적의 백혈구 회피와, 강제 조종·소음/조직 자극의 신호 억제 매핑을 보존했다.
- 씬·프리팹·Inspector 직렬화·ProjectSettings·패키지는 변경하지 않았다.

## 검증

- Unity batch EditMode: **90/90 통과**, 종료 코드 0.
- Unity MCP Play: `ImmuneSignalOrAlarm`의 면역 신호·경보 라벨이 신호 억제로, `Unspecified`가 설정 기본값으로 진입함을 재현했다.
- 타입이 같은 경우 표시 라벨을 바꿔도 매핑이 바뀌지 않음을 확인했다.
- Unity Console Error/Warning: 0건.
- 프로젝트 총괄 관리자 판정: **내부 승인 가능**.

## 남은 확인

- 사용자 플레이에서 면역 경계도 피드백 문구의 이해도와 2단계 리듬 HUD 체감 확인.

## 커밋

- 요청되지 않아 생성하지 않았다.
