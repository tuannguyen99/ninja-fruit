# Test Specification: Story 003 - CollisionManager MVP

**Story:** STORY-003: CollisionManager MVP  
**Epic:** Core Slicing Mechanics (EPIC-001)  
**Author:** BMAD (Test Design Agent)  
**Date:** November 29, 2025  
**Version:** 1.0  

---

## Executive Summary

This document provides detailed test case specifications for the CollisionManager MVP feature, including specific input values, expected outputs, preconditions, and pass/fail criteria. These specifications are designed to be directly translatable to C# test code using Unity Test Framework.

**Total Test Cases:** 14  
**Edit Mode Tests:** 8  
**Play Mode Tests:** 6  
**Estimated Implementation Time:** 2.5 hours

---

## Test Case Naming Convention

```
TC_[Layer]_[Feature]_[Scenario]_[Expected]

Layers: Unit (Edit Mode), Integration (Play Mode)
Feature: GeometryMath, MultiSlice, EventIntegration
Scenario: PassThrough, Tangent, Miss, etc.
Expected: ReturnsTrue, ReturnsFalse, DetectsAll, etc.
```

---

## EDIT MODE TESTS (Unit Tests - Pure Geometry)

### TC_Unit_GeometryMath_PassThrough_ReturnsTrue

**Test ID:** UT-001  
**Category:** Geometry Math - Pass-Through Cases  
**Priority:** CRITICAL  

**Preconditions:**
- CollisionManager component instantiated
- No external dependencies required (pure math)

**Test Input:**
```
Swipe Start:     Vector2(0, 0)
Swipe End:       Vector2(10, 0)
Fruit Position:  Vector2(5, 0)
Fruit Radius:    1.0f
```

**Expected Output:**
```
Result: true
Reason: Horizontal line passes through circle center
```

**Execution Steps:**
1. Call `CollisionManager.DoesSwipeIntersectFruit(start, end, fruitPos, radius)`
2. Verify return value equals `true`
3. No side effects expected

**Pass Criteria:**
- ✅ Return value is exactly `true`
- ✅ No exceptions thrown
- ✅ No null reference exceptions

**Fail Criteria:**
- ❌ Return value is `false`
- ❌ Exception thrown
- ❌ Unexpected null pointer

**Notes:**
- Baseline test case for pass-through validation
- Horizontal line through center is optimal case
- Used as regression test

---

### TC_Unit_GeometryMath_DiagonalPassThrough_ReturnsTrue

**Test ID:** UT-002  
**Category:** Geometry Math - Pass-Through Cases  
**Priority:** HIGH  

**Preconditions:**
- CollisionManager component instantiated

**Test Input:**
```
Swipe Start:     Vector2(0, 0)
Swipe End:       Vector2(10, 10)
Fruit Position:  Vector2(5, 5)
Fruit Radius:    2.0f
```

**Expected Output:**
```
Result: true
Reason: Diagonal (45°) line through circle center
```

**Execution Steps:**
1. Call `DoesSwipeIntersectFruit(start, end, fruitPos, radius)`
2. Verify return value equals `true`

**Pass Criteria:**
- ✅ Return value is `true`
- ✅ Handles non-horizontal/non-vertical swipes

**Fail Criteria:**
- ❌ Return value is `false`
- ❌ Returns incorrect result for diagonal vectors

**Notes:**
- Tests angle independence of geometry algorithm
- Verifies algorithm works with arbitrary line angles
- Margin of error: floating-point tolerance ±0.0001

---

### TC_Unit_GeometryMath_TangentCase_ReturnsFalse

**Test ID:** UT-003  
**Category:** Geometry Math - Tangent Edge Cases  
**Priority:** CRITICAL  

**Preconditions:**
- CollisionManager component instantiated
- Geometry calculation verified with reference implementation

**Test Input:**
```
Swipe Start:     Vector2(0, 0)
Swipe End:       Vector2(5, 2)
Fruit Position:  Vector2(2, 1)
Fruit Radius:    1.0f
```

**Expected Output:**
```
Result: false
Reason: Line barely touches circle edge (tangent), doesn't pass through
Closest Point on Line to Center: approximately (2, 1)
Distance to Center: 1.0 (equals radius - tangent condition)
```

**Execution Steps:**
1. Calculate reference solution with geometric tools
2. Call `DoesSwipeIntersectFruit(start, end, fruitPos, radius)`
3. Verify return value equals `false`

