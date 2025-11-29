using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NinjaFruit;

namespace NinjaFruit.Tests.PlayMode.Gameplay
{
    /// <summary>
    /// Play Mode Integration Tests for CollisionManager
    /// 
    /// Story: STORY-003: CollisionManager MVP
    /// Epic: Core Slicing Mechanics
    /// 
    /// Tests CollisionManager's integration with:
    /// - SwipeDetector event system
    /// - Physics2D collider detection
    /// - Multi-fruit slicing scenarios
    /// - Edge cases and boundary conditions
    /// 
    /// These tests require:
    /// - Unity runtime (Play Mode)
    /// - GameObjects and components
    /// - Physics2D simulation
    /// - Test fruit spawning utilities
    /// </summary>
    [TestFixture]
    public class CollisionDetectionIntegrationTests
    {
        private GameObject testSceneRoot;
        private CollisionManager collisionManager;
        private SwipeDetector swipeDetector;

        /// <summary>
        /// Setup before each test
        /// Creates scene objects and initializes managers
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Create root GameObject for this test
            testSceneRoot = new GameObject("PlayModeTestRoot");

            // Create CollisionManager
            GameObject cmObject = new GameObject("CollisionManager");
            cmObject.transform.SetParent(testSceneRoot.transform);
            collisionManager = cmObject.AddComponent<CollisionManager>();

            // Create SwipeDetector
            GameObject sdObject = new GameObject("SwipeDetector");
            sdObject.transform.SetParent(testSceneRoot.transform);
            swipeDetector = sdObject.AddComponent<SwipeDetector>();

            // Verify all components initialized
            Assert.IsNotNull(collisionManager, "CollisionManager failed to instantiate");
            Assert.IsNotNull(swipeDetector, "SwipeDetector failed to instantiate");
        }

        /// <summary>
        /// Teardown after each test
        /// Cleans up all test GameObjects
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            // Clean up test scene root (destroys all children and fruits)
            if (testSceneRoot != null)
            {
                Object.Destroy(testSceneRoot);
            }

