# Test Specification: Story 006 - Bomb Penalty & Golden Fruit

**Story:** STORY-006: Bomb Penalty & Golden Fruit  
**Epic:** Scoring System (EPIC-002)  
**Author:** BMAD (Test Design Agent)  
**Date:** November 30, 2025  
**Version:** 1.0

---

## Executive Summary

This document covers tests for bomb behavior (penalty) and golden fruits (bonus). Includes EditMode geometry/logic checks and PlayMode integration with physics and ScoreManager.

**Total Test Cases:** 12  
**Edit Mode Tests:** 6  
**Play Mode Tests:** 6

---

## Key Behaviour
- Bomb hit triggers immediate penalty (score deduction or game over depending on mode).
- Golden fruit grants bonus points and/or triggers special effects.
- Bombs should be excluded from normal fruit scoring flow and handled by CollisionManager's bomb path.
- Destroyed fruits should not cause exceptions when detected during collision processing.

---

## EDIT MODE TESTS (Unit / Logic)

1. TC_Unit_Bomb_DetectedByCollision_ReturnsTrue

- Input: collision check with bomb collider in swipe path
- Expected: bomb detection returns true and bomb handler invoked

2. TC_Unit_Bomb_PenaltyIsApplied

- Input: bomb hit event
- Expected: ScoreManager.RegisterBombHit() called

3. TC_Unit_GoldenFruit_BonusApplied

- Input: golden fruit slice event
- Expected: ScoreManager.RegisterBonus() called with correct value

4. TC_Unit_Bomb_IgnoredByFruitScoring

- Input: bomb collider exists within swipe path
- Expected: bomb not returned in fruit list from GetFruitsInSwipePath()

5. TC_Unit_DestroyedFruit_DoesNotThrow

- Input: fruit destroyed between detection and processing
- Expected: no null reference exceptions

6. TC_Unit_Bomb_NotDetectedWhenOutsidePath

- Input: swipe misses bomb
- Expected: no bomb penalty triggered

---

## PLAY MODE TESTS (Integration)

1. TC_Integration_Bomb_Hit_TriggersPenalty

- Setup: Spawn Bomb gameobject with collider; swipe through bomb
- Expected: ScoreManager.RegisterBombHit() invoked and bomb destroyed

2. TC_Integration_Bomb_Miss_NoPenalty

- Setup: Bomb present but swipe misses
- Expected: no penalty and bomb remains

3. TC_Integration_GoldenFruit_Hit_BonusAwarded

- Setup: Spawn golden fruit; swipe through it
- Expected: Bonus points awarded and fruit destroyed

4. TC_Integration_GoldenFruit_Multiple_Bonuses

- Setup: Multiple golden fruits sliced in combo
- Expected: Bonuses applied per fruit and combo handled

5. TC_Integration_DestroyedBomb_HandleGracefully

- Setup: Bomb destroyed before detection
- Expected: GetFruitsInSwipePath() handles without nulls

6. TC_Integration_UIFeedback_OnBombOrGolden

- Setup: Trigger bomb and golden fruit events
- Expected: UI receives proper notifications (e.g., flash, sound)

---

## Appendix: Implementation Notes

- Use `FindObjectsOfType<NinjaFruit.Gameplay.Bomb>()` for bomb handling as in `CollisionManager.cs`
- Ensure `Bomb` and `Fruit` components expose enough hooks for tests (e.g., `IsGolden` boolean)

---

**Status:** READY FOR CODING
