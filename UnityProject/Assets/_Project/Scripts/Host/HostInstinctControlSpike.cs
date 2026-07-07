using UnityEngine;

namespace LastHost.Prototype.Host
{
    public sealed class HostInstinctControlSpike
    {
        private readonly float requiredHoldSeconds;
        private readonly float cooldownSeconds;
        private readonly float directionDotThreshold;
        private float heldSeconds;
        private float nextAllowedTimeSeconds;

        public HostInstinctControlSpike(float requiredHoldSeconds, float cooldownSeconds, float directionDotThreshold)
        {
            this.requiredHoldSeconds = Mathf.Max(0.01f, requiredHoldSeconds);
            this.cooldownSeconds = Mathf.Max(0f, cooldownSeconds);
            this.directionDotThreshold = Mathf.Clamp(directionDotThreshold, -1f, 1f);
        }

        public bool Tick(Vector3 hostPosition, Vector3 dangerPosition, Vector3 inputDirection, float deltaTime, float currentTimeSeconds)
        {
            if (currentTimeSeconds < nextAllowedTimeSeconds)
            {
                return false;
            }

            if (!IsInputTowardDanger(hostPosition, dangerPosition, inputDirection))
            {
                heldSeconds = 0f;
                return false;
            }

            heldSeconds += Mathf.Max(0f, deltaTime);
            if (heldSeconds < requiredHoldSeconds)
            {
                return false;
            }

            heldSeconds = 0f;
            nextAllowedTimeSeconds = Mathf.Max(0f, currentTimeSeconds) + cooldownSeconds;
            return true;
        }

        public void Reset()
        {
            heldSeconds = 0f;
        }

        private bool IsInputTowardDanger(Vector3 hostPosition, Vector3 dangerPosition, Vector3 inputDirection)
        {
            var flatInput = Vector3.ProjectOnPlane(inputDirection, Vector3.up);
            if (flatInput.sqrMagnitude < 0.0001f)
            {
                return false;
            }

            var dangerDirection = Vector3.ProjectOnPlane(dangerPosition - hostPosition, Vector3.up);
            if (dangerDirection.sqrMagnitude < 0.0001f)
            {
                return false;
            }

            return Vector3.Dot(flatInput.normalized, dangerDirection.normalized) >= directionDotThreshold;
        }
    }
}
