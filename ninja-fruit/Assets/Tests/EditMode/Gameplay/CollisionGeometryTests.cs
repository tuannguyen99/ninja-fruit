using NUnit.Framework;
using UnityEngine;
using NinjaFruit;

namespace NinjaFruit.Tests.EditMode.Gameplay
{
    /// <summary>
    /// Edit Mode Unit Tests for CollisionManager geometry
    /// 
    /// Story: STORY-003: CollisionManager MVP
    /// Epic: Core Slicing Mechanics
    /// 
    /// Tests the pure line-circle intersection mathematics
    /// without requiring Unity runtime or GameObject instances.
    /// 
    /// Test Coverage:
    /// - Pass-through cases (swipe line enters and exits circle)
    /// - Tangent edge cases (swipe line touches circle perimeter)
    /// - Miss cases (swipe line completely outside circle)
    /// - Boundary conditions (zero-length swipes, etc.)
    /// - Various radius sizes (small fruits, large fruits)
    /// </summary>
    [TestFixture]
    public class CollisionGeometryTests
    {
        private CollisionManager collisionManager;

        /// <summary>
        /// Setup for each test
        /// Creates a CollisionManager instance for testing
        /// </summary>
        [SetUp]
        public void Setup()
        {
            GameObject testObject = new GameObject("CollisionManagerTest");
            collisionManager = testObject.AddComponent<CollisionManager>();
            
            Assert.IsNotNull(collisionManager, "CollisionManager failed to instantiate");
        }

        /// <summary>
        /// Teardown after each test
        /// Cleans up GameObject and component
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            if (collisionManager != null && collisionManager.gameObject != null)
            {
                Object.DestroyImmediate(collisionManager.gameObject);
            }
        }

        /// <summary>
        /// Helper method to assert intersection with clear error messaging
        /// </summary>
        private void AssertIntersection(bool expected, Vector2 start, Vector2 end,
                                       Vector2 fruitPos, float radius, string testName)
        {
            bool result = collisionManager.DoesSwipeIntersectFruit(start, end, fruitPos, radius);
            string message = $"{testName}\n" +
                           $"  Expected: {expected}, Got: {result}\n" +
                           $"  Swipe: ({start.x}, {start.y}) → ({end.x}, {end.y})\n" +
                           $"  Fruit: pos=({fruitPos.x}, {fruitPos.y}) radius={radius}";
            Assert.AreEqual(expected, result, message);
        }

        // ==================== PASS-THROUGH TESTS ====================

        /// <summary>
        /// TEST: UT-001 - Horizontal Pass-Through
        /// 
        /// Description:
        /// Horizontal line passing through circle center should return true
        /// This is the baseline pass-through test case
        /// 
        /// Input:
        /// - Swipe: (0,0) → (10,0)
        /// - Fruit: (5,0) radius=1.0
        /// 
        /// Expected: TRUE (line passes through circle)
        /// </summary>
        [Test]
        [Category("PassThrough")]
        public void DoesSwipeIntersectFruit_HorizontalPassThrough_ReturnsTrue()
        {
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(10, 0);
            Vector2 fruitPos = new Vector2(5, 0);
            float radius = 1.0f;

            AssertIntersection(true, start, end, fruitPos, radius, "UT-001: Horizontal PassThrough");
        }

        /// <summary>
        /// TEST: UT-002 - Diagonal Pass-Through
        /// 
        /// Description:
        /// Diagonal (45°) line passing through circle center should return true
        /// Tests angle independence of geometry algorithm
        /// 
        /// Input:
        /// - Swipe: (0,0) → (10,10)
        /// - Fruit: (5,5) radius=2.0
        /// 
        /// Expected: TRUE (diagonal line through center)
        /// </summary>
        [Test]
        [Category("PassThrough")]
        public void DoesSwipeIntersectFruit_DiagonalPassThrough_ReturnsTrue()
        {
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(10, 10);
            Vector2 fruitPos = new Vector2(5, 5);
            float radius = 2.0f;

            AssertIntersection(true, start, end, fruitPos, radius, "UT-002: Diagonal PassThrough");
        }

