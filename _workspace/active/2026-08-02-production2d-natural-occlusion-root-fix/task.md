# 작업 배정서

## 기본 정보

- 작업 ID: `2026-08-02-production2d-natural-occlusion-root-fix`
- 작업명: Production2D 자연 부분 가림·실제 충돌 루트 교정
- 상태: 기술 검증 통과 — 사용자 실제 WASD·최종 화면 수용 대기
- 생성일: 2026-08-02 KST
- 담당 에이전트: 프로젝트 조정 에이전트
- 보조 에이전트: 게임플레이 구현 에이전트, Unity 씬/통합 구현 에이전트, 문서/릴리즈 에이전트, QA/검증 에이전트, 프로젝트 총괄 관리자 에이전트
- 사용 스킬: `last-host-design-keeper`, `unity-verification-runner`

## 에이전트 역할과 책임

| 에이전트 | 역할 | 책임 범위 | 예상 산출물 |
| --- | --- | --- | --- |
| 프로젝트 조정 에이전트 | 범위·상태·비용 | S0 charter, 소유권, 보호 범위, 중앙 현황판 | 작업 패킷·현황판 |
| visual footprint analyst | 읽기 전용 정적 계측 | `alpha > 64`, PPU 128, pivot 기준 object polygon·rat capsule·현 box 오차 측정 | `artifacts/visual-footprint-measurement.md` |
| 게임플레이 구현 에이전트 | runtime production owner | whole-hide 제거/대체, collider·방향·footpoint runtime 불변식, 필요 helper·순수/단위 테스트 | gameplay 후보·수치 계약·targeted PASS |
| Unity 씬/통합 구현 에이전트 | scene production owner | builder·TechnicalSample scene·serialized collider/occluder wiring·씬 계약 테스트 | gameplay 인계 반영·rebuild·scene targeted PASS |
| 문서/릴리즈 에이전트 | 후속 문서 owner | 구현 확정 뒤 운영·설계 문서 상태 정합 | 후속 문서 diff |
| QA/검증 에이전트 | 독립 S0·후보 검증 | 원증상, 물리·시각 oracle, lifecycle, MCP Play, canonical evidence | `verification.md`·evidence manifest |
| 프로젝트 총괄 관리자 에이전트 | 최종 내부 승인 | QA 충분성·범위·비용·사용자 수용 대기 감사 | 최종 판정 |

## 구현 담당 확인

- 코드/테스트 변경 담당: gameplay runtime·순수/단위 테스트는 게임플레이 구현 에이전트, scene wiring·씬 계약 테스트는 Unity 씬/통합 구현 에이전트
- 씬/프리팹/입력/UI/ProjectSettings 변경 담당: builder·TechnicalSample scene·serialized wiring은 Unity 씬/통합 구현 에이전트. ProjectSettings는 변경 금지
- 메인 에이전트 직접 구현 여부: 아니오
- 메인 에이전트 직접 구현 예외 사유: 없음

## 루프 게이트

- 게이트 적용 대상: 예
- 위험 등급: R2
- 위험 등급 근거: 사용자 가시 결함, 2D 물리 footprint·정렬·가림 상태, scene lifecycle과 여러 오브젝트가 결합된 상태/통합 수정
- 적용 사유: 이전 기술 검증 PASS가 사용자 화면에서 whole-character hide라는 증상 은폐로 실패해 S0 acceptance부터 재고정해야 함
- QA/검증 필요: 예
- 총괄 관리자 판정 필요: 예
- 커밋 전 차단 조건 확인 필요: 예
- correction cycle: 모든 historical cycle 종료. 최종 `5cd81d7c...` 동결과 r4 evidence audit PASS; 상세 실패 이력은 `agent-activity.md`·`work-log.md`의 날짜별 섹션에 보존
- 재분류 root cause: task template의 one-owner 잔존 문구를 역할표·production 표와 함께 전수 갱신하지 않아 역할표, production 소유권 표, 커밋 게이트가 서로 다른 실행 계약을 가리켰다.

