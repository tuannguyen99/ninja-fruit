# Game Architecture Document: Ninja Fruit

**Project:** Ninja Fruit - Unity Test Automation Demo  
**Author:** Cloud Dragonborn (Game Architect) + Bmad  
**Engine:** Unity 6  
**Date:** November 26, 2025  
**Version:** 1.0  
**Primary Focus:** Testable Architecture with CI/CD Integration

---

## Executive Summary

This document defines the technical architecture for Ninja Fruit, a casual arcade game built in Unity 6 to demonstrate BMAD testing methodologies. The architecture emphasizes **testability, maintainability, and cross-platform compatibility** while keeping complexity appropriate for a demonstration project.

### Key Architectural Decisions

| Category | Decision | Rationale |
|----------|----------|-----------|
| **Engine** | Unity 6 (Latest) | Modern features, improved performance, latest Input System support |
| **Architecture Pattern** | Simple MonoBehaviour | Appropriate for small scope, easy to test, low cognitive overhead |
| **Test Assembly** | Single Test Assembly | Unified test structure, simpler CI/CD configuration |
| **CI/CD Platform** | GitHub Actions | Excellent Unity support via GameCI, free for public repos |
| **Input System** | New Input System | Modern, testable, cross-platform abstraction |

---

## Architecture Overview

### High-Level Structure

```
Ninja Fruit Unity Project
│
├── Assets/
│   ├── Scripts/                    # Core game code
│   │   ├── Gameplay/              # Game mechanics (spawning, scoring, collision)
│   │   ├── Input/                 # Input handling and gesture detection
│   │   ├── UI/                    # Menu and HUD controllers
│   │   ├── Managers/              # Game state and singleton managers
│   │   └── Utilities/             # Helper classes and extensions
│   │
│   ├── Tests/                      # All test code
│   │   ├── EditMode/              # Unit tests (no runtime required)
│   │   ├── PlayMode/              # Integration tests (Unity runtime)
│   │   ├── TestUtilities/         # Mocks, fixtures, test helpers
│   │   └── NinjaFruit.Tests.asmdef # Single test assembly definition
│   │
│   ├── Prefabs/                    # Game object prefabs (fruits, VFX)
│   ├── Scenes/                     # Unity scenes (MainMenu, GamePlay)
│   ├── InputActions/               # Input Action Assets
│   └── Resources/                  # Runtime-loaded assets
│
├── .github/workflows/              # CI/CD configuration
│   ├── build.yml                  # Multi-platform builds
│   ├── test.yml                   # Automated test execution
│   └── deploy.yml                 # Deployment automation
│
└── ProjectSettings/                # Unity project configuration
```

---

## Component Architecture

### Core Components (MonoBehaviour-based)

All game logic lives in MonoBehaviour components attached to GameObjects. This keeps the architecture simple, Unity-native, and easy to test with Unity Test Framework.

#### 1. Gameplay Components

##### FruitSpawner
**Responsibility:** Spawn fruits and bombs according to difficulty scaling  
**Dependencies:** None (pure logic + Unity physics)  
**Testability:** Edit Mode tests for spawn timing calculations, Play Mode tests for actual spawning

```csharp
public class FruitSpawner : MonoBehaviour
{
    [Header("Spawn Configuration")]
    [SerializeField] private GameObject[] fruitPrefabs;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject goldenFruitPrefab;
    
    [Header("Timing")]
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float maxSpawnInterval = 2.0f;
    
    // Public API for testing
    public void SpawnFruit();
    public float CalculateSpawnInterval(int currentScore);
    public float CalculateFruitSpeed(int currentScore);
    public bool ShouldSpawnBomb(int fruitCount);
}
```

**Testing Strategy:**
- Unit tests: Spawn interval formula, difficulty scaling, bomb spawn frequency
- Integration tests: Actual fruit instantiation, physics trajectory validation

---

##### SwipeDetector
**Responsibility:** Detect swipe gestures from New Input System, validate speed threshold  
**Dependencies:** New Input System, InputManager  
**Testability:** Input simulation via InputTestFixture

```csharp
public class SwipeDetector : MonoBehaviour
{
    [Header("Swipe Settings")]
    [SerializeField] private float minimumSwipeSpeed = 100f; // pixels/second
    
    private InputManager inputManager;
    private List<Vector2> swipePoints = new List<Vector2>();
    
    // Events for loose coupling
    public event Action<Vector2, Vector2> OnSwipeDetected; // start, end
    
    // Public API for testing
    public bool IsValidSwipe(List<Vector2> points, float deltaTime);
    public float CalculateSwipeSpeed(Vector2 start, Vector2 end, float deltaTime);
}
```

**Testing Strategy:**
- Unit tests: Speed calculation, swipe validation logic
- Integration tests: Input simulation with various swipe patterns

---

