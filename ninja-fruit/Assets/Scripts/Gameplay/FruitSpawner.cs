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

        [Header("Golden Fruit")]
        [Tooltip("Chance for a spawned fruit to be golden (0..1)")]
        [SerializeField] private float goldenChance = 0.05f;

        [Header("Spawn Configuration")]
        [SerializeField] private Vector2 spawnPosition = new Vector2(0, -5);
        [SerializeField] private int currentScore = 0;
        [Header("Prefab Resources")]
        [Tooltip("Resource path (under Assets/Resources) for fruit prefab folder, example: Prefabs/FruitPrefab")]
        [SerializeField] private string fruitPrefabResourcePath = "Prefabs/FruitPrefab";
        [Tooltip("Resource path for bomb prefab")]
        [SerializeField] private string bombPrefabResourcePath = "Prefabs/BombPrefab";

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
        /// Testable helper: determine if a given roll (0..1) counts as golden
        /// Uses strict < comparison to match gameplay semantics
        /// </summary>
        public bool IsRollGolden(float roll)
        {
            return roll < goldenChance;
        }

        /// <summary>
        /// Spawn a fruit prefab with initial velocity
        /// </summary>
        public void SpawnFruit()
        {
            if (fruitPrefabs == null || fruitPrefabs.Length == 0)
            {
                // Try resource-based prefab as fallback
                var loaded = Resources.Load<GameObject>(fruitPrefabResourcePath);
                if (loaded == null)
                {
                    Debug.LogError("No fruit prefabs assigned to FruitSpawner and no resource prefab found at " + fruitPrefabResourcePath);
                    return;
                }
                GameObject fruit = Instantiate(loaded, spawnPosition, Quaternion.identity);
                FinalizeSpawnedFruit(fruit);
                return;
            }

            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
            GameObject fruitObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
            FinalizeSpawnedFruit(fruitObj);
        }

        private void FinalizeSpawnedFruit(GameObject fruit)
        {
            // Decide golden
            bool isGolden = Random.value < goldenChance;

            // Attach Fruit component (for runtime/tests) and set metadata
            var fruitComp = fruit.GetComponent<NinjaFruit.Gameplay.Fruit>();
            bool hadFruitComp = fruitComp != null;
            if (!hadFruitComp) fruitComp = fruit.AddComponent<NinjaFruit.Gameplay.Fruit>();
            fruitComp.IsGolden = isGolden;
            // If the prefab already specified a Fruit.Type, preserve it; otherwise default to Apple
            if (!hadFruitComp)
            {
                fruitComp.Type = NinjaFruit.Gameplay.FruitType.Apple;
            }

            Rigidbody2D rb = fruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float speed = CalculateFruitSpeed(currentScore);
                float horizontalDirection = Random.Range(-1f, 1f);
                Vector2 velocity = new Vector2(horizontalDirection, 1f).normalized * speed;
                // Set the common velocity property
                rb.linearVelocity = velocity;

                // Some Unity versions exposed 'linearVelocity' historically; attempt to set via reflection
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

        /// <summary>
        /// Spawn a bomb either from resources or programmatically when prefab is missing.
        /// Calls out Bomb component and sets up collider/rigidbody.
        /// </summary>
        public void SpawnBomb()
        {
            GameObject bombObj = null;

            // Try resource prefab first
            var prefab = Resources.Load<GameObject>(bombPrefabResourcePath);
            if (prefab != null)
            {
                bombObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                // Create programmatic bomb GameObject
                bombObj = new GameObject("Bomb");
                bombObj.transform.position = spawnPosition;
                var cc = bombObj.AddComponent<CircleCollider2D>();
                cc.radius = 1.0f;
                var rb = bombObj.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            if (bombObj != null)
            {
                var bombComp = bombObj.GetComponent<NinjaFruit.Gameplay.Bomb>();
                if (bombComp == null) bombComp = bombObj.AddComponent<NinjaFruit.Gameplay.Bomb>();
            }
        }
    }
}
