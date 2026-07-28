using UnityEngine;

namespace LastHost.Prototype.RatHost2D
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class RatHost2DContaminationZone : MonoBehaviour
    {
        [SerializeField] private RatHost2DSessionController session;
        [SerializeField] private RatHost2DMovementController host;
        [SerializeField, Min(0f)] private float alertPerSecond =
            RatHost2DSessionController.ContaminationAlertPerSecond;
        [SerializeField, Min(0f)] private float healthDamagePerSecond =
            RatHost2DSessionController.ContaminationHealthDamagePerSecond;
        [SerializeField] private string feedbackLabel =
            RatHost2DSessionController.ContaminationFeedbackLabel;

        public float AlertPerSecond => alertPerSecond;
        public float HealthDamagePerSecond => healthDamagePerSecond;
        public string FeedbackLabel => feedbackLabel;

        public void Configure(
            RatHost2DSessionController sessionController,
            RatHost2DMovementController hostController,
            float alertRate,
            float healthDamageRate,
            string label)
        {
            session = sessionController;
            host = hostController;
            alertPerSecond = Mathf.Max(0f, alertRate);
            healthDamagePerSecond = Mathf.Max(0f, healthDamageRate);
            feedbackLabel = string.IsNullOrWhiteSpace(label)
                ? RatHost2DSessionController.ContaminationFeedbackLabel
                : label.Trim();
            EnsureTrigger();
        }

        public bool ApplyExposure(float deltaTime)
        {
            return session != null
                && session.ApplyContaminationExposure(
                    deltaTime,
                    alertPerSecond,
                    healthDamagePerSecond,
                    feedbackLabel);
        }

        private void Awake()
        {
            EnsureTrigger();
        }

        private void Reset()
        {
            EnsureTrigger();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other == null || !MatchesHost(other))
            {
                return;
            }

            ApplyExposure(Time.fixedDeltaTime);
        }

        private bool MatchesHost(Collider2D other)
        {
            var overlappingHost = other.GetComponentInParent<RatHost2DMovementController>();
            if (overlappingHost == null)
            {
                return false;
            }

            return host == null || overlappingHost == host;
        }

        private void EnsureTrigger()
        {
            var zoneCollider = GetComponent<Collider2D>();
            if (zoneCollider != null)
            {
                zoneCollider.isTrigger = true;
            }
        }
    }
}
