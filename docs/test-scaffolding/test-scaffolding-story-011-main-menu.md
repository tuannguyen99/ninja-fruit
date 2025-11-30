# Test Scaffolding: STORY-011 Main Menu & Navigation

**Story:** STORY-011 - Main Menu & Navigation  
**Epic:** EPIC-004 - User Interface & Game Flow  
**Scaffolding Version:** 1.0  
**Author:** Test Design Agent  
**Date:** November 30, 2025

---

## Overview

This document provides complete test scaffolding code for Story 011, including:
- Test class stubs with all 18 test methods
- Production code stubs (minimal implementation to compile)
- Helper utility classes
- Mock objects for testing

**Purpose:** Enable TDD workflow by providing compilable code structure before implementation.

---

## File Structure

```
ninja-fruit/Assets/
├── Scripts/
│   ├── UI/
│   │   ├── MainMenuController.cs ← Stub implementation
│   │   ├── HighScoreManager.cs ← Stub implementation
│   │   ├── SettingsManager.cs ← Stub implementation
│   │   └── SceneTransitionManager.cs ← Stub implementation
│   └── Interfaces/
│       └── ISceneTransitionManager.cs ← Interface for mocking
├── Tests/
│   ├── EditMode/
│   │   └── UI/
│   │       ├── HighScoreManagerTests.cs ← 4 tests
│   │       └── SettingsManagerTests.cs ← 4 tests
│   ├── PlayMode/
│   │   └── UI/
│   │       └── MainMenuControllerTests.cs ← 10 tests
│   └── Mocks/
│       └── MockSceneTransitionManager.cs ← Mock for testing
```

---

## Production Code Stubs

### 1. ISceneTransitionManager.cs (Interface)
**File:** `Assets/Scripts/Interfaces/ISceneTransitionManager.cs`

```csharp
using UnityEngine;

namespace NinjaFruit.Interfaces
{
    /// <summary>
    /// Interface for scene transition management (enables mocking in tests)
    /// </summary>
    public interface ISceneTransitionManager
    {
        void LoadGameplayScene();
        void LoadMainMenuScene();
        void QuitApplication();
    }
}
```

---

