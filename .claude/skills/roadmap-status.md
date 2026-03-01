# Roadmap Status Skill

## Purpose
Produce a clear snapshot of where the GoodMarket project stands against the planned roadmap — what's done, what's in progress, and what hasn't been started.

## When to Use
- At the start of a session to orient yourself
- Before planning the next piece of work
- When asked "where are we?" or "what's left to do?"

---

## Step-by-Step Process

### Step 1 — Read the Roadmap
Read `docs/roadmap.md` in full. Note every phase and sub-task.

### Step 2 — Inspect the Codebase

```bash
# All projects currently in the solution
find . -name "*.csproj" | grep -v "/obj/" | grep -v "/bin/" | sort

# All source files per existing service (repeat per service)
find Identity/src -name "*.cs" | grep -v obj | sort
find Shared/src -name "*.cs" | grep -v obj | sort

# Recent git activity
git log --oneline -15
```

Read key files in each existing service to assess completeness — not just whether a file exists, but whether it's implemented or scaffolded/empty.

### Step 3 — Map State to Roadmap

For each phase and sub-task in the roadmap, assign a status:

| Status | Meaning |
|---|---|
| Done | Fully implemented |
| In Progress | Scaffolded or partially implemented |
| Not Started | Doesn't exist yet |
| Blocked | Waiting on an open decision (note which ADR) |

### Step 4 — Output the Status Report

Structure the report as:

```
## Roadmap Status — [date]

### Done
- [specific items completed]

### In Progress
- [items partially implemented — note what's missing]

### Not Started
- Phase X — [name]
- Phase X — [name]

### Blocked / Open Decisions
- [item] — blocked on ADR-00X ([decision needed])

### Recommended Next Step
[One clear, specific recommendation for what to work on next, with reasoning]
```

Keep it factual — only mark something Done if the implementation is real, not just scaffolded.
