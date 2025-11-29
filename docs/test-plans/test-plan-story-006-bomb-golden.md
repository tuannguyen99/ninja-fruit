# Test Plan: Story 006 - Bomb Penalty & Golden Fruit

**Objective:** Validate bomb handling and golden fruit bonuses across EditMode and PlayMode.

## Scope
- Unit tests for bomb/golden fruit logic
- PlayMode integration tests involving physics and ScoreManager

## Resources
- Unity Test Framework
- Prefab fixtures for Bomb and GoldenFruit

## Schedule
- Test design & scaffolding: 0.5 day
- Implement tests & run: 0.5 day

## Exit Criteria
- No null-reference exceptions when fruits/bombs are destroyed mid-processing
- Correct penalties and bonuses applied to ScoreManager

See also: `docs/test-specs/test-spec-story-006-bomb-golden.md`
