using UnityEngine;

namespace NinjaFruit
{
    /// <summary>
    /// FruitSpawner component - spawns fruit prefabs with difficulty scaling
    /// Story: STORY-001 - FruitSpawner MVP
    /// 
    /// This is a STUB implementation for test scaffolding demonstration.
    /// Replace this with actual implementation following TDD workflow.
    /// </summary>
    public class FruitSpawner : MonoBehaviour
    {
        [Header("Fruit Prefabs")]
        [SerializeField] private GameObject[] fruitPrefabs;

        [Header("Bomb Configuration")]
        [SerializeField] private int bombSpawnRate = 10; // 1 bomb per 10 fruits (10%)

        [Header("Spawn Configuration")]
        [SerializeField] private Vector2 spawnPosition = new Vector2(0, -5);
        [SerializeField] private int currentScore = 0;

        /// <summary>
        /// Calculate spawn interval based on current score
        /// Formula: Max(0.3, 2.0 - (score / 500))
        /// </summary>
        public float CalculateSpawnInterval(int score)
        {
            float interval = 2.0f - (score / 500f);
            return Mathf.Max(0.3f, interval);
        }

        /// <summary>
        /// Calculate fruit speed based on current score
        /// Formula: Min(7.0, 2.0 + (score / 1000))
        /// </summary>
        public float CalculateFruitSpeed(int score)
        {
            float speed = 2.0f + (score / 1000f);
            return Mathf.Min(7.0f, speed);
        }

        /// <summary>
        /// Determine if a bomb should spawn based on fruit count
        /// MVP: Deterministic logic (every 10th fruit)
        /// </summary>
        public bool ShouldSpawnBomb(int fruitCount)
        {
            return fruitCount > 0 && fruitCount % bombSpawnRate == 0;
        }

        /// <summary>
        /// Spawn a fruit prefab with initial velocity
        /// </summary>
        public void SpawnFruit()
        {
            if (fruitPrefabs == null || fruitPrefabs.Length == 0)
            {
                Debug.LogError("No fruit prefabs assigned to FruitSpawner");
                return;
            }

            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
            GameObject fruit = Instantiate(prefab, spawnPosition, Quaternion.identity);
            fruit.tag = "Fruit";

            Rigidbody2D rb = fruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float speed = CalculateFruitSpeed(currentScore);
                float horizontalDirection = Random.Range(-1f, 1f);
                Vector2 velocity = new Vector2(horizontalDirection, 1f).normalized * speed;
                // Set the common velocity property
                rb.linearVelocity = velocity;

                // Some Unity versions expose 'linearVelocity' (tests reference it). Try to set it too.
                var prop = rb.GetType().GetProperty("linearVelocity");
                if (prop != null && prop.CanWrite)
                {
                    try
                    {
                        prop.SetValue(rb, velocity, null);
                    }
                    catch { /* ignore if assignment fails */ }
                }
            }
        }
    }
}
