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

        private CharacterController characterController;
        private Vector3 currentHostInstinctMoveDirection;
        private float nextForcedControlAlertTime;

        public Vector3 CurrentMoveWorldDirection { get; private set; }
        public Vector3 CurrentResolvedMoveDirection { get; private set; }

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            currentHostInstinctMoveDirection = NormalizeOrFallback(hostInstinctMoveDirection, Vector3.forward);
        }

        private void Update()
        {
            var playerInput = ReadMoveInput();
            if (playerInput.sqrMagnitude > 1f)
            {
                playerInput.Normalize();
            }

            UpdateHostInstinctDirection();

            CurrentMoveWorldDirection = playerInput;
            var state = session != null ? session.State : null;
            var controlFrame = RatHostControlModel.Resolve(
                currentHostInstinctMoveDirection,
                playerInput,
                state != null ? state.Mutations.RatControlPower : 0.35f,
                state != null ? state.Config.RatHostInstinctResistance : 1f,
                forcedControlConflictDotThreshold,
                passiveInstinctSpeedMultiplier,
                forcedControlSpeedMultiplier);
            CurrentResolvedMoveDirection = controlFrame.MoveDirection;
            ApplyForcedControlDemerit(controlFrame);

            var speedMultiplier = state != null ? state.Mutations.RatSpeedMultiplier : 1f;
            var delta = CurrentResolvedMoveDirection * (baseSpeed * speedMultiplier * controlFrame.SpeedMultiplier * Time.deltaTime);

            if (characterController != null)
            {
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
            currentHostInstinctMoveDirection = NormalizeOrFallback(currentHostInstinctMoveDirection, hostInstinctMoveDirection);
            var position = transform.position;
            var nextDirection = currentHostInstinctMoveDirection;

            if ((position.x <= xBounds.x + 0.2f && nextDirection.x < 0f)
                || (position.x >= xBounds.y - 0.2f && nextDirection.x > 0f))
            {
                nextDirection.x = -nextDirection.x;
            }

            if ((position.z <= zBounds.x + 0.2f && nextDirection.z < 0f)
                || (position.z >= zBounds.y - 0.2f && nextDirection.z > 0f))
            {
                nextDirection.z = -nextDirection.z;
            }

            currentHostInstinctMoveDirection = NormalizeOrFallback(nextDirection, Vector3.forward);
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
