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

        /// <summary>
        /// Calculate spawn interval based on current score
        /// Formula: Max(0.3, 2.0 - (score / 500))
        /// </summary>
        public float CalculateSpawnInterval(int score)
        {
            // TODO: Implement formula from GDD
            // Formula: Max(0.3s, 2.0s - (score / 500))
            throw new System.NotImplementedException("Implement spawn interval calculation");
        }

        /// <summary>
        /// Calculate fruit speed based on current score
        /// Formula: Min(7.0, 2.0 + (score / 1000))
        /// </summary>
        public float CalculateFruitSpeed(int score)
        {
            // TODO: Implement formula from GDD
            // Formula: Min(7m/s, 2m/s + (score / 1000))
            throw new System.NotImplementedException("Implement fruit speed calculation");
        }

        /// <summary>
        /// Determine if a bomb should spawn based on fruit count
        /// MVP: Deterministic logic (every 10th fruit)
        /// </summary>
        public bool ShouldSpawnBomb(int fruitCount)
        {
            // TODO: Implement bomb spawn logic
            // MVP: Return true every 10th fruit (10% rate)
            throw new System.NotImplementedException("Implement bomb spawn logic");
        }

        /// <summary>
        /// Spawn a fruit prefab with initial velocity
        /// </summary>
        public void SpawnFruit()
        {
            // TODO: Implement fruit spawning
            // 1. Select random fruit prefab
            // 2. Instantiate at spawn position
            // 3. Apply tag "Fruit"
            // 4. Apply initial velocity using CalculateFruitSpeed()
            throw new System.NotImplementedException("Implement fruit spawning");
        }
    }
}
