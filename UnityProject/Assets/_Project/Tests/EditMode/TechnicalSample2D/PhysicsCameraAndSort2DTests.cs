using NUnit.Framework;
using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D.Tests
{
    public sealed class PhysicsCameraAndSort2DTests
    {
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
    }
}
