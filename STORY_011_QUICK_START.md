# 🚀 Quick Start: Story 011 - Main Menu & Navigation

**Story:** STORY-011  
**Epic:** EPIC-004 - User Interface & Game Flow  
**Status:** Ready for Development  
**Approach:** Test-Driven Development (TDD)  
**Created:** November 30, 2025

---

## ⚡ 60-Second Summary

### What You're Building
A main menu system with:
- **Play** button → loads gameplay scene
- **High Scores** panel → shows persistent stats
- **Settings** panel → volume and audio toggles
- **Quit** button → exits game (PC only)
- **Data persistence** → PlayerPrefs for scores/settings

### How It Works
```
Main Menu → Button Click → Panel Navigation / Scene Load
     ↓
High Scores ← PlayerPrefs → Settings
     ↓
Data persists across sessions
```

### TDD Workflow
```
1. Write 18 tests (all fail ❌)
2. Implement code (all pass ✅)
3. Refactor (keep passing ✅)
4. Done!
```

---

## 📋 Pre-Development Checklist

Before you start, ensure:
- [ ] Story 010 (HUD Display) completed
- [ ] Unity Test Runner accessible
- [ ] TextMeshPro package installed
- [ ] Read test plan: `docs/test-plans/test-plan-story-011-main-menu.md`
- [ ] Read test spec: `docs/test-specs/test-spec-story-011-main-menu.md`
- [ ] Read scaffolding: `docs/test-scaffolding/test-scaffolding-story-011-main-menu.md`

---

## 🎯 Acceptance Criteria (Quick Reference)

| AC | Description | Test Count |
|----|-------------|------------|
| AC1 | Main menu displays all buttons correctly | 1 |
| AC2 | Play button loads gameplay scene | 1 |
| AC3 | High scores button shows panel with data | 3 |
| AC4 | Settings button shows panel with controls | 3 |
| AC5 | Quit button closes app (PC only) | 1 |
| AC6 | Back button returns to main menu | 2 |
| AC7 | Data persists across sessions | 7 |

**Total Tests:** 18 (8 Edit Mode + 10 Play Mode)

---

## 🛠️ TDD Workflow (4 Hours)

### Phase 1: RED (1 hour)
**Goal:** Write all tests, watch them fail

```powershell
# 1. Copy stub files from scaffolding doc
cd ninja-fruit/Assets

# 2. Create folder structure
New-Item -ItemType Directory -Path "Scripts/UI"
New-Item -ItemType Directory -Path "Scripts/Interfaces"
New-Item -ItemType Directory -Path "Tests/EditMode/UI"
New-Item -ItemType Directory -Path "Tests/PlayMode/UI"
New-Item -ItemType Directory -Path "Tests/Mocks"

# 3. Copy stub code from docs/test-scaffolding/test-scaffolding-story-011-main-menu.md

# 4. Refresh Unity (Ctrl+R)

# 5. Run tests
Window → Test Runner
EditMode Tab → Run All (expect 8 failures ❌)
PlayMode Tab → Run All (expect 10 failures ❌)
```

**Success Criteria:** All 18 tests fail with expected errors (not compile errors)

---

### Phase 2: GREEN (2 hours)
**Goal:** Implement code to pass all tests

#### 2.1 Implement HighScoreManager (30 min)
```csharp
// Assets/Scripts/UI/HighScoreManager.cs

public void LoadScores()
{
    HighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    TotalFruitsSliced = PlayerPrefs.GetInt(TOTAL_FRUITS_KEY, 0);
    LongestCombo = PlayerPrefs.GetInt(LONGEST_COMBO_KEY, 0);
}

public void SaveHighScore(int score)
{
    if (score > HighScore)
    {
        HighScore = score;
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, HighScore);
        PlayerPrefs.Save();
    }
}

public void SaveFruitCount(int count)
{
    TotalFruitsSliced += count;
    PlayerPrefs.SetInt(TOTAL_FRUITS_KEY, TotalFruitsSliced);
    PlayerPrefs.Save();
}

public void SaveCombo(int combo)
{
    if (combo > LongestCombo)
    {
        LongestCombo = combo;
        PlayerPrefs.SetInt(LONGEST_COMBO_KEY, LongestCombo);
        PlayerPrefs.Save();
    }
}
```

