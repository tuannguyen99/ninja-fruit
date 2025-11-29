# EPIC-002 Retrospective — Scoring System

**Epic:** EPIC-002: Scoring System  
**Period:** Nov 29 — Nov 30, 2025  
**Owner:** Link Freeman / Dev  
**Author:** BMAD Test Design Agent  
**Date:** November 30, 2025

---

## Objective

Implement scoring primitives: base point calculations, combo multiplier, golden fruit bonuses, and bomb penalties; verify behavior through unit and integration tests.

## What Went Well
- `ScoreManager` provided a central place for combo logic and scoring which simplified testing.
- Implemented combo behavior, max multiplier, and time-windowed combos and validated with EditMode and PlayMode tests.
- Golden fruit doubling and bomb penalty behaviors implemented and tested end-to-end.
- Created test-specs, test-plans and completion reports for Stories 005 and 006.

## What Didn't Go Well / Risks
- ScoreManager currently mixes game logic (timing state) and scoring; future refactor may be necessary to separate pure calculation from stateful combo tracking for easier unit testing.
- UI/UX for combo feedback not yet covered by automated tests — risk of visual regressions.

## Key Metrics
- Stories completed: 3 (004–006) ✅
- Tests added: 10+ unit and PlayMode tests for combo, bomb, golden fruit — all passing locally
- Score changes: bomb = -50, golden fruit doubles base points (confirmed by tests)

## Decisions & Trade-offs
- Chose to implement combo management within `ScoreManager` instead of a separate `ComboManager` component to reduce initial complexity and duplication.

## Action Items (Short-term)
1. Add UI/FX PlayMode tests to validate combo display and bomb/golden visual feedback.
2. Consider extracting scoring calculations to a pure class (e.g., `ScoreCalculator`) and keep `ScoreManager` as orchestrator.
3. Add CI job to run tests and produce test results/artifacts for PRs.

## Action Items (Long-term)
1. Add analytics hooks for scoring events and instrument combo lengths and bomb hit rates to tune gameplay.
2. Add end-to-end QA tests that simulate longer play sessions to validate score accumulation and high-score persistence.

---

## Summary
EPIC-002 delivered core scoring mechanics with strong test coverage. Next steps focus on CI automation and expanding test coverage to UI feedback and longer end-to-end scenarios.
