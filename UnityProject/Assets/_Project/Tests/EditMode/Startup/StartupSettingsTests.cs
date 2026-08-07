using System;
using System.Collections.Generic;
using System.Linq;
using LastHost.Prototype.UI.Startup;
using NUnit.Framework;
using UnityEngine;

namespace LastHost.Prototype.Tests.EditMode.Startup
{
    public sealed class StartupSettingsTests
    {
        private static readonly StartupResolution Resolution1920 = new StartupResolution(1920, 1080);
        private static readonly StartupResolution Resolution1600 = new StartupResolution(1600, 900);
        private static readonly StartupResolution Resolution1280 = new StartupResolution(1280, 720);

        [Test]
        public void LocalizationCatalog_HasEveryKeyForKoreanAndEnglish()
        {
            var catalog = StartupLocalizationCatalog.Default;
            var allKeys = ((StartupTextKey[])Enum.GetValues(typeof(StartupTextKey))).ToArray();

            CollectionAssert.AreEquivalent(allKeys, catalog.GetKeys(StartupLanguage.Korean));
            CollectionAssert.AreEquivalent(allKeys, catalog.GetKeys(StartupLanguage.English));

            foreach (var language in new[] { StartupLanguage.Korean, StartupLanguage.English })
            {
                foreach (var key in allKeys)
                {
                    Assert.True(catalog.TryGetText(language, key, out var text), $"Missing {language}/{key}");
                    Assert.False(string.IsNullOrWhiteSpace(text), $"Empty {language}/{key}");
                }
            }
        }

        [Test]
        public void Localizer_MissingRequestedTextFallsBackToKoreanThenMarker()
        {
            var localizer = new StartupLocalizer(new SparseCatalog(), StartupLanguage.English);

            Assert.AreEqual("한국어 제목", localizer.GetText(StartupTextKey.GameTitle));
            Assert.AreEqual("[Missing:Quit]", localizer.GetText(StartupTextKey.Quit));
        }

        [Test]
        public void Defaults_Prefers1920By1080WhenSupported()
        {
            var selected = StartupSettingsDefaults.SelectResolution(
                new[] { Resolution1280, Resolution1920, new StartupResolution(2560, 1440) },
                Resolution1280);

            Assert.AreEqual(Resolution1920, selected);
        }

        [Test]
        public void Defaults_WhenPreferredIsMissing_SelectsHighestSupportedSixteenByNine()
        {
            var selected = StartupSettingsDefaults.SelectResolution(
                new[]
                {
                    new StartupResolution(2560, 1080),
                    Resolution1280,
                    Resolution1600,
                    new StartupResolution(1024, 768)
                },
                Resolution1280);

            Assert.AreEqual(Resolution1600, selected);
        }

        [Test]
        public void Defaults_WhenNoSixteenByNineExists_SelectsHighestSupportedResolution()
        {
            var selected = StartupSettingsDefaults.SelectResolution(
                new[]
                {
                    new StartupResolution(1024, 768),
                    new StartupResolution(1600, 1200),
                    new StartupResolution(1280, 1024)
                },
                Resolution1280);

            Assert.AreEqual(new StartupResolution(1600, 1200), selected);
        }

        [Test]
        public void Defaults_WhenSupportedListIsEmpty_UsesCurrentResolution()
        {
            var current = new StartupResolution(1366, 768);

            Assert.AreEqual(
                current,
                StartupSettingsDefaults.SelectResolution(Array.Empty<StartupResolution>(), current));
        }

        [Test]
        public void PlayerPrefsRepository_RoundTripsOnlyACompleteBundle()
        {
            var repository = CreatePlayerPrefsRepository();
            var settings = new StartupSettings(
                StartupLanguage.English,
                StartupDisplayMode.Windowed,
                Resolution1280,
                0);

            try
            {
                Assert.True(repository.TrySave(settings));
                Assert.True(repository.TryLoad(out var restored));
                Assert.AreEqual(settings, restored);
            }
            finally
            {
                DeletePlayerPrefsBundle(repository);
            }
        }

