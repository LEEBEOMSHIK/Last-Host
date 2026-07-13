using LastHost.Prototype.Core;
using LastHost.Prototype.Input;
using UnityEngine;

namespace LastHost.Prototype.Host
{
    public sealed class RatRiskInteractable : MonoBehaviour
    {
        public PrototypeSessionController session;
        public float riskSeverity = 0.75f;
        public float cooldownSeconds = 1.2f;
        public string interactionPrompt = "소음 배관 조사 가능";
        public string immuneAlertFeedbackLabel = "소음/조직 자극";

        private float nextAllowedTime;
        private bool isRatInRange;

        private void Update()
        {
            if (!isRatInRange || session == null || Time.time < nextAllowedTime)
            {
                return;
            }

            if (!PrototypeKeyboardInput.WasInteractPressed())
            {
                return;
            }

            nextAllowedTime = Time.time + cooldownSeconds;
            session.AddRiskAlert(riskSeverity, new ImmuneAlertEvent(ImmuneAlertCauseType.NoiseOrTissueIrritation, immuneAlertFeedbackLabel));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsRatCollider(other))
            {
                SetRatInRange(true);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!IsRatCollider(other))
            {
                return;
            }

            SetRatInRange(true, forceRefresh: true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsRatCollider(other))
            {
                SetRatInRange(false);
            }
        }

        private void OnDisable()
        {
            if (isRatInRange)
            {
                SetRatInRange(false, forceRefresh: true);
            }
        }

        private static bool IsRatCollider(Collider other)
        {
            return other != null && other.GetComponentInParent<RatHostController>() != null;
        }

        private void SetRatInRange(bool inRange, bool forceRefresh = false)
        {
            if (!forceRefresh && isRatInRange == inRange)
            {
                return;
            }

            isRatInRange = inRange;
            session?.SetRatRiskInteractionAffordance(inRange, interactionPrompt);
        }
    }
}
