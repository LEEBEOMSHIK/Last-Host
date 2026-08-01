using System;
using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D
{
    /// <summary>
    /// Prevents a long, single-sprite character from being shown as two disconnected
    /// fragments when a foreground prop covers its middle. This is a visual-only
    /// contract; ground-footprint physics and deterministic Y sorting remain separate.
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
        private int _activeOccluderIndex = -1;
        private bool _rendererHiddenByResolver;
        private bool _rendererEnabledBeforeHide;
        private bool _visibilityStateInitialized;

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
            RestoreRendererVisibility();
            characterRenderer = renderer;
            characterSorter = sorter;
            frameContracts = suppliedFrameContracts ?? Array.Empty<FrameAlphaContract>();
            occluderContracts = suppliedOccluderContracts ?? Array.Empty<OccluderContract>();
            minimumFragmentWidth = Mathf.Max(0f, fragmentWidth);
            releaseHysteresis = Mathf.Max(0f, hysteresis);
            _isWholeCharacterOccluded = false;
            _visibilityTransitionCount = 0;
            _activeOccluderIndex = -1;
            // Configure is an explicit runtime ownership handoff. Preserve the
            // renderer's current state so an external disable remains authoritative.
            _visibilityStateInitialized = true;

            ResolveNow();
        }

        public bool ResolveNow()
        {
            InitializeDeserializedVisibilityState();

            if (characterRenderer == null || characterRenderer.sprite == null)
            {
                _activeOccluderIndex = -1;
                SetWholeCharacterOccluded(false);
                return false;
            }

            var frameIndex = FindFrameContract(characterRenderer.sprite);
            if (frameIndex < 0)
            {
                _activeOccluderIndex = -1;
                SetWholeCharacterOccluded(false);
                return false;
            }

            if (characterSorter != null)
            {
                characterSorter.ApplySorting();
            }

            var frame = frameContracts[frameIndex];
            var visibleWorldBounds = TransformLocalRect(
                characterRenderer.transform,
                frame.VisibleLocalBounds,
                characterRenderer.flipX);
            var coreWorldBounds = TransformLocalRect(
                characterRenderer.transform,
                frame.CoreLocalBounds,
                characterRenderer.flipX);
            var shouldHide = false;
            var nextActiveOccluderIndex = -1;

            for (var index = 0; index < occluderContracts.Length; index++)
            {
                var occluder = occluderContracts[index];
                if (occluder.Renderer == null || !occluder.Renderer.enabled)
                {
                    continue;
                }

                if (occluder.Sorter != null)
                {
                    occluder.Sorter.ApplySorting();
                }

                // A larger order is in front. Equal order is not treated as a stable
                // foreground relationship and therefore cannot trigger visual hiding.
                if (characterRenderer.sortingOrder >= occluder.Renderer.sortingOrder)
                {
                    continue;
                }

                var occluderWorldBounds = TransformLocalRect(
                    occluder.Renderer.transform,
                    occluder.VisibleLocalBounds,
                    occluder.Renderer.flipX);
                var activeHysteresis =
                    _isWholeCharacterOccluded && index == _activeOccluderIndex
                        ? releaseHysteresis
                        : 0f;
                var requiresOcclusion = activeHysteresis > 0f
                    ? WouldRemainOccludedDuringRelease(
                        visibleWorldBounds,
                        coreWorldBounds,
                        occluderWorldBounds,
                        minimumFragmentWidth,
                        activeHysteresis)
                    : WouldSplitIntoTwoVisibleFragments(
                        visibleWorldBounds,
                        coreWorldBounds,
                        occluderWorldBounds,
                        minimumFragmentWidth);
                if (requiresOcclusion)
                {
                    shouldHide = true;
                    nextActiveOccluderIndex = index;
                    break;
                }
            }

            _activeOccluderIndex = nextActiveOccluderIndex;
            SetWholeCharacterOccluded(shouldHide);
            return shouldHide;
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
            RestoreRendererVisibility();
            _isWholeCharacterOccluded = false;
            _activeOccluderIndex = -1;
        }

        private int FindFrameContract(Sprite sprite)
        {
            for (var index = 0; index < frameContracts.Length; index++)
            {
                if (frameContracts[index].Sprite == sprite)
                {
                    return index;
                }
            }

            return -1;
        }

        private void SetWholeCharacterOccluded(bool shouldHide)
        {
            if (_isWholeCharacterOccluded != shouldHide)
            {
                _isWholeCharacterOccluded = shouldHide;
                _visibilityTransitionCount++;
            }

            if (characterRenderer == null)
            {
                return;
            }

            if (shouldHide)
            {
                if (!_rendererHiddenByResolver)
                {
                    _rendererEnabledBeforeHide = characterRenderer.enabled;
                    _rendererHiddenByResolver = true;
                }

                characterRenderer.enabled = false;
            }
            else
            {
                RestoreRendererVisibility();
            }
        }

        private void RestoreRendererVisibility()
        {
            if (!_rendererHiddenByResolver)
            {
                return;
            }

            if (characterRenderer != null)
            {
                characterRenderer.enabled = _rendererEnabledBeforeHide;
            }

            _rendererHiddenByResolver = false;
        }

        private void InitializeDeserializedVisibilityState()
        {
            if (_visibilityStateInitialized)
            {
                return;
            }

            _visibilityStateInitialized = true;

            // Scene builders can evaluate this component before saving and thereby
            // serialize SpriteRenderer.enabled=false. The resolver's non-serialized
            // ownership fields do not survive that save/load boundary. A deserialized
            // resolver owns its dedicated character renderer, so normalize that stale
            // persisted hide before the first runtime decision. Explicit Configure
            // calls skip this branch and preserve an external disabled state.
            if (characterRenderer != null && !characterRenderer.enabled)
            {
                characterRenderer.enabled = true;
            }
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
