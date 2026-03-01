# Plan Phase Skill

## Purpose
Break down a specific roadmap phase into ordered, concrete implementation tasks ready to be worked on.

## When to Use
- When starting a new phase of the roadmap
- When asked "what do I need to build for X?"
- After `roadmap-status` identifies the next phase to work on

---

## Step-by-Step Process

### Step 1 — Read the Phase Definition
Read `docs/roadmap.md` for the target phase. Then read the relevant service detail in `docs/microservices.md`.

### Step 2 — Check Architecture Decisions
Read `docs/decisions.md`. Identify which ADRs apply to this phase and note any constraints they impose.

### Step 3 — Inspect Existing Code
If the service already has scaffolded files, read them:

```bash
find <ServiceName>/src -name "*.cs" | grep -v obj | sort
```

Understand what's already there before planning what to add.

### Step 4 — Check for Open Decisions
If the phase depends on a deferred decision (e.g. ADR-004 file storage for Media service, ADR-013 mobile tech), flag it before producing the plan. The decision must be made first.

### Step 5 — Produce the Task Breakdown

Order tasks by dependency — foundational things first.

```
## Phase X — [Name] Task Breakdown

**Prerequisites:** [anything that must exist before this phase can start]

**Open decisions needed:** [ADR numbers, or "none"]

### Tasks (in order)

1. [Task name]
   - What to build: [specific description]
   - Files to create/modify: [list]
   - Applies ADR: [ADR-00X — brief reason]

2. [Task name]
   ...

### Scaffold Command (if new service)
.\Create-Microservice.ps1 -MicroserviceName <Name>

### Definition of Done
- [ ] [specific acceptance criteria]
- [ ] Integration tests passing
- [ ] Registered in AppHost (if a new service)
- [ ] Endpoints reachable through the Gateway
```

Keep tasks granular enough to commit individually. Flag any task that touches an architectural boundary (e.g. a new RabbitMQ event — reference `docs/infrastructure.md` for the event table).
