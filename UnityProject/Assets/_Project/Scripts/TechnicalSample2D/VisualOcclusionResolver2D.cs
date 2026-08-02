using System;
using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D
{
    /// <summary>
    /// Legacy compatibility component for scenes that still serialize the former
    /// whole-character occlusion contract. Natural occlusion is owned by foreground
    /// geometry and sorting; this component never changes character visibility.
    /// </summary>
    public sealed class VisualOcclusionResolver2D : MonoBehaviour
    {
        [Serializable]
        public struct FrameAlphaContract
        {
            public FrameAlphaContract(Sprite sprite, Rect visibleLocalBounds, Rect coreLocalBounds)
            {
                Sprite = sprite;
                VisibleLocalBounds = visibleLocalBounds;
                CoreLocalBounds = coreLocalBounds;
            }

            public Sprite Sprite;
            public Rect VisibleLocalBounds;
            public Rect CoreLocalBounds;
        }

        [Serializable]
        public struct OccluderContract
        {
            public OccluderContract(
                SpriteRenderer renderer,
                YSortSprite2D sorter,
                Rect visibleLocalBounds)
            {
                Renderer = renderer;
                Sorter = sorter;
                VisibleLocalBounds = visibleLocalBounds;
            }

            public SpriteRenderer Renderer;
            public YSortSprite2D Sorter;
            public Rect VisibleLocalBounds;
        }

        [SerializeField] private SpriteRenderer characterRenderer;
        [SerializeField] private YSortSprite2D characterSorter;
        [SerializeField] private FrameAlphaContract[] frameContracts = Array.Empty<FrameAlphaContract>();
        [SerializeField] private OccluderContract[] occluderContracts = Array.Empty<OccluderContract>();
        [SerializeField, Min(0f)] private float minimumFragmentWidth = 4f / 128f;
        [SerializeField, Min(0f)] private float releaseHysteresis = 2f / 128f;

        private bool _isWholeCharacterOccluded;
        private int _visibilityTransitionCount;

        public bool IsWholeCharacterOccluded => _isWholeCharacterOccluded;
        public int VisibilityTransitionCount => _visibilityTransitionCount;
        public float MinimumFragmentWidth => minimumFragmentWidth;
        public float ReleaseHysteresis => releaseHysteresis;

        public void Configure(
            SpriteRenderer renderer,
            YSortSprite2D sorter,
            FrameAlphaContract[] suppliedFrameContracts,
            OccluderContract[] suppliedOccluderContracts,
            float fragmentWidth,
            float hysteresis)
        {
            characterRenderer = renderer;
            characterSorter = sorter;
            frameContracts = suppliedFrameContracts ?? Array.Empty<FrameAlphaContract>();
            occluderContracts = suppliedOccluderContracts ?? Array.Empty<OccluderContract>();
            minimumFragmentWidth = Mathf.Max(0f, fragmentWidth);
            releaseHysteresis = Mathf.Max(0f, hysteresis);
            _isWholeCharacterOccluded = false;
            _visibilityTransitionCount = 0;
            // Renderer enabled/color/active state remains externally owned.
            ResolveNow();
        }

        public bool ResolveNow()
        {
            if (characterSorter != null)
            {
                characterSorter.ApplySorting();
            }

            // Keep legacy occluder sorting refreshes while the scene owner removes
            // this wiring. No branch may disable, fade, deactivate, move, or lock
            // the character to conceal a geometry/sorting defect.
            for (var index = 0; index < occluderContracts.Length; index++)
            {
                var occluder = occluderContracts[index];
                if (occluder.Sorter != null)
                {
                    occluder.Sorter.ApplySorting();
                }
            }

            _isWholeCharacterOccluded = false;
            _visibilityTransitionCount = 0;
            return false;
        }

        public static bool WouldSplitIntoTwoVisibleFragments(
            Rect characterVisibleBounds,
            Rect characterCoreBounds,
            Rect occluderVisibleBounds,
            float requiredFragmentWidth)
        {
            if (!characterVisibleBounds.Overlaps(occluderVisibleBounds, true) ||
                !characterCoreBounds.Overlaps(occluderVisibleBounds, true))
            {
                return false;
            }

            var minimumWidth = Mathf.Max(0f, requiredFragmentWidth);
            var leftVisibleWidth = occluderVisibleBounds.xMin - characterVisibleBounds.xMin;
            var rightVisibleWidth = characterVisibleBounds.xMax - occluderVisibleBounds.xMax;
            var hasLeftFragment = leftVisibleWidth >= minimumWidth;
            var hasRightFragment = rightVisibleWidth >= minimumWidth;
            var hasTwoFragments = hasLeftFragment && hasRightFragment;

            // A foreground object can cover the body and one complete side while leaving
            // only the opposite tail tip. That one-sided remnant is just as detached as
            // the two-sided case and must not remain visible.
            var hasLeftTailOnlyFragment =
                hasLeftFragment &&
                occluderVisibleBounds.xMin - characterCoreBounds.xMin < minimumWidth;
            var hasRightTailOnlyFragment =
                hasRightFragment &&
                characterCoreBounds.xMax - occluderVisibleBounds.xMax < minimumWidth;

            return hasTwoFragments || hasLeftTailOnlyFragment || hasRightTailOnlyFragment;
        }

        public static bool WouldRemainOccludedDuringRelease(
            Rect characterVisibleBounds,
            Rect characterCoreBounds,
            Rect occluderVisibleBounds,
            float requiredFragmentWidth,
            float hysteresis)
        {
            var activeHysteresis = Mathf.Max(0f, hysteresis);
            var expandedCoreBounds = ExpandBounds(
                characterCoreBounds,
                activeHysteresis);
            var activeFragmentWidth = Mathf.Max(
                0f,
                requiredFragmentWidth - activeHysteresis);
            return WouldSplitIntoTwoVisibleFragments(
                characterVisibleBounds,
                expandedCoreBounds,
                occluderVisibleBounds,
                activeFragmentWidth);
        }

        private void LateUpdate()
        {
            ResolveNow();
        }

        private void OnDisable()
        {
            _isWholeCharacterOccluded = false;
            _visibilityTransitionCount = 0;
        }

        private static Rect TransformLocalRect(Transform target, Rect localBounds, bool flipX)
        {
            var xMin = flipX ? -localBounds.xMax : localBounds.xMin;
            var xMax = flipX ? -localBounds.xMin : localBounds.xMax;
            var bottomLeft = target.TransformPoint(new Vector3(xMin, localBounds.yMin, 0f));
            var topLeft = target.TransformPoint(new Vector3(xMin, localBounds.yMax, 0f));
            var bottomRight = target.TransformPoint(new Vector3(xMax, localBounds.yMin, 0f));
            var topRight = target.TransformPoint(new Vector3(xMax, localBounds.yMax, 0f));

            var worldMinX = Mathf.Min(bottomLeft.x, topLeft.x, bottomRight.x, topRight.x);
            var worldMaxX = Mathf.Max(bottomLeft.x, topLeft.x, bottomRight.x, topRight.x);
            var worldMinY = Mathf.Min(bottomLeft.y, topLeft.y, bottomRight.y, topRight.y);
            var worldMaxY = Mathf.Max(bottomLeft.y, topLeft.y, bottomRight.y, topRight.y);

            return Rect.MinMaxRect(worldMinX, worldMinY, worldMaxX, worldMaxY);
        }

        private static Rect ExpandBounds(Rect source, float amount)
        {
            return Rect.MinMaxRect(
                source.xMin - amount,
                source.yMin - amount,
                source.xMax + amount,
                source.yMax + amount);
        }
    }
}
