# 시네마틱 제작 실패 회고·재발 방지 플레이북

## 요청

- A01 시네마틱 에셋 작업의 생성·적용·검증 실패 원인을 프로젝트 내부에 정리한다.
- 다음 시네마틱에서는 동일한 실패와 토큰 낭비가 반복되지 않도록 재사용 절차를 만든다.
- 생성된 에셋과 가공·검증 도구는 이후 게임 에셋 제작에 재사용할 수 있게 경계를 남긴다.

## 등급과 범위

- 위험 등급: `R1`
- 근거: 기획·제작 절차 문서와 단일 작업 기록만 작성한다. Unity, 코드, 에셋, ProjectSettings, 패키지, 승인 범위와 에이전트 책임은 변경하지 않는다.
- owner: 프로젝트 조정
- 현재 단계: 사용자 written spec 승인 완료·최소 참조 연결 검증 완료

## 변경 파일

- `docs/design/narrative/cinematic-production-failure-prevention-playbook.md`
- `docs/design/narrative/pixel-art-motion-comic-cinematic-guide.md`
- `.agents/pixel-cinematic-director-agent.md`
- `docs/agents/agent-reference-map.md`
- `docs/superpowers/plans/2026-08-11-cinematic-playbook-reference-linking.md`
- `_workspace/active/2026-08-11-cinematic-pipeline-retrospective/record.md`

## 금지 범위

- A01 production 에셋 또는 Unity Import 설정 변경
- 새 이미지 생성과 correction 호출
- Timeline·씬·프리팹·코드 구현
- 기존 역할 책임이나 승인 게이트 변경
- 승인된 계획 밖의 기존 문서·에이전트·참조 맵 변경

## 완료 주장

- A01의 실제 실패를 계획·생성·가공·Unity Import·검증 래퍼·비용 계층으로 분류한다.
- 각 원인에 다음 작업의 선행 차단 조건과 저비용 검증을 연결한다.
- A01 최종 에셋·source·도구의 재사용 위치와 승계 금지 범위를 기록한다.
- 사용자가 written spec을 검토한 뒤 기존 시네마틱 가이드·에이전트·참조 맵 연결을 별도 최소 변경으로 진행한다.

## 표적 검증

- 임시 표식과 의미가 확정되지 않은 항목이 없어야 한다.
- 새 문서의 프로젝트 상대 경로가 실제로 존재해야 한다.
- A01 `verification.md`의 failure evidence와 원인표가 모순되지 않아야 한다.
- `git diff --check`가 통과해야 한다.

## correction·QA·비용

- correction: `1/2` — 최초 staged file-set 검사가 Windows expected 경로의 `\\`와 Git 출력의 `/`를 정규화하지 않아 내용과 무관하게 실패했다. 경로 구분자를 `/`로 통일한 동일 검사로 재검증한다.
- 계획 correction: `1/2` — 참조 연결 뒤에도 플레이북이 자신을 `현재 활성 게이트가 아니다`라고 표시하는 상태 모순이 초기 계획에서 누락됐다. 플레이북 상태 문장 한 줄을 활성 참조로 동기화하고 나머지 본문은 변경하지 않는다.
- 저비용 검증: 세 대상의 플레이북 경로 각 `1`, 플레이북 활성 상태 `1`, 이전 비활성 상태 `0`, 연출 역할 금지 범위 보존, 변경 allowlist·placeholder·`git diff --check` PASS.
- 독립 읽기 검토: `PASS — blocker 0건`. 경로 정확성, 역할 확대 부재, 플레이북 본문 중복 부재, 상위 게이트 비대체와 R1 유지 적합성을 확인했다.
- 프로젝트 총괄 written spec 검토: `PASS — blocker 0건`. 제안 상태 명시와 Unity `6000.4.6f1` Multiple Sprite 문맥 한정 권고를 반영했다.
- Unity/MCP/build/imagegen: `0`
- 비용 판정: `주의` — 저비용 staged 경로 정규화 correction 1회와 계획 상태 문장 correction 1회가 있었으며 Unity/MCP/build/imagegen 실행은 `0`이다.

## 현재 상태

- 플레이북 written spec 작성·자체 검토·프로젝트 총괄 검토·사용자 승인 완료
- 플레이북을 프로젝트 필수 제작 참조로 활성화하고 세 기존 문서의 최소 참조 연결 완료
- 저비용 검증과 독립 읽기 검토 PASS, Unity/MCP/build/imagegen `0`
