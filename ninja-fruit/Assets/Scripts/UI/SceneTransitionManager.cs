using UnityEngine;
using UnityEngine.SceneManagement;
using NinjaFruit.Interfaces;

namespace NinjaFruit.UI
{
    /// <summary>
    /// Handles scene loading and application quit
    /// </summary>
    public class SceneTransitionManager : MonoBehaviour, ISceneTransitionManager
    {
        private const string GAMEPLAY_SCENE = "Gameplay";
        private const string MAIN_MENU_SCENE = "MainMenu";
        
        /// <summary>
        /// Load the gameplay scene
        /// </summary>
        public void LoadGameplayScene()
        {
            SceneManager.LoadScene(GAMEPLAY_SCENE, LoadSceneMode.Single);
        }
        
        /// <summary>
        /// Load the main menu scene
        /// </summary>
        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(MAIN_MENU_SCENE, LoadSceneMode.Single);
        }
        
        /// <summary>
        /// Quit the application (only works in builds, not editor)
        /// </summary>
        public void QuitApplication()
        {
            // TODO: Implement quit logic
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
