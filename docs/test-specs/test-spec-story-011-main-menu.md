# Test Specification: STORY-011 Main Menu & Navigation

**Story:** STORY-011 - Main Menu & Navigation  
**Epic:** EPIC-004 - User Interface & Game Flow  
**Test Spec Version:** 1.0  
**Author:** Test Design Agent  
**Date:** November 30, 2025

---

## Test Suite Organization

### Edit Mode Tests (Data Layer)

#### Suite 1: HighScoreManagerTests
**File:** `Assets/Tests/EditMode/UI/HighScoreManagerTests.cs`  
**Assembly:** NinjaFruit.EditMode.Tests  
**Test Count:** 4  
**Purpose:** Verify high score persistence logic

**Test Methods:**
```csharp
[Test] HighScore_LoadsDefaultOnFirstLaunch_ReturnsZero()
[Test] HighScore_SavesAndLoadsCorrectly_PersistsValue()
[Test] HighScore_OnlyUpdatesIfHigher_IgnoresLowerScores()
[Test] TotalFruitsSliced_Accumulates_AddsToPrevious()
```

---

#### Suite 2: SettingsManagerTests
**File:** `Assets/Tests/EditMode/UI/SettingsManagerTests.cs`  
**Assembly:** NinjaFruit.EditMode.Tests  
**Test Count:** 4  
**Purpose:** Verify settings persistence logic

**Test Methods:**
```csharp
[Test] Settings_LoadDefaultValues_ReturnsExpectedDefaults()
[Test] MasterVolume_SavesAndLoads_PersistsCorrectly()
[Test] SoundEffectsToggle_SavesAndLoads_PersistsState()
[Test] MusicToggle_SavesAndLoads_PersistsState()
```

---

### Play Mode Tests (UI Integration)

#### Suite 3: MainMenuControllerTests
**File:** `Assets/Tests/PlayMode/UI/MainMenuControllerTests.cs`  
**Assembly:** NinjaFruit.PlayMode.Tests  
**Test Count:** 10  
**Purpose:** Verify main menu UI behavior and navigation

**Test Methods:**
```csharp
[UnityTest] MainMenu_Initialize_DisplaysAllButtons()
[UnityTest] PlayButton_Clicked_LoadsGameplayScene()
[UnityTest] HighScoresButton_Clicked_ShowsHighScoresPanel()
[UnityTest] SettingsButton_Clicked_ShowsSettingsPanel()
[UnityTest] QuitButton_Clicked_CallsApplicationQuit()
[UnityTest] BackButton_FromHighScores_ReturnsToMainMenu()
[UnityTest] BackButton_FromSettings_ReturnsToMainMenu()
[UnityTest] HighScoresPanel_DisplaysCorrectData()
[UnityTest] SettingsPanel_ReflectsCurrentSettings()
[UnityTest] SettingsChanges_TriggerEvents()
```

---

## Detailed Test Specifications

### Edit Mode Tests

---

#### TS-011-001: High Score Default Value
**Test Class:** HighScoreManagerTests  
**Test Method:** `HighScore_LoadsDefaultOnFirstLaunch_ReturnsZero()`  
**Type:** Unit Test (Edit Mode)  
**Priority:** High

**Preconditions:**
- PlayerPrefs cleared (simulating first app launch)

**Test Steps:**
```csharp
1. PlayerPrefs.DeleteAll()
2. var manager = new GameObject().AddComponent<HighScoreManager>()
3. manager.LoadScores()
4. int result = manager.HighScore
```

**Expected Results:**
- `result == 0`
- No exceptions thrown
- PlayerPrefs key created with value 0

**Assertions:**
```csharp
Assert.AreEqual(0, manager.HighScore);
Assert.AreEqual(0, manager.TotalFruitsSliced);
Assert.AreEqual(0, manager.LongestCombo);
```

**Cleanup:**
```csharp
Object.DestroyImmediate(manager.gameObject);
PlayerPrefs.DeleteAll();
```

---

