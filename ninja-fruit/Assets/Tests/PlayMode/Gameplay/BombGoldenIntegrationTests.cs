using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NinjaFruit.Gameplay;

namespace NinjaFruit.Tests.PlayMode.Gameplay
{
    public class BombGoldenIntegrationTests
    {
        private GameObject root;
        private ScoreManager scoreManager;
        private CollisionManager collisionManager;

        [SetUp]
        public void SetUp()
        {
            root = new GameObject("BombPlayTestRoot");
            var sm = new GameObject("ScoreManager");
            sm.transform.SetParent(root.transform);
            scoreManager = sm.AddComponent<ScoreManager>();

            var cm = new GameObject("CollisionManager");
            cm.transform.SetParent(root.transform);
            collisionManager = cm.AddComponent<CollisionManager>();

            scoreManager.ResetForTests();
        }

        [TearDown]
        public void TearDown()
        {
            if (root != null) Object.Destroy(root);
            Physics2D.gravity = Vector2.zero;
        }

        private GameObject CreateBomb(Vector2 pos, float radius)
        {
            var bomb = new GameObject("Bomb");
            bomb.transform.SetParent(root.transform);
            bomb.transform.position = pos;
            var c = bomb.AddComponent<CircleCollider2D>();
            c.radius = radius;
            bomb.AddComponent<Bomb>();
            var rb = bomb.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            return bomb;
        }

        private GameObject CreateGoldenFruit(Vector2 pos, float radius)
        {
            var fruit = new GameObject("GoldenFruit");
            fruit.transform.SetParent(root.transform);
            fruit.transform.position = pos;
            var c = fruit.AddComponent<CircleCollider2D>();
            c.radius = radius;
            var rb = fruit.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            var f = fruit.AddComponent<Fruit>();
            f.IsGolden = true;
            return fruit;
        }

        [UnityTest]
        public IEnumerator TC_Integration_Bomb_Hit_TriggersPenalty()
        {
            var bomb = CreateBomb(new Vector2(5,5), 1.0f);
            yield return null;

            int before = scoreManager.CurrentScore;
            collisionManager.HandleSwipe(new Vector2(0,5), new Vector2(10,5), scoreManager);
            yield return null;

            Assert.AreEqual(before - 50, scoreManager.CurrentScore, "Bomb hit should apply penalty");
            yield return null;
        }

        [UnityTest]
        public IEnumerator TC_Integration_Bomb_Miss_NoPenalty()
        {
            var bomb = CreateBomb(new Vector2(50,50), 1.0f);
            yield return null;

            int before = scoreManager.CurrentScore;
            collisionManager.HandleSwipe(new Vector2(0,5), new Vector2(10,5), scoreManager);
            yield return null;

            Assert.AreEqual(before, scoreManager.CurrentScore, "Missing bomb should not trigger penalty");
            yield return null;
        }

        [UnityTest]
        public IEnumerator TC_Integration_GoldenFruit_Hit_BonusAwarded()
        {
            var g = CreateGoldenFruit(new Vector2(5,5), 1.0f);
            yield return null;

            collisionManager.HandleSwipe(new Vector2(0,5), new Vector2(10,5), scoreManager);
            yield return null;

            Assert.Greater(scoreManager.CurrentScore, 0, "Golden fruit hit should award points");
            yield return null;
        }

        [UnityTest]
        public IEnumerator TC_Integration_DestroyedBomb_HandleGracefully()
        {
            var bomb = CreateBomb(new Vector2(5,5), 1.0f);
            yield return null;

            Object.Destroy(bomb);
            yield return null;

            // Should not throw
            collisionManager.HandleSwipe(new Vector2(0,5), new Vector2(10,5), scoreManager);
            yield return null;

            Assert.Pass("Handled destroyed bomb without exception");
        }
    }
}
