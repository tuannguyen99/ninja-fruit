# STORY-004: ScoreManager - Base Scoring & Persistence

**Epic:** Scoring System (EPIC-002)
**Estimate:** 3 pts
**Owner:** Link Freeman / Dev

## Description
Implement `ScoreManager` to apply base points for fruit types and persist `HighScore` via PlayerPrefs.

## Acceptance Criteria
- `CalculatePoints(FruitType, multiplier, isGolden)` returns correct base points.
- `HighScore` saved and loaded via PlayerPrefs on game over.
- Unit tests validate base scoring and persistence behavior.

## Tasks
- Implement `ScoreManager.CalculatePoints`
- Implement `SaveHighScore` and `LoadHighScore`
- Add unit tests for scoring and persistence

## Test Cases
- Apple (10 pts) with 1x => 10
- Watermelon (20 pts) with 3x => 60
- Golden Apple doubles base before multiplier
