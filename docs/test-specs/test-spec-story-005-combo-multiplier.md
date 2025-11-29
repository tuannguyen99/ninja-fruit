# Test Specification: Story 005 - Combo Multiplier

**Story:** STORY-005: Combo Multiplier Logic  
**Epic:** Scoring System (EPIC-002)  
**Author:** BMAD (Test Design Agent)  
**Date:** November 30, 2025  
**Version:** 1.0

---

## Executive Summary

This document defines unit and integration tests for the Combo Multiplier feature. Tests are written to be directly translatable to Unity Test Framework (EditMode and PlayMode).

**Total Test Cases:** 10  
**Edit Mode Tests:** 6  
**Play Mode Tests:** 4

---

## Key Behaviour
- Multiplier increases by 1 for each consecutive successful slice within the combo window.
- Multiplier resets to 1 when combo window expires or when player misses a slice.
- Multiplier has an upper cap (e.g., 10x).
- Score awarded uses base value * current multiplier.

---

## EDIT MODE TESTS (Unit)

1. TC_Unit_Combo_IncreaseOnConsecutiveSlices_ReturnsIncrement

- Input: consecutive slice events within combo timeout
- Expected: multiplier increments by 1 per slice

2. TC_Unit_Combo_ResetsOnTimeout_ReturnsOne

- Input: start combo, wait beyond timeout, next slice
- Expected: multiplier resets to 1

3. TC_Unit_Combo_ResetsOnMiss_ReturnsOne

- Input: combo active, miss event triggered
- Expected: multiplier resets to 1

4. TC_Unit_Combo_CapAtMaxMultiplier

- Input: more consecutive slices than max multiplier
- Expected: multiplier == MaxMultiplier

5. TC_Unit_Combo_ScoreAppliedWithMultiplier

- Input: base score event with multiplier n
- Expected: Score awarded == base * n

6. TC_Unit_Combo_MultiplePlayersIndependent

- Input: two ComboManager instances, perform sequences
- Expected: independent multiplier tracking

---

## PLAY MODE TESTS (Integration)

1. TC_Integration_Combo_EndToEnd_SlicesIncreaseScore

- Setup: Spawn fruits, perform swipes generating a combo
- Expected: ScoreManager receives multiplied scores

2. TC_Integration_Combo_TimeoutBetweenSlices_BreaksCombo

- Setup: Perform slice, wait past combo window, perform slice
- Expected: Next slice scored at multiplier 1

3. TC_Integration_Combo_CapObservedInGameplay

- Setup: Long combo session
- Expected: Displayed multiplier caps at configured max

4. TC_Integration_Combo_ResetOnBombOrPenalty

- Setup: Trigger bomb or penalty mid-combo
- Expected: Combo resets and multiplier cleared

---

## Appendix: Assertions

- Use `Assert.AreEqual(expected, actual)` for numeric checks
- Use `Assert.IsTrue/IsFalse` for boolean conditions
- For PlayMode, use `UnityTest` coroutines and `yield return null` to allow physics/frames

---

**Status:** READY FOR CODING
