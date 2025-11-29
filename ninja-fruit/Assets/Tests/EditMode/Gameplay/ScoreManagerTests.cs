using NUnit.Framework;
using NinjaFruit.Gameplay;

namespace NinjaFruit.Tests.EditMode.Gameplay
{
    [TestFixture]
    public class ScoreManagerTests
    {
        private ScoreManager scoreManager;

        [SetUp]
        public void SetUp()
        {
            scoreManager = new UnityEngine.GameObject("ScoreManagerTest").AddComponent<ScoreManager>();
            scoreManager.ResetForTests();
            UnityEngine.PlayerPrefs.DeleteAll();
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(scoreManager.gameObject);
            UnityEngine.PlayerPrefs.DeleteAll();
        }

        [Test]
        public void AddPoints_UpdatesCurrentAndHighScore_AndFiresEvents()
        {
            int scoreEvent = -1;
            int highEvent = -1;
            scoreManager.OnScoreChanged += s => scoreEvent = s;
            scoreManager.OnHighScoreChanged += h => highEvent = h;

            scoreManager.AddPoints(50);

            Assert.AreEqual(50, scoreManager.CurrentScore);
            Assert.AreEqual(50, scoreManager.HighScore);
            Assert.AreEqual(50, scoreEvent);
            Assert.AreEqual(50, highEvent);

            // Adding lower points shouldn't change high score
            scoreManager.AddPoints(10);
            Assert.AreEqual(60, scoreManager.CurrentScore);
            Assert.AreEqual(60, scoreEvent);
            Assert.AreEqual(60, highEvent);
        }

        [Test]
        public void LoadHighScore_InvokesHighScoreChanged_WhenValueDifferent()
        {
            UnityEngine.PlayerPrefs.SetInt("HighScore", 123);
            UnityEngine.PlayerPrefs.Save();

            int seen = -1;
            scoreManager.OnHighScoreChanged += h => seen = h;

            scoreManager.LoadHighScore();

            Assert.AreEqual(123, scoreManager.HighScore);
            Assert.AreEqual(123, seen);
        }

        [Test]
        public void CalculatePoints_Apple_Returns10()
        {
            int pts = scoreManager.CalculatePoints(FruitType.Apple, 1, false);
            Assert.AreEqual(10, pts);
        }

        [Test]
        public void CalculatePoints_Orange_Golden_ReturnsDouble()
        {
            int pts = scoreManager.CalculatePoints(FruitType.Orange, 1, true);
            Assert.AreEqual(30, pts);
        }

        [Test]
        public void CalculatePoints_WithMultiplier_AppliesMultiplier()
        {
            int pts = scoreManager.CalculatePoints(FruitType.Watermelon, 3, false);
            Assert.AreEqual(60, pts);
        }

        [Test]
        public void CalculatePoints_NegativeMultiplier_ClampsToOne()
        {
            int pts = scoreManager.CalculatePoints(FruitType.Strawberry, 0, false);
            Assert.AreEqual(8, pts);
        }
    }
}
