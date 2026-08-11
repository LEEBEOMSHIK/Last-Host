# 픽셀 시네마틱 제작·검증 재발 방지 플레이북

## 1. 목적과 적용 범위

이 문서는 `A01 회사 일상` 시네마틱 에셋 제작에서 확인된 실패를 다음 픽셀아트 모션 코믹형 시네마틱에서 반복하지 않기 위한 공통 제작·검증 기준이다.

적용 대상은 생성형 이미지 후보, 배경·캐릭터·전경 레이어, 제한 포즈 시트, Unity Sprite Import와 이후 시네마틱 재생 인계다. 이 문서는 [픽셀아트 모션 코믹형 시네마틱 가이드](pixel-art-motion-comic-cinematic-guide.md)를 보완하며, 위험 등급·correction·고비용 실행 규칙은 [루프 엔지니어링 게이트](../../agents/loop-engineering-gates.md)를 대체하지 않는다.

본 문서는 사용자 검토와 기존 시네마틱 가이드·연출 에이전트·참조 맵 연결을 거쳐 프로젝트의 필수 제작 참조로 활성화됐다. 상위 `loop-engineering-gates.md`를 대체하지 않는다.

이 문서가 자동으로 승인하지 않는 범위:

- 새 시네마틱 장면의 이미지 생성과 에셋 제작
- Unity Import, Timeline, 씬·프리팹·코드 변경
- 생성 후보 또는 기술 검증 에셋의 사용자 시각 수용
- 오디오, 자막 문자열, 다국어 폰트와 실제 재생 연결

## 2. A01에서 확인된 근본 원인

세부 실행 이력과 수치는 [A01 verification](../../../_workspace/active/2026-08-10-a01-office-animatic/verification.md)에 보존한다. 다음 표는 다른 장면에도 적용되는 원인과 차단 조건만 추린 것이다.

