# Test Code Scaffolding: STORY-001 - FruitSpawner MVP

**Generated:** 2025-11-27  
**Story:** STORY-001 - FruitSpawner MVP  
**Test Plan:** `test-plan-story-001-fruitspawner.md`  
**Test Spec:** `test-spec-story-001-fruitspawner.md`

---

## Generated Files

### Edit Mode Tests
- **File:** `Assets/Tests/EditMode/Gameplay/FruitSpawnerTests.cs`
- **Tests:** 10 unit tests (TEST-001 through TEST-010)
- **Coverage:** Spawn interval formulas, fruit speed formulas, bomb spawn logic
- **Framework:** NUnit (Unity Test Framework)
- **Execution:** Edit Mode (no Unity runtime required)
- **Target Duration:** <100ms total

### Play Mode Tests
- **File:** `Assets/Tests/PlayMode/Gameplay/FruitSpawningIntegrationTests.cs`
- **Tests:** 4 integration tests (TEST-011 through TEST-014)
- **Coverage:** GameObject instantiation, Rigidbody2D physics, velocity application
- **Framework:** NUnit with `[UnityTest]` coroutines
- **Execution:** Play Mode (requires Unity runtime)
- **Target Duration:** <10s total

### Assembly Definitions
- **Runtime Assembly:** `Assets/Scripts/NinjaFruit.Runtime.asmdef`
  - Contains production code (FruitSpawner, etc.)
  - Namespace: `NinjaFruit`
  - Platform: All
  
- **Test Assembly:** `Assets/Tests/NinjaFruit.Tests.asmdef`
  - Contains all test code
  - Namespace: `NinjaFruit.Tests`
  - Platform: Editor only
  - References: NinjaFruit.Runtime, Unity Test Runner

### Stub Implementation
- **File:** `Assets/Scripts/Gameplay/FruitSpawner.cs`
- **Purpose:** Stub class with method signatures for test compilation
- **Status:** NOT IMPLEMENTED (throws NotImplementedException)
- **Next Step:** Implement methods following TDD workflow (make tests pass)

---

## File Structure Created

```
Assets/
├── Scripts/
│   ├── NinjaFruit.Runtime.asmdef
│   └── Gameplay/
│       └── FruitSpawner.cs (STUB)
└── Tests/
    ├── NinjaFruit.Tests.asmdef
    ├── EditMode/
    │   └── Gameplay/
    │       └── FruitSpawnerTests.cs (10 tests)
    └── PlayMode/
        └── Gameplay/
            └── FruitSpawningIntegrationTests.cs (4 tests)
```

---

## Implementation Workflow (TDD)

### Step 1: Verify Test Compilation
```bash
# Open Unity Editor
# Window → General → Test Runner
# Verify all 14 tests appear in Test Runner
# All tests should show as "Not Implemented" (expected)
```

### Step 2: Implement CalculateSpawnInterval()
**Target:** Make TEST-001 through TEST-004 pass

Replace stub in `FruitSpawner.cs`:
```csharp
public float CalculateSpawnInterval(int score)
{
    // Formula: Max(0.3, 2.0 - (score / 500))
    float interval = 2.0f - (score / 500f);
    return Mathf.Max(0.3f, interval);
}
```

Run tests:
- TEST-001 ✅ (score 0 → 2.0s)
- TEST-002 ✅ (score 500 → 1.0s)
- TEST-003 ✅ (score 1000 → 0.3s min cap)
- TEST-004 ✅ (score -100 → ≥2.0s)

### Step 3: Implement CalculateFruitSpeed()
**Target:** Make TEST-005 through TEST-008 pass

Replace stub in `FruitSpawner.cs`:
```csharp
public float CalculateFruitSpeed(int score)
{
    // Formula: Min(7.0, 2.0 + (score / 1000))
    float speed = 2.0f + (score / 1000f);
    return Mathf.Min(7.0f, speed);
}
```

Run tests:
- TEST-005 ✅ (score 0 → 2.0 m/s)
- TEST-006 ✅ (score 1000 → 3.0 m/s)
- TEST-007 ✅ (score 5000 → 7.0 m/s max cap)
- TEST-008 ✅ (score 10000 → 7.0 m/s enforced)

