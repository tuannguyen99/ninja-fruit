# BMAD Agents Quick Reference Guide

**Project:** Ninja Fruit Test Automation  
**Purpose:** Know which agent to use and when

---

## 🎭 Your BMAD Team

Think of BMAD agents as your virtual development team. Each has expertise and specific workflows.

---

## 🎯 Max - Game Scrum Master (YOU'RE HERE NOW)

**Persona:** Talks in game terminology - milestones are save points  
**Icon:** 🎯  
**Best For:** Sprint planning, story creation, team coordination  

### When to Use Max
- ✅ Planning sprints and tracking progress
- ✅ Creating user stories for game features
- ✅ Breaking down epics into manageable tasks
- ✅ Generating story context for developers
- ✅ Facilitating retrospectives

### Key Commands (Menu Items)
1. `*sprint-planning` - Generate sprint-status.yaml from epics
2. `*epic-tech-context` - Create technical specs for an epic
3. `*create-story-draft` - Generate user story with acceptance criteria
4. `*story-context` - Assemble context XML for development
5. `*epic-retrospective` - Facilitate team review after epic
6. `*party-mode` - Consult other agents

### Max's Specialty
Translating game designs into actionable development work. If you need to organize work or create stories, Max is your agent.

---

## 🎲 Samus Shepard - Game Designer

**Persona:** Excited streamer - enthusiastic, celebrates breakthroughs  
**Icon:** 🎲  
**Best For:** Game concept, mechanics, player experience  

### When to Use Samus
- ✅ Brainstorming game ideas
- ✅ Creating Game Design Documents (GDD)
- ✅ Designing game mechanics and systems
- ✅ Narrative design (story-driven games)
- ✅ Defining player experience goals

### Key Commands
1. `*brainstorm-game` - Interactive brainstorming session
2. `*create-game-brief` - High-level game concept document
3. `*create-gdd` - Comprehensive Game Design Document
4. `*narrative` - Story and character design (for narrative games)
5. `*party-mode` - Consult other agents

### Samus's Specialty
**YOU NEED SAMUS NEXT!** To create your GDD, which will detail all testable game mechanics.

### What Samus Will Ask You
- What type of game? (Action, puzzle, arcade, etc.)
- Core gameplay loop?
- Win/loss conditions?
- Player progression?
- Key mechanics and rules?

**For Testing Focus:** Tell Samus you want extremely detailed, testable mechanics. Ask for edge cases, failure states, and specific numerical values.

---

## 🏛️ Cloud Dragonborn - Game Architect

**Persona:** Wise sage - calm, measured, uses architectural metaphors  
**Icon:** 🏛️  
**Best For:** Technical architecture, system design, platform decisions  

### When to Use Cloud
- ✅ Creating game architecture documents
- ✅ Designing system interactions and data flow
- ✅ Making technology stack decisions
- ✅ Planning multi-platform strategies
- ✅ Course correction when things change

### Key Commands
1. `*create-architecture` - Comprehensive technical architecture
2. `*correct-course` - Navigate significant project changes
3. `*party-mode` - Consult other agents

### Cloud's Specialty
**YOU NEED CLOUD AFTER SAMUS!** Cloud will design the technical architecture including your testing strategy and CI/CD pipeline.

### What Cloud Will Ask You
- Unity version and packages?
- Target platforms and constraints?
- Architecture patterns (MVC, ECS, etc.)?
- Testing layers and strategy?
- Build and deployment approach?

**For Testing Focus:** Tell Cloud you need a comprehensive testing architecture including unit, integration, and CI/CD layers.

---

## 👨‍💻 Game Developer Agent (Future Use)

**Icon:** 👨‍💻  
**Best For:** Actual code implementation, technical problem solving  

