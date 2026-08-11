using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace LastHost.Prototype.Cinematics.A01.Editor
{
    public static class A01OfficeAnimaticSceneBuilder
    {
        private const int TimelineFps = 24;
        private const int TotalFrames = 204;
        private const double DurationSeconds = 8.5;

        private const string ScenePath =
            "Assets/_Project/Scenes/Cinematics/Opening/A01OfficeAnimatic.unity";
        private const string TimelinePath =
            "Assets/_Project/Timelines/Cinematics/Opening/A01/A01OfficeAnimatic.playable";
        private const string AnimationRoot =
            "Assets/_Project/Animations/Cinematics/Opening/A01/Preview";
        private const string ArtRoot =
            "Assets/_Project/Art/Cinematics/Opening/A01/Office";
        private const string BackgroundPath = ArtRoot + "/a01-office-background-v1.png";
        private const string CastPath = ArtRoot + "/a01-office-cast-poses-v1.png";
        private const string OcclusionMaskPath = ArtRoot + "/a01-office-occlusion-mask-v1.png";

        private static readonly int[] BeatFrames = { 0, 36, 72, 108, 138, 180, TotalFrames };

        private static readonly string[] TrackNames =
        {
            "CameraTrack",
            "BackgroundTrack",
            "P1_SpeakerTrack",
            "P2_ReactorTrack",
            "P3_WorkerTrack",
            "P4_ExitLeadTrack",
            "P5_ExitFollowTrack"
        };

        private static readonly string[] ClipNames =
        {
            "A01_Camera_Preview",
            "A01_Background_Preview",
            "A01_P1_Preview",
            "A01_P2_Preview",
            "A01_P3_Preview",
            "A01_P4_Preview",
            "A01_P5_Preview"
        };

        private static readonly string[][] PoseNames =
        {
            new[]
            {
                "p1_seated_idle", "p1_speaking", "p1_speaking", "p1_laugh",
                "p1_rise_start", "p1_rise_start", "p1_rise_start"
            },
            new[]
            {
                "p2_seated_idle", "p2_seated_idle", "p2_nod_smile", "p2_laugh",
                "p2_laugh", "p2_neutral", "p2_neutral"
            },
            new[]
            {
                "p3_seated_work", "p3_seated_work", "p3_seated_work", "p3_shoulder_laugh",
                "p3_head_turn", "p3_neutral", "p3_neutral"
            },
            new[]
            {
                "p4_standing_idle", "p4_standing_idle", "p4_conversation", "p4_conversation",
                "p4_exit_turn", "p4_exit_turn", "p4_exit_turn"
            },
            new[]
            {
                "p5_standing_idle", "p5_standing_idle", "p5_standing_idle", "p5_laugh",
                "p5_exit_step", "p5_exit_step", "p5_exit_step"
            }
        };

        private static readonly Vector3[][] PositionKeys =
        {
            new[]
            {
                new Vector3(0f, 0f, -10f),
                new Vector3(-0.20f, 0f, -10f),
                new Vector3(-0.30f, 0f, -10f),
                new Vector3(-0.30f, 0f, -10f),
                new Vector3(0f, 0f, -10f),
                new Vector3(0.50f, 0f, -10f),
                new Vector3(0.50f, 0f, -10f)
            },
            Repeat(new Vector3(0f, 0f, 0f)),
            new[]
            {
                new Vector3(-2.10f, -2.70f, 0f),
                new Vector3(-2.10f, -2.70f, 0f),
                new Vector3(-2.10f, -2.70f, 0f),
                new Vector3(-2.10f, -2.64f, 0f),
                new Vector3(-2.00f, -2.55f, 0f),
                new Vector3(-1.90f, -2.40f, 0f),
                new Vector3(-1.90f, -2.40f, 0f)
            },
            new[]
            {
                new Vector3(-5.35f, -3.80f, 0f),
                new Vector3(-5.35f, -3.80f, 0f),
                new Vector3(-5.35f, -3.78f, 0f),
                new Vector3(-5.35f, -3.72f, 0f),
                new Vector3(-5.35f, -3.72f, 0f),
                new Vector3(-5.35f, -3.80f, 0f),
                new Vector3(-5.35f, -3.80f, 0f)
            },
            new[]
            {
                new Vector3(0.15f, -3.80f, 0f),
                new Vector3(0.15f, -3.80f, 0f),
                new Vector3(0.15f, -3.80f, 0f),
                new Vector3(0.15f, -3.76f, 0f),
                new Vector3(0.18f, -3.80f, 0f),
                new Vector3(0.15f, -3.80f, 0f),
                new Vector3(0.15f, -3.80f, 0f)
            },
            new[]
            {
                new Vector3(2.75f, -3.80f, 0f),
                new Vector3(2.75f, -3.80f, 0f),
                new Vector3(2.70f, -3.80f, 0f),
                new Vector3(2.70f, -3.80f, 0f),
                new Vector3(2.90f, -3.80f, 0f),
                new Vector3(3.20f, -3.80f, 0f),
                new Vector3(3.20f, -3.80f, 0f)
            },
            new[]
            {
                new Vector3(5.10f, -3.80f, 0f),
                new Vector3(5.10f, -3.80f, 0f),
                new Vector3(5.10f, -3.80f, 0f),
                new Vector3(5.10f, -3.74f, 0f),
                new Vector3(5.30f, -3.80f, 0f),
                new Vector3(5.60f, -3.80f, 0f),
                new Vector3(5.60f, -3.80f, 0f)
            }
        };

        private static readonly float[] CharacterScales = { 1.70f, 2.00f, 2.10f, 2.40f, 2.40f };
        private static readonly int[] CharacterSortingOrders = { 110, 100, 120, 130, 140 };

        [MenuItem("Last Host/Cinematics/A01/Rebuild Preview")]
        public static void RebuildPreview()
        {
            EnsureFolder("Assets/_Project/Scenes/Cinematics/Opening");
            EnsureFolder("Assets/_Project/Timelines/Cinematics/Opening/A01");
            EnsureFolder(AnimationRoot);

            var background = RequireAsset<Sprite>(BackgroundPath);
            var maskSprite = RequireAsset<Sprite>(OcclusionMaskPath);
            var castSprites = LoadCastSprites();
            var animationClips = BuildAnimationClips(castSprites);
            var timeline = LoadOrCreateTimeline();

            var scene = LoadOrCreateScene();
            var root = LoadOrCreateSceneRoot(scene);
            ValidateDirectChildren(root.transform, "CameraRig", "VisualRoot", "Timeline");

            var animators = new Animator[TrackNames.Length];
            CreateCamera(root.transform, out animators[0]);
            CreateVisuals(root.transform, background, maskSprite, castSprites, animators);
            CreateTimeline(root.transform, timeline, animationClips, animators);

            if (!SaveSceneWithoutReassigningExistingAsset(scene))
            {
                throw new InvalidOperationException("Failed to save A01 preview scene at " + ScenePath + ".");
            }

            AssetDatabase.SaveAssets();
        }

        private static void CreateCamera(Transform root, out Animator animator)
        {
            var rig = GetOrCreateDirectChild(root, "CameraRig", 0);
            ValidateAllowedComponents(rig, typeof(Transform));
            ValidateDirectChildren(rig.transform, "Main Camera");
            ConfigureGameObject(rig, true, Vector3.zero, Vector3.one);

            var cameraObject = GetOrCreateDirectChild(rig.transform, "Main Camera", 0);
            ValidateAllowedComponents(cameraObject, typeof(Transform), typeof(Camera), typeof(Animator));
            ValidateDirectChildren(cameraObject.transform);
            ConfigureGameObject(cameraObject, true, PositionKeys[0][0], Vector3.one);

            var camera = GetOrAddSingleComponent<Camera>(cameraObject);
            camera.enabled = true;
            camera.orthographic = true;
            camera.orthographicSize = 5.40f;
            camera.aspect = 16f / 9f;
            camera.rect = new Rect(0f, 0f, 1f, 1f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.16f, 0.13f, 0.11f, 1f);

            animator = AddTimelineAnimator(cameraObject);
        }

        private static void CreateVisuals(
            Transform root,
            Sprite background,
            Sprite maskSprite,
            IReadOnlyDictionary<string, Sprite> castSprites,
            Animator[] animators)
        {
            var visualRoot = GetOrCreateDirectChild(root, "VisualRoot", 1);
            ValidateAllowedComponents(visualRoot, typeof(Transform));
            ValidateDirectChildren(
                visualRoot.transform,
                "BG_Room",
                "CHAR_P1",
                "CHAR_P2",
                "CHAR_P3",
                "CHAR_P4",
                "CHAR_P5",
                "FG_BackgroundRepeat",
                "FG_OcclusionMask",
                "FX_Ambient");
            ConfigureGameObject(visualRoot, true, Vector3.zero, Vector3.one);

            var backgroundObject = GetOrCreateDirectChild(visualRoot.transform, "BG_Room", 0);
            ValidateAllowedComponents(
                backgroundObject,
                typeof(Transform),
                typeof(SpriteRenderer),
                typeof(Animator));
            ConfigureSpriteObject(
                backgroundObject,
                background,
                0,
                PositionKeys[1][0],
                Vector3.one);
            animators[1] = AddTimelineAnimator(backgroundObject);

            for (var index = 0; index < PoseNames.Length; index++)
            {
                var characterObject = GetOrCreateDirectChild(
                    visualRoot.transform,
                    "CHAR_P" + (index + 1),
                    index + 1);
                ValidateAllowedComponents(
                    characterObject,
                    typeof(Transform),
                    typeof(SpriteRenderer),
                    typeof(Animator));
                ConfigureSpriteObject(
                    characterObject,
                    castSprites[PoseNames[index][0]],
                    CharacterSortingOrders[index],
                    PositionKeys[index + 2][0],
                    new Vector3(CharacterScales[index], CharacterScales[index], 1f));
                animators[index + 2] = AddTimelineAnimator(characterObject);
            }

            var repeatObject = GetOrCreateDirectChild(visualRoot.transform, "FG_BackgroundRepeat", 6);
            ValidateAllowedComponents(repeatObject, typeof(Transform), typeof(SpriteRenderer));
            ConfigureSpriteObject(
                repeatObject,
                background,
                200,
                backgroundObject.transform.localPosition,
                backgroundObject.transform.localScale);
            repeatObject.transform.localRotation = backgroundObject.transform.localRotation;
            var repeatRenderer = repeatObject.GetComponent<SpriteRenderer>();
            repeatRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            var maskObject = GetOrCreateDirectChild(visualRoot.transform, "FG_OcclusionMask", 7);
            ValidateAllowedComponents(maskObject, typeof(Transform), typeof(SpriteMask));
            ValidateDirectChildren(maskObject.transform);
            maskObject.SetActive(true);
            CopyLocalTransform(backgroundObject.transform, maskObject.transform);
            var spriteMask = GetOrAddSingleComponent<SpriteMask>(maskObject);
            spriteMask.enabled = true;
            spriteMask.sprite = maskSprite;
            spriteMask.alphaCutoff = 0.5f;
            spriteMask.isCustomRangeActive = false;
            spriteMask.frontSortingLayerID = repeatRenderer.sortingLayerID;
            spriteMask.frontSortingOrder = 200;
            spriteMask.backSortingLayerID = repeatRenderer.sortingLayerID;
            spriteMask.backSortingOrder = 200;
            spriteMask.isCustomRangeActive = true;
            if (!spriteMask.isCustomRangeActive ||
                spriteMask.frontSortingLayerID != repeatRenderer.sortingLayerID ||
                spriteMask.backSortingLayerID != repeatRenderer.sortingLayerID ||
                spriteMask.frontSortingOrder != 200 ||
                spriteMask.backSortingOrder != 200)
            {
                throw new InvalidOperationException(
                    "A01 foreground mask custom range must resolve to the repeat renderer at order 200 only.");
            }

            var fx = GetOrCreateDirectChild(visualRoot.transform, "FX_Ambient", 8);
            ValidateAllowedComponents(fx, typeof(Transform));
            ValidateDirectChildren(fx.transform);
            ConfigureGameObject(fx, false, Vector3.zero, Vector3.one);
        }

        private static void CreateTimeline(
            Transform root,
            TimelineAsset timeline,
            AnimationClip[] animationClips,
            Animator[] animators)
        {
            var timelineObject = GetOrCreateDirectChild(root, "Timeline", 2);
            ValidateAllowedComponents(timelineObject, typeof(Transform), typeof(PlayableDirector));
            ValidateDirectChildren(timelineObject.transform);
            ConfigureGameObject(timelineObject, true, Vector3.zero, Vector3.one);

            var director = GetOrAddSingleComponent<PlayableDirector>(timelineObject);
            director.enabled = true;
            director.playOnAwake = true;
            director.extrapolationMode = DirectorWrapMode.None;
            director.initialTime = 0d;
            if (director.playableAsset != timeline)
            {
                director.playableAsset = timeline;
            }

            var tracks = LoadOrCreateTracks(timeline);
            for (var index = 0; index < tracks.Length; index++)
            {
                ConfigureTimelineClip(tracks[index], animationClips[index]);
                if (director.GetGenericBinding(tracks[index]) != animators[index])
                {
                    director.SetGenericBinding(tracks[index], animators[index]);
                }
            }

            EditorUtility.SetDirty(timeline);
        }

        private static AnimationClip[] BuildAnimationClips(IReadOnlyDictionary<string, Sprite> castSprites)
        {
            var clips = new AnimationClip[ClipNames.Length];
            for (var index = 0; index < clips.Length; index++)
            {
                clips[index] = LoadOrCreateAnimationClip(ClipNames[index]);
                SetPositionCurves(clips[index], PositionKeys[index]);

                if (index >= 2)
                {
                    SetSpriteCurve(clips[index], PoseNames[index - 2], castSprites);
                }

                EditorUtility.SetDirty(clips[index]);
            }

            return clips;
        }

        private static AnimationClip LoadOrCreateAnimationClip(string clipName)
        {
            var path = AnimationRoot + "/" + clipName + ".anim";
            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (clip == null)
            {
                ThrowIfPathOccupied(path, typeof(AnimationClip));
                clip = new AnimationClip();
                AssetDatabase.CreateAsset(clip, path);
            }

            clip.name = clipName;
            clip.frameRate = TimelineFps;
            clip.legacy = false;
            clip.wrapMode = WrapMode.Once;
            clip.ClearCurves();
            AnimationUtility.SetAnimationEvents(clip, Array.Empty<AnimationEvent>());
            return clip;
        }

        private static void SetPositionCurves(AnimationClip clip, Vector3[] positions)
        {
            if (positions.Length != BeatFrames.Length)
            {
                throw new InvalidOperationException(clip.name + " must have one position per A01 beat boundary.");
            }

            SetConstantFloatCurve(clip, "m_LocalPosition.x", positions, 0);
            SetConstantFloatCurve(clip, "m_LocalPosition.y", positions, 1);
            SetConstantFloatCurve(clip, "m_LocalPosition.z", positions, 2);
        }

        private static void SetConstantFloatCurve(
            AnimationClip clip,
            string propertyName,
            Vector3[] positions,
            int axis)
        {
            var curve = new AnimationCurve();
            for (var index = 0; index < BeatFrames.Length; index++)
            {
                var value = axis == 0 ? positions[index].x : axis == 1 ? positions[index].y : positions[index].z;
                ValidateHundredthGrid(value, clip.name, propertyName, BeatFrames[index]);
                curve.AddKey(new Keyframe(FrameToSeconds(BeatFrames[index]), value));
            }

            for (var index = 0; index < curve.length; index++)
            {
                AnimationUtility.SetKeyBroken(curve, index, true);
                AnimationUtility.SetKeyLeftTangentMode(curve, index, AnimationUtility.TangentMode.Constant);
                AnimationUtility.SetKeyRightTangentMode(curve, index, AnimationUtility.TangentMode.Constant);
            }

            var binding = EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), propertyName);
            AnimationUtility.SetEditorCurve(clip, binding, curve);
        }

        private static void SetSpriteCurve(
            AnimationClip clip,
            string[] poseNames,
            IReadOnlyDictionary<string, Sprite> castSprites)
        {
            if (poseNames.Length != BeatFrames.Length)
            {
                throw new InvalidOperationException(clip.name + " must have one pose per A01 beat boundary.");
            }

            var keys = new ObjectReferenceKeyframe[BeatFrames.Length];
            for (var index = 0; index < keys.Length; index++)
            {
                keys[index] = new ObjectReferenceKeyframe
                {
                    time = FrameToSeconds(BeatFrames[index]),
                    value = castSprites[poseNames[index]]
                };
            }

            var binding = EditorCurveBinding.PPtrCurve(string.Empty, typeof(SpriteRenderer), "m_Sprite");
            AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);
        }

        private static TimelineAsset LoadOrCreateTimeline()
        {
            var timeline = AssetDatabase.LoadAssetAtPath<TimelineAsset>(TimelinePath);
            if (timeline == null)
            {
                ThrowIfPathOccupied(TimelinePath, typeof(TimelineAsset));
                timeline = ScriptableObject.CreateInstance<TimelineAsset>();
                AssetDatabase.CreateAsset(timeline, TimelinePath);
            }

            timeline.name = "A01OfficeAnimatic";
            timeline.editorSettings.fps = TimelineFps;
            timeline.durationMode = TimelineAsset.DurationMode.FixedLength;
            timeline.fixedDuration = DurationSeconds;
            return timeline;
        }

        private static AnimationTrack[] LoadOrCreateTracks(TimelineAsset timeline)
        {
            var rootTracks = new List<TrackAsset>(timeline.GetRootTracks());
            var expectedLayout = rootTracks.Count == TrackNames.Length;
            if (expectedLayout)
            {
                for (var index = 0; index < rootTracks.Count; index++)
                {
                    if (!(rootTracks[index] is AnimationTrack) || rootTracks[index].name != TrackNames[index])
                    {
                        expectedLayout = false;
                        break;
                    }
                }
            }

            var tracks = new AnimationTrack[TrackNames.Length];
            if (expectedLayout)
            {
                for (var index = 0; index < tracks.Length; index++)
                {
                    tracks[index] = (AnimationTrack)rootTracks[index];
                }

                return tracks;
            }

            foreach (var rootTrack in rootTracks)
            {
                timeline.DeleteTrack(rootTrack);
            }

            for (var index = 0; index < tracks.Length; index++)
            {
                tracks[index] = timeline.CreateTrack<AnimationTrack>(null, TrackNames[index]);
            }

            return tracks;
        }

        private static void ConfigureTimelineClip(AnimationTrack track, AnimationClip persistentClip)
        {
            var clips = new List<TimelineClip>(track.GetClips());
            TimelineClip timelineClip = null;
            if (clips.Count == 1 && clips[0].asset is AnimationPlayableAsset)
            {
                timelineClip = clips[0];
            }
            else
            {
                foreach (var existingClip in clips)
                {
                    track.DeleteClip(existingClip);
                }

                timelineClip = track.CreateClip<AnimationPlayableAsset>();
            }

            var playable = (AnimationPlayableAsset)timelineClip.asset;
            playable.clip = persistentClip;
            playable.name = persistentClip.name + "_Playable";
            playable.position = Vector3.zero;
            playable.rotation = Quaternion.identity;
            playable.useTrackMatchFields = false;
            playable.removeStartOffset = false;
            playable.applyFootIK = false;
            playable.loop = AnimationPlayableAsset.LoopMode.Off;

            timelineClip.displayName = persistentClip.name;
            timelineClip.start = 0d;
            timelineClip.clipIn = 0d;
            timelineClip.timeScale = 1d;
            timelineClip.duration = DurationSeconds;
            timelineClip.easeInDuration = 0d;
            timelineClip.easeOutDuration = 0d;
            timelineClip.blendInDuration = 0d;
            timelineClip.blendOutDuration = 0d;

            EditorUtility.SetDirty(playable);
            EditorUtility.SetDirty(track);
        }

        private static IReadOnlyDictionary<string, Sprite> LoadCastSprites()
        {
            var sprites = new Dictionary<string, Sprite>(StringComparer.Ordinal);
            foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(CastPath))
            {
                if (asset is Sprite sprite)
                {
                    sprites[sprite.name] = sprite;
                }
            }

            foreach (var poseSet in PoseNames)
            {
                foreach (var poseName in poseSet)
                {
                    if (!sprites.ContainsKey(poseName))
                    {
                        throw new InvalidOperationException(
                            "Required A01 cast sprite '" + poseName + "' is missing from " + CastPath + ".");
                    }
                }
            }

            return sprites;
        }

        private static T RequireAsset<T>(string path) where T : UnityEngine.Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                throw new InvalidOperationException(
                    "Required A01 " + typeof(T).Name + " asset is missing at " + path + ".");
            }

            return asset;
        }

        private static void ThrowIfPathOccupied(string path, Type expectedType)
        {
            var existing = AssetDatabase.LoadMainAssetAtPath(path);
            if (existing != null)
            {
                throw new InvalidOperationException(
                    path + " is occupied by " + existing.GetType().Name + ", expected " + expectedType.Name + ".");
            }
        }

        private static Scene LoadOrCreateScene()
        {
            var existingAsset = AssetDatabase.LoadMainAssetAtPath(ScenePath);
            if (existingAsset != null)
            {
                if (!(existingAsset is SceneAsset))
                {
                    throw new InvalidOperationException(
                        ScenePath + " is occupied by " + existingAsset.GetType().Name + ", expected SceneAsset.");
                }

                return EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            }

            return EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static bool SaveSceneWithoutReassigningExistingAsset(Scene scene)
        {
            if (string.IsNullOrEmpty(scene.path))
            {
                return EditorSceneManager.SaveScene(scene, ScenePath);
            }

            if (!string.Equals(scene.path, ScenePath, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    "A01 preview builder opened unexpected scene path " + scene.path + ".");
            }

            return EditorSceneManager.SaveScene(scene);
        }

        private static GameObject LoadOrCreateSceneRoot(Scene scene)
        {
            var roots = scene.GetRootGameObjects();
            GameObject root;
            if (roots.Length == 0)
            {
                root = CreateMissingGameObject("A01_Office_Animatic", null);
                if (root.scene != scene)
                {
                    SceneManager.MoveGameObjectToScene(root, scene);
                }
            }
            else if (roots.Length == 1 && roots[0].name == "A01_Office_Animatic")
            {
                root = roots[0];
            }
            else
            {
                throw new InvalidOperationException(
                    "A01 preview scene must contain zero roots or the single root A01_Office_Animatic.");
            }

            ValidateAllowedComponents(root, typeof(Transform));
            ConfigureGameObject(root, true, Vector3.zero, Vector3.one);
            return root;
        }

        private static void ConfigureSpriteObject(
            GameObject gameObject,
            Sprite sprite,
            int sortingOrder,
            Vector3 localPosition,
            Vector3 localScale)
        {
            ValidateDirectChildren(gameObject.transform);
            ConfigureGameObject(gameObject, true, localPosition, localScale);

            var renderer = GetOrAddSingleComponent<SpriteRenderer>(gameObject);
            renderer.enabled = true;
            renderer.sprite = sprite;
            renderer.sortingLayerID = 0;
            renderer.sortingOrder = sortingOrder;
            renderer.maskInteraction = SpriteMaskInteraction.None;
        }

        private static Animator AddTimelineAnimator(GameObject gameObject)
        {
            var animator = GetOrAddSingleComponent<Animator>(gameObject);
            animator.enabled = true;
            animator.runtimeAnimatorController = null;
            animator.applyRootMotion = false;
            animator.updateMode = AnimatorUpdateMode.Normal;
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            return animator;
        }

        private static T GetOrAddSingleComponent<T>(GameObject gameObject) where T : Component
        {
            var components = gameObject.GetComponents<T>();
            if (components.Length > 1)
            {
                throw new InvalidOperationException(
                    gameObject.name + " has ambiguous duplicate " + typeof(T).Name + " components.");
            }

            return components.Length == 1 ? components[0] : gameObject.AddComponent<T>();
        }

        private static GameObject GetOrCreateDirectChild(Transform parent, string name, int siblingIndex)
        {
            GameObject match = null;
            for (var index = 0; index < parent.childCount; index++)
            {
                var child = parent.GetChild(index);
                if (child.name != name)
                {
                    continue;
                }

                if (match != null)
                {
                    throw new InvalidOperationException(
                        parent.name + " has ambiguous duplicate direct children named " + name + ".");
                }

                match = child.gameObject;
            }

            if (match == null)
            {
                match = CreateMissingGameObject(name, parent);
            }

            if (match.transform.GetSiblingIndex() != siblingIndex)
            {
                match.transform.SetSiblingIndex(siblingIndex);
            }

            return match;
        }

        private static GameObject CreateMissingGameObject(string name, Transform parent)
        {
            var gameObject = new GameObject(name);
            if (parent != null)
            {
                gameObject.transform.SetParent(parent, false);
            }

            return gameObject;
        }

        private static void ConfigureGameObject(
            GameObject gameObject,
            bool active,
            Vector3 localPosition,
            Vector3 localScale)
        {
            gameObject.transform.localPosition = localPosition;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = localScale;
            gameObject.SetActive(active);
        }

        private static void ValidateDirectChildren(Transform parent, params string[] allowedNames)
        {
            var allowed = new HashSet<string>(allowedNames, StringComparer.Ordinal);
            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (var index = 0; index < parent.childCount; index++)
            {
                var childName = parent.GetChild(index).name;
                if (!allowed.Contains(childName))
                {
                    throw new InvalidOperationException(
                        parent.name + " contains unexpected direct child " + childName + ".");
                }

                if (!seen.Add(childName))
                {
                    throw new InvalidOperationException(
                        parent.name + " contains duplicate direct child " + childName + ".");
                }
            }
        }

        private static void ValidateAllowedComponents(GameObject gameObject, params Type[] allowedTypes)
        {
            var allowed = new HashSet<Type>(allowedTypes);
            foreach (var component in gameObject.GetComponents<Component>())
            {
                if (component == null)
                {
                    throw new InvalidOperationException(gameObject.name + " contains a missing script component.");
                }

                if (!allowed.Contains(component.GetType()))
                {
                    throw new InvalidOperationException(
                        gameObject.name + " contains unexpected component " + component.GetType().Name + ".");
                }
            }
        }

        private static void CopyLocalTransform(Transform source, Transform destination)
        {
            destination.localPosition = source.localPosition;
            destination.localRotation = source.localRotation;
            destination.localScale = source.localScale;
        }

        private static Vector3[] Repeat(Vector3 value)
        {
            var values = new Vector3[BeatFrames.Length];
            for (var index = 0; index < values.Length; index++)
            {
                values[index] = value;
            }

            return values;
        }

        private static float FrameToSeconds(int frame)
        {
            return frame / (float)TimelineFps;
        }

        private static void ValidateHundredthGrid(
            float value,
            string clipName,
            string propertyName,
            int frame)
        {
            if (Mathf.Abs(value * 100f - Mathf.Round(value * 100f)) > 0.0001f)
            {
                throw new InvalidOperationException(
                    clipName + " " + propertyName + " at frame " + frame + " is not on the 0.01-unit grid.");
            }
        }

        private static void EnsureFolder(string path)
        {
            var segments = path.Split('/');
            var current = segments[0];
            for (var index = 1; index < segments.Length; index++)
            {
                var next = current + "/" + segments[index];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, segments[index]);
                }

                current = next;
            }
        }
    }
}
