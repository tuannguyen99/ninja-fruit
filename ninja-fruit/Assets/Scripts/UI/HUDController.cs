using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NinjaFruit.Gameplay;

namespace NinjaFruit.UI
{
    public class HUDController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI comboText;
        [SerializeField] private Image[] lifeHearts;
        [SerializeField] private Sprite heartFull;
        [SerializeField] private Sprite heartEmpty;
        
        private ScoreManager scoreManager;
        private GameStateController gameStateController;
        
        // Public API for testing
        public TextMeshProUGUI ScoreText => scoreText;
        public TextMeshProUGUI ComboText => comboText;
        public Image[] LifeHearts => lifeHearts;
        
        private void Awake()
        {
            // Find managers in scene if not already set
            if (scoreManager == null)
                scoreManager = FindObjectOfType<ScoreManager>();
            if (gameStateController == null)
                gameStateController = FindObjectOfType<GameStateController>();
        }
        
        // Allow manual manager injection for testing
        public void SetManagers(ScoreManager score, GameStateController gameState)
        {
            scoreManager = score;
            gameStateController = gameState;
        }
        
        private void OnEnable()
        {
            if (scoreManager != null)
            {
                scoreManager.OnScoreChanged += UpdateScoreDisplay;
                scoreManager.OnComboChanged += UpdateComboDisplay;
                // Sync to current state when re-enabling
                UpdateScoreDisplay(scoreManager.CurrentScore);
                UpdateComboDisplay(scoreManager.ComboMultiplier);
            }
            
            if (gameStateController != null)
            {
                gameStateController.OnLivesChanged += UpdateLivesDisplay;
                // Sync to current state when re-enabling
                UpdateLivesDisplay(gameStateController.LivesRemaining);
            }
        }
        
        private void OnDisable()
        {
            if (scoreManager != null)
            {
                scoreManager.OnScoreChanged -= UpdateScoreDisplay;
                scoreManager.OnComboChanged -= UpdateComboDisplay;
            }
            
            if (gameStateController != null)
            {
                gameStateController.OnLivesChanged -= UpdateLivesDisplay;
            }
        }
        
        public void Initialize()
        {
            UpdateScoreDisplay(0);
            UpdateLivesDisplay(3);
            UpdateComboDisplay(1);
        }
        
        private void UpdateScoreDisplay(int newScore)
        {
            if (scoreText != null)
            {
                scoreText.text = newScore.ToString();
            }
        }
        
        private void UpdateComboDisplay(int multiplier)
        {
            if (comboText == null) return;
            
            if (multiplier <= 1)
            {
                comboText.gameObject.SetActive(false);
            }
            else
            {
                comboText.gameObject.SetActive(true);
                comboText.text = $"COMBO {multiplier}x!";
            }
        }
        
        private void UpdateLivesDisplay(int livesRemaining)
        {
            if (lifeHearts == null) return;
            
            for (int i = 0; i < lifeHearts.Length; i++)
            {
                if (i < livesRemaining)
                {
                    if (heartFull != null) lifeHearts[i].sprite = heartFull;
                    lifeHearts[i].color = Color.white;
                }
                else
                {
                    if (heartEmpty != null) lifeHearts[i].sprite = heartEmpty;
                    lifeHearts[i].color = new Color(1f, 1f, 1f, 0.3f);
                }
            }
        }
        
        // Test helper methods
        public string GetScoreText() => scoreText?.text ?? "";
        public string GetComboText() => comboText?.text ?? "";
        public bool IsComboVisible() => comboText != null && comboText.gameObject.activeSelf;
        
        public int GetVisibleHearts()
        {
            if (lifeHearts == null) return 0;
            
            int count = 0;
            foreach (var heart in lifeHearts)
            {
                if (heart != null && heart.color.a > 0.5f)
                    count++;
            }
            return count;
        }
        
        // Allow manual setting for tests
        public void SetReferences(TextMeshProUGUI score, TextMeshProUGUI combo, Image[] hearts)
        {
            scoreText = score;
            comboText = combo;
            lifeHearts = hearts;
        }
    }
}
