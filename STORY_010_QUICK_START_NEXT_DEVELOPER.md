# 🚀 Quick Start: Continuing from Story 010

**Last Updated:** November 30, 2025  
**Current Status:** Story 010 ✅ COMPLETE  
**Next Story:** Story 011 - Main Menu (Ready to Start)

---

## ⚡ 60-Second Summary

### What Was Built
- ✅ HUD display system with score, lives, combo
- ✅ Game state controller for state management
- ✅ 14 automated UI tests (all passing)
- ✅ Test utilities library for future UI tests

### How It Works
```
Game Logic (Managers) → Events → HUD (UI) → Display Update
```

### Test Results
```
Total Tests: 64
Passing: 64
Success Rate: 100%
Time to Run: ~0.84 seconds
```

---

## 📁 Key Files to Know

### Production Code
| File | Purpose | Location |
|------|---------|----------|
| GameStateController.cs | Game state machine | Assets/Scripts/Gameplay/ |
| HUDController.cs | Main UI component | Assets/Scripts/UI/ |
| ScoreManager.cs | Score tracking | Assets/Scripts/Gameplay/ |
| ComboMultiplier.cs | Combo system | Assets/Scripts/Gameplay/ |

### Test Code
| File | Purpose | Location |
|------|---------|----------|
| HUDControllerTests.cs | 14 UI tests | Assets/Tests/PlayMode/UI/ |
| UITestHelpers.cs | Test utilities | Assets/Tests/Setup/ |

### Documentation
| File | Purpose |
|------|---------|
| STORY_010_SUMMARY.md | What was done |
| STORY_010_RETROSPECTIVE.md | What was learned |
| STORY_010_AUDIT_AND_COMPLETION.md | Code review |
| This file | Quick reference |

---

## 🧪 Running Tests

### In Unity Editor
```
1. Window → Test Runner
2. PlayMode tab
3. Click "Run All"
4. Should see: 64 passed, 0 failed
```

### From Command Line
```powershell
cd C:\Users\Admin\Desktop\ai\games\ninja-fruit

# Run all tests
"C:\Program Files\Unity\Hub\Editor\6.0.0f1\Editor\Unity.exe" `
  -projectPath . `
  -runTests `
  -testPlatform playmode
```

---

## 🎯 Key Patterns to Follow

### 1. Event-Driven UI
```csharp
// Wrong ❌
void Update() {
    scoreText.text = scoreManager.CurrentScore.ToString();
}

// Right ✅
void OnEnable() {
    scoreManager.OnScoreChanged += UpdateScore;
}
```

### 2. Dependency Injection
```csharp
// Right ✅
var manager = new MyManager();
var ui = new MyUIComponent();
ui.SetManager(manager); // Inject dependency
```

### 3. State Sync on Enable
```csharp
// Right ✅
void OnEnable() {
    manager.OnChange += Update;
    Update(manager.CurrentValue); // Sync state
}
```

### 4. Testing UI
```csharp
// Right ✅
[UnityTest]
public IEnumerator TestUIUpdate() {
    var manager = new MyManager();
    var ui = new MyUIComponent();
    ui.SetManager(manager);
    
    manager.DoSomething();
    
    Assert.AreEqual(expected, ui.GetDisplayValue());
    yield return null;
}
```

---

## 📦 Assembly References

### NinjaFruit.Runtime.asmdef
```json
{
  "references": [
    "Unity.TextMeshPro",
    "Unity.InputSystem"
  ]
}
```

### NinjaFruit.Tests.Setup.asmdef
```json
{
  "references": [
    "NinjaFruit.Runtime",
    "Unity.TextMeshPro",
    "Unity.InputSystem",
    "Unity.InputSystem.UI"
  ],
  "optionalUnityReferences": []
}
```

### NinjaFruit.PlayMode.Tests.asmdef
```json
{
  "references": [
    "NinjaFruit.Runtime",
    "NinjaFruit.Tests.Setup",
    "Unity.TextMeshPro"
  ]
}
```

---

## 🛠️ Testing Utilities

### UITestHelpers Methods
```csharp
// Create a test canvas
Canvas canvas = UITestHelpers.CreateTestCanvas();

// Create a text element
TextMeshProUGUI text = UITestHelpers.CreateTextElement(
    name: "Score", 
    parent: canvas,
    text: "0"
);

