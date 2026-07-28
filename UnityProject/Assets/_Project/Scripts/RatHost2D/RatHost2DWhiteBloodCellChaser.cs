using LastHost.Prototype.TechnicalSample2D;
using UnityEngine;

namespace LastHost.Prototype.RatHost2D
{
    public sealed class RatHost2DContactCooldownGate
    {
        private float _nextAllowedTime;
        private bool _hasConsumed;

        public void Reset()
        {
            _nextAllowedTime = 0f;
            _hasConsumed = false;
        }

        public bool TryConsume(float timeSeconds, float cooldownSeconds)
        {
            var now = Mathf.Max(0f, timeSeconds);
            if (_hasConsumed && now < _nextAllowedTime)
            {
                return false;
            }

            _hasConsumed = true;
            _nextAllowedTime = now + Mathf.Max(0f, cooldownSeconds);
            return true;
        }
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class RatHost2DWhiteBloodCellChaser : MonoBehaviour
    {
        [SerializeField] private RatHost2DSessionController session;
        [SerializeField] private RatHost2DVirusMovementController target;
        [SerializeField, Min(0f)] private float moveSpeed = 1.8f;
        [SerializeField, Min(0f)] private float contactCooldownSeconds = 0.5f;

        private readonly RatHost2DContactCooldownGate _contactGate =
            new RatHost2DContactCooldownGate();
        private Rigidbody2D _body;
        private Vector2 _spawnPosition;
        private bool _spawnCaptured;
        private bool _virusGameplayEnabled;

        public Rigidbody2D Body => _body;
        public Transform FollowTarget => target == null ? null : target.FollowTarget;
        public bool IsVirusGameplayEnabled => _virusGameplayEnabled;
        public float ContactCooldownSeconds => contactCooldownSeconds;

        public void Configure(
            RatHost2DSessionController sessionController,
            RatHost2DVirusMovementController virusTarget,
            float speed,
            float contactCooldown)
        {
            session = sessionController;
            target = virusTarget;
            moveSpeed = Mathf.Max(0f, speed);
            contactCooldownSeconds = Mathf.Max(0f, contactCooldown);
            EnsureBody();
            CaptureSpawnPosition();
        }

        public void SetVirusGameplayEnabled(bool enabled)
        {
            _virusGameplayEnabled = enabled;
            if (!enabled && _body != null)
            {
                _body.linearVelocity = Vector2.zero;
            }
        }

        public void ResetRun()
        {
            EnsureBody();
            CaptureSpawnPosition();
            _contactGate.Reset();
            if (_body != null)
            {
                _body.position = _spawnPosition;
                _body.linearVelocity = Vector2.zero;
            }

            transform.position = new Vector3(
                _spawnPosition.x,
                _spawnPosition.y,
                transform.position.z);
        }

        public bool TryApplyContact(float timeSeconds)
        {
            if (!CanChase()
                || !_contactGate.TryConsume(timeSeconds, contactCooldownSeconds))
            {
                return false;
            }

            return session != null && session.QueueWhiteBloodCellHit();
        }

        public void SimulateFixedStep(float fixedDeltaTime)
        {
            EnsureBody();
            if (!CanChase() || fixedDeltaTime <= 0f)
            {
                if (_body != null)
                {
                    _body.linearVelocity = Vector2.zero;
                }

                return;
            }

            var direction = Movement2DModel.NormalizeInput(
                target.LogicalPosition - _body.position);
            var speed = moveSpeed * session.State.WhiteBloodCellSpeedMultiplier;
            var step = Movement2DModel.CalculateStep(
                direction,
                speed,
                Mathf.Max(0f, fixedDeltaTime));
            _body.MovePosition(_body.position + step);
        }

        private void Awake()
        {
            EnsureBody();
            CaptureSpawnPosition();
        }

        private void FixedUpdate()
        {
            SimulateFixedStep(Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryApplyContactAgainst(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TryApplyContactAgainst(other);
        }

        private void TryApplyContactAgainst(Collider2D other)
        {
            if (other != null
                && other.GetComponentInParent<RatHost2DVirusMovementController>() == target)
            {
                TryApplyContact(Time.time);
            }
        }

        private bool CanChase()
        {
            return _virusGameplayEnabled
                && session != null
                && target != null
                && session.CanProcessVirusGameplay;
        }

        private void EnsureBody()
        {
            if (_body == null)
            {
                _body = GetComponent<Rigidbody2D>();
            }

            if (_body != null)
            {
                _body.gravityScale = 0f;
                _body.freezeRotation = true;
                _body.interpolation = RigidbodyInterpolation2D.Interpolate;
            }
        }

        private void CaptureSpawnPosition()
        {
            if (_spawnCaptured)
            {
                return;
            }

            _spawnPosition = _body == null
                ? (Vector2)transform.position
                : _body.position;
            _spawnCaptured = true;
        }
    }
}