#### TS-011-002: High Score Persistence
**Test Class:** HighScoreManagerTests  
**Test Method:** `HighScore_SavesAndLoadsCorrectly_PersistsValue()`  
**Type:** Unit Test (Edit Mode)  
**Priority:** Critical

**Preconditions:**
- PlayerPrefs cleared

**Test Steps:**
```csharp
1. var manager1 = new GameObject().AddComponent<HighScoreManager>()
2. manager1.SaveHighScore(1250)
3. Object.DestroyImmediate(manager1.gameObject)
4. var manager2 = new GameObject().AddComponent<HighScoreManager>()
5. manager2.LoadScores()
6. int result = manager2.HighScore
```

**Expected Results:**
- `result == 1250`
- PlayerPrefs contains "HighScore" key with value 1250
- Data persists across manager instances

**Assertions:**
```csharp
Assert.AreEqual(1250, manager2.HighScore);
Assert.AreEqual(1250, PlayerPrefs.GetInt("HighScore"));
```

**Cleanup:**
```csharp
Object.DestroyImmediate(manager2.gameObject);
PlayerPrefs.DeleteAll();
```

---

#### TS-011-003: High Score Only Updates Upward
**Test Class:** HighScoreManagerTests  
**Test Method:** `HighScore_OnlyUpdatesIfHigher_IgnoresLowerScores()`  
**Type:** Unit Test (Edit Mode)  
**Priority:** High

**Preconditions:**
- PlayerPrefs cleared

**Test Steps:**
```csharp
1. var manager = new GameObject().AddComponent<HighScoreManager>()
2. manager.SaveHighScore(1000)
3. manager.SaveHighScore(500)  // Lower score
4. manager.LoadScores()
5. int result = manager.HighScore
```

**Expected Results:**
- `result == 1000` (not 500)
- High score unchanged by lower score attempt

**Assertions:**
```csharp
Assert.AreEqual(1000, manager.HighScore);
Assert.AreEqual(1000, PlayerPrefs.GetInt("HighScore"));
```

**Edge Cases:**
- Equal score: SaveHighScore(1000) twice → result = 1000
- Zero score: SaveHighScore(0) → should still update if previous was negative

**Cleanup:**
```csharp
Object.DestroyImmediate(manager.gameObject);
PlayerPrefs.DeleteAll();
```

---

#### TS-011-004: Total Fruits Accumulation
**Test Class:** HighScoreManagerTests  
**Test Method:** `TotalFruitsSliced_Accumulates_AddsToPrevious()`  
**Type:** Unit Test (Edit Mode)  
**Priority:** Medium

**Preconditions:**
- PlayerPrefs cleared

**Test Steps:**
```csharp
1. var manager = new GameObject().AddComponent<HighScoreManager>()
2. manager.SaveFruitCount(50)   // First session
3. manager.SaveFruitCount(75)   // Second session (+25 more)
4. manager.LoadScores()
5. int result = manager.TotalFruitsSliced
```

**Expected Results:**
- `result == 125` (50 + 75, not 75)
- Fruit count is cumulative, not replaced

**Assertions:**
```csharp
Assert.AreEqual(125, manager.TotalFruitsSliced);
Assert.AreEqual(125, PlayerPrefs.GetInt("TotalFruitsSliced"));
```

**Cleanup:**
```csharp
Object.DestroyImmediate(manager.gameObject);
PlayerPrefs.DeleteAll();
```

---

#### TS-011-005: Settings Default Values
**Test Class:** SettingsManagerTests  
**Test Method:** `Settings_LoadDefaultValues_ReturnsExpectedDefaults()`  
**Type:** Unit Test (Edit Mode)  
**Priority:** High

**Preconditions:**
- PlayerPrefs cleared

**Test Steps:**
```csharp
1. PlayerPrefs.DeleteAll()
2. var manager = new GameObject().AddComponent<SettingsManager>()
3. manager.LoadSettings()
```

**Expected Results:**
- `manager.MasterVolume == 0.8f`
- `manager.SoundEffectsEnabled == true`
- `manager.MusicEnabled == true`

**Assertions:**
```csharp
Assert.AreEqual(0.8f, manager.MasterVolume, 0.01f);
Assert.IsTrue(manager.SoundEffectsEnabled);
Assert.IsTrue(manager.MusicEnabled);
```

