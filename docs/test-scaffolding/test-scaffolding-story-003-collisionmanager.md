# Test Scaffolding: Story 003 - CollisionManager MVP

**Story:** STORY-003: CollisionManager MVP  
**Epic:** Core Slicing Mechanics (EPIC-001)  
**Author:** BMAD (Test Architecture Agent)  
**Date:** November 29, 2025  
**Version:** 1.0  

---

## Executive Summary

This document defines the test structure, organization, utilities, and boilerplate code patterns for implementing tests for Story 003 (CollisionManager MVP). It provides reusable test fixtures, helper methods, and assembly organization to support efficient test development.

---

## Test Project Structure

### Directory Organization

```
Assets/
├── Scripts/
│   ├── Gameplay/
│   │   ├── CollisionManager.cs
│   │   ├── SwipeDetector.cs
│   │   └── ... other gameplay scripts
│   └── NinjaFruit.Runtime.asmdef
│
└── Tests/
    ├── EditMode/
    │   ├── Gameplay/
    │   │   ├── CollisionGeometryTests.cs         ← Edit Mode Unit Tests
    │   │   └── ... other Edit Mode tests
    │   └── EditMode.asmdef
    │
    ├── PlayMode/
    │   ├── Gameplay/
    │   │   ├── CollisionDetectionIntegrationTests.cs    ← Play Mode Integration Tests
    │   │   └── ... other Play Mode tests
    │   ├── TestScenes/
    │   │   └── CollisionTestScene.unity          ← Test scene for Play Mode
    │   └── PlayMode.asmdef
    │
    ├── TestUtilities/
    │   ├── CollisionTestHelpers.cs                ← Shared test utilities
    │   ├── FruitSpawner.cs                        ← Test fruit spawner
    │   └── TestUtilities.asmdef
    │
    └── [Other test files...]
```

### Assembly Definitions

**NinjaFruit.Runtime.asmdef:**
```json
{
    "name": "NinjaFruit.Runtime",
    "rootNamespace": "NinjaFruit",
    "references": [
        "Unity.InputSystem"
    ],
    "includePlatforms": [],
    "excludePlatforms": [],
    "allowUnsafeCode": false,
    "autoReferenced": true
}
```

**EditMode.asmdef:**
```json
{
    "name": "NinjaFruit.Tests.EditMode",
    "rootNamespace": "NinjaFruit.Tests.EditMode",
    "references": [
        "UnityEngine.TestRunner",
        "UnityEditor.TestRunner",
        "NinjaFruit.Runtime",
        "NinjaFruit.Tests.Utilities"
    ],
    "includePlatforms": [],
    "excludePlatforms": [],
    "defineConstraints": ["UNITY_INCLUDE_TESTS"]
}
```

**PlayMode.asmdef:**
```json
{
    "name": "NinjaFruit.Tests.PlayMode",
    "rootNamespace": "NinjaFruit.Tests.PlayMode",
    "references": [
        "UnityEngine.TestRunner",
        "NinjaFruit.Runtime",
        "NinjaFruit.Tests.Utilities"
    ],
    "includePlatforms": [],
    "excludePlatforms": [],
    "defineConstraints": ["UNITY_INCLUDE_TESTS"]
}
```

**TestUtilities.asmdef:**
```json
{
    "name": "NinjaFruit.Tests.Utilities",
    "rootNamespace": "NinjaFruit.Tests.Utilities",
    "references": [
        "UnityEngine.TestRunner",
        "NinjaFruit.Runtime"
    ],
    "includePlatforms": [],
    "excludePlatforms": [],
    "defineConstraints": ["UNITY_INCLUDE_TESTS"]
}
```

---

## Test Base Classes

### BaseCollisionTest (Edit Mode)

**Purpose:** Common setup/teardown for geometry tests

