using System;
using System.Globalization;
using System.IO;
using LastHost.Prototype.TechnicalSample2D;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace LastHost.Prototype.TechnicalSample2D.Editor
{
    public static class RatHost2DTechnicalSampleSceneBuilder
    {
        public const string ScenePath =
            "Assets/_Project/Scenes/RatHost2DTechnicalSample.unity";
        public const string ArtRoot =
            "Assets/_Project/Art/TechnicalSample2D";

        private const string TextureRoot = ArtRoot + "/Textures";
        private const string TileRoot = ArtRoot + "/Tiles";
        private const string InputAssetPath =
            "Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions";
        private static readonly Color32 Transparent = new Color32(0, 0, 0, 0);
        private static readonly Color32 FloorDark = new Color32(38, 50, 48, 255);
        private static readonly Color32 FloorLight = new Color32(51, 66, 61, 255);
        private static readonly Color32 WaterDark = new Color32(21, 70, 68, 255);
        private static readonly Color32 WaterLight = new Color32(36, 127, 111, 255);
        private static readonly Color32 WallDark = new Color32(48, 55, 52, 255);
        private static readonly Color32 WallLight = new Color32(91, 104, 96, 255);
        private static readonly Color32 Outline = new Color32(20, 24, 23, 255);
        private static readonly Color32 RatBody = new Color32(139, 126, 111, 255);
        private static readonly Color32 RatLight = new Color32(190, 171, 145, 255);

        [MenuItem("Last Host/Technical Sample 2D/Rebuild Scene")]
        public static void RebuildScene()
        {
            BuildAndSaveScene();
        }

        [MenuItem("Last Host/Technical Sample 2D/Build Windows Temporary")]
        public static void BuildWindowsTemporary()
        {
            BuildTemporaryWindowsPlayer();
        }

        internal static void BuildAndSaveScene()
        {
            EnsureFolders();
            GeneratePlaceholderTextures();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            ConfigureTextureImports();

            var floorTile = CreateOrUpdateTile(
                TileRoot + "/FloorTile.asset",
                TextureRoot + "/tile-floor.png",
                Tile.ColliderType.None);
            var waterTile = CreateOrUpdateTile(
                TileRoot + "/WaterTile.asset",
                TextureRoot + "/tile-water.png",
                Tile.ColliderType.Grid);
            var wallTile = CreateOrUpdateTile(
                TileRoot + "/WallTile.asset",
                TextureRoot + "/tile-wall.png",
                Tile.ColliderType.Grid);

            var scene = EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene,
                NewSceneMode.Single);
            var root = new GameObject("TechnicalSample2D");

            var environment = CreateChild(root.transform, "Environment");
            var grid = BuildTilemapRoom(environment, floorTile, waterTile, wallTile);

            var actors = CreateChild(root.transform, "Actors");
            var rat = BuildRatHost(actors, grid);

            var props = CreateChild(environment, "YSortProps");
            BuildYSortProp(props, "Pipe_A", TextureRoot + "/prop-pipe.png",
                new Vector2(-1.35f, 0.45f), 2,
                new Vector2(0.27f, 0.16f), new Vector2(0f, 0.02f));
            BuildYSortProp(props, "Barrel_A", TextureRoot + "/prop-barrel.png",
                new Vector2(1.1f, -0.65f), 4,
                new Vector2(0.31f, 0.14f), new Vector2(0f, 0.02f));

            var cameras = CreateChild(root.transform, "Cameras");
            var followCamera = BuildCamera(cameras, rat.transform);

            var telemetry = root.AddComponent<TechnicalSample2DTelemetry>();
            telemetry.Configure(
                rat.Controller,
                followCamera,
                rat.YSort);

            var ui = CreateChild(root.transform, "UI");
            BuildHud(ui, telemetry);

            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene, ScenePath))
            {
                throw new InvalidOperationException(
                    $"Failed to save technical sample scene: {ScenePath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeGameObject = root;
            Debug.Log(
                $"Rebuilt isolated 2D technical sample scene at '{ScenePath}'. " +
                "Generated art is technical placeholder only, not final art.");
        }

        internal static void BuildTemporaryWindowsPlayer()
        {
            BuildAndSaveScene();

            var runId = DateTime.Now.ToString(
                "yyyyMMdd-HHmmss",
                CultureInfo.InvariantCulture);
            var temporaryBuildPath =
                $"C:/tmp/LastHost2DTechnicalSample/{runId}/LastHost2DTechnicalSample.exe";
            var outputDirectory = Path.GetDirectoryName(temporaryBuildPath);
            if (string.IsNullOrEmpty(outputDirectory))
            {
                throw new InvalidOperationException("Temporary build directory is invalid.");
            }

            Directory.CreateDirectory(outputDirectory);
            var options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = temporaryBuildPath,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.Development
            };

            var report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"2D technical sample Windows build failed: {report.summary.result}");
            }

            Debug.Log(
                $"2D technical sample Windows build succeeded at '{temporaryBuildPath}'. " +
                $"Total size: {report.summary.totalSize} bytes.");
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

        private static RatBuildResult BuildRatHost(Transform parent, Grid grid)
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

            var inputAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputAssetPath);
            if (inputAsset == null ||
                inputAsset.FindAction(TechnicalSample2DConstants.MoveActionPath, false) == null)
            {
                throw new InvalidOperationException(
                    $"Read-only input action '{TechnicalSample2DConstants.MoveActionPath}' " +
                    $"is required at '{InputAssetPath}'.");
            }

            var input = ratObject.AddComponent<TechnicalSample2DInput>();
            input.Configure(inputAsset);
            var controller = ratObject.AddComponent<RatHost2DController>();
            controller.Configure(input, TechnicalSample2DConstants.TrialMoveSpeed);

            var visual = CreateChild(ratObject.transform, "Visual");
            var renderer = visual.gameObject.AddComponent<SpriteRenderer>();
            renderer.sortingOrder = 0;
            var frames = LoadRatFrames();
            renderer.sprite = frames[0];

            var view = visual.gameObject.AddComponent<RatHost2DView>();
            view.Configure(
                controller,
                renderer,
                frames,
                TechnicalSample2DConstants.WalkFramesPerSecond);

            var pixelSnap = visual.gameObject.AddComponent<VisualPixelSnap2D>();
            pixelSnap.Configure(ratObject.transform, TechnicalSample2DConstants.PixelsPerUnit);

            var footPoint = CreateChild(ratObject.transform, "FootPoint");
            footPoint.localPosition = new Vector3(0f, -0.12f, 0f);
            var ySort = visual.gameObject.AddComponent<YSortSprite2D>();
            ySort.Configure(
                footPoint,
                renderer,
                0,
                0,
                TechnicalSample2DConstants.YSortScale);

            return new RatBuildResult(controller, ySort);
        }

        private static void BuildYSortProp(
            Transform parent,
            string name,
            string texturePath,
            Vector2 position,
            int tieBreak,
            Vector2 colliderSize,
            Vector2 colliderOffset)
        {
            var prop = new GameObject(name);
            prop.transform.SetParent(parent, false);
            prop.transform.position = position;
            var renderer = prop.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadSprite(texturePath);
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

        private static PixelFollowCamera2D BuildCamera(Transform parent, Transform target)
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.transform.SetParent(parent, false);
            cameraObject.transform.position = new Vector3(
                target.position.x,
                target.position.y,
                -10f);
            cameraObject.tag = "MainCamera";

            var camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = TechnicalSample2DConstants.TrialOrthographicSize;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color32(12, 18, 18, 255);
            camera.transparencySortMode = TransparencySortMode.CustomAxis;
            camera.transparencySortAxis = Vector3.up;

            var follow = cameraObject.AddComponent<PixelFollowCamera2D>();
            follow.Configure(
                target,
                TechnicalSample2DConstants.PixelsPerUnit,
                TechnicalSample2DConstants.TrialOrthographicSize);
            follow.ApplyFollow();
            return follow;
        }

        private static void BuildHud(
            Transform parent,
            TechnicalSample2DTelemetry telemetry)
        {
            var canvasObject = new GameObject("Canvas");
            canvasObject.transform.SetParent(parent, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(
                TechnicalSample2DConstants.LogicalWidth,
                TechnicalSample2DConstants.LogicalHeight);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var title = CreateHudText(
                canvasObject.transform,
                "SampleTitle",
                "2D TECHNICAL SAMPLE",
                22,
                TextAnchor.UpperLeft,
                new Vector2(0f, 1f),
                new Vector2(16f, -14f),
                new Vector2(470f, 30f));
            var spec = CreateHudText(
                canvasObject.transform,
                "SpecText",
                "960×540 / Tile 64×32 / PPU 64",
                16,
                TextAnchor.UpperLeft,
                new Vector2(0f, 1f),
                new Vector2(16f, -46f),
                new Vector2(470f, 25f));
            var controls = CreateHudText(
                canvasObject.transform,
                "ControlsText",
                "WASD 이동",
                16,
                TextAnchor.UpperLeft,
                new Vector2(0f, 1f),
                new Vector2(16f, -73f),
                new Vector2(270f, 25f));
            var runtime = CreateHudText(
                canvasObject.transform,
                "RuntimeStatusText",
                "Direction South | Root pending | Camera error pending | Sort pending",
                14,
                TextAnchor.UpperRight,
                new Vector2(1f, 1f),
                new Vector2(-16f, -14f),
                new Vector2(520f, 58f));
            CreateHudText(
                canvasObject.transform,
                "PlaceholderNotice",
                "TECHNICAL PLACEHOLDER • NOT FINAL ART",
                13,
                TextAnchor.LowerLeft,
                new Vector2(0f, 0f),
                new Vector2(16f, 14f),
                new Vector2(430f, 24f));

            var hud = canvasObject.AddComponent<TechnicalSample2DHud>();
            hud.Configure(telemetry, title, spec, controls, runtime);
        }

        private static Text CreateHudText(
            Transform parent,
            string name,
            string value,
            int fontSize,
            TextAnchor alignment,
            Vector2 anchor,
            Vector2 anchoredPosition,
            Vector2 size)
        {
            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            var rect = textObject.AddComponent<RectTransform>();
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

        private static Font LoadBuiltinFont()
        {
            var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (font == null)
            {
                font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }

            if (font == null)
            {
                throw new InvalidOperationException("Unity built-in UI font is unavailable.");
            }

            return font;
        }

        private static Sprite[] LoadRatFrames()
        {
            var frames = new Sprite[16];
            var directionNames = new[] { "s", "sw", "w", "nw", "n", "ne", "e", "se" };
            for (var direction = 0; direction < directionNames.Length; direction++)
            {
                frames[(direction * 2)] = LoadSprite(
                    $"{TextureRoot}/rat-{direction:00}-{directionNames[direction]}-a.png");
                frames[(direction * 2) + 1] = LoadSprite(
                    $"{TextureRoot}/rat-{direction:00}-{directionNames[direction]}-b.png");
            }

            return frames;
        }

        private static Sprite LoadSprite(string path)
        {
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite == null)
            {
                throw new InvalidOperationException($"Sprite asset is unavailable: {path}");
            }

            return sprite;
        }

        private static Tile CreateOrUpdateTile(
            string tilePath,
            string spritePath,
            Tile.ColliderType colliderType)
        {
            var tile = AssetDatabase.LoadAssetAtPath<Tile>(tilePath);
            if (tile == null)
            {
                tile = ScriptableObject.CreateInstance<Tile>();
                AssetDatabase.CreateAsset(tile, tilePath);
            }

            tile.sprite = LoadSprite(spritePath);
            tile.color = Color.white;
            tile.colliderType = colliderType;
            EditorUtility.SetDirty(tile);
            return tile;
        }

        private static void GeneratePlaceholderTextures()
        {
            WriteTexture(
                TextureRoot + "/tile-floor.png",
                64,
                32,
                (x, y) => DiamondPixel(x, y, FloorDark, FloorLight, 7));
            WriteTexture(
                TextureRoot + "/tile-water.png",
                64,
                32,
                (x, y) => DiamondPixel(x, y, WaterDark, WaterLight, 5));
            WriteTexture(
                TextureRoot + "/tile-wall.png",
                64,
                32,
                (x, y) => DiamondPixel(x, y, WallDark, WallLight, 9));
            WriteTexture(
                TextureRoot + "/prop-pipe.png",
                32,
                48,
                PipePixel);
            WriteTexture(
                TextureRoot + "/prop-barrel.png",
                32,
                40,
                BarrelPixel);

            var directionVectors = new[]
            {
                new Vector2Int(0, -1),
                new Vector2Int(-1, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, 1),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(1, 0),
                new Vector2Int(1, -1)
            };
            var directionNames = new[] { "s", "sw", "w", "nw", "n", "ne", "e", "se" };
            for (var direction = 0; direction < directionVectors.Length; direction++)
            {
                for (var frame = 0; frame < 2; frame++)
                {
                    var capturedDirection = directionVectors[direction];
                    var capturedFrame = frame;
                    var suffix = frame == 0 ? "a" : "b";
                    WriteTexture(
                        $"{TextureRoot}/rat-{direction:00}-{directionNames[direction]}-{suffix}.png",
                        64,
                        64,
                        (x, y) => RatPixel(x, y, capturedDirection, capturedFrame));
                }
            }
        }

        private static void ConfigureTextureImports()
        {
            var guids = AssetDatabase.FindAssets("t:Texture2D", new[] { TextureRoot });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer == null)
                {
                    continue;
                }

                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = TechnicalSample2DConstants.PixelsPerUnit;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Point;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.npotScale = TextureImporterNPOTScale.None;

                var settings = new TextureImporterSettings();
                importer.ReadTextureSettings(settings);
                settings.spriteAlignment = (int)SpriteAlignment.Custom;
                settings.spritePivot = path.Contains("/rat-", StringComparison.Ordinal)
                    ? new Vector2(0.5f, 0.1875f)
                    : path.Contains("/prop-", StringComparison.Ordinal)
                        ? new Vector2(0.5f, 0.18f)
                        : new Vector2(0.5f, 0.5f);
                importer.SetTextureSettings(settings);
                importer.SaveAndReimport();
            }
        }

        private static Color32 DiamondPixel(
            int x,
            int y,
            Color32 dark,
            Color32 light,
            int stripePeriod)
        {
            var distance = Mathf.Abs(x - 31.5f) / 32f + Mathf.Abs(y - 15.5f) / 16f;
            if (distance > 1f)
            {
                return Transparent;
            }

            if (distance > 0.91f)
            {
                return Outline;
            }

            return ((x + (y * 2)) % stripePeriod) == 0 ? light : dark;
        }

        private static Color32 PipePixel(int x, int y)
        {
            if (y < 4 || y > 43 || x < 7 || x > 24)
            {
                return Transparent;
            }

            if (x == 7 || x == 24 || y == 4 || y == 43)
            {
                return Outline;
            }

            if (y > 31 && x > 10 && x < 22)
            {
                return new Color32(94, 124, 125, 255);
            }

            return ((x + y) % 6) == 0
                ? new Color32(111, 126, 117, 255)
                : new Color32(66, 79, 76, 255);
        }

        private static Color32 BarrelPixel(int x, int y)
        {
            var dx = Mathf.Abs(x - 15.5f);
            if (y < 3 || y > 35 || dx > 10f)
            {
                return Transparent;
            }

            if (dx > 8.5f || y == 3 || y == 35)
            {
                return Outline;
            }

            if (y == 10 || y == 28)
            {
                return new Color32(132, 116, 76, 255);
            }

            return new Color32(84, 72, 55, 255);
        }

        private static Color32 RatPixel(
            int x,
            int y,
            Vector2Int direction,
            int frame)
        {
            var bodyCenter = new Vector2(31.5f, 18.5f + frame);
            var bodyDx = (x - bodyCenter.x) / 9f;
            var bodyDy = (y - bodyCenter.y) / 5.5f;
            if ((bodyDx * bodyDx) + (bodyDy * bodyDy) <= 1f)
            {
                return ((x + y + frame) % 5) == 0 ? RatLight : RatBody;
            }

            var headCenter = bodyCenter + new Vector2(direction.x * 6f, direction.y * 3.5f);
            var headDx = x - headCenter.x;
            var headDy = y - headCenter.y;
            if ((headDx * headDx) + (headDy * headDy) <= 16f)
            {
                return RatLight;
            }

            var tailStart = bodyCenter - new Vector2(direction.x * 7f, direction.y * 3f);
            var tailEnd = tailStart - new Vector2(direction.x * 7f, direction.y * 3f);
            if (DistanceToSegment(new Vector2(x, y), tailStart, tailEnd) <= 0.8f)
            {
                return new Color32(169, 116, 111, 255);
            }

            var footOffset = frame == 0 ? 0 : 2;
            if (y == 12 + frame &&
                (Mathf.Abs(x - (27 + footOffset)) <= 1 ||
                 Mathf.Abs(x - (36 - footOffset)) <= 1))
            {
                return Outline;
            }

            return Transparent;
        }

        private static float DistanceToSegment(Vector2 point, Vector2 start, Vector2 end)
        {
            var segment = end - start;
            if (segment.sqrMagnitude <= Mathf.Epsilon)
            {
                return Vector2.Distance(point, start);
            }

            var t = Mathf.Clamp01(Vector2.Dot(point - start, segment) / segment.sqrMagnitude);
            return Vector2.Distance(point, start + (segment * t));
        }

        private static void WriteTexture(
            string assetPath,
            int width,
            int height,
            Func<int, int, Color32> pixelFactory)
        {
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            var pixels = new Color32[width * height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    pixels[(y * width) + x] = pixelFactory(x, y);
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply(false, false);
            var absolutePath = Path.GetFullPath(
                Path.Combine(Application.dataPath, "..", assetPath));
            File.WriteAllBytes(absolutePath, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);
        }

        private static Transform CreateChild(Transform parent, string name)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent, false);
            return child.transform;
        }

        private static void EnsureFolders()
        {
            Directory.CreateDirectory(ToAbsoluteAssetPath(ArtRoot));
            Directory.CreateDirectory(ToAbsoluteAssetPath(TextureRoot));
            Directory.CreateDirectory(ToAbsoluteAssetPath(TileRoot));
        }

        private static string ToAbsoluteAssetPath(string assetPath)
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, "..", assetPath));
        }

        private readonly struct RatBuildResult
        {
            public RatBuildResult(
                RatHost2DController controller,
                YSortSprite2D ySort)
            {
                Controller = controller;
                YSort = ySort;
            }

            public RatHost2DController Controller { get; }
            public YSortSprite2D YSort { get; }
            public Transform transform => Controller.transform;
        }
    }
}
