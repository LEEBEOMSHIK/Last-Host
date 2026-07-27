using System;
using NUnit.Framework;
using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D.Tests
{
    public sealed class VisualAndGrid2DTests
    {
        [Test]
        public void PixelGridUsesOneSixtyFourthWorldUnit()
        {
            var snapped = PixelGrid2D.Snap(new Vector2(0.02f, -0.02f), 64);

            Assert.That(snapped.x, Is.EqualTo(1f / 64f).Within(0.000001f));
            Assert.That(snapped.y, Is.EqualTo(-1f / 64f).Within(0.000001f));
        }

        [TestCase(0)]
        [TestCase(-64)]
        public void WrongPixelsPerUnitIsRejected(int invalidPixelsPerUnit)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => PixelGrid2D.Snap(1f, invalidPixelsPerUnit));
        }

        [Test]
        public void VisualAbsoluteSnapDoesNotAccumulateOffset()
        {
            var root = new GameObject("LogicalRoot");
            var visual = new GameObject("Visual");

            try
            {
                root.transform.position = new Vector3(0.022f, -0.027f, 0f);
                visual.transform.position = new Vector3(100f, -100f, 2f);
                var snap = visual.AddComponent<VisualPixelSnap2D>();
                snap.Configure(root.transform, 64);

                for (var index = 0; index < 300; index++)
                {
                    snap.ApplySnap();
                }

                var expected = PixelGrid2D.Snap((Vector2)root.transform.position, 64);
                Assert.That((Vector2)visual.transform.position, Is.EqualTo(expected));
                Assert.That(visual.transform.position.z, Is.EqualTo(2f));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(visual);
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void DirectionViewNeverMovesPhysicsRootAndIdleUsesFrameA()
        {
            var root = new GameObject("RatHost2D");
            var visual = new GameObject("Visual");
            var texture = new Texture2D(16, 16);
            var frames = new Sprite[16];

            try
            {
                visual.transform.SetParent(root.transform, false);
                var renderer = visual.AddComponent<SpriteRenderer>();
                var view = visual.AddComponent<RatHost2DView>();

                for (var index = 0; index < frames.Length; index++)
                {
                    frames[index] = Sprite.Create(
                        texture,
                        new Rect(0f, 0f, 16f, 16f),
                        new Vector2(0.5f, 0.5f),
                        64f);
                }

                view.Configure(null, renderer, frames);
                root.transform.position = new Vector3(3.125f, -8.25f, 0f);
                var originalRootPosition = root.transform.position;

                view.UpdateView(Direction8.East, true, 0.2f);
                view.UpdateView(Direction8.NorthWest, false, 0.2f);

                Assert.That(root.transform.position, Is.EqualTo(originalRootPosition));
                Assert.That(renderer.sprite, Is.SameAs(frames[((int)Direction8.NorthWest) * 2]));
            }
            finally
            {
                foreach (var frame in frames)
                {
                    if (frame != null)
                    {
                        UnityEngine.Object.DestroyImmediate(frame);
                    }
                }

                UnityEngine.Object.DestroyImmediate(texture);
                UnityEngine.Object.DestroyImmediate(root);
            }
        }
    }
}
