using System;
using UnityEngine;

namespace NinjaFruit.Gameplay
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public class GameStateController : MonoBehaviour
    {
        public event Action<GameState> OnStateChanged;
        public event Action<int> OnLivesChanged;

        public GameState CurrentState { get; private set; } = GameState.MainMenu;
        public int LivesRemaining { get; private set; } = 3;

        [SerializeField]
        private int startingLives = 3;

        public void StartGame()
        {
            LivesRemaining = startingLives;
            CurrentState = GameState.Playing;
            OnStateChanged?.Invoke(CurrentState);
            OnLivesChanged?.Invoke(LivesRemaining);
        }

        public void PauseGame()
        {
            if (CurrentState == GameState.Playing)
            {
                CurrentState = GameState.Paused;
                OnStateChanged?.Invoke(CurrentState);
            }
        }

        public void ResumeGame()
        {
            if (CurrentState == GameState.Paused)
            {
                CurrentState = GameState.Playing;
                OnStateChanged?.Invoke(CurrentState);
            }
        }

        public void RegisterMissedFruit()
        {
            if (CurrentState != GameState.Playing) return;

            LivesRemaining--;
            OnLivesChanged?.Invoke(LivesRemaining);

            if (LivesRemaining <= 0)
            {
                EndGame();
            }
        }

        public void EndGame()
        {
            CurrentState = GameState.GameOver;
            OnStateChanged?.Invoke(CurrentState);
        }
    }
}
