# 프로젝트 총괄 관리자 검토

상태: **내부 승인 가능 — 수정 화면과 실제 WASD 사용자 재확인 가능**

## 검토 대상

- `AGENTS.md`, `.agents/project-director-agent.md`
- `task.md`, `work-log.md`, `agent-activity.md`, `handoff.md`
- `artifacts/occlusion-diagnosis.md`
- `artifacts/implementation-verification.md`
- `artifacts/visual-tech-postfix-review.md`
- `verification.md`
- `artifacts/game-view-occlusion-hud-corrected.png`
- `artifacts/hud-before-after.png`
- 수정 전·후 오클루전 캡처·CSV와 clean-clone EditMode XML
- `docs/project-handoff/current-task-board.md`
- `_workspace/active/CURRENT.md`
- Production2D 빌더·씬·테스트·HUD 제작 소스와 보호 설정 관련 diff

## 판정

`내부 승인 가능 — 수정 화면과 실제 WASD 사용자 재확인 가능`

사용자가 지적한 벽·통 통과 가림과 HUD 초상 상단 잔여 그래픽은 승인된
최소 범위 안에서 원인을 분리해 수정됐다. HUD 수정은 지정된 상단 황동
컴포넌트만 제거했고, 가림 수정은 정적 오클루더의 전환선과 접지
footprint만 정규화했다. 비주얼 재검토와 독립 clean-clone QA도 통과했다.

현재 자동·대체 검증에서 재수정할 blocker는 없다. 단일 SpriteRenderer가
전환되는 실제 체감과 모서리에서의 짧은 방향 반전은 정지 캡처나 스크립트식
이동으로 완전히 대체할 수 없으므로, 실제 Game View 포커스의 네이티브
WASD는 사용자 확인 항목으로 남긴다.

## 방향·범위·승인 게이트

- 2D 아이소메트릭 도트, 발 접지 기반 Y 정렬, 2D 충돌 방향을 유지했다.
- 사용자 피드백 두 항목을 고치는 범위이며 새 기획·콘텐츠·아트 방향을
  추가하지 않았다.
- 쥐 외형·얼굴 재생성, 전체 HUD 재디자인, 전체 8방향, 전체 타일셋,
  패키지·렌더 파이프라인·Windows 빌드로 확장하지 않았다.
- Stage2·Stage3, `RatHost2DPrototype`, ProjectSettings 보호 범위는
  수정하지 않았다.
- 구현은 지정된 Unity 씬/통합 구현 담당과 2D 에셋 제작 담당이 수행했고,
  메인 조정자의 직접 구현 예외는 없다.

따라서 사용자 승인 범위와 에이전트 책임 경계를 준수했다.

## 원인과 최소 수정 대조

### 가림·충돌

수정 전 계약은 정적 오클루더별 tieBreak가 wall `3`, barrel `11`,
crate `12`로 달랐다. 현재 정렬식에서는 이 값이 단순 동률 해소를 넘어
전환선을 각각 최대 `0.12 world unit` 이동시켰다. 쥐·소품 collider도
실제 지면 점유부보다 좁아, 물리는 분리됐지만 긴 쥐 스프라이트와 오브젝트가
계속 겹치는 구간이 컸다.

적용된 수정:

- 정적 wall·barrel·crate tieBreak: 모두 `1`
- 쥐 CapsuleCollider 폭: `0.62 → 0.92`
- barrel footprint 폭: `0.48 → 0.60`
- crate footprint 폭: `0.55 → 0.70`
- wall footprint 폭·모든 높이·offset: 유지
- 별도 hysteresis·정렬 stride 재설계·마스크·스프라이트 분할: 미적용

수정 후 세 대상 모두 앞 분리에서는 쥐가 앞, 뒤 분리에서는 쥐가 뒤,
동일 pivot에서는 오브젝트가 쥐보다 정확히 `+1` 앞이다. 실제로 재현되지
않은 jitter를 가정해 새 시스템을 추가하지 않았고, 사용자 피드백 원인에
대응하는 최소 변경만 적용했다.

### HUD 초상

직접 확인한 `hud-before-after.png`에서 제거된 것은 초상 PNG 상단의
분리된 황동 조각 하나뿐이다.

- 변경 픽셀: `2,173`
- 변경 최대 Y: `26`
- `y >= 27`: byte-identical
- 가시 컴포넌트: `2 → 1`
- 다른 제작 에셋: `19/19` SHA 동일
- 보정 원본·제작 게임 에셋·Unity 반입본 SHA:
  `76BC4A430FC170C24C704CF54B2FAFC57EAED0CD2FE5DA5A0F52521F28371908`

