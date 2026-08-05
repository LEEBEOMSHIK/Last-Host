# 독립 QA r4 — S6 증거 감사

## 판정

`PASS — 기술 검증 통과, 실제 WASD·최종 화면 사용자 수용 대기`

- canonical run: `natural-occlusion-final-evidence-r1-20260802`
- final test-corrected candidate: `sha256:5cd81d7c836fb2561f9f416c20adeeec00f6ef960153b8380b32c7fafbef5db6`
- canonical manifest: `canonical-evidence-r1.json`
- 이번 감사 실행: Unity/MCP/TestRunner/build/test/capture 모두 0.

## 후보 연속성

- final manifest 9개 파일을 현재 파일과 다시 해시 대조해 mismatch 0을 확인했다.
- scene r2 manifest와 공유하는 production·scene·package·version 6개는 모두 동일 해시다.
- gameplay manifest와 공유하는 runtime 2개도 모두 동일 해시다.
- 최종 correction에서 달라진 범위는 stale fixture인 `RatHost2DTechnicalSampleSceneTests.cs` 하나다. Box 기대를 Polygon 정본으로 이관했으며 production·scene runtime은 변경되지 않았다.
- 따라서 gameplay `3/3`, scene r2 `8/8`, QA Play r3 PASS는 test-only correction으로 무효화되지 않고 final candidate에 계속 유효하다.

## S6 결과 감사

- `stale-fixture-targeted-r1.xml`: `4/4 PASS`, failed/skipped/inconclusive 0, exit 0.
- `full-editmode-r2.xml`: `203/203 PASS`, failed/skipped/inconclusive 0, strict `valid_pass=true`, exit 0.
- `full-editmode-r1.xml`: `200/203 FAIL` historical/SUPERSEDED. 실패 3개는 모두 과거 BoxCollider2D fixture 계약이며 targeted 4/4와 full r2가 대체한다.
- 같은 fingerprint full suite 중복은 없다. r1은 blocker 발견 run, r2는 test-only correction 뒤 최종 확인 run이다.

## 최종 범위

- 기술 증거: gameplay 3/3, scene 8/8, stale fixture 4/4, full EditMode 203/203, QA Play r3 PASS, Console 0·scene clean.
- 3D legacy: `RatHostPrototype`, `PrototypeCameraController`, V toggle 변경 0이며 legacy preservation test가 최종 passing suite에 포함된다.
- 미완료: MCP가 실제 키 이벤트를 주입하지 못하므로 연속 WASD, 최종 Game View의 자연 부분 가림과 전체 소실 0은 사용자 수용 대기다.
- 완료 표현: `기술 검증 통과 — 사용자 수용 대기`; 사용자 확인 전 작업 완료·커밋 승인으로 승격하지 않는다.

## 비용 감사

- 이번 S6 audit 자체 비용은 정적 manifest/diff/XML 읽기 1묶음뿐이며 동적 실행 0이다.
- 전체 작업 비용 판정은 `과다 — 부분 회피 가능`이다. 이유는 historical no-result Unity/MCP 요청과 QA Play 하네스 correction 2회가 있었기 때문이다.
- 필요한 비용: gameplay/scene targeted, initial scene 침투 correction, stale fixture targeted, blocker 발견 full r1과 final full r2, 최종 공개 API Play.
- 회피 가능 비용: S0 stale 계약 재검토 일부, MCP no-result 요청 2건, QA r1 Rigidbody/Transform self-check 누락과 r2 reflection 권한 blocker로 발생한 Play 세션 2회.
