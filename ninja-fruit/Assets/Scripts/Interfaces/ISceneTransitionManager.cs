using UnityEngine;

namespace NinjaFruit.Interfaces
{
    /// <summary>
    /// Interface for scene transition management (enables mocking in tests)
    /// </summary>
    public interface ISceneTransitionManager
    {
        void LoadGameplayScene();
        void LoadMainMenuScene();
        void QuitApplication();
    }
}
