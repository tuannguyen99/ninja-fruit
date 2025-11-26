# Game Brief: Ninja Fruit (Unity Multi-Platform)

**Project Name:** Ninja Fruit  
**Target Platforms:** Mobile (iOS/Android), PC (Windows/macOS/Linux), WebGL  
**Engine:** Unity (LTS version recommended)  
**Development Focus:** Test-Driven Game Development with CI/CD Automation  
**Date:** November 26, 2025

---

## Executive Summary

Ninja Fruit is a casual arcade game inspired by Fruit Ninja, where players slice falling fruits with swipe gestures. This project serves as a **testing methodology demonstration** for game development using BMAD workflows, emphasizing automated testing, continuous integration, and multi-platform deployment.

**Key Differentiator:** This is NOT an AI-generated game asset project. Instead, it focuses on how BMAD can automate testing workflows, test generation, and quality assurance for games while preserving creative control.

---

## Vision & Goals

### Primary Goal
Demonstrate how BMAD methodology can enhance game development through:
- Automated test plan generation
- Test case design from game mechanics
- Unit and integration test scaffolding
- CI/CD pipeline automation with GitHub Actions
- Cross-platform testing strategies

### Secondary Goals
- Learn Unity game development fundamentals
- Understand BMAD workflow application in game context
- Build a portfolio piece showing QA automation expertise
- Create reusable testing patterns for future games

---

## Core Gameplay (Test-Focused Design)

### Gameplay Loop
1. **Spawn System** - Fruits appear from bottom of screen with random trajectories
2. **Input Detection** - Player swipes/clicks to slice fruits
3. **Collision Detection** - Determine if swipe intersects with fruit
4. **Scoring System** - Award points based on fruit type and combo multipliers
5. **Game Over Conditions** - Miss 3 fruits or hit a bomb

### Testable Game Mechanics

| Mechanic | Testing Priority | Test Type |
|----------|------------------|-----------|
| Fruit spawning patterns | HIGH | Unit + Integration |
| Swipe/touch input detection | HIGH | Unit + Platform-specific |
| Collision detection accuracy | CRITICAL | Unit + Physics |
| Score calculation | HIGH | Unit |
| Combo multiplier logic | MEDIUM | Unit |
| Game state transitions | HIGH | Integration |
| Save/load high scores | MEDIUM | Integration + E2E |
| Multi-platform input handling | CRITICAL | Platform Integration |

---

## Technical Scope for Testing

### Platforms & Testing Approach
- **Mobile (iOS/Android):** Touch input testing, performance profiling, platform-specific input
- **PC (Windows/macOS/Linux):** Mouse input testing, resolution handling, cross-platform builds
- **WebGL:** Browser compatibility testing, input handling in web context

### Unity Components to Test
- `FruitSpawner` - spawning logic, randomization, difficulty curves
- `InputManager` - multi-platform input abstraction
- `SwipeDetector` - gesture recognition algorithms
- `CollisionManager` - physics-based slicing detection
- `ScoreManager` - scoring, combos, persistence
- `GameStateController` - scene management, game flow

### Testing Layers
1. **Unit Tests** - Individual component logic (Unity Test Framework)
2. **Integration Tests** - Component interactions (Play Mode Tests)
3. **Performance Tests** - Frame rate, memory, spawning stress tests
4. **Platform Tests** - Input handling across different platforms
5. **Build Validation** - Automated build and deployment verification

---

## BMAD Testing Workflow Integration

### How BMAD Will Help

1. **Test Planning Phase**
   - Generate comprehensive test plans from GDD
   - Identify edge cases and testing scenarios
   - Create test coverage matrix

2. **Test Design Phase**
   - Generate test specifications for each game mechanic
   - Define acceptance criteria for user stories
   - Create test data sets (spawn patterns, score scenarios)

3. **Test Implementation Phase**
   - Generate C# test scaffolding for Unity Test Framework
   - Create test utilities and mock objects
   - Set up test fixtures and data-driven tests

4. **CI/CD Automation Phase**
   - Generate GitHub Actions workflows
   - Configure multi-platform build pipelines
   - Set up automated test execution and reporting

---

## Success Criteria

### For the Game
- ✅ Playable on at least 2 platforms (e.g., Windows + WebGL)
- ✅ Core mechanics working: spawn, slice, score, game over
- ✅ 60 FPS performance on target platforms

### For Testing (Primary Focus)
- ✅ 80%+ unit test coverage for core mechanics
- ✅ Full suite of integration tests for game flow
- ✅ Automated GitHub Actions pipeline building for all platforms
- ✅ Automated test execution on commits
- ✅ Test reports generated and archived
- ✅ Demonstrated BMAD workflow effectiveness for test generation

---

