# 검증 기록

## 검증 대상

쥐 1종의 단일 저폴리 3D 원본에서 재현한 정지 포즈 8방향 시험 에셋과 브라우저 프리뷰가 재현 가능하며, 최종 아트나 Unity 통합 완료가 아닌 **사용자 시각 확인용 시험 플레이스홀더**로 제시할 수 있다는 주장.

## 작업영역

`_workspace/active/2026-07-16-rat-8-direction-trial-asset/`

## 실행한 검증

### 1. 기존 산출물 자동 검증

명령:

```powershell
python --version
python source\validate_outputs.py
```

작업 위치:

`_workspace/active/2026-07-16-rat-8-direction-trial-asset/artifacts/`

결과:

- Python `3.11.2`.
- `status: pass`.
- 방향 순서 `S, SW, W, NW, N, NE, E, SE`.
- 방향 PNG 8장 모두 RGBA8 `64×64`, 투명 배경, 투명 제외 허용 팔레트 7색 이하.
- 시트 `512×64`, 8셀의 정확한 순서 결합.
- OBJ 정점 528개, 삼각형 면 924개.
- 시트 SHA-256 `961ace066b18501973acde238b2521a75254a040fc2271994edfff4058db4db5`.

해석:

- 문서화된 형식·크기·방향·팔레트·원본 밀도와 실제 산출물이 일치한다.

### 2. 렌더러 독립 재실행과 SHA 안정성

명령:

```powershell
python source\render_rat_8dir.py
python source\validate_outputs.py
Get-FileHash -Algorithm SHA256 <방향 PNG 8장, 시트, OBJ, MTL, CSV, JSON>
```

결과:

- 기본 샌드박스 실행은 `source/rat-trial-v1.obj` 쓰기에서 `PermissionError: [Errno 13]`로 중단되었다.
- 동일 명령을 승인된 격리 해제 환경에서 재실행해 `528 vertices / 924 triangles`와 동일 시트 SHA를 출력했다.
- 재실행 뒤 `validate_outputs.py`가 다시 통과했다.
- 재실행 전후 비교 대상 13개 파일의 SHA-256이 모두 동일했다.
  - 방향 PNG 8장
  - 스프라이트 시트 1장
  - `rat-trial-v1.obj`, `rat-trial-v1.mtl`
  - `direction-map.csv`, `render-settings.json`
- 재실행 후 시트 SHA-256도 `961ace066b18501973acde238b2521a75254a040fc2271994edfff4058db4db5`로 유지되었다.

해석:

- 권한 제약은 렌더 정의나 결과의 오류가 아니다. 동일 Python 3.11.2 환경에서 렌더러가 결정론적으로 동일 산출물을 재현한다.

### 3. 방향·피벗·bounds와 원본 대조

명령:

```powershell
python source\validate_outputs.py
Get-Content render-settings.json | ConvertFrom-Json
Get-Content source\rat-trial-v1.obj
```

결과:

- 공통 캔버스 `64×64`, 공통 루트 피벗은 좌상단 기준 `(32, 48)`이다.
- 렌더 코드의 단일 `PIVOT`, 고정 직교 카메라, 고정 조명, 공통 scale을 모든 방향이 공유한다.
- 불투명 bounds:
  - S `[21,32,37,53]`
  - SW `[22,32,40,52]`
  - W `[11,31,56,50]`
  - NW `[22,29,52,56]`
  - N `[26,30,42,59]`
  - NE `[23,29,41,60]`
  - E `[7,31,52,52]`
  - SE `[11,32,41,52]`
- 최소 여백은 좌·우 7px, 위 29px, 아래 3px이며 잘린 방향은 없다.
- 코드의 `build_rat()` 단일 메시가 8개 yaw에 재사용되고, 교환용 OBJ도 528 vertices / 924 triangle faces로 일치한다.

해석:

- 방향별 bbox 크기 변화는 한 3D 원본의 투상·단축에 따른 결과이며 임의 확대·축소가 아니다.
- 모든 방향은 동일 root pivot과 화면 scale을 유지하고 셀 내부에 안전하게 들어온다.

### 4. HTML 프리뷰와 파일 위생

명령:

```powershell
# preview/index.html의 로컬 src/href를 추출해 문서 위치 기준 Test-Path 대조
Get-ChildItem -Recurse -File artifacts
Select-String -Pattern 'probe|temp|tmp|copy|duplicate|backup|\.bak|\.old|__pycache__'
```

