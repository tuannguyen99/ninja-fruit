# Ninja Fruit - Game Design Document

**Author:** Bmad  
**Game Type:** Casual Arcade Action  
**Target Platform(s):** Mobile (iOS/Android), PC (Windows/macOS/Linux), WebGL  
**Engine:** Unity (LTS)  
**Document Version:** 1.0  
**Date:** November 26, 2025  
**Primary Focus:** Test-Driven Game Development with Comprehensive Testing Strategy

---

## Executive Summary

### Core Concept

Ninja Fruit is a fast-paced casual arcade game where players slice falling fruits with swipe gestures while avoiding bombs. Inspired by Fruit Ninja, the game emphasizes immediate satisfaction through precise slicing mechanics, combo systems, and escalating difficulty. 

**Primary Purpose:** This game serves as a demonstration vehicle for BMAD-powered automated testing workflows, showcasing how comprehensive test infrastructure can be generated from detailed game design specifications.

### Target Audience

- **Primary:** Casual mobile gamers (ages 8-45) seeking quick, satisfying gameplay sessions
- **Secondary:** QA professionals and game developers learning test automation methodologies
- **Skill Level:** Accessible to beginners, skill ceiling for competitive high scores

### Unique Selling Points (USPs)

1. **Satisfying Dual Feedback** - Combines visual "pop" effects with powerful slash animations
2. **Multi-Fruit Slicing** - One swipe can slice multiple fruits for dramatic combos
3. **Cross-Platform Play** - Seamless experience across mobile touch and PC mouse inputs
4. **Test-Driven Design** - Every mechanic designed with testability and measurability in mind
5. **Progressive Difficulty** - Dynamic challenge scaling keeps players engaged

---

## Goals and Context

### Project Goals

**Primary Goal:**
Demonstrate BMAD methodology's effectiveness in automating game testing workflows:
- Generate comprehensive test plans from GDD specifications
- Create detailed test case documentation automatically
- Produce Unity Test Framework code scaffolding
- Configure CI/CD pipelines for multi-platform builds
- Achieve 80%+ test coverage on core mechanics

**Secondary Goals:**
- Create a playable, enjoyable arcade game experience
- Learn Unity game development fundamentals with AI assistance
- Build reusable testing patterns for future game projects
- Showcase time savings in QA infrastructure development (weeks → hours)

### Background and Rationale

**Problem Statement:**
Game development teams spend 3-4 weeks manually creating test plans, test specifications, and CI/CD configurations for each project. This represents significant overhead that delays time-to-market.

**Solution Approach:**
Use BMAD workflows to automate test infrastructure generation, reducing QA setup time by 95% while maintaining or improving test quality and coverage.

**Success Criteria:**
- Working game playable on minimum 2 platforms (Windows + WebGL)
- Complete test infrastructure generated via BMAD workflows
- Demonstrable time savings metrics for customer presentation
- Repeatable process applicable to other game genres

---

## Core Gameplay

### Game Pillars

1. **Instant Satisfaction** - Every successful slice delivers immediate visual and audio feedback
2. **Escalating Challenge** - Difficulty ramps smoothly to maintain engagement
3. **Risk vs Reward** - Aggressive multi-fruit slicing risks missing fruits or hitting bombs
4. **Precision Skill** - Mastery comes from accurate swipes and combo timing

### Core Gameplay Loop

```
1. SPAWN → Fruits launch upward in parabolic arcs
   ↓
2. DETECT → Player swipes through screen space
   ↓
3. SLICE → Swipe intersects fruit hitbox (>100px/s speed)
   ↓
4. SCORE → Points awarded based on fruit type + combo multiplier
   ↓
5. REPEAT → Continue until 3 fruits missed or manual quit
```

**Session Structure:**
- Single endless game session until fail condition
- No levels or stages - continuous escalation
- Average session length: 2-5 minutes (skill-dependent)

### Win/Loss Conditions

**Win Condition:**
- No traditional "win" - goal is maximize high score
- Achievements: Score milestones (500, 1000, 2000, 5000+)
- Mastery: Maintain 5x combo multiplier for extended periods

**Loss Conditions:**
1. **3 Missed Fruits** → Game Over (primary fail condition)
2. **Manual Quit** → Session ends, score saved if high score