##### CollisionManager
**Responsibility:** Detect swipe-fruit intersections, handle slicing physics  
**Dependencies:** SwipeDetector, ScoreManager  
**Testability:** Geometry logic testable in Edit Mode, collision testing in Play Mode

```csharp
public class CollisionManager : MonoBehaviour
{
    [Header("Collision Detection")]
    [SerializeField] private LayerMask fruitLayer;
    [SerializeField] private LayerMask bombLayer;
    
    // Subscribe to SwipeDetector.OnSwipeDetected
    private void OnEnable()
    {
        SwipeDetector.OnSwipeDetected += CheckCollisions;
    }
    
    // Public API for testing
    public bool DoesSwipeIntersectFruit(Vector2 swipeStart, Vector2 swipeEnd, Vector2 fruitPos, float fruitRadius);
    public List<GameObject> GetFruitsInSwipePath(Vector2 start, Vector2 end);
}
```

**Testing Strategy:**
- Unit tests: Line-circle intersection math, edge cases (tangent swipes)
- Integration tests: Multi-fruit slicing, bomb collision detection

---

##### ScoreManager
**Responsibility:** Calculate scores, manage combo multipliers, track statistics  
**Dependencies:** None (pure logic + PlayerPrefs for persistence)  
**Testability:** Highly testable - pure calculation logic

```csharp
public class ScoreManager : MonoBehaviour
{
    [Header("Scoring Rules")]
    [SerializeField] private int applePoints = 10;
    [SerializeField] private int watermelonPoints = 20;
    [SerializeField] private int bombPenalty = -50;
    
    [Header("Combo Settings")]
    [SerializeField] private float comboWindow = 1.5f;
    [SerializeField] private int maxComboMultiplier = 5;
    
    // Public state for UI binding
    public int CurrentScore { get; private set; }
    public int ComboMultiplier { get; private set; }
    public int HighScore { get; private set; }
    
    // Events for UI updates
    public event Action<int> OnScoreChanged;
    public event Action<int> OnComboChanged;
    
    // Public API for testing
    public void RegisterSlice(FruitType fruitType, bool isGolden = false);
    public void RegisterBombHit();
    public void UpdateComboTimer(float deltaTime);
    public int CalculatePoints(FruitType fruitType, int multiplier, bool isGolden);
}
```

**Testing Strategy:**
- Unit tests: Point calculation, combo logic, negative score handling
- Integration tests: Score persistence across sessions

---

##### GameStateController
**Responsibility:** Manage game states (Menu, Playing, Paused, GameOver), track lives  
**Dependencies:** ScoreManager, UIManager  
**Testability:** State machine logic testable, scene management testable in Play Mode

```csharp
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

public class GameStateController : MonoBehaviour
{
    [Header("Game Rules")]
    [SerializeField] private int startingLives = 3;
    
    public GameState CurrentState { get; private set; }
    public int LivesRemaining { get; private set; }
    
    public event Action<GameState> OnStateChanged;
    public event Action<int> OnLivesChanged;
    
    // Public API for testing
    public void StartGame();
    public void PauseGame();
    public void ResumeGame();
    public void RegisterMissedFruit();
    public void EndGame();
}
```

**Testing Strategy:**
- Unit tests: State transition logic, lives tracking
- Integration tests: Full game flow (start → play → 3 misses → game over)

---

#### 2. Input Components

##### InputManager
**Responsibility:** Abstract input across platforms (touch/mouse), integrate with New Input System  
**Dependencies:** New Input System package  
**Testability:** Input simulation via Unity's InputTestFixture

```csharp
public class InputManager : MonoBehaviour
{
    private PlayerInputActions inputActions; // Generated from Input Actions asset
    
    public event Action<Vector2> OnInputStart;
    public event Action<Vector2> OnInputMove;
    public event Action<Vector2> OnInputEnd;
    
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Gameplay.Touch.started += OnTouchStart;
        inputActions.Gameplay.Touch.performed += OnTouchMove;
        inputActions.Gameplay.Touch.canceled += OnTouchEnd;
    }
    
    // Public API for testing
    public Vector2 GetCurrentInputPosition();
    public bool IsInputActive();
}
```

**Input Actions Asset:**
```
Action Map: Gameplay
- Action: Touch
  - Binding: <Mouse>/leftButton + <Mouse>/position
  - Binding: <Touchscreen>/primaryTouch/press + <Touchscreen>/primaryTouch/position
```

**Testing Strategy:**
- Unit tests: Input event wiring
- Integration tests: Simulate touch/mouse input, verify gesture detection

---

#### 3. UI Components

##### UIManager
**Responsibility:** Update HUD elements, handle menu navigation  
**Dependencies:** ScoreManager, GameStateController  
**Testability:** UI state verification in Play Mode tests

