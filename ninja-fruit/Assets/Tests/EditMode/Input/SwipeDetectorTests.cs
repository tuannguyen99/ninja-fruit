using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using NinjaFruit;

namespace NinjaFruit.Tests.EditMode
{
    /// <summary>
    /// Edit Mode (Unit) tests for SwipeDetector component.
    /// Tests speed calculation logic and swipe validation without Unity runtime.
    /// 
    /// Story: STORY-002 - SwipeDetector MVP
    /// Test Plan: docs/test-plans/test-plan-story-002-swipedetector.md
    /// Test Spec: docs/test-specs/test-spec-story-002-swipedetector.md
    /// </summary>
    [TestFixture]
    public class SwipeDetectorTests
    {
        private SwipeDetector detector;

        [SetUp]
        public void Setup()
        {
            GameObject detectorObject = new GameObject("TestSwipeDetector");
            detector = detectorObject.AddComponent<SwipeDetector>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(detector.gameObject);
        }

        #region Speed Calculation Tests

        /// <summary>
        /// TEST-021: Validate basic horizontal swipe speed calculation.
        /// Priority: P0 | Risk: RISK-011 (HIGH)
        /// 
        /// Given: 200-pixel horizontal swipe over 1 second
        /// When: CalculateSwipeSpeed() is called
        /// Then: Returns 200 pixels/second
        /// </summary>
        [Test]
        public void CalculateSwipeSpeed_200Pixels1Second_Returns200PixelsPerSecond()
        {
            // Arrange
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(200, 0);
            float deltaTime = 1.0f;
            float expectedSpeed = 200.0f;

            // Act
            float actualSpeed = detector.CalculateSwipeSpeed(start, end, deltaTime);

            // Assert
            Assert.AreEqual(expectedSpeed, actualSpeed, 0.01f,
                "Speed should be 200 px/s for 200-pixel swipe over 1 second");
        }

        /// <summary>
        /// TEST-022: Validate threshold boundary speed calculation.
        /// Priority: P0 | Risk: RISK-011 (HIGH)
        /// 
        /// Given: 100-pixel swipe over 1 second (exact threshold)
        /// When: CalculateSwipeSpeed() is called
        /// Then: Returns exactly 100 pixels/second
        /// </summary>
        [Test]
        public void CalculateSwipeSpeed_100Pixels1Second_Returns100PixelsPerSecond()
        {
            // Arrange
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(100, 0);
            float deltaTime = 1.0f;
            float expectedSpeed = 100.0f;

            // Act
            float actualSpeed = detector.CalculateSwipeSpeed(start, end, deltaTime);

            // Assert
            Assert.AreEqual(expectedSpeed, actualSpeed, 0.01f,
                "Speed should be exactly 100 px/s (threshold boundary)");
        }

        /// <summary>
        /// TEST-023: Validate fast swipe timing calculation.
        /// Priority: P0 | Risk: RISK-011 (HIGH)
        /// 
        /// Given: 50-pixel swipe over 0.5 seconds
        /// When: CalculateSwipeSpeed() is called
        /// Then: Returns 100 pixels/second (50 / 0.5 = 100)
        /// </summary>
        [Test]
        public void CalculateSwipeSpeed_50Pixels0Point5Seconds_Returns100PixelsPerSecond()
        {
            // Arrange
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(50, 0);
            float deltaTime = 0.5f;
            float expectedSpeed = 100.0f;

            // Act
            float actualSpeed = detector.CalculateSwipeSpeed(start, end, deltaTime);

            // Assert
            Assert.AreEqual(expectedSpeed, actualSpeed, 0.01f,
                "Shorter distance with faster time should still reach 100 px/s threshold");
        }

