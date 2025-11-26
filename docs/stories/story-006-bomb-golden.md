# STORY-006: Bomb Penalty & Golden Fruit

**Epic:** Scoring System (EPIC-002)
**Estimate:** 2 pts
**Owner:** Link Freeman / Dev

## Description
Implement bomb hit penalty and golden fruit multiplier behavior in `ScoreManager` and spawn logic.

## Acceptance Criteria
- Hitting a bomb deducts 50 points and resets combo to 1.
- Golden fruit applies 2x to the fruit base points before combo multiplier.
- Unit tests verify both outcomes.

## Tasks
- Implement bomb penalty in `ScoreManager.RegisterBombHit()`
- Ensure `FruitSpawner` spawns golden fruit at 5% chance
- Add tests for bomb and golden fruit
