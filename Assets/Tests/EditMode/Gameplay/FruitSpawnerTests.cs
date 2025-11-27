using NUnit.Framework;
using UnityEngine;

namespace NinjaFruit.Tests.EditMode.Gameplay
{
    /// <summary>
    /// Edit Mode unit tests for FruitSpawner component
    /// Story: STORY-001 - FruitSpawner MVP
    /// Test Plan: test-plan-story-001-fruitspawner.md
    /// Test Spec: test-spec-story-001-fruitspawner.md
    /// </summary>
    [TestFixture]
    public class FruitSpawnerTests
    {
        private FruitSpawner spawner;

        [SetUp]
        public void Setup()
        {
            // Create test GameObject with FruitSpawner component
            GameObject spawnerObject = new GameObject("TestSpawner");
            spawner = spawnerObject.AddComponent<FruitSpawner>();
        }

        [TearDown]
        public void Teardown()
        {
            // Clean up test objects
            if (spawner != null && spawner.gameObject != null)
            {
                Object.DestroyImmediate(spawner.gameObject);
            }
        }

        #region SpawnIntervalCalculationTests

        /// <summary>
        /// TEST-001: CalculateSpawnInterval_ScoreZero_Returns2Seconds
        /// Priority: P0
        /// Risk: RISK-001 (HIGH)
        /// 
        /// Validates that at game start (score 0), spawn interval is 2.0 seconds.
        /// Formula: Max(0.3, 2.0 - (0 / 500)) = Max(0.3, 2.0) = 2.0
        /// </summary>
        [Test]
        public void CalculateSpawnInterval_ScoreZero_Returns2Seconds()
        {
            // Arrange
            int inputScore = 0;
            float expectedInterval = 2.0f;
            float tolerance = 0.001f;

            // Act
            float actualInterval = spawner.CalculateSpawnInterval(inputScore);

            // Assert
            Assert.AreEqual(
                expectedInterval,
                actualInterval,
                tolerance,
                "At score 0, spawn interval should be 2.0 seconds (max difficulty)"
            );
        }

        /// <summary>
        /// TEST-002: CalculateSpawnInterval_Score500_Returns1Second
        /// Priority: P0
        /// Risk: RISK-001 (HIGH)
        /// 
        /// Validates mid-game difficulty progression at score 500.
        /// Formula: Max(0.3, 2.0 - (500 / 500)) = Max(0.3, 1.0) = 1.0
        /// </summary>
        [Test]
        public void CalculateSpawnInterval_Score500_Returns1Second()
        {
            // Arrange
            int inputScore = 500;
            float expectedInterval = 1.0f;
            float tolerance = 0.001f;

            // Act
            float actualInterval = spawner.CalculateSpawnInterval(inputScore);

            // Assert
            Assert.AreEqual(
                expectedInterval,
                actualInterval,
                tolerance,
                "At score 500, spawn interval should be 1.0 seconds (mid-game difficulty)"
            );
        }

        /// <summary>
        /// TEST-003: CalculateSpawnInterval_Score1000_ReturnsMinimum0Point3Seconds
        /// Priority: P0
        /// Risk: RISK-001 (HIGH)
        /// 
        /// Validates maximum difficulty cap at minimum spawn interval.
        /// Formula: Max(0.3, 2.0 - (1000 / 500)) = Max(0.3, 0.0) = 0.3
        /// </summary>
        [Test]
        public void CalculateSpawnInterval_Score1000_ReturnsMinimum0Point3Seconds()
        {
            // Arrange
            int inputScore = 1000;
            float expectedInterval = 0.3f; // Minimum cap
            float tolerance = 0.001f;

            // Act
            float actualInterval = spawner.CalculateSpawnInterval(inputScore);

            // Assert
            Assert.AreEqual(
                expectedInterval,
                actualInterval,
                tolerance,
                "At score 1000, spawn interval should be capped at minimum 0.3 seconds"
            );
        }

        /// <summary>
        /// TEST-004: CalculateSpawnInterval_NegativeScore_ClampsTo2Seconds
        /// Priority: P0
        /// Risk: RISK-001 (HIGH)
        /// 
        /// Validates defensive programming: negative scores (from bomb penalties)
        /// should not reduce difficulty below initial state.
        /// Formula: Max(0.3, 2.0 - (-100 / 500)) = Max(0.3, 2.2) = 2.2 (or clamped to 2.0)
        /// </summary>
        [Test]
        public void CalculateSpawnInterval_NegativeScore_ClampsTo2Seconds()
        {
            // Arrange
            int inputScore = -100;
            float expectedMinInterval = 2.0f;

            // Act
            float actualInterval = spawner.CalculateSpawnInterval(inputScore);

            // Assert
            Assert.GreaterOrEqual(
                actualInterval,
                expectedMinInterval,
                "Negative scores should not reduce spawn interval below initial difficulty"
            );
        }

        #endregion

        #region FruitSpeedCalculationTests

