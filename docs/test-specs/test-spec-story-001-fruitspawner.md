# Test Specification: STORY-001 - FruitSpawner MVP

**Generated:** 2025-11-27  
**Story:** STORY-001 - FruitSpawner MVP  
**Epic:** EPIC-001 - Core Slicing Mechanics  
**Test Plan Reference:** `test-plan-story-001-fruitspawner.md`  
**Test Architect:** Murat

---

## Document Purpose

This specification provides detailed test case definitions with Given/When/Then format, expected results, and validation criteria for all 12 tests identified in the test plan. Use this document to implement test code with precision.

---

## Edit Mode Test Specifications

### Test Suite: SpawnIntervalCalculationTests

**File:** `Assets/Tests/EditMode/Gameplay/FruitSpawnerTests.cs`  
**Purpose:** Validate spawn interval formula accuracy

---

#### TEST-001: CalculateSpawnInterval_ScoreZero_Returns2Seconds

**Priority:** P0  
**Risk:** RISK-001 (HIGH)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <1ms

**Given:**
- FruitSpawner component is instantiated
- Current game score is 0

**When:**
- `CalculateSpawnInterval(0)` method is called

**Then:**
- Method returns exactly `2.0f` seconds
- No exceptions are thrown

**Test Data:**
```csharp
int inputScore = 0;
float expectedInterval = 2.0f;
float tolerance = 0.001f;
```

**Validation:**
```csharp
Assert.AreEqual(expectedInterval, actualInterval, tolerance, 
    "At score 0, spawn interval should be 2.0 seconds (max difficulty)");
```

**Edge Cases Covered:**
- Initial game state (score zero)
- Maximum interval boundary

---

#### TEST-002: CalculateSpawnInterval_Score500_Returns1Second

**Priority:** P0  
**Risk:** RISK-001 (HIGH)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <1ms

**Given:**
- FruitSpawner component is instantiated
- Current game score is 500

**When:**
- `CalculateSpawnInterval(500)` method is called

**Then:**
- Method returns exactly `1.0f` seconds
- Formula calculates: `Max(0.3, 2.0 - (500 / 500))` = `Max(0.3, 1.0)` = `1.0`

**Test Data:**
```csharp
int inputScore = 500;
float expectedInterval = 1.0f;
float tolerance = 0.001f;
```

**Validation:**
```csharp
Assert.AreEqual(expectedInterval, actualInterval, tolerance, 
    "At score 500, spawn interval should be 1.0 seconds (mid-game difficulty)");
```

**Edge Cases Covered:**
- Mid-game difficulty progression
- Linear interpolation validation

---

#### TEST-003: CalculateSpawnInterval_Score1000_ReturnsMinimum0Point3Seconds

**Priority:** P0  
**Risk:** RISK-001 (HIGH)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <1ms

**Given:**
- FruitSpawner component is instantiated
- Current game score is 1000

**When:**
- `CalculateSpawnInterval(1000)` method is called

**Then:**
- Method returns exactly `0.3f` seconds
- Formula calculates: `Max(0.3, 2.0 - (1000 / 500))` = `Max(0.3, 0.0)` = `0.3`
- Minimum boundary is enforced

**Test Data:**
```csharp
int inputScore = 1000;
float expectedInterval = 0.3f; // Minimum cap
float tolerance = 0.001f;
```

**Validation:**
```csharp
Assert.AreEqual(expectedInterval, actualInterval, tolerance, 
    "At score 1000, spawn interval should be capped at minimum 0.3 seconds");
```

**Edge Cases Covered:**
- Minimum interval boundary enforcement
- Max() function behavior
- High-score difficulty cap

---

#### TEST-004: CalculateSpawnInterval_NegativeScore_ClampsTo2Seconds

**Priority:** P0  
**Risk:** RISK-001 (HIGH)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <1ms

**Given:**
- FruitSpawner component is instantiated
- Current game score is -100 (edge case: bomb penalties causing negative score)

**When:**
- `CalculateSpawnInterval(-100)` method is called

**Then:**
- Method returns at least `2.0f` seconds (or handles gracefully)
- Formula calculates: `Max(0.3, 2.0 - (-100 / 500))` = `Max(0.3, 2.2)` = `2.2`
- No exceptions or undefined behavior