**Bombs:**
- Hitting bomb does NOT trigger game over
- Penalty: -50 points (can go negative)
- Breaks combo multiplier (resets to 1x)

---

## Game Mechanics

### Primary Mechanics

#### 1. Fruit Spawning System

**Spawn Timing:**
- **Interval:** 0.5 to 2.0 seconds between spawn events (randomized)
- **Quantity per Spawn:** 1-3 fruits simultaneously (random)
- **Initial Difficulty:** Lower spawn rate at game start
- **Progression:** Spawn rate increases over time (faster spawns as score increases)

**Spawn Behavior:**
- **Trajectory:** Parabolic arc (physics-based projectile motion)
- **Peak Height:** 70% of screen height
- **Spawn Position:** Random X position across bottom 10% of screen
- **Launch Angle:** Random upward angle (60-120 degrees from horizontal)

**Fruit Speed Variants:**
- **Slow Fruits:** 2 m/s initial velocity (easier targets)
- **Fast Fruits:** 5 m/s initial velocity (challenging targets)
- **Speed Distribution:** 70% slow, 30% fast (early game)
- **Speed Progression:** Fast fruit percentage increases over time

#### 2. Fruit Types and Properties

| Fruit Type | Points | Size | Collision Radius | Spawn Weight |
|------------|--------|------|------------------|--------------|
| Apple      | 10     | Small | 0.3 units | 25% |
| Banana     | 10     | Small | 0.3 units | 25% |
| Orange     | 15     | Medium | 0.4 units | 20% |
| Strawberry | 8      | Small | 0.25 units | 20% |
| Watermelon | 20     | Large | 0.6 units | 10% |
| **Golden Fruit** | **2x multiplier** | Small | 0.3 units | **Rare (5%)** |

**Golden Fruit Special Rules:**
- Points = (fruit base value) × 2
- Visual: Shimmering gold particle effect
- Spawn Condition: Random 5% chance on any fruit spawn
- Test Case: Verify 2x point calculation

**Size Impact on Gameplay:**
- Small fruits: Harder to hit, lower point value
- Large fruits: Easier to hit, higher point value
- Size affects collision detection precision (testing priority: CRITICAL)

#### 3. Slicing Mechanics

**Swipe Requirements:**
- **Minimum Speed:** 100 pixels/second to register as valid slice
- **Collision Model:** Swipe line segment must intersect fruit's circular hitbox
- **Pass-Through:** Swipe must COMPLETELY pass through fruit (entry + exit required)
- **Multi-Slice:** Single swipe can slice multiple fruits if path intersects all

**Slice Detection Algorithm (Testable):**
```
1. Track touch/mouse input positions over time
2. Calculate velocity between consecutive positions
3. If velocity >= 100px/s, create line segment
4. For each fruit:
   a. Check line segment vs circle intersection
   b. Verify line passes through circle (not just touches edge)
   c. If valid, trigger slice event
5. Award points and update combo for all sliced fruits
```

**Test Cases for Slicing:**
- Swipe speed below 100px/s = no slice (boundary condition)
- Swipe tangent to fruit edge = no slice (pass-through requirement)
- Single swipe hits 3 fruits = all 3 sliced + 3x combo
- Swipe between two fruits (no hit) = both fruits unsliced

#### 4. Scoring System

**Base Scoring:**
- Each fruit has fixed point value (see table above)
- Golden fruit multiplies base value by 2
- Bomb hit deducts 50 points (can result in negative score)

**Combo Multiplier System:**

| Fruits Sliced in Window | Multiplier | Display |
|-------------------------|------------|---------|
| 1 fruit | 1x | No special display |
| 2 fruits within 1.5s | 2x | "COMBO 2x!" |
| 3 fruits within 1.5s | 3x | "COMBO 3x!" |
| 4 fruits within 1.5s | 4x | "COMBO 4x!" |
| 5+ fruits within 1.5s | 5x (max) | "MEGA COMBO 5x!" |

**Combo Rules:**
- **Time Window:** 1.5 seconds between slices to maintain combo
- **Accumulation:** Combo builds as long as slices continue within window
- **Maximum:** 5x multiplier (cap)
- **Reset Conditions:**
  - 1.5 seconds pass without a slice
  - Player hits a bomb
  - Game over event
- **Score Calculation:** `Final Points = Base Points × Combo Multiplier`

