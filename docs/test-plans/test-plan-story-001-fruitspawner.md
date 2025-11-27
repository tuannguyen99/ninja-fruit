# Test Plan: STORY-001 - FruitSpawner MVP

**Generated:** 2025-11-27  
**Story:** STORY-001 - FruitSpawner MVP  
**Epic:** EPIC-001 - Core Slicing Mechanics  
**Test Architect:** Murat  
**Framework:** Unity Test Framework (NUnit)

---

## Executive Summary

This test plan covers the FruitSpawner component, which is **CRITICAL** to gameplay. All other mechanics depend on reliable, deterministic fruit spawning. Risk assessment indicates HIGH impact if spawn formulas are incorrect or spawning fails.

**Test Strategy:** Mix of fast Edit Mode tests (formulas) and Play Mode integration tests (actual instantiation).

**Estimated Test Count:** 12 tests (8 Edit Mode, 4 Play Mode)  
**Estimated Execution Time:** Edit Mode ~50ms, Play Mode ~5s  
**Target Coverage:** 95%+ on FruitSpawner logic

---

## Risk Assessment

### Risk Matrix Analysis

| Risk ID | Description | Category | Probability | Impact | Risk Score | Priority |
|---------|-------------|----------|-------------|--------|------------|----------|
| RISK-001 | Spawn interval formula incorrect | TECH | Medium | Critical | **HIGH** | P0 |
| RISK-002 | Fruit speed formula incorrect | TECH | Medium | High | **MEDIUM** | P1 |
| RISK-003 | Bomb spawn rate wrong | TECH | Low | High | **MEDIUM** | P1 |
| RISK-004 | Prefab instantiation fails | TECH | Low | Critical | **MEDIUM** | P1 |
| RISK-005 | Rigidbody2D not attached | TECH | Low | Critical | **MEDIUM** | P1 |
| RISK-006 | Fruit not tagged correctly | TECH | Low | Medium | **LOW** | P2 |

**Risk Scoring Formula:** `Risk Score = Probability × Impact`
- **HIGH:** Blocks core gameplay (P0 - test first)
- **MEDIUM:** Degrades experience (P1 - test before merge)
- **LOW:** Minor issues (P2 - test eventually)

---

## Test Categories

### Category Breakdown

| Category | Test Count | Rationale |
|----------|------------|-----------|
| **Edit Mode (Unit)** | 8 | Formula validation, boundary conditions, math accuracy |
| **Play Mode (Integration)** | 4 | Prefab instantiation, component verification, physics setup |

**Why this split?**
- **Edit Mode advantage:** Fast (<1ms per test), no Unity runtime overhead, perfect for math
- **Play Mode necessity:** Required to validate actual GameObject instantiation and Unity component setup

---

## Test Coverage Matrix

### Acceptance Criteria Mapping

| AC ID | Acceptance Criteria | Test Type | Test Count | Priority |
|-------|---------------------|-----------|------------|----------|
| **AC-1** | `SpawnFruit()` instantiates prefab and tags it `Fruit` | Play Mode | 2 | P0 |
| **AC-2** | `CalculateSpawnInterval(score)` follows `Max(0.3s, 2.0s - (score / 500))` | Edit Mode | 4 | P0 |
| **AC-3** | `CalculateFruitSpeed(score)` follows `Min(7m/s, 2m/s + (score / 1000))` | Edit Mode | 4 | P1 |
| **AC-4** | `ShouldSpawnBomb(fruitCount)` returns true at 10% rate | Edit Mode | 2 | P1 |
| **AC-5** | Play Mode test verifies `Rigidbody2D` component | Play Mode | 2 | P1 |

**Coverage Goal:** 100% of acceptance criteria, 95%+ code coverage

---

## Detailed Test Specifications

### Edit Mode Tests (Unit Tests)

#### Test Suite: SpawnIntervalCalculationTests

**Purpose:** Validate spawn interval formula accuracy at key score thresholds

**Location:** `Assets/Tests/EditMode/Gameplay/FruitSpawnerTests.cs`

**Test Cases:**

1. **CalculateSpawnInterval_ScoreZero_Returns2Seconds**
   - **Given:** Score = 0
   - **When:** `CalculateSpawnInterval(0)` is called
   - **Then:** Returns exactly 2.0f
   - **Risk:** RISK-001 (HIGH)
   - **Priority:** P0

2. **CalculateSpawnInterval_Score500_Returns1Second**
   - **Given:** Score = 500
   - **When:** `CalculateSpawnInterval(500)` is called
   - **Then:** Returns exactly 1.0f
   - **Risk:** RISK-001 (HIGH)
   - **Priority:** P0