**Test Data:**
```csharp
int inputScore = -100;
float expectedMinInterval = 2.0f;
float tolerance = 0.001f;
```

**Validation:**
```csharp
float actualInterval = spawner.CalculateSpawnInterval(inputScore);
Assert.GreaterOrEqual(actualInterval, expectedMinInterval, 
    "Negative scores should not reduce spawn interval below initial difficulty");
```

**Edge Cases Covered:**
- Defensive programming validation
- Negative score handling
- Robustness under unusual game states

---

### Test Suite: FruitSpeedCalculationTests

**File:** `Assets/Tests/EditMode/Gameplay/FruitSpawnerTests.cs`  
**Purpose:** Validate fruit speed formula accuracy

---

#### TEST-005: CalculateFruitSpeed_ScoreZero_Returns2MetersPerSecond

**Priority:** P1  
**Risk:** RISK-002 (MEDIUM)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <1ms

**Given:**
- FruitSpawner component is instantiated
- Current game score is 0

**When:**
- `CalculateFruitSpeed(0)` method is called

**Then:**
- Method returns exactly `2.0f` m/s
- Formula calculates: `Min(7.0, 2.0 + (0 / 1000))` = `Min(7.0, 2.0)` = `2.0`

**Test Data:**
```csharp
int inputScore = 0;
float expectedSpeed = 2.0f;
float tolerance = 0.001f;
```

**Validation:**
```csharp
Assert.AreEqual(expectedSpeed, actualSpeed, tolerance, 
    "At score 0, fruit speed should be 2.0 m/s (initial speed)");
```

**Edge Cases Covered:**
- Initial game speed
- Minimum speed boundary

---

#### TEST-006: CalculateFruitSpeed_Score1000_Returns3MetersPerSecond

**Priority:** P1  
**Risk:** RISK-002 (MEDIUM)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <1ms

**Given:**
- FruitSpawner component is instantiated
- Current game score is 1000

**When:**
- `CalculateFruitSpeed(1000)` method is called

**Then:**
- Method returns exactly `3.0f` m/s
- Formula calculates: `Min(7.0, 2.0 + (1000 / 1000))` = `Min(7.0, 3.0)` = `3.0`

**Test Data:**
```csharp
int inputScore = 1000;
float expectedSpeed = 3.0f;
float tolerance = 0.001f;
```

**Validation:**
```csharp
Assert.AreEqual(expectedSpeed, actualSpeed, tolerance, 
    "At score 1000, fruit speed should be 3.0 m/s");
```

**Edge Cases Covered:**
- Mid-game speed progression
- Linear speed increase validation

---

#### TEST-007: CalculateFruitSpeed_Score5000_ReturnsMaximum7MetersPerSecond

**Priority:** P1  
**Risk:** RISK-002 (MEDIUM)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <1ms

**Given:**
- FruitSpawner component is instantiated
- Current game score is 5000

**When:**
- `CalculateFruitSpeed(5000)` method is called

**Then:**
- Method returns exactly `7.0f` m/s
- Formula calculates: `Min(7.0, 2.0 + (5000 / 1000))` = `Min(7.0, 7.0)` = `7.0`
- Maximum boundary is reached

**Test Data:**
```csharp
int inputScore = 5000;
float expectedSpeed = 7.0f; // Maximum cap
float tolerance = 0.001f;
```

**Validation:**
```csharp
Assert.AreEqual(expectedSpeed, actualSpeed, tolerance, 
    "At score 5000, fruit speed should reach maximum cap of 7.0 m/s");
```

**Edge Cases Covered:**
- Maximum speed boundary enforcement
- Min() function behavior at boundary

---

#### TEST-008: CalculateFruitSpeed_Score10000_DoesNotExceed7MetersPerSecond

**Priority:** P1  
**Risk:** RISK-002 (MEDIUM)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <1ms

**Given:**
- FruitSpawner component is instantiated
- Current game score is 10000 (extreme high score)

**When:**
- `CalculateFruitSpeed(10000)` method is called