```csharp
public class UIManager : MonoBehaviour
{
    [Header("HUD References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private GameObject[] lifeHearts;
    
    [Header("Menus")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    
    // Subscribe to manager events
    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateScoreDisplay;
        GameStateController.OnLivesChanged += UpdateLivesDisplay;
    }
    
    // Public API for testing
    public void ShowGameOverScreen(int finalScore, int highScore);
    public void UpdateScoreDisplay(int score);
}
```

**Testing Strategy:**
- Integration tests: Verify UI updates when game events occur
- Play Mode tests: Check menu transitions

---

### Manager Pattern (Singleton for Global Services)

For truly global services (game state, score tracking), we use a lightweight singleton pattern:

```csharp
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                }
            }
            return instance;
        }
    }
    
    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this as T;
        }
    }
}
```

**Usage:**
```csharp
public class ScoreManager : Singleton<ScoreManager> { }
public class GameStateController : Singleton<GameStateController> { }
```

**Testing Considerations:**
- Reset singletons between tests using `[SetUp]` and `[TearDown]`
- Use `DestroyImmediate` in Edit Mode tests

---

## Testing Architecture

### Test Assembly Structure

**Single Test Assembly:** `NinjaFruit.Tests.asmdef`

**Location:** `Assets/Tests/`

**Assembly Definition File:**
```json
{
    "name": "NinjaFruit.Tests",
    "rootNamespace": "NinjaFruit.Tests",
    "references": [
        "UnityEngine.TestRunner",
        "UnityEditor.TestRunner",
        "NinjaFruit.Runtime"
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
    "versionDefines": [],
    "noEngineReferences": false
}
```

**Runtime Assembly:** `NinjaFruit.Runtime.asmdef`

**Location:** `Assets/Scripts/`

```json
{
    "name": "NinjaFruit.Runtime",
    "rootNamespace": "NinjaFruit",
    "references": [
        "Unity.InputSystem"
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

### Test Categories

#### Edit Mode Tests (Unit Tests)
**Location:** `Assets/Tests/EditMode/`  
**Purpose:** Test pure logic without Unity runtime  
**Speed:** Fast (milliseconds per test)  
**Coverage Target:** 80%+ on core logic

**Example Structure:**
```
Tests/EditMode/
├── Gameplay/
│   ├── FruitSpawnerTests.cs
│   ├── ScoreManagerTests.cs
│   └── CollisionMathTests.cs
├── Input/
│   └── SwipeDetectorTests.cs
└── Utilities/
    └── DifficultyCalculatorTests.cs
```

**Example Test:**
```csharp
using NUnit.Framework;
using NinjaFruit;

namespace NinjaFruit.Tests.EditMode
{
    [TestFixture]
    public class ScoreManagerTests
    {
        private ScoreManager scoreManager;
        
        [SetUp]
        public void Setup()
        {
            GameObject testObject = new GameObject();
            scoreManager = testObject.AddComponent<ScoreManager>();
        }
        
        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(scoreManager.gameObject);
        }
        
        [Test]
        public void CalculatePoints_AppleWithCombo3x_Returns30()
        {
            // Arrange
            FruitType fruitType = FruitType.Apple;
            int multiplier = 3;
            bool isGolden = false;
            
            // Act
            int result = scoreManager.CalculatePoints(fruitType, multiplier, isGolden);
            
            // Assert
            Assert.AreEqual(30, result);
        }
        
        [Test]
        public void RegisterBombHit_ResetsComboMultiplier()
        {
            // Arrange
            scoreManager.RegisterSlice(FruitType.Apple);
            scoreManager.RegisterSlice(FruitType.Banana);
            Assert.AreEqual(2, scoreManager.ComboMultiplier);
            
            // Act
            scoreManager.RegisterBombHit();
            
            // Assert
            Assert.AreEqual(1, scoreManager.ComboMultiplier);
            Assert.IsTrue(scoreManager.CurrentScore < 0); // Penalty applied
        }
    }
}
```

---

#### Play Mode Tests (Integration Tests)
**Location:** `Assets/Tests/PlayMode/`  
**Purpose:** Test component interactions with Unity runtime  
**Speed:** Slower (seconds per test)  
**Coverage Target:** 100% on critical game flows

**Example Structure:**
```
Tests/PlayMode/
├── Gameplay/
│   ├── FruitSpawningIntegrationTests.cs
│   ├── GameFlowTests.cs
│   └── CollisionDetectionTests.cs
├── Input/
│   └── InputSystemIntegrationTests.cs
└── UI/
    └── UIUpdateTests.cs
```

**Example Test:**
```csharp
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NinjaFruit;

namespace NinjaFruit.Tests.PlayMode
{
    [TestFixture]
    public class GameFlowTests
    {
        private GameStateController gameController;
        private ScoreManager scoreManager;
        
        [SetUp]
        public void Setup()
        {
            // Create test scene objects
            GameObject gameControllerObject = new GameObject("GameController");
            gameController = gameControllerObject.AddComponent<GameStateController>();
            
            GameObject scoreManagerObject = new GameObject("ScoreManager");
            scoreManager = scoreManagerObject.AddComponent<ScoreManager>();
        }
        