## S0 사용자 원증상·검증 charter

- 사용자 원문 또는 원증상: 현재 수정은 벽·통·상자와 겹칠 때 쥐 `SpriteRenderer` 전체를 꺼 쥐가 사라진다. 사용자는 오브젝트 충돌 범위 불일치 가능성을 지적했고 문제를 숨기는 방식의 재사용을 금지했다.
- 재현 씬·입력·좌표·상태: `RatHost2DTechnicalSample`에서 실제 쥐 root 1개로 wall/barrel/crate 각 오브젝트의 앞·뒤·측면 경계를 연속 WASD로 통과하고 접촉·정렬·부분 가림 상태를 관찰한다.
- 원증상 증거: 이전 작업 `2026-08-02-production2d-visual-overlap-correction`의 사용자 acceptance FAIL. `7ba12df`는 기술 검증을 통과했지만 whole-character hide가 증상 은폐로 판정돼 본 작업으로 대체한다.
- 합성 oracle의 금지 결과:
  - occlusion을 이유로 rat root, `Visual`, `SpriteRenderer`를 비활성화하거나 alpha 0으로 만드는 행위
  - teleport, 입력 잠금, 과도한 collider 확대로 관통을 보이지 않게 만드는 행위
  - 물리 footprint overlap, 보이는 solid base 관통, 경계 pop/jitter
  - 현재 wall/barrel/crate 표본에서 로직으로 쥐 전체를 숨기는 행위
- 합성 oracle의 허용 결과:
  - rat과 renderer는 항상 active/enabled
  - 보이는 solid ground footprint와 정확히 일치하는 collision
  - footpoint 기반 앞뒤 정렬과 foreground alpha에 의한 자연스러운 부분 가림
  - collision을 지키며 오브젝트 뒤로 합법적으로 이동하는 경로
- 완료 주장 한 문장: wall/barrel/crate 주변에서 쥐를 로직으로 숨기지 않고, 실제 solid footprint 충돌과 footpoint 정렬·foreground alpha로 자연스러운 부분 가림을 유지하며 연속 WASD에서도 관통·pop·jitter가 없다.

| criterion ID | 유형 | 입력·상태 | 기대값 | 최소 검증 |
| --- | --- | --- | --- | --- |
| C1 | 원증상·negative control | 3오브젝트 앞/뒤/접촉/이탈 전 구간 | rat active·renderer enabled 100%, hide transition 0 | renderer/active 상태 추적 + 실제 Play |
| C2 | 성공·경계 | wall/barrel/crate object-specific reference polygon × normal 8개 × 3 frames × 좌우 mirror | 모든 normal에서 `-2px <= Δ <= +1px`; visible gap `-1..+2px`; opaque-core intersection `0`; `ColliderDistance2D.isOverlapped=false`; 3-frame stop spread `<=1px`; mirrored stop error `<=1px` | exact polygon 원문 + 구현자 표적 + QA reduced pairwise matrix |
| C3 | 합성 oracle | 앞→경계→뒤 연속 이동 | 자연 부분 가림·앞뒤 정렬·연속 추적 가능, 강제 전체 숨김 0 | 원자 캡처 최대 4 + 육안 대조 |
| C4 | 경계 | 각 대표 경계 왕복 10회 | pop/jitter 0 | reduced boundary harness |
| C5 | 수명주기·negative control | external disabled renderer, scene save/reload, enable/disable | 외부 상태 침범 없음, reload 뒤 상태 계약 보존 | 관련 EditMode lifecycle |
| C6 | 실제 입력 | single-owner lease 아래 연속 WASD/MCP Play | 이동·collision·정렬·가림 연속, Console 0, scene clean | MCP 1 session |
| C7 | 보호 회귀 | 3D legacy 파일·씬 diff | `RatHostPrototype`, `PrototypeCameraController`, V toggle 변경 0 | 보호 diff 감사 |

