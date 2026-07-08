using UnityEngine;

namespace LastHost.Prototype.Host
{
    public readonly struct RatHostControlFrame
    {
        public RatHostControlFrame(Vector3 moveDirection, float speedMultiplier, bool isForcedControl, float controlRatio)
        {
            MoveDirection = moveDirection;
            SpeedMultiplier = speedMultiplier;
            IsForcedControl = isForcedControl;
            ControlRatio = controlRatio;
        }

        public Vector3 MoveDirection { get; }
        public float SpeedMultiplier { get; }
        public bool IsForcedControl { get; }
        public float ControlRatio { get; }
    }

    public static class RatHostControlModel
    {
        public static RatHostControlFrame Resolve(
            Vector3 hostInstinctDirection,
            Vector3 playerInputDirection,
            float virusControlPower,
            float hostInstinctResistance,
            float conflictDotThreshold,
            float passiveInstinctSpeedMultiplier,
            float forcedControlSpeedMultiplier,
            bool hostInstinctPaused = false)
        {
            var instinct = FlattenNormalizedOrFallback(hostInstinctDirection, Vector3.forward);
            var input = FlattenNormalizedOrFallback(playerInputDirection, Vector3.zero);
            var controlRatio = hostInstinctResistance <= 0f
                ? 1f
                : Mathf.Clamp01(Mathf.Max(0f, virusControlPower) / hostInstinctResistance);

            if (input == Vector3.zero)
            {
                if (hostInstinctPaused)
                {
                    return new RatHostControlFrame(
                        Vector3.zero,
                        0f,
                        isForcedControl: false,
                        controlRatio);
                }

                return new RatHostControlFrame(
                    instinct,
                    Mathf.Clamp01(passiveInstinctSpeedMultiplier),
                    isForcedControl: false,
                    controlRatio);
            }

            var isForcedControl = controlRatio < 1f && Vector3.Dot(instinct, input) <= conflictDotThreshold;
            var blended = controlRatio >= 1f
                ? input
                : FlattenNormalizedOrFallback((instinct * (1f - controlRatio)) + (input * controlRatio), input);
            var speedMultiplier = isForcedControl ? Mathf.Clamp01(forcedControlSpeedMultiplier) : 1f;

            return new RatHostControlFrame(blended, speedMultiplier, isForcedControl, controlRatio);
        }

        private static Vector3 FlattenNormalizedOrFallback(Vector3 value, Vector3 fallback)
        {
            var flattened = Vector3.ProjectOnPlane(value, Vector3.up);
            if (flattened.sqrMagnitude >= 0.0001f)
            {
                return flattened.normalized;
            }

            var flattenedFallback = Vector3.ProjectOnPlane(fallback, Vector3.up);
            return flattenedFallback.sqrMagnitude >= 0.0001f ? flattenedFallback.normalized : Vector3.zero;
        }
    }

    public static class RatHostInstinctWanderModel
    {
        private const float BoundaryPadding = 0.2f;

        public static Vector3 ResolveNextDirection(
            Vector3 currentDirection,
            Vector3 fallbackDirection,
            Vector3 position,
            Vector2 xBounds,
            Vector2 zBounds,
            bool turnRequested,
            float turnAngleDegrees,
            float turnSign)
        {
            var direction = FlattenNormalizedOrFallback(currentDirection, fallbackDirection);
            if (direction == Vector3.zero)
            {
                direction = Vector3.forward;
            }

            if (TryResolveBoundaryDirection(direction, position, xBounds, zBounds, turnSign, out var boundaryDirection))
            {
                return boundaryDirection;
            }

            if (!turnRequested)
            {
                return direction;
            }

            var signedAngle = Mathf.Clamp(turnAngleDegrees, 0f, 160f) * SignOrPositive(turnSign);
            var turned = Quaternion.Euler(0f, signedAngle, 0f) * direction;
            return FlattenNormalizedOrFallback(turned, direction);
        }

        public static Vector3 CreateInitialDirection(Vector3 fallbackDirection, float turnSign, float turnAngleDegrees)
        {
            var fallback = FlattenNormalizedOrFallback(fallbackDirection, Vector3.forward);
            if (fallback == Vector3.zero)
            {
                fallback = Vector3.forward;
            }

            var signedAngle = Mathf.Clamp(turnAngleDegrees, 15f, 160f) * SignOrPositive(turnSign);
            return FlattenNormalizedOrFallback(Quaternion.Euler(0f, signedAngle, 0f) * fallback, fallback);
        }

        private static bool TryResolveBoundaryDirection(
            Vector3 direction,
            Vector3 position,
            Vector2 xBounds,
            Vector2 zBounds,
            float turnSign,
            out Vector3 boundaryDirection)
        {
            var sign = SignOrPositive(turnSign);
            var wantsPastMinX = position.x <= xBounds.x + BoundaryPadding && direction.x < 0f;
            var wantsPastMaxX = position.x >= xBounds.y - BoundaryPadding && direction.x > 0f;
            if (wantsPastMinX || wantsPastMaxX)
            {
                var zSign = Mathf.Abs(direction.z) > 0.1f ? Mathf.Sign(direction.z) : sign;
                boundaryDirection = Vector3.forward * zSign;
                return true;
            }

            var wantsPastMinZ = position.z <= zBounds.x + BoundaryPadding && direction.z < 0f;
            var wantsPastMaxZ = position.z >= zBounds.y - BoundaryPadding && direction.z > 0f;
            if (wantsPastMinZ || wantsPastMaxZ)
            {
                var xSign = Mathf.Abs(direction.x) > 0.1f ? Mathf.Sign(direction.x) : sign;
                boundaryDirection = Vector3.right * xSign;
                return true;
            }

            boundaryDirection = Vector3.zero;
            return false;
        }

        private static float SignOrPositive(float value)
        {
            return value < 0f ? -1f : 1f;
        }

        private static Vector3 FlattenNormalizedOrFallback(Vector3 value, Vector3 fallback)
        {
            var flattened = Vector3.ProjectOnPlane(value, Vector3.up);
            if (flattened.sqrMagnitude >= 0.0001f)
            {
                return flattened.normalized;
            }

            var flattenedFallback = Vector3.ProjectOnPlane(fallback, Vector3.up);
            return flattenedFallback.sqrMagnitude >= 0.0001f ? flattenedFallback.normalized : Vector3.zero;
        }
    }
}