3. **CalculateSpawnInterval_Score1000_ReturnsMinimum0Point3Seconds**
   - **Given:** Score = 1000 (formula would yield 0.0s)
   - **When:** `CalculateSpawnInterval(1000)` is called
   - **Then:** Returns exactly 0.3f (minimum cap enforced)
   - **Risk:** RISK-001 (HIGH)
   - **Priority:** P0
   - **Edge Case:** Validates Min() boundary

4. **CalculateSpawnInterval_NegativeScore_ClampsTo2Seconds**
   - **Given:** Score = -100 (edge case: negative score)
   - **When:** `CalculateSpawnInterval(-100)` is called
   - **Then:** Returns 2.0f or greater (validates robustness)
   - **Risk:** RISK-001 (HIGH)
   - **Priority:** P0
   - **Edge Case:** Defensive programming validation

---

#### Test Suite: FruitSpeedCalculationTests

**Purpose:** Validate fruit speed formula accuracy and boundary conditions

**Location:** `Assets/Tests/EditMode/Gameplay/FruitSpawnerTests.cs`

**Test Cases:**

5. **CalculateFruitSpeed_ScoreZero_Returns2MetersPerSecond**
   - **Given:** Score = 0
   - **When:** `CalculateFruitSpeed(0)` is called
   - **Then:** Returns exactly 2.0f
   - **Risk:** RISK-002 (MEDIUM)
   - **Priority:** P1

6. **CalculateFruitSpeed_Score1000_Returns3MetersPerSecond**
   - **Given:** Score = 1000
   - **When:** `CalculateFruitSpeed(1000)` is called
   - **Then:** Returns exactly 3.0f (2.0 + 1.0)
   - **Risk:** RISK-002 (MEDIUM)
   - **Priority:** P1

7. **CalculateFruitSpeed_Score5000_ReturnsMaximum7MetersPerSecond**
   - **Given:** Score = 5000 (formula would yield 7.0f)
   - **When:** `CalculateFruitSpeed(5000)` is called
   - **Then:** Returns exactly 7.0f (maximum cap enforced)
   - **Risk:** RISK-002 (MEDIUM)
   - **Priority:** P1
   - **Edge Case:** Validates Max() boundary

8. **CalculateFruitSpeed_Score10000_DoesNotExceed7MetersPerSecond**
   - **Given:** Score = 10000 (formula would yield 12.0f)
   - **When:** `CalculateFruitSpeed(10000)` is called
   - **Then:** Returns exactly 7.0f (validates cap enforcement)
   - **Risk:** RISK-002 (MEDIUM)
   - **Priority:** P1
   - **Edge Case:** Extreme value validation

---

#### Test Suite: BombSpawnLogicTests

**Purpose:** Validate bomb spawn frequency follows 10% rate

**Location:** `Assets/Tests/EditMode/Gameplay/FruitSpawnerTests.cs`

**Test Cases:**

9. **ShouldSpawnBomb_FruitCount9_ReturnsFalse**
   - **Given:** 9 fruits have spawned
   - **When:** `ShouldSpawnBomb(9)` is called
   - **Then:** Returns false
   - **Risk:** RISK-003 (MEDIUM)
   - **Priority:** P1

10. **ShouldSpawnBomb_FruitCount10_ReturnsTrue**
    - **Given:** 10 fruits have spawned (10% threshold)
    - **When:** `ShouldSpawnBomb(10)` is called
    - **Then:** Returns true
    - **Risk:** RISK-003 (MEDIUM)
    - **Priority:** P1
    - **Note:** Assumes deterministic logic (not random for MVP)

---

### Play Mode Tests (Integration Tests)

#### Test Suite: FruitSpawningIntegrationTests

**Purpose:** Validate actual Unity GameObject instantiation and component setup

**Location:** `Assets/Tests/PlayMode/Gameplay/FruitSpawningIntegrationTests.cs`

**Test Cases:**

11. **SpawnFruit_CreatesGameObjectWithFruitTag**
    - **Given:** FruitSpawner component exists with valid prefab reference
    - **When:** `SpawnFruit()` is called
    - **Then:**
      - A GameObject is instantiated in the scene
      - GameObject has tag `Fruit`
    - **Risk:** RISK-004, RISK-006 (MEDIUM)
    - **Priority:** P1
    - **Setup:** Create FruitSpawner, assign test prefab via Resources.Load
    - **Teardown:** Destroy spawned fruit and spawner

12. **SpawnFruit_AttachesRigidbody2DComponent**
    - **Given:** FruitSpawner component exists with valid prefab reference
    - **When:** `SpawnFruit()` is called
    - **Then:**
      - Spawned fruit has `Rigidbody2D` component attached
      - Rigidbody2D is not null
    - **Risk:** RISK-005 (MEDIUM)
    - **Priority:** P1
    - **Validation:** `Assert.IsNotNull(spawnedFruit.GetComponent<Rigidbody2D>())`

