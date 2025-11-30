# Test Plan: STORY-010 HUD Display System

**Story:** STORY-010 - HUD Display System  
**Epic:** EPIC-004 - User Interface & Game Flow  
**Test Plan Version:** 1.0  
**Author:** Test Design Agent  
**Date:** November 30, 2025  
**Test Approach:** Test-Driven Development (TDD)

---

## Test Scope

### In Scope
- HUD initialization with default values
- Score display updates via events
- Lives display updates via events
- Combo multiplier display logic
- Event subscription/unsubscription
- UI element visibility states

### Out of Scope
- Visual appearance (colors, fonts, sizes)
- Animation smoothness or timing
- Performance/frame rate impact
- UI layout responsiveness (future story)
- Audio feedback for UI updates

---

## Test Environment

### Prerequisites
- Unity project with Test Framework installed
- TextMeshPro package installed and configured
- ScoreManager component implemented (Story 004)
- GameStateController component implemented (Story 003)
- Canvas and EventSystem in test scene

### Test Data
- **Score values:** 0, 50, 100, 1250, -50 (negative from bomb)
- **Lives values:** 3, 2, 1, 0
- **Combo multipliers:** 1x (hidden), 2x, 3x, 4x, 5x (max)
- **Fruit types:** Apple, Banana, Watermelon, Golden Apple

### Test Tools
- Unity Test Runner (Window → General → Test Runner)
- PlayMode test execution
- Test utilities: `TestSceneSetup`, `TestHelpers`

---

## Test Cases

### AC1: Score Display Updates

#### TC1.1 - Initial Score Display
**Test ID:** TC-010-001  
**Priority:** High  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Create test HUD with Canvas
2. Initialize HUD
3. Verify score text displays "0"