### Step 4: Implement ShouldSpawnBomb()
**Target:** Make TEST-009 through TEST-010 pass

Replace stub in `FruitSpawner.cs`:
```csharp
public bool ShouldSpawnBomb(int fruitCount)
{
    // MVP: Deterministic logic - every 10th fruit
    return fruitCount % bombSpawnRate == 0 && fruitCount > 0;
}
```

Run tests:
- TEST-009 ✅ (9 fruits → false)
- TEST-010 ✅ (10 fruits → true)

### Step 5: Create Test Prefab
**Required for Play Mode tests (TEST-011 through TEST-014)**

Create `Assets/Resources/Prefabs/TestFruit.prefab`:
1. Create GameObject: Right-click Hierarchy → Create Empty → Name: "TestFruit"
2. Set Tag: Inspector → Tag → "Fruit" (create tag if needed)
3. Add Rigidbody2D: Add Component → Rigidbody2D
   - Gravity Scale: 1.0
   - Body Type: Dynamic
4. Add CircleCollider2D: Add Component → CircleCollider2D
   - Radius: 0.3
5. Save as Prefab: Drag to `Assets/Resources/Prefabs/TestFruit.prefab`

### Step 6: Implement SpawnFruit()
**Target:** Make TEST-011 through TEST-014 pass

Replace stub in `FruitSpawner.cs`:
```csharp
[Header("Spawn Configuration")]
[SerializeField] private Vector2 spawnPosition = new Vector2(0, -5);
[SerializeField] private int currentScore = 0; // In real game, get from ScoreManager

public void SpawnFruit()
{
    // Select random fruit prefab
    if (fruitPrefabs == null || fruitPrefabs.Length == 0)
    {
        Debug.LogError("No fruit prefabs assigned to FruitSpawner");
        return;
    }
    
    GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
    
    // Instantiate fruit
    GameObject fruit = Instantiate(prefab, spawnPosition, Quaternion.identity);
    fruit.tag = "Fruit";
    
    // Apply initial velocity
    Rigidbody2D rb = fruit.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        float speed = CalculateFruitSpeed(currentScore);
        Vector2 velocity = new Vector2(
            Random.Range(-1f, 1f), // Random horizontal velocity
            1f // Upward
        ).normalized * speed;
        
        rb.velocity = velocity;
    }
}
```

Run Play Mode tests:
- TEST-011 ✅ (spawns GameObject with "Fruit" tag)
- TEST-012 ✅ (has Rigidbody2D component)
- TEST-013 ✅ (applies upward velocity)
- TEST-014 ✅ (multiple spawns create independent fruits)

### Step 7: Run Full Test Suite
```bash
# In Unity Test Runner
# Mode: EditMode → Run All (should complete in <100ms)
# Mode: PlayMode → Run All (should complete in <10s)
# Expected: 14/14 tests passing ✅
```

---

## Test Execution Instructions

### In Unity Editor
1. Open Unity Test Runner: `Window → General → Test Runner`
2. Switch to `EditMode` tab
3. Click "Run All" to execute Edit Mode tests (10 tests)
4. Switch to `PlayMode` tab
5. Click "Run All" to execute Play Mode tests (4 tests)
6. View results in Test Runner panel

### Via Command Line (CI/CD)
```bash
# Edit Mode tests
Unity.exe -runTests -batchmode -projectPath . \
  -testPlatform EditMode \
  -testResults results-editmode.xml \
  -logFile -

# Play Mode tests
Unity.exe -runTests -batchmode -projectPath . \
  -testPlatform PlayMode \
  -testResults results-playmode.xml \
  -logFile -
```

