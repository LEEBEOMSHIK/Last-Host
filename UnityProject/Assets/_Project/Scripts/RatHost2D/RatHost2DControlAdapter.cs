using LastHost.Prototype.Host;
using LastHost.Prototype.TechnicalSample2D;
using UnityEngine;

namespace LastHost.Prototype.RatHost2D
{
    public readonly struct RatHost2DControlFrame
    {
        public RatHost2DControlFrame(
            Vector2 moveDirection,
            float speedMultiplier,
            bool isForcedControl,
            float controlRatio)
        {
            MoveDirection = moveDirection;
            SpeedMultiplier = speedMultiplier;
            IsForcedControl = isForcedControl;
            ControlRatio = controlRatio;
        }

        public Vector2 MoveDirection { get; }
        public float SpeedMultiplier { get; }
        public bool IsForcedControl { get; }
        public float ControlRatio { get; }
    }

    /// <summary>
    /// Keeps the approved XZ host-instinct rules dimension independent while the
    /// 2D prototype owns an XY Rigidbody2D root.
    /// </summary>
    public static class RatHost2DControlAdapter
    {
        public static Vector3 XYToXZ(Vector2 value)
        {
            return new Vector3(value.x, 0f, value.y);
        }

        public static Vector2 XZToXY(Vector3 value)
        {
            return new Vector2(value.x, value.z);
        }

        public static RatHost2DControlFrame Resolve(
            Vector2 hostInstinctDirection,
            Vector2 playerInputDirection,
            float virusControlPower,
            float hostInstinctResistance,
            float conflictDotThreshold,
            float passiveInstinctSpeedMultiplier,
            float forcedControlSpeedMultiplier,
            bool hostInstinctPaused = false)
        {
            var frame = RatHostControlModel.Resolve(
                XYToXZ(hostInstinctDirection),
                XYToXZ(Movement2DModel.NormalizeInput(playerInputDirection)),
                virusControlPower,
                hostInstinctResistance,
                conflictDotThreshold,
                passiveInstinctSpeedMultiplier,
                forcedControlSpeedMultiplier,
                hostInstinctPaused);

            return new RatHost2DControlFrame(
                Movement2DModel.NormalizeInput(XZToXY(frame.MoveDirection)),
                frame.SpeedMultiplier,
                frame.IsForcedControl,
                frame.ControlRatio);
        }

        public static Vector2 ResolveNextInstinctDirection(
            Vector2 currentDirection,
            Vector2 fallbackDirection,
            Vector2 position,
            Vector2 xBounds,
            Vector2 yBounds,
            bool turnRequested,
            float turnAngleDegrees,
            float turnSign)
        {
            var resolved = RatHostInstinctWanderModel.ResolveNextDirection(
                XYToXZ(currentDirection),
                XYToXZ(fallbackDirection),
                XYToXZ(position),
                xBounds,
                yBounds,
                turnRequested,
                turnAngleDegrees,
                turnSign);

            return Movement2DModel.NormalizeInput(XZToXY(resolved));
        }

        public static Vector2 CreateInitialInstinctDirection(
            Vector2 fallbackDirection,
            float turnSign,
            float turnAngleDegrees)
        {
            var resolved = RatHostInstinctWanderModel.CreateInitialDirection(
                XYToXZ(fallbackDirection),
                turnSign,
                turnAngleDegrees);

            return Movement2DModel.NormalizeInput(XZToXY(resolved));
        }
    }
}
