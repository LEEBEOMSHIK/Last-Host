# 프로젝트 총괄 관리자 검토

상태: **내부 승인 가능 — 실제 RGBA 게임 에셋 1차 묶음 사용자 확인 가능**

## 검토 대상

- `task.md`, `handoff.md`
- 승인된 환경·쥐·HUD 품질 마스터와 `quality-rubric.md`
- `artifacts/source-generation-log.md`
- `artifacts/production-quality-rubric.md`
- `artifacts/asset-manifest.md`
- `artifacts/production-log.md`
- `artifacts/visual-review.md`
- `verification.md`
- source master 4종, 핵심 프리뷰 4종과 대표 실제 RGBA 원본

## 총괄 판정

`내부 승인 가능 — 실제 RGBA 게임 에셋 1차 묶음 사용자 확인 가능`

승인된 고품질 마스터를 환경 반복 타일·소품, 쥐 측면 보행 3프레임,
HUD 독립 모듈로 재제작한 현재 묶음은 사용자에게 1차 실제 게임 에셋
후보로 제시할 수 있다.

source 환경·소품·쥐·HUD는 모두 분리 가능한 품질을 유지했고, 실제
환경·쥐·HUD도 마스터 대비 명백한 품질 등급 하락이나 자동 반려 요소가
없다. 최초 HUD 상태 프리뷰에서 red/teal fill이 표시되지 않았던 조립
blocker는 최종 `hud_states.png`에서 해소됐다.

이 판정은 `전체 최종 에셋`, `최종 PPU·셀·피벗 확정`, `Unity 반입`,
`플레이 품질`, `Windows 빌드 완료`를 의미하지 않는다.

## 직접 시각 대조

### source master — 4종 PASS

- 환경: clean/worn 바닥, straight/corner 벽, water center/edge가
  비접촉 상태로 완전히 보이며 같은 2:1 축·벽돌 크기·좌상단 광원과
  석재·이끼·수면 재질 밀도를 유지한다.
- 소품: 통·상자·배수구가 잘림이나 융합 없이 분리되고, 목재·철재와
  최심부가 환경 마스터와 같은 등급으로 읽힌다.
- 쥐: neutral/contact/passing이 같은 낮고 긴 자연형 갈색 개체로
  읽힌다. 꼬리·발가락·수염이 완전하고 포즈 차이는 다리 위상 중심이다.
- HUD: 초상, 황동 초상 프레임, 공용 bar frame, red/teal fill이
  독립적으로 분리 가능하며 얇은 베벨·마모·분절을 보존한다.

따라서 source 단계 재생성 blocker는 없다. 단, 네 보드는 크로마가 있는
재제작 입력이며 그 자체를 게임 에셋으로 승인한 것은 아니다.

### 환경 실제 RGBA — PASS

- `environment_repeat_checker.png` 원본 배율에서 clean 4×4,
  clean/worn 4×4, water 4×4의 외곽 이음선·알파 구멍·명암 점프가
  보이지 않는다.
- clean과 worn은 같은 축과 재질 계열을 유지하면서 이끼·균열 정도가
  구분된다. 수면은 셀 경계보다 청록 깊이와 잔물결이 먼저 읽힌다.
- 실제 `floor_worn`, `wall_corner`, `prop_barrel` RGBA 원본에서
  석재 줄눈·벽돌 행·하단 이끼·나무결·금속 띠가 단순 도형으로
  축소되지 않았고 실루엣도 깨끗하다.
- 마스터 비교에서는 무조명 조립본이 더 밝지만 재질 밀도와 아이소메트릭
  투시는 유지된다. 현 Unity 미반입 단계에서 조명 차이는 반려 사유가
  아니다.

### 쥐 실제 RGBA — PASS

- `rat_actual_size.png`와 대표 contact RGBA를 원본 배율로 확인했다.
  세 프레임은 같은 코끝–엉덩이 길이, 낮은 등선, 복부 깊이, 머리·귀·
  꼬리 비율을 유지하고 발 위상으로 구분된다.
- visible 폭 `238/238/238`, 높이 `74/76/73`, 최대/최소 높이 비율
  `1.041`, 공통 접지선 top y=`152`, pivot `(128,40)` 기록이 실제
  시각적 안정성과 일치한다.
- 털 흐름, 귀 안쪽, 눈·코, 수염, 발가락과 가는 꼬리가 실제 캔버스에서도
  남아 있다. 프레임별 확대·축소 또는 다른 개체로 보이는 blocker가 없다.

### HUD 실제 RGBA — PASS

- `hud_states.png`에서 red full, teal half, empty와 동일 상태의 nearest
  50% 표시를 모두 직접 확인했다.