### When to Use Game Dev
- ✅ Implementing user stories
- ✅ Writing game code (C# for Unity)
- ✅ Refactoring and optimization
- ✅ Technical debugging
- ✅ Code reviews

### Game Dev's Specialty
Once you have stories ready (from Max), Game Dev agent writes the actual Unity C# code. This includes **writing test code** based on generated test specifications.

**Note:** You'll use this agent AFTER your planning phase is complete and you're ready to implement.

---

## 🎨 Other BMAD Agents (BMM Module)

Your workspace also has general software development agents that can help:

### 📋 Product Manager (PM)
- User requirements gathering
- Feature prioritization
- Roadmap planning

### 🏗️ Solution Architect
- Overall system architecture
- Integration patterns
- Technical strategy

### 💻 Developer
- General software development
- Non-game-specific code

### 🧪 Test Engineering Architect (TEA)
**⭐ ESPECIALLY USEFUL FOR YOU!**
- Test strategy and architecture
- Test automation frameworks
- Quality assurance processes

### 📝 Technical Writer
- Documentation creation
- User guides and API docs
- Process documentation

---

## 🎯 Your Workflow Path (For This Project)

```
Current → Next → Then → Finally
───────────────────────────────────────────────
Max       Samus      Cloud      Max         Game Dev
(Now)     (GDD)      (Arch)     (Stories)   (Code & Tests)
│         │          │          │           │
│         │          │          │           └─→ Implement stories
│         │          │          │               with TDD approach
│         │          │          │
│         │          │          └─→ Create test-focused
│         │          │              user stories
│         │          │
│         │          └─→ Design testing architecture,
│         │              Unity components, CI/CD
│         │
│         └─→ Detail all game mechanics
│             with testable requirements
│
└─→ Initial planning & coordination
```

---

## 🔄 How to Switch Agents

### Method 1: Use Party Mode
While in any agent, type `*party-mode` to see all available agents and switch.

### Method 2: Direct Agent Commands (VS Code)
If you have GitHub Copilot Chat modes configured:
- `@game-designer` - Switch to Samus
- `@game-architect` - Switch to Cloud  
- `@game-scrum-master` - Switch to Max (me!)

### Method 3: Manual Switch
Close current agent chat, open new chat, and activate desired agent.

---

## 💡 Pro Tips for Using Agents

### 1. **Stay in Context**
Each agent has access to your project files. They'll read GDD, Architecture, etc. as needed.

### 2. **Be Specific About Testing**
Since your focus is testing automation, remind each agent:
- "I need this to be highly testable"
- "Include test considerations"
- "Focus on automation potential"

### 3. **Don't Skip Steps**
The workflow is designed: Brief → GDD → Architecture → Stories → Implementation  
Each builds on the previous. Skipping steps means missing context.

### 4. **Use Validation Workflows**
Most workflows have a `*validate-*` command. Use these for quality checks!

### 5. **Save Progress**
After each agent creates a document, it's saved to `docs/`. Review before proceeding.

---

## 📝 What Each Agent Outputs

| Agent | Primary Output | File Location | Used By |
|-------|---------------|---------------|---------|
| **Max** | sprint-status.yaml, Story drafts | `docs/sprint-artifacts/` | Game Dev, Team |
| **Samus** | GDD.md | `docs/GDD.md` | Cloud, Max, Game Dev |
| **Cloud** | game-architecture.md | `docs/game-architecture.md` | Max, Game Dev |
| **Game Dev** | C# code, Tests | Unity project | Everyone |

---

## 🎓 Learning the BMAD Method

### Core Concepts

**1. Agents = Personas**
Each agent thinks differently. Samus focuses on player experience, Cloud on technical feasibility.

**2. Workflows = Guided Processes**
Workflows are YAML files that define step-by-step processes. They guide the agent through structured work.

**3. Templates = Consistency**
Each workflow uses templates to ensure consistent output format.

**4. Validation = Quality Gates**
Validation checklists ensure documents meet standards before moving forward.

### The BMAD Philosophy for Games
- Design first, implement later
- Documentation drives development
- Testability is a first-class concern
- Iteration is built into the process
- Multi-disciplinary collaboration (even with AI agents!)

---

## 🆘 Troubleshooting

**Problem:** Agent doesn't have the context I expected  
**Solution:** The agent will read files as needed. Mention specific files if you want them loaded.

**Problem:** Agent output is too generic  
**Solution:** Be more specific in your responses. Say "I need detailed edge cases" or "This will be unit tested, so be precise."

**Problem:** I want to change something in a generated document  
**Solution:** You can edit files directly, or ask the current agent to modify specific sections.

**Problem:** Workflow seems stuck  
**Solution:** Agents follow workflows step-by-step. Answer questions fully to proceed. Type "skip" if a step doesn't apply.

**Problem:** I don't know which agent to use  
**Solution:** Use `*party-mode` from any agent to see all options, or refer to this guide.

---

## 📋 Quick Decision Tree

**Need to...**
- ❓ Plan sprints or create stories? → **Max** (Scrum Master)
- ❓ Design game mechanics? → **Samus** (Designer)  
- ❓ Design technical architecture? → **Cloud** (Architect)
- ❓ Write code or tests? → **Game Dev**
- ❓ Design test strategy? → **TEA** (Test Engineering Architect)
- ❓ Write documentation? → **Technical Writer**
- ❓ Not sure? → **Max** (can coordinate)

---

## ✅ Current Status Checklist

- [x] Max (Scrum Master) - Active now
- [ ] Samus (Designer) - Use next for GDD
- [ ] Cloud (Architect) - Use after GDD for architecture
- [ ] Max (Scrum Master) - Return for story creation
- [ ] Game Dev - Use for implementation

**You Are Here:** Foundation phase with Max → Next: GDD with Samus

---

## 🎯 Ready for Next Step?

When you're ready to create your GDD with Samus:

**Option A:** I (Max) can call Samus via `*party-mode` for you  
**Option B:** You can switch to Samus directly and run `*create-gdd`

**My Recommendation:** Let me use party mode to introduce you to Samus with your specific testing context already explained. This ensures Samus understands your focus on testability.

**Just say:** "Call Samus to create the GDD" and I'll handle it! 🎮

