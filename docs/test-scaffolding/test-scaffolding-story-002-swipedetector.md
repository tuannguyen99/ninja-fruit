````markdown
# Test Code Scaffolding: STORY-002 - SwipeDetector MVP

**Generated:** 2025-11-28  
**Story:** STORY-002 - SwipeDetector MVP  
**Test Plan:** `test-plan-story-002-swipedetector.md`  
**Test Spec:** `test-spec-story-002-swipedetector.md`

---

## Generated Files

### Edit Mode Tests
- **File:** `Assets/Tests/EditMode/Input/SwipeDetectorTests.cs`
- **Tests:** 8 unit tests (TEST-021 through TEST-028)
- **Coverage:** Speed calculation formulas, swipe validation logic, boundary conditions
- **Framework:** NUnit (Unity Test Framework)
- **Execution:** Edit Mode (no Unity runtime required)
- **Target Duration:** <50ms total

### Play Mode Tests
- **File:** `Assets/Tests/PlayMode/Input/SwipeInputIntegrationTests.cs`
- **Tests:** 4 integration tests (TEST-029 through TEST-032)
- **Coverage:** Input system integration, event triggering, mouse simulation
- **Framework:** NUnit with `[UnityTest]` coroutines + InputTestFixture
- **Execution:** Play Mode (requires Unity runtime + Input System)
- **Target Duration:** <10s total

### Stub Implementation
- **File:** `Assets/Scripts/Input/SwipeDetector.cs`
- **Purpose:** Stub class with method signatures and event declarations
- **Status:** NOT IMPLEMENTED (throws NotImplementedException)
- **Next Step:** Implement methods following TDD workflow (make tests pass)

---

## File Structure Created

```
Assets/
├── Scripts/
│   ├── Input/
│   │   └── SwipeDetector.cs (STUB - contains method signatures)
└── Tests/
    ├── EditMode/
    │   └── Input/
    │       └── SwipeDetectorTests.cs (8 tests)
    └── PlayMode/
        └── Input/
            └── SwipeInputIntegrationTests.cs (4 tests)
```

---

## Implementation Workflow (TDD)

### Step 1: Verify Test Compilation
```bash
# Open Unity Editor
# Window → General → Test Runner
# Verify all 12 tests appear in Test Runner
# All tests should show as "Not Implemented" (expected)
```

### Step 2: Implement CalculateSwipeSpeed()
**Target:** Make TEST-021 through TEST-025 pass

Replace stub in `SwipeDetector.cs`:
```csharp
public float CalculateSwipeSpeed(Vector2 start, Vector2 end, float deltaTime)
{
    // Edge case: Prevent division by zero
    if (deltaTime <= 0f)
    {
        return 0f;
    }
    
    // Calculate Euclidean distance
    float distance = Vector2.Distance(start, end);
    
    // Calculate speed: distance / time
    float speed = distance / deltaTime;
    
    return speed;
}
```

Run tests:
- TEST-021 ✅ (200px/1s → 200 px/s)
- TEST-022 ✅ (100px/1s → 100 px/s boundary)
- TEST-023 ✅ (50px/0.5s → 100 px/s)
- TEST-024 ✅ (30,40 diagonal → 50 px/s)
- TEST-025 ✅ (zero deltaTime → 0 px/s, no exception)

### Step 3: Implement IsValidSwipe()
**Target:** Make TEST-026 through TEST-028 pass

Replace stub in `SwipeDetector.cs`:
```csharp
public bool IsValidSwipe(List<Vector2> points, float deltaTime)
{
    // Validate input
    if (points == null || points.Count < 2)
    {
        return false;
    }
    
    // Get start and end points
    Vector2 start = points[0];
    Vector2 end = points[points.Count - 1];
    
    // Calculate speed
    float speed = CalculateSwipeSpeed(start, end, deltaTime);
    
    // Check against threshold (inclusive: >=)
    return speed >= minimumSwipeSpeed;
}
```

