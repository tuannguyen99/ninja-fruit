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
        private NinjaFruit.FruitSpawner spawner;

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
            // Wire swipe events to collision manager runtime handler
            swipeDetector.OnSwipeDetected += (s, e) => collisionManager.HandleSwipe(s, e, scoreManager);

            // Create a spawner to use prefabs where available
            GameObject spObj = new GameObject("Spawner");
            spObj.transform.SetParent(testRoot.transform);
            spawner = spObj.AddComponent<NinjaFruit.FruitSpawner>();

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

        private GameObject CreateTestFruit(Vector2 pos, float radius, string name, NinjaFruit.Gameplay.FruitType type = NinjaFruit.Gameplay.FruitType.Apple, bool isGolden = false)
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
            var fc = fruit.AddComponent<NinjaFruit.Gameplay.Fruit>();
            fc.Type = type;
            fc.IsGolden = isGolden;
            return fruit;
        }

        [UnityTest]
        [Category("Integration")]
        public IEnumerator MultiFruitSwipe_AppliesComboToAllSlices()
        {
            // Arrange - spawn three fruits in line using spawner when available
            // If Resources prefab is available spawner.SpawnFruit will instantiate it
            // For tests we fall back to CreateTestFruit which adds Fruit component
            GameObject a = CreateTestFruit(new Vector2(2, 5), 1.0f, "FruitA", NinjaFruit.Gameplay.FruitType.Apple, false);
            GameObject b = CreateTestFruit(new Vector2(5, 5), 1.0f, "FruitB", NinjaFruit.Gameplay.FruitType.Apple, false);
            GameObject c = CreateTestFruit(new Vector2(8, 5), 1.0f, "FruitC", NinjaFruit.Gameplay.FruitType.Apple, false);

            yield return WaitFrames(1);

            // Subscribe CollisionManager to SwipeDetector (as in game boot)

            yield return WaitFrames(1);

            // Act - prepare swipe across all fruits (detect them), then simulate the game slicing flow
            Vector2 swipeStart = new Vector2(0, 5);
            Vector2 swipeEnd = new Vector2(10, 5);

            // Subscribe to ScoreManager events to capture combo and score changes
            var comboEvents = new System.Collections.Generic.List<int>();
            var scoreEvents = new System.Collections.Generic.List<int>();
            scoreManager.OnComboChanged += (c) => comboEvents.Add(c);
            scoreManager.OnScoreChanged += (s) => scoreEvents.Add(s);

            // Act - trigger the swipe event; CollisionManager.HandleSwipe will detect fruits and call ScoreManager
            swipeDetector.TriggerSwipeEvent(swipeStart, swipeEnd);

            // Allow a couple frames for processing and destruction
            yield return WaitFrames(2);

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

        [UnityTest]
        [Category("Integration")]
        public IEnumerator GoldenFruit_Sliced_DoublesBasePoints()
        {
            // Arrange - spawn one golden apple
            GameObject golden = CreateTestFruit(new Vector2(5, 5), 1.0f, "AppleGolden", NinjaFruit.Gameplay.FruitType.Apple, true);

            yield return WaitFrames(1);

            Vector2 swipeStart = new Vector2(2, 5);
            Vector2 swipeEnd = new Vector2(8, 5);

            var detected = collisionManager.GetFruitsInSwipePath(swipeStart, swipeEnd);
            Assert.AreEqual(1, detected.Count, "Should detect the golden fruit");

            // Act - trigger swipe; collision manager will call RegisterSlice with IsGolden = true
            swipeDetector.TriggerSwipeEvent(swipeStart, swipeEnd);
            yield return WaitFrames(2);

            // Base apple points = 10, golden doubles to 20
            Assert.AreEqual(20, scoreManager.CurrentScore);

            yield return null;
        }

        [UnityTest]
        [Category("Integration")]
        public IEnumerator Bomb_Sliced_AppliesPenaltyAndResetsCombo()
        {
            // Arrange - spawn a bomb
            // Spawn a bomb via spawner (resource prefab) or programmatic fallback
            spawner.SpawnBomb();

            // Move spawned bomb to test position so swipe will intersect
            var spawnedBomb = Object.FindObjectOfType<NinjaFruit.Gameplay.Bomb>();
            Assert.IsNotNull(spawnedBomb, "Spawner failed to create a Bomb");
            spawnedBomb.transform.position = new Vector2(5, 5);

            // Set an initial score and an active combo
            scoreManager.AddPoints(100);
            // Simulate a couple slices to set combo > 1, but neutralize their point effects
            int pts1 = scoreManager.RegisterSlice(NinjaFruit.Gameplay.FruitType.Apple, false, Time.time);
            int pts2 = scoreManager.RegisterSlice(NinjaFruit.Gameplay.FruitType.Apple, false, Time.time + 0.1f);
            Assert.Greater(scoreManager.ComboMultiplier, 1);
            // Remove the slice points so the base score remains 100 for the bomb penalty check
            scoreManager.AddPoints(-pts1 - pts2);

            yield return WaitFrames(1);

            // Act - swipe through the bomb
            Vector2 swipeStart = new Vector2(2, 5);
            Vector2 swipeEnd = new Vector2(8, 5);
            swipeDetector.TriggerSwipeEvent(swipeStart, swipeEnd);

            yield return WaitFrames(2);

            // Assert - penalty applied and combo reset
            Assert.AreEqual(50, scoreManager.CurrentScore, "Bomb should deduct 50 points");
            Assert.AreEqual(1, scoreManager.ComboMultiplier, "Bomb should reset combo to 1");

            yield return null;
        }
    }
}
