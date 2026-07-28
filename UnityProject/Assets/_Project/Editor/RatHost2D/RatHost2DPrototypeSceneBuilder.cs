using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using LastHost.Prototype.TechnicalSample2D;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace LastHost.Prototype.RatHost2D.Editor
{
    public static class RatHost2DPrototypeSceneBuilder
    {
        public const string ScenePath =
            "Assets/_Project/Scenes/RatHost2DPrototype.unity";

        private const string TechnicalArtRoot =
            "Assets/_Project/Art/TechnicalSample2D";
        private const string TextureRoot = TechnicalArtRoot + "/Textures";
        private const string TileRoot = TechnicalArtRoot + "/Tiles";
        private const string InputAssetPath =
            "Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions";
        private static readonly string[] BuildProtectedPaths =
        {
            "Assets/Settings/DefaultVolumeProfile.asset",
            "Assets/Settings/PC_RPAsset.asset",
            "Assets/Settings/UniversalRenderPipelineGlobalSettings.asset",
            "ProjectSettings/ProjectSettings.asset",
            "ProjectSettings/UnityConnectSettings.asset"
        };

        [MenuItem("Last Host/Rat Host 2D/Stage 2/Rebuild Scene")]
        public static void RebuildScene()
        {
            BuildAndSaveScene();
        }

        [MenuItem("Last Host/Rat Host 2D/Stage 2/Build Windows Temporary")]
        public static void BuildWindowsTemporary()
        {
            BuildTemporaryWindowsPlayer();
        }

        internal static void BuildAndSaveScene()
        {
            ValidateReadOnlyDependencies();

            var floorTile = LoadAsset<TileBase>(TileRoot + "/FloorTile.asset");
            var waterTile = LoadAsset<TileBase>(TileRoot + "/WaterTile.asset");
            var wallTile = LoadAsset<TileBase>(TileRoot + "/WallTile.asset");
            var inputAsset = LoadAsset<InputActionAsset>(InputAssetPath);
            if (inputAsset.FindAction(
                    TechnicalSample2DConstants.MoveActionPath,
                    false) == null)
            {
                throw new InvalidOperationException(
                    $"Read-only input action " +
                    $"'{TechnicalSample2DConstants.MoveActionPath}' is missing.");
            }

            var scene = EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene,
                NewSceneMode.Single);
            var root = new GameObject("RatHost2DPrototype");

            var core = CreateChild(root.transform, "Core2D");
            var session = core.gameObject.AddComponent<RatHost2DSessionController>();

            var hostMode = CreateChild(root.transform, "HostMode2D");
            var world = CreateChild(hostMode, "World2D");
            var grid = BuildTilemapRoom(world, floorTile, waterTile, wallTile);

            var props = CreateChild(world, "YSortProps");
            BuildYSortProp(
                props,
                "Pipe_A",
                TextureRoot + "/prop-pipe.png",
                new Vector2(-1.35f, 0.45f),
                2,
                new Vector2(0.27f, 0.16f),
                new Vector2(0f, 0.02f));
            BuildYSortProp(
                props,
                "Barrel_A",
                TextureRoot + "/prop-barrel.png",
                new Vector2(1.1f, -0.65f),
                4,
                new Vector2(0.31f, 0.14f),
                new Vector2(0f, 0.02f));

            var rat = BuildRatHost(hostMode, grid, session, inputAsset);
            var zone = BuildContaminationZone(world, session, rat.Movement);

            var cameraRoot = CreateChild(root.transform, "HostCamera2D");
            BuildCamera(
                cameraRoot,
                rat.Movement.FollowTarget,
                "Main Camera",
                new Color32(12, 18, 18, 255),
                TechnicalSample2DConstants.TrialOrthographicSize);

            var ui = CreateChild(root.transform, "UI2D");
            var hostHud = BuildHostHud(ui);
            var internalMode = BuildInternalMinigame(
                root.transform,
                session,
                inputAsset);
            var mutationShell = BuildMutationSelectionShell(root.transform);

            session.Configure(
                hostMode.gameObject,
                internalMode.Root,
                hostHud.Root,
                rat.Movement,
                new Collider2D[]
                {
                    rat.Collider,
                    zone.Collider
                });
            session.ConfigureStage2(
                cameraRoot.gameObject,
                internalMode.CameraRoot,
                internalMode.FailurePanelRoot,
                mutationShell,
                internalMode.Virus,
                internalMode.WhiteBloodCells,
                internalMode.Fragments,
                internalMode.Colliders);
            zone.Zone.Configure(
                session,
                rat.Movement,
                RatHost2DSessionController.ContaminationAlertPerSecond,
                RatHost2DSessionController.ContaminationHealthDamagePerSecond,
                RatHost2DSessionController.ContaminationFeedbackLabel);
            hostHud.Presenter.Configure(
                session,
                hostHud.HealthText,
                hostHud.AlertText,
                hostHud.ModeText,
                hostHud.FeedbackText);
            hostHud.Presenter.ConfigureSliders(
                hostHud.HealthSlider,
                hostHud.AlertSlider);
            internalMode.Hud.Presenter.Configure(
                session,
                internalMode.Hud.StabilityText,
                internalMode.Hud.FragmentsText,
                internalMode.Hud.ObjectiveText,
                internalMode.Hud.FeedbackText);
            internalMode.Hud.Presenter.ConfigureStabilitySlider(
                internalMode.Hud.StabilitySlider);

            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene, ScenePath))
            {
                throw new InvalidOperationException(
                    $"Failed to save 2D stage 2 scene: {ScenePath}");
            }

            AssetDatabase.ImportAsset(
                ScenePath,
                ImportAssetOptions.ForceSynchronousImport
                | ImportAssetOptions.ForceUpdate);
            if (scene.isDirty
                && !EditorSceneManager.SaveScene(scene, ScenePath))
            {
                throw new InvalidOperationException(
                    $"Failed to finalize 2D stage 2 scene: {ScenePath}");
            }

            if (scene.isDirty)
            {
                throw new InvalidOperationException(
                    $"2D stage 2 scene remained dirty after save: {ScenePath}");
            }

            Selection.activeGameObject = root;
            Debug.Log(
                $"Rebuilt isolated 2D stage 2 prototype scene at '{ScenePath}'. " +
                "TechnicalSample2D assets are referenced read-only and remain trial placeholders.");
        }

        internal static void BuildTemporaryWindowsPlayer()
        {
            var protectedSnapshots = CaptureBuildProtectedFiles();
            try
            {
                BuildTemporaryWindowsPlayerCore();
            }
            finally
            {
                RestoreBuildProtectedFiles(protectedSnapshots);
            }
        }

        private static void BuildTemporaryWindowsPlayerCore()
        {
            BuildAndSaveScene();

            var runId = DateTime.Now.ToString(
                "yyyyMMdd-HHmmss",
                CultureInfo.InvariantCulture);
            var outputPath =
                $"C:/tmp/LastHostRatHost2DStage2/{runId}/LastHostRatHost2DStage2.exe";
            var outputDirectory = Path.GetDirectoryName(outputPath);
            if (string.IsNullOrEmpty(outputDirectory))
            {
                throw new InvalidOperationException(
                    "Temporary Windows build directory is invalid.");
            }

            Directory.CreateDirectory(outputDirectory);
            var options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = outputPath,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.Development
            };

            var report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"2D stage 2 Windows build failed: {report.summary.result}");
            }

            Debug.Log(
                $"2D stage 2 Windows build succeeded at '{outputPath}'. " +
                $"Total size: {report.summary.totalSize} bytes.");
        }

        private static Dictionary<string, byte[]> CaptureBuildProtectedFiles()
        {
            var snapshots = new Dictionary<string, byte[]>(
                StringComparer.OrdinalIgnoreCase);
            foreach (var relativePath in BuildProtectedPaths)
            {
                var absolutePath = ToProjectAbsolutePath(relativePath);
                if (File.Exists(absolutePath))
                {
                    snapshots.Add(relativePath, File.ReadAllBytes(absolutePath));
                }
            }

            return snapshots;
        }

        private static void RestoreBuildProtectedFiles(
            IReadOnlyDictionary<string, byte[]> snapshots)
        {
            var restoredAsset = false;
            foreach (var snapshot in snapshots)
            {
                var absolutePath = ToProjectAbsolutePath(snapshot.Key);
                var currentBytes = File.Exists(absolutePath)
                    ? File.ReadAllBytes(absolutePath)
                    : Array.Empty<byte>();
                if (ByteArraysEqual(currentBytes, snapshot.Value))
                {
                    continue;
                }

                File.WriteAllBytes(absolutePath, snapshot.Value);
                if (snapshot.Key.StartsWith(
                        "Assets/",
                        StringComparison.OrdinalIgnoreCase))
                {
                    AssetDatabase.ImportAsset(
                        snapshot.Key,
                        ImportAssetOptions.ForceSynchronousImport
                        | ImportAssetOptions.ForceUpdate);
                    restoredAsset = true;
                }
            }

            if (restoredAsset)
            {
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }

        private static string ToProjectAbsolutePath(string relativePath)
        {
            var projectRoot = Directory.GetParent(Application.dataPath);
            if (projectRoot == null)
            {
                throw new InvalidOperationException(
                    "Unity project root could not be resolved.");
            }

            return Path.GetFullPath(
                Path.Combine(
                    projectRoot.FullName,
                    relativePath.Replace('/', Path.DirectorySeparatorChar)));
        }

        private static bool ByteArraysEqual(byte[] left, byte[] right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left == null || right == null || left.Length != right.Length)
            {
                return false;
            }

            for (var index = 0; index < left.Length; index++)
            {
                if (left[index] != right[index])
                {
                    return false;
                }
            }

            return true;
        }

        private static Grid BuildTilemapRoom(
            Transform parent,
            TileBase floorTile,
            TileBase waterTile,
            TileBase wallTile)
        {
            var gridObject = new GameObject("Grid");
            gridObject.transform.SetParent(parent, false);
            var grid = gridObject.AddComponent<Grid>();
            grid.cellLayout = GridLayout.CellLayout.Isometric;
            grid.cellSize = new Vector3(1f, 0.5f, 1f);

            var floor = CreateTilemap(gridObject.transform, "FloorTilemap", -1000, false);
            var water = CreateTilemap(gridObject.transform, "WaterTilemap", -900, true);
            var walls = CreateTilemap(gridObject.transform, "BlockingTilemap", -100, true);

            for (var y = -4; y <= 4; y++)
            {
                for (var x = -6; x <= 6; x++)
                {
                    floor.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
            }

            for (var x = -6; x <= 6; x++)
            {
                walls.SetTile(new Vector3Int(x, -4, 0), wallTile);
                walls.SetTile(new Vector3Int(x, 4, 0), wallTile);
            }

            for (var y = -3; y <= 3; y++)
            {
                walls.SetTile(new Vector3Int(-6, y, 0), wallTile);
                walls.SetTile(new Vector3Int(6, y, 0), wallTile);
            }

            for (var y = -2; y <= 2; y++)
            {
                water.SetTile(new Vector3Int(3, y, 0), waterTile);
            }

            floor.RefreshAllTiles();
            water.RefreshAllTiles();
            walls.RefreshAllTiles();
            return grid;
        }

        private static Tilemap CreateTilemap(
            Transform parent,
            string name,
            int sortingOrder,
            bool addCollider)
        {
            var tilemapObject = new GameObject(name);
            tilemapObject.transform.SetParent(parent, false);
            var tilemap = tilemapObject.AddComponent<Tilemap>();
            var renderer = tilemapObject.AddComponent<TilemapRenderer>();
            renderer.sortingOrder = sortingOrder;
            renderer.mode = TilemapRenderer.Mode.Individual;
            renderer.sortOrder = TilemapRenderer.SortOrder.TopRight;

            if (addCollider)
            {
                tilemapObject.AddComponent<TilemapCollider2D>();
            }

            return tilemap;
        }

        private static RatBuildResult BuildRatHost(
            Transform parent,
            Grid grid,
            RatHost2DSessionController session,
            InputActionAsset inputAsset)
        {
            var ratObject = new GameObject("RatHost2D");
            ratObject.transform.SetParent(parent, false);
            ratObject.transform.position =
                grid.GetCellCenterWorld(new Vector3Int(-2, 0, 0));

            var body = ratObject.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Dynamic;
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            var collider = ratObject.AddComponent<CapsuleCollider2D>();
            collider.direction = CapsuleDirection2D.Horizontal;
            collider.size = new Vector2(0.42f, 0.28f);
            collider.offset = new Vector2(0f, 0.02f);

            var input = ratObject.AddComponent<TechnicalSample2DInput>();
            input.Configure(inputAsset);
            var motor = ratObject.AddComponent<RatHost2DController>();
            motor.Configure(null, TechnicalSample2DConstants.TrialMoveSpeed);
            motor.enabled = false;

            var movement = ratObject.AddComponent<RatHost2DMovementController>();
            movement.Configure(
                session,
                input,
                TechnicalSample2DConstants.TrialMoveSpeed);
            movement.ConfigureInstinct(
                new Vector2(0.9f, 0.35f).normalized,
                new Vector2(-4.8f, 4.8f),
                new Vector2(-2.8f, 2.8f),
                1.5f,
                45f);

            var visual = CreateChild(ratObject.transform, "Visual");
            var renderer = visual.gameObject.AddComponent<SpriteRenderer>();
            renderer.sortingOrder = 0;
            var frames = LoadRatFrames();
            renderer.sprite = frames[0];

            var view = visual.gameObject.AddComponent<RatHost2DView>();
            view.Configure(
                motor,
                renderer,
                frames,
                TechnicalSample2DConstants.WalkFramesPerSecond);

            var pixelSnap = visual.gameObject.AddComponent<VisualPixelSnap2D>();
            pixelSnap.Configure(
                ratObject.transform,
                TechnicalSample2DConstants.PixelsPerUnit);

            var footPoint = CreateChild(ratObject.transform, "FootPoint");
            footPoint.localPosition = new Vector3(0f, -0.12f, 0f);
            var ySort = visual.gameObject.AddComponent<YSortSprite2D>();
            ySort.Configure(
                footPoint,
                renderer,
                0,
                0,
                TechnicalSample2DConstants.YSortScale);

            return new RatBuildResult(movement, collider);
        }

        private static ZoneBuildResult BuildContaminationZone(
            Transform parent,
            RatHost2DSessionController session,
            RatHost2DMovementController movement)
        {
            var zoneObject = new GameObject("ContaminationZone2D");
            zoneObject.transform.SetParent(parent, false);
            zoneObject.transform.position = new Vector3(0.35f, 1.1f, 0f);

            var renderer = zoneObject.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadAsset<Sprite>(TextureRoot + "/tile-water.png");
            renderer.color = new Color32(128, 235, 124, 190);
            renderer.sortingOrder = -850;
            zoneObject.transform.localScale = new Vector3(2.2f, 1.4f, 1f);

            var collider = zoneObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(0.8f, 0.34f);

            var zone = zoneObject.AddComponent<RatHost2DContaminationZone>();
            zone.Configure(
                session,
                movement,
                RatHost2DSessionController.ContaminationAlertPerSecond,
                RatHost2DSessionController.ContaminationHealthDamagePerSecond,
                RatHost2DSessionController.ContaminationFeedbackLabel);

            return new ZoneBuildResult(zone, collider);
        }

        private static void BuildYSortProp(
            Transform parent,
            string name,
            string spritePath,
            Vector2 position,
            int tieBreak,
            Vector2 colliderSize,
            Vector2 colliderOffset)
        {
            var prop = new GameObject(name);
            prop.transform.SetParent(parent, false);
            prop.transform.position = position;

            var renderer = prop.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadAsset<Sprite>(spritePath);

            var footprint = prop.AddComponent<BoxCollider2D>();
            footprint.isTrigger = false;
            footprint.size = colliderSize;
            footprint.offset = colliderOffset;

            var sorter = prop.AddComponent<YSortSprite2D>();
            sorter.Configure(
                prop.transform,
                renderer,
                0,
                tieBreak,
                TechnicalSample2DConstants.YSortScale);
            sorter.ApplySorting();
        }

        private static PixelFollowCamera2D BuildCamera(
            Transform parent,
            Transform target,
            string cameraName,
            Color backgroundColor,
            float orthographicSize)
        {
            var cameraObject = new GameObject(cameraName);
            cameraObject.transform.SetParent(parent, false);
            cameraObject.transform.position = new Vector3(
                target.position.x,
                target.position.y,
                -10f);
            cameraObject.tag = "MainCamera";

            var camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = orthographicSize;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = backgroundColor;
            camera.transparencySortMode = TransparencySortMode.CustomAxis;
            camera.transparencySortAxis = Vector3.up;

            var follow = cameraObject.AddComponent<PixelFollowCamera2D>();
            follow.Configure(
                target,
                TechnicalSample2DConstants.PixelsPerUnit,
                orthographicSize);
            follow.ApplyFollow();
            return follow;
        }

        private static HostHudBuildResult BuildHostHud(Transform parent)
        {
            var hudRoot = CreateCanvasRoot(parent, "HostHud2D", 10);

            var title = CreateText(
                hudRoot.transform,
                "StageTitle",
                "2D 쥐 숙주 · 2단계 내부 면역 미니게임",
                21,
                TextAnchor.UpperLeft,
                new Vector2(0f, 1f),
                new Vector2(16f, -14f),
                new Vector2(520f, 30f));
            title.color = new Color32(229, 235, 220, 255);

            var healthText = CreateText(
                hudRoot.transform,
                "HostHealthText",
                "숙주 생명력 100/100",
                16,
                TextAnchor.UpperLeft,
                new Vector2(0f, 1f),
                new Vector2(16f, -50f),
                new Vector2(300f, 24f));
            var healthSlider = CreateSlider(
                hudRoot.transform,
                "HostHealthSlider",
                new Vector2(0f, 1f),
                new Vector2(16f, -78f),
                new Vector2(250f, 12f),
                new Color32(114, 211, 126, 255));

            var alertText = CreateText(
                hudRoot.transform,
                "ImmuneAlertText",
                "면역 경계도 0/100",
                16,
                TextAnchor.UpperLeft,
                new Vector2(0f, 1f),
                new Vector2(16f, -98f),
                new Vector2(300f, 24f));
            var alertSlider = CreateSlider(
                hudRoot.transform,
                "ImmuneAlertSlider",
                new Vector2(0f, 1f),
                new Vector2(16f, -126f),
                new Vector2(250f, 12f),
                new Color32(229, 123, 94, 255));

            var modeText = CreateText(
                hudRoot.transform,
                "CurrentModeText",
                "현재 모드 쥐 숙주",
                16,
                TextAnchor.UpperRight,
                new Vector2(1f, 1f),
                new Vector2(-16f, -14f),
                new Vector2(320f, 26f));
            var feedbackText = CreateText(
                hudRoot.transform,
                "ImmuneCauseFeedbackText",
                string.Empty,
                18,
                TextAnchor.UpperRight,
                new Vector2(1f, 1f),
                new Vector2(-16f, -46f),
                new Vector2(380f, 28f));
            feedbackText.color = new Color32(246, 190, 117, 255);

            CreateText(
                hudRoot.transform,
                "ControlsText",
                "WASD 인계 · 무입력 숙주 본능 이동 · 초록 오염 구역",
                14,
                TextAnchor.LowerLeft,
                new Vector2(0f, 0f),
                new Vector2(16f, 36f),
                new Vector2(650f, 24f));
            var notice = CreateText(
                hudRoot.transform,
                "PlaceholderNotice",
                "TECHNICAL PLACEHOLDER · NOT FINAL ART/SPEC",
                12,
                TextAnchor.LowerLeft,
                new Vector2(0f, 0f),
                new Vector2(16f, 12f),
                new Vector2(480f, 20f));
            notice.color = new Color32(168, 183, 167, 255);

            var presenter = hudRoot.AddComponent<RatHost2DStage1Hud>();
            return new HostHudBuildResult(
                hudRoot,
                presenter,
                healthText,
                alertText,
                modeText,
                feedbackText,
                healthSlider,
                alertSlider);
        }

        private static InternalModeBuildResult BuildInternalMinigame(
            Transform parent,
            RatHost2DSessionController session,
            InputActionAsset inputAsset)
        {
            var root = new GameObject("InternalVirusMode2D");
            root.transform.SetParent(parent, false);

            var arena = CreateChild(root.transform, "Arena2D");
            BuildArenaBackdrop(arena);

            var colliders = new List<Collider2D>();
            var walls = CreateChild(arena, "ArenaWalls2D");
            colliders.Add(BuildArenaWall(
                walls,
                "Wall_North",
                new Vector2(0f, 2.35f),
                new Vector2(8.4f, 0.35f)));
            colliders.Add(BuildArenaWall(
                walls,
                "Wall_South",
                new Vector2(0f, -2.35f),
                new Vector2(8.4f, 0.35f)));
            colliders.Add(BuildArenaWall(
                walls,
                "Wall_West",
                new Vector2(-4.05f, 0f),
                new Vector2(0.35f, 5f)));
            colliders.Add(BuildArenaWall(
                walls,
                "Wall_East",
                new Vector2(4.05f, 0f),
                new Vector2(0.35f, 5f)));

            var virus = BuildVirus(
                arena,
                session,
                inputAsset,
                new Vector2(-2.7f, -0.25f));
            colliders.Add(virus.Collider);

            var whiteBloodCell = BuildWhiteBloodCell(
                arena,
                session,
                virus.Movement,
                new Vector2(2.55f, 0.75f));
            colliders.Add(whiteBloodCell.Collider);

            var fragmentRoot = CreateChild(arena, "MutationFragments2D");
            var fragmentPositions = new[]
            {
                new Vector2(-0.85f, 1.25f),
                new Vector2(0.75f, -1.2f),
                new Vector2(2.45f, 1.45f)
            };
            var fragments = new RatHost2DMutationFragment[fragmentPositions.Length];
            for (var index = 0; index < fragmentPositions.Length; index++)
            {
                var fragment = BuildMutationFragment(
                    fragmentRoot,
                    session,
                    index,
                    fragmentPositions[index]);
                fragments[index] = fragment.Fragment;
                colliders.Add(fragment.Collider);
            }

            var internalCameraRoot = CreateChild(root.transform, "InternalCamera2D");
            BuildCamera(
                internalCameraRoot,
                virus.Movement.FollowTarget,
                "Internal Camera",
                new Color32(18, 10, 17, 255),
                3.2f);

            var hud = BuildInternalHud(root.transform);
            var failurePanel = BuildFailurePanel(root.transform);
            root.SetActive(false);

            return new InternalModeBuildResult(
                root,
                internalCameraRoot.gameObject,
                failurePanel,
                virus.Movement,
                new[] { whiteBloodCell.Chaser },
                fragments,
                colliders.ToArray(),
                hud);
        }

        private static void BuildArenaBackdrop(Transform parent)
        {
            var backdrop = new GameObject("ArenaBackdrop");
            backdrop.transform.SetParent(parent, false);
            var renderer = backdrop.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadAsset<Sprite>(TextureRoot + "/tile-floor.png");
            renderer.color = new Color32(91, 55, 76, 255);
            renderer.sortingOrder = -1000;
            SetSpriteWorldSize(
                backdrop.transform,
                renderer.sprite,
                new Vector2(8.2f, 4.7f));
        }

        private static BoxCollider2D BuildArenaWall(
            Transform parent,
            string name,
            Vector2 position,
            Vector2 size)
        {
            var wall = new GameObject(name);
            wall.transform.SetParent(parent, false);
            wall.transform.localPosition = position;

            var visual = CreateChild(wall.transform, "Visual");
            var renderer = visual.gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadAsset<Sprite>(TextureRoot + "/tile-wall.png");
            renderer.color = new Color32(119, 77, 93, 255);
            renderer.sortingOrder = -100;
            SetSpriteWorldSize(visual, renderer.sprite, size);

            var collider = wall.AddComponent<BoxCollider2D>();
            collider.isTrigger = false;
            collider.size = size;
            return collider;
        }

        private static VirusBuildResult BuildVirus(
            Transform parent,
            RatHost2DSessionController session,
            InputActionAsset inputAsset,
            Vector2 position)
        {
            var virusObject = new GameObject("Virus2D");
            virusObject.transform.SetParent(parent, false);
            virusObject.transform.localPosition = position;

            var body = virusObject.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Dynamic;
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            var collider = virusObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = false;
            collider.radius = 0.24f;

            var input = virusObject.AddComponent<TechnicalSample2DInput>();
            input.Configure(inputAsset);
            var motor = virusObject.AddComponent<RatHost2DController>();
            motor.Configure(null, TechnicalSample2DConstants.TrialMoveSpeed);
            motor.enabled = false;

            var movement = virusObject.AddComponent<RatHost2DVirusMovementController>();
            movement.Configure(
                session,
                input,
                TechnicalSample2DConstants.TrialMoveSpeed);

            BuildPlaceholderVisual(
                virusObject.transform,
                "Visual",
                new Color32(86, 229, 204, 255),
                new Vector2(0.52f, 0.52f),
                8);
            return new VirusBuildResult(movement, collider);
        }

        private static WhiteBloodCellBuildResult BuildWhiteBloodCell(
            Transform parent,
            RatHost2DSessionController session,
            RatHost2DVirusMovementController virus,
            Vector2 position)
        {
            var whiteBloodCellObject = new GameObject("WhiteBloodCell2D");
            whiteBloodCellObject.transform.SetParent(parent, false);
            whiteBloodCellObject.transform.localPosition = position;

            var body = whiteBloodCellObject.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Dynamic;
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            var collider = whiteBloodCellObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 0.32f;

            var chaser =
                whiteBloodCellObject.AddComponent<RatHost2DWhiteBloodCellChaser>();
            chaser.Configure(session, virus, 1.8f, 0.5f);

            BuildPlaceholderVisual(
                whiteBloodCellObject.transform,
                "Visual",
                new Color32(240, 235, 214, 255),
                new Vector2(0.7f, 0.7f),
                10);
            return new WhiteBloodCellBuildResult(chaser, collider);
        }

        private static FragmentBuildResult BuildMutationFragment(
            Transform parent,
            RatHost2DSessionController session,
            int index,
            Vector2 position)
        {
            var fragmentObject = new GameObject($"Fragment_{index + 1:00}");
            fragmentObject.transform.SetParent(parent, false);
            fragmentObject.transform.localPosition = position;

            var collider = fragmentObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 0.2f;

            var fragment = fragmentObject.AddComponent<RatHost2DMutationFragment>();
            fragment.Configure(session, index);
            BuildPlaceholderVisual(
                fragmentObject.transform,
                "Visual",
                new Color32(255, 193, 79, 255),
                new Vector2(0.34f, 0.34f),
                6 + index);
            return new FragmentBuildResult(fragment, collider);
        }

        private static void BuildPlaceholderVisual(
            Transform parent,
            string name,
            Color color,
            Vector2 size,
            int tieBreak)
        {
            var visual = CreateChild(parent, name);
            var renderer = visual.gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadAsset<Sprite>(TextureRoot + "/tile-water.png");
            renderer.color = color;
            SetSpriteWorldSize(visual, renderer.sprite, size);

            var pixelSnap = visual.gameObject.AddComponent<VisualPixelSnap2D>();
            pixelSnap.Configure(parent, TechnicalSample2DConstants.PixelsPerUnit);
            var sorter = visual.gameObject.AddComponent<YSortSprite2D>();
            sorter.Configure(
                parent,
                renderer,
                0,
                tieBreak,
                TechnicalSample2DConstants.YSortScale);
            sorter.ApplySorting();
        }

        private static InternalHudBuildResult BuildInternalHud(Transform parent)
        {
            var hudRoot = CreateCanvasRoot(parent, "InternalHud2D", 20);
            var title = CreateText(
                hudRoot.transform,
                "InternalTitle",
                "내부 면역 반응 · WhiteBloodCellEvasion",
                21,
                TextAnchor.UpperLeft,
                new Vector2(0f, 1f),
                new Vector2(16f, -14f),
                new Vector2(560f, 30f));
            title.color = new Color32(220, 244, 234, 255);

            var stabilityText = CreateText(
                hudRoot.transform,
                "VirusStabilityText",
                "바이러스 안정도 100/100",
                16,
                TextAnchor.UpperLeft,
                new Vector2(0f, 1f),
                new Vector2(16f, -50f),
                new Vector2(320f, 24f));
            var stabilitySlider = CreateSlider(
                hudRoot.transform,
                "VirusStabilitySlider",
                new Vector2(0f, 1f),
                new Vector2(16f, -78f),
                new Vector2(250f, 12f),
                new Color32(86, 229, 204, 255));
            var fragmentsText = CreateText(
                hudRoot.transform,
                "MutationFragmentsText",
                "조각 0/3",
                18,
                TextAnchor.UpperRight,
                new Vector2(1f, 1f),
                new Vector2(-16f, -14f),
                new Vector2(240f, 28f));
            var objectiveText = CreateText(
                hudRoot.transform,
                "InternalObjectiveText",
                RatHost2DSessionController.InternalShellObjective,
                16,
                TextAnchor.UpperRight,
                new Vector2(1f, 1f),
                new Vector2(-16f, -47f),
                new Vector2(420f, 26f));
            var feedbackText = CreateText(
                hudRoot.transform,
                "ImmuneCaptureFeedbackText",
                string.Empty,
                18,
                TextAnchor.UpperRight,
                new Vector2(1f, 1f),
                new Vector2(-16f, -78f),
                new Vector2(340f, 28f));
            feedbackText.color = new Color32(255, 148, 133, 255);

            CreateText(
                hudRoot.transform,
                "InternalControlsText",
                "WASD 이동 · 조각 3개 수집 · 백혈구 접촉 시 면역 포착 +8",
                14,
                TextAnchor.LowerLeft,
                new Vector2(0f, 0f),
                new Vector2(16f, 36f),
                new Vector2(690f, 24f));
            var notice = CreateText(
                hudRoot.transform,
                "InternalPlaceholderNotice",
                "TECHNICAL PLACEHOLDER · NOT FINAL ART/SPEC",
                12,
                TextAnchor.LowerLeft,
                new Vector2(0f, 0f),
                new Vector2(16f, 12f),
                new Vector2(480f, 20f));
            notice.color = new Color32(168, 183, 167, 255);

            var presenter = hudRoot.AddComponent<RatHost2DStage2Hud>();
            return new InternalHudBuildResult(
                hudRoot,
                presenter,
                stabilityText,
                fragmentsText,
                objectiveText,
                feedbackText,
                stabilitySlider);
        }

        private static GameObject BuildFailurePanel(Transform parent)
        {
            var panelRoot = CreateCanvasRoot(parent, "FailurePanel2D", 80);
            var backdrop = CreateRectChild(panelRoot.transform, "FailureBackdrop");
            StretchToParent(backdrop.GetComponent<RectTransform>());
            var image = backdrop.AddComponent<Image>();
            image.color = new Color32(39, 8, 12, 230);
            image.raycastTarget = false;

            var title = CreateText(
                backdrop.transform,
                "FailureTitle",
                "면역 반응 돌파 실패",
                34,
                TextAnchor.MiddleCenter,
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, 52f),
                new Vector2(720f, 50f));
            title.color = new Color32(255, 171, 161, 255);
            CreateText(
                backdrop.transform,
                "FailureReturnNotice",
                "SPACE 확인 · 변이 보상 없이 쥐 숙주 / 면역 경계도 60% 복귀",
                19,
                TextAnchor.MiddleCenter,
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, -2f),
                new Vector2(780f, 40f));
            CreateText(
                backdrop.transform,
                "FailureLockNotice",
                "확인 전에는 Host·Virus 입력과 내부 충돌이 잠깁니다.",
                15,
                TextAnchor.MiddleCenter,
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, -42f),
                new Vector2(720f, 30f));
            panelRoot.SetActive(false);
            return panelRoot;
        }

        private static GameObject BuildMutationSelectionShell(Transform parent)
        {
            var shellRoot = CreateCanvasRoot(
                parent,
                "MutationSelectionShell2D",
                100);
            var backdrop = CreateRectChild(shellRoot.transform, "SuccessBackdrop");
            StretchToParent(backdrop.GetComponent<RectTransform>());
            var image = backdrop.AddComponent<Image>();
            image.color = new Color32(7, 25, 24, 240);
            image.raycastTarget = false;

            var title = CreateText(
                backdrop.transform,
                "SuccessTitle",
                "변이 조각 수집 완료",
                34,
                TextAnchor.MiddleCenter,
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, 55f),
                new Vector2(720f, 50f));
            title.color = new Color32(138, 244, 204, 255);
            CreateText(
                backdrop.transform,
                "MutationHandoff",
                "MutationSelection 인계 성공",
                22,
                TextAnchor.MiddleCenter,
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, 4f),
                new Vector2(720f, 38f));
            CreateText(
                backdrop.transform,
                "Stage3BoundaryNotice",
                "실제 변이 선택·효과 적용·성공 후 숙주 복귀는 3단계 범위",
                16,
                TextAnchor.MiddleCenter,
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, -42f),
                new Vector2(760f, 32f));
            shellRoot.SetActive(false);
            return shellRoot;
        }

        private static void SetSpriteWorldSize(
            Transform target,
            Sprite sprite,
            Vector2 worldSize)
        {
            if (sprite == null
                || sprite.bounds.size.x <= Mathf.Epsilon
                || sprite.bounds.size.y <= Mathf.Epsilon)
            {
                throw new InvalidOperationException(
                    "Placeholder sprite has invalid world bounds.");
            }

            target.localScale = new Vector3(
                worldSize.x / sprite.bounds.size.x,
                worldSize.y / sprite.bounds.size.y,
                1f);
        }

        private static GameObject CreateCanvasRoot(
            Transform parent,
            string name,
            int sortingOrder)
        {
            var canvasObject = new GameObject(
                name,
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(parent, false);

            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(
                TechnicalSample2DConstants.LogicalWidth,
                TechnicalSample2DConstants.LogicalHeight);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            return canvasObject;
        }

        private static Text CreateText(
            Transform parent,
            string name,
            string value,
            int fontSize,
            TextAnchor alignment,
            Vector2 anchor,
            Vector2 anchoredPosition,
            Vector2 size)
        {
            var textObject = CreateRectChild(parent, name);
            var rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = new Vector2(anchor.x, anchor.y);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;

            var text = textObject.AddComponent<Text>();
            text.font = LoadBuiltinFont();
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = new Color32(229, 235, 220, 255);
            text.text = value;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;
            return text;
        }

        private static Slider CreateSlider(
            Transform parent,
            string name,
            Vector2 anchor,
            Vector2 anchoredPosition,
            Vector2 size,
            Color fillColor)
        {
            var sliderObject = CreateRectChild(parent, name);
            var rect = sliderObject.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = new Vector2(anchor.x, anchor.y);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;

            var background = sliderObject.AddComponent<Image>();
            background.color = new Color32(38, 50, 48, 235);
            background.raycastTarget = false;

            var fillArea = CreateRectChild(sliderObject.transform, "Fill Area");
            StretchToParent(fillArea.GetComponent<RectTransform>());
            var fill = CreateRectChild(fillArea.transform, "Fill");
            StretchToParent(fill.GetComponent<RectTransform>());
            var fillImage = fill.AddComponent<Image>();
            fillImage.color = fillColor;
            fillImage.raycastTarget = false;

            var slider = sliderObject.AddComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 1f;
            slider.interactable = false;
            slider.targetGraphic = background;
            slider.fillRect = fill.GetComponent<RectTransform>();
            slider.direction = Slider.Direction.LeftToRight;
            return slider;
        }

        private static Sprite[] LoadRatFrames()
        {
            var frames = new Sprite[16];
            var directionNames =
                new[] { "s", "sw", "w", "nw", "n", "ne", "e", "se" };
            for (var direction = 0; direction < directionNames.Length; direction++)
            {
                frames[direction * 2] = LoadAsset<Sprite>(
                    $"{TextureRoot}/rat-{direction:00}-{directionNames[direction]}-a.png");
                frames[(direction * 2) + 1] = LoadAsset<Sprite>(
                    $"{TextureRoot}/rat-{direction:00}-{directionNames[direction]}-b.png");
            }

            return frames;
        }

        private static void ValidateReadOnlyDependencies()
        {
            LoadAsset<TileBase>(TileRoot + "/FloorTile.asset");
            LoadAsset<TileBase>(TileRoot + "/WaterTile.asset");
            LoadAsset<TileBase>(TileRoot + "/WallTile.asset");
            LoadAsset<Sprite>(TextureRoot + "/tile-floor.png");
            LoadAsset<Sprite>(TextureRoot + "/tile-water.png");
            LoadAsset<Sprite>(TextureRoot + "/tile-wall.png");
            LoadAsset<Sprite>(TextureRoot + "/prop-pipe.png");
            LoadAsset<Sprite>(TextureRoot + "/prop-barrel.png");
            LoadAsset<InputActionAsset>(InputAssetPath);
            LoadRatFrames();
        }

        private static T LoadAsset<T>(string path)
            where T : UnityEngine.Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                throw new InvalidOperationException(
                    $"Required read-only asset is unavailable: {path}");
            }

            return asset;
        }

        private static Font LoadBuiltinFont()
        {
            var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (font == null)
            {
                font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }

            if (font == null)
            {
                throw new InvalidOperationException(
                    "Unity built-in UI font is unavailable.");
            }

            return font;
        }

        private static Transform CreateChild(Transform parent, string name)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent, false);
            return child.transform;
        }

        private static GameObject CreateRectChild(Transform parent, string name)
        {
            var child = new GameObject(name, typeof(RectTransform));
            child.transform.SetParent(parent, false);
            return child;
        }

        private static void StretchToParent(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private readonly struct RatBuildResult
        {
            public RatBuildResult(
                RatHost2DMovementController movement,
                CapsuleCollider2D collider)
            {
                Movement = movement;
                Collider = collider;
            }

            public RatHost2DMovementController Movement { get; }
            public CapsuleCollider2D Collider { get; }
        }

        private readonly struct ZoneBuildResult
        {
            public ZoneBuildResult(
                RatHost2DContaminationZone zone,
                BoxCollider2D collider)
            {
                Zone = zone;
                Collider = collider;
            }

            public RatHost2DContaminationZone Zone { get; }
            public BoxCollider2D Collider { get; }
        }

        private readonly struct VirusBuildResult
        {
            public VirusBuildResult(
                RatHost2DVirusMovementController movement,
                CircleCollider2D collider)
            {
                Movement = movement;
                Collider = collider;
            }

            public RatHost2DVirusMovementController Movement { get; }
            public CircleCollider2D Collider { get; }
        }

        private readonly struct WhiteBloodCellBuildResult
        {
            public WhiteBloodCellBuildResult(
                RatHost2DWhiteBloodCellChaser chaser,
                CircleCollider2D collider)
            {
                Chaser = chaser;
                Collider = collider;
            }

            public RatHost2DWhiteBloodCellChaser Chaser { get; }
            public CircleCollider2D Collider { get; }
        }

        private readonly struct FragmentBuildResult
        {
            public FragmentBuildResult(
                RatHost2DMutationFragment fragment,
                CircleCollider2D collider)
            {
                Fragment = fragment;
                Collider = collider;
            }

            public RatHost2DMutationFragment Fragment { get; }
            public CircleCollider2D Collider { get; }
        }

        private readonly struct InternalModeBuildResult
        {
            public InternalModeBuildResult(
                GameObject root,
                GameObject cameraRoot,
                GameObject failurePanelRoot,
                RatHost2DVirusMovementController virus,
                RatHost2DWhiteBloodCellChaser[] whiteBloodCells,
                RatHost2DMutationFragment[] fragments,
                Collider2D[] colliders,
                InternalHudBuildResult hud)
            {
                Root = root;
                CameraRoot = cameraRoot;
                FailurePanelRoot = failurePanelRoot;
                Virus = virus;
                WhiteBloodCells = whiteBloodCells;
                Fragments = fragments;
                Colliders = colliders;
                Hud = hud;
            }

            public GameObject Root { get; }
            public GameObject CameraRoot { get; }
            public GameObject FailurePanelRoot { get; }
            public RatHost2DVirusMovementController Virus { get; }
            public RatHost2DWhiteBloodCellChaser[] WhiteBloodCells { get; }
            public RatHost2DMutationFragment[] Fragments { get; }
            public Collider2D[] Colliders { get; }
            public InternalHudBuildResult Hud { get; }
        }

        private readonly struct InternalHudBuildResult
        {
            public InternalHudBuildResult(
                GameObject root,
                RatHost2DStage2Hud presenter,
                Text stabilityText,
                Text fragmentsText,
                Text objectiveText,
                Text feedbackText,
                Slider stabilitySlider)
            {
                Root = root;
                Presenter = presenter;
                StabilityText = stabilityText;
                FragmentsText = fragmentsText;
                ObjectiveText = objectiveText;
                FeedbackText = feedbackText;
                StabilitySlider = stabilitySlider;
            }

            public GameObject Root { get; }
            public RatHost2DStage2Hud Presenter { get; }
            public Text StabilityText { get; }
            public Text FragmentsText { get; }
            public Text ObjectiveText { get; }
            public Text FeedbackText { get; }
            public Slider StabilitySlider { get; }
        }

        private readonly struct HostHudBuildResult
        {
            public HostHudBuildResult(
                GameObject root,
                RatHost2DStage1Hud presenter,
                Text healthText,
                Text alertText,
                Text modeText,
                Text feedbackText,
                Slider healthSlider,
                Slider alertSlider)
            {
                Root = root;
                Presenter = presenter;
                HealthText = healthText;
                AlertText = alertText;
                ModeText = modeText;
                FeedbackText = feedbackText;
                HealthSlider = healthSlider;
                AlertSlider = alertSlider;
            }

            public GameObject Root { get; }
            public RatHost2DStage1Hud Presenter { get; }
            public Text HealthText { get; }
            public Text AlertText { get; }
            public Text ModeText { get; }
            public Text FeedbackText { get; }
            public Slider HealthSlider { get; }
            public Slider AlertSlider { get; }
        }
    }
}