결과:

- 프리뷰의 로컬 이미지 참조 10개가 모두 존재한다: 콘셉트 1장, 방향 PNG 8장, 시트 1장.
- HTTP(S), `file://`, 외부 스크립트·스타일 참조는 없다.
- 프리뷰는 `image-rendering: pixelated/crisp-edges`와 4배 방향 카드, 공통 피벗 오버레이, 2배 시트를 제공한다.
- artifacts 파일은 총 20개이며 probe, 임시, 복사본, 백업, `__pycache__` 후보는 0건이다.
- 작업 패킷 텍스트 파일 14개의 trailing whitespace와 충돌 표식은 0건이다.

해석:

- 프리뷰는 저장소 내부 상대 경로만으로 사용자에게 제시할 수 있고, 진단 과정의 임시 중복 파일은 남아 있지 않다.

### 5. 콘셉트·최종 방향 렌더 역할과 출처

결과:

- `style-brief.md`, `generation-log.md`, `README.md`, 프리뷰가 imagegen 결과를 실루엣·비율·팔레트 참고 1안으로만 명시한다.
- 최종 프롬프트, 생성 도구·날짜, 외부 참조 없음, 원본 보관 위치, 프로젝트 보관 위치, 선택·미반영 이유가 기록되어 있다.
- 8방향 PNG는 AI가 방향별 생성한 이미지가 아니라 `render_rat_8dir.py::build_rat()`의 단일 메시를 회전해 출력한 결과다.
- 최종 아트 적합성·독점성 보증, Unity 반입, 애니메이션, PPU·최종 셀 크기 확정은 주장하지 않는다.

해석:

- AI 콘셉트와 재현 가능한 3D 방향 렌더의 역할·출처·승인 경계가 분리되어 있다.

### 6. 시각 검사

검사 대상:

- `renders/rat-00-s.png`부터 `rat-07-se.png`까지 개별 원본 PNG 8장
- `renders/rat-8dir-sheet.png`
- `concept/rat-concept-reference-v1.png`

결과:

- 시트는 S에서 시작해 SW, W, NW, N, NE, E, SE로 한 쥐가 연속 회전하는 것으로 읽힌다.
- 큰 몸통, 분홍색 귀·코·꼬리, 검은 눈의 재질과 비율이 모든 방향에서 같은 개체로 유지된다.
- W/E와 대각선은 주둥이와 긴 꼬리의 반대 배치로 즉시 방향을 구분할 수 있다.
- S/N은 투상 단축 때문에 폭이 좁지만, 분홍색 코·꼬리 위치와 인접 방향의 회전 흐름으로 4배 Point 확대에서 구분 가능하다.
- 귀·주둥이·꼬리는 모든 셀 안에 남고 잘리지 않는다. 공통 scale에서 방향에 따른 자연스러운 화면 점유 변화만 보인다.
- 네이티브 1배에서는 S/N 실루엣이 작게 보이므로 최종 셀 크기 수용 근거로 사용하지 않고, 이번 사용자 확인에서는 4배 Point 프리뷰를 기준으로 봐야 한다.

해석:

- 정지 8방향 파이프라인과 사용자 시각 비교용 시험 에셋으로는 판독 가능하다.
- 최종 게임 크기, 애니메이션 중 방향 팝핑과 발 미끄러짐은 이 정지 시험으로 검증되지 않는다.

### 7. Git·범위·현황판 대조

명령:

```powershell
git diff --check
git status --short -- UnityProject Builds
git diff --name-only -- UnityProject Builds
git status --short -- .codex/config.toml _workspace/previews _workspace/active/2026-07-16-natural-alert-build-loop-verification
git status --short -- _workspace/active/2026-07-16-rat-8-direction-trial-asset docs/project-handoff/current-task-board.md
git diff --cached --name-only
```

결과:

- `git diff --check` 오류 0건, 스테이징된 파일 0건.
- `UnityProject/`, `Builds/` 변경 0건.
- `.codex/config.toml`은 기존 별도 수정, `_workspace/previews/`는 기존 별도 미추적 상태다.
- 자연 경계도 엄격 검증 패킷의 변경은 없다.
- 현재 작업 패킷은 active에 존재하고 현황판도 `8방향 쥐 시험 에셋 | 진행 중 — Unity 반입 제외`로 같은 경로를 가리킨다.
- HEAD와 로컬 `origin/main`은 모두 `d5d1ade`다.

