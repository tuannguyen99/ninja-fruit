using System;
using UnityEngine;

namespace NinjaFruit.Gameplay
{
    public enum FruitType { Apple, Banana, Orange, Strawberry, Watermelon }

    public class ScoreManager : MonoBehaviour
    {
        public event Action<int> OnScoreChanged;
        public event Action<int> OnHighScoreChanged;
        public event Action<int> OnComboChanged;

        public int CurrentScore { get; private set; }
        public int HighScore { get; private set; }
        public int ComboMultiplier { get; private set; } = 1;

        [SerializeField]
        private float comboWindow = 1.5f;

        [SerializeField]
        private int maxComboMultiplier = 5;

        private float lastSliceTimestamp = -Mathf.Infinity;

        private const string HighScoreKey = "HighScore";

        public void Awake()
        {
            LoadHighScore();
        }

        public int CalculatePoints(FruitType fruitType, int multiplier = 1, bool isGolden = false)
        {
            if (multiplier < 1) multiplier = 1;

            int basePoints = fruitType switch
            {
                FruitType.Apple => 10,
                FruitType.Banana => 10,
                FruitType.Orange => 15,
                FruitType.Strawberry => 8,
                FruitType.Watermelon => 20,
                _ => 0
            };

            int points = basePoints * multiplier;
            if (isGolden) points *= 2;

            return points;
        }

        public void AddPoints(int pts)
        {
            CurrentScore += pts;
            OnScoreChanged?.Invoke(CurrentScore);
            if (CurrentScore > HighScore)
            {
                HighScore = CurrentScore;
                OnHighScoreChanged?.Invoke(HighScore);
            }
        }

        public int RegisterSlice(FruitType fruitType, bool isGolden = false, float timestamp = -1f)
        {
            if (timestamp < 0f) timestamp = Time.time;

            // Determine combo
            if (timestamp - lastSliceTimestamp < comboWindow)
            {
                ComboMultiplier = Mathf.Min(maxComboMultiplier, ComboMultiplier + 1);
            }
            else
            {
                ComboMultiplier = 1;
            }

            lastSliceTimestamp = timestamp;
            OnComboChanged?.Invoke(ComboMultiplier);

            int pts = CalculatePoints(fruitType, ComboMultiplier, isGolden);
            AddPoints(pts);
            return pts;
        }

        public void RegisterBombHit()
        {
            CurrentScore -= 50;
            OnScoreChanged?.Invoke(CurrentScore);
            ComboMultiplier = 1;
            OnComboChanged?.Invoke(ComboMultiplier);
        }

        public void SaveHighScore()
        {
            int stored = PlayerPrefs.GetInt(HighScoreKey, 0);
            if (HighScore > stored)
            {
                PlayerPrefs.SetInt(HighScoreKey, HighScore);
                PlayerPrefs.Save();
            }
        }

        public void LoadHighScore()
        {
            int prev = HighScore;
            HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
            if (HighScore != prev)
            {
                OnHighScoreChanged?.Invoke(HighScore);
            }
        }

        public void ResetForTests()
        {
            CurrentScore = 0;
            HighScore = 0;
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            LoadHighScore();
        }
    }
}