| 계층 | 관찰된 실패 | 근본 원인 | 다음 작업의 선행 차단 조건 |
| --- | --- | --- | --- |
| 계획·운영 | 계획 감사와 상태 동기화가 반복되고 실행 계획이 여러 번 재작성됨 | 설계, 실행 명령, 과거 이력과 현재 상태를 한 계획 안에 중첩해 작은 수정도 전체 계획 재검토로 번짐 | 설계 기준 1개, 실행 체크리스트 1개, canonical 상태 기록 1개를 유지한다. 과거 명령은 실행 가능한 형태로 남기지 않는다. 상태 문서 동기화는 blocker·기술 PASS·사용자 수용 같은 경계에서 묶어 처리한다. |
| 생성 이미지 | 캐스트가 `1254×1254`, `1122×1402` 등 임의 크기로 생성되고 4×5 셀 경계를 침범함 | 생성 모델에 exact canvas, 균등 셀, 포즈 소유권과 production SpriteSheet 규격을 동시에 보장하도록 요구함 | 생성 결과는 원화 후보로만 받는다. exact 캔버스·셀·피벗·이름·순서는 결정론적 패키징 또는 수작업 재제작 단계가 소유한다. |
| 인물·공간 연속성 | 인물 행 정체성, 가방 방향, 의자·모니터 같은 공간 앵커가 바뀜 | 연기 reference와 공간 reference의 소유권이 섞였고 한 번의 생성에 정체성·포즈·공간 복원을 모두 요구함 | reference마다 `정체성`, `연기`, `공간` 소유권을 브리프에 한 줄로 고정한다. 배경 clean plate와 캐릭터 포즈는 별도 후보로 만든다. |
| 크로마·알파 | 자동 alpha 계약을 통과한 color foreground도 머리·손·가방·신발 경계의 밝은 마젠타·분홍 fringe로 시각 QA에서 반려됨 | exact `#FF00FF` 제거와 수치 PASS를 clean foreground의 화면 PASS로 오인함 | dark·밝은 중립색·실제 배경 합성을 Unity 전에 모두 검사한다. color foreground가 시각 QA를 통과하지 못하면 rejected RGB를 연결하지 않고, 재분류 승인을 거쳐 승인 BG repeat와 exact mask만 쓰는 mask-only 가림으로 전환한다. |
| 결정론적 가공 | sourceCuts 누락, 행·열 매핑 교환, self-derived oracle, 기존 출력 원자 교체 실패가 뒤늦게 드러남 | manifest 필드·독립 oracle·overwrite 경로가 초기 계약과 테스트에 완전하게 포함되지 않음 | source hash, cuts, pose ID, source rect, target cell, pivot, output hash를 manifest로 고정한다. 테스트는 production과 독립된 mapper를 사용하고 신규 출력과 기존 출력 교체를 모두 검증한다. |
| PowerShell/C# 하네스 | `System.Drawing` 전이 assembly, hashtable 산술식, `System.Console`, `File.Replace` null backup에서 중단됨 | 실제 PowerShell 7.6·.NET 실행 환경의 참조 폐쇄와 파싱·overwrite 동작을 작은 probe로 먼저 확인하지 않음 | 실데이터 전 compile-only probe, 산술식 괄호, 결과 문자열 반환, same-directory backup을 사용하는 overwrite negative control을 통과시킨다. |
| Unity API 호환 | `SHA256.HashData`, `spriteGenerateFallbackPhysicsShape` 같은 현재 프로젝트 프로필 미지원 API로 컴파일이 실패함 | 일반 최신 .NET 또는 다른 Unity 버전의 API를 Unity `6000.4.6f1`·NET Standard 2.0에서 확인하지 않고 사용함 | 프로젝트 reference metadata와 compile-only gate에서 API 존재를 먼저 확인한다. 이 gate가 green이 되기 전에는 실제 candidate Unity run을 시작하지 않으며, 해시는 `SHA256.Create().ComputeHash`처럼 현재 프로필에서 지원되는 경로를 사용한다. |
| Unity Sprite Import | `.meta`에 20개 rect가 있어도 실제 Sprite rect가 `(0,0,0,0)`으로 Import됨 | Unity `6000.4.6f1`의 `TextureImporter` Multiple Sprite `.meta`가 요구하는 nested `rect` 직렬화 대신 inline rect를 사용했고 정적 텍스트 검사를 실제 importer 검증으로 오인함 | 설치된 Unity `6000.4.6f1`의 정상 Multiple Sprite `.meta`를 기준으로 nested rect를 작성한다. 정적 계약 뒤 EditMode에서 이름·rect·pivot·Import 설정·physics shape를 실제 Sprite로 조회한다. |
| Unity Scene 멱등성 | 같은 builder를 실행할 때마다 Scene dependency hash가 바뀜 | `NewScene`과 전체 `new GameObject`/`AddComponent`로 scene-local fileID를 매번 새로 부여함 | 최초에만 Scene을 만들고 이후에는 기존 Scene을 열어 exact hierarchy·component·binding을 reconcile한 뒤 같은 Scene asset에 in-place save한다. source baseline은 current builder로 한 번 생성한 뒤 fingerprint하며, baseline→rebuild 1→rebuild 2의 path·GUID·dependency hash를 비교한다. |
| SpriteMask 범위 | front order는 `200`인데 back order가 `0`으로 저장됨 | front가 기본 `0`인 상태에서 back을 먼저 `200`으로 설정해 Unity가 back을 `0`으로 clamp함 | custom range를 끄고 front layer/order, back layer/order 순으로 설정한 뒤 range를 켠다. 즉시 read-back하여 네 endpoint가 모두 기대값인지 fail-fast한다. |
| Preview 상태 복구 | builder가 현재 Scene을 바꾼 뒤 snapshot을 잡거나 실패 뒤 원래 Scene·start scene을 복구하지 못함 | 미리보기 실행과 작업자 Editor 상태 보존의 순서가 뒤집힘 | dirty 저장 확인 뒤 **어떤 rebuild/open보다 먼저** session snapshot을 잡는다. 정상·예외·중단 복구를 같은 공개 API로 검증하고 Play 종료 후 원래 상태를 복원한다. |
| 검증 테스트 | 안전 preflight에서 reflection이 차단되고, test setup이 누락 asset을 self-heal했으며, isolated project의 asset wrapper와 초기 `playModeStartScene`을 main Editor와 같다고 가정한 assertion이 실패함 | 공개 API shape·검증 대상 생성·Unity managed object identity·Editor 초기 상태를 하나의 oracle로 섞음 | 공개 API는 직접 delegate binding으로 compile-time 검증한다. asset 참조는 managed object `SameAs`가 아니라 asset path와 GUID로 비교하고, isolated 초기 `playModeStartScene`은 nullable로 캡처해 null/non-null을 그대로 복원한다. test setup은 builder를 호출하지 않으며 checkout asset이 없으면 RED, baseline은 explicit rebuild 전에 캡처한다. |
| Fingerprint·줄바꿈 | 같은 보호 C#이 격리 worktree에서는 CRLF, main에서는 LF여서 원시 SHA 대조가 실패로 보임 | Git blob 기준과 플랫폼별 working-tree 바이트를 같은 fingerprint 계약으로 취급함 | 텍스트 보호 파일은 Git blob 또는 CRLF→LF 정규화 SHA·길이를 canonical로 고정하고 lone CR은 실패시킨다. 바이너리·Unity YAML은 원시 SHA를 유지한다. 줄바꿈 오탐을 맞추려고 보호 파일을 다시 쓰거나 Unity를 재실행하지 않는다. |
| 검증 래퍼 | 여러 경로가 flatten되거나 결과 XML 이후 바깥 wrapper만 종료 대기함 | 외부 `pwsh` 경계에서 배열 cardinality가 사라지고 host timeout·child lifecycle·ledger 마감이 서로 다른 상태가 됨 | typed/cardinality payload와 multipath self-test를 사용한다. 배열 입력 스크립트는 같은 PowerShell 세션에서 호출하고, evidence 경로는 절대 경로로 고정한다. timeout만으로 재실행하지 말고 PID·XML·로그·ledger를 대조한다. |
| Unity 실행 격리·경로 | stale `bee_backend`와 긴 cache 경로가 후속 실행을 막고, 별도 visible Unity가 기존 main Editor와 혼동될 위험이 생김 | 이전 worker·Library를 소유 확인 없이 재사용하고 Windows·Bee 전체 경로 예산을 실행 직전에 계산하지 않음 | 사용자가 보고 있는 main Editor는 닫거나 대체하지 않고 visible duplicate Unity를 열지 않는다. 해당 isolated cache를 소유한 stale `bee_backend`만 확인·정리한 뒤 짧은 fresh cache root와 full-path budget을 선행 검사하고 hidden batch를 한 번 실행한다. fresh cache 첫 import에서 API Updater 전 일시적 package compile 오류가 보여도 조기 재실행하지 않고, 후속 Tundra 성공·최종 XML·Unity exit code로 판정한다. |
| 증거·비용 | 중간 로그와 상태 커밋이 늘고 최종 근거를 찾기 어려워짐 | 모든 시도 결과를 동일한 중요도로 보존하고 micro-step마다 상태를 여러 문서에 복제함 | raw source와 최종 canonical evidence, 첫 blocker 최소 반례를 우선한다. 무효 증거는 `SUPERSEDED`와 후속 run을 연결해 보존하되 같은 의미의 중복 전체 로그를 추가로 만들지 않고 최종 manifest가 SHA와 경로를 소유하게 한다. |

