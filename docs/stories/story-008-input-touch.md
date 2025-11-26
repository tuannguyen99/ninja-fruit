# STORY-008: InputManager - Touch Support

**Epic:** Multi-Platform Input (EPIC-003)
**Estimate:** 3 pts
**Owner:** Link Freeman / Dev

## Description
Add touchscreen bindings to `InputManager` and verify touch behavior on mobile control scheme.

## Acceptance Criteria
- Touch begin/move/end map to `OnInputStart/Move/End`.
- Input Actions include Touch control scheme.
- Play Mode tests simulate touchscreen device and validate event propagation.

## Tasks
- Add touchscreen bindings to Input Actions asset
- Update `InputManager` to handle touch callbacks
- Add Play Mode tests with simulated Touchscreen device