13. **SpawnFruit_AppliesInitialVelocity**
    - **Given:** FruitSpawner configured with upward velocity
    - **When:** `SpawnFruit()` is called
    - **Then:**
      - Rigidbody2D.velocity.y > 0 (upward motion)
      - Velocity magnitude matches calculated speed
    - **Risk:** RISK-002 (MEDIUM)
    - **Priority:** P1
    - **Note:** Requires 1 frame yield to allow physics to apply

14. **SpawnFruit_MultipleCalls_CreatesMultipleFruits**
    - **Given:** FruitSpawner component exists
    - **When:** `SpawnFruit()` is called 3 times
    - **Then:**
      - Scene contains 3 GameObjects tagged `Fruit`
      - Each has independent Rigidbody2D
    - **Risk:** RISK-004 (MEDIUM)
    - **Priority:** P2
    - **Purpose:** Validates no singleton/caching issues

---

## Test Data Requirements

### Prefab Requirements

| Asset | Location | Requirements |
|-------|----------|--------------|
| **Test Fruit Prefab** | `Assets/Resources/Prefabs/TestFruit.prefab` | - Tag: `Fruit`<br>- Rigidbody2D attached<br>- CircleCollider2D (radius 0.3) |
| **Apple Prefab** | `Assets/Resources/Prefabs/Apple.prefab` | Same as TestFruit (production asset) |

**Creation Steps:**
1. Create empty GameObject
2. Add `Rigidbody2D` (Gravity Scale: 1.0, Mass: 1.0)
3. Add `CircleCollider2D` (Radius: 0.3)
4. Set tag to `Fruit`
5. Save to `Resources/Prefabs/`

### Test Configuration

```csharp
// Test constants for validation
const float EXPECTED_MIN_INTERVAL = 0.3f;
const float EXPECTED_MAX_INTERVAL = 2.0f;
const float EXPECTED_MIN_SPEED = 2.0f;
const float EXPECTED_MAX_SPEED = 7.0f;
const int BOMB_SPAWN_FREQUENCY = 10; // 1 per 10 fruits
```

---

## Test Execution Strategy

### Execution Order

1. **Phase 1: Edit Mode Tests** (run first, fast feedback)
   - Spawn interval tests (4 tests)
   - Fruit speed tests (4 tests)
   - Bomb spawn tests (2 tests)
   - **Total time:** ~50ms

2. **Phase 2: Play Mode Tests** (run after Edit Mode passes)
   - Spawn integration tests (4 tests)
   - **Total time:** ~5 seconds

**Rationale:** Fail fast on formula errors before expensive Unity runtime tests

### CI/CD Integration

```yaml
# GitHub Actions test execution
- name: Run Edit Mode Tests
  run: unity-editor -runTests -testPlatform EditMode -testResults results-editmode.xml
  
- name: Run Play Mode Tests (if Edit Mode passes)
  if: success()
  run: unity-editor -runTests -testPlatform PlayMode -testResults results-playmode.xml
```

---

## Test Environment Setup

### Prerequisites

| Requirement | Version | Purpose |
|-------------|---------|---------|
| Unity 6 | 6000.0.25f1 | Game engine |
| Unity Test Framework | 1.4.5+ | Test runner |
| NUnit | 3.x (bundled) | Assertion library |
| Test prefabs | N/A | Resources for Play Mode tests |

### Setup Steps

1. Create assembly definitions:
   - `Assets/Scripts/NinjaFruit.Runtime.asmdef`
   - `Assets/Tests/NinjaFruit.Tests.asmdef` (references Runtime)

2. Create folder structure:
   ```
   Assets/
   ├── Tests/
   │   ├── EditMode/
   │   │   └── Gameplay/
   │   │       └── FruitSpawnerTests.cs
   │   └── PlayMode/
   │       └── Gameplay/
   │           └── FruitSpawningIntegrationTests.cs
   ```

3. Create test prefabs in `Assets/Resources/Prefabs/`

---

## Test Maintenance Strategy

### When to Update Tests

| Trigger | Action |
|---------|--------|
| Formula changes in GDD | Update Edit Mode boundary tests |
| New fruit types added | Add Play Mode instantiation tests |
| Physics changes | Update velocity validation thresholds |
| Spawn logic refactor | Review all test assertions |

### Flakiness Prevention

**Common Flaky Patterns:**
- ❌ **Race conditions:** Use `yield return null` to wait for physics frames
- ❌ **Floating-point comparisons:** Use `Assert.AreEqual(expected, actual, 0.001f)` with tolerance
- ❌ **Prefab loading failures:** Validate `Resources.Load` result before tests

**Best Practices:**
- ✅ Reset PlayerPrefs in `[SetUp]`
- ✅ Destroy all test GameObjects in `[TearDown]`
- ✅ Use deterministic seeds for any randomization (future bomb spawn randomness)

---

## Success Criteria

### Definition of Done

