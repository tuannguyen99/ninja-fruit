using NUnit.Framework;
using UnityEngine;
using NinjaFruit.Gameplay;

namespace NinjaFruit.Tests.EditMode.Gameplay
{
    [TestFixture]
    public class BombGoldenLogicTests
    {
        private ScoreManager scoreManager;

        [SetUp]
        public void SetUp()
        {
            var go = new GameObject("ScoreManagerTest");
            scoreManager = go.AddComponent<ScoreManager>();
            scoreManager.ResetForTests();
        }

        [TearDown]
        public void TearDown()
        {
            if (scoreManager != null && scoreManager.gameObject != null)
                Object.DestroyImmediate(scoreManager.gameObject);
        }

        [Test]
        public void TC_Unit_Bomb_PenaltyIsApplied()
        {
            scoreManager.ResetForTests();
            int before = scoreManager.CurrentScore;
            scoreManager.RegisterBombHit();
            Assert.AreEqual(before - 50, scoreManager.CurrentScore, "Bomb should deduct 50 points");
            Assert.AreEqual(1, scoreManager.ComboMultiplier, "Combo should reset to 1 after bomb");
        }

        [Test]
        public void TC_Unit_GoldenFruit_BonusApplied()
        {
            scoreManager.ResetForTests();
            int pts = scoreManager.RegisterSlice(FruitType.Apple, true, 0f);
            // Apple base 10, golden doubles
            Assert.AreEqual(20, pts, "Golden apple should award double points");
        }

        [Test]
        public void TC_Unit_Bomb_NotDetectedWhenOutsidePath()
        {
            // Use CollisionManager.DoesSwipeIntersectFruit to ensure bomb outside path is ignored
            var go = new GameObject("CMTest");
            var cm = go.AddComponent<CollisionManager>();

            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(1, 0);
            Vector2 bombPos = new Vector2(5, 5);
            float radius = 1.0f;

            bool hit = cm.DoesSwipeIntersectFruit(start, end, bombPos, radius);
            Assert.IsFalse(hit, "Bomb outside swipe path should not be detected");

            Object.DestroyImmediate(go);
        }

        [Test]
        public void TC_Unit_DestroyedFruit_DoesNotThrow()
        {
            // Ensure GetFruitsInSwipePath handles destroyed objects gracefully
            var root = new GameObject("TestRoot");
            var fruit = new GameObject("TempFruit");
            fruit.transform.SetParent(root.transform);
            var collider = fruit.AddComponent<CircleCollider2D>();
            collider.radius = 1.0f;

            var cmObj = new GameObject("CM");
            var cm = cmObj.AddComponent<CollisionManager>();

            Object.DestroyImmediate(fruit);

            // Should not throw
            Assert.DoesNotThrow(() => cm.GetFruitsInSwipePath(new Vector2(0,0), new Vector2(10,0)));

            Object.DestroyImmediate(root);
            Object.DestroyImmediate(cmObj);
        }
    }
}
