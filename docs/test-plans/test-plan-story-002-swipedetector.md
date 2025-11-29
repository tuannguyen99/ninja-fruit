````markdown
# Test Plan: STORY-002 - SwipeDetector MVP

**Generated:** 2025-11-28  
**Story:** STORY-002 - SwipeDetector MVP  
**Epic:** EPIC-001 - Core Slicing Mechanics  
**Test Architect:** Murat  
**Framework:** Unity Test Framework (NUnit) + Unity Input System Test Fixtures

---

## Executive Summary

This test plan covers the SwipeDetector component, which is **CRITICAL** to gameplay. The component must accurately detect swipe gestures and validate speed thresholds (≥100 px/s) to trigger fruit slicing. Risk assessment indicates HIGH impact if swipe detection fails or produces false positives/negatives.

**Test Strategy:** Mix of Edit Mode tests (speed calculation logic) and Play Mode tests (input simulation + event triggering).

**Estimated Test Count:** 10 tests (6 Edit Mode, 4 Play Mode)  
**Estimated Execution Time:** Edit Mode ~30ms, Play Mode ~8s  
**Target Coverage:** 95%+ on SwipeDetector logic

---

## Risk Assessment

### Risk Matrix Analysis

| Risk ID | Description | Category | Probability | Impact | Risk Score | Priority |
|---------|-------------|----------|-------------|--------|------------|----------|
| RISK-011 | Speed calculation formula incorrect | TECH | Medium | Critical | **HIGH** | P0 |
| RISK-012 | Swipe validation logic fails boundary conditions | TECH | Medium | Critical | **HIGH** | P0 |
| RISK-013 | Input event not captured/processed | TECH | Low | Critical | **MEDIUM** | P1 |
| RISK-014 | OnSwipeDetected event not triggered | TECH | Low | Critical | **MEDIUM** | P1 |
| RISK-015 | Slow swipes incorrectly validated as valid | TECH | Medium | High | **MEDIUM** | P1 |
| RISK-016 | False positives on accidental touches | TECH | Low | Medium | **LOW** | P2 |

**Risk Scoring Formula:** `Risk Score = Probability × Impact`
- **HIGH:** Blocks core gameplay (P0 - test first)
- **MEDIUM:** Degrades experience (P1 - test before merge)
- **LOW:** Minor issues (P2 - test eventually)

---

## Test Categories

### Category Breakdown

| Category | Test Count | Rationale |
|----------|------------|-----------|
| **Edit Mode (Unit)** | 6 | Speed calculation, swipe validation logic, boundary conditions |
| **Play Mode (Integration)** | 4 | Input simulation, event triggering, end-to-end gesture detection |

**Why this split?**
- **Edit Mode advantage:** Fast (<5ms per test), pure logic validation, no input system overhead
- **Play Mode necessity:** Required to validate Unity Input System integration and event flow

---

## Test Coverage Matrix

### Acceptance Criteria Mapping

| AC ID | Acceptance Criteria | Test Type | Test Count | Priority |
|-------|---------------------|-----------|------------|----------|
| **AC-1** | `IsValidSwipe(points, deltaTime)` returns `true` only when speed ≥100px/s | Edit Mode | 3 | P0 |
| **AC-2** | `CalculateSwipeSpeed(start, end, deltaTime)` computes pixels/second correctly | Edit Mode | 3 | P0 |
| **AC-3** | Play Mode test simulates fast mouse swipe and triggers `OnSwipeDetected` | Play Mode | 2 | P1 |
| **AC-4** | Boundary condition: exactly 100px/s should be valid | Edit Mode | 1 | P0 |
| **AC-5** | Slow swipes (<100px/s) do not trigger event | Play Mode | 1 | P1 |
| **AC-6** | Tangential movement not passing through fruit should not slice | Play Mode | 1 | P2 |

**Coverage Goal:** 100% of acceptance criteria, 95%+ code coverage

---

## Detailed Test Specifications

### Edit Mode Tests (Unit Tests)

#### Test Suite: SwipeSpeedCalculationTests

**Purpose:** Validate swipe speed calculation accuracy (pixels/second formula)

**Location:** `Assets/Tests/EditMode/Input/SwipeDetectorTests.cs`

**Test Cases:**

