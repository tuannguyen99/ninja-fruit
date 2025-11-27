# 🚀 QUICK START: TDD IMPLEMENTATION for STORY-001

**Time Estimate:** 30 minutes  
**Goal:** Make all 14 tests pass  

---

## PHASE 1: PROJECT SETUP (3 minutes)

### 1. Open Unity Project
- Open Unity Hub
- Project: `c:\Users\Admin\Desktop\ai\games`
- Wait for import (~30s)

### 2. Create Test Prefab (Menu-Driven)
```
Window → Ninja Fruit → Setup Test Prefab
```
- Creates TestFruit.prefab automatically
- Creates "Fruit" tag
- Dialog confirms: "Test Setup Complete"

**Verify:**
- File exists: `Assets/Resources/Prefabs/TestFruit.prefab`
- Tag "Fruit" in Project Settings → Tags and Layers

---

## PHASE 2: TEST VERIFICATION (2 minutes)

### 3. Open Test Runner
```
Window → General → Test Runner
```

### 4. Check EditMode Tests
- Tab: **EditMode**
- Should list 10 tests (all showing "Not Implemented" - EXPECTED)
- Should have NO red errors

**Verify:**
- [x] 10 tests appear in list
- [x] No red error messages
- [x] Tests list loads without exceptions

---

## PHASE 3: IMPLEMENTATION (25 minutes)

### Step 1: Implement CalculateSpawnInterval() [3-5 min]

**File:** `Assets/Scripts/Gameplay/FruitSpawner.cs`  
**Find:** Line with `throw new System.NotImplementedException("Implement spawn interval calculation");`  
**Replace with:**
```csharp
public float CalculateSpawnInterval(int score)
{
    // Formula: Max(0.3, 2.0 - (score / 500))
    float interval = 2.0f - (score / 500f);
    return Mathf.Max(0.3f, interval);
}
```

**Test:**
```
Test Runner → EditMode → Run All
Expected: ✅ TEST-001, 002, 003, 004 PASS (4/10)
```

---

### Step 2: Implement CalculateFruitSpeed() [3-5 min]

**File:** `Assets/Scripts/Gameplay/FruitSpawner.cs`  
**Find:** Line with `throw new System.NotImplementedException("Implement fruit speed calculation");`  
**Replace with:**
```csharp
public float CalculateFruitSpeed(int score)
{
    // Formula: Min(7.0, 2.0 + (score / 1000))
    float speed = 2.0f + (score / 1000f);
    return Mathf.Min(7.0f, speed);
}
```

**Test:**
```
Test Runner → EditMode → Run All
Expected: ✅ TEST-001 through TEST-008 PASS (8/10)
```

---

### Step 3: Implement ShouldSpawnBomb() [2-3 min]

**File:** `Assets/Scripts/Gameplay/FruitSpawner.cs`  
**Find:** Line with `throw new System.NotImplementedException("Implement bomb spawn logic");`  
**Replace with:**
```csharp
public bool ShouldSpawnBomb(int fruitCount)
{
    // MVP: Deterministic logic - every 10th fruit (10% rate)
    return fruitCount > 0 && fruitCount % bombSpawnRate == 0;
}
```

**Test:**
```
Test Runner → EditMode → Run All
Expected: ✅ TEST-001 through TEST-010 PASS (10/10)
Execution: <100ms total ✅
```

---

### Step 4: Implement SpawnFruit() [8-10 min]

**File:** `Assets/Scripts/Gameplay/FruitSpawner.cs`

**Add these fields** (after `[SerializeField] private int bombSpawnRate = 10;`):
```csharp
[Header("Spawn Configuration")]
[SerializeField] private Vector2 spawnPosition = new Vector2(0, -5);
[SerializeField] private int currentScore = 0;
```

**Replace the SpawnFruit stub** with:
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
        float horizontalDirection = Random.Range(-1f, 1f);
        Vector2 velocity = new Vector2(horizontalDirection, 1f).normalized * speed;
        rb.velocity = velocity;
    }
}
```

**Update Play Mode Test Setup** (in `Assets/Tests/PlayMode/Gameplay/FruitSpawningIntegrationTests.cs`, in the `Setup()` method):

After `spawner = spawnerObject.AddComponent<FruitSpawner>();` add:
```csharp
// Assign prefab to spawner
var fruitPrefabsField = typeof(FruitSpawner)
    .GetField("fruitPrefabs", 
        System.Reflection.BindingFlags.NonPublic | 
        System.Reflection.BindingFlags.Instance);
if (fruitPrefabsField != null)
{
    fruitPrefabsField.SetValue(spawner, new GameObject[] { testPrefab });
}
```

**Test:**
```
Test Runner → PlayMode → Run All
Expected: ✅ TEST-011, 012, 013, 014 PASS (4/4)
Execution: <10s total ✅
```

---

## PHASE 4: FINAL VERIFICATION (2 minutes)

### Run Complete Test Suite

**EditMode Tab:**
```
Run All
Expected: ✅ 10/10 passed (<100ms)
```

**PlayMode Tab:**
```
Run All
Expected: ✅ 4/4 passed (<10s)
```

**Console Output Should Show:**
```
10/10 tests passed ✅
 4/4 tests passed ✅
─────────────────────
14/14 TOTAL PASSED ✅
```

---

## 🎯 SUCCESS = All 14 Tests Pass ✅

When complete:
- [x] All 14 tests passing
- [x] No console errors
- [x] Execution time < 11 seconds total
- [x] FruitSpawner fully implemented

---

## 📚 Documentation Reference

**Detailed Guide:** `docs/IMPLEMENTATION_CHECKLIST.md`  
**Test Specifications:** `docs/test-specs/test-spec-story-001-fruitspawner.md`  
**Test Scaffolding:** `docs/test-scaffolding/test-scaffolding-story-001-fruitspawner.md`  

---

**Next Step After Completion:** Generate test scaffolding for STORY-002 (SwipeDetector)