**Run Edit Mode Tests:** 4/8 passing ✅

---

#### 2.2 Implement SettingsManager (30 min)
```csharp
// Assets/Scripts/UI/SettingsManager.cs

public void LoadSettings()
{
    MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, DEFAULT_MASTER_VOLUME);
    SoundEffectsEnabled = PlayerPrefs.GetInt(SOUND_FX_KEY, DEFAULT_SOUND_FX ? 1 : 0) == 1;
    MusicEnabled = PlayerPrefs.GetInt(MUSIC_KEY, DEFAULT_MUSIC ? 1 : 0) == 1;
}

public void SaveSettings()
{
    PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, MasterVolume);
    PlayerPrefs.SetInt(SOUND_FX_KEY, SoundEffectsEnabled ? 1 : 0);
    PlayerPrefs.SetInt(MUSIC_KEY, MusicEnabled ? 1 : 0);
    PlayerPrefs.Save();
}

public void SetMasterVolume(float volume)
{
    MasterVolume = Mathf.Clamp01(volume);
    SaveSettings();
    OnMasterVolumeChanged?.Invoke(MasterVolume);
}

public void SetSoundEffects(bool enabled)
{
    SoundEffectsEnabled = enabled;
    SaveSettings();
    OnSoundEffectsToggled?.Invoke(SoundEffectsEnabled);
}

public void SetMusic(bool enabled)
{
    MusicEnabled = enabled;
    SaveSettings();
    OnMusicToggled?.Invoke(MusicEnabled);
}
```

**Run Edit Mode Tests:** 8/8 passing ✅

---

#### 2.3 Implement SceneTransitionManager (15 min)
```csharp
// Assets/Scripts/UI/SceneTransitionManager.cs

public void LoadGameplayScene()
{
    SceneManager.LoadScene(GAMEPLAY_SCENE, LoadSceneMode.Single);
}

public void LoadMainMenuScene()
{
    SceneManager.LoadScene(MAIN_MENU_SCENE, LoadSceneMode.Single);
}

public void QuitApplication()
{
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #else
    Application.Quit();
    #endif
}
```

---

#### 2.4 Implement MainMenuController (45 min)
```csharp
// Assets/Scripts/UI/MainMenuController.cs

public void Initialize()
{
    // Show main menu, hide others
    ShowMainMenu();
    
    // Wire button events
    playButton.onClick.AddListener(OnPlayClicked);
    highScoresButton.onClick.AddListener(OnHighScoresClicked);
    settingsButton.onClick.AddListener(OnSettingsClicked);
    quitButton.onClick.AddListener(OnQuitClicked);
    
    // Platform-specific visibility
    #if !UNITY_STANDALONE
    quitButton.gameObject.SetActive(false);
    #endif
    
    // Initialize managers if not injected
    if (sceneManager == null)
        sceneManager = FindObjectOfType<SceneTransitionManager>();
    if (highScoreManager == null)
        highScoreManager = FindObjectOfType<HighScoreManager>();
    if (settingsManager == null)
        settingsManager = FindObjectOfType<SettingsManager>();
}

public void ShowMainMenu()
{
    mainMenuPanel.SetActive(true);
    highScoresPanel.SetActive(false);
    settingsPanel.SetActive(false);
}

public void ShowHighScores()
{
    mainMenuPanel.SetActive(false);
    highScoresPanel.SetActive(true);
    settingsPanel.SetActive(false);
    
    // Update UI with data
    if (highScoreManager != null)
    {
        highScoreManager.LoadScores();
        highScoreText.text = highScoreManager.HighScore.ToString();
        totalFruitsText.text = highScoreManager.TotalFruitsSliced.ToString();
        longestComboText.text = $"{highScoreManager.LongestCombo}x";
    }
}

public void ShowSettings()
{
    mainMenuPanel.SetActive(false);
    highScoresPanel.SetActive(false);
    settingsPanel.SetActive(true);
    
    // Sync UI with current settings
    if (settingsManager != null)
    {
        settingsManager.LoadSettings();
        masterVolumeSlider.value = settingsManager.MasterVolume;
        soundEffectsToggle.isOn = settingsManager.SoundEffectsEnabled;
        musicToggle.isOn = settingsManager.MusicEnabled;
    }
}

public void OnPlayClicked()
{
    sceneManager?.LoadGameplayScene();
}

public void OnHighScoresClicked()
{
    ShowHighScores();
}

public void OnSettingsClicked()
{
    ShowSettings();
}

public void OnQuitClicked()
{
    sceneManager?.QuitApplication();
}

public void OnBackClicked()
{
    ShowMainMenu();
}
```