        [Test]
        public void PlayerPrefsRepository_PartialBundleIsRejected()
        {
            var repository = CreatePlayerPrefsRepository();

            try
            {
                PlayerPrefs.SetString(repository.GetKey("schema"), StartupSettings.CurrentSchemaVersion.ToString());
                PlayerPrefs.SetString(repository.GetKey("language"), ((int)StartupLanguage.Korean).ToString());

                Assert.False(repository.TryLoad(out var restored));
                Assert.IsNull(restored);
            }
            finally
            {
                DeletePlayerPrefsBundle(repository);
            }
        }

        [TestCase("schema", "2")]
        [TestCase("language", "99")]
        [TestCase("displayMode", "99")]
        [TestCase("width", "broken")]
        [TestCase("height", "-1")]
        [TestCase("vSync", "2")]
        public void PlayerPrefsRepository_CorruptFieldRejectsWholeBundle(string suffix, string corruptValue)
        {
            var repository = CreatePlayerPrefsRepository();
            var valid = new StartupSettings(
                StartupLanguage.Korean,
                StartupDisplayMode.FullScreenWindow,
                Resolution1920,
                1);

            try
            {
                Assert.True(repository.TrySave(valid));
                PlayerPrefs.SetString(repository.GetKey(suffix), corruptValue);

                Assert.False(repository.TryLoad(out var restored));
                Assert.IsNull(restored);
            }
            finally
            {
                DeletePlayerPrefsBundle(repository);
            }
        }

        [Test]
        public void Controller_LanguageDraftPreviewsEveryStringWithoutSavingOrApplying_ThenCancelRestoresAll()
        {
            var context = CreateContext(ValidStoredSettings());
            var baselineApplyCount = context.Screen.ApplyCount;
            var languageEvents = 0;
            context.Localizer.LanguageChanged += () => languageEvents++;
            Assert.True(context.Controller.OpenSettings());

            Assert.True(context.Controller.SetDraftLanguage(StartupLanguage.English));

            Assert.AreEqual(StartupLanguage.English, context.Localizer.Language);
            Assert.AreEqual(0, context.Repository.SaveCount);
            Assert.AreEqual(baselineApplyCount, context.Screen.ApplyCount);
            AssertCatalogLanguage(context.Localizer, StartupLanguage.English);

            Assert.True(context.Controller.SetDraftDisplayMode(StartupDisplayMode.Windowed));
            Assert.True(context.Controller.SetDraftResolution(Resolution1920));
            Assert.True(context.Controller.SetDraftVSyncCount(0));
            Assert.True(context.Controller.CancelSettings());

            Assert.AreEqual(StartupPanel.MainMenu, context.Controller.Panel);
            Assert.IsNull(context.Controller.Draft);
            Assert.AreEqual(ValidStoredSettings(), context.Controller.SavedSettings);
            Assert.AreEqual(StartupLanguage.Korean, context.Localizer.Language);
            AssertCatalogLanguage(context.Localizer, StartupLanguage.Korean);
            Assert.AreEqual(0, context.Repository.SaveCount);
            Assert.AreEqual(baselineApplyCount, context.Screen.ApplyCount);
            Assert.AreEqual(2, languageEvents);
        }

        [Test]
        public void Controller_DefaultsChangesOnlyDraft()
        {
            var stored = new StartupSettings(
                StartupLanguage.English,
                StartupDisplayMode.Windowed,
                Resolution1280,
                0);
            var context = CreateContext(stored);
            var baselineApplyCount = context.Screen.ApplyCount;
            context.Controller.OpenSettings();

            Assert.True(context.Controller.UseDefaults());

            Assert.AreEqual(StartupSettingsDefaults.DefaultLanguage, context.Controller.Draft.Language);
            Assert.AreEqual(StartupSettingsDefaults.DefaultDisplayMode, context.Controller.Draft.DisplayMode);
            Assert.AreEqual(Resolution1920, context.Controller.Draft.Resolution);
            Assert.AreEqual(StartupSettingsDefaults.DefaultVSyncCount, context.Controller.Draft.VSyncCount);
            Assert.AreEqual(stored, context.Controller.SavedSettings);
            Assert.AreEqual(0, context.Repository.SaveCount);
            Assert.AreEqual(baselineApplyCount, context.Screen.ApplyCount);
        }

