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
                // Use the current in-memory settings from the injected manager instead
                // of re-loading from PlayerPrefs which would overwrite runtime changes.
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
