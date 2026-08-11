using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using LastHost.Prototype.Cinematics.A01.Editor;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace LastHost.Prototype.Cinematics.A01.Tests
{
    public sealed class A01OfficeAnimaticContractTests
    {
        private const string ScenePath = "Assets/_Project/Scenes/Cinematics/Opening/A01OfficeAnimatic.unity";
        private const string TimelinePath = "Assets/_Project/Timelines/Cinematics/Opening/A01/A01OfficeAnimatic.playable";
        private const string ClipRoot = "Assets/_Project/Animations/Cinematics/Opening/A01/Preview/";
        private const string ArtRoot = "Assets/_Project/Art/Cinematics/Opening/A01/Office/";
        private const string BackgroundPath = ArtRoot + "a01-office-background-v1.png";
        private const string CastPath = ArtRoot + "a01-office-cast-poses-v1.png";
        private const string MaskPath = ArtRoot + "a01-office-occlusion-mask-v1.png";
        private const string StartupScenePath = "Assets/_Project/Scenes/Startup.unity";
        private const string RejectedColorSha = "3B94269DE3D3CDD41BD534450EF0A6E5CB8E3A64C44316692E639B1A30A4AF4B";

        private static readonly int[] BeatFrames = { 0, 36, 72, 108, 138, 180, 204 };
        private static readonly string[] TrackNames =
        {
            "CameraTrack", "BackgroundTrack", "P1_SpeakerTrack", "P2_ReactorTrack",
            "P3_WorkerTrack", "P4_ExitLeadTrack", "P5_ExitFollowTrack"
        };
        private static readonly string[] ClipPaths =
        {
            ClipRoot + "A01_Camera_Preview.anim", ClipRoot + "A01_Background_Preview.anim",
            ClipRoot + "A01_P1_Preview.anim", ClipRoot + "A01_P2_Preview.anim",
            ClipRoot + "A01_P3_Preview.anim", ClipRoot + "A01_P4_Preview.anim",
            ClipRoot + "A01_P5_Preview.anim"
        };

        private SceneSetup[] originalSetup;
        private SceneAsset originalStartScene;
        private Dictionary<string, AssetIdentity> checkoutAssetBaseline;

        [OneTimeSetUp]
        public void CaptureCheckoutBaselineWithoutGeneratingMissingAssets()
        {
            originalSetup = EditorSceneManager.GetSceneManagerSetup();
            originalStartScene = EditorSceneManager.playModeStartScene;
            var paths = GeneratedAssetPaths();
            foreach (var path in paths)
            {
                Assert.That(File.Exists(ProjectPath(path)), Is.True, "Generated checkout asset is missing: " + path);
                Assert.That(File.Exists(ProjectPath(path + ".meta")), Is.True, "Generated checkout meta is missing: " + path + ".meta");
            }

            checkoutAssetBaseline = CaptureAssetIdentities(paths);
        }

        [OneTimeTearDown]
        public void RestoreEditorSetup()
        {
            EditorSceneManager.playModeStartScene = originalStartScene;
            if (originalSetup != null && originalSetup.Length > 0)
            {
                EditorSceneManager.RestoreSceneManagerSetup(originalSetup);
            }
        }

        [Test]
        public void Rebuild_twice_preserves_the_nine_generated_assets_guids_and_dependency_hashes()
        {
            var paths = GeneratedAssetPaths();
            A01OfficeAnimaticSceneBuilder.RebuildPreview();
            var afterFirst = CaptureAssetIdentities(paths);
            A01OfficeAnimaticSceneBuilder.RebuildPreview();
            var afterSecond = CaptureAssetIdentities(paths);

            Assert.That(checkoutAssetBaseline.Keys, Is.EquivalentTo(paths));
            foreach (var path in paths)
            {
                Assert.That(checkoutAssetBaseline[path].Guid, Is.Not.Empty, path);
                AssertAssetIdentity(checkoutAssetBaseline[path], afterFirst[path], "baseline -> rebuild 1", path);
                AssertAssetIdentity(checkoutAssetBaseline[path], afterSecond[path], "baseline -> rebuild 2", path);
                AssertAssetIdentity(afterFirst[path], afterSecond[path], "rebuild 1 -> rebuild 2", path);
            }
        }

        [Test]
        public void Scene_has_the_exact_hierarchy_one_camera_one_director_and_five_positioned_cast_roots()
        {
            var root = OpenRoot();
            var expected = new[]
            {
                "A01_Office_Animatic", "A01_Office_Animatic/CameraRig", "A01_Office_Animatic/CameraRig/Main Camera",
                "A01_Office_Animatic/VisualRoot", "A01_Office_Animatic/VisualRoot/BG_Room",
                "A01_Office_Animatic/VisualRoot/CHAR_P1", "A01_Office_Animatic/VisualRoot/CHAR_P2",
                "A01_Office_Animatic/VisualRoot/CHAR_P3", "A01_Office_Animatic/VisualRoot/CHAR_P4",
                "A01_Office_Animatic/VisualRoot/CHAR_P5", "A01_Office_Animatic/VisualRoot/FG_BackgroundRepeat",
                "A01_Office_Animatic/VisualRoot/FG_OcclusionMask", "A01_Office_Animatic/VisualRoot/FX_Ambient",
                "A01_Office_Animatic/Timeline"
            };
            var actual = root.GetComponentsInChildren<Transform>(true).Select(HierarchyPath).OrderBy(x => x).ToArray();
            Assert.That(actual, Is.EqualTo(expected.OrderBy(x => x).ToArray()));
            Assert.That(root.GetComponentsInChildren<Camera>(true), Has.Length.EqualTo(1));
            Assert.That(root.GetComponentsInChildren<PlayableDirector>(true), Has.Length.EqualTo(1));
            Assert.That(Enumerable.Range(1, 5).Select(i => root.transform.Find("VisualRoot/CHAR_P" + i)), Has.All.Not.Null);

            AssertTransform(root, "CameraRig/Main Camera", new Vector3(0f, 0f, -10f), Vector3.one);
            AssertTransform(root, "VisualRoot/CHAR_P1", new Vector3(-2.10f, -2.70f, 0f), new Vector3(1.70f, 1.70f, 1f));
            AssertTransform(root, "VisualRoot/CHAR_P2", new Vector3(-5.35f, -3.80f, 0f), new Vector3(2f, 2f, 1f));
            AssertTransform(root, "VisualRoot/CHAR_P3", new Vector3(0.15f, -3.80f, 0f), new Vector3(2.10f, 2.10f, 1f));
            AssertTransform(root, "VisualRoot/CHAR_P4", new Vector3(2.75f, -3.80f, 0f), new Vector3(2.40f, 2.40f, 1f));
            AssertTransform(root, "VisualRoot/CHAR_P5", new Vector3(5.10f, -3.80f, 0f), new Vector3(2.40f, 2.40f, 1f));
        }

        [Test]
        public void Scene_references_only_the_exact_approved_background_cast_and_mask_bytes()
        {
            var root = OpenRoot();
            var expectedHashes = new Dictionary<string, string>
            {
                { BackgroundPath, "DA5F22DE7D1C9BDBABE2A8887640085142D23E02CF3BF94B21E217A7EC98AA0C" },
                { CastPath, "71F6542C8DD6229F40DB8E1CD1DF9A1C7B293FFDB28B172A3C87900465BD365D" },
                { MaskPath, "F59EBC810A943DB76C17691AD364237F473BAB6A97EF3A8966321BAEF8400D95" }
            };
            var imageDependencies = AssetDatabase.GetDependencies(ScenePath, true)
                .Where(path => path.EndsWith(".png", StringComparison.OrdinalIgnoreCase)).ToArray();
            Assert.That(imageDependencies, Is.EquivalentTo(expectedHashes.Keys));

            foreach (var pair in expectedHashes)
            {
                var sha = Sha256(pair.Key);
                Assert.That(sha, Is.EqualTo(pair.Value), pair.Key);
                Assert.That(sha, Is.Not.EqualTo(RejectedColorSha), pair.Key);
                Assert.That(sha.StartsWith("24A03C", StringComparison.OrdinalIgnoreCase) &&
                            sha.EndsWith("D526", StringComparison.OrdinalIgnoreCase), Is.False, pair.Key);
            }

            var renderers = root.GetComponentsInChildren<SpriteRenderer>(true);
            Assert.That(renderers.Select(r => AssetDatabase.GetAssetPath(r.sprite)).Distinct(),
                Is.EquivalentTo(new[] { BackgroundPath, CastPath }));
            Assert.That(AssetDatabase.GetAssetPath(root.transform.Find("VisualRoot/FG_OcclusionMask").GetComponent<SpriteMask>().sprite),
                Is.EqualTo(MaskPath));
            Assert.That(imageDependencies.Any(path => path.Contains("/Preview/") || path.Contains("foreground")), Is.False);

            var maskTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(MaskPath);
            var maskSprite = AssetDatabase.LoadAssetAtPath<Sprite>(MaskPath);
            var maskImporter = AssetImporter.GetAtPath(MaskPath) as TextureImporter;
            Assert.That(maskTexture, Is.Not.Null);
            Assert.That(maskSprite, Is.Not.Null);
            Assert.That(maskSprite.GetPhysicsShapeCount(), Is.EqualTo(0));
            Assert.That(maskImporter, Is.Not.Null);
            Assert.That(maskImporter.textureType, Is.EqualTo(TextureImporterType.Sprite));
            Assert.That(maskImporter.spriteImportMode, Is.EqualTo(SpriteImportMode.Single));
            Assert.That(maskImporter.spritePixelsPerUnit, Is.EqualTo(100f));
            Assert.That(maskImporter.filterMode, Is.EqualTo(FilterMode.Point));
            Assert.That(maskImporter.mipmapEnabled, Is.False);
            Assert.That(maskImporter.textureCompression, Is.EqualTo(TextureImporterCompression.Uncompressed));
            Assert.That(maskImporter.alphaIsTransparency, Is.True);
        }

        [Test]
        public void Sorting_and_custom_mask_range_target_only_order_200_background_repeat()
        {
            var root = OpenRoot();
            var expected = new Dictionary<string, int>
            {
                { "BG_Room", 0 }, { "CHAR_P1", 110 }, { "CHAR_P2", 100 }, { "CHAR_P3", 120 },
                { "CHAR_P4", 130 }, { "CHAR_P5", 140 }, { "FG_BackgroundRepeat", 200 }
            };
            var visual = root.transform.Find("VisualRoot");
            foreach (var pair in expected)
            {
                var renderer = visual.Find(pair.Key).GetComponent<SpriteRenderer>();
                Assert.That(renderer.sortingLayerName, Is.EqualTo("Default"), pair.Key);
                Assert.That(renderer.sortingOrder, Is.EqualTo(pair.Value), pair.Key);
                Assert.That(renderer.maskInteraction,
                    Is.EqualTo(pair.Key == "FG_BackgroundRepeat" ? SpriteMaskInteraction.VisibleInsideMask : SpriteMaskInteraction.None), pair.Key);
            }

            var background = visual.Find("BG_Room");
            var repeat = visual.Find("FG_BackgroundRepeat");
            Assert.That(repeat.GetComponent<SpriteRenderer>().sprite, Is.SameAs(background.GetComponent<SpriteRenderer>().sprite));
            Assert.That(repeat.localPosition, Is.EqualTo(background.localPosition));
            Assert.That(repeat.localRotation, Is.EqualTo(background.localRotation));
            Assert.That(repeat.localScale, Is.EqualTo(background.localScale));

            var mask = visual.Find("FG_OcclusionMask").GetComponent<SpriteMask>();
            var repeatRenderer = repeat.GetComponent<SpriteRenderer>();
            Assert.That(mask.isCustomRangeActive, Is.True);
            Assert.That(mask.frontSortingLayerID, Is.EqualTo(repeatRenderer.sortingLayerID));
            Assert.That(mask.backSortingLayerID, Is.EqualTo(repeatRenderer.sortingLayerID));
            Assert.That(mask.frontSortingOrder, Is.EqualTo(200));
            Assert.That(mask.backSortingOrder, Is.EqualTo(200));
        }

        [Test]
        public void Camera_is_orthographic_unrotated_and_has_position_only_animation_without_zoom()
        {
            var root = OpenRoot();
            var camera = root.transform.Find("CameraRig/Main Camera").GetComponent<Camera>();
            Assert.That(camera.orthographic, Is.True);
            Assert.That(camera.orthographicSize, Is.EqualTo(5.4f).Within(0.0001f));
            Assert.That(camera.transform.localRotation, Is.EqualTo(Quaternion.identity));
            Assert.That(camera.transform.localScale, Is.EqualTo(Vector3.one));
            Assert.That(camera.backgroundColor.a, Is.EqualTo(1f));

            var bindings = AnimationUtility.GetCurveBindings(LoadClip(0));
            Assert.That(bindings.Select(binding => binding.propertyName),
                Is.EquivalentTo(new[] { "m_LocalPosition.x", "m_LocalPosition.y", "m_LocalPosition.z" }));
            Assert.That(bindings.All(binding => binding.type == typeof(Transform)), Is.True);
        }

        [Test]
        public void Timeline_is_24fps_204_frames_with_exactly_seven_animation_tracks_and_no_audio_or_signal()
        {
            var timeline = LoadTimeline();
            var tracks = timeline.GetRootTracks().ToArray();
            Assert.That(A01OfficeAnimaticContract.TimelineFps, Is.EqualTo(24));
            Assert.That(A01OfficeAnimaticContract.TotalFrames, Is.EqualTo(204));
            Assert.That(A01OfficeAnimaticContract.DurationSeconds, Is.EqualTo(8.5d));
            Assert.That(A01OfficeAnimaticContract.IsFinalTiming, Is.False);
            Assert.That(A01OfficeAnimaticContract.TimingStatus, Is.EqualTo("preview-measurement-only"));
            Assert.That(A01OfficeAnimaticContract.BeatFrames, Is.EqualTo(BeatFrames));
            Assert.That(timeline.editorSettings.fps, Is.EqualTo(24d));
            Assert.That(timeline.duration * 24d, Is.EqualTo(204d).Within(0.0001d));
            Assert.That(tracks.Select(track => track.name), Is.EqualTo(TrackNames));
            Assert.That(tracks.All(track => track.GetType() == typeof(AnimationTrack)), Is.True);
            Assert.That(tracks.OfType<AudioTrack>(), Is.Empty);
            Assert.That(tracks.OfType<SignalTrack>(), Is.Empty);
        }

        [Test]
        public void Director_uses_the_timeline_none_wrap_play_on_awake_and_exact_animator_bindings()
        {
            var root = OpenRoot();
            var director = root.transform.Find("Timeline").GetComponent<PlayableDirector>();
            var timeline = LoadTimeline();
            var targetPaths = new[]
            {
                "CameraRig/Main Camera", "VisualRoot/BG_Room", "VisualRoot/CHAR_P1", "VisualRoot/CHAR_P2",
                "VisualRoot/CHAR_P3", "VisualRoot/CHAR_P4", "VisualRoot/CHAR_P5"
            };
            Assert.That(director.playableAsset, Is.SameAs(timeline));
            Assert.That(director.playOnAwake, Is.True);
            Assert.That(director.extrapolationMode, Is.EqualTo(DirectorWrapMode.None));
            var tracks = timeline.GetRootTracks().Cast<AnimationTrack>().ToArray();
            for (var i = 0; i < tracks.Length; i++)
            {
                var expected = root.transform.Find(targetPaths[i]).GetComponent<Animator>();
                Assert.That(director.GetGenericBinding(tracks[i]), Is.SameAs(expected), tracks[i].name);
            }
        }

        [Test]
        public void Every_timeline_track_has_one_persistent_named_8_5_second_animation_clip()
        {
            var tracks = LoadTimeline().GetRootTracks().Cast<AnimationTrack>().ToArray();
            for (var i = 0; i < tracks.Length; i++)
            {
                var timelineClip = tracks[i].GetClips().Single();
                var playable = timelineClip.asset as AnimationPlayableAsset;
                Assert.That(timelineClip.start, Is.EqualTo(0d), tracks[i].name);
                Assert.That(timelineClip.duration, Is.EqualTo(8.5d).Within(0.0001d), tracks[i].name);
                Assert.That(playable, Is.Not.Null, tracks[i].name);
                Assert.That(playable.clip, Is.SameAs(LoadClip(i)), tracks[i].name);
                Assert.That(AssetDatabase.GetAssetPath(playable.clip), Is.EqualTo(ClipPaths[i]), tracks[i].name);
                Assert.That(playable.clip.frameRate, Is.EqualTo(24f), tracks[i].name);
            }
        }

        [Test]
        public void All_five_cast_sprite_sequences_match_the_seven_approved_beat_frames()
        {
            AssertSpriteSequence(2,
                "p1_seated_idle", "p1_speaking", "p1_speaking", "p1_laugh", "p1_rise_start", "p1_rise_start", "p1_rise_start");
            AssertSpriteSequence(3,
                "p2_seated_idle", "p2_seated_idle", "p2_nod_smile", "p2_laugh", "p2_laugh", "p2_neutral", "p2_neutral");
            AssertSpriteSequence(4,
                "p3_seated_work", "p3_seated_work", "p3_seated_work", "p3_shoulder_laugh", "p3_head_turn", "p3_neutral", "p3_neutral");
            AssertSpriteSequence(5,
                "p4_standing_idle", "p4_standing_idle", "p4_conversation", "p4_conversation", "p4_exit_turn", "p4_exit_turn", "p4_exit_turn");
            AssertSpriteSequence(6,
                "p5_standing_idle", "p5_standing_idle", "p5_standing_idle", "p5_laugh", "p5_exit_step", "p5_exit_step", "p5_exit_step");
        }

        [Test]
        public void Session_capture_restore_round_trips_scene_setup_and_active_scene()
        {
            var testBaseline = CaptureEditorState();
            Assert.That(A01OfficePreviewSession.HasSnapshot, Is.False, "A clean SessionState baseline is required.");
            try
            {
                EditorSceneManager.OpenScene(StartupScenePath, OpenSceneMode.Single);
                var previewScene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Additive);
                Assert.That(EditorSceneManager.SetActiveScene(previewScene), Is.True);
                var expectedSetup = EditorSceneManager.GetSceneManagerSetup();
                var expectedActivePath = EditorSceneManager.GetActiveScene().path;

                A01OfficePreviewSession.CaptureCurrent();
                EditorSceneManager.OpenScene(StartupScenePath, OpenSceneMode.Single);

                Assert.That(A01OfficePreviewSession.RestoreAndClear(), Is.True);
                AssertSceneSetupEqual(expectedSetup, EditorSceneManager.GetSceneManagerSetup());
                Assert.That(EditorSceneManager.GetActiveScene().path, Is.EqualTo(expectedActivePath));
            }
            finally
            {
                RestoreEditorState(testBaseline);
            }
        }

        [Test]
        public void Session_capture_restore_round_trips_previous_play_mode_start_scene()
        {
            var testBaseline = CaptureEditorState();
            Assert.That(A01OfficePreviewSession.HasSnapshot, Is.False, "A clean SessionState baseline is required.");
            try
            {
                var startup = AssetDatabase.LoadAssetAtPath<SceneAsset>(StartupScenePath);
                Assert.That(startup, Is.Not.Null);
                EditorSceneManager.playModeStartScene = startup;
                A01OfficePreviewSession.CaptureCurrent();
                EditorSceneManager.playModeStartScene = null;

                Assert.That(A01OfficePreviewSession.RestoreAndClear(), Is.True);
                Assert.That(
                    AssetDatabase.GetAssetPath(EditorSceneManager.playModeStartScene),
                    Is.EqualTo(AssetDatabase.GetAssetPath(startup)));
            }
            finally
            {
                RestoreEditorState(testBaseline);
            }
        }

        [Test]
        public void Session_restore_consumes_snapshot_and_a_second_restore_returns_false()
        {
            var testBaseline = CaptureEditorState();
            Assert.That(A01OfficePreviewSession.HasSnapshot, Is.False, "A clean SessionState baseline is required.");
            try
            {
                A01OfficePreviewSession.CaptureCurrent();
                Assert.That(A01OfficePreviewSession.HasSnapshot, Is.True);
                Assert.That(A01OfficePreviewSession.RestoreAndClear(), Is.True);
                Assert.That(A01OfficePreviewSession.HasSnapshot, Is.False);
                Assert.That(A01OfficePreviewSession.RestoreAndClear(), Is.False);
            }
            finally
            {
                RestoreEditorState(testBaseline);
            }
        }

        [Test]
        public void Launcher_restore_interrupted_preview_restores_setup_active_scene_and_start_scene()
        {
            var testBaseline = CaptureEditorState();
            Assert.That(A01OfficePreviewSession.HasSnapshot, Is.False, "A clean SessionState baseline is required.");
            try
            {
                var startup = AssetDatabase.LoadAssetAtPath<SceneAsset>(StartupScenePath);
                Assert.That(startup, Is.Not.Null);
                EditorSceneManager.playModeStartScene = startup;
                var expectedSetup = EditorSceneManager.GetSceneManagerSetup();
                var expectedActivePath = EditorSceneManager.GetActiveScene().path;
                A01OfficePreviewSession.CaptureCurrent();

                EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
                EditorSceneManager.playModeStartScene = null;

                Assert.That(A01OfficePreviewLauncher.RestoreInterruptedPreview(), Is.True);
                AssertSceneSetupEqual(expectedSetup, EditorSceneManager.GetSceneManagerSetup());
                Assert.That(EditorSceneManager.GetActiveScene().path, Is.EqualTo(expectedActivePath));
                Assert.That(
                    AssetDatabase.GetAssetPath(EditorSceneManager.playModeStartScene),
                    Is.EqualTo(AssetDatabase.GetAssetPath(startup)));
                Assert.That(A01OfficePreviewSession.HasSnapshot, Is.False);
            }
            finally
            {
                RestoreEditorState(testBaseline);
            }
        }

        [Test]
        public void All_position_keys_use_exact_even_frames_centimetre_values_and_constant_tangents()
        {
            var expected = ExpectedPositions();
            for (var clipIndex = 0; clipIndex < ClipPaths.Length; clipIndex++)
            {
                var clip = LoadClip(clipIndex);
                var bindings = AnimationUtility.GetCurveBindings(clip);
                Assert.That(bindings.Select(binding => binding.propertyName),
                    Is.EquivalentTo(new[] { "m_LocalPosition.x", "m_LocalPosition.y", "m_LocalPosition.z" }), ClipPaths[clipIndex]);
                foreach (var binding in bindings)
                {
                    var axis = binding.propertyName.EndsWith(".x") ? 0 : binding.propertyName.EndsWith(".y") ? 1 : 2;
                    var curve = AnimationUtility.GetEditorCurve(clip, binding);
                    Assert.That(curve.keys, Has.Length.EqualTo(BeatFrames.Length), binding.propertyName);
                    for (var keyIndex = 0; keyIndex < curve.keys.Length; keyIndex++)
                    {
                        var key = curve.keys[keyIndex];
                        var frame = Mathf.RoundToInt(key.time * 24f);
                        Assert.That(key.time * 24f, Is.EqualTo(frame).Within(0.0001f));
                        Assert.That(frame, Is.EqualTo(BeatFrames[keyIndex]));
                        Assert.That(frame % 2, Is.Zero);
                        Assert.That(key.value, Is.EqualTo(expected[clipIndex][keyIndex][axis]).Within(0.0001f));
                        Assert.That(key.value * 100f, Is.EqualTo(Mathf.Round(key.value * 100f)).Within(0.0001f));
                        Assert.That(AnimationUtility.GetKeyLeftTangentMode(curve, keyIndex), Is.EqualTo(AnimationUtility.TangentMode.Constant));
                        Assert.That(AnimationUtility.GetKeyRightTangentMode(curve, keyIndex), Is.EqualTo(AnimationUtility.TangentMode.Constant));
                    }
                }
            }
        }

        [Test]
        public void P3_position_response_is_exactly_0_04_units_and_weaker_than_every_primary_actor()
        {
            var p3 = MaximumExcursion(LoadClip(4));
            Assert.That(p3, Is.EqualTo(0.04f).Within(0.0001f));
            foreach (var primaryIndex in new[] { 2, 3, 5, 6 })
            {
                Assert.That(p3, Is.LessThan(MaximumExcursion(LoadClip(primaryIndex))), ClipPaths[primaryIndex]);
            }
        }

        [Test]
        public void Scene_contains_no_audio_source_particle_system_canvas_or_text_component()
        {
            var root = OpenRoot();
            Assert.That(root.GetComponentsInChildren<AudioSource>(true), Is.Empty);
            Assert.That(root.GetComponentsInChildren<ParticleSystem>(true), Is.Empty);
            Assert.That(root.GetComponentsInChildren<Canvas>(true), Is.Empty);
            Assert.That(root.GetComponentsInChildren<TextMesh>(true), Is.Empty);
            var forbiddenTextTypes = new[] { "UnityEngine.UI.Text", "TMPro.TextMeshPro", "TMPro.TextMeshProUGUI" };
            Assert.That(root.GetComponentsInChildren<Component>(true)
                .Where(component => component != null && forbiddenTextTypes.Contains(component.GetType().FullName)), Is.Empty);
            var ambient = root.transform.Find("VisualRoot/FX_Ambient");
            Assert.That(ambient.gameObject.activeSelf, Is.False);
        }

        [Test]
        public void A01_is_absent_first_two_startup_entries_and_five_protected_lf_baselines_are_unchanged()
        {
            var scenes = EditorBuildSettings.scenes;
            Assert.That(scenes.Any(scene => scene.path == ScenePath), Is.False);
            Assert.That(scenes, Has.Length.GreaterThanOrEqualTo(2));
            Assert.That(scenes[0].path, Is.EqualTo("Assets/_Project/Scenes/Startup.unity"));
            Assert.That(scenes[0].enabled, Is.True);
            Assert.That(scenes[1].path, Is.EqualTo("Assets/_Project/Scenes/RatHost2DPrototype.unity"));
            Assert.That(scenes[1].enabled, Is.True);
            AssertProtectedLfBaseline("Packages/manifest.json", 2069, "B07DD4E37BA1336B93D763B23E3480BE7943EF4C56DBFDA7EE191FF87B0AF298");
            AssertProtectedLfBaseline("Packages/packages-lock.json", 13840, "943F92F1229C2A366FD42AA7180B73BDB8B6019AE21C1A6CE38C80A15D8C262E");
            AssertProtectedLfBaseline("ProjectSettings/EditorBuildSettings.asset", 799, "67B153F8C73C6C9E7F8C60D47D03A837DFEC207E757AC65FEB6619F58BE28755");
            AssertProtectedLfBaseline("Assets/_Project/Editor/Startup/StartupPlayModeBootstrap.cs", 1346, "634BD355DF765B7283774D3B20983299F2637C8F0503B831057535F58133E5C2");
            AssertProtectedLfBaseline("Assets/_Project/Scripts/UI/Startup/StartupController.cs", 15040, "042B816E531448ABD5DC265C183D309AE1E084E25581E8DF9D4E48FE73931730");
        }

        [Test]
        public void Public_launcher_session_apis_and_both_menus_exist_without_mutating_editor_setup()
        {
            var setup = EditorSceneManager.GetSceneManagerSetup();
            var activePath = EditorSceneManager.GetActiveScene().path;
            var startScene = EditorSceneManager.playModeStartScene;
            var snapshotBefore = A01OfficePreviewSession.HasSnapshot;
            Func<bool> readSnapshot = () => A01OfficePreviewSession.HasSnapshot;
            Action captureCurrent = A01OfficePreviewSession.CaptureCurrent;
            Func<bool> restoreAndClear = A01OfficePreviewSession.RestoreAndClear;
            Action clear = A01OfficePreviewSession.Clear;
            Action playPreview = A01OfficePreviewLauncher.PlayPreview;
            Func<bool> restoreInterruptedPreview = A01OfficePreviewLauncher.RestoreInterruptedPreview;

            Assert.That(readSnapshot(), Is.EqualTo(snapshotBefore));
            Assert.That(new Delegate[] { captureCurrent, restoreAndClear, clear, playPreview, restoreInterruptedPreview },
                Has.All.Not.Null);
            Assert.That(Menu.GetEnabled("Last Host/Cinematics/A01/Rebuild Preview"), Is.True);
            Assert.That(Menu.GetEnabled("Last Host/Cinematics/A01/Play Preview"), Is.True);

            AssertSceneSetupEqual(setup, EditorSceneManager.GetSceneManagerSetup());
            Assert.That(EditorSceneManager.GetActiveScene().path, Is.EqualTo(activePath));
            Assert.That(EditorSceneManager.playModeStartScene, Is.SameAs(startScene));
        }

        private static GameObject OpenRoot()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Assert.That(scene.IsValid() && scene.isLoaded, Is.True);
            Assert.That(scene.GetRootGameObjects(), Has.Length.EqualTo(1));
            var root = scene.GetRootGameObjects()[0];
            Assert.That(root.name, Is.EqualTo("A01_Office_Animatic"));
            return root;
        }

        private static TimelineAsset LoadTimeline()
        {
            var timeline = AssetDatabase.LoadAssetAtPath<TimelineAsset>(TimelinePath);
            Assert.That(timeline, Is.Not.Null, TimelinePath);
            return timeline;
        }

        private static AnimationClip LoadClip(int index)
        {
            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(ClipPaths[index]);
            Assert.That(clip, Is.Not.Null, ClipPaths[index]);
            return clip;
        }

        private static Dictionary<string, AssetIdentity> CaptureAssetIdentities(IEnumerable<string> paths)
        {
            var result = new Dictionary<string, AssetIdentity>();
            foreach (var path in paths)
            {
                Assert.That(AssetDatabase.LoadMainAssetAtPath(path), Is.Not.Null, path);
                result.Add(path, new AssetIdentity(AssetDatabase.AssetPathToGUID(path), AssetDatabase.GetAssetDependencyHash(path)));
            }
            return result;
        }

        private static string[] GeneratedAssetPaths()
        {
            return new[] { ScenePath, TimelinePath }.Concat(ClipPaths).ToArray();
        }

        private static void AssertAssetIdentity(AssetIdentity expected, AssetIdentity actual, string transition, string path)
        {
            Assert.That(actual.Guid, Is.EqualTo(expected.Guid), transition + ": " + path + " GUID");
            Assert.That(actual.Hash, Is.EqualTo(expected.Hash), transition + ": " + path + " dependency hash");
        }

        private static void AssertSpriteSequence(int clipIndex, params string[] spriteNames)
        {
            var clip = LoadClip(clipIndex);
            var bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
            Assert.That(bindings, Has.Length.EqualTo(1));
            Assert.That(bindings[0].type, Is.EqualTo(typeof(SpriteRenderer)));
            Assert.That(bindings[0].path, Is.Empty);
            Assert.That(bindings[0].propertyName, Is.EqualTo("m_Sprite"));
            var keys = AnimationUtility.GetObjectReferenceCurve(clip, bindings[0]);
            Assert.That(keys, Has.Length.EqualTo(BeatFrames.Length));
            Assert.That(spriteNames, Has.Length.EqualTo(BeatFrames.Length));
            for (var i = 0; i < keys.Length; i++)
            {
                Assert.That(keys[i].time * 24f, Is.EqualTo(BeatFrames[i]).Within(0.0001f));
                var sprite = keys[i].value as Sprite;
                Assert.That(sprite, Is.Not.Null, "frame " + BeatFrames[i]);
                Assert.That(sprite.name, Is.EqualTo(spriteNames[i]), "frame " + BeatFrames[i]);
                Assert.That(AssetDatabase.GetAssetPath(sprite), Is.EqualTo(CastPath), "frame " + BeatFrames[i]);
            }
        }

        private static Vector3[][] ExpectedPositions()
        {
            return new[]
            {
                V((0,0,-10),(-.20,0,-10),(-.30,0,-10),(-.30,0,-10),(0,0,-10),(.50,0,-10),(.50,0,-10)),
                V((0,0,0),(0,0,0),(0,0,0),(0,0,0),(0,0,0),(0,0,0),(0,0,0)),
                V((-2.10,-2.70,0),(-2.10,-2.70,0),(-2.10,-2.70,0),(-2.10,-2.64,0),(-2.00,-2.55,0),(-1.90,-2.40,0),(-1.90,-2.40,0)),
                V((-5.35,-3.80,0),(-5.35,-3.80,0),(-5.35,-3.78,0),(-5.35,-3.72,0),(-5.35,-3.72,0),(-5.35,-3.80,0),(-5.35,-3.80,0)),
                V((.15,-3.80,0),(.15,-3.80,0),(.15,-3.80,0),(.15,-3.76,0),(.18,-3.80,0),(.15,-3.80,0),(.15,-3.80,0)),
                V((2.75,-3.80,0),(2.75,-3.80,0),(2.70,-3.80,0),(2.70,-3.80,0),(2.90,-3.80,0),(3.20,-3.80,0),(3.20,-3.80,0)),
                V((5.10,-3.80,0),(5.10,-3.80,0),(5.10,-3.80,0),(5.10,-3.74,0),(5.30,-3.80,0),(5.60,-3.80,0),(5.60,-3.80,0))
            };
        }

        private static Vector3[] V(params (double x, double y, double z)[] values) =>
            values.Select(value => new Vector3((float)value.x, (float)value.y, (float)value.z)).ToArray();

        private static float MaximumExcursion(AnimationClip clip)
        {
            var bindings = AnimationUtility.GetCurveBindings(clip).ToDictionary(binding => binding.propertyName);
            var curves = new[] { "m_LocalPosition.x", "m_LocalPosition.y", "m_LocalPosition.z" }
                .Select(name => AnimationUtility.GetEditorCurve(clip, bindings[name])).ToArray();
            var first = new Vector3(curves[0].keys[0].value, curves[1].keys[0].value, curves[2].keys[0].value);
            return Enumerable.Range(0, BeatFrames.Length).Select(index =>
                Vector3.Distance(first, new Vector3(curves[0].keys[index].value, curves[1].keys[index].value, curves[2].keys[index].value))).Max();
        }

        private static void AssertTransform(GameObject root, string path, Vector3 position, Vector3 scale)
        {
            var target = root.transform.Find(path);
            Assert.That(target, Is.Not.Null, path);
            Assert.That(target.localPosition, Is.EqualTo(position), path);
            Assert.That(target.localScale, Is.EqualTo(scale), path);
            Assert.That(target.localRotation, Is.EqualTo(Quaternion.identity), path);
        }

        private static string HierarchyPath(Transform transform)
        {
            var names = new Stack<string>();
            for (var current = transform; current != null; current = current.parent) names.Push(current.name);
            return string.Join("/", names);
        }

        private static string Sha256(string assetPath)
        {
            using (var stream = File.OpenRead(ProjectPath(assetPath)))
            using (var sha = SHA256.Create())
            {
                return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", string.Empty);
            }
        }

        private static string ProjectPath(string projectRelativePath)
        {
            var projectRoot = Directory.GetParent(Application.dataPath).FullName;
            return Path.GetFullPath(Path.Combine(projectRoot, projectRelativePath));
        }

        private static void AssertProtectedLfBaseline(string projectRelativePath, int expectedBytes, string expectedSha)
        {
            var raw = File.ReadAllBytes(ProjectPath(projectRelativePath));
            var normalized = new List<byte>(raw.Length);
            for (var index = 0; index < raw.Length; index++)
            {
                if (raw[index] != 13)
                {
                    normalized.Add(raw[index]);
                    continue;
                }

                if (index + 1 >= raw.Length || raw[index + 1] != 10)
                {
                    Assert.Fail("Lone CR in protected baseline " + projectRelativePath + " at byte " + index + ".");
                }

                normalized.Add(10);
                index++;
            }

            var canonicalBytes = normalized.ToArray();
            string actualSha;
            using (var sha = SHA256.Create())
            {
                actualSha = BitConverter.ToString(sha.ComputeHash(canonicalBytes)).Replace("-", string.Empty);
            }

            Assert.That(canonicalBytes.Length, Is.EqualTo(expectedBytes), projectRelativePath + " LF-normalized bytes");
            Assert.That(actualSha, Is.EqualTo(expectedSha), projectRelativePath + " LF-normalized SHA-256");
        }

        private static EditorState CaptureEditorState()
        {
            return new EditorState(
                EditorSceneManager.GetSceneManagerSetup(),
                EditorSceneManager.GetActiveScene().path,
                EditorSceneManager.playModeStartScene);
        }

        private static void RestoreEditorState(EditorState state)
        {
            try
            {
                A01OfficePreviewSession.Clear();
                if (state.Setup != null && state.Setup.Length > 0)
                {
                    EditorSceneManager.RestoreSceneManagerSetup(state.Setup);
                }

                if (!string.IsNullOrEmpty(state.ActiveScenePath))
                {
                    var activeScene = SceneManager.GetSceneByPath(state.ActiveScenePath);
                    if (activeScene.IsValid() && activeScene.isLoaded)
                    {
                        EditorSceneManager.SetActiveScene(activeScene);
                    }
                }
            }
            finally
            {
                EditorSceneManager.playModeStartScene = state.StartScene;
            }
        }

        private static void AssertSceneSetupEqual(SceneSetup[] expected, SceneSetup[] actual)
        {
            Assert.That(actual, Has.Length.EqualTo(expected.Length));
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.That(actual[i].path, Is.EqualTo(expected[i].path));
                Assert.That(actual[i].isLoaded, Is.EqualTo(expected[i].isLoaded));
                Assert.That(actual[i].isActive, Is.EqualTo(expected[i].isActive));
            }
        }

        private readonly struct AssetIdentity
        {
            public AssetIdentity(string guid, Hash128 hash) { Guid = guid; Hash = hash; }
            public string Guid { get; }
            public Hash128 Hash { get; }
        }

        private readonly struct EditorState
        {
            public EditorState(SceneSetup[] setup, string activeScenePath, SceneAsset startScene)
            {
                Setup = setup;
                ActiveScenePath = activeScenePath;
                StartScene = startScene;
            }

            public SceneSetup[] Setup { get; }
            public string ActiveScenePath { get; }
            public SceneAsset StartScene { get; }
        }
    }
}