        [Test]
        public void Controller_ApplyValidatesThenAppliesScreenThenSavesWholeBundle()
        {
            var trace = new List<string>();
            var context = CreateContext(ValidStoredSettings(), trace);
            context.Controller.OpenSettings();
            context.Controller.SetDraftLanguage(StartupLanguage.English);
            context.Controller.SetDraftDisplayMode(StartupDisplayMode.Windowed);
            context.Controller.SetDraftResolution(Resolution1920);
            context.Controller.SetDraftVSyncCount(0);
            trace.Clear();

            var result = context.Controller.ApplySettings();

            Assert.AreEqual(StartupApplyResult.Applied, result);
            CollectionAssert.AreEqual(new[] { "screen", "save" }, trace);
            Assert.AreEqual(
                new StartupSettings(StartupLanguage.English, StartupDisplayMode.Windowed, Resolution1920, 0),
                context.Controller.SavedSettings);
            Assert.AreEqual(context.Controller.SavedSettings, context.Repository.LastSaved);
            Assert.AreEqual(StartupPanel.MainMenu, context.Controller.Panel);
            Assert.IsNull(context.Controller.Draft);
        }

        [Test]
        public void Controller_InvalidDraftDoesNotApplyOrSave()
        {
            var trace = new List<string>();
            var context = CreateContext(ValidStoredSettings(), trace);
            context.Controller.OpenSettings();
            context.Controller.SetDraftResolution(new StartupResolution(1111, 777));
            trace.Clear();

            var result = context.Controller.ApplySettings();

            Assert.AreEqual(StartupApplyResult.InvalidDraft, result);
            CollectionAssert.IsEmpty(trace);
            Assert.AreEqual(0, context.Repository.SaveCount);
            Assert.AreEqual(ValidStoredSettings(), context.Controller.SavedSettings);
            Assert.IsNotNull(context.Controller.Draft);
        }

        [Test]
        public void Controller_ScreenApplyFailureDoesNotSave()
        {
            var trace = new List<string>();
            var context = CreateContext(ValidStoredSettings(), trace);
            context.Controller.OpenSettings();
            context.Controller.SetDraftResolution(Resolution1920);
            context.Screen.ApplySucceeds = false;
            trace.Clear();

            var result = context.Controller.ApplySettings();

            Assert.AreEqual(StartupApplyResult.ScreenApplyFailed, result);
            CollectionAssert.AreEqual(new[] { "screen" }, trace);
            Assert.AreEqual(0, context.Repository.SaveCount);
            Assert.AreEqual(ValidStoredSettings(), context.Controller.SavedSettings);
            Assert.IsNotNull(context.Controller.Draft);
        }

        [Test]
        public void Controller_ValidStoredBundleIsRestoredWithoutRewrite()
        {
            var stored = new StartupSettings(
                StartupLanguage.English,
                StartupDisplayMode.Windowed,
                Resolution1280,
                0);
            var context = CreateContext(stored);

            Assert.True(context.Controller.InitializationSucceeded);
            Assert.AreEqual(stored, context.Controller.SavedSettings);
            Assert.AreEqual(stored, context.Screen.LastApplied);
            Assert.AreEqual(StartupLanguage.English, context.Localizer.Language);
            Assert.AreEqual(0, context.Repository.SaveCount);
            Assert.AreEqual(1, context.Screen.ApplyCount);
        }

        [Test]
        public void Controller_MissingStoredBundleRecoversAndPersistsWholeDefaultProfile()
        {
            var context = CreateContext(null);
            var expected = new StartupSettings(
                StartupLanguage.Korean,
                StartupDisplayMode.FullScreenWindow,
                Resolution1920,
                1);

            Assert.True(context.Controller.InitializationSucceeded);
            Assert.AreEqual(expected, context.Controller.SavedSettings);
            Assert.AreEqual(expected, context.Screen.LastApplied);
            Assert.AreEqual(expected, context.Repository.LastSaved);
            Assert.AreEqual(1, context.Repository.SaveCount);
            Assert.AreEqual(1, context.Screen.ApplyCount);
        }

