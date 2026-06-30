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

        private void OnTriggerStay(Collider other)
        {
            if (session == null || other.GetComponentInParent<RatHostController>() == null)
            {
                return;
            }

            session.AddImmuneAlertAmount(alertPerSecond * Time.deltaTime);
            session.DamageHost(hostDamagePerSecond * Time.deltaTime);
        }
    }
}