## 3. 자산 상태를 섞지 않는 규칙

모든 시네마틱 이미지는 다음 상태 중 하나로만 표시한다.

1. `reference`: 분위기·구도·정체성을 설명하는 입력 자료
2. `generated-source`: 생성 도구가 반환한 변경하지 않은 원본 후보
3. `derived-candidate`: 결정론적 가공 또는 수작업 정리를 거친 검토 후보
4. `game-asset-candidate`: 규격·visual QA·정적 bundle 검증을 통과한 Unity 반입 후보
5. `import-verified`: 현재 Unity에서 실제 Sprite Import 테스트를 통과한 후보
6. `user-accepted`: 사용자가 실제 화면을 보고 시각적으로 수용한 에셋
7. `integrated`: 별도 승인된 씬·Timeline 재생 작업에 연결된 에셋

앞 단계를 건너뛰거나 `import-verified`를 `user-accepted` 또는 `integrated`로 표현하지 않는다.

## 4. 다음 시네마틱의 기본 제작 흐름

### Gate 0. 장면·비용 계약 고정

이미지 생성 전에 다음 항목을 한 작업 패킷에서 고정한다.

- 숏 ID와 한 문장 서사 목적
- 배경, 캐릭터, 전경, 효과, 마스크의 분리 목록
- reference별 소유권: 정체성, 연기, 공간, 팔레트
- 생성 후보 수와 correction 상한
- source, derivative, production, Unity 경로
- 사용자에게 먼저 확인받을 시각 후보
- Unity를 시작하기 전의 저비용 검증 명령

