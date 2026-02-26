# Toll Fee Calculator - Assumptions & Decisions

## What I Built
A toll fee calculator that takes timestamps and returns the total fee. The calculator doesn't handle vehicle registry, database, or external services - those are separate concerns.

## Key Decisions

### 1. Rolling 60-Minute Windows
**The requirement says:** "A vehicle should only be charged once an hour"

**Two ways to interpret this:**
- Clock hours (8:00-9:00, 9:00-10:00)
- Rolling 60 minutes from first passage

**I chose rolling windows** because it's fairer - doesn't double-charge someone at 8:59 and 9:01.

**Edge case decision:** At exactly 60 minutes, I keep it in the same window (using `>` not `>=`). So 7:00 and 8:00 = one charge, not two.

### 2. Separation of Concerns
**Calculator:** Calculates fees from timestamps  
**Vehicle Service:** Checks if vehicle type is toll-free  
**Holiday Provider:** Checks if date is toll-free

Each service has one job. The API endpoint orchestrates them:
```
1. Check if vehicle is toll-free → return 0
2. Otherwise, calculate fee from passages
```

### 3. Multiple Days
The calculator handles timestamps from multiple days - groups by day, applies the 60 SEK cap per day, then sums them up.

**Why per-day cap matters:**
- Day 1: 80 SEK → capped at 60 SEK
- Day 2: 80 SEK → capped at 60 SEK
- Total: 120 SEK (not 60!)

### 4. Configuration & Implementation
**What's configurable:**
- Fee schedule (times and amounts)
- Daily cap and charge interval
- Toll-free vehicle types
- Currency

**What's hardcoded (intentionally):**
- 2026 Swedish holidays (would come from external API in production)
- Default fallback config if appsettings.json is missing

**What's separate:**
- Vehicle registry lookup (external service)
- Holiday calendar (external API in production)
- Payment/billing (not part of calculator)

---

## Testing
- Time-based fees and boundaries
- 60-minute rule with various intervals
- Daily cap scenarios
- Edge cases: empty list, single passage, unsorted times
- Weekend/holiday handling

---

## Questions I'd Ask in a Real Project
1. **60-minute rule:** Rolling or clock hours? What about exactly 60 min?
2. **Vehicle filtering:** Upstream or in calculator?
3. **Fee updates:** How often? What's the process?
4. **Multi-city support:** Different configs per city?
5. **Holidays:** API, database, or config file?
