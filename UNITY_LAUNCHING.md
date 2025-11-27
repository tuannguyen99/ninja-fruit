# Unity Project Setup - LIVE WALKTHROUGH

**Unity is launching now...**

Expected startup time: 30-60 seconds depending on system performance

---

## What to Expect

### Upon First Load
- Unity will show "Importing..." at bottom-right
- Assets folder will be scanned
- Test assemblies will compile
- This takes 30-45 seconds

### After Import Complete
You'll see:
- Project folder (Assets, Packages, ProjectSettings visible in left panel)
- Empty scene view (black viewport)
- No errors in Console tab

---

## IMMEDIATE ACTION (Once Project Loads)

### Step 1: Create Test Prefab
```
Menu: Window → Ninja Fruit → Setup Test Prefab
```

This ONE CLICK will:
- ✅ Create Assets/Resources/Prefabs/TestFruit.prefab
- ✅ Create "Fruit" tag
- ✅ Show dialog: "Test Setup Complete"

**Time:** ~2 seconds

---

### Step 2: Open Test Runner
```
Menu: Window → General → Test Runner
```

This opens the Test Runner panel showing:
- **EditMode** tab: 10 unit tests
- **PlayMode** tab: 4 integration tests
- All 14 tests should appear with names

**Expected List:**
```
✓ TEST-001: CalculateSpawnInterval_ScoreZero_Returns2Seconds
✓ TEST-002: CalculateSpawnInterval_Score500_Returns1Second
✓ TEST-003: CalculateSpawnInterval_Score1000_ReturnsMinimum0Point3Seconds
✓ TEST-004: CalculateSpawnInterval_NegativeScore_ClampsTo2Seconds
✓ TEST-005: CalculateFruitSpeed_ScoreZero_Returns2MetersPerSecond
✓ TEST-006: CalculateFruitSpeed_Score1000_Returns3MetersPerSecond
✓ TEST-007: CalculateFruitSpeed_Score5000_ReturnsMaximum7MetersPerSecond
✓ TEST-008: CalculateFruitSpeed_Score10000_DoesNotExceed7MetersPerSecond
✓ TEST-009: ShouldSpawnBomb_FruitCount9_ReturnsFalse
✓ TEST-010: ShouldSpawnBomb_FruitCount10_ReturnsTrue
```

All showing: "NotImplemented" status (EXPECTED - this is good!)

**Time:** ~1 second

---

## THEN FOLLOW THIS SEQUENCE

### Phase 1: Setup (5 minutes)
1. ✅ Unity loads
2. ✅ Menu: Setup Test Prefab
3. ✅ Open Test Runner
4. ✅ Verify 10 EditMode tests listed
5. ✅ Verify 4 PlayMode tests listed (switch to PlayMode tab)

### Phase 2: Implementation (25 minutes)
Follow `QUICK_START.md` exactly:

**Step 1:** Implement `CalculateSpawnInterval()` → Run tests → Expect 4/10 pass
**Step 2:** Implement `CalculateFruitSpeed()` → Run tests → Expect 8/10 pass  
**Step 3:** Implement `ShouldSpawnBomb()` → Run tests → Expect 10/10 pass
**Step 4:** Implement `SpawnFruit()` + Update test setup → PlayMode tests → Expect 4/4 pass

### Phase 3: Verification (2 minutes)
- Run EditMode: 10/10 passing
- Run PlayMode: 4/4 passing
- Total: 14/14 ✅

---

## 📍 File Location Reference (In VS Code)

While Unity is loading, you'll edit this file in VS Code:

**File to Edit:**
```
Assets/Scripts/Gameplay/FruitSpawner.cs
```

This file has 4 stub methods that throw NotImplementedException:
1. `CalculateSpawnInterval(int score)` → Line ~20
2. `CalculateFruitSpeed(int score)` → Line ~26
3. `ShouldSpawnBomb(int fruitCount)` → Line ~32
4. `SpawnFruit()` → Line ~39

Replace each stub with the implementation from QUICK_START.md

**Also Edit for PlayMode:**
```
Assets/Tests/PlayMode/Gameplay/FruitSpawningIntegrationTests.cs
```

In the `Setup()` method, add prefab assignment after line `spawner = spawnerObject.AddComponent<FruitSpawner>();`

---

## ✅ Checklist for Success

Once Unity Opens:
- [ ] Project imports without errors
- [ ] Menu: Window → Ninja Fruit appears (if not, reimport: Assets → Reimport All)
- [ ] Menu: Setup Test Prefab works
- [ ] Test Runner shows 10 EditMode tests
- [ ] Test Runner shows 4 PlayMode tests
- [ ] All tests show "NotImplemented" initially
- [ ] Console has no red errors

If you see anything different, let me know immediately!

---

## 🚨 Common Issues & Quick Fixes

**Q: "Window → Ninja Fruit" menu doesn't appear**
- A: Assets → Reimport All (takes 10 seconds)
- Then try menu again

**Q: Test Runner shows 0 tests**
- A: Check Assets/Tests/EditMode and Assets/Tests/PlayMode folders exist
- Make sure FruitSpawnerTests.cs and FruitSpawningIntegrationTests.cs files are in those folders
- If files exist, try: Window → General → Test Runner (close and reopen)

**Q: Red error in Console about "NinjaFruit not found"**
- A: Assembly definitions might not be loading correctly
- Try: Assets → Reimport All
- Then: Window → General → Test Runner (refresh)

**Q: Tests show but all say "Unknown"**
- A: This is normal! Tests haven't been implemented yet
- They become "NotImplemented" once code loads
- After you implement methods, they'll either PASS ✅ or FAIL ❌

---

## Once You See Tests Listed

**You're ready! Do this:**

1. Open `QUICK_START.md` (already in VS Code)
2. Follow Step 1 in PHASE 3
3. Edit `Assets/Scripts/Gameplay/FruitSpawner.cs`
4. Replace the first method stub
5. Save file (Ctrl+S)
6. Return to Unity
7. Test Runner auto-runs tests
8. See 4/10 pass ✅
9. Repeat for steps 2, 3, 4

**Total time from here: ~25-30 minutes**

---

**Status: Unity is launching!**  
**Next: Monitor Console for import completion, then follow Setup steps**

Let me know once:
1. ✅ Unity fully loads (Console clear, no import errors)
2. ✅ Menu "Window → Ninja Fruit" appears
3. ✅ You click "Setup Test Prefab" and see success dialog
