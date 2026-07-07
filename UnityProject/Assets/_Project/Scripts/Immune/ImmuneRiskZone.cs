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
        public float forcedControlAlertAmount = 8f;
        public float forcedControlHoldSeconds = 0.5f;
        public float forcedControlCooldownSeconds = 1.2f;
        public float forcedControlDirectionDotThreshold = 0.7f;
        public float forcedControlDetectionDistance = 1.5f;
        public string forcedControlFeedbackLabel = "강제 조종";

        private Collider zoneCollider;
        private HostInstinctControlSpike forcedControlSpike;
        private int lastDetectedExposureFrame = -1;

        private void Awake()
        {
            zoneCollider = GetComponent<Collider>();
            forcedControlSpike = CreateForcedControlSpike();
        }

        private void Update()
        {
            ApplyOverlappingRatExposure(Time.deltaTime);
            ApplyNearbyForcedControlInput(Time.deltaTime);
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

            var rats = FindObjectsByType<RatHostController>(FindObjectsInactive.Exclude);
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

        public void ApplyForcedControlInput(RatHostController rat, Vector3 inputDirection, float deltaTime, float currentTimeSeconds)
        {
            if (session == null
                || session.State == null
                || session.CurrentMode != PrototypeGameMode.RatHost
                || session.State.IsRatHostRiskZoneGraceActive
                || rat == null)
            {
                return;
            }

            if (forcedControlSpike == null)
            {
                forcedControlSpike = CreateForcedControlSpike();
            }

            if (!forcedControlSpike.Tick(rat.transform.position, GetDangerCenter(), inputDirection, deltaTime, currentTimeSeconds))
            {
                return;
            }

            session.AddImmuneAlertAmount(forcedControlAlertAmount, forcedControlFeedbackLabel);
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

        private void ApplyNearbyForcedControlInput(float deltaTime, float currentTimeSeconds)
        {
            if (session == null || session.State == null || session.CurrentMode != PrototypeGameMode.RatHost)
            {
                return;
            }

            var rats = FindObjectsByType<RatHostController>(FindObjectsInactive.Exclude);
            for (var i = 0; i < rats.Length; i++)
            {
                var rat = rats[i];
                if (rat == null || !IsRatNearDanger(rat))
                {
                    continue;
                }

                ApplyForcedControlInput(rat, rat.CurrentMoveWorldDirection, deltaTime, currentTimeSeconds);
                return;
            }

            forcedControlSpike?.Reset();
        }

        private void ApplyNearbyForcedControlInput(float deltaTime)
        {
            ApplyNearbyForcedControlInput(deltaTime, Time.time);
        }

        private bool IsRatNearDanger(RatHostController rat)
        {
            var detector = GetZoneCollider();
            if (detector == null)
            {
                return true;
            }

            var closest = detector.ClosestPoint(rat.transform.position);
            var flatOffset = Vector3.ProjectOnPlane(closest - rat.transform.position, Vector3.up);
            var detectionDistance = Mathf.Max(0f, forcedControlDetectionDistance);
            return flatOffset.sqrMagnitude <= detectionDistance * detectionDistance;
        }

        private Vector3 GetDangerCenter()
        {
            var detector = GetZoneCollider();
            return detector != null ? detector.bounds.center : transform.position;
        }

        private HostInstinctControlSpike CreateForcedControlSpike()
        {
            return new HostInstinctControlSpike(
                forcedControlHoldSeconds,
                forcedControlCooldownSeconds,
                forcedControlDirectionDotThreshold);
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
