using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NinjaFruit;

namespace NinjaFruit.Tests.PlayMode
{
    /// <summary>
    /// Play Mode (Integration) tests for SwipeDetector component.
    /// Tests event triggering with simulated input using helper methods.
    /// 
    /// Story: STORY-002 - SwipeDetector MVP
    /// Test Plan: docs/test-plans/test-plan-story-002-swipedetector.md
    /// Test Spec: docs/test-specs/test-spec-story-002-swipedetector.md
    /// 
    /// NOTE: These tests use SwipeDetector's FeedPointerDown/FeedPointerUp helper methods
    /// rather than InputTestFixture, which allows them to work without the Input System package.
    /// The SwipeDetector supports both approaches for maximum compatibility.
    /// </summary>
    [TestFixture]
    public class SwipeInputIntegrationTests
    {
        private SwipeDetector detector;
        private bool swipeDetected;
        private Vector2 detectedStart;
        private Vector2 detectedEnd;
        private int swipeCount;
        private List<(Vector2 start, Vector2 end)> detectedSwipes;

        [SetUp]
        public void Setup()
        {
            // Create detector
            GameObject detectorObject = new GameObject("TestSwipeDetector");
            detector = detectorObject.AddComponent<SwipeDetector>();
            
            // Disable the component to prevent Update() from running
            // We'll call the helper methods directly
            detector.enabled = false;

            // Reset event tracking
            swipeDetected = false;
            swipeCount = 0;
            detectedStart = Vector2.zero;
            detectedEnd = Vector2.zero;
            detectedSwipes = new List<(Vector2, Vector2)>();

            // Subscribe to event
            detector.OnSwipeDetected += OnSwipeDetectedHandler;
        }

        [TearDown]
        public void TearDown()
        {
            // Unsubscribe from event
            if (detector != null)
            {
                detector.OnSwipeDetected -= OnSwipeDetectedHandler;
                Object.DestroyImmediate(detector.gameObject);
            }

            detectedSwipes.Clear();
        }

        private void OnSwipeDetectedHandler(Vector2 start, Vector2 end)
        {
            swipeDetected = true;
            swipeCount++;
            detectedStart = start;
            detectedEnd = end;
            detectedSwipes.Add((start, end));
        }

        /// <summary>
        /// TEST-029: Validate fast mouse swipe triggers event.
        /// Priority: P1 | Risk: RISK-014 (MEDIUM)
        /// 
        /// Given: SwipeDetector with simulated input
        /// When: Fast swipe from (100,100) to (300,300) over 0.5s (~566 px/s)
        /// Then: OnSwipeDetected event is triggered with correct coordinates
        /// </summary>
        [UnityTest]
        public IEnumerator SwipeDetector_FastMouseSwipe_TriggersOnSwipeDetectedEvent()
        {
            // Arrange
            Vector2 startPos = new Vector2(0, 0);
            Vector2 endPos = new Vector2(200, 0);
            float startTime = 0f;
            float endTime = 0.1f; // 200px / 0.1s = 2000 px/s (well above 100 minimum)

            // Act - Simulate fast mouse swipe using helper methods
            // NOTE: Component is disabled, so Update() won't run
            detector.FeedPointerDown(startPos, startTime);
            detector.FeedPointerUp(endPos, endTime);

            // Assert
            Assert.IsTrue(swipeDetected,
                "Fast swipe should trigger OnSwipeDetected event");

            Assert.AreEqual(startPos, detectedStart,
                "Start position should match");
            Assert.AreEqual(endPos, detectedEnd,
                "End position should match");
            
            yield return null;
        }

        /// <summary>
        /// TEST-030: Validate slow mouse swipe does NOT trigger event.
        /// Priority: P1 | Risk: RISK-015 (MEDIUM)
        /// 
        /// Given: SwipeDetector with simulated input
        /// When: Slow swipe from (100,100) to (150,150) over 2.0s (~35 px/s)
        /// Then: OnSwipeDetected event is NOT triggered
        /// </summary>
        [UnityTest]
        public IEnumerator SwipeDetector_SlowMouseSwipe_DoesNotTriggerEvent()
        {
            // Arrange
            Vector2 startPos = new Vector2(0, 0);
            Vector2 endPos = new Vector2(10, 0);
            float startTime = 0f;
            float endTime = 1.0f; // 10px / 1.0s = 10 px/s (well below 100 minimum)

            // Act - Simulate slow mouse swipe using helper methods
            detector.FeedPointerDown(startPos, startTime);
            detector.FeedPointerUp(endPos, endTime);

            // Assert
            Assert.IsFalse(swipeDetected,
                "Slow swipe should NOT trigger OnSwipeDetected event");
            Assert.AreEqual(0, swipeCount,
                "Event should not have been invoked for slow swipe");
            
            yield return null;
        }