쥐의 귀·눈·코·얼굴·털·수염·알파 외곽은 보존됐다. 새 이미지 생성이나
초상 리디자인이 아니라 분리·크롭 오류 수정이라는 작업 경계와 일치한다.

## 수정 화면 직접 검토

`game-view-occlusion-hud-corrected.png`와 수정 후 통·벽 앞뒤 캡처를
원본 배율로 직접 확인했다.

- HUD 초상 상단 이중 황동 장식이 사라지고 외곽 프레임만 남는다.
- 쥐 얼굴과 초상 품질, red/teal bar 상태가 유지된다.
- 통 앞 분리에서는 쥐가 통을 가리고, 뒤 분리에서는 통이 쥐 몸통을
  가려 앞뒤 관계가 올바르다.
- 벽 뒤 상태에서도 벽이 쥐의 중앙을 가리고 머리·꼬리는 벽 폭 밖에서
  보이므로 긴 실루엣의 공간 관계가 읽힌다.
- 수정된 전체 화면에서 새로운 알파 깨짐, HUD 재가림, 카메라 이탈,
  저품질 대체는 보이지 않는다.

정지 캡처의 강제 동일 pivot 상태는 collider overlap 상태라 실제 이동으로
도달하지 않는 진단 장면이다. 이를 플레이 화면 품질 blocker로 오인하지
않고, 실제 도달 가능한 앞·뒤 분리와 런타임 접촉 검증을 기준으로 판정했다.

## QA/검증 기록 확인

독립 QA 기록은 완료 주장에 충분하다.

### HUD·에셋

- 보정 3경로: RGBA `184×184`, SHA 일치
- 변경 `2,173px`, maxY `26`, `y >= 27` byte-identical
- 제작 해시 맵 `20/20`
- Unity PNG 반입본 `18/18`
- 대상 외 제작 에셋 `19/19`

### 씬·정렬·충돌

- tieBreak: 정적 오클루더 모두 `1`
- collider:
  - rat `(0.92,0.26)`
  - barrel `(0.60,0.22)`
  - crate `(0.70,0.24)`
  - wall `(1.05,0.18)` 유지
- 앞·뒤 분리 간격 `0.015`, overlap `false`
- 동일 pivot:
  - Barrel `75 < 76`
  - Crate `-25 < -24`
  - Wall `-75 < -74`
- MCP 스크립트식 접촉:
  - 세 대상 final distance `0.0006`
  - overlap `false`
  - 300회 정지 jitter `0`
- 이동 회귀: X `+0.72`, Y `0`
- 카메라 오차: `0.16px`

### 테스트·상태

- 독립 clean-clone 관련 EditMode: `44/44`
- 독립 clean-clone 전체 EditMode: `198/198`
- failed/skipped/inconclusive: 모두 `0`
- Console Error/Warning: `0`
- Stop 후 `RatHost2DTechnicalSample`, `sceneDirty=false`
- clean-clone Unity 잔류 프로세스: `0`
- `git diff --check`: PASS

QA가 구현자 결과와 같은 수를 별도 복제본에서 다시 얻었고 결과 XML을
보존했다. 따라서 구현 자체 검증에만 의존하지 않는다.

## MCP 플레이 체크 확인

총괄은 QA를 재실행하지 않고 기록을 대조했다.

- 원본 기술 샘플 Play·Stop: PASS
- 통·상자·벽 접촉 clamp와 overlap false: PASS
- 동일 pivot 앞뒤 order와 300회 정지 안정성: PASS
- HUD 런타임 경로와 크기: PASS
- 이동·카메라 회귀: PASS
- Console 0, scene clean: PASS

Game Camera 캡처는 런타임 instance ID 해석 오류로 한 차례 실패했으나
도구 오류였고, 구현 캡처·독립 파일 시각 대조와 fresh Console 0이
확보됐다. 이 한 차례의 캡처 도구 실패는 현재 완료 주장에 대한 blocker가
아니다.

스크립트식 이동 검증을 네이티브 WASD 통과라고 과장하지 않았으며 실제
키 입력 체감은 사용자 확인으로 분리했다.

## 보호 diff

기록된 보호 SHA는 구현 전 기준과 같다.

- `RatHost2DPrototype.unity`
- `RatHost2DPrototypeSceneBuilder.cs`
- `RatHost2DSessionController.cs`
- `ProjectSettings.asset`
- `Physics2DSettings.asset`

`Physics2DSettings.asset` diff는 없고, `ProjectSettings.asset`에는 작업
전부터 관리되던 `APP_UI_EDITOR_ONLY` define 차이만 있다. 이번 작업에서
생긴 예상 밖 ProjectSettings 직렬화 변경은 없다.