**Cleanup:**
```csharp
Object.DestroyImmediate(manager.gameObject);
PlayerPrefs.DeleteAll();
```

---

#### TS-011-006: Master Volume Persistence
**Test Class:** SettingsManagerTests  
**Test Method:** `MasterVolume_SavesAndLoads_PersistsCorrectly()`  
**Type:** Unit Test (Edit Mode)  
**Priority:** High

**Preconditions:**
- PlayerPrefs cleared

**Test Steps:**
```csharp
1. var manager1 = new GameObject().AddComponent<SettingsManager>()
2. manager1.SetMasterVolume(0.5f)
3. manager1.SaveSettings()
4. Object.DestroyImmediate(manager1.gameObject)
5. var manager2 = new GameObject().AddComponent<SettingsManager>()
6. manager2.LoadSettings()
```

**Expected Results:**
- `manager2.MasterVolume == 0.5f`
- Volume setting persists across instances

**Assertions:**
```csharp
Assert.AreEqual(0.5f, manager2.MasterVolume, 0.01f);
Assert.AreEqual(0.5f, PlayerPrefs.GetFloat("MasterVolume"), 0.01f);
```

**Edge Cases:**
- Volume = 0.0f (mute)
- Volume = 1.0f (max)
- Volume < 0.0f → should clamp to 0.0f
- Volume > 1.0f → should clamp to 1.0f

**Cleanup:**
```csharp
Object.DestroyImmediate(manager2.gameObject);
PlayerPrefs.DeleteAll();
```

---

#### TS-011-007: Sound Effects Toggle Persistence
**Test Class:** SettingsManagerTests  
**Test Method:** `SoundEffectsToggle_SavesAndLoads_PersistsState()`  
**Type:** Unit Test (Edit Mode)  
**Priority:** Medium

**Preconditions:**
- PlayerPrefs cleared

**Test Steps:**
```csharp
1. var manager1 = new GameObject().AddComponent<SettingsManager>()
2. manager1.SetSoundEffects(false)
3. manager1.SaveSettings()
4. Object.DestroyImmediate(manager1.gameObject)
5. var manager2 = new GameObject().AddComponent<SettingsManager>()
6. manager2.LoadSettings()
```

**Expected Results:**
- `manager2.SoundEffectsEnabled == false`
- Toggle state persists

**Assertions:**
```csharp
Assert.IsFalse(manager2.SoundEffectsEnabled);
Assert.AreEqual(0, PlayerPrefs.GetInt("SoundEffectsEnabled")); // 0 = false
```

**Cleanup:**
```csharp
Object.DestroyImmediate(manager2.gameObject);
PlayerPrefs.DeleteAll();
```

---

#### TS-011-008: Music Toggle Persistence
**Test Class:** SettingsManagerTests  
**Test Method:** `MusicToggle_SavesAndLoads_PersistsState()`  
**Type:** Unit Test (Edit Mode)  
**Priority:** Medium

**Preconditions:**
- PlayerPrefs cleared

**Test Steps:**
```csharp
1. var manager1 = new GameObject().AddComponent<SettingsManager>()
2. manager1.SetMusic(false)
3. manager1.SaveSettings()
4. Object.DestroyImmediate(manager1.gameObject)
5. var manager2 = new GameObject().AddComponent<SettingsManager>()
6. manager2.LoadSettings()
```

**Expected Results:**
- `manager2.MusicEnabled == false`
- Toggle state persists

**Assertions:**
```csharp
Assert.IsFalse(manager2.MusicEnabled);
Assert.AreEqual(0, PlayerPrefs.GetInt("MusicEnabled")); // 0 = false
```

**Cleanup:**
```csharp
Object.DestroyImmediate(manager2.gameObject);
PlayerPrefs.DeleteAll();
```

---

### Play Mode Tests

---

#### TS-011-009: Main Menu Button Visibility
**Test Class:** MainMenuControllerTests  
**Test Method:** `MainMenu_Initialize_DisplaysAllButtons()`  
**Type:** Integration Test (Play Mode)  
**Priority:** Critical

