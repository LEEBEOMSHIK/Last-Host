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
            float forcedControlSpeedMultiplier)
        {
            var instinct = FlattenNormalizedOrFallback(hostInstinctDirection, Vector3.forward);
            var input = FlattenNormalizedOrFallback(playerInputDirection, Vector3.zero);
            var controlRatio = hostInstinctResistance <= 0f
                ? 1f
                : Mathf.Clamp01(Mathf.Max(0f, virusControlPower) / hostInstinctResistance);

            if (input == Vector3.zero)
            {
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
}
