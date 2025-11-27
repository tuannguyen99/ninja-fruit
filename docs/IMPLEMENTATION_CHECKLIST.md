# Implementation Checklist: STORY-001 TDD Workflow

**Start Date:** 2025-11-27  
**Story:** STORY-001 - FruitSpawner MVP  
**Engine:** Unity 6 (6000.0.62f1)  
**Target:** All 14 tests passing

---

## ✅ Setup Phase (Complete)

- [x] Unity 6 installed (6000.0.62f1)
- [x] Assets folder structure created
- [x] Test scaffolding generated (14 tests)
- [x] Assembly definitions created (Runtime + Tests)
- [x] Stub implementation ready (FruitSpawner.cs)
- [x] Test setup menu created (Window > Ninja Fruit > Setup Test Prefab)

---

## 📋 Step 1: Create Test Prefab (FIRST RUN)

**Action Required in Unity Editor:**

1. Open Unity Editor → Project folder: `c:\Users\Admin\Desktop\ai\games`
2. Wait for initial import to complete (~30 seconds)
3. Menu: `Window → Ninja Fruit → Setup Test Prefab`
   - This creates `Assets/Resources/Prefabs/TestFruit.prefab`
   - Creates "Fruit" tag automatically
   - Should see dialog: "Test Setup Complete"

**Verification:**
- [ ] File exists: `Assets/Resources/Prefabs/TestFruit.prefab`
- [ ] "Fruit" tag appears in Project Settings → Tags
- [ ] No console errors

**If Error:** "Can't find menu item"
- Re-import: `Assets → Reimport All`
- Restart Unity Editor
- Try menu again

---

## 🧪 Step 2: Verify Test Compilation

**In Unity Editor:**

1. Open Test Runner: `Window → General → Test Runner`
2. Click **EditMode** tab
3. You should see a list of 10 tests:
   - TEST-001: CalculateSpawnInterval_ScoreZero_Returns2Seconds
   - TEST-002: CalculateSpawnInterval_Score500_Returns1Second
   - TEST-003: CalculateSpawnInterval_Score1000_ReturnsMinimum0Point3Seconds
   - TEST-004: CalculateSpawnInterval_NegativeScore_ClampsTo2Seconds
   - TEST-005: CalculateFruitSpeed_ScoreZero_Returns2MetersPerSecond
   - TEST-006: CalculateFruitSpeed_Score1000_Returns3MetersPerSecond
   - TEST-007: CalculateFruitSpeed_Score5000_ReturnsMaximum7MetersPerSecond
   - TEST-008: CalculateFruitSpeed_Score10000_DoesNotExceed7MetersPerSecond
   - TEST-009: ShouldSpawnBomb_FruitCount9_ReturnsFalse
   - TEST-010: ShouldSpawnBomb_FruitCount10_ReturnsTrue

4. All tests should show as **"NotImplementedException"** (expected - stub throws)

**Verification:**
- [ ] All 10 Edit Mode tests appear in list
- [ ] No red errors in console
- [ ] Tests show as "NotImplemented"

---

## 🔧 Step 3: Implement CalculateSpawnInterval() [Make TEST-001 → TEST-004 Pass]

**Current Code (Stub):**
```csharp
public float CalculateSpawnInterval(int score)
{
    throw new System.NotImplementedException("Implement spawn interval calculation");
}
```

**Implementation:**
Replace in `Assets/Scripts/Gameplay/FruitSpawner.cs`:

```csharp
public float CalculateSpawnInterval(int score)
{
    // Formula: Max(0.3, 2.0 - (score / 500))
    float interval = 2.0f - (score / 500f);
    return Mathf.Max(0.3f, interval);
}
```

**Run Tests:**
1. In Test Runner, click **Run All** under EditMode
2. Expected result: ✅ **4/10 tests pass** (TEST-001 through TEST-004)