**Then:**
- Method returns exactly `7.0f` m/s (not higher)
- Formula calculates: `Min(7.0, 2.0 + (10000 / 1000))` = `Min(7.0, 12.0)` = `7.0`
- Maximum cap is strictly enforced

**Test Data:**
```csharp
int inputScore = 10000;
float expectedMaxSpeed = 7.0f;
float tolerance = 0.001f;
```

**Validation:**
```csharp
float actualSpeed = spawner.CalculateFruitSpeed(inputScore);
Assert.AreEqual(expectedMaxSpeed, actualSpeed, tolerance, 
    "At extreme scores, fruit speed must not exceed 7.0 m/s cap");
Assert.LessOrEqual(actualSpeed, expectedMaxSpeed, 
    "Speed cap enforcement validation");
```

**Edge Cases Covered:**
- Extreme value handling
- Maximum cap enforcement beyond boundary
- Game balance preservation at high scores

---

### Test Suite: BombSpawnLogicTests

**File:** `Assets/Tests/EditMode/Gameplay/FruitSpawnerTests.cs`  
**Purpose:** Validate bomb spawn frequency

---

#### TEST-009: ShouldSpawnBomb_FruitCount9_ReturnsFalse

**Priority:** P1  
**Risk:** RISK-003 (MEDIUM)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <1ms

**Given:**
- FruitSpawner component is instantiated
- 9 fruits have been spawned since last bomb

**When:**
- `ShouldSpawnBomb(9)` method is called

**Then:**
- Method returns `false`
- Bomb should NOT spawn yet (10% rate = 1 per 10 fruits)

**Test Data:**
```csharp
int fruitCount = 9;
bool expectedResult = false;
```

**Validation:**
```csharp
bool shouldSpawn = spawner.ShouldSpawnBomb(fruitCount);
Assert.IsFalse(shouldSpawn, 
    "Bomb should not spawn before 10 fruits have spawned");
```

**Edge Cases Covered:**
- Pre-threshold bomb spawn prevention
- 10% frequency validation (1 per 10)

---

#### TEST-010: ShouldSpawnBomb_FruitCount10_ReturnsTrue

**Priority:** P1  
**Risk:** RISK-003 (MEDIUM)  
**Type:** Unit Test (Edit Mode)  
**Estimated Duration:** <1ms

**Given:**
- FruitSpawner component is instantiated
- Exactly 10 fruits have been spawned since last bomb

**When:**
- `ShouldSpawnBomb(10)` method is called

**Then:**
- Method returns `true`
- Bomb should spawn now (10% rate = 1 per 10 fruits)

**Test Data:**
```csharp
int fruitCount = 10;
bool expectedResult = true;
```

**Validation:**
```csharp
bool shouldSpawn = spawner.ShouldSpawnBomb(fruitCount);
Assert.IsTrue(shouldSpawn, 
    "Bomb should spawn after exactly 10 fruits (10% rate)");
```

**Edge Cases Covered:**
- Threshold boundary exactness
- Deterministic bomb spawn logic

**Implementation Note:**
MVP uses deterministic logic (every 10th fruit). Future enhancement may use random 10% chance, which would require different test approach (statistical validation over many runs).

---

## Play Mode Test Specifications

### Test Suite: FruitSpawningIntegrationTests

**File:** `Assets/Tests/PlayMode/Gameplay/FruitSpawningIntegrationTests.cs`  
**Purpose:** Validate actual Unity GameObject instantiation and physics setup

---

#### TEST-011: SpawnFruit_CreatesGameObjectWithFruitTag

**Priority:** P1  
**Risk:** RISK-004, RISK-006 (MEDIUM)  
**Type:** Integration Test (Play Mode)  
**Estimated Duration:** ~1s

**Given:**
- FruitSpawner component exists in scene
- Valid fruit prefab is assigned (loaded from `Resources/Prefabs/TestFruit.prefab`)
- Scene is empty (no existing fruits)

**When:**
- `SpawnFruit()` method is called

**Then:**
- A new GameObject is instantiated in the scene
- GameObject has tag `"Fruit"`
- GameObject is not null

