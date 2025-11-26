# STORY-005: Combo Multiplier Logic

**Epic:** Scoring System (EPIC-002)
**Estimate:** 3 pts
**Owner:** Link Freeman / Dev

## Description
Implement combo timing and multiplier logic: 1.5s window, cap at 5x, reset conditions.

## Acceptance Criteria
- `RegisterSlice` updates `ComboMultiplier` based on slices within 1.5s window
- `UpdateComboTimer(deltaTime)` expires combo after 1.5s of inactivity
- Unit tests validate accumulation and expiration, including reset on bomb hit

## Tasks
- Implement combo timer and multiplier logic in `ScoreManager`
- Add Edit Mode unit tests for combo state transitions

## Test Cases
- Three slices at 0s, 0.5s, 1.0s => combo = 3
- No slice for 1.6s => combo resets to 1
- Bomb hit resets combo and applies -50 points
