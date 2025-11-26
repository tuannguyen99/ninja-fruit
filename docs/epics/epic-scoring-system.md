# Epic: Scoring System

**Epic ID:** EPIC-002
**Owner:** Max (Scrum Master)
**Priority:** High
**Estimate:** 8 pts

## Objective
Implement the score calculation, combo multiplier logic, bomb penalties, and persistence of high score. Tests should validate numeric correctness, combo timing windows, and negative-score handling.

## Success Criteria
- Base point values are applied per fruit type (Apple=10, Banana=10, Orange=15, Strawberry=8, Watermelon=20).
- Golden fruit doubles base value.
- Combo multiplier accrues within the 1.5s window and caps at 5x.
- Bomb hit deducts 50 points and resets combo.
- Unit tests cover point calculation and combo state transitions.

## Stories
- STORY-004: ScoreManager - Base Scoring & Persistence (3 pts)
- STORY-005: Combo Multiplier Logic (3 pts)
- STORY-006: Bomb Penalty & Golden Fruit (2 pts)

## Notes for Test Architects
- Expose CalculatePoints as a pure function for fast unit tests
- Use PlayerPrefs.DeleteAll() in test setup to ensure deterministic persistence tests
