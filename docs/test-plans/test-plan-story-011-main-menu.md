# Test Plan: STORY-011 Main Menu & Navigation

**Story:** STORY-011 - Main Menu & Navigation  
**Epic:** EPIC-004 - User Interface & Game Flow  
**Test Plan Version:** 1.0  
**Author:** Test Design Agent  
**Date:** November 30, 2025  
**Test Approach:** Test-Driven Development (TDD)

---

## Test Scope

### In Scope
- Main menu UI display and button visibility
- Button click functionality (Play, High Scores, Settings, Quit)
- Panel navigation (show/hide panels)
- Scene transitions to gameplay
- High score data persistence (load/save)
- Settings data persistence (load/save)
- Platform-specific UI (Quit button on PC only)

### Out of Scope
- Visual appearance (colors, fonts, animations)
- Audio feedback for button clicks (Story 015+)
- Scene transition effects/animations (future story)
- High score leaderboards (online features)
- Settings validation beyond range checks
- Localization/language support

---

## Test Environment

### Prerequisites
- Unity project with Test Framework installed
- TextMeshPro package installed
- Unity UI package installed
- MainMenu scene created
- Gameplay scene created (from Story 010)

### Test Data
- **High Scores:** 0 (default), 100, 1250, 9999
- **Total Fruits:** 0 (default), 50, 500
- **Longest Combo:** 0 (default), 3, 5
- **Master Volume:** 0.0, 0.5, 0.8 (default), 1.0
- **Toggles:** true (default for FX/Music), false

### Test Tools
- Unity Test Runner (Edit Mode + Play Mode)
- PlayerPrefs (mocked in Edit Mode tests)
- Test utilities: `UITestHelpers` (from Story 010)

---

## Test Cases

### Category 1: Data Persistence (Edit Mode)

#### TC1.1 - High Score Loads Default on First Launch
**Test ID:** TC-011-001  
**Priority:** High  
**Type:** Edit Mode Unit Test

**Test Steps:**
1. Clear PlayerPrefs (simulate first launch)
2. Create HighScoreManager
3. Call LoadScores()
4. Verify HighScore = 0

**Expected Result:** HighScore defaults to 0

