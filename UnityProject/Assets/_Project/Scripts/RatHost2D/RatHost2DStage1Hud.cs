using UnityEngine;
using UnityEngine.UI;

namespace LastHost.Prototype.RatHost2D
{
    public sealed class RatHost2DStage1Hud : MonoBehaviour
    {
        [SerializeField] private RatHost2DSessionController session;
        [SerializeField] private Text healthText;
        [SerializeField] private Text alertText;
        [SerializeField] private Text modeText;
        [SerializeField] private Text feedbackText;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider alertSlider;

        private bool _subscribed;

        public void Configure(
            RatHost2DSessionController sessionController,
            Text hostHealthText,
            Text immuneAlertText,
            Text currentModeText,
            Text immuneFeedbackText)
        {
            Unsubscribe();
            session = sessionController;
            healthText = hostHealthText;
            alertText = immuneAlertText;
            modeText = currentModeText;
            feedbackText = immuneFeedbackText;
            SubscribeAndRefresh();
        }

        public void ConfigureSliders(Slider hostHealthSlider, Slider immuneAlertSlider)
        {
            healthSlider = hostHealthSlider;
            alertSlider = immuneAlertSlider;
            RefreshNow();
        }

        public void RefreshNow()
        {
            if (session != null)
            {
                Refresh(session.ReadHostHud());
            }
        }

        private void OnEnable()
        {
            SubscribeAndRefresh();
        }

        private void Start()
        {
            RefreshNow();
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
                session.HostHudChanged += Refresh;
                _subscribed = true;
            }

            RefreshNow();
        }

        private void Unsubscribe()
        {
            if (_subscribed && session != null)
            {
                session.HostHudChanged -= Refresh;
            }

            _subscribed = false;
        }

        private void Refresh(RatHost2DHudSnapshot snapshot)
        {
            SetText(
                healthText,
                $"숙주 생명력 {snapshot.HostHealth:0}/{snapshot.HostMaxHealth:0}");
            SetText(
                alertText,
                $"면역 경계도 {snapshot.ImmuneAlert:0}/{snapshot.ImmuneAlertMax:0}");
            SetText(modeText, $"현재 모드 {snapshot.ModeLabel}");
            SetText(feedbackText, snapshot.ImmuneAlertFeedback);
            SetSlider(healthSlider, snapshot.HostHealthNormalized);
            SetSlider(alertSlider, snapshot.ImmuneAlertNormalized);
        }

        private static void SetText(Text target, string value)
        {
            if (target != null)
            {
                target.text = value ?? string.Empty;
            }
        }

        private static void SetSlider(Slider target, float value)
        {
            if (target != null)
            {
                target.value = Mathf.Clamp01(value);
            }
        }
    }
}
