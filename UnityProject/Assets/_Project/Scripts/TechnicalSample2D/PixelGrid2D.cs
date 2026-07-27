using System;
using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D
{
    public static class PixelGrid2D
    {
        public static float Snap(float value, int pixelsPerUnit = TechnicalSample2DConstants.PixelsPerUnit)
        {
            ValidatePixelsPerUnit(pixelsPerUnit);
            return Mathf.Round(value * pixelsPerUnit) / pixelsPerUnit;
        }

        public static Vector2 Snap(Vector2 value, int pixelsPerUnit = TechnicalSample2DConstants.PixelsPerUnit)
        {
            return new Vector2(Snap(value.x, pixelsPerUnit), Snap(value.y, pixelsPerUnit));
        }

        public static Vector3 SnapXY(Vector3 value, int pixelsPerUnit = TechnicalSample2DConstants.PixelsPerUnit)
        {
            var snapped = Snap((Vector2)value, pixelsPerUnit);
            return new Vector3(snapped.x, snapped.y, value.z);
        }

        public static void ValidatePixelsPerUnit(int pixelsPerUnit)
        {
            if (pixelsPerUnit <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(pixelsPerUnit),
                    pixelsPerUnit,
                    "Pixels per unit must be greater than zero.");
            }
        }
    }
}
