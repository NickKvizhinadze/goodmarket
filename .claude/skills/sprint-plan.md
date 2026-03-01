# Sprint Plan Skill

## Purpose
Map the full GoodMarket roadmap to 2-week sprints with a goal, concrete daily tasks, and explicit blockers per sprint.

## When to Use
- When asked to plan the project as sprints
- When starting a new sprint and needing a detailed task list
- When the roadmap phases need to be translated into schedulable work

---

## Sprint Conventions

- **Length:** 2 weeks per sprint
- **Goal:** One sentence — what is shippable or demonstrable at the end
- **Tasks:** Concrete enough to start immediately — file to create, endpoint to implement, class to write
- **Parallel work:** Phases 6 and 7 (Notifications, CMS) can run in parallel with Phases 4 and 5
- **Blockers:** Open decisions (ADR-004 file storage, ADR-013 mobile) must be resolved before their dependent sprint begins

---

## Step-by-Step Process

### Step 1 — Assess Current State
Before planning, run the `roadmap-status` skill to know what's already done. Sprints should start from the current state, not from zero.

```bash
find . -name "*.csproj" | grep -v obj | grep -v bin | sort
git log --oneline -10
```

### Step 2 — Read the Roadmap
Read `docs/roadmap.md` in full. Map each phase and sub-task to a sprint slot, respecting the dependency order in the Build Order Summary.

### Step 3 — Read Microservice Details
For phases involving new services, read `docs/microservices.md` for the full entity list, commands, queries, and events. Use this to estimate task count and split phases across sprints if needed.

### Step 4 — Split Large Phases
Some phases are too large for a single sprint. Rules for splitting:
- **Catalog (Phase 3):** Split into Domain + Infrastructure (Sprint A) and Use Cases + Endpoints (Sprint B)
- **Orders (Phase 5):** Split into Order lifecycle (Sprint A) and Payment integration (Sprint B)
- **Next.js (Phase 8):** Split into Auth + Layout (Sprint A), Product/Category pages (Sprint B), Cart + Checkout (Sprint C)

### Step 5 — Output the Sprint Schedule

For each sprint output:

```
## Sprint N — [Goal]
**Roadmap phase:** Phase X.Y
**Depends on:** Sprint N-1 complete / [specific prerequisite]
**Blocker:** [ADR-00X decision needed] or "None"

### Tasks
- [ ] [Concrete task — specific enough to act on immediately]
- [ ] [Concrete task]
- [ ] ...

### Definition of Done
- [ ] [Specific acceptance criteria]
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with endpoints)
- [ ] Integration tests passing
```

---

## Full Sprint Map (Reference)

Use this as a baseline. Adjust based on actual current state from `roadmap-status`.

| Sprint | Goal | Roadmap Phase |
|---|---|---|
| 1 | Foundation — Aspire, Gateway, Shared.Contracts | Phase 0 |
| 2 | Identity core — login, refresh token, logout | Phase 1 (part 1) |
| 3 | Identity complete — Google OAuth2, password reset, events | Phase 1 (part 2) |
| 4 | Media service + Catalog domain model | Phase 2 + Phase 3 start |
| 5 | Catalog — categories, brands, products CRUD | Phase 3 (part 1) |
| 6 | Catalog — queries, filtering, pagination, events | Phase 3 (part 2) |
| 7 | Cart service | Phase 4 |
| 8 | Orders — lifecycle + state machine | Phase 5 (part 1) |
| 9 | Orders — payment integration (BOG/TBC) | Phase 5 (part 2) |
| 10 | Notifications + CMS (parallel) | Phase 6 + 7 |
| 11 | Next.js — auth, layout, product/category pages | Phase 8 (part 1) |
| 12 | Next.js — cart, checkout, user account | Phase 8 (part 2) |
| 13 | Admin desktop app (C++) — framework decision + setup | Phase 9 start |
| 14 | Admin desktop app — core management screens | Phase 9 (part 2) |
| 15 | Mobile app — decision + foundation | Phase 10 (ADR-013 must be resolved first) |
| 16 | Reporting + Survey | Phase 11 |

---

## Blocker Flags

Always check these before finalising the sprint schedule:

| Sprint | Blocker |
|---|---|
| 4 (Media) | ADR-004: file storage provider not decided — must choose Azure Blob / AWS S3 / MinIO before Sprint 4 starts |
| 13 (Admin desktop) | ADR-012: C++ framework not decided — must choose before Sprint 13 starts |
| 15 (Mobile) | ADR-013: React Native vs Flutter not decided — must choose before Sprint 15 starts |
