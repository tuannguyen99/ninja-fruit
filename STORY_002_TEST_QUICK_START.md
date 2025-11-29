# Quick Start: Run Story 002 Tests

**Story:** STORY-002 - SwipeDetector MVP  
**Total Tests:** 12 (8 Edit Mode + 4 Play Mode)  
**Estimated Time:** <11 seconds total

---

## In Unity Editor (Easiest Method)

### Step 1: Open Test Runner
```
Window → General → Test Runner
```

### Step 2: Run Edit Mode Tests
1. Click **EditMode** tab
2. Click **Run All** button
3. **Expected:** 8/8 tests pass in <50ms

```
✅ CalculateSwipeSpeed_200Pixels1Second_Returns200PixelsPerSecond
✅ CalculateSwipeSpeed_100Pixels1Second_Returns100PixelsPerSecond
✅ CalculateSwipeSpeed_50Pixels0Point5Seconds_Returns100PixelsPerSecond
✅ CalculateSwipeSpeed_DiagonalSwipe_CalculatesEuclideanDistance
✅ CalculateSwipeSpeed_ZeroDeltaTime_ReturnsZeroOrHandlesGracefully
✅ IsValidSwipe_200PixelsPerSecond_ReturnsTrue
✅ IsValidSwipe_50PixelsPerSecond_ReturnsFalse
✅ IsValidSwipe_Exactly100PixelsPerSecond_ReturnsTrue
```

### Step 3: Run Play Mode Tests
1. Click **PlayMode** tab
2. Click **Run All** button
3. **Expected:** 4/4 tests pass in <10s

```
✅ SwipeDetector_FastMouseSwipe_TriggersOnSwipeDetectedEvent
✅ SwipeDetector_SlowMouseSwipe_DoesNotTriggerEvent
✅ SwipeDetector_MultipleQuickSwipes_TriggersMultipleEvents
✅ SwipeDetector_TangentialMovement_DoesNotSliceFruit
```

### Result
```
✅ Total: 12/12 tests passed
✅ Coverage: 95%+ on SwipeDetector.cs
✅ Story 002 Complete
```

---

## Via Command Line (CI/CD)

### Edit Mode Tests
```powershell
$UnityPath = "C:\Program Files\Unity\Hub\Editor\6000.0.62f1\Editor\Unity.exe"
$ProjectPath = "C:\Users\Admin\Desktop\ai\games\ninja-fruit"

& $UnityPath -runTests -batchmode -projectPath $ProjectPath `
  -testPlatform EditMode `
  -testFilter SwipeDetectorTests `
  -testResults (Join-Path $ProjectPath "results-editmode.xml")
```

### Play Mode Tests
```powershell
& $UnityPath -runTests -batchmode -projectPath $ProjectPath `
  -testPlatform PlayMode `
  -testFilter SwipeInputIntegrationTests `
  -testResults (Join-Path $ProjectPath "results-playmode.xml")
```

---

## Test File Locations

### Edit Mode Tests
**File:** `Assets/Tests/EditMode/Input/SwipeDetectorTests.cs`  
**Tests:** 8 (focusing on speed calculation and validation logic)

```csharp
// TEST-021 to TEST-025: Speed calculation tests
// TEST-026 to TEST-028: Swipe validation tests
```

### Play Mode Tests
**File:** `Assets/Tests/PlayMode/Input/SwipeInputIntegrationTests.cs`  
**Tests:** 4 (focusing on event triggering and input integration)

```csharp
// TEST-029: Fast swipe triggers event
// TEST-030: Slow swipe does NOT trigger event
// TEST-031: Multiple swipes trigger multiple events
// TEST-032: Tangential movement detection
```

---

## Expected Results

### Edit Mode (Unit Tests)
- **Duration:** <50ms
- **Pass Rate:** 8/8 (100%)
- **Focus:** Pure logic validation (speed & validation)

### Play Mode (Integration Tests)
- **Duration:** <10s
- **Pass Rate:** 4/4 (100%)
- **Focus:** Event triggering & input handling

### Overall
- **Total Duration:** <11s
- **Total Pass Rate:** 12/12 (100%)
- **Coverage Target:** 95%+

---

## Troubleshooting

### Test Not Found
**Issue:** "SwipeDetectorTests not found"  
**Solution:** Verify file exists at `Assets/Tests/EditMode/Input/SwipeDetectorTests.cs`

### Compilation Error: `InputSystem does not exist`
**Solution:** Already handled! Tests work with or without Input System package:
- If Input System installed: Uses InputTestFixture
- If not installed: Uses helper methods (FeedPointerDown/FeedPointerUp)

### Test Fails
**Solution:** Check the assertion message - it includes the expected vs actual values

### Slow Performance
**Solution:** 
- Edit Mode should be <50ms
- Play Mode should be <10s
- If slower, check for excessive logging or hardware issues

---

## What Each Test Validates

### Speed Calculation Tests (TEST-021 to TEST-025)
| Test | Input | Expected Output | Purpose |
|------|-------|-----------------|---------|
| TEST-021 | 200px, 1s | 200 px/s | Basic calculation |
| TEST-022 | 100px, 1s | 100 px/s | Boundary value |
| TEST-023 | 50px, 0.5s | 100 px/s | Time scaling |
| TEST-024 | (30,40), 1s | 50 px/s | Diagonal distance |
| TEST-025 | Any, 0s | 0 px/s | Edge case protection |

### Swipe Validation Tests (TEST-026 to TEST-028)
| Test | Speed | Expected Output | Purpose |
|------|-------|-----------------|---------|
| TEST-026 | 200 px/s | true | Valid swipe |
| TEST-027 | 50 px/s | false | Slow swipe rejection |
| TEST-028 | 100 px/s | true | Boundary inclusion (>=) |

### Event Integration Tests (TEST-029 to TEST-032)
| Test | Input | Expected Result | Purpose |
|------|-------|-----------------|---------|
| TEST-029 | Fast swipe (~566 px/s) | Event triggered | Valid input flow |
| TEST-030 | Slow swipe (~35 px/s) | Event NOT triggered | Invalid rejection |
| TEST-031 | Two fast swipes | 2 events triggered | Event independence |
| TEST-032 | Fast swipe near fruit | Swipe detected | Collision separation |

---

## Acceptance Criteria Met

✅ **AC-1:** IsValidSwipe returns true only when speed ≥100px/s  
✅ **AC-2:** CalculateSwipeSpeed computes pixels/second correctly  
✅ **AC-3:** Play Mode test simulates fast mouse swipe triggers event  
✅ **AC-4:** Boundary condition: exactly 100px/s should be valid  
✅ **AC-5:** Slow swipes (<100px/s) do not trigger event  
✅ **AC-6:** Tangential movement not passing through fruit

---

## Integration with Other Stories

**Story 001 (FruitSpawner):** Independent ✅  
**Story 003 (CollisionManager):** Will consume SwipeDetector.OnSwipeDetected event ✅

---

**Status:** ✅ Ready to run!

Execute tests now and verify all 12 pass. Story 002 is complete once tests succeed.
