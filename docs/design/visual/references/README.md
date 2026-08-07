# 비주얼 레퍼런스

이 폴더는 `마지막 숙주 / The Last Host`의 승인된 시각 방향을 설명하는 참고 이미지를 보관한다. 참고 이미지는 목표 분위기와 화면 구성을 전달하는 자료이며, 별도 제작·검증 없이 게임에 바로 사용하는 최종 에셋이 아니다.

## 현재 기준 이미지

### `rat-host-2d-isometric-gameplay-mockup-v1.png`

- 생성 도구: OpenAI 내장 `imagegen`(ChatGPT 이미지 생성)
- 생성·채택일: 2026-07-27
- 입력 외형 참고: `_workspace/completed/2026-07-27-2026-07-24-rat-final-appearance-sample/artifacts/ai-concepts/rat-concept-a-natural.png`
- 용도: 2D 아이소메트릭 도트 하수도 화면의 목표 분위기, 쥐의 자연스러운 실루엣, 환경 밀도, 명암 깊이, HUD 배치 방향을 전달한다.
- 상태: 프로젝트 2D 전환의 목표 화면 reference

### `bacteriophage-base-character-reference-v1.png`

- 채택일: 2026-08-06 KST
- 입력 출처: 사용자 제공 `../../../references/images/image.png`와 기록된 후속 프롬프트
- 원본 처리: 원본을 수정하지 않고 비파괴 복사
- 해상도·크기: `1036×1248`, `1,882,663 bytes`
- SHA-256: `0C1D22C07C0CAC8B2F70D7BEFCFB5FA5E6ECB66D0F183125DFF359D33CEA039F`
- 용도: 가장 기본 바이러스인 박테리오파지의 외형, 성격, 업그레이드 실루엣 불변식 reference
- 상태: 사용자 채택 canonical character reference; 실제 2D 스프라이트나 Unity 에셋이 아님
- 상세 기준: `../characters/base-bacteriophage-character.md`

### `startup-bacteriophage-food-chain-background-v1.png`

- 채택일: 2026-08-07 KST
- 입력 출처: 사용자 제공·선택 `image.png`
- 해상도·크기: `1672×941`, `2,812,443 bytes`
- SHA-256: `5ED62B0BE9E0FC68FED15135C8BEDB3F08639CD020E914EF420FE73831B17C8D`
- 용도: 박테리오파지가 여러 숙주와 먹이사슬을 따라 이동하는 게임 콘셉트를 전달하는 PC 시작 화면 배경
- 상태: 사용자 선택 시작 화면 배경. Unity 적용 승인됨
- Unity import copy: `../../../../UnityProject/Assets/_Project/Art/Production2D/V1/UI/Startup/startup-bacteriophage-food-chain-background-v1.png`
- 한계: 시작 화면 전용이며 반복 타일·방향별 스프라이트·최종 게임플레이 아트로 확대 해석하지 않는다.

## 해석 한계

- 이 이미지는 반복 가능한 타일셋, 방향별 캐릭터 스프라이트 시트, 애니메이션 프레임, 충돌 맵, 깊이 정렬 데이터가 아니다.
- 이미지 안의 타일 규격, 투시, 오브젝트 간격, HUD 수치와 아이콘을 구현 규격으로 자동 확정하지 않는다.
- 실제 에셋은 공통 픽셀 격자, 아이소메트릭 타일 규칙, 방향·프레임 일관성, 피벗, 정렬, 충돌을 별도로 설계하고 Unity 플레이 화면에서 검증한다.
- OpenAI 이미지 생성 결과는 후보·브리프·목표 화면으로 사용할 수 있지만, 사람 선별과 후속 제작·QA 없이 최종 게임 에셋으로 자동 승인하지 않는다.
- 박테리오파지 reference의 3D voxel 표현은 외형 탐색 방식이다. production은 2D 아이소메트릭/쿼터뷰 도트로 재설계한다.
- 박테리오파지 이미지의 `150nm`, 행동·업그레이드 설명은 자동으로 기획 수치나 게임 규칙이 되지 않는다.
- 사용자 프롬프트 원문에 포함된 특정 회사·캐릭터 스타일 표현은 출처 artifact 밖의 제작 지시로 재사용하지 않는다.

## 사용 규칙

1. 새 시안은 이 목업의 분위기와 가독성을 비교 기준으로 삼는다.
2. 그대로 복사할 요소가 아니라 `자연형 캐릭터`, `조밀하지만 읽히는 환경`, `고정 아이소메트릭 시점`, `명확한 게임플레이 우선순위`를 추출한다.
3. 생성형 이미지 작업은 `../../../prototype/plans/rat-host-ai-assisted-art-workflow.md`와 `.agents/chatgpt-image-art-agent.md`를 따른다.
4. 새 reference를 추가할 때는 생성 도구·날짜·입력 출처·용도·한계를 이 문서 또는 작업 패킷에 기록한다.
5. 작업 `2026-08-06-virus-character-concept-v1`의 A/B/C는 `SUPERSEDED` 탐색 이력이며 현재 박테리오파지 제작 기준으로 사용하지 않는다.