해석:

- 현재 작업은 승인된 쥐 시험 에셋 패킷과 현황판에 한정되며 Unity·기존 제외 파일·차단 작업을 침범하지 않았다.

## MCP 플레이 체크

- 수행하지 않음.
- Unity 코드·씬·프리팹·Import·ProjectSettings 변경이 없고 에셋도 Unity에 반입하지 않았으므로 Unity 플레이어블 검증 대상이 아니다.

## 미검증 항목

- 사용자의 최종 실루엣·팔레트 선호와 수용 여부.
- Unity Sprite Import, PPU, SpriteRenderer·Animator·방향 매핑·3D 루트 연결.
- 실제 게임 카메라에서의 월드 크기, 접지, 그림자, 가림·깊이 정렬.
- 걷기 프레임, 방향 전환 팝핑, 프레임 위상, 발 미끄러짐.
- 최종 아트 품질과 상업 에셋 적합성.

## 남은 위험

- S/N은 3D 투상 단축으로 네이티브 1배에서 폭이 좁아 최종 셀 크기에 따라 판독성이 부족할 수 있다.
- 정지 포즈에서 맞는 공통 pivot이 걷기 애니메이션 프레임에서도 안정적이라는 보장은 없다.
- 구운 명암과 실제 하수도 환경 조명·그림자·가림의 조화는 Unity 반입 뒤 별도 시각 QA가 필요하다.
- 기본 샌드박스에서는 기존 OBJ 덮어쓰기 권한이 거부되므로 이 환경에서 재현할 때 동일 명령의 쓰기 권한 승인이 필요하다.

## 완료 판단

**완료 가능**

쥐 1종의 단일 3D 원본 기반 정지 8방향 시험 에셋은 형식·방향·피벗·팔레트·원본 밀도·프리뷰 참조가 일치하고, 승인된 재실행에서 SHA까지 동일하게 재현되었다. 시각적으로도 8방향 회전, 주둥이·꼬리, 공통 scale과 셀 내 여백을 판독할 수 있다. 최종 아트·Unity 반입·애니메이션 완료가 아니라 사용자 시각 확인에 올리는 시험 플레이스홀더라는 범위에서 완료 가능하다.

## 메인 통합 후 최종 상태판 대조

명령:

```powershell
rg -n '상태:|사용자.*대기|QA|다음 작업|8방향|자연 경계도|수동 플레이|\.codex/config|_workspace/previews' task.md work-log.md handoff.md _workspace/active/CURRENT.md docs/project-handoff/current-task-board.md
git diff --check
git diff --cached --name-only
git status --short -- UnityProject Builds
git diff --name-only -- UnityProject Builds
git status --short -- .codex/config.toml _workspace/previews _workspace/active/2026-07-16-natural-alert-build-loop-verification
```

결과:

- `task.md`는 `사용자 결정 대기`이고 QA `완료 가능`·총괄 `내부 승인 가능 — 사용자 결정 대기` 게이트를 기록한다.
- `work-log.md`, `handoff.md`, `_workspace/active/CURRENT.md`, 현황판은 제작·QA 완료와 사용자 실루엣·팔레트 결정 대기, Unity 반입 제외를 일관되게 기록한다.
- 작업은 `_workspace/active/2026-07-16-rat-8-direction-trial-asset/`에 유지된다.
- 현황판의 다음 작업 후보에는 자연 경계도 엄격 검증 재개만 있고 8방향 쥐 시험 에셋은 중복되지 않는다.
- 자연 경계도 엄격 검증은 별도 active·QA `차단`·총괄 `보류`, 사용자 수동 플레이 체감 확인은 별도 보류로 유지된다.
- `git diff --check` 오류 0건, staged 파일 0건, `UnityProject/`·`Builds/` 변경 0건이다.
- `.codex/config.toml`과 기존 `_workspace/previews/`는 작업 범위 밖 변경으로 유지되며, 자연 경계도 작업 패킷의 변경은 없다.

최종 판정: **완료 가능 유지**. 사용자 시각 결정 전까지 active 상태를 유지하는 현재 통합 상태와 QA 완료 범위가 일치한다.