Run tests:
- TEST-026 ✅ (200 px/s → true)
- TEST-027 ✅ (50 px/s → false)
- TEST-028 ✅ (exactly 100 px/s → true, validates >=)

### Step 4: Implement Input Tracking (for Play Mode tests)
**Target:** Make TEST-029 through TEST-032 pass

Add input tracking to `SwipeDetector.cs`:
```csharp
private List<Vector2> currentSwipePoints = new List<Vector2>();
private float swipeStartTime;
private bool isTracking = false;

private void Update()
{
    // Check for input start (mouse/touch down)
    if (Input.GetMouseButtonDown(0)) // Or use New Input System
    {
        StartTracking();
    }
    
    // Track input during swipe
    if (isTracking && Input.GetMouseButton(0))
    {
        TrackPosition(Input.mousePosition);
    }
    
    // Check for input end (mouse/touch up)
    if (Input.GetMouseButtonUp(0))
    {
        EndTracking();
    }
}

private void StartTracking()
{
    isTracking = true;
    swipeStartTime = Time.time;
    currentSwipePoints.Clear();
    currentSwipePoints.Add(Input.mousePosition);
}

private void TrackPosition(Vector2 position)
{
    currentSwipePoints.Add(position);
}

private void EndTracking()
{
    if (!isTracking) return;
    
    float deltaTime = Time.time - swipeStartTime;
    
    if (IsValidSwipe(currentSwipePoints, deltaTime))
    {
        // Trigger event
        Vector2 start = currentSwipePoints[0];
        Vector2 end = currentSwipePoints[currentSwipePoints.Count - 1];
        OnSwipeDetected?.Invoke(start, end);
    }
    
    isTracking = false;
    currentSwipePoints.Clear();
}
```

Run Play Mode tests:
- TEST-029 ✅ (fast mouse swipe triggers event)
- TEST-030 ✅ (slow swipe does NOT trigger event)
- TEST-031 ✅ (multiple swipes trigger multiple events)
- TEST-032 ✅ (tangential movement detected, slicing separate)

### Step 5: Run Full Test Suite
```bash
# In Unity Test Runner
# Mode: EditMode → Run All (should complete in <50ms)
# Mode: PlayMode → Run All (should complete in <10s)
# Expected: 12/12 tests passing ✅
```

---

## Test Execution Instructions

### In Unity Editor
1. Open Unity Test Runner: `Window → General → Test Runner`
2. Switch to `EditMode` tab
3. Click "Run All" to execute Edit Mode tests (8 tests)
4. Switch to `PlayMode` tab
5. Click "Run All" to execute Play Mode tests (4 tests)
6. View results in Test Runner panel

### Via Command Line (CI/CD)
```bash
# Edit Mode tests
Unity.exe -runTests -batchmode -projectPath . \
  -testPlatform EditMode \
  -testFilter SwipeDetector \
  -testResults results-swipe-editmode.xml \
  -logFile -

# Play Mode tests
Unity.exe -runTests -batchmode -projectPath . \
  -testPlatform PlayMode \
  -testFilter SwipeInput \
  -testResults results-swipe-playmode.xml \
  -logFile -
```

### Expected Output
```
EditMode Tests: 8/8 passed (<50ms)
  ✅ TEST-021: CalculateSwipeSpeed_200Pixels1Second_Returns200PixelsPerSecond
  ✅ TEST-022: CalculateSwipeSpeed_100Pixels1Second_Returns100PixelsPerSecond
  ✅ TEST-023: CalculateSwipeSpeed_50Pixels0Point5Seconds_Returns100PixelsPerSecond
  ✅ TEST-024: CalculateSwipeSpeed_DiagonalSwipe_CalculatesEuclideanDistance
  ✅ TEST-025: CalculateSwipeSpeed_ZeroDeltaTime_ReturnsZeroOrHandlesGracefully
  ✅ TEST-026: IsValidSwipe_200PixelsPerSecond_ReturnsTrue
  ✅ TEST-027: IsValidSwipe_50PixelsPerSecond_ReturnsFalse
  ✅ TEST-028: IsValidSwipe_Exactly100PixelsPerSecond_ReturnsTrue

PlayMode Tests: 4/4 passed (<10s)
  ✅ TEST-029: SwipeDetector_FastMouseSwipe_TriggersOnSwipeDetectedEvent
  ✅ TEST-030: SwipeDetector_SlowMouseSwipe_DoesNotTriggerEvent
  ✅ TEST-031: SwipeDetector_MultipleQuickSwipes_TriggersMultipleEvents
  ✅ TEST-032: SwipeDetector_TangentialMovement_DoesNotSliceFruit

Total: 12/12 tests passed ✅
Coverage: 95%+ on SwipeDetector.cs
```