        [TearDown]
        public void Teardown()
        {
            Object.Destroy(gameController.gameObject);
            Object.Destroy(scoreManager.gameObject);
        }
        
        [UnityTest]
        public IEnumerator GameFlow_ThreeMissedFruits_TriggersGameOver()
        {
            // Arrange
            gameController.StartGame();
            Assert.AreEqual(GameState.Playing, gameController.CurrentState);
            Assert.AreEqual(3, gameController.LivesRemaining);
            
            // Act - Simulate 3 missed fruits
            gameController.RegisterMissedFruit();
            yield return null; // Wait one frame
            
            gameController.RegisterMissedFruit();
            yield return null;
            
            gameController.RegisterMissedFruit();
            yield return null;
            
            // Assert
            Assert.AreEqual(GameState.GameOver, gameController.CurrentState);
            Assert.AreEqual(0, gameController.LivesRemaining);
        }
        
        [UnityTest]
        public IEnumerator SpawnSystem_SpawnsFruitWithPhysics()
        {
            // Arrange
            GameObject spawnerObject = new GameObject("Spawner");
            FruitSpawner spawner = spawnerObject.AddComponent<FruitSpawner>();
            
            // Load fruit prefab (requires Resources folder setup)
            GameObject fruitPrefab = Resources.Load<GameObject>("Prefabs/Apple");
            Assert.IsNotNull(fruitPrefab, "Fruit prefab not found in Resources");
            
            // Act
            spawner.SpawnFruit();
            yield return new WaitForSeconds(0.1f);
            
            // Assert
            GameObject spawnedFruit = GameObject.FindWithTag("Fruit");
            Assert.IsNotNull(spawnedFruit, "No fruit spawned");
            Assert.IsNotNull(spawnedFruit.GetComponent<Rigidbody2D>(), "Fruit missing Rigidbody2D");
            
            // Cleanup
            Object.Destroy(spawnerObject);
            Object.Destroy(spawnedFruit);
        }
    }
}
```

---

#### Input Testing with New Input System

Unity's Input System package provides `InputTestFixture` for simulating input:

```csharp
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using NUnit.Framework;
using System.Collections;

namespace NinjaFruit.Tests.PlayMode
{
    [TestFixture]
    public class InputSystemTests : InputTestFixture
    {
        private Mouse mouse;
        private Touchscreen touchscreen;
        
        public override void Setup()
        {
            base.Setup();
            mouse = InputSystem.AddDevice<Mouse>();
            touchscreen = InputSystem.AddDevice<Touchscreen>();
        }
        
        [UnityTest]
        public IEnumerator SwipeDetector_FastMouseSwipe_TriggersSwipeEvent()
        {
            // Arrange
            GameObject swipeObject = new GameObject("SwipeDetector");
            SwipeDetector detector = swipeObject.AddComponent<SwipeDetector>();
            
            bool swipeDetected = false;
            detector.OnSwipeDetected += (start, end) => swipeDetected = true;
            
            // Act - Simulate fast mouse swipe
            Set(mouse.position, new Vector2(100, 100));
            Press(mouse.leftButton);
            yield return null;
            
            Set(mouse.position, new Vector2(300, 300));
            yield return null;
            
            Release(mouse.leftButton);
            yield return null;
            
            // Assert
            Assert.IsTrue(swipeDetected, "Swipe event not triggered");
            
            // Cleanup
            Object.Destroy(swipeObject);
        }
    }
}
```

---

### Test Utilities

**Location:** `Assets/Tests/TestUtilities/`

Common test helpers for mocking, fixtures, and assertions:

```csharp
namespace NinjaFruit.Tests.Utilities
{
    public static class TestHelpers
    {
        public static GameObject CreateTestFruit(FruitType type, Vector2 position)
        {
            GameObject fruit = new GameObject($"Test{type}");
            fruit.tag = "Fruit";
            fruit.transform.position = position;
            
            var fruitComponent = fruit.AddComponent<Fruit>();
            fruitComponent.fruitType = type;
            
            var collider = fruit.AddComponent<CircleCollider2D>();
            collider.radius = 0.3f;
            
            return fruit;
        }
        
        public static void ResetSingletons()
        {
            var scoreManager = Object.FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
                Object.DestroyImmediate(scoreManager.gameObject);
                
            var gameController = Object.FindObjectOfType<GameStateController>();
            if (gameController != null)
                Object.DestroyImmediate(gameController.gameObject);
        }
    }
    
    public class MockInputProvider : IInputProvider
    {
        public Vector2 SimulatedPosition { get; set; }
        public bool SimulatedIsActive { get; set; }
        
