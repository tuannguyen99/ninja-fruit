# Learning Roadmap: BMAD for Game Testing Automation

**Project:** Ninja Fruit Unity Test Automation  
**Student:** Bmad  
**Goal:** Learn BMAD method for game test automation + Use AI to accelerate game development  
**Date:** November 26, 2025

---

## 🎯 Your Journey Overview

This roadmap shows you how to use BMAD to validate your idea: **Using AI to automate game testing workflows**.

**Practical Strategy:** Use AI tools to build the game quickly, then use BMAD to automate comprehensive testing infrastructure. This demonstrates BMAD's value for test automation without spending months learning game development.

```
Phase 1: Foundation (YOU ARE HERE)
└─ Create testing-focused game documentation
   ├─ ✅ Game Brief (COMPLETED)
   ├─ ⏳ Game Design Document (GDD) - Next
   └─ ⏳ Game Architecture - After GDD

Phase 2: Game Implementation (AI-Accelerated, 1-2 weeks)
└─ Build playable game quickly
   ├─ Use AI (Copilot/ChatGPT) for Unity code
   ├─ Use AI tools for graphics/sounds
   └─ Get working prototype

Phase 3: Epic & Story Planning (BMAD-driven)
└─ Break game into testable epics
   ├─ Epic 1: Core Slicing Mechanics
   ├─ Epic 2: Scoring System
   └─ Epic 3: Multi-Platform Input

Phase 4: Test Automation with BMAD ⭐ (MAIN FOCUS)
└─ Generate testing artifacts
   ├─ Test Plans per Epic
   ├─ Test Specifications
   ├─ Test Case Templates
   └─ Test Code Scaffolding

Phase 5: CI/CD Pipeline (BMAD-generated)
└─ GitHub Actions automation
   ├─ Multi-platform build configs
   ├─ Automated test execution
   └─ Test reporting

Phase 6: Customer Demonstration
└─ Show BMAD's testing value
   ├─ Show test coverage achieved
   ├─ Demo automated pipelines
   └─ Present time savings metrics
```

---

## 📚 Phase 1: Foundation Documents (Current Phase)

### What You're Learning Now
- BMAD workflow structure
- How game documentation drives testing
- The connection between GDD → Architecture → Tests

### Documents to Create

#### ✅ 1. Game Brief (COMPLETED)
**What it is:** High-level project vision focusing on testing goals  
**Why it matters:** Aligns team on priorities (testing > content generation)  
**File:** `docs/game-brief.md`

#### ⏳ 2. Game Design Document (GDD)
**What it is:** Detailed specification of ALL game mechanics  
**Why it matters for testing:** Each mechanic becomes a testable requirement  
**Key sections for testing:**
- Detailed game rules (become test assertions)
- Edge cases and failure states (become negative tests)
- Expected behaviors (become acceptance criteria)
- Performance requirements (become performance tests)

**How to create:** Use Game Designer agent workflow  
**BMAD Agent:** Samus Shepard (Game Designer)  
**Command:** `*create-gdd`

#### ⏳ 3. Game Architecture
**What it is:** Technical design showing Unity components, systems, and layers  
**Why it matters for testing:** Defines what to test and how  
**Key sections for testing:**
- Component diagram (unit test boundaries)
- System interactions (integration test scenarios)
- Testing strategy and layers
- CI/CD pipeline architecture
- Platform-specific considerations

**How to create:** Use Game Architect agent workflow  
**BMAD Agent:** Cloud Dragonborn (Game Architect)  
**Command:** `*create-architecture`

---

## 🎮 Phase 2: Epic Planning (After Phase 1)

Once you have GDD + Architecture, you'll break the game into "Epics" (large features).

### Example Epics for Ninja Fruit

**Epic 1: Core Fruit Slicing**
- Stories: Fruit spawning, swipe detection, collision detection
- **Testing Focus:** Unit tests for each component, integration tests for flow

**Epic 2: Scoring & Combos**
- Stories: Point calculation, combo tracking, high score persistence
- **Testing Focus:** Data-driven unit tests, save/load integration tests

**Epic 3: Multi-Platform Input**
- Stories: Touch handler (mobile), mouse handler (PC), input abstraction
- **Testing Focus:** Platform-specific tests, input simulation

**Epic 4: CI/CD Pipeline**
- Stories: Build automation, test execution, deployment
- **Testing Focus:** Pipeline validation, build verification tests

### How BMAD Helps
**Workflow:** Sprint Planning  
**Agent:** Max (Game Scrum Master)  
**Command:** `*sprint-planning`

