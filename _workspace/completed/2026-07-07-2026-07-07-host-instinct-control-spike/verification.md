# 검증 기록

## 작업 ID

2026-07-07-host-instinct-control-spike

## 검증 대상

숙주 본능과 강제 조종 스파이크 코드/씬 변경, 내부 미니게임 실패 후 보상 없는 쥐 숙주 복귀, QA/검증 MCP 플레이 체크 운영 문서 변경

## 검증 담당

QA/검증 에이전트

## 검증 에이전트 수행 이력

- 검증 에이전트: QA/검증 에이전트
- 검증 요청자: 프로젝트 조정 에이전트
- 검증한 산출물: 강제 조종 상태 모델, 위험 구역 연결, EditMode 테스트, `RatHostPrototype.unity` 위험 구역 Rigidbody 보정
- `agent-activity.md` 반영 여부: 예

## 입력 자료

- `docs/prototype/plans/rat-host-immune-alert-work-direction.md`
- `docs/design/hosts/host-instinct-control.md`
- `docs/design/ui-feedback/immune-alert-feedback.md`
- `UnityProject/Assets/_Project/Scripts/Host/HostInstinctControlSpike.cs`
- `UnityProject/Assets/_Project/Scripts/Host/RatHostController.cs`
- `UnityProject/Assets/_Project/Scripts/Immune/ImmuneRiskZone.cs`
- `UnityProject/Assets/_Project/Tests/EditMode/RatHostPrototypeCoreTests.cs`
- `UnityProject/Assets/_Project/Scenes/RatHostPrototype.unity`

## 원래 증상 또는 완료 주장

- 일반 이동에는 면역 경계도 상승을 주지 않는다.
- 위험 구역 근처에서 위험 중심을 향한 입력이 설정 시간 이상 반복될 때만 `강제 조종 +8` 피드백을 발생시킨다.
- 강제 조종 스파이크는 입력 지연, 반동, 조작 방해, 별도 게이지를 추가하지 않는다.

## 독립 검증 여부

- 구현 주체와 검증 주체 분리 여부: 역할 기준으로 분리. 도구 실행은 Codex 메인 에이전트가 대행했다.
- 구현 주체가 실행한 검증과 별도로 확인한 항목: RED 컴파일 오류, GREEN 전체 EditMode 결과 XML, Unity Console 오류/경고 여부, 실패했던 씬 테스트 원인 확인

## 실행한 검증

```text
명령 또는 확인 방법:
열린 Unity 에디터에서 TestRunnerApi로 LastHost.Prototype.Tests EditMode 테스트 실행

결과:
40 total / 40 passed / 0 failed / 0 skipped
결과 파일: _workspace/completed/2026-07-07-2026-07-07-host-instinct-control-spike/editmode-green-results.xml

해석:
신규 강제 조종 테스트 3개와 기존 핵심 EditMode 테스트가 모두 통과했다.
```

```text
명령 또는 확인 방법:
코드 리뷰 후 런타임 근접 경로 테스트를 추가하고 Unity 안에서 RatHostPrototypeCoreTests 전체를 직접 순회 실행

결과:
41 total / 41 passed / 0 failed
결과 파일: _workspace/completed/2026-07-07-2026-07-07-host-instinct-control-spike/editmode-direct-green-summary.txt

해석:
`ImmuneRiskZone_NearbyRatInputTowardZoneTriggersForcedControlFeedback`까지 포함해 최신 테스트 메서드가 모두 통과했다. 공식 Test Runner XML은 추가 테스트 후 갱신되지 않아 보조 검증으로 기록한다.
```

```text
명령 또는 확인 방법:
Unity Console 조회

결과:
오류 0건, 경고 0건. Test Runner 실행 로그만 남음.

해석:
컴파일 오류와 obsolete 경고가 정리됐다.
```

```text
명령 또는 확인 방법:
RED 확인

결과:
신규 테스트 추가 직후 Unity Console에서 HostInstinctControlSpike 타입 부재, ImmuneRiskZone 강제 조종 API 부재, 런타임 근접 경로 API 부재 컴파일 오류 확인.

해석:
테스트가 구현 부재를 실제로 잡는 것을 확인했다.
```

```text
명령 또는 확인 방법:
커밋 전 Unity MCP 직접 실행으로 RatHostPrototypeCoreTests 전체 public NUnit 테스트 순회

결과:
47 total / 47 passed / 0 failed

해석:
숙주 본능, 강제 조종, 실패 UI, 보상 없는 실패 복귀, 씬 라벨 검증을 포함한 최신 EditMode 테스트가 모두 통과했다.
```

```text
명령 또는 확인 방법:
Unity MCP Play 체크

결과:
- `RatHostPrototype.unity` 로드 성공
- Play 모드 진입/종료 성공
- 시작 상태 `RatHost` 확인
- 실패 패널 시작 비활성 확인
- 면역 경계도 강제 상승으로 내부 바이러스 모드 전환 확인
- 바이러스 안정도 소진 실패 상태 전환 확인
- 실패 UI 제목 `면역 반응 돌파 실패` 확인
- 실패 UI 목표 `보상 없이 쥐 숙주로 복귀` 확인
- 기존 버튼 연결 메서드 `RetryVirusMinigame()` 호출 시 보상 없이 쥐 숙주 복귀 확인
- 실패 복귀 후 변이 보상 없음 확인
- 실패 복귀 후 면역 경계도 60 확인

해석:
사용자 최종 확인 전 사전 MCP 플레이 체크 기준을 통과했다.
```

```text
명령 또는 확인 방법:
커밋 전 Unity Console Error/Warning 조회

결과:
오류 0건, 경고 0건.

해석:
컴파일, Play 체크, 테스트 실행 후 콘솔에 남은 Error/Warning이 없다.
```

## 검증하지 못한 항목

- 실제 사용자가 키보드로 위험 구역을 향해 반복 입력할 때 조작감이 거슬리지 않는지 최종 수동 확인하지 않았다.
- Windows PC 빌드는 실행하지 않았다.
- 코드 리뷰에서 지적된 씬의 `session` 참조 `{fileID: 0}` 상태는 테스트 통과와 런타임 AutoWire 동작으로 즉시 차단하지 않았지만 별도 씬 정리 확인 대상이다.

## 실패 또는 경고

- Unity batchmode EditMode 테스트는 라이선스 클라이언트 초기화 지연으로 결과 XML을 생성하지 못했다.
- 첫 GREEN 전체 테스트에서 `ToxicWaterRiskZone` Rigidbody 누락으로 기존 씬 테스트 1개가 실패했다. Unity 씬/통합 구현 에이전트 기준으로 `isKinematic` Rigidbody를 복구한 뒤 전체 테스트가 통과했다.
- `ProjectSettings.asset`의 `APP_UI_EDITOR_ONLY` define은 현재 작업트리에 남아 있으나 이번 작업 범위 밖 기존 변경으로 분류했다.

## 게이트 판정

- QA/검증 게이트 통과 여부: 통과
- `agent-activity.md`에 QA 판정 반영 여부: 예
- 총괄 관리자 검토로 넘길 수 있는지: 예

## 완료 판단

완료 가능

## 완료 판단 근거

- 코드와 EditMode 테스트/보조 직접 실행 기준으로 요구 동작은 검증됐다.
- Unity MCP Play 체크로 씬 시작 상태, 실패 UI, 보상 없는 실패 복귀, 콘솔 상태를 확인했다.
- 사용자의 최종 수동 플레이와 PC 빌드는 별도 최종 확인 또는 빌드 검증 항목으로 남긴다.