**Run Play Mode Tests:** 18/18 passing ✅

---

### Phase 3: REFACTOR (45 min)
**Goal:** Clean up code, maintain passing tests

```csharp
// Extract magic strings
private const string GAMEPLAY_SCENE = "Gameplay";
private const string MAIN_MENU_SCENE = "MainMenu";

// Add XML documentation
/// <summary>
/// Initialize the main menu UI and wire button events
/// </summary>
public void Initialize() { ... }

// Add null checks
public void OnPlayClicked()
{
    if (sceneManager == null)
    {
        Debug.LogError("SceneTransitionManager not set!");
        return;
    }
    sceneManager.LoadGameplayScene();
}

// Extract common panel logic
private void ShowPanel(GameObject panelToShow)
{
    mainMenuPanel.SetActive(panelToShow == mainMenuPanel);
    highScoresPanel.SetActive(panelToShow == highScoresPanel);
    settingsPanel.SetActive(panelToShow == settingsPanel);
}
```

**Run All Tests:** 18/18 still passing ✅

---

### Phase 4: MANUAL QA (30 min)
**Goal:** Verify UI works in real scene

1. Create MainMenu scene in Unity
2. Create Canvas with MainMenuController
3. Create UI hierarchy (panels, buttons, text)
4. Wire SerializeField references
5. Test navigation manually:
   - [ ] Click Play → should load gameplay (mock for now)
   - [ ] Click High Scores → panel appears
   - [ ] Click Settings → panel appears
   - [ ] Click Back → returns to main menu
   - [ ] Adjust settings → close game → reopen → settings persist

---

## 📁 Files to Create

### Production Code (5 files)
```
Assets/Scripts/
├── Interfaces/
│   └── ISceneTransitionManager.cs (20 lines)
└── UI/
    ├── HighScoreManager.cs (80 lines)
    ├── SettingsManager.cs (100 lines)
    ├── SceneTransitionManager.cs (40 lines)
    └── MainMenuController.cs (150 lines)
```

### Test Code (4 files)
```
Assets/Tests/
├── EditMode/UI/
│   ├── HighScoreManagerTests.cs (4 tests)
│   └── SettingsManagerTests.cs (4 tests)
├── PlayMode/UI/
│   └── MainMenuControllerTests.cs (10 tests)
└── Mocks/
    └── MockSceneTransitionManager.cs (mock)
```

**Total Lines:** ~500 lines production + ~400 lines tests

---

## 🧪 Testing Strategy

### Edit Mode Tests (Data Layer)
**What:** Pure logic without Unity lifecycle
**Why:** Fast, isolated, no scene setup
**Examples:**
- High score saves correctly
- Settings persist across instances
- Volume clamps to 0.0-1.0 range

### Play Mode Tests (UI Integration)
**What:** UI components with Unity lifecycle
**Why:** Test button clicks, panel visibility, data binding
**Examples:**
- Play button triggers scene load
- High scores panel displays correct data
- Settings UI reflects current settings

---

## 🔍 Common Pitfalls

### ❌ Mistake 1: Not Calling PlayerPrefs.Save()
```csharp
// Wrong
PlayerPrefs.SetInt(key, value);
// Data lost on crash!

// Right
PlayerPrefs.SetInt(key, value);
PlayerPrefs.Save();
```