**Example Score Calculations (Test Cases):**
- Slice Apple (10pts) with 3x combo = 30 points
- Slice Watermelon (20pts) with 5x combo = 100 points
- Slice Golden Apple (10×2=20pts) with 2x combo = 40 points
- Hit bomb = -50 points + combo reset to 1x

#### 5. Bomb Mechanics

**Spawn Rules:**
- **Frequency:** 1 bomb per 10 fruits spawned (10% rate)
- **Difficulty Scaling:** Bomb rate increases over time (up to 20% at high scores)
- **Appearance:** Clearly distinct from fruits (black sphere with red warning symbol)
- **Proximity:** Can spawn near fruits (creates risk/reward decisions)

**Bomb Behavior:**
- **Physics:** Same parabolic trajectory as fruits
- **Avoidance:** Player can ignore bombs - they fall off screen safely
- **Hit Detection:** If player's swipe intersects bomb, trigger penalty
- **Penalty Effect:**
  - Deduct 50 points from score
  - Reset combo multiplier to 1x
  - Visual feedback: Screen shake + red flash
  - Audio feedback: Explosion sound

**Test Cases for Bombs:**
- Bomb spawns after exactly 10 fruits (deterministic test)
- Bomb ignored (no swipe) = no penalty
- Bomb hit = -50 points + combo reset verified
- Bomb spawns within 0.5 units of fruit = proximity test passed

### Controls and Input

**Mobile (Touch Input):**
- **Primary Action:** Touch and drag finger across screen
- **Detection:** Continuous touch tracking at 60Hz
- **Responsiveness:** <16ms input lag target
- **Multi-touch:** Not required (single swipe only)

**PC (Mouse Input):**
- **Primary Action:** Click and drag mouse cursor
- **Alternative:** Keyboard arrows + spacebar (optional)
- **Responsiveness:** <16ms input lag target

**Input Abstraction (Cross-Platform):**
- Unified `InputManager` component handles touch/mouse translation
- Platform-specific gesture recognition normalized to common interface
- Test Coverage: Input simulation for both touch and mouse scenarios

---

## Progression and Balance

### Player Progression

**No Persistent Progression:**
- Each game session starts fresh
- No unlockables, upgrades, or power-ups
- Pure skill-based progression within each session

**Within-Session Scaling:**
- **Phase 1 (0-200 points):** Tutorial difficulty - slow spawns, few bombs
- **Phase 2 (200-500 points):** Standard difficulty - moderate spawn rate
- **Phase 3 (500-1000 points):** Challenging - faster spawns, more bombs
- **Phase 4 (1000+ points):** Expert - maximum spawn rate, 20% bomb rate

### Difficulty Curve

**Dynamic Difficulty Scaling Formula:**

```
Spawn Interval = Max(0.3s, 2.0s - (Score / 500))
Fruit Speed = Min(7m/s, 2m/s + (Score / 1000))
Bomb Rate = Min(20%, 10% + (Score / 100))
Fast Fruit % = Min(60%, 30% + (Score / 200))
```

**Test Cases for Difficulty:**
- At score 0: Spawn interval = 2.0s, Speed = 2m/s, Bomb rate = 10%
- At score 500: Spawn interval = 1.0s, Speed = 2.5m/s, Bomb rate = 15%
- At score 1000: Spawn interval = 0.3s (min), Speed = 4m/s, Bomb rate = 20% (max)

**Difficulty Milestones (Visual Feedback):**
- Every 500 points: Screen effect + "Level Up!" message
- Background intensity increases with score
- Particle effects become more dramatic

### Economy and Resources

**No Economy System:**
- No currency, coins, or purchasable items
- No resource management mechanics
- Focus is pure arcade skill expression

**Lives System:**
- **Starting Lives:** 3 (represented as missed fruit counter)
- **Loss Condition:** Reach 0 lives remaining
- **No Life Recovery:** Lives don't regenerate during session
- **Display:** Hearts UI in top corner (3 full hearts = 3 lives remaining)

---

## Level Design Framework

### Level Types

**Single Endless Level:**
- No traditional levels or stages
- One continuous play area (full screen)
- Visual background evolves based on score milestones
- Difficulty scales dynamically (see Progression section)

