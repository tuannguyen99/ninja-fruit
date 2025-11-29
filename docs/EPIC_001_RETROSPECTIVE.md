# EPIC-001 Retrospective — Core Slicing Mechanics

**Epic:** EPIC-001: Core Slicing Mechanics  
**Period:** Nov 26 — Nov 29, 2025  
**Owner:** Link Freeman / Dev  
**Author:** BMAD Test Design Agent  
**Date:** November 30, 2025

---

## Objective

Deliver robust, testable core slicing mechanics: fruit spawning, swipe detection, and collision detection — using TDD and automated tests as the primary verification strategy.

## What Went Well
- Implementation followed TDD: tests drove the design and caught edge cases early.
- Full test coverage for stories 001–003: EditMode + PlayMode tests passing (comprehensive geometry and integration coverage).
- Collision geometry implemented using vector projection produced a clear, efficient algorithm (O(1) per check).
- PlayMode integration tests validated real-world scenarios (overlapping fruits, destroyed objects, multi-fruit slicing).
- Documentation and test-specs were created alongside code, making verification repeatable and auditable.

## What Didn't Go Well / Risks
- Input System compatibility required workarounds for tests (component disabling or helper methods) — added friction for PlayMode tests.
- Boundary conditions (tangents, zero-length swipes, partial hits) required careful epsilon selection and clear spec definitions.
- Some PlayMode tests required careful timing and physics framing; flaky tests were possible until authors stabilized them with coroutines and helper waits.

## Key Metrics
- Stories completed: 3 (001–003) ✅
- Tests added: 24 (EditMode + PlayMode for CollisionManager) — all passing locally
- Performance: Per-collision check measured <1ms in verification notes (needs CI perf tests)

## Decisions & Trade-offs
- Reused Unity's physics/collider APIs for realism rather than pure math-only mocks — improved integration fidelity at cost of more complex PlayMode tests.
- Rejected tangent touches as valid slices (strict distance < radius and 0 < h < 1). This aligns with desired gameplay behavior but must be communicated to UX designers.

## Action Items (Short-term)
1. Add CI that runs EditMode and PlayMode tests (GitHub Actions) to catch regressions early.
2. Add deterministic test helpers for the Input System to avoid toggling components in tests.
3. Add a small performance benchmark test (measure collisions per frame) and include it in CI smoke tests.
4. Add a short developer guide describing the tangent decision and epsilon choices for future contributors.

## Action Items (Long-term)
1. Expand test matrix across platforms (Windows, WebGL) in CI.
2. Add automated flakiness detection and retry policy for PlayMode tests to surface intermittent failures.
3. Consider abstracting collision geometry into a small math-only library so pure-unit tests can exercise more corner cases deterministically.

---

## Summary
EPIC-001 delivered a robust, well-tested core of the gameplay mechanics. The work validated the TDD approach and left the codebase in a state suitable for CI automation and further features.
