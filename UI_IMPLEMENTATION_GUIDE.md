# UI Implementation - Next Steps Guide

**Project:** Ninja Fruit  
**Phase:** EPIC-004 UI & Game Flow  
**Approach:** Test-Driven Development (TDD)  
**Created:** November 30, 2025

---

## 📊 Current Project Status

### ✅ **Completed (EPIC-001 & EPIC-002)**
- **50 tests passing** (24 EditMode + 26 PlayMode)
- Core mechanics: Fruit spawning, swipe detection, collision, scoring
- Combo system, bombs, golden fruits fully functional
- Test infrastructure established

### 🎯 **Next Phase: UI Implementation**
**New Epic Created:** EPIC-004 - User Interface & Game Flow (13 points)

---

## 🚀 Quick Start: Story 010 (HUD Display)

### Step 1: Review Documentation
You have these new files to start with:
- `docs/epics/epic-ui-game-flow.md` - Full epic overview
- `docs/stories/story-010-hud-display.md` - Detailed story with TDD approach

### Step 2: Set Up Unity Scene (5 minutes)
```powershell
# Open Unity project
cd C:\Users\Admin\Desktop\ai\games\ninja-fruit
# Open Unity editor, create new scene
```

**Create UI Structure:**
1. Create new scene: `Assets/Scenes/GamePlay.unity` (if not exists)
2. Add Canvas (UI → Canvas)
3. Add EventSystem (auto-created with Canvas)
4. Configure Canvas:
   - Render Mode: Screen Space - Overlay
   - Canvas Scaler: Scale With Screen Size
   - Reference Resolution: 1920x1080

### Step 3: Write Tests First (TDD) ⚠️ IMPORTANT
```csharp
// Location: Assets/Tests/PlayMode/UI/HUDControllerTests.cs
// Copy test code from story-010-hud-display.md
```

**Run tests → They should FAIL** (this is expected, HUD not implemented yet)

### Step 4: Implement Minimal Code
```csharp
// Location: Assets/Scripts/UI/HUDController.cs
// Copy implementation from story-010-hud-display.md
```

Create UI elements in Unity Inspector:
- ScoreText (TextMeshProUGUI)
- ComboText (TextMeshProUGUI)  
- Life Hearts (3x Image components)

### Step 5: Run Tests Again
**Run tests → They should PASS** ✅

---

## 📋 TDD Workflow for Each Story

### Standard TDD Process:
```
1. RED   → Write test first (test fails)
2. GREEN → Write minimal code to pass test
3. REFACTOR → Clean up code while keeping tests passing
4. REPEAT → Add more tests for edge cases
```

### For Unity UI Testing:
```csharp
// Test pattern example
[UnityTest]
public IEnumerator HUD_ScoreUpdates_WhenPointsEarned()
{
    // ARRANGE - Set up test scene
    var hud = CreateTestHUD();
    var scoreManager = CreateTestScoreManager();
    
    // ACT - Trigger the behavior
    scoreManager.AddScore(100);
    yield return null; // Wait one frame for UI update
    
    // ASSERT - Verify the result
    Assert.AreEqual("100", hud.GetScoreText());
}
```

---

## 🎯 EPIC-004 Stories Breakdown

| Story | Title | Points | Status | Priority |
|-------|-------|--------|--------|----------|
| **010** | **HUD Display System** | **3** | **READY** | **START HERE** |
| 011 | Main Menu & Navigation | 3 | Backlog | Next |
| 012 | Game Over Screen | 2 | Backlog | After 011 |
| 013 | Pause Menu System | 2 | Backlog | After 012 |
| 014 | Visual Feedback Effects | 3 | Backlog | Polish |

**Recommended Order:** 010 → 011 → 012 → 013 → 014

---

## 🛠️ Required Unity Setup

### Install TextMeshPro (if not installed)
```
Window → TextMeshPro → Import TMP Essential Resources
```

### Create Assembly Definitions (if missing)
```
Assets/Scripts/NinjaFruit.Runtime.asmdef
Assets/Tests/NinjaFruit.Tests.asmdef
```

### Required Packages
```json
{
  "com.unity.textmeshpro": "3.0.8",
  "com.unity.test-framework": "1.4.5",
  "com.unity.ugui": "2.0.0"
}
```

---

## 📝 Test Utilities Already Available

From previous stories, you can reuse:
- `TestHelpers.CreateTestFruit()` - Create test fruits
- `TestHelpers.ResetSingletons()` - Clean up between tests
- `InputTestFixture` patterns from Story 002

**New Utilities Needed for Story 010:**
- `TestSceneSetup.CreateTestCanvas()` - Create Canvas for UI tests
- `TestSceneSetup.CreateTestHUD()` - Instantiate HUD for testing

*(Implementation provided in story-010-hud-display.md)*

---

## 🧪 Testing Strategy for UI

### What to Test:
✅ **DO Test:**
- UI element **visibility** (shown/hidden)
- Text **content** (displays correct values)
- **State transitions** (menu changes)
- **Event subscriptions** (UI updates on game events)
- **Initialization** (correct default values)

