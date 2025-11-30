using NinjaFruit.Interfaces;

namespace NinjaFruit.Tests.Mocks
{
    /// <summary>
    /// Mock implementation of ISceneTransitionManager for testing
    /// </summary>
    public class MockSceneTransitionManager : ISceneTransitionManager
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
}
