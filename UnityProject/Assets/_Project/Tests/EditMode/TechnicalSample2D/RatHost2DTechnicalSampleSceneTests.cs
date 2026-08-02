using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LastHost.Prototype.TechnicalSample2D.Tests
{
    public sealed class RatHost2DTechnicalSampleSceneTests
    {
        private const string SampleScenePath =
            "Assets/_Project/Scenes/RatHost2DTechnicalSample.unity";
        private const string LegacyScenePath =
            "Assets/_Project/Scenes/RatHostPrototype.unity";
        private const string InputAssetPath =
            "Assets/_Project/Settings/Input/RatHostPrototypeControls.inputactions";

        [TearDown]
        public void TearDown()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        [Test]
        public void E01_SampleSceneContainsRequiredIsolatedTwoDimensionalContract()
        {
            OpenRequiredSampleScene();

            var controllers = Object.FindObjectsByType<RatHost2DController>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);
            var followCameras = Object.FindObjectsByType<PixelFollowCamera2D>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);
            var sorters = Object.FindObjectsByType<YSortSprite2D>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);
            var sideViews = Object.FindObjectsByType<RatSide3FrameView>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            Assert.That(controllers, Has.Length.EqualTo(1));
            Assert.That(controllers[0].GetComponent<Rigidbody2D>(), Is.Not.Null);
            Assert.That(controllers[0].GetComponent<Collider2D>(), Is.Not.Null);
            Assert.That(followCameras, Has.Length.EqualTo(1));
            Assert.That(followCameras[0].TargetCamera.orthographic, Is.True);
            Assert.That(sorters.Length, Is.GreaterThanOrEqualTo(2));
            Assert.That(sideViews, Has.Length.EqualTo(1));
            Assert.That(sideViews[0].FrameCount, Is.EqualTo(3));
            Assert.That(Object.FindFirstObjectByType<Production2DSampleHud>(
                FindObjectsInactive.Include), Is.Not.Null);

            var propsRoot = GameObject.Find(
                "TechnicalSample2D/Environment/YSortProps");
            Assert.That(propsRoot, Is.Not.Null);
            Assert.That(propsRoot.transform.childCount, Is.GreaterThanOrEqualTo(3));
            Assert.That(
                propsRoot.GetComponentsInChildren<BoxCollider2D>(true),
                Is.Empty,
                "Measured prop footprints must not regress to rectangular colliders.");
            Assert.That(
                propsRoot.GetComponentsInChildren<PolygonCollider2D>(true),
                Has.Length.EqualTo(2));
            AssertPolygonPropContract(propsRoot, "Barrel_A", 16);
            AssertPolygonPropContract(propsRoot, "Crate_A", 4);
            Assert.That(
                Object.FindObjectsByType<VisualOcclusionResolver2D>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None),
                Is.Empty,
                "The scene must not hide overlap through a visibility resolver.");
        }

        [TestCase("Barrel_A")]
        [TestCase("Crate_A")]
        public void YSortPropFootprintBlocksRigidbodyMovementWithoutPenetration(
            string propName)
        {
            OpenRequiredSampleScene();
            var prop = GameObject.Find(
                $"TechnicalSample2D/Environment/YSortProps/{propName}");
            Assert.That(prop, Is.Not.Null);
            var obstacle = prop.GetComponent<PolygonCollider2D>();
            Assert.That(obstacle, Is.Not.Null);

            VerifyCollisionClampAgainst(obstacle);
        }

        [Test]
        public void E11_HudInputAndLegacyProtectionContractsArePresent()
        {
            var legacyScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(LegacyScenePath);
            var inputAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputAssetPath);

            Assert.That(legacyScene, Is.Not.Null, "The protected legacy scene must remain present.");
            Assert.That(inputAsset, Is.Not.Null);
            Assert.That(inputAsset.FindAction(TechnicalSample2DConstants.MoveActionPath, false),
                Is.Not.Null);
            Assert.That(SampleScenePath, Is.Not.EqualTo(LegacyScenePath));

            OpenRequiredSampleScene();

            AssertHudTextIsPresent("SampleTitle");
            AssertHudTextIsPresent("RuntimeStatusText");
            Assert.That(GameObject.Find("HostPortraitFrame"), Is.Not.Null);
            Assert.That(GameObject.Find("HostHealthFrame"), Is.Not.Null);
            Assert.That(GameObject.Find("HostImmuneFrame"), Is.Not.Null);
            Assert.That(SceneManager.GetActiveScene().path, Is.EqualTo(SampleScenePath));
        }

        private static void OpenRequiredSampleScene()
        {
            var sampleScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(SampleScenePath);
            Assert.That(sampleScene, Is.Not.Null,
                "The integration agent must generate the isolated 2D technical sample scene.");

            var openedScene = EditorSceneManager.OpenScene(SampleScenePath, OpenSceneMode.Single);
            Assert.That(openedScene.IsValid(), Is.True);
            Assert.That(openedScene.isLoaded, Is.True);
        }

        private static void AssertHudTextIsPresent(string objectName)
        {
            var hudObject = GameObject.Find(objectName);
            Assert.That(hudObject, Is.Not.Null, $"HUD object '{objectName}' is required.");
            var text = hudObject.GetComponent<UnityEngine.UI.Text>();
            Assert.That(text, Is.Not.Null);
            Assert.That(text.text, Is.Not.Null.And.Not.Empty);
        }

        private static void AssertPolygonPropContract(
            GameObject propsRoot,
            string propName,
            int expectedPointCount)
        {
            var prop = propsRoot.transform.Find(propName);
            Assert.That(prop, Is.Not.Null, propName);

            var colliders = prop.GetComponents<PolygonCollider2D>();
            Assert.That(colliders, Has.Length.EqualTo(1), propName);
            var footprint = colliders[0];
            Assert.That(footprint.isTrigger, Is.False, propName);
            Assert.That(footprint.pathCount, Is.EqualTo(1), propName);
            Assert.That(
                footprint.GetPath(0),
                Has.Length.EqualTo(expectedPointCount),
                propName);
            Assert.That(prop.GetComponent<BoxCollider2D>(), Is.Null, propName);
            Assert.That(prop.GetComponent<Rigidbody2D>(), Is.Null, propName);

            var renderer = prop.GetComponent<SpriteRenderer>();
            Assert.That(renderer, Is.Not.Null, propName);
            Assert.That(renderer.enabled, Is.True, propName);
            Assert.That(renderer.color.a, Is.EqualTo(1f), propName);
            Assert.That(prop.GetComponent<YSortSprite2D>(), Is.Not.Null, propName);
            Assert.That(prop.GetComponent<VisualOcclusionResolver2D>(), Is.Null, propName);
        }

        private static void VerifyCollisionClampAgainst(PolygonCollider2D obstacle)
        {
            var originalSimulationMode = Physics2D.simulationMode;
            var probe = new GameObject("PropCollisionProbe");

            try
            {
                Physics2D.simulationMode = SimulationMode2D.Script;
                var body = probe.AddComponent<Rigidbody2D>();
                body.bodyType = RigidbodyType2D.Dynamic;
                body.gravityScale = 0f;
                body.freezeRotation = true;
                body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                var probeCollider = probe.AddComponent<BoxCollider2D>();
                probeCollider.size = new Vector2(0.2f, 0.2f);
                var controller = probe.AddComponent<RatHost2DController>();
                controller.Configure(null, 3f);
                controller.CacheMoveInput(Vector2.right);

                probe.transform.position = new Vector3(
                    obstacle.bounds.min.x - 0.5f,
                    obstacle.bounds.center.y,
                    0f);
                foreach (var sceneCollider in Object.FindObjectsByType<Collider2D>(
                             FindObjectsInactive.Include,
                             FindObjectsSortMode.None))
                {
                    if (sceneCollider != obstacle && sceneCollider != probeCollider)
                    {
                        sceneCollider.enabled = false;
                    }
                }

                Physics2D.SyncTransforms();
                var positionAfterSixtySteps = Vector2.zero;
                for (var index = 0; index < 120; index++)
                {
                    controller.SimulateFixedStep(0.02f);
                    Assert.That(Physics2D.Simulate(0.02f), Is.True);
                    if (index == 59)
                    {
                        positionAfterSixtySteps = body.position;
                    }
                }

                Physics2D.SyncTransforms();
                var signedDistance =
                    Physics2D.Distance(probeCollider, obstacle).distance;
                Assert.That(signedDistance, Is.GreaterThanOrEqualTo(-0.001f));
                Assert.That(
                    Mathf.Abs(body.position.x - positionAfterSixtySteps.x),
                    Is.LessThanOrEqualTo(1f / 64f));
                Assert.That(body.position.x, Is.LessThan(obstacle.bounds.center.x));
            }
            finally
            {
                Physics2D.simulationMode = originalSimulationMode;
                Object.DestroyImmediate(probe);
            }
        }
    }
}
