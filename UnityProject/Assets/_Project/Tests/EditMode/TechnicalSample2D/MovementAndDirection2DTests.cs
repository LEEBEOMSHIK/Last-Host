using NUnit.Framework;
using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D.Tests
{
    public sealed class MovementAndDirection2DTests
    {
        private const float FixedDeltaTime = 0.02f;
        private const float Speed = 3f;

        [TestCase(0f, 1f, 0f, 1f)]
        [TestCase(0f, -1f, 0f, -1f)]
        [TestCase(-1f, 0f, -1f, 0f)]
        [TestCase(1f, 0f, 1f, 0f)]
        public void E02_ScreenAxisInputMatchesExpectedDirection(
            float inputX,
            float inputY,
            float expectedX,
            float expectedY)
        {
            var actual = Movement2DModel.NormalizeInput(new Vector2(inputX, inputY));
            var expected = new Vector2(expectedX, expectedY);

            Assert.That(Vector2.Dot(actual, expected), Is.GreaterThanOrEqualTo(0.999f));
            Assert.That(Mathf.Abs(Vector2.Dot(actual, new Vector2(-expected.y, expected.x))),
                Is.LessThanOrEqualTo(0.01f));
        }

        [TestCase(1f, 1f)]
        [TestCase(-1f, 1f)]
        [TestCase(1f, -1f)]
        [TestCase(-1f, -1f)]
        public void E03_DiagonalInputIsNormalized(float x, float y)
        {
            var cardinalDistance = Movement2DModel.CalculateStep(Vector2.right, Speed, 1f).magnitude;
            var diagonalDistance =
                Movement2DModel.CalculateStep(new Vector2(x, y), Speed, 1f).magnitude;

            Assert.That(Movement2DModel.NormalizeInput(new Vector2(x, y)).magnitude,
                Is.EqualTo(1f).Within(0.0001f));
            Assert.That(diagonalDistance / cardinalDistance, Is.InRange(0.99f, 1.01f));
        }

        [Test]
        public void E04_OppositeAndIdleInputsProduceZeroStep()
        {
            var horizontalOpposites = Vector2.right + Vector2.left;
            var verticalOpposites = Vector2.up + Vector2.down;

            Assert.That(Movement2DModel.CalculateStep(horizontalOpposites, Speed, FixedDeltaTime),
                Is.EqualTo(Vector2.zero));
            Assert.That(Movement2DModel.CalculateStep(verticalOpposites, Speed, FixedDeltaTime),
                Is.EqualTo(Vector2.zero));
            Assert.That(Movement2DModel.CalculateStep(Vector2.zero, Speed, FixedDeltaTime),
                Is.EqualTo(Vector2.zero));
        }

        [Test]
        public void E05_FixedStepDistanceIsEqualForCardinalAndDiagonalMovement()
        {
            var cardinalTotal = Vector2.zero;
            var diagonalTotal = Vector2.zero;
            var allowedStep = (Speed * FixedDeltaTime * 1.05f) + 0.0001f;

            for (var index = 0; index < 100; index++)
            {
                var cardinalStep =
                    Movement2DModel.CalculateStep(Vector2.right, Speed, FixedDeltaTime);
                var diagonalStep =
                    Movement2DModel.CalculateStep(Vector2.one, Speed, FixedDeltaTime);
                Assert.That(cardinalStep.magnitude, Is.LessThanOrEqualTo(allowedStep));
                Assert.That(diagonalStep.magnitude, Is.LessThanOrEqualTo(allowedStep));
                cardinalTotal += cardinalStep;
                diagonalTotal += diagonalStep;
            }

            Assert.That(diagonalTotal.magnitude / cardinalTotal.magnitude, Is.InRange(0.99f, 1.01f));
        }

        [TestCase(1f, 0f, -1f, 0f)]
        [TestCase(-1f, 0f, 1f, 0f)]
        [TestCase(0f, 1f, 0f, -1f)]
        [TestCase(0f, -1f, 0f, 1f)]
        public void E06_ReversalNeverExceedsOneAllowedStep(
            float firstX,
            float firstY,
            float secondX,
            float secondY)
        {
            var position = new Vector2(4.375f, -2.8125f);
            var allowedStep = (Speed * FixedDeltaTime * 1.05f) + 0.0001f;

            for (var index = 0; index < 30; index++)
            {
                var before = position;
                position += Movement2DModel.CalculateStep(
                    new Vector2(firstX, firstY),
                    Speed,
                    FixedDeltaTime);
                Assert.That(Vector2.Distance(before, position), Is.LessThanOrEqualTo(allowedStep));
            }

            for (var index = 0; index < 30; index++)
            {
                var before = position;
                position += Movement2DModel.CalculateStep(
                    new Vector2(secondX, secondY),
                    Speed,
                    FixedDeltaTime);
                Assert.That(Vector2.Distance(before, position), Is.LessThanOrEqualTo(allowedStep));
            }
        }

        [Test]
        public void E07_IdleForThreeHundredStepsHasNoDrift()
        {
            var start = new Vector2(11.25f, -7.5f);
            var current = start;

            for (var index = 0; index < 300; index++)
            {
                current += Movement2DModel.CalculateStep(Vector2.zero, Speed, FixedDeltaTime);
            }

            Assert.That(Vector2.Distance(start, current), Is.LessThanOrEqualTo(0.0001f));
        }

        [TestCase(0f, -1f, Direction8.South)]
        [TestCase(-1f, -1f, Direction8.SouthWest)]
        [TestCase(-1f, 0f, Direction8.West)]
        [TestCase(-1f, 1f, Direction8.NorthWest)]
        [TestCase(0f, 1f, Direction8.North)]
        [TestCase(1f, 1f, Direction8.NorthEast)]
        [TestCase(1f, 0f, Direction8.East)]
        [TestCase(1f, -1f, Direction8.SouthEast)]
        public void EightDirectionsMapToStableSpriteSlots(float x, float y, Direction8 expected)
        {
            Assert.That(Direction8Model.FromVector(new Vector2(x, y)), Is.EqualTo(expected));
        }

        [Test]
        public void IdleDirectionKeepsLastValidDirection()
        {
            Assert.That(Direction8Model.FromVector(Vector2.zero, Direction8.NorthEast),
                Is.EqualTo(Direction8.NorthEast));
        }
    }
}