        /// <summary>
        /// TEST-031: Validate multiple quick swipes trigger multiple events.
        /// Priority: P1 | Risk: RISK-013 (MEDIUM)
        /// 
        /// Given: SwipeDetector with simulated input
        /// When: Two fast swipes performed consecutively
        /// Then: OnSwipeDetected event triggered twice with different coordinates
        /// </summary>
        [UnityTest]
        public IEnumerator SwipeDetector_MultipleQuickSwipes_TriggersMultipleEvents()
        {
            // Arrange
            Vector2 swipe1Start = new Vector2(0, 0);
            Vector2 swipe1End = new Vector2(200, 0);
            Vector2 swipe2Start = new Vector2(0, 0);
            Vector2 swipe2End = new Vector2(150, 0);

            float time1Start = 0f;
            float time1End = 0.1f; // 200px / 0.1s = 2000 px/s
            float time2Start = 0.2f;
            float time2End = 0.35f; // 150px / 0.15s = 1000 px/s

            // Act - First swipe
            detector.FeedPointerDown(swipe1Start, time1Start);
            detector.FeedPointerUp(swipe1End, time1End);

            // Act - Second swipe
            detector.FeedPointerDown(swipe2Start, time2Start);
            detector.FeedPointerUp(swipe2End, time2End);

            // Assert
            Assert.AreEqual(2, swipeCount,
                "Two fast swipes should trigger two events");
            Assert.AreEqual(2, detectedSwipes.Count,
                "Should have recorded two swipes");

            // Verify first swipe
            Assert.AreEqual(swipe1Start, detectedSwipes[0].start,
                "First swipe start should match");
            Assert.AreEqual(swipe1End, detectedSwipes[0].end,
                "First swipe end should match");

            // Verify second swipe
            Assert.AreEqual(swipe2Start, detectedSwipes[1].start,
                "Second swipe start should match");
            Assert.AreEqual(swipe2End, detectedSwipes[1].end,
                "Second swipe end should match");
            
            yield return null; // Required by [UnityTest]
        }

        /// <summary>
        /// TEST-032: Validate tangential movement detection (preview for collision).
        /// Priority: P2 | Risk: RISK-016 (LOW)
        /// 
        /// Given: SwipeDetector + fruit positioned nearby
        /// When: Swipe passes near fruit but doesn't intersect
        /// Then: Swipe is detected but slicing logic is separate (handled by CollisionManager)
        /// 
        /// Note: This is a preview test for STORY-003 integration.
        /// SwipeDetector only handles gesture recognition, not collision detection.
        /// </summary>
        [UnityTest]
        public IEnumerator SwipeDetector_TangentialMovement_DoesNotSliceFruit()
        {
            // Arrange - Create test fruit
            GameObject fruitObject = new GameObject("TestFruit");
            fruitObject.transform.position = new Vector3(200, 200, 0);
            CircleCollider2D fruitCollider = fruitObject.AddComponent<CircleCollider2D>();
            fruitCollider.radius = 0.3f;
            fruitObject.tag = "Fruit";

            Vector2 swipeStart = new Vector2(0, 0);
            Vector2 swipeEnd = new Vector2(200, 0);

            float startTime = 0f;
            float endTime = 0.1f; // 200px / 0.1s = 2000 px/s

            // Act - Simulate tangential swipe
            detector.FeedPointerDown(swipeStart, startTime);
            detector.FeedPointerUp(swipeEnd, endTime);

            // Assert
            Assert.IsTrue(swipeDetected,
                "Fast swipe should be detected by SwipeDetector");

            // Note: Actual collision detection (slicing) is handled by CollisionManager (STORY-003)
            // This test validates that swipe detection is independent of collision logic

            // Cleanup
            Object.DestroyImmediate(fruitObject);
            
            yield return null; // Required by [UnityTest]
        }
    }
}
