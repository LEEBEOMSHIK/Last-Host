using NUnit.Framework;
using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D.Tests
{
    public sealed class PhysicsCameraAndSort2DTests
    {
        private const float FixedDeltaTime = 0.02f;
        private const float MoveSpeed = 3f;

        [TestCase("Wall")]
        [TestCase("WaterChannel")]
        public void E08_RigidbodyMovePositionDoesNotPenetrateBlockingCollider(string obstacleName)
        {
            var originalSimulationMode = Physics2D.simulationMode;
            var actor = new GameObject("RatHost2D");
            var obstacle = new GameObject(obstacleName);

            try
            {
                Physics2D.simulationMode = SimulationMode2D.Script;

                var body = actor.AddComponent<Rigidbody2D>();
                body.bodyType = RigidbodyType2D.Dynamic;
                body.gravityScale = 0f;
                body.freezeRotation = true;
                body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                var actorCollider = actor.AddComponent<BoxCollider2D>();
                actorCollider.size = Vector2.one;
                var controller = actor.AddComponent<RatHost2DController>();
                controller.Configure(null, 3f);
                controller.CacheMoveInput(Vector2.right);

                obstacle.transform.position = new Vector3(1.2f, 0f, 0f);
                var obstacleCollider = obstacle.AddComponent<BoxCollider2D>();
                obstacleCollider.size = Vector2.one;

                Physics2D.SyncTransforms();
                var positionAfterSixtySteps = Vector2.zero;

                for (var index = 0; index < 120; index++)
                {
                    controller.SimulateFixedStep(0.02f);
                    Assert.That(Physics2D.Simulate(0.02f), Is.True);
                    if (index == 59)
                    {
                        positionAfterSixtySteps = body.position;
                    }
                }

                Physics2D.SyncTransforms();
                var signedDistance = Physics2D.Distance(actorCollider, obstacleCollider).distance;
                var finalNormalMovement = Mathf.Abs(body.position.x - positionAfterSixtySteps.x);

                Assert.That(signedDistance, Is.GreaterThanOrEqualTo(-0.001f));
                Assert.That(finalNormalMovement, Is.LessThanOrEqualTo(1f / 64f));
            }
            finally
            {
                Physics2D.simulationMode = originalSimulationMode;
                Object.DestroyImmediate(actor);
                Object.DestroyImmediate(obstacle);
            }
        }

        [Test]
        public void C1_DiagonalInputSlidesAlongFlatWallWithoutPenetrating()
        {
            var originalSimulationMode = Physics2D.simulationMode;
            var actor = CreateActor(Vector2.zero, out var body, out var actorCollider,
                out var controller);
            var wall = CreateObstacle("FlatWall", new Vector2(0f, -0.65f),
                new Vector2(8f, 0.2f), out var wallCollider);

            try
            {
                Physics2D.simulationMode = SimulationMode2D.Script;
                controller.CacheMoveInput(new Vector2(1f, -1f));
                Physics2D.SyncTransforms();

                Simulate(controller, 30);

                Assert.That(body.position.x, Is.GreaterThan(1f));
                Assert.That(body.position.y, Is.GreaterThan(-0.06f));
                Assert.That(Physics2D.Distance(actorCollider, wallCollider).distance,
                    Is.GreaterThanOrEqualTo(-0.001f));
                Assert.That(controller.IsMoving, Is.True);
            }
            finally
            {
                Physics2D.simulationMode = originalSimulationMode;
                Object.DestroyImmediate(actor);
                Object.DestroyImmediate(wall);
            }
        }

        [Test]
        public void C2_FrontInputStopsAtFlatWall()
        {
            var originalSimulationMode = Physics2D.simulationMode;
            var actor = CreateActor(Vector2.zero, out var body, out var actorCollider,
                out var controller);
            var wall = CreateObstacle("FlatWall", new Vector2(0f, -0.65f),
                new Vector2(8f, 0.2f), out var wallCollider);

            try
            {
                Physics2D.simulationMode = SimulationMode2D.Script;
                controller.CacheMoveInput(Vector2.down);
                Physics2D.SyncTransforms();

                Simulate(controller, 30);
                var settledPosition = body.position;
                Simulate(controller, 30);

                Assert.That(Mathf.Abs(body.position.x), Is.LessThanOrEqualTo(0.0001f));
                Assert.That(Vector2.Distance(settledPosition, body.position),
                    Is.LessThanOrEqualTo(1f / 64f));
                Assert.That(Physics2D.Distance(actorCollider, wallCollider).distance,
                    Is.GreaterThanOrEqualTo(-0.001f));
            }
            finally
            {
                Physics2D.simulationMode = originalSimulationMode;
                Object.DestroyImmediate(actor);
                Object.DestroyImmediate(wall);
            }
        }

        [Test]
        public void C3_DiagonalInputStopsAtTrueCornerWithoutPenetrating()
        {
            var originalSimulationMode = Physics2D.simulationMode;
            var actor = CreateActor(Vector2.zero, out var body, out var actorCollider,
                out var controller);
            var floor = CreateObstacle("CornerFloor", new Vector2(0f, -0.65f),
                new Vector2(8f, 0.2f), out var floorCollider);
            var side = CreateObstacle("CornerSide", new Vector2(0.65f, 0f),
                new Vector2(0.2f, 8f), out var sideCollider);

            try
            {
                Physics2D.simulationMode = SimulationMode2D.Script;
                controller.CacheMoveInput(new Vector2(1f, -1f));
                Physics2D.SyncTransforms();

                Simulate(controller, 30);
                var settledPosition = body.position;
                Simulate(controller, 30);

                Assert.That(Vector2.Distance(settledPosition, body.position),
                    Is.LessThanOrEqualTo(1f / 64f));
                Assert.That(Physics2D.Distance(actorCollider, floorCollider).distance,
                    Is.GreaterThanOrEqualTo(-0.001f));
                Assert.That(Physics2D.Distance(actorCollider, sideCollider).distance,
                    Is.GreaterThanOrEqualTo(-0.001f));
            }
            finally
            {
                Physics2D.simulationMode = originalSimulationMode;
                Object.DestroyImmediate(actor);
                Object.DestroyImmediate(floor);
                Object.DestroyImmediate(side);
            }
        }

        [Test]
        public void C4_LeftAndRightSurfaceSlidesAreSymmetric()
        {
            var rightPosition = SimulateFlatWallSlide(1f);
            var leftPosition = SimulateFlatWallSlide(-1f);

            Assert.That(rightPosition.x, Is.EqualTo(-leftPosition.x).Within(0.001f));
            Assert.That(rightPosition.y, Is.EqualTo(leftPosition.y).Within(0.001f));
        }

        [Test]
        public void C5_UnobstructedAndIdleStepsPreserveSpeedContract()
        {
            var originalSimulationMode = Physics2D.simulationMode;
            var actor = CreateActor(Vector2.zero, out var body, out _, out var controller);

            try
            {
                Physics2D.simulationMode = SimulationMode2D.Script;
                controller.CacheMoveInput(Vector2.one);
                controller.SimulateFixedStep(FixedDeltaTime);
                Assert.That(Physics2D.Simulate(FixedDeltaTime), Is.True);

                Assert.That(controller.LastFixedStepDelta.magnitude,
                    Is.EqualTo(MoveSpeed * FixedDeltaTime).Within(0.0001f));
                var movingPosition = body.position;

                controller.CacheMoveInput(Vector2.zero);
                controller.SimulateFixedStep(FixedDeltaTime);
                Assert.That(Physics2D.Simulate(FixedDeltaTime), Is.True);

                Assert.That(controller.LastFixedStepDelta, Is.EqualTo(Vector2.zero));
                Assert.That(body.position, Is.EqualTo(movingPosition));
            }
            finally
            {
                Physics2D.simulationMode = originalSimulationMode;
                Object.DestroyImmediate(actor);
            }
        }

        [Test]
        public void E09_CameraImmediatelyFollowsAbsoluteSnappedTargetForThreeHundredSamples()
        {
            var target = new GameObject("RatHost2D");
            var cameraObject = new GameObject("Main Camera");

            try
            {
                cameraObject.transform.position = new Vector3(0f, 0f, -10f);
                cameraObject.AddComponent<Camera>();
                var follow = cameraObject.AddComponent<PixelFollowCamera2D>();
                follow.Configure(target.transform, 64, TechnicalSample2DConstants.TrialOrthographicSize);

                for (var index = 0; index < 300; index++)
                {
                    target.transform.position = new Vector3(
                        (index * 0.0137f) - 1.5f,
                        Mathf.Sin(index * 0.13f) * 2f,
                        0f);
                    follow.ApplyFollow();

                    Assert.That(Mathf.Abs(follow.LogicalPixelError.x), Is.LessThanOrEqualTo(0.5f));
                    Assert.That(Mathf.Abs(follow.LogicalPixelError.y), Is.LessThanOrEqualTo(0.5f));
                    Assert.That(follow.WorldCenterError.magnitude, Is.LessThanOrEqualTo(0.0112f));
                    Assert.That(cameraObject.transform.position.z, Is.EqualTo(-10f));
                }
            }
            finally
            {
                Object.DestroyImmediate(cameraObject);
                Object.DestroyImmediate(target);
            }
        }

        [Test]
        public void E10_YSortUsesFootYChangesOnceAtCrossingAndRemainsStable()
        {
            var lowerOrderObject = new GameObject("Prop");
            var actor = new GameObject("RatHost2D");
            var foot = new GameObject("FootPoint");

            try
            {
                foot.transform.SetParent(actor.transform, false);
                var renderer = actor.AddComponent<SpriteRenderer>();
                var ySort = actor.AddComponent<YSortSprite2D>();
                ySort.Configure(foot.transform, renderer);

                lowerOrderObject.transform.position = new Vector3(0f, 0f, 0f);
                var propOrder = YSortOrder2D.Calculate(lowerOrderObject.transform.position.y);

                foot.transform.position = new Vector3(0f, 1f / 64f, 0f);
                var behindOrder = ySort.ApplySorting();
                foot.transform.position = new Vector3(0f, -1f / 64f, 0f);
                var inFrontOrder = ySort.ApplySorting();

                Assert.That(behindOrder, Is.LessThan(propOrder));
                Assert.That(inFrontOrder, Is.GreaterThan(propOrder));

                var orderChanges = 0;
                var previousOrder = inFrontOrder;
                for (var index = 0; index < 300; index++)
                {
                    var currentOrder = ySort.ApplySorting();
                    if (currentOrder != previousOrder)
                    {
                        orderChanges++;
                    }

                    previousOrder = currentOrder;
                }

                Assert.That(orderChanges, Is.Zero);
            }
            finally
            {
                Object.DestroyImmediate(actor);
                Object.DestroyImmediate(lowerOrderObject);
            }
        }

        [Test]
        public void ControllerConfiguresZeroGravityAndFrozenRotation()
        {
            var actor = new GameObject("RatHost2D");

            try
            {
                var body = actor.AddComponent<Rigidbody2D>();
                body.gravityScale = 4f;
                body.freezeRotation = false;
                actor.AddComponent<RatHost2DController>();

                Assert.That(body.gravityScale, Is.Zero);
                Assert.That(body.freezeRotation, Is.True);
            }
            finally
            {
                Object.DestroyImmediate(actor);
            }
        }

        private static GameObject CreateActor(
            Vector2 position,
            out Rigidbody2D body,
            out BoxCollider2D actorCollider,
            out RatHost2DController controller)
        {
            var actor = new GameObject("RatHost2D");
            actor.transform.position = position;
            body = actor.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Dynamic;
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            actorCollider = actor.AddComponent<BoxCollider2D>();
            actorCollider.size = Vector2.one;
            controller = actor.AddComponent<RatHost2DController>();
            controller.Configure(null, MoveSpeed);
            return actor;
        }

        private static GameObject CreateObstacle(
            string name,
            Vector2 position,
            Vector2 size,
            out BoxCollider2D obstacleCollider)
        {
            var obstacle = new GameObject(name);
            obstacle.transform.position = position;
            obstacleCollider = obstacle.AddComponent<BoxCollider2D>();
            obstacleCollider.size = size;
            return obstacle;
        }

        private static void Simulate(RatHost2DController controller, int stepCount)
        {
            for (var index = 0; index < stepCount; index++)
            {
                controller.SimulateFixedStep(FixedDeltaTime);
                Assert.That(Physics2D.Simulate(FixedDeltaTime), Is.True);
            }
        }

        private static Vector2 SimulateFlatWallSlide(float horizontalInput)
        {
            var originalSimulationMode = Physics2D.simulationMode;
            var actor = CreateActor(Vector2.zero, out var body, out _, out var controller);
            var wall = CreateObstacle("FlatWall", new Vector2(0f, -0.65f),
                new Vector2(8f, 0.2f), out _);

            try
            {
                Physics2D.simulationMode = SimulationMode2D.Script;
                controller.CacheMoveInput(new Vector2(horizontalInput, -1f));
                Physics2D.SyncTransforms();
                Simulate(controller, 30);
                return body.position;
            }
            finally
            {
                Physics2D.simulationMode = originalSimulationMode;
                Object.DestroyImmediate(actor);
                Object.DestroyImmediate(wall);
            }
        }
    }
}