1. **CalculateSwipeSpeed_200Pixels1Second_Returns200PixelsPerSecond**
   - **Given:** Start point (0, 0), end point (200, 0), deltaTime = 1.0s
   - **When:** `CalculateSwipeSpeed(start, end, deltaTime)` is called
   - **Then:** Returns exactly 200.0f px/s
   - **Risk:** RISK-011 (HIGH)
   - **Priority:** P0

2. **CalculateSwipeSpeed_100Pixels1Second_Returns100PixelsPerSecond**
   - **Given:** Start point (0, 0), end point (100, 0), deltaTime = 1.0s
   - **When:** `CalculateSwipeSpeed(start, end, deltaTime)` is called
   - **Then:** Returns exactly 100.0f px/s (boundary condition)
   - **Risk:** RISK-011 (HIGH)
   - **Priority:** P0
   - **Edge Case:** Exact threshold boundary

3. **CalculateSwipeSpeed_50Pixels0Point5Seconds_Returns100PixelsPerSecond**
   - **Given:** Start point (0, 0), end point (50, 0), deltaTime = 0.5s
   - **When:** `CalculateSwipeSpeed(start, end, deltaTime)` is called
   - **Then:** Returns exactly 100.0f px/s (50 / 0.5 = 100)
   - **Risk:** RISK-011 (HIGH)
   - **Priority:** P0

4. **CalculateSwipeSpeed_DiagonalSwipe_CalculatesEuclideanDistance**
   - **Given:** Start (0, 0), end (30, 40), deltaTime = 1.0s (diagonal swipe)
   - **When:** `CalculateSwipeSpeed(start, end, deltaTime)` is called
   - **Then:** Returns 50.0f px/s (√(30² + 40²) = 50)
   - **Risk:** RISK-011 (HIGH)
   - **Priority:** P0
   - **Edge Case:** Validates Vector2.Distance() usage

5. **CalculateSwipeSpeed_ZeroDeltaTime_ReturnsZeroOrHandlesGracefully**
   - **Given:** Any points, deltaTime = 0.0s (edge case)
   - **When:** `CalculateSwipeSpeed(start, end, deltaTime)` is called
   - **Then:** Returns 0.0f or handles without exception (defensive)
   - **Risk:** RISK-011 (HIGH)
   - **Priority:** P0
   - **Edge Case:** Division by zero protection

---

#### Test Suite: SwipeValidationTests

**Purpose:** Validate IsValidSwipe() logic based on speed threshold (≥100 px/s)

**Location:** `Assets/Tests/EditMode/Input/SwipeDetectorTests.cs`

**Test Cases:**

6. **IsValidSwipe_200PixelsPerSecond_ReturnsTrue**
   - **Given:** Points with distance 200px, deltaTime = 1.0s (speed = 200 px/s)
   - **When:** `IsValidSwipe(points, deltaTime)` is called
   - **Then:** Returns `true` (exceeds 100 px/s threshold)
   - **Risk:** RISK-012 (HIGH)
   - **Priority:** P0

7. **IsValidSwipe_50PixelsPerSecond_ReturnsFalse**
   - **Given:** Points with distance 50px, deltaTime = 1.0s (speed = 50 px/s)
   - **When:** `IsValidSwipe(points, deltaTime)` is called
   - **Then:** Returns `false` (below 100 px/s threshold)
   - **Risk:** RISK-015 (MEDIUM)
   - **Priority:** P1

8. **IsValidSwipe_Exactly100PixelsPerSecond_ReturnsTrue**
   - **Given:** Points with distance 100px, deltaTime = 1.0s (speed = exactly 100 px/s)
   - **When:** `IsValidSwipe(points, deltaTime)` is called
   - **Then:** Returns `true` (boundary condition: inclusive threshold)
   - **Risk:** RISK-012 (HIGH)
   - **Priority:** P0
   - **Edge Case:** Validates ≥ operator (not just >)

---

### Play Mode Tests (Integration Tests)

#### Test Suite: SwipeInputIntegrationTests

**Purpose:** Validate input system integration and event triggering

**Location:** `Assets/Tests/PlayMode/Input/SwipeInputIntegrationTests.cs`

**Test Cases:**

