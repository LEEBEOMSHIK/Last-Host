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

        private Collider zoneCollider;
        private int lastDetectedExposureFrame = -1;

        private void Awake()
        {
            zoneCollider = GetComponent<Collider>();
        }

        private void Update()
        {
            ApplyOverlappingRatExposure(Time.deltaTime);
        }

        private void OnTriggerStay(Collider other)
        {
            ApplyDetectedExposure(other, Time.deltaTime);
        }

        public void ApplyOverlappingRatExposure(float deltaTime)
        {
            if (session == null)
            {
                return;
            }

            var detector = GetZoneCollider();
            if (detector == null)
            {
                return;
            }

            var rats = FindObjectsByType<RatHostController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            for (var i = 0; i < rats.Length; i++)
            {
                var ratCollider = rats[i] != null ? rats[i].GetComponent<Collider>() : null;
                if (ratCollider != null && detector.bounds.Intersects(ratCollider.bounds))
                {
                    ApplyDetectedExposure(ratCollider, deltaTime);
                    return;
                }
            }
        }

        public void ApplyExposure(Collider other, float deltaTime)
        {
            if (session == null
                || session.State == null
                || session.CurrentMode != PrototypeGameMode.RatHost
                || session.State.IsRatHostRiskZoneGraceActive
                || other == null
                || other.GetComponentInParent<RatHostController>() == null)
            {
                return;
            }

            var clampedDeltaTime = Mathf.Max(0f, deltaTime);
            session.AddImmuneAlertAmount(alertPerSecond * clampedDeltaTime, immuneAlertFeedbackLabel);
            session.DamageHost(hostDamagePerSecond * clampedDeltaTime);
        }

        private void ApplyDetectedExposure(Collider other, float deltaTime)
        {
            if (other == null || other.GetComponentInParent<RatHostController>() == null)
            {
                return;
            }

            if (lastDetectedExposureFrame == Time.frameCount)
            {
                return;
            }

            lastDetectedExposureFrame = Time.frameCount;
            ApplyExposure(other, deltaTime);
        }

        private Collider GetZoneCollider()
        {
            if (zoneCollider == null)
            {
                zoneCollider = GetComponent<Collider>();
            }

            return zoneCollider;
        }
    }
}