기본 생성 예산은 레이어당 initial 1회와 correction 1회를 넘지 않는 상한이며 목표 횟수가 아니다. 사용할 이유가 없는 호출은 실행하지 않고, 결과 파일이 없는 호출도 invocation으로 센다. 두 번째 correction이나 다른 생성 경로는 원인을 재분류하고 사용자 승인을 다시 받은 뒤 진행한다.

### Gate 1. 생성 후보 확보

- 배경 clean plate, 캐릭터·포즈, 전경 occluder와 효과를 한 이미지에 합쳐 생성하지 않는다.
- 생성 모델에 production SpriteSheet의 exact pixel size나 완전한 균등 격자를 성공 조건으로 맡기지 않는다.
- prompt, 도구, 날짜, 입력 reference, 출력 경로, 실제 width·height·SHA-256을 생성 직후 기록한다.
- 생성 원본은 변경하지 않고 versioned source로 보존한다.
- 정체성·공간 앵커·금지 요소가 틀리면 후처리로 숨기지 않고 source 단계에서 반려한다.

세부 생성 경계는 [ChatGPT 이미지 연계 아트 작업 순서](../../prototype/plans/rat-host-ai-assisted-art-workflow.md)를 따른다.

### Gate 2. Source visual checkpoint

결정론적 가공 전에 비주얼 담당과 사용자가 다음을 먼저 확인한다.

- 인물과 주요 오브젝트의 정체성
- 배경의 문·창·책상·의자 같은 continuity anchor
- 포즈 간 의상·소품·가방 방향
- 캐릭터와 배경의 팔레트·명암·픽셀 밀도 조화
- 텍스트·로고·범위 밖 오브젝트 부재
- 레이어 분리가 가능한 충분한 여백과 비충돌 배치

이 단계에서 반려된 후보는 Unity용 가공을 시작하지 않는다.

### Gate 3. 결정론적 패키징

- raw source SHA를 입력 계약으로 고정한다.
- canvas, cuts, pose ID, source rect, target cell, pivot, 출력 경로를 manifest가 소유한다.
- 정수 좌표와 nearest-neighbor를 사용하며 전역 crop·padding으로 경계 충돌을 숨기지 않는다.
- source RGBA 불변, output alpha·silhouette·coverage와 repeated SHA를 검사한다.
- 기존 출력 교체는 같은 디렉터리의 임시 파일·backup·검증·원자 교체 순서로 수행한다.

A01에서 만든 다음 도구는 동작 reference로 재사용한다.

- `tools/art/Repack-ChromaPoseGrid.ps1`
- `tools/art/Test-RepackChromaPoseGrid.ps1`
- `tools/art/Test-A01OfficeAssetBundle.ps1`

`A01` 이름의 bundle test는 다른 숏에 그대로 실행하지 않는다. 새 숏은 동일 계약 구조의 숏 전용 정적 테스트를 만들거나 공용화가 별도 승인된 뒤 사용한다.