        public Vector2 GetPosition() => SimulatedPosition;
        public bool IsActive() => SimulatedIsActive;
    }
}
```

---

## CI/CD Architecture

### GitHub Actions Integration

**Platform:** GitHub Actions + GameCI  
**Strategy:** Multi-platform matrix builds with automated testing  
**Triggers:** Push to main/develop, pull requests, weekly scheduled runs

---

### Workflow 1: Build Pipeline

**File:** `.github/workflows/build.yml`

```yaml
name: Build Multi-Platform

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main, develop]

jobs:
  build:
    name: Build for ${{ matrix.targetPlatform }}
    runs-on: ubuntu-latest
    strategy:
      fail-fast: false
      matrix:
        targetPlatform:
          - StandaloneWindows64  # Fastest build for quick feedback
          - WebGL                # Easy deployment/demo
          - Android              # Mobile target
          # - StandaloneOSX      # Optional: macOS requires macOS runner
          # - iOS                # Optional: requires macOS runner + certificates
    
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
        with:
          lfs: true  # If using Git LFS for assets
      
      - name: Cache Unity Library
        uses: actions/cache@v3
        with:
          path: Library
          key: Library-${{ matrix.targetPlatform }}-${{ hashFiles('Assets/**', 'Packages/**', 'ProjectSettings/**') }}
          restore-keys: |
            Library-${{ matrix.targetPlatform }}-
            Library-
      
      - name: Build Unity Project
        uses: game-ci/unity-builder@v4
        env:
          UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
          UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
          UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
        with:
          targetPlatform: ${{ matrix.targetPlatform }}
          unityVersion: 6000.0.25f1  # Unity 6 version (verify latest)
          buildName: NinjaFruit
          buildsPath: builds
      
      - name: Upload Build Artifact
        uses: actions/upload-artifact@v3
        with:
          name: Build-${{ matrix.targetPlatform }}
          path: builds/${{ matrix.targetPlatform }}
          retention-days: 7
```

---

### Workflow 2: Test Execution

**File:** `.github/workflows/test.yml`

```yaml
name: Run Tests

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main, develop]

jobs:
  test:
    name: Test Unity Project
    runs-on: ubuntu-latest
    
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
        with:
          lfs: true
      
      - name: Cache Unity Library
        uses: actions/cache@v3
        with:
          path: Library
          key: Library-Tests-${{ hashFiles('Assets/**', 'Packages/**') }}
          restore-keys: Library-Tests-
      
      - name: Run Edit Mode Tests
        uses: game-ci/unity-test-runner@v4
        env:
          UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
          UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
          UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
        with:
          unityVersion: 6000.0.25f1
          testMode: EditMode
          coverageOptions: 'generateAdditionalMetrics;generateHtmlReport;generateBadgeReport'
      
      - name: Run Play Mode Tests
        uses: game-ci/unity-test-runner@v4
        env:
          UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
          UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
          UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
        with:
          unityVersion: 6000.0.25f1
          testMode: PlayMode
          coverageOptions: 'generateAdditionalMetrics;generateHtmlReport;generateBadgeReport'
      
      - name: Upload Test Results
        uses: actions/upload-artifact@v3
        if: always()
        with:
          name: Test-Results
          path: artifacts/
          retention-days: 14
      
      - name: Upload Coverage Report
        uses: actions/upload-artifact@v3
        if: always()
        with:
          name: Coverage-Report
          path: CodeCoverage/
          retention-days: 14
      
      - name: Comment Test Results on PR
        uses: EnricoMi/publish-unit-test-result-action@v2
        if: github.event_name == 'pull_request'
        with:
          files: artifacts/**/*.xml
```

---

### Workflow 3: Deployment (Optional)

**File:** `.github/workflows/deploy.yml`

```yaml
name: Deploy WebGL Build

on:
  workflow_run:
    workflows: ["Build Multi-Platform"]
    types:
      - completed
    branches: [main]

jobs:
  deploy-webgl:
    name: Deploy to GitHub Pages
    runs-on: ubuntu-latest
    if: ${{ github.event.workflow_run.conclusion == 'success' }}
    
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
      
      - name: Download WebGL Build
        uses: dawidd6/action-download-artifact@v2
        with:
          workflow: build.yml
          name: Build-WebGL
          path: webgl-build
      
      - name: Deploy to GitHub Pages
        uses: peaceiris/actions-gh-pages@v3
        with:
          github_token: ${{ secrets.GITHUB_TOKEN }}
          publish_dir: ./webgl-build
          cname: ninja-fruit.yourdomain.com  # Optional custom domain
