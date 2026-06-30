using UnityEngine;

namespace LastHost.Prototype.VirusMinigame
{
    public sealed class WhiteBloodCellChaser : MonoBehaviour
    {
        public VirusMinigameController controller;
        public Transform target;
        public float speed = 2.2f;
        public float hitCooldownSeconds = 0.65f;

        private Vector3 startPosition;
        private float nextHitTime;

        private void Awake()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            if (target == null)
            {
                return;
            }

            var direction = target.position - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude <= 0.001f)
            {
                return;
            }

            transform.position += direction.normalized * (speed * Time.deltaTime);
        }

        private void OnTriggerStay(Collider other)
        {
            TryQueueHit(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            TryQueueHit(other);
        }

        public void ResetAgent()
        {
            transform.position = startPosition;
            nextHitTime = 0f;
        }

        private void TryQueueHit(Collider other)
        {
            if (Time.time < nextHitTime || other.GetComponentInParent<VirusPlayerController>() == null)
            {
                return;
            }

            nextHitTime = Time.time + hitCooldownSeconds;
            controller?.QueueWhiteBloodCellHit();
        }
    }
}