- [ ] All 12 tests written and passing locally
- [ ] Edit Mode tests execute in <100ms
- [ ] Play Mode tests execute in <10s
- [ ] Code coverage ≥95% on FruitSpawner.cs
- [ ] No flaky tests (100% pass rate over 10 consecutive runs)
- [ ] Tests pass in CI/CD pipeline (GitHub Actions)
- [ ] Test documentation complete (this plan + inline comments)

### Quality Gates

| Gate | Criteria | Blocker? |
|------|----------|----------|
| **Unit Test Coverage** | ≥95% on FruitSpawner | Yes |
| **Test Execution Speed** | Edit Mode <100ms | No (warning only) |
| **Test Stability** | 100% pass rate × 10 runs | Yes |
| **CI Integration** | Tests run on PR commits | Yes |

---

## Risk Mitigation

### Identified Risks & Mitigations

| Risk | Mitigation |
|------|------------|
| **Prefab not found in Resources** | Add null check + descriptive error: `Assert.IsNotNull(prefab, "TestFruit prefab not found in Resources/Prefabs/")` |
| **Physics not initialized in tests** | Use `yield return new WaitForFixedUpdate()` before velocity assertions |
| **Formula edge cases** | Test negative scores, extreme values (10000+), and boundary conditions (0.3s, 7.0m/s) |
| **Test isolation failures** | Use `Object.DestroyImmediate()` for Edit Mode, `Object.Destroy()` for Play Mode, ensure cleanup in `[TearDown]` |

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
    public class FruitSpawnerTests
    {
        private FruitSpawner spawner;
        
        [SetUp]
        public void Setup()
        {
            GameObject spawnerObject = new GameObject("TestSpawner");
            spawner = spawnerObject.AddComponent<FruitSpawner>();
        }
        
        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(spawner.gameObject);
        }
        
        [Test]
        public void CalculateSpawnInterval_ScoreZero_Returns2Seconds()
        {
            // Act
            float interval = spawner.CalculateSpawnInterval(0);
            
            // Assert
            Assert.AreEqual(2.0f, interval, 0.001f);
        }
        
        [Test]
        public void CalculateSpawnInterval_Score500_Returns1Second()
        {
            // Act
            float interval = spawner.CalculateSpawnInterval(500);
            
            // Assert
            Assert.AreEqual(1.0f, interval, 0.001f);
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
using System.Collections;
using NinjaFruit;

namespace NinjaFruit.Tests.PlayMode
{
    [TestFixture]
    public class FruitSpawningIntegrationTests
    {
        private FruitSpawner spawner;
        private GameObject testPrefab;
        
        [SetUp]
        public void Setup()
        {
            // Load test prefab
            testPrefab = Resources.Load<GameObject>("Prefabs/TestFruit");
            Assert.IsNotNull(testPrefab, "TestFruit prefab not found in Resources/Prefabs/");
            
            // Create spawner
            GameObject spawnerObject = new GameObject("TestSpawner");
            spawner = spawnerObject.AddComponent<FruitSpawner>();
            // TODO: Assign testPrefab to spawner's fruitPrefabs array
        }
        
        [TearDown]
        public void Teardown()
        {
            // Cleanup spawned fruits
            GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
            foreach (var fruit in fruits)
            {
                Object.Destroy(fruit);
            }
            Object.Destroy(spawner.gameObject);
        }
        
        [UnityTest]
        public IEnumerator SpawnFruit_CreatesGameObjectWithFruitTag()
        {
            // Act
            spawner.SpawnFruit();
            yield return null; // Wait one frame for instantiation
            
            // Assert
            GameObject spawnedFruit = GameObject.FindWithTag("Fruit");
            Assert.IsNotNull(spawnedFruit, "No fruit with tag 'Fruit' found");
        }
        
        // Additional tests...
    }
}
```

---

## Appendix: Testing Knowledge References

### Relevant BMAD Knowledge Base Articles

- `test-levels-framework.md` - Guidance on Edit Mode vs Play Mode selection
- `test-priorities-matrix.md` - P0-P3 prioritization criteria
- `risk-governance.md` - Risk scoring methodology
- `test-quality.md` - Quality standards and Definition of Done

### External Resources

- [Unity Test Framework Documentation](https://docs.unity3d.com/Packages/com.unity.test-framework@latest)
- [NUnit Assertions Reference](https://docs.nunit.org/articles/nunit/writing-tests/assertions/assertion-models/constraint.html)
- [Unity Play Mode Testing Best Practices](https://docs.unity3d.com/Packages/com.unity.test-framework@latest/manual/workflow-create-playmode-test.html)

---

**Document Status:** READY FOR IMPLEMENTATION  
**Next Step:** Generate test code scaffolding with `*test-scaffold` workflow  
**Approval Required:** Test Architect (Murat) - APPROVED ✅
