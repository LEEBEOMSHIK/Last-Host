using System.Text;
using LastHost.Prototype.Core;
using LastHost.Prototype.Mutations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace LastHost.Prototype.UI
{
    public sealed class PrototypeHud : MonoBehaviour
    {
        public Text modeText;
        public Text hostHealthText;
        public Text immuneAlertText;
        public Text virusStabilityText;
        public Text mutationFragmentsText;
        public Text mutationStatusText;
        public Text objectiveText;
        public Slider hostHealthSlider;
        public Slider immuneAlertSlider;
        public Slider virusStabilitySlider;
        public GameObject mutationPanel;
        public GameObject failurePanel;
        public ImmuneSignalSuppressionHud signalSuppressionHud;

        private PrototypeSessionController session;
        private readonly StringBuilder mutationBuilder = new StringBuilder();

        private void Awake()
        {
            EnsureEventSystem();
            EnsureSignalSuppressionHud();
        }

        public void Bind(PrototypeSessionController controller)
        {
            session = controller;
        }

        public void Refresh(PrototypeSessionState state)
        {
            SetText(modeText, GetModeName(state.Mode));
            SetText(hostHealthText, $"숙주 생명력 {state.HostHealth:0}/{state.Config.HostMaxHealth:0}");
            SetText(immuneAlertText, $"면역 경계도 {state.ImmuneAlert.Value:0}%");
            SetText(virusStabilityText, $"바이러스 안정도 {state.ActiveVirusStability:0}/{state.ActiveVirusStartingStability:0}");
            SetText(mutationFragmentsText, state.InternalMinigameProgressText);
            SetText(mutationStatusText, BuildMutationStatus(state.Mutations));
            SetText(objectiveText, GetObjective(state));

            SetSlider(hostHealthSlider, state.HostHealth / state.Config.HostMaxHealth);
            SetSlider(immuneAlertSlider, state.ImmuneAlert.NormalizedValue);
            SetSlider(virusStabilitySlider, state.ActiveVirusNormalizedStability);
            EnsureSignalSuppressionHud();
            signalSuppressionHud?.Refresh(state);

            if (mutationPanel != null)
            {
                mutationPanel.SetActive(state.Mode == PrototypeGameMode.MutationSelection);
            }

            if (failurePanel != null)
            {
                failurePanel.SetActive(state.Mode == PrototypeGameMode.VirusFailed);
            }
        }

        private void EnsureSignalSuppressionHud()
        {
            if (signalSuppressionHud != null)
            {
                return;
            }

            var parent = transform.parent != null ? transform.parent : transform;
            var existingPanel = parent.Find("SignalSuppressionPanel");
            if (existingPanel != null)
            {
                signalSuppressionHud = existingPanel.GetComponent<ImmuneSignalSuppressionHud>();
                if (signalSuppressionHud != null)
                {
                    return;
                }
            }

            signalSuppressionHud = BuildSignalSuppressionHud(parent);
        }

        private static ImmuneSignalSuppressionHud BuildSignalSuppressionHud(Transform parent)
        {
            var panel = CreateRuntimeImage(parent, "SignalSuppressionPanel", new Color(0.035f, 0.05f, 0.055f, 0.9f));
            var panelRect = panel.GetComponent<RectTransform>();
            SetCentered(panelRect, new Vector2(620f, 250f));

            var hud = panel.AddComponent<ImmuneSignalSuppressionHud>();
            hud.root = panel;
            hud.titleText = CreateRuntimeText(panel.transform, "SignalTitleText", "면역 신호 억제", 28, TextAnchor.MiddleCenter, new Vector2(0f, -28f), new Vector2(560f, 38f));
            hud.rhythmText = CreateRuntimeText(panel.transform, "SignalRhythmText", "리듬 1단계 · 다음 신호 일반", 18, TextAnchor.MiddleCenter, new Vector2(0f, -60f), new Vector2(560f, 26f));
            hud.timingText = CreateRuntimeText(panel.transform, "SignalTimingText", "다음 신호", 22, TextAnchor.MiddleCenter, new Vector2(0f, -88f), new Vector2(560f, 32f));
            hud.resultText = CreateRuntimeText(panel.transform, "SignalResultText", "면역 신호 억제", 20, TextAnchor.MiddleCenter, new Vector2(0f, -122f), new Vector2(560f, 30f));
            hud.progressText = CreateRuntimeText(panel.transform, "SignalProgressText", "신호 차단 0/8", 18, TextAnchor.MiddleCenter, new Vector2(0f, -214f), new Vector2(560f, 28f));

            var track = CreateRuntimeImage(panel.transform, "SignalTrack", new Color(1f, 1f, 1f, 0.12f));
            hud.trackRect = track.GetComponent<RectTransform>();
            SetCenteredChild(hud.trackRect, new Vector2(480f, 24f), new Vector2(0f, -145f));

            var accurateWindow = CreateRuntimeImage(track.transform, "AccurateWindow", new Color(1f, 1f, 1f, 0.12f));
            hud.accurateWindowRect = accurateWindow.GetComponent<RectTransform>();
            SetCenteredChild(hud.accurateWindowRect, new Vector2(60f, 34f), Vector2.zero);
            hud.accurateWindowImage = accurateWindow.GetComponent<Image>();

            var judgementLine = CreateRuntimeImage(track.transform, "JudgementLine", new Color(0.38f, 0.78f, 0.96f, 1f));
            hud.judgementLine = judgementLine.GetComponent<RectTransform>();
            SetCenteredChild(hud.judgementLine, new Vector2(6f, 56f), Vector2.zero);
            hud.judgementLineImage = judgementLine.GetComponent<Image>();

            var marker = CreateRuntimeImage(track.transform, "SignalMarker", new Color(0.38f, 0.78f, 0.96f, 1f));
            hud.signalMarker = marker.GetComponent<RectTransform>();
            SetCenteredChild(hud.signalMarker, new Vector2(28f, 28f), new Vector2(-226f, 0f));
            hud.signalMarkerImage = marker.GetComponent<Image>();

            hud.suppressionSlider = CreateRuntimeSlider(panel.transform, "SignalSuppressionSlider", new Vector2(0f, -184f), new Vector2(340f, 12f), new Color(0.45f, 0.96f, 0.58f, 1f));

            panel.SetActive(false);
            return hud;
        }

        public void SelectDormancy()
        {
            session?.SelectMutation(MutationType.Dormancy);
        }

        public void SelectNeuralControl()
        {
            session?.SelectMutation(MutationType.NeuralControl);
        }

        public void SelectMammalAdaptation()
        {
            session?.SelectMutation(MutationType.MammalAdaptation);
        }

        public void RetryVirusMinigame()
        {
            session?.ReturnToRatHostAfterVirusFailure();
        }

        private string BuildMutationStatus(MutationLoadout loadout)
        {
            mutationBuilder.Clear();

            AppendMutation(loadout, MutationType.Dormancy);
            AppendMutation(loadout, MutationType.NeuralControl);
            AppendMutation(loadout, MutationType.MammalAdaptation);

            return mutationBuilder.Length == 0 ? "획득 변이 없음" : mutationBuilder.ToString();
        }

        private void AppendMutation(MutationLoadout loadout, MutationType type)
        {
            if (!loadout.Has(type))
            {
                return;
            }

            if (mutationBuilder.Length > 0)
            {
                mutationBuilder.Append(" / ");
            }

            mutationBuilder.Append(MutationDefinition.GetName(type));
        }

        private static string GetModeName(PrototypeGameMode mode)
        {
            switch (mode)
            {
                case PrototypeGameMode.RatHost:
                    return "쥐 숙주";
                case PrototypeGameMode.InternalVirus:
                    return "내부 바이러스";
                case PrototypeGameMode.VirusFailed:
                    return "면역 반응 돌파 실패";
                case PrototypeGameMode.MutationSelection:
                    return "변이 선택";
                default:
                    return mode.ToString();
            }
        }

        private static string GetObjective(PrototypeSessionState state)
        {
            switch (state.Mode)
            {
                case PrototypeGameMode.RatHost:
                    if (state.HasImmuneAlertFeedback)
                    {
                        return state.LastImmuneAlertFeedbackText;
                    }

                    if (state.IsRatRiskInteractionAvailable)
                    {
                        return state.RatRiskInteractionPrompt;
                    }

                    return state.Mutations.CanUseMammalPassage ? "포유류 통로 개방됨" : "하수도 탐색 중";
                case PrototypeGameMode.InternalVirus:
                    if (state.HasVirusPatternExposureFeedback)
                    {
                        return state.LastVirusPatternExposureFeedbackText;
                    }

                    return state.InternalMinigameObjectiveText;
                case PrototypeGameMode.VirusFailed:
                    return "보상 없이 쥐 숙주로 복귀";
                case PrototypeGameMode.MutationSelection:
                    if (state.HasVirusPatternExposure)
                    {
                        return state.VirusPatternExposureSummaryText;
                    }

                    return "변이 선택";
                default:
                    return string.Empty;
            }
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

        private static void EnsureEventSystem()
        {
            if (UnityEngine.Object.FindAnyObjectByType<EventSystem>(FindObjectsInactive.Include) != null)
            {
                return;
            }

            var eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<InputSystemUIInputModule>();
        }

        private static GameObject CreateRuntimeImage(Transform parent, string name, Color color)
        {
            var gameObject = new GameObject(name);
            gameObject.transform.SetParent(parent, false);
            var rect = gameObject.AddComponent<RectTransform>();
            rect.sizeDelta = Vector2.zero;
            var image = gameObject.AddComponent<Image>();
            image.color = color;
            return gameObject;
        }

        private static Text CreateRuntimeText(Transform parent, string name, string text, int fontSize, TextAnchor anchor, Vector2 anchoredPosition, Vector2 size)
        {
            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            var rect = textObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;

            var uiText = textObject.AddComponent<Text>();
            uiText.text = text;
            uiText.fontSize = fontSize;
            uiText.alignment = anchor;
            uiText.color = Color.white;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.horizontalOverflow = HorizontalWrapMode.Wrap;
            uiText.verticalOverflow = VerticalWrapMode.Truncate;
            return uiText;
        }

        private static Slider CreateRuntimeSlider(Transform parent, string name, Vector2 anchoredPosition, Vector2 size, Color fillColor)
        {
            var sliderObject = CreateRuntimeImage(parent, name, new Color(1f, 1f, 1f, 0.12f));
            var rect = sliderObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;

            var fillArea = new GameObject("FillArea");
            fillArea.transform.SetParent(sliderObject.transform, false);
            var fillAreaRect = fillArea.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = Vector2.zero;
            fillAreaRect.anchorMax = Vector2.one;
            fillAreaRect.offsetMin = Vector2.zero;
            fillAreaRect.offsetMax = Vector2.zero;

            var fill = CreateRuntimeImage(fillArea.transform, "Fill", fillColor);
            var fillRect = fill.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;

            var slider = sliderObject.AddComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 0f;
            slider.transition = Selectable.Transition.None;
            slider.interactable = false;
            slider.fillRect = fillRect;
            slider.targetGraphic = sliderObject.GetComponent<Image>();
            return slider;
        }

        private static void SetCentered(RectTransform rect, Vector2 size)
        {
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = size;
        }

        private static void SetCenteredChild(RectTransform rect, Vector2 size, Vector2 anchoredPosition)
        {
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
        }
    }
}