- C2 support 정의: `support(P,n)=max(dot(p,n))`, `Δ=support(collider,n)-support(reference,n)`. 허용 world 범위는 `-0.015625 <= Δ <= +0.0078125`, visible gap은 `-0.0078125..+0.015625 world`다.
- C2 FAIL: `gap > +2px`는 invisible collider, `gap < -1px` 또는 opaque-core intersection은 visible penetration으로 즉시 FAIL이다.
- C2 normal: wall/crate는 face normal 4개와 인접 normal 합을 normalize한 4개, barrel은 cardinal/diagonal 8개다.
- normative input: `artifacts/visual-footprint-measurement.md`의 exact px/world polygon·normal·pivot·rat capsule 값을 사용한다.
- QA S0 사전 검토: `natural-occlusion-s0-r4-footprint-contract` PASS. 이후 gameplay·scene·fixture 구현과 검증을 마쳤고, 최종 `5cd81d7c...`에 대한 r4 evidence audit가 PASS했다. 상세 실패 이력은 `agent-activity.md`·`work-log.md`의 날짜별 섹션에서만 관리한다.

## 목적

쥐 전체를 숨겨 겹침을 감추는 이전 방식을 폐기하고, 실제 보이는 solid base와 충돌 footprint·footpoint 정렬·foreground alpha를 일치시켜 자연스러운 2D 아이소메트릭 가림을 구현한다.

## 입력 자료

- 사용자 acceptance FAIL과 현재 화면 피드백
- `_workspace/active/2026-08-02-production2d-visual-overlap-correction/`
- `_workspace/completed/2026-08-02-2026-08-02-loop-harness-efficiency-audit/`
- `docs/agents/loop-engineering-gates.md`
- `artifacts/visual-footprint-measurement.md`

## 해야 할 일

1. 독립 QA가 S0 원증상·금지/허용 oracle·criterion 추적을 사전 검토한다.
2. 게임플레이 구현 owner가 runtime 후보·수치 계약과 targeted PASS를 먼저 확정한다.
3. 명시 인계 뒤 Unity 씬/통합 owner가 builder·scene wiring을 적용하고 rebuild·scene targeted PASS를 수행한다.
4. S1~S5 fail-fast 후 freeze 후보에서 QA 관련/전체/MCP/matrix/capture 예산을 한 번씩만 사용한다.
5. 사용자 실제 WASD 수용 전 `완료`를 선언하지 않는다.

## 산출물

- 구현 후보 diff와 관련 테스트
- `verification.md`와 canonical evidence manifest
- 최대 4장의 canonical capture
- 사용자 수용 대기까지 동기화된 상태·비용 문서

## production 소유권과 검증 예산

| production 파일/불변식 | 전용 owner | 변경 금지/인계 조건 |
| --- | --- | --- |
| `VisualOcclusionResolver2D.cs` whole-hide 제거/대체 | 게임플레이 구현 에이전트 | renderer/rat hide·alpha 0 금지; scene owner 편집 금지 |
| `RatSide3FrameView.cs` collider·방향·footpoint runtime 불변식 | 게임플레이 구현 에이전트 | stable CapsuleCollider2D size `(1.2265625,0.25)`, offset X `±0.28515625`, Y `0.125`; 3-frame switch 중 resize 금지; scene owner 편집 금지 |
| 필요 새 runtime helper·순수/단위 테스트 | 게임플레이 구현 에이전트 | gameplay 후보·수치 계약·targeted PASS 전 scene 인계 금지 |
| `RatHost2DProductionSampleSceneBuilder.cs` | Unity 씬/통합 구현 에이전트 | gameplay 명시 인계 뒤 wall/barrel/crate `PolygonCollider2D` 적용; gameplay owner 편집 금지 |
| `RatHost2DTechnicalSample.unity`·serialized collider/occluder wiring·씬 계약 테스트 | Unity 씬/통합 구현 에이전트 | exact reference polygon과 gameplay 수치 계약을 임의 변경하지 않고 apply/rebuild |
| 정책·운영 문서 | 문서/릴리즈 에이전트(구현 확정 후) | 구현 전 정책 문서 수정 금지 |