---

## Code Quality Notes

### Test Structure Best Practices
✅ Clear test names (MethodName_Scenario_ExpectedResult)  
✅ AAA pattern (Arrange, Act, Assert)  
✅ XML documentation with test ID, priority, risk  
✅ Tolerance for floating-point comparisons (0.01f)  
✅ Proper Setup/TearDown for test isolation  
✅ Descriptive assertion messages  
✅ Event testing with flag pattern  

### Unity Input System Best Practices
✅ Inherit from InputTestFixture for Play Mode tests  
✅ Call `base.Setup()` and `base.TearDown()` for proper initialization  
✅ Use virtual input devices (Mouse, Touchscreen)  
✅ Set(), Press(), Release() pattern for input simulation  
✅ `yield return null` for frame timing  
✅ Clean up event listeners in TearDown  

### Edge Case Coverage
✅ Zero deltaTime (division by zero protection)  
✅ Exact threshold boundary (100 px/s inclusive)  
✅ Diagonal movement (Euclidean distance)  
✅ Multiple consecutive swipes (event independence)  
✅ Slow swipes (false positive prevention)  

---

## Next Steps After Code Generation

### Immediate (Unity Setup Required)
1. Verify Unity Input System package installed (1.7.0+)
2. Open project in Unity Editor
3. Verify Test Runner shows 12 tests
4. Implement SwipeDetector methods (TDD workflow)
5. Run tests until all 12 pass

### Near-Term (Test Expansion)
6. Generate test scaffolding for STORY-003 (CollisionManager)
7. Integrate SwipeDetector with CollisionManager
8. Create end-to-end tests for swipe → slice flow
9. Add touch input tests (mobile platforms)

### CI/CD Integration (Phase 5)
10. Update GitHub Actions workflows to include SwipeDetector tests
11. Add test result reporting
12. Configure test coverage tracking

---

## Integration with Other Stories

**Dependencies:**
- SwipeDetector → Independent (no dependencies on Story 001)
- Story 003 (CollisionManager) will depend on SwipeDetector's `OnSwipeDetected` event

**Event Flow (Future Integration):**
```
SwipeDetector.OnSwipeDetected
    ↓
CollisionManager.CheckCollisions(start, end)
    ↓
Fruit.Slice() + ScoreManager.AddPoints()
```

**Test Isolation:**
- SwipeDetector tests run independently
- No shared state with FruitSpawner tests
- Can run in parallel in CI/CD

---

## Traceability

| File | Tests | Story AC | Risks | Priority |
|------|-------|----------|-------|----------|
| SwipeDetectorTests.cs | TEST-021 to TEST-025 | AC-2 | RISK-011 | P0 |
| SwipeDetectorTests.cs | TEST-026 to TEST-028 | AC-1, AC-4 | RISK-012, RISK-015 | P0 |
| SwipeInputIntegrationTests.cs | TEST-029 | AC-3 | RISK-014 | P1 |
| SwipeInputIntegrationTests.cs | TEST-030 | AC-5 | RISK-015 | P1 |
| SwipeInputIntegrationTests.cs | TEST-031 | AC-3 | RISK-013 | P1 |
| SwipeInputIntegrationTests.cs | TEST-032 | AC-6 | RISK-016 | P2 |

