# STORY-003: CollisionManager MVP

**Epic:** Core Slicing Mechanics (EPIC-001)
**Estimate:** 4 pts
**Owner:** Link Freeman / Dev

## Description
Implement `CollisionManager` to perform line-circle intersection tests between a swipe segment and fruit hitboxes. Provide `DoesSwipeIntersectFruit` as a pure function for unit tests.

## Acceptance Criteria
- `DoesSwipeIntersectFruit(start, end, fruitPos, radius)` returns true for proper pass-through intersections and false for tangents.
- Unit tests cover edge cases (tangent, pass-through, completely outside).
- Play Mode test verifies multi-fruit slicing using simulated swipe.

## Tasks
- Implement line-circle math in a testable static utility class
- Hook up `CollisionManager` to subscribe to `SwipeDetector` events
- Write Edit Mode and Play Mode tests

## Test Cases
- Swipe line that just grazes circle => false
- Swipe fully passing through circle => true
- Multi-fruit swipe => returns list of intersected fruit GameObjects
