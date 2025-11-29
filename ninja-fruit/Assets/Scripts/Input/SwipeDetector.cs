using System;
using UnityEngine;

namespace NinjaFruit
{
    /// <summary>
    /// SwipeDetector records input points and exposes swipe calculation helpers.
    /// Story: STORY-002 - SwipeDetector MVP
    /// </summary>
    public class SwipeDetector : MonoBehaviour
    {
        [Header("Swipe Settings")]
        [Tooltip("Minimum swipe speed in pixels/sec to consider a swipe valid")]
        [SerializeField] private float minSwipeSpeed = 100f;

        public float MinSwipeSpeed => minSwipeSpeed;

        public bool IsValidSwipe(Vector2 start, Vector2 end, float deltaTime)
        {
            if (deltaTime <= 0f) return false;
            float speed = CalculateSwipeSpeed(start, end, deltaTime);
            return speed >= minSwipeSpeed;
        }

        public float CalculateSwipeSpeed(Vector2 start, Vector2 end, float deltaTime)
        {
            if (deltaTime <= 0f) return 0f;
            float distance = Vector2.Distance(start, end);
            return distance / deltaTime; // pixels per second assuming points are in pixels
        }

        // Example event for Play Mode tests; invoked when a swipe is detected
        public event Action<Vector2, Vector2> OnSwipeDetected;

        // Simple runtime check: collects mouse input and raises event when mouse up and valid
        private Vector2? pointerDownPos;
        private float pointerDownTime;

        private void Update()
        {
            // Runtime: use legacy Input so the component works without the Input System package
            if (Input.GetMouseButtonDown(0))
            {
                pointerDownPos = Input.mousePosition;
                pointerDownTime = Time.unscaledTime;
            }
            else if (Input.GetMouseButtonUp(0) && pointerDownPos.HasValue)
            {
                Vector2 upPos = Input.mousePosition;
                float deltaTime = Time.unscaledTime - pointerDownTime;
                if (IsValidSwipe(pointerDownPos.Value, upPos, deltaTime))
                {
                    OnSwipeDetected?.Invoke(pointerDownPos.Value, upPos);
                }

                pointerDownPos = null;
            }
        }

        // Test helper: feed pointer down event (tests can supply simulated times)
        public void FeedPointerDown(Vector2 position, float time)
        {
            pointerDownPos = position;
            pointerDownTime = time;
        }

        // Test helper: feed pointer up event (tests can supply simulated times)
        public void FeedPointerUp(Vector2 position, float time)
        {
            if (!pointerDownPos.HasValue)
            {
                // nothing to do
                return;
            }

            float deltaTime = time - pointerDownTime;
            if (IsValidSwipe(pointerDownPos.Value, position, deltaTime))
            {
                OnSwipeDetected?.Invoke(pointerDownPos.Value, position);
            }

            pointerDownPos = null;
        }

        /// <summary>
        /// Test helper: Directly trigger a swipe event
        /// Used by Play Mode tests to simulate swipe detection
        /// </summary>
        public void TriggerSwipeEvent(Vector2 start, Vector2 end)
        {
            OnSwipeDetected?.Invoke(start, end);
        }
    }
}
