using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using NinjaFruit.UI;

namespace NinjaFruit.Tests.PlayMode.UI
{
    /// <summary>
    /// Mock implementation of ISceneTransitionManager for testing
    /// </summary>
    public class MockSceneTransitionManager : NinjaFruit.Interfaces.ISceneTransitionManager
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
            var eventSystem = GameObject.Find("EventSystem");
            if (eventSystem != null)
                Object.DestroyImmediate(eventSystem);
            
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