```

---

### CI/CD Setup Checklist

- [ ] **Unity License Setup**
  - Obtain Unity license file (Personal/Plus/Pro)
  - Activate Unity via `game-ci/unity-request-activation-file`
  - Store license as GitHub secret: `UNITY_LICENSE`
  - Store credentials as secrets: `UNITY_EMAIL`, `UNITY_PASSWORD`

- [ ] **Repository Secrets Configuration**
  - Add `UNITY_LICENSE` (base64-encoded .ulf file)
  - Add `UNITY_EMAIL` (Unity account email)
  - Add `UNITY_PASSWORD` (Unity account password)

- [ ] **Branch Protection Rules**
  - Require passing tests before merge
  - Require build success for all platforms
  - Enable status checks for PRs

- [ ] **Workflow Permissions**
  - Enable "Read and write permissions" for `GITHUB_TOKEN`
  - Required for uploading artifacts and posting PR comments

---

## Cross-Platform Considerations

### Input Abstraction

**Challenge:** Different input methods across platforms  
**Solution:** New Input System's control schemes

**Input Actions Asset Configuration:**
```
Control Schemes:
1. Touch (Mobile)
   - Required Devices: Touchscreen
   
2. Mouse (Desktop)
   - Required Devices: Mouse, Keyboard
```

**Runtime Detection:**
```csharp
public class InputManager : MonoBehaviour
{
    private PlayerInputActions inputActions;
    
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        
        // Auto-detect platform
        #if UNITY_ANDROID || UNITY_IOS
        inputActions.bindingMask = InputBinding.MaskByGroup("Touch");
        #else
        inputActions.bindingMask = InputBinding.MaskByGroup("Mouse");
        #endif
        
        inputActions.Enable();
    }
}
```

---

### Performance Targets

| Platform | Target FPS | Max Spawn Rate | Memory Budget |
|----------|------------|----------------|---------------|
| **PC (Windows/Mac/Linux)** | 60 FPS | 20 fruits + 5 bombs | 500 MB |
| **Mobile (Android/iOS)** | 60 FPS (high-end), 30 FPS (low-end) | 15 fruits + 3 bombs | 200 MB |
| **WebGL** | 60 FPS | 15 fruits + 3 bombs | 300 MB |

**Performance Testing Strategy:**
- Unity Profiler snapshots during stress tests
- Automated performance tests in CI (Play Mode tests with frame time assertions)
- Memory leak detection via extended play sessions

---

### Platform-Specific Build Settings

**Windows Standalone:**
- Compression: LZ4 (faster loading)
- Scripting Backend: IL2CPP (better performance)
- API Compatibility Level: .NET Standard 2.1

**WebGL:**
- Compression: Brotli (smaller download)
- Memory Size: 512 MB
- Enable Exceptions: Explicitly Thrown Exceptions Only
- Optimization Level: Maximum Size

**Android:**
- Scripting Backend: IL2CPP
- API Level: Minimum 24 (Android 7.0)
- Target Architectures: ARM64 (required for Google Play)
- Install Location: Automatic
- Internet Access: Not Required

---

## Dependency Management

### Unity Packages (via Package Manager)

**Essential Packages:**
```json
{
  "dependencies": {
    "com.unity.inputsystem": "1.7.0",           // New Input System
    "com.unity.test-framework": "1.4.5",        // Unity Test Framework
    "com.unity.textmeshpro": "3.0.8",           // UI text rendering
    "com.unity.2d.sprite": "1.0.0",             // 2D sprite support
    "com.unity.2d.tilemap": "1.0.0",            // Optional: background tiles
    "com.unity.ugui": "2.0.0"                   // UI system
  }
}
```

**Optional Development Packages:**
```json
{
  "dependencies": {
    "com.unity.performance.profile-analyzer": "1.2.2",  // Performance analysis
    "com.unity.cinemachine": "2.9.7",                   // Camera control (future)
    "com.unity.2d.animation": "10.0.0"                  // Sprite animation (future)
  }
}
```

---

### Third-Party Dependencies

**None for MVP.**  
Philosophy: Keep dependencies minimal for demo project. Unity's built-in systems are sufficient.

**If Expanding Post-MVP:**
- DOTween (animation tweening)
- Addressables (asset management at scale)
- Unity Analytics (player behavior tracking)

---

## Data Architecture

### Persistence Strategy

**Technology:** Unity PlayerPrefs (simple key-value storage)  
**Scope:** Local device only (no cloud sync)

**Saved Data:**
```csharp
public class PersistenceManager : MonoBehaviour
{
    private const string HIGH_SCORE_KEY = "HighScore";
    private const string TOTAL_FRUITS_SLICED_KEY = "TotalFruitsSliced";
    private const string LONGEST_COMBO_KEY = "LongestCombo";
    