### ❌ Mistake 2: Not Handling Missing Dependencies
```csharp
// Wrong
sceneManager.LoadGameplayScene();
// NullReferenceException if not set!

// Right
if (sceneManager != null)
    sceneManager.LoadGameplayScene();
else
    Debug.LogError("SceneManager not set!");
```

### ❌ Mistake 3: Forgetting Platform-Specific Code
```csharp
// Wrong - Quit button always visible

// Right
#if UNITY_STANDALONE
quitButton.gameObject.SetActive(true);
#else
quitButton.gameObject.SetActive(false);
#endif
```

### ❌ Mistake 4: Not Accumulating Fruit Count
```csharp
// Wrong
TotalFruitsSliced = count; // Replaces!

// Right
TotalFruitsSliced += count; // Accumulates!
```

---

## 📊 Success Metrics

| Metric | Target | Measurement |
|--------|--------|-------------|
| Test Pass Rate | 100% | 18/18 tests passing |
| Test Execution Time | <3s | Unity Test Runner |
| Code Coverage | 80%+ | Unity Coverage package |
| Manual QA | No bugs | 5-min playthrough |

---

## 🆘 Debugging Checklist

### If Tests Fail
1. ✅ Check PlayerPrefs keys match constants
2. ✅ Check null references in managers
3. ✅ Check button event subscriptions
4. ✅ Check panel activation logic
5. ✅ Check platform-specific code

### If UI Doesn't Update
1. ✅ Is Initialize() called?
2. ✅ Are SerializeFields wired in Inspector?
3. ✅ Are managers assigned or found?
4. ✅ Is PlayerPrefs.Save() called?
5. ✅ Are events being triggered?

---

## 📚 Reference Documents

### Quick Access
- **Test Plan:** `docs/test-plans/test-plan-story-011-main-menu.md` (18 test cases)
- **Test Spec:** `docs/test-specs/test-spec-story-011-main-menu.md` (detailed specs)
- **Scaffolding:** `docs/test-scaffolding/test-scaffolding-story-011-main-menu.md` (stub code)
- **Story Doc:** `docs/stories/story-011-main-menu.md` (full requirements)

### Related Stories
- **Story 010:** HUD Display (UI patterns reference)
- **Story 012:** Game Over Screen (next story)
- **Story 013:** Pause Menu (future story)

---

## ✅ Definition of Done

- [ ] All 18 tests written and passing
- [ ] All 5 production files implemented
- [ ] Code coverage ≥ 80%
- [ ] No compiler warnings or errors
- [ ] XML documentation on all public methods
- [ ] Manual QA completed (no UI bugs)
- [ ] Settings persist after app restart
- [ ] Scene transitions work correctly
- [ ] Code reviewed and approved
- [ ] Story marked as "Done" in sprint status

---

## 🎯 Time Estimates

| Phase | Duration | Cumulative |
|-------|----------|------------|
| Phase 1 (RED) | 1 hour | 1h |
| Phase 2 (GREEN) | 2 hours | 3h |
| Phase 3 (REFACTOR) | 45 min | 3h 45m |
| Phase 4 (QA) | 30 min | 4h 15m |
| **Total** | **~4 hours** | |

**With AI assistance:** Can reduce to 2-3 hours if AI generates boilerplate

---

## 🚀 Ready to Start!

You have everything you need:
✅ Complete test plan (18 tests defined)  
✅ Test specifications (detailed assertions)  
✅ Stub code (ready to implement)  
✅ Pattern reference (Story 010 HUD)  
✅ TDD workflow (RED → GREEN → REFACTOR)  

**Next Action:** Copy stub files from scaffolding doc and start Phase 1 (RED)

---

**Questions?**
- Check Story 010 retrospective for UI patterns
- Check test-scaffolding doc for complete code stubs
- Check game-architecture.md for system design

**Good luck! 🎮**

---

**Status:** READY FOR DEVELOPMENT  
**Estimated Completion:** November 30, 2025 (same day start)  
**Developer:** [Your Name]

---

**Approval:**
- Created By: Test Design Agent
- Reviewed By: _____________
- Date: November 30, 2025
