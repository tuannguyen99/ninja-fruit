using NUnit.Framework;
using UnityEngine;
using NinjaFruit.UI;

namespace NinjaFruit.Tests.EditMode.UI
{
    /// <summary>
    /// Edit Mode tests for HighScoreManager
    /// </summary>
    [TestFixture]
    public class HighScoreManagerTests
    {
        [TearDown]
        public void TearDown()
        {
            // Clean up PlayerPrefs after each test
            PlayerPrefs.DeleteAll();
        }
        
        [Test]
        public void HighScore_LoadsDefaultOnFirstLaunch_ReturnsZero()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var managerGO = new GameObject("HighScoreManager");
            var manager = managerGO.AddComponent<HighScoreManager>();
            
            // Act
            manager.LoadScores();
            
            // Assert
            Assert.AreEqual(0, manager.HighScore);
            
            // Cleanup
            Object.DestroyImmediate(managerGO);
        }
        
        [Test]
        public void HighScore_SavesAndLoadsCorrectly_PersistsValue()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var manager1GO = new GameObject("HighScoreManager1");
            var manager1 = manager1GO.AddComponent<HighScoreManager>();
            
            // Act
            manager1.SaveHighScore(1250);
            Object.DestroyImmediate(manager1GO);
            
            var manager2GO = new GameObject("HighScoreManager2");
            var manager2 = manager2GO.AddComponent<HighScoreManager>();
            manager2.LoadScores();
            
            // Assert
            Assert.AreEqual(1250, manager2.HighScore);
            
            // Cleanup
            Object.DestroyImmediate(manager2GO);
        }
        
        [Test]
        public void HighScore_OnlyUpdatesIfHigher_IgnoresLowerScores()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var managerGO = new GameObject("HighScoreManager");
            var manager = managerGO.AddComponent<HighScoreManager>();
            
            // Act
            manager.SaveHighScore(1000);
            manager.SaveHighScore(500); // Lower score
            manager.LoadScores();
            
            // Assert
            Assert.AreEqual(1000, manager.HighScore, "High score should not be overwritten by lower score");
            
            // Cleanup
            Object.DestroyImmediate(managerGO);
        }
        
        [Test]
        public void TotalFruitsSliced_Accumulates_AddsToPrevious()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var managerGO = new GameObject("HighScoreManager");
            var manager = managerGO.AddComponent<HighScoreManager>();
            
            // Act
            manager.SaveFruitCount(50);
            manager.SaveFruitCount(75); // Should add, not replace
            manager.LoadScores();
            
            // Assert
            Assert.AreEqual(125, manager.TotalFruitsSliced, "Fruit count should accumulate");
            
            // Cleanup
            Object.DestroyImmediate(managerGO);
        }
    }
}