        /// <summary>
        /// TEST: UT-006 - Short Swipe Pass-Through
        /// 
        /// Description:
        /// Short swipe (4 units) passing through circle (diameter 2 units) should return true
        /// Tests that algorithm works with various swipe lengths
        /// 
        /// Input:
        /// - Swipe: (3,5) → (7,5)
        /// - Fruit: (5,5) radius=1.0
        /// 
        /// Expected: TRUE (short swipe passes through)
        /// </summary>
        [Test]
        [Category("PassThrough")]
        public void DoesSwipeIntersectFruit_ShortSwipePassThrough_ReturnsTrue()
        {
            Vector2 start = new Vector2(3, 5);
            Vector2 end = new Vector2(7, 5);
            Vector2 fruitPos = new Vector2(5, 5);
            float radius = 1.0f;

            AssertIntersection(true, start, end, fruitPos, radius, "UT-006: Short Swipe PassThrough");
        }

        // ==================== EDGE CASE: TANGENT TESTS ====================

        /// <summary>
        /// TEST: UT-003 - Tangent Case (CRITICAL)
        /// 
        /// Description:
        /// Swipe line touching circle perimeter (tangent) should return FALSE
        /// This is CRITICAL: tangent swipes should NOT register as slices
        /// 
        /// Input:
        /// - Swipe: (0,0) → (10,0) [Horizontal line along y=0]
        /// - Fruit: (5,1.0) radius=1.0 [Directly above swipe]
        /// - Geometry: Line is exactly tangent to circle (touches perimeter at one point)
        /// - Distance from line to center: exactly 1.0 (equals radius)
        /// 
        /// Expected: FALSE (tangent touches, doesn't pass-through)
        /// 
        /// Pass Criteria:
        /// - Must reject tangent cases (distance == radius)
        /// - Epsilon tolerance must not be too loose
        /// </summary>
        [Test]
        [Category("Tangent")]
        public void DoesSwipeIntersectFruit_TangentCase_ReturnsFalse()
        {
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(10, 0);
            Vector2 fruitPos = new Vector2(5, 1.0f);
            float radius = 1.0f;

            AssertIntersection(false, start, end, fruitPos, radius, 
                             "UT-003: Tangent Case (CRITICAL - should return FALSE)");
        }

        // ==================== MISS TESTS ====================

        /// <summary>
        /// TEST: UT-004 - Complete Miss
        /// 
        /// Description:
        /// Swipe line completely outside circle (3 units away, only 1 unit radius) should return false
        /// Tests basic negative case
        /// 
        /// Input:
        /// - Swipe: (0,0) → (10,0)
        /// - Fruit: (5,3) radius=1.0
        /// - Distance: 3 units (exceeds 1 unit radius)
        /// 
        /// Expected: FALSE (no intersection)
        /// </summary>
        [Test]
        [Category("Miss")]
        public void DoesSwipeIntersectFruit_CompleteMiss_ReturnsFalse()
        {
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(10, 0);
            Vector2 fruitPos = new Vector2(5, 3);
            float radius = 1.0f;

            AssertIntersection(false, start, end, fruitPos, radius, "UT-004: Complete Miss");
        }

        /// <summary>
        /// TEST: UT-008 - Very Close But Miss (CRITICAL)
        /// 
        /// Description:
        /// Swipe line 0.99 units away from circle center with 0.5 radius
        /// Should return FALSE (0.49 unit margin clear miss)
        /// This tests floating-point precision boundaries
        /// 
        /// Input:
        /// - Swipe: (0,0) → (10,0)
        /// - Fruit: (5, 0.99) radius=0.5
        /// - Margin: 0.49 units (clearly outside but close)
        /// 
        /// Expected: FALSE
        /// 
        /// Pass Criteria:
        /// - Precision epsilon working correctly
        /// - Not accepting marginal cases
        /// </summary>
        [Test]
        [Category("Boundary")]
        public void DoesSwipeIntersectFruit_VeryCloseButMiss_ReturnsFalse()
        {
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(10, 0);
            Vector2 fruitPos = new Vector2(5, 0.99f);
            float radius = 0.5f;

            AssertIntersection(false, start, end, fruitPos, radius, 
                             "UT-008: Very Close But Miss (CRITICAL - precision test)");
        }

        // ==================== BOUNDARY CONDITION TESTS ====================

