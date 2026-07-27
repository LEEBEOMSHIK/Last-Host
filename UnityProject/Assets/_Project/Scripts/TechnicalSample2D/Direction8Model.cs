using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D
{
    public enum Direction8
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

    public static class Direction8Model
    {
        public static Direction8 FromVector(Vector2 value, Direction8 fallback = Direction8.South)
        {
            if (value.sqrMagnitude <= 0.000001f)
            {
                return fallback;
            }

            var sectorFromEast = Mathf.RoundToInt(
                Mathf.Repeat(Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg, 360f) / 45f) % 8;

            switch (sectorFromEast)
            {
                case 0:
                    return Direction8.East;
                case 1:
                    return Direction8.NorthEast;
                case 2:
                    return Direction8.North;
                case 3:
                    return Direction8.NorthWest;
                case 4:
                    return Direction8.West;
                case 5:
                    return Direction8.SouthWest;
                case 6:
                    return Direction8.South;
                default:
                    return Direction8.SouthEast;
            }
        }
    }
}
