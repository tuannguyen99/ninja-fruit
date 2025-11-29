# Test Plan: Story 003 - CollisionManager MVP

**Story:** STORY-003: CollisionManager MVP  
**Epic:** Core Slicing Mechanics (EPIC-001)  
**Author:** BMAD (Test Planning Agent)  
**Date:** November 29, 2025  
**Version:** 1.0  

---

## Executive Summary

This test plan defines the comprehensive testing strategy for the CollisionManager MVP feature, which implements line-circle intersection detection for fruit slicing mechanics. The feature is **CRITICAL** to core gameplay and requires rigorous testing across geometry edge cases, integration with SwipeDetector, and multi-platform validation.

**Test Complexity:** HIGH  
**Testing Priority:** CRITICAL (core mechanic)  
**Estimated Test Effort:** 6-8 hours  

---

## Test Objectives

### Primary Objectives
1. **Validate Line-Circle Geometry:** Ensure mathematical accuracy of intersection detection across all edge cases
2. **Verify Pass-Through Logic:** Confirm that only swipes with proper entry and exit points register as valid slices
3. **Test Multi-Fruit Slicing:** Validate simultaneous slicing of multiple fruits in single swipe
4. **Integrate SwipeDetector:** Verify proper event subscription and collision detection flow
5. **Ensure Cross-Platform Accuracy:** Confirm geometry math works identically across all platforms

### Secondary Objectives
1. **Performance Validation:** Verify sub-millisecond collision detection for stress scenarios
2. **Edge Case Identification:** Discover and document boundary conditions
3. **Regression Prevention:** Establish baseline for future collision system enhancements
4. **Code Quality:** Ensure high test coverage (80%+ on collision math, 100% on public API)

---

## Scope and Coverage

### In Scope (Story 003)

**Component Under Test:** `CollisionManager`

**Public API:**
- `DoesSwipeIntersectFruit(Vector2 start, Vector2 end, Vector2 fruitPos, float radius) → bool`
- `GetFruitsInSwipePath(Vector2 start, Vector2 end) → List<GameObject>`
- `OnEnable()` - Event subscription to SwipeDetector
- `CheckCollisions(Vector2 start, Vector2 end)` - Event handler

**Dependencies Tested:**
- Mathematical line-circle intersection algorithm
- SwipeDetector event system
- Fruit physics layer and collider configuration

**Test Categories:**
1. **Edit Mode Tests** (Unit Tests) - Pure geometry logic
2. **Play Mode Tests** (Integration Tests) - Component interaction with SwipeDetector and fruit instances
3. **Performance Tests** - Stress testing with multiple fruits

### Out of Scope (Future Stories)

- ❌ Bomb collision detection (Story-005 scope)
- ❌ Particle effects on slicing (Polish phase)
- ❌ Audio feedback (Audio system feature)
- ❌ Score calculation (Story-004 scope)
- ❌ Platform-specific input variations (Story-007/008 scope)

---

## Test Strategy

### Testing Approach

**Test-Driven Development (TDD) Methodology:**
1. **Red Phase:** Write failing tests first
2. **Green Phase:** Implement minimal code to pass tests
3. **Refactor Phase:** Optimize geometry math while keeping tests green

**Test Pyramid:**
```
        Integration Tests
        (Play Mode - 20%)
       ╱────────────────╲
      ╱    Edge Cases     ╲
     ╱ Multi-Fruit Tests   ╲
    
   Unit Tests
   (Edit Mode - 80%)
  ╱──────────────────╲
╱ Geometry Math       ╲
╱ Tangent Cases        ╲
╱ Pass-Through Cases    ╲
```

### Test Layers

#### Layer 1: Edit Mode Unit Tests (80% of effort)

**Focus:** Pure mathematical accuracy of line-circle intersection

**Test Categories:**
1. **Pass-Through Cases** (Primary)
   - Swipe line enters and exits circle
   - Various entry/exit points on circle perimeter
   - Short swipes vs long swipes