// Create an image element
Image image = UITestHelpers.CreateImageElement(
    name: "Heart",
    parent: canvas,
    sprite: Resources.Load<Sprite>("heart")
);
```

---

## 📊 Project Status

### Completed
- ✅ EPIC-001: Core Slicing (3/3 stories)
- ✅ EPIC-002: Scoring System (3/3 stories)
- ✅ EPIC-004.1: HUD Display (1/5 stories)
- ✅ 64 tests total

### Backlog
- 📋 Story 011: Main Menu (3 pts) ← Next
- 📋 Story 012: Game Over Screen (2 pts)
- 📋 Story 013: Pause Menu (2 pts)
- 📋 Story 014: Visual Effects (3 pts)
- 📋 Story 007-009: Input handling (8 pts)

### Timeline
- **Completed:** 28 points (67% of UI epic)
- **Remaining:** 18 points
- **Velocity:** ~3 points/hour with AI
- **Estimated:** 6 hours to complete remaining UI work

---

## 🚀 Starting Story 011

### What Story 011 Will Have
- Main menu with Play/Settings/Quit buttons
- Scene transitions to game
- Settings persistence
- Similar architecture to Story 010

### Preparation Steps
```
1. Read Story 011 requirements
2. Write 10-12 test cases (TDD!)
3. Create test file: MainMenuTests.cs
4. Create UI component: MainMenuController.cs
5. Run tests (all fail - expected!)
6. Implement MainMenuController
7. All tests pass
8. Review and document
```

### Expected Timeline
```
Requirements: 30 min
Tests: 45 min
Implementation: 90 min
Debugging: 30 min
Documentation: 30 min
Total: ~3.5 hours
```

---

## ⚠️ Common Mistakes to Avoid

### ❌ Mistake 1: Using Update() for UI
```csharp
// Wrong
void Update() {
    text.text = manager.Value.ToString();
}
```
**Why:** Wastes CPU, ties to frame rate, hard to test

### ❌ Mistake 2: Forgetting to Unsubscribe
```csharp
// Wrong
void OnEnable() {
    manager.OnChange += Update;
    // Missing: OnDisable unsubscribe
}
```
**Why:** Memory leaks, multiple subscriptions

### ❌ Mistake 3: No State Sync on Enable
```csharp
// Wrong
void OnEnable() {
    manager.OnChange += Update;
    // Missing: Update(manager.CurrentValue);
}
```
**Why:** UI shows stale data when re-enabled

### ❌ Mistake 4: Wrong UI Module
```csharp
// Wrong
eventSystem.AddComponent<StandaloneInputModule>();

// Right
eventSystem.AddComponent<InputSystemUIInputModule>();
```
**Why:** Project uses New Input System, old module doesn't work

### ❌ Mistake 5: Wrong Assembly References
```json
// Wrong - Missing TextMeshPro
{
  "references": []
}

// Right
{
  "references": ["Unity.TextMeshPro"]
}
```
**Why:** Compilation errors without proper references

---

## 🔍 Debugging Checklist

### If Tests Fail
```
1. ✅ Check assembly references (UI packages added?)
2. ✅ Check test setup order (dependencies first?)
3. ✅ Check event subscriptions (subscribed in OnEnable?)
4. ✅ Check state sync (called in OnEnable?)
5. ✅ Check InputSystem (using right UI module?)
```

### If UI Doesn't Update
```
1. ✅ Is component enabled?
2. ✅ Is event subscribed?
3. ✅ Is state synced in OnEnable?
4. ✅ Is manager firing events?
5. ✅ Is canvas active?
```

### If Tests Won't Run
```
1. ✅ Are tests marked [UnityTest]?
2. ✅ Do they return IEnumerator?
3. ✅ Is assembly reference correct?
4. ✅ Is file in right folder (Tests/PlayMode)?
5. ✅ Did you rebuild (Ctrl+R)?
```

---

## 📚 Related Documentation

### Quick References
- `STORY_010_QUICK_REFERENCE.md` - Pattern reference
- `STORY_010_SUMMARY.md` - What was built
- `STORY_010_RETROSPECTIVE.md` - What was learned
- `STORY_010_AUDIT_AND_COMPLETION.md` - Code review

### Project Docs
- `docs/game-architecture.md` - System architecture
- `docs/GDD.md` - Game design document
- `docs/sprint-status.yaml` - Current sprint

### Stories
- `docs/stories/story-010-hud-display.md` - Story 010
- `docs/stories/story-011-main-menu.md` - Story 011 (next)

---

## ✅ Pre-Development Checklist

Before starting Story 011:

- [ ] All Story 010 tests passing (64/64)
- [ ] Reviewed Story 011 requirements
- [ ] Read STORY_010_RETROSPECTIVE.md
- [ ] Understand event-driven pattern
- [ ] Understand TDD workflow
- [ ] Have UITestHelpers reference available
- [ ] Assembly references reviewed

---

## 🎯 Success Criteria

Your Story 011 implementation is successful when:

✅ All acceptance criteria have tests  
✅ All tests are written BEFORE implementation  
✅ All tests pass (100%)  
✅ Code uses event-driven pattern  
✅ Code uses dependency injection  
✅ Code is clean and documented  
✅ Passes code review  

---

## 🆘 Need Help?

### If You're Stuck

1. **Check the pattern** → Look at HUDController.cs
2. **Check the tests** → Look at HUDControllerTests.cs
3. **Check the utilities** → Look at UITestHelpers.cs
4. **Read the retrospective** → STORY_010_RETROSPECTIVE.md
5. **Review the architecture** → docs/game-architecture.md

### Key Files to Reference
```
✅ HUDController.cs - Main UI example
✅ HUDControllerTests.cs - Test pattern
✅ UITestHelpers.cs - Test utilities
✅ GameStateController.cs - State management
✅ ScoreManager.cs - Manager pattern
```

---

## 🚀 Ready to Go!

You have everything you need:

✅ Working code to reference  
✅ Test patterns to follow  
✅ Test utilities library  
✅ Documentation  
✅ 14 passing tests as examples  
✅ ~3 hours of AI-assisted development time

**Start with Story 011 whenever you're ready!**

---

**Questions?** See STORY_010_RETROSPECTIVE.md for detailed explanations.  
**Need patterns?** See STORY_010_QUICK_REFERENCE.md for code templates.  
**Want details?** See STORY_010_AUDIT_AND_COMPLETION.md for code review.
