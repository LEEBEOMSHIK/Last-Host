using UnityEngine;

namespace LastHost.Prototype.RatHost2D
{
    public sealed class RatHost2DMutationFragment : MonoBehaviour
    {
        [SerializeField] private RatHost2DSessionController session;
        [SerializeField, Min(0)] private int fragmentIndex;

        private bool _collected;

        public int FragmentIndex => fragmentIndex;
        public bool IsCollected => _collected;

        public void Configure(
            RatHost2DSessionController sessionController,
            int uniqueFragmentIndex)
        {
            session = sessionController;
            fragmentIndex = Mathf.Max(0, uniqueFragmentIndex);
        }

        public bool TryCollect()
        {
            if (_collected
                || session == null
                || !session.QueueVirusFragmentCollected(fragmentIndex))
            {
                return false;
            }

            _collected = true;
            gameObject.SetActive(false);
            return true;
        }

        public void ResetRun()
        {
            _collected = false;
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other != null
                && other.GetComponentInParent<RatHost2DVirusMovementController>() != null)
            {
                TryCollect();
            }
        }
    }
}
