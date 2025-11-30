# STORY-011: Complete Reference Implementation

**Purpose:** Full working code for each production stub. Developer can copy/paste or use as a reference.

---

## 1. HighScoreManager.cs (Complete Implementation)

**File:** `Assets/Scripts/UI/HighScoreManager.cs`

```csharp
using UnityEngine;

namespace NinjaFruit.UI
{
    /// <summary>
    /// Manages high score persistence using PlayerPrefs
    /// </summary>
    public class HighScoreManager : MonoBehaviour
    {
        // PlayerPrefs keys
        private const string HIGH_SCORE_KEY = "HighScore";
        private const string TOTAL_FRUITS_KEY = "TotalFruitsSliced";
        private const string LONGEST_COMBO_KEY = "LongestCombo";
        
        // Properties
        public int HighScore { get; private set; }
        public int TotalFruitsSliced { get; private set; }
        public int LongestCombo { get; private set; }
        
        /// <summary>
        /// Load high scores from PlayerPrefs
        /// </summary>
        public void LoadScores()
        {
            HighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
            TotalFruitsSliced = PlayerPrefs.GetInt(TOTAL_FRUITS_KEY, 0);
            LongestCombo = PlayerPrefs.GetInt(LONGEST_COMBO_KEY, 0);
        }
        
        /// <summary>
        /// Save high score (only if higher than current)
        /// </summary>
        public void SaveHighScore(int score)
        {
            if (score > HighScore)
            {
                HighScore = score;
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
                PlayerPrefs.Save();
            }
        }
        
        /// <summary>
        /// Add to total fruits sliced count (accumulative)
        /// </summary>
        public void SaveFruitCount(int count)
        {
            TotalFruitsSliced += count;
            PlayerPrefs.SetInt(TOTAL_FRUITS_KEY, TotalFruitsSliced);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Save longest combo (only if higher than current)
        /// </summary>
        public void SaveCombo(int combo)
        {
            if (combo > LongestCombo)
            {
                LongestCombo = combo;
                PlayerPrefs.SetInt(LONGEST_COMBO_KEY, combo);
                PlayerPrefs.Save();
            }
        }
        
        /// <summary>
        /// Reset all scores to zero (for testing)
        /// </summary>
        public void ResetScores()
        {
            PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
            PlayerPrefs.DeleteKey(TOTAL_FRUITS_KEY);
            PlayerPrefs.DeleteKey(LONGEST_COMBO_KEY);
            PlayerPrefs.Save();
            LoadScores();
        }
    }
}
```

---

## 2. SettingsManager.cs (Complete Implementation)

**File:** `Assets/Scripts/UI/SettingsManager.cs`

```csharp
using UnityEngine;
using System;

namespace NinjaFruit.UI
{
    /// <summary>
    /// Manages game settings persistence using PlayerPrefs
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        // PlayerPrefs keys
        private const string MASTER_VOLUME_KEY = "MasterVolume";
        private const string SOUND_FX_KEY = "SoundEffectsEnabled";
        private const string MUSIC_KEY = "MusicEnabled";
        
        // Default values
        private const float DEFAULT_MASTER_VOLUME = 0.8f;
        private const bool DEFAULT_SOUND_FX = true;
        private const bool DEFAULT_MUSIC = true;
        
        // Properties
        public float MasterVolume { get; private set; }
        public bool SoundEffectsEnabled { get; private set; }
        public bool MusicEnabled { get; private set; }
        
        // Events
        public event Action<float> OnMasterVolumeChanged;
        public event Action<bool> OnSoundEffectsToggled;
        public event Action<bool> OnMusicToggled;
        
        /// <summary>
        /// Load settings from PlayerPrefs
        /// </summary>
        public void LoadSettings()
        {
            MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, DEFAULT_MASTER_VOLUME);
            SoundEffectsEnabled = PlayerPrefs.GetInt(SOUND_FX_KEY, DEFAULT_SOUND_FX ? 1 : 0) == 1;
            MusicEnabled = PlayerPrefs.GetInt(MUSIC_KEY, DEFAULT_MUSIC ? 1 : 0) == 1;
        }
        
        /// <summary>
        /// Save all settings to PlayerPrefs
        /// </summary>
        public void SaveSettings()
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, MasterVolume);
            PlayerPrefs.SetInt(SOUND_FX_KEY, SoundEffectsEnabled ? 1 : 0);
            PlayerPrefs.SetInt(MUSIC_KEY, MusicEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Set master volume (0.0 - 1.0) and trigger event
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            MasterVolume = Mathf.Clamp01(volume);
            OnMasterVolumeChanged?.Invoke(MasterVolume);
        }
        
        /// <summary>
        /// Toggle sound effects on/off
        /// </summary>
        public void SetSoundEffects(bool enabled)
        {
            SoundEffectsEnabled = enabled;
            OnSoundEffectsToggled?.Invoke(SoundEffectsEnabled);
        }
        
        /// <summary>
        /// Toggle music on/off
        /// </summary>
        public void SetMusic(bool enabled)
        {
            MusicEnabled = enabled;
            OnMusicToggled?.Invoke(MusicEnabled);
        }
    }
}
```

