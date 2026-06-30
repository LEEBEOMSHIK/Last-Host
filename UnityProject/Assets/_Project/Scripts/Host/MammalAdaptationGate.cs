using LastHost.Prototype.Core;
using UnityEngine;

namespace LastHost.Prototype.Host
{
    public sealed class MammalAdaptationGate : MonoBehaviour
    {
        public PrototypeSessionController session;
        public Collider gateCollider;
        public Renderer gateRenderer;
        public Material blockedMaterial;
        public Material openMaterial;

        private bool lastOpenState;

        private void Awake()
        {
            if (gateCollider == null)
            {
                gateCollider = GetComponent<Collider>();
            }

            if (gateRenderer == null)
            {
                gateRenderer = GetComponentInChildren<Renderer>();
            }
        }

        private void Update()
        {
            var open = session != null && session.State.Mutations.CanUseMammalPassage;
            if (open == lastOpenState)
            {
                return;
            }

            lastOpenState = open;
            if (gateCollider != null)
            {
                gateCollider.enabled = !open;
            }

            if (gateRenderer != null)
            {
                gateRenderer.sharedMaterial = open ? openMaterial : blockedMaterial;
            }
        }
    }
}
