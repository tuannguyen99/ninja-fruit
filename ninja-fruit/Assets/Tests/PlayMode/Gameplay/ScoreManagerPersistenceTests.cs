using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NinjaFruit.Gameplay;

namespace NinjaFruit.Tests.PlayMode.Gameplay
{
    [TestFixture]
    public class ScoreManagerPersistenceTests
    {
        private ScoreManager scoreManager;

        [SetUp]
        public void Setup()
        {
            PlayerPrefs.DeleteAll();
            var go = new GameObject("ScoreManagerPlayTest");
            scoreManager = go.AddComponent<ScoreManager>();
            scoreManager.ResetForTests();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(scoreManager.gameObject);
            PlayerPrefs.DeleteAll();
        }

        [UnityTest]
        public IEnumerator HighScore_Persisted_UpdatesPlayerPrefs()
        {
            scoreManager.AddPoints(100);
            scoreManager.SaveHighScore();
            yield return null;

            int stored = PlayerPrefs.GetInt("HighScore", 0);
            Assert.AreEqual(100, stored);
        }

        [UnityTest]
        public IEnumerator HighScore_NotOverwritten_WhenLower()
        {
            PlayerPrefs.SetInt("HighScore", 200);
            PlayerPrefs.Save();

            scoreManager.AddPoints(150);
            scoreManager.SaveHighScore();
            yield return null;

            int stored = PlayerPrefs.GetInt("HighScore", 0);
            Assert.AreEqual(200, stored);
        }
    }
}