### 상태·불변식 소유권 단일 진실

| 상태·불변식 | owner | 소유 범위 | 인계 전 무효화 | 인계 후 무효화 |
| --- | --- | --- | --- | --- |
| runtime visibility | 게임플레이 구현 에이전트 | rat/root/renderer 활성 상태와 whole-hide 금지 | 변경 시 gameplay 표적 근거와 모든 후속 scene·QA 근거 무효 | scene owner가 변경 금지; 변경 필요 시 gameplay로 되돌려 새 후보·인계 |
| collider direction | 게임플레이 구현 에이전트 | 방향별 runtime collider 상태·전환 | 변경 시 gameplay 표적 근거와 모든 후속 scene·QA 근거 무효 | scene owner가 수치 변경 금지; 변경 필요 시 인계 취소 |
| footpoint | 게임플레이 구현 에이전트 | runtime footpoint 계산·정렬 입력 | 변경 시 gameplay 표적 근거와 모든 후속 scene·QA 근거 무효 | scene owner가 계산 계약 변경 금지; 변경 필요 시 인계 취소 |
| serialized object footprint | Unity 씬/통합 구현 에이전트 | 오브젝트별 직렬화 footprint·occluder 값 | gameplay 후보를 변경하지 않으며 scene 작업 시작 전 근거 없음 | 변경 시 scene 표적 근거와 모든 후속 QA 근거 무효 |
| builder | Unity 씬/통합 구현 에이전트 | builder 생성·적용 계약 | gameplay release와 명시 인계 전 편집 금지 | 변경 시 scene 표적 근거와 모든 후속 QA 근거 무효 |
| scene | Unity 씬/통합 구현 에이전트 | TechnicalSample scene과 serialized wiring | gameplay release와 명시 인계 전 편집 금지 | 변경 시 scene 표적 근거와 모든 후속 QA 근거 무효 |
| scene tests | Unity 씬/통합 구현 에이전트 | scene serialization·wiring 계약 테스트 | gameplay release와 명시 인계 전 편집 금지 | 변경 시 scene 표적 근거와 모든 후속 QA 근거 무효 |
| policy docs | 문서/릴리즈 에이전트 | 구현 확정 뒤 정책·운영 문서 | 구현 전 변경 금지 | acceptance 계약을 바꾸면 S0부터 무효, 상태 동기화만이면 production 근거 유지 |

- shared file: `0`. owner가 다른 두 역할의 공동 파일 편집은 허용하지 않는다.
- 수치 변경 통제: exact polygon·normal set·rat capsule·tolerance는 S0 baseline이다. owner가 바꾸려면 구현 전에 QA contract revision과 독립 사전 검토가 필요하다.

- 구현 순서: gameplay owner 후보·수치 계약·targeted PASS → 명시 인계 → scene owner 적용·rebuild·scene targeted PASS
- Unity session lease 예정 소유자: gameplay owner의 Unity 사용 필요 시 `gameplay_implementation`, 명시 release·인계 뒤 `unity_scene_integration`; QA 단계 독립 인계 후 `qa_verification`
- 관련 suite: 신규 자연 가림·footprint·lifecycle 표적 + TechnicalSample2D 관련 suite
- 전체 suite 실행 조건: 동일 fingerprint에서 S1~S5 green, candidate frozen 후 QA 1회
- 대형 matrix 실행 필요·근거: 전체 Cartesian 대신 3오브젝트 reduced pairwise matrix 1회
- artifact budget / criterion별 canonical 증거: manifest 1개, capture 최대 4, raw logs commit 금지

## 비용 계획·실제