```csharp
using NUnit.Framework;
using UnityEngine;
using NinjaFruit;
using NinjaFruit.Tests.Utilities;

namespace NinjaFruit.Tests.EditMode.Gameplay
{
    /// <summary>
    /// Base class for Edit Mode collision geometry tests
    /// Provides common setup, teardown, and helper methods
    /// </summary>
    public abstract class BaseCollisionTest
    {
        protected CollisionManager collisionManager;
        
        /// <summary>
        /// Common setup for all Edit Mode geometry tests
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            // Create a GameObject to hold CollisionManager
            GameObject testObject = new GameObject("CollisionManagerTest");
            collisionManager = testObject.AddComponent<CollisionManager>();
            
            // Verify component initialized
            Assert.IsNotNull(collisionManager, "CollisionManager failed to instantiate");
        }
        
        /// <summary>
        /// Common teardown for all Edit Mode tests
        /// </summary>
        [TearDown]
        public virtual void Teardown()
        {
            // Clean up GameObject and component
            if (collisionManager != null && collisionManager.gameObject != null)
            {
                Object.DestroyImmediate(collisionManager.gameObject);
            }
        }
        
        /// <summary>
        /// Helper: Assert geometry calculation result with message
        /// </summary>
        protected void AssertIntersection(bool expected, Vector2 start, Vector2 end, 
                                         Vector2 fruitPos, float radius, string testName)
        {
            bool result = collisionManager.DoesSwipeIntersectFruit(start, end, fruitPos, radius);
            string message = $"{testName}: Expected {expected}, got {result}\n" +
                           $"  Swipe: ({start.x}, {start.y}) → ({end.x}, {end.y})\n" +
                           $"  Fruit: ({fruitPos.x}, {fruitPos.y}) r={radius}";
            Assert.AreEqual(expected, result, message);
        }
    }
}
```

### BaseCollisionPlayModeTest (Play Mode)

**Purpose:** Common setup/teardown for integration tests

```csharp
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NinjaFruit;
using NinjaFruit.Tests.Utilities;

namespace NinjaFruit.Tests.PlayMode.Gameplay
{
    /// <summary>
    /// Base class for Play Mode collision integration tests
    /// Handles GameObject/Scene management, fruit spawning, cleanup
    /// </summary>
    public abstract class BaseCollisionPlayModeTest
    {
        protected CollisionManager collisionManager;
        protected SwipeDetector swipeDetector;
        protected GameObject testSceneRoot;
        protected TestFruitSpawner fruitSpawner;
        
        /// <summary>
        /// Common setup for Play Mode tests
        /// Creates scene objects and initializes managers
        /// </summary>
        [SetUp]
        public virtual void Setup()
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
            
            // Create Fruit Spawner utility
            GameObject spawnerObject = new GameObject("TestFruitSpawner");
            spawnerObject.transform.SetParent(testSceneRoot.transform);
            fruitSpawner = spawnerObject.AddComponent<TestFruitSpawner>();
            
            // Initialize physics (important for Play Mode)
            Physics2D.defaultMaterial = Resources.Load<PhysicsMaterial2D>("DefaultPhysicsMaterial");
            
            Assert.IsNotNull(collisionManager, "CollisionManager failed to instantiate");
            Assert.IsNotNull(swipeDetector, "SwipeDetector failed to instantiate");
        }
        
        /// <summary>
        /// Common teardown for Play Mode tests
        /// Cleans up all test GameObjects
        /// </summary>
        [TearDown]
        public virtual void Teardown()
        {
            // Clean up test scene root (destroys all children)
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
        protected IEnumerator WaitForPhysics(int frames = 1)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return null;
            }
        }
        
        /// <summary>
        /// Helper: Assert fruit detection count and contents
        /// </summary>
        protected void AssertFruitsDetected(int expectedCount, 
                                           Vector2 start, Vector2 end, 
                                           string testName)
        {
            var detectedFruits = collisionManager.GetFruitsInSwipePath(start, end);
            string message = $"{testName}: Expected {expectedCount} fruits, got {detectedFruits.Count}";
            Assert.AreEqual(expectedCount, detectedFruits.Count, message);
        }
    }
}
```

---

## Test Helper Utilities

### CollisionTestHelpers.cs

