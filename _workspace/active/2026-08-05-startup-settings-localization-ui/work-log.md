# 작업 로그

## 작업 ID

`2026-08-05-startup-settings-localization-ui`

## 로그

### 2026-08-05 작업 접수·R3 분류

- 수행 내용: 사용자 승인과 다국어 고려 요청을 Startup/Settings V1 범위로 고정했다.
- 확인한 자료: 승인 문서, Unity 구조·MCP·루프 게이트, 현재 Build Settings·UI 코드.
- 판단: 새 씬·PlayerPrefs 저장 형식·Build Settings가 결합되어 R3다. 한국어·영어 실제 전환과 확장 가능한 키 구조를 포함하고 신규 패키지·아트·오디오는 제외한다.
- 루프 게이트 상태: S0 계약 작성, 아키텍처·QA 사전 검토 대기.
- 다음 작업: R3 사전 구조 검토와 S0 계약 검토.

### 2026-08-05 S0 correction r1

- 수행 내용: QA가 C3의 적용 전 무변경과 C5의 언어 즉시 갱신 사이 모호성을 첫 blocker로 반환했다.
- 판단: 언어만 Draft 상태에서 즉시 UI preview하고, 해상도·전체화면·VSync와 PlayerPrefs는 적용 전 바꾸지 않는다. 취소/Esc는 저장 언어와 전체 문자열을 원자 복원한다.
- 비용: S0 계약 검토 1회, Unity/MCP/build/TestRunner 0, correction `1/2`.
- 다음 작업: 같은 QA가 보정된 C3~C5를 한 번 재대조한다.

### 2026-08-05 S0 r2 재분류

- 문제: r1에서 C3/C5 충돌은 해소됐으나 C4/C6의 최초 실행·기본값·손상값 복구 프로필이 미지정이었다.
- 영향: `기본값`과 손상 저장 복구가 PC/구현자 판단에 따라 달라져 단일 oracle이 없었다.
- 재분류: `startup-settings-s0-default-profile-r2-20260805`, R3와 owner는 유지한다.
- 추천/확정: 한국어, `FullScreenWindow`, VSync 1, 1920×1080 우선→지원 16:9 최고→지원 최고→현재 화면값 순 fallback. 일부 키 손상도 전체 기본 프로필로 원자 복귀한다.
- 비용: S0 r0/r1 계약 blocker 2회, Unity/MCP/build/TestRunner 0, 동적 증거 폐기 0. 새 S0 revision correction 0/2.
- 다음 작업: S0 r2 최종 재검토 후 추가 계약 반복 없이 R3 총괄 사전 판정으로 넘긴다.

## 결정 기록

- 시작 버튼 대상은 전체 핵심 루프 씬 `RatHost2DPrototype`이다.
- `RatHostPrototype`은 3D 레거시로 보존한다.
- 다국어 V1은 한국어/영어, 누락 키 fallback은 영어로 한다.
- 설정 V1은 언어·화면 모드·해상도·VSync·조작 안내만 포함한다.

## 열린 질문

- 없음. 추가 언어·외부 폰트·오디오는 후속 승인으로 분리한다.

## 위험과 주의점

- Unity UI Text의 한글 glyph와 긴 영문 레이아웃을 실제 Game View에서 확인해야 한다.
- 현재 build capability는 unavailable이므로 Windows 빌드 성공을 자동 완료 근거로 주장하지 않는다.

## 게이트 진행 상태

- 작업 배정 게이트: 작성 중
- 담당 산출물 게이트: 대기
- QA/검증 게이트: S0 r0/r1 BLOCKER → r2 재분류 최종 검토 대기
- 총괄 관리자 게이트: R3 사전 검토 대기
- 커밋 전 차단 조건: 유지
