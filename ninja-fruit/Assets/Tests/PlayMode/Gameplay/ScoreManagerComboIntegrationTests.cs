using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NinjaFruit.Gameplay;
using NinjaFruit;

namespace NinjaFruit.Tests.PlayMode.Gameplay
{
    [TestFixture]
    public class ScoreManagerComboIntegrationTests
    {
        private GameObject testRoot;
        private CollisionManager collisionManager;
        private SwipeDetector swipeDetector;
        private ScoreManager scoreManager;

        [SetUp]
        public void Setup()
        {
            testRoot = new GameObject("ComboIntegrationRoot");

            GameObject cmObj = new GameObject("CollisionManager");
            cmObj.transform.SetParent(testRoot.transform);
            collisionManager = cmObj.AddComponent<CollisionManager>();

            GameObject sdObj = new GameObject("SwipeDetector");
            sdObj.transform.SetParent(testRoot.transform);
            swipeDetector = sdObj.AddComponent<SwipeDetector>();

            GameObject smObj = new GameObject("ScoreManager");
            smObj.transform.SetParent(testRoot.transform);
            scoreManager = smObj.AddComponent<ScoreManager>();
            scoreManager.ResetForTests();
        }

        [TearDown]
        public void Teardown()
        {
            if (testRoot != null) Object.Destroy(testRoot);
            PlayerPrefs.DeleteAll();
        }

        private IEnumerator WaitFrames(int frames = 1)
        {
            for (int i = 0; i < frames; i++) yield return null;
        }

        private GameObject CreateTestFruit(Vector2 pos, float radius, string name)
        {
            GameObject fruit = new GameObject(name);
            fruit.transform.SetParent(testRoot.transform);
            fruit.transform.position = pos;
            CircleCollider2D c = fruit.AddComponent<CircleCollider2D>();
            c.radius = radius;
            c.isTrigger = false;
            Rigidbody2D rb = fruit.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0;
            return fruit;
        }

        [UnityTest]
        [Category("Integration")]
        public IEnumerator MultiFruitSwipe_AppliesComboToAllSlices()
        {
            // Arrange - three fruits in line
            GameObject a = CreateTestFruit(new Vector2(2, 5), 1.0f, "FruitA");
            GameObject b = CreateTestFruit(new Vector2(5, 5), 1.0f, "FruitB");
            GameObject c = CreateTestFruit(new Vector2(8, 5), 1.0f, "FruitC");

            yield return WaitFrames(1);

            // Subscribe CollisionManager to SwipeDetector (as in game boot)

            yield return WaitFrames(1);

            // Act - trigger swipe across all fruits (detect them), then simulate the game slicing flow
            Vector2 swipeStart = new Vector2(0, 5);
            Vector2 swipeEnd = new Vector2(10, 5);
            swipeDetector.TriggerSwipeEvent(swipeStart, swipeEnd);

            yield return WaitFrames(1);

            var detected = collisionManager.GetFruitsInSwipePath(swipeStart, swipeEnd);

            // Subscribe to ScoreManager events to capture combo and score changes
            var comboEvents = new System.Collections.Generic.List<int>();
            var scoreEvents = new System.Collections.Generic.List<int>();
            scoreManager.OnComboChanged += (c) => comboEvents.Add(c);
            scoreManager.OnScoreChanged += (s) => scoreEvents.Add(s);

            // Simulate CollisionManager/Gameplay calling RegisterSlice for each detected fruit
            float t0 = Time.time;
            for (int i = 0; i < detected.Count; i++)
            {
                // Use small delta times so they are within combo window
                scoreManager.RegisterSlice(NinjaFruit.Gameplay.FruitType.Apple, false, t0 + i * 0.1f);
            }

            // We expect three slices, with combo multipliers 1,2,3 applied to base points.
            // Base points: Apple(10)
            Assert.That(scoreManager.ComboMultiplier == 3, "Combo should reach 3 after slicing 3 fruits in quick succession");

            // Verify current score is sum of points with multipliers: 10*1 + 10*2 + 10*3 = 60
            Assert.AreEqual(60, scoreManager.CurrentScore, "Expected total points with combo multipliers applied");

            // Verify events fired with expected sequences
            CollectionAssert.AreEqual(new int[] { 1, 2, 3 }, comboEvents, "OnComboChanged should fire with 1,2,3");
            CollectionAssert.AreEqual(new int[] { 10, 30, 60 }, scoreEvents, "OnScoreChanged should fire with cumulative scores");

            yield return null;
        }
    }
}
