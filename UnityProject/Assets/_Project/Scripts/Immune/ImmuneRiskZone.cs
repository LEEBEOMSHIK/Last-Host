using LastHost.Prototype.Core;
using LastHost.Prototype.Host;
using UnityEngine;

namespace LastHost.Prototype.Immune
{
    public sealed class ImmuneRiskZone : MonoBehaviour
    {
        public PrototypeSessionController session;
        public float alertPerSecond = 12f;
        public float hostDamagePerSecond = 4f;
        public string immuneAlertFeedbackLabel = "오염 노출";

        private void OnTriggerStay(Collider other)
        {
            ApplyExposure(other, Time.deltaTime);
        }

        public void ApplyExposure(Collider other, float deltaTime)
        {
            if (session == null || other == null || other.GetComponentInParent<RatHostController>() == null)
            {
                return;
            }

            var clampedDeltaTime = Mathf.Max(0f, deltaTime);
            session.AddImmuneAlertAmount(alertPerSecond * clampedDeltaTime, immuneAlertFeedbackLabel);
            session.DamageHost(hostDamagePerSecond * clampedDeltaTime);
        }
    }
}
