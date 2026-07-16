# 작업 로그: 백혈구 회피 대응 경험 스케일링

## 2026-07-09

- 사용자 요청: 기존 백혈구 회피 미니게임 일부 수정 작업 진행.
- 범위 판단: 문서의 첫 구현 후보인 `면역 대응 경험` 카운트를 기존 백혈구 회피 난이도에 연결한다.
- 제한: 백혈구 수, 스폰, 씬 배치는 건드리지 않고 백혈구 속도 배율만 적용한다.
- 주의: `UnityProject/ProjectSettings/ProjectSettings.asset`의 기존 미커밋 변경은 이번 작업 범위 밖이다.
- 테스트 추가: 첫 진입 기본 속도, 반복 진입 강화, 실패 추가 경험, 최대 배율 캡, 컨트롤러 배율 적용을 EditMode 테스트로 고정했다.
- 구현: `PrototypeSessionState`에 면역 대응 경험과 백혈구 속도 배율을 추가하고, `WhiteBloodCellChaser`가 세션 배율을 적용한 `CurrentSpeed`로 이동하도록 연결했다.
- 검증: Unity Test Runner EditMode 69개 통과, MCP Play 체크 통과, Console Error/Warning 0건, 씬 `isDirty=false`.
