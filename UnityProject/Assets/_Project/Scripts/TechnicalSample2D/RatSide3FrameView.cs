using System;
using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D
{
    /// <summary>
    /// Displays only the supplied Production2D V1 side-view walk frames.
    /// Horizontal input may mirror the supplied side view; no missing direction is synthesized.
    /// </summary>
    public sealed class RatSide3FrameView : MonoBehaviour
    {
        private const int RequiredFrameCount = 3;
        private static readonly Vector2 StableColliderSize =
            new Vector2(1.2265625f, 0.25f);
        private static readonly Vector2 StableRightFacingColliderOffset =
            new Vector2(0.28515625f, 0.125f);

        [SerializeField] private RatHost2DController controller;
        [SerializeField] private SpriteRenderer targetRenderer;
        [SerializeField] private Sprite[] frames = new Sprite[RequiredFrameCount];
        [SerializeField, Min(0.01f)] private float framesPerSecond = 7f;
        [SerializeField] private CapsuleCollider2D bodyClearanceCollider;
        [SerializeField] private Vector2 rightFacingColliderOffset =
            new Vector2(0.28515625f, 0.125f);

        private float _animationTime;
        private bool _facesRight = true;
        private int _currentFrameIndex;

        public int FrameCount => frames == null ? 0 : frames.Length;
        public int CurrentFrameIndex => _currentFrameIndex;
        public bool FacesRight => _facesRight;
        public SpriteRenderer TargetRenderer => targetRenderer;
        public CapsuleCollider2D BodyClearanceCollider => bodyClearanceCollider;
        public Vector2 BodyClearanceSize => StableColliderSize;
        public Vector2 RightFacingColliderOffset => StableRightFacingColliderOffset;

        public void Configure(
            RatHost2DController sourceController,
            SpriteRenderer renderer,
            Sprite[] suppliedSideFrames,
            float playbackFramesPerSecond = 7f)
        {
            if (suppliedSideFrames == null ||
                suppliedSideFrames.Length != RequiredFrameCount)
            {
                throw new ArgumentException(
                    "Production2D V1 requires exactly three supplied side-view frames.",
                    nameof(suppliedSideFrames));
            }

            for (var index = 0; index < suppliedSideFrames.Length; index++)
            {
                if (suppliedSideFrames[index] == null)
                {
                    throw new ArgumentException(
                        $"Supplied side-view frame {index} is missing.",
                        nameof(suppliedSideFrames));
                }
            }

            controller = sourceController;
            targetRenderer = renderer;
            frames = suppliedSideFrames;
            framesPerSecond = Mathf.Max(0.01f, playbackFramesPerSecond);
            _animationTime = 0f;
            ApplyView(Vector2.zero, false, 0f);
        }

        public void ConfigureBodyClearance(
            CapsuleCollider2D targetCollider,
            Vector2 colliderSize,
            Vector2 rightFacingOffset)
        {
            bodyClearanceCollider = targetCollider;
            // Keep the existing method signature for serialized scene/builder API
            // compatibility, but the measured gameplay footprint is not configurable.
            this.rightFacingColliderOffset = StableRightFacingColliderOffset;
            ApplyBodyClearanceFacing();
        }

        public void ApplyView(Vector2 move, bool isMoving, float deltaTime)
        {
            if (targetRenderer == null || frames == null ||
                frames.Length != RequiredFrameCount)
            {
                return;
            }

            if (move.x > 0.0001f)
            {
                _facesRight = true;
            }
            else if (move.x < -0.0001f)
            {
                _facesRight = false;
            }

            targetRenderer.flipX = !_facesRight;
            ApplyBodyClearanceFacing();

            if (!isMoving)
            {
                _animationTime = 0f;
                _currentFrameIndex = 0;
                targetRenderer.sprite = frames[0];
                return;
            }

            _animationTime += Mathf.Max(0f, deltaTime);
            _currentFrameIndex =
                Mathf.FloorToInt(_animationTime * framesPerSecond) % RequiredFrameCount;
            targetRenderer.sprite = frames[_currentFrameIndex];
        }

        private void Awake()
        {
            if (targetRenderer == null)
            {
                targetRenderer = GetComponent<SpriteRenderer>();
            }

            ApplyBodyClearanceFacing();
        }

        private void Update()
        {
            var move = controller == null ? Vector2.zero : controller.CachedMove;
            ApplyView(
                move,
                controller != null && controller.IsMoving,
                Time.deltaTime);
        }

        private void ApplyBodyClearanceFacing()
        {
            if (bodyClearanceCollider == null)
            {
                return;
            }

            rightFacingColliderOffset = StableRightFacingColliderOffset;
            bodyClearanceCollider.direction = CapsuleDirection2D.Horizontal;
            bodyClearanceCollider.size = StableColliderSize;
            bodyClearanceCollider.offset = new Vector2(
                _facesRight
                    ? rightFacingColliderOffset.x
                    : -rightFacingColliderOffset.x,
                rightFacingColliderOffset.y);
        }
    }
}
