using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using NinjaFruit.UI;

namespace NinjaFruit.Tests.PlayMode.E2E
{
    [TestFixture]
    public class MainMenuE2ETests
    {
        private GameObject canvasGO;
        private Canvas canvas;

        [SetUp]
        public void SetUp()
        {
            // Create test canvas
            canvasGO = new GameObject("E2E_TestCanvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // Create event system and attach correct input module if available
            var eventSystemGO = new GameObject("EventSystem");
            eventSystemGO.AddComponent<EventSystem>();
            var inputSystemType = System.Type.GetType("UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem.UI")
                                  ?? System.Type.GetType("UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");
            if (inputSystemType != null)
                eventSystemGO.AddComponent(inputSystemType);
            else
                eventSystemGO.AddComponent<StandaloneInputModule>();
        }

        [TearDown]
        public void TearDown()
        {
            var eventSystem = GameObject.Find("EventSystem");
            if (eventSystem != null)
                Object.DestroyImmediate(eventSystem);
            Object.DestroyImmediate(canvasGO);
            PlayerPrefs.DeleteAll();
        }

        [UnityTest]
        public IEnumerator EndToEnd_MainMenuFlow()
        {
            // Arrange: create managers and menu
            var highScoreManagerGO = new GameObject("HighScoreManager");
            var highScoreManager = highScoreManagerGO.AddComponent<HighScoreManager>();

            var settingsManagerGO = new GameObject("SettingsManager");
            var settingsManager = settingsManagerGO.AddComponent<SettingsManager>();

            var mockSceneManager = new NinjaFruit.Tests.PlayMode.UI.MockSceneTransitionManager();

            var menu = CreateTestMainMenu();
            menu.SetHighScoreManager(highScoreManager);
            menu.SetSettingsManager(settingsManager);
            menu.SetSceneManager(mockSceneManager);

            // Act & Assert sequence

            // Initialize and verify main menu visible
            menu.Initialize();
            yield return null;
            Assert.IsTrue(menu.IsMainMenuPanelVisible(), "Main menu should be visible after initialize");

            // Open Settings, change values, verify reflect
            settingsManager.SetMasterVolume(0.3f);
            settingsManager.SetSoundEffects(false);
            settingsManager.SetMusic(false);

            menu.OnSettingsClicked();
            yield return null;
            Assert.IsTrue(menu.IsSettingsPanelVisible(), "Settings panel should be visible");
            Assert.AreEqual(0.3f, menu.GetVolumeSliderValue(), 0.01f, "Volume slider should reflect settings manager value");
            Assert.IsFalse(menu.GetSoundEffectsToggleValue(), "Sound FX toggle should reflect settings manager");
            Assert.IsFalse(menu.GetMusicToggleValue(), "Music toggle should reflect settings manager");

            // Back to main menu
            menu.OnBackClicked();
            yield return null;
            Assert.IsTrue(menu.IsMainMenuPanelVisible(), "Main menu should be visible after back from settings");

            // Open High Scores and verify defaults (none)
            menu.OnHighScoresClicked();
            yield return null;
            Assert.IsTrue(menu.IsHighScoresPanelVisible(), "High scores panel should be visible");

            // Save a high score and verify it displays
            highScoreManager.SaveHighScore(2048);
            highScoreManager.SaveFruitCount(123);
            highScoreManager.SaveCombo(7);

            // Refresh high scores view
            menu.OnHighScoresClicked();
            yield return null;
            Assert.AreEqual("2048", menu.GetHighScoreText(), "High score text should match saved value");
            Assert.AreEqual("123", menu.GetTotalFruitsText(), "Total fruits text should match saved value");
            Assert.AreEqual("7x", menu.GetLongestComboText(), "Longest combo text should match saved value");

            // Back and then Play -> should call scene manager
            menu.OnBackClicked();
            yield return null;
            menu.OnPlayClicked();
            yield return null;
            Assert.IsTrue(mockSceneManager.LoadGameplaySceneCalled, "Play should trigger gameplay scene load via scene manager");

            // Cleanup
            Object.Destroy(menu.gameObject);
            Object.Destroy(highScoreManagerGO);
            Object.Destroy(settingsManagerGO);
        }

        // Helper to create test main menu (copied minimal helpers)
        private MainMenuController CreateTestMainMenu()
        {
            var menuGO = new GameObject("MainMenu");
            menuGO.transform.SetParent(canvas.transform);
            var menu = menuGO.AddComponent<MainMenuController>();

            // Panels
            menu.mainMenuPanel = CreatePanel(menuGO, "MainMenuPanel");
            menu.highScoresPanel = CreatePanel(menuGO, "HighScoresPanel");
            menu.settingsPanel = CreatePanel(menuGO, "SettingsPanel");

            // Buttons
            menu.playButton = CreateButton(menu.mainMenuPanel, "PlayButton");
            menu.highScoresButton = CreateButton(menu.mainMenuPanel, "HighScoresButton");
            menu.settingsButton = CreateButton(menu.mainMenuPanel, "SettingsButton");
            menu.quitButton = CreateButton(menu.mainMenuPanel, "QuitButton");

            // High scores UI
            menu.highScoreText = CreateText(menu.highScoresPanel, "HighScoreText");
            menu.totalFruitsText = CreateText(menu.highScoresPanel, "TotalFruitsText");
            menu.longestComboText = CreateText(menu.highScoresPanel, "LongestComboText");
            menu.highScoresBackButton = CreateButton(menu.highScoresPanel, "HighScoresBackButton");

            // Settings UI
            menu.masterVolumeSlider = CreateSlider(menu.settingsPanel, "VolumeSlider");
            menu.soundEffectsToggle = CreateToggle(menu.settingsPanel, "SoundFXToggle");
            menu.musicToggle = CreateToggle(menu.settingsPanel, "MusicToggle");
            menu.settingsBackButton = CreateButton(menu.settingsPanel, "SettingsBackButton");

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
