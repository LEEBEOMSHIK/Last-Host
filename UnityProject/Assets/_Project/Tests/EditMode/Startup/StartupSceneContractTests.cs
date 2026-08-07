using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using LastHost.Prototype.UI.Startup;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastHost.Prototype.Tests.Startup
{
    public sealed class StartupSceneContractTests
    {
        private const string LegacyPrototypeScenePath = "Assets/_Project/Scenes/RatHostPrototype.unity";
        private const string SampleScenePath = "Assets/Scenes/SampleScene.unity";
        private const string StartupBackgroundAssetPath =
            "Assets/_Project/Art/Production2D/V1/UI/Startup/startup-bacteriophage-food-chain-background-v1.png";
        private const string KoreanFontAssetPath =
            "Assets/_Project/Art/Production2D/V1/UI/Fonts/Galmuri11/Galmuri11.ttf";
        private const string KoreanLicenseAssetPath =
            "Assets/_Project/Art/Production2D/V1/UI/Fonts/Galmuri11/OFL.txt";
        private const string KoreanSourceAssetPath =
            "Assets/_Project/Art/Production2D/V1/UI/Fonts/Galmuri11/SOURCE.md";
        private const string EnglishFontAssetPath =
            "Assets/_Project/Art/Production2D/V1/UI/Fonts/Silkscreen/Silkscreen-Regular.ttf";
        private const string EnglishLicenseAssetPath =
            "Assets/_Project/Art/Production2D/V1/UI/Fonts/Silkscreen/OFL.txt";
        private const string EnglishSourceAssetPath =
            "Assets/_Project/Art/Production2D/V1/UI/Fonts/Silkscreen/SOURCE.md";

        [Test]
        public void BuildSettings_StartWithStartupThenExact2DPrototype()
        {
            var scenes = EditorBuildSettings.scenes;

            Assert.That(scenes, Has.Length.GreaterThanOrEqualTo(4));
            Assert.That(scenes[0].enabled, Is.True);
            Assert.That(scenes[0].path, Is.EqualTo(StartupSceneContract.StartupScenePath));
            Assert.That(scenes[1].enabled, Is.True);
            Assert.That(scenes[1].path, Is.EqualTo(StartupSceneContract.PrototypeScenePath));
        }

        [Test]
        public void BuildSettings_PreserveLegacyAndSampleEntriesAsDisabled()
        {
            var scenes = EditorBuildSettings.scenes;
            var legacy = scenes.Single(scene => scene.path == LegacyPrototypeScenePath);
            var sample = scenes.Single(scene => scene.path == SampleScenePath);

            Assert.That(legacy.enabled, Is.False);
            Assert.That(sample.enabled, Is.False);
        }

        [Test]
        public void StartupScene_IsLoadableAndContainsSingleStartupMenuView()
        {
            AssertSceneAssetExists(StartupSceneContract.StartupScenePath);

            var alreadyLoaded = SceneManager.GetSceneByPath(StartupSceneContract.StartupScenePath);
            var openedByTest = !alreadyLoaded.IsValid() || !alreadyLoaded.isLoaded;
            var scene = openedByTest
                ? EditorSceneManager.OpenScene(StartupSceneContract.StartupScenePath, OpenSceneMode.Additive)
                : alreadyLoaded;

            try
            {
                Assert.That(scene.IsValid(), Is.True);
                Assert.That(scene.isLoaded, Is.True);

                var views = scene
                    .GetRootGameObjects()
                    .SelectMany(root => root.GetComponentsInChildren<StartupMenuView>(true))
                    .ToArray();

                Assert.That(views, Has.Length.EqualTo(1));
            }
            finally
            {
                if (openedByTest && scene.IsValid() && scene.isLoaded)
                {
                    EditorSceneManager.CloseScene(scene, true);
                }
            }
        }

        [Test]
        public void ProtectedSceneAssets_AndExact2DTargetExist()
        {
            AssertSceneAssetExists(LegacyPrototypeScenePath);
            AssertSceneAssetExists(StartupSceneContract.PrototypeScenePath);
            Assert.That(
                StartupSceneContract.PrototypeScenePath,
                Is.EqualTo("Assets/_Project/Scenes/RatHost2DPrototype.unity"));
        }

        [Test]
        public void StartupScene_ReferencesSelectedSpriteBackgroundWithUiImportSettings()
        {
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(StartupBackgroundAssetPath);
            Assert.That(sprite, Is.Not.Null, $"Expected startup background Sprite at '{StartupBackgroundAssetPath}'.");

            var importer = AssetImporter.GetAtPath(StartupBackgroundAssetPath) as TextureImporter;
            Assert.That(importer, Is.Not.Null);
            Assert.That(importer.textureType, Is.EqualTo(TextureImporterType.Sprite));
            Assert.That(importer.mipmapEnabled, Is.False);
            Assert.That(importer.filterMode, Is.EqualTo(FilterMode.Point));

            var alreadyLoaded = SceneManager.GetSceneByPath(StartupSceneContract.StartupScenePath);
            var openedByTest = !alreadyLoaded.IsValid() || !alreadyLoaded.isLoaded;
            var scene = openedByTest
                ? EditorSceneManager.OpenScene(StartupSceneContract.StartupScenePath, OpenSceneMode.Additive)
                : alreadyLoaded;

            try
            {
                var view = scene
                    .GetRootGameObjects()
                    .SelectMany(root => root.GetComponentsInChildren<StartupMenuView>(true))
                    .Single();
                Assert.That(view.StartupBackground, Is.EqualTo(sprite));
            }
            finally
            {
                if (openedByTest && scene.IsValid() && scene.isLoaded)
                {
                    EditorSceneManager.CloseScene(scene, true);
                }
            }
        }

        [Test]
        public void EditorPlayMode_AlwaysStartsFromSavedStartupScene()
        {
            var startupScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(StartupSceneContract.StartupScenePath);

            Assert.That(startupScene, Is.Not.Null);
            Assert.That(EditorSceneManager.playModeStartScene, Is.EqualTo(startupScene));
            Assert.That(AssetDatabase.GetAssetPath(EditorSceneManager.playModeStartScene),
                Is.EqualTo(StartupSceneContract.StartupScenePath));
        }

        [Test]
        public void StartupScene_MapsLanguageProfilesToPinnedFonts()
        {
            var koreanFont = AssetDatabase.LoadAssetAtPath<Font>(KoreanFontAssetPath);
            var englishFont = AssetDatabase.LoadAssetAtPath<Font>(EnglishFontAssetPath);
            Assert.That(koreanFont, Is.Not.Null);
            Assert.That(englishFont, Is.Not.Null);

            WithStartupView(view =>
            {
                Assert.That(view.KoreanFont, Is.EqualTo(koreanFont));
                Assert.That(view.EnglishFont, Is.EqualTo(englishFont));
                Assert.That(view.GetFontProfile(StartupLanguage.Korean), Is.EqualTo(koreanFont));
                Assert.That(view.GetFontProfile(StartupLanguage.English), Is.EqualTo(englishFont));
                Assert.That(view.GetFontProfile(StartupLanguage.Korean).name, Does.Contain("Galmuri11"));
                Assert.That(view.GetFontProfile(StartupLanguage.English).name, Does.Contain("Silkscreen"));
            });
        }

        [Test]
        public void LanguageFonts_SupportEveryLocalizedCharacterAndSharedResolutionGlyphs()
        {
            var catalog = StartupLocalizationCatalog.Default;
            AssertFontSupportsLanguage(
                AssetDatabase.LoadAssetAtPath<Font>(KoreanFontAssetPath),
                catalog,
                StartupLanguage.Korean);
            AssertFontSupportsLanguage(
                AssetDatabase.LoadAssetAtPath<Font>(EnglishFontAssetPath),
                catalog,
                StartupLanguage.English);
        }

        [Test]
        public void FontFamilies_KeepPinnedBinaryLicenseAndSourceTogether()
        {
            AssertPinnedFile(KoreanFontAssetPath, 5376428L,
                "E24256F42E43713D2EA086A1E1669D78B968F5B3CC547E5C157F0606FFA5DEF1");
            AssertPinnedFile(KoreanLicenseAssetPath, 4266L,
                "9A9E5A342C430C3FCF01A408B680F4405D5BF4AC659C931BE35F8A1B27EA69C9");
            AssertPinnedFile(EnglishFontAssetPath, 32220L,
                "C845473330B94C2079CE9AF01C51AC8BA2D99C24F4D14C039843BBB8E642EBD8");
            AssertPinnedFile(EnglishLicenseAssetPath, 4394L,
                "86C5E9C9382CDCC5948704FDFE60F2AA164A719746931219A42736ECD9CEFBD3");

            AssertSourceManifest(KoreanSourceAssetPath, "71e1cacf1437a11220307120e63e30bc275312d4");
            AssertSourceManifest(EnglishSourceAssetPath, "c28e08582e7bd36751febb3391142a5eb18bbb34");
        }

        [Test]
        public void MissingReferences_HaveStableDiagnosticsAndNonBlackBackgroundFallback()
        {
            Assert.That(StartupMenuView.MissingBackgroundDiagnosticId,
                Is.EqualTo("[StartupUI:PFC6_MISSING_BACKGROUND]"));
            Assert.That(StartupMenuView.GetMissingFontDiagnosticId(StartupLanguage.Korean),
                Is.EqualTo("[StartupUI:PFC6_MISSING_FONT_KO]"));
            Assert.That(StartupMenuView.GetMissingFontDiagnosticId(StartupLanguage.English),
                Is.EqualTo("[StartupUI:PFC6_MISSING_FONT_EN]"));
            Assert.That(StartupMenuView.DiagnosticFallbackBackgroundColor.a, Is.EqualTo(1f));
            Assert.That(StartupMenuView.DiagnosticFallbackBackgroundColor, Is.Not.EqualTo(Color.black));
        }

        private static void WithStartupView(Action<StartupMenuView> assertion)
        {
            var alreadyLoaded = SceneManager.GetSceneByPath(StartupSceneContract.StartupScenePath);
            var openedByTest = !alreadyLoaded.IsValid() || !alreadyLoaded.isLoaded;
            var scene = openedByTest
                ? EditorSceneManager.OpenScene(StartupSceneContract.StartupScenePath, OpenSceneMode.Additive)
                : alreadyLoaded;

            try
            {
                var view = scene.GetRootGameObjects()
                    .SelectMany(root => root.GetComponentsInChildren<StartupMenuView>(true))
                    .Single();
                assertion(view);
            }
            finally
            {
                if (openedByTest && scene.IsValid() && scene.isLoaded)
                {
                    EditorSceneManager.CloseScene(scene, true);
                }
            }
        }

        private static void AssertFontSupportsLanguage(
            Font font,
            IStartupLocalizationCatalog catalog,
            StartupLanguage language)
        {
            Assert.That(font, Is.Not.Null);
            var requiredCharacters = new HashSet<char>("0123456789×");
            foreach (var key in catalog.GetKeys(language))
            {
                Assert.That(catalog.TryGetText(language, key, out var value), Is.True);
                foreach (var character in value)
                {
                    if (!char.IsWhiteSpace(character))
                    {
                        requiredCharacters.Add(character);
                    }
                }
            }

            var missing = requiredCharacters.Where(character => !font.HasCharacter(character)).ToArray();
            Assert.That(missing, Is.Empty,
                $"Font '{font.name}' is missing required {language} characters: {new string(missing)}");
        }

        private static void AssertPinnedFile(string assetPath, long expectedBytes, string expectedSha256)
        {
            var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
            Assert.That(File.Exists(absolutePath), Is.True, $"Expected pinned file at '{assetPath}'.");
            Assert.That(new FileInfo(absolutePath).Length, Is.EqualTo(expectedBytes));

            using (var stream = File.OpenRead(absolutePath))
            using (var sha256 = SHA256.Create())
            {
                var actualHash = BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", string.Empty);
                Assert.That(actualHash, Is.EqualTo(expectedSha256));
            }
        }

        private static void AssertSourceManifest(string assetPath, string pinnedCommit)
        {
            var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
            Assert.That(File.Exists(absolutePath), Is.True);
            var source = File.ReadAllText(absolutePath);
            Assert.That(source, Does.Contain(pinnedCommit));
            Assert.That(source, Does.Contain("raw.githubusercontent.com"));
        }

        private static void AssertSceneAssetExists(string path)
        {
            var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            Assert.That(asset, Is.Not.Null, $"Expected scene asset at '{path}'.");
        }
    }
}
