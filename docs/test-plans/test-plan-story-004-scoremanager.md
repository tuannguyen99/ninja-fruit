# Test Plan: STORY-004 - ScoreManager Base Scoring & Persistence

**Generated:** 2025-11-29  
**Story:** STORY-004 - ScoreManager - Base Scoring & Persistence  
**Epic:** EPIC-002 - Scoring System  
**Test Architect:** Automated (assistant)  
**Framework:** Unity Test Framework (NUnit)

---

## Executive Summary

This test plan covers the `ScoreManager` responsibilities for base point calculation and persistence of `HighScore`. Tests are written with a TDD approach: fast Edit Mode unit tests for pure calculations and Play Mode tests for `PlayerPrefs` persistence.

**Test Strategy:** Edit Mode unit tests for `CalculatePoints` and Play Mode tests for `HighScore` persistence.

**Estimated Test Count:** 6 tests (4 Edit Mode, 2 Play Mode)

---

## Acceptance Criteria Mapping
- Base point values per fruit type: Apple=10, Banana=10, Orange=15, Strawberry=8, Watermelon=20.
- Golden fruit doubles base value.
- Combo multiplier multiplies points.
- High score persisted via `PlayerPrefs` under key `HighScore`.

---

## Test Categories
- Edit Mode (Unit): `CalculatePoints` pure function tests.
- Play Mode (Integration): `HighScore` persistence tests using `PlayerPrefs`.

---

## Files To Generate
- `docs/test-specs/test-spec-story-004-scoremanager.md`  
- `docs/test-scaffolding/test-scaffolding-story-004-scoremanager.md`  
- `Assets/Scripts/Gameplay/ScoreManager.cs` (runtime)  
- `Assets/Tests/EditMode/Gameplay/ScoreManagerTests.cs`  
- `Assets/Tests/PlayMode/Gameplay/ScoreManagerPersistenceTests.cs`

---

## Success Criteria
- All unit tests pass locally.  
- HighScore persistence validated in Play Mode.  
- Tests are deterministic and clean up `PlayerPrefs` in setup.

---

**Document Status:** READY FOR CODE GENERATION
