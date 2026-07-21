using UnityEngine;

namespace LastHost.Prototype.Host
{
    public enum RatSpriteDirection
    {
        South = 0,
        SouthWest = 1,
        West = 2,
        NorthWest = 3,
        North = 4,
        NorthEast = 5,
        East = 6,
        SouthEast = 7
    }

    public static class RatDirectionQuantizer
    {
        private const float MinimumAxisSqrMagnitude = 0.0001f;

        public static RatSpriteDirection Quantize(
            Vector3 worldMoveDirection,
            Vector3 cameraRight,
            Vector3 cameraForward)
        {
            var move = Vector3.ProjectOnPlane(worldMoveDirection, Vector3.up);
            if (move.sqrMagnitude < MinimumAxisSqrMagnitude)
            {
                return RatSpriteDirection.South;
            }

            var screenRight = FlattenOrFallback(cameraRight, Vector3.right);
            var screenForward = Vector3.ProjectOnPlane(cameraForward, Vector3.up);
            screenForward -= Vector3.Dot(screenForward, screenRight) * screenRight;
            screenForward = FlattenOrFallback(screenForward, Vector3.forward);

            var screenX = Vector3.Dot(move, screenRight);
            var screenY = Vector3.Dot(move, screenForward);
            var sector = Mathf.RoundToInt(Mathf.Atan2(screenY, screenX) * Mathf.Rad2Deg / 45f);

            switch (sector)
            {
                case -3:
                    return RatSpriteDirection.SouthWest;
                case -2:
                    return RatSpriteDirection.South;
                case -1:
                    return RatSpriteDirection.SouthEast;
                case 0:
                    return RatSpriteDirection.East;
                case 1:
                    return RatSpriteDirection.NorthEast;
                case 2:
                    return RatSpriteDirection.North;
                case 3:
                    return RatSpriteDirection.NorthWest;
                default:
                    return RatSpriteDirection.West;
            }
        }

        private static Vector3 FlattenOrFallback(Vector3 value, Vector3 fallback)
        {
            var flattened = Vector3.ProjectOnPlane(value, Vector3.up);
            if (flattened.sqrMagnitude >= MinimumAxisSqrMagnitude)
            {
                return flattened.normalized;
            }

            return fallback;
        }
    }
}
