using UnityEngine;

namespace LastHost.Prototype.Host
{
    /// <summary>
    /// Snaps a billboard visual in world space without changing its grounded height.
    /// The host root owns movement and collision; this helper is deliberately visual-only.
    /// </summary>
    public static class RatVisualPixelSnapper
    {
        public static bool IsValidPixelsPerUnit(float pixelsPerUnit)
        {
            return !float.IsNaN(pixelsPerUnit)
                && !float.IsInfinity(pixelsPerUnit)
                && pixelsPerUnit > 0f;
        }

        public static Vector3 SnapHorizontal(Vector3 worldPosition, float pixelsPerUnit)
        {
            if (!IsValidPixelsPerUnit(pixelsPerUnit))
            {
                return worldPosition;
            }

            worldPosition.x = Mathf.Round(worldPosition.x * pixelsPerUnit) / pixelsPerUnit;
            worldPosition.z = Mathf.Round(worldPosition.z * pixelsPerUnit) / pixelsPerUnit;
            return worldPosition;
        }
    }
}