**Preconditions:**
- Test scene with Canvas created
- MainMenuController prefab instantiated

**Test Steps:**
```csharp
1. var testCanvas = UITestHelpers.CreateTestCanvas()
2. var menu = CreateMainMenuController(testCanvas)
3. menu.Initialize()
4. yield return null  // Wait one frame
```

**Expected Results:**
- Play button visible
- High Scores button visible
- Settings button visible
- Quit button visible if UNITY_STANDALONE, hidden otherwise

**Assertions:**
```csharp
Assert.IsNotNull(menu.playButton);
Assert.IsTrue(menu.playButton.gameObject.activeSelf);
Assert.IsNotNull(menu.highScoresButton);
Assert.IsTrue(menu.highScoresButton.gameObject.activeSelf);

#if UNITY_STANDALONE
Assert.IsTrue(menu.quitButton.gameObject.activeSelf);
#else
Assert.IsFalse(menu.quitButton.gameObject.activeSelf);
#endif
```

**Cleanup:**
```csharp
Object.Destroy(menu.gameObject);
Object.Destroy(testCanvas.gameObject);
```

---

#### TS-011-010: Play Button Scene Transition
**Test Class:** MainMenuControllerTests  
**Test Method:** `PlayButton_Clicked_LoadsGameplayScene()`  
**Type:** Integration Test (Play Mode)  
**Priority:** Critical

**Preconditions:**
- MainMenuController with mocked SceneTransitionManager

**Test Steps:**
```csharp
1. var menu = CreateMainMenuController()
2. var mockSceneManager = new MockSceneTransitionManager()
3. menu.SetSceneManager(mockSceneManager)
4. menu.playButton.onClick.Invoke()
5. yield return null
```

**Expected Results:**
- `mockSceneManager.LoadGameplaySceneCalled == true`
- Scene transition method invoked

**Assertions:**
```csharp
Assert.IsTrue(mockSceneManager.LoadGameplaySceneCalled);
Assert.AreEqual("Gameplay", mockSceneManager.LastSceneLoaded);
```

**Mock Implementation:**
```csharp
public class MockSceneTransitionManager : ISceneTransitionManager
{
    public bool LoadGameplaySceneCalled { get; private set; }
    public string LastSceneLoaded { get; private set; }
    
    public void LoadGameplayScene()
    {
        LoadGameplaySceneCalled = true;
        LastSceneLoaded = "Gameplay";
    }
}
```

**Cleanup:**
```csharp
Object.Destroy(menu.gameObject);
```

---

#### TS-011-011: High Scores Panel Navigation
**Test Class:** MainMenuControllerTests  
**Test Method:** `HighScoresButton_Clicked_ShowsHighScoresPanel()`  
**Type:** Integration Test (Play Mode)  
**Priority:** High

**Preconditions:**
- MainMenuController fully initialized

**Test Steps:**
```csharp
1. var menu = CreateMainMenuController()
2. menu.Initialize()
3. yield return null
4. Assert.IsTrue(menu.mainMenuPanel.activeSelf)  // Initially visible
5. menu.highScoresButton.onClick.Invoke()
6. yield return null
```

**Expected Results:**
- High scores panel visible
- Main menu panel hidden
- Panel transition smooth

**Assertions:**
```csharp
Assert.IsTrue(menu.highScoresPanel.activeSelf);
Assert.IsFalse(menu.mainMenuPanel.activeSelf);
```

**Cleanup:**
```csharp
Object.Destroy(menu.gameObject);
```

---

#### TS-011-012: Settings Panel Navigation
**Test Class:** MainMenuControllerTests  
**Test Method:** `SettingsButton_Clicked_ShowsSettingsPanel()`  
**Type:** Integration Test (Play Mode)  
**Priority:** High

**Preconditions:**
- MainMenuController fully initialized

**Test Steps:**
```csharp
1. var menu = CreateMainMenuController()
2. menu.Initialize()
3. yield return null
4. menu.settingsButton.onClick.Invoke()
5. yield return null
```

