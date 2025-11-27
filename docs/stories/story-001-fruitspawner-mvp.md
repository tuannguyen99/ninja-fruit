# STORY-001: FruitSpawner MVP

**Epic:** Core Slicing Mechanics (EPIC-001)
**Estimate:** 5 pts
**Owner:** Link Freeman / Dev

## Description
Create `FruitSpawner` component capable of instantiating fruit prefabs with configurable spawn interval and initial velocity. Include a deterministic API for tests to force spawns and query active count.

## Acceptance Criteria
- `SpawnFruit()` instantiates a fruit prefab under `Prefabs/` and tags it `Fruit`.
- `CalculateSpawnInterval(score)` returns values following `Max(0.3s, 2.0s - (score / 500))`.
- `CalculateFruitSpeed(score)` follows `Min(7m/s, 2m/s + (score / 1000))`.
- `ShouldSpawnBomb(fruitCount)` returns true at the configured bomb rate (10% early game).
- Edit Mode tests for interval and speed formulas exist.
- Play Mode test asserts `SpawnFruit()` creates an object with `Rigidbody2D`.

## Tasks
- Implement MonoBehaviour `FruitSpawner` with serialized config
- Add simple fruit prefab in `Resources/Prefabs/` (placeholder)
- Implement unit tests for formulas
- Implement Play Mode test for spawning

## Test Cases
- At score 0, spawn interval = 2.0s
- At score 500, spawn interval = 1.0s
- `SpawnFruit()` creates a GameObject tagged `Fruit` with Rigidbody2D

## Status
- **Done**: 2025-11-28
- **Commit**: 3ca43b1
