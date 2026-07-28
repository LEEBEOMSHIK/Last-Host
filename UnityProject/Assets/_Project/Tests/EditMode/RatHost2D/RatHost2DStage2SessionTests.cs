using LastHost.Prototype.Core;
using LastHost.Prototype.Mutations;
using LastHost.Prototype.TechnicalSample2D;
using LastHost.Prototype.VirusMinigame;
using NUnit.Framework;
using UnityEngine;

namespace LastHost.Prototype.RatHost2D.Tests
{
    public sealed class RatHost2DStage2SessionTests
    {
        [Test]
        public void HostAndVirusInputAreMutuallyExclusiveAcrossAllResultModes()
        {
            var sessionObject = new GameObject("Session");
            var host = CreateHost(out var hostMovement);
            var virus = CreateVirus(out var virusMovement);
            var internalRoot = new GameObject("InternalRoot");
            var failurePanel = new GameObject("FailurePanel");
            var mutationShell = new GameObject("MutationShell");

            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();
                hostMovement.Configure(session, null, 3f);
                virusMovement.Configure(session, null, 3f);
                session.Configure(
                    host,
                    internalRoot,
                    null,
                    hostMovement,
                    host.GetComponents<Collider2D>());
                session.ConfigureStage2(
                    null,
                    null,
                    failurePanel,
                    mutationShell,
                    virusMovement,
                    null,
                    null,
                    virus.GetComponents<Collider2D>());
                var mutationHandoffCount = 0;
                session.ModeChanged += mode =>
                {
                    if (mode == PrototypeGameMode.MutationSelection)
                    {
                        mutationHandoffCount++;
                    }
                };

                Assert.That(hostMovement.IsHostGameplayEnabled, Is.True);
                Assert.That(virusMovement.IsVirusGameplayEnabled, Is.False);

                EnterInternal(session);

                Assert.That(hostMovement.IsHostGameplayEnabled, Is.False);
                Assert.That(virusMovement.IsVirusGameplayEnabled, Is.True);

                CollectThreeFragments(session);
                session.ResolveVirusFrameNow(true, true);

                Assert.That(session.CurrentMode,
                    Is.EqualTo(PrototypeGameMode.MutationSelection));
                Assert.That(mutationHandoffCount, Is.EqualTo(1));
                Assert.That(hostMovement.IsHostGameplayEnabled, Is.False);
                Assert.That(virusMovement.IsVirusGameplayEnabled, Is.False);
                Assert.That(mutationShell.activeSelf, Is.True);
                Assert.That(failurePanel.activeSelf, Is.False);
            }
            finally
            {
                Object.DestroyImmediate(mutationShell);
                Object.DestroyImmediate(failurePanel);
                Object.DestroyImmediate(internalRoot);
                Object.DestroyImmediate(virus);
                Object.DestroyImmediate(host);
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void ThirdFragmentAndLethalHitInSameFrameResolveAsSuccess()
        {
            var sessionObject = new GameObject("Session");
            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();
                EnterInternal(session);
                session.QueueVirusFragmentCollected(0);
                session.FlushQueuedVirusFrame();
                session.QueueVirusFragmentCollected(1);
                session.QueueWhiteBloodCellHit();
                session.FlushQueuedVirusFrame();
                session.QueueWhiteBloodCellHit();
                session.FlushQueuedVirusFrame();

                session.QueueVirusFragmentCollected(2);
                session.QueueWhiteBloodCellHit();
                var outcome = session.FlushQueuedVirusFrame();

                Assert.That(outcome, Is.EqualTo(VirusMinigameOutcome.Success));
                Assert.That(session.CurrentMode,
                    Is.EqualTo(PrototypeGameMode.MutationSelection));
                Assert.That(session.State.VirusRun.CollectedFragments, Is.EqualTo(3));
                Assert.That(session.State.VirusRun.Stability, Is.Zero);
            }
            finally
            {
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void DistinctFragmentsInSameFrameEachCountButDuplicateIndexDoesNot()
        {
            var sessionObject = new GameObject("Session");
            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();
                EnterInternal(session);

                Assert.That(session.QueueVirusFragmentCollected(0), Is.True);
                Assert.That(session.QueueVirusFragmentCollected(1), Is.True);
                Assert.That(session.QueueVirusFragmentCollected(1), Is.False);
                session.FlushQueuedVirusFrame();

                Assert.That(session.State.VirusRun.CollectedFragments, Is.EqualTo(2));
                Assert.That(session.QueueVirusFragmentCollected(0), Is.False);
            }
            finally
            {
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void FailureWaitsForConfirmationThenReturnsAtSixtyWithoutMutation()
        {
            var sessionObject = new GameObject("Session");
            var hostRoot = new GameObject("HostRoot");
            var internalRoot = new GameObject("InternalRoot");
            var failurePanel = new GameObject("FailurePanel");
            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();
                session.Configure(hostRoot, internalRoot, null, null, null);
                session.ConfigureStage2(
                    null,
                    null,
                    failurePanel,
                    null,
                    null,
                    null,
                    null,
                    null);
                EnterInternal(session);

                session.ResolveVirusFrameNow(false, true);
                session.ResolveVirusFrameNow(false, true);
                session.ResolveVirusFrameNow(false, true);

                Assert.That(session.CurrentMode,
                    Is.EqualTo(PrototypeGameMode.VirusFailed));
                Assert.That(session.IsVirusFailureAwaitingConfirmation, Is.True);
                Assert.That(hostRoot.activeSelf, Is.False);
                Assert.That(failurePanel.activeSelf, Is.True);
                Assert.That(session.ProcessFailureConfirmationInput(false), Is.False);
                Assert.That(session.CurrentMode,
                    Is.EqualTo(PrototypeGameMode.VirusFailed));

                Assert.That(session.ProcessFailureConfirmationInput(true), Is.True);

                Assert.That(session.CurrentMode, Is.EqualTo(PrototypeGameMode.RatHost));
                Assert.That(session.State.ImmuneAlert.Value, Is.EqualTo(60f));
                Assert.That(hostRoot.activeSelf, Is.True);
                Assert.That(failurePanel.activeSelf, Is.False);
                Assert.That(session.State.Mutations.Has(MutationType.Dormancy), Is.False);
                Assert.That(session.State.Mutations.Has(MutationType.NeuralControl), Is.False);
                Assert.That(session.State.Mutations.Has(MutationType.MammalAdaptation), Is.False);
            }
            finally
            {
                Object.DestroyImmediate(failurePanel);
                Object.DestroyImmediate(internalRoot);
                Object.DestroyImmediate(hostRoot);
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void QueuedFragmentAndHitAreResolvedTogetherOnce()
        {
            var sessionObject = new GameObject("Session");
            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();
                EnterInternal(session);

                Assert.That(session.QueueVirusFragmentCollected(0), Is.True);
                Assert.That(session.QueueWhiteBloodCellHit(), Is.True);

                session.FlushQueuedVirusFrame();
                session.FlushQueuedVirusFrame();

                Assert.That(session.State.VirusRun.CollectedFragments, Is.EqualTo(1));
                Assert.That(session.State.VirusRun.Stability, Is.EqualTo(66f));
                Assert.That(session.State.LastVirusPatternExposureFeedbackText,
                    Is.EqualTo("면역 포착 +8"));
            }
            finally
            {
                Object.DestroyImmediate(sessionObject);
            }
        }

        private static void EnterInternal(RatHost2DSessionController session)
        {
            session.ApplyContaminationExposure(100f / 12f);
            Assert.That(session.CanProcessVirusGameplay, Is.True);
        }

        private static void CollectThreeFragments(RatHost2DSessionController session)
        {
            session.ResolveVirusFrameNow(true, false);
            session.ResolveVirusFrameNow(true, false);
            session.ResolveVirusFrameNow(true, false);
        }

        private static GameObject CreateHost(out RatHost2DMovementController movement)
        {
            var host = new GameObject("Host");
            host.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            host.AddComponent<BoxCollider2D>();
            host.AddComponent<RatHost2DController>();
            movement = host.AddComponent<RatHost2DMovementController>();
            return host;
        }

        private static GameObject CreateVirus(
            out RatHost2DVirusMovementController movement)
        {
            var virus = new GameObject("Virus");
            virus.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            virus.AddComponent<BoxCollider2D>();
            virus.AddComponent<RatHost2DController>();
            movement = virus.AddComponent<RatHost2DVirusMovementController>();
            return virus;
        }
    }
}