**Test Data:**
```csharp
string prefabPath = "Prefabs/TestFruit";
string expectedTag = "Fruit";
```

**Setup:**
```csharp
[SetUp]
public void Setup()
{
    testPrefab = Resources.Load<GameObject>("Prefabs/TestFruit");
    Assert.IsNotNull(testPrefab, "TestFruit prefab must exist in Resources/Prefabs/");
    
    GameObject spawnerObject = new GameObject("TestSpawner");
    spawner = spawnerObject.AddComponent<FruitSpawner>();
    // Assign prefab to spawner's serialized array
}
```

**Validation:**
```csharp
[UnityTest]
public IEnumerator SpawnFruit_CreatesGameObjectWithFruitTag()
{
    // Act
    spawner.SpawnFruit();
    yield return null; // Wait one frame for instantiation
    
    // Assert
    GameObject spawnedFruit = GameObject.FindWithTag("Fruit");
    Assert.IsNotNull(spawnedFruit, "A GameObject with tag 'Fruit' should exist after spawning");
    Assert.AreEqual("Fruit", spawnedFruit.tag, "Spawned GameObject must have 'Fruit' tag");
}
```

**Teardown:**
```csharp
[TearDown]
public void Teardown()
{
    GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
    foreach (var fruit in fruits)
    {
        Object.Destroy(fruit);
    }
    Object.Destroy(spawner.gameObject);
}
```

**Edge Cases Covered:**
- Prefab instantiation success
- Tag assignment correctness
- Scene pollution prevention (teardown)

---

#### TEST-012: SpawnFruit_AttachesRigidbody2DComponent

**Priority:** P1  
**Risk:** RISK-005 (MEDIUM)  
**Type:** Integration Test (Play Mode)  
**Estimated Duration:** ~1s

**Given:**
- FruitSpawner component exists in scene
- Valid fruit prefab is assigned with Rigidbody2D component
- Scene is empty

**When:**
- `SpawnFruit()` method is called

**Then:**
- Spawned GameObject has `Rigidbody2D` component attached
- Rigidbody2D is not null
- Rigidbody2D gravity scale is set (e.g., 1.0)

**Test Data:**
```csharp
float expectedGravityScale = 1.0f;
```

**Validation:**
```csharp
[UnityTest]
public IEnumerator SpawnFruit_AttachesRigidbody2DComponent()
{
    // Act
    spawner.SpawnFruit();
    yield return null;
    
    // Assert
    GameObject spawnedFruit = GameObject.FindWithTag("Fruit");
    Assert.IsNotNull(spawnedFruit, "Fruit must be spawned");
    
    Rigidbody2D rb = spawnedFruit.GetComponent<Rigidbody2D>();
    Assert.IsNotNull(rb, "Spawned fruit must have Rigidbody2D component");
    Assert.AreEqual(expectedGravityScale, rb.gravityScale, 0.01f, 
        "Rigidbody2D gravity scale should be configured");
}
```

**Edge Cases Covered:**
- Component existence validation
- Physics component configuration
- Prefab integrity verification

---

#### TEST-013: SpawnFruit_AppliesInitialVelocity

**Priority:** P1  
**Risk:** RISK-002 (MEDIUM)  
**Type:** Integration Test (Play Mode)  
**Estimated Duration:** ~2s

**Given:**
- FruitSpawner component exists with configured launch velocity
- Valid fruit prefab with Rigidbody2D is assigned
- Physics simulation is active

**When:**
- `SpawnFruit()` method is called
- One physics frame elapses

**Then:**
- Spawned fruit has upward velocity (velocity.y > 0)
- Velocity magnitude approximately matches calculated speed from GDD
- Fruit moves in parabolic arc

**Test Data:**
```csharp
float expectedMinVelocityY = 2.0f; // Minimum upward velocity
int scoreForTest = 0; // Use initial difficulty
float expectedSpeed = 2.0f; // CalculateFruitSpeed(0)
```

