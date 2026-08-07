using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastHost.Prototype.UI.Startup
{
    public interface IStartupSettingsRepository
    {
        bool TryLoad(out StartupSettings settings);
        bool TrySave(StartupSettings settings);
    }

    public sealed class PlayerPrefsStartupSettingsRepository : IStartupSettingsRepository
    {
        public const string DefaultKeyPrefix = "last_host.startup.settings.v1";

        private static readonly string[] RequiredSuffixes =
        {
            "schema",
            "language",
            "displayMode",
            "width",
            "height",
            "vSync"
        };

        public PlayerPrefsStartupSettingsRepository()
            : this(DefaultKeyPrefix)
        {
        }

        public PlayerPrefsStartupSettingsRepository(string keyPrefix)
        {
            if (string.IsNullOrWhiteSpace(keyPrefix))
            {
                throw new ArgumentException("A PlayerPrefs key prefix is required.", nameof(keyPrefix));
            }

            KeyPrefix = keyPrefix.Trim().TrimEnd('.');
        }

        public string KeyPrefix { get; }

        public bool TryLoad(out StartupSettings settings)
        {
            settings = null;
            if (RequiredSuffixes.Any(suffix => !PlayerPrefs.HasKey(GetKey(suffix))))
            {
                return false;
            }

            if (!TryReadInt("schema", out var schema) ||
                !TryReadInt("language", out var language) ||
                !TryReadInt("displayMode", out var displayMode) ||
                !TryReadInt("width", out var width) ||
                !TryReadInt("height", out var height) ||
                !TryReadInt("vSync", out var vSync))
            {
                return false;
            }

            var candidate = new StartupSettings(
                schema,
                (StartupLanguage)language,
                (StartupDisplayMode)displayMode,
                new StartupResolution(width, height),
                vSync);

            if (candidate.SchemaVersion != StartupSettings.CurrentSchemaVersion ||
                !Enum.IsDefined(typeof(StartupLanguage), candidate.Language) ||
                !Enum.IsDefined(typeof(StartupDisplayMode), candidate.DisplayMode) ||
                !candidate.Resolution.IsValid ||
                (candidate.VSyncCount != 0 && candidate.VSyncCount != 1))
            {
                return false;
            }

            settings = candidate;
            return true;
        }

        public bool TrySave(StartupSettings settings)
        {
            if (settings == null)
            {
                return false;
            }

            try
            {
                WriteInt("schema", settings.SchemaVersion);
                WriteInt("language", (int)settings.Language);
                WriteInt("displayMode", (int)settings.DisplayMode);
                WriteInt("width", settings.Resolution.Width);
                WriteInt("height", settings.Resolution.Height);
                WriteInt("vSync", settings.VSyncCount);
                PlayerPrefs.Save();
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Startup settings could not be saved: {exception.Message}");
                return false;
            }
        }

        public string GetKey(string suffix)
        {
            return $"{KeyPrefix}.{suffix}";
        }

        private bool TryReadInt(string suffix, out int value)
        {
            return int.TryParse(
                PlayerPrefs.GetString(GetKey(suffix), string.Empty),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out value);
        }

        private void WriteInt(string suffix, int value)
        {
            PlayerPrefs.SetString(GetKey(suffix), value.ToString(CultureInfo.InvariantCulture));
        }
    }

    public interface IStartupScreenPlatform
    {
        IReadOnlyList<StartupResolution> SupportedResolutions { get; }
        StartupResolution CurrentResolution { get; }
        bool TryApply(StartupSettings settings);
    }

    public interface IStartupScenePlatform
    {
        void LoadScene(string scenePath);
    }

    public interface IStartupQuitPlatform
    {
        void RequestQuit();
    }

    public sealed class StartupScreenPlatform : IStartupScreenPlatform
    {
        public IReadOnlyList<StartupResolution> SupportedResolutions =>
            StartupSettingsDefaults.NormalizeSupportedResolutions(
                Screen.resolutions.Select(resolution => new StartupResolution(resolution.width, resolution.height)));

        public StartupResolution CurrentResolution =>
            new StartupResolution(Screen.currentResolution.width, Screen.currentResolution.height);

        public bool TryApply(StartupSettings settings)
        {
            if (settings == null)
            {
                return false;
            }

            try
            {
                Screen.SetResolution(
                    settings.Resolution.Width,
                    settings.Resolution.Height,
                    ToUnityMode(settings.DisplayMode));
                QualitySettings.vSyncCount = settings.VSyncCount;
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Startup display settings could not be applied: {exception.Message}");
                return false;
            }
        }

        private static FullScreenMode ToUnityMode(StartupDisplayMode mode)
        {
            switch (mode)
            {
                case StartupDisplayMode.ExclusiveFullScreen:
                    return FullScreenMode.ExclusiveFullScreen;
                case StartupDisplayMode.FullScreenWindow:
                    return FullScreenMode.FullScreenWindow;
                case StartupDisplayMode.MaximizedWindow:
                    return FullScreenMode.MaximizedWindow;
                case StartupDisplayMode.Windowed:
                    return FullScreenMode.Windowed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unsupported display mode.");
            }
        }
    }

    public sealed class StartupScenePlatform : IStartupScenePlatform
    {
        public void LoadScene(string scenePath)
        {
            SceneManager.LoadScene(scenePath);
        }
    }

    public sealed class StartupQuitPlatform : IStartupQuitPlatform
    {
        public void RequestQuit()
        {
#if UNITY_EDITOR
            Debug.Log("Quit was requested from Startup. Application.Quit is skipped in the Editor.");
#else
            Application.Quit();
#endif
        }
    }

    public static class StartupSceneContract
    {
        public const string StartupScenePath = "Assets/_Project/Scenes/Startup.unity";
        public const string PrototypeScenePath = "Assets/_Project/Scenes/RatHost2DPrototype.unity";
    }

    public enum StartupPanel
    {
        MainMenu,
        Settings
    }

    public enum StartupApplyResult
    {
        None,
        Applied,
        NoDraft,
        InvalidDraft,
        ScreenApplyFailed,
        SettingsSaveFailed
    }

    public sealed class StartupController
    {
        private readonly IStartupSettingsRepository repository;
        private readonly IStartupScreenPlatform screenPlatform;
        private readonly IStartupScenePlatform scenePlatform;
        private readonly IStartupQuitPlatform quitPlatform;

        public StartupController(
            IStartupSettingsRepository repository,
            IStartupScreenPlatform screenPlatform,
            IStartupScenePlatform scenePlatform,
            IStartupQuitPlatform quitPlatform,
            IStartupLocalizer localizer)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.screenPlatform = screenPlatform ?? throw new ArgumentNullException(nameof(screenPlatform));
            this.scenePlatform = scenePlatform ?? throw new ArgumentNullException(nameof(scenePlatform));
            this.quitPlatform = quitPlatform ?? throw new ArgumentNullException(nameof(quitPlatform));
            Localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

            RefreshResolutionContract();
            Panel = StartupPanel.MainMenu;
            LastApplyResult = StartupApplyResult.None;
            InitializeSettings();
        }

        public event Action StateChanged;

        public StartupPanel Panel { get; private set; }
        public StartupSettings SavedSettings { get; private set; }
        public StartupSettings DefaultSettings { get; private set; }
        public StartupSettingsDraft Draft { get; private set; }
        public IReadOnlyList<StartupResolution> AvailableResolutions { get; private set; }
        public IStartupLocalizer Localizer { get; }
        public StartupApplyResult LastApplyResult { get; private set; }
        public bool InitializationSucceeded { get; private set; }

        public static StartupController CreateDefault()
        {
            return new StartupController(
                new PlayerPrefsStartupSettingsRepository(),
                new StartupScreenPlatform(),
                new StartupScenePlatform(),
                new StartupQuitPlatform(),
                new StartupLocalizer());
        }

        public bool OpenSettings()
        {
            if (Panel == StartupPanel.Settings)
            {
                return false;
            }

            Draft = new StartupSettingsDraft(SavedSettings);
            Panel = StartupPanel.Settings;
            Localizer.SetLanguage(Draft.Language);
            OnStateChanged();
            return true;
        }

        public bool SetDraftLanguage(StartupLanguage language)
        {
            if (Draft == null)
            {
                return false;
            }

            Draft.SetLanguage(language);
            Localizer.SetLanguage(language);
            OnStateChanged();
            return true;
        }

        public bool SetDraftDisplayMode(StartupDisplayMode displayMode)
        {
            if (Draft == null)
            {
                return false;
            }

            Draft.SetDisplayMode(displayMode);
            OnStateChanged();
            return true;
        }

        public bool SetDraftResolution(StartupResolution resolution)
        {
            if (Draft == null)
            {
                return false;
            }

            Draft.SetResolution(resolution);
            OnStateChanged();
            return true;
        }

        public bool SetDraftVSyncCount(int vSyncCount)
        {
            if (Draft == null)
            {
                return false;
            }

            Draft.SetVSyncCount(vSyncCount);
            OnStateChanged();
            return true;
        }

        public bool UseDefaults()
        {
            if (Draft == null)
            {
                return false;
            }

            RefreshResolutionContract();
            Draft.ReplaceWith(DefaultSettings);
            Localizer.SetLanguage(Draft.Language);
            OnStateChanged();
            return true;
        }

        public StartupApplyResult ApplySettings()
        {
            if (Draft == null)
            {
                return SetApplyResult(StartupApplyResult.NoDraft);
            }

            var candidate = Draft.ToSettings();
            if (!StartupSettingsDefaults.IsValid(
                    candidate,
                    screenPlatform.SupportedResolutions,
                    screenPlatform.CurrentResolution))
            {
                return SetApplyResult(StartupApplyResult.InvalidDraft);
            }

            if (!screenPlatform.TryApply(candidate))
            {
                return SetApplyResult(StartupApplyResult.ScreenApplyFailed);
            }

            if (!repository.TrySave(candidate))
            {
                screenPlatform.TryApply(SavedSettings);
                return SetApplyResult(StartupApplyResult.SettingsSaveFailed);
            }

            SavedSettings = candidate;
            Draft = null;
            Panel = StartupPanel.MainMenu;
            Localizer.SetLanguage(SavedSettings.Language);
            return SetApplyResult(StartupApplyResult.Applied);
        }

        public bool CancelSettings()
        {
            if (Draft == null)
            {
                return false;
            }

            Draft = null;
            Panel = StartupPanel.MainMenu;
            LastApplyResult = StartupApplyResult.None;
            Localizer.SetLanguage(SavedSettings.Language);
            OnStateChanged();
            return true;
        }

        public bool HandleEscape()
        {
            return Panel == StartupPanel.Settings && CancelSettings();
        }

        public void StartPrototype()
        {
            scenePlatform.LoadScene(StartupSceneContract.PrototypeScenePath);
        }

        public void RequestQuit()
        {
            quitPlatform.RequestQuit();
        }

        private void InitializeSettings()
        {
            if (repository.TryLoad(out var storedSettings) &&
                StartupSettingsDefaults.IsValid(
                    storedSettings,
                    screenPlatform.SupportedResolutions,
                    screenPlatform.CurrentResolution) &&
                screenPlatform.TryApply(storedSettings))
            {
                SavedSettings = storedSettings;
                InitializationSucceeded = true;
                Localizer.SetLanguage(SavedSettings.Language);
                return;
            }

            RefreshResolutionContract();
            SavedSettings = DefaultSettings;
            var screenApplied = screenPlatform.TryApply(SavedSettings);
            var settingsSaved = screenApplied && repository.TrySave(SavedSettings);
            InitializationSucceeded = screenApplied && settingsSaved;
            Localizer.SetLanguage(SavedSettings.Language);
        }

        private void RefreshResolutionContract()
        {
            var supported = StartupSettingsDefaults.NormalizeSupportedResolutions(screenPlatform.SupportedResolutions);
            DefaultSettings = StartupSettingsDefaults.Create(supported, screenPlatform.CurrentResolution);
            AvailableResolutions = supported.Count > 0
                ? supported
                : new[] { DefaultSettings.Resolution };
        }

        private StartupApplyResult SetApplyResult(StartupApplyResult result)
        {
            LastApplyResult = result;
            OnStateChanged();
            return result;
        }

        private void OnStateChanged()
        {
            StateChanged?.Invoke();
        }
    }
}