**Expected Results:**
- Settings panel visible
- Main menu panel hidden

**Assertions:**
```csharp
Assert.IsTrue(menu.settingsPanel.activeSelf);
Assert.IsFalse(menu.mainMenuPanel.activeSelf);
```

**Cleanup:**
```csharp
Object.Destroy(menu.gameObject);
```

---

#### TS-011-013: Quit Application
**Test Class:** MainMenuControllerTests  
**Test Method:** `QuitButton_Clicked_CallsApplicationQuit()`  
**Type:** Integration Test (Play Mode)  
**Priority:** Medium

**Preconditions:**
- MainMenuController with mocked SceneTransitionManager

**Test Steps:**
```csharp
1. var menu = CreateMainMenuController()
2. var mockSceneManager = new MockSceneTransitionManager()
3. menu.SetSceneManager(mockSceneManager)
4. menu.quitButton.onClick.Invoke()
5. yield return null
```

**Expected Results:**
- `mockSceneManager.QuitApplicationCalled == true`

**Assertions:**
```csharp
Assert.IsTrue(mockSceneManager.QuitApplicationCalled);
```

**Note:** Actual `Application.Quit()` only works in builds, not in editor

**Cleanup:**
```csharp
Object.Destroy(menu.gameObject);
```

---

#### TS-011-014: Back Navigation from High Scores
**Test Class:** MainMenuControllerTests  
**Test Method:** `BackButton_FromHighScores_ReturnsToMainMenu()`  
**Type:** Integration Test (Play Mode)  
**Priority:** High

**Preconditions:**
- MainMenuController navigated to high scores panel

**Test Steps:**
```csharp
1. var menu = CreateMainMenuController()
2. menu.Initialize()
3. menu.OnHighScoresClicked()
4. yield return null
5. Assert.IsTrue(menu.highScoresPanel.activeSelf)
6. menu.OnBackClicked()
7. yield return null
```

**Expected Results:**
- Main menu panel visible
- High scores panel hidden
- State reset to initial

**Assertions:**
```csharp
Assert.IsTrue(menu.mainMenuPanel.activeSelf);
Assert.IsFalse(menu.highScoresPanel.activeSelf);
Assert.IsFalse(menu.settingsPanel.activeSelf);
```

**Cleanup:**
```csharp
Object.Destroy(menu.gameObject);
```

---

#### TS-011-015: Back Navigation from Settings
**Test Class:** MainMenuControllerTests  
**Test Method:** `BackButton_FromSettings_ReturnsToMainMenu()`  
**Type:** Integration Test (Play Mode)  
**Priority:** High

**Preconditions:**
- MainMenuController navigated to settings panel

**Test Steps:**
```csharp
1. var menu = CreateMainMenuController()
2. menu.Initialize()
3. menu.OnSettingsClicked()
4. yield return null
5. Assert.IsTrue(menu.settingsPanel.activeSelf)
6. menu.OnBackClicked()
7. yield return null
```

**Expected Results:**
- Main menu panel visible
- Settings panel hidden

**Assertions:**
```csharp
Assert.IsTrue(menu.mainMenuPanel.activeSelf);
Assert.IsFalse(menu.settingsPanel.activeSelf);
Assert.IsFalse(menu.highScoresPanel.activeSelf);
```

**Cleanup:**
```csharp
Object.Destroy(menu.gameObject);
```

---

#### TS-011-016: High Scores Data Display
**Test Class:** MainMenuControllerTests  
**Test Method:** `HighScoresPanel_DisplaysCorrectData()`  
**Type:** Integration Test (Play Mode)  
**Priority:** High

**Preconditions:**
- HighScoreManager with pre-set values

**Test Steps:**
```csharp
1. var highScoreManager = CreateHighScoreManager()
2. highScoreManager.SaveHighScore(1250)
3. highScoreManager.SaveFruitCount(50)
4. highScoreManager.SaveCombo(5)
5. var menu = CreateMainMenuController()
6. menu.SetHighScoreManager(highScoreManager)
7. menu.OnHighScoresClicked()
8. yield return null
```