## Project Constraints & AI Usage Strategy

### AI Acceleration Strategy (Practical Approach)
**Reality Check:** You're not a game developer yet, and learning Unity from scratch takes time. Let's be pragmatic!

#### ✅ What We WILL Use AI For (Game Development Speed)
- ✅ **Game implementation code** - AI assists writing Unity C# scripts (GitHub Copilot, ChatGPT)
- ✅ **Game art/sprites** - AI-generated placeholder graphics (MidJourney, DALL-E, or simple shapes)
- ✅ **Sound effects** - AI-generated audio or free asset recommendations
- ✅ **Unity component setup** - AI helps configure GameObjects, prefabs, scenes
- ✅ **Debugging help** - AI assists troubleshooting Unity errors

**Why this is OK:** The game itself is just a vehicle to demonstrate testing automation. Speed matters.

#### 🎯 What We're PROVING (Primary Focus - Testing Automation)
- ⭐ **Test plan generation** - BMAD automatically creates comprehensive test strategies
- ⭐ **Test case design** - From GDD to structured test scenarios with acceptance criteria
- ⭐ **Test code scaffolding** - Unity Test Framework C# templates and boilerplate
- ⭐ **Test specifications** - Detailed test documentation with steps, data, assertions
- ⭐ **CI/CD pipeline generation** - GitHub Actions YAML for multi-platform builds
- ⭐ **Test execution automation** - Automated testing on commits and PRs
- ⭐ **Test reporting** - Coverage reports, test results, quality metrics

### Value Proposition for Customer
**The Pitch:**
> "We used AI to build a game quickly (not our focus). But look at what BMAD did for testing:
> - Generated 50+ test cases in minutes (would take days manually)
> - Created complete CI/CD pipeline configuration automatically
> - Provided 80% test coverage with generated scaffolding
> - Automated cross-platform testing setup
> - **This is where BMAD saves real development time and money!**"

### Implementation Workflow
**Phase 1: Design (BMAD-driven)**
1. BMAD creates GDD with testable requirements
2. BMAD creates Architecture with testing strategy
3. BMAD generates Epic and Story definitions

**Phase 2: Game Implementation (AI-accelerated)**
4. Use AI (Copilot/ChatGPT) to write Unity game code quickly
5. Use AI tools for placeholder graphics and sounds
6. Get a playable prototype running fast (1-2 weeks max)

**Phase 3: Testing (BMAD SHOWCASE - This is what matters!)**
7. BMAD generates comprehensive test plans per epic
8. BMAD creates detailed test specifications per story
9. BMAD generates Unity Test Framework scaffolding
10. You/AI complete test implementations
11. BMAD generates CI/CD GitHub Actions workflows
12. Set up automated test execution and reporting

**Phase 4: Customer Demonstration**
13. Show the working game (proves AI can build games)
14. **Show the testing infrastructure (proves BMAD's value!)**
15. Present metrics: time saved, coverage achieved, automation benefits

### Copyright & Creativity Considerations
**For Customer's Real Projects:**
- They would use their own designers/developers for game content
- They would NOT use AI to generate copyrighted game assets
- **But they WOULD use BMAD for test automation (no copyright issues!)**
- Test plans, test code, CI/CD configs are infrastructure, not creative content

**For This Demo:**
- AI-generated game content is fine (proof of concept only)
- Focus demonstration on testing value, not game quality
- Make it clear: "Game built fast with AI, Testing automated with BMAD"

---

## Learning Objectives

By completing this project, you will:
1. Understand BMAD workflow structure for game development
2. Know how to apply BMAD to testing automation
3. Learn Unity Test Framework implementation
4. Master GitHub Actions for game CI/CD
5. Build multi-platform Unity games
6. Create comprehensive test strategies for games

---

## Next Steps

1. **Create GDD** - Detail all game mechanics with testability focus
2. **Create Game Architecture** - Define Unity components, testing layers, CI/CD pipeline
3. **Define Epics** - Break down development into testable increments
4. **Generate Test Stories** - Use BMAD workflows to create test-focused user stories
5. **Implement & Test** - Build game with TDD approach using generated tests
6. **Deploy** - Use automated pipelines to build and distribute

---

## Questions for Customer Validation

Before proceeding, confirm with stakeholder:
- ✅ Is testing automation the primary deliverable (vs. the game itself)?
- ✅ Which platforms are mandatory vs. nice-to-have?
- ✅ What test coverage percentage is acceptable?
- ✅ Should we demonstrate Unity Test Framework, or other testing tools?
- ✅ Is GitHub Actions the preferred CI/CD platform, or alternatives?

---

**Document Status:** DRAFT - Ready for GDD Development  
**Owner:** Bmad (Learning Project)  
**Review Date:** TBD after customer feedback