        [TestCaseSource(nameof(InvalidStoredBundles))]
        public void Controller_DamagedOrUnsupportedStoredBundleRecoversAsOneDefaultProfile(StartupSettings invalid)
        {
            var context = CreateContext(invalid);
            var expected = new StartupSettings(
                StartupLanguage.Korean,
                StartupDisplayMode.FullScreenWindow,
                Resolution1920,
                1);

            Assert.AreEqual(expected, context.Controller.SavedSettings);
            Assert.AreEqual(expected, context.Repository.LastSaved);
            Assert.AreEqual(expected, context.Screen.LastApplied);
            Assert.AreEqual(1, context.Repository.SaveCount);
            Assert.AreEqual(1, context.Screen.ApplyCount);
        }

        [Test]
        public void Controller_StartUsesExact2DPrototypePath_AndQuitUsesPlatformBoundary()
        {
            var context = CreateContext(ValidStoredSettings());

            context.Controller.StartPrototype();
            context.Controller.RequestQuit();

            Assert.AreEqual("Assets/_Project/Scenes/RatHost2DPrototype.unity", StartupSceneContract.PrototypeScenePath);
            Assert.AreNotEqual("Assets/_Project/Scenes/RatHostPrototype.unity", StartupSceneContract.PrototypeScenePath);
            Assert.AreEqual(StartupSceneContract.PrototypeScenePath, context.Scene.LastScenePath);
            Assert.AreEqual(1, context.Scene.LoadCount);
            Assert.AreEqual(1, context.Quit.RequestCount);
        }

        [Test]
        public void Controller_EscapeCancelsSettingsButDoesNothingOnMainMenu()
        {
            var context = CreateContext(ValidStoredSettings());

            Assert.False(context.Controller.HandleEscape());
            context.Controller.OpenSettings();
            context.Controller.SetDraftLanguage(StartupLanguage.English);

            Assert.True(context.Controller.HandleEscape());
            Assert.AreEqual(StartupPanel.MainMenu, context.Controller.Panel);
            Assert.AreEqual(StartupLanguage.Korean, context.Localizer.Language);
            Assert.False(context.Controller.HandleEscape());
        }

        private static IEnumerable<StartupSettings> InvalidStoredBundles()
        {
            yield return new StartupSettings(
                StartupSettings.CurrentSchemaVersion + 1,
                StartupLanguage.English,
                StartupDisplayMode.Windowed,
                Resolution1280,
                0);
            yield return new StartupSettings(
                (StartupLanguage)99,
                StartupDisplayMode.Windowed,
                Resolution1280,
                0);
            yield return new StartupSettings(
                StartupLanguage.English,
                (StartupDisplayMode)99,
                Resolution1280,
                0);
            yield return new StartupSettings(
                StartupLanguage.English,
                StartupDisplayMode.Windowed,
                new StartupResolution(1111, 777),
                0);
            yield return new StartupSettings(
                StartupLanguage.English,
                StartupDisplayMode.Windowed,
                Resolution1280,
                2);
        }

        private static StartupSettings ValidStoredSettings()
        {
            return new StartupSettings(
                StartupLanguage.Korean,
                StartupDisplayMode.FullScreenWindow,
                Resolution1280,
                1);
        }

        private static TestContext CreateContext(StartupSettings stored, List<string> trace = null)
        {
            var sharedTrace = trace ?? new List<string>();
            var repository = new FakeRepository(stored, sharedTrace);
            var screen = new FakeScreenPlatform(
                new[] { Resolution1280, Resolution1600, Resolution1920 },
                Resolution1280,
                sharedTrace);
            var scene = new FakeScenePlatform();
            var quit = new FakeQuitPlatform();
            var localizer = new StartupLocalizer();
            var controller = new StartupController(repository, screen, scene, quit, localizer);
            return new TestContext(controller, repository, screen, scene, quit, localizer);
        }

        private static void AssertCatalogLanguage(IStartupLocalizer localizer, StartupLanguage language)
        {
            foreach (StartupTextKey key in Enum.GetValues(typeof(StartupTextKey)))
            {
                Assert.True(StartupLocalizationCatalog.Default.TryGetText(language, key, out var expected));
                Assert.AreEqual(expected, localizer.GetText(key), $"Unexpected text for {language}/{key}");
            }
        }

