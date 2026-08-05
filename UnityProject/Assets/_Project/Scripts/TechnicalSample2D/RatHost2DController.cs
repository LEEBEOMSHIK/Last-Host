using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class RatHost2DController : MonoBehaviour
    {
        private const float CollisionSkin = 1f / TechnicalSample2DConstants.PixelsPerUnit;
        private const float ProjectionEpsilon = 0.000001f;
        private const int CastHitCapacity = 8;
        private const int ProjectionIterationCapacity = 4;

        [SerializeField] private TechnicalSample2DInput input;
        [SerializeField, Min(0f)] private float moveSpeed = TechnicalSample2DConstants.TrialMoveSpeed;
        [SerializeField] private Direction8 facingDirection = Direction8.South;

        private Rigidbody2D _body;
        private readonly RaycastHit2D[] _castHits = new RaycastHit2D[CastHitCapacity];
        private Vector2 _cachedMove;
        private Vector2 _lastFixedStepDelta;

        public Rigidbody2D Body => _body;
        public Vector2 CachedMove => _cachedMove;
        public Vector2 LastFixedStepDelta => _lastFixedStepDelta;
        public Direction8 FacingDirection => facingDirection;
        public bool IsMoving => _cachedMove.sqrMagnitude > 0.000001f;
        public float MoveSpeed => moveSpeed;

        public void Configure(TechnicalSample2DInput inputSource, float speed)
        {
            input = inputSource;
            moveSpeed = Mathf.Max(0f, speed);
            EnsureBody();
        }

        public void CacheMoveInput(Vector2 rawInput)
        {
            _cachedMove = Movement2DModel.NormalizeInput(rawInput);
            if (_cachedMove.sqrMagnitude > 0.000001f)
            {
                facingDirection = Direction8Model.FromVector(_cachedMove, facingDirection);
            }
        }

        public void SimulateFixedStep(float fixedDeltaTime)
        {
            EnsureBody();
            var requestedStep =
                Movement2DModel.CalculateStep(_cachedMove, moveSpeed, fixedDeltaTime);
            if (requestedStep.sqrMagnitude <= Mathf.Epsilon)
            {
                _lastFixedStepDelta = Vector2.zero;
                return;
            }

            _lastFixedStepDelta = ResolveCollisionStep(requestedStep);
            _body.MovePosition(_body.position + _lastFixedStepDelta);
        }

        private void Awake()
        {
            EnsureBody();
        }

        private void Reset()
        {
            EnsureBody();
        }

        private void Update()
        {
            CacheMoveInput(input == null ? Vector2.zero : input.ReadMove());
        }

        private void FixedUpdate()
        {
            SimulateFixedStep(Time.fixedDeltaTime);
        }

        private void EnsureBody()
        {
            if (_body == null)
            {
                _body = GetComponent<Rigidbody2D>();
            }

            if (_body == null)
            {
                return;
            }

            _body.gravityScale = 0f;
            _body.freezeRotation = true;
            _body.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        private Vector2 ResolveCollisionStep(Vector2 requestedStep)
        {
            var candidateStep = requestedStep;
            for (var iteration = 0;
                 iteration < ProjectionIterationCapacity;
                 iteration++)
            {
                if (!ProjectAgainstCollisionNormals(ref candidateStep))
                {
                    return candidateStep;
                }

                if (candidateStep.sqrMagnitude <= ProjectionEpsilon)
                {
                    return Vector2.zero;
                }
            }

            return ClampUnsafeFinalSweep(candidateStep);
        }

        private bool ProjectAgainstCollisionNormals(ref Vector2 candidateStep)
        {
            var candidateDistance = candidateStep.magnitude;
            if (candidateDistance <= Mathf.Epsilon)
            {
                return false;
            }

            var direction = candidateStep / candidateDistance;
            var hitCount = _body.Cast(
                direction,
                _castHits,
                candidateDistance + CollisionSkin);
            var projectedStep = candidateStep;
            var changed = false;

            for (var index = 0; index < hitCount; index++)
            {
                var hit = _castHits[index];
                if (hit.collider == null || hit.collider.isTrigger)
                {
                    continue;
                }

                var inwardCosine = -Vector2.Dot(direction, hit.normal);
                if (inwardCosine <= ProjectionEpsilon)
                {
                    continue;
                }

                var requestedInward = Mathf.Max(
                    0f,
                    -Vector2.Dot(projectedStep, hit.normal));
                var contactInward = hit.distance * inwardCosine;
                var allowedInward = Mathf.Max(0f, contactInward - CollisionSkin);
                var blockedInward = requestedInward - allowedInward;
                if (blockedInward <= ProjectionEpsilon)
                {
                    continue;
                }

                projectedStep += hit.normal * blockedInward;
                changed = true;
            }

            candidateStep = projectedStep;
            return changed;
        }

        private Vector2 ClampUnsafeFinalSweep(Vector2 candidateStep)
        {
            var candidateDistance = candidateStep.magnitude;
            if (candidateDistance <= Mathf.Epsilon)
            {
                return Vector2.zero;
            }

            var direction = candidateStep / candidateDistance;
            var hitCount = _body.Cast(direction, _castHits, candidateDistance);
            var allowedDistance = candidateDistance;

            for (var index = 0; index < hitCount; index++)
            {
                var hit = _castHits[index];
                if (hit.collider == null
                    || hit.collider.isTrigger
                    || Vector2.Dot(direction, hit.normal) >= -ProjectionEpsilon)
                {
                    continue;
                }

                allowedDistance = Mathf.Min(
                    allowedDistance,
                    Mathf.Max(0f, hit.distance - CollisionSkin));
            }

            return direction * allowedDistance;
        }

    }
}
