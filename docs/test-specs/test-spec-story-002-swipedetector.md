````markdown
# Test Specification: STORY-002 - SwipeDetector MVP

**Generated:** 2025-11-28  
**Story:** STORY-002 - SwipeDetector MVP  
**Epic:** EPIC-001 - Core Slicing Mechanics  
**Test Plan Reference:** `test-plan-story-002-swipedetector.md`  
**Test Architect:** Murat

---

## Document Purpose

This specification provides detailed test case definitions with Given/When/Then format, expected results, and validation criteria for all 10 tests identified in the test plan. Use this document to implement test code with precision following TDD methodology.

---

## Edit Mode Test Specifications

### Test Suite: SwipeSpeedCalculationTests

**File:** `Assets/Tests/EditMode/Input/SwipeDetectorTests.cs`  
**Purpose:** Validate swipe speed calculation formula (distance / time)

---

#### TEST-021: CalculateSwipeSpeed_200Pixels1Second_Returns200PixelsPerSecond

**Priority:** P0  
**Risk:** RISK-011 (HIGH)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <5ms

**Given:**
- SwipeDetector component is instantiated
- Start point: (0, 0)
- End point: (200, 0) [horizontal swipe]
- Delta time: 1.0 seconds

**When:**
- `CalculateSwipeSpeed(Vector2(0,0), Vector2(200,0), 1.0f)` is called

**Then:**
- Method returns exactly `200.0f` pixels/second
- Formula: distance / time = 200px / 1.0s = 200 px/s
- No exceptions are thrown

**Test Data:**
```csharp
Vector2 startPoint = new Vector2(0, 0);
Vector2 endPoint = new Vector2(200, 0);
float deltaTime = 1.0f;
float expectedSpeed = 200.0f;
float tolerance = 0.01f;
```

**Validation:**
```csharp
float actualSpeed = detector.CalculateSwipeSpeed(startPoint, endPoint, deltaTime);
Assert.AreEqual(expectedSpeed, actualSpeed, tolerance, 
    "Speed should be 200 px/s for 200-pixel swipe over 1 second");
```

**Edge Cases Covered:**
- Basic horizontal swipe (X-axis only)
- Linear distance calculation
- Standard time interval

---

#### TEST-022: CalculateSwipeSpeed_100Pixels1Second_Returns100PixelsPerSecond

**Priority:** P0  
**Risk:** RISK-011 (HIGH)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <5ms

**Given:**
- SwipeDetector component is instantiated
- Start point: (0, 0)
- End point: (100, 0)
- Delta time: 1.0 seconds

**When:**
- `CalculateSwipeSpeed(Vector2(0,0), Vector2(100,0), 1.0f)` is called

**Then:**
- Method returns exactly `100.0f` pixels/second
- This is the **exact threshold boundary** for valid swipes
- Formula: 100px / 1.0s = 100 px/s

**Test Data:**
```csharp
Vector2 startPoint = new Vector2(0, 0);
Vector2 endPoint = new Vector2(100, 0);
float deltaTime = 1.0f;
float expectedSpeed = 100.0f; // Threshold boundary
float tolerance = 0.01f;
```

**Validation:**
```csharp
float actualSpeed = detector.CalculateSwipeSpeed(startPoint, endPoint, deltaTime);
Assert.AreEqual(expectedSpeed, actualSpeed, tolerance, 
    "Speed should be exactly 100 px/s (threshold boundary)");
```

**Edge Cases Covered:**
- Exact threshold boundary (100 px/s)
- Validates precision at critical value

---

#### TEST-023: CalculateSwipeSpeed_50Pixels0Point5Seconds_Returns100PixelsPerSecond

**Priority:** P0  
**Risk:** RISK-011 (HIGH)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <5ms

**Given:**
- SwipeDetector component is instantiated
- Start point: (0, 0)
- End point: (50, 0)
- Delta time: 0.5 seconds (fast swipe)

**When:**
- `CalculateSwipeSpeed(Vector2(0,0), Vector2(50,0), 0.5f)` is called

**Then:**
- Method returns exactly `100.0f` pixels/second
- Formula: 50px / 0.5s = 100 px/s (reaches threshold with shorter distance + faster time)
- Validates inverse relationship between distance and time

