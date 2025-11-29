using NUnit.Framework;
using NinjaFruit.Gameplay;
using UnityEngine;

namespace NinjaFruit.Tests.EditMode.Gameplay
{
    [TestFixture]
    public class ScoreManagerComboTests
    {
        private ScoreManager scoreManager;

        [SetUp]
        public void SetUp()
        {
            scoreManager = new GameObject("ScoreManagerComboTest").AddComponent<ScoreManager>();
            scoreManager.ResetForTests();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(scoreManager.gameObject);
            PlayerPrefs.DeleteAll();
        }

        [Test]
        public void RegisterSlice_IncrementsComboWithinWindow()
        {
            float t0 = 0f;
            int p1 = scoreManager.RegisterSlice(FruitType.Apple, false, t0);
            Assert.AreEqual(1, scoreManager.ComboMultiplier);

            int p2 = scoreManager.RegisterSlice(FruitType.Banana, false, t0 + 0.5f);
            Assert.AreEqual(2, scoreManager.ComboMultiplier);

            int p3 = scoreManager.RegisterSlice(FruitType.Orange, false, t0 + 1.0f);
            Assert.AreEqual(3, scoreManager.ComboMultiplier);
        }

        [Test]
        public void RegisterSlice_ResetsComboAfterWindowExpires()
        {
            float t0 = 0f;
            scoreManager.RegisterSlice(FruitType.Apple, false, t0);
            scoreManager.RegisterSlice(FruitType.Banana, false, t0 + 0.5f);
            Assert.AreEqual(2, scoreManager.ComboMultiplier);

            scoreManager.RegisterSlice(FruitType.Orange, false, t0 + 2.0f);
            Assert.AreEqual(1, scoreManager.ComboMultiplier);
        }

        [Test]
        public void RegisterSlice_ExactBoundary_ResetsCombo()
        {
            float t0 = 0f;
            // First slice
            scoreManager.RegisterSlice(FruitType.Apple, false, t0);
            Assert.AreEqual(1, scoreManager.ComboMultiplier);

            // Next slice occurs exactly at comboWindow after the previous slice
            // With strict < semantics the combo should reset
            scoreManager.RegisterSlice(FruitType.Orange, false, t0 + 1.5f);
            Assert.AreEqual(1, scoreManager.ComboMultiplier);
        }

        [Test]
        public void ComboCapsAtMaxMultiplier()
        {
            float t0 = 0f;
            scoreManager.RegisterSlice(FruitType.Apple, false, t0);
            scoreManager.RegisterSlice(FruitType.Apple, false, t0 + 0.2f);
            scoreManager.RegisterSlice(FruitType.Apple, false, t0 + 0.4f);
            scoreManager.RegisterSlice(FruitType.Apple, false, t0 + 0.6f);
            scoreManager.RegisterSlice(FruitType.Apple, false, t0 + 0.8f);
            // default maxComboMultiplier is 5
            Assert.AreEqual(5, scoreManager.ComboMultiplier);
        }

        [Test]
        public void RegisterBombHit_ResetsComboAndAppliesPenalty()
        {
            float t0 = 0f;
            scoreManager.RegisterSlice(FruitType.Apple, false, t0);
            scoreManager.RegisterSlice(FruitType.Banana, false, t0 + 0.5f);
            Assert.AreEqual(2, scoreManager.ComboMultiplier);

            int before = scoreManager.CurrentScore;
            scoreManager.RegisterBombHit();
            Assert.AreEqual(1, scoreManager.ComboMultiplier);
            Assert.AreEqual(before - 50, scoreManager.CurrentScore);
        }
    }
}
