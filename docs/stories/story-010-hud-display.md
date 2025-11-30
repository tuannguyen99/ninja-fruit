# STORY-010: HUD Display System

**Story ID:** STORY-010  
**Story Name:** HUD Display System  
**Epic:** EPIC-004 (User Interface & Game Flow)  
**Status:** Backlog  
**Points:** 3  
**Priority:** High  
**Owner:** Dev  
**Created:** November 30, 2025

---

## User Story

**As a** player  
**I want** to see my current score, lives remaining, and combo multiplier on screen  
**So that** I can track my performance and understand my progress during gameplay

---

## Acceptance Criteria

### AC1: Score Display
**Given** the game is running  
**When** the player slices a fruit and earns points  
**Then** the score display updates immediately to show the new total score  
**And** the score is formatted as an integer (e.g., "1250", not "1250.0")

**Test:**
```csharp
[UnityTest]
public IEnumerator HUD_ScoreUpdates_WhenPointsEarned()
```

---

### AC2: Lives Display
**Given** the game starts with 3 lives  
**When** the player misses a fruit  
**Then** one life heart icon becomes empty/grayed out  
**And** the lives counter shows the correct number remaining  
**And** when lives reach 0, all hearts are empty

**Test:**
```csharp
[UnityTest]
public IEnumerator HUD_LivesDisplay_UpdatesOnMiss()
```

---

### AC3: Combo Multiplier Display
**Given** the player has an active combo  
**When** the combo multiplier increases (2x, 3x, 4x, 5x)  
**Then** the combo text displays "COMBO Nx!" where N is the multiplier  
**And** when combo multiplier is 1x (no combo), the combo text is hidden or shows nothing  
**And** when combo expires, the display fades out or disappears

**Test:**
```csharp
[UnityTest]
public IEnumerator HUD_ComboDisplay_ShowsMultiplier()
```

---

### AC4: HUD Initialization
**Given** the game scene loads  
**When** the HUD initializes  
**Then** the score starts at 0  
**And** lives display shows 3 full hearts  
**And** combo display is hidden (no active combo)  
**And** all UI elements are visible and correctly positioned

**Test:**
```csharp
[UnityTest]
public IEnumerator HUD_Initializes_WithDefaultValues()
```

---

### AC5: Event-Driven Updates
**Given** the HUD is subscribed to game events  
**When** `ScoreManager.OnScoreChanged` event fires  
**Then** the HUD updates the score display without polling  
**And** when `GameStateController.OnLivesChanged` event fires  
**Then** the HUD updates the lives display without polling  
**And** when `ScoreManager.OnComboChanged` event fires  
**Then** the HUD updates the combo display without polling

**Test:**
```csharp
[UnityTest]
public IEnumerator HUD_UpdatesViaEvents_NotPolling()
```

---

## Technical Design

### Components

**HUDController.cs**
```csharp
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace NinjaFruit.UI
{
    public class HUDController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI comboText;
        [SerializeField] private Image[] lifeHearts; // 3 heart images
        [SerializeField] private Sprite heartFull;
        [SerializeField] private Sprite heartEmpty;
        
        [Header("Animation")]
        [SerializeField] private CanvasGroup comboCanvasGroup;
        [SerializeField] private float comboFadeDuration = 0.5f;
        
        private ScoreManager scoreManager;
        private GameStateController gameStateController;
        
        private void Awake()
        {
            scoreManager = ScoreManager.Instance;
            gameStateController = GameStateController.Instance;
        }
        
        private void OnEnable()
        {
            // Subscribe to events
            scoreManager.OnScoreChanged += UpdateScoreDisplay;
            scoreManager.OnComboChanged += UpdateComboDisplay;
            gameStateController.OnLivesChanged += UpdateLivesDisplay;
        }
        
        private void OnDisable()
        {
            // Unsubscribe from events
            scoreManager.OnScoreChanged -= UpdateScoreDisplay;
            scoreManager.OnComboChanged -= UpdateComboDisplay;
            gameStateController.OnLivesChanged -= UpdateLivesDisplay;
        }
        
        public void Initialize()
        {
            UpdateScoreDisplay(0);
            UpdateLivesDisplay(3);
            UpdateComboDisplay(1); // 1x = no combo, hide display
        }
        
        private void UpdateScoreDisplay(int newScore)
        {
            scoreText.text = newScore.ToString();
        }
        
        private void UpdateComboDisplay(int multiplier)
        {
            if (multiplier <= 1)
            {
                // No combo - hide display
                comboText.gameObject.SetActive(false);
            }
            else
            {
                // Show combo
                comboText.gameObject.SetActive(true);
                comboText.text = $"COMBO {multiplier}x!";
            }
        }
        
        private void UpdateLivesDisplay(int livesRemaining)
        {
            for (int i = 0; i < lifeHearts.Length; i++)
            {
                if (i < livesRemaining)
                {
                    lifeHearts[i].sprite = heartFull;
                    lifeHearts[i].color = Color.white;
                }
                else
                {
                    lifeHearts[i].sprite = heartEmpty;
                    lifeHearts[i].color = new Color(1f, 1f, 1f, 0.3f); // Grayed out
                }
            }
        }
        
        // Public API for testing
        public string GetScoreText() => scoreText.text;
        public string GetComboText() => comboText.text;
        public bool IsComboVisible() => comboText.gameObject.activeSelf;
        public int GetVisibleHearts()
        {
            int count = 0;
            foreach (var heart in lifeHearts)
            {
                if (heart.color.a > 0.5f) count++;
            }
            return count;
        }
    }
}
```