**Expected Results:**
- High score text shows "1250"
- Total fruits text shows "50"
- Longest combo text shows "5x"

**Assertions:**
```csharp
Assert.AreEqual("1250", menu.highScoreText.text);
Assert.AreEqual("50", menu.totalFruitsText.text);
Assert.AreEqual("5x", menu.longestComboText.text);
```

**Cleanup:**
```csharp
Object.Destroy(menu.gameObject);
Object.Destroy(highScoreManager.gameObject);
```

---

#### TS-011-017: Settings UI Synchronization
**Test Class:** MainMenuControllerTests  
**Test Method:** `SettingsPanel_ReflectsCurrentSettings()`  
**Type:** Integration Test (Play Mode)  
**Priority:** High

**Preconditions:**
- SettingsManager with custom values

**Test Steps:**
```csharp
1. var settingsManager = CreateSettingsManager()
2. settingsManager.SetMasterVolume(0.5f)
3. settingsManager.SetSoundEffects(false)
4. settingsManager.SetMusic(true)
5. var menu = CreateMainMenuController()
6. menu.SetSettingsManager(settingsManager)
7. menu.OnSettingsClicked()
8. yield return null
```

**Expected Results:**
- Volume slider shows 0.5
- Sound effects toggle is OFF
- Music toggle is ON

**Assertions:**
```csharp
Assert.AreEqual(0.5f, menu.masterVolumeSlider.value, 0.01f);
Assert.IsFalse(menu.soundEffectsToggle.isOn);
Assert.IsTrue(menu.musicToggle.isOn);
```

**Cleanup:**
```csharp
Object.Destroy(menu.gameObject);
Object.Destroy(settingsManager.gameObject);
```

---

#### TS-011-018: Settings Event Broadcasting
**Test Class:** MainMenuControllerTests  
**Test Method:** `SettingsChanges_TriggerEvents()`  
**Type:** Integration Test (Play Mode)  
**Priority:** Medium

**Preconditions:**
- SettingsManager with event listener

**Test Steps:**
```csharp
1. var settingsManager = CreateSettingsManager()
2. float receivedVolume = -1f
3. settingsManager.OnMasterVolumeChanged += (vol) => receivedVolume = vol
4. settingsManager.SetMasterVolume(0.7f)
5. yield return null
```

**Expected Results:**
- Event fired with correct volume value

**Assertions:**
```csharp
Assert.AreEqual(0.7f, receivedVolume, 0.01f);
```

**Additional Event Tests:**
```csharp
// Sound Effects event
bool soundFxEventFired = false;
settingsManager.OnSoundEffectsToggled += (enabled) => soundFxEventFired = true;
settingsManager.SetSoundEffects(false);
Assert.IsTrue(soundFxEventFired);

// Music event
bool musicEventFired = false;
settingsManager.OnMusicToggled += (enabled) => musicEventFired = true;
settingsManager.SetMusic(false);
Assert.IsTrue(musicEventFired);
```

**Cleanup:**
```csharp
Object.Destroy(settingsManager.gameObject);
```

---

## Test Utilities

### Helper Methods