**Test Code:**
```csharp
[Test]
public void TC001_HighScore_LoadsDefaultOnFirstLaunch()
{
    PlayerPrefs.DeleteAll();
    var manager = new HighScoreManager();
    manager.LoadScores();
    Assert.AreEqual(0, manager.HighScore);
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC1.2 - High Score Saves and Loads Correctly
**Test ID:** TC-011-002  
**Priority:** Critical  
**Type:** Edit Mode Unit Test

**Test Steps:**
1. Create HighScoreManager
2. SaveHighScore(1250)
3. Create new HighScoreManager instance
4. LoadScores()
5. Verify HighScore = 1250

**Expected Result:** High score persists across instances

**Test Code:**
```csharp
[Test]
public void TC002_HighScore_SavesAndLoadsCorrectly()
{
    var manager1 = new HighScoreManager();
    manager1.SaveHighScore(1250);
    
    var manager2 = new HighScoreManager();
    manager2.LoadScores();
    
    Assert.AreEqual(1250, manager2.HighScore);
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC1.3 - High Score Only Updates if Higher
**Test ID:** TC-011-003  
**Priority:** High  
**Type:** Edit Mode Unit Test

**Test Steps:**
1. Create HighScoreManager
2. SaveHighScore(1000)
3. SaveHighScore(500) (lower score)
4. Verify HighScore = 1000 (unchanged)

**Expected Result:** Lower scores don't overwrite high score

**Test Code:**
```csharp
[Test]
public void TC003_HighScore_OnlyUpdatesIfHigher()
{
    var manager = new HighScoreManager();
    manager.SaveHighScore(1000);
    manager.SaveHighScore(500);
    
    Assert.AreEqual(1000, manager.HighScore);
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC1.4 - Total Fruits Count Accumulates
**Test ID:** TC-011-004  
**Priority:** Medium  
**Type:** Edit Mode Unit Test

**Test Steps:**
1. Create HighScoreManager
2. SaveFruitCount(50)
3. SaveFruitCount(75) (adds 25 more)
4. Verify TotalFruitsSliced = 125

**Expected Result:** Fruit count accumulates, not replaces

**Test Code:**
```csharp
[Test]
public void TC004_TotalFruits_Accumulates()
{
    var manager = new HighScoreManager();
    manager.SaveFruitCount(50);
    manager.SaveFruitCount(75);
    
    Assert.AreEqual(125, manager.TotalFruitsSliced);
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC1.5 - Settings Load Default Values
**Test ID:** TC-011-005  
**Priority:** High  
**Type:** Edit Mode Unit Test

**Test Steps:**
1. Clear PlayerPrefs
2. Create SettingsManager
3. Call LoadSettings()
4. Verify MasterVolume = 0.8f, SoundFX = true, Music = true

**Expected Result:** Settings use sensible defaults

**Test Code:**
```csharp
[Test]
public void TC005_Settings_LoadDefaultValues()
{
    PlayerPrefs.DeleteAll();
    var manager = new SettingsManager();
    manager.LoadSettings();
    
    Assert.AreEqual(0.8f, manager.MasterVolume, 0.01f);
    Assert.IsTrue(manager.SoundEffectsEnabled);
    Assert.IsTrue(manager.MusicEnabled);
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC1.6 - Master Volume Saves and Loads
**Test ID:** TC-011-006  
**Priority:** High  
**Type:** Edit Mode Unit Test

**Test Steps:**
1. Create SettingsManager
2. SetMasterVolume(0.5f)
3. SaveSettings()
4. Create new SettingsManager
5. LoadSettings()
6. Verify MasterVolume = 0.5f

**Expected Result:** Volume setting persists

**Test Code:**
```csharp
[Test]
public void TC006_MasterVolume_SavesAndLoads()
{
    var manager1 = new SettingsManager();
    manager1.SetMasterVolume(0.5f);
    manager1.SaveSettings();
    
    var manager2 = new SettingsManager();
    manager2.LoadSettings();
    
    Assert.AreEqual(0.5f, manager2.MasterVolume, 0.01f);
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC1.7 - Sound Effects Toggle Persists
**Test ID:** TC-011-007  
**Priority:** Medium  
**Type:** Edit Mode Unit Test

**Test Steps:**
1. Create SettingsManager
2. SetSoundEffects(false)
3. SaveSettings()
4. Create new SettingsManager
5. LoadSettings()
6. Verify SoundEffectsEnabled = false

**Expected Result:** Toggle state persists

**Test Code:**
```csharp
[Test]
public void TC007_SoundEffectsToggle_Persists()
{
    var manager1 = new SettingsManager();
    manager1.SetSoundEffects(false);
    manager1.SaveSettings();
    
    var manager2 = new SettingsManager();
    manager2.LoadSettings();
    
    Assert.IsFalse(manager2.SoundEffectsEnabled);
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC1.8 - Music Toggle Persists
**Test ID:** TC-011-008  
**Priority:** Medium  
**Type:** Edit Mode Unit Test

**Test Steps:**
1. Create SettingsManager
2. SetMusic(false)
3. SaveSettings()
4. Create new SettingsManager
5. LoadSettings()
6. Verify MusicEnabled = false

**Expected Result:** Toggle state persists

**Test Code:**
```csharp
[Test]
public void TC008_MusicToggle_Persists()
{
    var manager1 = new SettingsManager();
    manager1.SetMusic(false);
    manager1.SaveSettings();
    
    var manager2 = new SettingsManager();
    manager2.LoadSettings();
    
    Assert.IsFalse(manager2.MusicEnabled);
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

### Category 2: UI Display & Navigation (Play Mode)

#### TC2.1 - Main Menu Displays All Buttons
**Test ID:** TC-011-009  
**Priority:** Critical  
**Type:** Play Mode Integration Test

**Test Steps:**
1. Create MainMenuController in test scene
2. Call Initialize()
3. Verify Play, HighScores, Settings buttons visible
4. Verify Quit button visible if UNITY_STANDALONE

**Expected Result:** All buttons displayed correctly

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC009_MainMenu_DisplaysAllButtons()
{
    var menu = CreateTestMainMenu();
    menu.Initialize();
    yield return null;
    
    Assert.IsTrue(menu.IsPlayButtonVisible());
    Assert.IsTrue(menu.IsHighScoresButtonVisible());
    Assert.IsTrue(menu.IsSettingsButtonVisible());
    
    #if UNITY_STANDALONE
    Assert.IsTrue(menu.IsQuitButtonVisible());
    #else
    Assert.IsFalse(menu.IsQuitButtonVisible());
    #endif
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC2.2 - Play Button Triggers Scene Load
**Test ID:** TC-011-010  
**Priority:** Critical  
**Type:** Play Mode Integration Test

**Test Steps:**
1. Create MainMenuController
2. Mock SceneTransitionManager
3. Click Play button
4. Verify LoadGameplayScene() was called

**Expected Result:** Gameplay scene load triggered

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC010_PlayButton_TriggersSceneLoad()
{
    var menu = CreateTestMainMenu();
    var sceneManager = CreateMockSceneManager();
    menu.SetSceneManager(sceneManager);
    
    menu.OnPlayClicked();
    yield return null;
    
    Assert.IsTrue(sceneManager.WasGameplaySceneLoaded);
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC2.3 - High Scores Button Shows Panel
**Test ID:** TC-011-011  
**Priority:** High  
**Type:** Play Mode Integration Test

**Test Steps:**
1. Create MainMenuController
2. Click High Scores button
3. Verify high scores panel visible
4. Verify main menu panel hidden

**Expected Result:** High scores panel replaces main menu

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC011_HighScoresButton_ShowsPanel()
{
    var menu = CreateTestMainMenu();
    menu.Initialize();
    
    menu.OnHighScoresClicked();
    yield return null;
    
    Assert.IsTrue(menu.IsHighScoresPanelVisible());
    Assert.IsFalse(menu.IsMainMenuPanelVisible());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC2.4 - Settings Button Shows Panel
**Test ID:** TC-011-012  
**Priority:** High  
**Type:** Play Mode Integration Test

**Test Steps:**
1. Create MainMenuController
2. Click Settings button
3. Verify settings panel visible
4. Verify main menu panel hidden

**Expected Result:** Settings panel replaces main menu

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC012_SettingsButton_ShowsPanel()
{
    var menu = CreateTestMainMenu();
    menu.Initialize();
    
    menu.OnSettingsClicked();
    yield return null;
    
    Assert.IsTrue(menu.IsSettingsPanelVisible());
    Assert.IsFalse(menu.IsMainMenuPanelVisible());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC2.5 - Quit Button Closes Application
**Test ID:** TC-011-013  
**Priority:** Medium  
**Type:** Play Mode Integration Test (Manual Verification)

**Test Steps:**
1. Create MainMenuController
2. Mock SceneTransitionManager
3. Click Quit button
4. Verify QuitApplication() was called

**Expected Result:** Quit method invoked

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC013_QuitButton_ClosesApplication()
{
    var menu = CreateTestMainMenu();
    var sceneManager = CreateMockSceneManager();
    menu.SetSceneManager(sceneManager);
    
    menu.OnQuitClicked();
    yield return null;
    
    Assert.IsTrue(sceneManager.WasQuitCalled);
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC2.6 - Back Button Returns from High Scores
**Test ID:** TC-011-014  
**Priority:** High  
**Type:** Play Mode Integration Test

**Test Steps:**
1. Create MainMenuController
2. Navigate to high scores panel
3. Click Back button
4. Verify main menu panel visible
5. Verify high scores panel hidden

**Expected Result:** Navigation returns to main menu

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC014_BackButton_ReturnsFromHighScores()
{
    var menu = CreateTestMainMenu();
    menu.Initialize();
    menu.OnHighScoresClicked();
    yield return null;
    
    menu.OnBackClicked();
    yield return null;
    
    Assert.IsTrue(menu.IsMainMenuPanelVisible());
    Assert.IsFalse(menu.IsHighScoresPanelVisible());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC2.7 - Back Button Returns from Settings
**Test ID:** TC-011-015  
**Priority:** High  
**Type:** Play Mode Integration Test

**Test Steps:**
1. Create MainMenuController
2. Navigate to settings panel
3. Click Back button
4. Verify main menu panel visible
5. Verify settings panel hidden

**Expected Result:** Navigation returns to main menu

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC015_BackButton_ReturnsFromSettings()
{
    var menu = CreateTestMainMenu();
    menu.Initialize();
    menu.OnSettingsClicked();
    yield return null;
    
    menu.OnBackClicked();
    yield return null;
    
    Assert.IsTrue(menu.IsMainMenuPanelVisible());
    Assert.IsFalse(menu.IsSettingsPanelVisible());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC2.8 - High Scores Panel Displays Data
**Test ID:** TC-011-016  
**Priority:** High  
**Type:** Play Mode Integration Test

**Test Steps:**
1. Set high score to 1250
2. Set total fruits to 50
3. Set longest combo to 5
4. Create MainMenuController
5. Navigate to high scores panel
6. Verify displayed values match

**Expected Result:** High scores show correct data

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC016_HighScoresPanel_DisplaysData()
{
    var highScoreManager = CreateTestHighScoreManager();
    highScoreManager.SaveHighScore(1250);
    highScoreManager.SaveFruitCount(50);
    highScoreManager.SaveCombo(5);
    
    var menu = CreateTestMainMenu();
    menu.SetHighScoreManager(highScoreManager);
    menu.OnHighScoresClicked();
    yield return null;
    
    Assert.AreEqual("1250", menu.GetHighScoreText());
    Assert.AreEqual("50", menu.GetTotalFruitsText());
    Assert.AreEqual("5", menu.GetLongestComboText());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC2.9 - Settings Panel Reflects Current Settings
**Test ID:** TC-011-017  
**Priority:** High  
**Type:** Play Mode Integration Test

**Test Steps:**
1. Set master volume to 0.5
2. Set sound effects to false
3. Set music to true
4. Create MainMenuController
5. Navigate to settings panel
6. Verify UI elements match settings

**Expected Result:** Settings UI shows current state

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC017_SettingsPanel_ReflectsCurrentSettings()
{
    var settingsManager = CreateTestSettingsManager();
    settingsManager.SetMasterVolume(0.5f);
    settingsManager.SetSoundEffects(false);
    settingsManager.SetMusic(true);
    
    var menu = CreateTestMainMenu();
    menu.SetSettingsManager(settingsManager);
    menu.OnSettingsClicked();
    yield return null;
    
    Assert.AreEqual(0.5f, menu.GetVolumeSliderValue(), 0.01f);
    Assert.IsFalse(menu.GetSoundEffectsToggleValue());
    Assert.IsTrue(menu.GetMusicToggleValue());
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

#### TC2.10 - Settings Changes Trigger Events
**Test ID:** TC-011-018  
**Priority:** Medium  
**Type:** Play Mode Integration Test

**Test Steps:**
1. Create SettingsManager
2. Subscribe to OnMasterVolumeChanged event
3. Change volume slider
4. Verify event fired with correct value

**Expected Result:** Settings changes broadcast events

**Test Code:**
```csharp
[UnityTest]
public IEnumerator TC018_SettingsChanges_TriggerEvents()
{
    var settingsManager = CreateTestSettingsManager();
    float receivedVolume = -1f;
    settingsManager.OnMasterVolumeChanged += (vol) => receivedVolume = vol;
    
    settingsManager.SetMasterVolume(0.7f);
    yield return null;
    
    Assert.AreEqual(0.7f, receivedVolume, 0.01f);
}
```

**Status:** ⬜ Not Run | ✅ Pass | ❌ Fail

---

## Test Execution Plan

### Phase 1: RED Phase (Write Failing Tests)
**Duration:** 1 hour  
**Tasks:**
- Create `HighScoreManagerTests.cs` (4 Edit Mode tests)
- Create `SettingsManagerTests.cs` (4 Edit Mode tests)
- Create `MainMenuControllerTests.cs` (10 Play Mode tests)
- Create test utilities: `CreateTestMainMenu()`, `CreateMockSceneManager()`
- Run tests → **Expect all 18 to FAIL** ❌

**Success Criteria:** All tests fail with expected errors

---

### Phase 2: GREEN Phase (Implement Code)
**Duration:** 2 hours  
**Tasks:**
- Create `HighScoreManager.cs` with PlayerPrefs logic
- Create `SettingsManager.cs` with PlayerPrefs logic
- Create `SceneTransitionManager.cs` with scene loading
- Create `MainMenuController.cs` with UI logic
- Create MainMenu scene with UI hierarchy
- Wire SerializeField references
- Run tests → **Expect all 18 to PASS** ✅

**Success Criteria:** All 18 tests passing

---

### Phase 3: REFACTOR Phase (Clean Up)
**Duration:** 45 minutes  
**Tasks:**
- Extract PlayerPrefs keys to constants
- Add XML documentation comments
- Add null checks and error handling
- Optimize button event subscriptions
- Run tests → **Still 18 passes** ✅

**Success Criteria:** Tests still pass after refactoring

---

### Phase 4: EDGE CASES (Additional Tests)
**Duration:** 30 minutes  
**Tasks:**
- Test corrupted PlayerPrefs data
- Test volume clamping (0.0-1.0)
- Test rapid button clicking
- Test scene load failures
- Run tests → **All pass** ✅

**Success Criteria:** 100% coverage on managers

---

## Test Metrics

### Coverage Goals
- **Line Coverage:** 80%+ on all managers
- **Branch Coverage:** 70%+ (conditional logic)
- **Method Coverage:** 100% (all public methods tested)

### Success Criteria
- **Pass Rate:** 100% (18/18 tests passing)
- **Execution Time:** <3 seconds for all tests
- **Manual QA:** No navigation bugs in 5-minute play test

---

## Risk Assessment

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| PlayerPrefs not available in tests | Low | High | Use PlayerPrefs wrapper for mocking |
| Scene loading fails in tests | Medium | High | Use mock SceneTransitionManager |
| Platform-specific code | Medium | Medium | Use preprocessor directives, test both |
| Button clicks not registering | Low | Medium | Use `button.onClick.Invoke()` |

---

## Test Deliverables

- [ ] `HighScoreManagerTests.cs` - 4 Edit Mode tests
- [ ] `SettingsManagerTests.cs` - 4 Edit Mode tests
- [ ] `MainMenuControllerTests.cs` - 10 Play Mode tests
- [ ] Test utilities (mocks, helpers)
- [ ] Test execution report
- [ ] Code coverage report

---

## Approval

**Test Plan Created By:** Test Design Agent  
**Reviewed By:** _____________  
**Approved By:** _____________  
**Date:** _____________

---

**Status:** READY FOR EXECUTION  
**Next Step:** Begin Phase 1 (Write 18 Failing Tests)

*TDD Reminder: Write the test for the behavior you want, then make it pass!*
