using NUnit.Framework;
using UnityEngine;
using NinjaFruit.UI;

namespace NinjaFruit.Tests.EditMode.UI
{
    /// <summary>
    /// Edit Mode tests for SettingsManager
    /// </summary>
    [TestFixture]
    public class SettingsManagerTests
    {
        [TearDown]
        public void TearDown()
        {
            // Clean up PlayerPrefs after each test
            PlayerPrefs.DeleteAll();
        }
        
        [Test]
        public void Settings_LoadDefaultValues_ReturnsExpectedDefaults()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var managerGO = new GameObject("SettingsManager");
            var manager = managerGO.AddComponent<SettingsManager>();
            
            // Act
            manager.LoadSettings();
            
            // Assert
            Assert.AreEqual(0.8f, manager.MasterVolume, 0.01f, "Default volume should be 0.8");
            Assert.IsTrue(manager.SoundEffectsEnabled, "Sound effects should be enabled by default");
            Assert.IsTrue(manager.MusicEnabled, "Music should be enabled by default");
            
            // Cleanup
            Object.DestroyImmediate(managerGO);
        }
        
        [Test]
        public void MasterVolume_SavesAndLoads_PersistsCorrectly()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var manager1GO = new GameObject("SettingsManager1");
            var manager1 = manager1GO.AddComponent<SettingsManager>();
            
            // Act
            manager1.SetMasterVolume(0.5f);
            manager1.SaveSettings();
            Object.DestroyImmediate(manager1GO);
            
            var manager2GO = new GameObject("SettingsManager2");
            var manager2 = manager2GO.AddComponent<SettingsManager>();
            manager2.LoadSettings();
            
            // Assert
            Assert.AreEqual(0.5f, manager2.MasterVolume, 0.01f, "Volume should persist");
            
            // Cleanup
            Object.DestroyImmediate(manager2GO);
        }
        
        [Test]
        public void SoundEffectsToggle_SavesAndLoads_PersistsState()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var manager1GO = new GameObject("SettingsManager1");
            var manager1 = manager1GO.AddComponent<SettingsManager>();
            
            // Act
            manager1.SetSoundEffects(false);
            manager1.SaveSettings();
            Object.DestroyImmediate(manager1GO);
            
            var manager2GO = new GameObject("SettingsManager2");
            var manager2 = manager2GO.AddComponent<SettingsManager>();
            manager2.LoadSettings();
            
            // Assert
            Assert.IsFalse(manager2.SoundEffectsEnabled, "Sound effects toggle should persist");
            
            // Cleanup
            Object.DestroyImmediate(manager2GO);
        }
        
        [Test]
        public void MusicToggle_SavesAndLoads_PersistsState()
        {
            // Arrange
            PlayerPrefs.DeleteAll();
            var manager1GO = new GameObject("SettingsManager1");
            var manager1 = manager1GO.AddComponent<SettingsManager>();
            
            // Act
            manager1.SetMusic(false);
            manager1.SaveSettings();
            Object.DestroyImmediate(manager1GO);
            
            var manager2GO = new GameObject("SettingsManager2");
            var manager2 = manager2GO.AddComponent<SettingsManager>();
            manager2.LoadSettings();
            
            // Assert
            Assert.IsFalse(manager2.MusicEnabled, "Music toggle should persist");
            
            // Cleanup
            Object.DestroyImmediate(manager2GO);
        }
    }
}
