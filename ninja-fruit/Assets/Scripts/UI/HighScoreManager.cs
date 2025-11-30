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
            HighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
            TotalFruitsSliced = PlayerPrefs.GetInt(TOTAL_FRUITS_KEY, 0);
            LongestCombo = PlayerPrefs.GetInt(LONGEST_COMBO_KEY, 0);
        }
        
        /// <summary>
        /// Save high score (only if higher than current)
        /// </summary>
        public void SaveHighScore(int score)
        {
            if (score > HighScore)
            {
                HighScore = score;
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
                PlayerPrefs.Save();
            }
        }
        
        /// <summary>
        /// Add to total fruits sliced count (accumulative)
        /// </summary>
        public void SaveFruitCount(int count)
        {
            TotalFruitsSliced += count;
            PlayerPrefs.SetInt(TOTAL_FRUITS_KEY, TotalFruitsSliced);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Save longest combo (only if higher than current)
        /// </summary>
        public void SaveCombo(int combo)
        {
            if (combo > LongestCombo)
            {
                LongestCombo = combo;
                PlayerPrefs.SetInt(LONGEST_COMBO_KEY, combo);
                PlayerPrefs.Save();
            }
        }
        
        /// <summary>
        /// Reset all scores to zero (for testing)
        /// </summary>
        public void ResetScores()
        {
            PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
            PlayerPrefs.DeleteKey(TOTAL_FRUITS_KEY);
            PlayerPrefs.DeleteKey(LONGEST_COMBO_KEY);
            PlayerPrefs.Save();
            LoadScores();
        }
    }
}
