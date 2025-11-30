# STORY-011: Main Menu & Navigation

**Story ID:** STORY-011  
**Epic:** EPIC-004 - User Interface & Game Flow  
**Story Name:** Main Menu & Navigation  
**Status:** Ready for Development  
**Points:** 3  
**Priority:** High  
**Owner:** Dev Team  
**Created:** November 30, 2025  
**TDD Approach:** Test-First Development

---

## User Story

**As a** player  
**I want** a main menu with clear navigation options  
**So that** I can start playing, view my high scores, adjust settings, or quit the game

---

## Acceptance Criteria

### AC1: Main Menu Display
- **Given** the game launches or player returns from game over screen
- **When** the main menu is displayed
- **Then** the following UI elements should be visible and correctly positioned:
  - Game title/logo
  - PLAY button (primary action, highlighted)
  - HIGH SCORES button
  - SETTINGS button
  - QUIT button (PC only, hidden on WebGL)

### AC2: Play Button Functionality
- **Given** player is on main menu
- **When** player clicks/taps the PLAY button
- **Then** the game should:
  - Trigger scene transition to gameplay scene
  - Initialize game state (score=0, lives=3, combo=1x)
  - Hide main menu UI
  - Start fruit spawning

### AC3: High Scores Button Functionality
- **Given** player is on main menu
- **When** player clicks/taps HIGH SCORES button
- **Then** the high scores panel should display:
  - Highest score achieved
  - Total fruits sliced (lifetime)
  - Longest combo achieved
  - BACK button to return to main menu

### AC4: Settings Button Functionality
- **Given** player is on main menu
- **When** player clicks/taps SETTINGS button
- **Then** the settings panel should display:
  - Master Volume slider (0-100%)
  - Sound Effects toggle (On/Off)
  - Music toggle (On/Off)
  - BACK button to return to main menu

### AC5: Quit Button Functionality (Platform-Specific)
- **Given** player is on main menu on PC platform
- **When** player clicks QUIT button
- **Then** the application should close gracefully
- **And Given** player is on WebGL platform
- **Then** QUIT button should not be visible

### AC6: Menu Navigation State Management
- **Given** player opens high scores or settings panel
- **When** player clicks BACK button
- **Then** the panel should close and main menu buttons should reappear
- **And** the menu state should return to initial state

---

## Technical Specifications

### Components to Implement

#### 1. MainMenuController.cs
```csharp
public class MainMenuController : MonoBehaviour
{
    // UI References
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject highScoresPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button highScoresButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    
    // Dependencies
    private GameStateController gameStateController;
    private HighScoreManager highScoreManager;
    private SettingsManager settingsManager;
    
    // Methods
    public void Initialize();
    public void ShowMainMenu();
    public void ShowHighScores();
    public void ShowSettings();
    public void OnPlayClicked();
    public void OnHighScoresClicked();
    public void OnSettingsClicked();
    public void OnQuitClicked();
    public void OnBackClicked();
}
```

#### 2. HighScoreManager.cs (Data Layer)
```csharp
public class HighScoreManager : MonoBehaviour
{
    // Properties
    public int HighScore { get; private set; }
    public int TotalFruitsSliced { get; private set; }
    public int LongestCombo { get; private set; }
    
    // Methods
    public void LoadScores();
    public void SaveHighScore(int score);
    public void SaveFruitCount(int count);
    public void SaveCombo(int combo);
    public void ResetScores(); // For testing
}
```

#### 3. SettingsManager.cs (Data Layer)
```csharp
public class SettingsManager : MonoBehaviour
{
    // Properties
    public float MasterVolume { get; private set; }
    public bool SoundEffectsEnabled { get; private set; }
    public bool MusicEnabled { get; private set; }
    
    // Events
    public event Action<float> OnMasterVolumeChanged;
    public event Action<bool> OnSoundEffectsToggled;
    public event Action<bool> OnMusicToggled;
    
    // Methods
    public void LoadSettings();
    public void SaveSettings();
    public void SetMasterVolume(float volume);
    public void SetSoundEffects(bool enabled);
    public void SetMusic(bool enabled);
}
```

#### 4. SceneTransitionManager.cs
```csharp
public class SceneTransitionManager : MonoBehaviour
{
    // Methods
    public void LoadGameplayScene();
    public void LoadMainMenuScene();
    public void QuitApplication();
}
```