---

### UI Hierarchy (Unity Scene)

```
Canvas (Screen Space - Overlay)
├── EventSystem
└── HUD_Container
    ├── ScorePanel (Top Center)
    │   └── ScoreText (TextMeshProUGUI) "0"
    ├── LivesPanel (Top Left)
    │   ├── HeartIcon1 (Image)
    │   ├── HeartIcon2 (Image)
    │   └── HeartIcon3 (Image)
    └── ComboPanel (Below Score)
        └── ComboText (TextMeshProUGUI) "COMBO 3x!"
```

**Canvas Settings:**
- Render Mode: Screen Space - Overlay
- Canvas Scaler: Scale With Screen Size
- Reference Resolution: 1920x1080
- Match: 0.5 (Width/Height)

---

## Test Plan

### Test Environment Setup

**TestSceneSetup.cs**
```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;

namespace NinjaFruit.Tests.Utilities
{
    public static class TestSceneSetup
    {
        public static Canvas CreateTestCanvas()
        {
            GameObject canvasObj = new GameObject("TestCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            GraphicRaycaster raycaster = canvasObj.AddComponent<GraphicRaycaster>();
            
            // Create EventSystem if not exists
            if (Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystemObj = new GameObject("EventSystem");
                eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
            
            return canvas;
        }
        
        public static HUDController CreateTestHUD(Canvas canvas)
        {
            GameObject hudObj = new GameObject("HUD");
            hudObj.transform.SetParent(canvas.transform, false);
            
            HUDController hud = hudObj.AddComponent<HUDController>();
            
            // Create score text
            GameObject scoreObj = new GameObject("ScoreText");
            scoreObj.transform.SetParent(hudObj.transform, false);
            var scoreText = scoreObj.AddComponent<TextMeshProUGUI>();
            
            // Create combo text
            GameObject comboObj = new GameObject("ComboText");
            comboObj.transform.SetParent(hudObj.transform, false);
            var comboText = comboObj.AddComponent<TextMeshProUGUI>();
            
            // Create life hearts
            Image[] hearts = new Image[3];
            for (int i = 0; i < 3; i++)
            {
                GameObject heartObj = new GameObject($"Heart{i}");
                heartObj.transform.SetParent(hudObj.transform, false);
                hearts[i] = heartObj.AddComponent<Image>();
            }
            
            // Use reflection to set private fields (for testing)
            var hudType = typeof(HUDController);
            hudType.GetField("scoreText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(hud, scoreText);
            hudType.GetField("comboText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(hud, comboText);
            hudType.GetField("lifeHearts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(hud, hearts);
            
            return hud;
        }
    }
}
```

---

### Test Implementation

