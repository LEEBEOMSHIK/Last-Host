# 작업: 쥐 걷기 애니메이션 Blender 시험 제작

## 상태

- 작업 ID: `2026-07-20-rat-walk-animation-blender`
- 상태: 차단 — Windows 앱 조작 연결 복구 또는 사용자가 저장한 Blender 원본 경로 필요
- 사용자 승인: 2026-07-20, 사용자가 Blender를 열어 둔 상태에서 쥐 걷기 애니메이션 작업을 명시 요청했다.

## 목적

기존 쥐 시험 원본을 기준으로 짧은 루프형 걷기 동작을 Blender에서 시험 제작하고, 8방향 프리렌더드 스프라이트로 출력할 수 있는 원본·프레임 기준을 마련한다. 이번 단계는 새 게임플레이나 숙주 콘텐츠를 추가하지 않는다.

## 입력 자료

- `AGENTS.md`
- `docs/prototype/official/rat-host-prototype.md`
- `docs/prototype/plans/rat-host-implementation-plan.md`
- `_workspace/completed/2026-07-16-2026-07-16-rat-8-direction-trial-asset/artifacts/source/rat-trial-v1.obj`
- 같은 완료 패킷의 `direction-map.csv`, `render-settings.json`, 정지 8방향 PNG

## 담당

- 비주얼/테크아트 에이전트: 스타일·프레임·접지 기준 검토
- Blender 시험 제작: Blender 애니메이션 테크아트 에이전트
  - 2026-07-20 역할 추가 이후의 Blender 원본·리깅·보행 수정·시험 렌더는 이 역할에 위임한다. 역할 추가 전 v1·v2 시험본을 메인 에이전트가 만든 이력은 `agent-activity.md`에 보존한다.
- Unity 통합: 이번 작업에서 수행하지 않음. 출력 수용 뒤 별도 Unity 통합 작업으로 분리한다.
- 독립 검증: QA/검증 에이전트
- 완료 판정: 프로젝트 총괄 관리자 에이전트

## 수용 기준

1. 쥐의 저폴리 원본과 제한 팔레트 방향을 유지한다.
2. 동일한 길이·위상의 짧은 루프 걷기 동작을 8방향에서 출력할 수 있다.
3. 프레임별 실제 발바닥 기준점이 공통 지면에 맞고, 불필요한 수직 튐이 없다.
4. 정지 8방향과 다른 파일명·출력 폴더를 사용해 기존 Unity 자산을 덮어쓰지 않는다.
5. Blender 원본·렌더 설정·출력 프레임과 검증 근거를 작업 패킷에 기록한다.

## 금지 범위

- `UnityProject/` 코드·씬·Sprite Import·ProjectSettings·패키지 변경
- 새 숙주, 캠페인, 인간 단계, 바이러스·백혈구 애니메이션 추가
- 기존 정지 8방향 자산 덮어쓰기
- `.codex/config.toml`, `_workspace/previews/` 수정

## 승인 필요 항목

- 이번 Blender 시험 제작과 신규 걷기 프레임 생성은 사용자 명시 승인됨.
- Unity 런타임 통합, 걷기 프레임 수/재생 속도 확정, 기존 정지 자산 교체는 별도 사용자 승인 필요.