| Test | Expected | Status |
|------|----------|--------|
| TEST-001 | 2.0s @ score 0 | ✅ PASS |
| TEST-002 | 1.0s @ score 500 | ✅ PASS |
| TEST-003 | 0.3s @ score 1000 | ✅ PASS |
| TEST-004 | ≥2.0s @ score -100 | ✅ PASS |
| TEST-005 | NotImplemented | ❌ FAIL |
| ... | ... | ❌ FAIL |

**Verification:**
- [ ] TEST-001 passes (2.0s)
- [ ] TEST-002 passes (1.0s)
- [ ] TEST-003 passes (0.3s min cap)
- [ ] TEST-004 passes (negative score handling)
- [ ] Execution time < 100ms

---

## 🔧 Step 4: Implement CalculateFruitSpeed() [Make TEST-005 → TEST-008 Pass]

**Implementation:**
Replace in `Assets/Scripts/Gameplay/FruitSpawner.cs`:

```csharp
public float CalculateFruitSpeed(int score)
{
    // Formula: Min(7.0, 2.0 + (score / 1000))
    float speed = 2.0f + (score / 1000f);
    return Mathf.Min(7.0f, speed);
}
```

**Run Tests:**
1. Click **Run All** in EditMode tab
2. Expected result: ✅ **8/10 tests pass** (TEST-001 through TEST-008)

| Test | Expected | Status |
|------|----------|--------|
| TEST-005 | 2.0 m/s @ score 0 | ✅ PASS |
| TEST-006 | 3.0 m/s @ score 1000 | ✅ PASS |
| TEST-007 | 7.0 m/s @ score 5000 | ✅ PASS |
| TEST-008 | 7.0 m/s @ score 10000 | ✅ PASS |
| TEST-009 | NotImplemented | ❌ FAIL |
| TEST-010 | NotImplemented | ❌ FAIL |

**Verification:**
- [ ] TEST-005 passes (2.0 m/s)
- [ ] TEST-006 passes (3.0 m/s)
- [ ] TEST-007 passes (7.0 m/s max cap)
- [ ] TEST-008 passes (7.0 m/s enforced at extreme)

---

## 🔧 Step 5: Implement ShouldSpawnBomb() [Make TEST-009 → TEST-010 Pass]

**Implementation:**
Replace in `Assets/Scripts/Gameplay/FruitSpawner.cs`:

```csharp
public bool ShouldSpawnBomb(int fruitCount)
{
    // MVP: Deterministic logic - every 10th fruit (10% rate)
    return fruitCount > 0 && fruitCount % bombSpawnRate == 0;
}
```

**Run Tests:**
1. Click **Run All** in EditMode tab
2. Expected result: ✅ **10/10 Edit Mode tests pass**

| Test | Expected | Status |
|------|----------|--------|
| TEST-009 | false @ 9 fruits | ✅ PASS |
| TEST-010 | true @ 10 fruits | ✅ PASS |

**Verification:**
- [ ] TEST-009 passes (9 fruits → false)
- [ ] TEST-010 passes (10 fruits → true)
- [ ] **EditMode execution time < 100ms total**
- [ ] Console shows: `10/10 tests passed`

---

## 🔧 Step 6: Create TestFruit Prefab (Required for Play Mode)

**In Unity Editor:**

1. Already created via menu (Step 1), but verify:
   - File: `Assets/Resources/Prefabs/TestFruit.prefab`
   - Should have:
     - Transform (position 0,0,0)
     - Rigidbody2D (gravity 1.0, dynamic)
     - CircleCollider2D (radius 0.3)
     - Tag: "Fruit"

**Manual Creation (If Menu Setup Failed):**

1. Right-click Hierarchy → Create Empty → Name: "TestFruit"
2. Inspector:
   - Tag: "Fruit" (create tag if needed)
   - Add Component → Rigidbody2D
     - Gravity Scale: 1.0
     - Body Type: Dynamic
     - Collision Detection: Continuous
   - Add Component → CircleCollider2D
     - Radius: 0.3