### Gate 4. Unity 전 visual QA

동일한 derived candidate를 다음 조건에서 확인한다.

- 투명 배경을 보여주는 dark view
- `#E8E8E8` 같은 밝은 중립 배경
- 실제 사용할 장면 배경
- 100% 실제 크기와 nearest-neighbor 확대 보기

필수 판정:

- 밝은 chroma·pink fringe가 반복되지 않음
- alpha 단계가 의도하지 않게 비지 않음
- 외곽 실루엣과 작은 소품이 깎이지 않음
- 정체성, 표정, 포즈, 의상과 소품이 유지됨
- 캐릭터가 배경과 따로 노는 팔레트·광원 불일치가 없음

자동 수치가 PASS여도 눈으로 보이는 fringe가 있으면 반려한다. visual QA가 PASS하고 사용자가 후보를 확인하기 전에는 Unity Import를 시작하지 않는다.

### Gate 5. 정적 Unity bundle 계약

Unity 실행 전에 다음을 파일 수준에서 검사한다.

- production PNG와 source/derivative의 역할·이름 분리
- manifest의 파일명, byte length와 SHA-256
- Sprite mode, PPU, filter, mipmap, compression, alpha 설정
- sprite 이름·순서·rect·pivot·internal ID 매핑
- Unity `6000.4.6f1` `TextureImporter` Multiple Sprite `.meta`의 nested rect 수와 inline rect 부재
- 폴더와 파일 `.meta`의 GUID 존재·중복 부재
- 테스트 C#의 현재 Unity API 호환성과 금지 reflection 부재

정적 계약이 실패하면 Unity를 실행하지 않는다.

### Gate 6. Unity Import 검증

1. 검증 래퍼 multipath self-test를 저비용으로 실행한다.
2. candidate fingerprint와 run ID를 고정한다.
3. `Invoke-HighCostVerification.ps1` preflight를 통과한 단일 isolated Unity run만 실행한다.
4. EditMode에서 실제 Import 결과를 조회한다.
   - background Sprite 존재와 설정
   - 캐릭터 Sprite 수·이름·rect·pivot
   - filter, mipmap, compression, alpha
   - physics shape 수
5. 결과 XML, Unity log와 fingerprint manifest를 canonical run 하나에 연결한다.

실패한 run은 재시도하지 않는다. 원인 계층을 확인하고 저비용 RED→GREEN을 통과한 새 fingerprint에서만 다음 run을 허용한다.

### Gate 7. 사용자 수용과 재생 통합

`import-verified` 에셋을 사용자에게 실제 크기와 장면 합성으로 보여준다. 사용자가 수용한 뒤에만 별도 승인된 작업에서 Timeline, 카메라, 제한 포즈 교체, 패럴랙스, 자막·오디오 큐와 게임 상태 복귀를 연결한다.

에셋 Import PASS는 움직이는 시네마틱 재생 PASS가 아니다. 통합 뒤에는 다음을 별도로 확인한다.

- 배경에 전경 가림이 구워져 있는지 S0에서 먼저 확인한다. color foreground visual QA가 실패하면 rejected RGB를 숨겨 연결하지 않고, 승인 BG repeat+exact mask의 mask-only 구조처럼 실제로 구현 가능한 가림 방식으로 재분류한다.
- Scene builder는 최초 생성과 재빌드를 분리한다. 재빌드는 기존 Scene을 열어 hierarchy·component·Timeline binding을 reconcile하고 같은 asset에 저장하며, 누락·중복·예상 밖 타입은 조용히 덮지 말고 실패시킨다.
- SpriteMask custom range는 front→back 설정 순서와 read-back 값까지 검사한다.
- 계약 테스트는 production asset을 self-heal하지 않는다. checkout Scene·Timeline·animation이 누락되면 실패하고, asset identity는 path·GUID로 확인하며, isolated 초기 `playModeStartScene`의 null도 정상 baseline으로 보존한다. 멱등성 검사는 명시적 rebuild 전 baseline부터 비교한다.
- QA 안전 preflight에서 금지 reflection·숨은 출력·경로 누락을 먼저 차단한 뒤 새 fingerprint로 Unity를 한 번만 실행한다.
- 시작·종료 게임 상태와 입력 잠금·복구
- 프레임·포즈 교체와 카메라 타이밍
- 자막 문자열·언어별 폰트 fallback·읽기 시간
- 건너뛰기, 재시청, 점멸·흔들림 완화
- 실제 재생 해상도에서 픽셀 안정성과 배경·캐릭터 조화

