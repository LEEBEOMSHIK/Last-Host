using LastHost.Prototype.Core;
using LastHost.Prototype.Input;
using UnityEngine;

namespace LastHost.Prototype.Host
{
    public sealed class RatHostController : MonoBehaviour
    {
        public PrototypeSessionController session;
        public float baseSpeed = 3.2f;
        public Vector2 xBounds = new Vector2(-5.4f, 5.4f);
        public Vector2 zBounds = new Vector2(-3.4f, 3.4f);
        public Vector3 hostInstinctMoveDirection = Vector3.forward;
        public float passiveInstinctSpeedMultiplier = 0.45f;
        public float forcedControlSpeedMultiplier = 0.65f;
        public float forcedControlConflictDotThreshold = -0.25f;
        public float forcedControlAlertAmount = 6f;
        public float forcedControlAlertCooldownSeconds = 1.2f;
        public string forcedControlFeedbackLabel = "강제 조종";
        public bool randomizeInstinctDirection = true;
        public Vector2 instinctTurnIntervalRange = new Vector2(1.2f, 2.8f);
        public float instinctTurnAngleDegrees = 55f;
        public bool enableInstinctPauses = true;
        public Vector2 instinctMoveDurationRange = new Vector2(1.1f, 2.3f);
        public Vector2 instinctPauseDurationRange = new Vector2(0.25f, 0.75f);

        private CharacterController characterController;
        private Vector3 currentHostInstinctMoveDirection;
        private float nextForcedControlAlertTime;
        private float nextInstinctTurnTime;
        private float nextInstinctActivityTime;
        private bool isHostInstinctPaused;

        public Vector3 CurrentMoveWorldDirection { get; private set; }
        public Vector3 CurrentResolvedMoveDirection { get; private set; }
        public Vector3 CurrentHostInstinctMoveDirection => currentHostInstinctMoveDirection;
        public bool IsHostInstinctPaused => isHostInstinctPaused;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            currentHostInstinctMoveDirection = randomizeInstinctDirection
                ? RatHostInstinctWanderModel.CreateInitialDirection(
                    hostInstinctMoveDirection,
                    Random.value < 0.5f ? -1f : 1f,
                    Random.Range(25f, 135f))
                : NormalizeOrFallback(hostInstinctMoveDirection, Vector3.forward);
            ScheduleNextInstinctTurn(Time.time);
            isHostInstinctPaused = false;
            ScheduleNextInstinctActivity(Time.time);
        }

        private void Update()
        {
            var playerInput = ReadMoveInput();
            if (playerInput.sqrMagnitude > 1f)
            {
                playerInput.Normalize();
            }

            UpdateHostInstinctDirection();
            UpdateHostInstinctActivity();

            CurrentMoveWorldDirection = playerInput;
            var state = session != null ? session.State : null;
            var controlFrame = RatHostControlModel.Resolve(
                currentHostInstinctMoveDirection,
                playerInput,
                state != null ? state.Mutations.RatControlPower : 0.35f,
                state != null ? state.Config.RatHostInstinctResistance : 1f,
                forcedControlConflictDotThreshold,
                passiveInstinctSpeedMultiplier,
                forcedControlSpeedMultiplier,
                isHostInstinctPaused);
            CurrentResolvedMoveDirection = controlFrame.MoveDirection;
            ApplyForcedControlDemerit(controlFrame);
            if (!CanContinueRatHostMovement(state))
            {
                return;
            }

            var speedMultiplier = state != null ? state.Mutations.RatSpeedMultiplier : 1f;
            var delta = CurrentResolvedMoveDirection * (baseSpeed * speedMultiplier * controlFrame.SpeedMultiplier * Time.deltaTime);

            if (characterController != null)
            {
                if (!characterController.enabled || !characterController.gameObject.activeInHierarchy)
                {
                    return;
                }

                characterController.Move(delta);
            }
            else
            {
                transform.position += delta;
            }

            ClampToSewerBounds();

            if (CurrentResolvedMoveDirection.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(CurrentResolvedMoveDirection, Vector3.up);
            }
        }

        private bool CanContinueRatHostMovement(PrototypeSessionState state)
        {
            return isActiveAndEnabled
                && gameObject.activeInHierarchy
                && (state == null || state.Mode == PrototypeGameMode.RatHost);
        }

        private static Vector3 ReadMoveInput()
        {
            var mainCamera = Camera.main;
            return PrototypeKeyboardInput.ReadCameraRelativeMoveInput(mainCamera != null ? mainCamera.transform : null);
        }

        private void ClampToSewerBounds()
        {
            var position = transform.position;
            position.x = Mathf.Clamp(position.x, xBounds.x, xBounds.y);
            position.z = Mathf.Clamp(position.z, zBounds.x, zBounds.y);
            transform.position = position;
        }

        private void UpdateHostInstinctDirection()
        {
            var shouldTurn = randomizeInstinctDirection && Time.time >= nextInstinctTurnTime;
            currentHostInstinctMoveDirection = RatHostInstinctWanderModel.ResolveNextDirection(
                currentHostInstinctMoveDirection,
                hostInstinctMoveDirection,
                transform.position,
                xBounds,
                zBounds,
                shouldTurn,
                instinctTurnAngleDegrees,
                Random.value < 0.5f ? -1f : 1f);

            if (shouldTurn)
            {
                ScheduleNextInstinctTurn(Time.time);
            }
        }

        private void UpdateHostInstinctActivity()
        {
            if (!enableInstinctPauses || Time.time < nextInstinctActivityTime)
            {
                return;
            }

            isHostInstinctPaused = !isHostInstinctPaused;
            ScheduleNextInstinctActivity(Time.time);
        }

        private void ApplyForcedControlDemerit(RatHostControlFrame controlFrame)
        {
            if (!controlFrame.IsForcedControl || session == null || Time.time < nextForcedControlAlertTime)
            {
                return;
            }

            nextForcedControlAlertTime = Time.time + Mathf.Max(0f, forcedControlAlertCooldownSeconds);
            session.AddImmuneAlertAmount(forcedControlAlertAmount, forcedControlFeedbackLabel);
        }

        private void ScheduleNextInstinctTurn(float currentTime)
        {
            var minInterval = Mathf.Max(0.2f, Mathf.Min(instinctTurnIntervalRange.x, instinctTurnIntervalRange.y));
            var maxInterval = Mathf.Max(minInterval, Mathf.Max(instinctTurnIntervalRange.x, instinctTurnIntervalRange.y));
            nextInstinctTurnTime = currentTime + Random.Range(minInterval, maxInterval);
        }

        private void ScheduleNextInstinctActivity(float currentTime)
        {
            var range = isHostInstinctPaused ? instinctPauseDurationRange : instinctMoveDurationRange;
            var minInterval = Mathf.Max(0.05f, Mathf.Min(range.x, range.y));
            var maxInterval = Mathf.Max(minInterval, Mathf.Max(range.x, range.y));
            nextInstinctActivityTime = currentTime + Random.Range(minInterval, maxInterval);
        }

        private static Vector3 NormalizeOrFallback(Vector3 value, Vector3 fallback)
        {
            var flattened = Vector3.ProjectOnPlane(value, Vector3.up);
            if (flattened.sqrMagnitude >= 0.0001f)
            {
                return flattened.normalized;
            }

            var flattenedFallback = Vector3.ProjectOnPlane(fallback, Vector3.up);
            return flattenedFallback.sqrMagnitude >= 0.0001f ? flattenedFallback.normalized : Vector3.forward;
        }
    }
}