3. Drag to folder: `Assets/Resources/Prefabs/` → Save as Prefab
4. Delete original from Hierarchy

**Verification:**
- [ ] File exists: `Assets/Resources/Prefabs/TestFruit.prefab`
- [ ] Has Rigidbody2D component
- [ ] Has CircleCollider2D component
- [ ] Tagged as "Fruit"

---

## 🔧 Step 7: Implement SpawnFruit() [Make TEST-011 → TEST-014 Pass]

**Current Code (Stub):**
```csharp
public void SpawnFruit()
{
    throw new System.NotImplementedException("Implement fruit spawning");
}
```

**Implementation:**

Add these fields to FruitSpawner class (top, after bombSpawnRate):
```csharp
[Header("Spawn Configuration")]
[SerializeField] private Vector2 spawnPosition = new Vector2(0, -5);
[SerializeField] private int currentScore = 0; // Will connect to ScoreManager later
```

Add implementation:
```csharp
public void SpawnFruit()
{
    // Validate prefabs exist
    if (fruitPrefabs == null || fruitPrefabs.Length == 0)
    {
        Debug.LogError("No fruit prefabs assigned to FruitSpawner");
        return;
    }
    
    // Select random fruit prefab
    GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
    
    // Instantiate fruit
    GameObject fruit = Instantiate(prefab, spawnPosition, Quaternion.identity);
    fruit.tag = "Fruit";
    
    // Apply initial velocity
    Rigidbody2D rb = fruit.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        float speed = CalculateFruitSpeed(currentScore);
        
        // Random horizontal direction, upward vertical
        float horizontalDirection = Random.Range(-1f, 1f);
        Vector2 velocity = new Vector2(horizontalDirection, 1f).normalized * speed;
        
        rb.velocity = velocity;
    }
}
```

**Setup for Play Mode Tests:**

1. In Test Runner, switch to **PlayMode** tab
2. You should see 4 tests listed:
   - TEST-011: SpawnFruit_CreatesGameObjectWithFruitTag
   - TEST-012: SpawnFruit_AttachesRigidbody2DComponent
   - TEST-013: SpawnFruit_AppliesInitialVelocity
   - TEST-014: SpawnFruit_MultipleCalls_CreatesMultipleFruits

3. Assign fruitPrefabs in FruitSpawnerTests.cs Setup():
   
   In `FruitSpawningIntegrationTests.cs`, find the Setup() method and modify:
   ```csharp
   [SetUp]
   public void Setup()
   {
       // Load test fruit prefab from Resources
       testPrefab = Resources.Load<GameObject>("Prefabs/TestFruit");
       Assert.IsNotNull(testPrefab, "TestFruit prefab must exist in Resources/Prefabs/");

       // Create spawner GameObject
       GameObject spawnerObject = new GameObject("TestSpawner");
       spawner = spawnerObject.AddComponent<FruitSpawner>();
       
       // ASSIGN PREFAB TO SPAWNER (NEW)
       var fruitPrefabsField = typeof(FruitSpawner)
           .GetField("fruitPrefabs", 
               System.Reflection.BindingFlags.NonPublic | 
               System.Reflection.BindingFlags.Instance);
       if (fruitPrefabsField != null)
       {
           fruitPrefabsField.SetValue(spawner, new GameObject[] { testPrefab });
       }
   }
   ```

**Run Play Mode Tests:**
1. Click **Run All** in PlayMode tab
2. Expected result: ✅ **4/4 Play Mode tests pass** (each ~1-2s)

| Test | Expected | Status |
|------|----------|--------|
| TEST-011 | GameObject with "Fruit" tag created | ✅ PASS |
| TEST-012 | Rigidbody2D component attached | ✅ PASS |
| TEST-013 | Upward velocity applied (>0) | ✅ PASS |
| TEST-014 | 3 independent fruits created | ✅ PASS |

