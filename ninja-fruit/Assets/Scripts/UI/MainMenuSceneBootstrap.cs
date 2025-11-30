using UnityEngine;
using UnityEngine.SceneManagement;

namespace NinjaFruit.UI
{
    /// <summary>
    /// Wires scene objects at runtime created by the editor tool.
    /// </summary>
    public class MainMenuSceneBootstrap : MonoBehaviour
    {
        private MainMenuController menu;

        void Awake()
        {
            menu = GetComponent<MainMenuController>();

            // Find managers in scene
            var sceneManager = FindObjectOfType<SceneTransitionManager>();
            var hs = FindObjectOfType<HighScoreManager>();
            var settings = FindObjectOfType<SettingsManager>();

            if (menu != null)
            {
                menu.SetSceneManager(sceneManager);
                menu.SetHighScoreManager(hs);
                menu.SetSettingsManager(settings);
            }
        }
    }
}
