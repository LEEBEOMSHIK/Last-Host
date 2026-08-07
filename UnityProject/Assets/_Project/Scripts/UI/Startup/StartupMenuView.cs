using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace LastHost.Prototype.UI.Startup
{
    [DisallowMultipleComponent]
    public sealed class StartupMenuView : MonoBehaviour
    {
        [SerializeField]
        private Sprite startupBackground;

        [SerializeField]
        private Font koreanFont;

        [SerializeField]
        private Font englishFont;

        public const string MissingBackgroundDiagnosticId = "[StartupUI:PFC6_MISSING_BACKGROUND]";
        public const string MissingKoreanFontDiagnosticId = "[StartupUI:PFC6_MISSING_FONT_KO]";
        public const string MissingEnglishFontDiagnosticId = "[StartupUI:PFC6_MISSING_FONT_EN]";

        public static readonly Color DiagnosticFallbackBackgroundColor =
            new Color(0.12f, 0.035f, 0.16f, 1f);

        private static readonly StartupLanguage[] LanguageOptions =
        {
            StartupLanguage.Korean,
            StartupLanguage.English
        };

        private static readonly StartupDisplayMode[] DisplayModeOptions =
        {
            StartupDisplayMode.ExclusiveFullScreen,
            StartupDisplayMode.FullScreenWindow,
            StartupDisplayMode.MaximizedWindow,
            StartupDisplayMode.Windowed
        };

        private readonly List<StartupResolution> resolutionOptions = new List<StartupResolution>();

        private StartupController controller;
        private Font activeFont;
        private Font diagnosticFallbackFont;
        private bool uiBuilt;
        private bool eventsSubscribed;
        private bool rendering;
        private bool missingBackgroundReported;
        private bool missingKoreanFontReported;
        private bool missingEnglishFontReported;

        private GameObject mainPanel;
        private GameObject settingsPanel;

        private Text titleText;
        private Text taglineText;
        private Text startButtonText;
        private Text settingsButtonText;
        private Text quitButtonText;

        private Text settingsTitleText;
        private Text languageLabelText;
        private Text displayModeLabelText;
        private Text resolutionLabelText;
        private Text vSyncLabelText;
        private Text vSyncValueText;
        private Text controlsTitleText;
        private Text controlsMoveText;
        private Text controlsInteractText;
        private Text applyButtonText;
        private Text cancelButtonText;
        private Text defaultsButtonText;
        private Text errorText;

        private Button startButton;
        private Button settingsButton;
        private Button quitButton;
        private Button applyButton;
        private Button cancelButton;
        private Button defaultsButton;
        private Dropdown languageDropdown;
        private Dropdown displayModeDropdown;
        private Dropdown resolutionDropdown;
        private Toggle vSyncToggle;

        public Sprite StartupBackground => startupBackground;
        public Font KoreanFont => koreanFont;
        public Font EnglishFont => englishFont;
        public Font ActiveFont => activeFont;

        private void Awake()
        {
            activeFont = ResolveFontProfile(StartupSettingsDefaults.DefaultLanguage);
            BuildUi();
            controller = StartupController.CreateDefault();
            Render();
        }

        private void OnEnable()
        {
            SubscribeToController();
            Render();
        }

        private void OnDisable()
        {
            UnsubscribeFromController();
        }

        private void OnDestroy()
        {
            UnsubscribeFromController();
            RemoveUiCallbacks();
        }

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            {
                controller?.HandleEscape();
            }
        }

        private void BuildUi()
        {
            if (uiBuilt)
            {
                return;
            }

            uiBuilt = true;
            EnsureEventSystem();

            var canvasObject = new GameObject(
                "StartupCanvas",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);

            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = true;

            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(960f, 540f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            var background = CreateImage(
                canvasObject.transform,
                "Background",
                startupBackground == null ? DiagnosticFallbackBackgroundColor : Color.white);
            var backgroundImage = background.GetComponent<Image>();
            backgroundImage.sprite = startupBackground;
            backgroundImage.raycastTarget = false;
            Stretch(background.GetComponent<RectTransform>());

            if (startupBackground == null)
            {
                ReportMissingBackground();
            }

            mainPanel = CreatePanel(canvasObject.transform, "MainMenuPanel", new Vector2(360f, 420f));
            SetCentered(mainPanel.GetComponent<RectTransform>(), new Vector2(-285f, 0f), new Vector2(360f, 420f));
            mainPanel.GetComponent<Image>().color = new Color(0.035f, 0.05f, 0.065f, 0.74f);
            BuildMainMenu(mainPanel.transform);

            settingsPanel = CreatePanel(canvasObject.transform, "SettingsPanel", new Vector2(880f, 510f));
            BuildSettingsMenu(settingsPanel.transform);
        }

        private void BuildMainMenu(Transform parent)
        {
            titleText = CreateText(parent, "GameTitle", 44, TextAnchor.MiddleCenter, new Vector2(0f, 125f), new Vector2(332f, 72f));
            taglineText = CreateText(parent, "GameTagline", 21, TextAnchor.MiddleCenter, new Vector2(0f, 65f), new Vector2(326f, 54f));

            startButton = CreateButton(parent, "StartPrototypeButton", new Vector2(0f, -20f), new Vector2(300f, 52f), out startButtonText);
            settingsButton = CreateButton(parent, "OpenSettingsButton", new Vector2(0f, -88f), new Vector2(300f, 52f), out settingsButtonText);
            quitButton = CreateButton(parent, "QuitButton", new Vector2(0f, -156f), new Vector2(300f, 52f), out quitButtonText);

            startButton.onClick.AddListener(OnStartPrototypeClicked);
            settingsButton.onClick.AddListener(OnOpenSettingsClicked);
            quitButton.onClick.AddListener(OnQuitClicked);
        }

        private void BuildSettingsMenu(Transform parent)
        {
            settingsTitleText = CreateText(parent, "SettingsTitle", 36, TextAnchor.MiddleCenter, new Vector2(0f, 220f), new Vector2(760f, 48f));

            languageLabelText = CreateRowLabel(parent, "LanguageLabel", 145f);
            languageDropdown = CreateDropdown(parent, "LanguageDropdown", new Vector2(120f, 145f), new Vector2(430f, 44f));

            displayModeLabelText = CreateRowLabel(parent, "DisplayModeLabel", 85f);
            displayModeDropdown = CreateDropdown(parent, "DisplayModeDropdown", new Vector2(120f, 85f), new Vector2(430f, 44f));

            resolutionLabelText = CreateRowLabel(parent, "ResolutionLabel", 25f);
            resolutionDropdown = CreateDropdown(parent, "ResolutionDropdown", new Vector2(120f, 25f), new Vector2(430f, 44f));

            vSyncLabelText = CreateRowLabel(parent, "VSyncLabel", -35f);
            vSyncToggle = CreateToggle(parent, "VSyncToggle", new Vector2(120f, -35f), new Vector2(430f, 44f), out vSyncValueText);

            controlsTitleText = CreateText(parent, "ControlsTitle", 20, TextAnchor.MiddleLeft, new Vector2(-260f, -108f), new Vector2(220f, 34f));
            controlsMoveText = CreateText(parent, "ControlsMove", 19, TextAnchor.MiddleLeft, new Vector2(120f, -96f), new Vector2(430f, 28f));
            controlsInteractText = CreateText(parent, "ControlsInteract", 19, TextAnchor.MiddleLeft, new Vector2(120f, -128f), new Vector2(430f, 28f));

            errorText = CreateText(parent, "ApplyError", 17, TextAnchor.MiddleCenter, new Vector2(0f, -168f), new Vector2(760f, 28f));
            errorText.color = new Color(1f, 0.48f, 0.42f, 1f);

            defaultsButton = CreateButton(parent, "DefaultsButton", new Vector2(-220f, -218f), new Vector2(190f, 46f), out defaultsButtonText);
            cancelButton = CreateButton(parent, "CancelButton", new Vector2(0f, -218f), new Vector2(190f, 46f), out cancelButtonText);
            applyButton = CreateButton(parent, "ApplyButton", new Vector2(220f, -218f), new Vector2(190f, 46f), out applyButtonText);

            languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
            displayModeDropdown.onValueChanged.AddListener(OnDisplayModeChanged);
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
            vSyncToggle.onValueChanged.AddListener(OnVSyncChanged);
            defaultsButton.onClick.AddListener(OnDefaultsClicked);
            cancelButton.onClick.AddListener(OnCancelClicked);
            applyButton.onClick.AddListener(OnApplyClicked);
        }

        private void SubscribeToController()
        {
            if (controller == null || eventsSubscribed)
            {
                return;
            }

            controller.StateChanged += Render;
            controller.Localizer.LanguageChanged += Render;
            eventsSubscribed = true;
        }

        private void UnsubscribeFromController()
        {
            if (controller == null || !eventsSubscribed)
            {
                return;
            }

            controller.StateChanged -= Render;
            controller.Localizer.LanguageChanged -= Render;
            eventsSubscribed = false;
        }

        private void RemoveUiCallbacks()
        {
            startButton?.onClick.RemoveListener(OnStartPrototypeClicked);
            settingsButton?.onClick.RemoveListener(OnOpenSettingsClicked);
            quitButton?.onClick.RemoveListener(OnQuitClicked);
            applyButton?.onClick.RemoveListener(OnApplyClicked);
            cancelButton?.onClick.RemoveListener(OnCancelClicked);
            defaultsButton?.onClick.RemoveListener(OnDefaultsClicked);
            languageDropdown?.onValueChanged.RemoveListener(OnLanguageChanged);
            displayModeDropdown?.onValueChanged.RemoveListener(OnDisplayModeChanged);
            resolutionDropdown?.onValueChanged.RemoveListener(OnResolutionChanged);
            vSyncToggle?.onValueChanged.RemoveListener(OnVSyncChanged);
        }

        private void Render()
        {
            if (controller == null || !uiBuilt || rendering)
            {
                return;
            }

            rendering = true;
            try
            {
                var localizer = controller.Localizer;
                ApplyFontProfile(localizer.Language);
                titleText.text = localizer.GetText(StartupTextKey.GameTitle);
                taglineText.text = localizer.GetText(StartupTextKey.GameTagline);
                startButtonText.text = localizer.GetText(StartupTextKey.StartPrototype);
                settingsButtonText.text = localizer.GetText(StartupTextKey.OpenSettings);
                quitButtonText.text = localizer.GetText(StartupTextKey.Quit);

                settingsTitleText.text = localizer.GetText(StartupTextKey.SettingsTitle);
                languageLabelText.text = localizer.GetText(StartupTextKey.LanguageLabel);
                displayModeLabelText.text = localizer.GetText(StartupTextKey.DisplayModeLabel);
                resolutionLabelText.text = localizer.GetText(StartupTextKey.ResolutionLabel);
                vSyncLabelText.text = localizer.GetText(StartupTextKey.VSyncLabel);
                controlsTitleText.text = localizer.GetText(StartupTextKey.ControlsTitle);
                controlsMoveText.text = localizer.GetText(StartupTextKey.ControlsMove);
                controlsInteractText.text = localizer.GetText(StartupTextKey.ControlsInteract);
                applyButtonText.text = localizer.GetText(StartupTextKey.Apply);
                cancelButtonText.text = localizer.GetText(StartupTextKey.Cancel);
                defaultsButtonText.text = localizer.GetText(StartupTextKey.Defaults);

                RenderLanguageOptions(localizer);
                RenderDisplayModeOptions(localizer);
                RenderResolutionOptions();
                RenderDraftValues(localizer);
                RenderApplyError(localizer);

                var showingSettings = controller.Panel == StartupPanel.Settings;
                mainPanel.SetActive(!showingSettings);
                settingsPanel.SetActive(showingSettings);
            }
            finally
            {
                rendering = false;
            }
        }

        private void RenderLanguageOptions(IStartupLocalizer localizer)
        {
            languageDropdown.ClearOptions();
            languageDropdown.AddOptions(new List<Dropdown.OptionData>
            {
                new Dropdown.OptionData(localizer.GetText(StartupTextKey.LanguageKorean)),
                new Dropdown.OptionData(localizer.GetText(StartupTextKey.LanguageEnglish))
            });
        }

        private void RenderDisplayModeOptions(IStartupLocalizer localizer)
        {
            displayModeDropdown.ClearOptions();
            displayModeDropdown.AddOptions(new List<Dropdown.OptionData>
            {
                new Dropdown.OptionData(localizer.GetText(StartupTextKey.DisplayModeExclusiveFullScreen)),
                new Dropdown.OptionData(localizer.GetText(StartupTextKey.DisplayModeFullScreenWindow)),
                new Dropdown.OptionData(localizer.GetText(StartupTextKey.DisplayModeMaximizedWindow)),
                new Dropdown.OptionData(localizer.GetText(StartupTextKey.DisplayModeWindowed))
            });
        }

        private void RenderResolutionOptions()
        {
            resolutionOptions.Clear();
            resolutionOptions.AddRange(controller.AvailableResolutions);

            var optionData = new List<Dropdown.OptionData>(resolutionOptions.Count);
            foreach (var resolution in resolutionOptions)
            {
                optionData.Add(new Dropdown.OptionData(FormatResolution(resolution)));
            }

            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(optionData);
        }

        private void RenderDraftValues(IStartupLocalizer localizer)
        {
            var draft = controller.Draft;
            if (draft == null)
            {
                return;
            }

            languageDropdown.SetValueWithoutNotify(IndexOf(LanguageOptions, draft.Language));
            languageDropdown.RefreshShownValue();

            displayModeDropdown.SetValueWithoutNotify(IndexOf(DisplayModeOptions, draft.DisplayMode));
            displayModeDropdown.RefreshShownValue();

            var resolutionIndex = resolutionOptions.IndexOf(draft.Resolution);
            resolutionDropdown.SetValueWithoutNotify(Mathf.Max(0, resolutionIndex));
            resolutionDropdown.RefreshShownValue();

            var vSyncEnabled = draft.VSyncCount == 1;
            vSyncToggle.SetIsOnWithoutNotify(vSyncEnabled);
            vSyncValueText.text = localizer.GetText(vSyncEnabled ? StartupTextKey.VSyncOn : StartupTextKey.VSyncOff);
        }

        private void RenderApplyError(IStartupLocalizer localizer)
        {
            if (TryGetApplyErrorKey(controller.LastApplyResult, out var key))
            {
                errorText.text = localizer.GetText(key);
                errorText.gameObject.SetActive(true);
                return;
            }

            errorText.text = string.Empty;
            errorText.gameObject.SetActive(false);
        }

        private void OnStartPrototypeClicked()
        {
            controller?.StartPrototype();
        }

        private void OnOpenSettingsClicked()
        {
            controller?.OpenSettings();
        }

        private void OnQuitClicked()
        {
            controller?.RequestQuit();
        }

        private void OnApplyClicked()
        {
            controller?.ApplySettings();
        }

        private void OnCancelClicked()
        {
            controller?.CancelSettings();
        }

        private void OnDefaultsClicked()
        {
            controller?.UseDefaults();
        }

        private void OnLanguageChanged(int index)
        {
            if (!rendering && controller?.Draft != null && IsValidIndex(LanguageOptions, index))
            {
                controller.SetDraftLanguage(LanguageOptions[index]);
            }
        }

        private void OnDisplayModeChanged(int index)
        {
            if (!rendering && controller?.Draft != null && IsValidIndex(DisplayModeOptions, index))
            {
                controller.SetDraftDisplayMode(DisplayModeOptions[index]);
            }
        }

        private void OnResolutionChanged(int index)
        {
            if (!rendering && controller?.Draft != null && index >= 0 && index < resolutionOptions.Count)
            {
                controller.SetDraftResolution(resolutionOptions[index]);
            }
        }

        private void OnVSyncChanged(bool isOn)
        {
            if (!rendering && controller?.Draft != null)
            {
                controller.SetDraftVSyncCount(isOn ? 1 : 0);
            }
        }

        private Text CreateRowLabel(Transform parent, string name, float y)
        {
            return CreateText(parent, name, 20, TextAnchor.MiddleLeft, new Vector2(-260f, y), new Vector2(220f, 40f));
        }

        private GameObject CreatePanel(Transform parent, string name, Vector2 size)
        {
            var panel = CreateImage(parent, name, new Color(0.05f, 0.075f, 0.08f, 0.96f));
            SetCentered(panel.GetComponent<RectTransform>(), Vector2.zero, size);
            return panel;
        }

        private GameObject CreateImage(Transform parent, string name, Color color)
        {
            var imageObject = new GameObject(name, typeof(RectTransform), typeof(Image));
            imageObject.transform.SetParent(parent, false);
            var image = imageObject.GetComponent<Image>();
            image.color = color;
            return imageObject;
        }

        private Text CreateText(Transform parent, string name, int fontSize, TextAnchor alignment, Vector2 position, Vector2 size)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);
            SetCentered(textObject.GetComponent<RectTransform>(), position, size);

            var text = textObject.GetComponent<Text>();
            text.font = activeFont;
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = Color.white;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            text.raycastTarget = false;
            return text;
        }

        public Font GetFontProfile(StartupLanguage language)
        {
            switch (language)
            {
                case StartupLanguage.Korean:
                    return koreanFont;
                case StartupLanguage.English:
                    return englishFont;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, "Unsupported startup language.");
            }
        }

        public static string GetMissingFontDiagnosticId(StartupLanguage language)
        {
            switch (language)
            {
                case StartupLanguage.Korean:
                    return MissingKoreanFontDiagnosticId;
                case StartupLanguage.English:
                    return MissingEnglishFontDiagnosticId;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, "Unsupported startup language.");
            }
        }

        private void ApplyFontProfile(StartupLanguage language)
        {
            activeFont = ResolveFontProfile(language);
            foreach (var text in GetComponentsInChildren<Text>(true))
            {
                text.font = activeFont;
            }
        }

        private Font ResolveFontProfile(StartupLanguage language)
        {
            var configuredFont = GetFontProfile(language);
            if (configuredFont != null)
            {
                return configuredFont;
            }

            ReportMissingFont(language);
            return GetDiagnosticFallbackFont();
        }

        private Font GetDiagnosticFallbackFont()
        {
            if (diagnosticFallbackFont != null)
            {
                return diagnosticFallbackFont;
            }

            foreach (var installedFont in Font.GetOSInstalledFontNames())
            {
                if (string.Equals(installedFont, "Malgun Gothic", StringComparison.OrdinalIgnoreCase))
                {
                    diagnosticFallbackFont = Font.CreateDynamicFontFromOSFont(installedFont, 16);
                    break;
                }
            }

            if (diagnosticFallbackFont == null)
            {
                diagnosticFallbackFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            }

            return diagnosticFallbackFont;
        }

        private void ReportMissingBackground()
        {
            if (missingBackgroundReported)
            {
                return;
            }

            missingBackgroundReported = true;
            Debug.LogError($"{MissingBackgroundDiagnosticId} Startup background Sprite is not assigned.", this);
        }

        private void ReportMissingFont(StartupLanguage language)
        {
            if (language == StartupLanguage.Korean)
            {
                if (missingKoreanFontReported)
                {
                    return;
                }

                missingKoreanFontReported = true;
            }
            else
            {
                if (missingEnglishFontReported)
                {
                    return;
                }

                missingEnglishFontReported = true;
            }

            Debug.LogError(
                $"{GetMissingFontDiagnosticId(language)} Startup font profile is not assigned. " +
                "Using the explicit diagnostic fallback only.",
                this);
        }

        private Button CreateButton(Transform parent, string name, Vector2 position, Vector2 size, out Text label)
        {
            var buttonObject = CreateImage(parent, name, new Color(0.12f, 0.25f, 0.26f, 1f));
            SetCentered(buttonObject.GetComponent<RectTransform>(), position, size);
            var button = buttonObject.AddComponent<Button>();
            button.targetGraphic = buttonObject.GetComponent<Image>();

            label = CreateText(buttonObject.transform, "Label", 21, TextAnchor.MiddleCenter, Vector2.zero, Vector2.zero);
            Stretch(label.rectTransform, new Vector2(14f, 6f), new Vector2(-14f, -6f));
            return button;
        }

        private Dropdown CreateDropdown(Transform parent, string name, Vector2 position, Vector2 size)
        {
            var dropdownObject = CreateImage(parent, name, new Color(0.09f, 0.14f, 0.15f, 1f));
            SetCentered(dropdownObject.GetComponent<RectTransform>(), position, size);
            var dropdown = dropdownObject.AddComponent<Dropdown>();
            dropdown.targetGraphic = dropdownObject.GetComponent<Image>();

            var captionText = CreateText(dropdownObject.transform, "CaptionText", 19, TextAnchor.MiddleLeft, Vector2.zero, Vector2.zero);
            Stretch(captionText.rectTransform, new Vector2(14f, 5f), new Vector2(-14f, -5f));

            var template = CreateImage(dropdownObject.transform, "Template", new Color(0.045f, 0.075f, 0.08f, 1f));
            var templateRect = template.GetComponent<RectTransform>();
            templateRect.anchorMin = new Vector2(0f, 0f);
            templateRect.anchorMax = new Vector2(1f, 0f);
            templateRect.pivot = new Vector2(0.5f, 1f);
            templateRect.anchoredPosition = new Vector2(0f, -2f);
            templateRect.sizeDelta = new Vector2(0f, 176f);

            var scrollRect = template.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;

            var viewport = CreateImage(template.transform, "Viewport", new Color(1f, 1f, 1f, 0.01f));
            var viewportRect = viewport.GetComponent<RectTransform>();
            Stretch(viewportRect, new Vector2(2f, 2f), new Vector2(-2f, -2f));
            var mask = viewport.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            var contentObject = new GameObject("Content", typeof(RectTransform));
            contentObject.transform.SetParent(viewport.transform, false);
            var contentRect = contentObject.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0f, 1f);
            contentRect.anchorMax = new Vector2(1f, 1f);
            contentRect.pivot = new Vector2(0.5f, 1f);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = new Vector2(0f, 34f);

            var itemObject = CreateImage(contentObject.transform, "Item", new Color(0.07f, 0.12f, 0.13f, 1f));
            var itemRect = itemObject.GetComponent<RectTransform>();
            itemRect.anchorMin = new Vector2(0f, 1f);
            itemRect.anchorMax = new Vector2(1f, 1f);
            itemRect.pivot = new Vector2(0.5f, 1f);
            itemRect.anchoredPosition = Vector2.zero;
            itemRect.sizeDelta = new Vector2(0f, 34f);

            var itemToggle = itemObject.AddComponent<Toggle>();
            itemToggle.targetGraphic = itemObject.GetComponent<Image>();

            var checkmarkObject = CreateImage(itemObject.transform, "Checkmark", new Color(0.35f, 0.78f, 0.62f, 1f));
            SetCentered(checkmarkObject.GetComponent<RectTransform>(), new Vector2(-199f, 0f), new Vector2(5f, 22f));
            itemToggle.graphic = checkmarkObject.GetComponent<Image>();

            var itemText = CreateText(itemObject.transform, "ItemLabel", 18, TextAnchor.MiddleLeft, Vector2.zero, Vector2.zero);
            Stretch(itemText.rectTransform, new Vector2(14f, 2f), new Vector2(-12f, -2f));

            scrollRect.viewport = viewportRect;
            scrollRect.content = contentRect;
            dropdown.captionText = captionText;
            dropdown.template = templateRect;
            dropdown.itemText = itemText;
            template.SetActive(false);
            return dropdown;
        }

        private Toggle CreateToggle(Transform parent, string name, Vector2 position, Vector2 size, out Text valueText)
        {
            var toggleObject = CreateImage(parent, name, new Color(0.09f, 0.14f, 0.15f, 1f));
            SetCentered(toggleObject.GetComponent<RectTransform>(), position, size);
            var toggle = toggleObject.AddComponent<Toggle>();

            var box = CreateImage(toggleObject.transform, "Box", new Color(0.025f, 0.045f, 0.05f, 1f));
            SetCentered(box.GetComponent<RectTransform>(), new Vector2(-188f, 0f), new Vector2(24f, 24f));
            var checkmark = CreateImage(box.transform, "Checkmark", new Color(0.35f, 0.78f, 0.62f, 1f));
            Stretch(checkmark.GetComponent<RectTransform>(), new Vector2(5f, 5f), new Vector2(-5f, -5f));

            toggle.targetGraphic = box.GetComponent<Image>();
            toggle.graphic = checkmark.GetComponent<Image>();

            valueText = CreateText(toggleObject.transform, "Value", 19, TextAnchor.MiddleLeft, new Vector2(20f, 0f), new Vector2(340f, 40f));
            return toggle;
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

        private static string FormatResolution(StartupResolution resolution)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} × {1}", resolution.Width, resolution.Height);
        }

        private static bool TryGetApplyErrorKey(StartupApplyResult result, out StartupTextKey key)
        {
            switch (result)
            {
                case StartupApplyResult.NoDraft:
                case StartupApplyResult.InvalidDraft:
                    key = StartupTextKey.InvalidSettings;
                    return true;
                case StartupApplyResult.ScreenApplyFailed:
                    key = StartupTextKey.ScreenApplyFailed;
                    return true;
                case StartupApplyResult.SettingsSaveFailed:
                    key = StartupTextKey.SettingsSaveFailed;
                    return true;
                default:
                    key = default;
                    return false;
            }
        }

        private static int IndexOf<T>(IReadOnlyList<T> values, T value)
        {
            var comparer = EqualityComparer<T>.Default;
            for (var index = 0; index < values.Count; index++)
            {
                if (comparer.Equals(values[index], value))
                {
                    return index;
                }
            }

            return 0;
        }

        private static bool IsValidIndex<T>(IReadOnlyList<T> values, int index)
        {
            return index >= 0 && index < values.Count;
        }

        private static void SetCentered(RectTransform rect, Vector2 position, Vector2 size)
        {
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        private static void Stretch(RectTransform rect)
        {
            Stretch(rect, Vector2.zero, Vector2.zero);
        }

        private static void Stretch(RectTransform rect, Vector2 offsetMin, Vector2 offsetMax)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
        }
    }
}
