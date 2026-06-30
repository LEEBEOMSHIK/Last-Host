using LastHost.Prototype.Core;
using LastHost.Prototype.Host;
using LastHost.Prototype.Immune;
using LastHost.Prototype.Input;
using LastHost.Prototype.Mutations;
using LastHost.Prototype.VirusMinigame;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LastHost.Prototype.Tests.EditMode
{
    public sealed class RatHostPrototypeCoreTests
    {
        [Test]
        public void ImmuneAlert_ReachesThresholdOnce_AndCanResetForNextLoop()
        {
            var alert = new ImmuneAlertModel(100f, 10f, 25f);

            Assert.False(alert.Tick(5f, 1f));
            Assert.AreEqual(50f, alert.Value);

            Assert.False(alert.AddRiskEvent(1f));
            Assert.AreEqual(75f, alert.Value);

            Assert.True(alert.Tick(3f, 1f));
            Assert.AreEqual(100f, alert.Value);
            Assert.False(alert.Tick(1f, 1f));

            alert.ResetAfterInternalBattle(20f);

            Assert.AreEqual(20f, alert.Value);
            Assert.True(alert.Tick(8f, 1f));
        }

        [Test]
        public void MutationLoadout_AppliesPrototypeEffects()
        {
            var loadout = new MutationLoadout();

            Assert.AreEqual(1f, loadout.ImmuneAlertRateMultiplier);
            Assert.AreEqual(1f, loadout.RatSpeedMultiplier);
            Assert.False(loadout.CanUseMammalPassage);

            loadout.Apply(MutationType.Dormancy);
            loadout.Apply(MutationType.NeuralControl);
            loadout.Apply(MutationType.MammalAdaptation);

            Assert.Less(loadout.ImmuneAlertRateMultiplier, 1f);
            Assert.Greater(loadout.RatSpeedMultiplier, 1f);
            Assert.True(loadout.CanUseMammalPassage);
        }

        [Test]
        public void VirusMinigame_SuccessBeatsFailure_WhenSameFrameCollectsFinalFragmentAndTakesHit()
        {
            var minigame = new VirusMinigameModel(requiredFragments: 3, startingStability: 10f, whiteBloodCellDamage: 10f);

            Assert.AreEqual(VirusMinigameOutcome.Running, minigame.ResolveFrame(collectedFragment: true, hitByWhiteBloodCell: false));
            Assert.AreEqual(VirusMinigameOutcome.Running, minigame.ResolveFrame(collectedFragment: true, hitByWhiteBloodCell: false));

            var outcome = minigame.ResolveFrame(collectedFragment: true, hitByWhiteBloodCell: true);

            Assert.AreEqual(VirusMinigameOutcome.Success, outcome);
            Assert.AreEqual(3, minigame.CollectedFragments);
            Assert.AreEqual(0f, minigame.Stability);
        }

        [Test]
        public void VirusMinigame_RetryRestoresStabilityAndFragmentCount()
        {
            var minigame = new VirusMinigameModel(requiredFragments: 3, startingStability: 30f, whiteBloodCellDamage: 15f);

            minigame.ResolveFrame(collectedFragment: true, hitByWhiteBloodCell: true);
            minigame.ResolveFrame(collectedFragment: false, hitByWhiteBloodCell: true);

            Assert.AreEqual(0f, minigame.Stability);
            Assert.AreEqual(1, minigame.CollectedFragments);

            minigame.ResetRun();

            Assert.AreEqual(30f, minigame.Stability);
            Assert.AreEqual(0, minigame.CollectedFragments);
            Assert.AreEqual(VirusMinigameOutcome.Running, minigame.Outcome);
        }

        [Test]
        public void Session_ImmuneThresholdMovesFromRatModeToVirusMode()
        {
            var session = new PrototypeSessionState();

            Assert.AreEqual(PrototypeGameMode.RatHost, session.Mode);

            session.TickRatMode(100f);

            Assert.AreEqual(PrototypeGameMode.InternalVirus, session.Mode);
            Assert.AreEqual(0, session.VirusRun.CollectedFragments);
            Assert.AreEqual(session.Config.VirusStartingStability, session.VirusRun.Stability);
        }

        [Test]
        public void Session_SuccessThenMutationSelection_ReturnsToRatModeWithMutationEffect()
        {
            var session = new PrototypeSessionState();

            session.EnterVirusMinigame();
            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: false);
            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: false);
            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: false);

            Assert.AreEqual(PrototypeGameMode.MutationSelection, session.Mode);

            session.SelectMutation(MutationType.MammalAdaptation);

            Assert.AreEqual(PrototypeGameMode.RatHost, session.Mode);
            Assert.True(session.Mutations.CanUseMammalPassage);
            Assert.AreEqual(session.Config.AlertAfterMutationReturn, session.ImmuneAlert.Value);
        }

        [Test]
        public void Session_FailedVirusRunCanRetryWithoutLeavingVirusMode()
        {
            var session = new PrototypeSessionState();

            session.EnterVirusMinigame();
            session.ResolveVirusFrame(collectedFragment: false, hitByWhiteBloodCell: true);
            session.ResolveVirusFrame(collectedFragment: false, hitByWhiteBloodCell: true);
            session.ResolveVirusFrame(collectedFragment: false, hitByWhiteBloodCell: true);

            Assert.AreEqual(PrototypeGameMode.VirusFailed, session.Mode);

            session.RetryVirusMinigame();

            Assert.AreEqual(PrototypeGameMode.InternalVirus, session.Mode);
            Assert.AreEqual(0, session.VirusRun.CollectedFragments);
            Assert.AreEqual(session.Config.VirusStartingStability, session.VirusRun.Stability);
        }

        [Test]
        public void PrototypeKeyboardInput_ComposesNormalizedMovement()
        {
            var input = new PrototypeInputState
            {
                MoveRight = true,
                MoveUp = true
            };

            var move = PrototypeKeyboardInput.ComposeMoveInput(input);

            Assert.Greater(move.x, 0f);
            Assert.Greater(move.y, 0f);
            Assert.AreEqual(1f, move.magnitude, 0.001f);
        }

        [Test]
        public void PrototypeKeyboardInput_MapsMutationSelectionKeys()
        {
            Assert.True(PrototypeKeyboardInput.TryGetSelectedMutation(new PrototypeInputState { SelectMutation1 = true }, out var first));
            Assert.AreEqual(MutationType.Dormancy, first);

            Assert.True(PrototypeKeyboardInput.TryGetSelectedMutation(new PrototypeInputState { SelectMutation2 = true }, out var second));
            Assert.AreEqual(MutationType.NeuralControl, second);

            Assert.True(PrototypeKeyboardInput.TryGetSelectedMutation(new PrototypeInputState { SelectMutation3 = true }, out var third));
            Assert.AreEqual(MutationType.MammalAdaptation, third);
        }

        [Test]
        public void RatHostPrototypeScene_StartsWithRatVisibleToCamera()
        {
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/RatHostPrototype.unity");

            var camera = Object.FindFirstObjectByType<Camera>();
            var rat = Object.FindFirstObjectByType<RatHostController>(FindObjectsInactive.Include);

            Assert.NotNull(camera);
            Assert.NotNull(rat);

            var ratRenderer = rat.GetComponentInChildren<Renderer>(true);

            Assert.NotNull(ratRenderer);
            Assert.True(ratRenderer.enabled);
            Assert.True(ratRenderer.gameObject.activeInHierarchy);

            var viewport = camera.WorldToViewportPoint(ratRenderer.bounds.center);

            Assert.Greater(viewport.z, camera.nearClipPlane);
            Assert.GreaterOrEqual(viewport.x, 0.08f);
            Assert.LessOrEqual(viewport.x, 0.92f);
            Assert.GreaterOrEqual(viewport.y, 0.08f);
            Assert.LessOrEqual(viewport.y, 0.92f);
        }
    }
}
