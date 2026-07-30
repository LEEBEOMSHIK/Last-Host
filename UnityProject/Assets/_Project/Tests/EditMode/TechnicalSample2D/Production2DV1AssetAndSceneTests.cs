using System;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace LastHost.Prototype.TechnicalSample2D.Tests
{
    public sealed class Production2DV1AssetAndSceneTests
    {
        private const string ProductionRoot =
            "Assets/_Project/Art/Production2D/V1";
        private const string SampleScenePath =
            "Assets/_Project/Scenes/RatHost2DTechnicalSample.unity";

        [TearDown]
        public void TearDown()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        [Test]
        public void ProductionV1_ContainsExactlyEighteenConfiguredRgbaSprites()
        {
            var textureGuids = AssetDatabase.FindAssets(
                "t:Texture2D",
                new[] { ProductionRoot });

            Assert.That(textureGuids, Has.Length.EqualTo(18));

            foreach (var guid in textureGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var importer = AssetImporter.GetAtPath(path) as TextureImporter;
                Assert.That(importer, Is.Not.Null, path);
                Assert.That(importer.textureType, Is.EqualTo(TextureImporterType.Sprite), path);
                Assert.That(importer.spriteImportMode, Is.EqualTo(SpriteImportMode.Single), path);
                Assert.That(importer.spritePixelsPerUnit, Is.EqualTo(128f), path);
                Assert.That(importer.filterMode, Is.EqualTo(FilterMode.Point), path);
                Assert.That(importer.mipmapEnabled, Is.False, path);
                Assert.That(importer.alphaIsTransparency, Is.True, path);
                Assert.That(
                    importer.textureCompression,
                    Is.EqualTo(TextureImporterCompression.Uncompressed),
                    path);
            }
        }

        [Test]
        public void ProductionV1_RatFramesShareCanvasPivotAndSideOnlyNaming()
        {
            var ratPaths = AssetDatabase.FindAssets(
                    "t:Texture2D",
                    new[] { ProductionRoot + "/Rat" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .OrderBy(path => path, StringComparer.Ordinal)
                .ToArray();

            Assert.That(ratPaths, Has.Length.EqualTo(4));
            foreach (var path in ratPaths)
            {
                Assert.That(path, Does.Contain("/rat_side_"));
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                Assert.That(sprite, Is.Not.Null, path);
                Assert.That(sprite.pixelsPerUnit, Is.EqualTo(128f), path);

                if (!path.EndsWith("_sheet.png", StringComparison.Ordinal))
                {
                    Assert.That(sprite.rect.width, Is.EqualTo(256f), path);
                    Assert.That(sprite.rect.height, Is.EqualTo(192f), path);
                    Assert.That(sprite.pivot.x, Is.EqualTo(128f).Within(0.01f), path);
                    Assert.That(sprite.pivot.y, Is.EqualTo(40f).Within(0.01f), path);
                }
            }
        }

        [Test]
        public void ProductionV1_SampleSceneUsesActualEnvironmentRatHudAndTwoDimensionalContract()
        {
            OpenSampleScene();

            var controller = UnityEngine.Object.FindFirstObjectByType<RatHost2DController>(
                FindObjectsInactive.Include);
            var view = UnityEngine.Object.FindFirstObjectByType<RatSide3FrameView>(
                FindObjectsInactive.Include);
            var follow = UnityEngine.Object.FindFirstObjectByType<PixelFollowCamera2D>(
                FindObjectsInactive.Include);
            var hud = UnityEngine.Object.FindFirstObjectByType<Production2DSampleHud>(
                FindObjectsInactive.Include);
            var tilemaps = UnityEngine.Object.FindObjectsByType<Tilemap>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            Assert.That(controller, Is.Not.Null);
            Assert.That(controller.GetComponent<Rigidbody2D>(), Is.Not.Null);
            Assert.That(controller.GetComponent<Collider2D>(), Is.Not.Null);
            Assert.That(view, Is.Not.Null);
            Assert.That(view.FrameCount, Is.EqualTo(3));
            Assert.That(view.transform.parent.name, Is.EqualTo("RatHost2D"));
            AssertProductionSprite(view.TargetRenderer.sprite);
            Assert.That(follow, Is.Not.Null);
            Assert.That(follow.Target, Is.EqualTo(controller.transform));
            Assert.That(follow.TargetCamera.orthographic, Is.True);
            Assert.That(hud, Is.Not.Null);
            Assert.That(tilemaps.Select(tilemap => tilemap.name),
                Does.Contain("FloorTilemap"));
            Assert.That(tilemaps.Select(tilemap => tilemap.name),
                Does.Contain("BlockingWaterTilemap"));

            AssertProductionSprite(
                GameObject.Find("TechnicalSample2D/Environment/YSortWalls/WallCorner_BackLeft")
                    .GetComponent<SpriteRenderer>().sprite);
            AssertProductionSprite(
                GameObject.Find("TechnicalSample2D/Environment/YSortProps/Barrel_A")
                    .GetComponent<SpriteRenderer>().sprite);
            AssertProductionSprite(
                GameObject.Find("TechnicalSample2D/Environment/YSortProps/Crate_A")
                    .GetComponent<SpriteRenderer>().sprite);
            AssertProductionSprite(
                GameObject.Find("TechnicalSample2D/Environment/YSortProps/Drain_A")
                    .GetComponent<SpriteRenderer>().sprite);

            var waterCollider = GameObject.Find(
                    "TechnicalSample2D/Environment/Grid/BlockingWaterTilemap")
                .GetComponent<TilemapCollider2D>();
            Assert.That(waterCollider, Is.Not.Null);
            Assert.That(GameObject.Find("TechnicalSample2D/Environment/RoomBoundary")
                .GetComponent<EdgeCollider2D>(), Is.Not.Null);

            AssertHudImageUsesProductionSprite("HostPortraitSubject");
            AssertHudImageUsesProductionSprite("HostPortraitFrame");
            AssertHudImageUsesProductionSprite("HostHealthFill");
            AssertHudImageUsesProductionSprite("HostHealthFrame");
            AssertHudImageUsesProductionSprite("HostImmuneFill");
            AssertHudImageUsesProductionSprite("HostImmuneFrame");
            Assert.That(SceneManager.GetActiveScene().path, Is.EqualTo(SampleScenePath));
        }

        [Test]
        public void SideThreeFrameViewMirrorsOnlyTheSuppliedSideAndNeverMovesLogicalRoot()
        {
            var root = new GameObject("RatHost2D");
            var visual = new GameObject("Visual");
            var texture = new Texture2D(8, 8);
            var sprites = new Sprite[3];

            try
            {
                visual.transform.SetParent(root.transform, false);
                var renderer = visual.AddComponent<SpriteRenderer>();
                var view = visual.AddComponent<RatSide3FrameView>();
                for (var index = 0; index < sprites.Length; index++)
                {
                    sprites[index] = Sprite.Create(
                        texture,
                        new Rect(0f, 0f, 8f, 8f),
                        new Vector2(0.5f, 0.25f),
                        128f);
                }

                view.Configure(null, renderer, sprites, 10f);
                root.transform.position = new Vector3(2.25f, -1.5f, 0f);
                var initialRoot = root.transform.position;

                view.ApplyView(Vector2.left, true, 0.11f);
                Assert.That(view.FacesRight, Is.False);
                Assert.That(renderer.flipX, Is.True);
                Assert.That(view.CurrentFrameIndex, Is.EqualTo(1));
                Assert.That(root.transform.position, Is.EqualTo(initialRoot));

                view.ApplyView(Vector2.right, false, 0.1f);
                Assert.That(view.FacesRight, Is.True);
                Assert.That(renderer.flipX, Is.False);
                Assert.That(view.CurrentFrameIndex, Is.Zero);
                Assert.That(root.transform.position, Is.EqualTo(initialRoot));
            }
            finally
            {
                foreach (var sprite in sprites)
                {
                    if (sprite != null)
                    {
                        UnityEngine.Object.DestroyImmediate(sprite);
                    }
                }

                UnityEngine.Object.DestroyImmediate(texture);
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void ProductionV1_OccludersUseGroundPivotTransitionAndCorrectedFootprints()
        {
            OpenSampleScene();

            var rat = GameObject.Find("TechnicalSample2D/Actors/RatHost2D");
            Assert.That(rat, Is.Not.Null);
            var ratCollider = rat.GetComponent<CapsuleCollider2D>();
            Assert.That(ratCollider, Is.Not.Null);
            Assert.That(ratCollider.size, Is.EqualTo(new Vector2(0.92f, 0.26f)));

            AssertOccluderContract(
                "TechnicalSample2D/Environment/YSortProps/Barrel_A",
                new Vector2(0.60f, 0.22f));
            AssertOccluderContract(
                "TechnicalSample2D/Environment/YSortProps/Crate_A",
                new Vector2(0.70f, 0.24f));
            AssertOccluderContract(
                "TechnicalSample2D/Environment/YSortWalls/WallStraight_Occlusion",
                new Vector2(1.05f, 0.18f));
        }

        [Test]
        public void ProductionV1_StationaryOccluderSortingDoesNotJitter()
        {
            OpenSampleScene();

            var objectNames = new[]
            {
                "TechnicalSample2D/Environment/YSortProps/Barrel_A",
                "TechnicalSample2D/Environment/YSortProps/Crate_A",
                "TechnicalSample2D/Environment/YSortWalls/WallStraight_Occlusion",
            };

            foreach (var objectName in objectNames)
            {
                var sorter = GameObject.Find(objectName).GetComponent<YSortSprite2D>();
                var initialOrder = sorter.ApplySorting();
                for (var iteration = 0; iteration < 300; iteration++)
                {
                    Assert.That(sorter.ApplySorting(), Is.EqualTo(initialOrder), objectName);
                }
            }
        }

        private static void OpenSampleScene()
        {
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(SampleScenePath);
            Assert.That(sceneAsset, Is.Not.Null);
            var opened = EditorSceneManager.OpenScene(SampleScenePath, OpenSceneMode.Single);
            Assert.That(opened.IsValid(), Is.True);
            Assert.That(opened.isLoaded, Is.True);
        }

        private static void AssertOccluderContract(string objectName, Vector2 expectedColliderSize)
        {
            var target = GameObject.Find(objectName);
            Assert.That(target, Is.Not.Null, objectName);

            var footprint = target.GetComponent<BoxCollider2D>();
            var sorter = target.GetComponent<YSortSprite2D>();
            Assert.That(footprint, Is.Not.Null, objectName);
            Assert.That(sorter, Is.Not.Null, objectName);
            Assert.That(footprint.size, Is.EqualTo(expectedColliderSize), objectName);

            var groundY = target.transform.position.y;
            var objectOrder = sorter.ApplySorting();
            Assert.That(
                objectOrder,
                Is.EqualTo(YSortOrder2D.Calculate(groundY, 0, 1)),
                objectName);
            Assert.That(
                YSortOrder2D.Calculate(groundY - 0.02f),
                Is.GreaterThan(objectOrder),
                objectName + " front");
            Assert.That(
                YSortOrder2D.Calculate(groundY),
                Is.LessThan(objectOrder),
                objectName + " same pivot");
            Assert.That(
                YSortOrder2D.Calculate(groundY + 0.02f),
                Is.LessThan(objectOrder),
                objectName + " behind");
        }

        private static void AssertProductionSprite(Sprite sprite)
        {
            Assert.That(sprite, Is.Not.Null);
            Assert.That(AssetDatabase.GetAssetPath(sprite), Does.StartWith(ProductionRoot));
        }

        private static void AssertHudImageUsesProductionSprite(string objectName)
        {
            var target = GameObject.Find(objectName);
            Assert.That(target, Is.Not.Null, objectName);
            var image = target.GetComponent<Image>();
            Assert.That(image, Is.Not.Null, objectName);
            AssertProductionSprite(image.sprite);
        }
    }
}