        /// <summary>
        /// TEST-024: Validate Euclidean distance calculation for diagonal swipes.
        /// Priority: P0 | Risk: RISK-011 (HIGH)
        /// 
        /// Given: Diagonal swipe forming 3-4-5 triangle (30, 40)
        /// When: CalculateSwipeSpeed() is called
        /// Then: Returns 50 pixels/second (√(30² + 40²) = 50)
        /// </summary>
        [Test]
        public void CalculateSwipeSpeed_DiagonalSwipe_CalculatesEuclideanDistance()
        {
            // Arrange
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(30, 40); // 3-4-5 right triangle
            float deltaTime = 1.0f;
            float expectedSpeed = 50.0f; // √(30² + 40²) = 50

            // Act
            float actualSpeed = detector.CalculateSwipeSpeed(start, end, deltaTime);

            // Assert
            Assert.AreEqual(expectedSpeed, actualSpeed, 0.01f,
                "Diagonal swipe should use Euclidean distance: √(30² + 40²) = 50px");
        }

        /// <summary>
        /// TEST-025: Validate division by zero protection.
        /// Priority: P0 | Risk: RISK-011 (HIGH)
        /// 
        /// Given: Any swipe with deltaTime = 0
        /// When: CalculateSwipeSpeed() is called
        /// Then: Returns 0 or handles gracefully without exception
        /// </summary>
        [Test]
        public void CalculateSwipeSpeed_ZeroDeltaTime_ReturnsZeroOrHandlesGracefully()
        {
            // Arrange
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(100, 0);
            float deltaTime = 0.0f; // Edge case

            // Act & Assert - Should not throw exception
            Assert.DoesNotThrow(() =>
            {
                float actualSpeed = detector.CalculateSwipeSpeed(start, end, deltaTime);
                Assert.AreEqual(0f, actualSpeed, 0.01f,
                    "Zero deltaTime should return 0 to avoid division by zero");
            }, "Zero deltaTime should not throw exception");
        }

        #endregion

        #region Swipe Validation Tests

        /// <summary>
        /// TEST-026: Validate fast swipe returns true.
        /// Priority: P0 | Risk: RISK-012 (HIGH)
        /// 
        /// Given: Swipe at 200 px/s (above threshold)
        /// When: IsValidSwipe() is called
        /// Then: Returns true
        /// </summary>
        [Test]
        public void IsValidSwipe_200PixelsPerSecond_ReturnsTrue()
        {
            // Arrange
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(200, 0);
            float deltaTime = 1.0f; // Speed: 200 px/s

            // Act
            bool isValid = detector.IsValidSwipe(start, end, deltaTime);

            // Assert
            Assert.IsTrue(isValid,
                "Swipe at 200 px/s should be valid (exceeds 100 px/s threshold)");
        }

        /// <summary>
        /// TEST-027: Validate slow swipe returns false.
        /// Priority: P1 | Risk: RISK-015 (MEDIUM)
        /// 
        /// Given: Swipe at 50 px/s (below threshold)
        /// When: IsValidSwipe() is called
        /// Then: Returns false
        /// </summary>
        [Test]
        public void IsValidSwipe_50PixelsPerSecond_ReturnsFalse()
        {
            // Arrange
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(50, 0);
            float deltaTime = 1.0f; // Speed: 50 px/s

            // Act
            bool isValid = detector.IsValidSwipe(start, end, deltaTime);

            // Assert
            Assert.IsFalse(isValid,
                "Swipe at 50 px/s should be invalid (below 100 px/s threshold)");
        }

        /// <summary>
        /// TEST-028: Validate exact threshold boundary (inclusive).
        /// Priority: P0 | Risk: RISK-012 (HIGH)
        /// 
        /// Given: Swipe at exactly 100 px/s (threshold boundary)
        /// When: IsValidSwipe() is called
        /// Then: Returns true (validates >= operator, not >)
        /// </summary>
        [Test]
        public void IsValidSwipe_Exactly100PixelsPerSecond_ReturnsTrue()
        {
            // Arrange
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(100, 0);
            float deltaTime = 1.0f; // Speed: exactly 100 px/s

            // Act
            bool isValid = detector.IsValidSwipe(start, end, deltaTime);

            // Assert
            Assert.IsTrue(isValid,
                "Swipe at exactly 100 px/s should be valid (inclusive threshold: speed >= 100)");
        }

        #endregion
    }
}
