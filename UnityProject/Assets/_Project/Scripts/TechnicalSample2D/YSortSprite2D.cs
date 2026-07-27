using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class YSortSprite2D : MonoBehaviour
    {
        [SerializeField] private Transform footPoint;
        [SerializeField] private SpriteRenderer targetRenderer;
        [SerializeField] private int baseOrder;
        [SerializeField] private int explicitTieBreak;
        [SerializeField, Min(1)] private int scale = TechnicalSample2DConstants.YSortScale;

        public int CurrentSortingOrder =>
            targetRenderer == null ? 0 : targetRenderer.sortingOrder;

        public void Configure(
            Transform sortingFootPoint,
            SpriteRenderer renderer,
            int sortingBaseOrder = 0,
            int tieBreak = 0,
            int sortingScale = TechnicalSample2DConstants.YSortScale)
        {
            footPoint = sortingFootPoint;
            targetRenderer = renderer;
            baseOrder = sortingBaseOrder;
            explicitTieBreak = tieBreak;
            scale = Mathf.Max(1, sortingScale);
        }

        public int ApplySorting()
        {
            EnsureReferences();
            var footY = footPoint == null ? transform.position.y : footPoint.position.y;
            var order = YSortOrder2D.Calculate(footY, baseOrder, explicitTieBreak, scale);

            if (targetRenderer != null)
            {
                targetRenderer.sortingOrder = order;
            }

            return order;
        }

        private void Awake()
        {
            EnsureReferences();
        }

        private void LateUpdate()
        {
            ApplySorting();
        }

        private void EnsureReferences()
        {
            if (footPoint == null)
            {
                footPoint = transform;
            }

            if (targetRenderer == null)
            {
                targetRenderer = GetComponent<SpriteRenderer>();
            }
        }
    }
}
