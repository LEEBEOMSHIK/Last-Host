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
            var occlusionResolver =
                UnityEngine.Object.FindFirstObjectByType<VisualOcclusionResolver2D>(
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
            Assert.That(occlusionResolver, Is.Not.Null);
            Assert.That(
                occlusionResolver.MinimumFragmentWidth,
                Is.EqualTo(4f / 128f).Within(0.000001f));
            Assert.That(
                occlusionResolver.ReleaseHysteresis,
                Is.EqualTo(2f / 128f).Within(0.000001f));
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
                var bodyClearance = root.AddComponent<CapsuleCollider2D>();
                for (var index = 0; index < sprites.Length; index++)
                {
                    sprites[index] = Sprite.Create(
                        texture,
                        new Rect(0f, 0f, 8f, 8f),
                        new Vector2(0.5f, 0.25f),
                        128f);
                }

                view.Configure(null, renderer, sprites, 10f);
                view.ConfigureBodyClearance(
                    bodyClearance,
                    new Vector2(1.28f, 0.26f),
                    new Vector2(0.30f, 0.13f));
                root.transform.position = new Vector3(2.25f, -1.5f, 0f);
                var initialRoot = root.transform.position;

                view.ApplyView(Vector2.left, true, 0.11f);
                Assert.That(view.FacesRight, Is.False);
                Assert.That(renderer.flipX, Is.True);
                Assert.That(bodyClearance.offset.x, Is.EqualTo(-0.30f).Within(0.000001f));
                Assert.That(view.CurrentFrameIndex, Is.EqualTo(1));
                Assert.That(root.transform.position, Is.EqualTo(initialRoot));

                view.ApplyView(Vector2.right, false, 0.1f);
                Assert.That(view.FacesRight, Is.True);
                Assert.That(renderer.flipX, Is.False);
                Assert.That(bodyClearance.offset.x, Is.EqualTo(0.30f).Within(0.000001f));
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
            Assert.That(ratCollider.size, Is.EqualTo(new Vector2(1.28f, 0.26f)));
            Assert.That(ratCollider.offset, Is.EqualTo(new Vector2(0.30f, 0.13f)));

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
        public void ProductionV1_WholeCharacterOcclusionUsesFourPixelEntryAndTwoPixelRelease()
        {
            OpenSampleScene();

            var rat = GameObject.Find("TechnicalSample2D/Actors/RatHost2D");
            var wall = GameObject.Find(
                "TechnicalSample2D/Environment/YSortWalls/WallStraight_Occlusion");
            var view = rat.GetComponentInChildren<RatSide3FrameView>();
            var resolver = rat.GetComponentInChildren<VisualOcclusionResolver2D>();

            Assert.That(view, Is.Not.Null);
            Assert.That(resolver, Is.Not.Null);
            Assert.That(
                view.TargetRenderer.enabled,
                Is.False,
                "The saved scene reproduces the builder-time serialized hide state.");

            view.ApplyView(Vector2.right, false, 0f);
            rat.transform.position = new Vector3(wall.transform.position.x, 0.90f, 0f);
            Assert.That(resolver.ResolveNow(), Is.True);
            Assert.That(view.TargetRenderer.enabled, Is.False);
            Assert.That(resolver.VisibilityTransitionCount, Is.EqualTo(1));

            for (var iteration = 0; iteration < 300; iteration++)
            {
                Assert.That(resolver.ResolveNow(), Is.True);
            }

            Assert.That(resolver.VisibilityTransitionCount, Is.EqualTo(1));

            // At +0.37 world units the left fragment is below the four-pixel
            // entry threshold but above the two-pixel release threshold.
            rat.transform.position = new Vector3(
                wall.transform.position.x + 0.37f,
                0.90f,
                0f);
            Assert.That(resolver.ResolveNow(), Is.True);
            Assert.That(resolver.VisibilityTransitionCount, Is.EqualTo(1));

            rat.transform.position = new Vector3(
                wall.transform.position.x + 0.38f,
                0.90f,
                0f);
            Assert.That(resolver.ResolveNow(), Is.False);
            Assert.That(view.TargetRenderer.enabled, Is.True);
            Assert.That(resolver.VisibilityTransitionCount, Is.EqualTo(2));
        }

        [Test]
        public void WholeCharacterOcclusionRequiresTwoVisibleFragmentsAndCoreIntersection()
        {
            var visible = Rect.MinMaxRect(-0.93f, 0f, 0.93f, 0.59f);
            var core = Rect.MinMaxRect(-0.33f, 0f, 0.90f, 0.59f);
            var centeredOccluder = Rect.MinMaxRect(-0.27f, 0.02f, 0.27f, 0.84f);

            Assert.That(
                VisualOcclusionResolver2D.WouldSplitIntoTwoVisibleFragments(
                    visible,
                    core,
                    centeredOccluder,
                    4f / 128f),
                Is.True);
            Assert.That(
                VisualOcclusionResolver2D.WouldSplitIntoTwoVisibleFragments(
                    visible,
                    core,
                    Rect.MinMaxRect(-1.0f, 0.02f, -0.91f, 0.84f),
                    4f / 128f),
                Is.False);
            Assert.That(
                VisualOcclusionResolver2D.WouldSplitIntoTwoVisibleFragments(
                    visible,
                    core,
                    Rect.MinMaxRect(-0.27f, 0.70f, 0.27f, 0.90f),
                    4f / 128f),
                Is.False);

            Assert.That(
                VisualOcclusionResolver2D.WouldSplitIntoTwoVisibleFragments(
                    visible,
                    core,
                    Rect.MinMaxRect(-0.30f, 0.02f, 1.0f, 0.84f),
                    4f / 128f),
                Is.True,
                "A detached tail-only fragment on one side must also be hidden.");

            var flippedCore = Rect.MinMaxRect(-0.90f, 0f, 0.33f, 0.59f);
            Assert.That(
                VisualOcclusionResolver2D.WouldSplitIntoTwoVisibleFragments(
                    visible,
                    flippedCore,
                    Rect.MinMaxRect(-1.0f, 0.02f, 0.30f, 0.84f),
                    4f / 128f),
                Is.True,
                "The mirrored tail-only fragment must follow the flipped core bounds.");
        }

        [Test]
        public void WholeCharacterOcclusionPreservesAnExternallyDisabledRenderer()
        {
            var target = new GameObject("OcclusionTarget");
            var texture = new Texture2D(8, 8);
            var sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, 8f, 8f),
                new Vector2(0.5f, 0.5f),
                128f);

            try
            {
                var renderer = target.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                renderer.enabled = false;
                var resolver = target.AddComponent<VisualOcclusionResolver2D>();
                resolver.Configure(
                    renderer,
                    null,
                    new[]
                    {
                        new VisualOcclusionResolver2D.FrameAlphaContract(
                            sprite,
                            Rect.MinMaxRect(-0.03f, -0.03f, 0.03f, 0.03f),
                            Rect.MinMaxRect(-0.02f, -0.02f, 0.02f, 0.02f))
                    },
                    Array.Empty<VisualOcclusionResolver2D.OccluderContract>(),
                    4f / 128f,
                    2f / 128f);

                Assert.That(resolver.ResolveNow(), Is.False);
                Assert.That(renderer.enabled, Is.False);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(sprite);
                UnityEngine.Object.DestroyImmediate(texture);
                UnityEngine.Object.DestroyImmediate(target);
            }
        }

        [Test]
        public void WholeCharacterOcclusionReleaseHysteresisProtectsHorizontalCoreBoundary()
        {
            const float fragmentWidth = 4f / 128f;
            const float hysteresis = 2f / 128f;
            var occluder = Rect.MinMaxRect(-0.27f, 0.02f, 0.27f, 0.84f);

            var entryVisible = Rect.MinMaxRect(-0.331f, 0f, 1.529f, 0.59f);
            var entryCore = Rect.MinMaxRect(0.269f, 0f, 1.499f, 0.59f);
            Assert.That(
                VisualOcclusionResolver2D.WouldSplitIntoTwoVisibleFragments(
                    entryVisible,
                    entryCore,
                    occluder,
                    fragmentWidth),
                Is.True);

            var subpixelVisible = Rect.MinMaxRect(-0.316f, 0f, 1.544f, 0.59f);
            var subpixelCore = Rect.MinMaxRect(0.284f, 0f, 1.514f, 0.59f);
            Assert.That(
                VisualOcclusionResolver2D.WouldSplitIntoTwoVisibleFragments(
                    subpixelVisible,
                    subpixelCore,
                    occluder,
                    fragmentWidth),
                Is.False,
                "The unexpanded character core loses horizontal barrel intersection.");
            Assert.That(
                VisualOcclusionResolver2D.WouldRemainOccludedDuringRelease(
                    subpixelVisible,
                    subpixelCore,
                    occluder,
                    fragmentWidth,
                    hysteresis),
                Is.True,
                "Two-pixel core release hysteresis must protect the horizontal boundary.");

            var releasedVisible = Rect.MinMaxRect(-0.313f, 0f, 1.547f, 0.59f);
            var releasedCore = Rect.MinMaxRect(0.287f, 0f, 1.517f, 0.59f);
            Assert.That(
                VisualOcclusionResolver2D.WouldRemainOccludedDuringRelease(
                    releasedVisible,
                    releasedCore,
                    occluder,
                    fragmentWidth,
                    hysteresis),
                Is.False,
                "Release must still occur after crossing the two-pixel horizontal band.");

            var fragmentSensitiveVisible =
                Rect.MinMaxRect(-0.2934375f, 0f, 1.50f, 0.59f);
            var overlappingCore = Rect.MinMaxRect(-0.10f, 0f, 1.40f, 0.59f);
            Assert.That(
                VisualOcclusionResolver2D.WouldSplitIntoTwoVisibleFragments(
                    fragmentSensitiveVisible,
                    overlappingCore,
                    occluder,
                    fragmentWidth),
                Is.False,
                "A three-pixel fragment must not meet the four-pixel entry contract.");
            Assert.That(
                VisualOcclusionResolver2D.WouldRemainOccludedDuringRelease(
                    fragmentSensitiveVisible,
                    overlappingCore,
                    occluder,
                    fragmentWidth,
                    hysteresis),
                Is.True,
                "The existing two-pixel fragment release band must remain intact.");
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
