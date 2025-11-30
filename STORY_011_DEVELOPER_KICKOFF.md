# STORY-011: Developer Kickoff — GREEN Phase Implementation

**Status:** RED Phase Complete (16 tests failing) → Ready for GREEN Phase  
**Date:** November 30, 2025  
**Target:** All 18 tests passing ✅

---

## 🎯 Quick Start

Hi — I created the test scaffolding for STORY-011. Tests are RED now. Please implement TODOs to satisfy them in this order:

1. **`Assets/Scripts/UI/HighScoreManager.cs`** (runs `Assets/Tests/EditMode/UI/HighScoreManagerTests.cs`)
2. **`Assets/Scripts/UI/SettingsManager.cs`** (runs `Assets/Tests/EditMode/UI/SettingsManagerTests.cs`)
3. **`Assets/Scripts/UI/SceneTransitionManager.cs`** (minimal, but complete the implementation)
4. **`Assets/Scripts/UI/MainMenuController.cs`** (runs `Assets/Tests/PlayMode/UI/MainMenuControllerTests.cs`)

Run tests after each part and push a focused PR with tests passing. I'll review and help if anything fails.

---

## 📋 Task Breakdown

### Task 1: HighScoreManager (30–45 min)
**File:** `Assets/Scripts/UI/HighScoreManager.cs`  
**Tests:** `TC-011-001..004` (4 Edit Mode tests)