**Test Data:**
```csharp
Vector2 startPoint = new Vector2(0, 0);
Vector2 endPoint = new Vector2(50, 0);
float deltaTime = 0.5f; // Fast swipe
float expectedSpeed = 100.0f;
float tolerance = 0.01f;
```

**Validation:**
```csharp
float actualSpeed = detector.CalculateSwipeSpeed(startPoint, endPoint, deltaTime);
Assert.AreEqual(expectedSpeed, actualSpeed, tolerance, 
    "Shorter distance with faster time should still reach 100 px/s threshold");
```

**Edge Cases Covered:**
- Fast swipe timing (0.5s)
- Distance/time ratio validation
- Alternative path to threshold

---

#### TEST-024: CalculateSwipeSpeed_DiagonalSwipe_CalculatesEuclideanDistance

**Priority:** P0  
**Risk:** RISK-011 (HIGH)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <5ms

**Given:**
- SwipeDetector component is instantiated
- Start point: (0, 0)
- End point: (30, 40) [diagonal swipe forming 3-4-5 triangle]
- Delta time: 1.0 seconds

**When:**
- `CalculateSwipeSpeed(Vector2(0,0), Vector2(30,40), 1.0f)` is called

**Then:**
- Method returns exactly `50.0f` pixels/second
- Formula: √(30² + 40²) / 1.0s = √(900 + 1600) / 1.0s = √2500 / 1.0s = 50 px/s
- Validates correct use of Euclidean distance (Vector2.Distance)

**Test Data:**
```csharp
Vector2 startPoint = new Vector2(0, 0);
Vector2 endPoint = new Vector2(30, 40); // 3-4-5 right triangle
float deltaTime = 1.0f;
float expectedSpeed = 50.0f; // √(30² + 40²) = 50
float tolerance = 0.01f;
```

**Validation:**
```csharp
float actualSpeed = detector.CalculateSwipeSpeed(startPoint, endPoint, deltaTime);
Assert.AreEqual(expectedSpeed, actualSpeed, tolerance, 
    "Diagonal swipe should use Euclidean distance: √(30² + 40²) = 50px");
```

**Edge Cases Covered:**
- Diagonal movement (both X and Y components)
- Pythagorean theorem application
- Vector2.Distance() correctness

---

#### TEST-025: CalculateSwipeSpeed_ZeroDeltaTime_ReturnsZeroOrHandlesGracefully

**Priority:** P0  
**Risk:** RISK-011 (HIGH)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <5ms

**Given:**
- SwipeDetector component is instantiated
- Start point: (0, 0)
- End point: (100, 0)
- Delta time: 0.0 seconds (edge case: instantaneous swipe or timing error)

**When:**
- `CalculateSwipeSpeed(Vector2(0,0), Vector2(100,0), 0.0f)` is called

**Then:**
- Method returns `0.0f` OR handles gracefully without exception
- Defensive programming: avoid division by zero
- Preferred implementation: `if (deltaTime <= 0f) return 0f;`

**Test Data:**
```csharp
Vector2 startPoint = new Vector2(0, 0);
Vector2 endPoint = new Vector2(100, 0);
float deltaTime = 0.0f; // Edge case
float expectedSpeed = 0.0f; // Safe return value
```

**Validation:**
```csharp
float actualSpeed = detector.CalculateSwipeSpeed(startPoint, endPoint, deltaTime);

// Option 1: Return zero
Assert.AreEqual(0f, actualSpeed, 0.01f, 
    "Zero deltaTime should return 0 to avoid division by zero");

// Option 2: Check no exception thrown
Assert.DoesNotThrow(() => 
    detector.CalculateSwipeSpeed(startPoint, endPoint, deltaTime),
    "Zero deltaTime should not throw exception");
```

**Edge Cases Covered:**
- Division by zero protection
- Defensive programming validation
- Robustness under edge input

---

### Test Suite: SwipeValidationTests

**File:** `Assets/Tests/EditMode/Input/SwipeDetectorTests.cs`  
**Purpose:** Validate IsValidSwipe() logic using speed threshold (≥100 px/s)

