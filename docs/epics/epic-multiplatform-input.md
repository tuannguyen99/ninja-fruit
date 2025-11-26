# Epic: Multi-Platform Input

**Epic ID:** EPIC-003
**Owner:** Max (Scrum Master)
**Priority:** Medium
**Estimate:** 8 pts

## Objective
Provide a unified input abstraction supporting mouse (desktop) and touch (mobile) via Unity's New Input System. Ensure input simulation is possible in Play Mode tests and that control schemes are properly configured.

## Success Criteria
- `InputManager` correctly raises `OnInputStart`, `OnInputMove`, and `OnInputEnd` for mouse and touch.
- Input Actions asset exists and bindable control schemes are configured (Touch, Mouse).
- InputTestFixture-based tests simulate inputs and validate `SwipeDetector` integration.

## Stories
- STORY-007: InputManager - Mouse Support (2 pts)
- STORY-008: InputManager - Touch Support (3 pts)
- STORY-009: Input Actions Asset + Bindings (3 pts)

## Notes for Test Architects
- Use InputTestFixture to create simulated devices in Play Mode tests
- Keep InputActions asset checked into `Assets/InputActions/` for repeatability
