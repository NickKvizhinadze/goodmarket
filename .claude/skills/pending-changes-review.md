# Pending Changes Review Skill

## Purpose
Structured methodology for reviewing local pending changes (staged, unstaged, unpushed commits) in this .NET project before they reach a PR.

## When to Use
- Before committing — catch WIP artifacts and bugs early
- Before pushing — validate nothing was missed
- During development — sanity check on a chunk of work in progress
- Before opening a PR — pre-flight check to avoid wasted review cycles

---

## Step-by-Step Process

### Step 1 — Collect the Diff
Run these in order to build a complete picture:

```bash
git status                    # overview of what files are affected
git diff HEAD                 # all pending changes (staged + unstaged)
git diff --cached             # staged-only changes
git log origin/HEAD..HEAD --oneline  # unpushed commits
```

If the diff is large, also read the full file for changed methods using the Read tool — diffs alone miss context.

### Step 2 — Identify Intent
Before reviewing quality, answer:
- What feature or bug does this address?
- Is this complete work or mid-flight?
- Which layers are touched (API, services, data, jobs, migrations)?

### Step 3 — WIP Scan (Fast Pass)
Quickly scan for things that should never be committed:

| Pattern | Why It's a Problem |
|---|---|
| `Console.WriteLine` / `Debug.Write` | Leaks to production logs |
| Hardcoded IDs, emails, passwords | Environment-specific or security risk |
| `// TODO` blocking correctness | Feature incomplete |
| Commented-out code blocks | Should be deleted, not committed |
| `throw new NotImplementedException()` | Will crash at runtime |
| `.Result` / `.Wait()` on Tasks | Deadlock risk in ASP.NET |

### Step 4 — Deep Review
Apply the full checklist from the `pending-changes-reviewer` agent.

Focus extra attention on:
- **EF Core changes** → always check if a migration is needed
- **New service/repository methods** → are they tested?

### Step 5 — Migration Check
If any entity model was changed, verify:

```bash
# Check if a migration was generated
ls Migrations/

# Or in EF CLI
dotnet ef migrations list
```

If models changed but no new migration file appears in the diff — flag it.

### Step 6 — Write the Review
Structure output as:

```
**What Changed**
[1-3 sentence summary of intent]

**WIP Issues** 🔴
[Things to fix before committing]

**Must Fix** 🔴
[Bugs, correctness, security]

**Should Fix** 🟡
[Quality, missing validation, edge cases]

**Suggestions** 🟢
[Optional improvements]

**Verdict**
[ ] Ready to commit
[ ] Needs cleanup first
[ ] Still in progress — not ready
```

---

## .NET Patterns to Catch

### Blocking Async Code
```csharp
// ❌ — Deadlock risk
var data = GetDataAsync().Result;

// ✅
var data = await GetDataAsync();
```

### SaveChanges Inside a Loop
```csharp
// ❌ — N round trips to DB
foreach (var item in items) {
    db.Items.Add(item);
    await db.SaveChangesAsync();
}

// ✅ — Single round trip
db.Items.AddRange(items);
await db.SaveChangesAsync();
```

### Missing CancellationToken
```csharp
// ❌ — Ignores cancellation
public async Task<List<Order>> GetOrdersAsync()
    => await db.Orders.ToListAsync();

// ✅
public async Task<List<Order>> GetOrdersAsync(CancellationToken ct = default)
    => await db.Orders.ToListAsync(ct);
```