작업 중 생성·컴파일은 짧은 fresh cache path를 쓰는 격리 프로젝트의 숨김 batch 실행을 기본으로 한다. stale `bee_backend`와 full-path budget을 먼저 확인하고 visible duplicate Unity를 열지 않으며, 사용자가 보고 있는 기존 main Editor를 닫거나 대체하지 않는다. 최종 후보를 통합한 뒤에만 그 main Editor의 전용 메뉴로 재생 수용을 받는다.

## 5. 다음 시네마틱 빠른 단일 경로 체크리스트

아래 순서를 위에서 아래로 한 번만 따른다. 하나라도 `아니오`면 그 자리에서 중지하고 Unity, MCP, build를 시작하지 않는다.

- [ ] 기존 main Editor의 PID·Scene·Play/Pause·dirty를 보존하고 visible duplicate Unity를 열지 않는가?
- [ ] 해당 isolated cache의 stale `bee_backend`를 소유 확인 뒤 정리하고, 짧은 fresh cache root와 전체 경로 예산을 확인했는가?
- [ ] fresh cache 첫 import의 중간 오류가 아니라 최종 compile 상태·결과 XML·Unity exit code를 함께 판정하도록 했는가?
- [ ] source와 production 후보의 SHA·경로·상태가 구분되어 있는가?
- [ ] 사용자가 생성 source 또는 derived visual을 먼저 확인했는가?
- [ ] dark·light·scene composite에서 color foreground가 PASS했는가? 실패했다면 rejected RGB를 버리고 승인된 mask-only 경로로 재분류했는가?
- [ ] exact canvas·rect·pivot·mapping을 생성 모델이 아니라 manifest와 도구가 소유하는가?
- [ ] 정적 bundle test가 current candidate에서 PASS했는가?
- [ ] C#이 Unity `6000.4.6f1` 지원 API만 사용하며 compile-only compatibility gate가 PASS했는가?
- [ ] QA test가 공개 API를 직접 바인딩하고, asset을 path·GUID로 비교하며, nullable 초기 start scene을 보존하고 production asset을 self-heal하지 않는가?
- [ ] Scene builder가 기존 Scene을 open→reconcile→in-place save하고 baseline→rebuild 1→rebuild 2의 path·GUID·hash가 안정적인가?
- [ ] SpriteMask를 range off→front→back→range on으로 설정하고 네 endpoint를 즉시 read-back했는가?
- [ ] Unity `6000.4.6f1` `TextureImporter` Multiple Sprite `.meta`의 nested rect 형식을 정상 reference와 대조했는가?
- [ ] PowerShell 배열을 typed/cardinality payload로 전달하는 multipath self-test·negative control이 PASS했는가?
- [ ] current fingerprint·run ID·ledger와 CWD 무관 절대 evidence 경로가 준비됐고, 실패 시 재시도 없이 돌아갈 저비용 gate가 정해졌는가?

## 6. 기록과 토큰 비용 절감 규칙

