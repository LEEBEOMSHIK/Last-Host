using LastHost.Prototype.Core;
using LastHost.Prototype.Input;
using LastHost.Prototype.Mutations;
using LastHost.Prototype.TechnicalSample2D;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace LastHost.Prototype.RatHost2D.Tests
{
    public sealed class RatHost2DStage3MutationTests
    {
        [Test]
        public void SuccessfulSelectionReturnsToHostOnceWithCapturedExposure()
        {
            var sessionObject = new GameObject("Session");
            var hostRoot = new GameObject("HostRoot");
            var internalRoot = new GameObject("InternalRoot");
            var hostCamera = new GameObject("HostCamera");
            var internalCamera = new GameObject("InternalCamera");
            var mutationRoot = new GameObject("MutationRoot");
            var hostColliderObject = new GameObject("HostCollider");
            var internalColliderObject = new GameObject("InternalCollider");

            try
            {
                var session =
                    sessionObject.AddComponent<RatHost2DSessionController>();
                var hostCollider =
                    hostColliderObject.AddComponent<BoxCollider2D>();
                var internalCollider =
                    internalColliderObject.AddComponent<BoxCollider2D>();
                session.Configure(
                    hostRoot,
                    internalRoot,
                    null,
                    null,
                    new Collider2D[] { hostCollider });
                session.ConfigureStage2(
                    hostCamera,
                    internalCamera,
                    null,
                    mutationRoot,
                    null,
                    null,
                    null,
                    new Collider2D[] { internalCollider });

                EnterSuccessfulMutationSelection(session, hitCount: 1);

                Assert.That(session.TrySelectMutation(MutationType.Dormancy),
                    Is.True);
                Assert.That(session.CurrentMode,
                    Is.EqualTo(PrototypeGameMode.RatHost));
                Assert.That(session.State.ImmuneAlert.Value,
                    Is.EqualTo(33f).Within(0.0001f));
                Assert.That(session.State.VirusPatternExposureTotal, Is.Zero);
                Assert.That(session.State.VirusRun.CollectedFragments, Is.Zero);
                Assert.That(session.State.VirusRun.Stability, Is.EqualTo(100f));
                Assert.That(hostRoot.activeSelf, Is.True);
                Assert.That(hostCamera.activeSelf, Is.True);
                Assert.That(hostCollider.enabled, Is.True);
                Assert.That(internalRoot.activeSelf, Is.False);
                Assert.That(internalCamera.activeSelf, Is.False);
                Assert.That(internalCollider.enabled, Is.False);
                Assert.That(mutationRoot.activeSelf, Is.False);

                Assert.That(
                    session.TrySelectMutation(MutationType.NeuralControl),
                    Is.False);
                Assert.That(
                    session.State.Mutations.Has(MutationType.Dormancy),
                    Is.True);
                Assert.That(
                    session.State.Mutations.Has(MutationType.NeuralControl),
                    Is.False);
            }
            finally
            {
                Object.DestroyImmediate(internalColliderObject);
                Object.DestroyImmediate(hostColliderObject);
                Object.DestroyImmediate(mutationRoot);
                Object.DestroyImmediate(internalCamera);
                Object.DestroyImmediate(hostCamera);
                Object.DestroyImmediate(internalRoot);
                Object.DestroyImmediate(hostRoot);
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void SelectionInputChoosesOnlyFirstMappedMutation()
        {
            var sessionObject = new GameObject("Session");
            try
            {
                var session =
                    sessionObject.AddComponent<RatHost2DSessionController>();
                EnterSuccessfulMutationSelection(session, hitCount: 0);
                var simultaneousInput = new PrototypeInputState
                {
                    SelectMutation1 = true,
                    SelectMutation2 = true,
                    SelectMutation3 = true
                };

                Assert.That(
                    session.ProcessMutationSelectionInput(simultaneousInput),
                    Is.True);
                Assert.That(
                    session.ProcessMutationSelectionInput(simultaneousInput),
                    Is.False);
                Assert.That(
                    session.State.Mutations.Has(MutationType.Dormancy),
                    Is.True);
                Assert.That(
                    session.State.Mutations.Has(MutationType.NeuralControl),
                    Is.False);
                Assert.That(
                    session.State.Mutations.Has(
                        MutationType.MammalAdaptation),
                    Is.False);
            }
            finally
            {
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void DormancyScalesContaminationAlertButNotDamageOrIdleTime()
        {
            var sessionObject = new GameObject("Session");
            try
            {
                var session =
                    sessionObject.AddComponent<RatHost2DSessionController>();
                EnterSuccessfulMutationSelection(session, hitCount: 0);
                Assert.That(session.TrySelectMutation(MutationType.Dormancy),
                    Is.True);

                var alertBeforeExposure = session.State.ImmuneAlert.Value;
                var healthBeforeExposure = session.State.HostHealth;
                session.TickHostMode(10f);

                Assert.That(session.State.ImmuneAlert.Value,
                    Is.EqualTo(alertBeforeExposure).Within(0.0001f));
                Assert.That(session.ApplyContaminationExposure(1f), Is.True);
                Assert.That(session.State.ImmuneAlert.Value,
                    Is.EqualTo(alertBeforeExposure + 6.6f).Within(0.0001f));
                Assert.That(session.State.HostHealth,
                    Is.EqualTo(healthBeforeExposure - 4f).Within(0.0001f));
                Assert.That(session.State.LastImmuneAlertFeedbackDelta,
                    Is.EqualTo(6.6f).Within(0.0001f));
            }
            finally
            {
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void NeuralControlChangesActual2DControlAndMovementStep()
        {
            var sessionObject = new GameObject("Session");
            var host = CreateHost(out var movement);
            try
            {
                var session =
                    sessionObject.AddComponent<RatHost2DSessionController>();
                movement.Configure(session, null, 3f);
                movement.ConfigureInstinct(
                    Vector2.left,
                    new Vector2(-10f, 10f),
                    new Vector2(-10f, 10f),
                    turnInterval: 100f,
                    turnAngle: 45f);
                EnterSuccessfulMutationSelection(session, hitCount: 0);
                Assert.That(
                    session.TrySelectMutation(MutationType.NeuralControl),
                    Is.True);

                movement.CachePlayerInput(Vector2.right);
                movement.SimulateFixedStep(0.02f);

                Assert.That(session.State.Mutations.RatControlPower,
                    Is.EqualTo(1.1f));
                Assert.That(session.State.Mutations.RatSpeedMultiplier,
                    Is.EqualTo(1.35f));
                Assert.That(movement.LastControlFrame.ControlRatio,
                    Is.EqualTo(1f));
                Assert.That(movement.LastControlFrame.IsForcedControl,
                    Is.False);
                Assert.That(movement.CurrentMoveDirection,
                    Is.EqualTo(Vector2.right));
                Assert.That(movement.Motor.MoveSpeed,
                    Is.EqualTo(4.05f).Within(0.0001f));
                Assert.That(movement.Motor.LastFixedStepDelta.magnitude,
                    Is.EqualTo(0.081f).Within(0.0001f));
            }
            finally
            {
                Object.DestroyImmediate(host);
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void MammalAdaptationOpensOnlyConfiguredPassageCollider()
        {
            var sessionObject = new GameObject("Session");
            var passageObject = new GameObject("MammalPassage");
            var unrelatedWallObject = new GameObject("UnrelatedWall");
            try
            {
                var session =
                    sessionObject.AddComponent<RatHost2DSessionController>();
                var passageCollider =
                    passageObject.AddComponent<BoxCollider2D>();
                var passageRenderer =
                    passageObject.AddComponent<SpriteRenderer>();
                var unrelatedWall =
                    unrelatedWallObject.AddComponent<BoxCollider2D>();
                var gate =
                    passageObject.AddComponent<RatHost2DMammalPassageGate>();
                gate.Configure(session, passageCollider, passageRenderer);

                Assert.That(gate.IsOpen, Is.False);
                Assert.That(passageCollider.enabled, Is.True);
                Assert.That(unrelatedWall.enabled, Is.True);

                EnterSuccessfulMutationSelection(session, hitCount: 0);
                Assert.That(
                    session.TrySelectMutation(MutationType.MammalAdaptation),
                    Is.True);
                gate.RefreshNow();

                Assert.That(session.CanUseMammalPassage, Is.True);
                Assert.That(gate.IsOpen, Is.True);
                Assert.That(passageCollider.enabled, Is.False);
                Assert.That(unrelatedWall.enabled, Is.True);
            }
            finally
            {
                Object.DestroyImmediate(unrelatedWallObject);
                Object.DestroyImmediate(passageObject);
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void MutationButtonAndStatusDisplayUseStage3SessionContract()
        {
            var sessionObject = new GameObject("Session");
            var buttonObject = new GameObject("DormancyButton");
            var labelObject = new GameObject("ButtonLabel");
            var statusObject = new GameObject("Status");
            var statusTextObject = new GameObject("StatusText");
            try
            {
                var session =
                    sessionObject.AddComponent<RatHost2DSessionController>();
                buttonObject.AddComponent<Image>();
                buttonObject.AddComponent<Button>();
                var label = labelObject.AddComponent<Text>();
                var option =
                    buttonObject.AddComponent<RatHost2DMutationOptionButton>();
                var statusText = statusTextObject.AddComponent<Text>();
                var status =
                    statusObject.AddComponent<RatHost2DMutationStatusDisplay>();
                option.Configure(session, MutationType.Dormancy, label);
                status.Configure(session, statusText);

                Assert.That(label.text, Does.Contain("잠복 강화"));
                Assert.That(statusText.text, Is.EqualTo("적용 변이 없음"));
                EnterSuccessfulMutationSelection(session, hitCount: 0);

                Assert.That(option.SelectMutation(), Is.True);
                status.RefreshNow();

                Assert.That(statusText.text, Is.EqualTo("적용 변이 잠복 강화"));
                Assert.That(option.SelectMutation(), Is.False);
            }
            finally
            {
                Object.DestroyImmediate(statusTextObject);
                Object.DestroyImmediate(statusObject);
                Object.DestroyImmediate(labelObject);
                Object.DestroyImmediate(buttonObject);
                Object.DestroyImmediate(sessionObject);
            }
        }

        private static void EnterSuccessfulMutationSelection(
            RatHost2DSessionController session,
            int hitCount)
        {
            session.ApplyContaminationExposure(100f / 12f);
            Assert.That(session.CanProcessVirusGameplay, Is.True);

            for (var index = 0; index < hitCount; index++)
            {
                session.ResolveVirusFrameNow(false, true);
            }

            session.ResolveVirusFrameNow(true, false);
            session.ResolveVirusFrameNow(true, false);
            session.ResolveVirusFrameNow(true, false);
            Assert.That(session.CurrentMode,
                Is.EqualTo(PrototypeGameMode.MutationSelection));
        }

        private static GameObject CreateHost(
            out RatHost2DMovementController movement)
        {
            var host = new GameObject("RatHost2D");
            host.AddComponent<Rigidbody2D>().bodyType =
                RigidbodyType2D.Kinematic;
            host.AddComponent<BoxCollider2D>();
            host.AddComponent<RatHost2DController>();
            movement = host.AddComponent<RatHost2DMovementController>();
            return host;
        }
    }
}