9. **SwipeDetector_FastMouseSwipe_TriggersOnSwipeDetectedEvent**
   - **Given:** SwipeDetector component with InputManager configured
   - **When:** Mouse input simulated from (100, 100) to (300, 300) over 0.5s (≈280 px/s)
   - **Then:** `OnSwipeDetected` event is triggered with correct start/end points
   - **Risk:** RISK-014 (MEDIUM)
   - **Priority:** P1
   - **Setup:** Use Unity InputTestFixture to simulate mouse
   - **Teardown:** Cleanup input devices

10. **SwipeDetector_SlowMouseSwipe_DoesNotTriggerEvent**
    - **Given:** SwipeDetector component configured
    - **When:** Mouse input simulated from (100, 100) to (150, 150) over 2.0s (≈35 px/s)
    - **Then:** `OnSwipeDetected` event is NOT triggered
    - **Risk:** RISK-015 (MEDIUM)
    - **Priority:** P1
    - **Validation:** Assert event was not invoked

11. **SwipeDetector_MultipleQuickSwipes_TriggersMultipleEvents**
    - **Given:** SwipeDetector component configured
    - **When:** Two fast swipes performed consecutively
    - **Then:** `OnSwipeDetected` event triggered twice with different coordinates
    - **Risk:** RISK-013 (MEDIUM)
    - **Priority:** P1
    - **Purpose:** Validates event system doesn't cache/suppress events

12. **SwipeDetector_TangentialMovement_DoesNotSliceFruit**
    - **Given:** SwipeDetector + Fruit positioned at (200, 200)
    - **When:** Swipe passes near fruit but doesn't intersect (tangent)
    - **Then:** Fruit is not marked as sliced (validates collision manager integration)
    - **Risk:** RISK-016 (LOW)
    - **Priority:** P2
    - **Note:** This is a preview test for STORY-003 integration

---

## Test Data Requirements

### Test Constants

```csharp
// Speed threshold constants
public const float MINIMUM_SWIPE_SPEED = 100f; // px/s
public const float TOLERANCE = 0.01f; // Floating-point comparison tolerance

// Test swipe data
public static readonly Vector2 START_POINT = new Vector2(0, 0);
public static readonly Vector2 END_POINT_200PX = new Vector2(200, 0);
public static readonly Vector2 END_POINT_100PX = new Vector2(100, 0);
public static readonly Vector2 END_POINT_50PX = new Vector2(50, 0);
public static readonly Vector2 END_POINT_DIAGONAL = new Vector2(30, 40); // 50px diagonal

// Time deltas
public const float ONE_SECOND = 1.0f;
public const float HALF_SECOND = 0.5f;
public const float TWO_SECONDS = 2.0f;
```

### Input Simulation Data (Play Mode)

| Test | Start Position | End Position | Duration | Expected Speed | Valid? |
|------|----------------|--------------|----------|----------------|--------|
| TEST-009 | (100, 100) | (300, 300) | 0.5s | ~280 px/s | ✅ Yes |
| TEST-010 | (100, 100) | (150, 150) | 2.0s | ~35 px/s | ❌ No |
| TEST-011 (Swipe 1) | (50, 50) | (250, 50) | 0.8s | 250 px/s | ✅ Yes |
| TEST-011 (Swipe 2) | (300, 300) | (100, 100) | 0.6s | ~330 px/s | ✅ Yes |

---

## Test Execution Strategy

### Execution Order

1. **Phase 1: Edit Mode Tests** (run first, fast feedback)
   - Speed calculation tests (5 tests)
   - Swipe validation tests (3 tests)
   - **Total time:** ~30ms

2. **Phase 2: Play Mode Tests** (run after Edit Mode passes)
   - Input simulation tests (4 tests)
   - **Total time:** ~8 seconds

**Rationale:** Fail fast on calculation/logic errors before expensive input simulation tests

### CI/CD Integration

```yaml
# GitHub Actions test execution
- name: Run Edit Mode Tests (SwipeDetector)
  run: unity-editor -runTests -testPlatform EditMode -testFilter SwipeDetectorTests -testResults results-swipe-editmode.xml
  
- name: Run Play Mode Tests (SwipeDetector)
  if: success()
  run: unity-editor -runTests -testPlatform PlayMode -testFilter SwipeInputIntegrationTests -testResults results-swipe-playmode.xml
```

---

## Test Environment Setup

### Prerequisites

