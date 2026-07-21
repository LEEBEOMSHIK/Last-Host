namespace LastHost.Prototype.Host
{
    public readonly struct RatGroundSurfaceCandidate
    {
        public RatGroundSurfaceCandidate(
            float height,
            float normalY,
            bool isSelf,
            bool isTrigger,
            bool hasVisibleSurface)
        {
            Height = height;
            NormalY = normalY;
            IsSelf = isSelf;
            IsTrigger = isTrigger;
            HasVisibleSurface = hasVisibleSurface;
        }

        public float Height { get; }
        public float NormalY { get; }
        public bool IsSelf { get; }
        public bool IsTrigger { get; }
        public bool HasVisibleSurface { get; }
    }

    public static class RatVisualGroundingResolver
    {
        public static bool TrySelectHighestSurface(
            RatGroundSurfaceCandidate[] candidates,
            int candidateCount,
            float hostHeight,
            float maxSurfaceRise,
            float maxSurfaceDrop,
            float minSurfaceNormalY,
            out float surfaceHeight)
        {
            surfaceHeight = 0f;
            var foundSurface = false;
            var minHeight = hostHeight - maxSurfaceDrop;
            var maxHeight = hostHeight + maxSurfaceRise;

            for (var i = 0; i < candidateCount; i++)
            {
                var candidate = candidates[i];
                if (candidate.IsSelf
                    || candidate.IsTrigger
                    || !candidate.HasVisibleSurface
                    || candidate.NormalY < minSurfaceNormalY
                    || candidate.Height < minHeight
                    || candidate.Height > maxHeight)
                {
                    continue;
                }

                if (!foundSurface || candidate.Height > surfaceHeight)
                {
                    surfaceHeight = candidate.Height;
                    foundSurface = true;
                }
            }

            return foundSurface;
        }
    }
}