| 비용 항목 | 계획 | 실제·근거 |
| --- | --- | --- |
| 역할·인계 | 조정1, gameplay1, scene1, 문서1, QA1, 총괄1 | 조정1, visual footprint analyst1, gameplay1, scene1, 문서1, stale fixture test owner1, 독립 QA1, 총괄1차 감사1. 모든 production/test owner release 완료, 총괄 read-only 재감사 대기 |
| 표적 검증 | 구현자 targeted 1묶음, QA targeted+관련 1묶음 | 현재 정본: gameplay `3/3`, scene `8/8`, stale fixture `4/4`, 모두 PASS |
| Unity/MCP/빌드·full suite | MCP 1 session, freeze 후 full suite 1회, build 0 | 현재 정본: full EditMode `203/203` PASS, QA Play r3 PASS, build 0. r4 evidence audit 동적 실행 0 |
| matrix/capture·artifact | 3-object reduced pairwise matrix 1회, canonical capture 최대 4, raw logs commit 금지 | QA Play r3 축소 왕복·Y-sort 검증 PASS, canonical manifest 1개, capture 0 |
| correction·무효/폐기·비용 판정 | r1/r2 `2/2` 뒤 재분류, 새 cycle 최대 2회, token/$ 미집계 | 최종 후보 `5cd81d7c...`, canonical run `natural-occlusion-final-evidence-r1-20260802`, r4 evidence audit PASS. historical 실패·no-result 실행은 현재 정본 수치에 합산하지 않고 역사 섹션에서 추적. 비용 `과다 — 부분 회피 가능`, token/$ 미집계 |

- 중앙 현황판 행: `docs/project-handoff/task-cost-dashboard.md`
- 비용 판정: **과다 — 부분 회피 가능**. exact token/$는 미집계다. 현재 정본 증거는 canonical manifest 하나로 단일화했고, 과다 판정의 원인이 된 historical correction·no-result는 날짜별 이력과 중앙 비용 현황판에서 분리 추적한다.

## 에이전트 수행 이력 기록

- `agent-activity.md` 생성 여부: 예
- 담당 에이전트별 수행 내용 기록 여부: gameplay·scene·문서·fixture·QA final 결과까지 기록 완료
- 위임/검토/승인 판정 기록 여부: QA r4 evidence audit와 총괄 1차 문서 정합 반려까지 갱신 완료

## 금지 범위

- Stage2/Stage3 코드·씬·문서
- ProjectSettings
- 3D legacy `RatHostPrototype`, `PrototypeCameraController`, V toggle 관련 파일
- art assets와 사용자 reference
- 정책 문서의 구현 전 변경
- build 실행

## 승인 필요 항목

- 본 작업은 사용자의 기존 2D 쥐 숙주 승인 범위와 명시적 수정 요청 안이다.
- 새 패키지·에셋·ProjectSettings·3D legacy·범위 변경은 별도 승인 없이는 금지한다.

## 커밋 전 차단 조건

- `_workspace` 작업 패킷 확인: 생성 완료
- 담당 에이전트 산출물 확인: gameplay·scene·stale fixture·정책 산출물과 owner release 완료
- 에이전트 수행 이력 확인: 독립 QA r4 evidence audit와 총괄 1차 문서 정합 반려까지 기록 완료
- 구현 담당 에이전트 확인: gameplay owner 1 + scene owner 1. gameplay release → 명시 handoff → scene owner acquire/apply 순서를 고정
- 메인 에이전트 직접 구현 예외 사유 확인: 해당 없음
- QA/검증 에이전트 기록 확인: final `5cd81d7c...`, gameplay `3/3`, scene `8/8`, stale fixture `4/4`, full `203/203`, QA Play r3 PASS, r4 evidence audit PASS
- 총괄 관리자 판정 확인: 1차 문서 정합 반려 — 상태-only 교정 후 read-only 재감사 대기
- 승인 게이트 확인: 기존 사용자 수정 요청 범위
- 완료 판단에 영향을 주는 미검증 항목: 사용자 실제 연속 WASD·최종 화면 수용, 총괄 read-only 재감사

## 완료 기준

- C1~C7이 같은 freeze candidate에서 canonical evidence로 통과한다.
- whole-character hide·alpha 0·teleport·입력 잠금·과도한 collider 확장이 없다.
- QA PASS와 총괄 내부 승인 뒤 사용자가 실제 WASD와 자연 부분 가림을 수용한다.
