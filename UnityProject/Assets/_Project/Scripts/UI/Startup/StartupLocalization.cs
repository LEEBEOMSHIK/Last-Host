using System;
using System.Collections.Generic;

namespace LastHost.Prototype.UI.Startup
{
    public enum StartupLanguage
    {
        Korean = 0,
        English = 1
    }

    public enum StartupTextKey
    {
        GameTitle,
        GameTagline,
        StartPrototype,
        OpenSettings,
        Quit,
        SettingsTitle,
        LanguageLabel,
        LanguageKorean,
        LanguageEnglish,
        DisplayModeLabel,
        DisplayModeExclusiveFullScreen,
        DisplayModeFullScreenWindow,
        DisplayModeMaximizedWindow,
        DisplayModeWindowed,
        ResolutionLabel,
        VSyncLabel,
        VSyncOn,
        VSyncOff,
        ControlsTitle,
        ControlsMove,
        ControlsInteract,
        Apply,
        Cancel,
        Defaults,
        Back,
        InvalidSettings,
        ScreenApplyFailed,
        SettingsSaveFailed
    }

    public interface IStartupLocalizationCatalog
    {
        bool TryGetText(StartupLanguage language, StartupTextKey key, out string text);
        IReadOnlyCollection<StartupTextKey> GetKeys(StartupLanguage language);
    }

    public sealed class StartupLocalizationCatalog : IStartupLocalizationCatalog
    {
        private static readonly IReadOnlyDictionary<StartupTextKey, string> Korean =
            new Dictionary<StartupTextKey, string>
            {
                { StartupTextKey.GameTitle, "마지막 숙주" },
                { StartupTextKey.GameTagline, "감염하고, 적응하고, 살아남아라." },
                { StartupTextKey.StartPrototype, "프로토타입 시작" },
                { StartupTextKey.OpenSettings, "설정" },
                { StartupTextKey.Quit, "종료" },
                { StartupTextKey.SettingsTitle, "설정" },
                { StartupTextKey.LanguageLabel, "언어" },
                { StartupTextKey.LanguageKorean, "한국어" },
                { StartupTextKey.LanguageEnglish, "English" },
                { StartupTextKey.DisplayModeLabel, "화면 모드" },
                { StartupTextKey.DisplayModeExclusiveFullScreen, "전체 화면" },
                { StartupTextKey.DisplayModeFullScreenWindow, "테두리 없는 전체 화면" },
                { StartupTextKey.DisplayModeMaximizedWindow, "최대화 창" },
                { StartupTextKey.DisplayModeWindowed, "창 모드" },
                { StartupTextKey.ResolutionLabel, "해상도" },
                { StartupTextKey.VSyncLabel, "수직 동기화" },
                { StartupTextKey.VSyncOn, "켜기" },
                { StartupTextKey.VSyncOff, "끄기" },
                { StartupTextKey.ControlsTitle, "조작 안내" },
                { StartupTextKey.ControlsMove, "이동: WASD" },
                { StartupTextKey.ControlsInteract, "상호작용 / 확인: Space" },
                { StartupTextKey.Apply, "적용" },
                { StartupTextKey.Cancel, "취소" },
                { StartupTextKey.Defaults, "기본값" },
                { StartupTextKey.Back, "뒤로" },
                { StartupTextKey.InvalidSettings, "지원하지 않는 설정입니다." },
                { StartupTextKey.ScreenApplyFailed, "화면 설정을 적용하지 못했습니다." },
                { StartupTextKey.SettingsSaveFailed, "설정을 저장하지 못했습니다." }
            };

        private static readonly IReadOnlyDictionary<StartupTextKey, string> English =
            new Dictionary<StartupTextKey, string>
            {
                { StartupTextKey.GameTitle, "The Last Host" },
                { StartupTextKey.GameTagline, "Infect. Adapt. Survive." },
                { StartupTextKey.StartPrototype, "Start Prototype" },
                { StartupTextKey.OpenSettings, "Settings" },
                { StartupTextKey.Quit, "Quit" },
                { StartupTextKey.SettingsTitle, "Settings" },
                { StartupTextKey.LanguageLabel, "Language" },
                { StartupTextKey.LanguageKorean, "한국어" },
                { StartupTextKey.LanguageEnglish, "English" },
                { StartupTextKey.DisplayModeLabel, "Display Mode" },
                { StartupTextKey.DisplayModeExclusiveFullScreen, "Exclusive Fullscreen" },
                { StartupTextKey.DisplayModeFullScreenWindow, "Borderless Fullscreen" },
                { StartupTextKey.DisplayModeMaximizedWindow, "Maximized Window" },
                { StartupTextKey.DisplayModeWindowed, "Windowed" },
                { StartupTextKey.ResolutionLabel, "Resolution" },
                { StartupTextKey.VSyncLabel, "VSync" },
                { StartupTextKey.VSyncOn, "On" },
                { StartupTextKey.VSyncOff, "Off" },
                { StartupTextKey.ControlsTitle, "Controls" },
                { StartupTextKey.ControlsMove, "Move: WASD" },
                { StartupTextKey.ControlsInteract, "Interact / Confirm: Space" },
                { StartupTextKey.Apply, "Apply" },
                { StartupTextKey.Cancel, "Cancel" },
                { StartupTextKey.Defaults, "Defaults" },
                { StartupTextKey.Back, "Back" },
                { StartupTextKey.InvalidSettings, "These settings are not supported." },
                { StartupTextKey.ScreenApplyFailed, "The display settings could not be applied." },
                { StartupTextKey.SettingsSaveFailed, "The settings could not be saved." }
            };

        public static StartupLocalizationCatalog Default { get; } = new StartupLocalizationCatalog();

        public bool TryGetText(StartupLanguage language, StartupTextKey key, out string text)
        {
            switch (language)
            {
                case StartupLanguage.Korean:
                    return Korean.TryGetValue(key, out text);
                case StartupLanguage.English:
                    return English.TryGetValue(key, out text);
                default:
                    text = null;
                    return false;
            }
        }

        public IReadOnlyCollection<StartupTextKey> GetKeys(StartupLanguage language)
        {
            switch (language)
            {
                case StartupLanguage.Korean:
                    return new List<StartupTextKey>(Korean.Keys);
                case StartupLanguage.English:
                    return new List<StartupTextKey>(English.Keys);
                default:
                    return Array.Empty<StartupTextKey>();
            }
        }
    }

    public interface IStartupLocalizer
    {
        event Action LanguageChanged;

        StartupLanguage Language { get; }
        string GetText(StartupTextKey key);
        void SetLanguage(StartupLanguage language);
    }

    public sealed class StartupLocalizer : IStartupLocalizer
    {
        private readonly IStartupLocalizationCatalog catalog;

        public StartupLocalizer()
            : this(StartupLocalizationCatalog.Default, StartupSettingsDefaults.DefaultLanguage)
        {
        }

        public StartupLocalizer(IStartupLocalizationCatalog catalog, StartupLanguage initialLanguage)
        {
            this.catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            Language = initialLanguage;
        }

        public event Action LanguageChanged;

        public StartupLanguage Language { get; private set; }

        public string GetText(StartupTextKey key)
        {
            if (catalog.TryGetText(Language, key, out var requestedText))
            {
                return requestedText;
            }

            if (catalog.TryGetText(StartupSettingsDefaults.DefaultLanguage, key, out var fallbackText))
            {
                return fallbackText;
            }

            return $"[Missing:{key}]";
        }

        public void SetLanguage(StartupLanguage language)
        {
            if (Language == language)
            {
                return;
            }

            Language = language;
            LanguageChanged?.Invoke();
        }
    }
}