| Requirement | Version | Purpose |
|-------------|---------|---------|
| Unity 6 | 6000.0.25f1 | Game engine |
| Unity Test Framework | 1.4.5+ | Test runner |
| Unity Input System | 1.7.0+ | New Input System package |
| NUnit | 3.x (bundled) | Assertion library |

### Input System Test Setup

**Required for Play Mode tests:**
```csharp
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class SwipeInputIntegrationTests : InputTestFixture
{
    private Mouse mouse;
    private Touchscreen touchscreen;
    
    public override void Setup()
    {
        base.Setup(); // Initialize InputTestFixture
        
        // Add virtual input devices
        mouse = InputSystem.AddDevice<Mouse>();
        touchscreen = InputSystem.AddDevice<Touchscreen>();
    }
    
    public override void TearDown()
    {
        base.TearDown(); // Cleanup input system
    }
}
```

### Setup Steps

1. Verify Unity Input System package is installed:
   - `Window → Package Manager → Input System → 1.7.0+`

2. Create folder structure:
   ```
   Assets/
   ├── Tests/
   │   ├── EditMode/
   │   │   └── Input/
   │   │       └── SwipeDetectorTests.cs
   │   └── PlayMode/
   │       └── Input/
   │           └── SwipeInputIntegrationTests.cs
   ```

3. Create InputManager stub (if not exists) in `Assets/Scripts/Input/`

---

## Test Maintenance Strategy

### When to Update Tests

| Trigger | Action |
|---------|--------|
| Speed threshold changes in GDD | Update MINIMUM_SWIPE_SPEED constant in tests |
| Input system refactor | Update Play Mode input simulation code |
| New input types (touch/stylus) | Add platform-specific integration tests |
| Swipe recording logic changes | Review IsValidSwipe() test assertions |

### Flakiness Prevention

**Common Flaky Patterns:**
- ❌ **Timing issues:** Use `yield return null` between input actions
- ❌ **Event race conditions:** Verify event subscribers exist before simulation
- ❌ **Input device state:** Reset devices in TearDown, use fresh instances per test
- ❌ **Floating-point precision:** Use tolerance (0.01f) for speed comparisons

**Best Practices:**
- ✅ Clear event listeners in `[TearDown]`
- ✅ Use deterministic input sequences (no randomness)
- ✅ Validate preconditions with `Assert.IsNotNull()` before acting
- ✅ Use `InputSystem.Update()` after input changes if needed

---

## Success Criteria

### Definition of Done

- [ ] All 10 tests written and passing locally
- [ ] Edit Mode tests execute in <50ms
- [ ] Play Mode tests execute in <10s
- [ ] Code coverage ≥95% on SwipeDetector.cs
- [ ] No flaky tests (100% pass rate over 10 consecutive runs)
- [ ] Tests pass in CI/CD pipeline (GitHub Actions)
- [ ] Integration with InputManager validated
- [ ] Test documentation complete (this plan + inline comments)

### Quality Gates

| Gate | Criteria | Blocker? |
|------|----------|----------|
| **Unit Test Coverage** | ≥95% on SwipeDetector | Yes |
| **Test Execution Speed** | Edit Mode <50ms | No (warning only) |
| **Test Stability** | 100% pass rate × 10 runs | Yes |
| **CI Integration** | Tests run on PR commits | Yes |
| **Input System Integration** | Mouse simulation working | Yes |

---

## Risk Mitigation

### Identified Risks & Mitigations

| Risk | Mitigation |
|------|------------|
| **Division by zero in speed calc** | Add null/zero check: `if (deltaTime <= 0f) return 0f;` |
| **InputTestFixture not initialized** | Use `base.Setup()` in test class constructor |
| **Event not triggered** | Add debug logging to SwipeDetector, verify event registration |
| **Input device not found** | Add null check after `InputSystem.AddDevice<>()` |
| **Floating-point rounding errors** | Use `Assert.AreEqual(expected, actual, tolerance)` with 0.01f tolerance |

---

## Test Code Scaffolding Preview

### Edit Mode Test Example

