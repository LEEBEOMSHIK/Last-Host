using UnityEngine;

namespace LastHost.Prototype.Host
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class RatDirectionalSpriteView : MonoBehaviour
    {
        private const int WalkFrameCount = 8;

        public RatHostController ratHostController;
        public SpriteRenderer spriteRenderer;
        public Camera viewCamera;
        public Sprite south;
        public Sprite southWest;
        public Sprite west;
        public Sprite northWest;
        public Sprite north;
        public Sprite northEast;
        public Sprite east;
        public Sprite southEast;
        [Tooltip("Trial-only, in source direction order f01 through f08. All eight arrays must contain eight sprites before walk playback is enabled.")]
        public Sprite[] southWalkFrames;
        public Sprite[] southWestWalkFrames;
        public Sprite[] westWalkFrames;
        public Sprite[] northWestWalkFrames;
        public Sprite[] northWalkFrames;
        public Sprite[] northEastWalkFrames;
        public Sprite[] eastWalkFrames;
        public Sprite[] southEastWalkFrames;
        [Min(0f)]
        public float walkFramesPerSecond = 8f;
        public RatSpriteDirection initialDirection = RatSpriteDirection.South;
        public float movementThreshold = 0.001f;
        public float groundProbeHeight = 1f;
        public float groundProbeDistance = 1.6f;
        public float maxSurfaceRise = 0.2f;
        public float maxSurfaceDrop = 0.5f;
        public float minSurfaceNormalY = 0.5f;
        // Direction-specific distances from the SpriteRenderer pivot to the lowest visible rat-foot alpha boundary.
        public float southVisibleFootBottomOffset = 0.1875f;
        public float southWestVisibleFootBottomOffset = 0.15625f;
        public float westVisibleFootBottomOffset = 0.09375f;
        public float northWestVisibleFootBottomOffset = 0.28125f;
        public float northVisibleFootBottomOffset = 0.375f;
        public float northEastVisibleFootBottomOffset = 0.40625f;
        public float eastVisibleFootBottomOffset = 0.15625f;
        public float southEastVisibleFootBottomOffset = 0.15625f;
        // Clearance between the visible rat-foot alpha boundary and the selected ground surface.
        public float groundClearance = 0.005f;
        [Tooltip("Snaps this RatVisual's horizontal world position only. The RatHost root, movement, and grounded vertical clearance stay unchanged.")]
        public bool enablePixelSnap;
        [Min(0f)]
        public float pixelSnapPixelsPerUnit = 64f;

        public RatSpriteDirection CurrentDirection { get; private set; } = RatSpriteDirection.South;
        public bool HasGroundSurface { get; private set; }
        public float CurrentGroundSurfaceY { get; private set; }
        public float CurrentVisibleFootBottomOffset => GetVisibleFootBottomOffset(CurrentDirection);
        public float CurrentVisibleFootY => transform.position.y - CurrentVisibleFootBottomOffset;
        public float CurrentGroundClearance => HasGroundSurface ? CurrentVisibleFootY - CurrentGroundSurfaceY : 0f;
        public bool IsUsingWalkSprite { get; private set; }
        public int CurrentWalkFrameIndex { get; private set; }
        public bool HasCompleteWalkCycles =>
            HasCompleteWalkFrames(southWalkFrames)
            && HasCompleteWalkFrames(southWestWalkFrames)
            && HasCompleteWalkFrames(westWalkFrames)
            && HasCompleteWalkFrames(northWestWalkFrames)
            && HasCompleteWalkFrames(northWalkFrames)
            && HasCompleteWalkFrames(northEastWalkFrames)
            && HasCompleteWalkFrames(eastWalkFrames)
            && HasCompleteWalkFrames(southEastWalkFrames);

        private readonly RaycastHit[] groundHits = new RaycastHit[16];
        private readonly RatGroundSurfaceCandidate[] groundCandidates = new RatGroundSurfaceCandidate[16];

        private void Awake()
        {
            ResolveReferences();
            CurrentDirection = initialDirection;
            ApplyCurrentSprite();
        }

        private void OnEnable()
        {
            ResolveReferences();
            ApplyCurrentSprite();
        }

        private void LateUpdate()
        {
            ResolveReferences();

            var cameraTransform = viewCamera != null ? viewCamera.transform : null;
            if (ratHostController != null && cameraTransform != null)
            {
                RefreshDirection(ratHostController.CurrentResolvedMoveDirection, cameraTransform);
            }

            RefreshWalkAnimation(
                ratHostController != null ? ratHostController.CurrentResolvedMoveDirection : Vector3.zero,
                Time.time);
            RefreshGrounding();
            RefreshPixelSnap();
            FaceCamera(cameraTransform);
        }

        public void RefreshDirection(Vector3 resolvedMoveDirection, Transform cameraTransform)
        {
            if (cameraTransform == null || resolvedMoveDirection.sqrMagnitude <= movementThreshold * movementThreshold)
            {
                return;
            }

            var cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up);
            if (cameraForward.sqrMagnitude < 0.0001f)
            {
                cameraForward = Vector3.ProjectOnPlane(cameraTransform.up, Vector3.up);
            }

            // The pre-rendered rat sheets use Blender's camera-horizontal basis, which is
            // mirrored from Unity's camera-right basis. Keep this conversion in the visual
            // layer so movement, collision, and the controller's world direction stay intact.
            var renderedSpriteCameraRight = -cameraTransform.right;
            var nextDirection = RatDirectionQuantizer.Quantize(
                resolvedMoveDirection,
                renderedSpriteCameraRight,
                cameraForward);
            if (nextDirection == CurrentDirection)
            {
                return;
            }

            CurrentDirection = nextDirection;
            ApplyCurrentSprite();
        }

        /// <summary>
        /// Updates this visual child only. Direction quantization, movement, collision, and root transforms stay owned by RatHostController.
        /// The elapsed time input is shared by every direction so a direction change keeps the current gait phase.
        /// </summary>
        public void RefreshWalkAnimation(Vector3 resolvedMoveDirection, float elapsedSeconds)
        {
            ResolveReferences();
            if (spriteRenderer == null
                || resolvedMoveDirection.sqrMagnitude <= movementThreshold * movementThreshold
                || !HasCompleteWalkCycles)
            {
                IsUsingWalkSprite = false;
                CurrentWalkFrameIndex = 0;
                ApplyCurrentSprite();
                return;
            }

            var frameRate = Mathf.Max(0f, walkFramesPerSecond);
            if (frameRate <= 0f)
            {
                IsUsingWalkSprite = false;
                CurrentWalkFrameIndex = 0;
                ApplyCurrentSprite();
                return;
            }

            CurrentWalkFrameIndex = Mathf.FloorToInt(Mathf.Max(0f, elapsedSeconds) * frameRate) % WalkFrameCount;
            var frames = GetWalkFrames(CurrentDirection);
            spriteRenderer.sprite = frames[CurrentWalkFrameIndex];
            IsUsingWalkSprite = true;
        }

        public void RefreshGrounding()
        {
            if (ratHostController == null)
            {
                HasGroundSurface = false;
                return;
            }

            var hostRoot = ratHostController.transform;
            var hostPosition = hostRoot.position;
            var origin = hostPosition + Vector3.up * groundProbeHeight;
            var hitCount = Physics.RaycastNonAlloc(
                origin,
                Vector3.down,
                groundHits,
                groundProbeDistance,
                Physics.AllLayers,
                QueryTriggerInteraction.Collide);
            var candidateCount = 0;

            for (var i = 0; i < hitCount && candidateCount < groundCandidates.Length; i++)
            {
                var hit = groundHits[i];
                var hitCollider = hit.collider;
                if (hitCollider == null)
                {
                    continue;
                }

                var hitTransform = hitCollider.transform;
                var isSelf = hitTransform == hostRoot || hitTransform.IsChildOf(hostRoot);
                var surfaceRenderer = hitCollider.GetComponent<Renderer>();
                var hasVisibleSurface = surfaceRenderer != null
                    && surfaceRenderer.enabled
                    && surfaceRenderer.gameObject.activeInHierarchy;
                groundCandidates[candidateCount++] = new RatGroundSurfaceCandidate(
                    hit.point.y,
                    hit.normal.y,
                    isSelf,
                    hitCollider.isTrigger,
                    hasVisibleSurface);
            }

            if (!RatVisualGroundingResolver.TrySelectHighestSurface(
                    groundCandidates,
                    candidateCount,
                    hostPosition.y,
                    maxSurfaceRise,
                    maxSurfaceDrop,
                    minSurfaceNormalY,
                    out var surfaceHeight))
            {
                HasGroundSurface = false;
                return;
            }

            HasGroundSurface = true;
            CurrentGroundSurfaceY = surfaceHeight;
            var visualPosition = transform.position;
            visualPosition.y = surfaceHeight + CurrentVisibleFootBottomOffset + groundClearance;
            transform.position = visualPosition;
        }

        /// <summary>
        /// Keeps only the billboard's horizontal world position on the configured sprite grid.
        /// Vertical snapping is intentionally excluded: the visible foot must retain its exact
        /// groundClearance even when a ground surface or clearance is not a grid multiple.
        /// </summary>
        public void RefreshPixelSnap()
        {
            if (!enablePixelSnap || !RatVisualPixelSnapper.IsValidPixelsPerUnit(pixelSnapPixelsPerUnit))
            {
                return;
            }

            transform.position = RatVisualPixelSnapper.SnapHorizontal(transform.position, pixelSnapPixelsPerUnit);
        }

        private void ResolveReferences()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (ratHostController == null)
            {
                ratHostController = GetComponentInParent<RatHostController>();
            }

            if (viewCamera == null)
            {
                viewCamera = Camera.main;
            }
        }

        private void ApplyCurrentSprite()
        {
            if (spriteRenderer != null)
            {
                // Keep an idle rat inside the same complete trial set as its walk cycle.
                // This prevents a visual-quality/form jump between TrialV1 at rest and V3
                // while moving. Incomplete cycles retain the original directional idle fallback.
                var idleWalkFrames = HasCompleteWalkCycles ? GetWalkFrames(CurrentDirection) : null;
                spriteRenderer.sprite = idleWalkFrames != null ? idleWalkFrames[0] : GetSprite(CurrentDirection);
            }
        }

        private Sprite[] GetWalkFrames(RatSpriteDirection direction)
        {
            switch (direction)
            {
                case RatSpriteDirection.SouthWest:
                    return southWestWalkFrames;
                case RatSpriteDirection.West:
                    return westWalkFrames;
                case RatSpriteDirection.NorthWest:
                    return northWestWalkFrames;
                case RatSpriteDirection.North:
                    return northWalkFrames;
                case RatSpriteDirection.NorthEast:
                    return northEastWalkFrames;
                case RatSpriteDirection.East:
                    return eastWalkFrames;
                case RatSpriteDirection.SouthEast:
                    return southEastWalkFrames;
                default:
                    return southWalkFrames;
            }
        }

        private static bool HasCompleteWalkFrames(Sprite[] frames)
        {
            if (frames == null || frames.Length != WalkFrameCount)
            {
                return false;
            }

            for (var i = 0; i < frames.Length; i++)
            {
                if (frames[i] == null)
                {
                    return false;
                }
            }

            return true;
        }

        private Sprite GetSprite(RatSpriteDirection direction)
        {
            switch (direction)
            {
                case RatSpriteDirection.SouthWest:
                    return southWest;
                case RatSpriteDirection.West:
                    return west;
                case RatSpriteDirection.NorthWest:
                    return northWest;
                case RatSpriteDirection.North:
                    return north;
                case RatSpriteDirection.NorthEast:
                    return northEast;
                case RatSpriteDirection.East:
                    return east;
                case RatSpriteDirection.SouthEast:
                    return southEast;
                default:
                    return south;
            }
        }

        private float GetVisibleFootBottomOffset(RatSpriteDirection direction)
        {
            switch (direction)
            {
                case RatSpriteDirection.SouthWest:
                    return southWestVisibleFootBottomOffset;
                case RatSpriteDirection.West:
                    return westVisibleFootBottomOffset;
                case RatSpriteDirection.NorthWest:
                    return northWestVisibleFootBottomOffset;
                case RatSpriteDirection.North:
                    return northVisibleFootBottomOffset;
                case RatSpriteDirection.NorthEast:
                    return northEastVisibleFootBottomOffset;
                case RatSpriteDirection.East:
                    return eastVisibleFootBottomOffset;
                case RatSpriteDirection.SouthEast:
                    return southEastVisibleFootBottomOffset;
                default:
                    return southVisibleFootBottomOffset;
            }
        }

        private void FaceCamera(Transform cameraTransform)
        {
            if (cameraTransform != null)
            {
                transform.rotation = cameraTransform.rotation;
            }
        }
    }
}