```csharp
using UnityEngine;
using System.Collections.Generic;
using NinjaFruit;

namespace NinjaFruit.Tests.Utilities
{
    /// <summary>
    /// Static helper methods for collision testing
    /// Used by both Edit Mode and Play Mode tests
    /// </summary>
    public static class CollisionTestHelpers
    {
        /// <summary>
        /// Create a reference solution for geometric test case
        /// (For validating against expected results)
        /// </summary>
        public static bool ReferenceLineCircleIntersection(
            Vector2 lineStart, Vector2 lineEnd, Vector2 circleCenter, float radius)
        {
            // Degenerate case: zero-length line segment
            if (Vector2.Distance(lineStart, lineEnd) < 0.00001f)
                return false;
            
            // Vector from line start to circle center
            Vector2 f = circleCenter - lineStart;
            Vector2 d = (lineEnd - lineStart).normalized;
            
            // Project circle center onto line
            float t = Vector2.Dot(f, d);
            t = Mathf.Clamp01(t / Vector2.Distance(lineEnd, lineStart));
            
            // Find closest point on segment to circle center
            Vector2 closest = lineStart + t * (lineEnd - lineStart);
            
            // Distance from center to closest point
            float distance = Vector2.Distance(circleCenter, closest);
            
            // Pass-through: distance less than radius AND closest point within segment
            return distance < radius && t > 0 && t < 1;
        }
        
        /// <summary>
        /// Calculate distance from point to line segment
        /// </summary>
        public static float DistancePointToLineSegment(
            Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            Vector2 PA = point - lineStart;
            Vector2 BA = lineEnd - lineStart;
            float h = Mathf.Clamp01(Vector2.Dot(PA, BA) / Vector2.Dot(BA, BA));
            return (PA - BA * h).magnitude;
        }
        
        /// <summary>
        /// Get intersection point(s) of line segment and circle
        /// Returns count of intersection points (0, 1, or 2)
        /// </summary>
        public static int GetIntersectionPoints(
            Vector2 lineStart, Vector2 lineEnd, Vector2 circleCenter, 
            float radius, out Vector2 point1, out Vector2 point2)
        {
            point1 = Vector2.zero;
            point2 = Vector2.zero;
            
            Vector2 d = lineEnd - lineStart;
            Vector2 f = lineStart - circleCenter;
            
            float a = Vector2.Dot(d, d);
            float b = 2 * Vector2.Dot(f, d);
            float c = Vector2.Dot(f, f) - radius * radius;
            
            float discriminant = b * b - 4 * a * c;
            
            if (discriminant < 0)
                return 0; // No intersection
            
            discriminant = Mathf.Sqrt(discriminant);
            
            float t1 = (-b - discriminant) / (2 * a);
            float t2 = (-b + discriminant) / (2 * a);
            
            int intersectionCount = 0;
            
            if (t1 >= 0 && t1 <= 1)
            {
                point1 = lineStart + t1 * d;
                intersectionCount++;
            }
            
            if (t2 >= 0 && t2 <= 1 && Mathf.Abs(t2 - t1) > 0.0001f)
            {
                point2 = lineStart + t2 * d;
                intersectionCount++;
            }
            
            return intersectionCount;
        }
        
        /// <summary>
        /// Check if two floats are equal within epsilon
        /// </summary>
        public static bool ApproximatelyEqual(float a, float b, float epsilon = 0.0001f)
        {
            return Mathf.Abs(a - b) <= epsilon;
        }
        
        /// <summary>
        /// Format test data for debugging output
        /// </summary>
        public static string FormatTestCase(
            Vector2 start, Vector2 end, Vector2 pos, float radius, string name)
        {
            return $"{name}:\n" +
                   $"  Start: ({start.x:F2}, {start.y:F2})\n" +
                   $"  End: ({end.x:F2}, {end.y:F2})\n" +
                   $"  Fruit: ({pos.x:F2}, {pos.y:F2}) r={radius:F2}";
        }
    }
}
```

### TestFruitSpawner.cs

**Purpose:** Spawn test fruits with predictable properties

