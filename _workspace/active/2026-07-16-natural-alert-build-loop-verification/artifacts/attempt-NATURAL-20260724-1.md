# Windows 빌드 엄격 검증 시도 1

- 시도 ID: `NATURAL-20260724-2026-07-24T02-05-04-587Z`
- 실행본: `C:\project\Last-Host\Builds\RatHostPrototype\LastHostPrototype.exe`
- 실행본 SHA-256: `65D646C9285BF2CBBAB784992E3AD5AE9012BEF5E8A6B4FFA46209592AF9DDA2`
- 프로세스 ID: `73708`
- 프로세스 시작: `2026-07-24 11:05:11 KST`
- 반환된 게임 창: app `process:C:\project\Last-Host\Builds\RatHostPrototype\LastHostPrototype.exe`, id `15532732`, title `Last Host`
- 종료: Computer Use `Alt+F4`, `2026-07-24 11:06 KST`
- 종료 확인: 게임 창 0개, `LastHostPrototype` 프로세스 0개

## 단계별 결과

| 단계 | 결과 | 근거 |
| --- | --- | --- |
| Computer Use 연결 | 통과 | 표준 bundled client `list_apps`, `list_windows` 정상 응답 |
| 새 빌드 실행·단일 창 식별 | 통과 | 실행 전 대상 게임 창 없음, 실행 후 정확히 1개 `Last Host` 창 반환 |
| 시작 화면 캡처 | 차단 | `get_window_state(include_screenshot:true)`가 `SetIsBorderRequired failed: 해당 인터페이스를 지원하지 않습니다. (0x80004002)` |
| 규정된 창 재선택·캡처 복구 1회 | 실패 | 새 `list_windows` 반환 객체로 `get_window` 후 재캡처했으나 같은 오류 |
| 실제 플레이 입력 | 미실행 | 화면을 보지 않은 입력은 엄격 증거가 아니므로 중단. `F6`·상태 주입 없음 |
| 자연 경계도 100% 이후 전체 루프 | 미검증 | 시작 화면 캡처 단계에서 차단 |
| 정상 종료 | 통과 | 정확한 대상 창에 `Alt+F4`, 즉시 창 목록과 프로세스 종료 확인 |
| 시도 로그 보존 | 통과 | `Player-NATURAL-20260724-attempt-1.log`, SHA-256 `D6634589E1E1B4EE5763598937099ED474857F0909CF18F851843A6726D5B2C9` |

## 로그 해석

- `Exception`, `Error`, `Crash`, `fatal`, `abort` 일치 없음.
- `failed` 일치 1건: `d3d12: failed to query info queue interface (0x80004002).`
- 로그에는 엔진 초기화, 입력 초기화, PhysX 초기화, `Alt+F4` 뒤 Physics/Input/CodeReloadManager 종료가 순서대로 남아 있다.
- 위 D3D12 메시지는 이번 짧은 실행을 중단시키지 않았지만, 핵심 플레이 단계가 실행되지 않았으므로 전체 루프 안전성 근거로 확장하지 않는다.

## 증거 한계

Computer Use 화면 캡처 API가 게임 창에서 실패해 단계별 이미지를 저장할 수 없었다. 반환 창·프로세스·시각·로그는 같은 시도 ID로 연결되지만, 시작 HUD부터 복귀까지의 시각 증거는 없다. 이 시도는 엄격 성공 시도로 사용할 수 없다.
