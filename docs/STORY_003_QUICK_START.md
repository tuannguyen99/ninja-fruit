# Story 003 - Quick Start Guide for TDD Development

**Story:** STORY-003: CollisionManager MVP  
**For:** Game Developers using Test-Driven Development  
**Time to Read:** 5 minutes  
**Status:** Ready to implement

---

## 🎯 Your Mission

Implement `CollisionManager` component that detects when a player's swipe line intersects with fruit collision circles.

**Goal:** Make 24 failing tests pass (Red → Green → Refactor)

---

## 📍 Location of Everything

### Test Artifacts
| Artifact | Location | Purpose |
|----------|----------|---------|
| **Test Plan** | `docs/test-plans/test-plan-story-003-collisionmanager.md` | Strategy & approach |
| **Test Spec** | `docs/test-specs/test-spec-story-003-collisionmanager.md` | Exact test cases |
| **Test Scaffolding** | `docs/test-scaffolding/test-scaffolding-story-003-collisionmanager.md` | Utilities & patterns |
| **Edit Mode Tests** | `Assets/Tests/EditMode/Gameplay/CollisionGeometryTests.cs` | Run these first |
| **Play Mode Tests** | `Assets/Tests/PlayMode/Gameplay/CollisionDetectionIntegrationTests.cs` | Run after Edit Mode |

### Implementation Location
```
Assets/Scripts/Gameplay/CollisionManager.cs  ← Create this file
```

---

## 🏗️ What You Need to Implement

### Minimal Interface

```csharp
namespace NinjaFruit
{
    public class CollisionManager : MonoBehaviour
    {
        // Core method: Detect if line segment intersects circle
        // Input: line from 'start' to 'end', circle at 'fruitPos' with radius
        // Output: true if line passes through circle (entry AND exit)
        public bool DoesSwipeIntersectFruit(Vector2 start, Vector2 end, 
                                           Vector2 fruitPos, float radius)
        {
            // TODO: Implement line-circle intersection
        }

        // Integration method: Get all fruits hit by swipe
        // Input: swipe line segment
        // Output: List of fruit GameObjects that were hit
        public List<GameObject> GetFruitsInSwipePath(Vector2 start, Vector2 end)
        {
            // TODO: Use DoesSwipeIntersectFruit to check all fruits in scene
        }
    }
}
```

---

## 🔴 Phase 1: RED (Watch Tests Fail)

### Step 1: Create Empty CollisionManager
Create file: `Assets/Scripts/Gameplay/CollisionManager.cs`

```csharp
using UnityEngine;
using System.Collections.Generic;

namespace NinjaFruit
{
    public class CollisionManager : MonoBehaviour
    {
        public bool DoesSwipeIntersectFruit(Vector2 start, Vector2 end, 
                                           Vector2 fruitPos, float radius)
        {
            return false; // Placeholder
        }

        public List<GameObject> GetFruitsInSwipePath(Vector2 start, Vector2 end)
        {
            return new List<GameObject>(); // Placeholder
        }
    }
}
```

### Step 2: Run Edit Mode Tests
```
Unity Editor → Window → Test Runner
  → Select "EditMode" tab
  → Right-click: "Run All"
```

**Expected Result:** 🔴 13 tests FAIL (RED phase ✓)

### Step 3: Read Test Failures
Each test failure tells you what's wrong:
- ✅ Good - tests are catching the missing implementation
- ✅ Means tests are well-designed

---

## 🟢 Phase 2: GREEN (Make Tests Pass)

### Step 1: Implement Line-Circle Intersection

The core algorithm (from test specification):

```csharp
public bool DoesSwipeIntersectFruit(Vector2 start, Vector2 end, 
                                    Vector2 fruitPos, float radius)
{
    // Edge case: zero-length swipe
    float segmentLength = Vector2.Distance(start, end);
    if (segmentLength < 0.00001f)
        return false;

    // Find closest point on line segment to circle center
    Vector2 PA = fruitPos - start;
    Vector2 BA = end - start;
    float h = Mathf.Clamp01(Vector2.Dot(PA, BA) / Vector2.Dot(BA, BA));
    
    // Calculate distance from circle center to line
    Vector2 closest = start + h * BA;
    float distance = Vector2.Distance(fruitPos, closest);
    
    // Pass-through: distance <= radius AND closest point within segment bounds
    // (h = 0 means start, h = 1 means end, h between 0-1 means within segment)
    return distance <= radius && h > 0 && h < 1;
}
```

