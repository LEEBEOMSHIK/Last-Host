using System;
using System.IO;
using LastHost.Prototype.Cameras;
using LastHost.Prototype.Core;
using LastHost.Prototype.Host;
using LastHost.Prototype.Immune;
using LastHost.Prototype.Mutations;
using LastHost.Prototype.UI;
using LastHost.Prototype.VirusMinigame;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace LastHost.Prototype.Editor
{
    public static class RatHostPrototypeSceneBuilder
    {
        private const string ProjectRoot = "Assets/_Project";
        private const string ScenePath = ProjectRoot + "/Scenes/RatHostPrototype.unity";
        private const string InputActionPath = ProjectRoot + "/Settings/Input/RatHostPrototypeControls.inputactions";
        private const string RatSpriteRoot = ProjectRoot + "/Sprites/Characters/Rat/TrialV1";
        private const float RatSpritePixelsPerUnit = 32f;
        private const float RatSouthVisibleFootBottomOffset = 0.1875f;
        private const float RatSouthWestVisibleFootBottomOffset = 0.15625f;
        private const float RatWestVisibleFootBottomOffset = 0.09375f;
        private const float RatNorthWestVisibleFootBottomOffset = 0.28125f;
        private const float RatNorthVisibleFootBottomOffset = 0.375f;
        private const float RatNorthEastVisibleFootBottomOffset = 0.40625f;
        private const float RatEastVisibleFootBottomOffset = 0.15625f;
        private const float RatSouthEastVisibleFootBottomOffset = 0.15625f;
        private const float RatVisibleFootGroundClearance = 0.005f;
        private const float SewerFloorTopY = -0.02f;
        private const float ToxicWaterVisualSurfaceOffset = 0.002f;
        private const float ToxicWaterVisualThickness = 0.01f;
        private static readonly Vector2 RatSpritePivot = new Vector2(0.5f, 0.25f);
        private static readonly string[] RatSpriteFileNames =
        {
            "rat-00-s.png",
            "rat-01-sw.png",
            "rat-02-w.png",
            "rat-03-nw.png",
            "rat-04-n.png",
            "rat-05-ne.png",
            "rat-06-e.png",
            "rat-07-se.png"
        };

        [MenuItem("Last Host/Prototype/Rebuild Rat Host Prototype Scene")]
        public static void RebuildScene()
        {
            EnsureFolders();
            WriteInputActionsAsset();
            AssetDatabase.Refresh();
            ConfigureRatSpriteImports();

            var materials = CreateMaterials();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var root = new GameObject("RatHostPrototype");
            var coreRoot = CreateChild(root.transform, "Core");
            var ratRoot = CreateChild(root.transform, "RatHostMode");
            var virusRoot = CreateChild(root.transform, "VirusMinigameMode");
            var uiRoot = CreateChild(root.transform, "UI");
            var cameraRoot = CreateChild(root.transform, "Cameras");
            var lightingRoot = CreateChild(root.transform, "Lighting");

            var session = coreRoot.gameObject.AddComponent<PrototypeSessionController>();
            var rat = BuildRatHostMode(ratRoot, session, materials);
            var virus = BuildVirusMinigameMode(virusRoot, session, materials);
            var hud = BuildUi(uiRoot, session);
            BuildEventSystem(uiRoot);
            BuildCamera(cameraRoot, session, rat, virus);
            BuildLighting(lightingRoot);

            session.ratHostModeRoot = ratRoot.gameObject;
            session.virusMinigameModeRoot = virusRoot.gameObject;
            session.ratHostController = rat;
            session.virusMinigameController = virus;
            session.hud = hud;

            virusRoot.gameObject.SetActive(false);

            EditorSceneManager.SaveScene(scene, ScenePath);
            UpdateBuildSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("Last Host/Prototype/Build Windows Prototype")]
        public static void BuildWindowsPrototype()
        {
            RebuildScene();

            var outputPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "Builds", "RatHostPrototype", "LastHostPrototype.exe"));
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            var options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = outputPath,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new InvalidOperationException($"Rat host prototype build failed: {report.summary.result}");
            }
        }

        private static RatHostController BuildRatHostMode(Transform parent, PrototypeSessionController session, PrototypeMaterials materials)
        {
            CreatePrimitive(PrimitiveType.Cube, "SewerFloor", parent, new Vector3(0f, -0.12f, 0f), new Vector3(12f, 0.2f, 8f), materials.SewerFloor);
            CreatePrimitive(PrimitiveType.Cube, "NorthWall", parent, new Vector3(0f, 0.75f, 4.1f), new Vector3(12.5f, 1.5f, 0.3f), materials.SewerWall);
            CreatePrimitive(PrimitiveType.Cube, "SouthWall", parent, new Vector3(0f, 0.75f, -4.1f), new Vector3(12.5f, 1.5f, 0.3f), materials.SewerWall);
            CreatePrimitive(PrimitiveType.Cube, "WestWall", parent, new Vector3(-6.1f, 0.75f, 0f), new Vector3(0.3f, 1.5f, 8f), materials.SewerWall);
            CreatePrimitive(PrimitiveType.Cube, "EastWallLower", parent, new Vector3(6.1f, 0.75f, -2.6f), new Vector3(0.3f, 1.5f, 3f), materials.SewerWall);
            CreatePrimitive(PrimitiveType.Cube, "EastWallUpper", parent, new Vector3(6.1f, 0.75f, 2.6f), new Vector3(0.3f, 1.5f, 3f), materials.SewerWall);

            var toxicWater = CreatePrimitive(PrimitiveType.Cube, "ToxicWaterRiskZone", parent, new Vector3(-0.7f, 0.03f, 1.35f), new Vector3(2.6f, 0.08f, 1.5f), materials.ToxicWater);
            toxicWater.GetComponent<Renderer>().enabled = false;
            var toxicCollider = toxicWater.GetComponent<Collider>();
            toxicCollider.isTrigger = true;
            var toxicBody = toxicWater.AddComponent<Rigidbody>();
            toxicBody.isKinematic = true;
            toxicBody.useGravity = false;
            var riskZone = toxicWater.AddComponent<ImmuneRiskZone>();
            riskZone.session = session;
            riskZone.immuneAlertFeedbackLabel = "오염 노출";

            var toxicWaterVisualY = SewerFloorTopY + ToxicWaterVisualSurfaceOffset - ToxicWaterVisualThickness * 0.5f;
            CreateVisualPrimitive(
                PrimitiveType.Cube,
                "ToxicWaterVisual",
                parent,
                new Vector3(-0.7f, toxicWaterVisualY, 1.35f),
                new Vector3(2.6f, ToxicWaterVisualThickness, 1.5f),
                Quaternion.identity,
                materials.ToxicWater);

            BuildNoisyPipeInteractable(parent, session, materials);

            var gate = CreatePrimitive(PrimitiveType.Cube, "MammalAdaptationGate", parent, new Vector3(5.65f, 0.8f, 0f), new Vector3(0.6f, 1.6f, 1.7f), materials.GateBlocked);
            var gateComponent = gate.AddComponent<MammalAdaptationGate>();
            gateComponent.session = session;
            gateComponent.gateCollider = gate.GetComponent<Collider>();
            gateComponent.gateRenderer = gate.GetComponent<Renderer>();
            gateComponent.blockedMaterial = materials.GateBlocked;
            gateComponent.openMaterial = materials.GateOpen;

            var rat = new GameObject("RatHost");
            rat.transform.SetParent(parent, false);
            rat.transform.localPosition = new Vector3(-4.4f, 0f, -1f);

            var controller = rat.AddComponent<CharacterController>();
            controller.height = 0.7f;
            controller.radius = 0.25f;
            controller.center = new Vector3(0f, 0.35f, 0f);

            var ratController = rat.AddComponent<RatHostController>();
            ratController.session = session;

            BuildRatDirectionalSpriteVisual(rat.transform, ratController);

            return ratController;
        }

        private static void BuildRatDirectionalSpriteVisual(Transform parent, RatHostController ratController)
        {
            var sprites = new Sprite[RatSpriteFileNames.Length];
            for (var i = 0; i < RatSpriteFileNames.Length; i++)
            {
                var path = $"{RatSpriteRoot}/{RatSpriteFileNames[i]}";
                sprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                if (sprites[i] == null)
                {
                    throw new InvalidOperationException($"Rat trial sprite is missing or not imported as Sprite: {path}");
                }
            }

            var visual = new GameObject("RatVisual");
            visual.transform.SetParent(parent, false);

            var spriteRenderer = visual.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprites[0];
            spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;
            spriteRenderer.sortingOrder = 5;
            spriteRenderer.receiveShadows = false;

            var directionalView = visual.AddComponent<RatDirectionalSpriteView>();
            directionalView.ratHostController = ratController;
            directionalView.spriteRenderer = spriteRenderer;
            directionalView.south = sprites[0];
            directionalView.southWest = sprites[1];
            directionalView.west = sprites[2];
            directionalView.northWest = sprites[3];
            directionalView.north = sprites[4];
            directionalView.northEast = sprites[5];
            directionalView.east = sprites[6];
            directionalView.southEast = sprites[7];
            directionalView.groundProbeHeight = 1f;
            directionalView.groundProbeDistance = 1.6f;
            directionalView.maxSurfaceRise = 0.2f;
            directionalView.maxSurfaceDrop = 0.5f;
            directionalView.minSurfaceNormalY = 0.5f;
            directionalView.southVisibleFootBottomOffset = RatSouthVisibleFootBottomOffset;
            directionalView.southWestVisibleFootBottomOffset = RatSouthWestVisibleFootBottomOffset;
            directionalView.westVisibleFootBottomOffset = RatWestVisibleFootBottomOffset;
            directionalView.northWestVisibleFootBottomOffset = RatNorthWestVisibleFootBottomOffset;
            directionalView.northVisibleFootBottomOffset = RatNorthVisibleFootBottomOffset;
            directionalView.northEastVisibleFootBottomOffset = RatNorthEastVisibleFootBottomOffset;
            directionalView.eastVisibleFootBottomOffset = RatEastVisibleFootBottomOffset;
            directionalView.southEastVisibleFootBottomOffset = RatSouthEastVisibleFootBottomOffset;
            directionalView.groundClearance = RatVisibleFootGroundClearance;
            spriteRenderer.sprite = sprites[0];
        }

        private static void BuildNoisyPipeInteractable(Transform parent, PrototypeSessionController session, PrototypeMaterials materials)
        {
            var noisyPipe = new GameObject("NoisyPipeRiskInteractable");
            noisyPipe.transform.SetParent(parent, false);
            noisyPipe.transform.localPosition = new Vector3(2.2f, 0f, -2.2f);

            var noisyPipeCollider = noisyPipe.AddComponent<BoxCollider>();
            noisyPipeCollider.isTrigger = true;
            noisyPipeCollider.center = new Vector3(0f, 0.55f, 0f);
            noisyPipeCollider.size = new Vector3(1.9f, 1.25f, 1.9f);

            var interactable = noisyPipe.AddComponent<RatRiskInteractable>();
            interactable.session = session;

            CreateVisualPrimitive(
                PrimitiveType.Cylinder,
                "PipeBody",
                noisyPipe.transform,
                new Vector3(0f, 0.42f, 0f),
                new Vector3(0.28f, 0.78f, 0.28f),
                Quaternion.Euler(0f, 0f, 90f),
                materials.NoisyPipeBody);
            CreateVisualPrimitive(
                PrimitiveType.Cylinder,
                "PipeEndCapLeft",
                noisyPipe.transform,
                new Vector3(-0.86f, 0.42f, 0f),
                new Vector3(0.34f, 0.07f, 0.34f),
                Quaternion.Euler(0f, 0f, 90f),
                materials.NoisyPipeBody);
            CreateVisualPrimitive(
                PrimitiveType.Cylinder,
                "PipeEndCapRight",
                noisyPipe.transform,
                new Vector3(0.86f, 0.42f, 0f),
                new Vector3(0.34f, 0.07f, 0.34f),
                Quaternion.Euler(0f, 0f, 90f),
                materials.NoisyPipeBody);
            CreateVisualPrimitive(
                PrimitiveType.Cube,
                "ValveStem",
                noisyPipe.transform,
                new Vector3(0f, 0.72f, 0f),
                new Vector3(0.14f, 0.36f, 0.14f),
                Quaternion.identity,
                materials.NoisyPipeValve);
            CreateVisualPrimitive(
                PrimitiveType.Cylinder,
                "ValveWheel",
                noisyPipe.transform,
                new Vector3(0f, 1.02f, 0f),
                new Vector3(0.42f, 0.05f, 0.42f),
                Quaternion.identity,
                materials.NoisyPipeValve);
            CreateVisualPrimitive(
                PrimitiveType.Cube,
                "ValveHandleCrossbarX",
                noisyPipe.transform,
                new Vector3(0f, 1.08f, 0f),
                new Vector3(0.82f, 0.05f, 0.08f),
                Quaternion.identity,
                materials.NoisyPipeValve);
            CreateVisualPrimitive(
                PrimitiveType.Cube,
                "ValveHandleCrossbarZ",
                noisyPipe.transform,
                new Vector3(0f, 1.09f, 0f),
                new Vector3(0.08f, 0.05f, 0.82f),
                Quaternion.identity,
                materials.NoisyPipeValve);
            CreateVisualPrimitive(
                PrimitiveType.Sphere,
                "NoiseCueDot",
                noisyPipe.transform,
                new Vector3(0f, 1.44f, 0f),
                new Vector3(0.16f, 0.16f, 0.16f),
                Quaternion.identity,
                materials.NoisyPipeMarker);

            BuildInteractionMarkerRing(noisyPipe.transform, materials.NoisyPipeMarker);
        }

        private static void BuildInteractionMarkerRing(Transform parent, Material material)
        {
            var markerRoot = CreateChild(parent, "InteractionMarkerRing");
            markerRoot.localPosition = new Vector3(0f, 0.03f, 0f);

            CreateVisualPrimitive(PrimitiveType.Cube, "NorthMarker", markerRoot, new Vector3(0f, 0f, 0.86f), new Vector3(1.5f, 0.04f, 0.08f), Quaternion.identity, material);
            CreateVisualPrimitive(PrimitiveType.Cube, "SouthMarker", markerRoot, new Vector3(0f, 0f, -0.86f), new Vector3(1.5f, 0.04f, 0.08f), Quaternion.identity, material);
            CreateVisualPrimitive(PrimitiveType.Cube, "EastMarker", markerRoot, new Vector3(0.86f, 0f, 0f), new Vector3(0.08f, 0.04f, 1.5f), Quaternion.identity, material);
            CreateVisualPrimitive(PrimitiveType.Cube, "WestMarker", markerRoot, new Vector3(-0.86f, 0f, 0f), new Vector3(0.08f, 0.04f, 1.5f), Quaternion.identity, material);
        }

        private static VirusMinigameController BuildVirusMinigameMode(Transform parent, PrototypeSessionController session, PrototypeMaterials materials)
        {
            CreatePrimitive(PrimitiveType.Cube, "InternalArenaFloor", parent, new Vector3(0f, -0.1f, 0f), new Vector3(10.5f, 0.2f, 6.5f), materials.ArenaFloor);
            CreatePrimitive(PrimitiveType.Cube, "ArenaTopBoundary", parent, new Vector3(0f, 0.45f, 3.25f), new Vector3(10.8f, 0.9f, 0.18f), materials.ArenaWall);
            CreatePrimitive(PrimitiveType.Cube, "ArenaBottomBoundary", parent, new Vector3(0f, 0.45f, -3.25f), new Vector3(10.8f, 0.9f, 0.18f), materials.ArenaWall);
            CreatePrimitive(PrimitiveType.Cube, "ArenaLeftBoundary", parent, new Vector3(-5.25f, 0.45f, 0f), new Vector3(0.18f, 0.9f, 6.5f), materials.ArenaWall);
            CreatePrimitive(PrimitiveType.Cube, "ArenaRightBoundary", parent, new Vector3(5.25f, 0.45f, 0f), new Vector3(0.18f, 0.9f, 6.5f), materials.ArenaWall);

            var controllerObject = CreateChild(parent, "VirusMinigameController").gameObject;
            var controller = controllerObject.AddComponent<VirusMinigameController>();
            controller.session = session;

            var virusPlayer = CreatePrimitive(PrimitiveType.Sphere, "VirusPlayer", parent, new Vector3(-3.8f, 0.45f, 0f), new Vector3(0.55f, 0.55f, 0.55f), materials.VirusPlayer);
            var virusCollider = virusPlayer.GetComponent<SphereCollider>();
            virusCollider.isTrigger = false;
            var virusBody = virusPlayer.AddComponent<Rigidbody>();
            virusBody.isKinematic = true;
            virusBody.useGravity = false;
            var playerController = virusPlayer.AddComponent<VirusPlayerController>();

            var whiteBloodCell = CreatePrimitive(PrimitiveType.Sphere, "WhiteBloodCell", parent, new Vector3(3.2f, 0.45f, 0f), new Vector3(0.72f, 0.72f, 0.72f), materials.WhiteBloodCell);
            var whiteCollider = whiteBloodCell.GetComponent<SphereCollider>();
            whiteCollider.isTrigger = true;
            var whiteController = whiteBloodCell.AddComponent<WhiteBloodCellChaser>();
            whiteController.controller = controller;
            whiteController.target = virusPlayer.transform;

            var fragmentA = CreateFragment(parent, "MutationFragment_A", new Vector3(-1.4f, 0.35f, 2.1f), materials.MutationFragment, controller);
            var fragmentB = CreateFragment(parent, "MutationFragment_B", new Vector3(1.2f, 0.35f, -1.7f), materials.MutationFragment, controller);
            var fragmentC = CreateFragment(parent, "MutationFragment_C", new Vector3(3.8f, 0.35f, 1.6f), materials.MutationFragment, controller);

            controller.virusPlayer = playerController;
            controller.whiteBloodCells = new[] { whiteController };
            controller.mutationFragments = new[] { fragmentA, fragmentB, fragmentC };

            return controller;
        }

        private static MutationFragmentPickup CreateFragment(Transform parent, string name, Vector3 position, Material material, VirusMinigameController controller)
        {
            var fragment = CreatePrimitive(PrimitiveType.Sphere, name, parent, position, new Vector3(0.35f, 0.35f, 0.35f), material);
            fragment.GetComponent<SphereCollider>().isTrigger = true;
            var pickup = fragment.AddComponent<MutationFragmentPickup>();
            pickup.controller = controller;
            return pickup;
        }

        private static PrototypeHud BuildUi(Transform parent, PrototypeSessionController session)
        {
            var canvasObject = new GameObject("Canvas");
            canvasObject.transform.SetParent(parent, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.AddComponent<GraphicRaycaster>();

            var hudObject = new GameObject("PrototypeHud");
            hudObject.transform.SetParent(canvasObject.transform, false);
            var hud = hudObject.AddComponent<PrototypeHud>();

            var topBand = CreatePanel(canvasObject.transform, "TopHudBand", new Color(0.04f, 0.05f, 0.05f, 0.82f));
            SetStretchTop(topBand.GetComponent<RectTransform>(), 112f);

            hud.modeText = CreateText(topBand.transform, "ModeText", "쥐 숙주", 24, TextAnchor.MiddleLeft, new Vector2(22f, -18f), new Vector2(220f, 32f));
            hud.objectiveText = CreateText(topBand.transform, "ObjectiveText", "하수도 탐색 중", 20, TextAnchor.MiddleLeft, new Vector2(22f, -56f), new Vector2(360f, 30f));
            hud.hostHealthText = CreateText(topBand.transform, "HostHealthText", "숙주 생명력", 18, TextAnchor.MiddleLeft, new Vector2(430f, -18f), new Vector2(240f, 28f));
            hud.immuneAlertText = CreateText(topBand.transform, "ImmuneAlertText", "면역 경계도", 18, TextAnchor.MiddleLeft, new Vector2(430f, -56f), new Vector2(240f, 28f));
            hud.virusStabilityText = CreateText(topBand.transform, "VirusStabilityText", "바이러스 안정도", 18, TextAnchor.MiddleLeft, new Vector2(810f, -18f), new Vector2(250f, 28f));
            hud.mutationFragmentsText = CreateText(topBand.transform, "MutationFragmentsText", "변이 조각", 18, TextAnchor.MiddleLeft, new Vector2(810f, -56f), new Vector2(220f, 28f));
            hud.mutationStatusText = CreateText(topBand.transform, "MutationStatusText", "획득 변이 없음", 18, TextAnchor.MiddleRight, new Vector2(-24f, -36f), new Vector2(420f, 32f), anchorRight: true);

            hud.hostHealthSlider = CreateSlider(topBand.transform, "HostHealthSlider", new Vector2(610f, -19f), new Vector2(150f, 16f), new Color(0.68f, 0.95f, 0.48f, 1f));
            hud.immuneAlertSlider = CreateSlider(topBand.transform, "ImmuneAlertSlider", new Vector2(610f, -57f), new Vector2(150f, 16f), new Color(0.95f, 0.28f, 0.22f, 1f));
            hud.virusStabilitySlider = CreateSlider(topBand.transform, "VirusStabilitySlider", new Vector2(1015f, -19f), new Vector2(150f, 16f), new Color(0.38f, 0.72f, 0.95f, 1f));

            hud.mutationPanel = BuildMutationPanel(canvasObject.transform, session);
            hud.failurePanel = BuildFailurePanel(canvasObject.transform, hud);

            return hud;
        }

        private static void BuildEventSystem(Transform parent)
        {
            var eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.transform.SetParent(parent, false);
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<InputSystemUIInputModule>();
        }

        private static GameObject BuildMutationPanel(Transform parent, PrototypeSessionController session)
        {
            var panel = CreatePanel(parent, "MutationSelectionPanel", new Color(0.05f, 0.06f, 0.06f, 0.9f));
            var rect = panel.GetComponent<RectTransform>();
            SetCentered(rect, new Vector2(760f, 310f));

            CreateText(panel.transform, "MutationTitle", "변이 선택", 30, TextAnchor.MiddleCenter, new Vector2(0f, -34f), new Vector2(660f, 42f), centered: true);
            CreateMutationButton(panel.transform, session, MutationType.Dormancy, new Vector2(-245f, -150f));
            CreateMutationButton(panel.transform, session, MutationType.NeuralControl, new Vector2(0f, -150f));
            CreateMutationButton(panel.transform, session, MutationType.MammalAdaptation, new Vector2(245f, -150f));

            panel.SetActive(false);
            return panel;
        }

        private static GameObject BuildFailurePanel(Transform parent, PrototypeHud hud)
        {
            var panel = CreatePanel(parent, "VirusFailurePanel", new Color(0.07f, 0.04f, 0.04f, 0.88f));
            var rect = panel.GetComponent<RectTransform>();
            SetCentered(rect, new Vector2(420f, 210f));

            CreateText(panel.transform, "FailureTitle", "바이러스 안정도 소진", 26, TextAnchor.MiddleCenter, new Vector2(0f, -45f), new Vector2(360f, 42f), centered: true);
            var retryButton = CreateButton(panel.transform, "RetryButton", "재시도", new Vector2(0f, -135f), new Vector2(170f, 52f));
            UnityEventTools.AddPersistentListener(retryButton.onClick, hud.RetryVirusMinigame);

            panel.SetActive(false);
            return panel;
        }

        private static void CreateMutationButton(Transform parent, PrototypeSessionController session, MutationType mutationType, Vector2 position)
        {
            var button = CreateButton(parent, MutationDefinition.GetName(mutationType) + "Button", string.Empty, position, new Vector2(220f, 150f));
            var text = button.GetComponentInChildren<Text>();
            text.fontSize = 18;
            text.alignment = TextAnchor.MiddleCenter;

            var option = button.gameObject.AddComponent<MutationOptionButton>();
            option.session = session;
            option.mutationType = mutationType;
            option.label = text;
            option.RefreshLabel();
        }

        private static void BuildCamera(Transform parent, PrototypeSessionController session, RatHostController rat, VirusMinigameController virus)
        {
            var cameraObject = new GameObject("IsometricCamera");
            cameraObject.transform.SetParent(parent, false);
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(7.8f, 8.2f, -7.8f);
            cameraObject.transform.rotation = Quaternion.Euler(58f, 45f, 0f);

            var camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 6.2f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.02f, 0.025f, 0.03f, 1f);
            cameraObject.AddComponent<AudioListener>();

            var controller = cameraObject.AddComponent<PrototypeCameraController>();
            controller.session = session;
            controller.hostTarget = rat != null ? rat.transform : null;
            controller.virusTarget = virus != null && virus.virusPlayer != null ? virus.virusPlayer.transform : null;
            controller.startingHostMode = PrototypeCameraMode.QuarterView;
            controller.quarterViewOffset = new Vector3(-2.8f, 7.4f, -6.4f);
            controller.quarterViewOrthographicSize = 5.2f;
            controller.topViewOffset = new Vector3(0f, 9.5f, 0f);
            controller.topViewOrthographicSize = 5.8f;
            controller.ApplyCameraNow(PrototypeGameMode.RatHost);
        }

        private static void BuildLighting(Transform parent)
        {
            var lightObject = new GameObject("DirectionalLight");
            lightObject.transform.SetParent(parent, false);
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.15f;
            light.color = new Color(0.9f, 0.95f, 1f, 1f);
        }

        private static Button CreateButton(Transform parent, string name, string text, Vector2 anchoredPosition, Vector2 size)
        {
            var buttonObject = new GameObject(name);
            buttonObject.transform.SetParent(parent, false);
            var rect = buttonObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;

            var image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.18f, 0.22f, 0.22f, 0.95f);
            var button = buttonObject.AddComponent<Button>();
            button.targetGraphic = image;

            var label = CreateText(buttonObject.transform, "Label", text, 20, TextAnchor.MiddleCenter, Vector2.zero, size - new Vector2(18f, 18f), centered: true);
            label.raycastTarget = false;
            return button;
        }

        private static Text CreateText(Transform parent, string name, string text, int fontSize, TextAnchor anchor, Vector2 anchoredPosition, Vector2 size, bool centered = false, bool anchorRight = false)
        {
            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            var rect = textObject.AddComponent<RectTransform>();
            if (centered)
            {
                rect.anchorMin = new Vector2(0.5f, 1f);
                rect.anchorMax = new Vector2(0.5f, 1f);
            }
            else if (anchorRight)
            {
                rect.anchorMin = new Vector2(1f, 1f);
                rect.anchorMax = new Vector2(1f, 1f);
            }
            else
            {
                rect.anchorMin = new Vector2(0f, 1f);
                rect.anchorMax = new Vector2(0f, 1f);
            }

            rect.pivot = new Vector2(centered ? 0.5f : anchorRight ? 1f : 0f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;

            var uiText = textObject.AddComponent<Text>();
            uiText.text = text;
            uiText.fontSize = fontSize;
            uiText.alignment = anchor;
            uiText.color = Color.white;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.horizontalOverflow = HorizontalWrapMode.Wrap;
            uiText.verticalOverflow = VerticalWrapMode.Truncate;
            return uiText;
        }

        private static Slider CreateSlider(Transform parent, string name, Vector2 anchoredPosition, Vector2 size, Color fillColor)
        {
            var sliderObject = new GameObject(name);
            sliderObject.transform.SetParent(parent, false);
            var rect = sliderObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;

            var background = new GameObject("Background");
            background.transform.SetParent(sliderObject.transform, false);
            var backgroundRect = background.AddComponent<RectTransform>();
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;
            background.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.14f);

            var fillArea = new GameObject("FillArea");
            fillArea.transform.SetParent(sliderObject.transform, false);
            var fillAreaRect = fillArea.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = Vector2.zero;
            fillAreaRect.anchorMax = Vector2.one;
            fillAreaRect.offsetMin = Vector2.zero;
            fillAreaRect.offsetMax = Vector2.zero;

            var fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform, false);
            var fillRect = fill.AddComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            var fillImage = fill.AddComponent<Image>();
            fillImage.color = fillColor;

            var slider = sliderObject.AddComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 1f;
            slider.transition = Selectable.Transition.None;
            slider.interactable = false;
            slider.fillRect = fillRect;
            slider.targetGraphic = background.GetComponent<Image>();
            return slider;
        }

        private static GameObject CreatePanel(Transform parent, string name, Color color)
        {
            var panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            var rect = panel.AddComponent<RectTransform>();
            rect.sizeDelta = Vector2.zero;
            panel.AddComponent<Image>().color = color;
            return panel;
        }

        private static void SetStretchTop(RectTransform rect, float height)
        {
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(0f, height);
        }

        private static void SetCentered(RectTransform rect, Vector2 size)
        {
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = size;
        }

        private static Transform CreateChild(Transform parent, string name)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent, false);
            return child.transform;
        }

        private static GameObject CreatePrimitive(PrimitiveType type, string name, Transform parent, Vector3 localPosition, Vector3 localScale, Material material)
        {
            var primitive = GameObject.CreatePrimitive(type);
            primitive.name = name;
            primitive.transform.SetParent(parent, false);
            primitive.transform.localPosition = localPosition;
            primitive.transform.localScale = localScale;

            var renderer = primitive.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
            }

            return primitive;
        }

        private static GameObject CreateVisualPrimitive(PrimitiveType type, string name, Transform parent, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, Material material)
        {
            var visual = CreatePrimitive(type, name, parent, localPosition, localScale, material);
            visual.transform.localRotation = localRotation;

            var collider = visual.GetComponent<Collider>();
            if (collider != null)
            {
                Object.DestroyImmediate(collider);
            }

            return visual;
        }

        private static void EnsureFolders()
        {
            Directory.CreateDirectory(ProjectRoot);
            Directory.CreateDirectory(ProjectRoot + "/Scenes");
            Directory.CreateDirectory(ProjectRoot + "/Scripts");
            Directory.CreateDirectory(ProjectRoot + "/Settings/Input");
            Directory.CreateDirectory(ProjectRoot + "/Materials/Placeholder");
            Directory.CreateDirectory(ProjectRoot + "/Prefabs");
            Directory.CreateDirectory(RatSpriteRoot);
            Directory.CreateDirectory(ProjectRoot + "/Tests/EditMode");
        }

        private static void ConfigureRatSpriteImports()
        {
            foreach (var fileName in RatSpriteFileNames)
            {
                var path = $"{RatSpriteRoot}/{fileName}";
                var importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer == null)
                {
                    throw new InvalidOperationException($"Rat trial texture importer is unavailable: {path}");
                }

                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = RatSpritePixelsPerUnit;
                var textureSettings = new TextureImporterSettings();
                importer.ReadTextureSettings(textureSettings);
                textureSettings.spriteAlignment = (int)SpriteAlignment.Custom;
                textureSettings.spritePivot = RatSpritePivot;
                importer.SetTextureSettings(textureSettings);
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Point;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.SaveAndReimport();
            }
        }

        private static PrototypeMaterials CreateMaterials()
        {
            return new PrototypeMaterials
            {
                SewerFloor = CreateMaterial("SewerFloor", new Color(0.13f, 0.16f, 0.15f, 1f)),
                SewerWall = CreateMaterial("SewerWall", new Color(0.23f, 0.27f, 0.25f, 1f)),
                ToxicWater = CreateMaterial("ToxicWater", new Color(0.22f, 0.7f, 0.38f, 1f)),
                NoisyPipe = CreateMaterial("NoisyPipe", new Color(0.52f, 0.46f, 0.34f, 1f)),
                NoisyPipeBody = CreateMaterial("NoisyPipeBody", new Color(0.26f, 0.48f, 0.52f, 1f)),
                NoisyPipeValve = CreateMaterial("NoisyPipeValve", new Color(0.86f, 0.24f, 0.18f, 1f)),
                NoisyPipeMarker = CreateMaterial("NoisyPipeMarker", new Color(0.92f, 0.8f, 0.18f, 1f)),
                GateBlocked = CreateMaterial("GateBlocked", new Color(0.62f, 0.18f, 0.16f, 1f)),
                GateOpen = CreateMaterial("GateOpen", new Color(0.25f, 0.64f, 0.84f, 1f)),
                Rat = CreateMaterial("Rat", new Color(0.38f, 0.36f, 0.32f, 1f)),
                ArenaFloor = CreateMaterial("InternalArenaFloor", new Color(0.15f, 0.05f, 0.08f, 1f)),
                ArenaWall = CreateMaterial("InternalArenaWall", new Color(0.38f, 0.08f, 0.14f, 1f)),
                VirusPlayer = CreateMaterial("VirusPlayer", new Color(0.12f, 0.78f, 0.92f, 1f)),
                WhiteBloodCell = CreateMaterial("WhiteBloodCell", new Color(0.92f, 0.94f, 0.88f, 1f)),
                MutationFragment = CreateMaterial("MutationFragment", new Color(0.93f, 0.78f, 0.22f, 1f))
            };
        }

        private static Material CreateMaterial(string name, Color color)
        {
            var path = $"{ProjectRoot}/Materials/Placeholder/{name}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                var shader = Shader.Find("Universal Render Pipeline/Lit");
                if (shader == null)
                {
                    shader = Shader.Find("Standard");
                }

                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            material.color = color;
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void UpdateBuildSettings()
        {
            var sampleScene = new EditorBuildSettingsScene("Assets/Scenes/SampleScene.unity", false);
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(ScenePath, true),
                sampleScene
            };
        }

        private static void WriteInputActionsAsset()
        {
            var json = @"{
  ""name"": ""RatHostPrototypeControls"",
  ""maps"": [
    {
      ""name"": ""Host"",
      ""id"": """ + Guid.NewGuid() + @""",
      ""actions"": [
        { ""name"": ""Move"", ""type"": ""Value"", ""id"": """ + Guid.NewGuid() + @""", ""expectedControlType"": ""Vector2"", ""processors"": """", ""interactions"": """", ""initialStateCheck"": true },
        { ""name"": ""Interact"", ""type"": ""Button"", ""id"": """ + Guid.NewGuid() + @""", ""expectedControlType"": ""Button"", ""processors"": """", ""interactions"": """", ""initialStateCheck"": false }
      ],
      ""bindings"": [
        { ""name"": ""WASD"", ""id"": """ + Guid.NewGuid() + @""", ""path"": ""2DVector"", ""interactions"": """", ""processors"": """", ""groups"": ""Keyboard"", ""action"": ""Move"", ""isComposite"": true, ""isPartOfComposite"": false },
        { ""name"": ""up"", ""id"": """ + Guid.NewGuid() + @""", ""path"": ""<Keyboard>/w"", ""interactions"": """", ""processors"": """", ""groups"": ""Keyboard"", ""action"": ""Move"", ""isComposite"": false, ""isPartOfComposite"": true },
        { ""name"": ""down"", ""id"": """ + Guid.NewGuid() + @""", ""path"": ""<Keyboard>/s"", ""interactions"": """", ""processors"": """", ""groups"": ""Keyboard"", ""action"": ""Move"", ""isComposite"": false, ""isPartOfComposite"": true },
        { ""name"": ""left"", ""id"": """ + Guid.NewGuid() + @""", ""path"": ""<Keyboard>/a"", ""interactions"": """", ""processors"": """", ""groups"": ""Keyboard"", ""action"": ""Move"", ""isComposite"": false, ""isPartOfComposite"": true },
        { ""name"": ""right"", ""id"": """ + Guid.NewGuid() + @""", ""path"": ""<Keyboard>/d"", ""interactions"": """", ""processors"": """", ""groups"": ""Keyboard"", ""action"": ""Move"", ""isComposite"": false, ""isPartOfComposite"": true },
        { ""name"": """", ""id"": """ + Guid.NewGuid() + @""", ""path"": ""<Keyboard>/space"", ""interactions"": """", ""processors"": """", ""groups"": ""Keyboard"", ""action"": ""Interact"", ""isComposite"": false, ""isPartOfComposite"": false }
      ]
    },
    {
      ""name"": ""VirusMinigame"",
      ""id"": """ + Guid.NewGuid() + @""",
      ""actions"": [
        { ""name"": ""Move"", ""type"": ""Value"", ""id"": """ + Guid.NewGuid() + @""", ""expectedControlType"": ""Vector2"", ""processors"": """", ""interactions"": """", ""initialStateCheck"": true }
      ],
      ""bindings"": [
        { ""name"": ""WASD"", ""id"": """ + Guid.NewGuid() + @""", ""path"": ""2DVector"", ""interactions"": """", ""processors"": """", ""groups"": ""Keyboard"", ""action"": ""Move"", ""isComposite"": true, ""isPartOfComposite"": false },
        { ""name"": ""up"", ""id"": """ + Guid.NewGuid() + @""", ""path"": ""<Keyboard>/w"", ""interactions"": """", ""processors"": """", ""groups"": ""Keyboard"", ""action"": ""Move"", ""isComposite"": false, ""isPartOfComposite"": true },
        { ""name"": ""down"", ""id"": """ + Guid.NewGuid() + @""", ""path"": ""<Keyboard>/s"", ""interactions"": """", ""processors"": """", ""groups"": ""Keyboard"", ""action"": ""Move"", ""isComposite"": false, ""isPartOfComposite"": true },
        { ""name"": ""left"", ""id"": """ + Guid.NewGuid() + @""", ""path"": ""<Keyboard>/a"", ""interactions"": """", ""processors"": """", ""groups"": ""Keyboard"", ""action"": ""Move"", ""isComposite"": false, ""isPartOfComposite"": true },
        { ""name"": ""right"", ""id"": """ + Guid.NewGuid() + @""", ""path"": ""<Keyboard>/d"", ""interactions"": """", ""processors"": """", ""groups"": ""Keyboard"", ""action"": ""Move"", ""isComposite"": false, ""isPartOfComposite"": true }
      ]
    },
    {
      ""name"": ""UI"",
      ""id"": """ + Guid.NewGuid() + @""",
      ""actions"": [
        { ""name"": ""Confirm"", ""type"": ""Button"", ""id"": """ + Guid.NewGuid() + @""", ""expectedControlType"": ""Button"", ""processors"": """", ""interactions"": """", ""initialStateCheck"": false }
      ],
      ""bindings"": [
        { ""name"": """", ""id"": """ + Guid.NewGuid() + @""", ""path"": ""<Keyboard>/space"", ""interactions"": """", ""processors"": """", ""groups"": ""Keyboard"", ""action"": ""Confirm"", ""isComposite"": false, ""isPartOfComposite"": false }
      ]
    }
  ],
  ""controlSchemes"": [
    {
      ""name"": ""Keyboard"",
      ""bindingGroup"": ""Keyboard"",
      ""devices"": [
        { ""devicePath"": ""<Keyboard>"", ""isOptional"": false, ""isOR"": false }
      ]
    }
  ]
}";

            File.WriteAllText(InputActionPath, json);
        }

        private sealed class PrototypeMaterials
        {
            public Material SewerFloor;
            public Material SewerWall;
            public Material ToxicWater;
            public Material NoisyPipe;
            public Material NoisyPipeBody;
            public Material NoisyPipeValve;
            public Material NoisyPipeMarker;
            public Material GateBlocked;
            public Material GateOpen;
            public Material Rat;
            public Material ArenaFloor;
            public Material ArenaWall;
            public Material VirusPlayer;
            public Material WhiteBloodCell;
            public Material MutationFragment;
        }
    }
}