**Validation:**
```csharp
[UnityTest]
public IEnumerator SpawnFruit_AppliesInitialVelocity()
{
    // Arrange
    int currentScore = 0;
    float expectedSpeed = spawner.CalculateFruitSpeed(currentScore);
    
    // Act
    spawner.SpawnFruit();
    yield return new WaitForFixedUpdate(); // Wait for physics update
    
    // Assert
    GameObject spawnedFruit = GameObject.FindWithTag("Fruit");
    Rigidbody2D rb = spawnedFruit.GetComponent<Rigidbody2D>();
    
    Assert.Greater(rb.velocity.y, 0f, "Fruit should have upward velocity");
    
    float actualSpeed = rb.velocity.magnitude;
    Assert.AreEqual(expectedSpeed, actualSpeed, 0.5f, 
        "Initial velocity magnitude should approximately match calculated speed");
}
```

**Edge Cases Covered:**
- Physics initialization timing
- Velocity application correctness
- GDD formula integration validation

---

#### TEST-014: SpawnFruit_MultipleCalls_CreatesMultipleFruits

**Priority:** P2  
**Risk:** RISK-004 (MEDIUM)  
**Type:** Integration Test (Play Mode)  
**Estimated Duration:** ~1s

**Given:**
- FruitSpawner component exists
- Valid fruit prefab is assigned
- Scene is empty

**When:**
- `SpawnFruit()` method is called 3 times consecutively

**Then:**
- Scene contains exactly 3 GameObjects with tag `"Fruit"`
- Each fruit has independent Rigidbody2D component
- No singleton/caching issues exist

**Test Data:**
```csharp
int numberOfSpawns = 3;
int expectedFruitCount = 3;
```

**Validation:**
```csharp
[UnityTest]
public IEnumerator SpawnFruit_MultipleCalls_CreatesMultipleFruits()
{
    // Act
    spawner.SpawnFruit();
    yield return null;
    spawner.SpawnFruit();
    yield return null;
    spawner.SpawnFruit();
    yield return null;
    
    // Assert
    GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
    Assert.AreEqual(expectedFruitCount, fruits.Length, 
        "Should spawn exactly 3 independent fruits");
    
    // Verify each has independent Rigidbody2D
    foreach (var fruit in fruits)
    {
        Rigidbody2D rb = fruit.GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb, "Each fruit must have its own Rigidbody2D");
    }
}
```

**Edge Cases Covered:**
- Multiple instantiation support
- Instance independence
- No object pooling side effects (not implemented yet)

---

## Test Data Assets Required

### Prefab: TestFruit.prefab

**Location:** `Assets/Resources/Prefabs/TestFruit.prefab`

**Components:**
1. **Transform**
   - Position: (0, 0, 0)
   - Rotation: (0, 0, 0)
   - Scale: (1, 1, 1)

2. **Rigidbody2D**
   - Body Type: Dynamic
   - Gravity Scale: 1.0
   - Mass: 1.0
   - Linear Drag: 0
   - Angular Drag: 0.05
   - Collision Detection: Continuous

3. **CircleCollider2D**
   - Radius: 0.3
   - Is Trigger: false

4. **Tag:** `Fruit`

**Creation Script:**
```csharp
// Unity Editor script to create test prefab
GameObject fruit = new GameObject("TestFruit");
fruit.tag = "Fruit";

Rigidbody2D rb = fruit.AddComponent<Rigidbody2D>();
rb.gravityScale = 1.0f;
rb.mass = 1.0f;

CircleCollider2D collider = fruit.AddComponent<CircleCollider2D>();
collider.radius = 0.3f;

// Save as prefab to Resources/Prefabs/TestFruit.prefab
```

---

## Test Execution Order

### Recommended Execution Sequence

**Phase 1: Edit Mode (Formula Validation)**
1. TEST-001 → TEST-004: Spawn interval tests
2. TEST-005 → TEST-008: Fruit speed tests
3. TEST-009 → TEST-010: Bomb spawn logic tests

**Phase 2: Play Mode (Integration)**
4. TEST-011: Basic instantiation
5. TEST-012: Component verification
6. TEST-013: Physics validation
7. TEST-014: Multiple spawn support

**Rationale:** Fail fast on formula errors (Edit Mode) before expensive Unity runtime tests (Play Mode).

---

## Success Criteria Per Test

