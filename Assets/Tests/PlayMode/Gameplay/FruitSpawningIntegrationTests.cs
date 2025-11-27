using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace NinjaFruit.Tests.PlayMode.Gameplay
{
    /// <summary>
    /// Play Mode integration tests for FruitSpawner component
    /// Story: STORY-001 - FruitSpawner MVP
    /// Test Plan: test-plan-story-001-fruitspawner.md
    /// Test Spec: test-spec-story-001-fruitspawner.md
    /// 
    /// These tests validate Unity runtime behavior: GameObject instantiation,
    /// Rigidbody2D physics, and component integration.
    /// </summary>
    [TestFixture]
    public class FruitSpawningIntegrationTests
    {
        private FruitSpawner spawner;
        private GameObject testPrefab;

        [SetUp]
        public void Setup()
        {
            // Load test fruit prefab from Resources
            testPrefab = Resources.Load<GameObject>("Prefabs/TestFruit");
            Assert.IsNotNull(testPrefab, "TestFruit prefab must exist in Resources/Prefabs/");

            // Create spawner GameObject
            GameObject spawnerObject = new GameObject("TestSpawner");
            spawner = spawnerObject.AddComponent<FruitSpawner>();

            // Configure spawner (implementation-dependent - adjust as needed)
            // Example: spawner.fruitPrefabs = new GameObject[] { testPrefab };
        }

        [TearDown]
        public void Teardown()
        {
            // Clean up spawned fruits
            GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
            foreach (var fruit in fruits)
            {
                Object.Destroy(fruit);
            }

            // Clean up spawner
            if (spawner != null && spawner.gameObject != null)
            {
                Object.Destroy(spawner.gameObject);
            }
        }

        /// <summary>
        /// TEST-011: SpawnFruit_CreatesGameObjectWithFruitTag
        /// Priority: P1
        /// Risk: RISK-004, RISK-006 (MEDIUM)
        /// 
        /// Validates that SpawnFruit() instantiates a GameObject with "Fruit" tag.
        /// This is the most basic integration test - if this fails, nothing else works.
        /// </summary>
        [UnityTest]
        public IEnumerator SpawnFruit_CreatesGameObjectWithFruitTag()
        {
            // Arrange
            string expectedTag = "Fruit";

            // Act
            spawner.SpawnFruit();
            yield return null; // Wait one frame for instantiation

            // Assert
            GameObject spawnedFruit = GameObject.FindWithTag(expectedTag);
            Assert.IsNotNull(
                spawnedFruit,
                "A GameObject with tag 'Fruit' should exist after spawning"
            );
            Assert.AreEqual(
                expectedTag,
                spawnedFruit.tag,
                "Spawned GameObject must have 'Fruit' tag"
            );
        }

        /// <summary>
        /// TEST-012: SpawnFruit_AttachesRigidbody2DComponent
        /// Priority: P1
        /// Risk: RISK-005 (MEDIUM)
        /// 
        /// Validates that spawned fruit has Rigidbody2D component attached
        /// with correct configuration (gravity scale, mass, etc.).
        /// </summary>
        [UnityTest]
        public IEnumerator SpawnFruit_AttachesRigidbody2DComponent()
        {
            // Arrange
            float expectedGravityScale = 1.0f;

            // Act
            spawner.SpawnFruit();
            yield return null; // Wait one frame

            // Assert
            GameObject spawnedFruit = GameObject.FindWithTag("Fruit");
            Assert.IsNotNull(spawnedFruit, "Fruit must be spawned");

            Rigidbody2D rb = spawnedFruit.GetComponent<Rigidbody2D>();
            Assert.IsNotNull(
                rb,
                "Spawned fruit must have Rigidbody2D component"
            );
            Assert.AreEqual(
                expectedGravityScale,
                rb.gravityScale,
                0.01f,
                "Rigidbody2D gravity scale should be configured"
            );
        }

        /// <summary>
        /// TEST-013: SpawnFruit_AppliesInitialVelocity
        /// Priority: P1
        /// Risk: RISK-002 (MEDIUM)
        /// 
        /// Validates that spawned fruit has upward velocity applied,
        /// matching the calculated speed from GDD formula.
        /// Formula at score 0: Min(7.0, 2.0 + (0 / 1000)) = 2.0 m/s
        /// </summary>
        [UnityTest]
        public IEnumerator SpawnFruit_AppliesInitialVelocity()
        {
            // Arrange
            int currentScore = 0;
            float expectedSpeed = spawner.CalculateFruitSpeed(currentScore);

            // Act
            spawner.SpawnFruit();
            yield return new WaitForFixedUpdate(); // Wait for physics update

            // Assert
            GameObject spawnedFruit = GameObject.FindWithTag("Fruit");
            Rigidbody2D rb = spawnedFruit.GetComponent<Rigidbody2D>();

            Assert.Greater(
                rb.velocity.y,
                0f,
                "Fruit should have upward velocity"
            );

            float actualSpeed = rb.velocity.magnitude;
            Assert.AreEqual(
                expectedSpeed,
                actualSpeed,
                0.5f, // Tolerance for physics initialization timing
                "Initial velocity magnitude should approximately match calculated speed"
            );
        }

        /// <summary>
        /// TEST-014: SpawnFruit_MultipleCalls_CreatesMultipleFruits
        /// Priority: P2
        /// Risk: RISK-004 (MEDIUM)
        /// 
        /// Validates that multiple calls to SpawnFruit() create independent
        /// fruit instances without singleton or caching issues.
        /// </summary>
        [UnityTest]
        public IEnumerator SpawnFruit_MultipleCalls_CreatesMultipleFruits()
        {
            // Arrange
            int numberOfSpawns = 3;
            int expectedFruitCount = 3;

            // Act
            spawner.SpawnFruit();
            yield return null;
            spawner.SpawnFruit();
            yield return null;
            spawner.SpawnFruit();
            yield return null;

            // Assert
            GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
            Assert.AreEqual(
                expectedFruitCount,
                fruits.Length,
                "Should spawn exactly 3 independent fruits"
            );

            // Verify each has independent Rigidbody2D
            foreach (var fruit in fruits)
            {
                Rigidbody2D rb = fruit.GetComponent<Rigidbody2D>();
                Assert.IsNotNull(
                    rb,
                    "Each fruit must have its own Rigidbody2D"
                );
            }
        }
    }
}
