using System;
using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class RatHost2DView : MonoBehaviour
    {
        private const int DirectionCount = 8;
        private const int FramesPerDirection = 2;
        private const int RequiredFrameCount = DirectionCount * FramesPerDirection;

        [SerializeField] private RatHost2DController controller;
        [SerializeField] private SpriteRenderer targetRenderer;
        [SerializeField] private Sprite[] directionFrames = new Sprite[RequiredFrameCount];
        [SerializeField, Min(0.01f)] private float walkFramesPerSecond =
            TechnicalSample2DConstants.WalkFramesPerSecond;

        private float _walkElapsed;
        private Direction8 _lastDirection = Direction8.South;

        public SpriteRenderer TargetRenderer => targetRenderer;
        public int FrameCount => directionFrames == null ? 0 : directionFrames.Length;

        public void Configure(
            RatHost2DController sourceController,
            SpriteRenderer renderer,
            Sprite[] frames,
            float framesPerSecond = TechnicalSample2DConstants.WalkFramesPerSecond)
        {
            controller = sourceController;
            targetRenderer = renderer;
            directionFrames = frames;
            walkFramesPerSecond = Mathf.Max(0.01f, framesPerSecond);
            ValidateFrameArray();
        }

        public Sprite GetFrame(Direction8 direction, int frameIndex)
        {
            ValidateFrameArray();
            var clampedFrame = Mathf.Clamp(frameIndex, 0, FramesPerDirection - 1);
            return directionFrames[((int)direction * FramesPerDirection) + clampedFrame];
        }

        public void UpdateView(Direction8 direction, bool isMoving, float deltaTime)
        {
            EnsureRenderer();

            if (direction != _lastDirection)
            {
                _lastDirection = direction;
                _walkElapsed = 0f;
            }

            if (!isMoving)
            {
                _walkElapsed = 0f;
                targetRenderer.sprite = GetFrame(direction, 0);
                return;
            }

            _walkElapsed += Mathf.Max(0f, deltaTime);
            var frame = Mathf.FloorToInt(_walkElapsed * walkFramesPerSecond) % FramesPerDirection;
            targetRenderer.sprite = GetFrame(direction, frame);
        }

        private void Awake()
        {
            EnsureRenderer();
        }

        private void LateUpdate()
        {
            if (controller == null)
            {
                return;
            }

            UpdateView(controller.FacingDirection, controller.IsMoving, Time.deltaTime);
        }

        private void EnsureRenderer()
        {
            if (targetRenderer == null)
            {
                targetRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void ValidateFrameArray()
        {
            if (directionFrames == null || directionFrames.Length != RequiredFrameCount)
            {
                throw new InvalidOperationException(
                    $"Direction frame array must contain exactly {RequiredFrameCount} sprites.");
            }
        }
    }
}