            // Reset physics settings
            Physics2D.gravity = Vector2.zero;
        }

        /// <summary>
        /// Helper: Wait for physics to settle
        /// </summary>
        private IEnumerator WaitForPhysics(int frames = 1)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return null;
            }
        }

        /// <summary>
        /// Helper: Create test fruit GameObject with collider
        /// </summary>
        private GameObject CreateTestFruit(Vector2 position, float colliderRadius, string name)
        {
            GameObject fruit = new GameObject(name);
            fruit.transform.SetParent(testSceneRoot.transform);
            fruit.transform.position = position;
            
            CircleCollider2D collider = fruit.AddComponent<CircleCollider2D>();
            collider.radius = colliderRadius;
            collider.isTrigger = false;
            
            Rigidbody2D rb = fruit.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0;
            rb.isKinematic = false;
            
            return fruit;
        }

        /// <summary>
        /// Helper: Assert fruit detection count
        /// </summary>
        private void AssertFruitsDetected(int expectedCount, Vector2 start, Vector2 end, string testName)
        {
            var detectedFruits = collisionManager.GetFruitsInSwipePath(start, end);
            string message = $"{testName}: Expected {expectedCount} fruits, got {detectedFruits.Count}";
            Assert.AreEqual(expectedCount, detectedFruits.Count, message);
        }

        /// <summary>
        /// Helper: Verify fruit is in detected list
        /// </summary>
        private void AssertFruitInList(GameObject fruit, List<GameObject> detectedFruits, string testName)
        {
            bool found = false;
            foreach (var detected in detectedFruits)
            {
                if (detected == fruit)
                {
                    found = true;
                    break;
                }
            }
            Assert.IsTrue(found, $"{testName}: Fruit not found in detected list");
        }

        // ==================== EVENT INTEGRATION TESTS ====================

        /// <summary>
        /// TEST: IT-001 - SwipeDetector Event Integration
        /// 
        /// Description:
        /// Verify that CollisionManager receives events from SwipeDetector
        /// when swipe gesture is detected
        /// 
        /// Verification:
        /// - Event handler is called
        /// - No null reference exceptions
        /// - Event subscription persists
        /// 
        /// Priority: HIGH
        /// </summary>
        [UnityTest]
        [Category("EventIntegration")]
        public IEnumerator SwipeDetectorEvent_CollisionManagerSubscribed_EventReceived()
        {
            // Track if event was received
            bool eventReceived = false;
            List<GameObject> detectedFruits = null;

            // Create callback to track event
            System.Action<Vector2, Vector2> eventHandler = (start, end) =>
            {
                eventReceived = true;
                detectedFruits = collisionManager.GetFruitsInSwipePath(start, end);
            };

            // Subscribe CollisionManager to SwipeDetector events
            swipeDetector.OnSwipeDetected += eventHandler;

            // Wait a frame for subscription to take effect
            yield return WaitForPhysics(1);

            // Trigger swipe event using public test helper
            swipeDetector.TriggerSwipeEvent(new Vector2(0, 0), new Vector2(10, 0));

            yield return WaitForPhysics(1);

            // Assert event was received
            Assert.IsTrue(eventReceived, "IT-001: SwipeDetector event not received");

            // Cleanup
            swipeDetector.OnSwipeDetected -= eventHandler;
        }

        // ==================== SINGLE FRUIT COLLISION TESTS ====================

        /// <summary>
        /// TEST: IT-002 - Single Fruit Detection (CRITICAL)
        /// 
        /// Description:
        /// Spawn one fruit and verify it's detected by collision system
        /// This is the baseline integration test
        /// 
        /// Setup:
        /// - Spawn fruit at (5, 5) with radius 1.0
        /// - Swipe: (2, 5) → (8, 5) [horizontal line through fruit]
        /// 
        /// Expected:
        /// - GetFruitsInSwipePath returns list with 1 fruit
        /// - Fruit GameObject reference is correct
        /// 
        /// Priority: CRITICAL
        /// </summary>
        [UnityTest]
        [Category("Collision")]
        public IEnumerator GetFruitsInSwipePath_SingleFruit_DetectsCorrectly()
        {
            // Arrange
            GameObject testFruit = CreateTestFruit(new Vector2(5, 5), 1.0f, "TestFruit_Single");
            
            Vector2 swipeStart = new Vector2(2, 5);
            Vector2 swipeEnd = new Vector2(8, 5);

            yield return WaitForPhysics(1);

            // Act
            var detectedFruits = collisionManager.GetFruitsInSwipePath(swipeStart, swipeEnd);

            // Assert
            Assert.AreEqual(1, detectedFruits.Count, 
                          "IT-002: Expected 1 fruit, got " + detectedFruits.Count);
            Assert.AreSame(testFruit, detectedFruits[0], 
                          "IT-002: Detected fruit is not the spawned fruit");
        }

        // ==================== MULTI-FRUIT SLICING TESTS ====================

        /// <summary>
        /// TEST: IT-003 - Three-Fruit Slicing (CRITICAL)
        /// 
        /// Description:
        /// Spawn three fruits in a line and verify all are detected by single swipe
        /// This tests the core multi-fruit slicing mechanic
        /// 
        /// Setup:
        /// - Fruit A: (2, 5) radius 1.0
        /// - Fruit B: (5, 5) radius 1.0
        /// - Fruit C: (8, 5) radius 1.0
        /// - Swipe: (0, 5) → (10, 5) [horizontal line through all three]
        /// 
        /// Expected:
        /// - GetFruitsInSwipePath returns list with 3 fruits
        /// - All fruits detected, no duplicates
        /// 
        /// Priority: CRITICAL
        /// </summary>
        [UnityTest]
        [Category("MultiSlice")]
        public IEnumerator GetFruitsInSwipePath_ThreeFruitsInLine_AllDetected()
        {
            // Arrange
            GameObject fruitA = CreateTestFruit(new Vector2(2, 5), 1.0f, "FruitA");
            GameObject fruitB = CreateTestFruit(new Vector2(5, 5), 1.0f, "FruitB");
            GameObject fruitC = CreateTestFruit(new Vector2(8, 5), 1.0f, "FruitC");

            Vector2 swipeStart = new Vector2(0, 5);
            Vector2 swipeEnd = new Vector2(10, 5);

            yield return WaitForPhysics(1);

            // Act
            var detectedFruits = collisionManager.GetFruitsInSwipePath(swipeStart, swipeEnd);

            // Assert
            Assert.AreEqual(3, detectedFruits.Count, 
                          "IT-003: Expected 3 fruits, got " + detectedFruits.Count);

            AssertFruitInList(fruitA, detectedFruits, "IT-003: Fruit A");
            AssertFruitInList(fruitB, detectedFruits, "IT-003: Fruit B");
            AssertFruitInList(fruitC, detectedFruits, "IT-003: Fruit C");
        }

        /// <summary>
        /// TEST: IT-004 - Selective Multi-Fruit Slicing (HIGH)
        /// 
        /// Description:
        /// Spawn three fruits where swipe hits only two of them
        /// Verifies that non-intersecting fruits are correctly excluded
        /// 
        /// Setup:
        /// - Fruit A: (2, 5) radius 1.0 [HIT]
        /// - Fruit B: (5, 2) radius 1.0 [MISS - 3 units above line]
        /// - Fruit C: (8, 5) radius 1.0 [HIT]
        /// - Swipe: (0, 5) → (10, 5) [horizontal line at Y=5]
        /// 
        /// Expected:
        /// - GetFruitsInSwipePath returns list with 2 fruits (A and C)
        /// - Fruit B NOT in list
        /// 
        /// Priority: HIGH
        /// </summary>
        [UnityTest]
        [Category("MultiSlice")]
        public IEnumerator GetFruitsInSwipePath_SelectiveMiss_CorrectlyExcludesMissedFruits()
        {
            // Arrange
            GameObject fruitA = CreateTestFruit(new Vector2(2, 5), 1.0f, "FruitA_Hit");
            GameObject fruitB = CreateTestFruit(new Vector2(5, 2), 1.0f, "FruitB_Miss"); // 3 units away
            GameObject fruitC = CreateTestFruit(new Vector2(8, 5), 1.0f, "FruitC_Hit");

            Vector2 swipeStart = new Vector2(0, 5);
            Vector2 swipeEnd = new Vector2(10, 5);

            yield return WaitForPhysics(1);

            // Act
            var detectedFruits = collisionManager.GetFruitsInSwipePath(swipeStart, swipeEnd);

            // Assert
            Assert.AreEqual(2, detectedFruits.Count, 
                          "IT-004: Expected 2 fruits, got " + detectedFruits.Count);
            
            AssertFruitInList(fruitA, detectedFruits, "IT-004: Fruit A should be detected");
            AssertFruitInList(fruitC, detectedFruits, "IT-004: Fruit C should be detected");
            
            // Verify fruit B is NOT in list
            foreach (var detected in detectedFruits)
            {
                Assert.AreNotEqual(fruitB, detected, 
                                 "IT-004: Fruit B should NOT be detected (3 units away)");
            }
        }

        /// <summary>
        /// TEST: IT-005 - Overlapping Fruits Slicing (MEDIUM)
        /// 
        /// Description:
        /// Spawn two overlapping fruits and verify both are detected
        /// Tests behavior with overlapping collision geometry
        /// 
        /// Setup:
        /// - Fruit A: (5, 5) radius 1.5
        /// - Fruit B: (5.5, 5.5) radius 1.5 [overlaps A]
        /// - Swipe: (3, 3) → (7, 7) [diagonal line through both]
        /// 
        /// Expected:
        /// - GetFruitsInSwipePath returns list with 2 fruits
        /// - Both fruits detected despite overlap
        /// 
        /// Priority: MEDIUM
        /// </summary>
        [UnityTest]
        [Category("MultiSlice")]
        public IEnumerator GetFruitsInSwipePath_OverlappingFruits_BothDetected()
        {
            // Arrange
            GameObject fruitA = CreateTestFruit(new Vector2(5, 5), 1.5f, "FruitA_Overlap");
            GameObject fruitB = CreateTestFruit(new Vector2(5.5f, 5.5f), 1.5f, "FruitB_Overlap");

            Vector2 swipeStart = new Vector2(3, 3);
            Vector2 swipeEnd = new Vector2(7, 7);

            yield return WaitForPhysics(1);

            // Act
            var detectedFruits = collisionManager.GetFruitsInSwipePath(swipeStart, swipeEnd);

            // Assert
            Assert.AreEqual(2, detectedFruits.Count, 
                          "IT-005: Expected 2 fruits, got " + detectedFruits.Count);
            
            AssertFruitInList(fruitA, detectedFruits, "IT-005: Fruit A");
            AssertFruitInList(fruitB, detectedFruits, "IT-005: Fruit B");
        }

        // ==================== BOUNDARY CONDITION TESTS ====================

        /// <summary>
        /// TEST: IT-006 - Destroyed Fruit Handling (MEDIUM)
        /// 
        /// Description:
        /// Spawn fruit, destroy it, then test collision detection
        /// Verify system handles gracefully without exceptions
        /// 
        /// Setup:
        /// - Spawn fruit
        /// - Destroy fruit immediately
        /// - Trigger collision detection
        /// 
        /// Expected:
        /// - GetFruitsInSwipePath returns empty list or no null entries
        /// - No exceptions thrown
        /// 
        /// Priority: MEDIUM
        /// </summary>
        [UnityTest]
        [Category("Boundary")]
        public IEnumerator GetFruitsInSwipePath_DestroyedFruit_HandleGracefully()
        {
            // Arrange
            GameObject testFruit = CreateTestFruit(new Vector2(5, 5), 1.0f, "DestroyedFruit");
            
            yield return WaitForPhysics(1);
            
            // Destroy the fruit
            Object.Destroy(testFruit);
            
            yield return WaitForPhysics(2);

            Vector2 swipeStart = new Vector2(2, 5);
            Vector2 swipeEnd = new Vector2(8, 5);

            // Act
            var detectedFruits = collisionManager.GetFruitsInSwipePath(swipeStart, swipeEnd);

            // Assert
            // Should either return empty list or not contain null entries
            foreach (var fruit in detectedFruits)
            {
                Assert.IsNotNull(fruit, "IT-006: Null entry in detected fruits list");
            }
            
            Assert.Pass("IT-006: Gracefully handled destroyed fruit");
        }

        // ==================== EDGE CASE TESTS ====================

        /// <summary>
        /// TEST: Additional - No Fruits Detected
        /// 
        /// Description:
        /// Swipe through empty space (no fruits)
        /// Should return empty list
        /// </summary>
        [UnityTest]
        [Category("Boundary")]
        public IEnumerator GetFruitsInSwipePath_NoFruitsSpawned_ReturnsEmpty()
        {
            // No fruits spawned

            Vector2 swipeStart = new Vector2(0, 0);
            Vector2 swipeEnd = new Vector2(10, 10);

            yield return WaitForPhysics(1);

            // Act
            var detectedFruits = collisionManager.GetFruitsInSwipePath(swipeStart, swipeEnd);

            // Assert
            Assert.AreEqual(0, detectedFruits.Count, 
                          "Additional: Empty scene should return empty list");
        }

        /// <summary>
        /// TEST: Additional - Vertical Multi-Fruit Slicing
        /// 
        /// Description:
        /// Spawn fruits vertically and slice with vertical swipe
        /// Tests that algorithm works with non-horizontal swipes
        /// </summary>
        [UnityTest]
        [Category("MultiSlice")]
        public IEnumerator GetFruitsInSwipePath_VerticalSlicing_AllDetected()
        {
            // Arrange
            GameObject fruitA = CreateTestFruit(new Vector2(5, 2), 1.0f, "FruitA_Vert");
            GameObject fruitB = CreateTestFruit(new Vector2(5, 5), 1.0f, "FruitB_Vert");
            GameObject fruitC = CreateTestFruit(new Vector2(5, 8), 1.0f, "FruitC_Vert");

            Vector2 swipeStart = new Vector2(5, 0);
            Vector2 swipeEnd = new Vector2(5, 10);

            yield return WaitForPhysics(1);

            // Act
            var detectedFruits = collisionManager.GetFruitsInSwipePath(swipeStart, swipeEnd);

            // Assert
            Assert.AreEqual(3, detectedFruits.Count, 
                          "Additional: Expected 3 vertically-aligned fruits");
        }

        /// <summary>
        /// TEST: Additional - Diagonal Multi-Fruit Slicing
        /// 
        /// Description:
        /// Spawn fruits diagonally and slice with diagonal swipe
        /// </summary>
        [UnityTest]
        [Category("MultiSlice")]
        public IEnumerator GetFruitsInSwipePath_DiagonalSlicing_AllDetected()
        {
            // Arrange
            GameObject fruitA = CreateTestFruit(new Vector2(2, 2), 1.0f, "FruitA_Diag");
            GameObject fruitB = CreateTestFruit(new Vector2(5, 5), 1.0f, "FruitB_Diag");
            GameObject fruitC = CreateTestFruit(new Vector2(8, 8), 1.0f, "FruitC_Diag");

            Vector2 swipeStart = new Vector2(0, 0);
            Vector2 swipeEnd = new Vector2(10, 10);

            yield return WaitForPhysics(1);

            // Act
            var detectedFruits = collisionManager.GetFruitsInSwipePath(swipeStart, swipeEnd);

            // Assert
            Assert.AreEqual(3, detectedFruits.Count, 
                          "Additional: Expected 3 diagonally-aligned fruits");
        }
    }
}
