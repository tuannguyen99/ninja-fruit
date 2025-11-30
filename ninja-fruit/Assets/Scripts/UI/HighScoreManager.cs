using UnityEngine;

namespace NinjaFruit.UI
{
    /// <summary>
    /// Manages high score persistence using PlayerPrefs
    /// </summary>
    public class HighScoreManager : MonoBehaviour
    {
        // PlayerPrefs keys
        private const string HIGH_SCORE_KEY = "HighScore";
        private const string TOTAL_FRUITS_KEY = "TotalFruitsSliced";
        private const string LONGEST_COMBO_KEY = "LongestCombo";
        
        // Properties
        public int HighScore { get; private set; }
        public int TotalFruitsSliced { get; private set; }
        public int LongestCombo { get; private set; }
        
        /// <summary>
        /// Load high scores from PlayerPrefs
        /// </summary>
        public void LoadScores()
        {
            // TODO: Implement PlayerPrefs loading logic
            HighScore = 0;
            TotalFruitsSliced = 0;
            LongestCombo = 0;
        }
        
        /// <summary>
        /// Save high score (only if higher than current)
        /// </summary>
        public void SaveHighScore(int score)
        {
            // TODO: Implement high score save logic
            // Should only update if score > HighScore
        }
        
        /// <summary>
        /// Add to total fruits sliced count (accumulative)
        /// </summary>
        public void SaveFruitCount(int count)
        {
            // TODO: Implement fruit count accumulation logic
        }
        
        /// <summary>
        /// Save longest combo (only if higher than current)
        /// </summary>
        public void SaveCombo(int combo)
        {
            // TODO: Implement combo save logic
        }
        
        /// <summary>
        /// Reset all scores to zero (for testing)
        /// </summary>
        public void ResetScores()
        {
            // TODO: Implement reset logic
            PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
            PlayerPrefs.DeleteKey(TOTAL_FRUITS_KEY);
            PlayerPrefs.DeleteKey(LONGEST_COMBO_KEY);
            PlayerPrefs.Save();
            LoadScores();
        }
    }
}