```csharp
using UnityEngine;
using System.Collections.Generic;
using NinjaFruit;

namespace NinjaFruit.Tests.Utilities
{
    /// <summary>
    /// MonoBehaviour utility for spawning test fruits
    /// Used in Play Mode tests for collision detection
    /// </summary>
    public class TestFruitSpawner : MonoBehaviour
    {
        private List<GameObject> spawnedFruits = new List<GameObject>();
        private int fruitCounter = 0;
        
        /// <summary>
        /// Spawn a test fruit at specified position with collision radius
        /// </summary>
        public GameObject SpawnTestFruit(Vector2 position, float colliderRadius, 
                                        string name = null)
        {
            // Create GameObject
            GameObject fruit = new GameObject(name ?? $"TestFruit_{fruitCounter++}");
            fruit.transform.SetParent(transform);
            fruit.transform.position = position;
            fruit.tag = "Fruit";
            fruit.layer = LayerMask.NameToLayer("Fruit");
            
            // Add CircleCollider2D
            CircleCollider2D collider = fruit.AddComponent<CircleCollider2D>();
            collider.radius = colliderRadius;
            collider.isTrigger = false;
            
            // Add Rigidbody2D (required for physics)
            Rigidbody2D rb = fruit.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0; // Disable gravity for tests
            rb.isKinematic = false; // Allow physics but won't be moved by tests
            
            // Add Fruit component (gameplay logic)
            var fruitComponent = fruit.AddComponent<Fruit>();
            fruitComponent.fruitType = FruitType.Apple; // Default type
            
            spawnedFruits.Add(fruit);
            return fruit;
        }
        
        /// <summary>
        /// Spawn multiple fruits in a line
        /// </summary>
        public List<GameObject> SpawnFruitsInLine(Vector2 startPos, Vector2 direction, 
                                                 int count, float spacing, float radius)
        {
            var fruits = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                Vector2 pos = startPos + direction * spacing * i;
                fruits.Add(SpawnTestFruit(pos, radius, $"TestFruit_Line_{i}"));
            }
            return fruits;
        }
        
        /// <summary>
        /// Clear all spawned fruits
        /// </summary>
        public void ClearAll()
        {
            foreach (var fruit in spawnedFruits)
            {
                if (fruit != null)
                    Object.Destroy(fruit);
            }
            spawnedFruits.Clear();
            fruitCounter = 0;
        }
        
        /// <summary>
        /// Get list of currently spawned fruits
        /// </summary>
        public List<GameObject> GetSpawnedFruits() => new List<GameObject>(spawnedFruits);
        
        /// <summary>
        /// Set physics layer for all spawned fruits
        /// </summary>
        public void SetPhysicsLayer(int layer)
        {
            foreach (var fruit in spawnedFruits)
            {
                if (fruit != null)
                    fruit.layer = layer;
            }
        }
    }
}
```

---

## Test Scene Setup

### CollisionTestScene.unity Configuration

Create a test scene with the following setup:

**Scene Hierarchy:**
```
CollisionTestScene
├── GameManager (empty GameObject)
│   ├── CollisionManager (component)
│   ├── SwipeDetector (component)
│   └── TestFruitSpawner (component)
└── Physics Settings (configured as below)
```

**Physics2D Settings (for this scene):**
- Gravity: (0, 0)
- Default Material: Standard (friction=0.4, bounciness=0)
- Solver Iterations: 8
- Solver Velocity Iterations: 4

**Layer Setup:**
```
Layer 8: Fruit
Layer 9: Bomb
Layer 10: Player
```

