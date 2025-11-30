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
