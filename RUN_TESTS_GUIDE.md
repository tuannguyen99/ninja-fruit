# 🧪 Run Story 010 Tests - Quick Guide

**Story:** STORY-010 HUD Display System  
**Tests:** 14 PlayMode Tests  
**Expected Result:** All Pass ✅

---

## 🚀 Quick Start (3 Steps)

### Step 1: Open Unity Project
```powershell
# If Unity is not open:
cd C:\Users\Admin\Desktop\ai\games\ninja-fruit

# Open project (Unity will auto-import new files)
# Wait for compilation to complete (~30 seconds)
```

### Step 2: Open Test Runner
```
Unity Menu Bar → Window → General → Test Runner
```

### Step 3: Run Tests
1. Click **"PlayMode"** tab
2. Expand **"NinjaFruit.Tests.PlayMode.UI"**
3. See **"HUDControllerTests"** with 14 tests
4. Click **"Run All"** button
5. Wait for tests to complete (~10-15 seconds)

**Expected:** 14/14 tests PASS ✅

---

## 📊 What You Should See

### Test Runner Window Should Show:

```
PlayMode Tests
└── NinjaFruit.Tests.PlayMode.UI
    └── HUDControllerTests
        ✅ TC001_InitialScoreDisplay_ShowsZero
        ✅ TC002_ScoreUpdates_WhenPointsEarned
        ✅ TC003_ScoreDisplays_LargeNumbers
        ✅ TC004_ScoreHandles_NegativeValues
        ✅ TC005_InitialLivesDisplay_ShowsThreeHearts
        ✅ TC006_LivesDecrease_OnMissedFruit
        ✅ TC007_AllLivesLost_ShowsEmptyHearts
        ✅ TC008_ComboHidden_Initially
        ✅ TC009_ComboDisplays_2xMultiplier
        ✅ TC010_ComboDisplays_Maximum5x
        ✅ TC011_ComboResets_OnBombHit
        ✅ TC012_AllUIElements_Initialized
        ✅ TC013_HUD_SubscribesToScoreEvents
        ✅ TC014_HUD_UnsubscribesOnDisable

Results: 14 passed, 0 failed, 0 skipped
Time: ~10-15 seconds
```

---

## 🐛 If You See Compilation Errors

### Error: "TextMeshPro namespace not found"
**Fix:**
1. Window → TextMeshPro → Import TMP Essential Resources
2. Wait for import to complete
3. Recompile (should auto-happen)

### Error: "Assembly reference 'NinjaFruit.Runtime' not found"
**Fix:**
1. Check `Assets/Scripts/NinjaFruit.Runtime.asmdef` exists
2. Check `Assets/Tests/NinjaFruit.Tests.asmdef` exists
3. Reimport both asmdef files

### Error: "Type or namespace 'Gameplay' does not exist"
**Fix:**
- Verify all new scripts imported correctly
- Check Console for specific file import errors
- Try: Assets → Reimport All

---

## 🎯 If Tests Fail

### Scenario 1: "NullReferenceException"
**Cause:** Test setup didn't create objects correctly  
**Fix:** Check console for which test failed, verify Setup() method

### Scenario 2: "Assertion Failed"
**Cause:** Actual behavior doesn't match expected  
**Fix:** Check test output, debug specific test case

### Scenario 3: Tests timeout
**Cause:** Missing `yield return null` after events  
**Fix:** Already included in tests, shouldn't happen

---

## 🎉 When All Tests Pass

### Next Steps:

**1. Commit to Git:**
```powershell
cd C:\Users\Admin\Desktop\ai\games
git add .
git commit -m "feat(ui): Story 010 - HUD Display System complete (14 tests passing)"
git push origin master
```

**2. Mark Story Complete:**
- Update `docs/sprint-status.yaml`:
  ```yaml
  story-010-hud-display: done
  ```

**3. Celebrate!** 🎊
You now have:
- ✅ Functional HUD system
- ✅ 14 automated tests
- ✅ Regression protection
- ✅ TDD experience!

**4. Choose Next Action:**
- Continue to Story 011 (Main Menu)
- Create visual HUD in Unity scene
- Take a break - you earned it!

---

## 📸 Screenshot Opportunity

After tests pass, take screenshots for:
- Portfolio/resume (TDD proof)
- Documentation
- Customer demo

**Good Screenshot Locations:**
1. Test Runner showing 14/14 pass
2. Console showing no errors
3. Code editor with test file

---

## 🔧 Alternative: Run Tests from Command Line

If you prefer command line:

```powershell
# Navigate to project
cd C:\Users\Admin\Desktop\ai\games\ninja-fruit

# Run tests (requires Unity Test Framework CLI)
# Unity.exe -runTests -batchmode -projectPath "." -testPlatform PlayMode -testResults test-results.xml

# Note: This requires Unity CLI setup
```

---

## 💡 Pro Tips

**Tip 1: Run Individual Tests**
- Right-click on specific test
- Select "Run"
- Faster iteration for debugging

**Tip 2: Keep Test Runner Open**
- Dock Test Runner as a tab
- Rerun tests after code changes
- Instant feedback loop

**Tip 3: Filter Tests**
- Use search box in Test Runner
- Filter by name: "Score", "Combo", etc.
- Faster test execution during development

---

## 📞 Need Help?

**If stuck, ask:**
- "Why is test TC002 failing?"
- "How do I fix TextMeshPro import error?"
- "Show me how to debug Unity tests"

**Or check:**
- Unity Console (for compilation errors)
- Test Runner output (for assertion details)
- `STORY_010_IMPLEMENTATION.md` (comprehensive guide)

---

**Current Status:** Code ready, waiting for Unity test run  
**Expected Time:** 1-2 minutes to run all tests  
**Success Criteria:** 14/14 tests passing ✅

**Let's see those green checkmarks! 🚦➡️✅**
