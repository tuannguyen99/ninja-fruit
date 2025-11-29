# Test Specification: STORY-004 - ScoreManager Base Scoring & Persistence

**Generated:** 2025-11-29  
**Story:** STORY-004 - ScoreManager - Base Scoring & Persistence  
**Epic:** EPIC-002 - Scoring System

---

## Purpose

Provide detailed Given/When/Then test cases for `ScoreManager.CalculatePoints` and `HighScore` persistence.

---

### Edit Mode Tests (Unit)

1. TEST-001: CalculatePoints_Apple_Returns10
   - Given: FruitType.Apple, multiplier=1, isGolden=false
   - When: CalculatePoints called
   - Then: returns 10

2. TEST-002: CalculatePoints_Orange_Golden_ReturnsDouble
   - Given: FruitType.Orange (base 15), multiplier=1, isGolden=true
   - When: CalculatePoints called
   - Then: returns 30

3. TEST-003: CalculatePoints_WithMultiplier_AppliesMultiplier
   - Given: FruitType.Watermelon (20), multiplier=3, isGolden=false
   - When: CalculatePoints called
   - Then: returns 60

4. TEST-004: CalculatePoints_NegativeMultiplier_ClampsToOne
   - Given: FruitType.Strawberry (8), multiplier=0, isGolden=false
   - When: CalculatePoints called
   - Then: treats multiplier as at least 1 and returns 8

### Play Mode Tests (Integration)

5. TEST-005: HighScore_Persisted_UpdatesPlayerPrefs
   - Given: Current score 100 saved to ScoreManager and PlayerPrefs cleared
   - When: SaveHighScore called
   - Then: PlayerPrefs.GetInt("HighScore") == 100

6. TEST-006: HighScore_NotOverwritten_WhenLower
   - Given: PlayerPrefs HighScore = 200, current score 150
   - When: SaveHighScore called
   - Then: PlayerPrefs HighScore remains 200

---

**Notes:** Tests must call `PlayerPrefs.DeleteAll()` in setup/teardown to ensure determinism.
