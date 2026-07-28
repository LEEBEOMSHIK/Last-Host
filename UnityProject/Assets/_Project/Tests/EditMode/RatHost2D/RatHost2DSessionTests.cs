using LastHost.Prototype.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace LastHost.Prototype.RatHost2D.Tests
{
    public sealed class RatHost2DSessionTests
    {
        [Test]
        public void SessionOwnsOneStablePrototypeSessionState()
        {
            var sessionObject = new GameObject("RatHost2DSession");
            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();
                var first = session.State;

                session.TickHostMode(1f);

                Assert.That(session.State, Is.SameAs(first));
                Assert.That(first.Config.BaseAlertPerSecond, Is.Zero);
                Assert.That(first.CurrentInternalMinigameType,
                    Is.EqualTo(InternalVirusMinigameType.WhiteBloodCellEvasion));
            }
            finally
            {
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void SafeWaitingAndOrdinaryHostTickDoNotChangeAlertOrHealth()
        {
            var sessionObject = new GameObject("RatHost2DSession");
            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();

                for (var index = 0; index < 300; index++)
                {
                    session.TickHostMode(0.02f);
                }

                Assert.That(session.State.ImmuneAlert.Value, Is.Zero);
                Assert.That(session.State.HostHealth,
                    Is.EqualTo(session.State.Config.HostMaxHealth));
                Assert.That(session.CurrentMode, Is.EqualTo(PrototypeGameMode.RatHost));
            }
            finally
            {
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void ContaminationAppliesApprovedRatesAndReadableFeedback()
        {
            var sessionObject = new GameObject("RatHost2DSession");
            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();

                Assert.That(session.ApplyContaminationExposure(0.5f), Is.True);

                Assert.That(session.State.ImmuneAlert.Value, Is.EqualTo(6f).Within(0.0001f));
                Assert.That(session.State.HostHealth, Is.EqualTo(98f).Within(0.0001f));
                Assert.That(session.State.LastImmuneAlertFeedbackText,
                    Is.EqualTo("오염 노출 +6"));

                var hud = session.ReadHostHud();
                Assert.That(hud.HostHealth, Is.EqualTo(98f).Within(0.0001f));
                Assert.That(hud.ImmuneAlert, Is.EqualTo(6f).Within(0.0001f));
                Assert.That(hud.ImmuneAlertFeedback, Is.EqualTo("오염 노출 +6"));
                Assert.That(hud.IsHostHudVisible, Is.True);
            }
            finally
            {
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void NaturalContaminationReachesOneWhiteBloodCellShellTransition()
        {
            var sessionObject = new GameObject("RatHost2DSession");
            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();

                for (var index = 0; index < 8; index++)
                {
                    Assert.That(session.ApplyContaminationExposure(1f), Is.True);
                }

                Assert.That(session.State.ImmuneAlert.Value, Is.EqualTo(96f).Within(0.0001f));
                Assert.That(session.CurrentMode, Is.EqualTo(PrototypeGameMode.RatHost));

                Assert.That(session.ApplyContaminationExposure(1f / 3f), Is.True);

                Assert.That(session.State.ImmuneAlert.Value, Is.EqualTo(100f).Within(0.0001f));
                Assert.That(session.CurrentMode, Is.EqualTo(PrototypeGameMode.InternalVirus));
                Assert.That(session.State.CurrentInternalMinigameType,
                    Is.EqualTo(InternalVirusMinigameType.WhiteBloodCellEvasion));
                Assert.That(session.InternalShellEntryCount, Is.EqualTo(1));

                var healthAtTransition = session.State.HostHealth;
                Assert.That(session.ApplyContaminationExposure(10f), Is.False);
                session.TickHostMode(10f);

                Assert.That(session.State.ImmuneAlert.Value, Is.EqualTo(100f));
                Assert.That(session.State.HostHealth, Is.EqualTo(healthAtTransition));
                Assert.That(session.InternalShellEntryCount, Is.EqualTo(1));
                Assert.That(session.ReadHostHud().IsHostHudVisible, Is.False);
            }
            finally
            {
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void TransitionDisablesHostRootHudAndCollidersAndShowsShell()
        {
            var sessionObject = new GameObject("RatHost2DSession");
            var hostRoot = new GameObject("RatHostMode2D");
            var shellRoot = new GameObject("InternalShell2D");
            var hudRoot = new GameObject("HostHud2D");
            var colliderObject = new GameObject("RatCollider");

            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();
                var hostCollider = colliderObject.AddComponent<BoxCollider2D>();
                session.Configure(
                    hostRoot,
                    shellRoot,
                    hudRoot,
                    null,
                    new Collider2D[] { hostCollider });

                Assert.That(hostRoot.activeSelf, Is.True);
                Assert.That(shellRoot.activeSelf, Is.False);
                Assert.That(hudRoot.activeSelf, Is.True);
                Assert.That(hostCollider.enabled, Is.True);

                session.ApplyContaminationExposure(100f / 12f);

                Assert.That(hostRoot.activeSelf, Is.False);
                Assert.That(shellRoot.activeSelf, Is.True);
                Assert.That(hudRoot.activeSelf, Is.False);
                Assert.That(hostCollider.enabled, Is.False);
                Assert.That(RatHost2DSessionController.InternalShellObjective,
                    Does.Contain("변이 조각").And.Contain("백혈구 회피"));
            }
            finally
            {
                Object.DestroyImmediate(colliderObject);
                Object.DestroyImmediate(hudRoot);
                Object.DestroyImmediate(shellRoot);
                Object.DestroyImmediate(hostRoot);
                Object.DestroyImmediate(sessionObject);
            }
        }

        [Test]
        public void StageOneHudReadsSessionAndRefreshesContaminationFeedback()
        {
            var sessionObject = new GameObject("RatHost2DSession");
            var hudObject = new GameObject("HostHud2D");
            var healthObject = new GameObject("HealthText");
            var alertObject = new GameObject("AlertText");
            var modeObject = new GameObject("ModeText");
            var feedbackObject = new GameObject("FeedbackText");

            try
            {
                var session = sessionObject.AddComponent<RatHost2DSessionController>();
                var healthText = healthObject.AddComponent<Text>();
                var alertText = alertObject.AddComponent<Text>();
                var modeText = modeObject.AddComponent<Text>();
                var feedbackText = feedbackObject.AddComponent<Text>();
                var hud = hudObject.AddComponent<RatHost2DStage1Hud>();
                hud.Configure(session, healthText, alertText, modeText, feedbackText);

                session.ApplyContaminationExposure(0.5f);

                Assert.That(healthText.text, Is.EqualTo("숙주 생명력 98/100"));
                Assert.That(alertText.text, Is.EqualTo("면역 경계도 6/100"));
                Assert.That(modeText.text, Is.EqualTo("현재 모드 쥐 숙주"));
                Assert.That(feedbackText.text, Is.EqualTo("오염 노출 +6"));
            }
            finally
            {
                Object.DestroyImmediate(feedbackObject);
                Object.DestroyImmediate(modeObject);
                Object.DestroyImmediate(alertObject);
                Object.DestroyImmediate(healthObject);
                Object.DestroyImmediate(hudObject);
                Object.DestroyImmediate(sessionObject);
            }
        }
    }
}