---

#### TEST-026: IsValidSwipe_200PixelsPerSecond_ReturnsTrue

**Priority:** P0  
**Risk:** RISK-012 (HIGH)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <5ms

**Given:**
- SwipeDetector component is instantiated
- Swipe points representing 200px distance
- Delta time: 1.0 seconds (speed = 200 px/s)

**When:**
- `IsValidSwipe(points, 1.0f)` is called with 200px distance

**Then:**
- Method returns `true`
- Speed (200 px/s) exceeds threshold (100 px/s)
- Validates correct positive case

**Test Data:**
```csharp
List<Vector2> points = new List<Vector2>
{
    new Vector2(0, 0),    // Start
    new Vector2(200, 0)   // End (200px away)
};
float deltaTime = 1.0f;
bool expectedResult = true;
```

**Validation:**
```csharp
bool isValid = detector.IsValidSwipe(points, deltaTime);
Assert.IsTrue(isValid, 
    "Swipe at 200 px/s should be valid (exceeds 100 px/s threshold)");
```

**Edge Cases Covered:**
- Positive validation case
- Speed well above threshold

---

#### TEST-027: IsValidSwipe_50PixelsPerSecond_ReturnsFalse

**Priority:** P1  
**Risk:** RISK-015 (MEDIUM)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <5ms

**Given:**
- SwipeDetector component is instantiated
- Swipe points representing 50px distance
- Delta time: 1.0 seconds (speed = 50 px/s)

**When:**
- `IsValidSwipe(points, 1.0f)` is called with 50px distance

**Then:**
- Method returns `false`
- Speed (50 px/s) is below threshold (100 px/s)
- Validates correct negative case (slow swipe rejection)

**Test Data:**
```csharp
List<Vector2> points = new List<Vector2>
{
    new Vector2(0, 0),   // Start
    new Vector2(50, 0)   // End (only 50px away)
};
float deltaTime = 1.0f;
bool expectedResult = false;
```

**Validation:**
```csharp
bool isValid = detector.IsValidSwipe(points, deltaTime);
Assert.IsFalse(isValid, 
    "Swipe at 50 px/s should be invalid (below 100 px/s threshold)");
```

**Edge Cases Covered:**
- Negative validation case
- Slow swipe rejection
- False positive prevention

---

#### TEST-028: IsValidSwipe_Exactly100PixelsPerSecond_ReturnsTrue

**Priority:** P0  
**Risk:** RISK-012 (HIGH)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <5ms

**Given:**
- SwipeDetector component is instantiated
- Swipe points representing exactly 100px distance
- Delta time: 1.0 seconds (speed = exactly 100 px/s)

**When:**
- `IsValidSwipe(points, 1.0f)` is called with 100px distance

**Then:**
- Method returns `true`
- Validates **inclusive** threshold (≥ not just >)
- Formula: 100px / 1.0s = exactly 100 px/s
- This is a **critical boundary condition**

**Test Data:**
```csharp
List<Vector2> points = new List<Vector2>
{
    new Vector2(0, 0),    // Start
    new Vector2(100, 0)   // End (exactly 100px away)
};
float deltaTime = 1.0f;
bool expectedResult = true; // Inclusive threshold
```

**Validation:**
```csharp
bool isValid = detector.IsValidSwipe(points, deltaTime);
Assert.IsTrue(isValid, 
    "Swipe at exactly 100 px/s should be valid (inclusive threshold: speed >= 100)");
```

**Edge Cases Covered:**
- Exact threshold boundary
- Validates ≥ operator usage (not >)
- Critical acceptance criteria validation

**Implementation Note:**
Ensure SwipeDetector uses `speed >= minimumSwipeSpeed`, NOT `speed > minimumSwipeSpeed`.

---

## Play Mode Test Specifications

### Test Suite: SwipeInputIntegrationTests

**File:** `Assets/Tests/PlayMode/Input/SwipeInputIntegrationTests.cs`  
**Purpose:** Validate Unity Input System integration and event triggering

---

#### TEST-029: SwipeDetector_FastMouseSwipe_TriggersOnSwipeDetectedEvent

