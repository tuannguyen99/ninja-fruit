using System;
using UnityEngine;

namespace NinjaFruit.Gameplay
{
    public enum FruitType { Apple, Banana, Orange, Strawberry, Watermelon }

    public class ScoreManager : MonoBehaviour
    {
        public event Action<int> OnScoreChanged;
        public event Action<int> OnHighScoreChanged;

        public int CurrentScore { get; private set; }
        public int HighScore { get; private set; }

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
