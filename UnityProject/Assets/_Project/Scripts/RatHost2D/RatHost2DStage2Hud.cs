using UnityEngine;
using UnityEngine.UI;

namespace LastHost.Prototype.RatHost2D
{
    public sealed class RatHost2DStage2Hud : MonoBehaviour
    {
        [SerializeField] private RatHost2DSessionController session;
        [SerializeField] private Text stabilityText;
        [SerializeField] private Text fragmentsText;
        [SerializeField] private Text objectiveText;
        [SerializeField] private Text exposureFeedbackText;
        [SerializeField] private Slider stabilitySlider;

        private bool _subscribed;

        public void Configure(
            RatHost2DSessionController sessionController,
            Text virusStabilityText,
            Text mutationFragmentsText,
            Text currentObjectiveText,
            Text currentExposureFeedbackText)
        {
            Unsubscribe();
            session = sessionController;
            stabilityText = virusStabilityText;
            fragmentsText = mutationFragmentsText;
            objectiveText = currentObjectiveText;
            exposureFeedbackText = currentExposureFeedbackText;
            SubscribeAndRefresh();
        }

        public void ConfigureStabilitySlider(Slider virusStabilitySlider)
        {
            stabilitySlider = virusStabilitySlider;
            RefreshNow();
        }

        public void RefreshNow()
        {
            if (session != null)
            {
                Refresh(session.ReadVirusHud());
            }
        }

        private void OnEnable()
        {
            SubscribeAndRefresh();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void SubscribeAndRefresh()
        {
            if (session == null)
            {
                return;
            }

            if (!_subscribed)
            {
                session.VirusHudChanged += Refresh;
                _subscribed = true;
            }

            RefreshNow();
        }

        private void Unsubscribe()
        {
            if (_subscribed && session != null)
            {
                session.VirusHudChanged -= Refresh;
            }

            _subscribed = false;
        }

        private void Refresh(RatHost2DVirusHudSnapshot snapshot)
        {
            SetText(
                stabilityText,
                $"바이러스 안정도 {snapshot.Stability:0}/{snapshot.StartingStability:0}");
            SetText(
                fragmentsText,
                $"조각 {snapshot.CollectedFragments}/{snapshot.RequiredFragments}");
            SetText(objectiveText, snapshot.ObjectiveText);
            SetText(exposureFeedbackText, snapshot.ExposureFeedback);
            if (stabilitySlider != null)
            {
                stabilitySlider.value = snapshot.NormalizedStability;
            }
        }

        private static void SetText(Text target, string value)
        {
            if (target != null)
            {
                target.text = value ?? string.Empty;
            }
        }
    }
}