**Why This Works:**
- `h` tells us where on the line the closest point is
- `h < 0`: closest point is before start
- `h > 1`: closest point is after end
- `0 < h < 1`: closest point is on the segment ✓
- `distance <= radius`: closest point is inside circle
- Combined: Line segment actually passes through circle

### Step 2: Implement GetFruitsInSwipePath

```csharp
public List<GameObject> GetFruitsInSwipePath(Vector2 start, Vector2 end)
{
    List<GameObject> fruits = new List<GameObject>();
    
    // Find all fruits in scene
    Fruit[] allFruits = FindObjectsOfType<Fruit>();
    
    foreach (Fruit fruit in allFruits)
    {
        CircleCollider2D collider = fruit.GetComponent<CircleCollider2D>();
        if (collider == null)
            continue;
        
        // Check if this fruit's collision circle intersects the swipe line
        Vector2 fruitPos = fruit.transform.position;
        float radius = collider.radius;
        
        if (DoesSwipeIntersectFruit(start, end, fruitPos, radius))
        {
            fruits.Add(fruit.gameObject);
        }
    }
    
    return fruits;
}
```

### Step 3: Run Edit Mode Tests Again
```
Unity Editor → Window → Test Runner
  → Select "EditMode" tab
  → Right-click: "Run All"
```

**Expected Result:** ✅ 13 tests PASS (GREEN phase ✓)

### Step 4: Run Play Mode Tests
```
Unity Editor → Window → Test Runner
  → Select "PlayMode" tab
  → Right-click: "Run All"
```

**Expected Result:** ✅ 11 tests PASS (all 24 passing ✓)

---

## 🔵 Phase 3: REFACTOR (Optional - If Tests Pass)

### If All Tests Pass, You Can:

1. **Extract Geometry Utilities** (if code is duplicated)
   ```csharp
   // Move math to: Assets/Scripts/Utilities/CollisionMath.cs
   public static class CollisionMath
   {
       public static bool LineCircleIntersect(Vector2 start, Vector2 end, 
                                             Vector2 center, float radius)
       {
           // Move implementation here
       }
   }
   ```

2. **Add Performance Optimizations**
   - Spatial hashing for large fruit counts
   - Early exit for obvious misses
   - Measure performance with performance tests

3. **Improve Code Comments**
   - Explain why epsilon tolerances matter
   - Document geometry logic clearly

### Important Rule
**Don't refactor until tests are GREEN!**
If you refactor before tests pass, you'll get lost.

---

## 🧪 Running Tests Command by Command

### Edit Mode Tests (Fastest - Start Here)
```bash
# In Unity Editor
Window → Test Runner → EditMode tab → Run All

# Command line
unity -projectPath . -runTests -testPlatform editmode
```
**Time:** < 100ms  
**Should see:** 13 passes

### Play Mode Tests (Slower - After Edit Mode Pass)
```bash
# In Unity Editor
Window → Test Runner → PlayMode tab → Run All

# Command line
unity -projectPath . -runTests -testPlatform playmode
```
**Time:** ~5-10 seconds  
**Should see:** 11 passes

### Run All Tests Together
```bash
# In Unity Editor
Window → Test Runner → Run All (at top)

# Command line
unity -projectPath . -runTests -testPlatform editmode,playmode
```
**Total:** 24 tests, should all pass

---

## 📊 Success Checklist

Use this checklist as you work:

### Before You Start
- [ ] Read Test Plan (get overview)
- [ ] Read Test Spec (understand exact scenarios)
- [ ] Skim Test Scaffolding (know what utilities are available)

### Implementation
- [ ] Create `CollisionManager.cs` file
- [ ] Implement `DoesSwipeIntersectFruit()` method
- [ ] Run Edit Mode tests - verify all 13 pass
- [ ] Implement `GetFruitsInSwipePath()` method
- [ ] Run Play Mode tests - verify all 11 pass
- [ ] Verify all 24 tests pass together

### Quality
- [ ] No compiler warnings
- [ ] No test failures
- [ ] Code is readable and documented
- [ ] Performance acceptable (<1ms per collision)