This generates a sprint status file tracking all epics and stories.

---

## 🧪 Phase 3: Test Generation with BMAD (The Main Event!)

This is where BMAD proves its value for testing automation.

### For Each Epic/Story, BMAD Can Generate:

#### 3.1 Test Plans
**What:** High-level strategy for testing a feature  
**Contains:** Test objectives, scope, approach, resources needed  
**BMAD Helps:** Analyzes GDD + Architecture to identify test requirements

#### 3.2 Test Specifications
**What:** Detailed test cases with steps and expected results  
**Contains:** Preconditions, test data, steps, assertions  
**BMAD Helps:** Converts user stories into structured test cases

#### 3.3 Test Code Scaffolding
**What:** C# test class templates for Unity Test Framework  
**Contains:** Test class structure, setup/teardown, test method stubs  
**BMAD Helps:** Generates boilerplate code following Unity conventions

**Example Generated Test:**
```csharp
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class FruitSpawnerTests
{
    private FruitSpawner spawner;
    
    [SetUp]
    public void Setup()
    {
        // TODO: Initialize test dependencies
        spawner = new GameObject().AddComponent<FruitSpawner>();
    }
    
    [Test]
    public void SpawnFruit_ShouldCreateFruitInstance()
    {
        // Arrange
        int initialCount = spawner.ActiveFruitCount;
        
        // Act
        spawner.SpawnFruit();
        
        // Assert
        Assert.AreEqual(initialCount + 1, spawner.ActiveFruitCount);
    }
    
    [TearDown]
    public void Teardown()
    {
        Object.Destroy(spawner.gameObject);
    }
}
```

### BMAD Workflows for Test Generation
**Agent:** Max (Game Scrum Master)  
**Workflows:**
- `*create-story-draft` - Creates story with acceptance criteria
- `*story-context` - Assembles context for implementation (includes test specs)

---

## 🚀 Phase 4: CI/CD Pipeline Automation

### What You'll Automate

#### GitHub Actions Workflows BMAD Can Help Generate:

**1. Build Pipeline (`build.yml`)**
```yaml
# Builds for Windows, Linux, macOS, iOS, Android, WebGL
name: Build Multi-Platform

on: [push, pull_request]

jobs:
  build:
    strategy:
      matrix:
        platform: [StandaloneWindows64, StandaloneLinux64, WebGL, iOS, Android]
    # ... build steps
```

**2. Test Execution (`test.yml`)**
```yaml
name: Run Unity Tests

on: [push, pull_request]

jobs:
  test:
    steps:
      - name: Run Edit Mode Tests
      - name: Run Play Mode Tests
      - name: Upload Test Results
      - name: Generate Coverage Report
```

**3. Deploy Pipeline (`deploy.yml`)**
```yaml
name: Deploy Builds

on:
  release:
    types: [published]

jobs:
  deploy:
    # Upload builds to itch.io, Steam, Google Play, App Store
```

### How BMAD Helps
- Generates workflow YAML files based on your architecture
- Creates scripts for Unity Cloud Build or GameCI
- Provides best practices for game testing in CI

---

## 📊 Phase 5: Customer Validation

### What You'll Demonstrate

**1. Time Savings Metrics**
- "Test plan that took X hours manually, generated in Y minutes"
- "50 test cases generated from 1 user story"
- "CI/CD pipeline configured in 1 hour vs. 1 week"

**2. Quality Improvements**
- Test coverage percentage (aim for 80%+)
- Number of edge cases identified by BMAD
- Consistency across test documentation

**3. Working Demonstrations**
- Show GitHub Actions building all platforms
- Show automated tests running on commits
- Show test reports with coverage metrics
- Show game running on 2+ platforms

**4. Process Artifacts**
- Complete GDD with testing annotations
- Architecture with testing strategy
- Epic files with test specifications
- Generated test code in Unity project
- Working CI/CD pipelines

### Success Indicators for Customer
✅ **BMAD saved significant time on testing tasks** (weeks → hours)  
✅ **Test quality is high** (thorough, consistent, comprehensive)  
✅ **Test automation is complete** (CI/CD runs successfully)  
✅ **Process is repeatable** for their real games  
✅ **Clear ROI demonstrated** with time/cost metrics  