- red/teal fill은 공용 프레임 안에 맞고 끝캡·베벨을 침범하지 않는다.
  empty와 filled 상태는 색뿐 아니라 명도·분절·내부 하이라이트로도
  구분된다.
- 대표 bar frame과 health fill RGBA에서 얇은 황동 외곽, 어두운 금속
  슬롯과 발광형 채움이 보존되며 배경 박스·잘린 모서리·크로마 fringe가
  보이지 않는다.
- 최초 fill 미표시 blocker는 현재 파일에서 재현되지 않는다.

## 비주얼 판정 대조

```text
source master:
- 환경: SOURCE PASS
- 소품: SOURCE PASS
- 쥐: SOURCE PASS
- HUD: SOURCE PASS

game asset:
- 환경: PASS
- 쥐: PASS
- HUD: PASS

마스터 대비 품질:
- 명백한 등급 하락 없음

반복·투시·접지·HUD 조립:
- 1차 범위 PASS

자동 반려:
- 없음
```

비주얼/테크아트의 PASS 근거는 직접 확인한 현재 PNG와 일치한다.

## QA·기술 게이트 대조

- validator: `128/128 PASS`, 실패 `0`
- 공식 산출물 재빌드: `20/20` SHA-256 일치
- 실제 PNG 18개: RGBA·예상 크기·투명 모서리 통과
- 크로마 잔류: magenta `0/18`, green `0/18`
- 환경 반복: clean/worn/water 모두 visible component `1`, hole `0`
- 쥐 시트와 개별 셀 픽셀 mismatch: `0/0/0`
- HUD layout: fill offset `(56,14)`, fill size `400×52` 일치
- UnityProject 안 공식 후보 파일명 검색: `0개`

QA는 비주얼 품질을 기술 검사로 대체하지 않았고, 비주얼 PASS와 기술
PASS는 서로 독립적으로 충족됐다.

## Unity 검증 경계

Unity EditMode·Play Mode·Windows Build 미실행은 현 작업 범위에 맞다.
현재 에셋은 `_workspace` 아래에만 있고 `UnityProject/`에는 반입되지
않았다. 따라서 Unity 검증을 실행해도 현재 후보의 실제 import·sorting·
가림·충돌·Pixel Perfect·UI scale 품질을 증명하지 못한다.

다음 단계는 사용자 수용 뒤 별도 승인된 `Unity 반입 기술 샘플`이어야 한다.
그 단계에서 Point/mipmap off, PPU·셀 크기, pivot·접지, Y축 sorting,
벽·소품 가림과 충돌, HUD safe area·fill masking·9-slice, 실제 이동 중
픽셀 안정성을 플레이로 검증한다.

## 사용자 확인 파일

전체 작업 패킷을 열어볼 필요 없이 다음 네 PNG만 확인하면 된다.

1. `artifacts/previews/master_asset_comparison.png`
   - 승인 마스터와 실제 조립본의 품질 등급 차이
2. `artifacts/previews/environment_repeat_checker.png`
   - 바닥·오염·수면의 4×4 반복 이음과 반복 주기
3. `artifacts/previews/rat_actual_size.png`
   - 실제 크기의 동일 개체성, 세 보행 위상과 공통 접지
4. `artifacts/previews/hud_states.png`
   - 수정된 full/half/empty 및 50% HUD 가독성

## 남은 위험

- 환경은 clean/worn/water 변형 수가 적어 큰 맵에서는 반복 주기가
  눈에 띌 수 있다.
- 쥐는 측면 1방향·3프레임뿐이며 전체 방향, 이동 속도와 프레임 타이밍,
  Point 출력의 픽셀 안정성은 미검증이다.
- `128×64`, `256×192`, pivot `(128,40)`, 50% 표시는 후보 규격이며
  최종 PPU·셀·화면 점유율이 아니다.
- 벽·소품의 Y축 sorting·가림·충돌은 실제 씬에서 확인하지 않았다.
- HUD safe area, UI scale, masking·9-slice는 실제 Canvas에서
  확인하지 않았다.
- cleaned board 이후 공식 20파일은 결정적으로 재생성되지만, imagegen
  RGB 원본에서 cleaned board를 만드는 크로마 제거 단계는 공식 build
  명령에 포함되지 않는다.
- 현재 묶음은 전체 8방향 쥐, 전체 하수도 타일 변형, 최종 UI 전체가 아니다.

## 다음 승인

사용자가 위 네 프리뷰의 품질을 수용하면 `Unity 반입 기술 샘플`을
별도 작업으로 승인받는다. 수용 전에는 현재 후보를 Unity에 넣거나
전체 방향·타일셋·UI로 확장하지 않는다.