```csharp
using NUnit.Framework;
using UnityEngine;
using NinjaFruit;

namespace NinjaFruit.Tests.EditMode
{
    [TestFixture]
    public class SwipeDetectorTests
    {
        private SwipeDetector detector;
        
        [SetUp]
        public void Setup()
        {
            GameObject detectorObject = new GameObject("TestSwipeDetector");
            detector = detectorObject.AddComponent<SwipeDetector>();
        }
        
        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(detector.gameObject);
        }
        
        [Test]
        public void CalculateSwipeSpeed_200Pixels1Second_Returns200PixelsPerSecond()
        {
            // Arrange
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(200, 0);
            float deltaTime = 1.0f;
            
            // Act
            float speed = detector.CalculateSwipeSpeed(start, end, deltaTime);
            
            // Assert
            Assert.AreEqual(200f, speed, 0.01f, "Speed should be 200 px/s (200px / 1s)");
        }
        
        [Test]
        public void IsValidSwipe_Exactly100PixelsPerSecond_ReturnsTrue()
        {
            // Arrange
            List<Vector2> points = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(100, 0)
            };
            float deltaTime = 1.0f;
            
            // Act
            bool isValid = detector.IsValidSwipe(points, deltaTime);
            
            // Assert
            Assert.IsTrue(isValid, "Swipe at exactly 100 px/s should be valid (boundary condition)");
        }
        
        // Additional tests...
    }
}
```

### Play Mode Test Example

```csharp
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using System.Collections;
using NinjaFruit;

namespace NinjaFruit.Tests.PlayMode
{
    [TestFixture]
    public class SwipeInputIntegrationTests : InputTestFixture
    {
        private SwipeDetector detector;
        private Mouse mouse;
        private bool swipeDetected;
        private Vector2 detectedStart;
        private Vector2 detectedEnd;
        
        public override void Setup()
        {
            base.Setup();
            
            // Add virtual mouse device
            mouse = InputSystem.AddDevice<Mouse>();
            Assert.IsNotNull(mouse, "Virtual mouse device should be created");
            
            // Create detector
            GameObject detectorObject = new GameObject("TestSwipeDetector");
            detector = detectorObject.AddComponent<SwipeDetector>();
            
            // Subscribe to event
            swipeDetected = false;
            detector.OnSwipeDetected += (start, end) =>
            {
                swipeDetected = true;
                detectedStart = start;
                detectedEnd = end;
            };
        }
        
        public override void TearDown()
        {
            if (detector != null)
            {
                Object.Destroy(detector.gameObject);
            }
            base.TearDown();
        }
        
        [UnityTest]
        public IEnumerator SwipeDetector_FastMouseSwipe_TriggersOnSwipeDetectedEvent()
        {
            // Arrange
            Vector2 startPos = new Vector2(100, 100);
            Vector2 endPos = new Vector2(300, 300);
            
            // Act - Simulate fast mouse swipe
            Set(mouse.position, startPos);
            Press(mouse.leftButton);
            yield return null; // Wait one frame
            
            Set(mouse.position, endPos);
            yield return null; // Wait one frame
            
            Release(mouse.leftButton);
            yield return null; // Wait one frame for event processing
            
            // Assert
            Assert.IsTrue(swipeDetected, "Fast swipe should trigger OnSwipeDetected event");
            Assert.AreEqual(startPos, detectedStart, "Start position should match");
            Assert.AreEqual(endPos, detectedEnd, "End position should match");
        }
        
        // Additional tests...
    }
}
```

---

## Appendix: Testing Knowledge References

### Relevant BMAD Knowledge Base Articles

- `test-levels-framework.md` - Edit Mode vs Play Mode selection
- `input-testing-patterns.md` - Unity Input System test strategies
- `event-testing.md` - C# event testing best practices
- `risk-governance.md` - Risk scoring methodology

### External Resources

- [Unity Input System Testing](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/manual/Testing.html)
- [InputTestFixture Documentation](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/api/UnityEngine.InputSystem.InputTestFixture.html)
- [NUnit Event Testing](https://stackoverflow.com/questions/tagged/nunit+events)

---

## Integration with Story 001 Tests

**Dependencies:**
- SwipeDetector does NOT depend on FruitSpawner (independent components)
- Future Story 003 (CollisionManager) will integrate both components

**Test Isolation:**
- SwipeDetector tests can run independently of FruitSpawner tests
- No shared state between test suites
- Both can run in parallel in CI/CD

---

**Document Status:** READY FOR IMPLEMENTATION  
**Next Step:** Generate test specifications with detailed Given/When/Then  
**Approval Required:** Test Architect (Murat) - APPROVED ✅

````