        /// <summary>
        /// TEST-005: CalculateFruitSpeed_ScoreZero_Returns2MetersPerSecond
        /// Priority: P1
        /// Risk: RISK-002 (MEDIUM)
        /// 
        /// Validates initial fruit speed at game start.
        /// Formula: Min(7.0, 2.0 + (0 / 1000)) = Min(7.0, 2.0) = 2.0
        /// </summary>
        [Test]
        public void CalculateFruitSpeed_ScoreZero_Returns2MetersPerSecond()
        {
            // Arrange
            int inputScore = 0;
            float expectedSpeed = 2.0f;
            float tolerance = 0.001f;

            // Act
            float actualSpeed = spawner.CalculateFruitSpeed(inputScore);

            // Assert
            Assert.AreEqual(
                expectedSpeed,
                actualSpeed,
                tolerance,
                "At score 0, fruit speed should be 2.0 m/s (initial speed)"
            );
        }

        /// <summary>
        /// TEST-006: CalculateFruitSpeed_Score1000_Returns3MetersPerSecond
        /// Priority: P1
        /// Risk: RISK-002 (MEDIUM)
        /// 
        /// Validates mid-game fruit speed progression.
        /// Formula: Min(7.0, 2.0 + (1000 / 1000)) = Min(7.0, 3.0) = 3.0
        /// </summary>
        [Test]
        public void CalculateFruitSpeed_Score1000_Returns3MetersPerSecond()
        {
            // Arrange
            int inputScore = 1000;
            float expectedSpeed = 3.0f;
            float tolerance = 0.001f;

            // Act
            float actualSpeed = spawner.CalculateFruitSpeed(inputScore);

            // Assert
            Assert.AreEqual(
                expectedSpeed,
                actualSpeed,
                tolerance,
                "At score 1000, fruit speed should be 3.0 m/s"
            );
        }

        /// <summary>
        /// TEST-007: CalculateFruitSpeed_Score5000_ReturnsMaximum7MetersPerSecond
        /// Priority: P1
        /// Risk: RISK-002 (MEDIUM)
        /// 
        /// Validates maximum speed cap is reached.
        /// Formula: Min(7.0, 2.0 + (5000 / 1000)) = Min(7.0, 7.0) = 7.0
        /// </summary>
        [Test]
        public void CalculateFruitSpeed_Score5000_ReturnsMaximum7MetersPerSecond()
        {
            // Arrange
            int inputScore = 5000;
            float expectedSpeed = 7.0f; // Maximum cap
            float tolerance = 0.001f;

            // Act
            float actualSpeed = spawner.CalculateFruitSpeed(inputScore);

            // Assert
            Assert.AreEqual(
                expectedSpeed,
                actualSpeed,
                tolerance,
                "At score 5000, fruit speed should reach maximum cap of 7.0 m/s"
            );
        }

        /// <summary>
        /// TEST-008: CalculateFruitSpeed_Score10000_DoesNotExceed7MetersPerSecond
        /// Priority: P1
        /// Risk: RISK-002 (MEDIUM)
        /// 
        /// Validates maximum speed cap is strictly enforced at extreme scores.
        /// Formula: Min(7.0, 2.0 + (10000 / 1000)) = Min(7.0, 12.0) = 7.0
        /// </summary>
        [Test]
        public void CalculateFruitSpeed_Score10000_DoesNotExceed7MetersPerSecond()
        {
            // Arrange
            int inputScore = 10000;
            float expectedMaxSpeed = 7.0f;
            float tolerance = 0.001f;

            // Act
            float actualSpeed = spawner.CalculateFruitSpeed(inputScore);

            // Assert
            Assert.AreEqual(
                expectedMaxSpeed,
                actualSpeed,
                tolerance,
                "At extreme scores, fruit speed must not exceed 7.0 m/s cap"
            );
            Assert.LessOrEqual(
                actualSpeed,
                expectedMaxSpeed,
                "Speed cap enforcement validation"
            );
        }

        #endregion

        #region BombSpawnLogicTests

        /// <summary>
        /// TEST-009: ShouldSpawnBomb_FruitCount9_ReturnsFalse
        /// Priority: P1
        /// Risk: RISK-003 (MEDIUM)
        /// 
        /// Validates bomb does NOT spawn before 10-fruit threshold.
        /// MVP uses deterministic logic: 1 bomb per 10 fruits (10% rate).
        /// </summary>
        [Test]
        public void ShouldSpawnBomb_FruitCount9_ReturnsFalse()
        {
            // Arrange
            int fruitCount = 9;
            bool expectedResult = false;

            // Act
            bool shouldSpawn = spawner.ShouldSpawnBomb(fruitCount);

            // Assert
            Assert.IsFalse(
                shouldSpawn,
                "Bomb should not spawn before 10 fruits have spawned"
            );
        }

        /// <summary>
        /// TEST-010: ShouldSpawnBomb_FruitCount10_ReturnsTrue
        /// Priority: P1
        /// Risk: RISK-003 (MEDIUM)
        /// 
        /// Validates bomb DOES spawn at exactly 10-fruit threshold.
        /// MVP uses deterministic logic: 1 bomb per 10 fruits (10% rate).
        /// </summary>
        [Test]
        public void ShouldSpawnBomb_FruitCount10_ReturnsTrue()
        {
            // Arrange
            int fruitCount = 10;
            bool expectedResult = true;

            // Act
            bool shouldSpawn = spawner.ShouldSpawnBomb(fruitCount);

            // Assert
            Assert.IsTrue(
                shouldSpawn,
                "Bomb should spawn after exactly 10 fruits (10% rate)"
            );
        }

        #endregion
    }
}