### Expected Output
```
EditMode Tests: 10/10 passed (<100ms)
  ✅ TEST-001: CalculateSpawnInterval_ScoreZero_Returns2Seconds
  ✅ TEST-002: CalculateSpawnInterval_Score500_Returns1Second
  ✅ TEST-003: CalculateSpawnInterval_Score1000_ReturnsMinimum0Point3Seconds
  ✅ TEST-004: CalculateSpawnInterval_NegativeScore_ClampsTo2Seconds
  ✅ TEST-005: CalculateFruitSpeed_ScoreZero_Returns2MetersPerSecond
  ✅ TEST-006: CalculateFruitSpeed_Score1000_Returns3MetersPerSecond
  ✅ TEST-007: CalculateFruitSpeed_Score5000_ReturnsMaximum7MetersPerSecond
  ✅ TEST-008: CalculateFruitSpeed_Score10000_DoesNotExceed7MetersPerSecond
  ✅ TEST-009: ShouldSpawnBomb_FruitCount9_ReturnsFalse
  ✅ TEST-010: ShouldSpawnBomb_FruitCount10_ReturnsTrue

PlayMode Tests: 4/4 passed (<10s)
  ✅ TEST-011: SpawnFruit_CreatesGameObjectWithFruitTag
  ✅ TEST-012: SpawnFruit_AttachesRigidbody2DComponent
  ✅ TEST-013: SpawnFruit_AppliesInitialVelocity
  ✅ TEST-014: SpawnFruit_MultipleCalls_CreatesMultipleFruits

Total: 14/14 tests passed ✅
Coverage: 95%+ on FruitSpawner.cs
```

---

## Code Quality Notes

### Test Structure Best Practices
✅ Clear test names (method_scenario_expectedResult)  
✅ AAA pattern (Arrange, Act, Assert)  
✅ XML documentation with test ID, priority, risk  
✅ Tolerance for floating-point comparisons (0.001f)  
✅ Proper Setup/TearDown for test isolation  
✅ Descriptive assertion messages  

### Unity-Specific Best Practices
✅ Edit Mode for fast unit tests (no runtime overhead)  
✅ Play Mode for integration tests (Unity lifecycle)  
✅ Coroutines with `yield return null` for frame timing  
✅ `yield return WaitForFixedUpdate` for physics  
✅ Proper GameObject cleanup in TearDown  
✅ Resources folder for test prefabs  

### Assembly Definition Best Practices
✅ Separate runtime and test assemblies  
✅ Editor-only test assembly (faster compilation)  
✅ Explicit references to production code  
✅ NUnit precompiled references  

---

## Next Steps After Code Generation

### Immediate (Unity Setup Required)
1. Install Unity 6 (6000.0.25f1) via Unity Hub
2. Open project in Unity Editor
3. Verify Test Runner shows 14 tests
4. Create TestFruit.prefab in Resources/Prefabs/
5. Implement FruitSpawner methods (TDD workflow)
6. Run tests until all 14 pass

### Near-Term (Test Expansion)
7. Generate test scaffolding for STORY-002 (SwipeDetector)
8. Generate test scaffolding for STORY-003 (CollisionManager)
9. Implement components following TDD workflow
10. Achieve 80%+ test coverage on Epic-001

### CI/CD Integration (Phase 5)
11. Set up GitHub Actions workflows
12. Configure automated test execution
13. Add coverage reporting
14. Enable multi-platform builds

---

## Traceability

| File | Tests | Story AC | Risks | Priority |
|------|-------|----------|-------|----------|
| FruitSpawnerTests.cs | TEST-001 to TEST-004 | AC-2 | RISK-001 | P0 |
| FruitSpawnerTests.cs | TEST-005 to TEST-008 | AC-3 | RISK-002 | P1 |
| FruitSpawnerTests.cs | TEST-009 to TEST-010 | AC-4 | RISK-003 | P1 |
| FruitSpawningIntegrationTests.cs | TEST-011 | AC-1, AC-5 | RISK-004, RISK-006 | P1 |
| FruitSpawningIntegrationTests.cs | TEST-012 | AC-5 | RISK-005 | P1 |
| FruitSpawningIntegrationTests.cs | TEST-013 | AC-3 | RISK-002 | P1 |
| FruitSpawningIntegrationTests.cs | TEST-014 | AC-1 | RISK-004 | P2 |

**Total Coverage:** 100% of STORY-001 acceptance criteria  
**Total Tests:** 14 (10 Edit Mode + 4 Play Mode)  
**Estimated Execution Time:** <100ms Edit Mode + <10s Play Mode = <11s total

---

**Document Status:** READY FOR IMPLEMENTATION  
**Generated By:** Test Architect (Murat)  
**Approval:** APPROVED ✅  
**Next Action:** Open Unity Editor, verify test compilation, begin TDD implementation