**Total Coverage:** 100% of STORY-002 acceptance criteria  
**Total Tests:** 12 (8 Edit Mode + 4 Play Mode)  
**Estimated Execution Time:** <50ms Edit Mode + <10s Play Mode = <11s total

---

## Common Gotchas & Solutions

### Gotcha 1: InputTestFixture Not Initialized
**Symptom:** `NullReferenceException` when calling `InputSystem.AddDevice<>()`  
**Solution:** Ensure `base.Setup()` is called in test class Setup method

### Gotcha 2: Events Not Triggering
**Symptom:** TEST-029 fails (event not detected)  
**Solution:** 
- Verify event subscriber is registered BEFORE input simulation
- Add `yield return null` after `Release()` to allow event processing
- Check SwipeDetector's Update() is being called

### Gotcha 3: Floating-Point Precision Errors
**Symptom:** TEST-024 fails on diagonal calculation  
**Solution:** Use tolerance in assertions: `Assert.AreEqual(expected, actual, 0.01f)`

### Gotcha 4: Input Device State Leaking Between Tests
**Symptom:** Flaky tests in TEST-031 (multiple swipes)  
**Solution:** Call `base.TearDown()` to properly clean up input system

---

## TDD Red-Green-Refactor Cycle

### Iteration 1: Speed Calculation
**Red:** TEST-021 fails (stub throws NotImplementedException)  
**Green:** Implement basic distance / time calculation  
**Refactor:** Add edge case handling (zero deltaTime)  
**Result:** TEST-021 through TEST-025 pass ✅

### Iteration 2: Swipe Validation
**Red:** TEST-026 fails (IsValidSwipe not implemented)  
**Green:** Implement threshold comparison (speed >= 100)  
**Refactor:** Extract speed calculation to reuse  
**Result:** TEST-026 through TEST-028 pass ✅

### Iteration 3: Input Integration
**Red:** TEST-029 fails (no input tracking)  
**Green:** Add Update() with mouse input tracking  
**Refactor:** Extract StartTracking/EndTracking methods  
**Result:** TEST-029 through TEST-032 pass ✅

---

## Performance Benchmarks

### Edit Mode Tests (Unit Tests)
| Test | Expected Duration |
|------|-------------------|
| TEST-021 | <5ms |
| TEST-022 | <5ms |
| TEST-023 | <5ms |
| TEST-024 | <5ms |
| TEST-025 | <5ms |
| TEST-026 | <5ms |
| TEST-027 | <5ms |
| TEST-028 | <5ms |
| **Total** | **<50ms** |

### Play Mode Tests (Integration)
| Test | Expected Duration |
|------|-------------------|
| TEST-029 | ~2s (input simulation + frames) |
| TEST-030 | ~3s (slow swipe simulation) |
| TEST-031 | ~3s (two swipes) |
| TEST-032 | ~2s (input + collision check) |
| **Total** | **<10s** |

---

## Code Review Checklist

Before marking Story 002 as complete, verify:

- [ ] All 12 tests written and passing locally
- [ ] All 12 tests passing in CI/CD pipeline
- [ ] Code coverage ≥95% on SwipeDetector.cs
- [ ] No flaky tests (10 consecutive runs at 100% pass rate)
- [ ] Zero deltaTime handled gracefully (no exceptions)
- [ ] Threshold is inclusive (≥ not >)
- [ ] Event triggered for fast swipes
- [ ] Event NOT triggered for slow swipes
- [ ] Multiple swipes work independently
- [ ] Input System package installed and configured
- [ ] Documentation complete (test plan, spec, scaffolding)
- [ ] Code reviewed by peer
- [ ] Merged to main branch

---

**Document Status:** READY FOR IMPLEMENTATION  
**Generated By:** Test Architect (Murat)  
**Approval:** APPROVED ✅  
**Next Action:** Open Unity Editor, verify test compilation, begin TDD implementation

**Estimated Time to Complete:** 2-3 hours (including implementation + test validation)

````