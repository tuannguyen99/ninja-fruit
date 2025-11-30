# EPIC-004: User Interface & Game Flow

**Epic ID:** EPIC-004  
**Epic Name:** User Interface & Game Flow  
**Status:** Backlog  
**Priority:** High  
**Points Estimate:** 13  
**Owner:** Dev Team  
**Created:** November 30, 2025  
**TDD Focus:** UI component testing with PlayMode integration tests

---

## Epic Goal

Implement complete UI/UX layer including HUD, menus, and visual feedback systems. All UI components must be testable through PlayMode tests, with automated verification of state transitions and display updates.

## Business Value

**For Players:**
- Clear, readable game information (score, lives, combos)
- Intuitive menu navigation
- Satisfying visual feedback for all actions
- Responsive game state transitions

**For Development:**
- Demonstrates TDD approach for Unity UI testing
- Showcases PlayMode test patterns for UI verification
- Validates UI-logic separation architecture
- Proves UI automation value for regression prevention

## Success Criteria

1. ✅ Complete HUD showing score, lives, combo multiplier
2. ✅ Main menu, pause menu, and game over screens functional
3. ✅ All UI updates triggered by game events (not manual polling)
4. ✅ 80%+ test coverage on UI state management
5. ✅ PlayMode tests verify UI displays correct data
6. ✅ Visual effects for slice, miss, bomb hit implemented
7. ✅ UI responsive to different resolutions (tested at 1080p, 720p)

## Technical Scope

### Components to Implement
- `UIManager` - Central UI controller (singleton)
- `HUDController` - Real-time gameplay HUD
- `MenuController` - Menu navigation and state
- `VisualEffectsManager` - Particle systems and screen effects
- `UIAnimationController` - UI transitions and tweens

### Testing Strategy
- **PlayMode Tests:** UI element existence, text content verification
- **Integration Tests:** UI updates when game events fire
- **State Tests:** Menu transitions, scene loading
- **Visual Tests:** Effect instantiation (not visual appearance)

## Stories in This Epic

| Story | Title | Points | Status |
|-------|-------|--------|--------|
| 010 | HUD Display System | 3 | Backlog |
| 011 | Main Menu & Navigation | 3 | Backlog |
| 012 | Game Over Screen | 2 | Backlog |
| 013 | Pause Menu System | 2 | Backlog |
| 014 | Visual Feedback Effects | 3 | Backlog |

**Total Points:** 13

## Dependencies

**Must Complete First:**
- ✅ EPIC-001 (Core Slicing Mechanics) - provides game events for UI to listen to
- ✅ EPIC-002 (Scoring System) - provides score data for HUD display

**Parallel Work:**
- Can work alongside EPIC-003 (Multi-Platform Input) if different devs

**Blocks:**
- EPIC-005 (Polish & Juice) - requires UI foundation

## Risks & Mitigation

| Risk | Impact | Mitigation |
|------|--------|------------|
| TextMeshPro setup in tests | Medium | Use Resources.Load for TMP fonts in test setup |
| UI Canvas rendering in tests | Low | Use CanvasScaler with reference resolution |
| Animation testing complexity | Medium | Test animation triggers, not visual output |
| Scene loading in PlayMode tests | Medium | Use LoadSceneMode.Additive for test scenes |

## Out of Scope

- ❌ Audio implementation (separate epic)
- ❌ Settings menu (future enhancement)
- ❌ Localization (future enhancement)
- ❌ Advanced UI animations (basic fades/scales only)
- ❌ Tutorial/help screens (future enhancement)

## Acceptance Criteria (Epic Level)

1. **Functional:**
   - Player can navigate all menus without errors
   - HUD updates in real-time during gameplay
   - Game over screen displays correct final score and stats
   - Pause/resume works correctly (game freezes/unfreezes)

2. **Technical:**
   - All UI components accessible via singleton managers
   - Event-driven architecture (UI subscribes to game events)
   - No direct coupling between UI and gameplay logic
   - UI components separated into prefabs

3. **Testing:**
   - Minimum 15 PlayMode tests covering UI states
   - All UI state transitions have passing tests
   - Test coverage report shows 80%+ on UI managers
   - No manual testing required for regression checks

4. **Performance:**
   - UI updates don't drop frame rate below 60 FPS
   - Menu transitions smooth (<0.3s)
   - No GC spikes from UI text updates

## Testing Approach

### Test-Driven Development Flow

**For Each Story:**
1. **Write test first** - PlayMode test checking UI element exists and displays correct data
2. **Run test (FAIL)** - Verify test fails (UI not implemented yet)
3. **Implement minimal UI** - Create Canvas, UI elements, wire to manager
4. **Run test (PASS)** - Verify test now passes
5. **Refactor** - Clean up code, optimize, extract common logic
6. **Add edge case tests** - Test error states, boundary conditions

**Example Test Pattern (Story 010 - HUD):**
```csharp
[UnityTest]
public IEnumerator HUD_ScoreChanges_DisplaysNewScore()
{
    // Arrange - Create test scene with HUD
    var hudController = CreateTestHUD();
    var scoreManager = CreateTestScoreManager();
    
    // Act - Trigger score change
    scoreManager.AddScore(100);
    yield return null; // Wait one frame for UI update
    
    // Assert - Verify HUD displays new score
    var scoreText = hudController.GetScoreText();
    Assert.AreEqual("100", scoreText);
}
```

### Test Utilities Needed
- `TestSceneBuilder` - Helper to create test scenes with UI Canvas
- `UITestHelpers` - Methods to find UI elements, extract text, simulate button clicks
- `MockGameEvents` - Trigger game events for UI testing without full game simulation

## Definition of Done

**Epic is considered DONE when:**
- [ ] All 5 stories marked as 'done'
- [ ] All story acceptance criteria met
- [ ] Minimum 15 PlayMode UI tests passing
- [ ] Test coverage report shows 80%+ on UI code
- [ ] Manual QA pass (5-minute playthrough with no UI bugs)
- [ ] Epic retrospective completed
- [ ] Code reviewed and merged to main branch

## Notes

**TDD Focus Areas:**
- UI tests should check **behavior**, not implementation details
- Test UI **state**, not visual appearance (colors, fonts, etc.)
- Use **integration tests** for UI-game event connections
- Keep tests **fast** - avoid unnecessary delays (use `yield return null` not `WaitForSeconds`)

**Unity UI Testing Tips:**
- Use `[UnitySetUp]` to create test Canvas with EventSystem
- Use `Canvas.ForceUpdateCanvases()` to force immediate layout updates
- Use `LayoutRebuilder.ForceRebuildLayoutImmediate()` for layout groups
- Test button clicks via `button.onClick.Invoke()`, not mouse simulation

---

## Related Documents

- Epic File: `docs/epics/epic-ui-game-flow.md`
- Stories: `docs/stories/story-010-*.md` through `story-014-*.md`
- Test Plans: `docs/test-plans/test-plan-story-010-*.md`
- Architecture: `docs/game-architecture.md` (UI Components section)
- GDD: `docs/GDD.md` (User Interface section)

---

**Document Status:** DRAFT - Ready for Story Creation  
**Next Step:** Create Story 010 (HUD Display System) with TDD approach  
**Review Required:** Scrum Master approval before starting implementation
