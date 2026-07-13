# 게임 기획 문서 색인

`docs/design/`은 게임의 의도, 규칙, 시스템, 콘텐츠 상세 기획을 보관한다. 구현 계획, 승인 이력, 검증 기록은 이 폴더에 두지 않는다.

## 폴더 구조

- `overview/`: 게임 전체 방향, 플레이어 경험, 핵심 기둥
- `loops/`: 반복 플레이 루프, 모드 전환, 세션 흐름
- `systems/`: 면역 경계도, 숙주 생명력, 변이, 감염, 백신 같은 수치/규칙 시스템
- `hosts/`: 쥐, 벌레, 인간 등 숙주별 조작, 능력, 제한, 전이 조건
- `environments/`: 하수도, 인간 생활권 등 장소별 구조, 위험 요소, 탐험 규칙
- `interactions/`: 배관, 문, 먹이, 감염 기회 같은 상호작용 오브젝트와 행동 결과
- `encounters/`: 내부 면역 미니게임, 백혈구, 항체, 위험 패턴, 조각 수집
- `progression/`: 변이 해금, 숙주 단계, 장기 성장, 엔딩 방향
- `ui-feedback/`: HUD, 프롬프트, 경고, 플레이어 피드백 규칙
- `balancing/`: 수치 기준, 튜닝 테이블, 난이도 조정 기록
- `narrative/`: 세계관, 테마, 스토리 사건, 문구 톤
- `glossary/`: 용어 정의와 명명 기준
- `visual/`: 도트풍 저폴리 3D의 모델, 픽셀 텍스처, 렌더링, 카메라, 조명, UI 제작 기준

## 현재 문서

- `../game-design-summary.md`: 현재 게임 방향 요약. 상세 기획 문서를 쓰기 전의 상위 기준이다.
- `systems/immune-alert.md`: 면역 경계도 상승 원리, 실제 면역 반응에서 빌린 설계 축, 현재 프로토타입 수치.
- `interactions/noisy-pipe-risk-interaction.md`: 소음 배관 상호작용이 왜 면역 경계도를 올리는지에 대한 오브젝트 기획.
- `ui-feedback/immune-alert-feedback.md`: 면역 경계도 상승 원인과 변화량을 플레이어에게 보여주는 피드백 규칙.
- `hosts/host-instinct-control.md`: 숙주 본능과 플레이어 강제 조종이 면역 경계도에 연결되는 기준.
- `encounters/internal-immune-response-minigame-types.md`: 내부 미니게임을 백혈구 전투로 고정하지 않기 위한 유형 기준.
- `encounters/immune-signal-suppression-rhythm-minigame.md`: 면역 신호 억제 리듬형 미니게임 상세 기준.
- `progression/host-experience-traits.md`: 바이러스 변이와 별개로, 숙주 경험을 통해 얻는 장기 특성 구조.
- `visual/pixel-lowpoly-3d-production-guide.md`: 순수 2D가 아닌 도트풍 저폴리 3D를 위한 모델·픽셀 텍스처·URP 렌더링·카메라·UI의 권장 시작 기준과 씬 검토 순서.
- `../prototype/plans/rat-host-ai-assisted-art-workflow.md`: AI 보조 래스터 초안의 자산 묶음 승인·선별·3D/Unity 분리·QA 순서. 실제 생성·적용 계획은 `prototype/`에 둔다.

## 작성 규칙

- 게임 규칙의 이유와 플레이어가 이해해야 할 의미는 `docs/design/`에 적는다.
- 구현 방법, 파일 경로, 테스트 절차는 `docs/prototype/` 또는 `_workspace/`에 둔다.
- 특정 오브젝트가 어떤 시스템 값을 바꾸는 경우 `interactions/` 문서와 `systems/` 문서를 함께 연결한다.
- 예: 긴 소음 배관 상호작용은 `interactions/`에 오브젝트 의도와 플레이 결과를 쓰고, 면역 경계도 상승 규칙은 `systems/`에 쓴다.
