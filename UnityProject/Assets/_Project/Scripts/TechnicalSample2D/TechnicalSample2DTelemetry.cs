using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D
{
    public readonly struct TechnicalSample2DTelemetrySnapshot
    {
        public TechnicalSample2DTelemetrySnapshot(
            Vector2 rootPosition,
            Direction8 direction,
            bool isMoving,
            float maximumObservedStep,
            Vector2 cameraErrorPixels,
            int sortingOrder)
        {
            RootPosition = rootPosition;
            Direction = direction;
            IsMoving = isMoving;
            MaximumObservedStep = maximumObservedStep;
            CameraErrorPixels = cameraErrorPixels;
            SortingOrder = sortingOrder;
        }

        public Vector2 RootPosition { get; }
        public Direction8 Direction { get; }
        public bool IsMoving { get; }
        public float MaximumObservedStep { get; }
        public Vector2 CameraErrorPixels { get; }
        public int SortingOrder { get; }
    }

    public sealed class TechnicalSample2DTelemetry : MonoBehaviour
    {
        [SerializeField] private RatHost2DController controller;
        [SerializeField] private PixelFollowCamera2D followCamera;
        [SerializeField] private YSortSprite2D ySort;

        private float _maximumObservedStep;

        public void Configure(
            RatHost2DController sourceController,
            PixelFollowCamera2D sourceCamera,
            YSortSprite2D sourceYSort)
        {
            controller = sourceController;
            followCamera = sourceCamera;
            ySort = sourceYSort;
        }

        public void ResetMaximumObservedStep()
        {
            _maximumObservedStep = 0f;
        }

        public TechnicalSample2DTelemetrySnapshot Capture()
        {
            if (controller != null)
            {
                _maximumObservedStep = Mathf.Max(
                    _maximumObservedStep,
                    controller.LastFixedStepDelta.magnitude);
            }

            return new TechnicalSample2DTelemetrySnapshot(
                controller == null ? Vector2.zero : controller.Body.position,
                controller == null ? Direction8.South : controller.FacingDirection,
                controller != null && controller.IsMoving,
                _maximumObservedStep,
                followCamera == null ? Vector2.zero : followCamera.LogicalPixelError,
                ySort == null ? 0 : ySort.CurrentSortingOrder);
        }
    }
}
