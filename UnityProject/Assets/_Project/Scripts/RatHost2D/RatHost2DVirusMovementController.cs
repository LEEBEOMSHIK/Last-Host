using LastHost.Prototype.TechnicalSample2D;
using UnityEngine;

namespace LastHost.Prototype.RatHost2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(RatHost2DController))]
    public sealed class RatHost2DVirusMovementController : MonoBehaviour
    {
        [SerializeField] private RatHost2DSessionController session;
        [SerializeField] private TechnicalSample2DInput input;
        [SerializeField, Min(0f)] private float moveSpeed = 3f;

        private Rigidbody2D _body;
        private RatHost2DController _motor;
        private Vector2 _cachedMove;
        private Vector2 _spawnPosition;
        private bool _spawnCaptured;
        private bool _virusGameplayEnabled;

        public Rigidbody2D Body => _body;
        public RatHost2DController Motor => _motor;
        public Transform FollowTarget => transform;
        public Vector2 LogicalPosition =>
            _body == null ? (Vector2)transform.position : _body.position;
        public Vector2 CachedMove => _cachedMove;
        public bool IsVirusGameplayEnabled => _virusGameplayEnabled;
        public Direction8 FacingDirection => _motor == null
            ? Direction8.South
            : _motor.FacingDirection;

        public void Configure(
            RatHost2DSessionController sessionController,
            TechnicalSample2DInput inputSource,
            float speed)
        {
            session = sessionController;
            input = inputSource;
            moveSpeed = Mathf.Max(0f, speed);
            EnsureComponents();
            CaptureSpawnPosition();
            _motor.Configure(null, moveSpeed);
        }

        public void CachePlayerInput(Vector2 rawInput)
        {
            _cachedMove = Movement2DModel.NormalizeInput(rawInput);
        }

        public void SetVirusGameplayEnabled(bool enabled)
        {
            _virusGameplayEnabled = enabled;
            if (!enabled)
            {
                StopImmediately();
            }
        }

        public void ResetRun()
        {
            EnsureComponents();
            CaptureSpawnPosition();
            StopImmediately();
            if (_body != null)
            {
                _body.position = _spawnPosition;
            }

            transform.position = new Vector3(
                _spawnPosition.x,
                _spawnPosition.y,
                transform.position.z);
        }

        public void SimulateFixedStep(float fixedDeltaTime)
        {
            EnsureComponents();
            if (!CanMoveVirus() || fixedDeltaTime <= 0f)
            {
                StopImmediately();
                return;
            }

            _motor.CacheMoveInput(_cachedMove);
            _motor.SimulateFixedStep(Mathf.Max(0f, fixedDeltaTime));
        }

        private void Awake()
        {
            EnsureComponents();
            CaptureSpawnPosition();
            _motor.Configure(null, moveSpeed);
        }

        private void OnDisable()
        {
            StopImmediately();
        }

        private void Update()
        {
            CachePlayerInput(CanMoveVirus() && input != null
                ? input.ReadMove()
                : Vector2.zero);
        }

        private void FixedUpdate()
        {
            SimulateFixedStep(Time.fixedDeltaTime);
        }

        private void EnsureComponents()
        {
            if (_body == null)
            {
                _body = GetComponent<Rigidbody2D>();
            }

            if (_motor == null)
            {
                _motor = GetComponent<RatHost2DController>();
            }

            if (_body != null)
            {
                _body.gravityScale = 0f;
                _body.freezeRotation = true;
                _body.interpolation = RigidbodyInterpolation2D.Interpolate;
            }

            if (_motor != null)
            {
                _motor.enabled = false;
            }
        }

        private void CaptureSpawnPosition()
        {
            if (_spawnCaptured)
            {
                return;
            }

            _spawnPosition = LogicalPosition;
            _spawnCaptured = true;
        }

        private bool CanMoveVirus()
        {
            return _virusGameplayEnabled
                && (session == null || session.CanProcessVirusGameplay);
        }

        private void StopImmediately()
        {
            _cachedMove = Vector2.zero;
            if (_motor != null)
            {
                _motor.CacheMoveInput(Vector2.zero);
            }

            if (_body != null)
            {
                _body.linearVelocity = Vector2.zero;
                _body.angularVelocity = 0f;
            }
        }
    }
}