    public void SaveHighScore(int score)
    {
        if (score > PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0))
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
            PlayerPrefs.Save();
        }
    }
    
    public int LoadHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }
    
    // Similar methods for other stats
}
```

**Testing Strategy:**
- Use `PlayerPrefs.DeleteAll()` in test `[SetUp]` methods
- Verify data persistence across simulated app restarts
- Test data migration (if adding new keys in future versions)

---

### Scene Architecture

**Scenes:**
1. **MainMenu.unity** - Entry point, UI-only scene
2. **GamePlay.unity** - Core game loop
3. **BootLoader.unity** (Optional) - Initialize singletons, load configs

**Scene Flow:**
```
MainMenu → (Play Button) → GamePlay
GamePlay → (Game Over → Main Menu Button) → MainMenu
GamePlay → (Pause → Quit Button) → MainMenu
```

**Scene Management:**
```csharp
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void LoadGamePlay()
    {
        SceneManager.LoadScene("GamePlay");
    }
    
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
```

---

## Error Handling and Debugging

### Logging Strategy

**Custom Logger:**
```csharp
public static class GameLogger
{
    private const bool ENABLE_DEBUG_LOGS = true;
    
    public static void Log(string message, Object context = null)
    {
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (ENABLE_DEBUG_LOGS)
            Debug.Log($"[NinjaFruit] {message}", context);
        #endif
    }
    
    public static void LogError(string message, Object context = null)
    {
        Debug.LogError($"[NinjaFruit ERROR] {message}", context);
    }
    
    public static void LogWarning(string message, Object context = null)
    {
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.LogWarning($"[NinjaFruit WARNING] {message}", context);
        #endif
    }
}
```

**Testing Strategy:**
- Capture Unity logs in tests using `LogAssert.Expect()`
- Verify error handling via negative test cases

---

### Exception Handling

**Graceful Degradation:**
```csharp
public class FruitSpawner : MonoBehaviour
{
    private void SpawnFruitInternal()
    {
        try
        {
            GameObject fruit = Instantiate(fruitPrefabs[Random.Range(0, fruitPrefabs.Length)]);
            // ... setup logic
        }
        catch (System.Exception e)
        {
            GameLogger.LogError($"Failed to spawn fruit: {e.Message}");
            // Optionally: trigger safe mode (slower spawn rate)
        }
    }
}
```

---

## Security and Data Privacy

**No Sensitive Data Collected:**
- All data stored locally (PlayerPrefs)
- No analytics, telemetry, or user tracking
- No network communication required

**If Expanding to Multiplayer/Online:**
- Use HTTPS for all API communication
- Implement authentication tokens with expiration
- Encrypt sensitive data at rest
- Follow platform-specific privacy policies (GDPR, COPPA, etc.)

---

## Performance Optimization Strategy

### Object Pooling (Future Enhancement)

**Current MVP:** Instantiate/Destroy fruits directly  
**Post-MVP:** Object pool for fruits/VFX to reduce GC pressure

```csharp
public class FruitPool : Singleton<FruitPool>
{
    private Queue<GameObject> fruitPool = new Queue<GameObject>();
    
    public GameObject GetFruit(FruitType type)
    {
        if (fruitPool.Count > 0)
        {
            var fruit = fruitPool.Dequeue();
            fruit.SetActive(true);
            return fruit;
        }
        else
        {
            return Instantiate(GetPrefabForType(type));
        }
    }
    
