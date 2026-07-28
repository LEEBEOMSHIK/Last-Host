using NUnit.Framework;
using UnityEngine;

namespace LastHost.Prototype.RatHost2D.Tests
{
    public sealed class RatHost2DControlAdapterTests
    {
        [TestCase(1f, 0f)]
        [TestCase(-1f, 0f)]
        [TestCase(0f, 1f)]
        [TestCase(0f, -1f)]
        [TestCase(1f, 1f)]
        [TestCase(-1f, 1f)]
        [TestCase(1f, -1f)]
        [TestCase(-1f, -1f)]
        public void XYAndXZRoundTripPreservesScreenDirection(float x, float y)
        {
            var input = new Vector2(x, y).normalized;

            var roundTrip = RatHost2DControlAdapter.XZToXY(
                RatHost2DControlAdapter.XYToXZ(input));

            Assert.That(Vector2.Dot(input, roundTrip), Is.GreaterThanOrEqualTo(0.999f));
        }

        [Test]
        public void IdleUsesHostInstinctAtPassiveSpeed()
        {
            var frame = RatHost2DControlAdapter.Resolve(
                Vector2.up,
                Vector2.zero,
                virusControlPower: 0.35f,
                hostInstinctResistance: 1f,
                conflictDotThreshold: -0.25f,
                passiveInstinctSpeedMultiplier: 0.35f,
                forcedControlSpeedMultiplier: 0.55f);

            Assert.That(frame.MoveDirection, Is.EqualTo(Vector2.up));
            Assert.That(frame.SpeedMultiplier, Is.EqualTo(0.35f).Within(0.0001f));
            Assert.That(frame.IsForcedControl, Is.False);
        }

        [TestCase(1f, 0f)]
        [TestCase(-1f, 0f)]
        [TestCase(0f, 1f)]
        [TestCase(0f, -1f)]
        [TestCase(1f, 1f)]
        [TestCase(-1f, 1f)]
        [TestCase(1f, -1f)]
        [TestCase(-1f, -1f)]
        public void ActiveWasdDirectionAlwaysWinsOverInstinct(float x, float y)
        {
            var expected = new Vector2(x, y).normalized;
            var frame = RatHost2DControlAdapter.Resolve(
                -expected,
                new Vector2(x, y),
                virusControlPower: 0.35f,
                hostInstinctResistance: 1f,
                conflictDotThreshold: -0.25f,
                passiveInstinctSpeedMultiplier: 0.35f,
                forcedControlSpeedMultiplier: 0.55f);

            Assert.That(Vector2.Dot(frame.MoveDirection, expected),
                Is.GreaterThanOrEqualTo(0.999f));
            Assert.That(frame.MoveDirection.magnitude, Is.EqualTo(1f).Within(0.0001f));
        }

        [Test]
        public void OpposingInputKeepsDirectionButAppliesForcedControlPenalty()
        {
            var frame = RatHost2DControlAdapter.Resolve(
                Vector2.left,
                Vector2.right,
                virusControlPower: 0.35f,
                hostInstinctResistance: 1f,
                conflictDotThreshold: -0.25f,
                passiveInstinctSpeedMultiplier: 0.35f,
                forcedControlSpeedMultiplier: 0.55f);

            Assert.That(frame.MoveDirection, Is.EqualTo(Vector2.right));
            Assert.That(frame.IsForcedControl, Is.True);
            Assert.That(frame.SpeedMultiplier, Is.EqualTo(0.55f).Within(0.0001f));
        }

        [Test]
        public void WanderBoundaryTurnsWithinMappedVerticalBounds()
        {
            var resolved = RatHost2DControlAdapter.ResolveNextInstinctDirection(
                Vector2.up,
                Vector2.up,
                new Vector2(0f, 0.95f),
                new Vector2(-1f, 1f),
                new Vector2(-1f, 1f),
                turnRequested: false,
                turnAngleDegrees: 45f,
                turnSign: 1f);

            Assert.That(resolved.y, Is.EqualTo(0f).Within(0.0001f));
            Assert.That(Mathf.Abs(resolved.x), Is.EqualTo(1f).Within(0.0001f));
        }
    }
}
