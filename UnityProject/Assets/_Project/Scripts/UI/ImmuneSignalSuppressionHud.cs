using LastHost.Prototype.Core;
using UnityEngine;
using UnityEngine.UI;

namespace LastHost.Prototype.UI
{
    public sealed class ImmuneSignalSuppressionHud : MonoBehaviour
    {
        private static readonly Color WaitingColor = new Color(0.38f, 0.78f, 0.96f, 1f);
        private static readonly Color ReadyColor = new Color(0.45f, 0.96f, 0.58f, 1f);
        private static readonly Color LateColor = new Color(0.96f, 0.34f, 0.28f, 1f);

        public GameObject root;
        public RectTransform trackRect;
        public RectTransform accurateWindowRect;
        public RectTransform judgementLine;
        public RectTransform signalMarker;
        public Image signalMarkerImage;
        public Image judgementLineImage;
        public Image accurateWindowImage;
        public Text titleText;
        public Text timingText;
        public Text resultText;
        public Text progressText;
        public Slider suppressionSlider;

        public void Refresh(PrototypeSessionState state)
        {
            var rootObject = root != null ? root : gameObject;
            var isActive = state != null
                && state.Mode == PrototypeGameMode.InternalVirus
                && state.CurrentInternalMinigameType == InternalVirusMinigameType.ImmuneSignalSuppression;

            rootObject.SetActive(isActive);
            if (!isActive)
            {
                return;
            }

            SetText(titleText, "면역 신호 억제");
            SetText(timingText, state.SignalSuppressionTimingText);
            SetText(resultText, state.InternalMinigameObjectiveText);
            SetText(progressText, state.InternalMinigameProgressText);
            SetSlider(suppressionSlider, state.SignalSuppressionRun.SuppressedSignals / (float)state.SignalSuppressionRun.RequiredSuppressions);
            UpdateSignalMarker(state);
        }

        private void UpdateSignalMarker(PrototypeSessionState state)
        {
            if (trackRect == null || signalMarker == null)
            {
                return;
            }

            var trackWidth = GetRectWidth(trackRect);
            var markerWidth = GetRectWidth(signalMarker);
            var halfTravelWidth = Mathf.Max(1f, (trackWidth - markerWidth) * 0.5f);
            var timeUntilSignal = state.SignalSuppressionRun.TimeUntilSignal;
            var xPosition = timeUntilSignal >= 0f
                ? CalculateApproachX(timeUntilSignal, state.Config.SignalSuppressionSignalIntervalSeconds, halfTravelWidth)
                : CalculateLateX(timeUntilSignal, state.Config.SignalSuppressionAccurateWindowSeconds, halfTravelWidth);

            signalMarker.anchoredPosition = new Vector2(xPosition, signalMarker.anchoredPosition.y);

            var isReady = Mathf.Abs(timeUntilSignal) <= state.Config.SignalSuppressionAccurateWindowSeconds;
            var markerColor = isReady ? ReadyColor : timeUntilSignal < 0f ? LateColor : WaitingColor;
            SetImageColor(signalMarkerImage, markerColor);
            SetImageColor(judgementLineImage, isReady ? ReadyColor : WaitingColor);

            if (accurateWindowRect != null)
            {
                var interval = Mathf.Max(0.01f, state.Config.SignalSuppressionSignalIntervalSeconds);
                var windowWidth = Mathf.Clamp(
                    trackWidth * (state.Config.SignalSuppressionAccurateWindowSeconds / interval) * 2f,
                    34f,
                    Mathf.Max(34f, trackWidth * 0.35f));
                accurateWindowRect.sizeDelta = new Vector2(windowWidth, accurateWindowRect.sizeDelta.y);
            }

            SetImageColor(accurateWindowImage, isReady
                ? new Color(0.45f, 0.96f, 0.58f, 0.35f)
                : new Color(1f, 1f, 1f, 0.12f));
        }

        private static float CalculateApproachX(float timeUntilSignal, float intervalSeconds, float halfTravelWidth)
        {
            var interval = Mathf.Max(0.01f, intervalSeconds);
            var progress = 1f - Mathf.Clamp01(timeUntilSignal / interval);
            return Mathf.Lerp(-halfTravelWidth, 0f, progress);
        }

        private static float CalculateLateX(float timeUntilSignal, float accurateWindowSeconds, float halfTravelWidth)
        {
            var lateWindow = Mathf.Max(0.01f, accurateWindowSeconds * 3f);
            var progress = Mathf.Clamp01(-timeUntilSignal / lateWindow);
            return Mathf.Lerp(0f, halfTravelWidth, progress);
        }

        private static float GetRectWidth(RectTransform rect)
        {
            return rect.rect.width > 0.01f ? rect.rect.width : Mathf.Abs(rect.sizeDelta.x);
        }

        private static void SetText(Text text, string value)
        {
            if (text != null)
            {
                text.text = value;
            }
        }

        private static void SetSlider(Slider slider, float value)
        {
            if (slider != null)
            {
                slider.value = Mathf.Clamp01(value);
            }
        }

        private static void SetImageColor(Image image, Color color)
        {
            if (image != null)
            {
                image.color = color;
            }
        }
    }
}
