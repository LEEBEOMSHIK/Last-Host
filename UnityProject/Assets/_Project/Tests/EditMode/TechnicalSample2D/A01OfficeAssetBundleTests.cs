using System;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D.Tests
{
    public sealed class A01OfficeAssetBundleTests
    {
        private const string Root = "Assets/_Project/Art/Cinematics/Opening/A01/Office";
        private const string BackgroundPath = Root + "/a01-office-background-v1.png";
        private const string CastPath = Root + "/a01-office-cast-poses-v1.png";
        private const string OcclusionMaskPath = Root + "/a01-office-occlusion-mask-v1.png";

        private static readonly string[] FrameNames =
        {
            "p1_seated_idle", "p1_speaking", "p1_laugh", "p1_rise_start",
            "p2_seated_idle", "p2_nod_smile", "p2_laugh", "p2_neutral",
            "p3_seated_work", "p3_shoulder_laugh", "p3_head_turn", "p3_neutral",
            "p4_standing_idle", "p4_conversation", "p4_exit_turn", "p4_neutral",
            "p5_standing_idle", "p5_laugh", "p5_exit_step", "p5_neutral"
        };

        [Test]
        public void A01Office_BackgroundImportsAsApprovedSingleSprite()
        {
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(BackgroundPath);
            var backgroundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BackgroundPath);
            var importer = AssetImporter.GetAtPath(BackgroundPath) as TextureImporter;

            Assert.That(texture, Is.Not.Null);
            Assert.That(backgroundSprite, Is.Not.Null);
            Assert.That(backgroundSprite.GetPhysicsShapeCount(), Is.EqualTo(0));
            Assert.That(texture.width, Is.EqualTo(1672));
            Assert.That(texture.height, Is.EqualTo(941));
            Assert.That(importer, Is.Not.Null);
            Assert.That(importer.textureType, Is.EqualTo(TextureImporterType.Sprite));
            Assert.That(importer.spriteImportMode, Is.EqualTo(SpriteImportMode.Single));
            Assert.That(importer.spritePixelsPerUnit, Is.EqualTo(100f));
            Assert.That(importer.filterMode, Is.EqualTo(FilterMode.Point));
            Assert.That(importer.mipmapEnabled, Is.False);
            Assert.That(importer.textureCompression, Is.EqualTo(TextureImporterCompression.Uncompressed));
        }

        [Test]
        public void A01Office_CastImportsTwentyConfiguredFrames()
        {
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(CastPath);
            var importer = AssetImporter.GetAtPath(CastPath) as TextureImporter;
            var sprites = AssetDatabase.LoadAllAssetsAtPath(CastPath).OfType<Sprite>()
                .OrderBy(sprite => Array.IndexOf(FrameNames, sprite.name))
                .ToArray();

            Assert.That(texture, Is.Not.Null);
            Assert.That(texture.width, Is.EqualTo(1280));
            Assert.That(texture.height, Is.EqualTo(1600));
            Assert.That(importer, Is.Not.Null);
            Assert.That(importer.textureType, Is.EqualTo(TextureImporterType.Sprite));
            Assert.That(importer.spriteImportMode, Is.EqualTo(SpriteImportMode.Multiple));
            Assert.That(importer.spritePixelsPerUnit, Is.EqualTo(100f));
            Assert.That(importer.filterMode, Is.EqualTo(FilterMode.Point));
            Assert.That(importer.mipmapEnabled, Is.False);
            Assert.That(importer.textureCompression, Is.EqualTo(TextureImporterCompression.Uncompressed));
            Assert.That(importer.alphaIsTransparency, Is.True);
            Assert.That(sprites.Length, Is.EqualTo(20));
            Assert.That(FrameNames.Length, Is.EqualTo(20));
            Assert.That(sprites.Select(sprite => sprite.name), Is.EqualTo(FrameNames));

            for (var index = 0; index < sprites.Length; index++)
            {
                var sprite = sprites[index];
                var expectedX = (index % 4) * 320f;
                var expectedY = (4 - (index / 4)) * 320f;
                Assert.That(sprite.rect, Is.EqualTo(new Rect(expectedX, expectedY, 320f, 320f)));
                Assert.That(sprite.pivot.x, Is.EqualTo(160f).Within(0.001f));
                Assert.That(sprite.pivot.y, Is.EqualTo(14f).Within(0.001f));
                Assert.That(sprite.pixelsPerUnit, Is.EqualTo(100f));
                Assert.That(sprite.GetPhysicsShapeCount(), Is.EqualTo(0), sprite.name);
            }
        }

        [Test]
        public void A01Office_OcclusionMaskImportsAsSingleSpriteWithoutPhysics()
        {
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(OcclusionMaskPath);
            var occlusionMaskSprite = AssetDatabase.LoadAssetAtPath<Sprite>(OcclusionMaskPath);
            var importer = AssetImporter.GetAtPath(OcclusionMaskPath) as TextureImporter;

            Assert.That(texture, Is.Not.Null);
            Assert.That(occlusionMaskSprite, Is.Not.Null);
            Assert.That(occlusionMaskSprite.GetPhysicsShapeCount(), Is.EqualTo(0));
            Assert.That(texture.width, Is.EqualTo(1672));
            Assert.That(texture.height, Is.EqualTo(941));
            Assert.That(importer, Is.Not.Null);
            Assert.That(importer.textureType, Is.EqualTo(TextureImporterType.Sprite));
            Assert.That(importer.spriteImportMode, Is.EqualTo(SpriteImportMode.Single));
            Assert.That(importer.spritePixelsPerUnit, Is.EqualTo(100f));
            Assert.That(importer.filterMode, Is.EqualTo(FilterMode.Point));
            Assert.That(importer.mipmapEnabled, Is.False);
            Assert.That(importer.textureCompression, Is.EqualTo(TextureImporterCompression.Uncompressed));
            Assert.That(importer.alphaIsTransparency, Is.True);
        }
    }
}
