using System;
using NUnit.Framework;
using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D.Tests
{
    public sealed class NaturalOcclusionGameplayContractTests
    {
        [Test]
        public void CompatibilityResolver_NeverHidesConfiguredRendererAcrossRiskFramesAndLifecycle()
        {
            var target = new GameObject("RatVisual");
            var occluderObject = new GameObject("ForegroundOccluder");
            var texture = new Texture2D(24, 8);
            var sprites = CreateThreeSprites(texture);

            try
            {
                var renderer = target.AddComponent<SpriteRenderer>();
                renderer.sprite = sprites[0];
                renderer.sortingOrder = 0;
                renderer.color = new Color(1f, 1f, 1f, 0.65f);
                var initialPosition = target.transform.position;

                var occluderRenderer = occluderObject.AddComponent<SpriteRenderer>();
                occluderRenderer.sprite = sprites[0];
                occluderRenderer.sortingOrder = 10;

                var resolver = target.AddComponent<VisualOcclusionResolver2D>();
                resolver.Configure(
                    renderer,
                    null,
                    CreateSplitRiskFrames(sprites),
                    new[]
                    {
                        new VisualOcclusionResolver2D.OccluderContract(
                            occluderRenderer,
                            null,
                            Rect.MinMaxRect(-0.20f, -0.50f, 0.20f, 0.50f))
                    },
                    4f / 128f,
                    2f / 128f);

                foreach (var sprite in sprites)
                {
                    renderer.sprite = sprite;
                    Assert.That(resolver.ResolveNow(), Is.False);
                    Assert.That(renderer.enabled, Is.True);
                    Assert.That(renderer.color.a, Is.EqualTo(0.65f).Within(0.000001f));
                    Assert.That(target.activeSelf, Is.True);
                    Assert.That(target.transform.position, Is.EqualTo(initialPosition));
                    Assert.That(resolver.IsWholeCharacterOccluded, Is.False);
                    Assert.That(resolver.VisibilityTransitionCount, Is.Zero);
                }

                resolver.enabled = false;
                Assert.That(renderer.enabled, Is.True);
                resolver.enabled = true;
                Assert.That(resolver.ResolveNow(), Is.False);
                Assert.That(renderer.enabled, Is.True);
                Assert.That(renderer.color.a, Is.EqualTo(0.65f).Within(0.000001f));
                Assert.That(resolver.VisibilityTransitionCount, Is.Zero);
            }
            finally
            {
                DestroySprites(sprites);
                UnityEngine.Object.DestroyImmediate(texture);
                UnityEngine.Object.DestroyImmediate(occluderObject);
                UnityEngine.Object.DestroyImmediate(target);
            }
        }

        [Test]
        public void CompatibilityResolver_PreservesExternallyDisabledRenderer()
        {
            var target = new GameObject("ExternallyDisabledRatVisual");
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
                renderer.color = new Color(1f, 1f, 1f, 0.4f);
                var resolver = target.AddComponent<VisualOcclusionResolver2D>();
                resolver.Configure(
                    renderer,
                    null,
                    new[]
                    {
                        new VisualOcclusionResolver2D.FrameAlphaContract(
                            sprite,
                            Rect.MinMaxRect(-1f, -1f, 1f, 1f),
                            Rect.MinMaxRect(-0.5f, -0.5f, 0.5f, 0.5f))
                    },
                    Array.Empty<VisualOcclusionResolver2D.OccluderContract>(),
                    4f / 128f,
                    2f / 128f);

                resolver.enabled = false;
                resolver.enabled = true;
                Assert.That(resolver.ResolveNow(), Is.False);
                Assert.That(renderer.enabled, Is.False);
                Assert.That(renderer.color.a, Is.EqualTo(0.4f).Within(0.000001f));
                Assert.That(resolver.IsWholeCharacterOccluded, Is.False);
                Assert.That(resolver.VisibilityTransitionCount, Is.Zero);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(sprite);
                UnityEngine.Object.DestroyImmediate(texture);
                UnityEngine.Object.DestroyImmediate(target);
            }
        }

        [Test]
        public void SideThreeFrameView_EnforcesStableMeasuredCapsuleAcrossFramesAndMirror()
        {
            var root = new GameObject("RatHost2D");
            var visual = new GameObject("Visual");
            var texture = new Texture2D(24, 8);
            var sprites = CreateThreeSprites(texture);

            try
            {
                visual.transform.SetParent(root.transform, false);
                visual.transform.localPosition = new Vector3(0.25f, 0.5f, 0f);
                var renderer = visual.AddComponent<SpriteRenderer>();
                var view = visual.AddComponent<RatSide3FrameView>();
                var collider = root.AddComponent<CapsuleCollider2D>();
                root.transform.position = new Vector3(2.25f, -1.5f, 0f);
                var initialRoot = root.transform.position;
                var initialVisualLocal = visual.transform.localPosition;

                view.Configure(null, renderer, sprites, 10f);
                view.ConfigureBodyClearance(
                    collider,
                    new Vector2(99f, 99f),
                    new Vector2(42f, 42f));

                var deltas = new[] { 0.11f, 0.10f, 0.10f };
                for (var index = 0; index < deltas.Length; index++)
                {
                    view.ApplyView(Vector2.right, true, deltas[index]);
                    AssertMeasuredCollider(collider, true);
                    Assert.That(root.transform.position, Is.EqualTo(initialRoot));
                    Assert.That(visual.transform.localPosition, Is.EqualTo(initialVisualLocal));
                }

                view.ApplyView(Vector2.left, true, 0.11f);
                Assert.That(renderer.flipX, Is.True);
                AssertMeasuredCollider(collider, false);
                Assert.That(root.transform.position, Is.EqualTo(initialRoot));
                Assert.That(visual.transform.localPosition, Is.EqualTo(initialVisualLocal));
            }
            finally
            {
                DestroySprites(sprites);
                UnityEngine.Object.DestroyImmediate(texture);
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static VisualOcclusionResolver2D.FrameAlphaContract[] CreateSplitRiskFrames(
            Sprite[] sprites)
        {
            var contracts = new VisualOcclusionResolver2D.FrameAlphaContract[sprites.Length];
            for (var index = 0; index < sprites.Length; index++)
            {
                contracts[index] = new VisualOcclusionResolver2D.FrameAlphaContract(
                    sprites[index],
                    Rect.MinMaxRect(-1f, -0.5f, 1f, 0.5f),
                    Rect.MinMaxRect(-0.6f, -0.4f, 0.6f, 0.4f));
            }

            return contracts;
        }

        private static Sprite[] CreateThreeSprites(Texture2D texture)
        {
            var sprites = new Sprite[3];
            for (var index = 0; index < sprites.Length; index++)
            {
                sprites[index] = Sprite.Create(
                    texture,
                    new Rect(index * 8f, 0f, 8f, 8f),
                    new Vector2(0.5f, 0.25f),
                    128f);
            }

            return sprites;
        }

        private static void AssertMeasuredCollider(
            CapsuleCollider2D collider,
            bool facesRight)
        {
            Assert.That(collider.direction, Is.EqualTo(CapsuleDirection2D.Horizontal));
            Assert.That(
                collider.size,
                Is.EqualTo(new Vector2(1.2265625f, 0.25f)));
            Assert.That(
                collider.offset,
                Is.EqualTo(new Vector2(
                    facesRight ? 0.28515625f : -0.28515625f,
                    0.125f)));
        }

        private static void DestroySprites(Sprite[] sprites)
        {
            foreach (var sprite in sprites)
            {
                if (sprite != null)
                {
                    UnityEngine.Object.DestroyImmediate(sprite);
                }
            }
        }
    }
}