**What to implement:**
- `LoadScores()` — Load from `PlayerPrefs` with defaults (0, 0, 0)
- `SaveHighScore(int score)` — Only update if `score > HighScore`
- `SaveFruitCount(int count)` — **Accumulate** (add to existing, don't replace)
- `SaveCombo(int combo)` — Only update if `combo > LongestCombo`
- `ResetScores()` — Clear all PlayerPrefs keys and reload

**PlayerPrefs Keys:**
```csharp
const string HIGH_SCORE_KEY = "HighScore";
const string TOTAL_FRUITS_KEY = "TotalFruitsSliced";
const string LONGEST_COMBO_KEY = "LongestCombo";
```

**Implementation hints:**
```csharp
// Load: use GetInt with defaults
HighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);

// Save & accumulate: read first, then write back
int current = PlayerPrefs.GetInt(TOTAL_FRUITS_KEY, 0);
PlayerPrefs.SetInt(TOTAL_FRUITS_KEY, current + count);
PlayerPrefs.Save();

// Conditional update: only if higher
if (score > HighScore)
{
    PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
    PlayerPrefs.Save();
}
```

**Verification:**
```powershell
# In Unity Test Runner:
Unity Editor → Window → Test Runner → EditMode Tab → Run All
# Expected: 4/4 tests pass ✅
```

---

### Task 2: SettingsManager (30–45 min)
**File:** `Assets/Scripts/UI/SettingsManager.cs`  
**Tests:** `TC-011-005..008` (4 Edit Mode tests)

**What to implement:**
- `LoadSettings()` — Load from `PlayerPrefs` or use defaults (0.8f, true, true)
- `SaveSettings()` — Persist current values to `PlayerPrefs`
- `SetMasterVolume(float volume)` — Clamp to [0.0, 1.0], invoke `OnMasterVolumeChanged` event
- `SetSoundEffects(bool enabled)` — Set flag, invoke `OnSoundEffectsToggled` event
- `SetMusic(bool enabled)` — Set flag, invoke `OnMusicToggled` event

**PlayerPrefs Keys:**
```csharp
const string MASTER_VOLUME_KEY = "MasterVolume";
const string SOUND_FX_KEY = "SoundEffectsEnabled";
const string MUSIC_KEY = "MusicEnabled";
```

**Implementation hints:**
```csharp
// Load with defaults
public void LoadSettings()
{
    MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 0.8f);
    SoundEffectsEnabled = PlayerPrefs.GetInt(SOUND_FX_KEY, 1) == 1;
    MusicEnabled = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
}

// Save
public void SaveSettings()
{
    PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, MasterVolume);
    PlayerPrefs.SetInt(SOUND_FX_KEY, SoundEffectsEnabled ? 1 : 0);
    PlayerPrefs.SetInt(MUSIC_KEY, MusicEnabled ? 1 : 0);
    PlayerPrefs.Save();
}

// Setter with event
public void SetMasterVolume(float volume)
{
    MasterVolume = Mathf.Clamp01(volume);
    OnMasterVolumeChanged?.Invoke(MasterVolume);
}
```

**Verification:**
```powershell
# In Unity Test Runner:
Unity Editor → Window → Test Runner → EditMode Tab → Run All
# Expected: 8/8 tests pass ✅ (4 from HighScoreManager + 4 from SettingsManager)
```

---

### Task 3: SceneTransitionManager (15–30 min)
**File:** `Assets/Scripts/UI/SceneTransitionManager.cs`  
**Tests:** Used by PlayMode tests (via mock in tests, but implement for completeness)

**What to implement:**
- `LoadGameplayScene()` — Call `SceneManager.LoadScene("Gameplay", LoadSceneMode.Single)`
- `LoadMainMenuScene()` — Call `SceneManager.LoadScene("MainMenu", LoadSceneMode.Single)`
- `QuitApplication()` — Already has conditional compile; no changes needed

**Implementation:**
```csharp
public void LoadGameplayScene()
{
    SceneManager.LoadScene(GAMEPLAY_SCENE, LoadSceneMode.Single);
}

public void LoadMainMenuScene()
{
    SceneManager.LoadScene(MAIN_MENU_SCENE, LoadSceneMode.Single);
}

// QuitApplication() already correct with #if UNITY_EDITOR logic
```

**Verification:**
- Compile check only (no dedicated tests for this; used via mock in PlayMode tests).

---

### Task 4: MainMenuController (45–90 min)
**File:** `Assets/Scripts/UI/MainMenuController.cs`  
**Tests:** `TC-011-009..018` (10 Play Mode tests)

**What to implement:**
- `Initialize()` — Show main menu, hide other panels, wire button listeners
- `ShowMainMenu()` — Activate main menu panel, deactivate others
- `ShowHighScores()` — Activate high scores panel, load and display scores
- `ShowSettings()` — Activate settings panel, sync UI with current settings
- **Button handlers:** `OnPlayClicked()`, `OnHighScoresClicked()`, `OnSettingsClicked()`, `OnQuitClicked()`, `OnBackClicked()`

**Implementation hints:**

```csharp
public void Initialize()
{
    // Wire button listeners
    playButton.onClick.AddListener(OnPlayClicked);
    highScoresButton.onClick.AddListener(OnHighScoresClicked);
    settingsButton.onClick.AddListener(OnSettingsClicked);
    quitButton.onClick.AddListener(OnQuitClicked);
    highScoresBackButton.onClick.AddListener(OnBackClicked);
    settingsBackButton.onClick.AddListener(OnBackClicked);
    
    // Show main menu by default
    ShowMainMenu();
    
    // Platform-specific: hide quit on non-PC
    #if UNITY_STANDALONE
    quitButton.gameObject.SetActive(true);
    #else
    quitButton.gameObject.SetActive(false);
    #endif
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
    
    // Load and display scores
    if (highScoreManager != null)
    {
        highScoreManager.LoadScores();
        highScoreText.text = highScoreManager.HighScore.ToString();
        totalFruitsText.text = highScoreManager.TotalFruitsSliced.ToString();
        longestComboText.text = highScoreManager.LongestCombo.ToString() + "x";
    }
}

public void ShowSettings()
{
    mainMenuPanel.SetActive(false);
    highScoresPanel.SetActive(false);
    settingsPanel.SetActive(true);
    
    // Sync UI with settings
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

// OnBackClicked needs to know which panel is active
private GameObject currentPanel;

public void OnBackClicked()
{
    ShowMainMenu();
}
```

**Note:** You may need to track `currentPanel` to handle the back button correctly in more complex scenarios. For now, `ShowMainMenu()` suffices.

**Verification:**
```powershell
# In Unity Test Runner:
Unity Editor → Window → Test Runner → PlayMode Tab → Run All
# Expected: 18/18 tests pass ✅
```

---

## ✅ Acceptance Checklist for PR

- [ ] All 18 tests pass (EditMode: 8, PlayMode: 10)
- [ ] No new console errors or warnings
- [ ] High score accumulation works (fruits add, not replace)
- [ ] Settings persist across manager instances
- [ ] Panel switching works smoothly
- [ ] Scene manager calls are invoked (or mock invoked in tests)
- [ ] Quit button hidden on non-UNITY_STANDALONE
- [ ] Small, logical commits (one per manager, one for controller)

---

## 🔄 Recommended Workflow

1. **Create branch:**
   ```powershell
   git checkout -b feature/story-011/green-impl
   ```

2. **Implement HighScoreManager:**
   - Edit `Assets/Scripts/UI/HighScoreManager.cs`
   - Run EditMode tests → should see 4/4 pass
   - Commit: `git commit -m "feat(STORY-011): implement HighScoreManager"`

3. **Implement SettingsManager:**
   - Edit `Assets/Scripts/UI/SettingsManager.cs`
   - Run EditMode tests → should see 8/8 pass
   - Commit: `git commit -m "feat(STORY-011): implement SettingsManager"`

4. **Implement SceneTransitionManager:**
   - Edit `Assets/Scripts/UI/SceneTransitionManager.cs`
   - Quick compile check (no dedicated tests)
   - Commit: `git commit -m "feat(STORY-011): implement SceneTransitionManager"`

5. **Implement MainMenuController:**
   - Edit `Assets/Scripts/UI/MainMenuController.cs`
   - Run PlayMode tests → iterate until 18/18 pass
   - Commit: `git commit -m "feat(STORY-011): implement MainMenuController"`

6. **Final checks:**
   - All 18 tests pass ✅
   - No console errors
   - Push and create PR with title: `feat(STORY-011): implement GREEN phase`

---

## 🆘 If You Get Stuck

**Flaky PlayMode test?**
- Check if `OnBackClicked` logic needs refinement (may need to track current active panel).
- Share the failing test name + error message, and I'll help.

**PlayerPrefs not persisting?**
- Ensure `PlayerPrefs.Save()` is called after `SetInt()` or `SetFloat()`.
- Tests clean up with `PlayerPrefs.DeleteAll()`, so isolation is handled.

**Mock not working?**
- The mock is already defined in the test file (`MockSceneTransitionManager`).
- Just ensure dependency injection via `SetSceneManager()` is called in tests.

---

## 📞 Contact

- Ping me with test failures, and I'll provide exact code fixes or patches.
- Expected total time: **3–4 hours** (1hr per manager + buffer for PlayMode iteration).

---

**Good luck! Let's get to GREEN. 🟢**