**Key Message for Customer:**
- Their creative teams still design and develop games (no AI replacing designers)
- BMAD automates the tedious testing infrastructure work
- Test plans, specs, and CI/CD configs generated in hours vs. weeks
- No copyright concerns (testing infrastructure isn't creative content)

---

## 🛠️ Tools & Technologies You'll Learn

### BMAD Components
- ✅ Agent system (different personas for different tasks)
- ⏳ Workflow execution (step-by-step guided processes)
- ⏳ Template system (reusable document structures)
- ⏳ Validation checklists (quality gates)

### Unity Testing
- Unity Test Framework (NUnit-based)
- Edit Mode Tests (code unit tests)
- Play Mode Tests (runtime integration tests)
- Performance Testing (Unity Profiler)

### CI/CD Tools
- GitHub Actions (workflow automation)
- GameCI (Unity + GitHub Actions integration)
- Unity Cloud Build (optional alternative)

### Version Control
- Git branching strategies for game dev
- Git LFS for large assets (if needed)

---

## 📖 Next Immediate Steps

### Step 1: Create GDD (Next Action)
**What to do:**
1. Switch to Game Designer agent (or I can call it for you)
2. Run the GDD workflow
3. You'll be guided through questions about mechanics
4. Focus on making mechanics testable and specific

**Estimated Time:** 30-60 minutes of Q&A

### Step 2: Create Game Architecture
**What to do:**
1. Switch to Game Architect agent
2. Run architecture workflow
3. Define testing layers explicitly
4. Include CI/CD architecture

**Estimated Time:** 45-90 minutes of Q&A

### Step 3: Define First Epic
**What to do:**
1. Come back to me (Max, Scrum Master)
2. Choose your first epic (recommend: Core Slicing Mechanics)
3. Create epic-tech-context if needed
4. Generate story drafts

**Estimated Time:** 30 minutes per story

---

## 🤔 Common Questions

**Q: Do I need to know Unity before starting?**  
A: No! Use AI (Copilot/ChatGPT) to help build the game. Focus your learning on Unity's testing tools and BMAD workflows.

**Q: How long will this whole process take?**  
A: Phase 1 (docs): 2-4 hours | Phase 2 (build game with AI): 1-2 weeks | Phase 3 (epics): 2-3 hours | Phase 4 (testing with BMAD): 3-4 hours | Phase 5 (CI/CD): 2-3 hours

**Q: Can I skip the GDD/Architecture and jump to testing?**  
A: No - those documents are the "source of truth" that BMAD uses to generate tests. Without them, test generation has no foundation.

**Q: What if the customer doesn't buy into this approach?**  
A: After Phase 1, you'll have clear documentation showing how BMAD structures the testing process. That alone demonstrates value.

**Q: Will this work for other game genres?**  
A: Yes! The BMAD method adapts. Ninja Fruit is simple to learn on, but workflows scale to RPGs, FPS, strategy games, etc.

---

## 🎓 Learning Resources

**BMAD Resources (in your project):**
- `.bmad/docs/` - BMAD documentation
- `.bmad/bmgd/workflows/` - All game dev workflows
- `.bmad/bmgd/agents/` - Agent definitions

**Unity Testing Resources:**
- Unity Test Framework docs: https://docs.unity3d.com/Packages/com.unity.test-framework@latest
- GameCI documentation: https://game.ci/
- Unity Testing Best Practices (search online)

**Game Testing Resources:**
- GDC talks on game testing (YouTube)
- "Game Testing: All in One" book (Charles P. Schultz)
- Gamasutra/Game Developer testing articles

---

## ✅ Validation Checklist

Use this to track your progress and validate readiness for customer presentation:

### Phase 1: Foundation
- [ ] Game Brief created with testing focus
- [ ] GDD completed with testable requirements
- [ ] Architecture includes testing strategy and CI/CD
- [ ] Documents reviewed for clarity and completeness

### Phase 2: Planning
- [ ] At least 3 epics defined
- [ ] Each epic has 3-5 stories
- [ ] Stories include acceptance criteria (testable)
- [ ] Sprint status generated

### Phase 3: Test Artifacts
- [ ] Test plans generated for at least 1 epic
- [ ] Test specifications created for at least 3 stories
- [ ] Test code scaffolding generated (C# files)
- [ ] Test documentation is clear and actionable

### Phase 4: Automation
- [ ] GitHub Actions workflows created
- [ ] Multi-platform builds configured
- [ ] Automated test execution working
- [ ] Test reports generated automatically

### Phase 5: Demonstration
- [ ] Unity project created with test stubs
- [ ] CI/CD pipeline runs successfully
- [ ] Documentation package prepared
- [ ] Metrics collected (time saved, coverage, etc.)
- [ ] Demo script prepared for customer

---

**Ready to proceed?** Let me know when you want to create the GDD (next step), and I'll guide you through it! 🎮

