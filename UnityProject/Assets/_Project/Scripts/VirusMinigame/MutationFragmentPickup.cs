using UnityEngine;

namespace LastHost.Prototype.VirusMinigame
{
    public sealed class MutationFragmentPickup : MonoBehaviour
    {
        public VirusMinigameController controller;

        private bool collected;

        private void OnTriggerEnter(Collider other)
        {
            if (collected || other.GetComponentInParent<VirusPlayerController>() == null)
            {
                return;
            }

            controller?.QueueFragmentCollected(this);
        }

        public void MarkCollected()
        {
            collected = true;
            gameObject.SetActive(false);
        }

        public void ResetPickup()
        {
            collected = false;
            gameObject.SetActive(true);
        }
    }
}
