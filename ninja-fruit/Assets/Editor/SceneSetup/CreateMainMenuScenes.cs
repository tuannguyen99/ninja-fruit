using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class CreateMainMenuScenes
{
    [MenuItem("Tools/Setup/Create MainMenu & Gameplay Scenes")]
    public static void CreateScenes()
    {
        // Ensure Scenes folder
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
        {
            AssetDatabase.CreateFolder("Assets", "Scenes");
        }

        // Create MainMenu scene
        var mainScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Canvas
        var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // EventSystem
        var eventGO = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem));
        eventGO.transform.SetParent(canvasGO.transform, false);
        eventGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // MainMenu controller root
        var menuRoot = new GameObject("MainMenu");
        menuRoot.transform.SetParent(canvasGO.transform, false);
        var menu = menuRoot.AddComponent<NinjaFruit.UI.MainMenuController>();

        // Panels
        var mainPanel = new GameObject("MainMenuPanel"); mainPanel.transform.SetParent(menuRoot.transform, false);
        var highPanel = new GameObject("HighScoresPanel"); highPanel.transform.SetParent(menuRoot.transform, false); highPanel.SetActive(false);
        var settingsPanel = new GameObject("SettingsPanel"); settingsPanel.transform.SetParent(menuRoot.transform, false); settingsPanel.SetActive(false);

        // Buttons
        var playBtn = CreateUIButton("PlayButton", "Play", mainPanel.transform);
        var hsBtn = CreateUIButton("HighScoresButton", "High Scores", mainPanel.transform);
        var settingsBtn = CreateUIButton("SettingsButton", "Settings", mainPanel.transform);
        var quitBtn = CreateUIButton("QuitButton", "Quit", mainPanel.transform);

        // High score texts
        var hsText = CreateUIText("HighScoreText", "0", highPanel.transform);
        var totalText = CreateUIText("TotalFruitsText", "0", highPanel.transform);
        var comboText = CreateUIText("LongestComboText", "0x", highPanel.transform);

        // Settings UI
        var volGO = new GameObject("VolumeSlider"); volGO.transform.SetParent(settingsPanel.transform, false); volGO.AddComponent<Slider>();
        var sfxGO = new GameObject("SoundFXToggle"); sfxGO.transform.SetParent(settingsPanel.transform, false); sfxGO.AddComponent<Toggle>();
        var musicGO = new GameObject("MusicToggle"); musicGO.transform.SetParent(settingsPanel.transform, false); musicGO.AddComponent<Toggle>();

        // Wire references
        menu.mainMenuPanel = mainPanel;
        menu.highScoresPanel = highPanel;
        menu.settingsPanel = settingsPanel;

        menu.playButton = playBtn.GetComponent<Button>();
        menu.highScoresButton = hsBtn.GetComponent<Button>();
        menu.settingsButton = settingsBtn.GetComponent<Button>();
        menu.quitButton = quitBtn.GetComponent<Button>();

        menu.highScoreText = hsText.GetComponent<TMPro.TextMeshProUGUI>();
        menu.totalFruitsText = totalText.GetComponent<TMPro.TextMeshProUGUI>();
        menu.longestComboText = comboText.GetComponent<TMPro.TextMeshProUGUI>();

        menu.masterVolumeSlider = volGO.GetComponent<Slider>();
        menu.soundEffectsToggle = sfxGO.GetComponent<Toggle>();
        menu.musicToggle = musicGO.GetComponent<Toggle>();

        // Add SceneTransitionManager and data managers
        var sceneManagerGO = new GameObject("SceneTransitionManager", typeof(NinjaFruit.UI.SceneTransitionManager));
        var highScoreGO = new GameObject("HighScoreManager", typeof(NinjaFruit.UI.HighScoreManager));
        var settingsGO = new GameObject("SettingsManager", typeof(NinjaFruit.UI.SettingsManager));

        // Add bootstrap to wire references at runtime
        var bootstrap = menuRoot.AddComponent<NinjaFruit.UI.MainMenuSceneBootstrap>();

        // Save MainMenu scene
        EditorSceneManager.SaveScene(mainScene, "Assets/Scenes/MainMenu.unity");

        // Create Gameplay scene
        var gameScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var camGO = new GameObject("Main Camera", typeof(Camera));
        camGO.tag = "MainCamera";

        // Simple UI to indicate gameplay
        var gCanvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        gCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        var txt = CreateUIText("GameplayLabel", "Gameplay Scene - Press Esc to return", gCanvas.transform);

        // Add gameplay controller
        var gameplayGO = new GameObject("GameplayController", typeof(NinjaFruit.UI.GameplayController));

        EditorSceneManager.SaveScene(gameScene, "Assets/Scenes/Gameplay.unity");

        AssetDatabase.Refresh();
        Debug.Log("Created MainMenu and Gameplay scenes at Assets/Scenes/");
    }

    private static GameObject CreateUIButton(string name, string label, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Button), typeof(Image));
        go.transform.SetParent(parent, false);
        var txt = new GameObject("Text", typeof(RectTransform), typeof(TMPro.TextMeshProUGUI));
        txt.transform.SetParent(go.transform, false);
        var t = txt.GetComponent<TMPro.TextMeshProUGUI>();
        t.text = label;
        t.alignment = TMPro.TextAlignmentOptions.Center;
        return go;
    }

    private static GameObject CreateUIText(string name, string text, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(TMPro.TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        var t = go.GetComponent<TMPro.TextMeshProUGUI>();
        t.text = text;
        t.alignment = TMPro.TextAlignmentOptions.Center;
        return go;
    }
}