```csharp
// MainMenuControllerTests.cs

private Canvas CreateTestCanvas()
{
    var canvasGO = new GameObject("TestCanvas");
    var canvas = canvasGO.AddComponent<Canvas>();
    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    canvasGO.AddComponent<CanvasScaler>();
    canvasGO.AddComponent<GraphicRaycaster>();
    
    var eventSystemGO = new GameObject("EventSystem");
    eventSystemGO.AddComponent<EventSystem>();
    eventSystemGO.AddComponent<InputSystemUIInputModule>();
    
    return canvas;
}

private MainMenuController CreateMainMenuController()
{
    var canvas = CreateTestCanvas();
    var menuGO = new GameObject("MainMenu");
    menuGO.transform.SetParent(canvas.transform);
    var menu = menuGO.AddComponent<MainMenuController>();
    
    // Create UI hierarchy
    menu.mainMenuPanel = CreatePanel(menuGO, "MainMenuPanel");
    menu.highScoresPanel = CreatePanel(menuGO, "HighScoresPanel");
    menu.settingsPanel = CreatePanel(menuGO, "SettingsPanel");
    
    menu.playButton = CreateButton(menu.mainMenuPanel, "PlayButton");
    menu.highScoresButton = CreateButton(menu.mainMenuPanel, "HighScoresButton");
    menu.settingsButton = CreateButton(menu.mainMenuPanel, "SettingsButton");
    menu.quitButton = CreateButton(menu.mainMenuPanel, "QuitButton");
    
    // Create text elements for high scores panel
    menu.highScoreText = CreateText(menu.highScoresPanel, "HighScoreText");
    menu.totalFruitsText = CreateText(menu.highScoresPanel, "TotalFruitsText");
    menu.longestComboText = CreateText(menu.highScoresPanel, "LongestComboText");
    
    // Create settings UI elements
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
    panelGO.AddComponent<CanvasRenderer>();
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

private HighScoreManager CreateHighScoreManager()
{
    var go = new GameObject("HighScoreManager");
    return go.AddComponent<HighScoreManager>();
}

private SettingsManager CreateSettingsManager()
{
    var go = new GameObject("SettingsManager");
    return go.AddComponent<SettingsManager>();
}
```

### Mock Objects

```csharp
// MockSceneTransitionManager.cs

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
```

---

## Assembly Configuration

### NinjaFruit.EditMode.Tests.asmdef
```json
{
  "name": "NinjaFruit.EditMode.Tests",
  "references": [
    "UnityEngine.TestRunner",
    "UnityEditor.TestRunner",
    "NinjaFruit.Runtime"
  ],
  "includePlatforms": [
    "Editor"
  ],
  "excludePlatforms": [],
  "allowUnsafeCode": false,
  "overrideReferences": true,
  "precompiledReferences": [
    "nunit.framework.dll"
  ],
  "autoReferenced": false,
  "defineConstraints": [
    "UNITY_INCLUDE_TESTS"
  ]
}
```

### NinjaFruit.PlayMode.Tests.asmdef
```json
{
  "name": "NinjaFruit.PlayMode.Tests",
  "references": [
    "UnityEngine.TestRunner",
    "NinjaFruit.Runtime",
    "NinjaFruit.Tests.Setup",
    "Unity.TextMeshPro",
    "Unity.UI"
  ],
  "includePlatforms": [],
  "excludePlatforms": [],
  "allowUnsafeCode": false,
  "overrideReferences": true,
  "precompiledReferences": [
    "nunit.framework.dll"
  ],
  "autoReferenced": false,
  "defineConstraints": [
    "UNITY_INCLUDE_TESTS"
  ],
  "optionalUnityReferences": [
    "TestAssemblies"
  ]
}
```

---

## Execution Instructions

### Running Tests in Unity Editor

1. Open Unity Test Runner: `Window → General → Test Runner`
2. Select **EditMode** tab → Click "Run All" (8 tests)
3. Select **PlayMode** tab → Click "Run All" (10 tests)
4. Verify all 18 tests pass

### Running Tests via Command Line

```powershell
# From project root
cd ninja-fruit

# Run Edit Mode tests
"C:\Program Files\Unity\Hub\Editor\6.0.0f1\Editor\Unity.exe" `
  -projectPath . `
  -runTests `
  -testPlatform editmode `
  -testResults TestResults-EditMode.xml

# Run Play Mode tests
"C:\Program Files\Unity\Hub\Editor\6.0.0f1\Editor\Unity.exe" `
  -projectPath . `
  -runTests `
  -testPlatform playmode `
  -testResults TestResults-PlayMode.xml
```

---

## Success Criteria

- [ ] All 18 tests pass (100% pass rate)
- [ ] Test execution time < 3 seconds
- [ ] No console errors or warnings
- [ ] Code coverage ≥ 80% on all managers
- [ ] Manual QA confirms UI navigation works

---

**Status:** READY FOR IMPLEMENTATION  
**Next Step:** Create test scaffolding files

---

**Approval:**
- Created By: Test Design Agent
- Reviewed By: _____________
- Date: _____________
