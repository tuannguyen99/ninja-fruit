# STORY-002: SwipeDetector MVP

**Epic:** Core Slicing Mechanics (EPIC-001)
**Estimate:** 4 pts
**Owner:** Link Freeman / Dev

## Description
Implement `SwipeDetector` to record input points and determine when a swipe exceeds the minimum speed threshold (100 px/s). Expose `IsValidSwipe` and `CalculateSwipeSpeed` for unit tests.

## Acceptance Criteria
- `IsValidSwipe(points, deltaTime)` returns `true` only when speed >= 100 px/s.
- `CalculateSwipeSpeed(start, end, deltaTime)` computes pixels/second correctly.
- Unit tests validate boundary conditions (exactly 100 px/s should be valid).
- Play Mode test simulates a fast mouse swipe and triggers `OnSwipeDetected`.

## Tasks
- Implement `SwipeDetector` MonoBehaviour
- Add unit tests for speed calculation
- Add Play Mode input simulation test

## Test Cases
- Two points 200px apart over 1s => 200 px/s => valid
- Two points 50px apart over 1s => 50 px/s => invalid
- Tangential movement that doesn't pass through a fruit should not trigger slice
