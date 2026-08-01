using System;
using System.IO;
using LastHost.Prototype.TechnicalSample2D;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace LastHost.Prototype.TechnicalSample2D.Editor
{
    public static class RatHost2DProductionSampleSceneBuilder
    {
        public const string ScenePath =
            "Assets/_Project/Scenes/RatHost2DTechnicalSample.unity";
        public const string ProductionRoot =
            "Assets/_Project/Art/Production2D/V1";
        public const int CandidatePixelsPerUnit = 128;
        public const float CandidateDisplayScale = 0.5f;

        private const string EnvironmentRoot = ProductionRoot + "/Environment";
        private const string RatRoot = ProductionRoot + "/Rat";
        private const string HudRoot = ProductionRoot + "/HUD";
        private const string GeneratedTileRoot = ProductionRoot + "/Generated/Tiles";
        private const string InputAssetPath =
            "Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions";
        private const float TrialOrthographicSize = 4.21875f;
        private const int OccluderTieBreak = 1;
        private const float LogicalPixel = 1f / CandidatePixelsPerUnit;

        [MenuItem("Last Host/Production 2D V1/Rebuild Technical Sample")]
        public static void RebuildTechnicalSample()
        {
            BuildAndSaveScene();
        }

        [MenuItem("Last Host/Production 2D V1/Configure Import Settings")]
        public static void ConfigureProductionImports()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            var textureGuids = AssetDatabase.FindAssets("t:Texture2D", new[] { ProductionRoot });
            if (textureGuids.Length != 18)
            {
                throw new InvalidOperationException(
                    $"Production2D V1 must contain exactly 18 PNG textures; found {textureGuids.Length}.");
            }

            foreach (var guid in textureGuids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ConfigureTextureImporter(assetPath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            Debug.Log(
                $"Configured {textureGuids.Length} Production2D V1 sprites: " +
                $"Point, mipmap off, RGBA, uncompressed, PPU {CandidatePixelsPerUnit} candidate.");
        }

        public static void BuildAndSaveScene()
        {
            ConfigureProductionImports();
            EnsureGeneratedFolders();

            var cleanFloorTile = CreateOrUpdateTile(
                GeneratedTileRoot + "/FloorCleanTile.asset",
                EnvironmentRoot + "/floor_clean_128x64.png",
                Tile.ColliderType.None);
            var wornFloorTile = CreateOrUpdateTile(
                GeneratedTileRoot + "/FloorWornTile.asset",
                EnvironmentRoot + "/floor_worn_128x64.png",
                Tile.ColliderType.None);
            var waterTile = CreateOrUpdateTile(
                GeneratedTileRoot + "/WaterCenterTile.asset",
                EnvironmentRoot + "/water_center_128x64.png",
                Tile.ColliderType.Grid);

            var scene = EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene,
                NewSceneMode.Single);
            var root = new GameObject("TechnicalSample2D");

            var environment = CreateChild(root.transform, "Environment");
            var grid = BuildRoom(environment, cleanFloorTile, wornFloorTile, waterTile);
            BuildWalls(environment, grid);
            BuildProps(environment, grid);

            var actors = CreateChild(root.transform, "Actors");
            var rat = BuildRatHost(actors, grid, environment);

            var cameras = CreateChild(root.transform, "Cameras");
            var followCamera = BuildCamera(cameras, rat.Root);

            var telemetry = root.AddComponent<TechnicalSample2DTelemetry>();
            telemetry.Configure(rat.Controller, followCamera, rat.YSort);

            var ui = CreateChild(root.transform, "UI");
            BuildHud(ui, telemetry, followCamera.TargetCamera);

            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene, ScenePath))
            {
                throw new InvalidOperationException(
                    $"Failed to save Production2D V1 technical sample: {ScenePath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            Selection.activeGameObject = root;
            Debug.Log(
                $"Rebuilt Production2D V1 technical sample at '{ScenePath}'. " +
                $"PPU {CandidatePixelsPerUnit} and display scale {CandidateDisplayScale:0.0} are candidates only.");
        }

        private static Grid BuildRoom(
            Transform parent,
            TileBase cleanFloorTile,
            TileBase wornFloorTile,
            TileBase waterTile)
        {
            var gridObject = new GameObject("Grid");
            gridObject.transform.SetParent(parent, false);
            var grid = gridObject.AddComponent<Grid>();
            grid.cellLayout = GridLayout.CellLayout.Isometric;
            grid.cellSize = new Vector3(1f, 0.5f, 1f);

            var floor = CreateTilemap(gridObject.transform, "FloorTilemap", -1000, false);
            for (var y = -8; y <= 8; y++)
            {
                for (var x = -11; x <= 11; x++)
                {
                    var tile = ((x * 3) + (y * 5)) % 7 == 0
                        ? wornFloorTile
                        : cleanFloorTile;
                    floor.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }

            var water = CreateTilemap(gridObject.transform, "BlockingWaterTilemap", -900, true);
            for (var y = -6; y <= 3; y++)
            {
                water.SetTile(new Vector3Int(6, y, 0), waterTile);
            }

            floor.RefreshAllTiles();
            water.RefreshAllTiles();

            var edgeObject = CreateSpriteObject(
                parent,
                "WaterEdgeVisual",
                EnvironmentRoot + "/water_edge_128x96.png",
                grid.GetCellCenterWorld(new Vector3Int(6, 4, 0)),
                -850);
            edgeObject.transform.localScale = Vector3.one;

            var boundary = new GameObject("RoomBoundary");
            boundary.transform.SetParent(parent, false);
            var edge = boundary.AddComponent<EdgeCollider2D>();
            edge.edgeRadius = 0.02f;
            edge.points = new[]
            {
                new Vector2(0f, 5.15f),
                new Vector2(10.15f, 0f),
                new Vector2(0f, -5.15f),
                new Vector2(-10.15f, 0f),
                new Vector2(0f, 5.15f)
            };

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

        private static void BuildWalls(Transform environment, Grid grid)
        {
            var walls = CreateChild(environment, "YSortWalls");
            BuildYSortObject(
                walls,
                "WallCorner_BackLeft",
                EnvironmentRoot + "/wall_corner_192x160.png",
                grid.GetCellCenterWorld(new Vector3Int(-7, 6, 0)),
                OccluderTieBreak,
                new Vector2(1.15f, 0.18f),
                new Vector2(0f, 0.08f));
            BuildYSortObject(
                walls,
                "WallStraight_Back",
                EnvironmentRoot + "/wall_straight_160x160.png",
                grid.GetCellCenterWorld(new Vector3Int(-4, 6, 0)),
                OccluderTieBreak,
                new Vector2(1.05f, 0.18f),
                new Vector2(0f, 0.08f));
            BuildYSortObject(
                walls,
                "WallStraight_Occlusion",
                EnvironmentRoot + "/wall_straight_160x160.png",
                grid.GetCellCenterWorld(new Vector3Int(1, 1, 0)),
                OccluderTieBreak,
                new Vector2(1.05f, 0.18f),
                new Vector2(0f, 0.08f));
            BuildYSortObject(
                walls,
                "WallStraight_BackRight",
                EnvironmentRoot + "/wall_straight_160x160.png",
                grid.GetCellCenterWorld(new Vector3Int(6, 5, 0)),
                OccluderTieBreak,
                new Vector2(1.05f, 0.18f),
                new Vector2(0f, 0.08f));
        }

        private static void BuildProps(Transform environment, Grid grid)
        {
            var props = CreateChild(environment, "YSortProps");
            BuildYSortObject(
                props,
                "Barrel_A",
                EnvironmentRoot + "/prop_barrel_96x112.png",
                grid.GetCellCenterWorld(new Vector3Int(-3, -1, 0)),
                OccluderTieBreak,
                new Vector2(0.60f, 0.22f),
                new Vector2(0f, 0.11f));
            BuildYSortObject(
                props,
                "Crate_A",
                EnvironmentRoot + "/prop_crate_112x112.png",
                grid.GetCellCenterWorld(new Vector3Int(2, -2, 0)),
                OccluderTieBreak,
                new Vector2(0.70f, 0.24f),
                new Vector2(0f, 0.12f));

            CreateSpriteObject(
                props,
                "Drain_A",
                EnvironmentRoot + "/prop_drain_128x80.png",
                grid.GetCellCenterWorld(new Vector3Int(0, -2, 0)),
                -800);
        }

        private static void BuildYSortObject(
            Transform parent,
            string name,
            string spritePath,
            Vector3 position,
            int tieBreak,
            Vector2 colliderSize,
            Vector2 colliderOffset)
        {
            var target = CreateSpriteObject(parent, name, spritePath, position, 0);
            var footprint = target.AddComponent<BoxCollider2D>();
            footprint.isTrigger = false;
            footprint.size = colliderSize;
            footprint.offset = colliderOffset;
            var sorter = target.AddComponent<YSortSprite2D>();
            sorter.Configure(
                target.transform,
                target.GetComponent<SpriteRenderer>(),
                0,
                tieBreak,
                TechnicalSample2DConstants.YSortScale);
            sorter.ApplySorting();
        }

        private static RatBuildResult BuildRatHost(
            Transform parent,
            Grid grid,
            Transform environment)
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
            collider.size = new Vector2(1.28f, 0.26f);
            collider.offset = new Vector2(0.30f, 0.13f);

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
            var frames = new[]
            {
                LoadSprite(RatRoot + "/rat_side_neutral_256x192.png"),
                LoadSprite(RatRoot + "/rat_side_contact_256x192.png"),
                LoadSprite(RatRoot + "/rat_side_passing_256x192.png")
            };
            renderer.sprite = frames[0];

            var sideView = visual.gameObject.AddComponent<RatSide3FrameView>();
            sideView.Configure(controller, renderer, frames, 7f);
            sideView.ConfigureBodyClearance(
                collider,
                new Vector2(1.28f, 0.26f),
                new Vector2(0.30f, 0.13f));

            var pixelSnap = visual.gameObject.AddComponent<VisualPixelSnap2D>();
            pixelSnap.Configure(ratObject.transform, CandidatePixelsPerUnit);

            var footPoint = CreateChild(ratObject.transform, "FootPoint");
            footPoint.localPosition = Vector3.zero;
            var ySort = visual.gameObject.AddComponent<YSortSprite2D>();
            ySort.Configure(
                footPoint,
                renderer,
                0,
                0,
                TechnicalSample2DConstants.YSortScale);
            ySort.ApplySorting();

            var occlusionResolver = visual.gameObject.AddComponent<VisualOcclusionResolver2D>();
            occlusionResolver.Configure(
                renderer,
                ySort,
                new[]
                {
                    new VisualOcclusionResolver2D.FrameAlphaContract(
                        frames[0],
                        Rect.MinMaxRect(-119f / 128f, 1f / 128f, 119f / 128f, 73f / 128f),
                        Rect.MinMaxRect(-40f / 128f, 1f / 128f, 115f / 128f, 73f / 128f)),
                    new VisualOcclusionResolver2D.FrameAlphaContract(
                        frames[1],
                        Rect.MinMaxRect(-119f / 128f, 1f / 128f, 119f / 128f, 75f / 128f),
                        Rect.MinMaxRect(-39f / 128f, 1f / 128f, 115f / 128f, 75f / 128f)),
                    new VisualOcclusionResolver2D.FrameAlphaContract(
                        frames[2],
                        Rect.MinMaxRect(-118f / 128f, 0f, 119f / 128f, 73f / 128f),
                        Rect.MinMaxRect(-42f / 128f, 0f, 114f / 128f, 73f / 128f))
                },
                new[]
                {
                    CreateOccluderContract(
                        environment,
                        "YSortWalls/WallStraight_Occlusion",
                        Rect.MinMaxRect(-69f / 128f, 4f / 128f, 68f / 128f, 154f / 128f)),
                    CreateOccluderContract(
                        environment,
                        "YSortProps/Barrel_A",
                        Rect.MinMaxRect(-35f / 128f, 2f / 128f, 34f / 128f, 108f / 128f)),
                    CreateOccluderContract(
                        environment,
                        "YSortProps/Crate_A",
                        Rect.MinMaxRect(-47f / 128f, 2f / 128f, 46f / 128f, 108f / 128f))
                },
                4f * LogicalPixel,
                2f * LogicalPixel);

            return new RatBuildResult(ratObject.transform, controller, ySort);
        }

        private static VisualOcclusionResolver2D.OccluderContract CreateOccluderContract(
            Transform environment,
            string relativePath,
            Rect visibleLocalBounds)
        {
            var target = environment.Find(relativePath);
            if (target == null)
            {
                throw new InvalidOperationException(
                    $"Production2D occluder is unavailable: {relativePath}");
            }

            return new VisualOcclusionResolver2D.OccluderContract(
                target.GetComponent<SpriteRenderer>(),
                target.GetComponent<YSortSprite2D>(),
                visibleLocalBounds);
        }

        private static PixelFollowCamera2D BuildCamera(Transform parent, Transform target)
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.transform.SetParent(parent, false);
            cameraObject.transform.position =
                new Vector3(target.position.x, target.position.y, -10f);
            cameraObject.tag = "MainCamera";

            var camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = TrialOrthographicSize;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color32(7, 10, 10, 255);
            camera.transparencySortMode = TransparencySortMode.CustomAxis;
            camera.transparencySortAxis = Vector3.up;

            var follow = cameraObject.AddComponent<PixelFollowCamera2D>();
            follow.Configure(target, CandidatePixelsPerUnit, TrialOrthographicSize);
            follow.ApplyFollow();
            return follow;
        }

        private static void BuildHud(
            Transform parent,
            TechnicalSample2DTelemetry telemetry,
            Camera uiCamera)
        {
            var canvasObject = new GameObject("Canvas");
            canvasObject.transform.SetParent(parent, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = uiCamera;
            canvas.planeDistance = 1f;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(960f, 540f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var portraitRoot = CreateRect(
                canvasObject.transform,
                "HostPortrait",
                new Vector2(0f, 1f),
                new Vector2(16f, -16f),
                new Vector2(128f, 128f),
                new Vector2(0f, 1f));
            CreateHudImage(
                portraitRoot,
                "HostPortraitSubject",
                HudRoot + "/hud_rat_portrait_184.png",
                new Vector2(0f, 1f),
                new Vector2(18f, -20f),
                new Vector2(92f, 92f),
                new Vector2(0f, 1f));
            CreateHudImage(
                portraitRoot,
                "HostPortraitFrame",
                HudRoot + "/hud_portrait_frame_256.png",
                new Vector2(0f, 1f),
                Vector2.zero,
                new Vector2(128f, 128f),
                new Vector2(0f, 1f));

            BuildHudBar(
                canvasObject.transform,
                "HostHealth",
                "숙주 생명력",
                HudRoot + "/hud_health_fill_400x52.png",
                new Vector2(152f, -28f),
                0.9f);
            BuildHudBar(
                canvasObject.transform,
                "HostImmune",
                "면역 경계도",
                HudRoot + "/hud_immune_fill_400x52.png",
                new Vector2(152f, -79f),
                0.55f);

            var title = CreateHudText(
                canvasObject.transform,
                "SampleTitle",
                "PRODUCTION2D V1 • ONE-ROOM SAMPLE",
                16,
                TextAnchor.UpperRight,
                new Vector2(1f, 1f),
                new Vector2(-16f, -16f),
                new Vector2(480f, 24f),
                new Vector2(1f, 1f));
            title.color = new Color32(224, 203, 139, 255);

            var status = CreateHudText(
                canvasObject.transform,
                "RuntimeStatusText",
                "WASD  |  SIDE 3F ONLY  |  PPU 128 CANDIDATE",
                13,
                TextAnchor.LowerRight,
                new Vector2(1f, 0f),
                new Vector2(-16f, 14f),
                new Vector2(620f, 42f),
                new Vector2(1f, 0f));

            var hud = canvasObject.AddComponent<Production2DSampleHud>();
            hud.Configure(telemetry, status);
        }

        private static void BuildHudBar(
            Transform parent,
            string name,
            string label,
            string fillPath,
            Vector2 anchoredPosition,
            float fillAmount)
        {
            var root = CreateRect(
                parent,
                name,
                new Vector2(0f, 1f),
                anchoredPosition,
                new Vector2(256f, 40f),
                new Vector2(0f, 1f));

            CreateHudImage(
                root,
                name + "Frame",
                HudRoot + "/hud_bar_frame_512x80.png",
                new Vector2(0f, 1f),
                Vector2.zero,
                new Vector2(256f, 40f),
                new Vector2(0f, 1f));
            var fill = CreateHudImage(
                root,
                name + "Fill",
                fillPath,
                new Vector2(0f, 1f),
                new Vector2(28f, -7f),
                new Vector2(200f, 26f),
                new Vector2(0f, 1f));
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Horizontal;
            fill.fillOrigin = (int)Image.OriginHorizontal.Left;
            fill.fillAmount = Mathf.Clamp01(fillAmount);
            var text = CreateHudText(
                root,
                name + "Label",
                label,
                12,
                TextAnchor.MiddleLeft,
                new Vector2(0f, 1f),
                new Vector2(30f, -20f),
                new Vector2(180f, 20f),
                new Vector2(0f, 0.5f));
            text.color = new Color32(239, 225, 185, 255);
        }

        private static Image CreateHudImage(
            Transform parent,
            string name,
            string spritePath,
            Vector2 anchor,
            Vector2 anchoredPosition,
            Vector2 size,
            Vector2 pivot)
        {
            var rect = CreateRect(parent, name, anchor, anchoredPosition, size, pivot);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = LoadSprite(spritePath);
            image.preserveAspect = true;
            image.raycastTarget = false;
            return image;
        }

        private static Text CreateHudText(
            Transform parent,
            string name,
            string value,
            int fontSize,
            TextAnchor alignment,
            Vector2 anchor,
            Vector2 anchoredPosition,
            Vector2 size,
            Vector2 pivot)
        {
            var rect = CreateRect(parent, name, anchor, anchoredPosition, size, pivot);
            var text = rect.gameObject.AddComponent<Text>();
            text.font = LoadBuiltinFont();
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = new Color32(226, 230, 211, 255);
            text.text = value;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;
            return text;
        }

        private static RectTransform CreateRect(
            Transform parent,
            string name,
            Vector2 anchor,
            Vector2 anchoredPosition,
            Vector2 size,
            Vector2 pivot)
        {
            var target = new GameObject(name);
            target.transform.SetParent(parent, false);
            var rect = target.AddComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = pivot;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
            return rect;
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

        private static GameObject CreateSpriteObject(
            Transform parent,
            string name,
            string spritePath,
            Vector3 position,
            int sortingOrder)
        {
            var target = new GameObject(name);
            target.transform.SetParent(parent, false);
            target.transform.position = position;
            var renderer = target.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadSprite(spritePath);
            renderer.sortingOrder = sortingOrder;
            return target;
        }

        private static Transform CreateChild(Transform parent, string name)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent, false);
            return child.transform;
        }

        private static Sprite LoadSprite(string path)
        {
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite == null)
            {
                throw new InvalidOperationException($"Production2D V1 sprite is unavailable: {path}");
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

        private static void ConfigureTextureImporter(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
            {
                throw new InvalidOperationException(
                    $"TextureImporter is unavailable for Production2D V1 asset: {assetPath}");
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = CandidatePixelsPerUnit;
            importer.filterMode = FilterMode.Point;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.wrapMode = TextureWrapMode.Clamp;

            var fileName = Path.GetFileNameWithoutExtension(assetPath);
            var pivot = ResolvePivot(fileName);
            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            settings.spriteAlignment = (int)SpriteAlignment.Custom;
            settings.spritePivot = pivot;
            importer.SetTextureSettings(settings);
            importer.SaveAndReimport();
        }

        private static Vector2 ResolvePivot(string fileName)
        {
            if (fileName.StartsWith("rat_side_", StringComparison.Ordinal))
            {
                return new Vector2(0.5f, 0.208333f);
            }

            if (fileName.StartsWith("wall_", StringComparison.Ordinal) ||
                fileName.StartsWith("prop_barrel", StringComparison.Ordinal) ||
                fileName.StartsWith("prop_crate", StringComparison.Ordinal))
            {
                return new Vector2(0.5f, 0f);
            }

            return new Vector2(0.5f, 0.5f);
        }

        private static void EnsureGeneratedFolders()
        {
            var absolutePath = Path.Combine(
                Application.dataPath,
                "_Project/Art/Production2D/V1/Generated/Tiles");
            Directory.CreateDirectory(absolutePath);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }

        private readonly struct RatBuildResult
        {
            public RatBuildResult(
                Transform root,
                RatHost2DController controller,
                YSortSprite2D ySort)
            {
                Root = root;
                Controller = controller;
                YSort = ySort;
            }

            public Transform Root { get; }
            public RatHost2DController Controller { get; }
            public YSortSprite2D YSort { get; }
        }
    }
}
