using LastHost.Prototype.Core;
using LastHost.Prototype.TechnicalSample2D;
using NUnit.Framework;
using UnityEngine;

namespace LastHost.Prototype.RatHost2D.Tests
{
    public sealed class RatHost2DStage2RuntimeTests
    {
        [Test]
        public void VirusUsesOneLogicalRootAndTechnicalCollisionMotor()
        {
            var virus = CreateVirus(Vector2.zero, out var movement);
            try
            {
                movement.Configure(null, null, 3f);
                movement.SetVirusGameplayEnabled(true);
                movement.CachePlayerInput(Vector2.right);
                movement.SimulateFixedStep(0.02f);

                Assert.That(movement.Motor.LastFixedStepDelta.x, Is.GreaterThan(0f));
                Assert.That(movement.Motor.LastFixedStepDelta.y,
                    Is.EqualTo(0f).Within(0.0001f));
                Assert.That(movement.FollowTarget, Is.SameAs(virus.transform));
                Assert.That(movement.LogicalPosition,
                    Is.EqualTo((Vector2)virus.transform.position));
                Assert.That(movement.Motor.enabled, Is.False);
            }
            finally
            {
                Object.DestroyImmediate(virus);
            }
        }

        [Test]
        public void ContactCooldownRejectsRepeatedStayUntilWindowExpires()
        {
            var gate = new RatHost2DContactCooldownGate();

            Assert.That(gate.TryConsume(0f, 0.5f), Is.True);
            Assert.That(gate.TryConsume(0.1f, 0.5f), Is.False);
            Assert.That(gate.TryConsume(0.49f, 0.5f), Is.False);
            Assert.That(gate.TryConsume(0.5f, 0.5f), Is.True);

            gate.Reset();
            Assert.That(gate.TryConsume(0f, 0.5f), Is.True);
        }