**Verification:**
- [ ] TEST-011 passes (tag verification)
- [ ] TEST-012 passes (component existence)
- [ ] TEST-013 passes (velocity applied)
- [ ] TEST-014 passes (multiple spawns)
- [ ] **PlayMode execution time < 10s total**

---

## ✅ Step 8: FINAL VERIFICATION - Full Test Suite

**Run Complete Test Suite:**

1. **EditMode Tests:** Click **Run All** on EditMode tab
   - Expected: ✅ **10/10 passed** (<100ms)
   
2. **PlayMode Tests:** Click **Run All** on PlayMode tab
   - Expected: ✅ **4/4 passed** (<10s)

**Final Summary:**
```
COMPLETE TEST RUN RESULTS
========================
Edit Mode:   10/10 passed ✅ (Execution time: ~50ms)
Play Mode:    4/4 passed ✅ (Execution time: ~8s)
────────────────────────────
TOTAL:       14/14 PASSED ✅
```

**Success Indicators:**
- [ ] All 14 tests shown in green
- [ ] No red errors in console
- [ ] Total execution: <11 seconds
- [ ] Code coverage: Review coverage report in Test Runner

---

## 🎯 Victory Conditions - You Win When:

✅ All 14 tests pass (EditMode + PlayMode)  
✅ Zero console errors  
✅ Execution time < 11 seconds total  
✅ FruitSpawner.cs fully implemented (no more NotImplementedExceptions)  
✅ TestFruit.prefab working correctly  

---

## 📊 Progress Tracking

| Step | Task | Status | Duration |
|------|------|--------|----------|
| 1 | Create Test Prefab | ⏳ TODO | ~2 min |
| 2 | Verify Test Compilation | ⏳ TODO | ~1 min |
| 3 | Implement CalculateSpawnInterval() | ⏳ TODO | ~5 min |
| 4 | Implement CalculateFruitSpeed() | ⏳ TODO | ~5 min |
| 5 | Implement ShouldSpawnBomb() | ⏳ TODO | ~3 min |
| 6 | Verify TestFruit Prefab | ⏳ TODO | ~2 min |
| 7 | Implement SpawnFruit() | ⏳ TODO | ~10 min |
| 8 | Final Verification | ⏳ TODO | ~2 min |
| **TOTAL** | **Complete STORY-001** | **⏳ TODO** | **~30 min** |

---

**Estimated Total Time:** 25-35 minutes for complete TDD implementation  
**Next After Completion:** Generate test scaffolding for STORY-002 (SwipeDetector)

---

## 🚨 Troubleshooting

**Q: Tests don't compile - "FruitSpawner not found"**
- A: Make sure `Assets/Scripts/NinjaFruit.Runtime.asmdef` exists
- Verify assembly reference in `Assets/Tests/NinjaFruit.Tests.asmdef`
- Reimport: Assets → Reimport All

**Q: "TestFruit.prefab not found" error in PlayMode tests**
- A: Ensure `Assets/Resources/Prefabs/TestFruit.prefab` exists
- Run: Window → Ninja Fruit → Setup Test Prefab (if not already done)
- Check file path is exactly: `Assets/Resources/Prefabs/TestFruit.prefab`

**Q: Play Mode tests fail - "Rigidbody2D not found"**
- A: Verify TestFruit.prefab has Rigidbody2D attached
- Open prefab, check Inspector for components

**Q: All tests show "Not Implemented"**
- A: This is EXPECTED initially - stubs throw NotImplementedException
- Tests pass when methods are implemented (not throwing exception)
- Follow Step 3-7 to implement methods

**Q: Slow test execution (>15s for PlayMode)**
- A: Normal first run due to prefab loading
- Subsequent runs cache prefab in memory (~5-8s)
- Check system resources, close other apps

---

**Ready to begin? Open Unity Editor and start with Step 1:** `Window → Ninja Fruit → Setup Test Prefab`
