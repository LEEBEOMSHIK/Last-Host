using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D
{
    public sealed class VisualPixelSnap2D : MonoBehaviour
    {
        [SerializeField] private Transform logicalRoot;
        [SerializeField, Min(1)] private int pixelsPerUnit = TechnicalSample2DConstants.PixelsPerUnit;

        public Transform LogicalRoot => logicalRoot;
        public int PixelsPerUnit => pixelsPerUnit;

        public void Configure(Transform root, int ppu = TechnicalSample2DConstants.PixelsPerUnit)
        {
            PixelGrid2D.ValidatePixelsPerUnit(ppu);
            logicalRoot = root;
            pixelsPerUnit = ppu;
        }

        public void ApplySnap()
        {
            if (logicalRoot == null)
            {
                return;
            }

            var snappedRoot = PixelGrid2D.Snap((Vector2)logicalRoot.position, pixelsPerUnit);
            transform.position = new Vector3(snappedRoot.x, snappedRoot.y, transform.position.z);
        }

        private void LateUpdate()
        {
            ApplySnap();
        }
    }
}
