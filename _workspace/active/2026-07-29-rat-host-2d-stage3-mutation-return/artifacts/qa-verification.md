# Stage3 독립 QA

## 범위와 판정

- 대상: `Assets/_Project/Scenes/RatHost2DPrototype.unity`
- 일자: 2026-07-29
- 판정: `조건부 통과`
- 빌드: 실행하지 않음

원본 Unity MCP에서 씬 계약과 세 변이·실패 경로를 독립 확인했다.
제품 오류는 발견하지 않았다. 독립 전체 EditMode 재실행과 실제 OS
키보드·마우스 수용 확인만 남는다.

## 정적 씬 계약

```text
scene dirty=false
missing scripts=0
FloorTilemap=117 cells, occupied=(-6,-4)..(6,4)
WaterTilemap=5 cells, occupied=(3,-2)..(3,2)
BlockingTilemap=40 cells, occupied=(-6,-4)..(6,4)
MutationSelectionShell2D=존재/초기 비활성
mutation buttons=3
button adapters=3
types=Dormancy, NeuralControl, MammalAdaptation
Mammal PassageGate component/collider/renderer=연결
BlockingTilemap collider=true
WaterTilemap collider=true
EventSystem=1
InputSystemUIInputModule=1
```

## 자동 테스트 증거 경계

구현 담당 기록:

```text
RatHost2DStage3MutationTests 6/6 PASS
LastHost.Prototype.RatHost2D.Tests 53/53 PASS
```

독립 QA는 전체 EditMode를 MCP TestRunner API로 재요청했으나 결과를
확정하지 못했다.

1. 첫 요청: MCP 자동 코드 보정이 QA 콜백 클래스를 중복 생성해
   `CS1527` 발생
2. 재요청: `UNEXPECTED_ERROR: No logs available`
3. Unity 상태: 비재생, 비컴파일, Console 빈 상태

이는 제품 코드 실패가 아니라 QA 실행 도구 실패다. 같은 요청의 추가
반복은 중단했고, 구현 담당의 `6/6`, `53/53` 증거와 독립 원본 Play를
분리해 기록한다.

## 잠복 강화

독립 Play 세션에서 백혈구 포착 1회와 조각 3개를 공개 Session 큐·flush
경로로 처리했다.

```text
InternalVirus -> MutationSelection
selection shell active=true
button SelectMutation()=true
return mode=RatHost
alert=33 (25 + capture 8)
HUD="적용 변이 잠복 강화"
second selection=false
```

복귀 뒤 효과:

```text
idle 10s: alert 33 -> 33
contamination 1s, alertRate=20: 33 -> 44
immune delta=11 (=20*0.55)
health damage=10 유지
feedback delta=11
```

재진입:

```text
mode=InternalVirus
stability=100/100
fragments=0/3
entryCount=2
```

## 신경 조종

조각 3개 성공 뒤 `PrototypeInputState.SelectMutation2=true`를
`ProcessMutationSelectionInput`에 전달했다.

```text
selection shell before=true
input selected=true
return mode=RatHost
alert=25
HUD="적용 변이 신경 조종"
HostMode/HostCamera=true
InternalMode/InternalCamera/selection=false
host colliders=2/2
internal colliders=0/9
```

실제 2D 이동 step:

```text
controlPower=1.1
speedMultiplier=1.35
controlRatio=1
moveDirection=(1,0)
motorSpeed=4.05
stepMagnitude=0.081
Physics2D.Simulate=true
body (-1.00,-0.25) -> (-0.92,-0.25)
```

## 포유류 적응

```text
selection=true
return mode=RatHost
alert=25
HUD="적용 변이 포유류 적응"
CanUseMammalPassage=true
gate IsOpen false -> true
gate collider true -> false
```

gate renderer는 비활성화하지 않고 막힘 색에서 반투명 녹색 열린 색으로
바뀐다.

```text
renderer enabled=true
blocked RGBA=(0.682,0.322,0.263,1.000)
open RGBA=(0.282,0.698,0.412,0.451)
BlockingTilemap collider true -> true
WaterTilemap collider true -> true
Barrel_A/Pipe_A/RatHost2D/ContaminationZone2D collider=true
```

따라서 지정 통로만 물리적으로 열리고 다른 벽·수로·소품 충돌은 유지된다.

## 실패 회귀

```text
WBC outcomes=Running, Running, Failed
during mode=VirusFailed
stability=0
failurePanel=true
selection=false
host=false
internal=true
no mutation=true
```

확인 복귀:

```text
confirmed=true
mode=RatHost
alert=60
no mutation=true
failurePanel=false
host/hostCamera=true
internal/internalCamera=false
```

재진입:

```text
mode=InternalVirus
stability=100/100
fragments=0/3
fragment objects=3/3 active
failurePanel=false
entryCount=2
```

## Console·저장·보호 diff

```text
Unity Console Error=0
Unity Console Warning=0
Play 종료
scene=Assets/_Project/Scenes/RatHost2DPrototype.unity
dirty=false
Packages diff=0
Windows build=not run
```

보호 대상은 모두 유지됐다.

- `ProjectSettings.asset`의 기존 사용자 define 한 줄
- `_workspace/previews/`
- 기존 3D 씬
- `RatHost2DTechnicalSample`
- 입력 asset

## 사용자 확인 항목

- 실제 숫자키 `1/2/3`과 마우스 버튼 클릭 수신
- 선택 UI와 적용 변이 HUD의 실제 해상도 가독성
- 신경 조종 이동 체감과 포유류 전용 통로 통과 체감

이 수동 확인은 기술 계약 통과와 별개이며, 최종 아트 수용을 뜻하지 않는다.

## 상태판·경로·Git 후속 대조

최초에는 완료 보관된 정합성 작업과 같은 빈 active 폴더가 남아 있어
수정이 필요했다. 메인 조정자가 해당 빈 폴더를 제거한 뒤 재대조했다.

```text
stale active path exists=false
actual active directories=4
board active references=4
active set equal=true
all board active/completed paths exist=true
active/completed duplicate=none
manual hold heading count=1
HEAD=origin/main=remote main
commit=73c575058ee73a9c4ae926d42ae77480a82e5604
```

Stage3의 구현·원본 Play QA 완료와 두 미확인 항목, Stage2 Space 미확인,
자연 경계도 작업 차단, 사용자 수동 플레이 보류가 상태판과
`CURRENT.md`에 실제 경로와 일치하게 기록되어 있다.

운영 상태판 게이트 최종 판정: `통과`.
