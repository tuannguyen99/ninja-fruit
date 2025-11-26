# STORY-007: InputManager - Mouse Support

**Epic:** Multi-Platform Input (EPIC-003)
**Estimate:** 2 pts
**Owner:** Link Freeman / Dev

## Description
Wire up `InputManager` to support mouse input using New Input System and raise `OnInputStart/Move/End` events.

## Acceptance Criteria
- Mouse click+drag triggers `OnInputStart`, repeated `OnInputMove`, then `OnInputEnd` on release.
- Play Mode tests simulate mouse device via `InputTestFixture` to validate events.

## Tasks
- Create Input Actions asset bindings for mouse
- Implement `InputManager` mouse event hooks
- Add Play Mode tests using InputTestFixture
