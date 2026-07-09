# 작업 패킷: 비활성 CharacterController Move 오류 수정

## 목표

쥐 숙주 업데이트 중 모드 전환 또는 컨트롤러 비활성화가 발생했을 때 `CharacterController.Move called on inactive controller` 오류가 발생하지 않도록 한다.

## 오류

```text
CharacterController.Move called on inactive controller
UnityEngine.CharacterController:Move (UnityEngine.Vector3)
LastHost.Prototype.Host.RatHostController:Update () (at Assets/_Project/Scripts/Host/RatHostController.cs:62)
```

## 원인 가설

`RatHostController.Update()` 중 `ApplyForcedControlDemerit()`가 면역 경계도를 올리고, 그 결과 `PrototypeSessionController`가 내부 미니게임으로 전환하면서 쥐 루트 또는 컨트롤러가 비활성화된다. 같은 `Update()`가 이후 계속 진행되어 비활성 `CharacterController`에 `Move()`를 호출한다.

## 구현 범위

- RatHostController가 비활성화되었거나 RatHost 모드가 아니면 같은 프레임 이동을 중단
- CharacterController가 비활성 상태면 `Move()` 호출 방지
- EditMode 회귀 테스트 추가
- Unity 생성 `.meta` 파일 포함

## 비범위

- 면역 신호 억제 미니게임 설계 변경
- 기본 내부 미니게임 선택 방식 변경
- `ProjectSettings.asset`의 기존 define 변경

## 수용 기준

1. 비활성 CharacterController에 `Move()`를 호출하지 않는다.
2. 강제 조종 디메리트로 모드가 전환되어도 같은 프레임에서 쥐 이동 처리를 중단한다.
3. 기존 쥐 이동/강제 조종 테스트가 회귀하지 않는다.
4. Unity MCP가 가능하면 콘솔 오류를 확인하고, 불가능하면 대체 검증을 기록한다.
