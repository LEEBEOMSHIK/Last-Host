using LastHost.Prototype.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LastHost.Prototype.Host
{
    public sealed class RatRiskInteractable : MonoBehaviour
    {
        public PrototypeSessionController session;
        public float riskSeverity = 0.75f;
        public float cooldownSeconds = 1.2f;

        private float nextAllowedTime;

        private void OnTriggerStay(Collider other)
        {
            if (session == null || Keyboard.current == null || Time.time < nextAllowedTime)
            {
                return;
            }

            if (!Keyboard.current.spaceKey.wasPressedThisFrame || other.GetComponentInParent<RatHostController>() == null)
            {
                return;
            }

            nextAllowedTime = Time.time + cooldownSeconds;
            session.AddRiskAlert(riskSeverity);
        }
    }
}