- 새 시네마틱 작업은 위험 등급에 맞는 canonical 작업 파일만 만든다. R1은 `record.md` 1개, R2/R3는 `task.md`와 `verification.md`를 기본으로 한다.
- 설계 문서와 실행 체크리스트를 한 번 승인한 뒤, 같은 내용을 이름만 바꾼 계획으로 반복 작성하지 않는다.
- blocker의 상세 stack trace 전체를 여러 문서에 복제하지 않는다. canonical 기록에는 첫 오류, root cause, 최소 change plan과 원본 로그 경로만 남긴다.
- 진행 보고는 `첫 blocker`, `사용자 결정 필요`, `기술 PASS`, `사용자 수용` 경계에서만 기본 제공한다.
- 후보 fingerprint가 바뀌지 않은 상태에서 같은 full suite나 Unity run을 반복하지 않는다.
- 중간 전체 로그·이미지 세대를 새 canonical 후보처럼 반복 생성하지 않는다. immutable source, 최종 production, canonical QA evidence와 첫 blocker 최소 반례를 우선하고, 무효 증거는 `SUPERSEDED`와 후속 run 포인터를 유지한다.
- board와 비용 현황은 실제 후보·blocker·고비용 실행·최종 상태가 바뀔 때만 동기화한다.
- 정확한 토큰이나 금액 계측값이 없으면 추정하지 않고 `미집계`로 둔다.

## 7. A01 재사용 자산과 도구

다음 산출물은 삭제하지 않고 이후 오피스 장면 또는 도구 설계 reference로 사용한다.

### 게임 에셋 후보

- `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Office/a01-office-background-v1.png`
- `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Office/a01-office-cast-poses-v1.png`
- `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Office/a01-office-occlusion-mask-v1.png`
- `UnityProject/Assets/_Project/Art/Cinematics/Opening/A01/Office/a01-office-assets-v1.manifest.json`

### 재생성·추적 원본

- `_workspace/active/2026-08-10-a01-office-animatic/artifacts/imagegen/`
- `_workspace/active/2026-08-10-a01-office-animatic/artifacts/qa/`

### 재사용 원칙

- 오피스 배경과 캐스트는 A01 장면의 에셋 후보이며 다른 장소나 인물로 자동 확장하지 않는다.
- raw source는 재생성·감사 입력이고 Unity production 경로에서 직접 사용하지 않는다.
- repack·test script는 다음 작업의 starting point이지만 새 캔버스·레이어·pose contract에 맞춘 S0와 RED가 먼저다.
- A01 final Unity run의 PASS는 새 숏이나 수정된 에셋에 승계하지 않는다.

## 8. 다음 장면 착수 템플릿

아래 빈 입력란은 다음 장면 작업 패킷에서 채우는 양식이며 현재 플레이북의 미정 사항이 아니다.

```text
시네마틱/숏 ID:
한 문장 서사 목적:
승인된 제작 범위:

레이어:
- 배경:
- 캐릭터:
- 전경:
- 효과·마스크:

reference 소유권:
- 정체성:
- 연기:
- 공간:
- 팔레트·광원:

생성 예산:
- initial:
- correction:
- 추가 호출 승인 조건:

상태별 경로:
- generated-source:
- derived-candidate:
- game-asset-candidate:
- Unity production:

Unity 전 시각 확인:
- dark:
- light:
- scene composite:
- 사용자 확인:

저비용 검증:
- source/manifest:
- alpha/fringe:
- static bundle:
- wrapper self-test:

고비용 검증:
- route:
- run 상한:
- canonical evidence:

별도 승인 대기:
- Unity Import:
- scene/Timeline integration:
- audio/subtitle/localization:
```

## 9. 완료 표현 기준

- 생성 후보만 있음: `생성 후보 검토 대기`
- 가공·visual QA PASS: `게임 에셋 후보 — 사용자 시각 확인 대기`
- Unity Import PASS: `기술 검증 통과 — 사용자 수용 대기`
- 사용자 수용 뒤 Timeline 미적용: `에셋 수용 완료 — 재생 통합 미착수`
- 실제 재생 검증과 사용자 수용까지 완료: 해당 작업의 위험 등급별 완료 게이트에 따라 `완료`

알려진 실패를 차단하는 것이 이 플레이북의 목적이다. 새로운 장면 고유의 문제를 무조건 없다고 가정하지 않으며, 새 문제는 가장 싼 gate에서 중지해 원인을 확인한다.