### 2. HighScoreManager.cs (Stub)
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
            // TODO: Implement PlayerPrefs loading logic
            HighScore = 0;
            TotalFruitsSliced = 0;
            LongestCombo = 0;
        }
        
        /// <summary>
        /// Save high score (only if higher than current)
        /// </summary>
        public void SaveHighScore(int score)
        {
            // TODO: Implement high score save logic
            // Should only update if score > HighScore
        }
        
        /// <summary>
        /// Add to total fruits sliced count (accumulative)
        /// </summary>
        public void SaveFruitCount(int count)
        {
            // TODO: Implement fruit count accumulation logic
        }
        
        /// <summary>
        /// Save longest combo (only if higher than current)
        /// </summary>
        public void SaveCombo(int combo)
        {
            // TODO: Implement combo save logic
        }
        
        /// <summary>
        /// Reset all scores to zero (for testing)
        /// </summary>
        public void ResetScores()
        {
            // TODO: Implement reset logic
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

### 3. SettingsManager.cs (Stub)
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
            // TODO: Implement PlayerPrefs loading with defaults
            MasterVolume = DEFAULT_MASTER_VOLUME;
            SoundEffectsEnabled = DEFAULT_SOUND_FX;
            MusicEnabled = DEFAULT_MUSIC;
        }
        
        /// <summary>
        /// Save all settings to PlayerPrefs
        /// </summary>
        public void SaveSettings()
        {
            // TODO: Implement PlayerPrefs save logic
        }
        
        /// <summary>
        /// Set master volume (0.0 - 1.0) and trigger event
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            // TODO: Implement volume setting with clamping
            MasterVolume = Mathf.Clamp01(volume);
            OnMasterVolumeChanged?.Invoke(MasterVolume);
        }
        
        /// <summary>
        /// Toggle sound effects on/off
        /// </summary>
        public void SetSoundEffects(bool enabled)
        {
            // TODO: Implement sound effects toggle
            SoundEffectsEnabled = enabled;
            OnSoundEffectsToggled?.Invoke(SoundEffectsEnabled);
        }
        
        /// <summary>
        /// Toggle music on/off
        /// </summary>
        public void SetMusic(bool enabled)
        {
            // TODO: Implement music toggle
            MusicEnabled = enabled;
            OnMusicToggled?.Invoke(MusicEnabled);
        }
    }
}
```

---

### 4. SceneTransitionManager.cs (Stub)
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
            // TODO: Implement scene loading
            // SceneManager.LoadScene(GAMEPLAY_SCENE, LoadSceneMode.Single);
        }
        
        /// <summary>
        /// Load the main menu scene
        /// </summary>
        public void LoadMainMenuScene()
        {
            // TODO: Implement scene loading
            // SceneManager.LoadScene(MAIN_MENU_SCENE, LoadSceneMode.Single);
        }
        
        /// <summary>
        /// Quit the application (only works in builds, not editor)
        /// </summary>
        public void QuitApplication()
        {
            // TODO: Implement quit logic
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

### 5. MainMenuController.cs (Stub)
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
            // TODO: Implement initialization logic
            // - Show main menu panel
            // - Hide other panels
            // - Wire button click events
            // - Set platform-specific UI visibility
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
            // TODO: Implement panel switching
        }
        
        /// <summary>
        /// Show high scores panel
        /// </summary>
        public void ShowHighScores()
        {
            // TODO: Implement panel switching + data loading
        }
        
        /// <summary>
        /// Show settings panel
        /// </summary>
        public void ShowSettings()
        {
            // TODO: Implement panel switching + settings sync
        }
        
        // Button click handlers
        public void OnPlayClicked()
        {
            // TODO: Implement play button logic
        }
        
        public void OnHighScoresClicked()
        {
            // TODO: Implement high scores button logic
        }
        
        public void OnSettingsClicked()
        {
            // TODO: Implement settings button logic
        }
        
        public void OnQuitClicked()
        {
            // TODO: Implement quit button logic
        }
        
        public void OnBackClicked()
        {
            // TODO: Implement back button logic
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

## Test Code Scaffolding

### 6. HighScoreManagerTests.cs
**File:** `Assets/Tests/EditMode/UI/HighScoreManagerTests.cs`

```csharp
using NUnit.Framework;
using UnityEngine;
using NinjaFruit.UI;

namespace NinjaFruit.Tests.EditMode.UI
{
    /// <summary>
    /// Edit Mode tests for HighScoreManager
    /// </summary>
    [TestFixture]
    public class HighScoreManagerTests
    {
        [TearDown]
        public void TearDown()
        {
            // Clean up PlayerPrefs after each test
            PlayerPrefs.DeleteAll();
        }
        
        [Test]
        public void HighScore_LoadsDefaultOnFirstLaunch_ReturnsZero()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var managerGO = new GameObject("HighScoreManager");
            var manager = managerGO.AddComponent<HighScoreManager>();
            
            // Act
            manager.LoadScores();
            
            // Assert
            Assert.AreEqual(0, manager.HighScore);
            
            // Cleanup
            Object.DestroyImmediate(managerGO);
        }
        
        [Test]
        public void HighScore_SavesAndLoadsCorrectly_PersistsValue()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var manager1GO = new GameObject("HighScoreManager1");
            var manager1 = manager1GO.AddComponent<HighScoreManager>();
            
            // Act
            manager1.SaveHighScore(1250);
            Object.DestroyImmediate(manager1GO);
            
            var manager2GO = new GameObject("HighScoreManager2");
            var manager2 = manager2GO.AddComponent<HighScoreManager>();
            manager2.LoadScores();
            
            // Assert
            Assert.AreEqual(1250, manager2.HighScore);
            
