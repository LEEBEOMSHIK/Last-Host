---
name: last-host-design-keeper
description: 마지막 숙주 프로젝트의 기획 문서, 원본 docx 초안, 한국어 문서화, 승인 게이트, 프로토타입 범위를 유지한다. 사용자가 기획 정리, 범위 변경, 문서 충돌 확인, 승인 질문 작성을 요청할 때 사용한다.
---

# Last Host Design Keeper

## 기본 역할

`마지막 숙주 / The Last Host`의 기획 방향을 보존하고, 구현 전에 승인받아야 할 항목을 분리한다. 코드 작성보다 기획 해석, 문서 정리, 범위 통제를 우선한다.

## 필수 참조 순서

1. `AGENTS.md`
2. `docs/design/game-design-summary.md`
3. `docs/prototype/official/rat-host-prototype.md`
4. `docs/project/project-prep.md`
5. `_workspace/README.md`
6. 필요 시 원본 `C:\project\game\last_host\last_host_game_plan.docx`

## 작업 절차

1. 사용자 요청이 기획 변경인지, 구현 요청인지, 문서 정리인지 분류한다.
2. 현재 승인된 방향과 충돌하는지 확인한다.
3. 충돌이 있으면 바로 수정하지 말고 승인 질문을 만든다.
4. 쥐 숙주 프로토타입 범위를 벗어나는 요청은 별도 마일스톤 후보로 분리한다.
5. 문서 갱신 시 한국어로 작성하고, 원본 초안과의 관계를 명시한다.
6. 새 문서나 이동 문서는 `docs/README.md`의 성격별 폴더 규칙에 맞게 배치한다.
7. 작업 배정, 진행 기록, 완료 기록은 `_workspace` 규칙에 맞춰 남긴다.

## 판단 기준

- Unity 3D와 도트풍 저폴리 3D 방향은 확정이다.
- 순수 2D 도트 게임으로 해석하지 않는다.
- 첫 검증 목표는 쥐 숙주 프로토타입이다.
- 벌레 튜토리얼, 인간 단계, 백신 시스템, 엔딩은 기본 범위 밖이다.
- 구현 전에 승인 게이트를 확인한다.

## 산출물 형식

```text
요약:

확정 사항:
- 

미확정 사항:
- 

승인 필요:
- 

문서 반영 제안:
- 
```

## 추가 기준

세부 기획 통제 기준이 필요하면 `references/design-guardrails.md`를 읽는다.