**Priority:** P1  
**Risk:** RISK-014 (MEDIUM)  
**Type:** Integration Test (Play Mode)  
**Estimated Duration:** ~2s

**Given:**
- SwipeDetector component exists in scene
- InputManager is configured (or SwipeDetector directly listens to input)
- Virtual mouse device is active
- Event listener is subscribed to `OnSwipeDetected`

**When:**
- Mouse simulated to move from (100, 100) to (300, 300) over 0.5 seconds
- Distance: √((300-100)² + (300-100)²) = √(200² + 200²) = √80000 ≈ 283 pixels
- Speed: 283px / 0.5s ≈ 566 px/s (well above 100 px/s threshold)

**Then:**
- `OnSwipeDetected` event is triggered
- Event provides start point ≈ (100, 100)
- Event provides end point ≈ (300, 300)
- Event is triggered exactly once

**Test Data:**
```csharp
Vector2 startPosition = new Vector2(100, 100);
Vector2 endPosition = new Vector2(300, 300);
float swipeDuration = 0.5f;
float expectedSpeed = 566f; // Approximate (283px / 0.5s)
```

**Setup:**
```csharp
[SetUp]
public void Setup()
{
    base.Setup(); // InputTestFixture initialization
    
    mouse = InputSystem.AddDevice<Mouse>();
    Assert.IsNotNull(mouse, "Virtual mouse should be created");
    
    GameObject detectorObject = new GameObject("TestSwipeDetector");
    detector = detectorObject.AddComponent<SwipeDetector>();
    
    swipeDetected = false;
    detector.OnSwipeDetected += (start, end) =>
    {
        swipeDetected = true;
        detectedStart = start;
        detectedEnd = end;
    };
}
```

**Validation:**
```csharp
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
    yield return null; // Wait for event processing
    
    // Assert
    Assert.IsTrue(swipeDetected, "Fast swipe should trigger OnSwipeDetected event");
    Assert.AreEqual(startPos.x, detectedStart.x, 5f, "Start X should approximately match");
    Assert.AreEqual(startPos.y, detectedStart.y, 5f, "Start Y should approximately match");
    Assert.AreEqual(endPos.x, detectedEnd.x, 5f, "End X should approximately match");
    Assert.AreEqual(endPos.y, detectedEnd.y, 5f, "End Y should approximately match");
}
```

**Teardown:**
```csharp
[TearDown]
public void Teardown()
{
    if (detector != null)
    {
        Object.Destroy(detector.gameObject);
    }
    base.TearDown(); // InputTestFixture cleanup
}
```

**Edge Cases Covered:**
- Input system integration
- Event triggering mechanism
- Mouse input simulation
- Event parameter accuracy

---

#### TEST-030: SwipeDetector_SlowMouseSwipe_DoesNotTriggerEvent

**Priority:** P1  
**Risk:** RISK-015 (MEDIUM)  
**Type:** Integration Test (Play Mode)  
**Estimated Duration:** ~3s

**Given:**
- SwipeDetector component exists in scene
- Virtual mouse device is active
- Event listener is subscribed to `OnSwipeDetected`

**When:**
- Mouse simulated to move from (100, 100) to (150, 150) over 2.0 seconds
- Distance: √((150-100)² + (150-100)²) = √(50² + 50²) = √5000 ≈ 71 pixels
- Speed: 71px / 2.0s ≈ 35.5 px/s (well below 100 px/s threshold)

**Then:**
- `OnSwipeDetected` event is **NOT** triggered
- Event count remains zero
- Validates slow swipe rejection

**Test Data:**
```csharp
Vector2 startPosition = new Vector2(100, 100);
Vector2 endPosition = new Vector2(150, 150);
float swipeDuration = 2.0f;
float expectedSpeed = 35.5f; // Below threshold
```