            // Cleanup
            Object.DestroyImmediate(manager2GO);
        }
        
        [Test]
        public void HighScore_OnlyUpdatesIfHigher_IgnoresLowerScores()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var managerGO = new GameObject("HighScoreManager");
            var manager = managerGO.AddComponent<HighScoreManager>();
            
            // Act
            manager.SaveHighScore(1000);
            manager.SaveHighScore(500); // Lower score
            manager.LoadScores();
            
            // Assert
            Assert.AreEqual(1000, manager.HighScore, "High score should not be overwritten by lower score");
            
            // Cleanup
            Object.DestroyImmediate(managerGO);
        }
        
        [Test]
        public void TotalFruitsSliced_Accumulates_AddsToPrevious()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var managerGO = new GameObject("HighScoreManager");
            var manager = managerGO.AddComponent<HighScoreManager>();
            
            // Act
            manager.SaveFruitCount(50);
            manager.SaveFruitCount(75); // Should add, not replace
            manager.LoadScores();
            
            // Assert
            Assert.AreEqual(125, manager.TotalFruitsSliced, "Fruit count should accumulate");
            
            // Cleanup
            Object.DestroyImmediate(managerGO);
        }
    }
}
```

---

### 7. SettingsManagerTests.cs
**File:** `Assets/Tests/EditMode/UI/SettingsManagerTests.cs`

```csharp
using NUnit.Framework;
using UnityEngine;
using NinjaFruit.UI;

namespace NinjaFruit.Tests.EditMode.UI
{
    /// <summary>
    /// Edit Mode tests for SettingsManager
    /// </summary>
    [TestFixture]
    public class SettingsManagerTests
    {
        [TearDown]
        public void TearDown()
        {
            // Clean up PlayerPrefs after each test
            PlayerPrefs.DeleteAll();
        }
        
        [Test]
        public void Settings_LoadDefaultValues_ReturnsExpectedDefaults()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var managerGO = new GameObject("SettingsManager");
            var manager = managerGO.AddComponent<SettingsManager>();
            
            // Act
            manager.LoadSettings();
            
            // Assert
            Assert.AreEqual(0.8f, manager.MasterVolume, 0.01f, "Default volume should be 0.8");
            Assert.IsTrue(manager.SoundEffectsEnabled, "Sound effects should be enabled by default");
            Assert.IsTrue(manager.MusicEnabled, "Music should be enabled by default");
            
            // Cleanup
            Object.DestroyImmediate(managerGO);
        }
        
        [Test]
        public void MasterVolume_SavesAndLoads_PersistsCorrectly()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var manager1GO = new GameObject("SettingsManager1");
            var manager1 = manager1GO.AddComponent<SettingsManager>();
            
            // Act
            manager1.SetMasterVolume(0.5f);
            manager1.SaveSettings();
            Object.DestroyImmediate(manager1GO);
            
            var manager2GO = new GameObject("SettingsManager2");
            var manager2 = manager2GO.AddComponent<SettingsManager>();
            manager2.LoadSettings();
            
            // Assert
            Assert.AreEqual(0.5f, manager2.MasterVolume, 0.01f, "Volume should persist");
            
            // Cleanup
            Object.DestroyImmediate(manager2GO);
        }
        
        [Test]
        public void SoundEffectsToggle_SavesAndLoads_PersistsState()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var manager1GO = new GameObject("SettingsManager1");
            var manager1 = manager1GO.AddComponent<SettingsManager>();
            
            // Act
            manager1.SetSoundEffects(false);
            manager1.SaveSettings();
            Object.DestroyImmediate(manager1GO);
            
            var manager2GO = new GameObject("SettingsManager2");
            var manager2 = manager2GO.AddComponent<SettingsManager>();
            manager2.LoadSettings();
            
            // Assert
            Assert.IsFalse(manager2.SoundEffectsEnabled, "Sound effects toggle should persist");
            
            // Cleanup
            Object.DestroyImmediate(manager2GO);
        }
        
        [Test]
        public void MusicToggle_SavesAndLoads_PersistsState()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var manager1GO = new GameObject("SettingsManager1");
            var manager1 = manager1GO.AddComponent<SettingsManager>();
            
            // Act
            manager1.SetMusic(false);
            manager1.SaveSettings();
            Object.DestroyImmediate(manager1GO);
            
            var manager2GO = new GameObject("SettingsManager2");
            var manager2 = manager2GO.AddComponent<SettingsManager>();
            manager2.LoadSettings();
            
            // Assert
            Assert.IsFalse(manager2.MusicEnabled, "Music toggle should persist");
            
            // Cleanup
            Object.DestroyImmediate(manager2GO);
        }
    }
}
```

---

### 8. MockSceneTransitionManager.cs
**File:** `Assets/Tests/Mocks/MockSceneTransitionManager.cs`

```csharp
using NinjaFruit.Interfaces;