### Submission
- [ ] All tests passing
- [ ] Code checked in with commit message:
   ```
   Story 003: Implement CollisionManager MVP
   
   - Implement line-circle intersection algorithm
   - Add multi-fruit detection
   - All 24 tests passing
   ```

---

## 🆘 Help! Tests Still Failing?

### Check This First

**Q: Edit Mode tests failing with NullReferenceException?**
- A: Make sure `CollisionManager.cs` exists and method returns something

**Q: Play Mode tests failing with "Fruit not found"?**
- A: Ensure `Fruit.cs` component exists in scene
- A: Check that CircleCollider2D is attached to fruit GameObjects

**Q: Getting "wrong result" assertions?**
- A: Check your distance calculation formula
- A: Verify epsilon tolerance (0.0001f)
- A: Use test spec to compare exact inputs/outputs

**Q: Tests run but say "Object not found"?**
- A: Make sure FindObjectsOfType<Fruit>() finds your fruits
- A: Verify fruits have the Fruit component attached

### Reference Implementation

If you get stuck, the key algorithm is in test spec:

```csharp
// Reference from Test Specification
bool DoesSwipeIntersectFruit(Vector2 start, Vector2 end, Vector2 center, float radius)
{
    // Zero-length check
    if (Vector2.Distance(start, end) < 0.00001f)
        return false;
    
    // Project center onto line
    Vector2 PA = center - start;
    Vector2 BA = end - start;
    float t = Mathf.Clamp01(Vector2.Dot(PA, BA) / Vector2.Dot(BA, BA));
    
    // Find closest point and distance
    Vector2 closest = start + t * BA;
    float distance = Vector2.Distance(center, closest);
    
    // Pass-through: within radius AND within segment bounds
    return distance <= radius && t > 0 && t < 1;
}
```

---

## 📚 Additional Resources

### In This Project
- Test Plan: High-level overview of what/why/how to test
- Test Spec: Exact test cases with inputs/outputs
- Test Code: Executable tests (look at test name = documentation)
- Test Scaffolding: Reusable patterns and utilities

### External Resources
- Unity Test Framework docs: https://docs.unity3d.com/Packages/com.unity.test-framework@latest
- Geometry resource: Wolfram MathWorld "Line-Circle Intersection"
- TDD explained: https://en.wikipedia.org/wiki/Test-driven_development

---

## 🎮 Once Tests Pass

Once all 24 tests pass:

1. **Move to Story-004** (ScoreManager MVP)
   - Same workflow: Plan → Spec → Scaffolding → Code
   - Use this as template

2. **Verify Integration** with Story-001 and Story-002
   - Does FruitSpawner work with CollisionManager?
   - Does SwipeDetector events trigger CollisionManager?

3. **Performance Testing** (optional)
   - Try stress test with 100+ fruits
   - Measure collision detection time
   - Document findings

4. **Demo to Customer**
   - Show test plan → specification → tests → passing code
   - Highlight BMAD's time savings
   - This is value demonstration!

---

## ⏱️ Estimated Time

| Phase | Task | Time |
|-------|------|------|
| 🔴 RED | Read docs, create empty class, run tests | 15 min |
| 🟢 GREEN | Implement algorithm, verify all tests pass | 30 min |
| 🔵 REFACTOR | Optional cleanup (if needed) | 15 min |
| **TOTAL** | **Full Story-003** | **1-2 hours** |

---

## 📝 Notes

**Remember:**
- ✅ Tests are your specification (read them!)
- ✅ Red → Green → Refactor (always in this order)
- ✅ One test failing is better than 24 passing (means you're learning)
- ✅ Geometry math is correct if tests pass

**Don't:**
- ❌ Skip reading test specifications
- ❌ Try to implement before tests exist
- ❌ Change test code (change implementation, not tests!)
- ❌ Commit code until all tests pass

---

**Ready to get started?**

1. Open `test-spec-story-003-collisionmanager.md` 
2. Read first 3 test cases (UT-001, UT-002, UT-003)
3. Create `CollisionManager.cs` with empty methods
4. Run Edit Mode tests and watch them fail
5. Implement algorithm
6. Watch tests turn green
7. 🎉 Success!

---

**Status:** READY FOR DEVELOPMENT  
**Created:** 2025-11-29  
**Owner:** BMAD (Test-Driven Development Support)

