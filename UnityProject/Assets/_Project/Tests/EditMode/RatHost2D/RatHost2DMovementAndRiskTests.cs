using LastHost.Prototype.TechnicalSample2D;
using NUnit.Framework;
using UnityEngine;

namespace LastHost.Prototype.RatHost2D.Tests
{
    public sealed class RatHost2DMovementAndRiskTests
    {
        [Test]
        public void IdleInstinctMovesSingleLogicalRootAndExposesSameCameraTarget()
        {
            var host = CreateHost(out var body, out var movement);
            try
            {
                movement.Configure(null, null, 3f);
                movement.ConfigureInstinct(
                    Vector2.up,
                    new Vector2(-10f, 10f),
                    new Vector2(-10f, 10f),
                    turnInterval: 100f,
                    turnAngle: 45f);
                movement.CachePlayerInput(Vector2.zero);

                movement.SimulateFixedStep(0.02f);

                Assert.That(movement.Motor.LastFixedStepDelta.y, Is.GreaterThan(0f));
                Assert.That(movement.Motor.LastFixedStepDelta.x,
                    Is.EqualTo(0f).Within(0.0001f));
                Assert.That(movement.CurrentMoveDirection, Is.EqualTo(Vector2.up));
                Assert.That(movement.FollowTarget, Is.SameAs(host.transform));
                Assert.That(movement.LogicalPosition, Is.EqualTo((Vector2)host.transform.position));
                Assert.That(movement.LogicalPosition, Is.EqualTo(body.position));
                Assert.That(movement.Motor.enabled, Is.False);
            }
            finally
            {
                Object.DestroyImmediate(host);
            }
        }

        [TestCase(1f, 0f)]
        [TestCase(-1f, 0f)]
        [TestCase(0f, 1f)]
        [TestCase(0f, -1f)]
        [TestCase(1f, 1f)]
        [TestCase(-1f, 1f)]
        [TestCase(1f, -1f)]
        [TestCase(-1f, -1f)]
        public void ActiveWasdProducesExpectedNormalizedScreenDirection(float x, float y)
        {
            var host = CreateHost(out _, out var movement);
            try
            {
                movement.Configure(null, null, 3f);
                movement.ConfigureInstinct(
                    new Vector2(-x, -y),
                    new Vector2(-10f, 10f),
                    new Vector2(-10f, 10f),
                    turnInterval: 100f,
                    turnAngle: 45f);
                movement.CachePlayerInput(new Vector2(x, y));

                movement.SimulateFixedStep(0.02f);

                var expected = new Vector2(x, y).normalized;
                Assert.That(Vector2.Dot(movement.CurrentMoveDirection, expected),
                    Is.GreaterThanOrEqualTo(0.999f));
                Assert.That(movement.CurrentMoveDirection.magnitude,
                    Is.EqualTo(1f).Within(0.0001f));
            }
            finally
            {
                Object.DestroyImmediate(host);
            }
        }

        [Test]
        public void CardinalAndDiagonalWasdKeepEqualSpeedWhenConflictStateMatches()
        {
            var cardinal = RatHost2DControlAdapter.Resolve(
                Vector2.left,
                Vector2.right,
                0.35f,
                1f,
                -0.25f,
                0.35f,
                0.55f);
            var diagonalInput = new Vector2(1f, 1f).normalized;
            var diagonal = RatHost2DControlAdapter.Resolve(
                -diagonalInput,
                diagonalInput,
                0.35f,
                1f,
                -0.25f,
                0.35f,
                0.55f);

            var cardinalStep = Movement2DModel.CalculateStep(
                cardinal.MoveDirection,
                3f * cardinal.SpeedMultiplier,
                0.02f);
            var diagonalStep = Movement2DModel.CalculateStep(
                diagonal.MoveDirection,
                3f * diagonal.SpeedMultiplier,
                0.02f);

            Assert.That(diagonalStep.magnitude / cardinalStep.magnitude,
                Is.InRange(0.99f, 1.01f));
        }

        [Test]
        public void SessionTransitionStopsFurtherHostMovement()
        {
            var sessionObject = new GameObject("RatHost2DSession");
            var host = CreateHost(out var body, out var movement);
            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();
                movement.Configure(session, null, 3f);
                session.Configure(host, new GameObject("Shell"), null, movement,
                    host.GetComponents<Collider2D>());
                movement.CachePlayerInput(Vector2.right);
                movement.SimulateFixedStep(0.02f);
                var beforeTransition = body.position;

                session.ApplyContaminationExposure(100f / 12f);
                movement.CachePlayerInput(Vector2.right);
                movement.SimulateFixedStep(1f);

                Assert.That(movement.IsHostGameplayEnabled, Is.False);
                Assert.That(body.position, Is.EqualTo(beforeTransition));
                Assert.That(movement.CurrentMoveDirection, Is.EqualTo(Vector2.zero));
            }
            finally
            {
                var shell = GameObject.Find("Shell");
                if (shell != null)
                {
                    Object.DestroyImmediate(shell);
                }

                Object.DestroyImmediate(host);
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void ContaminationZoneUsesApprovedRatesAndStopsAfterTransition()
        {
            var sessionObject = new GameObject("RatHost2DSession");
            var zoneObject = new GameObject("ContaminationZone2D");
            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();
                zoneObject.AddComponent<BoxCollider2D>();
                var zone = zoneObject.AddComponent<RatHost2DContaminationZone>();
                zone.Configure(
                    session,
                    null,
                    RatHost2DSessionController.ContaminationAlertPerSecond,
                    RatHost2DSessionController.ContaminationHealthDamagePerSecond,
                    RatHost2DSessionController.ContaminationFeedbackLabel);

                Assert.That(zone.GetComponent<Collider2D>().isTrigger, Is.True);
                Assert.That(zone.ApplyExposure(1f), Is.True);
                Assert.That(session.State.ImmuneAlert.Value, Is.EqualTo(12f));
                Assert.That(session.State.HostHealth, Is.EqualTo(96f));

                zone.ApplyExposure(88f / 12f);
                var alertAtTransition = session.State.ImmuneAlert.Value;
                var healthAtTransition = session.State.HostHealth;

                Assert.That(zone.ApplyExposure(1f), Is.False);
                Assert.That(session.State.ImmuneAlert.Value, Is.EqualTo(alertAtTransition));
                Assert.That(session.State.HostHealth, Is.EqualTo(healthAtTransition));
            }
            finally
            {
                Object.DestroyImmediate(zoneObject);
                Object.DestroyImmediate(sessionObject);
            }
        }

        private static GameObject CreateHost(
            out Rigidbody2D body,
            out RatHost2DMovementController movement)
        {
            var host = new GameObject("RatHost2D");
            body = host.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
            host.AddComponent<BoxCollider2D>();
            host.AddComponent<RatHost2DController>();
            movement = host.AddComponent<RatHost2DMovementController>();
            return host;
        }
    }
}