### UI Hierarchy (Unity Scene)
```
Canvas (MainMenuCanvas)
├── MainMenuPanel
│   ├── TitleText (TextMeshProUGUI)
│   ├── PlayButton
│   ├── HighScoresButton
│   ├── SettingsButton
│   └── QuitButton (Active if UNITY_STANDALONE)
├── HighScoresPanel (Initially inactive)
│   ├── TitleText
│   ├── HighScoreText
│   ├── TotalFruitsText
│   ├── LongestComboText
│   └── BackButton
└── SettingsPanel (Initially inactive)
    ├── TitleText
    ├── MasterVolumeSlider
    ├── SoundEffectsToggle
    ├── MusicToggle
    └── BackButton
```

### Data Persistence (PlayerPrefs)
```csharp
// Keys
private const string HIGH_SCORE_KEY = "HighScore";
private const string TOTAL_FRUITS_KEY = "TotalFruitsSliced";
private const string LONGEST_COMBO_KEY = "LongestCombo";
private const string MASTER_VOLUME_KEY = "MasterVolume";
private const string SOUND_FX_KEY = "SoundEffectsEnabled";
private const string MUSIC_KEY = "MusicEnabled";

// Defaults
private const int DEFAULT_HIGH_SCORE = 0;
private const int DEFAULT_TOTAL_FRUITS = 0;
private const int DEFAULT_LONGEST_COMBO = 0;
private const float DEFAULT_MASTER_VOLUME = 0.8f;
private const bool DEFAULT_SOUND_FX = true;
private const bool DEFAULT_MUSIC = true;
```

### Scene Management
- **MainMenu Scene:** Contains MainMenuCanvas and managers
- **Gameplay Scene:** Contains game logic (already implemented)
- **Transition Method:** `SceneManager.LoadScene("SceneName", LoadSceneMode.Single)`

---

## Testing Strategy

### Test Breakdown
- **Edit Mode Tests:** 8 tests (data layer logic)
- **Play Mode Tests:** 10 tests (UI integration)
- **Total:** 18 tests

### Test Categories

#### 1. Edit Mode Tests (Data Layer)
**File:** `Assets/Tests/EditMode/UI/HighScoreManagerTests.cs`
- ✅ High score loads default value (0) on first launch
- ✅ High score saves and loads correctly
- ✅ High score only updates if new score is higher
- ✅ Total fruits count accumulates across sessions

**File:** `Assets/Tests/EditMode/UI/SettingsManagerTests.cs`
- ✅ Settings load default values on first launch
- ✅ Master volume saves and loads correctly (0.0-1.0 range)
- ✅ Sound effects toggle saves and loads correctly
- ✅ Music toggle saves and loads correctly

#### 2. Play Mode Tests (UI Integration)
**File:** `Assets/Tests/PlayMode/UI/MainMenuControllerTests.cs`
- ✅ Main menu displays all buttons on initialization
- ✅ Play button triggers gameplay scene load
- ✅ High scores button shows high scores panel
- ✅ Settings button shows settings panel
- ✅ Quit button closes application (PC build only)
- ✅ Back button returns to main menu from high scores
- ✅ Back button returns to main menu from settings
- ✅ High scores panel displays correct data
- ✅ Settings panel reflects current settings
- ✅ Settings changes trigger events

---

## Dependencies

### Story Dependencies
- ✅ **STORY-010** (HUD Display) - provides UI patterns and test utilities
- ✅ **STORY-004** (ScoreManager) - provides score data for high scores
- ✅ **STORY-003** (GameStateController) - provides game state management

### Package Dependencies
- Unity TextMeshPro (UI text)
- Unity UI package (Canvas, Button, Slider, Toggle)
- Unity SceneManagement (scene transitions)

### Assembly References
```json
// NinjaFruit.Runtime.asmdef
{
  "references": [
    "Unity.TextMeshPro",
    "Unity.UI"
  ]
}

// NinjaFruit.PlayMode.Tests.asmdef
{
  "references": [
    "NinjaFruit.Runtime",
    "NinjaFruit.Tests.Setup",
    "Unity.TextMeshPro",
    "Unity.UI"
  ]
}
```

---

## Implementation Plan (TDD Flow)