        [Test]
        public void FragmentCanQueueOnlyOncePerRun()
        {
            var sessionObject = new GameObject("Session");
            var fragmentObject = new GameObject("Fragment");
            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();
                var fragment = fragmentObject.AddComponent<RatHost2DMutationFragment>();
                fragment.Configure(session, 2);
                EnterInternal(session);

                Assert.That(fragment.TryCollect(), Is.True);
                Assert.That(fragment.TryCollect(), Is.False);
                session.FlushQueuedVirusFrame();

                Assert.That(fragment.IsCollected, Is.True);
                Assert.That(fragment.FragmentIndex, Is.EqualTo(2));
                Assert.That(session.State.VirusRun.CollectedFragments, Is.EqualTo(1));

                fragment.ResetRun();
                Assert.That(fragment.IsCollected, Is.False);
                Assert.That(fragmentObject.activeSelf, Is.True);
            }
            finally
            {
                Object.DestroyImmediate(fragmentObject);
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void FailureReturnAndNextEntryResetAllRuntimeObjects()
        {
            var sessionObject = new GameObject("Session");
            var virus = CreateVirus(new Vector2(1f, 1f), out var virusMovement);
            var whiteBloodCellObject = new GameObject("WhiteBloodCell");
            whiteBloodCellObject.transform.position = new Vector3(-2f, 0.5f, 0f);
            var fragmentObject = new GameObject("Fragment");

            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();
                var whiteBloodCellBody =
                    whiteBloodCellObject.AddComponent<Rigidbody2D>();
                whiteBloodCellBody.bodyType = RigidbodyType2D.Kinematic;
                var whiteBloodCell =
                    whiteBloodCellObject.AddComponent<RatHost2DWhiteBloodCellChaser>();
                var fragment =
                    fragmentObject.AddComponent<RatHost2DMutationFragment>();

                virusMovement.Configure(session, null, 3f);
                whiteBloodCell.Configure(session, virusMovement, 1.8f, 0.5f);
                fragment.Configure(session, 0);
                session.ConfigureStage2(
                    null,
                    null,
                    null,
                    null,
                    virusMovement,
                    new[] { whiteBloodCell },
                    new[] { fragment },
                    virus.GetComponents<Collider2D>());

                EnterInternal(session);
                fragment.TryCollect();
                session.FlushQueuedVirusFrame();
                virusMovement.Body.position = new Vector2(4f, 4f);
                whiteBloodCell.Body.position = new Vector2(3f, 3f);

                session.ResolveVirusFrameNow(false, true);
                session.ResolveVirusFrameNow(false, true);
                session.ResolveVirusFrameNow(false, true);
                session.ProcessFailureConfirmationInput(true);
                session.ApplyContaminationExposure(40f / 12f);

                Assert.That(session.CurrentMode,
                    Is.EqualTo(PrototypeGameMode.InternalVirus));
                Assert.That(session.State.VirusRun.Stability, Is.EqualTo(100f));
                Assert.That(session.State.VirusRun.CollectedFragments, Is.Zero);
                Assert.That(fragment.IsCollected, Is.False);
                Assert.That(fragmentObject.activeSelf, Is.True);
                Assert.That(virusMovement.Body.position,
                    Is.EqualTo(new Vector2(1f, 1f)));
                Assert.That(whiteBloodCell.Body.position,
                    Is.EqualTo(new Vector2(-2f, 0.5f)));
            }
            finally
            {
                Object.DestroyImmediate(fragmentObject);
                Object.DestroyImmediate(whiteBloodCellObject);
                Object.DestroyImmediate(virus);
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void WhiteBloodCellCooldownQueuesOneDamagePerWindow()
        {
            var sessionObject = new GameObject("Session");
            var virus = CreateVirus(Vector2.zero, out var virusMovement);
            var whiteBloodCellObject = new GameObject("WhiteBloodCell");
            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();
                whiteBloodCellObject.AddComponent<Rigidbody2D>()
                    .bodyType = RigidbodyType2D.Kinematic;
                var whiteBloodCell =
                    whiteBloodCellObject.AddComponent<RatHost2DWhiteBloodCellChaser>();
                virusMovement.Configure(session, null, 3f);
                whiteBloodCell.Configure(session, virusMovement, 1.8f, 0.5f);
                session.ConfigureStage2(
                    null,
                    null,
                    null,
                    null,
                    virusMovement,
                    new[] { whiteBloodCell },
                    null,
                    virus.GetComponents<Collider2D>());
                EnterInternal(session);

                Assert.That(whiteBloodCell.TryApplyContact(0f), Is.True);
                Assert.That(whiteBloodCell.TryApplyContact(0.1f), Is.False);
                session.FlushQueuedVirusFrame();

                Assert.That(session.State.VirusRun.Stability, Is.EqualTo(66f));
                Assert.That(session.State.VirusPatternExposureTotal, Is.EqualTo(8f));

                Assert.That(whiteBloodCell.TryApplyContact(0.5f), Is.True);
                session.FlushQueuedVirusFrame();
                Assert.That(session.State.VirusRun.Stability, Is.EqualTo(32f));
                Assert.That(session.State.VirusPatternExposureTotal, Is.EqualTo(16f));
            }
            finally
            {
                Object.DestroyImmediate(whiteBloodCellObject);
                Object.DestroyImmediate(virus);
                Object.DestroyImmediate(sessionObject);
            }
        }

        private static void EnterInternal(RatHost2DSessionController session)
        {
            session.ApplyContaminationExposure(100f / 12f);
            Assert.That(session.CanProcessVirusGameplay, Is.True);
        }

        private static GameObject CreateVirus(
            Vector2 position,
            out RatHost2DVirusMovementController movement)
        {
            var virus = new GameObject("Virus");
            virus.transform.position = position;
            virus.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            virus.AddComponent<BoxCollider2D>();
            virus.AddComponent<RatHost2DController>();
            movement = virus.AddComponent<RatHost2DVirusMovementController>();
            return virus;
        }
    }
}
