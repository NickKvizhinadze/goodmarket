# PR Review Skill

## Purpose
Provides structured methodology for reviewing pull requests in this .NET project.

## How to Use
When asked to review a PR or code diff, follow this process:

### Step 1 — Understand the Change
- Read the PR description or commit messages for intent
- Identify what feature/bug this addresses
- Note the scope: how many files, which layers (API, service, data, jobs)

### Step 2 — Check the Diff
- Read each changed file carefully
- Cross-reference related files that weren't changed but may be affected
- Look for missing test coverage on changed logic

### Step 3 — Apply the Checklist
Run through the full review checklist from the pr-reviewer agent.

### Step 4 — Prioritize Findings
Categorize each finding:
- 🔴 **Must Fix** — blocks merge (bugs, security, broken behavior)
- 🟡 **Should Fix** — degrades quality but not blocking
- 🟢 **Suggestion** — nice to have, optional

### Step 5 — Write the Review
- Be specific: reference file names and line numbers when possible
- Explain *why* something is a problem, not just that it is
- Suggest the fix, don't just point out the issue
- Keep tone constructive — assume good intent

## .NET Specific Patterns to Watch

**EF Core Bulk Operations**
```csharp
// ❌ Bad — SaveChanges inside a loop causes N round trips
foreach (var item in items) {
    db.Add(item);
    await db.SaveChangesAsync();
}

// ✅ Good — batch outside the loop
db.AddRange(items);
await db.SaveChangesAsync();
```

---

## How to Invoke

Once files are in place, just tell Claude Code:
```
Use pr-reviewer to review the changes in this branch
```

Or paste a diff and say:
```
Review this PR using the pr-reviewer agent
```
