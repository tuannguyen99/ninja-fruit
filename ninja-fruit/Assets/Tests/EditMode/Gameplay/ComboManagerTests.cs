using NUnit.Framework;
using UnityEngine;
using NinjaFruit.Gameplay;

namespace NinjaFruit.Tests.EditMode.Gameplay
{
    [TestFixture]
    public class ComboManagerTests
    {
        private ScoreManager scoreManager;

        [SetUp]
        public void SetUp()
        {
            var go = new GameObject("ScoreManagerTest");
            scoreManager = go.AddComponent<ScoreManager>();
            scoreManager.ResetForTests();
        }

        [TearDown]
        public void TearDown()
        {
            if (scoreManager != null && scoreManager.gameObject != null)
                Object.DestroyImmediate(scoreManager.gameObject);
        }

        [Test]
        public void TC_Unit_Combo_IncreaseOnConsecutiveSlices_ReturnsIncrement()
        {
            float t0 = 0f;
            float t1 = t0 + 0.5f; // within combo window (default 1.5s)
            float t2 = t1 + 0.5f;

            int pts1 = scoreManager.RegisterSlice(FruitType.Apple, false, t0);
            int comboAfter1 = scoreManager.ComboMultiplier;

            int pts2 = scoreManager.RegisterSlice(FruitType.Apple, false, t1);
            int comboAfter2 = scoreManager.ComboMultiplier;

            int pts3 = scoreManager.RegisterSlice(FruitType.Apple, false, t2);
            int comboAfter3 = scoreManager.ComboMultiplier;

            Assert.AreEqual(1, comboAfter1, "First slice should start combo at 1");
            Assert.AreEqual(2, comboAfter2, "Second slice within window should increment combo");
            Assert.AreEqual(3, comboAfter3, "Third slice within window should increment combo again");
            Assert.Greater(pts2, pts1, "Points should increase with multiplier");
            Assert.Greater(pts3, pts2, "Points should increase with multiplier again");
        }

        [Test]
        public void TC_Unit_Combo_ResetsOnTimeout_ReturnsOne()
        {
            float t0 = 0f;
            float t1 = t0 + 2.0f; // beyond default combo window

            scoreManager.RegisterSlice(FruitType.Apple, false, t0);
            Assert.AreEqual(1, scoreManager.ComboMultiplier);

            scoreManager.RegisterSlice(FruitType.Apple, false, t1);
            Assert.AreEqual(1, scoreManager.ComboMultiplier, "Combo should reset after timeout");
        }

        [Test]
        public void TC_Unit_Combo_CapAtMaxMultiplier()
        {
            // Use consecutive timestamps within window to increment up to max
            int max = 5; // default from ScoreManager
            float t = 0f;
            scoreManager.ResetForTests();
            for (int i = 0; i < max + 3; i++)
            {
                scoreManager.RegisterSlice(FruitType.Apple, false, t + i * 0.5f);
            }

            Assert.AreEqual(max, scoreManager.ComboMultiplier, "Combo multiplier should cap at configured max");
        }

        [Test]
        public void TC_Unit_Combo_ScoreAppliedWithMultiplier()
        {
            scoreManager.ResetForTests();
            int pts1 = scoreManager.RegisterSlice(FruitType.Watermelon, false, 0f); // base 20 * 1
            int pts2 = scoreManager.RegisterSlice(FruitType.Watermelon, false, 0.5f); // base 20 * 2

            Assert.AreEqual(20, pts1, "First watermelon base points");
            Assert.AreEqual(40, pts2, "Second watermelon should be doubled by multiplier");
        }

        [Test]
        public void TC_Unit_Combo_MultiplePlayersIndependent()
        {
            var goA = new GameObject("ScoreA");
            var goB = new GameObject("ScoreB");
            var sA = goA.AddComponent<ScoreManager>();
            var sB = goB.AddComponent<ScoreManager>();
            sA.ResetForTests();
            sB.ResetForTests();

            sA.RegisterSlice(FruitType.Apple, false, 0f);
            sA.RegisterSlice(FruitType.Apple, false, 0.5f);

            sB.RegisterSlice(FruitType.Apple, false, 0f);

            Assert.AreEqual(2, sA.ComboMultiplier, "Player A should have combo 2");
            Assert.AreEqual(1, sB.ComboMultiplier, "Player B should have combo 1 independent");

            Object.DestroyImmediate(goA);
            Object.DestroyImmediate(goB);
        }
    }
}
