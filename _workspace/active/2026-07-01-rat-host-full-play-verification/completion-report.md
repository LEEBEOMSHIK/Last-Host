# 완료 보고

## 작업 ID

2026-07-01-rat-host-full-play-verification

## 현재 판정

빌드 실행본 성공 루프 최종 확인 완료 / 사용자 체감 확인 별도.

이번 재정리와 후속 확인으로 Editor Play 기준 전체 성공 루프와 실패 복귀 루프, Windows 빌드 실행본 구동, 16:9 해상도별 기본 화면, 빌드 실행본 실패 복귀, 빌드 실행본 성공 루프를 확인했다.

빌드 실행본 성공 루프는 기존 F6 디버그 진입의 `면역 신호 억제` 경로로 확인했다. 자연 경계도 100% 도달 후 기본 `백혈구 회피`에서 조각 3개를 직접 수집하는 빌드 실행본 루프는 별도로 주장하지 않는다.

## 완료로 주장할 수 있는 항목

- Unity Editor Play에서 쥐 모드 시작, 면역 경계도 100%, 내부 바이러스 백혈구 회피, 변이 조각 3개 수집, 변이 선택, 쥐 모드 복귀 통과.
- Unity Editor Play에서 내부 바이러스 실패, 실패 패널, 보상 없는 쥐 모드 복귀 통과.
- Unity Console Error/Warning 0건 확인.
- Windows 빌드 완료 로그와 최신 Managed 산출물 확인.
- Windows 빌드 실행본이 `1920x1080`, `1600x900`, `1366x768`, `1280x720`에서 창으로 구동됨.
- Player.log에서 `Exception`, `Error`, `Crash` 패턴 없음.
- 기준 16:9 해상도에서 RatHost 기본 화면의 HUD/카메라 핵심 가림 문제 없음.
- 1280x720 빌드 실행본에서 F6 내부 미니게임 진입, 실패 패널, Space 복귀 확인.
- 1280x720 빌드 실행본에서 F6 내부 미니게임 진입, `면역 신호 억제` 성공, 변이 선택 화면, `잠복 강화` 선택, RatHost 복귀 확인.

## 완료로 주장하지 않는 항목

- 자연 경계도 100% 도달 후 기본 `백혈구 회피`에서 조각 3개를 직접 수집하는 빌드 실행본 루프 완료 주장.
- 실제 사용자 조작감, 난이도, 설명 없이 목표 이해 여부.
- Unity Test Runner 정식 결과 파일 통과.

## artifacts

- `artifacts/build-rathost-after-w-1920x1080.png`
- `artifacts/build-topmost-rathost-after-w-1600x900.png`
- `artifacts/build-topmost-rathost-after-w-1366x768.png`
- `artifacts/build-topmost-rathost-idle-1280x720.png`
- `artifacts/build-1280x720-after-f6.png`
- `artifacts/build-1280x720-after-missed-f6.png`
- `artifacts/build-1280x720-after-failure-space.png`
- `artifacts/build-success-loop-start-20260710-224037.png`
- `artifacts/build-success-loop-signal-20260710-224037.png`
- `artifacts/build-success-loop-mutation-20260710-224037.png`
- `artifacts/build-success-loop-return-20260710-224037.png`
- `artifacts/player-success-loop-20260710-224037.log`
- `artifacts/player-*.log`
- `artifacts/player-topmost-*.log`

## 남은 선택지

1. 사용자가 빌드 실행본을 직접 플레이하며 조작감, 난이도, 설명 없이 목표 이해 여부를 확인한다.
2. 더 엄격한 빌드 검증이 필요하면 자연 경계도 100%와 기본 `백혈구 회피` 조각 수집 루프를 별도 수동 검증한다.

## 코드/씬/설정 변경

없음. 빌드 과정에서 생긴 Unity 자동 직렬화/설정 변경은 되돌렸다.
