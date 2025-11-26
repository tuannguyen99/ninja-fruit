# Epic: Core Slicing Mechanics

**Epic ID:** EPIC-001
**Owner:** Max (Scrum Master)
**Priority:** High
**Estimate:** 13 pts

## Objective
Implement the core mechanics that make the game playable: fruit spawning, swipe detection, and collision (slice) detection. This epic delivers a minimal, playable loop so other systems (scoring, UI, persistence) can be integrated and tested.

## Success Criteria
- Players can spawn fruits and interact with them using mouse input.
- Swipes are detected reliably above the minimum speed threshold (100px/s).
- Collision detection correctly identifies pass-through slices and ignores tangents.
- Edit Mode and Play Mode tests cover spawn timing formulas and basic collision math.

## Stories
- STORY-001: FruitSpawner MVP (5 pts)
- STORY-002: SwipeDetector MVP (4 pts)
- STORY-003: CollisionManager MVP (4 pts)

## Notes for Test Architects
- Provide deterministic hooks for spawn timing (exposed API to force spawn)
- Make collision math pure functions where possible for Edit Mode tests
- Keep prefabs in `Resources/Prefabs` to simplify Play Mode test loading
