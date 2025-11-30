using UnityEngine;
using NinjaFruit.Gameplay;

namespace NinjaFruit
{
    /// <summary>
    /// GameManager - Orchestrates the gameplay loop
    /// Connects all the managers and handles game flow
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Manager References")]
        [SerializeField] private FruitSpawner fruitSpawner;
        [SerializeField] private SwipeDetector swipeDetector;
        [SerializeField] private CollisionManager collisionManager;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private GameStateController gameStateController;

        [Header("Spawn Settings")]
        [SerializeField] private float spawnInterval = 1.5f;
        [SerializeField] private bool autoSpawn = true;
        
        private float spawnTimer = 0f;
        private int fruitsSpawned = 0;

        private void Awake()
        {
            // Find components if not assigned
            if (fruitSpawner == null) fruitSpawner = FindObjectOfType<FruitSpawner>();
            if (swipeDetector == null) swipeDetector = FindObjectOfType<SwipeDetector>();
            if (collisionManager == null) collisionManager = FindObjectOfType<CollisionManager>();
            if (scoreManager == null) scoreManager = FindObjectOfType<ScoreManager>();
            if (gameStateController == null) gameStateController = FindObjectOfType<GameStateController>();

            // Wire up swipe events
            if (swipeDetector != null && collisionManager != null && scoreManager != null)
            {
                swipeDetector.OnSwipeDetected += OnSwipeDetected;
            }

            // Start the game automatically
            if (gameStateController != null)
            {
                gameStateController.StartGame();
            }
        }

        private void Update()
        {
            if (gameStateController == null || gameStateController.CurrentState != GameState.Playing)
                return;

            // Auto-spawn fruits
            if (autoSpawn && fruitSpawner != null)
            {
                spawnTimer += Time.deltaTime;
                
                // Calculate spawn interval based on score
                float currentInterval = fruitSpawner.CalculateSpawnInterval(scoreManager != null ? scoreManager.CurrentScore : 0);
                
                if (spawnTimer >= currentInterval)
                {
                    spawnTimer = 0f;
                    fruitsSpawned++;
                    
                    // Spawn bomb every 10th fruit
                    if (fruitSpawner.ShouldSpawnBomb(fruitsSpawned))
                    {
                        fruitSpawner.SpawnBomb();
                    }
                    else
                    {
                        fruitSpawner.SpawnFruit();
                    }
                }
            }

            // Check for fruits that fell off screen (missed)
            CheckMissedFruits();
        }

        private void OnSwipeDetected(Vector2 start, Vector2 end)
        {
            if (collisionManager != null && scoreManager != null)
            {
                collisionManager.HandleSwipe(start, end, scoreManager);
            }
        }

        private void CheckMissedFruits()
        {
            // Find all fruits below a certain Y position (fell off screen)
            var fruits = FindObjectsOfType<NinjaFruit.Gameplay.Fruit>();
            foreach (var fruit in fruits)
            {
                if (fruit.transform.position.y < -6f) // Below spawn position
                {
                    gameStateController.RegisterMissedFruit();
                    Destroy(fruit.gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            if (swipeDetector != null)
            {
                swipeDetector.OnSwipeDetected -= OnSwipeDetected;
            }
        }
    }
}
