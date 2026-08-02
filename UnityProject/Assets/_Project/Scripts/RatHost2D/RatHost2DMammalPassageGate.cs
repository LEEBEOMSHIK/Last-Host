using UnityEngine;

namespace LastHost.Prototype.RatHost2D
{
    public sealed class RatHost2DMammalPassageGate : MonoBehaviour
    {
        [SerializeField] private RatHost2DSessionController session;
        [SerializeField] private Collider2D passageCollider;
        [SerializeField] private SpriteRenderer passageRenderer;
        [SerializeField] private Color blockedColor =
            new Color(0.55f, 0.22f, 0.18f, 1f);
        [SerializeField] private Color openColor =
            new Color(0.2f, 0.65f, 0.35f, 0.45f);

        private bool _hasAppliedState;
        private bool _lastOpenState;

        public bool IsOpen =>
            session != null && session.CanUseMammalPassage;

        public void Configure(
            RatHost2DSessionController sessionController,
            Collider2D targetCollider,
            SpriteRenderer targetRenderer)
        {
            session = sessionController;
            passageCollider = targetCollider;
            passageRenderer = targetRenderer;
            _hasAppliedState = false;
            RefreshNow();
        }

        public void ConfigureColors(Color blocked, Color open)
        {
            blockedColor = blocked;
            openColor = open;
            _hasAppliedState = false;
            RefreshNow();
        }

        public void RefreshNow()
        {
            var open = IsOpen;
            if (_hasAppliedState && open == _lastOpenState)
            {
                return;
            }

            _hasAppliedState = true;
            _lastOpenState = open;

            if (passageCollider != null)
            {
                passageCollider.enabled = !open;
            }

            if (passageRenderer != null)
            {
                passageRenderer.color = open ? openColor : blockedColor;
            }
        }

        private void Awake()
        {
            if (passageCollider == null)
            {
                passageCollider = GetComponent<Collider2D>();
            }

            if (passageRenderer == null)
            {
                passageRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }

        private void OnEnable()
        {
            _hasAppliedState = false;
            RefreshNow();
        }

        private void Update()
        {
            RefreshNow();
        }
    }
}
