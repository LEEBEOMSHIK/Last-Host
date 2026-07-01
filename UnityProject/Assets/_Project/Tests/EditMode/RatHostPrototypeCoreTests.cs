using LastHost.Prototype.Core;
using LastHost.Prototype.Cameras;
using LastHost.Prototype.Host;
using LastHost.Prototype.Immune;
using LastHost.Prototype.Input;
using LastHost.Prototype.Mutations;
using LastHost.Prototype.UI;
using LastHost.Prototype.VirusMinigame;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

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
        public void Session_DefaultRatModeTickDoesNotRaiseImmuneAlert()
        {
            var session = new PrototypeSessionState();

            Assert.AreEqual(PrototypeGameMode.RatHost, session.Mode);

            Assert.False(session.TickRatMode(10f));
            Assert.AreEqual(PrototypeGameMode.RatHost, session.Mode);
            Assert.AreEqual(0f, session.ImmuneAlert.Value);
        }

        [Test]
        public void Session_ConfiguredBaseAlertMovesFromRatModeToVirusMode()
        {
            var session = new PrototypeSessionState(new PrototypeConfig { BaseAlertPerSecond = 2.5f });

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
        public void Session_RatRiskInteractionAffordanceTracksPromptAndCanClear()
        {
            var session = new PrototypeSessionState();

            Assert.False(session.IsRatRiskInteractionAvailable);
            Assert.AreEqual(string.Empty, session.RatRiskInteractionPrompt);

            session.SetRatRiskInteractionAffordance(true, "소음 배관 조사 가능");

            Assert.True(session.IsRatRiskInteractionAvailable);
            Assert.AreEqual("소음 배관 조사 가능", session.RatRiskInteractionPrompt);

            session.SetRatRiskInteractionAffordance(false, "소음 배관 조사 가능");

            Assert.False(session.IsRatRiskInteractionAvailable);
            Assert.AreEqual(string.Empty, session.RatRiskInteractionPrompt);
        }

        [Test]
        public void Session_RatRiskInteractionAffordanceRequiresReadablePrompt()
        {
            var session = new PrototypeSessionState();

            Assert.False(session.SetRatRiskInteractionAffordance(true, "   "));
            Assert.False(session.IsRatRiskInteractionAvailable);
            Assert.AreEqual(string.Empty, session.RatRiskInteractionPrompt);

            Assert.True(session.SetRatRiskInteractionAffordance(true, "소음 배관 조사 가능"));
            Assert.True(session.SetRatRiskInteractionAffordance(true, null));
            Assert.False(session.IsRatRiskInteractionAvailable);
            Assert.AreEqual(string.Empty, session.RatRiskInteractionPrompt);
        }

        [Test]
        public void PrototypeHud_ShowsRiskInteractionPromptWhenRatCanInteract()
        {
            var session = new PrototypeSessionState();
            session.SetRatRiskInteractionAffordance(true, "소음 배관 조사 가능");

            var hudObject = new GameObject("HUD Under Test");
            var objectiveObject = new GameObject("Objective Text");
            objectiveObject.transform.SetParent(hudObject.transform, false);
            var objectiveText = objectiveObject.AddComponent<Text>();
            var hud = hudObject.AddComponent<PrototypeHud>();
            hud.objectiveText = objectiveText;

            hud.Refresh(session);

            Assert.AreEqual("소음 배관 조사 가능", objectiveText.text);

            Object.DestroyImmediate(hudObject);
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
        public void PrototypeKeyboardInput_MapsCameraToggleKey()
        {
            var input = new PrototypeInputState
            {
                ToggleCameraMode = true
            };

            Assert.True(PrototypeKeyboardInput.WasCameraToggleRequested(input));
        }

        [Test]
        public void PrototypeKeyboardInput_MapsInteractKey()
        {
            Assert.True(PrototypeKeyboardInput.WasInteractPressed(new PrototypeInputState { Interact = true }));
        }

        [Test]
        public void PrototypeKeyboardInput_ComposesCameraRelativeMovement()
        {
            var cameraObject = new GameObject("Camera Input Basis");
            cameraObject.transform.rotation = Quaternion.LookRotation(new Vector3(1f, -1f, 1f), Vector3.up);

            var move = PrototypeKeyboardInput.ComposeCameraRelativeMoveInput(
                new PrototypeInputState { MoveUp = true },
                cameraObject.transform);
            var expectedForward = Vector3.ProjectOnPlane(cameraObject.transform.forward, Vector3.up).normalized;

            Assert.AreEqual(0f, move.y, 0.001f);
            Assert.AreEqual(1f, move.magnitude, 0.001f);
            Assert.Greater(Vector3.Dot(expectedForward, move), 0.999f);

            Object.DestroyImmediate(cameraObject);
        }

        [Test]
        public void PrototypeCameraController_CyclesThroughThirdPersonQuarterViewAndTopView()
        {
            var cameraObject = new GameObject("Prototype Camera");
            var camera = cameraObject.AddComponent<Camera>();
            var controller = cameraObject.AddComponent<PrototypeCameraController>();
            var target = new GameObject("Rat Target");

            target.transform.position = Vector3.zero;
            controller.hostTarget = target.transform;

            controller.ApplyCameraNow(PrototypeGameMode.RatHost);

            Assert.AreEqual(PrototypeCameraMode.ThirdPerson, controller.CurrentHostMode);
            Assert.False(camera.orthographic);

            controller.ToggleHostCameraMode();
            controller.ApplyCameraNow(PrototypeGameMode.RatHost);

            Assert.AreEqual(PrototypeCameraMode.QuarterView, controller.CurrentHostMode);
            Assert.True(camera.orthographic);
            Assert.Greater(camera.orthographicSize, 0f);

            controller.ToggleHostCameraMode();
            controller.ApplyCameraNow(PrototypeGameMode.RatHost);

            Assert.AreEqual(PrototypeCameraMode.TopView, controller.CurrentHostMode);
            Assert.True(camera.orthographic);

            controller.ToggleHostCameraMode();
            controller.ApplyCameraNow(PrototypeGameMode.RatHost);

            Assert.AreEqual(PrototypeCameraMode.ThirdPerson, controller.CurrentHostMode);
            Assert.False(camera.orthographic);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(cameraObject);
        }

        [Test]
        public void PrototypeCameraController_QuarterViewUsesLeftRearDominantCameraAxis()
        {
            var cameraObject = new GameObject("Prototype Camera");
            cameraObject.AddComponent<Camera>();
            var controller = cameraObject.AddComponent<PrototypeCameraController>();
            var target = new GameObject("Rat Target");

            target.transform.position = Vector3.zero;
            target.transform.rotation = Quaternion.identity;
            controller.hostTarget = target.transform;

            controller.ToggleHostCameraMode();
            controller.ApplyCameraNow(PrototypeGameMode.RatHost);

            var offset = cameraObject.transform.position - target.transform.position;
            var horizontalForward = Vector3.ProjectOnPlane(cameraObject.transform.forward, Vector3.up).normalized;

            Assert.AreEqual(PrototypeCameraMode.QuarterView, controller.CurrentHostMode);
            Assert.Less(offset.x, -0.1f);
            Assert.Less(offset.z, -0.1f);
            Assert.Less(Mathf.Abs(offset.x), Mathf.Abs(offset.z));
            Assert.Greater(horizontalForward.z, 0.85f);
            Assert.Greater(horizontalForward.x, 0.15f);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(cameraObject);
        }

        [Test]
        public void PrototypeCameraController_TopViewLooksStraightDown()
        {
            var cameraObject = new GameObject("Prototype Camera");
            var camera = cameraObject.AddComponent<Camera>();
            var controller = cameraObject.AddComponent<PrototypeCameraController>();
            var target = new GameObject("Rat Target");

            target.transform.position = Vector3.zero;
            controller.hostTarget = target.transform;

            controller.ToggleHostCameraMode();
            controller.ToggleHostCameraMode();
            controller.ApplyCameraNow(PrototypeGameMode.RatHost);

            var offset = cameraObject.transform.position - target.transform.position;

            Assert.AreEqual(PrototypeCameraMode.TopView, controller.CurrentHostMode);
            Assert.True(camera.orthographic);
            Assert.AreEqual(0f, offset.x, 0.001f);
            Assert.Greater(offset.y, 1f);
            Assert.AreEqual(0f, offset.z, 0.001f);
            Assert.Greater(Vector3.Dot(Vector3.down, cameraObject.transform.forward), 0.999f);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(cameraObject);
        }

        [Test]
        public void PrototypeCameraController_ThirdPersonDoesNotOrbitWhenRatTurns()
        {
            var cameraObject = new GameObject("Prototype Camera");
            cameraObject.AddComponent<Camera>();
            var controller = cameraObject.AddComponent<PrototypeCameraController>();
            var target = new GameObject("Rat Target");

            target.transform.position = Vector3.zero;
            target.transform.rotation = Quaternion.identity;
            controller.hostTarget = target.transform;

            controller.ApplyCameraNow(PrototypeGameMode.RatHost);

            var firstOffset = cameraObject.transform.position - target.transform.position;
            var firstRotation = cameraObject.transform.rotation;

            target.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            controller.ApplyCameraNow(PrototypeGameMode.RatHost);

            var secondOffset = cameraObject.transform.position - target.transform.position;

            Assert.AreEqual(firstOffset.x, secondOffset.x, 0.001f);
            Assert.AreEqual(firstOffset.z, secondOffset.z, 0.001f);
            Assert.Less(Quaternion.Angle(firstRotation, cameraObject.transform.rotation), 0.1f);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(cameraObject);
        }

        [Test]
        public void RatHostPrototypeScene_StartsWithRatVisibleToCamera()
        {
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/RatHostPrototype.unity");

            var camera = Object.FindAnyObjectByType<Camera>();
            var rat = Object.FindAnyObjectByType<RatHostController>(FindObjectsInactive.Include);

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

        [Test]
        public void RatHostPrototypeScene_DefaultsToThirdPersonCameraController()
        {
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/RatHostPrototype.unity");

            var camera = Object.FindAnyObjectByType<Camera>();
            var controller = Object.FindAnyObjectByType<PrototypeCameraController>();

            Assert.NotNull(camera);
            Assert.NotNull(controller);
            Assert.AreEqual(camera, controller.GetComponent<Camera>());
            Assert.True(camera.CompareTag("MainCamera"));
            Assert.AreEqual(camera, Camera.main);
            Assert.AreEqual(PrototypeCameraMode.ThirdPerson, controller.CurrentHostMode);
        }

        [Test]
        public void RatHostPrototypeScene_QuarterViewOffsetUsesLeftRearDominantCameraAxis()
        {
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/RatHostPrototype.unity");

            var controller = Object.FindAnyObjectByType<PrototypeCameraController>();

            Assert.NotNull(controller);
            Assert.Less(controller.quarterViewOffset.x, -0.1f);
            Assert.Less(controller.quarterViewOffset.z, -0.1f);
            Assert.Less(Mathf.Abs(controller.quarterViewOffset.x), Mathf.Abs(controller.quarterViewOffset.z));
        }

        [Test]
        public void RatHostPrototypeScene_NoisyPipeIncludesReadablePipeValveMarkers()
        {
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/RatHostPrototype.unity");

            var interactableObject = GameObject.Find("NoisyPipeRiskInteractable");

            Assert.NotNull(interactableObject);
            Assert.NotNull(interactableObject.GetComponent<RatRiskInteractable>());
            Assert.NotNull(interactableObject.transform.Find("PipeBody"));
            Assert.NotNull(interactableObject.transform.Find("ValveWheel"));

            var marker = interactableObject.transform.Find("InteractionMarkerRing");

            Assert.NotNull(marker);
            Assert.Greater(marker.childCount, 0);
        }

    }
}