| Test ID | Pass Criteria | Fail Criteria |
|---------|---------------|---------------|
| TEST-001 | Returns 2.0f ±0.001 | Returns any other value or throws exception |
| TEST-002 | Returns 1.0f ±0.001 | Returns any other value |
| TEST-003 | Returns 0.3f ±0.001 (minimum enforced) | Returns <0.3f or >0.3f |
| TEST-004 | Returns ≥2.0f (handles negative gracefully) | Returns <2.0f or throws exception |
| TEST-005 | Returns 2.0f ±0.001 | Returns any other value |
| TEST-006 | Returns 3.0f ±0.001 | Returns any other value |
| TEST-007 | Returns 7.0f ±0.001 (maximum enforced) | Returns >7.0f |
| TEST-008 | Returns exactly 7.0f (cap enforced) | Returns >7.0f |
| TEST-009 | Returns false | Returns true |
| TEST-010 | Returns true | Returns false |
| TEST-011 | Fruit spawned with "Fruit" tag | No fruit spawned or wrong tag |
| TEST-012 | Rigidbody2D attached and configured | Rigidbody2D missing or null |
| TEST-013 | Upward velocity applied | No velocity or downward velocity |
| TEST-014 | Exactly 3 independent fruits exist | Wrong count or shared components |

---

## Traceability Matrix

| Test ID | Story AC | Risk ID | Priority | GDD Section |
|---------|----------|---------|----------|-------------|
| TEST-001 | AC-2 | RISK-001 | P0 | Progression & Balance → Difficulty Curve |
| TEST-002 | AC-2 | RISK-001 | P0 | Progression & Balance → Difficulty Curve |
| TEST-003 | AC-2 | RISK-001 | P0 | Progression & Balance → Difficulty Curve |
| TEST-004 | AC-2 | RISK-001 | P0 | (Edge case - robustness) |
| TEST-005 | AC-3 | RISK-002 | P1 | Progression & Balance → Difficulty Curve |
| TEST-006 | AC-3 | RISK-002 | P1 | Progression & Balance → Difficulty Curve |
| TEST-007 | AC-3 | RISK-002 | P1 | Progression & Balance → Difficulty Curve |
| TEST-008 | AC-3 | RISK-002 | P1 | (Edge case - cap enforcement) |
| TEST-009 | AC-4 | RISK-003 | P1 | Game Mechanics → Bomb Mechanics |
| TEST-010 | AC-4 | RISK-003 | P1 | Game Mechanics → Bomb Mechanics |
| TEST-011 | AC-1, AC-5 | RISK-004, RISK-006 | P1 | Game Mechanics → Fruit Spawning |
| TEST-012 | AC-5 | RISK-005 | P1 | Game Mechanics → Fruit Spawning |
| TEST-013 | AC-3 | RISK-002 | P1 | Game Mechanics → Fruit Spawning |
| TEST-014 | AC-1 | RISK-004 | P2 | (Robustness - multiple spawns) |

---

## Implementation Notes

### Common Setup Pattern

```csharp
private FruitSpawner spawner;

[SetUp]
public void Setup()
{
    GameObject spawnerObject = new GameObject("TestSpawner");
    spawner = spawnerObject.AddComponent<FruitSpawner>();
    // Configure serialized fields if needed
}

[TearDown]
public void Teardown()
{
    Object.DestroyImmediate(spawner.gameObject); // Edit Mode
    // or
    Object.Destroy(spawner.gameObject); // Play Mode
}
```

### Floating-Point Comparison Best Practice

Always use tolerance for float comparisons:
```csharp
Assert.AreEqual(expected, actual, 0.001f, "Reason for assertion");
```

### Play Mode Timing Best Practice

Use coroutines with yields for Unity lifecycle timing:
```csharp
[UnityTest]
public IEnumerator TestName()
{
    // Act
    spawner.SpawnFruit();
    yield return null; // Wait one frame
    
    // For physics:
    yield return new WaitForFixedUpdate(); // Wait for physics update
    
    // Assert
    // ...
}
```

---

**Document Status:** READY FOR CODE GENERATION  
**Next Step:** Generate C# test code scaffolding  
**Approval:** Test Architect (Murat) - APPROVED ✅
