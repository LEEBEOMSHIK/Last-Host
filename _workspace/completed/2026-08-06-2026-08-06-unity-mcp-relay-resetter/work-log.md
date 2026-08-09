# 작업 로그

- 2026-08-06: 사용자에게 이름·위치·리소스·검증 방법 승인 후 작업 시작.
- `skill-creator`의 `init_skill.py`로 스캐폴드 생성.
- Windows 코드페이지로 `openai.yaml`이 비 UTF-8 생성되어 손상본을 `artifacts/openai.yaml.invalid`로 격리하고 UTF-8 메타데이터를 재작성.
- 공식 `generate_openai_yaml.py`와 `quick_validate.py`는 로컬 `PyYAML` 부재로 실행 불가.
- 의존성 없는 정적 검사, PowerShell parser, 실제 프로세스 Inspect, `-Apply -WhatIf`로 대체 검증.
- QA correction 1: PID-only 재검증을 relay/parent 시작 시각 InstanceKey와 native Process 객체 종료로 보강. 같은 PID respawn 판정과 문구/WhatIf 결과 수정.
- QA correction 2: ParentProcessId PID 재사용 방지를 위해 `parent start <= relay start` 조건 추가. R2로 재분류.
- 독립 QA 최종 PASS, 총괄 `내부 승인 가능`.
- QA 후보와 전역 설치본 3파일 SHA-256 일치 확인. 실제 relay 종료는 수행하지 않음.