---

## 3. SceneTransitionManager.cs (Complete Implementation)

**File:** `Assets/Scripts/UI/SceneTransitionManager.cs`

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using NinjaFruit.Interfaces;

namespace NinjaFruit.UI
{
    /// <summary>
    /// Handles scene loading and application quit
    /// </summary>
    public class SceneTransitionManager : MonoBehaviour, ISceneTransitionManager
    {
        private const string GAMEPLAY_SCENE = "Gameplay";
        private const string MAIN_MENU_SCENE = "MainMenu";
        
        /// <summary>
        /// Load the gameplay scene
        /// </summary>
        public void LoadGameplayScene()
        {
            SceneManager.LoadScene(GAMEPLAY_SCENE, LoadSceneMode.Single);
        }
        
        /// <summary>
        /// Load the main menu scene
        /// </summary>
        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(MAIN_MENU_SCENE, LoadSceneMode.Single);
        }
        
        /// <summary>
        /// Quit the application (only works in builds, not editor)
        /// </summary>
        public void QuitApplication()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
```

---

## 4. MainMenuController.cs (Complete Implementation)

**File:** `Assets/Scripts/UI/MainMenuController.cs`

```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NinjaFruit.Interfaces;

namespace NinjaFruit.UI
{
    /// <summary>
    /// Main menu UI controller
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] public GameObject mainMenuPanel;
        [SerializeField] public GameObject highScoresPanel;
        [SerializeField] public GameObject settingsPanel;
        
        [Header("Main Menu Buttons")]
        [SerializeField] public Button playButton;
        [SerializeField] public Button highScoresButton;
        [SerializeField] public Button settingsButton;
        [SerializeField] public Button quitButton;
        
        [Header("High Scores UI")]
        [SerializeField] public TextMeshProUGUI highScoreText;
        [SerializeField] public TextMeshProUGUI totalFruitsText;
        [SerializeField] public TextMeshProUGUI longestComboText;
        [SerializeField] public Button highScoresBackButton;
        
        [Header("Settings UI")]
        [SerializeField] public Slider masterVolumeSlider;
        [SerializeField] public Toggle soundEffectsToggle;
        [SerializeField] public Toggle musicToggle;
        [SerializeField] public Button settingsBackButton;
        
        // Dependencies (injected for testing)
        private ISceneTransitionManager sceneManager;
        private HighScoreManager highScoreManager;
        private SettingsManager settingsManager;
        
        /// <summary>
        /// Initialize menu (called on Start or manually in tests)
        /// </summary>
        public void Initialize()
        {
            // Wire button listeners
            playButton?.onClick.AddListener(OnPlayClicked);
            highScoresButton?.onClick.AddListener(OnHighScoresClicked);
            settingsButton?.onClick.AddListener(OnSettingsClicked);
            quitButton?.onClick.AddListener(OnQuitClicked);
            highScoresBackButton?.onClick.AddListener(OnBackClicked);
            settingsBackButton?.onClick.AddListener(OnBackClicked);
            
            // Show main menu by default
            ShowMainMenu();
            
            // Platform-specific: hide quit button on non-PC
            #if UNITY_STANDALONE
            quitButton?.gameObject.SetActive(true);
            #else
            quitButton?.gameObject.SetActive(false);
            #endif
        }
        
        /// <summary>
        /// Set scene transition manager (for dependency injection)
        /// </summary>
        public void SetSceneManager(ISceneTransitionManager manager)
        {
            sceneManager = manager;
        }
        
        /// <summary>
        /// Set high score manager (for dependency injection)
        /// </summary>
        public void SetHighScoreManager(HighScoreManager manager)
        {
            highScoreManager = manager;
        }
        
        /// <summary>
        /// Set settings manager (for dependency injection)
        /// </summary>
        public void SetSettingsManager(SettingsManager manager)
        {
            settingsManager = manager;
        }
        
        /// <summary>
        /// Show main menu panel
        /// </summary>
        public void ShowMainMenu()
        {
            mainMenuPanel?.SetActive(true);
            highScoresPanel?.SetActive(false);
            settingsPanel?.SetActive(false);
        }
        
        /// <summary>
        /// Show high scores panel
        /// </summary>
        public void ShowHighScores()
        {
            mainMenuPanel?.SetActive(false);
            highScoresPanel?.SetActive(true);
            settingsPanel?.SetActive(false);
            
            // Load and display scores
            if (highScoreManager != null)
            {
                highScoreManager.LoadScores();
                highScoreText.text = highScoreManager.HighScore.ToString();
                totalFruitsText.text = highScoreManager.TotalFruitsSliced.ToString();
                longestComboText.text = highScoreManager.LongestCombo.ToString() + "x";
            }
        }
        