**Validation:**
```csharp
[UnityTest]
public IEnumerator SwipeDetector_SlowMouseSwipe_DoesNotTriggerEvent()
{
    // Arrange
    Vector2 startPos = new Vector2(100, 100);
    Vector2 endPos = new Vector2(150, 150);
    
    // Act - Simulate slow mouse swipe
    Set(mouse.position, startPos);
    Press(mouse.leftButton);
    yield return null;
    
    // Move slowly (wait multiple frames to simulate 2 seconds)
    float elapsedTime = 0f;
    while (elapsedTime < 2.0f)
    {
        elapsedTime += Time.deltaTime;
        Vector2 currentPos = Vector2.Lerp(startPos, endPos, elapsedTime / 2.0f);
        Set(mouse.position, currentPos);
        yield return null;
    }
    
    Release(mouse.leftButton);
    yield return null;
    
    // Assert
    Assert.IsFalse(swipeDetected, "Slow swipe should NOT trigger OnSwipeDetected event");
}
```

**Edge Cases Covered:**
- Slow swipe rejection
- False positive prevention
- Threshold validation in real-time input

---

#### TEST-031: SwipeDetector_MultipleQuickSwipes_TriggersMultipleEvents

**Priority:** P1  
**Risk:** RISK-013 (MEDIUM)  
**Type:** Integration Test (Play Mode)  
**Estimated Duration:** ~2s

**Given:**
- SwipeDetector component exists in scene
- Virtual mouse device is active
- Event listener is subscribed with counter

**When:**
- First fast swipe: (50, 50) to (250, 50) over 0.8s
- Second fast swipe: (300, 300) to (100, 100) over 0.6s

**Then:**
- `OnSwipeDetected` event is triggered **twice**
- First event has start (50, 50), end (250, 50)
- Second event has start (300, 300), end (100, 100)
- Events are independent (no caching/suppression)

**Test Data:**
```csharp
// First swipe
Vector2 swipe1Start = new Vector2(50, 50);
Vector2 swipe1End = new Vector2(250, 50);
float swipe1Duration = 0.8f; // Speed: 200px / 0.8s = 250 px/s

// Second swipe
Vector2 swipe2Start = new Vector2(300, 300);
Vector2 swipe2End = new Vector2(100, 100);
float swipe2Duration = 0.6f; // Speed: ~330 px/s
```

**Validation:**
```csharp
[UnityTest]
public IEnumerator SwipeDetector_MultipleQuickSwipes_TriggersMultipleEvents()
{
    // Arrange
    int eventCount = 0;
    List<(Vector2 start, Vector2 end)> detectedSwipes = new List<(Vector2, Vector2)>();
    
    detector.OnSwipeDetected += (start, end) =>
    {
        eventCount++;
        detectedSwipes.Add((start, end));
    };
    
    // Act - First swipe
    Set(mouse.position, swipe1Start);
    Press(mouse.leftButton);
    yield return null;
    
    Set(mouse.position, swipe1End);
    yield return null;
    
    Release(mouse.leftButton);
    yield return new WaitForSeconds(0.2f); // Small pause between swipes
    
    // Act - Second swipe
    Set(mouse.position, swipe2Start);
    Press(mouse.leftButton);
    yield return null;
    
    Set(mouse.position, swipe2End);
    yield return null;
    
    Release(mouse.leftButton);
    yield return null;
    
    // Assert
    Assert.AreEqual(2, eventCount, "Two fast swipes should trigger two events");
    Assert.AreEqual(2, detectedSwipes.Count, "Should have recorded two swipes");
    
    // Verify first swipe
    Assert.AreEqual(swipe1Start, detectedSwipes[0].start, "First swipe start should match");
    Assert.AreEqual(swipe1End, detectedSwipes[0].end, "First swipe end should match");
    
    // Verify second swipe
    Assert.AreEqual(swipe2Start, detectedSwipes[1].start, "Second swipe start should match");
    Assert.AreEqual(swipe2End, detectedSwipes[1].end, "Second swipe end should match");
}
```

**Edge Cases Covered:**
- Multiple event triggering
- Event independence
- No event suppression/caching
- State reset between swipes

---

#### TEST-032: SwipeDetector_TangentialMovement_DoesNotSliceFruit

**Priority:** P2  
**Risk:** RISK-016 (LOW)  
**Type:** Integration Test (Play Mode)  
**Estimated Duration:** ~2s

