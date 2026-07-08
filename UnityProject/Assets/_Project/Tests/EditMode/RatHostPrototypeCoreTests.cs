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
using UnityEngine.TestTools;
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
            Assert.Less(loadout.RatControlPower, 1f);
            Assert.False(loadout.CanUseMammalPassage);

            loadout.Apply(MutationType.Dormancy);
            loadout.Apply(MutationType.NeuralControl);
            loadout.Apply(MutationType.MammalAdaptation);

            Assert.Less(loadout.ImmuneAlertRateMultiplier, 1f);
            Assert.Greater(loadout.RatSpeedMultiplier, 1f);
            Assert.Greater(loadout.RatControlPower, 1f);
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
        public void ImmuneSignalSuppression_AccurateInputsReachSuccess()
        {
            var minigame = new ImmuneSignalSuppressionModel(
                totalSignals: 4,
                requiredSuppressions: 3,
                startingStability: 100f,
                mistakeDamage: 25f,
                accurateWindowSeconds: 0.1f,
                signalIntervalSeconds: 1f);

            for (var i = 0; i < 3; i++)
            {
                minigame.Tick(1f);
                Assert.AreEqual(ImmuneSignalSuppressionJudgement.Accurate, minigame.ResolveInput());
            }

            Assert.AreEqual(VirusMinigameOutcome.Success, minigame.Outcome);
            Assert.AreEqual(3, minigame.SuppressedSignals);
            Assert.AreEqual(0, minigame.MissedSignals);
        }

        [Test]
        public void ImmuneSignalSuppression_MistimedInputsDamageStabilityAndCanFail()
        {
            var minigame = new ImmuneSignalSuppressionModel(
                totalSignals: 4,
                requiredSuppressions: 3,
                startingStability: 40f,
                mistakeDamage: 20f,
                accurateWindowSeconds: 0.1f,
                signalIntervalSeconds: 1f);

            minigame.Tick(0.5f);
            Assert.AreEqual(ImmuneSignalSuppressionJudgement.Early, minigame.ResolveInput());
            Assert.AreEqual(20f, minigame.Stability);

            minigame.Tick(1.2f);
            Assert.AreEqual(ImmuneSignalSuppressionJudgement.Late, minigame.ResolveInput());

            Assert.AreEqual(VirusMinigameOutcome.Failed, minigame.Outcome);
            Assert.AreEqual(0f, minigame.Stability);
            Assert.AreEqual(2, minigame.MissedSignals);
        }

        [Test]
        public void ImmuneSignalSuppression_MissedSignalsFailWhenGoalIsImpossible()
        {
            var minigame = new ImmuneSignalSuppressionModel(
                totalSignals: 3,
                requiredSuppressions: 3,
                startingStability: 100f,
                mistakeDamage: 10f,
                accurateWindowSeconds: 0.1f,
                signalIntervalSeconds: 1f);

            Assert.AreEqual(ImmuneSignalSuppressionJudgement.Missed, minigame.ResolveMissedSignal());

            Assert.AreEqual(VirusMinigameOutcome.Failed, minigame.Outcome);
            Assert.AreEqual(0, minigame.SuppressedSignals);
            Assert.AreEqual(1, minigame.MissedSignals);
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
        public void Session_DefaultInternalMinigameTypeRemainsWhiteBloodCellEvasion()
        {
            var session = new PrototypeSessionState();

            session.EnterVirusMinigame();

            Assert.AreEqual(InternalVirusMinigameType.WhiteBloodCellEvasion, session.CurrentInternalMinigameType);
            Assert.AreEqual("변이 조각 수집 / 백혈구 회피", session.InternalMinigameObjectiveText);
        }

        [Test]
        public void Session_ImmuneSignalSuppressionSuccessMovesToMutationSelection()
        {
            var session = new PrototypeSessionState(new PrototypeConfig
            {
                SignalSuppressionRequiredSuppressions = 2,
                SignalSuppressionTotalSignals = 3,
                SignalSuppressionSignalIntervalSeconds = 1f
            });

            session.EnterVirusMinigame(InternalVirusMinigameType.ImmuneSignalSuppression);

            Assert.AreEqual(PrototypeGameMode.InternalVirus, session.Mode);
            Assert.AreEqual(InternalVirusMinigameType.ImmuneSignalSuppression, session.CurrentInternalMinigameType);
            Assert.AreEqual("면역 신호 억제", session.InternalMinigameObjectiveText);
            Assert.AreEqual("신호 차단 0/2 / 다음 신호 1.0초", session.InternalMinigameProgressText);

            session.TickSignalSuppression(1f);
            Assert.AreEqual("지금 차단", session.SignalSuppressionTimingText);
            Assert.AreEqual(ImmuneSignalSuppressionJudgement.Accurate, session.ResolveSignalSuppressionInput());
            Assert.AreEqual("신호 차단 1/2", session.InternalMinigameObjectiveText);
            Assert.AreEqual("신호 차단 1/2 / 다음 신호 1.0초", session.InternalMinigameProgressText);

            session.TickSignalSuppression(1f);
            Assert.AreEqual(ImmuneSignalSuppressionJudgement.Accurate, session.ResolveSignalSuppressionInput());

            Assert.AreEqual(PrototypeGameMode.MutationSelection, session.Mode);
        }

        [Test]
        public void Session_ImmuneSignalSuppressionFailureReturnsWithoutMutationReward()
        {
            var session = new PrototypeSessionState(new PrototypeConfig
            {
                SignalSuppressionRequiredSuppressions = 3,
                SignalSuppressionTotalSignals = 3,
                SignalSuppressionStartingStability = 20f,
                SignalSuppressionMistakeDamage = 20f,
                SignalSuppressionSignalIntervalSeconds = 1f
            });

            session.EnterVirusMinigame(InternalVirusMinigameType.ImmuneSignalSuppression);
            session.TickSignalSuppression(0.5f);
            Assert.AreEqual(ImmuneSignalSuppressionJudgement.Early, session.ResolveSignalSuppressionInput());

            Assert.AreEqual(PrototypeGameMode.VirusFailed, session.Mode);
            Assert.AreEqual("경보 증폭 1", session.InternalMinigameObjectiveText);

            session.ReturnToRatHostAfterVirusFailure();

            Assert.AreEqual(PrototypeGameMode.RatHost, session.Mode);
            Assert.False(session.Mutations.Has(MutationType.Dormancy));
            Assert.AreEqual(session.Config.AlertAfterVirusFailureReturn, session.ImmuneAlert.Value);
        }

        [Test]
        public void Session_WhiteBloodCellHitRecordsVirusPatternExposureFeedback()
        {
            var session = new PrototypeSessionState();

            session.EnterVirusMinigame();
            session.ResolveVirusFrame(collectedFragment: false, hitByWhiteBloodCell: true);

            Assert.AreEqual(PrototypeGameMode.InternalVirus, session.Mode);
            Assert.True(session.HasVirusPatternExposureFeedback);
            Assert.AreEqual("면역 포착", session.LastVirusPatternExposureLabel);
            Assert.AreEqual(session.Config.VirusPatternExposurePerWhiteBloodCellHit, session.LastVirusPatternExposureDelta);
            Assert.AreEqual(session.Config.VirusPatternExposurePerWhiteBloodCellHit, session.VirusPatternExposureTotal);
            Assert.AreEqual("면역 포착 +8", session.LastVirusPatternExposureFeedbackText);
        }

        [Test]
        public void Session_VirusPatternExposureRaisesSuccessfulReturnImmuneAlertAndClears()
        {
            var session = new PrototypeSessionState();

            session.EnterVirusMinigame();
            session.ResolveVirusFrame(collectedFragment: false, hitByWhiteBloodCell: true);
            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: false);
            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: false);
            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: false);

            Assert.AreEqual(PrototypeGameMode.MutationSelection, session.Mode);
            Assert.AreEqual("면역 포착 흔적 +8", session.VirusPatternExposureSummaryText);

            session.SelectMutation(MutationType.Dormancy);

            Assert.AreEqual(PrototypeGameMode.RatHost, session.Mode);
            Assert.True(session.Mutations.Has(MutationType.Dormancy));
            Assert.AreEqual(
                session.Config.AlertAfterMutationReturn + session.Config.VirusPatternExposurePerWhiteBloodCellHit,
                session.ImmuneAlert.Value);
            Assert.False(session.HasVirusPatternExposureFeedback);
            Assert.False(session.HasVirusPatternExposure);
            Assert.AreEqual(0f, session.VirusPatternExposureTotal);
        }

        [Test]
        public void Session_FailedVirusRunReturnsToRatModeWithoutMutationReward()
        {
            var session = new PrototypeSessionState();

            session.EnterVirusMinigame();
            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: true);
            session.ResolveVirusFrame(collectedFragment: false, hitByWhiteBloodCell: true);
            session.ResolveVirusFrame(collectedFragment: false, hitByWhiteBloodCell: true);

            Assert.AreEqual(PrototypeGameMode.VirusFailed, session.Mode);

            session.RetryVirusMinigame();

            Assert.AreEqual(PrototypeGameMode.RatHost, session.Mode);
            Assert.False(session.Mutations.Has(MutationType.Dormancy));
            Assert.False(session.Mutations.Has(MutationType.NeuralControl));
            Assert.False(session.Mutations.Has(MutationType.MammalAdaptation));
            Assert.Greater(session.ImmuneAlert.Value, session.Config.AlertAfterMutationReturn);
            Assert.Less(session.ImmuneAlert.Value, session.Config.MaxImmuneAlert);
            Assert.True(session.IsRatHostRiskZoneGraceActive);
            Assert.AreEqual(0, session.VirusRun.CollectedFragments);
            Assert.AreEqual(session.Config.VirusStartingStability, session.VirusRun.Stability);
            Assert.False(session.HasVirusPatternExposureFeedback);
            Assert.False(session.HasVirusPatternExposure);
            Assert.AreEqual(0f, session.VirusPatternExposureTotal);
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
        public void Session_NoisyPipeRiskAlertStoresCauseFeedbackAndActualDelta()
        {
            var session = new PrototypeSessionState();
            var interactableObject = new GameObject("Noisy Pipe Under Test");
            var interactable = interactableObject.AddComponent<RatRiskInteractable>();

            session.AddRiskAlert(interactable.riskSeverity, interactable.immuneAlertFeedbackLabel);

            Assert.AreEqual("소음/조직 자극", session.LastImmuneAlertFeedbackLabel);
            Assert.AreEqual(15f, session.LastImmuneAlertFeedbackDelta);
            Assert.AreEqual("소음/조직 자극 +15", session.LastImmuneAlertFeedbackText);

            Object.DestroyImmediate(interactableObject);
        }

        [Test]
        public void Session_ContinuousImmuneAlertFeedbackAccumulatesSmallSameCauseDeltas()
        {
            var session = new PrototypeSessionState();

            session.AddImmuneAlertAmount(0.04f, "오염 노출");
            session.AddImmuneAlertAmount(0.04f, "오염 노출");

            Assert.AreEqual("오염 노출", session.LastImmuneAlertFeedbackLabel);
            Assert.AreEqual(0.08f, session.LastImmuneAlertFeedbackDelta, 0.001f);
            Assert.AreEqual("오염 노출 +0.08", session.LastImmuneAlertFeedbackText);
        }

        [Test]
        public void ImmuneRiskZone_RatStayStoresContaminationFeedbackAndActualDelta()
        {
            var session = CreateSessionControllerForEditModeTest("Session Under Test");
            var zoneObject = new GameObject("Contamination Zone Under Test");
            var zone = zoneObject.AddComponent<ImmuneRiskZone>();
            var ratObject = new GameObject("Rat Collider Under Test");
            ratObject.AddComponent<RatHostController>();
            var ratCollider = ratObject.AddComponent<BoxCollider>();
            zone.session = session;

            zone.ApplyExposure(ratCollider, 0.5f);

            Assert.AreEqual("오염 노출", session.State.LastImmuneAlertFeedbackLabel);
            Assert.AreEqual(6f, session.State.LastImmuneAlertFeedbackDelta);
            Assert.AreEqual("오염 노출 +6", session.State.LastImmuneAlertFeedbackText);
            Assert.AreEqual(session.State.Config.HostMaxHealth - 2f, session.State.HostHealth);

            Object.DestroyImmediate(ratObject);
            Object.DestroyImmediate(zoneObject);
            Object.DestroyImmediate(session.gameObject);
        }

        [Test]
        public void ImmuneRiskZone_NonRatStayDoesNotChangeImmuneAlertOrHostHealth()
        {
            var session = CreateSessionControllerForEditModeTest("Session Under Test");
            var zoneObject = new GameObject("Contamination Zone Under Test");
            var zone = zoneObject.AddComponent<ImmuneRiskZone>();
            var nonRatObject = new GameObject("Non Rat Collider Under Test");
            var nonRatCollider = nonRatObject.AddComponent<BoxCollider>();
            zone.session = session;

            zone.ApplyExposure(nonRatCollider, 0.5f);

            Assert.AreEqual(0f, session.State.ImmuneAlert.Value);
            Assert.AreEqual(session.State.Config.HostMaxHealth, session.State.HostHealth);
            Assert.False(session.State.HasImmuneAlertFeedback);

            Object.DestroyImmediate(nonRatObject);
            Object.DestroyImmediate(zoneObject);
            Object.DestroyImmediate(session.gameObject);
        }

        [Test]
        public void ImmuneRiskZone_InternalVirusModeExposureDoesNotChangeImmuneAlertOrHostHealth()
        {
            var session = CreateSessionControllerForEditModeTest("Session Under Test");
            session.State.EnterVirusMinigame();

            var zoneObject = new GameObject("Contamination Zone Under Test");
            var zone = zoneObject.AddComponent<ImmuneRiskZone>();
            var ratObject = new GameObject("Rat Collider Under Test");
            ratObject.AddComponent<RatHostController>();
            var ratCollider = ratObject.AddComponent<BoxCollider>();
            zone.session = session;

            zone.ApplyExposure(ratCollider, 1f);

            Assert.AreEqual(0f, session.State.ImmuneAlert.Value);
            Assert.AreEqual(session.State.Config.HostMaxHealth, session.State.HostHealth);
            Assert.False(session.State.HasImmuneAlertFeedback);

            Object.DestroyImmediate(ratObject);
            Object.DestroyImmediate(zoneObject);
            Object.DestroyImmediate(session.gameObject);
        }

        [Test]
        public void ImmuneRiskZone_MutationReturnGraceSuppressesImmediateContaminationOnly()
        {
            var session = CreateSessionControllerForEditModeTest("Session Under Test");
            var zoneObject = new GameObject("Contamination Zone Under Test");
            var zone = zoneObject.AddComponent<ImmuneRiskZone>();
            var ratObject = new GameObject("Rat Collider Under Test");
            ratObject.AddComponent<RatHostController>();
            var ratCollider = ratObject.AddComponent<BoxCollider>();
            zone.session = session;

            MoveSessionToMutationSelection(session);
            session.SelectMutation(MutationType.Dormancy);

            var returnAlert = session.State.Config.AlertAfterMutationReturn;
            zone.ApplyExposure(ratCollider, 1f);

            Assert.AreEqual(returnAlert, session.State.ImmuneAlert.Value);
            Assert.AreEqual(session.State.Config.HostMaxHealth, session.State.HostHealth);
            Assert.False(session.State.HasImmuneAlertFeedback);

            session.State.TickRatMode(2f);
            zone.ApplyExposure(ratCollider, 1f);

            Assert.AreEqual(returnAlert + 12f, session.State.ImmuneAlert.Value);
            Assert.AreEqual(session.State.Config.HostMaxHealth - 4f, session.State.HostHealth);
            Assert.AreEqual("오염 노출 +12", session.State.LastImmuneAlertFeedbackText);

            Object.DestroyImmediate(ratObject);
            Object.DestroyImmediate(zoneObject);
            Object.DestroyImmediate(session.gameObject);
        }

        [Test]
        public void RatHostControlModel_NoInputFollowsHostInstinctAtPassiveSpeed()
        {
            var frame = RatHostControlModel.Resolve(
                Vector3.forward,
                Vector3.zero,
                virusControlPower: 0.35f,
                hostInstinctResistance: 1f,
                conflictDotThreshold: -0.25f,
                passiveInstinctSpeedMultiplier: 0.45f,
                forcedControlSpeedMultiplier: 0.65f);

            Assert.AreEqual(Vector3.forward, frame.MoveDirection);
            Assert.AreEqual(0.45f, frame.SpeedMultiplier);
            Assert.False(frame.IsForcedControl);
        }

        [Test]
        public void RatHostControlModel_NoInputStopsWhenHostInstinctIsPaused()
        {
            var frame = RatHostControlModel.Resolve(
                Vector3.forward,
                Vector3.zero,
                virusControlPower: 0.35f,
                hostInstinctResistance: 1f,
                conflictDotThreshold: -0.25f,
                passiveInstinctSpeedMultiplier: 0.45f,
                forcedControlSpeedMultiplier: 0.65f,
                hostInstinctPaused: true);

            Assert.AreEqual(Vector3.zero, frame.MoveDirection);
            Assert.AreEqual(0f, frame.SpeedMultiplier);
            Assert.False(frame.IsForcedControl);
        }

        [Test]
        public void RatHostControlModel_PlayerInputStillWorksWhileHostInstinctIsPaused()
        {
            var frame = RatHostControlModel.Resolve(
                Vector3.forward,
                Vector3.right,
                virusControlPower: 0.35f,
                hostInstinctResistance: 1f,
                conflictDotThreshold: -0.25f,
                passiveInstinctSpeedMultiplier: 0.45f,
                forcedControlSpeedMultiplier: 0.65f,
                hostInstinctPaused: true);

            Assert.Greater(frame.MoveDirection.x, 0f);
            Assert.Greater(frame.MoveDirection.z, 0f);
            Assert.AreEqual(1f, frame.SpeedMultiplier);
            Assert.False(frame.IsForcedControl);
        }

        [Test]
        public void RatHostControlModel_LowControlBlendsPlayerInputIntoInstinct()
        {
            var frame = RatHostControlModel.Resolve(
                Vector3.forward,
                Vector3.right,
                virusControlPower: 0.35f,
                hostInstinctResistance: 1f,
                conflictDotThreshold: -0.25f,
                passiveInstinctSpeedMultiplier: 0.45f,
                forcedControlSpeedMultiplier: 0.65f);

            Assert.Greater(Vector3.Dot(frame.MoveDirection, Vector3.forward), Vector3.Dot(frame.MoveDirection, Vector3.right));
            Assert.Greater(Vector3.Dot(frame.MoveDirection, Vector3.right), 0f);
            Assert.AreEqual(1f, frame.SpeedMultiplier);
            Assert.False(frame.IsForcedControl);
        }

        [Test]
        public void RatHostControlModel_HighControlOverridesHostInstinct()
        {
            var frame = RatHostControlModel.Resolve(
                Vector3.forward,
                Vector3.right,
                virusControlPower: 1.1f,
                hostInstinctResistance: 1f,
                conflictDotThreshold: -0.25f,
                passiveInstinctSpeedMultiplier: 0.45f,
                forcedControlSpeedMultiplier: 0.65f);

            Assert.AreEqual(Vector3.right, frame.MoveDirection);
            Assert.AreEqual(1f, frame.SpeedMultiplier);
            Assert.False(frame.IsForcedControl);
        }

        [Test]
        public void RatHostControlModel_LowControlAgainstInstinctAppliesDemerit()
        {
            var frame = RatHostControlModel.Resolve(
                Vector3.forward,
                Vector3.back,
                virusControlPower: 0.35f,
                hostInstinctResistance: 1f,
                conflictDotThreshold: -0.25f,
                passiveInstinctSpeedMultiplier: 0.45f,
                forcedControlSpeedMultiplier: 0.65f);

            Assert.AreEqual(Vector3.forward, frame.MoveDirection);
            Assert.AreEqual(0.65f, frame.SpeedMultiplier);
            Assert.True(frame.IsForcedControl);
        }

        [Test]
        public void RatHostControlModel_HighControlAgainstInstinctDoesNotApplyDemerit()
        {
            var frame = RatHostControlModel.Resolve(
                Vector3.forward,
                Vector3.back,
                virusControlPower: 1.1f,
                hostInstinctResistance: 1f,
                conflictDotThreshold: -0.25f,
                passiveInstinctSpeedMultiplier: 0.45f,
                forcedControlSpeedMultiplier: 0.65f);

            Assert.AreEqual(Vector3.back, frame.MoveDirection);
            Assert.AreEqual(1f, frame.SpeedMultiplier);
            Assert.False(frame.IsForcedControl);
        }

        [Test]
        public void RatHostInstinctWander_PeriodicTurnChangesInstinctDirection()
        {
            var nextDirection = RatHostInstinctWanderModel.ResolveNextDirection(
                Vector3.forward,
                Vector3.forward,
                Vector3.zero,
                new Vector2(-5f, 5f),
                new Vector2(-3f, 3f),
                turnRequested: true,
                turnAngleDegrees: 60f,
                turnSign: 1f);

            Assert.Greater(nextDirection.x, 0.5f);
            Assert.Greater(nextDirection.z, 0.1f);
            Assert.AreEqual(1f, nextDirection.magnitude, 0.001f);
        }

        [Test]
        public void RatHostInstinctWander_BoundarySteersAlongWallInsteadOfVerticalBounce()
        {
            var nextDirection = RatHostInstinctWanderModel.ResolveNextDirection(
                Vector3.forward,
                Vector3.forward,
                new Vector3(0f, 0f, 2.95f),
                new Vector2(-5f, 5f),
                new Vector2(-3f, 3f),
                turnRequested: false,
                turnAngleDegrees: 60f,
                turnSign: 1f);

            Assert.Greater(nextDirection.x, 0.9f);
            Assert.AreEqual(0f, nextDirection.z, 0.001f);
        }

        [Test]
        public void HostInstinctControlSpike_RepeatedInputTowardDangerTriggersForcedControlOnce()
        {
            var spike = new HostInstinctControlSpike(
                requiredHoldSeconds: 0.5f,
                cooldownSeconds: 1f,
                directionDotThreshold: 0.7f);

            Assert.False(spike.Tick(Vector3.zero, Vector3.forward, Vector3.forward, 0.24f, 0f));

            Assert.True(spike.Tick(Vector3.zero, Vector3.forward, Vector3.forward, 0.26f, 0.24f));

            Assert.False(spike.Tick(Vector3.zero, Vector3.forward, Vector3.forward, 0.5f, 0.5f));
        }

        [Test]
        public void HostInstinctControlSpike_MovementAwayFromDangerResetsForcedControlHold()
        {
            var spike = new HostInstinctControlSpike(
                requiredHoldSeconds: 0.5f,
                cooldownSeconds: 1f,
                directionDotThreshold: 0.7f);

            Assert.False(spike.Tick(Vector3.zero, Vector3.forward, Vector3.forward, 0.35f, 0f));
            Assert.False(spike.Tick(Vector3.zero, Vector3.forward, Vector3.back, 0.1f, 0.35f));
            Assert.False(spike.Tick(Vector3.zero, Vector3.forward, Vector3.forward, 0.2f, 0.45f));
        }

        [Test]
        public void ImmuneRiskZone_ForcedControlSpikeStoresFeedbackWithoutHostDamage()
        {
            var session = CreateSessionControllerForEditModeTest("Session Under Test");
            var zoneObject = new GameObject("Contamination Zone Under Test");
            zoneObject.transform.position = Vector3.forward;
            var zone = zoneObject.AddComponent<ImmuneRiskZone>();
            var ratObject = new GameObject("Rat Controller Under Test");
            var rat = ratObject.AddComponent<RatHostController>();
            zone.session = session;
            zone.forcedControlHoldSeconds = 0.5f;
            zone.forcedControlCooldownSeconds = 1f;
            zone.forcedControlAlertAmount = 8f;

            zone.ApplyForcedControlInput(rat, Vector3.forward, 0.24f, 0f);
            zone.ApplyForcedControlInput(rat, Vector3.forward, 0.26f, 0.24f);

            Assert.AreEqual("강제 조종", session.State.LastImmuneAlertFeedbackLabel);
            Assert.AreEqual(8f, session.State.LastImmuneAlertFeedbackDelta);
            Assert.AreEqual("강제 조종 +8", session.State.LastImmuneAlertFeedbackText);
            Assert.AreEqual(8f, session.State.ImmuneAlert.Value);
            Assert.AreEqual(session.State.Config.HostMaxHealth, session.State.HostHealth);

            Object.DestroyImmediate(ratObject);
            Object.DestroyImmediate(zoneObject);
            Object.DestroyImmediate(session.gameObject);
        }

        [Test]
        public void ImmuneRiskZone_NearbyRatInputTowardZoneTriggersForcedControlFeedback()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

            var session = CreateSessionControllerForEditModeTest("Session Under Test");
            var zoneObject = new GameObject("Contamination Zone Under Test");
            zoneObject.transform.position = Vector3.forward;
            zoneObject.AddComponent<BoxCollider>().isTrigger = true;
            var zone = zoneObject.AddComponent<ImmuneRiskZone>();
            var ratObject = new GameObject("Rat Controller Under Test");
            var rat = ratObject.AddComponent<RatHostController>();
            zone.session = session;
            zone.forcedControlHoldSeconds = 0.5f;
            zone.forcedControlCooldownSeconds = 1f;
            zone.forcedControlAlertAmount = 8f;

            SetRatCurrentMoveWorldDirection(rat, Vector3.forward);

            InvokeNearbyForcedControlInput(zone, 0.5f, 0f);

            Assert.AreEqual("강제 조종 +8", session.State.LastImmuneAlertFeedbackText);
            Assert.AreEqual(8f, session.State.ImmuneAlert.Value);

            Object.DestroyImmediate(ratObject);
            Object.DestroyImmediate(zoneObject);
            Object.DestroyImmediate(session.gameObject);
        }

        [Test]
        public void RatHostController_UpdateDoesNotMoveInactiveCharacterController()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

            var ratObject = new GameObject("Rat Controller Under Test");
            var rat = ratObject.AddComponent<RatHostController>();
            var controller = ratObject.AddComponent<CharacterController>();
            InvokeRatHostAwake(rat);
            controller.enabled = false;

            InvokeRatHostUpdate(rat);

            LogAssert.NoUnexpectedReceived();

            Object.DestroyImmediate(ratObject);
        }

        [Test]
        public void ImmuneRiskZone_OverlappingRatBoundsStoresContaminationFeedbackWithoutTriggerCallback()
        {
            var session = CreateSessionControllerForEditModeTest("Session Under Test");
            var zoneObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            zoneObject.name = "Contamination Zone Under Test";
            zoneObject.transform.position = new Vector3(0f, 0.03f, 0f);
            zoneObject.transform.localScale = new Vector3(2.6f, 0.08f, 1.5f);
            var zoneCollider = zoneObject.GetComponent<BoxCollider>();
            zoneCollider.isTrigger = true;
            var zone = zoneObject.AddComponent<ImmuneRiskZone>();
            zone.session = session;

            var ratObject = new GameObject("Rat Controller Under Test");
            ratObject.transform.position = Vector3.zero;
            ratObject.AddComponent<RatHostController>();
            var controller = ratObject.AddComponent<CharacterController>();
            controller.height = 0.7f;
            controller.radius = 0.25f;
            controller.center = new Vector3(0f, 0.35f, 0f);
            Physics.SyncTransforms();

            zone.ApplyOverlappingRatExposure(0.5f);

            Assert.AreEqual("오염 노출", session.State.LastImmuneAlertFeedbackLabel);
            Assert.AreEqual(6f, session.State.LastImmuneAlertFeedbackDelta);
            Assert.AreEqual("오염 노출 +6", session.State.LastImmuneAlertFeedbackText);
            Assert.AreEqual(session.State.Config.HostMaxHealth - 2f, session.State.HostHealth);

            Object.DestroyImmediate(ratObject);
            Object.DestroyImmediate(zoneObject);
            Object.DestroyImmediate(session.gameObject);
        }

        [Test]
        public void PrototypeHud_ShowsRecommendedVirusFailureCopyAndReturnPanel()
        {
            var session = new PrototypeSessionState();
            MoveSessionToVirusFailure(session);

            var hudObject = new GameObject("HUD Under Test");
            var modeObject = new GameObject("Mode Text");
            var objectiveObject = new GameObject("Objective Text");
            var failurePanel = new GameObject("Failure Panel");
            modeObject.transform.SetParent(hudObject.transform, false);
            objectiveObject.transform.SetParent(hudObject.transform, false);
            failurePanel.transform.SetParent(hudObject.transform, false);
            var modeText = modeObject.AddComponent<Text>();
            var objectiveText = objectiveObject.AddComponent<Text>();
            var hud = hudObject.AddComponent<PrototypeHud>();
            hud.modeText = modeText;
            hud.objectiveText = objectiveText;
            hud.failurePanel = failurePanel;

            hud.Refresh(session);

            Assert.AreEqual("면역 반응 돌파 실패", modeText.text);
            Assert.AreEqual("보상 없이 쥐 숙주로 복귀", objectiveText.text);
            Assert.True(failurePanel.activeSelf);

            Object.DestroyImmediate(hudObject);
        }

        [Test]
        public void PrototypeHud_ShowsImmuneAlertCauseFeedbackBeforeRatObjective()
        {
            var session = new PrototypeSessionState();
            session.AddRiskAlert(0.75f, "소음/조직 자극");

            var hudObject = new GameObject("HUD Under Test");
            var objectiveObject = new GameObject("Objective Text");
            objectiveObject.transform.SetParent(hudObject.transform, false);
            var objectiveText = objectiveObject.AddComponent<Text>();
            var hud = hudObject.AddComponent<PrototypeHud>();
            hud.objectiveText = objectiveText;

            hud.Refresh(session);

            Assert.AreEqual("소음/조직 자극 +15", objectiveText.text);

            Object.DestroyImmediate(hudObject);
        }

        [Test]
        public void Session_ImmuneAlertFeedbackExpiresAfterConfiguredRatModeTime()
        {
            var session = new PrototypeSessionState(CreateConfigWithFeedbackSeconds(1f));

            session.AddRiskAlert(0.75f, "소음/조직 자극");

            Assert.True(session.HasImmuneAlertFeedback);

            session.TickRatMode(0.99f);

            Assert.True(session.HasImmuneAlertFeedback);

            session.TickRatMode(0.01f);

            Assert.False(session.HasImmuneAlertFeedback);
            Assert.AreEqual(string.Empty, session.LastImmuneAlertFeedbackText);
        }

        [Test]
        public void PrototypeHud_ReturnsToRiskPromptAfterImmuneAlertFeedbackExpires()
        {
            var session = new PrototypeSessionState(CreateConfigWithFeedbackSeconds(1f));
            session.SetRatRiskInteractionAffordance(true, "소음 배관 조사 가능");
            session.AddRiskAlert(0.75f, "소음/조직 자극");

            var hudObject = new GameObject("HUD Under Test");
            var objectiveObject = new GameObject("Objective Text");
            objectiveObject.transform.SetParent(hudObject.transform, false);
            var objectiveText = objectiveObject.AddComponent<Text>();
            var hud = hudObject.AddComponent<PrototypeHud>();
            hud.objectiveText = objectiveText;

            hud.Refresh(session);

            Assert.AreEqual("소음/조직 자극 +15", objectiveText.text);

            session.TickRatMode(1f);
            hud.Refresh(session);

            Assert.AreEqual("소음 배관 조사 가능", objectiveText.text);

            Object.DestroyImmediate(hudObject);
        }

        [Test]
        public void Session_ClearsRiskPromptWhenFeedbackAlertEntersVirusMode()
        {
            var session = new PrototypeSessionState();
            session.SetRatRiskInteractionAffordance(true, "소음 배관 조사 가능");
            session.AddImmuneAlertAmount(90f);

            session.AddRiskAlert(0.75f, "소음/조직 자극");

            Assert.AreEqual(PrototypeGameMode.InternalVirus, session.Mode);
            Assert.False(session.IsRatRiskInteractionAvailable);
            Assert.AreEqual(string.Empty, session.RatRiskInteractionPrompt);
            Assert.AreEqual(10f, session.LastImmuneAlertFeedbackDelta);

            var hudObject = new GameObject("HUD Under Test");
            var objectiveObject = new GameObject("Objective Text");
            objectiveObject.transform.SetParent(hudObject.transform, false);
            var objectiveText = objectiveObject.AddComponent<Text>();
            var hud = hudObject.AddComponent<PrototypeHud>();
            hud.objectiveText = objectiveText;

            hud.Refresh(session);

            Assert.AreEqual("변이 조각 수집 / 백혈구 회피", objectiveText.text);

            Object.DestroyImmediate(hudObject);
        }

        [Test]
        public void PrototypeHud_ShowsVirusPatternExposureDuringInternalAndMutationSelection()
        {
            var session = new PrototypeSessionState();
            var hudObject = new GameObject("HUD Under Test");
            var objectiveObject = new GameObject("Objective Text");
            objectiveObject.transform.SetParent(hudObject.transform, false);
            var objectiveText = objectiveObject.AddComponent<Text>();
            var hud = hudObject.AddComponent<PrototypeHud>();
            hud.objectiveText = objectiveText;

            session.EnterVirusMinigame();
            session.ResolveVirusFrame(collectedFragment: false, hitByWhiteBloodCell: true);
            hud.Refresh(session);

            Assert.AreEqual("면역 포착 +8", objectiveText.text);

            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: false);
            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: false);
            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: false);
            hud.Refresh(session);

            Assert.AreEqual(PrototypeGameMode.MutationSelection, session.Mode);
            Assert.AreEqual("면역 포착 흔적 +8", objectiveText.text);

            Object.DestroyImmediate(hudObject);
        }

        [Test]
        public void PrototypeHud_ShowsSignalSuppressionPanelWithMovingMarker()
        {
            var session = new PrototypeSessionState(new PrototypeConfig
            {
                SignalSuppressionTotalSignals = 3,
                SignalSuppressionRequiredSuppressions = 2,
                SignalSuppressionSignalIntervalSeconds = 1f
            });
            session.EnterVirusMinigame(InternalVirusMinigameType.ImmuneSignalSuppression);

            var canvasObject = new GameObject("Canvas Under Test");
            var hudObject = new GameObject("HUD Under Test");
            hudObject.transform.SetParent(canvasObject.transform, false);
            var hud = hudObject.AddComponent<PrototypeHud>();

            hud.Refresh(session);

            var panel = FindChildIncludingInactive(canvasObject.transform, "SignalSuppressionPanel");
            Assert.NotNull(panel);
            Assert.True(panel.gameObject.activeSelf);

            var markerTransform = FindChildIncludingInactive(panel, "SignalMarker");
            var timingTransform = FindChildIncludingInactive(panel, "SignalTimingText");
            var progressTransform = FindChildIncludingInactive(panel, "SignalProgressText");

            Assert.NotNull(markerTransform);
            Assert.NotNull(timingTransform);
            Assert.NotNull(progressTransform);

            var marker = markerTransform.GetComponent<RectTransform>();
            var timingText = timingTransform.GetComponent<Text>();
            var progressText = progressTransform.GetComponent<Text>();

            Assert.NotNull(marker);
            Assert.NotNull(timingText);
            Assert.NotNull(progressText);
            Assert.AreEqual("신호 차단 0/2 / 다음 신호 1.0초", progressText.text);

            var startX = marker.anchoredPosition.x;

            session.TickSignalSuppression(0.5f);
            hud.Refresh(session);

            Assert.Greater(marker.anchoredPosition.x, startX);

            session.TickSignalSuppression(0.5f);
            hud.Refresh(session);

            Assert.AreEqual("지금 차단", timingText.text);
            Assert.AreEqual(0f, marker.anchoredPosition.x, 3f);

            Object.DestroyImmediate(canvasObject);
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
        public void PrototypeKeyboardInput_MapsDebugSignalSuppressionKey()
        {
            var input = new PrototypeInputState
            {
                DebugSignalSuppression = true
            };

            Assert.True(PrototypeKeyboardInput.WasDebugSignalSuppressionRequested(input));
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

        [Test]
        public void RatHostPrototypeScene_FailurePanelUsesReturnCopy()
        {
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/RatHostPrototype.unity");

            var titleObject = FindSceneGameObjectIncludingInactive("FailureTitle");
            var buttonObject = FindSceneGameObjectIncludingInactive("RetryButton");

            Assert.NotNull(titleObject);
            Assert.NotNull(buttonObject);

            var title = titleObject.GetComponent<Text>();
            var buttonText = buttonObject.GetComponentInChildren<Text>(true);

            Assert.NotNull(title);
            Assert.NotNull(buttonText);
            Assert.AreEqual("면역 반응 돌파 실패", title.text);
            Assert.AreEqual("쥐 숙주로 복귀", buttonText.text);
        }

        [Test]
        public void RatHostPrototypeScene_ToxicWaterRiskZoneSupportsCharacterControllerTriggerDelivery()
        {
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/RatHostPrototype.unity");

            var rat = Object.FindAnyObjectByType<RatHostController>(FindObjectsInactive.Include);
            var riskZoneObject = GameObject.Find("ToxicWaterRiskZone");

            Assert.NotNull(rat);
            Assert.NotNull(rat.GetComponent<CharacterController>());
            Assert.NotNull(riskZoneObject);
            Assert.NotNull(riskZoneObject.GetComponent<ImmuneRiskZone>());

            var trigger = riskZoneObject.GetComponent<BoxCollider>();
            var body = riskZoneObject.GetComponent<Rigidbody>();

            Assert.NotNull(trigger);
            Assert.True(trigger.isTrigger);
            Assert.NotNull(body);
            Assert.True(body.isKinematic);
            Assert.False(body.useGravity);
        }

        [Test]
        public void RatHostPrototypeScene_ToxicWaterRiskZoneOverlapRaisesHudFeedback()
        {
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/RatHostPrototype.unity");

            var session = Object.FindAnyObjectByType<PrototypeSessionController>(FindObjectsInactive.Include);
            var rat = Object.FindAnyObjectByType<RatHostController>(FindObjectsInactive.Include);
            var riskZoneObject = GameObject.Find("ToxicWaterRiskZone");

            Assert.NotNull(session);
            Assert.NotNull(rat);
            Assert.NotNull(riskZoneObject);

            EnsureSessionControllerAwake(session);

            var zone = riskZoneObject.GetComponent<ImmuneRiskZone>();
            var zoneCollider = riskZoneObject.GetComponent<Collider>();
            var ratCollider = rat.GetComponent<Collider>();

            Assert.NotNull(zone);
            Assert.NotNull(zoneCollider);
            Assert.NotNull(ratCollider);
            Assert.NotNull(session.hud);
            Assert.NotNull(session.hud.objectiveText);

            var zoneCenter = zoneCollider.bounds.center;
            rat.transform.position = new Vector3(zoneCenter.x, rat.transform.position.y, zoneCenter.z);
            Physics.SyncTransforms();

            Assert.True(zoneCollider.bounds.Intersects(ratCollider.bounds));

            zone.ApplyOverlappingRatExposure(0.5f);

            Assert.AreEqual("오염 노출 +6", session.State.LastImmuneAlertFeedbackText);
            Assert.AreEqual("오염 노출 +6", session.hud.objectiveText.text);
            Assert.Greater(session.State.ImmuneAlert.Value, 0f);
        }

        private static PrototypeConfig CreateConfigWithFeedbackSeconds(float seconds)
        {
            return new PrototypeConfig { ImmuneAlertFeedbackSeconds = seconds };
        }

        private static PrototypeSessionController CreateSessionControllerForEditModeTest(string objectName)
        {
            var sessionObject = new GameObject(objectName);
            var session = sessionObject.AddComponent<PrototypeSessionController>();
            EnsureSessionControllerAwake(session);

            return session;
        }

        private static void MoveSessionToMutationSelection(PrototypeSessionController session)
        {
            session.State.EnterVirusMinigame();
            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: false);
            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: false);
            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: false);

            Assert.AreEqual(PrototypeGameMode.MutationSelection, session.CurrentMode);
        }

        private static void MoveSessionToVirusFailure(PrototypeSessionState session)
        {
            session.EnterVirusMinigame();
            session.ResolveVirusFrame(collectedFragment: true, hitByWhiteBloodCell: true);
            session.ResolveVirusFrame(collectedFragment: false, hitByWhiteBloodCell: true);
            session.ResolveVirusFrame(collectedFragment: false, hitByWhiteBloodCell: true);

            Assert.AreEqual(PrototypeGameMode.VirusFailed, session.Mode);
        }

        private static GameObject FindSceneGameObjectIncludingInactive(string objectName)
        {
            foreach (var gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (gameObject.name == objectName && gameObject.scene.IsValid())
                {
                    return gameObject;
                }
            }

            return null;
        }

        private static Transform FindChildIncludingInactive(Transform parent, string childName)
        {
            if (parent.name == childName)
            {
                return parent;
            }

            foreach (Transform child in parent)
            {
                var found = FindChildIncludingInactive(child, childName);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        private static void EnsureSessionControllerAwake(PrototypeSessionController session)
        {
            if (session.State != null)
            {
                return;
            }

            var awake = typeof(PrototypeSessionController).GetMethod("Awake", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            awake.Invoke(session, null);
        }

        private static void SetRatCurrentMoveWorldDirection(RatHostController rat, Vector3 direction)
        {
            var property = typeof(RatHostController).GetProperty(nameof(RatHostController.CurrentMoveWorldDirection));
            property.SetValue(rat, direction);
        }

        private static void InvokeRatHostAwake(RatHostController rat)
        {
            var method = typeof(RatHostController).GetMethod(
                "Awake",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            method.Invoke(rat, null);
        }

        private static void InvokeRatHostUpdate(RatHostController rat)
        {
            var method = typeof(RatHostController).GetMethod(
                "Update",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            method.Invoke(rat, null);
        }

        private static void InvokeNearbyForcedControlInput(ImmuneRiskZone zone, float deltaTime, float currentTimeSeconds)
        {
            var method = typeof(ImmuneRiskZone).GetMethod(
                "ApplyNearbyForcedControlInput",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null,
                new[] { typeof(float), typeof(float) },
                null);
            method.Invoke(zone, new object[] { deltaTime, currentTimeSeconds });
        }

    }
}