2. **Tangent Cases** (Edge Cases - Critical)
   - Swipe touches circle edge but doesn't pass through
   - Tangent from outside
   - Tangent from inside (impossible - shouldn't occur)

3. **Miss Cases** (Negative Tests)
   - Swipe completely outside circle
   - Various distances from circle
   - Swipe parallel to circle

4. **Boundary Cases**
   - Zero-length swipe (start == end)
   - Swipe starting inside circle
   - Swipe ending inside circle
   - Swipe with radius = 0 (point collision)

5. **Performance Cases**
   - Large number of calculations (1000+ iterations)
   - Various radius sizes (tiny to large fruits)

#### Layer 2: Play Mode Integration Tests (20% of effort)

**Focus:** Component interaction and real-world scenarios

**Test Categories:**
1. **Event Integration**
   - SwipeDetector fires swipe event
   - CollisionManager receives event correctly
   - No null reference exceptions

2. **Multi-Fruit Slicing**
   - Single swipe intersects 2 fruits → both detected
   - Single swipe intersects 3 fruits → all detected
   - Single swipe misses all fruits → empty list returned

3. **Fruit Detection**
   - Fruits on correct physics layer detected
   - Fruits outside scene not detected
   - Fruits not in active scene ignored

4. **Collision System Integration**
   - Circle collider radius matches test expectations
   - Physics layer masking works correctly
   - Destruction/removal of fruits handled gracefully

---

## Test Data Sets

### Geometric Test Cases

#### Scenario 1: Clear Pass-Through (Should Pass)
```
Swipe Start:     (0, 0)
Swipe End:       (10, 0)
Fruit Position:  (5, 0)
Fruit Radius:    1.0

Expected: TRUE (line passes through circle)
Geometry: Horizontal line through circle center
```

#### Scenario 2: Tangent Touch (Should Fail)
```
Swipe Start:     (0, 0)
Swipe End:       (5, 2)
Fruit Position:  (2, 1)
Fruit Radius:    1.0

Expected: FALSE (line barely touches edge, no pass-through)
Geometry: Tangent point at (2, 1.5) approximately
```

#### Scenario 3: Diagonal Pass-Through (Should Pass)
```
Swipe Start:     (0, 0)
Swipe End:       (10, 10)
Fruit Position:  (5, 5)
Fruit Radius:    2.0

Expected: TRUE (diagonal line through center)
Geometry: 45-degree angle through center
```

#### Scenario 4: Complete Miss (Should Fail)
```
Swipe Start:     (0, 0)
Swipe End:       (10, 0)
Fruit Position:  (5, 3)
Fruit Radius:    1.0

Expected: FALSE (line is 3 units away, only 1 unit radius)
Geometry: Line parallel to circle, insufficient proximity
```

#### Scenario 5: Short Swipe Pass-Through (Should Pass)
```
Swipe Start:     (3, 5)
Swipe End:       (7, 5)
Fruit Position:  (5, 5)
Fruit Radius:    1.0

Expected: TRUE (short swipe passes through)
Geometry: 4-unit swipe through 2-unit diameter circle
```

#### Scenario 6: Zero-Length Swipe (Should Fail)
```
Swipe Start:     (5, 5)
Swipe End:       (5, 5)
Fruit Position:  (5, 5)
Fruit Radius:    1.0

Expected: FALSE (no valid line segment)
Geometry: Point collision (not valid swipe)
```

#### Scenario 7: Very Close Miss (Should Fail)
```
Swipe Start:     (0, 0)
Swipe End:       (10, 0)
Fruit Position:  (5, 0.99)
Fruit Radius:    0.5

Expected: FALSE (distance to line is 0.99, radius is 0.5, margin 0.49)
Geometry: Just outside collision threshold
```

#### Scenario 8: Large Fruit Pass-Through (Should Pass)
```
Swipe Start:     (2, 5)
Swipe End:       (8, 5)
Fruit Position:  (5, 5)
Fruit Radius:    3.0

Expected: TRUE (swipe passes through large circle)
Geometry: Swipe through center of large circle
```

### Multi-Fruit Test Scenarios

#### Scenario M1: Two-Fruit Horizontal Slicing
```
Fruits:
  - Fruit A: Position (3, 5), Radius 1.0
  - Fruit B: Position (7, 5), Radius 1.0

Swipe:  (0, 5) → (10, 5)

Expected: Both fruits detected (list size = 2)
```

#### Scenario M2: Three-Fruit Diagonal Slicing
```
Fruits:
  - Fruit A: Position (2, 2), Radius 0.5
  - Fruit B: Position (5, 5), Radius 0.5
  - Fruit C: Position (8, 8), Radius 0.5

Swipe: (0, 0) → (10, 10)

Expected: All 3 fruits detected
```

#### Scenario M3: Selective Slicing (Some Miss)
```
Fruits:
  - Fruit A: Position (2, 5), Radius 1.0 (HIT)
  - Fruit B: Position (5, 2), Radius 1.0 (MISS - above line)
  - Fruit C: Position (8, 5), Radius 1.0 (HIT)

Swipe: (0, 5) → (10, 5)

Expected: Only A and C detected (list size = 2)
```

#### Scenario M4: Overlapping Fruits
```
Fruits:
  - Fruit A: Position (5, 5), Radius 1.5
  - Fruit B: Position (5.5, 5.5), Radius 1.5 (overlaps A)

Swipe: (3, 3) → (7, 7)

Expected: Both fruits detected (both intersected by swipe)
```

---

## Test Execution Plan

### Phase 1: Setup (30 minutes)
1. Create test class scaffold: `CollisionManagerTests.cs`
2. Set up test fixtures (fruit spawning utilities)
3. Configure physics layer setup
4. Verify test data accuracy with manual calculations

### Phase 2: Edit Mode Unit Tests (2 hours)
1. Implement geometry math tests (Scenarios 1-8)
2. Add pass-through validation tests
3. Add tangent edge case tests
4. Add performance stress tests
5. Verify 80%+ code coverage on collision math

### Phase 3: Play Mode Integration Tests (1 hour)
1. Implement SwipeDetector event subscription test
2. Implement single-fruit collision test
3. Implement multi-fruit slicing tests (M1-M4)
4. Implement boundary condition tests (destroyed fruits, inactive scene)

### Phase 4: Manual Testing Validation (1 hour)
1. Create manual test scene with debug visualization
2. Visually verify collision detection accuracy
3. Test with actual swipe input on Windows build
4. Verify WebGL browser compatibility

### Phase 5: Performance Testing (30 minutes)
1. Run stress tests with 100+ fruits on screen
2. Measure average collision detection time per fruit
3. Document performance bottlenecks
4. Verify sub-millisecond performance targets

---

## Test Acceptance Criteria

### Coverage Requirements

| Metric | Target | Threshold |
|--------|--------|-----------|
| **Line-Circle Math Coverage** | 100% | ≥95% |
| **Public API Coverage** | 100% | ≥100% |
| **Pass-Through Logic Coverage** | 100% | ≥95% |
| **Overall Component Coverage** | 80% | ≥75% |
| **Critical Path Coverage** | 100% | ≥100% |

### Quality Gates

- ✅ **All unit tests passing** (Edit Mode: 8+ tests)
- ✅ **All integration tests passing** (Play Mode: 6+ tests)
- ✅ **Code coverage ≥80%**
- ✅ **No compiler warnings in test code**
- ✅ **Zero performance regressions** (collision < 1ms per fruit)
- ✅ **All edge cases documented**
- ✅ **Multi-fruit scenarios verified**

### Definition of Done

- ✅ Test code checked in with feature code
- ✅ Tests execute successfully in CI/CD pipeline
- ✅ Feature code is 100% covered by tests
- ✅ Documentation updated with test insights
- ✅ Team reviews and approves test coverage
- ✅ Performance benchmarks recorded

---

## Risk Assessment

### Technical Risks

| Risk | Impact | Likelihood | Mitigation |
|------|--------|------------|-----------|
| **Geometry Math Errors** | HIGH | MEDIUM | Extensive unit tests with known-good reference values |
| **Floating-Point Precision** | MEDIUM | MEDIUM | Test with epsilon tolerances; document precision limits |
| **Performance Degradation** | MEDIUM | LOW | Stress tests for 100+ fruits; optimization if needed |
| **Platform-Specific Issues** | MEDIUM | LOW | Test on Windows, WebGL, Android builds |
| **Physics Layer Misconfiguration** | MEDIUM | LOW | Validate layer masking in test setup |

### Schedule Risks

| Risk | Impact | Likelihood | Mitigation |
|------|--------|-----------|-----------|
| **Complex Edge Cases** | MEDIUM | MEDIUM | Pre-analyze geometry thoroughly; create test matrix |
| **Debug Cycle Length** | LOW | LOW | Use geometric visualization tools; document algorithm |

---

## Test Environment Requirements

### Hardware
- Development machine with GPU (for Play Mode tests)
- Minimum 4GB RAM (8GB recommended)
- SSD for fast test iteration

### Software
- Unity 6 (LTS version)
- Unity Test Framework package
- Input System package (for swipe simulation)
- Visual Studio or VSCode with C# extension

### Configuration
- Assembly Definition: `NinjaFruit.Tests`
- Edit Mode Tests Location: `Assets/Tests/EditMode/Gameplay/`
- Play Mode Tests Location: `Assets/Tests/PlayMode/Gameplay/`
- Test Scene: Create temporary test scene for Play Mode tests

---

## Success Metrics

### Quantitative Metrics
- **Test Coverage:** 80%+ on CollisionManager
- **Pass Rate:** 100% of all tests passing
- **Performance:** Collision detection <1ms per fruit
- **Build Time:** Test compilation <30 seconds

### Qualitative Metrics
- **Maintainability:** Test code is clear and well-documented
- **Confidence:** Developers confident in collision system correctness
- **Reusability:** Test utilities applicable to future collision features
- **Documentation:** Edge cases clearly understood and documented

---

## Approval and Sign-Off

| Role | Name | Signature | Date |
|------|------|-----------|------|
| **Test Plan Author** | BMAD | ✅ | 2025-11-29 |
| **QA Lead** | TBD | ⏳ | - |
| **Dev Lead** | TBD | ⏳ | - |
| **Product Owner** | TBD | ⏳ | - |

---

## Appendix A: Reference Geometry Calculations

### Line-Circle Intersection Formula

**Problem:** Given a line segment from P1 to P2 and a circle at center C with radius r, determine if the line passes through the circle.

**Solution Steps:**
1. Find perpendicular distance from circle center C to infinite line through P1-P2
2. If distance > radius, no intersection
3. If distance ≤ radius, find intersection points
4. Check if intersection points lie on segment (between P1 and P2)
5. If exactly 2 intersection points exist on segment, it's a pass-through (return TRUE)
6. If 0 or 1 intersection points, return FALSE (tangent or miss)

**Code Reference:**
```csharp
// Distance from point to line segment
float DistancePointToLineSegment(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
{
    Vector2 PA = point - lineStart;
    Vector2 BA = lineEnd - lineStart;
    float h = Mathf.Clamp01(Vector2.Dot(PA, BA) / Vector2.Dot(BA, BA));
    return (PA - BA * h).magnitude;
}

// Check if line segment intersects circle with pass-through validation
bool DoesSwipeIntersectFruit(Vector2 start, Vector2 end, Vector2 fruitPos, float radius)
{
    // Find closest point on line segment to circle center
    Vector2 PA = fruitPos - start;
    Vector2 BA = end - start;
    float segmentLength = BA.magnitude;
    
    if (segmentLength == 0) return false; // Zero-length segment
    
    float t = Mathf.Clamp01(Vector2.Dot(PA, BA) / Vector2.Dot(BA, BA));
    Vector2 closest = start + t * BA;
    
    float distance = (fruitPos - closest).magnitude;
    
    // Pass-through: intersection exists and closest point is within segment bounds
    return distance <= radius && t > 0 && t < 1;
}
```

### Tangent vs Pass-Through

**Tangent (Should Fail):**
- Distance from center to line = radius exactly
- Line touches circle at one point only
- Swipe does not "pass through"

**Pass-Through (Should Pass):**
- Distance from center to line < radius
- Line intersects circle at two points
- Both intersection points within segment bounds
- Represents a "slice" action

---

## Appendix B: Glossary

- **Pass-Through:** Swipe line enters and exits circular collision boundary
- **Tangent:** Swipe line touches circle perimeter at exactly one point
- **Line-Circle Intersection:** Geometric operation to determine if line segment intersects circle
- **Collision Radius:** Circle's radius value in units
- **Multi-Fruit Slicing:** Single swipe simultaneously intersecting multiple fruit hitboxes
- **Epsilon Tolerance:** Small value (0.0001) for floating-point equality comparisons

---

## Document History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-11-29 | BMAD | Initial test plan for Story 003 CollisionManager MVP |

---

**Status:** READY FOR IMPLEMENTATION  
**Next Step:** Create Test Specifications (test-spec-story-003-collisionmanager.md)  
**Owner:** BMAD Test Planning Agent