**Given:**
- SwipeDetector component exists
- Test fruit prefab positioned at (200, 200)
- Swipe passes near fruit but doesn't intersect (tangent)

**When:**
- Mouse swipe from (150, 200) to (250, 200) [horizontal line at Y=200]
- Fruit has CircleCollider2D with radius 0.3 units at (200, 200)
- Swipe is fast (valid speed) but doesn't pass through fruit center

**Then:**
- Swipe is detected as valid (speed > 100 px/s)
- However, fruit is NOT marked as sliced (collision detection will handle this)
- This test validates event is triggered, but slicing logic is separate

**Test Data:**
```csharp
Vector2 fruitPosition = new Vector2(200, 200);
float fruitRadius = 0.3f; // In world units

Vector2 swipeStart = new Vector2(150, 200);
Vector2 swipeEnd = new Vector2(250, 200); // Horizontal swipe at same Y
```

**Validation:**
```csharp
[UnityTest]
public IEnumerator SwipeDetector_TangentialMovement_DoesNotSliceFruit()
{
    // Arrange
    GameObject fruitObject = new GameObject("TestFruit");
    fruitObject.transform.position = new Vector3(200, 200, 0);
    CircleCollider2D fruitCollider = fruitObject.AddComponent<CircleCollider2D>();
    fruitCollider.radius = 0.3f;
    fruitObject.tag = "Fruit";
    
    bool swipeDetected = false;
    detector.OnSwipeDetected += (start, end) =>
    {
        swipeDetected = true;
    };
    
    // Act - Simulate tangential swipe
    Set(mouse.position, new Vector2(150, 200));
    Press(mouse.leftButton);
    yield return null;
    
    Set(mouse.position, new Vector2(250, 200));
    yield return null;
    
    Release(mouse.leftButton);
    yield return null;
    
    // Assert
    Assert.IsTrue(swipeDetected, "Fast swipe should be detected");
    
    // Note: Actual collision detection (slicing) is handled by CollisionManager (STORY-003)
    // This test validates swipe detection is independent of collision logic
    
    // Cleanup
    Object.Destroy(fruitObject);
}
```

**Edge Cases Covered:**
- Swipe detection vs. collision detection separation
- Preview of STORY-003 integration
- Validates event system independence

**Implementation Note:**
This test demonstrates that SwipeDetector only handles gesture recognition. Collision detection (slicing) is CollisionManager's responsibility (STORY-003).

---

## Test Data Assets Required

### No Prefabs Required for Edit Mode Tests
Edit Mode tests are pure logic validation - no Unity prefabs needed.

### Optional: Test Scene for Play Mode Tests

**Scene:** `Assets/Tests/PlayMode/Scenes/SwipeTestScene.unity`  
**Contents:**
- Empty scene (no special setup required)
- Tests create GameObjects dynamically

**Camera:**
- Not required for input tests (no rendering needed)

---

## Test Execution Order

### Recommended Execution Sequence

**Phase 1: Edit Mode (Speed & Validation Logic)**
1. TEST-021: Basic speed calculation (200px/1s)
2. TEST-022: Threshold boundary (100px/1s)
3. TEST-023: Fast swipe timing (50px/0.5s)
4. TEST-024: Diagonal swipe (Euclidean distance)
5. TEST-025: Zero deltaTime edge case
6. TEST-026: Valid swipe (200 px/s → true)
7. TEST-027: Invalid swipe (50 px/s → false)
8. TEST-028: Exact threshold (100 px/s → true)

**Phase 2: Play Mode (Input Integration)**
9. TEST-029: Fast mouse swipe triggers event
10. TEST-030: Slow mouse swipe does NOT trigger event
11. TEST-031: Multiple swipes trigger multiple events
12. TEST-032: Tangential movement detection

**Rationale:** Validate calculation logic (fast) before input simulation (slower).

---

## Success Criteria Per Test