### Phase 1: RED (Write Failing Tests) - 1 hour
1. Create `HighScoreManagerTests.cs` with 4 Edit Mode tests
2. Create `SettingsManagerTests.cs` with 4 Edit Mode tests
3. Create `MainMenuControllerTests.cs` with 10 Play Mode tests
4. Run all tests → **Expect 18 failures** ❌

### Phase 2: GREEN (Implement Minimal Code) - 2 hours
1. Implement `HighScoreManager.cs` (data persistence)
2. Implement `SettingsManager.cs` (settings persistence)
3. Implement `SceneTransitionManager.cs` (scene loading)
4. Implement `MainMenuController.cs` (UI logic)
5. Create MainMenu scene with UI hierarchy
6. Wire up SerializeField references
7. Run all tests → **Expect 18 passes** ✅

### Phase 3: REFACTOR (Clean & Optimize) - 45 minutes
1. Extract common PlayerPrefs logic to utility class
2. Add XML documentation comments
3. Add null checks and error handling
4. Optimize button click event subscriptions
5. Run tests → **Still 18 passes** ✅

### Phase 4: EDGE CASES (Additional Tests) - 30 minutes
1. Test corrupted PlayerPrefs data
2. Test negative volume values
3. Test rapid button clicking
4. Test scene load failure handling
5. Run tests → **All pass** ✅

---

## Definition of Done

- [ ] All 18 tests written and passing (100% pass rate)
- [ ] MainMenuController.cs implemented and documented
- [ ] HighScoreManager.cs implemented with PlayerPrefs
- [ ] SettingsManager.cs implemented with PlayerPrefs
- [ ] SceneTransitionManager.cs implemented
- [ ] MainMenu scene created in Unity
- [ ] All UI elements wired to controllers
- [ ] Manual QA: Can navigate all menu panels smoothly
- [ ] Manual QA: Scene transitions work (MainMenu → Gameplay)
- [ ] Manual QA: Settings persist after app restart
- [ ] Code reviewed and approved
- [ ] No compiler warnings or errors
- [ ] Test coverage report shows 80%+ coverage

---

## Risks & Mitigation

| Risk | Impact | Likelihood | Mitigation |
|------|--------|------------|------------|
| Scene loading fails in tests | High | Medium | Use SceneManager.LoadSceneAsync with callbacks |
| PlayerPrefs not available in tests | Medium | Low | Use mock/stub for PlayerPrefs in Edit Mode tests |
| UI elements not clickable in tests | Medium | Medium | Use `button.onClick.Invoke()` instead of mouse simulation |
| Platform-specific code (Quit button) | Low | Medium | Use `#if UNITY_STANDALONE` preprocessor directives |

---

## Test Plan Reference

See detailed test specifications in:
- `docs/test-plans/test-plan-story-011-main-menu.md`
- `docs/test-specs/test-spec-story-011-main-menu.md`
- `docs/test-scaffolding/test-scaffolding-story-011-main-menu.md`

---

## Related Documentation

- **Epic:** `docs/epics/epic-ui-game-flow.md`
- **GDD:** `docs/GDD.md` (UI Navigation section)
- **Architecture:** `docs/game-architecture.md` (UI Components)
- **Story 010:** `docs/stories/story-010-hud-display.md` (UI patterns)

---

## Notes

**Design Patterns:**
- **Singleton Pattern:** MenuController accessible globally via `MainMenuController.Instance`
- **Event-Driven:** Settings changes trigger events for audio system to react
- **Dependency Injection:** Managers injected into controllers for testability

**TDD Best Practices:**
- Write tests for **behavior**, not implementation details
- Test **state changes**, not internal method calls
- Use **Play Mode tests** for UI verification
- Use **Edit Mode tests** for pure logic (data persistence)

**Platform Considerations:**
- Quit button visible only on PC (`UNITY_STANDALONE`)
- WebGL builds ignore Application.Quit() calls
- Mobile builds may need back button handling (future story)

---

**Status:** READY FOR DEVELOPMENT  
**Next Step:** Phase 1 - Write 18 failing tests  
**Estimated Completion:** 4 hours with TDD approach

---

**Approval:**
- Created By: Test Design Agent
- Reviewed By: _____________
- Approved By: _____________
- Date: _____________