namespace NinjaFruit.Tests.Mocks
{
    /// <summary>
    /// Mock implementation of ISceneTransitionManager for testing
    /// </summary>
    public class MockSceneTransitionManager : ISceneTransitionManager
    {
        public bool LoadGameplaySceneCalled { get; private set; }
        public bool LoadMainMenuSceneCalled { get; private set; }
        public bool QuitApplicationCalled { get; private set; }
        public string LastSceneLoaded { get; private set; }
        
        public void LoadGameplayScene()
        {
            LoadGameplaySceneCalled = true;
            LastSceneLoaded = "Gameplay";
        }
        
        public void LoadMainMenuScene()
        {
            LoadMainMenuSceneCalled = true;
            LastSceneLoaded = "MainMenu";
        }
        
        public void QuitApplication()
        {
            QuitApplicationCalled = true;
        }
        
        public void Reset()
        {
            LoadGameplaySceneCalled = false;
            LoadMainMenuSceneCalled = false;
            QuitApplicationCalled = false;
            LastSceneLoaded = null;
        }
    }
}
```

---

### 9. MainMenuControllerTests.cs
**File:** `Assets/Tests/PlayMode/UI/MainMenuControllerTests.cs`

```csharp
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using NinjaFruit.UI;
using NinjaFruit.Tests.Mocks;

namespace NinjaFruit.Tests.PlayMode.UI
{
    /// <summary>
    /// Play Mode integration tests for MainMenuController
    /// </summary>
    [TestFixture]
    public class MainMenuControllerTests
    {
        private GameObject canvasGO;
        private Canvas canvas;
        