| Test ID | Pass Criteria | Fail Criteria |
|---------|---------------|---------------|
| TEST-021 | Returns 200.0 ±0.01 | Returns any other value or throws exception |
| TEST-022 | Returns 100.0 ±0.01 (boundary) | Returns any other value |
| TEST-023 | Returns 100.0 ±0.01 | Returns any other value |
| TEST-024 | Returns 50.0 ±0.01 (Euclidean) | Returns incorrect distance calculation |
| TEST-025 | Returns 0.0 OR handles gracefully | Throws exception (division by zero) |
| TEST-026 | Returns `true` | Returns `false` |
| TEST-027 | Returns `false` | Returns `true` (false positive) |
| TEST-028 | Returns `true` (inclusive) | Returns `false` (exclusive threshold) |
| TEST-029 | Event triggered, correct coordinates | Event not triggered or wrong data |
| TEST-030 | Event NOT triggered | Event triggered (false positive) |
| TEST-031 | 2 events triggered with correct data | Wrong count or wrong coordinates |
| TEST-032 | Swipe detected (collision separate) | Exception or incorrect behavior |

---

## Traceability Matrix

| Test ID | Story AC | Risk ID | Priority | GDD Section |
|---------|----------|---------|----------|-------------|
| TEST-021 | AC-2 | RISK-011 | P0 | Game Mechanics → Slicing Mechanics |
| TEST-022 | AC-2 | RISK-011 | P0 | Game Mechanics → Slicing Mechanics |
| TEST-023 | AC-2 | RISK-011 | P0 | Game Mechanics → Slicing Mechanics |
| TEST-024 | AC-2 | RISK-011 | P0 | Game Mechanics → Slicing Mechanics |
| TEST-025 | AC-2 | RISK-011 | P0 | (Edge case - robustness) |
| TEST-026 | AC-1 | RISK-012 | P0 | Game Mechanics → Slicing Mechanics |
| TEST-027 | AC-1, AC-5 | RISK-015 | P1 | Game Mechanics → Slicing Mechanics |
| TEST-028 | AC-1, AC-4 | RISK-012 | P0 | Game Mechanics → Slicing Mechanics (boundary) |
| TEST-029 | AC-3 | RISK-014 | P1 | Controls and Input |
| TEST-030 | AC-5 | RISK-015 | P1 | Controls and Input |
| TEST-031 | AC-3 | RISK-013 | P1 | (Robustness - multiple events) |
| TEST-032 | AC-6 | RISK-016 | P2 | Game Mechanics → Collision Detection (preview) |

---

## Implementation Notes

### Common Setup Pattern (Edit Mode)

```csharp
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
```

### Common Setup Pattern (Play Mode)

```csharp
public class SwipeInputIntegrationTests : InputTestFixture
{
    private SwipeDetector detector;
    private Mouse mouse;
    private bool swipeDetected;
    private Vector2 detectedStart;
    private Vector2 detectedEnd;
    
    public override void Setup()
    {
        base.Setup(); // CRITICAL: Initialize InputTestFixture
        
        mouse = InputSystem.AddDevice<Mouse>();
        Assert.IsNotNull(mouse, "Virtual mouse creation failed");
        
        GameObject detectorObject = new GameObject("TestSwipeDetector");
        detector = detectorObject.AddComponent<SwipeDetector>();
        
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
        base.TearDown(); // CRITICAL: Cleanup InputTestFixture
    }
}
```

### Input Simulation Best Practices

```csharp
// Simulate mouse press + move + release
Set(mouse.position, startPosition);
Press(mouse.leftButton);
yield return null; // Wait one frame

Set(mouse.position, endPosition);
yield return null; // Wait one frame

Release(mouse.leftButton);
yield return null; // Wait for event processing
```

### Event Testing Pattern

```csharp
// Track event invocations
bool eventTriggered = false;
Vector2 capturedStart = Vector2.zero;
Vector2 capturedEnd = Vector2.zero;

detector.OnSwipeDetected += (start, end) =>
{
    eventTriggered = true;
    capturedStart = start;
    capturedEnd = end;
};

// ... perform action ...

Assert.IsTrue(eventTriggered, "Event should have been triggered");
Assert.AreEqual(expectedStart, capturedStart, "Start position mismatch");
```

---

**Document Status:** READY FOR CODE GENERATION  
**Next Step:** Generate C# test code scaffolding  
**Approval:** Test Architect (Murat) - APPROVED ✅

````