        /// <summary>
        /// TEST: UT-005 - Zero-Length Swipe
        /// 
        /// Description:
        /// Swipe with start == end (zero length) should return FALSE
        /// A point collision is not a valid slice
        /// 
        /// Input:
        /// - Swipe: (5,5) → (5,5) [same point]
        /// - Fruit: (5,5) radius=1.0
        /// 
        /// Expected: FALSE
        /// 
        /// Pass Criteria:
        /// - Rejects degenerate case gracefully
        /// - No division by zero exception
        /// </summary>
        [Test]
        [Category("Boundary")]
        public void DoesSwipeIntersectFruit_ZeroLengthSwipe_ReturnsFalse()
        {
            Vector2 start = new Vector2(5, 5);
            Vector2 end = new Vector2(5, 5);  // Same as start
            Vector2 fruitPos = new Vector2(5, 5);
            float radius = 1.0f;

            AssertIntersection(false, start, end, fruitPos, radius, 
                             "UT-005: Zero-Length Swipe (Boundary Condition)");
        }

        // ==================== RADIUS VARIATION TESTS ====================

        /// <summary>
        /// TEST: UT-007 - Large Fruit Pass-Through
        /// 
        /// Description:
        /// Swipe passing through large fruit (watermelon-sized, radius 3.0)
        /// should return TRUE
        /// Tests that algorithm works with large radius values
        /// 
        /// Input:
        /// - Swipe: (2,5) → (8,5)
        /// - Fruit: (5,5) radius=3.0
        /// 
        /// Expected: TRUE (swipe passes through large circle)
        /// </summary>
        [Test]
        [Category("RadiusVariation")]
        public void DoesSwipeIntersectFruit_LargeFruitPassThrough_ReturnsTrue()
        {
            Vector2 start = new Vector2(2, 5);
            Vector2 end = new Vector2(8, 5);
            Vector2 fruitPos = new Vector2(5, 5);
            float radius = 3.0f;

            AssertIntersection(true, start, end, fruitPos, radius, "UT-007: Large Fruit PassThrough");
        }

        // ==================== ADDITIONAL EDGE CASES ====================

        /// <summary>
        /// TEST: Additional - Vertical Line Pass-Through
        /// 
        /// Tests pass-through with vertical swipe (not horizontal or diagonal)
        /// </summary>
        [Test]
        [Category("PassThrough")]
        public void DoesSwipeIntersectFruit_VerticalPassThrough_ReturnsTrue()
        {
            Vector2 start = new Vector2(5, 0);
            Vector2 end = new Vector2(5, 10);
            Vector2 fruitPos = new Vector2(5, 5);
            float radius = 1.0f;

            AssertIntersection(true, start, end, fruitPos, radius, "Additional: Vertical PassThrough");
        }

        /// <summary>
        /// TEST: Additional - Offset Pass-Through
        /// 
        /// Swipe passes through circle but not through center
        /// Still should return TRUE (partial pass-through counts)
        /// </summary>
        [Test]
        [Category("PassThrough")]
        public void DoesSwipeIntersectFruit_OffsetPassThrough_ReturnsTrue()
        {
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(10, 5);
            Vector2 fruitPos = new Vector2(5, 2);
            float radius = 1.5f;

            AssertIntersection(true, start, end, fruitPos, radius, "Additional: Offset PassThrough");
        }

        /// <summary>
        /// TEST: Additional - Swipe Starting Inside Circle
        /// 
        /// Edge case: Swipe starts inside circle but doesn't pass all the way through
        /// Behavior: Should NOT count as pass-through
        /// </summary>
        [Test]
        [Category("Boundary")]
        public void DoesSwipeIntersectFruit_SwipeStartInsideCircle_ReturnsFalse()
        {
            Vector2 start = new Vector2(4, 0);  // Inside circle (only 1 unit from center)
            Vector2 end = new Vector2(4, 10);   // Extends upward
            Vector2 fruitPos = new Vector2(5, 0);
            float radius = 2.0f;

            AssertIntersection(false, start, end, fruitPos, radius, 
                             "Additional: Start Inside Circle");
        }

        /// <summary>
        /// TEST: Additional - Swipe Ending Inside Circle
        /// 
        /// Edge case: Swipe ends inside circle
        /// Behavior: Should NOT count as pass-through (need proper entry AND exit)
        /// </summary>
        [Test]
        [Category("Boundary")]
        public void DoesSwipeIntersectFruit_SwipeEndInsideCircle_ReturnsFalse()
        {
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(4, 0);    // Ends inside circle
            Vector2 fruitPos = new Vector2(5, 0);
            float radius = 2.0f;

            AssertIntersection(false, start, end, fruitPos, radius, 
                             "Additional: End Inside Circle");
        }
    }
}