**Play Area Specifications:**
- **Dimensions:** Full screen (responsive to device resolution)
- **Spawn Zone:** Bottom 10% of screen
- **Play Zone:** Middle 70% (peak height of arcs)
- **Miss Zone:** Top 20% (fruits fall back down through this)
- **Bounds:** Fruits despawn when passing bottom edge again (counts as miss)

### Level Progression

**Score Milestones Visual Themes:**
- **0-500 pts:** Bright daytime (beginner friendly)
- **500-1000 pts:** Sunset (ramping challenge)
- **1000-2000 pts:** Dusk (expert territory)
- **2000+ pts:** Night with neon effects (mastery level)

**No Artificial Gating:**
- Players can reach any score threshold in single session
- Difficulty scaling is continuous, not step-function

---

## Art and Audio Direction

### Art Style

**Visual Theme:** Vibrant, juicy, arcade-style 2D graphics

**Fruit Design:**
- Semi-realistic fruit appearances (recognizable at a glance)
- Bright, saturated colors (high contrast against backgrounds)
- Slice animation: Fruit splits in half with juice particle effects
- Golden fruit: Glowing aura + sparkle particles

**UI Style:**
- Bold, readable fonts (visibility during fast gameplay)
- Minimalist HUD (don't obscure play area)
- Score display: Large numbers, top center
- Combo multiplier: Animated popup near sliced fruits

**Visual Effects:**
- **Slice Trail:** Temporary line showing swipe path (0.2s fade)
- **Juice Splatter:** Particle system on fruit destruction
- **Screen Shake:** Subtle on bomb hit
- **Flash Effects:** White flash on combo milestones

### Audio Design

**Sound Effects:**
- **Fruit Slice:** Satisfying "slice" sound (varies by fruit type)
- **Combo Trigger:** Rising pitch with combo multiplier
- **Bomb Hit:** Explosion sound + negative audio cue
- **Fruit Miss:** Whoosh + descending tone (disappointment)
- **Milestone:** Fanfare at 500-point intervals

**Music:**
- **Style:** Upbeat, energetic arcade music
- **Dynamic:** Tempo/intensity increases with score
- **Loop:** Seamless 2-minute loop
- **Volume Control:** User-adjustable in settings

**Audio Settings:**
- Master volume slider (0-100%)
- Sound effects toggle (on/off)
- Music toggle (on/off)
- Settings persist between sessions

---

## User Interface (UI)

### HUD (Heads-Up Display)

**During Gameplay:**
```
┌─────────────────────────────────────┐
│  ❤️❤️❤️            SCORE: 1250    │  ← Lives + Score
│                   COMBO: 3x        │  ← Combo Multiplier
│                                     │
│          [Gameplay Area]            │
│                                     │
│                                     │
│                   ⏸️                │  ← Pause Button
└─────────────────────────────────────┘
```

**UI Elements:**
- **Lives Display:** Top-left, 3 hearts (full/empty states)
- **Score:** Top-right, large font, updates real-time
- **Combo Indicator:** Below score, animated when active, fades when idle
- **Timer:** Optional - tracks session duration (bottom-left, small font)
- **Pause Button:** Bottom-right corner, always accessible

### Menus

**Main Menu:**
- **PLAY** - Start new game session
- **HIGH SCORES** - View local high scores + stats
- **SETTINGS** - Audio and display settings
- **QUIT** - Exit game (PC only)

**Pause Menu:**
- **RESUME** - Continue current game
- **RESTART** - End current game, start fresh
- **SETTINGS** - Adjust audio without ending game
- **QUIT TO MENU** - End game, return to main menu

**Game Over Screen:**
- Display final score (large, centered)
- Show high score comparison ("New High Score!" or "High Score: XXX")
- Display session stats:
  - Total fruits sliced
  - Longest combo achieved
  - Session duration
- **PLAY AGAIN** button
- **MAIN MENU** button

### Settings Menu

**Audio Settings:**
- Master Volume: Slider (0-100%)
- Sound Effects: Toggle (On/Off)
- Music: Toggle (On/Off)

**Display Settings (Optional):**
- Fullscreen toggle (PC only)
- Resolution selection (PC only)

---

## Persistence and Data

### High Score System

**Saved Data (Local Storage):**
- **High Score:** Highest score achieved across all sessions
- **Total Fruits Sliced:** Lifetime counter
- **Longest Combo:** Best combo multiplier achieved
- **Session Stats:** Last game's stats (for game over screen)

**No Online Features:**
- No leaderboards (local only)
- No cloud saves
- No multiplayer functionality

**Data Format:**
- Unity PlayerPrefs or JSON file
- Keys: `HighScore`, `TotalFruitsSliced`, `LongestCombo`
- Saved on game over, loaded on game start

**Test Cases for Persistence:**
- New high score saves correctly
- Stats accumulate across sessions
- Data persists after app close/reopen
- Invalid/corrupted data handled gracefully (default to 0)

---

## Performance Requirements

### Technical Targets

**Frame Rate:**
- **Target:** 60 FPS on all platforms
- **Minimum:** 30 FPS (degradation acceptable on low-end mobile)
- **Performance Test:** Stress test with 20 fruits + 5 bombs on screen

**Input Latency:**
- **Target:** <16ms (single frame at 60fps)
- **Measurement:** Time from touch/click to visual feedback
- **Test:** Automated input simulation with timestamp logging

**Memory Usage:**
- **Target:** <200MB RAM on mobile
- **Target:** <500MB RAM on PC
- **Test:** Memory profiling during extended play sessions

**Build Size:**
- **Target:** <50MB for mobile (pre-install)
- **Target:** <100MB for PC (compressed)

**Supported Resolutions:**
- Mobile: 720p - 1440p (responsive UI scaling)
- PC: 1080p - 4K (UI scales proportionally)
- WebGL: Any browser window size (responsive)

---

## Testing Strategy

### Critical Test Areas (Priority Order)

1. **Collision Detection (CRITICAL):**
   - Swipe-fruit intersection accuracy
   - Edge cases: Tangent swipes, ultra-fast swipes, slow swipes
   - Multi-fruit slicing in single swipe
   - Pass-through verification

2. **Scoring System (HIGH):**
   - Base point values per fruit type
   - Combo multiplier calculation
   - Golden fruit 2x multiplier
   - Bomb penalty (-50 points)
   - Negative score handling

3. **Spawn System (HIGH):**
   - Spawn timing intervals (0.5-2s randomization)
   - Fruit quantity per spawn (1-3)
   - Bomb frequency (1 per 10 fruits)
   - Difficulty scaling formulas

4. **Input Handling (CRITICAL):**
   - Touch input on mobile (iOS + Android)
   - Mouse input on PC (Windows, macOS, Linux)
   - WebGL browser input compatibility
   - Input latency measurements

5. **Game State Management (HIGH):**
   - Game start initialization
   - Pause/resume functionality
   - Game over trigger (3 misses)
   - Score persistence between sessions

6. **Performance (MEDIUM):**
   - 60 FPS under stress (20 fruits + 5 bombs)
   - Memory leak detection (extended sessions)
   - Build size verification

### Test Coverage Goals

- **Unit Tests:** 80%+ coverage on core mechanics (spawn, score, collision)
- **Integration Tests:** 100% coverage on game flow (start → play → game over)
- **Platform Tests:** Full coverage on 3 platforms minimum (Windows, WebGL, Android)
- **Performance Tests:** Stress tests for all difficulty levels

### Automated Test Categories

1. **Edit Mode Tests (Unit Tests):**
   - Pure logic testing (no Unity runtime)
   - Scoring calculations
   - Spawn timing algorithms
   - Difficulty scaling formulas

2. **Play Mode Tests (Integration Tests):**
   - Component interaction testing (requires Unity runtime)
   - Fruit spawning → collision → scoring flow
   - Input simulation → slice detection
   - Game state transitions

3. **Platform Integration Tests:**
   - Input handling per platform (touch vs mouse)
   - Resolution scaling verification
   - Build verification tests (successful compilation per platform)

4. **Performance Tests:**
   - Frame rate monitoring under load
   - Memory profiling
   - Input latency measurements

---

## Implementation Priorities (For Development)

### MVP Phase 1: Core Mechanics (Week 1)
- [ ] Fruit spawning system (parabolic arcs)
- [ ] Basic swipe input detection (mouse only)
- [ ] Collision detection (swipe vs fruit)
- [ ] Simple scoring (no combos)
- [ ] Game over condition (3 misses)

### MVP Phase 2: Full Scoring System (Week 1-2)
- [ ] All 5 fruit types + Golden fruit
- [ ] Combo multiplier system (1x-5x)
- [ ] Bomb spawning and penalty
- [ ] Lives tracking and display

### Phase 3: Polish and UI (Week 2)
- [ ] Full HUD (score, lives, combo)
- [ ] Main menu + pause menu
- [ ] Game over screen with stats
- [ ] Basic visual effects (slice trails, particles)

### Phase 4: Multi-Platform (Week 2-3)
- [ ] Touch input for mobile
- [ ] Platform-specific builds (Windows, WebGL, Android)
- [ ] Input abstraction layer
- [ ] Resolution scaling

### Phase 5: Testing Infrastructure (Week 3)
- [ ] Unit test suite (80%+ coverage)
- [ ] Integration tests
- [ ] Performance tests
- [ ] CI/CD pipeline (GitHub Actions)

---

## Known Limitations and Future Enhancements

### Current Scope Limitations

**Out of Scope for V1:**
- ❌ Online multiplayer
- ❌ Online leaderboards
- ❌ Unlockable content
- ❌ Power-ups or special abilities
- ❌ Multiple game modes
- ❌ In-app purchases
- ❌ Achievements/trophies system

### Potential Future Features (Post-MVP)

**If Expanding Beyond Demo:**
1. **Game Modes:**
   - Time Attack (60-second score rush)
   - Zen Mode (no bombs, no game over)
   - Challenge Mode (specific fruit targets)

2. **Power-Ups:**
   - Freeze Time (slow-motion for 5 seconds)
   - Multi-Slice (one swipe slices everything on screen)
   - Score Boost (2x all points for 10 seconds)

3. **Social Features:**
   - Online leaderboards
   - Ghost data (replay best runs)
   - Share score to social media

4. **Progression:**
   - Unlockable blade skins
   - Background themes
   - Particle effect variations

---

## Appendix A: Test Case Examples

### Example Unit Test: Combo Multiplier Calculation

```csharp
[Test]
public void ComboMultiplier_ThreeFruitsInWindow_Returns3x()
{
    // Arrange
    var scoreManager = new ScoreManager();
    float currentTime = 0f;
    
    // Act
    scoreManager.RegisterSlice(currentTime); // Slice 1
    scoreManager.RegisterSlice(currentTime + 0.5f); // Slice 2 (0.5s later)
    scoreManager.RegisterSlice(currentTime + 1.0f); // Slice 3 (1.0s after first)
    
    // Assert
    Assert.AreEqual(3, scoreManager.CurrentComboMultiplier);
}
```

### Example Integration Test: Full Game Flow

```csharp
[UnityTest]
public IEnumerator GameFlow_ThreeMisses_TriggersGameOver()
{
    // Arrange
    var gameController = GameObject.Find("GameController").GetComponent<GameStateController>();
    gameController.StartGame();
    
    // Act - Simulate 3 missed fruits
    for (int i = 0; i < 3; i++)
    {
        var fruit = SpawnTestFruit();
        yield return new WaitForSeconds(5f); // Let fruit fall and despawn
    }
    
    // Assert
    Assert.AreEqual(GameState.GameOver, gameController.CurrentState);
    Assert.AreEqual(0, gameController.LivesRemaining);
}
```

---

## Appendix B: Glossary

- **Combo Window:** 1.5-second time period to maintain combo multiplier
- **Pass-Through:** Swipe line segment must enter AND exit fruit's collision circle
- **Spawn Event:** Moment when 1-3 fruits are instantiated simultaneously
- **Miss:** Fruit exits play area through bottom of screen without being sliced
- **Golden Fruit:** Rare fruit variant that doubles point value (5% spawn chance)
- **Stress Test:** Performance test with maximum on-screen entities (20 fruits + 5 bombs)

---

## Document Change Log

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-11-26 | Bmad / Samus Shepard | Initial GDD creation with complete mechanics specification |

---

**Document Status:** COMPLETE - Ready for Architecture Phase  
**Next Step:** Create Game Architecture document with Cloud Dragonborn (Game Architect)  
**Approval:** Pending review by Max (Scrum Master) and Murat (Test Architect)

---

*This GDD is designed to be consumed by BMAD workflows for automated test generation. Every mechanic includes specific numerical values and testable assertions.*