**Expected Result:** Score text shows "0"

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC001_InitialScoreDisplay_ShowsZero()
{
    var hud = CreateTestHUD();
    hud.Initialize();
    yield return null;
    Assert.AreEqual("0", hud.GetScoreText());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC1.2 - Score Updates on Points Earned
**Test ID:** TC-010-002  
**Priority:** Critical  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Create test HUD and ScoreManager
2. Initialize HUD with score 0
3. Trigger `ScoreManager.AddScore(100)`
4. Wait one frame
5. Verify HUD displays "100"

**Expected Result:** Score text updates to "100"

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC002_ScoreUpdates_WhenPointsEarned()
{
    var hud = CreateTestHUD();
    var scoreManager = CreateTestScoreManager();
    hud.Initialize();
    
    scoreManager.AddScore(100);
    yield return null;
    
    Assert.AreEqual("100", hud.GetScoreText());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC1.3 - Score Displays Large Numbers
**Test ID:** TC-010-003  
**Priority:** Medium  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Initialize HUD
2. Add score 1250
3. Verify displays "1250" (not "1,250" or "1250.0")

**Expected Result:** Score text shows "1250" as integer format

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC003_ScoreDisplays_LargeNumbers()
{
    var hud = CreateTestHUD();
    var scoreManager = CreateTestScoreManager();
    hud.Initialize();
    
    scoreManager.AddScore(1250);
    yield return null;
    
    Assert.AreEqual("1250", hud.GetScoreText());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC1.4 - Score Handles Negative Values
**Test ID:** TC-010-004  
**Priority:** High  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Initialize HUD with score 20
2. Hit bomb (penalty -50)
3. Verify score displays "-30"

**Expected Result:** Negative scores displayed correctly

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC004_ScoreHandles_NegativeValues()
{
    var hud = CreateTestHUD();
    var scoreManager = CreateTestScoreManager();
    hud.Initialize();
    
    scoreManager.AddScore(20);
    scoreManager.RegisterBombHit(); // -50 penalty
    yield return null;
    
    Assert.AreEqual("-30", hud.GetScoreText());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

### AC2: Lives Display Updates

#### TC2.1 - Initial Lives Display
**Test ID:** TC-010-005  
**Priority:** High  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Create HUD with 3 heart images
2. Initialize HUD
3. Verify all 3 hearts visible (full)

**Expected Result:** 3 hearts displayed as full

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC005_InitialLivesDisplay_ShowsThreeHearts()
{
    var hud = CreateTestHUD();
    hud.Initialize();
    yield return null;
    Assert.AreEqual(3, hud.GetVisibleHearts());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC2.2 - Lives Decrease on Miss
**Test ID:** TC-010-006  
**Priority:** Critical  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Initialize HUD (3 lives)
2. Register missed fruit via GameStateController
3. Verify 2 hearts visible, 1 grayed out

**Expected Result:** Lives display shows 2 full hearts

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC006_LivesDecrease_OnMissedFruit()
{
    var hud = CreateTestHUD();
    var gameController = CreateTestGameController();
    hud.Initialize();
    
    gameController.RegisterMissedFruit();
    yield return null;
    
    Assert.AreEqual(2, hud.GetVisibleHearts());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC2.3 - All Lives Lost Shows Empty Hearts
**Test ID:** TC-010-007  
**Priority:** High  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Initialize HUD (3 lives)
2. Miss 3 fruits (lives = 0)
3. Verify 0 hearts visible, all grayed out

**Expected Result:** All hearts shown as empty

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC007_AllLivesLost_ShowsEmptyHearts()
{
    var hud = CreateTestHUD();
    var gameController = CreateTestGameController();
    hud.Initialize();
    
    gameController.RegisterMissedFruit();
    gameController.RegisterMissedFruit();
    gameController.RegisterMissedFruit();
    yield return null;
    
    Assert.AreEqual(0, hud.GetVisibleHearts());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

### AC3: Combo Multiplier Display

#### TC3.1 - Combo Hidden Initially
**Test ID:** TC-010-008  
**Priority:** Medium  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Initialize HUD
2. Verify combo display is hidden (1x = no combo)

**Expected Result:** Combo UI element not visible

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC008_ComboHidden_Initially()
{
    var hud = CreateTestHUD();
    hud.Initialize();
    yield return null;
    Assert.IsFalse(hud.IsComboVisible());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC3.2 - Combo Displays 2x Multiplier
**Test ID:** TC-010-009  
**Priority:** High  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Initialize HUD
2. Slice 2 fruits within combo window
3. Verify combo shows "COMBO 2x!"

**Expected Result:** Combo text visible with "COMBO 2x!"

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC009_ComboDisplays_2xMultiplier()
{
    var hud = CreateTestHUD();
    var scoreManager = CreateTestScoreManager();
    hud.Initialize();
    
    scoreManager.RegisterSlice(FruitType.Apple);
    scoreManager.RegisterSlice(FruitType.Banana);
    yield return null;
    
    Assert.IsTrue(hud.IsComboVisible());
    Assert.AreEqual("COMBO 2x!", hud.GetComboText());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC3.3 - Combo Displays Maximum 5x
**Test ID:** TC-010-010  
**Priority:** Medium  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Initialize HUD
2. Slice 5+ fruits rapidly (combo 5x)
3. Verify combo shows "COMBO 5x!"

**Expected Result:** Combo capped at 5x (not 6x, 7x, etc.)

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC010_ComboDisplays_Maximum5x()
{
    var hud = CreateTestHUD();
    var scoreManager = CreateTestScoreManager();
    hud.Initialize();
    
    for (int i = 0; i < 6; i++)
    {
        scoreManager.RegisterSlice(FruitType.Apple);
    }
    yield return null;
    
    Assert.AreEqual("COMBO 5x!", hud.GetComboText());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC3.4 - Combo Resets on Bomb Hit
**Test ID:** TC-010-011  
**Priority:** High  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Build combo to 3x
2. Hit bomb (resets to 1x)
3. Verify combo display hidden

**Expected Result:** Combo UI hidden after bomb hit

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC011_ComboResets_OnBombHit()
{
    var hud = CreateTestHUD();
    var scoreManager = CreateTestScoreManager();
    hud.Initialize();
    
    // Build combo
    scoreManager.RegisterSlice(FruitType.Apple);
    scoreManager.RegisterSlice(FruitType.Banana);
    scoreManager.RegisterSlice(FruitType.Orange);
    yield return null;
    Assert.IsTrue(hud.IsComboVisible());
    
    // Hit bomb
    scoreManager.RegisterBombHit();
    yield return null;
    
    Assert.IsFalse(hud.IsComboVisible());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

### AC4: HUD Initialization

#### TC4.1 - All UI Elements Initialized
**Test ID:** TC-010-012  
**Priority:** Critical  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Create HUD from prefab
2. Call Initialize()
3. Verify score = 0, lives = 3, combo hidden

**Expected Result:** All UI elements in correct initial state

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC012_AllUIElements_Initialized()
{
    var hud = CreateTestHUD();
    hud.Initialize();
    yield return null;
    
    Assert.AreEqual("0", hud.GetScoreText());
    Assert.AreEqual(3, hud.GetVisibleHearts());
    Assert.IsFalse(hud.IsComboVisible());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

### AC5: Event-Driven Updates

#### TC5.1 - HUD Subscribes to Score Events
**Test ID:** TC-010-013  
**Priority:** Critical  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Initialize HUD (subscribes to events)
2. Fire ScoreManager.OnScoreChanged event (not calling HUD directly)
3. Verify HUD updated automatically

**Expected Result:** HUD reacts to event, not polling

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC013_HUD_SubscribesToScoreEvents()
{
    var hud = CreateTestHUD();
    var scoreManager = CreateTestScoreManager();
    hud.Initialize();
    
    // Trigger event (HUD should auto-update)
    scoreManager.AddScore(50);
    yield return null;
    
    Assert.AreEqual("50", hud.GetScoreText());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC5.2 - HUD Unsubscribes on Disable
**Test ID:** TC-010-014  
**Priority:** Medium  
**Type:** PlayMode Integration Test

**Test Steps:**
1. Initialize HUD (subscribed)
2. Disable HUD GameObject
3. Fire score event
4. Re-enable HUD
5. Verify HUD did NOT update while disabled

**Expected Result:** No memory leak from lingering subscriptions

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC014_HUD_UnsubscribesOnDisable()
{
    var hud = CreateTestHUD();
    var scoreManager = CreateTestScoreManager();
    hud.Initialize();
    
    hud.gameObject.SetActive(false);
    scoreManager.AddScore(100);
    yield return null;
    
    hud.gameObject.SetActive(true);
    yield return null;
    
    // Should still show 0, not 100
    Assert.AreEqual("0", hud.GetScoreText());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

## Test Execution Plan

### Phase 1: Red Phase (Write Tests)
**Duration:** 1 hour  
**Tasks:**
- Create `HUDControllerTests.cs` file
- Implement all 14 test cases
- Create test utilities (`CreateTestHUD`, `CreateTestCanvas`)
- Run tests → **Expect all to FAIL** ❌

**Success Criteria:** All tests fail with expected errors (not syntax errors)

---

### Phase 2: Green Phase (Implement Code)
**Duration:** 2-3 hours  
**Tasks:**
- Create `HUDController.cs` script
- Create HUD prefab in Unity scene
- Wire up SerializeField references
- Implement event subscriptions
- Run tests → **Expect all to PASS** ✅

**Success Criteria:** All 14 tests passing

---

### Phase 3: Refactor Phase (Clean Up)
**Duration:** 1 hour  
**Tasks:**
- Extract common code to helper methods
- Add XML documentation comments
- Optimize event subscription pattern
- Add null checks and error handling
- Run tests → **Still all PASS** ✅

**Success Criteria:** Tests still pass after refactoring

---

### Phase 4: Edge Cases (Additional Tests)
**Duration:** 1 hour  
**Tasks:**
- Add tests for null references
- Add tests for missing components
- Add tests for rapid state changes
- Run all tests → **All PASS** ✅

**Success Criteria:** 100% test coverage on HUDController

---

## Test Metrics

### Coverage Goals
- **Line Coverage:** 80%+ on HUDController.cs
- **Branch Coverage:** 70%+ (conditional logic)
- **Method Coverage:** 100% (all public methods tested)

### Success Criteria
- **Pass Rate:** 100% (all tests must pass)
- **Execution Time:** <5 seconds for all UI tests
- **Manual QA:** No visual bugs in 5-minute play test

---

## Risk Assessment

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| TextMeshPro not rendering in tests | High | Low | Test text content, not visual render |
| Event subscriptions causing memory leaks | Medium | High | Test OnDisable unsubscription |
| UI not updating on same frame | High | Medium | Use `yield return null` in tests |
| Missing EventSystem in test scene | High | High | Auto-create in TestSceneSetup |

---

## Test Deliverables

- [ ] `HUDControllerTests.cs` - 14 PlayMode test cases
- [ ] `TestSceneSetup.cs` - UI test utilities
- [ ] Test execution report (Unity Test Runner)
- [ ] Code coverage report (via Unity Coverage package)
- [ ] Manual test checklist (visual QA)

---

## Approval

**Test Plan Created By:** Test Design Agent  
**Reviewed By:** _____________  
**Approved By:** _____________  
**Date:** _____________

---

**Status:** READY FOR EXECUTION  
**Next Step:** Begin Phase 1 (Write Failing Tests)

*Remember: In TDD, a failing test is progress! It proves you're testing the right thing.*