    public void ReturnFruit(GameObject fruit)
    {
        fruit.SetActive(false);
        fruitPool.Enqueue(fruit);
    }
}
```

---

### Profiling Integration

**During Development:**
- Unity Profiler for CPU/GPU/Memory analysis
- Deep Profile mode for hotspot identification
- Frame Debugger for rendering optimization

**In CI/CD:**
- Automated performance tests (Play Mode with frame time assertions)
- Memory leak detection via extended test runs

---

## Extensibility and Future Enhancements

### Architecture Supports (Post-MVP):

1. **New Game Modes**
   - Add new `GameMode` enum to `GameStateController`
   - Create mode-specific spawning rules in `FruitSpawner`
   - Minimal code changes required

2. **Power-Ups**
   - Create `PowerUp` base class (similar to `Fruit`)
   - Add `PowerUpManager` singleton
   - Wire into `CollisionManager` detection

3. **Multiplayer**
   - Add networking layer (Unity Netcode for GameObjects)
   - Synchronize `ScoreManager` state
   - Refactor input to support multiple players

4. **Analytics**
   - Add `AnalyticsManager` singleton
   - Track events via Unity Analytics SDK
   - Zero impact on existing components (observer pattern)

---

## Known Limitations and Technical Debt

### Current Limitations:

1. **No Network Layer**
   - Rationale: Demo project doesn't require multiplayer/leaderboards
   - Future: Add if expanding to online features

2. **Simple Collision Detection**
   - Current: Line-circle intersection math
   - Limitation: Doesn't handle complex fruit shapes
   - Future: Use Unity's Physics2D.Raycast for more accuracy

3. **No Audio Mixing**
   - Current: Direct AudioSource.Play() calls
   - Future: Implement AudioMixer for dynamic volume control

4. **Limited Save System**
   - Current: PlayerPrefs (not robust for complex data)
   - Future: JSON serialization or Unity's Save System package

---

## Validation Checklist

### Architecture Review Checklist:

- [x] **Testability:** All core components testable in Edit Mode or Play Mode
- [x] **Simplicity:** MonoBehaviour pattern appropriate for scope
- [x] **Cross-Platform:** Input abstraction via New Input System
- [x] **CI/CD Ready:** Test assembly structure compatible with GameCI
- [x] **Performance:** Architecture supports 60 FPS target
- [x] **Maintainability:** Clear component responsibilities, low coupling
- [x] **Extensibility:** Easy to add new features post-MVP
- [x] **Documentation:** All major components and patterns documented

---

## Implementation Order (For Development Phase)

### Sprint 1: Core Infrastructure
1. Create Unity 6 project, configure packages
2. Set up assembly definitions (`NinjaFruit.Runtime`, `NinjaFruit.Tests`)
3. Configure New Input System (create Input Actions asset)
4. Implement singleton pattern base class
5. Create GameStateController skeleton
6. Write first passing test (Hello World test)

### Sprint 2: Gameplay Foundations
7. Implement FruitSpawner (spawn logic, no difficulty scaling yet)
8. Implement SwipeDetector (basic gesture detection)
9. Implement CollisionManager (line-circle intersection)
10. Write Edit Mode tests for collision math
11. Create fruit prefabs with Rigidbody2D
12. Write Play Mode test for spawning

### Sprint 3: Scoring and Game Loop
13. Implement ScoreManager (points, combos, persistence)
14. Wire GameStateController state machine
15. Implement lives tracking and game over
16. Write comprehensive tests for scoring logic
17. Add difficulty scaling formulas
18. Integration test: full game flow

### Sprint 4: UI and Polish
19. Implement UIManager (HUD updates)
20. Create MainMenu and GamePlay scenes
21. Add visual effects (slice trails, particles)
22. Implement audio system (SFX, music)
23. Write UI integration tests

### Sprint 5: CI/CD and Multi-Platform
24. Configure GitHub Actions workflows
25. Set up Unity license activation
26. Test Windows build pipeline
27. Test WebGL build pipeline
28. Configure test execution in CI
29. Add coverage reporting
30. Deploy WebGL build to GitHub Pages

---

## Appendix A: Key Decisions Record

| Decision | Options Considered | Chosen | Rationale |
|----------|-------------------|--------|-----------|
| **Unity Version** | Unity 2022 LTS, Unity 6 | Unity 6 | Latest features, modern Input System support, future-proof |
| **Architecture Pattern** | MonoBehaviour, Service Locator, DI Container | MonoBehaviour | Simplicity for demo scope, Unity-native, easy testing |
| **Test Assembly Structure** | Single, Multiple (per-feature) | Single | Simpler CI/CD, unified test namespace, appropriate for small project |
| **CI/CD Platform** | GitHub Actions, Unity Cloud Build, GitLab CI | GitHub Actions | Free for public repos, GameCI integration, wide adoption |
| **Input System** | Legacy Input, New Input System | New Input System | Modern, testable, cross-platform, official recommendation |
| **Test Framework** | Unity Test Framework, NSubstitute, Moq | Unity Test Framework (NUnit) | Built-in, no external dependencies, sufficient for needs |
| **Dependency Injection** | Zenject, VContainer, Manual | Manual (Singletons) | Lightweight, no learning curve, appropriate for scope |

---

## Appendix B: Glossary

- **Assembly Definition (.asmdef):** Unity feature for organizing code into separate compiled assemblies
- **Edit Mode Tests:** Unit tests that run without Unity runtime (fast, no scene required)
- **Play Mode Tests:** Integration tests that require Unity runtime (slower, can interact with scenes)
- **GameCI:** Open-source CI/CD solution for Unity projects on GitHub Actions
- **New Input System:** Unity's modern input API (package `com.unity.inputsystem`)
- **PlayerPrefs:** Unity's built-in key-value storage system for persistent data
- **Singleton Pattern:** Design pattern ensuring only one instance of a class exists globally
- **Swipe Line Segment:** Mathematical representation of player's swipe gesture (start/end points)
- **Test Assembly:** Separate compiled assembly containing test code (excluded from builds)
- **Unity Test Framework:** Unity's official testing library (based on NUnit)

---

## Document Change Log

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-11-26 | Cloud Dragonborn + Bmad | Initial architecture document with Unity 6, MonoBehaviour pattern, single test assembly, GitHub Actions CI/CD, New Input System |

---

**Document Status:** COMPLETE - Ready for Epic Planning Phase  
**Next Step:** Return to Max (Scrum Master) for Epic and Story creation  
**Approval:** Pending review by Max and Murat (Test Architect)

---

*This architecture is designed to maximize testability and CI/CD automation while maintaining simplicity appropriate for a demonstration project. Every component decision considers test automation and BMAD workflow integration.*