❌ **DON'T Test:**
- Visual appearance (colors, fonts, sizes)
- Animation smoothness
- Exact pixel positions
- Performance (separate performance tests)

### PlayMode Test Tips:
```csharp
// Wait one frame for UI updates
yield return null;

// Force layout recalculation (if using LayoutGroups)
Canvas.ForceUpdateCanvases();

// Check if UI element is visible
bool isVisible = uiElement.gameObject.activeSelf;

// Get text content
string text = textMeshPro.text;

// Simulate button click
button.onClick.Invoke();
```

---

## 🏁 Definition of Done Checklist

For **each story** to be considered DONE:
- [ ] All acceptance criteria have **passing tests**
- [ ] Tests follow **TDD approach** (written first)
- [ ] Code reviewed (self or peer)
- [ ] Manual smoke test (5 min playthrough)
- [ ] No console errors/warnings
- [ ] Test coverage ≥ 80% on new code
- [ ] Story status updated to `done` in `sprint-status.yaml`

---

## 📚 Key Documents Reference

### Epic & Stories
- **Epic:** `docs/epics/epic-ui-game-flow.md`
- **Story 010:** `docs/stories/story-010-hud-display.md` ← START HERE
- Sprint tracking: `docs/sprint-status.yaml`

### Technical Guides
- **Architecture:** `docs/game-architecture.md` (UI section)
- **GDD:** `docs/GDD.md` (User Interface section)
- **Test patterns:** `Assets/Tests/PlayMode/` (existing tests)

### Git Workflow
```powershell
# Create feature branch
git checkout -b feature/story-010-hud-display

# After tests pass and code is done
git add .
git commit -m "feat: Story 010 - HUD Display System (TDD)"
git push origin feature/story-010-hud-display
```

---

## 🎓 TDD Learning Resources

### Unity Testing Docs
- [Unity Test Framework Manual](https://docs.unity3d.com/Packages/com.unity.test-framework@latest)
- [PlayMode vs EditMode Tests](https://docs.unity3d.com/Manual/testing-editortestsrunner.html)

### TDD Best Practices
1. **Write test first** - See it fail before implementing
2. **Keep tests simple** - One assertion per test ideally
3. **Fast tests** - Use `yield return null`, not `WaitForSeconds`
4. **Isolated tests** - Each test cleans up after itself
5. **Descriptive names** - Test name describes what it tests

---

## 🐛 Common Issues & Solutions

### Issue: Tests can't find ScoreManager singleton
**Solution:** Create test instance in `[UnitySetUp]`
```csharp
GameObject scoreObj = new GameObject("ScoreManager");
scoreManager = scoreObj.AddComponent<ScoreManager>();
```

### Issue: TextMeshPro text not rendering in tests
**Solution:** It's OK! You're testing **content**, not rendering
```csharp
// This works even if text doesn't render visually
Assert.AreEqual("100", textMeshPro.text);
```

### Issue: UI doesn't update immediately
**Solution:** Wait one frame
```csharp
scoreManager.AddScore(100);
yield return null; // ← IMPORTANT
Assert.AreEqual("100", hud.GetScoreText());
```

### Issue: EventSystem not found
**Solution:** Create in test setup
```csharp
GameObject eventSystemObj = new GameObject("EventSystem");
eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
```

---

## 💡 Success Tips

### For Fast Prototyping:
1. **Copy test patterns** from Story 010 doc
2. **Run tests frequently** (every 5 minutes)
3. **Commit often** (every passing test)
4. **Ask AI for help** with Unity-specific code

### For Quality:
1. **Write tests BEFORE implementation** (TDD discipline)
2. **Keep tests readable** (clear arrange/act/assert)
3. **Test edge cases** (negative scores, max combos, etc.)
4. **Refactor after passing** (clean up while tests protect you)

---

## 🎯 Your Next Action

**IMMEDIATE NEXT STEP:**
1. Open `docs/stories/story-010-hud-display.md`
2. Read the full story (focus on Acceptance Criteria)
3. Copy test code from story doc to `Assets/Tests/PlayMode/UI/HUDControllerTests.cs`
4. Run tests → See them FAIL (RED phase)
5. Implement `HUDController.cs` from story doc
6. Run tests → See them PASS (GREEN phase) ✅
7. Celebrate! 🎉 You've completed Story 010 with TDD

**AFTER STORY 010:**
Request next story (011 - Main Menu) or continue through remaining UI stories.

---

## 📞 Need Help?

**If stuck, ask:**
- "Generate test plan for Story 010"
- "Show me how to test button clicks in Unity"
- "Help debug failing UI test"
- "Create Story 011 (Main Menu)"

**Resources:**
- Existing tests in `Assets/Tests/PlayMode/Gameplay/`
- Architecture doc: `docs/game-architecture.md`
- GDD specs: `docs/GDD.md`

---

**Good luck with your UI implementation! Remember: RED → GREEN → REFACTOR** 🚦

*Test-driven development isn't just about testing—it's about design. Writing tests first forces you to think about the API before implementation, leading to cleaner, more maintainable code.*