        private static PlayerPrefsStartupSettingsRepository CreatePlayerPrefsRepository()
        {
            return new PlayerPrefsStartupSettingsRepository(
                $"{PlayerPrefsStartupSettingsRepository.DefaultKeyPrefix}.tests.{Guid.NewGuid():N}");
        }

        private static void DeletePlayerPrefsBundle(PlayerPrefsStartupSettingsRepository repository)
        {
            foreach (var suffix in new[] { "schema", "language", "displayMode", "width", "height", "vSync" })
            {
                PlayerPrefs.DeleteKey(repository.GetKey(suffix));
            }
        }

        private sealed class SparseCatalog : IStartupLocalizationCatalog
        {
            public bool TryGetText(StartupLanguage language, StartupTextKey key, out string text)
            {
                if (language == StartupLanguage.Korean && key == StartupTextKey.GameTitle)
                {
                    text = "한국어 제목";
                    return true;
                }

                text = null;
                return false;
            }

            public IReadOnlyCollection<StartupTextKey> GetKeys(StartupLanguage language)
            {
                return language == StartupLanguage.Korean
                    ? new[] { StartupTextKey.GameTitle }
                    : Array.Empty<StartupTextKey>();
            }
        }

        private sealed class FakeRepository : IStartupSettingsRepository
        {
            private readonly List<string> trace;
            private readonly StartupSettings stored;

            public FakeRepository(StartupSettings stored, List<string> trace)
            {
                this.stored = stored;
                this.trace = trace;
            }

            public int SaveCount { get; private set; }
            public StartupSettings LastSaved { get; private set; }
            public bool SaveSucceeds { get; set; } = true;

            public bool TryLoad(out StartupSettings settings)
            {
                settings = stored;
                return stored != null;
            }

            public bool TrySave(StartupSettings settings)
            {
                trace.Add("save");
                SaveCount++;
                LastSaved = settings;
                return SaveSucceeds;
            }
        }

        private sealed class FakeScreenPlatform : IStartupScreenPlatform
        {
            private readonly List<string> trace;

            public FakeScreenPlatform(
                IReadOnlyList<StartupResolution> supportedResolutions,
                StartupResolution currentResolution,
                List<string> trace)
            {
                SupportedResolutions = supportedResolutions;
                CurrentResolution = currentResolution;
                this.trace = trace;
            }

            public IReadOnlyList<StartupResolution> SupportedResolutions { get; }
            public StartupResolution CurrentResolution { get; }
            public bool ApplySucceeds { get; set; } = true;
            public int ApplyCount { get; private set; }
            public StartupSettings LastApplied { get; private set; }

            public bool TryApply(StartupSettings settings)
            {
                trace.Add("screen");
                ApplyCount++;
                LastApplied = settings;
                return ApplySucceeds;
            }
        }

        private sealed class FakeScenePlatform : IStartupScenePlatform
        {
            public int LoadCount { get; private set; }
            public string LastScenePath { get; private set; }

            public void LoadScene(string scenePath)
            {
                LoadCount++;
                LastScenePath = scenePath;
            }
        }

        private sealed class FakeQuitPlatform : IStartupQuitPlatform
        {
            public int RequestCount { get; private set; }

            public void RequestQuit()
            {
                RequestCount++;
            }
        }

        private sealed class TestContext
        {
            public TestContext(
                StartupController controller,
                FakeRepository repository,
                FakeScreenPlatform screen,
                FakeScenePlatform scene,
                FakeQuitPlatform quit,
                StartupLocalizer localizer)
            {
                Controller = controller;
                Repository = repository;
                Screen = screen;
                Scene = scene;
                Quit = quit;
                Localizer = localizer;
            }

            public StartupController Controller { get; }
            public FakeRepository Repository { get; }
            public FakeScreenPlatform Screen { get; }
            public FakeScenePlatform Scene { get; }
            public FakeQuitPlatform Quit { get; }
            public StartupLocalizer Localizer { get; }
        }
    }
}