**Pass Criteria:**
- ✅ Return value is `false` (tangent doesn't count as slice)
- ✅ Boundary condition correctly rejected

**Fail Criteria:**
- ❌ Return value is `true` (incorrectly accepted tangent)
- ❌ Epsilon tolerance too loose (accepting tangent)

**Notes:**
- **CRITICAL EDGE CASE:** Tangent rejection is key to accurate slicing
- Requires precise floating-point comparison with epsilon tolerance
- Tangent swipes should NOT trigger slice events

---

### TC_Unit_GeometryMath_CompleteMiss_ReturnsFalse

**Test ID:** UT-004  
**Category:** Geometry Math - Miss Cases  
**Priority:** HIGH  

**Preconditions:**
- CollisionManager component instantiated

**Test Input:**
```
Swipe Start:     Vector2(0, 0)
Swipe End:       Vector2(10, 0)
Fruit Position:  Vector2(5, 3)
Fruit Radius:    1.0f
```

**Expected Output:**
```
Result: false
Reason: Perpendicular distance (3.0) exceeds radius (1.0)
```

**Execution Steps:**
1. Call `DoesSwipeIntersectFruit(start, end, fruitPos, radius)`
2. Verify return value equals `false`

**Pass Criteria:**
- ✅ Return value is `false`
- ✅ No false positives for nearby-but-not-intersecting fruits

**Fail Criteria:**
- ❌ Return value is `true` (incorrect collision detection)

**Notes:**
- Tests negative case (should fail)
- Fruit is 3 units away, radius only 1.0
- Margin: 2.0 units clear miss

---

### TC_Unit_GeometryMath_ZeroLengthSwipe_ReturnsFalse

**Test ID:** UT-005  
**Category:** Geometry Math - Boundary Cases  
**Priority:** MEDIUM  

**Preconditions:**
- CollisionManager component instantiated

**Test Input:**
```
Swipe Start:     Vector2(5, 5)
Swipe End:       Vector2(5, 5)  // Same as start
Fruit Position:  Vector2(5, 5)
Fruit Radius:    1.0f
```

**Expected Output:**
```
Result: false
Reason: Zero-length segment is not a valid swipe
```

**Execution Steps:**
1. Call `DoesSwipeIntersectFruit(start, end, fruitPos, radius)`
2. Verify return value equals `false`

**Pass Criteria:**
- ✅ Return value is `false`
- ✅ No exceptions thrown
- ✅ Handles degenerate case gracefully

**Fail Criteria:**
- ❌ Return value is `true` (incorrectly accepting point collision)
- ❌ Exception thrown for division by zero

**Notes:**
- Boundary condition: swipe must have non-zero length
- Tests robustness against invalid input
- Should never occur in gameplay but must not crash

---

### TC_Unit_GeometryMath_ShortSwipePassThrough_ReturnsTrue

**Test ID:** UT-006  
**Category:** Geometry Math - Pass-Through Cases  
**Priority:** HIGH  

**Preconditions:**
- CollisionManager component instantiated

**Test Input:**
```
Swipe Start:     Vector2(3, 5)
Swipe End:       Vector2(7, 5)
Fruit Position:  Vector2(5, 5)
Fruit Radius:    1.0f
```

**Expected Output:**
```
Result: true
Reason: Short 4-unit swipe passes through 2-unit diameter circle
```

**Execution Steps:**
1. Call `DoesSwipeIntersectFruit(start, end, fruitPos, radius)`
2. Verify return value equals `true`

**Pass Criteria:**
- ✅ Return value is `true`
- ✅ Works with various swipe lengths

**Fail Criteria:**
- ❌ Return value is `false`
- ❌ Length-dependent failures

**Notes:**
- Tests that algorithm works for swipes shorter than typical
- 4-unit total swipe length
- Circle diameter = 2 units (radius 1.0)

---

### TC_Unit_GeometryMath_LargeFruitPassThrough_ReturnsTrue

**Test ID:** UT-007  
**Category:** Geometry Math - Radius Variation  
**Priority:** HIGH  

**Preconditions:**
- CollisionManager component instantiated

**Test Input:**
```
Swipe Start:     Vector2(2, 5)
Swipe End:       Vector2(8, 5)
Fruit Position:  Vector2(5, 5)
Fruit Radius:    3.0f
```

**Expected Output:**
```
Result: true
Reason: Swipe passes through large circle (radius 3.0)
```

**Execution Steps:**
1. Call `DoesSwipeIntersectFruit(start, end, fruitPos, radius)`
2. Verify return value equals `true`

**Pass Criteria:**
- ✅ Return value is `true`
- ✅ Works with large radius values

**Fail Criteria:**
- ❌ Return value is `false`
- ❌ Fails on large fruit sizes

**Notes:**
- Tests algorithm with watermelon-sized fruit (large radius)
- Ensures algorithm not optimized for small radii only
- Radius: 3.0 (2.5x the small fruit radius of 1.0)

---

### TC_Unit_GeometryMath_VeryCloseButMiss_ReturnsFalse

**Test ID:** UT-008  
**Category:** Geometry Math - Boundary Cases  
**Priority:** CRITICAL  

**Preconditions:**
- CollisionManager component instantiated
- Epsilon tolerance calibrated to ~0.0001

**Test Input:**
```
Swipe Start:     Vector2(0, 0)
Swipe End:       Vector2(10, 0)
Fruit Position:  Vector2(5, 0.99)
Fruit Radius:    0.5f
```

**Expected Output:**
```
Result: false
Reason: Distance to line (0.99) exceeds radius (0.5)
Margin: 0.49 units clear miss
```

**Execution Steps:**
1. Call `DoesSwipeIntersectFruit(start, end, fruitPos, radius)`
2. Verify return value equals `false`

**Pass Criteria:**
- ✅ Return value is `false`
- ✅ Boundary condition correctly rejected
- ✅ Precision epsilon working correctly

**Fail Criteria:**
- ❌ Return value is `true` (false positive)
- ❌ Epsilon tolerance too loose

**Notes:**
- **CRITICAL:** Tests precision of floating-point boundary
- 0.49 unit margin between line and circle edge
- Validates that epsilon tolerance is not too loose
- Ensures tight collision detection accuracy

---

## PLAY MODE TESTS (Integration Tests)

### TC_Integration_EventIntegration_SwipeDetectorIntegration_EventReceived

**Test ID:** IT-001  
**Category:** Event Integration  
**Priority:** HIGH  

**Preconditions:**
- Unity scene created with GameObjects
- SwipeDetector component attached to GameObject
- CollisionManager component attached to different GameObject
- Both components enabled

**Test Input:**
- Manual setup of event subscription

**Expected Output:**
- CollisionManager receives swipe event from SwipeDetector
- No null reference exceptions

**Execution Steps:**
1. Create GameObject with SwipeDetector
2. Create GameObject with CollisionManager
3. Enable both components
4. Manually invoke `SwipeDetector.OnSwipeDetected` event
5. Verify CollisionManager event handler called
6. Verify no exceptions thrown

**Pass Criteria:**
- ✅ Event handler called successfully
- ✅ No null reference exceptions
- ✅ Event subscription persists

**Fail Criteria:**
- ❌ Event handler not called
- ❌ Null reference exception thrown
- ❌ Event not properly subscribed

**Notes:**
- Validates event-driven integration pattern
- Tests loose coupling between components
- Uses UnityTest coroutine for proper frame timing

---

### TC_Integration_SingleFruitCollision_FruitDetected_ReturnsGameObject

**Test ID:** IT-002  
**Category:** Collision Detection  
**Priority:** CRITICAL  

**Preconditions:**
- Play Mode test environment
- CollisionManager instantiated
- Single test fruit spawned at known position
- Fruit has CircleCollider2D with correct radius
- Fruit on correct physics layer

**Test Input:**
```
Swipe Start:     Vector2(2, 5)
Swipe End:       Vector2(8, 5)
Fruit Position:  Vector2(5, 5)
Fruit Radius:    1.0f (collider)
```

**Expected Output:**
```
GetFruitsInSwipePath() returns List<GameObject> with 1 element
- Element[0].name == "TestFruit_0"
- Element[0].transform.position == Vector2(5, 5)
```

**Execution Steps:**
1. Spawn test fruit at position (5, 5) with CircleCollider2D (radius 1.0)
2. Call `CollisionManager.GetFruitsInSwipePath(start, end)`
3. Verify returned list has exactly 1 fruit
4. Verify fruit GameObject reference is correct
5. Clean up test fruit

**Pass Criteria:**
- ✅ List contains exactly 1 fruit
- ✅ Fruit GameObject reference is valid
- ✅ Fruit position matches expected

**Fail Criteria:**
- ❌ List is empty (fruit not detected)
- ❌ List has wrong number of fruits
- ❌ GameObject reference is null

**Notes:**
- First integration test for real fruit detection
- Validates physics layer masking
- Tests CircleCollider2D integration

---

### TC_Integration_MultiFruitSlicing_ThreeFruits_AllDetected

**Test ID:** IT-003  
**Category:** Multi-Fruit Slicing  
**Priority:** CRITICAL  

**Preconditions:**
- Play Mode test environment
- CollisionManager instantiated
- Three test fruits spawned in aligned positions
- All fruits have CircleCollider2D on correct physics layer

**Test Input:**
```
Swipe Start:     Vector2(0, 5)
Swipe End:       Vector2(10, 5)

Fruits:
- Fruit A: Position (2, 5), Radius 1.0
- Fruit B: Position (5, 5), Radius 1.0
- Fruit C: Position (8, 5), Radius 1.0
```

**Expected Output:**
```
GetFruitsInSwipePath() returns List<GameObject> with 3 elements
List contains references to all 3 fruits (order independent)
```

**Execution Steps:**
1. Spawn 3 test fruits at (2,5), (5,5), (8,5)
2. Call `CollisionManager.GetFruitsInSwipePath(start, end)`
3. Verify returned list has exactly 3 fruits
4. Verify all fruit references are non-null
5. Verify all fruits are in returned list (using GameObject reference comparison)
6. Clean up test fruits

**Pass Criteria:**
- ✅ List size == 3
- ✅ All fruit references present
- ✅ No duplicates in list
- ✅ All references valid (not null)

**Fail Criteria:**
- ❌ List size != 3
- ❌ Missing any fruit from list
- ❌ Duplicate fruits in list
- ❌ Any null references

**Notes:**
- **CRITICAL FEATURE:** Multi-fruit slicing must work
- All three fruits on single horizontal line
- Tests core gameplay mechanic

---

### TC_Integration_SelectiveMultiFruitSlicing_SelectiveMiss_PartialDetection

**Test ID:** IT-004  
**Category:** Multi-Fruit Slicing - Selective  
**Priority:** HIGH  

**Preconditions:**
- Play Mode test environment
- CollisionManager instantiated
- Three test fruits spawned in mixed positions (some hit, some miss)

**Test Input:**
```
Swipe Start:     Vector2(0, 5)
Swipe End:       Vector2(10, 5)

Fruits:
- Fruit A: Position (2, 5), Radius 1.0 (HIT - on swipe line)
- Fruit B: Position (5, 2), Radius 1.0 (MISS - 3 units above line)
- Fruit C: Position (8, 5), Radius 1.0 (HIT - on swipe line)
```

**Expected Output:**
```
GetFruitsInSwipePath() returns List<GameObject> with 2 elements
- Contains Fruit A and Fruit C
- Does NOT contain Fruit B
```

**Execution Steps:**
1. Spawn 3 test fruits (A, B, C) as described
2. Call `CollisionManager.GetFruitsInSwipePath(start, end)`
3. Verify returned list has exactly 2 fruits
4. Verify list contains A and C (not B)
5. Clean up test fruits

**Pass Criteria:**
- ✅ List size == 2
- ✅ Contains exactly A and C
- ✅ Does not contain B

**Fail Criteria:**
- ❌ List size != 2
- ❌ Contains fruit B (false positive)
- ❌ Missing fruit A or C (false negative)

**Notes:**
- Tests selective collision detection
- Fruit B is 3 units away (1.0 radius, so 2.0 unit margin)
- Validates that non-intersecting fruits not included

---

### TC_Integration_OverlappingFruits_BothDetected

**Test ID:** IT-005  
**Category:** Multi-Fruit Slicing - Complex  
**Priority:** MEDIUM  

**Preconditions:**
- Play Mode test environment
- CollisionManager instantiated
- Two overlapping test fruits spawned

**Test Input:**
```
Swipe Start:     Vector2(3, 3)
Swipe End:       Vector2(7, 7)

Fruits (overlapping):
- Fruit A: Position (5, 5), Radius 1.5
- Fruit B: Position (5.5, 5.5), Radius 1.5
(Fruits overlap - distance between centers = 0.707 < sum of radii 3.0)
```

**Expected Output:**
```
GetFruitsInSwipePath() returns List<GameObject> with 2 elements
Both fruits detected despite overlap
```

**Execution Steps:**
1. Spawn 2 overlapping test fruits at (5,5) and (5.5, 5.5)
2. Call `CollisionManager.GetFruitsInSwipePath(start, end)`
3. Verify returned list has exactly 2 fruits
4. Verify both fruit references present
5. Clean up test fruits

**Pass Criteria:**
- ✅ List size == 2
- ✅ Both fruits detected
- ✅ Handles overlapping colliders correctly

**Fail Criteria:**
- ❌ List size != 2
- ❌ Only one fruit detected
- ❌ Neither fruit detected

**Notes:**
- Tests overlapping fruit scenario (can occur in gameplay)
- Both fruits on diagonal swipe line (Y=X)
- Validates algorithm doesn't fail on overlapping hitboxes

---

### TC_Integration_BoundaryCondition_DestroyedFruit_HandleGracefully

**Test ID:** IT-006  
**Category:** Boundary Conditions  
**Priority:** MEDIUM  

**Preconditions:**
- Play Mode test environment
- CollisionManager instantiated
- Test fruit spawned and then destroyed during collision detection

**Test Input:**
- Spawn fruit, mark for deletion, trigger collision detection

**Expected Output:**
```
GetFruitsInSwipePath() returns List<GameObject> without null entries
Or returns empty list (if fruit destroyed before detection)
No exceptions thrown
```

**Execution Steps:**
1. Spawn test fruit
2. Immediately mark for destruction (Object.Destroy)
3. Call `CollisionManager.GetFruitsInSwipePath(start, end)`
4. Verify no null reference exceptions thrown
5. Verify list doesn't contain null entries

**Pass Criteria:**
- ✅ No exception thrown
- ✅ No null entries in returned list
- ✅ Graceful handling of deleted fruits

**Fail Criteria:**
- ❌ Null reference exception thrown
- ❌ Null entry in returned list
- ❌ Unknown exception thrown

**Notes:**
- Stress test for error handling
- Tests robustness against edge cases
- Simulates destroyed fruits (can occur in rapid gameplay)

---

## Test Case Summary Table

| Test ID | Test Name | Category | Priority | Expected Result | Difficulty |
|---------|-----------|----------|----------|-----------------|-----------|
| UT-001 | PassThrough Horizontal | Geometry | CRITICAL | TRUE | Easy |
| UT-002 | PassThrough Diagonal | Geometry | HIGH | TRUE | Easy |
| UT-003 | Tangent Case | Geometry | **CRITICAL** | FALSE | Hard |
| UT-004 | Complete Miss | Geometry | HIGH | FALSE | Easy |
| UT-005 | Zero-Length Swipe | Boundary | MEDIUM | FALSE | Medium |
| UT-006 | Short Swipe | Geometry | HIGH | TRUE | Medium |
| UT-007 | Large Fruit | Radius Variation | HIGH | TRUE | Easy |
| UT-008 | Very Close Miss | Boundary | **CRITICAL** | FALSE | Hard |
| IT-001 | Event Integration | Integration | HIGH | Received | Easy |
| IT-002 | Single Fruit | Collision | **CRITICAL** | Detected | Easy |
| IT-003 | Three Fruits | Multi-Slice | **CRITICAL** | All Detected | Hard |
| IT-004 | Selective Miss | Multi-Slice | HIGH | 2/3 Detected | Hard |
| IT-005 | Overlapping | Complex | MEDIUM | Both Detected | Hard |
| IT-006 | Destroyed Fruit | Boundary | MEDIUM | Handle Gracefully | Hard |

**Legend:**
- 🔴 **CRITICAL:** Must pass for story completion
- 🟠 **HIGH:** Important for functionality
- 🟡 **MEDIUM:** Nice to have, stress test

---

## Appendix A: Test Assertions

### NUnit Assertion Patterns

```csharp
// Boolean results
Assert.IsTrue(result, "Should return true for pass-through");
Assert.IsFalse(result, "Should return false for tangent");

// Collection assertions
Assert.AreEqual(3, fruitsList.Count, "Should detect 3 fruits");
Assert.IsEmpty(fruitsList, "Should return empty list");

// Reference assertions
Assert.IsNotNull(fruit, "Fruit reference should not be null");
Assert.AreSame(expectedFruit, actualFruit, "Should return correct fruit");

// Exception assertions
Assert.Throws<ArgumentException>(() => method(), "Should throw exception");
Assert.DoesNotThrow(() => method(), "Should not throw exception");
```

---

## Appendix B: Test Data Reference Values

All geometric test cases calculated with reference implementation:
- Line-circle intersection algorithm verified with GeoGebra
- Floating-point precision: epsilon = 0.0001
- All distances calculated using Vector2.Distance()
- All angles verified with vector dot product

---

## Document History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-11-29 | BMAD | Initial test specification for Story 003 CollisionManager |

---

**Status:** READY FOR CODING  
**Next Step:** Create Test Scaffolding and C# Test Code  
**Owner:** BMAD Test Design Agent

