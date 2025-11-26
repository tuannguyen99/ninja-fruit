# STORY-009: Input Actions Asset + Bindings

**Epic:** Multi-Platform Input (EPIC-003)
**Estimate:** 3 pts
**Owner:** Link Freeman / Dev

## Description
Create and commit the Input Actions asset, with two control schemes (Touch, Mouse) and actions used by `SwipeDetector`.

## Acceptance Criteria
- `Assets/InputActions/PlayerInputActions.inputactions` exists and is checked into repo
- Action map `Gameplay` contains `Touch` action with Mouse and Touchscreen bindings
- Control schemes defined and working in Editor
- Integration tests reference the asset and run in CI

## Tasks
- Create Input Actions asset in `Assets/InputActions/`
- Configure bindings for Mouse and Touch
- Commit asset to repository
- Add integration tests referencing the asset
