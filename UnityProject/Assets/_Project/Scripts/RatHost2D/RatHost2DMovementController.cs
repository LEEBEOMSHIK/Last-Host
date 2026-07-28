using LastHost.Prototype.TechnicalSample2D;
using UnityEngine;

namespace LastHost.Prototype.RatHost2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(RatHost2DController))]
    public sealed class RatHost2DMovementController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RatHost2DSessionController session;
        [SerializeField] private TechnicalSample2DInput input;

        [Header("Movement")]
        [SerializeField, Min(0f)] private float moveSpeed = 3f;
        [SerializeField, Range(0f, 1f)] private float passiveInstinctSpeedMultiplier = 0.35f;
        [SerializeField, Range(0f, 1f)] private float forcedControlSpeedMultiplier = 0.55f;
        [SerializeField, Range(-1f, 1f)] private float conflictDotThreshold = -0.25f;

        [Header("Deterministic Instinct Wander")]
        [SerializeField] private Vector2 instinctDirection = Vector2.up;
        [SerializeField] private Vector2 xBounds = new Vector2(-4f, 4f);
        [SerializeField] private Vector2 yBounds = new Vector2(-2.5f, 2.5f);
        [SerializeField, Min(0.1f)] private float turnIntervalSeconds = 1.5f;
        [SerializeField, Range(15f, 160f)] private float turnAngleDegrees = 45f;

        private Rigidbody2D _body;
        private RatHost2DController _motor;
        private Vector2 _cachedPlayerInput;
        private RatHost2DControlFrame _lastControlFrame;
        private float _turnElapsedSeconds;
        private float _turnSign = 1f;
        private bool _hostGameplayEnabled = true;

        public Rigidbody2D Body => _body;
        public RatHost2DController Motor => _motor;
        public Transform FollowTarget => transform;
        public Vector2 LogicalPosition => _body == null ? (Vector2)transform.position : _body.position;
        public Vector2 CachedPlayerInput => _cachedPlayerInput;
        public Vector2 CurrentInstinctDirection => instinctDirection;
        public Vector2 CurrentMoveDirection => _lastControlFrame.MoveDirection;
        public RatHost2DControlFrame LastControlFrame => _lastControlFrame;
        public Direction8 FacingDirection => _motor == null
            ? Direction8.South
            : _motor.FacingDirection;
        public bool IsHostGameplayEnabled => _hostGameplayEnabled;

        public void Configure(
            RatHost2DSessionController sessionController,
            TechnicalSample2DInput inputSource,
            float speed)
        {
            session = sessionController;
            input = inputSource;
            moveSpeed = Mathf.Max(0f, speed);
            EnsureComponents();
            ConfigureMotor(moveSpeed);
        }

        public void ConfigureInstinct(
            Vector2 initialDirection,
            Vector2 horizontalBounds,
            Vector2 verticalBounds,
            float turnInterval,
            float turnAngle)
        {
            instinctDirection = Movement2DModel.NormalizeInput(initialDirection);
            if (instinctDirection == Vector2.zero)
            {
                instinctDirection = Vector2.up;
            }

            xBounds = OrderedBounds(horizontalBounds);
            yBounds = OrderedBounds(verticalBounds);
            turnIntervalSeconds = Mathf.Max(0.1f, turnInterval);
            turnAngleDegrees = Mathf.Clamp(turnAngle, 15f, 160f);
            _turnElapsedSeconds = 0f;
            _turnSign = 1f;
        }

        public void CachePlayerInput(Vector2 rawInput)
        {
            _cachedPlayerInput = Movement2DModel.NormalizeInput(rawInput);
        }

        public void SetHostGameplayEnabled(bool enabled)
        {
            _hostGameplayEnabled = enabled;
            if (!enabled)
            {
                StopImmediately();
            }
        }

        public void SimulateFixedStep(float fixedDeltaTime)
        {
            EnsureComponents();
            var deltaTime = Mathf.Max(0f, fixedDeltaTime);

            if (!CanMoveHost() || deltaTime <= 0f)
            {
                StopImmediately();
                return;
            }

            AdvanceInstinct(deltaTime);

            var state = session == null ? null : session.State;
            var virusControlPower = state == null ? 0.35f : state.Mutations.RatControlPower;
            var hostResistance = state == null ? 1f : state.Config.RatHostInstinctResistance;
            var mutationSpeedMultiplier = state == null ? 1f : state.Mutations.RatSpeedMultiplier;

            _lastControlFrame = RatHost2DControlAdapter.Resolve(
                instinctDirection,
                _cachedPlayerInput,
                virusControlPower,
                hostResistance,
                conflictDotThreshold,
                passiveInstinctSpeedMultiplier,
                forcedControlSpeedMultiplier);

            ConfigureMotor(moveSpeed * mutationSpeedMultiplier * _lastControlFrame.SpeedMultiplier);
            _motor.CacheMoveInput(_lastControlFrame.MoveDirection);
            _motor.SimulateFixedStep(deltaTime);
        }

        private void Awake()
        {
            EnsureComponents();
            instinctDirection = Movement2DModel.NormalizeInput(instinctDirection);
            if (instinctDirection == Vector2.zero)
            {
                instinctDirection = Vector2.up;
            }

            ConfigureMotor(moveSpeed);
        }

        private void OnDisable()
        {
            StopImmediately();
        }

        private void Update()
        {
            if (!CanMoveHost())
            {
                CachePlayerInput(Vector2.zero);
                return;
            }

            CachePlayerInput(input == null ? Vector2.zero : input.ReadMove());
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

            // RatHost2DController is composed as the single collision motor. Its own
            // Update/FixedUpdate must stay disabled so it cannot overwrite the resolved
            // instinct/WASD frame with a second input source.
            if (_motor != null)
            {
                _motor.enabled = false;
            }
        }

        private void ConfigureMotor(float speed)
        {
            if (_motor == null)
            {
                return;
            }

            _motor.Configure(null, Mathf.Max(0f, speed));
        }

        private bool CanMoveHost()
        {
            return _hostGameplayEnabled
                && (session == null || session.CanProcessHostGameplay);
        }

        private void AdvanceInstinct(float deltaTime)
        {
            _turnElapsedSeconds += deltaTime;
            var turnRequested = _turnElapsedSeconds >= Mathf.Max(0.1f, turnIntervalSeconds);
            if (turnRequested)
            {
                _turnElapsedSeconds = 0f;
            }

            instinctDirection = RatHost2DControlAdapter.ResolveNextInstinctDirection(
                instinctDirection,
                Vector2.up,
                LogicalPosition,
                OrderedBounds(xBounds),
                OrderedBounds(yBounds),
                turnRequested,
                turnAngleDegrees,
                _turnSign);

            if (turnRequested)
            {
                _turnSign = -_turnSign;
            }
        }

        private void StopImmediately()
        {
            _cachedPlayerInput = Vector2.zero;
            _lastControlFrame = new RatHost2DControlFrame(
                Vector2.zero,
                0f,
                false,
                _lastControlFrame.ControlRatio);

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

        private static Vector2 OrderedBounds(Vector2 bounds)
        {
            return bounds.x <= bounds.y ? bounds : new Vector2(bounds.y, bounds.x);
        }
    }
}