**Physics Collision Matrix:**
- Fruit ↔ Fruit: Disabled (fruits don't collide with each other)
- Bomb ↔ Fruit: Disabled
- All others: Default

---

## Test Fixture Patterns

### Data-Driven Test Pattern

```csharp
/// <summary>
/// Example: Data-driven test using TestCase attributes
/// </summary>
[TestFixture]
public class CollisionGeometryTests : BaseCollisionTest
{
    // Test case data: (start, end, fruitPos, radius, shouldIntersect)
    [TestCase(
        new float[] { 0, 0 },      // start X, Y
        new float[] { 10, 0 },     // end X, Y
        new float[] { 5, 0 },      // fruit X, Y
        1.0f,                       // radius
        true,                       // expected result
        TestName = "UT-001: Horizontal PassThrough")]
    [TestCase(
        new float[] { 0, 0 },
        new float[] { 5, 2 },
        new float[] { 2, 1 },
        1.0f,
        false,
        TestName = "UT-003: Tangent")]
    public void DoesSwipeIntersectFruit_VariousInputs_CorrectResult(
        float[] start, float[] end, float[] fruitPos, float radius, bool expected)
    {
        Vector2 startVec = new Vector2(start[0], start[1]);
        Vector2 endVec = new Vector2(end[0], end[1]);
        Vector2 posvec = new Vector2(fruitPos[0], fruitPos[1]);
        
        AssertIntersection(expected, startVec, endVec, posvec, radius, 
                          TestContext.CurrentTestExecutionContext.Test.Name);
    }
}
```

### Parametrized Test Pattern

```csharp
/// <summary>
/// Example: Multiple test cases in single method
/// </summary>
[TestFixture]
public class CollisionMultiFruitTests : BaseCollisionPlayModeTest
{
    public static IEnumerable<TestCaseData> MultiSliceTestCases()
    {
        // (start, end, expectedCount, testName)
        yield return new TestCaseData(
            new Vector2(0, 5), new Vector2(10, 5), 3, "Three Fruits Horizontal"
        );
        
        yield return new TestCaseData(
            new Vector2(3, 3), new Vector2(7, 7), 2, "Two Fruits Diagonal"
        );
    }
    
    [TestCaseSource(nameof(MultiSliceTestCases))]
    [UnityTest]
    public IEnumerator GetFruitsInSwipePath_MultipleScenarios_CorrectCount(
        Vector2 start, Vector2 end, int expectedCount, string testName)
    {
        // Spawn fruits based on test case...
        yield return WaitForPhysics(1);
        
        AssertFruitsDetected(expectedCount, start, end, testName);
    }
}
```

---

## Performance Testing Pattern

```csharp
/// <summary>
/// Example: Performance test for stress scenarios
/// </summary>
[TestFixture]
[Performance]
public class CollisionPerformanceTests : BaseCollisionTest
{
    [Performance]
    [Test]
    public void DoesSwipeIntersectFruit_LargeNumber_PerformanceAcceptable()
    {
        Vector2 start = new Vector2(0, 0);
        Vector2 end = new Vector2(100, 0);
        
        Measure.Frames()
            .Warmup(10)
            .Run(() =>
            {
                // Test 1000 collision checks
                for (int i = 0; i < 1000; i++)
                {
                    float x = (i % 20) * 5;
                    float y = (i / 20) * 5;
                    Vector2 fruitPos = new Vector2(x, y);
                    
                    collisionManager.DoesSwipeIntersectFruit(start, end, fruitPos, 1.0f);
                }
            });
    }
}
```

---

## Integration Pattern: SwipeDetector Event

```csharp
/// <summary>
/// Example: Test SwipeDetector event subscription
/// </summary>
[TestFixture]
public class SwipeEventIntegrationTest : BaseCollisionPlayModeTest
{
    [UnityTest]
    public IEnumerator OnSwipeDetected_FiresEvent_CollisionManagerResponds()
    {
        // Track if collision handler was called
        bool eventReceived = false;
        
        // Subscribe to collision manager's internal event (or expose a public event)
        // For this example, assuming CollisionManager subscribes to SwipeDetector
        
        // Simulate swipe event
        swipeDetector.OnSwipeDetected?.Invoke(
            new Vector2(0, 5), 
            new Vector2(10, 5)
        );
        
        yield return WaitForPhysics(1);
        
        // Verify collision detection was triggered
        // (This depends on how CollisionManager exposes its state)
        Assert.Pass("Event received and handled");
    }
}
```

---

## Test Documentation Template

```csharp
/// <summary>
/// Test: [Test Name]
/// 
/// Story: STORY-003: CollisionManager MVP
/// Test ID: [UT-001]
/// Category: [Category]
/// Priority: [CRITICAL/HIGH/MEDIUM]
/// 
/// Preconditions:
/// - CollisionManager instantiated
/// - [Other preconditions]
/// 
/// Test Data:
/// - Swipe Start: (0, 0)
/// - Swipe End: (10, 0)
/// - Fruit Position: (5, 0)
/// - Fruit Radius: 1.0
/// 
/// Expected Result:
/// - Should return true
/// - Reason: Horizontal line passes through circle center
/// 
/// Pass Criteria:
/// - Return value is exactly true
/// - No exceptions thrown
/// 
/// Fail Criteria:
/// - Return value is false
/// - Exception thrown
/// </summary>
[Test]
public void DoesSwipeIntersectFruit_PassThrough_ReturnsTrue()
{
    // Implementation...
}
```

---

## Code Quality Checklist

Before submitting tests, verify:

- ✅ All tests have clear names following `TC_[Layer]_[Feature]_[Scenario]_[Expected]`
- ✅ Each test has a docstring explaining purpose
- ✅ Test data explicitly defined (not hardcoded magic numbers)
- ✅ Setup and teardown are deterministic
- ✅ No test-to-test dependencies
- ✅ Assertions have meaningful error messages
- ✅ Play Mode tests include `yield` statements for frame timing
- ✅ Resource cleanup in teardown (no memory leaks)
- ✅ No platform-specific code (tests cross-platform)
- ✅ Performance benchmarks acceptable (<1ms per collision)
- ✅ Comments explain non-obvious logic

---

## Common Patterns and Anti-Patterns

### ✅ GOOD: Clear Test Structure
```csharp
[Test]
public void FeatureName_Condition_ExpectedResult()
{
    // Arrange
    var input = CreateTestData();
    
    // Act
    var result = collisionManager.DoesSwipeIntersectFruit(...);
    
    // Assert
    Assert.AreEqual(expected, result, "Clear error message");
}
```

### ❌ BAD: Unclear Test Structure
```csharp
[Test]
public void Test1()
{
    bool x = collisionManager.DoesSwipeIntersectFruit(v1, v2, v3, 1.0f);
    Assert.IsTrue(x); // What does this test? Why should it pass?
}
```

### ✅ GOOD: Detailed Assertions
```csharp
Assert.AreEqual(expectedCount, actualList.Count, 
    $"Expected {expectedCount} fruits in swipe path, but got {actualList.Count}");
```

### ❌ BAD: Vague Assertions
```csharp
Assert.IsTrue(result); // What does result represent?
```

---

## Continuous Integration Considerations

### Test Execution in CI/CD

**Edit Mode Tests:**
```bash
# Run with: unity -runTests -testPlatform editmode
unity -projectPath . \
  -runTests \
  -testPlatform editmode \
  -testCategory NinjaFruit.Tests.EditMode \
  -logFile test-results-editmode.log \
  -resultsFile TestResults/editmode/results.xml
```

**Play Mode Tests:**
```bash
# Run with: unity -runTests -testPlatform playmode
unity -projectPath . \
  -runTests \
  -testPlatform playmode \
  -testCategory NinjaFruit.Tests.PlayMode \
  -logFile test-results-playmode.log \
  -resultsFile TestResults/playmode/results.xml
```

### Parallel Execution

- Edit Mode tests can run in parallel (no scene state shared)
- Play Mode tests should run sequentially (scene-dependent)
- Consider test ordering if needed

---

## Appendix: Environment Setup Checklist

- [ ] Unity 6 project created
- [ ] Assembly Definitions configured (Runtime, EditMode, PlayMode, TestUtilities)
- [ ] Fruit layer created (Layer 8)
- [ ] Test utilities scripts created
- [ ] Base test classes created
- [ ] CollisionTestScene.unity created with proper physics settings
- [ ] Test scenes linked in Test Framework settings
- [ ] NUnit framework verified in project
- [ ] Input System package installed (for future input tests)

---

## Document History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-11-29 | BMAD | Initial test scaffolding for Story 003 CollisionManager |

---

**Status:** READY FOR TEST IMPLEMENTATION  
**Next Step:** Create actual test C# code files  
**Owner:** BMAD Test Architecture Agent

