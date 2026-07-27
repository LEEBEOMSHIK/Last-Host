using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D
{
    public static class YSortOrder2D
    {
        public static int Calculate(
            float footWorldY,
            int baseOrder = 0,
            int explicitTieBreak = 0,
            int scale = TechnicalSample2DConstants.YSortScale)
        {
            return baseOrder - Mathf.RoundToInt(footWorldY * scale) + explicitTieBreak;
        }
    }
}