**HUDControllerTests.cs**
```csharp
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NinjaFruit.UI;
using NinjaFruit.Tests.Utilities;

namespace NinjaFruit.Tests.PlayMode
{
    [TestFixture]
    public class HUDControllerTests
    {
        private Canvas testCanvas;
        private HUDController hudController;
        private ScoreManager scoreManager;
        private GameStateController gameStateController;
        
        [UnitySetUp]
        public IEnumerator Setup()
        {
            // Create test scene
            testCanvas = TestSceneSetup.CreateTestCanvas();
            hudController = TestSceneSetup.CreateTestHUD(testCanvas);
            
            // Create managers
            GameObject scoreObj = new GameObject("ScoreManager");
            scoreManager = scoreObj.AddComponent<ScoreManager>();
            
            GameObject gameObj = new GameObject("GameStateController");
            gameStateController = gameObj.AddComponent<GameStateController>();
            
            yield return null; // Wait one frame for initialization
        }
        
        [UnityTearDown]
        public IEnumerator Teardown()
        {
            Object.Destroy(testCanvas.gameObject);
            Object.Destroy(scoreManager.gameObject);
            Object.Destroy(gameStateController.gameObject);
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator AC1_HUD_ScoreUpdates_WhenPointsEarned()
        {
            // Arrange
            hudController.Initialize();
            Assert.AreEqual("0", hudController.GetScoreText());
            
            // Act
            scoreManager.AddScore(100);
            yield return null;
            
            // Assert
            Assert.AreEqual("100", hudController.GetScoreText());
        }
        
        [UnityTest]
        public IEnumerator AC2_HUD_LivesDisplay_UpdatesOnMiss()
        {
            // Arrange
            hudController.Initialize();
            Assert.AreEqual(3, hudController.GetVisibleHearts());
            
            // Act - Simulate missed fruit
            gameStateController.RegisterMissedFruit();
            yield return null;
            
            // Assert
            Assert.AreEqual(2, hudController.GetVisibleHearts());
            
            // Act - Miss two more
            gameStateController.RegisterMissedFruit();
            gameStateController.RegisterMissedFruit();
            yield return null;
            
            // Assert
            Assert.AreEqual(0, hudController.GetVisibleHearts());
        }
        
        [UnityTest]
        public IEnumerator AC3_HUD_ComboDisplay_ShowsMultiplier()
        {
            // Arrange
            hudController.Initialize();
            Assert.IsFalse(hudController.IsComboVisible(), "Combo should be hidden initially");
            
            // Act - Trigger combo (2x)
            scoreManager.RegisterSlice(FruitType.Apple);
            scoreManager.RegisterSlice(FruitType.Banana);
            yield return null;
            
            // Assert
            Assert.IsTrue(hudController.IsComboVisible(), "Combo should be visible at 2x");
            Assert.AreEqual("COMBO 2x!", hudController.GetComboText());
        }
        
        [UnityTest]
        public IEnumerator AC4_HUD_Initializes_WithDefaultValues()
        {
            // Act
            hudController.Initialize();
            yield return null;
            
            // Assert
            Assert.AreEqual("0", hudController.GetScoreText(), "Score should start at 0");
            Assert.AreEqual(3, hudController.GetVisibleHearts(), "Should show 3 lives");
            Assert.IsFalse(hudController.IsComboVisible(), "Combo should be hidden");
        }
        
        [UnityTest]
        public IEnumerator AC5_HUD_UpdatesViaEvents_NotPolling()
        {
            // Arrange
            hudController.Initialize();
            string initialScore = hudController.GetScoreText();
            
            // Act - Fire score changed event (not calling HUD directly)
            scoreManager.AddScore(50);
            yield return null;
            
            // Assert - HUD updated because it subscribed to event
            Assert.AreNotEqual(initialScore, hudController.GetScoreText());
            Assert.AreEqual("50", hudController.GetScoreText());
        }
    }
}
```

---

## Implementation Steps (TDD Flow)

### Step 1: Write Failing Tests ❌
1. Create `HUDControllerTests.cs` in `Assets/Tests/PlayMode/UI/`
2. Implement all 5 test methods (AC1-AC5)
3. Run tests → **All should FAIL** (HUDController doesn't exist yet)

### Step 2: Minimal Implementation ✅
4. Create `HUDController.cs` in `Assets/Scripts/UI/`
5. Create basic Unity scene with Canvas and HUD prefab
6. Wire up SerializeField references in Inspector
7. Implement minimal code to make tests pass
8. Run tests → **All should PASS**

### Step 3: Refactor 🔄
9. Extract common code to helper methods
10. Optimize event subscription/unsubscription
11. Add XML documentation comments
12. Run tests → **All should still PASS**

### Step 4: Edge Cases 🧪
13. Add tests for edge cases:
    - Score goes negative (bomb hit)
    - Combo multiplier exceeds 5x (should cap at 5x)
    - Lives go negative (should stay at 0)
14. Fix implementation to handle edge cases
15. Run tests → **All should PASS**

---

## Definition of Done

- [ ] All 5 acceptance criteria have passing PlayMode tests
- [ ] HUD prefab created and configured in Unity
- [ ] HUDController.cs implemented with event-driven updates
- [ ] No Update() polling for UI updates (event-driven only)
- [ ] Code reviewed by peer
- [ ] Manual smoke test in Play Mode (5 minutes)
- [ ] Test coverage report shows 80%+ on HUDController
- [ ] No console errors or warnings
- [ ] Story moved to 'done' status in sprint-status.yaml

---

## Dependencies

**Required Before Starting:**
- ✅ ScoreManager (STORY-004) - for score events
- ✅ GameStateController (STORY-003) - for lives events
- TextMeshPro package installed in Unity project

**Blocks:**
- STORY-011 (Main Menu) - needs HUD prefab reference
- STORY-014 (Visual Effects) - VFX may trigger HUD updates

---

## Notes

**Testing Tips:**
- Use `[UnitySetUp]` instead of `[SetUp]` for PlayMode tests
- Use `Canvas.ForceUpdateCanvases()` if testing layout groups
- Test UI **state** (text content, visibility), not visual appearance
- Keep tests fast - use `yield return null` for one-frame waits

**Common Pitfalls:**
- Forgetting to create EventSystem in test scene (required for UI input)
- Not waiting one frame after triggering events (UI updates next frame)
- Testing with `Update()` polling instead of events (defeats TDD purpose)

---

## Related Documents

- Epic: `docs/epics/epic-ui-game-flow.md`
- Test Plan: `docs/test-plans/test-plan-story-010-hud.md` (to be created)
- Test Spec: `docs/test-specs/test-spec-story-010-hud.md` (to be created)
- Architecture: `docs/game-architecture.md` (UI Components section)

---

**Document Status:** READY FOR DEVELOPMENT  
**Next Step:** Begin TDD implementation (write tests first!)  
**Estimated Time:** 4-6 hours (including testing)