        [SetUp]
        public void SetUp()
        {
            // Create test canvas
            canvasGO = new GameObject("TestCanvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
            
            // Create event system
            var eventSystemGO = new GameObject("EventSystem");
            eventSystemGO.AddComponent<EventSystem>();
            eventSystemGO.AddComponent<InputSystemUIInputModule>();
        }
        
        [TearDown]
        public void TearDown()
        {
            // Clean up all test objects
            Object.DestroyImmediate(GameObject.Find("EventSystem"));
            Object.DestroyImmediate(canvasGO);
            PlayerPrefs.DeleteAll();
        }
        
        [UnityTest]
        public IEnumerator MainMenu_Initialize_DisplaysAllButtons()
        {
            // Arrange
            var menu = CreateTestMainMenu();
            
            // Act
            menu.Initialize();
            yield return null;
            
            // Assert
            Assert.IsTrue(menu.IsPlayButtonVisible(), "Play button should be visible");
            Assert.IsTrue(menu.IsHighScoresButtonVisible(), "High Scores button should be visible");
            Assert.IsTrue(menu.IsSettingsButtonVisible(), "Settings button should be visible");
            
            #if UNITY_STANDALONE
            Assert.IsTrue(menu.IsQuitButtonVisible(), "Quit button should be visible on PC");
            #else
            Assert.IsFalse(menu.IsQuitButtonVisible(), "Quit button should be hidden on non-PC platforms");
            #endif
            
            // Cleanup
            Object.Destroy(menu.gameObject);
        }
        
        [UnityTest]
        public IEnumerator PlayButton_Clicked_LoadsGameplayScene()
        {
            // Arrange
            var menu = CreateTestMainMenu();
            var mockSceneManager = new MockSceneTransitionManager();
            menu.SetSceneManager(mockSceneManager);
            
            // Act
            menu.OnPlayClicked();
            yield return null;
            
            // Assert
            Assert.IsTrue(mockSceneManager.LoadGameplaySceneCalled, "Gameplay scene load should be called");
            
            // Cleanup
            Object.Destroy(menu.gameObject);
        }
        
        [UnityTest]
        public IEnumerator HighScoresButton_Clicked_ShowsHighScoresPanel()
        {
            // Arrange
            var menu = CreateTestMainMenu();
            menu.Initialize();
            yield return null;
            
            // Act
            menu.OnHighScoresClicked();
            yield return null;
            
            // Assert
            Assert.IsTrue(menu.IsHighScoresPanelVisible(), "High scores panel should be visible");
            Assert.IsFalse(menu.IsMainMenuPanelVisible(), "Main menu panel should be hidden");
            
            // Cleanup
            Object.Destroy(menu.gameObject);
        }
        
        [UnityTest]
        public IEnumerator SettingsButton_Clicked_ShowsSettingsPanel()
        {
            // Arrange
            var menu = CreateTestMainMenu();
            menu.Initialize();
            yield return null;
            
            // Act
            menu.OnSettingsClicked();
            yield return null;
            
            // Assert
            Assert.IsTrue(menu.IsSettingsPanelVisible(), "Settings panel should be visible");
            Assert.IsFalse(menu.IsMainMenuPanelVisible(), "Main menu panel should be hidden");
            
            // Cleanup
            Object.Destroy(menu.gameObject);
        }
        
        [UnityTest]
        public IEnumerator QuitButton_Clicked_CallsApplicationQuit()
        {
            // Arrange
            var menu = CreateTestMainMenu();
            var mockSceneManager = new MockSceneTransitionManager();
            menu.SetSceneManager(mockSceneManager);
            
            // Act
            menu.OnQuitClicked();
            yield return null;
            
            // Assert
            Assert.IsTrue(mockSceneManager.QuitApplicationCalled, "Quit application should be called");
            
            // Cleanup
            Object.Destroy(menu.gameObject);
        }
        
        [UnityTest]
        public IEnumerator BackButton_FromHighScores_ReturnsToMainMenu()
        {
            // Arrange
            var menu = CreateTestMainMenu();
            menu.Initialize();
            menu.OnHighScoresClicked();
            yield return null;
            
            // Act
            menu.OnBackClicked();
            yield return null;
            
            // Assert
            Assert.IsTrue(menu.IsMainMenuPanelVisible(), "Main menu should be visible");
            Assert.IsFalse(menu.IsHighScoresPanelVisible(), "High scores panel should be hidden");
            
            // Cleanup
            Object.Destroy(menu.gameObject);
        }
        
        [UnityTest]
        public IEnumerator BackButton_FromSettings_ReturnsToMainMenu()
        {
            // Arrange
            var menu = CreateTestMainMenu();
            menu.Initialize();
            menu.OnSettingsClicked();
            yield return null;
            
            // Act
            menu.OnBackClicked();
            yield return null;
            
            // Assert
            Assert.IsTrue(menu.IsMainMenuPanelVisible(), "Main menu should be visible");
            Assert.IsFalse(menu.IsSettingsPanelVisible(), "Settings panel should be hidden");
            
            // Cleanup
            Object.Destroy(menu.gameObject);
        }
        
        [UnityTest]
        public IEnumerator HighScoresPanel_DisplaysCorrectData()
        {
            // Arrange
            var highScoreManagerGO = new GameObject("HighScoreManager");
            var highScoreManager = highScoreManagerGO.AddComponent<HighScoreManager>();
            highScoreManager.SaveHighScore(1250);
            highScoreManager.SaveFruitCount(50);
            highScoreManager.SaveCombo(5);
            
            var menu = CreateTestMainMenu();
            menu.SetHighScoreManager(highScoreManager);
            
            // Act
            menu.OnHighScoresClicked();
            yield return null;
            
            // Assert
            Assert.AreEqual("1250", menu.GetHighScoreText(), "High score text should match");
            Assert.AreEqual("50", menu.GetTotalFruitsText(), "Total fruits text should match");
            Assert.AreEqual("5x", menu.GetLongestComboText(), "Longest combo text should match");
            
            // Cleanup
            Object.Destroy(menu.gameObject);
            Object.Destroy(highScoreManagerGO);
        }
        
        [UnityTest]
        public IEnumerator SettingsPanel_ReflectsCurrentSettings()
        {
            // Arrange
            var settingsManagerGO = new GameObject("SettingsManager");
            var settingsManager = settingsManagerGO.AddComponent<SettingsManager>();
            settingsManager.SetMasterVolume(0.5f);
            settingsManager.SetSoundEffects(false);
            settingsManager.SetMusic(true);
            
            var menu = CreateTestMainMenu();
            menu.SetSettingsManager(settingsManager);
            
            // Act
            menu.OnSettingsClicked();
            yield return null;
            
            // Assert
            Assert.AreEqual(0.5f, menu.GetVolumeSliderValue(), 0.01f, "Volume slider should match");
            Assert.IsFalse(menu.GetSoundEffectsToggleValue(), "Sound effects toggle should be off");
            Assert.IsTrue(menu.GetMusicToggleValue(), "Music toggle should be on");
            
            // Cleanup
            Object.Destroy(menu.gameObject);
            Object.Destroy(settingsManagerGO);
        }
        
        [UnityTest]
        public IEnumerator SettingsChanges_TriggerEvents()
        {
            // Arrange
            var settingsManagerGO = new GameObject("SettingsManager");
            var settingsManager = settingsManagerGO.AddComponent<SettingsManager>();
            float receivedVolume = -1f;
            settingsManager.OnMasterVolumeChanged += (vol) => receivedVolume = vol;
            
            // Act
            settingsManager.SetMasterVolume(0.7f);
            yield return null;
            
            // Assert
            Assert.AreEqual(0.7f, receivedVolume, 0.01f, "Volume change event should fire");
            
            // Cleanup
            Object.Destroy(settingsManagerGO);
        }
        
        // Helper method to create test main menu
        private MainMenuController CreateTestMainMenu()
        {
            var menuGO = new GameObject("MainMenu");
            menuGO.transform.SetParent(canvas.transform);
            var menu = menuGO.AddComponent<MainMenuController>();
            
            // Create panels
            menu.mainMenuPanel = CreatePanel(menuGO, "MainMenuPanel");
            menu.highScoresPanel = CreatePanel(menuGO, "HighScoresPanel");
            menu.settingsPanel = CreatePanel(menuGO, "SettingsPanel");
            
            // Create buttons
            menu.playButton = CreateButton(menu.mainMenuPanel, "PlayButton");
            menu.highScoresButton = CreateButton(menu.mainMenuPanel, "HighScoresButton");
            menu.settingsButton = CreateButton(menu.mainMenuPanel, "SettingsButton");
            menu.quitButton = CreateButton(menu.mainMenuPanel, "QuitButton");
            
            // Create high scores UI
            menu.highScoreText = CreateText(menu.highScoresPanel, "HighScoreText");
            menu.totalFruitsText = CreateText(menu.highScoresPanel, "TotalFruitsText");
            menu.longestComboText = CreateText(menu.highScoresPanel, "LongestComboText");
            
            // Create settings UI
            menu.masterVolumeSlider = CreateSlider(menu.settingsPanel, "VolumeSlider");
            menu.soundEffectsToggle = CreateToggle(menu.settingsPanel, "SoundFXToggle");
            menu.musicToggle = CreateToggle(menu.settingsPanel, "MusicToggle");
            
            return menu;
        }
        
        private GameObject CreatePanel(GameObject parent, string name)
        {
            var panelGO = new GameObject(name);
            panelGO.transform.SetParent(parent.transform);
            panelGO.AddComponent<RectTransform>();
            return panelGO;
        }
        
        private Button CreateButton(GameObject parent, string name)
        {
            var buttonGO = new GameObject(name);
            buttonGO.transform.SetParent(parent.transform);
            buttonGO.AddComponent<RectTransform>();
            var button = buttonGO.AddComponent<Button>();
            buttonGO.AddComponent<Image>();
            return button;
        }
        
        private TextMeshProUGUI CreateText(GameObject parent, string name)
        {
            var textGO = new GameObject(name);
            textGO.transform.SetParent(parent.transform);
            textGO.AddComponent<RectTransform>();
            var text = textGO.AddComponent<TextMeshProUGUI>();
            text.text = "0";
            return text;
        }
        
        private Slider CreateSlider(GameObject parent, string name)
        {
            var sliderGO = new GameObject(name);
            sliderGO.transform.SetParent(parent.transform);
            sliderGO.AddComponent<RectTransform>();
            var slider = sliderGO.AddComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 0.8f;
            return slider;
        }
        
        private Toggle CreateToggle(GameObject parent, string name)
        {
            var toggleGO = new GameObject(name);
            toggleGO.transform.SetParent(parent.transform);
            toggleGO.AddComponent<RectTransform>();
            var toggle = toggleGO.AddComponent<Toggle>();
            toggle.isOn = true;
            return toggle;
        }
    }
}
```

---

## Assembly Definition Files

### NinjaFruit.Runtime.asmdef (Update)
**File:** `Assets/Scripts/NinjaFruit.Runtime.asmdef`

```json
{
  "name": "NinjaFruit.Runtime",
  "rootNamespace": "NinjaFruit",
  "references": [
    "Unity.TextMeshPro",
    "Unity.InputSystem",
    "Unity.UI"
  ],
  "includePlatforms": [],
  "excludePlatforms": [],
  "allowUnsafeCode": false,
  "overrideReferences": false,
  "precompiledReferences": [],
  "autoReferenced": true,
  "defineConstraints": [],
  "versionDefines": [],
  "noEngineReferences": false
}
```

---

## Build & Run Instructions

### Step 1: Create Folder Structure
```powershell
# From ninja-fruit/Assets/ directory
New-Item -ItemType Directory -Path "Scripts/UI" -Force
New-Item -ItemType Directory -Path "Scripts/Interfaces" -Force
New-Item -ItemType Directory -Path "Tests/EditMode/UI" -Force
New-Item -ItemType Directory -Path "Tests/PlayMode/UI" -Force
New-Item -ItemType Directory -Path "Tests/Mocks" -Force
```

### Step 2: Copy Stub Files
1. Copy all production code stubs to `Assets/Scripts/`
2. Copy all test files to `Assets/Tests/`
3. Refresh Unity project (Ctrl+R)

### Step 3: Verify Compilation
```
Unity Editor → Console
Expected: 0 errors, 0 warnings
```

### Step 4: Run Tests (Should Fail - RED Phase)
```
Unity Editor → Window → Test Runner
EditMode Tab → Run All (expect 8 failures)
PlayMode Tab → Run All (expect 10 failures)
```

### Step 5: Implement Production Code (GREEN Phase)
Now implement the `TODO` sections in:
- HighScoreManager.cs
- SettingsManager.cs
- SceneTransitionManager.cs
- MainMenuController.cs

### Step 6: Run Tests Again (Should Pass)
```
Unity Editor → Window → Test Runner
EditMode Tab → Run All (expect 8 passes ✅)
PlayMode Tab → Run All (expect 10 passes ✅)
```

---

## Success Criteria

- [ ] All stub files compile without errors
- [ ] All 18 tests execute (even if failing in RED phase)
- [ ] Test Runner shows 18 total tests
- [ ] No missing assembly references
- [ ] Production code stubs are minimal but complete
- [ ] Mock objects work in tests

---

## Next Steps

1. **RED Phase:** Verify all 18 tests fail appropriately
2. **GREEN Phase:** Implement production code to pass tests
3. **REFACTOR Phase:** Clean up code while keeping tests passing
4. **Documentation:** Add XML comments to all public methods

---

**Status:** READY FOR TDD WORKFLOW  
**Expected Time:** 4 hours (1hr RED + 2hrs GREEN + 1hr REFACTOR)

---

**Approval:**
- Created By: Test Design Agent
- Reviewed By: _____________
- Date: _____________