        /// <summary>
        /// Show settings panel
        /// </summary>
        public void ShowSettings()
        {
            mainMenuPanel?.SetActive(false);
            highScoresPanel?.SetActive(false);
            settingsPanel?.SetActive(true);
            
            // Load and sync UI with settings
            if (settingsManager != null)
            {
                settingsManager.LoadSettings();
                masterVolumeSlider.value = settingsManager.MasterVolume;
                soundEffectsToggle.isOn = settingsManager.SoundEffectsEnabled;
                musicToggle.isOn = settingsManager.MusicEnabled;
            }
        }
        
        // Button click handlers
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
        
        // Test helper methods (for assertions)
        public bool IsPlayButtonVisible() => playButton != null && playButton.gameObject.activeSelf;
        public bool IsHighScoresButtonVisible() => highScoresButton != null && highScoresButton.gameObject.activeSelf;
        public bool IsSettingsButtonVisible() => settingsButton != null && settingsButton.gameObject.activeSelf;
        public bool IsQuitButtonVisible() => quitButton != null && quitButton.gameObject.activeSelf;
        public bool IsMainMenuPanelVisible() => mainMenuPanel != null && mainMenuPanel.activeSelf;
        public bool IsHighScoresPanelVisible() => highScoresPanel != null && highScoresPanel.activeSelf;
        public bool IsSettingsPanelVisible() => settingsPanel != null && settingsPanel.activeSelf;
        
        public string GetHighScoreText() => highScoreText?.text ?? "";
        public string GetTotalFruitsText() => totalFruitsText?.text ?? "";
        public string GetLongestComboText() => longestComboText?.text ?? "";
        
        public float GetVolumeSliderValue() => masterVolumeSlider?.value ?? 0f;
        public bool GetSoundEffectsToggleValue() => soundEffectsToggle?.isOn ?? false;
        public bool GetMusicToggleValue() => musicToggle?.isOn ?? false;
    }
}
```

---

## ✅ Testing Checklist

After implementing each manager, run the corresponding test group:

### EditMode Tests (after managers 1–2)
```powershell
Unity Editor → Window → Test Runner → EditMode Tab → Run All
Expected: 8/8 tests pass
  ✅ TC-011-001: HighScore_LoadsDefaultOnFirstLaunch_ReturnsZero
  ✅ TC-011-002: HighScore_SavesAndLoadsCorrectly_PersistsValue
  ✅ TC-011-003: HighScore_OnlyUpdatesIfHigher_IgnoresLowerScores
  ✅ TC-011-004: TotalFruitsSliced_Accumulates_AddsToPrevious
  ✅ TC-011-005: Settings_LoadDefaultValues_ReturnsExpectedDefaults
  ✅ TC-011-006: MasterVolume_SavesAndLoads_PersistsCorrectly
  ✅ TC-011-007: SoundEffectsToggle_SavesAndLoads_PersistsState
  ✅ TC-011-008: MusicToggle_SavesAndLoads_PersistsState
```

### PlayMode Tests (after managers 3–4)
```powershell
Unity Editor → Window → Test Runner → PlayMode Tab → Run All
Expected: 10/10 tests pass
  ✅ TC-011-009: MainMenu_Initialize_DisplaysAllButtons
  ✅ TC-011-010: PlayButton_Clicked_LoadsGameplayScene
  ✅ TC-011-011: HighScoresButton_Clicked_ShowsHighScoresPanel
  ✅ TC-011-012: SettingsButton_Clicked_ShowsSettingsPanel
  ✅ TC-011-013: QuitButton_Clicked_CallsApplicationQuit
  ✅ TC-011-014: BackButton_FromHighScores_ReturnsToMainMenu
  ✅ TC-011-015: BackButton_FromSettings_ReturnsToMainMenu
  ✅ TC-011-016: HighScoresPanel_DisplaysCorrectData
  ✅ TC-011-017: SettingsPanel_ReflectsCurrentSettings
  ✅ TC-011-018: SettingsChanges_TriggerEvents
```

---

## 🎯 Key Implementation Points

1. **PlayerPrefs usage:**
   - Always call `PlayerPrefs.Save()` after writes.
   - Use `GetInt()` / `GetFloat()` with sensible defaults.
   - Boolean values stored as int (1 = true, 0 = false).

2. **Accumulation logic (TotalFruitsSliced):**
   - Read existing value: `current = PlayerPrefs.GetInt(TOTAL_FRUITS_KEY, 0)`
   - Add new count: `current += count`
   - Write back: `PlayerPrefs.SetInt(..., current)`

3. **Conditional updates (HighScore, LongestCombo):**
   - Only update if new value is greater than existing.
   - Use `if (value > Property)` before saving.

4. **Events in SettingsManager:**
   - Invoke immediately after property update.
   - Use null-coalescing: `OnMasterVolumeChanged?.Invoke(...)`

5. **Panel switching in MainMenuController:**
   - Use `SetActive(true/false)` to toggle visibility.
   - Always restore main menu state on back button.

6. **Platform-specific code:**
   - Use `#if UNITY_STANDALONE` to conditionally show Quit button.
   - Handle editor quit with `UnityEditor.EditorApplication.isPlaying = false`.

---

**Good luck! All tests should pass with this reference. 🟢**
