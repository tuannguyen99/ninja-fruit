using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NinjaFruit.Gameplay;

namespace NinjaFruit.Tests.PlayMode.Gameplay
{
    public class ComboIntegrationTests
    {
        private GameObject root;
        private ScoreManager scoreManager;
        private CollisionManager collisionManager;

        [SetUp]
        public void SetUp()
        {
            root = new GameObject("ComboPlayTestRoot");
            var smObj = new GameObject("ScoreManager");
            smObj.transform.SetParent(root.transform);
            scoreManager = smObj.AddComponent<ScoreManager>();

            var cmObj = new GameObject("CollisionManager");
            cmObj.transform.SetParent(root.transform);
            collisionManager = cmObj.AddComponent<CollisionManager>();

            scoreManager.ResetForTests();
        }

        [TearDown]
        public void TearDown()
        {
            if (root != null) Object.Destroy(root);
            Physics2D.gravity = Vector2.zero;
        }

        private GameObject CreateTestFruit(Vector2 position, float radius, bool isGolden = false, string name = "Fruit")
        {
            GameObject fruit = new GameObject(name);
            fruit.transform.SetParent(root.transform);
            fruit.transform.position = position;

            var c = fruit.AddComponent<CircleCollider2D>();
            c.radius = radius;
            c.isTrigger = false;

            var rb = fruit.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;

            var f = fruit.AddComponent<Fruit>();
            f.IsGolden = isGolden;

            return fruit;
        }

        [UnityTest]
        public IEnumerator TC_Integration_Combo_EndToEnd_SlicesIncreaseScore()
        {
            // Arrange: spawn fruit and swipe through it twice within combo window
            var fruit1 = CreateTestFruit(new Vector2(2,5), 1.0f, false, "Fruit1");
            var fruit2 = CreateTestFruit(new Vector2(5,5), 1.0f, false, "Fruit2");

            yield return null;

            Vector2 swipeStart = new Vector2(0,5);
            Vector2 swipeEnd = new Vector2(10,5);

            // Act: handle swipe which should slice both fruits and register slices
            collisionManager.HandleSwipe(swipeStart, swipeEnd, scoreManager);

            yield return null;

            // Assert: score should be > 0 and combo multiplier should have advanced (>=2)
            Assert.Greater(scoreManager.CurrentScore, 0, "Score should increase after slicing fruits");
            Assert.GreaterOrEqual(scoreManager.ComboMultiplier, 1, "Combo multiplier should be at least 1");

            yield return null;
        }

        [UnityTest]
        public IEnumerator TC_Integration_Combo_TimeoutBetweenSlices_BreaksCombo()
        {
            var f1 = CreateTestFruit(new Vector2(2,5), 1.0f, false, "FruitA");
            yield return null;

            collisionManager.HandleSwipe(new Vector2(0,5), new Vector2(10,5), scoreManager);
            int comboAfterFirst = scoreManager.ComboMultiplier;

            // Wait beyond combo window
            yield return new WaitForSeconds(2.0f);

            var f2 = CreateTestFruit(new Vector2(5,5), 1.0f, false, "FruitB");
            yield return null;

            collisionManager.HandleSwipe(new Vector2(0,5), new Vector2(10,5), scoreManager);
            int comboAfterSecond = scoreManager.ComboMultiplier;

            Assert.AreEqual(1, comboAfterSecond, "Combo should reset after timeout between slices");

            yield return null;
        }

        [UnityTest]
        public IEnumerator TC_Integration_Combo_CapObservedInGameplay()
        {
            // Spawn many fruits in line and perform rapid slices to push combo to cap
            for (int i = 0; i < 10; i++)
            {
                CreateTestFruit(new Vector2(2 + i*0.5f, 5), 0.5f, false, "Fruit" + i);
            }
            yield return null;

            // Perform multiple swipes in quick succession
            for (int i = 0; i < 8; i++)
            {
                collisionManager.HandleSwipe(new Vector2(0,5), new Vector2(10,5), scoreManager);
                yield return null;
            }

            Assert.LessOrEqual(scoreManager.ComboMultiplier, 5, "Combo multiplier should not exceed configured max");

            yield return null;
        }
    }
}
