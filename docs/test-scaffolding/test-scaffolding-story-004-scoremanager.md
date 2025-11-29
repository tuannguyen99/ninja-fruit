# Test Code Scaffolding: STORY-004 - ScoreManager Base Scoring & Persistence

**Generated:** 2025-11-29  
**Story:** STORY-004 - ScoreManager - Base Scoring & Persistence

---

## Generated Files

- `Assets/Scripts/Gameplay/ScoreManager.cs` - runtime class implementing `CalculatePoints`, `SaveHighScore`, `LoadHighScore`, and simple state for `CurrentScore` and `HighScore`.
- `Assets/Tests/EditMode/Gameplay/ScoreManagerTests.cs` - Edit Mode NUnit tests for `CalculatePoints`.
- `Assets/Tests/PlayMode/Gameplay/ScoreManagerPersistenceTests.cs` - Play Mode tests using `PlayerPrefs`.

## Implementation Notes

- `CalculatePoints` should be pure and public/static or instance method to be testable in Edit Mode.
- Use `PlayerPrefs` key `HighScore` for persistence.
- Tests should call `PlayerPrefs.DeleteAll()` in `[SetUp]` and `PlayerPrefs.Save()` in teardown as needed.

## TDD Steps

1. Add `ScoreManager.cs` with `CalculatePoints` signature and minimal enum `FruitType`.
2. Add Edit Mode tests and run (should fail initially).
3. Implement logic to pass tests: base values, golden doubling, multiplier, clamp multiplier >=1.
4. Add Play Mode tests for `SaveHighScore` and `LoadHighScore`.
5. Implement persistence methods and run Play Mode tests.
