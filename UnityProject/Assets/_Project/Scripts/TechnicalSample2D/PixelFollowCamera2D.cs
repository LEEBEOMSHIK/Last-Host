using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D
{
    [RequireComponent(typeof(Camera))]
    public sealed class PixelFollowCamera2D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField, Min(1)] private int pixelsPerUnit = TechnicalSample2DConstants.PixelsPerUnit;
        [SerializeField, Min(0.01f)] private float orthographicSize =
            TechnicalSample2DConstants.TrialOrthographicSize;

        private Camera _camera;

        public Transform Target => target;
        public Camera TargetCamera => _camera != null ? _camera : GetComponent<Camera>();

        public Vector2 WorldCenterError =>
            target == null ? Vector2.zero : (Vector2)(target.position - transform.position);

        public Vector2 LogicalPixelError => WorldCenterError * pixelsPerUnit;

        public void Configure(
            Transform followTarget,
            int ppu = TechnicalSample2DConstants.PixelsPerUnit,
            float size = TechnicalSample2DConstants.TrialOrthographicSize)
        {
            PixelGrid2D.ValidatePixelsPerUnit(ppu);
            target = followTarget;
            pixelsPerUnit = ppu;
            orthographicSize = Mathf.Max(0.01f, size);
            EnsureCamera();
            ApplyCameraSettings();
        }

        public void ApplyFollow()
        {
            EnsureCamera();
            ApplyCameraSettings();

            if (target == null)
            {
                return;
            }

            var snapped = PixelGrid2D.Snap((Vector2)target.position, pixelsPerUnit);
            transform.position = new Vector3(snapped.x, snapped.y, transform.position.z);
        }

        private void Awake()
        {
            EnsureCamera();
            ApplyCameraSettings();
        }

        private void LateUpdate()
        {
            ApplyFollow();
        }

        private void EnsureCamera()
        {
            if (_camera == null)
            {
                _camera = GetComponent<Camera>();
            }
        }

        private void ApplyCameraSettings()
        {
            if (_camera == null)
            {
                return;
            }

            _camera.orthographic = true;
            _camera.orthographicSize = orthographicSize;
        }
    }
}
