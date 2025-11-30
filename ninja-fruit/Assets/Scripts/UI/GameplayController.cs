using UnityEngine;
using UnityEngine.SceneManagement;

namespace NinjaFruit.UI
{
    /// <summary>
    /// Simple gameplay controller for the demo scene.
    /// Press Escape to return to MainMenu scene.
    /// </summary>
    public class GameplayController : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