같은 worktree의 Stage2·Stage3·기존 Production2D 미커밋 변경은 이번 작업
소유가 아니므로 커밋 시 선별해야 한다.

## 상태판 사실성

초기 검토에서 다음 운영 문서 불일치를 발견했다.

- `handoff.md`가 독립 QA 대기로 남아 있었다.
- 현황판 다음 작업 후보가 QA 전 상태로 남아 있었다.

메인 조정자가 이를 다음처럼 수정했고 총괄이 재대조했다.

- handoff: 독립 `44/44`, 전체 `198/198`, 비주얼 PASS와
  총괄→사용자 WASD/HUD 확인 순서로 갱신
- 현황판 후보: `QA를 통과한 수정본 사용자 재확인`으로 갱신
- `CURRENT.md`: 작업 ID·경로·`QA 완료 — 총괄 검토 대기` 일치
- 현황판 현재 작업 행: 원인·수정값·비주얼·QA·사용자 확인 경계 일치
- Git 기준: `HEAD = origin/main = 73c5750`

운영 문서 blocker는 최종 판정 전에 해소됐다. 총괄 판정 반영 뒤 메인
조정자는 상태를 `내부 승인 가능 — 사용자 재확인 대기`로 동기화해야 한다.

## 수정 필요

현재 내부 승인을 막는 수정은 없다.

실제 WASD에서 다음 중 하나가 보이면 다시 `수정 필요`로 연다.

- 물리적으로 멈춘 뒤에도 쥐 몸통이 오브젝트 표면에 올라탄 듯 보임
- 모서리에서 짧게 방향을 반전할 때 앞뒤 순서가 반복 점멸함
- 확장된 collider 때문에 통·상자 옆 통로가 의도보다 과도하게 좁아짐

## 문제 사안

해소된 문제:

- handoff와 현황판 후보의 QA 상태가 실제 기록보다 뒤처져 있었다.
- 사용자에게 완료 상태를 잘못 전달하기 전에 동기화했다.

현재 미해소 내부 blocker는 없다.

## 사용자 결정 필요

사용자는 실제 Game View에서 다음을 확인해야 한다.

1. 통의 좌우 모서리를 앞→옆→뒤와 뒤→옆→앞으로 각각 통과
2. 상자와 벽에서도 같은 경로 반복
3. 접촉해 멈춘 상태에서 쥐가 표면 위에 올라탄 듯 보이는지 확인
4. 모서리에서 짧게 방향을 반전할 때 앞뒤가 두 번 이상 점멸하는지 확인
5. HUD 초상 상단 황동 조각이 사라지고 쥐 얼굴이 그대로인지 확인

정상이라면 사용자 피드백 두 항목은 화면상 종결 가능하다.

## 사용자에게 올릴 최소 확인 파일

다음 두 파일만 제시한다.

1. `artifacts/game-view-occlusion-hud-corrected.png`
   - 실제 수정 화면, HUD와 월드 조합 확인
2. `artifacts/hud-before-after.png`
   - 제거한 황동 조각과 쥐 본체 보존 확인

오클루전 진단 캡처·CSV·테스트 XML은 내부 근거이며 기본 사용자 확인
목록에 올리지 않는다. 가림 전환의 최종 체감은 정지 파일이 아니라 실제
WASD로 확인한다.

## 남은 위험

- 쥐는 긴 꼬리를 포함한 단일 SpriteRenderer라 몸 일부를 단계적으로
  마스킹하지 않는다.
- 모서리 연속 이동과 짧은 방향 반전 체감은 네이티브 WASD 미확인이다.
- 확대된 collider가 실제 좁은 통로에서 느끼는 이동 여유는 사용자 판단이
  필요하다.
- PPU 128·쥐·HUD 상대 크기 수용은 이전 Unity 샘플의 별도 사용자
  결정 항목이다.
- 전체 8방향, 전체 타일셋·HUD, Windows 빌드는 이번 범위 밖이다.

## 다음 단계

1. 메인 조정자가 총괄 판정을 현황판과 `CURRENT.md`에 반영한다.
2. 사용자에게 수정 Game View와 HUD 전후 비교 두 파일만 제시한다.
3. 사용자가 실제 WASD로 통·상자·벽 모서리 왕복과 짧은 방향 반전을
   확인한다.
4. 사용자 수용 시 이 피드백 작업을 완료 보관하고, PPU·전체 방향·전체
   아트 확장은 별도 승인 순서를 따른다.
